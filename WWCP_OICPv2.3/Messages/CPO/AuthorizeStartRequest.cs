/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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
    /// The AuthorizeStart request.
    /// </summary>
    public class AuthorizeStartRequest : ARequest<AuthorizeStartRequest>
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the EVSE operator sending this request.
        /// </summary>
        [Mandatory]
        public Operator_Id            OperatorId             { get; }

        /// <summary>
        /// The authentication data used to authorize the user or the car.
        /// </summary>
        [Mandatory]
        public Identification         Identification         { get; }

        /// <summary>
        /// An optional EVSE identification.
        /// </summary>
        [Optional]
        public EVSE_Id?               EVSEId                 { get; }

        /// <summary>
        /// An optional partner product identification (for identifying a charging tariff).
        /// </summary>
        [Optional]
        public PartnerProduct_Id?     PartnerProductId       { get; }

        /// <summary>
        /// An optional charging session identification.
        /// </summary>
        [Optional]
        public Session_Id?            SessionId              { get; }

        /// <summary>
        /// An optional CPO partner session identification.
        /// </summary>
        [Optional]
        public CPOPartnerSession_Id?  CPOPartnerSessionId    { get; }

        /// <summary>
        /// An optional EMP partner session identification.
        /// </summary>
        [Optional]
        public EMPPartnerSession_Id?  EMPPartnerSessionId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new AuthorizeStart request.
        /// </summary>
        /// <param name="OperatorId">The unqiue identification of the EVSE operator sending this request.</param>
        /// <param name="Identification">Authentication data used to authorize the user or the car.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification (for identifying a charging tariff).</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public AuthorizeStartRequest(Operator_Id            OperatorId,
                                     Identification         Identification,
                                     EVSE_Id?               EVSEId                = null,
                                     PartnerProduct_Id?     PartnerProductId      = null,
                                     Session_Id?            SessionId             = null,
                                     CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                     EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                     Process_Id?            ProcessId             = null,
                                     JObject?               CustomData            = null,

                                     DateTime?              Timestamp             = null,
                                     CancellationToken?     CancellationToken     = null,
                                     EventTracking_Id?      EventTrackingId       = null,
                                     TimeSpan?              RequestTimeout        = null)

            : base(ProcessId,
                   CustomData,
                   Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.OperatorId           = OperatorId;
            this.Identification       = Identification;
            this.EVSEId               = EVSEId;
            this.PartnerProductId     = PartnerProductId;
            this.SessionId            = SessionId;
            this.CPOPartnerSessionId  = CPOPartnerSessionId;
            this.EMPPartnerSessionId  = EMPPartnerSessionId;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#eRoamingAuthorizeStart

        // {
        //   "SessionID":               "string",
        //   "CPOPartnerSessionID":     "string",
        //   "EMPPartnerSessionID":     "string",
        //   "OperatorID":              "string",
        //   "EvseID":                  "string",
        //   "PartnerProductID":        "string",
        //   "Identification": {
        //     "RFIDMifareFamilyIdentification": {
        //       "UID":                 "string"
        //     },
        //     "QRCodeIdentification": {
        //       "EvcoID":              "string",
        //       "HashedPIN": {
        //         "Function":          "Bcrypt",
        //         "LegacyHashData": {
        //           "Function":        "MD5",
        //           "Salt":            "string",
        //           "Value":           "string"
        //         },
        //         "Value":             "string"
        //       },
        //       "PIN":                 "string"
        //     },
        //     "PlugAndChargeIdentification": {
        //       "EvcoID":              "string"
        //     },
        //     "RemoteIdentification": {
        //       "EvcoID":              "string"
        //     },
        //     "RFIDIdentification": {
        //       "EvcoID":              "string",
        //       "ExpiryDate":          "2021-01-10T16:13:09.941Z",
        //       "PrintedNumber":       "string",
        //       "RFID":                "mifareCls",
        //       "UID":                 "string"
        //     }
        //   }
        // }

        // {
        //     "SessionID":            "77641229-f359-40eb-b1d3-b143d6e008c5",
        //     "CPOPartnerSessionID":  "1d2c2b4e-a8f9-4ef6-9cfc-c768a814cc98",
        //     "EMPPartnerSessionID":   null,
        //     "OperatorID":           "DE*BDO",
        //     "EvseID":               "DE*BDO*E*TEST*1",
        //     "PartnerProductID":     "ATOMSTROM",
        //     "Identification": {
        //         "RFIDMifareFamilyIdentification": {
        //             "UID":          "00000000"
        //         }
        //     }
        // }

        #endregion

        #region (static) Parse   (JSON, OperatorIdURL, ..., CustomAuthorizeStartRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of an AuthorizeStart request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OperatorIdURL">The EVSE operator identification given in the URL of the HTTP request.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomAuthorizeStartRequestParser">A delegate to parse custom AuthorizeStart JSON objects.</param>
        public static AuthorizeStartRequest Parse(JObject                                              JSON,
                                                  Operator_Id                                          OperatorIdURL,
                                                  Process_Id?                                          ProcessId                           = null,

                                                  DateTime?                                            Timestamp                           = null,
                                                  CancellationToken?                                   CancellationToken                   = null,
                                                  EventTracking_Id?                                    EventTrackingId                     = null,
                                                  TimeSpan?                                            RequestTimeout                      = null,

                                                  CustomJObjectParserDelegate<AuthorizeStartRequest>?  CustomAuthorizeStartRequestParser   = null)
        {

            if (TryParse(JSON,
                         OperatorIdURL,
                         out AuthorizeStartRequest?  auhorizeStartRequest,
                         out String?                 errorResponse,
                         ProcessId,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout,
                         CustomAuthorizeStartRequestParser))
            {
                return auhorizeStartRequest!;
            }

            throw new ArgumentException("The given JSON representation of an AuthorizeStart request is invalid: " + errorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, OperatorIdURL, ..., CustomAuthorizeStartRequestParser = null)

        /// <summary>
        /// Parse the given text representation of an AuthorizeStart request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="OperatorIdURL">The EVSE operator identification given in the URL of the HTTP request.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomAuthorizeStartRequestParser">A delegate to parse custom AuthorizeStart request JSON objects.</param>
        public static AuthorizeStartRequest Parse(String                                               Text,
                                                  Operator_Id                                          OperatorIdURL,
                                                  Process_Id?                                          ProcessId                           = null,

                                                  DateTime?                                            Timestamp                           = null,
                                                  CancellationToken?                                   CancellationToken                   = null,
                                                  EventTracking_Id?                                    EventTrackingId                     = null,
                                                  TimeSpan?                                            RequestTimeout                      = null,

                                                  CustomJObjectParserDelegate<AuthorizeStartRequest>?  CustomAuthorizeStartRequestParser   = null)
        {

            if (TryParse(Text,
                         OperatorIdURL,
                         out AuthorizeStartRequest?  auhorizeStartRequest,
                         out String?                 errorResponse,
                         ProcessId,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout,
                         CustomAuthorizeStartRequestParser))
            {
                return auhorizeStartRequest!;
            }

            throw new ArgumentException("The given text representation of an AuthorizeStart request is invalid: " + errorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, OperatorIdURL, out AuthorizeStartRequest, out ErrorResponse, ..., CustomAuthorizeStartRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of an AuthorizeStart request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OperatorIdURL">The EVSE operator identification given in the URL of the HTTP request.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="AuthorizeStartRequest">The parsed AuthorizeStart request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomAuthorizeStartRequestParser">A delegate to parse custom AuthorizeStart request JSON objects.</param>
        public static Boolean TryParse(JObject                                              JSON,
                                       Operator_Id                                          OperatorIdURL,
                                       out AuthorizeStartRequest?                           AuthorizeStartRequest,
                                       out String?                                          ErrorResponse,
                                       Process_Id?                                          ProcessId                           = null,

                                       DateTime?                                            Timestamp                           = null,
                                       CancellationToken?                                   CancellationToken                   = null,
                                       EventTracking_Id?                                    EventTrackingId                     = null,
                                       TimeSpan?                                            RequestTimeout                      = null,

                                       CustomJObjectParserDelegate<AuthorizeStartRequest>?  CustomAuthorizeStartRequestParser   = null)
        {

            try
            {

                AuthorizeStartRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse OperatorId                [mandatory]

                if (!JSON.ParseMandatory("OperatorID",
                                         "operator identification",
                                         Operator_Id.TryParse,
                                         out Operator_Id OperatorId,
                                         out ErrorResponse))
                {
                    return false;
                }

                if (OperatorId != OperatorIdURL)
                {
                    ErrorResponse = "Inconsistend operator identifications: '" + OperatorIdURL + "' (URL) <> '" + OperatorId + "' (JSON)!";
                    return false;
                }

                #endregion

                #region Parse Identification            [mandatory]

                if (!JSON.ParseMandatoryJSON2("Identification",
                                              "identification",
                                              OICPv2_3.Identification.TryParse,
                                              out Identification? Identification,
                                              out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse EVSEId                    [optional]

                if (JSON.ParseOptional("EvseID",
                                       "EVSE identification",
                                       EVSE_Id.TryParse,
                                       out EVSE_Id? EVSEId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse PartnerProductId          [optional]

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

                #region Parse SessionId                 [optional]

                if (JSON.ParseOptional("SessionID",
                                       "session identification",
                                       Session_Id.TryParse,
                                       out Session_Id? SessionId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CPOPartnerSessionId       [optional]

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

                #region Parse EMPPartnerSessionId       [optional]

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

                #region Parse CustomData                [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                AuthorizeStartRequest = new AuthorizeStartRequest(OperatorId,
                                                                  Identification!,
                                                                  EVSEId,
                                                                  PartnerProductId,
                                                                  SessionId,
                                                                  CPOPartnerSessionId,
                                                                  EMPPartnerSessionId,
                                                                  ProcessId,
                                                                  customData,

                                                                  Timestamp,
                                                                  CancellationToken,
                                                                  EventTrackingId,
                                                                  RequestTimeout);

                if (CustomAuthorizeStartRequestParser is not null)
                    AuthorizeStartRequest = CustomAuthorizeStartRequestParser(JSON,
                                                                              AuthorizeStartRequest);

                return true;

            }
            catch (Exception e)
            {
                AuthorizeStartRequest  = default;
                ErrorResponse          = "The given JSON representation of an AuthorizeStart request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, OperatorIdURL, out AuthorizeStartRequest, out ErrorResponse, ..., CustomAuthorizeStartRequestParser = null)

        /// <summary>
        /// Try to parse the given text representation of an AuthorizeStart request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="OperatorIdURL">The EVSE operator identification given in the URL of the HTTP request.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="AuthorizeStartRequest">The parsed AuthorizeStart request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomAuthorizeStartRequestParser">A delegate to parse custom AuthorizeStart request JSON objects.</param>
        public static Boolean TryParse(String                                               Text,
                                       Operator_Id                                          OperatorIdURL,
                                       out AuthorizeStartRequest?                           AuthorizeStartRequest,
                                       out String?                                          ErrorResponse,
                                       Process_Id?                                          ProcessId                           = null,

                                       DateTime?                                            Timestamp                           = null,
                                       CancellationToken?                                   CancellationToken                   = null,
                                       EventTracking_Id?                                    EventTrackingId                     = null,
                                       TimeSpan?                                            RequestTimeout                      = null,

                                       CustomJObjectParserDelegate<AuthorizeStartRequest>?  CustomAuthorizeStartRequestParser   = null)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                OperatorIdURL,
                                out AuthorizeStartRequest,
                                out ErrorResponse,
                                ProcessId,
                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout,
                                CustomAuthorizeStartRequestParser);

            }
            catch (Exception e)
            {
                AuthorizeStartRequest  = default;
                ErrorResponse          = "The given text representation of an AuthorizeStart request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomAuthorizeStartRequestSerializer = null, CustomOperatorEVSEDataSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizeStartRequestSerializer">A delegate to customize the serialization of AuthorizeStartRequest responses.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<AuthorizeStartRequest>?  CustomAuthorizeStartRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<Identification>?         CustomIdentificationSerializer          = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("OperatorID",                 OperatorId.               ToString()),
                           new JProperty("Identification",             Identification.           ToJSON(CustomIdentificationSerializer)),

                           EVSEId.HasValue
                               ? new JProperty("EvseID",               EVSEId.             Value.ToString())
                               : null,

                           PartnerProductId.HasValue
                               ? new JProperty("PartnerProductID",     PartnerProductId.   Value.ToString())
                               : null,

                           SessionId.HasValue
                               ? new JProperty("SessionID",            SessionId.          Value.ToString())
                               : null,

                           CPOPartnerSessionId.HasValue
                               ? new JProperty("CPOPartnerSessionID",  CPOPartnerSessionId.Value.ToString())
                               : null,

                           EMPPartnerSessionId.HasValue
                               ? new JProperty("EMPPartnerSessionID",  EMPPartnerSessionId.Value.ToString())
                               : null,

                           CustomData is not null
                               ? new JProperty("CustomData",           CustomData)
                               : null

                       );

            return CustomAuthorizeStartRequestSerializer is not null
                       ? CustomAuthorizeStartRequestSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthorizeStart1, AuthorizeStart2)

        /// <summary>
        /// Compares two authorize start requests for equality.
        /// </summary>
        /// <param name="AuthorizeStart1">An authorize start request.</param>
        /// <param name="AuthorizeStart2">Another authorize start request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthorizeStartRequest AuthorizeStart1,
                                           AuthorizeStartRequest AuthorizeStart2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(AuthorizeStart1, AuthorizeStart2))
                return true;

            // If one is null, but not both, return false.
            if (AuthorizeStart1 is null || AuthorizeStart2 is null)
                return false;

            return AuthorizeStart1.Equals(AuthorizeStart2);

        }

        #endregion

        #region Operator != (AuthorizeStart1, AuthorizeStart2)

        /// <summary>
        /// Compares two authorize start requests for inequality.
        /// </summary>
        /// <param name="AuthorizeStart1">An authorize start request.</param>
        /// <param name="AuthorizeStart2">Another authorize start request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthorizeStartRequest AuthorizeStart1,
                                           AuthorizeStartRequest AuthorizeStart2)

            => !(AuthorizeStart1 == AuthorizeStart2);

        #endregion

        #endregion

        #region IEquatable<AuthorizeStartRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is AuthorizeStartRequest authorizeStartRequest &&
                   Equals(authorizeStartRequest);

        #endregion

        #region Equals(AuthorizeStart)

        /// <summary>
        /// Compares two authorize start requests for equality.
        /// </summary>
        /// <param name="AuthorizeStart">An authorize start request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AuthorizeStartRequest? AuthorizeStart)

            => AuthorizeStart is not null &&

               OperatorId.    Equals(AuthorizeStart.OperatorId)     &&
               Identification.Equals(AuthorizeStart.Identification) &&

               ((!EVSEId.             HasValue && !AuthorizeStart.EVSEId.             HasValue) ||
                 (EVSEId.             HasValue &&  AuthorizeStart.EVSEId.             HasValue && EVSEId.             Value.Equals(AuthorizeStart.EVSEId.             Value))) &&

               ((!PartnerProductId.   HasValue && !AuthorizeStart.PartnerProductId.   HasValue) ||
                 (PartnerProductId.   HasValue &&  AuthorizeStart.PartnerProductId.   HasValue && PartnerProductId.   Value.Equals(AuthorizeStart.PartnerProductId.   Value))) &&

               ((!SessionId.          HasValue && !AuthorizeStart.SessionId.          HasValue) ||
                 (SessionId.          HasValue &&  AuthorizeStart.SessionId.          HasValue && SessionId.          Value.Equals(AuthorizeStart.SessionId.          Value))) &&

               ((!CPOPartnerSessionId.HasValue && !AuthorizeStart.CPOPartnerSessionId.HasValue) ||
                 (CPOPartnerSessionId.HasValue &&  AuthorizeStart.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(AuthorizeStart.CPOPartnerSessionId.Value))) &&

               ((!EMPPartnerSessionId.HasValue && !AuthorizeStart.EMPPartnerSessionId.HasValue) ||
                 (EMPPartnerSessionId.HasValue &&  AuthorizeStart.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(AuthorizeStart.EMPPartnerSessionId.Value)));

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

                return OperatorId.          GetHashCode()       * 17 ^
                       Identification.      GetHashCode()       * 13 ^

                      (EVSEId?.             GetHashCode() ?? 0) * 11 ^
                      (SessionId?.          GetHashCode() ?? 0) *  7 ^
                      (CPOPartnerSessionId?.GetHashCode() ?? 0) *  5 ^
                      (EMPPartnerSessionId?.GetHashCode() ?? 0) *  3 ^
                      (PartnerProductId?.   GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Identification,

                             EVSEId.HasValue
                                 ? " at " + EVSEId
                                 : "",

                             " (", OperatorId, ")",

                             PartnerProductId.HasValue
                                 ? " using " + PartnerProductId
                                 : "");

        #endregion

    }

}
