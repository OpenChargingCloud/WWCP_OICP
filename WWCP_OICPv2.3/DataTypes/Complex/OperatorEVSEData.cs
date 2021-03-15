/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Operator EVSE data.
    /// </summary>
    public class OperatorEVSEData : IEquatable<OperatorEVSEData>,
                                    IComparable<OperatorEVSEData>,
                                    IComparable
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE data records.
        /// </summary>
        [Mandatory]
        public IEnumerable<EVSEDataRecord>  EVSEDataRecords    { get; }

        /// <summary>
        /// The unqiue identification of the EVSE operator maintaining the given EVSE data records.
        /// </summary>
        [Mandatory]
        public Operator_Id                  OperatorId         { get; }

        /// <summary>
        /// The name of the EVSE operator maintaining the given EVSE data records.
        /// </summary>
        [Mandatory]
        public String                       OperatorName       { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject                      CustomData         { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new operator EVSE data object.
        /// </summary>
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        /// <param name="OperatorId">The unqiue identification of the EVSE operator maintaining the given EVSE data records.</param>
        /// <param name="OperatorName">The name of the EVSE operator maintaining the given EVSE data records.</param>
        /// 
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public OperatorEVSEData(IEnumerable<EVSEDataRecord>  EVSEDataRecords,
                                Operator_Id                  OperatorId,
                                String                       OperatorName,

                                JObject                      CustomData   = null)
        {

            if (!EVSEDataRecords.SafeAny())
                throw new ArgumentNullException(nameof(EVSEDataRecords),  "The given enumeration of EVSE data records must not be null or empty!");

            OperatorName = OperatorName?.Trim();

            if (OperatorName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(OperatorName),     "The given EVSE operator name must not be null or empty!");

            this.EVSEDataRecords  = EVSEDataRecords;
            this.OperatorId       = OperatorId;
            this.OperatorName     = OperatorName;

            this.CustomData       = CustomData;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#OperatorEvseDataType

        // {
        //   "OperatorID":     "string",
        //   "OperatorName":   "string",
        //   "EvseDataRecord":  [
        //     {
        //       ...
        //     }
        //   ]
        // }

        #endregion

        #region (static) Parse   (JSON, CustomOperatorEVSEDataParser = null)

        /// <summary>
        /// Parse the given JSON representation of operator EVSE data.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom operator EVSE datas JSON objects.</param>
        public static OperatorEVSEData Parse(JObject                                        JSON,
                                             CustomJObjectParserDelegate<OperatorEVSEData>  CustomOperatorEVSEDataParser   = null)
        {

            if (TryParse(JSON,
                         out OperatorEVSEData operatorEVSEData,
                         out String           ErrorResponse,
                         CustomOperatorEVSEDataParser))
            {
                return operatorEVSEData;
            }

            throw new ArgumentException("The given JSON representation of operator EVSE data is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomOperatorEVSEDataParser = null)

        /// <summary>
        /// Parse the given text representation of operator EVSE data.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom operator EVSE datas JSON objects.</param>
        public static OperatorEVSEData Parse(String                                         Text,
                                             CustomJObjectParserDelegate<OperatorEVSEData>  CustomOperatorEVSEDataParser   = null)
        {

            if (TryParse(Text,
                         out OperatorEVSEData operatorEVSEData,
                         out String           ErrorResponse,
                         CustomOperatorEVSEDataParser))
            {
                return operatorEVSEData;
            }

            throw new ArgumentException("The given text representation of operator EVSE data is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out OperatorEVSEData, out ErrorResponse, CustomOperatorEVSEDataParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of operator EVSE data.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OperatorEVSEData">The parsed operator EVSE data.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject               JSON,
                                       out OperatorEVSEData  OperatorEVSEData,
                                       out String            ErrorResponse)

            => TryParse(JSON,
                        out OperatorEVSEData,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of operator EVSE data.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OperatorEVSEData">The parsed operator EVSE data.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom operator EVSE datas JSON objects.</param>
        public static Boolean TryParse(JObject                                        JSON,
                                       out OperatorEVSEData                           OperatorEVSEData,
                                       out String                                     ErrorResponse,
                                       CustomJObjectParserDelegate<OperatorEVSEData>  CustomOperatorEVSEDataParser)
        {

            try
            {

                OperatorEVSEData = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse EvseDataRecords       [mandatory]

                if (!JSON.ParseMandatoryJSON("EvseDataRecord",
                                             "EVSE data records",
                                             EVSEDataRecord.TryParse,
                                             out IEnumerable<EVSEDataRecord> EvseDataRecords,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse OperatorId            [mandatory]

                if (!JSON.ParseMandatoryEnum("OperatorID",
                                             "operator identification",
                                             out Operator_Id OperatorId,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse OperatorName          [mandatory]

                if (!JSON.ParseMandatoryText("OperatorName",
                                             "operator name",
                                             out String OperatorName,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Custom Data           [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                OperatorEVSEData = new OperatorEVSEData(EvseDataRecords,
                                                        OperatorId,
                                                        OperatorName,
                                                        CustomData);


                if (CustomOperatorEVSEDataParser != null)
                    OperatorEVSEData = CustomOperatorEVSEDataParser(JSON,
                                                                    OperatorEVSEData);

                return true;

            }
            catch (Exception e)
            {
                OperatorEVSEData  = default;
                ErrorResponse     = "The given JSON representation of operator EVSE data is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out OperatorEVSEData, out ErrorResponse, CustomOperatorEVSEDataParser = null)

        /// <summary>
        /// Try to parse the given text representation of operator EVSE data.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="OperatorEVSEData">The parsed operator EVSE data.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom operator EVSE datas JSON objects.</param>
        public static Boolean TryParse(String                                         Text,
                                       out OperatorEVSEData                           OperatorEVSEData,
                                       out String                                     ErrorResponse,
                                       CustomJObjectParserDelegate<OperatorEVSEData>  CustomOperatorEVSEDataParser)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out OperatorEVSEData,
                                out ErrorResponse,
                                CustomOperatorEVSEDataParser);

            }
            catch (Exception e)
            {
                OperatorEVSEData  = default;
                ErrorResponse     = "The given text representation of operator EVSE data is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomOperatorEVSEDataSerializer = null, CustomEVSEDataRecordSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomOperatorEVSEDataSerializer">A delegate to serialize custom operator EVSE data JSON objects.</param>
        /// <param name="CustomEVSEDataRecordSerializer">A delegate to serialize custom EVSE data record JSON objects.</param>
        /// <param name="CustomAddressSerializer">A delegate to serialize custom address JSON objects.</param>
        /// <param name="CustomChargingFacilitySerializer">A delegate to serialize custom charging facility JSON objects.</param>
        /// <param name="CustomGeoCoordinatesSerializer">A delegate to serialize custom geo coordinates JSON objects.</param>
        /// <param name="CustomEnergySourceSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomEnvironmentalImpactSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomOpeningTimesSerializer">A delegate to serialize custom opening time JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<OperatorEVSEData>     CustomOperatorEVSEDataSerializer      = null,
                              CustomJObjectSerializerDelegate<EVSEDataRecord>       CustomEVSEDataRecordSerializer        = null,
                              CustomJObjectSerializerDelegate<Address>              CustomAddressSerializer               = null,
                              CustomJObjectSerializerDelegate<ChargingFacility>     CustomChargingFacilitySerializer      = null,
                              CustomJObjectSerializerDelegate<GeoCoordinates>       CustomGeoCoordinatesSerializer        = null,
                              CustomJObjectSerializerDelegate<EnergySource>         CustomEnergySourceSerializer          = null,
                              CustomJObjectSerializerDelegate<EnvironmentalImpact>  CustomEnvironmentalImpactSerializer   = null,
                              CustomJObjectSerializerDelegate<OpeningTime>          CustomOpeningTimesSerializer          = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("EvseDataRecord",  new JArray(EVSEDataRecords.Select(evseDataRecord => evseDataRecord.ToJSON(CustomEVSEDataRecordSerializer,
                                                                                                                                      CustomAddressSerializer,
                                                                                                                                      CustomChargingFacilitySerializer,
                                                                                                                                      CustomGeoCoordinatesSerializer,
                                                                                                                                      CustomEnergySourceSerializer,
                                                                                                                                      CustomEnvironmentalImpactSerializer,
                                                                                                                                      CustomOpeningTimesSerializer)))),
                           new JProperty("OperatorID",      OperatorId.ToString()),
                           new JProperty("OperatorName",    OperatorName),

                           CustomData != null
                               ? new JProperty("CustomData",  CustomData)
                               : null

                       );

            return CustomOperatorEVSEDataSerializer != null
                       ? CustomOperatorEVSEDataSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public OperatorEVSEData Clone

            => new OperatorEVSEData(EVSEDataRecords.SafeSelect(evseDataRecord => evseDataRecord.Clone).ToArray(),
                                    OperatorId.Clone,
                                    new String(OperatorName.ToCharArray()),
                                    CustomData   != null ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None)) : null);

        #endregion


        #region Operator overloading

        #region Operator == (OperatorEVSEData1, OperatorEVSEData2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="OperatorEVSEData1">An operator EVSE data.</param>
        /// <param name="OperatorEVSEData2">Another operator EVSE data.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (OperatorEVSEData OperatorEVSEData1,
                                           OperatorEVSEData OperatorEVSEData2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(OperatorEVSEData1, OperatorEVSEData2))
                return true;

            // If one is null, but not both, return false.
            if (OperatorEVSEData1 is null || OperatorEVSEData2 is null)
                return false;

            return OperatorEVSEData1.Equals(OperatorEVSEData2);

        }

        #endregion

        #region Operator != (OperatorEVSEData1, OperatorEVSEData2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="OperatorEVSEData1">An operator EVSE data.</param>
        /// <param name="OperatorEVSEData2">Another operator EVSE data.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (OperatorEVSEData OperatorEVSEData1,
                                           OperatorEVSEData OperatorEVSEData2)

            => !(OperatorEVSEData1 == OperatorEVSEData2);

        #endregion

        #region Operator <  (OperatorEVSEData1, OperatorEVSEData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEData1">An operator EVSE data.</param>
        /// <param name="OperatorEVSEData2">Another operator EVSE data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (OperatorEVSEData OperatorEVSEData1,
                                          OperatorEVSEData OperatorEVSEData2)
        {

            if (OperatorEVSEData1 is null)
                throw new ArgumentNullException(nameof(OperatorEVSEData1), "The given OperatorEVSEData1 must not be null!");

            return OperatorEVSEData1.CompareTo(OperatorEVSEData2) < 0;

        }

        #endregion

        #region Operator <= (OperatorEVSEData1, OperatorEVSEData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEData1">An operator EVSE data.</param>
        /// <param name="OperatorEVSEData2">Another operator EVSE data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (OperatorEVSEData OperatorEVSEData1,
                                           OperatorEVSEData OperatorEVSEData2)

            => !(OperatorEVSEData1 > OperatorEVSEData2);

        #endregion

        #region Operator >  (OperatorEVSEData1, OperatorEVSEData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEData1">An operator EVSE data.</param>
        /// <param name="OperatorEVSEData2">Another operator EVSE data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (OperatorEVSEData OperatorEVSEData1,
                                          OperatorEVSEData OperatorEVSEData2)
        {

            if (OperatorEVSEData1 is null)
                throw new ArgumentNullException(nameof(OperatorEVSEData1), "The given OperatorEVSEData1 must not be null!");

            return OperatorEVSEData1.CompareTo(OperatorEVSEData2) > 0;

        }

        #endregion

        #region Operator >= (OperatorEVSEData1, OperatorEVSEData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEData1">An operator EVSE data.</param>
        /// <param name="OperatorEVSEData2">Another operator EVSE data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (OperatorEVSEData OperatorEVSEData1,
                                           OperatorEVSEData OperatorEVSEData2)

            => !(OperatorEVSEData1 < OperatorEVSEData2);

        #endregion

        #endregion

        #region IComparable<OperatorEVSEData> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is OperatorEVSEData operatorEVSEData
                   ? CompareTo(operatorEVSEData)
                   : throw new ArgumentException("The given object is not operator EVSE data!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(OperatorEVSEData)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEData">An object to compare with.</param>
        public Int32 CompareTo(OperatorEVSEData OperatorEVSEData)
        {

            if (OperatorEVSEData is null)
                throw new ArgumentNullException(nameof(OperatorEVSEData), "The given operator EVSE data must not be null!");

            var result = OperatorId.  CompareTo(OperatorEVSEData.OperatorId);

            if (result == 0)
                result = OperatorName.CompareTo(OperatorEVSEData.OperatorName);

            if (result == 0)
                result = EVSEDataRecords.Count().CompareTo(OperatorEVSEData.EVSEDataRecords.Count());

            if (result == 0)
                result = EVSEDataRecords.Select(evseDataRecord => evseDataRecord.Id.ToString()).AggregateWith("-").
                             CompareTo(OperatorEVSEData.EVSEDataRecords.Select(evseDataRecord => evseDataRecord.Id.ToString()).AggregateWith("-"));

            return result;

        }

        #endregion

        #endregion

        #region IEquatable<OperatorEVSEData> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is OperatorEVSEData operatorEVSEData &&
                   Equals(operatorEVSEData);

        #endregion

        #region Equals(OperatorEVSEData)

        /// <summary>
        /// Compares two operator EVSE datas for equality.
        /// </summary>
        /// <param name="OperatorEVSEData">A operator EVSE data to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(OperatorEVSEData OperatorEVSEData)

            => !(OperatorEVSEData is null) &&

                 OperatorId.  Equals(OperatorEVSEData.OperatorId)   &&
                 OperatorName.Equals(OperatorEVSEData.OperatorName) &&

                 EVSEDataRecords.Count().Equals(OperatorEVSEData.EVSEDataRecords.Count()) &&
                 EVSEDataRecords.All(evseDataRecord => OperatorEVSEData.EVSEDataRecords.Contains(evseDataRecord));

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

                return OperatorId.GetHashCode() * 5 ^

                       (OperatorName.IsNotNullOrEmpty()
                            ? OperatorName.   GetHashCode()
                            : 0) * 3 ^

                       (EVSEDataRecords.Any()
                            ? EVSEDataRecords.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(OperatorId,
                             OperatorName.IsNotNullOrEmpty() ? ", " + OperatorName : "",
                             ", ",  EVSEDataRecords.Count(), " EVSE data record(s)");

        #endregion

    }

}
