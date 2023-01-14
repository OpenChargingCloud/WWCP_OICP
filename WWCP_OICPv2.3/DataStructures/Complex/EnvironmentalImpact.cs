/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The environmental impact.
    /// </summary>
    public readonly struct EnvironmentalImpact : IEquatable<EnvironmentalImpact>,
                                                 IComparable<EnvironmentalImpact>,
                                                 IComparable
    {

        #region Properties

        /// <summary>
        /// Total CO2 emited by the energy source being used by this charging station to supply energy to EV. Units are in g/kWh.
        /// </summary>
        public readonly Decimal?  CO2Emission     { get; }

        /// <summary>
        /// Total NuclearWaste emited by the energy source being used by this charging station to supply energy to EV. Units are in g/kWh.
        /// </summary>
        public readonly Decimal?  NuclearWaste    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new environmental impact.
        /// </summary>
        /// <param name="CO2Emission">Total CO2 emited by the energy source being used by this charging station to supply energy to EV. Units are in g/kWh.</param>
        /// <param name="NuclearWaste">Total NuclearWaste emited by the energy source being used by this charging station to supply energy to EV. Units are in g/kWh.</param>
        public EnvironmentalImpact(Decimal? CO2Emission    = null,
                                   Decimal? NuclearWaste   = null)
        {

            this.CO2Emission   = CO2Emission;
            this.NuclearWaste  = NuclearWaste;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#EnvironmentalImpactType

        // {
        //     "CO2Emission":   0,
        //     "NuclearWaste":  0
        // }

        #endregion

        #region (static) Parse   (JSON, CustomEnvironmentalImpactParser = null)

        /// <summary>
        /// Parse the given JSON representation of an environmental impact.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomEnvironmentalImpactParser">A delegate to parse custom environmental impacts JSON objects.</param>
        public static EnvironmentalImpact Parse(JObject                                            JSON,
                                                CustomJObjectParserDelegate<EnvironmentalImpact>?  CustomEnvironmentalImpactParser   = null)
        {

            if (TryParse(JSON,
                         out var environmentalImpact,
                         out var errorResponse,
                         CustomEnvironmentalImpactParser))
            {
                return environmentalImpact;
            }

            throw new ArgumentException("The given JSON representation of an environmental impact is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, CustomEnvironmentalImpactParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of an environmental impact.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomEnvironmentalImpactParser">A delegate to parse custom environmental impacts JSON objects.</param>
        public static EnvironmentalImpact? TryParse(JObject                                            JSON,
                                                    CustomJObjectParserDelegate<EnvironmentalImpact>?  CustomEnvironmentalImpactParser   = null)
        {

            if (TryParse(JSON,
                         out EnvironmentalImpact environmentalImpact,
                         out _,
                         CustomEnvironmentalImpactParser))
            {
                return environmentalImpact;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(JSON, out EnvironmentalImpact, out ErrorResponse, CustomEnvironmentalImpactParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an environmental impact.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="EnvironmentalImpact">The parsed environmental impact.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                  JSON,
                                       out EnvironmentalImpact  EnvironmentalImpact,
                                       out String?              ErrorResponse)

            => TryParse(JSON,
                        out EnvironmentalImpact,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an environmental impact.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="EnvironmentalImpact">The parsed environmental impact.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomEnvironmentalImpactParser">A delegate to parse custom environmental impacts JSON objects.</param>
        public static Boolean TryParse(JObject                                            JSON,
                                       out EnvironmentalImpact                            EnvironmentalImpact,
                                       out String?                                        ErrorResponse,
                                       CustomJObjectParserDelegate<EnvironmentalImpact>?  CustomEnvironmentalImpactParser)
        {

            try
            {

                EnvironmentalImpact = default;

                #region Parse CO2Emission     [optional]

                if (JSON.ParseOptional("CO2Emission",
                                       "CO2 emission",
                                       out Decimal? CO2Emission,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse NuclearWaste    [optional]

                if (JSON.ParseOptional("NuclearWaste",
                                       "nuclear waste",
                                       out Decimal? NuclearWaste,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                EnvironmentalImpact = new EnvironmentalImpact(CO2Emission,
                                                              NuclearWaste);

                if (CustomEnvironmentalImpactParser is not null)
                    EnvironmentalImpact = CustomEnvironmentalImpactParser(JSON,
                                                                          EnvironmentalImpact);

                return true;

            }
            catch (Exception e)
            {
                EnvironmentalImpact  = default;
                ErrorResponse        = "The given JSON representation of an environmental impact is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomEnvironmentalImpactSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomEnvironmentalImpactSerializer">A delegate to serialize custom time period JSON objects.</param>
        public JObject? ToJSON(CustomJObjectSerializerDelegate<EnvironmentalImpact>?  CustomEnvironmentalImpactSerializer   = null)
        {

            if (!CO2Emission. HasValue &&
                !NuclearWaste.HasValue)
            {
                return null;
            }

            var JSON = JSONObject.Create(

                           CO2Emission.HasValue
                               ? new JProperty("CO2Emission",   CO2Emission)
                               : null,

                           NuclearWaste.HasValue
                               ? new JProperty("NuclearWaste",  NuclearWaste)
                               : null

                       );

            return CustomEnvironmentalImpactSerializer is not null
                       ? CustomEnvironmentalImpactSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this environmental impact.
        /// </summary>
        public EnvironmentalImpact Clone

            => new (CO2Emission,
                    NuclearWaste);

        #endregion


        #region Operator overloading

        #region Operator == (EnvironmentalImpact1, EnvironmentalImpact2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnvironmentalImpact1">An environmental impact.</param>
        /// <param name="EnvironmentalImpact2">Another environmental impact.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EnvironmentalImpact EnvironmentalImpact1,
                                           EnvironmentalImpact EnvironmentalImpact2)

            => EnvironmentalImpact1.Equals(EnvironmentalImpact2);

        #endregion

        #region Operator != (EnvironmentalImpact1, EnvironmentalImpact2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnvironmentalImpact1">An environmental impact.</param>
        /// <param name="EnvironmentalImpact2">Another environmental impact.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EnvironmentalImpact EnvironmentalImpact1,
                                           EnvironmentalImpact EnvironmentalImpact2)

            => !EnvironmentalImpact1.Equals(EnvironmentalImpact2);

        #endregion

        #region Operator <  (EnvironmentalImpact1, EnvironmentalImpact2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnvironmentalImpact1">An environmental impact.</param>
        /// <param name="EnvironmentalImpact2">Another environmental impact.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EnvironmentalImpact EnvironmentalImpact1,
                                          EnvironmentalImpact EnvironmentalImpact2)

            => EnvironmentalImpact1.CompareTo(EnvironmentalImpact2) < 0;

        #endregion

        #region Operator <= (EnvironmentalImpact1, EnvironmentalImpact2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnvironmentalImpact1">An environmental impact.</param>
        /// <param name="EnvironmentalImpact2">Another environmental impact.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EnvironmentalImpact EnvironmentalImpact1,
                                           EnvironmentalImpact EnvironmentalImpact2)

            => EnvironmentalImpact1.CompareTo(EnvironmentalImpact2) <= 0;

        #endregion

        #region Operator >  (EnvironmentalImpact1, EnvironmentalImpact2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnvironmentalImpact1">An environmental impact.</param>
        /// <param name="EnvironmentalImpact2">Another environmental impact.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EnvironmentalImpact EnvironmentalImpact1,
                                          EnvironmentalImpact EnvironmentalImpact2)

            => EnvironmentalImpact1.CompareTo(EnvironmentalImpact2) > 0;

        #endregion

        #region Operator >= (EnvironmentalImpact1, EnvironmentalImpact2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnvironmentalImpact1">An environmental impact.</param>
        /// <param name="EnvironmentalImpact2">Another environmental impact.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EnvironmentalImpact EnvironmentalImpact1,
                                           EnvironmentalImpact EnvironmentalImpact2)

            => EnvironmentalImpact1.CompareTo(EnvironmentalImpact2) >= 0;

        #endregion

        #endregion

        #region IComparable<EnvironmentalImpact> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two environmental impacts.
        /// </summary>
        /// <param name="Object">An environmental impact to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EnvironmentalImpact environmentalImpact
                   ? CompareTo(environmentalImpact)
                   : throw new ArgumentException("The given object is not an environmental impact!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EnvironmentalImpact)

        /// <summary>
        /// Compares two environmental impacts.
        /// </summary>
        /// <param name="EnvironmentalImpact">An environmental impact to compare with.</param>
        public Int32 CompareTo(EnvironmentalImpact EnvironmentalImpact)
        {

            var c = CO2Emission.HasValue && EnvironmentalImpact.CO2Emission.HasValue
                        ? CO2Emission.Value.CompareTo(EnvironmentalImpact.CO2Emission.Value)
                        : 0;

            if (c == 0 && NuclearWaste.HasValue && EnvironmentalImpact.NuclearWaste.HasValue)
                return NuclearWaste.Value.CompareTo(EnvironmentalImpact.NuclearWaste.Value);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EnvironmentalImpact> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two environmental impacts for equality.
        /// </summary>
        /// <param name="Object">An environmental impact to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EnvironmentalImpact environmentalImpactute &&
                   Equals(environmentalImpactute);

        #endregion

        #region Equals(EnvironmentalImpact)

        /// <summary>
        /// Compares two environmental impacts for equality.
        /// </summary>
        /// <param name="EnvironmentalImpact">An environmental impact to compare with.</param>
        public Boolean Equals(EnvironmentalImpact EnvironmentalImpact)

            => CO2Emission. Equals(EnvironmentalImpact.CO2Emission) &&
               NuclearWaste.Equals(EnvironmentalImpact.NuclearWaste);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return CO2Emission. GetHashCode() * 3 ^
                       NuclearWaste.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(CO2Emission,
                             ", ",
                             NuclearWaste);

        #endregion

    }

}
