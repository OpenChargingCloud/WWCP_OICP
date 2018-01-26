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
using System.Security.Authentication;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// A WWCP wrapper for the OICP EMP roaming client which maps
    /// WWCP data structures onto OICP data structures and vice versa.
    /// </summary>
    public class WWCPEMPAdapter : ABaseEMobilityEntity<EMPRoamingProvider_Id>,
                                  IEMPRoamingProvider,
                                  ISendAuthenticationData
    {

        #region Data

        private readonly EVSEDataRecord2EVSEDelegate _EVSEDataRecord2EVSE;

        /// <summary>
        ///  The default reservation time.
        /// </summary>
        public static readonly TimeSpan DefaultReservationTime = TimeSpan.FromMinutes(15);


        //private readonly        Object         PullDataServiceLock;
        private readonly        Timer          PullDataServiceTimer;

        /// <summary>
        /// The default status check intervall.
        /// </summary>
        public  readonly static TimeSpan       DefaultPullDataServiceEvery              = TimeSpan.FromMinutes(15);

        public  readonly static TimeSpan       DefaultPullDataServiceRequestTimeout     = TimeSpan.FromMinutes(30);


        //private readonly        Object         PullStatusServiceLock;
        private readonly        Timer          PullStatusServiceTimer;

        /// <summary>
        /// The default status check intervall.
        /// </summary>
        public  readonly static TimeSpan       DefaultPullStatusServiceEvery            = TimeSpan.FromMinutes(1);

        public  readonly static TimeSpan       DefaultPullStatusServiceRequestTimeout   = TimeSpan.FromMinutes(3);

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


        /// <summary>
        /// The offical (multi-language) name of the roaming provider.
        /// </summary>
        [Mandatory]
        public I18NString    Name                { get; }


        /// <summary>
        /// An optional default e-mobility provider identification.
        /// </summary>
        public Provider_Id?  DefaultProviderId   { get; }



        public EVSEOperatorFilterDelegate EVSEOperatorFilter;


        #region OnWWCPEMPAdapterException

        public delegate Task OnWWCPEMPAdapterExceptionDelegate(DateTime        Timestamp,
                                                               WWCPEMPAdapter  Sender,
                                                               Exception       Exception);

        public event OnWWCPEMPAdapterExceptionDelegate OnWWCPEMPAdapterException;

        #endregion

        #region PullDataService

        public Boolean  DisablePullData { get; set; }

        private UInt32 _PullDataServiceEvery;

        public TimeSpan PullDataServiceRequestTimeout { get; }

        /// <summary>
        /// The 'Pull Status' service intervall.
        /// </summary>
        public TimeSpan PullDataServiceEvery
        {

            get
            {
                return TimeSpan.FromSeconds(_PullDataServiceEvery);
            }

            set
            {
                _PullDataServiceEvery = (UInt32) value.TotalSeconds;
            }

        }

        public DateTime? TimestampOfLastPullDataRun { get; private set; }

        private static SemaphoreSlim PullEVSEDataLock = new SemaphoreSlim(1, 1);

        public delegate void PullEVSEDataDelegate(DateTime Timestamp, WWCPEMPAdapter Sender, TimeSpan Every);

        public event PullEVSEDataDelegate FlushServiceQueuesEvent;

        #endregion

        #region PullStatusService

        private static SemaphoreSlim PullEVSEStatusLock = new SemaphoreSlim(1, 1);

        public Boolean  DisablePullStatus { get; set; }

        private UInt32 _PullStatusServiceEvery;

        public TimeSpan PullStatusServiceRequestTimeout { get; }

        /// <summary>
        /// The 'Pull Status' service intervall.
        /// </summary>
        public TimeSpan PullStatusServiceEvery
        {

            get
            {
                return TimeSpan.FromSeconds(_PullStatusServiceEvery);
            }

            set
            {
                _PullStatusServiceEvery = (UInt32) value.TotalSeconds;
            }

        }

        #endregion

        public GeoCoordinate? DefaultSearchCenter { get; }
        public UInt64?        DefaultDistanceKM   { get; }

        public Func<EVSEStatusReport, ChargingStationStatusTypes> EVSEStatusAggregationDelegate { get; }

        #endregion

        #region Events

        // Client methods (logging)

        #region OnPullEVSEDataRequest/-Response (OICP event!)

        /// <summary>
        /// An event sent whenever a 'pull EVSE data' request will be send.
        /// </summary>
        public event OnPullEVSEDataRequestHandler        OnPullEVSEDataRequest;

        /// <summary>
        /// An event sent whenever a response for a 'pull EVSE data' request had been received.
        /// </summary>
        public event OnPullEVSEDataResponseHandler       OnPullEVSEDataResponse;

        #endregion

        #region OnPullEVSEStatusRequest/-Response (OICP event!)

        /// <summary>
        /// An event sent whenever a 'pull EVSE status' request will be send.
        /// </summary>
        public event OnPullEVSEStatusRequestHandler      OnPullEVSEStatusRequest;

        /// <summary>
        /// An event sent whenever a response for a 'pull EVSE status' request had been received.
        /// </summary>
        public event OnPullEVSEStatusResponseHandler     OnPullEVSEStatusResponse;

        #endregion


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


        #region OnGetChargeDetailRecordsRequest/-Response

        /// <summary>
        /// An event sent whenever a 'get charge detail records' request will be send.
        /// </summary>
        public event OnGetCDRsRequestDelegate    OnGetChargeDetailRecordsRequest;

        /// <summary>
        /// An event sent whenever a response to a 'get charge detail records' request was received.
        /// </summary>
        public event OnGetCDRsResponseDelegate   OnGetChargeDetailRecordsResponse;

        #endregion


        // Server methods (logging)

        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event sent whenever an 'authorize start' request was received.
        /// </summary>
        public event WWCP.OnAuthorizeStartRequestDelegate   OnAuthorizeStartRequest;

        /// <summary>
        /// An event sent whenever a response to an 'authorize start' request was sent.
        /// </summary>
        public event WWCP.OnAuthorizeStartResponseDelegate  OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeEVSEStartRequest/-Response

        /// <summary>
        /// An event sent whenever an 'authorize EVSE start' request was received.
        /// </summary>
        public event OnAuthorizeEVSEStartRequestDelegate   OnAuthorizeEVSEStartRequest;

        /// <summary>
        /// An event sent whenever a response to an 'authorize EVSE start' request was sent.
        /// </summary>
        public event OnAuthorizeEVSEStartResponseDelegate  OnAuthorizeEVSEStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event sent whenever an 'authorize stop' request was received.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate   OnAuthorizeStopRequest;

        /// <summary>
        /// An event sent whenever a response to an 'authorize stop' request was sent.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate  OnAuthorizeStopResponse;

        #endregion

        #region OnAuthorizeEVSEStopRequest/-Response

        /// <summary>
        /// An event sent whenever an 'authorize EVSE stop' request was received.
        /// </summary>
        public event OnAuthorizeEVSEStopRequestDelegate   OnAuthorizeEVSEStopRequest;

        /// <summary>
        /// An event sent whenever a response to an 'authorize EVSE stop' request was sent.
        /// </summary>
        public event OnAuthorizeEVSEStopResponseDelegate  OnAuthorizeEVSEStopResponse;

        #endregion

        #region OnChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event sent whenever a 'charge detail record' was received.
        /// </summary>
        public event OnSendCDRRequestDelegate   OnChargeDetailRecordRequest;

        /// <summary>
        /// An event sent whenever a response to a 'charge detail record' was sent.
        /// </summary>
        public event OnSendCDRResponseDelegate  OnChargeDetailRecordResponse;

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
                              EVSEDataRecord2EVSEDelegate  EVSEDataRecord2EVSE               = null,

                              EVSEOperatorFilterDelegate   EVSEOperatorFilter                = null,

                              TimeSpan?                    PullDataServiceEvery              = null,
                              Boolean                      DisablePullData                   = false,
                              TimeSpan?                    PullDataServiceRequestTimeout     = null,

                              TimeSpan?                    PullStatusServiceEvery            = null,
                              Boolean                      DisablePullStatus                 = false,
                              TimeSpan?                    PullStatusServiceRequestTimeout   = null,

                              eMobilityProvider            DefaultProvider                   = null,
                              GeoCoordinate?               DefaultSearchCenter               = null,
                              UInt64?                      DefaultDistanceKM                 = null)

            : base(Id,
                   RoamingNetwork)

        {

            #region Initial checks

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),        "The given roaming provider name must not be null or empty!");

            if (EMPRoaming == null)
                throw new ArgumentNullException(nameof(EMPRoaming),  "The given OICP EMP Roaming object must not be null!");

            #endregion

            this.Name                             = Name;
            this.EMPRoaming                       = EMPRoaming;
            this._EVSEDataRecord2EVSE             = EVSEDataRecord2EVSE;

            this.EVSEOperatorFilter               = EVSEOperatorFilter ?? ((name, id) => true);

            this._PullDataServiceEvery            = (UInt32) (PullDataServiceEvery.HasValue
                                                                  ? PullDataServiceEvery.Value.  TotalMilliseconds
                                                                  : DefaultPullDataServiceEvery. TotalMilliseconds);
            this.PullDataServiceRequestTimeout    = PullDataServiceRequestTimeout ?? DefaultPullDataServiceRequestTimeout;
            //this.PullDataServiceLock              = new Object();
            this.PullDataServiceTimer             = new Timer(PullDataService, null, 5000, _PullDataServiceEvery);
            this.DisablePullData                  = DisablePullData;


            this._PullStatusServiceEvery          = (UInt32) (PullStatusServiceEvery.HasValue
                                                                  ? PullStatusServiceEvery.Value.  TotalMilliseconds
                                                                  : DefaultPullStatusServiceEvery. TotalMilliseconds);
            this.PullStatusServiceRequestTimeout  = PullStatusServiceRequestTimeout ?? DefaultPullStatusServiceRequestTimeout;
            //this.PullStatusServiceLock            = new Object();
            this.PullStatusServiceTimer           = new Timer(PullStatusService, null, 150000, _PullStatusServiceEvery);
            this.DisablePullStatus                = DisablePullStatus;

            this.DefaultProviderId                = DefaultProvider != null
                                                        ? new Provider_Id?(DefaultProvider.Id.ToOICP())
                                                        : null;
            this.DefaultSearchCenter              = DefaultSearchCenter;
            this.DefaultDistanceKM                = DefaultDistanceKM;


            // Link events...

            #region OnAuthorizeStart

            this.EMPRoaming.OnAuthorizeStart += async (Timestamp,
                                                       Sender,
                                                       Request) => {

                #region Map parameter values

                var OperatorId      = Request.OperatorId.    ToWWCP();
                var Identification  = Request.Identification.ToWWCP();
                var EVSEId          = Request.EVSEId?.       ToWWCP();
                var ProductId       = Request.PartnerProductId.HasValue
                                          ? new ChargingProduct(Request.PartnerProductId.Value.ToWWCP())
                                          : null;
                var SessionId       = Request.SessionId.     ToWWCP();

                #endregion

                if (EVSEId.HasValue)
                {

                    #region Send OnAuthorizeEVSEStartRequest event

                    var StartTime = DateTime.UtcNow;

                    try
                    {

                        OnAuthorizeEVSEStartRequest?.Invoke(StartTime,
                                                            Timestamp,
                                                            this,
                                                            Id.ToString(),
                                                            Request.EventTrackingId,
                                                            RoamingNetwork.Id,
                                                            OperatorId,
                                                            Identification,
                                                            EVSEId.Value,
                                                            ProductId,
                                                            SessionId,
                                                            new ISendAuthorizeStartStop[0],
                                                            Request.RequestTimeout);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeEVSEStartRequest));
                    }

                    #endregion

                    var response = await RoamingNetwork.AuthorizeStart(Identification,
                                                                       EVSEId.Value,
                                                                       ProductId,
                                                                       SessionId,
                                                                       OperatorId,

                                                                       Timestamp,
                                                                       Request.CancellationToken,
                                                                       Request.EventTrackingId,
                                                                       Request.RequestTimeout);

                    #region Send OnAuthorizeEVSEStartResponse event

                    var EndTime = DateTime.UtcNow;

                    try
                    {

                        OnAuthorizeEVSEStartResponse?.Invoke(EndTime,
                                                             Timestamp,
                                                             this,
                                                             Id.ToString(),
                                                             Request.EventTrackingId,
                                                             RoamingNetwork.Id,
                                                             OperatorId,
                                                             Identification,
                                                             EVSEId.Value,
                                                             ProductId,
                                                             SessionId,
                                                             new ISendAuthorizeStartStop[0],
                                                             Request.RequestTimeout,
                                                             response,
                                                             EndTime - StartTime);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeEVSEStartResponse));
                    }

                    #endregion

                    #region Map response

                    if (response != null)
                    {
                        switch (response.Result)
                        {

                            case AuthStartEVSEResultType.Authorized:
                                return CPO.AuthorizationStart.Authorized(Request,
                                                                         response.SessionId. HasValue ? response.SessionId. Value.ToOICP() : default(Session_Id?),
                                                                         default(PartnerSession_Id?),
                                                                         DefaultProviderId,//    response.ProviderId.HasValue ? response.ProviderId.Value.ToOICP() : default(Provider_Id?),
                                                                         "Ready to charge!",
                                                                         null,
                                                                         response.ListOfAuthStopTokens.
                                                                             SafeSelect(token => OICPv2_1.Identification.FromUID(token.ToOICP()))
                                                                        );

                            case AuthStartEVSEResultType.NotAuthorized:
                                return CPO.AuthorizationStart.NotAuthorized(Request,
                                                                            StatusCodes.RFIDAuthenticationfailed_InvalidUID,
                                                                            "RFID Authentication failed - invalid UID");

                            case AuthStartEVSEResultType.InvalidSessionId:
                                return CPO.AuthorizationStart.SessionIsInvalid(Request,
                                                                               SessionId:         Request.SessionId,
                                                                               PartnerSessionId:  Request.PartnerSessionId);

                            case AuthStartEVSEResultType.CommunicationTimeout:
                                return CPO.AuthorizationStart.CommunicationToEVSEFailed(Request);

                            case AuthStartEVSEResultType.StartChargingTimeout:
                                return CPO.AuthorizationStart.NoEVConnectedToEVSE(Request);

                            case AuthStartEVSEResultType.Reserved:
                                return CPO.AuthorizationStart.EVSEAlreadyReserved(Request);

                            case AuthStartEVSEResultType.UnknownEVSE:
                                return CPO.AuthorizationStart.UnknownEVSEID(Request);

                            case AuthStartEVSEResultType.OutOfService:
                                return CPO.AuthorizationStart.EVSEOutOfService(Request);

                        }
                    }

                    #endregion

                    return CPO.AuthorizationStart.ServiceNotAvailable(
                               Request,
                               SessionId:  response?.SessionId. ToOICP() ?? Request.SessionId,
                               ProviderId: response?.ProviderId.ToOICP()
                           );

                }

                else
                {

                    #region Send OnAuthorizeStartRequest event

                    var StartTime = DateTime.UtcNow;

                    try
                    {

                        OnAuthorizeStartRequest?.Invoke(StartTime,
                                                        Timestamp,
                                                        this,
                                                        Id.ToString(),
                                                        Request.EventTrackingId,
                                                        RoamingNetwork.Id,
                                                        OperatorId,
                                                        Identification,
                                                        ProductId,
                                                        SessionId,
                                                        Request.RequestTimeout);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeStartRequest));
                    }

                    #endregion

                    var response = await RoamingNetwork.AuthorizeStart(Identification,
                                                                       ProductId,
                                                                       SessionId,
                                                                       OperatorId,

                                                                       Timestamp,
                                                                       Request.CancellationToken,
                                                                       Request.EventTrackingId,
                                                                       Request.RequestTimeout);


                    #region Send OnAuthorizeStartResponse event

                    var EndTime = DateTime.UtcNow;

                    try
                    {

                        OnAuthorizeStartResponse?.Invoke(EndTime,
                                                         Timestamp,
                                                         this,
                                                         Id.ToString(),
                                                         Request.EventTrackingId,
                                                         RoamingNetwork.Id,
                                                         OperatorId,
                                                         Identification,
                                                         ProductId,
                                                         SessionId,
                                                         Request.RequestTimeout,
                                                         response,
                                                         EndTime - StartTime);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeStartResponse));
                    }

                    #endregion

                    #region Map response

                    if (response != null)
                    {
                        switch (response.Result)
                        {

                            case AuthStartResultType.Authorized:
                                return CPO.AuthorizationStart.Authorized(Request,
                                                                         response.SessionId. HasValue ? response.SessionId. Value.ToOICP() : default(Session_Id?),
                                                                         default(PartnerSession_Id?),
                                                                         DefaultProviderId,//    response.ProviderId.HasValue ? response.ProviderId.Value.ToOICP() : default(Provider_Id?),
                                                                         "Ready to charge!",
                                                                         null,
                                                                         response.ListOfAuthStopTokens.
                                                                             SafeSelect(token => OICPv2_1.Identification.FromUID(token.ToOICP()))
                                                                        );

                            case AuthStartResultType.NotAuthorized:
                                if (Request.Identification.RFIDId != null)
                                    return CPO.AuthorizationStart.NotAuthorized(Request,
                                                                                StatusCodes.RFIDAuthenticationfailed_InvalidUID,
                                                                                "RFID Authentication failed - Invalid UID!");

                                if (Request.Identification.QRCodeIdentification != null)
                                    return CPO.AuthorizationStart.NotAuthorized(Request,
                                                                                StatusCodes.QRCodeAuthenticationFailed_InvalidCredentials,
                                                                                "QR-Code Authentication failed - Invalid credentials!");

                                return CPO.AuthorizationStart.NotAuthorized(Request,
                                                                            StatusCodes.NoPositiveAuthenticationResponse,
                                                                            "No positive authentication response!");


                            case AuthStartResultType.InvalidSessionId:
                                return CPO.AuthorizationStart.SessionIsInvalid(Request,
                                                                               SessionId:         Request.SessionId,
                                                                               PartnerSessionId:  Request.PartnerSessionId);

                            case AuthStartResultType.CommunicationTimeout:
                                return CPO.AuthorizationStart.CommunicationToEVSEFailed(Request);

                            case AuthStartResultType.StartChargingTimeout:
                                return CPO.AuthorizationStart.NoEVConnectedToEVSE(Request);

                            case AuthStartResultType.Reserved:
                                return CPO.AuthorizationStart.EVSEAlreadyReserved(Request);

                            case AuthStartResultType.OutOfService:
                                return CPO.AuthorizationStart.EVSEOutOfService(Request);

                        }
                    }

                    #endregion

                    return CPO.AuthorizationStart.ServiceNotAvailable(
                           Request,
                           SessionId:  response?.SessionId. ToOICP() ?? Request.SessionId,
                           ProviderId: response?.ProviderId.ToOICP()
                       );

                }

            };

            #endregion

            #region OnAuthorizeStop

            this.EMPRoaming.OnAuthorizeStop += async (Timestamp,
                                                      Sender,
                                                      Request) => {

                #region Map parameter values

                var SessionId            = Request.SessionId.     ToWWCP();
                var AuthIdentification   = Request.Identification.ToWWCP();
                var EVSEId               = Request.EVSEId?.       ToWWCP();
                var OperatorId           = Request.OperatorId.    ToWWCP();

                #endregion

                if (EVSEId.HasValue)
                {

                    #region Send OnAuthorizeEVSEStopRequest event

                    var StartTime = DateTime.UtcNow;

                    try
                    {

                        OnAuthorizeEVSEStopRequest?.Invoke(StartTime,
                                                           Timestamp,
                                                           this,
                                                           Id.ToString(),
                                                           Request.EventTrackingId,
                                                           RoamingNetwork.Id,
                                                           OperatorId,
                                                           EVSEId.Value,
                                                           SessionId,
                                                           AuthIdentification,
                                                           Request.RequestTimeout);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeEVSEStopRequest));
                    }

                    #endregion

                    var response = await RoamingNetwork.AuthorizeStop(SessionId,
                                                                      AuthIdentification,
                                                                      EVSEId.Value,
                                                                      OperatorId,

                                                                      Request.Timestamp,
                                                                      Request.CancellationToken,
                                                                      Request.EventTrackingId,
                                                                      Request.RequestTimeout);


                    #region Send OnAuthorizeEVSEStopResponse event

                    var EndTime = DateTime.UtcNow;

                    try
                    {

                        OnAuthorizeEVSEStopResponse?.Invoke(EndTime,
                                                            Timestamp,
                                                            this,
                                                            Id.ToString(),
                                                            Request.EventTrackingId,
                                                            RoamingNetwork.Id,
                                                            OperatorId,
                                                            EVSEId.Value,
                                                            SessionId,
                                                            AuthIdentification,
                                                            Request.RequestTimeout,
                                                            response,
                                                            EndTime - StartTime);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeEVSEStopResponse));
                    }

                    #endregion

                    #region Map response

                    if (response != null)
                    {
                        switch (response.Result)
                        {

                            case AuthStopEVSEResultType.Authorized:
                                return CPO.AuthorizationStop.Authorized(
                                           Request,
                                           response.SessionId. ToOICP(),
                                           null,
                                           response.ProviderId.ToOICP(),
                                           "Ready to stop charging!"
                                       );

                            case AuthStopEVSEResultType.InvalidSessionId:
                                return CPO.AuthorizationStop.SessionIsInvalid(Request);

                            case AuthStopEVSEResultType.CommunicationTimeout:
                                return CPO.AuthorizationStop.CommunicationToEVSEFailed(Request);

                            case AuthStopEVSEResultType.StopChargingTimeout:
                                return CPO.AuthorizationStop.NoEVConnectedToEVSE(Request);

                            case AuthStopEVSEResultType.UnknownEVSE:
                                return CPO.AuthorizationStop.UnknownEVSEID(Request);

                            case AuthStopEVSEResultType.OutOfService:
                                return CPO.AuthorizationStop.EVSEOutOfService(Request);

                        }
                    }

                    #endregion

                    return CPO.AuthorizationStop.ServiceNotAvailable(
                               Request,
                               SessionId:  response?.SessionId. ToOICP() ?? Request.SessionId,
                               ProviderId: response?.ProviderId.ToOICP()
                           );

                }

                else
                {

                    #region Send OnAuthorizeStopRequest event

                    var StartTime = DateTime.UtcNow;

                    try
                    {

                        OnAuthorizeStopRequest?.Invoke(StartTime,
                                                       Timestamp,
                                                       this,
                                                       Id.ToString(),
                                                       Request.EventTrackingId,
                                                       RoamingNetwork.Id,
                                                       OperatorId,
                                                       SessionId,
                                                       AuthIdentification,
                                                       Request.RequestTimeout);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeStopRequest));
                    }

                    #endregion

                    var response = await RoamingNetwork.AuthorizeStop(SessionId,
                                                                      AuthIdentification,
                                                                      OperatorId,

                                                                      Request.Timestamp,
                                                                      Request.CancellationToken,
                                                                      Request.EventTrackingId,
                                                                      Request.RequestTimeout);


                    #region Send OnAuthorizeStopResponse event

                    var EndTime = DateTime.UtcNow;

                    try
                    {

                        OnAuthorizeStopResponse?.Invoke(EndTime,
                                                        Timestamp,
                                                        this,
                                                        Id.ToString(),
                                                        Request.EventTrackingId,
                                                        RoamingNetwork.Id,
                                                        OperatorId,
                                                        SessionId,
                                                        AuthIdentification,
                                                        Request.RequestTimeout,
                                                        response,
                                                        EndTime - StartTime);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeStopResponse));
                    }

                    #endregion

                    #region Map response

                    if (response != null)
                    {
                        switch (response.Result)
                        {

                            case AuthStopResultType.Authorized:
                                return CPO.AuthorizationStop.Authorized(
                                           Request,
                                           response.SessionId. ToOICP(),
                                           null,
                                           response.ProviderId.ToOICP(),
                                           "Ready to stop charging!"
                                       );

                            case AuthStopResultType.InvalidSessionId:
                                return CPO.AuthorizationStop.SessionIsInvalid(Request);

                            case AuthStopResultType.CommunicationTimeout:
                                return CPO.AuthorizationStop.CommunicationToEVSEFailed(Request);

                            case AuthStopResultType.StopChargingTimeout:
                                return CPO.AuthorizationStop.NoEVConnectedToEVSE(Request);

                            case AuthStopResultType.OutOfService:
                                return CPO.AuthorizationStop.EVSEOutOfService(Request);

                        }
                    }

                    #endregion

                    return CPO.AuthorizationStop.ServiceNotAvailable(
                               Request,
                               SessionId:  response?.SessionId. ToOICP() ?? Request.SessionId,
                               ProviderId: response?.ProviderId.ToOICP()
                           );

                }

            };

            #endregion

            #region OnChargeDetailRecord

            this.EMPRoaming.OnChargeDetailRecord += async (Timestamp,
                                                           Sender,
                                                           ChargeDetailRecordRequest) => {

                #region Map parameter values

                var CDRs = new WWCP.ChargeDetailRecord[] { ChargeDetailRecordRequest.ChargeDetailRecord.ToWWCP() };

                #endregion

                #region Send OnChargeDetailRecordRequest event

                var StartTime = DateTime.UtcNow;

                try
                {

                    OnChargeDetailRecordRequest?.Invoke(StartTime,
                                                        Timestamp,
                                                        this,
                                                        Id.ToString(),
                                                        ChargeDetailRecordRequest.EventTrackingId,
                                                        RoamingNetwork.Id,
                                                        CDRs,
                                                        ChargeDetailRecordRequest.RequestTimeout);

                }
                catch (Exception e)
                {
                    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeEVSEStopRequest));
                }

                #endregion


                var response = await RoamingNetwork.SendChargeDetailRecords(CDRs,
                                                                            TransmissionTypes.Direct,

                                                                            ChargeDetailRecordRequest.Timestamp,
                                                                            ChargeDetailRecordRequest.CancellationToken,
                                                                            ChargeDetailRecordRequest.EventTrackingId,
                                                                            ChargeDetailRecordRequest.RequestTimeout);


                #region Send OnChargeDetailRecordResponse event

                var EndTime = DateTime.UtcNow;

                try
                {

                    OnChargeDetailRecordResponse?.Invoke(EndTime,
                                                         Timestamp,
                                                         this,
                                                         Id.ToString(),
                                                         ChargeDetailRecordRequest.EventTrackingId,
                                                         RoamingNetwork.Id,
                                                         CDRs,
                                                         ChargeDetailRecordRequest.RequestTimeout,
                                                         response,
                                                         EndTime - StartTime);

                }
                catch (Exception e)
                {
                    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnChargeDetailRecordResponse));
                }

                #endregion

                #region Map response

                if (response != null)
                {

                    if (response.Result == SendCDRsResultTypes.Success)
                        return Acknowledgement<CPO.SendChargeDetailRecordRequest>.Success(
                                   ChargeDetailRecordRequest,
                                   ChargeDetailRecordRequest.ChargeDetailRecord.SessionId,
                                   ChargeDetailRecordRequest.ChargeDetailRecord.PartnerSessionId,
                                   "Charge detail record forwarded!"
                               );

                    var FailedCDR = response.RejectedChargeDetailRecords.FirstOrDefault();

                    if (FailedCDR != null)
                    {
                        switch (FailedCDR.Result)
                        {

                            //case SendCDRResultTypes.NotForwared:
                            //    return Acknowledgement<CPO.SendChargeDetailRecordRequest>.SystemError(
                            //               ChargeDetailRecordRequest,
                            //               "Communication to EVSE failed!",
                            //               SessionId:         ChargeDetailRecordRequest.ChargeDetailRecord.SessionId,
                            //               PartnerSessionId:  ChargeDetailRecordRequest.ChargeDetailRecord.PartnerSessionId
                            //           );

                            case SendCDRResultTypes.InvalidSessionId:
                                return Acknowledgement<CPO.SendChargeDetailRecordRequest>.SessionIsInvalid(
                                           ChargeDetailRecordRequest,
                                           SessionId:         ChargeDetailRecordRequest.ChargeDetailRecord.SessionId,
                                           PartnerSessionId:  ChargeDetailRecordRequest.ChargeDetailRecord.PartnerSessionId
                                       );

                            case SendCDRResultTypes.UnknownEVSE:
                                return Acknowledgement<CPO.SendChargeDetailRecordRequest>.UnknownEVSEID(
                                           ChargeDetailRecordRequest,
                                           SessionId:         ChargeDetailRecordRequest.ChargeDetailRecord.SessionId,
                                           PartnerSessionId:  ChargeDetailRecordRequest.ChargeDetailRecord.PartnerSessionId
                                       );

                            case SendCDRResultTypes.Error:
                                return Acknowledgement<CPO.SendChargeDetailRecordRequest>.DataError(
                                           ChargeDetailRecordRequest,
                                           SessionId:         ChargeDetailRecordRequest.ChargeDetailRecord.SessionId,
                                           PartnerSessionId:  ChargeDetailRecordRequest.ChargeDetailRecord.PartnerSessionId
                                       );

                        }
                    }

                }

                #endregion

                return Acknowledgement<CPO.SendChargeDetailRecordRequest>.ServiceNotAvailable(
                           ChargeDetailRecordRequest,
                           SessionId: ChargeDetailRecordRequest.ChargeDetailRecord.SessionId
                       );

            };

            #endregion

        }

        #endregion

        #region WWCPEMPAdapter(Id, Name, RoamingNetwork, EMPClient, EMPServer, Context = EMPRoaming.DefaultLoggingContext, LogfileCreator = null)

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
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record after receiving it from the roaming provider.</param>
        public WWCPEMPAdapter(EMPRoamingProvider_Id        Id,
                              I18NString                   Name,
                              RoamingNetwork               RoamingNetwork,

                              EMPClient                    EMPClient,
                              EMPServer                    EMPServer,
                              String                       ServerLoggingContext              = EMPServerLogger.DefaultContext,
                              LogfileCreatorDelegate       LogfileCreator                    = null,

                              EVSEDataRecord2EVSEDelegate  EVSEDataRecord2EVSE               = null,

                              EVSEOperatorFilterDelegate   EVSEOperatorFilter                = null,

                              TimeSpan?                    PullDataServiceEvery              = null,
                              Boolean                      DisablePullData                   = false,
                              TimeSpan?                    PullDataServiceRequestTimeout     = null,

                              TimeSpan?                    PullStatusServiceEvery            = null,
                              Boolean                      DisablePullStatus                 = false,
                              TimeSpan?                    PullStatusServiceRequestTimeout   = null,

                              eMobilityProvider            DefaultProvider                   = null,
                              GeoCoordinate?               DefaultSearchCenter               = null,
                              UInt64?                      DefaultDistanceKM                 = null)

            : this(Id,
                   Name,
                   RoamingNetwork,

                   new EMPRoaming(EMPClient,
                                  EMPServer,
                                  ServerLoggingContext,
                                  LogfileCreator),

                   EVSEDataRecord2EVSE,

                   EVSEOperatorFilter,

                   PullDataServiceEvery,
                   DisablePullData,
                   PullDataServiceRequestTimeout,

                   PullStatusServiceEvery,
                   DisablePullStatus,
                   PullStatusServiceRequestTimeout,

                   DefaultProvider,
                   DefaultSearchCenter,
                   DefaultDistanceKM)

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
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerCertificateSelector">An optional delegate to select a SSL/TLS server certificate.</param>
        /// <param name="RemoteClientCertificateValidator">An optional delegate to verify the SSL/TLS client certificate used for authentication.</param>
        /// <param name="RemoteClientCertificateSelector">An optional delegate to select the SSL/TLS client certificate used for authentication.</param>
        /// <param name="AllowedTLSProtocols">The SSL/TLS protocol(s) allowed for this connection.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerContentType">An optional HTTP content type to use.</param>
        /// <param name="ServerRegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record after receiving it from the roaming provider.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public WWCPEMPAdapter(EMPRoamingProvider_Id                Id,
                              I18NString                           Name,
                              RoamingNetwork                       RoamingNetwork,

                              String                               RemoteHostname,
                              IPPort                               RemoteTCPPort                      = null,
                              RemoteCertificateValidationCallback  RemoteCertificateValidator         = null,
                              LocalCertificateSelectionCallback    ClientCertificateSelector          = null,
                              String                               RemoteHTTPVirtualHost              = null,
                              String                               URIPrefix                          = EMPClient.DefaultURIPrefix,
                              String                               EVSEDataURI                        = EMPClient.DefaultEVSEDataURI,
                              String                               EVSEStatusURI                      = EMPClient.DefaultEVSEStatusURI,
                              String                               AuthenticationDataURI              = EMPClient.DefaultAuthenticationDataURI,
                              String                               ReservationURI                     = EMPClient.DefaultReservationURI,
                              String                               AuthorizationURI                   = EMPClient.DefaultAuthorizationURI,

                              Provider_Id?                         DefaultProviderId                  = null,

                              String                               HTTPUserAgent                      = EMPClient.DefaultHTTPUserAgent,
                              TimeSpan?                            RequestTimeout                     = null,
                              Byte?                                MaxNumberOfRetries                 = EMPClient.DefaultMaxNumberOfRetries,

                              String                               ServerName                         = EMPServer.DefaultHTTPServerName,
                              String                               ServiceId                          = null,
                              IPPort                               ServerTCPPort                      = null,
                              ServerCertificateSelectorDelegate    ServerCertificateSelector          = null,
                              RemoteCertificateValidationCallback  RemoteClientCertificateValidator   = null,
                              LocalCertificateSelectionCallback    RemoteClientCertificateSelector    = null,
                              SslProtocols                         AllowedTLSProtocols                = SslProtocols.Tls12,
                              String                               ServerURIPrefix                    = EMPServer.DefaultURIPrefix,
                              String                               ServerAuthorizationURI             = EMPServer.DefaultAuthorizationURI,
                              HTTPContentType                      ServerContentType                  = null,
                              Boolean                              ServerRegisterHTTPRootService      = true,
                              Boolean                              ServerAutoStart                    = false,

                              String                               ClientLoggingContext               = EMPClient.EMPClientLogger.DefaultContext,
                              String                               ServerLoggingContext               = EMPServerLogger.DefaultContext,
                              LogfileCreatorDelegate               LogfileCreator                     = null,

                              EVSEDataRecord2EVSEDelegate          EVSEDataRecord2EVSE                = null,

                              EVSEOperatorFilterDelegate           EVSEOperatorFilter                 = null,

                              TimeSpan?                            PullDataServiceEvery               = null,
                              Boolean                              DisablePullData                    = false,
                              TimeSpan?                            PullDataServiceRequestTimeout      = null,

                              TimeSpan?                            PullStatusServiceEvery             = null,
                              Boolean                              DisablePullStatus                  = false,
                              TimeSpan?                            PullStatusServiceRequestTimeout    = null,

                              eMobilityProvider                    DefaultProvider                    = null,
                              GeoCoordinate?                       DefaultSearchCenter                = null,
                              UInt64?                              DefaultDistanceKM                  = null,

                              DNSClient                            DNSClient                          = null)

            : this(Id,
                   Name,
                   RoamingNetwork,

                   new EMPRoaming(Id.ToString(),
                                  RemoteHostname,
                                  RemoteTCPPort,
                                  RemoteCertificateValidator,
                                  ClientCertificateSelector,
                                  RemoteHTTPVirtualHost,
                                  URIPrefix,
                                  EVSEDataURI,
                                  EVSEStatusURI,
                                  AuthenticationDataURI,
                                  ReservationURI,
                                  AuthorizationURI,
                                  DefaultProviderId,

                                  HTTPUserAgent,
                                  RequestTimeout,
                                  MaxNumberOfRetries,

                                  ServerName,
                                  ServiceId,
                                  ServerTCPPort,
                                  ServerCertificateSelector,
                                  RemoteClientCertificateValidator,
                                  RemoteClientCertificateSelector,
                                  AllowedTLSProtocols,
                                  ServerURIPrefix,
                                  ServerAuthorizationURI,
                                  ServerContentType,
                                  ServerRegisterHTTPRootService,
                                  false,

                                  ClientLoggingContext,
                                  ServerLoggingContext,
                                  LogfileCreator,

                                  DNSClient),

                   EVSEDataRecord2EVSE,
                   EVSEOperatorFilter,

                   PullDataServiceEvery,
                   DisablePullData,
                   PullDataServiceRequestTimeout,

                   PullStatusServiceEvery,
                   DisablePullStatus,
                   PullStatusServiceRequestTimeout,

                   DefaultProvider,
                   DefaultSearchCenter,
                   DefaultDistanceKM)

        {

            if (ServerAutoStart)
                EMPServer.Start();

        }

        #endregion

        #endregion


        // Outgoing EMPClient requests...

        #region PullEVSEData  (SearchCenter = null, DistanceKM = 0.0, LastCall = null, ProviderId = null, ...)

        /// <summary>
        /// Create a new task querying EVSE data from the OICP server.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="LastCall">An optional timestamp of the last call.</param>
        /// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
        /// <param name="OperatorIdFilter">Only return EVSEs belonging to the given optional enumeration of EVSE operators.</param>
        /// <param name="CountryCodeFilter">An optional enumeration of countries whose EVSE's a provider wants to retrieve.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<EVSEDataPull>

            PullEVSEData(DateTime?                                LastCall            = null,
                         GeoCoordinate?                           SearchCenter        = null,
                         Single                                   DistanceKM          = 0f,
                         eMobilityProvider_Id?                    ProviderId          = null,
                         IEnumerable<ChargingStationOperator_Id>  OperatorIdFilter    = null,
                         IEnumerable<Country>                     CountryCodeFilter   = null,

                         DateTime?                                Timestamp           = null,
                         CancellationToken?                       CancellationToken   = null,
                         EventTracking_Id                         EventTrackingId     = null,
                         TimeSpan?                                RequestTimeout      = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;

            #endregion

            var result = await EMPRoaming.PullEVSEData(ProviderId.HasValue
                                                           ? ProviderId.Value.ToOICP()
                                                           : DefaultProviderId.Value,
                                                       SearchCenter,
                                                       DistanceKM,
                                                       LastCall,
                                                       GeoCoordinatesResponseFormats.DecimalDegree,
                                                       OperatorIdFilter != null
                                                           ? OperatorIdFilter.Select(csoid => csoid.ToOICP())
                                                           : null,
                                                       CountryCodeFilter,

                                                       Timestamp,
                                                       CancellationToken,
                                                       EventTrackingId,
                                                       RequestTimeout).
                                          ConfigureAwait(false);


            if (result.HTTPStatusCode == HTTPStatusCode.OK &&
                result.Content        != null)
            {

                var WWCPEVSEs = new List<EVSE>();
                var Warnings  = new List<String>();
                EVSE _EVSE    = null;

                foreach (var evseoperator in result.Content.EVSEData.OperatorEVSEData)
                    foreach (var evse in evseoperator.EVSEDataRecords)
                    {

                        try
                        {

                            _EVSE = evse.ToWWCP();

                            if (_EVSE != null)
                                WWCPEVSEs.Add(_EVSE);

                            else
                                Warnings.Add("Could not convert EVSE '" + evse.Id + "'!");

                        }
                        catch (Exception e)
                        {
                            Warnings.Add(evse.Id.ToString() + " - " + e.Message);
                        }

                    }


            }

            return new EVSEDataPull(new EVSE[0],
                                    result.HTTPStatusCode +
                                    (result.ContentLength.HasValue && result.ContentLength.Value > 0
                                         ? Environment.NewLine + result.HTTPBody.ToUTF8String()
                                         : ""));

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
        public async Task<EVSEStatusPull>

            PullEVSEStatus(DateTime?              LastCall            = null,
                           GeoCoordinate?         SearchCenter        = null,
                           Single                 DistanceKM          = 0f,
                           WWCP.EVSEStatusTypes?  EVSEStatusFilter    = null,
                           eMobilityProvider_Id?  ProviderId          = null,

                           DateTime?              Timestamp           = null,
                           CancellationToken?     CancellationToken   = null,
                           EventTracking_Id       EventTrackingId     = null,
                           TimeSpan?              RequestTimeout      = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;

            #endregion

            #region Send OnPushAuthenticationDataRequest event

            //var StartTime = DateTime.UtcNow;

            //try
            //{

            //    OnPullEVSEDataRequest?.Invoke(StartTime,
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


            var result = await EMPRoaming.PullEVSEStatus(ProviderId.HasValue
                                                             ? ProviderId.Value.ToOICP()
                                                             : DefaultProviderId.Value,
                                                         SearchCenter,
                                                         DistanceKM,
                                                         EVSEStatusFilter.HasValue
                                                             ? new OICPv2_1.EVSEStatusTypes?(EVSEStatusFilter.Value.AsOICPEVSEStatus())
                                                             : null,

                                                         Timestamp,
                                                         CancellationToken,
                                                         EventTrackingId,
                                                         RequestTimeout).
                                          ConfigureAwait(false);

            if (result.HTTPStatusCode == HTTPStatusCode.OK &&
                result.Content        != null)
            {

                var EVSEStatusList = new List<WWCP.EVSEStatus>();
                var Warnings       = new List<String>();
                WWCP.EVSE_Id? EVSEId = null;

                foreach (var operatorevsestatus in result.Content.OperatorEVSEStatus)
                    foreach (var evsestatusrecord in operatorevsestatus.EVSEStatusRecords)
                    {

                        EVSEId = evsestatusrecord.Id.ToWWCP();

                        if (EVSEId.HasValue)
                            EVSEStatusList.Add(new WWCP.EVSEStatus(EVSEId.Value,
                                                                   new Timestamped<WWCP.EVSEStatusTypes>(
                                                                       result.Timestamp,
                                                                       OICPMapper.AsWWCPEVSEStatus(evsestatusrecord.Status)
                                                                   ),
                                                                   evsestatusrecord.CustomData));

                        else
                            Warnings.Add("Invalid EVSE identification '" + evsestatusrecord.Id + "'!");

                    }

                return new EVSEStatusPull(EVSEStatusList, Warnings);

            }

            return new EVSEStatusPull(new WWCP.EVSEStatus[0],
                                      new String[] {
                                          result.HTTPStatusCode.ToString(),
                                          result.HTTPBody.ToUTF8String()
                                      });

        }

        #endregion

        #region PullEVSEStatusById(EVSEId,  ProviderId = null, ...)

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
        public async Task<EVSEStatusPull>

            PullEVSEStatusById(WWCP.EVSE_Id           EVSEId,
                               eMobilityProvider_Id?  ProviderId          = null,

                               DateTime?              Timestamp           = null,
                               CancellationToken?     CancellationToken   = null,
                               EventTracking_Id       EventTrackingId     = null,
                               TimeSpan?              RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEId == null)
                return new EVSEStatusPull(new WWCP.EVSEStatus[0],
                                          new String[] { "Parameter 'EVSEId' was null!" });

            #endregion

            return await PullEVSEStatusById(new WWCP.EVSE_Id[] { EVSEId },
                                            ProviderId,

                                            Timestamp,
                                            CancellationToken,
                                            EventTrackingId,
                                            RequestTimeout);

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
        public async Task<EVSEStatusPull>

            PullEVSEStatusById(IEnumerable<WWCP.EVSE_Id>  EVSEIds,
                               eMobilityProvider_Id?      ProviderId          = null,

                               DateTime?                  Timestamp           = null,
                               CancellationToken?         CancellationToken   = null,
                               EventTracking_Id           EventTrackingId     = null,
                               TimeSpan?                  RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEIds.IsNullOrEmpty())
                return new EVSEStatusPull(new WWCP.EVSEStatus[0],
                                          new String[] { "Parameter 'EVSEIds' was null or empty!" });

            #endregion


            var EVSEStatusList  = new List<WWCP.EVSEStatus>();
            var Warnings        = new List<String>();

            // Hubject has a limit of 100 EVSEIds per request!
            // Do not make concurrent requests!
            foreach (var evsepart in EVSEIds.Select(evse => evse.ToOICP()).
                                             Where (evse => evse.HasValue).
                                             Select(evse => evse.Value).
                                             ToPartitions(100))
            {

                var result = await EMPRoaming.PullEVSEStatusById(evsepart,
                                                                 ProviderId.HasValue
                                                                     ? ProviderId.Value.ToOICP()
                                                                     : DefaultProviderId.Value,

                                                                 Timestamp,
                                                                 CancellationToken,
                                                                 EventTrackingId,
                                                                 RequestTimeout).
                                              ConfigureAwait(false);

                if (result.HTTPStatusCode == HTTPStatusCode.OK &&
                    result.Content != null)
                {

                    WWCP.EVSE_Id? EVSEId = null;

                    foreach (var evsestatusrecord in result.Content.EVSEStatusRecords)
                    {

                        EVSEId = evsestatusrecord.Id.ToWWCP();

                        if (EVSEId.HasValue)
                            EVSEStatusList.Add(new WWCP.EVSEStatus(EVSEId.Value,
                                                                   new Timestamped<WWCP.EVSEStatusTypes>(
                                                                       result.Timestamp,
                                                                       OICPMapper.AsWWCPEVSEStatus(evsestatusrecord.Status)
                                                                   )));

                        else
                            Warnings.Add("Invalid EVSE identification '" + evsestatusrecord.Id + "'!");

                    }

                }

                else
                    Warnings.AddRange(new String[] {
                                          result.HTTPStatusCode.ToString(),
                                          result.HTTPBody.ToUTF8String()
                                      });

            }

            return new EVSEStatusPull(EVSEStatusList, Warnings);

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
        public async Task<PushAuthenticationDataResult>

            PushAuthenticationData(IEnumerable<Identification>  AuthorizationIdentifications,
                                   ActionType                   Action              = ActionType.fullLoad,
                                   eMobilityProvider_Id?        ProviderId          = null,

                                   DateTime?                    Timestamp           = null,
                                   CancellationToken?           CancellationToken   = null,
                                   EventTracking_Id             EventTrackingId     = null,
                                   TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthorizationIdentifications.IsNullOrEmpty())
                return PushAuthenticationDataResult.NoOperation(Id, this);


            PushAuthenticationDataResult result = null;

            #endregion

            #region Send OnPushAuthenticationDataRequest event

            //var StartTime = DateTime.UtcNow;

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
                                                                   ProviderId.HasValue
                                                                       ? ProviderId.Value.ToOICP()
                                                                       : DefaultProviderId.Value,
                                                                   Action.    ToOICP(),

                                                                   Timestamp,
                                                                   CancellationToken,
                                                                   EventTrackingId,
                                                                   RequestTimeout).
                                            ConfigureAwait(false);

            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        != null)
            {

                result = response.Content.Result

                             ? PushAuthenticationDataResult.Success(Id,
                                                                    this,
                                                                    response.Content.StatusCode.Description,
                                                                    response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                        ? new String[] { response.Content.StatusCode.AdditionalInfo }
                                                                        : null)

                             : PushAuthenticationDataResult.Error(Id,
                                                                  this,
                                                                  null,
                                                                  response.Content.StatusCode.Description,
                                                                  response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                      ? new String[] { response.Content.StatusCode.AdditionalInfo }
                                                                      : null);

            }

            else
                result = PushAuthenticationDataResult.Error(Id,
                                                            this,
                                                            null,
                                                            response.Content != null
                                                                ? response.Content.StatusCode.Description
                                                                : null,
                                                            response.Content != null
                                                                ? response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                      ? new String[] { response.Content.StatusCode.AdditionalInfo }
                                                                      : null
                                                                : null);


            #region Send OnPushAuthenticationDataResponse event

            //var Endtime = DateTime.UtcNow;
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


        #region Reserve(EVSEId, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be reserved.</param>
        /// <param name="ReservationStartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
        /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProduct">The charging product to be reserved.</param>
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
                                            DateTime?                         ReservationStartTime,
                                            TimeSpan?                         Duration,
                                            ChargingReservation_Id?           ReservationId,
                                            eMobilityProvider_Id?             ProviderId,
                                            AuthIdentification                Identification,
                                            ChargingProduct                   ChargingProduct,
                                            IEnumerable<Auth_Token>           AuthTokens,
                                            IEnumerable<eMobilityAccount_Id>  eMAIds,
                                            IEnumerable<UInt32>               PINs,

                                            DateTime?                         Timestamp,
                                            CancellationToken?                CancellationToken,
                                            EventTracking_Id                  EventTrackingId,
                                            TimeSpan?                         RequestTimeout)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;


            ReservationResult result = null;

            #endregion

            #region Send OnReserveEVSERequest event

            var StartTime = DateTime.UtcNow;

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
                                             Identification,
                                             ChargingProduct,
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

            if (ChargingProduct != null)
                PartnerProductIdElements.Add("P", ChargingProduct.Id.ToString());

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

            if (eMAIds == null && Identification?.RemoteIdentification.HasValue == true)
                eMAIds = new List<eMobilityAccount_Id> { Identification.RemoteIdentification.Value };

            if (eMAIds != null && Identification?.RemoteIdentification.HasValue == true && !eMAIds.Contains(Identification.RemoteIdentification.Value))
            {
                var _eMAIds = new List<eMobilityAccount_Id>(eMAIds);
                _eMAIds.Add(Identification.RemoteIdentification.Value);
                eMAIds = _eMAIds;
            }

            #endregion


            var response = await EMPRoaming.ReservationStart(EVSEId:             EVSEId.ToOICP().Value,
                                                             ProviderId:         ProviderId.HasValue
                                                                                     ? ProviderId.Value.ToOICP()
                                                                                     : DefaultProviderId.Value,
                                                             Identification:     Identification.ToOICP(),
                                                             Duration:           Duration,
                                                             SessionId:          ReservationId != null ? Session_Id.Parse(ReservationId.ToString()) : new Session_Id?(),
                                                             PartnerSessionId:   null,
                                                             PartnerProductId:   PartnerProductIdElements.Count > 0
                                                                                     ? PartnerProduct_Id.Parse(PartnerProductIdElements.
                                                                                                                   Select(kvp => kvp.Key + "=" + kvp.Value).
                                                                                                                   AggregateWith("|"))
                                                                                     : default(PartnerProduct_Id?),

                                                             Timestamp:          Timestamp,
                                                             CancellationToken:  CancellationToken,
                                                             EventTrackingId:    EventTrackingId,
                                                             RequestTimeout:     RequestTimeout).
                                            ConfigureAwait(false);


            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        != null              &&
                response.Content.Result)
            {

                result = ReservationResult.Success(response.Content.SessionId != null
                                                   ? new ChargingReservation(ReservationId:            ChargingReservation_Id.Parse(EVSEId.OperatorId.ToString() +
                                                                                                           "*R" + response.Content.SessionId.ToString()),
                                                                             Timestamp:                DateTime.UtcNow,
                                                                             StartTime:                DateTime.UtcNow,
                                                                             Duration:                 Duration.HasValue ? Duration.Value : DefaultReservationTime,
                                                                             EndTime:                  DateTime.UtcNow + (Duration.HasValue ? Duration.Value : DefaultReservationTime),
                                                                             ConsumedReservationTime:  TimeSpan.FromSeconds(0),
                                                                             ReservationLevel:         ChargingReservationLevel.EVSE,
                                                                             ProviderId:               ProviderId,
                                                                             Identification:           Identification,
                                                                             RoamingNetwork:           RoamingNetwork,
                                                                             ChargingPoolId:           null,
                                                                             ChargingStationId:        null,
                                                                             EVSEId:                   EVSEId,
                                                                             ChargingProduct:          ChargingProduct,
                                                                             AuthTokens:               AuthTokens,
                                                                             eMAIds:                   eMAIds,
                                                                             PINs:                     PINs)
                                                   : null);

            }

            else
                result = ReservationResult.Error(response.HTTPStatusCode.ToString(),
                                                 response);


            #region Send OnReserveEVSEResponse event

            var EndTime = DateTime.UtcNow;

            try
            {

                OnReserveEVSEResponse?.Invoke(EndTime,
                                              Timestamp.Value,
                                              this,
                                              EventTrackingId,
                                              RoamingNetwork.Id,
                                              ReservationId,
                                              EVSEId,
                                              ReservationStartTime,
                                              Duration,
                                              ProviderId,
                                              Identification,
                                              ChargingProduct,
                                              AuthTokens,
                                              eMAIds,
                                              PINs,
                                              result,
                                              EndTime - StartTime,
                                              RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnReserveEVSEResponse));
            }

            #endregion

            return result;

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

            if (!EVSEId.HasValue)
                throw new ArgumentNullException(nameof(EVSEId),  "The EVSE identification is mandatory in OICP!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;

            #endregion

            #region Send OnCancelReservationRequest event

            var StartTime = DateTime.UtcNow;

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


            var result = await EMPRoaming.ReservationStop(SessionId:          Session_Id.Parse(ReservationId.Suffix),
                                                          ProviderId:         ProviderId.HasValue
                                                                                  ? ProviderId.Value.ToOICP()
                                                                                  : DefaultProviderId.Value,
                                                          EVSEId:             EVSEId.Value.ToOICP().Value,
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

                return CancelReservationResult.Success(ReservationId,
                                                       Reason);

            }

            return CancelReservationResult.Error(ReservationId,
                                                 Reason,
                                                 result.HTTPStatusCode.ToString(),
                                                 result.EntirePDU);

        }

        #endregion


        #region RemoteStart(EVSEId,            ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be started remotely.</param>
        /// <param name="ChargingProduct">The charging product to use.</param>
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
                                                ChargingProduct          ChargingProduct,  // = null,
                                                ChargingReservation_Id?  ReservationId,    // = null,
                                                ChargingSession_Id?      SessionId,        // = null,
                                                eMobilityProvider_Id?    ProviderId,       // = null,
                                                eMobilityAccount_Id?     eMAId,            // = null,

                                                DateTime?                Timestamp,
                                                CancellationToken?       CancellationToken,
                                                EventTracking_Id         EventTrackingId,
                                                TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (!eMAId.HasValue)
                throw new ArgumentNullException(nameof(eMAId),  "The e-mobility account identification is mandatory in OICP!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;


            RemoteStartEVSEResult result = null;

            #endregion

            #region Send OnRemoteStartEVSERequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnRemoteStartEVSERequest?.Invoke(StartTime,
                                                 Timestamp.Value,
                                                 this,
                                                 EventTrackingId,
                                                 RoamingNetwork.Id,
                                                 EVSEId,
                                                 ChargingProduct,
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

            if (ChargingProduct != null)
                PartnerProductIdElements.Add("P", ChargingProduct.Id.ToString());

            #endregion

            #region Copy the 'PlannedDuration' value into the PartnerProductId "D=...min"

            //if (PlannedDuration.HasValue && PlannedDuration.Value >= TimeSpan.FromSeconds(1))
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

            #region Copy the 'PlannedEnergy' value into the PartnerProductId

            //if (PlannedEnergy.HasValue && PlannedEnergy.Value > 0))
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
                    PartnerProductIdElements.Add("R", ReservationId.Value.Suffix);
                else
                    PartnerProductIdElements["R"] = ReservationId.Value.Suffix;

            }

            #endregion


            var response = await EMPRoaming.RemoteStart(ProviderId:         ProviderId.HasValue
                                                                                ? ProviderId.Value.ToOICP()
                                                                                : DefaultProviderId.Value,
                                                        EVSEId:             EVSEId.ToOICP().Value,
                                                        EVCOId:             eMAId.     Value.ToOICP(),
                                                        SessionId:          SessionId.       ToOICP(),
                                                        PartnerSessionId:   null,
                                                        PartnerProductId:   PartnerProductIdElements.Count > 0
                                                                                ? new PartnerProduct_Id?(PartnerProduct_Id.Parse(PartnerProductIdElements.
                                                                                                                                 Select(kvp => kvp.Key + "=" + kvp.Value).
                                                                                                                                 AggregateWith("|")))
                                                                                : null,

                                                        Timestamp:          Timestamp,
                                                        CancellationToken:  CancellationToken,
                                                        EventTrackingId:    EventTrackingId,
                                                        RequestTimeout:     RequestTimeout
                                                        ).
                                            ConfigureAwait(false);


            var Now     = DateTime.UtcNow;
            var Runtime = Now - Timestamp.Value;

            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        != null              &&
                response.Content.Result)
            {

                result = RemoteStartEVSEResult.Success(response.Content.SessionId.HasValue
                                                           ? new ChargingSession(response.Content.SessionId.ToWWCP().Value)
                                                           : null);

            }

            else
                result = RemoteStartEVSEResult.Error(response.HTTPStatusCode.ToString(),
                                                     response);


            #region Send OnRemoteStartEVSEResponse event

            var EndTime = DateTime.UtcNow;

            try
            {

                OnRemoteStartEVSEResponse?.Invoke(EndTime,
                                                  Timestamp.Value,
                                                  this,
                                                  EventTrackingId,
                                                  RoamingNetwork.Id,
                                                  EVSEId,
                                                  ChargingProduct,
                                                  ReservationId,
                                                  SessionId,
                                                  ProviderId,
                                                  eMAId,
                                                  RequestTimeout,
                                                  result,
                                                  EndTime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnRemoteStartEVSEResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStart(ChargingStationId, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session at the given charging station.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station to be started.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
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

            IReserveRemoteStartStop.RemoteStart(WWCP.ChargingStation_Id  ChargingStationId,
                                                ChargingProduct          ChargingProduct,  // = null,
                                                ChargingReservation_Id?  ReservationId,    // = null,
                                                ChargingSession_Id?      SessionId,        // = null,
                                                eMobilityProvider_Id?    ProviderId,       // = null,
                                                eMobilityAccount_Id?     eMAId,            // = null,

                                                DateTime?                Timestamp,
                                                CancellationToken?       CancellationToken,
                                                EventTracking_Id         EventTrackingId,
                                                TimeSpan?                RequestTimeout)


                => Task.FromResult(RemoteStartChargingStationResult.OutOfService);

        #endregion


        #region RemoteStop(                   SessionId, ReservationHandling = null, ProviderId = null, eMAId = null, ...)

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
                                               ReservationHandling?   ReservationHandling,
                                               eMobilityProvider_Id?  ProviderId,         // = null,
                                               eMobilityAccount_Id?   eMAId,              // = null,

                                               DateTime?              Timestamp,
                                               CancellationToken?     CancellationToken,
                                               EventTracking_Id       EventTrackingId,
                                               TimeSpan?              RequestTimeout)


                => Task.FromResult(RemoteStopResult.OutOfService(SessionId));

        #endregion

        #region RemoteStop(EVSEId,            SessionId, ReservationHandling = null, ProviderId = null, eMAId = null, ...)

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
                       ReservationHandling?   ReservationHandling   = null,
                       eMobilityProvider_Id?  ProviderId            = null,
                       eMobilityAccount_Id?   eMAId                 = null,

                       DateTime?              Timestamp             = null,
                       CancellationToken?     CancellationToken     = null,
                       EventTracking_Id       EventTrackingId       = null,
                       TimeSpan?              RequestTimeout        = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;


            RemoteStopEVSEResult result = null;

            #endregion

            #region Send OnRemoteStopEVSERequest event

            var StartTime = DateTime.UtcNow;

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


            var response = await EMPRoaming.RemoteStop(SessionId:          SessionId.       ToOICP(),
                                                       ProviderId:         ProviderId.HasValue
                                                                                ? ProviderId.Value.ToOICP()
                                                                                : DefaultProviderId.Value,
                                                       EVSEId:             EVSEId.          ToOICP().Value,
                                                       PartnerSessionId:   null,

                                                       Timestamp:          Timestamp,
                                                       CancellationToken:  CancellationToken,
                                                       EventTrackingId:    EventTrackingId,
                                                       RequestTimeout:     RequestTimeout).
                                            ConfigureAwait(false);

            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        != null              &&
                response.Content.Result)
            {

                result = RemoteStopEVSEResult.Success(SessionId);

            }

            else
                result = RemoteStopEVSEResult.Error(SessionId,
                                                    response.HTTPStatusCode.ToString(),
                                                    response);


            #region Send OnRemoteStopEVSEResponse event

            var EndTime = DateTime.UtcNow;

            try
            {

                OnRemoteStopEVSEResponse?.Invoke(EndTime,
                                                 Timestamp.Value,
                                                 this,
                                                 EventTrackingId,
                                                 RoamingNetwork.Id,
                                                 EVSEId,
                                                 SessionId,
                                                 ReservationHandling,
                                                 ProviderId,
                                                 eMAId,
                                                 RequestTimeout,
                                                 result,
                                                 EndTime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnRemoteStopEVSEResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop(ChargingStationId, SessionId, ReservationHandling = null, ProviderId = null, eMAId = null, ...)

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

            IReserveRemoteStartStop.RemoteStop(WWCP.ChargingStation_Id  ChargingStationId,
                                               ChargingSession_Id       SessionId,
                                               ReservationHandling?     ReservationHandling,
                                               eMobilityProvider_Id?    ProviderId,         // = null,
                                               eMobilityAccount_Id?     eMAId,              // = null,

                                               DateTime?                Timestamp,
                                               CancellationToken?       CancellationToken,
                                               EventTracking_Id         EventTrackingId,
                                               TimeSpan?                RequestTimeout)


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
        public async Task<IEnumerable<WWCP.ChargeDetailRecord>>

            GetChargeDetailRecords(DateTime               From,
                                   DateTime?              To                  = null,
                                   eMobilityProvider_Id?  ProviderId          = null,

                                   DateTime?              Timestamp           = null,
                                   CancellationToken?     CancellationToken   = null,
                                   EventTracking_Id       EventTrackingId     = null,
                                   TimeSpan?              RequestTimeout      = null)

        {

            #region Initial checks

            if (!To.HasValue)
                To = DateTime.UtcNow;


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;


            IEnumerable<WWCP.ChargeDetailRecord> result = null;

            #endregion

            #region Send OnGetChargeDetailRecordsRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnGetChargeDetailRecordsRequest?.Invoke(StartTime,
                                                        Timestamp.Value,
                                                        this,
                                                        Id.ToString(),
                                                        EventTrackingId,
                                                        RoamingNetwork.Id,
                                                        From,
                                                        To,
                                                        ProviderId,
                                                        RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnGetChargeDetailRecordsRequest));
            }

            #endregion


            var response = await EMPRoaming.GetChargeDetailRecords(ProviderId.HasValue
                                                                       ? ProviderId.Value.ToOICP()
                                                                       : DefaultProviderId.Value,
                                                                   From,
                                                                   To.Value,

                                                                   Timestamp,
                                                                   CancellationToken,
                                                                   EventTrackingId,
                                                                   RequestTimeout).
                                            ConfigureAwait(false);

            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        != null)
            {

                var Warnings = new List<String>();

                result = response.Content.
                             ChargeDetailRecords.
                             SafeSelect(cdr => {

                                                   try
                                                   {

                                                       return cdr.ToWWCP();

                                                   }
                                                   catch (Exception e)
                                                   {
                                                       Warnings.Add("Error during import of charge detail record: " + e.Message);
                                                       return null;
                                                   }

                                               }).
                             SafeWhere(cdr => cdr != null);

            }

            else
                result = new WWCP.ChargeDetailRecord[0];


            #region Send OnGetChargeDetailRecordsResponse event

            var EndTime = DateTime.UtcNow;

            try
            {

                OnGetChargeDetailRecordsResponse?.Invoke(StartTime,
                                                         Timestamp.Value,
                                                         this,
                                                         Id.ToString(),
                                                         EventTrackingId,
                                                         RoamingNetwork.Id,
                                                         From,
                                                         To,
                                                         ProviderId,
                                                         RequestTimeout,
                                                         result,
                                                         EndTime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnGetChargeDetailRecordsResponse));
            }

            #endregion

            return result;

        }

        #endregion


        // -----------------------------------------------------------------------------------------------------

        #region (timer) PullDataService(State)

        private void PullDataService(Object State)
        {

            if (!DisablePullData)
            {

                try
                {

                    PullData().Wait();

                }
                catch (Exception e)
                {

                    while (e.InnerException != null)
                        e = e.InnerException;

                    DebugX.Log("A exception occured during PullDataService: " + e.Message + Environment.NewLine + e.StackTrace);

                    OnWWCPEMPAdapterException?.Invoke(DateTime.Now,
                                                      this,
                                                      e);

                }

            }

        }

        public async Task PullData()
        {

            var LockTaken = await PullEVSEDataLock.WaitAsync(0).ConfigureAwait(false);

            if (LockTaken)
            {

                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

                var StartTime = DateTime.UtcNow;
                DebugX.LogT("[" + Id + "] 'Pull data service' started at " + StartTime.ToIso8601());

                try
                {

                    var TimestampBeforeLastPullDataRun = DateTime.UtcNow;

                    var PullEVSEData  = await EMPRoaming.PullEVSEData(DefaultProviderId.Value,
                                                                      DefaultSearchCenter,
                                                                      DefaultDistanceKM ?? 0,
                                                                      TimestampOfLastPullDataRun,

                                                                      CancellationToken:  new CancellationTokenSource().Token,
                                                                      EventTrackingId:    EventTracking_Id.New,
                                                                      RequestTimeout:     PullDataServiceRequestTimeout).

                                                         ConfigureAwait(false);

                    //var PullEVSEData = new {
                    //    Content = PullEVSEDataResponse.Parse(null,
                    //                                         XDocument.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar +
                    //                                                                          "PullEvseDataResponse_2017-07-19_PROD.xml", Encoding.UTF8)).Root)
                    //};

                    var DownloadTime = DateTime.UtcNow;

                    TimestampOfLastPullDataRun = TimestampBeforeLastPullDataRun;

                    #region Everything is ok!

                    if (PullEVSEData                    != null     &&
                        PullEVSEData.Content            != null     &&
                        PullEVSEData.Content.StatusCode == null)
                        //PullEVSEData.Content.StatusCode != null     &&
                        //PullEVSEData.Content.StatusCode.HasResult() &&
                        //PullEVSEData.Content.StatusCode.Value.Code == StatusCodes.Success)
                    {

                        // This will parse all nested data structures!
                        var OperatorEVSEData = PullEVSEData?.Content?.EVSEData?.OperatorEVSEData?.ToArray();

                        if (OperatorEVSEData?.Length > 0)
                        {

                            DebugX.Log(String.Concat("Imported data from ", OperatorEVSEData.Length, " OICP EVSE operators"));

                            ChargingStationOperator      WWCPChargingStationOperator     = null;
                            ChargingStationOperator_Id?  WWCPChargingStationOperatorId   = null;
                            EVSEDataRecord[]             CurrentEVSEDataRecords          = null;

                            UInt64                       IllegalOperatorsIds             = 0;
                            UInt64                       OperatorsSkipped                = 0;
                            UInt64                       TotalEVSEsCreated               = 0;
                            UInt64                       TotalEVSEsUpdated               = 0;
                            UInt64                       TotalEVSEsSkipped               = 0;

                            CPInfoList                   _CPInfoList;
                            WWCP.EVSE_Id?                CurrentEVSEId;
                            UInt64                       EVSEsSkipped;

                            foreach (var CurrentOperatorEVSEData in OperatorEVSEData.OrderBy(evseoperator => evseoperator.OperatorName))
                            {

                                if (EVSEOperatorFilter(CurrentOperatorEVSEData.OperatorName,
                                                       CurrentOperatorEVSEData.OperatorId))
                                {

                                    WWCPChargingStationOperatorId = CurrentOperatorEVSEData.OperatorId.ToWWCP();

                                    if (WWCPChargingStationOperatorId.HasValue)
                                    {

                                        #region Get WWCP charging station operator...

                                        if (!RoamingNetwork.TryGetChargingStationOperatorById(WWCPChargingStationOperatorId, out WWCPChargingStationOperator))
                                        {

                                            DebugX.Log(String.Concat("Creating OICP EVSE operator '", CurrentOperatorEVSEData.OperatorName,
                                                                 "' (", CurrentOperatorEVSEData.OperatorId.ToString(),
                                                                 " => ", WWCPChargingStationOperatorId, ")"));

                                            WWCPChargingStationOperator = RoamingNetwork.CreateChargingStationOperator(WWCPChargingStationOperatorId.Value,
                                                                                                                       I18NString.Create(Languages.unknown,
                                                                                                                                         CurrentOperatorEVSEData.OperatorName));

                                        }

                                        #endregion

                                        #region ...or create a new one!

                                        else
                                        {

                                            DebugX.Log(String.Concat("Updating OICP EVSE operator '", CurrentOperatorEVSEData.OperatorName,
                                                                     "' (", CurrentOperatorEVSEData.OperatorId.ToString(),
                                                                     " => ", WWCPChargingStationOperatorId, ")"));

                                            // Update name (via events)!
                                            WWCPChargingStationOperator.Name = I18NString.Create(Languages.unknown,
                                                                                                 CurrentOperatorEVSEData.OperatorName);

                                        }

                                        #endregion


                                        #region Generate a list of all charging pools/stations/EVSEs

                                        CurrentEVSEId           = null;
                                        EVSEsSkipped            = 0;
                                        _CPInfoList             = new CPInfoList(WWCPChargingStationOperator.Id);
                                        CurrentEVSEDataRecords  = CurrentOperatorEVSEData.EVSEDataRecords.ToArray();

                                        foreach (var CurrentEVSEDataRecord in CurrentEVSEDataRecords)
                                        {

                                            CurrentEVSEId = CurrentEVSEDataRecord.Id.ToWWCP();

                                            if (CurrentEVSEId.HasValue)
                                            {

                                                try
                                                {
                                                                                  // Generate a stable charging pool identification
                                                    _CPInfoList.AddOrUpdateCPInfo(WWCP.ChargingPool_Id.Generate(CurrentEVSEDataRecord.Id.OperatorId.ToWWCP().Value,
                                                                                                                CurrentEVSEDataRecord.Address.      ToWWCP(),
                                                                                                                CurrentEVSEDataRecord.GeoCoordinate),
                                                                                  CurrentEVSEDataRecord.Address,
                                                                                  CurrentEVSEDataRecord.GeoCoordinate,
                                                                                  CurrentEVSEDataRecord.ChargingStationId.ToString(),
                                                                                  CurrentEVSEDataRecord.Id);

                                                } catch (Exception e)
                                                {
                                                    DebugX.Log("WWCPEMPAdapter PullEVSEData failed: " + e.Message);
                                                    EVSEsSkipped++;
                                                }

                                            }

                                            else
                                                // Invalid WWCP EVSE identification
                                                EVSEsSkipped++;

                                        }

                                        var EVSEIdLookup = _CPInfoList.VerifyUniquenessOfChargingStationIds();

                                        DebugX.Log(String.Concat(_CPInfoList.                                                               Count(), " pools, ",
                                                                 _CPInfoList.SelectMany(_ => _.ChargingStations).                           Count(), " stations, ",
                                                                 _CPInfoList.SelectMany(_ => _.ChargingStations).SelectMany(_ => _.EVSEIds).Count(), " EVSEs imported. ",
                                                                 EVSEsSkipped, " EVSEs skipped."));

                                        #endregion

                                        #region Data

                                        ChargingPool     _ChargingPool                  = null;
                                        UInt64           ChargingPoolsCreated           = 0;
                                        UInt64           ChargingPoolsUpdated           = 0;
                                        Languages        LocationLanguage               = Languages.unknown;
                                        Languages        LocalChargingStationLanguage   = Languages.unknown;

                                        ChargingStation  _ChargingStation               = null;
                                        UInt64           ChargingStationsCreated        = 0;
                                        UInt64           ChargingStationsUpdated        = 0;

                                        EVSEInfo         EVSEInfo                       = null;
                                        EVSE             _EVSE                          = null;
                                        UInt64           EVSEsCreated                   = 0;
                                        UInt64           EVSEsUpdated                   = 0;

                                        #endregion


                                        //foreach (var poolinfo in _CPInfoList.ChargingPools)
                                        //{

                                        //    try
                                        //    {

                                        //        foreach (var stationinfo in poolinfo)
                                        //        {

                                        //            try
                                        //            {

                                        //                foreach (var evseid in stationinfo)
                                        //                {

                                        //                    try
                                        //                    {


                                        //                    }
                                        //                    catch (Exception e)
                                        //                    { }

                                        //                }

                                        //            }
                                        //            catch (Exception e)
                                        //            { }

                                        //        }

                                        //    }
                                        //    catch (Exception e)
                                        //    { }

                                        //}

                                        foreach (var CurrentEVSEDataRecord in CurrentEVSEDataRecords)
                                        {

                                            CurrentEVSEId = CurrentEVSEDataRecord.Id.ToWWCP();

                                            if (CurrentEVSEId.HasValue && EVSEIdLookup.Contains(CurrentEVSEDataRecord.Id))
                                            {

                                                try
                                                {

                                                    EVSEInfo = EVSEIdLookup[CurrentEVSEDataRecord.Id];

                                                    #region Set LocationLanguage

                                                    switch (EVSEInfo.PoolAddress.Country.Alpha2Code.ToLower())
                                                    {

                                                        case "de": LocationLanguage = Languages.deu; break;
                                                        case "fr": LocationLanguage = Languages.fra; break;
                                                        case "dk": LocationLanguage = Languages.dk; break;
                                                        case "no": LocationLanguage = Languages.no; break;
                                                        case "fi": LocationLanguage = Languages.fin; break;
                                                        case "se": LocationLanguage = Languages.swe; break;

                                                        case "sk": LocationLanguage = Languages.sk; break;
                                                        case "it": LocationLanguage = Languages.ita; break;
                                                        case "us": LocationLanguage = Languages.eng; break;
                                                        case "nl": LocationLanguage = Languages.nld; break;
                                                        case "at": LocationLanguage = Languages.deu; break;
                                                        case "ru": LocationLanguage = Languages.ru; break;
                                                        case "il": LocationLanguage = Languages.heb; break;

                                                        case "be":
                                                        case "ch":
                                                        case "al":
                                                        default:   LocationLanguage = Languages.unknown; break;

                                                    }

                                                    if (EVSEInfo.PoolAddress.Country == Country.Germany)
                                                        LocalChargingStationLanguage = Languages.deu;

                                                    else if (EVSEInfo.PoolAddress.Country == Country.Denmark)
                                                        LocalChargingStationLanguage = Languages.dk;

                                                    else if (EVSEInfo.PoolAddress.Country == Country.France)
                                                        LocalChargingStationLanguage = Languages.fra;

                                                    else
                                                        LocalChargingStationLanguage = Languages.unknown;

                                                    #endregion

                                                    #region Guess the language of the 'ChargingStationName' by '_Address.Country'

                                                    //_ChargingStationName = new I18NString();

                                                    //if (LocalChargingStationName.IsNotNullOrEmpty())
                                                    //    _ChargingStationName.Add(LocalChargingStationLanguage,
                                                    //                             LocalChargingStationName);

                                                    //if (EnChargingStationName.IsNotNullOrEmpty())
                                                    //    _ChargingStationName.Add(Languages.en,
                                                    //                             EnChargingStationName);

                                                    #endregion


                                                    #region Update matching charging pool...

                                                    if (WWCPChargingStationOperator.TryGetChargingPoolbyId(EVSEInfo.PoolId, out _ChargingPool))
                                                    {

                                                        // External update via events!
                                                        _ChargingPool.Description           = CurrentEVSEDataRecord.AdditionalInfo;
                                                        _ChargingPool.LocationLanguage      = LocationLanguage;
                                                        _ChargingPool.EntranceLocation      = CurrentEVSEDataRecord.GeoChargingPointEntrance;
                                                        _ChargingPool.OpeningTimes          = CurrentEVSEDataRecord.OpeningTime != null ? OpeningTimes.Parse(CurrentEVSEDataRecord.OpeningTime) : null;
                                                        _ChargingPool.AuthenticationModes   = new ReactiveSet<WWCP.AuthenticationModes>(CurrentEVSEDataRecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OICPMapper.AsWWCPAuthenticationMode(mode)));
                                                        _ChargingPool.PaymentOptions        = new ReactiveSet<WWCP.PaymentOptions>     (CurrentEVSEDataRecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OICPMapper.AsWWCPPaymentOption(option)));
                                                        _ChargingPool.Accessibility         = CurrentEVSEDataRecord.Accessibility.ToWWCP();
                                                        _ChargingPool.HotlinePhoneNumber    = I18NString.Create(Languages.unknown, CurrentEVSEDataRecord.HotlinePhoneNumber);

                                                        ChargingPoolsUpdated++;

                                                    }

                                                    #endregion

                                                    #region  ...or create a new one!

                                                    else
                                                    {

                                                        // An operator might have multiple suboperator ids!
                                                        if (!WWCPChargingStationOperator.Ids.Contains(EVSEInfo.PoolId.OperatorId))
                                                            WWCPChargingStationOperator.AddId(EVSEInfo.PoolId.OperatorId);

                                                        _ChargingPool = WWCPChargingStationOperator.CreateChargingPool(

                                                                            EVSEInfo.PoolId,

                                                                            Configurator: pool => {

                                                                                pool.DataSource                  = Id.ToString();
                                                                                pool.Description                 = CurrentEVSEDataRecord.AdditionalInfo;
                                                                                pool.Address                     = CurrentEVSEDataRecord.Address.ToWWCP();
                                                                                pool.GeoLocation                 = CurrentEVSEDataRecord.GeoCoordinate;
                                                                                pool.LocationLanguage            = LocationLanguage;
                                                                                pool.EntranceLocation            = CurrentEVSEDataRecord.GeoChargingPointEntrance;
                                                                                pool.OpeningTimes                = CurrentEVSEDataRecord.OpeningTime != null ? OpeningTimes.Parse(CurrentEVSEDataRecord.OpeningTime) : null;
                                                                                pool.AuthenticationModes         = new ReactiveSet<WWCP.AuthenticationModes>(CurrentEVSEDataRecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OICPMapper.AsWWCPAuthenticationMode(mode)));
                                                                                pool.PaymentOptions              = new ReactiveSet<WWCP.PaymentOptions>     (CurrentEVSEDataRecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OICPMapper.AsWWCPPaymentOption(option)));
                                                                                pool.Accessibility               = CurrentEVSEDataRecord.Accessibility.ToWWCP();
                                                                                pool.HotlinePhoneNumber          = I18NString.Create(Languages.unknown, CurrentEVSEDataRecord.HotlinePhoneNumber);
                                                                                //pool.StatusAggregationDelegate   = ChargingStationStatusAggregationDelegate;

                                                                                ChargingPoolsCreated++;

                                                                            });

                                                    }

                                                    #endregion


                                                    #region Update matching charging station...

                                                    if (_ChargingPool.TryGetChargingStationbyId(EVSEInfo.StationId, out _ChargingStation))
                                                    {

                                                        // Update via events!
                                                        _ChargingStation.Name                       = CurrentEVSEDataRecord.ChargingStationName;
                                                        _ChargingStation.HubjectStationId           = CurrentEVSEDataRecord.ChargingStationId.ToString();
                                                        _ChargingStation.Description                = CurrentEVSEDataRecord.AdditionalInfo;
                                                        _ChargingStation.AuthenticationModes        = new ReactiveSet<WWCP.AuthenticationModes>(CurrentEVSEDataRecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OICPMapper.AsWWCPAuthenticationMode(mode)));
                                                        _ChargingStation.PaymentOptions             = new ReactiveSet<WWCP.PaymentOptions>     (CurrentEVSEDataRecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OICPMapper.AsWWCPPaymentOption(option)));
                                                        _ChargingStation.Accessibility              = CurrentEVSEDataRecord.Accessibility.ToWWCP();
                                                        _ChargingStation.HotlinePhoneNumber         = I18NString.Create(Languages.unknown, CurrentEVSEDataRecord.HotlinePhoneNumber);
                                                        _ChargingStation.IsHubjectCompatible        = CurrentEVSEDataRecord.IsHubjectCompatible;
                                                        _ChargingStation.DynamicInfoAvailable       = CurrentEVSEDataRecord.DynamicInfoAvailable;
                                                        _ChargingStation.StatusAggregationDelegate  = EVSEStatusAggregationDelegate;

                                                        ChargingStationsUpdated++;

                                                    }

                                                    #endregion

                                                    #region ...or create a new one!

                                                    else
                                                        _ChargingStation = _ChargingPool.CreateChargingStation(

                                                                                EVSEInfo.StationId,

                                                                                Configurator: station => {

                                                                                    station.DataSource                 = Id.ToString();
                                                                                    station.Name                       = CurrentEVSEDataRecord.ChargingStationName;
                                                                                    station.HubjectStationId           = CurrentEVSEDataRecord.ChargingStationId.ToString();
                                                                                    station.Description                = CurrentEVSEDataRecord.AdditionalInfo;
                                                                                    station.AuthenticationModes        = new ReactiveSet<WWCP.AuthenticationModes>(CurrentEVSEDataRecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OICPMapper.AsWWCPAuthenticationMode(mode)));
                                                                                    station.PaymentOptions             = new ReactiveSet<WWCP.PaymentOptions>     (CurrentEVSEDataRecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OICPMapper.AsWWCPPaymentOption(option)));
                                                                                    station.Accessibility              = CurrentEVSEDataRecord.Accessibility.ToWWCP();
                                                                                    station.HotlinePhoneNumber         = I18NString.Create(Languages.unknown, CurrentEVSEDataRecord.HotlinePhoneNumber);
                                                                                    station.IsHubjectCompatible        = CurrentEVSEDataRecord.IsHubjectCompatible;
                                                                                    station.DynamicInfoAvailable       = CurrentEVSEDataRecord.DynamicInfoAvailable;
                                                                                    station.StatusAggregationDelegate  = EVSEStatusAggregationDelegate;

                                                                                    // photo_uri => "place_photo"

                                                                                    ChargingStationsCreated++;

                                                                                }

                                                               );

                                                    #endregion


                                                    #region Update matching EVSE...

                                                    if (_ChargingStation.TryGetEVSEbyId(CurrentEVSEDataRecord.Id.ToWWCP().Value, out _EVSE))
                                                    {

                                                        // Update via events!
                                                        _EVSE.Description     = CurrentEVSEDataRecord.AdditionalInfo;
                                                        _EVSE.ChargingModes   = CurrentEVSEDataRecord.ChargingModes.AsWWCPChargingMode();
                                                        OICPMapper.ApplyChargingFacilities(_EVSE, CurrentEVSEDataRecord.ChargingFacilities);
                                                        _EVSE.MaxCapacity     = CurrentEVSEDataRecord.MaxCapacity;
                                                        _EVSE.SocketOutlets   = new ReactiveSet<SocketOutlet>(CurrentEVSEDataRecord.Plugs.ToEnumeration().SafeSelect(Plug => new SocketOutlet(Plug.AsWWCPPlugTypes())));

                                                        EVSEsUpdated++;

                                                    }

                                                    #endregion

                                                    #region ...or create a new one!

                                                    else
                                                        _ChargingStation.CreateEVSE(CurrentEVSEDataRecord.Id.ToWWCP().Value,

                                                                                    Configurator: evse => {

                                                                                        evse.DataSource      = Id.ToString();
                                                                                        evse.Description     = CurrentEVSEDataRecord.AdditionalInfo;
                                                                                        evse.ChargingModes   = CurrentEVSEDataRecord.ChargingModes.AsWWCPChargingMode();
                                                                                        OICPMapper.ApplyChargingFacilities(evse, CurrentEVSEDataRecord.ChargingFacilities);
                                                                                        evse.MaxCapacity     = CurrentEVSEDataRecord.MaxCapacity;
                                                                                        evse.SocketOutlets   = new ReactiveSet<SocketOutlet>(CurrentEVSEDataRecord.Plugs.ToEnumeration().SafeSelect(Plug => new SocketOutlet(Plug.AsWWCPPlugTypes())));

                                                                                        EVSEsCreated++;

                                                                                    }
                                                                                   );

                                                    #endregion


                                                }
                                                catch (Exception e)
                                                {
                                                    DebugX.Log(e.Message);
                                                }

                                            }

                                        }

                                        DebugX.Log(EVSEsCreated + " EVSE created, " + EVSEsUpdated + " EVSEs updated, " + EVSEsSkipped + " EVSEs skipped");

                                        TotalEVSEsCreated += EVSEsCreated;
                                        TotalEVSEsUpdated += EVSEsUpdated;
                                        TotalEVSEsSkipped += EVSEsSkipped;

                                    }

                                    #region Illegal charging station operator identification...

                                    else
                                    {
                                        DebugX.Log("Illegal charging station operator identification: '" + CurrentOperatorEVSEData.OperatorId.ToString() + "'!");
                                        IllegalOperatorsIds++;
                                        TotalEVSEsSkipped += (UInt64) CurrentOperatorEVSEData.EVSEDataRecords.LongCount();
                                    }

                                    #endregion

                                }

                                #region EVSE operator is filtered...

                                else
                                {
                                    DebugX.Log("Skipping EVSE operator " + CurrentOperatorEVSEData.OperatorName + " (" + CurrentOperatorEVSEData.OperatorId.ToString() + ") with " + CurrentOperatorEVSEData.EVSEDataRecords.Count() + " EVSE data records");
                                    OperatorsSkipped++;
                                    TotalEVSEsSkipped += (UInt64) CurrentOperatorEVSEData.EVSEDataRecords.LongCount();
                                }

                                #endregion

                            }

                            if (IllegalOperatorsIds > 0)
                                DebugX.Log(IllegalOperatorsIds + " illegal EVSE operator identifications");

                            if (OperatorsSkipped > 0)
                                DebugX.Log(OperatorsSkipped    + " EVSE operators skipped");

                            if (TotalEVSEsCreated > 0)
                                DebugX.Log(TotalEVSEsCreated   + " EVSEs created");

                            if (TotalEVSEsUpdated > 0)
                                DebugX.Log(TotalEVSEsUpdated   + " EVSEs updated");

                            if (TotalEVSEsSkipped > 0)
                                DebugX.Log(TotalEVSEsSkipped   + " EVSEs skipped");

                        }

                    }

                    #endregion

                    #region HTTP status is not 200 - OK

                        //else if (PullEVSEDataTask.Result.HTTPStatusCode != HTTPStatusCode.OK)
                        //{
                        //
                        //    DebugX.Log("Importing EVSE data records failed: " +
                        //               PullEVSEDataTask.Result.HTTPStatusCode.ToString() +
                        //
                        //               PullEVSEDataTask.Result.HTTPBody != null
                        //                   ? Environment.NewLine + PullEVSEDataTask.Result.HTTPBody.ToUTF8String()
                        //                   : "");
                        //
                        //}

                        #endregion

                    #region OICP StatusCode is not 'Success'

                        //else if (PullEVSEDataTask.Result.Content.StatusCode != null &&
                        //        !PullEVSEDataTask.Result.Content.StatusCode.HasResult)
                        //{
                        //
                        //    DebugX.Log("Importing EVSE data records failed: " +
                        //               PullEVSEDataTask.Result.Content.StatusCode.Code.ToString() +
                        //
                        //               (PullEVSEDataTask.Result.Content.StatusCode.Description.IsNotNullOrEmpty()
                        //                    ? ", " + PullEVSEDataTask.Result.Content.StatusCode.Description
                        //                    : "") +
                        //
                        //               (PullEVSEDataTask.Result.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                        //                    ? ", " + PullEVSEDataTask.Result.Content.StatusCode.AdditionalInfo
                        //                    : ""));
                        //
                        //}

                        #endregion

                    #region Something unexpected happend!

                        //else
                        //{
                        //    DebugX.Log("Importing EVSE data records failed unexpectedly!");
                        //}

                        #endregion


                    var EndTime = DateTime.UtcNow;
                    DebugX.LogT("[" + Id + "] 'Pull data service' finished after " + (EndTime - StartTime).TotalSeconds + " seconds (" + (DownloadTime - StartTime).TotalSeconds + "/" + (EndTime - DownloadTime).TotalSeconds + ")");

                }
                catch (Exception e)
                {

                    while (e.InnerException != null)
                        e = e.InnerException;

                    DebugX.LogT(nameof(WWCPEMPAdapter) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                }

                finally
                {
                    if (LockTaken)
                        PullEVSEDataLock.Release();
                }

            }

        }

        #endregion

        #region (timer) PullStatusService(State)

        private void PullStatusService(Object State)
        {

            if (!DisablePullStatus)
            {

                PullStatus().Wait();

                //ToDo: Handle errors!

            }

        }

        public async Task PullStatus()
        {

            DebugX.LogT("[" + Id + "] 'Pull status service', as every " + _PullStatusServiceEvery + "ms!");

            var DataLockTaken = await PullEVSEDataLock.WaitAsync(0).ConfigureAwait(false);

            if (DataLockTaken)
            {

                var StatusLockTaken = await PullEVSEStatusLock.WaitAsync(0).ConfigureAwait(false);

                if (StatusLockTaken)
                {

                    Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

                    var StartTime = DateTime.UtcNow;
                    DebugX.LogT("[" + Id + "] 'Pull status service' started at " + StartTime.ToIso8601());

                    try
                    {

                        var PullEVSEStatus = await EMPRoaming.PullEVSEStatus(DefaultProviderId.Value,
                                                                             DefaultSearchCenter,
                                                                             DefaultDistanceKM ?? 0,

                                                                             CancellationToken:  new CancellationTokenSource().Token,
                                                                             EventTrackingId:    EventTracking_Id.New,
                                                                             RequestTimeout:     PullStatusServiceRequestTimeout);

                        var DownloadTime = DateTime.UtcNow;

                    #region Everything is ok!

                    if (PullEVSEStatus                    != null  &&
                        PullEVSEStatus.Content            != null  &&
                        PullEVSEStatus.Content.StatusCode.HasValue &&
                        PullEVSEStatus.Content.StatusCode.Value.Code == StatusCodes.Success)
                    {

                        var OperatorEVSEStatus = PullEVSEStatus.Content.OperatorEVSEStatus;

                        if (OperatorEVSEStatus != null && OperatorEVSEStatus.Any())
                        {

                            DebugX.Log("Imported " + OperatorEVSEStatus.Count() + " OperatorEVSEStatus!");
                            DebugX.Log("Imported " + OperatorEVSEStatus.SelectMany(status => status.EVSEStatusRecords).Count() + " EVSEStatusRecords!");

                            ChargingStationOperator      WWCPChargingStationOperator     = null;
                            ChargingStationOperator_Id?  WWCPChargingStationOperatorId   = null;
                            UInt64                       IllegalOperatorsIds             = 0;
                            UInt64                       OperatorsSkipped                = 0;
                            UInt64                       TotalEVSEsUpdated               = 0;
                            UInt64                       TotalEVSEsSkipped               = 0;

                            foreach (var CurrentOperatorEVSEStatus in OperatorEVSEStatus.OrderBy(evseoperator => evseoperator.OperatorName))
                            {

                                if (EVSEOperatorFilter(CurrentOperatorEVSEStatus.OperatorName,
                                                       CurrentOperatorEVSEStatus.OperatorId))
                                {

                                    DebugX.Log("Importing EVSE operator " + CurrentOperatorEVSEStatus.OperatorName + " (" + CurrentOperatorEVSEStatus.OperatorId.ToString() + ") with " + CurrentOperatorEVSEStatus.EVSEStatusRecords.Count() + " EVSE status records");

                                    WWCPChargingStationOperatorId = CurrentOperatorEVSEStatus.OperatorId.ToWWCP();

                                    if (WWCPChargingStationOperatorId.HasValue)
                                    {

                                        if (!RoamingNetwork.TryGetChargingStationOperatorById(WWCPChargingStationOperatorId, out WWCPChargingStationOperator))
                                            WWCPChargingStationOperator = RoamingNetwork.CreateChargingStationOperator(WWCPChargingStationOperatorId.Value,
                                                                                                                       I18NString.Create(Languages.unknown, CurrentOperatorEVSEStatus.OperatorName));

                                        else
                                            // Update name (via events)!
                                            WWCPChargingStationOperator.Name = I18NString.Create(Languages.unknown, CurrentOperatorEVSEStatus.OperatorName);

                                        WWCP.EVSE     CurrentEVSE    = null;
                                        WWCP.EVSE_Id? CurrentEVSEId  = null;
                                        UInt64        EVSEsUpdated   = 0;
                                        UInt64        EVSEsSkipped   = 0;

                                        foreach (var CurrentEVSEDataRecord in CurrentOperatorEVSEStatus.EVSEStatusRecords)
                                        {

                                            CurrentEVSEId = CurrentEVSEDataRecord.Id.ToWWCP();

                                            if (CurrentEVSEId.HasValue &&
                                                WWCPChargingStationOperator.TryGetEVSEbyId(CurrentEVSEId, out CurrentEVSE))
                                            {
                                                CurrentEVSE.Status = CurrentEVSEDataRecord.Status.AsWWCPEVSEStatus();
                                                EVSEsUpdated++;
                                            }

                                            else
                                                EVSEsSkipped++;

                                        }

                                        DebugX.Log(EVSEsUpdated + " EVSE status updated, " + EVSEsSkipped + " EVSEs skipped");

                                        TotalEVSEsUpdated += EVSEsUpdated;
                                        TotalEVSEsSkipped += EVSEsSkipped;

                                    }

                                    #region Illegal charging station operator identification...

                                    else
                                    {
                                        DebugX.Log("Illegal charging station operator identification: '" + CurrentOperatorEVSEStatus.OperatorId.ToString() + "'!");
                                        IllegalOperatorsIds++;
                                        TotalEVSEsSkipped += (UInt64) CurrentOperatorEVSEStatus.EVSEStatusRecords.LongCount();
                                    }

                                    #endregion

                                }

                                #region EVSE operator is filtered...

                                else
                                {
                                    DebugX.Log("Skipping EVSE operator " + CurrentOperatorEVSEStatus.OperatorName + " (" + CurrentOperatorEVSEStatus.OperatorId.ToString() + ") with " + CurrentOperatorEVSEStatus.EVSEStatusRecords.Count() + " EVSE status records");
                                    OperatorsSkipped++;
                                    TotalEVSEsSkipped += (UInt64) CurrentOperatorEVSEStatus.EVSEStatusRecords.LongCount();
                                }

                                #endregion

                            }

                            if (IllegalOperatorsIds > 0)
                                DebugX.Log(OperatorsSkipped + " illegal EVSE operator identifications");

                            if (OperatorsSkipped > 0)
                                DebugX.Log(OperatorsSkipped + " EVSE operators skipped");

                            if (TotalEVSEsUpdated > 0)
                                DebugX.Log(TotalEVSEsUpdated + " EVSEs updated");

                            if (TotalEVSEsSkipped > 0)
                                DebugX.Log(TotalEVSEsSkipped + " EVSEs skipped");

                        }

                    }

                    #endregion

                    #region HTTP status is not 200 - OK

                    else if (PullEVSEStatus.HTTPStatusCode != HTTPStatusCode.OK)
                    {

                        DebugX.Log("Importing EVSE status records failed: " +
                                   PullEVSEStatus.HTTPStatusCode +

                                   PullEVSEStatus.HTTPBody != null
                                       ? Environment.NewLine + PullEVSEStatus.HTTPBody.ToUTF8String()
                                       : "");

                    }

                    #endregion

                    #region OICP StatusCode is not 'Success'

                    else if (PullEVSEStatus.Content.StatusCode.HasValue &&
                            !PullEVSEStatus.Content.StatusCode.Value.HasResult)
                    {

                        DebugX.Log("Importing EVSE status records failed: " +
                                   PullEVSEStatus.Content.StatusCode.Value.Code.ToString() +

                                   (PullEVSEStatus.Content.StatusCode.Value.Description.IsNotNullOrEmpty()
                                        ? ", " + PullEVSEStatus.Content.StatusCode.Value.Description
                                        : "") +

                                   (PullEVSEStatus.Content.StatusCode.Value.AdditionalInfo.IsNotNullOrEmpty()
                                        ? ", " + PullEVSEStatus.Content.StatusCode.Value.AdditionalInfo
                                        : ""));

                    }

                    #endregion

                    #region Something unexpected happend!

                    else
                    {
                        DebugX.Log("Importing EVSE status records failed unexpectedly!");
                    }

                    #endregion


                        var EndTime = DateTime.UtcNow;

                        DebugX.LogT("[" + Id + "] 'Pull status service' finished after " + (EndTime - StartTime).TotalSeconds + " seconds (" + (DownloadTime - StartTime).TotalSeconds + "/" + (EndTime - DownloadTime).TotalSeconds + ")");

                    }
                    catch (Exception e)
                    {

                        while (e.InnerException != null)
                            e = e.InnerException;

                        DebugX.LogT(nameof(WWCPEMPAdapter) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                    }

                    finally
                    {
                        PullEVSEStatusLock.Release();
                    }

                }

                else
                    Console.WriteLine("PullStatus->PullStatusServiceLock missed!");

                PullEVSEDataLock.Release();

            }

            else
                Console.WriteLine("PullStatus->PullDataServiceLock missed!");

            return;

        }

        #endregion

        // Pull CDRs!

        // -----------------------------------------------------------------------------------------------------


        #region Operator overloading

        #region Operator == (WWCPEMPAdapter1, WWCPEMPAdapter2)

        /// <summary>
        /// Compares two WWCPEMPAdapters for equality.
        /// </summary>
        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (WWCPEMPAdapter WWCPEMPAdapter1, WWCPEMPAdapter WWCPEMPAdapter2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(WWCPEMPAdapter1, WWCPEMPAdapter2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) WWCPEMPAdapter1 == null) || ((Object) WWCPEMPAdapter2 == null))
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
        public static Boolean operator != (WWCPEMPAdapter WWCPEMPAdapter1, WWCPEMPAdapter WWCPEMPAdapter2)

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

            if ((Object) WWCPEMPAdapter1 == null)
                throw new ArgumentNullException(nameof(WWCPEMPAdapter1),  "The given WWCPEMPAdapter must not be null!");

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

            if ((Object) WWCPEMPAdapter1 == null)
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
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var WWCPEMPAdapter = Object as WWCPEMPAdapter;
            if ((Object) WWCPEMPAdapter == null)
                throw new ArgumentException("The given object is not an WWCPEMPAdapter!", nameof(Object));

            return CompareTo(WWCPEMPAdapter);

        }

        #endregion

        #region CompareTo(WWCPEMPAdapter)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPEMPAdapter">An WWCPEMPAdapter object to compare with.</param>
        public Int32 CompareTo(WWCPEMPAdapter WWCPEMPAdapter)
        {

            if ((Object) WWCPEMPAdapter == null)
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
        {

            if (Object == null)
                return false;

            var WWCPEMPAdapter = Object as WWCPEMPAdapter;
            if ((Object) WWCPEMPAdapter == null)
                return false;

            return Equals(WWCPEMPAdapter);

        }

        #endregion

        #region Equals(WWCPEMPAdapter)

        /// <summary>
        /// Compares two WWCPEMPAdapter for equality.
        /// </summary>
        /// <param name="WWCPEMPAdapter">An WWCPEMPAdapter to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(WWCPEMPAdapter WWCPEMPAdapter)
        {

            if ((Object) WWCPEMPAdapter == null)
                return false;

            return Id.Equals(WWCPEMPAdapter.Id);

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

            => "OICP" + Version.Number + " EMP Adapter " + Id;

        #endregion


    }

}
