/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

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
        public String?   CalibrationLawCertificateId                    { get; }

        /// <summary>
        /// Unique PublicKey for EVSEID (or the smart energy meter within the charging station) can be provided here.
        /// </summary>
        [Optional]
        public String?   PublicKey                                      { get; }

        /// <summary>
        /// In this field CPO can also provide an url to a external data set. This data set can give calibration law information which can be simply added to the end customer invoice of the EMP.
        /// The information can contain for eg Charging Station Details, Charging Session Date/Time, SignedMeteringValues(Transparency Software format), SignedMeterValuesVerificationInstruction etc.
        /// </summary>
        [Optional]
        public URL?      MeteringSignatureURL                           { get; }

        /// <summary>
        /// Encoding format of the metering signature data as well as the version.
        /// </summary>
        /// <example>EDL40 Mennekes: V1</example>
        [Optional]
        public String?   MeteringSignatureEncodingFormat                { get; }

        /// <summary>
        /// Additional information (e.g. instruction on how to use the transparency software).
        /// </summary>
        [Optional]
        public String?   SignedMeteringValuesVerificationInstruction    { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject?  CustomData                                     { get; }


        /// <summary>
        /// Whether all sub-datastructures are empty/null.
        /// </summary>
        public Boolean IsEmpty

            => CalibrationLawCertificateId                 is null &&
               PublicKey                                   is null &&
              !MeteringSignatureURL.HasValue                       &&
               MeteringSignatureEncodingFormat             is null &&
               SignedMeteringValuesVerificationInstruction is null;

        /// <summary>
        /// Whether NOT all sub-datastructures are empty/null.
        /// </summary>
        public Boolean IsNotEmpty
            => !IsEmpty;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new calibration law verification.
        /// </summary>
        /// <param name="CalibrationLawCertificateId">The Calibration Law Compliance ID from respective authority along with the revision and issueing date.</param>
        /// <param name="PublicKey">Unique PublicKey for EVSEID (or the smart energy meter within the charging station) can be provided here.</param>
        /// <param name="MeteringSignatureURL">In this field CPO can also provide an url to a external data set. This data set can give calibration law information which can be simply added to the end customer invoice of the EMP.</param>
        /// <param name="MeteringSignatureEncodingFormat">Encoding format of the metering signature data as well as the version.</param>
        /// <param name="SignedMeteringValuesVerificationInstruction">Additional information (e.g. instruction on how to use the transparency software).</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public CalibrationLawVerification(String?  CalibrationLawCertificateId                   = null,
                                          String?  PublicKey                                     = null,
                                          URL?     MeteringSignatureURL                          = null,
                                          String?  MeteringSignatureEncodingFormat               = null,
                                          String?  SignedMeteringValuesVerificationInstruction   = null,
                                          JObject? CustomData                                    = null)

        {

            this.CalibrationLawCertificateId                  = CalibrationLawCertificateId;
            this.PublicKey                                    = PublicKey;
            this.MeteringSignatureURL                         = MeteringSignatureURL;
            this.MeteringSignatureEncodingFormat              = MeteringSignatureEncodingFormat;
            this.SignedMeteringValuesVerificationInstruction  = SignedMeteringValuesVerificationInstruction;
            this.CustomData                                   = CustomData;

            unchecked
            {

                hashCode = (this.PublicKey?.                                  GetHashCode() ?? 0) * 11 ^
                           (this.CalibrationLawCertificateId?.                GetHashCode() ?? 0) *  7 ^
                           (this.MeteringSignatureURL?.                       GetHashCode() ?? 0) *  5 ^
                           (this.MeteringSignatureEncodingFormat?.            GetHashCode() ?? 0) *  3 ^
                           (this.SignedMeteringValuesVerificationInstruction?.GetHashCode() ?? 0);

            }

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#CalibrationLawVerificationType

        // {
        //   "CalibrationLawCertificateID":                  "string",
        //   "PublicKey":                                    "string",
        //   "MeteringSignatureUrl":                         "string",
        //   "MeteringSignatureEncodingFormat":              "string",
        //   "SignedMeteringValuesVerificationInstruction":  "string"
        // }

        #endregion

        #region (static) Parse   (JSON, CustomCalibrationLawVerificationParser = null)

        /// <summary>
        /// Parse the given JSON representation of a calibration law verification.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomCalibrationLawVerificationParser">A delegate to parse custom calibration law verifications JSON objects.</param>
        public static CalibrationLawVerification Parse(JObject                                                   JSON,
                                                       CustomJObjectParserDelegate<CalibrationLawVerification>?  CustomCalibrationLawVerificationParser   = null)
        {

            if (TryParse(JSON,
                         out var calibrationLawVerification,
                         out var errorResponse,
                         CustomCalibrationLawVerificationParser))
            {
                return calibrationLawVerification;
            }

            throw new ArgumentException("The given JSON representation of a calibration law verification is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out CalibrationLawVerification, out ErrorResponse, CustomCalibrationLawVerificationParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a calibration law verification.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CalibrationLawVerification">The parsed calibration law verification.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                               JSON,
                                       [NotNullWhen(true)]  out CalibrationLawVerification?  CalibrationLawVerification,
                                       [NotNullWhen(false)] out String?                      ErrorResponse)

            => TryParse(JSON,
                        out CalibrationLawVerification,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a calibration law verification.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CalibrationLawVerification">The parsed calibration law verification.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomCalibrationLawVerificationParser">A delegate to parse custom calibration law verifications JSON objects.</param>
        public static Boolean TryParse(JObject                                                   JSON,
                                       [NotNullWhen(true)]  out CalibrationLawVerification?      CalibrationLawVerification,
                                       [NotNullWhen(false)] out String?                          ErrorResponse,
                                       CustomJObjectParserDelegate<CalibrationLawVerification>?  CustomCalibrationLawVerificationParser)
        {

            try
            {

                CalibrationLawVerification  = default;
                ErrorResponse               = default;

                #region Parse CalibrationLawCertificateId                     [optional]

                if (JSON.ParseOptional("CalibrationLawCertificateID",
                                       "calibration law certificate identification",
                                       out String? CalibrationLawCertificateId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse PublicKey                                       [optional]

                if (JSON.ParseOptional("PublicKey",
                                       "public key",
                                       out String? PublicKey,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse MeteringSignatureURL                            [optional]

                if (JSON.ParseOptional("MeteringSignatureUrl",
                                       "metering signature URL",
                                       URL.TryParse,
                                       out URL? MeteringSignatureURL,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse MeteringSignatureEncodingFormat                 [optional]

                if (JSON.ParseOptional("MeteringSignatureEncodingFormat",
                                       "metering signature encoding format",
                                       out String? MeteringSignatureEncodingFormat,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse SignedMeteringValuesVerificationInstruction     [optional]

                if (JSON.ParseOptional("SignedMeteringValuesVerificationInstruction",
                                       "signed metering values verification instruction",
                                       out String? SignedMeteringValuesVerificationInstruction,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CustomData                                      [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                CalibrationLawVerification  = new CalibrationLawVerification(
                                                  CalibrationLawCertificateId,
                                                  PublicKey,
                                                  MeteringSignatureURL,
                                                  MeteringSignatureEncodingFormat,
                                                  SignedMeteringValuesVerificationInstruction,
                                                  customData
                                              );

                if (CustomCalibrationLawVerificationParser is not null)
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

        #region ToJSON(CustomCalibrationLawVerificationSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomCalibrationLawVerificationSerializer">A delegate to serialize custom calibration law verification JSON objects.</param>
        public JObject? ToJSON(CustomJObjectSerializerDelegate<CalibrationLawVerification>?  CustomCalibrationLawVerificationSerializer   = null)
        {

            var json = JSONObject.Create(

                           CalibrationLawCertificateId                 is not null && CalibrationLawCertificateId.                IsNotNullOrEmpty()
                               ? new JProperty("CalibrationLawCertificateID",                  CalibrationLawCertificateId)
                               : null,

                           PublicKey                                   is not null && PublicKey.                                  IsNotNullOrEmpty()
                               ? new JProperty("PublicKey",                                    PublicKey)
                               : null,

                           MeteringSignatureURL.HasValue
                               ? new JProperty("MeteringSignatureUrl",                         MeteringSignatureURL.Value.ToString())
                               : null,

                           MeteringSignatureEncodingFormat             is not null && MeteringSignatureEncodingFormat.            IsNotNullOrEmpty()
                               ? new JProperty("MeteringSignatureEncodingFormat",              MeteringSignatureEncodingFormat)
                               : null,

                           SignedMeteringValuesVerificationInstruction is not null && SignedMeteringValuesVerificationInstruction.IsNotNullOrEmpty()
                               ? new JProperty("SignedMeteringValuesVerificationInstruction",  SignedMeteringValuesVerificationInstruction)
                               : null,

                           CustomData?.HasValues == true
                               ? new JProperty(nameof(CustomData),                             CustomData)
                               : null

                       );

            var JSON2 = CustomCalibrationLawVerificationSerializer is not null
                            ? CustomCalibrationLawVerificationSerializer(this, json)
                            : json;

            return JSON2.HasValues
                       ? JSON2
                       : null;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public CalibrationLawVerification Clone

            => new (CalibrationLawCertificateId                 is not null ? new String(CalibrationLawCertificateId.                ToCharArray()) : null,
                    PublicKey                                   is not null ? new String(PublicKey.                                  ToCharArray()) : null,
                    MeteringSignatureURL?.Clone,
                    MeteringSignatureEncodingFormat             is not null ? new String(MeteringSignatureEncodingFormat.            ToCharArray()) : null,
                    SignedMeteringValuesVerificationInstruction is not null ? new String(SignedMeteringValuesVerificationInstruction.ToCharArray()) : null,
                    CustomData                                  is not null ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))   : null);

        #endregion


        #region Operator overloading

        #region Operator == (CalibrationLawVerification1, CalibrationLawVerification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CalibrationLawVerification1">A calibration law verification.</param>
        /// <param name="CalibrationLawVerification2">Another calibration law verification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (CalibrationLawVerification? CalibrationLawVerification1,
                                           CalibrationLawVerification? CalibrationLawVerification2)
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
        public static Boolean operator != (CalibrationLawVerification? CalibrationLawVerification1,
                                           CalibrationLawVerification? CalibrationLawVerification2)

            => !(CalibrationLawVerification1 == CalibrationLawVerification2);

        #endregion

        #region Operator <  (CalibrationLawVerification1, CalibrationLawVerification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CalibrationLawVerification1">A calibration law verification.</param>
        /// <param name="CalibrationLawVerification2">Another calibration law verification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (CalibrationLawVerification? CalibrationLawVerification1,
                                          CalibrationLawVerification? CalibrationLawVerification2)
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
        public static Boolean operator <= (CalibrationLawVerification? CalibrationLawVerification1,
                                           CalibrationLawVerification? CalibrationLawVerification2)

            => !(CalibrationLawVerification1 > CalibrationLawVerification2);

        #endregion

        #region Operator >  (CalibrationLawVerification1, CalibrationLawVerification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CalibrationLawVerification1">A calibration law verification.</param>
        /// <param name="CalibrationLawVerification2">Another calibration law verification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (CalibrationLawVerification? CalibrationLawVerification1,
                                          CalibrationLawVerification? CalibrationLawVerification2)
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
        public static Boolean operator >= (CalibrationLawVerification? CalibrationLawVerification1,
                                           CalibrationLawVerification? CalibrationLawVerification2)

            => !(CalibrationLawVerification1 < CalibrationLawVerification2);

        #endregion

        #endregion

        #region IComparable<CalibrationLawVerification> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two calibration law verifications.
        /// </summary>
        /// <param name="Object">A calibration law verification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is CalibrationLawVerification CalibrationLawVerification
                   ? CompareTo(CalibrationLawVerification)
                   : throw new ArgumentException("The given object is not a calibration law verification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(CalibrationLawVerification)

        /// <summary>
        /// Compares two calibration law verifications.
        /// </summary>
        /// <param name="CalibrationLawVerification">A calibration law verification to compare with.</param>
        public Int32 CompareTo(CalibrationLawVerification? CalibrationLawVerification)
        {

            var c = String.Compare(PublicKey,                                    CalibrationLawVerification?.PublicKey);

            if (c == 0)
                c = String.Compare(CalibrationLawCertificateId,                  CalibrationLawVerification?.CalibrationLawCertificateId);

            if (c == 0 && MeteringSignatureURL.HasValue && CalibrationLawVerification?.MeteringSignatureURL.HasValue == true)
                c =                MeteringSignatureURL.Value.CompareTo(         CalibrationLawVerification.MeteringSignatureURL.Value);

            if (c == 0)
                c = String.Compare(MeteringSignatureEncodingFormat,              CalibrationLawVerification?.MeteringSignatureEncodingFormat);

            if (c == 0)
                c = String.Compare(SignedMeteringValuesVerificationInstruction,  CalibrationLawVerification?.SignedMeteringValuesVerificationInstruction);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<CalibrationLawVerification> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two calibration law verifications for equality.
        /// </summary>
        /// <param name="Object">A calibration law verification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is CalibrationLawVerification calibrationLawVerification &&
                   Equals(calibrationLawVerification);

        #endregion

        #region Equals(CalibrationLawVerification)

        /// <summary>
        /// Compares two calibration law verifications for equality.
        /// </summary>
        /// <param name="CalibrationLawVerification">A calibration law verification to compare with.</param>
        public Boolean Equals(CalibrationLawVerification? CalibrationLawVerification)

            => CalibrationLawVerification is not null &&

               String.Equals(PublicKey,                                    CalibrationLawVerification?.PublicKey)                                   &&
               String.Equals(CalibrationLawCertificateId,                  CalibrationLawVerification?.CalibrationLawCertificateId)                 &&
               String.Equals(MeteringSignatureEncodingFormat,              CalibrationLawVerification?.MeteringSignatureEncodingFormat)             &&
               String.Equals(SignedMeteringValuesVerificationInstruction,  CalibrationLawVerification?.SignedMeteringValuesVerificationInstruction) &&

             ((!MeteringSignatureURL.HasValue && !CalibrationLawVerification?.MeteringSignatureURL.HasValue == true) ||
               (MeteringSignatureURL.HasValue &&  CalibrationLawVerification?.MeteringSignatureURL.HasValue == true &&
                             MeteringSignatureURL.Equals(CalibrationLawVerification.MeteringSignatureURL)));

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

            => new String?[] {
                   PublicKey                                   is not null && PublicKey.                                  IsNotNullOrEmpty() ? "pk: "       + PublicKey.                                  SubstringMax(10) : null,
                   CalibrationLawCertificateId                 is not null && CalibrationLawCertificateId.                IsNotNullOrEmpty() ? "certId: "   + CalibrationLawCertificateId.                SubstringMax(10) : null,
                   MeteringSignatureURL                        is not null && MeteringSignatureURL.                       HasValue           ? "URL: "      + MeteringSignatureURL.Value.ToString().      SubstringMax(10) : null,
                   MeteringSignatureEncodingFormat             is not null && MeteringSignatureEncodingFormat.            IsNotNullOrEmpty() ? "encoding: " + MeteringSignatureEncodingFormat.            SubstringMax(10) : null,
                   SignedMeteringValuesVerificationInstruction is not null && SignedMeteringValuesVerificationInstruction.IsNotNullOrEmpty() ? "info: "     + SignedMeteringValuesVerificationInstruction.SubstringMax(10) : null
               }.Where(text => text is not null).
                 AggregateWith(", ");

        #endregion

    }

}
