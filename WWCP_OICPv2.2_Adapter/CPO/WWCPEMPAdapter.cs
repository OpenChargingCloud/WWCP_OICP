/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using Org.BouncyCastle.Ocsp;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.CPO
{

    /// <summary>
    /// A WWCP wrapper for the OICP CPO Roaming client which maps
    /// WWCP data structures onto OICP data structures and vice versa.
    /// </summary>
    public class WWCPEMPAdapter : AWWCPEMPAdapter<ChargeDetailRecord>,
                                  IEMPRoamingProvider,
                                  IEquatable <WWCPEMPAdapter>,
                                  IComparable<WWCPEMPAdapter>,
                                  IComparable
    {

        #region Data

        private        readonly  EVSE2EVSEDataRecordDelegate                          _EVSE2EVSEDataRecord;

        private        readonly  EVSEStatusUpdate2EVSEStatusRecordDelegate            _EVSEStatusUpdate2EVSEStatusRecord;

        private        readonly  WWCPChargeDetailRecord2ChargeDetailRecordDelegate    _WWCPChargeDetailRecord2OICPChargeDetailRecord;

        private        readonly  ChargingStationOperatorNameSelectorDelegate          _OperatorNameSelector;

        private static readonly  Regex                                                pattern                             = new Regex(@"\s=\s");

        public  static readonly  ChargingStationOperatorNameSelectorDelegate          DefaultOperatorNameSelector         = I18N => I18N.FirstText();

        #endregion

        #region Properties

        IId IAuthorizeStartStop.AuthId
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
        public CPOSOAPServer CPOServer
            => CPORoaming?.CPOServer;

        /// <summary>
        /// The CPO server logger.
        /// </summary>
        public CPOServerLogger ServerLogger
            => CPORoaming?.CPOServerLogger;


        /// <summary>
        /// An optional default charging station operator.
        /// </summary>
        public ChargingStationOperator  DefaultOperator     { get; }


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
        public event OnAuthorizeStartRequestDelegate   OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate  OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate  OnAuthorizeStopResponse;

        #endregion

        #region OnSendCDRsRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record was enqueued for later sending upstream.
        /// </summary>
        public event OnSendCDRsRequestDelegate   OnEnqueueSendCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record will be send upstream.
        /// </summary>
        public event OnSendCDRsRequestDelegate   OnSendCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record had been sent upstream.
        /// </summary>
        public event OnSendCDRsResponseDelegate  OnSendCDRsResponse;

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

        #region WWCPEMPAdapter(Id, Name, RoamingNetwork, CPORoaming, EVSE2EVSEDataRecord = null)

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
        /// <param name="DNSClient">The attached DNS service.</param>
        public WWCPEMPAdapter(EMPRoamingProvider_Id                              Id,
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
                              ChargeDetailRecordFilterDelegate                   ChargeDetailRecordFilter                        = null,

                              CustomEVSEIdMapperDelegate                         CustomEVSEIdMapper                              = null,

                              TimeSpan?                                          ServiceCheckEvery                               = null,
                              TimeSpan?                                          StatusCheckEvery                                = null,
                              TimeSpan?                                          CDRCheckEvery                                   = null,

                              Boolean                                            DisablePushData                                 = false,
                              Boolean                                            DisablePushStatus                               = false,
                              Boolean                                            DisableAuthentication                           = false,
                              Boolean                                            DisableSendChargeDetailRecords                  = false,

                              String                                             EllipticCurve                                   = "P-256",
                              ECPrivateKeyParameters                             PrivateKey                                      = null,
                              PublicKeyCertificates                              PublicKeyCertificates                           = null,
                              DNSClient                                          DNSClient                                       = null)

            : base(Id,
                   Name,
                   Description,
                   RoamingNetwork,

                   IncludeEVSEIds,
                   IncludeEVSEs,
                   null,
                   null,
                   null,
                   null,
                   ChargeDetailRecordFilter,
                   //CustomEVSEIdMapper,

                   ServiceCheckEvery,
                   StatusCheckEvery,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushStatus,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords,

                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,

                   DNSClient ?? CPORoaming?.DNSClient)

        {

            this.CPORoaming                                       = CPORoaming      ?? throw new ArgumentNullException(nameof(CPORoaming),      "The given CPO roaming object must not be null!");
            this._EVSE2EVSEDataRecord                             = EVSE2EVSEDataRecord;
            this._EVSEStatusUpdate2EVSEStatusRecord               = EVSEStatusUpdate2EVSEStatusRecord;
            this._WWCPChargeDetailRecord2OICPChargeDetailRecord   = WWCPChargeDetailRecord2OICPChargeDetailRecord;

            this.DefaultOperator                                  = DefaultOperator ?? throw new ArgumentNullException(nameof(DefaultOperator), "The given charging station operator must not be null!");
            this.DefaultOperatorIdFormat                          = DefaultOperatorIdFormat;
            this.DefaultOperatorName                              = DefaultOperatorNameSelector(DefaultOperator.Name);
            this._OperatorNameSelector                            = OperatorNameSelector;

            this.CustomEVSEIdMapper                               = CustomEVSEIdMapper;


            // Link incoming OICP events...

            #region OnAuthorizeRemoteReservationStart

            this.CPORoaming.OnAuthorizeRemoteReservationStart += async (Timestamp,
                                                                        Sender,
                                                                        Request) => {


                #region Request transformation

                TimeSpan?           Duration           = null;
                DateTime?           ReservationStartTime          = null;
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
                            ReservationStartTime = DateTime.Parse(StartTimeText.Substring(2));
                        }

                    }

                }

                #endregion

                var response = await RoamingNetwork.
                                         Reserve(ChargingLocation.FromEVSEId(Request.EVSEId.ToWWCP().Value),
                                                 ReservationStartTime:  ReservationStartTime,
                                                 Duration:              Duration,

                                                 // Always create a reservation identification usable for OICP!
                                                 ReservationId:         ChargingReservation_Id.Parse(
                                                                            Request.EVSEId.OperatorId.ToWWCP().Value,
                                                                            Request.SessionId.HasValue
                                                                                ? Request.SessionId.ToString()
                                                                                : Session_Id.NewRandom.ToString()
                                                                        ),

                                                 ProviderId:            Request.ProviderId.    ToWWCP(),
                                                 RemoteAuthentication:  Request.Identification.ToWWCP(),
                                                 ChargingProduct:       PartnerProductId.HasValue
                                                                            ? new ChargingProduct(PartnerProductId.Value.ToWWCP())
                                                                            : null,

                                                 eMAIds:                Request?.Identification?.RemoteIdentification           != null &&
                                                                        Request?.Identification?.RemoteIdentification?.ToWWCP() != null
                                                                            ? new eMobilityAccount_Id[] {
                                                                                  Request.Identification.RemoteIdentification.ToWWCP().Value
                                                                              }
                                                                            : null,

                                                 Timestamp:             Request.Timestamp,
                                                 CancellationToken:     Request.CancellationToken,
                                                 EventTrackingId:       Request.EventTrackingId,
                                                 RequestTimeout:        Request.RequestTimeout).
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

                        case ReservationResultType.UnknownLocation:
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
                                                           //Request.ProviderId.ToWWCP(),
                                                           //Request.EVSEId.    ToWWCP(),

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

                        case CancelReservationResultTypes.Success:
                            return Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.Success(
                                       Request,
                                       StatusCodeDescription: "Reservation deleted!"
                                   );

                        case CancelReservationResultTypes.UnknownReservationId:
                            return Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.SessionIsInvalid(
                                       Request,
                                       SessionId: Request.SessionId
                                   );

                        case CancelReservationResultTypes.Offline:
                        case CancelReservationResultTypes.Timeout:
                        case CancelReservationResultTypes.CommunicationError:
                            return Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.CommunicationToEVSEFailed(Request);

                        case CancelReservationResultTypes.UnknownEVSE:
                            return Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.UnknownEVSEID(Request);

                        case CancelReservationResultTypes.OutOfService:
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

                try
                {

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
                                             RemoteStart(EMPRoamingProvider:    this,
                                                         ChargingLocation:      ChargingLocation.FromEVSEId(Request.EVSEId.ToWWCP().Value),
                                                         ChargingProduct:       ChargingProduct,
                                                         ReservationId:         ReservationId,
                                                         SessionId:             Request.SessionId.     ToWWCP(),
                                                         ProviderId:            Request.ProviderId.    ToWWCP(),
                                                         RemoteAuthentication:  Request.Identification.ToWWCP(),

                                                         Timestamp:             Request.Timestamp,
                                                         CancellationToken:     Request.CancellationToken,
                                                         EventTrackingId:       Request.EventTrackingId,
                                                         RequestTimeout:        Request.RequestTimeout).
                                             ConfigureAwait(false);

                    #region Response mapping

                    if (response != null)
                    {
                        switch (response.Result)
                        {

                            case RemoteStartResultTypes.Success:
                                return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.Success(
                                           Request:                   Request,
                                           SessionId:                 response.Session.Id.ToOICP(),
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : "Ready to charge!",
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case RemoteStartResultTypes.AsyncOperation:
                                return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.Success(
                                           Request:                   Request,
                                           SessionId:                 response.Session.Id.ToOICP(),
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : "Ready to charge (async)!",
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case RemoteStartResultTypes.InvalidSessionId:
                                return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.SessionIsInvalid(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case RemoteStartResultTypes.InvalidCredentials:
                                return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.NoValidContract(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case RemoteStartResultTypes.NoEVConnectedToEVSE:
                                return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.NoEVConnectedToEVSE(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case RemoteStartResultTypes.Offline:
                            case RemoteStartResultTypes.Timeout:
                            case RemoteStartResultTypes.CommunicationError:
                                return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.CommunicationToEVSEFailed(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case RemoteStartResultTypes.Reserved:
                                return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.EVSEAlreadyReserved(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case RemoteStartResultTypes.AlreadyInUse:
                                return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.EVSEAlreadyInUse_WrongToken(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case RemoteStartResultTypes.UnknownLocation:
                                return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.UnknownEVSEID(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case RemoteStartResultTypes.OutOfService:
                                return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.EVSEOutOfService(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            default:
                                return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.ServiceNotAvailable(
                                    Request:                   Request,
                                    StatusCodeAdditionalInfo:  "Unknown WWCP RemoteStart result: " + response.Result.ToString() + "\n" +
                                                               response.Description + "\n" +
                                                               response.AdditionalInfo,
                                    SessionId:                 Request.SessionId,
                                    EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                    CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                );

                        }
                    }

                    return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.ServiceNotAvailable(
                               Request:                   Request,
                               StatusCodeAdditionalInfo:  "Invalid WWCP RemoteStart result!",
                               SessionId:                 Request.SessionId,
                               EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                               CPOPartnerSessionId:       Request.CPOPartnerSessionId
                           );

                    #endregion

                }
                catch (Exception e)
                {
                    return Acknowledgement<EMP.AuthorizeRemoteStartRequest>.ServiceNotAvailable(
                        Request:                   Request,
                        StatusCodeAdditionalInfo:  e.Message + "\n" + e.StackTrace,
                        SessionId:                 Request.SessionId
                    );
                }

            };

            #endregion

            #region OnAuthorizeRemoteStop

            this.CPORoaming.OnAuthorizeRemoteStop += async (Timestamp,
                                                            Sender,
                                                            Request) => {

                try
                {

                    var response = await RoamingNetwork.
                                             RemoteStop(EMPRoamingProvider:    this,
                                                        //Request.EVSEId.     ToWWCP().Value,
                                                        SessionId:             Request.SessionId.ToWWCP(),
                                                        ReservationHandling:   ReservationHandling.Close,
                                                        ProviderId:            Request.ProviderId.ToWWCP(),
                                                        RemoteAuthentication:  null,

                                                        Timestamp:             Request.Timestamp,
                                                        CancellationToken:     Request.CancellationToken,
                                                        EventTrackingId:       Request.EventTrackingId,
                                                        RequestTimeout:        Request.RequestTimeout).
                                             ConfigureAwait(false);

                    #region Response mapping

                    if (response != null)
                    {
                        switch (response.Result)
                        {

                            case RemoteStopResultTypes.Success:
                                return Acknowledgement<EMP.AuthorizeRemoteStopRequest>.Success(
                                           Request:                   Request,
                                           SessionId:                 response.SessionId.ToOICP(),
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : "Ready to stop charging!",
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case RemoteStopResultTypes.AsyncOperation:
                                return Acknowledgement<EMP.AuthorizeRemoteStopRequest>.Success(
                                           Request:                   Request,
                                           SessionId:                 response.SessionId.ToOICP(),
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : "Ready to stop charging (async)!",
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case RemoteStopResultTypes.InvalidSessionId:
                                return Acknowledgement<EMP.AuthorizeRemoteStopRequest>.SessionIsInvalid(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case RemoteStopResultTypes.Offline:
                            case RemoteStopResultTypes.Timeout:
                            case RemoteStopResultTypes.CommunicationError:
                                return Acknowledgement<EMP.AuthorizeRemoteStopRequest>.CommunicationToEVSEFailed(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case RemoteStopResultTypes.UnknownLocation:
                                return Acknowledgement<EMP.AuthorizeRemoteStopRequest>.UnknownEVSEID(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case RemoteStopResultTypes.OutOfService:
                                return Acknowledgement<EMP.AuthorizeRemoteStopRequest>.EVSEOutOfService(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            default:
                                return Acknowledgement<EMP.AuthorizeRemoteStopRequest>.ServiceNotAvailable(
                                    Request:                   Request,
                                    StatusCodeAdditionalInfo:  "Unknown WWCP RemoteStop result: " + response.Result.ToString() + "\n" +
                                                               response.Description + "\n" +
                                                               response.AdditionalInfo,
                                    SessionId:                 Request.SessionId,
                                    EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                    CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                );

                        }
                    }

                    return Acknowledgement<EMP.AuthorizeRemoteStopRequest>.ServiceNotAvailable(
                               Request:                   Request,
                               StatusCodeAdditionalInfo:  "Invalid WWCP RemoteStop result!",
                               SessionId:                 Request.SessionId,
                               EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                               CPOPartnerSessionId:       Request.CPOPartnerSessionId
                           );

                    #endregion

                }
                catch (Exception e)
                {
                    return Acknowledgement<EMP.AuthorizeRemoteStopRequest>.ServiceNotAvailable(
                        Request:                   Request,
                        StatusCodeAdditionalInfo:  e.Message + "\n" + e.StackTrace,
                        SessionId:                 Request.SessionId
                    );
                }

            };

            #endregion

        }

        #endregion

        #region WWCPEMPAdapter(Id, Name, RoamingNetwork, CPOClient, CPOServer, EVSEDataRecordProcessing = null)

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
        public WWCPEMPAdapter(EMPRoamingProvider_Id                              Id,
                              I18NString                                         Name,
                              I18NString                                         Description,
                              RoamingNetwork                                     RoamingNetwork,

                              CPOClient                                          CPOClient,
                              CPOSOAPServer                                          CPOServer,
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
                              ChargeDetailRecordFilterDelegate                   ChargeDetailRecordFilter                        = null,
                              CustomEVSEIdMapperDelegate                         CustomEVSEIdMapper                              = null,

                              TimeSpan?                                          ServiceCheckEvery                               = null,
                              TimeSpan?                                          StatusCheckEvery                                = null,
                              TimeSpan?                                          CDRCheckEvery                                   = null,

                              Boolean                                            DisablePushData                                 = false,
                              Boolean                                            DisablePushStatus                               = false,
                              Boolean                                            DisableAuthentication                           = false,
                              Boolean                                            DisableSendChargeDetailRecords                  = false,

                              String                                             EllipticCurve                                   = "P-256",
                              ECPrivateKeyParameters                             PrivateKey                                      = null,
                              PublicKeyCertificates                              PublicKeyCertificates                           = null,

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
                   ChargeDetailRecordFilter,
                   CustomEVSEIdMapper,

                   ServiceCheckEvery,
                   StatusCheckEvery,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushStatus,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords,

                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,

                   DNSClient ?? CPOServer?.DNSClient)

        { }

        #endregion

        #region WWCPEMPAdapter(Id, Name, RoamingNetwork, RemoteHostName, ...)

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
        /// <param name="ServiceName">An optional identification for this SOAP service.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURLPrefix">An optional prefix for the HTTP URLs.</param>
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
        public WWCPEMPAdapter(EMPRoamingProvider_Id                              Id,
                              I18NString                                         Name,
                              I18NString                                         Description,
                              RoamingNetwork                                     RoamingNetwork,

                              HTTPHostname                                       RemoteHostname,
                              IPPort?                                            RemoteTCPPort                                   = null,
                              RemoteCertificateValidationCallback                RemoteCertificateValidator                      = null,
                              LocalCertificateSelectionCallback                  ClientCertificateSelector                       = null,
                              HTTPHostname?                                      RemoteHTTPVirtualHost                           = null,
                              HTTPPath?                                          URLPrefix                                       = null,
                              String                                             EVSEDataURL                                     = CPOClient.DefaultEVSEDataURL,
                              String                                             EVSEStatusURL                                   = CPOClient.DefaultEVSEStatusURL,
                              String                                             AuthorizationURL                                = CPOClient.DefaultAuthorizationURL,
                              String                                             AuthenticationDataURL                           = CPOClient.DefaultAuthenticationDataURL,
                              String                                             HTTPUserAgent                                   = CPOClient.DefaultHTTPUserAgent,
                              TimeSpan?                                          RequestTimeout                                  = null,
                              Byte?                                              MaxNumberOfRetries                              = CPOClient.DefaultMaxNumberOfRetries,

                              String                                             ServerName                                      = CPOSOAPServer.DefaultHTTPServerName,
                              String                                             ServiceName                                       = null,
                              IPPort?                                            ServerTCPPort                                   = null,
                              HTTPPath?                                          ServerURLPrefix                                 = null,
                              String                                             ServerAuthorizationURL                          = CPOSOAPServer.DefaultAuthorizationURL,
                              String                                             ServerReservationURL                            = CPOSOAPServer.DefaultReservationURL,
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
                              ChargeDetailRecordFilterDelegate                   ChargeDetailRecordFilter                        = null,
                              CustomEVSEIdMapperDelegate                         CustomEVSEIdMapper                              = null,

                              TimeSpan?                                          ServiceCheckEvery                               = null,
                              TimeSpan?                                          StatusCheckEvery                                = null,
                              TimeSpan?                                          CDRCheckEvery                                   = null,

                              Boolean                                            DisablePushData                                 = false,
                              Boolean                                            DisablePushStatus                               = false,
                              Boolean                                            DisableAuthentication                           = false,
                              Boolean                                            DisableSendChargeDetailRecords                  = false,

                              String                                             EllipticCurve                                   = "P-256",
                              ECPrivateKeyParameters                             PrivateKey                                      = null,
                              PublicKeyCertificates                              PublicKeyCertificates                           = null,

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
                                  URLPrefix ?? CPOClient.DefaultURLPrefix,
                                  EVSEDataURL,
                                  EVSEStatusURL,
                                  AuthorizationURL,
                                  AuthenticationDataURL,
                                  HTTPUserAgent,
                                  RequestTimeout,
                                  MaxNumberOfRetries,

                                  ServerName,
                                  ServerTCPPort,
                                  ServiceName,
                                  ServerURLPrefix ?? CPOSOAPServer.DefaultURLPathPrefix,
                                  ServerAuthorizationURL,
                                  ServerReservationURL,
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
                   ChargeDetailRecordFilter,
                   CustomEVSEIdMapper,

                   ServiceCheckEvery,
                   StatusCheckEvery,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushStatus,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords,

                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,

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
            {
                foreach (var evse in EVSEs)
                {

                    try
                    {

                        if (evse == null)
                            continue;

                        if (IncludeEVSEs(evse) && IncludeEVSEIds(evse.Id))
                            // WWCP EVSE will be added as custom data "WWCP.EVSE"...
                            EVSEDataRecords.Add(evse.ToOICP(_EVSE2EVSEDataRecord));

                        else
                            DebugX.Log(evse.Id + " was filtered!");

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e.Message);
                        Warnings.Add(Warning.Create(I18NString.Create(Languages.en, e.Message), evse));
                    }

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
                                                  Warnings.Where(warning => warning.IsNeitherNullNorEmpty()),
                                                  RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnPushEVSEDataWWCPRequest));
            }

            #endregion

            DateTime Endtime;
            TimeSpan Runtime;

            if (EVSEDataRecords.Count > 0)
            {

                var response = await CPORoaming.
                                         PushEVSEData(EVSEDataRecords,
                                                      DefaultOperator.Id.ToOICP(DefaultOperatorIdFormat),
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
                                                                  ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.Content.StatusCode.AdditionalInfo))
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

                            Warnings.Add(Warning.Create(I18NString.Create(Languages.en, ServerAction.ToString() + " of " + EVSEDataRecords.Count + " EVSEs failed!")));
                            Warnings.Add(Warning.Create(I18NString.Create(Languages.en, response.Content.StatusCode.Code.ToString())));
                            Warnings.Add(Warning.Create(I18NString.Create(Languages.en, response.Content.StatusCode.Description)));

                            if (response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty())
                                Warnings.Add(Warning.Create(I18NString.Create(Languages.en, response.Content.StatusCode.AdditionalInfo)));

                            Warnings.Add(Warning.Create(I18NString.Create(Languages.en, "Will try to fix this issue via a 'fullLoad' of all EVSEs!")));

                            #endregion

                            #region Get all EVSEs from the roaming network

                            var FullLoadEVSEs = RoamingNetwork.EVSEs.Where(evse => evse != null &&
                                                                           IncludeEVSEs(evse) &&
                                                                           IncludeEVSEIds(evse.Id)).
                                                        Select(evse =>
                                                        {

                                                            try
                                                            {

                                                                return evse.ToOICP(_EVSE2EVSEDataRecord);

                                                            }
                                                            catch (Exception e)
                                                            {
                                                                DebugX.Log(e.Message);
                                                                Warnings.Add(Warning.Create(I18NString.Create(Languages.en, e.Message), evse));
                                                            }

                                                            return null;

                                                        }).
                                                        Where(evsedatarecord => evsedatarecord != null).
                                                        ToArray();

                            #endregion

                            #region Send request

                            var FullLoadResponse = await CPORoaming.
                                                              PushEVSEData(FullLoadEVSEs,
                                                                           DefaultOperator.Id.ToOICP(DefaultOperatorIdFormat),
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
                                                                        ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, FullLoadResponse.Content.StatusCode.AdditionalInfo))
                                                                        : Warnings,
                                                                    Runtime);

                                else
                                    result = PushEVSEDataResult.Error(Id,
                                                                  this,
                                                                  EVSEs,
                                                                  FullLoadResponse.Content.StatusCode.Description,
                                                                  FullLoadResponse.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                      ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, FullLoadResponse.Content.StatusCode.AdditionalInfo))
                                                                      : Warnings,
                                                                  Runtime);

                            }

                            else
                                result = PushEVSEDataResult.Error(Id,
                                                              this,
                                                              EVSEs,
                                                              FullLoadResponse.HTTPStatusCode.ToString(),
                                                              FullLoadResponse.HTTPBody != null
                                                                  ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, FullLoadResponse.HTTPBody.ToUTF8String()))
                                                                  : Warnings.AddAndReturnList(I18NString.Create(Languages.en, "No HTTP body received!")),
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
                                                                    ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.HTTPBody.ToUTF8String()))
                                                                    : Warnings.AddAndReturnList(I18NString.Create(Languages.en, "No HTTP body received!")),
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
                                                            ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.HTTPBody.ToUTF8String()))
                                                            : Warnings.AddAndReturnList(I18NString.Create(Languages.en, "No HTTP body received!")),
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
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnPushEVSEDataWWCPResponse));
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
                                  Where       (evsestatusupdate => IncludeEVSEs  (evsestatusupdate.EVSE) &&
                                                                   IncludeEVSEIds(evsestatusupdate.EVSE.Id)).
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
                                                     _EVSEId.Value,
                                                     evsestatusupdate.Value.First().NewStatus.Value.AsOICPEVSEStatus()
                                                 );

                                      }
                                      catch (Exception e)
                                      {
                                          DebugX.  Log(e.Message);
                                          Warnings.Add(Warning.Create(I18NString.Create(Languages.en, e.Message), evsestatusupdate));
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
                                                    Warnings.Where(warning => warning.IsNeitherNullNorEmpty()),
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnPushEVSEStatusWWCPRequest));
            }

            #endregion


            var response = await CPORoaming.
                                     PushEVSEStatus(_EVSEStatus,
                                                    DefaultOperator.Id.ToOICP(DefaultOperatorIdFormat),
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
                                                          ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.Content.StatusCode.AdditionalInfo))
                                                          : Warnings,
                                                      Runtime);

                else
                    result = PushEVSEStatusResult.Error(Id,
                                                    this,
                                                    EVSEStatusUpdates,
                                                    response.Content.StatusCode.Description,
                                                    response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                        ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.Content.StatusCode.AdditionalInfo))
                                                        : Warnings,
                                                    Runtime);

            }
            else
                result = PushEVSEStatusResult.Error(Id,
                                                this,
                                                EVSEStatusUpdates,
                                                response.HTTPStatusCode.ToString(),
                                                response.HTTPBody != null
                                                    ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.HTTPBody.ToUTF8String()))
                                                    : Warnings.AddAndReturnList(I18NString.Create(Languages.en, "No HTTP body received!")),
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
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnPushEVSEStatusWWCPResponse));
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

            ISendPOIData.SetStaticData(EVSE                EVSE,
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
                //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken &&
                       (IncludeEVSEs == null || IncludeEVSEs(EVSE)))
                    {

                        EVSEsToAddQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                    }

                }
                finally
                {
                    if (LockTaken)
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

            ISendPOIData.AddStaticData(EVSE                EVSE,
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
                //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken &&
                        (IncludeEVSEs == null || IncludeEVSEs(EVSE)))
                    {

                        EVSEsToAddQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                    }

                }
                finally
                {
                    if (LockTaken)
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

            ISendPOIData.UpdateStaticData(EVSE                EVSE,
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
                //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (IncludeEVSEs == null ||
                       (IncludeEVSEs != null && IncludeEVSEs(EVSE)))
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
                    if (LockTaken)
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

            ISendPOIData.DeleteStaticData(EVSE                EVSE,
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
                //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken &&
                        (IncludeEVSEs == null || IncludeEVSEs(EVSE)))
                    {

                        EVSEsToRemoveQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                    }

                }
                finally
                {
                    if (LockTaken)
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

            ISendPOIData.SetStaticData(IEnumerable<EVSE>   EVSEs,
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
                //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        var FilteredEVSEs = EVSEs.Where(evse => IncludeEVSEs(evse) &&
                                                                IncludeEVSEIds(evse.Id)).
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

                }
                finally
                {
                    if (LockTaken)
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

            ISendPOIData.AddStaticData(IEnumerable<EVSE>   EVSEs,
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
                //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        var FilteredEVSEs = EVSEs.Where(evse => IncludeEVSEs(evse) &&
                                                                IncludeEVSEIds(evse.Id)).
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

                }
                finally
                {
                    if (LockTaken)
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

            ISendPOIData.UpdateStaticData(IEnumerable<EVSE>   EVSEs,
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
                //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        var FilteredEVSEs = EVSEs.Where(evse => IncludeEVSEs(evse) &&
                                                                IncludeEVSEIds(evse.Id)).
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

                }
                finally
                {
                    if (LockTaken)
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

            ISendPOIData.DeleteStaticData(IEnumerable<EVSE>   EVSEs,
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
                //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        var FilteredEVSEs = EVSEs.Where(evse => IncludeEVSEs(evse) &&
                                                                IncludeEVSEIds(evse.Id)).
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

                }
                finally
                {
                    if (LockTaken)
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
        public async Task<PushEVSEStatusResult>

            UpdateStatus(IEnumerable<EVSEStatusUpdate>  StatusUpdates,
                         TransmissionTypes              TransmissionType,

                         DateTime?                      Timestamp,
                         CancellationToken?             CancellationToken,
                         EventTracking_Id               EventTrackingId,
                         TimeSpan?                      RequestTimeout)

        {

            #region Initial checks

            if (StatusUpdates == null || !StatusUpdates.Any())
                return PushEVSEStatusResult.NoOperation(Id, this);

            PushEVSEStatusResult result = null;

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
                //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var invokeTimer  = false;
                var LockTaken    = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        var FilteredUpdates = StatusUpdates.Where(statusupdate => IncludeEVSEs  (statusupdate.EVSE) &&
                                                                                  IncludeEVSEIds(statusupdate.EVSE.Id)).
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

                            invokeTimer = true;

                            result = PushEVSEStatusResult.Enqueued(Id, this);

                        }

                        result = PushEVSEStatusResult.NoOperation(Id, this);

                    }

                }
                finally
                {
                    if (LockTaken)
                        DataAndStatusLock.Release();
                }

                if (!LockTaken)
                    return PushEVSEStatusResult.Error(Id, this, Description: "Could not acquire DataAndStatusLock!");

                if (invokeTimer)
                    FlushEVSEFastStatusTimer.Change(FlushEVSEFastStatusEvery, TimeSpan.FromMilliseconds(-1));

                return result;

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

            ISendPOIData.SetStaticData(ChargingStation     ChargingStation,
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
                //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {
                        foreach (var evse in ChargingStation)
                        {

                            if (IncludeEVSEs == null ||
                               (IncludeEVSEs != null && IncludeEVSEs(evse)))
                            {

                                EVSEsToAddQueue.Add(evse);

                                FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                            }

                        }
                    }

                }
                finally
                {
                    if (LockTaken)
                        DataAndStatusLock.Release();
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

            ISendPOIData.AddStaticData(ChargingStation     ChargingStation,
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
                //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        foreach (var evse in ChargingStation)
                        {

                            if (IncludeEVSEs == null ||
                               (IncludeEVSEs != null && IncludeEVSEs(evse)))
                            {

                                EVSEsToAddQueue.Add(evse);

                                FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                            }

                        }

                    }

                }
                finally
                {
                    if (LockTaken)
                        DataAndStatusLock.Release();
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

            ISendPOIData.UpdateStaticData(ChargingStation     ChargingStation,
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
                //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        var AddData = false;

                        foreach (var evse in ChargingStation)
                        {
                            if (IncludeEVSEs == null ||
                               (IncludeEVSEs != null && IncludeEVSEs(evse)))
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

                }
                finally
                {
                    if (LockTaken)
                        DataAndStatusLock.Release();
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

            ISendPOIData.DeleteStaticData(ChargingStation     ChargingStation,
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

            ISendPOIData.SetStaticData(IEnumerable<ChargingStation>  ChargingStations,
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

            ISendPOIData.AddStaticData(IEnumerable<ChargingStation>  ChargingStations,
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

            ISendPOIData.UpdateStaticData(IEnumerable<ChargingStation>  ChargingStations,
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

            ISendPOIData.DeleteStaticData(IEnumerable<ChargingStation>  ChargingStations,
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

            ISendPOIData.SetStaticData(ChargingPool        ChargingPool,
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
                //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        foreach (var evse in ChargingPool.EVSEs)
                        {

                            if (IncludeEVSEs == null ||
                               (IncludeEVSEs != null && IncludeEVSEs(evse)))
                            {

                                EVSEsToAddQueue.Add(evse);

                                FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                            }

                        }

                    }

                }
                finally
                {
                    if (LockTaken)
                        DataAndStatusLock.Release();
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

            ISendPOIData.AddStaticData(ChargingPool        ChargingPool,
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
                //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        foreach (var evse in ChargingPool.EVSEs)
                        {

                            if (IncludeEVSEs == null ||
                               (IncludeEVSEs != null && IncludeEVSEs(evse)))
                            {

                                EVSEsToAddQueue.Add(evse);

                                FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                            }

                        }

                    }

                }
                finally
                {
                    if (LockTaken)
                        DataAndStatusLock.Release();
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

            ISendPOIData.UpdateStaticData(ChargingPool        ChargingPool,
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
                //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        var AddData = false;

                        foreach (var evse in ChargingPool.EVSEs)
                        {
                            if (IncludeEVSEs == null ||
                               (IncludeEVSEs != null && IncludeEVSEs(evse)))
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

                }
                finally
                {
                    if (LockTaken)
                        DataAndStatusLock.Release();
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

            ISendPOIData.DeleteStaticData(ChargingPool        ChargingPool,
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

            ISendPOIData.SetStaticData(IEnumerable<ChargingPool>  ChargingPools,
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

            ISendPOIData.AddStaticData(IEnumerable<ChargingPool>  ChargingPools,
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

            ISendPOIData.UpdateStaticData(IEnumerable<ChargingPool>  ChargingPools,
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

            ISendPOIData.DeleteStaticData(IEnumerable<ChargingPool>  ChargingPools,
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

            ISendPOIData.SetStaticData(ChargingStationOperator  ChargingStationOperator,

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

            ISendPOIData.AddStaticData(ChargingStationOperator  ChargingStationOperator,

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

            ISendPOIData.UpdateStaticData(ChargingStationOperator  ChargingStationOperator,

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

            ISendPOIData.DeleteStaticData(ChargingStationOperator  ChargingStationOperator,

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

            ISendPOIData.SetStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

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

            ISendPOIData.AddStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

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

            ISendPOIData.UpdateStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

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

            ISendPOIData.DeleteStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

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

            ISendPOIData.SetStaticData(RoamingNetwork      RoamingNetwork,

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

            ISendPOIData.AddStaticData(RoamingNetwork      RoamingNetwork,

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

            ISendPOIData.UpdateStaticData(RoamingNetwork      RoamingNetwork,

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

            ISendPOIData.DeleteStaticData(RoamingNetwork      RoamingNetwork,

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


        #region AuthorizeStart(           LocalAuthentication, ChargingLocation = null, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging location.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           ChargingLocation             ChargingLocation    = null,
                           ChargingProduct              ChargingProduct     = null,   // [maxlength: 100]
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (LocalAuthentication == null)
                throw new ArgumentNullException(nameof(LocalAuthentication),  "The given authentication token must not be null!");


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
                                                null,
                                                Id,
                                                OperatorId,
                                                LocalAuthentication,
                                                ChargingLocation,
                                                ChargingProduct,
                                                SessionId,
                                                new ISendAuthorizeStartStop[0],
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            DateTime         Endtime;
            TimeSpan         Runtime;
            AuthStartResult  result;

            if (ChargingLocation?.EVSEId == null)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStartResult.UnknownLocation(Id,
                                                           this,
                                                           SessionId,
                                                           Runtime: Runtime);

            }

            else if (DisableAuthentication)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStartResult.AdminDown(Id,
                                                     this,
                                                     SessionId,
                                                     Runtime: Runtime);

            }

            else
            {

                var response  = await CPORoaming.
                                          AuthorizeStart((OperatorId ?? DefaultOperator.Id).ToOICP(DefaultOperatorIdFormat),
                                                         LocalAuthentication.               ToOICP().RFIDId.Value,
                                                         ChargingLocation.EVSEId.Value.     ToOICP(),
                                                         ChargingProduct?.Id.               ToOICP(),
                                                         SessionId.                         ToOICP(),
                                                         null,
                                                         null,

                                                         Timestamp,
                                                         CancellationToken,
                                                         EventTrackingId,
                                                         RequestTimeout);


                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;

                if (response?.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response?.Content                     != null              &&
                    response?.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
                {

                    result = AuthStartResult.Authorized(
                                 Id,
                                 this,
                                 response.Content.SessionId.ToWWCP().Value,
                                 ProviderId:      response.Content.ProviderId.ToWWCP(),
                                 Description:     response.Content.StatusCode.Description.   ToI18NString(),
                                 AdditionalInfo:  response.Content.StatusCode.AdditionalInfo.ToI18NString(),
                                 NumberOfRetries: response.NumberOfRetries,
                                 Runtime:         Runtime
                             );

                }

                else
                    result = AuthStartResult.NotAuthorized(
                                 Id,
                                 this,
                                 SessionId,
                                 response.Content.ProviderId.ToWWCP(),
                                 response.Content.StatusCode.Description.   ToI18NString(),
                                 response.Content.StatusCode.AdditionalInfo.ToI18NString(),
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
                                                 null,
                                                 Id,
                                                 OperatorId,
                                                 LocalAuthentication,
                                                 ChargingLocation,
                                                 ChargingProduct,
                                                 SessionId,
                                                 new ISendAuthorizeStartStop[0],
                                                 RequestTimeout,
                                                 result,
                                                 Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop (SessionId, LocalAuthentication, ChargingLocation = null,                                           OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging location.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          ChargingLocation             ChargingLocation    = null,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (LocalAuthentication  == null)
                throw new ArgumentNullException(nameof(LocalAuthentication), "The given authentication token must not be null!");


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
                                               null,
                                               Id,
                                               OperatorId,
                                               ChargingLocation,
                                               SessionId,
                                               LocalAuthentication,
                                               RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            DateTime        Endtime;
            TimeSpan        Runtime;
            AuthStopResult  result;

            if (ChargingLocation?.EVSEId == null)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStopResult.UnknownLocation(Id,
                                                          this,
                                                          SessionId,
                                                          Runtime: Runtime);

            }

            else if (DisableAuthentication)
            {
                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStopResult.AdminDown(Id,
                                                    this,
                                                    SessionId,
                                                    Runtime: Runtime);
            }

            else
            {

                var response  = await CPORoaming.AuthorizeStop((OperatorId ?? DefaultOperator.Id).ToOICP(DefaultOperatorIdFormat),
                                                               SessionId.                         ToOICP(),
                                                               LocalAuthentication.               ToOICP().RFIDId.Value,
                                                               ChargingLocation.EVSEId.Value.     ToOICP(),
                                                               null,
                                                               null,

                                                               Timestamp,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               RequestTimeout);


                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;

                if (response?.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response?.Content                     != null              &&
                    response?.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
                {

                    result = AuthStopResult.Authorized(
                                 Id,
                                 this,
                                 SessionId,
                                 response.Content?.ProviderId?.ToWWCP(),
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.Description.ToI18NString()
                                     : null,
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.AdditionalInfo.ToI18NString()
                                     : null
                             );

                }
                else
                    result = AuthStopResult.NotAuthorized(
                                 Id,
                                 this,
                                 SessionId,
                                 response.Content?.ProviderId?.ToWWCP(),
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.Description.ToI18NString()
                                     : I18NString.Empty,
                                 response.Content.StatusCode.HasResult
                                     ? response.Content.StatusCode.AdditionalInfo.ToI18NString()
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
                                                null,
                                                Id,
                                                OperatorId,
                                                ChargingLocation,
                                                SessionId,
                                                LocalAuthentication,
                                                RequestTimeout,
                                                result,
                                                Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

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
        public async Task<SendCDRsResult>

            SendChargeDetailRecords(IEnumerable<WWCP.ChargeDetailRecord>  ChargeDetailRecords,
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
            SendCDRsResult  results;

            #endregion

            #region Filter charge detail records

            var ForwardedCDRs  = new List<WWCP.ChargeDetailRecord>();
            var FilteredCDRs   = new List<SendCDRResult>();

            foreach (var cdr in ChargeDetailRecords)
            {

                if (ChargeDetailRecordFilter(cdr) == ChargeDetailRecordFilters.forward)
                    ForwardedCDRs.Add(cdr);

                else
                    FilteredCDRs.Add(SendCDRResult.Filtered(DateTime.UtcNow,
                                                            cdr,
                                                            Warning.Create(I18NString.Create(Languages.en, "This charge detail record was filtered!"))));

            }

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
                                          ChargeDetailRecords,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRsRequest));
            }

            #endregion


            #region if disabled => 'AdminDown'...

            if (DisableSendChargeDetailRecords)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                results  = SendCDRsResult.AdminDown(DateTime.UtcNow, 
                                                    Id,
                                                    this,
                                                    ChargeDetailRecords,
                                                    Runtime: Runtime);

            }

            #endregion

            else
            {

                var invokeTimer  = false;
                var LockTaken    = await FlushChargeDetailRecordsLock.WaitAsync(TimeSpan.FromSeconds(180));

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
                                                                 ChargeDetailRecords,
                                                                 RequestTimeout);

                            }
                            catch (Exception e)
                            {
                                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRsRequest));
                            }

                            #endregion

                            foreach (var ChargeDetailRecord in ChargeDetailRecords)
                            {

                                try
                                {

                                    ChargeDetailRecordsQueue.Add(ChargeDetailRecord.ToOICP(_WWCPChargeDetailRecord2OICPChargeDetailRecord));
                                    SendCDRsResults.Add(SendCDRResult.Enqueued(DateTime.UtcNow,
                                                                               ChargeDetailRecord));

                                }
                                catch (Exception e)
                                {
                                    SendCDRsResults.Add(SendCDRResult.CouldNotConvertCDRFormat(DateTime.UtcNow,
                                                                                               ChargeDetailRecord,
                                                                                               Warning.Create(I18NString.Create(Languages.en, e.Message))));
                                }

                            }

                            Endtime      = DateTime.UtcNow;
                            Runtime      = Endtime - StartTime;
                            results      = SendCDRsResult.Enqueued(DateTime.UtcNow,
                                                                   Id,
                                                                   this,
                                                                   ChargeDetailRecords,
                                                                   I18NString.Create(Languages.en, "Enqueued for at least " + FlushChargeDetailRecordsEvery.TotalSeconds + " seconds!"),
                                                                   //SendCDRsResults.SafeWhere(cdrresult => cdrresult.Result != SendCDRResultTypes.Enqueued),
                                                                   Runtime: Runtime);
                            invokeTimer  = true;

                        }

                        #endregion

                        #region ...or send at once!

                        else
                        {

                            HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>> response;
                            SendCDRResult result;

                            foreach (var chargeDetailRecord in ChargeDetailRecords)
                            {

                                try
                                {

                                    response = await CPORoaming.SendChargeDetailRecord(chargeDetailRecord.ToOICP(_WWCPChargeDetailRecord2OICPChargeDetailRecord),

                                                                                       Timestamp,
                                                                                       CancellationToken,
                                                                                       EventTrackingId,
                                                                                       RequestTimeout);

                                    if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                                        response.Content        != null              &&
                                        response.Content.Result == true)
                                    {

                                        result = SendCDRResult.Success(DateTime.UtcNow,
                                                                       chargeDetailRecord);

                                    }

                                    else
                                        result = SendCDRResult.Error(DateTime.UtcNow,
                                                                     chargeDetailRecord,
                                                                     I18NString.Create(Languages.en, response.HTTPBodyAsUTF8String));

                                }
                                catch (Exception e)
                                {
                                    result = SendCDRResult.CouldNotConvertCDRFormat(DateTime.UtcNow,
                                                                                    chargeDetailRecord,
                                                                                    I18NString.Create(Languages.en, e.Message));
                                }

                                SendCDRsResults.Add(result);
                                RoamingNetwork.SessionsStore.CDRForwarded(chargeDetailRecord.SessionId, result);

                            }

                            Endtime  = DateTime.UtcNow;
                            Runtime  = Endtime - StartTime;

                            if (SendCDRsResults.All(cdrresult => cdrresult.Result == SendCDRResultTypes.Success))
                                results = SendCDRsResult.Success(DateTime.UtcNow,
                                                                 Id,
                                                                 this,
                                                                 ChargeDetailRecords,
                                                                 Runtime: Runtime);

                            else
                                results = SendCDRsResult.Error(DateTime.UtcNow,
                                                               Id,
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
                        results  = SendCDRsResult.Timeout(DateTime.UtcNow, 
                                                          Id,
                                                          this,
                                                          ChargeDetailRecords,
                                                          I18NString.Create(Languages.en, "Could not " + (TransmissionType == TransmissionTypes.Enqueue ? "enqueue" : "send") + " charge detail records!"),
                                                          //ChargeDetailRecords.SafeSelect(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Timeout)),
                                                          Runtime: Runtime);

                    }

                    #endregion

                }
                finally
                {
                    if (LockTaken)
                        FlushChargeDetailRecordsLock.Release();
                }

                if (invokeTimer)
                    FlushChargeDetailRecordsTimer.Change(FlushChargeDetailRecordsEvery, TimeSpan.FromMilliseconds(-1));

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
                                           ChargeDetailRecords,
                                           RequestTimeout,
                                           results,
                                           Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRsResponse));
            }

            #endregion

            return results;

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

            var LockTaken = await DataAndStatusLock.WaitAsync(0);

            try
            {

                if (LockTaken)
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

            }
            catch (Exception e)
            {

                while (e.InnerException != null)
                    e = e.InnerException;

                DebugX.LogT(GetType().Name + ".DataAndStatusLock '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                //OnWWCPEMPAdapterException?.Invoke(DateTime.UtcNow,
                //                                  this,
                //                                  e);

            }

            finally
            {

                if (LockTaken)
                    DataAndStatusLock.Release();

                else
                    DebugX.LogT("DataAndStatusLock exited!");

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
                    try
                    {
                        SendOnWarnings(DateTime.UtcNow,
                                       nameof(WWCPEMPAdapter) + Id,
                                       "EVSEsToAddTask",
                                       EVSEsToAddTask.Result.Warnings);
                    }
                    catch (Exception)
                    { }
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
                        try
                        {
                            SendOnWarnings(DateTime.UtcNow,
                                       nameof(WWCPEMPAdapter) + Id,
                                       "PushEVSEDataTask",
                                       PushEVSEDataTask.Result.Warnings);
                        }
                        catch (Exception)
                        { }
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
                    try
                    {
                        SendOnWarnings(DateTime.UtcNow,
                                   nameof(WWCPEMPAdapter) + Id,
                                   "PushEVSEStatusTask",
                                   PushEVSEStatusTask.Result.Warnings);
                    }
                    catch (Exception)
                    { }
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
                        try
                        {
                            SendOnWarnings(DateTime.UtcNow,
                                       nameof(WWCPEMPAdapter) + Id,
                                       "EVSEsToRemoveTask",
                                       EVSEsToRemoveTask.Result.Warnings);
                        }
                        catch (Exception)
                        { }
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

            var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

            try
            {

                if (LockTaken)
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

            }
            finally
            {
                if (LockTaken)
                    DataAndStatusLock.Release();
            }

            #endregion

            #region Send changed EVSE status

            if (EVSEStatusFastQueueCopy.Count > 0)
            {

                var pushEVSEStatusResult = await PushEVSEStatus(EVSEStatusFastQueueCopy,
                                                                ActionTypes.update,

                                                                DateTime.UtcNow,
                                                                new CancellationTokenSource().Token,
                                                                EventTracking_Id.New,
                                                                DefaultRequestTimeout).
                                                     ConfigureAwait(false);

                if (pushEVSEStatusResult.Warnings.Any())
                {
                    try
                    {
                        SendOnWarnings(DateTime.UtcNow,
                                       nameof(WWCPEMPAdapter) + Id,
                                       "PushEVSEStatus",
                                       pushEVSEStatusResult.Warnings);
                    }
                    catch (Exception)
                    { }
                }

            }

            #endregion

        }

        #endregion

        #region (timer) FlushChargeDetailRecords()

        protected override Boolean SkipFlushChargeDetailRecordsQueues()
            => ChargeDetailRecordsQueue.Count == 0;

        protected override async Task FlushChargeDetailRecordsQueues(IEnumerable<ChargeDetailRecord> ChargeDetailRecords)
        {

            HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>> response;
            SendCDRResult result;

            foreach (var chargeDetailRecord in ChargeDetailRecords)
            {

                try
                {

                    response = await CPORoaming.SendChargeDetailRecord(chargeDetailRecord,

                                                                       DateTime.UtcNow,
                                                                       new CancellationTokenSource().Token,
                                                                       EventTracking_Id.New,
                                                                       DefaultRequestTimeout).
                                                ConfigureAwait(false);

                    if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                        response.Content        != null &&
                        response.Content.Result == true)
                    {
                        result = SendCDRResult.Success(DateTime.UtcNow,
                                                       chargeDetailRecord.GetCustomDataAs<WWCP.ChargeDetailRecord>(OICPMapper.WWCP_CDR),
                                                       Runtime: response.Runtime);
                    }

                    else
                        result = SendCDRResult.Error(DateTime.UtcNow,
                                                     chargeDetailRecord.GetCustomDataAs<WWCP.ChargeDetailRecord>(OICPMapper.WWCP_CDR),
                                                     I18NString.Create(Languages.en, response.HTTPBodyAsUTF8String),
                                                     response.Runtime);

                }
                catch (Exception e)
                {
                    result = SendCDRResult.Error(DateTime.UtcNow, 
                                                 chargeDetailRecord.GetCustomDataAs<WWCP.ChargeDetailRecord>(OICPMapper.WWCP_CDR),
                                                 Warning.Create(I18NString.Create(Languages.en, e.Message)),
                                                 Runtime: TimeSpan.Zero);
                }

                RoamingNetwork.SessionsStore.CDRForwarded(chargeDetailRecord.SessionId.ToWWCP(), result);

            }

            //ToDo: Re-add to queue if it could not be send...

        }

        #endregion


        // -----------------------------------------------------------------------------------------------------


        #region Operator overloading

        #region Operator == (WWCPEMPAdapter1, WWCPEMPAdapter2)

        /// <summary>
        /// Compares two WWCPEMPAdapters for equality.
        /// </summary>
        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (WWCPEMPAdapter WWCPEMPAdapter1,
                                           WWCPEMPAdapter WWCPEMPAdapter2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(WWCPEMPAdapter1, WWCPEMPAdapter2))
                return true;

            // If one is null, but not both, return false.
            if (WWCPEMPAdapter1 is null || WWCPEMPAdapter2 is null)
                return false;

            return WWCPEMPAdapter1.Equals(WWCPEMPAdapter2);

        }

        #endregion

        #region Operator != (WWCPEMPAdapter1, WWCPEMPAdapter2)

        /// <summary>
        /// Compares two WWCPEMPAdapters for inequality.
        /// </summary>
        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (WWCPEMPAdapter WWCPEMPAdapter1,
                                           WWCPEMPAdapter WWCPEMPAdapter2)

            => !(WWCPEMPAdapter1 == WWCPEMPAdapter2);

        #endregion

        #region Operator <  (WWCPEMPAdapter1, WWCPEMPAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (WWCPEMPAdapter  WWCPEMPAdapter1,
                                          WWCPEMPAdapter  WWCPEMPAdapter2)
        {

            if (WWCPEMPAdapter1 is null)
                throw new ArgumentNullException(nameof(WWCPEMPAdapter1),  "The given WWCPEMPAdapter1 must not be null!");

            return WWCPEMPAdapter1.CompareTo(WWCPEMPAdapter2) < 0;

        }

        #endregion

        #region Operator <= (WWCPEMPAdapter1, WWCPEMPAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (WWCPEMPAdapter WWCPEMPAdapter1,
                                           WWCPEMPAdapter WWCPEMPAdapter2)

            => !(WWCPEMPAdapter1 > WWCPEMPAdapter2);

        #endregion

        #region Operator >  (WWCPEMPAdapter1, WWCPEMPAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (WWCPEMPAdapter WWCPEMPAdapter1,
                                          WWCPEMPAdapter WWCPEMPAdapter2)
        {

            if (WWCPEMPAdapter1 is null)
                throw new ArgumentNullException(nameof(WWCPEMPAdapter1),  "The given WWCPEMPAdapter must not be null!");

            return WWCPEMPAdapter1.CompareTo(WWCPEMPAdapter2) > 0;

        }

        #endregion

        #region Operator >= (WWCPEMPAdapter1, WWCPEMPAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (WWCPEMPAdapter WWCPEMPAdapter1,
                                           WWCPEMPAdapter WWCPEMPAdapter2)

            => !(WWCPEMPAdapter1 < WWCPEMPAdapter2);

        #endregion

        #endregion

        #region IComparable<WWCPEMPAdapter> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object is WWCPEMPAdapter WWCPEMPAdapter)
                return CompareTo(WWCPEMPAdapter);

            throw new ArgumentException("The given object is not an WWCPEMPAdapter!", nameof(Object));

        }

        #endregion

        #region CompareTo(WWCPEMPAdapter)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPEMPAdapter">An WWCPEMPAdapter object to compare with.</param>
        public Int32 CompareTo(WWCPEMPAdapter WWCPEMPAdapter)
        {

            if (WWCPEMPAdapter is null)
                throw new ArgumentNullException(nameof(WWCPEMPAdapter), "The given WWCPEMPAdapter must not be null!");

            return Id.CompareTo(WWCPEMPAdapter.Id);

        }

        #endregion

        #endregion

        #region IEquatable<WWCPEMPAdapter> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is WWCPEMPAdapter WWCPEMPAdapter &&
                   Equals(WWCPEMPAdapter);

        #endregion

        #region Equals(WWCPEMPAdapter)

        /// <summary>
        /// Compares two WWCPEMPAdapter for equality.
        /// </summary>
        /// <param name="WWCPEMPAdapter">An WWCPEMPAdapter to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(WWCPEMPAdapter WWCPEMPAdapter)

            => WWCPEMPAdapter is null
                   ? false
                   : Id.Equals(WWCPEMPAdapter.Id);

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
