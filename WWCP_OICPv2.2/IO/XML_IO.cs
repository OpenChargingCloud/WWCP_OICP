/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// OICP XML I/O.
    /// </summary>
    public static partial class XML_IO
    {

        #region ParseGeoCoordinatesXML(GeoCoordinatesXML, OnException = null)

        public static GeoCoordinate ParseGeoCoordinatesXML(XElement             GeoCoordinatesXML,
                                                           OnExceptionDelegate  OnException  = null)
        {

            var EVSEGoogleXML              = GeoCoordinatesXML.Element(OICPNS.CommonTypes + "Google");
            var EVSEDecimalDegreeXML       = GeoCoordinatesXML.Element(OICPNS.CommonTypes + "DecimalDegree");
            var EVSEDegreeMinuteSecondsXML = GeoCoordinatesXML.Element(OICPNS.CommonTypes + "DegreeMinuteSeconds");

            if ((EVSEGoogleXML        != null && EVSEDecimalDegreeXML       != null) ||
                (EVSEGoogleXML        != null && EVSEDegreeMinuteSecondsXML != null) ||
                (EVSEDecimalDegreeXML != null && EVSEDegreeMinuteSecondsXML != null))
            {
                throw new ApplicationException("Invalid GeoCoordinates XML tag: Should only include one of the following XML tags Google, DecimalDegree or DegreeMinuteSeconds!");
            }

            if (EVSEGoogleXML != null)
            {

                var Coordinates = EVSEGoogleXML.Value.Split(new Char[] { ',' }, StringSplitOptions.None);

                if (!Longitude.TryParse(Coordinates[0],
                                        out Longitude LongitudeValue))
                {
                    throw new ApplicationException("Invalid longitude in Google format provided!");
                }

                if (!Latitude.TryParse(Coordinates[1],
                                       out Latitude LatitudeValue))
                {
                    throw new ApplicationException("Invalid latitude in Google format provided!");
                }

                return new GeoCoordinate(LatitudeValue,
                                         LongitudeValue);

            }

            if (EVSEDecimalDegreeXML != null)
            {

                if (!Longitude.TryParse(EVSEDecimalDegreeXML.ElementValueOrFail(OICPNS.CommonTypes + "Longitude",
                                                                                "No GeoCoordinates DecimalDegree Longitude XML tag provided!"),
                                        out Longitude LongitudeValue))
                {
                    throw new ApplicationException("Invalid Longitude XML tag provided!");
                }

                if (!Latitude.TryParse(EVSEDecimalDegreeXML.ElementValueOrFail(OICPNS.CommonTypes + "Latitude",
                                                                               "No GeoCoordinates DecimalDegree Latitude XML tag provided!"),
                                       out Latitude LatitudeValue))
                {
                    throw new ApplicationException("Invalid Latitude XML tag provided!");
                }

                return new GeoCoordinate(LatitudeValue,
                                         LongitudeValue);

            }

            if (EVSEDegreeMinuteSecondsXML != null)
                throw new NotImplementedException("GeoCoordinates DegreeMinuteSeconds XML parsing!");

            throw new ApplicationException("Invalid GeoCoordinates XML tag: Should at least include one of the following XML tags Google, DecimalDegree or DegreeMinuteSeconds!");

        }

        #endregion


        public static PlugTypes Reduce(this IEnumerable<PlugTypes> OICPPlugTypes)
        {

            var _PlugTypes = PlugTypes.Unspecified;

            foreach (var _PlugType in OICPPlugTypes)
                _PlugTypes |= _PlugType;

            return _PlugTypes;

        }

        public static IEnumerable<PlugTypes> ToEnumeration(this PlugTypes e)

            => Enum.GetValues(typeof(PlugTypes)).
                    Cast<PlugTypes>().
                    Where(flag => e.HasFlag(flag) && flag != PlugTypes.Unspecified);




        public static ChargingModes Reduce(this IEnumerable<ChargingModes> ChargingModes)
        {

            var _ChargingModes = OICPv2_2.ChargingModes.Unspecified;

            foreach (var _ChargingMode in ChargingModes)
                _ChargingModes |= _ChargingMode;

            return _ChargingModes;

        }

        public static IEnumerable<ChargingModes> ToEnumeration(this ChargingModes e)
        {

            return Enum.GetValues(typeof(ChargingModes)).
                        Cast<ChargingModes>().
                        Where(flag => e.HasFlag(flag) && flag != ChargingModes.Unspecified);

        }



        public static AuthenticationModes Reduce(this IEnumerable<AuthenticationModes> AuthenticationModes)
        {

            var _AuthenticationModes = OICPv2_2.AuthenticationModes.Unknown;

            foreach (var _AuthenticationMode in AuthenticationModes)
                _AuthenticationModes |= _AuthenticationMode;

            return _AuthenticationModes;

        }

        public static IEnumerable<AuthenticationModes> ToEnumeration(this AuthenticationModes e)

            => Enum.GetValues(typeof(AuthenticationModes)).
                    Cast<AuthenticationModes>().
                    Where(flag => e.HasFlag(flag) && flag != AuthenticationModes.Unknown);


        public static PaymentOptions Reduce(this IEnumerable<PaymentOptions> OICPPaymentOptions)
        {

            var _PaymentOptions = PaymentOptions.Unspecified;

            foreach (var PaymentOption in OICPPaymentOptions)
                _PaymentOptions |= PaymentOption;

            return _PaymentOptions;

        }

        public static IEnumerable<PaymentOptions> ToEnumeration(this PaymentOptions e)
        {

            return Enum.GetValues(typeof(PaymentOptions)).
                        Cast<PaymentOptions>().
                        Where(flag => e.HasFlag(flag) && flag != PaymentOptions.Unspecified);

        }



        public static AccessibilityTypes Reduce(this IEnumerable<AccessibilityTypes> OICPAccessibilityTypes)
        {

            var _AccessibilityTypes = AccessibilityTypes.Unspecified;

            foreach (var _AccessibilityType in OICPAccessibilityTypes)
                _AccessibilityTypes |= _AccessibilityType;

            return _AccessibilityTypes;

        }

        public static IEnumerable<AccessibilityTypes> ToEnumeration(this AccessibilityTypes e)
        {

            return Enum.GetValues(typeof(AccessibilityTypes)).
                        Cast<AccessibilityTypes>().
                        Where(flag => e.HasFlag(flag) && flag != AccessibilityTypes.Unspecified);

        }


        public static ValueAddedServices Reduce(this IEnumerable<ValueAddedServices> OICPPlugTypes)
        {

            var _PlugTypes = ValueAddedServices.None;

            foreach (var _PlugType in OICPPlugTypes)
                _PlugTypes |= _PlugType;

            return _PlugTypes;

        }

        public static IEnumerable<ValueAddedServices> ToEnumeration(this ValueAddedServices e)

            => Enum.GetValues(typeof(ValueAddedServices)).
                    Cast<ValueAddedServices>().
                    Where(flag => e.HasFlag(flag) && flag != ValueAddedServices.None);


    }

}
