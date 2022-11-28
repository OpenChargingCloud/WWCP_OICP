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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Provider authentication data.
    /// </summary>
    public class ProviderAuthenticationData : IEquatable<ProviderAuthenticationData>,
                                              IComparable<ProviderAuthenticationData>,
                                              IComparable
    {

        #region Properties

        /// <summary>
        /// An enumeration of user identification data records.
        /// </summary>
        [Mandatory]
        public IEnumerable<Identification>  Identifications    { get; }

        /// <summary>
        /// The unqiue identification of the e-mobility provider maintaining the given user identification data records.
        /// </summary>
        [Mandatory]
        public Provider_Id                  ProviderId         { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject?                     CustomData         { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create new provider authentication data.
        /// </summary>
        /// <param name="Identifications">An enumeration of user identification data records.</param>
        /// <param name="ProviderId">The unqiue identification of the e-mobility provider maintaining the given user identification data records.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public ProviderAuthenticationData(IEnumerable<Identification>  Identifications,
                                          Provider_Id                  ProviderId,
                                          JObject?                     CustomData   = null)
        {

            if (!Identifications.SafeAny())
                throw new ArgumentNullException(nameof(Identifications),  "The given enumeration of user identification data records must not be null or empty!");

            this.Identifications  = Identifications;
            this.ProviderId       = ProviderId;
            this.CustomData       = CustomData;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/03_EMP_Data_Types.asciidoc#18-providerauthenticationdatatype

        // {
        //   "AuthenticationDataRecord": [
        //     {
        //       "Identification": {
        //         ...
        //       }
        //     }
        //   ],
        //   "ProviderID": "DE-GDF"
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomProviderAuthenticationDataParser = null)

        /// <summary>
        /// Parse the given JSON representation of provider user identification data.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomProviderAuthenticationDataParser">A delegate to parse custom provider user identification datas JSON objects.</param>
        public static ProviderAuthenticationData Parse(JObject                                                   JSON,
                                                       CustomJObjectParserDelegate<ProviderAuthenticationData>?  CustomProviderAuthenticationDataParser   = null)
        {

            if (TryParse(JSON,
                         out var providerAuthenticationData,
                         out var errorResponse,
                         CustomProviderAuthenticationDataParser))
            {
                return providerAuthenticationData!;
            }

            throw new ArgumentException("The given JSON representation of provider user identification data is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out ProviderAuthenticationData, out ErrorResponse, CustomProviderAuthenticationDataParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of provider user identification data.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ProviderAuthenticationData">The parsed provider user identification data.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                          JSON,
                                       out ProviderAuthenticationData?  ProviderAuthenticationData,
                                       out String?                      ErrorResponse)

            => TryParse(JSON,
                        out ProviderAuthenticationData,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of provider user identification data.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ProviderAuthenticationData">The parsed provider user identification data.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomProviderAuthenticationDataParser">A delegate to parse custom provider user identification datas JSON objects.</param>
        public static Boolean TryParse(JObject                                                   JSON,
                                       out ProviderAuthenticationData?                           ProviderAuthenticationData,
                                       out String?                                               ErrorResponse,
                                       CustomJObjectParserDelegate<ProviderAuthenticationData>?  CustomProviderAuthenticationDataParser)
        {

            try
            {

                ProviderAuthenticationData = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse AuthenticationDataRecord    [mandatory]

                var Identifications = new List<Identification>();

                if (JSON["AuthenticationDataRecord"] is JArray authenticationDataRecordsJSON)
                {
                    foreach (var authenticationDataRecordJSON in authenticationDataRecordsJSON)
                    {
                        if (authenticationDataRecordJSON is JObject authenticationDataRecordJObject)
                        {
                            if (authenticationDataRecordJObject.ParseMandatoryJSON2("Identification",
                                                                                    "user identification data record",
                                                                                    Identification.TryParse,
                                                                                    out Identification? identification,
                                                                                    out ErrorResponse))
                            {
                                Identifications.Add(identification!);
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }

                #endregion

                #region Parse ProviderId                  [mandatory]

                if (!JSON.ParseMandatory("ProviderID",
                                         "provider identification",
                                         Provider_Id.TryParse,
                                         out Provider_Id ProviderId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse CustomData                  [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                ProviderAuthenticationData = new ProviderAuthenticationData(Identifications,
                                                                            ProviderId,
                                                                            customData);


                if (CustomProviderAuthenticationDataParser is not null)
                    ProviderAuthenticationData = CustomProviderAuthenticationDataParser(JSON,
                                                                                        ProviderAuthenticationData);

                return true;

            }
            catch (Exception e)
            {
                ProviderAuthenticationData  = default;
                ErrorResponse     = "The given JSON representation of provider user identification data is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomProviderAuthenticationDataSerializer = null, CustomIdentificationSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomProviderAuthenticationDataSerializer">A delegate to serialize custom provider user identification data JSON objects.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ProviderAuthenticationData>?  CustomProviderAuthenticationDataSerializer   = null,
                              CustomJObjectSerializerDelegate<Identification>?              CustomIdentificationSerializer               = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("AuthenticationDataRecord",  new JArray(
                               Identifications.Select(identification => JSONObject.Create(
                                   new JProperty("Identification", identification.ToJSON(CustomIdentificationSerializer))
                               ))
                           )),

                           new JProperty("ProviderID",        ProviderId.ToString()),

                           CustomData?.HasValues == true
                               ? new JProperty("CustomData",  CustomData)
                               : null

                       );

            return CustomProviderAuthenticationDataSerializer is not null
                       ? CustomProviderAuthenticationDataSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public ProviderAuthenticationData Clone

            => new (Identifications.SafeSelect(identification => identification.Clone).ToArray(),
                    ProviderId.Clone,
                    CustomData is not null
                        ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))
                        : null);

        #endregion


        #region Operator overloading

        #region Operator == (ProviderAuthenticationData1, ProviderAuthenticationData2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="ProviderAuthenticationData1">Provider authentication data.</param>
        /// <param name="ProviderAuthenticationData2">Other provider authentication data.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ProviderAuthenticationData? ProviderAuthenticationData1,
                                           ProviderAuthenticationData? ProviderAuthenticationData2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ProviderAuthenticationData1, ProviderAuthenticationData2))
                return true;

            // If one is null, but not both, return false.
            if (ProviderAuthenticationData1 is null || ProviderAuthenticationData2 is null)
                return false;

            return ProviderAuthenticationData1.Equals(ProviderAuthenticationData2);

        }

        #endregion

        #region Operator != (ProviderAuthenticationData1, ProviderAuthenticationData2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="ProviderAuthenticationData1">Provider authentication data.</param>
        /// <param name="ProviderAuthenticationData2">Other provider authentication data.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ProviderAuthenticationData? ProviderAuthenticationData1,
                                           ProviderAuthenticationData? ProviderAuthenticationData2)

            => !(ProviderAuthenticationData1 == ProviderAuthenticationData2);

        #endregion

        #region Operator <  (ProviderAuthenticationData1, ProviderAuthenticationData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderAuthenticationData1">Provider authentication data.</param>
        /// <param name="ProviderAuthenticationData2">Other provider authentication data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ProviderAuthenticationData? ProviderAuthenticationData1,
                                          ProviderAuthenticationData? ProviderAuthenticationData2)
        {

            if (ProviderAuthenticationData1 is null)
                throw new ArgumentNullException(nameof(ProviderAuthenticationData1), "The given ProviderAuthenticationData1 must not be null!");

            return ProviderAuthenticationData1.CompareTo(ProviderAuthenticationData2) < 0;

        }

        #endregion

        #region Operator <= (ProviderAuthenticationData1, ProviderAuthenticationData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderAuthenticationData1">Provider authentication data.</param>
        /// <param name="ProviderAuthenticationData2">Other provider authentication data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ProviderAuthenticationData? ProviderAuthenticationData1,
                                           ProviderAuthenticationData? ProviderAuthenticationData2)

            => !(ProviderAuthenticationData1 > ProviderAuthenticationData2);

        #endregion

        #region Operator >  (ProviderAuthenticationData1, ProviderAuthenticationData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderAuthenticationData1">Provider authentication data.</param>
        /// <param name="ProviderAuthenticationData2">Other provider authentication data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ProviderAuthenticationData? ProviderAuthenticationData1,
                                          ProviderAuthenticationData? ProviderAuthenticationData2)
        {

            if (ProviderAuthenticationData1 is null)
                throw new ArgumentNullException(nameof(ProviderAuthenticationData1), "The given ProviderAuthenticationData1 must not be null!");

            return ProviderAuthenticationData1.CompareTo(ProviderAuthenticationData2) > 0;

        }

        #endregion

        #region Operator >= (ProviderAuthenticationData1, ProviderAuthenticationData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderAuthenticationData1">Provider authentication data.</param>
        /// <param name="ProviderAuthenticationData2">Other provider authentication data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ProviderAuthenticationData? ProviderAuthenticationData1,
                                           ProviderAuthenticationData? ProviderAuthenticationData2)

            => !(ProviderAuthenticationData1 < ProviderAuthenticationData2);

        #endregion

        #endregion

        #region IComparable<ProviderAuthenticationData> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ProviderAuthenticationData providerAuthenticationData
                   ? CompareTo(providerAuthenticationData)
                   : throw new ArgumentException("The given object is not provider user identification data!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ProviderAuthenticationData)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderAuthenticationData">An object to compare with.</param>
        public Int32 CompareTo(ProviderAuthenticationData? ProviderAuthenticationData)
        {

            if (ProviderAuthenticationData is null)
                throw new ArgumentNullException(nameof(ProviderAuthenticationData), "The given provider user identification data must not be null!");

            var c = ProviderId.  CompareTo(ProviderAuthenticationData.ProviderId);

            if (c == 0)
                c = Identifications.Count().CompareTo(ProviderAuthenticationData.Identifications.Count());

            //if (c == 0)
            //    c = Identifications.Select(evseDataRecord => evseDataRecord.Id.ToString()).AggregateWith("-").
            //                 CompareTo(ProviderAuthenticationData.Identifications.Select(evseDataRecord => evseDataRecord.Id.ToString()).AggregateWith("-"));

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ProviderAuthenticationData> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is ProviderAuthenticationData providerAuthenticationData &&
                   Equals(providerAuthenticationData);

        #endregion

        #region Equals(ProviderAuthenticationData)

        /// <summary>
        /// Compares two provider user identification datas for equality.
        /// </summary>
        /// <param name="ProviderAuthenticationData">A provider user identification data to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ProviderAuthenticationData? ProviderAuthenticationData)

            => ProviderAuthenticationData is not null &&

               ProviderId.  Equals(ProviderAuthenticationData.ProviderId)   &&

               Identifications.Count().Equals(ProviderAuthenticationData.Identifications.Count()) &&
               Identifications.All(evseDataRecord => ProviderAuthenticationData.Identifications.Contains(evseDataRecord));

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

                return ProviderId.GetHashCode() * 3 ^

                       (Identifications.Any()
                            ? Identifications.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(ProviderId,
                             ", ", Identifications.Count(), " user identification data record(s)");

        #endregion

    }

}
