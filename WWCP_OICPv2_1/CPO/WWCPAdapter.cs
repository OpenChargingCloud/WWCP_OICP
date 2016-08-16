/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
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
    public class WWCPAdapter : AEVSEOperatorRoamingProvider
    {

        #region Data

        private        readonly  EVSE2EVSEDataRecordDelegate        _EVSE2EVSEDataRecord;

        private        readonly  EVSEDataRecord2XMLDelegate         _EVSEDataRecord2XML;

        private        readonly  ChargingStationOperatorNameSelectorDelegate   _OperatorNameSelector;

        private static readonly  Regex                              pattern = new Regex(@"\s=\s");

        public  static readonly  ChargingStationOperatorNameSelectorDelegate   DefaultOperatorNameSelector = I18N => I18N.FirstText;

        #endregion

        #region Properties

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


        /// <summary>
        /// An optional default Charging Station Operator.
        /// </summary>
        public ChargingStationOperator DefaultOperator { get; }

        #endregion

        #region Events

        // Client logging...

        #region OnPushEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever new EVSE data will be send upstream.
        /// </summary>
        public override event WWCP.OnPushEVSEDataRequestDelegate   OnPushEVSEDataRequest;

        /// <summary>
        /// An event fired whenever new EVSE data had been sent upstream.
        /// </summary>
        public override event WWCP.OnPushEVSEDataResponseDelegate  OnPushEVSEDataResponse;

        #endregion

        #region OnPushEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever new EVSE status will be send upstream.
        /// </summary>
        public override event WWCP.OnPushEVSEStatusRequestDelegate   OnPushEVSEStatusRequest;

        /// <summary>
        /// An event fired whenever new EVSE status had been sent upstream.
        /// </summary>
        public override event WWCP.OnPushEVSEStatusResponseDelegate  OnPushEVSEStatusResponse;

        #endregion

        #region OnAuthorizeStart/-Started

        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging.
        /// </summary>
        public override event WWCP.OnAuthorizeStartDelegate              OnAuthorizeStart;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging.
        /// </summary>
        public override event OnAuthorizeStartedDelegate                 OnAuthorizeStarted;

        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given EVSE.
        /// </summary>
        public override event OnAuthorizeEVSEStartDelegate               OnAuthorizeEVSEStart;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given EVSE.
        /// </summary>
        public override event OnAuthorizeEVSEStartedDelegate             OnAuthorizeEVSEStarted;

        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given charging station.
        /// </summary>
        public override event OnAuthorizeChargingStationStartDelegate    OnAuthorizeChargingStationStart;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given charging station.
        /// </summary>
        public override event OnAuthorizeChargingStationStartedDelegate  OnAuthorizeChargingStationStarted;

        #endregion

        #region OnAuthorizeStop/-Stopped

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process.
        /// </summary>
        public override event WWCP.OnAuthorizeStopDelegate               OnAuthorizeStop;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process.
        /// </summary>
        public override event OnAuthorizeStoppedDelegate                 OnAuthorizeStopped;

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given EVSE.
        /// </summary>
        public override event OnAuthorizeEVSEStopDelegate                OnAuthorizeEVSEStop;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given EVSE.
        /// </summary>
        public override event OnAuthorizeEVSEStoppedDelegate             OnAuthorizeEVSEStopped;

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given charging station.
        /// </summary>
        public override event OnAuthorizeChargingStationStopDelegate     OnAuthorizeChargingStationStop;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given charging station.
        /// </summary>
        public override event OnAuthorizeChargingStationStoppedDelegate  OnAuthorizeChargingStationStopped;

        #endregion

        #endregion

        #region Constructor(s)

        #region WWCPAdapter(Id, Name, RoamingNetwork, CPORoaming, EVSE2EVSEDataRecord = null)

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
        /// <param name="DisableAutoUploads">This service can be disabled, e.g. for debugging reasons.</param>
        public WWCPAdapter(RoamingProvider_Id                Id,
                           I18NString                        Name,
                           RoamingNetwork                    RoamingNetwork,

                           CPORoaming                        CPORoaming,
                           EVSE2EVSEDataRecordDelegate       EVSE2EVSEDataRecord   = null,
                           EVSEDataRecord2XMLDelegate        EVSEDataRecord2XML    = null,

                           ChargingStationOperator                      DefaultOperator       = null,
                           ChargingStationOperatorNameSelectorDelegate  OperatorNameSelector  = null,
                           IncludeEVSEDelegate               IncludeEVSEs          = null,
                           TimeSpan?                         ServiceCheckEvery     = null,
                           TimeSpan?                         StatusCheckEvery      = null,
                           Boolean                           DisableAutoUploads    = false)

            : base(Id,
                   Name,
                   RoamingNetwork,

                   IncludeEVSEs,
                   ServiceCheckEvery,
                   StatusCheckEvery,
                   DisableAutoUploads)

        {

            #region Initial checks

            if (CPORoaming == null)
                throw new ArgumentNullException(nameof(CPORoaming),  "The given OICP CPO Roaming object must not be null!");

            #endregion

            this.CPORoaming             = CPORoaming;
            this._EVSE2EVSEDataRecord   = EVSE2EVSEDataRecord;
            this._EVSEDataRecord2XML    = EVSEDataRecord2XML;
            this.DefaultOperator        = DefaultOperator;
            this._OperatorNameSelector  = OperatorNameSelector;

            // Link events...

            #region OnRemoteReservationStart

            this.CPORoaming.OnRemoteReservationStart += async (Timestamp,
                                                               Sender,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               EVSEId,
                                                               ChargingProductId,
                                                               SessionId,
                                                               PartnerSessionId,
                                                               ProviderId,
                                                               eMAId,
                                                               RequestTimeout) => {


                #region Request transformation

                TimeSpan? Duration = null;

                // Analyse the ChargingProductId field and apply the found key/value-pairs
                if (ChargingProductId != null && ChargingProductId.ToString().IsNotNullOrEmpty())
                {

                    var Elements = pattern.Replace(ChargingProductId.ToString(), "=").Split('|').ToArray();

                    if (Elements.Length > 0)
                    {

                        var DurationText = Elements.FirstOrDefault(element => element.StartsWith("D=", StringComparison.InvariantCulture));
                        if (DurationText != null)
                        {

                            DurationText = DurationText.Substring(2);

                            if (DurationText.EndsWith("sec", StringComparison.InvariantCulture))
                                Duration = TimeSpan.FromSeconds(UInt32.Parse(DurationText.Substring(0, DurationText.Length - 3)));

                            if (DurationText.EndsWith("min", StringComparison.InvariantCulture))
                                Duration = TimeSpan.FromMinutes(UInt32.Parse(DurationText.Substring(0, DurationText.Length - 3)));

                        }

                        var ChargingProductText = Elements.FirstOrDefault(element => element.StartsWith("P=", StringComparison.InvariantCulture));
                        if (ChargingProductText != null)
                        {

                            ChargingProductId = ChargingProduct_Id.Parse(DurationText.Substring(2));

                        }

                    }

                }

                #endregion

                var response = await RoamingNetwork.Reserve(EVSEId,
                                                            Duration:           Duration,
                                                            ReservationId:      SessionId != null ? ChargingReservation_Id.Parse(SessionId.ToString()) : null,
                                                            ProviderId:         ProviderId,
                                                            eMAId:              eMAId,
                                                            ChargingProductId:  ChargingProductId,
                                                            eMAIds:             new eMA_Id[] { eMAId },
                                                            Timestamp:          Timestamp,
                                                            CancellationToken:  CancellationToken,
                                                            EventTrackingId:    EventTrackingId,
                                                            RequestTimeout:     RequestTimeout);

                #region Response mapping

                if (response != null)
                {
                    switch (response.Result)
                    {

                        case ReservationResultType.Success:
                            return new eRoamingAcknowledgement(ChargingSession_Id.Parse(response.Reservation.Id.ToString()),
                                                               StatusCodeDescription: "Ready to charge!");

                        case ReservationResultType.InvalidCredentials:
                            return new eRoamingAcknowledgement(StatusCodes.SessionIsInvalid,
                                                               "Session is invalid",
                                                               SessionId: ChargingSession_Id.Parse(response.Reservation.Id.ToString()));

                        case ReservationResultType.Timeout:
                        case ReservationResultType.CommunicationError:
                            return new eRoamingAcknowledgement(StatusCodes.CommunicationToEVSEFailed,
                                                               "Communication to EVSE failed!");

                        case ReservationResultType.AlreadyReserved:
                            return new eRoamingAcknowledgement(StatusCodes.EVSEAlreadyReserved,
                                                               "EVSE already reserved!");

                        case ReservationResultType.AlreadyInUse:
                            return new eRoamingAcknowledgement(StatusCodes.EVSEAlreadyInUse_WrongToken,
                                                               "EVSE is already in use!");

                        case ReservationResultType.UnknownEVSE:
                            return new eRoamingAcknowledgement(StatusCodes.UnknownEVSEID,
                                                               "Unknown EVSE ID!");

                        case ReservationResultType.OutOfService:
                            return new eRoamingAcknowledgement(StatusCodes.EVSEOutOfService,
                                                               "EVSE out of service!");

                    }
                }

                return new eRoamingAcknowledgement(StatusCodes.ServiceNotAvailable,
                                                   "Service not available!",
                                                   SessionId: ChargingSession_Id.Parse(response.Reservation.Id.ToString()));

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

                var response = await _RoamingNetwork.CancelReservation(ChargingReservation_Id.Parse(SessionId.ToString()),
                                                                       ChargingReservationCancellationReason.Deleted,
                                                                       ProviderId,
                                                                       EVSEId,

                                                                       Timestamp,
                                                                       CancellationToken,
                                                                       EventTrackingId,
                                                                       RequestTimeout);

                #region Response mapping

                if (response != null)
                {
                    switch (response.Result)
                    {

                        case CancelReservationResultType.Success:
                            return new eRoamingAcknowledgement(ChargingSession_Id.Parse(response.ReservationId.ToString()),
                                                               StatusCodeDescription: "Reservation deleted!");

                        case CancelReservationResultType.UnknownReservationId:
                            return new eRoamingAcknowledgement(StatusCodes.SessionIsInvalid,
                                                               "Session is invalid!",
                                                               SessionId: SessionId);

                        case CancelReservationResultType.Offline:
                        case CancelReservationResultType.Timeout:
                        case CancelReservationResultType.CommunicationError:
                            return new eRoamingAcknowledgement(StatusCodes.CommunicationToEVSEFailed,
                                                               "Communication to EVSE failed!");

                        case CancelReservationResultType.UnknownEVSE:
                            return new eRoamingAcknowledgement(StatusCodes.UnknownEVSEID,
                                                               "Unknown EVSE ID!");

                        case CancelReservationResultType.OutOfService:
                            return new eRoamingAcknowledgement(StatusCodes.EVSEOutOfService,
                                                               "EVSE out of service!");

                    }
                }

                return new eRoamingAcknowledgement(StatusCodes.ServiceNotAvailable,
                                                   "Service not available!",
                                                   SessionId: SessionId);

                #endregion

            };

            #endregion

            #region OnRemoteStart

            this.CPORoaming.OnRemoteStart += async (Timestamp,
                                                    Sender,
                                                    CancellationToken,
                                                    EventTrackingId,
                                                    EVSEId,
                                                    ChargingProductId,
                                                    SessionId,
                                                    PartnerSessionId,
                                                    ProviderId,
                                                    eMAId,
                                                    RequestTimeout) => {

                #region Request mapping

                ChargingReservation_Id ReservationId = null;

                if (ChargingProductId != null && ChargingProductId.ToString().IsNotNullOrEmpty())
                {

                    var Elements = ChargingProductId.ToString().Split('|').ToArray();

                    if (Elements.Length > 0)
                    {
                        var ChargingReservationIdText = Elements.FirstOrDefault(element => element.StartsWith("R=", StringComparison.InvariantCulture));
                        if (ChargingReservationIdText.IsNotNullOrEmpty())
                            ReservationId = ChargingReservation_Id.Parse(ChargingReservationIdText.Substring(2));
                    }

                }

                #endregion

                var response = await _RoamingNetwork.RemoteStart(EVSEId,
                                                                 ChargingProductId,
                                                                 ReservationId,
                                                                 SessionId,
                                                                 ProviderId,
                                                                 eMAId,

                                                                 Timestamp,
                                                                 CancellationToken,
                                                                 EventTrackingId,
                                                                 RequestTimeout);

                #region Response mapping

                if (response != null)
                {
                    switch (response.Result)
                    {

                        case RemoteStartEVSEResultType.Success:
                            return new eRoamingAcknowledgement(response.Session.Id,
                                                               StatusCodeDescription: "Ready to charge!");

                        case RemoteStartEVSEResultType.InvalidSessionId:
                            return new eRoamingAcknowledgement(StatusCodes.SessionIsInvalid,
                                                               "Session is invalid!",
                                                               SessionId: SessionId);

                        case RemoteStartEVSEResultType.InvalidCredentials:
                            return new eRoamingAcknowledgement(StatusCodes.NoValidContract,
                                                               "No valid contract!");

                        case RemoteStartEVSEResultType.Offline:
                            return new eRoamingAcknowledgement(StatusCodes.CommunicationToEVSEFailed,
                                                               "Communication to EVSE failed!");

                        case RemoteStartEVSEResultType.Timeout:
                        case RemoteStartEVSEResultType.CommunicationError:
                            return new eRoamingAcknowledgement(StatusCodes.CommunicationToEVSEFailed,
                                                               "Communication to EVSE failed!");

                        case RemoteStartEVSEResultType.Reserved:
                            return new eRoamingAcknowledgement(StatusCodes.EVSEAlreadyReserved,
                                                               "EVSE already reserved!");

                        case RemoteStartEVSEResultType.AlreadyInUse:
                            return new eRoamingAcknowledgement(StatusCodes.EVSEAlreadyInUse_WrongToken,
                                                               "EVSE is already in use!");

                        case RemoteStartEVSEResultType.UnknownEVSE:
                            return new eRoamingAcknowledgement(StatusCodes.UnknownEVSEID,
                                                               "Unknown EVSE ID!");

                        case RemoteStartEVSEResultType.OutOfService:
                            return new eRoamingAcknowledgement(StatusCodes.EVSEOutOfService,
                                                               "EVSE out of service!");

                    }
                }

                return new eRoamingAcknowledgement(StatusCodes.ServiceNotAvailable,
                                                   "Service not available!",
                                                   SessionId: SessionId);

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

                var response = await _RoamingNetwork.RemoteStop(EVSEId,
                                                                SessionId,
                                                                ReservationHandling.Close,
                                                                ProviderId,
                                                                null,

                                                                Timestamp,
                                                                CancellationToken,
                                                                EventTrackingId,
                                                                RequestTimeout);

                #region Response mapping

                if (response != null)
                {
                    switch (response.Result)
                    {

                        case RemoteStopEVSEResultType.Success:
                            return new eRoamingAcknowledgement(response.SessionId,
                                                               StatusCodeDescription: "Ready to stop charging!");

                        case RemoteStopEVSEResultType.InvalidSessionId:
                            return new eRoamingAcknowledgement(StatusCodes.SessionIsInvalid,
                                                               "Session is invalid!",
                                                               SessionId: SessionId);

                        case RemoteStopEVSEResultType.Offline:
                        case RemoteStopEVSEResultType.Timeout:
                        case RemoteStopEVSEResultType.CommunicationError:
                            return new eRoamingAcknowledgement(StatusCodes.CommunicationToEVSEFailed,
                                                               "Communication to EVSE failed!");

                        case RemoteStopEVSEResultType.UnknownEVSE:
                            return new eRoamingAcknowledgement(StatusCodes.UnknownEVSEID,
                                                               "Unknown EVSE ID!");

                        case RemoteStopEVSEResultType.OutOfService:
                            return new eRoamingAcknowledgement(StatusCodes.EVSEOutOfService,
                                                               "EVSE out of service!");

                    }
                }

                return new eRoamingAcknowledgement(StatusCodes.ServiceNotAvailable,
                                                   "Service not available!",
                                                   SessionId: SessionId);

                #endregion

            };

            #endregion

        }

        #endregion

        #region WWCPAdapter(Id, Name, RoamingNetwork, CPOClient, CPOServer, EVSEDataRecordProcessing = null)

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
        /// <param name="DisableAutoUploads">This service can be disabled, e.g. for debugging reasons.</param>
        public WWCPAdapter(RoamingProvider_Id                Id,
                           I18NString                        Name,
                           RoamingNetwork                    RoamingNetwork,

                           CPOClient                         CPOClient,
                           CPOServer                         CPOServer,
                           String                            ServerLoggingContext  = CPOServerLogger.DefaultContext,
                           Func<String, String, String>      LogFileCreator        = null,

                           EVSE2EVSEDataRecordDelegate       EVSE2EVSEDataRecord   = null,
                           EVSEDataRecord2XMLDelegate        EVSEDataRecord2XML    = null,

                           ChargingStationOperator                      DefaultOperator       = null,
                           ChargingStationOperatorNameSelectorDelegate  OperatorNameSelector  = null,
                           IncludeEVSEDelegate               IncludeEVSEs          = null,
                           TimeSpan?                         ServiceCheckEvery     = null,
                           TimeSpan?                         StatusCheckEvery      = null,
                           Boolean                           DisableAutoUploads    = false)

            : this(Id,
                   Name,
                   RoamingNetwork,

                   new CPORoaming(CPOClient,
                                  CPOServer,
                                  ServerLoggingContext,
                                  LogFileCreator),

                   EVSE2EVSEDataRecord,
                   EVSEDataRecord2XML,

                   DefaultOperator,
                   OperatorNameSelector,
                   IncludeEVSEs,
                   ServiceCheckEvery,
                   StatusCheckEvery,
                   DisableAutoUploads)

        { }

        #endregion

        #region WWCPAdapter(Id, Name, RoamingNetwork, RemoteHostName, ...)

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
        /// <param name="DisableAutoUploads">This service can be disabled, e.g. for debugging reasons.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public WWCPAdapter(RoamingProvider_Id                   Id,
                           I18NString                           Name,
                           RoamingNetwork                       RoamingNetwork,

                           String                               RemoteHostname,
                           IPPort                               RemoteTCPPort               = null,
                           RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                           X509Certificate                      ClientCert                  = null,
                           String                               RemoteHTTPVirtualHost       = null,
                           String                               HTTPUserAgent               = CPOClient.DefaultHTTPUserAgent,
                           TimeSpan?                            RequestTimeout              = null,

                           String                               ServerName                  = CPOServer.DefaultHTTPServerName,
                           IPPort                               ServerTCPPort               = null,
                           String                               ServerURIPrefix             = "",
                           Boolean                              ServerAutoStart             = false,

                           String                               ClientLoggingContext        = CPOClient.CPOClientLogger.DefaultContext,
                           String                               ServerLoggingContext        = CPOServerLogger.DefaultContext,
                           Func<String, String, String>         LogFileCreator              = null,

                           EVSE2EVSEDataRecordDelegate          EVSE2EVSEDataRecord         = null,
                           EVSEDataRecord2XMLDelegate           EVSEDataRecord2XML          = null,

                           ChargingStationOperator                         DefaultOperator             = null,
                           ChargingStationOperatorNameSelectorDelegate     OperatorNameSelector        = null,
                           IncludeEVSEDelegate                  IncludeEVSEs                = null,
                           TimeSpan?                            ServiceCheckEvery           = null,
                           TimeSpan?                            StatusCheckEvery            = null,
                           Boolean                              DisableAutoUploads          = false,

                           DNSClient                            DNSClient                   = null)

            : this(Id,
                   Name,
                   RoamingNetwork,

                   new CPORoaming(Id.ToString(),
                                  RemoteHostname,
                                  RemoteTCPPort,
                                  RemoteCertificateValidator,
                                  ClientCert,
                                  RemoteHTTPVirtualHost,
                                  HTTPUserAgent,
                                  RequestTimeout,

                                  ServerName,
                                  ServerTCPPort,
                                  ServerURIPrefix,
                                  ServerAutoStart,

                                  ClientLoggingContext,
                                  ServerLoggingContext,
                                  LogFileCreator,

                                  DNSClient),

                   EVSE2EVSEDataRecord,
                   EVSEDataRecord2XML,

                   DefaultOperator,
                   OperatorNameSelector,
                   IncludeEVSEs,
                   ServiceCheckEvery,
                   StatusCheckEvery,
                   DisableAutoUploads)

        { }

        #endregion

        #endregion


        // Outgoing requests...

        #region PushEVSEData...

        #region PushEVSEData(GroupedEVSEs,     ActionType = fullLoad, ...)

        /// <summary>
        /// Upload the EVSE data of the given lookup of EVSEs grouped by their Charging Station Operator.
        /// </summary>
        /// <param name="GroupedEVSEs">A lookup of EVSEs grouped by their Charging Station Operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEData(ILookup<ChargingStationOperator, EVSE>  GroupedEVSEs,
                         WWCP.ActionType              ActionType         = WWCP.ActionType.fullLoad,

                         DateTime?                    Timestamp          = null,
                         CancellationToken?           CancellationToken  = null,
                         EventTracking_Id             EventTrackingId    = null,
                         TimeSpan?                    RequestTimeout     = null)

        {

            #region Initial checks

            if (GroupedEVSEs == null)
                throw new ArgumentNullException(nameof(GroupedEVSEs), "The given lookup of EVSEs must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Get effective number of EVSE data records to upload

            Acknowledgement result = null;

            var _NumberOfEVSEs = GroupedEVSEs.
                                     Where     (group => group.Key != null).
                                     SelectMany(group => group.Where(evse => evse != null)).
                                     Count();

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            #endregion

            #region Send OnEVSEDataPush event

            try
            {

                OnPushEVSEDataRequest?.Invoke(DateTime.Now,
                                              Timestamp.Value,
                                              this,
                                              this.Id.ToString(),
                                              EventTrackingId,
                                              RoamingNetwork.Id,
                                              ActionType,
                                              GroupedEVSEs,
                                              (UInt32) _NumberOfEVSEs,
                                              RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPAdapter) + "." + nameof(OnPushEVSEStatusRequest));
            }

            #endregion


            if (_NumberOfEVSEs > 0)
            {

                var response = await CPORoaming.PushEVSEData(GroupedEVSEs.
                                                                 Where       (group => group.Key != null).
                                                                 ToDictionary(group => group.Key,
                                                                              group => group.AsEnumerable()).
                                                                 SelectMany  (kvp   => kvp.Value.Select(evse => evse.AsOICPEVSEDataRecord(_EVSE2EVSEDataRecord)), Tuple.Create).
                                                                 ToLookup    (kvp   => kvp.Item1.Key,
                                                                              kvp   => kvp.Item2),
                                                             ActionType.AsOICPActionType(),
                                                             DefaultOperator,
                                                             _OperatorNameSelector ?? DefaultOperatorNameSelector,

                                                             Timestamp,
                                                             CancellationToken,
                                                             EventTrackingId,
                                                             RequestTimeout);


                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                    response.Content        != null              &&
                    response.Content.Result == true)
                {
                    result = new Acknowledgement(ResultType.True);
                }

                else
                    result = new Acknowledgement(ResultType.False,
                                                 response.Content.StatusCode.Description,
                                                 response.Content.StatusCode.AdditionalInfo);

            }

            else
                result = new Acknowledgement(ResultType.NoOperation);


            #region Send OnEVSEDataPushed event

            try
            {

                OnPushEVSEDataResponse?.Invoke(DateTime.Now,
                                               Timestamp.Value,
                                               this,
                                               this.Id.ToString(),
                                               EventTrackingId,
                                               RoamingNetwork.Id,
                                               ActionType,
                                               GroupedEVSEs,
                                               (UInt32) _NumberOfEVSEs,
                                               RequestTimeout,
                                               result,
                                               DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPAdapter) + "." + nameof(OnPushEVSEStatusResponse));
            }

            #endregion


            return result;

        }

        #endregion


        #region PushEVSEData(EVSE,             ActionType = insert, ...)

        /// <summary>
        /// Upload the EVSE data of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEData(EVSE                 EVSE,
                         WWCP.ActionType      ActionType         = WWCP.ActionType.insert,

                         DateTime?            Timestamp          = null,
                         CancellationToken?   CancellationToken  = null,
                         EventTracking_Id     EventTrackingId    = null,
                         TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            #endregion

            return await PushEVSEData(new EVSE[] { EVSE },
                                      ActionType,
                                      null,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region PushEVSEData(EVSEs,            ActionType = fullLoad, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload the EVSE data of the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEData(IEnumerable<EVSE>    EVSEs,
                         WWCP.ActionType      ActionType         = WWCP.ActionType.fullLoad,
                         IncludeEVSEDelegate  IncludeEVSEs       = null,

                         DateTime?            Timestamp          = null,
                         CancellationToken?   CancellationToken  = null,
                         EventTracking_Id     EventTrackingId    = null,
                         TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSE => true;

            #endregion

            #region Get effective number of EVSE status to upload

            var _EVSEs = EVSEs.Where(evse => evse          != null &&
                                             evse.Operator != null &&
                                             IncludeEVSEs(evse)).
                               ToArray();

            #endregion


            if (_EVSEs.Length > 0)
                return await PushEVSEData(_EVSEs.ToLookup(evse => evse.Operator,
                                                          evse => evse),
                                          ActionType,

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);

            return new Acknowledgement(ResultType.True);

        }

        #endregion

        #region PushEVSEData(ChargingStation,  ActionType = fullLoad, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload the EVSE data of the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEData(ChargingStation      ChargingStation,
                         WWCP.ActionType      ActionType         = WWCP.ActionType.fullLoad,
                         IncludeEVSEDelegate  IncludeEVSEs       = null,

                         DateTime?            Timestamp          = null,
                         CancellationToken?   CancellationToken  = null,
                         EventTracking_Id     EventTrackingId    = null,
                         TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            return await PushEVSEData(ChargingStation.EVSEs,
                                      ActionType,
                                      IncludeEVSEs,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region PushEVSEData(ChargingStations, ActionType = fullLoad, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload the EVSE data of the given charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEData(IEnumerable<ChargingStation>  ChargingStations,
                         WWCP.ActionType               ActionType         = WWCP.ActionType.fullLoad,
                         IncludeEVSEDelegate           IncludeEVSEs       = null,

                         DateTime?                     Timestamp          = null,
                         CancellationToken?            CancellationToken  = null,
                         EventTracking_Id              EventTrackingId    = null,
                         TimeSpan?                     RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return await PushEVSEData(ChargingStations.SelectMany(station => station.EVSEs),
                                      ActionType,
                                      IncludeEVSEs,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region PushEVSEData(ChargingPool,     ActionType = fullLoad, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload the EVSE data of the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEData(ChargingPool         ChargingPool,
                         WWCP.ActionType      ActionType         = WWCP.ActionType.fullLoad,
                         IncludeEVSEDelegate  IncludeEVSEs       = null,

                         DateTime?            Timestamp          = null,
                         CancellationToken?   CancellationToken  = null,
                         EventTracking_Id     EventTrackingId    = null,
                         TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            return await PushEVSEData(ChargingPool.EVSEs,
                                      ActionType,
                                      IncludeEVSEs,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region PushEVSEData(ChargingPools,    ActionType = fullLoad, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload the EVSE data of the given charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEData(IEnumerable<ChargingPool>  ChargingPools,
                         WWCP.ActionType            ActionType         = WWCP.ActionType.fullLoad,
                         IncludeEVSEDelegate        IncludeEVSEs       = null,

                         DateTime?                  Timestamp          = null,
                         CancellationToken?         CancellationToken  = null,
                         EventTracking_Id           EventTrackingId    = null,
                         TimeSpan?                  RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return await PushEVSEData(ChargingPools.SelectMany(pool    => pool.ChargingStations).
                                                    SelectMany(station => station.EVSEs),
                                      ActionType,
                                      IncludeEVSEs,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region PushEVSEData(EVSEOperator,     ActionType = fullLoad, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload the EVSE data of the given Charging Station Operator.
        /// </summary>
        /// <param name="EVSEOperator">An Charging Station Operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEData(ChargingStationOperator         EVSEOperator,
                         WWCP.ActionType      ActionType         = WWCP.ActionType.fullLoad,
                         IncludeEVSEDelegate  IncludeEVSEs       = null,

                         DateTime?            Timestamp          = null,
                         CancellationToken?   CancellationToken  = null,
                         EventTracking_Id     EventTrackingId    = null,
                         TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSEOperator == null)
                throw new ArgumentNullException(nameof(EVSEOperator), "The given Charging Station Operator must not be null!");

            #endregion

            return await PushEVSEData(new ChargingStationOperator[] { EVSEOperator },
                                      ActionType,
                                      IncludeEVSEs,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region PushEVSEData(EVSEOperators,    ActionType = fullLoad, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload the EVSE data of the given Charging Station Operators.
        /// </summary>
        /// <param name="EVSEOperators">An enumeration of Charging Station Operators.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEData(IEnumerable<ChargingStationOperator>  EVSEOperators,
                         WWCP.ActionType            ActionType         = WWCP.ActionType.fullLoad,
                         IncludeEVSEDelegate        IncludeEVSEs       = null,

                         DateTime?                  Timestamp          = null,
                         CancellationToken?         CancellationToken  = null,
                         EventTracking_Id           EventTrackingId    = null,
                         TimeSpan?                  RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSEOperators == null)
                throw new ArgumentNullException(nameof(EVSEOperators),  "The given enumeration of Charging Station Operators must not be null!");

            #endregion

            return await PushEVSEData(EVSEOperators.SelectMany(evseoperator => evseoperator.ChargingPools).
                                                    SelectMany(pool         => pool.ChargingStations).
                                                    SelectMany(station      => station.EVSEs),
                                      ActionType,
                                      IncludeEVSEs,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #region PushEVSEData(RoamingNetwork,   ActionType = fullLoad, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload the EVSE data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEData(RoamingNetwork       RoamingNetwork,
                         WWCP.ActionType      ActionType         = WWCP.ActionType.fullLoad,
                         IncludeEVSEDelegate  IncludeEVSEs       = null,

                         DateTime?            Timestamp          = null,
                         CancellationToken?   CancellationToken  = null,
                         EventTracking_Id     EventTrackingId    = null,
                         TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return await PushEVSEData(RoamingNetwork.EVSEs,
                                      ActionType,
                                      IncludeEVSEs,

                                      Timestamp,
                                      CancellationToken,
                                      EventTrackingId,
                                      RequestTimeout);

        }

        #endregion

        #endregion

        #region PushEVSEStatus...

        #region PushEVSEStatus(GroupedEVSEStatus, ActionType = update, ...)

        /// <summary>
        /// Upload the EVSE status of the given lookup of EVSE status types grouped by their Charging Station Operator.
        /// </summary>
        /// <param name="GroupedEVSEStatus">A lookup of EVSEs grouped by their Charging Station Operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEStatus(ILookup<ChargingStationOperator, EVSEStatus>  GroupedEVSEStatus,
                           WWCP.ActionType                    ActionType         = WWCP.ActionType.update,

                           DateTime?                          Timestamp          = null,
                           CancellationToken?                 CancellationToken  = null,
                           EventTracking_Id                   EventTrackingId    = null,
                           TimeSpan?                          RequestTimeout     = null)

        {

            #region Initial checks

            if (GroupedEVSEStatus == null)
                throw new ArgumentNullException(nameof(GroupedEVSEStatus), "The given lookup of EVSE status must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Get effective number of EVSE status to upload

            Acknowledgement result = null;

            var _NumberOfEVSEStatus  = GroupedEVSEStatus.
                                          Where     (group => group.Key != null).
                                          SelectMany(group => group.Where(evsestatus => evsestatus != null)).
                                          Count();

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            #endregion

            #region Send OnEVSEStatusPush event

            try
            {

                OnPushEVSEStatusRequest?.Invoke(DateTime.Now,
                                                Timestamp.Value,
                                                this,
                                                this.Id.ToString(),
                                                EventTrackingId,
                                                RoamingNetwork.Id,
                                                ActionType,
                                                GroupedEVSEStatus,
                                                (UInt32) _NumberOfEVSEStatus,
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPAdapter) + "." + nameof(OnPushEVSEStatusRequest));
            }

            #endregion


            if (_NumberOfEVSEStatus > 0)
            {

                var response = await CPORoaming.PushEVSEStatus(GroupedEVSEStatus.
                                                                   Where       (group => group.Key != null).
                                                                   ToDictionary(group => group.Key,
                                                                                group => group.AsEnumerable(). // Only send the latest EVSE status!
                                                                                               GroupBy(evsestatus      => evsestatus.Id).
                                                                                               Select (sameevseidgroup => sameevseidgroup.OrderByDescending(status => status.Timestamp).First())).
                                                                   SelectMany(kvp => kvp.Value.Select(evsestatus => new EVSEStatusRecord(evsestatus.Id, evsestatus.Status.AsOICPEVSEStatus())), Tuple.Create).
                                                                   ToLookup  (kvp => kvp.Item1.Key,
                                                                              kvp => kvp.Item2), 
                                                               ActionType.AsOICPActionType(),
                                                               DefaultOperator,
                                                               _OperatorNameSelector ?? DefaultOperatorNameSelector,

                                                               Timestamp,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               RequestTimeout);


                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                    response.Content        != null              &&
                    response.Content.Result == true)
                {
                    result = new Acknowledgement(ResultType.True);
                }

                else
                    result = new Acknowledgement(ResultType.False,
                                                 response.Content.StatusCode.Description,
                                                 response.Content.StatusCode.AdditionalInfo);

            }

            else
                result = new Acknowledgement(ResultType.NoOperation);


            #region Send OnEVSEStatusPushed event

            try
            {

                OnPushEVSEStatusResponse?.Invoke(DateTime.Now,
                                                 Timestamp.Value,
                                                 this,
                                                 this.Id.ToString(),
                                                 EventTrackingId,
                                                 RoamingNetwork.Id,
                                                 ActionType,
                                                 GroupedEVSEStatus,
                                                 (UInt32) _NumberOfEVSEStatus,
                                                 RequestTimeout,
                                                 result,
                                                 DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPAdapter) + "." + nameof(OnPushEVSEStatusResponse));
            }

            #endregion


            return result;

        }

        #endregion


        #region PushEVSEStatus(EVSEStatus,        ActionType = update, ...)

        /// <summary>
        /// Upload the given EVSE status.
        /// </summary>
        /// <param name="EVSEStatus">An EVSE status.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEStatus(EVSEStatus          EVSEStatus,
                           WWCP.ActionType     ActionType         = WWCP.ActionType.update,

                           DateTime?           Timestamp          = null,
                           CancellationToken?  CancellationToken  = null,
                           EventTracking_Id    EventTrackingId    = null,
                           TimeSpan?           RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSEStatus == null)
                throw new ArgumentNullException(nameof(EVSEStatus),  "The given EVSE status must not be null!");

            #endregion

            return await PushEVSEStatus(new EVSEStatus[] { EVSEStatus },
                                        ActionType,

                                        Timestamp,
                                        CancellationToken,
                                        EventTrackingId,
                                        RequestTimeout);

        }

        #endregion

        #region PushEVSEStatus(EVSEStatus,        ActionType = update, ...)

        /// <summary>
        /// Upload the status of the given enumeration of EVSE status.
        /// </summary>
        /// <param name="EVSEStatus">An enumeration of EVSE status.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEStatus(IEnumerable<EVSEStatus>  EVSEStatus,
                           WWCP.ActionType          ActionType         = WWCP.ActionType.update,

                           DateTime?                Timestamp          = null,
                           CancellationToken?       CancellationToken  = null,
                           EventTracking_Id         EventTrackingId    = null,
                           TimeSpan?                RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSEStatus == null)
                throw new ArgumentNullException(nameof(EVSEStatus),  "The given enumeration of EVSE status must not be null!");

            var _EVSEStatus = EVSEStatus.Where (status => status != null).
                                         Select(status => new {
                                                              Status   = status,
                                                              Operator = RoamingNetwork.GetChargingStationOperatorById(status.Id.OperatorId)
                                                          }).
                                         Where (tuple  => tuple.Operator != null).
                                         ToArray();

            #endregion


            if (_EVSEStatus.Length > 0)
                return await PushEVSEStatus(_EVSEStatus.ToLookup(evsestatus => evsestatus.Operator,
                                                                 evsestatus => evsestatus.Status),
                                            ActionType,

                                            Timestamp,
                                            CancellationToken,
                                            EventTrackingId,
                                            RequestTimeout);

            return new Acknowledgement(ResultType.True);

        }

        #endregion

        #region PushEVSEStatus(EVSE,              ActionType = update, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload the EVSE status of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEStatus(EVSE                 EVSE,
                           WWCP.ActionType      ActionType         = WWCP.ActionType.update,
                           IncludeEVSEDelegate  IncludeEVSEs       = null,

                           DateTime?            Timestamp          = null,
                           CancellationToken?   CancellationToken  = null,
                           EventTracking_Id     EventTrackingId    = null,
                           TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE),  "The given charging station must not be null!");

            #endregion

            if (IncludeEVSEs != null && !IncludeEVSEs(EVSE))
                return new Acknowledgement(ResultType.NoOperation);

            return await PushEVSEStatus(EVSEStatus.Snapshot(EVSE),
                                        ActionType,

                                        Timestamp,
                                        CancellationToken,
                                        EventTrackingId,
                                        RequestTimeout);

        }

        #endregion

        #region PushEVSEStatus(EVSEs,             ActionType = update, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload all EVSE status of the given enumeration of charging stations.
        /// </summary>
        /// <param name="EVSEs">An enumeration of charging stations.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEStatus(IEnumerable<EVSE>    EVSEs,
                           WWCP.ActionType      ActionType         = WWCP.ActionType.update,
                           IncludeEVSEDelegate  IncludeEVSEs       = null,

                           DateTime?            Timestamp          = null,
                           CancellationToken?   CancellationToken  = null,
                           EventTracking_Id     EventTrackingId    = null,
                           TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs),  "The given enumeration of charging stations must not be null!");

            var _EVSEs = IncludeEVSEs != null
                             ? EVSEs.Where(evse => IncludeEVSEs(evse)).ToArray()
                             : EVSEs.ToArray();

            #endregion

            if (_EVSEs.Length > 0)
                return await PushEVSEStatus(EVSEs.Select(evse => EVSEStatus.Snapshot(evse)),
                                            ActionType,

                                            Timestamp,
                                            CancellationToken,
                                            EventTrackingId,
                                            RequestTimeout);

            return new Acknowledgement(ResultType.NoOperation);

        }

        #endregion

        #region PushEVSEStatus(ChargingStation,   ActionType = update, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload all EVSE status of the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEStatus(ChargingStation      ChargingStation,
                           WWCP.ActionType      ActionType         = WWCP.ActionType.update,
                           IncludeEVSEDelegate  IncludeEVSEs       = null,

                           DateTime?            Timestamp          = null,
                           CancellationToken?   CancellationToken  = null,
                           EventTracking_Id     EventTrackingId    = null,
                           TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation),  "The given charging station must not be null!");

            #endregion

            return await PushEVSEStatus((IncludeEVSEs != null
                                             ? ChargingStation.EVSEs.Where(evse => IncludeEVSEs(evse))
                                             : ChargingStation.EVSEs).
                                             Select(evse => EVSEStatus.Snapshot(evse)),
                                        ActionType,

                                        Timestamp,
                                        CancellationToken,
                                        EventTrackingId,
                                        RequestTimeout);

        }

        #endregion

        #region PushEVSEStatus(ChargingStations,  ActionType = update, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload all EVSE status of the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEStatus(IEnumerable<ChargingStation>  ChargingStations,
                           WWCP.ActionType               ActionType         = WWCP.ActionType.update,
                           IncludeEVSEDelegate           IncludeEVSEs       = null,

                           DateTime?                     Timestamp          = null,
                           CancellationToken?            CancellationToken  = null,
                           EventTracking_Id              EventTrackingId    = null,
                           TimeSpan?                     RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations),  "The given enumeration of charging stations must not be null!");

            #endregion

            return await PushEVSEStatus((IncludeEVSEs != null
                                            ? ChargingStations.SelectMany(station => station.EVSEs.Where(evse => IncludeEVSEs(evse)))
                                            : ChargingStations.SelectMany(station => station.EVSEs)).
                                            Select(evse => EVSEStatus.Snapshot(evse)),
                                        ActionType,

                                        Timestamp,
                                        CancellationToken,
                                        EventTrackingId,
                                        RequestTimeout);

        }

        #endregion

        #region PushEVSEStatus(ChargingPool,      ActionType = update, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload all EVSE status of the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEStatus(ChargingPool         ChargingPool,
                           WWCP.ActionType      ActionType         = WWCP.ActionType.update,
                           IncludeEVSEDelegate  IncludeEVSEs       = null,

                           DateTime?            Timestamp          = null,
                           CancellationToken?   CancellationToken  = null,
                           EventTracking_Id     EventTrackingId    = null,
                           TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool),  "The given charging pool must not be null!");

            #endregion

            return await PushEVSEStatus((IncludeEVSEs != null
                                            ? ChargingPool.EVSEs.Where(evse => IncludeEVSEs(evse))
                                            : ChargingPool.EVSEs).
                                            Select(evse => EVSEStatus.Snapshot(evse)),
                                        ActionType,

                                        Timestamp,
                                        CancellationToken,
                                        EventTrackingId,
                                        RequestTimeout);

        }

        #endregion

        #region PushEVSEStatus(ChargingPools,     ActionType = update, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload all EVSE status of the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEStatus(IEnumerable<ChargingPool>  ChargingPools,
                           WWCP.ActionType            ActionType         = WWCP.ActionType.update,
                           IncludeEVSEDelegate        IncludeEVSEs       = null,

                           DateTime?                  Timestamp          = null,
                           CancellationToken?         CancellationToken  = null,
                           EventTracking_Id           EventTrackingId    = null,
                           TimeSpan?                  RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools),  "The given enumeration of charging pools must not be null!");

            #endregion

            return await PushEVSEStatus((IncludeEVSEs != null
                                            ? ChargingPools.SelectMany(pool    => pool.   ChargingStations).
                                                            SelectMany(station => station.EVSEs.
                                                                                          Where(evse => IncludeEVSEs(evse)))
                                            : ChargingPools.SelectMany(pool    => pool.   ChargingStations).
                                                            SelectMany(station => station.EVSEs)).
                                            Select(evse => EVSEStatus.Snapshot(evse)),
                                        ActionType,

                                        Timestamp,
                                        CancellationToken,
                                        EventTrackingId,
                                        RequestTimeout);

        }

        #endregion

        #region PushEVSEStatus(EVSEOperator,      ActionType = update, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload all EVSE status of the given Charging Station Operator.
        /// </summary>
        /// <param name="EVSEOperator">An Charging Station Operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEStatus(ChargingStationOperator         EVSEOperator,
                           WWCP.ActionType      ActionType         = WWCP.ActionType.update,
                           IncludeEVSEDelegate  IncludeEVSEs       = null,

                           DateTime?            Timestamp          = null,
                           CancellationToken?   CancellationToken  = null,
                           EventTracking_Id     EventTrackingId    = null,
                           TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSEOperator == null)
                throw new ArgumentNullException(nameof(EVSEOperator),  "The given Charging Station Operator must not be null!");

            #endregion

            return await PushEVSEStatus((IncludeEVSEs != null
                                            ? EVSEOperator.EVSEs.Where(evse => IncludeEVSEs(evse))
                                            : EVSEOperator.EVSEs).
                                            Select(evse => EVSEStatus.Snapshot(evse)),
                                        ActionType,

                                        Timestamp,
                                        CancellationToken,
                                        EventTrackingId,
                                        RequestTimeout);

        }

        #endregion

        #region PushEVSEStatus(EVSEOperators,     ActionType = update, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload all EVSE status of the given enumeration of Charging Station Operators.
        /// </summary>
        /// <param name="EVSEOperators">An enumeration of EVSES operators.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEStatus(IEnumerable<ChargingStationOperator>  EVSEOperators,
                           WWCP.ActionType            ActionType         = WWCP.ActionType.update,
                           IncludeEVSEDelegate        IncludeEVSEs       = null,

                           DateTime?                  Timestamp          = null,
                           CancellationToken?         CancellationToken  = null,
                           EventTracking_Id           EventTrackingId    = null,
                           TimeSpan?                  RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSEOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator),  "The given enumeration of Charging Station Operators must not be null!");

            #endregion

            return await PushEVSEStatus((IncludeEVSEs != null
                                            ? EVSEOperators.SelectMany(evseoperator => evseoperator.ChargingPools).
                                                            SelectMany(pool         => pool.        ChargingStations).
                                                            SelectMany(station      => station.     EVSEs.
                                                                                                    Where(evse => IncludeEVSEs(evse)))
                                            : EVSEOperators.SelectMany(evseoperator => evseoperator.ChargingPools).
                                                            SelectMany(pool         => pool.        ChargingStations).
                                                            SelectMany(station      => station.     EVSEs)).
                                            Select(evse => EVSEStatus.Snapshot(evse)),
                                        ActionType,

                                        Timestamp,
                                        CancellationToken,
                                        EventTrackingId,
                                        RequestTimeout);

        }

        #endregion

        #region PushEVSEStatus(RoamingNetwork,    ActionType = update, IncludeEVSEs = null, ...)

        /// <summary>
        /// Upload all EVSE status of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<Acknowledgement>

            PushEVSEStatus(RoamingNetwork       RoamingNetwork,
                           WWCP.ActionType      ActionType         = WWCP.ActionType.update,
                           IncludeEVSEDelegate  IncludeEVSEs       = null,

                           DateTime?            Timestamp          = null,
                           CancellationToken?   CancellationToken  = null,
                           EventTracking_Id     EventTrackingId    = null,
                           TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return await PushEVSEStatus((IncludeEVSEs != null
                                            ? RoamingNetwork.EVSEs.Where(evse => IncludeEVSEs(evse))
                                            : RoamingNetwork.EVSEs).
                                            Select(evse => EVSEStatus.Snapshot(evse)),
                                        ActionType,

                                        Timestamp,
                                        CancellationToken,
                                        EventTrackingId,
                                        RequestTimeout);

        }

        #endregion


        #region PushEVSEStatus(EVSEStatusDiff, ...)

        ///// <summary>
        ///// Send EVSE status updates.
        ///// </summary>
        ///// <param name="EVSEStatusDiff">An EVSE status diff.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //public override async Task

        //    PushEVSEStatus(EVSEStatusDiff      EVSEStatusDiff,

        //                   DateTime?           Timestamp          = null,
        //                   CancellationToken?  CancellationToken  = null,
        //                   EventTracking_Id    EventTrackingId    = null,
        //                   TimeSpan?           RequestTimeout     = null)

        //{

        //    if (EVSEStatusDiff == null)
        //        return;

        //    var TrackingId = Guid.NewGuid().ToString();

        //    #region Insert new EVSEs...

        //    if (EVSEStatusDiff.NewStatus.Count() > 0)
        //    {

        //        var NewEVSEStatus = EVSEStatusDiff.
        //                                NewStatus.
        //                                Select(v => new EVSEStatus(v.Key, v.Value, )).
        //                                ToArray();

        //        //OnNewEVSEStatusSending?.Invoke(DateTime.Now,
        //        //                               NewEVSEStatus,
        //        //                               _HTTPVirtualHost,
        //        //                               TrackingId);

        //        var result = await CPORoaming.PushEVSEStatus(NewEVSEStatus,
        //                                                     ActionType.insert,
        //                                                     EVSEStatusDiff.EVSEOperatorId,
        //                                                     null,

        //                                                     Timestamp,
        //                                                     CancellationToken,
        //                                                     EventTrackingId,
        //                                                     RequestTimeout);

        //    }

        //    #endregion

        //    #region Upload EVSE changes...

        //    if (EVSEStatusDiff.ChangedStatus.Count() > 0)
        //    {

        //        var ChangedEVSEStatus = EVSEStatusDiff.
        //                                    ChangedStatus.
        //                                    Select(v => new EVSEStatusRecord(v.Key, v.Value.AsOICPEVSEStatus())).
        //                                    ToArray();

        //        //OnChangedEVSEStatusSending?.Invoke(DateTime.Now,
        //        //                                   ChangedEVSEStatus,
        //        //                                   _HTTPVirtualHost,
        //        //                                   TrackingId);

        //        var result = await CPORoaming.PushEVSEStatus(ChangedEVSEStatus,
        //                                                      ActionType.update,
        //                                                      EVSEStatusDiff.EVSEOperatorId,
        //                                                      null,

        //                                                      Timestamp,
        //                                                      CancellationToken,
        //                                                      EventTrackingId,
        //                                                      RequestTimeout);

        //    }

        //    #endregion

        //    #region Remove outdated EVSEs...

        //    if (EVSEStatusDiff.RemovedIds.Count() > 0)
        //    {

        //        var RemovedEVSEStatus = EVSEStatusDiff.
        //                                    RemovedIds.
        //                                    ToArray();

        //        //OnRemovedEVSEStatusSending?.Invoke(DateTime.Now,
        //        //                                   RemovedEVSEStatus,
        //        //                                   _HTTPVirtualHost,
        //        //                                   TrackingId);

        //        var result = await CPORoaming.PushEVSEStatus(RemovedEVSEStatus.Select(EVSEId => new EVSEStatusRecord(EVSEId, EVSEStatusType.OutOfService)),
        //                                                      ActionType.delete,
        //                                                      EVSEStatusDiff.EVSEOperatorId,
        //                                                      null,

        //                                                      Timestamp,
        //                                                      CancellationToken,
        //                                                      EventTrackingId,
        //                                                      RequestTimeout);

        //    }

        //    #endregion

        //}

        #endregion

        #endregion


        #region AuthorizeStart(...OperatorId, AuthToken, ChargingProductId = null, SessionId = null, ...)

        /// <summary>
        /// Create an OICP AuthorizeStart request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An Charging Station Operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<AuthStartResult>

            AuthorizeStart(ChargingStationOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           ChargingProduct_Id  ChargingProductId  = null,
                           ChargingSession_Id  SessionId          = null,

                           DateTime?           Timestamp          = null,
                           CancellationToken?  CancellationToken  = null,
                           EventTracking_Id    EventTrackingId    = null,
                           TimeSpan?           RequestTimeout     = null)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The given Charging Station Operator identification must not be null!");

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken),   "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStart event

            try
            {

                OnAuthorizeStart?.Invoke(DateTime.Now,
                                         this,
                                         EventTrackingId,
                                         RoamingNetwork.Id,
                                         OperatorId,
                                         AuthToken,
                                         ChargingProductId,
                                         SessionId,
                                         RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPAdapter) + "." + nameof(OnAuthorizeStart));
            }

            #endregion


            var response = await CPORoaming.AuthorizeStart(OperatorId,
                                                           AuthToken,
                                                           null,
                                                           SessionId,
                                                           ChargingProductId,
                                                           null,

                                                           Timestamp,
                                                           CancellationToken,
                                                           EventTrackingId,
                                                           RequestTimeout);


            var Now     = DateTime.Now;
            var Runtime = Now - Timestamp.Value;

            AuthStartResult result = null;

            if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                response.Content                     != null              &&
                response.Content.AuthorizationStatus == AuthorizationStatusType.Authorized)
            {

                result = AuthStartResult.Authorized(AuthorizatorId,
                                                    response.Content.SessionId,
                                                    response.Content.ProviderId,
                                                    response.Content.StatusCode.Description,
                                                    response.Content.StatusCode.AdditionalInfo,
                                                    Runtime);

            }

            else
                result = AuthStartResult.NotAuthorized(AuthorizatorId,
                                                       response.Content.ProviderId,
                                                       response.Content.StatusCode.Description,
                                                       response.Content.StatusCode.AdditionalInfo,
                                                       Runtime);


            #region Send OnAuthorizeStarted event

            try
            {

                OnAuthorizeStarted?.Invoke(Now,
                                           this,
                                           EventTrackingId,
                                           RoamingNetwork.Id,
                                           OperatorId,
                                           AuthToken,
                                           ChargingProductId,
                                           SessionId,
                                           RequestTimeout,
                                           result,
                                           Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPAdapter) + "." + nameof(OnAuthorizeStarted));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(...OperatorId, AuthToken, EVSEId, ChargingProductId = null, SessionId = null, ...)

        /// <summary>
        /// Create an OICP AuthorizeStart request at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An Charging Station Operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<AuthStartEVSEResult>

            AuthorizeStart(ChargingStationOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           EVSE_Id             EVSEId,
                           ChargingProduct_Id  ChargingProductId  = null,   // [maxlength: 100]
                           ChargingSession_Id  SessionId          = null,

                           DateTime?           Timestamp          = null,
                           CancellationToken?  CancellationToken  = null,
                           EventTracking_Id    EventTrackingId    = null,
                           TimeSpan?           RequestTimeout     = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId), "The given Charging Station Operator identification must not be null!");

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken),  "The given authentication token must not be null!");

            if (EVSEId    == null)
                throw new ArgumentNullException(nameof(EVSEId),     "The given EVSE identification must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeEVSEStart event

            try
            {

                OnAuthorizeEVSEStart?.Invoke(DateTime.Now,
                                             this,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             OperatorId,
                                             AuthToken,
                                             EVSEId,
                                             ChargingProductId,
                                             SessionId,
                                             RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPAdapter) + "." + nameof(OnAuthorizeEVSEStart));
            }

            #endregion


            var response  = await CPORoaming.AuthorizeStart(OperatorId,
                                                            AuthToken,
                                                            EVSEId,
                                                            SessionId,
                                                            ChargingProductId,
                                                            null,

                                                            Timestamp,
                                                            CancellationToken,
                                                            EventTrackingId,
                                                            RequestTimeout);


            var Now     = DateTime.Now;
            var Runtime = Now - Timestamp.Value;

            AuthStartEVSEResult result = null;

            if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                response.Content                     != null              &&
                response.Content.AuthorizationStatus == AuthorizationStatusType.Authorized)
            {

                result = AuthStartEVSEResult.Authorized(AuthorizatorId,
                                                        response.Content.SessionId,
                                                        response.Content.ProviderId,
                                                        response.Content.StatusCode.Description,
                                                        response.Content.StatusCode.AdditionalInfo,
                                                        Runtime);

            }

            else
                result = AuthStartEVSEResult.NotAuthorized(AuthorizatorId,
                                                           response.Content.ProviderId,
                                                           response.Content.StatusCode.Description,
                                                           response.Content.StatusCode.AdditionalInfo,
                                                           Runtime);


            #region Send OnAuthorizeEVSEStarted event

            try
            {

                OnAuthorizeEVSEStarted?.Invoke(DateTime.Now,
                                               this,
                                               EventTrackingId,
                                               RoamingNetwork.Id,
                                               OperatorId,
                                               AuthToken,
                                               EVSEId,
                                               ChargingProductId,
                                               SessionId,
                                               RequestTimeout,
                                               result,
                                               DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPAdapter) + "." + nameof(OnAuthorizeEVSEStarted));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(...OperatorId, AuthToken, ChargingStationId, ChargingProductId = null, SessionId = null, ...)

        /// <summary>
        /// Create an OICP AuthorizeStart request at the given charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An Charging Station Operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override Task<AuthStartChargingStationResult>

            AuthorizeStart(ChargingStationOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           ChargingStation_Id  ChargingStationId,
                           ChargingProduct_Id  ChargingProductId  = null,   // [maxlength: 100]
                           ChargingSession_Id  SessionId          = null,

                           DateTime?           Timestamp          = null,
                           CancellationToken?  CancellationToken  = null,
                           EventTracking_Id    EventTrackingId    = null,
                           TimeSpan?           RequestTimeout     = null)

        {

            #region Initial checks

            if (OperatorId        == null)
                throw new ArgumentNullException(nameof(OperatorId),         "The given Charging Station Operator identification must not be null!");

            if (AuthToken         == null)
                throw new ArgumentNullException(nameof(AuthToken),          "The given authentication token must not be null!");

            if (ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId),  "The given charging station identification must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingStationStart event

            try
            {

                OnAuthorizeChargingStationStart?.Invoke(DateTime.Now,
                                                        this,
                                                        EventTrackingId,
                                                        RoamingNetwork.Id,
                                                        OperatorId,
                                                        AuthToken,
                                                        ChargingStationId,
                                                        ChargingProductId,
                                                        SessionId,
                                                        RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPAdapter) + "." + nameof(OnAuthorizeChargingStationStart));
            }

            #endregion


            //ToDo: Implement AuthorizeStart(...ChargingStationId...)
            var result = AuthStartChargingStationResult.Error(AuthorizatorId, "Not implemented!");


            #region Send OnAuthorizeChargingStationStarted event

            try
            {

                OnAuthorizeChargingStationStarted?.Invoke(DateTime.Now,
                                                          this,
                                                          EventTrackingId,
                                                          RoamingNetwork.Id,
                                                          OperatorId,
                                                          AuthToken,
                                                          ChargingStationId,
                                                          ChargingProductId,
                                                          SessionId,
                                                          RequestTimeout,
                                                          result,
                                                          DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPAdapter) + "." + nameof(OnAuthorizeChargingStationStarted));
            }

            #endregion

            return Task.FromResult(result);

        }

        #endregion


        #region AuthorizeStop(...OperatorId, SessionId, AuthToken, ...)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an OICP AuthorizeStop request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<AuthStopResult>

            AuthorizeStop(ChargingStationOperator_Id     OperatorId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,

                          DateTime?           Timestamp          = null,
                          CancellationToken?  CancellationToken  = null,
                          EventTracking_Id    EventTrackingId    = null,
                          TimeSpan?           RequestTimeout     = null)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The given Charging Station Operator identification must not be null!");

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),   "The given charging session identification must not be null!");

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken),   "The given auth token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStop event

            try
            {

                OnAuthorizeStop?.Invoke(DateTime.Now,
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
                e.Log(nameof(WWCPAdapter) + "." + nameof(OnAuthorizeStop));
            }

            #endregion


            var response = await CPORoaming.AuthorizeStop(OperatorId,
                                                          SessionId,
                                                          AuthToken,
                                                          null,
                                                          null,

                                                          Timestamp,
                                                          CancellationToken,
                                                          EventTrackingId,
                                                          RequestTimeout);


            AuthStopResult result = null;

            if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                response.Content                     != null              &&
                response.Content.AuthorizationStatus == AuthorizationStatusType.Authorized)
            {

                result = AuthStopResult.Authorized(AuthorizatorId,
                                                   response.Content?.ProviderId,
                                                   response.Content?.StatusCode?.Description,
                                                   response.Content?.StatusCode?.AdditionalInfo);

            }
            else
                result = AuthStopResult.NotAuthorized(AuthorizatorId,
                                                      response.Content?.ProviderId,
                                                      response.Content?.StatusCode?.Description,
                                                      response.Content?.StatusCode?.AdditionalInfo);


            #region Send OnAuthorizeStopped event

            try
            {

                OnAuthorizeStopped?.Invoke(DateTime.Now,
                                           this,
                                           EventTrackingId,
                                           RoamingNetwork.Id,
                                           OperatorId,
                                           SessionId,
                                           AuthToken,
                                           RequestTimeout,
                                           result);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPAdapter) + "." + nameof(OnAuthorizeStopped));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(...OperatorId, EVSEId, SessionId, AuthToken, ...)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an OICP AuthorizeStop request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<AuthStopEVSEResult>

            AuthorizeStop(ChargingStationOperator_Id     OperatorId,
                          EVSE_Id             EVSEId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,

                          DateTime?           Timestamp          = null,
                          CancellationToken?  CancellationToken  = null,
                          EventTracking_Id    EventTrackingId    = null,
                          TimeSpan?           RequestTimeout     = null)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The given Charging Station Operator identification must not be null!");

            if (EVSEId     == null)
                throw new ArgumentNullException(nameof(EVSEId),      "The given EVSE identification must not be null!");

            if (SessionId  == null)
                throw new ArgumentNullException(nameof(SessionId),   "The given charging session identification must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException(nameof(AuthToken),   "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeEVSEStop event

            try
            {

                OnAuthorizeEVSEStop?.Invoke(DateTime.Now,
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
                e.Log(nameof(WWCPAdapter) + "." + nameof(OnAuthorizeEVSEStop));
            }

            #endregion


            var response  = await CPORoaming.AuthorizeStop(OperatorId,
                                                           SessionId,
                                                           AuthToken,
                                                           EVSEId,
                                                           null,

                                                           Timestamp,
                                                           CancellationToken,
                                                           EventTrackingId,
                                                           RequestTimeout);

            AuthStopEVSEResult result = null;

            if (response.HTTPStatusCode              == HTTPStatusCode.OK &&
                response.Content                     != null              &&
                response.Content.AuthorizationStatus == AuthorizationStatusType.Authorized)
            {

                result = AuthStopEVSEResult.Authorized(AuthorizatorId,
                                                       response.Content?.ProviderId,
                                                       response.Content?.StatusCode?.Description,
                                                       response.Content?.StatusCode?.AdditionalInfo);

            }
            else
                result = AuthStopEVSEResult.NotAuthorized(AuthorizatorId,
                                                          response.Content?.ProviderId,
                                                          response.Content?.StatusCode?.Description,
                                                          response.Content?.StatusCode?.AdditionalInfo);


            #region Send OnAuthorizeEVSEStopped event

            try
            {

                OnAuthorizeEVSEStopped?.Invoke(DateTime.Now,
                                               this,
                                               EventTrackingId,
                                               RoamingNetwork.Id,
                                               OperatorId,
                                               EVSEId,
                                               SessionId,
                                               AuthToken,
                                               RequestTimeout,
                                               result);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPAdapter) + "." + nameof(OnAuthorizeEVSEStopped));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(...OperatorId, ChargingStationId, SessionId, AuthToken, ...)

        /// <summary>
        /// Create an OICP AuthorizeStop request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An Charging Station Operator identification.</param>
        /// <param name="ChargingStationId">A charging station identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override Task<AuthStopChargingStationResult>

            AuthorizeStop(ChargingStationOperator_Id     OperatorId,
                          ChargingStation_Id  ChargingStationId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,

                          DateTime?           Timestamp          = null,
                          CancellationToken?  CancellationToken  = null,
                          EventTracking_Id    EventTrackingId    = null,
                          TimeSpan?           RequestTimeout     = null)

        {

            #region Initial checks

            if (OperatorId         == null)
                throw new ArgumentNullException(nameof(OperatorId),         "The given Charging Station Operator identification must not be null!");

            if (ChargingStationId  == null)
                throw new ArgumentNullException(nameof(ChargingStationId),  "The given EVSE identification must not be null!");

            if (SessionId          == null)
                throw new ArgumentNullException(nameof(SessionId),          "The given charging session identification must not be null!");

            if (AuthToken          == null)
                throw new ArgumentNullException(nameof(AuthToken),          "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingStationStop event

            try
            {

                OnAuthorizeChargingStationStop?.Invoke(DateTime.Now,
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
                e.Log(nameof(WWCPAdapter) + "." + nameof(OnAuthorizeChargingStationStop));
            }

            #endregion


            var result = AuthStopChargingStationResult.Error(AuthorizatorId,
                                                             "OICP does not support this request!");


            #region Send OnAuthorizeChargingStationStopped event

            try
            {

                OnAuthorizeChargingStationStopped?.Invoke(DateTime.Now,
                                                          this,
                                                          EventTrackingId,
                                                          RoamingNetwork.Id,
                                                          OperatorId,
                                                          ChargingStationId,
                                                          SessionId,
                                                          AuthToken,
                                                          RequestTimeout,
                                                          result);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPAdapter) + "." + nameof(OnAuthorizeChargingStationStopped));
            }

            #endregion

            return Task.FromResult(result);

        }

        #endregion


        #region SendChargeDetailRecord(ChargeDetailRecord, ...)

        /// <summary>
        /// Send a charge detail record to an OICP server.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<SendCDRResult>

            SendChargeDetailRecord(WWCP.ChargeDetailRecord  ChargeDetailRecord,

                                   DateTime?                Timestamp          = null,
                                   CancellationToken?       CancellationToken  = null,
                                   EventTracking_Id         EventTrackingId    = null,
                                   TimeSpan?                RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord),  "The given charge detail record must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            var response = await CPORoaming.SendChargeDetailRecord(new ChargeDetailRecord(
                                                                       ChargeDetailRecord.EVSEId,
                                                                       ChargeDetailRecord.SessionId,
                                                                       ChargeDetailRecord.SessionTime.Value.StartTime,
                                                                       ChargeDetailRecord.SessionTime.Value.EndTime.Value,
                                                                       new AuthorizationIdentification(ChargeDetailRecord.IdentificationStart),
                                                                       ChargeDetailRecord.ChargingProductId,
                                                                       null, // PartnerSessionId
                                                                       ChargeDetailRecord.SessionTime.HasValue ? new DateTime?(ChargeDetailRecord.SessionTime.Value.StartTime) : null,
                                                                       ChargeDetailRecord.SessionTime.HasValue ? ChargeDetailRecord.SessionTime.Value.EndTime : null,
                                                                       ChargeDetailRecord.EnergyMeteringValues != null && ChargeDetailRecord.EnergyMeteringValues.Any() ? new Double?(ChargeDetailRecord.EnergyMeteringValues.First().Value) : null,
                                                                       ChargeDetailRecord.EnergyMeteringValues != null && ChargeDetailRecord.EnergyMeteringValues.Any() ? new Double?(ChargeDetailRecord.EnergyMeteringValues.Last().Value) : null,
                                                                       ChargeDetailRecord.EnergyMeteringValues != null && ChargeDetailRecord.EnergyMeteringValues.Any() ? ChargeDetailRecord.EnergyMeteringValues.Select((Timestamped<double> v) => v.Value) : null,
                                                                       ChargeDetailRecord.ConsumedEnergy,
                                                                       ChargeDetailRecord.MeteringSignature),

                                                                   Timestamp,
                                                                   CancellationToken,
                                                                   EventTrackingId,
                                                                   RequestTimeout);


            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        != null              &&
                response.Content.Result)
            {

                return SendCDRResult.Forwarded(AuthorizatorId);

            }

            return SendCDRResult.NotForwared(AuthorizatorId);

        }

        #endregion


        public override void

            RemoveChargingStations(DateTime                      Timestamp,
                                   IEnumerable<ChargingStation>  ChargingStations)

        {

            foreach (var _ChargingStation in ChargingStations)
                foreach (var _EVSE in _ChargingStation)
                    Console.WriteLine(DateTime.Now + " WWCPAdapter says: " + _EVSE.Id + " was removed!");

        }

    }

}
