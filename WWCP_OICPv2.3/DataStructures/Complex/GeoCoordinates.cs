/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

using System.Globalization;
using System.Text.RegularExpressions;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Geographical coordinates.
    /// </summary>
    public readonly struct GeoCoordinates: IEquatable<GeoCoordinates>
    {

        #region Data

        /// <summary>
        /// A regular expression for matching sexagesimal geo coordinates.
        /// </summary>
        public static readonly Regex SexagesimalRegEx = new Regex("(^-?1?\\d{1,2})°[\\s]*(\\d{1,2})'[\\s]*(\\d{1,2}[\\.\\,]*\\d*)''$");

        #endregion

        #region Properties

        /// <summary>
        /// The latitude ("Breitengrad": South -90° to Nord +90°).
        /// </summary>
        public readonly Double                 Latitude     { get; }

        /// <summary>
        /// The longitude ("Längengrad": West -180° to East +180°).
        /// </summary>
        public readonly Double                 Longitude    { get; }

        /// <summary>
        /// The format of the geo coordinates.
        /// </summary>
        public readonly GeoCoordinatesFormats  GeoFormat    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new geographical coordinate or position on a map.
        /// </summary>
        /// <param name="Latitude">The latitude ("Breitengrad": South -90° to Nord +90°).</param>
        /// <param name="Longitude">The longitude ("Längengrad": West -180° to East +180°).</param>
        /// <param name="GeoFormat">The format of the coordinates.</param>
        public GeoCoordinates(Double                 Latitude,
                              Double                 Longitude,
                              GeoCoordinatesFormats  GeoFormat   = GeoCoordinatesFormats.DecimalDegree)
        {

            this.Latitude   = Latitude;
            this.Longitude  = Longitude;
            this.GeoFormat  = GeoFormat;

        }

        #endregion


        #region (static) Zero

        /// <summary>
        /// The zero coordinate.
        /// </summary>
        public static GeoCoordinates Zero
            => new GeoCoordinates(0, 0);

        #endregion


        #region FromLatLng(Latitude,  Longitude)

        /// <summary>
        /// Parse the given latitude and longitude values.
        /// </summary>
        /// <param name="Latitude">The latitude ("Breitengrad": South -90° to Nord +90°).</param>
        /// <param name="Longitude">The longitude ("Längengrad": West -180° to East +180°).</param>
        public static GeoCoordinates FromLatLng(Double?  Latitude,
                                                Double?  Longitude)
        {

            if (!Latitude.HasValue)
                throw new ArgumentNullException(nameof(Latitude),   "The given latitude must not be null or empty!");

            if (!Longitude.HasValue)
                throw new ArgumentNullException(nameof(Longitude),  "The given longitude must not be null or empty!");


            if (TryFromLatLng(Latitude. Value,
                              Longitude.Value,
                              out GeoCoordinates geoCoordinates,
                              out String         ErrorResponse))
            {
                return geoCoordinates;
            }

            throw new ArgumentException("Invalid geo coordinates: '" + ErrorResponse + "'!");

        }

        #endregion

        #region FromLngLat(Longitude, Latitude)

        /// <summary>
        /// Parse the given longitude and latitude values.
        /// </summary>
        /// <param name="Longitude">The longitude ("Längengrad": West -180° to East +180°).</param>
        /// <param name="Latitude">The latitude ("Breitengrad": South -90° to Nord +90°).</param>
        public static GeoCoordinates FromLngLat(Double?  Longitude,
                                                Double?  Latitude)

            => FromLatLng(Latitude,
                          Longitude);

        #endregion

        #region TryFromLatLng(Latitude,  Longitude)

        /// <summary>
        /// Try to parse the given latitude and longitude values.
        /// </summary>
        /// <param name="Latitude">The latitude ("Breitengrad": South -90° to Nord +90°).</param>
        /// <param name="Longitude">The longitude (parallel to equator).</param>
        public static GeoCoordinates? TryFromLatLng(Double?  Latitude,
                                                    Double?  Longitude)
        {

            if (Latitude. HasValue &&
                Longitude.HasValue &&
                TryFromLatLng(Latitude. Value,
                              Longitude.Value,
                              out GeoCoordinates geoCoordinates,
                              out _))
            {
                return geoCoordinates;
            }

            return default;

        }

        #endregion

        #region TryFromLngLat(Longitude, Latitude)

        /// <summary>
        /// Try to parse the given longitude and latitude values.
        /// </summary>
        /// <param name="Longitude">The longitude (parallel to equator).</param>
        /// <param name="Latitude">The latitude ("Breitengrad": South -90° to Nord +90°).</param>
        public static GeoCoordinates? TryFromLngLat(Double?  Longitude,
                                                    Double?  Latitude)

            => TryFromLatLng(Latitude,
                             Longitude);

        #endregion

        #region TryFromLatLng(Latitude,  Longitude, out GeoCoordinates, out ErrorResponse)

        /// <summary>
        /// Try to parse the given latitude and longitude values.
        /// </summary>
        /// <param name="Latitude">The latitude ("Breitengrad": South -90° to Nord +90°).</param>
        /// <param name="Longitude">The longitude ("Längengrad": West -180° to East +180°).</param>
        /// <param name="GeoCoordinate">The parsed geo coordinate.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryFromLatLng(Double              Latitude,
                                            Double              Longitude,
                                            out GeoCoordinates  GeoCoordinate,
                                            out String          ErrorResponse)
        {

            GeoCoordinate  = default;
            ErrorResponse  = default;

            if (-90 > Latitude || Latitude > 90)
            {
                ErrorResponse = "The latitude of geo coordinates must be between -90 and +90!";
                return false;
            }

            if (-180 > Longitude || Longitude > 180)
            {
                ErrorResponse = "The longitude of geo coordinates must be between -180 and +180!";
                return false;
            }

            GeoCoordinate = new GeoCoordinates(Latitude,
                                               Longitude);

            return true;

        }

        #endregion

        #region TryFromLngLat(Longitude, Latitude,  out GeoCoordinates, out ErrorResponse)

        /// <summary>
        /// Try to parse the given longitude and latitude values.
        /// </summary>
        /// <param name="Longitude">The longitude (parallel to equator).</param>
        /// <param name="Latitude">The latitude ("Breitengrad": South -90° to Nord +90°).</param>
        /// <param name="GeoCoordinate">The parsed geo coordinate.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParseLngLat(Double              Longitude,
                                             Double              Latitude,
                                             out GeoCoordinates  GeoCoordinate,
                                             out String          ErrorResponse)

            => TryFromLatLng(Latitude,
                             Longitude,
                             out GeoCoordinate,
                             out ErrorResponse);

        #endregion


        #region ParseLatLng   (Latitude,  Longitude)

        /// <summary>
        /// Parse the given latitude and longitude text representations.
        /// </summary>
        /// <param name="Latitude">The latitude ("Breitengrad": South -90° to Nord +90°).</param>
        /// <param name="Longitude">The longitude (parallel to equator).</param>
        public static GeoCoordinates ParseLatLng(String  Latitude,
                                                 String  Longitude)
        {

            #region Initial checks

            Latitude  = Latitude. Trim();
            Longitude = Longitude.Trim();

            if (Latitude.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Latitude),   "The given text representation of a latitude must not be null or empty!");

            if (Longitude.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Longitude),  "The given text representation of a longitude must not be null or empty!");

            #endregion

            if (TryParseLatLng(Latitude,
                               Longitude,
                               out GeoCoordinates?  geoCoordinates,
                               out String?          errorResponse))
            {
                return geoCoordinates!.Value;
            }

            throw new ArgumentException("Invalid text representation of geo coordinates: '" + errorResponse + "'!");

        }

        #endregion

        #region ParseLngLat   (Longitude, Latitude)

        /// <summary>
        /// Parse the given longitude and latitude text representations.
        /// </summary>
        /// <param name="Longitude">The longitude (parallel to equator).</param>
        /// <param name="Latitude">The latitude ("Breitengrad": South -90° to Nord +90°).</param>
        public static GeoCoordinates ParseLngLat(String  Longitude,
                                                 String  Latitude)

            => ParseLatLng(Latitude,
                           Longitude);

        #endregion

        #region TryParseLatLng(Latitude,  Longitude)

        /// <summary>
        /// Try to parse the given latitude and longitude text representations.
        /// </summary>
        /// <param name="Latitude">The latitude ("Breitengrad": South -90° to Nord +90°).</param>
        /// <param name="Longitude">The longitude (parallel to equator).</param>
        public static GeoCoordinates? TryParseLatLng(String  Latitude,
                                                     String  Longitude)
        {

            #region Initial checks

            Latitude  = Latitude. Trim();
            Longitude = Longitude.Trim();

            if (Latitude.IsNullOrEmpty() || Longitude.IsNullOrEmpty())
                return default;

            #endregion

            if (TryParseLatLng(Latitude,
                               Longitude,
                               out GeoCoordinates? geoCoordinates,
                               out _))
            {
                return geoCoordinates;
            }

            return default;

        }

        #endregion

        #region TryParseLngLat(Longitude, Latitude)

        /// <summary>
        /// Try to parse the given longitude and latitude text representations.
        /// </summary>
        /// <param name="Longitude">The longitude (parallel to equator).</param>
        /// <param name="Latitude">The latitude ("Breitengrad": South -90° to Nord +90°).</param>
        public static GeoCoordinates? TryParseLngLat(String  Longitude,
                                                     String  Latitude)

            => TryParseLatLng(Latitude,
                              Longitude);

        #endregion

        #region TryParseLatLng(Latitude,  Longitude, out GeoCoordinates, out ErrorResponse)

        /// <summary>
        /// Try to parse the given latitude and longitude text representations.
        /// </summary>
        /// <param name="Latitude">The latitude ("Breitengrad": South -90° to Nord +90°).</param>
        /// <param name="Longitude">The longitude ("Längengrad": West -180° to East +180°).</param>
        /// <param name="GeoCoordinate">The parsed geo coordinate.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParseLatLng(String               Latitude,
                                             String               Longitude,
                                             out GeoCoordinates?  GeoCoordinate,
                                             out String?          ErrorResponse)
        {

            GeoCoordinate  = default;
            ErrorResponse  = default;
            Latitude       = Latitude. Trim();
            Longitude      = Longitude.Trim();

            // 50.927054  11.5892372
            if (Double.TryParse(Latitude,  NumberStyles.Float, CultureInfo.InvariantCulture, out Double latitude)  &&
                Double.TryParse(Longitude, NumberStyles.Float, CultureInfo.InvariantCulture, out Double longitude) &&
                -90  <= latitude  && latitude  <=  90 &&
                -180 <= longitude && longitude <= 180)
            {

                GeoCoordinate = FromLatLng(latitude,
                                           longitude);

                return true;

            }

            // 50.927054 N  11.5892372 E
            if ((Latitude. Contains('S') || Latitude. Contains('N')) &&
                (Longitude.Contains('W') || Longitude.Contains('E')) &&
                Double.TryParse(Latitude. Replace("S", "").Replace("N", ""), NumberStyles.Float, CultureInfo.InvariantCulture, out latitude)  &&
                Double.TryParse(Longitude.Replace("W", "").Replace("E", ""), NumberStyles.Float, CultureInfo.InvariantCulture, out longitude) &&
                0 <= latitude  && latitude  <=  90 &&
                0 <= longitude && longitude <= 180)
            {

                GeoCoordinate = FromLatLng(Latitude. Contains("N") ? latitude  : -1 * latitude,
                                           Longitude.Contains("E") ? longitude : -1 * longitude);

                return true;

            }


            // 50° 55' 37.394"  11° 35' 21.254"
            var LatitudeMatch   = SexagesimalRegEx.Match(Latitude);
            var LongitudeMatch  = SexagesimalRegEx.Match(Longitude);

            if (LatitudeMatch.Success && LongitudeMatch.Success)
            {

                var latitudeDegree   = Double.Parse(LatitudeMatch. Groups[1].Value.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture);
                var latitudeMinute   = Double.Parse(LatitudeMatch. Groups[2].Value.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture) / 60;
                var latitudeSecond   = Double.Parse(LatitudeMatch. Groups[3].Value.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture) / 3600;

                var longitudeDegree  = Double.Parse(LongitudeMatch.Groups[1].Value.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture);
                var longitudeMinute  = Double.Parse(LongitudeMatch.Groups[2].Value.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture) / 60;
                var longitudeSecond  = Double.Parse(LongitudeMatch.Groups[3].Value.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture) / 3600;

                latitude             = Math.Abs(latitudeDegree)  + latitudeMinute  + latitudeSecond;
                longitude            = Math.Abs(longitudeDegree) + longitudeMinute + longitudeSecond;

                GeoCoordinate        = FromLatLng(latitudeDegree  < 0 ? -1 * latitude  : latitude,
                                                  longitudeDegree < 0 ? -1 * longitude : longitude);

                return true;

            }

            ErrorResponse = "Invalid text representation of geo coordinates: '" + Latitude + "', '" + Longitude + "'!";
            return false;

        }

        #endregion

        #region TryParseLngLat(Longitude, Latitude,  out GeoCoordinates, out ErrorResponse)

        /// <summary>
        /// Try to parse the given latitude and longitude text representations.
        /// </summary>
        /// <param name="Longitude">The longitude (parallel to equator).</param>
        /// <param name="Latitude">The latitude ("Breitengrad": South -90° to Nord +90°).</param>
        /// <param name="GeoCoordinate">The parsed geo coordinate.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParseLngLat(String               Longitude,
                                             String               Latitude,
                                             out GeoCoordinates?  GeoCoordinate,
                                             out String?          ErrorResponse)

            => TryParseLatLng(Latitude,
                              Longitude,
                              out GeoCoordinate,
                              out ErrorResponse);

        #endregion


        #region Parse   (Text)

        /// <summary>
        /// Parse the given text representation of geo coordinates.
        /// </summary>
        /// <param name="Text">A text representation of geo coordinates.</param>
        public static GeoCoordinates Parse(String Text)
        {

            if (TryParse(Text,
                         out GeoCoordinates?  geoCoordinates,
                         out String?          errorResponse))
            {
                return geoCoordinates!.Value;
            }

            throw new ArgumentException("Invalid text representation of geo coordinates: '" + errorResponse + "'!", nameof(Text));

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given text representation of geo coordinates.
        /// </summary>
        /// <param name="Text">A text representation of geo coordinates.</param>
        public static GeoCoordinates? TryParse(String Text)
        {

            if (TryParse(Text,
                         out GeoCoordinates? geoCoordinates,
                         out _))
            {
                return geoCoordinates;
            }

            return null;

        }

        #endregion

        #region TryParse(Text, out GeoCoordinates, out ErrorResponse)

        /// <summary>
        /// Try to parse the given text representation of geo coordinates.
        /// </summary>
        /// <param name="Text">A text representation of geo coordinates.</param>
        /// <param name="GeoCoordinates">The parsed geo coordinates.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(String               Text,
                                       out GeoCoordinates?  GeoCoordinates,
                                       out String?          ErrorResponse)
        {

            GeoCoordinates  = default;
            ErrorResponse   = default;
            Text            = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                ErrorResponse = "The given text representation of geo coordinates must not be null or empty!";
                return false;
            }

            var elements = Text.Split(new Char[] { ' ', ',', ';', '/' }, StringSplitOptions.RemoveEmptyEntries).
                                SafeSelect(element => element?.Trim()).
                                ToArray();

            // 9.360922      -21.568201
            // 9°21'39.32''  -21°34'23.16''
            if (elements.Length == 2)
            {

                if (TryParseLatLng(elements[0], elements[1], out GeoCoordinates, out ErrorResponse))
                    return true;

                if (TryParseLngLat(elements[0], elements[1], out GeoCoordinates, out ErrorResponse))
                    return true;

            }

            // 9° 21' 39.32''  -21° 34' 23.16''
            if (elements.Length == 6)
            {

                if (TryParseLatLng(elements[0] + elements[1] + elements[2],
                                   elements[3] + elements[4] + elements[5], out GeoCoordinates, out ErrorResponse))
                    return true;

                if (TryParseLngLat(elements[3] + elements[4] + elements[5],
                                   elements[0] + elements[1] + elements[2], out GeoCoordinates, out ErrorResponse))
                    return true;

            }

            ErrorResponse = "Invalid text representation of geo coordinates!";
            return false;

        }

        #endregion


        #region Parse   (JSON)

        /// <summary>
        /// Parse the given JSON representation of geo coordinates.
        /// </summary>
        /// <param name="JSON">A JSON representation of geo coordinates.</param>
        public static GeoCoordinates Parse(JObject JSON)
        {

            #region Initial checks

            if (JSON is null || !JSON.HasValues)
                throw new ArgumentNullException(nameof(JSON), "The given JSON representation of geo coordinates must not be null or empty!");

            #endregion

            if (TryParse(JSON,
                         out GeoCoordinates?  geoCoordinates,
                         out String?          errorResponse))
            {
                return geoCoordinates!.Value;
            }

            throw new ArgumentException("Invalid JSON representation of geo coordinates: '" + errorResponse + "'!", nameof(JSON));

        }

        #endregion

        #region TryParse(JSON)

        /// <summary>
        /// Try to parse the given JSON representation of geo coordinates.
        /// </summary>
        /// <param name="JSON">A JSON representation of geo coordinates.</param>
        public static GeoCoordinates? TryParse(JObject JSON)
        {

            if (TryParse(JSON,
                         out GeoCoordinates? geoCoordinates,
                         out _))
            {
                return geoCoordinates;
            }

            return null;

        }

        #endregion

        #region TryParse(JSON, out GeoCoordinates, out ErrorResponse)

        /// <summary>
        /// Try to parse the given JSON representation of geo coordinates.
        /// </summary>
        /// <param name="JSON">A JSON representation of geo coordinates.</param>
        /// <param name="GeoCoordinates">The parsed geo coordinates.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject              JSON,
                                       out GeoCoordinates?  GeoCoordinates,
                                       out String?          ErrorResponse)
        {

            GeoCoordinates  = default;

            if (JSON == null || !JSON.HasValues)
            {
                ErrorResponse = "The given JSON representation of geo coordinates must not be null or empty!";
                return false;
            }

            if (JSON["Google"] is JObject GoogleFormat)
            {

                if (TryParse(GoogleFormat["Coordinates"]?.Value<String>() ?? "",
                             out GeoCoordinates? geoCoordinates,
                             out ErrorResponse))
                {

                    GeoCoordinates = new GeoCoordinates(geoCoordinates!.Value.Latitude,
                                                        geoCoordinates!.Value.Longitude,
                                                        GeoCoordinatesFormats.Google);

                    return true;

                }

                ErrorResponse = "Invalid 'Google' format!";
                return false;

            }

            if (JSON["DecimalDegree"] is JObject DecimalDegreeFormat)
            {

                var latitude  = DecimalDegreeFormat[nameof(Latitude)]?. Value<String>();
                var longitude = DecimalDegreeFormat[nameof(Longitude)]?.Value<String>();

                if (latitude  != null &&
                    longitude != null &&
                    TryParseLatLng(latitude,
                                   longitude,
                                   out GeoCoordinates? geoCoordinates,
                                   out ErrorResponse))
                {

                    GeoCoordinates = new GeoCoordinates(geoCoordinates!.Value.Latitude,
                                                        geoCoordinates!.Value.Longitude,
                                                        GeoCoordinatesFormats.DecimalDegree);

                    return true;

                }

                ErrorResponse = "Invalid 'DecimalDegree' format!";
                return false;

            }

            if (JSON["DegreeMinuteSeconds"] is JObject DegreeMinuteSecondsFormat)
            {

                var latitude  = DegreeMinuteSecondsFormat[nameof(Latitude)]?. Value<String>();
                var longitude = DegreeMinuteSecondsFormat[nameof(Longitude)]?.Value<String>();

                if (latitude  != null &&
                    longitude != null &&
                    TryParseLatLng(latitude,
                                   longitude,
                                   out GeoCoordinates? geoCoordinates,
                                   out ErrorResponse))
                {

                    GeoCoordinates = new GeoCoordinates(geoCoordinates!.Value.Latitude,
                                                        geoCoordinates!.Value.Longitude,
                                                        GeoCoordinatesFormats.DegreeMinuteSeconds);

                    return true;

                }

                ErrorResponse = "Invalid 'DegreeMinuteSeconds' format!";
                return false;

            }

            ErrorResponse = "Invalid JSON representation of geo coordinates!";
            return false;

        }

        #endregion

        #region ToJSON(CustomGeoCoordinatesSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomGeoCoordinatesSerializer">A delegate to serialize custom geo coordinates JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<GeoCoordinates>?  CustomGeoCoordinatesSerializer   = null)
        {

            JObject? JSON = default;

            switch (GeoFormat)
            {

                case GeoCoordinatesFormats.Google:
                    JSON = JSONObject.Create(
                               new JProperty("Google", JSONObject.Create(
                                   new JProperty("Coordinates",  Latitude.ToString("F6") + " " + Longitude.ToString("F6").Replace(",", "."))
                               ))
                           );
                    break;


                case GeoCoordinatesFormats.DecimalDegree:

                    JSON = JSONObject.Create(
                               new JProperty("DecimalDegree", JSONObject.Create(
                                   new JProperty("Latitude",   Latitude. ToString("F6").Replace(",", ".")),
                                   new JProperty("Longitude",  Longitude.ToString("F6").Replace(",", "."))
                               ))
                           );

                    break;


                case GeoCoordinatesFormats.DegreeMinuteSeconds:

                    JSON = JSONObject.Create(
                               new JProperty("DegreeMinuteSeconds", JSONObject.Create(
                                   new JProperty("Latitude",  ToDegreeMinuteSeconds(Latitude)),
                                   new JProperty("Longitude", ToDegreeMinuteSeconds(Longitude))
                               ))
                           );

                    break;

            }

            return CustomGeoCoordinatesSerializer is not null
                       ? CustomGeoCoordinatesSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public GeoCoordinates Clone

            => new GeoCoordinates(Latitude,
                                  Longitude,
                                  GeoFormat);

        #endregion


        #region (private) ToDegreeMinuteSeconds(Value)

        private String ToDegreeMinuteSeconds(Double Value)
        {

            var grad       = (UInt32) Math.Abs(Math.Floor(Value));
            var minuteDec  = (Math.Abs(Value) - grad) * 60;
            var minute     = (UInt32) Math.Floor(minuteDec);
            var secondDec  = (minuteDec - minute) * 60;

            return String.Format("{0}{1}° {2}' {3}''", Value < 0 ? "-" : "", grad, minute, secondDec).Replace(",", ".");

        }

        #endregion

        #region DistanceKM(Target, EarthRadiusInKM = 6371)

        /// <summary>
        /// Calculate the distance between two geo coordinates in kilometers.
        /// </summary>
        /// <remarks>See also: http://www.movable-type.co.uk/scripts/latlong.html and http://en.wikipedia.org/wiki/Haversine_formula </remarks>
        /// <param name="Target">Another geo coordinate</param>
        /// <param name="EarthRadiusInKM">The currently accepted (WGS84) earth radius at the equator is 6378.137 km and 6356.752 km at the polar caps. For aviation purposes the FAI uses a radius of 6371.0 km.</param>
        public Double DistanceKM(GeoCoordinates Target, UInt32 EarthRadiusInKM = 6371)
        {

            var dLat = (Target.Latitude  - Latitude)  * (Math.PI / 180); //ToRadians
            var dLon = (Target.Longitude - Longitude) * (Math.PI / 180); //ToRadians

            var a = Math.Sin(dLat / 2)                   * Math.Sin(dLat / 2) +
                    Math.Cos(Latitude * (Math.PI / 180)) * Math.Cos(Target.Latitude * (Math.PI / 180)) *
                    Math.Sin(dLon / 2)                   * Math.Sin(dLon / 2);

            // A (surprisingly marginal) performance improvement can be obtained,
            // of course, by factoring out the terms which get squared.
            //return EarthRadiusInKM * 2 * Math.Asin(Math.Sqrt(a));

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusInKM * c;

        }

        #endregion


        #region Operator overloading

        #region Operator == (GeoCoordinates1, GeoCoordinates2)

        /// <summary>
        /// Compares two geo coordinates for equality.
        /// </summary>
        /// <param name="GeoCoordinates1">A geo coordinate.</param>
        /// <param name="GeoCoordinates2">Another geo coordinate.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GeoCoordinates GeoCoordinates1,
                                           GeoCoordinates GeoCoordinates2)

            => GeoCoordinates1.Equals(GeoCoordinates2);

        #endregion

        #region Operator != (GeoCoordinates1, GeoCoordinates2)

        /// <summary>
        /// Compares two geo coordinates for inequality.
        /// </summary>
        /// <param name="GeoCoordinates1">A geo coordinate.</param>
        /// <param name="GeoCoordinates2">Another geo coordinate.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GeoCoordinates GeoCoordinates1,
                                           GeoCoordinates GeoCoordinates2)

            => !GeoCoordinates1.Equals(GeoCoordinates2);

        #endregion

        #endregion

        #region IEquatable<GeoCoordinates> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two geo coordinates for equality.
        /// </summary>
        /// <param name="Object">Another geo coordinate.</param>
        /// <returns>True if both are equal; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is GeoCoordinates geoCoordinate &&
                   Equals(geoCoordinate);

        #endregion

        #region Equals(GeoCoordinates)

        /// <summary>
        /// Compares two geo coordinates for equality.
        /// </summary>
        /// <param name="GeoCoordinates">Another geo coordinate.</param>
        /// <returns>True if both are equal; False otherwise.</returns>
        public Boolean Equals(GeoCoordinates GeoCoordinates)

            => Latitude. Equals(GeoCoordinates.Latitude) &&
               Longitude.Equals(GeoCoordinates.Longitude);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hashcode of this object.
        /// </summary>
        /// <returns></returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return Latitude. GetHashCode() * 3 ^
                       Longitude.GetHashCode();
            }
        }

        #endregion

        #region (override) ToString(GeoFormat)

        /// <summary>
        /// Returns a string representation of the given object.
        /// </summary>
        /// <param name="GeoFormat">The format of the geo coordinates.</param>
        public String ToString(GeoCoordinatesFormats GeoFormat)
        {

            switch (GeoFormat)
            {

                case GeoCoordinatesFormats.DegreeMinuteSeconds:
                    return  ToDegreeMinuteSeconds(Latitude) +
                            " " +
                            ToDegreeMinuteSeconds(Longitude);

                default:
                    return (Latitude.ToString("F6") +
                            " " +
                            Longitude.ToString("F6")).
                            Replace(",", ".");

            }

        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Returns a string representation of the given object.
        /// </summary>
        public override String ToString()
            => ToString(GeoFormat);

        #endregion

    }

}
