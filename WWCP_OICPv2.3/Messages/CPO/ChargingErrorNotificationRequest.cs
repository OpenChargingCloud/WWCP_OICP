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
    /// The ChargingErrorNotification request.
    /// </summary>
    public class ChargingErrorNotificationRequest : ARequest<ChargingErrorNotificationRequest>
    {

        #region Properties

        /// <summary>
        /// The charging notification type.
        /// </summary>
        [Mandatory]
        public ChargingNotificationTypes  Type                      { get; }

        /// <summary>
        /// The Hubject session identification, that identifies the charging process.
        /// </summary>
        [Mandatory]
        public Session_Id                 SessionId                 { get; }

        /// <summary>
        /// The optional session identification assinged by the CPO partner.
        /// </summary>
        [Optional]
        public CPOPartnerSession_Id?      CPOPartnerSessionId       { get; }

        /// <summary>
        /// The optional session identification assinged by the EMP partner.
        /// </summary>
        [Optional]
        public EMPPartnerSession_Id?      EMPPartnerSessionId       { get; }

        /// <summary>
        /// The authentication data used to authorize the user or the car.
        /// </summary>
        [Mandatory]
        public Identification             Identification            { get; }

        /// <summary>
        /// The EVSE identification, that identifies the location of the charging process.
        /// </summary>
        [Mandatory]
        public EVSE_Id                    EVSEId                    { get; }

        /// <summary>
        /// The error class.
        /// </summary>
        [Mandatory]
        public ErrorClassTypes            ErrorType                 { get; }

        /// <summary>
        /// Additional information about the error.
        /// </summary>
        [Optional]
        public String                     ErrorAdditionalInfo       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new ChargingErrorNotification request.
        /// </summary>
        /// <param name="SessionId">The Hubject session identification, that identifies the charging process.</param>
        /// <param name="Identification">The authentication data used to authorize the user or the car.</param>
        /// <param name="EVSEId">The EVSE identification, that identifies the location of the charging process.</param>
        /// <param name="ErrorType">The error class.</param>
        /// 
        /// <param name="CPOPartnerSessionId">An optional session identification assinged by the CPO partner.</param>
        /// <param name="EMPPartnerSessionId">An optional session identification assinged by the EMP partner.</param>
        /// <param name="ErrorAdditionalInfo">Additional information about the error.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public ChargingErrorNotificationRequest(Session_Id             SessionId,
                                                 Identification         Identification,
                                                 EVSE_Id                EVSEId,
                                                 ErrorClassTypes        ErrorType,

                                                 CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                                 EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                                 String                 ErrorAdditionalInfo   = null,
                                                 JObject                CustomData            = null,

                                                 DateTime?              Timestamp             = null,
                                                 CancellationToken?     CancellationToken     = null,
                                                 EventTracking_Id       EventTrackingId       = null,
                                                 TimeSpan?              RequestTimeout        = null)

            : base(CustomData,
                   Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.Type                 = ChargingNotificationTypes.End;
            this.SessionId            = SessionId;
            this.Identification       = Identification;
            this.EVSEId               = EVSEId;
            this.ErrorType            = ErrorType;

            this.CPOPartnerSessionId  = CPOPartnerSessionId;
            this.EMPPartnerSessionId  = EMPPartnerSessionId;
            this.ErrorAdditionalInfo  = ErrorAdditionalInfo;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#64-eroamingchargingnotifications-error

        // {
        //     Swagger file is missing!
        // }

        #endregion

        #region (static) Parse   (JSON, CustomChargingErrorNotificationRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a charging notification error request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingErrorNotificationRequestParser">A delegate to parse custom charging notification error request JSON objects.</param>
        public static ChargingErrorNotificationRequest Parse(JObject                                                        JSON,
                                                             TimeSpan                                                       RequestTimeout,
                                                             DateTime?                                                      Timestamp                                      = null,
                                                             EventTracking_Id                                               EventTrackingId                                = null,
                                                             CustomJObjectParserDelegate<ChargingErrorNotificationRequest>  CustomChargingErrorNotificationRequestParser   = null)
        {

            if (TryParse(JSON,
                         RequestTimeout,
                         out ChargingErrorNotificationRequest  chargingErrorNotificationRequest,
                         out String                            ErrorResponse,
                         Timestamp,
                         EventTrackingId,
                         CustomChargingErrorNotificationRequestParser))
            {
                return chargingErrorNotificationRequest;
            }

            throw new ArgumentException("The given JSON representation of a charging notification error request is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomChargingErrorNotificationRequestParser = null)

        /// <summary>
        /// Parse the given text representation of a charging notification error request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingErrorNotificationRequestParser">A delegate to parse custom charging notification error request JSON objects.</param>
        public static ChargingErrorNotificationRequest Parse(String                                                         Text,
                                                             TimeSpan                                                       RequestTimeout,
                                                             DateTime?                                                      Timestamp                                      = null,
                                                             EventTracking_Id                                               EventTrackingId                                = null,
                                                             CustomJObjectParserDelegate<ChargingErrorNotificationRequest>  CustomChargingErrorNotificationRequestParser   = null)
        {

            if (TryParse(Text,
                         RequestTimeout,
                         out ChargingErrorNotificationRequest  chargingErrorNotificationRequest,
                         out String                            ErrorResponse,
                         Timestamp,
                         EventTrackingId,
                         CustomChargingErrorNotificationRequestParser))
            {
                return chargingErrorNotificationRequest;
            }

            throw new ArgumentException("The given text representation of a charging notification error request is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out ChargingErrorNotificationRequest, out ErrorResponse, CustomChargingErrorNotificationRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a charging notification error request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="ChargingErrorNotificationRequest">The parsed charging notification error request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingErrorNotificationRequestParser">A delegate to parse custom charging notification error request JSON objects.</param>
        public static Boolean TryParse(JObject                                                        JSON,
                                       TimeSpan                                                       RequestTimeout,
                                       out ChargingErrorNotificationRequest                           ChargingErrorNotificationRequest,
                                       out String                                                     ErrorResponse,
                                       DateTime?                                                      Timestamp                                      = null,
                                       EventTracking_Id                                               EventTrackingId                                = null,
                                       CustomJObjectParserDelegate<ChargingErrorNotificationRequest>  CustomChargingErrorNotificationRequestParser   = null)
        {

            try
            {

                ChargingErrorNotificationRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Type                      [mandatory]

                if (!JSON.ParseMandatoryEnum("Type",
                                             "charging notifications type",
                                             out ChargingNotificationTypes Type,
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
                    if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
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

                #region Parse ErrorType                 [mandatory]

                if (!JSON.ParseMandatory("ErrorType",
                                         "error type",
                                         ErrorClassTypesExtentions.TryParse,
                                         out ErrorClassTypes ErrorType,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ErrorAdditionalInfo       [optional]

                var ErrorAdditionalInfo = JSON.GetOptional("ErrorAdditionalInfo");

                #endregion

                #region Parse CustomData                [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                ChargingErrorNotificationRequest = new ChargingErrorNotificationRequest(SessionId,
                                                                                        Identification,
                                                                                        EVSEId,
                                                                                        ErrorType,

                                                                                        CPOPartnerSessionId,
                                                                                        EMPPartnerSessionId,
                                                                                        ErrorAdditionalInfo,
                                                                                        CustomData,

                                                                                        Timestamp,
                                                                                        null,
                                                                                        EventTrackingId,
                                                                                        RequestTimeout);

                if (CustomChargingErrorNotificationRequestParser != null)
                    ChargingErrorNotificationRequest = CustomChargingErrorNotificationRequestParser(JSON,
                                                                                                    ChargingErrorNotificationRequest);

                return true;

            }
            catch (Exception e)
            {
                ChargingErrorNotificationRequest  = default;
                ErrorResponse                     = "The given JSON representation of a charging notification error request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out ChargingErrorNotificationRequest, out ErrorResponse, CustomChargingErrorNotificationRequestParser = null)

        /// <summary>
        /// Try to parse the given text representation of a charging notification error request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="ChargingErrorNotificationRequest">The parsed charging notification error request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingErrorNotificationRequestParser">A delegate to parse custom charging notification error request JSON objects.</param>
        public static Boolean TryParse(String                                                         Text,
                                       TimeSpan                                                       RequestTimeout,
                                       out ChargingErrorNotificationRequest                           ChargingErrorNotificationRequest,
                                       out String                                                     ErrorResponse,
                                       DateTime?                                                      Timestamp                                      = null,
                                       EventTracking_Id                                               EventTrackingId                                = null,
                                       CustomJObjectParserDelegate<ChargingErrorNotificationRequest>  CustomChargingErrorNotificationRequestParser   = null)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                RequestTimeout,
                                out ChargingErrorNotificationRequest,
                                out ErrorResponse,
                                Timestamp,
                                EventTrackingId,
                                CustomChargingErrorNotificationRequestParser);

            }
            catch (Exception e)
            {
                ChargingErrorNotificationRequest  = default;
                ErrorResponse                     = "The given text representation of a charging notification error request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomChargingErrorNotificationRequestSerializer = null, CustomIdentificationSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomChargingErrorNotificationRequestSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ChargingErrorNotificationRequest>  CustomChargingErrorNotificationRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<Identification>                    CustomIdentificationSerializer                     = null)
        {

            var JSON = JSONObject.Create(
                           new JProperty("Type",                        Type.                     AsString()),
                           new JProperty("SessionID",                   SessionId.                ToString()),
                           new JProperty("EvseID",                      EVSEId.                   ToString()),
                           new JProperty("Identification",              Identification.           ToJSON(CustomIdentificationSerializer: CustomIdentificationSerializer)),
                           new JProperty("ErrorType",                   ErrorType.                AsString()),

                           CPOPartnerSessionId.   HasValue
                               ? new JProperty("CPOPartnerSessionID",   CPOPartnerSessionId.Value.ToString())
                               : null,

                           EMPPartnerSessionId.   HasValue
                               ? new JProperty("EMPPartnerSessionID",   EMPPartnerSessionId.Value.ToString())
                               : null,

                           ErrorAdditionalInfo.IsNotNullOrEmpty()
                               ? new JProperty("ErrorAdditionalInfo",   ErrorAdditionalInfo)
                               : null,

                           CustomData != null
                               ? new JProperty("CustomData",            CustomData)
                               : null

                       );

            return CustomChargingErrorNotificationRequestSerializer != null
                       ? CustomChargingErrorNotificationRequestSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this request.
        /// </summary>
        public ChargingErrorNotificationRequest Clone

            => new ChargingErrorNotificationRequest(SessionId,
                                                     Identification,
                                                     EVSEId,
                                                     ErrorType,

                                                     CPOPartnerSessionId,
                                                     EMPPartnerSessionId,
                                                     ErrorAdditionalInfo,
                                                     CustomData,

                                                     Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     RequestTimeout);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingErrorNotification1, ChargingErrorNotification2)

        /// <summary>
        /// Compares two charging notification error requests for equality.
        /// </summary>
        /// <param name="ChargingErrorNotification1">A charging notification error request.</param>
        /// <param name="ChargingErrorNotification2">Another charging notification error request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingErrorNotificationRequest ChargingErrorNotification1,
                                           ChargingErrorNotificationRequest ChargingErrorNotification2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingErrorNotification1, ChargingErrorNotification2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingErrorNotification1 is null || ChargingErrorNotification2 is null)
                return false;

            return ChargingErrorNotification1.Equals(ChargingErrorNotification2);

        }

        #endregion

        #region Operator != (ChargingErrorNotification1, ChargingErrorNotification2)

        /// <summary>
        /// Compares two charging notification error requests for inequality.
        /// </summary>
        /// <param name="ChargingErrorNotification1">A charging notification error request.</param>
        /// <param name="ChargingErrorNotification2">Another charging notification error request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingErrorNotificationRequest ChargingErrorNotification1,
                                           ChargingErrorNotificationRequest ChargingErrorNotification2)

            => !(ChargingErrorNotification1 == ChargingErrorNotification2);

        #endregion

        #endregion

        #region IEquatable<ChargingErrorNotificationRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is ChargingErrorNotificationRequest chargingErrorNotificationRequest &&
                   Equals(chargingErrorNotificationRequest);

        #endregion

        #region Equals(ChargingErrorNotificationRequest)

        /// <summary>
        /// Compares two charging notification error requests for equality.
        /// </summary>
        /// <param name="ChargingErrorNotificationRequest">A charging notification error request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(ChargingErrorNotificationRequest ChargingErrorNotificationRequest)

            => !(ChargingErrorNotificationRequest is null) &&

                 Type.          Equals(ChargingErrorNotificationRequest.Type)           &&
                 SessionId.     Equals(ChargingErrorNotificationRequest.SessionId)      &&
                 Identification.Equals(ChargingErrorNotificationRequest.Identification) &&
                 EVSEId.        Equals(ChargingErrorNotificationRequest.EVSEId)         &&
                 ErrorType.     Equals(ChargingErrorNotificationRequest.ErrorType);

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

                return Type.                   GetHashCode()       * 19 ^
                       SessionId.              GetHashCode()       * 17 ^
                       Identification.         GetHashCode()       * 13 ^
                       EVSEId.                 GetHashCode()       * 11 ^
                       ErrorType.              GetHashCode()       *  7 ^

                      (CPOPartnerSessionId?.   GetHashCode() ?? 0) *  5 ^
                      (EMPPartnerSessionId?.   GetHashCode() ?? 0) *  3 ^
                      (ErrorAdditionalInfo?.   GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Type,
                             " at ",  EVSEId,
                             " for ", Identification,

                             " (", SessionId, ") => ",

                             ErrorType.AsString(),

                             ErrorAdditionalInfo.IsNotNullOrEmpty() ? ": '" + ErrorAdditionalInfo + "'" : "");

        #endregion

    }

}
