/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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

using System.Text.RegularExpressions;

using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// A WWCP wrapper for the OICP CPO Roaming client which maps
    /// WWCP data structures onto OICP data structures and vice versa.
    /// </summary>
    public partial class CPOAdapter : WWCP.AWWCPEMPAdapter<ChargeDetailRecord>,
                                      WWCP.IEMPRoamingProvider,
                                      IEquatable <CPOAdapter>,
                                      IComparable<CPOAdapter>,
                                      IComparable
    {

        #region Data

        private static readonly  Regex                                             pattern                             = new(@"\s=\s");

        public  static readonly  WWCP.ChargingStationOperatorNameSelectorDelegate  DefaultOperatorNameSelector         = I18N => I18N.FirstText();

        private readonly         HashSet<WWCP.EVSE_Id>                             SuccessfullyUploadedEVSEs           = new ();


        /// <summary>
        /// The default logging context.
        /// </summary>
        public  const       String         DefaultLoggingContext        = "OICPv2.3_CPOAdapter";

        public  const       String         DefaultHTTPAPI_LoggingPath   = "default";

        public  const       String         DefaultHTTPAPI_LogfileName   = "OICPv2.3_CPOAdapter.log";


        /// <summary>
        /// The request timeout.
        /// </summary>
        public readonly     TimeSpan       RequestTimeout               = TimeSpan.FromSeconds(60);

        #endregion

        #region Properties

        IId WWCP.IAuthorizeStartStop.AuthId
            => Id;

        IId WWCP.ISendChargeDetailRecords.SendChargeDetailRecordsId
            => Id;

        /// <summary>
        /// The wrapped CPO roaming object.
        /// </summary>
        public CPORoaming CPORoaming { get; }


        /// <summary>
        /// The CPO client.
        /// </summary>
        public CPOClient? CPOClient
            => CPORoaming?.CPOClient;

        /// <summary>
        /// The CPO client HTTP logger.
        /// </summary>
        public CPOClient.HTTP_Logger? ClientHTTPLogger
            => CPORoaming?.CPOClient?.HTTPLogger;

        /// <summary>
        /// The CPO client logger.
        /// </summary>
        public CPOClient.CPOClientLogger? ClientLogger
            => CPORoaming?.CPOClient?.Logger;


        /// <summary>
        /// The CPO server.
        /// </summary>
        public CPOServerAPI? CPOServer
            => CPORoaming?.CPOServer;

        /// <summary>
        /// The CPO server HTTP logger.
        /// </summary>
        public CPOServerAPI.HTTP_Logger? ServerHTTPLogger
            => CPORoaming?.CPOServer?.HTTPLogger;

        /// <summary>
        /// The CPO server logger.
        /// </summary>
        public CPOServerAPI.ServerAPILogger? ServerAPILogger
            => CPORoaming?.CPOServer?.Logger;



        public WWCPEVSEId_2_EVSEId_Delegate?                       CustomEVSEIdConverter                            { get; }

        /// <summary>
        /// An optional default charging station operator.
        /// </summary>
        public WWCP.IChargingStationOperator                       DefaultOperator                                  { get; }

        public WWCP.OperatorIdFormats                              DefaultOperatorIdFormat                          { get; }

        public WWCP.ChargingStationOperatorNameSelectorDelegate?   OperatorNameSelector                             { get; }

        public EVSE2EVSEDataRecordDelegate?                        EVSE2EVSEDataRecord                              { get; }

        public EVSEStatusUpdate2EVSEStatusRecordDelegate?          EVSEStatusUpdate2EVSEStatusRecord                { get; }

        public WWCPChargeDetailRecord2ChargeDetailRecordDelegate?  WWCPChargeDetailRecord2OICPChargeDetailRecord    { get; }


        /// <summary>
        /// An optional default charging station operator name.
        /// </summary>
        public String                                              DefaultOperatorName                              { get; }

        #endregion

        #region Events

        // Client logging...

        #region OnPushEVSEDataWWCPRequest/-Response

        /// <summary>
        /// An event fired whenever new EVSE data will be send upstream.
        /// </summary>
        public event OnPushEVSEDataWWCPRequestDelegate?   OnPushEVSEDataWWCPRequest;

        /// <summary>
        /// An event fired whenever new EVSE data had been sent upstream.
        /// </summary>
        public event OnPushEVSEDataWWCPResponseDelegate?  OnPushEVSEDataWWCPResponse;

        #endregion

        #region OnPushEVSEStatusWWCPRequest/-Response

        /// <summary>
        /// An event fired whenever new EVSE status will be send upstream.
        /// </summary>
        public event OnPushEVSEStatusWWCPRequestDelegate?   OnPushEVSEStatusWWCPRequest;

        /// <summary>
        /// An event fired whenever new EVSE status had been sent upstream.
        /// </summary>
        public event OnPushEVSEStatusWWCPResponseDelegate?  OnPushEVSEStatusWWCPResponse;

        #endregion


        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging.
        /// </summary>
        public event WWCP.OnAuthorizeStartRequestDelegate?   OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging.
        /// </summary>
        public event WWCP.OnAuthorizeStartResponseDelegate?  OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process.
        /// </summary>
        public event WWCP.OnAuthorizeStopRequestDelegate?   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process.
        /// </summary>
        public event WWCP.OnAuthorizeStopResponseDelegate?  OnAuthorizeStopResponse;

        #endregion

        #region OnSendCDRsRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record was enqueued for later sending upstream.
        /// </summary>
        public event WWCP.OnSendCDRsRequestDelegate?   OnEnqueueSendCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record will be send upstream.
        /// </summary>
        public event WWCP.OnSendCDRsRequestDelegate?   OnSendCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record had been sent upstream.
        /// </summary>
        public event WWCP.OnSendCDRsResponseDelegate?  OnSendCDRsResponse;

        #endregion

        #endregion

        #region Constructor(s)

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
        /// 
        /// <param name="DefaultOperator">An optional Charging Station Operator, which will be copied into the main OperatorID-section of the OICP HTTP request.</param>
        /// <param name="OperatorNameSelector">An optional delegate to select an Charging Station Operator name, which will be copied into the OperatorName-section of the OICP HTTP request.</param>
        /// 
        /// <param name="IncludeEVSEIds">Only include the EVSE matching the given delegate.</param>
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="ChargeDetailRecordFilter"></param>
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
        public CPOAdapter(WWCP.EMPRoamingProvider_Id                          Id,
                          I18NString                                          Name,
                          I18NString                                          Description,
                          WWCP.RoamingNetwork                                 RoamingNetwork,
                          CPORoaming                                          CPORoaming,

                          WWCPEVSEId_2_EVSEId_Delegate?                       CustomEVSEIdConverter                           = null,
                          EVSE2EVSEDataRecordDelegate?                        EVSE2EVSEDataRecord                             = null,
                          EVSEStatusUpdate2EVSEStatusRecordDelegate?          EVSEStatusUpdate2EVSEStatusRecord               = null,
                          WWCPChargeDetailRecord2ChargeDetailRecordDelegate?  WWCPChargeDetailRecord2OICPChargeDetailRecord   = null,

                          WWCP.IChargingStationOperator?                      DefaultOperator                                 = null,
                          WWCP.OperatorIdFormats                              DefaultOperatorIdFormat                         = WWCP.OperatorIdFormats.ISO_STAR,
                          WWCP.ChargingStationOperatorNameSelectorDelegate?   OperatorNameSelector                            = null,

                          WWCP.IncludeEVSEIdDelegate?                         IncludeEVSEIds                                  = null,
                          WWCP.IncludeEVSEDelegate?                           IncludeEVSEs                                    = null,
                          WWCP.ChargeDetailRecordFilterDelegate?              ChargeDetailRecordFilter                        = null,

                          TimeSpan?                                           ServiceCheckEvery                               = null,
                          TimeSpan?                                           StatusCheckEvery                                = null,
                          TimeSpan?                                           CDRCheckEvery                                   = null,

                          Boolean                                             DisablePushData                                 = false,
                          Boolean                                             DisablePushAdminStatus                          = true,
                          Boolean                                             DisablePushStatus                               = false,
                          Boolean                                             DisablePushEnergyStatus                         = false,
                          Boolean                                             DisableAuthentication                           = false,
                          Boolean                                             DisableSendChargeDetailRecords                  = false,

                          String                                              EllipticCurve                                   = "P-256",
                          ECPrivateKeyParameters?                             PrivateKey                                      = null,
                          WWCP.PublicKeyCertificates?                         PublicKeyCertificates                           = null,

                          Boolean?                                            IsDevelopment                                   = null,
                          IEnumerable<String>?                                DevelopmentServers                              = null,
                          Boolean?                                            DisableLogging                                  = false,
                          String?                                             LoggingPath                                     = DefaultHTTPAPI_LoggingPath,
                          String?                                             LoggingContext                                  = DefaultLoggingContext,
                          String?                                             LogfileName                                     = DefaultHTTPAPI_LogfileName,
                          LogfileCreatorDelegate?                             LogfileCreator                                  = null,

                          String?                                             ClientsLoggingPath                              = DefaultHTTPAPI_LoggingPath,
                          String?                                             ClientsLoggingContext                           = DefaultLoggingContext,
                          LogfileCreatorDelegate?                             ClientsLogfileCreator                           = null,
                          DNSClient?                                          DNSClient                                       = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description,

                   IncludeEVSEIds,
                   IncludeEVSEs,
                   null,
                   null,
                   null,
                   null,
                   null,
                   null,
                   ChargeDetailRecordFilter,

                   ServiceCheckEvery,
                   StatusCheckEvery,
                   null,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushAdminStatus,
                   DisablePushStatus,
                   true,
                   DisablePushEnergyStatus,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords,

                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,

                   IsDevelopment,
                   DevelopmentServers,
                   DisableLogging,
                   LoggingPath,
                   LoggingContext,
                   LogfileName,
                   LogfileCreator,

                   ClientsLoggingPath,
                   ClientsLoggingContext,
                   ClientsLogfileCreator,
                   DNSClient)

        {

            this.CPORoaming                                     = CPORoaming      ?? throw new ArgumentNullException(nameof(CPORoaming),      "The given CPO roaming object must not be null!");
            this.CustomEVSEIdConverter                          = CustomEVSEIdConverter;
            this.EVSE2EVSEDataRecord                            = EVSE2EVSEDataRecord;
            this.EVSEStatusUpdate2EVSEStatusRecord              = EVSEStatusUpdate2EVSEStatusRecord;
            this.WWCPChargeDetailRecord2OICPChargeDetailRecord  = WWCPChargeDetailRecord2OICPChargeDetailRecord;

            this.DefaultOperator                                = DefaultOperator ?? throw new ArgumentNullException(nameof(DefaultOperator), "The given charging station operator must not be null!");
            this.DefaultOperatorIdFormat                        = DefaultOperatorIdFormat;
            this.OperatorNameSelector                           = OperatorNameSelector;
            DefaultOperatorName = (this.OperatorNameSelector is not null
                                                                       ? this.OperatorNameSelector  (DefaultOperator.Name)
                                                                       : DefaultOperatorNameSelector(DefaultOperator.Name)).Trim();

            if (DefaultOperatorName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(DefaultOperator), "The given default charging station operator name must not be null!");


            // Link incoming OICP events...

            #region OnAuthorizeRemoteReservationStart

            //this.CPORoaming.OnAuthorizeRemoteReservationStart += async (Timestamp,
            //                                                            Sender,
            //                                                            Request) => {


            //    #region Request transformation

            //    TimeSpan?           Duration           = null;
            //    DateTime?           ReservationStartTime          = null;
            //    PartnerProduct_Id?  PartnerProductId   = Request.PartnerProductId;

            //    // Analyse the ChargingProductId field and apply the found key/value-pairs
            //    if (PartnerProductId != null && PartnerProductId.ToString().IsNotNullOrEmpty())
            //    {

            //        var Elements = pattern.Replace(PartnerProductId.ToString(), "=").Split('|').ToArray();

            //        if (Elements.Length > 0)
            //        {

            //            var DurationText = Elements.FirstOrDefault(element => element.StartsWith("D=", StringComparison.InvariantCulture));
            //            if (DurationText.IsNotNullOrEmpty())
            //            {

            //                DurationText = DurationText.Substring(2);

            //                if (DurationText.EndsWith("sec", StringComparison.InvariantCulture))
            //                    Duration = TimeSpan.FromSeconds(UInt32.Parse(DurationText.Substring(0, DurationText.Length - 3)));

            //                if (DurationText.EndsWith("min", StringComparison.InvariantCulture))
            //                    Duration = TimeSpan.FromMinutes(UInt32.Parse(DurationText.Substring(0, DurationText.Length - 3)));

            //            }

            //            var PartnerProductText = Elements.FirstOrDefault(element => element.StartsWith("P=", StringComparison.InvariantCulture));
            //            if (PartnerProductText.IsNotNullOrEmpty())
            //            {
            //                PartnerProductId = PartnerProduct_Id.Parse(PartnerProductText.Substring(2));
            //            }

            //            var StartTimeText = Elements.FirstOrDefault(element => element.StartsWith("S=", StringComparison.InvariantCulture));
            //            if (StartTimeText.IsNotNullOrEmpty())
            //            {
            //                ReservationStartTime = DateTime.Parse(StartTimeText.Substring(2));
            //            }

            //        }

            //    }

            //    #endregion

            //    var response = await RoamingNetwork.
            //                             Reserve(ChargingLocation.FromEVSEId(Request.EVSEId.ToWWCP().Value),
            //                                     ReservationStartTime:  ReservationStartTime,
            //                                     Duration:              Duration,

            //                                     // Always create a reservation identification usable for OICP!
            //                                     ReservationId:         ChargingReservation_Id.Parse(
            //                                                                Request.EVSEId.OperatorId.ToWWCP().Value,
            //                                                                Request.SessionId.HasValue
            //                                                                    ? Request.SessionId.ToString()
            //                                                                    : Session_Id.NewRandom.ToString()
            //                                                            ),

            //                                     ProviderId:            Request.ProviderId.    ToWWCP(),
            //                                     RemoteAuthentication:  Request.Identification.ToWWCP(),
            //                                     ChargingProduct:       PartnerProductId.HasValue
            //                                                                ? new ChargingProduct(PartnerProductId.Value.ToWWCP())
            //                                                                : null,

            //                                     eMAIds:                Request?.Identification?.RemoteIdentification           != null &&
            //                                                            Request?.Identification?.RemoteIdentification?.ToWWCP() != null
            //                                                                ? new eMobilityAccount_Id[] {
            //                                                                      Request.Identification.RemoteIdentification.ToWWCP().Value
            //                                                                  }
            //                                                                : null,

            //                                     Timestamp:             Request.Timestamp,
            //                                     CancellationToken:     Request.CancellationToken,
            //                                     EventTrackingId:       Request.EventTrackingId,
            //                                     RequestTimeout:        Request.RequestTimeout).
            //                             ConfigureAwait(false);

            //    #region Response mapping

            //    if (response != null)
            //    {
            //        switch (response.Result)
            //        {

            //            case ReservationResultType.Success:
            //                return Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.Success(
            //                           Request,
            //                           response.Reservation != null
            //                               ? Session_Id.Parse(response.Reservation.Id.Suffix)
            //                               : new Session_Id?(),

            //                           StatusCodeDescription :    "Reservation successful!",
            //                           StatusCodeAdditionalInfo:  response.Reservation != null ? "ReservationId: " + response.Reservation.Id : null

            //                       );

            //            case ReservationResultType.InvalidCredentials:
            //                return Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.SessionIsInvalid(
            //                           Request,
            //                           SessionId: Session_Id.Parse(response.Reservation.Id.ToString())
            //                       );

            //            case ReservationResultType.Timeout:
            //            case ReservationResultType.CommunicationError:
            //                return Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.CommunicationToEVSEFailed(Request);

            //            case ReservationResultType.AlreadyReserved:
            //                return Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.EVSEAlreadyReserved(Request);

            //            case ReservationResultType.AlreadyInUse:
            //                return Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.EVSEAlreadyInUse_WrongToken(Request);

            //            case ReservationResultType.UnknownLocation:
            //                return Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.UnknownEVSEID(Request);

            //            case ReservationResultType.OutOfService:
            //                return Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.EVSEOutOfService(Request);

            //        }
            //    }

            //    return Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.ServiceNotAvailable(
            //               Request,
            //               SessionId: Session_Id.Parse(response.Reservation.Id.ToString())
            //           );

            //    #endregion

            //};

            #endregion

            #region OnAuthorizeRemoteReservationStop

            //this.CPORoaming.OnAuthorizeRemoteReservationStop += async (Timestamp,
            //                                                           Sender,
            //                                                           Request) => {

            //    var response = await RoamingNetwork.
            //                             CancelReservation(ChargingReservation_Id.Parse(
            //                                                   Request.EVSEId.OperatorId.ToWWCP().Value,
            //                                                   Request.SessionId.ToString()
            //                                               ),
            //                                               ChargingReservationCancellationReason.Deleted,
            //                                               //Request.ProviderId.ToWWCP(),
            //                                               //Request.EVSEId.    ToWWCP(),

            //                                               Request.Timestamp,
            //                                               Request.CancellationToken,
            //                                               Request.EventTrackingId,
            //                                               Request.RequestTimeout).
            //                             ConfigureAwait(false);

            //    #region Response mapping

            //    if (response != null)
            //    {
            //        switch (response.Result)
            //        {

            //            case CancelReservationResultTypes.Success:
            //                return Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.Success(
            //                           Request,
            //                           StatusCodeDescription: "Reservation deleted!"
            //                       );

            //            case CancelReservationResultTypes.UnknownReservationId:
            //                return Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.SessionIsInvalid(
            //                           Request,
            //                           SessionId: Request.SessionId
            //                       );

            //            case CancelReservationResultTypes.Offline:
            //            case CancelReservationResultTypes.Timeout:
            //            case CancelReservationResultTypes.CommunicationError:
            //                return Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.CommunicationToEVSEFailed(Request);

            //            case CancelReservationResultTypes.UnknownEVSE:
            //                return Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.UnknownEVSEID(Request);

            //            case CancelReservationResultTypes.OutOfService:
            //                return Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.EVSEOutOfService(Request);

            //        }
            //    }

            //    return Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.ServiceNotAvailable(
            //               Request,
            //               SessionId: Request.SessionId
            //           );

            //    #endregion

            //};

            #endregion


            #region OnAuthorizeRemoteStart

            this.CPORoaming.OnAuthorizeRemoteStart += async (Timestamp,
                                                             Sender,
                                                             Request) => {

                try
                {

                    #region Request mapping

                    WWCP.ChargingReservation_Id?  ReservationId      = null;
                    TimeSpan?                     MinDuration        = null;
                    Single?                       PlannedEnergy      = null;
                    WWCP.ChargingProduct_Id?      ProductId          = WWCP.ChargingProduct_Id.Parse("AC1");
                    WWCP.ChargingProduct          ChargingProduct    = null;
                    PartnerProduct_Id?            PartnerProductId   = Request.PartnerProductId;

                    if (PartnerProductId != null && PartnerProductId.ToString().IsNotNullOrEmpty())
                    {

                        // The PartnerProductId is a simple string...
                        if (!PartnerProductId.Value.ToString().Contains('='))
                        {
                            ChargingProduct = new WWCP.ChargingProduct(
                                                  WWCP.ChargingProduct_Id.Parse(PartnerProductId.Value.ToString())
                                              );
                        }

                        else
                        {

                            var ProductIdElements = PartnerProductId.ToString().DoubleSplit('|', '=');

                            if (ProductIdElements.Any())
                            {

                                if (ProductIdElements.ContainsKey("R") &&
                                    WWCP.ChargingReservation_Id.TryParse(Request.EVSEId.OperatorId.ToWWCP().Value,
                                                                         ProductIdElements["R"],
                                                                         out WWCP.ChargingReservation_Id _ReservationId))
                                    ReservationId = _ReservationId;


                                if (ProductIdElements.ContainsKey("D"))
                                {

                                    var MinDurationText = ProductIdElements["D"];

                                    if (MinDurationText.EndsWith("sec", StringComparison.InvariantCulture))
                                        MinDuration = TimeSpan.FromSeconds(uint.Parse(MinDurationText[..^3]));

                                    if (MinDurationText.EndsWith("min", StringComparison.InvariantCulture))
                                        MinDuration = TimeSpan.FromMinutes(uint.Parse(MinDurationText[..^3]));

                                }


                                if (ProductIdElements.ContainsKey("E") &&
                                    float.TryParse(ProductIdElements["E"], out Single _PlannedEnergy))
                                    PlannedEnergy = _PlannedEnergy;


                                if (ProductIdElements.ContainsKey("P") &&
                                    WWCP.ChargingProduct_Id.TryParse(ProductIdElements["P"], out WWCP.ChargingProduct_Id _ProductId))
                                    ProductId = _ProductId;


                                ChargingProduct = new WWCP.ChargingProduct(
                                                          ProductId.Value,
                                                          MinDuration
                                                      );

                            }

                        }

                    }

                    #endregion

                    var response = await RoamingNetwork.
                                             RemoteStart(EMPRoamingProvider:    this,
                                                         ChargingLocation:      WWCP.ChargingLocation.FromEVSEId(Request.EVSEId.ToWWCP()),
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

                    if (response is not null)
                    {
                        switch (response.Result)
                        {

                            case WWCP.RemoteStartResultTypes.Success:
                                return Acknowledgement<AuthorizeRemoteStartRequest>.Success(
                                           Request:                   Request,
                                           SessionId:                 response.Session.Id.ToOICP(),
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : "Ready to charge!",
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case WWCP.RemoteStartResultTypes.AsyncOperation:
                                return Acknowledgement<AuthorizeRemoteStartRequest>.Success(
                                           Request:                   Request,
                                           SessionId:                 response.Session.Id.ToOICP(),
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : "Ready to charge (async)!",
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case WWCP.RemoteStartResultTypes.InvalidSessionId:
                                return Acknowledgement<AuthorizeRemoteStartRequest>.SessionIsInvalid(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case WWCP.RemoteStartResultTypes.InvalidCredentials:
                                return Acknowledgement<AuthorizeRemoteStartRequest>.NoValidContract(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case WWCP.RemoteStartResultTypes.NoEVConnectedToEVSE:
                                return Acknowledgement<AuthorizeRemoteStartRequest>.NoEVConnectedToEVSE(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case WWCP.RemoteStartResultTypes.Offline:
                            case WWCP.RemoteStartResultTypes.Timeout:
                            case WWCP.RemoteStartResultTypes.CommunicationError:
                            case WWCP.RemoteStartResultTypes.Error:
                                return Acknowledgement<AuthorizeRemoteStartRequest>.CommunicationToEVSEFailed(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case WWCP.RemoteStartResultTypes.Reserved:
                                return Acknowledgement<AuthorizeRemoteStartRequest>.EVSEAlreadyReserved(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case WWCP.RemoteStartResultTypes.AlreadyInUse:
                                return Acknowledgement<AuthorizeRemoteStartRequest>.EVSEAlreadyInUse_WrongToken(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case WWCP.RemoteStartResultTypes.UnknownLocation:
                                return Acknowledgement<AuthorizeRemoteStartRequest>.UnknownEVSEID(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case WWCP.RemoteStartResultTypes.OutOfService:
                                return Acknowledgement<AuthorizeRemoteStartRequest>.EVSEOutOfService(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            default:
                                return Acknowledgement<AuthorizeRemoteStartRequest>.ServiceNotAvailable(
                                    Request:                   Request,
                                    StatusCodeAdditionalInfo:  "Unknown WWCP RemoteStart result: " + response.Result.ToString() + Environment.NewLine +
                                                               response.Description.FirstText()                                 + Environment.NewLine +
                                                               response.AdditionalInfo,
                                    SessionId:                 Request.SessionId,
                                    EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                    CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                );

                        }
                    }

                    return Acknowledgement<AuthorizeRemoteStartRequest>.ServiceNotAvailable(
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
                    return Acknowledgement<AuthorizeRemoteStartRequest>.ServiceNotAvailable(
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

                    var sessionId  = Request.SessionId.ToWWCP();

                    var response   = await RoamingNetwork.
                                               RemoteStop(EMPRoamingProvider:    this,
                                                          SessionId:             sessionId.Value,
                                                          ReservationHandling:   WWCP.ReservationHandling.Close,
                                                          ProviderId:            Request.ProviderId.ToWWCP(),
                                                          //RemoteAuthentication:  null,

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

                            case WWCP.RemoteStopResultTypes.Success:
                                return Acknowledgement<AuthorizeRemoteStopRequest>.Success(
                                           Request:                   Request,
                                           SessionId:                 response.SessionId.ToOICP(),
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : "Ready to stop charging!",
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case WWCP.RemoteStopResultTypes.AsyncOperation:
                                return Acknowledgement<AuthorizeRemoteStopRequest>.Success(
                                           Request:                   Request,
                                           SessionId:                 response.SessionId.ToOICP(),
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : "Ready to stop charging (async)!",
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case WWCP.RemoteStopResultTypes.InvalidSessionId:
                                return Acknowledgement<AuthorizeRemoteStopRequest>.SessionIsInvalid(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case WWCP.RemoteStopResultTypes.Offline:
                            case WWCP.RemoteStopResultTypes.Timeout:
                            case WWCP.RemoteStopResultTypes.CommunicationError:
                                return Acknowledgement<AuthorizeRemoteStopRequest>.CommunicationToEVSEFailed(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case WWCP.RemoteStopResultTypes.UnknownLocation:
                                return Acknowledgement<AuthorizeRemoteStopRequest>.UnknownEVSEID(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            case WWCP.RemoteStopResultTypes.OutOfService:
                                return Acknowledgement<AuthorizeRemoteStopRequest>.EVSEOutOfService(
                                           Request:                   Request,
                                           SessionId:                 Request.SessionId,
                                           StatusCodeDescription:     response.Description.IsNeitherNullNorEmpty() ? response.Description.FirstText() : null,
                                           StatusCodeAdditionalInfo:  response.AdditionalInfo,
                                           EMPPartnerSessionId:       Request.EMPPartnerSessionId,
                                           CPOPartnerSessionId:       Request.CPOPartnerSessionId
                                       );

                            default:
                                return Acknowledgement<AuthorizeRemoteStopRequest>.ServiceNotAvailable(
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

                    return Acknowledgement<AuthorizeRemoteStopRequest>.ServiceNotAvailable(
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
                    return Acknowledgement<AuthorizeRemoteStopRequest>.ServiceNotAvailable(
                        Request:                   Request,
                        StatusCodeAdditionalInfo:  e.Message + "\n" + e.StackTrace,
                        SessionId:                 Request.SessionId
                    );
                }

            };

            #endregion

        }

        #endregion


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
        private async Task<WWCP.PushEVSEDataResult>

            PushEVSEData(IEnumerable<WWCP.IEVSE>  EVSEs,
                         ActionTypes              ServerAction,
                         JObject?                 CustomData          = null,

                         DateTime?                Timestamp           = null,
                         CancellationToken        CancellationToken   = default,
                         EventTracking_Id?        EventTrackingId     = null,
                         TimeSpan?                RequestTimeout      = null)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;
            RequestTimeout  ??= CPOClient?.RequestTimeout;

            WWCP.PushEVSEDataResult? result = null;

            #endregion

            #region Get effective list of EVSEs/EVSEDataRecords to upload

            var warnings         = new List<Warning>();
            var evseDataRecords  = new List<EVSEDataRecord>();

            foreach (var evse in EVSEs)
            {

                try
                {

                    if (evse is null)
                        continue;

                    if (IncludeEVSEs(evse) && IncludeEVSEIds(evse.Id))
                        // WWCP EVSE will be added as internal data "WWCP.EVSE"...
                        evseDataRecords.Add(evse.ToOICP(evse.Operator.Name.FirstText(),
                                                        EVSE2EVSEDataRecord));

                    else
                        DebugX.Log(evse.Id + " was filtered!");

                }
                catch (Exception e)
                {
                    DebugX.Log(e.Message + (e.InnerException != null ? " " + e.InnerException.Message : ""));
                    warnings.Add(Warning.Create(I18NString.Create(Languages.en,
                                                                  e.Message + (e.InnerException != null ? " " + e.InnerException.Message : "")),
                                                evse));
                }

            }

            #endregion

            #region Send OnPushEVSEDataWWCPRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnPushEVSEDataWWCPRequest?.Invoke(startTime,
                                                  Timestamp.Value,
                                                  this,
                                                  Id,
                                                  EventTrackingId,
                                                  RoamingNetwork.Id,
                                                  ServerAction,
                                                  evseDataRecords,
                                                  warnings.Where(warning => warning.IsNeitherNullNorEmpty()),
                                                  RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOAdapter) + "." + nameof(OnPushEVSEDataWWCPRequest));
            }

            #endregion

            DateTime endtime;
            TimeSpan runtime;


            if (evseDataRecords.Count > 0)
            {

                var operatorId  = DefaultOperator.Id.ToOICP(DefaultOperatorIdFormat);

                var response    = await CPORoaming.PushEVSEData(
                                        new PushEVSEDataRequest(
                                            new OperatorEVSEData(
                                                evseDataRecords,
                                                operatorId.Value,
                                                DefaultOperatorName
                                            ),
                                            ServerAction,
                                            null, // ProcessId
                                            CustomData,

                                            Timestamp,
                                            CancellationToken,
                                            EventTrackingId,
                                            RequestTimeout)).
                                        ConfigureAwait(false);

                if (response.IsSuccess())
                {

                    // Success...
                    if (response.Response.Result == true)
                    {

                        endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                        runtime  = endtime - startTime;
                        result   = WWCP.PushEVSEDataResult.Success(Id,
                                                                   this,
                                                                   evseDataRecords.Select(evseDataRecord => evseDataRecord.GetInternalData("WWCP.EVSE") as WWCP.EVSE),
                                                                   response.Response.StatusCode.Description,
                                                                   response.Response.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                       ? warnings.AddAndReturnList(I18NString.Create(Languages.en, response.Response.StatusCode.AdditionalInfo))
                                                                       : warnings,
                                                                   runtime);

                    }

                    // Operation failed... maybe the systems are out of sync?!
                    // Try an automatic fullLoad in order to repair...
                    else
                    {

                        if (ServerAction == ActionTypes.Insert ||
                            ServerAction == ActionTypes.Update ||
                            ServerAction == ActionTypes.Delete)
                        {

                            #region Add warnings...

                            warnings.Add(Warning.Create(I18NString.Create(Languages.en, ServerAction.ToString() + " of " + evseDataRecords.Count + " EVSEs failed!")));
                            warnings.Add(Warning.Create(I18NString.Create(Languages.en, response.Response.StatusCode.Code.ToString())));
                            warnings.Add(Warning.Create(I18NString.Create(Languages.en, response.Response.StatusCode.Description)));

                            if (response.Response.StatusCode.AdditionalInfo.IsNotNullOrEmpty())
                                warnings.Add(Warning.Create(I18NString.Create(Languages.en, response.Response.StatusCode.AdditionalInfo)));

                            warnings.Add(Warning.Create(I18NString.Create(Languages.en, "Will try to fix this issue via a 'fullLoad' of all EVSEs!")));

                            #endregion

                            #region Get all EVSEs from the roaming network

                            var FullLoadEVSEs = RoamingNetwork.EVSEs.Where(evse => evse is not null &&
                                                                           IncludeEVSEs(evse) &&
                                                                           IncludeEVSEIds(evse.Id)).
                                                        Select(evse =>
                                                        {

                                                            try
                                                            {

                                                                return evse.ToOICP(evse.Operator.Name.FirstText(),
                                                                                   EVSE2EVSEDataRecord);

                                                            }
                                                            catch (Exception e)
                                                            {
                                                                DebugX.Log(e.Message);
                                                                warnings.Add(Warning.Create(I18NString.Create(Languages.en, e.Message), evse));
                                                            }

                                                            return null;

                                                        }).
                                                        Where(evsedatarecord => evsedatarecord is not null).
                                                        ToArray();

                            #endregion

                            #region Send request

                            var FullLoadResponse = await CPORoaming.PushEVSEData(
                                                         new PushEVSEDataRequest(
                                                             new OperatorEVSEData(
                                                                 FullLoadEVSEs,
                                                                 operatorId.Value,
                                                                 DefaultOperatorName.IsNotNullOrEmpty() ? DefaultOperatorName : null
                                                             ),
                                                             ActionTypes.FullLoad,
                                                             null, // ProcessId
                                                             CustomData,

                                                             Timestamp,
                                                             CancellationToken,
                                                             EventTrackingId,
                                                             RequestTimeout)).
                                                         ConfigureAwait(false);

                            #endregion

                            #region Result mapping

                            endtime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                            runtime = endtime - startTime;

                            if (FullLoadResponse.IsSuccess())
                            {

                                if (FullLoadResponse.Response.Result == true)
                                    result = WWCP.PushEVSEDataResult.Success(Id,
                                                                             this,
                                                                             evseDataRecords.Select(evseDataRecord => evseDataRecord.GetInternalData("WWCP.EVSE") as WWCP.EVSE),
                                                                             FullLoadResponse.Response.StatusCode.Description,
                                                                             FullLoadResponse.Response.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                                 ? warnings.AddAndReturnList(I18NString.Create(Languages.en, FullLoadResponse.Response.StatusCode.AdditionalInfo))
                                                                                 : warnings,
                                                                             runtime);

                                else
                                    result = WWCP.PushEVSEDataResult.Error(Id,
                                                                           this,
                                                                           evseDataRecords.Select(evseDataRecord => new WWCP.PushSingleEVSEDataResult(evseDataRecord.GetInternalData("WWCP.EVSE") as WWCP.EVSE, WWCP.PushSingleDataResultTypes.Error)),
                                                                           FullLoadResponse.Response.StatusCode.Description,
                                                                           FullLoadResponse.Response.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                               ? warnings.AddAndReturnList(I18NString.Create(Languages.en, FullLoadResponse.Response.StatusCode.AdditionalInfo))
                                                                               : warnings,
                                                                           runtime);

                            }

                            else
                                result = WWCP.PushEVSEDataResult.Error(Id,
                                                                       this,
                                                                       evseDataRecords.Select(evseDataRecord => new WWCP.PushSingleEVSEDataResult(evseDataRecord.GetInternalData("WWCP.EVSE") as WWCP.EVSE, WWCP.PushSingleDataResultTypes.Error)),
                                                                       //FullLoadResponse.HTTPStatusCode.ToString(),
                                                                       //FullLoadResponse.HTTPBody != null
                                                                       //    ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, FullLoadResponse.HTTPBody.ToUTF8String()))
                                                                       //    : Warnings.AddAndReturnList(I18NString.Create(Languages.en, "No HTTP body received!")),
                                                                       Runtime: runtime);

                            #endregion

                        }

                        // Or a 'fullLoad' Operation failed...
                        else
                        {

                            endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                            runtime  = endtime - startTime;
                            result   = WWCP.PushEVSEDataResult.Error(Id,
                                                                     this,
                                                                     evseDataRecords.Select(evseDataRecord => new WWCP.PushSingleEVSEDataResult(evseDataRecord.GetInternalData("WWCP.EVSE") as WWCP.EVSE, WWCP.PushSingleDataResultTypes.Error)),
                                                                     //response.HTTPStatusCode.ToString(),
                                                                     //response.HTTPBody != null
                                                                     //    ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.HTTPBody.ToUTF8String()))
                                                                     //    : Warnings.AddAndReturnList(I18NString.Create(Languages.en, "No HTTP body received!")),
                                                                     Runtime: runtime);

                        }

                    }

                }
                else
                {

                    endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                    runtime  = endtime - startTime;
                    result   = WWCP.PushEVSEDataResult.Error(Id,
                                                             this,
                                                             evseDataRecords.Select(evseDataRecord => new WWCP.PushSingleEVSEDataResult(evseDataRecord.GetInternalData("WWCP.EVSE") as WWCP.EVSE, WWCP.PushSingleDataResultTypes.Error)),
                                                             //response.HTTPStatusCode.ToString(),
                                                             //response.HTTPBody != null
                                                             //    ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.HTTPBody.ToUTF8String()))
                                                             //    : Warnings.AddAndReturnList(I18NString.Create(Languages.en, "No HTTP body received!")),
                                                             Runtime: runtime);

                }

            }

            #region ...or no EVSEs to push...

            else
            {

                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;
                result   = WWCP.PushEVSEDataResult.NoOperation(Id,
                                                               this,
                                                               EVSEs,
                                                               "No EVSEDataRecords to push!",
                                                               warnings,
                                                               runtime);

            }

            #endregion


            #region Send OnPushEVSEDataResponse event

            try
            {

                OnPushEVSEDataWWCPResponse?.Invoke(endtime,
                                                   Timestamp.Value,
                                                   this,
                                                   Id,
                                                   EventTrackingId,
                                                   RoamingNetwork.Id,
                                                   ServerAction,
                                                   evseDataRecords,
                                                   RequestTimeout,
                                                   result,
                                                   runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOAdapter) + "." + nameof(OnPushEVSEDataWWCPResponse));
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
        public async Task<WWCP.PushEVSEStatusResult>

            PushEVSEStatus(IEnumerable<WWCP.EVSEStatusUpdate>  EVSEStatusUpdates,
                           ActionTypes                         ServerAction,
                           JObject?                            CustomData          = null,

                           DateTime?                           Timestamp           = null,
                           CancellationToken                   CancellationToken   = default,
                           EventTracking_Id?                   EventTrackingId     = null,
                           TimeSpan?                           RequestTimeout      = null)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;
            RequestTimeout  ??= CPOClient?.RequestTimeout;

            WWCP.PushEVSEStatusResult? result = null;

            #endregion

            #region Get effective number of EVSEStatus/EVSEStatusRecords to upload

            var warnings        = new List<Warning>();
            var evseStatusList  = new List<EVSEStatusRecord>();

            foreach (var evseStatusUpdate in EVSEStatusUpdates.
                                                 Where       (evseStatusUpdate => IncludeEVSEIds(evseStatusUpdate.Id)).
                                                 ToLookup    (evseStatusUpdate => evseStatusUpdate.Id,
                                                              evseStatusUpdate => evseStatusUpdate).
                                                 ToDictionary(group            => group.Key,
                                                              group            => group.AsEnumerable().OrderByDescending(item => item.NewStatus.Timestamp)))

            {

                try
                {

                    // Only push the current status of the latest status update!
                    var evseId  = evseStatusUpdate.Key.                          ToOICP();
                    var status  = evseStatusUpdate.Value.First().NewStatus.Value.ToOICP();

                    if (evseId.HasValue && status.HasValue)
                        evseStatusList.Add(new EVSEStatusRecord(
                                               evseId.Value,
                                               status.Value
                                           ));

                }
                catch (Exception e)
                {

                    DebugX.LogException(e);

                    warnings.Add(Warning.Create(I18NString.Create(Languages.en, e.Message),
                                                evseStatusUpdate));

                }

            }

            #endregion

            #region Send OnEVSEStatusPush event

            DateTime endtime;
            TimeSpan runtime;

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnPushEVSEStatusWWCPRequest?.Invoke(StartTime,
                                                    Timestamp.Value,
                                                    this,
                                                    Id,
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    ServerAction,
                                                    evseStatusList,
                                                    warnings.Where(warning => warning.IsNeitherNullNorEmpty()),
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOAdapter) + "." + nameof(OnPushEVSEStatusWWCPRequest));
            }

            #endregion


            if (evseStatusList.Any())
            {

                var operatorId  = DefaultOperator.Id.ToOICP(DefaultOperatorIdFormat);

                var response    = await CPORoaming.PushEVSEStatus(
                                        new PushEVSEStatusRequest(
                                            new OperatorEVSEStatus(
                                                evseStatusList,
                                                operatorId.Value,
                                                DefaultOperatorName
                                            ),
                                            ServerAction,
                                            null, // ProcessId
                                            CustomData,

                                            Timestamp,
                                            CancellationToken,
                                            EventTrackingId,
                                            RequestTimeout));


                endtime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime = endtime - StartTime;

                if (response.IsSuccess())
                {

                    if (response.Response.Result == true)
                        result = WWCP.PushEVSEStatusResult.Success(Id,
                                                                   this,
                                                                   response.Response.StatusCode.Description,
                                                                   response.Response.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                       ? warnings.AddAndReturnList(I18NString.Create(Languages.en, response.Response.StatusCode.AdditionalInfo))
                                                                       : warnings,
                                                                   runtime);

                    else
                        result = WWCP.PushEVSEStatusResult.Error(Id,
                                                                 this,
                                                                 EVSEStatusUpdates,
                                                                 response.Response.StatusCode.Description,
                                                                 response.Response.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                     ? warnings.AddAndReturnList(I18NString.Create(Languages.en, response.Response.StatusCode.AdditionalInfo))
                                                                     : warnings,
                                                                 runtime);

                }
                else
                    result = WWCP.PushEVSEStatusResult.Error(Id,
                                                             this,
                                                             EVSEStatusUpdates,
                                                             //response.HTTPStatusCode.ToString(),
                                                             //response.HTTPBody != null
                                                             //    ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.HTTPBody.ToUTF8String()))
                                                             //    : Warnings.AddAndReturnList(I18NString.Create(Languages.en, "No HTTP body received!")),
                                                             Runtime: runtime);

            }

            #region ...or no EVSEs to push...

            else
            {

                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - StartTime;
                result   = WWCP.PushEVSEStatusResult.NoOperation(Id,
                                                                 this,
                                                                 "No EVSEStatusRecords to push!",
                                                                 EVSEStatusUpdates,
                                                                 warnings,
                                                                 Runtime: TimeSpan.Zero);

            }

            #endregion


            #region Send OnPushEVSEStatusResponse event

            try
            {

                OnPushEVSEStatusWWCPResponse?.Invoke(endtime,
                                                     Timestamp.Value,
                                                     this,
                                                     Id,
                                                     EventTrackingId,
                                                     RoamingNetwork.Id,
                                                     ServerAction,
                                                     evseStatusList,
                                                     RequestTimeout,
                                                     result,
                                                     runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOAdapter) + "." + nameof(OnPushEVSEStatusWWCPResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region (Set/Add/Update/Delete) EVSE(s)...

        // OICP only knows EVSEs!

        #region SetStaticData   (EVSE,  TransmissionType = Enqueue, ...)

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
        public override async Task<WWCP.PushEVSEDataResult>

            SetStaticData(WWCP.IEVSE              EVSE,
                          WWCP.TransmissionTypes  TransmissionType    = WWCP.TransmissionTypes.Enqueue,

                          DateTime?               Timestamp           = null,
                          CancellationToken       CancellationToken   = default,
                          EventTracking_Id?       EventTrackingId     = null,
                          TimeSpan?               RequestTimeout      = null)

        {

            #region Enqueue, if requested...

            if (TransmissionType == WWCP.TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(Timestamp.Now,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var lockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (lockTaken &&
                       (IncludeEVSEs is null || IncludeEVSEs(EVSE)))
                    {

                        evsesToAddQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery,
                                                           TimeSpan.FromMilliseconds(-1));

                    }

                }
                finally
                {
                    if (lockTaken)
                        DataAndStatusLock.Release();
                }

                return lockTaken
                           ? WWCP.PushEVSEDataResult.Enqueued   (Id, this, new WWCP.IEVSE[] { EVSE })
                           : WWCP.PushEVSEDataResult.LockTimeout(Id, this, new WWCP.IEVSE[] { EVSE });

            }

            #endregion

            var result = await PushEVSEData(new[] { EVSE },
                                            ActionTypes.FullLoad,
                                            null,

                                            Timestamp,
                                            CancellationToken,
                                            EventTrackingId,
                                            RequestTimeout);

            return new WWCP.PushEVSEDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleEVSEDataResult>(),
                       Array.Empty<WWCP.PushSingleEVSEDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion

        #region AddStaticData   (EVSE,  TransmissionType = Enqueue, ...)

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
        public override async Task<WWCP.PushEVSEDataResult>

            AddStaticData(WWCP.IEVSE              EVSE,
                          WWCP.TransmissionTypes  TransmissionType    = WWCP.TransmissionTypes.Enqueue,

                          DateTime?               Timestamp           = null,
                          CancellationToken       CancellationToken   = default,
                          EventTracking_Id?       EventTrackingId     = null,
                          TimeSpan?               RequestTimeout      = null)

        {

            #region Enqueue, if requested...

            if (TransmissionType == WWCP.TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(Timestamp.Now,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var lockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (lockTaken &&
                        (IncludeEVSEs is null || IncludeEVSEs(EVSE)))
                    {

                        evsesToAddQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery,
                                                           TimeSpan.FromMilliseconds(-1));

                    }

                }
                finally
                {
                    if (lockTaken)
                        DataAndStatusLock.Release();
                }

                return lockTaken
                           ? WWCP.PushEVSEDataResult.Enqueued   (Id, this, new WWCP.IEVSE[] { EVSE })
                           : WWCP.PushEVSEDataResult.LockTimeout(Id, this, new WWCP.IEVSE[] { EVSE });

            }

            #endregion

            var result = await PushEVSEData(new[] { EVSE },
                                            ActionTypes.Insert,
                                            null,

                                            Timestamp,
                                            CancellationToken,
                                            EventTrackingId,
                                            RequestTimeout);

            return new WWCP.PushEVSEDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleEVSEDataResult>(),
                       Array.Empty<WWCP.PushSingleEVSEDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion

        #region UpdateStaticData(EVSE,  PropertyName, NewValue, OldValue = null, DataSource = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the static data of the given EVSE.
        /// The EVSE can be uploaded as a whole, or just a single property of the EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to update.</param>
        /// <param name="PropertyName">The name of the EVSE property to update.</param>
        /// <param name="NewValue">The new value of the EVSE property to update.</param>
        /// <param name="OldValue">The optional old value of the EVSE property to update.</param>
        /// <param name="DataSource">An optional data source or context for the EVSE property update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<WWCP.PushEVSEDataResult>

            UpdateStaticData(WWCP.IEVSE               EVSE,
                             String                   PropertyName,
                             Object?                  NewValue,
                             Object?                  OldValue            = null,
                             Context?                 DataSource          = null,
                             WWCP.TransmissionTypes   TransmissionType    = WWCP.TransmissionTypes.Enqueue,

                             DateTime?                Timestamp           = null,
                             CancellationToken        CancellationToken   = default,
                             EventTracking_Id?        EventTrackingId     = null,
                             TimeSpan?                RequestTimeout      = null)

        {

            #region Enqueue, if requested...

            if (TransmissionType == WWCP.TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(Timestamp.Now,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var lockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (IncludeEVSEs is     null ||
                       IncludeEVSEs is not null && IncludeEVSEs(EVSE))
                    {

                        if (!evsesUpdateLog.TryGetValue(EVSE, out var propertyUpdateInfo))
                            propertyUpdateInfo = evsesUpdateLog.AddAndReturnValue(EVSE, new List<PropertyUpdateInfo>());

                        propertyUpdateInfo.Add(new PropertyUpdateInfo(
                                                   PropertyName,
                                                   NewValue,
                                                   OldValue,
                                                   DataSource
                                               ));

                        evsesToUpdateQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                    }

                }
                finally
                {
                    if (lockTaken)
                        DataAndStatusLock.Release();
                }

                return WWCP.PushEVSEDataResult.Enqueued(
                           Id,
                           this,
                           Array.Empty<WWCP.IEVSE>()
                       );

            }

            #endregion

            return await PushEVSEData(new[] { EVSE },
                                      ActionTypes.Update,
                                      null,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(EVSE,  TransmissionType = Enqueue, ...)

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
        public override async Task<WWCP.PushEVSEDataResult>

            DeleteStaticData(WWCP.IEVSE              EVSE,
                             WWCP.TransmissionTypes  TransmissionType    = WWCP.TransmissionTypes.Enqueue,

                             DateTime?               Timestamp           = null,
                             CancellationToken       CancellationToken   = default,
                             EventTracking_Id?       EventTrackingId     = null,
                             TimeSpan?               RequestTimeout      = null)

        {

            #region Enqueue, if requested...

            if (TransmissionType == WWCP.TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(Timestamp.Now,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var lockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (lockTaken &&
                        (IncludeEVSEs == null || IncludeEVSEs(EVSE)))
                    {

                        evsesToRemoveQueue.Add(EVSE);

                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                    }

                }
                finally
                {
                    if (lockTaken)
                        DataAndStatusLock.Release();
                }

                return WWCP.PushEVSEDataResult.Enqueued(Id, this, null);

            }

            #endregion

            return await PushEVSEData(new[] { EVSE },
                                      ActionTypes.Delete,
                                      null,

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
        public override async Task<WWCP.PushEVSEDataResult>

            SetStaticData(IEnumerable<WWCP.IEVSE>  EVSEs,
                          WWCP.TransmissionTypes   TransmissionType    = WWCP.TransmissionTypes.Enqueue,

                          DateTime?                Timestamp           = null,
                          CancellationToken        CancellationToken   = default,
                          EventTracking_Id?        EventTrackingId     = null,
                          TimeSpan?                RequestTimeout      = null)

        {

            #region Initial checks

            if (!EVSEs.Any())
                return WWCP.PushEVSEDataResult.NoOperation(Id, this, null);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == WWCP.TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(Timestamp.Now,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var lockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (lockTaken)
                    {

                        var filteredEVSEs = EVSEs.Where(evse => IncludeEVSEs(evse) &&
                                                                IncludeEVSEIds(evse.Id)).
                                                  ToArray();

                        if (filteredEVSEs.Any())
                        {

                            foreach (var EVSE in filteredEVSEs)
                                evsesToAddQueue.Add(EVSE);

                            FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                            return WWCP.PushEVSEDataResult.Enqueued(Id, this, EVSEs);

                        }

                        return WWCP.PushEVSEDataResult.NoOperation(Id, this, EVSEs);

                    }

                }
                finally
                {
                    if (lockTaken)
                        DataAndStatusLock.Release();
                }

            }

            #endregion


            return await PushEVSEData(EVSEs,
                                      ActionTypes.FullLoad,
                                      null,

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
        public override async Task<WWCP.PushEVSEDataResult>

            AddStaticData(IEnumerable<WWCP.IEVSE>  EVSEs,
                          WWCP.TransmissionTypes   TransmissionType    = WWCP.TransmissionTypes.Enqueue,

                          DateTime?                Timestamp           = null,
                          CancellationToken        CancellationToken   = default,
                          EventTracking_Id?        EventTrackingId     = null,
                          TimeSpan?                RequestTimeout      = null)

        {

            #region Initial checks

            if (!EVSEs.Any())
                return WWCP.PushEVSEDataResult.NoOperation(Id, this, null);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == WWCP.TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(Timestamp.Now,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var lockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (lockTaken)
                    {

                        var filteredEVSEs = EVSEs.Where(evse => IncludeEVSEs(evse) &&
                                                                IncludeEVSEIds(evse.Id)).
                                                  ToArray();

                        if (filteredEVSEs.Any())
                        {

                            foreach (var EVSE in filteredEVSEs)
                                evsesToAddQueue.Add(EVSE);

                            FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                            return WWCP.PushEVSEDataResult.Enqueued(Id, this, EVSEs);

                        }

                        return WWCP.PushEVSEDataResult.NoOperation(Id, this, EVSEs);

                    }

                }
                finally
                {
                    if (lockTaken)
                        DataAndStatusLock.Release();
                }

            }

            #endregion

            return await PushEVSEData(EVSEs,
                                      ActionTypes.Insert,
                                      null,

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
        public override async Task<WWCP.PushEVSEDataResult>

            UpdateStaticData(IEnumerable<WWCP.IEVSE>  EVSEs,
                             WWCP.TransmissionTypes   TransmissionType    = WWCP.TransmissionTypes.Enqueue,

                             DateTime?                Timestamp           = null,
                             CancellationToken        CancellationToken   = default,
                             EventTracking_Id?        EventTrackingId     = null,
                             TimeSpan?                RequestTimeout      = null)

        {

            #region Initial checks

            if (!EVSEs.Any())
                return WWCP.PushEVSEDataResult.NoOperation(Id, this, null);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == WWCP.TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(Timestamp.Now,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var lockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (lockTaken)
                    {

                        var filteredEVSEs = EVSEs.Where(evse => IncludeEVSEs(evse) &&
                                                                IncludeEVSEIds(evse.Id)).
                                                  ToArray();

                        if (filteredEVSEs.Any())
                        {

                            foreach (var EVSE in filteredEVSEs)
                                evsesToUpdateQueue.Add(EVSE);

                            FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                            return WWCP.PushEVSEDataResult.Enqueued(Id, this, EVSEs);

                        }

                        return WWCP.PushEVSEDataResult.NoOperation(Id, this, EVSEs);

                    }

                }
                finally
                {
                    if (lockTaken)
                        DataAndStatusLock.Release();
                }

            }

            #endregion

            return await PushEVSEData(EVSEs,
                                      ActionTypes.Update,
                                      null,

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
        public override async Task<WWCP.PushEVSEDataResult>

            DeleteStaticData(IEnumerable<WWCP.IEVSE>  EVSEs,
                             WWCP.TransmissionTypes   TransmissionType    = WWCP.TransmissionTypes.Enqueue,

                             DateTime?                Timestamp           = null,
                             CancellationToken        CancellationToken   = default,
                             EventTracking_Id?        EventTrackingId     = null,
                             TimeSpan?                RequestTimeout      = null)

        {

            #region Initial checks

            if (!EVSEs.Any())
                return WWCP.PushEVSEDataResult.NoOperation(Id, this, null);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == WWCP.TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(Timestamp.Now,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var lockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (lockTaken)
                    {

                        var filteredEVSEs = EVSEs.Where(evse => IncludeEVSEs(evse) &&
                                                                IncludeEVSEIds(evse.Id)).
                                                  ToArray();

                        if (filteredEVSEs.Any())
                        {

                            foreach (var EVSE in filteredEVSEs)
                                evsesToRemoveQueue.Add(EVSE);

                            FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                            return WWCP.PushEVSEDataResult.Enqueued(Id, this, EVSEs);

                        }

                        return WWCP.PushEVSEDataResult.NoOperation(Id, this, EVSEs);

                    }

                }
                finally
                {
                    if (lockTaken)
                        DataAndStatusLock.Release();
                }

            }

            #endregion

            return await PushEVSEData(EVSEs,
                                      ActionTypes.Delete,
                                      null,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion


        #region UpdateStatus    (StatusUpdates, TransmissionType = Enqueue, ...)

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
        public override async Task<WWCP.PushEVSEStatusResult>

            UpdateStatus(IEnumerable<WWCP.EVSEStatusUpdate>  StatusUpdates,
                         WWCP.TransmissionTypes              TransmissionType    = WWCP.TransmissionTypes.Enqueue,

                         DateTime?                           Timestamp           = null,
                         CancellationToken                   CancellationToken   = default,
                         EventTracking_Id?                   EventTrackingId     = null,
                         TimeSpan?                           RequestTimeout      = null)

        {

            #region Initial checks

            if (!StatusUpdates.Any())
                return WWCP.PushEVSEStatusResult.NoOperation(Id, this);

            WWCP.PushEVSEStatusResult? result = null;

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == WWCP.TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(Timestamp.Now,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                var invokeTimer  = false;
                var lockTaken    = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (lockTaken)
                    {

                        var filteredUpdates = StatusUpdates.Where(statusupdate => IncludeEVSEIds(statusupdate.Id)).
                                                            ToArray();

                        if (filteredUpdates.Length > 0)
                        {

                            foreach (var filteredUpdate in filteredUpdates)
                            {

                                // Delay the status update until the EVSE data had been uploaded!
                                if (evsesToAddQueue.Any(evse => evse.Id == filteredUpdate.Id))
                                    evseStatusChangesDelayedQueue.Add(filteredUpdate);

                                else
                                    evseStatusChangesFastQueue.Add(filteredUpdate);

                            }

                            invokeTimer = true;

                            result = WWCP.PushEVSEStatusResult.Enqueued(Id, this);

                        }

                        result = WWCP.PushEVSEStatusResult.NoOperation(Id, this);

                    }

                    else
                        result = WWCP.PushEVSEStatusResult.LockTimeout(Id, this);

                }
                finally
                {
                    if (lockTaken)
                        DataAndStatusLock.Release();
                }

                if (!lockTaken)
                    return WWCP.PushEVSEStatusResult.Error(Id, this, Description: "Could not acquire DataAndStatusLock!");

                if (invokeTimer)
                    FlushEVSEFastStatusTimer.Change(FlushEVSEFastStatusEvery, TimeSpan.FromMilliseconds(-1));

                return result;

            }

            #endregion

            return await PushEVSEStatus(StatusUpdates,
                                        ActionTypes.Update,
                                        null,

                                        Timestamp,
                                        CancellationToken,
                                        EventTrackingId,
                                        RequestTimeout);

        }

        #endregion

        #endregion

        #endregion


        // PushPricingProductData

        // PushEVSEPricing


        #region AuthorizeStart(           LocalAuthentication, ChargingLocation = null, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging location.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<WWCP.AuthStartResult>

            AuthorizeStart(WWCP.LocalAuthentication          LocalAuthentication,
                           WWCP.ChargingLocation?            ChargingLocation      = null,
                           WWCP.ChargingProduct?             ChargingProduct       = null,   // [maxlength: 100]
                           WWCP.ChargingSession_Id?          SessionId             = null,
                           WWCP.ChargingSession_Id?          CPOPartnerSessionId   = null,
                           WWCP.ChargingStationOperator_Id?  OperatorId            = null,

                           DateTime?                         Timestamp             = null,
                           CancellationToken                 CancellationToken     = default,
                           EventTracking_Id?                 EventTrackingId       = null,
                           TimeSpan?                         RequestTimeout        = null)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;
            RequestTimeout  ??= CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnAuthorizeStartRequest?.Invoke(startTime,
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
                                                CPOPartnerSessionId,
                                                Array.Empty<WWCP.ISendAuthorizeStartStop>(),
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOAdapter) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            DateTime               endtime;
            TimeSpan               runtime;
            WWCP.AuthStartResult?  authStartResult   = null;

            var operatorId      = (OperatorId ?? DefaultOperator.Id).ToOICP(DefaultOperatorIdFormat);
            var evseId          = ChargingLocation?.EVSEId?.ToOICP(CustomEVSEIdConverter);
            var identification  = LocalAuthentication.ToOICP();

            if (!operatorId.HasValue)
            {
                endtime          = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime          = endtime - startTime;
                authStartResult  = WWCP.AuthStartResult.AdminDown(Id,
                                                                  this,
                                                                  SessionId,
                                                                  Runtime: runtime);
            }

            // An optional EVSE Id is given, but it is invalid!
            else if (ChargingLocation?.EVSEId.HasValue == true && !evseId.HasValue)
            {
                endtime          = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime          = endtime - startTime;
                authStartResult  = WWCP.AuthStartResult.UnknownLocation(Id,
                                                                        this,
                                                                        SessionId,
                                                                        Runtime: runtime);
            }

            else if (identification?.RFIDId is null && identification?.RFIDIdentification is null)
            {
                endtime          = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime          = endtime - startTime;
                authStartResult  = WWCP.AuthStartResult.InvalidToken(Id,
                                                                     this,
                                                                     SessionId,
                                                                     Runtime: runtime);
            }

            else if (DisableAuthentication)
            {
                endtime          = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime          = endtime - startTime;
                authStartResult  = WWCP.AuthStartResult.AdminDown(Id,
                                                                  this,
                                                                  SessionId,
                                                                  Runtime: runtime);
            }

            else
            {

                var response  = await CPORoaming.AuthorizeStart(
                                      new AuthorizeStartRequest(
                                           operatorId.Value,
                                           identification,
                                           evseId,
                                           ChargingProduct?.ToOICP(),
                                           SessionId.       ToOICP(),
                                           CPOPartnerSessionId.HasValue
                                               ? CPOPartnerSession_Id.Parse(CPOPartnerSessionId.Value.ToString())
                                               : null,
                                           null, // EMPPartnerSessionId
                                           null, // ProcessId

                                           null, // CustomData

                                           Timestamp,
                                           CancellationToken,
                                           EventTrackingId,
                                           RequestTimeout));


                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;

                if (response.IsSuccess()          &&
                    response.Response is not null &&
                    response.Response.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
                {

                    var responseSessionId = response.Response.SessionId.ToWWCP();

                    if (responseSessionId is not null)
                        authStartResult = WWCP.AuthStartResult.Authorized(
                                     Id,
                                     this,
                                     responseSessionId.Value,
                                     response.Response.EMPPartnerSessionId.ToWWCP(),
                                     null,      // ContractId
                                     null,      // PrintedNumber
                                     null,      // ExpiryDate
                                     null,      // MaxkW
                                     null,      // MaxkWh
                                     null,      // MaxDuration
                                     null,      // ChargingTariffs
                                     null,      // ListOfAuthStopTokens
                                     null,      // ListOfAuthStopPINs
                                     response.Response.ProviderId.ToWWCP(),
                                     response.Response.StatusCode?.Description    is not null
                                         ? response.Response.StatusCode.Description.   ToI18NString()
                                         : I18NString.Empty,
                                     response.Response.StatusCode?.AdditionalInfo is not null
                                         ? response.Response.StatusCode.AdditionalInfo.ToI18NString()
                                         : I18NString.Empty,
                                     0,         // NumberOfRetries
                                     runtime
                                 );;

                }

                authStartResult ??= WWCP.AuthStartResult.NotAuthorized(
                                        Id,
                                        this,
                                        SessionId,
                                        response?.Response?.ProviderId.ToWWCP(),
                                        response?.Response?.StatusCode?.Description is not null
                                            ? response.Response.StatusCode.Description.ToI18NString()
                                            : I18NString.Empty,
                                        response?.Response?.StatusCode?.AdditionalInfo is not null
                                            ? response.Response.StatusCode.AdditionalInfo.ToI18NString()
                                            : I18NString.Empty,
                                        0,
                                        runtime
                                    );

            }


            #region Send OnAuthorizeStartResponse event

            try
            {

                OnAuthorizeStartResponse?.Invoke(endtime,
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
                                                 CPOPartnerSessionId,
                                                 Array.Empty<WWCP.ISendAuthorizeStartStop>(),
                                                 RequestTimeout,
                                                 authStartResult,
                                                 runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOAdapter) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return authStartResult;

        }

        #endregion

        #region AuthorizeStop (SessionId, LocalAuthentication, ChargingLocation = null,                                           OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging location.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="LocalAuthentication">A local user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<WWCP.AuthStopResult>

            AuthorizeStop(WWCP.ChargingSession_Id           SessionId,
                          WWCP.LocalAuthentication          LocalAuthentication,
                          WWCP.ChargingLocation?            ChargingLocation      = null,
                          WWCP.ChargingSession_Id?          CPOPartnerSessionId   = null,
                          WWCP.ChargingStationOperator_Id?  OperatorId            = null,

                          DateTime?                         Timestamp             = null,
                          CancellationToken                 CancellationToken     = default,
                          EventTracking_Id?                 EventTrackingId       = null,
                          TimeSpan?                         RequestTimeout        = null)
        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;
            RequestTimeout  ??= CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnAuthorizeStopRequest?.Invoke(startTime,
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
                                               CPOPartnerSessionId,
                                               LocalAuthentication,
                                               RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOAdapter) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            DateTime              endtime;
            TimeSpan              runtime;
            WWCP.AuthStopResult?  authStopResult   = null;

            var operatorId      = (OperatorId ?? DefaultOperator.Id).ToOICP(DefaultOperatorIdFormat);
            var sessionId       = SessionId.ToOICP();
            var evseId          = ChargingLocation?.EVSEId?.ToOICP(CustomEVSEIdConverter);
            var identification  = LocalAuthentication.ToOICP();

            if (!operatorId.HasValue)
            {
                endtime         = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime         = endtime - startTime;
                authStopResult  = WWCP.AuthStopResult.AdminDown(Id,
                                                                this,
                                                                SessionId,
                                                                Runtime: runtime);
            }

            else if (!sessionId.HasValue)
            {
                endtime         = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime         = endtime - startTime;
                authStopResult  = WWCP.AuthStopResult.InvalidSessionId(Id,
                                                                       this,
                                                                       SessionId,
                                                                       Runtime: runtime);
            }

            // An optional EVSE Id is given, but it is invalid!
            else if (ChargingLocation?.EVSEId.HasValue == true && !evseId.HasValue)
            {
                endtime         = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime         = endtime - startTime;
                authStopResult  = WWCP.AuthStopResult.UnknownLocation(Id,
                                                                      this,
                                                                      SessionId,
                                                                      Runtime: runtime);
            }

            else if (identification?.RFIDId is null && identification?.RFIDIdentification is null)
            {
                endtime         = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime         = endtime - startTime;
                authStopResult  = WWCP.AuthStopResult.InvalidToken(Id,
                                                                   this,
                                                                   SessionId,
                                                                   Runtime: runtime);
            }

            else if (DisableAuthentication)
            {
                endtime         = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime         = endtime - startTime;
                authStopResult  = WWCP.AuthStopResult.AdminDown(Id,
                                                                this,
                                                                SessionId,
                                                                Runtime: runtime);
            }

            else
            {

                var response  = await CPORoaming.AuthorizeStop(
                                      new AuthorizeStopRequest(
                                          operatorId.Value,
                                          sessionId. Value,
                                          identification,
                                          evseId,
                                          CPOPartnerSessionId.HasValue
                                               ? CPOPartnerSession_Id.Parse(CPOPartnerSessionId.Value.ToString())
                                               : null,
                                          null, // EMPPartnerSessionId
                                          null, // ProcessId

                                          null, // CustomData

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout));


                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;

                if (response.IsSuccess()          &&
                    response.Response is not null &&
                    response.Response.AuthorizationStatus == AuthorizationStatusTypes.Authorized)
                {

                    authStopResult = WWCP.AuthStopResult.Authorized(
                                         Id,
                                         this,
                                         SessionId,
                                         response.Response?.ProviderId?.ToWWCP(),
                                         response.Response?.StatusCode?.Description     is not null
                                             ? response.Response.StatusCode.Description.   ToI18NString()
                                             : I18NString.Empty,
                                         response?.Response?.StatusCode?.AdditionalInfo is not null
                                             ? response.Response.StatusCode.AdditionalInfo.ToI18NString()
                                             : I18NString.Empty
                                     );

                }
                else
                    authStopResult = WWCP.AuthStopResult.NotAuthorized(
                                         Id,
                                         this,
                                         SessionId,
                                         response?.Response?.ProviderId?.ToWWCP(),
                                         response?.Response?.StatusCode?.Description    is not null
                                             ? response.Response.StatusCode.Description.   ToI18NString()
                                             : I18NString.Empty,
                                         response?.Response?.StatusCode?.AdditionalInfo is not null
                                             ? response.Response.StatusCode.AdditionalInfo.ToI18NString()
                                             : I18NString.Empty
                                     );

            }


            #region Send OnAuthorizeStopResponse event

            try
            {

                OnAuthorizeStopResponse?.Invoke(endtime,
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
                                                CPOPartnerSessionId,
                                                LocalAuthentication,
                                                RequestTimeout,
                                                authStopResult,
                                                runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOAdapter) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return authStopResult;

        }

        #endregion


        // SendChargingStartNotification

        // SendChargingProgressNotification

        // SendChargingEndNotification

        // SendChargingErrorNotification


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
        public async Task<WWCP.SendCDRsResult>

            SendChargeDetailRecords(IEnumerable<WWCP.ChargeDetailRecord>  ChargeDetailRecords,
                                    WWCP.TransmissionTypes                TransmissionType,

                                    DateTime?                             Timestamp,
                                    CancellationToken                     CancellationToken,
                                    EventTracking_Id?                     EventTrackingId,
                                    TimeSpan?                             RequestTimeout)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;
            RequestTimeout  ??= CPOClient?.RequestTimeout;

            DateTime              endtime;
            TimeSpan              runtime;
            WWCP.SendCDRsResult?  sendCDRsResult   = null;

            #endregion

            #region Filter charge detail records

            var forwardedCDRs  = new List<WWCP.ChargeDetailRecord>();
            var filteredCDRs   = new List<WWCP.SendCDRResult>();

            foreach (var cdr in ChargeDetailRecords)
            {

                if (ChargeDetailRecordFilter(cdr) == WWCP.ChargeDetailRecordFilters.forward)
                    forwardedCDRs.Add(cdr);

                else
                    filteredCDRs.Add(WWCP.SendCDRResult.Filtered(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                 cdr,
                                                                 Warning.Create(I18NString.Create(Languages.en, "This charge detail record was filtered!"))));

            }

            #endregion

            #region Send OnSendCDRsRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnSendCDRsRequest?.Invoke(startTime,
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
                DebugX.LogException(e, nameof(CPOAdapter) + "." + nameof(OnSendCDRsRequest));
            }

            #endregion


            #region if disabled => 'AdminDown'...

            if (DisableSendChargeDetailRecords)
            {

                endtime         = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime         = endtime - startTime;
                sendCDRsResult  = WWCP.SendCDRsResult.AdminDown(
                                      org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                      Id,
                                      this,
                                      ChargeDetailRecords,
                                      Runtime: runtime
                                  );

            }

            #endregion

            #region ..., or when there are no charge detail records...

            else if (!ChargeDetailRecords.Any())
            {

                endtime         = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime         = endtime - startTime;
                sendCDRsResult  = WWCP.SendCDRsResult.NoOperation(
                                      org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                      Id,
                                      this,
                                      ChargeDetailRecords,
                                      Runtime: runtime
                                  );

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

                        var SendCDRsResults = new List<WWCP.SendCDRResult>();

                        #region if enqueuing is requested...

                        if (TransmissionType == WWCP.TransmissionTypes.Enqueue)
                        {

                            #region Send OnEnqueueSendCDRRequest event

                            try
                            {

                                OnEnqueueSendCDRsRequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
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
                                DebugX.LogException(e, nameof(CPOAdapter) + "." + nameof(OnSendCDRsRequest));
                            }

                            #endregion

                            foreach (var ChargeDetailRecord in ChargeDetailRecords)
                            {

                                try
                                {

                                    chargeDetailRecordsQueue.Add(ChargeDetailRecord.ToOICP(WWCPChargeDetailRecord2OICPChargeDetailRecord));
                                    SendCDRsResults.Add(WWCP.SendCDRResult.Enqueued(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                                    ChargeDetailRecord));

                                }
                                catch (Exception e)
                                {
                                    SendCDRsResults.Add(WWCP.SendCDRResult.CouldNotConvertCDRFormat(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                                                    ChargeDetailRecord,
                                                                                                    Warning.Create(I18NString.Create(Languages.en, e.Message))));
                                }

                            }

                            endtime         = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                            runtime         = endtime - startTime;
                            sendCDRsResult  = WWCP.SendCDRsResult.Enqueued(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                           Id,
                                                                           this,
                                                                           ChargeDetailRecords,
                                                                           I18NString.Create(Languages.en, "Enqueued for at least " + FlushChargeDetailRecordsEvery.TotalSeconds + " seconds!"),
                                                                           //SendCDRsResults.SafeWhere(cdrresult => cdrresult.Result != SendCDRResultTypes.Enqueued),
                                                                           Runtime: runtime);
                            invokeTimer     = true;

                        }

                        #endregion

                        #region ...or send at once!

                        else
                        {

                            OICPResult<Acknowledgement<ChargeDetailRecordRequest>> response;
                            WWCP.SendCDRResult result;

                            foreach (var chargeDetailRecord in ChargeDetailRecords)
                            {

                                try
                                {

                                    response = await CPORoaming.SendChargeDetailRecord(chargeDetailRecord.ToOICP(WWCPChargeDetailRecord2OICPChargeDetailRecord),
                                                                                       DefaultOperator.Id.ToOICP().Value,
                                                                                       null,

                                                                                       Timestamp,
                                                                                       CancellationToken,
                                                                                       EventTrackingId,
                                                                                       RequestTimeout);

                                    if (response.IsSuccess())
                                    {

                                        if (response.Response.Result == true)
                                        {

                                            result = WWCP.SendCDRResult.Success(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                                chargeDetailRecord);

                                        }

                                        else
                                        {

                                            result = WWCP.SendCDRResult.Error(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                              chargeDetailRecord);

                                        }

                                    }

                                    else
                                        result = WWCP.SendCDRResult.Error(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                          chargeDetailRecord);

                                }
                                catch (Exception e)
                                {
                                    result = WWCP.SendCDRResult.CouldNotConvertCDRFormat(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                                         chargeDetailRecord,
                                                                                         I18NString.Create(Languages.en, e.Message));
                                }

                                SendCDRsResults.Add(result);
                                RoamingNetwork.SessionsStore.CDRForwarded(chargeDetailRecord.SessionId, result);

                            }

                            endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                            runtime  = endtime - startTime;

                            if (SendCDRsResults.All(cdrresult => cdrresult.Result == WWCP.SendCDRResultTypes.Success))
                                sendCDRsResult = WWCP.SendCDRsResult.Success(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                      Id,
                                                                      this,
                                                                      ChargeDetailRecords,
                                                                      Runtime: runtime);

                            else
                                sendCDRsResult = WWCP.SendCDRsResult.Error(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                    Id,
                                                                    this,
                                                                    SendCDRsResults.
                                                                        Where (cdrresult => cdrresult.Result != WWCP.SendCDRResultTypes.Success).
                                                                        Select(cdrresult => cdrresult.ChargeDetailRecord),
                                                                    Runtime: runtime);

                        }

                        #endregion

                    }

                    #region Could not get the lock for toooo long!

                    else
                    {

                        endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                        runtime  = endtime - startTime;
                        sendCDRsResult  = WWCP.SendCDRsResult.Timeout(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                               Id,
                                                               this,
                                                               ChargeDetailRecords,
                                                               I18NString.Create(Languages.en, "Could not " + (TransmissionType == WWCP.TransmissionTypes.Enqueue ? "enqueue" : "send") + " charge detail records!"),
                                                               //ChargeDetailRecords.SafeSelect(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Timeout)),
                                                               Runtime: runtime);

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

                OnSendCDRsResponse?.Invoke(endtime,
                                           Timestamp.Value,
                                           this,
                                           Id.ToString(),
                                           EventTrackingId,
                                           RoamingNetwork.Id,
                                           ChargeDetailRecords,
                                           RequestTimeout,
                                           sendCDRsResult,
                                           runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOAdapter) + "." + nameof(OnSendCDRsResponse));
            }

            #endregion

            return sendCDRsResult;

        }

        #endregion


        // -----------------------------------------------------------------------------------------------------


        #region (timer) FlushEVSEDataAndStatus()

        protected override Boolean SkipFlushEVSEDataAndStatusQueues()

            => evsesToAddQueue.              Count == 0 &&
               evsesToUpdateQueue.           Count == 0 &&
               evseStatusChangesDelayedQueue.Count == 0 &&
               evsesToRemoveQueue.           Count == 0;

        protected override async Task FlushEVSEDataAndStatusQueues()
        {

            #region Get a copy of all current EVSE data and delayed status

            var evsesToAddQueueCopy                = new HashSet<WWCP.IEVSE>();
            var evsesToUpdateQueueCopy             = new HashSet<WWCP.IEVSE>();
            var evseStatusChangesDelayedQueueCopy  = new List   <WWCP.EVSEStatusUpdate>();
            var evsesToRemoveQueueCopy             = new HashSet<WWCP.IEVSE>();
            var evsesUpdateLogCopy                 = new Dictionary<WWCP.IEVSE,            PropertyUpdateInfo[]>();
            var chargingStationsUpdateLogCopy      = new Dictionary<WWCP.IChargingStation, PropertyUpdateInfo[]>();
            var chargingPoolsUpdateLogCopy         = new Dictionary<WWCP.IChargingPool,    PropertyUpdateInfo[]>();

            var lockTaken = await DataAndStatusLock.WaitAsync(0);

            try
            {

                if (lockTaken)
                {

                    // Copy 'EVSEs to add', remove originals...
                    evsesToAddQueueCopy                      = new HashSet<WWCP.IEVSE>                (evsesToAddQueue);
                    evsesToAddQueue.Clear();

                    // Copy 'EVSEs to update', remove originals...
                    evsesToUpdateQueueCopy                   = new HashSet<WWCP.IEVSE>                (evsesToUpdateQueue);
                    evsesToUpdateQueue.Clear();

                    // Copy 'EVSE status changes', remove originals...
                    evseStatusChangesDelayedQueueCopy        = new List<WWCP.EVSEStatusUpdate>        (evseStatusChangesDelayedQueue);
                    evseStatusChangesDelayedQueueCopy.AddRange(evsesToAddQueueCopy.SafeSelect(evse => new WWCP.EVSEStatusUpdate(evse.Id, evse.Status, evse.Status)));
                    evseStatusChangesDelayedQueue.Clear();

                    // Copy 'EVSEs to remove', remove originals...
                    evsesToRemoveQueueCopy                   = new HashSet<WWCP.IEVSE>                (evsesToRemoveQueue);
                    evsesToRemoveQueue.Clear();

                    // Copy EVSE property updates
                    evsesUpdateLog.           ForEach(_ => evsesUpdateLogCopy.           Add(_.Key, _.Value.ToArray()));
                    evsesUpdateLog.Clear();

                    // Copy charging station property updates
                    chargingStationsUpdateLog.ForEach(_ => chargingStationsUpdateLogCopy.Add(_.Key, _.Value.ToArray()));
                    chargingStationsUpdateLog.Clear();

                    // Copy charging pool property updates
                    chargingPoolsUpdateLog.   ForEach(_ => chargingPoolsUpdateLogCopy.   Add(_.Key, _.Value.ToArray()));
                    chargingPoolsUpdateLog.Clear();


                    // Stop the timer. Will be rescheduled by next EVSE data/status change...
                    FlushEVSEDataAndStatusTimer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));

                }

            }
            catch (Exception e)
            {

                while (e.InnerException != null)
                    e = e.InnerException;

                DebugX.LogT(GetType().Name + ".DataAndStatusLock '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                //OnWWCPEMPAdapterException?.Invoke(Timestamp.Now,
                //                                  this,
                //                                  e);

            }

            finally
            {

                if (lockTaken)
                    DataAndStatusLock.Release();

                else
                    DebugX.LogT("DataAndStatusLock exited!");

            }

            #endregion

            // Use events to check if something went wrong!
            var EventTrackingId                           = EventTracking_Id.New;
            var NumberOfSuccessfullyUploadedEVSEs_before  = SuccessfullyUploadedEVSEs.Count;

            #region Send new EVSE data

            if (evsesToAddQueueCopy.Any())
            {

                var EVSEsToAddTask = await PushEVSEData(evsesToAddQueueCopy,
                                                        //_FlushEVSEDataRunId == 1
                                                        //    ? ActionTypes.FullLoad
                                                        //    : ActionTypes.Update,
                                                        NumberOfSuccessfullyUploadedEVSEs_before == 0
                                                            ? ActionTypes.FullLoad
                                                            : ActionTypes.Update,
                                                        EventTrackingId: EventTrackingId);

                foreach (var pushEVSEResult in EVSEsToAddTask.SuccessfulEVSEs)
                    SuccessfullyUploadedEVSEs.Add(pushEVSEResult.EVSE.Id);

                if (EVSEsToAddTask.Warnings.Any())
                {
                    try
                    {

                        SendOnWarnings(Timestamp.Now,
                                       nameof(CPOAdapter) + Id,
                                       nameof(EVSEsToAddTask),
                                       EVSEsToAddTask.Warnings);

                    }
                    catch (Exception)
                    { }
                }

            }

            #endregion

            #region Send changed EVSE data

            if (evsesToUpdateQueueCopy.Any())
            {

                // Surpress EVSE data updates for all newly added EVSEs
                foreach (var _evse in evsesToUpdateQueueCopy.Where(evse => evsesToAddQueueCopy.Contains(evse)).ToArray())
                    evsesToUpdateQueueCopy.Remove(_evse);

                if (evsesToUpdateQueueCopy.Any())
                {

                    var EVSEsToUpdateResult = await PushEVSEData(evsesToUpdateQueueCopy,
                                                                 ActionTypes.Update,
                                                                 EventTrackingId: EventTrackingId);

                    foreach (var pushEVSEResult in EVSEsToUpdateResult.SuccessfulEVSEs)
                        SuccessfullyUploadedEVSEs.Add(pushEVSEResult.EVSE.Id);

                    if (EVSEsToUpdateResult.Warnings.Any())
                    {
                        try
                        {

                            SendOnWarnings(Timestamp.Now,
                                           nameof(CPOAdapter) + Id,
                                           nameof(EVSEsToUpdateResult),
                                           EVSEsToUpdateResult.Warnings);

                        }
                        catch (Exception)
                        { }
                    }

                }

            }

            #endregion

            #region Send changed EVSE status

            if (!DisablePushStatus &&
                evseStatusChangesDelayedQueueCopy.Count > 0)
            {

                var PushEVSEStatusTask = await PushEVSEStatus(evseStatusChangesDelayedQueueCopy.Where(evseStatusUpdate => SuccessfullyUploadedEVSEs.Contains(evseStatusUpdate.Id)),
                                                              //_FlushEVSEDataRunId == 1
                                                              //    ? ActionTypes.FullLoad
                                                              //    : ActionTypes.Update,
                                                              NumberOfSuccessfullyUploadedEVSEs_before == 0
                                                                  ? ActionTypes.FullLoad
                                                                  : ActionTypes.Update,
                                                              EventTrackingId: EventTrackingId);


                if (PushEVSEStatusTask.Warnings.Any())
                {
                    try
                    {

                        SendOnWarnings(Timestamp.Now,
                                       nameof(CPOAdapter) + Id,
                                       nameof(PushEVSEStatusTask),
                                       PushEVSEStatusTask.Warnings);

                    }
                    catch (Exception)
                    { }
                }

            }

            #endregion

            #region Send removed charging stations

            if (evsesToRemoveQueueCopy.Count > 0)
            {

                var EVSEsToRemove = evsesToRemoveQueueCopy.ToArray();

                if (EVSEsToRemove.Length > 0)
                {

                    var EVSEsToRemoveTask = await PushEVSEData(EVSEsToRemove,
                                                               ActionTypes.Delete,
                                                               EventTrackingId: EventTrackingId);

                    foreach (var pushEVSEResult in EVSEsToRemoveTask.SuccessfulEVSEs)
                        SuccessfullyUploadedEVSEs.Remove(pushEVSEResult.EVSE.Id);

                    if (EVSEsToRemoveTask.Warnings.Any())
                    {
                        try
                        {

                            SendOnWarnings(Timestamp.Now,
                                           nameof(CPOAdapter) + Id,
                                           nameof(EVSEsToRemoveTask),
                                           EVSEsToRemoveTask.Warnings);

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
            => evseStatusChangesFastQueue.Count == 0;

        protected override async Task FlushEVSEFastStatusQueues()
        {

            #region Get a copy of all current EVSE data and delayed status

            var EVSEStatusFastQueueCopy = new List<WWCP.EVSEStatusUpdate>();

            var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

            try
            {

                if (LockTaken)
                {

                    // Copy 'EVSE status changes', remove originals...
                    EVSEStatusFastQueueCopy = new List<WWCP.EVSEStatusUpdate>(evseStatusChangesFastQueue.Where(evsestatuschange => !evsesToAddQueue.Any(evse => evse.Id == evsestatuschange.Id)));

                    // Add all evse status changes of EVSE *NOT YET UPLOADED* into the delayed queue...
                    var EVSEStatusChangesDelayed = evseStatusChangesFastQueue.Where(evsestatuschange => evsesToAddQueue.Any(evse => evse.Id == evsestatuschange.Id)).ToArray();

                    if (EVSEStatusChangesDelayed.Length > 0)
                        evseStatusChangesDelayedQueue.AddRange(EVSEStatusChangesDelayed);

                    evseStatusChangesFastQueue.Clear();

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

                var pushEVSEStatusFastTask = await PushEVSEStatus(EVSEStatusFastQueueCopy.Where(evseStatusUpdate => SuccessfullyUploadedEVSEs.Contains(evseStatusUpdate.Id)),
                                                                  ActionTypes.Update,
                                                                  null,

                                                                  Timestamp.Now,
                                                                  new CancellationTokenSource().Token,
                                                                  EventTracking_Id.New,
                                                                  DefaultRequestTimeout).
                                                   ConfigureAwait(false);

                if (pushEVSEStatusFastTask.Warnings.Any())
                {
                    try
                    {

                        SendOnWarnings(Timestamp.Now,
                                       nameof(CPOAdapter) + Id,
                                       nameof(pushEVSEStatusFastTask),
                                       pushEVSEStatusFastTask.Warnings);

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
            => chargeDetailRecordsQueue.Count == 0;

        protected override async Task FlushChargeDetailRecordsQueues(IEnumerable<ChargeDetailRecord> ChargeDetailRecords)
        {

            WWCP.SendCDRResult result;

            foreach (var chargeDetailRecord in ChargeDetailRecords)
            {

                try
                {

                    var response  = await CPORoaming.SendChargeDetailRecord(chargeDetailRecord,
                                                                            DefaultOperator.Id.ToOICP().Value,
                                                                            null,

                                                                            Timestamp.Now,
                                                                            new CancellationTokenSource().Token,
                                                                            EventTracking_Id.New,
                                                                            DefaultRequestTimeout).
                                                     ConfigureAwait(false);

                    if (response.IsSuccess())
                    {

                        if (response.Response.Result == true)
                        {

                            result = WWCP.SendCDRResult.Success(Timestamp.Now,
                                                                chargeDetailRecord.GetInternalDataAs<WWCP.ChargeDetailRecord>(OICPMapper.WWCP_CDR),
                                                                Runtime: response.Response.Runtime);

                        }

                        else
                        {

                            result = WWCP.SendCDRResult.Error(Timestamp.Now,
                                                              chargeDetailRecord.GetInternalDataAs<WWCP.ChargeDetailRecord>(OICPMapper.WWCP_CDR),
                                                              //I18NString.Create(Languages.en, response.HTTPBodyAsUTF8String),
                                                              Runtime: response.Response.Runtime);

                        }

                    }

                    else
                        result = WWCP.SendCDRResult.Error(Timestamp.Now,
                                                          chargeDetailRecord.GetInternalDataAs<WWCP.ChargeDetailRecord>(OICPMapper.WWCP_CDR),
                                                          //I18NString.Create(Languages.en, response.HTTPBodyAsUTF8String),
                                                          Runtime: response.Response.Runtime);

                }
                catch (Exception e)
                {

                    result = WWCP.SendCDRResult.Error(Timestamp.Now,
                                                      chargeDetailRecord.GetInternalDataAs<WWCP.ChargeDetailRecord>(OICPMapper.WWCP_CDR),
                                                      Warning.Create(I18NString.Create(Languages.en, e.Message)),
                                                      Runtime: TimeSpan.Zero);

                }

                RoamingNetwork.SessionsStore.CDRForwarded(chargeDetailRecord.SessionId.ToWWCP().Value, result);

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
        public static Boolean operator == (CPOAdapter WWCPEMPAdapter1,
                                           CPOAdapter WWCPEMPAdapter2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(WWCPEMPAdapter1, WWCPEMPAdapter2))
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
        public static Boolean operator != (CPOAdapter WWCPEMPAdapter1,
                                           CPOAdapter WWCPEMPAdapter2)

            => !(WWCPEMPAdapter1 == WWCPEMPAdapter2);

        #endregion

        #region Operator <  (WWCPEMPAdapter1, WWCPEMPAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (CPOAdapter  WWCPEMPAdapter1,
                                          CPOAdapter  WWCPEMPAdapter2)
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
        public static Boolean operator <= (CPOAdapter WWCPEMPAdapter1,
                                           CPOAdapter WWCPEMPAdapter2)

            => !(WWCPEMPAdapter1 > WWCPEMPAdapter2);

        #endregion

        #region Operator >  (WWCPEMPAdapter1, WWCPEMPAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (CPOAdapter WWCPEMPAdapter1,
                                          CPOAdapter WWCPEMPAdapter2)
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
        public static Boolean operator >= (CPOAdapter WWCPEMPAdapter1,
                                           CPOAdapter WWCPEMPAdapter2)

            => !(WWCPEMPAdapter1 < WWCPEMPAdapter2);

        #endregion

        #endregion

        #region IComparable<CPOAdapter> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object? Object)

            => Object is CPOAdapter cpoAdapter
                   ? CompareTo(cpoAdapter)
                   : throw new ArgumentException("The given object is not a CPO adapter!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(WWCPEMPAdapter)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CPOAdapter">A CPO adapter object to compare with.</param>
        public Int32 CompareTo(CPOAdapter? CPOAdapter)
        {

            if (CPOAdapter is null)
                throw new ArgumentNullException(nameof(CPOAdapter), "The given CPO adapter must not be null!");

            return Id.CompareTo(CPOAdapter.Id);

        }

        #endregion

        #endregion

        #region IEquatable<CPOAdapter> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is CPOAdapter cpoAdapter &&
                   Equals(cpoAdapter);

        #endregion

        #region Equals(CPOAdapter)

        /// <summary>
        /// Compares two CPO adapters for equality.
        /// </summary>
        /// <param name="CPOAdapter">A CPO adapter to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(CPOAdapter? CPOAdapter)

            => CPOAdapter is not null &&
               Id.Equals(CPOAdapter.Id);

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

            => $"OICP {Version.String} CPO Adapter: {Id}";

        #endregion


    }

}
