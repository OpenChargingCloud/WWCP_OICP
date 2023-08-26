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
    /// The PullAuthenticationData response.
    /// </summary>
    public class PullAuthenticationDataResponse : AResponse<PullAuthenticationDataRequest,
                                                  PullAuthenticationDataResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of provider authentication data.
        /// </summary>
        [Mandatory]
        public IEnumerable<ProviderAuthenticationData>  ProviderAuthenticationData    { get; }


        public UInt64?                                  Number                        { get; }
        public UInt64?                                  Size                          { get; }
        public UInt64?                                  TotalElements                 { get; }
        public Boolean?                                 LastPage                      { get; }
        public Boolean?                                 FirstPage                     { get; }
        public UInt64?                                  TotalPages                    { get; }
        public UInt64?                                  NumberOfElements              { get; }


        /// <summary>
        /// The optional status code of this response.
        /// </summary>
        [Optional]
        public StatusCode?                              StatusCode                    { get; }

        /// <summary>
        /// Optional warnings.
        /// </summary>
        public IEnumerable<Warning>?                    Warnings                      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullAuthenticationData response.
        /// </summary>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProviderAuthenticationData">An enumeration of provider authentication data.</param>
        /// 
        /// <param name="Request">An optional PullAuthenticationData request.</param>
        /// 
        /// <param name="StatusCode">An optional status code of this response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// <param name="Warnings">Optional warnings.</param>
        public PullAuthenticationDataResponse(DateTime                                 ResponseTimestamp,
                                              EventTracking_Id                         EventTrackingId,
                                              Process_Id                               ProcessId,
                                              TimeSpan                                 Runtime,
                                              IEnumerable<ProviderAuthenticationData>  ProviderAuthenticationData,

                                              PullAuthenticationDataRequest?           Request            = null,
                                              UInt64?                                  Number             = null,
                                              UInt64?                                  Size               = null,
                                              UInt64?                                  TotalElements      = null,
                                              Boolean?                                 LastPage           = null,
                                              Boolean?                                 FirstPage          = null,
                                              UInt64?                                  TotalPages         = null,
                                              UInt64?                                  NumberOfElements   = null,

                                              StatusCode?                              StatusCode         = null,
                                              HTTPResponse?                            HTTPResponse       = null,
                                              JObject?                                 CustomData         = null,
                                              IEnumerable<Warning>?                    Warnings           = null)

            : base(ResponseTimestamp,
                   EventTrackingId,
                   ProcessId,
                   Runtime,
                   Request,
                   HTTPResponse,
                   CustomData)

        {

            this.ProviderAuthenticationData  = ProviderAuthenticationData ?? throw new ArgumentNullException(nameof(ProviderAuthenticationData), "The given enumeration of provider authentication data must not be null!");

            this.Number                      = Number;
            this.Size                        = Size;
            this.TotalElements               = TotalElements;
            this.LastPage                    = LastPage;
            this.FirstPage                   = FirstPage;
            this.TotalPages                  = TotalPages;
            this.NumberOfElements            = NumberOfElements;

            this.StatusCode                  = StatusCode;
            this.Warnings                    = Warnings;

        }

        #endregion


        #region Documentation

        // ???

        // {

        // }

        #endregion

        #region (static) Parse   (JSON, CustomPullAuthenticationDataResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullAuthenticationData response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullAuthenticationDataResponseParser">A delegate to parse custom PullAuthenticationData JSON objects.</param>
        public static PullAuthenticationDataResponse Parse(PullAuthenticationDataRequest                                 Request,
                                                           JObject                                                       JSON,
                                                           DateTime                                                      ResponseTimestamp,
                                                           EventTracking_Id                                              EventTrackingId,
                                                           TimeSpan                                                      Runtime,
                                                           Process_Id?                                                   ProcessId                                    = null,
                                                           HTTPResponse?                                                 HTTPResponse                                 = null,
                                                           CustomJObjectParserDelegate<PullAuthenticationDataResponse>?  CustomPullAuthenticationDataResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         out var pullAuthenticationDataResponse,
                         out var errorResponse,
                         ProcessId,
                         HTTPResponse,
                         CustomPullAuthenticationDataResponseParser))
            {
                return pullAuthenticationDataResponse!;
            }

            throw new ArgumentException("The given JSON representation of a PullAuthenticationData response is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out PullAuthenticationDataResponse, out ErrorResponse, CustomPullAuthenticationDataResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PullAuthenticationData response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="PullAuthenticationDataResponse">The parsed PullAuthenticationData response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullAuthenticationDataResponseParser">A delegate to parse custom PullAuthenticationData response JSON objects.</param>
        public static Boolean TryParse(PullAuthenticationDataRequest                                 Request,
                                       JObject                                                       JSON,
                                       DateTime                                                      ResponseTimestamp,
                                       EventTracking_Id                                              EventTrackingId,
                                       TimeSpan                                                      Runtime,
                                       out PullAuthenticationDataResponse?                           PullAuthenticationDataResponse,
                                       out String?                                                   ErrorResponse,
                                       Process_Id?                                                   ProcessId                                    = null,
                                       HTTPResponse?                                                 HTTPResponse                                 = null,
                                       CustomJObjectParserDelegate<PullAuthenticationDataResponse>?  CustomPullAuthenticationDataResponseParser   = null)
        {

            try
            {

                PullAuthenticationDataResponse = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse ProviderAuthenticationData    [mandatory]

                if (!JSON.ParseMandatory("ProviderAuthenticationData",
                                         "provider authentication data",
                                         out JArray ProviderAuthenticationDataJSON,
                                         out ErrorResponse))
                {
                    return false;
                }

                var operatorEVSEPricings  = new List<ProviderAuthenticationData>();
                var warnings              = new List<Warning>();

                foreach (var evseDataRecordJSON in ProviderAuthenticationDataJSON)
                {

                    try
                    {

                        var ErrorResponse2 = String.Empty;

                        if (evseDataRecordJSON is JObject evseDataRecordJObject &&
                            OICPv2_3.ProviderAuthenticationData.TryParse(evseDataRecordJObject,
                                                                         out ProviderAuthenticationData?  providerAuthenticationData,
                                                                         out                              ErrorResponse2))
                        {
                            operatorEVSEPricings.Add(providerAuthenticationData!);
                        }

                        else
                        {

                            if (evseDataRecordJSON is JObject evseDataRecordJObject2)
                                ErrorResponse2 = "EVSE " + evseDataRecordJObject2["EvseID"]?.Value<String>() + ": " + ErrorResponse2;

                            if (ErrorResponse2 is not null)
                                warnings.Add(Warning.Create(I18NString.Create(Languages.en, ErrorResponse2)));

                        }

                    }
                    catch (Exception e)
                    {

                        var message = e.Message;

                        if (evseDataRecordJSON is JObject evseDataRecordJObject2)
                            message = "EVSE " + evseDataRecordJObject2["EvseID"]?.Value<String>() + ": " + message;

                        warnings.Add(Warning.Create(I18NString.Create(Languages.en, message)));

                    }

                }

                //if (!JSON.ParseMandatoryJSON("content",
                //                             "EVSE data",
                //                             ProviderAuthenticationData.TryParse,
                //                             out IEnumerable<ProviderAuthenticationData> ProviderAuthenticationData,
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


                PullAuthenticationDataResponse = new PullAuthenticationDataResponse(ResponseTimestamp,
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

                if (CustomPullAuthenticationDataResponseParser is not null)
                    PullAuthenticationDataResponse = CustomPullAuthenticationDataResponseParser(JSON,
                                                                                                PullAuthenticationDataResponse);

                return true;

            }
            catch (Exception e)
            {
                PullAuthenticationDataResponse  = default;
                ErrorResponse                   = "The given JSON representation of a PullAuthenticationData response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPullAuthenticationDataResponseSerializer = null, CustomProviderAuthenticationDataSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullAuthenticationDataResponseSerializer">A delegate to customize the serialization of PullAuthenticationData responses.</param>
        /// <param name="CustomProviderAuthenticationDataSerializer">A delegate to serialize custom provider user identification data JSON objects.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON objects.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullAuthenticationDataResponse>?  CustomPullAuthenticationDataResponseSerializer   = null,
                              CustomJObjectSerializerDelegate<ProviderAuthenticationData>?      CustomProviderAuthenticationDataSerializer       = null,
                              CustomJObjectSerializerDelegate<Identification>?                  CustomIdentificationSerializer                   = null,
                              CustomJObjectSerializerDelegate<StatusCode>?                      CustomStatusCodeSerializer                       = null)
        {

            var json = JSONObject.Create(

                           new JProperty("ProviderAuthenticationData",  new JArray(ProviderAuthenticationData.Select(providerAuthenticationData => providerAuthenticationData.ToJSON(CustomProviderAuthenticationDataSerializer,
                                                                                                                                                                                     CustomIdentificationSerializer)))),

                           StatusCode is not null
                               ? new JProperty("StatusCode",     StatusCode.ToJSON(CustomStatusCodeSerializer))
                               : null,

                           CustomData is not null
                               ? new JProperty("CustomData",     CustomData)
                               : null

                       );

            return CustomPullAuthenticationDataResponseSerializer is not null
                       ? CustomPullAuthenticationDataResponseSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullAuthenticationDataResponse1, PullAuthenticationDataResponse2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="PullAuthenticationDataResponse1">A PullAuthenticationData response.</param>
        /// <param name="PullAuthenticationDataResponse2">Another PullAuthenticationData response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullAuthenticationDataResponse PullAuthenticationDataResponse1,
                                           PullAuthenticationDataResponse PullAuthenticationDataResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PullAuthenticationDataResponse1, PullAuthenticationDataResponse2))
                return true;

            // If one is null, but not both, return false.
            if (PullAuthenticationDataResponse1 is null || PullAuthenticationDataResponse2 is null)
                return false;

            return PullAuthenticationDataResponse1.Equals(PullAuthenticationDataResponse2);

        }

        #endregion

        #region Operator != (PullAuthenticationDataResponse1, PullAuthenticationDataResponse2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="PullAuthenticationDataResponse1">A PullAuthenticationData response.</param>
        /// <param name="PullAuthenticationDataResponse2">Another PullAuthenticationData response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullAuthenticationDataResponse PullAuthenticationDataResponse1,
                                           PullAuthenticationDataResponse PullAuthenticationDataResponse2)

            => !(PullAuthenticationDataResponse1 == PullAuthenticationDataResponse2);

        #endregion

        #endregion

        #region IEquatable<PullAuthenticationDataResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is PullAuthenticationDataResponse pullAuthenticationDataResponse &&
                   Equals(pullAuthenticationDataResponse);

        #endregion

        #region Equals(PullAuthenticationDataResponse)

        /// <summary>
        /// Compares two PullAuthenticationData responses for equality.
        /// </summary>
        /// <param name="PullAuthenticationDataResponse">A PullAuthenticationData response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullAuthenticationDataResponse? PullAuthenticationDataResponse)

            => PullAuthenticationDataResponse is not null &&

               (!ProviderAuthenticationData.Any() && !PullAuthenticationDataResponse.ProviderAuthenticationData.Any() ||
                 ProviderAuthenticationData.Any() &&  PullAuthenticationDataResponse.ProviderAuthenticationData.Any() && ProviderAuthenticationData.Count().Equals(PullAuthenticationDataResponse.ProviderAuthenticationData.Count())) &&

               ((StatusCode is     null && PullAuthenticationDataResponse.StatusCode is     null) ||
                (StatusCode is not null && PullAuthenticationDataResponse.StatusCode is not null && StatusCode.Equals(PullAuthenticationDataResponse.StatusCode)));

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

                return ProviderAuthenticationData.Aggregate(0, (hashCode, operatorEVSEPricing) => hashCode ^ operatorEVSEPricing.GetHashCode()) ^
                      (StatusCode?.GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(ProviderAuthenticationData.Count() + " provider authentication data",
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
                    ProviderAuthenticationData,
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
        /// A PullAuthenticationData response builder.
        /// </summary>
        public new class Builder : AResponse<PullAuthenticationDataRequest,
                                             PullAuthenticationDataResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// An enumeration of provider authentication data grouped.
            /// </summary>
            public HashSet<ProviderAuthenticationData>  ProviderAuthenticationData    { get; }

            public UInt64?                              Number                        { get; set; }
            public UInt64?                              Size                          { get; set; }
            public UInt64?                              TotalElements                 { get; set; }
            public Boolean?                             LastPage                      { get; set; }
            public Boolean?                             FirstPage                     { get; set; }
            public UInt64?                              TotalPages                    { get; set; }
            public UInt64?                              NumberOfElements              { get; set; }

            /// <summary>
            /// The optional status code for this request.
            /// </summary>
            public StatusCode.Builder                   StatusCode                    { get; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new PullAuthenticationData response builder.
            /// </summary>
            /// <param name="Request">A PullAuthenticationData request.</param>
            /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
            /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
            /// <param name="Runtime">The runtime of the request/response.</param>
            /// <param name="ProviderAuthenticationData">An enumeration of provider authentication data grouped by their operators.</param>
            /// <param name="StatusCode">An optional status code for this request.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(PullAuthenticationDataRequest?            Request                      = null,
                           DateTime?                                 ResponseTimestamp            = null,
                           EventTracking_Id?                         EventTrackingId              = null,
                           TimeSpan?                                 Runtime                      = null,
                           IEnumerable<ProviderAuthenticationData>?  ProviderAuthenticationData   = null,

                           UInt64?                                   Number                       = null,
                           UInt64?                                   Size                         = null,
                           UInt64?                                   TotalElements                = null,
                           Boolean?                                  LastPage                     = null,
                           Boolean?                                  FirstPage                    = null,
                           UInt64?                                   TotalPages                   = null,
                           UInt64?                                   NumberOfElements             = null,

                           StatusCode?                               StatusCode                   = null,
                           Process_Id?                               ProcessId                    = null,
                           HTTPResponse?                             HTTPResponse                 = null,
                           JObject?                                  CustomData                   = null)

                : base(ResponseTimestamp,
                       EventTrackingId,
                       Runtime,
                       Request,
                       HTTPResponse,
                       ProcessId,
                       CustomData)

            {

                this.ProviderAuthenticationData  = ProviderAuthenticationData is not null
                                                       ? new HashSet<ProviderAuthenticationData>(ProviderAuthenticationData)
                                                       : new HashSet<ProviderAuthenticationData>();

                this.Number                      = Number;
                this.Size                        = Size;
                this.TotalElements               = TotalElements;
                this.LastPage                    = LastPage;
                this.FirstPage                   = FirstPage;
                this.TotalPages                  = TotalPages;
                this.NumberOfElements            = NumberOfElements;

                this.StatusCode                  = StatusCode is not null
                                                       ? StatusCode.ToBuilder()
                                                       : new StatusCode.Builder();

            }

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the PullAuthenticationData response.
            /// </summary>
            /// <param name="Builder">A PullAuthenticationDataResponse builder.</param>
            public static implicit operator PullAuthenticationDataResponse(Builder Builder)

                => Builder.ToImmutable();


            /// <summary>
            /// Return an immutable version of the PullAuthenticationData response.
            /// </summary>
            public override PullAuthenticationDataResponse ToImmutable()

                => new (ResponseTimestamp ?? Timestamp.Now,
                        EventTrackingId   ?? EventTracking_Id.New,
                        ProcessId         ?? Process_Id.NewRandom(),
                        Runtime           ?? (Timestamp.Now - (Request?.Timestamp ?? Timestamp.Now)),
                        ProviderAuthenticationData,
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
