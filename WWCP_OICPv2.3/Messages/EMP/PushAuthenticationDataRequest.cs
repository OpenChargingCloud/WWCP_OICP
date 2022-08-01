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
    /// The PushAuthenticationData request.
    /// </summary>
    public class PushAuthenticationDataRequest : ARequest<PushAuthenticationDataRequest>
    {

        #region Properties

        /// <summary>
        /// The provider authentication data record.
        /// </summary>
        [Mandatory]
        public ProviderAuthenticationData   ProviderAuthenticationData    { get; }

        /// <summary>
        /// The server-side data management operation.
        /// </summary>
        [Mandatory]
        public ActionTypes                  Action                        { get; }

        /// <summary>
        /// The enumeration of EVSE data records.
        /// </summary>
        public IEnumerable<Identification>  Identifications
            => ProviderAuthenticationData.Identifications;

        /// <summary>
        /// The unqiue identification of the e-mobility provider maintaining the given user identification data records.</param>
        /// </summary>
        public Provider_Id                  ProviderId
            => ProviderAuthenticationData.ProviderId;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PushAuthenticationData request.
        /// </summary>
        /// <param name="ProviderAuthenticationData">The provider authentication data record.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public PushAuthenticationDataRequest(ProviderAuthenticationData  ProviderAuthenticationData,
                                             ActionTypes                 Action              = ActionTypes.FullLoad,
                                             Process_Id?                 ProcessId           = null,
                                             JObject?                    CustomData          = null,

                                             DateTime?                   Timestamp           = null,
                                             CancellationToken?          CancellationToken   = null,
                                             EventTracking_Id?           EventTrackingId     = null,
                                             TimeSpan?                   RequestTimeout      = null)

            : base(ProcessId,
                   CustomData,
                   Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.ProviderAuthenticationData  = ProviderAuthenticationData ?? throw new ArgumentNullException(nameof(ProviderAuthenticationData), "The given provider authentication data must not be null!");
            this.Action                      = Action;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#311-eroamingpushauthenticationdata-message

        // {
        //   "ActionType":  "fullLoad",
        //   "ProviderAuthenticationData": {
        //     "AuthenticationDataRecord": [
        //       {
        //         "Identification": {
        //           ...
        //         }
        //       }
        //     ],
        //     "ProviderID": "DE-GDF"
        //   }
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomPushAuthenticationDataRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PushAuthenticationData request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomPushAuthenticationDataRequestParser">A delegate to parse custom PushAuthenticationData JSON objects.</param>
        public static PushAuthenticationDataRequest Parse(JObject                                                      JSON,
                                                          Process_Id?                                                  ProcessId                                   = null,

                                                          DateTime?                                                    Timestamp                                   = null,
                                                          CancellationToken?                                           CancellationToken                           = null,
                                                          EventTracking_Id?                                            EventTrackingId                             = null,
                                                          TimeSpan?                                                    RequestTimeout                              = null,

                                                          CustomJObjectParserDelegate<PushAuthenticationDataRequest>?  CustomPushAuthenticationDataRequestParser   = null)
        {

            if (TryParse(JSON,
                         out PushAuthenticationDataRequest?  pullEVSEStatusResponse,
                         out String?                         errorResponse,
                         ProcessId,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout,
                         CustomPushAuthenticationDataRequestParser))
            {
                return pullEVSEStatusResponse!;
            }

            throw new ArgumentException("The given JSON representation of a PushAuthenticationData request is invalid: " + errorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, ..., CustomPushAuthenticationDataRequestParser = null)

        /// <summary>
        /// Parse the given text representation of a PushAuthenticationData request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomPushAuthenticationDataRequestParser">A delegate to parse custom PushAuthenticationData request JSON objects.</param>
        public static PushAuthenticationDataRequest Parse(String                                                       Text,
                                                          Process_Id?                                                  ProcessId                                   = null,

                                                          DateTime?                                                    Timestamp                                   = null,
                                                          CancellationToken?                                           CancellationToken                           = null,
                                                          EventTracking_Id?                                            EventTrackingId                             = null,
                                                          TimeSpan?                                                    RequestTimeout                              = null,

                                                          CustomJObjectParserDelegate<PushAuthenticationDataRequest>?  CustomPushAuthenticationDataRequestParser   = null)
        {

            if (TryParse(Text,
                         out PushAuthenticationDataRequest?  pullEVSEStatusResponse,
                         out String?                         errorResponse,
                         ProcessId,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout,
                         CustomPushAuthenticationDataRequestParser))
            {
                return pullEVSEStatusResponse!;
            }

            throw new ArgumentException("The given text representation of a PushAuthenticationData request is invalid: " + errorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out PushAuthenticationDataRequest, out ErrorResponse, ..., CustomPushAuthenticationDataRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PushAuthenticationData request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="PushAuthenticationDataRequest">The parsed PushAuthenticationData request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPushAuthenticationDataRequestParser">A delegate to parse custom PushAuthenticationData request JSON objects.</param>
        public static Boolean TryParse(JObject                                                      JSON,
                                       out PushAuthenticationDataRequest?                           PushAuthenticationDataRequest,
                                       out String?                                                  ErrorResponse,
                                       Process_Id?                                                  ProcessId                                   = null,

                                       DateTime?                                                    Timestamp                                   = null,
                                       CancellationToken?                                           CancellationToken                           = null,
                                       EventTracking_Id?                                            EventTrackingId                             = null,
                                       TimeSpan?                                                    RequestTimeout                              = null,

                                       CustomJObjectParserDelegate<PushAuthenticationDataRequest>?  CustomPushAuthenticationDataRequestParser   = null)
        {

            try
            {

                PushAuthenticationDataRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse ActionType                    [mandatory]

                if (!JSON.ParseMandatoryEnum("ActionType",
                                             "action type",
                                             out ActionTypes ActionType,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ProviderAuthenticationData    [mandatory => optional, because of Hubject data quality issues!]

                if (!JSON.ParseMandatory("ProviderAuthenticationData",
                                         "provider authentication data",
                                         OICPv2_3.ProviderAuthenticationData.TryParse,
                                         out ProviderAuthenticationData ProviderAuthenticationData,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse CustomData                    [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                PushAuthenticationDataRequest = new PushAuthenticationDataRequest(ProviderAuthenticationData,
                                                                                  ActionType,
                                                                                  ProcessId,
                                                                                  CustomData,
                                                                                  Timestamp,
                                                                                  CancellationToken,
                                                                                  EventTrackingId,
                                                                                  RequestTimeout);

                if (CustomPushAuthenticationDataRequestParser is not null)
                    PushAuthenticationDataRequest = CustomPushAuthenticationDataRequestParser(JSON,
                                                                                              PushAuthenticationDataRequest);

                return true;

            }
            catch (Exception e)
            {
                PushAuthenticationDataRequest  = default;
                ErrorResponse                  = "The given JSON representation of a PushAuthenticationData request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out PushAuthenticationDataRequest, out ErrorResponse, ..., CustomPushAuthenticationDataRequestParser = null)

        /// <summary>
        /// Try to parse the given text representation of a PushAuthenticationData request.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="PushAuthenticationDataRequest">The parsed PushAuthenticationData request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPushAuthenticationDataRequestParser">A delegate to parse custom PushAuthenticationData request JSON objects.</param>
        public static Boolean TryParse(String                                                       Text,
                                       out PushAuthenticationDataRequest?                           PushAuthenticationDataRequest,
                                       out String?                                                  ErrorResponse,
                                       Process_Id?                                                  ProcessId                                   = null,

                                       DateTime?                                                    Timestamp                                   = null,
                                       CancellationToken?                                           CancellationToken                           = null,
                                       EventTracking_Id?                                            EventTrackingId                             = null,
                                       TimeSpan?                                                    RequestTimeout                              = null,

                                       CustomJObjectParserDelegate<PushAuthenticationDataRequest>?  CustomPushAuthenticationDataRequestParser   = null)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out PushAuthenticationDataRequest,
                                out ErrorResponse,
                                ProcessId,
                                Timestamp,
                                CancellationToken,
                                EventTrackingId,
                                RequestTimeout,
                                CustomPushAuthenticationDataRequestParser);

            }
            catch (Exception e)
            {
                PushAuthenticationDataRequest  = default;
                ErrorResponse                  = "The given text representation of a PushAuthenticationData request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPushAuthenticationDataRequestSerializer = null, CustomProviderAuthenticationDataSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPushAuthenticationDataRequestSerializer">A delegate to customize the serialization of PushAuthenticationDataRequest responses.</param>
        /// <param name="CustomProviderAuthenticationDataSerializer">A delegate to serialize custom provider user identification data JSON objects.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PushAuthenticationDataRequest>?  CustomPushAuthenticationDataRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<ProviderAuthenticationData>?     CustomProviderAuthenticationDataSerializer      = null,
                              CustomJObjectSerializerDelegate<Identification>?                 CustomIdentificationSerializer                  = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("ActionType",                  Action.AsString()),

                           new JProperty("ProviderAuthenticationData",  ProviderAuthenticationData.ToJSON(CustomProviderAuthenticationDataSerializer,
                                                                                                          CustomIdentificationSerializer)),

                           CustomData is not null
                               ? new JProperty("CustomData",            CustomData)
                               : null

                       );

            return CustomPushAuthenticationDataRequestSerializer is not null
                       ? CustomPushAuthenticationDataRequestSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PushAuthenticationData1, PushAuthenticationData2)

        /// <summary>
        /// Compares two pull EVSE status requests for equality.
        /// </summary>
        /// <param name="PushAuthenticationData1">An pull EVSE status request.</param>
        /// <param name="PushAuthenticationData2">Another pull EVSE status request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PushAuthenticationDataRequest PushAuthenticationData1,
                                           PushAuthenticationDataRequest PushAuthenticationData2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PushAuthenticationData1, PushAuthenticationData2))
                return true;

            // If one is null, but not both, return false.
            if (PushAuthenticationData1 is null || PushAuthenticationData2 is null)
                return false;

            return PushAuthenticationData1.Equals(PushAuthenticationData2);

        }

        #endregion

        #region Operator != (PushAuthenticationData1, PushAuthenticationData2)

        /// <summary>
        /// Compares two pull EVSE status requests for inequality.
        /// </summary>
        /// <param name="PushAuthenticationData1">An pull EVSE status request.</param>
        /// <param name="PushAuthenticationData2">Another pull EVSE status request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PushAuthenticationDataRequest PushAuthenticationData1,
                                           PushAuthenticationDataRequest PushAuthenticationData2)

            => !(PushAuthenticationData1 == PushAuthenticationData2);

        #endregion

        #endregion

        #region IEquatable<PushAuthenticationDataRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is PushAuthenticationDataRequest pullEVSEStatusRequest &&
                   Equals(pullEVSEStatusRequest);

        #endregion

        #region Equals(PushAuthenticationDataRequest)

        /// <summary>
        /// Compares two pull EVSE status requests for equality.
        /// </summary>
        /// <param name="PushAuthenticationDataRequest">A pull EVSE status request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PushAuthenticationDataRequest? PushAuthenticationDataRequest)

            => PushAuthenticationDataRequest is not null &&

               ProviderAuthenticationData.Equals(PushAuthenticationDataRequest.ProviderAuthenticationData) &&
               Action.Equals(PushAuthenticationDataRequest.Action);

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

                return ProviderAuthenticationData.GetHashCode() * 3 ^
                       Action.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Action, " of ",
                             Identifications.Count(), " provider authentication data record(s)",
                             " by ", ProviderId);

        #endregion

    }

}
