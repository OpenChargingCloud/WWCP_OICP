/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Linq;
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using Org.BouncyCastle.Bcpg.OpenPgp;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.CPO
{

    /// <summary>
    /// A WWCP wrapper for the OICP CPO Roaming client which maps
    /// WWCP data structures onto OICP data structures and vice versa.
    /// </summary>
    public class WWCPCPOAdapter : AWWCPCSOAdapter,
                                  ICSORoamingProvider,
                                  IEquatable <WWCPCPOAdapter>,
                                  IComparable<WWCPCPOAdapter>,
                                  IComparable
    {

        #region Data

        private        readonly  EVSE2EVSEDataRecordDelegate                            _EVSE2EVSEDataRecord;

        private        readonly  EVSEStatusUpdate2EVSEStatusRecordDelegate              _EVSEStatusUpdate2EVSEStatusRecord;

        private        readonly  WWCPChargeDetailRecord2ChargeDetailRecordDelegate      _WWCPChargeDetailRecord2OICPChargeDetailRecord;

        private        readonly  ChargingStationOperatorNameSelectorDelegate            _OperatorNameSelector;

        private static readonly  Regex                                                  pattern                      = new Regex(@"\s=\s");

        public  static readonly  ChargingStationOperatorNameSelectorDelegate            DefaultOperatorNameSelector  = I18N => I18N.FirstText();

        private readonly        List<ChargeDetailRecord>                                OICP_ChargeDetailRecords_Queue;

        protected readonly      SemaphoreSlim                                           FlushOICPChargeDetailRecordsLock      = new SemaphoreSlim(1, 1);

        public readonly static  TimeSpan                                                DefaultRequestTimeout  = TimeSpan.FromSeconds(30);

        #endregion

        #region Properties

        IId ISendAuthorizeStartStop.AuthId
            => Id;

        IId ISendChargeDetailRecords.Id
            => Id;

        IEnumerable<IId> ISendChargeDetailRecords.Ids
            => Ids.Cast<IId>();

        /// <summary>
        /// The wrapped CPO roaming object.
        /// </summary>
        public CPORoaming CPORoaming { get; }


        /// <summary>
        /// The CPO client.
        /// </summary>
        public CPOClient CPOClient
            => CPORoaming?.CPOClient;

        /// <summary>
        /// The CPO client logger.
        /// </summary>
        public CPOClient.CPOClientLogger ClientLogger
            => CPORoaming?.CPOClient?.Logger;


        /// <summary>
        /// The CPO server.
        /// </summary>
        public CPOServer CPOServer
            => CPORoaming?.CPOServer;

        /// <summary>
        /// The CPO server logger.
        /// </summary>
        public CPOServerLogger ServerLogger
            => CPORoaming?.CPOServerLogger;


        /// <summary>
        /// An optional default charging station operator identification.
        /// </summary>
        public Operator_Id  DefaultOperatorId     { get; }


        public WWCP.OperatorIdFormats DefaultOperatorIdFormat { get; }

        /// <summary>
        /// An optional default charging station operator name.
        /// </summary>
        public String       DefaultOperatorName   { get; }


        protected readonly CustomEVSEIdMapperDelegate CustomEVSEIdMapper;

        #endregion

        #region Events

        // Client logging...

        #region OnPushEVSEDataWWCPRequest/-Response

        /// <summary>
        /// An event fired whenever new EVSE data will be send upstream.
        /// </summary>
        public event OnPushEVSEDataWWCPRequestDelegate   OnPushEVSEDataWWCPRequest;

        /// <summary>
        /// An event fired whenever new EVSE data had been sent upstream.
        /// </summary>
        public event OnPushEVSEDataWWCPResponseDelegate  OnPushEVSEDataWWCPResponse;

        #endregion

        #region OnPushEVSEStatusWWCPRequest/-Response

        /// <summary>
        /// An event fired whenever new EVSE status will be send upstream.
        /// </summary>
        public event OnPushEVSEStatusWWCPRequestDelegate   OnPushEVSEStatusWWCPRequest;

        /// <summary>
        /// An event fired whenever new EVSE status had been sent upstream.
        /// </summary>
        public event OnPushEVSEStatusWWCPResponseDelegate  OnPushEVSEStatusWWCPResponse;

        #endregion


        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate                  OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate                 OnAuthorizeStartResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given EVSE.
        /// </summary>
        public event OnAuthorizeEVSEStartRequestDelegate              OnAuthorizeEVSEStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given EVSE.
        /// </summary>
        public event OnAuthorizeEVSEStartResponseDelegate             OnAuthorizeEVSEStartResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given charging station.
        /// </summary>
        public event OnAuthorizeChargingStationStartRequestDelegate   OnAuthorizeChargingStationStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given charging station.
        /// </summary>
        public event OnAuthorizeChargingStationStartResponseDelegate  OnAuthorizeChargingStationStartResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given charging pool.
        /// </summary>
        public event OnAuthorizeChargingPoolStartRequestDelegate      OnAuthorizeChargingPoolStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given charging pool.
        /// </summary>
        public event OnAuthorizeChargingPoolStartResponseDelegate     OnAuthorizeChargingPoolStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate                  OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate                 OnAuthorizeStopResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given EVSE.
        /// </summary>
        public event OnAuthorizeEVSEStopRequestDelegate              OnAuthorizeEVSEStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given EVSE.
        /// </summary>
        public event OnAuthorizeEVSEStopResponseDelegate             OnAuthorizeEVSEStopResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given charging station.
        /// </summary>
        public event OnAuthorizeChargingStationStopRequestDelegate   OnAuthorizeChargingStationStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given charging station.
        /// </summary>
        public event OnAuthorizeChargingStationStopResponseDelegate  OnAuthorizeChargingStationStopResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given charging pool.
        /// </summary>
        public event OnAuthorizeChargingPoolStopRequestDelegate      OnAuthorizeChargingPoolStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given charging pool.
        /// </summary>
        public event OnAuthorizeChargingPoolStopResponseDelegate     OnAuthorizeChargingPoolStopResponse;

        #endregion

        #region OnSendCDRsRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record was enqueued for later sending upstream.
        /// </summary>
        public event OnSendCDRRequestDelegate   OnEnqueueSendCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record will be send upstream.
        /// </summary>
        public event OnSendCDRRequestDelegate   OnSendCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record had been sent upstream.
        /// </summary>
        public event OnSendCDRResponseDelegate  OnSendCDRsResponse;

        #endregion



        // OICP CPO Client SOAP logging

        #region OnPushEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a request pushing EVSE data records will be send.
        /// </summary>
        public event OnPushEVSEDataRequestDelegate OnPushEVSEDataRequest
        {

            add
            {
                CPOClient.OnPushEVSEDataRequest += value;
            }

            remove
            {
                CPOClient.OnPushEVSEDataRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request pushing EVSE data records will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPushEVSEDataSOAPRequest
        {

            add
            {
                CPOClient.OnPushEVSEDataSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnPushEVSEDataSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a push EVSE data records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPushEVSEDataSOAPResponse
        {

            add
            {
                CPOClient.OnPushEVSEDataSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnPushEVSEDataSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever EVSE data records had been sent upstream.
        /// </summary>
        public event OnPushEVSEDataResponseDelegate OnPushEVSEDataResponse
        {

            add
            {
                CPOClient.OnPushEVSEDataResponse += value;
            }

            remove
            {
                CPOClient.OnPushEVSEDataResponse -= value;
            }

        }

        #endregion

        #region OnPushEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request pushing EVSE status records will be send.
        /// </summary>
        public event OnPushEVSEStatusRequestDelegate OnPushEVSEStatusRequest
        {

            add
            {
                CPOClient.OnPushEVSEStatusRequest += value;
            }

            remove
            {
                CPOClient.OnPushEVSEStatusRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request pushing EVSE status records will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPushEVSEStatusSOAPRequest
        {

            add
            {
                CPOClient.OnPushEVSEStatusSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnPushEVSEStatusSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a push EVSE status records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPushEVSEStatusSOAPResponse
        {

            add
            {
                CPOClient.OnPushEVSEStatusSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnPushEVSEStatusSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever EVSE status records had been sent upstream.
        /// </summary>
        public event OnPushEVSEStatusResponseDelegate OnPushEVSEStatusResponse
        {

            add
            {
                CPOClient.OnPushEVSEStatusResponse += value;
            }

            remove
            {
                CPOClient.OnPushEVSEStatusResponse -= value;
            }

        }

        #endregion


        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start request will be send.
        /// </summary>
        public event OnAuthorizeStartRequestHandler OnOICPuthorizeStartRequest
        {

            add
            {
                CPOClient.OnAuthorizeStartRequest += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStartRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever an authorize start SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnAuthorizeStartSOAPRequest
        {

            add
            {
                CPOClient.OnAuthorizeStartSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStartSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an authorize start SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnAuthorizeStartSOAPResponse
        {

            add
            {
                CPOClient.OnAuthorizeStartSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStartSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever an authorize start request was sent.
        /// </summary>
        public event OnAuthorizeStartResponseHandler OnOICPAuthorizeStartResponse
        {

            add
            {
                CPOClient.OnAuthorizeStartResponse += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStartResponse -= value;
            }

        }

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize stop request will be send.
        /// </summary>
        public event OnAuthorizeStopRequestHandler OnOICPAuthorizeStopRequest
        {

            add
            {
                CPOClient.OnAuthorizeStopRequest += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStopRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever an authorize stop SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnAuthorizeStopSOAPRequest
        {

            add
            {
                CPOClient.OnAuthorizeStopSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStopSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an authorize stop SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnAuthorizeStopSOAPResponse
        {

            add
            {
                CPOClient.OnAuthorizeStopSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStopSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever an authorize start request was sent.
        /// </summary>
        public event OnAuthorizeStopResponseHandler OnOICPAuthorizeStopResponse
        {

            add
            {
                CPOClient.OnAuthorizeStopResponse += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStopResponse -= value;
            }

        }

        #endregion

        #region OnSendChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record will be send.
        /// </summary>
        public event OnSendChargeDetailRecordRequestHandler OnSendChargeDetailRecordRequest
        {

            add
            {
                CPOClient.OnSendChargeDetailRecordRequest += value;
            }

            remove
            {
                CPOClient.OnSendChargeDetailRecordRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a charge detail record will be send via SOAP.
        /// </summary>
        public event ClientRequestLogHandler OnSendChargeDetailRecordSOAPRequest
        {

            add
            {
                CPOClient.OnSendChargeDetailRecordSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnSendChargeDetailRecordSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response to a sent charge detail record had been received.
        /// </summary>
        public event ClientResponseLogHandler OnSendChargeDetailRecordSOAPResponse
        {

            add
            {
                CPOClient.OnSendChargeDetailRecordSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnSendChargeDetailRecordSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a sent charge detail record had been received.
        /// </summary>
        public event OnSendChargeDetailRecordResponseHandler OnSendChargeDetailRecordResponse
        {

            add
            {
                CPOClient.OnSendChargeDetailRecordResponse += value;
            }

            remove
            {
                CPOClient.OnSendChargeDetailRecordResponse -= value;
            }

        }

        #endregion


        #region OnPullAuthenticationDataRequest/-Response

        /// <summary>
        /// An event fired whenever a request pulling authentication data will be send.
        /// </summary>
        public event OnPullAuthenticationDataRequestHandler OnPullAuthenticationDataRequest
        {

            add
            {
                CPOClient.OnPullAuthenticationDataRequest += value;
            }

            remove
            {
                CPOClient.OnPullAuthenticationDataRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request pulling authentication data will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPullAuthenticationDataSOAPRequest
        {

            add
            {
                CPOClient.OnPullAuthenticationDataSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnPullAuthenticationDataSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a pull authentication data SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPullAuthenticationDataSOAPResponse
        {

            add
            {
                CPOClient.OnPullAuthenticationDataSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnPullAuthenticationDataSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a pull authentication data request was received.
        /// </summary>
        public event OnPullAuthenticationDataResponseHandler OnPullAuthenticationDataResponse
        {

            add
            {
                CPOClient.OnPullAuthenticationDataResponse += value;
            }

            remove
            {
                CPOClient.OnPullAuthenticationDataResponse -= value;
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        #region WWCPCPOAdapter(Id, Name, RoamingNetwork, CPORoaming, EVSE2EVSEDataRecord = null)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for Charging Station Operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="Description">An optional (multi-language) description of the charging station operator roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="CPORoaming">A OICP CPO roaming object to be mapped to WWCP.</param>
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// 
        /// <param name="DefaultOperator">An optional Charging Station Operator, which will be copied into the main OperatorID-section of the OICP SOAP request.</param>
        /// <param name="OperatorNameSelector">An optional delegate to select an Charging Station Operator name, which will be copied into the OperatorName-section of the OICP SOAP request.</param>
        /// 
        /// <param name="IncludeEVSEIds">Only include the EVSE matching the given delegate.</param>
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="CustomEVSEIdMapper">A delegate to customize the mapping of EVSE identifications.</param>
        /// 
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// <param name="CDRCheckEvery">The charge detail record intervall.</param>
        /// 
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        /// 
        /// <param name="PublicKeyRing">The public key ring of the entity.</param>
        /// <param name="SecretKeyRing">The secrect key ring of the entity.</param>
        /// <param name="DNSClient">The attached DNS service.</param>
        public WWCPCPOAdapter(CSORoamingProvider_Id                              Id,
                              I18NString                                         Name,
                              I18NString                                         Description,
                              RoamingNetwork                                     RoamingNetwork,

                              CPORoaming                                         CPORoaming,
                              EVSE2EVSEDataRecordDelegate                        EVSE2EVSEDataRecord                             = null,
                              EVSEStatusUpdate2EVSEStatusRecordDelegate          EVSEStatusUpdate2EVSEStatusRecord               = null,
                              WWCPChargeDetailRecord2ChargeDetailRecordDelegate  WWCPChargeDetailRecord2OICPChargeDetailRecord   = null,

                              ChargingStationOperator                            DefaultOperator                                 = null,
                              WWCP.OperatorIdFormats                             DefaultOperatorIdFormat                         = WWCP.OperatorIdFormats.ISO_STAR,
                              ChargingStationOperatorNameSelectorDelegate        OperatorNameSelector                            = null,

                              IncludeEVSEIdDelegate                              IncludeEVSEIds                                  = null,
                              IncludeEVSEDelegate                                IncludeEVSEs                                    = null,
                              CustomEVSEIdMapperDelegate                         CustomEVSEIdMapper                              = null,

                              TimeSpan?                                          ServiceCheckEvery                               = null,
                              TimeSpan?                                          StatusCheckEvery                                = null,
                              TimeSpan?                                          CDRCheckEvery                                   = null,

                              Boolean                                            DisablePushData                                 = false,
                              Boolean                                            DisablePushStatus                               = false,
                              Boolean                                            DisableAuthentication                           = false,
                              Boolean                                            DisableSendChargeDetailRecords                  = false,

                              PgpPublicKeyRing                                   PublicKeyRing                                   = null,
                              PgpSecretKeyRing                                   SecretKeyRing                                   = null,
                              DNSClient                                          DNSClient                                       = null)

            : base(Id,
                   Name,
                   Description,
                   RoamingNetwork,

                   IncludeEVSEIds,
                   IncludeEVSEs,
                   //CustomEVSEIdMapper,

                   ServiceCheckEvery,
                   StatusCheckEvery,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushStatus,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords,

                   PublicKeyRing,
                   SecretKeyRing,
                   DNSClient ?? CPORoaming?.DNSClient)

        {

            #region Initial checks

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),        "The given roaming provider name must not be null or empty!");

            if (CPORoaming == null)
                throw new ArgumentNullException(nameof(CPORoaming),  "The given OICP CPO Roaming object must not be null!");

            #endregion

            this.CPORoaming                                       = CPORoaming;
            this._EVSE2EVSEDataRecord                             = EVSE2EVSEDataRecord;
            this._EVSEStatusUpdate2EVSEStatusRecord               = EVSEStatusUpdate2EVSEStatusRecord;
            this._WWCPChargeDetailRecord2OICPChargeDetailRecord   = WWCPChargeDetailRecord2OICPChargeDetailRecord;

            this.DefaultOperatorId                                = DefaultOperator.Id.ToOICP(DefaultOperatorIdFormat);
            this.DefaultOperatorIdFormat                          = DefaultOperatorIdFormat;
            this.DefaultOperatorName                              = DefaultOperatorNameSelector(DefaultOperator.Name);
            this._OperatorNameSelector                            = OperatorNameSelector;

            this.CustomEVSEIdMapper                               = CustomEVSEIdMapper;

            this.OICP_ChargeDetailRecords_Queue                   = new List<ChargeDetailRecord>();


            // Link incoming OICP events...

            #region OnAuthorizeRemoteReservationStart

            this.CPORoaming.OnAuthorizeRemoteReservationStart += async (Timestamp,
                                                                        Sender,
                                                                        Request) => {


                #region Request transformation

                TimeSpan?           Duration           = null;
                DateTime?           StartTime          = null;
                PartnerProduct_Id?  PartnerProductId   = Request.PartnerProductId;

                // Analyse the ChargingProductId field and apply the found key/value-pairs
                if (PartnerProductId != null && PartnerProductId.ToString().IsNotNullOrEmpty())
                {

                    var Elements = pattern.Replace(PartnerProductId.ToString(), "=").Split('|').ToArray();

                    if (Elements.Length > 0)
                    {

                        var DurationText = Elements.FirstOrDefault(element => element.StartsWith("D=", StringComparison.InvariantCulture));
                        if (DurationText.IsNotNullOrEmpty())
                        {

                            DurationText = DurationText.Substring(2);

                            if (DurationText.EndsWith("sec", StringComparison.InvariantCulture))
                                Duration = TimeSpan.FromSeconds(UInt32.Parse(DurationText.Substring(0, DurationText.Length - 3)));

                            if (DurationText.EndsWith("min", StringComparison.InvariantCulture))
                                Duration = TimeSpan.FromMinutes(UInt32.Parse(DurationText.Substring(0, DurationText.Length - 3)));

                        }

                        var PartnerProductText = Elements.FirstOrDefault(element => element.StartsWith("P=", StringComparison.InvariantCulture));
                        if (PartnerProductText.IsNotNullOrEmpty())
                        {
                            PartnerProductId = PartnerProduct_Id.Parse(PartnerProductText.Substring(2));
                        }

                        var StartTimeText = Elements.FirstOrDefault(element => element.StartsWith("S=", StringComparison.InvariantCulture));
                        if (StartTimeText.IsNotNullOrEmpty())
                        {
                            StartTime = DateTime.Parse(StartTimeText.Substring(2));
                        }

                    }

                }

                #endregion

                var response = await RoamingNetwork.
                                         Reserve(Request.EVSEId.ToWWCP().Value,
                                                 StartTime:          StartTime,
                                                 Duration:           Duration,

                                                 // Always create a reservation identification usable for OICP!
                                                 ReservationId:      ChargingReservation_Id.Parse(
                                                                         Request.EVSEId.OperatorId.ToWWCP().Value,
                                                                         Request.SessionId.HasValue
                                                                             ? Request.SessionId.ToString()
                                                                             : Session_Id.NewRandom.ToString()
                                                                     ),

                                                 ProviderId:         Request.ProviderId.      ToWWCP(),
                                                 Identification:              Request.Identification.  ToWWCP(),
                                                 ChargingProduct:    PartnerProductId.HasValue
                                                                         ? new ChargingProduct(PartnerProductId.Value.ToWWCP())
                                                                         : null,

                                                 eMAIds:             Request?.Identification?.RemoteIdentification           != null &&
                                                                     Request?.Identification?.RemoteIdentification?.ToWWCP() != null
                                                                         ? new eMobilityAccount_Id[] {
                                                                               Request.Identification.RemoteIdentification.ToWWCP().Value
                                                                           }
                                                                         : null,

                                                 Timestamp:          Request.Timestamp,
                                                 CancellationToken:  Request.CancellationToken,
                                                 EventTrackingId:    Request.EventTrackingId,
                                                 RequestTimeout:     Request.RequestTimeout).
                                         ConfigureAwait(false);

                #region Response mapping

                if (response != null)
                {
                    switch (response.Result)
                    {

                        case ReservationResultType.Success:
                            return Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.Success(
                                       Request,
                                       response.Reservation != null
                                           ? Session_Id.Parse(response.Reservation.Id.Suffix)
                                           : new Session_Id?(),

                                       StatusCodeDescription :    "Reservation successful!",
                                       StatusCodeAdditionalInfo:  response.Reservation != null ? "ReservationId: " + response.Reservation.Id : null

                                   );

                        case ReservationResultType.InvalidCredentials:
                            return Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.SessionIsInvalid(
                                       Request,
                                       SessionId: Session_Id.Parse(response.Reservation.Id.ToString())
                                   );

                        case ReservationResultType.Timeout:
                        case ReservationResultType.CommunicationError:
                            return Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.CommunicationToEVSEFailed(Request);

                        case ReservationResultType.AlreadyReserved:
                            return Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.EVSEAlreadyReserved(Request);

                        case ReservationResultType.AlreadyInUse:
                            return Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.EVSEAlreadyInUse_WrongToken(Request);

                        case ReservationResultType.UnknownEVSE:
                            return Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.UnknownEVSEID(Request);

                        case ReservationResultType.OutOfService:
                            return Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.EVSEOutOfService(Request);

                    }
                }

                return Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.ServiceNotAvailable(
                           Request,
                           SessionId: Session_Id.Parse(response.Reservation.Id.ToString())
                       );

                #endregion

            };

            #endregion

            #region OnAuthorizeRemoteReservationStop

            this.CPORoaming.OnAuthorizeRemoteReservationStop += async (Timestamp,
                                                                       Sender,
                                                                       Request) => {

                var response = await RoamingNetwork.
                                         CancelReservation(ChargingReservation_Id.Parse(
                                                               Request.EVSEId.OperatorId.ToWWCP().Value,
                                                               Request.SessionId.ToString()
                                                           ),
                                                           ChargingReservationCancellationReason.Deleted,
                                                           Request.ProviderId.ToWWCP(),
                                                           Request.EVSEId.    ToWWCP(),

                                                           Request.Timestamp,
                                                           Request.CancellationToken,
                                                           Request.EventTrackingId,
                                                           Request.RequestTimeout).
                                         ConfigureAwait(false);

                #region Response mapping

                if (response != null)
                {
                    switch (response.Result)
                    {

                        case CancelReservationResults.Success:
                            return Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.Success(
                                       Request,
                                       StatusCodeDescription: "Reservation deleted!"
                                   );

                        case CancelReservationResults.UnknownReservationId:
                            return Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.SessionIsInvalid(
                                       Request,
                                       SessionId: Request.SessionId
                                   );

                        case CancelReservationResults.Offline:
                        case CancelReservationResults.Timeout:
                        case CancelReservationResults.CommunicationError:
                            return Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.CommunicationToEVSEFailed(Request);

                        case CancelReservationResults.UnknownEVSE:
                            return Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.UnknownEVSEID(Request);

                        case CancelReservationResults.OutOfService:
                            return Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.EVSEOutOfService(Request);

                    }
                }

                return Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.ServiceNotAvailable(
                           Request,
                           SessionId: Request.SessionId
                       );

                #endregion

            };

            #endregion


            #region OnAuthorizeRemoteStart

            this.CPORoaming.OnAuthorizeRemoteStart += async (Timestamp,
                                                             Sender,
                                                             Request) => {

                #region Request mapping

                ChargingReservation_Id? ReservationId      = null;
                TimeSpan?               MinDuration        = null;
                Single?                 PlannedEnergy      = null;
                ChargingProduct_Id?     ProductId          = ChargingProduct_Id.Parse("AC1");
                ChargingProduct         ChargingProduct    = null;
                PartnerProduct_Id?      PartnerProductId   = Request.PartnerProductId;

                if (PartnerProductId != null && PartnerProductId.ToString().IsNotNullOrEmpty())
                {

                    // The PartnerProductId is a simple string...
                    if (!PartnerProductId.Value.ToString().Contains("="))
                    {
                        ChargingProduct = new ChargingProduct(
                                              ChargingProduct_Id.Parse(PartnerProductId.Value.ToString())
                                          );
                    }

                    else
                    {

                        var ProductIdElements = PartnerProductId.ToString().DoubleSplit('|', '=');

                        if (ProductIdElements.Any())
                        {

                            if (ProductIdElements.ContainsKey("R") &&
                                ChargingReservation_Id.TryParse(Request.EVSEId.OperatorId.ToWWCP().Value,
                                                                ProductIdElements["R"],
                                                                out ChargingReservation_Id _ReservationId))
                                ReservationId = _ReservationId;


                            if (ProductIdElements.ContainsKey("D"))
                            {

                                var MinDurationText = ProductIdElements["D"];

                                if (MinDurationText.EndsWith("sec", StringComparison.InvariantCulture))
                                    MinDuration = TimeSpan.FromSeconds(UInt32.Parse(MinDurationText.Substring(0, MinDurationText.Length - 3)));

                                if (MinDurationText.EndsWith("min", StringComparison.InvariantCulture))
                                    MinDuration = TimeSpan.FromMinutes(UInt32.Parse(MinDurationText.Substring(0, MinDurationText.Length - 3)));

                            }


                            if (ProductIdElements.ContainsKey("E") &&
                                Single.TryParse(ProductIdElements["E"], out Single _PlannedEnergy))
                                PlannedEnergy = _PlannedEnergy;


                            if (ProductIdElements.ContainsKey("P") &&
                                ChargingProduct_Id.TryParse(ProductIdElements["P"], out ChargingProduct_Id _ProductId))
                                ProductId = _ProductId;


                            ChargingProduct = new ChargingProduct(
                                                      ProductId.Value,
                                                      MinDuration
                                                  );

                        }

                    }

                }

                #endregion

                var response = await RoamingNetwork.
                                         RemoteStart(Request.EVSEId.    ToWWCP().Value,
                                                     ChargingProduct,
                                                     ReservationId,
                                                     Request.SessionId. ToWWCP(),
                                                     Request.ProviderId.ToWWCP(),
                                                     Request.EVCOId.    ToWWCP(),
                                                     this,

                                                     Request.Timestamp,
                                                     Request.CancellationToken,
                                                     Request.EventTrackingId,
                                                     Request.RequestTimeout).
                                         ConfigureAwait(false);

                #region Response mapping

                if (response != null)
                {
                    switch (response.Result)
                    {

                        case RemoteStartEVSEResultType.Success:
                            return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.Success(
                                       Request,
                                       response.Session.Id.ToOICP(),
                                       StatusCodeDescription: "Ready to charge!"
                                   );

                        case RemoteStartEVSEResultType.InvalidSessionId:
                            return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.SessionIsInvalid(
                                       Request,
                                       SessionId: Request.SessionId
                                   );

                        case RemoteStartEVSEResultType.InvalidCredentials:
                            return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.NoValidContract(Request);

                        case RemoteStartEVSEResultType.Offline:
                        case RemoteStartEVSEResultType.Timeout:
                        case RemoteStartEVSEResultType.CommunicationError:
                            return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.CommunicationToEVSEFailed(Request);

                        case RemoteStartEVSEResultType.Reserved:
                            return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.EVSEAlreadyReserved(Request);

                        case RemoteStartEVSEResultType.AlreadyInUse:
                            return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.EVSEAlreadyInUse_WrongToken(Request);

                        case RemoteStartEVSEResultType.UnknownEVSE:
                            return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.UnknownEVSEID(Request);

                        case RemoteStartEVSEResultType.OutOfService:
                            return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.EVSEOutOfService(Request);

                    }
                }

                return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.ServiceNotAvailable(
                           Request,
                           SessionId: Request.SessionId
                       );

                #endregion

            };

            #endregion

            #region OnAuthorizeRemoteStop

            this.CPORoaming.OnAuthorizeRemoteStop += async (Timestamp,
                                                            Sender,
                                                            Request) => {

                var response = await RoamingNetwork.
                                         RemoteStop(Request.EVSEId.   ToWWCP().Value,
                                                    Request.SessionId.ToWWCP(),
                                                    ReservationHandling.Close,
                                                    Request.ProviderId.ToWWCP(),
                                                    null,

                                                    Request.Timestamp,
                                                    Request.CancellationToken,
                                                    Request.EventTrackingId,
                                                    Request.RequestTimeout).
                                         ConfigureAwait(false);

                #region Response mapping

                if (response != null)
                {
                    switch (response.Result)
                    {

                        case RemoteStopEVSEResultType.Success:
                            return Acknowledgement<EMP.AuthorizeRemoteStopRequest>.Success(
                                       Request,
                                       response.SessionId.ToOICP(),
                                       StatusCodeDescription: "Ready to stop charging!"
                                   );

                        case RemoteStopEVSEResultType.InvalidSessionId:
                            return Acknowledgement<EMP.AuthorizeRemoteStopRequest>.SessionIsInvalid(
                                       Request,
                                       SessionId: Request.SessionId
                                   );

                        case RemoteStopEVSEResultType.Offline:
                        case RemoteStopEVSEResultType.Timeout:
                        case RemoteStopEVSEResultType.CommunicationError:
                            return Acknowledgement<EMP.AuthorizeRemoteStopRequest>.CommunicationToEVSEFailed(Request);

                        case RemoteStopEVSEResultType.UnknownEVSE:
                            return Acknowledgement<EMP.AuthorizeRemoteStopRequest>.UnknownEVSEID(Request);

                        case RemoteStopEVSEResultType.OutOfService:
                            return Acknowledgement<EMP.AuthorizeRemoteStopRequest>.EVSEOutOfService(Request);

                    }
                }

                return Acknowledgement<EMP.AuthorizeRemoteStopRequest>.ServiceNotAvailable(
                           Request,
                           SessionId: Request.SessionId
                       );

                #endregion

            };

            #endregion

        }

        #endregion

        #region WWCPCPOAdapter(Id, Name, RoamingNetwork, CPOClient, CPOServer, EVSEDataRecordProcessing = null)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for Charging Station Operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="CPOClient">An OICP CPO client.</param>
        /// <param name="CPOServer">An OICP CPO sever.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// 
        /// <param name="IncludeEVSEIds">Only include the EVSE matching the given delegate.</param>
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="CustomEVSEIdMapper">A delegate to customize the mapping of EVSE identifications.</param>
        /// 
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// 
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        /// 
        /// <param name="PublicKeyRing">The public key ring of the entity.</param>
        /// <param name="SecretKeyRing">The secrect key ring of the entity.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public WWCPCPOAdapter(CSORoamingProvider_Id                              Id,
                              I18NString                                         Name,
                              I18NString                                         Description,
                              RoamingNetwork                                     RoamingNetwork,

                              CPOClient                                          CPOClient,
                              CPOServer                                          CPOServer,
                              String                                             ServerLoggingContext                            = CPOServerLogger.DefaultContext,
                              LogfileCreatorDelegate                             LogfileCreator                                  = null,

                              EVSE2EVSEDataRecordDelegate                        EVSE2EVSEDataRecord                             = null,
                              EVSEStatusUpdate2EVSEStatusRecordDelegate          EVSEStatusUpdate2EVSEStatusRecord               = null,
                              WWCPChargeDetailRecord2ChargeDetailRecordDelegate  WWCPChargeDetailRecord2OICPChargeDetailRecord   = null,

                              ChargingStationOperator                            DefaultOperator                                 = null,
                              WWCP.OperatorIdFormats                             DefaultOperatorIdFormat                         = WWCP.OperatorIdFormats.ISO_STAR,
                              ChargingStationOperatorNameSelectorDelegate        OperatorNameSelector                            = null,

                              IncludeEVSEIdDelegate                              IncludeEVSEIds                                  = null,
                              IncludeEVSEDelegate                                IncludeEVSEs                                    = null,
                              CustomEVSEIdMapperDelegate                         CustomEVSEIdMapper                              = null,

                              TimeSpan?                                          ServiceCheckEvery                               = null,
                              TimeSpan?                                          StatusCheckEvery                                = null,
                              TimeSpan?                                          CDRCheckEvery                                   = null,

                              Boolean                                            DisablePushData                                 = false,
                              Boolean                                            DisablePushStatus                               = false,
                              Boolean                                            DisableAuthentication                           = false,
                              Boolean                                            DisableSendChargeDetailRecords                  = false,

                              PgpPublicKeyRing                                   PublicKeyRing                                   = null,
                              PgpSecretKeyRing                                   SecretKeyRing                                   = null,
                              DNSClient                                          DNSClient                                       = null)

            : this(Id,
                   Name,
                   Description,
                   RoamingNetwork,

                   new CPORoaming(CPOClient,
                                  CPOServer,
                                  ServerLoggingContext,
                                  LogfileCreator),

                   EVSE2EVSEDataRecord,
                   EVSEStatusUpdate2EVSEStatusRecord,
                   WWCPChargeDetailRecord2OICPChargeDetailRecord,

                   DefaultOperator,
                   DefaultOperatorIdFormat,
                   OperatorNameSelector,

                   IncludeEVSEIds,
                   IncludeEVSEs,
                   CustomEVSEIdMapper,

                   ServiceCheckEvery,
                   StatusCheckEvery,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushStatus,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords,

                   PublicKeyRing,
                   SecretKeyRing,
                   DNSClient ?? CPOServer?.DNSClient)

        { }

        #endregion

        #region WWCPCPOAdapter(Id, Name, RoamingNetwork, RemoteHostName, ...)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for Charging Station Operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="Description">An optional (multi-language) description of the charging station operator roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="RemoteHostname">The hostname of the remote OICP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerContentType">An optional HTTP content type to use.</param>
        /// <param name="ServerRegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// 
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// 
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        /// 
        /// <param name="PublicKeyRing">The public key ring of the entity.</param>
        /// <param name="SecretKeyRing">The secrect key ring of the entity.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public WWCPCPOAdapter(CSORoamingProvider_Id                              Id,
                              I18NString                                         Name,
                              I18NString                                         Description,
                              RoamingNetwork                                     RoamingNetwork,

                              String                                             RemoteHostname,
                              IPPort?                                            RemoteTCPPort                                   = null,
                              RemoteCertificateValidationCallback                RemoteCertificateValidator                      = null,
                              LocalCertificateSelectionCallback                  ClientCertificateSelector                       = null,
                              String                                             RemoteHTTPVirtualHost                           = null,
                              HTTPURI?                                           URIPrefix                                       = null,
                              String                                             EVSEDataURI                                     = CPOClient.DefaultEVSEDataURI,
                              String                                             EVSEStatusURI                                   = CPOClient.DefaultEVSEStatusURI,
                              String                                             AuthorizationURI                                = CPOClient.DefaultAuthorizationURI,
                              String                                             AuthenticationDataURI                           = CPOClient.DefaultAuthenticationDataURI,
                              String                                             HTTPUserAgent                                   = CPOClient.DefaultHTTPUserAgent,
                              TimeSpan?                                          RequestTimeout                                  = null,
                              Byte?                                              MaxNumberOfRetries                              = CPOClient.DefaultMaxNumberOfRetries,

                              String                                             ServerName                                      = CPOServer.DefaultHTTPServerName,
                              String                                             ServiceId                                       = null,
                              IPPort?                                            ServerTCPPort                                   = null,
                              HTTPURI?                                           ServerURIPrefix                                 = null,
                              String                                             ServerAuthorizationURI                          = CPOServer.DefaultAuthorizationURI,
                              String                                             ServerReservationURI                            = CPOServer.DefaultReservationURI,
                              HTTPContentType                                    ServerContentType                               = null,
                              Boolean                                            ServerRegisterHTTPRootService                   = true,
                              Boolean                                            ServerAutoStart                                 = false,

                              String                                             ClientLoggingContext                            = CPOClient.CPOClientLogger.DefaultContext,
                              String                                             ServerLoggingContext                            = CPOServerLogger.DefaultContext,
                              LogfileCreatorDelegate                             LogfileCreator                                  = null,

                              EVSE2EVSEDataRecordDelegate                        EVSE2EVSEDataRecord                             = null,
                              EVSEStatusUpdate2EVSEStatusRecordDelegate          EVSEStatusUpdate2EVSEStatusRecord               = null,
                              WWCPChargeDetailRecord2ChargeDetailRecordDelegate  WWCPChargeDetailRecord2OICPChargeDetailRecord   = null,

                              ChargingStationOperator                            DefaultOperator                                 = null,
                              WWCP.OperatorIdFormats                             DefaultOperatorIdFormat                         = WWCP.OperatorIdFormats.ISO_STAR,
                              ChargingStationOperatorNameSelectorDelegate        OperatorNameSelector                            = null,

                              IncludeEVSEIdDelegate                              IncludeEVSEIds                                  = null,
                              IncludeEVSEDelegate                                IncludeEVSEs                                    = null,
                              CustomEVSEIdMapperDelegate                         CustomEVSEIdMapper                              = null,

                              TimeSpan?                                          ServiceCheckEvery                               = null,
                              TimeSpan?                                          StatusCheckEvery                                = null,
                              TimeSpan?                                          CDRCheckEvery                                   = null,

                              Boolean                                            DisablePushData                                 = false,
                              Boolean                                            DisablePushStatus                               = false,
                              Boolean                                            DisableAuthentication                           = false,
                              Boolean                                            DisableSendChargeDetailRecords                  = false,

                              PgpPublicKeyRing                                   PublicKeyRing                                   = null,
                              PgpSecretKeyRing                                   SecretKeyRing                                   = null,
                              DNSClient                                          DNSClient                                       = null)

            : this(Id,
                   Name,
                   Description,
                   RoamingNetwork,

                   new CPORoaming(Id.ToString(),
                                  RemoteHostname,
                                  RemoteTCPPort,
                                  RemoteCertificateValidator,
                                  ClientCertificateSelector,
                                  RemoteHTTPVirtualHost,
                                  URIPrefix ?? CPOClient.DefaultURIPrefix,
                                  EVSEDataURI,
                                  EVSEStatusURI,
                                  AuthorizationURI,
                                  AuthenticationDataURI,
                                  HTTPUserAgent,
                                  RequestTimeout,
                                  MaxNumberOfRetries,

                                  ServerName,
                                  ServiceId,
                                  ServerTCPPort,
                                  ServerURIPrefix ?? CPOServer.DefaultURIPrefix,
                                  ServerAuthorizationURI,
                                  ServerReservationURI,
                                  ServerContentType,
                                  ServerRegisterHTTPRootService,
                                  false,

                                  ClientLoggingContext,
                                  ServerLoggingContext,
                                  LogfileCreator,

                                  DNSClient),

                   EVSE2EVSEDataRecord,
                   EVSEStatusUpdate2EVSEStatusRecord,
                   WWCPChargeDetailRecord2OICPChargeDetailRecord,

                   DefaultOperator,
                   DefaultOperatorIdFormat,
                   OperatorNameSelector,

                   IncludeEVSEIds,
                   IncludeEVSEs,
                   CustomEVSEIdMapper,

                   ServiceCheckEvery,
                   StatusCheckEvery,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushStatus,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords,

                   PublicKeyRing,
                   SecretKeyRing,
                   DNSClient)

        {

            if (ServerAutoStart)
                CPOServer.Start();

        }

        #endregion

        #endregion


        // RN -> External service requests...

        #region PushEVSEData/-Status directly...

        #region (private) PushEVSEData  (EVSEs,             ServerAction, ...)

        /// <summary>
        /// Upload the EVSE data of the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="ServerAction">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        private async Task<PushEVSEDataResult>

            PushEVSEData(IEnumerable<EVSE>    EVSEs,
                         ActionTypes          ServerAction,

                         DateTime?            Timestamp          = null,
                         CancellationToken?   CancellationToken  = null,
                         EventTracking_Id     EventTrackingId    = null,
                         TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;


            PushEVSEDataResult result;

            #endregion

            #region Get effective number of EVSEs/EVSEDataRecords to upload

            var Warnings         = new List<Warning>();
            var EVSEDataRecords  = new List<EVSEDataRecord>();

            if (EVSEs.IsNeitherNullNorEmpty())
                foreach (var evse in EVSEs.Where(evse => evse != null          &&
                                                         _IncludeEVSEs  (evse) &&
                                                         _IncludeEVSEIds(evse.Id)))
                {

                    try
                    {

                        // WWCP EVSE will be added as custom data "WWCP.EVSE"...
                        EVSEDataRecords.Add(evse.ToOICP(_EVSE2EVSEDataRecord));

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e.Message);
                        Warnings.Add(Warning.Create(e.Message, evse));
                    }

                }

            #endregion

            #region Send OnPushEVSEDataWWCPRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnPushEVSEDataWWCPRequest?.Invoke(StartTime,
                                                  Timestamp.Value,
                                                  this,
                                                  Id,
                                                  EventTrackingId,
                                                  RoamingNetwork.Id,
                                                  ServerAction,
                                                  EVSEDataRecords.ULongCount(),
                                                  EVSEDataRecords,
                                                  Warnings.Where(warning => warning.IsNotNullOrEmpty()),
                                                  RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnPushEVSEDataWWCPRequest));
            }

            #endregion


            DateTime Endtime;
            TimeSpan Runtime;

            if (EVSEDataRecords.Count > 0)
            {

                var response = await CPORoaming.
                                         PushEVSEData(EVSEDataRecords,
                                                      DefaultOperatorId,
                                                      DefaultOperatorName.IsNotNullOrEmpty() ? DefaultOperatorName : null,
                                                      ServerAction,
                                                      null,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);


                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                    response.Content != null)
                {

                    // Success...
                    if (response.Content.Result)
                    {

                        Endtime  = DateTime.UtcNow;
                        Runtime  = Endtime - StartTime;
                        result   = PushEVSEDataResult.Success(Id,
                                                              this,
                                                              response.Content.StatusCode.Description,
                                                              response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                  ? Warnings.AddAndReturnList(response.Content.StatusCode.AdditionalInfo)
                                                                  : Warnings,
                                                              Runtime);

                    }

                    // Operation failed... maybe the systems are out of sync?!
                    // Try an automatic fullLoad in order to repair...
                    else
                    {

                        if (ServerAction == ActionTypes.insert ||
                            ServerAction == ActionTypes.update ||
                            ServerAction == ActionTypes.delete)
                        {

                            #region Add warnings...

                            Warnings.Add(Warning.Create(ServerAction.ToString() + " of " + EVSEDataRecords.Count + " EVSEs failed!"));
                            Warnings.Add(Warning.Create(response.Content.StatusCode.Code.ToString()));
                            Warnings.Add(Warning.Create(response.Content.StatusCode.Description));

                            if (response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty())
                                Warnings.Add(Warning.Create(response.Content.StatusCode.AdditionalInfo));

                            Warnings.Add(Warning.Create("Will try to fix this issue via a 'fullLoad' of all EVSEs!"));

                            #endregion

                            #region Get all EVSEs from the roaming network

                            var FullLoadEVSEs = RoamingNetwork.EVSEs.Where(evse => evse != null &&
                                                                           _IncludeEVSEs(evse) &&
                                                                           _IncludeEVSEIds(evse.Id)).
                                                        Select(evse =>
                                                        {

                                                            try
                                                            {

                                                                return evse.ToOICP(_EVSE2EVSEDataRecord);

                                                            }
                                                            catch (Exception e)
                                                            {
                                                                DebugX.Log(e.Message);
                                                                Warnings.Add(Warning.Create(e.Message, evse));
                                                            }

                                                            return null;

                                                        }).
                                                        Where(evsedatarecord => evsedatarecord != null).
                                                        ToArray();

                            #endregion

                            #region Send request

                            var FullLoadResponse = await CPORoaming.
                                                              PushEVSEData(FullLoadEVSEs,
                                                                           DefaultOperatorId,
                                                                           DefaultOperatorName.IsNotNullOrEmpty() ? DefaultOperatorName : null,
                                                                           ActionTypes.fullLoad,
                                                                           null,

                                                                           Timestamp,
                                                                           CancellationToken,
                                                                           EventTrackingId,
                                                                           RequestTimeout).
                                                              ConfigureAwait(false);

                            #endregion

                            #region Result mapping

                            Endtime = DateTime.UtcNow;
                            Runtime = Endtime - StartTime;

                            if (FullLoadResponse.HTTPStatusCode == HTTPStatusCode.OK &&
                                FullLoadResponse.Content != null)
                            {

                                if (FullLoadResponse.Content.Result)
                                    result = PushEVSEDataResult.Success(Id,
                                                                    this,
                                                                    FullLoadResponse.Content.StatusCode.Description,
                                                                    FullLoadResponse.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                        ? Warnings.AddAndReturnList(FullLoadResponse.Content.StatusCode.AdditionalInfo)
                                                                        : Warnings,
                                                                    Runtime);

                                else
                                    result = PushEVSEDataResult.Error(Id,
                                                                  this,
                                                                  EVSEs,
                                                                  FullLoadResponse.Content.StatusCode.Description,
                                                                  FullLoadResponse.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                      ? Warnings.AddAndReturnList(FullLoadResponse.Content.StatusCode.AdditionalInfo)
                                                                      : Warnings,
                                                                  Runtime);

                            }

                            else
                                result = PushEVSEDataResult.Error(Id,
                                                              this,
                                                              EVSEs,
                                                              FullLoadResponse.HTTPStatusCode.ToString(),
                                                              FullLoadResponse.HTTPBody != null
                                                                  ? Warnings.AddAndReturnList(FullLoadResponse.HTTPBody.ToUTF8String())
                                                                  : Warnings.AddAndReturnList("No HTTP body received!"),
                                                              Runtime);

                            #endregion

                        }

                        // Or a 'fullLoad' Operation failed...
                        else
                        {

                            Endtime  = DateTime.UtcNow;
                            Runtime  = Endtime - StartTime;
                            result   = PushEVSEDataResult.Error(Id,
                                                                this,
                                                                EVSEs,
                                                                response.HTTPStatusCode.ToString(),
                                                                response.HTTPBody != null
                                                                    ? Warnings.AddAndReturnList(response.HTTPBody.ToUTF8String())
                                                                    : Warnings.AddAndReturnList("No HTTP body received!"),
                                                                Runtime);

                        }

                    }

                }
                else
                {

                    Endtime  = DateTime.UtcNow;
                    Runtime  = Endtime - StartTime;
                    result   = PushEVSEDataResult.Error(Id,
                                                        this,
                                                        EVSEs,
                                                        response.HTTPStatusCode.ToString(),
                                                        response.HTTPBody != null
                                                            ? Warnings.AddAndReturnList(response.HTTPBody.ToUTF8String())
                                                            : Warnings.AddAndReturnList("No HTTP body received!"),
                                                        Runtime);

                }

            }

            #region ...or no EVSEs to push...

            else
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = PushEVSEDataResult.NoOperation(Id,
                                                          this,
                                                          "No EVSEDataRecords to push!",
                                                          Warnings,
                                                          DateTime.UtcNow - StartTime);

            }

            #endregion


            #region Send OnPushEVSEDataResponse event

            try
            {

                OnPushEVSEDataWWCPResponse?.Invoke(Endtime,
                                                   Timestamp.Value,
                                                   this,
                                                   Id,
                                                   EventTrackingId,
                                                   RoamingNetwork.Id,
                                                   ServerAction,
                                                   EVSEDataRecords.ULongCount(),
                                                   EVSEDataRecords,
                                                   RequestTimeout,
                                                   result,
                                                   Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnPushEVSEDataWWCPResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region (private) PushEVSEStatus(EVSEStatusUpdates, ServerAction, ...)

        /// <summary>
        /// Upload the EVSE status of the given lookup of EVSE status types grouped by their Charging Station Operator.
        /// </summary>
        /// <param name="EVSEStatusUpdates">An enumeration of EVSE status updates.</param>
        /// <param name="ServerAction">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<PushEVSEStatusResult>

            PushEVSEStatus(IEnumerable<EVSEStatusUpdate>  EVSEStatusUpdates,
                           ActionTypes                    ServerAction,

                           DateTime?                      Timestamp           = null,
                           CancellationToken?             CancellationToken   = null,
                           EventTracking_Id               EventTrackingId     = null,
                           TimeSpan?                      RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEStatusUpdates == null)
                throw new ArgumentNullException(nameof(EVSEStatusUpdates), "The given enumeration of EVSE status updates must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Get effective number of EVSEStatus/EVSEStatusRecords to upload

            var Warnings = new List<Warning>();

            var _EVSEStatus = EVSEStatusUpdates.
                                  Where       (evsestatusupdate => _IncludeEVSEs  (evsestatusupdate.EVSE) &&
                                                                   _IncludeEVSEIds(evsestatusupdate.EVSE.Id)).
                                  ToLookup    (evsestatusupdate => evsestatusupdate.EVSE.Id,
                                               evsestatusupdate => evsestatusupdate).
                                  ToDictionary(group            => group.Key,
                                               group            => group.AsEnumerable().OrderByDescending(item => item.NewStatus.Timestamp)).
                                  Select      (evsestatusupdate => {

                                      try
                                      {

                                          var _EVSEId = evsestatusupdate.Key.ToOICP();

                                          if (!_EVSEId.HasValue)
                                              throw new InvalidEVSEIdentificationException(evsestatusupdate.Key.ToString());

                                          // Only push the current status of the latest status update!
                                          return new EVSEStatusRecord(
                                                     evsestatusupdate.Key.ToOICP().Value,
                                                     evsestatusupdate.Value.First().NewStatus.Value.AsOICPEVSEStatus()
                                                 );

                                      }
                                      catch (Exception e)
                                      {
                                          DebugX.  Log(e.Message);
                                          Warnings.Add(Warning.Create(e.Message, evsestatusupdate));
                                      }

                                      return null;

                                  }).
                                  Where(evsestatusrecord => evsestatusrecord != null).
                                  ToArray();

            PushEVSEStatusResult result = null;

            #endregion

            #region Send OnEVSEStatusPush event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnPushEVSEStatusWWCPRequest?.Invoke(StartTime,
                                                    Timestamp.Value,
                                                    this,
                                                    Id,
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    ServerAction,
                                                    _EVSEStatus.ULongCount(),
                                                    _EVSEStatus,
                                                    Warnings.Where(warning => warning.IsNotNullOrEmpty()),
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnPushEVSEStatusWWCPRequest));
            }

            #endregion


            var response = await CPORoaming.
                                     PushEVSEStatus(_EVSEStatus,
                                                    DefaultOperatorId,
                                                    DefaultOperatorName.IsNotNullOrEmpty() ? DefaultOperatorName : null,
                                                    ServerAction,
                                                    null,

                                                    Timestamp,
                                                    CancellationToken,
                                                    EventTrackingId,
                                                    RequestTimeout);


            var Endtime = DateTime.UtcNow;
            var Runtime = Endtime - StartTime;

            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        != null)
            {

                if (response.Content.Result)
                    result = PushEVSEStatusResult.Success(Id,
                                                      this,
                                                      response.Content.StatusCode.Description,
                                                      response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                          ? Warnings.AddAndReturnList(response.Content.StatusCode.AdditionalInfo)
                                                          : Warnings,
                                                      Runtime);

                else
                    result = PushEVSEStatusResult.Error(Id,
                                                    this,
                                                    EVSEStatusUpdates,
                                                    response.Content.StatusCode.Description,
                                                    response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                        ? Warnings.AddAndReturnList(response.Content.StatusCode.AdditionalInfo)
                                                        : Warnings,
                                                    Runtime);

            }
            else
                result = PushEVSEStatusResult.Error(Id,
                                                this,
                                                EVSEStatusUpdates,
                                                response.HTTPStatusCode.ToString(),
                                                response.HTTPBody != null
                                                    ? Warnings.AddAndReturnList(response.HTTPBody.ToUTF8String())
                                                    : Warnings.AddAndReturnList("No HTTP body received!"),
                                                Runtime);


            #region Send OnPushEVSEStatusResponse event

            try
            {

                OnPushEVSEStatusWWCPResponse?.Invoke(Endtime,
                                                     Timestamp.Value,
                                                     this,
                                                     Id,
                                                     EventTrackingId,
                                                     RoamingNetwork.Id,
                                                     ServerAction,
                                                     _EVSEStatus.ULongCount(),
                                                     _EVSEStatus,
                                                     RequestTimeout,
                                                     result,
                                                     Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnPushEVSEStatusWWCPResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region (Set/Add/Update/Delete) EVSE(s)...

        #region SetStaticData   (EVSE, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the given EVSE as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="EVSE">An EVSE to upload.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.SetStaticData(EVSE                EVSE,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();

                try
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        EVSEsToAddQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                    }

                }
                finally
                {
                    DataAndStatusLock.Release();
                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(new EVSE[] { EVSE },
                                      ActionTypes.fullLoad,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region AddStaticData   (EVSE, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given EVSE to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="EVSE">An EVSE to upload.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.AddStaticData(EVSE                EVSE,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();

                try
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        EVSEsToAddQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                    }

                }
                finally
                {
                    DataAndStatusLock.Release();
                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(new EVSE[] { EVSE },
                                      ActionTypes.insert,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(EVSE, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the static data of the given EVSE.
        /// The EVSE can be uploaded as a whole, or just a single property of the EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to update.</param>
        /// <param name="PropertyName">The name of the EVSE property to update.</param>
        /// <param name="OldValue">The old value of the EVSE property to update.</param>
        /// <param name="NewValue">The new value of the EVSE property to update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(EVSE                EVSE,
                                       String              PropertyName,
                                       Object              OldValue,
                                       Object              NewValue,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE         == null)
                throw new ArgumentNullException(nameof(EVSE),          "The given EVSE must not be null!");

            if (PropertyName == null)
                throw new ArgumentNullException(nameof(PropertyName),  "The given EVSE property name must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();

                try
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        if (EVSEsUpdateLog.TryGetValue(EVSE, out List<PropertyUpdateInfos> PropertyUpdateInfo))
                            PropertyUpdateInfo.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));

                        else
                        {

                            var List = new List<PropertyUpdateInfos>();
                            List.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));
                            EVSEsUpdateLog.Add(EVSE, List);

                        }

                        EVSEsToUpdateQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                    }

                }
                finally
                {
                    DataAndStatusLock.Release();
                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(new EVSE[] { EVSE },
                                      ActionTypes.update,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(EVSE, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the static data of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to delete.</param>
        /// <param name="TransmissionType">Whether to send the EVSE deletion directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(EVSE                EVSE,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();

                try
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        EVSEsToRemoveQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                    }

                }
                finally
                {
                    DataAndStatusLock.Release();
                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(new EVSE[] { EVSE },
                                      ActionTypes.delete,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion


        #region SetStaticData   (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the given enumeration of EVSEs as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.SetStaticData(IEnumerable<EVSE>   EVSEs,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return PushEVSEDataResult.NoOperation(Id, this);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();

                try
                {

                    var FilteredEVSEs = EVSEs.Where(evse => _IncludeEVSEs(evse) &&
                                                            _IncludeEVSEIds(evse.Id)).
                                              ToArray();

                    if (FilteredEVSEs.Any())
                    {

                        foreach (var EVSE in FilteredEVSEs)
                            EVSEsToAddQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                        return PushEVSEDataResult.Enqueued(Id, this);

                    }

                    return PushEVSEDataResult.NoOperation(Id, this);

                }
                finally
                {
                    DataAndStatusLock.Release();
                }

            }

            #endregion


            return await PushEVSEData(EVSEs,
                                      ActionTypes.fullLoad,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region AddStaticData   (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of EVSEs to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.AddStaticData(IEnumerable<EVSE>   EVSEs,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return PushEVSEDataResult.NoOperation(Id, this);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();

                try
                {

                    var FilteredEVSEs = EVSEs.Where(evse => _IncludeEVSEs(evse) &&
                                                            _IncludeEVSEIds(evse.Id)).
                                              ToArray();

                    if (FilteredEVSEs.Any())
                    {

                        foreach (var EVSE in FilteredEVSEs)
                            EVSEsToAddQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                        return PushEVSEDataResult.Enqueued(Id, this);

                    }

                    return PushEVSEDataResult.NoOperation(Id, this);

                }
                finally
                {
                    DataAndStatusLock.Release();
                }

            }

            #endregion

            return await PushEVSEData(EVSEs,
                                      ActionTypes.insert,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSEs within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(IEnumerable<EVSE>   EVSEs,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return PushEVSEDataResult.NoOperation(Id, this);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();

                try
                {

                    var FilteredEVSEs = EVSEs.Where(evse => _IncludeEVSEs(evse) &&
                                                            _IncludeEVSEIds(evse.Id)).
                                              ToArray();

                    if (FilteredEVSEs.Any())
                    {

                        foreach (var EVSE in FilteredEVSEs)
                            EVSEsToUpdateQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                        return PushEVSEDataResult.Enqueued(Id, this);

                    }

                    return PushEVSEDataResult.NoOperation(Id, this);

                }
                finally
                {
                    DataAndStatusLock.Release();
                }

            }

            #endregion

            return await PushEVSEData(EVSEs,
                                      ActionTypes.update,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the given enumeration of EVSEs from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(IEnumerable<EVSE>   EVSEs,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return PushEVSEDataResult.NoOperation(Id, this);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();

                try
                {

                    var FilteredEVSEs = EVSEs.Where(evse => _IncludeEVSEs(evse) &&
                                                            _IncludeEVSEIds(evse.Id)).
                                              ToArray();

                    if (FilteredEVSEs.Any())
                    {

                        foreach (var EVSE in FilteredEVSEs)
                            EVSEsToRemoveQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                        return PushEVSEDataResult.Enqueued(Id, this);

                    }

                    return PushEVSEDataResult.NoOperation(Id, this);

                }
                finally
                {
                    DataAndStatusLock.Release();
                }

            }

            #endregion

            return await PushEVSEData(EVSEs,
                                      ActionTypes.delete,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of EVSE admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<EVSEAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                   TransmissionType,

                                               DateTime?                           Timestamp,
                                               CancellationToken?                  CancellationToken,
                                               EventTracking_Id                    EventTrackingId,
                                               TimeSpan?                           RequestTimeout)


                => Task.FromResult(PushEVSEAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of EVSE status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<EVSEStatusUpdate>  StatusUpdates,
                                     TransmissionTypes              TransmissionType,

                                     DateTime?                      Timestamp,
                                     CancellationToken?             CancellationToken,
                                     EventTracking_Id               EventTrackingId,
                                     TimeSpan?                      RequestTimeout)

        {

            #region Initial checks

            if (StatusUpdates == null || !StatusUpdates.Any())
                return PushEVSEStatusResult.NoOperation(Id, this);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();

                try
                {

                    var FilteredUpdates = StatusUpdates.Where(statusupdate => _IncludeEVSEs  (statusupdate.EVSE) &&
                                                                              _IncludeEVSEIds(statusupdate.EVSE.Id)).
                                                        ToArray();

                    if (FilteredUpdates.Length > 0)
                    {

                        foreach (var Update in FilteredUpdates)
                        {

                            // Delay the status update until the EVSE data had been uploaded!
                            if (EVSEsToAddQueue.Any(evse => evse == Update.EVSE))
                                EVSEStatusChangesDelayedQueue.Add(Update);

                            else
                                EVSEStatusChangesFastQueue.Add(Update);

                        }

                        FlushEVSEFastStatusTimer.Change(FlushEVSEFastStatusEvery, TimeSpan.FromMilliseconds(-1));

                        return PushEVSEStatusResult.Enqueued(Id, this);

                    }

                    return PushEVSEStatusResult.NoOperation(Id, this);

                }
                finally
                {
                    DataAndStatusLock.Release();
                }

            }

            #endregion

            return await PushEVSEStatus(StatusUpdates,
                                        ActionTypes.update,

                                        Timestamp,
                                        CancellationToken,
                                        EventTrackingId,
                                        RequestTimeout);

        }

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station(s)...

        #region SetStaticData   (ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.SetStaticData(ChargingStation     ChargingStation,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();
                {

                    foreach (var evse in ChargingStation)
                    {

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                        }

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingStation.EVSEs,
                                      ActionTypes.fullLoad,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region AddStaticData   (ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.AddStaticData(ChargingStation     ChargingStation,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();
                {

                    foreach (var evse in ChargingStation)
                    {

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                        }

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingStation.EVSEs,
                                      ActionTypes.insert,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region UpdateStaticData(ChargingStation, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="PropertyName">The name of the charging station property to update.</param>
        /// <param name="OldValue">The old value of the charging station property to update.</param>
        /// <param name="NewValue">The new value of the charging station property to update.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(ChargingStation     ChargingStation,
                                       String              PropertyName,
                                       Object              OldValue,
                                       Object              NewValue,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();
                {

                    var AddData = false;

                    foreach (var evse in ChargingStation)
                    {
                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {
                            EVSEsToUpdateQueue.Add(evse);
                            AddData = true;
                        }
                    }

                    if (AddData)
                    {

                        if (ChargingStationsUpdateLog.TryGetValue(ChargingStation, out List<PropertyUpdateInfos> PropertyUpdateInfo))
                            PropertyUpdateInfo.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));

                        else
                        {
                            var List = new List<PropertyUpdateInfos>();
                            List.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));
                            ChargingStationsUpdateLog.Add(ChargingStation, List);
                        }

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingStation,
                                      ActionTypes.update,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region DeleteStaticData(ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(ChargingStation     ChargingStation,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            return PushEVSEData(ChargingStation.EVSEs,
                                ActionTypes.delete,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion


        #region SetStaticData   (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging stations as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(IEnumerable<ChargingStation>  ChargingStations,
                                    TransmissionTypes             TransmissionType,

                                    DateTime?                     Timestamp,
                                    CancellationToken?            CancellationToken,
                                    EventTracking_Id              EventTrackingId,
                                    TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return PushEVSEData(ChargingStations.SafeSelectMany(station => station.EVSEs),
                                ActionTypes.fullLoad,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region AddStaticData   (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging stations to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(IEnumerable<ChargingStation>  ChargingStations,
                                    TransmissionTypes             TransmissionType,


                                    DateTime?                     Timestamp,
                                    CancellationToken?            CancellationToken,
                                    EventTracking_Id              EventTrackingId,
                                    TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return PushEVSEData(ChargingStations.SafeSelectMany(station => station.EVSEs),
                                ActionTypes.insert,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging stations within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(IEnumerable<ChargingStation>  ChargingStations,
                                       TransmissionTypes             TransmissionType,

                                       DateTime?                     Timestamp,
                                       CancellationToken?            CancellationToken,
                                       EventTracking_Id              EventTrackingId,
                                       TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return PushEVSEData(ChargingStations.SafeSelectMany(station => station.EVSEs),
                                ActionTypes.update,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging stations from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(IEnumerable<ChargingStation>  ChargingStations,
                                       TransmissionTypes             TransmissionType,

                                       DateTime?                     Timestamp,
                                       CancellationToken?            CancellationToken,
                                       EventTracking_Id              EventTrackingId,
                                       TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return PushEVSEData(ChargingStations.SafeSelectMany(station => station.EVSEs),
                                ActionTypes.delete,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging station admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                              TransmissionType,

                                               DateTime?                                      Timestamp,
                                               CancellationToken?                             CancellationToken,
                                               EventTracking_Id                               EventTrackingId,
                                               TimeSpan?                                      RequestTimeout)


                => Task.FromResult(PushChargingStationAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging station status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingStationStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                         TransmissionType,

                                     DateTime?                                 Timestamp,
                                     CancellationToken?                        CancellationToken,
                                     EventTracking_Id                          EventTrackingId,
                                     TimeSpan?                                 RequestTimeout)


                => Task.FromResult(PushChargingStationStatusResult.NoOperation(Id, this));

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging pool(s)...

        #region SetStaticData   (ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given charging pool as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.SetStaticData(ChargingPool        ChargingPool,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();
                {

                    foreach (var evse in ChargingPool.EVSEs)
                    {

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                        }

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingPool.EVSEs,
                                      ActionTypes.fullLoad,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region AddStaticData   (ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given charging pool to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.AddStaticData(ChargingPool        ChargingPool,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();
                {

                    foreach (var evse in ChargingPool.EVSEs)
                    {

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                        }

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingPool.EVSEs,
                                      ActionTypes.insert,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region UpdateStaticData(ChargingPool, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given charging pool within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="PropertyName">The name of the charging pool property to update.</param>
        /// <param name="OldValue">The old value of the charging pool property to update.</param>
        /// <param name="NewValue">The new value of the charging pool property to update.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(ChargingPool        ChargingPool,
                                       String              PropertyName,
                                       Object              OldValue,
                                       Object              NewValue,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                await DataAndStatusLock.WaitAsync();
                {

                    var AddData = false;

                    foreach (var evse in ChargingPool.EVSEs)
                    {
                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {
                            EVSEsToUpdateQueue.Add(evse);
                            AddData = true;
                        }
                    }

                    if (AddData)
                    {

                        if (ChargingPoolsUpdateLog.TryGetValue(ChargingPool, out List<PropertyUpdateInfos> PropertyUpdateInfo))
                            PropertyUpdateInfo.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));

                        else
                        {
                            var List = new List<PropertyUpdateInfos>();
                            List.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));
                            ChargingPoolsUpdateLog.Add(ChargingPool, List);
                        }

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingPool.EVSEs,
                                      ActionTypes.update,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region DeleteStaticData(ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging pool from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(ChargingPool        ChargingPool,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            return PushEVSEData(ChargingPool.EVSEs,
                                ActionTypes.delete,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion


        #region SetStaticData   (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging pools as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(IEnumerable<ChargingPool>  ChargingPools,
                                    TransmissionTypes          TransmissionType,

                                    DateTime?                  Timestamp,
                                    CancellationToken?         CancellationToken,
                                    EventTracking_Id           EventTrackingId,
                                    TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return PushEVSEData(ChargingPools.SafeSelectMany(pool => pool.EVSEs),
                                ActionTypes.fullLoad,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region AddStaticData   (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging pools to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(IEnumerable<ChargingPool>  ChargingPools,
                                    TransmissionTypes          TransmissionType,

                                    DateTime?                  Timestamp,
                                    CancellationToken?         CancellationToken,
                                    EventTracking_Id           EventTrackingId,
                                    TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return PushEVSEData(ChargingPools.SafeSelectMany(pool => pool.EVSEs),
                                ActionTypes.insert,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging pools within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(IEnumerable<ChargingPool>  ChargingPools,
                                       TransmissionTypes          TransmissionType,

                                       DateTime?                  Timestamp,
                                       CancellationToken?         CancellationToken,
                                       EventTracking_Id           EventTrackingId,
                                       TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return PushEVSEData(ChargingPools.SafeSelectMany(pool => pool.EVSEs),
                                ActionTypes.update,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging pools from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(IEnumerable<ChargingPool>  ChargingPools,
                                       TransmissionTypes          TransmissionType,

                                       DateTime?                  Timestamp,
                                       CancellationToken?         CancellationToken,
                                       EventTracking_Id           EventTrackingId,
                                       TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return PushEVSEData(ChargingPools.SafeSelectMany(pool => pool.EVSEs),
                                ActionTypes.delete,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging pool admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging pool admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging pool admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingPoolAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                           TransmissionType,

                                               DateTime?                                   Timestamp,
                                               CancellationToken?                          CancellationToken,
                                               EventTracking_Id                            EventTrackingId,
                                               TimeSpan?                                   RequestTimeout)


                => Task.FromResult(PushChargingPoolAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging pool status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging pool status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging pool status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingPoolStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingPoolStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                      TransmissionType,

                                     DateTime?                              Timestamp,
                                     CancellationToken?                     CancellationToken,
                                     EventTracking_Id                       EventTrackingId,
                                     TimeSpan?                              RequestTimeout)


                => Task.FromResult(PushChargingPoolStatusResult.NoOperation(Id, this));

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station operator(s)...

        #region SetStaticData   (ChargingStationOperator, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station operator as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(ChargingStationOperator  ChargingStationOperator,

                                    DateTime?                     Timestamp,
                                    CancellationToken?            CancellationToken,
                                    EventTracking_Id              EventTrackingId,
                                    TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return PushEVSEData(ChargingStationOperator.EVSEs,
                                ActionTypes.fullLoad,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region AddStaticData   (ChargingStationOperator, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station operator to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(ChargingStationOperator  ChargingStationOperator,

                                    DateTime?                     Timestamp,
                                    CancellationToken?            CancellationToken,
                                    EventTracking_Id              EventTrackingId,
                                    TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return PushEVSEData(ChargingStationOperator.EVSEs,
                                ActionTypes.insert,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(ChargingStationOperator, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station operator within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(ChargingStationOperator  ChargingStationOperator,

                                       DateTime?                     Timestamp,
                                       CancellationToken?            CancellationToken,
                                       EventTracking_Id              EventTrackingId,
                                       TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return PushEVSEData(ChargingStationOperator.EVSEs,
                                ActionTypes.update,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(ChargingStationOperator, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station operator from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(ChargingStationOperator  ChargingStationOperator,

                                       DateTime?                     Timestamp,
                                       CancellationToken?            CancellationToken,
                                       EventTracking_Id              EventTrackingId,
                                       TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return PushEVSEData(ChargingStationOperator.EVSEs,
                                ActionTypes.delete,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion


        #region SetStaticData   (ChargingStationOperators, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging station operators as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                    DateTime?                                  Timestamp,
                                    CancellationToken?                         CancellationToken,
                                    EventTracking_Id                           EventTrackingId,
                                    TimeSpan?                                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return PushEVSEData(ChargingStationOperators.SafeSelectMany(stationoperator => stationoperator.EVSEs),
                                ActionTypes.fullLoad,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region AddStaticData   (ChargingStationOperators, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging station operators to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                    DateTime?                                  Timestamp,
                                    CancellationToken?                         CancellationToken,
                                    EventTracking_Id                           EventTrackingId,
                                    TimeSpan?                                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return PushEVSEData(ChargingStationOperators.SafeSelectMany(stationoperator => stationoperator.EVSEs),
                                ActionTypes.insert,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(ChargingStationOperators, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging station operators within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                       DateTime?                                  Timestamp,
                                       CancellationToken?                         CancellationToken,
                                       EventTracking_Id                           EventTrackingId,
                                       TimeSpan?                                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return PushEVSEData(ChargingStationOperators.SafeSelectMany(stationoperator => stationoperator.EVSEs),
                                ActionTypes.update,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(ChargingStationOperators, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging station operators from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                       DateTime?                                  Timestamp,
                                       CancellationToken?                         CancellationToken,
                                       EventTracking_Id                           EventTrackingId,
                                       TimeSpan?                                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return PushEVSEData(ChargingStationOperators.SafeSelectMany(stationoperator => stationoperator.EVSEs),
                                ActionTypes.delete,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion


        #region UpdateChargingStationOperatorAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station operator admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging station operator admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationOperatorAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                                      TransmissionType,

                                               DateTime?                                              Timestamp,
                                               CancellationToken?                                     CancellationToken,
                                               EventTracking_Id                                       EventTrackingId,
                                               TimeSpan?                                              RequestTimeout)


                => Task.FromResult(PushChargingStationOperatorAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateChargingStationOperatorStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station operator status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging station operator status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationOperatorStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingStationOperatorStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                                 TransmissionType,

                                     DateTime?                                         Timestamp,
                                     CancellationToken?                                CancellationToken,
                                     EventTracking_Id                                  EventTrackingId,
                                     TimeSpan?                                         RequestTimeout)


                => Task.FromResult(PushChargingStationOperatorStatusResult.NoOperation(Id, this));

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Roaming network...

        #region SetStaticData   (RoamingNetwork, ...)

        /// <summary>
        /// Set the EVSE data of the given roaming network as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(RoamingNetwork      RoamingNetwork,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return PushEVSEData(RoamingNetwork.EVSEs,
                                ActionTypes.fullLoad,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region AddStaticData   (RoamingNetwork, ...)

        /// <summary>
        /// Add the EVSE data of the given roaming network to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(RoamingNetwork      RoamingNetwork,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return PushEVSEData(RoamingNetwork.EVSEs,
                                ActionTypes.insert,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(RoamingNetwork, ...)

        /// <summary>
        /// Update the EVSE data of the given roaming network within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(RoamingNetwork      RoamingNetwork,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return PushEVSEData(RoamingNetwork.EVSEs,
                                ActionTypes.update,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(RoamingNetwork, ...)

        /// <summary>
        /// Delete the EVSE data of the given roaming network from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network to upload.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(RoamingNetwork      RoamingNetwork,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return PushEVSEData(RoamingNetwork.EVSEs,
                                ActionTypes.delete,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion


        #region UpdateRoamingNetworkAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of roaming network admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of roaming network admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the roaming network admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushRoamingNetworkAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                             TransmissionType,

                                               DateTime?                                     Timestamp,
                                               CancellationToken?                            CancellationToken,
                                               EventTracking_Id                              EventTrackingId,
                                               TimeSpan?                                     RequestTimeout)


                => Task.FromResult(PushRoamingNetworkAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateRoamingNetworkStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of roaming network status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of roaming network status updates.</param>
        /// <param name="TransmissionType">Whether to send the roaming network status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushRoamingNetworkStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<RoamingNetworkStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                        TransmissionType,

                                     DateTime?                                Timestamp,
                                     CancellationToken?                       CancellationToken,
                                     EventTracking_Id                         EventTrackingId,
                                     TimeSpan?                                RequestTimeout)


                => Task.FromResult(PushRoamingNetworkStatusResult.NoOperation(Id, this));

        #endregion

        #endregion

        #endregion

        #region AuthorizeStart/-Stop  directly...

        #region AuthorizeStart(AuthIdentification,                    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(AuthIdentification           AuthIdentification,
                           ChargingProduct              ChargingProduct     = null,
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification),   "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeStartRequest?.Invoke(StartTime,
                                                Timestamp.Value,
                                                this,
                                                Id.ToString(),
                                                EventTrackingId,
                                                RoamingNetwork.Id,
                                                OperatorId,
                                                AuthIdentification,
                                                ChargingProduct,
                                                SessionId,
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            DateTime         Endtime;
            TimeSpan         Runtime;
            AuthStartResult  result;

            if (DisableAuthentication)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStartResult.AdminDown(Id,
                                                     this,
                                                     SessionId,
                                                     Runtime);

            }

            else
            {

                var response = await CPORoaming.
                                         AuthorizeStart(OperatorId.HasValue
                                                            ? OperatorId.Value.ToOICP(DefaultOperatorIdFormat)
                                                            : DefaultOperatorId,
                                                        AuthIdentification. ToOICP().RFIDId.Value,
                                                        null,
                                                        ChargingProduct?.Id.ToOICP(),
                                                        SessionId.          ToOICP(),
                                                        null,

                                                        Timestamp,
                                                        CancellationToken,
                                                        EventTrackingId,
                                                        RequestTimeout);


                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response.Content                     != null              &&
                    response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
                {

                    result = AuthStartResult.Authorized(
                                 Id,
                                 this,
                                 response.Content.SessionId.ToWWCP().Value,
                                 ProviderId:      response.Content.ProviderId.ToWWCP(),
                                 Description:     response.Content.StatusCode.Description,
                                 AdditionalInfo:  response.Content.StatusCode.AdditionalInfo,
                                 Runtime:         Runtime
                             );

                }

                else
                    result = AuthStartResult.NotAuthorized(
                                 Id,
                                 this,
                                 SessionId,
                                 response.Content.ProviderId.ToWWCP(),
                                 response.Content.StatusCode.Description,
                                 response.Content.StatusCode.AdditionalInfo,
                                 Runtime
                             );

            }


            #region Send OnAuthorizeStartResponse event

            try
            {

                OnAuthorizeStartResponse?.Invoke(Endtime,
                                                 Timestamp.Value,
                                                 this,
                                                 Id.ToString(),
                                                 EventTrackingId,
                                                 RoamingNetwork.Id,
                                                 OperatorId,
                                                 AuthIdentification,
                                                 ChargingProduct,
                                                 SessionId,
                                                 RequestTimeout,
                                                 result,
                                                 Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(AuthIdentification, EVSEId,            ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartEVSEResult>

            AuthorizeStart(AuthIdentification           AuthIdentification,
                           WWCP.EVSE_Id                 EVSEId,
                           ChargingProduct              ChargingProduct     = null,   // [maxlength: 100]
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification),  "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeEVSEStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeEVSEStartRequest?.Invoke(StartTime,
                                                    Timestamp.Value,
                                                    this,
                                                    Id.ToString(),
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    OperatorId,
                                                    AuthIdentification,
                                                    EVSEId,
                                                    ChargingProduct,
                                                    SessionId,
                                                    new ISendAuthorizeStartStop[0],
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeEVSEStartRequest));
            }

            #endregion


            DateTime             Endtime;
            TimeSpan             Runtime;
            AuthStartEVSEResult  result;

            if (DisableAuthentication)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStartEVSEResult.AdminDown(Id,
                                                         this,
                                                         SessionId,
                                                         Runtime);

            }

            else
            {

                var response  = await CPORoaming.
                                          AuthorizeStart(OperatorId.HasValue
                                                            ? OperatorId.Value.ToOICP(DefaultOperatorIdFormat)
                                                            : DefaultOperatorId,
                                                         AuthIdentification. ToOICP().RFIDId.Value,
                                                         EVSEId.             ToOICP(),
                                                         ChargingProduct?.Id.ToOICP(),
                                                         SessionId.          ToOICP(),
                                                         null,

                                                         Timestamp,
                                                         CancellationToken,
                                                         EventTrackingId,
                                                         RequestTimeout);


                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response.Content                     != null              &&
                    response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
                {

                    result = AuthStartEVSEResult.Authorized(
                                 Id,
                                 this,
                                 response.Content.SessionId.ToWWCP().Value,
                                 ProviderId:      response.Content.ProviderId.ToWWCP(),
                                 Description:     response.Content.StatusCode.Description,
                                 AdditionalInfo:  response.Content.StatusCode.AdditionalInfo,
                                 NumberOfRetries: response.NumberOfRetries,
                                 Runtime:         Runtime
                             );

                }

                else
                    result = AuthStartEVSEResult.NotAuthorized(
                                 Id,
                                 this,
                                 SessionId,
                                 response.Content.ProviderId.ToWWCP(),
                                 response.Content.StatusCode.Description,
                                 response.Content.StatusCode.AdditionalInfo,
                                 Runtime
                             );

            }


            #region Send OnAuthorizeEVSEStartResponse event

            try
            {

                OnAuthorizeEVSEStartResponse?.Invoke(Endtime,
                                                     Timestamp.Value,
                                                     this,
                                                     Id.ToString(),
                                                     EventTrackingId,
                                                     RoamingNetwork.Id,
                                                     OperatorId,
                                                     AuthIdentification,
                                                     EVSEId,
                                                     ChargingProduct,
                                                     SessionId,
                                                     new ISendAuthorizeStartStop[0],
                                                     RequestTimeout,
                                                     result,
                                                     Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeEVSEStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(AuthIdentification, ChargingStationId, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging station.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingStationId">The unique identification charging station.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartChargingStationResult>

            AuthorizeStart(AuthIdentification           AuthIdentification,
                           WWCP.ChargingStation_Id      ChargingStationId,
                           ChargingProduct              ChargingProduct     = null,   // [maxlength: 100]
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingStationStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingStationStartRequest?.Invoke(StartTime,
                                                               Timestamp.Value,
                                                               this,
                                                               Id.ToString(),
                                                               EventTrackingId,
                                                               RoamingNetwork.Id,
                                                               OperatorId,
                                                               AuthIdentification,
                                                               ChargingStationId,
                                                               ChargingProduct,
                                                               SessionId,
                                                               RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingStationStartRequest));
            }

            #endregion


            DateTime                        Endtime;
            TimeSpan                        Runtime;
            AuthStartChargingStationResult  result;

            if (DisableAuthentication)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStartChargingStationResult.AdminDown(Id,
                                                                    this,
                                                                    SessionId,
                                                                    Runtime);

            }

            else
            {

                var response  = await CPORoaming.
                                          AuthorizeStart(OperatorId.HasValue
                                                            ? OperatorId.Value.ToOICP(DefaultOperatorIdFormat)
                                                            : DefaultOperatorId,
                                                         AuthIdentification. ToOICP().RFIDId.Value,
                                                         null,
                                                         ChargingProduct?.Id.ToOICP(),
                                                         SessionId.          ToOICP(),
                                                         null,

                                                         Timestamp,
                                                         CancellationToken,
                                                         EventTrackingId,
                                                         RequestTimeout);


                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response.Content                     != null              &&
                    response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
                {

                    result = AuthStartChargingStationResult.Authorized(
                                 Id,
                                 this,
                                 response.Content.SessionId.ToWWCP().Value,
                                 ProviderId:      response.Content.ProviderId.ToWWCP(),
                                 Description:     response.Content.StatusCode.Description,
                                 AdditionalInfo:  response.Content.StatusCode.AdditionalInfo,
                                 Runtime:         Runtime
                             );

                }

                else
                    result = AuthStartChargingStationResult.NotAuthorized(
                                 Id,
                                 this,
                                 SessionId,
                                 response.Content.ProviderId.ToWWCP(),
                                 response.Content.StatusCode.Description,
                                 response.Content.StatusCode.AdditionalInfo,
                                 Runtime
                             );

            }


            #region Send OnAuthorizeChargingStationStartResponse event

            try
            {

                OnAuthorizeChargingStationStartResponse?.Invoke(Endtime,
                                                                Timestamp.Value,
                                                                this,
                                                                Id.ToString(),
                                                                EventTrackingId,
                                                                RoamingNetwork.Id,
                                                                OperatorId,
                                                                AuthIdentification,
                                                                ChargingStationId,
                                                                ChargingProduct,
                                                                SessionId,
                                                                RequestTimeout,
                                                                result,
                                                                Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingStationStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(AuthIdentification, ChargingPoolId,    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging pool.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingPoolId">The unique identification charging pool.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartChargingPoolResult>

            AuthorizeStart(AuthIdentification           AuthIdentification,
                           WWCP.ChargingPool_Id         ChargingPoolId,
                           ChargingProduct              ChargingProduct     = null,   // [maxlength: 100]
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingPoolStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingPoolStartRequest?.Invoke(StartTime,
                                                            Timestamp.Value,
                                                            this,
                                                            Id.ToString(),
                                                            EventTrackingId,
                                                            RoamingNetwork.Id,
                                                            OperatorId,
                                                            AuthIdentification,
                                                            ChargingPoolId,
                                                            ChargingProduct,
                                                            SessionId,
                                                            RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingPoolStartRequest));
            }

            #endregion


            DateTime                     Endtime;
            TimeSpan                     Runtime;
            AuthStartChargingPoolResult  result;

            if (DisableAuthentication)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStartChargingPoolResult.AdminDown(Id,
                                                                 this,
                                                                 SessionId,
                                                                 Runtime);

            }

            else
            {

                var response  = await CPORoaming.
                                          AuthorizeStart(OperatorId.HasValue
                                                            ? OperatorId.Value.ToOICP(DefaultOperatorIdFormat)
                                                            : DefaultOperatorId,
                                                         AuthIdentification. ToOICP().RFIDId.Value,
                                                         null,
                                                         ChargingProduct?.Id.ToOICP(),
                                                         SessionId.          ToOICP(),
                                                         null,

                                                         Timestamp,
                                                         CancellationToken,
                                                         EventTrackingId,
                                                         RequestTimeout);


                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response.Content                     != null              &&
                    response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
                {

                    result = AuthStartChargingPoolResult.Authorized(
                                 Id,
                                 this,
                                 response.Content.SessionId.ToWWCP().Value,
                                 ProviderId:      response.Content.ProviderId.ToWWCP(),
                                 Description:     response.Content.StatusCode.Description,
                                 AdditionalInfo:  response.Content.StatusCode.AdditionalInfo,
                                 Runtime:         Runtime
                             );

                }

                else
                    result = AuthStartChargingPoolResult.NotAuthorized(
                                 Id,
                                 this,
                                 SessionId,
                                 response.Content.ProviderId.ToWWCP(),
                                 response.Content.StatusCode.Description,
                                 response.Content.StatusCode.AdditionalInfo,
                                 Runtime
                             );

            }


            #region Send OnAuthorizeChargingPoolStartResponse event

            try
            {

                OnAuthorizeChargingPoolStartResponse?.Invoke(Endtime,
                                                             Timestamp.Value,
                                                             this,
                                                             Id.ToString(),
                                                             EventTrackingId,
                                                             RoamingNetwork.Id,
                                                             OperatorId,
                                                             AuthIdentification,
                                                             ChargingPoolId,
                                                             ChargingProduct,
                                                             SessionId,
                                                             RequestTimeout,
                                                             result,
                                                             Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingPoolStartResponse));
            }

            #endregion

            return result;

        }

        #endregion


        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        #region AuthorizeStop(SessionId, AuthIdentification,                    OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          AuthIdentification           AuthIdentification,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification),  "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeStopRequest?.Invoke(StartTime,
                                               Timestamp.Value,
                                               this,
                                               Id.ToString(),
                                               EventTrackingId,
                                               RoamingNetwork.Id,
                                               OperatorId,
                                               SessionId,
                                               AuthIdentification,
                                               RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            DateTime        Endtime;
            TimeSpan        Runtime;
            AuthStopResult  result;

            if (DisableAuthentication)
            {
                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStopResult.AdminDown(Id,
                                                    this,
                                                    SessionId,
                                                    Runtime);
            }

            else
            {

                var response = await CPORoaming.AuthorizeStop(OperatorId.HasValue
                                                                  ? OperatorId.Value.ToOICP(DefaultOperatorIdFormat)
                                                                  : DefaultOperatorId,
                                                              SessionId.         ToOICP(),
                                                              AuthIdentification.ToOICP().RFIDId.Value,
                                                              null,
                                                              null,

                                                              Timestamp,
                                                              CancellationToken,
                                                              EventTrackingId,
                                                              RequestTimeout);


                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response.Content                     != null              &&
                    response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
                {

                    result = AuthStopResult.Authorized(
                                 Id,
                                 this,
                                 SessionId,
                                 response.Content.ProviderId.ToWWCP(),
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.Description
                                     : null,
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.AdditionalInfo
                                     : null
                             );

                }
                else
                    result = AuthStopResult.NotAuthorized(
                                 Id,
                                 this,
                                 SessionId,
                                 response.Content?.ProviderId.ToWWCP(),
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.Description
                                     : null,
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.AdditionalInfo
                                     : null
                             );

            }


            #region Send OnAuthorizeStopResponse event

            try
            {

                OnAuthorizeStopResponse?.Invoke(Endtime,
                                                Timestamp.Value,
                                                this,
                                                Id.ToString(),
                                                EventTrackingId,
                                                RoamingNetwork.Id,
                                                OperatorId,
                                                SessionId,
                                                AuthIdentification,
                                                RequestTimeout,
                                                result,
                                                Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(SessionId, AuthIdentification, EVSEId,            OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given EVSE.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopEVSEResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          AuthIdentification           AuthIdentification,
                          WWCP.EVSE_Id                 EVSEId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (AuthIdentification  == null)
                throw new ArgumentNullException(nameof(AuthIdentification), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeEVSEStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeEVSEStopRequest?.Invoke(StartTime,
                                                   Timestamp.Value,
                                                   this,
                                                   Id.ToString(),
                                                   EventTrackingId,
                                                   RoamingNetwork.Id,
                                                   OperatorId,
                                                   EVSEId,
                                                   SessionId,
                                                   AuthIdentification,
                                                   RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeEVSEStopRequest));
            }

            #endregion


            DateTime            Endtime;
            TimeSpan            Runtime;
            AuthStopEVSEResult  result;

            if (DisableAuthentication)
            {
                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStopEVSEResult.AdminDown(Id,
                                                        this,
                                                        SessionId,
                                                        Runtime);
            }

            else
            {

                var response  = await CPORoaming.AuthorizeStop(OperatorId.HasValue
                                                                  ? OperatorId.Value.ToOICP(DefaultOperatorIdFormat)
                                                                  : DefaultOperatorId,
                                                               SessionId.         ToOICP(),
                                                               AuthIdentification.ToOICP().RFIDId.Value,
                                                               EVSEId.            ToOICP(),
                                                               null,

                                                               Timestamp,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               RequestTimeout);


                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response.Content                     != null              &&
                    response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
                {

                    result = AuthStopEVSEResult.Authorized(
                                 Id,
                                 this,
                                 SessionId,
                                 response.Content?.ProviderId?.ToWWCP(),
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.Description
                                     : null,
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.AdditionalInfo
                                     : null
                             );

                }
                else
                    result = AuthStopEVSEResult.NotAuthorized(
                                 Id,
                                 this,
                                 SessionId,
                                 response.Content?.ProviderId?.ToWWCP(),
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.Description
                                     : null,
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.AdditionalInfo
                                     : null
                             );

            }


            #region Send OnAuthorizeEVSEStopResponse event

            try
            {

                OnAuthorizeEVSEStopResponse?.Invoke(Endtime,
                                                    Timestamp.Value,
                                                    this,
                                                    Id.ToString(),
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    OperatorId,
                                                    EVSEId,
                                                    SessionId,
                                                    AuthIdentification,
                                                    RequestTimeout,
                                                    result,
                                                    Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeEVSEStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(SessionId, AuthIdentification, ChargingStationId, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging station.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopChargingStationResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          AuthIdentification           AuthIdentification,
                          WWCP.ChargingStation_Id      ChargingStationId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingStationStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingStationStopRequest?.Invoke(StartTime,
                                                              Timestamp.Value,
                                                              this,
                                                              Id.ToString(),
                                                              EventTrackingId,
                                                              RoamingNetwork.Id,
                                                              OperatorId,
                                                              ChargingStationId,
                                                              SessionId,
                                                              AuthIdentification,
                                                              RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingStationStopRequest));
            }

            #endregion


            DateTime                       Endtime;
            TimeSpan                       Runtime;
            AuthStopChargingStationResult  result;

            if (DisableAuthentication)
            {
                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStopChargingStationResult.AdminDown(Id,
                                                                   this,
                                                                   SessionId,
                                                                   Runtime);
            }

            else
            {

                var response  = await CPORoaming.AuthorizeStop(OperatorId.HasValue
                                                                  ? OperatorId.Value.ToOICP(DefaultOperatorIdFormat)
                                                                  : DefaultOperatorId,
                                                               SessionId.         ToOICP(),
                                                               AuthIdentification.ToOICP().RFIDId.Value,
                                                               null,
                                                               null,

                                                               Timestamp,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               RequestTimeout);


                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response.Content                     != null              &&
                    response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
                {

                    result = AuthStopChargingStationResult.Authorized(
                                 Id,
                                 this,
                                 SessionId,
                                 response.Content?.ProviderId?.ToWWCP(),
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.Description
                                     : null,
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.AdditionalInfo
                                     : null
                             );

                }
                else
                    result = AuthStopChargingStationResult.NotAuthorized(
                                 Id,
                                 this,
                                 SessionId,
                                 response.Content?.ProviderId?.ToWWCP(),
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.Description
                                     : null,
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.AdditionalInfo
                                     : null
                             );

            }


            #region Send OnAuthorizeChargingStationStopResponse event

            try
            {

                OnAuthorizeChargingStationStopResponse?.Invoke(Endtime,
                                                               Timestamp.Value,
                                                               this,
                                                               Id.ToString(),
                                                               EventTrackingId,
                                                               RoamingNetwork.Id,
                                                               OperatorId,
                                                               ChargingStationId,
                                                               SessionId,
                                                               AuthIdentification,
                                                               RequestTimeout,
                                                               result,
                                                               Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingStationStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(SessionId, AuthIdentification, ChargingPoolId,    OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging pool.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopChargingPoolResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          AuthIdentification           AuthIdentification,
                          WWCP.ChargingPool_Id         ChargingPoolId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingPoolStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingPoolStopRequest?.Invoke(StartTime,
                                                           Timestamp.Value,
                                                           this,
                                                           Id.ToString(),
                                                           EventTrackingId,
                                                           RoamingNetwork.Id,
                                                           OperatorId,
                                                           ChargingPoolId,
                                                           SessionId,
                                                           AuthIdentification,
                                                           RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingPoolStopRequest));
            }

            #endregion


            DateTime                    Endtime;
            TimeSpan                    Runtime;
            AuthStopChargingPoolResult  result;

            if (DisableAuthentication)
            {
                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStopChargingPoolResult.AdminDown(Id,
                                                                this,
                                                                SessionId,
                                                                Runtime);
            }

            else
            {

                var response  = await CPORoaming.AuthorizeStop(OperatorId.HasValue
                                                                  ? OperatorId.Value.ToOICP(DefaultOperatorIdFormat)
                                                                  : DefaultOperatorId,
                                                               SessionId.         ToOICP(),
                                                               AuthIdentification.ToOICP().RFIDId.Value,
                                                               null,
                                                               null,

                                                               Timestamp,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               RequestTimeout);


                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response.Content                     != null              &&
                    response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
                {

                    result = AuthStopChargingPoolResult.Authorized(
                                 Id,
                                 this,
                                 SessionId,
                                 response.Content?.ProviderId?.ToWWCP(),
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.Description
                                     : null,
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.AdditionalInfo
                                     : null
                             );

                }
                else
                    result = AuthStopChargingPoolResult.NotAuthorized(
                                 Id,
                                 this,
                                 SessionId,
                                 response.Content?.ProviderId?.ToWWCP(),
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.Description
                                     : null,
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.AdditionalInfo
                                     : null
                             );

            }


            #region Send OnAuthorizeChargingPoolStopResponse event

            try
            {

                OnAuthorizeChargingPoolStopResponse?.Invoke(Endtime,
                                                            Timestamp.Value,
                                                            this,
                                                            Id.ToString(),
                                                            EventTrackingId,
                                                            RoamingNetwork.Id,
                                                            OperatorId,
                                                            ChargingPoolId,
                                                            SessionId,
                                                            AuthIdentification,
                                                            RequestTimeout,
                                                            result,
                                                            Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingPoolStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #endregion

        #region SendChargeDetailRecords(ChargeDetailRecords, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Send charge detail records to an OICP server.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="TransmissionType">Whether to send the CDR directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<SendCDRsResult>

            ISendChargeDetailRecords.SendChargeDetailRecords(IEnumerable<WWCP.ChargeDetailRecord>  ChargeDetailRecords,
                                                             TransmissionTypes                     TransmissionType,

                                                             DateTime?                             Timestamp,
                                                             CancellationToken?                    CancellationToken,
                                                             EventTracking_Id                      EventTrackingId,
                                                             TimeSpan?                             RequestTimeout)

        {

            #region Initial checks

            if (ChargeDetailRecords == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecords),  "The given enumeration of charge detail records must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;


            DateTime        Endtime;
            TimeSpan        Runtime;
            SendCDRsResult  result;

            #endregion

            #region Send OnSendCDRsRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnSendCDRsRequest?.Invoke(StartTime,
                                          Timestamp.Value,
                                          this,
                                          Id.ToString(),
                                          EventTrackingId,
                                          RoamingNetwork.Id,
                                          new WWCP.ChargeDetailRecord[0],
                                          ChargeDetailRecords,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsRequest));
            }

            #endregion


            #region if disabled => 'AdminDown'...

            if (DisableSendChargeDetailRecords)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = SendCDRsResult.AdminDown(Id,
                                                    this,
                                                    ChargeDetailRecords,
                                                    Runtime: Runtime);

            }

            #endregion

            else
            {

                var LockTaken = await FlushOICPChargeDetailRecordsLock.WaitAsync(TimeSpan.FromSeconds(60));

                try
                {

                    if (LockTaken)
                    {

                        var SendCDRsResults = new List<SendCDRResult>();

                        #region if enqueuing is requested...

                        if (TransmissionType == TransmissionTypes.Enqueue)
                        {

                            #region Send OnEnqueueSendCDRRequest event

                            try
                            {

                                OnEnqueueSendCDRsRequest?.Invoke(DateTime.UtcNow,
                                                                 Timestamp.Value,
                                                                 this,
                                                                 Id.ToString(),
                                                                 EventTrackingId,
                                                                 RoamingNetwork.Id,
                                                                 new WWCP.ChargeDetailRecord[0],
                                                                 ChargeDetailRecords,
                                                                 RequestTimeout);

                            }
                            catch (Exception e)
                            {
                                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsRequest));
                            }

                            #endregion

                            foreach (var ChargeDetailRecord in ChargeDetailRecords)
                            {

                                try
                                {

                                    OICP_ChargeDetailRecords_Queue.Add(ChargeDetailRecord.ToOICP(_WWCPChargeDetailRecord2OICPChargeDetailRecord));
                                    SendCDRsResults.Add(new SendCDRResult(ChargeDetailRecord,
                                                                          SendCDRResultTypes.Enqueued));

                                }
                                catch (Exception e)
                                {
                                    SendCDRsResults.Add(new SendCDRResult(ChargeDetailRecord,
                                                                          SendCDRResultTypes.CouldNotConvertCDRFormat,
                                                                          e.Message));
                                }

                            }

                            Endtime  = DateTime.UtcNow;
                            Runtime  = Endtime - StartTime;
                            result   = SendCDRsResult.Enqueued(Id,
                                                               this,
                                                               ChargeDetailRecords,
                                                               "Enqueued for at least " + FlushChargeDetailRecordsEvery.TotalSeconds + " seconds!",
                                                               //SendCDRsResults.SafeWhere(cdrresult => cdrresult.Result != SendCDRResultTypes.Enqueued),
                                                               Runtime: Runtime);

                            FlushChargeDetailRecordsTimer.Change(FlushChargeDetailRecordsEvery, TimeSpan.FromMilliseconds(-1));

                        }

                        #endregion

                        #region ...or send at once!

                        else
                        {

                            HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>> response;

                            foreach (var ChargeDetailRecord in ChargeDetailRecords)
                            {

                                try
                                {

                                    response = await CPORoaming.SendChargeDetailRecord(ChargeDetailRecord.ToOICP(_WWCPChargeDetailRecord2OICPChargeDetailRecord),

                                                                                       Timestamp,
                                                                                       CancellationToken,
                                                                                       EventTrackingId,
                                                                                       RequestTimeout);

                                    if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                                        response.Content        != null              &&
                                        response.Content.Result)
                                    {
                                        SendCDRsResults.Add(new SendCDRResult(ChargeDetailRecord,
                                                                              SendCDRResultTypes.Success));
                                    }

                                    else
                                        SendCDRsResults.Add(new SendCDRResult(ChargeDetailRecord,
                                                                              SendCDRResultTypes.Error,
                                                                              response.HTTPBodyAsUTF8String));

                                }
                                catch (Exception e)
                                {
                                    SendCDRsResults.Add(new SendCDRResult(ChargeDetailRecord,
                                                                          SendCDRResultTypes.CouldNotConvertCDRFormat,
                                                                          e.Message));
                                }

                            }

                            Endtime  = DateTime.UtcNow;
                            Runtime  = Endtime - StartTime;

                            if      (SendCDRsResults.All(cdrresult => cdrresult.Result == SendCDRResultTypes.Success))
                                result = SendCDRsResult.Success(Id,
                                                                this,
                                                                ChargeDetailRecords,
                                                                Runtime: Runtime);

                            else
                                result = SendCDRsResult.Error(Id,
                                                              this,
                                                              SendCDRsResults.
                                                                  Where (cdrresult => cdrresult.Result != SendCDRResultTypes.Success).
                                                                  Select(cdrresult => cdrresult.ChargeDetailRecord),
                                                              Runtime: Runtime);


                        }

                        #endregion

                    }

                    #region Could not get the lock for toooo long!

                    else
                    {

                        Endtime  = DateTime.UtcNow;
                        Runtime  = Endtime - StartTime;
                        result   = SendCDRsResult.Timeout(Id,
                                                          this,
                                                          ChargeDetailRecords,
                                                          "Could not " + (TransmissionType == TransmissionTypes.Enqueue ? "enqueue" : "send") + " charge detail records!",
                                                          //ChargeDetailRecords.SafeSelect(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Timeout)),
                                                          Runtime: Runtime);

                    }

                    #endregion

                }
                finally
                {
                    if (LockTaken)
                        FlushOICPChargeDetailRecordsLock.Release();
                }

            }


            #region Send OnSendCDRsResponse event

            try
            {

                OnSendCDRsResponse?.Invoke(Endtime,
                                           Timestamp.Value,
                                           this,
                                           Id.ToString(),
                                           EventTrackingId,
                                           RoamingNetwork.Id,
                                           new WWCP.ChargeDetailRecord[0],
                                           ChargeDetailRecords,
                                           RequestTimeout,
                                           result,
                                           Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsResponse));
            }

            #endregion

            return result;

        }

        #endregion


        // -----------------------------------------------------------------------------------------------------


        #region (timer) FlushEVSEDataAndStatus()

        protected override Boolean SkipFlushEVSEDataAndStatusQueues()
            => EVSEsToAddQueue.              Count == 0 &&
               EVSEsToUpdateQueue.           Count == 0 &&
               EVSEStatusChangesDelayedQueue.Count == 0 &&
               EVSEsToRemoveQueue.           Count == 0;

        protected override async Task FlushEVSEDataAndStatusQueues()
        {

            #region Get a copy of all current EVSE data and delayed status

            var EVSEsToAddQueueCopy                = new HashSet<EVSE>();
            var EVSEsToUpdateQueueCopy             = new HashSet<EVSE>();
            var EVSEStatusChangesDelayedQueueCopy  = new List<EVSEStatusUpdate>();
            var EVSEsToRemoveQueueCopy             = new HashSet<EVSE>();
            var EVSEsUpdateLogCopy                 = new Dictionary<EVSE,            PropertyUpdateInfos[]>();
            var ChargingStationsUpdateLogCopy      = new Dictionary<ChargingStation, PropertyUpdateInfos[]>();
            var ChargingPoolsUpdateLogCopy         = new Dictionary<ChargingPool,    PropertyUpdateInfos[]>();

            await DataAndStatusLock.WaitAsync();

            try
            {

                // Copy 'EVSEs to add', remove originals...
                EVSEsToAddQueueCopy                      = new HashSet<EVSE>                (EVSEsToAddQueue);
                EVSEsToAddQueue.Clear();

                // Copy 'EVSEs to update', remove originals...
                EVSEsToUpdateQueueCopy                   = new HashSet<EVSE>                (EVSEsToUpdateQueue);
                EVSEsToUpdateQueue.Clear();

                // Copy 'EVSE status changes', remove originals...
                EVSEStatusChangesDelayedQueueCopy        = new List<EVSEStatusUpdate>       (EVSEStatusChangesDelayedQueue);
                EVSEStatusChangesDelayedQueueCopy.AddRange(EVSEsToAddQueueCopy.SafeSelect(evse => new EVSEStatusUpdate(evse, evse.Status, evse.Status)));
                EVSEStatusChangesDelayedQueue.Clear();

                // Copy 'EVSEs to remove', remove originals...
                EVSEsToRemoveQueueCopy                   = new HashSet<EVSE>                (EVSEsToRemoveQueue);
                EVSEsToRemoveQueue.Clear();

                // Copy EVSE property updates
                EVSEsUpdateLog.           ForEach(_ => EVSEsUpdateLogCopy.           Add(_.Key, _.Value.ToArray()));
                EVSEsUpdateLog.Clear();

                // Copy charging station property updates
                ChargingStationsUpdateLog.ForEach(_ => ChargingStationsUpdateLogCopy.Add(_.Key, _.Value.ToArray()));
                ChargingStationsUpdateLog.Clear();

                // Copy charging pool property updates
                ChargingPoolsUpdateLog.   ForEach(_ => ChargingPoolsUpdateLogCopy.   Add(_.Key, _.Value.ToArray()));
                ChargingPoolsUpdateLog.Clear();


                // Stop the timer. Will be rescheduled by next EVSE data/status change...
                FlushEVSEDataAndStatusTimer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));

            }
            finally
            {
                DataAndStatusLock.Release();
            }

            #endregion

            // Use events to check if something went wrong!
            var EventTrackingId = EventTracking_Id.New;

            //Thread.Sleep(30000);

            #region Send new EVSE data

            if (EVSEsToAddQueueCopy.Count > 0)
            {

                var EVSEsToAddTask = PushEVSEData(EVSEsToAddQueueCopy,
                                                  _FlushEVSEDataRunId == 1
                                                      ? ActionTypes.fullLoad
                                                      : ActionTypes.update,
                                                  EventTrackingId: EventTrackingId);

                EVSEsToAddTask.Wait();

                if (EVSEsToAddTask.Result.Warnings.Any())
                {

                    SendOnWarnings(DateTime.UtcNow,
                                   nameof(WWCPCPOAdapter) + Id,
                                   "EVSEsToAddTask",
                                   EVSEsToAddTask.Result.Warnings);

                }

            }

            #endregion

            #region Send changed EVSE data

            if (EVSEsToUpdateQueueCopy.Count > 0)
            {

                // Surpress EVSE data updates for all newly added EVSEs
                var EVSEsWithoutNewEVSEs = EVSEsToUpdateQueueCopy.
                                               Where(evse => !EVSEsToAddQueueCopy.Contains(evse)).
                                               ToArray();


                if (EVSEsWithoutNewEVSEs.Length > 0)
                {

                    var PushEVSEDataTask = PushEVSEData(EVSEsWithoutNewEVSEs,
                                                        ActionTypes.update,
                                                        EventTrackingId: EventTrackingId);

                    PushEVSEDataTask.Wait();

                    if (PushEVSEDataTask.Result.Warnings.Any())
                    {

                        SendOnWarnings(DateTime.UtcNow,
                                       nameof(WWCPCPOAdapter) + Id,
                                       "PushEVSEDataTask",
                                       PushEVSEDataTask.Result.Warnings);

                    }

                }

            }

            #endregion

            #region Send changed EVSE status

            if (!DisablePushStatus &&
                EVSEStatusChangesDelayedQueueCopy.Count > 0)
            {

                var PushEVSEStatusTask = PushEVSEStatus(EVSEStatusChangesDelayedQueueCopy,
                                                        _FlushEVSEDataRunId == 1
                                                            ? ActionTypes.fullLoad
                                                            : ActionTypes.update,
                                                        EventTrackingId: EventTrackingId);

                PushEVSEStatusTask.Wait();

                if (PushEVSEStatusTask.Result.Warnings.Any())
                {

                    SendOnWarnings(DateTime.UtcNow,
                                   nameof(WWCPCPOAdapter) + Id,
                                   "PushEVSEStatusTask",
                                   PushEVSEStatusTask.Result.Warnings);

                }

            }

            #endregion

            #region Send removed charging stations

            if (EVSEsToRemoveQueueCopy.Count > 0)
            {

                var EVSEsToRemove = EVSEsToRemoveQueueCopy.ToArray();

                if (EVSEsToRemove.Length > 0)
                {

                    var EVSEsToRemoveTask = PushEVSEData(EVSEsToRemove,
                                                         ActionTypes.delete,
                                                         EventTrackingId: EventTrackingId);

                    EVSEsToRemoveTask.Wait();

                    if (EVSEsToRemoveTask.Result.Warnings.Any())
                    {

                        SendOnWarnings(DateTime.UtcNow,
                                       nameof(WWCPCPOAdapter) + Id,
                                       "EVSEsToRemoveTask",
                                       EVSEsToRemoveTask.Result.Warnings);

                    }

                }

            }

            #endregion

        }

        #endregion

        #region (timer) FlushEVSEFastStatus()

        protected override Boolean SkipFlushEVSEFastStatusQueues()
            => EVSEStatusChangesFastQueue.Count == 0;

        protected override async Task FlushEVSEFastStatusQueues()
        {

            #region Get a copy of all current EVSE data and delayed status

            var EVSEStatusFastQueueCopy = new List<EVSEStatusUpdate>();

            await DataAndStatusLock.WaitAsync();

            try
            {

                // Copy 'EVSE status changes', remove originals...
                EVSEStatusFastQueueCopy = new List<EVSEStatusUpdate>(EVSEStatusChangesFastQueue.Where(evsestatuschange => !EVSEsToAddQueue.Any(evse => evse == evsestatuschange.EVSE)));

                // Add all evse status changes of EVSE *NOT YET UPLOADED* into the delayed queue...
                var EVSEStatusChangesDelayed = EVSEStatusChangesFastQueue.Where(evsestatuschange => EVSEsToAddQueue.Any(evse => evse == evsestatuschange.EVSE)).ToArray();

                if (EVSEStatusChangesDelayed.Length > 0)
                    EVSEStatusChangesDelayedQueue.AddRange(EVSEStatusChangesDelayed);

                EVSEStatusChangesFastQueue.Clear();

                // Stop the timer. Will be rescheduled by next EVSE status change...
                FlushEVSEFastStatusTimer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));

            }
            finally
            {
                DataAndStatusLock.Release();
            }

            #endregion

            // Use events to check if something went wrong!
            var EventTrackingId = EventTracking_Id.New;

            #region Send changed EVSE status

            if (EVSEStatusFastQueueCopy.Count > 0)
            {

                var _PushEVSEStatus = await PushEVSEStatus(EVSEStatusFastQueueCopy,
                                                           ActionTypes.update,
                                                           EventTrackingId: EventTrackingId);

                if (_PushEVSEStatus.Warnings.Any())
                {

                    SendOnWarnings(DateTime.UtcNow,
                                   nameof(WWCPCPOAdapter) + Id,
                                   "PushEVSEFastStatus",
                                   _PushEVSEStatus.Warnings);

                }

            }

            #endregion

        }

        #endregion

        #region (timer) FlushChargeDetailRecords()

        protected override Boolean SkipFlushChargeDetailRecordsQueues()
            => OICP_ChargeDetailRecords_Queue.Count == 0;

        protected override async Task FlushChargeDetailRecordsQueues()
        {

            #region Make a thread local copy of all data

            var LockTaken                    = await FlushOICPChargeDetailRecordsLock.WaitAsync(TimeSpan.FromSeconds(30));
            var ChargeDetailRecordQueueCopy  = new List<ChargeDetailRecord>();

            try
            {

                if (LockTaken)
                {

                    // Copy CDRs, empty original queue...
                    ChargeDetailRecordQueueCopy.AddRange(OICP_ChargeDetailRecords_Queue);
                    OICP_ChargeDetailRecords_Queue.Clear();

                    //// Stop the timer. Will be rescheduled by the next CDR...
                    //FlushChargeDetailRecordsTimer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));

                }

            }
            catch (Exception e)
            {

                while (e.InnerException != null)
                    e = e.InnerException;

                DebugX.LogT(nameof(WWCPCPOAdapter) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

            }

            finally
            {
                if (LockTaken)
                    FlushOICPChargeDetailRecordsLock.Release();
            }

            #endregion

            // Use the events to evaluate if something went wrong!

            #region Send charge detail records

            if (ChargeDetailRecordQueueCopy.Count > 0)
            {

                var EventTrackingId  = EventTracking_Id.New;
                var results          = new List<HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>>>();

                foreach (var chargedetailrecord in ChargeDetailRecordQueueCopy)
                    results.Add(await CPORoaming.SendChargeDetailRecord(chargedetailrecord,
                                                                        DateTime.UtcNow,
                                                                        new CancellationTokenSource().Token,
                                                                        EventTrackingId,
                                                                        DefaultRequestTimeout));

                //var Warnings         = results.Where(result => result.Content

            }

            #endregion

            //ToDo: Send FlushChargeDetailRecordsQueues result event...
            //ToDo: Re-add to queue if it could not be send...

        }

        #endregion


        // -----------------------------------------------------------------------------------------------------


        #region Operator overloading

        #region Operator == (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two WWCPCPOAdapters for equality.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (WWCPCPOAdapter WWCPCPOAdapter1, WWCPCPOAdapter WWCPCPOAdapter2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(WWCPCPOAdapter1, WWCPCPOAdapter2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) WWCPCPOAdapter1 == null) || ((Object) WWCPCPOAdapter2 == null))
                return false;

            return WWCPCPOAdapter1.Equals(WWCPCPOAdapter2);

        }

        #endregion

        #region Operator != (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two WWCPCPOAdapters for inequality.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (WWCPCPOAdapter WWCPCPOAdapter1, WWCPCPOAdapter WWCPCPOAdapter2)

            => !(WWCPCPOAdapter1 == WWCPCPOAdapter2);

        #endregion

        #region Operator <  (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (WWCPCPOAdapter  WWCPCPOAdapter1,
                                          WWCPCPOAdapter  WWCPCPOAdapter2)
        {

            if ((Object) WWCPCPOAdapter1 == null)
                throw new ArgumentNullException(nameof(WWCPCPOAdapter1),  "The given WWCPCPOAdapter must not be null!");

            return WWCPCPOAdapter1.CompareTo(WWCPCPOAdapter2) < 0;

        }

        #endregion

        #region Operator <= (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (WWCPCPOAdapter WWCPCPOAdapter1,
                                           WWCPCPOAdapter WWCPCPOAdapter2)

            => !(WWCPCPOAdapter1 > WWCPCPOAdapter2);

        #endregion

        #region Operator >  (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (WWCPCPOAdapter WWCPCPOAdapter1,
                                          WWCPCPOAdapter WWCPCPOAdapter2)
        {

            if ((Object) WWCPCPOAdapter1 == null)
                throw new ArgumentNullException(nameof(WWCPCPOAdapter1),  "The given WWCPCPOAdapter must not be null!");

            return WWCPCPOAdapter1.CompareTo(WWCPCPOAdapter2) > 0;

        }

        #endregion

        #region Operator >= (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (WWCPCPOAdapter WWCPCPOAdapter1,
                                           WWCPCPOAdapter WWCPCPOAdapter2)

            => !(WWCPCPOAdapter1 < WWCPCPOAdapter2);

        #endregion

        #endregion

        #region IComparable<WWCPCPOAdapter> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var WWCPCPOAdapter = Object as WWCPCPOAdapter;
            if ((Object) WWCPCPOAdapter == null)
                throw new ArgumentException("The given object is not an WWCPCPOAdapter!", nameof(Object));

            return CompareTo(WWCPCPOAdapter);

        }

        #endregion

        #region CompareTo(WWCPCPOAdapter)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter">An WWCPCPOAdapter object to compare with.</param>
        public Int32 CompareTo(WWCPCPOAdapter WWCPCPOAdapter)
        {

            if ((Object) WWCPCPOAdapter == null)
                throw new ArgumentNullException(nameof(WWCPCPOAdapter), "The given WWCPCPOAdapter must not be null!");

            return Id.CompareTo(WWCPCPOAdapter.Id);

        }

        #endregion

        #endregion

        #region IEquatable<WWCPCPOAdapter> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            var WWCPCPOAdapter = Object as WWCPCPOAdapter;
            if ((Object) WWCPCPOAdapter == null)
                return false;

            return Equals(WWCPCPOAdapter);

        }

        #endregion

        #region Equals(WWCPCPOAdapter)

        /// <summary>
        /// Compares two WWCPCPOAdapter for equality.
        /// </summary>
        /// <param name="WWCPCPOAdapter">An WWCPCPOAdapter to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(WWCPCPOAdapter WWCPCPOAdapter)
        {

            if ((Object) WWCPCPOAdapter == null)
                return false;

            return Id.Equals(WWCPCPOAdapter.Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => Id.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => "OICP" + Version.Number + " CPO Adapter " + Id;

        #endregion


    }

}
