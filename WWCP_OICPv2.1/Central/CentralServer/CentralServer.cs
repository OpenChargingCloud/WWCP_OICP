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
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.Central
{

    /// <summary>
    /// An OICP Central HTTP/SOAP/XML server.
    /// </summary>
    public class CentralServer : ASOAPServer
    {

        #region Data

        /// <summary>
        /// The default HTTP/SOAP/XML server name.
        /// </summary>
        public new const           String           DefaultHTTPServerName           = "GraphDefined OICP " + Version.Number + " HTTP/SOAP/XML Central API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort           DefaultHTTPServerPort           = IPPort.Parse(2003);

        /// <summary>
        /// The default HTTP/SOAP/XML server URI prefix.
        /// </summary>
        public new static readonly HTTPPath          DefaultURIPrefix                = HTTPPath.Parse("/");

        /// <summary>
        /// The default HTTP/SOAP/XML URI for OICP EvseData requests.
        /// </summary>
        public     const           String           DefaultEVSEDataURI              = "/eRoamingEvseData_V2.1";

        /// <summary>
        /// The default HTTP/SOAP/XML URI for OICP EvseStatus requests.
        /// </summary>
        public     const           String           DefaultEVSEStatusURI            = "/eRoamingEvseStatus_V2.0";

        /// <summary>
        /// The default HTTP/SOAP/XML URI for OICP AuthenticationData requests.
        /// </summary>
        public     const           String           DefaultAuthenticationDataURI    = "/eRoamingAuthenticationData_V2.0";

        /// <summary>
        /// The default HTTP/SOAP/XML URI for OICP Reservation requests.
        /// </summary>
        public     const           String           DefaultReservationURI           = "/eRoamingReservation_V1.0";

        /// <summary>
        /// The default HTTP/SOAP/XML URI for OICP Authorization requests.
        /// </summary>
        public     const           String           DefaultAuthorizationURI         = "/eRoamingAuthorization_V2.0";

        /// <summary>
        /// The default HTTP/SOAP/XML URI for OICP MobileAuthorization requests.
        /// </summary>
        public     const           String           DefaultMobileAuthorizationURI   = "/eRoamingMobileAuthorization_V2.0";

        /// <summary>
        /// The default HTTP/SOAP/XML content type.
        /// </summary>
        public new static readonly HTTPContentType  DefaultContentType              = HTTPContentType.XMLTEXT_UTF8;

        /// <summary>
        /// The default request timeout.
        /// </summary>
        public new static readonly TimeSpan         DefaultRequestTimeout           = TimeSpan.FromMinutes(1);

        #endregion

        #region Properties

        /// <summary>
        /// The identification of this HTTP/SOAP service.
        /// </summary>
        public String           ServiceId                 { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP EvseData requests.
        /// </summary>
        public String           EVSEDataURI               { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP EvseStatus requests.
        /// </summary>
        public String           EVSEStatusURI             { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP AuthenticationData requests.
        /// </summary>
        public String           AuthenticationDataURI     { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP Reservation requests.
        /// </summary>
        public String           ReservationURI            { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP Authorization requests.
        /// </summary>
        public String           AuthorizationURI          { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP Mobile Authorization requests.
        /// </summary>
        public String           MobileAuthorizationURI    { get; }

        #endregion

        #region Custom request/response mappers

        #region EMP event delegates...

        public CustomXMLParserDelegate<EMP.PullEVSEDataRequest>                                          CustomPullEVSEDataRequestParser                            { get; set; }
        public CustomXMLSerializerDelegate<OperatorEVSEData>                                             CustomOperatorEVSEDataSerializer                           { get; set; }
        public CustomXMLSerializerDelegate<EVSEDataRecord>                                               CustomEVSEDataRecordSerializer                             { get; set; }

        public CustomXMLParserDelegate<EMP.PullEVSEStatusRequest>                                        CustomPullEVSEStatusRequestParser                          { get; set; }
        public CustomXMLSerializerDelegate<EMP.EVSEStatus>                                               CustomEVSEStatusSerializer                                 { get; set; }
        public CustomXMLSerializerDelegate<OperatorEVSEStatus>                                           CustomOperatorEVSEStatusSerializer                         { get; set; }
        public CustomXMLSerializerDelegate<EVSEStatusRecord>                                             CustomEVSEStatusRecordSerializer                           { get; set; }

        public CustomXMLParserDelegate<EMP.PullEVSEStatusByIdRequest>                                    CustomPullEVSEStatusByIdRequestParser                      { get; set; }
        public CustomXMLSerializerDelegate<EMP.EVSEStatusById>                                           CustomEVSEStatusByIdSerializer                             { get; set; }

        public CustomXMLParserDelegate<EMP.PushAuthenticationDataRequest>                                CustomPushAuthenticationDataRequestParser                  { get; set; }
        public CustomXMLParserDelegate<ProviderAuthenticationData>                                       CustomProviderAuthenticationDataParser                     { get; set; }
        public CustomXMLParserDelegate<Identification>                                                   CustomAuthorizationIdentificationParser                    { get; set; }
        public CustomXMLSerializerDelegate<EMP.PushAuthenticationDataRequest>                            CustomPushAuthenticationDataRequestSerializer              { get; set; }
        public CustomXMLSerializerDelegate<Acknowledgement<EMP.PushAuthenticationDataRequest>>           CustomPushAuthenticationDataResponseSerializer             { get; set; }
        public CustomXMLSerializerDelegate<StatusCode>                                                   CustomStatusCodeSerializer                                 { get; set; }

        public CustomXMLParserDelegate<EMP.AuthorizeRemoteReservationStartRequest>                       CustomAuthorizeRemoteReservationStartRequestParser         { get; set; }
        public CustomXMLParserDelegate<Identification>                                                   CustomIdentificationParser                                 { get; set; }
        public CustomXMLSerializerDelegate<Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>>  CustomAuthorizeRemoteReservationStartResponseSerializer    { get; set; }

        public CustomXMLParserDelegate<EMP.AuthorizeRemoteReservationStopRequest>                        CustomAuthorizeRemoteReservationStopRequestParser          { get; set; }
        public CustomXMLSerializerDelegate<Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>>   CustomAuthorizeRemoteReservationStopResponseSerializer     { get; set; }

        public CustomXMLParserDelegate<EMP.AuthorizeRemoteStartRequest>                                  CustomAuthorizeRemoteStartRequestParser                    { get; set; }
        public CustomXMLSerializerDelegate<Acknowledgement<EMP.AuthorizeRemoteStartRequest>>             CustomAuthorizeRemoteStartResponseSerializer               { get; set; }

        public CustomXMLParserDelegate<EMP.AuthorizeRemoteStopRequest>                                   CustomAuthorizeRemoteStopRequestParser                     { get; set; }
        public CustomXMLSerializerDelegate<Acknowledgement<EMP.AuthorizeRemoteStopRequest>>              CustomAuthorizeRemoteStopResponseSerializer                { get; set; }

        public CustomXMLParserDelegate<EMP.GetChargeDetailRecordsRequest>                                CustomGetChargeDetailRecordsRequestParser                  { get; set; }
        public CustomXMLSerializerDelegate<EMP.GetChargeDetailRecordsResponse>                           CustomGetChargeDetailRecordsResponseSerializer             { get; set; }
        public CustomXMLSerializerDelegate<ChargeDetailRecord>                                           CustomChargeDetailRecordSerializer                         { get; set; }
        public CustomXMLSerializerDelegate<Identification>                                               CustomIdentificationSerializer                             { get; set; }

        #endregion

        #region CPO event delegates...

        public CustomXMLParserDelegate<OperatorEVSEData>                                                 CustomOperatorEVSEDataParser                               { get; set; }
        public CustomXMLParserDelegate<EVSEDataRecord>                                                   CustomEVSEDataRecordParser                                 { get; set; }
        public CustomXMLSerializerDelegate<Acknowledgement<CPO.PushEVSEDataRequest>>                     CustomPushEVSEDataResponseSerializer                       { get; set; }

        public CustomXMLParserDelegate<OperatorEVSEStatus>                                               CustomOperatorEVSEStatusParser                             { get; set; }
        public CustomXMLParserDelegate<EVSEStatusRecord>                                                 CustomEVSEStatusRecordParser                               { get; set; }
        public CustomXMLSerializerDelegate<Acknowledgement<CPO.PushEVSEStatusRequest>>                   CustomPushEVSEStatusResponseSerializer                     { get; set; }

        public CustomXMLParserDelegate<CPO.AuthorizeStartRequest>                                        CustomAuthorizeStartRequestParser                          { get; set; }
        public CustomXMLSerializerDelegate<CPO.AuthorizationStart>                                       CustomAuthorizationStartSerializer                         { get; set; }

        public CustomXMLParserDelegate<CPO.AuthorizeStopRequest>                                         CustomAuthorizeStopRequestParser                           { get; set; }
        public CustomXMLSerializerDelegate<CPO.AuthorizationStop>                                        CustomAuthorizationStopSerializer                          { get; set; }

        public CustomXMLParserDelegate<ChargeDetailRecord>                                               CustomChargeDetailRecordParser                             { get; set; }
        public CustomXMLSerializerDelegate<Acknowledgement<CPO.SendChargeDetailRecordRequest>>           CustomSendChargeDetailRecordResponseSerializer             { get; set; }

        public CustomXMLParserDelegate<CPO.PullAuthenticationDataRequest>                                CustomPullAuthenticationDataRequestParser                  { get; set; }
        public CustomXMLSerializerDelegate<CPO.AuthenticationData>                                       CustomAuthenticationDataSerializer                         { get; set; }
        public CustomXMLSerializerDelegate<ProviderAuthenticationData>                                   CustomProviderAuthenticationDataSerializer                 { get; set; }

        #endregion

        #region  Mobile event delegates...

        public CustomXMLParserDelegate<Mobile.MobileAuthorizeStartRequest>                               CustomMobileAuthorizeStartRequestParser                    { get; set; }
        public CustomXMLSerializerDelegate<Mobile.MobileAuthorizationStart>                              CustomMobileAuthorizationStartSerializer                   { get; set; }
        public CustomXMLSerializerDelegate<Address>                                                      CustomAddressSerializer                                    { get; set; }

        public CustomXMLParserDelegate<Mobile.MobileRemoteStartRequest>                                  CustomMobileRemoteStartRequestParser                       { get; set; }
        public CustomXMLSerializerDelegate<Acknowledgement<Mobile.MobileRemoteStartRequest>>             CustomMobileRemoteStartResponseSerializer                  { get; set; }

        public CustomXMLParserDelegate<Mobile.MobileRemoteStopRequest>                                   CustomMobileRemoteStopRequestParser                        { get; set; }
        public CustomXMLSerializerDelegate<Acknowledgement<Mobile.MobileRemoteStopRequest>>              CustomMobileRemoteStopResponseSerializer                   { get; set; }

        #endregion

        public OnExceptionDelegate  OnException    { get; set; }

        #endregion

        #region Events

        #region EMP events...

        #region OnPullEVSEData

        /// <summary>
        /// An event sent whenever a PullEVSEData SOAP request was received.
        /// </summary>
        public event RequestLogHandler                OnPullEVSEDataSOAPRequest;

        /// <summary>
        /// An event sent whenever a PullEVSEData request was received.
        /// </summary>
        public event OnPullEVSEDataRequestDelegate    OnPullEVSEDataRequest;

        /// <summary>
        /// An event sent whenever a PullEVSEData command was received.
        /// </summary>
        public event OnPullEVSEDataDelegate           OnPullEVSEData;

        /// <summary>
        /// An event sent whenever a PullEVSEData response was sent.
        /// </summary>
        public event OnPullEVSEDataResponseDelegate   OnPullEVSEDataResponse;

        /// <summary>
        /// An event sent whenever a PullEVSEData SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                 OnPullEVSEDataSOAPResponse;

        #endregion

        #region OnPullEVSEStatus

        /// <summary>
        /// An event sent whenever a PullEVSEStatus SOAP request was received.
        /// </summary>
        public event RequestLogHandler                  OnPullEVSEStatusSOAPRequest;

        /// <summary>
        /// An event sent whenever a PullEVSEStatus request was received.
        /// </summary>
        public event OnPullEVSEStatusRequestDelegate    OnPullEVSEStatusRequest;

        /// <summary>
        /// An event sent whenever a PullEVSEStatus command was received.
        /// </summary>
        public event OnPullEVSEStatusDelegate           OnPullEVSEStatus;

        /// <summary>
        /// An event sent whenever a PullEVSEStatus response was sent.
        /// </summary>
        public event OnPullEVSEStatusResponseDelegate   OnPullEVSEStatusResponse;

        /// <summary>
        /// An event sent whenever a PullEVSEStatus SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                   OnPullEVSEStatusSOAPResponse;

        #endregion

        #region OnPullEVSEStatusById

        /// <summary>
        /// An event sent whenever a PullEVSEStatusById SOAP request was received.
        /// </summary>
        public event RequestLogHandler                      OnPullEVSEStatusByIdSOAPRequest;

        /// <summary>
        /// An event sent whenever a PullEVSEStatusById request was received.
        /// </summary>
        public event OnPullEVSEStatusByIdRequestDelegate    OnPullEVSEStatusByIdRequest;

        /// <summary>
        /// An event sent whenever a PullEVSEStatusById command was received.
        /// </summary>
        public event OnPullEVSEStatusByIdDelegate           OnPullEVSEStatusById;

        /// <summary>
        /// An event sent whenever a PullEVSEStatusById response was sent.
        /// </summary>
        public event OnPullEVSEStatusByIdResponseDelegate   OnPullEVSEStatusByIdResponse;

        /// <summary>
        /// An event sent whenever a PullEVSEStatusById SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                       OnPullEVSEStatusByIdSOAPResponse;

        #endregion


        #region OnPushAuthenticationData

        /// <summary>
        /// An event sent whenever a PushAuthenticationData SOAP request was received.
        /// </summary>
        public event RequestLogHandler                          OnPushAuthenticationDataSOAPRequest;

        /// <summary>
        /// An event sent whenever a PushAuthenticationData request was received.
        /// </summary>
        public event OnPushAuthenticationDataRequestDelegate    OnPushAuthenticationDataRequest;

        /// <summary>
        /// An event sent whenever a PushAuthenticationData command was received.
        /// </summary>
        public event OnPushAuthenticationDataDelegate           OnPushAuthenticationData;

        /// <summary>
        /// An event sent whenever a PushAuthenticationData response was sent.
        /// </summary>
        public event OnPushAuthenticationDataResponseDelegate   OnPushAuthenticationDataResponse;

        /// <summary>
        /// An event sent whenever a PushAuthenticationData SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                           OnPushAuthenticationDataSOAPResponse;

        #endregion


        #region OnAuthorizeRemoteReservationStart

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteReservationStart SOAP request was received.
        /// </summary>
        public event RequestLogHandler                                   OnAuthorizeRemoteReservationStartSOAPRequest;

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteReservationStart request was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartRequestDelegate    OnAuthorizeRemoteReservationStartRequest;

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteReservationStart command was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartDelegate           OnAuthorizeRemoteReservationStart;

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteReservationStart response was sent.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartResponseDelegate   OnAuthorizeRemoteReservationStartResponse;

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteReservationStart SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                                    OnAuthorizeRemoteReservationStartSOAPResponse;

        #endregion

        #region OnAuthorizeRemoteReservationStop

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteReservationStop SOAP request was received.
        /// </summary>
        public event RequestLogHandler                                  OnAuthorizeRemoteReservationStopSOAPRequest;

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteReservationStop request was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopRequestDelegate    OnAuthorizeRemoteReservationStopRequest;

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteReservationStop command was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopDelegate           OnAuthorizeRemoteReservationStop;

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteReservationStop response was sent.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopResponseDelegate   OnAuthorizeRemoteReservationStopResponse;

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteReservationStop SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                                   OnAuthorizeRemoteReservationStopSOAPResponse;

        #endregion

        #region OnAuthorizeRemoteStart

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteStart SOAP request was received.
        /// </summary>
        public event RequestLogHandler                        OnAuthorizeRemoteStartSOAPRequest;

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteStart request was received.
        /// </summary>
        public event OnAuthorizeRemoteStartRequestDelegate    OnAuthorizeRemoteStartRequest;

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteStart command was received.
        /// </summary>
        public event OnAuthorizeRemoteStartDelegate           OnAuthorizeRemoteStart;

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteStart response was sent.
        /// </summary>
        public event OnAuthorizeRemoteStartResponseDelegate   OnAuthorizeRemoteStartResponse;

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteStart SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                         OnAuthorizeRemoteStartSOAPResponse;

        #endregion

        #region OnAuthorizeRemoteStop

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteStop SOAP request was received.
        /// </summary>
        public event RequestLogHandler                       OnAuthorizeRemoteStopSOAPRequest;

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteStop request was received.
        /// </summary>
        public event OnAuthorizeRemoteStopRequestDelegate    OnAuthorizeRemoteStopRequest;

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteStop command was received.
        /// </summary>
        public event OnAuthorizeRemoteStopDelegate           OnAuthorizeRemoteStop;

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteStop response was sent.
        /// </summary>
        public event OnAuthorizeRemoteStopResponseDelegate   OnAuthorizeRemoteStopResponse;

        /// <summary>
        /// An event sent whenever a AuthorizeRemoteStop SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                        OnAuthorizeRemoteStopSOAPResponse;

        #endregion


        #region OnGetChargeDetailRecords

        /// <summary>
        /// An event sent whenever a GetChargeDetailRecords SOAP request was received.
        /// </summary>
        public event RequestLogHandler                          OnGetChargeDetailRecordsSOAPRequest;

        /// <summary>
        /// An event sent whenever a GetChargeDetailRecords request was received.
        /// </summary>
        public event OnGetChargeDetailRecordsRequestDelegate    OnGetChargeDetailRecordsRequest;

        /// <summary>
        /// An event sent whenever a GetChargeDetailRecords command was received.
        /// </summary>
        public event OnGetChargeDetailRecordsDelegate           OnGetChargeDetailRecords;

        /// <summary>
        /// An event sent whenever a GetChargeDetailRecords response was sent.
        /// </summary>
        public event OnGetChargeDetailRecordsResponseDelegate   OnGetChargeDetailRecordsResponse;

        /// <summary>
        /// An event sent whenever a GetChargeDetailRecords SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                           OnGetChargeDetailRecordsSOAPResponse;

        #endregion

        #endregion

        #region CPO events...

        #region OnPushEVSEData

        /// <summary>
        /// An event sent whenever a PushEVSEData SOAP request was received.
        /// </summary>
        public event RequestLogHandler                OnPushEVSEDataSOAPRequest;

        /// <summary>
        /// An event sent whenever a PushEVSEData request was received.
        /// </summary>
        public event OnPushEVSEDataRequestDelegate    OnPushEVSEDataRequest;

        /// <summary>
        /// An event sent whenever a PushEVSEData command was received.
        /// </summary>
        public event OnPushEVSEDataDelegate           OnPushEVSEData;

        /// <summary>
        /// An event sent whenever a PushEVSEData response was sent.
        /// </summary>
        public event OnPushEVSEDataResponseDelegate   OnPushEVSEDataResponse;

        /// <summary>
        /// An event sent whenever a PushEVSEData SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                 OnPushEVSEDataSOAPResponse;

        #endregion

        #region OnPushEVSEStatus

        /// <summary>
        /// An event sent whenever a PushEVSEStatus SOAP request was received.
        /// </summary>
        public event RequestLogHandler                  OnPushEVSEStatusSOAPRequest;

        /// <summary>
        /// An event sent whenever a PushEVSEStatus request was received.
        /// </summary>
        public event OnPushEVSEStatusRequestDelegate    OnPushEVSEStatusRequest;

        /// <summary>
        /// An event sent whenever a PushEVSEStatus command was received.
        /// </summary>
        public event OnPushEVSEStatusDelegate           OnPushEVSEStatus;

        /// <summary>
        /// An event sent whenever a PushEVSEStatus response was sent.
        /// </summary>
        public event OnPushEVSEStatusResponseDelegate   OnPushEVSEStatusResponse;

        /// <summary>
        /// An event sent whenever a PushEVSEStatus SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                   OnPushEVSEStatusSOAPResponse;

        #endregion


        #region OnAuthorizeStart

        /// <summary>
        /// An event sent whenever a AuthorizeStart SOAP request was received.
        /// </summary>
        public event RequestLogHandler                  OnAuthorizeStartSOAPRequest;

        /// <summary>
        /// An event sent whenever a AuthorizeStart request was received.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate    OnAuthorizeStartRequest;

        /// <summary>
        /// An event sent whenever a AuthorizeStart command was received.
        /// </summary>
        public event OnAuthorizeStartDelegate           OnAuthorizeStart;

        /// <summary>
        /// An event sent whenever a AuthorizeStart response was sent.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate   OnAuthorizeStartResponse;

        /// <summary>
        /// An event sent whenever a AuthorizeStart SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                   OnAuthorizeStartSOAPResponse;

        #endregion

        #region OnAuthorizeStop

        /// <summary>
        /// An event sent whenever a AuthorizeStop SOAP request was received.
        /// </summary>
        public event RequestLogHandler                 OnAuthorizeStopSOAPRequest;

        /// <summary>
        /// An event sent whenever a AuthorizeStop request was received.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate    OnAuthorizeStopRequest;

        /// <summary>
        /// An event sent whenever a AuthorizeStop command was received.
        /// </summary>
        public event OnAuthorizeStopDelegate           OnAuthorizeStop;

        /// <summary>
        /// An event sent whenever a AuthorizeStop response was sent.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate   OnAuthorizeStopResponse;

        /// <summary>
        /// An event sent whenever a AuthorizeStop SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                  OnAuthorizeStopSOAPResponse;

        #endregion

        #region OnSendChargeDetailRecord

        /// <summary>
        /// An event sent whenever a SendChargeDetailRecord SOAP request was received.
        /// </summary>
        public event RequestLogHandler                          OnSendChargeDetailRecordSOAPRequest;

        /// <summary>
        /// An event sent whenever a SendChargeDetailRecord request was received.
        /// </summary>
        public event OnSendChargeDetailRecordRequestDelegate    OnSendChargeDetailRecordRequest;

        /// <summary>
        /// An event sent whenever a SendChargeDetailRecord command was received.
        /// </summary>
        public event OnSendChargeDetailRecordDelegate           OnSendChargeDetailRecord;

        /// <summary>
        /// An event sent whenever a SendChargeDetailRecord response was sent.
        /// </summary>
        public event OnSendChargeDetailRecordResponseDelegate   OnSendChargeDetailRecordResponse;

        /// <summary>
        /// An event sent whenever a SendChargeDetailRecord SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                           OnSendChargeDetailRecordSOAPResponse;

        #endregion


        #region OnPullAuthenticationData

        /// <summary>
        /// An event sent whenever a PullAuthenticationData SOAP request was received.
        /// </summary>
        public event RequestLogHandler                          OnPullAuthenticationDataSOAPRequest;

        /// <summary>
        /// An event sent whenever a PullAuthenticationData request was received.
        /// </summary>
        public event OnPullAuthenticationDataRequestDelegate    OnPullAuthenticationDataRequest;

        /// <summary>
        /// An event sent whenever a PullAuthenticationData command was received.
        /// </summary>
        public event OnPullAuthenticationDataDelegate           OnPullAuthenticationData;

        /// <summary>
        /// An event sent whenever a PullAuthenticationData response was sent.
        /// </summary>
        public event OnPullAuthenticationDataResponseDelegate   OnPullAuthenticationDataResponse;

        /// <summary>
        /// An event sent whenever a PullAuthenticationData SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                           OnPullAuthenticationDataSOAPResponse;

        #endregion

        #endregion

        #region Mobile events...

        #region OnMobileAuthorizeStart

        /// <summary>
        /// An event sent whenever a MobileAuthorizeStart SOAP request was received.
        /// </summary>
        public event RequestLogHandler                        OnMobileAuthorizeStartSOAPRequest;

        /// <summary>
        /// An event sent whenever a MobileAuthorizeStart request was received.
        /// </summary>
        public event OnMobileAuthorizeStartRequestDelegate    OnMobileAuthorizeStartRequest;

        /// <summary>
        /// An event sent whenever a MobileAuthorizeStart command was received.
        /// </summary>
        public event OnMobileAuthorizeStartDelegate           OnMobileAuthorizeStart;

        /// <summary>
        /// An event sent whenever a MobileAuthorizeStart response was sent.
        /// </summary>
        public event OnMobileAuthorizeStartResponseDelegate   OnMobileAuthorizeStartResponse;

        /// <summary>
        /// An event sent whenever a MobileAuthorizeStart SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                         OnMobileAuthorizeStartSOAPResponse;

        #endregion

        #region OnMobileRemoteStart

        /// <summary>
        /// An event sent whenever a MobileRemoteStart SOAP request was received.
        /// </summary>
        public event RequestLogHandler                     OnMobileRemoteStartSOAPRequest;

        /// <summary>
        /// An event sent whenever a MobileRemoteStart request was received.
        /// </summary>
        public event OnMobileRemoteStartRequestDelegate    OnMobileRemoteStartRequest;

        /// <summary>
        /// An event sent whenever a MobileRemoteStart command was received.
        /// </summary>
        public event OnMobileRemoteStartDelegate           OnMobileRemoteStart;

        /// <summary>
        /// An event sent whenever a MobileRemoteStart response was sent.
        /// </summary>
        public event OnMobileRemoteStartResponseDelegate   OnMobileRemoteStartResponse;

        /// <summary>
        /// An event sent whenever a MobileRemoteStart SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                      OnMobileRemoteStartSOAPResponse;

        #endregion

        #region OnMobileRemoteStop

        /// <summary>
        /// An event sent whenever a MobileRemoteStop SOAP request was received.
        /// </summary>
        public event RequestLogHandler                    OnMobileRemoteStopSOAPRequest;

        /// <summary>
        /// An event sent whenever a MobileRemoteStop request was received.
        /// </summary>
        public event OnMobileRemoteStopRequestDelegate    OnMobileRemoteStopRequest;

        /// <summary>
        /// An event sent whenever a MobileRemoteStop command was received.
        /// </summary>
        public event OnMobileRemoteStopDelegate           OnMobileRemoteStop;

        /// <summary>
        /// An event sent whenever a MobileRemoteStop response was sent.
        /// </summary>
        public event OnMobileRemoteStopResponseDelegate   OnMobileRemoteStopResponse;

        /// <summary>
        /// An event sent whenever a MobileRemoteStop SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                     OnMobileRemoteStopSOAPResponse;

        #endregion

        #endregion

        #endregion

        #region Constructor(s)

        #region CentralServer(HTTPServerName, TCPPort = default, URIPrefix = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OICP HTTP/SOAP/XML Central API.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public CentralServer(String           HTTPServerName            = DefaultHTTPServerName,
                             String           ServiceId                 = null,
                             IPPort?          TCPPort                   = null,
                             HTTPPath?         URIPrefix                 = null,
                             String           EVSEDataURI               = DefaultAuthorizationURI,
                             String           EVSEStatusURI             = DefaultAuthorizationURI,
                             String           AuthenticationDataURI     = DefaultAuthenticationDataURI,
                             String           ReservationURI            = DefaultReservationURI,
                             String           AuthorizationURI          = DefaultAuthorizationURI,
                             String           MobileAuthorizationURI    = DefaultMobileAuthorizationURI,
                             HTTPContentType  ContentType               = null,
                             Boolean          RegisterHTTPRootService   = true,
                             DNSClient        DNSClient                 = null,
                             Boolean          AutoStart                 = false)

            : base(HTTPServerName.IsNotNullOrEmpty() ? HTTPServerName : DefaultHTTPServerName,
                   TCPPort     ?? DefaultHTTPServerPort,
                   URIPrefix   ?? DefaultURIPrefix,
                   ContentType ?? DefaultContentType,
                   RegisterHTTPRootService,
                   DNSClient,
                   AutoStart: false)

        {

            this.ServiceId               = ServiceId              ?? nameof(CentralServer);
            this.EVSEDataURI             = EVSEDataURI            ?? DefaultEVSEDataURI;
            this.EVSEStatusURI           = EVSEStatusURI          ?? DefaultEVSEStatusURI;
            this.AuthenticationDataURI   = AuthenticationDataURI  ?? DefaultAuthenticationDataURI;
            this.ReservationURI          = ReservationURI         ?? DefaultReservationURI;
            this.AuthorizationURI        = AuthorizationURI       ?? DefaultAuthorizationURI;
            this.MobileAuthorizationURI  = MobileAuthorizationURI ?? DefaultMobileAuthorizationURI;

            RegisterURITemplates();

            if (AutoStart)
                Start();

        }

        #endregion

        #region CentralServer(SOAPServer, URIPrefix = default)

        /// <summary>
        /// Use the given SOAP server for the OICP HTTP/SOAP/XML Central API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        public CentralServer(SOAPServer  SOAPServer,
                             String      ServiceId                  = null,
                             HTTPPath?    URIPrefix                  = null,
                             String      EVSEDataURI                = DefaultAuthorizationURI,
                             String      EVSEStatusURI              = DefaultAuthorizationURI,
                             String      AuthorizationURI           = DefaultAuthorizationURI,
                             String      ReservationURI             = DefaultReservationURI,
                             String      MobileAuthorizationURI     = DefaultMobileAuthorizationURI)

            : base(SOAPServer,
                   URIPrefix ?? DefaultURIPrefix)

        {

            this.ServiceId               = ServiceId              ?? nameof(CentralServer);
            this.EVSEDataURI             = EVSEDataURI            ?? DefaultEVSEDataURI;
            this.EVSEStatusURI           = EVSEStatusURI          ?? DefaultEVSEStatusURI;
            this.AuthenticationDataURI   = AuthenticationDataURI  ?? DefaultAuthenticationDataURI;
            this.ReservationURI          = ReservationURI         ?? DefaultReservationURI;
            this.AuthorizationURI        = AuthorizationURI       ?? DefaultAuthorizationURI;
            this.MobileAuthorizationURI  = MobileAuthorizationURI ?? DefaultMobileAuthorizationURI;

            RegisterURITemplates();

        }

        #endregion

        #endregion


        #region (override) RegisterURITemplates()

        /// <summary>
        /// Register all URI templates for this SOAP API.
        /// </summary>
        protected void RegisterURITemplates()
        {

            #region EMP methods

            #region /EVSEData           - PullEVSEData

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + EVSEDataURI,
                                            "PullEVSEData",
                                            XML => XML.Descendants(OICPNS.EVSEData + "eRoamingPullEvseData").FirstOrDefault(),
                                            async (HTTPRequest, PullEvseDataXML) => {


                EMP.PullEVSEDataRequest PullEVSEDataRequest  = null;
                EVSEData                EVSEData             = null;

                #region Send OnPullEVSEDataSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    if (OnPullEVSEDataSOAPRequest != null)
                        await Task.WhenAll(OnPullEVSEDataSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnPullEVSEDataSOAPRequest));
                }

                #endregion


                if (EMP.PullEVSEDataRequest.TryParse(PullEvseDataXML,
                                                     out PullEVSEDataRequest,
                                                     CustomPullEVSEDataRequestParser,
                                                     OnException,

                                                     HTTPRequest.Timestamp,
                                                     HTTPRequest.CancellationToken,
                                                     HTTPRequest.EventTrackingId,
                                                     HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnPullEVSEDataRequest event

                    try
                    {

                        if (OnPullEVSEDataRequest != null)
                            await Task.WhenAll(OnPullEVSEDataRequest.GetInvocationList().
                                               Cast<OnPullEVSEDataRequestDelegate>().
                                               Select(e => e(StartTime,
                                                             PullEVSEDataRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             PullEVSEDataRequest.EventTrackingId,
                                                             PullEVSEDataRequest.ProviderId,
                                                             PullEVSEDataRequest.SearchCenter,
                                                             PullEVSEDataRequest.DistanceKM,
                                                             PullEVSEDataRequest.LastCall,
                                                             PullEVSEDataRequest.GeoCoordinatesResponseFormat,
                                                             PullEVSEDataRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnPullEVSEDataRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnPullEVSEData != null)
                    {

                        var results = await Task.WhenAll(OnPullEVSEData.GetInvocationList().
                                                             Cast<OnPullEVSEDataDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           PullEVSEDataRequest))).
                                                             ConfigureAwait(false);

                        EVSEData = results.FirstOrDefault();

                    }

                    //if (EVSEData == null)
                    //    EVSEData = EVSEData.SystemError(
                    //                         PullEVSEDataRequest,
                    //                         "Could not process the incoming AuthorizeRemoteReservationStart request!",
                    //                         null,
                    //                         PullEVSEDataRequest.SessionId,
                    //                         PullEVSEDataRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnPullEVSEDataResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnPullEVSEDataResponse != null)
                            await Task.WhenAll(OnPullEVSEDataResponse.GetInvocationList().
                                               Cast<OnPullEVSEDataResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             PullEVSEDataRequest.EventTrackingId,
                                                             PullEVSEDataRequest.ProviderId,
                                                             PullEVSEDataRequest.SearchCenter,
                                                             PullEVSEDataRequest.DistanceKM,
                                                             PullEVSEDataRequest.LastCall,
                                                             PullEVSEDataRequest.GeoCoordinatesResponseFormat,
                                                             PullEVSEDataRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             EVSEData,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnPullEVSEDataResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(EVSEData.ToXML(CustomOperatorEVSEDataSerializer: CustomOperatorEVSEDataSerializer,
                                                                        CustomEVSEDataRecordSerializer:   CustomEVSEDataRecordSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnPullEVSEDataSOAPResponse event

                try
                {

                    if (OnPullEVSEDataSOAPResponse != null)
                        await Task.WhenAll(OnPullEVSEDataSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnPullEVSEDataSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /EVSEStatus         - PullEVSEStatus

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + EVSEStatusURI,
                                            "PullEVSEStatus",
                                            XML => XML.Descendants(OICPNS.EVSEStatus + "eRoamingPullEvseStatus").FirstOrDefault(),
                                            async (HTTPRequest, PullEvseStatusXML) => {


                EMP.PullEVSEStatusRequest PullEVSEStatusRequest  = null;
                EMP.EVSEStatus            EVSEStatus             = null;

                #region Send OnPullEVSEStatusSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    if (OnPullEVSEStatusSOAPRequest != null)
                        await Task.WhenAll(OnPullEVSEStatusSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnPullEVSEStatusSOAPRequest));
                }

                #endregion


                if (EMP.PullEVSEStatusRequest.TryParse(PullEvseStatusXML,
                                                       out PullEVSEStatusRequest,
                                                       CustomPullEVSEStatusRequestParser,
                                                       OnException,

                                                       HTTPRequest.Timestamp,
                                                       HTTPRequest.CancellationToken,
                                                       HTTPRequest.EventTrackingId,
                                                       HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnPullEVSEStatusRequest event

                    try
                    {

                        if (OnPullEVSEStatusRequest != null)
                            await Task.WhenAll(OnPullEVSEStatusRequest.GetInvocationList().
                                               Cast<OnPullEVSEStatusRequestDelegate>().
                                               Select(e => e(StartTime,
                                                             PullEVSEStatusRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             PullEVSEStatusRequest.EventTrackingId,
                                                             PullEVSEStatusRequest.ProviderId,
                                                             PullEVSEStatusRequest.SearchCenter,
                                                             PullEVSEStatusRequest.DistanceKM,
                                                             PullEVSEStatusRequest.EVSEStatusFilter,
                                                             PullEVSEStatusRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnPullEVSEStatusRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnPullEVSEStatus != null)
                    {

                        var results = await Task.WhenAll(OnPullEVSEStatus.GetInvocationList().
                                                             Cast<OnPullEVSEStatusDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           PullEVSEStatusRequest))).
                                                             ConfigureAwait(false);

                        EVSEStatus = results.FirstOrDefault();

                    }

                    //if (EVSEStatus == null)
                    //    EVSEStatus = EVSEStatus.SystemError(
                    //                         PullEVSEStatusRequest,
                    //                         "Could not process the incoming AuthorizeRemoteReservationStart request!",
                    //                         null,
                    //                         PullEVSEStatusRequest.SessionId,
                    //                         PullEVSEStatusRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnPullEVSEStatusResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnPullEVSEStatusResponse != null)
                            await Task.WhenAll(OnPullEVSEStatusResponse.GetInvocationList().
                                               Cast<OnPullEVSEStatusResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             PullEVSEStatusRequest.EventTrackingId,
                                                             PullEVSEStatusRequest.ProviderId,
                                                             PullEVSEStatusRequest.SearchCenter,
                                                             PullEVSEStatusRequest.DistanceKM,
                                                             PullEVSEStatusRequest.EVSEStatusFilter,
                                                             PullEVSEStatusRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             EVSEStatus,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnPullEVSEStatusResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(EVSEStatus.ToXML(CustomOperatorEVSEStatusSerializer: CustomOperatorEVSEStatusSerializer,
                                                                          CustomEVSEStatusRecordSerializer:   CustomEVSEStatusRecordSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnPullEVSEStatusSOAPResponse event

                try
                {

                    if (OnPullEVSEStatusSOAPResponse != null)
                        await Task.WhenAll(OnPullEVSEStatusSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnPullEVSEStatusSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /EVSEStatus         - PullEVSEStatusById

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + EVSEStatusURI,
                                            "PullEVSEStatusById",
                                            XML => XML.Descendants(OICPNS.EVSEStatus + "eRoamingPullEvseStatusById").FirstOrDefault(),
                                            async (HTTPRequest, PullEvseStatusByIdXML) => {


                EMP.PullEVSEStatusByIdRequest PullEVSEStatusByIdRequest  = null;
                EMP.EVSEStatusById            EVSEStatusById             = null;

                #region Send OnPullEVSEStatusByIdSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    if (OnPullEVSEStatusByIdSOAPRequest != null)
                        await Task.WhenAll(OnPullEVSEStatusByIdSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnPullEVSEStatusByIdSOAPRequest));
                }

                #endregion


                if (EMP.PullEVSEStatusByIdRequest.TryParse(PullEvseStatusByIdXML,
                                                           out PullEVSEStatusByIdRequest,
                                                           CustomPullEVSEStatusByIdRequestParser,
                                                           OnException,

                                                           HTTPRequest.Timestamp,
                                                           HTTPRequest.CancellationToken,
                                                           HTTPRequest.EventTrackingId,
                                                           HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnPullEVSEStatusByIdRequest event

                    try
                    {

                        if (OnPullEVSEStatusByIdRequest != null)
                            await Task.WhenAll(OnPullEVSEStatusByIdRequest.GetInvocationList().
                                               Cast<OnPullEVSEStatusByIdRequestDelegate>().
                                               Select(e => e(StartTime,
                                                             PullEVSEStatusByIdRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             PullEVSEStatusByIdRequest.EventTrackingId,
                                                             PullEVSEStatusByIdRequest.ProviderId,
                                                             PullEVSEStatusByIdRequest.EVSEIds,
                                                             PullEVSEStatusByIdRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnPullEVSEStatusByIdRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnPullEVSEStatusById != null)
                    {

                        var results = await Task.WhenAll(OnPullEVSEStatusById.GetInvocationList().
                                                             Cast<OnPullEVSEStatusByIdDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           PullEVSEStatusByIdRequest))).
                                                             ConfigureAwait(false);

                        EVSEStatusById = results.FirstOrDefault();

                    }

                    //if (EVSEStatusById == null)
                    //    EVSEStatusById = EVSEStatusById.SystemError(
                    //                         PullEVSEStatusByIdRequest,
                    //                         "Could not process the incoming AuthorizeRemoteReservationStart request!",
                    //                         null,
                    //                         PullEVSEStatusByIdRequest.SessionId,
                    //                         PullEVSEStatusByIdRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnPullEVSEStatusByIdResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnPullEVSEStatusByIdResponse != null)
                            await Task.WhenAll(OnPullEVSEStatusByIdResponse.GetInvocationList().
                                               Cast<OnPullEVSEStatusByIdResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             PullEVSEStatusByIdRequest.EventTrackingId,
                                                             PullEVSEStatusByIdRequest.ProviderId,
                                                             PullEVSEStatusByIdRequest.EVSEIds,
                                                             PullEVSEStatusByIdRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             EVSEStatusById,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnPullEVSEStatusByIdResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(EVSEStatusById.ToXML(CustomEVSEStatusByIdSerializer:   CustomEVSEStatusByIdSerializer,
                                                                              CustomEVSEStatusRecordSerializer: CustomEVSEStatusRecordSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnPullEVSEStatusByIdSOAPResponse event

                try
                {

                    if (OnPullEVSEStatusByIdSOAPResponse != null)
                        await Task.WhenAll(OnPullEVSEStatusByIdSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnPullEVSEStatusByIdSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion


            #region /AuthenticationData - PushAuthenticationData

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + AuthenticationDataURI,
                                            "PushAuthenticationData",
                                            XML => XML.Descendants(OICPNS.AuthenticationData + "eRoamingPushAuthenticationData").FirstOrDefault(),
                                            async (HTTPRequest, PushAuthenticationDataXML) => {


                EMP.PushAuthenticationDataRequest                   PushAuthenticationDataRequest  = null;
                Acknowledgement<EMP.PushAuthenticationDataRequest>  Acknowledgement                = null;

                #region Send OnPushAuthenticationDataSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    if (OnPushAuthenticationDataSOAPRequest != null)
                        await Task.WhenAll(OnPushAuthenticationDataSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnPushAuthenticationDataSOAPRequest));
                }

                #endregion


                if (EMP.PushAuthenticationDataRequest.TryParse(PushAuthenticationDataXML,
                                                               out PushAuthenticationDataRequest,
                                                               CustomPushAuthenticationDataRequestParser,
                                                               CustomProviderAuthenticationDataParser,
                                                               CustomAuthorizationIdentificationParser,
                                                               OnException,

                                                               HTTPRequest.Timestamp,
                                                               HTTPRequest.CancellationToken,
                                                               HTTPRequest.EventTrackingId,
                                                               HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnPushAuthenticationDataRequest event

                    try
                    {

                        if (OnPushAuthenticationDataRequest != null)
                            await Task.WhenAll(OnPushAuthenticationDataRequest.GetInvocationList().
                                               Cast<OnPushAuthenticationDataRequestDelegate>().
                                               Select(e => e(StartTime,
                                                             PushAuthenticationDataRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             PushAuthenticationDataRequest.EventTrackingId,
                                                             PushAuthenticationDataRequest.ProviderAuthenticationData,
                                                             PushAuthenticationDataRequest.OICPAction,
                                                             PushAuthenticationDataRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnPushAuthenticationDataRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnPushAuthenticationData != null)
                    {

                        var results = await Task.WhenAll(OnPushAuthenticationData.GetInvocationList().
                                                             Cast<OnPushAuthenticationDataDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           PushAuthenticationDataRequest))).
                                                             ConfigureAwait(false);

                        Acknowledgement = results.FirstOrDefault();

                    }

                    //if (EVSEData == null)
                    //    EVSEData = EVSEData.SystemError(
                    //                         PushAuthenticationDataRequest,
                    //                         "Could not process the incoming AuthorizeRemoteReservationStart request!",
                    //                         null,
                    //                         PushAuthenticationDataRequest.SessionId,
                    //                         PushAuthenticationDataRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnPushAuthenticationDataResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnPushAuthenticationDataResponse != null)
                            await Task.WhenAll(OnPushAuthenticationDataResponse.GetInvocationList().
                                               Cast<OnPushAuthenticationDataResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             PushAuthenticationDataRequest.EventTrackingId,
                                                             PushAuthenticationDataRequest.ProviderAuthenticationData,
                                                             PushAuthenticationDataRequest.OICPAction,
                                                             PushAuthenticationDataRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             Acknowledgement,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnPushAuthenticationDataResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML(CustomPushAuthenticationDataResponseSerializer,
                                                                               CustomStatusCodeSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnPushAuthenticationDataSOAPResponse event

                try
                {

                    if (OnPushAuthenticationDataSOAPResponse != null)
                        await Task.WhenAll(OnPushAuthenticationDataSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnPushAuthenticationDataSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion


            #region /Reservation        - AuthorizeRemoteReservationStart

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + ReservationURI,
                                            "AuthorizeRemoteReservationStart",
                                            XML => XML.Descendants(OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStart").FirstOrDefault(),
                                            async (HTTPRequest, PullEvseDataXML) => {


                EMP.AuthorizeRemoteReservationStartRequest                   AuthorizeRemoteReservationStartRequest  = null;
                Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>  Acknowledgement                         = null;

                #region Send OnAuthorizeRemoteReservationStartSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    if (OnAuthorizeRemoteReservationStartSOAPRequest != null)
                        await Task.WhenAll(OnAuthorizeRemoteReservationStartSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeRemoteReservationStartSOAPRequest));
                }

                #endregion


                if (EMP.AuthorizeRemoteReservationStartRequest.TryParse(PullEvseDataXML,
                                                                        out AuthorizeRemoteReservationStartRequest,
                                                                        CustomAuthorizeRemoteReservationStartRequestParser,
                                                                        CustomIdentificationParser,
                                                                        OnException,

                                                                        HTTPRequest.Timestamp,
                                                                        HTTPRequest.CancellationToken,
                                                                        HTTPRequest.EventTrackingId,
                                                                        HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnAuthorizeRemoteReservationStartRequest event

                    try
                    {

                        if (OnAuthorizeRemoteReservationStartRequest != null)
                            await Task.WhenAll(OnAuthorizeRemoteReservationStartRequest.GetInvocationList().
                                               Cast<OnAuthorizeRemoteReservationStartRequestDelegate>().
                                               Select(e => e(StartTime,
                                                             AuthorizeRemoteReservationStartRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             AuthorizeRemoteReservationStartRequest.EventTrackingId,
                                                             AuthorizeRemoteReservationStartRequest.ProviderId,
                                                             AuthorizeRemoteReservationStartRequest.EVSEId,
                                                             AuthorizeRemoteReservationStartRequest.Identification,
                                                             AuthorizeRemoteReservationStartRequest.SessionId,
                                                             AuthorizeRemoteReservationStartRequest.PartnerSessionId,
                                                             AuthorizeRemoteReservationStartRequest.PartnerProductId,
                                                             AuthorizeRemoteReservationStartRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeRemoteReservationStartRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnAuthorizeRemoteReservationStart != null)
                    {

                        var results = await Task.WhenAll(OnAuthorizeRemoteReservationStart.GetInvocationList().
                                                             Cast<OnAuthorizeRemoteReservationStartDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           AuthorizeRemoteReservationStartRequest))).
                                                             ConfigureAwait(false);

                        Acknowledgement = results.FirstOrDefault();

                    }

                    //if (EVSEData == null)
                    //    EVSEData = EVSEData.SystemError(
                    //                         AuthorizeRemoteReservationStartRequest,
                    //                         "Could not process the incoming AuthorizeRemoteReservationStart request!",
                    //                         null,
                    //                         AuthorizeRemoteReservationStartRequest.SessionId,
                    //                         AuthorizeRemoteReservationStartRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnAuthorizeRemoteReservationStartResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnAuthorizeRemoteReservationStartResponse != null)
                            await Task.WhenAll(OnAuthorizeRemoteReservationStartResponse.GetInvocationList().
                                               Cast<OnAuthorizeRemoteReservationStartResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             AuthorizeRemoteReservationStartRequest.EventTrackingId,
                                                             AuthorizeRemoteReservationStartRequest.ProviderId,
                                                             AuthorizeRemoteReservationStartRequest.EVSEId,
                                                             AuthorizeRemoteReservationStartRequest.Identification,
                                                             AuthorizeRemoteReservationStartRequest.SessionId,
                                                             AuthorizeRemoteReservationStartRequest.PartnerSessionId,
                                                             AuthorizeRemoteReservationStartRequest.PartnerProductId,
                                                             AuthorizeRemoteReservationStartRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             Acknowledgement,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeRemoteReservationStartResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML(CustomAuthorizeRemoteReservationStartResponseSerializer,
                                                                               CustomStatusCodeSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnAuthorizeRemoteReservationStartSOAPResponse event

                try
                {

                    if (OnAuthorizeRemoteReservationStartSOAPResponse != null)
                        await Task.WhenAll(OnAuthorizeRemoteReservationStartSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeRemoteReservationStartSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /Reservation        - AuthorizeRemoteReservationStop

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + ReservationURI,
                                            "AuthorizeRemoteReservationStop",
                                            XML => XML.Descendants(OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStop").FirstOrDefault(),
                                            async (HTTPRequest, PullEvseDataXML) => {


                EMP.AuthorizeRemoteReservationStopRequest                   AuthorizeRemoteReservationStopRequest  = null;
                Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>  Acknowledgement                         = null;

                #region Send OnAuthorizeRemoteReservationStopSOAPRequest event

                var StopTime = DateTime.Now;

                try
                {

                    if (OnAuthorizeRemoteReservationStopSOAPRequest != null)
                        await Task.WhenAll(OnAuthorizeRemoteReservationStopSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StopTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeRemoteReservationStopSOAPRequest));
                }

                #endregion


                if (EMP.AuthorizeRemoteReservationStopRequest.TryParse(PullEvseDataXML,
                                                                       out AuthorizeRemoteReservationStopRequest,
                                                                       CustomAuthorizeRemoteReservationStopRequestParser,
                                                                       OnException,

                                                                       HTTPRequest.Timestamp,
                                                                       HTTPRequest.CancellationToken,
                                                                       HTTPRequest.EventTrackingId,
                                                                       HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnAuthorizeRemoteReservationStopRequest event

                    try
                    {

                        if (OnAuthorizeRemoteReservationStopRequest != null)
                            await Task.WhenAll(OnAuthorizeRemoteReservationStopRequest.GetInvocationList().
                                               Cast<OnAuthorizeRemoteReservationStopRequestDelegate>().
                                               Select(e => e(StopTime,
                                                             AuthorizeRemoteReservationStopRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             AuthorizeRemoteReservationStopRequest.EventTrackingId,
                                                             AuthorizeRemoteReservationStopRequest.SessionId,
                                                             AuthorizeRemoteReservationStopRequest.ProviderId,
                                                             AuthorizeRemoteReservationStopRequest.EVSEId,
                                                             AuthorizeRemoteReservationStopRequest.PartnerSessionId,
                                                             AuthorizeRemoteReservationStopRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeRemoteReservationStopRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnAuthorizeRemoteReservationStop != null)
                    {

                        var results = await Task.WhenAll(OnAuthorizeRemoteReservationStop.GetInvocationList().
                                                             Cast<OnAuthorizeRemoteReservationStopDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           AuthorizeRemoteReservationStopRequest))).
                                                             ConfigureAwait(false);

                        Acknowledgement = results.FirstOrDefault();

                    }

                    //if (EVSEData == null)
                    //    EVSEData = EVSEData.SystemError(
                    //                         AuthorizeRemoteReservationStopRequest,
                    //                         "Could not process the incoming AuthorizeRemoteReservationStop request!",
                    //                         null,
                    //                         AuthorizeRemoteReservationStopRequest.SessionId,
                    //                         AuthorizeRemoteReservationStopRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnAuthorizeRemoteReservationStopResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnAuthorizeRemoteReservationStopResponse != null)
                            await Task.WhenAll(OnAuthorizeRemoteReservationStopResponse.GetInvocationList().
                                               Cast<OnAuthorizeRemoteReservationStopResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             AuthorizeRemoteReservationStopRequest.EventTrackingId,
                                                             AuthorizeRemoteReservationStopRequest.SessionId,
                                                             AuthorizeRemoteReservationStopRequest.ProviderId,
                                                             AuthorizeRemoteReservationStopRequest.EVSEId,
                                                             AuthorizeRemoteReservationStopRequest.PartnerSessionId,
                                                             AuthorizeRemoteReservationStopRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             Acknowledgement,
                                                             EndTime - StopTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeRemoteReservationStopResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML(CustomAuthorizeRemoteReservationStopResponseSerializer,
                                                                               CustomStatusCodeSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnAuthorizeRemoteReservationStopSOAPResponse event

                try
                {

                    if (OnAuthorizeRemoteReservationStopSOAPResponse != null)
                        await Task.WhenAll(OnAuthorizeRemoteReservationStopSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeRemoteReservationStopSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /Authorization      - AuthorizeRemoteStart

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + AuthorizationURI,
                                            "AuthorizeRemoteStart",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeRemoteStart").FirstOrDefault(),
                                            async (HTTPRequest, PullEvseDataXML) => {


                EMP.AuthorizeRemoteStartRequest                   AuthorizeRemoteStartRequest  = null;
                Acknowledgement<EMP.AuthorizeRemoteStartRequest>  Acknowledgement              = null;

                #region Send OnAuthorizeRemoteStartSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    if (OnAuthorizeRemoteStartSOAPRequest != null)
                        await Task.WhenAll(OnAuthorizeRemoteStartSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeRemoteStartSOAPRequest));
                }

                #endregion


                if (EMP.AuthorizeRemoteStartRequest.TryParse(PullEvseDataXML,
                                                             out AuthorizeRemoteStartRequest,
                                                             CustomAuthorizeRemoteStartRequestParser,
                                                             CustomIdentificationParser,
                                                             OnException,

                                                             HTTPRequest.Timestamp,
                                                             HTTPRequest.CancellationToken,
                                                             HTTPRequest.EventTrackingId,
                                                             HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnAuthorizeRemoteStartRequest event

                    try
                    {

                        if (OnAuthorizeRemoteStartRequest != null)
                            await Task.WhenAll(OnAuthorizeRemoteStartRequest.GetInvocationList().
                                               Cast<OnAuthorizeRemoteStartRequestDelegate>().
                                               Select(e => e(StartTime,
                                                             AuthorizeRemoteStartRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             AuthorizeRemoteStartRequest.EventTrackingId,
                                                             AuthorizeRemoteStartRequest.ProviderId,
                                                             AuthorizeRemoteStartRequest.EVSEId,
                                                             AuthorizeRemoteStartRequest.EVCOId,
                                                             AuthorizeRemoteStartRequest.SessionId,
                                                             AuthorizeRemoteStartRequest.PartnerSessionId,
                                                             AuthorizeRemoteStartRequest.PartnerProductId,
                                                             AuthorizeRemoteStartRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeRemoteStartRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnAuthorizeRemoteStart != null)
                    {

                        var results = await Task.WhenAll(OnAuthorizeRemoteStart.GetInvocationList().
                                                             Cast<OnAuthorizeRemoteStartDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           AuthorizeRemoteStartRequest))).
                                                             ConfigureAwait(false);

                        Acknowledgement = results.FirstOrDefault();

                    }

                    //if (EVSEData == null)
                    //    EVSEData = EVSEData.SystemError(
                    //                         AuthorizeRemoteStartRequest,
                    //                         "Could not process the incoming AuthorizeRemoteStart request!",
                    //                         null,
                    //                         AuthorizeRemoteStartRequest.SessionId,
                    //                         AuthorizeRemoteStartRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnAuthorizeRemoteStartResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnAuthorizeRemoteStartResponse != null)
                            await Task.WhenAll(OnAuthorizeRemoteStartResponse.GetInvocationList().
                                               Cast<OnAuthorizeRemoteStartResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             AuthorizeRemoteStartRequest.EventTrackingId,
                                                             AuthorizeRemoteStartRequest.ProviderId,
                                                             AuthorizeRemoteStartRequest.EVSEId,
                                                             AuthorizeRemoteStartRequest.EVCOId,
                                                             AuthorizeRemoteStartRequest.SessionId,
                                                             AuthorizeRemoteStartRequest.PartnerSessionId,
                                                             AuthorizeRemoteStartRequest.PartnerProductId,
                                                             AuthorizeRemoteStartRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             Acknowledgement,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeRemoteStartResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML(CustomAuthorizeRemoteStartResponseSerializer,
                                                                               CustomStatusCodeSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnAuthorizeRemoteStartSOAPResponse event

                try
                {

                    if (OnAuthorizeRemoteStartSOAPResponse != null)
                        await Task.WhenAll(OnAuthorizeRemoteStartSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeRemoteStartSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /Authorization      - AuthorizeRemoteStop

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + AuthorizationURI,
                                            "AuthorizeRemoteStop",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeRemoteStop").FirstOrDefault(),
                                            async (HTTPRequest, PullEvseDataXML) => {


                EMP.AuthorizeRemoteStopRequest                   AuthorizeRemoteStopRequest  = null;
                Acknowledgement<EMP.AuthorizeRemoteStopRequest>  Acknowledgement             = null;

                #region Send OnAuthorizeRemoteStopSOAPRequest event

                var StopTime = DateTime.Now;

                try
                {

                    if (OnAuthorizeRemoteStopSOAPRequest != null)
                        await Task.WhenAll(OnAuthorizeRemoteStopSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StopTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeRemoteStopSOAPRequest));
                }

                #endregion


                if (EMP.AuthorizeRemoteStopRequest.TryParse(PullEvseDataXML,
                                                             out AuthorizeRemoteStopRequest,
                                                             CustomAuthorizeRemoteStopRequestParser,
                                                             OnException,

                                                             HTTPRequest.Timestamp,
                                                             HTTPRequest.CancellationToken,
                                                             HTTPRequest.EventTrackingId,
                                                             HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnAuthorizeRemoteStopRequest event

                    try
                    {

                        if (OnAuthorizeRemoteStopRequest != null)
                            await Task.WhenAll(OnAuthorizeRemoteStopRequest.GetInvocationList().
                                               Cast<OnAuthorizeRemoteStopRequestDelegate>().
                                               Select(e => e(StopTime,
                                                             AuthorizeRemoteStopRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             AuthorizeRemoteStopRequest.EventTrackingId,
                                                             AuthorizeRemoteStopRequest.SessionId,
                                                             AuthorizeRemoteStopRequest.ProviderId,
                                                             AuthorizeRemoteStopRequest.EVSEId,
                                                             AuthorizeRemoteStopRequest.PartnerSessionId,
                                                             AuthorizeRemoteStopRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeRemoteStopRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnAuthorizeRemoteStop != null)
                    {

                        var results = await Task.WhenAll(OnAuthorizeRemoteStop.GetInvocationList().
                                                             Cast<OnAuthorizeRemoteStopDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           AuthorizeRemoteStopRequest))).
                                                             ConfigureAwait(false);

                        Acknowledgement = results.FirstOrDefault();

                    }

                    //if (EVSEData == null)
                    //    EVSEData = EVSEData.SystemError(
                    //                         AuthorizeRemoteStopRequest,
                    //                         "Could not process the incoming AuthorizeRemoteStop request!",
                    //                         null,
                    //                         AuthorizeRemoteStopRequest.SessionId,
                    //                         AuthorizeRemoteStopRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnAuthorizeRemoteStopResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnAuthorizeRemoteStopResponse != null)
                            await Task.WhenAll(OnAuthorizeRemoteStopResponse.GetInvocationList().
                                               Cast<OnAuthorizeRemoteStopResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             AuthorizeRemoteStopRequest.EventTrackingId,
                                                             AuthorizeRemoteStopRequest.SessionId,
                                                             AuthorizeRemoteStopRequest.ProviderId,
                                                             AuthorizeRemoteStopRequest.EVSEId,
                                                             AuthorizeRemoteStopRequest.PartnerSessionId,
                                                             AuthorizeRemoteStopRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             Acknowledgement,
                                                             EndTime - StopTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeRemoteStopResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML(CustomAuthorizeRemoteStopResponseSerializer,
                                                                               CustomStatusCodeSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnAuthorizeRemoteStopSOAPResponse event

                try
                {

                    if (OnAuthorizeRemoteStopSOAPResponse != null)
                        await Task.WhenAll(OnAuthorizeRemoteStopSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeRemoteStopSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion


            #region /Authorization      - GetChargeDetailRecords

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + AuthorizationURI,
                                            "GetChargeDetailRecords",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingGetChargeDetailRecords").FirstOrDefault(),
                                            async (HTTPRequest, PullEvseDataXML) => {


                EMP.GetChargeDetailRecordsRequest   GetChargeDetailRecordsRequest  = null;
                EMP.GetChargeDetailRecordsResponse  Acknowledgement                = null;

                #region Send OnGetChargeDetailRecordsSOAPRequest event

                var StopTime = DateTime.Now;

                try
                {

                    if (OnGetChargeDetailRecordsSOAPRequest != null)
                        await Task.WhenAll(OnGetChargeDetailRecordsSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StopTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnGetChargeDetailRecordsSOAPRequest));
                }

                #endregion


                if (EMP.GetChargeDetailRecordsRequest.TryParse(PullEvseDataXML,
                                                               out GetChargeDetailRecordsRequest,
                                                               CustomGetChargeDetailRecordsRequestParser,
                                                               OnException,

                                                               HTTPRequest.Timestamp,
                                                               HTTPRequest.CancellationToken,
                                                               HTTPRequest.EventTrackingId,
                                                               HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnGetChargeDetailRecordsRequest event

                    try
                    {

                        if (OnGetChargeDetailRecordsRequest != null)
                            await Task.WhenAll(OnGetChargeDetailRecordsRequest.GetInvocationList().
                                               Cast<OnGetChargeDetailRecordsRequestDelegate>().
                                               Select(e => e(StopTime,
                                                             GetChargeDetailRecordsRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             GetChargeDetailRecordsRequest.EventTrackingId,
                                                             GetChargeDetailRecordsRequest.ProviderId,
                                                             GetChargeDetailRecordsRequest.From,
                                                             GetChargeDetailRecordsRequest.To,
                                                             GetChargeDetailRecordsRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnGetChargeDetailRecordsRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnGetChargeDetailRecords != null)
                    {

                        var results = await Task.WhenAll(OnGetChargeDetailRecords.GetInvocationList().
                                                             Cast<OnGetChargeDetailRecordsDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           GetChargeDetailRecordsRequest))).
                                                             ConfigureAwait(false);

                        Acknowledgement = results.FirstOrDefault();

                    }

                    //if (EVSEData == null)
                    //    EVSEData = EVSEData.SystemError(
                    //                         GetChargeDetailRecordsRequest,
                    //                         "Could not process the incoming GetChargeDetailRecords request!",
                    //                         null,
                    //                         GetChargeDetailRecordsRequest.SessionId,
                    //                         GetChargeDetailRecordsRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnGetChargeDetailRecordsResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnGetChargeDetailRecordsResponse != null)
                            await Task.WhenAll(OnGetChargeDetailRecordsResponse.GetInvocationList().
                                               Cast<OnGetChargeDetailRecordsResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             GetChargeDetailRecordsRequest.EventTrackingId,
                                                             GetChargeDetailRecordsRequest.ProviderId,
                                                             GetChargeDetailRecordsRequest.From,
                                                             GetChargeDetailRecordsRequest.To,
                                                             GetChargeDetailRecordsRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             Acknowledgement,
                                                             EndTime - StopTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnGetChargeDetailRecordsResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML(CustomGetChargeDetailRecordsResponseSerializer,
                                                                               CustomChargeDetailRecordSerializer,
                                                                               CustomIdentificationSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnGetChargeDetailRecordsSOAPResponse event

                try
                {

                    if (OnGetChargeDetailRecordsSOAPResponse != null)
                        await Task.WhenAll(OnGetChargeDetailRecordsSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnGetChargeDetailRecordsSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #endregion

            #region CPO methods

            #region /EVSEData           - PushEVSEData

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + EVSEDataURI,
                                            "PushEVSEData",
                                            XML => XML.Descendants(OICPNS.EVSEData + "eRoamingPushEvseData").FirstOrDefault(),
                                            async (HTTPRequest, PushEvseDataXML) => {


                CPO.PushEVSEDataRequest                   PushEVSEDataRequest  = null;
                Acknowledgement<CPO.PushEVSEDataRequest>  Acknowledgement      = null;

                #region Send OnPushEVSEDataSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    if (OnPushEVSEDataSOAPRequest != null)
                        await Task.WhenAll(OnPushEVSEDataSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnPushEVSEDataSOAPRequest));
                }

                #endregion


                if (CPO.PushEVSEDataRequest.TryParse(PushEvseDataXML,
                                                     out PushEVSEDataRequest,
                                                     CustomOperatorEVSEDataParser,
                                                     CustomEVSEDataRecordParser,
                                                     OnException,

                                                     HTTPRequest.Timestamp,
                                                     HTTPRequest.CancellationToken,
                                                     HTTPRequest.EventTrackingId,
                                                     HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnPushEVSEDataRequest event

                    try
                    {

                        if (OnPushEVSEDataRequest != null)
                            await Task.WhenAll(OnPushEVSEDataRequest.GetInvocationList().
                                               Cast<OnPushEVSEDataRequestDelegate>().
                                               Select(e => e(StartTime,
                                                             PushEVSEDataRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             PushEVSEDataRequest.EventTrackingId,
                                                             PushEVSEDataRequest.OperatorEVSEData,
                                                             PushEVSEDataRequest.Action,
                                                             PushEVSEDataRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnPushEVSEDataRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnPushEVSEData != null)
                    {

                        var results = await Task.WhenAll(OnPushEVSEData.GetInvocationList().
                                                             Cast<OnPushEVSEDataDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           PushEVSEDataRequest))).
                                                             ConfigureAwait(false);

                        Acknowledgement = results.FirstOrDefault();

                    }

                    //if (EVSEData == null)
                    //    EVSEData = EVSEData.SystemError(
                    //                         PushEVSEDataRequest,
                    //                         "Could not process the incoming AuthorizeRemoteReservationStart request!",
                    //                         null,
                    //                         PushEVSEDataRequest.SessionId,
                    //                         PushEVSEDataRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnPushEVSEDataResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnPushEVSEDataResponse != null)
                            await Task.WhenAll(OnPushEVSEDataResponse.GetInvocationList().
                                               Cast<OnPushEVSEDataResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             PushEVSEDataRequest.EventTrackingId,
                                                             PushEVSEDataRequest.OperatorEVSEData,
                                                             PushEVSEDataRequest.Action,
                                                             PushEVSEDataRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             Acknowledgement,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnPushEVSEDataResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML(CustomAcknowledgementSerializer:  CustomPushEVSEDataResponseSerializer,
                                                                               CustomStatusCodeSerializer:       CustomStatusCodeSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnPushEVSEDataSOAPResponse event

                try
                {

                    if (OnPushEVSEDataSOAPResponse != null)
                        await Task.WhenAll(OnPushEVSEDataSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnPushEVSEDataSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /EVSEStatus         - PushEVSEStatus

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + EVSEStatusURI,
                                            "PushEVSEStatus",
                                            XML => XML.Descendants(OICPNS.EVSEStatus + "eRoamingPushEvseStatus").FirstOrDefault(),
                                            async (HTTPRequest, PushEvseStatusXML) => {


                CPO.PushEVSEStatusRequest                   PushEVSEStatusRequest  = null;
                Acknowledgement<CPO.PushEVSEStatusRequest>  Acknowledgement        = null;

                #region Send OnPushEVSEStatusSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    if (OnPushEVSEStatusSOAPRequest != null)
                        await Task.WhenAll(OnPushEVSEStatusSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnPushEVSEStatusSOAPRequest));
                }

                #endregion


                if (CPO.PushEVSEStatusRequest.TryParse(PushEvseStatusXML,
                                                       out PushEVSEStatusRequest,
                                                       CustomOperatorEVSEStatusParser,
                                                       CustomEVSEStatusRecordParser,
                                                       OnException,

                                                       HTTPRequest.Timestamp,
                                                       HTTPRequest.CancellationToken,
                                                       HTTPRequest.EventTrackingId,
                                                       HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnPushEVSEStatusRequest event

                    try
                    {

                        if (OnPushEVSEStatusRequest != null)
                            await Task.WhenAll(OnPushEVSEStatusRequest.GetInvocationList().
                                               Cast<OnPushEVSEStatusRequestDelegate>().
                                               Select(e => e(StartTime,
                                                             PushEVSEStatusRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             PushEVSEStatusRequest.EventTrackingId,
                                                             PushEVSEStatusRequest.OperatorEVSEStatus,
                                                             PushEVSEStatusRequest.Action,
                                                             PushEVSEStatusRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnPushEVSEStatusRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnPushEVSEStatus != null)
                    {

                        var results = await Task.WhenAll(OnPushEVSEStatus.GetInvocationList().
                                                             Cast<OnPushEVSEStatusDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           PushEVSEStatusRequest))).
                                                             ConfigureAwait(false);

                        Acknowledgement = results.FirstOrDefault();

                    }

                    //if (EVSEStatus == null)
                    //    EVSEStatus = EVSEStatus.SystemError(
                    //                         PushEVSEStatusRequest,
                    //                         "Could not process the incoming AuthorizeRemoteReservationStart request!",
                    //                         null,
                    //                         PushEVSEStatusRequest.SessionId,
                    //                         PushEVSEStatusRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnPushEVSEStatusResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnPushEVSEStatusResponse != null)
                            await Task.WhenAll(OnPushEVSEStatusResponse.GetInvocationList().
                                               Cast<OnPushEVSEStatusResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             PushEVSEStatusRequest.EventTrackingId,
                                                             PushEVSEStatusRequest.OperatorEVSEStatus,
                                                             PushEVSEStatusRequest.Action,
                                                             PushEVSEStatusRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             Acknowledgement,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnPushEVSEStatusResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML(CustomAcknowledgementSerializer:  CustomPushEVSEStatusResponseSerializer,
                                                                               CustomStatusCodeSerializer:       CustomStatusCodeSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnPushEVSEStatusSOAPResponse event

                try
                {

                    if (OnPushEVSEStatusSOAPResponse != null)
                        await Task.WhenAll(OnPushEVSEStatusSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnPushEVSEStatusSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion


            #region /Authorization      - AuthorizeStart

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + AuthorizationURI,
                                            "AuthorizeStart",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeStart").FirstOrDefault(),
                                            async (HTTPRequest, PushEvseStatusXML) => {


                CPO.AuthorizeStartRequest  AuthorizeStartRequest  = null;
                CPO.AuthorizationStart     AuthorizationStart     = null;

                #region Send OnAuthorizeStartSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    if (OnAuthorizeStartSOAPRequest != null)
                        await Task.WhenAll(OnAuthorizeStartSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeStartSOAPRequest));
                }

                #endregion


                if (CPO.AuthorizeStartRequest.TryParse(PushEvseStatusXML,
                                                       out AuthorizeStartRequest,
                                                       CustomAuthorizeStartRequestParser,
                                                       CustomIdentificationParser,
                                                       OnException,

                                                       HTTPRequest.Timestamp,
                                                       HTTPRequest.CancellationToken,
                                                       HTTPRequest.EventTrackingId,
                                                       HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnAuthorizeStartRequest event

                    try
                    {

                        if (OnAuthorizeStartRequest != null)
                            await Task.WhenAll(OnAuthorizeStartRequest.GetInvocationList().
                                               Cast<OnAuthorizeStartRequestDelegate>().
                                               Select(e => e(StartTime,
                                                             AuthorizeStartRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             AuthorizeStartRequest.EventTrackingId,
                                                             AuthorizeStartRequest.OperatorId,
                                                             AuthorizeStartRequest.Identification,
                                                             AuthorizeStartRequest.EVSEId,
                                                             AuthorizeStartRequest.PartnerProductId,
                                                             AuthorizeStartRequest.SessionId,
                                                             AuthorizeStartRequest.PartnerSessionId,
                                                             AuthorizeStartRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeStartRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnAuthorizeStart != null)
                    {

                        var results = await Task.WhenAll(OnAuthorizeStart.GetInvocationList().
                                                             Cast<OnAuthorizeStartDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           AuthorizeStartRequest))).
                                                             ConfigureAwait(false);

                        AuthorizationStart = results.FirstOrDefault();

                    }

                    //if (EVSEStatus == null)
                    //    EVSEStatus = EVSEStatus.SystemError(
                    //                         AuthorizeStartRequest,
                    //                         "Could not process the incoming AuthorizeRemoteReservationStart request!",
                    //                         null,
                    //                         AuthorizeStartRequest.SessionId,
                    //                         AuthorizeStartRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnAuthorizeStartResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnAuthorizeStartResponse != null)
                            await Task.WhenAll(OnAuthorizeStartResponse.GetInvocationList().
                                               Cast<OnAuthorizeStartResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             AuthorizeStartRequest.EventTrackingId,
                                                             AuthorizeStartRequest.OperatorId,
                                                             AuthorizeStartRequest.Identification,
                                                             AuthorizeStartRequest.EVSEId,
                                                             AuthorizeStartRequest.PartnerProductId,
                                                             AuthorizeStartRequest.SessionId,
                                                             AuthorizeStartRequest.PartnerSessionId,
                                                             AuthorizeStartRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             AuthorizationStart,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeStartResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(AuthorizationStart.ToXML(CustomAuthorizationStartSerializer,
                                                                                  CustomStatusCodeSerializer,
                                                                                  CustomIdentificationSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnAuthorizeStartSOAPResponse event

                try
                {

                    if (OnAuthorizeStartSOAPResponse != null)
                        await Task.WhenAll(OnAuthorizeStartSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeStartSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /Authorization      - AuthorizeStop

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + AuthorizationURI,
                                            "AuthorizeStop",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeStop").FirstOrDefault(),
                                            async (HTTPRequest, PushEvseStatusXML) => {


                CPO.AuthorizeStopRequest  AuthorizeStopRequest  = null;
                CPO.AuthorizationStop     AuthorizationStop     = null;

                #region Send OnAuthorizeStopSOAPRequest event

                var StopTime = DateTime.Now;

                try
                {

                    if (OnAuthorizeStopSOAPRequest != null)
                        await Task.WhenAll(OnAuthorizeStopSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StopTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeStopSOAPRequest));
                }

                #endregion


                if (CPO.AuthorizeStopRequest.TryParse(PushEvseStatusXML,
                                                      out AuthorizeStopRequest,
                                                      CustomAuthorizeStopRequestParser,
                                                      CustomIdentificationParser,
                                                      OnException,

                                                      HTTPRequest.Timestamp,
                                                      HTTPRequest.CancellationToken,
                                                      HTTPRequest.EventTrackingId,
                                                      HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnAuthorizeStopRequest event

                    try
                    {

                        if (OnAuthorizeStopRequest != null)
                            await Task.WhenAll(OnAuthorizeStopRequest.GetInvocationList().
                                               Cast<OnAuthorizeStopRequestDelegate>().
                                               Select(e => e(StopTime,
                                                             AuthorizeStopRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             AuthorizeStopRequest.EventTrackingId,
                                                             AuthorizeStopRequest.OperatorId,
                                                             AuthorizeStopRequest.SessionId,
                                                             AuthorizeStopRequest.Identification,
                                                             AuthorizeStopRequest.EVSEId,
                                                             AuthorizeStopRequest.PartnerSessionId,
                                                             AuthorizeStopRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeStopRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnAuthorizeStop != null)
                    {

                        var results = await Task.WhenAll(OnAuthorizeStop.GetInvocationList().
                                                             Cast<OnAuthorizeStopDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           AuthorizeStopRequest))).
                                                             ConfigureAwait(false);

                        AuthorizationStop = results.FirstOrDefault();

                    }

                    //if (EVSEStatus == null)
                    //    EVSEStatus = EVSEStatus.SystemError(
                    //                         AuthorizeStopRequest,
                    //                         "Could not process the incoming AuthorizeRemoteReservationStop request!",
                    //                         null,
                    //                         AuthorizeStopRequest.SessionId,
                    //                         AuthorizeStopRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnAuthorizeStopResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnAuthorizeStopResponse != null)
                            await Task.WhenAll(OnAuthorizeStopResponse.GetInvocationList().
                                               Cast<OnAuthorizeStopResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             AuthorizeStopRequest.EventTrackingId,
                                                             AuthorizeStopRequest.OperatorId,
                                                             AuthorizeStopRequest.SessionId,
                                                             AuthorizeStopRequest.Identification,
                                                             AuthorizeStopRequest.EVSEId,
                                                             AuthorizeStopRequest.PartnerSessionId,
                                                             AuthorizeStopRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             AuthorizationStop,
                                                             EndTime - StopTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeStopResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(AuthorizationStop.ToXML(CustomAuthorizationStopSerializer,
                                                                                 CustomStatusCodeSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnAuthorizeStopSOAPResponse event

                try
                {

                    if (OnAuthorizeStopSOAPResponse != null)
                        await Task.WhenAll(OnAuthorizeStopSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnAuthorizeStopSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /Authorization      - SendChargeDetailRecord

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + AuthorizationURI,
                                            "SendChargeDetailRecord",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingChargeDetailRecord").FirstOrDefault(),
                                            async (HTTPRequest, PushEvseStatusXML) => {


                CPO.SendChargeDetailRecordRequest                   SendChargeDetailRecordRequest  = null;
                Acknowledgement<CPO.SendChargeDetailRecordRequest>  Acknowledgement                = null;

                #region Send OnSendChargeDetailRecordSOAPRequest event

                var StopTime = DateTime.Now;

                try
                {

                    if (OnSendChargeDetailRecordSOAPRequest != null)
                        await Task.WhenAll(OnSendChargeDetailRecordSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StopTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnSendChargeDetailRecordSOAPRequest));
                }

                #endregion


                if (CPO.SendChargeDetailRecordRequest.TryParse(PushEvseStatusXML,
                                                               out SendChargeDetailRecordRequest,
                                                               CustomChargeDetailRecordParser,
                                                               CustomIdentificationParser,
                                                               OnException,

                                                               HTTPRequest.Timestamp,
                                                               HTTPRequest.CancellationToken,
                                                               HTTPRequest.EventTrackingId,
                                                               HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnSendChargeDetailRecordRequest event

                    try
                    {

                        if (OnSendChargeDetailRecordRequest != null)
                            await Task.WhenAll(OnSendChargeDetailRecordRequest.GetInvocationList().
                                               Cast<OnSendChargeDetailRecordRequestDelegate>().
                                               Select(e => e(StopTime,
                                                             SendChargeDetailRecordRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             SendChargeDetailRecordRequest.EventTrackingId,
                                                             SendChargeDetailRecordRequest.ChargeDetailRecord,
                                                             SendChargeDetailRecordRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnSendChargeDetailRecordRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnSendChargeDetailRecord != null)
                    {

                        var results = await Task.WhenAll(OnSendChargeDetailRecord.GetInvocationList().
                                                             Cast<OnSendChargeDetailRecordDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           SendChargeDetailRecordRequest))).
                                                             ConfigureAwait(false);

                        Acknowledgement = results.FirstOrDefault();

                    }

                    //if (EVSEStatus == null)
                    //    EVSEStatus = EVSEStatus.SystemError(
                    //                         SendChargeDetailRecordRequest,
                    //                         "Could not process the incoming AuthorizeRemoteReservationStop request!",
                    //                         null,
                    //                         SendChargeDetailRecordRequest.SessionId,
                    //                         SendChargeDetailRecordRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnSendChargeDetailRecordResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnSendChargeDetailRecordResponse != null)
                            await Task.WhenAll(OnSendChargeDetailRecordResponse.GetInvocationList().
                                               Cast<OnSendChargeDetailRecordResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             SendChargeDetailRecordRequest.EventTrackingId,
                                                             SendChargeDetailRecordRequest.ChargeDetailRecord,
                                                             SendChargeDetailRecordRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             Acknowledgement,
                                                             EndTime - StopTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnSendChargeDetailRecordResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML(CustomSendChargeDetailRecordResponseSerializer,
                                                                               CustomStatusCodeSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnSendChargeDetailRecordSOAPResponse event

                try
                {

                    if (OnSendChargeDetailRecordSOAPResponse != null)
                        await Task.WhenAll(OnSendChargeDetailRecordSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnSendChargeDetailRecordSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion


            #region /AuthenticationData - PullAuthenticationData

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + AuthenticationDataURI,
                                            "PullAuthenticationData",
                                            XML => XML.Descendants(OICPNS.AuthenticationData + "eRoamingPullAuthenticationData").FirstOrDefault(),
                                            async (HTTPRequest, PullAuthenticationDataXML) => {


                CPO.PullAuthenticationDataRequest  PullAuthenticationDataRequest  = null;
                CPO.AuthenticationData             AuthenticationData             = null;

                #region Send OnPullAuthenticationDataSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    if (OnPullAuthenticationDataSOAPRequest != null)
                        await Task.WhenAll(OnPullAuthenticationDataSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnPullAuthenticationDataSOAPRequest));
                }

                #endregion


                if (CPO.PullAuthenticationDataRequest.TryParse(PullAuthenticationDataXML,
                                                               out PullAuthenticationDataRequest,
                                                               CustomPullAuthenticationDataRequestParser,
                                                               OnException,

                                                               HTTPRequest.Timestamp,
                                                               HTTPRequest.CancellationToken,
                                                               HTTPRequest.EventTrackingId,
                                                               HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnPullAuthenticationDataRequest event

                    try
                    {

                        if (OnPullAuthenticationDataRequest != null)
                            await Task.WhenAll(OnPullAuthenticationDataRequest.GetInvocationList().
                                               Cast<OnPullAuthenticationDataRequestDelegate>().
                                               Select(e => e(StartTime,
                                                             PullAuthenticationDataRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             PullAuthenticationDataRequest.EventTrackingId,
                                                             PullAuthenticationDataRequest.OperatorId,
                                                             PullAuthenticationDataRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnPullAuthenticationDataRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnPullAuthenticationData != null)
                    {

                        var results = await Task.WhenAll(OnPullAuthenticationData.GetInvocationList().
                                                             Cast<OnPullAuthenticationDataDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           PullAuthenticationDataRequest))).
                                                             ConfigureAwait(false);

                        AuthenticationData = results.FirstOrDefault();

                    }

                    //if (EVSEData == null)
                    //    EVSEData = EVSEData.SystemError(
                    //                         PullAuthenticationDataRequest,
                    //                         "Could not process the incoming AuthorizeRemoteReservationStart request!",
                    //                         null,
                    //                         PullAuthenticationDataRequest.SessionId,
                    //                         PullAuthenticationDataRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnPullAuthenticationDataResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnPullAuthenticationDataResponse != null)
                            await Task.WhenAll(OnPullAuthenticationDataResponse.GetInvocationList().
                                               Cast<OnPullAuthenticationDataResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             PullAuthenticationDataRequest.EventTrackingId,
                                                             PullAuthenticationDataRequest.OperatorId,
                                                             PullAuthenticationDataRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             AuthenticationData,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnPullAuthenticationDataResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(AuthenticationData.ToXML(CustomAuthenticationDataSerializer,
                                                                                  CustomProviderAuthenticationDataSerializer,
                                                                                  CustomIdentificationSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnPullAuthenticationDataSOAPResponse event

                try
                {

                    if (OnPullAuthenticationDataSOAPResponse != null)
                        await Task.WhenAll(OnPullAuthenticationDataSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnPullAuthenticationDataSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #endregion

            #region Mobile methods

            #region /MobileAuthorization - MobileAuthorizeStart

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + MobileAuthorizationURI,
                                            "MobileAuthorizeStart",
                                            XML => XML.Descendants(OICPNS.MobileAuthorization + "eRoamingMobileAuthorizeStart").FirstOrDefault(),
                                            async (HTTPRequest, PushEvseDataXML) => {


                Mobile.MobileAuthorizeStartRequest  MobileAuthorizeStartRequest  = null;
                Mobile.MobileAuthorizationStart     MobileAuthorizationStart     = null;

                #region Send OnMobileAuthorizeStartSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    if (OnMobileAuthorizeStartSOAPRequest != null)
                        await Task.WhenAll(OnMobileAuthorizeStartSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnMobileAuthorizeStartSOAPRequest));
                }

                #endregion


                if (Mobile.MobileAuthorizeStartRequest.TryParse(PushEvseDataXML,
                                                                out MobileAuthorizeStartRequest,
                                                                CustomMobileAuthorizeStartRequestParser,
                                                                OnException,

                                                                HTTPRequest.Timestamp,
                                                                HTTPRequest.CancellationToken,
                                                                HTTPRequest.EventTrackingId,
                                                                HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnMobileAuthorizeStartRequest event

                    try
                    {

                        if (OnMobileAuthorizeStartRequest != null)
                            await Task.WhenAll(OnMobileAuthorizeStartRequest.GetInvocationList().
                                               Cast<OnMobileAuthorizeStartRequestDelegate>().
                                               Select(e => e(StartTime,
                                                             MobileAuthorizeStartRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             MobileAuthorizeStartRequest.EventTrackingId,
                                                             MobileAuthorizeStartRequest.EVSEId,
                                                             MobileAuthorizeStartRequest.QRCodeIdentification,
                                                             MobileAuthorizeStartRequest.PartnerProductId,
                                                             MobileAuthorizeStartRequest.GetNewSession,
                                                             MobileAuthorizeStartRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnMobileAuthorizeStartRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnMobileAuthorizeStart != null)
                    {

                        var results = await Task.WhenAll(OnMobileAuthorizeStart.GetInvocationList().
                                                             Cast<OnMobileAuthorizeStartDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           MobileAuthorizeStartRequest))).
                                                             ConfigureAwait(false);

                        MobileAuthorizationStart = results.FirstOrDefault();

                    }

                    //if (EVSEData == null)
                    //    EVSEData = EVSEData.SystemError(
                    //                         MobileAuthorizeStartRequest,
                    //                         "Could not process the incoming AuthorizeRemoteReservationStart request!",
                    //                         null,
                    //                         MobileAuthorizeStartRequest.SessionId,
                    //                         MobileAuthorizeStartRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnMobileAuthorizeStartResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnMobileAuthorizeStartResponse != null)
                            await Task.WhenAll(OnMobileAuthorizeStartResponse.GetInvocationList().
                                               Cast<OnMobileAuthorizeStartResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             MobileAuthorizeStartRequest.EventTrackingId,
                                                             MobileAuthorizeStartRequest.EVSEId,
                                                             MobileAuthorizeStartRequest.QRCodeIdentification,
                                                             MobileAuthorizeStartRequest.PartnerProductId,
                                                             MobileAuthorizeStartRequest.GetNewSession,
                                                             MobileAuthorizeStartRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             MobileAuthorizationStart,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnMobileAuthorizeStartResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(MobileAuthorizationStart.ToXML(CustomMobileAuthorizationStartSerializer,
                                                                                        CustomStatusCodeSerializer,
                                                                                        CustomAddressSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnMobileAuthorizeStartSOAPResponse event

                try
                {

                    if (OnMobileAuthorizeStartSOAPResponse != null)
                        await Task.WhenAll(OnMobileAuthorizeStartSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnMobileAuthorizeStartSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /MobileAuthorization - MobileRemoteStart

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + MobileAuthorizationURI,
                                            "MobileRemoteStart",
                                            XML => XML.Descendants(OICPNS.MobileAuthorization + "eRoamingMobileRemoteStart").FirstOrDefault(),
                                            async (HTTPRequest, PushEvseDataXML) => {


                Mobile.MobileRemoteStartRequest                   MobileRemoteStartRequest  = null;
                Acknowledgement<Mobile.MobileRemoteStartRequest>  Acknowledgement           = null;

                #region Send OnMobileRemoteStartSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    if (OnMobileRemoteStartSOAPRequest != null)
                        await Task.WhenAll(OnMobileRemoteStartSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnMobileRemoteStartSOAPRequest));
                }

                #endregion


                if (Mobile.MobileRemoteStartRequest.TryParse(PushEvseDataXML,
                                                             out MobileRemoteStartRequest,
                                                             CustomMobileRemoteStartRequestParser,
                                                             OnException,

                                                             HTTPRequest.Timestamp,
                                                             HTTPRequest.CancellationToken,
                                                             HTTPRequest.EventTrackingId,
                                                             HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnMobileRemoteStartRequest event

                    try
                    {

                        if (OnMobileRemoteStartRequest != null)
                            await Task.WhenAll(OnMobileRemoteStartRequest.GetInvocationList().
                                               Cast<OnMobileRemoteStartRequestDelegate>().
                                               Select(e => e(StartTime,
                                                             MobileRemoteStartRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             MobileRemoteStartRequest.EventTrackingId,
                                                             MobileRemoteStartRequest.SessionId,
                                                             MobileRemoteStartRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnMobileRemoteStartRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnMobileRemoteStart != null)
                    {

                        var results = await Task.WhenAll(OnMobileRemoteStart.GetInvocationList().
                                                             Cast<OnMobileRemoteStartDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           MobileRemoteStartRequest))).
                                                             ConfigureAwait(false);

                        Acknowledgement = results.FirstOrDefault();

                    }

                    //if (EVSEData == null)
                    //    EVSEData = EVSEData.SystemError(
                    //                         MobileRemoteStartRequest,
                    //                         "Could not process the incoming RemoteRemoteReservationStart request!",
                    //                         null,
                    //                         MobileRemoteStartRequest.SessionId,
                    //                         MobileRemoteStartRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnMobileRemoteStartResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnMobileRemoteStartResponse != null)
                            await Task.WhenAll(OnMobileRemoteStartResponse.GetInvocationList().
                                               Cast<OnMobileRemoteStartResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             MobileRemoteStartRequest.EventTrackingId,
                                                             MobileRemoteStartRequest.SessionId,
                                                             MobileRemoteStartRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             Acknowledgement,
                                                             EndTime - StartTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnMobileRemoteStartResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML(CustomMobileRemoteStartResponseSerializer,
                                                                               CustomStatusCodeSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnMobileRemoteStartSOAPResponse event

                try
                {

                    if (OnMobileRemoteStartSOAPResponse != null)
                        await Task.WhenAll(OnMobileRemoteStartSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnMobileRemoteStartSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /MobileAuthorization - MobileRemoteStop

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + MobileAuthorizationURI,
                                            "MobileRemoteStop",
                                            XML => XML.Descendants(OICPNS.MobileAuthorization + "eRoamingMobileRemoteStop").FirstOrDefault(),
                                            async (HTTPRequest, PushEvseDataXML) => {


                Mobile.MobileRemoteStopRequest                   MobileRemoteStopRequest  = null;
                Acknowledgement<Mobile.MobileRemoteStopRequest>  Acknowledgement           = null;

                #region Send OnMobileRemoteStopSOAPRequest event

                var StopTime = DateTime.Now;

                try
                {

                    if (OnMobileRemoteStopSOAPRequest != null)
                        await Task.WhenAll(OnMobileRemoteStopSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StopTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnMobileRemoteStopSOAPRequest));
                }

                #endregion


                if (Mobile.MobileRemoteStopRequest.TryParse(PushEvseDataXML,
                                                            out MobileRemoteStopRequest,
                                                            CustomMobileRemoteStopRequestParser,
                                                            OnException,

                                                            HTTPRequest.Timestamp,
                                                            HTTPRequest.CancellationToken,
                                                            HTTPRequest.EventTrackingId,
                                                            HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnMobileRemoteStopRequest event

                    try
                    {

                        if (OnMobileRemoteStopRequest != null)
                            await Task.WhenAll(OnMobileRemoteStopRequest.GetInvocationList().
                                               Cast<OnMobileRemoteStopRequestDelegate>().
                                               Select(e => e(StopTime,
                                                             MobileRemoteStopRequest.Timestamp.Value,
                                                             this,
                                                             ServiceId,
                                                             MobileRemoteStopRequest.EventTrackingId,
                                                             MobileRemoteStopRequest.SessionId,
                                                             MobileRemoteStopRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnMobileRemoteStopRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (OnMobileRemoteStop != null)
                    {

                        var results = await Task.WhenAll(OnMobileRemoteStop.GetInvocationList().
                                                             Cast<OnMobileRemoteStopDelegate>().
                                                             Select(e => e(DateTime.Now,
                                                                           this,
                                                                           MobileRemoteStopRequest))).
                                                             ConfigureAwait(false);

                        Acknowledgement = results.FirstOrDefault();

                    }

                    //if (EVSEData == null)
                    //    EVSEData = EVSEData.SystemError(
                    //                         MobileRemoteStopRequest,
                    //                         "Could not process the incoming RemoteRemoteReservationStop request!",
                    //                         null,
                    //                         MobileRemoteStopRequest.SessionId,
                    //                         MobileRemoteStopRequest.PartnerSessionId
                    //                     );

                    #endregion

                    #region Send OnMobileRemoteStopResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        if (OnMobileRemoteStopResponse != null)
                            await Task.WhenAll(OnMobileRemoteStopResponse.GetInvocationList().
                                               Cast<OnMobileRemoteStopResponseDelegate>().
                                               Select(e => e(EndTime,
                                                             this,
                                                             ServiceId,
                                                             MobileRemoteStopRequest.EventTrackingId,
                                                             MobileRemoteStopRequest.SessionId,
                                                             MobileRemoteStopRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                             Acknowledgement,
                                                             EndTime - StopTime))).
                                               ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CentralServer) + "." + nameof(OnMobileRemoteStopResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML(CustomMobileRemoteStopResponseSerializer,
                                                                               CustomStatusCodeSerializer)).ToUTF8Bytes()
                };

                #endregion

                #region Send OnMobileRemoteStopSOAPResponse event

                try
                {

                    if (OnMobileRemoteStopSOAPResponse != null)
                        await Task.WhenAll(OnMobileRemoteStopSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CentralServer) + "." + nameof(OnMobileRemoteStopSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #endregion

        }

        #endregion


    }

}
