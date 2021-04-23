/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The PullEVSEData response.
    /// </summary>
    public class PullEVSEDataResponse : AResponse<PullEVSEDataRequest,
                                                  PullEVSEDataResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE data records grouped by their operators.
        /// </summary>
        [Mandatory]
        public IEnumerable<OperatorEVSEData>  OperatorEVSEData    { get; }

        /// <summary>
        /// The optional status code of this response.
        /// </summary>
        [Optional]
        public StatusCode                     StatusCode          { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullEVSEData response.
        /// </summary>
        /// <param name="Request">A PullEVSEData request.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="OperatorEVSEData">An enumeration of EVSE data records grouped by their operators.</param>
        /// <param name="StatusCode">An optional status code of this response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public PullEVSEDataResponse(PullEVSEDataRequest            Request,
                                    DateTime                       ResponseTimestamp,
                                    EventTracking_Id               EventTrackingId,
                                    TimeSpan                       Runtime,
                                    IEnumerable<OperatorEVSEData>  OperatorEVSEData,
                                    StatusCode                     StatusCode     = null,
                                    Process_Id?                    ProcessId      = null,
                                    HTTPResponse                   HTTPResponse   = null,
                                    JObject                        CustomData     = null)

            : base(Request,
                   ResponseTimestamp,
                   EventTrackingId,
                   Runtime,
                   HTTPResponse,
                   ProcessId,
                   CustomData)

        {

            this.OperatorEVSEData  = OperatorEVSEData ?? throw new ArgumentNullException(nameof(OperatorEVSEData), "The given enumeration of EVSE data records must not be null!");
            this.StatusCode        = StatusCode;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#412-eroamingevsedata-message

        // {
        //   "EvseData": {
        //     "OperatorEvseData": [
        //       {
        //         "EvseDataRecord": [
        //           {
        //             "EvseID":    "string",
        //             [...]
        //           },
        //           [...]
        //         ],
        //         "OperatorID":    "string",
        //         "OperatorName":  "string"
        //       }
        //     ]
        //   },
        //   "StatusCode": {
        //     "AdditionalInfo":    "string",
        //     "Code":              "000",
        //     "Description":       "string"
        //   }
        // }

        #endregion

        #region (static) Parse   (JSON, CustomPullEVSEDataResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullEVSEData response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullEVSEDataResponseParser">A delegate to parse custom PullEVSEData JSON objects.</param>
        public static PullEVSEDataResponse Parse(PullEVSEDataRequest                                Request,
                                                 JObject                                            JSON,
                                                 DateTime                                           ResponseTimestamp,
                                                 EventTracking_Id                                   EventTrackingId,
                                                 TimeSpan                                           Runtime,
                                                 Process_Id?                                        ProcessId                          = null,
                                                 HTTPResponse                                       HTTPResponse                       = null,
                                                 CustomJObjectParserDelegate<PullEVSEDataResponse>  CustomPullEVSEDataResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         out PullEVSEDataResponse  pullEVSEDataResponse,
                         out String                ErrorResponse,
                         ProcessId,
                         HTTPResponse,
                         CustomPullEVSEDataResponseParser))
            {
                return pullEVSEDataResponse;
            }

            throw new ArgumentException("The given JSON representation of a PullEVSEData response is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomPullEVSEDataResponseParser = null)

        /// <summary>
        /// Parse the given text representation of a PullEVSEData response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="Text">The text to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullEVSEDataResponseParser">A delegate to parse custom PullEVSEData response JSON objects.</param>
        public static PullEVSEDataResponse Parse(PullEVSEDataRequest                                Request,
                                                 String                                             Text,
                                                 DateTime                                           ResponseTimestamp,
                                                 EventTracking_Id                                   EventTrackingId,
                                                 TimeSpan                                           Runtime,
                                                 Process_Id?                                        ProcessId                          = null,
                                                 HTTPResponse                                       HTTPResponse                       = null,
                                                 CustomJObjectParserDelegate<PullEVSEDataResponse>  CustomPullEVSEDataResponseParser   = null)
        {

            if (TryParse(Request,
                         Text,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         out PullEVSEDataResponse  pullEVSEDataResponse,
                         out String                ErrorResponse,
                         ProcessId,
                         HTTPResponse,
                         CustomPullEVSEDataResponseParser))
            {
                return pullEVSEDataResponse;
            }

            throw new ArgumentException("The given text representation of a PullEVSEData response is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out PullEVSEDataResponse, out ErrorResponse, CustomPullEVSEDataResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PullEVSEData response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="PullEVSEDataResponse">The parsed PullEVSEData response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullEVSEDataResponseParser">A delegate to parse custom PullEVSEData response JSON objects.</param>
        public static Boolean TryParse(PullEVSEDataRequest                                Request,
                                       JObject                                            JSON,
                                       DateTime                                           ResponseTimestamp,
                                       EventTracking_Id                                   EventTrackingId,
                                       TimeSpan                                           Runtime,
                                       out PullEVSEDataResponse                           PullEVSEDataResponse,
                                       out String                                         ErrorResponse,
                                       Process_Id?                                        ProcessId                          = null,
                                       HTTPResponse                                       HTTPResponse                       = null,
                                       CustomJObjectParserDelegate<PullEVSEDataResponse>  CustomPullEVSEDataResponseParser   = null)
        {

            try
            {

                PullEVSEDataResponse = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse OperatorEVSEStatus    [mandatory]

                if (!JSON.ParseMandatory("EvseData",
                                         "EVSE data",
                                         out JObject evseData,
                                         out         ErrorResponse))
                {
                    return false;
                }

                if (!evseData.ParseMandatoryJSON("OperatorEvseData",
                                                 "operator EVSE data",
                                                 OICPv2_3.OperatorEVSEData.TryParse,
                                                 out IEnumerable<OperatorEVSEData> OperatorEVSEData,
                                                 out ErrorResponse))
                {
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
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse CustomData            [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                PullEVSEDataResponse = new PullEVSEDataResponse(Request,
                                                                ResponseTimestamp,
                                                                EventTrackingId,
                                                                Runtime,
                                                                OperatorEVSEData,
                                                                StatusCode,
                                                                ProcessId,
                                                                HTTPResponse,
                                                                CustomData);

                if (CustomPullEVSEDataResponseParser != null)
                    PullEVSEDataResponse = CustomPullEVSEDataResponseParser(JSON,
                                                                            PullEVSEDataResponse);

                return true;

            }
            catch (Exception e)
            {
                PullEVSEDataResponse  = default;
                ErrorResponse         = "The given JSON representation of a PullEVSEData response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out PullEVSEDataResponse, out ErrorResponse, CustomPullEVSEDataResponseParser = null)

        /// <summary>
        /// Try to parse the given text representation of a PullEVSEData response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="Text">The text to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="PullEVSEDataResponse">The parsed PullEVSEData response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullEVSEDataResponseParser">A delegate to parse custom PullEVSEData response JSON objects.</param>
        public static Boolean TryParse(PullEVSEDataRequest                                Request,
                                       String                                             Text,
                                       DateTime                                           ResponseTimestamp,
                                       EventTracking_Id                                   EventTrackingId,
                                       TimeSpan                                           Runtime,
                                       out PullEVSEDataResponse                           PullEVSEDataResponse,
                                       out String                                         ErrorResponse,
                                       Process_Id?                                        ProcessId                          = null,
                                       HTTPResponse                                       HTTPResponse                       = null,
                                       CustomJObjectParserDelegate<PullEVSEDataResponse>  CustomPullEVSEDataResponseParser   = null)
        {

            try
            {

                return TryParse(Request,
                                JObject.Parse(Text),
                                ResponseTimestamp,
                                EventTrackingId,
                                Runtime,
                                out PullEVSEDataResponse,
                                out ErrorResponse,
                                ProcessId,
                                HTTPResponse,
                                CustomPullEVSEDataResponseParser);

            }
            catch (Exception e)
            {
                PullEVSEDataResponse  = default;
                ErrorResponse         = "The given text representation of a PullEVSEData response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPullEVSEDataResponseSerializer = null, CustomOperatorEVSEDataSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEDataResponseSerializer">A delegate to customize the serialization of PullEVSEDataResponse responses.</param>
        /// <param name="CustomOperatorEVSEDataSerializer">A delegate to serialize custom operator EVSE data JSON objects.</param>
        /// <param name="CustomEVSEDataRecordSerializer">A delegate to serialize custom EVSE data record JSON objects.</param>
        /// <param name="CustomAddressSerializer">A delegate to serialize custom address JSON objects.</param>
        /// <param name="CustomChargingFacilitySerializer">A delegate to serialize custom charging facility JSON objects.</param>
        /// <param name="CustomGeoCoordinatesSerializer">A delegate to serialize custom geo coordinates JSON objects.</param>
        /// <param name="CustomEnergySourceSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomEnvironmentalImpactSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomOpeningTimesSerializer">A delegate to serialize custom opening time JSON objects.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullEVSEDataResponse>  CustomPullEVSEDataResponseSerializer   = null,
                              CustomJObjectSerializerDelegate<OperatorEVSEData>      CustomOperatorEVSEDataSerializer       = null,
                              CustomJObjectSerializerDelegate<EVSEDataRecord>        CustomEVSEDataRecordSerializer         = null,
                              CustomJObjectSerializerDelegate<Address>               CustomAddressSerializer                = null,
                              CustomJObjectSerializerDelegate<ChargingFacility>      CustomChargingFacilitySerializer       = null,
                              CustomJObjectSerializerDelegate<GeoCoordinates>        CustomGeoCoordinatesSerializer         = null,
                              CustomJObjectSerializerDelegate<EnergySource>          CustomEnergySourceSerializer           = null,
                              CustomJObjectSerializerDelegate<EnvironmentalImpact>   CustomEnvironmentalImpactSerializer    = null,
                              CustomJObjectSerializerDelegate<OpeningTime>           CustomOpeningTimesSerializer           = null,
                              CustomJObjectSerializerDelegate<StatusCode>            CustomStatusCodeSerializer             = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("EvseData",
                               new JProperty("OperatorEvseData",  new JArray(OperatorEVSEData.Select(operatorEVSEData => operatorEVSEData.ToJSON(CustomOperatorEVSEDataSerializer,
                                                                                                                                                 CustomEVSEDataRecordSerializer,
                                                                                                                                                 CustomAddressSerializer,
                                                                                                                                                 CustomChargingFacilitySerializer,
                                                                                                                                                 CustomGeoCoordinatesSerializer,
                                                                                                                                                 CustomEnergySourceSerializer,
                                                                                                                                                 CustomEnvironmentalImpactSerializer,
                                                                                                                                                 CustomOpeningTimesSerializer))))
                           ),

                           StatusCode != null
                               ? new JProperty("StatusCode",  StatusCode.ToJSON(CustomStatusCodeSerializer))
                               : null,

                           CustomData != null
                               ? new JProperty("CustomData",  CustomData)
                               : null

                       );

            return CustomPullEVSEDataResponseSerializer != null
                       ? CustomPullEVSEDataResponseSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullEVSEDataResponse1, PullEVSEDataResponse2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="PullEVSEDataResponse1">A PullEVSEData response.</param>
        /// <param name="PullEVSEDataResponse2">Another PullEVSEData response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullEVSEDataResponse PullEVSEDataResponse1,
                                           PullEVSEDataResponse PullEVSEDataResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PullEVSEDataResponse1, PullEVSEDataResponse2))
                return true;

            // If one is null, but not both, return false.
            if (PullEVSEDataResponse1 is null || PullEVSEDataResponse2 is null)
                return false;

            return PullEVSEDataResponse1.Equals(PullEVSEDataResponse2);

        }

        #endregion

        #region Operator != (PullEVSEDataResponse1, PullEVSEDataResponse2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="PullEVSEDataResponse1">A PullEVSEData response.</param>
        /// <param name="PullEVSEDataResponse2">Another PullEVSEData response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullEVSEDataResponse PullEVSEDataResponse1,
                                           PullEVSEDataResponse PullEVSEDataResponse2)

            => !(PullEVSEDataResponse1 == PullEVSEDataResponse2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEDataResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is PullEVSEDataResponse pullEVSEDataResponse &&
                   Equals(pullEVSEDataResponse);

        #endregion

        #region Equals(PullEVSEDataResponse)

        /// <summary>
        /// Compares two PullEVSEData responses for equality.
        /// </summary>
        /// <param name="PullEVSEDataResponse">A PullEVSEData response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEDataResponse PullEVSEDataResponse)

            => !(PullEVSEDataResponse is null) &&

               (!OperatorEVSEData.Any() && !PullEVSEDataResponse.OperatorEVSEData.Any()) ||
                (OperatorEVSEData.Any() && PullEVSEDataResponse.OperatorEVSEData.Any() && OperatorEVSEData.Count().Equals(PullEVSEDataResponse.OperatorEVSEData.Count())) &&

               ((StatusCode == null && PullEVSEDataResponse.StatusCode == null) ||
                (StatusCode != null && PullEVSEDataResponse.StatusCode != null && StatusCode.Equals(PullEVSEDataResponse.StatusCode)));

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

                return OperatorEVSEData.Aggregate(0, (hashCode, operatorEVSEData) => hashCode ^ operatorEVSEData.GetHashCode()) ^
                       StatusCode?.GetHashCode() ?? 0;

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(OperatorEVSEData.Count() + " operator EVSE data record(s)",
                             StatusCode != null
                                 ? " -> " + StatusCode.Code
                                 : "");

        #endregion


        #region ToBuilder

        /// <summary>
        /// Return a response builder.
        /// </summary>
        public Builder ToBuilder

            => new Builder(Request,
                           ResponseTimestamp,
                           EventTrackingId,
                           Runtime,
                           OperatorEVSEData,
                           StatusCode,
                           ProcessId,
                           HTTPResponse,
                           CustomData);

        #endregion

        #region (class) Builder

        /// <summary>
        /// A PullEVSEData response builder.
        /// </summary>
        public new class Builder : AResponse<PullEVSEDataRequest,
                                             PullEVSEDataResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// An enumeration of EVSE data records grouped by their operators.
            /// </summary>
            public HashSet<OperatorEVSEData>  OperatorEVSEData    { get; }

            /// <summary>
            /// The optional status code for this request.
            /// </summary>
            public StatusCode.Builder         StatusCode          { get; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new PullEVSEData response builder.
            /// </summary>
            /// <param name="Request">A PullEVSEData request.</param>
            /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
            /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
            /// <param name="Runtime">The runtime of the request/response.</param>
            /// <param name="OperatorEVSEData">An enumeration of EVSE data records grouped by their operators.</param>
            /// <param name="StatusCode">An optional status code for this request.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(PullEVSEDataRequest            Request             = null,
                           DateTime?                      ResponseTimestamp   = null,
                           EventTracking_Id               EventTrackingId     = null,
                           TimeSpan?                      Runtime             = null,
                           IEnumerable<OperatorEVSEData>  OperatorEVSEData    = null,
                           StatusCode                     StatusCode          = null,
                           Process_Id?                    ProcessId           = null,
                           HTTPResponse                   HTTPResponse        = null,
                           JObject                        CustomData          = null)

                : base(Request,
                       ResponseTimestamp,
                       EventTrackingId,
                       Runtime,
                       HTTPResponse,
                       ProcessId,
                       CustomData)

            {

                this.OperatorEVSEData  = OperatorEVSEData != null ? new HashSet<OperatorEVSEData>(OperatorEVSEData) : new HashSet<OperatorEVSEData>();
                this.StatusCode        = StatusCode       != null ? StatusCode.ToBuilder()                          : new StatusCode.Builder();

            }

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the PullEVSEData response.
            /// </summary>
            /// <param name="Builder">A PullEVSEData builder.</param>
            public static implicit operator PullEVSEDataResponse(Builder Builder)

                => Builder?.ToImmutable();


            /// <summary>
            /// Return an immutable version of the PullEVSEData response.
            /// </summary>
            public override PullEVSEDataResponse ToImmutable()

                => new PullEVSEDataResponse(Request           ?? throw new ArgumentNullException(nameof(Request), "The given request must not be null!"),
                                            ResponseTimestamp ?? DateTime.UtcNow,
                                            EventTrackingId   ?? EventTracking_Id.New,
                                            Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                            OperatorEVSEData,
                                            StatusCode,
                                            ProcessId,
                                            HTTPResponse,
                                            CustomData);

            #endregion

        }

        #endregion

    }

}
