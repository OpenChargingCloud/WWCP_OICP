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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The PullEVSEPricing response.
    /// </summary>
    public class PullEVSEPricingResponse : AResponse<PullEVSEPricingRequest,
                                                     PullEVSEPricingResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of operator EVSE pricings.
        /// </summary>
        [Mandatory]
        public IEnumerable<OperatorEVSEPricing>  OperatorEVSEPricings    { get; }


        public UInt64?                           Number                  { get; }
        public UInt64?                           Size                    { get; }
        public UInt64?                           TotalElements           { get; }
        public Boolean?                          LastPage                { get; }
        public Boolean?                          FirstPage               { get; }
        public UInt64?                           TotalPages              { get; }
        public UInt64?                           NumberOfElements        { get; }


        /// <summary>
        /// The optional status code of this response.
        /// </summary>
        [Optional]
        public StatusCode?                       StatusCode              { get; }

        /// <summary>
        /// Optional warnings.
        /// </summary>
        public IEnumerable<Warning>?             Warnings                { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullEVSEPricing response.
        /// </summary>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="OperatorEVSEPricings">An enumeration of pricing product data.</param>
        /// 
        /// <param name="Request">An optional PullEVSEPricing request.</param>
        /// 
        /// <param name="StatusCode">An optional status code of this response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// <param name="Warnings">Optional warnings.</param>
        public PullEVSEPricingResponse(DateTime                          ResponseTimestamp,
                                       EventTracking_Id                  EventTrackingId,
                                       Process_Id                        ProcessId,
                                       TimeSpan                          Runtime,
                                       IEnumerable<OperatorEVSEPricing>  OperatorEVSEPricings,

                                       PullEVSEPricingRequest?           Request            = null,
                                       UInt64?                           Number             = null,
                                       UInt64?                           Size               = null,
                                       UInt64?                           TotalElements      = null,
                                       Boolean?                          LastPage           = null,
                                       Boolean?                          FirstPage          = null,
                                       UInt64?                           TotalPages         = null,
                                       UInt64?                           NumberOfElements   = null,

                                       StatusCode?                       StatusCode         = null,
                                       HTTPResponse?                     HTTPResponse       = null,
                                       JObject?                          CustomData         = null,
                                       IEnumerable<Warning>?             Warnings           = null)

            : base(ResponseTimestamp,
                   EventTrackingId,
                   ProcessId,
                   Runtime,
                   Request,
                   HTTPResponse,
                   CustomData)

        {

            this.OperatorEVSEPricings  = OperatorEVSEPricings ?? throw new ArgumentNullException(nameof(OperatorEVSEPricings), "The given enumeration of operator EVSE pricings must not be null!");

            this.Number                = Number;
            this.Size                  = Size;
            this.TotalElements         = TotalElements;
            this.LastPage              = LastPage;
            this.FirstPage             = FirstPage;
            this.TotalPages            = TotalPages;
            this.NumberOfElements      = NumberOfElements;

            this.StatusCode            = StatusCode;
            this.Warnings              = Warnings;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#612-eroamingpricingproductdata-message

        // {

        // }

        #endregion

        #region (static) Parse   (JSON, CustomPullEVSEPricingResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullEVSEPricing response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullEVSEPricingResponseParser">A delegate to parse custom PullEVSEPricing JSON objects.</param>
        public static PullEVSEPricingResponse Parse(PullEVSEPricingRequest                                 Request,
                                                    JObject                                                JSON,
                                                    DateTime                                               ResponseTimestamp,
                                                    EventTracking_Id                                       EventTrackingId,
                                                    TimeSpan                                               Runtime,
                                                    Process_Id?                                            ProcessId                             = null,
                                                    HTTPResponse?                                          HTTPResponse                          = null,
                                                    CustomJObjectParserDelegate<PullEVSEPricingResponse>?  CustomPullEVSEPricingResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         out var pullEVSEPricingResponse,
                         out var errorResponse,
                         ProcessId,
                         HTTPResponse,
                         CustomPullEVSEPricingResponseParser))
            {
                return pullEVSEPricingResponse!;
            }

            throw new ArgumentException("The given JSON representation of a PullEVSEPricing response is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out PullEVSEPricingResponse, out ErrorResponse, CustomPullEVSEPricingResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PullEVSEPricing response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="PullEVSEPricingResponse">The parsed PullEVSEPricing response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullEVSEPricingResponseParser">A delegate to parse custom PullEVSEPricing response JSON objects.</param>
        public static Boolean TryParse(PullEVSEPricingRequest                                 Request,
                                       JObject                                                JSON,
                                       DateTime                                               ResponseTimestamp,
                                       EventTracking_Id                                       EventTrackingId,
                                       TimeSpan                                               Runtime,
                                       out PullEVSEPricingResponse?                           PullEVSEPricingResponse,
                                       out String?                                            ErrorResponse,
                                       Process_Id?                                            ProcessId                             = null,
                                       HTTPResponse?                                          HTTPResponse                          = null,
                                       CustomJObjectParserDelegate<PullEVSEPricingResponse>?  CustomPullEVSEPricingResponseParser   = null)
        {

            try
            {

                PullEVSEPricingResponse = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse OperatorEVSEPricing    [mandatory]

                if (!JSON.ParseMandatory("OperatorEVSEPricing",
                                         "operator EVSE pricing",
                                         out JArray OperatorEVSEPricingJSON,
                                         out ErrorResponse))
                {
                    return false;
                }

                var operatorEVSEPricings  = new List<OperatorEVSEPricing>();
                var warnings              = new List<Warning>();

                foreach (var operatorEVSEPricingJToken in OperatorEVSEPricingJSON)
                {

                    try
                    {

                        var ErrorResponse2 = String.Empty;

                        if (operatorEVSEPricingJToken is JObject operatorEVSEPricingJObject &&
                            OperatorEVSEPricing.TryParse(operatorEVSEPricingJObject,
                                                         out OperatorEVSEPricing?  operatorEVSEPricing,
                                                         out                       ErrorResponse2))
                        {
                            operatorEVSEPricings.Add(operatorEVSEPricing!);
                        }

                        else
                        {

                            if (operatorEVSEPricingJToken is JObject operatorEVSEPricingJObject2)
                                ErrorResponse2 = "EVSE " + operatorEVSEPricingJObject2["EvseID"]?.Value<String>() + ": " + ErrorResponse2;

                            if (ErrorResponse2 is not null)
                                warnings.Add(Warning.Create(I18NString.Create(Languages.en, ErrorResponse2)));

                        }

                    }
                    catch (Exception e)
                    {

                        var message = e.Message;

                        if (operatorEVSEPricingJToken is JObject operatorEVSEPricingJObject2)
                            message = "EVSE " + operatorEVSEPricingJObject2["EvseID"]?.Value<String>() + ": " + message;

                        warnings.Add(Warning.Create(I18NString.Create(Languages.en, message)));

                    }

                }

                //if (!JSON.ParseMandatoryJSON("content",
                //                             "EVSE data",
                //                             OperatorEVSEPricings.TryParse,
                //                             out IEnumerable<OperatorEVSEPricings> OperatorEVSEPricings,
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


                PullEVSEPricingResponse = new PullEVSEPricingResponse(ResponseTimestamp,
                                                                      EventTrackingId,
                                                                      ProcessId ?? Process_Id.NewRandom(),
                                                                      Runtime,
                                                                      operatorEVSEPricings,

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
                                                                      warnings);

                if (CustomPullEVSEPricingResponseParser is not null)
                    PullEVSEPricingResponse = CustomPullEVSEPricingResponseParser(JSON,
                                                                                  PullEVSEPricingResponse);

                return true;

            }
            catch (Exception e)
            {
                PullEVSEPricingResponse  = default;
                ErrorResponse            = "The given JSON representation of a PullEVSEPricing response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPullEVSEPricingResponseSerializer = null, CustomOperatorEVSEPricingsSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEPricingResponseSerializer">A delegate to customize the serialization of PullEVSEPricing responses.</param>
        /// <param name="CustomOperatorEVSEPricingsSerializer">A delegate to serialize custom pricing product data JSON objects.</param>
        /// <param name="CustomOperatorEVSEPricingsRecordSerializer">A delegate to serialize custom pricing product data record JSON objects.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullEVSEPricingResponse>?  CustomPullEVSEPricingResponseSerializer   = null,
                              CustomJObjectSerializerDelegate<OperatorEVSEPricing>?      CustomOperatorEVSEPricingSerializer       = null,
                              CustomJObjectSerializerDelegate<EVSEPricing>?              CustomEVSEPricingSerializer               = null,
                              CustomJObjectSerializerDelegate<StatusCode>?               CustomStatusCodeSerializer                = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("OperatorEVSEPricing",  new JArray(OperatorEVSEPricings.Select(operatorEVSEPricing => operatorEVSEPricing.ToJSON(CustomOperatorEVSEPricingSerializer,
                                                                                                                                                          CustomEVSEPricingSerializer)))),

                           StatusCode is not null
                               ? new JProperty("StatusCode",     StatusCode.ToJSON(CustomStatusCodeSerializer))
                               : null,

                           CustomData is not null
                               ? new JProperty("CustomData",     CustomData)
                               : null

                       );

            return CustomPullEVSEPricingResponseSerializer is not null
                       ? CustomPullEVSEPricingResponseSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullEVSEPricingResponse1, PullEVSEPricingResponse2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="PullEVSEPricingResponse1">A PullEVSEPricing response.</param>
        /// <param name="PullEVSEPricingResponse2">Another PullEVSEPricing response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullEVSEPricingResponse PullEVSEPricingResponse1,
                                           PullEVSEPricingResponse PullEVSEPricingResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PullEVSEPricingResponse1, PullEVSEPricingResponse2))
                return true;

            // If one is null, but not both, return false.
            if (PullEVSEPricingResponse1 is null || PullEVSEPricingResponse2 is null)
                return false;

            return PullEVSEPricingResponse1.Equals(PullEVSEPricingResponse2);

        }

        #endregion

        #region Operator != (PullEVSEPricingResponse1, PullEVSEPricingResponse2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="PullEVSEPricingResponse1">A PullEVSEPricing response.</param>
        /// <param name="PullEVSEPricingResponse2">Another PullEVSEPricing response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullEVSEPricingResponse PullEVSEPricingResponse1,
                                           PullEVSEPricingResponse PullEVSEPricingResponse2)

            => !(PullEVSEPricingResponse1 == PullEVSEPricingResponse2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEPricingResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is PullEVSEPricingResponse pullEVSEPricingResponse &&
                   Equals(pullEVSEPricingResponse);

        #endregion

        #region Equals(PullEVSEPricingResponse)

        /// <summary>
        /// Compares two PullEVSEPricing responses for equality.
        /// </summary>
        /// <param name="PullEVSEPricingResponse">A PullEVSEPricing response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEPricingResponse? PullEVSEPricingResponse)

            => PullEVSEPricingResponse is not null &&

               (!OperatorEVSEPricings.Any() && !PullEVSEPricingResponse.OperatorEVSEPricings.Any() ||
                 OperatorEVSEPricings.Any() &&  PullEVSEPricingResponse.OperatorEVSEPricings.Any() && OperatorEVSEPricings.Count().Equals(PullEVSEPricingResponse.OperatorEVSEPricings.Count())) &&

               ((StatusCode is     null && PullEVSEPricingResponse.StatusCode is     null) ||
                (StatusCode is not null && PullEVSEPricingResponse.StatusCode is not null && StatusCode.Equals(PullEVSEPricingResponse.StatusCode)));

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

                return OperatorEVSEPricings.Aggregate(0, (hashCode, operatorEVSEPricing) => hashCode ^ operatorEVSEPricing.GetHashCode()) ^
                       StatusCode?.GetHashCode() ?? 0;

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(OperatorEVSEPricings.Count() + " operator EVSE pricing record(s)",
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
                    OperatorEVSEPricings,
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
        /// A PullEVSEPricing response builder.
        /// </summary>
        public new class Builder : AResponse<PullEVSEPricingRequest,
                                             PullEVSEPricingResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// An enumeration of pricing product data grouped.
            /// </summary>
            public HashSet<OperatorEVSEPricing>  OperatorEVSEPricings    { get; }

            public UInt64?                       Number                  { get; set; }
            public UInt64?                       Size                    { get; set; }
            public UInt64?                       TotalElements           { get; set; }
            public Boolean?                      LastPage                { get; set; }
            public Boolean?                      FirstPage               { get; set; }
            public UInt64?                       TotalPages              { get; set; }
            public UInt64?                       NumberOfElements        { get; set; }

            /// <summary>
            /// The optional status code for this request.
            /// </summary>
            public StatusCode.Builder            StatusCode              { get; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new PullEVSEPricing response builder.
            /// </summary>
            /// <param name="Request">A PullEVSEPricing request.</param>
            /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
            /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
            /// <param name="Runtime">The runtime of the request/response.</param>
            /// <param name="OperatorEVSEPricings">An enumeration of pricing product data grouped by their operators.</param>
            /// <param name="StatusCode">An optional status code for this request.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(PullEVSEPricingRequest?            Request                = null,
                           DateTime?                          ResponseTimestamp      = null,
                           EventTracking_Id?                  EventTrackingId        = null,
                           TimeSpan?                          Runtime                = null,
                           IEnumerable<OperatorEVSEPricing>?  OperatorEVSEPricings   = null,

                           UInt64?                            Number                 = null,
                           UInt64?                            Size                   = null,
                           UInt64?                            TotalElements          = null,
                           Boolean?                           LastPage               = null,
                           Boolean?                           FirstPage              = null,
                           UInt64?                            TotalPages             = null,
                           UInt64?                            NumberOfElements       = null,

                           StatusCode?                        StatusCode             = null,
                           Process_Id?                        ProcessId              = null,
                           HTTPResponse?                      HTTPResponse           = null,
                           JObject?                           CustomData             = null)

                : base(ResponseTimestamp,
                       EventTrackingId,
                       Runtime,
                       Request,
                       HTTPResponse,
                       ProcessId,
                       CustomData)

            {

                this.OperatorEVSEPricings  = OperatorEVSEPricings is not null
                                               ? new HashSet<OperatorEVSEPricing>(OperatorEVSEPricings)
                                               : new HashSet<OperatorEVSEPricing>();

                this.Number                = Number;
                this.Size                  = Size;
                this.TotalElements         = TotalElements;
                this.LastPage              = LastPage;
                this.FirstPage             = FirstPage;
                this.TotalPages            = TotalPages;
                this.NumberOfElements      = NumberOfElements;

                this.StatusCode            = StatusCode is not null
                                                 ? StatusCode.ToBuilder()
                                                 : new StatusCode.Builder();

            }

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the PullEVSEPricing response.
            /// </summary>
            /// <param name="Builder">A PullEVSEPricingResponse builder.</param>
            public static implicit operator PullEVSEPricingResponse(Builder Builder)

                => Builder.ToImmutable();


            /// <summary>
            /// Return an immutable version of the PullEVSEPricing response.
            /// </summary>
            public override PullEVSEPricingResponse ToImmutable()

                => new (ResponseTimestamp ?? Timestamp.Now,
                        EventTrackingId   ?? EventTracking_Id.New,
                        ProcessId         ?? Process_Id.NewRandom(),
                        Runtime           ?? (Timestamp.Now - (Request?.Timestamp ?? Timestamp.Now)),
                        OperatorEVSEPricings,
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
