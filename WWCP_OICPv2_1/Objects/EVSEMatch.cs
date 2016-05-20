/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An OICP v2.0 EVSE search match.
    /// </summary>
    public class EVSEMatch
    {

        #region Properties

        #region Distance

        private readonly Double _Distance;

        /// <summary>
        /// The distance of the matched EVSE to the search center.
        /// </summary>
        public Double Distance
        {
            get
            {
                return _Distance;
            }
        }

        #endregion

        #region EVSEDataRecord

        private readonly EVSEDataRecord _EVSEDataRecord;

        /// <summary>
        /// The matched EVSE data record.
        /// </summary>
        public EVSEDataRecord EVSEDataRecord
        {
            get
            {
                return _EVSEDataRecord;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP v2.0 EVSE search match.
        /// </summary>
        /// <param name="Distance">The distance of the matched EVSE to the search center.</param>
        /// <param name="OperatorName">The name of an Electric Vehicle Supply Equipment Operator.</param>
        /// <param name="EVSEDataRecord">The EVSE matched data record.</param>
        public EVSEMatch(Double          Distance,
                         EVSEDataRecord  EVSEDataRecord)
        {

            #region Initial checks

            if (EVSEDataRecord == null)
                throw new ArgumentNullException("EVSEDataRecord", "The given parameter must not be null!");

            #endregion

            this._Distance        = Distance;
            this._EVSEDataRecord  = EVSEDataRecord;

        }

        #endregion


        #region (static) Parse(EVSEMatchXML, OnException = null)

        public static EVSEMatch Parse(XElement             EVSEMatchXML,
                                      OnExceptionDelegate  OnException = null)
        {

            #region Initial checks

            if (EVSEMatchXML == null)
                return null;

            #endregion

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSESearch  = "http://www.hubject.com/b2b/services/evsesearch/v2.0"
            //                   xmlns:EVSEData    = "http://www.hubject.com/b2b/services/evsedata/v2.0"
            //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            // 
            //    <soapenv:Header/>
            // 
            //    <soapenv:Body>
            //       <EVSESearch:eRoamingEvseSearchResult>
            //          <EVSESearch:EvseMatches>
            // 
            //             <!--Zero or more repetitions:-->
            //             <EVSESearch:EvseMatch>
            // 
            //                <EVSESearch:Distance>?</EVSESearch:Distance>
            // 
            //                <EVSESearch:EVSE>
            // 
            //                   <EVSEData:EvseId>?</EVSEData:EvseId>
            // 
            //                   <!--Optional:-->
            //                   <EVSEData:ChargingStationId>?</EVSEData:ChargingStationId>
            //                   <!--Optional:-->
            //                   <EVSEData:ChargingStationName>?</EVSEData:ChargingStationName>
            //                   <!--Optional:-->
            //                   <EVSEData:EnChargingStationName>?</EVSEData:EnChargingStationName>
            // 
            //                   <EVSEData:Address>
            //                      <CommonTypes:Country>?</CommonTypes:Country>
            //                      <CommonTypes:City>?</CommonTypes:City>
            //                      <CommonTypes:Street>?</CommonTypes:Street>
            //                      <!--Optional:-->
            //                      <CommonTypes:PostalCode>?</CommonTypes:PostalCode>
            //                      <!--Optional:-->
            //                      <CommonTypes:HouseNum>?</CommonTypes:HouseNum>
            //                      <!--Optional:-->
            //                      <CommonTypes:Floor>?</CommonTypes:Floor>
            //                      <!--Optional:-->
            //                      <CommonTypes:Region>?</CommonTypes:Region>
            //                      <!--Optional:-->
            //                      <CommonTypes:TimeZone>?</CommonTypes:TimeZone>
            //                   </EVSEData:Address>
            // 
            //                   <EVSEData:GeoCoordinates>
            //                      <!--You have a CHOICE of the next 3 items at this level-->
            // 
            //                      <CommonTypes:Google>
            //                         <CommonTypes:Coordinates>?</CommonTypes:Coordinates>
            //                      </CommonTypes:Google>
            // 
            //                      <CommonTypes:DecimalDegree>
            //                         <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //                         <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //                      </CommonTypes:DecimalDegree>
            // 
            //                      <CommonTypes:DegreeMinuteSeconds>
            //                         <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //                         <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //                      </CommonTypes:DegreeMinuteSeconds>
            // 
            //                   </EVSEData:GeoCoordinates>
            // 
            //                   <EVSEData:Plugs>
            //                      <!--1 or more repetitions:-->
            //                      <EVSEData:Plug>?</EVSEData:Plug>
            //                   </EVSEData:Plugs>
            // 
            //                   <!--Optional:-->
            //                   <EVSEData:ChargingFacilities>
            //                      <!--1 or more repetitions:-->
            //                      <EVSEData:ChargingFacility>?</EVSEData:ChargingFacility>
            //                   </EVSEData:ChargingFacilities>
            // 
            //                   <!--Optional:-->
            //                   <EVSEData:ChargingModes>
            //                      <!--1 or more repetitions:-->
            //                      <EVSEData:ChargingMode>?</EVSEData:ChargingMode>
            //                   </EVSEData:ChargingModes>
            // 
            //                   <EVSEData:AuthenticationModes>
            //                      <!--1 or more repetitions:-->
            //                      <EVSEData:AuthenticationMode>?</EVSEData:AuthenticationMode>
            //                   </EVSEData:AuthenticationModes>
            // 
            //                   <!--Optional:-->
            //                   <EVSEData:MaxCapacity>?</EVSEData:MaxCapacity>
            // 
            //                   <!--Optional:-->
            //                   <EVSEData:PaymentOptions>
            //                      <!--1 or more repetitions:-->
            //                      <EVSEData:PaymentOption>?</EVSEData:PaymentOption>
            //                   </EVSEData:PaymentOptions>
            // 
            //                   <EVSEData:Accessibility>?</EVSEData:Accessibility>
            //                   <EVSEData:HotlinePhoneNum>?</EVSEData:HotlinePhoneNum>
            // 
            //                   <!--Optional:-->
            //                   <EVSEData:AdditionalInfo>?</EVSEData:AdditionalInfo>
            //                   <!--Optional:-->
            //                   <EVSEData:EnAdditionalInfo>?</EVSEData:EnAdditionalInfo>
            // 
            //                   <!--Optional:-->
            //                   <EVSEData:GeoChargingPointEntrance>
            //                      <!--You have a CHOICE of the next 3 items at this level-->
            // 
            //                      <CommonTypes:Google>
            //                         <CommonTypes:Coordinates>?</CommonTypes:Coordinates>
            //                      </CommonTypes:Google>
            // 
            //                      <CommonTypes:DecimalDegree>
            //                         <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //                         <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //                      </CommonTypes:DecimalDegree>
            // 
            //                      <CommonTypes:DegreeMinuteSeconds>
            //                         <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //                         <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //                      </CommonTypes:DegreeMinuteSeconds>
            // 
            //                   </EVSEData:GeoChargingPointEntrance>
            // 
            //                   <EVSEData:IsOpen24Hours>?</EVSEData:IsOpen24Hours>
            // 
            //                   <!--Optional:-->
            //                   <EVSEData:OpeningTime>?</EVSEData:OpeningTime>
            // 
            //                   <!--Optional:-->
            //                   <EVSEData:HubOperatorID>?</EVSEData:HubOperatorID>
            //                   <!--Optional:-->
            //                   <EVSEData:ClearinghouseID>?</EVSEData:ClearinghouseID>
            // 
            //                   <EVSEData:IsHubjectCompatible>?</EVSEData:IsHubjectCompatible>
            //                   <EVSEData:DynamicInfoAvailable>?</EVSEData:DynamicInfoAvailable>
            // 
            //                </EVSESearch:EVSE>
            //             </EVSESearch:EvseMatch>
            //          </EVSESearch:EvseMatches>
            //       </EVSESearch:eRoamingEvseSearchResult>
            //    </soapenv:Body>
            // 
            // </soapenv:Envelope>

            #endregion

            try
            {

                return new EVSEMatch(EVSEMatchXML.MapValueOrFail(OICPNS.EVSESearch + "Distance",
                                                                 Double.Parse,
                                                                 "Missing Distance!"),

                                     EVSEMatchXML.MapElementOrFail(OICPNS.EVSESearch + "EVSE", "EVSE XML element was no found!",
                                                                   (EvseDataRecordXML, e) => EVSEDataRecord.Parse(EvseDataRecordXML, e),
                                                                   OnException));

            }
            catch (Exception e)
            {

                if (OnException != null)
                    OnException(DateTime.Now, EVSEMatchXML, e);

                return null;

            }

        }

        #endregion

        #region (static) Parse(EVSEMatchXMLs, OnException = null)

        public static IEnumerable<EVSEMatch> Parse(IEnumerable<XElement>  EVSEMatchXMLs,
                                                   OnExceptionDelegate    OnException = null)
        {

            #region Initial checks

            if (EVSEMatchXMLs == null)
                return new EVSEMatch[0];

            var _EVSEMatchXMLs = EVSEMatchXMLs.ToArray();

            if (_EVSEMatchXMLs.Length == 0)
                return new EVSEMatch[0];

            #endregion

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSESearch  = "http://www.hubject.com/b2b/services/evsesearch/v2.0"
            //                   xmlns:EVSEData    = "http://www.hubject.com/b2b/services/evsedata/v2.0"
            //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            // 
            //    <soapenv:Header/>
            // 
            //    <soapenv:Body>
            //       <EVSESearch:eRoamingEvseSearchResult>
            //          <EVSESearch:EvseMatches>
            // 
            //             <!--Zero or more repetitions:-->
            //             <EVSESearch:EvseMatch>
            // 
            //                <EVSESearch:Distance>?</EVSESearch:Distance>
            // 
            //                <EVSESearch:EVSE>
            //                   [...]
            //                </EVSESearch:EVSE>
            //
            //             </EVSESearch:EvseMatch>
            //
            //          </EVSESearch:EvseMatches>
            //       </EVSESearch:eRoamingEvseSearchResult>
            //    </soapenv:Body>
            // 
            // </soapenv:Envelope>

            #endregion

            return EVSEMatchXMLs.
                       Select(EVSEMatchXML     => EVSEMatch.Parse(EVSEMatchXML, OnException)).
                       Where (OperatorEvseData => OperatorEvseData != null);

        }

        #endregion

    }

}
