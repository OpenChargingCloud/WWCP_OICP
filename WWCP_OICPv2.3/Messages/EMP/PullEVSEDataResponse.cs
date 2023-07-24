/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The PullEVSEData response.
    /// </summary>
    public class PullEVSEDataResponse : AResponse<PullEVSEDataRequest,
                                                  PullEVSEDataResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE data records.
        /// </summary>
        [Mandatory]
        public IEnumerable<EVSEDataRecord>  EVSEDataRecords         { get; }


        public UInt64?                      Number                  { get; }
        public UInt64?                      Size                    { get; }
        public UInt64?                      TotalElements           { get; }
        public Boolean?                     LastPage                { get; }
        public Boolean?                     FirstPage               { get; }
        public UInt64?                      TotalPages              { get; }
        public UInt64?                      NumberOfElements        { get; }


        /// <summary>
        /// The optional status code of this response.
        /// </summary>
        [Optional]
        public StatusCode?                  StatusCode              { get; }

        /// <summary>
        /// Optional warnings.
        /// </summary>
        public IEnumerable<Warning>?        Warnings                { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullEVSEData response.
        /// </summary>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="ProcessId">The server side process identification of the request.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        /// 
        /// <param name="Request">An optional PullEVSEData request.</param>
        /// 
        /// <param name="StatusCode">An optional status code of this response.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// <param name="Warnings">Optional warnings.</param>
        public PullEVSEDataResponse(DateTime                     ResponseTimestamp,
                                    EventTracking_Id             EventTrackingId,
                                    Process_Id                   ProcessId,
                                    TimeSpan                     Runtime,
                                    IEnumerable<EVSEDataRecord>  EVSEDataRecords,

                                    PullEVSEDataRequest?         Request            = null,
                                    UInt64?                      Number             = null,
                                    UInt64?                      Size               = null,
                                    UInt64?                      TotalElements      = null,
                                    Boolean?                     LastPage           = null,
                                    Boolean?                     FirstPage          = null,
                                    UInt64?                      TotalPages         = null,
                                    UInt64?                      NumberOfElements   = null,

                                    StatusCode?                  StatusCode         = null,
                                    HTTPResponse?                HTTPResponse       = null,
                                    JObject?                     CustomData         = null,
                                    IEnumerable<Warning>?        Warnings           = null)

            : base(ResponseTimestamp,
                   EventTrackingId,
                   ProcessId,
                   Runtime,
                   Request,
                   HTTPResponse,
                   CustomData)

        {

            this.EVSEDataRecords   = EVSEDataRecords ?? throw new ArgumentNullException(nameof(EVSEDataRecords), "The given enumeration of EVSE data records must not be null!");

            this.Number            = Number;
            this.Size              = Size;
            this.TotalElements     = TotalElements;
            this.LastPage          = LastPage;
            this.FirstPage         = FirstPage;
            this.TotalPages        = TotalPages;
            this.NumberOfElements  = NumberOfElements;

            this.StatusCode        = StatusCode;
            this.Warnings          = Warnings;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#eRoamingEVSEDatamessage

        // {
        // 
        //     "content": [
        //         {
        //             "EvseID":  "string",
        //             [...]
        //         },
        //         [...]
        //     ],
        // 
        //     "number":            0,
        //     "size":              2000,
        //     "totalElements":     144145,
        //     "pageable": {
        //         "sort": {
        //             "sorted":            false,
        //             "unsorted":          true,
        //             "empty":             true
        //         },
        //         "pageSize":          2000,
        //         "pageNumber":        0,
        //         "offset":            0,
        //         "paged":             true,
        //         "unpaged":           false
        //     },
        //     "last":              false,
        //     "totalPages":        73,
        //     "first":             true,
        //     "numberOfElements":  2000,
        // 
        //     "StatusCode": {
        //         "Code":              "000",
        //         "Description":       null,
        //         "AdditionalInfo":    null
        //     },
        // 
        //     "empty": false
        // 
        // }

        #endregion

        #region (static) Parse   (JSON, CustomPullEVSEDataResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullEVSEData response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullEVSEDataResponseParser">A delegate to parse custom PullEVSEData JSON objects.</param>
        public static PullEVSEDataResponse Parse(PullEVSEDataRequest                                 Request,
                                                 JObject                                             JSON,
                                                 DateTime                                            ResponseTimestamp,
                                                 EventTracking_Id                                    EventTrackingId,
                                                 TimeSpan                                            Runtime,
                                                 Process_Id?                                         ProcessId                          = null,
                                                 HTTPResponse?                                       HTTPResponse                       = null,
                                                 CustomJObjectParserDelegate<PullEVSEDataResponse>?  CustomPullEVSEDataResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         out var pullEVSEDataResponse,
                         out var errorResponse,
                         ProcessId,
                         HTTPResponse,
                         CustomPullEVSEDataResponseParser))
            {
                return pullEVSEDataResponse!;
            }

            throw new ArgumentException("The given JSON representation of a PullEVSEData response is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out PullEVSEDataResponse, out ErrorResponse, CustomPullEVSEDataResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PullEVSEData response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="PullEVSEDataResponse">The parsed PullEVSEData response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullEVSEDataResponseParser">A delegate to parse custom PullEVSEData response JSON objects.</param>
        public static Boolean TryParse(PullEVSEDataRequest                                 Request,
                                       JObject                                             JSON,
                                       DateTime                                            ResponseTimestamp,
                                       EventTracking_Id                                    EventTrackingId,
                                       TimeSpan                                            Runtime,
                                       out PullEVSEDataResponse?                           PullEVSEDataResponse,
                                       out String?                                         ErrorResponse,
                                       Process_Id?                                         ProcessId                          = null,
                                       HTTPResponse?                                       HTTPResponse                       = null,
                                       CustomJObjectParserDelegate<PullEVSEDataResponse>?  CustomPullEVSEDataResponseParser   = null)
        {

            try
            {

                PullEVSEDataResponse = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Content               [mandatory]

                if (!JSON.ParseMandatory("content",
                                         "EVSE data content",
                                         out JArray EVSEDataRecordsJSON,
                                         out ErrorResponse))
                {
                    return false;
                }

                var EVSEDataRecords  = new List<EVSEDataRecord>();
                var Warnings         = new List<Warning>();

                foreach (var evseDataRecordJSON in EVSEDataRecordsJSON)
                {

                    try
                    {

                        var ErrorResponse2 = String.Empty;

                        if (evseDataRecordJSON is JObject evseDataRecordJObject &&
                            EVSEDataRecord.TryParse(evseDataRecordJObject,
                                                    out EVSEDataRecord?  evseDataRecord,
                                                    out                  ErrorResponse2))
                        {
                            EVSEDataRecords.Add(evseDataRecord!);
                        }

                        else
                        {

                            if (evseDataRecordJSON is JObject evseDataRecordJObject2)
                                ErrorResponse2 = "EVSE " + evseDataRecordJObject2["EvseID"]?.Value<String>() + ": " + ErrorResponse2;

                            if (ErrorResponse2 is not null)
                                Warnings.Add(Warning.Create(I18NString.Create(Languages.en, ErrorResponse2)));

                        }

                    }
                    catch (Exception e)
                    {

                        var message = e.Message;

                        if (evseDataRecordJSON is JObject evseDataRecordJObject2)
                            message = "EVSE " + evseDataRecordJObject2["EvseID"]?.Value<String>() + ": " + message;

                        Warnings.Add(Warning.Create(I18NString.Create(Languages.en, message)));

                    }

                }

                //if (!JSON.ParseMandatoryJSON("content",
                //                             "EVSE data",
                //                             EVSEDataRecord.TryParse,
                //                             out IEnumerable<EVSEDataRecord> EVSEDataRecords,
                //                             out ErrorResponse))
                //{
                //    return false;
                //}

                #endregion


                #region Parse Number                [optional]

                if (JSON.ParseOptional("number",
                                       "number",
                                       out UInt64? Number,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse Size                  [optional]

                if (JSON.ParseOptional("size",
                                       "size",
                                       out UInt64? Size,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse TotalElements         [optional]

                if (JSON.ParseOptional("totalElements",
                                       "total elements",
                                       out UInt64? TotalElements,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse LastPage              [optional]

                if (JSON.ParseOptional("last",
                                       "last page",
                                       out Boolean? LastPage,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse FirstPage             [optional]

                if (JSON.ParseOptional("first",
                                       "first page",
                                       out Boolean? FirstPage,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse TotalPages            [optional]

                if (JSON.ParseOptional("totalPages",
                                       "total pages",
                                       out UInt64? TotalPages,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse NumberOfElements      [optional]

                if (JSON.ParseOptional("numberOfElements",
                                       "number of elements",
                                       out UInt64? NumberOfElements,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region Parse StatusCode            [optional]

                if (JSON.ParseOptionalJSON("StatusCode",
                                           "StatusCode",
                                           OICPv2_3.StatusCode.TryParse,
                                           out StatusCode StatusCode,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CustomData            [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                PullEVSEDataResponse = new PullEVSEDataResponse(ResponseTimestamp,
                                                                EventTrackingId,
                                                                ProcessId ?? Process_Id.NewRandom(),
                                                                Runtime,
                                                                EVSEDataRecords,

                                                                Request,
                                                                Number,
                                                                Size,
                                                                TotalElements,
                                                                LastPage,
                                                                FirstPage,
                                                                TotalPages,
                                                                NumberOfElements,

                                                                StatusCode,
                                                                HTTPResponse,
                                                                customData,
                                                                Warnings);

                if (CustomPullEVSEDataResponseParser is not null)
                    PullEVSEDataResponse = CustomPullEVSEDataResponseParser(JSON,
                                                                            PullEVSEDataResponse);

                return true;

            }
            catch (Exception e)
            {
                PullEVSEDataResponse  = default;
                ErrorResponse         = "The given JSON representation of a PullEVSEData response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPullEVSEDataResponseSerializer = null, CustomEVSEDataRecordSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEDataResponseSerializer">A delegate to customize the serialization of PullEVSEDataResponse responses.</param>
        /// <param name="CustomEVSEDataRecordSerializer">A delegate to serialize custom EVSE data record JSON objects.</param>
        /// <param name="CustomAddressSerializer">A delegate to serialize custom address JSON objects.</param>
        /// <param name="CustomChargingFacilitySerializer">A delegate to serialize custom charging facility JSON objects.</param>
        /// <param name="CustomGeoCoordinatesSerializer">A delegate to serialize custom geo coordinates JSON objects.</param>
        /// <param name="CustomEnergyMeterSerializer">A delegate to serialize custom energy meter JSON objects.</param>
        /// <param name="CustomTransparencySoftwareStatusSerializer">A delegate to serialize custom transparency software status JSON objects.</param>
        /// <param name="CustomTransparencySoftwareSerializer">A delegate to serialize custom transparency software JSON objects.</param>
        /// <param name="CustomEnergySourceSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomEnvironmentalImpactSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomOpeningTimesSerializer">A delegate to serialize custom opening time JSON objects.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullEVSEDataResponse>?        CustomPullEVSEDataResponseSerializer         = null,
                              CustomJObjectSerializerDelegate<EVSEDataRecord>?              CustomEVSEDataRecordSerializer               = null,
                              CustomJObjectSerializerDelegate<Address>?                     CustomAddressSerializer                      = null,
                              CustomJObjectSerializerDelegate<ChargingFacility>?            CustomChargingFacilitySerializer             = null,
                              CustomJObjectSerializerDelegate<GeoCoordinates>?              CustomGeoCoordinatesSerializer               = null,
                              CustomJObjectSerializerDelegate<EnergyMeter>?                 CustomEnergyMeterSerializer                  = null,
                              CustomJObjectSerializerDelegate<TransparencySoftwareStatus>?  CustomTransparencySoftwareStatusSerializer   = null,
                              CustomJObjectSerializerDelegate<TransparencySoftware>?        CustomTransparencySoftwareSerializer         = null,
                              CustomJObjectSerializerDelegate<EnergySource>?                CustomEnergySourceSerializer                 = null,
                              CustomJObjectSerializerDelegate<EnvironmentalImpact>?         CustomEnvironmentalImpactSerializer          = null,
                              CustomJObjectSerializerDelegate<OpeningTime>?                 CustomOpeningTimesSerializer                 = null,
                              CustomJObjectSerializerDelegate<StatusCode>?                  CustomStatusCodeSerializer                   = null)
        {

            var json = JSONObject.Create(

                           new JProperty("content",           new JArray(EVSEDataRecords.Select(evseDataRecord => evseDataRecord.ToJSON(CustomEVSEDataRecordSerializer,
                                                                                                                                        CustomAddressSerializer,
                                                                                                                                        CustomChargingFacilitySerializer,
                                                                                                                                        CustomGeoCoordinatesSerializer,
                                                                                                                                        CustomEnergyMeterSerializer,
                                                                                                                                        CustomTransparencySoftwareStatusSerializer,
                                                                                                                                        CustomTransparencySoftwareSerializer,
                                                                                                                                        CustomEnergySourceSerializer,
                                                                                                                                        CustomEnvironmentalImpactSerializer,
                                                                                                                                        CustomOpeningTimesSerializer)))),

                           StatusCode is not null
                               ? new JProperty("StatusCode",  StatusCode.ToJSON(CustomStatusCodeSerializer))
                               : null,

                           CustomData is not null
                               ? new JProperty("CustomData",  CustomData)
                               : null

                       );

            return CustomPullEVSEDataResponseSerializer is not null
                       ? CustomPullEVSEDataResponseSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullEVSEDataResponse1, PullEVSEDataResponse2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="PullEVSEDataResponse1">A PullEVSEData response.</param>
        /// <param name="PullEVSEDataResponse2">Another PullEVSEData response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullEVSEDataResponse PullEVSEDataResponse1,
                                           PullEVSEDataResponse PullEVSEDataResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PullEVSEDataResponse1, PullEVSEDataResponse2))
                return true;

            // If one is null, but not both, return false.
            if (PullEVSEDataResponse1 is null || PullEVSEDataResponse2 is null)
                return false;

            return PullEVSEDataResponse1.Equals(PullEVSEDataResponse2);

        }

        #endregion

        #region Operator != (PullEVSEDataResponse1, PullEVSEDataResponse2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="PullEVSEDataResponse1">A PullEVSEData response.</param>
        /// <param name="PullEVSEDataResponse2">Another PullEVSEData response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullEVSEDataResponse PullEVSEDataResponse1,
                                           PullEVSEDataResponse PullEVSEDataResponse2)

            => !(PullEVSEDataResponse1 == PullEVSEDataResponse2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEDataResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is PullEVSEDataResponse pullEVSEDataResponse &&
                   Equals(pullEVSEDataResponse);

        #endregion

        #region Equals(PullEVSEDataResponse)

        /// <summary>
        /// Compares two PullEVSEData responses for equality.
        /// </summary>
        /// <param name="PullEVSEDataResponse">A PullEVSEData response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEDataResponse? PullEVSEDataResponse)

            => PullEVSEDataResponse is not null &&

               (!EVSEDataRecords.Any() && !PullEVSEDataResponse.EVSEDataRecords.Any() ||
                 EVSEDataRecords.Any() &&  PullEVSEDataResponse.EVSEDataRecords.Any() && EVSEDataRecords.Count().Equals(PullEVSEDataResponse.EVSEDataRecords.Count())) &&

               ((StatusCode is     null && PullEVSEDataResponse.StatusCode is     null) ||
                (StatusCode is not null && PullEVSEDataResponse.StatusCode is not null && StatusCode.Equals(PullEVSEDataResponse.StatusCode)));

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return EVSEDataRecords.Aggregate(0, (hashCode, operatorEVSEData) => hashCode ^ operatorEVSEData.GetHashCode()) ^
                      (StatusCode?.GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVSEDataRecords.Count() + " operator EVSE data record(s)",
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
                    Runtime,
                    EVSEDataRecords,
                    Number,
                    Size,
                    TotalElements,
                    LastPage,
                    FirstPage,
                    TotalPages,
                    NumberOfElements,
                    StatusCode,
                    ProcessId,
                    HTTPResponse,
                    CustomData);

        #endregion

        #region (class) Builder

        /// <summary>
        /// The PullEVSEData response builder.
        /// </summary>
        public new class Builder : AResponse<PullEVSEDataRequest,
                                             PullEVSEDataResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// An enumeration of EVSE data records grouped.
            /// </summary>
            public HashSet<EVSEDataRecord>  EVSEDataRecords         { get; }

            public UInt64?                  Number                  { get; set; }
            public UInt64?                  Size                    { get; set; }
            public UInt64?                  TotalElements           { get; set; }
            public Boolean?                 LastPage                { get; set; }
            public Boolean?                 FirstPage               { get; set; }
            public UInt64?                  TotalPages              { get; set; }
            public UInt64?                  NumberOfElements        { get; set; }

            /// <summary>
            /// The optional status code for this request.
            /// </summary>
            public StatusCode.Builder       StatusCode              { get; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new PullEVSEData response builder.
            /// </summary>
            /// <param name="Request">A PullEVSEData request.</param>
            /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
            /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
            /// <param name="Runtime">The runtime of the request/response.</param>
            /// <param name="EVSEDataRecords">An enumeration of EVSE data records grouped by their operators.</param>
            /// <param name="StatusCode">An optional status code for this request.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(PullEVSEDataRequest?          Request             = null,
                           DateTime?                     ResponseTimestamp   = null,
                           EventTracking_Id?             EventTrackingId     = null,
                           TimeSpan?                     Runtime             = null,
                           IEnumerable<EVSEDataRecord>?  EVSEDataRecords     = null,

                           UInt64?                       Number              = null,
                           UInt64?                       Size                = null,
                           UInt64?                       TotalElements       = null,
                           Boolean?                      LastPage            = null,
                           Boolean?                      FirstPage           = null,
                           UInt64?                       TotalPages          = null,
                           UInt64?                       NumberOfElements    = null,

                           StatusCode?                   StatusCode          = null,
                           Process_Id?                   ProcessId           = null,
                           HTTPResponse?                 HTTPResponse        = null,
                           JObject?                      CustomData          = null)

                : base(ResponseTimestamp,
                       EventTrackingId,
                       Runtime,
                       Request,
                       HTTPResponse,
                       ProcessId,
                       CustomData)

            {

                this.EVSEDataRecords   = EVSEDataRecords is not null
                                             ? new HashSet<EVSEDataRecord>(EVSEDataRecords)
                                             : new HashSet<EVSEDataRecord>();

                this.Number            = Number;
                this.Size              = Size;
                this.TotalElements     = TotalElements;
                this.LastPage          = LastPage;
                this.FirstPage         = FirstPage;
                this.TotalPages        = TotalPages;
                this.NumberOfElements  = NumberOfElements;

                this.StatusCode        = StatusCode is not null
                                             ? StatusCode.ToBuilder()
                                             : new StatusCode.Builder();

            }

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the PullEVSEData response.
            /// </summary>
            /// <param name="Builder">A PullEVSEDataResponse builder.</param>
            public static implicit operator PullEVSEDataResponse(Builder Builder)

                => Builder.ToImmutable();


            /// <summary>
            /// Return an immutable version of the PullEVSEData response.
            /// </summary>
            public override PullEVSEDataResponse ToImmutable()

                => new (ResponseTimestamp ?? Timestamp.Now,
                        EventTrackingId   ?? EventTracking_Id.New,
                        ProcessId         ?? Process_Id.NewRandom(),
                        Runtime           ?? (Timestamp.Now - (Request?.Timestamp ?? Timestamp.Now)),
                        EVSEDataRecords,

                        Request ?? throw new ArgumentNullException(nameof(Request), "The given request must not be null!"),
                        Number,
                        Size,
                        TotalElements,
                        LastPage,
                        FirstPage,
                        TotalPages,
                        NumberOfElements,
                        StatusCode,
                        HTTPResponse,
                        CustomData);

            #endregion

        }

        #endregion

    }

}
