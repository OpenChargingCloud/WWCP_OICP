﻿/*
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
    /// The PullEVSEData request.
    /// </summary>
    public class PullEVSEDataRequest : APagedRequest<PullEVSEDataRequest>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the e-mobility provider.
        /// </summary>
        [Mandatory]
        public Provider_Id                                     ProviderId                              { get; }

        /// <summary>
        /// The optional timestamp of the last call.
        /// </summary>
        [Optional]
        public DateTime?                                       LastCall                                { get; }


        /// <summary>
        /// Only return EVSEs belonging to the given optional enumeration of EVSE operators.
        /// </summary>
        [Optional]
        public IEnumerable<Operator_Id>?                       OperatorIdFilter                        { get; }

        /// <summary>
        /// An optional enumeration of countries whose EVSE's a provider wants to retrieve.
        /// </summary>
        [Optional]
        public IEnumerable<Country>?                           CountryCodeFilter                       { get; }

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public IEnumerable<AccessibilityTypes>?                AccessibilityFilter                     { get; }

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public IEnumerable<AuthenticationModes>?               AuthenticationModeFilter                { get; }

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public IEnumerable<CalibrationLawDataAvailabilities>?  CalibrationLawDataAvailabilityFilter    { get; }


        [Optional]
        public Boolean?                                        RenewableEnergyFilter                   { get; }

        [Optional]
        public Boolean?                                        IsHubjectCompatibleFilter               { get; }

        [Optional]
        public Boolean?                                        IsOpen24HoursFilter                     { get; }



        /// <summary>
        /// The optional geo coordinate of the search center.
        /// </summary>
        [Optional]
        public GeoCoordinates?                                 SearchCenter                            { get; }

        /// <summary>
        /// The optional search distance relative to the search center.
        /// </summary>
        [Optional]
        public Single?                                         DistanceKM                              { get; }

        /// <summary>
        /// The optional response format for representing geo coordinates.
        /// </summary>
        [Optional]
        public GeoCoordinatesFormats                           GeoCoordinatesResponseFormat            { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullEVSEData request.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="LastCall">An optional timestamp of the last call. Cannot be combined with 'SearchCenter'.</param>
        /// 
        /// <param name="GeoCoordinatesResponseFormat">An optional response format for representing geo coordinates.</param>
        /// <param name="OperatorIdFilter">Only return EVSEs belonging to the given optional enumeration of EVSE operators.</param>
        /// <param name="CountryCodeFilter">An optional enumeration of countries whose EVSE's a provider wants to retrieve.</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// 
        /// <param name="ProcessId">The optional unique OICP process identification.</param>
        /// <param name="Page">An optional page number of the request page.</param>
        /// <param name="Size">An optional size of a request page.</param>
        /// <param name="SortOrder">Optional sorting criteria in the format: property(,asc|desc).</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public PullEVSEDataRequest(Provider_Id                                     ProviderId,
                                   DateTime?                                       LastCall                               = null,

                                   IEnumerable<Operator_Id>?                       OperatorIdFilter                       = null,
                                   IEnumerable<Country>?                           CountryCodeFilter                      = null,
                                   IEnumerable<AccessibilityTypes>?                AccessibilityFilter                    = null,
                                   IEnumerable<AuthenticationModes>?               AuthenticationModeFilter               = null,
                                   IEnumerable<CalibrationLawDataAvailabilities>?  CalibrationLawDataAvailabilityFilter   = null,
                                   Boolean?                                        RenewableEnergyFilter                  = null,
                                   Boolean?                                        IsHubjectCompatibleFilter              = null,
                                   Boolean?                                        IsOpen24HoursFilter                    = null,

                                   GeoCoordinates?                                 SearchCenter                           = null,
                                   Single?                                         DistanceKM                             = null,
                                   GeoCoordinatesFormats?                          GeoCoordinatesResponseFormat           = GeoCoordinatesFormats.DecimalDegree,

                                   Process_Id?                                     ProcessId                              = null,
                                   UInt32?                                         Page                                   = null,
                                   UInt32?                                         Size                                   = null,
                                   IEnumerable<String>?                            SortOrder                              = null,
                                   JObject?                                        CustomData                             = null,

                                   DateTime?                                       Timestamp                              = null,
                                   EventTracking_Id?                               EventTrackingId                        = null,
                                   TimeSpan?                                       RequestTimeout                         = null,
                                   CancellationToken                               CancellationToken                      = default)

            : base(ProcessId,
                   Page,
                   Size,
                   SortOrder,
                   CustomData,

                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.ProviderId                            = ProviderId;
            this.LastCall                              = LastCall;

            this.OperatorIdFilter                      = OperatorIdFilter?.                    Distinct() ?? [];
            this.CountryCodeFilter                     = CountryCodeFilter?.                   Distinct() ?? [];
            this.AccessibilityFilter                   = AccessibilityFilter?.                 Distinct() ?? [];
            this.AuthenticationModeFilter              = AuthenticationModeFilter?.            Distinct() ?? [];
            this.CalibrationLawDataAvailabilityFilter  = CalibrationLawDataAvailabilityFilter?.Distinct() ?? [];
            this.RenewableEnergyFilter                 = RenewableEnergyFilter;
            this.IsHubjectCompatibleFilter             = IsHubjectCompatibleFilter;
            this.IsOpen24HoursFilter                   = IsOpen24HoursFilter;

            this.SearchCenter                          = SearchCenter;
            this.DistanceKM                            = DistanceKM;
            this.GeoCoordinatesResponseFormat          = GeoCoordinatesResponseFormat                     ?? GeoCoordinatesFormats.DecimalDegree;


            unchecked
            {

                hashCode = this.ProviderId.                  GetHashCode()       * 11 ^
                           this.DistanceKM.                  GetHashCode()       *  7 ^
                           this.GeoCoordinatesResponseFormat.GetHashCode()       *  5 ^
                          (this.SearchCenter?.               GetHashCode() ?? 0) *  3 ^
                           this.LastCall?.                   GetHashCode() ?? 0;

            }

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20EMP/02_EMP_Services_and_Operations.asciidoc#eRoamingPullEvseData

        // {
        //   "ProviderID":                    "string",
        //   "LastCall":                      "2021-01-09T09:16:26.888Z",
        //   "OperatorIds": [
        //     "string"
        //   ],
        //   "CountryCodes": [
        //     "string"
        //   ],
        //   "Accessibility": [
        //     "Unspecified"
        //   ],
        //   "AuthenticationModes": [
        //     "NFC RFID Classic"
        //   ],
        //   "CalibrationLawDataAvailability": [
        //     "Local"
        //   ],
        //   "IsHubjectCompatible":            false,
        //   "IsOpen24Hours":                  false,
        //   "RenewableEnergy":                false,
        //   "SearchCenter": {
        //     "GeoCoordinates": {
        //       "Google": {
        //         "Coordinates":             "string"
        //       },
        //       "DecimalDegree": {
        //         "Latitude":                "string",
        //         "Longitude":               "string"
        //       },
        //       "DegreeMinuteSeconds": {
        //         "Latitude":                "string",
        //         "Longitude":               "string"
        //       }
        //     },
        //     "Radius":                       0
        //   },
        //   "GeoCoordinatesResponseFormat":  "Google"
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomPullEVSEDataRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a PullEVSEData request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomPullEVSEDataRequestParser">A delegate to parse custom PullEVSEData JSON objects.</param>
        public static PullEVSEDataRequest Parse(JObject                                            JSON,
                                                Process_Id?                                        ProcessId                         = null,
                                                UInt32?                                            Page                              = null,
                                                UInt32?                                            Size                              = null,
                                                IEnumerable<String>?                               SortOrder                         = null,

                                                DateTime?                                          Timestamp                         = null,
                                                EventTracking_Id?                                  EventTrackingId                   = null,
                                                TimeSpan?                                          RequestTimeout                    = null,
                                                CustomJObjectParserDelegate<PullEVSEDataRequest>?  CustomPullEVSEDataRequestParser   = null,
                                                CancellationToken                                  CancellationToken                 = default)
        {

            if (TryParse(JSON,
                         out var pullEVSEDataRequest,
                         out var errorResponse,
                         ProcessId,
                         Page,
                         Size,
                         SortOrder,
                         Timestamp,
                         EventTrackingId,
                         RequestTimeout,
                         CustomPullEVSEDataRequestParser,
                         CancellationToken))
            {
                return pullEVSEDataRequest;
            }

            throw new ArgumentException("The given JSON representation of a PullEVSEData request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out PullEVSEDataRequest, out ErrorResponse, ..., CustomPullEVSEDataRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a PullEVSEData request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="PullEVSEDataRequest">The parsed PullEVSEData request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPullEVSEDataRequestParser">A delegate to parse custom PullEVSEData request JSON objects.</param>
        public static Boolean TryParse(JObject                                            JSON,
                                       [NotNullWhen(true)]  out PullEVSEDataRequest?      PullEVSEDataRequest,
                                       [NotNullWhen(false)] out String?                   ErrorResponse,
                                       Process_Id?                                        ProcessId                         = null,
                                       UInt32?                                            Page                              = null,
                                       UInt32?                                            Size                              = null,
                                       IEnumerable<String>?                               SortOrder                         = null,

                                       DateTime?                                          Timestamp                         = null,
                                       EventTracking_Id?                                  EventTrackingId                   = null,
                                       TimeSpan?                                          RequestTimeout                    = null,
                                       CustomJObjectParserDelegate<PullEVSEDataRequest>?  CustomPullEVSEDataRequestParser   = null,
                                       CancellationToken                                  CancellationToken                 = default)
        {

            try
            {

                PullEVSEDataRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse ProviderId                                [mandatory]

                if (!JSON.ParseMandatory("ProviderID",
                                         "provider identification",
                                         Provider_Id.TryParse,
                                         out Provider_Id ProviderId,
                                         out             ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse LastCall                                  [optional]

                if (JSON.ParseOptional("LastCall",
                                       "last call",
                                       out DateTime? LastCall,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region Parse OperatorIdFilter                          [optional]

                if (JSON.ParseOptionalJSON("OperatorIds",
                                           "operator identification filter",
                                           Operator_Id.TryParse,
                                           out IEnumerable<Operator_Id> OperatorIdFilter,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CountryCodeFilter                         [optional]

                if (JSON.ParseOptionalJSON("CountryCodes",
                                           "country code filter",
                                           Country.TryParse,
                                           out IEnumerable<Country> CountryCodeFilter,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse AccessibilityFilter                       [optional]

                if (JSON.ParseOptionalJSON("Accessibility",
                                           "accessibility filter",
                                           AccessibilityTypesExtensions.TryParse,
                                           out IEnumerable<AccessibilityTypes> AccessibilityFilter,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse AuthenticationModeFilter                  [optional]

                if (JSON.ParseOptionalJSON("AuthenticationModes",
                                           "authentication mode filter",
                                           AuthenticationModesExtensions.TryParse,
                                           out IEnumerable<AuthenticationModes> AuthenticationModeFilter,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CalibrationLawDataAvailabilityFilter      [optional]

                if (JSON.ParseOptionalJSON("CalibrationLawDataAvailability",
                                           "calibration law data availability filter",
                                           CalibrationLawDataAvailabilitiesExtensions.TryParse,
                                           out IEnumerable<CalibrationLawDataAvailabilities> CalibrationLawDataAvailabilityFilter,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse RenewableEnergyFilter                     [optional]

                if (JSON.ParseOptional("RenewableEnergy",
                                       "renewable energy filter",
                                       out Boolean? RenewableEnergyFilter,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse IsHubjectCompatibleFilter                 [optional]

                if (JSON.ParseOptional("IsHubjectCompatible",
                                       "is hubject compatible filter",
                                       out Boolean? IsHubjectCompatibleFilter,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse IsOpen24Hours                             [optional]

                if (JSON.ParseOptional("IsOpen24Hours",
                                       "is open 24 hours filter",
                                       out Boolean? IsOpen24HoursFilter,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region Parse SearchCenter                              [optional]

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

                #region Parse DistanceKM                                [optional]

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

                #region Parse OperatorIdFilter                          [optional]

                if (JSON.ParseOptional("GeoCoordinatesResponseFormat",
                                       "geo coordinates response format",
                                       GeoCoordinatesFormatsExtensions.TryParse,
                                       out GeoCoordinatesFormats GeoCoordinatesResponseFormat,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region Parse CustomData                                [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                PullEVSEDataRequest = new PullEVSEDataRequest(

                                          ProviderId,
                                          LastCall,

                                          OperatorIdFilter,
                                          CountryCodeFilter,
                                          AccessibilityFilter,
                                          AuthenticationModeFilter,
                                          CalibrationLawDataAvailabilityFilter,
                                          RenewableEnergyFilter,
                                          IsHubjectCompatibleFilter,
                                          IsOpen24HoursFilter,

                                          SearchCenter,
                                          DistanceKM,
                                          GeoCoordinatesResponseFormat,

                                          ProcessId,
                                          Page,
                                          Size,
                                          SortOrder,

                                          customData,

                                          Timestamp,
                                          EventTrackingId,
                                          RequestTimeout,
                                          CancellationToken

                                      );

                if (CustomPullEVSEDataRequestParser is not null)
                    PullEVSEDataRequest = CustomPullEVSEDataRequestParser(JSON,
                                                                          PullEVSEDataRequest);

                return true;

            }
            catch (Exception e)
            {
                PullEVSEDataRequest  = default;
                ErrorResponse        = "The given JSON representation of a PullEVSEData request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPullEVSEDataRequestSerializer = null, CustomGeoCoordinatesSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEDataRequestSerializer">A delegate to customize the serialization of PullEVSEDataRequest responses.</param>
        /// <param name="CustomGeoCoordinatesSerializer">A delegate to serialize custom geo coordinates JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PullEVSEDataRequest>?  CustomPullEVSEDataRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<GeoCoordinates>?       CustomGeoCoordinatesSerializer        = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("ProviderID",                       ProviderId.                  ToString()),
                                 new JProperty("GeoCoordinatesResponseFormat",     GeoCoordinatesResponseFormat.AsString()),

                           LastCall.HasValue
                               ? new JProperty("LastCall",                         LastCall.              Value.ToISO8601())
                               : null,

                           OperatorIdFilter                     is not null && OperatorIdFilter.Any()
                               ? new JProperty("OperatorIds",                      new JArray(OperatorIdFilter.                    Select(operatorId                     => operatorId.                    ToString())))
                               : null,

                           CountryCodeFilter                    is not null && CountryCodeFilter.Any()
                               ? new JProperty("CountryCodes",                     new JArray(CountryCodeFilter.                   Select(countryCode                    => countryCode.                   Alpha3Code)))
                               : null,

                           AccessibilityFilter                  is not null && AccessibilityFilter.Any()
                               ? new JProperty("Accessibility",                    new JArray(AccessibilityFilter.                 Select(accessibility                  => accessibility.                 AsString())))
                               : null,

                           AuthenticationModeFilter             is not null && AuthenticationModeFilter.Any()
                               ? new JProperty("AuthenticationModes",              new JArray(AuthenticationModeFilter.            Select(authenticationMode             => authenticationMode.            AsString())))
                               : null,

                           CalibrationLawDataAvailabilityFilter is not null && CalibrationLawDataAvailabilityFilter.Any()
                               ? new JProperty("CalibrationLawDataAvailability",   new JArray(CalibrationLawDataAvailabilityFilter.Select(calibrationLawDataAvailability => calibrationLawDataAvailability.AsString())))
                               : null,

                           IsHubjectCompatibleFilter.HasValue
                               ? new JProperty("IsHubjectCompatible",              IsHubjectCompatibleFilter.Value)
                               : null,

                           IsOpen24HoursFilter.HasValue
                               ? new JProperty("IsOpen24Hours",                    IsOpen24HoursFilter.      Value)
                               : null,

                           RenewableEnergyFilter.HasValue
                               ? new JProperty("RenewableEnergy",                  RenewableEnergyFilter.    Value)
                               : null,

                           SearchCenter.HasValue && DistanceKM.HasValue
                               ? new JProperty("SearchCenter",                     new JObject(
                                                                                       new JProperty("GeoCoordinates",  SearchCenter.Value.ToJSON(CustomGeoCoordinatesSerializer)),
                                                                                       new JProperty("Radius",          DistanceKM.Value)
                                                                                   ))
                               : null,

                           CustomData is not null
                               ? new JProperty("CustomData",                       CustomData)
                               : null

                       );

            return CustomPullEVSEDataRequestSerializer is not null
                       ? CustomPullEVSEDataRequestSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullEVSEData1, PullEVSEData2)

        /// <summary>
        /// Compares two pull EVSE data requests for equality.
        /// </summary>
        /// <param name="PullEVSEData1">An pull EVSE data request.</param>
        /// <param name="PullEVSEData2">Another pull EVSE data request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullEVSEDataRequest PullEVSEData1,
                                           PullEVSEDataRequest PullEVSEData2)
        {

            if (ReferenceEquals(PullEVSEData1, PullEVSEData2))
                return true;

            if (PullEVSEData1 is null || PullEVSEData2 is null)
                return false;

            return PullEVSEData1.Equals(PullEVSEData2);

        }

        #endregion

        #region Operator != (PullEVSEData1, PullEVSEData2)

        /// <summary>
        /// Compares two pull EVSE data requests for inequality.
        /// </summary>
        /// <param name="PullEVSEData1">An pull EVSE data request.</param>
        /// <param name="PullEVSEData2">Another pull EVSE data request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullEVSEDataRequest PullEVSEData1,
                                           PullEVSEDataRequest PullEVSEData2)

            => !(PullEVSEData1 == PullEVSEData2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEDataRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is PullEVSEDataRequest pullEVSEDataRequest &&
                   Equals(pullEVSEDataRequest);

        #endregion

        #region Equals(PullEVSEData)

        /// <summary>
        /// Compares two pull EVSE data requests for equality.
        /// </summary>
        /// <param name="PullEVSEData">An pull EVSE data request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEDataRequest? PullEVSEData)

            => PullEVSEData is not null &&

               ProviderId.                  Equals(PullEVSEData.ProviderId)                   &&
               DistanceKM.                  Equals(PullEVSEData.DistanceKM)                   &&
               GeoCoordinatesResponseFormat.Equals(PullEVSEData.GeoCoordinatesResponseFormat) &&

               ((!SearchCenter.HasValue && !PullEVSEData.SearchCenter.HasValue) ||
                 (SearchCenter.HasValue &&  PullEVSEData.SearchCenter.HasValue && SearchCenter.Value.Equals(PullEVSEData.SearchCenter.Value))) &&

               ((!LastCall.    HasValue && !PullEVSEData.LastCall.    HasValue) ||
                 (LastCall.    HasValue &&  PullEVSEData.LastCall.    HasValue && LastCall.    Value.Equals(PullEVSEData.LastCall.    Value)));

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

            => new String[] {

                   SearchCenter.HasValue
                       ? $"{SearchCenter} / {DistanceKM} km"
                       : "",

                   LastCall.HasValue
                       ? LastCall.Value.ToString()
                       : "",

                   $" ({ProviderId})"

               }.Where(text => text.IsNotNullOrEmpty()).
                 AggregateWith(", ");

        #endregion

    }

}
