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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// A WWCP wrapper for the OICP CPO Roaming client which maps
    /// WWCP data structures onto OICP data structures and vice versa.
    /// </summary>
    public class CPOAdapter : WWCP.AWWCPEMPAdapter<ChargeDetailRecord>,
                              WWCP.IEMPRoamingProvider,
                              IEquatable <CPOAdapter>,
                              IComparable<CPOAdapter>,
                              IComparable
    {

        #region Data

        private static readonly  Regex                                             pattern                             = new (@"\s=\s");

        public  static readonly  WWCP.ChargingStationOperatorNameSelectorDelegate  DefaultOperatorNameSelector         = I18N => I18N.FirstText();

        private readonly         HashSet<WWCP.EVSE_Id>                             SuccessfullyUploadedEVSEs           = new();

        #endregion

        #region Properties

        IId WWCP.IAuthorizeStartStop.AuthId
            => Id;

        IId WWCP.ISendChargeDetailRecords.Id
            => Id;

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
        /// The CPO client HTTP logger.
        /// </summary>
        public CPOClient.HTTP_Logger ClientHTTPLogger
            => CPORoaming?.CPOClient?.HTTPLogger;

        /// <summary>
        /// The CPO client logger.
        /// </summary>
        public CPOClient.CPOClientLogger ClientLogger
            => CPORoaming?.CPOClient?.Logger;


        /// <summary>
        /// The CPO server.
        /// </summary>
        public CPOServerAPI CPOServer
            => CPORoaming?.CPOServer;

        /// <summary>
        /// The CPO server HTTP logger.
        /// </summary>
        public CPOServerAPI.HTTP_Logger ServerHTTPLogger
            => CPORoaming?.CPOServer?.HTTPLogger;

        /// <summary>
        /// The CPO server logger.
        /// </summary>
        public CPOServerAPI.ServerAPILogger ServerAPILogger
            => CPORoaming?.CPOServer?.Logger;



        public Func<WWCP.EVSE_Id, EVSE_Id>?                        CustomEVSEIdConverter                            { get; }

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

                          Func<WWCP.EVSE_Id, EVSE_Id>?                        CustomEVSEIdConverter                           = null,
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
                          Boolean                                             DisablePushStatus                               = false,
                          Boolean                                             DisableAuthentication                           = false,
                          Boolean                                             DisableSendChargeDetailRecords                  = false,

                          String                                              EllipticCurve                                   = "P-256",
                          ECPrivateKeyParameters?                             PrivateKey                                      = null,
                          WWCP.PublicKeyCertificates?                         PublicKeyCertificates                           = null)

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
                   ChargeDetailRecordFilter,

                   ServiceCheckEvery,
                   StatusCheckEvery,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushStatus,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords,

                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates)

        {

            this.CPORoaming                                     = CPORoaming      ?? throw new ArgumentNullException(nameof(CPORoaming),      "The given CPO roaming object must not be null!");
            this.CustomEVSEIdConverter                          = CustomEVSEIdConverter;
            this.EVSE2EVSEDataRecord                            = EVSE2EVSEDataRecord;
            this.EVSEStatusUpdate2EVSEStatusRecord              = EVSEStatusUpdate2EVSEStatusRecord;
            this.WWCPChargeDetailRecord2OICPChargeDetailRecord  = WWCPChargeDetailRecord2OICPChargeDetailRecord;

            this.DefaultOperator                                = DefaultOperator ?? throw new ArgumentNullException(nameof(DefaultOperator), "The given charging station operator must not be null!");
            this.DefaultOperatorIdFormat                        = DefaultOperatorIdFormat;
            this.OperatorNameSelector                           = OperatorNameSelector;
            this.DefaultOperatorName                            = (this.OperatorNameSelector is not null
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
                                        MinDuration = TimeSpan.FromSeconds(UInt32.Parse(MinDurationText[..^3]));

                                    if (MinDurationText.EndsWith("min", StringComparison.InvariantCulture))
                                        MinDuration = TimeSpan.FromMinutes(UInt32.Parse(MinDurationText[..^3]));

                                }


                                if (ProductIdElements.ContainsKey("E") &&
                                    Single.TryParse(ProductIdElements["E"], out Single _PlannedEnergy))
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
                         CancellationToken?       CancellationToken   = null,
                         EventTracking_Id?        EventTrackingId     = null,
                         TimeSpan?                RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEs is null)
                EVSEs = Array.Empty<WWCP.EVSE>();


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId is null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;


            WWCP.PushEVSEDataResult? result = null;

            #endregion

            #region Get effective list of EVSEs/EVSEDataRecords to upload

            var Warnings         = new List<Warning>();
            var EVSEDataRecords  = new List<EVSEDataRecord>();

            foreach (var evse in EVSEs)
            {

                try
                {

                    if (evse is null)
                        continue;

                    if (IncludeEVSEs(evse) && IncludeEVSEIds(evse.Id))
                        // WWCP EVSE will be added as internal data "WWCP.EVSE"...
                        EVSEDataRecords.Add(evse.ToOICP(evse.Operator.Name.FirstText(),
                                                        EVSE2EVSEDataRecord));

                    else
                        DebugX.Log(evse.Id + " was filtered!");

                }
                catch (Exception e)
                {
                    DebugX.Log(e.Message + (e.InnerException != null ? " " + e.InnerException.Message : ""));
                    Warnings.Add(Warning.Create(I18NString.Create(Languages.en,
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
                                                  EVSEDataRecords,
                                                  Warnings.Where(warning => warning.IsNeitherNullNorEmpty()),
                                                  RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOAdapter) + "." + nameof(OnPushEVSEDataWWCPRequest));
            }

            #endregion

            DateTime Endtime;
            TimeSpan Runtime;


            if (EVSEDataRecords.Count > 0)
            {

                var operatorId  = DefaultOperator.Id.ToOICP(DefaultOperatorIdFormat);

                var response    = await CPORoaming.PushEVSEData(
                                        new PushEVSEDataRequest(
                                            new OperatorEVSEData(
                                                EVSEDataRecords,
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

                        Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                        Runtime  = Endtime - startTime;
                        result   = WWCP.PushEVSEDataResult.Success(Id,
                                                                   this,
                                                                   EVSEDataRecords.Select(evseDataRecord => evseDataRecord.GetInternalData("WWCP.EVSE") as WWCP.EVSE),
                                                                   response.Response.StatusCode.Description,
                                                                   response.Response.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                       ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.Response.StatusCode.AdditionalInfo))
                                                                       : Warnings,
                                                                   Runtime);

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

                            Warnings.Add(Warning.Create(I18NString.Create(Languages.en, ServerAction.ToString() + " of " + EVSEDataRecords.Count + " EVSEs failed!")));
                            Warnings.Add(Warning.Create(I18NString.Create(Languages.en, response.Response.StatusCode.Code.ToString())));
                            Warnings.Add(Warning.Create(I18NString.Create(Languages.en, response.Response.StatusCode.Description)));

                            if (response.Response.StatusCode.AdditionalInfo.IsNotNullOrEmpty())
                                Warnings.Add(Warning.Create(I18NString.Create(Languages.en, response.Response.StatusCode.AdditionalInfo)));

                            Warnings.Add(Warning.Create(I18NString.Create(Languages.en, "Will try to fix this issue via a 'fullLoad' of all EVSEs!")));

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
                                                                Warnings.Add(Warning.Create(I18NString.Create(Languages.en, e.Message), evse));
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

                            Endtime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                            Runtime = Endtime - startTime;

                            if (FullLoadResponse.IsSuccess())
                            {

                                if (FullLoadResponse.Response.Result == true)
                                    result = WWCP.PushEVSEDataResult.Success(Id,
                                                                             this,
                                                                             EVSEDataRecords.Select(evseDataRecord => evseDataRecord.GetInternalData("WWCP.EVSE") as WWCP.EVSE),
                                                                             FullLoadResponse.Response.StatusCode.Description,
                                                                             FullLoadResponse.Response.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                                 ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, FullLoadResponse.Response.StatusCode.AdditionalInfo))
                                                                                 : Warnings,
                                                                             Runtime);

                                else
                                    result = WWCP.PushEVSEDataResult.Error(Id,
                                                                           this,
                                                                           EVSEDataRecords.Select(evseDataRecord => evseDataRecord.GetInternalData("WWCP.EVSE") as WWCP.EVSE),
                                                                           FullLoadResponse.Response.StatusCode.Description,
                                                                           FullLoadResponse.Response.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                               ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, FullLoadResponse.Response.StatusCode.AdditionalInfo))
                                                                               : Warnings,
                                                                           Runtime);

                            }

                            else
                                result = WWCP.PushEVSEDataResult.Error(Id,
                                                                       this,
                                                                       EVSEDataRecords.Select(evseDataRecord => evseDataRecord.GetInternalData("WWCP.EVSE") as WWCP.EVSE),
                                                                       //FullLoadResponse.HTTPStatusCode.ToString(),
                                                                       //FullLoadResponse.HTTPBody != null
                                                                       //    ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, FullLoadResponse.HTTPBody.ToUTF8String()))
                                                                       //    : Warnings.AddAndReturnList(I18NString.Create(Languages.en, "No HTTP body received!")),
                                                                       Runtime: Runtime);

                            #endregion

                        }

                        // Or a 'fullLoad' Operation failed...
                        else
                        {

                            Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                            Runtime  = Endtime - startTime;
                            result   = WWCP.PushEVSEDataResult.Error(Id,
                                                                     this,
                                                                     EVSEDataRecords.Select(evseDataRecord => evseDataRecord.GetInternalData("WWCP.EVSE") as WWCP.EVSE),
                                                                     //response.HTTPStatusCode.ToString(),
                                                                     //response.HTTPBody != null
                                                                     //    ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.HTTPBody.ToUTF8String()))
                                                                     //    : Warnings.AddAndReturnList(I18NString.Create(Languages.en, "No HTTP body received!")),
                                                                     Runtime: Runtime);

                        }

                    }

                }
                else
                {

                    Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                    Runtime  = Endtime - startTime;
                    result   = WWCP.PushEVSEDataResult.Error(Id,
                                                             this,
                                                             EVSEDataRecords.Select(evseDataRecord => evseDataRecord.GetInternalData("WWCP.EVSE") as WWCP.EVSE),
                                                             //response.HTTPStatusCode.ToString(),
                                                             //response.HTTPBody != null
                                                             //    ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.HTTPBody.ToUTF8String()))
                                                             //    : Warnings.AddAndReturnList(I18NString.Create(Languages.en, "No HTTP body received!")),
                                                             Runtime: Runtime);

                }

            }

            #region ...or no EVSEs to push...

            else
            {

                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - startTime;
                result   = WWCP.PushEVSEDataResult.NoOperation(Id,
                                                               this,
                                                               EVSEs,
                                                               "No EVSEDataRecords to push!",
                                                               Warnings,
                                                               Runtime);

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
                                                   EVSEDataRecords,
                                                   RequestTimeout,
                                                   result,
                                                   Runtime);

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
                           CancellationToken?                  CancellationToken   = null,
                           EventTracking_Id?                   EventTrackingId     = null,
                           TimeSpan?                           RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEStatusUpdates is null)
                EVSEStatusUpdates = Array.Empty<WWCP.EVSEStatusUpdate>();


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId is null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;


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

            DateTime Endtime;
            TimeSpan Runtime;

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


                Endtime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime = Endtime - StartTime;

                if (response.IsSuccess())
                {

                    if (response.Response.Result == true)
                        result = WWCP.PushEVSEStatusResult.Success(Id,
                                                                   this,
                                                                   response.Response.StatusCode.Description,
                                                                   response.Response.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                       ? warnings.AddAndReturnList(I18NString.Create(Languages.en, response.Response.StatusCode.AdditionalInfo))
                                                                       : warnings,
                                                                   Runtime);

                    else
                        result = WWCP.PushEVSEStatusResult.Error(Id,
                                                                 this,
                                                                 EVSEStatusUpdates,
                                                                 response.Response.StatusCode.Description,
                                                                 response.Response.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                     ? warnings.AddAndReturnList(I18NString.Create(Languages.en, response.Response.StatusCode.AdditionalInfo))
                                                                     : warnings,
                                                                 Runtime);

                }
                else
                    result = WWCP.PushEVSEStatusResult.Error(Id,
                                                             this,
                                                             EVSEStatusUpdates,
                                                             //response.HTTPStatusCode.ToString(),
                                                             //response.HTTPBody != null
                                                             //    ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.HTTPBody.ToUTF8String()))
                                                             //    : Warnings.AddAndReturnList(I18NString.Create(Languages.en, "No HTTP body received!")),
                                                             Runtime: Runtime);

            }

            #region ...or no EVSEs to push...

            else
            {

                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - StartTime;
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

                OnPushEVSEStatusWWCPResponse?.Invoke(Endtime,
                                                     Timestamp.Value,
                                                     this,
                                                     Id,
                                                     EventTrackingId,
                                                     RoamingNetwork.Id,
                                                     ServerAction,
                                                     evseStatusList,
                                                     RequestTimeout,
                                                     result,
                                                     Runtime);

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
        async Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.SetStaticData(WWCP.IEVSE               EVSE,
                                            WWCP.TransmissionTypes   TransmissionType,

                                            DateTime?                Timestamp,
                                            CancellationToken?       CancellationToken,
                                            EventTracking_Id?        EventTrackingId,
                                            TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

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

                return WWCP.PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(new WWCP.IEVSE[] { EVSE },
                                      ActionTypes.FullLoad,
                                      null,

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
        async Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.AddStaticData(WWCP.IEVSE               EVSE,
                                            WWCP.TransmissionTypes   TransmissionType,

                                            DateTime?                Timestamp,
                                            CancellationToken?       CancellationToken,
                                            EventTracking_Id?        EventTrackingId,
                                            TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

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

                return WWCP.PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(new WWCP.IEVSE[] { EVSE },
                                      ActionTypes.Insert,
                                      null,

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
        async Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.UpdateStaticData(WWCP.IEVSE               EVSE,
                                               String?                  PropertyName,
                                               Object?                  OldValue,
                                               Object?                  NewValue,
                                               WWCP.TransmissionTypes   TransmissionType,

                                               DateTime?                Timestamp,
                                               CancellationToken?       CancellationToken,
                                               EventTracking_Id?        EventTrackingId,
                                               TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (EVSE         == null)
                throw new ArgumentNullException(nameof(EVSE),          "The given EVSE must not be null!");

            if (PropertyName == null)
                throw new ArgumentNullException(nameof(PropertyName),  "The given EVSE property name must not be null!");

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

                            var List = new List<PropertyUpdateInfos> {
                                           new PropertyUpdateInfos(PropertyName, OldValue, NewValue)
                                       };
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

                return WWCP.PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(new WWCP.IEVSE[] { EVSE },
                                      ActionTypes.Update,
                                      null,

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
        async Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.DeleteStaticData(WWCP.IEVSE               EVSE,
                                               WWCP.TransmissionTypes   TransmissionType,

                                               DateTime?                Timestamp,
                                               CancellationToken?       CancellationToken,
                                               EventTracking_Id?        EventTrackingId,
                                               TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

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

                return WWCP.PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(new WWCP.IEVSE[] { EVSE },
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
        async Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.SetStaticData(IEnumerable<WWCP.IEVSE>  EVSEs,
                                            WWCP.TransmissionTypes   TransmissionType,

                                            DateTime?                Timestamp,
                                            CancellationToken?       CancellationToken,
                                            EventTracking_Id?        EventTrackingId,
                                            TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return WWCP.PushEVSEDataResult.NoOperation(Id, this);

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

                            return WWCP.PushEVSEDataResult.Enqueued(Id, this);

                        }

                        return WWCP.PushEVSEDataResult.NoOperation(Id, this);

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
        async Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.AddStaticData(IEnumerable<WWCP.IEVSE>  EVSEs,
                                            WWCP.TransmissionTypes   TransmissionType,

                                            DateTime?                Timestamp,
                                            CancellationToken?       CancellationToken,
                                            EventTracking_Id?        EventTrackingId,
                                            TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return WWCP.PushEVSEDataResult.NoOperation(Id, this);

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

                            return WWCP.PushEVSEDataResult.Enqueued(Id, this);

                        }

                        return WWCP.PushEVSEDataResult.NoOperation(Id, this);

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
        async Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.UpdateStaticData(IEnumerable<WWCP.IEVSE>  EVSEs,
                                               WWCP.TransmissionTypes   TransmissionType,

                                               DateTime?                Timestamp,
                                               CancellationToken?       CancellationToken,
                                               EventTracking_Id?        EventTrackingId,
                                               TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return WWCP.PushEVSEDataResult.NoOperation(Id, this);

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

                            return WWCP.PushEVSEDataResult.Enqueued(Id, this);

                        }

                        return WWCP.PushEVSEDataResult.NoOperation(Id, this);

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
        async Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.DeleteStaticData(IEnumerable<WWCP.IEVSE>  EVSEs,
                                               WWCP.TransmissionTypes   TransmissionType,

                                               DateTime?                Timestamp,
                                               CancellationToken?       CancellationToken,
                                               EventTracking_Id?        EventTrackingId,
                                               TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null || !EVSEs.Any())
                return WWCP.PushEVSEDataResult.NoOperation(Id, this);

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

                            return WWCP.PushEVSEDataResult.Enqueued(Id, this);

                        }

                        return WWCP.PushEVSEDataResult.NoOperation(Id, this);

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
                                      ActionTypes.Delete,
                                      null,

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
        Task<WWCP.PushEVSEAdminStatusResult>

            WWCP.ISendAdminStatus.UpdateAdminStatus(IEnumerable<WWCP.EVSEAdminStatusUpdate>  AdminStatusUpdates,
                                                    WWCP.TransmissionTypes                   TransmissionType,

                                                    DateTime?                                Timestamp,
                                                    CancellationToken?                       CancellationToken,
                                                    EventTracking_Id                         EventTrackingId,
                                                    TimeSpan?                                RequestTimeout)


                => Task.FromResult(WWCP.PushEVSEAdminStatusResult.NoOperation(Id, this));

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
        public async Task<WWCP.PushEVSEStatusResult>

            UpdateStatus(IEnumerable<WWCP.EVSEStatusUpdate>  StatusUpdates,
                         WWCP.TransmissionTypes              TransmissionType,

                         DateTime?                           Timestamp,
                         CancellationToken?                  CancellationToken,
                         EventTracking_Id                    EventTrackingId,
                         TimeSpan?                           RequestTimeout)

        {

            #region Initial checks

            if (StatusUpdates == null || !StatusUpdates.Any())
                return WWCP.PushEVSEStatusResult.NoOperation(Id, this);

            WWCP.PushEVSEStatusResult result = null;

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
                var LockTaken    = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        var FilteredUpdates = StatusUpdates.Where(statusupdate => IncludeEVSEIds(statusupdate.Id)).
                                                            ToArray();

                        if (FilteredUpdates.Length > 0)
                        {

                            foreach (var update in FilteredUpdates)
                            {

                                // Delay the status update until the EVSE data had been uploaded!
                                if (EVSEsToAddQueue.Any(evse => evse.Id == update.Id))
                                    EVSEStatusChangesDelayedQueue.Add(update);

                                else
                                    EVSEStatusChangesFastQueue.Add(update);

                            }

                            invokeTimer = true;

                            result = WWCP.PushEVSEStatusResult.Enqueued(Id, this);

                        }

                        result = WWCP.PushEVSEStatusResult.NoOperation(Id, this);

                    }

                }
                finally
                {
                    if (LockTaken)
                        DataAndStatusLock.Release();
                }

                if (!LockTaken)
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
        async Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.SetStaticData(WWCP.IChargingStation    ChargingStation,
                                            WWCP.TransmissionTypes   TransmissionType,

                                            DateTime?                Timestamp,
                                            CancellationToken?       CancellationToken,
                                            EventTracking_Id?        EventTrackingId,
                                            TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

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

                return WWCP.PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingStation.EVSEs,
                                      ActionTypes.FullLoad,
                                      null,

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
        async Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.AddStaticData(WWCP.IChargingStation    ChargingStation,
                                            WWCP.TransmissionTypes   TransmissionType,

                                            DateTime?                Timestamp,
                                            CancellationToken?       CancellationToken,
                                            EventTracking_Id?        EventTrackingId,
                                            TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

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

                return WWCP.PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingStation.EVSEs,
                                      ActionTypes.Insert,
                                      null,

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
        async Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.UpdateStaticData(WWCP.IChargingStation    ChargingStation,
                                               String?                  PropertyName,
                                               Object?                  OldValue,
                                               Object?                  NewValue,
                                               WWCP.TransmissionTypes   TransmissionType,

                                               DateTime?                Timestamp,
                                               CancellationToken?       CancellationToken,
                                               EventTracking_Id?        EventTrackingId,
                                               TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

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
                                var List = new List<PropertyUpdateInfos> {
                                    new PropertyUpdateInfos(PropertyName, OldValue, NewValue)
                                };
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

                return WWCP.PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingStation,
                                      ActionTypes.Update,
                                      null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.DeleteStaticData(WWCP.IChargingStation    ChargingStation,
                                               WWCP.TransmissionTypes   TransmissionType,

                                               DateTime?                Timestamp,
                                               CancellationToken?       CancellationToken,
                                               EventTracking_Id?        EventTrackingId,
                                               TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            return PushEVSEData(ChargingStation.EVSEs,
                                ActionTypes.Delete,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.SetStaticData(IEnumerable<WWCP.IChargingStation>  ChargingStations,
                                            WWCP.TransmissionTypes              TransmissionType,

                                            DateTime?                           Timestamp,
                                            CancellationToken?                  CancellationToken,
                                            EventTracking_Id?                   EventTrackingId,
                                            TimeSpan?                           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return PushEVSEData(ChargingStations.SafeSelectMany(station => station.EVSEs),
                                ActionTypes.FullLoad,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.AddStaticData(IEnumerable<WWCP.IChargingStation>  ChargingStations,
                                            WWCP.TransmissionTypes              TransmissionType,


                                            DateTime?                           Timestamp,
                                            CancellationToken?                  CancellationToken,
                                            EventTracking_Id?                   EventTrackingId,
                                            TimeSpan?                           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return PushEVSEData(ChargingStations.SafeSelectMany(station => station.EVSEs),
                                ActionTypes.Insert,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.UpdateStaticData(IEnumerable<WWCP.IChargingStation>  ChargingStations,
                                               WWCP.TransmissionTypes              TransmissionType,

                                               DateTime?                           Timestamp,
                                               CancellationToken?                  CancellationToken,
                                               EventTracking_Id?                   EventTrackingId,
                                               TimeSpan?                           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return PushEVSEData(ChargingStations.SafeSelectMany(station => station.EVSEs),
                                ActionTypes.Update,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.DeleteStaticData(IEnumerable<WWCP.IChargingStation>  ChargingStations,
                                               WWCP.TransmissionTypes              TransmissionType,

                                               DateTime?                           Timestamp,
                                               CancellationToken?                  CancellationToken,
                                               EventTracking_Id?                   EventTrackingId,
                                               TimeSpan?                           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return PushEVSEData(ChargingStations.SafeSelectMany(station => station.EVSEs),
                                ActionTypes.Delete,
                                null,

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
        Task<WWCP.PushChargingStationAdminStatusResult>

            WWCP.ISendAdminStatus.UpdateAdminStatus(IEnumerable<WWCP.ChargingStationAdminStatusUpdate>  AdminStatusUpdates,
                                                    WWCP.TransmissionTypes                              TransmissionType,

                                                    DateTime?                                           Timestamp,
                                                    CancellationToken?                                  CancellationToken,
                                                    EventTracking_Id                                    EventTrackingId,
                                                    TimeSpan?                                           RequestTimeout)


                => Task.FromResult(WWCP.PushChargingStationAdminStatusResult.NoOperation(Id, this));

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
        Task<WWCP.PushChargingStationStatusResult>

            WWCP.ISendStatus.UpdateStatus(IEnumerable<WWCP.ChargingStationStatusUpdate>  StatusUpdates,
                                          WWCP.TransmissionTypes                         TransmissionType,

                                          DateTime?                                      Timestamp,
                                          CancellationToken?                             CancellationToken,
                                          EventTracking_Id                               EventTrackingId,
                                          TimeSpan?                                      RequestTimeout)


                => Task.FromResult(WWCP.PushChargingStationStatusResult.NoOperation(Id, this));

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
        async Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.SetStaticData(WWCP.ChargingPool        ChargingPool,
                                            WWCP.TransmissionTypes   TransmissionType,

                                            DateTime?                Timestamp,
                                            CancellationToken?       CancellationToken,
                                            EventTracking_Id         EventTrackingId,
                                            TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

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

                return WWCP.PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingPool.EVSEs,
                                      ActionTypes.FullLoad,
                                      null,

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
        async Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.AddStaticData(WWCP.ChargingPool        ChargingPool,
                                            WWCP.TransmissionTypes   TransmissionType,

                                            DateTime?                Timestamp,
                                            CancellationToken?       CancellationToken,
                                            EventTracking_Id         EventTrackingId,
                                            TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

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

                return WWCP.PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingPool.EVSEs,
                                      ActionTypes.Insert,
                                      null,

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
        async Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.UpdateStaticData(WWCP.ChargingPool        ChargingPool,
                                               String                   PropertyName,
                                               Object                   OldValue,
                                               Object                   NewValue,
                                               WWCP.TransmissionTypes   TransmissionType,

                                               DateTime?                Timestamp,
                                               CancellationToken?       CancellationToken,
                                               EventTracking_Id         EventTrackingId,
                                               TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

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
                                var List = new List<PropertyUpdateInfos> {
                                    new PropertyUpdateInfos(PropertyName, OldValue, NewValue)
                                };
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

                return WWCP.PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await PushEVSEData(ChargingPool.EVSEs,
                                      ActionTypes.Update,
                                      null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.DeleteStaticData(WWCP.ChargingPool       ChargingPool,
                                               WWCP.TransmissionTypes  TransmissionType,

                                               DateTime?               Timestamp,
                                               CancellationToken?      CancellationToken,
                                               EventTracking_Id        EventTrackingId,
                                               TimeSpan?               RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            return PushEVSEData(ChargingPool.EVSEs,
                                ActionTypes.Delete,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.SetStaticData(IEnumerable<WWCP.ChargingPool>  ChargingPools,
                                            WWCP.TransmissionTypes          TransmissionType,

                                            DateTime?                       Timestamp,
                                            CancellationToken?              CancellationToken,
                                            EventTracking_Id                EventTrackingId,
                                            TimeSpan?                       RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return PushEVSEData(ChargingPools.SafeSelectMany(pool => pool.EVSEs),
                                ActionTypes.FullLoad,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.AddStaticData(IEnumerable<WWCP.ChargingPool>  ChargingPools,
                                            WWCP.TransmissionTypes          TransmissionType,

                                            DateTime?                       Timestamp,
                                            CancellationToken?              CancellationToken,
                                            EventTracking_Id                EventTrackingId,
                                            TimeSpan?                       RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return PushEVSEData(ChargingPools.SafeSelectMany(pool => pool.EVSEs),
                                ActionTypes.Insert,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.UpdateStaticData(IEnumerable<WWCP.ChargingPool>  ChargingPools,
                                               WWCP.TransmissionTypes          TransmissionType,

                                               DateTime?                       Timestamp,
                                               CancellationToken?              CancellationToken,
                                               EventTracking_Id                EventTrackingId,
                                               TimeSpan?                       RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return PushEVSEData(ChargingPools.SafeSelectMany(pool => pool.EVSEs),
                                ActionTypes.Update,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.DeleteStaticData(IEnumerable<WWCP.ChargingPool>  ChargingPools,
                                               WWCP.TransmissionTypes          TransmissionType,

                                               DateTime?                       Timestamp,
                                               CancellationToken?              CancellationToken,
                                               EventTracking_Id                EventTrackingId,
                                               TimeSpan?                       RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return PushEVSEData(ChargingPools.SafeSelectMany(pool => pool.EVSEs),
                                ActionTypes.Delete,
                                null,

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
        Task<WWCP.PushChargingPoolAdminStatusResult>

            WWCP.ISendAdminStatus.UpdateAdminStatus(IEnumerable<WWCP.ChargingPoolAdminStatusUpdate>  AdminStatusUpdates,
                                                    WWCP.TransmissionTypes                           TransmissionType,

                                                    DateTime?                                        Timestamp,
                                                    CancellationToken?                               CancellationToken,
                                                    EventTracking_Id                                 EventTrackingId,
                                                    TimeSpan?                                        RequestTimeout)


                => Task.FromResult(WWCP.PushChargingPoolAdminStatusResult.NoOperation(Id, this));

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
        Task<WWCP.PushChargingPoolStatusResult>

            WWCP.ISendStatus.UpdateStatus(IEnumerable<WWCP.ChargingPoolStatusUpdate>  StatusUpdates,
                                          WWCP.TransmissionTypes                      TransmissionType,

                                          DateTime?                                   Timestamp,
                                          CancellationToken?                          CancellationToken,
                                          EventTracking_Id                            EventTrackingId,
                                          TimeSpan?                                   RequestTimeout)


                => Task.FromResult(WWCP.PushChargingPoolStatusResult.NoOperation(Id, this));

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.SetStaticData(WWCP.ChargingStationOperator  ChargingStationOperator,

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
                                ActionTypes.FullLoad,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.AddStaticData(WWCP.ChargingStationOperator  ChargingStationOperator,

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
                                ActionTypes.Insert,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.UpdateStaticData(WWCP.ChargingStationOperator  ChargingStationOperator,

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
                                ActionTypes.Update,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.DeleteStaticData(WWCP.ChargingStationOperator  ChargingStationOperator,

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
                                ActionTypes.Delete,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.SetStaticData(IEnumerable<WWCP.ChargingStationOperator>  ChargingStationOperators,

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
                                ActionTypes.FullLoad,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.AddStaticData(IEnumerable<WWCP.ChargingStationOperator>  ChargingStationOperators,

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
                                ActionTypes.Insert,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.UpdateStaticData(IEnumerable<WWCP.ChargingStationOperator>  ChargingStationOperators,

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
                                ActionTypes.Update,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.DeleteStaticData(IEnumerable<WWCP.ChargingStationOperator>  ChargingStationOperators,

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
                                ActionTypes.Delete,
                                null,

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
        Task<WWCP.PushChargingStationOperatorAdminStatusResult>

            WWCP.ISendAdminStatus.UpdateAdminStatus(IEnumerable<WWCP.ChargingStationOperatorAdminStatusUpdate>  AdminStatusUpdates,
                                                    WWCP.TransmissionTypes                                      TransmissionType,

                                                    DateTime?                                                   Timestamp,
                                                    CancellationToken?                                          CancellationToken,
                                                    EventTracking_Id                                            EventTrackingId,
                                                    TimeSpan?                                                   RequestTimeout)


                => Task.FromResult(WWCP.PushChargingStationOperatorAdminStatusResult.NoOperation(Id, this));

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
        Task<WWCP.PushChargingStationOperatorStatusResult>

            WWCP.ISendStatus.UpdateStatus(IEnumerable<WWCP.ChargingStationOperatorStatusUpdate>  StatusUpdates,
                                          WWCP.TransmissionTypes                                 TransmissionType,

                                          DateTime?                                              Timestamp,
                                          CancellationToken?                                     CancellationToken,
                                          EventTracking_Id                                       EventTrackingId,
                                          TimeSpan?                                              RequestTimeout)


                => Task.FromResult(WWCP.PushChargingStationOperatorStatusResult.NoOperation(Id, this));

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.SetStaticData(WWCP.RoamingNetwork      RoamingNetwork,

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
                                ActionTypes.FullLoad,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.AddStaticData(WWCP.RoamingNetwork      RoamingNetwork,

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
                                ActionTypes.Insert,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.UpdateStaticData(WWCP.RoamingNetwork      RoamingNetwork,

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
                                ActionTypes.Update,
                                null,

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
        Task<WWCP.PushEVSEDataResult>

            WWCP.ISendPOIData.DeleteStaticData(WWCP.RoamingNetwork      RoamingNetwork,

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
                                ActionTypes.Delete,
                                null,

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
        Task<WWCP.PushRoamingNetworkAdminStatusResult>

            WWCP.ISendAdminStatus.UpdateAdminStatus(IEnumerable<WWCP.RoamingNetworkAdminStatusUpdate>  AdminStatusUpdates,
                                                    WWCP.TransmissionTypes                             TransmissionType,

                                                    DateTime?                                          Timestamp,
                                                    CancellationToken?                                 CancellationToken,
                                                    EventTracking_Id                                   EventTrackingId,
                                                    TimeSpan?                                          RequestTimeout)


                => Task.FromResult(WWCP.PushRoamingNetworkAdminStatusResult.NoOperation(Id, this));

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
        Task<WWCP.PushRoamingNetworkStatusResult>

            WWCP.ISendStatus.UpdateStatus(IEnumerable<WWCP.RoamingNetworkStatusUpdate>  StatusUpdates,
                                          WWCP.TransmissionTypes                        TransmissionType,

                                          DateTime?                                     Timestamp,
                                          CancellationToken?                            CancellationToken,
                                          EventTracking_Id                              EventTrackingId,
                                          TimeSpan?                                     RequestTimeout)


                => Task.FromResult(WWCP.PushRoamingNetworkStatusResult.NoOperation(Id, this));

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
                           CancellationToken?                CancellationToken     = null,
                           EventTracking_Id?                 EventTrackingId       = null,
                           TimeSpan?                         RequestTimeout        = null)

        {

            #region Initial checks

            if (LocalAuthentication is null)
                throw new ArgumentNullException(nameof(LocalAuthentication),  "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            EventTrackingId ??= EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

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
            WWCP.AuthStartResult?  result = null;

            var operatorId      = (OperatorId ?? DefaultOperator.Id).ToOICP(DefaultOperatorIdFormat);
            var evseId          = ChargingLocation?.EVSEId?.ToOICP(CustomEVSEIdConverter);
            var identification  = LocalAuthentication.ToOICP();

            if (!operatorId.HasValue)
            {
                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;
                result   = WWCP.AuthStartResult.AdminDown(Id,
                                                          this,
                                                          SessionId,
                                                          Runtime: runtime);
            }

            // An optional EVSE Id is given, but it is invalid!
            else if (ChargingLocation?.EVSEId.HasValue == true && !evseId.HasValue)
            {
                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;
                result   = WWCP.AuthStartResult.UnknownLocation(Id,
                                                                this,
                                                                SessionId,
                                                                Runtime: runtime);
            }

            else if (identification?.RFIDId is null && identification?.RFIDIdentification is null)
            {
                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;
                result   = WWCP.AuthStartResult.InvalidToken(Id,
                                                             this,
                                                             SessionId,
                                                             Runtime: runtime);
            }

            else if (DisableAuthentication)
            {
                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;
                result   = WWCP.AuthStartResult.AdminDown(Id,
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
                        result = WWCP.AuthStartResult.Authorized(
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

                result ??= WWCP.AuthStartResult.NotAuthorized(
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
                                                 result,
                                                 runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOAdapter) + "." + nameof(OnAuthorizeStartResponse));
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
                          CancellationToken?                CancellationToken     = null,
                          EventTracking_Id?                 EventTrackingId       = null,
                          TimeSpan?                         RequestTimeout        = null)
        {

            #region Initial checks

            if (LocalAuthentication is null)
                throw new ArgumentNullException(nameof(LocalAuthentication), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            EventTrackingId ??= EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

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


            DateTime             endtime;
            TimeSpan             runtime;
            WWCP.AuthStopResult  result;

            var operatorId      = (OperatorId ?? DefaultOperator.Id).ToOICP(DefaultOperatorIdFormat);
            var sessionId       = SessionId.ToOICP();
            var evseId          = ChargingLocation?.EVSEId?.ToOICP(CustomEVSEIdConverter);
            var identification  = LocalAuthentication.ToOICP();

            if (!operatorId.HasValue)
            {
                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;
                result   = WWCP.AuthStopResult.AdminDown(Id,
                                                         this,
                                                         SessionId,
                                                         Runtime: runtime);
            }

            else if (!sessionId.HasValue)
            {
                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;
                result   = WWCP.AuthStopResult.InvalidSessionId(Id,
                                                                this,
                                                                SessionId,
                                                                Runtime: runtime);
            }

            // An optional EVSE Id is given, but it is invalid!
            else if (ChargingLocation?.EVSEId.HasValue == true && !evseId.HasValue)
            {
                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;
                result   = WWCP.AuthStopResult.UnknownLocation(Id,
                                                               this,
                                                               SessionId,
                                                               Runtime: runtime);
            }

            else if (identification?.RFIDId is null && identification?.RFIDIdentification is null)
            {
                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;
                result   = WWCP.AuthStopResult.InvalidToken(Id,
                                                            this,
                                                            SessionId,
                                                            Runtime: runtime);
            }

            else if (DisableAuthentication)
            {
                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;
                result   = WWCP.AuthStopResult.AdminDown(Id,
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

                    result = WWCP.AuthStopResult.Authorized(
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
                    result = WWCP.AuthStopResult.NotAuthorized(
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
                                                result,
                                                runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOAdapter) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

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
                                    CancellationToken?                    CancellationToken,
                                    EventTracking_Id                      EventTrackingId,
                                    TimeSpan?                             RequestTimeout)

        {

            #region Initial checks

            if (ChargeDetailRecords is null)
                throw new ArgumentNullException(nameof(ChargeDetailRecords),  "The given enumeration of charge detail records must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            EventTrackingId ??= EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;


            DateTime             endtime;
            TimeSpan             runtime;
            WWCP.SendCDRsResult  results;

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

                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;
                results  = WWCP.SendCDRsResult.AdminDown(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                         Id,
                                                         this,
                                                         ChargeDetailRecords,
                                                         Runtime: runtime);

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

                                    ChargeDetailRecordsQueue.Add(ChargeDetailRecord.ToOICP(WWCPChargeDetailRecord2OICPChargeDetailRecord));
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

                            endtime      = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                            runtime      = endtime - startTime;
                            results      = WWCP.SendCDRsResult.Enqueued(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                        Id,
                                                                        this,
                                                                        ChargeDetailRecords,
                                                                        I18NString.Create(Languages.en, "Enqueued for at least " + FlushChargeDetailRecordsEvery.TotalSeconds + " seconds!"),
                                                                        //SendCDRsResults.SafeWhere(cdrresult => cdrresult.Result != SendCDRResultTypes.Enqueued),
                                                                        Runtime: runtime);
                            invokeTimer  = true;

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
                                results = WWCP.SendCDRsResult.Success(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                      Id,
                                                                      this,
                                                                      ChargeDetailRecords,
                                                                      Runtime: runtime);

                            else
                                results = WWCP.SendCDRsResult.Error(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
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
                        results  = WWCP.SendCDRsResult.Timeout(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
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
                                           results,
                                           runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOAdapter) + "." + nameof(OnSendCDRsResponse));
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

            var EVSEsToAddQueueCopy                = new HashSet<WWCP.IEVSE>();
            var EVSEsToUpdateQueueCopy             = new HashSet<WWCP.IEVSE>();
            var EVSEStatusChangesDelayedQueueCopy  = new List<WWCP.EVSEStatusUpdate>();
            var EVSEsToRemoveQueueCopy             = new HashSet<WWCP.IEVSE>();
            var EVSEsUpdateLogCopy                 = new Dictionary<WWCP.IEVSE,            PropertyUpdateInfos[]>();
            var ChargingStationsUpdateLogCopy      = new Dictionary<WWCP.IChargingStation, PropertyUpdateInfos[]>();
            var ChargingPoolsUpdateLogCopy         = new Dictionary<WWCP.ChargingPool,     PropertyUpdateInfos[]>();

            var LockTaken = await DataAndStatusLock.WaitAsync(0);

            try
            {

                if (LockTaken)
                {

                    // Copy 'EVSEs to add', remove originals...
                    EVSEsToAddQueueCopy                      = new HashSet<WWCP.IEVSE>                (EVSEsToAddQueue);
                    EVSEsToAddQueue.Clear();

                    // Copy 'EVSEs to update', remove originals...
                    EVSEsToUpdateQueueCopy                   = new HashSet<WWCP.IEVSE>                (EVSEsToUpdateQueue);
                    EVSEsToUpdateQueue.Clear();

                    // Copy 'EVSE status changes', remove originals...
                    EVSEStatusChangesDelayedQueueCopy        = new List<WWCP.EVSEStatusUpdate>       (EVSEStatusChangesDelayedQueue);
                    EVSEStatusChangesDelayedQueueCopy.AddRange(EVSEsToAddQueueCopy.SafeSelect(evse => new WWCP.EVSEStatusUpdate(evse.Id, evse.Status, evse.Status)));
                    EVSEStatusChangesDelayedQueue.Clear();

                    // Copy 'EVSEs to remove', remove originals...
                    EVSEsToRemoveQueueCopy                   = new HashSet<WWCP.IEVSE>                (EVSEsToRemoveQueue);
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

                //OnWWCPEMPAdapterException?.Invoke(Timestamp.Now,
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
            var EventTrackingId                           = EventTracking_Id.New;
            var NumberOfSuccessfullyUploadedEVSEs_before  = SuccessfullyUploadedEVSEs.Count;

            #region Send new EVSE data

            if (EVSEsToAddQueueCopy.Any())
            {

                var EVSEsToAddTask = await PushEVSEData(EVSEsToAddQueueCopy,
                                                        //_FlushEVSEDataRunId == 1
                                                        //    ? ActionTypes.FullLoad
                                                        //    : ActionTypes.Update,
                                                        NumberOfSuccessfullyUploadedEVSEs_before == 0
                                                            ? ActionTypes.FullLoad
                                                            : ActionTypes.Update,
                                                        EventTrackingId: EventTrackingId);

                foreach (var evseId in EVSEsToAddTask.SuccessfulEVSEs)
                    SuccessfullyUploadedEVSEs.Add(evseId.Id);

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

            if (EVSEsToUpdateQueueCopy.Any())
            {

                // Surpress EVSE data updates for all newly added EVSEs
                foreach (var _evse in EVSEsToUpdateQueueCopy.Where(evse => EVSEsToAddQueueCopy.Contains(evse)).ToArray())
                    EVSEsToUpdateQueueCopy.Remove(_evse);

                if (EVSEsToUpdateQueueCopy.Any())
                {

                    var EVSEsToUpdateResult = await PushEVSEData(EVSEsToUpdateQueueCopy,
                                                                 ActionTypes.Update,
                                                                 EventTrackingId: EventTrackingId);

                    foreach (var evseId in EVSEsToUpdateResult.SuccessfulEVSEs)
                        SuccessfullyUploadedEVSEs.Add(evseId.Id);

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
                EVSEStatusChangesDelayedQueueCopy.Count > 0)
            {

                var PushEVSEStatusTask = await PushEVSEStatus(EVSEStatusChangesDelayedQueueCopy.Where(evseStatusUpdate => SuccessfullyUploadedEVSEs.Contains(evseStatusUpdate.Id)),
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

            if (EVSEsToRemoveQueueCopy.Count > 0)
            {

                var EVSEsToRemove = EVSEsToRemoveQueueCopy.ToArray();

                if (EVSEsToRemove.Length > 0)
                {

                    var EVSEsToRemoveTask = await PushEVSEData(EVSEsToRemove,
                                                               ActionTypes.Delete,
                                                               EventTrackingId: EventTrackingId);

                    foreach (var evseId in EVSEsToRemoveTask.SuccessfulEVSEs)
                        SuccessfullyUploadedEVSEs.Remove(evseId.Id);

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
            => EVSEStatusChangesFastQueue.Count == 0;

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
                    EVSEStatusFastQueueCopy = new List<WWCP.EVSEStatusUpdate>(EVSEStatusChangesFastQueue.Where(evsestatuschange => !EVSEsToAddQueue.Any(evse => evse.Id == evsestatuschange.Id)));

                    // Add all evse status changes of EVSE *NOT YET UPLOADED* into the delayed queue...
                    var EVSEStatusChangesDelayed = EVSEStatusChangesFastQueue.Where(evsestatuschange => EVSEsToAddQueue.Any(evse => evse.Id == evsestatuschange.Id)).ToArray();

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
            => ChargeDetailRecordsQueue.Count == 0;

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

            => "OICP" + Version.Number + " CPO Adapter " + Id;

        #endregion


    }

}
