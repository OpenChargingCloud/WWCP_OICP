﻿/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
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

using eu.Vanaheimr.Aegir;
using System.Collections.Generic;
using System.Globalization;

#endregion

namespace org.emi3group.IO.OICP
{

    #region HubjectEVSESearchReply

    public class HubjectEVSESearchReply
    {

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
        public Byte             MaxCapacity             { get; private set; }
        public String[]         PaymentOptions          { get; private set; }

        public HubjectEVSESearchReply(XElement EvseMatch)
        {

            var EVSE                    = EvseMatch.Element(NS.OICPv1EVSESearch + "EVSE");

            var AddressXML              = EVSE.Element(NS.OICPv1EVSEData + "Address");

            this.Address                = new Address() {
                                                  Country       = Country.Parse(AddressXML.Element(NS.OICPv1CommonTypes + "Country"   ).Value),
                                                  City          = AddressXML.Element(NS.OICPv1CommonTypes + "City"      ).Value,
                                                  Street        = AddressXML.Element(NS.OICPv1CommonTypes + "Street"    ).Value,
                                                  PostalCode    = AddressXML.Element(NS.OICPv1CommonTypes + "PostalCode").Value,
                                                  FloorLevel    = AddressXML.Element(NS.OICPv1CommonTypes + "Floor"     ).Value,
                                                  //Region        = Address.Element(NS.OICPv1CommonTypes + "Region"    ).Value,
                                                  //TimeZone      = Address.Element(NS.OICPv1CommonTypes + "TimeZone"  ).Value
                                              };

            this.Distance               = Double.Parse(EvseMatch.Element(NS.OICPv1EVSESearch + "Distance").Value, NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo);

            this.EVSEId                 = EVSE.     Element(NS.OICPv1EVSEData + "EvseId"               ).Value;
            this.ChargingStationId      = EVSE.     Element(NS.OICPv1EVSEData + "ChargingStationId"    ).Value;
            this.ChargingStationName    = EVSE.     Element(NS.OICPv1EVSEData + "ChargingStationName"  ).Value;
            this.EnChargingStationName  = EVSE.     Element(NS.OICPv1EVSEData + "EnChargingStationName").Value;

            var GeoCoordinatesXML       = EVSE.Element(NS.OICPv1EVSEData + "GeoCoordinates").Element(NS.OICPv1CommonTypes + "DecimalDegree");

            this.GeoCoordinate          = new GeoCoordinate(new Latitude (Double.Parse(GeoCoordinatesXML.Element(NS.OICPv1CommonTypes + "Latitude" ).Value, NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo)),
                                                            new Longitude(Double.Parse(GeoCoordinatesXML.Element(NS.OICPv1CommonTypes + "Longitude").Value, NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo)));

            this.Plugs                  = EVSE.Element (NS.OICPv1EVSEData + "Plugs"              ).Elements(NS.OICPv1EVSEData + "Plug"              ).Select(v => v.Value).ToArray();
            this.ChargingFacilities     = EVSE.Elements(NS.OICPv1EVSEData + "ChargingFacilities" ).Elements(NS.OICPv1EVSEData + "ChargingFacility"  ).Select(v => v.Value).ToArray();
            this.ChargingModes          = EVSE.Elements(NS.OICPv1EVSEData + "ChargingModes"      ).Elements(NS.OICPv1EVSEData + "ChargingMode"      ).Select(v => v.Value).ToArray();
            this.AuthenticationModes    = EVSE.Elements(NS.OICPv1EVSEData + "AuthenticationModes").Elements(NS.OICPv1EVSEData + "AuthenticationMode").Select(v => v.Value).ToArray();
            this.MaxCapacity            = (EVSE.Element(NS.OICPv1EVSEData + "MaxCapacity") != null) ? Byte.Parse(EVSE.Element(NS.OICPv1EVSEData + "MaxCapacity").Value) : (Byte) 0;
            this.PaymentOptions         = EVSE.Elements(NS.OICPv1EVSEData + "PaymentOptions"     ).Elements(NS.OICPv1EVSEData + "PaymentOption"     ).Select(v => v.Value).ToArray();

        }

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

        public override String ToString()
        {
            return EVSEId + ", distance: " + Distance + "m";
        }

    }

    #endregion


    public static class Ext
    {

        public static IEnumerable<HubjectEVSESearchReply> ParseSearchReplies(XElement XML)
        {
            return (from   EvseMatch
                    in     XML.Descendants(NS.OICPv1EVSESearch + "EvseMatch")
                    select new HubjectEVSESearchReply(EvseMatch)).ToArray();
        }

    }

}
