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
    /// The current dynamic status of an EVSE.
    /// </summary>
    public readonly struct EVSEStatusRecord : IEquatable<EVSEStatusRecord>,
                                              IComparable<EVSEStatusRecord>,
                                              IComparable
    {

        #region Properties

        /// <summary>
        /// The ID that identifies the charging spot.
        /// </summary>
        [Mandatory]
        public readonly EVSE_Id          Id            { get; }

        /// <summary>
        /// The unique identification of the EVSE.
        /// </summary>
        [Mandatory]
        public readonly EVSEStatusTypes  Status        { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public readonly JObject?         CustomData    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE status record.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="Status">The status of the charging spot.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public EVSEStatusRecord(EVSE_Id          Id,
                                EVSEStatusTypes  Status,
                                JObject?         CustomData  = null)
        {

            this.Id          = Id;
            this.Status      = Status;
            this.CustomData  = CustomData;

            unchecked
            {

                hashCode = Id.    GetHashCode() * 3 ^
                           Status.GetHashCode();

            }

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#EvseStatusRecordType

        // {
        //     "EvseID":     "DE*GEF*123456789*1",
        //     "EvseStatus": "Available"
        // }

        #endregion

        #region (static) Parse   (JSON, CustomEVSEStatusRecordParser = null)

        /// <summary>
        /// Parse the given JSON representation of an EVSE status record.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSE status records JSON objects.</param>
        public static EVSEStatusRecord Parse(JObject                                         JSON,
                                             CustomJObjectParserDelegate<EVSEStatusRecord>?  CustomEVSEStatusRecordParser   = null)
        {

            if (TryParse(JSON,
                         out var evseStatusRecord,
                         out var errorResponse,
                         CustomEVSEStatusRecordParser))
            {
                return evseStatusRecord;
            }

            throw new ArgumentException("The given JSON representation of an EVSE status record is invalid: " + errorResponse, nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out EVSEStatusRecord, out ErrorResponse, CustomEVSEStatusRecordParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an EVSE status record.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="EVSEStatusRecord">The parsed EVSE status record.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject               JSON,
                                       out EVSEStatusRecord  EVSEStatusRecord,
                                       out String?           ErrorResponse)

            => TryParse(JSON,
                        out EVSEStatusRecord,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an EVSE status record.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="EVSEStatusRecord">The parsed EVSE status record.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSE status records JSON objects.</param>
        public static Boolean TryParse(JObject                                         JSON,
                                       out EVSEStatusRecord                            EVSEStatusRecord,
                                       out String?                                     ErrorResponse,
                                       CustomJObjectParserDelegate<EVSEStatusRecord>?  CustomEVSEStatusRecordParser)
        {

            try
            {

                EVSEStatusRecord = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse EVSEId         [mandatory]

                if (!JSON.ParseMandatory("EvseID",
                                         "EVSE identification",
                                         EVSE_Id.TryParse,
                                         out EVSE_Id EVSEId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse EVSEStatus     [mandatory]

                if (!JSON.ParseMandatoryEnum("EvseStatus",
                                             "EVSE status",
                                             out EVSEStatusTypes EVSEStatus,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse CustomData     [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                EVSEStatusRecord = new EVSEStatusRecord(EVSEId,
                                                        EVSEStatus,
                                                        customData);


                if (CustomEVSEStatusRecordParser is not null)
                    EVSEStatusRecord = CustomEVSEStatusRecordParser(JSON,
                                                                    EVSEStatusRecord);

                return true;

            }
            catch (Exception e)
            {
                EVSEStatusRecord  = default;
                ErrorResponse     = "The given JSON representation of an EVSE status record is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomEVSEStatusRecordSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomEVSEStatusRecordSerializer">A delegate to serialize custom EVSE status record JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<EVSEStatusRecord>?  CustomEVSEStatusRecordSerializer   = null)
        {

            var json = JSONObject.Create(

                           new JProperty("EvseID",      Id.    ToString()),
                           new JProperty("EvseStatus",  Status.ToString()),

                           CustomData?.HasValues == true
                               ? new JProperty("CustomData",  CustomData)
                               : null

                       );

            return CustomEVSEStatusRecordSerializer is not null
                       ? CustomEVSEStatusRecordSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this dynamic status of an EVSE.
        /// </summary>
        public EVSEStatusRecord Clone

            => new (Id.Clone,
                    Status,
                    CustomData is not null
                        ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))
                        : null);

        #endregion


        #region Operator overloading

        #region Operator == (EVSEStatusRecord1, EVSEStatusRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusRecord1">An EVSE status record.</param>
        /// <param name="EVSEStatusRecord2">Another EVSE status record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEStatusRecord EVSEStatusRecord1,
                                           EVSEStatusRecord EVSEStatusRecord2)

            => EVSEStatusRecord1.Equals(EVSEStatusRecord2);

        #endregion

        #region Operator != (EVSEStatusRecord1, EVSEStatusRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusRecord1">An EVSE status record.</param>
        /// <param name="EVSEStatusRecord2">Another EVSE status record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEStatusRecord EVSEStatusRecord1,
                                           EVSEStatusRecord EVSEStatusRecord2)

            => !EVSEStatusRecord1.Equals(EVSEStatusRecord2);

        #endregion

        #region Operator <  (EVSEStatusRecord1, EVSEStatusRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusRecord1">An EVSE status record.</param>
        /// <param name="EVSEStatusRecord2">Another EVSE status record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEStatusRecord EVSEStatusRecord1,
                                          EVSEStatusRecord EVSEStatusRecord2)

            => EVSEStatusRecord1.CompareTo(EVSEStatusRecord2) < 0;

        #endregion

        #region Operator <= (EVSEStatusRecord1, EVSEStatusRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusRecord1">An EVSE status record.</param>
        /// <param name="EVSEStatusRecord2">Another EVSE status record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEStatusRecord EVSEStatusRecord1,
                                           EVSEStatusRecord EVSEStatusRecord2)

            => EVSEStatusRecord1.CompareTo(EVSEStatusRecord2) <= 0;

        #endregion

        #region Operator >  (EVSEStatusRecord1, EVSEStatusRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusRecord1">An EVSE status record.</param>
        /// <param name="EVSEStatusRecord2">Another EVSE status record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEStatusRecord EVSEStatusRecord1,
                                          EVSEStatusRecord EVSEStatusRecord2)

            => EVSEStatusRecord1.CompareTo(EVSEStatusRecord2) > 0;

        #endregion

        #region Operator >= (EVSEStatusRecord1, EVSEStatusRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusRecord1">An EVSE status record.</param>
        /// <param name="EVSEStatusRecord2">Another EVSE status record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEStatusRecord EVSEStatusRecord1,
                                          EVSEStatusRecord EVSEStatusRecord2)

            => EVSEStatusRecord1.CompareTo(EVSEStatusRecord2) >= 0;

        #endregion

        #endregion

        #region IComparable<EVSEStatusRecord> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two EVSE status records.
        /// </summary>
        /// <param name="Object">An EVSE status record to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EVSEStatusRecord evseStatusRecord
                   ? CompareTo(evseStatusRecord)
                   : throw new ArgumentException("The given object is not an EVSE status record!", nameof(Object));

        #endregion

        #region CompareTo(EVSEStatusRecord)

        /// <summary>
        /// Compares two EVSE status records.
        /// </summary>
        /// <param name="EVSEStatusRecord">An EVSE status record to compare with.</param>
        public Int32 CompareTo(EVSEStatusRecord EVSEStatusRecord)
        {

            var c = Id.    CompareTo(EVSEStatusRecord.Id);

            if (c == 0)
                c = Status.CompareTo(EVSEStatusRecord.Status);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEStatusRecord> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two EVSE status records for equality.
        /// </summary>
        /// <param name="Object">An EVSE status record to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EVSEStatusRecord evseStatusRecord &&
                   Equals(evseStatusRecord);

        #endregion

        #region Equals(EVSEStatusRecord)

        /// <summary>
        /// Compares two EVSE status records for equality.
        /// </summary>
        /// <param name="EVSEStatusRecord">An EVSE status record to compare with.</param>
        public Boolean Equals(EVSEStatusRecord EVSEStatusRecord)

            => Id.    Equals(EVSEStatusRecord.Id) &&
               Status.Equals(EVSEStatusRecord.Status);

        #endregion

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()
            => hashCode;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{Id} -> {Status}";

        #endregion

    }

}
