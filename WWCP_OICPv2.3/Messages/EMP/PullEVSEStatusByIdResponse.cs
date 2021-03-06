﻿/*
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
    /// The PullEVSEStatusById response.
    /// </summary>
    public class PullEVSEStatusByIdResponse : AResponse<PullEVSEStatusByIdRequest,
                                              PullEVSEStatusByIdResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE status records.
        /// </summary>
        [Mandatory]
        public IEnumerable<EVSEStatusRecord>  EVSEStatusRecords    { get; }

        /// <summary>
        /// The optional status code of this response.
        /// </summary>
        [Optional]
        public StatusCode                     StatusCode           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullEVSEStatusById response.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusById request.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
        /// <param name="StatusCode">An optional status code of this response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public PullEVSEStatusByIdResponse(PullEVSEStatusByIdRequest      Request,
                                          DateTime                       ResponseTimestamp,
                                          EventTracking_Id               EventTrackingId,
                                          TimeSpan                       Runtime,
                                          IEnumerable<EVSEStatusRecord>  EVSEStatusRecords,
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

            this.EVSEStatusRecords  = EVSEStatusRecords ?? throw new ArgumentNullException(nameof(EVSEStatusRecords), "The given enumeration of EVSE status records must not be null!");
            this.StatusCode         = StatusCode;

        }

        #endregion


        #region Documentation

        // {
        //   "EVSEStatusRecords": {
        //     "EvseStatusRecord": [
        //       {
        //         "EvseID": "string",
        //         "EvseStatus": "Available"
        //       }
        //     ]
        //   },
        //   "StatusCode": {
        //     "AdditionalInfo": "string",
        //     "Code": "000",
        //     "Description": "string"
        //   }
        // }

        #endregion

        #region (static) Parse   (JSON, CustomPullEVSEStatusByIdResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullEVSEStatusById response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullEVSEStatusByIdResponseParser">A delegate to parse custom PullEVSEStatusById JSON objects.</param>
        public static PullEVSEStatusByIdResponse Parse(PullEVSEStatusByIdRequest                                Request,
                                                       JObject                                                  JSON,
                                                       DateTime                                                 ResponseTimestamp,
                                                       EventTracking_Id                                         EventTrackingId,
                                                       TimeSpan                                                 Runtime,
                                                       Process_Id?                                              ProcessId                                = null,
                                                       HTTPResponse                                             HTTPResponse                             = null,
                                                       CustomJObjectParserDelegate<PullEVSEStatusByIdResponse>  CustomPullEVSEStatusByIdResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         out PullEVSEStatusByIdResponse  pullEVSEStatusByIdResponse,
                         out String                      ErrorResponse,
                         ProcessId,
                         HTTPResponse,
                         CustomPullEVSEStatusByIdResponseParser))
            {
                return pullEVSEStatusByIdResponse;
            }

            throw new ArgumentException("The given JSON representation of a PullEVSEStatusById response is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomPullEVSEStatusByIdResponseParser = null)

        /// <summary>
        /// Parse the given text representation of a PullEVSEStatusById response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="Text">The text to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullEVSEStatusByIdResponseParser">A delegate to parse custom PullEVSEStatusById response JSON objects.</param>
        public static PullEVSEStatusByIdResponse Parse(PullEVSEStatusByIdRequest                                Request,
                                                       String                                                   Text,
                                                       DateTime                                                 ResponseTimestamp,
                                                       EventTracking_Id                                         EventTrackingId,
                                                       TimeSpan                                                 Runtime,
                                                       Process_Id?                                              ProcessId                                = null,
                                                       HTTPResponse                                             HTTPResponse                             = null,
                                                       CustomJObjectParserDelegate<PullEVSEStatusByIdResponse>  CustomPullEVSEStatusByIdResponseParser   = null)
        {

            if (TryParse(Request,
                         Text,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         out PullEVSEStatusByIdResponse  pullEVSEStatusByIdResponse,
                         out String                      ErrorResponse,
                         ProcessId,
                         HTTPResponse,
                         CustomPullEVSEStatusByIdResponseParser))
            {
                return pullEVSEStatusByIdResponse;
            }

            throw new ArgumentException("The given text representation of a PullEVSEStatusById response is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out PullEVSEStatusByIdResponse, out ErrorResponse, CustomPullEVSEStatusByIdResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PullEVSEStatusById response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="PullEVSEStatusByIdResponse">The parsed PullEVSEStatusById response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullEVSEStatusByIdResponseParser">A delegate to parse custom PullEVSEStatusById response JSON objects.</param>
        public static Boolean TryParse(PullEVSEStatusByIdRequest                                Request,
                                       JObject                                                  JSON,
                                       DateTime                                                 ResponseTimestamp,
                                       EventTracking_Id                                         EventTrackingId,
                                       TimeSpan                                                 Runtime,
                                       out PullEVSEStatusByIdResponse                           PullEVSEStatusByIdResponse,
                                       out String                                               ErrorResponse,
                                       Process_Id?                                              ProcessId                                = null,
                                       HTTPResponse                                             HTTPResponse                             = null,
                                       CustomJObjectParserDelegate<PullEVSEStatusByIdResponse>  CustomPullEVSEStatusByIdResponseParser   = null)
        {

            try
            {

                PullEVSEStatusByIdResponse = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse EVSEStatusRecords     [mandatory]

                if (!JSON.ParseMandatory("EVSEStatusRecords",
                                         "EVSEStatusRecords",
                                         out JObject evseStatusRecords,
                                         out         ErrorResponse))
                {
                    return false;
                }

                if (!evseStatusRecords.ParseMandatoryJSON("EvseStatusRecord",
                                                          "EVSE status record",
                                                          EVSEStatusRecord.TryParse,
                                                          out IEnumerable<EVSEStatusRecord> EVSEStatusRecords,
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


                PullEVSEStatusByIdResponse = new PullEVSEStatusByIdResponse(Request,
                                                                            ResponseTimestamp,
                                                                            EventTrackingId,
                                                                            Runtime,
                                                                            EVSEStatusRecords,
                                                                            StatusCode,
                                                                            ProcessId,
                                                                            HTTPResponse,
                                                                            CustomData);

                if (CustomPullEVSEStatusByIdResponseParser != null)
                    PullEVSEStatusByIdResponse = CustomPullEVSEStatusByIdResponseParser(JSON,
                                                                                        PullEVSEStatusByIdResponse);

                return true;

            }
            catch (Exception e)
            {
                PullEVSEStatusByIdResponse  = default;
                ErrorResponse               = "The given JSON representation of a PullEVSEStatusById response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out PullEVSEStatusByIdResponse, out ErrorResponse, CustomPullEVSEStatusByIdResponseParser = null)

        /// <summary>
        /// Try to parse the given text representation of a PullEVSEStatusById response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="Text">The text to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="PullEVSEStatusByIdResponse">The parsed PullEVSEStatusById response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomPullEVSEStatusByIdResponseParser">A delegate to parse custom PullEVSEStatusById response JSON objects.</param>
        public static Boolean TryParse(PullEVSEStatusByIdRequest                                Request,
                                       String                                                   Text,
                                       DateTime                                                 ResponseTimestamp,
                                       EventTracking_Id                                         EventTrackingId,
                                       TimeSpan                                                 Runtime,
                                       out PullEVSEStatusByIdResponse                           PullEVSEStatusByIdResponse,
                                       out String                                               ErrorResponse,
                                       Process_Id?                                              ProcessId                                = null,
                                       HTTPResponse                                             HTTPResponse                             = null,
                                       CustomJObjectParserDelegate<PullEVSEStatusByIdResponse>  CustomPullEVSEStatusByIdResponseParser   = null)
        {

            try
            {

                return TryParse(Request,
                                JObject.Parse(Text),
                                ResponseTimestamp,
                                EventTrackingId,
                                Runtime,
                                out PullEVSEStatusByIdResponse,
                                out ErrorResponse,
                                ProcessId,
                                HTTPResponse,
                                CustomPullEVSEStatusByIdResponseParser);

            }
            catch (Exception e)
            {
                PullEVSEStatusByIdResponse  = default;
                ErrorResponse               = "The given text representation of a PullEVSEStatusById response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPullEVSEStatusByIdResponseSerializer = null, CustomEVSEStatusRecordSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEStatusByIdResponseSerializer">A delegate to customize the serialization of PullEVSEStatusByIdResponse responses.</param>
        /// <param name="CustomEVSEStatusRecordSerializer">A delegate to serialize custom EVSE status record JSON objects.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullEVSEStatusByIdResponse>  CustomPullEVSEStatusByIdResponseSerializer   = null,
                              CustomJObjectSerializerDelegate<EVSEStatusRecord>            CustomEVSEStatusRecordSerializer             = null,
                              CustomJObjectSerializerDelegate<StatusCode>                  CustomStatusCodeSerializer                   = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("EVSEStatusRecords",
                               new JProperty("EvseStatusRecord",  new JArray(EVSEStatusRecords.Select(evseStatusRecord => evseStatusRecord.ToJSON(CustomEVSEStatusRecordSerializer))))
                           ),

                           StatusCode != null
                               ? new JProperty("StatusCode",  StatusCode.ToJSON(CustomStatusCodeSerializer))
                               : null

                       );

            return CustomPullEVSEStatusByIdResponseSerializer != null
                       ? CustomPullEVSEStatusByIdResponseSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullEVSEStatusByIdResponse1, PullEVSEStatusByIdResponse2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PullEVSEStatusByIdResponse1">An EVSE status by id request.</param>
        /// <param name="PullEVSEStatusByIdResponse2">Another EVSE status by id request.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (PullEVSEStatusByIdResponse PullEVSEStatusByIdResponse1,
                                           PullEVSEStatusByIdResponse PullEVSEStatusByIdResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PullEVSEStatusByIdResponse1, PullEVSEStatusByIdResponse2))
                return true;

            // If one is null, but not both, return false.
            if (PullEVSEStatusByIdResponse1 is null || PullEVSEStatusByIdResponse2 is null)
                return false;

            return PullEVSEStatusByIdResponse1.Equals(PullEVSEStatusByIdResponse2);

        }

        #endregion

        #region Operator != (PullEVSEStatusByIdResponse1, PullEVSEStatusByIdResponse2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PullEVSEStatusByIdResponse1">An EVSE status by id request.</param>
        /// <param name="PullEVSEStatusByIdResponse2">Another EVSE status by id request.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (PullEVSEStatusByIdResponse PullEVSEStatusByIdResponse1,
                                           PullEVSEStatusByIdResponse PullEVSEStatusByIdResponse2)

            => !(PullEVSEStatusByIdResponse1 == PullEVSEStatusByIdResponse2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEStatusByIdResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is PullEVSEStatusByIdResponse pullEVSEStatusByIdResponse &&
                   Equals(pullEVSEStatusByIdResponse);

        #endregion

        #region Equals(PullEVSEStatusByIdResponse)

        /// <summary>
        /// Compares two PullEVSEStatusById responses for equality.
        /// </summary>
        /// <param name="PullEVSEStatusByIdResponse">A PullEVSEStatusById response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEStatusByIdResponse PullEVSEStatusByIdResponse)

            => !(PullEVSEStatusByIdResponse is null) &&

               (!EVSEStatusRecords.Any() && !PullEVSEStatusByIdResponse.EVSEStatusRecords.Any()) ||
                (EVSEStatusRecords.Any() &&  PullEVSEStatusByIdResponse.EVSEStatusRecords.Any() && EVSEStatusRecords.Count().Equals(PullEVSEStatusByIdResponse.EVSEStatusRecords.Count())) &&

               ((StatusCode == null && PullEVSEStatusByIdResponse.StatusCode == null) ||
                (StatusCode != null && PullEVSEStatusByIdResponse.StatusCode != null && StatusCode.Equals(PullEVSEStatusByIdResponse.StatusCode)));

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

                return EVSEStatusRecords.Aggregate(0, (hashCode, evseStatusRecord) => hashCode ^ evseStatusRecord.GetHashCode()) ^
                       StatusCode?.GetHashCode() ?? 0;

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVSEStatusRecords.Count() + " EVSE status record(s)",
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
                           EVSEStatusRecords,
                           StatusCode,
                           ProcessId,
                           HTTPResponse,
                           CustomData);

        #endregion

        #region (class) Builder

        /// <summary>
        /// An EVSEStatusById response builder.
        /// </summary>
        public new class Builder : AResponse<PullEVSEStatusByIdRequest,
                                             PullEVSEStatusByIdResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// An enumeration of EVSE status records.
            /// </summary>
            public HashSet<EVSEStatusRecord>  EVSEStatusRecords    { get; }

            /// <summary>
            /// The status code for this request.
            /// </summary>
            public StatusCode.Builder         StatusCode           { get; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new PullEVSEStatusById response builder.
            /// </summary>
            /// <param name="Request">A PullEVSEStatusById request.</param>
            /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
            /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
            /// <param name="Runtime">The runtime of the request/response.</param>
            /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
            /// <param name="StatusCode">An optional status code of this response.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(PullEVSEStatusByIdRequest      Request             = null,
                           DateTime?                      ResponseTimestamp   = null,
                           EventTracking_Id               EventTrackingId     = null,
                           TimeSpan?                      Runtime             = null,
                           IEnumerable<EVSEStatusRecord>  EVSEStatusRecords   = null,
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

                this.EVSEStatusRecords  = EVSEStatusRecords != null ? new HashSet<EVSEStatusRecord>(EVSEStatusRecords) : new HashSet<EVSEStatusRecord>();
                this.StatusCode         = StatusCode        != null ? StatusCode.ToBuilder()                           : new StatusCode.Builder();

            }

            #endregion


            #region Equals(EVSEStatusById)

            ///// <summary>
            ///// Compares two EVSE status by id responses for equality.
            ///// </summary>
            ///// <param name="EVSEStatusById">An EVSE status by id response to compare with.</param>
            ///// <returns>True if both match; False otherwise.</returns>
            //public override Boolean Equals(EVSEStatusById EVSEStatusById)
            //{

            //    if ((Object) EVSEStatusById == null)
            //        return false;

            //    return (!EVSEStatusRecords.Any() && !EVSEStatusById.EVSEStatusRecords.Any()) ||
            //            (EVSEStatusRecords.Any() && EVSEStatusById.EVSEStatusRecords.Any() && EVSEStatusRecords.Count().Equals(EVSEStatusById.EVSEStatusRecords.Count())) &&

            //            (StatusCode != null && EVSEStatusById.StatusCode != null) ||
            //            (StatusCode == null && EVSEStatusById.StatusCode == null && StatusCode.Equals(EVSEStatusById.StatusCode));

            //}

            #endregion

            public override PullEVSEStatusByIdResponse ToImmutable()

                => new PullEVSEStatusByIdResponse(Request           ?? throw new ArgumentNullException(nameof(Request), "The given request must not be null!"),
                                                  ResponseTimestamp ?? DateTime.UtcNow,
                                                  EventTrackingId   ?? EventTracking_Id.New,
                                                  Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                                  EVSEStatusRecords,
                                                  StatusCode,
                                                  ProcessId,
                                                  HTTPResponse,
                                                  CustomData);

        }

        #endregion

    }

}
