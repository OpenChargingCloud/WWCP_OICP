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
                                         Process_Id?           ProcessId           = null,
                                         JObject?              CustomData          = null,

                                         DateTime?             Timestamp           = null,
                                         EventTracking_Id?     EventTrackingId     = null,
                                         TimeSpan?             RequestTimeout      = null,
                                         CancellationToken     CancellationToken   = default)

            : base(ProcessId,
                   CustomData,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.ProviderId  = ProviderId;
            this.EVSEIds     = EVSEIds ?? [];

            unchecked
            {

                hashCode = this.ProviderId.GetHashCode() * 3 ^
                           this.EVSEIds.   CalcHashCode();

            }

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#eRoamingPullEVSEStatusByIDmessage

        // {
        //   "ProviderID": "string",
        //   "EvseID": [
        //     "string"
        //   ]
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomPullEVSEStatusByIdRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullEVSEStatusById request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomPullEVSEStatusByIdRequestParser">A delegate to parse custom PullEVSEStatusById JSON objects.</param>
        public static PullEVSEStatusByIdRequest Parse(JObject                                                  JSON,
                                                      Process_Id?                                              ProcessId                               = null,

                                                      DateTime?                                                Timestamp                               = null,
                                                      CancellationToken                                        CancellationToken                       = default,
                                                      EventTracking_Id?                                        EventTrackingId                         = null,
                                                      TimeSpan?                                                RequestTimeout                          = null,

                                                      CustomJObjectParserDelegate<PullEVSEStatusByIdRequest>?  CustomPullEVSEStatusByIdRequestParser   = null)
        {

            if (TryParse(JSON,
                         out var pullEVSEStatusResponse,
                         out var errorResponse,
                         ProcessId,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout,
                         CustomPullEVSEStatusByIdRequestParser))
            {
                return pullEVSEStatusResponse;
            }

            throw new ArgumentException("The given JSON representation of a PullEVSEStatusById request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out PullEVSEStatusByIdRequest, out ErrorResponse, ..., CustomPullEVSEStatusByIdRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PullEVSEStatusById request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="PullEVSEStatusByIdRequest">The parsed PullEVSEStatusById request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPullEVSEStatusByIdRequestParser">A delegate to parse custom PullEVSEStatusById request JSON objects.</param>
        public static Boolean TryParse(JObject                                                  JSON,
                                       [NotNullWhen(true)]  out PullEVSEStatusByIdRequest?      PullEVSEStatusByIdRequest,
                                       [NotNullWhen(false)] out String?                         ErrorResponse,
                                       Process_Id?                                              ProcessId                               = null,

                                       DateTime?                                                Timestamp                               = null,
                                       CancellationToken                                        CancellationToken                       = default,
                                       EventTracking_Id?                                        EventTrackingId                         = null,
                                       TimeSpan?                                                RequestTimeout                          = null,

                                       CustomJObjectParserDelegate<PullEVSEStatusByIdRequest>?  CustomPullEVSEStatusByIdRequestParser   = null)
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
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CustomData     [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                PullEVSEStatusByIdRequest = new PullEVSEStatusByIdRequest(

                                                ProviderId,
                                                EVSEIds?.ToArray() ?? [],
                                                ProcessId,
                                                customData,

                                                Timestamp,
                                                EventTrackingId,
                                                RequestTimeout,
                                                CancellationToken

                                            );

                if (CustomPullEVSEStatusByIdRequestParser is not null)
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

        #region ToJSON(CustomPullEVSEStatusByIdRequestSerializer = null)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEStatusByIdRequestSerializer">A delegate to customize the serialization of PullEVSEStatusByIdRequest responses.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullEVSEStatusByIdRequest>?  CustomPullEVSEStatusByIdRequestSerializer   = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("ProviderID",   ProviderId.ToString()),

                                 new JProperty("EvseID",       new JArray(EVSEIds.Select(evseId => evseId.ToString()))),

                           CustomData is not null
                               ? new JProperty("CustomData",   CustomData)
                               : null

                       );

            return CustomPullEVSEStatusByIdRequestSerializer is not null
                       ? CustomPullEVSEStatusByIdRequestSerializer(this, json)
                       : json;

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

            if (ReferenceEquals(PullEVSEStatusById1, PullEVSEStatusById2))
                return true;

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
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is PullEVSEStatusByIdRequest pullEVSEStatusByIdRequest &&
                   Equals(pullEVSEStatusByIdRequest);

        #endregion

        #region Equals(PullEVSEStatusByIdRequest)

        /// <summary>
        /// Compares two pull EVSE status by id requests for equality.
        /// </summary>
        /// <param name="PullEVSEStatusByIdRequest">An pull EVSE status by id request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEStatusByIdRequest? PullEVSEStatusByIdRequest)

            => PullEVSEStatusByIdRequest is not null &&

               ProviderId.Equals(PullEVSEStatusByIdRequest.ProviderId) &&

               EVSEIds.Count().Equals(PullEVSEStatusByIdRequest.EVSEIds.Count()) &&
               EVSEIds.All(operatorId => PullEVSEStatusByIdRequest.EVSEIds.Contains(operatorId));

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

            => $"{ProviderId}: {EVSEIds.Count()} EVSE identifications";

        #endregion

    }

}
