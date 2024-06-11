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
    /// The PullAuthenticationData request.
    /// </summary>
    public class PullAuthenticationDataRequest : ARequest<PullAuthenticationDataRequest>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charge point operator.
        /// </summary>
        [Mandatory]
        public Operator_Id  OperatorId    { get; }

        #endregion

        #region Constructor(s)

#pragma warning disable IDE0290 // Use primary constructor

        /// <summary>
        /// Create a new PullAuthenticationData request.
        /// </summary>
        /// <param name="OperatorId">The unique identification of the charge point operator.</param>
        /// <param name="ProcessId">The optional unique OICP process identification.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public PullAuthenticationDataRequest(Operator_Id        OperatorId,
                                             Process_Id?        ProcessId           = null,
                                             JObject?           CustomData          = null,

                                             DateTime?          Timestamp           = null,
                                             EventTracking_Id?  EventTrackingId     = null,
                                             TimeSpan?          RequestTimeout      = null,
                                             CancellationToken  CancellationToken   = default)

            : base(ProcessId,
                   CustomData,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.OperatorId = OperatorId;

        }

#pragma warning restore IDE0290 // Use primary constructor

        #endregion


        #region Documentation

        // ???

        // {
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomPullAuthenticationDataRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullAuthenticationData request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomPullAuthenticationDataRequestParser">A delegate to parse custom PullAuthenticationData JSON objects.</param>
        public static PullAuthenticationDataRequest Parse(JObject                                                      JSON,
                                                          Process_Id?                                                  ProcessId                                   = null,

                                                          DateTime?                                                    Timestamp                                   = null,
                                                          EventTracking_Id?                                            EventTrackingId                             = null,
                                                          TimeSpan?                                                    RequestTimeout                              = null,
                                                          CustomJObjectParserDelegate<PullAuthenticationDataRequest>?  CustomPullAuthenticationDataRequestParser   = null,
                                                          CancellationToken                                            CancellationToken                           = default)
        {

            if (TryParse(JSON,
                         out var pullAuthenticationDataRequest,
                         out var errorResponse,
                         ProcessId,
                         Timestamp,
                         EventTrackingId,
                         RequestTimeout,
                         CustomPullAuthenticationDataRequestParser,
                         CancellationToken))
            {
                return pullAuthenticationDataRequest;
            }

            throw new ArgumentException("The given JSON representation of a PullAuthenticationData request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out PullAuthenticationDataRequest, out ErrorResponse, ..., CustomPullAuthenticationDataRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PullAuthenticationData request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="PullAuthenticationDataRequest">The parsed PullAuthenticationData request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPullAuthenticationDataRequestParser">A delegate to parse custom PullAuthenticationData request JSON objects.</param>
        public static Boolean TryParse(JObject                                                      JSON,
                                       [NotNullWhen(true)]  out PullAuthenticationDataRequest?      PullAuthenticationDataRequest,
                                       [NotNullWhen(false)] out String?                             ErrorResponse,
                                       Process_Id?                                                  ProcessId                                   = null,

                                       DateTime?                                                    Timestamp                                   = null,
                                       EventTracking_Id?                                            EventTrackingId                             = null,
                                       TimeSpan?                                                    RequestTimeout                              = null,
                                       CustomJObjectParserDelegate<PullAuthenticationDataRequest>?  CustomPullAuthenticationDataRequestParser   = null,
                                       CancellationToken                                            CancellationToken                           = default)
        {

            try
            {

                PullAuthenticationDataRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse OperatorId    [mandatory]

                if (!JSON.ParseMandatory("OperatorID",
                                         "charge point operator identification",
                                         Operator_Id.TryParse,
                                         out Operator_Id OperatorId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse CustomData    [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                PullAuthenticationDataRequest = new PullAuthenticationDataRequest(
                                                    OperatorId,
                                                    ProcessId,
                                                    customData,

                                                    Timestamp,
                                                    EventTrackingId,
                                                    RequestTimeout,
                                                    CancellationToken
                                                );

                if (CustomPullAuthenticationDataRequestParser is not null)
                    PullAuthenticationDataRequest = CustomPullAuthenticationDataRequestParser(JSON,
                                                                                              PullAuthenticationDataRequest);

                return true;

            }
            catch (Exception e)
            {
                PullAuthenticationDataRequest  = default;
                ErrorResponse                  = "The given JSON representation of a PullAuthenticationData request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPullAuthenticationDataRequestSerializer = null, CustomGeoCoordinatesSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullAuthenticationDataRequestSerializer">A delegate to customize the serialization of PullAuthenticationDataRequest responses.</param>
        /// <param name="CustomGeoCoordinatesSerializer">A delegate to serialize custom geo coordinates JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullAuthenticationDataRequest>?  CustomPullAuthenticationDataRequestSerializer   = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("OperatorID",   OperatorId.ToString()),

                           CustomData is not null
                               ? new JProperty("CustomData",   CustomData)
                               : null

                       );

            return CustomPullAuthenticationDataRequestSerializer is not null
                       ? CustomPullAuthenticationDataRequestSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullAuthenticationData1, PullAuthenticationData2)

        /// <summary>
        /// Compares two pull EVSE status requests for equality.
        /// </summary>
        /// <param name="PullAuthenticationData1">An pull EVSE status request.</param>
        /// <param name="PullAuthenticationData2">Another pull EVSE status request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullAuthenticationDataRequest PullAuthenticationData1,
                                           PullAuthenticationDataRequest PullAuthenticationData2)
        {

            if (ReferenceEquals(PullAuthenticationData1, PullAuthenticationData2))
                return true;

            if (PullAuthenticationData1 is null || PullAuthenticationData2 is null)
                return false;

            return PullAuthenticationData1.Equals(PullAuthenticationData2);

        }

        #endregion

        #region Operator != (PullAuthenticationData1, PullAuthenticationData2)

        /// <summary>
        /// Compares two pull EVSE status requests for inequality.
        /// </summary>
        /// <param name="PullAuthenticationData1">An pull EVSE status request.</param>
        /// <param name="PullAuthenticationData2">Another pull EVSE status request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullAuthenticationDataRequest PullAuthenticationData1,
                                           PullAuthenticationDataRequest PullAuthenticationData2)

            => !(PullAuthenticationData1 == PullAuthenticationData2);

        #endregion

        #endregion

        #region IEquatable<PullAuthenticationDataRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is PullAuthenticationDataRequest pullAuthenticationDataRequest &&
                   Equals(pullAuthenticationDataRequest);

        #endregion

        #region Equals(PullAuthenticationDataRequest)

        /// <summary>
        /// Compares two pull EVSE status requests for equality.
        /// </summary>
        /// <param name="PullAuthenticationDataRequest">A pull EVSE status request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullAuthenticationDataRequest? PullAuthenticationDataRequest)

            => PullAuthenticationDataRequest is not null &&

               OperatorId.Equals(PullAuthenticationDataRequest.OperatorId);

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
                return OperatorId.GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => OperatorId.ToString();

        #endregion

    }

}
