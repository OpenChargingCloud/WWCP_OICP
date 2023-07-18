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
    /// The AuthorizeRemoteStop request.
    /// </summary>
    public class AuthorizeRemoteStopRequest : ARequest<AuthorizeRemoteStopRequest>
    {

        #region Properties

        /// <summary>
        /// An e-mobility provider identification.
        /// </summary>
        [Mandatory]
        public Provider_Id            ProviderId             { get; }

        /// <summary>
        /// An EVSE identification.
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
        /// Create a new AuthorizeRemoteStop request.
        /// </summary>
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">A charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public AuthorizeRemoteStopRequest(Provider_Id            ProviderId,
                                          EVSE_Id                EVSEId,
                                          Session_Id             SessionId,
                                          CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                          EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                          Process_Id?            ProcessId             = null,
                                          JObject?               CustomData            = null,

                                          DateTime?              Timestamp             = null,
                                          CancellationToken      CancellationToken     = default,
                                          EventTracking_Id?      EventTrackingId       = null,
                                          TimeSpan?              RequestTimeout        = null)

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

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#eRoamingAuthorizeRemoteStop

        // {
        //   "CPOPartnerSessionID": "string",
        //   "EMPPartnerSessionID": "string",
        //   "EvseID":              "string",
        //   "ProviderID":          "string",
        //   "SessionID":           "string"
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomAuthorizeRemoteStopRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a AuthorizeRemoteStop request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ProviderIdURL">The provider identification given in the URL of the HTTP request.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomAuthorizeRemoteStopRequestParser">A delegate to parse custom AuthorizeRemoteStop JSON objects.</param>
        public static AuthorizeRemoteStopRequest Parse(JObject                                                   JSON,
                                                       Provider_Id                                               ProviderIdURL,
                                                       Process_Id?                                               ProcessId                                = null,

                                                       DateTime?                                                 Timestamp                                = null,
                                                       CancellationToken                                         CancellationToken                        = default,
                                                       EventTracking_Id?                                         EventTrackingId                          = null,
                                                       TimeSpan?                                                 RequestTimeout                           = null,

                                                       CustomJObjectParserDelegate<AuthorizeRemoteStopRequest>?  CustomAuthorizeRemoteStopRequestParser   = null)
        {

            if (TryParse(JSON,
                         ProviderIdURL,
                         out var authorizeRemoteStopRequest,
                         out var errorResponse,
                         ProcessId,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout,
                         CustomAuthorizeRemoteStopRequestParser))
            {
                return authorizeRemoteStopRequest!;
            }

            throw new ArgumentException("The given JSON representation of a AuthorizeRemoteStop request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out AuthorizeRemoteStopRequest, out ErrorResponse, ..., CustomAuthorizeRemoteStopRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a AuthorizeRemoteStop request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ProviderIdURL">The provider identification given in the URL of the HTTP request.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="AuthorizeRemoteStopRequest">The parsed AuthorizeRemoteStop request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomAuthorizeRemoteStopRequestParser">A delegate to parse custom AuthorizeRemoteStop request JSON objects.</param>
        public static Boolean TryParse(JObject                                                   JSON,
                                       Provider_Id                                               ProviderIdURL,
                                       out AuthorizeRemoteStopRequest?                           AuthorizeRemoteStopRequest,
                                       out String?                                               ErrorResponse,
                                       Process_Id?                                               ProcessId                                = null,

                                       DateTime?                                                 Timestamp                                = null,
                                       CancellationToken                                         CancellationToken                        = default,
                                       EventTracking_Id?                                         EventTrackingId                          = null,
                                       TimeSpan?                                                 RequestTimeout                           = null,

                                       CustomJObjectParserDelegate<AuthorizeRemoteStopRequest>?  CustomAuthorizeRemoteStopRequestParser   = null)
        {

            try
            {

                AuthorizeRemoteStopRequest = default;

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


                AuthorizeRemoteStopRequest = new AuthorizeRemoteStopRequest(ProviderId,
                                                                            EVSEId,
                                                                            SessionId,
                                                                            CPOPartnerSessionId,
                                                                            EMPPartnerSessionId,
                                                                            ProcessId,
                                                                            customData,

                                                                            Timestamp,
                                                                            CancellationToken,
                                                                            EventTrackingId,
                                                                            RequestTimeout);

                if (CustomAuthorizeRemoteStopRequestParser is not null)
                    AuthorizeRemoteStopRequest = CustomAuthorizeRemoteStopRequestParser(JSON,
                                                                                        AuthorizeRemoteStopRequest);

                return true;

            }
            catch (Exception e)
            {
                AuthorizeRemoteStopRequest  = default;
                ErrorResponse               = "The given JSON representation of a AuthorizeRemoteStop request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomAuthorizeRemoteStopRequestSerializer = null)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizeRemoteStopRequestSerializer">A delegate to customize the serialization of AuthorizeRemoteStopRequest responses.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<AuthorizeRemoteStopRequest>? CustomAuthorizeRemoteStopRequestSerializer = null)
        {

            var json = JSONObject.Create(

                           new JProperty("ProviderID",                  ProviderId.               ToString()),
                           new JProperty("EvseID",                      EVSEId.                   ToString()),
                           new JProperty("SessionID",                   SessionId.                ToString()),

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

            return CustomAuthorizeRemoteStopRequestSerializer is not null
                       ? CustomAuthorizeRemoteStopRequestSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthorizeRemoteStopRequest1, AuthorizeRemoteStopRequest2)

        /// <summary>
        /// Compares two authorize remote stop requests for equality.
        /// </summary>
        /// <param name="AuthorizeRemoteStopRequest1">An authorize remote stop request.</param>
        /// <param name="AuthorizeRemoteStopRequest2">Another authorize remote stop request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthorizeRemoteStopRequest AuthorizeRemoteStopRequest1,
                                           AuthorizeRemoteStopRequest AuthorizeRemoteStopRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(AuthorizeRemoteStopRequest1, AuthorizeRemoteStopRequest2))
                return true;

            // If one is null, but not both, return false.
            if (AuthorizeRemoteStopRequest1 is null || AuthorizeRemoteStopRequest2 is null)
                return false;

            return AuthorizeRemoteStopRequest1.Equals(AuthorizeRemoteStopRequest2);

        }

        #endregion

        #region Operator != (AuthorizeRemoteStopRequest1, AuthorizeRemoteStopRequest2)

        /// <summary>
        /// Compares two authorize remote stop requests for inequality.
        /// </summary>
        /// <param name="AuthorizeRemoteStopRequest1">An authorize remote stop request.</param>
        /// <param name="AuthorizeRemoteStopRequest2">Another authorize remote stop request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthorizeRemoteStopRequest AuthorizeRemoteStopRequest1,
                                           AuthorizeRemoteStopRequest AuthorizeRemoteStopRequest2)

            => !(AuthorizeRemoteStopRequest1 == AuthorizeRemoteStopRequest2);

        #endregion

        #endregion

        #region IEquatable<AuthorizeRemoteStopRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is AuthorizeRemoteStopRequest authorizeRemoteStopRequest &&
                   Equals(authorizeRemoteStopRequest);

        #endregion

        #region Equals(AuthorizeRemoteStopRequest)

        /// <summary>
        /// Compares two authorize remote stop requests for equality.
        /// </summary>
        /// <param name="AuthorizeRemoteStopRequest">An authorize remote stop request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AuthorizeRemoteStopRequest? AuthorizeRemoteStopRequest)

            => AuthorizeRemoteStopRequest is not null &&

               ProviderId.    Equals(AuthorizeRemoteStopRequest.ProviderId)     &&
               EVSEId.        Equals(AuthorizeRemoteStopRequest.EVSEId)         &&
               SessionId.     Equals(AuthorizeRemoteStopRequest.SessionId)      &&

               ((!CPOPartnerSessionId.HasValue && !AuthorizeRemoteStopRequest.CPOPartnerSessionId.HasValue) ||
                 (CPOPartnerSessionId.HasValue &&  AuthorizeRemoteStopRequest.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(AuthorizeRemoteStopRequest.CPOPartnerSessionId.Value))) &&

               ((!EMPPartnerSessionId.HasValue && !AuthorizeRemoteStopRequest.EMPPartnerSessionId.HasValue) ||
                 (EMPPartnerSessionId.HasValue &&  AuthorizeRemoteStopRequest.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(AuthorizeRemoteStopRequest.EMPPartnerSessionId.Value)));

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

                return ProviderId.          GetHashCode()       * 11 ^
                       EVSEId.              GetHashCode()       *  7 ^
                       SessionId.           GetHashCode()       *  5 ^

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

            => String.Concat(EVSEId,
                             " for ", SessionId,
                             " (", ProviderId, ")");

        #endregion

    }

}
