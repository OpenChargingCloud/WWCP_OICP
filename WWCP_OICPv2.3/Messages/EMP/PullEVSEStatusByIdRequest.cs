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
    /// The PullEVSEStatusById request.
    /// </summary>
    public class PullEVSEStatusByIdRequest : ARequest<PullEVSEStatusByIdRequest>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the e-mobility provider.
        /// </summary>
        public Provider_Id           ProviderId     { get; }

        /// <summary>
        /// The enumeration of EVSE identifications.
        /// </summary>
        public IEnumerable<EVSE_Id>  EVSEIds        { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullEVSEStatusById request.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="EVSEIds">An enumeration of up to 100 EVSE identifications.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public PullEVSEStatusByIdRequest(Provider_Id           ProviderId,
                                         IEnumerable<EVSE_Id>  EVSEIds,
                                         JObject               CustomData          = null,

                                         DateTime?             Timestamp           = null,
                                         CancellationToken?    CancellationToken   = null,
                                         EventTracking_Id      EventTrackingId     = null,
                                         TimeSpan?             RequestTimeout      = null)

            : base(CustomData,
                   Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.ProviderId  = ProviderId;
            this.EVSEIds     = EVSEIds ?? new EVSE_Id[0];

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#512-eroamingpullevsestatusbyid-message

        // {
        //   "ProviderID": "string",
        //   "EvseID": [
        //     "string"
        //   ]
        // }

        #endregion

        #region (static) Parse   (JSON, CustomPullEVSEStatusByIdRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullEVSEStatusById request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomPullEVSEStatusByIdRequestParser">A delegate to parse custom PullEVSEStatusById JSON objects.</param>
        public static PullEVSEStatusByIdRequest Parse(JObject                                                 JSON,
                                                      CustomJObjectParserDelegate<PullEVSEStatusByIdRequest>  CustomPullEVSEStatusByIdRequestParser   = null)
        {

            if (TryParse(JSON,
                         out PullEVSEStatusByIdRequest  pullEVSEStatusResponse,
                         out String                     ErrorResponse,
                         CustomPullEVSEStatusByIdRequestParser))
            {
                return pullEVSEStatusResponse;
            }

            throw new ArgumentException("The given JSON representation of a PullEVSEStatusById request is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomPullEVSEStatusByIdRequestParser = null)

        /// <summary>
        /// Parse the given text representation of a PullEVSEStatusById request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomPullEVSEStatusByIdRequestParser">A delegate to parse custom PullEVSEStatusById request JSON objects.</param>
        public static PullEVSEStatusByIdRequest Parse(String                                                  Text,
                                                      CustomJObjectParserDelegate<PullEVSEStatusByIdRequest>  CustomPullEVSEStatusByIdRequestParser   = null)
        {

            if (TryParse(Text,
                         out PullEVSEStatusByIdRequest  pullEVSEStatusResponse,
                         out String                     ErrorResponse,
                         CustomPullEVSEStatusByIdRequestParser))
            {
                return pullEVSEStatusResponse;
            }

            throw new ArgumentException("The given text representation of a PullEVSEStatusById request is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out PullEVSEStatusByIdRequest, out ErrorResponse, CustomPullEVSEStatusByIdRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PullEVSEStatusById request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="PullEVSEStatusByIdRequest">The parsed PullEVSEStatusById request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPullEVSEStatusByIdRequestParser">A delegate to parse custom PullEVSEStatusById request JSON objects.</param>
        public static Boolean TryParse(JObject                                                 JSON,
                                       out PullEVSEStatusByIdRequest                           PullEVSEStatusByIdRequest,
                                       out String                                              ErrorResponse,
                                       CustomJObjectParserDelegate<PullEVSEStatusByIdRequest>  CustomPullEVSEStatusByIdRequestParser   = null)
        {

            try
            {

                PullEVSEStatusByIdRequest = default;

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

                #region Parse EVSEIds        [optional]

                if (JSON.ParseOptionalHashSet("EvseID",
                                              "EVSE identifications",
                                              EVSE_Id.TryParse,
                                              out HashSet<EVSE_Id> EVSEIds,
                                              out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse CustomData     [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                PullEVSEStatusByIdRequest = new PullEVSEStatusByIdRequest(ProviderId,
                                                                          EVSEIds?.ToArray() ?? new EVSE_Id[0],
                                                                          CustomData);

                if (CustomPullEVSEStatusByIdRequestParser != null)
                    PullEVSEStatusByIdRequest = CustomPullEVSEStatusByIdRequestParser(JSON,
                                                                                      PullEVSEStatusByIdRequest);

                return true;

            }
            catch (Exception e)
            {
                PullEVSEStatusByIdRequest  = default;
                ErrorResponse              = "The given JSON representation of a PullEVSEStatusById request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out PullEVSEStatusByIdRequest, out ErrorResponse, CustomPullEVSEStatusByIdRequestParser = null)

        /// <summary>
        /// Try to parse the given text representation of a PullEVSEStatusById request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="PullEVSEStatusByIdRequest">The parsed PullEVSEStatusById request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPullEVSEStatusByIdRequestParser">A delegate to parse custom PullEVSEStatusById request JSON objects.</param>
        public static Boolean TryParse(String                                                  Text,
                                       out PullEVSEStatusByIdRequest                           PullEVSEStatusByIdRequest,
                                       out String                                              ErrorResponse,
                                       CustomJObjectParserDelegate<PullEVSEStatusByIdRequest>  CustomPullEVSEStatusByIdRequestParser   = null)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out PullEVSEStatusByIdRequest,
                                out ErrorResponse,
                                CustomPullEVSEStatusByIdRequestParser);

            }
            catch (Exception e)
            {
                PullEVSEStatusByIdRequest  = default;
                ErrorResponse              = "The given text representation of a PullEVSEStatusById request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPullEVSEStatusByIdRequestSerializer = null)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEStatusByIdRequestSerializer">A delegate to customize the serialization of PullEVSEStatusByIdRequest responses.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullEVSEStatusByIdRequest>  CustomPullEVSEStatusByIdRequestSerializer   = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("ProviderID",        ProviderId.ToString()),

                           new JProperty("EvseID",            new JArray(EVSEIds.Select(evseId => evseId.ToString()))),

                           CustomData != null
                               ? new JProperty("CustomData",  CustomData)
                               : null

                       );

            return CustomPullEVSEStatusByIdRequestSerializer != null
                       ? CustomPullEVSEStatusByIdRequestSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullEVSEStatusById1, PullEVSEStatusById2)

        /// <summary>
        /// Compares two pull EVSE status by id requests for equality.
        /// </summary>
        /// <param name="PullEVSEStatusById1">An pull EVSE status by id request.</param>
        /// <param name="PullEVSEStatusById2">Another pull EVSE status by id request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullEVSEStatusByIdRequest PullEVSEStatusById1,
                                           PullEVSEStatusByIdRequest PullEVSEStatusById2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PullEVSEStatusById1, PullEVSEStatusById2))
                return true;

            // If one is null, but not both, return false.
            if ((PullEVSEStatusById1 is null) || (PullEVSEStatusById2 is null))
                return false;

            return PullEVSEStatusById1.Equals(PullEVSEStatusById2);

        }

        #endregion

        #region Operator != (PullEVSEStatusById1, PullEVSEStatusById2)

        /// <summary>
        /// Compares two pull EVSE status by id requests for inequality.
        /// </summary>
        /// <param name="PullEVSEStatusById1">An pull EVSE status by id request.</param>
        /// <param name="PullEVSEStatusById2">Another pull EVSE status by id request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullEVSEStatusByIdRequest PullEVSEStatusById1,
                                           PullEVSEStatusByIdRequest PullEVSEStatusById2)

            => !(PullEVSEStatusById1 == PullEVSEStatusById2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEStatusByIdRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is PullEVSEStatusByIdRequest pullEVSEStatusByIdRequest &&
                   Equals(pullEVSEStatusByIdRequest);

        #endregion

        #region Equals(PullEVSEStatusByIdRequest)

        /// <summary>
        /// Compares two pull EVSE status by id requests for equality.
        /// </summary>
        /// <param name="PullEVSEStatusByIdRequest">An pull EVSE status by id request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEStatusByIdRequest PullEVSEStatusByIdRequest)

            => !(PullEVSEStatusByIdRequest is null) &&

                 ProviderId.Equals(PullEVSEStatusByIdRequest.ProviderId) &&

                 EVSEIds.Count().Equals(PullEVSEStatusByIdRequest.EVSEIds.Count()) &&
                 EVSEIds.All(operatorId => PullEVSEStatusByIdRequest.EVSEIds.Contains(operatorId));

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

                return ProviderId.GetHashCode() * 3 ^
                       EVSEIds.Aggregate(0, (hashCode, evseId) => hashCode ^ evseId.GetHashCode());

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(ProviderId, ", ",
                             EVSEIds.Count(), " EVSE identifications");

        #endregion

    }

}
