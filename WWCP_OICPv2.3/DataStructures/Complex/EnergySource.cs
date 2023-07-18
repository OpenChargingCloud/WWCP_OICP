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
    /// The energy source.
    /// </summary>
    public readonly struct EnergySource : IEquatable<EnergySource>,
                                          IComparable<EnergySource>,
                                          IComparable
    {

        #region Properties

        /// <summary>
        /// The energy type.
        /// </summary>
        [Mandatory]
        public readonly EnergyTypes  EnergyType    { get; }

        /// <summary>
        /// Percentage of EnergyType being used by the charging stations.
        /// </summary>
        [Optional]
        public readonly Byte?        Percentage    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new energy source.
        /// </summary>
        /// <param name="EnergyType">The energy type.</param>
        /// <param name="Percentage">Percentage of EnergyType being used by the charging stations.</param>
        public EnergySource(EnergyTypes  EnergyType,
                            Byte?        Percentage   = null)
        {

            this.EnergyType  = EnergyType;

            this.Percentage  = Percentage.HasValue
                                   ? Percentage < 100 ? Percentage : 100
                                   : new Byte?();

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#EnergySourceType

        // {
        //     "Energy":     "Solar",
        //     "Percentage":  80
        // }

        #endregion

        #region (static) Parse   (JSON, CustomEnergySourceParser = null)

        /// <summary>
        /// Parse the given JSON representation of an energy source.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomEnergySourceParser">A delegate to parse custom energy sources JSON objects.</param>
        public static EnergySource Parse(JObject                                     JSON,
                                         CustomJObjectParserDelegate<EnergySource>?  CustomEnergySourceParser   = null)
        {

            if (TryParse(JSON,
                         out var energySource,
                         out var errorResponse,
                         CustomEnergySourceParser))
            {
                return energySource;
            }

            throw new ArgumentException("The given JSON representation of an energy source is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, CustomEnergySourceParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of an energy source.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomEnergySourceParser">A delegate to parse custom energy sources JSON objects.</param>
        public static EnergySource? TryParse(JObject                                     JSON,
                                             CustomJObjectParserDelegate<EnergySource>?  CustomEnergySourceParser   = null)
        {

            if (TryParse(JSON,
                         out EnergySource energySource,
                         out _,
                         CustomEnergySourceParser))
            {
                return energySource;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(JSON, out EnergySource, out ErrorResponse, CustomEnergySourceParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an energy source.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="EnergySource">The parsed energy source.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject           JSON,
                                       out EnergySource  EnergySource,
                                       out String?       ErrorResponse)

            => TryParse(JSON,
                        out EnergySource,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an energy source.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="EnergySource">The parsed energy source.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomEnergySourceParser">A delegate to parse custom energy sources JSON objects.</param>
        public static Boolean TryParse(JObject                                     JSON,
                                       out EnergySource                            EnergySource,
                                       out String?                                 ErrorResponse,
                                       CustomJObjectParserDelegate<EnergySource>?  CustomEnergySourceParser)
        {

            try
            {

                EnergySource = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse EnergyType     [mandatory]

                if (!JSON.ParseMandatoryEnum("Energy",
                                             "energy type",
                                             out EnergyTypes EnergyType,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Percentage     [optional]

                if (JSON.ParseOptional("Percentage",
                                       "percentage",
                                       out Byte? Percentage,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                EnergySource = new EnergySource(EnergyType,
                                                Percentage);


                if (CustomEnergySourceParser is not null)
                    EnergySource = CustomEnergySourceParser(JSON,
                                                            EnergySource);

                return true;

            }
            catch (Exception e)
            {
                EnergySource   = default;
                ErrorResponse  = "The given JSON representation of an energy source is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomEnergySourceSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomEnergySourceSerializer">A delegate to serialize custom time period JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<EnergySource>?  CustomEnergySourceSerializer   = null)
        {

            var json = JSONObject.Create(

                           new JProperty("Energy",  EnergyType.AsString()),

                           Percentage.HasValue
                               ? new JProperty("Percentage",  Percentage.Value)
                               : null

                       );

            return CustomEnergySourceSerializer is not null
                       ? CustomEnergySourceSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public EnergySource Clone

            => new (EnergyType,
                    Percentage);

        #endregion


        #region Operator overloading

        #region Operator == (EnergySource1, EnergySource2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergySource1">An energy source.</param>
        /// <param name="EnergySource2">Another energy source.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EnergySource EnergySource1,
                                           EnergySource EnergySource2)

            => EnergySource1.Equals(EnergySource2);

        #endregion

        #region Operator != (EnergySource1, EnergySource2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergySource1">An energy source.</param>
        /// <param name="EnergySource2">Another energy source.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EnergySource EnergySource1,
                                           EnergySource EnergySource2)

            => !EnergySource1.Equals(EnergySource2);

        #endregion

        #region Operator <  (EnergySource1, EnergySource2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergySource1">An energy source.</param>
        /// <param name="EnergySource2">Another energy source.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EnergySource EnergySource1,
                                          EnergySource EnergySource2)

            => EnergySource1.CompareTo(EnergySource2) < 0;

        #endregion

        #region Operator <= (EnergySource1, EnergySource2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergySource1">An energy source.</param>
        /// <param name="EnergySource2">Another energy source.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EnergySource EnergySource1,
                                           EnergySource EnergySource2)

            => EnergySource1.CompareTo(EnergySource2) <= 0;

        #endregion

        #region Operator >  (EnergySource1, EnergySource2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergySource1">An energy source.</param>
        /// <param name="EnergySource2">Another energy source.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EnergySource EnergySource1,
                                          EnergySource EnergySource2)

            => EnergySource1.CompareTo(EnergySource2) > 0;

        #endregion

        #region Operator >= (EnergySource1, EnergySource2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergySource1">An energy source.</param>
        /// <param name="EnergySource2">Another energy source.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EnergySource EnergySource1,
                                           EnergySource EnergySource2)

            => EnergySource1.CompareTo(EnergySource2) >= 0;

        #endregion

        #endregion

        #region IComparable<EnergySource> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two energy sources.
        /// </summary>
        /// <param name="Object">An energy source to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EnergySource energySource
                   ? CompareTo(energySource)
                   : throw new ArgumentException("The given object is not an energy source!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EnergySource)

        /// <summary>
        /// Compares two energy sources.
        /// </summary>
        /// <param name="EnergySource">An energy source to compare with.</param>
        public Int32 CompareTo(EnergySource EnergySource)
        {

            var c = EnergyType.CompareTo(EnergySource.EnergyType);

            if (c == 0 && Percentage.HasValue && EnergySource.Percentage.HasValue)
                c = Percentage.Value.CompareTo(EnergySource.Percentage.Value);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EnergySource> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two energy sources for equality.
        /// </summary>
        /// <param name="Object">An energy source to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EnergySource energySource &&
                   Equals(energySource);

        #endregion

        #region Equals(EnergySource)

        /// <summary>
        /// Compares two energy sources for equality.
        /// </summary>
        /// <param name="EnergySource">An energy source to compare with.</param>
        public Boolean Equals(EnergySource EnergySource)

            => EnergyType.Equals(EnergySource.EnergyType) &&

            ((!Percentage.HasValue && !EnergySource.Percentage.HasValue) ||
              (Percentage.HasValue &&  EnergySource.Percentage.HasValue && Percentage.Value.Equals(EnergySource.Percentage.Value)));

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

                return EnergyType.GetHashCode() * 3 ^

                      (Percentage.HasValue
                           ? Percentage.Value.GetHashCode()
                           : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EnergyType,
                             Percentage.HasValue
                                 ? " " + Percentage.Value + "%"
                                 : "");

        #endregion

    }

}
