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
        /// The charge point operator identification.
        /// </summary>
        [Mandatory]
        public Operator_Id                OperatorId                { get; }

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
        public String?                    ErrorAdditionalInfo       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new ChargingErrorNotification request.
        /// </summary>
        /// <param name="SessionId">The Hubject session identification, that identifies the charging process.</param>
        /// <param name="Identification">The authentication data used to authorize the user or the car.</param>
        /// <param name="OperatorId">The charge point operator identification.</param>
        /// <param name="EVSEId">The EVSE identification, that identifies the location of the charging process.</param>
        /// <param name="ErrorType">The error class.</param>
        /// 
        /// <param name="CPOPartnerSessionId">An optional session identification assinged by the CPO partner.</param>
        /// <param name="EMPPartnerSessionId">An optional session identification assinged by the EMP partner.</param>
        /// <param name="ErrorAdditionalInfo">Additional information about the error.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public ChargingErrorNotificationRequest(Session_Id             SessionId,
                                                Identification         Identification,
                                                Operator_Id            OperatorId,
                                                EVSE_Id                EVSEId,
                                                ErrorClassTypes        ErrorType,

                                                CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                                EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                                String?                ErrorAdditionalInfo   = null,
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

            this.Type                 = ChargingNotificationTypes.Error;
            this.SessionId            = SessionId;
            this.Identification       = Identification;
            this.OperatorId           = OperatorId;
            this.EVSEId               = EVSEId;
            this.ErrorType            = ErrorType;

            this.CPOPartnerSessionId  = CPOPartnerSessionId;
            this.EMPPartnerSessionId  = EMPPartnerSessionId;
            this.ErrorAdditionalInfo  = ErrorAdditionalInfo;


            unchecked
            {

                hashCode = this.Type.                GetHashCode()       * 27 ^
                           this.SessionId.           GetHashCode()       * 23 ^
                           this.Identification.      GetHashCode()       * 17 ^
                           this.OperatorId.          GetHashCode()       * 13 ^
                           this.EVSEId.              GetHashCode()       * 11 ^
                           this.ErrorType.           GetHashCode()       *  7 ^
                          (this.CPOPartnerSessionId?.GetHashCode() ?? 0) *  5 ^
                          (this.EMPPartnerSessionId?.GetHashCode() ?? 0) *  3 ^
                          (this.ErrorAdditionalInfo?.GetHashCode() ?? 0);

            }

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#64-eroamingchargingnotifications-error

        // {
        //     "CPOPartnerSessionID":       "1234XYZ",
        //     "EMPPartnerSessionID":       "2345ABC",
        //     "EvseID":                    "DE*XYZ*ETEST1",
        //     "ErrorType":                 "Connector Error",
        //     "ErrorAdditionalInfo":       "Plug was not connected, EVSEID timed out reached",
        //     "Identification": {
        //         "RFIDMifareFamilyIdentification": {
        //             "UID":               "1234ABCD"
        //         }
        //     },
        //     "OperatorID":                "DE*ABC",
        //     "SessionID":                 "f98efba4-02d8-4fa0-b810-9a9d50d2c527",
        //     "Type":                      "Error"
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomChargingErrorNotificationRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a charging notification error request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomChargingErrorNotificationRequestParser">A delegate to parse custom charging notification error request JSON objects.</param>
        public static ChargingErrorNotificationRequest Parse(JObject                                                         JSON,
                                                             Process_Id?                                                     ProcessId                                      = null,

                                                             DateTime?                                                       Timestamp                                      = null,
                                                             EventTracking_Id?                                               EventTrackingId                                = null,
                                                             TimeSpan?                                                       RequestTimeout                                 = null,
                                                             CustomJObjectParserDelegate<ChargingErrorNotificationRequest>?  CustomChargingErrorNotificationRequestParser   = null,
                                                             CancellationToken                                               CancellationToken                              = default)
        {

            if (TryParse(JSON,
                         out var chargingErrorNotificationRequest,
                         out var errorResponse,
                         ProcessId,
                         Timestamp,
                         EventTrackingId,
                         RequestTimeout,
                         CustomChargingErrorNotificationRequestParser,
                         CancellationToken))
            {
                return chargingErrorNotificationRequest;
            }

            throw new ArgumentException("The given JSON representation of a charging notification error request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out ChargingErrorNotificationRequest, out ErrorResponse, ..., CustomChargingErrorNotificationRequestParser = null)

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
        public static Boolean TryParse(JObject                                                         JSON,
                                       [NotNullWhen(true)]  out ChargingErrorNotificationRequest?      ChargingErrorNotificationRequest,
                                       [NotNullWhen(false)] out String?                                ErrorResponse,
                                       Process_Id?                                                     ProcessId                                      = null,

                                       DateTime?                                                       Timestamp                                      = null,
                                       EventTracking_Id?                                               EventTrackingId                                = null,
                                       TimeSpan?                                                       RequestTimeout                                 = null,
                                       CustomJObjectParserDelegate<ChargingErrorNotificationRequest>?  CustomChargingErrorNotificationRequestParser   = null,
                                       CancellationToken                                               CancellationToken                              = default)
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

                #region Parse OperatorId                [mandatory]

                if (!JSON.ParseMandatory("OperatorID",
                                         "EVSE identification",
                                         Operator_Id.TryParse,
                                         out Operator_Id OperatorId,
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
                                         ErrorClassTypesExtensions.TryParse,
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

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                ChargingErrorNotificationRequest = new ChargingErrorNotificationRequest(
                                                       SessionId,
                                                       Identification,
                                                       OperatorId,
                                                       EVSEId,
                                                       ErrorType,

                                                       CPOPartnerSessionId,
                                                       EMPPartnerSessionId,
                                                       ErrorAdditionalInfo,
                                                       ProcessId,

                                                       customData,

                                                       Timestamp,
                                                       EventTrackingId,
                                                       RequestTimeout,
                                                       CancellationToken
                                                   );

                if (CustomChargingErrorNotificationRequestParser is not null)
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

        #region ToJSON(CustomChargingErrorNotificationRequestSerializer = null, CustomIdentificationSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomChargingErrorNotificationRequestSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ChargingErrorNotificationRequest>?  CustomChargingErrorNotificationRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<Identification>?                    CustomIdentificationSerializer                     = null)
        {

            var json = JSONObject.Create(
                           new JProperty("Type",                        Type.                     AsString()),
                           new JProperty("SessionID",                   SessionId.                ToString()),
                           new JProperty("EvseID",                      EVSEId.                   ToString()),
                           new JProperty("Identification",              Identification.           ToJSON(CustomIdentificationSerializer: CustomIdentificationSerializer)),
                           new JProperty("ErrorType",                   ErrorType.                AsString()),
                           new JProperty("OperatorID",                  OperatorId.               ToString()),

                           CPOPartnerSessionId.   HasValue
                               ? new JProperty("CPOPartnerSessionID",   CPOPartnerSessionId.Value.ToString())
                               : null,

                           EMPPartnerSessionId.   HasValue
                               ? new JProperty("EMPPartnerSessionID",   EMPPartnerSessionId.Value.ToString())
                               : null,

                           ErrorAdditionalInfo.IsNotNullOrEmpty()
                               ? new JProperty("ErrorAdditionalInfo",   ErrorAdditionalInfo)
                               : null,

                           CustomData is not null
                               ? new JProperty("CustomData",            CustomData)
                               : null

                       );

            return CustomChargingErrorNotificationRequestSerializer is not null
                       ? CustomChargingErrorNotificationRequestSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this request.
        /// </summary>
        public ChargingErrorNotificationRequest Clone

            => new (SessionId,
                    Identification,
                    OperatorId,
                    EVSEId,
                    ErrorType,

                    CPOPartnerSessionId,
                    EMPPartnerSessionId,
                    ErrorAdditionalInfo,
                    ProcessId,
                    CustomData,

                    Timestamp,
                    EventTrackingId,
                    RequestTimeout,
                    CancellationToken);

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

            if (ReferenceEquals(ChargingErrorNotification1, ChargingErrorNotification2))
                return true;

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
        public override Boolean Equals(Object? Object)

            => Object is ChargingErrorNotificationRequest chargingErrorNotificationRequest &&
                   Equals(chargingErrorNotificationRequest);

        #endregion

        #region Equals(ChargingErrorNotificationRequest)

        /// <summary>
        /// Compares two charging notification error requests for equality.
        /// </summary>
        /// <param name="ChargingErrorNotificationRequest">A charging notification error request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(ChargingErrorNotificationRequest? ChargingErrorNotificationRequest)

            => ChargingErrorNotificationRequest is not null &&

               Type.          Equals(ChargingErrorNotificationRequest.Type)           &&
               SessionId.     Equals(ChargingErrorNotificationRequest.SessionId)      &&
               Identification.Equals(ChargingErrorNotificationRequest.Identification) &&
               OperatorId.    Equals(ChargingErrorNotificationRequest.OperatorId)     &&
               EVSEId.        Equals(ChargingErrorNotificationRequest.EVSEId)         &&
               ErrorType.     Equals(ChargingErrorNotificationRequest.ErrorType);

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

                   $"{Type} at {EVSEId} ({OperatorId}) for {Identification} ({SessionId}) => {ErrorType.AsString()}",

                   ErrorAdditionalInfo.IsNotNullOrEmpty()
                       ? $": '{ErrorAdditionalInfo}'"
                       : ""

               );

        #endregion

    }

}
