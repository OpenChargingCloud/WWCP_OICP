/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/WorldWideCharging/WWCP_OICP>
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
using System.Globalization;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    public class HubjectEVSESearchReply
    {

        #region Properties

        public Double           Distance                { get; private set; }
        public String           EVSEId                  { get; private set; }
        public String           ChargingStationId       { get; private set; }
        public String           ChargingStationName     { get; private set; }
        public String           EnChargingStationName   { get; private set; }
        public Address          Address                 { get; private set; }
        public GeoCoordinate    GeoCoordinate           { get; private set; }
        public String[]         Plugs                   { get; private set; }
        public String[]         ChargingFacilities      { get; private set; }
        public String[]         ChargingModes           { get; private set; }
        public String[]         AuthenticationModes     { get; private set; }
        public UInt16           MaxCapacity             { get; private set; }
        public String[]         PaymentOptions          { get; private set; }

        #endregion

        #region Constructor(s)

        public HubjectEVSESearchReply(XElement EvseMatch)
        {

            var EVSE                    = EvseMatch.Element(OICPNS.EVSESearch + "EVSE");

            var AddressXML              = EVSE.Element(OICPNS.EVSEData + "Address");

            this.Address                = new Address(AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "Floor",       ""),
                                                      AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "Housenumber", ""),
                                                      AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "Street",      ""),
                                                      AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "PostalCode",  ""),
                                                      "",
                                                      AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "City",        ""),
                                                      Country.Parse(AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "Country", "").
                                                                               Replace("Deutschland", "Germany")) // Stupid work-around!
                                                      //Region        = Address.Element(NS.OICPv1_2CommonTypes + "Region"    ).Value,
                                                      //TimeZone      = Address.Element(NS.OICPv1_2CommonTypes + "TimeZone"  ).Value
                                                     );

            this.Distance               = Double.Parse(EvseMatch.ElementValueOrDefault(OICPNS.EVSESearch + "Distance", "0.0"), NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo);

            this.EVSEId                 = EVSE.     ElementValueOrDefault(OICPNS.EVSEData + "EvseId",                "");
            this.ChargingStationId      = EVSE.     ElementValueOrDefault(OICPNS.EVSEData + "ChargingStationId",     "");
            this.ChargingStationName    = EVSE.     ElementValueOrDefault(OICPNS.EVSEData + "ChargingStationName",   "");
            this.EnChargingStationName  = EVSE.     ElementValueOrDefault(OICPNS.EVSEData + "EnChargingStationName", "");

            var GeoCoordinatesXML       = EVSE.Element(OICPNS.EVSEData + "GeoCoordinates").Element(OICPNS.CommonTypes + "DecimalDegree");

            this.GeoCoordinate          = new GeoCoordinate(new Latitude (Double.Parse(GeoCoordinatesXML.ElementValueOrDefault(OICPNS.CommonTypes + "Latitude",  "0.0"), NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo)),
                                                            new Longitude(Double.Parse(GeoCoordinatesXML.ElementValueOrDefault(OICPNS.CommonTypes + "Longitude", "0.0"), NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo)));

            this.Plugs                  = EVSE.Element (OICPNS.EVSEData + "Plugs"              ).Elements(OICPNS.EVSEData + "Plug"              ).Select(v => v.Value).ToArray();
            this.ChargingFacilities     = EVSE.Elements(OICPNS.EVSEData + "ChargingFacilities" ).Elements(OICPNS.EVSEData + "ChargingFacility"  ).Select(v => v.Value).ToArray();
            this.ChargingModes          = EVSE.Elements(OICPNS.EVSEData + "ChargingModes"      ).Elements(OICPNS.EVSEData + "ChargingMode"      ).Select(v => v.Value).ToArray();
            this.AuthenticationModes    = EVSE.Elements(OICPNS.EVSEData + "AuthenticationModes").Elements(OICPNS.EVSEData + "AuthenticationMode").Select(v => v.Value).ToArray();

            this.MaxCapacity            = UInt16.Parse(EVSE.ElementValueOrDefault(OICPNS.EVSEData + "MaxCapacity", "0"));
            this.PaymentOptions         = EVSE.Elements(OICPNS.EVSEData + "PaymentOptions"     ).Elements(OICPNS.EVSEData + "PaymentOption"     ).Select(v => v.Value).ToArray();

        }

        #endregion


        #region (static) Parse(XML)

        /// <summary>
        /// Create a new Hubject Acknowledgement result.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        public static HubjectEVSESearchReply Parse(XElement XML)
        {
            try
            {
                return new HubjectEVSESearchReply(XML);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return EVSEId + ", distance: " + Distance + "m";
        }

        #endregion

    }

}
