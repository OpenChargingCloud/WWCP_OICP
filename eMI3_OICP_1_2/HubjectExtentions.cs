/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of eMI3 OICP <http://www.github.com/eMI3/OICP-Bindings>
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
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.eMI3.IO.OICP;
using org.GraphDefined.eMI3.IO.WWCP;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.eMI3.IO.OICP_1_2
{

    public static class HubjectExtentions
    {

        #region SendEVSEStatusUpdate(...)

        public static void SendEVSEStatusUpdate(EVSEOperator     EVSEOperator,
                                                EVSEStatusDiff   EVSEStatusDiff,
                                                DNSClient        DNSClient,
                                                String           Hostname,
                                                IPPort           Port,
                                                String           HTTPVirtualHost,
                                                WWCP_HTTPServer  WWCP_HTTPServer,
                                                String           UserAgent  = "Belectric Drive Hubject Gateway")

        {

            var IPv4Addresses = DNSClient.Query<A>(Hostname).Select(a => a.IPv4Address).ToArray();

            #region Insert new EVSEs...

            if (EVSEStatusDiff.NewEVSEStates.Count > 0)
            {

                //Console.WriteLine("Insert " + NewEVSEStates.Count + " new EVSE states at " + HTTPVirtualHost + "...");

                #region HTTP Logging

                WWCP_HTTPServer.
                    GetEventSource(Semantics.DebugLog).
                        SubmitSubEvent("INSERTEVSEStatesRequest",
                                       new JObject(
                                           new JProperty("Timestamp",       DateTime.Now.ToIso8601()),
                                           new JProperty("RoamingNetwork",  EVSEOperator.RoamingNetwork.ToString()),
                                           new JProperty("Values",          new JObject(EVSEStatusDiff.NewEVSEStates.Select(v => new JProperty(v.Key.ToString(), v.Value.ToString()))))
                                       ).ToString().
                                         Replace(Environment.NewLine, ""));

                #endregion

                var EVSEStatesInsertXML = EVSEStatusDiff.NewEVSEStates.
                                              Select(v => new KeyValuePair<EVSE_Id, HubjectEVSEState>(v.Key, v.Value.AsHubjectEVSEState())).
                                              PushEVSEStatusXML(EVSEOperator.Id,
                                                                EVSEOperator.Name[Languages.de],
                                                                ActionType.insert).
                                                                ToString();

                using (var httpClient = new HTTPClient(IPv4Addresses.First(), Port))
                {

                    var builder = httpClient.POST("/ibis/ws/eRoamingEvseStatus_V1.2");
                    builder.Host         = HTTPVirtualHost;
                    builder.Content      = EVSEStatesInsertXML.ToUTF8Bytes();
                    builder.ContentType  = HTTPContentType.XMLTEXT_UTF8;
                    builder.Set("SOAPAction", "eRoamingPushEvseStatus");
                    builder.UserAgent    = UserAgent;

                    var Task02 = httpClient.Execute(builder, (req, resp) => {

                        var ack = HubjectAcknowledgement.Parse(XDocument.Parse(resp.Content.ToUTF8String()).Root);

                        #region HTTP Logging

                        WWCP_HTTPServer.
                            GetEventSource(Semantics.DebugLog).
                                SubmitSubEvent("INSERTEVSEStatesResponse",
                                               new JObject(
                                                   new JProperty("Timestamp",       DateTime.Now.ToIso8601()),
                                                   new JProperty("RoamingNetwork",  EVSEOperator.RoamingNetwork.ToString()),
                                                   new JProperty("Values",          new JObject(EVSEStatusDiff.NewEVSEStates.Select(v => new JProperty(v.Key.ToString(), v.Value.ToString())))),
                                                   new JProperty("Result",          ack.Result),
                                                   new JProperty("Description",     ack.Description),
                                                   new JProperty("AdditionalInfo",  ack.AdditionalInfo)
                                               ).ToString().
                                                 Replace(Environment.NewLine, ""));

                        #endregion

                        if (!ack.Result)
                        {

                            var Ids = EVSEStatusDiff.NewEVSEStates.
                                          Select(v => "[" + v.Key.ToString() + ", " + v.Value.ToString() + "]").
                                          Aggregate((a, b) => a + ", " + b);

                            Console.WriteLine("Ids: " + Ids);
                            Console.WriteLine("EVSE states insert: " + ack.Result + " / " + ack.Description);
                            Console.WriteLine("Error: " + ack.Description + " / " + ack.AdditionalInfo);

                        }

                    });

                    Task02.Wait(TimeSpan.FromSeconds(30));

                }

            }

            #endregion

            #region Upload EVSE changes...

            if (EVSEStatusDiff.ChangedEVSEStates.Count > 0)
            {

                //Console.WriteLine("Update " + ChangedEVSEStates.Count + " EVSE states at " + HTTPVirtualHost + "...");

                #region HTTP Logging

                WWCP_HTTPServer.
                    GetEventSource(Semantics.DebugLog).
                        SubmitSubEvent("UPDATEEVSEStatesRequest",
                                       new JObject(
                                           new JProperty("Timestamp",       DateTime.Now.ToIso8601()),
                                           new JProperty("RoamingNetwork",  EVSEOperator.RoamingNetwork.ToString()),
                                           new JProperty("Values",          new JObject(EVSEStatusDiff.ChangedEVSEStates.Select(v => new JProperty(v.Key.ToString(), v.Value.ToString()))))
                                       ).ToString().
                                         Replace(Environment.NewLine, ""));

                #endregion

                var EVSEStatesUpdateXML = EVSEStatusDiff.ChangedEVSEStates.
                                              Select(v => new KeyValuePair<EVSE_Id, HubjectEVSEState>(v.Key, v.Value.AsHubjectEVSEState())).
                                              PushEVSEStatusXML(EVSEOperator.Id,
                                                                EVSEOperator.Name[Languages.de],
                                                                ActionType.update).
                                                                ToString();

                using (var httpClient = new HTTPClient(IPv4Addresses.First(), Port))
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

                        WWCP_HTTPServer.
                            GetEventSource(Semantics.DebugLog).
                                SubmitSubEvent("UPDATEEVSEStatesResponse",
                                               new JObject(
                                                   new JProperty("Timestamp",       DateTime.Now.ToIso8601()),
                                                   new JProperty("RoamingNetwork",  EVSEOperator.RoamingNetwork.ToString()),
                                                   new JProperty("Values",          new JObject(EVSEStatusDiff.ChangedEVSEStates.Select(v => new JProperty(v.Key.ToString(), v.Value.ToString())))),
                                                   new JProperty("Result",          ack.Result),
                                                   new JProperty("Description",     ack.Description),
                                                   new JProperty("AdditionalInfo",  ack.AdditionalInfo)
                                               ).ToString().
                                                 Replace(Environment.NewLine, ""));

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

                WWCP_HTTPServer.
                    GetEventSource(Semantics.DebugLog).
                        SubmitSubEvent("REMOVEEVSEStatesRequest",
                                       new JObject(
                                           new JProperty("Timestamp",       DateTime.Now.ToIso8601()),
                                           new JProperty("RoamingNetwork",  EVSEOperator.RoamingNetwork.ToString()),
                                           new JProperty("Values",          new JArray(EVSEStatusDiff.RemovedEVSEIds.Select(v => v.ToString())))
                                       ).ToString().
                                         Replace(Environment.NewLine, ""));

                #endregion

                var EVSEStatesUpdateXML = EVSEStatusDiff.RemovedEVSEIds.
                                              Select(v => new KeyValuePair<EVSE_Id, HubjectEVSEState>(v, HubjectEVSEState.OutOfService)).
                                              PushEVSEStatusXML(EVSEOperator.Id,
                                                                EVSEOperator.Name[Languages.de],
                                                                ActionType.delete).
                                                                ToString();

                using (var httpClient = new HTTPClient(IPv4Addresses.First(), Port))
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

                        WWCP_HTTPServer.
                            GetEventSource(Semantics.DebugLog).
                                SubmitSubEvent("REMOVEEVSEStatesResponse",
                                               new JObject(
                                                   new JProperty("Timestamp",       DateTime.Now.ToIso8601()),
                                                   new JProperty("RoamingNetwork",  EVSEOperator.RoamingNetwork.ToString()),
                                                   new JProperty("Values",          new JArray(EVSEStatusDiff.RemovedEVSEIds.Select(v => v.ToString()))),
                                                   new JProperty("Result",          ack.Result),
                                                   new JProperty("Description",     ack.Description),
                                                   new JProperty("AdditionalInfo",  ack.AdditionalInfo)
                                               ).ToString().
                                                 Replace(Environment.NewLine, ""));

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

        #region InitialEVSEStatus_Upload(...)

        public static void InitialEVSEStatus_Upload(EVSEOperator  EVSEOperator,
                                                    DNSClient     DNSClient,
                                                    String        Hostname,
                                                    IPPort        Port,
                                                    String        HTTPVirtualHost,
                                                    String        UserAgent  = "Belectric Drive Hubject Gateway")
        {

            var XML_EVSEStates = EVSEOperator.AllEVSEStates.ToArray();

            var IPv4Addresses = DNSClient.Query<A>(Hostname).Select(a => a.IPv4Address).ToArray();

            #region FullLoad EVSEs...

            if (XML_EVSEStates.Any())
            {

                Console.WriteLine("FullLoad of " + XML_EVSEStates.Length + " EVSE states at " + HTTPVirtualHost + "...");

                var EVSEStatesInsertXML = XML_EVSEStates.
                                              Select(v => new KeyValuePair<EVSE_Id, HubjectEVSEState>(v.Key, v.Value.AsHubjectEVSEState())).
                                              PushEVSEStatusXML(EVSEOperator.Id,
                                                                EVSEOperator.Name[Languages.de],
                                                                ActionType.fullLoad).
                                                                ToString();

                using (var httpClient = new HTTPClient(IPv4Addresses.First(), Port))
                {

                    var builder = httpClient.POST("/ibis/ws/eRoamingEvseStatus_V1.2");
                    builder.Host         = HTTPVirtualHost;
                    builder.Content      = EVSEStatesInsertXML.ToUTF8Bytes();
                    builder.ContentType  = HTTPContentType.XMLTEXT_UTF8;
                    builder.Set("SOAPAction", "eRoamingPushEvseStatus");
                    builder.UserAgent    = UserAgent;

                    var Task02 = httpClient.Execute(builder, (req, resp) => {
                        var ack = HubjectAcknowledgement.Parse(XDocument.Parse(resp.Content.ToUTF8String()).Root);
                        Console.WriteLine("EVSE states fullload: " + ack.Result + " / " + ack.Description + Environment.NewLine);
                    });

                    Task02.Wait(TimeSpan.FromSeconds(30));

                }

            }

            #endregion

        }

        #endregion

        #region InitialEVSEData_Upload(EVSEOperator, DNSClient, Hostname, Port, HTTPVirtualHost, UserAgent = "Belectric Drive Hubject Gateway")

        public static void InitialEVSEData_Upload(EVSEOperator  EVSEOperator,
                                                  DNSClient     DNSClient,
                                                  String        Hostname,
                                                  IPPort        Port,
                                                  String        HTTPVirtualHost,
                                                  String        UserAgent  = "Belectric Drive Hubject Gateway")

        {

            Console.WriteLine("FullLoad of " + EVSEOperator.ChargingPools.SelectMany(Pool => Pool.ChargingStations).SelectMany(Station => Station.EVSEs).Count() + " EVSE static data sets at " + HTTPVirtualHost + "...");
            try
            {

            var EVSEDataFullLoadXML = EVSEOperator.ChargingPools.
                                          PushEVSEDataXML(EVSEOperator.Id,
                                                          EVSEOperator.Name[Languages.de],
                                                          ActionType.fullLoad).
                                          ToString();

            var IPv4Addresses = DNSClient.Query<A>(Hostname).Select(a => a.IPv4Address).ToArray();

            Console.WriteLine(Hostname + " => " + IPv4Addresses.First().ToString() + ":" + Port.ToString());

            using (var httpClient = new HTTPClient(IPv4Addresses.First(), Port))
            {

                var builder = httpClient.POST("/ibis/ws/eRoamingEvseData_V1.2");
                builder.Host        = HTTPVirtualHost;
                builder.Content     = EVSEDataFullLoadXML.ToUTF8Bytes();
                builder.ContentType = HTTPContentType.XMLTEXT_UTF8;
                builder.Set("SOAPAction", "eRoamingPushEvseData");
                builder.UserAgent   = UserAgent;

                var Task01 = httpClient.Execute(builder, (req, resp) => {
                    var ack = HubjectAcknowledgement.Parse(XDocument.Parse(resp.Content.ToUTF8String()).Root);
                    Console.WriteLine("EVSE data fullload: " + ack.Result + " / " + ack.Description + Environment.NewLine);
                });

                Task01.Wait(TimeSpan.FromSeconds(30));

            }

            } catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        #endregion

    }

}
