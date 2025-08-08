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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The PullEVSEPricing request.
    /// </summary>
    public class PullEVSEPricingRequest : APagedRequest<PullEVSEPricingRequest>
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
        [Optional]
        public DateTimeOffset?           LastCall       { get; }


        /// <summary>
        /// An enumeration of EVSE operator identifications to download pricing data from.
        /// </summary>
        [Mandatory]
        public IEnumerable<Operator_Id>  OperatorIds    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullEVSEPricing request.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="OperatorIds">An enumeration of EVSE operator identifications to download pricing data from.</param>
        /// <param name="LastCall">An optional timestamp of the last call. Cannot be combined with 'SearchCenter'.</param>
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
        public PullEVSEPricingRequest(Provider_Id               ProviderId,
                                      IEnumerable<Operator_Id>  OperatorIds,
                                      DateTimeOffset?           LastCall            = null,

                                      Process_Id?               ProcessId           = null,
                                      UInt32?                   Page                = null,
                                      UInt32?                   Size                = null,
                                      IEnumerable<String>?      SortOrder           = null,
                                      JObject?                  CustomData          = null,

                                      DateTimeOffset?           Timestamp           = null,
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

                hashCode = this.ProviderId. GetHashCode()  * 5 ^
                           this.OperatorIds.CalcHashCode() * 3 ^
                           this.LastCall?.  GetHashCode() ?? 0;

            }

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#621-eroamingpullevsepricing-message

        // {
        //   "ProviderID":  "DE-GDF",
        //   "LastCall":    "2021-01-09T09:16:26.888Z",
        //   "OperatorIds": [
        //     "string"
        //   ]
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomPullEVSEPricingRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullEVSEPricing request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomPullEVSEPricingRequestParser">A delegate to parse custom PullEVSEPricing JSON objects.</param>
        public static PullEVSEPricingRequest Parse(JObject                                               JSON,
                                                   Process_Id?                                           ProcessId                            = null,
                                                   UInt32?                                               Page                                 = null,
                                                   UInt32?                                               Size                                 = null,
                                                   IEnumerable<String>?                                  SortOrder                            = null,

                                                   DateTimeOffset?                                       Timestamp                            = null,
                                                   EventTracking_Id?                                     EventTrackingId                      = null,
                                                   TimeSpan?                                             RequestTimeout                       = null,
                                                   CustomJObjectParserDelegate<PullEVSEPricingRequest>?  CustomPullEVSEPricingRequestParser   = null,
                                                   CancellationToken                                     CancellationToken                    = default)
        {

            if (TryParse(JSON,
                         out var pullEVSEPricingRequest,
                         out var errorResponse,
                         ProcessId,
                         Page,
                         Size,
                         SortOrder,
                         Timestamp,
                         EventTrackingId,
                         RequestTimeout,
                         CustomPullEVSEPricingRequestParser,
                         CancellationToken))
            {
                return pullEVSEPricingRequest;
            }

            throw new ArgumentException("The given JSON representation of a PullEVSEPricing request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out PullEVSEPricingRequest, out ErrorResponse, ..., CustomPullEVSEPricingRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PullEVSEPricing request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="PullEVSEPricingRequest">The parsed PullEVSEPricing request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPullEVSEPricingRequestParser">A delegate to parse custom PullEVSEPricing request JSON objects.</param>
        public static Boolean TryParse(JObject                                               JSON,
                                       [NotNullWhen(true)]  out PullEVSEPricingRequest?      PullEVSEPricingRequest,
                                       [NotNullWhen(false)] out String?                      ErrorResponse,
                                       Process_Id?                                           ProcessId                            = null,
                                       UInt32?                                               Page                                 = null,
                                       UInt32?                                               Size                                 = null,
                                       IEnumerable<String>?                                  SortOrder                            = null,

                                       DateTimeOffset?                                       Timestamp                            = null,
                                       EventTracking_Id?                                     EventTrackingId                      = null,
                                       TimeSpan?                                             RequestTimeout                       = null,
                                       CustomJObjectParserDelegate<PullEVSEPricingRequest>?  CustomPullEVSEPricingRequestParser   = null,
                                       CancellationToken                                     CancellationToken                    = default)
        {

            try
            {

                PullEVSEPricingRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse ProviderId     [mandatory]

                if (!JSON.ParseMandatory("ProviderID",
                                         "provider identification",
                                         Provider_Id.TryParse,
                                         out Provider_Id ProviderId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

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


                PullEVSEPricingRequest = new PullEVSEPricingRequest(
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

                if (CustomPullEVSEPricingRequestParser is not null)
                    PullEVSEPricingRequest = CustomPullEVSEPricingRequestParser(JSON,
                                                                                PullEVSEPricingRequest);

                return true;

            }
            catch (Exception e)
            {
                PullEVSEPricingRequest  = default;
                ErrorResponse           = "The given JSON representation of a PullEVSEPricing request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPullEVSEPricingRequestSerializer = null)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEPricingRequestSerializer">A delegate to customize the serialization of PullEVSEPricingRequest responses.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullEVSEPricingRequest>?  CustomPullEVSEPricingRequestSerializer   = null)
        {

            var json = JSONObject.Create(

                           new JProperty("ProviderID",        ProviderId.ToString()),
                           new JProperty("OperatorIDs",       new JArray(OperatorIds.Select(operatorId => operatorId.ToString()))),

                           LastCall.HasValue
                               ? new JProperty("LastCall",    LastCall.Value.ToISO8601())
                               : null,

                           CustomData is not null
                               ? new JProperty("CustomData",  CustomData)
                               : null

                       );

            return CustomPullEVSEPricingRequestSerializer is not null
                       ? CustomPullEVSEPricingRequestSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullEVSEPricing1, PullEVSEPricing2)

        /// <summary>
        /// Compares two pull EVSE data requests for equality.
        /// </summary>
        /// <param name="PullEVSEPricing1">An pull EVSE data request.</param>
        /// <param name="PullEVSEPricing2">Another pull EVSE data request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullEVSEPricingRequest PullEVSEPricing1,
                                           PullEVSEPricingRequest PullEVSEPricing2)
        {

            if (ReferenceEquals(PullEVSEPricing1, PullEVSEPricing2))
                return true;

            if (PullEVSEPricing1 is null || PullEVSEPricing2 is null)
                return false;

            return PullEVSEPricing1.Equals(PullEVSEPricing2);

        }

        #endregion

        #region Operator != (PullEVSEPricing1, PullEVSEPricing2)

        /// <summary>
        /// Compares two pull EVSE data requests for inequality.
        /// </summary>
        /// <param name="PullEVSEPricing1">An pull EVSE data request.</param>
        /// <param name="PullEVSEPricing2">Another pull EVSE data request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullEVSEPricingRequest PullEVSEPricing1,
                                           PullEVSEPricingRequest PullEVSEPricing2)

            => !(PullEVSEPricing1 == PullEVSEPricing2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEPricingRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is PullEVSEPricingRequest pullEVSEPricingRequest &&
                   Equals(pullEVSEPricingRequest);

        #endregion

        #region Equals(PullEVSEPricing)

        /// <summary>
        /// Compares two pull EVSE data requests for equality.
        /// </summary>
        /// <param name="PullEVSEPricing">An pull EVSE data request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEPricingRequest? PullEVSEPricing)

            => PullEVSEPricing is not null &&

               ProviderId.Equals(PullEVSEPricing.ProviderId) &&

            ((!LastCall.HasValue && !PullEVSEPricing.LastCall.HasValue) ||
              (LastCall.HasValue &&  PullEVSEPricing.LastCall.HasValue && LastCall.Value.Equals(PullEVSEPricing.LastCall.Value))) &&

               OperatorIds.Count().Equals(PullEVSEPricing.OperatorIds.Count()) &&
               OperatorIds.All(operatorId => PullEVSEPricing.OperatorIds.Contains(operatorId));

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
