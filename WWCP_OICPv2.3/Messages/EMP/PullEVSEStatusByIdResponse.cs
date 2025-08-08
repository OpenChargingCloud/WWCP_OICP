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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The PullEVSEStatusById response.
    /// </summary>
    public class PullEVSEStatusByIdResponse : AResponse<PullEVSEStatusByIdRequest,
                                              PullEVSEStatusByIdResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE status records.
        /// </summary>
        [Mandatory]
        public IEnumerable<EVSEStatusRecord>  EVSEStatusRecords    { get; }

        /// <summary>
        /// The optional status code of this response.
        /// </summary>
        [Optional]
        public StatusCode?                    StatusCode           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullEVSEStatusById response.
        /// </summary>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="ProcessId">The server side process identification of the request.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
        /// 
        /// <param name="Request">An optional PullEVSEStatusById request.</param>
        /// <param name="StatusCode">An optional status code of this response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public PullEVSEStatusByIdResponse(DateTimeOffset                 ResponseTimestamp,
                                          EventTracking_Id               EventTrackingId,
                                          Process_Id                     ProcessId,
                                          TimeSpan                       Runtime,
                                          IEnumerable<EVSEStatusRecord>  EVSEStatusRecords,

                                          PullEVSEStatusByIdRequest?     Request        = null,
                                          StatusCode?                    StatusCode     = null,
                                          HTTPResponse?                  HTTPResponse   = null,
                                          JObject?                       CustomData     = null)

            : base(ResponseTimestamp,
                   EventTrackingId,
                   ProcessId,
                   Runtime,
                   Request,
                   HTTPResponse,
                   CustomData)

        {

            this.EVSEStatusRecords  = EVSEStatusRecords ?? throw new ArgumentNullException(nameof(EVSEStatusRecords), "The given enumeration of EVSE status records must not be null!");
            this.StatusCode         = StatusCode;

            unchecked
            {

                hashCode = this.EVSEStatusRecords.CalcHashCode() * 3 ^
                           this.StatusCode?.      GetHashCode() ?? 0;

            }

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#eRoamingPullEVSEStatusByIDmessage

        // {
        //   "EVSEStatusRecords": {
        //     "EvseStatusRecord": [
        //       {
        //         "EvseID":     "string",
        //         "EvseStatus": "Available"
        //       }
        //     ]
        //   },
        //   "StatusCode": {
        //     "AdditionalInfo": "string",
        //     "Code":           "000",
        //     "Description":    "string"
        //   }
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomPullEVSEStatusByIdResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullEVSEStatusById response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullEVSEStatusByIdResponseParser">A delegate to parse custom PullEVSEStatusById JSON objects.</param>
        public static PullEVSEStatusByIdResponse Parse(PullEVSEStatusByIdRequest                                 Request,
                                                       JObject                                                   JSON,
                                                       DateTime                                                  ResponseTimestamp,
                                                       EventTracking_Id                                          EventTrackingId,
                                                       TimeSpan                                                  Runtime,
                                                       Process_Id?                                               ProcessId                                = null,
                                                       HTTPResponse?                                             HTTPResponse                             = null,
                                                       CustomJObjectParserDelegate<PullEVSEStatusByIdResponse>?  CustomPullEVSEStatusByIdResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         out var pullEVSEStatusByIdResponse,
                         out var errorResponse,
                         ProcessId,
                         HTTPResponse,
                         CustomPullEVSEStatusByIdResponseParser))
            {
                return pullEVSEStatusByIdResponse;
            }

            throw new ArgumentException("The given JSON representation of a PullEVSEStatusById response is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, ..., out PullEVSEStatusByIdResponse, out ErrorResponse, ..., CustomPullEVSEStatusByIdResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PullEVSEStatusById response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="PullEVSEStatusByIdResponse">The parsed PullEVSEStatusById response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullEVSEStatusByIdResponseParser">A delegate to parse custom PullEVSEStatusById response JSON objects.</param>
        public static Boolean TryParse(PullEVSEStatusByIdRequest                                 Request,
                                       JObject                                                   JSON,
                                       DateTimeOffset                                            ResponseTimestamp,
                                       EventTracking_Id                                          EventTrackingId,
                                       TimeSpan                                                  Runtime,
                                       [NotNullWhen(true)]  out PullEVSEStatusByIdResponse?      PullEVSEStatusByIdResponse,
                                       [NotNullWhen(false)] out String?                          ErrorResponse,
                                       Process_Id?                                               ProcessId                                = null,
                                       HTTPResponse?                                             HTTPResponse                             = null,
                                       CustomJObjectParserDelegate<PullEVSEStatusByIdResponse>?  CustomPullEVSEStatusByIdResponseParser   = null)
        {

            try
            {

                PullEVSEStatusByIdResponse = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse EVSEStatusRecords     [mandatory]

                if (!JSON.ParseMandatory("EVSEStatusRecords",
                                         "EVSEStatusRecords",
                                         out JObject evseStatusRecords,
                                         out         ErrorResponse))
                {
                    return false;
                }

                if (!evseStatusRecords.ParseMandatoryJSON("EvseStatusRecord",
                                                          "EVSE status record",
                                                          EVSEStatusRecord.TryParse,
                                                          out IEnumerable<EVSEStatusRecord> EVSEStatusRecords,
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


                PullEVSEStatusByIdResponse = new PullEVSEStatusByIdResponse(

                                                 ResponseTimestamp,
                                                 EventTrackingId,
                                                 ProcessId ?? Process_Id.NewRandom(),
                                                 Runtime,
                                                 EVSEStatusRecords,

                                                 Request,
                                                 StatusCode,
                                                 HTTPResponse,
                                                 customData

                                             );

                if (CustomPullEVSEStatusByIdResponseParser is not null)
                    PullEVSEStatusByIdResponse = CustomPullEVSEStatusByIdResponseParser(JSON,
                                                                                        PullEVSEStatusByIdResponse);

                return true;

            }
            catch (Exception e)
            {
                PullEVSEStatusByIdResponse  = default;
                ErrorResponse               = "The given JSON representation of a PullEVSEStatusById response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPullEVSEStatusByIdResponseSerializer = null, CustomEVSEStatusRecordSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEStatusByIdResponseSerializer">A delegate to customize the serialization of PullEVSEStatusByIdResponse responses.</param>
        /// <param name="CustomEVSEStatusRecordSerializer">A delegate to serialize custom EVSE status record JSON objects.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullEVSEStatusByIdResponse>?  CustomPullEVSEStatusByIdResponseSerializer   = null,
                              CustomJObjectSerializerDelegate<EVSEStatusRecord>?            CustomEVSEStatusRecordSerializer             = null,
                              CustomJObjectSerializerDelegate<StatusCode>?                  CustomStatusCodeSerializer                   = null)
        {

            var json = JSONObject.Create(

                           new JProperty("EVSEStatusRecords", new JObject(
                               new JProperty("EvseStatusRecord",  new JArray(EVSEStatusRecords.Select(evseStatusRecord => evseStatusRecord.ToJSON(CustomEVSEStatusRecordSerializer))))
                           )),

                           StatusCode is not null
                               ? new JProperty("StatusCode",  StatusCode.ToJSON(CustomStatusCodeSerializer))
                               : null

                       );

            return CustomPullEVSEStatusByIdResponseSerializer is not null
                       ? CustomPullEVSEStatusByIdResponseSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullEVSEStatusByIdResponse1, PullEVSEStatusByIdResponse2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PullEVSEStatusByIdResponse1">An EVSE status by id request.</param>
        /// <param name="PullEVSEStatusByIdResponse2">Another EVSE status by id request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullEVSEStatusByIdResponse PullEVSEStatusByIdResponse1,
                                           PullEVSEStatusByIdResponse PullEVSEStatusByIdResponse2)
        {

            if (ReferenceEquals(PullEVSEStatusByIdResponse1, PullEVSEStatusByIdResponse2))
                return true;

            if (PullEVSEStatusByIdResponse1 is null || PullEVSEStatusByIdResponse2 is null)
                return false;

            return PullEVSEStatusByIdResponse1.Equals(PullEVSEStatusByIdResponse2);

        }

        #endregion

        #region Operator != (PullEVSEStatusByIdResponse1, PullEVSEStatusByIdResponse2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PullEVSEStatusByIdResponse1">An EVSE status by id request.</param>
        /// <param name="PullEVSEStatusByIdResponse2">Another EVSE status by id request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullEVSEStatusByIdResponse PullEVSEStatusByIdResponse1,
                                           PullEVSEStatusByIdResponse PullEVSEStatusByIdResponse2)

            => !(PullEVSEStatusByIdResponse1 == PullEVSEStatusByIdResponse2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEStatusByIdResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is PullEVSEStatusByIdResponse pullEVSEStatusByIdResponse &&
                   Equals(pullEVSEStatusByIdResponse);

        #endregion

        #region Equals(PullEVSEStatusByIdResponse)

        /// <summary>
        /// Compares two PullEVSEStatusById responses for equality.
        /// </summary>
        /// <param name="PullEVSEStatusByIdResponse">A PullEVSEStatusById response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEStatusByIdResponse? PullEVSEStatusByIdResponse)

            => PullEVSEStatusByIdResponse is not null &&

               (!EVSEStatusRecords.Any() && !PullEVSEStatusByIdResponse.EVSEStatusRecords.Any() ||
                 EVSEStatusRecords.Any() &&  PullEVSEStatusByIdResponse.EVSEStatusRecords.Any() && EVSEStatusRecords.Count().Equals(PullEVSEStatusByIdResponse.EVSEStatusRecords.Count())) &&

               ((StatusCode is     null && PullEVSEStatusByIdResponse.StatusCode is     null) ||
                (StatusCode is not null && PullEVSEStatusByIdResponse.StatusCode is not null && StatusCode.Equals(PullEVSEStatusByIdResponse.StatusCode)));

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

                   $"{EVSEStatusRecords.Count()} EVSE status record(s)",

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
                    EVSEStatusRecords,
                    StatusCode,
                    ProcessId,
                    HTTPResponse,
                    CustomData);

        #endregion

        #region (class) Builder

        /// <summary>
        /// An EVSEStatusById response builder.
        /// </summary>
        public new class Builder : AResponse<PullEVSEStatusByIdRequest,
                                             PullEVSEStatusByIdResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// An enumeration of EVSE status records.
            /// </summary>
            public HashSet<EVSEStatusRecord>  EVSEStatusRecords    { get; }

            /// <summary>
            /// The status code for this request.
            /// </summary>
            public StatusCode.Builder         StatusCode           { get; }

            #endregion

            #region Constructor(s)

#pragma warning disable IDE0290 // Use primary constructor

            /// <summary>
            /// Create a new PullEVSEStatusById response builder.
            /// </summary>
            /// <param name="Request">A PullEVSEStatusById request.</param>
            /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
            /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
            /// <param name="Runtime">The runtime of the request/response.</param>
            /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
            /// <param name="StatusCode">An optional status code of this response.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(PullEVSEStatusByIdRequest?      Request             = null,
                           DateTimeOffset?                 ResponseTimestamp   = null,
                           EventTracking_Id?               EventTrackingId     = null,
                           TimeSpan?                       Runtime             = null,
                           IEnumerable<EVSEStatusRecord>?  EVSEStatusRecords   = null,
                           StatusCode?                     StatusCode          = null,
                           Process_Id?                     ProcessId           = null,
                           HTTPResponse?                   HTTPResponse        = null,
                           JObject?                        CustomData          = null)

                : base(ResponseTimestamp,
                       EventTrackingId,
                       Runtime,
                       Request,
                       HTTPResponse,
                       ProcessId,
                       CustomData)

            {

                this.EVSEStatusRecords  = EVSEStatusRecords is not null
                                              ? new HashSet<EVSEStatusRecord>(EVSEStatusRecords)
                                              : [];

                this.StatusCode         = StatusCode is not null
                                              ? StatusCode.ToBuilder()
                                              : new StatusCode.Builder();

            }

#pragma warning restore IDE0290 // Use primary constructor

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the PullEVSEStatusById response.
            /// </summary>
            /// <param name="Builder">A PullEVSEStatusByIdResponse builder.</param>
            public static implicit operator PullEVSEStatusByIdResponse(Builder Builder)

                => Builder.ToImmutable();


            /// <summary>
            /// Return an immutable version of the PullEVSEStatusById response.
            /// </summary>
            public override PullEVSEStatusByIdResponse ToImmutable()

                => new (ResponseTimestamp ?? Timestamp.Now,
                        EventTrackingId   ?? EventTracking_Id.New,
                        ProcessId         ?? Process_Id.NewRandom(),
                        Runtime           ?? (Timestamp.Now - (Request?.Timestamp ?? Timestamp.Now)),
                        EVSEStatusRecords,
                        Request ?? throw new ArgumentNullException(nameof(Request), "The given request must not be null!"),
                        StatusCode,
                        HTTPResponse,
                        CustomData);

            #endregion

        }

        #endregion

    }

}
