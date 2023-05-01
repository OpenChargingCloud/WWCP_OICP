/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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
    /// The GetChargeDetailRecords request.
    /// </summary>
    public class GetChargeDetailRecordsRequest : APagedRequest<GetChargeDetailRecordsRequest>
    {

        #region Properties

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        [Mandatory]
        public Provider_Id                ProviderId       { get; }

        /// <summary>
        /// The start of the requested time range.
        /// </summary>
        [Mandatory]
        public DateTime                   From             { get; }

        /// <summary>
        /// The end of the requested time range.
        /// </summary>
        [Mandatory]
        public DateTime                   To               { get; }


        /// <summary>
        /// An optional enumeration of charging session identifications.
        /// </summary>
        [Optional]
        public IEnumerable<Session_Id>?   SessionIds       { get; }

        /// <summary>
        /// An optional enumeration of operator identifications.
        /// </summary>
        [Optional]
        public IEnumerable<Operator_Id>?  OperatorIds      { get; }

        /// <summary>
        /// Whether the CDR was successfuly forwarded to the EMP or not.
        /// </summary>
        [Optional]
        public Boolean?                   CDRForwarded     { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new GetChargeDetailRecords request.
        /// </summary>
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="From">The start of the requested time range.</param>
        /// <param name="To">The end of the requested time range.</param>
        /// 
        /// <param name="SessionIds">An optional enumeration of charging session identifications.</param>
        /// <param name="OperatorIds">An optional enumeration of operator identifications.</param>
        /// <param name="CDRForwarded">Whether the CDR was successfuly forwarded to the EMP or not.</param>
        /// 
        /// <param name="Page">An optional page number of the request page.</param>
        /// <param name="Size">An optional size of a request page.</param>
        /// <param name="SortOrder">Optional sorting criteria in the format: property(,asc|desc).</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public GetChargeDetailRecordsRequest(Provider_Id                ProviderId,
                                             DateTime                   From,
                                             DateTime                   To,
                                             IEnumerable<Session_Id>?   SessionIds          = null,
                                             IEnumerable<Operator_Id>?  OperatorIds         = null,
                                             Boolean?                   CDRForwarded        = null,

                                             Process_Id?                ProcessId           = null,
                                             UInt32?                    Page                = null,
                                             UInt32?                    Size                = null,
                                             IEnumerable<String>?       SortOrder           = null,
                                             JObject?                   CustomData          = null,

                                             DateTime?                  Timestamp           = null,
                                             CancellationToken          CancellationToken   = default,
                                             EventTracking_Id?          EventTrackingId     = null,
                                             TimeSpan?                  RequestTimeout      = null)

            : base(ProcessId,
                   Page,
                   Size,
                   SortOrder,
                   CustomData,

                   Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.ProviderId    = ProviderId;
            this.From          = From;
            this.To            = To;

            this.SessionIds    = SessionIds;
            this.OperatorIds   = OperatorIds;
            this.CDRForwarded  = CDRForwarded;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#eRoamingGetChargeDetailRecordsmessage

        // {
        //   "ProviderID":    "string",
        //   "From":          "2021-04-23T02:18:52.478Z",
        //   "To":            "2021-04-23T02:18:52.478Z",
        //   "SessionID":     [ "string" ],
        //   "OperatorID":    [ "string" ],
        //   "CDRForwarded":  false
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomGetChargeDetailRecordsRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a GetChargeDetailRecords request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="Page">An optional page number of the request page.</param>
        /// <param name="Size">An optional size of a request page.</param>
        /// <param name="SortOrder">Optional sorting criteria in the format: property(,asc|desc).</param>
        /// <param name="CustomGetChargeDetailRecordsRequestParser">A delegate to parse custom GetChargeDetailRecords request JSON objects.</param>
        public static GetChargeDetailRecordsRequest Parse(JObject                                                      JSON,
                                                          Process_Id?                                                  ProcessId                                   = null,
                                                          UInt32?                                                      Page                                        = null,
                                                          UInt32?                                                      Size                                        = null,
                                                          IEnumerable<String>?                                         SortOrder                                   = null,

                                                          DateTime?                                                    Timestamp                                   = null,
                                                          CancellationToken                                            CancellationToken                           = default,
                                                          EventTracking_Id?                                            EventTrackingId                             = null,
                                                          TimeSpan?                                                    RequestTimeout                              = null,

                                                          CustomJObjectParserDelegate<GetChargeDetailRecordsRequest>?  CustomGetChargeDetailRecordsRequestParser   = null)
        {

            if (TryParse(JSON,
                         out var getChargeDetailRecordsRequest,
                         out var errorResponse,
                         ProcessId,
                         Page,
                         Size,
                         SortOrder,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout,
                         CustomGetChargeDetailRecordsRequestParser))
            {
                return getChargeDetailRecordsRequest!;
            }

            throw new ArgumentException("The given JSON representation of a GetChargeDetailRecords request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out GetChargeDetailRecordsRequest, out ErrorResponse, ..., CustomGetChargeDetailRecordsRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a GetChargeDetailRecords request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="GetChargeDetailRecordsRequest">The parsed GetChargeDetailRecords request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Page">An optional page number of the request page.</param>
        /// <param name="Size">An optional size of a request page.</param>
        /// <param name="SortOrder">Optional sorting criteria in the format: property(,asc|desc).</param>
        /// <param name="CustomGetChargeDetailRecordsRequestParser">A delegate to parse custom GetChargeDetailRecords request JSON objects.</param>
        public static Boolean TryParse(JObject                                                      JSON,
                                       out GetChargeDetailRecordsRequest?                           GetChargeDetailRecordsRequest,
                                       out String?                                                  ErrorResponse,
                                       Process_Id?                                                  ProcessId                                   = null,
                                       UInt32?                                                      Page                                        = null,
                                       UInt32?                                                      Size                                        = null,
                                       IEnumerable<String>?                                         SortOrder                                   = null,

                                       DateTime?                                                    Timestamp                                   = null,
                                       CancellationToken                                            CancellationToken                           = default,
                                       EventTracking_Id?                                            EventTrackingId                             = null,
                                       TimeSpan?                                                    RequestTimeout                              = null,

                                       CustomJObjectParserDelegate<GetChargeDetailRecordsRequest>?  CustomGetChargeDetailRecordsRequestParser   = null)
        {

            try
            {

                GetChargeDetailRecordsRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse ProviderID      [mandatory]

                if (!JSON.ParseMandatory("ProviderID",
                                         "provider identification",
                                         Provider_Id.TryParse,
                                         out Provider_Id ProviderId,
                                         out             ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse From            [mandatory]

                if (!JSON.ParseMandatory("From",
                                         "from timestamp",
                                         out DateTime From,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse To              [mandatory]

                if (!JSON.ParseMandatory("To",
                                         "to timestamp",
                                         out DateTime To,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse SessionID       [optional]

                if (JSON.ParseOptionalHashSet("SessionID",
                                              "charging session identifications",
                                              Session_Id.TryParse,
                                              out HashSet<Session_Id> SessionIds,
                                              out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse OperatorID      [optional]

                if (JSON.ParseOptionalHashSet("OperatorID",
                                              "charging operator identifications",
                                              Operator_Id.TryParse,
                                              out HashSet<Operator_Id> OperatorIds,
                                              out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CDRForwarded    [optional]

                if (JSON.ParseOptional("CDRForwarded",
                                       "CDR was forwarded",
                                       out Boolean? CDRForwarded,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CustomData      [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                GetChargeDetailRecordsRequest = new GetChargeDetailRecordsRequest(ProviderId,
                                                                                  From,
                                                                                  To,
                                                                                  SessionIds,
                                                                                  OperatorIds,
                                                                                  CDRForwarded,

                                                                                  ProcessId,
                                                                                  Page,
                                                                                  Size,
                                                                                  SortOrder,

                                                                                  customData,

                                                                                  Timestamp,
                                                                                  CancellationToken,
                                                                                  EventTrackingId,
                                                                                  RequestTimeout);

                if (CustomGetChargeDetailRecordsRequestParser is not null)
                    GetChargeDetailRecordsRequest = CustomGetChargeDetailRecordsRequestParser(JSON,
                                                                                              GetChargeDetailRecordsRequest);

                return true;

            }
            catch (Exception e)
            {
                GetChargeDetailRecordsRequest  = default;
                ErrorResponse                  = "The given JSON representation of a GetChargeDetailRecords request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomGetChargeDetailRecordsRequestSerializer = null)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomGetChargeDetailRecordsRequestSerializer">A delegate to customize the serialization of GetChargeDetailRecordsRequest responses.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<GetChargeDetailRecordsRequest>?  CustomGetChargeDetailRecordsRequestSerializer   = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("ProviderID",         ProviderId.ToString()),
                           new JProperty("From",               From.      ToIso8601()),
                           new JProperty("To",                 To.        ToIso8601()),

                           SessionIds  is not null && SessionIds .Any()
                               ? new JProperty("SessionID",    new JArray(SessionIds. Select(sessionId  => sessionId. ToString())))
                               : null,

                           OperatorIds is not null && OperatorIds.Any()
                               ? new JProperty("OperatorID",   new JArray(OperatorIds.Select(operatorId => operatorId.ToString())))
                               : null,

                           CustomData is not null
                               ? new JProperty("CustomData",   CustomData)
                               : null

                       );

            return CustomGetChargeDetailRecordsRequestSerializer is not null
                       ? CustomGetChargeDetailRecordsRequestSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (GetChargeDetailRecords1, GetChargeDetailRecords2)

        /// <summary>
        /// Compares two pull EVSE status requests for equality.
        /// </summary>
        /// <param name="GetChargeDetailRecords1">An pull EVSE status request.</param>
        /// <param name="GetChargeDetailRecords2">Another pull EVSE status request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetChargeDetailRecordsRequest GetChargeDetailRecords1,
                                           GetChargeDetailRecordsRequest GetChargeDetailRecords2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GetChargeDetailRecords1, GetChargeDetailRecords2))
                return true;

            // If one is null, but not both, return false.
            if (GetChargeDetailRecords1 is null || GetChargeDetailRecords2 is null)
                return false;

            return GetChargeDetailRecords1.Equals(GetChargeDetailRecords2);

        }

        #endregion

        #region Operator != (GetChargeDetailRecords1, GetChargeDetailRecords2)

        /// <summary>
        /// Compares two pull EVSE status requests for inequality.
        /// </summary>
        /// <param name="GetChargeDetailRecords1">An pull EVSE status request.</param>
        /// <param name="GetChargeDetailRecords2">Another pull EVSE status request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetChargeDetailRecordsRequest GetChargeDetailRecords1,
                                           GetChargeDetailRecordsRequest GetChargeDetailRecords2)

            => !(GetChargeDetailRecords1 == GetChargeDetailRecords2);

        #endregion

        #endregion

        #region IEquatable<GetChargeDetailRecordsRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is GetChargeDetailRecordsRequest getChargeDetailRecordsRequest &&
                   Equals(getChargeDetailRecordsRequest);

        #endregion

        #region Equals(GetChargeDetailRecordsRequest)

        /// <summary>
        /// Compares two pull EVSE status requests for equality.
        /// </summary>
        /// <param name="GetChargeDetailRecordsRequest">A pull EVSE status request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetChargeDetailRecordsRequest? GetChargeDetailRecordsRequest)

            => GetChargeDetailRecordsRequest is not null &&

               ProviderId.      Equals(GetChargeDetailRecordsRequest.ProviderId) &&
               From.            Equals(GetChargeDetailRecordsRequest.From)       &&
               To.              Equals(GetChargeDetailRecordsRequest.To)         &&

            ((!CDRForwarded.HasValue && !GetChargeDetailRecordsRequest.CDRForwarded.HasValue) ||
              (CDRForwarded.HasValue &&  GetChargeDetailRecordsRequest.CDRForwarded.HasValue && CDRForwarded.Value.Equals(GetChargeDetailRecordsRequest.CDRForwarded.Value)));

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

                return  ProviderId.   GetHashCode() * 13 ^
                        From.         GetHashCode() * 11 ^
                        To.           GetHashCode() *  7 ^

                       (CDRForwarded?.GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(
                   ProviderId.ToString(),
                   ", " + From.ToIso8601(), " <> ", To.ToIso8601(),
                   CDRForwarded == true
                       ? ", only forwarded"
                       : "");

        #endregion

    }

}
