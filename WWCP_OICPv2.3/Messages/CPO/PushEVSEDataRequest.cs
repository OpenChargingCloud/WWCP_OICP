﻿/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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
    /// The push EVSE data request.
    /// </summary>
    public class PushEVSEDataRequest : ARequest<PushEVSEDataRequest>
    {

        #region Properties

        /// <summary>
        /// The operator EVSE data record.
        /// </summary>
        [Mandatory]
        public OperatorEVSEData             OperatorEVSEData    { get; }

        /// <summary>
        /// The server-side data management operation.
        /// </summary>
        [Mandatory]
        public ActionTypes                  Action              { get; }

        /// <summary>
        /// The enumeration of EVSE data records.
        /// </summary>
        public IEnumerable<EVSEDataRecord>  EVSEDataRecords
            => OperatorEVSEData.EVSEDataRecords;

        /// <summary>
        /// The unqiue identification of the charging station operator maintaining the given EVSE data records.
        /// </summary>
        public Operator_Id                  OperatorId
            => OperatorEVSEData.OperatorId;

        /// <summary>
        /// The optional name of the charging station operator maintaining the given EVSE data records.
        /// </summary>
        public String                       OperatorName
            => OperatorEVSEData.OperatorName;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new push EVSE data request.
        /// </summary>
        /// <param name="OperatorEVSEData">The operator EVSE data record.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public PushEVSEDataRequest(OperatorEVSEData    OperatorEVSEData,
                                   ActionTypes         Action              = ActionTypes.FullLoad,

                                   DateTime?           Timestamp           = null,
                                   CancellationToken?  CancellationToken   = null,
                                   EventTracking_Id    EventTrackingId     = null,
                                   TimeSpan?           RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.OperatorEVSEData  = OperatorEVSEData ?? throw new ArgumentNullException(nameof(OperatorEVSEData), "The given operator EVSE data must not be null!");
            this.Action            = Action;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#31-eroamingpushevsedata_v23

        // {
        //   "ActionType":      "fullLoad",
        //   "OperatorEvseData": {
        //     "EvseDataRecord": [
        //       {
        //         ...
        //       }
        //     ],
        //     "OperatorID":    "string",
        //     "OperatorName":  "string"
        //   }
        // }

        #endregion

        #region (static) Parse   (JSON, CustomPushEVSEDataRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a push EVSE data request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomPushEVSEDataRequestParser">A delegate to parse custom push EVSE data requests JSON objects.</param>
        public static PushEVSEDataRequest Parse(JObject                                           JSON,
                                                CustomJObjectParserDelegate<PushEVSEDataRequest>  CustomPushEVSEDataRequestParser   = null)
        {

            if (TryParse(JSON,
                         out PushEVSEDataRequest pushEVSEDataRequest,
                         out String              ErrorResponse,
                         CustomPushEVSEDataRequestParser))
            {
                return pushEVSEDataRequest;
            }

            throw new ArgumentException("The given JSON representation of a push EVSE data request is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomPushEVSEDataRequestParser = null)

        /// <summary>
        /// Parse the given text representation of a push EVSE data request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomPushEVSEDataRequestParser">A delegate to parse custom push EVSE data requests JSON objects.</param>
        public static PushEVSEDataRequest Parse(String                                            Text,
                                                CustomJObjectParserDelegate<PushEVSEDataRequest>  CustomPushEVSEDataRequestParser   = null)
        {

            if (TryParse(Text,
                         out PushEVSEDataRequest pushEVSEDataRequest,
                         out String              ErrorResponse,
                         CustomPushEVSEDataRequestParser))
            {
                return pushEVSEDataRequest;
            }

            throw new ArgumentException("The given text representation of a push EVSE data request is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out PushEVSEDataRequest, out ErrorResponse, CustomPushEVSEDataRequestParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a push EVSE data request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="PushEVSEDataRequest">The parsed push EVSE data request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                  JSON,
                                       out PushEVSEDataRequest  PushEVSEDataRequest,
                                       out String               ErrorResponse)

            => TryParse(JSON,
                        out PushEVSEDataRequest,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a push EVSE data request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="PushEVSEDataRequest">The parsed push EVSE data request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPushEVSEDataRequestParser">A delegate to parse custom push EVSE data requests JSON objects.</param>
        public static Boolean TryParse(JObject                                           JSON,
                                       out PushEVSEDataRequest                           PushEVSEDataRequest,
                                       out String                                        ErrorResponse,
                                       CustomJObjectParserDelegate<PushEVSEDataRequest>  CustomPushEVSEDataRequestParser)
        {

            try
            {

                PushEVSEDataRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse ActionType          [mandatory]

                if (!JSON.ParseMandatoryEnum("ActionType",
                                             "action type",
                                             out ActionTypes ActionType,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse OperatorEVSEData    [mandatory]

                if (!JSON.ParseMandatory("OperatorEvseData",
                                         "operator EVSE data",
                                         OICPv2_3.OperatorEVSEData.TryParse,
                                         out OperatorEVSEData OperatorEVSEData,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion


                PushEVSEDataRequest = new PushEVSEDataRequest(OperatorEVSEData,
                                                              ActionType);

                if (CustomPushEVSEDataRequestParser != null)
                    PushEVSEDataRequest = CustomPushEVSEDataRequestParser(JSON,
                                                                          PushEVSEDataRequest);

                return true;

            }
            catch (Exception e)
            {
                PushEVSEDataRequest  = default;
                ErrorResponse        = "The given JSON representation of a push EVSE data request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out PushEVSEDataRequest, out ErrorResponse, CustomPushEVSEDataRequestParser = null)

        /// <summary>
        /// Try to parse the given text representation of a push EVSE data request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="PushEVSEDataRequest">The parsed push EVSE data request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPushEVSEDataRequestParser">A delegate to parse custom push EVSE data requests JSON objects.</param>
        public static Boolean TryParse(String                                            Text,
                                       out PushEVSEDataRequest                           PushEVSEDataRequest,
                                       out String                                        ErrorResponse,
                                       CustomJObjectParserDelegate<PushEVSEDataRequest>  CustomPushEVSEDataRequestParser)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out PushEVSEDataRequest,
                                out ErrorResponse,
                                CustomPushEVSEDataRequestParser);

            }
            catch (Exception e)
            {
                PushEVSEDataRequest  = default;
                ErrorResponse        = "The given text representation of a push EVSE data request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPushEVSEDataRequestSerializer = null, CustomOperatorEVSEDataSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomPushEVSEDataRequestSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomOperatorEVSEDataSerializer">A delegate to serialize custom operator EVSE data JSON objects.</param>
        /// <param name="CustomEVSEDataRecordSerializer">A delegate to serialize custom EVSE data record JSON objects.</param>
        /// <param name="CustomAddressSerializer">A delegate to serialize custom address JSON objects.</param>
        /// <param name="CustomChargingFacilitySerializer">A delegate to serialize custom charging facility JSON objects.</param>
        /// <param name="CustomGeoCoordinatesSerializer">A delegate to serialize custom geo coordinates JSON objects.</param>
        /// <param name="CustomEnergySourceSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomEnvironmentalImpactSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomOpeningTimesSerializer">A delegate to serialize custom opening time JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PushEVSEDataRequest>  CustomPushEVSEDataRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<OperatorEVSEData>     CustomOperatorEVSEDataSerializer      = null,
                              CustomJObjectSerializerDelegate<EVSEDataRecord>       CustomEVSEDataRecordSerializer        = null,
                              CustomJObjectSerializerDelegate<Address>              CustomAddressSerializer               = null,
                              CustomJObjectSerializerDelegate<ChargingFacility>     CustomChargingFacilitySerializer      = null,
                              CustomJObjectSerializerDelegate<GeoCoordinates>       CustomGeoCoordinatesSerializer        = null,
                              CustomJObjectSerializerDelegate<EnergySource>         CustomEnergySourceSerializer          = null,
                              CustomJObjectSerializerDelegate<EnvironmentalImpact>  CustomEnvironmentalImpactSerializer   = null,
                              CustomJObjectSerializerDelegate<OpeningTime>          CustomOpeningTimesSerializer          = null)
        {

            var JSON = JSONObject.Create(
                           new JProperty("ActionType",        Action.AsString()),
                           new JProperty("OperatorEvseData",  OperatorEVSEData.ToJSON(CustomOperatorEVSEDataSerializer,
                                                                                      CustomEVSEDataRecordSerializer,
                                                                                      CustomAddressSerializer,
                                                                                      CustomChargingFacilitySerializer,
                                                                                      CustomGeoCoordinatesSerializer,
                                                                                      CustomEnergySourceSerializer,
                                                                                      CustomEnvironmentalImpactSerializer,
                                                                                      CustomOpeningTimesSerializer))
                       );

            return CustomPushEVSEDataRequestSerializer != null
                       ? CustomPushEVSEDataRequestSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this request.
        /// </summary>
        public PushEVSEDataRequest Clone

            => new PushEVSEDataRequest(OperatorEVSEData.Clone,
                                       Action);

        #endregion


        #region Operator overloading

        #region Operator == (PushEVSEData1, PushEVSEData2)

        /// <summary>
        /// Compares two push EVSE data requests for equality.
        /// </summary>
        /// <param name="PushEVSEData1">An push EVSE data request.</param>
        /// <param name="PushEVSEData2">Another push EVSE data request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PushEVSEDataRequest PushEVSEData1,
                                           PushEVSEDataRequest PushEVSEData2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PushEVSEData1, PushEVSEData2))
                return true;

            // If one is null, but not both, return false.
            if (PushEVSEData1 is null || PushEVSEData2 is null)
                return false;

            return PushEVSEData1.Equals(PushEVSEData2);

        }

        #endregion

        #region Operator != (PushEVSEData1, PushEVSEData2)

        /// <summary>
        /// Compares two push EVSE data requests for inequality.
        /// </summary>
        /// <param name="PushEVSEData1">An push EVSE data request.</param>
        /// <param name="PushEVSEData2">Another push EVSE data request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PushEVSEDataRequest PushEVSEData1,
                                           PushEVSEDataRequest PushEVSEData2)

            => !(PushEVSEData1 == PushEVSEData2);

        #endregion

        #endregion

        #region IEquatable<PushEVSEDataRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is PushEVSEDataRequest pushEVSEDataRequest &&
                   Equals(pushEVSEDataRequest);

        #endregion

        #region Equals(PushEVSEDataRequest)

        /// <summary>
        /// Compares two push EVSE data requests for equality.
        /// </summary>
        /// <param name="PushEVSEDataRequest">An push EVSE data request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PushEVSEDataRequest PushEVSEDataRequest)

            => !(PushEVSEDataRequest is null) &&

                 OperatorEVSEData.Equals(PushEVSEDataRequest.OperatorEVSEData) &&
                 Action.          Equals(PushEVSEDataRequest.Action);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return OperatorEVSEData.GetHashCode() * 3 ^
                       Action.          GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Action, " of ",
                             EVSEDataRecords.Count(), " EVSE data record(s)",
                             " by ", OperatorName, " (", OperatorId, ")");

        #endregion

    }

}
