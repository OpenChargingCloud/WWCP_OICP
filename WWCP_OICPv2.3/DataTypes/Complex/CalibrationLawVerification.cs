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
    /// A calibration law verification.
    /// </summary>
    public class CalibrationLawVerification : IEquatable<CalibrationLawVerification>,
                                              IComparable<CalibrationLawVerification>,
                                              IComparable
    {

        #region Properties

        /// <summary>
        /// The Calibration Law Compliance ID from respective authority along with the revision and issueing date.
        /// (Compliance ID : Revision : Date)
        /// </summary>
        /// <example>PTB - X-X-XXXX : V1 : 01Jan2020</example>
        [Optional]
        public String  CalibrationLawCertificateId                    { get; }

        /// <summary>
        /// Unique PublicKey for EVSEID (or the smart energy meter within the charging station) can be provided here.
        /// </summary>
        [Optional]
        public String  PublicKey                                      { get; }

        /// <summary>
        /// In this field CPO can also provide an url to a external data set. This data set can give calibration law information which can be simply added to the end customer invoice of the EMP.
        /// The information can contain for eg Charging Station Details, Charging Session Date/Time, SignedMeteringValues(Transparency Software format), SignedMeterValuesVerificationInstruction etc.
        /// </summary>
        [Optional]
        public URL?    MeteringSignatureURL                           { get; }

        /// <summary>
        /// Encoding format of the metering signature data as well as the version.
        /// </summary>
        /// <example>EDL40 Mennekes: V1</example>
        [Optional]
        public String  MeteringSignatureEncodingFormat                { get; }

        /// <summary>
        /// Additional information (e.g. instruction on how to use the transparency software).
        /// </summary>
        [Optional]
        public String  SignedMeteringValuesVerificationInstruction    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new calibration law verification.
        /// </summary>
        /// <param name="CalibrationLawCertificateId">The Calibration Law Compliance ID from respective authority along with the revision and issueing date.</param>
        /// <param name="PublicKey">Unique PublicKey for EVSEID (or the smart energy meter within the charging station) can be provided here.</param>
        /// <param name="MeteringSignatureURL"></param>
        /// <param name="MeteringSignatureEncodingFormat"></param>
        /// <param name="SignedMeteringValuesVerificationInstruction"></param>
        public CalibrationLawVerification(String CalibrationLawCertificateId                   = null,
                                          String PublicKey                                     = null,
                                          URL?   MeteringSignatureURL                          = null,
                                          String MeteringSignatureEncodingFormat               = null,
                                          String SignedMeteringValuesVerificationInstruction   = null)
        {

            this.CalibrationLawCertificateId                  = CalibrationLawCertificateId;
            this.PublicKey                                    = PublicKey;
            this.MeteringSignatureURL                         = MeteringSignatureURL;
            this.MeteringSignatureEncodingFormat              = MeteringSignatureEncodingFormat;
            this.SignedMeteringValuesVerificationInstruction  = SignedMeteringValuesVerificationInstruction;

        }

        #endregion


        #region Documentation

        // https://github.com/ahzf/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#CalibrationLawVerificationType

        // {
        //     "CO2Emission":   0,
        //     "NuclearWaste":  0
        // }

        #endregion

        #region (static) Parse   (JSON, CustomCalibrationLawVerificationParser = null)

        /// <summary>
        /// Parse the given JSON representation of a calibration law verification.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomCalibrationLawVerificationParser">A delegate to parse custom calibration law verifications JSON objects.</param>
        public static CalibrationLawVerification Parse(JObject                                                  JSON,
                                                       CustomJObjectParserDelegate<CalibrationLawVerification>  CustomCalibrationLawVerificationParser   = null)
        {

            if (TryParse(JSON,
                         out CalibrationLawVerification calibrationLawVerification,
                         out String                     ErrorResponse,
                         CustomCalibrationLawVerificationParser))
            {
                return calibrationLawVerification;
            }

            throw new ArgumentException("The given JSON representation of a calibration law verification is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomCalibrationLawVerificationParser = null)

        /// <summary>
        /// Parse the given text representation of a calibration law verification.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomCalibrationLawVerificationParser">A delegate to parse custom calibration law verifications JSON objects.</param>
        public static CalibrationLawVerification Parse(String                                                   Text,
                                                       CustomJObjectParserDelegate<CalibrationLawVerification>  CustomCalibrationLawVerificationParser   = null)
        {

            if (TryParse(Text,
                         out CalibrationLawVerification calibrationLawVerification,
                         out String                     ErrorResponse,
                         CustomCalibrationLawVerificationParser))
            {
                return calibrationLawVerification;
            }

            throw new ArgumentException("The given text representation of a calibration law verification is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out CalibrationLawVerification, out ErrorResponse, CustomCalibrationLawVerificationParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a calibration law verification.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CalibrationLawVerification">The parsed connector.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                         JSON,
                                       out CalibrationLawVerification  CalibrationLawVerification,
                                       out String                      ErrorResponse)

            => TryParse(JSON,
                        out CalibrationLawVerification,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a calibration law verification.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CalibrationLawVerification">The parsed connector.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomCalibrationLawVerificationParser">A delegate to parse custom calibration law verifications JSON objects.</param>
        public static Boolean TryParse(JObject                                                  JSON,
                                       out CalibrationLawVerification                           CalibrationLawVerification,
                                       out String                                               ErrorResponse,
                                       CustomJObjectParserDelegate<CalibrationLawVerification>  CustomCalibrationLawVerificationParser)
        {

            try
            {

                CalibrationLawVerification  = default;
                ErrorResponse               = default;

                #region Parse MeteringSignatureURL    [optional]

                if (JSON.ParseOptional("MeteringSignatureUrl",
                                       "metering signature URL",
                                       URL.TryParse,
                                       out URL? MeteringSignatureURL,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion


                CalibrationLawVerification  = new CalibrationLawVerification(JSON["CalibrationLawCertificateID"                ]?.Value<String>(),
                                                                             JSON["PublicKey"                                  ]?.Value<String>(),
                                                                             MeteringSignatureURL,
                                                                             JSON["MeteringSignatureEncodingFormat"            ]?.Value<String>(),
                                                                             JSON["SignedMeteringValuesVerificationInstruction"]?.Value<String>());

                if (CustomCalibrationLawVerificationParser != null)
                    CalibrationLawVerification = CustomCalibrationLawVerificationParser(JSON,
                                                                                        CalibrationLawVerification);

                return true;

            }
            catch (Exception e)
            {
                CalibrationLawVerification  = default;
                ErrorResponse               = "The given JSON representation of a calibration law verification is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out CalibrationLawVerification, out ErrorResponse, CustomCalibrationLawVerificationParser = null)

        /// <summary>
        /// Try to parse the given text representation of a calibration law verification.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CalibrationLawVerification">The parsed connector.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomCalibrationLawVerificationParser">A delegate to parse custom calibration law verifications JSON objects.</param>
        public static Boolean TryParse(String                                                   Text,
                                       out CalibrationLawVerification                           CalibrationLawVerification,
                                       out String                                               ErrorResponse,
                                       CustomJObjectParserDelegate<CalibrationLawVerification>  CustomCalibrationLawVerificationParser)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out CalibrationLawVerification,
                                out ErrorResponse,
                                CustomCalibrationLawVerificationParser);

            }
            catch (Exception e)
            {
                CalibrationLawVerification  = default;
                ErrorResponse               = "The given text representation of a calibration law verification is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomCalibrationLawVerificationSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomCalibrationLawVerificationSerializer">A delegate to serialize custom calibration law verification JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<CalibrationLawVerification>  CustomCalibrationLawVerificationSerializer   = null)
        {

            var JSON = JSONObject.Create(

                           CalibrationLawCertificateId.IsNullOrEmpty()
                               ? new JProperty("CalibrationLawCertificateID",                  CalibrationLawCertificateId)
                               : null,

                           PublicKey.IsNullOrEmpty()
                               ? new JProperty("PublicKey",                                    PublicKey)
                               : null,

                           MeteringSignatureURL.HasValue
                               ? new JProperty("MeteringSignatureUrl",                         MeteringSignatureURL.Value.ToString())
                               : null,

                           MeteringSignatureEncodingFormat.IsNullOrEmpty()
                               ? new JProperty("MeteringSignatureEncodingFormat",              MeteringSignatureEncodingFormat)
                               : null,

                           SignedMeteringValuesVerificationInstruction.IsNullOrEmpty()
                               ? new JProperty("SignedMeteringValuesVerificationInstruction",  SignedMeteringValuesVerificationInstruction)
                               : null

                       );

            var JSON2 = CustomCalibrationLawVerificationSerializer != null
                            ? CustomCalibrationLawVerificationSerializer(this, JSON)
                            : JSON;

            return JSON2.HasValues
                       ? JSON2
                       : null;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this calibration law verification.
        /// </summary>
        public CalibrationLawVerification Clone

            => new CalibrationLawVerification(new String(CalibrationLawCertificateId.                ToCharArray()),
                                              new String(PublicKey.                                  ToCharArray()),
                                              MeteringSignatureURL,
                                              new String(MeteringSignatureEncodingFormat.            ToCharArray()),
                                              new String(SignedMeteringValuesVerificationInstruction.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (CalibrationLawVerification1, CalibrationLawVerification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CalibrationLawVerification1">A calibration law verification.</param>
        /// <param name="CalibrationLawVerification2">Another calibration law verification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (CalibrationLawVerification CalibrationLawVerification1,
                                           CalibrationLawVerification CalibrationLawVerification2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(CalibrationLawVerification1, CalibrationLawVerification2))
                return true;

            // If one is null, but not both, return false.
            if (CalibrationLawVerification1 is null || CalibrationLawVerification2 is null)
                return false;

            return CalibrationLawVerification1.Equals(CalibrationLawVerification2);

        }

        #endregion

        #region Operator != (CalibrationLawVerification1, CalibrationLawVerification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CalibrationLawVerification1">A calibration law verification.</param>
        /// <param name="CalibrationLawVerification2">Another calibration law verification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (CalibrationLawVerification CalibrationLawVerification1,
                                           CalibrationLawVerification CalibrationLawVerification2)

            => !(CalibrationLawVerification1 == CalibrationLawVerification2);

        #endregion

        #region Operator <  (CalibrationLawVerification1, CalibrationLawVerification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CalibrationLawVerification1">A calibration law verification.</param>
        /// <param name="CalibrationLawVerification2">Another calibration law verification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (CalibrationLawVerification CalibrationLawVerification1,
                                          CalibrationLawVerification CalibrationLawVerification2)
        {

            if (CalibrationLawVerification1 is null)
                throw new ArgumentNullException(nameof(CalibrationLawVerification1), "The given calibration law verification must not be null!");

            return CalibrationLawVerification1.CompareTo(CalibrationLawVerification2) < 0;

        }

        #endregion

        #region Operator <= (CalibrationLawVerification1, CalibrationLawVerification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CalibrationLawVerification1">A calibration law verification.</param>
        /// <param name="CalibrationLawVerification2">Another calibration law verification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (CalibrationLawVerification CalibrationLawVerification1,
                                           CalibrationLawVerification CalibrationLawVerification2)

            => !(CalibrationLawVerification1 > CalibrationLawVerification2);

        #endregion

        #region Operator >  (CalibrationLawVerification1, CalibrationLawVerification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CalibrationLawVerification1">A calibration law verification.</param>
        /// <param name="CalibrationLawVerification2">Another calibration law verification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (CalibrationLawVerification CalibrationLawVerification1,
                                          CalibrationLawVerification CalibrationLawVerification2)
        {

            if (CalibrationLawVerification1 is null)
                throw new ArgumentNullException(nameof(CalibrationLawVerification1), "The given calibration law verification must not be null!");

            return CalibrationLawVerification1.CompareTo(CalibrationLawVerification2) > 0;

        }

        #endregion

        #region Operator >= (CalibrationLawVerification1, CalibrationLawVerification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CalibrationLawVerification1">A calibration law verification.</param>
        /// <param name="CalibrationLawVerification2">Another calibration law verification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (CalibrationLawVerification CalibrationLawVerification1,
                                           CalibrationLawVerification CalibrationLawVerification2)

            => !(CalibrationLawVerification1 < CalibrationLawVerification2);

        #endregion

        #endregion

        #region IComparable<CalibrationLawVerification> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is CalibrationLawVerification CalibrationLawVerification
                   ? CompareTo(CalibrationLawVerification)
                   : throw new ArgumentException("The given object is not a calibration law verification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(CalibrationLawVerification)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CalibrationLawVerification">An object to compare with.</param>
        public Int32 CompareTo(CalibrationLawVerification CalibrationLawVerification)
        {

            var result = String.Compare(PublicKey,                                    CalibrationLawVerification.PublicKey);

            if (result == 0)
                result = String.Compare(CalibrationLawCertificateId,                  CalibrationLawVerification.CalibrationLawCertificateId);

            if (result == 0 && MeteringSignatureURL.HasValue && CalibrationLawVerification.MeteringSignatureURL.HasValue)
                result =                MeteringSignatureURL.Value.CompareTo(         CalibrationLawVerification.MeteringSignatureURL.Value);

            if (result == 0)
                result = String.Compare(MeteringSignatureEncodingFormat,              CalibrationLawVerification.MeteringSignatureEncodingFormat);

            if (result == 0)
                result = String.Compare(SignedMeteringValuesVerificationInstruction,  CalibrationLawVerification.SignedMeteringValuesVerificationInstruction);

            return result;

        }

        #endregion

        #endregion

        #region IEquatable<CalibrationLawVerification> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is CalibrationLawVerification calibrationLawVerificationute &&
                   Equals(calibrationLawVerificationute);

        #endregion

        #region Equals(CalibrationLawVerification)

        /// <summary>
        /// Compares two CalibrationLawVerifications for equality.
        /// </summary>
        /// <param name="CalibrationLawVerification">A calibration law verification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(CalibrationLawVerification CalibrationLawVerification)

            => String.Equals(PublicKey,                                    CalibrationLawVerification.PublicKey)                                   &&
               String.Equals(CalibrationLawCertificateId,                  CalibrationLawVerification.CalibrationLawCertificateId)                 &&
               String.Equals(MeteringSignatureEncodingFormat,              CalibrationLawVerification.MeteringSignatureEncodingFormat)             &&
               String.Equals(SignedMeteringValuesVerificationInstruction,  CalibrationLawVerification.SignedMeteringValuesVerificationInstruction) &&

             ((!MeteringSignatureURL.HasValue && !CalibrationLawVerification.MeteringSignatureURL.HasValue) ||
               (MeteringSignatureURL.HasValue &&  CalibrationLawVerification.MeteringSignatureURL.HasValue &&
                             MeteringSignatureURL.Equals(                  CalibrationLawVerification.MeteringSignatureURL)));

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return (PublicKey?.                                  GetHashCode() ?? 0) * 11 ^
                       (CalibrationLawCertificateId?.                GetHashCode() ?? 0) *  7 ^
                       (MeteringSignatureURL?.                       GetHashCode() ?? 0) *  5 ^
                       (MeteringSignatureEncodingFormat?.            GetHashCode() ?? 0) *  3 ^
                       (SignedMeteringValuesVerificationInstruction?.GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => new String[] {
                   PublicKey.                                  IsNotNullOrEmpty() ? "pk: "       + PublicKey.                                  SubstringMax(10) : "",
                   CalibrationLawCertificateId.                IsNotNullOrEmpty() ? "certId: "   + CalibrationLawCertificateId.                SubstringMax(10) : "",
                   MeteringSignatureURL.                       HasValue           ? "URL: "      + MeteringSignatureURL.Value.ToString().      SubstringMax(10) : "",
                   MeteringSignatureEncodingFormat.            IsNotNullOrEmpty() ? "encoding: " + MeteringSignatureEncodingFormat.            SubstringMax(10) : "",
                   SignedMeteringValuesVerificationInstruction.IsNotNullOrEmpty() ? "info: "     + SignedMeteringValuesVerificationInstruction.SubstringMax(10) : ""
               }.AggregateWith(", ");

        #endregion

    }

}
