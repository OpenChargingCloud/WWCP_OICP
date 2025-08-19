/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The ChargingProgressNotification request.
    /// </summary>
    /// <remarks>
    /// Hubject has a rate limit of 1 request per 5 minutes!
    /// </remarks>
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
        /// The optional session identification assigned by the CPO partner.
        /// </summary>
        [Optional]
        public CPOPartnerSession_Id?             CPOPartnerSessionId                { get; }

        /// <summary>
        /// The optional session identification assigned by the EMP partner.
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
        public DateTimeOffset                    ChargingStart                      { get; }

        /// <summary>
        /// The timestamp when the charging progress parameters had been captured.
        /// </summary>
        [Mandatory]
        public DateTimeOffset                    EventOccurred                      { get; }

        /// <summary>
        /// Charging Duration = EventOccurred - Charging Start.
        /// </summary>
        [Optional]
        public TimeSpan?                         ChargingDuration                   { get; }

        /// <summary>
        /// The timestamp when the charging session started.
        /// </summary>
        [Optional]
        public DateTimeOffset?                   SessionStart                       { get; }

        /// <summary>
        /// The optional consumed energy till now.
        /// </summary>
        [Optional]
        public WattHour?                         ConsumedEnergyProgress             { get; }

        /// <summary>
        /// The optional starting value of the energy meter [kWh].
        /// </summary>
        [Optional]
        public WattHour?                         MeterValueStart                    { get; }

        /// <summary>
        /// The optional enumeration of meter values during the charging session.
        /// </summary>
        [Optional]
        public IEnumerable<WattHour>?            MeterValuesInBetween               { get; }

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
        /// <param name="EventOccurred">The timestamp when the charging progress parameters had been captured.</param>
        /// 
        /// <param name="CPOPartnerSessionId">An optional session identification assigned by the CPO partner.</param>
        /// <param name="EMPPartnerSessionId">An optional session identification assigned by the EMP partner.</param>
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
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public ChargingProgressNotificationRequest(Session_Id              SessionId,
                                                   Identification          Identification,
                                                   EVSE_Id                 EVSEId,
                                                   DateTimeOffset          ChargingStart,
                                                   DateTimeOffset          EventOccurred,

                                                   CPOPartnerSession_Id?   CPOPartnerSessionId      = null,
                                                   EMPPartnerSession_Id?   EMPPartnerSessionId      = null,
                                                   TimeSpan?               ChargingDuration         = null,
                                                   DateTimeOffset?         SessionStart             = null,
                                                   WattHour?               ConsumedEnergyProgress   = null,
                                                   WattHour?               MeterValueStart          = null,
                                                   IEnumerable<WattHour>?  MeterValuesInBetween     = null,
                                                   Operator_Id?            OperatorId               = null,
                                                   PartnerProduct_Id?      PartnerProductId         = null,
                                                   Process_Id?             ProcessId                = null,
                                                   JObject?                CustomData               = null,

                                                   DateTimeOffset?         Timestamp                = null,
                                                   EventTracking_Id?       EventTrackingId          = null,
                                                   TimeSpan?               RequestTimeout           = null,
                                                   CancellationToken       CancellationToken        = default)

            : base(ProcessId,
                   CustomData,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.Type                    = ChargingNotificationTypes.Progress;
            this.SessionId               = SessionId;
            this.Identification          = Identification;
            this.EVSEId                  = EVSEId;
            this.ChargingStart           = ChargingStart;
            this.EventOccurred           = EventOccurred;

            this.CPOPartnerSessionId     = CPOPartnerSessionId;
            this.EMPPartnerSessionId     = EMPPartnerSessionId;
            this.ChargingDuration        = ChargingDuration;
            this.SessionStart            = SessionStart;
            this.ConsumedEnergyProgress  = ConsumedEnergyProgress;
            this.MeterValueStart         = MeterValueStart;
            this.MeterValuesInBetween    = MeterValuesInBetween;
            this.OperatorId              = OperatorId;
            this.PartnerProductId        = PartnerProductId;


            unchecked
            {

                hashCode = this.Type.                   GetHashCode()       * 47 ^
                           this.SessionId.              GetHashCode()       * 43 ^
                           this.Identification.         GetHashCode()       * 41 ^
                           this.EVSEId.                 GetHashCode()       * 37 ^
                           this.ChargingStart.          GetHashCode()       * 31 ^
                           this.EventOccurred.          GetHashCode()       * 29 ^
                          (this.CPOPartnerSessionId?.   GetHashCode() ?? 0) * 23 ^
                          (this.EMPPartnerSessionId?.   GetHashCode() ?? 0) * 19 ^
                          (this.ChargingDuration?.      GetHashCode() ?? 0) * 17 ^
                          (this.SessionStart?.          GetHashCode() ?? 0) * 13 ^
                          (this.ConsumedEnergyProgress?.GetHashCode() ?? 0) * 11 ^
                          (this.MeterValueStart?.       GetHashCode() ?? 0) *  7 ^
                          (this.MeterValuesInBetween?.  GetHashCode() ?? 0) *  5 ^
                          (this.OperatorId?.            GetHashCode() ?? 0) *  3 ^
                          (this.PartnerProductId?.      GetHashCode() ?? 0);

            }

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#62-eroamingchargingnotifications-progress

        // {
        //     "CPOPartnerSessionID":    "1234XYZ",
        //     "ChargingStart":          "2020-09-23T14:17:53.038Z",
        //     "EventOccurred":          "2020-09-23T14:25:53.038Z",
        //     "ChargingDuration":        48000,
        //     "ConsumedEnergyProgress":  9,
        //     "EMPPartnerSessionID":    "2345ABC",
        //     "EvseID":                 "DE*XYZ*ETEST1",
        //     "Identification": {
        //         "RFIDMifareFamilyIdentification": {
        //             "UID":            "1234ABCD"
        //         }
        //     },
        //     "MeterValueStart":         0,
        //     "MeterValueInBetween": {
        //         "meterValues": [
        //             9
        //         ]
        //     },
        //     "PartnerProductID":       "AC 1",
        //     "OperatorID":             "DE*ABC",
        //     "SessionID":              "f98efba4-02d8-4fa0-b810-9a9d50d2c527",
        //     "SessionStart":           "2020-09-23T14:17:53.038Z",
        //     "Type":                   "Progress"
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomChargingProgressNotificationRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a charging notification progress request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingProgressNotificationRequestParser">A delegate to parse custom charging notification progress request JSON objects.</param>
        public static ChargingProgressNotificationRequest Parse(JObject                                                            JSON,
                                                                Process_Id?                                                        ProcessId                                         = null,

                                                                DateTimeOffset?                                                    Timestamp                                         = null,
                                                                EventTracking_Id?                                                  EventTrackingId                                   = null,
                                                                TimeSpan?                                                          RequestTimeout                                    = null,
                                                                CustomJObjectParserDelegate<ChargingProgressNotificationRequest>?  CustomChargingProgressNotificationRequestParser   = null,
                                                                CancellationToken                                                  CancellationToken                                 = default)
        {

            if (TryParse(JSON,
                         out var chargingProgressNotificationRequest,
                         out var errorResponse,
                         ProcessId,
                         Timestamp,
                         EventTrackingId,
                         RequestTimeout,
                         CustomChargingProgressNotificationRequestParser,
                         CancellationToken))
            {
                return chargingProgressNotificationRequest;
            }

            throw new ArgumentException("The given JSON representation of a charging notification progress request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out ChargingProgressNotificationRequest, out ErrorResponse, ..., CustomChargingProgressNotificationRequestParser = null)

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
                                       [NotNullWhen(true)]  out ChargingProgressNotificationRequest?      ChargingProgressNotificationRequest,
                                       [NotNullWhen(false)] out String?                                   ErrorResponse,
                                       Process_Id?                                                        ProcessId                                         = null,

                                       DateTimeOffset?                                                    Timestamp                                         = null,
                                       EventTracking_Id?                                                  EventTrackingId                                   = null,
                                       TimeSpan?                                                          RequestTimeout                                    = null,
                                       CustomJObjectParserDelegate<ChargingProgressNotificationRequest>?  CustomChargingProgressNotificationRequestParser   = null,
                                       CancellationToken                                                  CancellationToken                                 = default)
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

                if (!JSON.ParseMandatory("Type",
                                         "charging notifications type",
                                         ChargingNotificationTypesExtensions.TryParse,
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

                if (!JSON.ParseMandatoryJSON("Identification",
                                             "identification",
                                             OICPv2_3.Identification.TryParse,
                                             out Identification? Identification,
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

                #region Parse EventOccurred             [mandatory]

                if (!JSON.ParseMandatory("EventOccurred",
                                         "event occurred",
                                         out DateTime EventOccurred,
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
                                       WattHour.TryParseKWh,
                                       out WattHour? ConsumedEnergyProgress,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse MeterValueStart           [optional]

                if (JSON.ParseOptional("MeterValueStart",
                                       "meter value start",
                                       WattHour.TryParseKWh,
                                       out WattHour? MeterValueStart,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse MeterValuesInBetween      [optional]

                List<WattHour>? MeterValuesInBetween = null;

                if (JSON.ParseOptional("MeterValueInBetween",
                                       "meter values in between",
                                       out JObject MeterValuesInBetweenJSON,
                                       out ErrorResponse))
                {

                    if (ErrorResponse is not null)
                        return false;

                    if (MeterValuesInBetweenJSON["meterValues"] is JArray meterValuesArray)
                    {

                        MeterValuesInBetween = [];

                        foreach (var meterValueJSON in meterValuesArray)
                        {
                            if (WattHour.TryParseKWh(meterValueJSON.ToString(), out var meterValue))
                                MeterValuesInBetween.Add(meterValue);
                        }

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

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                ChargingProgressNotificationRequest = new ChargingProgressNotificationRequest(

                                                          SessionId,
                                                          Identification,
                                                          EVSEId,
                                                          ChargingStart,
                                                          EventOccurred,

                                                          CPOPartnerSessionId,
                                                          EMPPartnerSessionId,
                                                          ChargingDuration,
                                                          SessionStart,
                                                          ConsumedEnergyProgress,
                                                          MeterValueStart,
                                                          MeterValuesInBetween,
                                                          OperatorId,
                                                          PartnerProductId,
                                                          ProcessId,
                                                          customData,

                                                          Timestamp,
                                                          EventTrackingId,
                                                          RequestTimeout,
                                                          CancellationToken

                                                      );

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

        #region ToJSON(CustomChargingProgressNotificationRequestSerializer = null, CustomIdentificationSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomChargingProgressNotificationRequestSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ChargingProgressNotificationRequest>?  CustomChargingProgressNotificationRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<Identification>?                       CustomIdentificationSerializer                        = null)
        {

            var json = JSONObject.Create(
                           new JProperty("Type",                          Type.                     AsString()),
                           new JProperty("SessionID",                     SessionId.                ToString()),
                           new JProperty("EvseID",                        EVSEId.                   ToString()),
                           new JProperty("Identification",                Identification.           ToJSON(CustomIdentificationSerializer: CustomIdentificationSerializer)),
                           new JProperty("ChargingStart",                 ChargingStart.            ToISO8601()),
                           new JProperty("EventOccurred",                 EventOccurred.            ToISO8601()),

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
                               ? new JProperty("SessionStart",            SessionStart.       Value.ToISO8601())
                               : null,

                           ConsumedEnergyProgress.HasValue
                               ? new JProperty("ConsumedEnergyProgress",  String.Format("{0:0.###}", ConsumedEnergyProgress.Value.kWh).Replace(",", "."))
                               : null,

                           MeterValueStart.    HasValue
                               ? new JProperty("MeterValueStart",         String.Format("{0:0.###}", MeterValueStart.       Value.kWh).Replace(",", "."))
                               : null,

                           MeterValuesInBetween is not null && MeterValuesInBetween.Any()
                               ? new JProperty("MeterValueInBetween",
                                     new JObject(  // OICP is crazy!
                                         new JProperty("meterValues",     new JArray(MeterValuesInBetween.
                                                                                         Select(meterValue => String.Format("{0:0.###}", meterValue.kWh).Replace(",", ".")))
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
                       ? CustomChargingProgressNotificationRequestSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this ChargingProgressNotification request.
        /// </summary>
        public ChargingProgressNotificationRequest Clone()

            => new (

                   SessionId.              Clone(),
                   Identification.         Clone(),
                   EVSEId.                 Clone(),
                   ChargingStart,
                   EventOccurred,

                   CPOPartnerSessionId?.   Clone(),
                   EMPPartnerSessionId?.   Clone(),
                   ChargingDuration,
                   SessionStart,
                   ConsumedEnergyProgress?.Clone(),
                   MeterValueStart?.       Clone(),
                   MeterValuesInBetween?.  Select(meterValueInBetween => meterValueInBetween.Clone()),
                   OperatorId?.            Clone(),
                   PartnerProductId?.      Clone(),
                   ProcessId?.             Clone(),

                   CustomData is not null
                       ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))
                       : null,

                   Timestamp,
                   EventTrackingId.        Clone(),
                   RequestTimeout,
                   CancellationToken

               );

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

            if (ReferenceEquals(ChargingProgressNotification1, ChargingProgressNotification2))
                return true;

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
        /// <returns>True if both match; False otherwise.</returns>
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
               EventOccurred. Equals(ChargingProgressNotificationRequest.EventOccurred);

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

            => String.Concat(

                   $"{Type} at {EVSEId} for {Identification} ({SessionId})",

                   PartnerProductId.HasValue
                       ? $" of {PartnerProductId}"
                       : null,

                   $" at {EventOccurred}"

               );

        #endregion

    }

}
