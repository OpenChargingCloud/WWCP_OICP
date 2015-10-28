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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// OICP v2.0 XML management methods.
    /// </summary>
    public static class XMLMethods
    {

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

    }

}
