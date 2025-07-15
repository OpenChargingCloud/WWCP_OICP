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
    /// The PushPricingProductData request.
    /// </summary>
    public class PushPricingProductDataRequest : ARequest<PushPricingProductDataRequest>
    {

        #region Properties

        /// <summary>
        /// The pricing product data record.
        /// </summary>
        [Mandatory]
        public PricingProductData           PricingProductData    { get; }

        /// <summary>
        /// The server-side data management operation.
        /// </summary>
        [Mandatory]
        public ActionTypes                  Action              { get; }

        /// <summary>
        /// The enumeration of pricing product data records.
        /// </summary>
        public IEnumerable<PricingProductDataRecord>  PricingProductDataRecords
            => PricingProductData.PricingProductDataRecords;

        /// <summary>
        /// The unique identification of the charging station operator maintaining the given pricing product data records.
        /// </summary>
        public Operator_Id                  OperatorId
            => PricingProductData.OperatorId;

        /// <summary>
        /// The optional name of the charging station operator maintaining the given pricing product data records.
        /// </summary>
        public String?                      OperatorName
            => PricingProductData.OperatorName;

        #endregion

        #region Constructor(s)

#pragma warning disable IDE0290 // Use primary constructor

        /// <summary>
        /// Create a new PushPricingProductData request.
        /// </summary>
        /// <param name="PricingProductData">The pricing product data record.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public PushPricingProductDataRequest(PricingProductData  PricingProductData,
                                             ActionTypes         Action              = ActionTypes.FullLoad,
                                             Process_Id?         ProcessId           = null,
                                             JObject?            CustomData          = null,

                                             DateTime?           Timestamp           = null,
                                             EventTracking_Id?   EventTrackingId     = null,
                                             TimeSpan?           RequestTimeout      = null,
                                             CancellationToken   CancellationToken   = default)

            : base(ProcessId,
                   CustomData,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.PricingProductData  = PricingProductData ?? throw new ArgumentNullException(nameof(PricingProductData), "The given pricing product data must not be null!");
            this.Action              = Action;

        }

#pragma warning restore IDE0290 // Use primary constructor

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#eRoamingPushEvseData

        // {
        //   "ActionType":         "fullLoad",
        //   "PricingProductData": {
        //     {
        //         ...
        //     }
        //   }
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomPushPricingProductDataRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a push pricing product data request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomPushPricingProductDataRequestParser">A delegate to parse custom push pricing product data request JSON objects.</param>
        public static PushPricingProductDataRequest Parse(JObject                                                      JSON,
                                                          Process_Id?                                                  ProcessId                                   = null,

                                                          DateTime?                                                    Timestamp                                   = null,
                                                          EventTracking_Id?                                            EventTrackingId                             = null,
                                                          TimeSpan?                                                    RequestTimeout                              = null,
                                                          CustomJObjectParserDelegate<PushPricingProductDataRequest>?  CustomPushPricingProductDataRequestParser   = null,
                                                          CancellationToken                                            CancellationToken                           = default)
        {

            if (TryParse(JSON,
                         out var pushEVSEDataRequest,
                         out var errorResponse,
                         ProcessId,
                         Timestamp,
                         EventTrackingId,
                         RequestTimeout,
                         CustomPushPricingProductDataRequestParser,
                         CancellationToken))
            {
                return pushEVSEDataRequest;
            }

            throw new ArgumentException("The given JSON representation of a push pricing product data request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out PushPricingProductDataRequest, out ErrorResponse, ..., CustomPushPricingProductDataRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a push pricing product data request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="PushPricingProductDataRequest">The parsed push pricing product data request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomPushPricingProductDataRequestParser">A delegate to parse custom push pricing product data request JSON objects.</param>
        public static Boolean TryParse(JObject                                                      JSON,
                                       [NotNullWhen(true)]  out PushPricingProductDataRequest?      PushPricingProductDataRequest,
                                       [NotNullWhen(false)] out String?                             ErrorResponse,
                                       Process_Id?                                                  ProcessId                                   = null,

                                       DateTime?                                                    Timestamp                                   = null,
                                       EventTracking_Id?                                            EventTrackingId                             = null,
                                       TimeSpan?                                                    RequestTimeout                              = null,
                                       CustomJObjectParserDelegate<PushPricingProductDataRequest>?  CustomPushPricingProductDataRequestParser   = null,
                                       CancellationToken                                            CancellationToken                           = default)
        {

            try
            {

                PushPricingProductDataRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse ActionType            [mandatory]

                if (!JSON.ParseMandatory("ActionType",
                                         "action type",
                                         ActionTypesExtensions.TryParse,
                                         out ActionTypes ActionType,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse PricingProductData    [mandatory]

                if (!JSON.ParseMandatoryJSON("PricingProductData",
                                             "pricing product data",
                                             OICPv2_3.PricingProductData.TryParse,
                                             out PricingProductData? PricingProductData,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse CustomData            [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                PushPricingProductDataRequest = new PushPricingProductDataRequest(
                                                    PricingProductData,
                                                    ActionType,
                                                    ProcessId,
                                                    customData,

                                                    Timestamp,
                                                    EventTrackingId,
                                                    RequestTimeout,
                                                    CancellationToken
                                                );

                if (CustomPushPricingProductDataRequestParser is not null)
                    PushPricingProductDataRequest = CustomPushPricingProductDataRequestParser(JSON,
                                                                                              PushPricingProductDataRequest);

                return true;

            }
            catch (Exception e)
            {
                PushPricingProductDataRequest  = default;
                ErrorResponse        = "The given JSON representation of a push pricing product data request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPushPricingProductDataRequestSerializer = null, CustomPricingProductDataSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomPushPricingProductDataRequestSerializer">A delegate to serialize custom PushPricingProductData requests.</param>
        /// <param name="CustomPricingProductDataSerializer">A delegate to serialize custom pricing product data JSON objects.</param>
        /// <param name="CustomPricingProductDataRecordSerializer">A delegate to serialize custom pricing product data record JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PushPricingProductDataRequest>?  CustomPushPricingProductDataRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<PricingProductData>?             CustomPricingProductDataSerializer              = null,
                              CustomJObjectSerializerDelegate<PricingProductDataRecord>?       CustomPricingProductDataRecordSerializer        = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("ActionType",           Action.AsString()),

                                 new JProperty("PricingProductData",   PricingProductData.ToJSON(CustomPricingProductDataSerializer,
                                                                                                 CustomPricingProductDataRecordSerializer)),

                           CustomData is not null
                               ? new JProperty("CustomData",           CustomData)
                               : null

                       );

            return CustomPushPricingProductDataRequestSerializer is not null
                       ? CustomPushPricingProductDataRequestSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this push pricing product data request.
        /// </summary>
        public PushPricingProductDataRequest Clone()

            => new (
                   PricingProductData.Clone(),
                   Action
               );

        #endregion


        #region Operator overloading

        #region Operator == (PushPricingProductData1, PushPricingProductData2)

        /// <summary>
        /// Compares two push pricing product data requests for equality.
        /// </summary>
        /// <param name="PushPricingProductData1">An push pricing product data request.</param>
        /// <param name="PushPricingProductData2">Another push pricing product data request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PushPricingProductDataRequest PushPricingProductData1,
                                           PushPricingProductDataRequest PushPricingProductData2)
        {

            if (ReferenceEquals(PushPricingProductData1, PushPricingProductData2))
                return true;

            if (PushPricingProductData1 is null || PushPricingProductData2 is null)
                return false;

            return PushPricingProductData1.Equals(PushPricingProductData2);

        }

        #endregion

        #region Operator != (PushPricingProductData1, PushPricingProductData2)

        /// <summary>
        /// Compares two push pricing product data requests for inequality.
        /// </summary>
        /// <param name="PushPricingProductData1">An push pricing product data request.</param>
        /// <param name="PushPricingProductData2">Another push pricing product data request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PushPricingProductDataRequest PushPricingProductData1,
                                           PushPricingProductDataRequest PushPricingProductData2)

            => !(PushPricingProductData1 == PushPricingProductData2);

        #endregion

        #endregion

        #region IEquatable<PushPricingProductDataRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is PushPricingProductDataRequest pushEVSEDataRequest &&
                   Equals(pushEVSEDataRequest);

        #endregion

        #region Equals(PushPricingProductDataRequest)

        /// <summary>
        /// Compares two push pricing product data requests for equality.
        /// </summary>
        /// <param name="PushPricingProductDataRequest">An push pricing product data request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PushPricingProductDataRequest? PushPricingProductDataRequest)

            => PushPricingProductDataRequest is not null &&

               PricingProductData.Equals(PushPricingProductDataRequest.PricingProductData) &&
               Action.            Equals(PushPricingProductDataRequest.Action);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return PricingProductData.GetHashCode() * 3 ^
                       Action.            GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{Action} of {PricingProductDataRecords.Count()} pricing product data record(s) by {OperatorName} ({OperatorId})";

        #endregion

    }

}
