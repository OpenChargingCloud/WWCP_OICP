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
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.ConsoleLog;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;

using org.GraphDefined.WWCP.LocalService;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OICPClient_2_0
{

    /// <summary>
    /// OICP v2.0 HTTP/SOAP server.
    /// </summary>
    public class OICPServer : HTTPServer
    {

        #region Properties

        #region RequestRouter

        private readonly RequestRouter _RequestRouter;

        public RequestRouter RequestRouter
        {
            get
            {
                return _RequestRouter;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initialize the OICP v2.0 HTTP/SOAP server using IPAddress.Any.
        /// </summary>
        /// <param name="RequestRouter">The request router.</param>
        /// <param name="IPPort">The TCP listing port.</param>
        /// <param name="HTTPRoot">The document root for the HTTP service.</param>
        /// <param name="RegisterHTTPRootService">Whether to register a simple webpage for '/', or not.</param>
        /// 
        public OICPServer(RequestRouter  RequestRouter,
                          IPPort         IPPort,
                          Boolean        RegisterHTTPRootService  = true,
                          DNSClient      DNSClient                = null)

            : base(//IPPort,
                   DNSClient: DNSClient)

        {

            this._RequestRouter  = RequestRouter;

            this.AttachTCPPort(IPPort);
            this.Start();

            #region / (HTTPRoot), if RegisterHTTPRootService == true

            if (RegisterHTTPRootService)
            {

                // HTML
                this.AddMethodCallback(HTTPMethod.GET,
                                       "/",
                                       HTTPContentType.HTML_UTF8,
                                       HTTPDelegate: HTTPRequest => {

                                           var RoamingNetworkId = HTTPRequest.ParsedURIParameters[0];

                                           return new HTTPResponseBuilder() {
                                               HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                               ContentType     = HTTPContentType.HTML_UTF8,
                                               Content         = ("/RNs/{RoamingNetworkId} is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                               Connection      = "close"
                                           };

                                       });

                // Text
                this.AddMethodCallback(HTTPMethod.GET,
                                       "/",
                                       HTTPContentType.TEXT_UTF8,
                                       HTTPDelegate: HTTPRequest => {

                                           var RoamingNetworkId = HTTPRequest.ParsedURIParameters[0];

                                           return new HTTPResponseBuilder() {
                                               HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                               ContentType     = HTTPContentType.HTML_UTF8,
                                               Content         = ("/RNs/{RoamingNetworkId} is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                               Connection      = "close"
                                           };

                                       });

            }

            #endregion

            #region /RNs/{RoamingNetworkId}

            #region Generic OICPServerDelegate

            HTTPDelegate OICPServerDelegate = HTTPRequest => {

                //var _EventTrackingId = EventTracking_Id.New;
                //Log.WriteLine("Event tracking: " + _EventTrackingId);

                #region Parse XML request body... or fail!

                var XMLRequest = HTTPRequest.ParseXMLRequestBody();
                if (XMLRequest.HasErrors)
                {

                    Log.WriteLine("Invalid XML request!");
                    Log.WriteLine(HTTPRequest.Content.ToUTF8String());

                    GetEventSource(Semantics.DebugLog).
                        SubmitSubEvent("InvalidXMLRequest",
                                       new JObject(
                                           new JProperty("@context",      "http://emi3group.org/contexts/InvalidXMLRequest.jsonld"),
                                           new JProperty("Timestamp",     DateTime.Now.ToIso8601()),
                                           new JProperty("RemoteSocket",  HTTPRequest.RemoteSocket.ToString()),
                                           new JProperty("XMLRequest",    HTTPRequest.Content.ToUTF8String()) //ToDo: Handle errors!
                                       ).ToString().
                                         Replace(Environment.NewLine, ""));

                    return XMLRequest.Error;

                }

                #endregion

                //ToDo: Check SOAP header/body XML tags!

                try
                {

                    #region Get and verify XML/SOAP request...

                    IEnumerable<XElement> PushEVSEDataXMLs;
                    IEnumerable<XElement> PullEVSEDataXMLs;
                    IEnumerable<XElement> GetEVSEByIdXMLs;

                    IEnumerable<XElement> PushEVSEStatusXMLs;
                    IEnumerable<XElement> PullEVSEStatusXMLs;
                    IEnumerable<XElement> PullEVSEStatusByIdXMLs;


                    // EvseDataBinding
                    PushEVSEDataXMLs        = XMLRequest.Data.Root.Descendants(OICPNS.EVSEData   + "eRoamingPushEvseData");
                    PullEVSEDataXMLs        = XMLRequest.Data.Root.Descendants(OICPNS.EVSEData   + "eRoamingPullEvseData");
                    GetEVSEByIdXMLs         = XMLRequest.Data.Root.Descendants(OICPNS.EVSEData   + "eRoamingGetEvseById");

                    // EvseStatusBinding
                    PushEVSEStatusXMLs      = XMLRequest.Data.Root.Descendants(OICPNS.EVSEStatus + "eRoamingPushEvseStatus");
                    PullEVSEStatusXMLs      = XMLRequest.Data.Root.Descendants(OICPNS.EVSEStatus + "eRoamingPullEvseStatus");
                    PullEVSEStatusByIdXMLs  = XMLRequest.Data.Root.Descendants(OICPNS.EVSEStatus + "eRoamingPullEvseStatusById");

                    if (!PushEVSEDataXMLs.      Any() &&
                        !PullEVSEDataXMLs.      Any() &&
                        !GetEVSEByIdXMLs.       Any() &&

                        !PushEVSEStatusXMLs.    Any() &&
                        !PullEVSEStatusXMLs.    Any() &&
                        !PullEVSEStatusByIdXMLs.Any())

                        throw new Exception("Unkown XML/SOAP request!");

                    if (PushEVSEDataXMLs.       Count() > 1)
                        throw new Exception("Multiple PushEvseData XML tags within a single request are not supported!");

                    if (PullEVSEDataXMLs.       Count() > 1)
                        throw new Exception("Multiple PullEVSEData XML tags within a single request are not supported!");

                    if (GetEVSEByIdXMLs.        Count() > 1)
                        throw new Exception("Multiple GetEVSEById XML tags within a single request are not supported!");


                    if (PushEVSEStatusXMLs.     Count() > 1)
                        throw new Exception("Multiple PushEVSEStatus XML tags within a single request are not supported!");

                    if (PullEVSEStatusXMLs.     Count() > 1)
                        throw new Exception("Multiple PullEVSEStatus XML tags within a single request are not supported!");

                    if (PullEVSEStatusByIdXMLs. Count() > 1)
                        throw new Exception("Multiple PullEVSEStatusBy XML tags within a single request are not supported!");

                    #endregion

                    #region PushEVSEData

                    var PushEVSEDataXML = PushEVSEDataXMLs.FirstOrDefault();
                    if (PushEVSEDataXML != null)
                    {

                        #region Parse request parameters

                        String                  ActionType;
                        IEnumerable<XElement>   OperatorEvseDataXML;

                        ActionType           = PushEVSEDataXML.ElementValue  (OICPNS.EVSEData + "ActionType",       "No ActionType XML tag provided!");
                        OperatorEvseDataXML  = PushEVSEDataXML.ElementsOrFail(OICPNS.EVSEData + "OperatorEvseData", "No OperatorEvseData XML tags provided!");

                        foreach (var SingleOperatorEvseDataXML in OperatorEvseDataXML)
                        {

                            EVSEOperator_Id         OperatorId;
                            String                  OperatorName;
                            IEnumerable<XElement>   EVSEDataRecordsXML;

                            if (!EVSEOperator_Id.TryParse(SingleOperatorEvseDataXML.ElementValue(OICPNS.EVSEData + "OperatorID", "No OperatorID XML tag provided!"), out OperatorId))
                                throw new ApplicationException("Invalid OperatorID XML tag provided!");

                            OperatorName        = SingleOperatorEvseDataXML.ElementValueOrDefault(OICPNS.EVSEData + "OperatorName",   "");
                            EVSEDataRecordsXML  = SingleOperatorEvseDataXML.ElementsOrFail       (OICPNS.EVSEData + "EvseDataRecord", "No EvseDataRecord XML tags provided!");

                            foreach (var EVSEDataRecordXML in EVSEDataRecordsXML)
                            {

                                #region Data

                                String                  EVSEDataRecord_deltaType;
                                String                  EVSEDataRecord_lastUpdate;
                                EVSE_Id                 EVSEId;
                                String                  ChargingStationId;
                                String                  ChargingStationName;
                                String                  EnChargingStationName;

                                XElement                AddressXML;
                                Address                 Address;
                                Country                 CountryValue;
                                String                  City;
                                String                  Street;
                                String                  PostalCode;
                                String                  HouseNum;
                                String                  Floor;
                                String                  Region;
                                String                  Timezone;

                                XElement                GeoCoordinatesXML;
                                GeoCoordinate           EVSEGeoCoordinate;
                                XElement                EVSEGoogleXML;
                                XElement                EVSEDecimalDegreeXML;
                                Longitude               EVSELongitudeValue;
                                Latitude                EVSELatitudeValue;
                                XElement                EVSEDegreeMinuteSecondsXML;

                                XElement                GeoChargingPointEntranceXML;
                                GeoCoordinate           EntranceGeoCoordinate;
                                XElement                EntranceGoogleXML;
                                XElement                EntranceDecimalDegreeXML;
                                Longitude               EntranceLongitudeValue;
                                Latitude                EntranceLatitudeValue;
                                XElement                EntranceDegreeMinuteSecondsXML;

                                XElement                PlugsXML;
                                IEnumerable<String>     Plugs;
                                XElement                ChargingFacilitiesXML;
                                IEnumerable<String>     ChargingFacilities;
                                XElement                ChargingModesXML;
                                IEnumerable<String>     ChargingModes;
                                XElement                AuthenticationModesXML;
                                IEnumerable<String>     AuthenticationModes;
                                XElement                PaymentOptionsXML;
                                IEnumerable<String>     PaymentOptions;

                                String                  MaxCapacity;
                                String                  Accessibility;
                                String                  HotlinePhoneNum;
                                String                  AdditionalInfo;
                                String                  EnAdditionalInfo;
                                String                  IsOpen24Hours;
                                String                  OpeningTimes;
                                String                  HubOperatorId;
                                String                  ClearinghouseId;
                                String                  IsHubjectCompatible;
                                String                  DynamicInfoAvailable;

                                #endregion

                                EVSEDataRecord_deltaType   = EVSEDataRecordXML.AttributeValueOrDefault(XName.Get("deltaType"),  "");
                                EVSEDataRecord_lastUpdate  = EVSEDataRecordXML.AttributeValueOrDefault(XName.Get("lastUpdate"), "");

                                if (!EVSE_Id.TryParse(EVSEDataRecordXML.ElementValue(OICPNS.EVSEData + "EvseId", "No EvseId XML tag provided!"), out EVSEId))
                                    throw new ApplicationException("Invalid EvseId XML tag provided!");

                                ChargingStationId          = EVSEDataRecordXML.ElementValueOrDefault(OICPNS.EVSEData    + "ChargingStationId",     "");
                                ChargingStationName        = EVSEDataRecordXML.ElementValueOrDefault(OICPNS.EVSEData    + "ChargingStationName",   "");
                                EnChargingStationName      = EVSEDataRecordXML.ElementValueOrDefault(OICPNS.EVSEData    + "EnChargingStationName", "");

                                #region Parse Address

                                AddressXML                 = EVSEDataRecordXML.ElementOrFail        (OICPNS.EVSEData    + "Address",               "No Address XML tags provided!");

                                if (!Country.TryParse(       AddressXML.       ElementValue         (OICPNS.CommonTypes + "Country",               "No Address Country XML tags provided!"), out CountryValue))
                                    throw new ApplicationException("Invalid Address Country XML tag provided!");

                                City                       = AddressXML.       ElementValue         (OICPNS.CommonTypes + "City",                  "No Address City XML tags provided!");
                                Street                     = AddressXML.       ElementValue         (OICPNS.CommonTypes + "Street",                "No Address Street XML tags provided!");
                                PostalCode                 = AddressXML.       ElementValueOrDefault(OICPNS.CommonTypes + "PostalCode",            "");
                                HouseNum                   = AddressXML.       ElementValueOrDefault(OICPNS.CommonTypes + "HouseNum",              "");
                                Floor                      = AddressXML.       ElementValueOrDefault(OICPNS.CommonTypes + "Floor",                 "");
                                Region                     = AddressXML.       ElementValueOrDefault(OICPNS.CommonTypes + "Region",                "");
                                Timezone                   = AddressXML.       ElementValueOrDefault(OICPNS.CommonTypes + "Timezone",              "");

                                Address = new Address(Street, HouseNum, Floor, PostalCode, "", City, CountryValue);

                                #endregion

                                #region Parse GeoCoordinates

                                GeoCoordinatesXML          = EVSEDataRecordXML.ElementOrFail        (OICPNS.EVSEData    + "GeoCoordinates",        "No GeoCoordinates XML tag provided!");
                                EVSEGoogleXML                  = GeoCoordinatesXML.Element              (OICPNS.CommonTypes + "Google");
                                EVSEDecimalDegreeXML           = GeoCoordinatesXML.Element              (OICPNS.CommonTypes + "DecimalDegree");
                                EVSEDegreeMinuteSecondsXML     = GeoCoordinatesXML.Element              (OICPNS.CommonTypes + "DegreeMinuteSeconds");

                                if (EVSEGoogleXML              == null &&
                                    EVSEDecimalDegreeXML       == null &&
                                    EVSEDegreeMinuteSecondsXML == null)
                                    throw new ApplicationException("Invalid GeoCoordinates XML tag: Should at least include one of the following XML tags Google, DecimalDegree or DegreeMinuteSeconds!");

                                if ((EVSEGoogleXML              != null && EVSEDecimalDegreeXML       != null) ||
                                    (EVSEGoogleXML              != null && EVSEDegreeMinuteSecondsXML != null) ||
                                    (EVSEDecimalDegreeXML       != null && EVSEDegreeMinuteSecondsXML != null))
                                    throw new ApplicationException("Invalid GeoCoordinates XML tag: Should only include one of the following XML tags Google, DecimalDegree or DegreeMinuteSeconds!");

                                if (EVSEGoogleXML != null)
                                {
                                    throw new NotImplementedException("GeoCoordinates Google XML parsing!");
                                }

                                if (EVSEDecimalDegreeXML != null)
                                {

                                    if (!Longitude.TryParse(EVSEDecimalDegreeXML.ElementValue(OICPNS.CommonTypes + "Longitude", "No GeoCoordinates DecimalDegree Longitude XML tag provided!"), out EVSELongitudeValue))
                                        throw new ApplicationException("Invalid Longitude XML tag provided!");

                                    if (!Latitude. TryParse(EVSEDecimalDegreeXML.ElementValue(OICPNS.CommonTypes + "Latitude",  "No GeoCoordinates DecimalDegree Latitude XML tag provided!"),  out EVSELatitudeValue))
                                        throw new ApplicationException("Invalid Latitude XML tag provided!");

                                    EVSEGeoCoordinate = new GeoCoordinate(EVSELatitudeValue, EVSELongitudeValue);

                                }

                                if (EVSEDegreeMinuteSecondsXML != null)
                                {
                                    throw new NotImplementedException("GeoCoordinates DegreeMinuteSeconds XML parsing!");
                                }

                                #endregion

                                #region Parse GeoChargingPointEntrance

                                GeoChargingPointEntranceXML    = EVSEDataRecordXML.Element              (OICPNS.EVSEData    + "GeoChargingPointEntrance");

                                if (GeoChargingPointEntranceXML != null)
                                {

                                    EntranceGoogleXML              = GeoChargingPointEntranceXML.Element    (OICPNS.CommonTypes + "Google");
                                    EntranceDecimalDegreeXML       = GeoChargingPointEntranceXML.Element    (OICPNS.CommonTypes + "DecimalDegree");
                                    EntranceDegreeMinuteSecondsXML = GeoChargingPointEntranceXML.Element    (OICPNS.CommonTypes + "DegreeMinuteSeconds");

                                    if (EntranceGoogleXML              == null &&
                                        EntranceDecimalDegreeXML       == null &&
                                        EntranceDegreeMinuteSecondsXML == null)
                                        throw new ApplicationException("Invalid GeoChargingPointEntrance XML tag: Should at least include one of the following XML tags Google, DecimalDegree or DegreeMinuteSeconds!");

                                    if ((EntranceGoogleXML             != null && EntranceDecimalDegreeXML       != null) ||
                                        (EntranceGoogleXML             != null && EntranceDegreeMinuteSecondsXML != null) ||
                                        (EntranceDecimalDegreeXML      != null && EntranceDegreeMinuteSecondsXML != null))
                                        throw new ApplicationException("Invalid GeoChargingPointEntrance XML tag: Should only include one of the following XML tags Google, DecimalDegree or DegreeMinuteSeconds!");

                                    if (EntranceGoogleXML != null)
                                    {
                                        throw new NotImplementedException("GeoChargingPointEntrance Google XML parsing!");
                                    }

                                    if (EntranceDecimalDegreeXML != null)
                                    {

                                        if (!Longitude.TryParse(EntranceDecimalDegreeXML.ElementValue(OICPNS.CommonTypes + "Longitude", "No GeoCoordinates DecimalDegree Longitude XML tag provided!"), out EntranceLongitudeValue))
                                            throw new ApplicationException("Invalid Longitude XML tag provided!");

                                        if (!Latitude. TryParse(EntranceDecimalDegreeXML.ElementValue(OICPNS.CommonTypes + "Latitude",  "No GeoCoordinates DecimalDegree Latitude XML tag provided!"),  out EntranceLatitudeValue))
                                            throw new ApplicationException("Invalid Latitude XML tag provided!");

                                        EntranceGeoCoordinate = new GeoCoordinate(EntranceLatitudeValue, EntranceLongitudeValue);

                                    }

                                    if (EntranceDegreeMinuteSecondsXML != null)
                                    {
                                        throw new NotImplementedException("GeoChargingPointEntrance DegreeMinuteSeconds XML parsing!");
                                    }

                                }

                                #endregion

                                #region Parse Plugs, ChargingFacilities, ChargingModes, AuthenticationModes, PaymentOptions

                                PlugsXML                   = EVSEDataRecordXML.     ElementOrFail    (OICPNS.EVSEData + "Plugs",                "No Plugs XML tag provided!");
                                Plugs                      = PlugsXML.              Elements         (OICPNS.EVSEData + "Plug").              SafeSelect(XMLTag => XMLTag.Value).ToArray();
                                if (!Plugs.Any())
                                    throw new ApplicationException("Invalid Plugs XML tag provided: At least one Plug XML tag is required!");

                                ChargingFacilitiesXML      = EVSEDataRecordXML.     Element          (OICPNS.EVSEData + "ChargingFacilities");
                                ChargingFacilities         = ChargingFacilitiesXML. ElementsOrDefault(OICPNS.EVSEData + "ChargingFacility").ToArray();

                                ChargingModesXML           = EVSEDataRecordXML.     Element          (OICPNS.EVSEData + "ChargingModes");
                                ChargingModes              = ChargingModesXML.      ElementsOrDefault(OICPNS.EVSEData + "ChargingMode").ToArray();

                                AuthenticationModesXML     = EVSEDataRecordXML.     ElementOrFail    (OICPNS.EVSEData + "AuthenticationModes",  "No AuthenticationModes XML tag provided!");
                                AuthenticationModes        = AuthenticationModesXML.Elements         (OICPNS.EVSEData + "AuthenticationMode").SafeSelect(XMLTag => XMLTag.Value).ToArray();
                                if (!AuthenticationModes.Any())
                                    throw new ApplicationException("Invalid AuthenticationModes XML tag provided: At least one AuthenticationMode XML tag is required!");

                                PaymentOptionsXML          = EVSEDataRecordXML.     Element          (OICPNS.EVSEData + "PaymentOptions");
                                PaymentOptions             = PaymentOptionsXML.     ElementsOrDefault(OICPNS.EVSEData + "PaymentOption").ToArray();

                                #endregion

                                #region MaxCapacity, Accessibility, HotlinePhoneNum, AdditionalInfo, EnAdditionalInfo, IsOpen24Hours, OpeningTime, HubOperatorID, ClearinghouseID, IsHubjectCompatible, DynamicInfoAvailable

                                MaxCapacity                = EVSEDataRecordXML.ElementValueOrDefault(OICPNS.EVSEData + "MaxCapacity",          "");
                                Accessibility              = EVSEDataRecordXML.ElementValue         (OICPNS.EVSEData + "Accessibility",        "No Accessibility XML tag provided!");
                                HotlinePhoneNum            = EVSEDataRecordXML.ElementValue         (OICPNS.EVSEData + "HotlinePhoneNum",      "No HotlinePhoneNum XML tag provided!");
                                AdditionalInfo             = EVSEDataRecordXML.ElementValueOrDefault(OICPNS.EVSEData + "AdditionalInfo",       "");
                                EnAdditionalInfo           = EVSEDataRecordXML.ElementValueOrDefault(OICPNS.EVSEData + "EnAdditionalInfo",     "");
                                IsOpen24Hours              = EVSEDataRecordXML.ElementValue         (OICPNS.EVSEData + "IsOpen24Hours",        "No IsOpen24Hours XML tag provided!");
                                OpeningTimes               = EVSEDataRecordXML.ElementValueOrDefault(OICPNS.EVSEData + "OpeningTime",          "");
                                HubOperatorId              = EVSEDataRecordXML.ElementValueOrDefault(OICPNS.EVSEData + "HubOperatorID",        "");
                                ClearinghouseId            = EVSEDataRecordXML.ElementValueOrDefault(OICPNS.EVSEData + "ClearinghouseID",      "");
                                IsHubjectCompatible        = EVSEDataRecordXML.ElementValue         (OICPNS.EVSEData + "IsHubjectCompatible",  "No IsHubjectCompatible XML tag provided!");
                                DynamicInfoAvailable       = EVSEDataRecordXML.ElementValue         (OICPNS.EVSEData + "DynamicInfoAvailable", "No DynamicInfoAvailable XML tag provided!");

                                #endregion

                            }

                        }

                        #endregion

                        #region HTTPResponse

                        return new HTTPResponseBuilder() {
                            HTTPStatusCode  = HTTPStatusCode.OK,
                            ContentType     = HTTPContentType.XMLTEXT_UTF8,
                            Content         = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                                                     new XElement(OICPNS.CommonTypes + "Result", "true"),

                                                                     new XElement(OICPNS.CommonTypes + "StatusCode",
                                                                         new XElement(OICPNS.CommonTypes + "Code",            "000"),
                                                                         new XElement(OICPNS.CommonTypes + "Description",     "Success"),
                                                                         new XElement(OICPNS.CommonTypes + "AdditionalInfo",  "")
                                                                     )

                                                                )).ToString().
                                                                   ToUTF8Bytes()
                        };

                        #endregion

                    }

                    #endregion

                    #region PullEVSEData

                    var PullEVSEDataXML = PullEVSEDataXMLs.FirstOrDefault();
                    if (PullEVSEDataXML != null)
                    {
                    }

                    #endregion

                    #region GetEVSEById

                    var GetEVSEByIdXML = GetEVSEByIdXMLs.FirstOrDefault();
                    if (GetEVSEByIdXML != null)
                    {
                    }

                    #endregion


                    #region PushEVSEStatus

                    var PushEVSEStatusXML = PushEVSEStatusXMLs.FirstOrDefault();
                    if (PushEVSEStatusXML != null)
                    {

                        #region Parse request parameters

                        String                  ActionType;
                        IEnumerable<XElement>   OperatorEvseStatusXML;

                        ActionType             = PushEVSEStatusXML.ElementValue  (OICPNS.EVSEStatus + "ActionType",         "No ActionType XML tag provided!");
                        OperatorEvseStatusXML  = PushEVSEStatusXML.ElementsOrFail(OICPNS.EVSEStatus + "OperatorEvseStatus", "No OperatorEvseStatus XML tags provided!");

                        foreach (var SingleOperatorEvseStatusXML in OperatorEvseStatusXML)
                        {

                            EVSEOperator_Id         OperatorId;
                            String                  OperatorName;
                            IEnumerable<XElement>   EVSEStatusRecordsXML;

                            if (!EVSEOperator_Id.TryParse(SingleOperatorEvseStatusXML.ElementValue(OICPNS.EVSEStatus + "OperatorID", "No OperatorID XML tag provided!"), out OperatorId))
                                throw new ApplicationException("Invalid OperatorID XML tag provided!");

                            OperatorName          = SingleOperatorEvseStatusXML.ElementValueOrDefault(OICPNS.EVSEStatus + "OperatorName",     "");
                            EVSEStatusRecordsXML  = SingleOperatorEvseStatusXML.ElementsOrFail       (OICPNS.EVSEStatus + "EvseStatusRecord", "No EvseStatusRecord XML tags provided!");

                            foreach (var EVSEStatusRecordXML in EVSEStatusRecordsXML)
                            {

                                EVSE_Id  EVSEId;
                                String   EVSEStatus;
                                
                                if (!EVSE_Id.TryParse(EVSEStatusRecordXML.ElementValue(OICPNS.EVSEStatus + "EvseId", "No EvseId XML tag provided!"), out EVSEId))
                                    throw new ApplicationException("Invalid EvseId XML tag provided!");

                                EVSEStatus = EVSEStatusRecordXML.ElementValue(OICPNS.EVSEStatus + "EvseStatus", "No EvseStatus XML tag provided!");

                            }

                        }

                        #endregion

                        #region HTTPResponse

                        return new HTTPResponseBuilder() {
                            HTTPStatusCode  = HTTPStatusCode.OK,
                            ContentType     = HTTPContentType.XMLTEXT_UTF8,
                            Content         = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                                                     new XElement(OICPNS.CommonTypes + "Result", "true"),

                                                                     new XElement(OICPNS.CommonTypes + "StatusCode",
                                                                         new XElement(OICPNS.CommonTypes + "Code",            "000"),
                                                                         new XElement(OICPNS.CommonTypes + "Description",     "Success"),
                                                                         new XElement(OICPNS.CommonTypes + "AdditionalInfo",  "")
                                                                     )

                                                                )).ToString().
                                                                   ToUTF8Bytes()
                        };

                        #endregion

                    }

                    #endregion

                    #region PullEVSEStatus

                    var PullEVSEStatusXML = PullEVSEStatusXMLs.FirstOrDefault();
                    if (PullEVSEStatusXML != null)
                    {

                    }

                    #endregion

                    #region PullEVSEStatusById

                    var PullEVSEStatusByIdXML = PullEVSEStatusByIdXMLs.FirstOrDefault();
                    if (PullEVSEStatusByIdXML != null)
                    {

                    }

                    #endregion


                    #region HTTPResponse: Unkown XML/SOAP message

                    return new HTTPResponseBuilder() {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        ContentType     = HTTPContentType.XMLTEXT_UTF8,
                        Content         = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                                                 new XElement(OICPNS.CommonTypes + "Result", "false"),

                                                                 new XElement(OICPNS.CommonTypes + "StatusCode",
                                                                     new XElement(OICPNS.CommonTypes + "Code",            ""),
                                                                     new XElement(OICPNS.CommonTypes + "Description",     "Unkown XML/SOAP message"),
                                                                     new XElement(OICPNS.CommonTypes + "AdditionalInfo",  "")
                                                                 ),

                                                                 new XElement(OICPNS.CommonTypes + "SessionID", "")
                                                                 //new XElement(NS.OICPv1_2CommonTypes + "PartnerSessionID", SessionID),

                                                            )).ToString().
                                                               ToUTF8Bytes()
                    };

                    #endregion

                }

                #region Catch exceptions...

                catch (Exception e)
                {

                    Log.WriteLine("Invalid XML request!");

                    GetEventSource(Semantics.DebugLog).
                        SubmitSubEvent("InvalidXMLRequest",
                                       new JObject(
                                           new JProperty("@context",      "http://emi3group.org/contexts/InvalidXMLRequest.jsonld"),
                                           new JProperty("Timestamp",     DateTime.Now.ToIso8601()),
                                           new JProperty("RemoteSocket",  HTTPRequest.RemoteSocket.ToString()),
                                           new JProperty("Exception",     e.Message),
                                           new JProperty("XMLRequest",    XMLRequest.ToString())
                                       ).ToString().
                                         Replace(Environment.NewLine, ""));

                    return new HTTPResponseBuilder() {

                        HTTPStatusCode = HTTPStatusCode.OK,
                        ContentType    = HTTPContentType.XMLTEXT_UTF8,
                        Content        = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                                                new XElement(OICPNS.CommonTypes + "Result", "false"),

                                                                new XElement(OICPNS.CommonTypes + "StatusCode",
                                                                    new XElement(OICPNS.CommonTypes + "Code",           "022"),
                                                                    new XElement(OICPNS.CommonTypes + "Description",    "Request led to an exception!"),
                                                                    new XElement(OICPNS.CommonTypes + "AdditionalInfo", e.Message)
                                                                )

                                                            )).ToString().ToUTF8Bytes()

                    };

                }

                #endregion

            };

            #endregion

            #region Register SOAP-XML Request via GET

            this.AddMethodCallback(HTTPMethod.GET,
                                   "/RNs/{RoamingNetworkId}",
                                   HTTPContentType.XMLTEXT_UTF8,
                                   HTTPDelegate: OICPServerDelegate);

            this.AddMethodCallback(HTTPMethod.GET,
                                   "/RNs/{RoamingNetworkId}",
                                   HTTPContentType.XML_UTF8,
                                   HTTPDelegate: OICPServerDelegate);

            #endregion

            #region Register SOAP-XML Request via POST

            this.AddMethodCallback(HTTPMethod.POST,
                                   "/RNs/{RoamingNetwork}",
                                   HTTPContentType.XMLTEXT_UTF8,
                                   HTTPDelegate: OICPServerDelegate);

            this.AddMethodCallback(HTTPMethod.POST,
                                   "/RNs/{RoamingNetwork}",
                                   HTTPContentType.XML_UTF8,
                                   HTTPDelegate: OICPServerDelegate);

            #endregion

            #region Register HTML+Plaintext ErrorResponse

            // HTML
            this.AddMethodCallback(HTTPMethod.GET,
                                   "/RNs/{RoamingNetwork}",
                                   HTTPContentType.HTML_UTF8,
                                   HTTPDelegate: HTTPRequest => {

                                       var RoamingNetworkId = HTTPRequest.ParsedURIParameters[0];

                                       return new HTTPResponseBuilder() {
                                           HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                           ContentType     = HTTPContentType.HTML_UTF8,
                                           Content         = ("/RNs/" + RoamingNetworkId + " is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                           Connection      = "close"
                                       };

                                   });

            // Text
            this.AddMethodCallback(HTTPMethod.GET,
                                   "/RNs/{RoamingNetwork}",
                                   HTTPContentType.TEXT_UTF8,
                                   HTTPDelegate: HTTPRequest => {

                                       var RoamingNetworkId = HTTPRequest.ParsedURIParameters[0];

                                       return new HTTPResponseBuilder() {
                                           HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                           ContentType     = HTTPContentType.HTML_UTF8,
                                           Content         = ("/RNs/" + RoamingNetworkId + " is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                           Connection      = "close"
                                       };

                                   });

            #endregion

            #endregion

        }

        #endregion

    }

}
