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
using System.Linq;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The ChargingEndNotification request.
    /// </summary>
    public class ChargingEndNotificationRequest : ARequest<ChargingEndNotificationRequest>
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
        /// The timestamp when the charging process stopped.
        /// </summary>
        [Mandatory]
        public DateTime                          ChargingEnd                        { get; }

        /// <summary>
        /// The timestamp when the charging session started.
        /// </summary>
        [Optional]
        public DateTime?                         SessionStart                       { get; }

        /// <summary>
        /// The timestamp when the charging session stopped.
        /// </summary>
        [Optional]
        public DateTime?                         SessionEnd                         { get; }

        /// <summary>
        /// The optional consumed energy.
        /// </summary>
        [Optional]
        public Decimal?                          ConsumedEnergy                     { get; }

        /// <summary>
        /// The optional starting value of the energy meter [kWh].
        /// </summary>
        [Optional]
        public Decimal?                          MeterValueStart                    { get; }

        /// <summary>
        /// The optional ending value of the energy meter [kWh].
        /// </summary>
        [Optional]
        public Decimal?                          MeterValueEnd                      { get; }

        /// <summary>
        /// The optional enumeration of meter values during the charging session.
        /// </summary>
        [Optional]
        public IEnumerable<Decimal>              MeterValuesInBetween               { get; }

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

        /// <summary>
        /// The optional timestamp when the penalty time start after the grace period.
        /// </summary>
        [Optional]
        public DateTime?                         PenaltyTimeStart                   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new ChargingNotificationsEnd request.
        /// </summary>
        /// <param name="SessionId">The Hubject session identification, that identifies the charging process.</param>
        /// <param name="Identification">The authentication data used to authorize the user or the car.</param>
        /// <param name="EVSEId">The EVSE identification, that identifies the location of the charging process.</param>
        /// <param name="ChargingStart">The timestamp when the charging process started.</param>
        /// <param name="ChargingEnd">The timestamp when the charging process stopped.</param>
        /// 
        /// <param name="CPOPartnerSessionId">An optional session identification assinged by the CPO partner.</param>
        /// <param name="EMPPartnerSessionId">An optional session identification assinged by the EMP partner.</param>
        /// <param name="SessionStart">An optional timestamp when the charging session started.</param>
        /// <param name="SessionEnd">An optional timestamp when the charging session stopped.</param>
        /// <param name="ConsumedEnergy">The optional consumed energy.</param>
        /// <param name="MeterValueStart">An optional starting value of the energy meter [kWh].</param>
        /// <param name="MeterValueEnd">An optional ending value of the energy meter [kWh].</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="OperatorId">An optional operator identification of the hub operator.</param>
        /// <param name="PartnerProductId">An optional pricing product name (for identifying a tariff) that must be unique.</param>
        /// <param name="PenaltyTimeStart">An optional timestamp when the penalty time start after the grace period.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public ChargingEndNotificationRequest(Session_Id             SessionId,
                                               Identification         Identification,
                                               EVSE_Id                EVSEId,
                                               DateTime               ChargingStart,
                                               DateTime               ChargingEnd,

                                               CPOPartnerSession_Id?  CPOPartnerSessionId      = null,
                                               EMPPartnerSession_Id?  EMPPartnerSessionId      = null,
                                               DateTime?              SessionStart             = null,
                                               DateTime?              SessionEnd               = null,
                                               Decimal?               ConsumedEnergy           = null,
                                               Decimal?               MeterValueStart          = null,
                                               Decimal?               MeterValueEnd            = null,
                                               IEnumerable<Decimal>   MeterValuesInBetween     = null,
                                               Operator_Id?           OperatorId               = null,
                                               PartnerProduct_Id?     PartnerProductId         = null,
                                               DateTime?              PenaltyTimeStart         = null,
                                               JObject                CustomData               = null,

                                               DateTime?              Timestamp                = null,
                                               CancellationToken?     CancellationToken        = null,
                                               EventTracking_Id       EventTrackingId          = null,
                                               TimeSpan?              RequestTimeout           = null)

            : base(CustomData,
                   Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.Type                    = ChargingNotificationTypes.End;
            this.SessionId               = SessionId;
            this.Identification          = Identification;
            this.EVSEId                  = EVSEId;
            this.ChargingStart           = ChargingStart;
            this.ChargingEnd             = ChargingEnd;

            this.CPOPartnerSessionId     = CPOPartnerSessionId;
            this.EMPPartnerSessionId     = EMPPartnerSessionId;
            this.SessionStart            = SessionStart;
            this.SessionEnd              = SessionEnd;
            this.ConsumedEnergy          = ConsumedEnergy;
            this.MeterValueStart         = MeterValueStart;
            this.MeterValueEnd           = MeterValueEnd;
            this.MeterValuesInBetween    = MeterValuesInBetween;
            this.OperatorId              = OperatorId;
            this.PartnerProductId        = PartnerProductId;
            this.PenaltyTimeStart        = PenaltyTimeStart;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#63-eroamingchargingnotifications-end

        // {
        //     Swagger file is missing!
        // }

        #endregion

        #region (static) Parse   (JSON, CustomChargingNotificationsEndRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a charging notification end request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingNotificationsEndRequestParser">A delegate to parse custom charging notification end request JSON objects.</param>
        public static ChargingEndNotificationRequest Parse(JObject                                                      JSON,
                                                           TimeSpan                                                     RequestTimeout,
                                                           DateTime?                                                    Timestamp                                     = null,
                                                           EventTracking_Id                                             EventTrackingId                               = null,
                                                           CustomJObjectParserDelegate<ChargingEndNotificationRequest>  CustomChargingNotificationsEndRequestParser   = null)
        {

            if (TryParse(JSON,
                         RequestTimeout,
                         out ChargingEndNotificationRequest  chargingNotificationsEndRequest,
                         out String                          ErrorResponse,
                         Timestamp,
                         EventTrackingId,
                         CustomChargingNotificationsEndRequestParser))
            {
                return chargingNotificationsEndRequest;
            }

            throw new ArgumentException("The given JSON representation of a charging notification end request is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomChargingNotificationsEndRequestParser = null)

        /// <summary>
        /// Parse the given text representation of a charging notification end request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingNotificationsEndRequestParser">A delegate to parse custom charging notification end request JSON objects.</param>
        public static ChargingEndNotificationRequest Parse(String                                                       Text,
                                                           TimeSpan                                                     RequestTimeout,
                                                           DateTime?                                                    Timestamp                                     = null,
                                                           EventTracking_Id                                             EventTrackingId                               = null,
                                                           CustomJObjectParserDelegate<ChargingEndNotificationRequest>  CustomChargingNotificationsEndRequestParser   = null)
        {

            if (TryParse(Text,
                         RequestTimeout,
                         out ChargingEndNotificationRequest  chargingNotificationsEndRequest,
                         out String                          ErrorResponse,
                         Timestamp,
                         EventTrackingId,
                         CustomChargingNotificationsEndRequestParser))
            {
                return chargingNotificationsEndRequest;
            }

            throw new ArgumentException("The given text representation of a charging notification end request is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out ChargingNotificationsEndRequest, out ErrorResponse, CustomChargingNotificationsEndRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a charging notification end request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="ChargingNotificationsEndRequest">The parsed charging notification end request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingNotificationsEndRequestParser">A delegate to parse custom charging notification end request JSON objects.</param>
        public static Boolean TryParse(JObject                                                      JSON,
                                       TimeSpan                                                     RequestTimeout,
                                       out ChargingEndNotificationRequest                           ChargingNotificationsEndRequest,
                                       out String                                                   ErrorResponse,
                                       DateTime?                                                    Timestamp                                     = null,
                                       EventTracking_Id                                             EventTrackingId                               = null,
                                       CustomJObjectParserDelegate<ChargingEndNotificationRequest>  CustomChargingNotificationsEndRequestParser   = null)
        {

            try
            {

                ChargingNotificationsEndRequest = default;

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

                #region Parse ChargingStart             [mandatory]

                if (!JSON.ParseMandatory("ChargingStart",
                                         "charging start",
                                         out DateTime ChargingStart,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ChargingEnd               [mandatory]

                if (!JSON.ParseMandatory("ChargingEnd",
                                         "charging end",
                                         out DateTime ChargingEnd,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse SessionStart              [optional]

                if (JSON.ParseOptional("SessionStart",
                                       "session start",
                                       out DateTime? SessionStart,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse SessionEnd                [optional]

                if (JSON.ParseOptional("SessionEnd",
                                       "session end",
                                       out DateTime? SessionEnd,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse ConsumedEnergy            [optional]

                if (JSON.ParseOptional("ConsumedEnergy",
                                       "consumed energy",
                                       out Decimal? ConsumedEnergy,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse MeterValueStart           [optional]

                if (JSON.ParseOptional("MeterValueStart",
                                       "meter value start",
                                       out Decimal? MeterValueStart,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse MeterValueEnd             [optional]

                if (JSON.ParseOptional("MeterValueEnd",
                                       "meter value end",
                                       out Decimal? MeterValueEnd,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
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

                    if (ErrorResponse != null)
                        return false;

                    if (MeterValuesInBetweenJSON.ParseOptionalJSON("meterValues",
                                                                   "meter values",
                                                                   (String input, out Decimal number) => Decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out number),
                                                                   out MeterValuesInBetween,
                                                                   out ErrorResponse))
                    {
                        if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse PenaltyTimeStart          [optional]

                if (JSON.ParseOptional("PenaltyTimeStart",
                                       "penalty time start",
                                       out DateTime? PenaltyTimeStart,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse CustomData                [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                ChargingNotificationsEndRequest = new ChargingEndNotificationRequest(SessionId,
                                                                                     Identification,
                                                                                     EVSEId,
                                                                                     ChargingStart,
                                                                                     ChargingEnd,

                                                                                     CPOPartnerSessionId,
                                                                                     EMPPartnerSessionId,
                                                                                     SessionStart,
                                                                                     SessionEnd,
                                                                                     ConsumedEnergy,
                                                                                     MeterValueStart,
                                                                                     MeterValueEnd,
                                                                                     MeterValuesInBetween,
                                                                                     OperatorId,
                                                                                     PartnerProductId,
                                                                                     PenaltyTimeStart,
                                                                                     CustomData,

                                                                                     Timestamp,
                                                                                     null,
                                                                                     EventTrackingId,
                                                                                     RequestTimeout);

                if (CustomChargingNotificationsEndRequestParser != null)
                    ChargingNotificationsEndRequest = CustomChargingNotificationsEndRequestParser(JSON,
                                                                                                  ChargingNotificationsEndRequest);

                return true;

            }
            catch (Exception e)
            {
                ChargingNotificationsEndRequest  = default;
                ErrorResponse                    = "The given JSON representation of a charging notification end request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out ChargingNotificationsEndRequest, out ErrorResponse, CustomChargingNotificationsEndRequestParser = null)

        /// <summary>
        /// Try to parse the given text representation of a charging notification end request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="ChargingNotificationsEndRequest">The parsed charging notification end request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingNotificationsEndRequestParser">A delegate to parse custom charging notification end request JSON objects.</param>
        public static Boolean TryParse(String                                                       Text,
                                       TimeSpan                                                     RequestTimeout,
                                       out ChargingEndNotificationRequest                           ChargingNotificationsEndRequest,
                                       out String                                                   ErrorResponse,
                                       DateTime?                                                    Timestamp                                     = null,
                                       EventTracking_Id                                             EventTrackingId                               = null,
                                       CustomJObjectParserDelegate<ChargingEndNotificationRequest>  CustomChargingNotificationsEndRequestParser   = null)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                RequestTimeout,
                                out ChargingNotificationsEndRequest,
                                out ErrorResponse,
                                Timestamp,
                                EventTrackingId,
                                CustomChargingNotificationsEndRequestParser);

            }
            catch (Exception e)
            {
                ChargingNotificationsEndRequest  = default;
                ErrorResponse                    = "The given text representation of a charging notification end request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomChargingNotificationsEndRequestSerializer = null, CustomIdentificationSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomChargingNotificationsEndRequestSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ChargingEndNotificationRequest>  CustomChargingNotificationsEndRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<Identification>                   CustomIdentificationSerializer                    = null)
        {

            var JSON = JSONObject.Create(
                           new JProperty("Type",                        Type.                     AsString()),
                           new JProperty("SessionID",                   SessionId.                ToString()),
                           new JProperty("EvseID",                      EVSEId.                   ToString()),
                           new JProperty("Identification",              Identification.           ToJSON(CustomIdentificationSerializer: CustomIdentificationSerializer)),
                           new JProperty("ChargingStart",               ChargingStart.            ToIso8601()),
                           new JProperty("ChargingEnd",                 ChargingEnd.              ToIso8601()),

                           CPOPartnerSessionId.   HasValue
                               ? new JProperty("CPOPartnerSessionID",   CPOPartnerSessionId.Value.ToString())
                               : null,

                           EMPPartnerSessionId.   HasValue
                               ? new JProperty("EMPPartnerSessionID",   EMPPartnerSessionId.Value.ToString())
                               : null,

                           SessionStart.          HasValue
                               ? new JProperty("SessionStart",          SessionStart.       Value.ToIso8601())
                               : null,

                           SessionEnd.            HasValue
                               ? new JProperty("SessionEnd",            SessionEnd.         Value.ToIso8601())
                               : null,

                           ConsumedEnergy.HasValue
                               ? new JProperty("ConsumedEnergy",        String.Format("{0:0.###}", ConsumedEnergy. Value).Replace(",", "."))
                               : null,

                           MeterValueStart.    HasValue
                               ? new JProperty("MeterValueStart",       String.Format("{0:0.###}", MeterValueStart.Value).Replace(",", "."))
                               : null,

                           MeterValueEnd.      HasValue
                               ? new JProperty("MeterValueEnd",         String.Format("{0:0.###}", MeterValueEnd.  Value).Replace(",", "."))
                               : null,

                           MeterValuesInBetween.SafeAny()
                               ? new JProperty("MeterValueInBetween",
                                     new JObject(  // OICP is crazy!
                                         new JProperty("meterValues",   new JArray(MeterValuesInBetween.
                                                                                       Select(meterValue => String.Format("{0:0.###}", meterValue).Replace(",", ".")))
                                         )
                                     )
                                 )
                               : null,

                           OperatorId.HasValue
                               ? new JProperty("OperatorID",            OperatorId.         Value.ToString())
                               : null,

                           PartnerProductId.   HasValue
                               ? new JProperty("PartnerProductID",      PartnerProductId.   Value.ToString())
                               : null,

                           PenaltyTimeStart.   HasValue
                               ? new JProperty("PenaltyTimeStart",      PenaltyTimeStart.   Value.ToIso8601())
                               : null,

                           CustomData != null
                               ? new JProperty("CustomData",            CustomData)
                               : null

                       );

            return CustomChargingNotificationsEndRequestSerializer != null
                       ? CustomChargingNotificationsEndRequestSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this request.
        /// </summary>
        public ChargingEndNotificationRequest Clone

            => new ChargingEndNotificationRequest(SessionId,
                                                   Identification,
                                                   EVSEId,
                                                   ChargingStart,
                                                   ChargingEnd,

                                                   CPOPartnerSessionId,
                                                   EMPPartnerSessionId,
                                                   SessionStart,
                                                   SessionEnd,
                                                   ConsumedEnergy,
                                                   MeterValueStart,
                                                   MeterValueEnd,
                                                   MeterValuesInBetween,
                                                   OperatorId,
                                                   PartnerProductId,
                                                   PenaltyTimeStart,
                                                   CustomData,

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingNotificationsEnd1, ChargingNotificationsEnd2)

        /// <summary>
        /// Compares two charging notification end requests for equality.
        /// </summary>
        /// <param name="ChargingNotificationsEnd1">A charging notification end request.</param>
        /// <param name="ChargingNotificationsEnd2">Another charging notification end request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingEndNotificationRequest ChargingNotificationsEnd1,
                                           ChargingEndNotificationRequest ChargingNotificationsEnd2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingNotificationsEnd1, ChargingNotificationsEnd2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingNotificationsEnd1 is null || ChargingNotificationsEnd2 is null)
                return false;

            return ChargingNotificationsEnd1.Equals(ChargingNotificationsEnd2);

        }

        #endregion

        #region Operator != (ChargingNotificationsEnd1, ChargingNotificationsEnd2)

        /// <summary>
        /// Compares two charging notification end requests for inequality.
        /// </summary>
        /// <param name="ChargingNotificationsEnd1">A charging notification end request.</param>
        /// <param name="ChargingNotificationsEnd2">Another charging notification end request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingEndNotificationRequest ChargingNotificationsEnd1,
                                           ChargingEndNotificationRequest ChargingNotificationsEnd2)

            => !(ChargingNotificationsEnd1 == ChargingNotificationsEnd2);

        #endregion

        #endregion

        #region IEquatable<ChargingNotificationsEndRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is ChargingEndNotificationRequest chargingNotificationsEndRequest &&
                   Equals(chargingNotificationsEndRequest);

        #endregion

        #region Equals(ChargingNotificationsEndRequest)

        /// <summary>
        /// Compares two charging notification end requests for equality.
        /// </summary>
        /// <param name="ChargingNotificationsEndRequest">A charging notification end request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(ChargingEndNotificationRequest ChargingNotificationsEndRequest)

            => !(ChargingNotificationsEndRequest is null) &&

                 Type.          Equals(ChargingNotificationsEndRequest.Type)           &&
                 SessionId.     Equals(ChargingNotificationsEndRequest.SessionId)      &&
                 Identification.Equals(ChargingNotificationsEndRequest.Identification) &&
                 EVSEId.        Equals(ChargingNotificationsEndRequest.EVSEId)         &&
                 ChargingStart. Equals(ChargingNotificationsEndRequest.ChargingStart)  &&
                 ChargingEnd.   Equals(ChargingNotificationsEndRequest.ChargingEnd);

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

                return Type.                   GetHashCode()       * 59 ^
                       SessionId.              GetHashCode()       * 53 ^
                       Identification.         GetHashCode()       * 47 ^
                       EVSEId.                 GetHashCode()       * 43 ^
                       ChargingStart.          GetHashCode()       * 41 ^
                       ChargingEnd.            GetHashCode()       * 37 ^

                      (CPOPartnerSessionId?.   GetHashCode() ?? 0) * 31 ^
                      (EMPPartnerSessionId?.   GetHashCode() ?? 0) * 29 ^
                      (SessionStart?.          GetHashCode() ?? 0) * 23 ^
                      (SessionEnd?.            GetHashCode() ?? 0) * 19 ^
                      (ConsumedEnergy?.        GetHashCode() ?? 0) * 17 ^
                      (MeterValueStart?.       GetHashCode() ?? 0) * 13 ^
                      (MeterValueEnd?.         GetHashCode() ?? 0) * 11 ^
                      (MeterValuesInBetween?.  GetHashCode() ?? 0) *  7 ^
                      (OperatorId?.            GetHashCode() ?? 0) *  5 ^
                      (PartnerProductId?.      GetHashCode() ?? 0) *  3 ^
                      (PenaltyTimeStart?.      GetHashCode() ?? 0);

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

                             " at " + ChargingEnd.ToIso8601());

        #endregion

    }

}
