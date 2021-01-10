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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The PullEVSEStatusByOperatorId response.
    /// </summary>
    public class PullEVSEStatusByOperatorIdResponse : AResponse<PullEVSEStatusByOperatorIdRequest,
                                                                PullEVSEStatusByOperatorIdResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE status records grouped by their operators.
        /// </summary>
        [Mandatory]
        public IEnumerable<OperatorEVSEStatus>  OperatorEVSEStatus    { get; }

        /// <summary>
        /// The optional status code of this response.
        /// </summary>
        [Optional]
        public StatusCode                       StatusCode            { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullEVSEStatusByOperatorId response.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusByOperatorIdRequest request.</param>
        /// <param name="OperatorEVSEStatus">An enumeration of EVSE status records grouped by their operators.</param>
        /// <param name="StatusCode">An optional status code of this response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public PullEVSEStatusByOperatorIdResponse(PullEVSEStatusByOperatorIdRequest  Request,
                                                  IEnumerable<OperatorEVSEStatus>    OperatorEVSEStatus,
                                                  StatusCode                         StatusCode   = null,
                                                  Process_Id?                        ProcessId    = null,
                                                  JObject                            CustomData   = null)

            : base(Request,
                   DateTime.UtcNow,
                   ProcessId,
                   CustomData)

        {

            this.OperatorEVSEStatus  = OperatorEVSEStatus ?? throw new ArgumentNullException(nameof(OperatorEVSEStatus), "The given OperatorEVSEStatus must not be null!");
            this.StatusCode          = StatusCode;

        }

        #endregion


        #region Documentation

        // {
        //   "EvseStatuses": {
        //     "OperatorEvseStatus": [
        //       {
        //         "EvseStatusRecord": [
        //           {
        //             "EvseID":      "string",
        //             "EvseStatus":  "Available"
        //           }
        //         ],
        //         "OperatorID":      "string",
        //         "OperatorName":    "string"
        //       }
        //     ]
        //   },
        //   "StatusCode": {
        //     "AdditionalInfo":      "string",
        //     "Code":                "000",
        //     "Description":         "string"
        //   }
        // }

        #endregion

        #region (static) Parse   (JSON, CustomPullEVSEStatusByOperatorIdResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullEVSEStatusByOperatorId response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomPullEVSEStatusByOperatorIdResponseParser">A delegate to parse custom PullEVSEStatusByOperatorId JSON objects.</param>
        public static PullEVSEStatusByOperatorIdResponse Parse(PullEVSEStatusByOperatorIdRequest                                Request,
                                                               JObject                                                          JSON,
                                                               CustomJObjectParserDelegate<PullEVSEStatusByOperatorIdResponse>  CustomPullEVSEStatusByOperatorIdResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         out PullEVSEStatusByOperatorIdResponse  pullEVSEStatusByOperatorIdResponse,
                         out String                              ErrorResponse,
                         CustomPullEVSEStatusByOperatorIdResponseParser))
            {
                return pullEVSEStatusByOperatorIdResponse;
            }

            throw new ArgumentException("The given JSON representation of a PullEVSEStatusByOperatorId response is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomPullEVSEStatusByOperatorIdResponseParser = null)

        /// <summary>
        /// Parse the given text representation of a PullEVSEStatusByOperatorId response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomPullEVSEStatusByOperatorIdResponseParser">A delegate to parse custom PullEVSEStatusByOperatorId response JSON objects.</param>
        public static PullEVSEStatusByOperatorIdResponse Parse(PullEVSEStatusByOperatorIdRequest                                Request,
                                                               String                                                           Text,
                                                               CustomJObjectParserDelegate<PullEVSEStatusByOperatorIdResponse>  CustomPullEVSEStatusByOperatorIdResponseParser   = null)
        {

            if (TryParse(Request,
                         Text,
                         out PullEVSEStatusByOperatorIdResponse  pullEVSEStatusByOperatorIdResponse,
                         out String                              ErrorResponse,
                         CustomPullEVSEStatusByOperatorIdResponseParser))
            {
                return pullEVSEStatusByOperatorIdResponse;
            }

            throw new ArgumentException("The given text representation of a PullEVSEStatusByOperatorId response is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out PullEVSEStatusByOperatorIdResponse, out ErrorResponse, CustomPullEVSEStatusByOperatorIdResponseParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a PullEVSEStatusByOperatorId response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="PullEVSEStatusByOperatorIdResponse">The parsed PullEVSEStatusByOperatorId response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(PullEVSEStatusByOperatorIdRequest       Request,
                                       JObject                                 JSON,
                                       out PullEVSEStatusByOperatorIdResponse  PullEVSEStatusByOperatorIdResponse,
                                       out String                              ErrorResponse)

            => TryParse(Request,
                        JSON,
                        out PullEVSEStatusByOperatorIdResponse,
                        out ErrorResponse,
                        null,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a PullEVSEStatusByOperatorId response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="PullEVSEStatusByOperatorIdResponse">The parsed PullEVSEStatusByOperatorId response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPullEVSEStatusByOperatorIdResponseParser">A delegate to parse custom PullEVSEStatusByOperatorId response JSON objects.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        public static Boolean TryParse(PullEVSEStatusByOperatorIdRequest                                Request,
                                       JObject                                                          JSON,
                                       out PullEVSEStatusByOperatorIdResponse                           PullEVSEStatusByOperatorIdResponse,
                                       out String                                                       ErrorResponse,
                                       CustomJObjectParserDelegate<PullEVSEStatusByOperatorIdResponse>  CustomPullEVSEStatusByOperatorIdResponseParser,
                                       Process_Id?                                                      ProcessId   = null)
        {

            try
            {

                PullEVSEStatusByOperatorIdResponse = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse OperatorEVSEStatus    [mandatory]

                if (!JSON.ParseMandatory("EvseStatuses",
                                         "EVSE statuses",
                                         out JObject EVSEStatuses,
                                         out         ErrorResponse))
                {
                    return false;
                }

                if (!EVSEStatuses.ParseMandatoryJSON("OperatorEvseStatus",
                                                     "operator EVSE status",
                                                     OICPv2_3.OperatorEVSEStatus.TryParse,
                                                     out IEnumerable<OperatorEVSEStatus> OperatorEVSEStatus,
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

                #region Parse Custom Data           [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                PullEVSEStatusByOperatorIdResponse = new PullEVSEStatusByOperatorIdResponse(Request,
                                                                                            OperatorEVSEStatus,
                                                                                            StatusCode,
                                                                                            ProcessId,
                                                                                            CustomData);

                if (CustomPullEVSEStatusByOperatorIdResponseParser != null)
                    PullEVSEStatusByOperatorIdResponse = CustomPullEVSEStatusByOperatorIdResponseParser(JSON,
                                                                                                        PullEVSEStatusByOperatorIdResponse);

                return true;

            }
            catch (Exception e)
            {
                PullEVSEStatusByOperatorIdResponse  = default;
                ErrorResponse                       = "The given JSON representation of a PullEVSEStatusByOperatorId response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out PullEVSEStatusByOperatorIdResponse, out ErrorResponse, CustomPullEVSEStatusByOperatorIdResponseParser = null)

        /// <summary>
        /// Try to parse the given text representation of a PullEVSEStatusByOperatorId response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="Text">The text to parse.</param>
        /// <param name="PullEVSEStatusByOperatorIdResponse">The parsed PullEVSEStatusByOperatorId response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPullEVSEStatusByOperatorIdResponseParser">A delegate to parse custom PullEVSEStatusByOperatorId response JSON objects.</param>
        public static Boolean TryParse(PullEVSEStatusByOperatorIdRequest                                Request,
                                       String                                                           Text,
                                       out PullEVSEStatusByOperatorIdResponse                           PullEVSEStatusByOperatorIdResponse,
                                       out String                                                       ErrorResponse,
                                       CustomJObjectParserDelegate<PullEVSEStatusByOperatorIdResponse>  CustomPullEVSEStatusByOperatorIdResponseParser)
        {

            try
            {

                return TryParse(Request,
                                JObject.Parse(Text),
                                out PullEVSEStatusByOperatorIdResponse,
                                out ErrorResponse,
                                CustomPullEVSEStatusByOperatorIdResponseParser);

            }
            catch (Exception e)
            {
                PullEVSEStatusByOperatorIdResponse  = default;
                ErrorResponse                       = "The given text representation of a PullEVSEStatusByOperatorId response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPullEVSEStatusByOperatorIdResponseSerializer = null, CustomEVSEStatusRecordSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEStatusByOperatorIdResponseSerializer">A delegate to customize the serialization of PullEVSEStatusByOperatorIdResponse responses.</param>
        /// <param name="CustomOperatorEVSEStatusSerializer">A delegate to serialize custom operator EVSE status JSON objects.</param>
        /// <param name="CustomEVSEStatusRecordSerializer">A delegate to serialize custom EVSE status record JSON objects.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullEVSEStatusByOperatorIdResponse>  CustomPullEVSEStatusByOperatorIdResponseSerializer   = null,
                              CustomJObjectSerializerDelegate<OperatorEVSEStatus>                  CustomOperatorEVSEStatusSerializer                   = null,
                              CustomJObjectSerializerDelegate<EVSEStatusRecord>                    CustomEVSEStatusRecordSerializer                     = null,
                              CustomJObjectSerializerDelegate<StatusCode>                          CustomStatusCodeSerializer                           = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("EvseStatuses",
                               new JProperty("OperatorEvseStatus",  new JArray(OperatorEVSEStatus.Select(operatorEVSEStatus => operatorEVSEStatus.ToJSON(CustomOperatorEVSEStatusSerializer,
                                                                                                                                                         CustomEVSEStatusRecordSerializer))))
                           ),

                           StatusCode != null
                               ? new JProperty("StatusCode",  StatusCode.ToJSON(CustomStatusCodeSerializer))
                               : null

                       );

            return CustomPullEVSEStatusByOperatorIdResponseSerializer != null
                       ? CustomPullEVSEStatusByOperatorIdResponseSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="EVSEStatus1">An EVSE status.</param>
        /// <param name="EVSEStatus2">Another EVSE status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullEVSEStatusByOperatorIdResponse EVSEStatus1,
                                           PullEVSEStatusByOperatorIdResponse EVSEStatus2)
        {

            if (ReferenceEquals(EVSEStatus1, EVSEStatus2))
                return true;

            if (EVSEStatus1 is null || EVSEStatus2 is null)
                return false;

            return EVSEStatus1.Equals(EVSEStatus2);

        }

        #endregion

        #region Operator != (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="EVSEStatus1">An EVSE status.</param>
        /// <param name="EVSEStatus2">Another EVSE status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullEVSEStatusByOperatorIdResponse EVSEStatus1,
                                           PullEVSEStatusByOperatorIdResponse EVSEStatus2)

            => !(EVSEStatus1 == EVSEStatus2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEStatusByOperatorIdResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is PullEVSEStatusByOperatorIdResponse pullEVSEStatusByOperatorIdResponse &&
                   Equals(pullEVSEStatusByOperatorIdResponse);

        #endregion

        #region Equals(PullEVSEStatusByOperatorIdResponse)

        /// <summary>
        /// Compares two PullEVSEStatusByOperatorId responses for equality.
        /// </summary>
        /// <param name="PullEVSEStatusByOperatorIdResponse">A PullEVSEStatusByOperatorId response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEStatusByOperatorIdResponse PullEVSEStatusByOperatorIdResponse)

            => !(PullEVSEStatusByOperatorIdResponse is null) &&

               (!OperatorEVSEStatus.Any() && !PullEVSEStatusByOperatorIdResponse.OperatorEVSEStatus.Any()) ||
                (OperatorEVSEStatus.Any() &&  PullEVSEStatusByOperatorIdResponse.OperatorEVSEStatus.Any() && OperatorEVSEStatus.Count().Equals(PullEVSEStatusByOperatorIdResponse.OperatorEVSEStatus.Count())) &&

               ((StatusCode == null && PullEVSEStatusByOperatorIdResponse.StatusCode == null) ||
                (StatusCode != null && PullEVSEStatusByOperatorIdResponse.StatusCode != null && StatusCode.Equals(PullEVSEStatusByOperatorIdResponse.StatusCode)));

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

                return OperatorEVSEStatus.Aggregate(0, (hashCode, operatorEVSEStatus) => hashCode ^ operatorEVSEStatus.GetHashCode()) ^
                       StatusCode?.GetHashCode() ?? 0;

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(OperatorEVSEStatus.Count() + " operator EVSE status record(s)",
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
                           OperatorEVSEStatus,
                           StatusCode,
                           ProcessId,
                           CustomData);

        #endregion

        #region (class) Builder

        /// <summary>
        /// An EVSEStatus response builder.
        /// </summary>
        public new class Builder : AResponse<PullEVSEStatusByOperatorIdRequest,
                                             PullEVSEStatusByOperatorIdResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// An enumeration of EVSE status records grouped by their operators.
            /// </summary>
            public HashSet<OperatorEVSEStatus>  OperatorEVSEStatus   { get; }

            /// <summary>
            /// The status code for this request.
            /// </summary>
            public StatusCode.Builder           StatusCode           { get; set; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new EVSEStatus response builder.
            /// </summary>
            /// <param name="Request">A PullEVSEStatusByOperatorIdRequest request.</param>
            /// <param name="OperatorEVSEStatus">An enumeration of EVSE status records grouped by their operators.</param>
            /// <param name="StatusCode">An optional status code for this request.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(PullEVSEStatusByOperatorIdRequest  Request = null,
                           IEnumerable<OperatorEVSEStatus>    OperatorEVSEStatus = null,
                           StatusCode                         StatusCode         = null,
                           Process_Id?                        ProcessId          = null,
                           JObject                            CustomData         = null)

                : base(Request,
                       DateTime.UtcNow,
                       ProcessId,
                       CustomData)

            {

                this.OperatorEVSEStatus  = OperatorEVSEStatus != null ? new HashSet<OperatorEVSEStatus>(OperatorEVSEStatus) : new HashSet<OperatorEVSEStatus>();
                this.StatusCode          = StatusCode         != null ? StatusCode.ToBuilder()                              : new StatusCode.Builder();

            }

            #endregion


            #region Equals(EVSEStatus)

            ///// <summary>
            ///// Compares two EVSE status for equality.
            ///// </summary>
            ///// <param name="EVSEStatus">An EVSE status to compare with.</param>
            ///// <returns>True if both match; False otherwise.</returns>
            //public Boolean Equals(EVSEStatus2 EVSEStatus)

            //    => !(EVSEStatus is null) &&

            //       (!OperatorEVSEStatus.Any() && !EVSEStatus.OperatorEVSEStatus.Any()) ||
            //       (OperatorEVSEStatus.Any() &&  EVSEStatus.OperatorEVSEStatus.Any() && OperatorEVSEStatus.Count().Equals(EVSEStatus.OperatorEVSEStatus.Count())) &&

            //       (!StatusCode.HasValue && !EVSEStatus.StatusCode.HasValue) ||
            //        (StatusCode.HasValue &&  EVSEStatus.StatusCode.HasValue && StatusCode.Value.Equals(EVSEStatus.StatusCode.Value));

            #endregion

            public override PullEVSEStatusByOperatorIdResponse ToImmutable()

                => new PullEVSEStatusByOperatorIdResponse(Request,
                                   OperatorEVSEStatus,
                                   StatusCode,
                                   ProcessId,
                                   CustomData);

        }

        #endregion

    }

}
