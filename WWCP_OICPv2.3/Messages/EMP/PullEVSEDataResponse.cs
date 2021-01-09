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
        /// <param name="OperatorEVSEData">An enumeration of EVSE data records grouped by their operators.</param>
        /// <param name="StatusCode">An optional status code of this response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public PullEVSEDataResponse(PullEVSEDataRequest            Request,
                                    IEnumerable<OperatorEVSEData>  OperatorEVSEData,
                                    StatusCode                     StatusCode   = null,
                                    Process_Id?                    ProcessId    = null,
                                    JObject                        CustomData   = null)

            : base(Request,
                   DateTime.UtcNow,
                   ProcessId,
                   CustomData)

        {

            this.OperatorEVSEData  = OperatorEVSEData ?? throw new ArgumentNullException(nameof(OperatorEVSEData), "The given enumeration of EVSE data records must not be null!");
            this.StatusCode        = StatusCode;

        }

        #endregion


        #region Documentation

        // {
        //   "EvseData": {
        //     "OperatorEvseData": [
        //       {
        //         "EvseDataRecord": [
        //           {
        //             "EvseID": "string",
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
        //     "AdditionalInfo":  "string",
        //     "Code":            "000",
        //     "Description":     "string"
        //   }
        // }

        #endregion

        //#region (static) Parse   (Request, PullEVSEDataResponseXML,  ..., OnException = null, ...)

        ///// <summary>
        ///// Parse the given XML representation of an OICP EVSE statuses request.
        ///// </summary>
        ///// <param name="Request">An PullPullEVSEDataResponse request.</param>
        ///// <param name="PullEVSEDataResponseXML">The XML to parse.</param>
        ///// <param name="CustomPullEVSEDataResponseParser">A delegate to parse custom PullEVSEData responses.</param>
        ///// <param name="CustomEVSEDataParser">A delegate to parse custom EVSEData XML elements.</param>
        ///// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        ///// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        ///// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        ///// <param name="CustomChargingFacilityParser">A delegate to parse custom ChargingFacility XML elements.</param>
        ///// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        //public static PullEVSEDataResponse

        //    Parse(PullEVSEDataRequest                            Request,
        //          XElement                                       PullEVSEDataResponseXML,
        //          CustomXMLParserDelegate<PullEVSEDataResponse>  CustomPullEVSEDataResponseParser   = null,
        //          CustomXMLParserDelegate<EVSEData>              CustomEVSEDataParser               = null,
        //          CustomXMLParserDelegate<OperatorEVSEData>      CustomOperatorEVSEDataParser       = null,
        //          CustomXMLParserDelegate<EVSEDataRecord>        CustomEVSEDataRecordParser         = null,
        //          CustomXMLParserDelegate<Address>               CustomAddressParser                = null,
        //          CustomXMLParserDelegate<ChargingFacility>      CustomChargingFacilityParser       = null,
        //          CustomXMLParserDelegate<StatusCode>            CustomStatusCodeParser             = null,
        //          OnExceptionDelegate                            OnException                        = null)

        //{

        //    if (TryParse(Request,
        //                 PullEVSEDataResponseXML,
        //                 out PullEVSEDataResponse _PullEVSEDataResponse,
        //                 CustomPullEVSEDataResponseParser,
        //                 CustomEVSEDataParser,
        //                 CustomOperatorEVSEDataParser,
        //                 CustomEVSEDataRecordParser,
        //                 CustomAddressParser,
        //                 CustomChargingFacilityParser,
        //                 CustomStatusCodeParser,
        //                 OnException))

        //        return _PullEVSEDataResponse;

        //    return null;

        //}

        //#endregion

        //#region (static) Parse   (Request, PullEVSEDataResponseText, ..., OnException = null, ...)

        ///// <summary>
        ///// Parse the given text-representation of an OICP EVSE statuses request.
        ///// </summary>
        ///// <param name="Request">An PullPullEVSEDataResponse request.</param>
        ///// <param name="PullEVSEDataResponseText">The text to parse.</param>
        ///// <param name="CustomPullEVSEDataResponseParser">A delegate to parse custom PullEVSEData responses.</param>
        ///// <param name="CustomEVSEDataParser">A delegate to parse custom EVSEData XML elements.</param>
        ///// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        ///// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        ///// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        ///// <param name="CustomChargingFacilityParser">A delegate to parse custom ChargingFacility XML elements.</param>
        ///// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        //public static PullEVSEDataResponse

        //    Parse(PullEVSEDataRequest                            Request,
        //          String                                         PullEVSEDataResponseText,
        //          CustomXMLParserDelegate<PullEVSEDataResponse>  CustomPullEVSEDataResponseParser   = null,
        //          CustomXMLParserDelegate<EVSEData>              CustomEVSEDataParser               = null,
        //          CustomXMLParserDelegate<OperatorEVSEData>      CustomOperatorEVSEDataParser       = null,
        //          CustomXMLParserDelegate<EVSEDataRecord>        CustomEVSEDataRecordParser         = null,
        //          CustomXMLParserDelegate<Address>               CustomAddressParser                = null,
        //          CustomXMLParserDelegate<ChargingFacility>      CustomChargingFacilityParser       = null,
        //          CustomXMLParserDelegate<StatusCode>            CustomStatusCodeParser             = null,
        //          OnExceptionDelegate                            OnException                        = null)

        //{

        //    if (TryParse(Request,
        //                 PullEVSEDataResponseText,
        //                 out PullEVSEDataResponse _PullEVSEDataResponse,
        //                 CustomPullEVSEDataResponseParser,
        //                 CustomEVSEDataParser,
        //                 CustomOperatorEVSEDataParser,
        //                 CustomEVSEDataRecordParser,
        //                 CustomAddressParser,
        //                 CustomChargingFacilityParser,
        //                 CustomStatusCodeParser,
        //                 OnException))

        //        return _PullEVSEDataResponse;

        //    return null;

        //}

        //#endregion

        //#region (static) TryParse(Request, PullEVSEDataResponseXML,  out PullEVSEDataResponse, ..., OnException = null, ...)

        ///// <summary>
        ///// Try to parse the given XML representation of an OICP EVSE statuses request.
        ///// </summary>
        ///// <param name="Request">An PullPullEVSEDataResponse request.</param>
        ///// <param name="PullEVSEDataResponseXML">The XML to parse.</param>
        ///// <param name="PullEVSEDataResponse">The parsed PullEVSEDataResponse request.</param>
        ///// <param name="CustomPullEVSEDataResponseParser">A delegate to parse custom PullEVSEData responses.</param>
        ///// <param name="CustomEVSEDataParser">A delegate to parse custom EVSEData XML elements.</param>
        ///// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        ///// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        ///// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        ///// <param name="CustomChargingFacilityParser">A delegate to parse custom ChargingFacility XML elements.</param>
        ///// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        //public static Boolean TryParse(PullEVSEDataRequest                            Request,
        //                               XElement                                       PullEVSEDataResponseXML,
        //                               out PullEVSEDataResponse                       PullEVSEDataResponse,
        //                               CustomXMLParserDelegate<PullEVSEDataResponse>  CustomPullEVSEDataResponseParser   = null,
        //                               CustomXMLParserDelegate<EVSEData>              CustomEVSEDataParser               = null,
        //                               CustomXMLParserDelegate<OperatorEVSEData>      CustomOperatorEVSEDataParser       = null,
        //                               CustomXMLParserDelegate<EVSEDataRecord>        CustomEVSEDataRecordParser         = null,
        //                               CustomXMLParserDelegate<Address>               CustomAddressParser                = null,
        //                               CustomXMLParserDelegate<ChargingFacility>      CustomChargingFacilityParser       = null,
        //                               CustomXMLParserDelegate<StatusCode>            CustomStatusCodeParser             = null,
        //                               OnExceptionDelegate                            OnException                        = null)
        //{

        //    try
        //    {

        //        if (PullEVSEDataResponseXML.Name != OICPNS.EVSEData + "eRoamingEvseData")
        //        {
        //            PullEVSEDataResponse = null;
        //            return false;
        //        }

        //        PullEVSEDataResponse =  new PullEVSEDataResponse(

        //                                    Request,

        //                                    PullEVSEDataResponseXML.MapElementOrFail    (OICPNS.EVSEData + "EvseData",
        //                                                                                 (xml, e) => OICPv2_2.EVSEData.Parse  (xml,
        //                                                                                                                       CustomEVSEDataParser,
        //                                                                                                                       CustomOperatorEVSEDataParser,
        //                                                                                                                       CustomEVSEDataRecordParser,
        //                                                                                                                       CustomAddressParser,
        //                                                                                                                       CustomChargingFacilityParser,
        //                                                                                                                       e),
        //                                                                                 OnException),

        //                                    PullEVSEDataResponseXML.MapElementOrNullable(OICPNS.CommonTypes + "StatusCode",
        //                                                                                 (xml, e) => OICPv2_2.StatusCode.Parse(xml,
        //                                                                                                                       CustomStatusCodeParser,
        //                                                                                                                       e),
        //                                                                                 OnException)

        //                                );


        //        if (CustomPullEVSEDataResponseParser != null)
        //            PullEVSEDataResponse = CustomPullEVSEDataResponseParser(PullEVSEDataResponseXML,
        //                                                                    PullEVSEDataResponse);

        //        return true;

        //    }
        //    catch (Exception e)
        //    {

        //        OnException?.Invoke(DateTime.UtcNow, PullEVSEDataResponseXML, e);

        //        PullEVSEDataResponse = null;
        //        return false;

        //    }

        //}

        //#endregion

        //#region (static) TryParse(Request, PullEVSEDataResponseText, out PullEVSEDataResponse, ..., OnException = null, ...)

        ///// <summary>
        ///// Try to parse the given text-representation of an OICP EVSE statuses request.
        ///// </summary>
        ///// <param name="Request">An PullPullEVSEDataResponse request.</param>
        ///// <param name="PullEVSEDataResponseText">The text to parse.</param>
        ///// <param name="PullEVSEDataResponse">The parsed EVSE statuses request.</param>
        ///// <param name="CustomPullEVSEDataResponseParser">A delegate to parse custom PullEVSEData responses.</param>
        ///// <param name="CustomEVSEDataParser">A delegate to parse custom EVSEData XML elements.</param>
        ///// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        ///// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        ///// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        ///// <param name="CustomChargingFacilityParser">A delegate to parse custom ChargingFacility XML elements.</param>
        ///// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        //public static Boolean TryParse(PullEVSEDataRequest                            Request,
        //                               String                                         PullEVSEDataResponseText,
        //                               out PullEVSEDataResponse                       PullEVSEDataResponse,
        //                               CustomXMLParserDelegate<PullEVSEDataResponse>  CustomPullEVSEDataResponseParser   = null,
        //                               CustomXMLParserDelegate<EVSEData>              CustomEVSEDataParser               = null,
        //                               CustomXMLParserDelegate<OperatorEVSEData>      CustomOperatorEVSEDataParser       = null,
        //                               CustomXMLParserDelegate<EVSEDataRecord>        CustomEVSEDataRecordParser         = null,
        //                               CustomXMLParserDelegate<Address>               CustomAddressParser                = null,
        //                               CustomXMLParserDelegate<ChargingFacility>      CustomChargingFacilityParser       = null,
        //                               CustomXMLParserDelegate<StatusCode>            CustomStatusCodeParser             = null,
        //                               OnExceptionDelegate                            OnException                        = null)
        //{

        //    try
        //    {

        //        if (TryParse(Request,
        //                     XDocument.Parse(PullEVSEDataResponseText).Root,
        //                     out PullEVSEDataResponse,
        //                     CustomPullEVSEDataResponseParser,
        //                     CustomEVSEDataParser,
        //                     CustomOperatorEVSEDataParser,
        //                     CustomEVSEDataRecordParser,
        //                     CustomAddressParser,
        //                     CustomChargingFacilityParser,
        //                     CustomStatusCodeParser,
        //                     OnException))

        //            return true;

        //    }
        //    catch (Exception e)
        //    {
        //        OnException?.Invoke(DateTime.UtcNow, PullEVSEDataResponseText, e);
        //    }

        //    PullEVSEDataResponse = null;
        //    return false;

        //}

        //#endregion

        //#region ToXML(CustomPullEVSEDataResponseSerializer = null, CustomChargeDetailRecordSerializer = null, CustomIdentificationSerializer = null)

        ///// <summary>
        ///// Return a XML representation of this object.
        ///// </summary>
        ///// <param name="CustomPullEVSEDataResponseSerializer">A delegate to customize the serialization of PullEVSEData responses.</param>
        ///// <param name="CustomEVSEDataSerializer">A delegate to serialize custom EVSEData XML elements.</param>
        ///// <param name="OperatorEVSEDataXName">The OperatorEVSEData XML name to use.</param>
        ///// <param name="CustomOperatorEVSEDataSerializer">A delegate to serialize custom OperatorEVSEData XML elements.</param>
        ///// <param name="EVSEDataRecordXName">The EVSEDataRecord XML name to use.</param>
        ///// <param name="IncludeEVSEDataRecordMetadata">Include EVSEDataRecord deltaType and lastUpdate meta data.</param>
        ///// <param name="CustomEVSEDataRecordSerializer">A delegate to serialize custom EVSEDataRecord XML elements.</param>
        ///// <param name="CustomAddressSerializer">A delegate to serialize custom Address XML elements.</param>
        ///// <param name="CustomChargingFacilitySerializer">A delegate to serialize custom ChargingFacility XML elements.</param>
        ///// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode XML elements.</param>
        //public XElement ToXML(CustomXMLSerializerDelegate<PullEVSEDataResponse>  CustomPullEVSEDataResponseSerializer   = null,
        //                      CustomXMLSerializerDelegate<EVSEData>              CustomEVSEDataSerializer               = null,
        //                      XName                                              OperatorEVSEDataXName                  = null,
        //                      CustomXMLSerializerDelegate<OperatorEVSEData>      CustomOperatorEVSEDataSerializer       = null,
        //                      XName                                              EVSEDataRecordXName                    = null,
        //                      Boolean                                            IncludeEVSEDataRecordMetadata          = true,
        //                      CustomXMLSerializerDelegate<EVSEDataRecord>        CustomEVSEDataRecordSerializer         = null,
        //                      CustomXMLSerializerDelegate<Address>               CustomAddressSerializer                = null,
        //                      CustomXMLSerializerDelegate<ChargingFacility>      CustomChargingFacilitySerializer       = null,
        //                      CustomXMLSerializerDelegate<StatusCode>            CustomStatusCodeSerializer             = null)

        //{

        //    var XML = new XElement(OICPNS.EVSEData + "eRoamingEVSEData",

        //                  EVSEData.ToXML(CustomEVSEDataSerializer,
        //                                 OperatorEVSEDataXName,
        //                                 CustomOperatorEVSEDataSerializer,
        //                                 EVSEDataRecordXName,
        //                                 IncludeEVSEDataRecordMetadata,
        //                                 CustomEVSEDataRecordSerializer,
        //                                 CustomAddressSerializer,
        //                                 CustomChargingFacilitySerializer),

        //                  StatusCode.HasValue
        //                      ? StatusCode.Value.ToXML(OICPNS.EVSEData + "StatusCode",
        //                                               CustomStatusCodeSerializer)
        //                      : null

        //              );


        //    return CustomPullEVSEDataResponseSerializer != null
        //               ? CustomPullEVSEDataResponseSerializer(this, XML)
        //               : XML;

        //}

        //#endregion


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

                (StatusCode != null && PullEVSEDataResponse.StatusCode != null) ||
                (StatusCode == null && PullEVSEDataResponse.StatusCode == null && StatusCode.Equals(PullEVSEDataResponse.StatusCode));

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
                           OperatorEVSEData,
                           StatusCode,
                           ProcessId,
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
            /// <param name="Request">A PullEVSEStatus request.</param>
            /// <param name="OperatorEVSEData">An enumeration of EVSE data records grouped by their operators.</param>
            /// <param name="StatusCode">An optional status code for this request.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(PullEVSEDataRequest            Request            = null,
                           IEnumerable<OperatorEVSEData>  OperatorEVSEData   = null,
                           StatusCode                     StatusCode         = null,
                           Process_Id?                    ProcessId          = null,
                           JObject                        CustomData         = null)

                : base(Request,
                       DateTime.UtcNow,
                       ProcessId,
                       CustomData)

            {

                this.OperatorEVSEData  = OperatorEVSEData != null ? new HashSet<OperatorEVSEData>(OperatorEVSEData) : new HashSet<OperatorEVSEData>();
                this.StatusCode        = StatusCode       != null ? StatusCode.ToBuilder()                          : new StatusCode.Builder();

            }

            #endregion


            #region Equals(PullEVSEDataResponse)

            ///// <summary>
            ///// Compares two PullEVSEData responses for equality.
            ///// </summary>
            ///// <param name="PullEVSEDataResponse">A PullEVSEData response to compare with.</param>
            ///// <returns>True if both match; False otherwise.</returns>
            //public Boolean Equals(PullEVSEDataResponse PullEVSEDataResponse)
            //{

            //    if ((Object) PullEVSEDataResponse == null)
            //        return false;

            //    return  (EVSEData   != null && PullEVSEDataResponse.EVSEData   != null) ||
            //            (EVSEData   == null && PullEVSEDataResponse.EVSEData   == null && EVSEData.  Equals(PullEVSEDataResponse.EVSEData)) &&

            //            (StatusCode != null && PullEVSEDataResponse.StatusCode != null) ||
            //            (StatusCode == null && PullEVSEDataResponse.StatusCode == null && StatusCode.Equals(PullEVSEDataResponse.StatusCode));

            //}

            #endregion

            public override PullEVSEDataResponse ToImmutable()

                => new PullEVSEDataResponse(Request,
                                            OperatorEVSEData,
                                            StatusCode,
                                            ProcessId,
                                            CustomData);

        }

        #endregion

    }

}
