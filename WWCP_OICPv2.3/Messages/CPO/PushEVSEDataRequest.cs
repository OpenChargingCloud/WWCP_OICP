/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The PushEVSEData request.
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
        /// The unique identification of the charging station operator maintaining the given EVSE data records.
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

#pragma warning disable IDE0290 // Use primary constructor

        /// <summary>
        /// Create a new PushEVSEData request.
        /// </summary>
        /// <param name="OperatorEVSEData">The operator EVSE data record.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public PushEVSEDataRequest(OperatorEVSEData    OperatorEVSEData,
                                   ActionTypes         Action              = ActionTypes.FullLoad,
                                   Process_Id?         ProcessId           = null,
                                   JObject?            CustomData          = null,

                                   DateTimeOffset?     Timestamp           = null,
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

            this.OperatorEVSEData  = OperatorEVSEData ?? throw new ArgumentNullException(nameof(OperatorEVSEData), "The given operator EVSE data must not be null!");
            this.Action            = Action;

        }

#pragma warning restore IDE0290 // Use primary constructor

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#eRoamingPushEvseData

        // {
        //   "ActionType":  "fullLoad",
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

        #region (static) Parse   (JSON, ..., CustomPushEVSEDataRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a push EVSE data request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomPushEVSEDataRequestParser">A delegate to parse custom push EVSE data request JSON objects.</param>
        public static PushEVSEDataRequest Parse(JObject                                            JSON,
                                                Process_Id?                                        ProcessId                         = null,

                                                DateTimeOffset?                                    Timestamp                         = null,
                                                EventTracking_Id?                                  EventTrackingId                   = null,
                                                TimeSpan?                                          RequestTimeout                    = null,
                                                CustomJObjectParserDelegate<PushEVSEDataRequest>?  CustomPushEVSEDataRequestParser   = null,
                                                CancellationToken                                  CancellationToken                 = default)
        {

            if (TryParse(JSON,
                         out var pushEVSEDataRequest,
                         out var errorResponse,
                         ProcessId,
                         Timestamp,
                         EventTrackingId,
                         RequestTimeout,
                         CustomPushEVSEDataRequestParser,
                         CancellationToken))
            {
                return pushEVSEDataRequest;
            }

            throw new ArgumentException("The given JSON representation of a push EVSE data request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out PushEVSEDataRequest, out ErrorResponse, ..., CustomPushEVSEDataRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a push EVSE data request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="PushEVSEDataRequest">The parsed push EVSE data request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomPushEVSEDataRequestParser">A delegate to parse custom push EVSE data request JSON objects.</param>
        public static Boolean TryParse(JObject                                            JSON,
                                       [NotNullWhen(true)]  out PushEVSEDataRequest?      PushEVSEDataRequest,
                                       [NotNullWhen(false)] out String?                   ErrorResponse,
                                       Process_Id?                                        ProcessId                         = null,

                                       DateTimeOffset?                                    Timestamp                         = null,
                                       EventTracking_Id?                                  EventTrackingId                   = null,
                                       TimeSpan?                                          RequestTimeout                    = null,
                                       CustomJObjectParserDelegate<PushEVSEDataRequest>?  CustomPushEVSEDataRequestParser   = null,
                                       CancellationToken                                  CancellationToken                 = default)
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

                if (!JSON.ParseMandatory("ActionType",
                                         "action type",
                                         ActionTypesExtensions.TryParse,
                                         out ActionTypes ActionType,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse OperatorEVSEData    [mandatory]

                if (!JSON.ParseMandatoryJSON("OperatorEvseData",
                                             "operator EVSE data",
                                             OICPv2_3.OperatorEVSEData.TryParse,
                                             out OperatorEVSEData? OperatorEVSEData,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse CustomData          [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                PushEVSEDataRequest = new PushEVSEDataRequest(
                                          OperatorEVSEData,
                                          ActionType,
                                          ProcessId,
                                          customData,

                                          Timestamp,
                                          EventTrackingId,
                                          RequestTimeout,
                                          CancellationToken
                                      );

                if (CustomPushEVSEDataRequestParser is not null)
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

        #region ToJSON(CustomPushEVSEDataRequestSerializer = null, CustomOperatorEVSEDataSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomPushEVSEDataRequestSerializer">A delegate to serialize custom PushEVSEData request.</param>
        /// <param name="CustomOperatorEVSEDataSerializer">A delegate to serialize custom operator EVSE data JSON objects.</param>
        /// <param name="CustomEVSEDataRecordSerializer">A delegate to serialize custom EVSE data record JSON objects.</param>
        /// <param name="CustomAddressSerializer">A delegate to serialize custom address JSON objects.</param>
        /// <param name="CustomChargingFacilitySerializer">A delegate to serialize custom charging facility JSON objects.</param>
        /// <param name="CustomGeoCoordinatesSerializer">A delegate to serialize custom geo coordinates JSON objects.</param>
        /// <param name="CustomEnergyMeterSerializer">A delegate to serialize custom energy meter JSON objects.</param>
        /// <param name="CustomTransparencySoftwareStatusSerializer">A delegate to serialize custom transparency software status JSON objects.</param>
        /// <param name="CustomTransparencySoftwareSerializer">A delegate to serialize custom transparency software JSON objects.</param>
        /// <param name="CustomEnergySourceSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomEnvironmentalImpactSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomOpeningTimesSerializer">A delegate to serialize custom opening time JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PushEVSEDataRequest>?         CustomPushEVSEDataRequestSerializer          = null,
                              CustomJObjectSerializerDelegate<OperatorEVSEData>?            CustomOperatorEVSEDataSerializer             = null,
                              CustomJObjectSerializerDelegate<EVSEDataRecord>?              CustomEVSEDataRecordSerializer               = null,
                              CustomJObjectSerializerDelegate<Address>?                     CustomAddressSerializer                      = null,
                              CustomJObjectSerializerDelegate<ChargingFacility>?            CustomChargingFacilitySerializer             = null,
                              CustomJObjectSerializerDelegate<GeoCoordinates>?              CustomGeoCoordinatesSerializer               = null,
                              CustomJObjectSerializerDelegate<EnergyMeter>?                 CustomEnergyMeterSerializer                  = null,
                              CustomJObjectSerializerDelegate<TransparencySoftwareStatus>?  CustomTransparencySoftwareStatusSerializer   = null,
                              CustomJObjectSerializerDelegate<TransparencySoftware>?        CustomTransparencySoftwareSerializer         = null,
                              CustomJObjectSerializerDelegate<EnergySource>?                CustomEnergySourceSerializer                 = null,
                              CustomJObjectSerializerDelegate<EnvironmentalImpact>?         CustomEnvironmentalImpactSerializer          = null,
                              CustomJObjectSerializerDelegate<OpeningTime>?                 CustomOpeningTimesSerializer                 = null)
        {

            var json = JSONObject.Create(

                           new JProperty("ActionType",        Action.AsString()),

                           new JProperty("OperatorEvseData",  OperatorEVSEData.ToJSON(CustomOperatorEVSEDataSerializer,
                                                                                      CustomEVSEDataRecordSerializer,
                                                                                      CustomAddressSerializer,
                                                                                      CustomChargingFacilitySerializer,
                                                                                      CustomGeoCoordinatesSerializer,
                                                                                      CustomEnergyMeterSerializer,
                                                                                      CustomTransparencySoftwareStatusSerializer,
                                                                                      CustomTransparencySoftwareSerializer,
                                                                                      CustomEnergySourceSerializer,
                                                                                      CustomEnvironmentalImpactSerializer,
                                                                                      CustomOpeningTimesSerializer)),

                           CustomData is not null
                               ? new JProperty("CustomData",  CustomData)
                               : null

                       );

            return CustomPushEVSEDataRequestSerializer is not null
                       ? CustomPushEVSEDataRequestSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this push EVSE data request.
        /// </summary>
        public PushEVSEDataRequest Clone()

            => new (
                   OperatorEVSEData.Clone(),
                   Action
               );

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

            if (ReferenceEquals(PushEVSEData1, PushEVSEData2))
                return true;

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
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is PushEVSEDataRequest pushEVSEDataRequest &&
                   Equals(pushEVSEDataRequest);

        #endregion

        #region Equals(PushEVSEDataRequest)

        /// <summary>
        /// Compares two push EVSE data requests for equality.
        /// </summary>
        /// <param name="PushEVSEDataRequest">An push EVSE data request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PushEVSEDataRequest? PushEVSEDataRequest)

            => PushEVSEDataRequest is not null &&

               OperatorEVSEData.Equals(PushEVSEDataRequest.OperatorEVSEData) &&
               Action.          Equals(PushEVSEDataRequest.Action);

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

                return OperatorEVSEData.GetHashCode() * 3 ^
                       Action.          GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{Action} of {EVSEDataRecords.Count()} EVSE data record(s) by {OperatorName} ({OperatorId})";

        #endregion

    }

}
