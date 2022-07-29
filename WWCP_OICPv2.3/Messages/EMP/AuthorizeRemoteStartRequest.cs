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
    /// The AuthorizeRemoteStart request.
    /// </summary>
    public class AuthorizeRemoteStartRequest : ARequest<AuthorizeRemoteStartRequest>
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
        /// The user or contract identification.
        /// </summary>
        [Mandatory]
        public Identification         Identification         { get; }

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

        /// <summary>
        /// An optional partner product identification.
        /// </summary>
        [Optional]
        public PartnerProduct_Id?     PartnerProductId       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new AuthorizeRemoteStart request.
        /// </summary>
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="Identification">An user or contract identification.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public AuthorizeRemoteStartRequest(Provider_Id            ProviderId,
                                           EVSE_Id                EVSEId,
                                           Identification         Identification,
                                           Session_Id?            SessionId             = null,
                                           CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                           EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                           PartnerProduct_Id?     PartnerProductId      = null,
                                           JObject?               CustomData            = null,

                                           DateTime?              Timestamp             = null,
                                           CancellationToken?     CancellationToken     = null,
                                           EventTracking_Id?      EventTrackingId       = null,
                                           TimeSpan?              RequestTimeout        = null)

            : base(CustomData,
                   Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.ProviderId           = ProviderId;
            this.EVSEId               = EVSEId;
            this.Identification       = Identification;
            this.SessionId            = SessionId;
            this.CPOPartnerSessionId  = CPOPartnerSessionId;
            this.EMPPartnerSessionId  = EMPPartnerSessionId;
            this.PartnerProductId     = PartnerProductId;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#eRoamingAuthorizeRemoteStart

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
        //   "SessionID":           "string"
        // }

        #endregion

        #region (static) Parse   (JSON, CustomAuthorizeRemoteStartRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of an AuthorizeRemoteStart request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ProviderIdURL">The provider identification given in the URL of the HTTP request.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomAuthorizeRemoteStartRequestParser">A delegate to parse custom AuthorizeRemoteStart JSON objects.</param>
        public static AuthorizeRemoteStartRequest Parse(JObject                                                    JSON,
                                                        Provider_Id                                                ProviderIdURL,
                                                        TimeSpan                                                   RequestTimeout,
                                                        DateTime?                                                  Timestamp                                 = null,
                                                        EventTracking_Id?                                          EventTrackingId                           = null,
                                                        CustomJObjectParserDelegate<AuthorizeRemoteStartRequest>?  CustomAuthorizeRemoteStartRequestParser   = null)
        {

            if (TryParse(JSON,
                         ProviderIdURL,
                         RequestTimeout,
                         out AuthorizeRemoteStartRequest?  authorizeRemoteStartRequest,
                         out String?                       errorResponse,
                         Timestamp,
                         EventTrackingId,
                         CustomAuthorizeRemoteStartRequestParser))
            {
                return authorizeRemoteStartRequest!;
            }

            throw new ArgumentException("The given JSON representation of an AuthorizeRemoteStart request is invalid: " + errorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomAuthorizeRemoteStartRequestParser = null)

        /// <summary>
        /// Parse the given text representation of an AuthorizeRemoteStart request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="ProviderIdURL">The provider identification given in the URL of the HTTP request.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomAuthorizeRemoteStartRequestParser">A delegate to parse custom AuthorizeRemoteStart request JSON objects.</param>
        public static AuthorizeRemoteStartRequest Parse(String                                                     Text,
                                                        Provider_Id                                                ProviderIdURL,
                                                        TimeSpan                                                   RequestTimeout,
                                                        DateTime?                                                  Timestamp                                 = null,
                                                        EventTracking_Id?                                          EventTrackingId                           = null,
                                                        CustomJObjectParserDelegate<AuthorizeRemoteStartRequest>?  CustomAuthorizeRemoteStartRequestParser   = null)
        {

            if (TryParse(Text,
                         ProviderIdURL,
                         RequestTimeout,
                         out AuthorizeRemoteStartRequest?  authorizeRemoteStartRequest,
                         out String?                       errorResponse,
                         Timestamp,
                         EventTrackingId,
                         CustomAuthorizeRemoteStartRequestParser))
            {
                return authorizeRemoteStartRequest!;
            }

            throw new ArgumentException("The given text representation of an AuthorizeRemoteStart request is invalid: " + errorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out AuthorizeRemoteStartRequest, out ErrorResponse, CustomAuthorizeRemoteStartRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of an AuthorizeRemoteStart request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ProviderIdURL">The provider identification given in the URL of the HTTP request.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="AuthorizeRemoteStartRequest">The parsed AuthorizeRemoteStart request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomAuthorizeRemoteStartRequestParser">A delegate to parse custom AuthorizeRemoteStart request JSON objects.</param>
        public static Boolean TryParse(JObject                                                    JSON,
                                       Provider_Id                                                ProviderIdURL,
                                       TimeSpan                                                   RequestTimeout,
                                       out AuthorizeRemoteStartRequest?                           AuthorizeRemoteStartRequest,
                                       out String?                                                ErrorResponse,
                                       DateTime?                                                  Timestamp                                 = null,
                                       EventTracking_Id?                                          EventTrackingId                           = null,
                                       CustomJObjectParserDelegate<AuthorizeRemoteStartRequest>?  CustomAuthorizeRemoteStartRequestParser   = null)
        {

            try
            {

                AuthorizeRemoteStartRequest = default;

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

                #region Parse Identification            [mandatory]

                if (!JSON.ParseMandatoryJSON2("Identification",
                                              "identification",
                                              OICPv2_3.Identification.TryParse,
                                              out Identification Identification,
                                              out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse SessionId                 [optional]

                if (JSON.ParseOptional("SessionID",
                                       "charging session identification",
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

                #region Parse CustomData                [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                AuthorizeRemoteStartRequest = new AuthorizeRemoteStartRequest(ProviderId,
                                                                              EVSEId,
                                                                              Identification,
                                                                              SessionId,
                                                                              CPOPartnerSessionId,
                                                                              EMPPartnerSessionId,
                                                                              PartnerProductId,
                                                                              CustomData,

                                                                              Timestamp,
                                                                              null,
                                                                              EventTrackingId,
                                                                              RequestTimeout);

                if (CustomAuthorizeRemoteStartRequestParser is not null)
                    AuthorizeRemoteStartRequest = CustomAuthorizeRemoteStartRequestParser(JSON,
                                                                                          AuthorizeRemoteStartRequest);

                return true;

            }
            catch (Exception e)
            {
                AuthorizeRemoteStartRequest  = default;
                ErrorResponse                = "The given JSON representation of an AuthorizeRemoteStart request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out AuthorizeRemoteStartRequest, out ErrorResponse, CustomAuthorizeRemoteStartRequestParser = null)

        /// <summary>
        /// Try to parse the given text representation of an AuthorizeRemoteStart request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="ProviderIdURL">The provider identification given in the URL of the HTTP request.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="AuthorizeRemoteStartRequest">The parsed AuthorizeRemoteStart request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomAuthorizeRemoteStartRequestParser">A delegate to parse custom AuthorizeRemoteStart request JSON objects.</param>
        public static Boolean TryParse(String                                                     Text,
                                       Provider_Id                                                ProviderIdURL,
                                       TimeSpan                                                   RequestTimeout,
                                       out AuthorizeRemoteStartRequest?                           AuthorizeRemoteStartRequest,
                                       out String?                                                ErrorResponse,
                                       DateTime?                                                  Timestamp                                 = null,
                                       EventTracking_Id?                                          EventTrackingId                           = null,
                                       CustomJObjectParserDelegate<AuthorizeRemoteStartRequest>?  CustomAuthorizeRemoteStartRequestParser   = null)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                ProviderIdURL,
                                RequestTimeout,
                                out AuthorizeRemoteStartRequest,
                                out ErrorResponse,
                                Timestamp,
                                EventTrackingId,
                                CustomAuthorizeRemoteStartRequestParser);

            }
            catch (Exception e)
            {
                AuthorizeRemoteStartRequest  = default;
                ErrorResponse                = "The given text representation of an AuthorizeRemoteStart request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomAuthorizeRemoteStartRequestSerializer = null, CustomIdentificationSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizeRemoteStartRequestSerializer">A delegate to customize the serialization of AuthorizeRemoteStartRequest responses.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<AuthorizeRemoteStartRequest>?  CustomAuthorizeRemoteStartRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<Identification>?               CustomIdentificationSerializer                = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("ProviderID",                  ProviderId.               ToString()),
                           new JProperty("EvseID",                      EVSEId.                   ToString()),
                           new JProperty("Identification",              Identification.           ToJSON(CustomIdentificationSerializer)),

                           SessionId.HasValue
                               ? new JProperty("SessionID",             SessionId.          Value.ToString())
                               : null,

                           CPOPartnerSessionId.HasValue
                               ? new JProperty("CPOPartnerSessionID",   CPOPartnerSessionId.Value.ToString())
                               : null,

                           EMPPartnerSessionId.HasValue
                               ? new JProperty("EMPPartnerSessionID",   EMPPartnerSessionId.Value.ToString())
                               : null,

                           PartnerProductId.HasValue
                               ? new JProperty("PartnerProductID",      PartnerProductId.   Value.ToString())
                               : null,

                           CustomData is not null
                               ? new JProperty("CustomData",            CustomData)
                               : null

                       );

            return CustomAuthorizeRemoteStartRequestSerializer is not null
                       ? CustomAuthorizeRemoteStartRequestSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthorizeRemoteStartRequest1, AuthorizeRemoteStartRequest2)

        /// <summary>
        /// Compares two authorize remote start requests for equality.
        /// </summary>
        /// <param name="AuthorizeRemoteStartRequest1">An authorize remote start request.</param>
        /// <param name="AuthorizeRemoteStartRequest2">Another authorize remote start request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthorizeRemoteStartRequest AuthorizeRemoteStartRequest1,
                                           AuthorizeRemoteStartRequest AuthorizeRemoteStartRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(AuthorizeRemoteStartRequest1, AuthorizeRemoteStartRequest2))
                return true;

            // If one is null, but not both, return false.
            if (AuthorizeRemoteStartRequest1 is null || AuthorizeRemoteStartRequest2 is null)
                return false;

            return AuthorizeRemoteStartRequest1.Equals(AuthorizeRemoteStartRequest2);

        }

        #endregion

        #region Operator != (AuthorizeRemoteStartRequest1, AuthorizeRemoteStartRequest2)

        /// <summary>
        /// Compares two authorize remote start requests for inequality.
        /// </summary>
        /// <param name="AuthorizeRemoteStartRequest1">An authorize remote start request.</param>
        /// <param name="AuthorizeRemoteStartRequest2">Another authorize remote start request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthorizeRemoteStartRequest AuthorizeRemoteStartRequest1,
                                           AuthorizeRemoteStartRequest AuthorizeRemoteStartRequest2)

            => !(AuthorizeRemoteStartRequest1 == AuthorizeRemoteStartRequest2);

        #endregion

        #endregion

        #region IEquatable<AuthorizeRemoteStartRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is AuthorizeRemoteStartRequest authorizeRemoteStartRequest &&
                   Equals(authorizeRemoteStartRequest);

        #endregion

        #region Equals(AuthorizeRemoteStartRequest)

        /// <summary>
        /// Compares two authorize remote start requests for equality.
        /// </summary>
        /// <param name="AuthorizeRemoteStartRequest">An authorize remote start request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AuthorizeRemoteStartRequest? AuthorizeRemoteStartRequest)

            => AuthorizeRemoteStartRequest is not null &&

               ProviderId.    Equals(AuthorizeRemoteStartRequest.ProviderId)     &&
               EVSEId.        Equals(AuthorizeRemoteStartRequest.EVSEId)         &&
               Identification.Equals(AuthorizeRemoteStartRequest.Identification) &&

               ((!SessionId.          HasValue && !AuthorizeRemoteStartRequest.SessionId.          HasValue) ||
                 (SessionId.          HasValue &&  AuthorizeRemoteStartRequest.SessionId.          HasValue && SessionId.          Value.Equals(AuthorizeRemoteStartRequest.SessionId.          Value))) &&

               ((!CPOPartnerSessionId.HasValue && !AuthorizeRemoteStartRequest.CPOPartnerSessionId.HasValue) ||
                 (CPOPartnerSessionId.HasValue &&  AuthorizeRemoteStartRequest.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(AuthorizeRemoteStartRequest.CPOPartnerSessionId.Value))) &&

               ((!EMPPartnerSessionId.HasValue && !AuthorizeRemoteStartRequest.EMPPartnerSessionId.HasValue) ||
                 (EMPPartnerSessionId.HasValue &&  AuthorizeRemoteStartRequest.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(AuthorizeRemoteStartRequest.EMPPartnerSessionId.Value))) &&

               ((!PartnerProductId.   HasValue && !AuthorizeRemoteStartRequest.PartnerProductId.   HasValue) ||
                 (PartnerProductId.   HasValue &&  AuthorizeRemoteStartRequest.PartnerProductId.   HasValue && PartnerProductId.   Value.Equals(AuthorizeRemoteStartRequest.PartnerProductId.   Value)));

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

                return ProviderId.          GetHashCode()       * 17 ^
                       EVSEId.              GetHashCode()       * 13 ^
                       Identification.      GetHashCode()       * 11 ^

                      (SessionId?.          GetHashCode() ?? 0) *  7 ^
                      (CPOPartnerSessionId?.GetHashCode() ?? 0) *  5 ^
                      (EMPPartnerSessionId?.GetHashCode() ?? 0) *  3 ^
                      (PartnerProductId?.   GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVSEId,
                             " for ", Identification,
                             " (", ProviderId, ")");

        #endregion

    }

}
