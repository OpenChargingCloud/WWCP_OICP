/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

using WWCP = cloud.charging.open.protocols.WWCP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// A delegate called whenever new EVSEStatusRecords had been received.
    /// </summary>
    public delegate Task OnPullEVSEDataDelegate          (DateTime                       Timestamp,
                                                          WWCPCSOAdapter                 Sender,
                                                          String                         SenderDescription,
                                                          IEnumerable<EVSEDataRecord>    EVSEDataRecords);

    /// <summary>
    /// A delegate called whenever new OperatorInfos had been fetched.
    /// </summary>
    public delegate Task OnPullOperatorInfosDelegate     (DateTime                       Timestamp,
                                                          WWCPCSOAdapter                 Sender,
                                                          String                         SenderDescription,
                                                          IEnumerable<OperatorInfo>      OperatorInfos);

    /// <summary>
    /// A delegate called whenever new EVSEStatusRecords had been received.
    /// </summary>
    public delegate Task OnPullEVSEStatusDelegate        (DateTime                       Timestamp,
                                                          WWCPCSOAdapter                 Sender,
                                                          String                         SenderDescription,
                                                          IEnumerable<EVSEStatusRecord>  EVSEStatusRecords);

    /// <summary>
    /// A delegate called whenever new EVSE status changes had been received.
    /// </summary>
    public delegate Task OnEVSEStatusChangesDelegate     (DateTime                       Timestamp,
                                                          WWCPCSOAdapter                 Sender,
                                                          String                         SenderDescription,
                                                          IEnumerable<WWCP.EVSEStatus>   EVSEStatusChanges);


    /// <summary>
    /// A delegate called whenever new EVSEStatusRecords had been received.
    /// </summary>
    public delegate Task OnGetChargeDetailRecordsDelegate(DateTime                         Timestamp,
                                                          WWCPCSOAdapter                   Sender,
                                                          String                           SenderDescription,
                                                          IEnumerable<ChargeDetailRecord>  ChargeDetailRecords);


    /// <summary>
    /// A WWCP wrapper for the OICP EMP roaming client which maps
    /// WWCP data structures onto OICP data structures and vice versa.
    /// </summary>
    public class WWCPCSOAdapter : WWCP.ACryptoEMobilityEntity<WWCP.CSORoamingProvider_Id,
                                                              WWCP.CSORoamingProviderAdminStatusTypes,
                                                              WWCP.CSORoamingProviderStatusTypes>,
                                  //WWCP.ACSORoamingProvider,//<ChargeDetailRecord>,
                                  WWCP.ICSORoamingProvider
                                  //IEquatable<WWCPEMPAdapter>,
                                  //IComparable<WWCPEMPAdapter>,
                                  //IComparable

                                  //ACryptoEMobilityEntity<CSORoamingProvider_Id>,
                                  //ICSORoamingProvider,
                                  //ISendAuthenticationData
    {

        #region Data

        private static readonly  SemaphoreSlim  PullEVSEDataLock                                = new SemaphoreSlim(1, 1);
        private static readonly  SemaphoreSlim  PullEVSEStatusLock                              = new SemaphoreSlim(1, 1);
        private static readonly  SemaphoreSlim  GetChargeDetailRecordsLock                      = new SemaphoreSlim(1, 1);

        private readonly         Timer          PullEVSEData_Timer;
        private readonly         Timer          PullEVSEStatus_Timer;
        private readonly         Timer          GetChargeDetailRecords_Timer;

        public  static readonly  TimeSpan       Default_PullEVSEData_Every                      = TimeSpan.FromHours(3);
        public  static readonly  TimeSpan       Default_PullEVSEStatus_Every                    = TimeSpan.FromMinutes(1);
        public  static readonly  TimeSpan       Default_GetChargeDetailRecords_Every            = TimeSpan.FromMinutes(15);

        public  static readonly  TimeSpan       Default_PullEVSEData_RequestTimeout             = TimeSpan.FromMinutes(5);
        public  static readonly  TimeSpan       Default_PullEVSEStatus_RequestTimeout           = TimeSpan.FromMinutes(1);
        public  static readonly  TimeSpan       Default_GetChargeDetailRecords_RequestTimeout   = TimeSpan.FromMinutes(1);


        /// <summary>
        ///  The default reservation time.
        /// </summary>
        public  static readonly  TimeSpan       DefaultReservationTime                          = TimeSpan.FromMinutes(15);


        private static readonly  TimeSpan       SemaphoreSlimTimeout                            = TimeSpan.FromSeconds(5);

        #endregion

        #region Properties

        /// <summary>
        /// The wrapped EMP roaming object.
        /// </summary>
        public EMPRoaming  EMPRoaming     { get; }


        /// <summary>
        /// The EMP client.
        /// </summary>
        public EMPClient EMPClient
            => EMPRoaming.EMPClient;

        /// <summary>
        /// The EMP HTTP client logger.
        /// </summary>
        public EMPClient.HTTP_Logger HTTPClientLogger
            => EMPRoaming.EMPClient.HTTPLogger;

        /// <summary>
        /// The EMP client logger.
        /// </summary>
        public EMPClient.EMPClientLogger? ClientLogger
            => EMPRoaming.EMPClient.Logger;


        /// <summary>
        /// The EMP server.
        /// </summary>
        public EMPServerAPI EMPServer
            => EMPRoaming.EMPServer;

        /// <summary>
        /// The EMP HTTP server logger.
        /// </summary>
        public EMPServerAPI.HTTP_Logger HTTPServerLogger
            => EMPRoaming.EMPServer.HTTPLogger;

        /// <summary>
        /// The EMP server logger.
        /// </summary>
        public EMPServerAPI.ServerAPILogger? ServerLogger
            => EMPRoaming.EMPServer.Logger;


        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        public DNSClient DNSClient
            => EMPRoaming.EMPClient.DNSClient;


        /// <summary>
        /// An optional default e-mobility provider identification.
        /// </summary>
        public Provider_Id                  DefaultProviderId      { get; }


        public EVSEDataRecord2EVSEDelegate  EVSEDataRecord2EVSE    { get; }
        //public EVSEOperatorFilterDelegate   EVSEOperatorFilter;


        #region OnWWCPCSOAdapterException

        public delegate Task OnWWCPCSOAdapterExceptionDelegate(DateTime        Timestamp,
                                                               WWCPCSOAdapter  Sender,
                                                               Exception       Exception);

        public event OnWWCPCSOAdapterExceptionDelegate? OnWWCPCSOAdapterException;

        #endregion

        #region PullDataService

        public Boolean                                        PullOperatorInfos_IsDisabled                         { get; set; }

        /// <summary>
        /// The 'Pull EVSE Data' service interval.
        /// </summary>
        public TimeSpan                                       PullEVSEData_Every                                   { get; }

        public UInt32                                         PullEVSEData_RequestPageSize                         { get; }

        public TimeSpan                                       PullEVSEData_RequestTimeout                          { get; }

        public DateTime?                                      TimestampOfLastPullDataRun                           { get; private set; }

        /// <summary>
        /// Only return EVSEs belonging to the given optional enumeration of EVSE operators.
        /// </summary>
        public IEnumerable<Operator_Id>                       PullEVSEData_OperatorIdFilter                        { get; }

        /// <summary>
        /// An optional enumeration of countries whose EVSE's a provider wants to retrieve.
        /// </summary>
        public IEnumerable<Country>                           PullEVSEData_CountryCodeFilter                       { get; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<AccessibilityTypes>                PullEVSEData_AccessibilityFilter                     { get; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<AuthenticationModes>               PullEVSEData_AuthenticationModeFilter                { get; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<CalibrationLawDataAvailabilities>  PullEVSEData_CalibrationLawDataAvailabilityFilter    { get; }


        public Boolean?                                       PullEVSEData_RenewableEnergyFilter                   { get; }
        public Boolean?                                       PullEVSEData_IsHubjectCompatibleFilter               { get; }
        public Boolean?                                       PullEVSEData_IsOpen24HoursFilter                     { get; }

        #endregion

        #region PullStatusService

        public Boolean         PullEVSEStatus_IsDisabled                    { get; set; }

        /// <summary>
        /// The 'Pull EVSE Status' service interval.
        /// </summary>
        public TimeSpan        PullEVSEStatus_Every                         { get; set; }

        public TimeSpan        PullEVSEStatus_RequestTimeout                { get; }

        public DateTime?       PullStatus_LastRunTimestamp                  { get; private set; }

        #endregion

        #region GetChargeDetailRecords

        public Boolean         GetChargeDetailRecords_IsDisabled                 { get; set; }

        /// <summary>
        /// The 'GetChargeDetailRecords' service intervall.
        /// </summary>
        public TimeSpan        GetChargeDetailRecords_Every                      { get; set; }

        public TimeSpan        GetChargeDetailRecords_RequestTimeout             { get; }

        public DateTime        GetChargeDetailRecords_LastRunTimestamp           { get; private set; }

        #endregion

        public GeoCoordinate?  DefaultSearchCenter                               { get; }
        public UInt64?         DefaultDistanceKM                                 { get; }

        public Func<WWCP.EVSEStatusReport, WWCP.ChargingStationStatusTypes> EVSEStatusAggregationDelegate { get; }

        public IEnumerable<WWCP.ChargingReservation> ChargingReservations => throw new NotImplementedException();

        public IEnumerable<WWCP.ChargingSession>     ChargingSessions     => throw new NotImplementedException();

        public Boolean PullEVSEData_IsDisabled { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever new EVSEDataRecords had been received.
        /// </summary>
        public event OnPullEVSEDataDelegate            OnPullEVSEData;

        /// <summary>
        /// An event sent whenever new OperatorInfos had been fetched.
        /// </summary>
        public event OnPullOperatorInfosDelegate       OnPullOperatorInfos;

        /// <summary>
        /// An event sent whenever new EVSEStatusRecords had been received.
        /// </summary>
        public event OnPullEVSEStatusDelegate          OnPullEVSEStatus;

        /// <summary>
        /// An event sent whenever new EVSE status changes had been received.
        /// </summary>
        public event OnEVSEStatusChangesDelegate       OnEVSEStatusChanges;

        /// <summary>
        /// An event sent whenever new ChargeDetailRecords had been received.
        /// </summary>
        public event OnGetChargeDetailRecordsDelegate  OnGetChargeDetailRecords;


        // Client methods (logging)

        #region OnPullEVSEDataRequest/-Response (OICP event!)

        /// <summary>
        /// An event sent whenever a 'pull EVSE data' request will be send.
        /// </summary>
        public event OnPullEVSEDataRequestDelegate        OnPullEVSEDataRequest;

        /// <summary>
        /// An event sent whenever a response for a 'pull EVSE data' request had been received.
        /// </summary>
        public event OnPullEVSEDataResponseDelegate       OnPullEVSEDataResponse;

        #endregion

        #region OnPullEVSEStatusRequest/-Response (OICP event!)

        /// <summary>
        /// An event sent whenever a 'pull EVSE status' request will be send.
        /// </summary>
        public event OnPullEVSEStatusRequestDelegate      OnPullEVSEStatusRequest;

        /// <summary>
        /// An event sent whenever a response for a 'pull EVSE status' request had been received.
        /// </summary>
        public event OnPullEVSEStatusResponseDelegate     OnPullEVSEStatusResponse;

        #endregion


        #region OnReserveRequest/-Response

        /// <summary>
        /// An event sent whenever a reserve EVSE command will be send.
        /// </summary>
        public event WWCP.OnReserveRequestDelegate         OnReserveRequest;

        /// <summary>
        /// An event sent whenever a reserve EVSE command was sent.
        /// </summary>
        public event WWCP.OnReserveResponseDelegate        OnReserveResponse;

        #endregion

        #region OnCancelReservationRequest/-Response

        /// <summary>
        /// An event sent whenever a cancel reservation command will be send.
        /// </summary>
        public event WWCP.OnCancelReservationRequestDelegate   OnCancelReservationRequest;

        /// <summary>
        /// An event sent whenever a cancel reservation command was sent.
        /// </summary>
        public event WWCP.OnCancelReservationResponseDelegate  OnCancelReservationResponse;

        #endregion


        #region OnRemoteStartRequest/-Response

        /// <summary>
        /// An event sent whenever a remote start command will be send.
        /// </summary>
        public event WWCP.OnRemoteStartRequestDelegate     OnRemoteStartRequest;

        /// <summary>
        /// An event sent whenever a remote start command was sent.
        /// </summary>
        public event WWCP.OnRemoteStartResponseDelegate    OnRemoteStartResponse;

        #endregion

        #region OnRemoteStopRequest/-Response

        /// <summary>
        /// An event sent whenever a remote stop command will be send.
        /// </summary>
        public event WWCP.OnRemoteStopRequestDelegate      OnRemoteStopRequest;

        /// <summary>
        /// An event sent whenever a remote stop command was sent.
        /// </summary>
        public event WWCP.OnRemoteStopResponseDelegate     OnRemoteStopResponse;

        #endregion


        #region OnGetChargeDetailRecordsRequest/-Response

        /// <summary>
        /// An event sent whenever a 'get charge detail records' request will be send.
        /// </summary>
        public event WWCP.OnGetCDRsRequestDelegate    OnGetChargeDetailRecordsRequest;

        /// <summary>
        /// An event sent whenever a response to a 'get charge detail records' request was received.
        /// </summary>
        public event WWCP.OnGetCDRsResponseDelegate   OnGetChargeDetailRecordsResponse;

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

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event sent whenever an 'authorize stop' request was received.
        /// </summary>
        public event WWCP.OnAuthorizeStopRequestDelegate   OnAuthorizeStopRequest;

        /// <summary>
        /// An event sent whenever a response to an 'authorize stop' request was sent.
        /// </summary>
        public event WWCP.OnAuthorizeStopResponseDelegate  OnAuthorizeStopResponse;

        #endregion


        #region OnChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event sent whenever a 'charge detail record' was received.
        /// </summary>
        public event WWCP.OnSendCDRsRequestDelegate   OnChargeDetailRecordRequest;

        /// <summary>
        /// An event sent whenever a response to a 'charge detail record' was sent.
        /// </summary>
        public event WWCP.OnSendCDRsResponseDelegate  OnChargeDetailRecordResponse;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP EMP Roaming client for e-mobility providers/EMPs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="EMPRoaming">A OICP EMP roaming object to be mapped to WWCP.</param>
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record after receiving it from the roaming provider.</param>
        public WWCPCSOAdapter(WWCP.CSORoamingProvider_Id                     Id,
                              I18NString                                     Name,
                              I18NString                                     Description,
                              WWCP.RoamingNetwork                            RoamingNetwork,
                              EMPRoaming                                     EMPRoaming,

                              EVSEDataRecord2EVSEDelegate                    EVSEDataRecord2EVSE                                 = null,

                              Boolean                                        PullOperatorInfos_IsDisabled                        = false,
                              TimeSpan?                                      PullEVSEData_InitialDelay                           = null,
                              TimeSpan?                                      PullEVSEData_Every                                  = null,
                              UInt32?                                        PullEVSEData_RequestPageSize                        = null,
                              TimeSpan?                                      PullEVSEData_RequestTimeout                         = null,

                              IEnumerable<Operator_Id>                       PullEVSEData_OperatorIdFilter                       = null,
                              IEnumerable<Country>                           PullEVSEData_CountryCodeFilter                      = null,
                              IEnumerable<AccessibilityTypes>                PullEVSEData_AccessibilityFilter                    = null,
                              IEnumerable<AuthenticationModes>               PullEVSEData_AuthenticationModeFilter               = null,
                              IEnumerable<CalibrationLawDataAvailabilities>  PullEVSEData_CalibrationLawDataAvailabilityFilter   = null,
                              Boolean?                                       PullEVSEData_RenewableEnergyFilter                  = null,
                              Boolean?                                       PullEVSEData_IsHubjectCompatibleFilter              = null,
                              Boolean?                                       PullEVSEData_IsOpen24HoursFilter                    = null,

                              Boolean                                        PullEVSEStatus_IsDisabled                           = false,
                              TimeSpan?                                      PullEVSEStatus_InitialDelay                         = null,
                              TimeSpan?                                      PullEVSEStatus_Every                                = null,
                              TimeSpan?                                      PullEVSEStatus_RequestTimeout                       = null,

                              Boolean                                        GetChargeDetailRecords_IsDisabled                   = false,
                              TimeSpan?                                      GetChargeDetailRecords_InitialDelay                 = null,
                              TimeSpan?                                      GetChargeDetailRecords_Every                        = null,
                              DateTime?                                      GetChargeDetailRecords_LastRunTimestamp             = null,
                              TimeSpan?                                      GetChargeDetailRecords_RequestTimeout               = null,

                              WWCP.eMobilityProvider                         DefaultProvider                                     = null,
                              WWCP.eMobilityProvider_Id?                     DefaultProviderId                                   = null,
                              GeoCoordinate?                                 DefaultSearchCenter                                 = null,
                              UInt64?                                        DefaultDistanceKM                                   = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description)

        {

            this.EMPRoaming                                         = EMPRoaming                              ?? throw new ArgumentNullException(nameof(EMPRoaming),  "The given EMP roaming object must not be null!");
            this.EVSEDataRecord2EVSE                                = EVSEDataRecord2EVSE;

            this.PullOperatorInfos_IsDisabled                       = PullOperatorInfos_IsDisabled;
            this.PullEVSEData_Every                                 = PullEVSEData_Every                      ?? Default_PullEVSEData_Every;
            this.PullEVSEData_RequestPageSize                       = PullEVSEData_RequestPageSize            ?? 2000;
            this.PullEVSEData_RequestTimeout                        = PullEVSEData_RequestTimeout             ?? Default_PullEVSEData_RequestTimeout;
            this.PullEVSEData_Timer                                 = new Timer(PullOperatorInfosService,             null, PullEVSEData_InitialDelay           ?? TimeSpan.FromSeconds(10), this.PullEVSEData_Every);

            this.PullEVSEData_OperatorIdFilter                      = PullEVSEData_OperatorIdFilter;
            this.PullEVSEData_CountryCodeFilter                     = PullEVSEData_CountryCodeFilter;
            this.PullEVSEData_AccessibilityFilter                   = PullEVSEData_AccessibilityFilter;
            this.PullEVSEData_AuthenticationModeFilter              = PullEVSEData_AuthenticationModeFilter;
            this.PullEVSEData_CalibrationLawDataAvailabilityFilter  = PullEVSEData_CalibrationLawDataAvailabilityFilter;
            this.PullEVSEData_RenewableEnergyFilter                 = PullEVSEData_RenewableEnergyFilter;
            this.PullEVSEData_IsHubjectCompatibleFilter             = PullEVSEData_IsHubjectCompatibleFilter;
            this.PullEVSEData_IsOpen24HoursFilter                   = PullEVSEData_IsOpen24HoursFilter;

            this.PullEVSEStatus_IsDisabled                          = PullEVSEStatus_IsDisabled;
            this.PullEVSEStatus_Every                               = PullEVSEStatus_Every                    ?? Default_PullEVSEStatus_Every;
            this.PullEVSEStatus_RequestTimeout                      = PullEVSEStatus_RequestTimeout           ?? Default_PullEVSEStatus_RequestTimeout;
            this.PullEVSEStatus_Timer                               = new Timer(PullStatusService,               null, PullEVSEStatus_InitialDelay         ?? this.PullEVSEStatus_Every, this.PullEVSEStatus_Every);

            this.GetChargeDetailRecords_IsDisabled                  = GetChargeDetailRecords_IsDisabled;
            this.GetChargeDetailRecords_Every                       = GetChargeDetailRecords_Every            ?? Default_GetChargeDetailRecords_Every;
            this.GetChargeDetailRecords_LastRunTimestamp            = GetChargeDetailRecords_LastRunTimestamp ?? DateTime.Now - TimeSpan.FromDays(3);
            this.GetChargeDetailRecords_RequestTimeout              = GetChargeDetailRecords_RequestTimeout   ?? Default_GetChargeDetailRecords_RequestTimeout;
            this.GetChargeDetailRecords_Timer                       = new Timer(GetChargeDetailRecordsService, null, GetChargeDetailRecords_InitialDelay ?? TimeSpan.FromSeconds(10),  this.GetChargeDetailRecords_Every);

            var defaultProviderId = (DefaultProvider?.Id ?? DefaultProviderId)?.ToOICP();

            if (!defaultProviderId.HasValue)
                throw new ArgumentException("The given default provider identification is invalid!",
                                            nameof(DefaultProviderId));

            this.DefaultProviderId                                  = defaultProviderId.Value;
            this.DefaultSearchCenter                                = DefaultSearchCenter;
            this.DefaultDistanceKM                                  = DefaultDistanceKM;


            // Link events...

            #region OnAuthorizeStart

            this.EMPRoaming.OnAuthorizeStart += async (Timestamp,
                                                       Sender,
                                                       Request) => {

                #region Map parameter values

                var operatorId           = Request.OperatorId.    ToWWCP();
                var localAuthentication  = Request.Identification.ToWWCP().ToLocal;
                var chargingLocation     = WWCP.ChargingLocation.FromEVSEId(Request.EVSEId?.ToWWCP());
                var productId            = Request.PartnerProductId.HasValue
                                               ? WWCP.ChargingProduct.FromId(Request.PartnerProductId.Value.ToWWCP()?.ToString())
                                               : default;
                var sessionId            = Request.SessionId.          ToWWCP();
                var CPOPartnerSessionId  = Request.CPOPartnerSessionId.ToWWCP();

                #endregion

                #region Send OnAuthorizeStartRequest event

                var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

                try
                {

                    OnAuthorizeStartRequest?.Invoke(StartTime,
                                                    Timestamp,
                                                    this,
                                                    Id.ToString(),
                                                    Request.EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    Id,
                                                    null,
                                                    operatorId,
                                                    localAuthentication,
                                                    chargingLocation,
                                                    productId,
                                                    sessionId,
                                                    CPOPartnerSessionId,
                                                    new WWCP.ISendAuthorizeStartStop[0],
                                                    Request.RequestTimeout);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnAuthorizeStartRequest));
                }

                #endregion


                var response = await RoamingNetwork.AuthorizeStart(localAuthentication,
                                                                   chargingLocation,
                                                                   productId,
                                                                   sessionId,
                                                                   CPOPartnerSessionId,
                                                                   operatorId,

                                                                   Timestamp,
                                                                   Request.CancellationToken,
                                                                   Request.EventTrackingId,
                                                                   Request.RequestTimeout);


                #region Send OnAuthorizeStartResponse event

                var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

                try
                {

                    OnAuthorizeStartResponse?.Invoke(EndTime,
                                                     Timestamp,
                                                     this,
                                                     Id.ToString(),
                                                     Request.EventTrackingId,
                                                     RoamingNetwork.Id,
                                                     Id,
                                                     null,
                                                     operatorId,
                                                     localAuthentication,
                                                     chargingLocation,
                                                     productId,
                                                     sessionId,
                                                     CPOPartnerSessionId,
                                                     new WWCP.ISendAuthorizeStartStop[0],
                                                     Request.RequestTimeout,
                                                     response,
                                                     EndTime - StartTime);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnAuthorizeStartResponse));
                }

                #endregion

                #region Map response

                if (response != null)
                {
                    switch (response.Result)
                    {

                        case WWCP.AuthStartResultTypes.Authorized:
                            return AuthorizationStartResponse.Authorized               (Request,
                                                                                        response.SessionId. ToOICP(),
                                                                                        default,
                                                                                        default,
                                                                                        response.ProviderId.ToOICP(),
                                                                                        "Ready to charge!",
                                                                                        default,
                                                                                        response.ListOfAuthStopTokens.
                                                                                            SafeSelect(token => Identification.FromUID(token.ToOICP())));

                        case WWCP.AuthStartResultTypes.NotAuthorized:
                            return AuthorizationStartResponse.NotAuthorized            (Request,
                                                                                        new StatusCode(
                                                                                            StatusCodes.RFIDAuthenticationfailed_InvalidUID,
                                                                                            "RFID Authentication failed - invalid UID")
                                                                                        );

                        case WWCP.AuthStartResultTypes.InvalidSessionId:
                            return AuthorizationStartResponse.SessionIsInvalid         (Request,
                                                                                        SessionId:            Request.SessionId,
                                                                                        CPOPartnerSessionId:  Request.CPOPartnerSessionId,
                                                                                        EMPPartnerSessionId:  Request.EMPPartnerSessionId);

                        case WWCP.AuthStartResultTypes.CommunicationTimeout:
                            return AuthorizationStartResponse.CommunicationToEVSEFailed(Request);

                        case WWCP.AuthStartResultTypes.StartChargingTimeout:
                            return AuthorizationStartResponse.NoEVConnectedToEVSE      (Request);

                        case WWCP.AuthStartResultTypes.Reserved:
                            return AuthorizationStartResponse.EVSEAlreadyReserved      (Request);

                        case WWCP.AuthStartResultTypes.UnknownLocation:
                            return AuthorizationStartResponse.UnknownEVSEID            (Request);

                        case WWCP.AuthStartResultTypes.OutOfService:
                            return AuthorizationStartResponse.EVSEOutOfService         (Request);

                    }
                }

                #endregion

                return AuthorizationStartResponse.ServiceNotAvailable(
                           Request,
                           SessionId:  response?.SessionId. ToOICP() ?? Request.SessionId,
                           ProviderId: response?.ProviderId.ToOICP()
                       );

            };

            #endregion

            #region OnAuthorizeStop

            this.EMPRoaming.OnAuthorizeStop += async (Timestamp,
                                                      Sender,
                                                      Request) => {

                #region Map parameter values

                var sessionId            = Request.SessionId.          ToWWCP();
                var localAuthentication  = Request.Identification.     ToWWCP().ToLocal;
                var chargingLocation     = WWCP.ChargingLocation.FromEVSEId(Request.EVSEId?.ToWWCP());
                var CPOPartnerSessionId  = Request.CPOPartnerSessionId.ToWWCP();
                var operatorId           = Request.OperatorId.         ToWWCP();

                #endregion

                #region Send OnAuthorizeStopRequest event

                var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

                try
                {

                    OnAuthorizeStopRequest?.Invoke(StartTime,
                                                    Timestamp,
                                                    this,
                                                    Id.ToString(),
                                                    Request.EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    Id,
                                                    null,
                                                    operatorId,
                                                    chargingLocation,
                                                    sessionId,
                                                    CPOPartnerSessionId,
                                                    localAuthentication,
                                                    Request.RequestTimeout);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnAuthorizeStopRequest));
                }

                #endregion


                var response = await RoamingNetwork.AuthorizeStop(sessionId.Value,
                                                                  localAuthentication,
                                                                  chargingLocation,
                                                                  CPOPartnerSessionId,
                                                                  operatorId,

                                                                  Request.Timestamp,
                                                                  Request.CancellationToken,
                                                                  Request.EventTrackingId,
                                                                  Request.RequestTimeout);


                #region Send OnAuthorizeStopResponse event

                var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

                try
                {

                    OnAuthorizeStopResponse?.Invoke(EndTime,
                                                    Timestamp,
                                                    this,
                                                    Id.ToString(),
                                                    Request.EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    Id,
                                                    null,
                                                    operatorId,
                                                    chargingLocation,
                                                    sessionId,
                                                    CPOPartnerSessionId,
                                                    localAuthentication,
                                                    Request.RequestTimeout,
                                                    response,
                                                    EndTime - StartTime);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnAuthorizeStopResponse));
                }

                #endregion

                #region Map response

                if (response != null)
                {
                    switch (response.Result)
                    {

                        case WWCP.AuthStopResultTypes.Authorized:
                                return AuthorizationStopResponse.Authorized          (Request,
                                                                                      response.SessionId. ToOICP(),
                                                                                      default,
                                                                                      default,
                                                                                      response.ProviderId.ToOICP(),
                                                                                      "Ready to stop charging!");

                        case WWCP.AuthStopResultTypes.InvalidSessionId:
                            return AuthorizationStopResponse.SessionIsInvalid         (Request);

                        case WWCP.AuthStopResultTypes.CommunicationTimeout:
                            return AuthorizationStopResponse.CommunicationToEVSEFailed(Request);

                        case WWCP.AuthStopResultTypes.StopChargingTimeout:
                            return AuthorizationStopResponse.NoEVConnectedToEVSE      (Request);

                        case WWCP.AuthStopResultTypes.UnknownLocation:
                            return AuthorizationStopResponse.UnknownEVSEID            (Request);

                        case WWCP.AuthStopResultTypes.OutOfService:
                            return AuthorizationStopResponse.EVSEOutOfService         (Request);

                    }
                }

                #endregion

                return AuthorizationStopResponse.ServiceNotAvailable(
                            Request,
                            SessionId:  response?.SessionId. ToOICP() ?? Request.SessionId,
                            ProviderId: response?.ProviderId.ToOICP()
                        );

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

                var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

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
                    DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnChargeDetailRecordRequest));
                }

                #endregion


                var response = await RoamingNetwork.SendChargeDetailRecords(CDRs,
                                                                            WWCP.TransmissionTypes.Direct,

                                                                            ChargeDetailRecordRequest.Timestamp,
                                                                            ChargeDetailRecordRequest.CancellationToken,
                                                                            ChargeDetailRecordRequest.EventTrackingId,
                                                                            ChargeDetailRecordRequest.RequestTimeout);


                #region Send OnChargeDetailRecordResponse event

                var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

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
                    DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnChargeDetailRecordResponse));
                }

                #endregion

                #region Map response

                if (response != null)
                {

                    if (response.Result == WWCP.SendCDRsResultTypes.Success)
                        return Acknowledgement<ChargeDetailRecordRequest>.Success(
                                   ChargeDetailRecordRequest,
                                   ChargeDetailRecordRequest.ChargeDetailRecord.SessionId,
                                   ChargeDetailRecordRequest.ChargeDetailRecord.CPOPartnerSessionId,
                                   ChargeDetailRecordRequest.ChargeDetailRecord.EMPPartnerSessionId,
                                   "Charge detail record forwarded!"
                               );

                    var FailedCDR = response.ResultMap.FirstOrDefault();

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

                            case WWCP.SendCDRResultTypes.InvalidSessionId:
                                return Acknowledgement<ChargeDetailRecordRequest>.SessionIsInvalid(
                                           ChargeDetailRecordRequest,
                                           SessionId:            ChargeDetailRecordRequest.ChargeDetailRecord.SessionId,
                                           CPOPartnerSessionId:  ChargeDetailRecordRequest.ChargeDetailRecord.CPOPartnerSessionId,
                                           EMPPartnerSessionId:  ChargeDetailRecordRequest.ChargeDetailRecord.EMPPartnerSessionId
                                       );

                            case WWCP.SendCDRResultTypes.UnknownLocation:
                                return Acknowledgement<ChargeDetailRecordRequest>.UnknownEVSEID(
                                           ChargeDetailRecordRequest,
                                           SessionId:            ChargeDetailRecordRequest.ChargeDetailRecord.SessionId,
                                           CPOPartnerSessionId:  ChargeDetailRecordRequest.ChargeDetailRecord.CPOPartnerSessionId,
                                           EMPPartnerSessionId:  ChargeDetailRecordRequest.ChargeDetailRecord.EMPPartnerSessionId
                                       );

                            case WWCP.SendCDRResultTypes.Error:
                                return Acknowledgement<ChargeDetailRecordRequest>.DataError(
                                           ChargeDetailRecordRequest,
                                           SessionId:            ChargeDetailRecordRequest.ChargeDetailRecord.SessionId,
                                           CPOPartnerSessionId:  ChargeDetailRecordRequest.ChargeDetailRecord.CPOPartnerSessionId,
                                           EMPPartnerSessionId:  ChargeDetailRecordRequest.ChargeDetailRecord.EMPPartnerSessionId
                                       );

                        }
                    }

                }

                #endregion

                return Acknowledgement<ChargeDetailRecordRequest>.ServiceNotAvailable(
                           ChargeDetailRecordRequest,
                           SessionId: ChargeDetailRecordRequest.ChargeDetailRecord.SessionId
                       );

            };

            #endregion

        }

        #endregion



        public Boolean TryGetChargingReservationById(WWCP.ChargingReservation_Id ReservationId, out WWCP.ChargingReservation ChargingReservation)
        {
            throw new NotImplementedException();
        }

        public Boolean TryGetChargingSessionById(WWCP.ChargingSession_Id ChargingSessionId, out WWCP.ChargingSession ChargingSession)
        {
            throw new NotImplementedException();
        }


        public event WWCP.OnNewReservationDelegate         OnNewReservation;
        public event WWCP.OnReservationCanceledDelegate    OnReservationCanceled;
        public event WWCP.OnNewChargingSessionDelegate     OnNewChargingSession;
        public event WWCP.OnNewChargeDetailRecordDelegate  OnNewChargeDetailRecord;


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
        public async Task<WWCP.POIDataPull<WWCP.EVSE>>

            PullEVSEData(DateTime?                                     LastCall            = null,
                         GeoCoordinate?                                SearchCenter        = null,
                         Single                                        DistanceKM          = 0f,
                         WWCP.eMobilityProvider_Id?                    ProviderId          = null,
                         IEnumerable<WWCP.ChargingStationOperator_Id>  OperatorIdFilter    = null,
                         IEnumerable<Country>                          CountryCodeFilter   = null,

                         DateTime?                                     Timestamp           = null,
                         CancellationToken?                            CancellationToken   = null,
                         EventTracking_Id                              EventTrackingId     = null,
                         TimeSpan?                                     RequestTimeout      = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;

            #endregion

           // var providerId  = ProviderId.ToOICP() ?? DefaultProviderId;

            var result      = await EMPRoaming.PullEVSEData(
                                 new PullEVSEDataRequest(
                                     ProviderId.ToOICP() ?? DefaultProviderId,
                                     LastCall,

                                     null, // OperatorIdFilter
                                     null, // CountryCodeFilter
                                     null, // AccessibilityFilter
                                     null, // AuthenticationModeFilter
                                     null, // CalibrationLawDataAvailabilityFilter
                                     null, // RenewableEnergyFilter
                                     null, // IsHubjectCompatibleFilter
                                     null, // IsOpen24HoursFilter

                                     null, // SearchCenter
                                     null, // DistanceKM
                                     null, // GeoCoordinatesResponseFormat

                                     null, // ProcessId
                                     null, // Page
                                     null, // Size
                                     null, // SortOrder
                                     null, // CustomData

                                     Timestamp,
                                     CancellationToken,
                                     EventTrackingId,
                                     RequestTimeout)).
                                 ConfigureAwait(false);


            if (result.IsSuccess())
            {

                var WWCPEVSEs    = new List<WWCP.EVSE>();
                var Warnings     = new List<String>();
                WWCP.EVSE _EVSE  = null;

                foreach (var evseDataRecord in result.Response.EVSEDataRecords)
                {

                    try
                    {

                        _EVSE = evseDataRecord.ToWWCP();

                        if (_EVSE != null)
                            WWCPEVSEs.Add(_EVSE);

                        else
                            Warnings.Add("Could not convert EVSE '" + evseDataRecord.Id + "'!");

                    }
                    catch (Exception e)
                    {
                        Warnings.Add(evseDataRecord.Id + " - " + e.Message);
                    }

                }

            }

            return new WWCP.POIDataPull<WWCP.EVSE>(Array.Empty<WWCP.EVSE>(),
                                                   Warning.Create(I18NString.Create(Languages.en, result.Response.HTTPResponse.HTTPStatusCode +
                                                                                                 (result.Response.HTTPResponse.ContentLength.HasValue && result.Response.HTTPResponse.ContentLength.Value > 0
                                                                                                      ? Environment.NewLine + result.Response.HTTPResponse.HTTPBody.ToUTF8String()
                                                                                                      : ""))));

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
        public async Task<WWCP.StatusPull<WWCP.EVSEStatus>>

            PullEVSEStatus(DateTime?                   LastCall            = null,
                           GeoCoordinate?              SearchCenter        = null,
                           Single                      DistanceKM          = 0f,
                           WWCP.EVSEStatusTypes?       EVSEStatusFilter    = null,
                           WWCP.eMobilityProvider_Id?  ProviderId          = null,

                           DateTime?                   Timestamp           = null,
                           CancellationToken?          CancellationToken   = null,
                           EventTracking_Id            EventTrackingId     = null,
                           TimeSpan?                   RequestTimeout      = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;

            #endregion

            #region Send OnPushAuthenticationDataRequest event

            //var StartTime = Timestamp.Now;

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
            //    DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnPushAuthenticationDataRequest));
            //}

            #endregion


            //var providerId           = ProviderId.ToOICP() ?? DefaultProviderId;

            var pullEVSEStatusResult = await EMPRoaming.PullEVSEStatus(
                                             new PullEVSEStatusRequest(
                                                 ProviderId:         ProviderId.ToOICP() ?? DefaultProviderId,
                                                 SearchCenter:       SearchCenter.    ToOICP(),
                                                 DistanceKM:         DistanceKM,
                                                 EVSEStatusFilter:   EVSEStatusFilter.ToOICP(),
                                                 CustomData:         null,

                                                 Timestamp:          Timestamp,
                                                 CancellationToken:  CancellationToken,
                                                 EventTrackingId:    EventTrackingId,
                                                 RequestTimeout:     RequestTimeout)
                                             ).ConfigureAwait(false);

            if (pullEVSEStatusResult.IsSuccess())
            {

                var EVSEStatusList = new List<WWCP.EVSEStatus>();
                var Warnings       = new List<Warning>();

            //    WWCP.EVSE_Id?          evseId       = default;
            //    WWCP.EVSEStatusTypes?  evseStatus   = default;

                foreach (var operatorEVSEStatus in pullEVSEStatusResult.Response.OperatorEVSEStatus)
                {
                    foreach (var evseStatusRecord in operatorEVSEStatus.EVSEStatusRecords)
                    {

                        var evseId      = evseStatusRecord.Id.    ToWWCP();
                        var evseStatus  = evseStatusRecord.Status.ToWWCP();

                        if      (!evseId.HasValue)
                            Warnings.Add(Warning.Create(I18NString.Create(Languages.en, "Invalid EVSE identification '" + evseStatusRecord.Id + "'!")));

                        else if (!evseStatus.HasValue)
                            Warnings.Add(Warning.Create(I18NString.Create(Languages.en, "Invalid EVSE identification '" + evseStatusRecord.Id + "'!")));

                        else
                            EVSEStatusList.Add(new WWCP.EVSEStatus(
                                                   evseId.Value,
                                                   new Timestamped<WWCP.EVSEStatusTypes>(
                                                       pullEVSEStatusResult.Response.HTTPResponse.Timestamp,
                                                       evseStatus.Value
                                                   )
                                                   //evseStatusRecord.CustomData)
                                               ));

                    }
                }

                return new WWCP.StatusPull<WWCP.EVSEStatus>(EVSEStatusList, Warnings);

            }

            return new WWCP.StatusPull<WWCP.EVSEStatus>(Array.Empty<WWCP.EVSEStatus>(),
                                                        new Warning[] {
                                                            Warning.Create(I18NString.Create(Languages.en, pullEVSEStatusResult.Response.HTTPResponse.HTTPStatusCode.ToString())),
                                                            Warning.Create(I18NString.Create(Languages.en, pullEVSEStatusResult.Response.HTTPResponse.HTTPBody.ToUTF8String()))
                                                        });

        }

        #endregion

        #region PullEVSEStatusById(EVSEId,  ProviderId = null, ...)

        ///// <summary>
        ///// Check the current status of the given EVSE Ids.
        ///// </summary>
        ///// <param name="EVSEIds">An enumeration of EVSE Ids.</param>
        ///// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //public async Task<WWCP.StatusPull<WWCP.EVSEStatus>>

        //    PullEVSEStatusById(WWCP.EVSE_Id                EVSEId,
        //                       WWCP.eMobilityProvider_Id?  ProviderId          = null,

        //                       DateTime?                   Timestamp           = null,
        //                       CancellationToken?          CancellationToken   = null,
        //                       EventTracking_Id            EventTrackingId     = null,
        //                       TimeSpan?                   RequestTimeout      = null)

        //{

        //    #region Initial checks

        //    if (EVSEId == null)
        //        return new WWCP.StatusPull<WWCP.EVSEStatus>(new WWCP.EVSEStatus[0],
        //                                                    new Warning[] { Warning.Create(I18NString.Create(Languages.en, "Parameter 'EVSEId' was null!")) });

        //    #endregion

        //    return await PullEVSEStatusById(new WWCP.EVSE_Id[] { EVSEId },
        //                                    ProviderId,

        //                                    Timestamp,
        //                                    CancellationToken,
        //                                    EventTrackingId,
        //                                    RequestTimeout);

        //}

        #endregion

        #region PullEVSEStatusById(EVSEIds, ProviderId = null, ...)

        ///// <summary>
        ///// Check the current status of the given EVSE Ids.
        ///// </summary>
        ///// <param name="EVSEIds">An enumeration of EVSE Ids.</param>
        ///// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //public async Task<WWCP.StatusPull<WWCP.EVSEStatus>>

        //    PullEVSEStatusById(IEnumerable<WWCP.EVSE_Id>   EVSEIds,
        //                       WWCP.eMobilityProvider_Id?  ProviderId          = null,

        //                       DateTime?                   Timestamp           = null,
        //                       CancellationToken?          CancellationToken   = null,
        //                       EventTracking_Id            EventTrackingId     = null,
        //                       TimeSpan?                   RequestTimeout      = null)

        //{

        //    #region Initial checks

        //    if (EVSEIds.IsNullOrEmpty())
        //        return new WWCP.StatusPull<WWCP.EVSEStatus>(new WWCP.EVSEStatus[0],
        //                                                    new Warning[] { Warning.Create(I18NString.Create(Languages.en, "Parameter 'EVSEIds' was null or empty!")) });

        //    #endregion


        //    var EVSEStatusList  = new List<WWCP.EVSEStatus>();
        //    var Warnings        = new List<Warning>();

        //    // Hubject has a limit of 100 EVSEIds per request!
        //    // Do not make concurrent requests!
        //    foreach (var evsepart in EVSEIds.Select(evse => evse.ToOICP()).
        //                                     Where (evse => evse.HasValue).
        //                                     Select(evse => evse.Value).
        //                                     ToPartitions(100))
        //    {

        //        var result = await EMPRoaming.PullEVSEStatusById(evsepart,
        //                                                         ProviderId.HasValue
        //                                                             ? ProviderId.Value.ToOICP()
        //                                                             : DefaultProviderId.Value,

        //                                                         Timestamp,
        //                                                         CancellationToken,
        //                                                         EventTrackingId,
        //                                                         RequestTimeout).
        //                                      ConfigureAwait(false);

        //        if (result.HTTPStatusCode == HTTPStatusCode.OK &&
        //            result.Content != null)
        //        {

        //            WWCP.EVSE_Id? EVSEId = null;

        //            foreach (var evsestatusrecord in result.Content.EVSEStatusRecords)
        //            {

        //                EVSEId = evsestatusrecord.Id.ToWWCP();

        //                if (EVSEId.HasValue)
        //                    EVSEStatusList.Add(new WWCP.EVSEStatus(EVSEId.Value,
        //                                                           new Timestamped<WWCP.EVSEStatusTypes>(
        //                                                               result.Timestamp,
        //                                                               OICPMapper.AsWWCPEVSEStatus(evsestatusrecord.Status)
        //                                                           )));

        //                else
        //                    Warnings.Add(Warning.Create(I18NString.Create(Languages.en, "Invalid EVSE identification '" + evsestatusrecord.Id + "'!")));

        //            }

        //        }

        //        else
        //            Warnings.AddRange(new Warning[] {
        //                                  Warning.Create(I18NString.Create(Languages.en, result.HTTPStatusCode.ToString())),
        //                                  Warning.Create(I18NString.Create(Languages.en, result.HTTPBody.ToUTF8String()))
        //                              });

        //    }

        //    return new StatusPull<WWCP.EVSEStatus>(EVSEStatusList, Warnings);

        //}

        #endregion


        //ToDo: Implement PushAuthenticationData!
        #region PushAuthenticationData(...AuthorizationIdentifications, Action = fullLoad, ProviderId = null, ...)

        ///// <summary>
        ///// Create a new task pushing authorization identifications onto the OICP server.
        ///// </summary>
        ///// <param name="AuthorizationIdentifications">An enumeration of authorization identifications.</param>
        ///// <param name="Action">An optional OICP action.</param>
        ///// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //public async Task<PushAuthenticationDataResult>

        //    PushAuthenticationData(IEnumerable<Identification>  AuthorizationIdentifications,
        //                           WWCP.ActionTypes                   Action              = WWCP.ActionTypes.fullLoad,
        //                           eMobilityProvider_Id?        ProviderId          = null,

        //                           DateTime?                    Timestamp           = null,
        //                           CancellationToken?           CancellationToken   = null,
        //                           EventTracking_Id             EventTrackingId     = null,
        //                           TimeSpan?                    RequestTimeout      = null)

        //{

        //    #region Initial checks

        //    if (AuthorizationIdentifications.IsNullOrEmpty())
        //        return PushAuthenticationDataResult.NoOperation(Id, this);


        //    PushAuthenticationDataResult result = null;

        //    #endregion

        //    #region Send OnPushAuthenticationDataRequest event

        //    //var StartTime = Timestamp.Now;

        //    //try
        //    //{

        //    //    OnPushAuthenticationDataRequest?.Invoke(StartTime,
        //    //                                            Request.Timestamp.Value,
        //    //                                            this,
        //    //                                            ClientId,
        //    //                                            Request.EventTrackingId,
        //    //                                            Request.AuthorizationIdentifications,
        //    //                                            Request.ProviderId,
        //    //                                            Request.OICPAction,
        //    //                                            RequestTimeout);

        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnPushAuthenticationDataRequest));
        //    //}

        //    #endregion


        //    var response = await EMPRoaming.PushAuthenticationData(AuthorizationIdentifications,
        //                                                           ProviderId.HasValue
        //                                                               ? ProviderId.Value.ToOICP()
        //                                                               : DefaultProviderId.Value,
        //                                                           Action.    ToOICP(),

        //                                                           Timestamp,
        //                                                           CancellationToken,
        //                                                           EventTrackingId,
        //                                                           RequestTimeout).
        //                                    ConfigureAwait(false);

        //    if (response.HTTPStatusCode == HTTPStatusCode.OK &&
        //        response.Content        != null)
        //    {

        //        result = response.Content.Result

        //                     ? PushAuthenticationDataResult.Success(Id,
        //                                                            this,
        //                                                            response.Content.StatusCode.Description,
        //                                                            response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
        //                                                                ? new String[] { response.Content.StatusCode.AdditionalInfo }
        //                                                                : null)

        //                     : PushAuthenticationDataResult.Error(Id,
        //                                                          this,
        //                                                          null,
        //                                                          response.Content.StatusCode.Description,
        //                                                          response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
        //                                                              ? new String[] { response.Content.StatusCode.AdditionalInfo }
        //                                                              : null);

        //    }

        //    else
        //        result = PushAuthenticationDataResult.Error(Id,
        //                                                    this,
        //                                                    null,
        //                                                    response.Content != null
        //                                                        ? response.Content.StatusCode.Description
        //                                                        : null,
        //                                                    response.Content != null
        //                                                        ? response.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
        //                                                              ? new String[] { response.Content.StatusCode.AdditionalInfo }
        //                                                              : null
        //                                                        : null);


        //    #region Send OnPushAuthenticationDataResponse event

        //    //var Endtime = Timestamp.Now;
        //    //
        //    //try
        //    //{
        //    //
        //    //    OnPushAuthenticationDataResponse?.Invoke(Endtime,
        //    //                                             this,
        //    //                                             ClientId,
        //    //                                             Request.EventTrackingId,
        //    //                                             Request.AuthorizationIdentifications,
        //    //                                             Request.ProviderId,
        //    //                                             Request.OICPAction,
        //    //                                             RequestTimeout,
        //    //                                             response.Content,
        //    //                                             Endtime - StartTime);
        //    //
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPushAuthenticationDataResponse));
        //    //}

        //    #endregion

        //    return result;

        //}

        #endregion


        #region Reserve(EVSEId, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

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
        async Task<WWCP.ReservationResult>

            WWCP.IReserveRemoteStartStop.Reserve(WWCP.ChargingLocation                  ChargingLocation,
                                                 WWCP.ChargingReservationLevel          ReservationLevel,
                                                 DateTime?                              ReservationStartTime,
                                                 TimeSpan?                              Duration,
                                                 WWCP.ChargingReservation_Id?           ReservationId,
                                                 WWCP.eMobilityProvider_Id?             ProviderId,
                                                 WWCP.RemoteAuthentication              RemoteAuthentication,
                                                 WWCP.ChargingProduct                   ChargingProduct,
                                                 IEnumerable<WWCP.Auth_Token>           AuthTokens,
                                                 IEnumerable<WWCP.eMobilityAccount_Id>  eMAIds,
                                                 IEnumerable<UInt32>                    PINs,

                                                 DateTime?                              Timestamp,
                                                 CancellationToken?                     CancellationToken,
                                                 EventTracking_Id                       EventTrackingId,
                                                 TimeSpan?                              RequestTimeout)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;


            WWCP.ReservationResult result = null;

            #endregion

            #region Send OnReserveRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveRequest?.Invoke(StartTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             ReservationId,
                                             ChargingLocation,
                                             ReservationStartTime,
                                             Duration,
                                             ProviderId,
                                             RemoteAuthentication,
                                             ChargingProduct,
                                             AuthTokens,
                                             eMAIds,
                                             PINs,
                                             RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnReserveRequest));
            }

            #endregion


            var EVSEId = ChargingLocation.EVSEId.Value;


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

            if (eMAIds == null && RemoteAuthentication?.RemoteIdentification.HasValue == true)
                eMAIds = new List<WWCP.eMobilityAccount_Id> { RemoteAuthentication.RemoteIdentification.Value };

            if (eMAIds != null && RemoteAuthentication?.RemoteIdentification.HasValue == true && !eMAIds.Contains(RemoteAuthentication.RemoteIdentification.Value))
            {
                var _eMAIds = new List<WWCP.eMobilityAccount_Id>(eMAIds);
                _eMAIds.Add(RemoteAuthentication.RemoteIdentification.Value);
                eMAIds = _eMAIds;
            }

            #endregion


            //var providerId       = ProviderId.ToOICP() ?? DefaultProviderId;

            var reserveResponse  = await EMPRoaming.AuthorizeRemoteReservationStart(
                                         new AuthorizeRemoteReservationStartRequest(
                                             EVSEId:                EVSEId.ToOICP().Value,
                                             ProviderId:            ProviderId.ToOICP() ?? DefaultProviderId,
                                             Identification:        RemoteAuthentication.ToOICP(),
                                             Duration:              Duration,
                                             SessionId:             ReservationId != null ? Session_Id.Parse(ReservationId.ToString()) : new Session_Id?(),
                                             CPOPartnerSessionId:   null,
                                             EMPPartnerSessionId:   null,
                                             PartnerProductId:      PartnerProductIdElements.Count > 0
                                                                        ? PartnerProduct_Id.Parse(PartnerProductIdElements.
                                                                                                      Select(kvp => kvp.Key + "=" + kvp.Value).
                                                                                                      AggregateWith("|"))
                                                                        : default(PartnerProduct_Id?),

                                             Timestamp:             Timestamp,
                                             CancellationToken:     CancellationToken,
                                             EventTrackingId:       EventTrackingId,
                                             RequestTimeout:        RequestTimeout)).
                                         ConfigureAwait(false);


            if (reserveResponse.IsSuccess())
            {

                result = WWCP.ReservationResult.Success(reserveResponse.Response.SessionId != null
                                                        ? new WWCP.ChargingReservation(Id:                        WWCP.ChargingReservation_Id.Parse(EVSEId.OperatorId.ToString() +
                                                                                                                      "*R" + reserveResponse.Response.SessionId.ToString()),
                                                                                       Timestamp:                 org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                                       StartTime:                 org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                                       Duration:                  Duration ?? DefaultReservationTime,
                                                                                       EndTime:                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now + (Duration ?? DefaultReservationTime),
                                                                                       ConsumedReservationTime:   TimeSpan.FromSeconds(0),
                                                                                       ReservationLevel:          WWCP.ChargingReservationLevel.EVSE,
                                                                                       ProviderId:                ProviderId,
                                                                                       StartAuthentication:       RemoteAuthentication,
                                                                                       RoamingNetworkId:          RoamingNetwork.Id,
                                                                                       ChargingPoolId:            null,
                                                                                       ChargingStationId:         null,
                                                                                       EVSEId:                    EVSEId,
                                                                                       ChargingProduct:           ChargingProduct,
                                                                                       AuthTokens:                AuthTokens,
                                                                                       eMAIds:                    eMAIds,
                                                                                       PINs:                      PINs)
                                                        : null);

            }

            else
                result = WWCP.ReservationResult.Error(reserveResponse.Response.HTTPResponse.HTTPStatusCode.ToString(),
                                                      reserveResponse);


            #region Send OnReserveResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveResponse?.Invoke(EndTime,
                                          Timestamp.Value,
                                          this,
                                          EventTrackingId,
                                          RoamingNetwork.Id,
                                          ReservationId,
                                          ChargingLocation,
                                          ReservationStartTime,
                                          Duration,
                                          ProviderId,
                                          RemoteAuthentication,
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
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnReserveResponse));
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
        async Task<WWCP.CancelReservationResult>

            WWCP.IReserveRemoteStartStop.CancelReservation(WWCP.ChargingReservation_Id                 ReservationId,
                                                           WWCP.ChargingReservationCancellationReason  Reason,
                                                           //eMobilityProvider_Id?                       ProviderId,  // = null,
                                                           //WWCP.EVSE_Id?                               EVSEId,      // = null,

                                                           DateTime?                                   Timestamp,
                                                           CancellationToken?                          CancellationToken,
                                                           EventTracking_Id                            EventTrackingId,
                                                           TimeSpan?                                   RequestTimeout)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;

            #endregion

            #region Send OnCancelReservationRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnCancelReservationRequest?.Invoke(StartTime,
                                                   Timestamp.Value,
                                                   this,
                                                   EventTrackingId,
                                                   RoamingNetwork.Id,
                                                   //ProviderId.Value,
                                                   ReservationId,
                                                   Reason,
                                                   RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            RoamingNetwork.ReservationsStore.TryGetLatest(ReservationId, out WWCP.ChargingReservation reservation);

            var providerId  = reservation.ProviderId.ToOICP();
            var evseId      = reservation.EVSEId.    ToOICP();
            var sessionId   = reservation.Id.        ToOICP();

            var result      = await EMPRoaming.AuthorizeRemoteReservationStop(
                                    new AuthorizeRemoteReservationStopRequest(
                                        ProviderId:            providerId.Value,
                                        EVSEId:                evseId.    Value,
                                        SessionId:             sessionId. Value,
                                        CPOPartnerSessionId:   null,
                                        EMPPartnerSessionId:   null,
                                        CustomData:            null,

                                        Timestamp:             Timestamp,
                                        CancellationToken:     CancellationToken,
                                        EventTrackingId:       EventTrackingId,
                                        RequestTimeout:        RequestTimeout)).
                                    ConfigureAwait(false);

            if (result.IsSuccess())
            {

                return WWCP.CancelReservationResult.Success(ReservationId,
                                                            Reason);

            }

            return WWCP.CancelReservationResult.Error(ReservationId,
                                                      Reason,
                                                      result.Response.HTTPResponse.HTTPStatusCode.ToString(),
                                                      result.Response.HTTPResponse.EntirePDU);

        }

        #endregion


        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The charging product to use.</param>
        /// <param name="ReservationId">An optional identification of a charging reservation.</param>
        /// <param name="SessionId">An optional identification of this charging session.</param>
        /// <param name="ProviderId">An optional identification of the e-mobility service provider, whenever this identification is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">An optional identification of the e-mobility account who wants to charge.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.RemoteStartResult>

            WWCP.IReserveRemoteStartStop.RemoteStart(WWCP.ChargingLocation         ChargingLocation,
                                                     WWCP.ChargingProduct          ChargingProduct,       // = null,
                                                     WWCP.ChargingReservation_Id?  ReservationId,         // = null,
                                                     WWCP.ChargingSession_Id?      SessionId,             // = null,
                                                     WWCP.eMobilityProvider_Id?    ProviderId,            // = null,
                                                     WWCP.RemoteAuthentication     RemoteAuthentication,  // = null,

                                                     DateTime?                     Timestamp,
                                                     CancellationToken?            CancellationToken,
                                                     EventTracking_Id              EventTrackingId,
                                                     TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (RemoteAuthentication == null || !RemoteAuthentication.RemoteIdentification.HasValue)
                throw new ArgumentNullException(nameof(RemoteAuthentication),  "The e-mobility account identification is mandatory in OICP!");


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;


            WWCP.RemoteStartResult result = null;

            #endregion

            #region Send OnRemoteStartRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStartRequest?.Invoke(StartTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             ChargingLocation,
                                             ChargingProduct,
                                             ReservationId,
                                             SessionId,
                                             Id,
                                             null,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            var EVSEId = ChargingLocation.EVSEId.Value;

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


            //var providerId           = ProviderId.ToOICP() ?? DefaultProviderId;

            var remoteStartResponse  = await EMPRoaming.AuthorizeRemoteStart(
                                             new AuthorizeRemoteStartRequest(
                                                 ProviderId:           ProviderId.ToOICP() ?? DefaultProviderId,
                                                 EVSEId:               EVSEId.ToOICP().Value,
                                                 Identification:       RemoteAuthentication.ToOICP(),
                                                 SessionId:            SessionId.           ToOICP(),
                                                 CPOPartnerSessionId:  null,
                                                 EMPPartnerSessionId:  null,
                                                 PartnerProductId:     PartnerProductIdElements.Count > 0
                                                                           ? new PartnerProduct_Id?(PartnerProduct_Id.Parse(PartnerProductIdElements.
                                                                                                                            Select(kvp => kvp.Key + "=" + kvp.Value).
                                                                                                                            AggregateWith("|")))
                                                                           : null,

                                                 Timestamp:            Timestamp,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 RequestTimeout:       RequestTimeout)).
                                             ConfigureAwait(false);


            var Now      = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            var Runtime  = Now - Timestamp.Value;

            if (remoteStartResponse.IsSuccess())
            {

                result = WWCP.RemoteStartResult.Success(remoteStartResponse.Response.SessionId.HasValue
                                                            ? new WWCP.ChargingSession(remoteStartResponse.Response.SessionId.ToWWCP().Value)
                                                            : default,
                                                        Runtime);

            }

            else
                result = WWCP.RemoteStartResult.Error(remoteStartResponse.Response.HTTPResponse.HTTPStatusCode.ToString(),
                                                      remoteStartResponse.Response.HTTPResponse.HTTPBodyAsUTF8String,
                                                      Runtime);


            #region Send OnRemoteStartResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStartResponse?.Invoke(EndTime,
                                              Timestamp.Value,
                                              this,
                                              EventTrackingId,
                                              RoamingNetwork.Id,
                                              ChargingLocation,
                                              ChargingProduct,
                                              ReservationId,
                                              SessionId,
                                              Id,
                                              null,
                                              ProviderId,
                                              RemoteAuthentication,
                                              RequestTimeout,
                                              result,
                                              EndTime - StartTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnRemoteStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop(                   SessionId, ReservationHandling = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<WWCP.RemoteStopResult>

            WWCP.IReserveRemoteStartStop.RemoteStop(WWCP.ChargingSession_Id     SessionId,
                                                    WWCP.ReservationHandling?   ReservationHandling    = null,
                                                    WWCP.eMobilityProvider_Id?  ProviderId             = null,
                                                    WWCP.RemoteAuthentication   RemoteAuthentication   = null,

                                                    DateTime?                   Timestamp              = null,
                                                    CancellationToken?          CancellationToken      = null,
                                                    EventTracking_Id            EventTrackingId        = null,
                                                    TimeSpan?                   RequestTimeout         = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;


            WWCP.RemoteStopResult result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStopRequest?.Invoke(StartTime,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            RoamingNetwork.Id,
                                            SessionId,
                                            ReservationHandling,
                                            Id,
                                            null,
                                            ProviderId,
                                            RemoteAuthentication,
                                            RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            RoamingNetwork.SessionsStore.TryGet(SessionId, out WWCP.ChargingSession session);
            var EVSEId = session.EVSEId.Value;

            //var providerId          = ProviderId.ToOICP() ?? DefaultProviderId;
            var sessionId           = SessionId. ToOICP();
            var evseId              = EVSEId.    ToOICP();

            var remoteStopResponse  = await EMPRoaming.AuthorizeRemoteStop(
                                            new AuthorizeRemoteStopRequest(
                                                SessionId:            sessionId. Value,
                                                ProviderId:           ProviderId.ToOICP() ?? DefaultProviderId,
                                                EVSEId:               evseId.    Value,
                                                CPOPartnerSessionId:  null,
                                                EMPPartnerSessionId:  null,

                                                Timestamp:            Timestamp,
                                                CancellationToken:    CancellationToken,
                                                EventTrackingId:      EventTrackingId,
                                                RequestTimeout:       RequestTimeout)).
                                            ConfigureAwait(false);

            if (remoteStopResponse.IsSuccess())
            {

                result = WWCP.RemoteStopResult.Success(SessionId);

            }

            else
                result = WWCP.RemoteStopResult.Error(SessionId,
                                                     remoteStopResponse.Response.HTTPResponse.HTTPStatusCode.ToString(),
                                                     Runtime: org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - StartTime);


            #region Send OnRemoteStopResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStopResponse?.Invoke(EndTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             SessionId,
                                             ReservationHandling,
                                             Id,
                                             null,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout,
                                             result,
                                             EndTime - StartTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnRemoteStopResponse));
            }

            #endregion

            return result;

        }

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

            GetChargeDetailRecords(DateTime                    From,
                                   DateTime?                   To                  = null,
                                   WWCP.eMobilityProvider_Id?  ProviderId          = null,

                                   DateTime?                   Timestamp           = null,
                                   CancellationToken?          CancellationToken   = null,
                                   EventTracking_Id            EventTrackingId     = null,
                                   TimeSpan?                   RequestTimeout      = null)

        {

            #region Initial checks

            if (!To.HasValue)
                To = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;


            IEnumerable<WWCP.ChargeDetailRecord> result = null;

            #endregion

            #region Send OnGetChargeDetailRecordsRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

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
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnGetChargeDetailRecordsRequest));
            }

            #endregion


            var providerId  = ProviderId.ToOICP() ?? DefaultProviderId;

            //var response    = await EMPRoaming.GetChargeDetailRecords(
            //                        new GetChargeDetailRecordsRequest(
            //                            providerId,
            //                            From,
            //                            To.Value,

            //                            Timestamp,
            //                            CancellationToken,
            //                            EventTrackingId,
            //                            RequestTimeout)).
            //                        ConfigureAwait(false);

            //if (response.HTTPStatusCode == HTTPStatusCode.OK &&
            //    response.Content        != null)
            //{

            //    var Warnings = new List<String>();

            //    result = response.Content.
            //                 ChargeDetailRecords.
            //                 SafeSelect(cdr => {

            //                                       try
            //                                       {

            //                                           return cdr.ToWWCP();

            //                                       }
            //                                       catch (Exception e)
            //                                       {
            //                                           Warnings.Add("Error during import of charge detail record: " + e.Message);
            //                                           return null;
            //                                       }

            //                                   }).
            //                 SafeWhere(cdr => cdr != null);

            //}

            //else
            //    result = new WWCP.ChargeDetailRecord[0];


            #region Send OnGetChargeDetailRecordsResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

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
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnGetChargeDetailRecordsResponse));
            }

            #endregion

            return result;

        }

        #endregion


        // -----------------------------------------------------------------------------------------------------

        #region (timer) PullOperatorInfosService(State)

        private void PullOperatorInfosService(Object? State)
        {
            if (!PullOperatorInfos_IsDisabled)
                PullOperatorInfos(State).Wait();
        }

        private async Task PullOperatorInfos(Object? State)
        {

            DebugX.LogT("[" + Id + "] 'Pull operator infos service', as every " + PullEVSEData_Every.TotalMinutes + " minutes!");

            if (await PullEVSEDataLock.WaitAsync(SemaphoreSlimTimeout))
            {
                try
                {

                    Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

                    var StartTime                       = Timestamp.Now;
                    var eventTrackingId                 = EventTracking_Id.New;
                    var requestPage                     = 0U;
                    var TimestampBeforeLastPullDataRun  = Timestamp.Now;
                    var operatorInfos                   = new Dictionary<Operator_Id, OperatorInfo>();
                    var finished                        = false;

                    var IllegalOperatorsIds             = 0UL;
                    var OperatorsSkipped                = 0UL;
                    var TotalEVSEsCreated               = 0UL;
                    var TotalEVSEsUpdated               = 0UL;
                    var TotalEVSEsSkipped               = 0UL;

                    var EVSEsFailed                     = 0UL;
                    var EVSEsSkipped                    = 0UL;

                    DebugX.LogT("[" + Id + "] 'Pull operator infos service' started at " + StartTime.ToIso8601());

                    do
                    {

                        var pullEVSEDataResponse  = await EMPRoaming.PullEVSEData(
                                                          new PullEVSEDataRequest(
                                                              ProviderId:                            DefaultProviderId,
                                                              LastCall:                              TimestampOfLastPullDataRun,

                                                              OperatorIdFilter:                      PullEVSEData_OperatorIdFilter,
                                                              CountryCodeFilter:                     PullEVSEData_CountryCodeFilter,
                                                              AccessibilityFilter:                   PullEVSEData_AccessibilityFilter,
                                                              AuthenticationModeFilter:              PullEVSEData_AuthenticationModeFilter,
                                                              CalibrationLawDataAvailabilityFilter:  PullEVSEData_CalibrationLawDataAvailabilityFilter,
                                                              RenewableEnergyFilter:                 PullEVSEData_RenewableEnergyFilter,
                                                              IsHubjectCompatibleFilter:             PullEVSEData_IsHubjectCompatibleFilter,
                                                              IsOpen24HoursFilter:                   PullEVSEData_IsOpen24HoursFilter,

                                                              SearchCenter:                          DefaultSearchCenter?.ToOICP(),
                                                              DistanceKM:                            DefaultDistanceKM ?? 0,
                                                              GeoCoordinatesResponseFormat:          GeoCoordinatesFormats.DecimalDegree,

                                                              Page:                                  requestPage,
                                                              Size:                                  PullEVSEData_RequestPageSize,
                                                              SortOrder:                             null,

                                                              CustomData:                            null,

                                                              Timestamp:                             null,
                                                              CancellationToken:                     new CancellationTokenSource().Token,
                                                              EventTrackingId:                       eventTrackingId,
                                                              RequestTimeout:                        PullEVSEData_RequestTimeout
                                                          )).ConfigureAwait(false);

                        #region Everything is ok!

                        if (pullEVSEDataResponse.IsSuccessful)
                        {

                            DebugX.Log(String.Concat("Imported ", pullEVSEDataResponse.Response.NumberOfElements, " OICP EVSE data records (page " + (requestPage + 1) + " of " + pullEVSEDataResponse.Response.TotalPages + ")"));

                            foreach (var evseDataRecordGroup in pullEVSEDataResponse.Response.EVSEDataRecords.GroupBy(evseDataRecord => evseDataRecord.OperatorId))
                            {

                                #region Get or create operator info

                                if (!operatorInfos.TryGetValue(evseDataRecordGroup.Key, out OperatorInfo operatorInfo))
                                {

                                    operatorInfo = new OperatorInfo(evseDataRecordGroup.Key,
                                                                    evseDataRecordGroup.FirstOrDefault()?.OperatorName);

                                    operatorInfos.Add(operatorInfo.OperatorId, operatorInfo);

                                }

                                #endregion

                                foreach (var currentEVSEDataRecord in evseDataRecordGroup)
                                {

                                    try
                                    {

                                        if (operatorInfo.OperatorName != currentEVSEDataRecord.OperatorName)
                                        {
                                            //ToDo: What else to do here?!
                                            DebugX.Log("OICP.WWCPCSOAdapter.PullOperatorInfos operatorInfo.OperatorName '" + operatorInfo.OperatorName + "' != '" + currentEVSEDataRecord.OperatorName + "'!");
                                        }

                                        operatorInfo.AddOrUpdateChargingPool(currentEVSEDataRecord);

                                    } catch (Exception e)
                                    {
                                        DebugX.Log("OICP.WWCPCSOAdapter.PullOperatorInfos failed: " + e.Message + Environment.NewLine + e.StackTrace);
                                        EVSEsFailed++;
                                    }

                                }

                            }


                            #region old

                                    //var EVSEIdLookup = operatorInfo.VerifyUniquenessOfChargingStationIds();

                                    //DebugX.Log(String.Concat(operatorInfo.                                                               Count(), " pools, ",
                                    //                         operatorInfo.SelectMany(_ => _.ChargingStations).                           Count(), " stations, ",
                                    //                         operatorInfo.SelectMany(_ => _.ChargingStations).SelectMany(_ => _.EVSEIds).Count(), " EVSEs imported. ",
                                    //                         EVSEsSkipped, " EVSEs skipped."));





                                        //#region Data

                                        //UInt64     ChargingPoolsCreated           = 0;
                                        //UInt64     chargingPoolsUpdated           = 0;
                                        //Languages  LocationLanguage               = Languages.unknown;
                                        //Languages  LocalChargingStationLanguage   = Languages.unknown;

                                        //UInt64     ChargingStationsCreated        = 0;
                                        //UInt64     chargingStationsUpdated        = 0;

                                        //UInt64     EVSEsCreated                   = 0;
                                        //UInt64     EVSEsUpdated                   = 0;

                                        //#endregion

                                        //foreach (var CurrentEVSEDataRecord in evseDataRecordGroup)
                                        //{

                                        //    var currentEVSEId = CurrentEVSEDataRecord.Id.ToWWCP();

                                        //    if (!currentEVSEId.HasValue || !EVSEIdLookup.Contains(CurrentEVSEDataRecord.Id))
                                        //        continue;

                                        //    try
                                        //    {


                                        //        //// Get or create charging station operator...
                                        //        //if (!RoamingNetwork.TryGetChargingStationOperatorById(chargingStationOperatorId, out WWCP.ChargingStationOperator WWCPChargingStationOperator))
                                        //        //    WWCPChargingStationOperator = RoamingNetwork.CreateChargingStationOperator(chargingStationOperatorId.Value,
                                        //        //                                                                               I18NString.Create(Languages.unknown,
                                        //        //                                                                                                 evseDataRecordGroup.Key.ToString()));


                                        //        var EVSEInfo = EVSEIdLookup[CurrentEVSEDataRecord.Id];

                                        //        #region Set LocationLanguage

                                        //        LocationLanguage = EVSEInfo.PoolAddress.Country.ToLanguages();

                                        //        #endregion

                                        //        #region Guess the language of the 'ChargingStationName' by '_Address.Country'

                                        //        //_ChargingStationName = new I18NString();

                                        //        //if (LocalChargingStationName.IsNotNullOrEmpty())
                                        //        //    _ChargingStationName.Add(LocalChargingStationLanguage,
                                        //        //                             LocalChargingStationName);

                                        //        //if (EnChargingStationName.IsNotNullOrEmpty())
                                        //        //    _ChargingStationName.Add(Languages.en,
                                        //        //                             EnChargingStationName);

                                        //        #endregion


                                        //        #region Update matching charging pool...

                                        //        if (WWCPChargingStationOperator.TryGetChargingPoolById(EVSEInfo.PoolId.ToWWCP().Value, out WWCP.ChargingPool chargingPool))
                                        //        {

                                        //            // External update via events!
                                        //            //_ChargingPool.Description           = CurrentEVSEDataRecord.AdditionalInfo;
                                        //            chargingPool.LocationLanguage      = LocationLanguage;
                                        //            chargingPool.EntranceLocation      = CurrentEVSEDataRecord.GeoChargingPointEntrance.ToWWCP();
                                        //            //_ChargingPool.OpeningTimes          = CurrentEVSEDataRecord.OpeningTimes != null ? OpeningTimes.Parse(CurrentEVSEDataRecord.OpeningTimes) : null;
                                        //            chargingPool.AuthenticationModes   = new ReactiveSet<WWCP.AuthenticationModes>(CurrentEVSEDataRecord.AuthenticationModes.SafeSelect(mode   => mode.  ToWWCP()));
                                        //            chargingPool.PaymentOptions        = new ReactiveSet<WWCP.PaymentOptions>     (CurrentEVSEDataRecord.PaymentOptions.     SafeSelect(option => option.ToWWCP()));
                                        //            chargingPool.Accessibility         = CurrentEVSEDataRecord.Accessibility.ToWWCP();
                                        //            chargingPool.HotlinePhoneNumber    = I18NString.Create(Languages.unknown, CurrentEVSEDataRecord.HotlinePhoneNumber.ToString());

                                        //            chargingPoolsUpdated++;

                                        //        }

                                        //        #endregion

                                        //        #region  ...or create a new one!

                                        //        else
                                        //        {

                                        //            // An operator might have multiple suboperator ids!
                                        //            if (!WWCPChargingStationOperator.Ids.Contains(EVSEInfo.OperatorId.ToWWCP().Value))
                                        //                WWCPChargingStationOperator.AddId(EVSEInfo.OperatorId.ToWWCP().Value);

                                        //            chargingPool = WWCPChargingStationOperator.CreateChargingPool(

                                        //                                EVSEInfo.PoolId.ToWWCP(),

                                        //                                Configurator: pool => {

                                        //                                    pool.DataSource                  = Id.ToString();
                                        //                                    //pool.Description                 = CurrentEVSEDataRecord.AdditionalInfo;
                                        //                                    pool.Address                     = CurrentEVSEDataRecord.Address.       ToWWCP();
                                        //                                    pool.GeoLocation                 = CurrentEVSEDataRecord.GeoCoordinates.ToWWCP();
                                        //                                    pool.LocationLanguage            = LocationLanguage;
                                        //                                    pool.EntranceLocation            = CurrentEVSEDataRecord.GeoChargingPointEntrance.ToWWCP();
                                        //                                    //pool.OpeningTimes                = CurrentEVSEDataRecord.OpeningTimes != null ? OpeningTimes.Parse(CurrentEVSEDataRecord.OpeningTimes) : null;
                                        //                                    pool.AuthenticationModes         = new ReactiveSet<WWCP.AuthenticationModes>(CurrentEVSEDataRecord.AuthenticationModes.SafeSelect(mode   => mode.  ToWWCP()));
                                        //                                    pool.PaymentOptions              = new ReactiveSet<WWCP.PaymentOptions>     (CurrentEVSEDataRecord.PaymentOptions.     SafeSelect(option => option.ToWWCP()));
                                        //                                    pool.Accessibility               = CurrentEVSEDataRecord.Accessibility.ToWWCP();
                                        //                                    pool.HotlinePhoneNumber          = I18NString.Create(Languages.unknown, CurrentEVSEDataRecord.HotlinePhoneNumber.ToString());
                                        //                                    //pool.StatusAggregationDelegate   = ChargingStationStatusAggregationDelegate;

                                        //                                    ChargingPoolsCreated++;

                                        //                                });

                                        //        }

                                        //        #endregion


                                        //        #region Update matching charging station...

                                        //        if (chargingPool.TryGetChargingStationById(EVSEInfo.StationId.ToWWCP().Value, out WWCP.ChargingStation chargingStation))
                                        //        {

                                        //            // Update via events!
                                        //            //_ChargingStation.Name                       = CurrentEVSEDataRecord.ChargingStationName;
                                        //            chargingStation.HubjectStationId           = CurrentEVSEDataRecord.ChargingStationId.ToString();
                                        //            //_ChargingStation.Description                = CurrentEVSEDataRecord.AdditionalInfo;
                                        //            chargingStation.AuthenticationModes        = new ReactiveSet<WWCP.AuthenticationModes>(CurrentEVSEDataRecord.AuthenticationModes.SafeSelect(mode   => mode.  ToWWCP()));
                                        //            chargingStation.PaymentOptions             = new ReactiveSet<WWCP.PaymentOptions>     (CurrentEVSEDataRecord.PaymentOptions.     SafeSelect(option => option.ToWWCP()));
                                        //            chargingStation.Accessibility              = CurrentEVSEDataRecord.Accessibility.ToWWCP();
                                        //            chargingStation.HotlinePhoneNumber         = I18NString.Create(Languages.unknown, CurrentEVSEDataRecord.HotlinePhoneNumber.ToString());
                                        //            chargingStation.IsHubjectCompatible        = CurrentEVSEDataRecord.IsHubjectCompatible;
                                        //            chargingStation.DynamicInfoAvailable       = CurrentEVSEDataRecord.DynamicInfoAvailable == FalseTrueAuto.True;
                                        //            chargingStation.StatusAggregationDelegate  = EVSEStatusAggregationDelegate;

                                        //            chargingStationsUpdated++;

                                        //        }

                                        //        #endregion

                                        //        #region ...or create a new one!

                                        //        else
                                        //            chargingStation = chargingPool.CreateChargingStation(

                                        //                                    EVSEInfo.StationId.ToWWCP().Value,

                                        //                                    Configurator: station => {

                                        //                                        station.DataSource                 = Id.ToString();
                                        //                                        //station.Name                       = CurrentEVSEDataRecord.ChargingStationName;
                                        //                                        station.HubjectStationId           = CurrentEVSEDataRecord.ChargingStationId.ToString();
                                        //                                        //station.Description                = CurrentEVSEDataRecord.AdditionalInfo;
                                        //                                        station.AuthenticationModes        = new ReactiveSet<WWCP.AuthenticationModes>(CurrentEVSEDataRecord.AuthenticationModes.SafeSelect(mode   => mode.  ToWWCP()));
                                        //                                        station.PaymentOptions             = new ReactiveSet<WWCP.PaymentOptions>     (CurrentEVSEDataRecord.PaymentOptions.     SafeSelect(option => option.ToWWCP()));
                                        //                                        station.Accessibility              = CurrentEVSEDataRecord.Accessibility.ToWWCP();
                                        //                                        station.HotlinePhoneNumber         = I18NString.Create(Languages.unknown, CurrentEVSEDataRecord.HotlinePhoneNumber.ToString());
                                        //                                        station.IsHubjectCompatible        = CurrentEVSEDataRecord.IsHubjectCompatible;
                                        //                                        station.DynamicInfoAvailable       = CurrentEVSEDataRecord.DynamicInfoAvailable == FalseTrueAuto.True;
                                        //                                        station.StatusAggregationDelegate  = EVSEStatusAggregationDelegate;

                                        //                                        // photo_uri => "place_photo"

                                        //                                        ChargingStationsCreated++;

                                        //                                    }

                                        //                   );

                                        //        #endregion


                                        //        #region Update matching EVSE...

                                        //        if (chargingStation.TryGetEVSEById(CurrentEVSEDataRecord.Id.ToWWCP().Value, out WWCP.EVSE EVSE))
                                        //        {

                                        //            // Update via events!
                                        //            //_EVSE.Description     = CurrentEVSEDataRecord.AdditionalInfo;
                                        //            //_EVSE.ChargingModes   = new ReactiveSet<WWCP.ChargingModes>(CurrentEVSEDataRecord.ChargingModes.SafeSelect(mode => mode.AsWWCPChargingMode()));
                                        //            //OICPMapper.ApplyChargingFacilities(CurrentEVSEDataRecord.ChargingFacilities, _EVSE);
                                        //            EVSE.MaxCapacity     = CurrentEVSEDataRecord.MaxCapacity.HasValue ? new Decimal?(Convert.ToDecimal(CurrentEVSEDataRecord.MaxCapacity.Value)) : null;
                                        //            //_EVSE.SocketOutlets   = new ReactiveSet<WWCP.SocketOutlet>(CurrentEVSEDataRecord.PlugTypes.SafeSelect(Plug => new WWCP.SocketOutlet(Plug.AsWWCPPlugTypes())));

                                        //            EVSEsUpdated++;

                                        //        }

                                        //        #endregion

                                        //        #region ...or create a new one!

                                        //        else
                                        //            chargingStation.CreateEVSE(CurrentEVSEDataRecord.Id.ToWWCP().Value,

                                        //                                        Configurator: evse => {

                                        //                                            evse.DataSource      = Id.ToString();
                                        //                                            //evse.Description     = CurrentEVSEDataRecord.AdditionalInfo;
                                        //                                            //evse.ChargingModes   = new ReactiveSet<WWCP.ChargingModes>(CurrentEVSEDataRecord.ChargingModes.SafeSelect(mode => mode.AsWWCPChargingMode()));
                                        //                                            //OICPMapper.ApplyChargingFacilities(CurrentEVSEDataRecord.ChargingFacilities, evse);
                                        //                                            evse.MaxCapacity     = CurrentEVSEDataRecord.MaxCapacity.HasValue ? new Decimal?(Convert.ToDecimal(CurrentEVSEDataRecord.MaxCapacity.Value)) : null;
                                        //                                            //evse.SocketOutlets   = new ReactiveSet<WWCP.SocketOutlet>(CurrentEVSEDataRecord.PlugTypes.SafeSelect(Plug => new WWCP.SocketOutlet(Plug.AsWWCPPlugTypes())));

                                        //                                            EVSEsCreated++;

                                        //                                        });

                                        //        #endregion


                                        //    }
                                        //    catch (Exception e)
                                        //    {
                                        //        DebugX.Log(e.Message);
                                        //    }

                                        //}

                                        //DebugX.Log(EVSEsCreated + " EVSE created, " + EVSEsUpdated + " EVSEs updated, " + EVSEsSkipped + " EVSEs skipped");

                                        //TotalEVSEsCreated += EVSEsCreated;
                                        //TotalEVSEsUpdated += EVSEsUpdated;
                                        //TotalEVSEsSkipped += EVSEsSkipped;



                                    #region Illegal charging station operator identification...

                                    //else
                                    //{
                                    //    DebugX.Log("Illegal charging station operator identification: '" + evseDataRecordGroup.Key + "'!");
                                    //    IllegalOperatorsIds++;
                                    //    TotalEVSEsSkipped += (UInt64) evseDataRecordGroup.LongCount();
                                    //}

                                    #endregion

                                #endregion

                            if (pullEVSEDataResponse.Response.LastPage.HasValue && pullEVSEDataResponse.Response.LastPage == false)
                                requestPage++;

                        }

                        #endregion

                        #region ...else

                        else
                        {

                            DebugX.Log("Importing operator infos failed: " +
                                       (pullEVSEDataResponse.Response.StatusCode != null
                                            ? String.Concat(pullEVSEDataResponse.Response.StatusCode.Code, " ", pullEVSEDataResponse.Response.StatusCode.Description,
                                                            pullEVSEDataResponse.Response.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                                                ? pullEVSEDataResponse.Response.StatusCode.AdditionalInfo
                                                                : "")
                                            : ""));

                        }

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


                        if (pullEVSEDataResponse.Response.LastPage == true)
                            finished = true;

                    } while (!finished);

                    var EndTime = Timestamp.Now;
                    DebugX.LogT("[" + Id + "] 'Pull operator infos service' finished after " + (EndTime - StartTime).TotalSeconds + " seconds");

                    #region Send OnPullOperatorInfos event

                    try
                    {

                        if (OnPullOperatorInfos != null)
                            await Task.WhenAll(OnPullOperatorInfos.GetInvocationList().
                                               Cast<OnPullOperatorInfosDelegate>().
                                               Select(e => e(StartTime,
                                                             this,
                                                             nameof(OICPv2_3) + "." + nameof(WWCPCSOAdapter),
                                                             operatorInfos.Values))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnPullOperatorInfos));
                    }

                    #endregion

                    TimestampOfLastPullDataRun = TimestampBeforeLastPullDataRun;

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
                catch (Exception e)
                {

                    while (e.InnerException != null)
                        e = e.InnerException;

                    DebugX.LogException(e, nameof(WWCPCSOAdapter) + ".PullOperatorInfos(State)");

                }
                finally
                {
                    try
                    {
                        PullEVSEDataLock.Release();
                    }
                    catch
                    { }
                }
            }

        }

        #endregion

        #region (timer) PullStatusService(State)

        private void PullStatusService(Object? State)
        {
            if (!PullEVSEStatus_IsDisabled)
                PullStatus(State).Wait();
        }

        private async Task PullStatus(Object? State)
        {

            DebugX.LogT("[" + Id + "] 'Pull status service', as every " + PullEVSEStatus_Every.TotalSeconds + " seconds!");

            if (await PullEVSEDataLock.WaitAsync(SemaphoreSlimTimeout))
            {
                if (await PullEVSEStatusLock.WaitAsync(SemaphoreSlimTimeout))
                {
                    try
                    {

                        Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

                        var StartTime = Timestamp.Now;

                        PullStatus_LastRunTimestamp = StartTime;
                        DebugX.LogT("[" + Id + "] 'Pull status service' started at " + StartTime.ToIso8601());

                        var pullEVSEStatusResult = await EMPRoaming.PullEVSEStatus(
                                                         new PullEVSEStatusRequest(
                                                             DefaultProviderId,
                                                             DefaultSearchCenter.HasValue
                                                                 ? DefaultSearchCenter.Value.ToOICP()
                                                                 : new GeoCoordinates?(),
                                                             DefaultDistanceKM ?? 0,

                                                             CancellationToken:  new CancellationTokenSource().Token,
                                                             EventTrackingId:    EventTracking_Id.New,
                                                             RequestTimeout:     PullEVSEStatus_RequestTimeout)
                                                         );

                        var DownloadTime = Timestamp.Now;

                        #region Everything is ok!

                        if (pullEVSEStatusResult.IsSuccessful &&
                            pullEVSEStatusResult.Response?.OperatorEVSEStatus.SafeAny() == true)
                        {

                            //DebugX.Log("Imported " + OperatorEVSEStatus.Count() + " OperatorEVSEStatus!");
                            DebugX.Log("Imported " + pullEVSEStatusResult.Response.OperatorEVSEStatus.SelectMany(status => status.EVSEStatusRecords).Count() + " EVSEStatusRecords!");

                            var IllegalOperatorsIds  = 0UL;
                            var OperatorsSkipped     = 0UL;
                            var TotalEVSEsUpdated    = 0UL;
                            var TotalEVSEsSkipped    = 0UL;
                            var newStatusList        = new List<WWCP.EVSEStatus>();

                            foreach (var CurrentOperatorEVSEStatus in pullEVSEStatusResult.Response.OperatorEVSEStatus.OrderBy(evseOperator => evseOperator.OperatorName))
                            {

                                DebugX.Log(String.Concat("Importing EVSE operator '", CurrentOperatorEVSEStatus.OperatorName, "' (", CurrentOperatorEVSEStatus.OperatorId, ") with ", CurrentOperatorEVSEStatus.EVSEStatusRecords.Count(), " EVSE status records"));

                                var WWCPChargingStationOperatorId = CurrentOperatorEVSEStatus.OperatorId.ToWWCP();

                                if (WWCPChargingStationOperatorId.HasValue)
                                {

                                    if (!RoamingNetwork.TryGetChargingStationOperatorById(WWCPChargingStationOperatorId, out WWCP.ChargingStationOperator WWCPChargingStationOperator))
                                        WWCPChargingStationOperator = RoamingNetwork.CreateChargingStationOperator(WWCPChargingStationOperatorId.Value,
                                                                                                                   I18NString.Create(Languages.unknown, CurrentOperatorEVSEStatus.OperatorName));

                                    //else
                                        // Update name (via events)!
                                        //WWCPChargingStationOperator.Name = I18NString.Create(Languages.unknown, CurrentOperatorEVSEStatus.OperatorName);

                                    var EVSEsUpdated  = 0UL;
                                    var EVSEsSkipped  = 0UL;

                                    foreach (var currentEVSEDataRecord in CurrentOperatorEVSEStatus.EVSEStatusRecords)
                                    {

                                        var currentEVSEId      = currentEVSEDataRecord.Id.    ToWWCP();
                                        var currentEVSEStatus  = currentEVSEDataRecord.Status.ToWWCP();

                                        if (currentEVSEId.    HasValue &&
                                            currentEVSEStatus.HasValue &&
                                            WWCPChargingStationOperator.TryGetEVSEbyId(currentEVSEId, out var currentEVSE) &&
                                            currentEVSE?.Status.Value != currentEVSEStatus.Value)
                                        {

                                            // Update via events!
                                            currentEVSE.Status = currentEVSEStatus.Value;
                                            newStatusList.Add(new WWCP.EVSEStatus(currentEVSEId.Value, new Timestamped<WWCP.EVSEStatusTypes>(DownloadTime, currentEVSEStatus.Value)));
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
                                    DebugX.Log("Illegal charging station operator identification: '" + CurrentOperatorEVSEStatus.OperatorId + "'!");
                                    IllegalOperatorsIds++;
                                    TotalEVSEsSkipped += (UInt64) CurrentOperatorEVSEStatus.EVSEStatusRecords.LongCount();
                                }

                                #endregion

                            }

                            if (IllegalOperatorsIds > 0)
                                DebugX.Log(OperatorsSkipped  + " illegal EVSE operator identifications");

                            if (OperatorsSkipped    > 0)
                                DebugX.Log(OperatorsSkipped  + " EVSE operators skipped");

                            if (TotalEVSEsUpdated   > 0)
                                DebugX.Log(TotalEVSEsUpdated + " EVSEs updated");

                            if (TotalEVSEsSkipped   > 0)
                                DebugX.Log(TotalEVSEsSkipped + " EVSEs skipped");


                            #region Send OnPullEVSEStatus event

                            try
                            {

                                if (OnPullEVSEStatus is not null)
                                    await Task.WhenAll(OnPullEVSEStatus.GetInvocationList().
                                                       Cast<OnPullEVSEStatusDelegate>().
                                                       Select(e => e(StartTime,
                                                                     this,
                                                                     nameof(OICPv2_3) + "." + nameof(WWCPCSOAdapter),
                                                                     pullEVSEStatusResult.Response.OperatorEVSEStatus.SelectMany(status => status.EVSEStatusRecords).ToArray()))).
                                                       ConfigureAwait(false);

                            }
                            catch (Exception e)
                            {
                                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnPullEVSEStatus));
                            }

                            #endregion

                            #region Send OnEVSEStatusChanges event

                            if (newStatusList.Any())
                            {
                                try
                                {

                                    if (OnEVSEStatusChanges is not null)
                                        await Task.WhenAll(OnEVSEStatusChanges.GetInvocationList().
                                                           Cast<OnEVSEStatusChangesDelegate>().
                                                           Select(e => e(StartTime,
                                                                         this,
                                                                         nameof(OICPv2_3) + "." + nameof(WWCPCSOAdapter),
                                                                         newStatusList))).
                                                           ConfigureAwait(false);

                                }
                                catch (Exception e)
                                {
                                    DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnEVSEStatusChanges));
                                }
                            }

                            #endregion

                        }

                        #endregion

                        #region Something unexpected happend!

                        else
                        {
                            DebugX.Log("Importing EVSE status records failed unexpectedly!");
                        }

                        #endregion


                        var EndTime = Timestamp.Now;

                        DebugX.LogT("[" + Id + "] 'Pull status service' finished after " + (EndTime - StartTime).TotalSeconds + " seconds (" + (DownloadTime - StartTime).TotalSeconds + "/" + (EndTime - DownloadTime).TotalSeconds + ")");

                    }
                    catch (Exception e)
                    {

                        while (e.InnerException != null)
                            e = e.InnerException;

                        DebugX.LogException(e, nameof(WWCPCSOAdapter) + ".PullStatus(State)");

                    }
                    finally
                    {
                        try
                        {
                            PullEVSEStatusLock.Release();
                            PullEVSEDataLock.  Release();
                        }
                        catch
                        { }
                    }

                }
                else
                {
                    try
                    {
                        DebugX.Log(nameof(WWCPCSOAdapter) + ".PullStatus(State) could not acquire PullEVSEStatusLock!");
                        PullEVSEDataLock.Release();
                    }
                    catch
                    { }
                }
            }
            else
                DebugX.Log(nameof(WWCPCSOAdapter) + ".PullStatus(State) could not acquire PullEVSEDataLock!");

        }

        #endregion

        #region (timer) GetChargeDetailRecordsService(State)

        private void GetChargeDetailRecordsService(Object? State)
        {
            if (!GetChargeDetailRecords_IsDisabled)
                _GetChargeDetailRecords(State).Wait();
        }

        private async Task _GetChargeDetailRecords(Object? State)
        {

            DebugX.LogT("[" + Id + "] 'GetChargeDetailRecords service', as every " + GetChargeDetailRecords_Every.TotalSeconds + " seconds!");


            var GetChargeDetailRecordsLockTaken = await GetChargeDetailRecordsLock.WaitAsync(0).ConfigureAwait(false);

            if (GetChargeDetailRecordsLockTaken)
            {

                Thread.CurrentThread.Priority                   = ThreadPriority.BelowNormal;
                var oldGetChargeDetailRecords_LastRunTimestamp  = GetChargeDetailRecords_LastRunTimestamp;
                var StartTime                                   = Timestamp.Now;
                GetChargeDetailRecords_LastRunTimestamp         = StartTime;

                DebugX.LogT("[" + Id + "] 'GetChargeDetailRecords service' started at " + StartTime.ToIso8601());

                try
                {

                    var getChargeDetailRecordsResult = await EMPRoaming.GetChargeDetailRecords(
                                                                 new GetChargeDetailRecordsRequest(
                                                                     ProviderId:         DefaultProviderId,
                                                                     From:               oldGetChargeDetailRecords_LastRunTimestamp,
                                                                     To:                 Timestamp.Now,
                                                                     OperatorIds:        null,
                                                                     CDRForwarded:       null,
                                                                     Page:               0,
                                                                     Size:               2000,
                                                                     SortOrder:          null,

                                                                     CancellationToken:  new CancellationTokenSource().Token,
                                                                     EventTrackingId:    EventTracking_Id.New,
                                                                     RequestTimeout:     GetChargeDetailRecords_RequestTimeout)
                                                                 );

                    var DownloadTime = Timestamp.Now;

                    #region Everything is ok!

                    if (getChargeDetailRecordsResult.IsSuccessful)
                    {

                        var chargeDetailRecords = getChargeDetailRecordsResult.Response.ChargeDetailRecords;


                        #region Send OnGetChargeDetailRecords event

                        try
                        {

                            if (OnGetChargeDetailRecords != null)
                                await Task.WhenAll(OnGetChargeDetailRecords.GetInvocationList().
                                                   Cast<OnGetChargeDetailRecordsDelegate>().
                                                   Select(e => e(StartTime,
                                                                 this,
                                                                 nameof(OICPv2_3) + "." + nameof(WWCPCSOAdapter),
                                                                 chargeDetailRecords.ToArray()))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnGetChargeDetailRecords));
                        }

                        #endregion

                    }

                    #endregion

                    #region Something unexpected happend!

                    else
                    {
                        DebugX.Log("Importing charge detail records failed unexpectedly!");
                    }

                    #endregion


                    var EndTime = Timestamp.Now;

                    DebugX.LogT("[" + Id + "] 'GetChargeDetailRecords service' finished after " + (EndTime - StartTime).TotalSeconds + " seconds (" + (DownloadTime - StartTime).TotalSeconds + "/" + (EndTime - DownloadTime).TotalSeconds + ")");

                }
                catch (Exception e)
                {

                    while (e.InnerException != null)
                        e = e.InnerException;

                    DebugX.LogT(nameof(WWCPCSOAdapter) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                }

                finally
                {
                    GetChargeDetailRecordsLock.Release();
                }

            }

            else
                Console.WriteLine("GetChargeDetailRecords->GetChargeDetailRecordsLock missed!");

            return;

        }

        #endregion

        // -----------------------------------------------------------------------------------------------------


        #region Operator overloading

        #region Operator == (WWCPCSOAdapter1, WWCPCSOAdapter2)

        /// <summary>
        /// Compares two WWCPCSOAdapters for equality.
        /// </summary>
        /// <param name="WWCPCSOAdapter1">A WWCPCSOAdapter.</param>
        /// <param name="WWCPCSOAdapter2">Another WWCPCSOAdapter.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (WWCPCSOAdapter WWCPCSOAdapter1, WWCPCSOAdapter WWCPCSOAdapter2)
        {

            if (Object.ReferenceEquals(WWCPCSOAdapter1, WWCPCSOAdapter2))
                return true;

            if (WWCPCSOAdapter1 is null || WWCPCSOAdapter2 is null)
                return false;

            return WWCPCSOAdapter1.Equals(WWCPCSOAdapter2);

        }

        #endregion

        #region Operator != (WWCPCSOAdapter1, WWCPCSOAdapter2)

        /// <summary>
        /// Compares two WWCPCSOAdapters for inequality.
        /// </summary>
        /// <param name="WWCPCSOAdapter1">A WWCPCSOAdapter.</param>
        /// <param name="WWCPCSOAdapter2">Another WWCPCSOAdapter.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (WWCPCSOAdapter WWCPCSOAdapter1, WWCPCSOAdapter WWCPCSOAdapter2)

            => !(WWCPCSOAdapter1 == WWCPCSOAdapter2);

        #endregion

        #region Operator <  (WWCPCSOAdapter1, WWCPCSOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCSOAdapter1">A WWCPCSOAdapter.</param>
        /// <param name="WWCPCSOAdapter2">Another WWCPCSOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (WWCPCSOAdapter  WWCPCSOAdapter1,
                                          WWCPCSOAdapter  WWCPCSOAdapter2)
        {

            if (WWCPCSOAdapter1 is null)
                throw new ArgumentNullException(nameof(WWCPCSOAdapter1),  "The given WWCPCSOAdapter must not be null!");

            return WWCPCSOAdapter1.CompareTo(WWCPCSOAdapter2) < 0;

        }

        #endregion

        #region Operator <= (WWCPCSOAdapter1, WWCPCSOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCSOAdapter1">A WWCPCSOAdapter.</param>
        /// <param name="WWCPCSOAdapter2">Another WWCPCSOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (WWCPCSOAdapter WWCPCSOAdapter1,
                                           WWCPCSOAdapter WWCPCSOAdapter2)

            => !(WWCPCSOAdapter1 > WWCPCSOAdapter2);

        #endregion

        #region Operator >  (WWCPCSOAdapter1, WWCPCSOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCSOAdapter1">A WWCPCSOAdapter.</param>
        /// <param name="WWCPCSOAdapter2">Another WWCPCSOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (WWCPCSOAdapter WWCPCSOAdapter1,
                                          WWCPCSOAdapter WWCPCSOAdapter2)
        {

            if (WWCPCSOAdapter1 is null)
                throw new ArgumentNullException(nameof(WWCPCSOAdapter1),  "The given WWCPCSOAdapter must not be null!");

            return WWCPCSOAdapter1.CompareTo(WWCPCSOAdapter2) > 0;

        }

        #endregion

        #region Operator >= (WWCPCSOAdapter1, WWCPCSOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCSOAdapter1">A WWCPCSOAdapter.</param>
        /// <param name="WWCPCSOAdapter2">Another WWCPCSOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (WWCPCSOAdapter WWCPCSOAdapter1,
                                           WWCPCSOAdapter WWCPCSOAdapter2)

            => !(WWCPCSOAdapter1 < WWCPCSOAdapter2);

        #endregion

        #endregion

        #region IComparable<WWCPCSOAdapter> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object is WWCPCSOAdapter WWCPCSOAdapter)
                return CompareTo(WWCPCSOAdapter);

            throw new ArgumentException("The given object is not an WWCPCSOAdapter!", nameof(Object));

        }

        #endregion

        #region CompareTo(WWCPCSOAdapter)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCSOAdapter">An WWCPCSOAdapter object to compare with.</param>
        public Int32 CompareTo(WWCPCSOAdapter WWCPCSOAdapter)
        {

            if (WWCPCSOAdapter is null)
                throw new ArgumentNullException(nameof(WWCPCSOAdapter), "The given WWCPCSOAdapter must not be null!");

            return Id.CompareTo(WWCPCSOAdapter.Id);

        }

        #endregion

        #endregion

        #region IEquatable<WWCPCSOAdapter> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is WWCPCSOAdapter WWCPCSOAdapter &&
                   Equals(WWCPCSOAdapter);

        #endregion

        #region Equals(WWCPCSOAdapter)

        /// <summary>
        /// Compares two WWCPCSOAdapter for equality.
        /// </summary>
        /// <param name="WWCPCSOAdapter">An WWCPCSOAdapter to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(WWCPCSOAdapter WWCPCSOAdapter)

            => WWCPCSOAdapter is null
                   ? false
                   : Id.Equals(WWCPCSOAdapter.Id);

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
