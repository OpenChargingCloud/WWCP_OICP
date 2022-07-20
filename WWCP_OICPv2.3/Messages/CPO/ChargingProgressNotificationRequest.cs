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

using System;
using System.Globalization;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The ChargingProgressNotification request.
    /// </summary>
    public class ChargingProgressNotificationRequest : ARequest<ChargingProgressNotificationRequest>
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
        /// The timestamp when the charging progress parameters had been captured.
        /// </summary>
        [Mandatory]
        public DateTime                          EventOcurred                       { get; }

        /// <summary>
        /// Charging Duration = EventOccurred - Charging Start.
        /// </summary>
        [Optional]
        public TimeSpan?                         ChargingDuration                   { get; }

        /// <summary>
        /// The timestamp when the charging session started.
        /// </summary>
        [Optional]
        public DateTime?                         SessionStart                       { get; }

        /// <summary>
        /// The optional consumed energy till now.
        /// </summary>
        [Optional]
        public Decimal?                          ConsumedEnergyProgress             { get; }

        /// <summary>
        /// The optional starting value of the energy meter [kWh].
        /// </summary>
        [Optional]
        public Decimal?                          MeterValueStart                    { get; }

        /// <summary>
        /// The optional enumeration of meter values during the charging session.
        /// </summary>
        [Optional]
        public IEnumerable<Decimal>?             MeterValuesInBetween               { get; }

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
        /// Create a new ChargingProgressNotification request.
        /// </summary>
        /// <param name="SessionId">The Hubject session identification, that identifies the charging process.</param>
        /// <param name="Identification">The authentication data used to authorize the user or the car.</param>
        /// <param name="EVSEId">The EVSE identification, that identifies the location of the charging process.</param>
        /// <param name="ChargingStart">The timestamp when the charging process started.</param>
        /// <param name="EventOcurred">The timestamp when the charging progress parameters had been captured.</param>
        /// 
        /// <param name="CPOPartnerSessionId">An optional session identification assinged by the CPO partner.</param>
        /// <param name="EMPPartnerSessionId">An optional session identification assinged by the EMP partner.</param>
        /// <param name="ChargingDuration">Charging Duration = EventOccurred - Charging Duration.</param>
        /// <param name="SessionStart">An optional timestamp when the charging session started.</param>
        /// <param name="ConsumedEnergyProgress">The optional consumed energy till now.</param>
        /// <param name="MeterValueStart">An optional starting value of the energy meter [kWh].</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="OperatorId">An optional operator identification of the hub operator.</param>
        /// <param name="PartnerProductId">An optional pricing product name (for identifying a tariff) that must be unique.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public ChargingProgressNotificationRequest(Session_Id             SessionId,
                                                   Identification         Identification,
                                                   EVSE_Id                EVSEId,
                                                   DateTime               ChargingStart,
                                                   DateTime               EventOcurred,

                                                   CPOPartnerSession_Id?  CPOPartnerSessionId      = null,
                                                   EMPPartnerSession_Id?  EMPPartnerSessionId      = null,
                                                   TimeSpan?              ChargingDuration         = null,
                                                   DateTime?              SessionStart             = null,
                                                   Decimal?               ConsumedEnergyProgress   = null,
                                                   Decimal?               MeterValueStart          = null,
                                                   IEnumerable<Decimal>?  MeterValuesInBetween     = null,
                                                   Operator_Id?           OperatorId               = null,
                                                   PartnerProduct_Id?     PartnerProductId         = null,
                                                   JObject?               CustomData               = null,

                                                   DateTime?              Timestamp                = null,
                                                   CancellationToken?     CancellationToken        = null,
                                                   EventTracking_Id?      EventTrackingId          = null,
                                                   TimeSpan?              RequestTimeout           = null)

            : base(CustomData,
                   Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.Type                    = ChargingNotificationTypes.Progress;
            this.SessionId               = SessionId;
            this.Identification          = Identification;
            this.EVSEId                  = EVSEId;
            this.ChargingStart           = ChargingStart;
            this.EventOcurred            = EventOcurred;

            this.CPOPartnerSessionId     = CPOPartnerSessionId;
            this.EMPPartnerSessionId     = EMPPartnerSessionId;
            this.ChargingDuration        = ChargingDuration;
            this.SessionStart            = SessionStart;
            this.ConsumedEnergyProgress  = ConsumedEnergyProgress;
            this.MeterValueStart         = MeterValueStart;
            this.MeterValuesInBetween    = MeterValuesInBetween;
            this.OperatorId              = OperatorId;
            this.PartnerProductId        = PartnerProductId;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#62-eroamingchargingnotifications-progress

        // {
        //     Swagger file is missing!
        // }

        #endregion

        #region (static) Parse   (JSON, CustomChargingProgressNotificationRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a charging notification progress request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingProgressNotificationRequestParser">A delegate to parse custom charging notification progress request JSON objects.</param>
        public static ChargingProgressNotificationRequest Parse(JObject                                                            JSON,
                                                                TimeSpan                                                           RequestTimeout,
                                                                DateTime?                                                          Timestamp                                         = null,
                                                                EventTracking_Id?                                                  EventTrackingId                                   = null,
                                                                CustomJObjectParserDelegate<ChargingProgressNotificationRequest>?  CustomChargingProgressNotificationRequestParser   = null)
        {

            if (TryParse(JSON,
                         RequestTimeout,
                         out ChargingProgressNotificationRequest?  chargingProgressNotificationRequest,
                         out String?                               errorResponse,
                         Timestamp,
                         EventTrackingId,
                         CustomChargingProgressNotificationRequestParser))
            {
                return chargingProgressNotificationRequest!;
            }

            throw new ArgumentException("The given JSON representation of a charging notification progress request is invalid: " + errorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomChargingProgressNotificationRequestParser = null)

        /// <summary>
        /// Parse the given text representation of a charging notification progress request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingProgressNotificationRequestParser">A delegate to parse custom charging notification progress request JSON objects.</param>
        public static ChargingProgressNotificationRequest Parse(String                                                             Text,
                                                                TimeSpan                                                           RequestTimeout,
                                                                DateTime?                                                          Timestamp                                         = null,
                                                                EventTracking_Id?                                                  EventTrackingId                                   = null,
                                                                CustomJObjectParserDelegate<ChargingProgressNotificationRequest>?  CustomChargingProgressNotificationRequestParser   = null)
        {

            if (TryParse(Text,
                         RequestTimeout,
                         out ChargingProgressNotificationRequest?  chargingProgressNotificationRequest,
                         out String?                               errorResponse,
                         Timestamp,
                         EventTrackingId,
                         CustomChargingProgressNotificationRequestParser))
            {
                return chargingProgressNotificationRequest!;
            }

            throw new ArgumentException("The given text representation of a charging notification progress request is invalid: " + errorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out ChargingProgressNotificationRequest, out ErrorResponse, CustomChargingProgressNotificationRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a charging notification progress request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="ChargingProgressNotificationRequest">The parsed charging notification progress request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingProgressNotificationRequestParser">A delegate to parse custom charging notification progress request JSON objects.</param>
        public static Boolean TryParse(JObject                                                            JSON,
                                       TimeSpan                                                           RequestTimeout,
                                       out ChargingProgressNotificationRequest?                           ChargingProgressNotificationRequest,
                                       out String?                                                        ErrorResponse,
                                       DateTime?                                                          Timestamp                                         = null,
                                       EventTracking_Id?                                                  EventTrackingId                                   = null,
                                       CustomJObjectParserDelegate<ChargingProgressNotificationRequest>?  CustomChargingProgressNotificationRequestParser   = null)
        {

            try
            {

                ChargingProgressNotificationRequest = default;

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

                #region Parse ChargingStart             [mandatory]

                if (!JSON.ParseMandatory("ChargingStart",
                                         "charging start",
                                         out DateTime ChargingStart,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse EventOcurred              [mandatory]

                if (!JSON.ParseMandatory("EventOcurred",
                                         "event ocurred",
                                         out DateTime EventOcurred,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ChargingDuration          [optional]

                TimeSpan? ChargingDuration = default;

                if (JSON.ParseOptional("ChargingDuration",
                                       "charging duration",
                                       out UInt64? ChargingDurationUInt64,
                                       out ErrorResponse))
                {

                    if (ErrorResponse is not null)
                        return false;

                    if (ChargingDurationUInt64.HasValue)
                        ChargingDuration = TimeSpan.FromMilliseconds(ChargingDurationUInt64.Value);

                }

                #endregion

                #region Parse SessionStart              [optional]

                if (JSON.ParseOptional("SessionStart",
                                       "session start",
                                       out DateTime? SessionStart,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ConsumedEnergyProgress    [optional]

                if (JSON.ParseOptional("ConsumedEnergyProgress",
                                       "consumed energy progress",
                                       out Decimal? ConsumedEnergyProgress,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse MeterValueStart           [optional]

                if (JSON.ParseOptional("MeterValueStart",
                                       "meter value start",
                                       out Decimal? MeterValueStart,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse MeterValuesInBetween      [optional]

                IEnumerable<Decimal> MeterValuesInBetween = null;

                if (JSON.ParseOptional("MeterValueInBetween",
                                       "meter values in between",
                                       out JObject MeterValuesInBetweenJSON,
                                       out ErrorResponse))
                {

                    if (ErrorResponse is not null)
                        return false;

                    if (MeterValuesInBetweenJSON.ParseOptionalJSON("meterValues",
                                                                   "meter values",
                                                                   (String input, out Decimal number) => Decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out number),
                                                                   out MeterValuesInBetween,
                                                                   out ErrorResponse))
                    {
                        if (ErrorResponse is not null)
                            return false;
                    }

                }

                #endregion

                #region Parse OperatorId                [optional]

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


                ChargingProgressNotificationRequest = new ChargingProgressNotificationRequest(SessionId,
                                                                                              Identification,
                                                                                              EVSEId,
                                                                                              ChargingStart,
                                                                                              EventOcurred,

                                                                                              CPOPartnerSessionId,
                                                                                              EMPPartnerSessionId,
                                                                                              ChargingDuration,
                                                                                              SessionStart,
                                                                                              ConsumedEnergyProgress,
                                                                                              MeterValueStart,
                                                                                              MeterValuesInBetween,
                                                                                              OperatorId,
                                                                                              PartnerProductId,
                                                                                              CustomData,

                                                                                              Timestamp,
                                                                                              null,
                                                                                              EventTrackingId,
                                                                                              RequestTimeout);

                if (CustomChargingProgressNotificationRequestParser is not null)
                    ChargingProgressNotificationRequest = CustomChargingProgressNotificationRequestParser(JSON,
                                                                                                          ChargingProgressNotificationRequest);

                return true;

            }
            catch (Exception e)
            {
                ChargingProgressNotificationRequest  = default;
                ErrorResponse                        = "The given JSON representation of a charging notification progress request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out ChargingProgressNotificationRequest, out ErrorResponse, CustomChargingProgressNotificationRequestParser = null)

        /// <summary>
        /// Try to parse the given text representation of a charging notification progress request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="ChargingProgressNotificationRequest">The parsed charging notification progress request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingProgressNotificationRequestParser">A delegate to parse custom charging notification progress request JSON objects.</param>
        public static Boolean TryParse(String                                                             Text,
                                       TimeSpan                                                           RequestTimeout,
                                       out ChargingProgressNotificationRequest?                           ChargingProgressNotificationRequest,
                                       out String?                                                        ErrorResponse,
                                       DateTime?                                                          Timestamp                                         = null,
                                       EventTracking_Id?                                                  EventTrackingId                                   = null,
                                       CustomJObjectParserDelegate<ChargingProgressNotificationRequest>?  CustomChargingProgressNotificationRequestParser   = null)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                RequestTimeout,
                                out ChargingProgressNotificationRequest,
                                out ErrorResponse,
                                Timestamp,
                                EventTrackingId,
                                CustomChargingProgressNotificationRequestParser);

            }
            catch (Exception e)
            {
                ChargingProgressNotificationRequest  = default;
                ErrorResponse                        = "The given text representation of a charging notification progress request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomChargingProgressNotificationRequestSerializer = null, CustomIdentificationSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomChargingProgressNotificationRequestSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ChargingProgressNotificationRequest>?  CustomChargingProgressNotificationRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<Identification>?                       CustomIdentificationSerializer                        = null)
        {

            var JSON = JSONObject.Create(
                           new JProperty("Type",                          Type.                     AsString()),
                           new JProperty("SessionID",                     SessionId.                ToString()),
                           new JProperty("EvseID",                        EVSEId.                   ToString()),
                           new JProperty("Identification",                Identification.           ToJSON(CustomIdentificationSerializer: CustomIdentificationSerializer)),
                           new JProperty("ChargingStart",                 ChargingStart.            ToIso8601()),
                           new JProperty("EventOcurred",                  EventOcurred.             ToIso8601()),

                           CPOPartnerSessionId.   HasValue
                               ? new JProperty("CPOPartnerSessionID",     CPOPartnerSessionId.Value.ToString())
                               : null,

                           EMPPartnerSessionId.   HasValue
                               ? new JProperty("EMPPartnerSessionID",     EMPPartnerSessionId.Value.ToString())
                               : null,

                           ChargingDuration.      HasValue
                               ? new JProperty("ChargingDuration",        Convert.ToUInt64(ChargingDuration.Value.TotalMilliseconds))
                               : null,

                           SessionStart.          HasValue
                               ? new JProperty("SessionStart",            SessionStart.       Value.ToIso8601())
                               : null,

                           ConsumedEnergyProgress.HasValue
                               ? new JProperty("ConsumedEnergyProgress",  String.Format("{0:0.###}", ConsumedEnergyProgress.Value).Replace(",", "."))
                               : null,

                           MeterValueStart.    HasValue
                               ? new JProperty("MeterValueStart",         String.Format("{0:0.###}", MeterValueStart.       Value).Replace(",", "."))
                               : null,

                           MeterValuesInBetween is not null && MeterValuesInBetween.Any()
                               ? new JProperty("MeterValueInBetween",
                                     new JObject(  // OICP is crazy!
                                         new JProperty("meterValues",     new JArray(MeterValuesInBetween.
                                                                                         Select(meterValue => String.Format("{0:0.###}", meterValue).Replace(",", ".")))
                                         )
                                     )
                                 )
                               : null,

                           OperatorId.HasValue
                               ? new JProperty("OperatorID",              OperatorId.         Value.ToString())
                               : null,

                           PartnerProductId.   HasValue
                               ? new JProperty("PartnerProductID",        PartnerProductId.   Value.ToString())
                               : null,

                           CustomData is not null
                               ? new JProperty("CustomData",              CustomData)
                               : null

                       );

            return CustomChargingProgressNotificationRequestSerializer is not null
                       ? CustomChargingProgressNotificationRequestSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this request.
        /// </summary>
        public ChargingProgressNotificationRequest Clone

            => new (SessionId,
                    Identification,
                    EVSEId,
                    ChargingStart,
                    EventOcurred,

                    CPOPartnerSessionId,
                    EMPPartnerSessionId,
                    ChargingDuration,
                    SessionStart,
                    ConsumedEnergyProgress,
                    MeterValueStart,
                    MeterValuesInBetween,
                    OperatorId,
                    PartnerProductId,
                    CustomData,

                    Timestamp,
                    CancellationToken,
                    EventTrackingId,
                    RequestTimeout);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingProgressNotification1, ChargingProgressNotification2)

        /// <summary>
        /// Compares two charging notification progress requests for equality.
        /// </summary>
        /// <param name="ChargingProgressNotification1">A charging notification progress request.</param>
        /// <param name="ChargingProgressNotification2">Another charging notification progress request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingProgressNotificationRequest ChargingProgressNotification1,
                                           ChargingProgressNotificationRequest ChargingProgressNotification2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingProgressNotification1, ChargingProgressNotification2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingProgressNotification1 is null || ChargingProgressNotification2 is null)
                return false;

            return ChargingProgressNotification1.Equals(ChargingProgressNotification2);

        }

        #endregion

        #region Operator != (ChargingProgressNotification1, ChargingProgressNotification2)

        /// <summary>
        /// Compares two charging notification progress requests for inequality.
        /// </summary>
        /// <param name="ChargingProgressNotification1">A charging notification progress request.</param>
        /// <param name="ChargingProgressNotification2">Another charging notification progress request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingProgressNotificationRequest ChargingProgressNotification1,
                                           ChargingProgressNotificationRequest ChargingProgressNotification2)

            => !(ChargingProgressNotification1 == ChargingProgressNotification2);

        #endregion

        #endregion

        #region IEquatable<ChargingProgressNotificationRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargingProgressNotificationRequest chargingProgressNotificationRequest &&
                   Equals(chargingProgressNotificationRequest);

        #endregion

        #region Equals(ChargingProgressNotificationRequest)

        /// <summary>
        /// Compares two charging notification progress requests for equality.
        /// </summary>
        /// <param name="ChargingProgressNotificationRequest">A charging notification progress request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(ChargingProgressNotificationRequest? ChargingProgressNotificationRequest)

            => ChargingProgressNotificationRequest is not null &&

               Type.          Equals(ChargingProgressNotificationRequest.Type)           &&
               SessionId.     Equals(ChargingProgressNotificationRequest.SessionId)      &&
               Identification.Equals(ChargingProgressNotificationRequest.Identification) &&
               EVSEId.        Equals(ChargingProgressNotificationRequest.EVSEId)         &&
               ChargingStart. Equals(ChargingProgressNotificationRequest.ChargingStart)  &&
               EventOcurred.  Equals(ChargingProgressNotificationRequest.EventOcurred);

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
                return Type.                   GetHashCode()       * 47 ^
                       SessionId.              GetHashCode()       * 43 ^
                       Identification.         GetHashCode()       * 41 ^
                       EVSEId.                 GetHashCode()       * 37 ^
                       ChargingStart.          GetHashCode()       * 31 ^
                       EventOcurred.           GetHashCode()       * 29 ^

                      (CPOPartnerSessionId?.   GetHashCode() ?? 0) * 23 ^
                      (EMPPartnerSessionId?.   GetHashCode() ?? 0) * 19 ^
                      (ChargingDuration?.      GetHashCode() ?? 0) * 17 ^
                      (SessionStart?.          GetHashCode() ?? 0) * 13 ^
                      (ConsumedEnergyProgress?.GetHashCode() ?? 0) * 11 ^
                      (MeterValueStart?.       GetHashCode() ?? 0) *  7 ^
                      (MeterValuesInBetween?.  GetHashCode() ?? 0) *  5 ^
                      (OperatorId?.            GetHashCode() ?? 0) *  3 ^
                      (PartnerProductId?.      GetHashCode() ?? 0);
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

                             " at " + EventOcurred.ToIso8601());

        #endregion

    }

}
