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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The GetChargeDetailRecords response.
    /// </summary>
    public class GetChargeDetailRecordsResponse : APagedResponse<GetChargeDetailRecordsRequest,
                                                                 GetChargeDetailRecordsResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of charge detail records.
        /// </summary>
        [Mandatory]
        public IEnumerable<ChargeDetailRecord>  ChargeDetailRecords    { get; }

        #endregion

        #region Constructor(s)

#pragma warning disable IDE0290 // Use primary constructor

        /// <summary>
        /// Create a new GetChargeDetailRecords response.
        /// </summary>
        /// <param name="Request">A GetChargeDetailRecords request.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="ProcessId">The server side process identification of the request.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// 
        /// <param name="Request">The request leading to this result. Might be null, when the request e.g. was not parsable!</param>
        /// 
        /// <param name="StatusCode">An optional status code of this response.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public GetChargeDetailRecordsResponse(DateTime                         ResponseTimestamp,
                                              EventTracking_Id                 EventTrackingId,
                                              Process_Id                       ProcessId,
                                              TimeSpan                         Runtime,
                                              IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,

                                              GetChargeDetailRecordsRequest?   Request            = null,
                                              Boolean?                         FirstPage          = null,
                                              Boolean?                         LastPage           = null,
                                              UInt64?                          Number             = null,
                                              UInt64?                          NumberOfElements   = null,
                                              UInt64?                          Size               = null,
                                              UInt64?                          TotalElements      = null,
                                              UInt64?                          TotalPages         = null,

                                              StatusCode?                      StatusCode         = null,
                                              HTTPResponse?                    HTTPResponse       = null,
                                              JObject?                         CustomData         = null)

            : base(ResponseTimestamp,
                   EventTrackingId,
                   ProcessId,
                   Runtime,

                   Request,
                   FirstPage,
                   LastPage,
                   Number,
                   NumberOfElements,
                   Size,
                   TotalElements,
                   TotalPages,
                   StatusCode,

                   HTTPResponse,
                   CustomData)

        {

            this.ChargeDetailRecords = ChargeDetailRecords ?? throw new ArgumentNullException(nameof(ChargeDetailRecords), "The given enumeration of charge detail records must not be null!");

        }

#pragma warning restore IDE0290 // Use primary constructor

        #endregion


        #region Documentation

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

        #region (static) Parse   (JSON, CustomGetChargeDetailRecordsResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a GetChargeDetailRecords response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomGetChargeDetailRecordsResponseParser">A delegate to parse custom GetChargeDetailRecords JSON objects.</param>
        public static GetChargeDetailRecordsResponse Parse(GetChargeDetailRecordsRequest                                 Request,
                                                           JObject                                                       JSON,
                                                           DateTime                                                      ResponseTimestamp,
                                                           EventTracking_Id                                              EventTrackingId,
                                                           TimeSpan                                                      Runtime,
                                                           Process_Id?                                                   ProcessId                                    = null,
                                                           HTTPResponse?                                                 HTTPResponse                                 = null,
                                                           CustomJObjectParserDelegate<GetChargeDetailRecordsResponse>?  CustomGetChargeDetailRecordsResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         out var getChargeDetailRecordsResponse,
                         out var errorResponse,
                         HTTPResponse,
                         ProcessId,
                         CustomGetChargeDetailRecordsResponseParser))
            {
                return getChargeDetailRecordsResponse;
            }

            throw new ArgumentException("The given JSON representation of a GetChargeDetailRecords response is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out GetChargeDetailRecordsResponse, out ErrorResponse, CustomGetChargeDetailRecordsResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a GetChargeDetailRecords response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="GetChargeDetailRecordsResponse">The parsed GetChargeDetailRecords response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomGetChargeDetailRecordsResponseParser">A delegate to parse custom GetChargeDetailRecords response JSON objects.</param>
        public static Boolean TryParse(GetChargeDetailRecordsRequest                                 Request,
                                       JObject                                                       JSON,
                                       DateTime                                                      ResponseTimestamp,
                                       EventTracking_Id                                              EventTrackingId,
                                       TimeSpan                                                      Runtime,
                                       [NotNullWhen(true)]  out GetChargeDetailRecordsResponse?      GetChargeDetailRecordsResponse,
                                       [NotNullWhen(false)] out String?                              ErrorResponse,
                                       HTTPResponse?                                                 HTTPResponse                                 = null,
                                       Process_Id?                                                   ProcessId                                    = null,
                                       CustomJObjectParserDelegate<GetChargeDetailRecordsResponse>?  CustomGetChargeDetailRecordsResponseParser   = null)
        {

            try
            {

                GetChargeDetailRecordsResponse = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse ChargeDetailRecords    [mandatory]

                if (JSON.ParseOptionalJSON("content",
                                           "charge detail records",
                                           ChargeDetailRecord.TryParse,
                                           out IEnumerable<ChargeDetailRecord> ChargeDetailRecords,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse StatusCode             [optional]

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

                #region Parse First                  [optional]

                if (JSON.ParseOptional("first",
                                       "first result",
                                       out Boolean? First,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse Last                   [optional]

                if (JSON.ParseOptional("last",
                                       "last result",
                                       out Boolean? Last,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse Number                 [optional]

                if (JSON.ParseOptional("number",
                                       "number",
                                       out UInt32? Number,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse NumberOfElements       [optional]

                if (JSON.ParseOptional("numberOfElements",
                                       "number",
                                       out UInt32? NumberOfElements,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse Size                   [optional]

                if (JSON.ParseOptional("size",
                                       "size",
                                       out UInt32? Size,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse TotalElements          [optional]

                if (JSON.ParseOptional("totalElements",
                                       "total elements",
                                       out UInt32? TotalElements,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse TotalPages             [optional]

                if (JSON.ParseOptional("totalPages",
                                       "total pages",
                                       out UInt32? TotalPages,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CustomData             [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                GetChargeDetailRecordsResponse = new GetChargeDetailRecordsResponse(
                                                     ResponseTimestamp,
                                                     EventTrackingId,
                                                     ProcessId           ?? Process_Id.NewRandom(),
                                                     Runtime,
                                                     ChargeDetailRecords ?? [],

                                                     Request,
                                                     First,
                                                     Last,
                                                     Number,
                                                     NumberOfElements,
                                                     Size,
                                                     TotalElements,
                                                     TotalPages,
                                                     StatusCode,

                                                     HTTPResponse,
                                                     customData
                                                 );

                if (CustomGetChargeDetailRecordsResponseParser is not null)
                    GetChargeDetailRecordsResponse = CustomGetChargeDetailRecordsResponseParser(JSON,
                                                                                                GetChargeDetailRecordsResponse);

                return true;

            }
            catch (Exception e)
            {
                GetChargeDetailRecordsResponse  = default;
                ErrorResponse                   = "The given JSON representation of a GetChargeDetailRecords response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomGetChargeDetailRecordsResponseSerializer = null, CustomOperatorEVSEStatusSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomGetChargeDetailRecordsResponseSerializer">A delegate to customize the serialization of GetChargeDetailRecordsResponse responses.</param>
        /// <param name="CustomIPagedResponseSerializer">A delegate to customize the serialization of paged responses.</param>
        /// <param name="CustomChargeDetailRecordSerializer">A delegate to serialize custom ChargeDetailRecord JSON elements.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON elements.</param>
        /// <param name="CustomSignedMeteringValueSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomCalibrationLawVerificationSerializer">A delegate to serialize custom calibration law verification JSON objects.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<GetChargeDetailRecordsResponse>?  CustomGetChargeDetailRecordsResponseSerializer   = null,
                              CustomJObjectSerializerDelegate<IPagedResponse>?                  CustomIPagedResponseSerializer                   = null,
                              CustomJObjectSerializerDelegate<ChargeDetailRecord>?              CustomChargeDetailRecordSerializer               = null,
                              CustomJObjectSerializerDelegate<Identification>?                  CustomIdentificationSerializer                   = null,
                              CustomJObjectSerializerDelegate<SignedMeteringValue>?             CustomSignedMeteringValueSerializer              = null,
                              CustomJObjectSerializerDelegate<CalibrationLawVerification>?      CustomCalibrationLawVerificationSerializer       = null,
                              CustomJObjectSerializerDelegate<StatusCode>?                      CustomStatusCodeSerializer                       = null)
        {

            var json = ToJSON(CustomIPagedResponseSerializer,
                              CustomStatusCodeSerializer);

            if (ChargeDetailRecords.SafeAny())
                json.Add(new JProperty("content",
                                       new JArray(ChargeDetailRecords.Select(chargeDetailRecord => chargeDetailRecord.ToJSON(CustomChargeDetailRecordSerializer,
                                                                                                                             CustomIdentificationSerializer,
                                                                                                                             CustomSignedMeteringValueSerializer,
                                                                                                                             CustomCalibrationLawVerificationSerializer)))));

            return CustomGetChargeDetailRecordsResponseSerializer is not null
                       ? CustomGetChargeDetailRecordsResponseSerializer(this, json)
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
        public static Boolean operator == (GetChargeDetailRecordsResponse EVSEStatus1,
                                           GetChargeDetailRecordsResponse EVSEStatus2)
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
        public static Boolean operator != (GetChargeDetailRecordsResponse EVSEStatus1,
                                           GetChargeDetailRecordsResponse EVSEStatus2)

            => !(EVSEStatus1 == EVSEStatus2);

        #endregion

        #endregion

        #region IEquatable<GetChargeDetailRecordsResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is GetChargeDetailRecordsResponse getChargeDetailRecordsResponse &&
                   Equals(getChargeDetailRecordsResponse);

        #endregion

        #region Equals(GetChargeDetailRecordsResponse)

        /// <summary>
        /// Compares two GetChargeDetailRecords responses for equality.
        /// </summary>
        /// <param name="GetChargeDetailRecordsResponse">A GetChargeDetailRecords response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetChargeDetailRecordsResponse? GetChargeDetailRecordsResponse)

            => GetChargeDetailRecordsResponse is not null &&

               (!ChargeDetailRecords.Any() && !GetChargeDetailRecordsResponse.ChargeDetailRecords.Any() ||
                 ChargeDetailRecords.Any() &&  GetChargeDetailRecordsResponse.ChargeDetailRecords.Any() && ChargeDetailRecords.Count().Equals(GetChargeDetailRecordsResponse.ChargeDetailRecords.Count())) &&

               ((StatusCode is     null && GetChargeDetailRecordsResponse.StatusCode is     null) ||
                (StatusCode is not null && GetChargeDetailRecordsResponse.StatusCode is not null && StatusCode.Equals(GetChargeDetailRecordsResponse.StatusCode)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return base.GetHashCode() ^
                       ChargeDetailRecords.Aggregate(0, (hashCode, operatorEVSEStatus) => hashCode ^ operatorEVSEStatus.GetHashCode());

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(ChargeDetailRecords.Count() + " charge detail record(s)",
                             StatusCode is not null
                                 ? " -> " + StatusCode.Code
                                 : "");

        #endregion


        #region ToBuilder

        /// <summary>
        /// Return a response builder.
        /// </summary>
        public Builder ToBuilder

            => new (Request,
                    ResponseTimestamp,
                    EventTrackingId,
                    ProcessId,
                    Runtime,
                    ChargeDetailRecords,
                    FirstPage,
                    LastPage,
                    Number,
                    NumberOfElements,
                    Size,
                    TotalElements,
                    TotalPages,
                    StatusCode,
                    HTTPResponse,
                    CustomData);

        #endregion

        #region (class) Builder

        /// <summary>
        /// The GetChargeDetailRecords response builder.
        /// </summary>
        public new class Builder : APagedResponse<GetChargeDetailRecordsRequest,
                                                  GetChargeDetailRecordsResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// An enumeration of EVSE status records grouped by their operators.
            /// </summary>
            public HashSet<ChargeDetailRecord>  ChargeDetailRecords    { get; }

            #endregion

            #region Constructor(s)

#pragma warning disable IDE0290 // Use primary constructor

            /// <summary>
            /// Create a new GetChargeDetailRecords response builder.
            /// </summary>
            /// <param name="Request">A GetChargeDetailRecords request.</param>
            /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
            /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
            /// <param name="Runtime">The runtime of the request/response.</param>
            /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
            /// <param name="StatusCode">An optional status code for this request.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(GetChargeDetailRecordsRequest?    Request               = null,
                           DateTime?                         ResponseTimestamp     = null,
                           EventTracking_Id?                 EventTrackingId       = null,
                           Process_Id?                       ProcessId             = null,
                           TimeSpan?                         Runtime               = null,

                           IEnumerable<ChargeDetailRecord>?  ChargeDetailRecords   = null,
                           Boolean?                          FirstPage             = null,
                           Boolean?                          LastPage              = null,
                           UInt64?                           Number                = null,
                           UInt64?                           NumberOfElements      = null,
                           UInt64?                           Size                  = null,
                           UInt64?                           TotalElements         = null,
                           UInt64?                           TotalPages            = null,
                           StatusCode?                       StatusCode            = null,

                           HTTPResponse?                     HTTPResponse          = null,
                           JObject?                          CustomData            = null)

                : base(Request,
                       ResponseTimestamp,
                       EventTrackingId,
                       Runtime,

                       HTTPResponse,
                       ProcessId,
                       StatusCode,
                       FirstPage,
                       LastPage,
                       Number,
                       NumberOfElements,
                       Size,
                       TotalElements,
                       TotalPages,

                       CustomData)

            {

                this.ChargeDetailRecords = ChargeDetailRecords is not null
                                               ? new HashSet<ChargeDetailRecord>(ChargeDetailRecords)
                                               : [];

            }

#pragma warning restore IDE0290 // Use primary constructor

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the GetChargeDetailRecords response.
            /// </summary>
            /// <param name="Builder">A GetChargeDetailRecordsResponse builder.</param>
            public static implicit operator GetChargeDetailRecordsResponse(Builder Builder)

                => Builder.ToImmutable();


            /// <summary>
            /// Return an immutable version of the GetChargeDetailRecords response.
            /// </summary>
            public override GetChargeDetailRecordsResponse ToImmutable()

                => new (ResponseTimestamp ?? Timestamp.Now,
                        EventTrackingId   ?? EventTracking_Id.New,
                        ProcessId         ?? Process_Id.NewRandom(),
                        Runtime           ?? (Timestamp.Now - (Request?.Timestamp ?? Timestamp.Now)),
                        ChargeDetailRecords,

                        Request ?? throw new ArgumentNullException(nameof(Request), "The given request must not be null!"),
                        FirstPage,
                        LastPage,
                        Number,
                        NumberOfElements,
                        Size,
                        TotalElements,
                        TotalPages,
                        StatusCode,

                        HTTPResponse,
                        CustomData);

            #endregion

        }

        #endregion

    }

}
