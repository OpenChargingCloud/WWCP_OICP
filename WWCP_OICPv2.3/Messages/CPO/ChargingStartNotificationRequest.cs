﻿/*
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
    /// The ChargingStartNotification request.
    /// </summary>
    public class ChargingStartNotificationRequest : ARequest<ChargingStartNotificationRequest>
    {

        #region Properties

        /// <summary>
        /// The charging notification type.
        /// </summary>
        [Mandatory]
        public ChargingNotificationTypes         Type                               { get; }

        /// <summary>
        /// The Hubject session identification, that identifies the charging process.
        /// </summary>
        [Mandatory]
        public Session_Id                        SessionId                          { get; }

        /// <summary>
        /// The optional session identification assinged by the CPO partner.
        /// </summary>
        [Optional]
        public CPOPartnerSession_Id?             CPOPartnerSessionId                { get; }

        /// <summary>
        /// The optional session identification assinged by the EMP partner.
        /// </summary>
        [Optional]
        public EMPPartnerSession_Id?             EMPPartnerSessionId                { get; }

        /// <summary>
        /// The authentication data used to authorize the user or the car.
        /// </summary>
        [Mandatory]
        public Identification                    Identification                     { get; }

        /// <summary>
        /// The EVSE identification, that identifies the location of the charging process.
        /// </summary>
        [Mandatory]
        public EVSE_Id                           EVSEId                             { get; }

        /// <summary>
        /// The timestamp when the charging process started.
        /// </summary>
        [Mandatory]
        public DateTime                          ChargingStart                      { get; }

        /// <summary>
        /// The timestamp when the charging session started.
        /// </summary>
        [Optional]
        public DateTime?                         SessionStart                       { get; }

        /// <summary>
        /// The optional starting value of the energy meter [kWh].
        /// </summary>
        [Optional]
        public Decimal?                          MeterValueStart                    { get; }

        /// <summary>
        /// The optional operator identification.
        /// </summary>
        [Optional]
        public Operator_Id?                      OperatorId                         { get; }

        /// <summary>
        /// The optional pricing product name (for identifying a tariff) that must be unique.
        /// </summary>
        [Optional]
        public PartnerProduct_Id?                PartnerProductId                   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new ChargingStartNotification request.
        /// </summary>
        /// <param name="SessionId">The Hubject session identification, that identifies the charging process.</param>
        /// <param name="Identification">The authentication data used to authorize the user or the car.</param>
        /// <param name="EVSEId">The EVSE identification, that identifies the location of the charging process.</param>
        /// <param name="ChargingStart">The timestamp when the charging process started.</param>
        /// 
        /// <param name="CPOPartnerSessionId">An optional session identification assinged by the CPO partner.</param>
        /// <param name="EMPPartnerSessionId">An optional session identification assinged by the EMP partner.</param>
        /// <param name="SessionStart">An optional timestamp when the charging session started.</param>
        /// <param name="MeterValueStart">An optional starting value of the energy meter [kWh].</param>
        /// <param name="OperatorId">An optional operator identification of the hub operator.</param>
        /// <param name="PartnerProductId">An optional pricing product name (for identifying a tariff) that must be unique.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public ChargingStartNotificationRequest(Session_Id             SessionId,
                                                Identification         Identification,
                                                EVSE_Id                EVSEId,
                                                DateTime               ChargingStart,

                                                CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                                EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                                DateTime?              SessionStart          = null,
                                                Decimal?               MeterValueStart       = null,
                                                Operator_Id?           OperatorId            = null,
                                                PartnerProduct_Id?     PartnerProductId      = null,
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

            this.Type                 = ChargingNotificationTypes.Start;
            this.SessionId            = SessionId;
            this.Identification       = Identification;
            this.EVSEId               = EVSEId;
            this.ChargingStart        = ChargingStart;

            this.CPOPartnerSessionId  = CPOPartnerSessionId;
            this.EMPPartnerSessionId  = EMPPartnerSessionId;
            this.SessionStart         = SessionStart;
            this.MeterValueStart      = MeterValueStart;
            this.OperatorId           = OperatorId;
            this.PartnerProductId     = PartnerProductId;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#61-eroamingchargingnotifications-start

        // {
        //     Swagger file is missing!
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomChargingStartNotificationRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a charging notification start request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingStartNotificationRequestParser">A delegate to parse custom charging notification start request JSON objects.</param>
        public static ChargingStartNotificationRequest Parse(JObject                                                         JSON,
                                                             Process_Id?                                                     ProcessId                                      = null,

                                                             DateTime?                                                       Timestamp                                      = null,
                                                             CancellationToken?                                              CancellationToken                              = null,
                                                             EventTracking_Id?                                               EventTrackingId                                = null,
                                                             TimeSpan?                                                       RequestTimeout                                 = null,

                                                             CustomJObjectParserDelegate<ChargingStartNotificationRequest>?  CustomChargingStartNotificationRequestParser   = null)
        {

            if (TryParse(JSON,
                         out ChargingStartNotificationRequest?  chargingStartNotificationRequest,
                         out String?                            errorResponse,
                         ProcessId,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout,
                         CustomChargingStartNotificationRequestParser))
            {
                return chargingStartNotificationRequest!;
            }

            throw new ArgumentException("The given JSON representation of a charging notification start request is invalid: " + errorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, ..., CustomChargingStartNotificationRequestParser = null)

        /// <summary>
        /// Parse the given text representation of a charging notification start request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingStartNotificationRequestParser">A delegate to parse custom charging notification start request JSON objects.</param>
        public static ChargingStartNotificationRequest Parse(String                                                          Text,
                                                             Process_Id?                                                     ProcessId                                      = null,

                                                             DateTime?                                                       Timestamp                                      = null,
                                                             CancellationToken?                                              CancellationToken                              = null,
                                                             EventTracking_Id?                                               EventTrackingId                                = null,
                                                             TimeSpan?                                                       RequestTimeout                                 = null,

                                                             CustomJObjectParserDelegate<ChargingStartNotificationRequest>?  CustomChargingStartNotificationRequestParser   = null)
        {

            if (TryParse(Text,
                         out ChargingStartNotificationRequest?  chargingStartNotificationRequest,
                         out String?                            errorResponse,
                         ProcessId,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout,
                         CustomChargingStartNotificationRequestParser))
            {
                return chargingStartNotificationRequest!;
            }

            throw new ArgumentException("The given text representation of a charging notification start request is invalid: " + errorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out ChargingStartNotificationRequest, out ErrorResponse, ..., CustomChargingStartNotificationRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a charging notification start request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="ChargingStartNotificationRequest">The parsed charging notification start request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingStartNotificationRequestParser">A delegate to parse custom charging notification start request JSON objects.</param>
        public static Boolean TryParse(JObject                                                         JSON,
                                       out ChargingStartNotificationRequest?                           ChargingStartNotificationRequest,
                                       out String?                                                     ErrorResponse,
                                       Process_Id?                                                     ProcessId                                      = null,

                                       DateTime?                                                       Timestamp                                      = null,
                                       CancellationToken?                                              CancellationToken                              = null,
                                       EventTracking_Id?                                               EventTrackingId                                = null,
                                       TimeSpan?                                                       RequestTimeout                                 = null,

                                       CustomJObjectParserDelegate<ChargingStartNotificationRequest>?  CustomChargingStartNotificationRequestParser   = null)
        {

            try
            {

                ChargingStartNotificationRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Type                   [mandatory]

                if (!JSON.ParseMandatoryEnum("Type",
                                             "charging notifications type",
                                             out ChargingNotificationTypes Type,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse SessionId              [mandatory]

                if (!JSON.ParseMandatory("SessionID",
                                         "session identification",
                                         Session_Id.TryParse,
                                         out Session_Id SessionId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse CPOPartnerSessionId    [optional]

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

                #region Parse EMPPartnerSessionId    [optional]

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

                #region Parse Identification         [mandatory]

                if (!JSON.ParseMandatoryJSON2("Identification",
                                              "identification",
                                              OICPv2_3.Identification.TryParse,
                                              out Identification Identification,
                                              out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse EVSEId                 [mandatory]

                if (!JSON.ParseMandatory("EvseID",
                                         "EVSE identification",
                                         EVSE_Id.TryParse,
                                         out EVSE_Id EVSEId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ChargingStart          [mandatory]

                if (!JSON.ParseMandatory("ChargingStart",
                                         "charging start",
                                         out DateTime ChargingStart,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse SessionStart           [optional]

                if (JSON.ParseOptional("SessionStart",
                                       "session start",
                                       out DateTime? SessionStart,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse MeterValueStart        [optional]

                if (JSON.ParseOptional("MeterValueStart",
                                       "meter value start",
                                       out Decimal? MeterValueStart,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse OperatorId             [optional]

                if (JSON.ParseOptional("OperatorID",
                                       "operator identification",
                                       Operator_Id.TryParse,
                                       out Operator_Id? OperatorId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse PartnerProductId       [optional]

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

                #region Parse CustomData             [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                ChargingStartNotificationRequest = new ChargingStartNotificationRequest(SessionId,
                                                                                        Identification,
                                                                                        EVSEId,
                                                                                        ChargingStart,

                                                                                        CPOPartnerSessionId,
                                                                                        EMPPartnerSessionId,
                                                                                        SessionStart,
                                                                                        MeterValueStart,
                                                                                        OperatorId,
                                                                                        PartnerProductId,
                                                                                        ProcessId,
                                                                                        CustomData,

                                                                                        Timestamp,
                                                                                        CancellationToken,
                                                                                        EventTrackingId,
                                                                                        RequestTimeout);

                if (CustomChargingStartNotificationRequestParser is not null)
                    ChargingStartNotificationRequest = CustomChargingStartNotificationRequestParser(JSON,
                                                                                                    ChargingStartNotificationRequest);

                return true;

            }
            catch (Exception e)
            {
                ChargingStartNotificationRequest  = default;
                ErrorResponse                     = "The given JSON representation of a charging notification start request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out ChargingStartNotificationRequest, out ErrorResponse, ..., CustomChargingStartNotificationRequestParser = null)

        /// <summary>
        /// Try to parse the given text representation of a charging notification start request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="ChargingStartNotificationRequest">The parsed charging notification start request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingStartNotificationRequestParser">A delegate to parse custom charging notification start request JSON objects.</param>
        public static Boolean TryParse(String                                                          Text,
                                       out ChargingStartNotificationRequest?                           ChargingStartNotificationRequest,
                                       out String?                                                     ErrorResponse,
                                       Process_Id?                                                     ProcessId                                      = null,

                                       DateTime?                                                       Timestamp                                      = null,
                                       CancellationToken?                                              CancellationToken                              = null,
                                       EventTracking_Id?                                               EventTrackingId                                = null,
                                       TimeSpan?                                                       RequestTimeout                                 = null,

                                       CustomJObjectParserDelegate<ChargingStartNotificationRequest>?  CustomChargingStartNotificationRequestParser   = null)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out ChargingStartNotificationRequest,
                                out ErrorResponse,
                                ProcessId,
                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout,
                                CustomChargingStartNotificationRequestParser);

            }
            catch (Exception e)
            {
                ChargingStartNotificationRequest  = default;
                ErrorResponse                     = "The given text representation of a charging notification start request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomChargingStartNotificationRequestSerializer = null, CustomIdentificationSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomChargingStartNotificationRequestSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ChargingStartNotificationRequest>?  CustomChargingStartNotificationRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<Identification>?                    CustomIdentificationSerializer                     = null)
        {

            var JSON = JSONObject.Create(
                           new JProperty("Type",                       Type.                     AsString()),
                           new JProperty("SessionID",                  SessionId.                ToString()),
                           new JProperty("EvseID",                     EVSEId.                   ToString()),
                           new JProperty("Identification",             Identification.           ToJSON(CustomIdentificationSerializer: CustomIdentificationSerializer)),
                           new JProperty("ChargingStart",              ChargingStart.            ToIso8601()),

                           CPOPartnerSessionId.HasValue
                               ? new JProperty("CPOPartnerSessionID",  CPOPartnerSessionId.Value.ToString())
                               : null,

                           EMPPartnerSessionId.HasValue
                               ? new JProperty("EMPPartnerSessionID",  EMPPartnerSessionId.Value.ToString())
                               : null,

                           SessionStart.       HasValue
                               ? new JProperty("SessionStart",         SessionStart.       Value.ToIso8601())
                               : null,

                           MeterValueStart.    HasValue
                               ? new JProperty("MeterValueStart",      String.Format("{0:0.###}", MeterValueStart.Value).Replace(",", "."))
                               : null,

                           OperatorId.         HasValue
                               ? new JProperty("OperatorID",           OperatorId.         Value.ToString())
                               : null,

                           PartnerProductId.   HasValue
                               ? new JProperty("PartnerProductID",     PartnerProductId.   Value.ToString())
                               : null,

                           CustomData is not null
                               ? new JProperty("CustomData",           CustomData)
                               : null

                       );

            return CustomChargingStartNotificationRequestSerializer is not null
                       ? CustomChargingStartNotificationRequestSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this request.
        /// </summary>
        public ChargingStartNotificationRequest Clone

            => new (SessionId,
                    Identification,
                    EVSEId,
                    ChargingStart,

                    CPOPartnerSessionId,
                    EMPPartnerSessionId,
                    SessionStart,
                    MeterValueStart,
                    OperatorId,
                    PartnerProductId,
                    ProcessId,
                    CustomData,

                    Timestamp,
                    CancellationToken,
                    EventTrackingId,
                    RequestTimeout);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStartNotification1, ChargingStartNotification2)

        /// <summary>
        /// Compares two charging notification start requests for equality.
        /// </summary>
        /// <param name="ChargingStartNotification1">A charging notification start request.</param>
        /// <param name="ChargingStartNotification2">Another charging notification start request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingStartNotificationRequest ChargingStartNotification1,
                                           ChargingStartNotificationRequest ChargingStartNotification2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStartNotification1, ChargingStartNotification2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingStartNotification1 is null || ChargingStartNotification2 is null)
                return false;

            return ChargingStartNotification1.Equals(ChargingStartNotification2);

        }

        #endregion

        #region Operator != (ChargingStartNotification1, ChargingStartNotification2)

        /// <summary>
        /// Compares two charging notification start requests for inequality.
        /// </summary>
        /// <param name="ChargingStartNotification1">A charging notification start request.</param>
        /// <param name="ChargingStartNotification2">Another charging notification start request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingStartNotificationRequest ChargingStartNotification1,
                                           ChargingStartNotificationRequest ChargingStartNotification2)

            => !(ChargingStartNotification1 == ChargingStartNotification2);

        #endregion

        #endregion

        #region IEquatable<ChargingStartNotificationRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStartNotificationRequest chargingStartNotificationRequest &&
                   Equals(chargingStartNotificationRequest);

        #endregion

        #region Equals(ChargingStartNotificationRequest)

        /// <summary>
        /// Compares two charging notification start requests for equality.
        /// </summary>
        /// <param name="ChargingStartNotificationRequest">A charging notification start request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(ChargingStartNotificationRequest? ChargingStartNotificationRequest)

            => ChargingStartNotificationRequest is not null &&

               Type.          Equals(ChargingStartNotificationRequest.Type)           &&
               SessionId.     Equals(ChargingStartNotificationRequest.SessionId)      &&
               Identification.Equals(ChargingStartNotificationRequest.Identification) &&
               EVSEId.        Equals(ChargingStartNotificationRequest.EVSEId)         &&
               ChargingStart. Equals(ChargingStartNotificationRequest.ChargingStart);

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

                return Type.                GetHashCode()       * 31 ^
                       SessionId.           GetHashCode()       * 29 ^
                       Identification.      GetHashCode()       * 23 ^
                       EVSEId.              GetHashCode()       * 19 ^
                       ChargingStart.       GetHashCode()       * 17 ^

                      (CPOPartnerSessionId?.GetHashCode() ?? 0) * 13 ^
                      (EMPPartnerSessionId?.GetHashCode() ?? 0) * 11 ^
                      (SessionStart?.       GetHashCode() ?? 0) *  7 ^
                      (MeterValueStart?.    GetHashCode() ?? 0) *  5 ^
                      (OperatorId?.         GetHashCode() ?? 0) *  3 ^
                      (PartnerProductId?.   GetHashCode() ?? 0);

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

                             " (" + SessionId + ") ",

                             PartnerProductId.HasValue
                                 ? " of " + PartnerProductId.Value
                                 : null,

                             " at " + ChargingStart.ToIso8601());

        #endregion

    }

}
