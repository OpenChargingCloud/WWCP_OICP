﻿/*
 * Copyright (c) 2014 Achim Friedland <achim.friedland@graphdefined.com>
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
using System.Globalization;

using eu.Vanaheimr.Illias.Commons;
using eu.Vanaheimr.Aegir;

#endregion

namespace com.graphdefined.eMI3.IO.OICP_1_2
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

            var EVSE                    = EvseMatch.Element(NS.OICPv1_2EVSESearch + "EVSE");

            var AddressXML              = EVSE.Element(NS.OICPv1_2EVSEData + "Address");

            this.Address                = new Address() {
                                                  Country       = Country.Parse(AddressXML.ElementOrDefault(NS.OICPv1_2CommonTypes + "Country", "").
                                                                                   Replace("Deutschland", "Germany") // Stupid work-around!
                                                                               ),
                                                  City          = AddressXML.ElementOrDefault(NS.OICPv1_2CommonTypes + "City",       ""),
                                                  Street        = AddressXML.ElementOrDefault(NS.OICPv1_2CommonTypes + "Street",     ""),
                                                  PostalCode    = AddressXML.ElementOrDefault(NS.OICPv1_2CommonTypes + "PostalCode", ""),
                                                  FloorLevel    = AddressXML.ElementOrDefault(NS.OICPv1_2CommonTypes + "Floor",      ""),
                                                  //Region        = Address.Element(NS.OICPv1_2CommonTypes + "Region"    ).Value,
                                                  //TimeZone      = Address.Element(NS.OICPv1_2CommonTypes + "TimeZone"  ).Value
                                              };

            this.Distance               = Double.Parse(EvseMatch.ElementOrDefault(NS.OICPv1_2EVSESearch + "Distance", "0.0"), NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo);

            this.EVSEId                 = EVSE.     ElementOrDefault(NS.OICPv1_2EVSEData + "EvseId",                "");
            this.ChargingStationId      = EVSE.     ElementOrDefault(NS.OICPv1_2EVSEData + "ChargingStationId",     "");
            this.ChargingStationName    = EVSE.     ElementOrDefault(NS.OICPv1_2EVSEData + "ChargingStationName",   "");
            this.EnChargingStationName  = EVSE.     ElementOrDefault(NS.OICPv1_2EVSEData + "EnChargingStationName", "");

            var GeoCoordinatesXML       = EVSE.Element(NS.OICPv1_2EVSEData + "GeoCoordinates").Element(NS.OICPv1_2CommonTypes + "DecimalDegree");

            this.GeoCoordinate          = new GeoCoordinate(new Latitude (Double.Parse(GeoCoordinatesXML.ElementOrDefault(NS.OICPv1_2CommonTypes + "Latitude",  "0.0"), NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo)),
                                                            new Longitude(Double.Parse(GeoCoordinatesXML.ElementOrDefault(NS.OICPv1_2CommonTypes + "Longitude", "0.0"), NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo)));

            this.Plugs                  = EVSE.Element (NS.OICPv1_2EVSEData + "Plugs"              ).Elements(NS.OICPv1_2EVSEData + "Plug"              ).Select(v => v.Value).ToArray();
            this.ChargingFacilities     = EVSE.Elements(NS.OICPv1_2EVSEData + "ChargingFacilities" ).Elements(NS.OICPv1_2EVSEData + "ChargingFacility"  ).Select(v => v.Value).ToArray();
            this.ChargingModes          = EVSE.Elements(NS.OICPv1_2EVSEData + "ChargingModes"      ).Elements(NS.OICPv1_2EVSEData + "ChargingMode"      ).Select(v => v.Value).ToArray();
            this.AuthenticationModes    = EVSE.Elements(NS.OICPv1_2EVSEData + "AuthenticationModes").Elements(NS.OICPv1_2EVSEData + "AuthenticationMode").Select(v => v.Value).ToArray();

            this.MaxCapacity            = UInt16.Parse(EVSE.ElementOrDefault(NS.OICPv1_2EVSEData + "MaxCapacity", "0"));
            this.PaymentOptions         = EVSE.Elements(NS.OICPv1_2EVSEData + "PaymentOptions"     ).Elements(NS.OICPv1_2EVSEData + "PaymentOption"     ).Select(v => v.Value).ToArray();

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