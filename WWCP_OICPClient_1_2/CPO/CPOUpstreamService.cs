/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICPClient <https://github.com/WorldWideCharging/WWCP_OICPClient>
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
using System.Xml.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

using org.GraphDefined.WWCP.LocalService;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;

#endregion

namespace org.GraphDefined.WWCP.OICPClient_1_2
{

    /// <summary>
    /// OICPv1.2 CPO Upstream Service(s).
    /// </summary>
    public class CPOUpstreamService : AOICPUpstreamService,
                                      IAuthServices
    {

        #region Constructor(s)

        public CPOUpstreamService(String           OICPHost,
                                  IPPort           OICPPort,
                                  String           HTTPVirtualHost = null,
                                  Authorizator_Id  AuthorizatorId  = null,
                                  String           UserAgent       = "GraphDefined OICP Gateway",
                                  DNSClient        DNSClient       = null)

            : base(OICPHost,
                   OICPPort,
                   HTTPVirtualHost,
                   AuthorizatorId,
                   UserAgent,
                   DNSClient)

        { }

        #endregion


        #region Tokens

        public IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> AllTokens
        {
            get
            {
                return new KeyValuePair<Auth_Token, AuthorizationResult>[0];
            }
        }

        public IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> AuthorizedTokens
        {
            get
            {
                return new KeyValuePair<Auth_Token, AuthorizationResult>[0];
            }
        }

        public IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> NotAuthorizedTokens
        {
            get
            {
                return new KeyValuePair<Auth_Token, AuthorizationResult>[0];
            }
        }

        public IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> BlockedTokens
        {
            get
            {
                return new KeyValuePair<Auth_Token, AuthorizationResult>[0];
            }
        }

        #endregion


        #region EVSEDataFullload(EVSEOperator)

        public Task<HTTPResponse<HubjectAcknowledgement>> EVSEDataFullload(EVSEOperator  EVSEOperator)

        {

            Console.WriteLine("FullLoad of " + EVSEOperator.ChargingPools.
                                                            SelectMany(Pool    => Pool.ChargingStations).
                                                            SelectMany(Station => Station.EVSEs).
                                                            Where(EVSE => !EVSEOperator.InvalidEVSEIds.Contains(EVSE.Id)).
                                                            Count() + " EVSE static data sets at " + _HTTPVirtualHost + "...");

            try
            {

                var EVSEDataFullLoadXML = EVSEOperator.
                                              ChargingPools.
                                              PushEVSEDataXML(EVSEOperator.Id,
                                                              EVSEOperator.Name[Languages.de],
                                                              ActionType.fullLoad,
                                                              IncludeEVSEs: EVSEId => !EVSEOperator.InvalidEVSEIds.Contains(EVSEId));

                using (var _OICPClient = new SOAPClient(_Hostname,
                                                        _TCPPort,
                                                        _HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingEvseData_V1.2",
                                                        _UserAgent,
                                                        _DNSClient))
                    {

                        return _OICPClient.Query(EVSEDataFullLoadXML,
                                                   "eRoamingPushEvseData",
                                                   Timeout: TimeSpan.FromSeconds(60),

                                                   OnSuccess: XMLData =>
                                                   {

                                                       // <cmn:eRoamingAcknowledgement xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">
                                                       //   <cmn:Result>true</cmn:Result>
                                                       //   <cmn:StatusCode>
                                                       //     <cmn:Code>000</cmn:Code>
                                                       //     <cmn:Description>Success</cmn:Description>
                                                       //     <cmn:AdditionalInfo />
                                                       //   </cmn:StatusCode>
                                                       // </cmn:eRoamingAcknowledgement>

                                                       // <cmn:eRoamingAcknowledgement xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">
                                                       //   <cmn:Result>false</cmn:Result>
                                                       //   <cmn:StatusCode>
                                                       //     <cmn:Code>009</cmn:Code>
                                                       //     <cmn:Description>Data transaction error</cmn:Description>
                                                       //     <cmn:AdditionalInfo>The Push of data is already in progress.</cmn:AdditionalInfo>
                                                       //   </cmn:StatusCode>
                                                       // </cmn:eRoamingAcknowledgement>

                                                       var ack = HubjectAcknowledgement.Parse(XMLData.Content);
                                                       Console.WriteLine("EVSE data fullload: " + ack.Result + " / " + ack.Description + Environment.NewLine);

                                                       return new HTTPResponse<HubjectAcknowledgement>(XMLData.HttpResponse, ack, false);

                                                   },


                                                   OnSOAPFault: Fault =>
                                                   {

                                                       Console.WriteLine("EVSE data fullload lead to a fault!" + Environment.NewLine);

                                                       return new HTTPResponse<HubjectAcknowledgement>(
                                                           Fault.HttpResponse,
                                                           new HubjectAcknowledgement(false, 0, "", ""),
                                                           IsFault: true);

                                                   },

                                                   OnHTTPError: (t, s, e) => SendOnHTTPError(t, s, e),
    
                                                   OnException: (t, s, e) => SendOnException(t, s, e)

                                                  );

                    }

            } catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

            }

            return Task.FromResult<HTTPResponse<HubjectAcknowledgement>>(new HTTPResponse<HubjectAcknowledgement>(null, null, true));

        }

        #endregion

        #region EVSEStatusFullload(EVSEOperator)

        public Task<HTTPResponse<HubjectAcknowledgement>> EVSEStatusFullload(EVSEOperator EVSEOperator)

        {

            try
            {

                var XML_EVSEStates = EVSEOperator.
                                         AllEVSEStatus.
                                         Where(v => !EVSEOperator.InvalidEVSEIds.Contains(v.Key)).
                                         ToArray();

                if (XML_EVSEStates.Any())
                {

                    Console.WriteLine("FullLoad of " + XML_EVSEStates.Length + " EVSE states at " + _HTTPVirtualHost + "...");

                    var EVSEStatesInsertXML = XML_EVSEStates.
                                                  Select(v => new KeyValuePair<EVSE_Id, HubjectEVSEState>(v.Key, v.Value.AsHubjectEVSEState())).
                                                  PushEVSEStatusXML(EVSEOperator.Id,
                                                                    EVSEOperator.Name[Languages.de],
                                                                    ActionType.fullLoad);

                    using (var _OICPClient = new SOAPClient(_Hostname,
                                                            _TCPPort,
                                                            _HTTPVirtualHost,
                                                            "/ibis/ws/eRoamingEvseStatus_V1.2",
                                                            _UserAgent,
                                                            _DNSClient))
                    {

                        return _OICPClient.Query(EVSEStatesInsertXML,
                                                   "eRoamingPushEvseStatus",
                                                   Timeout: TimeSpan.FromSeconds(60),

                                                   OnSuccess: XMLData =>
                                                   {

                                                       // <cmn:eRoamingAcknowledgement xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">
                                                       //   <cmn:Result>true</cmn:Result>
                                                       //   <cmn:StatusCode>
                                                       //     <cmn:Code>000</cmn:Code>
                                                       //     <cmn:Description>Success</cmn:Description>
                                                       //     <cmn:AdditionalInfo />
                                                       //   </cmn:StatusCode>
                                                       // </cmn:eRoamingAcknowledgement>

                                                       var ack = HubjectAcknowledgement.Parse(XMLData.Content);
                                                       Console.WriteLine("EVSE states fullload: " + ack.Result + " / " + ack.Description + Environment.NewLine);

                                                       return new HTTPResponse<HubjectAcknowledgement>(XMLData.HttpResponse, ack, false);

                                                   },


                                                   OnSOAPFault: Fault =>
                                                   {

                                                       Console.WriteLine("EVSE states fullload lead to a fault!" + Environment.NewLine);

                                                       return new HTTPResponse<HubjectAcknowledgement>(
                                                           Fault.HttpResponse,
                                                           new HubjectAcknowledgement(false, 0, "", ""),
                                                           IsFault: true);

                                                   },

                                                   OnHTTPError: (t, s, e) => SendOnHTTPError(t, s, e),
    
                                                   OnException: (t, s, e) => SendOnException(t, s, e)

                                                  );

                    }

                    //using (var httpClient = new HTTPClient(Hostname, Port, DNSClient))
                    //{

                    //    var builder = httpClient.POST("/ibis/ws/eRoamingEvseStatus_V1.2");
                    //    builder.Host         = HTTPVirtualHost;
                    //    builder.Content      = EVSEStatesInsertXML.ToUTF8Bytes();
                    //    builder.ContentType  = HTTPContentType.XMLTEXT_UTF8;
                    //    builder.Set("SOAPAction", "eRoamingPushEvseStatus");
                    //    builder.UserAgent    = UserAgent;

                    //    var Task02 = httpClient.Execute(builder, (req, resp) => {
                    //        var ack = HubjectAcknowledgement.Parse(XDocument.Parse(resp.Content.ToUTF8String()).Root);
                    //        Console.WriteLine("EVSE states fullload: " + ack.Result + " / " + ack.Description + Environment.NewLine);
                    //    });

                    //    Task02.Wait(TimeSpan.FromSeconds(30));

                    //}

                }

            }
            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

            }

            return Task.FromResult<HTTPResponse<HubjectAcknowledgement>>(new HTTPResponse<HubjectAcknowledgement>(null, null, true));

        }

        #endregion

        #region SendEVSEStatusUpdates(...)

        public void SendEVSEStatusUpdates(EVSEOperator     EVSEOperator,
                                          EVSEStatusDiff   EVSEStatusDiff,
                                          HTTPEventSource  EventSource)

        {

            #region Insert new EVSEs...

            if (EVSEStatusDiff.NewEVSEStates.Count > 0)
            {

                //Console.WriteLine("Insert " + NewEVSEStates.Count + " new EVSE states at " + HTTPVirtualHost + "...");

                #region HTTP Logging

                //EventSource.
                //    SubmitSubEvent("INSERTEVSEStatesRequest",
                //                   new JObject(
                //                       new JProperty("Timestamp",       DateTime.Now.ToIso8601()),
                //                       new JProperty("RoamingNetwork",  EVSEOperator.RoamingNetwork.ToString()),
                //                       new JProperty("Values",          new JObject(EVSEStatusDiff.NewEVSEStates.Select(v => new JProperty(v.Key.ToString(), v.Value.ToString()))))
                //                   ).ToString().
                //                     Replace(Environment.NewLine, ""));

                #endregion

                var EVSEStatesInsertXML = EVSEStatusDiff.NewEVSEStates.
                                              Select(v => new KeyValuePair<EVSE_Id, HubjectEVSEState>(v.Key, v.Value.AsHubjectEVSEState())).
                                              PushEVSEStatusXML(EVSEOperator.Id,
                                                                EVSEOperator.Name[Languages.de],
                                                                ActionType.insert);

                using (var _OICPClient = new SOAPClient(_Hostname,
                                                        _TCPPort,
                                                        _HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingEvseStatus_V1.2",
                                                        UserAgent,
                                                        DNSClient))

                {

                    var r1 = _OICPClient.Query(EVSEStatesInsertXML,
                                             "eRoamingPushEvseStatus",
                                             Timeout: TimeSpan.FromSeconds(180),

                                             OnSuccess: XMLData => {

                                                 var ack = HubjectAcknowledgement.Parse(XMLData.Content);

                                                 #region HTTP Logging

                                                 //EventSource.
                                                 //        SubmitSubEvent("INSERTEVSEStatesResponse",
                                                 //                       new JObject(
                                                 //                           new JProperty("Timestamp",       DateTime.Now.ToIso8601()),
                                                 //                           new JProperty("RoamingNetwork",  EVSEOperator.RoamingNetwork.ToString()),
                                                 //                           new JProperty("Values",          new JObject(EVSEStatusDiff.NewEVSEStates.Select(v => new JProperty(v.Key.ToString(), v.Value.ToString())))),
                                                 //                           new JProperty("Result",          ack.Result),
                                                 //                           new JProperty("Description",     ack.Description),
                                                 //                           new JProperty("AdditionalInfo",  ack.AdditionalInfo)
                                                 //                       ).ToString().
                                                 //                         Replace(Environment.NewLine, ""));

                                                 #endregion

                                                 return new HTTPResponse<HubjectAcknowledgement>(XMLData.HttpResponse, ack, false);

                                             },


                                             OnSOAPFault: Fault => {

                                                 Debug.WriteLine("[" + DateTime.Now + "] 'eRoamingPushEvseStatus' lead to a fault!");

                                                 return new HTTPResponse<HubjectAcknowledgement>(
                                                     Fault.HttpResponse,
                                                     new HubjectAcknowledgement(false, 0, "", ""),
                                                     IsFault: true);

                                             },

                                             OnHTTPError: (t, s, e) => {

                                                 //var OnHTTPErrorLocal = OnHTTPError;
                                                 //if (OnHTTPErrorLocal != null)
                                                 //    OnHTTPErrorLocal(t, s, e);

                                             },

                                             OnException: (t, s, e) => {

                                                 //var OnExceptionLocal = OnException;
                                                 //if (OnExceptionLocal != null)
                                                 //    OnExceptionLocal(t, s, e);

                                             }

                                            );

                }


                //using (var httpClient = new HTTPClient(Hostname, Port, DNSClient))
                //{

                //    var builder = httpClient.POST("/ibis/ws/eRoamingEvseStatus_V1.2");
                //    builder.Host         = HTTPVirtualHost;
                //    builder.Content      = EVSEStatesInsertXML.ToUTF8Bytes();
                //    builder.ContentType  = HTTPContentType.XMLTEXT_UTF8;
                //    builder.Set("SOAPAction", "eRoamingPushEvseStatus");
                //    builder.UserAgent    = UserAgent;

                //    var Task02 = httpClient.Execute(builder, (req, resp) => {

                //        var ack = HubjectAcknowledgement.Parse(XDocument.Parse(resp.Content.ToUTF8String()).Root);

                //        #region HTTP Logging

                //        WWCP_HTTPServer.
                //            GetEventSource(Semantics.DebugLog).
                //                SubmitSubEvent("INSERTEVSEStatesResponse",
                //                               new JObject(
                //                                   new JProperty("Timestamp",       DateTime.Now.ToIso8601()),
                //                                   new JProperty("RoamingNetwork",  EVSEOperator.RoamingNetwork.ToString()),
                //                                   new JProperty("Values",          new JObject(EVSEStatusDiff.NewEVSEStates.Select(v => new JProperty(v.Key.ToString(), v.Value.ToString())))),
                //                                   new JProperty("Result",          ack.Result),
                //                                   new JProperty("Description",     ack.Description),
                //                                   new JProperty("AdditionalInfo",  ack.AdditionalInfo)
                //                               ).ToString().
                //                                 Replace(Environment.NewLine, ""));

                //        #endregion

                //        if (!ack.Result)
                //        {

                //            var Ids = EVSEStatusDiff.NewEVSEStates.
                //                          Select(v => "[" + v.Key.ToString() + ", " + v.Value.ToString() + "]").
                //                          Aggregate((a, b) => a + ", " + b);

                //            Console.WriteLine("Ids: " + Ids);
                //            Console.WriteLine("EVSE states insert: " + ack.Result + " / " + ack.Description);
                //            Console.WriteLine("Error: " + ack.Description + " / " + ack.AdditionalInfo);

                //        }

                //    });

                //    Task02.Wait(TimeSpan.FromSeconds(30));

                //}

            }

            #endregion

            #region Upload EVSE changes...

            if (EVSEStatusDiff.ChangedEVSEStates.Count > 0)
            {

                //Console.WriteLine("Update " + ChangedEVSEStates.Count + " EVSE states at " + HTTPVirtualHost + "...");

                #region HTTP Logging

                //EventSource.
                //        SubmitSubEvent("UPDATEEVSEStatesRequest",
                //                       new JObject(
                //                           new JProperty("Timestamp",       DateTime.Now.ToIso8601()),
                //                           new JProperty("RoamingNetwork",  EVSEOperator.RoamingNetwork.ToString()),
                //                           new JProperty("Values",          new JObject(EVSEStatusDiff.ChangedEVSEStates.Select(v => new JProperty(v.Key.ToString(), v.Value.ToString()))))
                //                       ).ToString().
                //                         Replace(Environment.NewLine, ""));

                #endregion

                var EVSEStatesUpdateXML = EVSEStatusDiff.ChangedEVSEStates.
                                              Select(v => new KeyValuePair<EVSE_Id, HubjectEVSEState>(v.Key, v.Value.AsHubjectEVSEState())).
                                              PushEVSEStatusXML(EVSEOperator.Id,
                                                                EVSEOperator.Name[Languages.de],
                                                                ActionType.update).
                                                                ToString();

                using (var httpClient = new HTTPClient(_Hostname, _TCPPort, DNSClient))
                {

                    var builder = httpClient.POST("/ibis/ws/eRoamingEvseStatus_V1.2");
                    builder.Host         = HTTPVirtualHost;
                    builder.Content      = EVSEStatesUpdateXML.ToUTF8Bytes();
                    builder.ContentType  = HTTPContentType.XMLTEXT_UTF8;
                    builder.Set("SOAPAction", "eRoamingPushEvseStatus");
                    builder.UserAgent    = UserAgent;

                    var Task02 = httpClient.Execute(builder, (req, resp) => {

                        var ack  = HubjectAcknowledgement.Parse(XDocument.Parse(resp.Content.ToUTF8String()).Root);

                        #region HTTP Logging

                        //EventSource.
                        //    SubmitSubEvent("UPDATEEVSEStatesResponse",
                        //                   new JObject(
                        //                       new JProperty("Timestamp",       DateTime.Now.ToIso8601()),
                        //                       new JProperty("RoamingNetwork",  EVSEOperator.RoamingNetwork.ToString()),
                        //                       new JProperty("Values",          new JObject(EVSEStatusDiff.ChangedEVSEStates.Select(v => new JProperty(v.Key.ToString(), v.Value.ToString())))),
                        //                       new JProperty("Result",          ack.Result),
                        //                       new JProperty("Description",     ack.Description),
                        //                       new JProperty("AdditionalInfo",  ack.AdditionalInfo)
                        //                   ).ToString().
                        //                     Replace(Environment.NewLine, ""));

                        #endregion

                        if (!ack.Result)
                        {

                            var Ids = EVSEStatusDiff.ChangedEVSEStates.
                                          Select(v => "[" + v.Key.ToString() + ", " + v.Value.ToString() + "]").
                                          Aggregate((a, b) => a + ", " + b);

                            Console.WriteLine("Ids: " + Ids);
                            Console.WriteLine("EVSE states updated: " + ack.Result + " / " + ack.Description);
                            Console.WriteLine("Error: " + ack.Description + " / " + ack.AdditionalInfo);

                        }

                    });

                    Task02.Wait(TimeSpan.FromSeconds(30));

                }

            }

            #endregion

            #region Remove outdated EVSEs...

            if (EVSEStatusDiff.RemovedEVSEIds.Count > 0)
            {

                Console.WriteLine("Removing " + EVSEStatusDiff.RemovedEVSEIds.Count + " EVSE states at " + HTTPVirtualHost + "...");

                #region HTTP Logging

                //EventSource.
                //    SubmitSubEvent("REMOVEEVSEStatesRequest",
                //                   new JObject(
                //                       new JProperty("Timestamp",       DateTime.Now.ToIso8601()),
                //                       new JProperty("RoamingNetwork",  EVSEOperator.RoamingNetwork.ToString()),
                //                       new JProperty("Values",          new JArray(EVSEStatusDiff.RemovedEVSEIds.Select(v => v.ToString())))
                //                   ).ToString().
                //                     Replace(Environment.NewLine, ""));

                #endregion

                var EVSEStatesUpdateXML = EVSEStatusDiff.RemovedEVSEIds.
                                              Select(v => new KeyValuePair<EVSE_Id, HubjectEVSEState>(v, HubjectEVSEState.OutOfService)).
                                              PushEVSEStatusXML(EVSEOperator.Id,
                                                                EVSEOperator.Name[Languages.de],
                                                                ActionType.delete).
                                                                ToString();

                using (var httpClient = new HTTPClient(_Hostname, _TCPPort, _DNSClient))
                {

                    var builder = httpClient.POST("/ibis/ws/eRoamingEvseStatus_V1.2");
                    builder.Host         = HTTPVirtualHost;
                    builder.Content      = EVSEStatesUpdateXML.ToUTF8Bytes();
                    builder.ContentType  = HTTPContentType.XMLTEXT_UTF8;
                    builder.Set("SOAPAction", "eRoamingPushEvseStatus");
                    builder.UserAgent    = UserAgent;

                    var Task02 = httpClient.Execute(builder, (req, resp) => {

                        var ack = HubjectAcknowledgement.Parse(XDocument.Parse(resp.Content.ToUTF8String()).Root);

                        #region HTTP Logging

                        //EventSource.
                        //    SubmitSubEvent("REMOVEEVSEStatesResponse",
                        //                   new JObject(
                        //                       new JProperty("Timestamp",       DateTime.Now.ToIso8601()),
                        //                       new JProperty("RoamingNetwork",  EVSEOperator.RoamingNetwork.ToString()),
                        //                       new JProperty("Values",          new JArray(EVSEStatusDiff.RemovedEVSEIds.Select(v => v.ToString()))),
                        //                       new JProperty("Result",          ack.Result),
                        //                       new JProperty("Description",     ack.Description),
                        //                       new JProperty("AdditionalInfo",  ack.AdditionalInfo)
                        //                   ).ToString().
                        //                     Replace(Environment.NewLine, ""));

                        #endregion

                        if (!ack.Result)
                        {

                            var Ids = EVSEStatusDiff.RemovedEVSEIds.
                                          Select(v => v.ToString()).
                                          Aggregate((a, b) => a + ", " + b);

                            Console.WriteLine("Ids: " + Ids);
                            Console.WriteLine("EVSE states removed: " + ack.Result + " / " + ack.Description);
                            Console.WriteLine("Error: " + ack.Description + " / " + ack.AdditionalInfo);

                        }

                    });

                    Task02.Wait(TimeSpan.FromSeconds(30));

                }

            }

            #endregion

        }

        #endregion


        #region AuthorizeStart(OperatorId, EVSEId, PartnerSessionId, UID)

        /// <summary>
        /// Create an OICP authorize start request.
        /// </summary>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="PartnerSessionId">Your own session identification.</param>
        /// <param name="UID">A RFID user identification.</param>
        public AUTHSTARTResult AuthorizeStart(EVSEOperator_Id     OperatorId,
                                              EVSE_Id             EVSEId,
                                              ChargingSession_Id  PartnerSessionId,
                                              Auth_Token          UID)

        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname, TCPPort, HTTPVirtualHost, "/ibis/ws/eRoamingAuthorization_V1.2"))
                {

                    var HttpResponse = _OICPClient.Query(CPOMethods.AuthorizeStartXML(OperatorId,
                                                                                      EVSEId,
                                                                                      PartnerSessionId,
                                                                                      UID),
                                                         "eRoamingAuthorizeStart");

                    Console.WriteLine(HttpResponse.Content.ToUTF8String());

                    //ToDo: In case of errors this will not parse!
                    var AuthStartResult = HubjectAuthorizationStart.Parse(XDocument.Parse(HttpResponse.Content.ToUTF8String()).Root);

                    #region Authorized

                    if (AuthStartResult.AuthorizationStatus == AuthorizationStatusType.Authorized)

                        // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                        //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                        //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
                        //   <soapenv:Body>
                        //     <tns:HubjectAuthorizationStart>
                        //       <tns:SessionID>8fade8bd-0a88-1296-0f2f-41ae8a80af1b</tns:SessionID>
                        //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                        //       <tns:ProviderID>BMW</tns:ProviderID>
                        //       <tns:AuthorizationStatus>Authorized</tns:AuthorizationStatus>
                        //       <tns:StatusCode>
                        //         <cmn:Code>000</cmn:Code>
                        //         <cmn:Description>Success</cmn:Description>
                        //       </tns:StatusCode>
                        //     </tns:HubjectAuthorizationStart>
                        //   </soapenv:Body>
                        // </soapenv:Envelope>

                        return new AUTHSTARTResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthorizationResult.Authorized,
                                       SessionId            = AuthStartResult.SessionID,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVSP_Id.Parse(AuthStartResult.ProviderID),
                                       Description          = AuthStartResult.Description
                                   };

                    #endregion

                    #region NotAuthorized

                    else // AuthorizationStatus == AuthorizationStatusType.NotAuthorized
                    {

                        //- Invalid OperatorId ----------------------------------------------------------------------

                        // <isns:Envelope xmlns:fn   = "http://www.w3.org/2005/xpath-functions"
                        //                xmlns:isns = "http://schemas.xmlsoap.org/soap/envelope/"
                        //                xmlns:v1   = "http://www.hubject.com/b2b/services/commontypes/v1"
                        //                xmlns:wsc  = "http://www.hubject.com/b2b/services/authorization/v1">
                        //   <isns:Body>
                        //     <wsc:HubjectAuthorizationStop>
                        //       <wsc:SessionID>8f9cbd74-0a88-1296-1078-6e9cca762de2</wsc:SessionID>
                        //       <wsc:PartnerSessionID>0815</wsc:PartnerSessionID>
                        //       <wsc:AuthorizationStatus>NotAuthorized</wsc:AuthorizationStatus>
                        //       <wsc:StatusCode>
                        //         <v1:Code>017</v1:Code>
                        //         <v1:Description>Unauthorized Access</v1:Description>
                        //         <v1:AdditionalInfo>The identification criterion for the provider/operator with the ID "812" doesn't match the given identification information "/C=DE/ST=Bayern/L=Kitzingen/O=Hubject/OU=Belectric Drive GmbH/CN=Belectric ITS Software Development/emailAddress=achim.friedland@belectric.com" from the certificate.</v1:AdditionalInfo>
                        //       </wsc:StatusCode>
                        //     </wsc:HubjectAuthorizationStop>
                        //   </isns:Body>
                        // </isns:Envelope>

                        if (AuthStartResult.Code == 017)
                            return new AUTHSTARTResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                       PartnerSessionId     = PartnerSessionId,
                                       Description          = AuthStartResult.Description + " - " + AuthStartResult.AdditionalInfo
                                   };


                        //- Invalid UID -----------------------------------------------------------------------------

                        // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                        //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                        //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
                        //   <soapenv:Body>
                        //     <tns:HubjectAuthorizationStart>
                        //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                        //       <tns:AuthorizationStatus>NotAuthorized</tns:AuthorizationStatus>
                        //       <tns:StatusCode>
                        //         <cmn:Code>320</cmn:Code>
                        //         <cmn:Description>Service not available</cmn:Description>
                        //       </tns:StatusCode>
                        //     </tns:HubjectAuthorizationStart>
                        //   </soapenv:Body>
                        // </soapenv:Envelope>

                        // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1.2"
                        //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                        //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1.2">
                        //   <soapenv:Body>
                        //     <tns:eRoamingAuthorizationStart>
                        //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                        //       <tns:AuthorizationStatus>NotAuthorized</tns:AuthorizationStatus>
                        //       <tns:StatusCode>
                        //         <cmn:Code>102</cmn:Code>
                        //         <cmn:Description>RFID Authentication failed – invalid UID</cmn:Description>
                        //       </tns:StatusCode>
                        //     </tns:eRoamingAuthorizationStart>
                        //   </soapenv:Body>
                        // </soapenv:Envelope>

                        else
                            return new AUTHSTARTResult(AuthorizatorId) {
                                           AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                           PartnerSessionId     = PartnerSessionId,
                                           Description          = AuthStartResult.Description
                                       };

                    }

                    #endregion

                }

            }

            catch (Exception e)
            {

                return new AUTHSTARTResult(AuthorizatorId) {
                               AuthorizationResult  = AuthorizationResult.NotAuthorized,
                               PartnerSessionId     = PartnerSessionId,
                               Description          = "An exception occured: " + e.Message
                           };

            }

        }

        #endregion

        #region AuthorizeStop(OperatorId, EVSEId, SessionId, PartnerSessionId, UID)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an OICP authorize stop request.
        /// </summary>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="PartnerSessionId">Your own session identification.</param>
        /// <param name="UID">A RFID user identification.</param>
        public AUTHSTOPResult AuthorizeStop(EVSEOperator_Id     OperatorId,
                                            EVSE_Id             EVSEId,
                                            ChargingSession_Id  SessionId,
                                            ChargingSession_Id  PartnerSessionId,
                                            Auth_Token          UID)

        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname, TCPPort, HTTPVirtualHost, "/ibis/ws/eRoamingAuthorization_V1.2"))
                {

                    var HttpResponse = _OICPClient.Query(CPOMethods.AuthorizeStopXML(OperatorId,
                                                                                     EVSEId,
                                                                                     SessionId,
                                                                                     PartnerSessionId,
                                                                                     UID),
                                                         "eRoamingAuthorizeStop");

                    //ToDo: In case of errors this will not parse!
                    var AuthStopResult = HubjectAuthorizationStop.Parse(XDocument.Parse(HttpResponse.Content.ToUTF8String()).Root);

                    #region Authorized

                    if (AuthStopResult.AuthorizationStatus == AuthorizationStatusType.Authorized)

                        // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                        //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                        //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
                        //   <soapenv:Body>
                        //     <tns:HubjectAuthorizationStop>
                        //       <tns:SessionID>8f9cbd74-0a88-1296-2078-6e9cca762de2</tns:SessionID>
                        //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                        //       <tns:ProviderID>BMW</tns:ProviderID>
                        //       <tns:AuthorizationStatus>Authorized</tns:AuthorizationStatus>
                        //       <tns:StatusCode>
                        //         <cmn:Code>000</cmn:Code>
                        //         <cmn:Description>Success</cmn:Description>
                        //       </tns:StatusCode>
                        //     </tns:HubjectAuthorizationStop>
                        //   </soapenv:Body>
                        // </soapenv:Envelope>

                        return new AUTHSTOPResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthorizationResult.Authorized,
                                       SessionId            = AuthStopResult.SessionID,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVSP_Id.Parse(AuthStopResult.ProviderID),
                                       Description          = AuthStopResult.Description
                                   };

                    #endregion

                    #region NotAuthorized

                    else // AuthorizationStatus == AuthorizationStatusType.NotAuthorized
                    {

                        //- Invalid OperatorId ----------------------------------------------------------------------

                        // <isns:Envelope xmlns:fn   = "http://www.w3.org/2005/xpath-functions"
                        //                xmlns:isns = "http://schemas.xmlsoap.org/soap/envelope/"
                        //                xmlns:v1   = "http://www.hubject.com/b2b/services/commontypes/v1"
                        //                xmlns:wsc  = "http://www.hubject.com/b2b/services/authorization/v1">
                        //   <isns:Body>
                        //     <wsc:HubjectAuthorizationStop>
                        //       <wsc:SessionID>8f9cbd74-0a88-1296-1078-6e9cca762de2</wsc:SessionID>
                        //       <wsc:PartnerSessionID>0815</wsc:PartnerSessionID>
                        //       <wsc:AuthorizationStatus>NotAuthorized</wsc:AuthorizationStatus>
                        //       <wsc:StatusCode>
                        //         <v1:Code>017</v1:Code>
                        //         <v1:Description>Unauthorized Access</v1:Description>
                        //         <v1:AdditionalInfo>The identification criterion for the provider/operator with the ID "812" doesn't match the given identification information "/C=DE/ST=Bayern/L=Kitzingen/O=Hubject/OU=Belectric Drive GmbH/CN=Belectric ITS Software Development/emailAddress=achim.friedland@belectric.com" from the certificate.</v1:AdditionalInfo>
                        //       </wsc:StatusCode>
                        //     </wsc:HubjectAuthorizationStop>
                        //   </isns:Body>
                        // </isns:Envelope>

                        if (AuthStopResult.Code == 017)
                            return new AUTHSTOPResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                       PartnerSessionId     = PartnerSessionId,
                                       Description          = AuthStopResult.Description + " - " + AuthStopResult.AdditionalInfo
                                   };


                        //- Invalid SessionId -----------------------------------------------------------------------

                        // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                        //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                        //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
                        //   <soapenv:Body>
                        //     <tns:HubjectAuthorizationStop>
                        //       <tns:SessionID>8f9cbd74-0a88-1296-1078-6e9cca762de2</tns:SessionID>
                        //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                        //       <tns:AuthorizationStatus>NotAuthorized</tns:AuthorizationStatus>
                        //       <tns:StatusCode>
                        //         <cmn:Code>400</cmn:Code>
                        //         <cmn:Description>Session is invalid</cmn:Description>
                        //       </tns:StatusCode>
                        //     </tns:HubjectAuthorizationStop>
                        //   </soapenv:Body>
                        // </soapenv:Envelope>

                        //- Invalid UID -----------------------------------------------------------------------------

                        // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                        //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                        //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
                        //   <soapenv:Body>
                        //     <tns:HubjectAuthorizationStop>
                        //       <tns:SessionID>8f9cbd74-0a88-1296-2078-6e9cca762de2</tns:SessionID>
                        //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                        //       <tns:AuthorizationStatus>NotAuthorized</tns:AuthorizationStatus>
                        //       <tns:StatusCode>
                        //         <cmn:Code>102</cmn:Code>
                        //         <cmn:Description>RFID Authentication failed – invalid UID</cmn:Description>
                        //       </tns:StatusCode>
                        //     </tns:HubjectAuthorizationStop>
                        //   </soapenv:Body>
                        // </soapenv:Envelope>


                        //- Invalid PartnerSessionId ----------------------------------------------------------------

                        // No checks!


                        //- EVSEID changed/is invalid! --------------------------------------------------------------

                        //   => Session is invalid

                        else

                            return new AUTHSTOPResult(AuthorizatorId) {
                                           AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                           PartnerSessionId     = PartnerSessionId,
                                           Description          = AuthStopResult.Description
                                       };

                    }

                    #endregion

                }

            }

            catch (Exception e)
            {

                return new AUTHSTOPResult(AuthorizatorId) {
                    AuthorizationResult  = AuthorizationResult.NotAuthorized,
                    PartnerSessionId     = PartnerSessionId,
                    Description          = "An exception occured: " + e.Message
                };

            }

        }

        #endregion

        #region SendCDR(EVSEId, SessionId, PartnerSessionId, PartnerProductId, ChargeStart, ChargeEnd, UID = null, EVCOId = null, SessionStart = null, SessionEnd = null, MeterValueStart = null, MeterValueEnd = null)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="PartnerSessionId">Your own session identification.</param>
        /// <param name="PartnerProductId"></param>
        /// <param name="UID">The optional RFID user identification.</param>
        /// <param name="EVCOId"></param>
        /// <param name="ChargeStart">The timestamp of the charging start.</param>
        /// <param name="ChargeEnd">The timestamp of the charging end.</param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="MeterValueStart">The initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">The final value of the energy meter.</param>
        public SENDCDRResult SendCDR(EVSE_Id             EVSEId,
                                     ChargingSession_Id  SessionId,
                                     ChargingSession_Id  PartnerSessionId,
                                     String              PartnerProductId,
                                     DateTime            ChargeStart,
                                     DateTime            ChargeEnd,
                                     Auth_Token          UID             = null,
                                     eMA_Id              EVCOId          = null,
                                     DateTime?           SessionStart    = null,
                                     DateTime?           SessionEnd      = null,
                                     Double?             MeterValueStart = null,
                                     Double?             MeterValueEnd   = null)

        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname, TCPPort, HTTPVirtualHost, "/ibis/ws/eRoamingAuthorization_V1.2"))
                {

                    var HttpResponse = _OICPClient.Query(CPOMethods.SendChargeDetailRecordXML(EVSEId,
                                                                                              SessionId,
                                                                                              PartnerSessionId,
                                                                                              PartnerProductId,
                                                                                              ChargeStart,
                                                                                              ChargeEnd,
                                                                                              UID,
                                                                                              EVCOId,
                                                                                              SessionStart,
                                                                                              SessionEnd,
                                                                                              MeterValueStart,
                                                                                              MeterValueEnd),
                                                         "eRoamingChargeDetailRecord");

                    //ToDo: In case of errors this will not parse!
                    var ack = HubjectAcknowledgement.Parse(XDocument.Parse(HttpResponse.Content.ToUTF8String()).Root);

                    #region Ok

                    if (ack.Result)
                        return new SENDCDRResult(AuthorizatorId) {
                            State             = SENDCDRState.Forwarded,
                            PartnerSessionId  = PartnerSessionId,
                            Description       = ack.Description
                        };

                    #endregion

                    #region Error

                    else
                        return new SENDCDRResult(AuthorizatorId) {
                            State             = SENDCDRState.False,
                            PartnerSessionId  = PartnerSessionId,
                            Description       = ack.Description
                        };

                    #endregion

                }

            }

            catch (Exception e)
            {

                return
                    new SENDCDRResult(AuthorizatorId) {
                        State             = SENDCDRState.False,
                        PartnerSessionId  = PartnerSessionId,
                        Description       = "An exception occured: " + e.Message
                    };

            }

        }

        #endregion

    }

}
