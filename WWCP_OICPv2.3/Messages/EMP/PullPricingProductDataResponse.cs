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
    /// The PullPricingProductData response.
    /// </summary>
    public class PullPricingProductDataResponse : AResponse<PullPricingProductDataRequest,
                                                  PullPricingProductDataResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of pricing product data.
        /// </summary>
        [Mandatory]
        public IEnumerable<PricingProductData>  PricingProductData         { get; }


        public UInt64?                          Number                  { get; }
        public UInt64?                          Size                    { get; }
        public UInt64?                          TotalElements           { get; }
        public Boolean?                         LastPage                { get; }
        public Boolean?                         FirstPage               { get; }
        public UInt64?                          TotalPages              { get; }
        public UInt64?                          NumberOfElements        { get; }


        /// <summary>
        /// The optional status code of this response.
        /// </summary>
        [Optional]
        public StatusCode?                      StatusCode              { get; }

        /// <summary>
        /// Optional warnings.
        /// </summary>
        public IEnumerable<Warning>?            Warnings                { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullPricingProductData response.
        /// </summary>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="PricingProductData">An enumeration of pricing product data.</param>
        /// 
        /// <param name="Request">An optional PullPricingProductData request.</param>
        /// 
        /// <param name="StatusCode">An optional status code of this response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// <param name="Warnings">Optional warnings.</param>
        public PullPricingProductDataResponse(DateTime                         ResponseTimestamp,
                                              EventTracking_Id                 EventTrackingId,
                                              Process_Id                       ProcessId,
                                              TimeSpan                         Runtime,
                                              IEnumerable<PricingProductData>  PricingProductData,

                                              PullPricingProductDataRequest?   Request            = null,
                                              UInt64?                          Number             = null,
                                              UInt64?                          Size               = null,
                                              UInt64?                          TotalElements      = null,
                                              Boolean?                         LastPage           = null,
                                              Boolean?                         FirstPage          = null,
                                              UInt64?                          TotalPages         = null,
                                              UInt64?                          NumberOfElements   = null,

                                              StatusCode?                      StatusCode         = null,
                                              HTTPResponse?                    HTTPResponse       = null,
                                              JObject?                         CustomData         = null,
                                              IEnumerable<Warning>?            Warnings           = null)

            : base(ResponseTimestamp,
                   EventTrackingId,
                   ProcessId,
                   Runtime,
                   Request,
                   HTTPResponse,
                   CustomData)

        {

            this.PricingProductData  = PricingProductData ?? throw new ArgumentNullException(nameof(PricingProductData), "The given enumeration of pricing product data must not be null!");

            this.Number              = Number;
            this.Size                = Size;
            this.TotalElements       = TotalElements;
            this.LastPage            = LastPage;
            this.FirstPage           = FirstPage;
            this.TotalPages          = TotalPages;
            this.NumberOfElements    = NumberOfElements;

            this.StatusCode          = StatusCode;
            this.Warnings            = Warnings;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#612-eroamingpricingproductdata-message

        // {

        // }

        #endregion

        #region (static) Parse   (JSON, CustomPullPricingProductDataResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullPricingProductData response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullPricingProductDataResponseParser">A delegate to parse custom PullPricingProductData JSON objects.</param>
        public static PullPricingProductDataResponse Parse(PullPricingProductDataRequest                                 Request,
                                                           JObject                                                       JSON,
                                                           DateTime                                                      ResponseTimestamp,
                                                           EventTracking_Id                                              EventTrackingId,
                                                           TimeSpan                                                      Runtime,
                                                           Process_Id?                                                   ProcessId                                    = null,
                                                           HTTPResponse?                                                 HTTPResponse                                 = null,
                                                           CustomJObjectParserDelegate<PullPricingProductDataResponse>?  CustomPullPricingProductDataResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         out var pullPricingProductDataResponse,
                         out var errorResponse,
                         ProcessId,
                         HTTPResponse,
                         CustomPullPricingProductDataResponseParser))
            {
                return pullPricingProductDataResponse;
            }

            throw new ArgumentException("The given JSON representation of a PullPricingProductData response is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out PullPricingProductDataResponse, out ErrorResponse, CustomPullPricingProductDataResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PullPricingProductData response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="PullPricingProductDataResponse">The parsed PullPricingProductData response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullPricingProductDataResponseParser">A delegate to parse custom PullPricingProductData response JSON objects.</param>
        public static Boolean TryParse(PullPricingProductDataRequest                                 Request,
                                       JObject                                                       JSON,
                                       DateTime                                                      ResponseTimestamp,
                                       EventTracking_Id                                              EventTrackingId,
                                       TimeSpan                                                      Runtime,
                                       [NotNullWhen(true)]  out PullPricingProductDataResponse?      PullPricingProductDataResponse,
                                       [NotNullWhen(false)] out String?                              ErrorResponse,
                                       Process_Id?                                                   ProcessId                                    = null,
                                       HTTPResponse?                                                 HTTPResponse                                 = null,
                                       CustomJObjectParserDelegate<PullPricingProductDataResponse>?  CustomPullPricingProductDataResponseParser   = null)
        {

            try
            {

                PullPricingProductDataResponse = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse OperatorPricingProducts    [mandatory]

                if (!JSON.ParseMandatory("OperatorPricingProducts",
                                         "operator pricing products",
                                         out JArray PricingProductDataJSON,
                                         out ErrorResponse))
                {
                    return false;
                }

                var PricingProductData  = new List<PricingProductData>();
                var Warnings            = new List<Warning>();

                foreach (var pricingProductDataJSON in PricingProductDataJSON)
                {

                    try
                    {

                        var ErrorResponse2 = String.Empty;

                        if (pricingProductDataJSON is JObject pricingProductDataJObject &&
                            OICPv2_3.PricingProductData.TryParse(pricingProductDataJObject,
                                                                 out PricingProductData?  pricingProductData,
                                                                 out                      ErrorResponse2))
                        {
                            PricingProductData.Add(pricingProductData!);
                        }

                        else
                        {

                            if (pricingProductDataJSON is JObject pricingProductDataJObject2)
                                ErrorResponse2 = "EVSE " + pricingProductDataJObject2["EvseID"]?.Value<String>() + ": " + ErrorResponse2;

                            if (ErrorResponse2 is not null)
                                Warnings.Add(Warning.Create(ErrorResponse2));

                        }

                    }
                    catch (Exception e)
                    {

                        var message = e.Message;

                        if (pricingProductDataJSON is JObject pricingProductDataJObject2)
                            message = "EVSE " + pricingProductDataJObject2["EvseID"]?.Value<String>() + ": " + message;

                        Warnings.Add(Warning.Create(message));

                    }

                }

                //if (!JSON.ParseMandatoryJSON("content",
                //                             "EVSE data",
                //                             PricingProductData.TryParse,
                //                             out IEnumerable<PricingProductData> PricingProductData,
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


                PullPricingProductDataResponse = new PullPricingProductDataResponse(
                                                     ResponseTimestamp,
                                                     EventTrackingId,
                                                     ProcessId ?? Process_Id.NewRandom(),
                                                     Runtime,
                                                     PricingProductData,

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
                                                     Warnings
                                                 );

                if (CustomPullPricingProductDataResponseParser is not null)
                    PullPricingProductDataResponse = CustomPullPricingProductDataResponseParser(JSON,
                                                                                                PullPricingProductDataResponse);

                return true;

            }
            catch (Exception e)
            {
                PullPricingProductDataResponse  = default;
                ErrorResponse                   = "The given JSON representation of a PullPricingProductData response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPullPricingProductDataResponseSerializer = null, CustomPricingProductDataSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullPricingProductDataResponseSerializer">A delegate to customize the serialization of PullPricingProductData responses.</param>
        /// <param name="CustomPricingProductDataSerializer">A delegate to serialize custom pricing product data JSON objects.</param>
        /// <param name="CustomPricingProductDataRecordSerializer">A delegate to serialize custom pricing product data record JSON objects.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullPricingProductDataResponse>?  CustomPullPricingProductDataResponseSerializer   = null,
                              CustomJObjectSerializerDelegate<PricingProductData>?              CustomPricingProductDataSerializer               = null,
                              CustomJObjectSerializerDelegate<PricingProductDataRecord>?        CustomPricingProductDataRecordSerializer         = null,
                              CustomJObjectSerializerDelegate<StatusCode>?                      CustomStatusCodeSerializer                       = null)
        {

            var json = JSONObject.Create(

                           new JProperty("OperatorPricingProducts",  new JArray(PricingProductData.Select(pricingProductData => pricingProductData.ToJSON(CustomPricingProductDataSerializer,
                                                                                                                                                          CustomPricingProductDataRecordSerializer)))),

                           StatusCode is not null
                               ? new JProperty("StatusCode",         StatusCode.ToJSON(CustomStatusCodeSerializer))
                               : null,

                           CustomData is not null
                               ? new JProperty("CustomData",         CustomData)
                               : null

                       );

            return CustomPullPricingProductDataResponseSerializer is not null
                       ? CustomPullPricingProductDataResponseSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullPricingProductDataResponse1, PullPricingProductDataResponse2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="PullPricingProductDataResponse1">A PullPricingProductData response.</param>
        /// <param name="PullPricingProductDataResponse2">Another PullPricingProductData response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullPricingProductDataResponse PullPricingProductDataResponse1,
                                           PullPricingProductDataResponse PullPricingProductDataResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PullPricingProductDataResponse1, PullPricingProductDataResponse2))
                return true;

            // If one is null, but not both, return false.
            if (PullPricingProductDataResponse1 is null || PullPricingProductDataResponse2 is null)
                return false;

            return PullPricingProductDataResponse1.Equals(PullPricingProductDataResponse2);

        }

        #endregion

        #region Operator != (PullPricingProductDataResponse1, PullPricingProductDataResponse2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="PullPricingProductDataResponse1">A PullPricingProductData response.</param>
        /// <param name="PullPricingProductDataResponse2">Another PullPricingProductData response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullPricingProductDataResponse PullPricingProductDataResponse1,
                                           PullPricingProductDataResponse PullPricingProductDataResponse2)

            => !(PullPricingProductDataResponse1 == PullPricingProductDataResponse2);

        #endregion

        #endregion

        #region IEquatable<PullPricingProductDataResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is PullPricingProductDataResponse pullPricingProductDataResponse &&
                   Equals(pullPricingProductDataResponse);

        #endregion

        #region Equals(PullPricingProductDataResponse)

        /// <summary>
        /// Compares two PullPricingProductData responses for equality.
        /// </summary>
        /// <param name="PullPricingProductDataResponse">A PullPricingProductData response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullPricingProductDataResponse? PullPricingProductDataResponse)

            => PullPricingProductDataResponse is not null &&

             (!PricingProductData.Any() && !PullPricingProductDataResponse.PricingProductData.Any() ||
               PricingProductData.Any() &&  PullPricingProductDataResponse.PricingProductData.Any() && PricingProductData.Count().Equals(PullPricingProductDataResponse.PricingProductData.Count())) &&

             ((StatusCode is     null && PullPricingProductDataResponse.StatusCode is     null) ||
              (StatusCode is not null && PullPricingProductDataResponse.StatusCode is not null && StatusCode.Equals(PullPricingProductDataResponse.StatusCode)));

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

                return PricingProductData.Aggregate(0, (hashCode, operatorEVSEData) => hashCode ^ operatorEVSEData.GetHashCode()) ^
                       (StatusCode?.GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(PricingProductData.Count() + " operator EVSE data record(s)",
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
                    PricingProductData,
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
        /// A PullPricingProductData response builder.
        /// </summary>
        public new class Builder : AResponse<PullPricingProductDataRequest,
                                             PullPricingProductDataResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// An enumeration of pricing product data grouped.
            /// </summary>
            public HashSet<PricingProductData>  PricingProductData      { get; }

            public UInt64?                      Number                  { get; set; }
            public UInt64?                      Size                    { get; set; }
            public UInt64?                      TotalElements           { get; set; }
            public Boolean?                     LastPage                { get; set; }
            public Boolean?                     FirstPage               { get; set; }
            public UInt64?                      TotalPages              { get; set; }
            public UInt64?                      NumberOfElements        { get; set; }

            /// <summary>
            /// The optional status code for this request.
            /// </summary>
            public StatusCode.Builder           StatusCode              { get; }

            #endregion

            #region Constructor(s)

#pragma warning disable IDE0290 // Use primary constructor

            /// <summary>
            /// Create a new PullPricingProductData response builder.
            /// </summary>
            /// <param name="Request">A PullPricingProductData request.</param>
            /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
            /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
            /// <param name="Runtime">The runtime of the request/response.</param>
            /// <param name="PricingProductData">An enumeration of pricing product data grouped by their operators.</param>
            /// <param name="StatusCode">An optional status code for this request.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(PullPricingProductDataRequest?    Request              = null,
                           DateTime?                         ResponseTimestamp    = null,
                           EventTracking_Id?                 EventTrackingId      = null,
                           TimeSpan?                         Runtime              = null,
                           IEnumerable<PricingProductData>?  PricingProductData   = null,

                           UInt64?                           Number               = null,
                           UInt64?                           Size                 = null,
                           UInt64?                           TotalElements        = null,
                           Boolean?                          LastPage             = null,
                           Boolean?                          FirstPage            = null,
                           UInt64?                           TotalPages           = null,
                           UInt64?                           NumberOfElements     = null,

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

                this.PricingProductData  = PricingProductData is not null
                                               ? new HashSet<PricingProductData>(PricingProductData)
                                               : [];

                this.Number              = Number;
                this.Size                = Size;
                this.TotalElements       = TotalElements;
                this.LastPage            = LastPage;
                this.FirstPage           = FirstPage;
                this.TotalPages          = TotalPages;
                this.NumberOfElements    = NumberOfElements;

                this.StatusCode          = StatusCode is not null
                                               ? StatusCode.ToBuilder()
                                               : new StatusCode.Builder();

            }

#pragma warning restore IDE0290 // Use primary constructor

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the PullPricingProductData response.
            /// </summary>
            /// <param name="Builder">A PullPricingProductDataResponse builder.</param>
            public static implicit operator PullPricingProductDataResponse(Builder Builder)

                => Builder.ToImmutable();


            /// <summary>
            /// Return an immutable version of the PullPricingProductData response.
            /// </summary>
            public override PullPricingProductDataResponse ToImmutable()

                => new (ResponseTimestamp ?? Timestamp.Now,
                        EventTrackingId   ?? EventTracking_Id.New,
                        ProcessId         ?? Process_Id.NewRandom(),
                        Runtime           ?? (Timestamp.Now - (Request?.Timestamp ?? Timestamp.Now)),
                        PricingProductData,

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
