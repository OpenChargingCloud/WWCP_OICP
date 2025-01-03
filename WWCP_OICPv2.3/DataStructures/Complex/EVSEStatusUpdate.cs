/*
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

using System;
using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The current dynamic status of an EVSE.
    /// </summary>
    public readonly struct EVSEStatusUpdate : IEquatable<EVSEStatusUpdate>,
                                              IComparable<EVSEStatusUpdate>,
                                              IComparable
    {

        #region Properties

        /// <summary>
        /// The timestamp of the EVSE status update.
        /// </summary>
        public readonly DateTime         Timestamp     { get; }

        /// <summary>
        /// The unique identification of the EVSE.
        /// </summary>
        [Mandatory]
        public readonly EVSE_Id          Id            { get; }

        /// <summary>
        /// The status of the charging spot.
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
        /// Create a new EVSE status update.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the EVSE status update.</param>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="Status">The status of the charging spot.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public EVSEStatusUpdate(DateTime         Timestamp,
                                EVSE_Id          Id,
                                EVSEStatusTypes  Status,
                                JObject?         CustomData  = null)
        {

            this.Timestamp   = Timestamp;
            this.Id          = Id;
            this.Status      = Status;
            this.CustomData  = CustomData;


            unchecked
            {

                hashCode = this.Id.       GetHashCode() * 5 ^
                           this.Status.   GetHashCode() * 3 ^
                           this.Timestamp.GetHashCode();

            }

        }

        #endregion


        #region (static) Parse   (JSON, CustomEVSEStatusUpdateParser = null)

        /// <summary>
        /// Parse the given JSON representation of an EVSE status update.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomEVSEStatusUpdateParser">A delegate to parse custom EVSE status updates JSON objects.</param>
        public static EVSEStatusUpdate Parse(JObject                                         JSON,
                                             CustomJObjectParserDelegate<EVSEStatusUpdate>?  CustomEVSEStatusUpdateParser   = null)
        {

            if (TryParse(JSON,
                         out var evseStatusUpdate,
                         out var errorResponse,
                         CustomEVSEStatusUpdateParser))
            {
                return evseStatusUpdate;
            }

            throw new ArgumentException("The given JSON representation of an EVSE status update is invalid: " + errorResponse, nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, CustomEVSEStatusUpdateParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of an EVSE status update.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomChargingFacilityParser">A delegate to parse custom charging facilitys JSON objects.</param>
        public static EVSEStatusUpdate? TryParse(JObject                                         JSON,
                                                 CustomJObjectParserDelegate<EVSEStatusUpdate>?  CustomEVSEStatusUpdateParser   = null)
        {

            if (TryParse(JSON,
                         out var evseStatusUpdate,
                         out _,
                         CustomEVSEStatusUpdateParser))
            {
                return evseStatusUpdate;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(JSON, out EVSEStatusUpdate, out ErrorResponse, CustomEVSEStatusUpdateParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an EVSE status update.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="EVSEStatusUpdate">The parsed EVSE status update.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                    JSON,
                                       [NotNullWhen(true)]  out EVSEStatusUpdate  EVSEStatusUpdate,
                                       [NotNullWhen(false)] out String?           ErrorResponse)

            => TryParse(JSON,
                        out EVSEStatusUpdate,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an EVSE status update.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="EVSEStatusUpdate">The parsed EVSE status update.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomEVSEStatusUpdateParser">A delegate to parse custom EVSE status updates JSON objects.</param>
        public static Boolean TryParse(JObject                                         JSON,
                                       [NotNullWhen(true)]  out EVSEStatusUpdate       EVSEStatusUpdate,
                                       [NotNullWhen(false)] out String?                ErrorResponse,
                                       CustomJObjectParserDelegate<EVSEStatusUpdate>?  CustomEVSEStatusUpdateParser)
        {

            try
            {

                EVSEStatusUpdate = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Timestamp      [mandatory]

                if (!JSON.ParseMandatory("Timestamp",
                                         "status update timestamp",
                                         out DateTime Timestamp,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

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

                if (!JSON.ParseMandatory("EvseStatus",
                                         "EVSE status",
                                         EVSEStatusTypesExtensions.TryParse,
                                         out EVSEStatusTypes EVSEStatus,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse CustomData     [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                EVSEStatusUpdate = new EVSEStatusUpdate(
                                       Timestamp,
                                       EVSEId,
                                       EVSEStatus,
                                       customData
                                   );


                if (CustomEVSEStatusUpdateParser is not null)
                    EVSEStatusUpdate = CustomEVSEStatusUpdateParser(JSON,
                                                                    EVSEStatusUpdate);

                return true;

            }
            catch (Exception e)
            {
                EVSEStatusUpdate  = default;
                ErrorResponse     = "The given JSON representation of an EVSE status update is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomEVSEStatusUpdateSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomEVSEStatusUpdateSerializer">A delegate to serialize custom EVSE status update JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<EVSEStatusUpdate>?  CustomEVSEStatusUpdateSerializer   = null)
        {

            var json = JSONObject.Create(

                           new JProperty("Timestamp",   Timestamp.ToIso8601()),
                           new JProperty("EvseID",      Id.       ToString()),
                           new JProperty("EvseStatus",  Status.   ToString()),

                           CustomData?.HasValues == true
                               ? new JProperty("CustomData",  CustomData)
                               : null

                       );

            return CustomEVSEStatusUpdateSerializer is not null
                       ? CustomEVSEStatusUpdateSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this dynamic status of an EVSE.
        /// </summary>
        public EVSEStatusUpdate Clone()

            => new (

                   Timestamp,
                   Id.Clone(),
                   Status,

                   CustomData is not null
                       ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EVSEStatusUpdate EVSEStatusUpdate1,
                                           EVSEStatusUpdate EVSEStatusUpdate2)

            => EVSEStatusUpdate1.Equals(EVSEStatusUpdate2);

        #endregion

        #region Operator != (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EVSEStatusUpdate EVSEStatusUpdate1,
                                           EVSEStatusUpdate EVSEStatusUpdate2)

            => !EVSEStatusUpdate1.Equals(EVSEStatusUpdate2);

        #endregion

        #region Operator <  (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EVSEStatusUpdate EVSEStatusUpdate1,
                                          EVSEStatusUpdate EVSEStatusUpdate2)

            => EVSEStatusUpdate1.CompareTo(EVSEStatusUpdate2) < 0;

        #endregion

        #region Operator <= (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EVSEStatusUpdate EVSEStatusUpdate1,
                                           EVSEStatusUpdate EVSEStatusUpdate2)

            => EVSEStatusUpdate1.CompareTo(EVSEStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EVSEStatusUpdate EVSEStatusUpdate1,
                                          EVSEStatusUpdate EVSEStatusUpdate2)

            => EVSEStatusUpdate1.CompareTo(EVSEStatusUpdate2) > 0;

        #endregion

        #region Operator >= (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EVSEStatusUpdate EVSEStatusUpdate1,
                                          EVSEStatusUpdate EVSEStatusUpdate2)

            => EVSEStatusUpdate1.CompareTo(EVSEStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<EVSEStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two EVSE status updates.
        /// </summary>
        /// <param name="Object">An EVSE status update to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EVSEStatusUpdate evseStatusUpdate
                   ? CompareTo(evseStatusUpdate)
                   : throw new ArgumentException("The given object is not an EVSE status update!", nameof(Object));

        #endregion

        #region CompareTo(EVSEStatusUpdate)

        /// <summary>
        /// Compares two EVSE status updates.
        /// </summary>
        /// <param name="EVSEStatusUpdate">An EVSE status update to compare with.</param>
        public Int32 CompareTo(EVSEStatusUpdate EVSEStatusUpdate)
        {

            var c = Id.       CompareTo(EVSEStatusUpdate.Id);

            if (c == 0)
                c = Status.   CompareTo(EVSEStatusUpdate.Status);

            if (c == 0)
                c = Timestamp.CompareTo(EVSEStatusUpdate.Timestamp);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two EVSE status updates for equality.
        /// </summary>
        /// <param name="Object">An EVSE status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EVSEStatusUpdate evseStatusUpdate &&
                   Equals(evseStatusUpdate);

        #endregion

        #region Equals(EVSEStatusUpdate)

        /// <summary>
        /// Compares two EVSE status updates for equality.
        /// </summary>
        /// <param name="EVSEStatusUpdate">An EVSE status update to compare with.</param>
        public Boolean Equals(EVSEStatusUpdate EVSEStatusUpdate)

            => Id.       Equals(EVSEStatusUpdate.Id)     &&
               Status.   Equals(EVSEStatusUpdate.Status) &&
               Timestamp.Equals(EVSEStatusUpdate.Timestamp);

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

            => $"{Id} -> {Status} [{Timestamp.ToIso8601()}]";

        #endregion

    }

}
