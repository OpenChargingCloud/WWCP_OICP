/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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

        //private        readonly  EVSEDataRecord2XMLDelegate                             _EVSEDataRecord2XML;

        //private        readonly  EVSEStatusRecord2XMLDelegate                           _EVSEStatusRecord2XML;

        //private        readonly  ChargeDetailRecord2XMLDelegate                         _ChargeDetailRecord2XML;

        private        readonly  ChargingStationOperatorNameSelectorDelegate            _OperatorNameSelector;

        private static readonly  Regex                                                  pattern                      = new Regex(@"\s=\s");

        public  static readonly  ChargingStationOperatorNameSelectorDelegate            DefaultOperatorNameSelector  = I18N => I18N.FirstText();

        private readonly        HashSet<EVSE>                                           EVSEsToAddQueue;
        private readonly        HashSet<EVSE>                                           EVSEsToUpdateQueue;
        private readonly        List<EVSEStatusUpdate>                                  EVSEStatusChangesFastQueue;
        private readonly        List<EVSEStatusUpdate>                                  EVSEStatusChangesDelayedQueue;
        private readonly        HashSet<EVSE>                                           EVSEsToRemoveQueue;
        private readonly        List<WWCP.ChargeDetailRecord>                           ChargeDetailRecordQueue;

        private                 IncludeEVSEIdDelegate                                   _IncludeEVSEIds;
        private                 IncludeEVSEDelegate                                     _IncludeEVSEs;

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

        #region OnSendCDRRequest/-Response

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
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// 
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// 
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        /// 
        /// <param name="DNSClient">The attached DNS service.</param>
        public WWCPCPOAdapter(CSORoamingProvider_Id                              Id,
                              I18NString                                         Name,
                              I18NString                                         Description,
                              RoamingNetwork                                     RoamingNetwork,

                              CPORoaming                                         CPORoaming,
                              EVSE2EVSEDataRecordDelegate                        EVSE2EVSEDataRecord                             = null,
                              EVSEStatusUpdate2EVSEStatusRecordDelegate          EVSEStatusUpdate2EVSEStatusRecord               = null,
                              WWCPChargeDetailRecord2ChargeDetailRecordDelegate  WWCPChargeDetailRecord2OICPChargeDetailRecord   = null,
                              //EVSEDataRecord2XMLDelegate                         EVSEDataRecord2XML                              = null,
                              //EVSEStatusRecord2XMLDelegate                       EVSEStatusRecord2XML                            = null,
                              //ChargeDetailRecord2XMLDelegate                     ChargeDetailRecord2XML                          = null,

                              ChargingStationOperator                            DefaultOperator                                 = null,
                              WWCP.OperatorIdFormats                             DefaultOperatorIdFormat                         = WWCP.OperatorIdFormats.ISO_STAR,
                              ChargingStationOperatorNameSelectorDelegate        OperatorNameSelector                            = null,
                              IncludeEVSEIdDelegate                              IncludeEVSEIds                                  = null,
                              IncludeEVSEDelegate                                IncludeEVSEs                                    = null,

                              TimeSpan?                                          ServiceCheckEvery                               = null,
                              TimeSpan?                                          StatusCheckEvery                                = null,
                              TimeSpan?                                          CDRCheckEvery                                   = null,

                              Boolean                                            DisablePushData                                 = false,
                              Boolean                                            DisablePushStatus                               = false,
                              Boolean                                            DisableAuthentication                           = false,
                              Boolean                                            DisableSendChargeDetailRecords                  = false,

                              DNSClient                                          DNSClient                                       = null)

            : base(Id,
                   Name,
                   Description,
                   RoamingNetwork,

                   ServiceCheckEvery,
                   StatusCheckEvery,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushStatus,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords,

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
            //this._EVSEDataRecord2XML                              = EVSEDataRecord2XML;
            //this._EVSEStatusRecord2XML                            = EVSEStatusRecord2XML;
            //this._ChargeDetailRecord2XML                          = ChargeDetailRecord2XML;
            this.DefaultOperatorId                                = DefaultOperator.Id.ToOICP(DefaultOperatorIdFormat);
            this.DefaultOperatorIdFormat                          = DefaultOperatorIdFormat;
            this.DefaultOperatorName                              = DefaultOperatorNameSelector(DefaultOperator.Name);
            this._OperatorNameSelector                            = OperatorNameSelector;

            this._IncludeEVSEIds                                  = IncludeEVSEIds ?? (evseid => true);
            this._IncludeEVSEs                                    = IncludeEVSEs   ?? (evse   => true);

            this.EVSEsToAddQueue                                  = new HashSet<EVSE>();
            this.EVSEsToUpdateQueue                               = new HashSet<EVSE>();
            this.EVSEStatusChangesFastQueue                       = new List<EVSEStatusUpdate>();
            this.EVSEStatusChangesDelayedQueue                    = new List<EVSEStatusUpdate>();
            this.EVSEsToRemoveQueue                               = new HashSet<EVSE>();
            this.ChargeDetailRecordQueue                          = new List<WWCP.ChargeDetailRecord>();


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
                                                 eMAId:              Request.EVCOId.          ToWWCP(),
                                                 ChargingProduct:    PartnerProductId.HasValue
                                                                         ? new ChargingProduct(PartnerProductId.Value.ToWWCP())
                                                                         : null,

                                                 eMAIds:             new eMobilityAccount_Id[] {
                                                                         Request.EVCOId.ToWWCP()
                                                                     },

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

                            ChargingReservation_Id _ReservationId;

                            if (ProductIdElements.ContainsKey("R") &&
                                ChargingReservation_Id.TryParse(Request.EVSEId.OperatorId.ToWWCP().Value, ProductIdElements["R"], out _ReservationId))
                                ReservationId = _ReservationId;


                            if (ProductIdElements.ContainsKey("D"))
                            {

                                var MinDurationText = ProductIdElements["D"];

                                if (MinDurationText.EndsWith("sec", StringComparison.InvariantCulture))
                                    MinDuration = TimeSpan.FromSeconds(UInt32.Parse(MinDurationText.Substring(0, MinDurationText.Length - 3)));

                                if (MinDurationText.EndsWith("min", StringComparison.InvariantCulture))
                                    MinDuration = TimeSpan.FromMinutes(UInt32.Parse(MinDurationText.Substring(0, MinDurationText.Length - 3)));

                            }


                            Single _PlannedEnergy = 0;

                            if (ProductIdElements.ContainsKey("E") &&
                                Single.TryParse(ProductIdElements["E"], out _PlannedEnergy))
                                PlannedEnergy = _PlannedEnergy;


                            ChargingProduct_Id _ProductId;

                            if (ProductIdElements.ContainsKey("P") &&
                                ChargingProduct_Id.TryParse(ProductIdElements["P"], out _ProductId))
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
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// 
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
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
                              //EVSEDataRecord2XMLDelegate                         EVSEDataRecord2XML                              = null,
                              //EVSEStatusRecord2XMLDelegate                       EVSEStatusRecord2XML                            = null,
                              //ChargeDetailRecord2XMLDelegate                     ChargeDetailRecord2XML                          = null,

                              ChargingStationOperator                            DefaultOperator                                 = null,
                              WWCP.OperatorIdFormats                             DefaultOperatorIdFormat                         = WWCP.OperatorIdFormats.ISO_STAR,
                              ChargingStationOperatorNameSelectorDelegate        OperatorNameSelector                            = null,
                              IncludeEVSEIdDelegate                              IncludeEVSEIds                                  = null,
                              IncludeEVSEDelegate                                IncludeEVSEs                                    = null,
                              TimeSpan?                                          ServiceCheckEvery                               = null,
                              TimeSpan?                                          StatusCheckEvery                                = null,
                              TimeSpan?                                          CDRCheckEvery                                   = null,

                              Boolean                                            DisablePushData                                 = false,
                              Boolean                                            DisablePushStatus                               = false,
                              Boolean                                            DisableAuthentication                           = false,
                              Boolean                                            DisableSendChargeDetailRecords                  = false)

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
                   //EVSEDataRecord2XML,
                   //EVSEStatusRecord2XML,
                   //ChargeDetailRecord2XML,

                   DefaultOperator,
                   DefaultOperatorIdFormat,
                   OperatorNameSelector,
                   IncludeEVSEIds,
                   IncludeEVSEs,
                   ServiceCheckEvery,
                   StatusCheckEvery,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushStatus,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords)

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
        /// <param name="ClientCert">The TLS client certificate to use.</param>
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
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public WWCPCPOAdapter(CSORoamingProvider_Id                              Id,
                              I18NString                                         Name,
                              I18NString                                         Description,
                              RoamingNetwork                                     RoamingNetwork,

                              String                                             RemoteHostname,
                              IPPort                                             RemoteTCPPort                                   = null,
                              RemoteCertificateValidationCallback                RemoteCertificateValidator                      = null,
                              LocalCertificateSelectionCallback                  LocalCertificateSelector                        = null,
                              X509Certificate                                    ClientCert                                      = null,
                              String                                             RemoteHTTPVirtualHost                           = null,
                              String                                             URIPrefix                                       = CPOClient.DefaultURIPrefix,
                              String                                             EVSEDataURI                                     = CPOClient.DefaultEVSEDataURI,
                              String                                             EVSEStatusURI                                   = CPOClient.DefaultEVSEStatusURI,
                              String                                             AuthorizationURI                                = CPOClient.DefaultAuthorizationURI,
                              String                                             AuthenticationDataURI                           = CPOClient.DefaultAuthenticationDataURI,
                              String                                             HTTPUserAgent                                   = CPOClient.DefaultHTTPUserAgent,
                              TimeSpan?                                          RequestTimeout                                  = null,
                              Byte?                                              MaxNumberOfRetries                              = CPOClient.DefaultMaxNumberOfRetries,

                              String                                             ServerName                                      = CPOServer.DefaultHTTPServerName,
                              String                                             ServiceId                                       = null,
                              IPPort                                             ServerTCPPort                                   = null,
                              String                                             ServerURIPrefix                                 = CPOServer.DefaultURIPrefix,
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
                              //EVSEDataRecord2XMLDelegate                         EVSEDataRecord2XML                              = null,
                              //EVSEStatusRecord2XMLDelegate                       EVSEStatusRecord2XML                            = null,
                              //ChargeDetailRecord2XMLDelegate                     ChargeDetailRecord2XML                          = null,

                              ChargingStationOperator                            DefaultOperator                                 = null,
                              WWCP.OperatorIdFormats                             DefaultOperatorIdFormat                         = WWCP.OperatorIdFormats.ISO_STAR,
                              ChargingStationOperatorNameSelectorDelegate        OperatorNameSelector                            = null,
                              IncludeEVSEIdDelegate                              IncludeEVSEIds                                  = null,
                              IncludeEVSEDelegate                                IncludeEVSEs                                    = null,
                              TimeSpan?                                          ServiceCheckEvery                               = null,
                              TimeSpan?                                          StatusCheckEvery                                = null,
                              TimeSpan?                                          CDRCheckEvery                                   = null,

                              Boolean                                            DisablePushData                                 = false,
                              Boolean                                            DisablePushStatus                               = false,
                              Boolean                                            DisableAuthentication                           = false,
                              Boolean                                            DisableSendChargeDetailRecords                  = false,

                              DNSClient                                          DNSClient                                       = null)

            : this(Id,
                   Name,
                   Description,
                   RoamingNetwork,

                   new CPORoaming(Id.ToString(),
                                  RemoteHostname,
                                  RemoteTCPPort,
                                  RemoteCertificateValidator,
                                  LocalCertificateSelector,
                                  ClientCert,
                                  RemoteHTTPVirtualHost,
                                  URIPrefix,
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
                                  ServerURIPrefix,
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
                   //EVSEDataRecord2XML,
                   //EVSEStatusRecord2XML,
                   //ChargeDetailRecord2XML,

                   DefaultOperator,
                   DefaultOperatorIdFormat,
                   OperatorNameSelector,
                   IncludeEVSEIds,
                   IncludeEVSEs,
                   ServiceCheckEvery,
                   StatusCheckEvery,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushStatus,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords)

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

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Get effective number of EVSE status to upload

            var Warnings = new List<String>();

            var _EVSEs = EVSEs.Where (evse => evse != null          &&
                                              _IncludeEVSEs  (evse) &&
                                              _IncludeEVSEIds(evse.Id)).

                               Select(evse => {

                                   try
                                   {

                                       return evse.ToOICP(_EVSE2EVSEDataRecord);

                                   }
                                   catch (Exception e)
                                   {
                                       DebugX.  Log(e.Message);
                                       Warnings.Add(e.Message);
                                   }

                                   return null;

                               }).
                               Where(evsedatarecord => evsedatarecord != null).
                               ToArray();

            PushEVSEDataResult result;

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
                                                  _EVSEs.ULongCount(),
                                                  _EVSEs,
                                                  Warnings.Where(warning => warning.IsNotNullOrEmpty()),
                                                  RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnPushEVSEDataWWCPRequest));
            }

            #endregion


            var response = await CPORoaming.
                                     PushEVSEData(_EVSEs,
                                                  DefaultOperatorId,
                                                  DefaultOperatorName.IsNotNullOrEmpty() ? DefaultOperatorName : null,
                                                  ServerAction,
                                                  null,

                                                  Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  RequestTimeout).
                                     ConfigureAwait(false);


            var Endtime = DateTime.UtcNow;
            var Runtime = Endtime - StartTime;

            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        != null)
            {

                // Success...
                if (response.Content.Result)
                    result = PushEVSEDataResult.Success(Id,
                                                    this,
                                                    response.Content.StatusCode.Description,
                                                    response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                        ? Warnings.AddAndReturnList(response.Content.StatusCode.AdditionalInfo)
                                                        : Warnings,
                                                    Runtime);


                // Operation failed... maybe the systems are out of sync?!
                // Try an automatic fullLoad in order to repair...
                else
                {

                    if (ServerAction == ActionTypes.insert ||
                        ServerAction == ActionTypes.update ||
                        ServerAction == ActionTypes.delete)
                    {

                        #region Add warnings...

                        Warnings.Add(ServerAction.ToString() + " of " + _EVSEs.Length + " EVSEs failed!");
                        Warnings.Add(response.Content.StatusCode.Code.ToString());
                        Warnings.Add(response.Content.StatusCode.Description);

                        if (response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty())
                            Warnings.Add(response.Content.StatusCode.AdditionalInfo);

                        Warnings.Add("Will try to fix this issue via a 'fullLoad' of all EVSEs!");

                        #endregion

                        #region Get all EVSEs from the roaming network

                        var FullLoadEVSEs     = RoamingNetwork.EVSEs.Where (evse => evse != null  &&
                                                                            _IncludeEVSEs  (evse) &&
                                                                            _IncludeEVSEIds(evse.Id)).
                                                    Select(evse => {

                                                        try
                                                        {

                                                            return evse.ToOICP(_EVSE2EVSEDataRecord);

                                                        }
                                                        catch (Exception e)
                                                        {
                                                            DebugX.  Log(e.Message);
                                                            Warnings.Add(e.Message);
                                                        }

                                                        return null;

                                                    }).
                                                    Where(evsedatarecord => evsedatarecord != null).
                                                    ToArray();

                        #endregion

                        #region Send request

                        var FullLoadResponse  = await CPORoaming.
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

                        Endtime  = DateTime.UtcNow;
                        Runtime  = Endtime - StartTime;

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
                        result = PushEVSEDataResult.Error(Id,
                                                      this,
                                                      EVSEs,
                                                      response.HTTPStatusCode.ToString(),
                                                      response.HTTPBody != null
                                                          ? Warnings.AddAndReturnList(response.HTTPBody.ToUTF8String())
                                                          : Warnings.AddAndReturnList("No HTTP body received!"),
                                                      Runtime);

                }

            }
            else
                result = PushEVSEDataResult.Error(Id,
                                              this,
                                              EVSEs,
                                              response.HTTPStatusCode.ToString(),
                                              response.HTTPBody != null
                                                  ? Warnings.AddAndReturnList(response.HTTPBody.ToUTF8String())
                                                  : Warnings.AddAndReturnList("No HTTP body received!"),
                                              Runtime);


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
                                                   _EVSEs.ULongCount(),
                                                   _EVSEs,
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
        public async Task<PushStatusResult>

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

            #region Get effective number of EVSE status to upload

            var Warnings = new List<String>();

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

                                          // Only push the current status of the latest status update!
                                          return new EVSEStatusRecord(
                                                     evsestatusupdate.Key.ToOICP().Value,
                                                     evsestatusupdate.Value.First().NewStatus.Value.AsOICPEVSEStatus()
                                                 );

                                      }
                                      catch (Exception e)
                                      {
                                          DebugX.  Log(e.Message);
                                          Warnings.Add(e.Message);
                                      }

                                      return null;

                                  }).
                                  Where(evsestatusrecord => evsestatusrecord != null).
                                  ToArray();

            PushStatusResult result = null;

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
                                                    RequestTimeout).
                                     ConfigureAwait(false);


            var Endtime = DateTime.UtcNow;
            var Runtime = Endtime - StartTime;

            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        != null)
            {

                if (response.Content.Result)
                    result = PushStatusResult.Success(Id,
                                                      this,
                                                      response.Content.StatusCode.Description,
                                                      response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                          ? Warnings.AddAndReturnList(response.Content.StatusCode.AdditionalInfo)
                                                          : Warnings,
                                                      Runtime);

                else
                    result = PushStatusResult.Error(Id,
                                                    this,
                                                    EVSEStatusUpdates,
                                                    response.Content.StatusCode.Description,
                                                    response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                        ? Warnings.AddAndReturnList(response.Content.StatusCode.AdditionalInfo)
                                                        : Warnings,
                                                    Runtime);

            }
            else
                result = PushStatusResult.Error(Id,
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

        #region SetStaticData   (EVSE, TransmissionType = Enqueued, ...)

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
        Task<PushEVSEDataResult>

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

            if (TransmissionType == TransmissionTypes.Enqueued)
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

                lock (DataAndStatusLock)
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        EVSEsToAddQueue.Add(EVSE);

                        FlushEVSEDataTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

                    }

                }

                return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

            }

            #endregion

            return PushEVSEData(new EVSE[] { EVSE },
                                ActionTypes.fullLoad,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region AddStaticData   (EVSE, TransmissionType = Enqueued, ...)

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
        Task<PushEVSEDataResult>

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

            if (TransmissionType == TransmissionTypes.Enqueued)
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

                lock (DataAndStatusLock)
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        EVSEsToAddQueue.Add(EVSE);

                        FlushEVSEDataTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

                    }

                }

                return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

            }

            #endregion

            return PushEVSEData(new EVSE[] { EVSE },
                                ActionTypes.insert,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(EVSE, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueued, ...)

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
        Task<PushEVSEDataResult>

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

            if (TransmissionType == TransmissionTypes.Enqueued)
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

                lock (DataAndStatusLock)
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

                        FlushEVSEDataTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

                    }

                }

                return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

            }

            #endregion

            return PushEVSEData(new EVSE[] { EVSE },
                                ActionTypes.update,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(EVSE, TransmissionType = Enqueued, ...)

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
        Task<PushEVSEDataResult>

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

            if (TransmissionType == TransmissionTypes.Enqueued)
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

                lock (DataAndStatusLock)
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        EVSEsToRemoveQueue.Add(EVSE);

                        FlushEVSEDataTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

                    }

                }

                return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

            }

            #endregion

            return PushEVSEData(new EVSE[] { EVSE },
                                ActionTypes.delete,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion


        #region SetStaticData   (EVSEs, TransmissionType = Enqueued, ...)

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
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(IEnumerable<EVSE>   EVSEs,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueued)
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

                lock(DataAndStatusLock)
                {

                    var FilteredEVSEs = EVSEs.Where(evse => _IncludeEVSEs  (evse) &&
                                                            _IncludeEVSEIds(evse.Id)).
                                              ToArray();

                    if (FilteredEVSEs.Any())
                    {

                        foreach (var EVSE in FilteredEVSEs)
                            EVSEsToAddQueue.Add(EVSE);

                        FlushEVSEDataTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

                        return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

                    }

                    return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

                }

            }

            #endregion

            return PushEVSEData(EVSEs,
                                ActionTypes.fullLoad,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region AddStaticData   (EVSEs, TransmissionType = Enqueued, ...)

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
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(IEnumerable<EVSE>   EVSEs,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueued)
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

                lock(DataAndStatusLock)
                {

                    var FilteredEVSEs = EVSEs.Where(evse => _IncludeEVSEs  (evse) &&
                                                            _IncludeEVSEIds(evse.Id)).
                                              ToArray();

                    if (FilteredEVSEs.Any())
                    {

                        foreach (var EVSE in FilteredEVSEs)
                            EVSEsToAddQueue.Add(EVSE);

                        FlushEVSEDataTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

                        return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

                    }

                    return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

                }

            }

            #endregion

            return PushEVSEData(EVSEs,
                                ActionTypes.insert,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(EVSEs, TransmissionType = Enqueued, ...)

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
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(IEnumerable<EVSE>   EVSEs,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueued)
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

                lock(DataAndStatusLock)
                {

                    var FilteredEVSEs = EVSEs.Where(evse => _IncludeEVSEs  (evse) &&
                                                            _IncludeEVSEIds(evse.Id)).
                                              ToArray();

                    if (FilteredEVSEs.Any())
                    {

                        foreach (var EVSE in FilteredEVSEs)
                            EVSEsToUpdateQueue.Add(EVSE);

                        FlushEVSEDataTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

                        return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

                    }

                    return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

                }

            }

            #endregion

            return PushEVSEData(EVSEs,
                                ActionTypes.update,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(EVSEs, TransmissionType = Enqueued, ...)

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
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(IEnumerable<EVSE>   EVSEs,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueued)
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

                lock(DataAndStatusLock)
                {

                    var FilteredEVSEs = EVSEs.Where(evse => _IncludeEVSEs  (evse) &&
                                                            _IncludeEVSEIds(evse.Id)).
                                              ToArray();

                    if (FilteredEVSEs.Any())
                    {

                        foreach (var EVSE in FilteredEVSEs)
                            EVSEsToRemoveQueue.Add(EVSE);

                        FlushEVSEDataTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

                        return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

                    }

                    return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

                }

            }

            #endregion

            return PushEVSEData(EVSEs,
                                ActionTypes.delete,

                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout);

        }

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueued, ...)

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
        Task<PushAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<EVSEAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                   TransmissionType,

                                               DateTime?                           Timestamp,
                                               CancellationToken?                  CancellationToken,
                                               EventTracking_Id                    EventTrackingId,
                                               TimeSpan?                           RequestTimeout)


                => Task.FromResult(PushAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueued, ...)

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
        Task<PushStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<EVSEStatusUpdate>  StatusUpdates,
                                     TransmissionTypes              TransmissionType,

                                     DateTime?                      Timestamp,
                                     CancellationToken?             CancellationToken,
                                     EventTracking_Id               EventTrackingId,
                                     TimeSpan?                      RequestTimeout)

        {

            #region Initial checks

            if (StatusUpdates == null || !StatusUpdates.Any())
                return Task.FromResult(PushStatusResult.NoOperation(Id, this));

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueued)
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

                lock (DataAndStatusLock)
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

                        StatusCheckTimer.Change(_FlushEVSEFastStatusEvery, Timeout.Infinite);

                        return Task.FromResult(PushStatusResult.Enqueued(Id, this));

                    }

                    return Task.FromResult(PushStatusResult.NoOperation(Id, this));

                }

            }

            #endregion

            return PushEVSEStatus(StatusUpdates,
                                  ActionTypes.update,

                                  Timestamp,
                                  CancellationToken,
                                  EventTrackingId,
                                  RequestTimeout);

        }

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station(s)...

        #region SetStaticData   (ChargingStation, TransmissionType = Enqueued, ...)

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

            if (TransmissionType == TransmissionTypes.Enqueued)
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

                lock(DataAndStatusLock)
                {

                    foreach (var evse in ChargingStation)
                    {

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            FlushEVSEDataTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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

        #region AddStaticData   (ChargingStation, TransmissionType = Enqueued, ...)

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

            if (TransmissionType == TransmissionTypes.Enqueued)
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

                lock(DataAndStatusLock)
                {

                    foreach (var evse in ChargingStation)
                    {

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            FlushEVSEDataTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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

        #region UpdateStaticData(ChargingStation, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueued, ...)

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

            if (TransmissionType == TransmissionTypes.Enqueued)
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

                lock(DataAndStatusLock)
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

                        FlushEVSEDataTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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

        #region DeleteStaticData(ChargingStation, TransmissionType = Enqueued, ...)

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


        #region SetStaticData   (ChargingStations, TransmissionType = Enqueued, ...)

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

        #region AddStaticData   (ChargingStations, TransmissionType = Enqueued, ...)

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

        #region UpdateStaticData(ChargingStations, TransmissionType = Enqueued, ...)

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

        #region DeleteStaticData(ChargingStations, TransmissionType = Enqueued, ...)

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


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueued, ...)

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
        Task<PushAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                              TransmissionType,

                                               DateTime?                                      Timestamp,
                                               CancellationToken?                             CancellationToken,
                                               EventTracking_Id                               EventTrackingId,
                                               TimeSpan?                                      RequestTimeout)


                => Task.FromResult(PushAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueued, ...)

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
        Task<PushStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingStationStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                         TransmissionType,

                                     DateTime?                                 Timestamp,
                                     CancellationToken?                        CancellationToken,
                                     EventTracking_Id                          EventTrackingId,
                                     TimeSpan?                                 RequestTimeout)


                => Task.FromResult(PushStatusResult.NoOperation(Id, this));

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging pool(s)...

        #region SetStaticData   (ChargingPool, TransmissionType = Enqueued, ...)

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

            if (TransmissionType == TransmissionTypes.Enqueued)
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

                lock(DataAndStatusLock)
                {

                    foreach (var evse in ChargingPool.EVSEs)
                    {

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            FlushEVSEDataTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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

        #region AddStaticData   (ChargingPool, TransmissionType = Enqueued, ...)

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

            if (TransmissionType == TransmissionTypes.Enqueued)
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

                lock(DataAndStatusLock)
                {

                    foreach (var evse in ChargingPool.EVSEs)
                    {

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            FlushEVSEDataTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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

        #region UpdateStaticData(ChargingPool, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueued, ...)

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

            if (TransmissionType == TransmissionTypes.Enqueued)
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

                lock(DataAndStatusLock)
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

                        FlushEVSEDataTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

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

        #region DeleteStaticData(ChargingPool, TransmissionType = Enqueued, ...)

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


        #region SetStaticData   (ChargingPools, TransmissionType = Enqueued, ...)

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

        #region AddStaticData   (ChargingPools, TransmissionType = Enqueued, ...)

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

        #region UpdateStaticData(ChargingPools, TransmissionType = Enqueued, ...)

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

        #region DeleteStaticData(ChargingPools, TransmissionType = Enqueued, ...)

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


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueued, ...)

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
        Task<PushAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                           TransmissionType,

                                               DateTime?                                   Timestamp,
                                               CancellationToken?                          CancellationToken,
                                               EventTracking_Id                            EventTrackingId,
                                               TimeSpan?                                   RequestTimeout)


                => Task.FromResult(PushAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueued, ...)

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
        Task<PushStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingPoolStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                      TransmissionType,

                                     DateTime?                              Timestamp,
                                     CancellationToken?                     CancellationToken,
                                     EventTracking_Id                       EventTrackingId,
                                     TimeSpan?                              RequestTimeout)


                => Task.FromResult(PushStatusResult.NoOperation(Id, this));

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


        #region UpdateChargingStationOperatorAdminStatus(AdminStatusUpdates, TransmissionType = Enqueued, ...)

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
        Task<PushAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                                      TransmissionType,

                                               DateTime?                                              Timestamp,
                                               CancellationToken?                                     CancellationToken,
                                               EventTracking_Id                                       EventTrackingId,
                                               TimeSpan?                                              RequestTimeout)


                => Task.FromResult(PushAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateChargingStationOperatorStatus     (StatusUpdates,      TransmissionType = Enqueued, ...)

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
        Task<PushStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingStationOperatorStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                                 TransmissionType,

                                     DateTime?                                         Timestamp,
                                     CancellationToken?                                CancellationToken,
                                     EventTracking_Id                                  EventTrackingId,
                                     TimeSpan?                                         RequestTimeout)


                => Task.FromResult(PushStatusResult.NoOperation(Id, this));

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


        #region UpdateRoamingNetworkAdminStatus(AdminStatusUpdates, TransmissionType = Enqueued, ...)

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
        Task<PushAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                             TransmissionType,

                                               DateTime?                                     Timestamp,
                                               CancellationToken?                            CancellationToken,
                                               EventTracking_Id                              EventTrackingId,
                                               TimeSpan?                                     RequestTimeout)


                => Task.FromResult(PushAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateRoamingNetworkStatus     (StatusUpdates,      TransmissionType = Enqueued, ...)

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
        Task<PushStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<RoamingNetworkStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                        TransmissionType,

                                     DateTime?                                Timestamp,
                                     CancellationToken?                       CancellationToken,
                                     EventTracking_Id                         EventTrackingId,
                                     TimeSpan?                                RequestTimeout)


                => Task.FromResult(PushStatusResult.NoOperation(Id, this));

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
                                                        RequestTimeout).
                                         ConfigureAwait(false);


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
                                                         RequestTimeout).
                                          ConfigureAwait(false);


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
                           ChargingStation_Id           ChargingStationId,
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
                                                         RequestTimeout).
                                          ConfigureAwait(false);


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
                           ChargingPool_Id              ChargingPoolId,
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
                                                         RequestTimeout).
                                          ConfigureAwait(false);


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
                                                              RequestTimeout).ConfigureAwait(false);


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
                                                               RequestTimeout).ConfigureAwait(false);


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
                          ChargingStation_Id           ChargingStationId,
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
                                                               RequestTimeout).ConfigureAwait(false);


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
                          ChargingPool_Id              ChargingPoolId,
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
                                                               RequestTimeout).ConfigureAwait(false);


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

        #region SendChargeDetailRecords(ChargeDetailRecords, TransmissionType = Enqueued, ...)

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
                                    TransmissionTypes                     TransmissionType    = TransmissionTypes.Enqueued,

                                    DateTime?                             Timestamp           = null,
                                    CancellationToken?                    CancellationToken   = null,
                                    EventTracking_Id                      EventTrackingId     = null,
                                    TimeSpan?                             RequestTimeout      = null)

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

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueued)
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
                    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsRequest));
                }

                #endregion

                lock(DataAndStatusLock)
                {

                    ChargeDetailRecordQueue.AddRange(ChargeDetailRecords);

                    FlushEVSEDataTimer.Change(_FlushChargeDetailRecordsEvery, Timeout.Infinite);

                }

                return SendCDRsResult.Enqueued(Id, this);

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
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsRequest));
            }

            #endregion


            DateTime        Endtime;
            TimeSpan        Runtime;
            SendCDRsResult  result;

            if (DisableSendChargeDetailRecords)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = SendCDRsResult.AdminDown(Id,
                                                    this,
                                                    ChargeDetailRecords,
                                                    Runtime: Runtime);

            }

            else
            {

                HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>> response;
                var SendCDRsResults = new Dictionary<WWCP.ChargeDetailRecord, SendCDRResultTypes>();

                foreach (var _ChargeDetailRecord in ChargeDetailRecords)
                {

                    response = await CPORoaming.SendChargeDetailRecord(_ChargeDetailRecord.ToOICP(_WWCPChargeDetailRecord2OICPChargeDetailRecord),

                                                                       Timestamp,
                                                                       CancellationToken,
                                                                       EventTrackingId,
                                                                       RequestTimeout).ConfigureAwait(false);

                    if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                        response.Content        != null              &&
                        response.Content.Result)
                    {
                        SendCDRsResults.Add(_ChargeDetailRecord, SendCDRResultTypes.Success);
                    }

                    else
                        SendCDRsResults.Add(_ChargeDetailRecord, SendCDRResultTypes.Error);

                }

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;

                if      (SendCDRsResults.All(cdrresult => cdrresult.Value == SendCDRResultTypes.Success))
                    result = SendCDRsResult.Success(Id, this);

                else
                    result = SendCDRsResult.Error  (Id,
                                                    this,
                                                    SendCDRsResults.
                                                        Where (cdrresult => cdrresult.Value != SendCDRResultTypes.Success).
                                                        Select(cdrresult => cdrresult.Key));

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

            lock (DataAndStatusLock)
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
                FlushEVSEDataTimer.Change(Timeout.Infinite, Timeout.Infinite);

            }

            #endregion


            // Use events to check if something went wrong!
            var EventTrackingId = EventTracking_Id.New;

            Thread.Sleep(30000);

            #region Send new EVSE data

            if (EVSEsToAddQueueCopy.Count > 0)
            {

                var EVSEsToAddTask = PushEVSEData(EVSEsToAddQueueCopy,
                                                  _FlushEVSEDataRunId == 1
                                                      ? ActionTypes.fullLoad
                                                      : ActionTypes.update,
                                                  EventTrackingId: EventTrackingId);

                EVSEsToAddTask.Wait();

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

            lock (DataAndStatusLock)
            {

                // Copy 'EVSE status changes', remove originals...
                EVSEStatusFastQueueCopy = new List<EVSEStatusUpdate>(EVSEStatusChangesFastQueue.Where(evsestatuschange => !EVSEsToAddQueue.Any(evse => evse == evsestatuschange.EVSE)));

                // Add all evse status changes of EVSE *NOT YET UPLOADED* into the delayed queue...
                var EVSEStatusChangesDelayed = EVSEStatusChangesFastQueue.Where(evsestatuschange => EVSEsToAddQueue.Any(evse => evse == evsestatuschange.EVSE)).ToArray();

                if (EVSEStatusChangesDelayed.Length > 0)
                    EVSEStatusChangesDelayedQueue.AddRange(EVSEStatusChangesDelayed);

                EVSEStatusChangesFastQueue.Clear();

                // Stop the timer. Will be rescheduled by next EVSE status change...
                StatusCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);

            }

            #endregion


            // Use events to check if something went wrong!
            var EventTrackingId = EventTracking_Id.New;

            #region Send changed EVSE status

            if (EVSEStatusFastQueueCopy.Count > 0)
            {

                var _PushEVSEStatus = await PushEVSEStatus(EVSEStatusFastQueueCopy,
                                                           ActionTypes.update,
                                                           EventTrackingId: EventTrackingId).
                                            ConfigureAwait(false);

            }

            #endregion

        }

        #endregion

        #region (timer) FlushChargeDetailRecords()

        protected override Boolean SkipFlushChargeDetailRecordsQueues()
            => ChargeDetailRecordQueue.Count == 0;

        protected override async Task FlushChargeDetailRecordsQueues()
        {

            #region Make a thread local copy of all data

            var ChargeDetailRecordQueueCopy        = new ThreadLocal<List<WWCP.ChargeDetailRecord>>();

            if (Monitor.TryEnter(FlushEVSEDataAndStatusLock))
            {

                try
                {

                    if (ChargeDetailRecordQueue.       Count == 0)
                    {
                        return;
                    }

                    _FlushEVSEDataRunId++;

                    // Copy 'EVSEs to remove', remove originals...
                    ChargeDetailRecordQueueCopy.Value        = new List<WWCP.ChargeDetailRecord>(ChargeDetailRecordQueue);
                    ChargeDetailRecordQueue.Clear();

                    // Stop the timer. Will be rescheduled by next EVSE data/status change...
                    FlushEVSEDataTimer.Change(Timeout.Infinite, Timeout.Infinite);

                }
                catch (Exception e)
                {

                    while (e.InnerException != null)
                        e = e.InnerException;

                    DebugX.LogT(nameof(WWCPCPOAdapter) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                }

                finally
                {
                    Monitor.Exit(FlushEVSEDataAndStatusLock);
                }

            }

            else
            {

                Console.WriteLine("ServiceCheckLock missed!");
                FlushEVSEDataTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

            }

            #endregion


            // Upload status changes...
            if (ChargeDetailRecordQueueCopy.Value != null)
            {

                // Use the events to evaluate if something went wrong!

                var EventTrackingId = EventTracking_Id.New;

                #region Send charge detail records

                if (ChargeDetailRecordQueueCopy.Value.Count > 0)
                {

                    var SendCDRResults = await SendChargeDetailRecords(ChargeDetailRecordQueueCopy.Value,
                                                                       TransmissionTypes.Direct,
                                                                       DateTime.UtcNow,
                                                                       new CancellationTokenSource().Token,
                                                                       EventTrackingId,
                                                                       DefaultRequestTimeout).
                                               ConfigureAwait(false);

                    //ToDo: Send results events...
                    //ToDo: Read to queue if it could not be sent...

                }

                #endregion

            }

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => "OICP" + Version.Number + " CPO Adapter " + Id;

        #endregion


    }

}
