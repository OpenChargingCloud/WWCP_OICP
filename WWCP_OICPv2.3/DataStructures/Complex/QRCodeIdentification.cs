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
    /// A QR code identification with a (hashed) pin.
    /// </summary>
    public readonly struct QRCodeIdentification : IEquatable<QRCodeIdentification>,
                                                  IComparable<QRCodeIdentification>,
                                                  IComparable
    {

        #region Properties

        /// <summary>
        /// The e-mobility contract identification.
        /// </summary>
        [Mandatory]
        public EVCO_Id     EVCOId       { get; }

        /// <summary>
        /// The QR code identification.
        /// </summary>
        [Optional]
        public HashedPIN?  HashedPIN    { get; }

        /// <summary>
        /// The PIN.
        /// </summary>
        [Optional]
        public PIN?        PIN          { get; }

        #endregion

        #region Constructor(s)

        #region (private) QRCodeIdentification(EVCOId, HashedPIN, PIN)

        /// <summary>
        /// Create a new QR code identification.
        /// </summary>
        /// <param name="EVCOId">The e-mobility contract identification.</param>
        /// <param name="HashedPIN">An optional QR code identification.</param>
        /// <param name="PIN">An optional PIN.</param>
        private QRCodeIdentification(EVCO_Id     EVCOId,
                                     HashedPIN?  HashedPIN,
                                     PIN?        PIN)
        {

            this.EVCOId     = EVCOId;
            this.HashedPIN  = HashedPIN;
            this.PIN        = PIN;

        }

        #endregion

        #region QRCodeIdentification(EVCOId, HashedPIN)

        /// <summary>
        /// Create a new QR code identification for uploading authentication data.
        /// </summary>
        /// <param name="EVCOId">The e-mobility contract identification.</param>
        /// <param name="HashedPIN">The QR code identification.</param>
        public QRCodeIdentification(EVCO_Id    EVCOId,
                                    HashedPIN  HashedPIN)

            : this(EVCOId,
                   HashedPIN,
                   default)

        { }

        #endregion

        #region QRCodeIdentification(EVCOId, PIN)

        /// <summary>
        /// Create a new QR code identification for authorization requests.
        /// </summary>
        /// <param name="EVCOId">The e-mobility contract identification.</param>
        /// <param name="PIN">The PIN.</param>
        public QRCodeIdentification(EVCO_Id  EVCOId,
                                    PIN      PIN)

            : this(EVCOId,
                   default,
                   PIN)

        { }

        #endregion

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#QRCodeIdentificationType

        // {
        //   "EvcoID":    "string",
        //   "HashedPIN": {
        //     "Function":  "Bcrypt",
        //     "LegacyHashData": {
        //       "Function":   "MD5",
        //       "Salt":       "string",
        //       "Value":      "string"
        //     },
        //     "Value":     "string"
        //   },
        //   "PIN":       "string"
        // }

        #endregion

        #region (static) Parse   (JSON, CustomQRCodeIdentificationParser = null)

        /// <summary>
        /// Parse the given JSON representation of a QR code identification.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomQRCodeIdentificationParser">A delegate to parse custom QR code identification JSON objects.</param>
        public static QRCodeIdentification Parse(JObject                                             JSON,
                                                 CustomJObjectParserDelegate<QRCodeIdentification>?  CustomQRCodeIdentificationParser   = null)
        {

            if (TryParse(JSON,
                         out var qrCodeIdentification,
                         out var errorResponse,
                         CustomQRCodeIdentificationParser))
            {
                return qrCodeIdentification;
            }

            throw new ArgumentException("The given JSON representation of a QR code identification is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, CustomQRCodeIdentificationParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a QR code identification.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomQRCodeIdentificationParser">A delegate to parse custom QR code identification JSON objects.</param>
        public static QRCodeIdentification? TryParse(JObject                                             JSON,
                                                     CustomJObjectParserDelegate<QRCodeIdentification>?  CustomQRCodeIdentificationParser   = null)
        {

            if (TryParse(JSON,
                         out QRCodeIdentification qrCodeIdentification,
                         out _,
                         CustomQRCodeIdentificationParser))
            {
                return qrCodeIdentification;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(JSON, out QRCodeIdentification, out ErrorResponse, CustomQRCodeIdentificationParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a QR code identification.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="QRCodeIdentification">The parsed QR code identification.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                   JSON,
                                       out QRCodeIdentification  QRCodeIdentification,
                                       out String?               ErrorResponse)

            => TryParse(JSON,
                        out QRCodeIdentification,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a QR code identification.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="QRCodeIdentification">The parsed QR code identification.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomQRCodeIdentificationParser">A delegate to parse custom QR code identification JSON objects.</param>
        public static Boolean TryParse(JObject                                             JSON,
                                       out QRCodeIdentification                            QRCodeIdentification,
                                       out String?                                         ErrorResponse,
                                       CustomJObjectParserDelegate<QRCodeIdentification>?  CustomQRCodeIdentificationParser)
        {

            try
            {

                QRCodeIdentification = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse EVCOId        [mandatory]

                if (!JSON.ParseMandatory("EvcoID",
                                         "e-mobility contract identification",
                                         EVCO_Id.TryParse,
                                         out EVCO_Id EVCOId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse HashedPIN     [optional]

                if (JSON.ParseOptionalJSON("HashedPIN",
                                           "QR code identification",
                                           OICPv2_3.HashedPIN.TryParse,
                                           out HashedPIN? HashedPIN,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse PIN           [optional]

                if (JSON.ParseOptional("PIN",
                                       "PIN",
                                       OICPv2_3.PIN.TryParse,
                                       out PIN? PIN,
                                       out ErrorResponse))
                {
                    return false;
                }

                #endregion


                QRCodeIdentification = new QRCodeIdentification(EVCOId,
                                                                HashedPIN,
                                                                PIN);


                if (CustomQRCodeIdentificationParser is not null)
                    QRCodeIdentification = CustomQRCodeIdentificationParser(JSON,
                                                                            QRCodeIdentification);

                return true;

            }
            catch (Exception e)
            {
                QRCodeIdentification  = default;
                ErrorResponse         = "The given JSON representation of a QR code identification is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomQRCodeIdentificationSerializer = null, CustomHashedPINSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomQRCodeIdentificationSerializer">A delegate to serialize custom QR code identification JSON objects.</param>
        /// <param name="CustomHashedPINSerializer">A delegate to serialize custom QR code identification JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<QRCodeIdentification>?  CustomQRCodeIdentificationSerializer   = null,
                              CustomJObjectSerializerDelegate<HashedPIN>?             CustomHashedPINSerializer              = null)
        {

            var json = JSONObject.Create(

                           new JProperty("EvcoID",  EVCOId.ToString()),

                           HashedPIN.HasValue
                               ? new JProperty("HashedPIN",  HashedPIN.Value.ToJSON(CustomHashedPINSerializer))
                               : null,

                           PIN.      HasValue
                               ? new JProperty("PIN",        PIN.      Value.ToString())
                               : null

                       );

            return CustomQRCodeIdentificationSerializer is not null
                       ? CustomQRCodeIdentificationSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public QRCodeIdentification Clone

            => new (EVCOId.    Clone,
                    HashedPIN?.Clone,
                    PIN?.      Clone);

        #endregion


        #region Operator overloading

        #region Operator == (QRCodeIdentification1, QRCodeIdentification2)

        /// <summary>
        /// Compares two QR code identifications for equality.
        /// </summary>
        /// <param name="QRCodeIdentification1">A QR code identification.</param>
        /// <param name="QRCodeIdentification2">Another QR code identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (QRCodeIdentification QRCodeIdentification1,
                                           QRCodeIdentification QRCodeIdentification2)

            => QRCodeIdentification1.Equals(QRCodeIdentification2);

        #endregion

        #region Operator != (QRCodeIdentification1, QRCodeIdentification2)

        /// <summary>
        /// Compares two QR code identifications for inequality.
        /// </summary>
        /// <param name="QRCodeIdentification1">A QR code identification.</param>
        /// <param name="QRCodeIdentification2">Another QR code identification.</param>
            /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (QRCodeIdentification QRCodeIdentification1,
                                           QRCodeIdentification QRCodeIdentification2)

            => !QRCodeIdentification1.Equals(QRCodeIdentification2);

        #endregion

        #region Operator <  (QRCodeIdentification1, QRCodeIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="QRCodeIdentification1">A QR code identification.</param>
        /// <param name="QRCodeIdentification2">Another QR code identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (QRCodeIdentification QRCodeIdentification1,
                                          QRCodeIdentification QRCodeIdentification2)

            => QRCodeIdentification1.CompareTo(QRCodeIdentification2) < 0;

        #endregion

        #region Operator <= (QRCodeIdentification1, QRCodeIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="QRCodeIdentification1">A QR code identification.</param>
        /// <param name="QRCodeIdentification2">Another QR code identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (QRCodeIdentification QRCodeIdentification1,
                                           QRCodeIdentification QRCodeIdentification2)

            => QRCodeIdentification1.CompareTo(QRCodeIdentification2) <= 0;

        #endregion

        #region Operator >  (QRCodeIdentification1, QRCodeIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="QRCodeIdentification1">A QR code identification.</param>
        /// <param name="QRCodeIdentification2">Another QR code identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (QRCodeIdentification QRCodeIdentification1,
                                          QRCodeIdentification QRCodeIdentification2)

            => QRCodeIdentification1.CompareTo(QRCodeIdentification2) > 0;

        #endregion

        #region Operator >= (QRCodeIdentification1, QRCodeIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="QRCodeIdentification1">A QR code identification.</param>
        /// <param name="QRCodeIdentification2">Another QR code identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (QRCodeIdentification QRCodeIdentification1,
                                           QRCodeIdentification QRCodeIdentification2)

            => QRCodeIdentification1.CompareTo(QRCodeIdentification2) >= 0;

        #endregion

        #endregion

        #region IComparable<QRCodeIdentification> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two QR code identifications with (hashed) pins.
        /// </summary>
        /// <param name="Object">A QR code identification with (hashed) pin to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is QRCodeIdentification qrCodeIdentification
                   ? CompareTo(qrCodeIdentification)
                   : throw new ArgumentException("The given object is not a QR code identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(QRCodeIdentification)

        /// <summary>
        /// Compares two QR code identifications with (hashed) pins.
        /// </summary>
        /// <param name="QRCodeIdentification">A QR code identification with (hashed) pin to compare with.</param>
        public Int32 CompareTo(QRCodeIdentification QRCodeIdentification)
        {

            var c = EVCOId.CompareTo(QRCodeIdentification.EVCOId);

            if (c == 0 && HashedPIN.HasValue && QRCodeIdentification.HashedPIN.HasValue)
                c = HashedPIN.Value.CompareTo(QRCodeIdentification.HashedPIN.Value);

            if (c == 0 && PIN.      HasValue && QRCodeIdentification.PIN.      HasValue)
                c = PIN.      Value.CompareTo(QRCodeIdentification.PIN.      Value);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<QRCodeIdentification> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two QR code identifications with (hashed) pins for equality.
        /// </summary>
        /// <param name="Object">A QR code identification with (hashed) pin to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is QRCodeIdentification qrCodeIdentification &&
                   Equals(qrCodeIdentification);

        #endregion

        #region Equals(QRCodeIdentification)

        /// <summary>
        /// Compares two QR code identifications with (hashed) pins for equality.
        /// </summary>
        /// <param name="QRCodeIdentification">A QR code identification with (hashed) pin to compare with.</param>
        public Boolean Equals(QRCodeIdentification QRCodeIdentification)

            => EVCOId.   Equals(QRCodeIdentification.EVCOId)    &&
               HashedPIN.Equals(QRCodeIdentification.HashedPIN) &&
               PIN.      Equals(QRCodeIdentification.PIN);

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
                return EVCOId.   GetHashCode() * 5 ^
                       HashedPIN.GetHashCode() * 3 ^
                       PIN.      GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVCOId.ToString(),
                             HashedPIN.HasValue ? ", hashed PIN: " + HashedPIN.ToString() : "",
                             PIN.      HasValue ? ", PIN: "        + PIN.      ToString() : "");

        #endregion

    }

}
