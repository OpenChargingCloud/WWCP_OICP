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
    /// The ChargingNotificationsStart request.
    /// </summary>
    public class ChargingNotificationsStartRequest : ARequest<ChargingNotificationsStartRequest>
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
        /// An optional session identification assinged by the CPO partner.
        /// </summary>
        [Optional]
        public CPOPartnerSession_Id?             CPOPartnerSessionId                { get; }

        /// <summary>
        /// An optional session identification assinged by the EMP partner.
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
        /// The timestamp when the charging session started.
        /// </summary>
        [Optional]
        public DateTime?                         SessionStart                       { get; }

        /// <summary>
        /// The timestamp when the charging process started.
        /// </summary>
        [Mandatory]
        public DateTime                          ChargingStart                      { get; }

        /// <summary>
        /// An optional initial value of the energy meter [kWh].
        /// </summary>
        [Optional]
        public Decimal?                          MeterValueStart                    { get; }

        /// <summary>
        /// An optional operator identification.
        /// </summary>
        [Optional]
        public Operator_Id?                      OperatorId                         { get; }

        /// <summary>
        /// An optional pricing product name (for identifying a tariff) that must be unique.
        /// </summary>
        [Optional]
        public PartnerProduct_Id?                PartnerProductId                   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new ChargingNotifications request.
        /// </summary>
        /// <param name="SessionId">The Hubject session identification, that identifies the charging process.</param>
        /// <param name="Identification">The authentication data used to authorize the user or the car.</param>
        /// <param name="EVSEId">The EVSE identification, that identifies the location of the charging process.</param>
        /// <param name="ChargingStart">The timestamp when the charging process started.</param>
        /// 
        /// <param name="CPOPartnerSessionId">An optional session identification assinged by the CPO partner.</param>
        /// <param name="EMPPartnerSessionId">An optional session identification assinged by the EMP partner.</param>
        /// <param name="SessionStart">An optional timestamp when the charging session started.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter [kWh].</param>
        /// <param name="OperatorId">An optional operator identification of the hub operator.</param>
        /// <param name="PartnerProductId">An optional pricing product name (for identifying a tariff) that must be unique.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public ChargingNotificationsStartRequest(Session_Id             SessionId,
                                                 Identification         Identification,
                                                 EVSE_Id                EVSEId,
                                                 DateTime               ChargingStart,

                                                 CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                                 EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                                 DateTime?              SessionStart          = null,
                                                 Decimal?               MeterValueStart       = null,
                                                 Operator_Id?           OperatorId            = null,
                                                 PartnerProduct_Id?     PartnerProductId      = null,

                                                 DateTime?              Timestamp             = null,
                                                 CancellationToken?     CancellationToken     = null,
                                                 EventTracking_Id       EventTrackingId       = null,
                                                 TimeSpan?              RequestTimeout        = null)

            : base(Timestamp,
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

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#eRoamingChargingNotificationsstart

        // {
        //     Swagger file is missing!
        // }

        #endregion

        #region (static) Parse   (JSON, CustomChargingNotificationsStartRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a charging notification start request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingNotificationsStartRequestParser">A delegate to parse custom charging notification start request JSON objects.</param>
        public static ChargingNotificationsStartRequest Parse(JObject                                                         JSON,
                                                              TimeSpan                                                        RequestTimeout,
                                                              DateTime?                                                       Timestamp                                       = null,
                                                              EventTracking_Id                                                EventTrackingId                                 = null,
                                                              CustomJObjectParserDelegate<ChargingNotificationsStartRequest>  CustomChargingNotificationsStartRequestParser   = null)
        {

            if (TryParse(JSON,
                         RequestTimeout,
                         out ChargingNotificationsStartRequest  chargingNotificationsStartRequest,
                         out String                             ErrorResponse,
                         Timestamp,
                         EventTrackingId,
                         CustomChargingNotificationsStartRequestParser))
            {
                return chargingNotificationsStartRequest;
            }

            throw new ArgumentException("The given JSON representation of a charging notification start request is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomChargingNotificationsStartRequestParser = null)

        /// <summary>
        /// Parse the given text representation of a charging notification start request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingNotificationsStartRequestParser">A delegate to parse custom charging notification start request JSON objects.</param>
        public static ChargingNotificationsStartRequest Parse(String                                                          Text,
                                                              TimeSpan                                                        RequestTimeout,
                                                              DateTime?                                                       Timestamp                                       = null,
                                                              EventTracking_Id                                                EventTrackingId                                 = null,
                                                              CustomJObjectParserDelegate<ChargingNotificationsStartRequest>  CustomChargingNotificationsStartRequestParser   = null)
        {

            if (TryParse(Text,
                         RequestTimeout,
                         out ChargingNotificationsStartRequest  chargingNotificationsStartRequest,
                         out String                             ErrorResponse,
                         Timestamp,
                         EventTrackingId,
                         CustomChargingNotificationsStartRequestParser))
            {
                return chargingNotificationsStartRequest;
            }

            throw new ArgumentException("The given text representation of a charging notification start request is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out ChargingNotificationsStartRequest, out ErrorResponse, CustomChargingNotificationsStartRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a charging notification start request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="ChargingNotificationsStartRequest">The parsed charging notification start request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingNotificationsStartRequestParser">A delegate to parse custom charging notification start request JSON objects.</param>
        public static Boolean TryParse(JObject                                                         JSON,
                                       TimeSpan                                                        RequestTimeout,
                                       out ChargingNotificationsStartRequest                           ChargingNotificationsStartRequest,
                                       out String                                                      ErrorResponse,
                                       DateTime?                                                       Timestamp                                       = null,
                                       EventTracking_Id                                                EventTrackingId                                 = null,
                                       CustomJObjectParserDelegate<ChargingNotificationsStartRequest>  CustomChargingNotificationsStartRequestParser   = null)
        {

            try
            {

                ChargingNotificationsStartRequest = default;

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
                    if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse MeterValueStart        [optional]

                if (JSON.ParseOptional("MeterValueStart",
                                       "meter value start",
                                       out Decimal? MeterValueStart,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion


                ChargingNotificationsStartRequest = new ChargingNotificationsStartRequest(SessionId,
                                                                                          Identification,
                                                                                          EVSEId,
                                                                                          ChargingStart,

                                                                                          CPOPartnerSessionId,
                                                                                          EMPPartnerSessionId,
                                                                                          SessionStart,
                                                                                          MeterValueStart,
                                                                                          OperatorId,
                                                                                          PartnerProductId,

                                                                                          Timestamp,
                                                                                          null,
                                                                                          EventTrackingId,
                                                                                          RequestTimeout);

                if (CustomChargingNotificationsStartRequestParser != null)
                    ChargingNotificationsStartRequest = CustomChargingNotificationsStartRequestParser(JSON,
                                                                                                      ChargingNotificationsStartRequest);

                return true;

            }
            catch (Exception e)
            {
                ChargingNotificationsStartRequest  = default;
                ErrorResponse                      = "The given JSON representation of a charging notification start request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out ChargingNotificationsStartRequest, out ErrorResponse, CustomChargingNotificationsStartRequestParser = null)

        /// <summary>
        /// Try to parse the given text representation of a charging notification start request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="ChargingNotificationsStartRequest">The parsed charging notification start request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingNotificationsStartRequestParser">A delegate to parse custom charging notification start request JSON objects.</param>
        public static Boolean TryParse(String                                                          Text,
                                       TimeSpan                                                        RequestTimeout,
                                       out ChargingNotificationsStartRequest                           ChargingNotificationsStartRequest,
                                       out String                                                      ErrorResponse,
                                       DateTime?                                                       Timestamp                                       = null,
                                       EventTracking_Id                                                EventTrackingId                                 = null,
                                       CustomJObjectParserDelegate<ChargingNotificationsStartRequest>  CustomChargingNotificationsStartRequestParser   = null)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                RequestTimeout,
                                out ChargingNotificationsStartRequest,
                                out ErrorResponse,
                                Timestamp,
                                EventTrackingId,
                                CustomChargingNotificationsStartRequestParser);

            }
            catch (Exception e)
            {
                ChargingNotificationsStartRequest  = default;
                ErrorResponse                      = "The given text representation of a charging notification start request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomChargingNotificationsStartRequestSerializer = null, CustomIdentificationSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomChargingNotificationsStartRequestSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ChargingNotificationsStartRequest>  CustomChargingNotificationsStartRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<Identification>                     CustomIdentificationSerializer                      = null)
        {

            var JSON = JSONObject.Create(
                           new JProperty("Type",                       Type.AsString()),
                           new JProperty("SessionID",                  SessionId.          ToString()),
                           new JProperty("EvseID",                     EVSEId.             ToString()),
                           new JProperty("Identification",             Identification.     ToJSON(CustomIdentificationSerializer: CustomIdentificationSerializer)),
                           new JProperty("ChargingStart",              ChargingStart.      ToIso8601()),


                           SessionStart.HasValue
                               ? new JProperty("SessionStart",         SessionStart.Value.       ToIso8601())
                               : null,

                           PartnerProductId.   HasValue
                               ? new JProperty("PartnerProductID",     PartnerProductId.   Value.ToString())
                               : null,

                           CPOPartnerSessionId.HasValue
                               ? new JProperty("CPOPartnerSessionID",  CPOPartnerSessionId.Value.ToString())
                               : null,

                           EMPPartnerSessionId.HasValue
                               ? new JProperty("EMPPartnerSessionID",  EMPPartnerSessionId.Value.ToString())
                               : null,

                           MeterValueStart.    HasValue
                               ? new JProperty("MeterValueStart",      String.Format("{0:0.###}", MeterValueStart.Value).Replace(",", "."))
                               : null,

                           OperatorId.HasValue
                               ? new JProperty("OperatorID",           OperatorId.Value.ToString())
                               : null,

                           CustomData != null
                               ? new JProperty("CustomData",           CustomData)
                               : null
                       );

            return CustomChargingNotificationsStartRequestSerializer != null
                       ? CustomChargingNotificationsStartRequestSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this request.
        /// </summary>
        public ChargingNotificationsStartRequest Clone

            => new ChargingNotificationsStartRequest(SessionId,
                                                     Identification,
                                                     EVSEId,
                                                     ChargingStart,

                                                     CPOPartnerSessionId,
                                                     EMPPartnerSessionId,
                                                     SessionStart,
                                                     MeterValueStart,
                                                     OperatorId,
                                                     PartnerProductId,

                                                     Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     RequestTimeout);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingNotificationsStart1, ChargingNotificationsStart2)

        /// <summary>
        /// Compares two charging notification start requests for equality.
        /// </summary>
        /// <param name="ChargingNotificationsStart1">A charging notification start request.</param>
        /// <param name="ChargingNotificationsStart2">Another charging notification start request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingNotificationsStartRequest ChargingNotificationsStart1,
                                           ChargingNotificationsStartRequest ChargingNotificationsStart2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingNotificationsStart1, ChargingNotificationsStart2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingNotificationsStart1 is null || ChargingNotificationsStart2 is null)
                return false;

            return ChargingNotificationsStart1.Equals(ChargingNotificationsStart2);

        }

        #endregion

        #region Operator != (ChargingNotificationsStart1, ChargingNotificationsStart2)

        /// <summary>
        /// Compares two charging notification start requests for inequality.
        /// </summary>
        /// <param name="ChargingNotificationsStart1">A charging notification start request.</param>
        /// <param name="ChargingNotificationsStart2">Another charging notification start request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingNotificationsStartRequest ChargingNotificationsStart1,
                                           ChargingNotificationsStartRequest ChargingNotificationsStart2)

            => !(ChargingNotificationsStart1 == ChargingNotificationsStart2);

        #endregion

        #endregion

        #region IEquatable<ChargingNotificationsStartRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is ChargingNotificationsStartRequest chargingNotificationsStartRequest &&
                   Equals(chargingNotificationsStartRequest);

        #endregion

        #region Equals(ChargingNotificationsStartRequest)

        /// <summary>
        /// Compares two charging notification start requests for equality.
        /// </summary>
        /// <param name="ChargingNotificationsStartRequest">A charging notification start request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(ChargingNotificationsStartRequest ChargingNotificationsStartRequest)

            => !(ChargingNotificationsStartRequest is null) &&

                 Type.          Equals(ChargingNotificationsStartRequest.Type)           &&
                 SessionId.     Equals(ChargingNotificationsStartRequest.SessionId)      &&
                 Identification.Equals(ChargingNotificationsStartRequest.Identification) &&
                 EVSEId.        Equals(ChargingNotificationsStartRequest.EVSEId)         &&
                 ChargingStart. Equals(ChargingNotificationsStartRequest.ChargingStart);

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
                       Identification.      GetHashCode()       * 27 ^
                       EVSEId.              GetHashCode()       * 21 ^
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