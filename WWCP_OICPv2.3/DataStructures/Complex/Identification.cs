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

using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// An user identification (RFID or EVCO identification).
    /// </summary>
    public class Identification : IEquatable<Identification>,
                                  IComparable<Identification>,
                                  IComparable
    {

        #region Properties

        /// <summary>
        /// A RFID Mifare identification.
        /// </summary>
        [Optional]
        public UID?                   RFIDId                         { get; }

        /// <summary>
        /// A RFID identification.
        /// </summary>
        [Optional]
        public RFIDIdentification?    RFIDIdentification             { get; }

        /// <summary>
        /// An e-mobility account identification (EVCO Id) and a (hashed) PIN.
        /// </summary>
        [Optional]
        public QRCodeIdentification?  QRCodeIdentification           { get; }

        /// <summary>
        /// A plug'n'charge identification (EVCO Id).
        /// </summary>
        [Optional]
        public EVCO_Id?               PlugAndChargeIdentification    { get; }

        /// <summary>
        /// A remote identification (EVCO Id).
        /// </summary>
        [Optional]
        public EVCO_Id?               RemoteIdentification           { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject?               CustomData                     { get; }


        #region IsNullOrEmpty

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty

            => !RFIDId.                     HasValue &&
                RFIDIdentification          is null  &&
               !QRCodeIdentification.       HasValue &&
               !PlugAndChargeIdentification.HasValue &&
               !RemoteIdentification.       HasValue;

        #endregion

        #region IsNotNullOrEmpty

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty

            => RFIDId.                     HasValue    ||
               RFIDIdentification          is not null ||
               QRCodeIdentification.       HasValue    ||
               PlugAndChargeIdentification.HasValue    ||
               RemoteIdentification.       HasValue;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new user identification (RFID or EVCO identification).
        /// </summary>
        /// <param name="RFIDId">A RFID Mifare identification.</param>
        /// <param name="RFIDIdentification">A RFID identification.</param>
        /// <param name="QRCodeIdentification">An e-mobility account identification (EVCO Id) and a (hashed) PIN.</param>
        /// <param name="PlugAndChargeIdentification">A plug'n'charge identification (EVCO Id).</param>
        /// <param name="RemoteIdentification">A remote identification (EVCO Id).</param>
        /// 
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        private Identification(UID?                   RFIDId                        = null,
                               RFIDIdentification?    RFIDIdentification            = null,
                               QRCodeIdentification?  QRCodeIdentification          = null,
                               EVCO_Id?               PlugAndChargeIdentification   = null,
                               EVCO_Id?               RemoteIdentification          = null,

                               JObject?               CustomData                    = null)
        {

            this.RFIDId                       = RFIDId;
            this.RFIDIdentification           = RFIDIdentification;
            this.QRCodeIdentification         = QRCodeIdentification;
            this.PlugAndChargeIdentification  = PlugAndChargeIdentification;
            this.RemoteIdentification         = RemoteIdentification;

            this.CustomData                   = CustomData;


            unchecked
            {

                hashCode = (this.RFIDId?.                     GetHashCode() ?? 0) * 11 ^
                           (this.RFIDIdentification?.         GetHashCode() ?? 0) *  7 ^
                           (this.QRCodeIdentification?.       GetHashCode() ?? 0) *  5 ^
                           (this.PlugAndChargeIdentification?.GetHashCode() ?? 0) *  3 ^
                           (this.RemoteIdentification?.       GetHashCode() ?? 0);

            }

        }

        #endregion


        #region (static) FromUID                         (MifareUID,                       CustomData = null)

        /// <summary>
        /// Create a new Mifare identification.
        /// </summary>
        /// <param name="MifareUID">A Mifare user identification.</param>
        /// <param name="CustomData">Optional customer specific data.</param>
        public static Identification FromUID(UID       MifareUID,
                                             JObject?  CustomData  = null)

            => new (RFIDId:      MifareUID,
                    CustomData:  CustomData);

        #endregion

        #region (static) FromRFIDIdentification          (RFIDIdentification,              CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="RFIDIdentification">An RFID identification.</param>
        /// <param name="CustomData">Optional customer specific data.</param>
        public static Identification FromRFIDIdentification(RFIDIdentification  RFIDIdentification,
                                                            JObject?            CustomData  = null)

            => new (RFIDIdentification:  RFIDIdentification,
                    CustomData:          CustomData);

        #endregion

        #region (static) FromQRCodeIdentification        (QRCodeIdentification,            CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="QRCodeIdentification">A QR-code identification (EVCO Id).</param>
        /// <param name="CustomData">Optional customer specific data.</param>
        public static Identification FromQRCodeIdentification(QRCodeIdentification  QRCodeIdentification,
                                                              JObject?              CustomData   = null)

            => new (QRCodeIdentification:  QRCodeIdentification,
                    CustomData:            CustomData);

        #endregion

        #region (static) FromQRCodeIdentification        (EVCOId, PIN,                     CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="EVCOId">An electric vehicle contract identification (EVCO Id).</param>
        /// <param name="PIN">A PIN.</param>
        /// <param name="CustomData">Optional customer specific data.</param>
        public static Identification FromQRCodeIdentification(EVCO_Id   EVCOId,
                                                              PIN       PIN,
                                                              JObject?  CustomData   = null)

            => new (QRCodeIdentification:  new QRCodeIdentification(
                                               EVCOId,
                                               PIN
                                           ),
                    CustomData:            CustomData);

        #endregion

        #region (static) FromQRCodeIdentification        (EVCOId, HashedPIN,               CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="EVCOId">A QR code identification.</param>
        /// <param name="HashedPIN">A hashed PIN.</param>
        /// <param name="CustomData">Optional customer specific data.</param>
        public static Identification FromQRCodeIdentification(EVCO_Id    EVCOId,
                                                              HashedPIN  HashedPIN,
                                                              JObject?   CustomData   = null)

            => new (QRCodeIdentification:  new QRCodeIdentification(
                                               EVCOId,
                                               HashedPIN
                                           ),
                    CustomData:            CustomData);

        #endregion

        #region (static) FromQRCodeIdentification        (EVCOId, HashValue, HashFunction, CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="EVCOId">A QR code identification.</param>
        /// <param name="HashValue">Hash value created by partner.</param>
        /// <param name="HashFunction">Function that was used to generate the hash value.</param>
        /// <param name="CustomData">Optional customer specific data.</param>
        public static Identification FromQRCodeIdentification(EVCO_Id        EVCOId,
                                                              Hash_Value     HashValue,
                                                              HashFunctions  HashFunction,
                                                              JObject?       CustomData   = null)

            => new (QRCodeIdentification:  new QRCodeIdentification(
                                               EVCOId,
                                               new HashedPIN(
                                                   HashValue,
                                                   HashFunction
                                               )
                                           ),
                    CustomData:            CustomData);

        #endregion

        #region (static) FromPlugAndChargeIdentification (PlugAndChargeIdentification,     CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="PlugAndChargeIdentification">A plug'n'charge identification (EVCO Id).</param>
        /// <param name="CustomData">Optional customer specific data.</param>
        public static Identification FromPlugAndChargeIdentification(EVCO_Id   PlugAndChargeIdentification,
                                                                     JObject?  CustomData   = null)

            => new (PlugAndChargeIdentification:  PlugAndChargeIdentification,
                    CustomData:                   CustomData);

        #endregion

        #region (static) FromRemoteIdentification        (RemoteIdentification,            CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="RemoteIdentification">A remote identification (EVCO Id).</param>
        /// <param name="CustomData">Optional customer specific data.</param>
        public static Identification FromRemoteIdentification(EVCO_Id   RemoteIdentification,
                                                              JObject?  CustomData   = null)

            => new (RemoteIdentification:  RemoteIdentification,
                    CustomData:            CustomData);

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#IdentificationType

        // {
        //   "RFIDMifareFamilyIdentification": {
        //     "UID": "string"
        //   },
        //   "QRCodeIdentification": {
        //     "EvcoID": "string",
        //     "HashedPIN": {
        //       "Function": "Bcrypt",
        //       "LegacyHashData": {
        //         "Function": "MD5",
        //         "Salt": "string",
        //         "Value": "string"
        //       },
        //       "Value": "string"
        //     },
        //     "PIN": "string"
        //   },
        //   "PlugAndChargeIdentification": {
        //     "EvcoID": "string"
        //   },
        //   "RemoteIdentification": {
        //     "EvcoID": "string"
        //   },
        //   "RFIDIdentification": {
        //     "EvcoID": "string",
        //     "ExpiryDate": "2020-12-24T07:17:30.354Z",
        //     "PrintedNumber": "string",
        //     "RFID": "mifareCls",
        //     "UID": "string"
        //   }
        // }

        #endregion

        #region (static) Parse   (JSON, CustomIdentificationParser = null)

        /// <summary>
        /// Parse the given JSON representation of an identification.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom identification JSON objects.</param>
        public static Identification Parse(JObject                                       JSON,
                                           CustomJObjectParserDelegate<Identification>?  CustomIdentificationParser   = null)
        {

            if (TryParse(JSON,
                         out var identification,
                         out var errorResponse,
                         CustomIdentificationParser))
            {
                return identification;
            }

            throw new ArgumentException("The given JSON representation of an identification is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out Identification, out ErrorResponse, CustomIdentificationParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an identification.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="Identification">The parsed identification.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                   JSON,
                                       [NotNullWhen(true)]  out Identification?  Identification,
                                       [NotNullWhen(false)] out String?          ErrorResponse)

            => TryParse(JSON,
                        out Identification,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an identification.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="Identification">The parsed identification.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom identification JSON objects.</param>
        public static Boolean TryParse(JObject                                       JSON,
                                       [NotNullWhen(true)]  out Identification?      Identification,
                                       [NotNullWhen(false)] out String?              ErrorResponse,
                                       CustomJObjectParserDelegate<Identification>?  CustomIdentificationParser)
        {

            try
            {

                Identification = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse RFIDMifareFamilyIdentification    [optional]

                UID? UID = default;

                if (JSON.ParseOptional("RFIDMifareFamilyIdentification",
                                       "RFID mifare family identification",
                                       out JObject RFIDMifareFamilyIdentification,
                                       out ErrorResponse))
                {

                    if (ErrorResponse is not null)
                        return false;

                    if (!RFIDMifareFamilyIdentification.ParseMandatory("UID",
                                                                       "RFID mifare family identification -> UID",
                                                                       OICPv2_3.UID.TryParse,
                                                                       out UID uid,
                                                                       out ErrorResponse))
                    {
                        return false;
                    }

                    UID = uid;

                }

                #endregion

                #region Parse RFIDIdentification                [optional]

                if (JSON.ParseOptionalJSON("RFIDIdentification",
                                           "RFID identification",
                                           OICPv2_3.RFIDIdentification.TryParse,
                                           out RFIDIdentification? RFIDIdentification,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse QRCodeIdentification              [optional]

                if (JSON.ParseOptionalJSON("QRCodeIdentification",
                                           "QR code identification",
                                           OICPv2_3.QRCodeIdentification.TryParse,
                                           out QRCodeIdentification? QRCodeIdentification,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse PlugAndChargeIdentification       [optional]

                EVCO_Id? PnC_EVCOId = default;

                if (JSON.ParseOptional("PlugAndChargeIdentification",
                                       "plug & charge identification",
                                       out JObject plugAndChargeIdentification,
                                       out ErrorResponse))
                {

                    if (ErrorResponse is not null)
                        return false;

                    if (!plugAndChargeIdentification.ParseMandatory("EvcoID",
                                                                    "plug & charge identification -> EVCOId",
                                                                    EVCO_Id.TryParse,
                                                                    out EVCO_Id evcoId,
                                                                    out ErrorResponse))
                    {
                        return false;
                    }

                    PnC_EVCOId = evcoId;

                }

                #endregion

                #region Parse RemoteIdentification              [optional]

                EVCO_Id? Remote_EVCOId = default;

                if (JSON.ParseOptional("RemoteIdentification",
                                       "remote identification",
                                       out JObject RemoteIdentification,
                                       out ErrorResponse))
                {

                    if (ErrorResponse is not null)
                        return false;

                    if (!RemoteIdentification.ParseMandatory("EvcoID",
                                                             "remote identification -> EVCOId",
                                                             EVCO_Id.TryParse,
                                                             out EVCO_Id evcoId,
                                                             out ErrorResponse))
                    {
                        return false;
                    }

                    Remote_EVCOId = evcoId;

                }

                #endregion


                Identification = new Identification(
                                     UID,
                                     RFIDIdentification,
                                     QRCodeIdentification,
                                     PnC_EVCOId,
                                     Remote_EVCOId
                                 );


                if (CustomIdentificationParser is not null)
                    Identification = CustomIdentificationParser(JSON,
                                                                Identification);

                return true;

            }
            catch (Exception e)
            {
                Identification  = default;
                ErrorResponse   = "The given JSON representation of an identification is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomIdentificationSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON objects.</param>
        public JObject? ToJSON(CustomJObjectSerializerDelegate<Identification>?  CustomIdentificationSerializer   = null)

        {

            var json = JSONObject.Create(

                           RFIDId.HasValue
                               ? new JProperty("RFIDMifareFamilyIdentification",  new JObject(
                                                                                      new JProperty("UID",    RFIDId.                     Value.ToString())
                                                                                  ))
                               : null,

                           RFIDIdentification is not null
                               ? new JProperty("RFIDIdentification",              RFIDIdentification.               ToJSON())
                               : null,

                           QRCodeIdentification.HasValue
                               ? new JProperty("QRCodeIdentification",            QRCodeIdentification.       Value.ToJSON())
                               : null,

                           PlugAndChargeIdentification.HasValue
                               ? new JProperty("PlugAndChargeIdentification",     new JObject(
                                                                                      new JProperty("EvcoID", PlugAndChargeIdentification.Value.ToString())
                                                                                  ))
                               : null,

                           RemoteIdentification.HasValue
                               ? new JProperty("RemoteIdentification",            new JObject(
                                                                                      new JProperty("EvcoID", RemoteIdentification.       Value.ToString())
                                                                                  ))
                               : null);

            var JSON2 = CustomIdentificationSerializer is not null
                            ? CustomIdentificationSerializer(this, json)
                            : json;

            return JSON2.HasValues
                       ? JSON2
                       : null;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this identification.
        /// </summary>
        public Identification Clone()

            => new (

                   RFIDId?.                     Clone(),
                   RFIDIdentification?.         Clone(),
                   QRCodeIdentification?.       Clone(),
                   PlugAndChargeIdentification?.Clone(),
                   RemoteIdentification?.       Clone(),

                   CustomData is not null
                       ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (Identification1, Identification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Identification1">An identification.</param>
        /// <param name="Identification2">Another identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Identification? Identification1,
                                           Identification? Identification2)
        {

            if (ReferenceEquals(Identification1, Identification2))
                return true;

            if (Identification1 is null || Identification2 is null)
                return false;

            return Identification1.Equals(Identification2);

        }

        #endregion

        #region Operator != (Identification1, Identification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Identification1">An identification.</param>
        /// <param name="Identification2">Another identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Identification? Identification1,
                                           Identification? Identification2)

            => !(Identification1 == Identification2);

        #endregion

        #region Operator <  (Identification1, Identification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Identification1">An identification.</param>
        /// <param name="Identification2">Another identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (Identification? Identification1,
                                          Identification? Identification2)
        {

            if (Identification1 is null)
                throw new ArgumentNullException(nameof(Identification1), "The given identification must not be null!");

            return Identification1.CompareTo(Identification2) < 0;

        }

        #endregion

        #region Operator <= (Identification1, Identification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Identification1">An identification.</param>
        /// <param name="Identification2">Another identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (Identification? Identification1,
                                           Identification? Identification2)

            => !(Identification1 > Identification2);

        #endregion

        #region Operator >  (Identification1, Identification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Identification1">An identification.</param>
        /// <param name="Identification2">Another identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (Identification? Identification1,
                                          Identification? Identification2)
        {

            if (Identification1 is null)
                throw new ArgumentNullException(nameof(Identification1), "The given identification must not be null!");

            return Identification1.CompareTo(Identification2) > 0;

        }

        #endregion

        #region Operator >= (Identification1, Identification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Identification1">An identification.</param>
        /// <param name="Identification2">Another identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (Identification? Identification1,
                                           Identification? Identification2)

            => !(Identification1 < Identification2);

        #endregion

        #endregion

        #region IComparable<Identification> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two identifications.
        /// </summary>
        /// <param name="Object">An identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Identification identification
                   ? CompareTo(identification)
                   : throw new ArgumentException("The given object is not an identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(Identification)

        /// <summary>
        /// Compares two identifications.
        /// </summary>
        /// <param name="Identification">An identification to compare with.</param>
        public Int32 CompareTo(Identification? Identification)
        {

            if (Identification is null)
                throw new ArgumentNullException(nameof(Identification), "The given identification must not be null!");

            if (RFIDId.                     HasValue && Identification.RFIDId.                     HasValue)
                return RFIDId.                     Value.CompareTo(Identification.RFIDId.                     Value);

            if (RFIDIdentification is not null       && Identification.RFIDIdentification is not null)
                return RFIDIdentification.               CompareTo(Identification.RFIDIdentification);

            if (QRCodeIdentification.       HasValue && Identification.QRCodeIdentification.       HasValue)
                return QRCodeIdentification.       Value.CompareTo(Identification.QRCodeIdentification.       Value);

            if (PlugAndChargeIdentification.HasValue && Identification.PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.Value.CompareTo(Identification.PlugAndChargeIdentification.Value);

            if (RemoteIdentification.       HasValue && Identification.RemoteIdentification.       HasValue)
                return RemoteIdentification.       Value.CompareTo(Identification.RemoteIdentification.       Value);


            return ToString().CompareTo(Identification.ToString());

        }

        #endregion

        #endregion

        #region IEquatable<Identification> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two identifications for equality.
        /// </summary>
        /// <param name="Object">An identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Identification identification &&
                   Equals(identification);

        #endregion

        #region Equals(Identification)

        /// <summary>
        /// Compares two identifications for equality.
        /// </summary>
        /// <param name="Identification">An identification to compare with.</param>
        public Boolean Equals(Identification? Identification)

            => Identification is not null &&

               ((!RFIDId.                     HasValue && !Identification.RFIDId.                     HasValue) ||
                 (RFIDId.                     HasValue &&  Identification.RFIDId.                     HasValue && RFIDId.Value.                     Equals(Identification.RFIDId.Value))) &&

                ((RFIDIdentification is null           &&  Identification.RFIDIdentification is null) ||
                 (RFIDIdentification is not null       &&  Identification.RFIDIdentification is not null       && RFIDIdentification.               Equals(Identification.RFIDIdentification))) &&

               ((!QRCodeIdentification.       HasValue && !Identification.QRCodeIdentification.       HasValue) ||
                 (QRCodeIdentification.       HasValue &&  Identification.QRCodeIdentification.       HasValue && QRCodeIdentification.Value.       Equals(Identification.QRCodeIdentification.Value))) &&

               ((!PlugAndChargeIdentification.HasValue && !Identification.PlugAndChargeIdentification.HasValue) ||
                 (PlugAndChargeIdentification.HasValue &&  Identification.PlugAndChargeIdentification.HasValue && PlugAndChargeIdentification.Value.Equals(Identification.PlugAndChargeIdentification.Value))) &&

               ((!RemoteIdentification.       HasValue && !Identification.RemoteIdentification.       HasValue) ||
                 (RemoteIdentification.       HasValue &&  Identification.RemoteIdentification.       HasValue && RemoteIdentification.Value.       Equals(Identification.RemoteIdentification.Value)));

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
        {

            if (RFIDId.HasValue)
                return RFIDId.                     Value.       ToString();

            if (RFIDIdentification is not null)
                return RFIDIdentification.               UID.   ToString();

            if (QRCodeIdentification.HasValue)
                return QRCodeIdentification.       Value.EVCOId.ToString();

            if (PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.Value.       ToString();

            if (RemoteIdentification.HasValue)
                return RemoteIdentification.       Value.       ToString();

            return String.Empty;

        }

        #endregion

    }

}
