﻿/*
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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The PullEVSEStatus response.
    /// </summary>
    public class PullEVSEStatusResponse : AResponse<PullEVSEStatusRequest,
                                                    PullEVSEStatusResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE status records grouped by their operators.
        /// </summary>
        [Mandatory]
        public IEnumerable<OperatorEVSEStatus>  OperatorEVSEStatus    { get; }

        /// <summary>
        /// The optional status code of this response.
        /// </summary>
        [Optional]
        public StatusCode?                      StatusCode            { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullEVSEStatus response.
        /// </summary>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="OperatorEVSEStatus">An enumeration of EVSE status records grouped by their operators.</param>
        /// 
        /// <param name="Request">An optional PullEVSEStatus request.</param>
        /// <param name="StatusCode">An optional status code of this response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public PullEVSEStatusResponse(DateTime                          ResponseTimestamp,
                                      EventTracking_Id                  EventTrackingId,
                                      Process_Id                        ProcessId,
                                      TimeSpan                          Runtime,
                                      IEnumerable<OperatorEVSEStatus>   OperatorEVSEStatus,

                                      PullEVSEStatusRequest?            Request        = null,
                                      StatusCode?                       StatusCode     = null,
                                      HTTPResponse?                     HTTPResponse   = null,
                                      JObject?                          CustomData     = null)

            : base(ResponseTimestamp,
                   EventTrackingId,
                   ProcessId,
                   Runtime,
                   Request,
                   HTTPResponse,
                   CustomData)

        {

            this.OperatorEVSEStatus  = OperatorEVSEStatus ?? throw new ArgumentNullException(nameof(OperatorEVSEStatus), "The given OperatorEVSEStatus must not be null or empty!");
            this.StatusCode          = StatusCode;

            unchecked
            {

                hashCode = this.OperatorEVSEStatus.CalcHashCode() * 3 ^
                           this.StatusCode?.       GetHashCode() ?? 0;

            }

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#eRoamingEVSEStatusmessage

        // {
        //   "EvseStatuses": {
        //     "OperatorEvseStatus": [
        //       {
        //         "EvseStatusRecord": [
        //           {
        //             "EvseID":      "string",
        //             "EvseStatus":  "Available"
        //           }
        //         ],
        //         "OperatorID":      "string",
        //         "OperatorName":    "string"
        //       }
        //     ]
        //   },
        //   "StatusCode": {
        //     "AdditionalInfo":      "string",
        //     "Code":                "000",
        //     "Description":         "string"
        //   }
        // }

        #endregion

        #region (static) Parse   (JSON, CustomPullEVSEStatusResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullEVSEStatus response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullEVSEStatusResponseParser">A delegate to parse custom PullEVSEStatus JSON objects.</param>
        public static PullEVSEStatusResponse Parse(PullEVSEStatusRequest                                 Request,
                                                   JObject                                               JSON,
                                                   DateTime                                              ResponseTimestamp,
                                                   EventTracking_Id                                      EventTrackingId,
                                                   TimeSpan                                              Runtime,
                                                   Process_Id?                                           ProcessId                            = null,
                                                   HTTPResponse?                                         HTTPResponse                         = null,
                                                   CustomJObjectParserDelegate<PullEVSEStatusResponse>?  CustomPullEVSEStatusResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         out var pullEVSEStatusResponse,
                         out var errorResponse,
                         ProcessId,
                         HTTPResponse,
                         CustomPullEVSEStatusResponseParser))
            {
                return pullEVSEStatusResponse;
            }

            throw new ArgumentException("The given JSON representation of a PullEVSEStatus response is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out PullEVSEStatusResponse, out ErrorResponse, CustomPullEVSEStatusResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PullEVSEStatus response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="PullEVSEStatusResponse">The parsed PullEVSEStatus response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullEVSEStatusResponseParser">A delegate to parse custom PullEVSEStatus response JSON objects.</param>
        public static Boolean TryParse(PullEVSEStatusRequest                                 Request,
                                       JObject                                               JSON,
                                       DateTime                                              ResponseTimestamp,
                                       EventTracking_Id                                      EventTrackingId,
                                       TimeSpan                                              Runtime,
                                       [NotNullWhen(true)]  out PullEVSEStatusResponse?      PullEVSEStatusResponse,
                                       [NotNullWhen(false)] out String?                      ErrorResponse,
                                       Process_Id?                                           ProcessId                            = null,
                                       HTTPResponse?                                         HTTPResponse                         = null,
                                       CustomJObjectParserDelegate<PullEVSEStatusResponse>?  CustomPullEVSEStatusResponseParser   = null)
        {

            try
            {

                PullEVSEStatusResponse = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse OperatorEVSEStatus    [mandatory]

                if (!JSON.ParseMandatory("EvseStatuses",
                                         "EVSE statuses",
                                         out JObject EVSEStatuses,
                                         out         ErrorResponse))
                {
                    return false;
                }

                if (!EVSEStatuses.ParseMandatoryJSON("OperatorEvseStatus",
                                                     "operator EVSE status",
                                                     OICPv2_3.OperatorEVSEStatus.TryParse,
                                                     out IEnumerable<OperatorEVSEStatus> OperatorEVSEStatus,
                                                     out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse StatusCode            [optional]

                if (JSON.ParseOptionalJSON("StatusCode",
                                           "StatusCode",
                                           OICPv2_3.StatusCode.TryParse,
                                           out StatusCode? StatusCode,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CustomData            [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                PullEVSEStatusResponse = new PullEVSEStatusResponse(

                                             ResponseTimestamp,
                                             EventTrackingId,
                                             ProcessId ?? Process_Id.NewRandom(),
                                             Runtime,
                                             OperatorEVSEStatus,

                                             Request,
                                             StatusCode,
                                             HTTPResponse,
                                             customData

                                         );

                if (CustomPullEVSEStatusResponseParser is not null)
                    PullEVSEStatusResponse = CustomPullEVSEStatusResponseParser(JSON,
                                                                                PullEVSEStatusResponse);

                return true;

            }
            catch (Exception e)
            {
                PullEVSEStatusResponse  = default;
                ErrorResponse           = "The given JSON representation of a PullEVSEStatus response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPullEVSEStatusResponseSerializer = null, CustomOperatorEVSEStatusSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEStatusResponseSerializer">A delegate to customize the serialization of PullEVSEStatusResponse responses.</param>
        /// <param name="CustomOperatorEVSEStatusSerializer">A delegate to serialize custom operator EVSE status JSON objects.</param>
        /// <param name="CustomEVSEStatusRecordSerializer">A delegate to serialize custom EVSE status record JSON objects.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullEVSEStatusResponse>?  CustomPullEVSEStatusResponseSerializer   = null,
                              CustomJObjectSerializerDelegate<OperatorEVSEStatus>?      CustomOperatorEVSEStatusSerializer       = null,
                              CustomJObjectSerializerDelegate<EVSEStatusRecord>?        CustomEVSEStatusRecordSerializer         = null,
                              CustomJObjectSerializerDelegate<StatusCode>?              CustomStatusCodeSerializer               = null)
        {

            var json = JSONObject.Create(

                           new JProperty("EvseStatuses", new JObject(
                               new JProperty("OperatorEvseStatus",  new JArray(OperatorEVSEStatus.Select(operatorEVSEStatus => operatorEVSEStatus.ToJSON(CustomOperatorEVSEStatusSerializer,
                                                                                                                                                         CustomEVSEStatusRecordSerializer))))
                           )),

                           StatusCode is not null
                               ? new JProperty("StatusCode",  StatusCode.ToJSON(CustomStatusCodeSerializer))
                               : null

                       );

            return CustomPullEVSEStatusResponseSerializer is not null
                       ? CustomPullEVSEStatusResponseSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="EVSEStatus1">An EVSE status.</param>
        /// <param name="EVSEStatus2">Another EVSE status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullEVSEStatusResponse EVSEStatus1,
                                           PullEVSEStatusResponse EVSEStatus2)
        {

            if (ReferenceEquals(EVSEStatus1, EVSEStatus2))
                return true;

            if ((EVSEStatus1 is null) || (EVSEStatus2 is null))
                return false;

            return EVSEStatus1.Equals(EVSEStatus2);

        }

        #endregion

        #region Operator != (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="EVSEStatus1">An EVSE status.</param>
        /// <param name="EVSEStatus2">Another EVSE status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullEVSEStatusResponse EVSEStatus1,
                                           PullEVSEStatusResponse EVSEStatus2)

            => !(EVSEStatus1 == EVSEStatus2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEStatusResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is PullEVSEStatusResponse pullEVSEStatusResponse &&
                   Equals(pullEVSEStatusResponse);

        #endregion

        #region Equals(PullEVSEStatusResponse)

        /// <summary>
        /// Compares two PullEVSEStatus responses for equality.
        /// </summary>
        /// <param name="PullEVSEStatusResponse">A PullEVSEStatus response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEStatusResponse? PullEVSEStatusResponse)

            => PullEVSEStatusResponse is not null &&

               (!OperatorEVSEStatus.Any() && !PullEVSEStatusResponse.OperatorEVSEStatus.Any() ||
                 OperatorEVSEStatus.Any() &&  PullEVSEStatusResponse.OperatorEVSEStatus.Any() && OperatorEVSEStatus.Count().Equals(PullEVSEStatusResponse.OperatorEVSEStatus.Count())) &&

               ((StatusCode is     null && PullEVSEStatusResponse.StatusCode is     null) ||
                (StatusCode is not null && PullEVSEStatusResponse.StatusCode is not null && StatusCode.Equals(PullEVSEStatusResponse.StatusCode)));

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

                   $"{OperatorEVSEStatus.Count()} operator EVSE status record(s)",

                   StatusCode is not null
                       ? $" -> {StatusCode.Code}"
                       : ""

               );

        #endregion


        #region ToBuilder

        /// <summary>
        /// Return a response builder.
        /// </summary>
        public Builder ToBuilder

            => new (Request,
                    ResponseTimestamp,
                    EventTrackingId,
                    Runtime,
                    OperatorEVSEStatus,
                    StatusCode,
                    ProcessId,
                    HTTPResponse,
                    CustomData);

        #endregion

        #region (class) Builder

        /// <summary>
        /// The PullEVSEStatus response builder.
        /// </summary>
        public new class Builder : AResponse<PullEVSEStatusRequest,
                                             PullEVSEStatusResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// An enumeration of EVSE status records grouped by their operators.
            /// </summary>
            public HashSet<OperatorEVSEStatus>  OperatorEVSEStatus    { get; }

            /// <summary>
            /// The status code for this request.
            /// </summary>
            public StatusCode.Builder           StatusCode            { get; }

            #endregion

            #region Constructor(s)

#pragma warning disable IDE0290 // Use primary constructor

            /// <summary>
            /// Create a new EVSEStatus response builder.
            /// </summary>
            /// <param name="Request">A PullEVSEStatus request.</param>
            /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
            /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
            /// <param name="Runtime">The runtime of the request/response.</param>
            /// <param name="OperatorEVSEStatus">An enumeration of EVSE status records grouped by their operators.</param>
            /// <param name="StatusCode">An optional status code for this request.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(PullEVSEStatusRequest?            Request              = null,
                           DateTime?                         ResponseTimestamp    = null,
                           EventTracking_Id?                 EventTrackingId      = null,
                           TimeSpan?                         Runtime              = null,
                           IEnumerable<OperatorEVSEStatus>?  OperatorEVSEStatus   = null,
                           StatusCode?                       StatusCode           = null,
                           Process_Id?                       ProcessId            = null,
                           HTTPResponse?                     HTTPResponse         = null,
                           JObject?                          CustomData           = null)

                : base(ResponseTimestamp,
                       EventTrackingId,
                       Runtime,
                       Request,
                       HTTPResponse,
                       ProcessId,
                       CustomData)

            {

                this.OperatorEVSEStatus  = OperatorEVSEStatus is not null
                                               ? new HashSet<OperatorEVSEStatus>(OperatorEVSEStatus)
                                               : [];

                this.StatusCode          = StatusCode is not null
                                               ? StatusCode.ToBuilder()
                                               : new StatusCode.Builder();

            }

#pragma warning restore IDE0290 // Use primary constructor

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the PullEVSEStatus response.
            /// </summary>
            /// <param name="Builder">A PullEVSEStatusResponse builder.</param>
            public static implicit operator PullEVSEStatusResponse(Builder Builder)

                => Builder.ToImmutable();


            /// <summary>
            /// Return an immutable version of the PullEVSEStatus response.
            /// </summary>
            public override PullEVSEStatusResponse ToImmutable()

                => new (ResponseTimestamp ?? Timestamp.Now,
                        EventTrackingId   ?? EventTracking_Id.New,
                        ProcessId         ?? Process_Id.NewRandom(),
                        Runtime           ?? (Timestamp.Now - (Request?.Timestamp ?? Timestamp.Now)),
                        OperatorEVSEStatus,

                        Request ?? throw new ArgumentNullException(nameof(Request), "The given request must not be null!"),
                        StatusCode,
                        HTTPResponse,
                        CustomData);

            #endregion

        }

        #endregion

    }

}
