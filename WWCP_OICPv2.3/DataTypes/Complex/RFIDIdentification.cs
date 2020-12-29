/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// A RFID identification.
    /// </summary>
    public class RFIDIdentification : IEquatable<RFIDIdentification>,
                                      IComparable<RFIDIdentification>,
                                      IComparable
    {

        #region Properties

        /// <summary>
        /// The UID from the RFID-Card. It should be read from left to right using big-endian format.
        /// </summary>
        [Mandatory]
        public UID        UID              { get; }

        /// <summary>
        /// An optional electric vehicle contract identification for the given UID.
        /// </summary>
        [Optional]
        public EVCO_Id?   EVCOId           { get; }

        /// <summary>
        /// The type of the used RFID card.
        /// </summary>
        [Mandatory]
        public RFIDTypes  RFIDType         { get; }

        /// <summary>
        /// A number printed on a customer’s card for manual authorization (e.q. via a call center).
        /// </summary>
        [Optional]
        public String     PrintedNumber    { get; }

        /// <summary>
        /// Until when this card is valid. Should not be set if card does not have an expiration yet.
        /// </summary>
        [Optional]
        public DateTime?  ExpiryDate       { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject    CustomData       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new RFID identification.
        /// </summary>
        /// <param name="UID">The UID from the RFID-Card. It should be read from left to right using big-endian format.</param>
        /// <param name="EVCOId">An optional electric vehicle contract identification for the given UID.</param>
        /// <param name="RFIDType">The type of the used RFID card.</param>
        /// <param name="PrintedNumber">A number printed on a customer’s card for manual authorization (e.q. via a call center).</param>
        /// <param name="ExpiryDate">Until when this card is valid. Should not be set if card does not have an expiration yet.</param>
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public RFIDIdentification(UID         UID,
                                  RFIDTypes   RFIDType,
                                  EVCO_Id?    EVCOId          = null,
                                  String      PrintedNumber   = null,
                                  DateTime?   ExpiryDate      = null,
                                  JObject     CustomData      = null)
        {

            this.UID            = UID;
            this.RFIDType       = RFIDType;
            this.EVCOId         = EVCOId;
            this.PrintedNumber  = PrintedNumber?.Trim();
            this.ExpiryDate     = ExpiryDate;
            this.CustomData     = CustomData;

        }

        #endregion


        #region Documentation

        // https://github.com/ahzf/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#RFIDIdentificationType

        // {
        //   "EvcoID":         "string",
        //   "ExpiryDate":     "2020-12-24T07:17:30.354Z",
        //   "PrintedNumber":  "string",
        //   "RFID":           "mifareCls",
        //   "UID":            "string"
        // }

        #endregion

        #region (static) Parse   (JSON, CustomRFIDIdentificationParser = null)

        /// <summary>
        /// Parse the given JSON representation of a RFID identification.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identifications JSON objects.</param>
        public static RFIDIdentification Parse(JObject                                          JSON,
                                               CustomJObjectParserDelegate<RFIDIdentification>  CustomRFIDIdentificationParser   = null)
        {

            if (TryParse(JSON,
                         out RFIDIdentification calibrationLawVerification,
                         out String             ErrorResponse,
                         CustomRFIDIdentificationParser))
            {
                return calibrationLawVerification;
            }

            throw new ArgumentException("The given JSON representation of a RFID identification is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomRFIDIdentificationParser = null)

        /// <summary>
        /// Parse the given text representation of a RFID identification.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identifications JSON objects.</param>
        public static RFIDIdentification Parse(String                                           Text,
                                               CustomJObjectParserDelegate<RFIDIdentification>  CustomRFIDIdentificationParser   = null)
        {

            if (TryParse(Text,
                         out RFIDIdentification calibrationLawVerification,
                         out String             ErrorResponse,
                         CustomRFIDIdentificationParser))
            {
                return calibrationLawVerification;
            }

            throw new ArgumentException("The given text representation of a RFID identification is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out RFIDIdentification, out ErrorResponse, CustomRFIDIdentificationParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a RFID identification.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RFIDIdentification">The parsed RFID identification.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                 JSON,
                                       out RFIDIdentification  RFIDIdentification,
                                       out String              ErrorResponse)

            => TryParse(JSON,
                        out RFIDIdentification,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a RFID identification.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RFIDIdentification">The parsed RFID identification.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identifications JSON objects.</param>
        public static Boolean TryParse(JObject                                          JSON,
                                       out RFIDIdentification                           RFIDIdentification,
                                       out String                                       ErrorResponse,
                                       CustomJObjectParserDelegate<RFIDIdentification>  CustomRFIDIdentificationParser)
        {

            try
            {

                RFIDIdentification = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse UID               [mandatory]

                if (!JSON.ParseMandatory("UID",
                                         "unique/user identification",
                                         OICPv2_3.UID.TryParse,
                                         out UID UID,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse RFIDType          [mandatory]

                if (!JSON.ParseMandatoryEnum("RFID",
                                             "RFID type",
                                             out RFIDTypes RFIDType,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse EVCOId            [optional]

                if (JSON.ParseOptional("EvcoID",
                                       "EVCO identification",
                                       EVCO_Id.TryParse,
                                       out EVCO_Id? EVCOId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse PrintedNumber     [optional]

                var PrintedNumber = JSON["PrintedNumber"]?.Value<String>();

                #endregion

                #region Parse ExpiryDate        [optional]

                if (JSON.ParseOptional("ExpiryDate",
                                       "expiry date",
                                       out DateTime? ExpiryDate,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse Custom Data       [optional]

                var CustomData        = JSON["CustomData"] as JObject;

                #endregion


                RFIDIdentification = new RFIDIdentification(UID,
                                                            RFIDType,
                                                            EVCOId,
                                                            PrintedNumber,
                                                            ExpiryDate,
                                                            CustomData);


                if (CustomRFIDIdentificationParser != null)
                    RFIDIdentification = CustomRFIDIdentificationParser(JSON,
                                                                        RFIDIdentification);

                return true;

            }
            catch (Exception e)
            {
                RFIDIdentification  = default;
                ErrorResponse       = "The given JSON representation of a RFID identification is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out RFIDIdentification, out ErrorResponse, CustomRFIDIdentificationParser = null)

        /// <summary>
        /// Try to parse the given text representation of a RFID identification.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="RFIDIdentification">The parsed RFID identification.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identifications JSON objects.</param>
        public static Boolean TryParse(String                                           Text,
                                       out RFIDIdentification                           RFIDIdentification,
                                       out String                                       ErrorResponse,
                                       CustomJObjectParserDelegate<RFIDIdentification>  CustomRFIDIdentificationParser)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out RFIDIdentification,
                                out ErrorResponse,
                                CustomRFIDIdentificationParser);

            }
            catch (Exception e)
            {
                RFIDIdentification  = default;
                ErrorResponse       = "The given text representation of a RFID identification is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomRFIDIdentificationSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomRFIDIdentificationSerializer">A delegate to serialize custom RFID identification JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<RFIDIdentification>  CustomRFIDIdentificationSerializer   = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("UID",                     UID.         ToString()),

                           new JProperty("RFID",                    RFIDType.    AsString()),

                           EVCOId.HasValue
                               ? new JProperty("EvcoID",            EVCOId.Value.ToString())
                               : null,

                           PrintedNumber.IsNeitherNullNorEmpty()
                               ? new JProperty("PrintedNumber",     PrintedNumber)
                               : null,

                           ExpiryDate.HasValue
                               ? new JProperty("ExpiryDate",        ExpiryDate.Value.ToIso8601())
                               : null,

                           CustomData != null
                               ? new JProperty("CustomData",        CustomData)
                               : null

                       );

            return CustomRFIDIdentificationSerializer != null
                       ? CustomRFIDIdentificationSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this RFID identification.
        /// </summary>
        public RFIDIdentification Clone

            => new RFIDIdentification(UID.Clone,
                                      RFIDType,
                                      EVCOId?.Clone,
                                      PrintedNumber != null ? new String(PrintedNumber.ToCharArray())                             : null,
                                      ExpiryDate,
                                      CustomData    != null ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None)) : null);

        #endregion


        #region Operator overloading

        #region Operator == (RFIDIdentification1, RFIDIdentification2)

        /// <summary>
        /// Compares two RFID identifications for equality.
        /// </summary>
        /// <param name="RFIDIdentification1">A RFID identification.</param>
        /// <param name="RFIDIdentification2">Another RFID identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RFIDIdentification RFIDIdentification1,
                                           RFIDIdentification RFIDIdentification2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RFIDIdentification1, RFIDIdentification2))
                return true;

            // If one is null, but not both, return false.
            if ((RFIDIdentification1 is null) || (RFIDIdentification2 is null))
                return false;

            return RFIDIdentification1.Equals(RFIDIdentification2);

        }

        #endregion

        #region Operator != (RFIDIdentification1, RFIDIdentification2)

        /// <summary>
        /// Compares two RFID identifications for inequality.
        /// </summary>
        /// <param name="RFIDIdentification1">A RFID identification.</param>
        /// <param name="RFIDIdentification2">Another RFID identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RFIDIdentification RFIDIdentification1,
                                           RFIDIdentification RFIDIdentification2)

            => !(RFIDIdentification1 == RFIDIdentification2);

        #endregion

        #region Operator <  (RFIDIdentification1, RFIDIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDIdentification1">A RFID identification.</param>
        /// <param name="RFIDIdentification2">Another RFID identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RFIDIdentification RFIDIdentification1,
                                          RFIDIdentification RFIDIdentification2)
        {

            if (RFIDIdentification1 is null)
                throw new ArgumentNullException(nameof(RFIDIdentification1), "The given RFID identification must not be null!");

            return RFIDIdentification1.CompareTo(RFIDIdentification2) < 0;

        }

        #endregion

        #region Operator <= (RFIDIdentification1, RFIDIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDIdentification1">A RFID identification.</param>
        /// <param name="RFIDIdentification2">Another RFID identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RFIDIdentification RFIDIdentification1,
                                           RFIDIdentification RFIDIdentification2)

            => !(RFIDIdentification1 > RFIDIdentification2);

        #endregion

        #region Operator >  (RFIDIdentification1, RFIDIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDIdentification1">A RFID identification.</param>
        /// <param name="RFIDIdentification2">Another RFID identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RFIDIdentification RFIDIdentification1,
                                          RFIDIdentification RFIDIdentification2)
        {

            if (RFIDIdentification1 is null)
                throw new ArgumentNullException(nameof(RFIDIdentification1), "The given RFID identification must not be null!");

            return RFIDIdentification1.CompareTo(RFIDIdentification2) > 0;

        }

        #endregion

        #region Operator >= (RFIDIdentification1, RFIDIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDIdentification1">A RFID identification.</param>
        /// <param name="RFIDIdentification2">Another RFID identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RFIDIdentification RFIDIdentification1,
                                           RFIDIdentification RFIDIdentification2)

            => !(RFIDIdentification1 < RFIDIdentification2);

        #endregion

        #endregion

        #region IComparable<RFIDIdentification> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is RFIDIdentification rfidIdentification
                   ? CompareTo(rfidIdentification)
                   : throw new ArgumentException("The given object is not a RFID identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(RFIDIdentification)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDIdentification">An RFID identification object to compare with.</param>
        public Int32 CompareTo(RFIDIdentification RFIDIdentification)
        {

            if (RFIDIdentification is null)
                throw new ArgumentNullException(nameof(RFIDIdentification),  "The given RFID identification must not be null!");

            var result = UID.CompareTo(RFIDIdentification.UID);

            if (result == 0)
                result = RFIDType.CompareTo(RFIDIdentification.RFIDType);

            if (result == 0 && EVCOId.HasValue && RFIDIdentification.EVCOId.HasValue)
                result = EVCOId.Value.CompareTo(RFIDIdentification.EVCOId.Value);

            if (result == 0)
                result = String.Compare(PrintedNumber, RFIDIdentification.PrintedNumber, StringComparison.OrdinalIgnoreCase);

            if (result == 0 && ExpiryDate.HasValue && RFIDIdentification.ExpiryDate.HasValue)
                result = ExpiryDate.Value.CompareTo(RFIDIdentification.ExpiryDate.Value);

            return result;

        }

        #endregion

        #endregion

        #region IEquatable<RFIDIdentification> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is RFIDIdentification rfidIdentification &&
                   Equals(rfidIdentification);

        #endregion

        #region Equals(RFIDIdentification)

        /// <summary>
        /// Compares two RFID identificationss for equality.
        /// </summary>
        /// <param name="RFIDIdentification">An RFID identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RFIDIdentification RFIDIdentification)

            => !(RFIDIdentification is null) &&

                 UID.     Equals(RFIDIdentification.UID)      &&
                 RFIDType.Equals(RFIDIdentification.RFIDType) &&

                 ((!EVCOId.HasValue && !RFIDIdentification.EVCOId.HasValue) ||
                   (EVCOId.HasValue && RFIDIdentification.EVCOId.HasValue && EVCOId.Value.Equals(RFIDIdentification.EVCOId.Value))) &&

                 ((PrintedNumber == null && RFIDIdentification.PrintedNumber == null) ||
                  (PrintedNumber != null && RFIDIdentification.PrintedNumber != null &&
                   String.Compare(PrintedNumber, RFIDIdentification.PrintedNumber, StringComparison.OrdinalIgnoreCase) != 0)) &&

                 ((!ExpiryDate.HasValue && !RFIDIdentification.ExpiryDate.HasValue) ||
                   (ExpiryDate.HasValue && RFIDIdentification.ExpiryDate.HasValue && ExpiryDate.Value.Equals(RFIDIdentification.ExpiryDate.Value)));

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return EVCOId.  GetHashCode() * 11 ^
                       RFIDType.GetHashCode() *  7 ^

                       (EVCOId.HasValue
                            ? EVCOId.GetHashCode() * 5
                            : 0) ^

                       (PrintedNumber != null
                            ? PrintedNumber.GetHashCode() * 3
                            : 0) ^

                       (EVCOId.HasValue
                            ? EVCOId.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(UID.ToString(),
                             " (", RFIDType, ") ",

                             PrintedNumber.IsNeitherNullNorEmpty()
                                 ? ", '" + PrintedNumber + "'"
                                 : "",

                             EVCOId.HasValue
                                 ? ", ContractId: '" + EVCOId + "'"
                                 : "",

                             ExpiryDate.HasValue
                                 ? ", expires: " + ExpiryDate
                                 : "");

        #endregion

    }

}
