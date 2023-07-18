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
    /// Operator EVSE status.
    /// </summary>
    public class OperatorEVSEStatus : IEquatable<OperatorEVSEStatus>,
                                      IComparable<OperatorEVSEStatus>,
                                      IComparable
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE status records.
        /// </summary>
        [Mandatory]
        public IEnumerable<EVSEStatusRecord>  EVSEStatusRecords    { get; }

        /// <summary>
        /// The unqiue identification of the EVSE operator maintaining the given EVSE status records.
        /// </summary>
        [Mandatory]
        public Operator_Id                    OperatorId           { get; }

        /// <summary>
        /// The name of the EVSE operator maintaining the given EVSE status records.
        /// </summary>
        [Mandatory]
        public String                         OperatorName         { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject?                       CustomData           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new operator EVSE status object.
        /// </summary>
        /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
        /// <param name="OperatorId">The unqiue identification of the EVSE operator maintaining the given EVSE status records.</param>
        /// <param name="OperatorName">The name of the EVSE operator maintaining the given EVSE status records.</param>
        /// 
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public OperatorEVSEStatus(IEnumerable<EVSEStatusRecord>  EVSEStatusRecords,
                                  Operator_Id                    OperatorId,
                                  String                         OperatorName,

                                  JObject?                       CustomData   = null)
        {

            var duplicateEVSEStatusRecords  = EVSEStatusRecords.GroupBy(evseStatusRecord => evseStatusRecord.Id).Where(group => group.Count() > 1).ToArray();
            if (duplicateEVSEStatusRecords.SafeAny())
                throw new ArgumentException("The following EVSE Ids are not unique: " + duplicateEVSEStatusRecords.AggregateWith(", "), nameof(EVSEStatusRecords));

            this.EVSEStatusRecords  = EVSEStatusRecords.Distinct();
            this.OperatorId         = OperatorId;
            this.OperatorName       = OperatorName.Trim();

            this.CustomData         = CustomData;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#OperatorEvseStatusType

        // {
        //   "OperatorID":     "string",
        //   "OperatorName":   "string",
        //   "EvseStatusRecord":  [
        //     {
        //       ...
        //     }
        //   ]
        // }

        #endregion

        #region (static) Parse   (JSON, CustomOperatorEVSEStatusParser = null)

        /// <summary>
        /// Parse the given JSON representation of operator EVSE status.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom operator EVSE statuss JSON objects.</param>
        public static OperatorEVSEStatus Parse(JObject                                           JSON,
                                               CustomJObjectParserDelegate<OperatorEVSEStatus>?  CustomOperatorEVSEStatusParser   = null)
        {

            if (TryParse(JSON,
                         out var operatorEVSEStatus,
                         out var errorResponse,
                         CustomOperatorEVSEStatusParser))
            {
                return operatorEVSEStatus!;
            }

            throw new ArgumentException("The given JSON representation of operator EVSE status is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out OperatorEVSEStatus, out ErrorResponse, CustomOperatorEVSEStatusParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of operator EVSE status.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OperatorEVSEStatus">The parsed operator EVSE status.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                  JSON,
                                       out OperatorEVSEStatus?  OperatorEVSEStatus,
                                       out String?              ErrorResponse)

            => TryParse(JSON,
                        out OperatorEVSEStatus,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of operator EVSE status.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OperatorEVSEStatus">The parsed operator EVSE status.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom operator EVSE statuss JSON objects.</param>
        public static Boolean TryParse(JObject                                           JSON,
                                       out OperatorEVSEStatus?                           OperatorEVSEStatus,
                                       out String?                                       ErrorResponse,
                                       CustomJObjectParserDelegate<OperatorEVSEStatus>?  CustomOperatorEVSEStatusParser)
        {

            try
            {

                OperatorEVSEStatus = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse EvseStatusRecords     [mandatory]

                if (!JSON.ParseMandatoryJSON("EvseStatusRecord",
                                             "EVSE status records",
                                             EVSEStatusRecord.TryParse,
                                             out IEnumerable<EVSEStatusRecord> EvseStatusRecords,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse OperatorId            [mandatory]

                if (!JSON.ParseMandatory("OperatorID",
                                         "operator identification",
                                         Operator_Id.TryParse,
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

                #region Parse CustomData            [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                OperatorEVSEStatus = new OperatorEVSEStatus(EvseStatusRecords,
                                                            OperatorId,
                                                            OperatorName,
                                                            customData);


                if (CustomOperatorEVSEStatusParser is not null)
                    OperatorEVSEStatus = CustomOperatorEVSEStatusParser(JSON,
                                                                        OperatorEVSEStatus);

                return true;

            }
            catch (Exception e)
            {
                OperatorEVSEStatus  = default;
                ErrorResponse       = "The given JSON representation of operator EVSE status is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomOperatorEVSEStatusSerializer = null, CustomEVSEStatusRecordSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomOperatorEVSEStatusSerializer">A delegate to serialize custom operator EVSE status JSON objects.</param>
        /// <param name="CustomEVSEStatusRecordSerializer">A delegate to serialize custom EVSE status record JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<OperatorEVSEStatus>?  CustomOperatorEVSEStatusSerializer   = null,
                              CustomJObjectSerializerDelegate<EVSEStatusRecord>?    CustomEVSEStatusRecordSerializer     = null)
        {

            var json = JSONObject.Create(

                           new JProperty("EvseStatusRecord",  new JArray(EVSEStatusRecords.Select(evseStatusRecord => evseStatusRecord.ToJSON(CustomEVSEStatusRecordSerializer)))),
                           new JProperty("OperatorID",        OperatorId.ToString()),
                           new JProperty("OperatorName",      OperatorName),

                           CustomData?.HasValues == true
                               ? new JProperty("CustomData",    CustomData)
                               : null

                       );

            return CustomOperatorEVSEStatusSerializer is not null
                       ? CustomOperatorEVSEStatusSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public OperatorEVSEStatus Clone

            => new (EVSEStatusRecords.SafeSelect(evseStatusRecord => evseStatusRecord.Clone).ToArray(),
                    OperatorId.Clone,
                    new String(OperatorName.ToCharArray()),
                    CustomData is not null
                        ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))
                        : null);

        #endregion


        #region Operator overloading

        #region Operator == (OperatorEVSEStatus1, OperatorEVSEStatus2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="OperatorEVSEStatus1">An operator EVSE status.</param>
        /// <param name="OperatorEVSEStatus2">Another operator EVSE status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (OperatorEVSEStatus? OperatorEVSEStatus1,
                                           OperatorEVSEStatus? OperatorEVSEStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(OperatorEVSEStatus1, OperatorEVSEStatus2))
                return true;

            // If one is null, but not both, return false.
            if (OperatorEVSEStatus1 is null || OperatorEVSEStatus2 is null)
                return false;

            return OperatorEVSEStatus1.Equals(OperatorEVSEStatus2);

        }

        #endregion

        #region Operator != (OperatorEVSEStatus1, OperatorEVSEStatus2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="OperatorEVSEStatus1">An operator EVSE status.</param>
        /// <param name="OperatorEVSEStatus2">Another operator EVSE status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (OperatorEVSEStatus? OperatorEVSEStatus1,
                                           OperatorEVSEStatus? OperatorEVSEStatus2)

            => !(OperatorEVSEStatus1 == OperatorEVSEStatus2);

        #endregion

        #region Operator <  (OperatorEVSEStatus1, OperatorEVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEStatus1">An operator EVSE status.</param>
        /// <param name="OperatorEVSEStatus2">Another operator EVSE status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (OperatorEVSEStatus? OperatorEVSEStatus1,
                                          OperatorEVSEStatus? OperatorEVSEStatus2)
        {

            if (OperatorEVSEStatus1 is null)
                throw new ArgumentNullException(nameof(OperatorEVSEStatus1), "The given OperatorEVSEStatus1 must not be null!");

            return OperatorEVSEStatus1.CompareTo(OperatorEVSEStatus2) < 0;

        }

        #endregion

        #region Operator <= (OperatorEVSEStatus1, OperatorEVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEStatus1">An operator EVSE status.</param>
        /// <param name="OperatorEVSEStatus2">Another operator EVSE status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (OperatorEVSEStatus? OperatorEVSEStatus1,
                                           OperatorEVSEStatus? OperatorEVSEStatus2)

            => !(OperatorEVSEStatus1 > OperatorEVSEStatus2);

        #endregion

        #region Operator >  (OperatorEVSEStatus1, OperatorEVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEStatus1">An operator EVSE status.</param>
        /// <param name="OperatorEVSEStatus2">Another operator EVSE status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (OperatorEVSEStatus? OperatorEVSEStatus1,
                                          OperatorEVSEStatus? OperatorEVSEStatus2)
        {

            if (OperatorEVSEStatus1 is null)
                throw new ArgumentNullException(nameof(OperatorEVSEStatus1), "The given OperatorEVSEStatus1 must not be null!");

            return OperatorEVSEStatus1.CompareTo(OperatorEVSEStatus2) > 0;

        }

        #endregion

        #region Operator >= (OperatorEVSEStatus1, OperatorEVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEStatus1">An operator EVSE status.</param>
        /// <param name="OperatorEVSEStatus2">Another operator EVSE status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (OperatorEVSEStatus? OperatorEVSEStatus1,
                                           OperatorEVSEStatus? OperatorEVSEStatus2)

            => !(OperatorEVSEStatus1 < OperatorEVSEStatus2);

        #endregion

        #endregion

        #region IComparable<OperatorEVSEStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two operator EVSE status.
        /// </summary>
        /// <param name="Object">An operator EVSE status to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is OperatorEVSEStatus operatorEVSEStatus
                   ? CompareTo(operatorEVSEStatus)
                   : throw new ArgumentException("The given object is not operator EVSE status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(OperatorEVSEStatus)

        /// <summary>
        /// Compares two operator EVSE status.
        /// </summary>
        /// <param name="OperatorEVSEStatus">An operator EVSE status to compare with.</param>
        public Int32 CompareTo(OperatorEVSEStatus? OperatorEVSEStatus)
        {

            if (OperatorEVSEStatus is null)
                throw new ArgumentNullException(nameof(OperatorEVSEStatus), "The given operator EVSE status must not be null!");

            var c = OperatorId.  CompareTo(OperatorEVSEStatus.OperatorId);

            if (c == 0)
                c = OperatorName.CompareTo(OperatorEVSEStatus.OperatorName);

            if (c == 0)
                c = EVSEStatusRecords.Count().CompareTo(OperatorEVSEStatus.EVSEStatusRecords.Count());

            if (c == 0)
                c = EVSEStatusRecords.Select(evseStatusRecord => evseStatusRecord.Id.ToString()).AggregateWith("-").
                             CompareTo(OperatorEVSEStatus.EVSEStatusRecords.Select(evseStatusRecord => evseStatusRecord.Id.ToString()).AggregateWith("-"));

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<OperatorEVSEStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two operator EVSE status for equality.
        /// </summary>
        /// <param name="Object">An operator EVSE status to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is OperatorEVSEStatus operatorEVSEStatus &&
                   Equals(operatorEVSEStatus);

        #endregion

        #region Equals(OperatorEVSEStatus)

        /// <summary>
        /// Compares two operator EVSE status for equality.
        /// </summary>
        /// <param name="OperatorEVSEStatus">An operator EVSE status to compare with.</param>
        public Boolean Equals(OperatorEVSEStatus? OperatorEVSEStatus)

            => OperatorEVSEStatus is not null &&

               OperatorId.  Equals(OperatorEVSEStatus.OperatorId)   &&
               OperatorName.Equals(OperatorEVSEStatus.OperatorName) &&

               EVSEStatusRecords.Count().Equals(OperatorEVSEStatus.EVSEStatusRecords.Count()) &&
               EVSEStatusRecords.All(evseStatusRecord => OperatorEVSEStatus.EVSEStatusRecords.Contains(evseStatusRecord));

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

                return OperatorId.    GetHashCode()       * 5 ^
                       (OperatorName?.GetHashCode() ?? 0) * 3 ^

                       (EVSEStatusRecords.Any()
                            ? EVSEStatusRecords.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(OperatorId,
                             OperatorName.IsNotNullOrEmpty() ? ", " + OperatorName : "",
                             ", ",  EVSEStatusRecords.Count(), " EVSE status record(s)");

        #endregion

    }

}
