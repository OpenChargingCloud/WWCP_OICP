/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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
    public class WWCPCPOAdapter : ABaseEMobilityEntity<CSORoamingProvider_Id>,
                                  ICSORoamingProvider,
                                  IEquatable <WWCPCPOAdapter>,
                                  IComparable<WWCPCPOAdapter>,
                                  IComparable
    {

        #region Data

        private        readonly  IRemotePushData                               _IRemotePushData;

        private        readonly  IRemotePushStatus                             _IRemotePushStatus;

        private        readonly  EVSE2EVSEDataRecordDelegate                   _EVSE2EVSEDataRecord;

        private        readonly  EVSEStatusUpdate2EVSEStatusRecordDelegate     _EVSEStatusUpdate2EVSEStatusRecord;

        private        readonly  EVSEDataRecord2XMLDelegate                    _EVSEDataRecord2XML;

        private        readonly  EVSEStatusRecord2XMLDelegate                  _EVSEStatusRecord2XML;

        private        readonly  ChargingStationOperatorNameSelectorDelegate   _OperatorNameSelector;

        private static readonly  Regex                                         pattern = new Regex(@"\s=\s");

        public  static readonly  ChargingStationOperatorNameSelectorDelegate   DefaultOperatorNameSelector = I18N => I18N.FirstText;

                /// <summary>
        /// The default service check intervall.
        /// </summary>
        public  readonly static TimeSpan                                       DefaultServiceCheckEvery = TimeSpan.FromSeconds(31);

        /// <summary>
        /// The default status check intervall.
        /// </summary>
        public  readonly static TimeSpan                                       DefaultStatusCheckEvery  = TimeSpan.FromSeconds(3);


        private readonly        Object                                         ServiceCheckLock;
        private readonly        Timer                                          ServiceCheckTimer;
        private readonly        Object                                         StatusCheckLock;
        private readonly        Timer                                          StatusCheckTimer;

        private readonly        HashSet<EVSE>                                  EVSEsToAddQueue;
        private readonly        HashSet<EVSE>                                  EVSEsToUpdateQueue;
        private readonly        List<EVSEStatusUpdate>                         EVSEStatusChangesFastQueue;
        private readonly        List<EVSEStatusUpdate>                         EVSEStatusChangesDelayedQueue;
        private readonly        HashSet<EVSE>                                  EVSEsToRemoveQueue;
        private readonly        List<WWCP.ChargeDetailRecord>                  ChargeDetailRecordQueue;

        private                 UInt64                                         _ServiceRunId;
        private                 UInt64                                         _StatusRunId;
        private                 IncludeEVSEDelegate                            _IncludeEVSEs;

        public readonly static TimeSpan DefaultRequestTimeout = TimeSpan.FromSeconds(30);

        #endregion

        #region Properties

        #region Name

        /// <summary>
        /// The offical (multi-language) name of the roaming provider.
        /// </summary>
        [Mandatory]
        public I18NString Name { get; }

        #endregion

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
        /// The attached DNS server.
        /// </summary>
        public DNSClient DNSClient
            => CPORoaming.DNSClient;




        #region ServiceCheckEvery

        private UInt32 _ServiceCheckEvery;

        /// <summary>
        /// The service check intervall.
        /// </summary>
        public TimeSpan ServiceCheckEvery
        {

            get
            {
                return TimeSpan.FromSeconds(_ServiceCheckEvery);
            }

            set
            {
                _ServiceCheckEvery = (UInt32)value.TotalSeconds;
            }

        }

        #endregion

        #region StatusCheckEvery

        private UInt32 _StatusCheckEvery;

        /// <summary>
        /// The status check intervall.
        /// </summary>
        public TimeSpan StatusCheckEvery
        {

            get
            {
                return TimeSpan.FromSeconds(_StatusCheckEvery);
            }

            set
            {
                _StatusCheckEvery = (UInt32)value.TotalSeconds;
            }

        }

        #endregion


        #region DisablePushData

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisablePushData                  { get; set; }

        #endregion

        #region DisablePushStatus

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisablePushStatus                { get; set; }

        #endregion

        #region DisableAuthentication

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisableAuthentication            { get; set; }

        #endregion

        #region DisableSendChargeDetailRecords

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisableSendChargeDetailRecords   { get; set; }

        #endregion


        /// <summary>
        /// An optional default charging station operator identification.
        /// </summary>
        public Operator_Id  DefaultOperatorId     { get; }

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
        public event OnSendCDRRequestDelegate   OnEnqueueSendCDRRequest;

        /// <summary>
        /// An event fired whenever a charge detail record will be send upstream.
        /// </summary>
        public event OnSendCDRRequestDelegate   OnSendCDRRequest;

        /// <summary>
        /// An event fired whenever a charge detail record had been sent upstream.
        /// </summary>
        public event OnSendCDRResponseDelegate  OnSendCDRResponse;

        #endregion


        #region OnWWCPCPOAdapterException

        public delegate Task OnWWCPCPOAdapterExceptionDelegate(DateTime        Timestamp,
                                                               WWCPCPOAdapter  Sender,
                                                               Exception       Exception);

        public event OnWWCPCPOAdapterExceptionDelegate OnWWCPCPOAdapterException;

        #endregion

        #endregion

        #region Constructor(s)

        #region WWCPCPOAdapter(Id, Name, RoamingNetwork, CPORoaming, EVSE2EVSEDataRecord = null)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for Charging Station Operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="CPORoaming">A OICP CPO roaming object to be mapped to WWCP.</param>
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// 
        /// <param name="DefaultOperator">An optional Charging Station Operator, which will be copied into the main OperatorID-section of the OICP SOAP request.</param>
        /// <param name="OperatorNameSelector">An optional delegate to select an Charging Station Operator name, which will be copied into the OperatorName-section of the OICP SOAP request.</param>
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// 
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        public WWCPCPOAdapter(CSORoamingProvider_Id                        Id,
                              I18NString                                   Name,
                              RoamingNetwork                               RoamingNetwork,

                              CPORoaming                                   CPORoaming,
                              EVSE2EVSEDataRecordDelegate                  EVSE2EVSEDataRecord                 = null,
                              EVSEStatusUpdate2EVSEStatusRecordDelegate    EVSEStatusUpdate2EVSEStatusRecord   = null,
                              EVSEDataRecord2XMLDelegate                   EVSEDataRecord2XML                  = null,
                              EVSEStatusRecord2XMLDelegate                 EVSEStatusRecord2XML                = null,

                              ChargingStationOperator                      DefaultOperator                     = null,
                              ChargingStationOperatorNameSelectorDelegate  OperatorNameSelector                = null,
                              IncludeEVSEDelegate                          IncludeEVSEs                        = null,
                              TimeSpan?                                    ServiceCheckEvery                   = null,
                              TimeSpan?                                    StatusCheckEvery                    = null,

                              Boolean                                      DisablePushData                     = false,
                              Boolean                                      DisablePushStatus                   = false,
                              Boolean                                      DisableAuthentication               = false,
                              Boolean                                      DisableSendChargeDetailRecords      = false)

            : base(Id,
                   RoamingNetwork)

        {

            #region Initial checks

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),        "The given roaming provider name must not be null or empty!");

            if (CPORoaming == null)
                throw new ArgumentNullException(nameof(CPORoaming),  "The given OICP CPO Roaming object must not be null!");

            #endregion

            this.Name = Name;
            this._IRemotePushData                     = this as IRemotePushData;
            this._IRemotePushStatus                   = this as IRemotePushStatus;

            this.CPORoaming                           = CPORoaming;
            this._EVSE2EVSEDataRecord                 = EVSE2EVSEDataRecord;
            this._EVSEStatusUpdate2EVSEStatusRecord   = EVSEStatusUpdate2EVSEStatusRecord;
            this._EVSEDataRecord2XML                  = EVSEDataRecord2XML;
            this._EVSEStatusRecord2XML                = EVSEStatusRecord2XML;
            this.DefaultOperatorId                    = DefaultOperator.Id.ToOICP();
            this.DefaultOperatorName                  = DefaultOperatorNameSelector(DefaultOperator.Name);
            this._OperatorNameSelector                = OperatorNameSelector;


            this._IncludeEVSEs                        = IncludeEVSEs;

            this._ServiceCheckEvery                   = (UInt32) (ServiceCheckEvery.HasValue
                                                                     ? ServiceCheckEvery.Value. TotalMilliseconds
                                                                     : DefaultServiceCheckEvery.TotalMilliseconds);

            this.ServiceCheckLock                     = new Object();
            this.ServiceCheckTimer                    = new Timer(ServiceCheck, null, 0, _ServiceCheckEvery);

            this._StatusCheckEvery                    = (UInt32) (StatusCheckEvery.HasValue
                                                                     ? StatusCheckEvery.Value.  TotalMilliseconds
                                                                     : DefaultStatusCheckEvery. TotalMilliseconds);

            this.StatusCheckLock                      = new Object();
            this.StatusCheckTimer                     = new Timer(StatusCheck, null, 0, _StatusCheckEvery);

            this.DisablePushData                      = DisablePushData;
            this.DisablePushStatus                    = DisablePushStatus;
            this.DisableAuthentication                = DisableAuthentication;
            this.DisableSendChargeDetailRecords       = DisableSendChargeDetailRecords;

            this.EVSEsToAddQueue                      = new HashSet<EVSE>();
            this.EVSEsToUpdateQueue                   = new HashSet<EVSE>();
            this.EVSEStatusChangesFastQueue           = new List<EVSEStatusUpdate>();
            this.EVSEStatusChangesDelayedQueue        = new List<EVSEStatusUpdate>();
            this.EVSEsToRemoveQueue                   = new HashSet<EVSE>();
            this.ChargeDetailRecordQueue              = new List<WWCP.ChargeDetailRecord>();


            // Link events...

            #region OnRemoteReservationStart

            this.CPORoaming.OnRemoteReservationStart += async (Timestamp,
                                                               Sender,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               EVSEId,
                                                               PartnerProductId,
                                                               SessionId,
                                                               PartnerSessionId,
                                                               ProviderId,
                                                               EVCOId,
                                                               RequestTimeout) => {


                #region Request transformation

                TimeSpan? Duration   = null;
                DateTime? StartTime  = null;

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

                var response = await RoamingNetwork.Reserve(EVSEId.ToWWCP(),
                                                            StartTime:          StartTime,
                                                            Duration:           Duration,

                                                            // Always create a reservation identification usable for OICP!
                                                            ReservationId:      ChargingReservation_Id.Parse(
                                                                                    EVSEId.OperatorId.ToWWCP(),
                                                                                    SessionId.HasValue
                                                                                        ? SessionId.ToString()
                                                                                        : Session_Id.NewRandom.ToString()
                                                                                ),

                                                            ProviderId:         ProviderId.      ToWWCP(),
                                                            eMAId:              EVCOId.          ToWWCP(),
                                                            ChargingProduct:    PartnerProductId.HasValue
                                                                                    ? new ChargingProduct(PartnerProductId.Value.ToWWCP())
                                                                                    : null,

                                                            eMAIds:             new eMobilityAccount_Id[] {
                                                                                    EVCOId.Value.ToWWCP()
                                                                                },

                                                            Timestamp:          Timestamp,
                                                            CancellationToken:  CancellationToken,
                                                            EventTrackingId:    EventTrackingId,
                                                            RequestTimeout:     RequestTimeout).ConfigureAwait(false);

                #region Response mapping

                if (response != null)
                {
                    switch (response.Result)
                    {

                        case ReservationResultType.Success:
                            return Acknowledgement.Success(

                                       response.Reservation != null
                                           ? Session_Id.Parse(response.Reservation.Id.Suffix)
                                           : new Session_Id?(),

                                       StatusCodeDescription :    "Reservation successful!",
                                       StatusCodeAdditionalInfo:  response.Reservation != null ? "ReservationId: " + response.Reservation.Id : null

                                   );

                        case ReservationResultType.InvalidCredentials:
                            return Acknowledgement.SessionIsInvalid(
                                       SessionId: Session_Id.Parse(response.Reservation.Id.ToString())
                                   );

                        case ReservationResultType.Timeout:
                        case ReservationResultType.CommunicationError:
                            return Acknowledgement.CommunicationToEVSEFailed();

                        case ReservationResultType.AlreadyReserved:
                            return Acknowledgement.EVSEAlreadyReserved();

                        case ReservationResultType.AlreadyInUse:
                            return Acknowledgement.EVSEAlreadyInUse_WrongToken();

                        case ReservationResultType.UnknownEVSE:
                            return Acknowledgement.UnknownEVSEID();

                        case ReservationResultType.OutOfService:
                            return Acknowledgement.EVSEOutOfService();

                    }
                }

                return Acknowledgement.ServiceNotAvailable(
                           SessionId: Session_Id.Parse(response.Reservation.Id.ToString())
                       );

                #endregion

            };

            #endregion

            #region OnRemoteReservationStop

            this.CPORoaming.OnRemoteReservationStop += async (Timestamp,
                                                              Sender,
                                                              CancellationToken,
                                                              EventTrackingId,
                                                              EVSEId,
                                                              SessionId,
                                                              PartnerSessionId,
                                                              ProviderId,
                                                              RequestTimeout) => {

                var response = await RoamingNetwork.CancelReservation(ChargingReservation_Id.Parse(
                                                                          EVSEId.OperatorId.ToWWCP(),
                                                                          SessionId.ToString()
                                                                      ),
                                                                      ChargingReservationCancellationReason.Deleted,
                                                                      ProviderId.ToWWCP(),
                                                                      EVSEId.    ToWWCP(),

                                                                      Timestamp,
                                                                      CancellationToken,
                                                                      EventTrackingId,
                                                                      RequestTimeout).ConfigureAwait(false);

                #region Response mapping

                if (response != null)
                {
                    switch (response.Result)
                    {

                        case CancelReservationResults.Success:
                            return Acknowledgement.Success(
                                       StatusCodeDescription: "Reservation deleted!"
                                   );

                        case CancelReservationResults.UnknownReservationId:
                            return Acknowledgement.SessionIsInvalid(
                                       SessionId: SessionId
                                   );

                        case CancelReservationResults.Offline:
                        case CancelReservationResults.Timeout:
                        case CancelReservationResults.CommunicationError:
                            return Acknowledgement.CommunicationToEVSEFailed();

                        case CancelReservationResults.UnknownEVSE:
                            return Acknowledgement.UnknownEVSEID();

                        case CancelReservationResults.OutOfService:
                            return Acknowledgement.EVSEOutOfService();

                    }
                }

                return Acknowledgement.ServiceNotAvailable(
                           SessionId: SessionId
                       );

                #endregion

            };

            #endregion

            #region OnRemoteStart

            this.CPORoaming.OnRemoteStart += async (Timestamp,
                                                    Sender,
                                                    CancellationToken,
                                                    EventTrackingId,
                                                    EVSEId,
                                                    PartnerProductId,
                                                    SessionId,
                                                    PartnerSessionId,
                                                    ProviderId,
                                                    EVCOId,
                                                    RequestTimeout) => {

                #region Request mapping

                ChargingReservation_Id? ReservationId    = null;
                TimeSpan?               MinDuration      = null;
                Single?                 PlannedEnergy    = null;
                ChargingProduct_Id?     ProductId        = ChargingProduct_Id.Parse("AC1");
                ChargingProduct         ChargingProduct  = null;

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
                                ChargingReservation_Id.TryParse(EVSEId.OperatorId.ToWWCP(), ProductIdElements["R"], out _ReservationId))
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
                                         RemoteStart(EVSEId.    ToWWCP(),
                                                     ChargingProduct,
                                                     ReservationId,
                                                     SessionId. ToWWCP(),
                                                     ProviderId.ToWWCP(),
                                                     EVCOId.    ToWWCP(),

                                                     Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     RequestTimeout).ConfigureAwait(false);

                #region Response mapping

                if (response != null)
                {
                    switch (response.Result)
                    {

                        case RemoteStartEVSEResultType.Success:
                            return Acknowledgement.Success(
                                       response.Session.Id.ToOICP(),
                                       StatusCodeDescription: "Ready to charge!"
                                   );

                        case RemoteStartEVSEResultType.InvalidSessionId:
                            return Acknowledgement.SessionIsInvalid(
                                       SessionId: SessionId
                                   );

                        case RemoteStartEVSEResultType.InvalidCredentials:
                            return Acknowledgement.NoValidContract();

                        case RemoteStartEVSEResultType.Offline:
                            return Acknowledgement.CommunicationToEVSEFailed();

                        case RemoteStartEVSEResultType.Timeout:
                        case RemoteStartEVSEResultType.CommunicationError:
                            return Acknowledgement.CommunicationToEVSEFailed();

                        case RemoteStartEVSEResultType.Reserved:
                            return Acknowledgement.EVSEAlreadyReserved();

                        case RemoteStartEVSEResultType.AlreadyInUse:
                            return Acknowledgement.EVSEAlreadyInUse_WrongToken();

                        case RemoteStartEVSEResultType.UnknownEVSE:
                            return Acknowledgement.UnknownEVSEID();

                        case RemoteStartEVSEResultType.OutOfService:
                            return Acknowledgement.EVSEOutOfService();

                    }
                }

                return Acknowledgement.ServiceNotAvailable(
                           SessionId: SessionId
                       );

                #endregion

            };

            #endregion

            #region OnRemoteStop

            this.CPORoaming.OnRemoteStop += async (Timestamp,
                                                   Sender,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   EVSEId,
                                                   SessionId,
                                                   PartnerSessionId,
                                                   ProviderId,
                                                   RequestTimeout) => {

                var response = await RoamingNetwork.RemoteStop(EVSEId.ToWWCP(),
                                                               SessionId. ToWWCP(),
                                                               ReservationHandling.Close,
                                                               ProviderId.ToWWCP(),
                                                               null,

                                                               Timestamp,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               RequestTimeout).ConfigureAwait(false);

                #region Response mapping

                if (response != null)
                {
                    switch (response.Result)
                    {

                        case RemoteStopEVSEResultType.Success:
                            return Acknowledgement.Success(
                                       response.SessionId.ToOICP(),
                                       StatusCodeDescription: "Ready to stop charging!"
                                   );

                        case RemoteStopEVSEResultType.InvalidSessionId:
                            return Acknowledgement.SessionIsInvalid(
                                       SessionId: SessionId
                                   );

                        case RemoteStopEVSEResultType.Offline:
                        case RemoteStopEVSEResultType.Timeout:
                        case RemoteStopEVSEResultType.CommunicationError:
                            return Acknowledgement.CommunicationToEVSEFailed();

                        case RemoteStopEVSEResultType.UnknownEVSE:
                            return Acknowledgement.UnknownEVSEID();

                        case RemoteStopEVSEResultType.OutOfService:
                            return Acknowledgement.EVSEOutOfService();

                    }
                }

                return Acknowledgement.ServiceNotAvailable(
                           SessionId: SessionId
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
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
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
        public WWCPCPOAdapter(CSORoamingProvider_Id                        Id,
                              I18NString                                   Name,
                              RoamingNetwork                               RoamingNetwork,

                              CPOClient                                    CPOClient,
                              CPOServer                                    CPOServer,
                              String                                       ServerLoggingContext                = CPOServerLogger.DefaultContext,
                              Func<String, String, String>                 LogFileCreator                      = null,

                              EVSE2EVSEDataRecordDelegate                  EVSE2EVSEDataRecord                 = null,
                              EVSEStatusUpdate2EVSEStatusRecordDelegate    EVSEStatusUpdate2EVSEStatusRecord   = null,
                              EVSEDataRecord2XMLDelegate                   EVSEDataRecord2XML                  = null,
                              EVSEStatusRecord2XMLDelegate                 EVSEStatusRecord2XML                = null,

                              ChargingStationOperator                      DefaultOperator                     = null,
                              ChargingStationOperatorNameSelectorDelegate  OperatorNameSelector                = null,
                              IncludeEVSEDelegate                          IncludeEVSEs                        = null,
                              TimeSpan?                                    ServiceCheckEvery                   = null,
                              TimeSpan?                                    StatusCheckEvery                    = null,

                              Boolean                                      DisablePushData                     = false,
                              Boolean                                      DisablePushStatus                   = false,
                              Boolean                                      DisableAuthentication               = false,
                              Boolean                                      DisableSendChargeDetailRecords      = false)

            : this(Id,
                   Name,
                   RoamingNetwork,

                   new CPORoaming(CPOClient,
                                  CPOServer,
                                  ServerLoggingContext,
                                  LogFileCreator),

                   EVSE2EVSEDataRecord,
                   EVSEStatusUpdate2EVSEStatusRecord,
                   EVSEDataRecord2XML,
                   EVSEStatusRecord2XML,

                   DefaultOperator,
                   OperatorNameSelector,
                   IncludeEVSEs,
                   ServiceCheckEvery,
                   StatusCheckEvery,

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
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="RemoteHostname">The hostname of the remote OICP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerContentType">An optional HTTP content type to use.</param>
        /// <param name="ServerRegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
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
        public WWCPCPOAdapter(CSORoamingProvider_Id                        Id,
                              I18NString                                   Name,
                              RoamingNetwork                               RoamingNetwork,

                              String                                       RemoteHostname,
                              IPPort                                       RemoteTCPPort                       = null,
                              RemoteCertificateValidationCallback          RemoteCertificateValidator          = null,
                              X509Certificate                              ClientCert                          = null,
                              String                                       RemoteHTTPVirtualHost               = null,
                              String                                       URIPrefix                           = CPOClient.DefaultURIPrefix,
                              String                                       HTTPUserAgent                       = CPOClient.DefaultHTTPUserAgent,
                              TimeSpan?                                    RequestTimeout                      = null,

                              String                                       ServerName                          = CPOServer.DefaultHTTPServerName,
                              IPPort                                       ServerTCPPort                       = null,
                              String                                       ServerURIPrefix                     = CPOServer.DefaultURIPrefix,
                              HTTPContentType                              ServerContentType                   = null,
                              Boolean                                      ServerRegisterHTTPRootService       = true,
                              Boolean                                      ServerAutoStart                     = false,

                              String                                       ClientLoggingContext                = CPOClient.CPOClientLogger.DefaultContext,
                              String                                       ServerLoggingContext                = CPOServerLogger.DefaultContext,
                              Func<String, String, String>                 LogFileCreator                      = null,

                              EVSE2EVSEDataRecordDelegate                  EVSE2EVSEDataRecord                 = null,
                              EVSEStatusUpdate2EVSEStatusRecordDelegate    EVSEStatusUpdate2EVSEStatusRecord   = null,
                              EVSEDataRecord2XMLDelegate                   EVSEDataRecord2XML                  = null,
                              EVSEStatusRecord2XMLDelegate                 EVSEStatusRecord2XML                = null,

                              ChargingStationOperator                      DefaultOperator                     = null,
                              ChargingStationOperatorNameSelectorDelegate  OperatorNameSelector                = null,
                              IncludeEVSEDelegate                          IncludeEVSEs                        = null,
                              TimeSpan?                                    ServiceCheckEvery                   = null,
                              TimeSpan?                                    StatusCheckEvery                    = null,

                              Boolean                                      DisablePushData                     = false,
                              Boolean                                      DisablePushStatus                   = false,
                              Boolean                                      DisableAuthentication               = false,
                              Boolean                                      DisableSendChargeDetailRecords      = false,

                              DNSClient                                    DNSClient                           = null)

            : this(Id,
                   Name,
                   RoamingNetwork,

                   new CPORoaming(Id.ToString(),
                                  RemoteHostname,
                                  RemoteTCPPort,
                                  RemoteCertificateValidator,
                                  ClientCert,
                                  RemoteHTTPVirtualHost,
                                  URIPrefix,
                                  HTTPUserAgent,
                                  RequestTimeout,

                                  ServerName,
                                  ServerTCPPort,
                                  ServerURIPrefix,
                                  ServerContentType,
                                  ServerRegisterHTTPRootService,
                                  false,

                                  ClientLoggingContext,
                                  ServerLoggingContext,
                                  LogFileCreator,

                                  DNSClient),

                   EVSE2EVSEDataRecord,
                   EVSEStatusUpdate2EVSEStatusRecord,
                   EVSEDataRecord2XML,
                   EVSEStatusRecord2XML,

                   DefaultOperator,
                   OperatorNameSelector,
                   IncludeEVSEs,
                   ServiceCheckEvery,
                   StatusCheckEvery,

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
        private async Task<WWCP.Acknowledgement>

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
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Get effective number of EVSE status to upload

            var Warnings = new List<String>();

            var _EVSEs = EVSEs.Where (evse => evse != null && _IncludeEVSEs(evse)).
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

            WWCP.Acknowledgement result;

            #endregion

            #region Send OnPushEVSEDataWWCPRequest event

            var StartTime = DateTime.Now;

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


            var Endtime = DateTime.Now;
            var Runtime = Endtime - StartTime;

            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        != null)
            {

                if (response.Content.Result)
                    result = new WWCP.Acknowledgement(ResultType.True,
                                                      response.Content.StatusCode.Description,
                                                      response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                          ? Warnings.AddAndReturnList(response.Content.StatusCode.AdditionalInfo)
                                                          : Warnings,
                                                      Runtime);

                else
                    result = new WWCP.Acknowledgement(ResultType.False,
                                                      response.Content.StatusCode.Description,
                                                      response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                          ? Warnings.AddAndReturnList(response.Content.StatusCode.AdditionalInfo)
                                                          : Warnings,
                                                      Runtime);

            }
            else
                result = new WWCP.Acknowledgement(ResultType.False,
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
                                                   Warnings.Where(warning => warning.IsNotNullOrEmpty()),
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
        public async Task<WWCP.Acknowledgement>

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
                Timestamp = DateTime.Now;

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
                                  Where       (evsestatusupdate => _IncludeEVSEs(RoamingNetwork.GetEVSEbyId(evsestatusupdate.Id))).
                                  ToLookup    (evsestatusupdate => evsestatusupdate.Id,
                                               evsestatusupdate => evsestatusupdate).
                                  ToDictionary(group            => group.Key,
                                               group            => group.AsEnumerable().OrderByDescending(item => item.NewStatus.Timestamp)).
                                  Select      (evsestatusupdate => {

                                      try
                                      {

                                          // Only push the current status of the latest status update!
                                          return new EVSEStatusRecord(
                                                     evsestatusupdate.Key.ToOICP(),
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

            WWCP.Acknowledgement result = null;

            #endregion

            #region Send OnEVSEStatusPush event

            var StartTime = DateTime.Now;

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


            var Endtime = DateTime.Now;
            var Runtime = Endtime - StartTime;

            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        != null)
            {

                if (response.Content.Result)
                    result = new WWCP.Acknowledgement(ResultType.True,
                                                      response.Content.StatusCode.Description,
                                                      response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                          ? Warnings.AddAndReturnList(response.Content.StatusCode.AdditionalInfo)
                                                          : Warnings,
                                                      Runtime);

                else
                    result = new WWCP.Acknowledgement(ResultType.False,
                                                      response.Content.StatusCode.Description,
                                                      response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                          ? Warnings.AddAndReturnList(response.Content.StatusCode.AdditionalInfo)
                                                          : Warnings,
                                                      Runtime);

            }
            else
                result = new WWCP.Acknowledgement(ResultType.False,
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
                                                     Warnings.Where(warning => warning.IsNotNullOrEmpty()),
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
        async Task<WWCP.Acknowledgement>

            IRemotePushData.SetStaticData(EVSE                EVSE,
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

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.Now,
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

                lock (ServiceCheckLock)
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        EVSEsToAddQueue.Add(EVSE);

                        ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                    }

                }

                return new WWCP.Acknowledgement(ResultType.True);

            }

            #endregion


            return await PushEVSEData(new EVSE[] { EVSE },
                                      ActionTypes.fullLoad,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

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
        async Task<WWCP.Acknowledgement>

            IRemotePushData.AddStaticData(EVSE                EVSE,
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

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.Now,
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

                lock (ServiceCheckLock)
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        EVSEsToAddQueue.Add(EVSE);

                        ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                    }

                }

                return new WWCP.Acknowledgement(ResultType.True);

            }

            #endregion


            return await PushEVSEData(new EVSE[] { EVSE },
                                      ActionTypes.insert,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

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
        async Task<WWCP.Acknowledgement>

            IRemotePushData.UpdateStaticData(EVSE                EVSE,
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

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueued)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.Now,
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

                lock (ServiceCheckLock)
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        EVSEsToUpdateQueue.Add(EVSE);

                        ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                    }

                }

                return new WWCP.Acknowledgement(ResultType.True);

            }

            #endregion


            return await PushEVSEData(new EVSE[] { EVSE },
                                      ActionTypes.update,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

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
        async Task<WWCP.Acknowledgement>

            IRemotePushData.DeleteStaticData(EVSE                EVSE,
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

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.Now,
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

                lock (ServiceCheckLock)
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        EVSEsToRemoveQueue.Add(EVSE);

                        ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                    }

                }

                return new WWCP.Acknowledgement(ResultType.True);

            }

            #endregion


            return await PushEVSEData(new EVSE[] { EVSE },
                                      ActionTypes.delete,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion


        #region SetStaticData   (EVSEs, ...)

        /// <summary>
        /// Set the given enumeration of EVSEs as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.SetStaticData(IEnumerable<EVSE>   EVSEs,

                                          DateTime?           Timestamp,
                                          CancellationToken?  CancellationToken,
                                          EventTracking_Id    EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

            #endregion

            return await PushEVSEData(EVSEs,
                                      ActionTypes.fullLoad,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region AddStaticData   (EVSEs, ...)

        /// <summary>
        /// Add the given enumeration of EVSEs to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.AddStaticData(IEnumerable<EVSE>   EVSEs,

                                          DateTime?           Timestamp,
                                          CancellationToken?  CancellationToken,
                                          EventTracking_Id    EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

            #endregion

            return await PushEVSEData(EVSEs,
                                      ActionTypes.insert,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region UpdateStaticData(EVSEs, ...)

        /// <summary>
        /// Update the given enumeration of EVSEs within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.UpdateStaticData(IEnumerable<EVSE>   EVSEs,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

            #endregion

            return await PushEVSEData(EVSEs,
                                      ActionTypes.update,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region DeleteStaticData(EVSEs, ...)

        /// <summary>
        /// Delete the given enumeration of EVSEs from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.DeleteStaticData(IEnumerable<EVSE>   EVSEs,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

            #endregion

            return await PushEVSEData(EVSEs,
                                      ActionTypes.delete,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion


        #region UpdateEVSEAdminStatus(AdminStatusUpdates, TransmissionType = Enqueued, ...)

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
        Task<WWCP.Acknowledgement>

            IRemotePushStatus.UpdateEVSEAdminStatus(IEnumerable<EVSEAdminStatusUpdate>  AdminStatusUpdates,
                                                    TransmissionTypes                   TransmissionType,

                                                    DateTime?                           Timestamp,
                                                    CancellationToken?                  CancellationToken,
                                                    EventTracking_Id                    EventTrackingId,
                                                    TimeSpan?                           RequestTimeout)


                => Task.FromResult(new WWCP.Acknowledgement(ResultType.NoOperation));

        #endregion

        #region UpdateEVSEStatus     (StatusUpdates,      TransmissionType = Enqueued, ...)

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
        async Task<WWCP.Acknowledgement>

            IRemotePushStatus.UpdateEVSEStatus(IEnumerable<EVSEStatusUpdate>  StatusUpdates,
                                               TransmissionTypes              TransmissionType,

                                               DateTime?                      Timestamp,
                                               CancellationToken?             CancellationToken,
                                               EventTracking_Id               EventTrackingId,
                                               TimeSpan?                      RequestTimeout)

        {

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueued)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.Now,
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


                lock (ServiceCheckLock)
                {

                    //if (_IncludeEVSEs == null ||
                    //   (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    //{

                        EVSEStatusChangesFastQueue.AddRange(StatusUpdates);
                        StatusCheckTimer.Change(_StatusCheckEvery, Timeout.Infinite);

                    //}

                }

                return new WWCP.Acknowledgement(ResultType.True);

            }

            #endregion


            return await PushEVSEStatus(StatusUpdates,
                                        ActionTypes.update,

                                        Timestamp,
                                        CancellationToken,
                                        EventTrackingId,
                                        RequestTimeout).

                                        ConfigureAwait(false);

        }

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station(s)...

        #region SetStaticData   (ChargingStation, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.SetStaticData(ChargingStation     ChargingStation,

                                          DateTime?           Timestamp,
                                          CancellationToken?  CancellationToken,
                                          EventTracking_Id    EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

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

        #region AddStaticData   (ChargingStation, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.AddStaticData(ChargingStation     ChargingStation,

                                          DateTime?           Timestamp,
                                          CancellationToken?  CancellationToken,
                                          EventTracking_Id    EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

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

        #region UpdateStaticData(ChargingStation, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.UpdateStaticData(ChargingStation     ChargingStation,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            return await PushEVSEData(ChargingStation.EVSEs,
                                      ActionTypes.update,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region DeleteStaticData(ChargingStation, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.DeleteStaticData(ChargingStation     ChargingStation,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            return await PushEVSEData(ChargingStation.EVSEs,
                                      ActionTypes.delete,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion


        #region SetStaticData   (ChargingStations, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging stations as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.SetStaticData(IEnumerable<ChargingStation>  ChargingStations,

                                          DateTime?                     Timestamp,
                                          CancellationToken?            CancellationToken,
                                          EventTracking_Id              EventTrackingId,
                                          TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return await PushEVSEData(ChargingStations.SafeSelectMany(station => station.EVSEs),
                                      ActionTypes.fullLoad,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region AddStaticData   (ChargingStations, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging stations to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.AddStaticData(IEnumerable<ChargingStation>  ChargingStations,

                                          DateTime?                     Timestamp,
                                          CancellationToken?            CancellationToken,
                                          EventTracking_Id              EventTrackingId,
                                          TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return await PushEVSEData(ChargingStations.SafeSelectMany(station => station.EVSEs),
                                      ActionTypes.insert,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region UpdateStaticData(ChargingStations, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging stations within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.UpdateStaticData(IEnumerable<ChargingStation>  ChargingStations,

                                             DateTime?                     Timestamp,
                                             CancellationToken?            CancellationToken,
                                             EventTracking_Id              EventTrackingId,
                                             TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return await PushEVSEData(ChargingStations.SafeSelectMany(station => station.EVSEs),
                                      ActionTypes.update,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region DeleteStaticData(ChargingStations, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging stations from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.DeleteStaticData(IEnumerable<ChargingStation>  ChargingStations,

                                             DateTime?                     Timestamp,
                                             CancellationToken?            CancellationToken,
                                             EventTracking_Id              EventTrackingId,
                                             TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return await PushEVSEData(ChargingStations.SafeSelectMany(station => station.EVSEs),
                                      ActionTypes.delete,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion


        #region UpdateChargingStationAdminStatus(AdminStatusUpdates, TransmissionType = Enqueued, ...)

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
        Task<WWCP.Acknowledgement>

            IRemotePushStatus.UpdateChargingStationAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate>  AdminStatusUpdates,
                                                               TransmissionTypes                              TransmissionType,

                                                               DateTime?                                      Timestamp,
                                                               CancellationToken?                             CancellationToken,
                                                               EventTracking_Id                               EventTrackingId,
                                                               TimeSpan?                                      RequestTimeout)


                => Task.FromResult(new WWCP.Acknowledgement(ResultType.NoOperation));

        #endregion

        #region UpdateChargingStationStatus     (StatusUpdates,      TransmissionType = Enqueued, ...)

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
        Task<WWCP.Acknowledgement>

            IRemotePushStatus.UpdateChargingStationStatus(IEnumerable<ChargingStationStatusUpdate>  StatusUpdates,
                                                          TransmissionTypes                         TransmissionType,

                                                          DateTime?                                 Timestamp,
                                                          CancellationToken?                        CancellationToken,
                                                          EventTracking_Id                          EventTrackingId,
                                                          TimeSpan?                                 RequestTimeout)


                => Task.FromResult(new WWCP.Acknowledgement(ResultType.NoOperation));

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging pool(s)...

        #region SetStaticData   (ChargingPool, ...)

        /// <summary>
        /// Set the EVSE data of the given charging pool as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.SetStaticData(ChargingPool        ChargingPool,

                                          DateTime?           Timestamp,
                                          CancellationToken?  CancellationToken,
                                          EventTracking_Id    EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

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

        #region AddStaticData   (ChargingPool, ...)

        /// <summary>
        /// Add the EVSE data of the given charging pool to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.AddStaticData(ChargingPool        ChargingPool,

                                          DateTime?           Timestamp,
                                          CancellationToken?  CancellationToken,
                                          EventTracking_Id    EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

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

        #region UpdateStaticData(ChargingPool, ...)

        /// <summary>
        /// Update the EVSE data of the given charging pool within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.UpdateStaticData(ChargingPool        ChargingPool,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

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

        #region DeleteStaticData(ChargingPool, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging pool from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.DeleteStaticData(ChargingPool        ChargingPool,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            return await PushEVSEData(ChargingPool.EVSEs,
                                      ActionTypes.delete,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion


        #region SetStaticData   (ChargingPools, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging pools as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.SetStaticData(IEnumerable<ChargingPool>  ChargingPools,

                                          DateTime?                  Timestamp,
                                          CancellationToken?         CancellationToken,
                                          EventTracking_Id           EventTrackingId,
                                          TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return await PushEVSEData(ChargingPools.SafeSelectMany(pool => pool.EVSEs),
                                      ActionTypes.fullLoad,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region AddStaticData   (ChargingPools, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging pools to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.AddStaticData(IEnumerable<ChargingPool>  ChargingPools,

                                          DateTime?                  Timestamp,
                                          CancellationToken?         CancellationToken,
                                          EventTracking_Id           EventTrackingId,
                                          TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return await PushEVSEData(ChargingPools.SafeSelectMany(pool => pool.EVSEs),
                                      ActionTypes.insert,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region UpdateStaticData(ChargingPools, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging pools within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.UpdateStaticData(IEnumerable<ChargingPool>  ChargingPools,

                                             DateTime?                  Timestamp,
                                             CancellationToken?         CancellationToken,
                                             EventTracking_Id           EventTrackingId,
                                             TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return await PushEVSEData(ChargingPools.SafeSelectMany(pool => pool.EVSEs),
                                      ActionTypes.update,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion

        #region DeleteStaticData(ChargingPools, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging pools from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.Acknowledgement>

            IRemotePushData.DeleteStaticData(IEnumerable<ChargingPool>  ChargingPools,

                                             DateTime?                  Timestamp,
                                             CancellationToken?         CancellationToken,
                                             EventTracking_Id           EventTrackingId,
                                             TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return await PushEVSEData(ChargingPools.SafeSelectMany(pool => pool.EVSEs),
                                      ActionTypes.delete,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

        }

        #endregion


        #region UpdateChargingPoolAdminStatus(AdminStatusUpdates, TransmissionType = Enqueued, ...)

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
        Task<WWCP.Acknowledgement>

            IRemotePushStatus.UpdateChargingPoolAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate>  AdminStatusUpdates,
                                                            TransmissionTypes                           TransmissionType,

                                                            DateTime?                                   Timestamp,
                                                            CancellationToken?                          CancellationToken,
                                                            EventTracking_Id                            EventTrackingId,
                                                            TimeSpan?                                   RequestTimeout)


                => Task.FromResult(new WWCP.Acknowledgement(ResultType.NoOperation));

        #endregion

        #region UpdateChargingPoolStatus     (StatusUpdates,      TransmissionType = Enqueued, ...)

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
        Task<WWCP.Acknowledgement>

            IRemotePushStatus.UpdateChargingPoolStatus(IEnumerable<ChargingPoolStatusUpdate>  StatusUpdates,
                                                       TransmissionTypes                      TransmissionType,

                                                       DateTime?                              Timestamp,
                                                       CancellationToken?                     CancellationToken,
                                                       EventTracking_Id                       EventTrackingId,
                                                       TimeSpan?                              RequestTimeout)


                => Task.FromResult(new WWCP.Acknowledgement(ResultType.NoOperation));

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
        async Task<WWCP.Acknowledgement>

            IRemotePushData.SetStaticData(ChargingStationOperator  ChargingStationOperator,

                                          DateTime?                Timestamp,
                                          CancellationToken?       CancellationToken,
                                          EventTracking_Id         EventTrackingId,
                                          TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return await PushEVSEData(ChargingStationOperator.EVSEs,
                                      ActionTypes.fullLoad,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

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
        async Task<WWCP.Acknowledgement>

            IRemotePushData.AddStaticData(ChargingStationOperator  ChargingStationOperator,

                                          DateTime?                Timestamp,
                                          CancellationToken?       CancellationToken,
                                          EventTracking_Id         EventTrackingId,
                                          TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return await PushEVSEData(ChargingStationOperator.EVSEs,
                                      ActionTypes.insert,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

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
        async Task<WWCP.Acknowledgement>

            IRemotePushData.UpdateStaticData(ChargingStationOperator  ChargingStationOperator,

                                             DateTime?                Timestamp,
                                             CancellationToken?       CancellationToken,
                                             EventTracking_Id         EventTrackingId,
                                             TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return await PushEVSEData(ChargingStationOperator.EVSEs,
                                      ActionTypes.update,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

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
        async Task<WWCP.Acknowledgement>

            IRemotePushData.DeleteStaticData(ChargingStationOperator  ChargingStationOperator,

                                             DateTime?                Timestamp,
                                             CancellationToken?       CancellationToken,
                                             EventTracking_Id         EventTrackingId,
                                             TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return await PushEVSEData(ChargingStationOperator.EVSEs,
                                      ActionTypes.delete,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

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
        async Task<WWCP.Acknowledgement>

            IRemotePushData.SetStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                          DateTime?                             Timestamp,
                                          CancellationToken?                    CancellationToken,
                                          EventTracking_Id                      EventTrackingId,
                                          TimeSpan?                             RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return await PushEVSEData(ChargingStationOperators.SafeSelectMany(stationoperator => stationoperator.EVSEs),
                                      ActionTypes.fullLoad,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

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
        async Task<WWCP.Acknowledgement>

            IRemotePushData.AddStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                          DateTime?                             Timestamp,
                                          CancellationToken?                    CancellationToken,
                                          EventTracking_Id                      EventTrackingId,
                                          TimeSpan?                             RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return await PushEVSEData(ChargingStationOperators.SafeSelectMany(stationoperator => stationoperator.EVSEs),
                                      ActionTypes.insert,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

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
        async Task<WWCP.Acknowledgement>

            IRemotePushData.UpdateStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                             DateTime?                             Timestamp,
                                             CancellationToken?                    CancellationToken,
                                             EventTracking_Id                      EventTrackingId,
                                             TimeSpan?                             RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return await PushEVSEData(ChargingStationOperators.SafeSelectMany(stationoperator => stationoperator.EVSEs),
                                      ActionTypes.update,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

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
        async Task<WWCP.Acknowledgement>

            IRemotePushData.DeleteStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                             DateTime?                             Timestamp,
                                             CancellationToken?                    CancellationToken,
                                             EventTracking_Id                      EventTrackingId,
                                             TimeSpan?                             RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return await PushEVSEData(ChargingStationOperators.SafeSelectMany(stationoperator => stationoperator.EVSEs),
                                      ActionTypes.delete,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

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
        Task<WWCP.Acknowledgement>

            IRemotePushStatus.UpdateChargingStationOperatorAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate>  AdminStatusUpdates,
                                                                       TransmissionTypes                                      TransmissionType,

                                                                       DateTime?                                              Timestamp,
                                                                       CancellationToken?                                     CancellationToken,
                                                                       EventTracking_Id                                       EventTrackingId,
                                                                       TimeSpan?                                              RequestTimeout)


                => Task.FromResult(new WWCP.Acknowledgement(ResultType.NoOperation));

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
        Task<WWCP.Acknowledgement>

            IRemotePushStatus.UpdateChargingStationOperatorStatus(IEnumerable<ChargingStationOperatorStatusUpdate>  StatusUpdates,
                                                                  TransmissionTypes                                 TransmissionType,

                                                                  DateTime?                                         Timestamp,
                                                                  CancellationToken?                                CancellationToken,
                                                                  EventTracking_Id                                  EventTrackingId,
                                                                  TimeSpan?                                         RequestTimeout)


                => Task.FromResult(new WWCP.Acknowledgement(ResultType.NoOperation));

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
        async Task<WWCP.Acknowledgement>

            IRemotePushData.SetStaticData(RoamingNetwork      RoamingNetwork,

                                          DateTime?           Timestamp,
                                          CancellationToken?  CancellationToken,
                                          EventTracking_Id    EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return await PushEVSEData(RoamingNetwork.EVSEs,
                                      ActionTypes.fullLoad,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

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
        async Task<WWCP.Acknowledgement>

            IRemotePushData.AddStaticData(RoamingNetwork      RoamingNetwork,

                                          DateTime?           Timestamp,
                                          CancellationToken?  CancellationToken,
                                          EventTracking_Id    EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return await PushEVSEData(RoamingNetwork.EVSEs,
                                      ActionTypes.insert,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

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
        async Task<WWCP.Acknowledgement>

            IRemotePushData.UpdateStaticData(RoamingNetwork      RoamingNetwork,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return await PushEVSEData(RoamingNetwork.EVSEs,
                                      ActionTypes.update,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

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
        async Task<WWCP.Acknowledgement>

            IRemotePushData.DeleteStaticData(RoamingNetwork      RoamingNetwork,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return await PushEVSEData(RoamingNetwork.EVSEs,
                                      ActionTypes.delete,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout).

                                      ConfigureAwait(false);

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
        Task<WWCP.Acknowledgement>

            IRemotePushStatus.UpdateRoamingNetworkAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate>  AdminStatusUpdates,
                                                              TransmissionTypes                             TransmissionType,

                                                              DateTime?                                     Timestamp,
                                                              CancellationToken?                            CancellationToken,
                                                              EventTracking_Id                              EventTrackingId,
                                                              TimeSpan?                                     RequestTimeout)


                => Task.FromResult(new WWCP.Acknowledgement(ResultType.NoOperation));

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
        Task<WWCP.Acknowledgement>

            IRemotePushStatus.UpdateRoamingNetworkStatus(IEnumerable<RoamingNetworkStatusUpdate>  StatusUpdates,
                                                         TransmissionTypes                        TransmissionType,

                                                         DateTime?                                Timestamp,
                                                         CancellationToken?                       CancellationToken,
                                                         EventTracking_Id                         EventTrackingId,
                                                         TimeSpan?                                RequestTimeout)


                => Task.FromResult(new WWCP.Acknowledgement(ResultType.NoOperation));

        #endregion

        #endregion

        #endregion

        #region AuthorizeStart/-Stop  directly...

        #region AuthorizeStart(AuthToken,                    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(Auth_Token                   AuthToken,
                           ChargingProduct              ChargingProduct     = null,
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken),   "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeStartRequest?.Invoke(StartTime,
                                                Timestamp.Value,
                                                this,
                                                EventTrackingId,
                                                RoamingNetwork.Id,
                                                OperatorId,
                                                AuthToken,
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
                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;
                result   = AuthStartResult.OutOfService(Id, SessionId, Runtime);
            }

            else
            {

                var response = await CPORoaming.
                                         AuthorizeStart(OperatorId.HasValue
                                                            ? OperatorId.Value.ToOICP()
                                                            : DefaultOperatorId,
                                                        AuthToken.          ToOICP(),
                                                        null,
                                                        ChargingProduct?.Id.ToOICP(),
                                                        SessionId.          ToOICP(),
                                                        null,

                                                        Timestamp,
                                                        CancellationToken,
                                                        EventTrackingId,
                                                        RequestTimeout).ConfigureAwait(false);


                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response.Content                     != null              &&
                    response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
                {

                    result = AuthStartResult.Authorized(
                                 Id,
                                 response.Content.SessionId. ToWWCP().Value,
                                 ProviderId:      response.Content.ProviderId.ToWWCP(),
                                 Description:     response.Content.StatusCode.Description,
                                 AdditionalInfo:  response.Content.StatusCode.AdditionalInfo,
                                 Runtime:         Runtime
                             );

                }

                else
                    result = AuthStartResult.NotAuthorized(
                                 Id,
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
                                                 EventTrackingId,
                                                 RoamingNetwork.Id,
                                                 OperatorId,
                                                 AuthToken,
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

        #region AuthorizeStart(AuthToken, EVSEId,            ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="AuthToken">A (RFID) user identification.</param>
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

            AuthorizeStart(Auth_Token                   AuthToken,
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

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken),  "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeEVSEStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeEVSEStartRequest?.Invoke(StartTime,
                                                    Timestamp.Value,
                                                    this,
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    OperatorId,
                                                    AuthToken,
                                                    EVSEId,
                                                    ChargingProduct,
                                                    SessionId,
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
                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;
                result   = AuthStartEVSEResult.OutOfService(Id, SessionId, Runtime);
            }

            else
            {

                var response  = await CPORoaming.
                                          AuthorizeStart(OperatorId.HasValue
                                                            ? OperatorId.Value.ToOICP()
                                                            : DefaultOperatorId,
                                                         AuthToken.          ToOICP(),
                                                         EVSEId.             ToOICP(),
                                                         ChargingProduct?.Id.ToOICP(),
                                                         SessionId.          ToOICP(),
                                                         null,

                                                         Timestamp,
                                                         CancellationToken,
                                                         EventTrackingId,
                                                         RequestTimeout).ConfigureAwait(false);


                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response.Content                     != null              &&
                    response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
                {

                    result = AuthStartEVSEResult.Authorized(
                                 Id,
                                 response.Content.SessionId.ToWWCP().Value,
                                 ProviderId:      response.Content.ProviderId.ToWWCP(),
                                 Description:     response.Content.StatusCode.Description,
                                 AdditionalInfo:  response.Content.StatusCode.AdditionalInfo,
                                 Runtime:         Runtime
                             );

                }

                else
                    result = AuthStartEVSEResult.NotAuthorized(
                                 Id,
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
                                                     EventTrackingId,
                                                     RoamingNetwork.Id,
                                                     OperatorId,
                                                     AuthToken,
                                                     EVSEId,
                                                     ChargingProduct,
                                                     SessionId,
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

        #region AuthorizeStart(AuthToken, ChargingStationId, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging station.
        /// </summary>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingStationId">The unique identification charging station.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<AuthStartChargingStationResult>

            AuthorizeStart(Auth_Token                   AuthToken,
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

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingStationStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeChargingStationStartRequest?.Invoke(StartTime,
                                                               Timestamp.Value,
                                                               this,
                                                               EventTrackingId,
                                                               RoamingNetwork.Id,
                                                               OperatorId,
                                                               AuthToken,
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


            var result   = AuthStartChargingStationResult.Error(
                               Id,
                               SessionId,
                               "Not implemented!"
                           );

            var Endtime  = DateTime.Now;
            var Runtime  = Endtime - StartTime;


            #region Send OnAuthorizeChargingStationStartResponse event

            try
            {

                OnAuthorizeChargingStationStartResponse?.Invoke(Endtime,
                                                                Timestamp.Value,
                                                                this,
                                                                EventTrackingId,
                                                                RoamingNetwork.Id,
                                                                OperatorId,
                                                                AuthToken,
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

            return Task.FromResult(result);

        }

        #endregion

        #region AuthorizeStart(AuthToken, ChargingPoolId,    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging pool.
        /// </summary>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingPoolId">The unique identification charging pool.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<AuthStartChargingPoolResult>

            AuthorizeStart(Auth_Token                   AuthToken,
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

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingPoolStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeChargingPoolStartRequest?.Invoke(StartTime,
                                                            Timestamp.Value,
                                                            this,
                                                            EventTrackingId,
                                                            RoamingNetwork.Id,
                                                            OperatorId,
                                                            AuthToken,
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


            var result   = AuthStartChargingPoolResult.Error(
                               Id,
                               SessionId,
                               "Not implemented!"
                           );

            var Endtime  = DateTime.Now;
            var Runtime  = Endtime - StartTime;


            #region Send OnAuthorizeChargingPoolStartResponse event

            try
            {

                OnAuthorizeChargingPoolStartResponse?.Invoke(Endtime,
                                                             Timestamp.Value,
                                                             this,
                                                             EventTrackingId,
                                                             RoamingNetwork.Id,
                                                             OperatorId,
                                                             AuthToken,
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

            return Task.FromResult(result);

        }

        #endregion


        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        #region AuthorizeStop(SessionId, AuthToken,                    OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          Auth_Token                   AuthToken,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken),  "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeStopRequest?.Invoke(StartTime,
                                               Timestamp.Value,
                                               this,
                                               EventTrackingId,
                                               RoamingNetwork.Id,
                                               OperatorId,
                                               SessionId,
                                               AuthToken,
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
                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;
                result   = AuthStopResult.OutOfService(Id, SessionId, Runtime);
            }

            else
            {

                var response = await CPORoaming.AuthorizeStop(OperatorId.HasValue
                                                                  ? OperatorId.Value.ToOICP()
                                                                  : DefaultOperatorId,
                                                              SessionId. ToOICP(),
                                                              AuthToken. ToOICP(),
                                                              null,
                                                              null,

                                                              Timestamp,
                                                              CancellationToken,
                                                              EventTrackingId,
                                                              RequestTimeout).ConfigureAwait(false);


                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response.Content                     != null              &&
                    response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
                {

                    result = AuthStopResult.Authorized(
                                 Id,
                                 SessionId,
                                 response.Content.ProviderId.ToWWCP(),
                                 response.Content?.StatusCode?.Description,
                                 response.Content?.StatusCode?.AdditionalInfo
                             );

                }
                else
                    result = AuthStopResult.NotAuthorized(
                                 Id,
                                 SessionId,
                                 response.Content?.ProviderId.ToWWCP(),
                                 response.Content?.StatusCode?.Description,
                                 response.Content?.StatusCode?.AdditionalInfo
                             );

            }


            #region Send OnAuthorizeStopResponse event

            try
            {

                OnAuthorizeStopResponse?.Invoke(Endtime,
                                                Timestamp.Value,
                                                this,
                                                EventTrackingId,
                                                RoamingNetwork.Id,
                                                OperatorId,
                                                SessionId,
                                                AuthToken,
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

        #region AuthorizeStop(SessionId, AuthToken, EVSEId,            OperatorId = null, ...)

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
                          Auth_Token                   AuthToken,
                          WWCP.EVSE_Id                 EVSEId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (AuthToken  == null)
                throw new ArgumentNullException(nameof(AuthToken), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeEVSEStopRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeEVSEStopRequest?.Invoke(StartTime,
                                                   Timestamp.Value,
                                                   this,
                                                   EventTrackingId,
                                                   RoamingNetwork.Id,
                                                   OperatorId,
                                                   EVSEId,
                                                   SessionId,
                                                   AuthToken,
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
                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;
                result   = AuthStopEVSEResult.OutOfService(Id, SessionId, Runtime);
            }

            else
            {

                var response  = await CPORoaming.AuthorizeStop(OperatorId.HasValue
                                                                  ? OperatorId.Value.ToOICP()
                                                                  : DefaultOperatorId,
                                                               SessionId. ToOICP(),
                                                               AuthToken. ToOICP(),
                                                               EVSEId.    ToOICP(),
                                                               null,

                                                               Timestamp,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               RequestTimeout).ConfigureAwait(false);


                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                    response.Content                     != null              &&
                    response.Content.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
                {

                    result = AuthStopEVSEResult.Authorized(
                                 Id,
                                 SessionId,
                                 response.Content?.ProviderId.ToWWCP(),
                                 response.Content?.StatusCode?.Description,
                                 response.Content?.StatusCode?.AdditionalInfo
                             );

                }
                else
                    result = AuthStopEVSEResult.NotAuthorized(
                                 Id,
                                 SessionId,
                                 response.Content?.ProviderId.ToWWCP(),
                                 response.Content?.StatusCode?.Description,
                                 response.Content?.StatusCode?.AdditionalInfo
                             );

            }


            #region Send OnAuthorizeEVSEStopResponse event

            try
            {

                OnAuthorizeEVSEStopResponse?.Invoke(Endtime,
                                                    Timestamp.Value,
                                                    this,
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    OperatorId,
                                                    EVSEId,
                                                    SessionId,
                                                    AuthToken,
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

        #region AuthorizeStop(SessionId, AuthToken, ChargingStationId, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging station.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<AuthStopChargingStationResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          Auth_Token                   AuthToken,
                          ChargingStation_Id           ChargingStationId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingStationStopRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeChargingStationStopRequest?.Invoke(StartTime,
                                                              Timestamp.Value,
                                                              this,
                                                              EventTrackingId,
                                                              RoamingNetwork.Id,
                                                              OperatorId,
                                                              ChargingStationId,
                                                              SessionId,
                                                              AuthToken,
                                                              RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingStationStopRequest));
            }

            #endregion


            var result   = AuthStopChargingStationResult.Error(
                               Id,
                               SessionId,
                               "OICP does not support this request!"
                           );

            var Endtime  = DateTime.Now;
            var Runtime  = Endtime - StartTime;


            #region Send OnAuthorizeChargingStationStopResponse event

            try
            {

                OnAuthorizeChargingStationStopResponse?.Invoke(Endtime,
                                                               Timestamp.Value,
                                                               this,
                                                               EventTrackingId,
                                                               RoamingNetwork.Id,
                                                               OperatorId,
                                                               ChargingStationId,
                                                               SessionId,
                                                               AuthToken,
                                                               RequestTimeout,
                                                               result,
                                                               Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingStationStopResponse));
            }

            #endregion

            return Task.FromResult(result);

        }

        #endregion

        #region AuthorizeStop(SessionId, AuthToken, ChargingPoolId,    OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging pool.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<AuthStopChargingPoolResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          Auth_Token                   AuthToken,
                          ChargingPool_Id              ChargingPoolId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingPoolStopRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeChargingPoolStopRequest?.Invoke(StartTime,
                                                           Timestamp.Value,
                                                           this,
                                                           EventTrackingId,
                                                           RoamingNetwork.Id,
                                                           OperatorId,
                                                           ChargingPoolId,
                                                           SessionId,
                                                           AuthToken,
                                                           RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingPoolStopRequest));
            }

            #endregion


            var result   = AuthStopChargingPoolResult.Error(
                               Id,
                               SessionId,
                               "OICP does not support this request!"
                           );

            var Endtime  = DateTime.Now;
            var Runtime  = Endtime - StartTime;


            #region Send OnAuthorizeChargingPoolStopResponse event

            try
            {

                OnAuthorizeChargingPoolStopResponse?.Invoke(Endtime,
                                                            Timestamp.Value,
                                                            this,
                                                            EventTrackingId,
                                                            RoamingNetwork.Id,
                                                            OperatorId,
                                                            ChargingPoolId,
                                                            SessionId,
                                                            AuthToken,
                                                            RequestTimeout,
                                                            result,
                                                            Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingPoolStopResponse));
            }

            #endregion

            return Task.FromResult(result);

        }

        #endregion

        #endregion

        #region SendChargeDetailRecord(ChargeDetailRecord, TransmissionType = Enqueued, ...)

        /// <summary>
        /// Send a charge detail record to an OICP server.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="TransmissionType">Whether to send the CDR directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<SendCDRsResult>

            SendChargeDetailRecord(WWCP.ChargeDetailRecord  ChargeDetailRecord,
                                   TransmissionTypes        TransmissionType    = TransmissionTypes.Enqueued,

                                   DateTime?                Timestamp           = null,
                                   CancellationToken?       CancellationToken   = null,
                                   EventTracking_Id         EventTrackingId     = null,
                                   TimeSpan?                RequestTimeout      = null)

        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord),  "The given charge detail record must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

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

                    OnEnqueueSendCDRRequest?.Invoke(DateTime.Now,
                                                    Timestamp.Value,
                                                    this,
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    ChargeDetailRecord,
                                                    RequestTimeout);

                }
                catch (Exception e)
                {
                    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                }

                #endregion

                lock (ServiceCheckLock)
                {

                    ChargeDetailRecordQueue.Add(ChargeDetailRecord);

                    ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                }

                return SendCDRsResult.Enqueued(Id);

            }

            #endregion

            #region Send OnSendCDRRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnSendCDRRequest?.Invoke(StartTime,
                                         Timestamp.Value,
                                         this,
                                         EventTrackingId,
                                         RoamingNetwork.Id,
                                         ChargeDetailRecord,
                                         RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
            }

            #endregion


            DateTime        Endtime;
            TimeSpan        Runtime;
            SendCDRsResult  result;

            if (DisableSendChargeDetailRecords)
            {
                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;
                result   = SendCDRsResult.OutOfService(Id, Runtime: Runtime);
            }

            else
            {

                var response = await CPORoaming.SendChargeDetailRecord(ChargeDetailRecord.ToOICP(),

                                                                       Timestamp,
                                                                       CancellationToken,
                                                                       EventTrackingId,
                                                                       RequestTimeout).ConfigureAwait(false);


                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                    response.Content        != null              &&
                    response.Content.Result)
                {
                    result = SendCDRsResult.Forwarded(Id);
                }

                else
                    result = SendCDRsResult.NotForwared(Id,
                                                        response?.Content?.StatusCode?.Description);

            }


            #region Send OnSendCDRResponse event

            try
            {

                OnSendCDRResponse?.Invoke(Endtime,
                                          Timestamp.Value,
                                          this,
                                          EventTrackingId,
                                          RoamingNetwork.Id,
                                          ChargeDetailRecord,
                                          RequestTimeout,
                                          result,
                                          Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region Delayed upstream methods...

        #region EnqueueChargingPoolDataUpdate(ChargingPool, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Enqueue the given EVSE data for a delayed upload.
        /// </summary>
        /// <param name="ChargingPool">A charging station.</param>
        public Task<WWCP.Acknowledgement>

            EnqueueChargingPoolDataUpdate(ChargingPool  ChargingPool,
                                          String        PropertyName,
                                          Object        OldValue,
                                          Object        NewValue)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging station must not be null!");

            #endregion

            lock (ServiceCheckLock)
            {

                foreach (var evse in ChargingPool.SelectMany(station => station.EVSEs))
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                    {

                        EVSEsToUpdateQueue.Add(evse);

                        ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                    }

                }

            }

            return Task.FromResult(new WWCP.Acknowledgement(ResultType.True));

        }

        #endregion

        #region EnqueueChargingStationDataUpdate(ChargingStation, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Enqueue the given EVSE data for a delayed upload.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public Task<WWCP.Acknowledgement>

            EnqueueChargingStationDataUpdate(ChargingStation  ChargingStation,
                                             String           PropertyName,
                                             Object           OldValue,
                                             Object           NewValue)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            lock (ServiceCheckLock)
            {

                foreach (var evse in ChargingStation.EVSEs)
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                    {

                        EVSEsToUpdateQueue.Add(evse);

                        ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                    }

                }

            }

            return Task.FromResult(new WWCP.Acknowledgement(ResultType.True));

        }

        #endregion

        #endregion


        // -----------------------------------------------------------------------------------------------------


        #region (timer) ServiceCheck(State)

        private void ServiceCheck(Object State)
        {

            if (!DisablePushData)
            {

                try
                {

                    FlushServiceQueues().Wait();

                }
                catch (Exception e)
                {

                    while (e.InnerException != null)
                        e = e.InnerException;

                    OnWWCPCPOAdapterException?.Invoke(DateTime.Now,
                                                      this,
                                                      e);

                }

            }

        }

        public async Task FlushServiceQueues()
        {

            DebugX.Log("ServiceCheck, as every " + _ServiceCheckEvery + "ms!");

            #region Make a thread local copy of all data

            //ToDo: AsyncLocal is currently not implemented in Mono!
            //var EVSEDataQueueCopy   = new AsyncLocal<HashSet<EVSE>>();
            //var EVSEStatusQueueCopy = new AsyncLocal<List<EVSEStatusChange>>();

            var EVSEsToAddQueueCopy          = new ThreadLocal<HashSet<EVSE>>();
            var EVSEDataQueueCopy            = new ThreadLocal<HashSet<EVSE>>();
            var EVSEStatusQueueCopy          = new ThreadLocal<List<EVSEStatusUpdate>>();
            var EVSEsToRemoveQueueCopy       = new ThreadLocal<HashSet<EVSE>>();
            var ChargeDetailRecordQueueCopy  = new ThreadLocal<List<WWCP.ChargeDetailRecord>>();

            if (Monitor.TryEnter(ServiceCheckLock))
            {

                try
                {

                    if (EVSEsToAddQueue.               Count == 0 &&
                        EVSEsToUpdateQueue.          Count == 0 &&
                        EVSEStatusChangesDelayedQueue. Count == 0 &&
                        EVSEsToRemoveQueue.            Count == 0 &&
                        ChargeDetailRecordQueue.       Count == 0)
                    {
                        return;
                    }

                    _ServiceRunId++;

                    // Copy 'EVSEs to add', remove originals...
                    EVSEsToAddQueueCopy.Value          = new HashSet<EVSE>           (EVSEsToAddQueue);
                    EVSEsToAddQueue.Clear();

                    // Copy 'EVSEs to update', remove originals...
                    EVSEDataQueueCopy.Value            = new HashSet<EVSE>           (EVSEsToUpdateQueue);
                    EVSEsToUpdateQueue.Clear();

                    // Copy 'EVSE status changes', remove originals...
                    EVSEStatusQueueCopy.Value          = new List<EVSEStatusUpdate>  (EVSEStatusChangesDelayedQueue);
                    EVSEStatusChangesDelayedQueue.Clear();

                    // Copy 'EVSEs to remove', remove originals...
                    EVSEsToRemoveQueueCopy.Value       = new HashSet<EVSE>           (EVSEsToRemoveQueue);
                    EVSEsToRemoveQueue.Clear();

                    // Copy 'EVSEs to remove', remove originals...
                    ChargeDetailRecordQueueCopy.Value  = new List<WWCP.ChargeDetailRecord>(ChargeDetailRecordQueue);
                    ChargeDetailRecordQueue.Clear();

                    // Stop the timer. Will be rescheduled by next EVSE data/status change...
                    ServiceCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);

                }
                catch (Exception e)
                {

                    while (e.InnerException != null)
                        e = e.InnerException;

                    DebugX.LogT(nameof(WWCPCPOAdapter) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                }

                finally
                {
                    Monitor.Exit(ServiceCheckLock);
                }

            }

            else
            {

                Console.WriteLine("ServiceCheckLock missed!");
                ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

            }

            #endregion

            // Upload status changes...
            if (EVSEsToAddQueueCopy.        Value != null ||
                EVSEDataQueueCopy.          Value != null ||
                EVSEStatusQueueCopy.        Value != null ||
                EVSEsToRemoveQueueCopy.     Value != null ||
                ChargeDetailRecordQueueCopy.Value != null)
            {

                // Use the events to evaluate if something went wrong!

                var EventTrackingId = EventTracking_Id.New;

                #region Send new EVSE data

                if (EVSEsToAddQueueCopy.Value.Count > 0)
                {

                    var EVSEsToAddTask = _ServiceRunId == 1
                                             ? (this as IRemotePushData).SetStaticData(EVSEsToAddQueueCopy.Value, EventTrackingId: EventTrackingId)
                                             : (this as IRemotePushData).AddStaticData(EVSEsToAddQueueCopy.Value, EventTrackingId: EventTrackingId);

                    EVSEsToAddTask.Wait();

                }

                #endregion

                #region Send changed EVSE data

                if (EVSEDataQueueCopy.Value.Count > 0)
                {

                    // Surpress EVSE data updates for all newly added EVSEs
                    var EVSEsWithoutNewEVSEs = EVSEDataQueueCopy.Value.
                                                   Where(evse => !EVSEsToAddQueueCopy.Value.Contains(evse)).
                                                   ToArray();


                    if (EVSEsWithoutNewEVSEs.Length > 0)
                    {

                        var PushEVSEDataTask = (this as IRemotePushData).UpdateStaticData(EVSEsWithoutNewEVSEs, EventTrackingId: EventTrackingId);

                        PushEVSEDataTask.Wait();

                    }

                }

                #endregion

                #region Send changed EVSE status

                if (EVSEStatusQueueCopy.Value.Count > 0)
                {

                    var PushEVSEStatusTask = PushEVSEStatus(EVSEStatusQueueCopy.Value,
                                                            _ServiceRunId == 1
                                                                ? ActionTypes.fullLoad
                                                                : ActionTypes.update,
                                                            EventTrackingId: EventTrackingId);

                    PushEVSEStatusTask.Wait();

                }

                #endregion

                #region Send charge detail records

                if (ChargeDetailRecordQueueCopy.Value.Count > 0)
                {

                    var SendCDRResults   = new Dictionary<WWCP.ChargeDetailRecord, SendCDRsResult>();

                    foreach (var _ChargeDetailRecord in ChargeDetailRecordQueueCopy.Value)
                    {

                        SendCDRResults.Add(_ChargeDetailRecord,
                                           await SendChargeDetailRecord(_ChargeDetailRecord,
                                                                        TransmissionTypes.Direct,
                                                                        DateTime.Now,
                                                                        new CancellationTokenSource().Token,
                                                                        EventTrackingId,
                                                                        DefaultRequestTimeout).ConfigureAwait(false));

                    }

                    //ToDo: Send results events...
                    //ToDo: Read to queue if it could not be sent...

                }

                #endregion

                //ToDo: Send removed EVSE data!

            }

            return;

        }

        #endregion

        #region (timer) StatusCheck(State)

        private void StatusCheck(Object State)
        {

            if (!DisablePushStatus)
            {

                FlushStatusQueues().Wait();

                //ToDo: Handle errors!

            }

        }

        public async Task FlushStatusQueues()
        {

            DebugX.Log("StatusCheck, as every " + _StatusCheckEvery + "ms!");

            #region Make a thread local copy of all data

            //ToDo: AsyncLocal is currently not implemented in Mono!
            //var EVSEStatusQueueCopy = new AsyncLocal<List<EVSEStatusChange>>();

            var EVSEStatusFastQueueCopy = new ThreadLocal<List<EVSEStatusUpdate>>();

            if (Monitor.TryEnter(ServiceCheckLock,
                                 TimeSpan.FromMinutes(5)))
            {

                try
                {

                    if (EVSEStatusChangesFastQueue.Count == 0)
                        return;

                    _StatusRunId++;

                    // Copy 'EVSE status changes', remove originals...
                    EVSEStatusFastQueueCopy.Value = new List<EVSEStatusUpdate>(EVSEStatusChangesFastQueue.Where(evsestatuschange => !EVSEsToAddQueue.Any(evse => evse.Id == evsestatuschange.Id)));

                    // Add all evse status changes of EVSE *NOT YET UPLOADED* into the delayed queue...
                    EVSEStatusChangesDelayedQueue.AddRange(EVSEStatusChangesFastQueue.Where(evsestatuschange => EVSEsToAddQueue.Any(evse => evse.Id == evsestatuschange.Id)));

                    EVSEStatusChangesFastQueue.Clear();

                    // Stop the timer. Will be rescheduled by next EVSE status change...
                    StatusCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);

                }
                catch (Exception e)
                {

                    while (e.InnerException != null)
                        e = e.InnerException;

                    DebugX.LogT(nameof(WWCPCPOAdapter) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                }

                finally
                {
                    Monitor.Exit(ServiceCheckLock);
                }

            }

            else
            {

                Console.WriteLine("StatusCheckLock missed!");
                StatusCheckTimer.Change(_StatusCheckEvery, Timeout.Infinite);

            }

            #endregion

            // Upload status changes...
            if (EVSEStatusFastQueueCopy.Value != null)
            {

                // Use the events to evaluate if something went wrong!

                #region Send changed EVSE status

                if (EVSEStatusFastQueueCopy.Value.Count > 0)
                {

                    var PushEVSEStatusTask = _IRemotePushStatus.UpdateEVSEStatus(EVSEStatusFastQueueCopy.Value);
                                                          //  _StatusRunId == 1
                                                          //      ? ActionType.fullLoad
                                                          //      : ActionType.update);

                    PushEVSEStatusTask.Wait();

                }

                #endregion

            }

            return;

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

            return this.Equals(WWCPCPOAdapter);

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

            => "Hubject CPO " + Id;

        #endregion


    }

}
