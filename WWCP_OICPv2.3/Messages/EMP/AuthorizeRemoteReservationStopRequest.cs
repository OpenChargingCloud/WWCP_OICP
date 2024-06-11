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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The AuthorizeRemoteReservationStop request.
    /// </summary>
    public class AuthorizeRemoteReservationStopRequest : ARequest<AuthorizeRemoteReservationStopRequest>
    {

        #region Properties

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        [Mandatory]
        public Provider_Id            ProviderId             { get; }

        /// <summary>
        /// The EVSE identification.
        /// </summary>
        [Mandatory]
        public EVSE_Id                EVSEId                 { get; }

        /// <summary>
        /// The charging session identification.
        /// </summary>
        [Mandatory]
        public Session_Id             SessionId              { get; }

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
        /// Create an AuthorizeRemoteReservationStop request.
        /// </summary>
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">An charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public AuthorizeRemoteReservationStopRequest(Provider_Id            ProviderId,
                                                     EVSE_Id                EVSEId,
                                                     Session_Id             SessionId,
                                                     CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                                     EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                                     Process_Id?            ProcessId             = null,
                                                     JObject?               CustomData            = null,

                                                     DateTime?              Timestamp             = null,
                                                     EventTracking_Id?      EventTrackingId       = null,
                                                     TimeSpan?              RequestTimeout        = null,
                                                     CancellationToken      CancellationToken     = default)

            : base(ProcessId,
                   CustomData,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.ProviderId           = ProviderId;
            this.EVSEId               = EVSEId;
            this.SessionId            = SessionId;
            this.CPOPartnerSessionId  = CPOPartnerSessionId;
            this.EMPPartnerSessionId  = EMPPartnerSessionId;


            unchecked
            {

                hashCode = this.ProviderId.          GetHashCode()       * 17 ^
                           this.EVSEId.              GetHashCode()       *  7 ^
                           this.SessionId.           GetHashCode()       *  5 ^
                          (this.CPOPartnerSessionId?.GetHashCode() ?? 0) *  3 ^
                          (this.EMPPartnerSessionId?.GetHashCode() ?? 0);

            }

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#eRoamingAuthorizeRemoteReservationStop

        // {
        //   "CPOPartnerSessionID": "string",
        //   "EMPPartnerSessionID": "string",
        //   "EvseID":              "string",
        //   "Identification": {
        //     "RFIDMifareFamilyIdentification": {
        //       "UID":             "string"
        //     },
        //     "QRCodeIdentification": {
        //       "EvcoID":          "string",
        //       "HashedPIN": {
        //         "Function":      "Bcrypt",
        //         "LegacyHashData": {
        //           "Function":    "MD5",
        //           "Salt":        "string",
        //           "Value":       "string"
        //         },
        //         "Value":         "string"
        //       },
        //       "PIN":             "string"
        //     },
        //     "PlugAndChargeIdentification": {
        //       "EvcoID":          "string"
        //     },
        //     "RemoteIdentification": {
        //       "EvcoID":          "string"
        //     },
        //     "RFIDIdentification": {
        //       "EvcoID":          "string",
        //       "ExpiryDate":      "2021-01-11T08:13:21.929Z",
        //       "PrintedNumber":   "string",
        //       "RFID":            "mifareCls",
        //       "UID":             "string"
        //     }
        //   },
        //   "PartnerProductID":    "string",
        //   "ProviderID":          "string",
        //   "SessionID":           "string",
        //   "Duration":            0
        // }

        #endregion

        #region (static) Parse   (JSON, CustomAuthorizeRemoteReservationStopRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a AuthorizeRemoteReservationStop request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ProviderIdURL">The provider identification given in the URL of the HTTP request.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomAuthorizeRemoteReservationStopRequestParser">A delegate to parse custom AuthorizeRemoteReservationStop JSON objects.</param>
        public static AuthorizeRemoteReservationStopRequest Parse(JObject                                                              JSON,
                                                                  Provider_Id                                                          ProviderIdURL,
                                                                  Process_Id?                                                          ProcessId                                           = null,

                                                                  DateTime?                                                            Timestamp                                           = null,
                                                                  EventTracking_Id?                                                    EventTrackingId                                     = null,
                                                                  TimeSpan?                                                            RequestTimeout                                      = null,
                                                                  CancellationToken                                                    CancellationToken                                   = default,
                                                                  CustomJObjectParserDelegate<AuthorizeRemoteReservationStopRequest>?  CustomAuthorizeRemoteReservationStopRequestParser   = null)
        {

            if (TryParse(JSON,
                         ProviderIdURL,
                         out var authorizeRemoteReservationStopRequest,
                         out var errorResponse,
                         ProcessId,
                         Timestamp,
                         EventTrackingId,
                         RequestTimeout,
                         CustomAuthorizeRemoteReservationStopRequestParser,
                         CancellationToken))
            {
                return authorizeRemoteReservationStopRequest;
            }

            throw new ArgumentException("The given JSON representation of a AuthorizeRemoteReservationStop request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out AuthorizeRemoteReservationStopRequest, out ErrorResponse, ..., CustomAuthorizeRemoteReservationStopRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a AuthorizeRemoteReservationStop request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ProviderIdURL">The provider identification given in the URL of the HTTP request.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="AuthorizeRemoteReservationStopRequest">The parsed AuthorizeRemoteReservationStop request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomAuthorizeRemoteReservationStopRequestParser">A delegate to parse custom AuthorizeRemoteReservationStop request JSON objects.</param>
        public static Boolean TryParse(JObject                                                              JSON,
                                       Provider_Id                                                          ProviderIdURL,
                                       [NotNullWhen(true)]  out AuthorizeRemoteReservationStopRequest?      AuthorizeRemoteReservationStopRequest,
                                       [NotNullWhen(false)] out String?                                     ErrorResponse,
                                       Process_Id?                                                          ProcessId                                           = null,

                                       DateTime?                                                            Timestamp                                           = null,
                                       EventTracking_Id?                                                    EventTrackingId                                     = null,
                                       TimeSpan?                                                            RequestTimeout                                      = null,
                                       CustomJObjectParserDelegate<AuthorizeRemoteReservationStopRequest>?  CustomAuthorizeRemoteReservationStopRequestParser   = null,
                                       CancellationToken                                                    CancellationToken                                   = default)
        {

            try
            {

                AuthorizeRemoteReservationStopRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse ProviderId                [mandatory]

                if (!JSON.ParseMandatory("ProviderID",
                                         "provider identification",
                                         Provider_Id.TryParse,
                                         out Provider_Id ProviderId,
                                         out             ErrorResponse))
                {
                    return false;
                }

                if (ProviderId != ProviderIdURL)
                {
                    ErrorResponse = "Inconsistend provider identifications: '" + ProviderIdURL + "' (URL) <> '" + ProviderId + "' (JSON)!";
                    return false;
                }

                #endregion

                #region Parse EVSEId                    [mandatory]

                if (!JSON.ParseMandatory("EvseID",
                                         "EVSE identification",
                                         EVSE_Id.TryParse,
                                         out EVSE_Id EVSEId,
                                         out ErrorResponse))
                {
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


                AuthorizeRemoteReservationStopRequest = new AuthorizeRemoteReservationStopRequest(
                                                            ProviderId,
                                                            EVSEId,
                                                            SessionId,
                                                            CPOPartnerSessionId,
                                                            EMPPartnerSessionId,
                                                            ProcessId,
                                                            customData,

                                                            Timestamp,
                                                            EventTrackingId,
                                                            RequestTimeout,
                                                            CancellationToken
                                                        );

                if (CustomAuthorizeRemoteReservationStopRequestParser is not null)
                    AuthorizeRemoteReservationStopRequest = CustomAuthorizeRemoteReservationStopRequestParser(JSON,
                                                                                                              AuthorizeRemoteReservationStopRequest);

                return true;

            }
            catch (Exception e)
            {
                AuthorizeRemoteReservationStopRequest  = default;
                ErrorResponse                          = "The given JSON representation of a AuthorizeRemoteReservationStop request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomAuthorizeRemoteReservationStopRequestSerializer = null)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizeRemoteReservationStopRequestSerializer">A delegate to customize the serialization of AuthorizeRemoteReservationStopRequest responses.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<AuthorizeRemoteReservationStopRequest>?  CustomAuthorizeRemoteReservationStopRequestSerializer   = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("ProviderID",            ProviderId.               ToString()),
                                 new JProperty("EvseID",                EVSEId.                   ToString()),
                                 new JProperty("SessionID",             SessionId.                ToString()),

                           CPOPartnerSessionId.HasValue
                               ? new JProperty("CPOPartnerSessionID",   CPOPartnerSessionId.Value.ToString())
                               : null,

                           EMPPartnerSessionId.HasValue
                               ? new JProperty("EMPPartnerSessionID",   EMPPartnerSessionId.Value.ToString())
                               : null,

                           CustomData is not null
                               ? new JProperty("CustomData",            CustomData)
                               : null

                       );

            return CustomAuthorizeRemoteReservationStopRequestSerializer is not null
                       ? CustomAuthorizeRemoteReservationStopRequestSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthorizeRemoteReservationStopRequest1, AuthorizeRemoteReservationStopRequest2)

        /// <summary>
        /// Compares two authorize remote reservation stop requests for equality.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStopRequest1">An authorize remote reservation stop request.</param>
        /// <param name="AuthorizeRemoteReservationStopRequest2">Another authorize remote reservation stop request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthorizeRemoteReservationStopRequest AuthorizeRemoteReservationStopRequest1,
                                           AuthorizeRemoteReservationStopRequest AuthorizeRemoteReservationStopRequest2)
        {

            if (ReferenceEquals(AuthorizeRemoteReservationStopRequest1, AuthorizeRemoteReservationStopRequest2))
                return true;

            if (AuthorizeRemoteReservationStopRequest1 is null || AuthorizeRemoteReservationStopRequest2 is null)
                return false;

            return AuthorizeRemoteReservationStopRequest1.Equals(AuthorizeRemoteReservationStopRequest2);

        }

        #endregion

        #region Operator != (AuthorizeRemoteReservationStopRequest1, AuthorizeRemoteReservationStopRequest2)

        /// <summary>
        /// Compares two authorize remote reservation stop requests for inequality.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStopRequest1">An authorize remote reservation stop request.</param>
        /// <param name="AuthorizeRemoteReservationStopRequest2">Another authorize remote reservation stop request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthorizeRemoteReservationStopRequest AuthorizeRemoteReservationStopRequest1,
                                           AuthorizeRemoteReservationStopRequest AuthorizeRemoteReservationStopRequest2)

            => !(AuthorizeRemoteReservationStopRequest1 == AuthorizeRemoteReservationStopRequest2);

        #endregion

        #endregion

        #region IEquatable<AuthorizeRemoteReservationStopRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is AuthorizeRemoteReservationStopRequest authorizeRemoteReservationStopRequest &&
                   Equals(authorizeRemoteReservationStopRequest);

        #endregion

        #region Equals(AuthorizeRemoteReservationStopRequest)

        /// <summary>
        /// Compares two authorize remote reservation stop requests for equality.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStopRequest">An authorize remote reservation stop request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AuthorizeRemoteReservationStopRequest? AuthorizeRemoteReservationStopRequest)

            => AuthorizeRemoteReservationStopRequest is not null &&

               ProviderId.Equals(AuthorizeRemoteReservationStopRequest.ProviderId) &&
               EVSEId.    Equals(AuthorizeRemoteReservationStopRequest.EVSEId)     &&
               SessionId. Equals(AuthorizeRemoteReservationStopRequest.SessionId)  &&

               ((!CPOPartnerSessionId.HasValue && !AuthorizeRemoteReservationStopRequest.CPOPartnerSessionId.HasValue) ||
                 (CPOPartnerSessionId.HasValue &&  AuthorizeRemoteReservationStopRequest.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(AuthorizeRemoteReservationStopRequest.CPOPartnerSessionId.Value))) &&

               ((!EMPPartnerSessionId.HasValue && !AuthorizeRemoteReservationStopRequest.EMPPartnerSessionId.HasValue) ||
                 (EMPPartnerSessionId.HasValue &&  AuthorizeRemoteReservationStopRequest.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(AuthorizeRemoteReservationStopRequest.EMPPartnerSessionId.Value)));

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

            => $"{EVSEId} for {SessionId} ({ProviderId})";

        #endregion

    }

}
