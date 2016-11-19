/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// OCHP XML I/O.
    /// </summary>
    public static class XML_IO
    {

        #region AsWWCPActionType(this Action)

        /// <summary>
        /// Convert an OICP v2.0 action type into a corresponding WWCP EVSE action type.
        /// </summary>
        /// <param name="ActionType">An OICP v2.0 action type.</param>
        /// <returns>The corresponding WWCP action type.</returns>
        public static WWCP.ActionType AsWWCPActionType(this ActionTypes ActionType)
        {

            switch (ActionType)
            {

                case ActionTypes.fullLoad:
                    return WWCP.ActionType.fullLoad;

                case ActionTypes.update:
                    return WWCP.ActionType.update;

                case ActionTypes.insert:
                    return WWCP.ActionType.insert;

                case ActionTypes.delete:
                    return WWCP.ActionType.delete;

                default:
                    return WWCP.ActionType.fullLoad;

            }

        }

        #endregion

        #region AsOICPActionType(this ActionType)

        /// <summary>
        /// Convert a WWCP action type into a corresponding OICP v2.0 action type.
        /// </summary>
        /// <param name="ActionType">An WWCP action type.</param>
        /// <returns>The corresponding OICP v2.0 action type.</returns>
        public static ActionTypes AsOICPActionType(this WWCP.ActionType ActionType)
        {

            switch (ActionType)
            {

                case WWCP.ActionType.fullLoad:
                    return OICPv2_1.ActionTypes.fullLoad;

                case WWCP.ActionType.update:
                    return OICPv2_1.ActionTypes.update;

                case WWCP.ActionType.insert:
                    return OICPv2_1.ActionTypes.insert;

                case WWCP.ActionType.delete:
                    return OICPv2_1.ActionTypes.delete;

                default:
                    return OICPv2_1.ActionTypes.fullLoad;

            }

        }

        #endregion


        #region ParseGeoCoordinatesXML(GeoCoordinatesXML)

        public static GeoCoordinate ParseGeoCoordinatesXML(XElement GeoCoordinatesXML)
        {

            var EVSEGoogleXML              = GeoCoordinatesXML.Element(OICPNS.CommonTypes + "Google");
            var EVSEDecimalDegreeXML       = GeoCoordinatesXML.Element(OICPNS.CommonTypes + "DecimalDegree");
            var EVSEDegreeMinuteSecondsXML = GeoCoordinatesXML.Element(OICPNS.CommonTypes + "DegreeMinuteSeconds");

            if ((EVSEGoogleXML        != null && EVSEDecimalDegreeXML       != null) ||
                (EVSEGoogleXML        != null && EVSEDegreeMinuteSecondsXML != null) ||
                (EVSEDecimalDegreeXML != null && EVSEDegreeMinuteSecondsXML != null))
                throw new ApplicationException("Invalid GeoCoordinates XML tag: Should only include one of the following XML tags Google, DecimalDegree or DegreeMinuteSeconds!");

            if (EVSEGoogleXML != null)
            {
                throw new NotImplementedException("GeoCoordinates Google XML parsing!");
            }

            if (EVSEDecimalDegreeXML != null)
            {

                Longitude LongitudeValue;
                if (!Longitude.TryParse(EVSEDecimalDegreeXML.ElementValueOrFail(OICPNS.CommonTypes + "Longitude", "No GeoCoordinates DecimalDegree Longitude XML tag provided!"), out LongitudeValue))
                    throw new ApplicationException("Invalid Longitude XML tag provided!");

                Latitude LatitudeValue;
                if (!Latitude. TryParse(EVSEDecimalDegreeXML.ElementValueOrFail(OICPNS.CommonTypes + "Latitude",  "No GeoCoordinates DecimalDegree Latitude XML tag provided!"),  out LatitudeValue))
                    throw new ApplicationException("Invalid Latitude XML tag provided!");

                return new GeoCoordinate(LatitudeValue, LongitudeValue);

            }

            if (EVSEDegreeMinuteSecondsXML != null)
            {
                throw new NotImplementedException("GeoCoordinates DegreeMinuteSeconds XML parsing!");
            }

            throw new ApplicationException("Invalid GeoCoordinates XML tag: Should at least include one of the following XML tags Google, DecimalDegree or DegreeMinuteSeconds!");

        }

        #endregion

        #region ParseAddressXML(AddressXML)

        public static Address ParseAddressXML(XElement AddressXML)
        {

            var _CountryTXT = AddressXML.ElementValueOrFail(OICPNS.CommonTypes + "Country", "Missing 'Country'-XML tag!").Trim();

            Country _Country;
            if (!Country.TryParse(_CountryTXT, out _Country))
            {

                if (_CountryTXT.ToUpper() == "UNKNOWN")
                    _Country = Country.unknown;

                else
                    throw new Exception("'" + _CountryTXT + "' is an unknown country name!");

            }

            return new Address(AddressXML.ElementValueOrFail(OICPNS.CommonTypes + "Street", "Missing 'Street'-XML tag!").Trim(),
                               AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "HouseNum", "").Trim(),
                               AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "Floor", "").Trim(),
                               AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "PostalCode", "").Trim(),
                               "",
                               I18NString.Create(Languages.unknown, AddressXML.ElementValueOrFail(OICPNS.CommonTypes + "City", "Missing 'City'-XML tag!").Trim()),
                               _Country);

            // Currently not used OICP address information!
            //var _Region       = AddressXML.       ElementValueOrDefault(OICPNS.OICPv2_0CommonTypes + "Region",     "").Trim();
            //var _Timezone     = AddressXML.       ElementValueOrDefault(OICPNS.OICPv2_0CommonTypes + "Timezone",   "").Trim();


        }

        #endregion


        #region AsDeltaType(Text)

        public static DeltaTypes AsDeltaType(this String Text)
        {

            switch (Text)
            {

                case "update":
                    return DeltaTypes.update;

                case "insert":
                    return DeltaTypes.insert;

                case "delete":
                    return DeltaTypes.delete;

                default:
                    return DeltaTypes.Unknown;

            }

        }

        #endregion

        #region AsText(this DeltaType)

        public static String AsText(this DeltaTypes DeltaType)
        {

            switch (DeltaType)
            {

                case DeltaTypes.update:
                    return "update";

                case DeltaTypes.insert:
                    return "insert";

                case DeltaTypes.delete:
                    return "delete";

                default:
                    return "Unknown";

            }

        }

        #endregion


        #region AsActionType(Text)

        public static ActionTypes AsActionType(this String Text)
        {

            switch (Text)
            {

                case "fullLoad":
                    return ActionTypes.fullLoad;

                case "update":
                    return ActionTypes.update;

                case "insert":
                    return ActionTypes.insert;

                case "delete":
                    return ActionTypes.delete;

                default:
                    return ActionTypes.Unknown;

            }

        }

        #endregion

        #region AsText(this ActionType)

        public static String AsText(this ActionTypes ActionType)
        {

            switch (ActionType)
            {

                case ActionTypes.fullLoad:
                    return "fullLoad";

                case ActionTypes.update:
                    return "update";

                case ActionTypes.insert:
                    return "insert";

                case ActionTypes.delete:
                    return "delete";

                default:
                    return "Unknown";

            }

        }

        #endregion


    }

}
