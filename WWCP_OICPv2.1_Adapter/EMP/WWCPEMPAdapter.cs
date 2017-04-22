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
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Text;

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


        private readonly        Object                                                  PullDataServiceLock;
        private readonly        Timer                                                   PullDataServiceTimer;

        /// <summary>
        /// The default status check intervall.
        /// </summary>
        public  readonly static TimeSpan                                                DefaultPullDataServiceEvery              = TimeSpan.FromMinutes(15);

        public  readonly static TimeSpan                                                DefaultPullDataServiceRequestTimeout     = TimeSpan.FromMinutes(30);


        private readonly        Object                                                  PullStatusServiceLock;
        private readonly        Timer                                                   PullStatusServiceTimer;

        /// <summary>
        /// The default status check intervall.
        /// </summary>
        public  readonly static TimeSpan                                                DefaultPullStatusServiceEvery            = TimeSpan.FromMinutes(1);

        public  readonly static TimeSpan                                                DefaultPullStatusServiceRequestTimeout   = TimeSpan.FromMinutes(3);

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

        public DateTime? LastPullDataRun { get; private set; }

        #endregion

        #region PullStatusService

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

        #region Custom request mappers

        #region CustomPullEVSEData(SOAP)RequestMapper

        #region CustomPullEVSEDataRequestMapper

        public Func<PullEVSEDataRequest, PullEVSEDataRequest> CustomPullEVSEDataRequestMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEDataRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEDataRequestMapper = value;
            }

        }

        #endregion

        #region CustomPullEVSEDataSOAPRequestMapper

        public Func<PullEVSEDataRequest, XElement, XElement> CustomPullEVSEDataSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEDataSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEDataSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<PullEVSEDataRequest>, Acknowledgement<PullEVSEDataRequest>.Builder> CustomPullEVSEDataResponseMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEDataResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEDataResponseMapper = value;
            }

        }

        #endregion

        #region CustomPullEVSEStatus(SOAP)RequestMapper

        #region CustomPullEVSEStatusRequestMapper

        public Func<PullEVSEStatusRequest, PullEVSEStatusRequest> CustomPullEVSEStatusRequestMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomPullEVSEStatusSOAPRequestMapper

        public Func<PullEVSEStatusRequest, XElement, XElement> CustomPullEVSEStatusSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<PullEVSEStatusRequest>, Acknowledgement<PullEVSEStatusRequest>.Builder> CustomPullEVSEStatusResponseMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEStatusResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEStatusResponseMapper = value;
            }

        }

        #endregion

        #region CustomPullEVSEStatusById(SOAP)RequestMapper

        #region CustomPullEVSEStatusByIdRequestMapper

        public Func<PullEVSEStatusByIdRequest, PullEVSEStatusByIdRequest> CustomPullEVSEStatusByIdRequestMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEStatusByIdRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEStatusByIdRequestMapper = value;
            }

        }

        #endregion

        #region CustomPullEVSEStatusByIdSOAPRequestMapper

        public Func<PullEVSEStatusByIdRequest, XElement, XElement> CustomPullEVSEStatusByIdSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEStatusByIdSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEStatusByIdSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<PullEVSEStatusByIdRequest>, Acknowledgement<PullEVSEStatusByIdRequest>.Builder> CustomPullEVSEStatusByIdResponseMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEStatusByIdResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEStatusByIdResponseMapper = value;
            }

        }

        #endregion


        #region CustomPushAuthenticationData(SOAP)RequestMapper

        #region CustomPushAuthenticationDataRequestMapper

        public Func<PushAuthenticationDataRequest, PushAuthenticationDataRequest> CustomPushAuthenticationDataRequestMapper
        {

            get
            {
                return EMPClient.CustomPushAuthenticationDataRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPushAuthenticationDataRequestMapper = value;
            }

        }

        #endregion

        #region CustomPushAuthenticationDataSOAPRequestMapper

        public Func<PushAuthenticationDataRequest, XElement, XElement> CustomPushAuthenticationDataSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomPushAuthenticationDataSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPushAuthenticationDataSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<PushAuthenticationDataRequest>, Acknowledgement<PushAuthenticationDataRequest>.Builder> CustomPushAuthenticationDataResponseMapper
        {

            get
            {
                return EMPClient.CustomPushAuthenticationDataResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPushAuthenticationDataResponseMapper = value;
            }

        }

        #endregion


        #region CustomAuthorizeRemoteReservationStart(SOAP)RequestMapper

        #region CustomAuthorizeRemoteReservationStartRequestMapper

        public Func<AuthorizeRemoteReservationStartRequest, AuthorizeRemoteReservationStartRequest> CustomAuthorizeRemoteReservationStartRequestMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteReservationStartRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteReservationStartRequestMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeRemoteReservationStartSOAPRequestMapper

        public Func<AuthorizeRemoteReservationStartRequest, XElement, XElement> CustomAuthorizeRemoteReservationStartSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteReservationStartSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteReservationStartSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<AuthorizeRemoteReservationStartRequest>, Acknowledgement<AuthorizeRemoteReservationStartRequest>.Builder> CustomAuthorizeRemoteReservationStartResponseMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteReservationStartResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteReservationStartResponseMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeRemoteReservationStop(SOAP)RequestMapper

        #region CustomAuthorizeRemoteReservationStopRequestMapper

        public Func<AuthorizeRemoteReservationStopRequest, AuthorizeRemoteReservationStopRequest> CustomAuthorizeRemoteReservationStopRequestMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteReservationStopRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteReservationStopRequestMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeRemoteReservationStopSOAPRequestMapper

        public Func<AuthorizeRemoteReservationStopRequest, XElement, XElement> CustomAuthorizeRemoteReservationStopSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteReservationStopSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteReservationStopSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<AuthorizeRemoteReservationStopRequest>, Acknowledgement<AuthorizeRemoteReservationStopRequest>.Builder> CustomAuthorizeRemoteReservationStopResponseMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteReservationStopResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteReservationStopResponseMapper = value;
            }

        }

        #endregion


        #region CustomAuthorizeRemoteStart(SOAP)RequestMapper

        #region CustomAuthorizeRemoteStartRequestMapper

        public Func<AuthorizeRemoteStartRequest, AuthorizeRemoteStartRequest> CustomAuthorizeRemoteStartRequestMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteStartRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteStartRequestMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeRemoteStartSOAPRequestMapper

        public Func<AuthorizeRemoteStartRequest, XElement, XElement> CustomAuthorizeRemoteStartSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteStartSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteStartSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<AuthorizeRemoteStartRequest>, Acknowledgement<AuthorizeRemoteStartRequest>.Builder> CustomAuthorizeRemoteStartResponseMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteStartResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteStartResponseMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeRemoteStop(SOAP)Mappers

        #region CustomAuthorizeRemoteStopRequestMapper

        public Func<AuthorizeRemoteStopRequest, AuthorizeRemoteStopRequest> CustomAuthorizeRemoteStopRequestMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteStopRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteStopRequestMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeRemoteStopSOAPRequestMapper

        public Func<AuthorizeRemoteStopRequest, XElement, XElement> CustomAuthorizeRemoteStopSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteStopSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteStopSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<AuthorizeRemoteStopRequest>, Acknowledgement<AuthorizeRemoteStopRequest>.Builder> CustomAuthorizeRemoteStopResponseMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteStopResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteStopResponseMapper = value;
            }

        }

        #endregion


        #region CustomGetChargeDetailRecords(SOAP)RequestMapper

        #region CustomGetChargeDetailRecordsRequestMapper

        public Func<GetChargeDetailRecordsRequest, GetChargeDetailRecordsRequest> CustomGetChargeDetailRecordsRequestMapper
        {

            get
            {
                return EMPClient.CustomGetChargeDetailRecordsRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomGetChargeDetailRecordsRequestMapper = value;
            }

        }

        #endregion

        #region CustomGetChargeDetailRecordsSOAPRequestMapper

        public Func<XElement, XElement> CustomGetChargeDetailRecordsSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomGetChargeDetailRecordsSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomGetChargeDetailRecordsSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<GetChargeDetailRecordsResponse, GetChargeDetailRecordsResponse.Builder> CustomGetChargeDetailRecordsResponseMapper
        {

            get
            {
                return EMPClient.CustomGetChargeDetailRecordsResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomGetChargeDetailRecordsResponseMapper = value;
            }

        }


        public CustomMapperDelegate<ChargeDetailRecord> CustomChargeDetailRecordXMLMapper
        {

            get
            {
                return EMPClient.CustomChargeDetailRecordXMLMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomChargeDetailRecordXMLMapper = value;
            }

        }


        #endregion

        #endregion

        #region Events

        // Client methods (logging)

        #region OnPullEVSEDataRequest/-Response

        /// <summary>
        /// An event sent whenever a 'pull EVSE data' request will be send.
        /// </summary>
        public event OnPullEVSEDataRequestHandler         OnPullEVSEDataRequest;

        /// <summary>
        /// An event sent whenever a response for a 'pull EVSE data' request had been received.
        /// </summary>
        public event OnPullEVSEDataResponseHandler        OnPullEVSEDataResponse;

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


        // EMPServer methods

        #region OnAuthorizeStart

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event RequestLogHandler OnLogAuthorizeStart
        {

            add
            {
                EMPRoaming.OnAuthorizeStartSOAPRequest += value;
            }

            remove
            {
                EMPRoaming.OnAuthorizeStartSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize stop response was sent.
        /// </summary>
        public event AccessLogHandler OnLogAuthorizeStarted
        {

            add
            {
                EMPRoaming.OnAuthorizeStartSOAPResponse += value;
            }

            remove
            {
                EMPRoaming.OnAuthorizeStartSOAPResponse -= value;
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
                EMPRoaming.OnAuthorizeStopSOAPRequest += value;
            }

            remove
            {
                EMPRoaming.OnAuthorizeStopSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize stop response was sent.
        /// </summary>
        public event AccessLogHandler OnLogAuthorizeStopped
        {

            add
            {
                EMPRoaming.OnAuthorizeStopSOAPResponse += value;
            }

            remove
            {
                EMPRoaming.OnAuthorizeStopSOAPResponse -= value;
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
                EMPRoaming.OnChargeDetailRecordSOAPRequest += value;
            }

            remove
            {
                EMPRoaming.OnChargeDetailRecordSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a charge detail record response was sent.
        /// </summary>
        public event AccessLogHandler OnLogChargeDetailRecordSent
        {

            add
            {
                EMPRoaming.OnChargeDetailRecordSOAPResponse += value;
            }

            remove
            {
                EMPRoaming.OnChargeDetailRecordSOAPResponse -= value;
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
                              EVSEDataRecord2EVSEDelegate  EVSEDataRecord2EVSE               = null,

                              EVSEOperatorFilterDelegate   EVSEOperatorFilter                = null,

                              TimeSpan?                    PullDataServiceEvery              = null,
                              Boolean                      DisablePullData                   = false,
                              TimeSpan?                    PullDataServiceRequestTimeout     = null,

                              TimeSpan?                    PullStatusServiceEvery            = null,
                              Boolean                      DisablePullStatus                 = false,
                              TimeSpan?                    PullStatusServiceRequestTimeout   = null,

                              IRemoteEMobilityProvider     DefaultProvider                   = null,
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

            this.Name                               = Name;
            this.EMPRoaming                         = EMPRoaming;
            this._EVSEDataRecord2EVSE               = EVSEDataRecord2EVSE;

            this.EVSEOperatorFilter                 = EVSEOperatorFilter != null ? EVSEOperatorFilter : (name, id) => true;

            this._PullDataServiceEvery              = (UInt32) (PullDataServiceEvery.HasValue
                                                                    ? PullDataServiceEvery.Value.  TotalMilliseconds
                                                                    : DefaultPullDataServiceEvery. TotalMilliseconds);
            this.PullDataServiceRequestTimeout      = PullDataServiceRequestTimeout.HasValue ? PullDataServiceRequestTimeout.Value : DefaultPullDataServiceRequestTimeout;
            this.PullDataServiceLock                = new Object();
            this.PullDataServiceTimer               = new Timer(PullDataService, null, 5000, _PullDataServiceEvery);
            this.DisablePullData                    = DisablePullData;


            this._PullStatusServiceEvery            = (UInt32) (PullStatusServiceEvery.HasValue
                                                                    ? PullStatusServiceEvery.Value.  TotalMilliseconds
                                                                    : DefaultPullStatusServiceEvery. TotalMilliseconds);
            this.PullStatusServiceRequestTimeout    = PullStatusServiceRequestTimeout.HasValue ? PullStatusServiceRequestTimeout.Value : DefaultPullStatusServiceRequestTimeout;
            this.PullStatusServiceLock              = new Object();
            this.PullStatusServiceTimer             = new Timer(PullStatusService, null, 150000, _PullStatusServiceEvery);
            this.DisablePullStatus                  = DisablePullStatus;

            this.DefaultProviderId                  = DefaultProvider != null
                                                          ? new Provider_Id?(DefaultProvider.Id.ToOICP())
                                                          : null;
            this.DefaultSearchCenter                = DefaultSearchCenter;
            this.DefaultDistanceKM                  = DefaultDistanceKM;


            // Link events...

            #region OnAuthorizeStart

            this.EMPRoaming.OnAuthorizeStart += async (Timestamp,
                                                       Sender,
                                                       Request) => {


                var response = await RoamingNetwork.AuthorizeStart(Request.UID.             ToWWCP(),
                                                                   Request.EVSEId.Value.    ToWWCP().Value,
                                                                   Request.PartnerProductId.HasValue
                                                                       ? new ChargingProduct(Request.PartnerProductId.Value.ToWWCP())
                                                                       : null,
                                                                   Request.SessionId.       ToWWCP(),
                                                                   Request.OperatorId.      ToWWCP(),

                                                                   Timestamp,
                                                                   Request.CancellationToken,
                                                                   Request.EventTrackingId,
                                                                   Request.RequestTimeout).
                                                    ConfigureAwait(false);

                if (response != null)
                {
                    switch (response.Result)
                    {

                        case AuthStartEVSEResultType.Authorized:
                            return CPO.AuthorizationStart.Authorized(Request,
                                                                     response.SessionId. HasValue ? response.SessionId. Value.ToOICP() : default(Session_Id?),
                                                                     default(PartnerSession_Id?),
                                                                     response.ProviderId.HasValue ? response.ProviderId.Value.ToOICP() : default(Provider_Id?),
                                                                     "Ready to charge!",
                                                                     null,
                                                                     response.ListOfAuthStopTokens.
                                                                         SafeSelect(token => AuthorizationIdentification.FromRFIDId(token.ToOICP()))
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

                return CPO.AuthorizationStart.ServiceNotAvailable(
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


                var response = await RoamingNetwork.AuthorizeStop(Request.SessionId. ToWWCP(),
                                                                  Request.UID.       ToWWCP(),
                                                                  Request.EVSEId.    ToWWCP().Value,
                                                                  Request.OperatorId.ToWWCP(),

                                                                  Request.Timestamp,
                                                                  Request.CancellationToken,
                                                                  Request.EventTrackingId,
                                                                  Request.RequestTimeout).
                                                    ConfigureAwait(false);

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

                return CPO.AuthorizationStop.ServiceNotAvailable(
                           Request,
                           SessionId:  response?.SessionId. ToOICP() ?? Request.SessionId,
                           ProviderId: response?.ProviderId.ToOICP()
                       );

            };

            #endregion

            #region OnChargeDetailRecord

            this.EMPRoaming.OnChargeDetailRecord += async (Timestamp,
                                                           Sender,
                                                           CancellationToken,
                                                           EventTrackingId,
                                                           ChargeDetailRecord,
                                                           RequestTimeout) => {


                var response = await RoamingNetwork.SendChargeDetailRecords(new WWCP.ChargeDetailRecord[] { ChargeDetailRecord.ToWWCP() },

                                                                            Timestamp,
                                                                            CancellationToken,
                                                                            EventTrackingId,
                                                                            RequestTimeout).
                                                    ConfigureAwait(false);

                if (response != null)
                {
                    switch (response.Status)
                    {

                        case SendCDRsResultType.Forwarded:
                            return Acknowledgement.Success(
                                       ChargeDetailRecord.SessionId,
                                       ChargeDetailRecord.PartnerSessionId,
                                       "Charge detail record forwarded!"
                                   );

                        case SendCDRsResultType.NotForwared:
                            return Acknowledgement.SystemError(
                                       "Communication to EVSE failed!",
                                       SessionId:         ChargeDetailRecord.SessionId,
                                       PartnerSessionId:  ChargeDetailRecord.PartnerSessionId
                                   );

                        case SendCDRsResultType.InvalidSessionId:
                            return Acknowledgement.SessionIsInvalid(
                                       SessionId:         ChargeDetailRecord.SessionId,
                                       PartnerSessionId:  ChargeDetailRecord.PartnerSessionId
                                   );

                        case SendCDRsResultType.UnknownEVSE:
                            return Acknowledgement.UnknownEVSEID(
                                       SessionId:         ChargeDetailRecord.SessionId,
                                       PartnerSessionId:  ChargeDetailRecord.PartnerSessionId
                                   );

                        case SendCDRsResultType.Error:
                            return Acknowledgement.DataError(
                                       SessionId:         ChargeDetailRecord.SessionId,
                                       PartnerSessionId:  ChargeDetailRecord.PartnerSessionId
                                   );

                    }
                }

                return Acknowledgement.ServiceNotAvailable(
                           SessionId: ChargeDetailRecord.SessionId
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

                              IRemoteEMobilityProvider     DefaultProvider                   = null,
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
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
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
                              String                               EVSEDataURI                     = EMPClient.DefaultEVSEDataURI,
                              String                               EVSEStatusURI                   = EMPClient.DefaultEVSEStatusURI,
                              String                               AuthenticationDataURI           = EMPClient.DefaultAuthenticationDataURI,
                              String                               ReservationURI                  = EMPClient.DefaultReservationURI,
                              String                               AuthorizationURI                = EMPClient.DefaultAuthorizationURI,
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
                              LogfileCreatorDelegate               LogfileCreator                  = null,

                              EVSEDataRecord2EVSEDelegate          EVSEDataRecord2EVSE             = null,

                              EVSEOperatorFilterDelegate           EVSEOperatorFilter                = null,

                              TimeSpan?                            PullDataServiceEvery              = null,
                              Boolean                              DisablePullData                   = false,
                              TimeSpan?                            PullDataServiceRequestTimeout     = null,

                              TimeSpan?                            PullStatusServiceEvery            = null,
                              Boolean                              DisablePullStatus                 = false,
                              TimeSpan?                            PullStatusServiceRequestTimeout   = null,

                              IRemoteEMobilityProvider             DefaultProvider                   = null,
                              GeoCoordinate?                       DefaultSearchCenter               = null,
                              UInt64?                              DefaultDistanceKM                 = null,

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
                                  EVSEDataURI,
                                  EVSEStatusURI,
                                  AuthenticationDataURI,
                                  ReservationURI,
                                  AuthorizationURI,
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
                                  LogfileCreator,

                                  DNSClient),

                   EVSEDataRecord2EVSE,

                   DefaultProvider: DefaultProvider)

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
                         GeoCoordinate?         SearchCenter       = null,
                         Single                 DistanceKM         = 0f,
                         DateTime?              LastCall           = null,
                         eMobilityProvider_Id?  ProviderId         = null,

                         DateTime?              Timestamp          = null,
                         CancellationToken?     CancellationToken  = null,
                         EventTracking_Id       EventTrackingId    = null,
                         TimeSpan?              RequestTimeout     = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;

            #endregion

            #region Send OnPullEVSEDataRequest event

            //var StartTime = DateTime.Now;

            //try
            //{

            //    OnPullEVSEDataRequest?.Invoke(StartTime,
            //                                  Timestamp.Value,
            //                                  this,
            //                                  EventTrackingId,
            //                                  ProviderId.HasValue
            //                                               ? ProviderId.Value.ToOICP()
            //                                               : DefaultProviderId.Value,
            //                                  SearchCenter,
            //                                  DistanceKM,
            //                                  LastCall,
            //                                  RequestTimeout);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnRemoteStartEVSERequest));
            //}

            #endregion


            var result = await EMPRoaming.PullEVSEData(ProviderId.HasValue
                                                           ? ProviderId.Value.ToOICP()
                                                           : DefaultProviderId.Value,
                                                       SearchCenter,
                                                       DistanceKM,
                                                       LastCall,
                                                       GeoCoordinatesResponseFormats.DecimalDegree,

                                                       Timestamp,
                                                       CancellationToken,
                                                       EventTrackingId,
                                                       RequestTimeout).
                                          ConfigureAwait(false);


            if (result.HTTPStatusCode == HTTPStatusCode.OK &&
                result.Content        != null)
            {

                #region Data

                ChargingStationOperator  _EVSEOperator                  = null;
                CPInfoList               _CPInfoList                    = null;
                EVSEIdLookup             _EVSEIdLookup                  = null;
                EVSEInfo                 _EVSEInfo                      = null;
                Languages                LocationLanguage;
                Languages                LocalChargingStationLanguage;
                I18NString               AdditionalInfo                 = null;
                ChargingPool             _ChargingPool                  = null;
                ChargingStation          _ChargingStation               = null;
                EVSE                     _EVSE                          = null;

                #endregion

                result.Content.OperatorEVSEData.ForEach(operatorevsedata => {

                    try
                    {

                        #region Find Charging Station Operator, or create a new one...

                        if (!RoamingNetwork.TryGetChargingStationOperatorById(operatorevsedata.OperatorId.ToWWCP().Value, out _EVSEOperator))
                            _EVSEOperator = RoamingNetwork.CreateChargingStationOperator(operatorevsedata.OperatorId.ToWWCP().Value, I18NString.Create(Languages.unknown, operatorevsedata.OperatorName));

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

                                _CPInfoList.AddOrUpdateCPInfo(ChargingPool_Id.Generate(operatorevsedata.OperatorId.ToWWCP().Value,
                                                                                       evsedatarecord.  Address.ToWWCP(),
                                                                                       evsedatarecord.  GeoCoordinate),
                                                              evsedatarecord.Address,
                                                              evsedatarecord.GeoCoordinate,
                                                              evsedatarecord.ChargingStationId,
                                                              evsedatarecord.Id);

                                _EVSEIdLookup = _CPInfoList.VerifyUniquenessOfChargingStationIds();

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

                                    case "de": LocationLanguage = Languages.deu; break;
                                    case "fr": LocationLanguage = Languages.fra; break;
                                    case "dk": LocationLanguage = Languages.dk; break;
                                    case "no": LocationLanguage = Languages.no; break;
                                    case "fi": LocationLanguage = Languages.fin; break;
                                    case "se": LocationLanguage = Languages.swe; break;

                                    case "sk": LocationLanguage = Languages.sk; break;
                                    //case "be": LocationLanguage = Languages.; break;
                                    case "us": LocationLanguage = Languages.eng; break;
                                    case "nl": LocationLanguage = Languages.nld; break;
                                    //case "fo": LocationLanguage = Languages.; break;
                                    case "at": LocationLanguage = Languages.deu; break;
                                    case "ru": LocationLanguage = Languages.ru; break;
                                    //case "ch": LocationLanguage = Languages.; break;

                                    default: LocationLanguage = Languages.unknown; break;

                                }

                                if (_EVSEInfo.PoolAddress.Country == Country.Germany)
                                    LocalChargingStationLanguage = Languages.deu;

                                else if (_EVSEInfo.PoolAddress.Country == Country.Denmark)
                                    LocalChargingStationLanguage = Languages.dk;

                                else if (_EVSEInfo.PoolAddress.Country == Country.France)
                                    LocalChargingStationLanguage = Languages.fra;

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

                                    _ChargingPool = _EVSEOperator.CreateChargingPool(

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
                                    _ChargingStation = _ChargingPool.CreateChargingStation(

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

                                if (_ChargingStation.TryGetEVSEbyId(evsedatarecord.Id.ToWWCP().Value, out _EVSE))
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
                                    _ChargingStation.CreateEVSE(evsedatarecord.Id.ToWWCP().Value,

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

            PullEVSEStatus(GeoCoordinate?         SearchCenter        = null,
                           Single                 DistanceKM          = 0f,
                           EVSEStatusTypes?       EVSEStatusFilter    = null,
                           eMobilityProvider_Id?  ProviderId          = null,

                           DateTime?              Timestamp           = null,
                           CancellationToken?     CancellationToken   = null,
                           EventTracking_Id       EventTrackingId     = null,
                           TimeSpan?              RequestTimeout      = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;

            #endregion

            var result = await EMPRoaming.PullEVSEStatus(ProviderId.HasValue
                                                             ? ProviderId.Value.ToOICP()
                                                             : DefaultProviderId.Value,
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
                                      SafeSelect(evsestatusrecord   => new WWCP.EVSEStatus(evsestatusrecord.Id.ToWWCP().Value,
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

            PullEVSEStatusById(IEnumerable<WWCP.EVSE_Id>  EVSEIds,
                               eMobilityProvider_Id?      ProviderId          = null,

                               DateTime?                  Timestamp           = null,
                               CancellationToken?         CancellationToken   = null,
                               EventTracking_Id           EventTrackingId     = null,
                               TimeSpan?                  RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEIds.IsNullOrEmpty())
                return new WWCP.EVSEStatus[0];

            #endregion

            var _EVSEStatus = new List<WWCP.EVSEStatus>();

            // Hubject has a limit of 100 EVSEIds per request!
            // Do not make concurrent requests!
            foreach (var evsepart in EVSEIds.Select(evse => evse.ToOICP()).ToPartitions(100))
            {

                var result = await EMPRoaming.PullEVSEStatusById(ProviderId.HasValue
                                                                     ? ProviderId.Value.ToOICP()
                                                                     : DefaultProviderId.Value,
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
                                                        SafeSelect(evsestatusrecord => new WWCP.EVSEStatus(evsestatusrecord.Id.ToWWCP().Value,
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

            if (AuthorizationIdentifications.IsNullOrEmpty())
                return new WWCP.Acknowledgement(ResultType.NoOperation);


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
                                            DateTime?                         ReservationStartTime,  // = null,
                                            TimeSpan?                         Duration,              // = null,
                                            ChargingReservation_Id?           ReservationId,         // = null,
                                            eMobilityProvider_Id?             ProviderId,            // = null,
                                            eMobilityAccount_Id?              eMAId,                 // = null,
                                            ChargingProduct                   ChargingProduct,       // = null,
                                            IEnumerable<Auth_Token>           AuthTokens,            // = null,
                                            IEnumerable<eMobilityAccount_Id>  eMAIds,                // = null,
                                            IEnumerable<UInt32>               PINs,                  // = null,

                                            DateTime?                         Timestamp,
                                            CancellationToken?                CancellationToken,
                                            EventTracking_Id                  EventTrackingId,
                                            TimeSpan?                         RequestTimeout)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;

            #endregion

            #region Send OnReserveEVSERequest event

            var StartTime = DateTime.Now;

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
                                                           ProviderId:         ProviderId.HasValue
                                                                                   ? ProviderId.Value.ToOICP()
                                                                                   : DefaultProviderId.Value,
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
                                                     ? new ChargingReservation(ReservationId:            ChargingReservation_Id.Parse(EVSEId.OperatorId.ToString() + "*R" + result.Content.SessionId.ToString()),
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
                                                                               ChargingProduct:          ChargingProduct,
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

            if (!EVSEId.HasValue)
                throw new ArgumentNullException(nameof(EVSEId),  "The EVSE identification is mandatory in OICP!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;

            #endregion

            #region Send OnCancelReservationRequest event

            var StartTime = DateTime.Now;

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
                                                          EVSEId:             EVSEId.Value.ToOICP(),
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
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;

            #endregion

            #region Send OnRemoteStartEVSERequest event

            var StartTime = DateTime.Now;

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


            var result = await EMPRoaming.RemoteStart(ProviderId:         ProviderId.HasValue
                                                                              ? ProviderId.Value.ToOICP()
                                                                              : DefaultProviderId.Value,
                                                      EVSEId:             EVSEId.ToOICP(),
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

            IReserveRemoteStartStop.RemoteStart(ChargingStation_Id       ChargingStationId,
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
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = EMPClient?.RequestTimeout;

            #endregion

            #region Send OnRemoteStopEVSERequest event

            var StartTime = DateTime.Now;

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
                                                     ProviderId:         ProviderId.HasValue
                                                                              ? ProviderId.Value.ToOICP()
                                                                              : DefaultProviderId.Value,
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

            IReserveRemoteStartStop.RemoteStop(ChargingStation_Id     ChargingStationId,
                                               ChargingSession_Id     SessionId,
                                               ReservationHandling?   ReservationHandling,
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
                To = DateTime.Now;

            #endregion

            var result = await EMPRoaming.GetChargeDetailRecords(ProviderId.HasValue
                                                                     ? ProviderId.Value.ToOICP()
                                                                     : DefaultProviderId.Value,
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

                return result.Content.ChargeDetailRecords.
                                      SafeSelect(cdr => {

                                                            try
                                                            {

                                                                return OICPMapper.ToWWCP(cdr);

                                                            }
                                                            catch (Exception e)
                                                            {
                                                                //ToDo: Add exceptions to information list!
                                                                return null;
                                                            }

                                                        }).
                                      SafeWhere(cdr => cdr != null);

            }

            return new WWCP.ChargeDetailRecord[0];

        }

        #endregion


        // -----------------------------------------------------------------------------------------------------

        #region (timer) PullDataService(State)

        private void PullDataService(Object State)
        {

            if (!DisablePullData)
            {

                PullData().Wait();

                //ToDo: Handle errors!

            }

        }

        public async Task PullData()
        {

            DebugX.LogT("[" + Id + "] 'Pull data service', as every " + _PullStatusServiceEvery + "ms!");

            if (Monitor.TryEnter(PullDataServiceLock,
                                 TimeSpan.FromSeconds(30)))
            {

                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

                var StartTime = DateTime.Now;
                DebugX.LogT("[" + Id + "] 'Pull data service' started at " + StartTime.ToIso8601());

                try
                {
                    var TimestampBeforeLastPullDataRun = DateTime.Now;

                    //var PullEVSEDataTask  = EMPRoaming.PullEVSEData(DefaultProviderId.Value,
                    //                                                DefaultSearchCenter,
                    //                                                DefaultDistanceKM.HasValue ? DefaultDistanceKM.Value : 0,
                    //                                                LastPullDataRun,
                    //
                    //                                                CancellationToken:  new CancellationTokenSource().Token,
                    //                                                EventTrackingId:    EventTracking_Id.New,
                    //                                                RequestTimeout:     PullDataServiceRequestTimeout);
                    //
                    //PullEVSEDataTask.Wait();

                    var DownloadTime = DateTime.Now;

                    LastPullDataRun = TimestampBeforeLastPullDataRun;

                    #region Everything is ok!

                    //if (PullEVSEDataTask.Result                    != null   &&
                    //    PullEVSEDataTask.Result.Content            != null   &&
                    //    PullEVSEDataTask.Result.Content.StatusCode != null   &&
                    //    PullEVSEDataTask.Result.Content.StatusCode.HasResult &&
                    //    PullEVSEDataTask.Result.Content.StatusCode.Code == StatusCodes.Success)
                    //{
                    //
                    //    var OperatorEVSEData = PullEVSEDataTask.Result.Content.OperatorEVSEData;

                        var SOAPXML = XDocument.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "PullEVSEData_TestData001.xml", Encoding.UTF8)).
                                                      Root.
                                                      Element(Vanaheimr.Hermod.SOAP.NS.SOAPEnvelope_v1_1 + "Body").
                                                      Descendants().
                                                      FirstOrDefault();

                        var OperatorEVSEData = EVSEData.Parse(SOAPXML).OperatorEVSEData;

                        if (OperatorEVSEData != null && OperatorEVSEData.Any())
                        {

                            DebugX.Log("Imported " + OperatorEVSEData.Count() + " OperatorEVSEData!");
                            DebugX.Log("Imported " + OperatorEVSEData.SelectMany(status => status.EVSEDataRecords).Count() + " EVSEDataRecords!");

                            ChargingStationOperator      WWCPChargingStationOperator     = null;
                            ChargingStationOperator_Id?  WWCPChargingStationOperatorId   = null;
                            UInt64                       IllegalOperatorsIds             = 0;
                            UInt64                       OperatorsSkipped                = 0;
                            UInt64                       TotalEVSEsCreated               = 0;
                            UInt64                       TotalEVSEsUpdated               = 0;
                            UInt64                       TotalEVSEsSkipped               = 0;

                            CPInfoList                   _CPInfoList;

                            foreach (var CurrentOperatorEVSEData in OperatorEVSEData.OrderBy(evseoperator => evseoperator.OperatorName))
                            {

                                if (EVSEOperatorFilter(CurrentOperatorEVSEData.OperatorName,
                                                       CurrentOperatorEVSEData.OperatorId))
                                {

                                    DebugX.Log("Importing EVSE operator " + CurrentOperatorEVSEData.OperatorName + " (" + CurrentOperatorEVSEData.OperatorId.ToString() + ") with " + CurrentOperatorEVSEData.EVSEDataRecords.Count() + " EVSE data records");

                                    WWCPChargingStationOperatorId = CurrentOperatorEVSEData.OperatorId.ToWWCP();

                                    if (WWCPChargingStationOperatorId.HasValue)
                                    {

                                        if (!RoamingNetwork.TryGetChargingStationOperatorById(WWCPChargingStationOperatorId, out WWCPChargingStationOperator))
                                            WWCPChargingStationOperator = RoamingNetwork.CreateChargingStationOperator(WWCPChargingStationOperatorId.Value,
                                                                                                                       I18NString.Create(Languages.unknown, CurrentOperatorEVSEData.OperatorName));

                                        else
                                            // Update name (via events)!
                                            WWCPChargingStationOperator.Name = I18NString.Create(Languages.unknown, CurrentOperatorEVSEData.OperatorName);

                                        WWCP.EVSE_Id?  CurrentEVSEId   = null;
                                        UInt64         EVSEsSkipped    = 0;

                                        #region Generate a list of all charging pools/stations/EVSEs

                                        _CPInfoList = new CPInfoList(WWCPChargingStationOperator.Id);

                                        foreach (var CurrentEVSEDataRecord in CurrentOperatorEVSEData.EVSEDataRecords)
                                        {

                                            CurrentEVSEId = CurrentEVSEDataRecord.Id.ToWWCP();

                                            if (CurrentEVSEId.HasValue)
                                            {

                                                try
                                                {
                                                                                  // Generate a stable charging pool identification
                                                    _CPInfoList.AddOrUpdateCPInfo(ChargingPool_Id.Generate(CurrentEVSEDataRecord.Id.OperatorId.ToWWCP().Value,
                                                                                                           CurrentEVSEDataRecord.Address.      ToWWCP(),
                                                                                                           CurrentEVSEDataRecord.GeoCoordinate),
                                                                                  CurrentEVSEDataRecord.Address,
                                                                                  CurrentEVSEDataRecord.GeoCoordinate,
                                                                                  CurrentEVSEDataRecord.ChargingStationId,
                                                                                  CurrentEVSEDataRecord.Id);

                                                } catch (Exception e)
                                                {
                                                    DebugX.Log("EMPClient PullEVSEData failed: " + e.Message);
                                                    EVSEsSkipped++;
                                                }

                                            }

                                            else
                                                // Invalid WWCP EVSE identification
                                                EVSEsSkipped++;

                                        }

                                        var EVSEIdLookup = _CPInfoList.VerifyUniquenessOfChargingStationIds();

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

                                        foreach (var CurrentEVSEDataRecord in CurrentOperatorEVSEData.EVSEDataRecords)
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
                                                        _ChargingPool.HotlinePhoneNumber    = CurrentEVSEDataRecord.HotlinePhoneNumber;

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
                                                                                pool.HotlinePhoneNumber          = CurrentEVSEDataRecord.HotlinePhoneNumber;
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
                                                        _ChargingStation.HubjectStationId           = CurrentEVSEDataRecord.ChargingStationId;
                                                        _ChargingStation.Description                = CurrentEVSEDataRecord.AdditionalInfo;
                                                        _ChargingStation.AuthenticationModes        = new ReactiveSet<WWCP.AuthenticationModes>(CurrentEVSEDataRecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OICPMapper.AsWWCPAuthenticationMode(mode)));
                                                        _ChargingStation.PaymentOptions             = new ReactiveSet<WWCP.PaymentOptions>     (CurrentEVSEDataRecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OICPMapper.AsWWCPPaymentOption(option)));
                                                        _ChargingStation.Accessibility              = CurrentEVSEDataRecord.Accessibility.ToWWCP();
                                                        _ChargingStation.HotlinePhoneNumber         = CurrentEVSEDataRecord.HotlinePhoneNumber;
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
                                                                                    station.HubjectStationId           = CurrentEVSEDataRecord.ChargingStationId;
                                                                                    station.Description                = CurrentEVSEDataRecord.AdditionalInfo;
                                                                                    station.AuthenticationModes        = new ReactiveSet<WWCP.AuthenticationModes>(CurrentEVSEDataRecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OICPMapper.AsWWCPAuthenticationMode(mode)));
                                                                                    station.PaymentOptions             = new ReactiveSet<WWCP.PaymentOptions>     (CurrentEVSEDataRecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OICPMapper.AsWWCPPaymentOption(option)));
                                                                                    station.Accessibility              = CurrentEVSEDataRecord.Accessibility.ToWWCP();
                                                                                    station.HotlinePhoneNumber         = CurrentEVSEDataRecord.HotlinePhoneNumber;
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
                                                        _EVSE.ChargingModes   = new ReactiveSet<WWCP.ChargingModes>(CurrentEVSEDataRecord.ChargingModes.ToEnumeration().SafeSelect(mode => OICPMapper.AsWWCPChargingMode(mode)));
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
                                                                                        evse.ChargingModes   = new ReactiveSet<WWCP.ChargingModes>(CurrentEVSEDataRecord.ChargingModes.ToEnumeration().SafeSelect(mode => OICPMapper.AsWWCPChargingMode(mode)));
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

                    //}

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


                    var EndTime = DateTime.Now;
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
                    Monitor.Exit(PullDataServiceLock);
                }

            }

            else
                Console.WriteLine("PullDataServiceLock missed!");

            return;

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

            if (Monitor.TryEnter(PullStatusServiceLock,
                                 TimeSpan.FromSeconds(5)))
            {

                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

                var StartTime = DateTime.Now;
                DebugX.LogT("[" + Id + "] 'Pull status service' started at " + StartTime.ToIso8601());

                try
                {

                    var PullEVSEStatusTask  = EMPRoaming.PullEVSEStatus(DefaultProviderId.Value,
                                                                        DefaultSearchCenter,
                                                                        DefaultDistanceKM.HasValue ? DefaultDistanceKM.Value : 0,

                                                                        CancellationToken:  new CancellationTokenSource().Token,
                                                                        EventTrackingId:    EventTracking_Id.New,
                                                                        RequestTimeout:     PullStatusServiceRequestTimeout);

                    PullEVSEStatusTask.Wait();

                    var DownloadTime = DateTime.Now;

                    #region Everything is ok!

                    if (PullEVSEStatusTask.Result                    != null   &&
                        PullEVSEStatusTask.Result.Content            != null   &&
                        PullEVSEStatusTask.Result.Content.StatusCode != null   &&
                        PullEVSEStatusTask.Result.Content.StatusCode.HasResult &&
                        PullEVSEStatusTask.Result.Content.StatusCode.Code == StatusCodes.Success)
                    {

                        var OperatorEVSEStatus = PullEVSEStatusTask.Result.Content.OperatorEVSEStatus;

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

                    else if (PullEVSEStatusTask.Result.HTTPStatusCode != HTTPStatusCode.OK)
                    {

                        DebugX.Log("Importing EVSE status records failed: " +
                                   PullEVSEStatusTask.Result.HTTPStatusCode.ToString() +

                                   PullEVSEStatusTask.Result.HTTPBody != null
                                       ? Environment.NewLine + PullEVSEStatusTask.Result.HTTPBody.ToUTF8String()
                                       : "");

                    }

                    #endregion

                    #region OICP StatusCode is not 'Success'

                    else if (PullEVSEStatusTask.Result.Content.StatusCode != null &&
                            !PullEVSEStatusTask.Result.Content.StatusCode.HasResult)
                    {

                        DebugX.Log("Importing EVSE status records failed: " +
                                   PullEVSEStatusTask.Result.Content.StatusCode.Code.ToString() +

                                   (PullEVSEStatusTask.Result.Content.StatusCode.Description.IsNotNullOrEmpty()
                                        ? ", " + PullEVSEStatusTask.Result.Content.StatusCode.Description
                                        : "") +

                                   (PullEVSEStatusTask.Result.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                        ? ", " + PullEVSEStatusTask.Result.Content.StatusCode.AdditionalInfo
                                        : ""));

                    }

                    #endregion

                    #region Something unexpected happend!

                    else
                    {
                        DebugX.Log("Importing EVSE status records failed unexpectedly!");
                    }

                    #endregion


                    var EndTime = DateTime.Now;

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
                    Monitor.Exit(PullStatusServiceLock);
                }

            }

            else
                Console.WriteLine("PullStatusServiceLock missed!");

            return;

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => "OICP" + Version.Number + " EMP Adapter " + Id;

        #endregion

    }

}
