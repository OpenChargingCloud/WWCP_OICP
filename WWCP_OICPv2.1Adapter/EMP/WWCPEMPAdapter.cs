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
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// A WWCP wrapper for the OICP EMP Roaming client which maps
    /// WWCP data structures onto OICP data structures and vice versa.
    /// </summary>
    public class WWCPEMPAdapter : ABaseEMobilityEntity<EMPRoamingProvider_Id>,
                                  IEMPRoamingProvider
    {

        #region Data

        private readonly EVSEDataRecord2EVSEDelegate _EVSEDataRecord2EVSE;

        /// <summary>
        ///  The default reservation time.
        /// </summary>
        public static readonly TimeSpan DefaultReservationTime = TimeSpan.FromMinutes(15);

        #endregion

        #region Properties

        /// <summary>
        /// The wrapped EMP roaming object.
        /// </summary>
        public EMPRoaming EMPRoaming { get; }


        /// <summary>
        /// The EMP client.
        /// </summary>
        public EMPClient EMPClient
            => EMPRoaming?.EMPClient;

        /// <summary>
        /// The EMP client logger.
        /// </summary>
        public EMPClient.EMPClientLogger ClientLogger
            => EMPRoaming?.EMPClient?.Logger;


        /// <summary>
        /// The EMP server.
        /// </summary>
        public EMPServer EMPServer
            => EMPRoaming?.EMPServer;

        /// <summary>
        /// The EMP server logger.
        /// </summary>
        public EMPServerLogger ServerLogger
            => EMPRoaming?.EMPServerLogger;


        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        public DNSClient DNSClient
            => EMPRoaming?.DNSClient;

        EMPRoamingProvider_Id IEMPRoamingProvider.Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        I18NString IEMPRoamingProvider.Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        RoamingNetwork IEMPRoamingProvider.RoamingNetwork
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Events

        // Client methods (logging)

        #region OnReserveEVSERequest/-Response

        /// <summary>
        /// An event sent whenever a reserve EVSE command will be send.
        /// </summary>
        public event OnReserveEVSERequestDelegate         OnReserveEVSERequest;

        /// <summary>
        /// An event sent whenever a reserve EVSE command was sent.
        /// </summary>
        public event OnReserveEVSEResponseDelegate        OnReserveEVSEResponse;

        #endregion

        #region OnCancelReservationRequest/-Response

        /// <summary>
        /// An event sent whenever a cancel reservation command will be send.
        /// </summary>
        public event OnCancelReservationRequestDelegate   OnCancelReservationRequest;

        /// <summary>
        /// An event sent whenever a cancel reservation command was sent.
        /// </summary>
        public event OnCancelReservationResponseDelegate  OnCancelReservationResponse;

        #endregion

        #region OnRemoteStartEVSERequest/-Response

        /// <summary>
        /// An event sent whenever a remote start EVSE command will be send.
        /// </summary>
        public event OnRemoteStartEVSERequestDelegate     OnRemoteStartEVSERequest;

        /// <summary>
        /// An event sent whenever a remote start EVSE command was sent.
        /// </summary>
        public event OnRemoteStartEVSEResponseDelegate    OnRemoteStartEVSEResponse;

        #endregion

        #region OnRemoteStopEVSERequest/-Response

        /// <summary>
        /// An event sent whenever a remote stop EVSE command will be send.
        /// </summary>
        public event OnRemoteStopEVSERequestDelegate      OnRemoteStopEVSERequest;

        /// <summary>
        /// An event sent whenever a remote stop EVSE command was sent.
        /// </summary>
        public event OnRemoteStopEVSEResponseDelegate     OnRemoteStopEVSEResponse;

        #endregion


        // EMPServer methods

        #region OnAuthorizeStart

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event RequestLogHandler OnLogAuthorizeStart
        {

            add
            {
                EMPRoaming.OnLogAuthorizeStart += value;
            }

            remove
            {
                EMPRoaming.OnLogAuthorizeStart -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize stop response was sent.
        /// </summary>
        public event AccessLogHandler OnLogAuthorizeStarted
        {

            add
            {
                EMPRoaming.OnLogAuthorizeStarted += value;
            }

            remove
            {
                EMPRoaming.OnLogAuthorizeStarted -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event OnAuthorizeStartEVSEDelegate OnAuthorizeStartEVSE;

        #endregion

        #region OnAuthorizeStop

        /// <summary>
        /// An event sent whenever a authorize stop command was received.
        /// </summary>
        public event RequestLogHandler OnLogAuthorizeStop
        {

            add
            {
                EMPRoaming.OnLogAuthorizeStop += value;
            }

            remove
            {
                EMPRoaming.OnLogAuthorizeStop -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize stop response was sent.
        /// </summary>
        public event AccessLogHandler OnLogAuthorizeStopped
        {

            add
            {
                EMPRoaming.OnLogAuthorizeStopped += value;
            }

            remove
            {
                EMPRoaming.OnLogAuthorizeStopped -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize stop command was received.
        /// </summary>
        public event OnAuthorizeStopEVSEDelegate OnAuthorizeStopEVSE;

        #endregion

        #region OnChargeDetailRecord

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        public event RequestLogHandler OnLogChargeDetailRecordSend
        {

            add
            {
                EMPRoaming.OnLogChargeDetailRecordSend += value;
            }

            remove
            {
                EMPRoaming.OnLogChargeDetailRecordSend -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a charge detail record response was sent.
        /// </summary>
        public event AccessLogHandler OnLogChargeDetailRecordSent
        {

            add
            {
                EMPRoaming.OnLogChargeDetailRecordSent += value;
            }

            remove
            {
                EMPRoaming.OnLogChargeDetailRecordSent -= value;
            }

        }

        event OnReserveEVSERequestDelegate IEMPRoamingProvider.OnReserveEVSERequest
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event OnReserveEVSEResponseDelegate IEMPRoamingProvider.OnReserveEVSEResponse
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event OnCancelReservationRequestDelegate IEMPRoamingProvider.OnCancelReservationRequest
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event OnCancelReservationResponseDelegate IEMPRoamingProvider.OnCancelReservationResponse
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event OnRemoteStartEVSERequestDelegate IEMPRoamingProvider.OnRemoteStartEVSERequest
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event OnRemoteStartEVSEResponseDelegate IEMPRoamingProvider.OnRemoteStartEVSEResponse
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event OnRemoteStopEVSERequestDelegate IEMPRoamingProvider.OnRemoteStopEVSERequest
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event OnRemoteStopEVSEResponseDelegate IEMPRoamingProvider.OnRemoteStopEVSEResponse
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event OnAuthorizeStartEVSEDelegate IEMPRoamingProvider.OnAuthorizeStartEVSE
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event OnAuthorizeStopEVSEDelegate IEMPRoamingProvider.OnAuthorizeStopEVSE
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event WWCP.OnChargeDetailRecordDelegate IEMPRoamingProvider.OnChargeDetailRecord
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        public event WWCP.OnChargeDetailRecordDelegate OnChargeDetailRecord;

        #endregion

        #endregion

        #region Constructor(s)

        #region WWCPEMPAdapter(Id, Name, RoamingNetwork, EMPRoaming, EVSEDataRecord2EVSE = null)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP EMP Roaming client for e-mobility providers/EMPs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="EMPRoaming">A OICP EMP roaming object to be mapped to WWCP.</param>
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record after receiving it from the roaming provider.</param>
        public WWCPEMPAdapter(EMPRoamingProvider_Id        Id,
                              I18NString                   Name,
                              RoamingNetwork               RoamingNetwork,

                              EMPRoaming                   EMPRoaming,
                              EVSEDataRecord2EVSEDelegate  EVSEDataRecord2EVSE   = null)

            : base(Id,
                   RoamingNetwork)

        {

            #region Initial checks

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),        "The given roaming provider name must not be null or empty!");

            if (EMPRoaming     == null)
                throw new ArgumentNullException(nameof(EMPRoaming),  "The given OICP EMP Roaming object must not be null!");

            #endregion

            this.EMPRoaming            = EMPRoaming;
            this._EVSEDataRecord2EVSE  = EVSEDataRecord2EVSE;

            // Link events...

            #region OnAuthorizeStart

            this.EMPRoaming.OnAuthorizeStart += async (Timestamp,
                                                       Sender,
                                                       CancellationToken,
                                                       EventTrackingId,
                                                       OperatorId,
                                                       UID,
                                                       EVSEId,
                                                       SessionId,
                                                       PartnerProductId,
                                                       PartnerSessionId,
                                                       RequestTimeout) => {


                var response = await RoamingNetwork.AuthorizeStart(UID.ToWWCP(),
                                                                   EVSEId.Value.ToWWCP(),
                                                                   PartnerProductId.ToWWCP(),
                                                                   SessionId.ToWWCP(),
                                                                   OperatorId.ToWWCP(),

                                                                   Timestamp,
                                                                   CancellationToken,
                                                                   EventTrackingId,
                                                                   RequestTimeout).
                                                    ConfigureAwait(false);

                if (response != null)
                {
                    switch (response.Result)
                    {

                        case AuthStartEVSEResultType.Authorized:
                            return new AuthorizationStart(response.SessionId.ToOICP(),
                                                          null,
                                                          response.ProviderId.ToOICP(),
                                                          "Ready to charge!",
                                                          null,
                                                          response.ListOfAuthStopTokens.
                                                              SafeSelect(token => AuthorizationIdentification.FromRFIDId(token.ToOICP())));

                        case AuthStartEVSEResultType.NotAuthorized:
                            return new AuthorizationStart(StatusCodes.RFIDAuthenticationfailed_InvalidUID,
                                                          "RFID Authentication failed - invalid UID");

                        case AuthStartEVSEResultType.InvalidSessionId:
                            return new AuthorizationStart(StatusCodes.SessionIsInvalid,
                                                          "Session is invalid");

                        case AuthStartEVSEResultType.CommunicationTimeout:
                            return new AuthorizationStart(StatusCodes.CommunicationToEVSEFailed,
                                                          "Communication to EVSE failed!");

                        case AuthStartEVSEResultType.StartChargingTimeout:
                            return new AuthorizationStart(StatusCodes.NoEVConnectedToEVSE,
                                                          "No EV connected to EVSE!");

                        case AuthStartEVSEResultType.Reserved:
                            return new AuthorizationStart(StatusCodes.EVSEAlreadyReserved,
                                                          "EVSE already reserved!");

                        case AuthStartEVSEResultType.UnknownEVSE:
                            return new AuthorizationStart(StatusCodes.UnknownEVSEID,
                                                          "Unknown EVSE ID!");

                        case AuthStartEVSEResultType.OutOfService:
                            return new AuthorizationStart(StatusCodes.EVSEOutOfService,
                                                          "EVSE out of service!");

                    }
                }

                return new AuthorizationStart(StatusCodes.ServiceNotAvailable,
                                              "Service not available!",
                                              SessionId:  response?.SessionId. ToOICP() ?? SessionId,
                                              ProviderId: response?.ProviderId.ToOICP());

            };

            #endregion

            #region OnAuthorizeStop

            this.EMPRoaming.OnAuthorizeStop += async (Timestamp,
                                                      Sender,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      SessionId,
                                                      PartnerSessionId,
                                                      OperatorId,
                                                      EVSEId,
                                                      AuthToken,
                                                      RequestTimeout) => {


                var response = await RoamingNetwork.AuthorizeStop(SessionId. ToWWCP().Value,
                                                                  AuthToken. ToWWCP(),
                                                                  EVSEId.    ToWWCP(),
                                                                  OperatorId.ToWWCP(),

                                                                  Timestamp,
                                                                  CancellationToken,
                                                                  EventTrackingId,
                                                                  RequestTimeout).
                                                    ConfigureAwait(false);

                                                          if (response != null)
                {
                    switch (response.Result)
                    {

                        case AuthStopEVSEResultType.Authorized:
                            return new AuthorizationStop(response.SessionId. ToOICP(),
                                                         response.ProviderId.ToOICP(),
                                                         null,
                                                         "Ready to stop charging!");

                        case AuthStopEVSEResultType.InvalidSessionId:
                            return new AuthorizationStop(StatusCodes.SessionIsInvalid,
                                                         "Session is invalid");

                        case AuthStopEVSEResultType.EVSECommunicationTimeout:
                            return new AuthorizationStop(StatusCodes.CommunicationToEVSEFailed,
                                                         "Communication to EVSE failed!");

                        case AuthStopEVSEResultType.StopChargingTimeout:
                            return new AuthorizationStop(StatusCodes.NoEVConnectedToEVSE,
                                                         "No EV connected to EVSE!");

                        case AuthStopEVSEResultType.UnknownEVSE:
                            return new AuthorizationStop(StatusCodes.UnknownEVSEID,
                                                         "Unknown EVSE ID!");

                        case AuthStopEVSEResultType.OutOfService:
                            return new AuthorizationStop(StatusCodes.EVSEOutOfService,
                                                         "EVSE out of service!");

                    }
                }

                return new AuthorizationStop(StatusCodes.ServiceNotAvailable,
                                             "Service not available!",
                                             SessionId:  response?.SessionId. ToOICP() ?? SessionId,
                                             ProviderId: response?.ProviderId.ToOICP());

            };

            #endregion

            #region OnChargeDetailRecord

            this.EMPRoaming.OnChargeDetailRecord += async (Timestamp,
                                                           Sender,
                                                           CancellationToken,
                                                           EventTrackingId,
                                                           ChargeDetailRecord,
                                                           RequestTimeout) => {


                var response = await RoamingNetwork.SendChargeDetailRecord(ChargeDetailRecord.ToWWCP(),
                                                                           DateTime.Now,
                                                                           Timestamp,
                                                                           CancellationToken,
                                                                           EventTrackingId,
                                                                           RequestTimeout).
                                                    ConfigureAwait(false);

                if (response != null)
                {
                    switch (response.Status)
                    {

                        case SendCDRResultType.Forwarded:
                            return new Acknowledgement(StatusCodes.Success,
                                                       "Charge detail record forwarded!",
                                                       null,
                                                       ChargeDetailRecord?.SessionId,
                                                       ChargeDetailRecord?.PartnerSessionId);

                        case SendCDRResultType.NotForwared:
                            return new Acknowledgement(StatusCodes.SystemError,
                                                       "Communication to EVSE failed!",
                                                       null,
                                                       ChargeDetailRecord?.SessionId,
                                                       ChargeDetailRecord?.PartnerSessionId);

                        case SendCDRResultType.InvalidSessionId:
                            return new Acknowledgement(StatusCodes.SessionIsInvalid,
                                                       "Session is invalid",
                                                       null,
                                                       ChargeDetailRecord?.SessionId,
                                                       ChargeDetailRecord?.PartnerSessionId);

                        case SendCDRResultType.UnknownEVSE:
                            return new Acknowledgement(StatusCodes.UnknownEVSEID,
                                                       "Unknown EVSE ID!",
                                                       null,
                                                       ChargeDetailRecord?.SessionId,
                                                       ChargeDetailRecord?.PartnerSessionId);

                        case SendCDRResultType.Error:
                            return new Acknowledgement(StatusCodes.DataError,
                                                       "Data Error!",
                                                       null,
                                                       ChargeDetailRecord?.SessionId,
                                                       ChargeDetailRecord?.PartnerSessionId);

                    }
                }

                return new Acknowledgement(StatusCodes.ServiceNotAvailable,
                                                   "Service not available!",
                                                   null,
                                                   ChargeDetailRecord?.SessionId);

            };

            #endregion

        }

        #endregion

        #region WWCPEMPAdapter(Id, Name, RoamingNetwork, EMPClient, EMPServer, Context = EMPRoaming.DefaultLoggingContext, LogFileCreator = null)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP EMP Roaming client for e-mobility providers/EMPs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="EMPClient">An OICP EMP client.</param>
        /// <param name="EMPServer">An OICP EMP sever.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record after receiving it from the roaming provider.</param>
        public WWCPEMPAdapter(EMPRoamingProvider_Id         Id,
                              I18NString                    Name,
                              RoamingNetwork                RoamingNetwork,

                              EMPClient                     EMPClient,
                              EMPServer                     EMPServer,
                              String                        ServerLoggingContext  = EMPServerLogger.DefaultContext,
                              Func<String, String, String>  LogFileCreator        = null,

                              EVSEDataRecord2EVSEDelegate   EVSEDataRecord2EVSE   = null)

            : this(Id,
                   Name,
                   RoamingNetwork,

                   new EMPRoaming(EMPClient,
                                  EMPServer,
                                  ServerLoggingContext,
                                  LogFileCreator),

                   EVSEDataRecord2EVSE)

        { }

        #endregion

        #region WWCPEMPAdapter(Id, Name, RoamingNetwork, RemoteHostName, ...)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP EMP Roaming client for e-mobility providers/EMPs.
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
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record after receiving it from the roaming provider.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public WWCPEMPAdapter(EMPRoamingProvider_Id                Id,
                              I18NString                           Name,
                              RoamingNetwork                       RoamingNetwork,

                              String                               RemoteHostname,
                              IPPort                               RemoteTCPPort                   = null,
                              RemoteCertificateValidationCallback  RemoteCertificateValidator      = null,
                              X509Certificate                      ClientCert                      = null,
                              String                               RemoteHTTPVirtualHost           = null,
                              String                               URIPrefix                       = EMPClient.DefaultURIPrefix,
                              String                               HTTPUserAgent                   = EMPClient.DefaultHTTPUserAgent,
                              TimeSpan?                            RequestTimeout                  = null,

                              String                               ServerName                      = EMPServer.DefaultHTTPServerName,
                              IPPort                               ServerTCPPort                   = null,
                              String                               ServerURIPrefix                 = EMPServer.DefaultURIPrefix,
                              HTTPContentType                      ServerContentType               = null,
                              Boolean                              ServerRegisterHTTPRootService   = true,
                              Boolean                              ServerAutoStart                 = false,

                              String                               ClientLoggingContext            = EMPClient.EMPClientLogger.DefaultContext,
                              String                               ServerLoggingContext            = EMPServerLogger.DefaultContext,
                              Func<String, String, String>         LogFileCreator                  = null,

                              EVSEDataRecord2EVSEDelegate          EVSEDataRecord2EVSE             = null,

                              DNSClient                            DNSClient                       = null)

            : this(Id,
                   Name,
                   RoamingNetwork,

                   new EMPRoaming(Id.ToString(),
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

                   EVSEDataRecord2EVSE)

        {

            if (ServerAutoStart)
                EMPServer.Start();

        }

        #endregion

        #endregion


        // Outgoing EMPClient requests...

        #region PushEVSEData/-Status directly...

        #region PullEVSEData(RoamingNetwork, SearchCenter = null, DistanceKM = 0.0, LastCall = null, ProviderId = null, ...)

        /// <summary>
        /// Create a new task querying EVSE data from the OICP server.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network to store the downloaded EVSE data.</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="LastCall">An optional timestamp of the last call.</param>
        /// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task

            PullEVSEData(RoamingNetwork         RoamingNetwork,
                         GeoCoordinate          SearchCenter       = null,
                         Double                 DistanceKM         = 0.0,
                         DateTime?              LastCall           = null,
                         eMobilityProvider_Id?  ProviderId         = null,

                         DateTime?              Timestamp          = null,
                         CancellationToken?     CancellationToken  = null,
                         EventTracking_Id       EventTrackingId    = null,
                         TimeSpan?              RequestTimeout     = null)

        {

            #region Initial checks

            if (ProviderId == null || !ProviderId.HasValue)
                throw new ArgumentNullException(nameof(ProviderId), "The provider identification is mandatory in OICP!");

            #endregion

            var result = await EMPRoaming.PullEVSEData(ProviderId.Value.ToOICP(),
                                                       SearchCenter,
                                                       DistanceKM,
                                                       LastCall,

                                                       Timestamp,
                                                       CancellationToken,
                                                       EventTrackingId,
                                                       RequestTimeout).
                                          ConfigureAwait(false);


            if (result.HTTPStatusCode == HTTPStatusCode.OK &&
                result.Content        != null)
            {

                #region Data

                ChargingStationOperator     _EVSEOperator                  = null;
                CPInfoList       _CPInfoList                    = null;
                EVSEIdLookup     _EVSEIdLookup                  = null;
                EVSEInfo         _EVSEInfo                      = null;
                Languages        LocationLanguage;
                Languages        LocalChargingStationLanguage;
                I18NString       AdditionalInfo                 = null;
                ChargingPool     _ChargingPool                  = null;
                ChargingStation  _ChargingStation               = null;
                EVSE             _EVSE                          = null;

                #endregion

                result.Content.OperatorEVSEData.ForEach(operatorevsedata => {

                    try
                    {

                        #region Find Charging Station Operator, or create a new one...

                        if (!RoamingNetwork.TryGetChargingStationOperatorById(operatorevsedata.OperatorId.ToWWCP(), out _EVSEOperator))
                            _EVSEOperator = RoamingNetwork.CreateNewChargingStationOperator(operatorevsedata.OperatorId.ToWWCP(), I18NString.Create(Languages.unknown, operatorevsedata.OperatorName));

                        else
                        {

                            // Update via events!
                            _EVSEOperator.Name = I18NString.Create(Languages.unknown, operatorevsedata.OperatorName);

                        }

                        #endregion

                        #region Generate a list of all charging pools/stations/EVSEs

                        _CPInfoList = new CPInfoList(_EVSEOperator.Id);

                        #region Create EVSEIdLookup

                        foreach (var evsedatarecord in operatorevsedata.EVSEDataRecords)
                        {

                            try
                            {

                                _CPInfoList.AddOrUpdateCPInfo(ChargingPool_Id.Generate(operatorevsedata.OperatorId.ToWWCP(),
                                                                                       evsedatarecord.  Address.ToWWCP(),
                                                                                       evsedatarecord.  GeoCoordinate),
                                                              evsedatarecord.Address,
                                                              evsedatarecord.GeoCoordinate,
                                                              evsedatarecord.ChargingStationId,
                                                              evsedatarecord.Id);

                                _EVSEIdLookup = _CPInfoList.AnalyseAndGenerateLookUp();

                            }
                            catch (Exception e)
                            {

                                // Processing the EVSEDataRecords for the EVSEIdLookup failed!

                            }

                        }

                        #endregion

                        #region Process EVSEDataRecords

                        foreach (var evsedatarecord in operatorevsedata.EVSEDataRecords)
                        {

                            try
                            {

                                _EVSEInfo = _EVSEIdLookup[evsedatarecord.Id];

                                // Set derived WWCP properties

                                #region Set LocationLanguage

                                switch (_EVSEInfo.PoolAddress.Country.Alpha2Code.ToLower())
                                {

                                    case "de": LocationLanguage = Languages.de; break;
                                    case "fr": LocationLanguage = Languages.fr; break;
                                    case "dk": LocationLanguage = Languages.dk; break;
                                    case "no": LocationLanguage = Languages.no; break;
                                    case "fi": LocationLanguage = Languages.fi; break;
                                    case "se": LocationLanguage = Languages.se; break;

                                    case "sk": LocationLanguage = Languages.sk; break;
                                    //case "be": LocationLanguage = Languages.; break;
                                    case "us": LocationLanguage = Languages.en; break;
                                    case "nl": LocationLanguage = Languages.nl; break;
                                    //case "fo": LocationLanguage = Languages.; break;
                                    case "at": LocationLanguage = Languages.de; break;
                                    case "ru": LocationLanguage = Languages.ru; break;
                                    //case "ch": LocationLanguage = Languages.; break;

                                    default: LocationLanguage = Languages.unknown; break;

                                }

                                if (_EVSEInfo.PoolAddress.Country == Country.Germany)
                                    LocalChargingStationLanguage = Languages.de;

                                else if (_EVSEInfo.PoolAddress.Country == Country.Denmark)
                                    LocalChargingStationLanguage = Languages.dk;

                                else if (_EVSEInfo.PoolAddress.Country == Country.France)
                                    LocalChargingStationLanguage = Languages.fr;

                                else
                                    LocalChargingStationLanguage = Languages.unknown;

                                #endregion

                                #region Update a matching charging pool... or create a new one!

                                if (_EVSEOperator.TryGetChargingPoolbyId(_EVSEInfo.PoolId, out _ChargingPool))
                                {

                                    // External update via events!
                                    _ChargingPool.Description          = evsedatarecord.AdditionalInfo;
                                    _ChargingPool.LocationLanguage     = LocationLanguage;
                                    _ChargingPool.EntranceLocation     = evsedatarecord.GeoChargingPointEntrance;
                                    _ChargingPool.OpeningTimes         = OpeningTimes.Parse(evsedatarecord.OpeningTime);
                                    _ChargingPool.AuthenticationModes  = new ReactiveSet<WWCP.AuthenticationModes>(evsedatarecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => mode.  AsWWCPAuthenticationMode()));
                                    _ChargingPool.PaymentOptions       = new ReactiveSet<WWCP.PaymentOptions>     (evsedatarecord.PaymentOptions.     ToEnumeration().SafeSelect(option => option.AsWWCPPaymentOption()));
                                    _ChargingPool.Accessibility        = evsedatarecord.Accessibility.ToWWCP();
                                    _ChargingPool.HotlinePhoneNumber   = evsedatarecord.HotlinePhoneNumber;

                                }

                                else

                                    _ChargingPool = _EVSEOperator.CreateNewChargingPool(

                                                                      _EVSEInfo.PoolId,

                                                                      Configurator: pool => {
                                                                          pool.Description          = evsedatarecord.AdditionalInfo;
                                                                          pool.Address              = _EVSEInfo.PoolAddress.ToWWCP();
                                                                          pool.GeoLocation          = _EVSEInfo.PoolLocation;
                                                                          pool.LocationLanguage     = LocationLanguage;
                                                                          pool.EntranceLocation     = evsedatarecord.GeoChargingPointEntrance;
                                                                          pool.OpeningTimes         = OpeningTimes.Parse(evsedatarecord.OpeningTime);
                                                                          pool.AuthenticationModes  = new ReactiveSet<WWCP.AuthenticationModes>(evsedatarecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OICPMapper.AsWWCPAuthenticationMode(mode)));
                                                                          pool.PaymentOptions       = new ReactiveSet<WWCP.PaymentOptions>     (evsedatarecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OICPMapper.AsWWCPPaymentOption(option)));
                                                                          pool.Accessibility        = evsedatarecord.Accessibility.ToWWCP();
                                                                          pool.HotlinePhoneNumber   = evsedatarecord.HotlinePhoneNumber;
                                                                      }

                                );

                                #endregion

                                #region Update a matching charging station... or create a new one!

                                if (_ChargingPool.TryGetChargingStationbyId(_EVSEInfo.StationId, out _ChargingStation))
                                {

                                    // Update via events!
                                    _ChargingStation.Name                  = evsedatarecord.ChargingStationName;
                                    _ChargingStation.HubjectStationId      = evsedatarecord.ChargingStationId;
                                    _ChargingStation.AuthenticationModes   = new ReactiveSet<WWCP.AuthenticationModes>(evsedatarecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OICPMapper.AsWWCPAuthenticationMode(mode)));
                                    _ChargingStation.PaymentOptions        = new ReactiveSet<WWCP.PaymentOptions>     (evsedatarecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OICPMapper.AsWWCPPaymentOption(option)));
                                    _ChargingStation.Accessibility         = evsedatarecord.Accessibility.ToWWCP();
                                    _ChargingStation.HotlinePhoneNumber    = evsedatarecord.HotlinePhoneNumber;
                                    _ChargingStation.Description           = evsedatarecord.AdditionalInfo;
                                    _ChargingStation.IsHubjectCompatible   = evsedatarecord.IsHubjectCompatible;
                                    _ChargingStation.DynamicInfoAvailable  = evsedatarecord.DynamicInfoAvailable;

                                }

                                else
                                    _ChargingStation = _ChargingPool.CreateNewStation(

                                                                         _EVSEInfo.StationId,

                                                                         Configurator: station => {
                                                                             station.Name                  = evsedatarecord.ChargingStationName;
                                                                             station.HubjectStationId      = evsedatarecord.ChargingStationId;
                                                                             station.AuthenticationModes   = new ReactiveSet<WWCP.AuthenticationModes>(evsedatarecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OICPMapper.AsWWCPAuthenticationMode(mode)));
                                                                             station.PaymentOptions        = new ReactiveSet<WWCP.PaymentOptions>     (evsedatarecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OICPMapper.AsWWCPPaymentOption(option)));
                                                                             station.Accessibility         = evsedatarecord.Accessibility.ToWWCP();
                                                                             station.HotlinePhoneNumber    = evsedatarecord.HotlinePhoneNumber;
                                                                             station.Description           = evsedatarecord.AdditionalInfo;
                                                                             station.IsHubjectCompatible   = evsedatarecord.IsHubjectCompatible;
                                                                             station.DynamicInfoAvailable  = evsedatarecord.DynamicInfoAvailable;
                                                                         }

                                                       );

                                #endregion

                                #region Update matching EVSE... or create a new one!

                                if (_ChargingStation.TryGetEVSEbyId(evsedatarecord.Id.ToWWCP(), out _EVSE))
                                {

                                    // Update via events!
                                    _EVSE.Description         = evsedatarecord.AdditionalInfo;
                                    _EVSE.ChargingModes       = new ReactiveSet<WWCP.ChargingModes>(evsedatarecord.ChargingModes.ToEnumeration().SafeSelect(mode => OICPMapper.AsWWCPChargingMode(mode)));
                                    //_EVSE.ChargingFacilities  = evsedatarecord.ChargingFacilities;
                                    //_EVSE.MaxPower            = 
                                    //_EVSE.AverageVoltage      = 
                                    //_EVSE.MaxCapacity_kWh     = evsedatarecord.MaxCapacity_kWh;
                                    _EVSE.SocketOutlets       = new ReactiveSet<SocketOutlet>(evsedatarecord.Plugs.ToEnumeration().SafeSelect(Plug => new SocketOutlet(Plug.AsWWCPPlugTypes())));

                                }

                                else
                                    _ChargingStation.CreateNewEVSE(evsedatarecord.Id.ToWWCP(),

                                                                   Configurator: evse => {
                                                                       evse.Description         = evsedatarecord.AdditionalInfo;
                                                                       evse.ChargingModes       = new ReactiveSet<WWCP.ChargingModes>(evsedatarecord.ChargingModes.ToEnumeration().SafeSelect(mode => OICPMapper.AsWWCPChargingMode(mode)));
                                                                       //evse.ChargingFacilities  = evsedatarecord.ChargingFacilities;
                                                                       //evse.MaxPower            = 
                                                                       //evse.AverageVoltage      = 
                                                                       //evse.MaxCapacity_kWh     = 
                                                                       evse.SocketOutlets       = new ReactiveSet<SocketOutlet>(evsedatarecord.Plugs.ToEnumeration().SafeSelect(Plug => new SocketOutlet(Plug.AsWWCPPlugTypes())));
                                                                   }

                                                                  );

                                #endregion


                            }
                            catch (Exception e)
                            {

                                // Processing the EVSEDataRecords failed!

                            }

                        }

                        #endregion

                        #endregion

                    }
                    catch (Exception e)
                    {

                        // Processing the OperatorEvseData failed!

                    }

                });

            }

        }

        #endregion

        #region SearchEVSE(ProviderId, SearchCenter = null, DistanceKM = 0.0, Address = null, Plug = null, ChargingFacility = null, RequestTimeout = null)

        /// <summary>
        /// Create a new Search EVSE request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geocoordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="Address">An optional address of the charging stations.</param>
        /// <param name="Plug">Optional plugs of the charging station.</param>
        /// <param name="ChargingFacility">Optional charging facilities of the charging station.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<EVSESearchResult>

            SearchEVSE(eMobilityProvider_Id  ProviderId,
                       GeoCoordinate         SearchCenter       = null,
                       Double                DistanceKM         = 0.0,
                       Address               Address            = null,
                       PlugTypes?            Plug               = null,
                       ChargingFacilities?   ChargingFacility   = null,

                       DateTime?             Timestamp          = null,
                       CancellationToken?    CancellationToken  = null,
                       EventTracking_Id      EventTrackingId    = null,
                       TimeSpan?             RequestTimeout     = null)

        {

            var result = await EMPRoaming.SearchEVSE(ProviderId.ToOICP(),
                                                     SearchCenter,
                                                     DistanceKM,
                                                     Address,
                                                     Plug,
                                                     ChargingFacility,

                                                     Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     RequestTimeout).
                                          ConfigureAwait(false);

            if (result.HTTPStatusCode == HTTPStatusCode.OK &&
                result.Content        != null)
            {

                return result.Content;

            }

            return result.Content;

        }

        #endregion


        #region PullEVSEStatus(SearchCenter = null, DistanceKM = 0.0, EVSEStatusFilter = null, ProviderId = null, ...)

        /// <summary>
        /// Create a new task requesting the current status of all EVSEs (within an optional search radius and status).
        /// </summary>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="EVSEStatusFilter">An optional EVSE status as filter criteria.</param>
        /// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<IEnumerable<WWCP.EVSEStatus>>

            PullEVSEStatus(GeoCoordinate          SearchCenter        = null,
                           Double                 DistanceKM          = 0.0,
                           EVSEStatusTypes?       EVSEStatusFilter    = null,
                           eMobilityProvider_Id?  ProviderId          = null,

                           DateTime?              Timestamp           = null,
                           CancellationToken?     CancellationToken   = null,
                           EventTracking_Id       EventTrackingId     = null,
                           TimeSpan?              RequestTimeout      = null)

        {

            #region Initial checks

            if (ProviderId == null || !ProviderId.HasValue)
                throw new ArgumentNullException(nameof(ProviderId), "The provider identification is mandatory in OICP!");

            #endregion

            var result = await EMPRoaming.PullEVSEStatus(ProviderId.Value.ToOICP(),
                                                         SearchCenter,
                                                         DistanceKM,
                                                         EVSEStatusFilter,

                                                         Timestamp,
                                                         CancellationToken,
                                                         EventTrackingId,
                                                         RequestTimeout).
                                          ConfigureAwait(false);

            if (result.HTTPStatusCode == HTTPStatusCode.OK &&
                result.Content        != null)
            {

                return result.Content.OperatorEVSEStatus.
                           SelectMany(operatorevsestatus => operatorevsestatus.EVSEStatusRecords).
                           SafeSelect(evsestatusrecord   => new WWCP.EVSEStatus(evsestatusrecord.Id.ToWWCP(),
                                                                                OICPMapper.AsWWCPEVSEStatus(evsestatusrecord.Status),
                                                                                result.Timestamp));

            }

            return new WWCP.EVSEStatus[0];

        }

        #endregion

        #region PullEVSEStatusById(EVSEIds, ProviderId = null, ...)

        /// <summary>
        /// Check the current status of the given EVSE Ids.
        /// </summary>
        /// <param name="EVSEIds">An enumeration of EVSE Ids.</param>
        /// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<IEnumerable<WWCP.EVSEStatus>>

            PullEVSEStatusById(IEnumerable<EVSE_Id>   EVSEIds,
                               eMobilityProvider_Id?  ProviderId          = null,

                               DateTime?              Timestamp           = null,
                               CancellationToken?     CancellationToken   = null,
                               EventTracking_Id       EventTrackingId     = null,
                               TimeSpan?              RequestTimeout      = null)

        {

            #region Initial checks

            if (ProviderId == null || !ProviderId.HasValue)
                throw new ArgumentNullException(nameof(ProviderId), "The provider identification is mandatory in OICP!");

            #endregion

            var _EVSEStatus = new List<WWCP.EVSEStatus>();

            // Hubject has a limit of 100 EVSEIds per request!
            // Do not make concurrent requests!
            foreach (var evsepart in EVSEIds.ToPartitions(100))
            {

                var result = await EMPRoaming.PullEVSEStatusById(ProviderId.Value.ToOICP(),
                                                                 evsepart,

                                                                 Timestamp,
                                                                 CancellationToken,
                                                                 EventTrackingId,
                                                                 RequestTimeout).
                                              ConfigureAwait(false);

                if (result.HTTPStatusCode == HTTPStatusCode.OK &&
                    result.Content != null)
                {

                    _EVSEStatus.AddRange(result.Content.EVSEStatusRecords.
                                                SafeSelect(evsestatusrecord => new WWCP.EVSEStatus(evsestatusrecord.Id.ToWWCP(),
                                                                                                   OICPMapper.AsWWCPEVSEStatus(evsestatusrecord.Status),
                                                                                                   result.Timestamp)));

                }

            }

            return _EVSEStatus;

        }

        #endregion


        #region PushAuthenticationData(...AuthorizationIdentifications, Action = fullLoad, ProviderId = null, ...)

        /// <summary>
        /// Create a new task pushing authorization identifications onto the OICP server.
        /// </summary>
        /// <param name="AuthorizationIdentifications">An enumeration of authorization identifications.</param>
        /// <param name="Action">An optional OICP action.</param>
        /// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<WWCP.Acknowledgement>

            PushAuthenticationData(IEnumerable<AuthorizationIdentification>  AuthorizationIdentifications,
                                   ActionType                                Action             = ActionType.fullLoad,
                                   eMobilityProvider_Id?                     ProviderId         = null,

                                   DateTime?                                 Timestamp          = null,
                                   CancellationToken?                        CancellationToken  = null,
                                   EventTracking_Id                          EventTrackingId    = null,
                                   TimeSpan?                                 RequestTimeout     = null)

        {

            #region Initial checks

            if (!ProviderId.HasValue)
                throw new ArgumentNullException(nameof(ProviderId), "The provider identification is mandatory in OICP!");


            WWCP.Acknowledgement result = null;

            #endregion

            #region Send OnPushAuthenticationDataRequest event

            //var StartTime = DateTime.Now;

            //try
            //{

            //    OnPushAuthenticationDataRequest?.Invoke(StartTime,
            //                                            Request.Timestamp.Value,
            //                                            this,
            //                                            ClientId,
            //                                            Request.EventTrackingId,
            //                                            Request.AuthorizationIdentifications,
            //                                            Request.ProviderId,
            //                                            Request.OICPAction,
            //                                            RequestTimeout);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnPushAuthenticationDataRequest));
            //}

            #endregion


            var response = await EMPRoaming.PushAuthenticationData(AuthorizationIdentifications,
                                                                   ProviderId.ToOICP().Value,
                                                                   Action.    ToOICP(),

                                                                   Timestamp,
                                                                   CancellationToken,
                                                                   EventTrackingId,
                                                                   RequestTimeout).
                                            ConfigureAwait(false);

            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        != null)
            {

                result = new WWCP.Acknowledgement(response.Content.Result
                                                      ? ResultType.True
                                                      : ResultType.False,
                                                  response.Content.StatusCode.Description,
                                                  response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                      ? new String[] { response.Content.StatusCode.AdditionalInfo }
                                                      : null);

            }

            result = new WWCP.Acknowledgement(ResultType.False,
                                              response.Content != null
                                                  ? response.Content.StatusCode.Description
                                                  : null,
                                              response.Content != null
                                                  ? response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                        ? new String[] { response.Content.StatusCode.AdditionalInfo }
                                                        : null
                                                  : null);


            #region Send OnPushAuthenticationDataResponse event

            //var Endtime = DateTime.Now;
            //
            //try
            //{
            //
            //    OnPushAuthenticationDataResponse?.Invoke(Endtime,
            //                                             this,
            //                                             ClientId,
            //                                             Request.EventTrackingId,
            //                                             Request.AuthorizationIdentifications,
            //                                             Request.ProviderId,
            //                                             Request.OICPAction,
            //                                             RequestTimeout,
            //                                             response.Content,
            //                                             Endtime - StartTime);
            //
            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(EMPClient) + "." + nameof(OnPushAuthenticationDataResponse));
            //}

            #endregion

            return result;

        }

        #endregion


        #region Reserve(EVSEId, ChargingProductId = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be reserved.</param>
        /// <param name="ReservationStartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
        /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<ReservationResult>

            IReserveRemoteStartStop.Reserve(WWCP.EVSE_Id                      EVSEId,
                                            DateTime?                         ReservationStartTime,  // = null,
                                            TimeSpan?                         Duration,              // = null,
                                            ChargingReservation_Id?           ReservationId,         // = null,
                                            eMobilityProvider_Id?             ProviderId,            // = null,
                                            eMobilityAccount_Id?              eMAId,                 // = null,
                                            ChargingProduct_Id?               ChargingProductId,     // = null,
                                            IEnumerable<Auth_Token>           AuthTokens,            // = null,
                                            IEnumerable<eMobilityAccount_Id>  eMAIds,                // = null,
                                            IEnumerable<UInt32>               PINs,                  // = null,

                                            DateTime?                         Timestamp,
                                            CancellationToken?                CancellationToken,
                                            EventTracking_Id                  EventTrackingId,
                                            TimeSpan?                         RequestTimeout)

        {

            #region Initial checks

            if (ProviderId == null || !ProviderId.HasValue)
                throw new ArgumentNullException(nameof(ProviderId),  "The provider identification is mandatory in OICP!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;


            var StartTime = DateTime.Now;

            #endregion

            #region Send OnReserveEVSERequest event

            try
            {

                OnReserveEVSERequest?.Invoke(StartTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             ReservationId,
                                             EVSEId,
                                             ReservationStartTime,
                                             Duration,
                                             ProviderId,
                                             eMAId,
                                             ChargingProductId,
                                             AuthTokens,
                                             eMAIds,
                                             PINs,
                                             RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnReserveEVSERequest));
            }

            #endregion


            #region Check if the PartnerProductId has a special format like 'D=15min|P=AC1'

            var PartnerProductIdElements = new Dictionary<String, String>();

            if (ChargingProductId.ToString().IsNotNullOrEmpty() &&
               !ChargingProductId.ToString().Contains('='))
            {
                PartnerProductIdElements.Add("P", ChargingProductId.ToString());
                ChargingProductId = null;
            }

            if (ChargingProductId != null)
                ChargingProductId.ToString().DoubleSplitInto('|', '=', PartnerProductIdElements);

            #endregion

            #region Copy the 'ReservationStartTime' value into the PartnerProductId "S=..."

            if (ReservationStartTime.HasValue)
            {

                if (!PartnerProductIdElements.ContainsKey("S"))
                    PartnerProductIdElements.Add("S", ReservationStartTime.Value.ToIso8601());
                else
                    PartnerProductIdElements["S"] = ReservationStartTime.Value.ToIso8601();

            }

            #endregion

            #region Copy the 'Duration' value into the PartnerProductId "D=...min"

            if (Duration.HasValue && Duration.Value >= TimeSpan.FromSeconds(1))
            {

                if (Duration.Value.Minutes > 0 && Duration.Value.Seconds == 0)
                {
                    if (!PartnerProductIdElements.ContainsKey("D"))
                        PartnerProductIdElements.Add("D", Duration.Value.TotalMinutes + "min");
                    else
                        PartnerProductIdElements["D"] = Duration.Value.TotalMinutes + "min";
                }

                else
                {
                    if (!PartnerProductIdElements.ContainsKey("D"))
                        PartnerProductIdElements.Add("D", Duration.Value.TotalSeconds + "sec");
                    else
                        PartnerProductIdElements["D"] = Duration.Value.TotalSeconds + "sec";
                }

            }

            #endregion

            #region Add the eMAId to the list of valid eMAIds

            if (eMAIds == null && eMAId.HasValue)
                eMAIds = new List<eMobilityAccount_Id> { eMAId.Value };

            if (eMAIds != null && eMAId.HasValue && !eMAIds.Contains(eMAId.Value))
            {
                var _eMAIds = new List<eMobilityAccount_Id>(eMAIds);
                _eMAIds.Add(eMAId.Value);
                eMAIds = _eMAIds;
            }

            #endregion


            var result = await EMPRoaming.ReservationStart(EVSEId:             EVSEId.ToOICP(),
                                                           ProviderId:         ProviderId.Value.ToOICP(),
                                                           EVCOId:             eMAId.     Value.ToOICP(),
                                                           SessionId:          ReservationId != null ? Session_Id.Parse(ReservationId.ToString()) : new Session_Id?(),
                                                           PartnerSessionId:   null,
                                                           PartnerProductId:   PartnerProduct_Id.Parse(PartnerProductIdElements.
                                                                                                            Select(kvp => kvp.Key + "=" + kvp.Value).
                                                                                                            AggregateWith("|")),

                                                           Timestamp:          Timestamp,
                                                           CancellationToken:  CancellationToken,
                                                           EventTrackingId:    EventTrackingId,
                                                           RequestTimeout:     RequestTimeout).
                                          ConfigureAwait(false);


            if (result.HTTPStatusCode == HTTPStatusCode.OK &&
                result.Content        != null              &&
                result.Content.Result)
            {

                return ReservationResult.Success(result.Content.SessionId != null
                                                     ? new ChargingReservation(ReservationId:            ChargingReservation_Id.Parse(result.Content.SessionId.ToString()),
                                                                               Timestamp:                DateTime.Now,
                                                                               StartTime:                DateTime.Now,
                                                                               Duration:                 Duration.HasValue ? Duration.Value : DefaultReservationTime,
                                                                               EndTime:                  DateTime.Now + (Duration.HasValue ? Duration.Value : DefaultReservationTime),
                                                                               ConsumedReservationTime:  TimeSpan.FromSeconds(0),
                                                                               ReservationLevel:         ChargingReservationLevel.EVSE,
                                                                               ProviderId:               ProviderId,
                                                                               eMAId:                    eMAId,
                                                                               RoamingNetwork:           RoamingNetwork,
                                                                               ChargingPoolId:           null,
                                                                               ChargingStationId:        null,
                                                                               EVSEId:                   EVSEId,
                                                                               ChargingProductId:        ChargingProductId,
                                                                               AuthTokens:               AuthTokens,
                                                                               eMAIds:                   eMAIds,
                                                                               PINs:                     PINs)
                                                     : null);

            }

            return ReservationResult.Error(result.HTTPStatusCode.ToString(),
                                           result);

        }

        #endregion

        #region CancelReservation(ReservationId, Reason, ProviderId = null, EVSEId = null, ...)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
        /// <param name="EVSEId">An optional identification of the EVSE.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<CancelReservationResult>

            IReserveRemoteStartStop.CancelReservation(ChargingReservation_Id                 ReservationId,
                                                      ChargingReservationCancellationReason  Reason,
                                                      eMobilityProvider_Id?                  ProviderId,  // = null,
                                                      WWCP.EVSE_Id?                          EVSEId,      // = null,

                                                      DateTime?                              Timestamp,
                                                      CancellationToken?                     CancellationToken,
                                                      EventTracking_Id                       EventTrackingId,
                                                      TimeSpan?                              RequestTimeout)

        {

            #region Initial checks

            if (!ProviderId.HasValue)
                throw new ArgumentNullException(nameof(ProviderId), "The provider identification is mandatory in OICP!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;


            var StartTime = DateTime.Now;

            #endregion

            #region Send OnCancelReservationRequest event

            try
            {

                OnCancelReservationRequest?.Invoke(StartTime,
                                                   Timestamp.Value,
                                                   this,
                                                   EventTrackingId,
                                                   RoamingNetwork.Id,
                                                   ProviderId.Value,
                                                   ReservationId,
                                                   Reason,
                                                   RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            var result = await EMPRoaming.ReservationStop(SessionId:          Session_Id.Parse(ReservationId.ToString()),
                                                          ProviderId:         ProviderId.Value.ToOICP(),
                                                          EVSEId:             EVSEId.    Value.ToOICP(),
                                                          PartnerSessionId:   null,

                                                          Timestamp:          Timestamp,
                                                          CancellationToken:  CancellationToken,
                                                          EventTrackingId:    EventTrackingId,
                                                          RequestTimeout:     RequestTimeout).
                                          ConfigureAwait(false);

            if (result.HTTPStatusCode == HTTPStatusCode.OK &&
                result.Content        != null              &&
                result.Content.Result)
            {

                return CancelReservationResult.Success(ReservationId);

            }

            return CancelReservationResult.Error(result.HTTPStatusCode.ToString(),
                                                 result);

        }

        #endregion


        #region RemoteStart(EVSEId,            ChargingProductId = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be started remotely.</param>
        /// <param name="ChargingProductId">An optional identification of the charging product to use.</param>
        /// <param name="Duration">An optional maximum time span to charge. When it is reached, the charging process will stop automatically.</param>
        /// <param name="MaxEnergy">An optional maximum amount of energy to charge. When it is reached, the charging process will stop automatically.</param>
        /// <param name="ReservationId">An optional identification of a charging reservation.</param>
        /// <param name="SessionId">An optional identification of this charging session.</param>
        /// <param name="ProviderId">An optional identification of the e-mobility service provider, whenever this identification is different from the current message sender.</param>
        /// <param name="eMAId">An optional identification of the e-mobility account who wants to charge.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<RemoteStartEVSEResult>

            IReserveRemoteStartStop.RemoteStart(WWCP.EVSE_Id             EVSEId,
                                                ChargingProduct_Id?      ChargingProductId,  // = null,
//                                              TimeSpan?                Duration            // = null,
//                                              Double?                  MaxEnergy           // = null,
                                                ChargingReservation_Id?  ReservationId,      // = null,
                                                ChargingSession_Id?      SessionId,          // = null,
                                                eMobilityProvider_Id?    ProviderId,         // = null,
                                                eMobilityAccount_Id?     eMAId,              // = null,

                                                DateTime?                Timestamp,
                                                CancellationToken?       CancellationToken,
                                                EventTracking_Id         EventTrackingId,
                                                TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (!ProviderId.HasValue)
                throw new ArgumentNullException(nameof(ProviderId),  "The e-mobility provider identification is mandatory in OICP!");

            if (!eMAId.HasValue)
                throw new ArgumentNullException(nameof(eMAId),       "The e-mobility account identification is mandatory in OICP!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;


            var StartTime = DateTime.Now;

            #endregion

            #region Send OnRemoteStartEVSERequest event

            try
            {

                OnRemoteStartEVSERequest?.Invoke(StartTime,
                                                 Timestamp.Value,
                                                 this,
                                                 EventTrackingId,
                                                 RoamingNetwork.Id,
                                                 EVSEId,
                                                 ChargingProductId,
                                                 ReservationId,
                                                 SessionId,
                                                 ProviderId,
                                                 eMAId,
                                                 RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnRemoteStartEVSERequest));
            }

            #endregion


            #region Check if the PartnerProductId has a special format like 'R=12345-1234...|P=AC1'

            var PartnerProductIdElements = new Dictionary<String, String>();

            if (ChargingProductId.ToString().IsNotNullOrEmpty() &&
               !ChargingProductId.ToString().Contains('='))
            {
                PartnerProductIdElements.Add("P", ChargingProductId.ToString());
                ChargingProductId = null;
            }

            if (ChargingProductId != null)
                ChargingProductId.ToString().DoubleSplitInto('|', '=', PartnerProductIdElements);

            #endregion

            #region Copy the 'Duration' value into the PartnerProductId "D=...min"

            //if (Duration.HasValue && Duration.Value >= TimeSpan.FromSeconds(1))
            //{
            //
            //    if (Duration.Value.Minutes > 0 && Duration.Value.Seconds == 0)
            //    {
            //        if (!PartnerProductIdElements.ContainsKey("D"))
            //            PartnerProductIdElements.Add("D", Duration.Value.TotalMinutes + "min");
            //        else
            //            PartnerProductIdElements["D"] = Duration.Value.TotalMinutes + "min";
            //    }
            //
            //    else
            //    {
            //        if (!PartnerProductIdElements.ContainsKey("D"))
            //            PartnerProductIdElements.Add("D", Duration.Value.TotalSeconds + "sec");
            //        else
            //            PartnerProductIdElements["D"] = Duration.Value.TotalSeconds + "sec";
            //    }
            //
            //}

            #endregion

            #region Copy the 'MaxEnergy' value into the PartnerProductId

            //if (MaxEnergy.HasValue && MaxEnergy.Value > 0))
            //{
            //
            //    if (Duration.Value.Minutes > 0 && Duration.Value.Seconds == 0)
            //    {
            //        if (!PartnerProductIdElements.ContainsKey("D"))
            //            PartnerProductIdElements.Add("D", Duration.Value.TotalMinutes + "min");
            //        else
            //            PartnerProductIdElements["D"] = Duration.Value.TotalMinutes + "min";
            //    }
            //
            //    else
            //    {
            //        if (!PartnerProductIdElements.ContainsKey("D"))
            //            PartnerProductIdElements.Add("D", Duration.Value.TotalSeconds + "sec");
            //        else
            //            PartnerProductIdElements["D"] = Duration.Value.TotalSeconds + "sec";
            //    }
            //
            //}

            #endregion

            #region Copy the 'ReservationId' value into the PartnerProductId

            if (ReservationId != null)
            {

                if (!PartnerProductIdElements.ContainsKey("R"))
                    PartnerProductIdElements.Add("R", ReservationId.ToString());
                else
                    PartnerProductIdElements["R"] = ReservationId.ToString();

            }

            #endregion


            var result = await EMPRoaming.RemoteStart(EVSEId:             EVSEId.ToOICP(),
                                                      ProviderId:         ProviderId.Value.ToOICP(),
                                                      EVCOId:             eMAId.     Value.ToOICP(),
                                                      SessionId:          SessionId.       ToOICP(),
                                                      PartnerSessionId:   null,
                                                      PartnerProductId:   PartnerProduct_Id.Parse(PartnerProductIdElements.
                                                                                                      Select(kvp => kvp.Key + "=" + kvp.Value).
                                                                                                      AggregateWith("|")),

                                                      Timestamp:          Timestamp,
                                                      CancellationToken:  CancellationToken,
                                                      EventTrackingId:    EventTrackingId,
                                                      RequestTimeout:     RequestTimeout).
                                          ConfigureAwait(false);


            var Now     = DateTime.Now;
            var Runtime = Now - Timestamp.Value;

            if (result.HTTPStatusCode == HTTPStatusCode.OK &&
                result.Content        != null              &&
                result.Content.Result)
            {

                return RemoteStartEVSEResult.Success(result.Content.SessionId.HasValue
                                                         ? new ChargingSession(result.Content.SessionId.ToWWCP().Value)
                                                         : null);

            }

            return RemoteStartEVSEResult.Error(result.HTTPStatusCode.ToString(),
                                               result);

        }

        #endregion

        #region RemoteStart(ChargingStationId, ChargingProductId = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session at the given charging station.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station to be started.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<RemoteStartChargingStationResult>

            IReserveRemoteStartStop.RemoteStart(ChargingStation_Id       ChargingStationId,
                                                ChargingProduct_Id?      ChargingProductId,  // = null,
                                                ChargingReservation_Id?  ReservationId,      // = null,
                                                ChargingSession_Id?      SessionId,          // = null,
                                                eMobilityProvider_Id?    ProviderId,         // = null,
                                                eMobilityAccount_Id?     eMAId,              // = null,

                                                DateTime?                Timestamp,
                                                CancellationToken?       CancellationToken,
                                                EventTracking_Id         EventTrackingId,
                                                TimeSpan?                RequestTimeout)


                => Task.FromResult(RemoteStartChargingStationResult.OutOfService);

        #endregion


        #region RemoteStop(                   SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<RemoteStopResult>

            IReserveRemoteStartStop.RemoteStop(ChargingSession_Id     SessionId,
                                               ReservationHandling    ReservationHandling,
                                               eMobilityProvider_Id?  ProviderId,         // = null,
                                               eMobilityAccount_Id?   eMAId,              // = null,

                                               DateTime?              Timestamp,
                                               CancellationToken?     CancellationToken,
                                               EventTracking_Id       EventTrackingId,
                                               TimeSpan?              RequestTimeout)


                => Task.FromResult(RemoteStopResult.OutOfService(SessionId));

        #endregion

        #region RemoteStop(EVSEId,            SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be stopped remotely.</param>
        /// <param name="SessionId">An optional identification of this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">An optional identification of the e-mobility service provider, whenever this identification is different from the current message sender.</param>
        /// <param name="eMAId">An optional identification of the e-mobility account who wants to stop charging.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopEVSEResult>

            RemoteStop(WWCP.EVSE_Id           EVSEId,
                       ChargingSession_Id     SessionId,
                       ReservationHandling    ReservationHandling   = null,
                       eMobilityProvider_Id?  ProviderId            = null,
                       eMobilityAccount_Id?   eMAId                 = null,

                       DateTime?              Timestamp             = null,
                       CancellationToken?     CancellationToken     = null,
                       EventTracking_Id       EventTrackingId       = null,
                       TimeSpan?              RequestTimeout        = null)

        {

            #region Initial checks

            if (!ProviderId.HasValue)
                throw new ArgumentNullException(nameof(ProviderId),  "The e-mobility provider identification is mandatory in OICP!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;


            var StartTime = DateTime.Now;

            #endregion

            #region Send OnRemoteStopEVSERequest event

            try
            {

                OnRemoteStopEVSERequest?.Invoke(StartTime,
                                                 Timestamp.Value,
                                                 this,
                                                 EventTrackingId,
                                                 RoamingNetwork.Id,
                                                 EVSEId,
                                                 SessionId,
                                                 ReservationHandling,
                                                 ProviderId,
                                                 eMAId,
                                                 RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnRemoteStopEVSERequest));
            }

            #endregion



            var result = await EMPRoaming.RemoteStop(SessionId:          SessionId.       ToOICP(),
                                                     ProviderId:         ProviderId.Value.ToOICP(),
                                                     EVSEId:             EVSEId.          ToOICP(),
                                                     PartnerSessionId:   null,

                                                     Timestamp:          Timestamp,
                                                     CancellationToken:  CancellationToken,
                                                     EventTrackingId:    EventTrackingId,
                                                     RequestTimeout:     RequestTimeout).
                                          ConfigureAwait(false);

            if (result.HTTPStatusCode == HTTPStatusCode.OK &&
                result.Content        != null              &&
                result.Content.Result)
            {

                return RemoteStopEVSEResult.Success(SessionId);

            }

            return RemoteStopEVSEResult.Error(SessionId,
                                              result.HTTPStatusCode.ToString(),
                                              result);

        }

        #endregion

        #region RemoteStop(ChargingStationId, SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session at the given charging station.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station to be stopped.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<RemoteStopChargingStationResult>

            IReserveRemoteStartStop.RemoteStop(ChargingStation_Id     ChargingStationId,
                                               ChargingSession_Id     SessionId,
                                               ReservationHandling    ReservationHandling,
                                               eMobilityProvider_Id?  ProviderId,         // = null,
                                               eMobilityAccount_Id?   eMAId,              // = null,

                                               DateTime?              Timestamp,
                                               CancellationToken?     CancellationToken,
                                               EventTracking_Id       EventTrackingId,
                                               TimeSpan?              RequestTimeout)


                => Task.FromResult(RemoteStopChargingStationResult.OutOfService(SessionId));

        #endregion


        #region GetChargeDetailRecords(From, To = null, ProviderId = null, ...)

        /// <summary>
        /// Download all charge detail records from the OICP server.
        /// </summary>
        /// <param name="From">The starting time.</param>
        /// <param name="To">An optional end time. [default: current time].</param>
        /// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<IEnumerable<WWCP.ChargeDetailRecord>>

            IEMPRoamingProvider.GetChargeDetailRecords(DateTime               From,
                                                       DateTime?              To,          // = null,
                                                       eMobilityProvider_Id?  ProviderId,  // = null,

                                                       DateTime?              Timestamp,
                                                       CancellationToken?     CancellationToken,
                                                       EventTracking_Id       EventTrackingId,
                                                       TimeSpan?              RequestTimeout)

        {

            #region Initial checks

            if (!ProviderId.HasValue)
                throw new ArgumentNullException(nameof(ProviderId), "The provider identification is mandatory in OICP!");

            if (!To.HasValue)
                To = DateTime.Now;

            #endregion

            var result = await EMPRoaming.GetChargeDetailRecords(ProviderId.Value.ToOICP(),
                                                                 From,
                                                                 To.Value,

                                                                 Timestamp,
                                                                 CancellationToken,
                                                                 EventTrackingId,
                                                                 RequestTimeout).
                                          ConfigureAwait(false);

            if (result.HTTPStatusCode == HTTPStatusCode.OK &&
                result.Content        != null)
            {

                return result.Content.ChargeDetailRecords.SafeSelect(cdr => OICPMapper.ToWWCP(cdr));

            }

            return new WWCP.ChargeDetailRecord[0];

        }

        #endregion

        #endregion


    }

}
