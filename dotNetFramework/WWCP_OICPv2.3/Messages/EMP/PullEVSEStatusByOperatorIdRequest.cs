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

using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The PullEVSEStatusByOperatorId request.
    /// </summary>
    public class PullEVSEStatusByOperatorIdRequest : ARequest<PullEVSEStatusByOperatorIdRequest>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the e-mobility provider.
        /// </summary>
        public Provider_Id               ProviderId     { get; }

        /// <summary>
        /// The enumeration of operator identifications.
        /// </summary>
        public IEnumerable<Operator_Id>  OperatorIds    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullEVSEStatusByOperatorId request.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="OperatorIds">An enumeration of operator identifications.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public PullEVSEStatusByOperatorIdRequest(Provider_Id               ProviderId,
                                                 IEnumerable<Operator_Id>  OperatorIds,
                                                 JObject                   CustomData          = null,

                                                 DateTime?                 Timestamp           = null,
                                                 CancellationToken?        CancellationToken   = null,
                                                 EventTracking_Id          EventTrackingId     = null,
                                                 TimeSpan?                 RequestTimeout      = null)

            : base(CustomData,
                   Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.ProviderId   = ProviderId;
            this.OperatorIds  = OperatorIds ?? new Operator_Id[0];

        }

        #endregion


        #region Documentation

        // {
        //   "ProviderID": "string",
        //   "OperatorID": [
        //     "string"
        //   ]
        // }

        #endregion

        #region (static) Parse   (JSON, CustomPullEVSEStatusByOperatorIdRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullEVSEStatusByOperatorId request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomPullEVSEStatusByOperatorIdRequestParser">A delegate to parse custom PullEVSEStatusByOperatorId JSON objects.</param>
        public static PullEVSEStatusByOperatorIdRequest Parse(JObject                                                         JSON,
                                                              CustomJObjectParserDelegate<PullEVSEStatusByOperatorIdRequest>  CustomPullEVSEStatusByOperatorIdRequestParser   = null)
        {

            if (TryParse(JSON,
                         out PullEVSEStatusByOperatorIdRequest  pullEVSEStatusResponse,
                         out String                             ErrorResponse,
                         CustomPullEVSEStatusByOperatorIdRequestParser))
            {
                return pullEVSEStatusResponse;
            }

            throw new ArgumentException("The given JSON representation of a PullEVSEStatusByOperatorId request is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomPullEVSEStatusByOperatorIdRequestParser = null)

        /// <summary>
        /// Parse the given text representation of a PullEVSEStatusByOperatorId request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomPullEVSEStatusByOperatorIdRequestParser">A delegate to parse custom PullEVSEStatusByOperatorId request JSON objects.</param>
        public static PullEVSEStatusByOperatorIdRequest Parse(String                                                          Text,
                                                              CustomJObjectParserDelegate<PullEVSEStatusByOperatorIdRequest>  CustomPullEVSEStatusByOperatorIdRequestParser   = null)
        {

            if (TryParse(Text,
                         out PullEVSEStatusByOperatorIdRequest  pullEVSEStatusResponse,
                         out String                             ErrorResponse,
                         CustomPullEVSEStatusByOperatorIdRequestParser))
            {
                return pullEVSEStatusResponse;
            }

            throw new ArgumentException("The given text representation of a PullEVSEStatusByOperatorId request is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out PullEVSEStatusByOperatorIdRequest, out ErrorResponse, CustomPullEVSEStatusByOperatorIdRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PullEVSEStatusByOperatorId request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="PullEVSEStatusByOperatorIdRequest">The parsed PullEVSEStatusByOperatorId request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPullEVSEStatusByOperatorIdRequestParser">A delegate to parse custom PullEVSEStatusByOperatorId request JSON objects.</param>
        public static Boolean TryParse(JObject                                                         JSON,
                                       out PullEVSEStatusByOperatorIdRequest                           PullEVSEStatusByOperatorIdRequest,
                                       out String                                                      ErrorResponse,
                                       CustomJObjectParserDelegate<PullEVSEStatusByOperatorIdRequest>  CustomPullEVSEStatusByOperatorIdRequestParser   = null)
        {

            try
            {

                PullEVSEStatusByOperatorIdRequest = default;

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
                                         out             ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse OperatorIds    [optional]

                if (JSON.ParseOptionalHashSet("OperatorID",
                                              "EVSE operator identifications",
                                              Operator_Id.TryParse,
                                              out HashSet<Operator_Id> OperatorIds,
                                              out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse CustomData     [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                PullEVSEStatusByOperatorIdRequest = new PullEVSEStatusByOperatorIdRequest(ProviderId,
                                                                                          OperatorIds?.ToArray() ?? new Operator_Id[0],
                                                                                          CustomData);

                if (CustomPullEVSEStatusByOperatorIdRequestParser != null)
                    PullEVSEStatusByOperatorIdRequest = CustomPullEVSEStatusByOperatorIdRequestParser(JSON,
                                                                                                      PullEVSEStatusByOperatorIdRequest);

                return true;

            }
            catch (Exception e)
            {
                PullEVSEStatusByOperatorIdRequest  = default;
                ErrorResponse                      = "The given JSON representation of a PullEVSEStatusByOperatorId request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out PullEVSEStatusByOperatorIdRequest, out ErrorResponse, CustomPullEVSEStatusByOperatorIdRequestParser = null)

        /// <summary>
        /// Try to parse the given text representation of a PullEVSEStatusByOperatorId request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="PullEVSEStatusByOperatorIdRequest">The parsed PullEVSEStatusByOperatorId request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPullEVSEStatusByOperatorIdRequestParser">A delegate to parse custom PullEVSEStatusByOperatorId request JSON objects.</param>
        public static Boolean TryParse(String                                                          Text,
                                       out PullEVSEStatusByOperatorIdRequest                           PullEVSEStatusByOperatorIdRequest,
                                       out String                                                      ErrorResponse,
                                       CustomJObjectParserDelegate<PullEVSEStatusByOperatorIdRequest>  CustomPullEVSEStatusByOperatorIdRequestParser   = null)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out PullEVSEStatusByOperatorIdRequest,
                                out ErrorResponse,
                                CustomPullEVSEStatusByOperatorIdRequestParser);

            }
            catch (Exception e)
            {
                PullEVSEStatusByOperatorIdRequest  = default;
                ErrorResponse                      = "The given text representation of a PullEVSEStatusByOperatorId request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPullEVSEStatusByOperatorIdRequestSerializer = null)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEStatusByOperatorIdRequestSerializer">A delegate to customize the serialization of PullEVSEStatusByOperatorIdRequest responses.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullEVSEStatusByOperatorIdRequest>  CustomPullEVSEStatusByOperatorIdRequestSerializer   = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("ProviderID",        ProviderId.ToString()),

                           new JProperty("OperatorID",        new JArray(OperatorIds.Select(operatorId => operatorId.ToString()))),

                           CustomData != null
                               ? new JProperty("CustomData",  CustomData)
                               : null

                       );

            return CustomPullEVSEStatusByOperatorIdRequestSerializer != null
                       ? CustomPullEVSEStatusByOperatorIdRequestSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullEVSEStatusByOperatorId1, PullEVSEStatusByOperatorId2)

        /// <summary>
        /// Compares two pull EVSE status by id requests for equality.
        /// </summary>
        /// <param name="PullEVSEStatusByOperatorId1">An pull EVSE status by id request.</param>
        /// <param name="PullEVSEStatusByOperatorId2">Another pull EVSE status by id request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullEVSEStatusByOperatorIdRequest PullEVSEStatusByOperatorId1,
                                           PullEVSEStatusByOperatorIdRequest PullEVSEStatusByOperatorId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PullEVSEStatusByOperatorId1, PullEVSEStatusByOperatorId2))
                return true;

            // If one is null, but not both, return false.
            if ((PullEVSEStatusByOperatorId1 is null) || (PullEVSEStatusByOperatorId2 is null))
                return false;

            return PullEVSEStatusByOperatorId1.Equals(PullEVSEStatusByOperatorId2);

        }

        #endregion

        #region Operator != (PullEVSEStatusByOperatorId1, PullEVSEStatusByOperatorId2)

        /// <summary>
        /// Compares two pull EVSE status by id requests for inequality.
        /// </summary>
        /// <param name="PullEVSEStatusByOperatorId1">An pull EVSE status by id request.</param>
        /// <param name="PullEVSEStatusByOperatorId2">Another pull EVSE status by id request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullEVSEStatusByOperatorIdRequest PullEVSEStatusByOperatorId1,
                                           PullEVSEStatusByOperatorIdRequest PullEVSEStatusByOperatorId2)

            => !(PullEVSEStatusByOperatorId1 == PullEVSEStatusByOperatorId2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEStatusByOperatorIdRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is PullEVSEStatusByOperatorIdRequest pullEVSEStatusByOperatorIdRequest &&
                   Equals(pullEVSEStatusByOperatorIdRequest);

        #endregion

        #region Equals(PullEVSEStatusByOperatorIdRequest)

        /// <summary>
        /// Compares two pull EVSE status by id requests for equality.
        /// </summary>
        /// <param name="PullEVSEStatusByOperatorIdRequest">An pull EVSE status by id request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEStatusByOperatorIdRequest PullEVSEStatusByOperatorIdRequest)

            => !(PullEVSEStatusByOperatorIdRequest is null) &&

                 ProviderId.         Equals(PullEVSEStatusByOperatorIdRequest.ProviderId) &&

                 OperatorIds.Count().Equals(PullEVSEStatusByOperatorIdRequest.OperatorIds.Count()) &&
                 OperatorIds.All(operatorId => PullEVSEStatusByOperatorIdRequest.OperatorIds.Contains(operatorId));

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

                return ProviderId. GetHashCode() * 3 ^
                       OperatorIds.Aggregate(0, (hashCode, operatorId) => hashCode ^ operatorId.GetHashCode());

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(ProviderId, ", ",
                             OperatorIds.Count(), " operator identifications");

        #endregion

    }

}
