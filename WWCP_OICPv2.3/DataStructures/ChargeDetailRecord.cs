﻿/*
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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// A charge detail record.
    /// </summary>
    public class ChargeDetailRecord : AInternalData,
                                      IEquatable<ChargeDetailRecord>,
                                      IComparable<ChargeDetailRecord>,
                                      IComparable
    {

        #region Properties

        /// <summary>
        /// The Hubject session identification, that identifies the charging process.
        /// </summary>
        [Mandatory]
        public Session_Id                        SessionId                          { get; }

        /// <summary>
        /// The EVSE identification, that identifies the location of the charging process.
        /// </summary>
        [Mandatory]
        public EVSE_Id                           EVSEId                             { get; }

        /// <summary>
        /// The authentication data used to authorize the user or the car.
        /// </summary>
        [Mandatory]
        public Identification                    Identification                     { get; }

        /// <summary>
        /// The timestamp when the charging session started.
        /// </summary>
        [Mandatory]
        public DateTime                          SessionStart                       { get; }

        /// <summary>
        /// The timestamp when the charging session ended.
        /// </summary>
        [Mandatory]
        public DateTime                          SessionEnd                         { get; }

        /// <summary>
        /// The timestamp when the charging process started.
        /// </summary>
        [Mandatory]
        public DateTime                          ChargingStart                      { get; }

        /// <summary>
        /// The timestamp when the charging process stopped.
        /// </summary>
        [Mandatory]
        public DateTime                          ChargingEnd                        { get; }

        /// <summary>
        /// The amount of consumed energy [kWh].
        /// </summary>
        [Mandatory]
        public WattHour                          ConsumedEnergy                     { get; }

        /// <summary>
        /// The optional pricing product name (for identifying a tariff) that must be unique.
        /// </summary>
        [Optional]
        public PartnerProduct_Id?                PartnerProductId                   { get; }

        /// <summary>
        /// The optional session identification assigned by the CPO partner.
        /// </summary>
        [Optional]
        public CPOPartnerSession_Id?             CPOPartnerSessionId                { get; }

        /// <summary>
        /// The optional session identification assigned by the EMP partner.
        /// </summary>
        [Optional]
        public EMPPartnerSession_Id?             EMPPartnerSessionId                { get; }

        /// <summary>
        /// The optional starting value of the energy meter [kWh].
        /// </summary>
        [Optional]
        public WattHour?                         MeterValueStart                    { get; }

        /// <summary>
        /// The optional ending value of the energy meter [kWh].
        /// </summary>
        [Optional]
        public WattHour?                         MeterValueEnd                      { get; }

        /// <summary>
        /// The optional enumeration of meter values during the charging session.
        /// </summary>
        [Optional]
        public IEnumerable<WattHour>             MeterValuesInBetween               { get; }

        /// <summary>
        /// Optional signed metering values, with can e.g. verified via a transparency software.
        /// </summary>
        [Optional]
        public IEnumerable<SignedMeteringValue>  SignedMeteringValues               { get; }

        /// <summary>
        /// Optional additional information which could directly or indirectly help to verify the
        /// signed metering values by using a valid transparency software.
        /// </summary>
        [Optional]
        public CalibrationLawVerification?       CalibrationLawVerificationInfo     { get; }

        /// <summary>
        /// The optional operator identification of the hub operator.
        /// </summary>
        [Optional]
        public Operator_Id?                      HubOperatorId                      { get; }

        /// <summary>
        /// The optional provider identification of the hub provider.
        /// </summary>
        [Optional]
        public Provider_Id?                      HubProviderId                      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charge detail record.
        /// </summary>
        /// <param name="SessionId">The Hubject session identification, that identifies the charging process.</param>
        /// <param name="EVSEId">The EVSE identification, that identifies the location of the charging process.</param>
        /// <param name="Identification">The authentication data used to authorize the user or the car.</param>
        /// <param name="SessionStart">The timestamp when the charging session started.</param>
        /// <param name="SessionEnd">The timestamp when the charging session ended.</param>
        /// <param name="ChargingStart">The timestamp when the charging process started.</param>
        /// <param name="ChargingEnd">The timestamp when the charging process stopped.</param>
        /// <param name="ConsumedEnergy">The amount of consumed energy [kWh].</param>
        /// 
        /// <param name="PartnerProductId">An optional pricing product name (for identifying a tariff) that must be unique.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification assigned by the CPO partner.</param>
        /// <param name="EMPPartnerSessionId">An optional session identification assigned by the EMP partner.</param>
        /// <param name="MeterValueStart">An optional starting value of the energy meter [kWh].</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter [kWh].</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session [kWh].</param>
        /// <param name="SignedMeteringValues">Optional signed metering values, with can e.g. verified via a transparency software.</param>
        /// <param name="CalibrationLawVerificationInfo">Optional additional information which could directly or indirectly help to verify the signed metering values by using a valid transparency software.</param>
        /// <param name="HubOperatorId">An optional operator identification of the hub operator.</param>
        /// <param name="HubProviderId">An optional provider identification of the hub provider.</param>
        /// 
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        /// <param name="InternalData">Optional internal customer specific data, e.g. in combination with custom parsers and serializers, which will not be serialized.</param>
        public ChargeDetailRecord(Session_Id                         SessionId,
                                  EVSE_Id                            EVSEId,
                                  Identification                     Identification,
                                  DateTime                           SessionStart,
                                  DateTime                           SessionEnd,
                                  DateTime                           ChargingStart,
                                  DateTime                           ChargingEnd,
                                  WattHour                           ConsumedEnergy,

                                  PartnerProduct_Id?                 PartnerProductId                 = null,
                                  CPOPartnerSession_Id?              CPOPartnerSessionId              = null,
                                  EMPPartnerSession_Id?              EMPPartnerSessionId              = null,
                                  WattHour?                          MeterValueStart                  = null,
                                  WattHour?                          MeterValueEnd                    = null,
                                  IEnumerable<WattHour>?             MeterValuesInBetween             = null,
                                  IEnumerable<SignedMeteringValue>?  SignedMeteringValues             = null,
                                  CalibrationLawVerification?        CalibrationLawVerificationInfo   = null,
                                  Operator_Id?                       HubOperatorId                    = null,
                                  Provider_Id?                       HubProviderId                    = null,

                                  JObject?                           CustomData                       = null,
                                  UserDefinedDictionary?             InternalData                     = null,
                                  DateTime?                          LastUpdate                       = null)

                : base(CustomData,
                       InternalData,
                       LastUpdate ?? Timestamp.Now)

        {

            this.EVSEId                          = EVSEId;
            this.SessionId                       = SessionId;
            this.Identification                  = Identification;
            this.SessionStart                    = SessionStart;
            this.SessionEnd                      = SessionEnd;
            this.ChargingStart                   = ChargingStart;
            this.ChargingEnd                     = ChargingEnd;
            this.ConsumedEnergy                  = ConsumedEnergy;

            this.PartnerProductId                = PartnerProductId;
            this.CPOPartnerSessionId             = CPOPartnerSessionId;
            this.EMPPartnerSessionId             = EMPPartnerSessionId;
            this.MeterValueStart                 = MeterValueStart;
            this.MeterValueEnd                   = MeterValueEnd;
            this.MeterValuesInBetween            = MeterValuesInBetween?.Distinct() ?? [];
            this.SignedMeteringValues            = SignedMeteringValues?.Distinct() ?? [];
            this.CalibrationLawVerificationInfo  = CalibrationLawVerificationInfo;
            this.HubOperatorId                   = HubOperatorId;
            this.HubProviderId                   = HubProviderId;


            unchecked
            {

                hashCode = SessionId.                      GetHashCode()        * 61 ^
                           EVSEId.                         GetHashCode()        * 59 ^
                           Identification.                 GetHashCode()        * 53 ^
                           SessionStart.                   GetHashCode()        * 47 ^
                           SessionEnd.                     GetHashCode()        * 43 ^
                           ChargingStart.                  GetHashCode()        * 41 ^
                           ChargingEnd.                    GetHashCode()        * 37 ^
                           ConsumedEnergy.                 GetHashCode()        * 31 ^

                          (PartnerProductId?.              GetHashCode()  ?? 0) * 29 ^
                          (CPOPartnerSessionId?.           GetHashCode()  ?? 0) * 23 ^
                          (EMPPartnerSessionId?.           GetHashCode()  ?? 0) * 19 ^
                          (MeterValueStart?.               GetHashCode()  ?? 0) * 17 ^
                          (MeterValueEnd?.                 GetHashCode()  ?? 0) * 13 ^

                          (MeterValuesInBetween?.          CalcHashCode() ?? 0) * 11 ^
                          (SignedMeteringValues?.          CalcHashCode() ?? 0) *  7 ^

                          (CalibrationLawVerificationInfo?.GetHashCode()  ?? 0) *  5 ^
                          (HubOperatorId?.                 GetHashCode()  ?? 0) *  3 ^
                          (HubProviderId?.                 GetHashCode()  ?? 0);

            }

        }

        #endregion


        #region Documentation

        // {
        //     "CPOPartnerSessionID":                               "string",
        //     "CalibrationLawVerificationInfo": {
        //         "CalibrationLawCertificateID":                   "string",
        //         "PublicKey":                                     "string",
        //         "MeteringSignatureUrl":                          "string",
        //         "MeteringSignatureEncodingFormat":               "string",
        //         "SignedMeteringValuesVerificationInstruction":   "string"
        //     },
        //     "ChargingEnd":                                       "2020-12-24T07:17:30.354Z",
        //     "ChargingStart":                                     "2020-12-24T07:17:30.354Z",
        //     "ConsumedEnergy":                                     0,
        //     "EMPPartnerSessionID":                               "string",
        //     "EvseID":                                            "string",
        //     "HubOperatorID":                                     "string",
        //     "HubProviderID":                                     "string",
        //     "Identification": {
        //         "RFIDMifareFamilyIdentification": {
        //             "UID":                                       "string"
        //         },
        //         "QRCodeIdentification": {
        //             "EvcoID":                                    "string",
        //             "HashedPIN": {
        //                 "Function":                              "Bcrypt",
        //                 "LegacyHashData": {
        //                     "Function":                          "MD5",
        //                     "Salt":                              "string",
        //                     "Value":                             "string"
        //                 },
        //                 "Value":                                 "string"
        //             },
        //             "PIN":                                       "string"
        //         },
        //         "PlugAndChargeIdentification": {
        //             "EvcoID":                                    "string"
        //         },
        //         "RemoteIdentification": {
        //             "EvcoID":                                    "string"
        //         },
        //         "RFIDIdentification": {
        //             "EvcoID":                                    "string",
        //             "ExpiryDate":                                "2020-12-24T07:17:30.354Z",
        //             "PrintedNumber":                             "string",
        //             "RFID":                                      "mifareCls",
        //             "UID":                                       "string"
        //         }
        //     },
        //     "MeterValueEnd":                                      0,
        //     "MeterValueInBetween": {
        //         "meterValues":                                   [ 0 ]
        //     },
        //     "MeterValueStart":                                    0,
        //     "PartnerProductID":                                  "string",
        //     "SessionEnd":                                        "2020-12-24T07:17:30.354Z",
        //     "SessionID":                                         "string",
        //     "SessionStart":                                      "2020-12-24T07:17:30.354Z",
        //     "SignedMeteringValues": [{
        //         "SignedMeteringValue":                           "string",
        //         "MeteringStatus":                                "Start"
        //     }]
        // }

        #endregion

        #region (static) Parse   (JSON, CustomChargeDetailRecordParser = null)

        /// <summary>
        /// Parse the given JSON representation of a charge detail record.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomChargeDetailRecordParser">A delegate to parse custom charge detail records JSON objects.</param>
        public static ChargeDetailRecord Parse(JObject                                           JSON,
                                               CustomJObjectParserDelegate<ChargeDetailRecord>?  CustomChargeDetailRecordParser   = null)
        {

            if (TryParse(JSON,
                         out var chargeDetailRecord,
                         out var errorResponse,
                         CustomChargeDetailRecordParser))
            {
                return chargeDetailRecord;
            }

            throw new ArgumentException("The given JSON representation of a charge detail record is invalid: " + errorResponse, nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out ChargeDetailRecord, out ErrorResponse, CustomChargeDetailRecordParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a charge detail record.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ChargeDetailRecord">The parsed charge detail record.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                       JSON,
                                       [NotNullWhen(true)]  out ChargeDetailRecord?  ChargeDetailRecord,
                                       [NotNullWhen(false)] out String?              ErrorResponse)

            => TryParse(JSON,
                        out ChargeDetailRecord,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a charge detail record.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ChargeDetailRecord">The parsed charge detail record.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomChargeDetailRecordParser">A delegate to parse custom charge detail records JSON objects.</param>
        public static Boolean TryParse(JObject                                           JSON,
                                       [NotNullWhen(true)]  out ChargeDetailRecord?      ChargeDetailRecord,
                                       [NotNullWhen(false)] out String?                  ErrorResponse,
                                       CustomJObjectParserDelegate<ChargeDetailRecord>?  CustomChargeDetailRecordParser)
        {

            try
            {

                ChargeDetailRecord = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse SessionId                         [mandatory]

                if (!JSON.ParseMandatory("SessionID",
                                         "session identification",
                                         Session_Id.TryParse,
                                         out Session_Id SessionId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse EVSEId                            [mandatory]

                if (!JSON.ParseMandatory("EvseID",
                                         "EVSE identification",
                                         EVSE_Id.TryParse,
                                         out EVSE_Id EVSEId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Identification                    [mandatory]

                if (!JSON.ParseMandatoryJSON("Identification",
                                             "identification",
                                             OICPv2_3.Identification.TryParse,
                                             out Identification? Identification,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse SessionStart                      [mandatory]

                if (!JSON.ParseMandatory("SessionStart",
                                         "session start",
                                         out DateTime SessionStart,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse SessionEnd                        [mandatory]

                if (!JSON.ParseMandatory("SessionEnd",
                                         "session end",
                                         out DateTime SessionEnd,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ChargingStart                     [mandatory => optional, because of Hubject data quality issues!]

                if (JSON.ParseOptional("ChargingStart",
                                       "charging start",
                                       out DateTime? ChargingStart,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ChargingEnd                       [mandatory => optional, because of Hubject data quality issues!]

                if (JSON.ParseOptional("ChargingEnd",
                                       "charging start",
                                       out DateTime? ChargingEnd,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ConsumedEnergy                    [mandatory]

                if (!JSON.ParseMandatory("ConsumedEnergy",
                                         "consumed energy",
                                         WattHour.TryParseKWh,
                                         out WattHour ConsumedEnergy,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse PartnerProductId                  [optional]

                if (JSON.ParseOptional("PartnerProductID",
                                       "partner product identification",
                                       PartnerProduct_Id.TryParse,
                                       out PartnerProduct_Id? PartnerProductId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CPOPartnerSessionId               [optional]

                if (JSON.ParseOptional("CPOPartnerSessionID",
                                       "CPO product session identification",
                                       CPOPartnerSession_Id.TryParse,
                                       out CPOPartnerSession_Id? CPOPartnerSessionId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse EMPPartnerSessionId               [optional]

                if (JSON.ParseOptional("EMPPartnerSessionID",
                                       "EMP product session identification",
                                       EMPPartnerSession_Id.TryParse,
                                       out EMPPartnerSession_Id? EMPPartnerSessionId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse MeterValueStart                   [optional]

                if (JSON.ParseOptional("MeterValueStart",
                                       "meter value start",
                                       WattHour.TryParseKWh,
                                       out WattHour? MeterValueStart,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse MeterValueEnd                     [optional]

                if (JSON.ParseOptional("MeterValueEnd",
                                       "meter value end",
                                       WattHour.TryParseKWh,
                                       out WattHour? MeterValueEnd,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse MeterValuesInBetween              [optional]

                List<WattHour>? MeterValuesInBetween = null;

                if (JSON.ParseOptional("MeterValueInBetween",
                                       "meter values in between",
                                       out JObject MeterValuesInBetweenJSON,
                                       out ErrorResponse))
                {

                    if (ErrorResponse is not null)
                        return false;

                    if (MeterValuesInBetweenJSON["meterValues"] is JArray meterValuesArray)
                    {

                        MeterValuesInBetween = [];

                        foreach (var meterValueJSON in meterValuesArray)
                        {
                            if (WattHour.TryParse(meterValueJSON.ToString(), out var meterValue))
                                MeterValuesInBetween.Add(meterValue);
                        }

                    }

                    //if (MeterValuesInBetweenJSON.ParseOptionalJSONArray("meterValues",
                    //                                                    "meter values",
                    //                                                    //(String input, out WattHour number) => WattHour.TryParseKWh(input, NumberStyles.Any, CultureInfo.InvariantCulture, out number),
                    //                                                    WattHour.TryParseKWh,
                    //                                                    out MeterValuesInBetween,
                    //                                                    out ErrorResponse))
                    //{
                    //    if (ErrorResponse is not null)
                    //        return false;
                    //}

                }

                #endregion

                #region Parse SignedMeteringValues              [optional]

                if (JSON.ParseOptionalJSON("SignedMeteringValues",
                                           "signed metering values",
                                           SignedMeteringValue.TryParse,
                                           out IEnumerable<SignedMeteringValue> SignedMeteringValues,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CalibrationLawVerificationInfo    [optional]

                if (JSON.ParseOptionalJSON("CalibrationLawVerificationInfo",
                                           "calibration law verification info",
                                           OICPv2_3.CalibrationLawVerification.TryParse,
                                           out CalibrationLawVerification? CalibrationLawVerification,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse HubOperatorId                     [optional]

                if (JSON.ParseOptional("HubOperatorID",
                                       "hub operator identification",
                                       Operator_Id.TryParse,
                                       out Operator_Id? HubOperatorId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse HubProviderId                     [optional]

                if (JSON.ParseOptional("HubProviderID",
                                       "hub provider identification",
                                       Provider_Id.TryParse,
                                       out Provider_Id? HubProviderId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region Parse CustomData                        [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                ChargeDetailRecord = new ChargeDetailRecord(

                                         SessionId,
                                         EVSEId,
                                         Identification,
                                         SessionStart,
                                         SessionEnd,
                                         ChargingStart ?? SessionStart,
                                         ChargingEnd   ?? SessionEnd,
                                         ConsumedEnergy,

                                         PartnerProductId,
                                         CPOPartnerSessionId,
                                         EMPPartnerSessionId,
                                         MeterValueStart,
                                         MeterValueEnd,
                                         MeterValuesInBetween,
                                         SignedMeteringValues,
                                         CalibrationLawVerification,
                                         HubOperatorId,
                                         HubProviderId,

                                         customData

                                     );

                if (CustomChargeDetailRecordParser is not null)
                    ChargeDetailRecord = CustomChargeDetailRecordParser(JSON,
                                                                        ChargeDetailRecord);

                return true;

            }
            catch (Exception e)
            {
                ChargeDetailRecord  = default;
                ErrorResponse       = "The given JSON representation of a charge detail record is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomChargeDetailRecordSerializer = null, CustomIdentificationSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomChargeDetailRecordSerializer">A delegate to serialize custom ChargeDetailRecord JSON elements.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON elements.</param>
        /// <param name="CustomSignedMeteringValueSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomCalibrationLawVerificationSerializer">A delegate to serialize custom calibration law verification JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ChargeDetailRecord>?          CustomChargeDetailRecordSerializer           = null,
                              CustomJObjectSerializerDelegate<Identification>?              CustomIdentificationSerializer               = null,
                              CustomJObjectSerializerDelegate<SignedMeteringValue>?         CustomSignedMeteringValueSerializer          = null,
                              CustomJObjectSerializerDelegate<CalibrationLawVerification>?  CustomCalibrationLawVerificationSerializer   = null)

        {

            var json = JSONObject.Create(

                                 new JProperty("SessionID",                        SessionId.          ToString()),
                                 new JProperty("EvseID",                           EVSEId.             ToString()),
                                 new JProperty("Identification",                   Identification.     ToJSON(CustomIdentificationSerializer: CustomIdentificationSerializer)),
                                 new JProperty("SessionStart",                     SessionStart.       ToISO8601()),
                                 new JProperty("SessionEnd",                       SessionEnd.         ToISO8601()),
                                 new JProperty("ChargingStart",                    ChargingStart.      ToISO8601()),
                                 new JProperty("ChargingEnd",                      ChargingEnd.        ToISO8601()),
                                 new JProperty("ConsumedEnergy",                   Math.Round(ConsumedEnergy.kWh, 3)),

                           PartnerProductId.   HasValue
                               ? new JProperty("PartnerProductID",                 PartnerProductId.   Value.ToString())
                               : null,

                           CPOPartnerSessionId.HasValue
                               ? new JProperty("CPOPartnerSessionID",              CPOPartnerSessionId.Value.ToString())
                               : null,

                           EMPPartnerSessionId.HasValue
                               ? new JProperty("EMPPartnerSessionID",              EMPPartnerSessionId.Value.ToString())
                               : null,

                           MeterValueStart.    HasValue
                               ? new JProperty("MeterValueStart",                  Math.Round(MeterValueStart.Value.kWh, 3))
                               : null,

                           MeterValueEnd.      HasValue
                               ? new JProperty("MeterValueEnd",                    Math.Round(MeterValueEnd.  Value.kWh, 3))
                               : null,

                           MeterValuesInBetween is not null && MeterValuesInBetween.Any()
                               ? new JProperty("MeterValueInBetween",
                                     new JObject(  // OICP is crazy!
                                         new JProperty("meterValues",              new JArray(MeterValuesInBetween.
                                                                                                  SafeSelect(meterValue => Math.Round(meterValue.kWh, 3)))
                                         )
                                     )
                                 )
                               : null,

                           SignedMeteringValues is not null && SignedMeteringValues.Any()
                               ? new JProperty("SignedMeteringValues",             new JArray(SignedMeteringValues.
                                                                                                  Select(signedMeteringValue => signedMeteringValue.ToJSON(CustomSignedMeteringValueSerializer))))
                               : null,

                           CalibrationLawVerificationInfo is not null && CalibrationLawVerificationInfo.IsNotEmpty
                               ? new JProperty("CalibrationLawVerificationInfo",   CalibrationLawVerificationInfo.ToJSON(CustomCalibrationLawVerificationSerializer))
                               : null,

                           HubOperatorId.HasValue
                               ? new JProperty("HubOperatorID",                    HubOperatorId.Value.ToString())
                               : null,

                           HubProviderId.HasValue
                               ? new JProperty("HubProviderID",                    HubProviderId.Value.ToString())
                               : null,

                           CustomData?.HasValues == true
                               ? new JProperty("CustomData",                       CustomData)
                               : null

                    );

            return CustomChargeDetailRecordSerializer is not null
                       ? CustomChargeDetailRecordSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone(NewSessionId = null)

        /// <summary>
        /// Clone this object.
        /// </summary>
        /// <param name="NewSessionId">An optional new charging session identification.</param>
        public ChargeDetailRecord Clone(Session_Id? NewSessionId = null)

            => new (

                   NewSessionId ?? SessionId.      Clone(),
                   EVSEId.                         Clone(),
                   Identification.                 Clone(),
                   SessionStart,
                   SessionEnd,
                   ChargingStart,
                   ChargingEnd,
                   ConsumedEnergy,

                   PartnerProductId?.              Clone(),
                   CPOPartnerSessionId?.           Clone(),
                   EMPPartnerSessionId?.           Clone(),
                   MeterValueStart,
                   MeterValueEnd,
                   MeterValuesInBetween?.          ToArray(),
                   SignedMeteringValues is not null && SignedMeteringValues.Any()
                       ? SignedMeteringValues.Select(signedMeteringValue => signedMeteringValue.Clone())
                       : [],
                   CalibrationLawVerificationInfo?.Clone(),
                   HubOperatorId?.                 Clone(),
                   HubProviderId?.                 Clone(),

                   CustomData is not null
                       ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargeDetailRecord ChargeDetailRecord1,
                                           ChargeDetailRecord ChargeDetailRecord2)
        {

            if (ReferenceEquals(ChargeDetailRecord1, ChargeDetailRecord2))
                return true;

            if (ChargeDetailRecord1 is null || ChargeDetailRecord2 is null)
                return false;

            return ChargeDetailRecord1.Equals(ChargeDetailRecord2);

        }

        #endregion

        #region Operator != (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargeDetailRecord ChargeDetailRecord1,
                                           ChargeDetailRecord ChargeDetailRecord2)

            => !(ChargeDetailRecord1 == ChargeDetailRecord2);

        #endregion

        #region Operator <  (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargeDetailRecord ChargeDetailRecord1,
                                          ChargeDetailRecord ChargeDetailRecord2)
        {

            if (ChargeDetailRecord1 is null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord1), "The given charge detail record must not be null!");

            return ChargeDetailRecord1.CompareTo(ChargeDetailRecord2) < 0;

        }

        #endregion

        #region Operator <= (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargeDetailRecord ChargeDetailRecord1,
                                           ChargeDetailRecord ChargeDetailRecord2)

            => !(ChargeDetailRecord1 > ChargeDetailRecord2);

        #endregion

        #region Operator >  (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargeDetailRecord ChargeDetailRecord1,
                                          ChargeDetailRecord ChargeDetailRecord2)
        {

            if (ChargeDetailRecord1 is null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord1), "The given charge detail record must not be null!");

            return ChargeDetailRecord1.CompareTo(ChargeDetailRecord2) > 0;

        }

        #endregion

        #region Operator >= (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargeDetailRecord ChargeDetailRecord1,
                                           ChargeDetailRecord ChargeDetailRecord2)

            => !(ChargeDetailRecord1 < ChargeDetailRecord2);

        #endregion

        #endregion

        #region IComparable<ChargeDetailRecord> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargeDetailRecord chargeDetailRecord
                   ? CompareTo(chargeDetailRecord)
                   : throw new ArgumentException("The given object is not a charge detail record!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargeDetailRecord)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record object to compare with.</param>
        public Int32 CompareTo(ChargeDetailRecord? ChargeDetailRecord)
        {

            if (ChargeDetailRecord is null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord), "The given charge detail record must not be null!");

            var c = SessionId.     CompareTo(ChargeDetailRecord.SessionId);

            if (c == 0)
                c = EVSEId.        CompareTo(ChargeDetailRecord.EVSEId);

            if (c == 0)
                c = Identification.CompareTo(ChargeDetailRecord.Identification);

            if (c == 0)
                c = SessionStart.  CompareTo(ChargeDetailRecord.SessionStart);

            if (c == 0)
                c = SessionEnd.    CompareTo(ChargeDetailRecord.SessionEnd);

            if (c == 0)
                c = ChargingStart. CompareTo(ChargeDetailRecord.ChargingStart);

            if (c == 0)
                c = ChargingEnd.   CompareTo(ChargeDetailRecord.ChargingEnd);

            if (c == 0)
                c = ConsumedEnergy.CompareTo(ChargeDetailRecord.ConsumedEnergy);


            if (c == 0 && PartnerProductId.   HasValue && ChargeDetailRecord.PartnerProductId.   HasValue)
                c = PartnerProductId.   Value.CompareTo(ChargeDetailRecord.PartnerProductId.   Value);

            if (c == 0 && CPOPartnerSessionId.HasValue && ChargeDetailRecord.CPOPartnerSessionId.HasValue)
                c = CPOPartnerSessionId.Value.CompareTo(ChargeDetailRecord.CPOPartnerSessionId.Value);

            if (c == 0 && EMPPartnerSessionId.HasValue && ChargeDetailRecord.EMPPartnerSessionId.HasValue)
                c = EMPPartnerSessionId.Value.CompareTo(ChargeDetailRecord.EMPPartnerSessionId.Value);

            if (c == 0 && MeterValueStart.    HasValue && ChargeDetailRecord.MeterValueStart.    HasValue)
                c = MeterValueStart.    Value.CompareTo(ChargeDetailRecord.MeterValueStart.    Value);

            if (c == 0 && MeterValueEnd.      HasValue && ChargeDetailRecord.MeterValueEnd.      HasValue)
                c = MeterValueEnd.      Value.CompareTo(ChargeDetailRecord.MeterValueEnd.      Value);

            // MeterValuesInBetween

            // SignedMeteringValues

            if (c == 0 && CalibrationLawVerificationInfo is not null && ChargeDetailRecord.CalibrationLawVerificationInfo is not null)
                c = CalibrationLawVerificationInfo.CompareTo(ChargeDetailRecord.CalibrationLawVerificationInfo);

            if (c == 0 && HubOperatorId.HasValue && ChargeDetailRecord.HubOperatorId.HasValue)
                c = HubOperatorId.Value.CompareTo(ChargeDetailRecord.HubOperatorId.Value);

            if (c == 0 && HubProviderId.HasValue && ChargeDetailRecord.HubProviderId.HasValue)
                c = HubProviderId.Value.CompareTo(ChargeDetailRecord.HubProviderId.Value);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargeDetailRecord> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargeDetailRecord chargeDetailRecord &&
                   Equals(chargeDetailRecord);

        #endregion

        #region Equals(ChargeDetailRecord)

        /// <summary>
        /// Compares two charge detail records for equality.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargeDetailRecord? ChargeDetailRecord)

            => ChargeDetailRecord is not null &&

               SessionId.     Equals(ChargeDetailRecord.SessionId)      &&
               EVSEId.        Equals(ChargeDetailRecord.EVSEId)         &&
               Identification.Equals(ChargeDetailRecord.Identification) &&
               SessionStart.  Equals(ChargeDetailRecord.SessionStart)   &&
               SessionEnd.    Equals(ChargeDetailRecord.SessionEnd)     &&
               ChargingStart. Equals(ChargeDetailRecord.ChargingStart)  &&
               ChargingEnd.   Equals(ChargeDetailRecord.ChargingEnd)    &&
               ConsumedEnergy.Equals(ChargeDetailRecord.ConsumedEnergy) &&

             ((!PartnerProductId.   HasValue && !ChargeDetailRecord.PartnerProductId.   HasValue) ||
               (PartnerProductId.   HasValue &&  ChargeDetailRecord.PartnerProductId.   HasValue && PartnerProductId.   Value.Equals(ChargeDetailRecord.PartnerProductId.   Value))) &&

             ((!CPOPartnerSessionId.HasValue && !ChargeDetailRecord.CPOPartnerSessionId.HasValue) ||
               (CPOPartnerSessionId.HasValue &&  ChargeDetailRecord.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(ChargeDetailRecord.CPOPartnerSessionId.Value))) &&

             ((!EMPPartnerSessionId.HasValue && !ChargeDetailRecord.EMPPartnerSessionId.HasValue) ||
               (EMPPartnerSessionId.HasValue &&  ChargeDetailRecord.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(ChargeDetailRecord.EMPPartnerSessionId.Value))) &&

             ((!MeterValueStart.    HasValue && !ChargeDetailRecord.MeterValueStart.    HasValue) ||
               (MeterValueStart.    HasValue &&  ChargeDetailRecord.MeterValueStart.    HasValue && MeterValueStart.    Value.Equals(ChargeDetailRecord.MeterValueStart.    Value))) &&

             ((!MeterValueEnd.      HasValue && !ChargeDetailRecord.MeterValueEnd.      HasValue) ||
               (MeterValueEnd.      HasValue &&  ChargeDetailRecord.MeterValueEnd.      HasValue && MeterValueEnd.      Value.Equals(ChargeDetailRecord.MeterValueEnd.      Value))) &&

                MeterValuesInBetween.Count().Equals(ChargeDetailRecord.MeterValuesInBetween.Count()) &&
                MeterValuesInBetween.All(meterValue          => ChargeDetailRecord.MeterValuesInBetween.Contains(meterValue))          &&

                SignedMeteringValues.Count().Equals(ChargeDetailRecord.SignedMeteringValues.Count()) &&
                SignedMeteringValues.All(signedMeteringValue => ChargeDetailRecord.SignedMeteringValues.Contains(signedMeteringValue)) &&

              ((CalibrationLawVerificationInfo is     null && ChargeDetailRecord.CalibrationLawVerificationInfo is     null) ||
               (CalibrationLawVerificationInfo is not null && ChargeDetailRecord.CalibrationLawVerificationInfo is not null &&
                CalibrationLawVerificationInfo.CompareTo(ChargeDetailRecord.CalibrationLawVerificationInfo) != 0)) &&

             ((!HubOperatorId.      HasValue && !ChargeDetailRecord.HubOperatorId.      HasValue) ||
               (HubOperatorId.      HasValue &&  ChargeDetailRecord.HubOperatorId.      HasValue && HubOperatorId.      Value.Equals(ChargeDetailRecord.HubOperatorId.      Value))) &&

             ((!HubProviderId.      HasValue && !ChargeDetailRecord.HubProviderId.      HasValue) ||
               (HubProviderId.      HasValue &&  ChargeDetailRecord.HubProviderId.      HasValue && HubProviderId.      Value.Equals(ChargeDetailRecord.HubProviderId.      Value)));

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

            => String.Concat(

                   $"{SessionId}: {ConsumedEnergy} kWh",

                   PartnerProductId.HasValue
                       ? $" of {PartnerProductId.Value}"
                       : null,

                   $" at {EVSEId}  for {Identification}, {SessionStart.ToISO8601()} -> {SessionEnd.ToISO8601()}"

               );

        #endregion


        #region ToBuilder(NewSessionId = null)

        /// <summary>
        /// Return a builder for this charge detail record.
        /// </summary>
        /// <param name="NewSessionId">An optional new charging session identification.</param>
        public Builder ToBuilder(Session_Id? NewSessionId = null)

            => new (NewSessionId ?? SessionId,
                    EVSEId,
                    Identification,
                    SessionStart,
                    SessionEnd,
                    ChargingStart,
                    ChargingEnd,
                    ConsumedEnergy,

                    PartnerProductId,
                    CPOPartnerSessionId,
                    EMPPartnerSessionId,
                    MeterValueStart,
                    MeterValueEnd,
                    MeterValuesInBetween,
                    SignedMeteringValues,
                    CalibrationLawVerificationInfo,
                    HubOperatorId,
                    HubProviderId,

                    CustomData,
                    InternalData);

        #endregion

        #region (class) Builder

        /// <summary>
        /// A charge detail record builder.
        /// </summary>
        public new class Builder : AInternalData.Builder
        {

            #region Properties

            /// <summary>
            /// The Hubject session identification, that identifies the charging process.
            /// </summary>
            [Mandatory]
            public Session_Id?                       SessionId                          { get; set; }

            /// <summary>
            /// The EVSE identification, that identifies the location of the charging process.
            /// </summary>
            [Mandatory]
            public EVSE_Id?                          EVSEId                             { get; set; }

            /// <summary>
            /// The authentication data used to authorize the user or car.
            /// </summary>
            [Optional]
            public Identification?                   Identification                     { get; set; }

            /// <summary>
            /// The timestamp when the charging session started.
            /// </summary>
            [Mandatory]
            public DateTime?                         SessionStart                       { get; set; }

            /// <summary>
            /// The timestamp when the charging session ended.
            /// </summary>
            [Mandatory]
            public DateTime?                         SessionEnd                         { get; set; }

            /// <summary>
            /// The timestamp when the charging process started.
            /// </summary>
            [Mandatory]
            public DateTime?                         ChargingStart                      { get; set; }

            /// <summary>
            /// The timestamp when the charging process stopped.
            /// </summary>
            [Mandatory]
            public DateTime?                         ChargingEnd                        { get; set; }

            /// <summary>
            /// The amount of consumed energy [kWh].
            /// </summary>
            [Mandatory]
            public WattHour?                         ConsumedEnergy                     { get; set; }

            /// <summary>
            /// An optional pricing product name (for identifying a tariff) that must be unique.
            /// </summary>
            [Optional]
            public PartnerProduct_Id?                PartnerProductId                   { get; set; }

            /// <summary>
            /// An optional session identification assigned by the CPO partner.
            /// </summary>
            [Optional]
            public CPOPartnerSession_Id?             CPOPartnerSessionId                { get; set; }

            /// <summary>
            /// An optional session identification assigned by the EMP partner.
            /// </summary>
            [Optional]
            public EMPPartnerSession_Id?             EMPPartnerSessionId                { get; set; }


            /// <summary>
            /// The optional starting value of the energy meter [kWh].
            /// </summary>
            [Optional]
            public WattHour?                         MeterValueStart                    { get; set; }

            /// <summary>
            /// The optional final value of the energy meter [kWh].
            /// </summary>
            [Optional]
            public WattHour?                         MeterValueEnd                      { get; set; }

            /// <summary>
            /// An optional enumeration of meter values during the charging session.
            /// </summary>
            [Optional]
            public List<WattHour>                    MeterValuesInBetween               { get; }

            /// <summary>
            /// Optional signed metering values, with can e.g. verified via a transparency software.
            /// </summary>
            [Optional]
            public List<SignedMeteringValue>?        SignedMeteringValues               { get; }

            /// <summary>
            /// Optional additional information which could directly or indirectly help to verify the
            /// signed metering values by using a valid transparency software.
            /// </summary>
            [Optional]
            public CalibrationLawVerification?       CalibrationLawVerificationInfo     { get; set; }

            /// <summary>
            /// An optional operator identification of the hub operator.
            /// </summary>
            [Optional]
            public Operator_Id?                      HubOperatorId                      { get; set; }

            /// <summary>
            /// An optional provider identification of the hub provider.
            /// </summary>
            [Optional]
            public Provider_Id?                      HubProviderId                      { get; set; }

            #endregion

            #region Constructor(s)

#pragma warning disable IDE0290 // Use primary constructor

            /// <summary>
            /// Create a new charge detail record builder.
            /// </summary>
            /// <param name="SessionId">The Hubject session identification, that identifies the charging process.</param>
            /// <param name="EVSEId">The EVSE identification, that identifies the location of the charging process.</param>
            /// <param name="Identification">The authentication data used to authorize the user or the car.</param>
            /// <param name="SessionStart">The timestamp when the charging session started.</param>
            /// <param name="SessionEnd">The timestamp when the charging session ended.</param>
            /// <param name="ChargingStart">The timestamp when the charging process started.</param>
            /// <param name="ChargingEnd">The timestamp when the charging process stopped.</param>
            /// <param name="ConsumedEnergy">The amount of consumed energy [kWh].</param>
            /// 
            /// <param name="PartnerProductId">An optional pricing product name (for identifying a tariff) that must be unique.</param>
            /// <param name="CPOPartnerSessionId">An optional session identification assigned by the CPO partner.</param>
            /// <param name="EMPPartnerSessionId">An optional session identification assigned by the EMP partner.</param>
            /// <param name="MeterValueStart">An optional starting value of the energy meter [kWh].</param>
            /// <param name="MeterValueEnd">An optional final value of the energy meter [kWh].</param>
            /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session [kWh].</param>
            /// <param name="SignedMeteringValues">Optional signed metering values, with can e.g. verified via a transparency software.</param>
            /// <param name="CalibrationLawVerificationInfo">Optional additional information which could directly or indirectly help to verify the signed metering values by using a valid transparency software.</param>
            /// <param name="HubOperatorId">An optional operator identification of the hub operator.</param>
            /// <param name="HubProviderId">An optional provider identification of the hub provider.</param>
            /// 
            /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
            /// <param name="InternalData">Optional internal customer specific data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(Session_Id?                        SessionId                        = null,
                           EVSE_Id?                           EVSEId                           = null,
                           Identification?                    Identification                   = null,
                           DateTime?                          SessionStart                     = null,
                           DateTime?                          SessionEnd                       = null,
                           DateTime?                          ChargingStart                    = null,
                           DateTime?                          ChargingEnd                      = null,
                           WattHour?                          ConsumedEnergy                   = null,

                           PartnerProduct_Id?                 PartnerProductId                 = null,
                           CPOPartnerSession_Id?              CPOPartnerSessionId              = null,
                           EMPPartnerSession_Id?              EMPPartnerSessionId              = null,
                           WattHour?                          MeterValueStart                  = null,
                           WattHour?                          MeterValueEnd                    = null,
                           IEnumerable<WattHour>?             MeterValuesInBetween             = null,
                           IEnumerable<SignedMeteringValue>?  SignedMeteringValues             = null,
                           CalibrationLawVerification?        CalibrationLawVerificationInfo   = null,
                           Operator_Id?                       HubOperatorId                    = null,
                           Provider_Id?                       HubProviderId                    = null,

                           JObject?                           CustomData                       = null,
                           UserDefinedDictionary?             InternalData                     = null)

                : base(CustomData,
                       InternalData)

            {

                this.EVSEId                          = EVSEId;
                this.SessionId                       = SessionId;
                this.Identification                  = Identification;
                this.SessionStart                    = SessionStart;
                this.SessionEnd                      = SessionEnd;
                this.ChargingStart                   = ChargingStart;
                this.ChargingEnd                     = ChargingEnd;
                this.ConsumedEnergy                  = ConsumedEnergy;

                this.PartnerProductId                = PartnerProductId;
                this.CPOPartnerSessionId             = CPOPartnerSessionId;
                this.EMPPartnerSessionId             = EMPPartnerSessionId;
                this.MeterValueStart                 = MeterValueStart;
                this.MeterValueEnd                   = MeterValueEnd;
                this.MeterValuesInBetween            = MeterValuesInBetween is not null && MeterValuesInBetween.Any()
                                                           ? new List<WattHour>           (MeterValuesInBetween)
                                                           : [];
                this.SignedMeteringValues            = SignedMeteringValues is not null && SignedMeteringValues.Any()
                                                           ? new List<SignedMeteringValue>(SignedMeteringValues)
                                                           : [];
                this.CalibrationLawVerificationInfo  = CalibrationLawVerificationInfo;
                this.HubOperatorId                   = HubOperatorId;
                this.HubProviderId                   = HubProviderId;

            }

#pragma warning restore IDE0290 // Use primary constructor

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the charge detail record.
            /// </summary>
            /// <param name="Builder">A ChargeDetailRecord builder.</param>
            public static implicit operator ChargeDetailRecord(Builder Builder)

                => Builder.ToImmutable();


            /// <summary>
            /// Return an immutable version of the charge detail record.
            /// </summary>
            public ChargeDetailRecord ToImmutable()
            {

                #region Check mandatory parameters

                if (!SessionId.     HasValue)
                    throw new ArgumentException("The given session identification must not be null!",       nameof(SessionId));

                if (!EVSEId.        HasValue)
                    throw new ArgumentException("The given EVSE identification must not be null!",          nameof(EVSEId));

                if (Identification is null || Identification.IsNullOrEmpty)
                    throw new ArgumentException("The given user/contract identification must not be null!", nameof(Identification));

                if (!SessionStart.  HasValue)
                    throw new ArgumentException("The given session start timestamp must not be null!",      nameof(SessionStart));

                if (!SessionEnd.    HasValue)
                    throw new ArgumentException("The given session end timestamp must not be null!",        nameof(SessionEnd));

                if (!ChargingStart. HasValue)
                    throw new ArgumentException("The given charging start timestamp must not be null!",     nameof(ChargingStart));

                if (!ChargingEnd.   HasValue)
                    throw new ArgumentException("The given charging end timestamp must not be null!",       nameof(ChargingEnd));

                if (!ConsumedEnergy.HasValue)
                    throw new ArgumentException("The given consumed energy must not be null!",              nameof(ConsumedEnergy));

                #endregion

                return new ChargeDetailRecord(
                           SessionId.     Value,
                           EVSEId.        Value,
                           Identification,
                           SessionStart.  Value,
                           SessionEnd.    Value,
                           ChargingStart. Value,
                           ChargingEnd.   Value,
                           ConsumedEnergy.Value,

                           PartnerProductId,
                           CPOPartnerSessionId,
                           EMPPartnerSessionId,
                           MeterValueStart,
                           MeterValueEnd,
                           MeterValuesInBetween,
                           SignedMeteringValues,
                           CalibrationLawVerificationInfo,
                           HubOperatorId,
                           HubProviderId,

                           CustomData,
                           InternalData
                       );

            }

            #endregion

        }

        #endregion

    }

}
