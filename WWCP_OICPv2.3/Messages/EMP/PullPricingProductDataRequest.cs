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
    /// The PullPricingProductData request.
    /// </summary>
    public class PullPricingProductDataRequest : APagedRequest<PullPricingProductDataRequest>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the e-mobility provider.
        /// </summary>
        [Mandatory]
        public Provider_Id               ProviderId     { get; }

        /// <summary>
        /// The optional timestamp of the last call.
        /// </summary>
        public DateTime?                 LastCall       { get; }


        /// <summary>
        /// An enumeration of EVSE operator identifications to download pricing data from.
        /// </summary>
        public IEnumerable<Operator_Id>  OperatorIds    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullPricingProductData request.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="LastCall">An optional timestamp of the last call. Cannot be combined with 'SearchCenter'.</param>
        /// <param name="OperatorIds">An enumeration of EVSE operator identifications to download pricing data from.</param>
        /// 
        /// <param name="Page">An optional page number of the request page.</param>
        /// <param name="Size">An optional size of a request page.</param>
        /// <param name="SortOrder">Optional sorting criteria in the format: property(,asc|desc).</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public PullPricingProductDataRequest(Provider_Id               ProviderId,
                                             IEnumerable<Operator_Id>  OperatorIds,
                                             DateTime?                 LastCall            = null,

                                             Process_Id?               ProcessId           = null,
                                             UInt32?                   Page                = null,
                                             UInt32?                   Size                = null,
                                             IEnumerable<String>?      SortOrder           = null,
                                             JObject?                  CustomData          = null,

                                             DateTime?                 Timestamp           = null,
                                             EventTracking_Id?         EventTrackingId     = null,
                                             TimeSpan?                 RequestTimeout      = null,
                                             CancellationToken         CancellationToken   = default)

            : base(ProcessId,
                   Page,
                   Size,
                   SortOrder,
                   CustomData,

                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.ProviderId   = ProviderId;
            this.OperatorIds  = OperatorIds;
            this.LastCall     = LastCall;

            unchecked
            {

                hashCode = this.ProviderId. GetHashCode() * 5 ^
                           this.OperatorIds.GetHashCode() * 3 ^
                           this.LastCall?.  GetHashCode() ?? 0;

            }

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#611-eroamingpullpricingproductdata-message

        // {
        //   "LastCall":  "2021-01-09T09:16:26.888Z",
        //   "OperatorIds": [
        //     "string"
        //   ]
        // }

        #endregion

        #region (static) Parse   (JSON, ProviderId, ..., CustomPullPricingProductDataRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullPricingProductData request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="CustomPullPricingProductDataRequestParser">A delegate to parse custom PullPricingProductData JSON objects.</param>
        public static PullPricingProductDataRequest Parse(JObject                                                      JSON,
                                                          Provider_Id                                                  ProviderId,
                                                          Process_Id?                                                  ProcessId                                   = null,
                                                          UInt32?                                                      Page                                        = null,
                                                          UInt32?                                                      Size                                        = null,
                                                          IEnumerable<String>?                                         SortOrder                                   = null,

                                                          DateTime?                                                    Timestamp                                   = null,
                                                          EventTracking_Id?                                            EventTrackingId                             = null,
                                                          TimeSpan?                                                    RequestTimeout                              = null,
                                                          CustomJObjectParserDelegate<PullPricingProductDataRequest>?  CustomPullPricingProductDataRequestParser   = null,
                                                          CancellationToken                                            CancellationToken                           = default)
        {

            if (TryParse(JSON,
                         ProviderId,
                         out var pullEVSEDataResponse,
                         out var errorResponse,
                         ProcessId,
                         Page,
                         Size,
                         SortOrder,
                         Timestamp,
                         EventTrackingId,
                         RequestTimeout,
                         CustomPullPricingProductDataRequestParser,
                         CancellationToken))
            {
                return pullEVSEDataResponse;
            }

            throw new ArgumentException("The given JSON representation of a PullPricingProductData request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, ProviderId, out PullPricingProductDataRequest, out ErrorResponse, ..., CustomPullPricingProductDataRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PullPricingProductData request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="PullPricingProductDataRequest">The parsed PullPricingProductData request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPullPricingProductDataRequestParser">A delegate to parse custom PullPricingProductData request JSON objects.</param>
        public static Boolean TryParse(JObject                                                      JSON,
                                       Provider_Id                                                  ProviderId,
                                       [NotNullWhen(true)]  out PullPricingProductDataRequest?      PullPricingProductDataRequest,
                                       [NotNullWhen(false)] out String?                             ErrorResponse,
                                       Process_Id?                                                  ProcessId                                   = null,
                                       UInt32?                                                      Page                                        = null,
                                       UInt32?                                                      Size                                        = null,
                                       IEnumerable<String>?                                         SortOrder                                   = null,

                                       DateTime?                                                    Timestamp                                   = null,
                                       EventTracking_Id?                                            EventTrackingId                             = null,
                                       TimeSpan?                                                    RequestTimeout                              = null,
                                       CustomJObjectParserDelegate<PullPricingProductDataRequest>?  CustomPullPricingProductDataRequestParser   = null,
                                       CancellationToken                                            CancellationToken                           = default)
        {

            try
            {

                PullPricingProductDataRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse OperatorIds    [mandatory]

                if (JSON.ParseOptionalJSON("OperatorIDs",
                                           "operator identifications",
                                           Operator_Id.TryParse,
                                           out IEnumerable<Operator_Id> OperatorIds,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse LastCall       [optional]

                if (JSON.ParseOptional("LastCall",
                                       "last call",
                                       out DateTime? LastCall,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CustomData     [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                PullPricingProductDataRequest = new PullPricingProductDataRequest(

                                                    ProviderId,
                                                    OperatorIds,
                                                    LastCall,

                                                    ProcessId,
                                                    Page,
                                                    Size,
                                                    SortOrder,

                                                    customData,

                                                    Timestamp,
                                                    EventTrackingId,
                                                    RequestTimeout,
                                                    CancellationToken

                                                );

                if (CustomPullPricingProductDataRequestParser is not null)
                    PullPricingProductDataRequest = CustomPullPricingProductDataRequestParser(JSON,
                                                                                              PullPricingProductDataRequest);

                return true;

            }
            catch (Exception e)
            {
                PullPricingProductDataRequest  = default;
                ErrorResponse                  = "The given JSON representation of a PullPricingProductData request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPullPricingProductDataRequestSerializer = null)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullPricingProductDataRequestSerializer">A delegate to customize the serialization of PullPricingProductDataRequest responses.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullPricingProductDataRequest>?  CustomPullPricingProductDataRequestSerializer   = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("OperatorIDs",   new JArray(OperatorIds.Select(operatorId => operatorId.ToString()))),

                           LastCall.HasValue
                               ? new JProperty("LastCall",      LastCall.Value.ToIso8601())
                               : null,

                           CustomData is not null
                               ? new JProperty("CustomData",    CustomData)
                               : null

                       );

            return CustomPullPricingProductDataRequestSerializer is not null
                       ? CustomPullPricingProductDataRequestSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullPricingProductData1, PullPricingProductData2)

        /// <summary>
        /// Compares two pull EVSE data requests for equality.
        /// </summary>
        /// <param name="PullPricingProductData1">An pull EVSE data request.</param>
        /// <param name="PullPricingProductData2">Another pull EVSE data request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullPricingProductDataRequest PullPricingProductData1,
                                           PullPricingProductDataRequest PullPricingProductData2)
        {

            if (ReferenceEquals(PullPricingProductData1, PullPricingProductData2))
                return true;

            if (PullPricingProductData1 is null || PullPricingProductData2 is null)
                return false;

            return PullPricingProductData1.Equals(PullPricingProductData2);

        }

        #endregion

        #region Operator != (PullPricingProductData1, PullPricingProductData2)

        /// <summary>
        /// Compares two pull EVSE data requests for inequality.
        /// </summary>
        /// <param name="PullPricingProductData1">An pull EVSE data request.</param>
        /// <param name="PullPricingProductData2">Another pull EVSE data request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullPricingProductDataRequest PullPricingProductData1,
                                           PullPricingProductDataRequest PullPricingProductData2)

            => !(PullPricingProductData1 == PullPricingProductData2);

        #endregion

        #endregion

        #region IEquatable<PullPricingProductDataRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is PullPricingProductDataRequest pullPricingProductDataRequest &&
                   Equals(pullPricingProductDataRequest);

        #endregion

        #region Equals(PullPricingProductData)

        /// <summary>
        /// Compares two pull EVSE data requests for equality.
        /// </summary>
        /// <param name="PullPricingProductData">An pull EVSE data request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullPricingProductDataRequest? PullPricingProductData)

            => PullPricingProductData is not null &&

               ProviderId.Equals(PullPricingProductData.ProviderId) &&

            ((!LastCall.HasValue && !PullPricingProductData.LastCall.HasValue) ||
              (LastCall.HasValue &&  PullPricingProductData.LastCall.HasValue && LastCall.Value.Equals(PullPricingProductData.LastCall.Value))) &&

               OperatorIds.Count().Equals(PullPricingProductData.OperatorIds.Count()) &&
               OperatorIds.All(operatorId => PullPricingProductData.OperatorIds.Contains(operatorId));

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

                   $"{ProviderId} => Operators: {OperatorIds.AggregateWith(", ")}",

                   LastCall.HasValue
                       ? $"; last call: {LastCall}"
                       : ""

               );

        #endregion

    }

}
