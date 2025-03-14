﻿/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// A charging facility.
    /// </summary>
    public readonly struct ChargingFacility : IEquatable<ChargingFacility>,
                                              IComparable<ChargingFacility>,
                                              IComparable
    {

        #region Properties

        /// <summary>
        /// The power type  of the charging facility, e.g. AC or DC.
        /// </summary>
        [Mandatory]
        public readonly PowerTypes                  PowerType        { get; }

        /// <summary>
        /// Optional voltage of the charging facility.
        /// </summary>
        [Optional]
        public readonly UInt32?                     Voltage          { get; }

        /// <summary>
        /// Optional amperage of the charging facility.
        /// </summary>
        [Optional]
        public readonly UInt32?                     Amperage         { get; }

        /// <summary>
        /// The power of the charging facility [kW].
        /// </summary>
        [Mandatory]
        public readonly UInt32                      Power            { get; }

        /// <summary>
        /// Optional enumeration of supported charging modes.
        /// </summary>
        [Optional]
        public readonly IEnumerable<ChargingModes>  ChargingModes    { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public readonly JObject?                    CustomData       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging facility.
        /// </summary>
        /// <param name="PowerType">The power type  of the charging facility, e.g. AC or DC.</param>
        /// <param name="Power">The power of the charging facility [kW].</param>
        /// 
        /// <param name="Voltage">Optional voltage of the charging facility.</param>
        /// <param name="Amperage">Optional amperage of the charging facility.</param>
        /// <param name="ChargingModes">Optional enumeration of supported charging modes.</param>
        /// 
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public ChargingFacility(PowerTypes                   PowerType,
                                UInt32                       Power,

                                UInt32?                      Voltage         = null,
                                UInt32?                      Amperage        = null,
                                IEnumerable<ChargingModes>?  ChargingModes   = null,

                                JObject?                     CustomData      = null)

        {

            this.PowerType      = PowerType;
            this.Power          = Power;

            this.Voltage        = Voltage;
            this.Amperage       = Amperage;
            this.ChargingModes  = ChargingModes?.Distinct() ?? [];

            this.CustomData     = CustomData;


            unchecked
            {

                hashCode = this.PowerType.    GetHashCode()       * 11 ^
                           this.Power.        GetHashCode()       *  7 ^
                          (this.Voltage?.     GetHashCode() ?? 0) *  5 ^
                          (this.Amperage?.    GetHashCode() ?? 0) *  3 ^
                           this.ChargingModes.CalcHashCode();

            }

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#ChargingFacilityType

        // {
        //   "PowerType":  "AC_1_PHASE",
        //   "Power":       50,
        //   "Voltage":     400,
        //   "Amperage":    125,
        //   "ChargingModes": [
        //     "Mode_1"
        //   ],
        // }

        #endregion

        #region (static) Parse   (JSON, CustomChargingFacilityParser = null)

        /// <summary>
        /// Parse the given JSON representation of a charging facility.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomChargingFacilityParser">A delegate to parse custom charging facility JSON objects.</param>
        public static ChargingFacility Parse(JObject                                         JSON,
                                             CustomJObjectParserDelegate<ChargingFacility>?  CustomChargingFacilityParser   = null)
        {

            if (TryParse(JSON,
                         out var chargingFacility,
                         out var errorResponse,
                         CustomChargingFacilityParser))
            {
                return chargingFacility;
            }

            throw new ArgumentException("The given JSON representation of a charging facility is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, CustomChargingFacilityParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a charging facility.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomChargingFacilityParser">A delegate to parse custom charging facilitys JSON objects.</param>
        public static ChargingFacility? TryParse(JObject                                         JSON,
                                                 CustomJObjectParserDelegate<ChargingFacility>?  CustomChargingFacilityParser   = null)
        {

            if (TryParse(JSON,
                         out var chargingFacility,
                         out _,
                         CustomChargingFacilityParser))
            {
                return chargingFacility;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(JSON, ..., out ChargingFacility, out ErrorResponse, CustomChargingFacilityParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a charging facility.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ChargingFacility">The parsed charging facility.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                    JSON,
                                       [NotNullWhen(true)]  out ChargingFacility  ChargingFacility,
                                       [NotNullWhen(false)] out String?           ErrorResponse)

            => TryParse(JSON,
                        out ChargingFacility,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a charging facility.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ChargingFacility">The parsed charging facility.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomChargingFacilityParser">A delegate to parse custom charging facilitys JSON objects.</param>
        public static Boolean TryParse(JObject                                         JSON,
                                       [NotNullWhen(true)]  out ChargingFacility       ChargingFacility,
                                       [NotNullWhen(false)] out String?                ErrorResponse,
                                       CustomJObjectParserDelegate<ChargingFacility>?  CustomChargingFacilityParser)
        {

            try
            {

                ChargingFacility = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse PowerType         [mandatory]

                if (!JSON.ParseMandatory("PowerType",
                                         "power type",
                                         PowerTypesExtensions.TryParse,
                                         out PowerTypes PowerType,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Power             [mandatory => optional, because of Hubject data quality issues!]

                if (!JSON.ParseMandatory("Power",
                                         "power",
                                         out UInt32 Power,
                                         out ErrorResponse))
                {
                    //return false;
                    Power = 0;
                }

                #endregion

                #region Parse Voltage           [optional]

                if (JSON.ParseOptional("Voltage",
                                       "voltage",
                                       out UInt32? Voltage,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse Amperage          [optional]

                if (JSON.ParseOptional("Amperage",
                                       "amperage",
                                       out UInt32? Amperage,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ChargingModes     [optional]

                if (JSON.ParseOptionalHashSet("ChargingModes",
                                              "charging modes",
                                              ChargingModesExtensions.TryParse,
                                              out HashSet<ChargingModes> ChargingModes,
                                              out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CustomData        [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                ChargingFacility = new ChargingFacility(
                                       PowerType,
                                       Power,
                                       Voltage,
                                       Amperage,
                                       ChargingModes,
                                       customData
                                   );

                if (CustomChargingFacilityParser is not null)
                    ChargingFacility = CustomChargingFacilityParser(JSON,
                                                                    ChargingFacility);

                return true;

            }
            catch (Exception e)
            {
                ChargingFacility  = default;
                ErrorResponse     = "The given JSON representation of a charging facility is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomChargingFacilitySerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomChargingFacilitySerializer">A delegate to serialize custom charging facility JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ChargingFacility>?  CustomChargingFacilitySerializer   = null)
        {

            var json = JSONObject.Create(

                           new JProperty("PowerType",              PowerType.AsString()),
                           new JProperty("Power",                  Power),

                           Voltage.HasValue
                               ? new JProperty("Voltage",          Voltage. Value)
                               : null,

                           Amperage.HasValue
                               ? new JProperty("Amperage",         Amperage.Value)
                               : null,

                           ChargingModes.SafeAny()
                               ? new JProperty("ChargingModes",    new JArray(ChargingModes.Select(chargingMode => chargingMode.AsString())))
                               : null,

                           CustomData?.HasValues == true
                               ? new JProperty("CustomData",       CustomData)
                               : null

                );

            return CustomChargingFacilitySerializer is not null
                       ? CustomChargingFacilitySerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this charging facility.
        /// </summary>
        public ChargingFacility Clone()

            => new (

                   PowerType,
                   Power,
                   Voltage,
                   Amperage,
                   ChargingModes,

                   CustomData is not null
                       ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (ChargingFacility1, ChargingFacility2)

        /// <summary>
        /// Compares two charging facilityes for equality.
        /// </summary>
        /// <param name="ChargingFacility1">A charging facility.</param>
        /// <param name="ChargingFacility2">Another charging facility.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingFacility ChargingFacility1,
                                           ChargingFacility ChargingFacility2)

            => ChargingFacility1.Equals(ChargingFacility2);

        #endregion

        #region Operator != (ChargingFacility1, ChargingFacility2)

        /// <summary>
        /// Compares two charging facilityes for inequality.
        /// </summary>
        /// <param name="ChargingFacility1">A charging facility.</param>
        /// <param name="ChargingFacility2">Another charging facility.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingFacility ChargingFacility1,
                                           ChargingFacility ChargingFacility2)

            => !ChargingFacility1.Equals(ChargingFacility2);

        #endregion

        #region Operator <  (ChargingFacility1, ChargingFacility2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingFacility1">A charging facility.</param>
        /// <param name="ChargingFacility2">Another charging facility.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingFacility ChargingFacility1,
                                          ChargingFacility ChargingFacility2)

            => ChargingFacility1.CompareTo(ChargingFacility2) < 0;

        #endregion

        #region Operator <= (ChargingFacility1, ChargingFacility2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingFacility1">A charging facility.</param>
        /// <param name="ChargingFacility2">Another charging facility.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingFacility ChargingFacility1,
                                           ChargingFacility ChargingFacility2)

            => ChargingFacility1.CompareTo(ChargingFacility2) <= 0;

        #endregion

        #region Operator >  (ChargingFacility1, ChargingFacility2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingFacility1">A charging facility.</param>
        /// <param name="ChargingFacility2">Another charging facility.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingFacility ChargingFacility1,
                                          ChargingFacility ChargingFacility2)

            => ChargingFacility1.CompareTo(ChargingFacility2) > 0;

        #endregion

        #region Operator >= (ChargingFacility1, ChargingFacility2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingFacility1">A charging facility.</param>
        /// <param name="ChargingFacility2">Another charging facility.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingFacility ChargingFacility1,
                                           ChargingFacility ChargingFacility2)

            => ChargingFacility1.CompareTo(ChargingFacility2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingFacility> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging facilities.
        /// </summary>
        /// <param name="Object">A charging facility to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingFacility chargingFacility
                   ? CompareTo(chargingFacility)
                   : throw new ArgumentException("The given object is not a charging facility!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingFacility)

        /// <summary>
        /// Compares two charging facilities.
        /// </summary>
        /// <param name="ChargingFacility">A charging facility to compare with.</param>
        public Int32 CompareTo(ChargingFacility ChargingFacility)
        {

            var c =  PowerType.     CompareTo(ChargingFacility.PowerType);

            if (c == 0)
                c =  Power.         CompareTo(ChargingFacility.Power);

            if (c == 0)
                c = (Voltage  ?? 0).CompareTo(ChargingFacility.Voltage  ?? 0);

            if (c == 0)
                c = (Amperage ?? 0).CompareTo(ChargingFacility.Amperage ?? 0);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingFacility> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging facilities for equality.
        /// </summary>
        /// <param name="Object">A charging facility to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingFacility chargingFacility &&
                   Equals(chargingFacility);

        #endregion

        #region Equals(ChargingFacility)

        /// <summary>
        /// Compares two charging facilities for equality.
        /// </summary>
        /// <param name="ChargingFacility">A charging facility to compare with.</param>
        public Boolean Equals(ChargingFacility ChargingFacility)

            => PowerType.            Equals(ChargingFacility.PowerType)             &&
               Power.                Equals(ChargingFacility.Power)                 &&
               Voltage.              Equals(ChargingFacility.Voltage)               &&
               Amperage.             Equals(ChargingFacility.Amperage)              &&
               ChargingModes.Count().Equals(ChargingFacility.ChargingModes.Count()) &&
               ChargingModes.All(chargingMode => ChargingFacility.ChargingModes.Contains(chargingMode));

        #endregion

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
            => hashCode;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(

                   $"{PowerType}, {Power} kW",

                   Voltage.HasValue
                       ? $", {Voltage} V"
                       : "",

                   Amperage.HasValue
                       ? $", {Amperage} A"
                       : ""

               );

        #endregion

    }

}
