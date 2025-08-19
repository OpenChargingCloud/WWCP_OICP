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
    /// The ChargeDetailRecord request.
    /// </summary>
    public class ChargeDetailRecordRequest : ARequest<ChargeDetailRecordRequest>
    {

        #region Properties

        /// <summary>
        /// The charge detail record to send.
        /// </summary>
        [Mandatory]
        public ChargeDetailRecord  ChargeDetailRecord    { get; }

        /// <summary>
        /// The unique identification of the operator sending the given charge detail record
        /// This means: Not the sub operator or the operator of the EVSE!
        /// </summary>
        [Mandatory]
        public Operator_Id         OperatorId            { get; }

        #endregion

        #region Constructor(s)

#pragma warning disable IDE0290 // Use primary constructor

        /// <summary>
        /// Create a new ChargeDetailRecord request.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record to send.</param>
        /// <param name="OperatorId">The unique identification of the operator sending the given charge detail record (not the suboperator or the operator of the EVSE).</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public ChargeDetailRecordRequest(ChargeDetailRecord  ChargeDetailRecord,
                                         Operator_Id         OperatorId,
                                         Process_Id?         ProcessId           = null,

                                         DateTimeOffset?     Timestamp           = null,
                                         EventTracking_Id?   EventTrackingId     = null,
                                         TimeSpan?           RequestTimeout      = null,
                                         CancellationToken   CancellationToken   = default)

            : base(ProcessId,
                   ChargeDetailRecord.CustomData,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.ChargeDetailRecord  = ChargeDetailRecord ?? throw new ArgumentNullException(nameof(ChargeDetailRecord), "The given charge detail record must not be null!");
            this.OperatorId          = OperatorId;

        }

#pragma warning restore IDE0290 // Use primary constructor

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#eRoamingChargeDetailRecord

        // {
        //   "CPOPartnerSessionID": "string",
        //   "CalibrationLawVerificationInfo": {
        //     "CalibrationLawCertificateID": "string",
        //     "PublicKey": "string",
        //     "MeteringSignatureUrl": "string",
        //     "MeteringSignatureEncodingFormat": "string",
        //     "SignedMeteringValuesVerificationInstruction": "string"
        //   },
        //   "ChargingEnd": "2021-01-11T16:16:08.800Z",
        //   "ChargingStart": "2021-01-11T16:16:08.800Z",
        //   "ConsumedEnergy": 0,
        //   "EMPPartnerSessionID": "string",
        //   "EvseID": "string",
        //   "HubOperatorID": "string",
        //   "HubProviderID": "string",
        //   "Identification": {
        //     "RFIDMifareFamilyIdentification": {
        //       "UID": "string"
        //     },
        //     "QRCodeIdentification": {
        //       "EvcoID": "string",
        //       "HashedPIN": {
        //         "Function": "Bcrypt",
        //         "LegacyHashData": {
        //           "Function": "MD5",
        //           "Salt": "string",
        //           "Value": "string"
        //         },
        //         "Value": "string"
        //       },
        //       "PIN": "string"
        //     },
        //     "PlugAndChargeIdentification": {
        //       "EvcoID": "string"
        //     },
        //     "RemoteIdentification": {
        //       "EvcoID": "string"
        //     },
        //     "RFIDIdentification": {
        //       "EvcoID": "string",
        //       "ExpiryDate": "2021-01-11T16:16:08.800Z",
        //       "PrintedNumber": "string",
        //       "RFID": "mifareCls",
        //       "UID": "string"
        //     }
        //   },
        //   "MeterValueEnd": 0,
        //   "MeterValueInBetween": {
        //     "meterValues": [
        //       0
        //     ]
        //   },
        //   "MeterValueStart": 0,
        //   "PartnerProductID": "string",
        //   "SessionEnd": "2021-01-11T16:16:08.800Z",
        //   "SessionID": "string",
        //   "SessionStart": "2021-01-11T16:16:08.800Z",
        //   "SignedMeteringValues": [
        //     {
        //       "SignedMeteringValue": "string",
        //       "MeteringStatus": "Start"
        //     }
        //   ]
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomChargeDetailRecordRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a ChargeDetailRecord request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OperatorIdURL">The unique identification of the operator sending the given charge detail record (not the suboperator or the operator of the EVSE).</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargeDetailRecordRequestParser">A delegate to parse custom ChargeDetailRecord JSON objects.</param>
        public static ChargeDetailRecordRequest Parse(JObject                                                  JSON,
                                                      Operator_Id                                              OperatorIdURL,
                                                      Process_Id?                                              ProcessId                               = null,

                                                      DateTimeOffset?                                          Timestamp                               = null,
                                                      EventTracking_Id?                                        EventTrackingId                         = null,
                                                      TimeSpan?                                                RequestTimeout                          = null,
                                                      CustomJObjectParserDelegate<ChargeDetailRecordRequest>?  CustomChargeDetailRecordRequestParser   = null,
                                                      CancellationToken                                        CancellationToken                       = default)
        {

            if (TryParse(JSON,
                         OperatorIdURL,
                         out var authorizeRemoteStopRequest,
                         out var errorResponse,
                         ProcessId,
                         Timestamp,
                         EventTrackingId,
                         RequestTimeout,
                         CustomChargeDetailRecordRequestParser,
                         CancellationToken))
            {
                return authorizeRemoteStopRequest;
            }

            throw new ArgumentException("The given JSON representation of a ChargeDetailRecord request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, OperatorIdURL, out ChargeDetailRecordRequest, out ErrorResponse, ..., CustomChargeDetailRecordRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a ChargeDetailRecord request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OperatorIdURL">The unique identification of the operator sending the given charge detail record (not the suboperator or the operator of the EVSE).</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="ChargeDetailRecordRequest">The parsed ChargeDetailRecord request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargeDetailRecordRequestParser">A delegate to parse custom ChargeDetailRecord request JSON objects.</param>
        public static Boolean TryParse(JObject                                                  JSON,
                                       Operator_Id                                              OperatorIdURL,
                                       [NotNullWhen(true)]  out ChargeDetailRecordRequest?      ChargeDetailRecordRequest,
                                       [NotNullWhen(false)] out String?                         ErrorResponse,
                                       Process_Id?                                              ProcessId                               = null,

                                       DateTimeOffset?                                          Timestamp                               = null,
                                       EventTracking_Id?                                        EventTrackingId                         = null,
                                       TimeSpan?                                                RequestTimeout                          = null,
                                       CustomJObjectParserDelegate<ChargeDetailRecordRequest>?  CustomChargeDetailRecordRequestParser   = null,
                                       CancellationToken                                        CancellationToken                       = default)
        {

            try
            {

                ChargeDetailRecordRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse ChargeDetailRecord     [mandatory]

                if (!ChargeDetailRecord.TryParse(JSON,
                                                 out var chargeDetailRecord,
                                                 out ErrorResponse))
                {
                    return false;
                }

                #endregion


                ChargeDetailRecordRequest = new ChargeDetailRecordRequest(
                                                chargeDetailRecord,
                                                OperatorIdURL,
                                                ProcessId,

                                                Timestamp,
                                                EventTrackingId,
                                                RequestTimeout,
                                                CancellationToken
                                            );

                if (CustomChargeDetailRecordRequestParser is not null)
                    ChargeDetailRecordRequest = CustomChargeDetailRecordRequestParser(JSON,
                                                                                      ChargeDetailRecordRequest);

                return true;

            }
            catch (Exception e)
            {
                ChargeDetailRecordRequest  = default;
                ErrorResponse              = "The given JSON representation of a ChargeDetailRecord request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomChargeDetailRecordRequestSerializer = null, CustomChargeDetailRecordSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomChargeDetailRecordRequestSerializer">A delegate to customize the serialization of ChargeDetailRecordRequest responses.</param>
        /// <param name="CustomChargeDetailRecordSerializer">A delegate to serialize custom ChargeDetailRecord JSON elements.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON elements.</param>
        /// <param name="CustomSignedMeteringValueSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomCalibrationLawVerificationSerializer">A delegate to serialize custom calibration law verification JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ChargeDetailRecordRequest>?   CustomChargeDetailRecordRequestSerializer    = null,
                              CustomJObjectSerializerDelegate<ChargeDetailRecord>?          CustomChargeDetailRecordSerializer           = null,
                              CustomJObjectSerializerDelegate<Identification>?              CustomIdentificationSerializer               = null,
                              CustomJObjectSerializerDelegate<SignedMeteringValue>?         CustomSignedMeteringValueSerializer          = null,
                              CustomJObjectSerializerDelegate<CalibrationLawVerification>?  CustomCalibrationLawVerificationSerializer   = null)
        {

            var json = ChargeDetailRecord.ToJSON(CustomChargeDetailRecordSerializer,
                                                 CustomIdentificationSerializer,
                                                 CustomSignedMeteringValueSerializer,
                                                 CustomCalibrationLawVerificationSerializer);

            return CustomChargeDetailRecordRequestSerializer is not null
                       ? CustomChargeDetailRecordRequestSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two send charge detail record requests for equality.
        /// </summary>
        /// <param name="ChargeDetailRecord1">An send charge detail record request.</param>
        /// <param name="ChargeDetailRecord2">Another send charge detail record request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargeDetailRecordRequest ChargeDetailRecord1,
                                           ChargeDetailRecordRequest ChargeDetailRecord2)
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
        /// Compares two send charge detail record requests for inequality.
        /// </summary>
        /// <param name="ChargeDetailRecord1">An send charge detail record request.</param>
        /// <param name="ChargeDetailRecord2">Another send charge detail record request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargeDetailRecordRequest ChargeDetailRecord1,
                                           ChargeDetailRecordRequest ChargeDetailRecord2)

            => !(ChargeDetailRecord1 == ChargeDetailRecord2);

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

            => Object is ChargeDetailRecordRequest chargeDetailRecordRequest &&
                   Equals(chargeDetailRecordRequest);

        #endregion

        #region Equals(ChargeDetailRecord)

        /// <summary>
        /// Compares two send charge detail record requests for equality.
        /// </summary>
        /// <param name="ChargeDetailRecord">An send charge detail record request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(ChargeDetailRecordRequest? ChargeDetailRecord)

            => ChargeDetailRecord is not null &&

               ChargeDetailRecord.Equals(ChargeDetailRecord.ChargeDetailRecord);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => ChargeDetailRecord.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => ChargeDetailRecord.ToString();

        #endregion

    }

}
