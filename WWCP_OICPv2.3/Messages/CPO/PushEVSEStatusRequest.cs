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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The PushEVSEStatus request.
    /// </summary>
    public class PushEVSEStatusRequest : ARequest<PushEVSEStatusRequest>
    {

        #region Properties

        /// <summary>
        /// The operator EVSE status record.
        /// </summary>
        [Mandatory]
        public OperatorEVSEStatus             OperatorEVSEStatus    { get; }

        /// <summary>
        /// The server-side status management operation.
        /// </summary>
        [Mandatory]
        public ActionTypes                    Action                { get; }

        /// <summary>
        /// The enumeration of EVSE status records.
        /// </summary>
        public IEnumerable<EVSEStatusRecord>  EVSEStatusRecords
            => OperatorEVSEStatus.EVSEStatusRecords;

        /// <summary>
        /// The unqiue identification of the charging station operator maintaining the given EVSE status records.
        /// </summary>
        public Operator_Id                    OperatorId
            => OperatorEVSEStatus.OperatorId;

        /// <summary>
        /// The optional name of the charging station operator maintaining the given EVSE status records.
        /// </summary>
        public String                         OperatorName
            => OperatorEVSEStatus.OperatorName;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PushEVSEStatus request.
        /// </summary>
        /// <param name="OperatorEVSEStatus">The operator EVSE status record.</param>
        /// <param name="Action">The server-side status management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public PushEVSEStatusRequest(OperatorEVSEStatus  OperatorEVSEStatus,
                                     ActionTypes         Action              = ActionTypes.FullLoad,
                                     Process_Id?         ProcessId           = null,
                                     JObject?            CustomData          = null,

                                     DateTime?           Timestamp           = null,
                                     CancellationToken?  CancellationToken   = null,
                                     EventTracking_Id?   EventTrackingId     = null,
                                     TimeSpan?           RequestTimeout      = null)

            : base(ProcessId,
                   CustomData,
                   Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.OperatorEVSEStatus  = OperatorEVSEStatus ?? throw new ArgumentNullException(nameof(OperatorEVSEStatus), "The given operator EVSE status must not be null!");
            this.Action              = Action;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#eRoamingPushEvseStatus

        // {
        //   "ActionType":      "fullLoad",
        //   "OperatorEvseStatus": {
        //     "EvseStatusRecord": [
        //       {
        //         ...
        //       }
        //     ],
        //     "OperatorID":    "string",
        //     "OperatorName":  "string"
        //   }
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomPushEVSEStatusRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a push EVSE status request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomPushEVSEStatusRequestParser">A delegate to parse custom push EVSE status request JSON objects.</param>
        public static PushEVSEStatusRequest Parse(JObject                                              JSON,
                                                  Process_Id?                                          ProcessId                           = null,

                                                  DateTime?                                            Timestamp                           = null,
                                                  CancellationToken?                                   CancellationToken                   = null,
                                                  EventTracking_Id?                                    EventTrackingId                     = null,
                                                  TimeSpan?                                            RequestTimeout                      = null,

                                                  CustomJObjectParserDelegate<PushEVSEStatusRequest>?  CustomPushEVSEStatusRequestParser   = null)
        {

            if (TryParse(JSON,
                         out PushEVSEStatusRequest?  pushEVSEStatusRequest,
                         out String?                 errorResponse,
                         ProcessId,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout,
                         CustomPushEVSEStatusRequestParser))
            {
                return pushEVSEStatusRequest!;
            }

            throw new ArgumentException("The given JSON representation of a push EVSE status request is invalid: " + errorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, ..., CustomPushEVSEStatusRequestParser = null)

        /// <summary>
        /// Parse the given text representation of a push EVSE status request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomPushEVSEStatusRequestParser">A delegate to parse custom push EVSE status request JSON objects.</param>
        public static PushEVSEStatusRequest Parse(String                                               Text,
                                                  Process_Id?                                          ProcessId                           = null,

                                                  DateTime?                                            Timestamp                           = null,
                                                  CancellationToken?                                   CancellationToken                   = null,
                                                  EventTracking_Id?                                    EventTrackingId                     = null,
                                                  TimeSpan?                                            RequestTimeout                      = null,

                                                  CustomJObjectParserDelegate<PushEVSEStatusRequest>?  CustomPushEVSEStatusRequestParser   = null)
        {

            if (TryParse(Text,
                         out PushEVSEStatusRequest?  pushEVSEStatusRequest,
                         out String?                 errorResponse,
                         ProcessId,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout,
                         CustomPushEVSEStatusRequestParser))
            {
                return pushEVSEStatusRequest!;
            }

            throw new ArgumentException("The given text representation of a push EVSE status request is invalid: " + errorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out PushEVSEStatusRequest, out ErrorResponse, ..., CustomPushEVSEStatusRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a push EVSE status request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="PushEVSEStatusRequest">The parsed push EVSE status request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomPushEVSEStatusRequestParser">A delegate to parse custom push EVSE status request JSON objects.</param>
        public static Boolean TryParse(JObject                                              JSON,
                                       out PushEVSEStatusRequest?                           PushEVSEStatusRequest,
                                       out String?                                          ErrorResponse,
                                       Process_Id?                                          ProcessId                           = null,

                                       DateTime?                                            Timestamp                           = null,
                                       CancellationToken?                                   CancellationToken                   = null,
                                       EventTracking_Id?                                    EventTrackingId                     = null,
                                       TimeSpan?                                            RequestTimeout                      = null,

                                       CustomJObjectParserDelegate<PushEVSEStatusRequest>?  CustomPushEVSEStatusRequestParser   = null)
        {

            try
            {

                PushEVSEStatusRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse ActionType            [mandatory]

                if (!JSON.ParseMandatoryEnum("ActionType",
                                             "action type",
                                             out ActionTypes ActionType,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse OperatorEVSEStatus    [mandatory]

                if (!JSON.ParseMandatory("OperatorEvseStatus",
                                         "operator EVSE status",
                                         OICPv2_3.OperatorEVSEStatus.TryParse,
                                         out OperatorEVSEStatus OperatorEVSEStatus,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion


                #region Parse CustomData            [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                PushEVSEStatusRequest = new PushEVSEStatusRequest(OperatorEVSEStatus,
                                                                  ActionType,
                                                                  ProcessId,
                                                                  CustomData,
                                                                  Timestamp,
                                                                  CancellationToken,
                                                                  EventTrackingId,
                                                                  RequestTimeout);

                if (CustomPushEVSEStatusRequestParser is not null)
                    PushEVSEStatusRequest = CustomPushEVSEStatusRequestParser(JSON,
                                                                              PushEVSEStatusRequest);

                return true;

            }
            catch (Exception e)
            {
                PushEVSEStatusRequest  = default;
                ErrorResponse          = "The given JSON representation of a push EVSE status request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out PushEVSEStatusRequest, out ErrorResponse, ..., CustomPushEVSEStatusRequestParser = null)

        /// <summary>
        /// Try to parse the given text representation of a push EVSE status request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="PushEVSEStatusRequest">The parsed push EVSE status request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomPushEVSEStatusRequestParser">A delegate to parse custom push EVSE status request JSON objects.</param>
        public static Boolean TryParse(String                                               Text,
                                       out PushEVSEStatusRequest?                           PushEVSEStatusRequest,
                                       out String?                                          ErrorResponse,
                                       Process_Id?                                          ProcessId                           = null,

                                       DateTime?                                            Timestamp                           = null,
                                       CancellationToken?                                   CancellationToken                   = null,
                                       EventTracking_Id?                                    EventTrackingId                     = null,
                                       TimeSpan?                                            RequestTimeout                      = null,

                                       CustomJObjectParserDelegate<PushEVSEStatusRequest>?  CustomPushEVSEStatusRequestParser   = null)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out PushEVSEStatusRequest,
                                out ErrorResponse,
                                ProcessId,
                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout,
                                CustomPushEVSEStatusRequestParser);

            }
            catch (Exception e)
            {
                PushEVSEStatusRequest  = default;
                ErrorResponse          = "The given text representation of a push EVSE status request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPushEVSEStatusRequestSerializer = null, CustomOperatorEVSEStatusSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomPushEVSEStatusRequestSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomOperatorEVSEStatusSerializer">A delegate to serialize custom operator EVSE status JSON objects.</param>
        /// <param name="CustomEVSEStatusRecordSerializer">A delegate to serialize custom EVSE status record JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PushEVSEStatusRequest>?  CustomPushEVSEStatusRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<OperatorEVSEStatus>?     CustomOperatorEVSEStatusSerializer      = null,
                              CustomJObjectSerializerDelegate<EVSEStatusRecord>?       CustomEVSEStatusRecordSerializer        = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("ActionType",          Action.AsString()),

                           new JProperty("OperatorEvseStatus",  OperatorEVSEStatus.ToJSON(CustomOperatorEVSEStatusSerializer,
                                                                                          CustomEVSEStatusRecordSerializer)),

                           CustomData is not null
                               ? new JProperty("CustomData",    CustomData)
                               : null

                       );

            return CustomPushEVSEStatusRequestSerializer is not null
                       ? CustomPushEVSEStatusRequestSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this request.
        /// </summary>
        public PushEVSEStatusRequest Clone

            => new (OperatorEVSEStatus.Clone,
                    Action);

        #endregion


        #region Operator overloading

        #region Operator == (PushEVSEStatus1, PushEVSEStatus2)

        /// <summary>
        /// Compares two push EVSE status requests for equality.
        /// </summary>
        /// <param name="PushEVSEStatus1">An push EVSE status request.</param>
        /// <param name="PushEVSEStatus2">Another push EVSE status request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PushEVSEStatusRequest PushEVSEStatus1,
                                           PushEVSEStatusRequest PushEVSEStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PushEVSEStatus1, PushEVSEStatus2))
                return true;

            // If one is null, but not both, return false.
            if (PushEVSEStatus1 is null || PushEVSEStatus2 is null)
                return false;

            return PushEVSEStatus1.Equals(PushEVSEStatus2);

        }

        #endregion

        #region Operator != (PushEVSEStatus1, PushEVSEStatus2)

        /// <summary>
        /// Compares two push EVSE status requests for inequality.
        /// </summary>
        /// <param name="PushEVSEStatus1">An push EVSE status request.</param>
        /// <param name="PushEVSEStatus2">Another push EVSE status request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PushEVSEStatusRequest PushEVSEStatus1,
                                           PushEVSEStatusRequest PushEVSEStatus2)

            => !(PushEVSEStatus1 == PushEVSEStatus2);

        #endregion

        #endregion

        #region IEquatable<PushEVSEStatusRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is PushEVSEStatusRequest pushEVSEStatusRequest &&
                   Equals(pushEVSEStatusRequest);

        #endregion

        #region Equals(PushEVSEStatusRequest)

        /// <summary>
        /// Compares two push EVSE status requests for equality.
        /// </summary>
        /// <param name="PushEVSEStatusRequest">An push EVSE status request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PushEVSEStatusRequest? PushEVSEStatusRequest)

            => PushEVSEStatusRequest is not null &&

               OperatorEVSEStatus.Equals(PushEVSEStatusRequest.OperatorEVSEStatus) &&
               Action.            Equals(PushEVSEStatusRequest.Action);

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

                return OperatorEVSEStatus.GetHashCode() * 3 ^
                       Action.            GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Action, " of ",
                             EVSEStatusRecords.Count(), " EVSE status record(s)",
                             " by ", OperatorName, " (", OperatorId, ")");

        #endregion

    }

}
