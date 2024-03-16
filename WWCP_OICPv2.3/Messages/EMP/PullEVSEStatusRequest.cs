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
    /// The PullEVSEStatus request.
    /// </summary>
    public class PullEVSEStatusRequest : ARequest<PullEVSEStatusRequest>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the e-mobility provider.
        /// </summary>
        [Mandatory]
        public Provider_Id       ProviderId          { get; }

        /// <summary>
        /// The optional geo coordinate of the search center.
        /// </summary>
        [Optional]
        public GeoCoordinates?   SearchCenter        { get; }

        /// <summary>
        /// The optional search distance relative to the search center.
        /// </summary>
        [Optional]
        public Single?           DistanceKM          { get; }

        /// <summary>
        /// The optional EVSE status as filter criteria.
        /// </summary>
        [Optional]
        public EVSEStatusTypes?  EVSEStatusFilter    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullEVSEStatus request.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="EVSEStatusFilter">An optional EVSE status as filter criteria.</param>
        /// <param name="ProcessId">The optional unique OICP process identification.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public PullEVSEStatusRequest(Provider_Id        ProviderId,
                                     GeoCoordinates?    SearchCenter        = null,
                                     Single?            DistanceKM          = null,
                                     EVSEStatusTypes?   EVSEStatusFilter    = null,
                                     Process_Id?        ProcessId           = null,
                                     JObject?           CustomData          = null,

                                     DateTime?          Timestamp           = null,
                                     CancellationToken  CancellationToken   = default,
                                     EventTracking_Id?  EventTrackingId     = null,
                                     TimeSpan?          RequestTimeout      = null)

            : base(ProcessId,
                   CustomData,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.ProviderId        = ProviderId;
            this.SearchCenter      = SearchCenter;
            this.DistanceKM        = DistanceKM;
            this.EVSEStatusFilter  = EVSEStatusFilter;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#eRoamingPullEvseStatus

        // {
        //   "ProviderID":        "string",
        //   "SearchCenter": {
        //     "GeoCoordinates": {
        //       "Google": {
        //         "Coordinates": "string"
        //       },
        //       "DecimalDegree": {
        //         "Latitude":    "string",
        //         "Longitude":   "string"
        //       },
        //       "DegreeMinuteSeconds": {
        //         "Latitude":    "string",
        //         "Longitude":   "string"
        //       }
        //     },
        //     "Radius":          0
        //   },
        //   "EvseStatus":        "Available"
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomPullEVSEStatusRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullEVSEStatus request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomPullEVSEStatusRequestParser">A delegate to parse custom PullEVSEStatus JSON objects.</param>
        public static PullEVSEStatusRequest Parse(JObject                                              JSON,
                                                  Process_Id?                                          ProcessId                           = null,

                                                  DateTime?                                            Timestamp                           = null,
                                                  CancellationToken                                    CancellationToken                   = default,
                                                  EventTracking_Id?                                    EventTrackingId                     = null,
                                                  TimeSpan?                                            RequestTimeout                      = null,

                                                  CustomJObjectParserDelegate<PullEVSEStatusRequest>?  CustomPullEVSEStatusRequestParser   = null)
        {

            if (TryParse(JSON,
                         out var pullEVSEStatusResponse,
                         out var errorResponse,
                         ProcessId,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout,
                         CustomPullEVSEStatusRequestParser))
            {
                return pullEVSEStatusResponse;
            }

            throw new ArgumentException("The given JSON representation of a PullEVSEStatus request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out PullEVSEStatusRequest, out ErrorResponse, ..., CustomPullEVSEStatusRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PullEVSEStatus request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="PullEVSEStatusRequest">The parsed PullEVSEStatus request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPullEVSEStatusRequestParser">A delegate to parse custom PullEVSEStatus request JSON objects.</param>
        public static Boolean TryParse(JObject                                              JSON,
                                       [NotNullWhen(true)]  out PullEVSEStatusRequest?      PullEVSEStatusRequest,
                                       [NotNullWhen(false)] out String?                     ErrorResponse,
                                       Process_Id?                                          ProcessId                           = null,

                                       DateTime?                                            Timestamp                           = null,
                                       CancellationToken                                    CancellationToken                   = default,
                                       EventTracking_Id?                                    EventTrackingId                     = null,
                                       TimeSpan?                                            RequestTimeout                      = null,

                                       CustomJObjectParserDelegate<PullEVSEStatusRequest>?  CustomPullEVSEStatusRequestParser   = null)
        {

            try
            {

                PullEVSEStatusRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse ProviderId            [mandatory]

                if (!JSON.ParseMandatory("ProviderID",
                                         "provider identification",
                                         Provider_Id.TryParse,
                                         out Provider_Id ProviderId,
                                         out             ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse SearchCenter          [optional]

                GeoCoordinates? SearchCenter   = default;
                Single?         DistanceKM     = default;

                if (JSON.ParseOptional("SearchCenter",
                                       "SearchCenter",
                                       out JObject searchCenter,
                                       out ErrorResponse))
                {

                    if (searchCenter.ParseOptionalJSON("GeoCoordinates",
                                                       "search center geo coordinates",
                                                       GeoCoordinates.TryParse,
                                                       out SearchCenter,
                                                       out ErrorResponse))
                    {
                        if (ErrorResponse is not null)
                            return false;
                    }

                #endregion

                #region Parse DistanceKM            [optional]

                    if (searchCenter.ParseOptional("Radius",
                                                   "search center radius",
                                                   out DistanceKM,
                                                   out ErrorResponse))
                    {
                        if (ErrorResponse is not null)
                            return false;
                    }

                }

                #endregion

                #region Parse EVSEStatusFilter      [optional]

                if (JSON.ParseOptional("EvseStatus",
                                       "EVSE status filter",
                                       EVSEStatusTypesExtensions.TryParse,
                                       out EVSEStatusTypes EVSEStatusFilter,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CustomData            [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                PullEVSEStatusRequest = new PullEVSEStatusRequest(ProviderId,
                                                                  SearchCenter,
                                                                  DistanceKM,
                                                                  EVSEStatusFilter,
                                                                  ProcessId,

                                                                  customData,

                                                                  Timestamp,
                                                                  CancellationToken,
                                                                  EventTrackingId,
                                                                  RequestTimeout);

                if (CustomPullEVSEStatusRequestParser is not null)
                    PullEVSEStatusRequest = CustomPullEVSEStatusRequestParser(JSON,
                                                                              PullEVSEStatusRequest);

                return true;

            }
            catch (Exception e)
            {
                PullEVSEStatusRequest  = default;
                ErrorResponse          = "The given JSON representation of a PullEVSEStatus request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPullEVSEStatusRequestSerializer = null, CustomGeoCoordinatesSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEStatusRequestSerializer">A delegate to customize the serialization of PullEVSEStatusRequest responses.</param>
        /// <param name="CustomGeoCoordinatesSerializer">A delegate to serialize custom geo coordinates JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullEVSEStatusRequest>?  CustomPullEVSEStatusRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<GeoCoordinates>?         CustomGeoCoordinatesSerializer          = null)
        {

            var json = JSONObject.Create(

                           new JProperty("ProviderID",                    ProviderId.            ToString()),

                           SearchCenter.HasValue && DistanceKM.HasValue
                               ? new JProperty("SearchCenter",            new JObject(
                                                                              new JProperty("GeoCoordinates",  SearchCenter.Value.ToJSON(CustomGeoCoordinatesSerializer)),
                                                                              new JProperty("Radius",          DistanceKM.Value)
                                                                          ))
                               : null,

                           EVSEStatusFilter.HasValue
                               ? new JProperty("EvseStatus",              EVSEStatusFilter.Value.AsString())
                               : null,

                           CustomData is not null
                               ? new JProperty("CustomData",              CustomData)
                               : null

                       );

            return CustomPullEVSEStatusRequestSerializer is not null
                       ? CustomPullEVSEStatusRequestSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullEVSEStatus1, PullEVSEStatus2)

        /// <summary>
        /// Compares two pull EVSE status requests for equality.
        /// </summary>
        /// <param name="PullEVSEStatus1">An pull EVSE status request.</param>
        /// <param name="PullEVSEStatus2">Another pull EVSE status request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullEVSEStatusRequest PullEVSEStatus1,
                                           PullEVSEStatusRequest PullEVSEStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PullEVSEStatus1, PullEVSEStatus2))
                return true;

            // If one is null, but not both, return false.
            if (PullEVSEStatus1 is null || PullEVSEStatus2 is null)
                return false;

            return PullEVSEStatus1.Equals(PullEVSEStatus2);

        }

        #endregion

        #region Operator != (PullEVSEStatus1, PullEVSEStatus2)

        /// <summary>
        /// Compares two pull EVSE status requests for inequality.
        /// </summary>
        /// <param name="PullEVSEStatus1">An pull EVSE status request.</param>
        /// <param name="PullEVSEStatus2">Another pull EVSE status request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullEVSEStatusRequest PullEVSEStatus1,
                                           PullEVSEStatusRequest PullEVSEStatus2)

            => !(PullEVSEStatus1 == PullEVSEStatus2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEStatusRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is PullEVSEStatusRequest pullEVSEStatusRequest &&
                   Equals(pullEVSEStatusRequest);

        #endregion

        #region Equals(PullEVSEStatusRequest)

        /// <summary>
        /// Compares two pull EVSE status requests for equality.
        /// </summary>
        /// <param name="PullEVSEStatusRequest">A pull EVSE status request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEStatusRequest? PullEVSEStatusRequest)

            => PullEVSEStatusRequest is not null &&

                 ProviderId.      Equals(PullEVSEStatusRequest.ProviderId) &&

              ((!SearchCenter.    HasValue && !PullEVSEStatusRequest.SearchCenter.    HasValue) ||
                (SearchCenter.    HasValue &&  PullEVSEStatusRequest.SearchCenter.    HasValue && SearchCenter.    Value.Equals(PullEVSEStatusRequest.SearchCenter.    Value))) &&

              ((!DistanceKM.      HasValue && !PullEVSEStatusRequest.DistanceKM.      HasValue) ||
                (DistanceKM.      HasValue &&  PullEVSEStatusRequest.DistanceKM.      HasValue && DistanceKM.      Value.Equals(PullEVSEStatusRequest.DistanceKM.      Value))) &&

              ((!EVSEStatusFilter.HasValue && !PullEVSEStatusRequest.EVSEStatusFilter.HasValue) ||
                (EVSEStatusFilter.HasValue &&  PullEVSEStatusRequest.EVSEStatusFilter.HasValue && EVSEStatusFilter.Value.Equals(PullEVSEStatusRequest.EVSEStatusFilter.Value)));

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

                return  ProviderId.       GetHashCode()       * 7 ^
                       (SearchCenter?.    GetHashCode() ?? 0) * 5 ^
                       (DistanceKM?.      GetHashCode() ?? 0) * 3 ^
                       (EVSEStatusFilter?.GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => new String[] {
                   ProviderId.ToString(),
                   SearchCenter.HasValue && DistanceKM.HasValue
                       ? SearchCenter.ToString() + " / " + DistanceKM + " km"
                       : "",
                   EVSEStatusFilter.HasValue
                       ? EVSEStatusFilter.Value.ToString()
                       : ""
               }.Where(text => text.IsNotNullOrEmpty()).
                 AggregateWith(", ");

        #endregion

    }

}
