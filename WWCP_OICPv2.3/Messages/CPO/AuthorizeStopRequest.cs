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
    /// The AuthorizeStop request.
    /// </summary>
    public class AuthorizeStopRequest : ARequest<AuthorizeStopRequest>
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the EVSE operator sending this request.
        /// </summary>
        [Mandatory]
        public Operator_Id            OperatorId             { get; }

        /// <summary>
        /// The charging session identification.
        /// </summary>
        [Mandatory]
        public Session_Id             SessionId              { get; }

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
        /// Create a new AuthorizeStop request.
        /// </summary>
        /// <param name="OperatorId">The unqiue identification of the EVSE operator sending this request.</param>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="Identification">Authentication data used to authorize the user or the car.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public AuthorizeStopRequest(Operator_Id            OperatorId,
                                    Session_Id             SessionId,
                                    Identification         Identification,
                                    EVSE_Id?               EVSEId                = null,
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
            this.SessionId            = SessionId;
            this.CPOPartnerSessionId  = CPOPartnerSessionId;
            this.EMPPartnerSessionId  = EMPPartnerSessionId;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#eRoamingAuthorizeStop

        // {
        //   "OperatorID":              "string",
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
        //   },
        //   "EvseID":                  "string",
        //   "PartnerProductID":        "string",
        //   "SessionID":               "string",
        //   "CPOPartnerSessionID":     "string",
        //   "EMPPartnerSessionID":     "string"
        // }

        #endregion

        #region (static) Parse   (JSON, OperatorIdURL, ..., CustomAuthorizeStopRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of an AuthorizeStop request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OperatorIdURL">The EVSE operator identification given in the URL of the HTTP request.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomAuthorizeStopRequestParser">A delegate to parse custom AuthorizeStop JSON objects.</param>
        public static AuthorizeStopRequest Parse(JObject                                             JSON,
                                                 Operator_Id                                         OperatorIdURL,
                                                 Process_Id?                                         ProcessId                          = null,

                                                 DateTime?                                           Timestamp                          = null,
                                                 CancellationToken?                                  CancellationToken                  = null,
                                                 EventTracking_Id?                                   EventTrackingId                    = null,
                                                 TimeSpan?                                           RequestTimeout                     = null,

                                                 CustomJObjectParserDelegate<AuthorizeStopRequest>?  CustomAuthorizeStopRequestParser   = null)
        {

            if (TryParse(JSON,
                         OperatorIdURL,
                         out var auhorizeStopRequest,
                         out var errorResponse,
                         ProcessId,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout,
                         CustomAuthorizeStopRequestParser))
            {
                return auhorizeStopRequest!;
            }

            throw new ArgumentException("The given JSON representation of an AuthorizeStop request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, OperatorIdURL, out AuthorizeStopRequest, out ErrorResponse, ..., CustomAuthorizeStopRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of an AuthorizeStop request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OperatorIdURL">The EVSE operator identification given in the URL of the HTTP request.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="AuthorizeStopRequest">The parsed AuthorizeStop request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomAuthorizeStopRequestParser">A delegate to parse custom AuthorizeStop request JSON objects.</param>
        public static Boolean TryParse(JObject                                             JSON,
                                       Operator_Id                                         OperatorIdURL,
                                       out AuthorizeStopRequest?                           AuthorizeStopRequest,
                                       out String?                                         ErrorResponse,
                                       Process_Id?                                         ProcessId                          = null,

                                       DateTime?                                           Timestamp                          = null,
                                       CancellationToken?                                  CancellationToken                  = null,
                                       EventTracking_Id?                                   EventTrackingId                    = null,
                                       TimeSpan?                                           RequestTimeout                     = null,

                                       CustomJObjectParserDelegate<AuthorizeStopRequest>?  CustomAuthorizeStopRequestParser   = null)
        {

            try
            {

                AuthorizeStopRequest = default;

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

                #region Parse SessionId                 [mandatory]

                if (!JSON.ParseMandatory("SessionID",
                                         "session identification",
                                         Session_Id.TryParse,
                                         out Session_Id SessionId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Identification            [mandatory]

                if (!JSON.ParseMandatoryJSON("Identification",
                                             "identification",
                                             OICPv2_3.Identification.TryParse,
                                             out Identification? Identification,
                                             out ErrorResponse) ||
                     Identification is null)
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


                AuthorizeStopRequest = new AuthorizeStopRequest(OperatorId,
                                                                SessionId,
                                                                Identification!,
                                                                EVSEId,
                                                                CPOPartnerSessionId,
                                                                EMPPartnerSessionId,
                                                                ProcessId,
                                                                customData,

                                                                Timestamp,
                                                                CancellationToken,
                                                                EventTrackingId,
                                                                RequestTimeout);

                if (CustomAuthorizeStopRequestParser is not null)
                    AuthorizeStopRequest = CustomAuthorizeStopRequestParser(JSON,
                                                                            AuthorizeStopRequest);

                return true;

            }
            catch (Exception e)
            {
                AuthorizeStopRequest  = default;
                ErrorResponse         = "The given JSON representation of an AuthorizeStop request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomAuthorizeStopRequestSerializer = null, CustomOperatorEVSEDataSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizeStopRequestSerializer">A delegate to customize the serialization of AuthorizeStopRequest responses.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<AuthorizeStopRequest>?  CustomAuthorizeStopRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<Identification>?        CustomIdentificationSerializer         = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("OperatorID",                 OperatorId.               ToString()),
                           new JProperty("SessionID",                  SessionId.                ToString()),
                           new JProperty("Identification",             Identification.           ToJSON(CustomIdentificationSerializer)),

                           EVSEId.HasValue
                               ? new JProperty("EvseID",               EVSEId.             Value.ToString())
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

            return CustomAuthorizeStopRequestSerializer is not null
                       ? CustomAuthorizeStopRequestSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthorizeStop1, AuthorizeStop2)

        /// <summary>
        /// Compares two authorize stop requests for equality.
        /// </summary>
        /// <param name="AuthorizeStop1">An authorize stop request.</param>
        /// <param name="AuthorizeStop2">Another authorize stop request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthorizeStopRequest AuthorizeStop1,
                                           AuthorizeStopRequest AuthorizeStop2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(AuthorizeStop1, AuthorizeStop2))
                return true;

            // If one is null, but not both, return false.
            if (AuthorizeStop1 is null || AuthorizeStop2 is null)
                return false;

            return AuthorizeStop1.Equals(AuthorizeStop2);

        }

        #endregion

        #region Operator != (AuthorizeStop1, AuthorizeStop2)

        /// <summary>
        /// Compares two authorize stop requests for inequality.
        /// </summary>
        /// <param name="AuthorizeStop1">An authorize stop request.</param>
        /// <param name="AuthorizeStop2">Another authorize stop request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthorizeStopRequest AuthorizeStop1,
                                           AuthorizeStopRequest AuthorizeStop2)

            => !(AuthorizeStop1 == AuthorizeStop2);

        #endregion

        #endregion

        #region IEquatable<AuthorizeStopRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is AuthorizeStopRequest authorizeStopRequest &&
                   Equals(authorizeStopRequest);

        #endregion

        #region Equals(AuthorizeStop)

        /// <summary>
        /// Compares two authorize stop requests for equality.
        /// </summary>
        /// <param name="AuthorizeStop">An authorize stop request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AuthorizeStopRequest? AuthorizeStop)

            => AuthorizeStop is not null &&

               OperatorId.    Equals(AuthorizeStop.OperatorId)     &&
               SessionId.     Equals(AuthorizeStop.SessionId)      &&
               Identification.Equals(AuthorizeStop.Identification) &&

               ((!EVSEId.             HasValue && !AuthorizeStop.EVSEId.             HasValue) ||
                 (EVSEId.             HasValue &&  AuthorizeStop.EVSEId.             HasValue && EVSEId.             Value.Equals(AuthorizeStop.EVSEId.             Value))) &&

               ((!CPOPartnerSessionId.HasValue && !AuthorizeStop.CPOPartnerSessionId.HasValue) ||
                 (CPOPartnerSessionId.HasValue &&  AuthorizeStop.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(AuthorizeStop.CPOPartnerSessionId.Value))) &&

               ((!EMPPartnerSessionId.HasValue && !AuthorizeStop.EMPPartnerSessionId.HasValue) ||
                 (EMPPartnerSessionId.HasValue &&  AuthorizeStop.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(AuthorizeStop.EMPPartnerSessionId.Value)));

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

                return OperatorId.          GetHashCode()       * 13 ^
                       SessionId.           GetHashCode()       * 11 ^
                       Identification.      GetHashCode()       *  7 ^

                      (EVSEId?.             GetHashCode() ?? 0) *  5 ^
                      (CPOPartnerSessionId?.GetHashCode() ?? 0) *  3 ^
                      (EMPPartnerSessionId?.GetHashCode() ?? 0);

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

                             " (", OperatorId, ")");

        #endregion

    }

}
