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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The PushEVSEPricing request.
    /// </summary>
    public class PushEVSEPricingRequest : ARequest<PushEVSEPricingRequest>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station operator maintaining the given EVSE pricing information.
        /// </summary>
        [Mandatory]
        public Operator_Id               OperatorId     { get; }

        /// <summary>
        /// The EVSE pricing data.
        /// </summary>
        [Mandatory]
        public IEnumerable<EVSEPricing>  EVSEPricing    { get; }

        /// <summary>
        /// The server-side data management operation.
        /// </summary>
        [Mandatory]
        public ActionTypes               Action         { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PushEVSEPricing request.
        /// </summary>
        /// <param name="EVSEPricing">The EVSE pricing data.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public PushEVSEPricingRequest(Operator_Id               OperatorId,
                                      IEnumerable<EVSEPricing>  EVSEPricing,
                                      ActionTypes               Action              = ActionTypes.FullLoad,
                                      Process_Id?               ProcessId           = null,
                                      JObject?                  CustomData          = null,

                                      DateTime?                 Timestamp           = null,
                                      EventTracking_Id?         EventTrackingId     = null,
                                      TimeSpan?                 RequestTimeout      = null,
                                      CancellationToken         CancellationToken   = default)

            : base(ProcessId,
                   CustomData,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.OperatorId   = OperatorId;
            this.EVSEPricing  = EVSEPricing ?? throw new ArgumentNullException(nameof(EVSEPricing), "The given EVSE pricing data must not be null!");
            this.Action       = Action;


            unchecked
            {

                hashCode = this.OperatorId. GetHashCode()  * 5 ^
                           this.EVSEPricing.CalcHashCode() * 3 ^
                           this.Action.     GetHashCode();

            }

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#eRoamingPushEvseData

        // {
        //     "ActionType":  "fullLoad",
        //     "EVSEPricing": [
        //         {
        //             "EvseID": "DE*XYZ*ETEST1",
        //             "EvseIDProductList": [
        //                 "AC 1"
        //             ],
        //             "ProviderID": "*"
        //         }
        //     ]
        // }

        #endregion

        #region (static) Parse   (JSON, OperatorId, ..., CustomPushEVSEPricingRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a push EVSE pricing data request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomPushEVSEPricingRequestParser">A delegate to parse custom push EVSE pricing data request JSON objects.</param>
        public static PushEVSEPricingRequest Parse(JObject                                               JSON,
                                                   Operator_Id                                           OperatorId,
                                                   Process_Id?                                           ProcessId                            = null,

                                                   DateTime?                                             Timestamp                            = null,
                                                   EventTracking_Id?                                     EventTrackingId                      = null,
                                                   TimeSpan?                                             RequestTimeout                       = null,
                                                   CustomJObjectParserDelegate<PushEVSEPricingRequest>?  CustomPushEVSEPricingRequestParser   = null,
                                                   CancellationToken                                     CancellationToken                    = default)
        {

            if (TryParse(JSON,
                         OperatorId,
                         out var pushEVSEDataRequest,
                         out var errorResponse,
                         ProcessId,
                         Timestamp,
                         EventTrackingId,
                         RequestTimeout,
                         CustomPushEVSEPricingRequestParser,
                         CancellationToken))
            {
                return pushEVSEDataRequest;
            }

            throw new ArgumentException("The given JSON representation of a push EVSE pricing data request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, OperatorId, out PushEVSEPricingRequest, out ErrorResponse, ..., CustomPushEVSEPricingRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a push EVSE pricing data request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="PushEVSEPricingRequest">The parsed push EVSE pricing data request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomPushEVSEPricingRequestParser">A delegate to parse custom push EVSE pricing data request JSON objects.</param>
        public static Boolean TryParse(JObject                                               JSON,
                                       Operator_Id                                           OperatorId,
                                       [NotNullWhen(true)]  out PushEVSEPricingRequest?      PushEVSEPricingRequest,
                                       [NotNullWhen(false)] out String?                      ErrorResponse,
                                       Process_Id?                                           ProcessId                            = null,

                                       DateTime?                                             Timestamp                            = null,
                                       EventTracking_Id?                                     EventTrackingId                      = null,
                                       TimeSpan?                                             RequestTimeout                       = null,
                                       CustomJObjectParserDelegate<PushEVSEPricingRequest>?  CustomPushEVSEPricingRequestParser   = null,
                                       CancellationToken                                     CancellationToken                    = default)
        {

            try
            {

                PushEVSEPricingRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse ActionType     [mandatory]

                if (!JSON.ParseMandatory("ActionType",
                                         "action type",
                                         ActionTypesExtensions.TryParse,
                                         out ActionTypes ActionType,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse EVSEPricing    [mandatory]

                if (!JSON.ParseMandatoryJSON("EVSEPricing",
                                             "EVSE pricing data",
                                             OICPv2_3.EVSEPricing.TryParse,
                                             out IEnumerable<EVSEPricing> EVSEPricing,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse CustomData     [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                PushEVSEPricingRequest = new PushEVSEPricingRequest(
                                             OperatorId,
                                             EVSEPricing,
                                             ActionType,
                                             ProcessId,
                                             customData,

                                             Timestamp,
                                             EventTrackingId,
                                             RequestTimeout,
                                             CancellationToken
                                         );

                if (CustomPushEVSEPricingRequestParser is not null)
                    PushEVSEPricingRequest = CustomPushEVSEPricingRequestParser(JSON,
                                                                                PushEVSEPricingRequest);

                return true;

            }
            catch (Exception e)
            {
                PushEVSEPricingRequest  = default;
                ErrorResponse           = "The given JSON representation of a push EVSE pricing data request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPushEVSEPricingRequestSerializer = null, CustomEVSEPricingSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomPushEVSEPricingRequestSerializer">A delegate to serialize custom PushEVSEPricing requests.</param>
        /// <param name="CustomEVSEPricingSerializer">A delegate to serialize custom EVSE pricing data JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PushEVSEPricingRequest>?  CustomPushEVSEPricingRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<EVSEPricing>?             CustomEVSEPricingSerializer              = null)
        {

            var json = JSONObject.Create(

                           new JProperty("ActionType",        Action.AsString()),

                           new JProperty("EVSEPricing",       new JArray(EVSEPricing.Select(evsePricing => evsePricing.ToJSON(CustomEVSEPricingSerializer)))),

                           CustomData is not null
                               ? new JProperty("CustomData",  CustomData)
                               : null

                       );

            return CustomPushEVSEPricingRequestSerializer is not null
                       ? CustomPushEVSEPricingRequestSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this push EVSE pricing data request.
        /// </summary>
        public PushEVSEPricingRequest Clone()

            => new (
                   OperatorId,
                   EVSEPricing.Select(evsePricing => evsePricing.Clone()),
                   Action
               );

        #endregion


        #region Operator overloading

        #region Operator == (PushEVSEPricing1, PushEVSEPricing2)

        /// <summary>
        /// Compares two push EVSE pricing data requests for equality.
        /// </summary>
        /// <param name="PushEVSEPricing1">An push EVSE pricing data request.</param>
        /// <param name="PushEVSEPricing2">Another push EVSE pricing data request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PushEVSEPricingRequest PushEVSEPricing1,
                                           PushEVSEPricingRequest PushEVSEPricing2)
        {

            if (ReferenceEquals(PushEVSEPricing1, PushEVSEPricing2))
                return true;

            if (PushEVSEPricing1 is null || PushEVSEPricing2 is null)
                return false;

            return PushEVSEPricing1.Equals(PushEVSEPricing2);

        }

        #endregion

        #region Operator != (PushEVSEPricing1, PushEVSEPricing2)

        /// <summary>
        /// Compares two push EVSE pricing data requests for inequality.
        /// </summary>
        /// <param name="PushEVSEPricing1">An push EVSE pricing data request.</param>
        /// <param name="PushEVSEPricing2">Another push EVSE pricing data request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PushEVSEPricingRequest PushEVSEPricing1,
                                           PushEVSEPricingRequest PushEVSEPricing2)

            => !(PushEVSEPricing1 == PushEVSEPricing2);

        #endregion

        #endregion

        #region IEquatable<PushEVSEPricingRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is PushEVSEPricingRequest pushEVSEDataRequest &&
                   Equals(pushEVSEDataRequest);

        #endregion

        #region Equals(PushEVSEPricingRequest)

        /// <summary>
        /// Compares two push EVSE pricing data requests for equality.
        /// </summary>
        /// <param name="PushEVSEPricingRequest">An push EVSE pricing data request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PushEVSEPricingRequest? PushEVSEPricingRequest)

            => PushEVSEPricingRequest is not null &&

               OperatorId. Equals(PushEVSEPricingRequest.OperatorId)  &&
               EVSEPricing.Equals(PushEVSEPricingRequest.EVSEPricing) &&
               Action.     Equals(PushEVSEPricingRequest.Action);

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

            => $"{Action} of {EVSEPricing.Count()} EVSE pricing record(s)";

        #endregion

    }

}
