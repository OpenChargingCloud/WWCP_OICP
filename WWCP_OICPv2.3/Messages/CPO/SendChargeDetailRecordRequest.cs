﻿/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
using System.Threading;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The SendChargeDetailRecordStart request.
    /// </summary>
    public class SendChargeDetailRecordRequest : ARequest<SendChargeDetailRecordRequest>
    {

        #region Properties

        /// <summary>
        /// The charge detail record to send.
        /// </summary>
        [Mandatory]
        public ChargeDetailRecord  ChargeDetailRecord    { get; }

        /// <summary>
        /// The unqiue identification of the operator sending the given charge detail record
        /// This means: Not the sub operator or the operator of the EVSE!
        /// </summary>
        [Mandatory]
        public Operator_Id         OperatorId            { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new SendChargeDetailRecordStart request.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record to send.</param>
        /// <param name="OperatorId">The unqiue identification of the operator sending the given charge detail record (not the suboperator or the operator of the EVSE).</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers. This means: Not the sub operator or the operator of the EVSE!</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public SendChargeDetailRecordRequest(ChargeDetailRecord  ChargeDetailRecord,
                                             Operator_Id         OperatorId,
                                             JObject             CustomData          = null,

                                             DateTime?           Timestamp           = null,
                                             CancellationToken?  CancellationToken   = null,
                                             EventTracking_Id    EventTrackingId     = null,
                                             TimeSpan?           RequestTimeout      = null)

            : base(CustomData,
                   Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.ChargeDetailRecord  = ChargeDetailRecord ?? throw new ArgumentNullException(nameof(ChargeDetailRecord), "The given charge detail record must not be null!");
            this.OperatorId          = OperatorId;

        }

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

        #region (static) Parse   (JSON, CustomSendChargeDetailRecordRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a SendChargeDetailRecordStart request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OperatorId">The unqiue identification of the operator sending the given charge detail record (not the suboperator or the operator of the EVSE).</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomSendChargeDetailRecordRequestParser">A delegate to parse custom SendChargeDetailRecordStart JSON objects.</param>
        public static SendChargeDetailRecordRequest Parse(JObject                                                     JSON,
                                                          Operator_Id                                                 OperatorId,
                                                          TimeSpan                                                    RequestTimeout,
                                                          DateTime?                                                   Timestamp                                   = null,
                                                          EventTracking_Id                                            EventTrackingId                             = null,
                                                          CustomJObjectParserDelegate<SendChargeDetailRecordRequest>  CustomSendChargeDetailRecordRequestParser   = null)
        {

            if (TryParse(JSON,
                         OperatorId,
                         RequestTimeout,
                         out SendChargeDetailRecordRequest  authorizeRemoteStopRequest,
                         out String                         ErrorResponse,
                         Timestamp,
                         EventTrackingId,
                         CustomSendChargeDetailRecordRequestParser))
            {
                return authorizeRemoteStopRequest;
            }

            throw new ArgumentException("The given JSON representation of a SendChargeDetailRecordStart request is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomSendChargeDetailRecordRequestParser = null)

        /// <summary>
        /// Parse the given text representation of a SendChargeDetailRecordStart request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="OperatorId">The unqiue identification of the operator sending the given charge detail record (not the suboperator or the operator of the EVSE).</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomSendChargeDetailRecordRequestParser">A delegate to parse custom SendChargeDetailRecordStart request JSON objects.</param>
        public static SendChargeDetailRecordRequest Parse(String                                                      Text,
                                                          Operator_Id                                                 OperatorId,
                                                          TimeSpan                                                    RequestTimeout,
                                                          DateTime?                                                   Timestamp                                   = null,
                                                          EventTracking_Id                                            EventTrackingId                             = null,
                                                          CustomJObjectParserDelegate<SendChargeDetailRecordRequest>  CustomSendChargeDetailRecordRequestParser   = null)
        {

            if (TryParse(Text,
                         OperatorId,
                         RequestTimeout,
                         out SendChargeDetailRecordRequest  authorizeRemoteStopRequest,
                         out String                         ErrorResponse,
                         Timestamp,
                         EventTrackingId,
                         CustomSendChargeDetailRecordRequestParser))
            {
                return authorizeRemoteStopRequest;
            }

            throw new ArgumentException("The given text representation of a SendChargeDetailRecordStart request is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out SendChargeDetailRecordRequest, out ErrorResponse, CustomSendChargeDetailRecordRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a SendChargeDetailRecordStart request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OperatorId">The unqiue identification of the operator sending the given charge detail record (not the suboperator or the operator of the EVSE).</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="SendChargeDetailRecordRequest">The parsed SendChargeDetailRecordStart request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomSendChargeDetailRecordRequestParser">A delegate to parse custom SendChargeDetailRecordStart request JSON objects.</param>
        public static Boolean TryParse(JObject                                                     JSON,
                                       Operator_Id                                                 OperatorId,
                                       TimeSpan                                                    RequestTimeout,
                                       out SendChargeDetailRecordRequest                           SendChargeDetailRecordRequest,
                                       out String                                                  ErrorResponse,
                                       DateTime?                                                   Timestamp                                   = null,
                                       EventTracking_Id                                            EventTrackingId                             = null,
                                       CustomJObjectParserDelegate<SendChargeDetailRecordRequest>  CustomSendChargeDetailRecordRequestParser   = null)
        {

            try
            {

                SendChargeDetailRecordRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse embdedded CDR     [mandatory]

                if (!ChargeDetailRecord.TryParse(JSON,
                                                 out ChargeDetailRecord CDR,
                                                 out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse CustomData        [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                SendChargeDetailRecordRequest = new SendChargeDetailRecordRequest(CDR,
                                                                                  OperatorId,
                                                                                  CustomData,

                                                                                  Timestamp,
                                                                                  null,
                                                                                  EventTrackingId,
                                                                                  RequestTimeout);

                if (CustomSendChargeDetailRecordRequestParser != null)
                    SendChargeDetailRecordRequest = CustomSendChargeDetailRecordRequestParser(JSON,
                                                                                              SendChargeDetailRecordRequest);

                return true;

            }
            catch (Exception e)
            {
                SendChargeDetailRecordRequest  = default;
                ErrorResponse                  = "The given JSON representation of a SendChargeDetailRecordStart request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out SendChargeDetailRecordRequest, out ErrorResponse, CustomSendChargeDetailRecordRequestParser = null)

        /// <summary>
        /// Try to parse the given text representation of a SendChargeDetailRecordStart request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="OperatorId">The unqiue identification of the operator sending the given charge detail record (not the suboperator or the operator of the EVSE).</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="SendChargeDetailRecordRequest">The parsed SendChargeDetailRecordStart request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomSendChargeDetailRecordRequestParser">A delegate to parse custom SendChargeDetailRecordStart request JSON objects.</param>
        public static Boolean TryParse(String                                                      Text,
                                       Operator_Id                                                 OperatorId,
                                       TimeSpan                                                    RequestTimeout,
                                       out SendChargeDetailRecordRequest                           SendChargeDetailRecordRequest,
                                       out String                                                  ErrorResponse,
                                       DateTime?                                                   Timestamp                                   = null,
                                       EventTracking_Id                                            EventTrackingId                             = null,
                                       CustomJObjectParserDelegate<SendChargeDetailRecordRequest>  CustomSendChargeDetailRecordRequestParser   = null)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                OperatorId,
                                RequestTimeout,
                                out SendChargeDetailRecordRequest,
                                out ErrorResponse,
                                Timestamp,
                                EventTrackingId,
                                CustomSendChargeDetailRecordRequestParser);

            }
            catch (Exception e)
            {
                SendChargeDetailRecordRequest  = default;
                ErrorResponse                  = "The given text representation of a SendChargeDetailRecordStart request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomSendChargeDetailRecordRequestSerializer = null, CustomChargeDetailRecordSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomSendChargeDetailRecordRequestSerializer">A delegate to customize the serialization of SendChargeDetailRecordRequest responses.</param>
        /// <param name="CustomChargeDetailRecordSerializer">A delegate to serialize custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON elements.</param>
        /// <param name="CustomSignedMeteringValueSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomCalibrationLawVerificationSerializer">A delegate to serialize custom calibration law verification JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<SendChargeDetailRecordRequest>  CustomSendChargeDetailRecordRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<ChargeDetailRecord>             CustomChargeDetailRecordSerializer              = null,
                              CustomJObjectSerializerDelegate<Identification>                 CustomIdentificationSerializer                  = null,
                              CustomJObjectSerializerDelegate<SignedMeteringValue>            CustomSignedMeteringValueSerializer             = null,
                              CustomJObjectSerializerDelegate<CalibrationLawVerification>     CustomCalibrationLawVerificationSerializer      = null)
        {

            var JSON = ChargeDetailRecord.ToJSON(CustomChargeDetailRecordSerializer,
                                                 CustomIdentificationSerializer,
                                                 CustomSignedMeteringValueSerializer,
                                                 CustomCalibrationLawVerificationSerializer);

            return CustomSendChargeDetailRecordRequestSerializer != null
                       ? CustomSendChargeDetailRecordRequestSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (SendChargeDetailRecordStart1, SendChargeDetailRecordStart2)

        /// <summary>
        /// Compares two send charge detail record requests for equality.
        /// </summary>
        /// <param name="SendChargeDetailRecordStart1">An send charge detail record request.</param>
        /// <param name="SendChargeDetailRecordStart2">Another send charge detail record request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SendChargeDetailRecordRequest SendChargeDetailRecordStart1,
                                           SendChargeDetailRecordRequest SendChargeDetailRecordStart2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SendChargeDetailRecordStart1, SendChargeDetailRecordStart2))
                return true;

            // If one is null, but not both, return false.
            if (SendChargeDetailRecordStart1 is null || SendChargeDetailRecordStart2 is null)
                return false;

            return SendChargeDetailRecordStart1.Equals(SendChargeDetailRecordStart2);

        }

        #endregion

        #region Operator != (SendChargeDetailRecordStart1, SendChargeDetailRecordStart2)

        /// <summary>
        /// Compares two send charge detail record requests for inequality.
        /// </summary>
        /// <param name="SendChargeDetailRecordStart1">An send charge detail record request.</param>
        /// <param name="SendChargeDetailRecordStart2">Another send charge detail record request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SendChargeDetailRecordRequest SendChargeDetailRecordStart1,
                                           SendChargeDetailRecordRequest SendChargeDetailRecordStart2)

            => !(SendChargeDetailRecordStart1 == SendChargeDetailRecordStart2);

        #endregion

        #endregion

        #region IEquatable<SendChargeDetailRecordStart> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is SendChargeDetailRecordRequest sendChargeDetailRecordRequest &&
                   Equals(sendChargeDetailRecordRequest);

        #endregion

        #region Equals(SendChargeDetailRecordStart)

        /// <summary>
        /// Compares two send charge detail record requests for equality.
        /// </summary>
        /// <param name="SendChargeDetailRecordStart">An send charge detail record request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SendChargeDetailRecordRequest SendChargeDetailRecordStart)

            => !(SendChargeDetailRecordStart is null) &&
                 ChargeDetailRecord.Equals(SendChargeDetailRecordStart.ChargeDetailRecord);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => ChargeDetailRecord.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => ChargeDetailRecord.ToString();

        #endregion

    }

}
