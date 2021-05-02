/*
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

using Newtonsoft.Json.Linq;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// An acknowledgement.
    /// </summary>
    public class Acknowledgement

    {

        #region Properties

        /// <summary>
        /// The timestamp of the request.
        /// </summary>
        [Mandatory]
        public DateTime               RequestTimestamp       { get; }

        /// <summary>
        /// The timestamp of the response creation.
        /// </summary>
        [Mandatory]
        public DateTime               ResponseTimestamp      { get; }

        /// <summary>
        /// An optional event tracking identification for correlating this response with other events.
        /// </summary>
        [Mandatory]
        public EventTracking_Id       EventTrackingId        { get; }

        /// <summary>
        /// The runtime of the request/response.
        /// </summary>
        [Mandatory]
        public TimeSpan               Runtime                { get; }

        /// <summary>
        /// The status code of the operation.
        /// </summary>
        [Mandatory]
        public StatusCode             StatusCode             { get; }

        /// <summary>
        /// The HTTP response.
        /// </summary>
        [Optional]
        public HTTPResponse           HTTPResponse           { get; }

        /// <summary>
        /// Whether the respective operation was performed or not performed successfully.
        /// </summary>
        public Boolean?               Result                 { get; }

        /// <summary>
        /// An optional charging session identification for
        /// RemoteReservationStart and RemoteStart requests.
        /// </summary>
        public Session_Id?            SessionId              { get; }

        /// <summary>
        /// An optional EMP partner charging session identification for
        /// RemoteReservationStart and RemoteStart requests.
        /// </summary>
        public CPOPartnerSession_Id?  CPOPartnerSessionId    { get; }

        /// <summary>
        /// An optional CPO partner charging session identification for
        /// RemoteReservationStart and RemoteStart requests.
        /// </summary>
        public EMPPartnerSession_Id?  EMPPartnerSessionId    { get; }

        /// <summary>
        /// The optional Hubject process identification of the request.
        /// </summary>
        [Optional]
        public Process_Id?            ProcessId              { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject                CustomData             { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="StatusCode">The status code of the operation.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="Result">Whether the respective operation was performed or not performed successfully.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public Acknowledgement(DateTime               RequestTimestamp,
                               DateTime               ResponseTimestamp,
                               EventTracking_Id       EventTrackingId,
                               TimeSpan               Runtime,
                               StatusCode             StatusCode,
                               HTTPResponse           HTTPResponse          = null,
                               Boolean?               Result                = null,
                               Session_Id?            SessionId             = null,
                               CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                               EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                               Process_Id?            ProcessId             = null,
                               JObject                CustomData            = null)
        {

            this.RequestTimestamp     = RequestTimestamp;
            this.ResponseTimestamp    = ResponseTimestamp;
            this.EventTrackingId      = EventTrackingId;
            this.Runtime              = Runtime;
            this.StatusCode           = StatusCode;

            this.HTTPResponse         = HTTPResponse;
            this.Result               = Result;
            this.SessionId            = SessionId;
            this.CPOPartnerSessionId  = CPOPartnerSessionId;
            this.EMPPartnerSessionId  = EMPPartnerSessionId;
            this.ProcessId            = ProcessId;
            this.CustomData           = CustomData;

        }

        #endregion


        #region (static) DataError                  (StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new 'DataError' acknowledgement.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        public static Acknowledgement

            DataError(String                 StatusCodeDescription      = null,
                      String                 StatusCodeAdditionalInfo   = null,
                      Session_Id?            SessionId                  = null,
                      CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                      EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                      HTTPResponse           HTTPResponse               = null,
                      DateTime?              RequestTimestamp           = null,
                      DateTime?              ResponseTimestamp          = null,
                      EventTracking_Id       EventTrackingId            = null,
                      TimeSpan?              Runtime                    = null)

                => new Acknowledgement(RequestTimestamp  ?? DateTime.UtcNow,
                                       ResponseTimestamp ?? DateTime.UtcNow,
                                       EventTrackingId   ?? EventTracking_Id.New,
                                       Runtime           ?? (DateTime.UtcNow - (RequestTimestamp ?? DateTime.UtcNow)),
                                       new StatusCode(
                                           StatusCodes.DataError,
                                           StatusCodeDescription ?? "Data Error!",
                                           StatusCodeAdditionalInfo
                                       ),
                                       HTTPResponse,
                                       false,
                                       SessionId,
                                       CPOPartnerSessionId,
                                       EMPPartnerSessionId);

        #endregion

        #region (static) SystemError                (StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new 'SystemError' acknowledgement.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        public static Acknowledgement

            SystemError(String                 StatusCodeDescription      = null,
                        String                 StatusCodeAdditionalInfo   = null,
                        Session_Id?            SessionId                  = null,
                        CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                        EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                        HTTPResponse           HTTPResponse               = null,
                        DateTime?              RequestTimestamp           = null,
                        DateTime?              ResponseTimestamp          = null,
                        EventTracking_Id       EventTrackingId            = null,
                        TimeSpan?              Runtime                    = null)

                => new Acknowledgement(RequestTimestamp  ?? DateTime.UtcNow,
                                       ResponseTimestamp ?? DateTime.UtcNow,
                                       EventTrackingId   ?? EventTracking_Id.New,
                                       Runtime           ?? (DateTime.UtcNow - (RequestTimestamp ?? DateTime.UtcNow)),
                                       new StatusCode(
                                           StatusCodes.SystemError,
                                           StatusCodeDescription ?? "System Error!",
                                           StatusCodeAdditionalInfo
                                       ),
                                       HTTPResponse,
                                       false,
                                       SessionId,
                                       CPOPartnerSessionId,
                                       EMPPartnerSessionId);

        #endregion

        #region (static) ServiceNotAvailable        (StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new 'ServiceNotAvailable' acknowledgement.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        public static Acknowledgement

            ServiceNotAvailable(String                 StatusCodeDescription      = null,
                                String                 StatusCodeAdditionalInfo   = null,
                                Session_Id?            SessionId                  = null,
                                CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                HTTPResponse           HTTPResponse               = null,
                                DateTime?              RequestTimestamp           = null,
                                DateTime?              ResponseTimestamp          = null,
                                EventTracking_Id       EventTrackingId            = null,
                                TimeSpan?              Runtime                    = null)

                => new Acknowledgement(RequestTimestamp  ?? DateTime.UtcNow,
                                       ResponseTimestamp ?? DateTime.UtcNow,
                                       EventTrackingId   ?? EventTracking_Id.New,
                                       Runtime           ?? (DateTime.UtcNow - (RequestTimestamp ?? DateTime.UtcNow)),
                                       new StatusCode(
                                           StatusCodes.ServiceNotAvailable,
                                           StatusCodeDescription ?? "Service not available!",
                                           StatusCodeAdditionalInfo
                                       ),
                                       HTTPResponse,
                                       false,
                                       SessionId,
                                       CPOPartnerSessionId,
                                       EMPPartnerSessionId);

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#7-eroamingacknowledgment

        // {
        //   "CPOPartnerSessionID":  "string",
        //   "EMPPartnerSessionID":  "string",
        //   "Result":                false,
        //   "SessionID":            "string",
        //   "StatusCode": {
        //     "AdditionalInfo":     "string",
        //     "Code":               "000",
        //     "Description":        "string"
        //   }
        // }

        // HTTP/1.1 200 
        // Server: nginx/1.18.0
        // Date: Sat, 09 Jan 2021 04:31:25 GMT
        // Content-Type: application/json;charset=utf-8
        // Transfer-Encoding: chunked
        // Connection: keep-alive
        // Process-ID: dc83fb59-4dad-430e-9f1f-84f5c9a042c6
        // 
        // {"Result":true,"StatusCode":{"Code":"000","Description":null,"AdditionalInfo":null},"SessionID":null,"CPOPartnerSessionID":null,"EMPPartnerSessionID":null}
        // {"StatusCode":{"Code":"001","Description":"OICP service not found for URI: /api/oicp/evsepush/v23/operators/DE*BDO/status-records","AdditionalInfo":null}}
        // {"Result":false,"StatusCode":{"Code":"018","Description":"Duplicate EVSE IDs","AdditionalInfo":null},"SessionID":null,"CPOPartnerSessionID":null,"EMPPartnerSessionID":null}

        #endregion

        #region (static) Parse   (JSON, CustomAcknowledgementParser = null)

        /// <summary>
        /// Parse the given JSON representation of an acknowledgement.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="CustomAcknowledgementParser">A delegate to parse custom acknowledgement JSON objects.</param>
        public static Acknowledgement Parse(JObject                                       JSON,
                                            HTTPResponse                                  HTTPResponse                  = null,
                                            DateTime?                                     RequestTimestamp              = null,
                                            DateTime?                                     ResponseTimestamp             = null,
                                            EventTracking_Id                              EventTrackingId               = null,
                                            TimeSpan?                                     Runtime                       = null,
                                            Process_Id?                                   ProcessId                     = null,
                                            CustomJObjectParserDelegate<Acknowledgement>  CustomAcknowledgementParser   = null)
        {

            if (TryParse(JSON,
                         out Acknowledgement acknowledgement,
                         out String          ErrorResponse,
                         HTTPResponse,
                         RequestTimestamp,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         ProcessId,
                         CustomAcknowledgementParser))
            {
                return acknowledgement;
            }

            throw new ArgumentException("The given JSON representation of an acknowledgement is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomAcknowledgementParser = null)

        /// <summary>
        /// Parse the given text representation of an acknowledgement.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="CustomAcknowledgementParser">A delegate to parse custom acknowledgement JSON objects.</param>
        public static Acknowledgement Parse(String                                        Text,
                                            HTTPResponse                                  HTTPResponse                  = null,
                                            DateTime?                                     RequestTimestamp              = null,
                                            DateTime?                                     ResponseTimestamp             = null,
                                            EventTracking_Id                              EventTrackingId               = null,
                                            TimeSpan?                                     Runtime                       = null,
                                            Process_Id?                                   ProcessId                     = null,
                                            CustomJObjectParserDelegate<Acknowledgement>  CustomAcknowledgementParser   = null)
        {

            if (TryParse(Text,
                         out Acknowledgement acknowledgement,
                         out String          ErrorResponse,
                         HTTPResponse,
                         RequestTimestamp,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         ProcessId,
                         CustomAcknowledgementParser))
            {
                return acknowledgement;
            }

            throw new ArgumentException("The given text representation of an acknowledgement is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out Acknowledgement, out ErrorResponse, CustomAcknowledgementParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of an acknowledgement.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="Acknowledgement">The parsed acknowledgement.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="CustomAcknowledgementParser">A delegate to parse custom acknowledgement JSON objects.</param>
        public static Boolean TryParse(JObject                                       JSON,
                                       out Acknowledgement                           Acknowledgement,
                                       out String                                    ErrorResponse,
                                       HTTPResponse                                  HTTPResponse                  = null,
                                       DateTime?                                     RequestTimestamp              = null,
                                       DateTime?                                     ResponseTimestamp             = null,
                                       EventTracking_Id                              EventTrackingId               = null,
                                       TimeSpan?                                     Runtime                       = null,
                                       Process_Id?                                   ProcessId                     = null,
                                       CustomJObjectParserDelegate<Acknowledgement>  CustomAcknowledgementParser   = null)
        {

            try
            {

                Acknowledgement = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse StatusCode              [mandatory]

                if (!JSON.ParseMandatoryJSON2("StatusCode",
                                              "StatusCode",
                                              OICPv2_3.StatusCode.TryParse,
                                              out StatusCode StatusCode,
                                              out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Result                  [optional]

                if (JSON.ParseOptional("Result",
                                       "result",
                                       out Boolean? Result,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse SessionId               [optional]

                if (JSON.ParseOptional("SessionID",
                                       "session identification",
                                       Session_Id.TryParse,
                                       out Session_Id? SessionId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse CPOPartnerSessionId     [optional]

                if (JSON.ParseOptional("CPOPartnerSessionID",
                                       "CPO partner session identification",
                                       CPOPartnerSession_Id.TryParse,
                                       out CPOPartnerSession_Id? CPOPartnerSessionId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse EMPPartnerSessionId     [optional]

                if (JSON.ParseOptional("EMPPartnerSessionID",
                                       "EMP partner session identification",
                                       EMPPartnerSession_Id.TryParse,
                                       out EMPPartnerSession_Id? EMPPartnerSessionId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse CustomData              [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                Acknowledgement = new Acknowledgement(RequestTimestamp  ?? DateTime.UtcNow,
                                                      ResponseTimestamp ?? DateTime.UtcNow,
                                                      EventTrackingId   ?? EventTracking_Id.New,
                                                      Runtime           ?? DateTime.UtcNow - (RequestTimestamp ?? DateTime.UtcNow),
                                                      StatusCode,
                                                      HTTPResponse,
                                                      Result,
                                                      SessionId,
                                                      CPOPartnerSessionId,
                                                      EMPPartnerSessionId,
                                                      ProcessId,
                                                      CustomData);

                if (CustomAcknowledgementParser != null)
                    Acknowledgement = CustomAcknowledgementParser(JSON,
                                                                  Acknowledgement);

                return true;

            }
            catch (Exception e)
            {
                Acknowledgement  = default;
                ErrorResponse    = "The given JSON representation of an acknowledgement is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out Acknowledgement, out ErrorResponse, CustomAcknowledgementParser = null)

        /// <summary>
        /// Try to parse the given text representation of an acknowledgement.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="Acknowledgement">The parsed acknowledgement.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="CustomAcknowledgementParser">A delegate to parse custom acknowledgement JSON objects.</param>
        public static Boolean TryParse(String                                        Text,
                                       out Acknowledgement                           Acknowledgement,
                                       out String                                    ErrorResponse,
                                       HTTPResponse                                  HTTPResponse                  = null,
                                       DateTime?                                     RequestTimestamp              = null,
                                       DateTime?                                     ResponseTimestamp             = null,
                                       EventTracking_Id                              EventTrackingId               = null,
                                       TimeSpan?                                     Runtime                       = null,
                                       Process_Id?                                   ProcessId                     = null,
                                       CustomJObjectParserDelegate<Acknowledgement>  CustomAcknowledgementParser   = null)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out Acknowledgement,
                                out ErrorResponse,
                                HTTPResponse,
                                RequestTimestamp,
                                ResponseTimestamp,
                                EventTrackingId,
                                Runtime,
                                ProcessId,
                                CustomAcknowledgementParser);

            }
            catch (Exception e)
            {
                Acknowledgement  = default;
                ErrorResponse    = "The given text representation of an acknowledgement is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomAcknowledgementSerializer = null, CustomStatusCodeSerializer = null)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomAcknowledgementSerializer">A delegate to customize the serialization of Acknowledgement responses.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<Acknowledgement>  CustomAcknowledgementSerializer   = null,
                              CustomJObjectSerializerDelegate<StatusCode>       CustomStatusCodeSerializer        = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("StatusCode", StatusCode.ToJSON(CustomStatusCodeSerializer: CustomStatusCodeSerializer)),

                           Result.             HasValue
                               ? new JProperty("Result",                Result.             Value)
                               : null,

                           SessionId.          HasValue
                               ? new JProperty("SessionID",             SessionId.          Value.ToString())
                               : null,

                           CPOPartnerSessionId.HasValue
                               ? new JProperty("CPOPartnerSessionID",   CPOPartnerSessionId.Value.ToString())
                               : null,

                           EMPPartnerSessionId.HasValue
                               ? new JProperty("EMPPartnerSessionID",   EMPPartnerSessionId.Value.ToString())
                               : null

                       );

            return CustomAcknowledgementSerializer != null
                       ? CustomAcknowledgementSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (Acknowledgement1, Acknowledgement2)

        /// <summary>
        /// Compares two acknowledgements for equality.
        /// </summary>
        /// <param name="Acknowledgement1">An acknowledgement.</param>
        /// <param name="Acknowledgement2">Another acknowledgement.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Acknowledgement Acknowledgement1,
                                           Acknowledgement Acknowledgement2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(Acknowledgement1, Acknowledgement2))
                return true;

            // If one is null, but not both, return false.
            if (Acknowledgement1 is null || Acknowledgement2 is null)
                return false;

            return Acknowledgement1.Equals(Acknowledgement2);

        }

        #endregion

        #region Operator != (Acknowledgement1, Acknowledgement2)

        /// <summary>
        /// Compares two acknowledgements for inequality.
        /// </summary>
        /// <param name="Acknowledgement1">An acknowledgement.</param>
        /// <param name="Acknowledgement2">Another acknowledgement.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Acknowledgement Acknowledgement1,
                                           Acknowledgement Acknowledgement2)

            => !(Acknowledgement1 == Acknowledgement2);

        #endregion

        #endregion

        #region IEquatable<Acknowledgement> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is Acknowledgement acknowledgement &&
                   Equals(acknowledgement);

        #endregion

        #region Equals(Acknowledgement)

        /// <summary>
        /// Compares two acknowledgements for equality.
        /// </summary>
        /// <param name="Acknowledgement">An acknowledgement to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Acknowledgement Acknowledgement)

            => !(Acknowledgement is null) &&

                 StatusCode.Equals(Acknowledgement.StatusCode) &&

              ((!Result.             HasValue && !Acknowledgement.Result.             HasValue) ||
                (Result.             HasValue &&  Acknowledgement.Result.             HasValue && Result.             Value.Equals(Acknowledgement.Result.             Value))) &&

              ((!SessionId.          HasValue && !Acknowledgement.SessionId.          HasValue) ||
                (SessionId.          HasValue &&  Acknowledgement.SessionId.          HasValue && SessionId.          Value.Equals(Acknowledgement.SessionId.          Value))) &&

              ((!CPOPartnerSessionId.HasValue && !Acknowledgement.CPOPartnerSessionId.HasValue) ||
                (CPOPartnerSessionId.HasValue &&  Acknowledgement.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(Acknowledgement.CPOPartnerSessionId.Value))) &&

              ((!EMPPartnerSessionId.HasValue && !Acknowledgement.EMPPartnerSessionId.HasValue) ||
                (EMPPartnerSessionId.HasValue &&  Acknowledgement.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(Acknowledgement.EMPPartnerSessionId.Value)));

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return StatusCode.          GetHashCode()       * 11 ^
                      (Result?.             GetHashCode() ?? 0) *  7 ^
                      (SessionId?.          GetHashCode() ?? 0) *  5 ^
                      (CPOPartnerSessionId?.GetHashCode() ?? 0) *  3 ^
                      (EMPPartnerSessionId?.GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(StatusCode.Code.ToString(), " => ",
                             new String[] {
                                 Result.             HasValue                 ? "result: "       + Result.             Value.ToString() : null,
                                 StatusCode.Description.   IsNotNullOrEmpty() ? "description: "  + StatusCode.Description               : null,
                                 StatusCode.AdditionalInfo.IsNotNullOrEmpty() ? "info: "         + StatusCode.AdditionalInfo            : null,
                                 SessionId.          HasValue                 ? "sessionId: "    + SessionId.          Value.ToString() : null,
                                 CPOPartnerSessionId.HasValue                 ? "CPOSessionId: " + CPOPartnerSessionId.Value.ToString() : null,
                                 EMPPartnerSessionId.HasValue                 ? "EMPSessionId: " + EMPPartnerSessionId.Value.ToString() : null,
                                 ProcessId.          HasValue                 ? "processId: "    + ProcessId.          Value.ToString() : null
                             }.AggregateWith(", "));

        #endregion


        #region ToBuilder()

        /// <summary>
        /// Return an acknowledgement builder.
        /// </summary>
        public Builder ToBuilder()

            => new Builder(RequestTimestamp,
                           ResponseTimestamp,
                           EventTrackingId,
                           Runtime,
                           StatusCode,
                           HTTPResponse,
                           Result,
                           SessionId,
                           CPOPartnerSessionId,
                           EMPPartnerSessionId,
                           ProcessId,
                           CustomData);

        #endregion

        #region (class) Builder

        /// <summary>
        /// An acknowledgement builder.
        /// </summary>
        public class Builder
        {

            #region Properties

            /// <summary>
            /// The timestamp of the request.
            /// </summary>
            [Mandatory]
            public DateTime?              RequestTimestamp       { get; set; }

            /// <summary>
            /// The timestamp of the response creation.
            /// </summary>
            [Mandatory]
            public DateTime?              ResponseTimestamp      { get; set; }

            /// <summary>
            /// An optional event tracking identification for correlating this response with other events.
            /// </summary>
            [Mandatory]
            public EventTracking_Id?      EventTrackingId        { get; set; }

            /// <summary>
            /// The runtime of the request/response.
            /// </summary>
            [Mandatory]
            public TimeSpan?              Runtime                { get; set; }

            /// <summary>
            /// The status code of the operation.
            /// </summary>
            [Mandatory]
            public StatusCode.Builder     StatusCode             { get; }

            /// <summary>
            /// The HTTP response.
            /// </summary>
            [Optional]
            public HTTPResponse           HTTPResponse           { get; set; }

            /// <summary>
            /// Whether the respective operation was performed or not performed successfully.
            /// </summary>
            public Boolean?               Result                 { get; set; }

            /// <summary>
            /// An optional charging session identification for
            /// RemoteReservationStart and RemoteStart requests.
            /// </summary>
            public Session_Id?            SessionId              { get; set; }

            /// <summary>
            /// An optional EMP partner charging session identification for
            /// RemoteReservationStart and RemoteStart requests.
            /// </summary>
            public CPOPartnerSession_Id?  CPOPartnerSessionId    { get; set; }

            /// <summary>
            /// An optional CPO partner charging session identification for
            /// RemoteReservationStart and RemoteStart requests.
            /// </summary>
            public EMPPartnerSession_Id?  EMPPartnerSessionId    { get; set; }

            /// <summary>
            /// The optional Hubject process identification of the request.
            /// </summary>
            [Optional]
            public Process_Id?            ProcessId              { get; set; }

            /// <summary>
            /// Optional custom data, e.g. in combination with custom parsers and serializers.
            /// </summary>
            [Optional]
            public JObject                CustomData             { get; set; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new acknowledgement builder.
            /// </summary>
            /// <param name="RequestTimestamp">The timestamp of the request.</param>
            /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
            /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
            /// <param name="Runtime">The runtime of the request/response.</param>
            /// <param name="StatusCode">The status code of the operation.</param>
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            /// <param name="Result">The result of the operation.</param>
            /// <param name="SessionId">An optional charging session identification.</param>
            /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
            /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(DateTime?              RequestTimestamp      = null,
                           DateTime?              ResponseTimestamp     = null,
                           EventTracking_Id       EventTrackingId       = null,
                           TimeSpan?              Runtime               = null,
                           StatusCode             StatusCode            = null,
                           HTTPResponse           HTTPResponse          = null,
                           Boolean?               Result                = null,
                           Session_Id?            SessionId             = null,
                           CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                           EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                           Process_Id?            ProcessId             = null,
                           JObject                CustomData            = null)
            {

                this.RequestTimestamp     = RequestTimestamp;
                this.ResponseTimestamp    = ResponseTimestamp;
                this.EventTrackingId      = EventTrackingId;
                this.Runtime              = Runtime;
                this.StatusCode           = StatusCode != null ? StatusCode.ToBuilder() : new StatusCode.Builder();

                this.HTTPResponse         = HTTPResponse;
                this.Result               = Result;
                this.SessionId            = SessionId;
                this.CPOPartnerSessionId  = CPOPartnerSessionId;
                this.EMPPartnerSessionId  = EMPPartnerSessionId;
                this.ProcessId            = ProcessId;
                this.CustomData           = CustomData;

            }

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the acknowledgement.
            /// </summary>
            /// <param name="Builder">An acknowledgement builder.</param>
            public static implicit operator Acknowledgement(Builder Builder)

                => Builder?.ToImmutable();


            /// <summary>
            /// Return an immutable version of the acknowledgement.
            /// </summary>
            public Acknowledgement ToImmutable()

                => new Acknowledgement(RequestTimestamp  ?? DateTime.UtcNow,
                                       ResponseTimestamp ?? DateTime.UtcNow,
                                       EventTrackingId   ?? EventTracking_Id.New,
                                       Runtime           ?? (DateTime.UtcNow - (RequestTimestamp ?? DateTime.UtcNow)),
                                       StatusCode?.ToImmutable(),
                                       HTTPResponse,
                                       Result,
                                       SessionId,
                                       CPOPartnerSessionId,
                                       EMPPartnerSessionId,
                                       ProcessId,
                                       CustomData);

            #endregion

        }

        #endregion

    }


    /// <summary>
    /// A generic acknowledgement.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    public class Acknowledgement<TRequest> : AResponse<TRequest,
                                                       Acknowledgement<TRequest>>

        where TRequest : class, IRequest

    {

        #region Properties

        /// <summary>
        /// The status code of the operation.
        /// </summary>
        public StatusCode             StatusCode             { get; }

        /// <summary>
        /// Whether the respective operation was performed or not performed successfully.
        /// </summary>
        public Boolean?               Result                 { get; }

        /// <summary>
        /// An optional charging session identification for
        /// RemoteReservationStart and RemoteStart requests.
        /// </summary>
        public Session_Id?            SessionId              { get; }

        /// <summary>
        /// An optional EMP partner charging session identification for
        /// RemoteReservationStart and RemoteStart requests.
        /// </summary>
        public CPOPartnerSession_Id?  CPOPartnerSessionId    { get; }

        /// <summary>
        /// An optional CPO partner charging session identification for
        /// RemoteReservationStart and RemoteStart requests.
        /// </summary>
        public EMPPartnerSession_Id?  EMPPartnerSessionId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="StatusCode">The status code of the operation.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="Result">Whether the respective operation was performed or not performed successfully.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public Acknowledgement(TRequest               Request,
                               DateTime               ResponseTimestamp,
                               EventTracking_Id       EventTrackingId,
                               TimeSpan               Runtime,
                               StatusCode             StatusCode,
                               HTTPResponse           HTTPResponse          = null,
                               Boolean?               Result                = null,
                               Session_Id?            SessionId             = null,
                               CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                               EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                               Process_Id?            ProcessId             = null,
                               JObject                CustomData            = null)

            : base(Request,
                   ResponseTimestamp,
                   EventTrackingId,
                   Runtime,
                   HTTPResponse,
                   ProcessId,
                   CustomData)

        {

            this.StatusCode           = StatusCode;
            this.Result               = Result;
            this.SessionId            = SessionId;
            this.CPOPartnerSessionId  = CPOPartnerSessionId;
            this.EMPPartnerSessionId  = EMPPartnerSessionId;

        }

        #endregion


        #region (static) Success                    (Request, SessionId = null, ...)

        /// <summary>
        /// Create a new 'positive' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        public static Acknowledgement<TRequest>

            Success(TRequest               Request,
                    Session_Id?            SessionId                  = null,
                    CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                    EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                    String                 StatusCodeDescription      = null,
                    String                 StatusCodeAdditionalInfo   = null,
                    HTTPResponse           HTTPResponse               = null,
                    DateTime?              ResponseTimestamp          = null,
                    EventTracking_Id       EventTrackingId            = null,
                    TimeSpan?              Runtime                    = null)

                => new Acknowledgement<TRequest>(Request,
                                                 ResponseTimestamp ?? DateTime.UtcNow,
                                                 EventTrackingId   ?? EventTracking_Id.New,
                                                 Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                                 new StatusCode(
                                                     StatusCodes.Success,
                                                     StatusCodeDescription ?? "Success",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 HTTPResponse,
                                                 true,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) DataError                  (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new 'DataError' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        public static Acknowledgement<TRequest>

            DataError(TRequest               Request,
                      String                 StatusCodeDescription      = null,
                      String                 StatusCodeAdditionalInfo   = null,
                      Session_Id?            SessionId                  = null,
                      CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                      EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                      HTTPResponse           HTTPResponse               = null,
                      DateTime?              ResponseTimestamp          = null,
                      EventTracking_Id       EventTrackingId            = null,
                      TimeSpan?              Runtime                    = null)

                => new Acknowledgement<TRequest>(Request,
                                                 ResponseTimestamp ?? DateTime.UtcNow,
                                                 EventTrackingId   ?? EventTracking_Id.New,
                                                 Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                                 new StatusCode(
                                                     StatusCodes.DataError,
                                                     StatusCodeDescription ?? "Data Error!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 HTTPResponse,
                                                 false,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) SystemError                (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new 'SystemError' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        public static Acknowledgement<TRequest>

            SystemError(TRequest               Request,
                        String                 StatusCodeDescription      = null,
                        String                 StatusCodeAdditionalInfo   = null,
                        Session_Id?            SessionId                  = null,
                        CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                        EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                        HTTPResponse           HTTPResponse               = null,
                        DateTime?              ResponseTimestamp          = null,
                        EventTracking_Id       EventTrackingId            = null,
                        TimeSpan?              Runtime                    = null)

                => new Acknowledgement<TRequest>(Request,
                                                 ResponseTimestamp ?? DateTime.UtcNow,
                                                 EventTrackingId   ?? EventTracking_Id.New,
                                                 Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     StatusCodeDescription ?? "System Error!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 HTTPResponse,
                                                 false,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) ServiceNotAvailable        (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new 'ServiceNotAvailable' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        public static Acknowledgement<TRequest>

            ServiceNotAvailable(TRequest               Request,
                                String                 StatusCodeDescription      = null,
                                String                 StatusCodeAdditionalInfo   = null,
                                Session_Id?            SessionId                  = null,
                                CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                HTTPResponse           HTTPResponse               = null,
                                DateTime?              ResponseTimestamp          = null,
                                EventTracking_Id       EventTrackingId            = null,
                                TimeSpan?              Runtime                    = null)

                => new Acknowledgement<TRequest>(Request,
                                                 ResponseTimestamp ?? DateTime.UtcNow,
                                                 EventTrackingId   ?? EventTracking_Id.New,
                                                 Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                                 new StatusCode(
                                                     StatusCodes.ServiceNotAvailable,
                                                     StatusCodeDescription ?? "Service not available!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 HTTPResponse,
                                                 false,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) SessionIsInvalid           (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new 'SessionIsInvalid' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        public static Acknowledgement<TRequest>

            SessionIsInvalid(TRequest               Request,
                             String                 StatusCodeDescription      = null,
                             String                 StatusCodeAdditionalInfo   = null,
                             Session_Id?            SessionId                  = null,
                             CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                             EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                             HTTPResponse           HTTPResponse               = null,
                             DateTime?              ResponseTimestamp          = null,
                             EventTracking_Id       EventTrackingId            = null,
                             TimeSpan?              Runtime                    = null)

                => new Acknowledgement<TRequest>(Request,
                                                 ResponseTimestamp ?? DateTime.UtcNow,
                                                 EventTrackingId   ?? EventTracking_Id.New,
                                                 Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                                 new StatusCode(
                                                     StatusCodes.SessionIsInvalid,
                                                     StatusCodeDescription ?? "Session is invalid!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 HTTPResponse,
                                                 false,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) CommunicationToEVSEFailed  (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new 'CommunicationToEVSEFailed' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        public static Acknowledgement<TRequest>

            CommunicationToEVSEFailed(TRequest               Request,
                                      String                 StatusCodeDescription      = null,
                                      String                 StatusCodeAdditionalInfo   = null,
                                      Session_Id?            SessionId                  = null,
                                      CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                      EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                      HTTPResponse           HTTPResponse               = null,
                                      DateTime?              ResponseTimestamp          = null,
                                      EventTracking_Id       EventTrackingId            = null,
                                      TimeSpan?              Runtime                    = null)

                => new Acknowledgement<TRequest>(Request,
                                                 ResponseTimestamp ?? DateTime.UtcNow,
                                                 EventTrackingId   ?? EventTracking_Id.New,
                                                 Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                                 new StatusCode(
                                                     StatusCodes.CommunicationToEVSEFailed,
                                                     StatusCodeDescription ?? "Communication to EVSE failed!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 HTTPResponse,
                                                 false,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) EVSEAlreadyReserved        (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new 'EVSEAlreadyReserved' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        public static Acknowledgement<TRequest>

            EVSEAlreadyReserved(TRequest               Request,
                                String                 StatusCodeDescription      = null,
                                String                 StatusCodeAdditionalInfo   = null,
                                Session_Id?            SessionId                  = null,
                                CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                HTTPResponse           HTTPResponse               = null,
                                DateTime?              ResponseTimestamp          = null,
                                EventTracking_Id       EventTrackingId            = null,
                                TimeSpan?              Runtime                    = null)

                => new Acknowledgement<TRequest>(Request,
                                                 ResponseTimestamp ?? DateTime.UtcNow,
                                                 EventTrackingId   ?? EventTracking_Id.New,
                                                 Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                                 new StatusCode(
                                                     StatusCodes.EVSEAlreadyReserved,
                                                     StatusCodeDescription ?? "EVSE already reserved!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 HTTPResponse,
                                                 false,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) EVSEAlreadyInUse_WrongToken(Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new 'EVSEAlreadyInUse_WrongToken' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        public static Acknowledgement<TRequest>

            EVSEAlreadyInUse_WrongToken(TRequest               Request,
                                        String                 StatusCodeDescription      = null,
                                        String                 StatusCodeAdditionalInfo   = null,
                                        Session_Id?            SessionId                  = null,
                                        CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                        EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                        HTTPResponse           HTTPResponse               = null,
                                        DateTime?              ResponseTimestamp          = null,
                                        EventTracking_Id       EventTrackingId            = null,
                                        TimeSpan?              Runtime                    = null)

                => new Acknowledgement<TRequest>(Request,
                                                 ResponseTimestamp ?? DateTime.UtcNow,
                                                 EventTrackingId   ?? EventTracking_Id.New,
                                                 Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                                 new StatusCode(
                                                     StatusCodes.EVSEAlreadyInUse_WrongToken,
                                                     StatusCodeDescription ?? "EVSE is already in use!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 HTTPResponse,
                                                 false,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) UnknownEVSEID              (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new 'UnknownEVSEID' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        public static Acknowledgement<TRequest>

            UnknownEVSEID(TRequest               Request,
                          String                 StatusCodeDescription      = null,
                          String                 StatusCodeAdditionalInfo   = null,
                          Session_Id?            SessionId                  = null,
                          CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                          EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                          HTTPResponse           HTTPResponse               = null,
                          DateTime?              ResponseTimestamp          = null,
                          EventTracking_Id       EventTrackingId            = null,
                          TimeSpan?              Runtime                    = null)

                => new Acknowledgement<TRequest>(Request,
                                                 ResponseTimestamp ?? DateTime.UtcNow,
                                                 EventTrackingId   ?? EventTracking_Id.New,
                                                 Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                                 new StatusCode(
                                                     StatusCodes.UnknownEVSEID,
                                                     StatusCodeDescription ?? "Unknown EVSE identification!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 HTTPResponse,
                                                 false,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) EVSEOutOfService           (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new 'EVSEOutOfService' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        public static Acknowledgement<TRequest>

            EVSEOutOfService(TRequest               Request,
                             String                 StatusCodeDescription      = null,
                             String                 StatusCodeAdditionalInfo   = null,
                             Session_Id?            SessionId                  = null,
                             CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                             EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                             HTTPResponse           HTTPResponse               = null,
                             DateTime?              ResponseTimestamp          = null,
                             EventTracking_Id       EventTrackingId            = null,
                             TimeSpan?              Runtime                    = null)

                => new Acknowledgement<TRequest>(Request,
                                                 ResponseTimestamp ?? DateTime.UtcNow,
                                                 EventTrackingId   ?? EventTracking_Id.New,
                                                 Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                                 new StatusCode(
                                                     StatusCodes.EVSEOutOfService,
                                                     StatusCodeDescription ?? "EVSE out of service!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 HTTPResponse,
                                                 false,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) NoValidContract            (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new 'NoValidContract' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        public static Acknowledgement<TRequest>

            NoValidContract(TRequest               Request,
                            String                 StatusCodeDescription      = null,
                            String                 StatusCodeAdditionalInfo   = null,
                            Session_Id?            SessionId                  = null,
                            CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                            EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                            HTTPResponse           HTTPResponse               = null,
                            DateTime?              ResponseTimestamp          = null,
                            EventTracking_Id       EventTrackingId            = null,
                            TimeSpan?              Runtime                    = null)

                => new Acknowledgement<TRequest>(Request,
                                                 ResponseTimestamp ?? DateTime.UtcNow,
                                                 EventTrackingId   ?? EventTracking_Id.New,
                                                 Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                                 new StatusCode(
                                                     StatusCodes.NoValidContract,
                                                     StatusCodeDescription ?? "No valid contract!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 HTTPResponse,
                                                 false,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) NoEVConnectedToEVSE        (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new 'NoEVConnectedToEVSE' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        public static Acknowledgement<TRequest>

            NoEVConnectedToEVSE(TRequest               Request,
                                String                 StatusCodeDescription      = null,
                                String                 StatusCodeAdditionalInfo   = null,
                                Session_Id?            SessionId                  = null,
                                CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                HTTPResponse           HTTPResponse               = null,
                                DateTime?              ResponseTimestamp          = null,
                                EventTracking_Id       EventTrackingId            = null,
                                TimeSpan?              Runtime                    = null)

                => new Acknowledgement<TRequest>(Request,
                                                 ResponseTimestamp ?? DateTime.UtcNow,
                                                 EventTrackingId   ?? EventTracking_Id.New,
                                                 Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                                 new StatusCode(
                                                     StatusCodes.NoEVConnectedToEVSE,
                                                     StatusCodeDescription ?? "No electric vehicle connected to EVSE!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 HTTPResponse,
                                                 false,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion



        public static implicit operator Acknowledgement(Acknowledgement<TRequest> acknowledgement)

                => new Acknowledgement(acknowledgement.Request.Timestamp,
                                       acknowledgement.ResponseTimestamp,
                                       acknowledgement.Request.EventTrackingId,
                                       acknowledgement.Runtime,
                                       acknowledgement.StatusCode,
                                       acknowledgement.HTTPResponse,
                                       acknowledgement.Result,
                                       acknowledgement.SessionId,
                                       acknowledgement.CPOPartnerSessionId,
                                       acknowledgement.EMPPartnerSessionId,
                                       acknowledgement.ProcessId,
                                       acknowledgement.CustomData);


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#7-eroamingacknowledgment

        // {
        //   "CPOPartnerSessionID":  "string",
        //   "EMPPartnerSessionID":  "string",
        //   "Result":                false,
        //   "SessionID":            "string",
        //   "StatusCode": {
        //     "AdditionalInfo":     "string",
        //     "Code":               "000",
        //     "Description":        "string"
        //   }
        // }

        // HTTP/1.1 200 
        // Server: nginx/1.18.0
        // Date: Sat, 09 Jan 2021 04:31:25 GMT
        // Content-Type: application/json;charset=utf-8
        // Transfer-Encoding: chunked
        // Connection: keep-alive
        // Process-ID: dc83fb59-4dad-430e-9f1f-84f5c9a042c6
        // 
        // {"Result":true,"StatusCode":{"Code":"000","Description":null,"AdditionalInfo":null},"SessionID":null,"CPOPartnerSessionID":null,"EMPPartnerSessionID":null}
        // {"StatusCode":{"Code":"001","Description":"OICP service not found for URI: /api/oicp/evsepush/v23/operators/DE*BDO/status-records","AdditionalInfo":null}}
        // {"Result":false,"StatusCode":{"Code":"018","Description":"Duplicate EVSE IDs","AdditionalInfo":null},"SessionID":null,"CPOPartnerSessionID":null,"EMPPartnerSessionID":null}

        #endregion

        #region (static) Parse   (JSON, CustomAcknowledgementParser = null)

        /// <summary>
        /// Parse the given JSON representation of an acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="CustomAcknowledgementParser">A delegate to parse custom acknowledgement JSON objects.</param>
        public static Acknowledgement<TRequest> Parse(TRequest                                                Request,
                                                      JObject                                                 JSON,
                                                      HTTPResponse                                            HTTPResponse                  = null,
                                                      DateTime?                                               ResponseTimestamp             = null,
                                                      EventTracking_Id                                        EventTrackingId               = null,
                                                      TimeSpan?                                               Runtime                       = null,
                                                      Process_Id?                                             ProcessId                     = null,
                                                      CustomJObjectParserDelegate<Acknowledgement<TRequest>>  CustomAcknowledgementParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         out Acknowledgement<TRequest> acknowledgement,
                         out String                    ErrorResponse,
                         HTTPResponse,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         ProcessId,
                         CustomAcknowledgementParser))
            {
                return acknowledgement;
            }

            throw new ArgumentException("The given JSON representation of an acknowledgement is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomAcknowledgementParser = null)

        /// <summary>
        /// Parse the given text representation of an acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="Text">The text to parse.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="CustomAcknowledgementParser">A delegate to parse custom acknowledgement JSON objects.</param>
        public static Acknowledgement<TRequest> Parse(TRequest                                                Request,
                                                      String                                                  Text,
                                                      HTTPResponse                                            HTTPResponse                  = null,
                                                      DateTime?                                               ResponseTimestamp             = null,
                                                      EventTracking_Id                                        EventTrackingId               = null,
                                                      TimeSpan?                                               Runtime                       = null,
                                                      Process_Id?                                             ProcessId                     = null,
                                                      CustomJObjectParserDelegate<Acknowledgement<TRequest>>  CustomAcknowledgementParser   = null)
        {

            if (TryParse(Request,
                         Text,
                         out Acknowledgement<TRequest> acknowledgement,
                         out String                    ErrorResponse,
                         HTTPResponse,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         ProcessId,
                         CustomAcknowledgementParser))
            {
                return acknowledgement;
            }

            throw new ArgumentException("The given text representation of an acknowledgement is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out Acknowledgement, out ErrorResponse, CustomAcknowledgementParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of an acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="Acknowledgement">The parsed acknowledgement.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="CustomAcknowledgementParser">A delegate to parse custom acknowledgement JSON objects.</param>
        public static Boolean TryParse(TRequest                                                Request,
                                       JObject                                                 JSON,
                                       out Acknowledgement<TRequest>                           Acknowledgement,
                                       out String                                              ErrorResponse,
                                       HTTPResponse                                            HTTPResponse                  = null,
                                       DateTime?                                               ResponseTimestamp             = null,
                                       EventTracking_Id                                        EventTrackingId               = null,
                                       TimeSpan?                                               Runtime                       = null,
                                       Process_Id?                                             ProcessId                     = null,
                                       CustomJObjectParserDelegate<Acknowledgement<TRequest>>  CustomAcknowledgementParser   = null)
        {

            try
            {

                Acknowledgement = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse StatusCode              [mandatory]

                if (!JSON.ParseMandatoryJSON2("StatusCode",
                                              "StatusCode",
                                              OICPv2_3.StatusCode.TryParse,
                                              out StatusCode StatusCode,
                                              out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Result                  [optional]

                if (JSON.ParseOptional("Result",
                                       "result",
                                       out Boolean? Result,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse SessionId               [optional]

                if (JSON.ParseOptional("SessionID",
                                       "session identification",
                                       Session_Id.TryParse,
                                       out Session_Id? SessionId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse CPOPartnerSessionId     [optional]

                if (JSON.ParseOptional("CPOPartnerSessionID",
                                       "CPO partner session identification",
                                       CPOPartnerSession_Id.TryParse,
                                       out CPOPartnerSession_Id? CPOPartnerSessionId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse EMPPartnerSessionId     [optional]

                if (JSON.ParseOptional("EMPPartnerSessionID",
                                       "EMP partner session identification",
                                       EMPPartnerSession_Id.TryParse,
                                       out EMPPartnerSession_Id? EMPPartnerSessionId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse CustomData              [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                Acknowledgement = new Acknowledgement<TRequest>(Request,
                                                                ResponseTimestamp ?? DateTime.UtcNow,
                                                                EventTrackingId   ?? Request.EventTrackingId,
                                                                Runtime           ?? DateTime.UtcNow - Request.Timestamp,
                                                                StatusCode,
                                                                HTTPResponse,
                                                                Result,
                                                                SessionId,
                                                                CPOPartnerSessionId,
                                                                EMPPartnerSessionId,
                                                                ProcessId,
                                                                CustomData);

                if (CustomAcknowledgementParser != null)
                    Acknowledgement = CustomAcknowledgementParser(JSON,
                                                                  Acknowledgement);

                return true;

            }
            catch (Exception e)
            {
                Acknowledgement  = default;
                ErrorResponse    = "The given JSON representation of an acknowledgement is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out Acknowledgement, out ErrorResponse, CustomAcknowledgementParser = null)

        /// <summary>
        /// Try to parse the given text representation of an acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="Text">The text to parse.</param>
        /// <param name="Acknowledgement">The parsed acknowledgement.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="CustomAcknowledgementParser">A delegate to parse custom acknowledgement JSON objects.</param>
        public static Boolean TryParse(TRequest                                                Request,
                                       String                                                  Text,
                                       out Acknowledgement<TRequest>                           Acknowledgement,
                                       out String                                              ErrorResponse,
                                       HTTPResponse                                            HTTPResponse                  = null,
                                       DateTime?                                               ResponseTimestamp             = null,
                                       EventTracking_Id                                        EventTrackingId               = null,
                                       TimeSpan?                                               Runtime                       = null,
                                       Process_Id?                                             ProcessId                     = null,
                                       CustomJObjectParserDelegate<Acknowledgement<TRequest>>  CustomAcknowledgementParser   = null)
        {

            try
            {

                return TryParse(Request,
                                JObject.Parse(Text),
                                out Acknowledgement,
                                out ErrorResponse,
                                HTTPResponse,
                                ResponseTimestamp,
                                EventTrackingId,
                                Runtime,
                                ProcessId,
                                CustomAcknowledgementParser);

            }
            catch (Exception e)
            {
                Acknowledgement  = default;
                ErrorResponse    = "The given text representation of an acknowledgement is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomAcknowledgementSerializer = null, CustomStatusCodeSerializer = null)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomAcknowledgementSerializer">A delegate to customize the serialization of Acknowledgement responses.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<Acknowledgement>  CustomAcknowledgementSerializer   = null,
                              CustomJObjectSerializerDelegate<StatusCode>       CustomStatusCodeSerializer        = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("StatusCode", StatusCode.ToJSON(CustomStatusCodeSerializer: CustomStatusCodeSerializer)),

                           Result.             HasValue
                               ? new JProperty("Result",                Result.             Value)
                               : null,

                           SessionId.          HasValue
                               ? new JProperty("SessionID",             SessionId.          Value.ToString())
                               : null,

                           CPOPartnerSessionId.HasValue
                               ? new JProperty("CPOPartnerSessionID",   CPOPartnerSessionId.Value.ToString())
                               : null,

                           EMPPartnerSessionId.HasValue
                               ? new JProperty("EMPPartnerSessionID",   EMPPartnerSessionId.Value.ToString())
                               : null

                       );

            return CustomAcknowledgementSerializer != null
                       ? CustomAcknowledgementSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (Acknowledgement1, Acknowledgement2)

        /// <summary>
        /// Compares two acknowledgements for equality.
        /// </summary>
        /// <param name="Acknowledgement1">An acknowledgement.</param>
        /// <param name="Acknowledgement2">Another acknowledgement.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Acknowledgement<TRequest> Acknowledgement1,
                                           Acknowledgement<TRequest> Acknowledgement2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(Acknowledgement1, Acknowledgement2))
                return true;

            // If one is null, but not both, return false.
            if (Acknowledgement1 is null || Acknowledgement2 is null)
                return false;

            return Acknowledgement1.Equals(Acknowledgement2);

        }

        #endregion

        #region Operator != (Acknowledgement1, Acknowledgement2)

        /// <summary>
        /// Compares two acknowledgements for inequality.
        /// </summary>
        /// <param name="Acknowledgement1">An acknowledgement.</param>
        /// <param name="Acknowledgement2">Another acknowledgement.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Acknowledgement<TRequest> Acknowledgement1,
                                           Acknowledgement<TRequest> Acknowledgement2)

            => !(Acknowledgement1 == Acknowledgement2);

        #endregion

        #endregion

        #region IEquatable<Acknowledgement> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is Acknowledgement<TRequest> acknowledgement &&
                   Equals(acknowledgement);

        #endregion

        #region Equals(Acknowledgement)

        /// <summary>
        /// Compares two acknowledgements for equality.
        /// </summary>
        /// <param name="Acknowledgement">An acknowledgement to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Acknowledgement<TRequest> Acknowledgement)

            => !(Acknowledgement is null) &&

                 StatusCode.Equals(Acknowledgement.StatusCode) &&

              ((!Result.             HasValue && !Acknowledgement.Result.             HasValue) ||
                (Result.             HasValue &&  Acknowledgement.Result.             HasValue && Result.             Value.Equals(Acknowledgement.Result.             Value))) &&

              ((!SessionId.          HasValue && !Acknowledgement.SessionId.          HasValue) ||
                (SessionId.          HasValue &&  Acknowledgement.SessionId.          HasValue && SessionId.          Value.Equals(Acknowledgement.SessionId.          Value))) &&

              ((!CPOPartnerSessionId.HasValue && !Acknowledgement.CPOPartnerSessionId.HasValue) ||
                (CPOPartnerSessionId.HasValue &&  Acknowledgement.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(Acknowledgement.CPOPartnerSessionId.Value))) &&

              ((!EMPPartnerSessionId.HasValue && !Acknowledgement.EMPPartnerSessionId.HasValue) ||
                (EMPPartnerSessionId.HasValue &&  Acknowledgement.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(Acknowledgement.EMPPartnerSessionId.Value)));

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return StatusCode.          GetHashCode()       * 11 ^
                      (Result?.             GetHashCode() ?? 0) *  7 ^
                      (SessionId?.          GetHashCode() ?? 0) *  5 ^
                      (CPOPartnerSessionId?.GetHashCode() ?? 0) *  3 ^
                      (EMPPartnerSessionId?.GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(StatusCode.Code.ToString(), " => ",
                             new String[] {
                                 Result.             HasValue                 ? "result: "       + Result.             Value.ToString() : null,
                                 StatusCode.Description.   IsNotNullOrEmpty() ? "description: "  + StatusCode.Description               : null,
                                 StatusCode.AdditionalInfo.IsNotNullOrEmpty() ? "info: "         + StatusCode.AdditionalInfo            : null,
                                 SessionId.          HasValue                 ? "sessionId: "    + SessionId.          Value.ToString() : null,
                                 CPOPartnerSessionId.HasValue                 ? "CPOSessionId: " + CPOPartnerSessionId.Value.ToString() : null,
                                 EMPPartnerSessionId.HasValue                 ? "EMPSessionId: " + EMPPartnerSessionId.Value.ToString() : null,
                                 ProcessId.          HasValue                 ? "processId: "    + ProcessId.          Value.ToString() : null
                             }.AggregateWith(", "));

        #endregion


        #region ToBuilder()

        /// <summary>
        /// Return an acknowledgement builder.
        /// </summary>
        public Builder ToBuilder()

            => new Builder(Request,
                           HTTPResponse,
                           ResponseTimestamp,
                           EventTrackingId,
                           Runtime,
                           StatusCode,
                           Result,
                           SessionId,
                           CPOPartnerSessionId,
                           EMPPartnerSessionId,
                           ProcessId,
                           CustomData);

        #endregion

        #region (class) Builder

        /// <summary>
        /// An acknowledgement builder.
        /// </summary>
        public new class Builder : AResponse<TRequest,
                                             Acknowledgement<TRequest>>.Builder
        {

            #region Properties

            /// <summary>
            /// The status code of the operation.
            /// </summary>
            public StatusCode.Builder          StatusCode             { get; }

            /// <summary>
            /// The result of the operation.
            /// </summary>
            public Boolean?                    Result                 { get; set; }

            /// <summary>
            /// An optional charging session identification for
            /// RemoteReservationStart and RemoteStart requests.
            /// </summary>
            public Session_Id?                 SessionId              { get; set; }

            /// <summary>
            /// An optional EMP partner charging session identification for
            /// RemoteReservationStart and RemoteStart requests.
            /// </summary>
            public CPOPartnerSession_Id?       CPOPartnerSessionId    { get; set; }

            /// <summary>
            /// An optional CPO partner charging session identification for
            /// RemoteReservationStart and RemoteStart requests.
            /// </summary>
            public EMPPartnerSession_Id?       EMPPartnerSessionId    { get; set; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new acknowledgement builder.
            /// </summary>
            /// <param name="Request">The request leading to this response.</param>
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
            /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
            /// <param name="Runtime">The runtime of the request/response.</param>
            /// <param name="StatusCode">The status code of the operation.</param>
            /// <param name="Result">The result of the operation.</param>
            /// <param name="SessionId">An optional charging session identification.</param>
            /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
            /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(TRequest               Request               = null,
                           HTTPResponse           HTTPResponse          = null,
                           DateTime?              ResponseTimestamp     = null,
                           EventTracking_Id       EventTrackingId       = null,
                           TimeSpan?              Runtime               = null,
                           StatusCode?            StatusCode            = null,
                           Boolean?               Result                = null,
                           Session_Id?            SessionId             = null,
                           CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                           EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                           Process_Id?            ProcessId             = null,
                           JObject                CustomData            = null)

                : base(Request,
                       ResponseTimestamp,
                       EventTrackingId,
                       Runtime,
                       HTTPResponse,
                       ProcessId,
                       CustomData)

            {

                this.StatusCode           = StatusCode != null ? StatusCode.ToBuilder() : new StatusCode.Builder();
                this.Result               = Result;
                this.SessionId            = SessionId;
                this.CPOPartnerSessionId  = CPOPartnerSessionId;
                this.EMPPartnerSessionId  = EMPPartnerSessionId;

            }

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the acknowledgement.
            /// </summary>
            /// <param name="Builder">An acknowledgement builder.</param>
            public static implicit operator Acknowledgement<TRequest>(Builder Builder)

                => Builder?.ToImmutable();


            /// <summary>
            /// Return an immutable version of the acknowledgement.
            /// </summary>
            public override Acknowledgement<TRequest> ToImmutable()

                => new Acknowledgement<TRequest>(Request           ?? throw new ArgumentNullException(nameof(Request), "The given request must not be null!"),
                                                 ResponseTimestamp ?? DateTime.UtcNow,
                                                 EventTrackingId   ?? EventTracking_Id.New,
                                                 Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                                 StatusCode?.ToImmutable(),
                                                 HTTPResponse,
                                                 Result,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId,
                                                 ProcessId,
                                                 CustomData);

            #endregion

        }

        #endregion

    }

}
