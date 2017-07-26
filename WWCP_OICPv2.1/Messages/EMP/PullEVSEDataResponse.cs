/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// A PullEVSEData response.
    /// </summary>
    public class PullEVSEDataResponse : AResponse<PullEVSEDataRequest,
                                                  PullEVSEDataResponse>
    {

        #region Properties

        /// <summary>
        /// The static EVSE data.
        /// </summary>
        public EVSEData     EVSEData     { get; }

        /// <summary>
        /// The optional status code for this request.
        /// </summary>
        public StatusCode?  StatusCode   { get; }

        #endregion

        #region Constructor(s)

        #region PullEVSEDataResponse(Request, EVSEData, StatusCode = null, CustomData = null)

        /// <summary>
        /// Create a new PullEVSEData response.
        /// </summary>
        /// <param name="Request">A PullEVSEData request.</param>
        /// <param name="EVSEData">The static EVSE data.</param>
        /// <param name="StatusCode">An optional status code for this request.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public PullEVSEDataResponse(PullEVSEDataRequest                  Request,
                                    EVSEData                             EVSEData,
                                    StatusCode?                          StatusCode  = null,
                                    IReadOnlyDictionary<String, Object>  CustomData  = null)

            : base(Request,
                   CustomData)

        {

            #region Initial checks

            if (EVSEData == null)
                throw new ArgumentNullException(nameof(EVSEData),  "The given EVSE data must not be null!");

            #endregion

            this.EVSEData    = EVSEData;
            this.StatusCode  = StatusCode;

        }

        #endregion

        #region PullEVSEDataResponse(Request, StatusCode, Description = null, AdditionalInfo = null, CustomData = null)

        /// <summary>
        /// Create a new OICP 'negative' acknowledgement.
        /// </summary>
        /// <param name="Request">A PullEVSEData request.</param>
        /// <param name="StatusCode">The status code of the operation.</param>
        /// <param name="Description">An optional description of the status code.</param>
        /// <param name="AdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public PullEVSEDataResponse(PullEVSEDataRequest                  Request,
                                    StatusCodes                          StatusCode,
                                    String                               Description      = null,
                                    String                               AdditionalInfo   = null,
                                    IReadOnlyDictionary<String, Object>  CustomData       = null)

            : this(Request,
                   new EVSEData(new OperatorEVSEData[0]),
                   new StatusCode(StatusCode,
                                  Description,
                                  AdditionalInfo),
                   CustomData)

        { }

        #endregion

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv   = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEData  = "http://www.hubject.com/b2b/services/evsedata/v2.1"
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <EVSEData:eRoamingEvseData>
        //
        //           <EVSEData:EvseData>
        //
        //               <!--Zero or more repetitions:-->
        //               <EVSEData:OperatorEvseData>
        //                   [...]
        //               </EVSEData:OperatorEvseData>
        //
        //          </EVSEData:EvseData>
        //
        //          <!--Optional:-->
        //          <CommonTypes:StatusCode>
        //            <CommonTypes:Code>...</CommonTypes:Code>
        //            <CommonTypes:Description>...</CommonTypes:Description>
        //            <CommonTypes:AdditionalInfo>...</CommonTypes:AdditionalInfo>
        //          </CommonTypes:StatusCode>
        //
        //       </EVSEData:eRoamingEvseData>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, PullEVSEDataResponseXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="Request">An PullPullEVSEDataResponse request.</param>
        /// <param name="PullEVSEDataResponseXML">The XML to parse.</param>
        /// <param name="CustomPullEVSEDataResponseParser">A delegate to parse custom PullEVSEData responses.</param>
        /// <param name="CustomEVSEDataParser">A delegate to parse custom EVSEData XML elements.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static PullEVSEDataResponse

            Parse(PullEVSEDataRequest                            Request,
                  XElement                                       PullEVSEDataResponseXML,
                  CustomXMLParserDelegate<PullEVSEDataResponse>  CustomPullEVSEDataResponseParser   = null,
                  CustomXMLParserDelegate<EVSEData>              CustomEVSEDataParser               = null,
                  CustomXMLParserDelegate<OperatorEVSEData>      CustomOperatorEVSEDataParser       = null,
                  CustomXMLParserDelegate<EVSEDataRecord>        CustomEVSEDataRecordParser         = null,
                  CustomXMLParserDelegate<Address>               CustomAddressParser                = null,
                  CustomXMLParserDelegate<StatusCode>            CustomStatusCodeParser             = null,
                  OnExceptionDelegate                            OnException                        = null)

        {

            if (TryParse(Request,
                         PullEVSEDataResponseXML,
                         out PullEVSEDataResponse _PullEVSEDataResponse,
                         CustomPullEVSEDataResponseParser,
                         CustomEVSEDataParser,
                         CustomOperatorEVSEDataParser,
                         CustomEVSEDataRecordParser,
                         CustomAddressParser,
                         CustomStatusCodeParser,
                         OnException))

                return _PullEVSEDataResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, PullEVSEDataResponseText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="Request">An PullPullEVSEDataResponse request.</param>
        /// <param name="PullEVSEDataResponseText">The text to parse.</param>
        /// <param name="CustomPullEVSEDataResponseParser">A delegate to parse custom PullEVSEData responses.</param>
        /// <param name="CustomEVSEDataParser">A delegate to parse custom EVSEData XML elements.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static PullEVSEDataResponse

            Parse(PullEVSEDataRequest                            Request,
                  String                                         PullEVSEDataResponseText,
                  CustomXMLParserDelegate<PullEVSEDataResponse>  CustomPullEVSEDataResponseParser   = null,
                  CustomXMLParserDelegate<EVSEData>              CustomEVSEDataParser               = null,
                  CustomXMLParserDelegate<OperatorEVSEData>      CustomOperatorEVSEDataParser       = null,
                  CustomXMLParserDelegate<EVSEDataRecord>        CustomEVSEDataRecordParser         = null,
                  CustomXMLParserDelegate<Address>               CustomAddressParser                = null,
                  CustomXMLParserDelegate<StatusCode>            CustomStatusCodeParser             = null,
                  OnExceptionDelegate                            OnException                        = null)

        {

            if (TryParse(Request,
                         PullEVSEDataResponseText,
                         out PullEVSEDataResponse _PullEVSEDataResponse,
                         CustomPullEVSEDataResponseParser,
                         CustomEVSEDataParser,
                         CustomOperatorEVSEDataParser,
                         CustomEVSEDataRecordParser,
                         CustomAddressParser,
                         CustomStatusCodeParser,
                         OnException))

                return _PullEVSEDataResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, PullEVSEDataResponseXML,  out PullEVSEDataResponse, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="Request">An PullPullEVSEDataResponse request.</param>
        /// <param name="PullEVSEDataResponseXML">The XML to parse.</param>
        /// <param name="PullEVSEDataResponse">The parsed PullEVSEDataResponse request.</param>
        /// <param name="CustomPullEVSEDataResponseParser">A delegate to parse custom PullEVSEData responses.</param>
        /// <param name="CustomEVSEDataParser">A delegate to parse custom EVSEData XML elements.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(PullEVSEDataRequest                            Request,
                                       XElement                                       PullEVSEDataResponseXML,
                                       out PullEVSEDataResponse                       PullEVSEDataResponse,
                                       CustomXMLParserDelegate<PullEVSEDataResponse>  CustomPullEVSEDataResponseParser   = null,
                                       CustomXMLParserDelegate<EVSEData>              CustomEVSEDataParser               = null,
                                       CustomXMLParserDelegate<OperatorEVSEData>      CustomOperatorEVSEDataParser       = null,
                                       CustomXMLParserDelegate<EVSEDataRecord>        CustomEVSEDataRecordParser         = null,
                                       CustomXMLParserDelegate<Address>               CustomAddressParser                = null,
                                       CustomXMLParserDelegate<StatusCode>            CustomStatusCodeParser             = null,
                                       OnExceptionDelegate                            OnException                        = null)
        {

            try
            {

                if (PullEVSEDataResponseXML.Name != OICPNS.EVSEData + "eRoamingEvseData")
                {
                    PullEVSEDataResponse = null;
                    return false;
                }

                PullEVSEDataResponse =  new PullEVSEDataResponse(

                                            Request,

                                            PullEVSEDataResponseXML.MapElementOrFail    (OICPNS.EVSEData + "EvseData",
                                                                                         (xml, e) => OICPv2_1.EVSEData.Parse  (xml,
                                                                                                                               CustomEVSEDataParser,
                                                                                                                               CustomOperatorEVSEDataParser,
                                                                                                                               CustomEVSEDataRecordParser,
                                                                                                                               CustomAddressParser,
                                                                                                                               e),
                                                                                         OnException),

                                            PullEVSEDataResponseXML.MapElementOrNullable(OICPNS.CommonTypes + "StatusCode",
                                                                                         (xml, e) => OICPv2_1.StatusCode.Parse(xml,
                                                                                                                               CustomStatusCodeParser,
                                                                                                                               e),
                                                                                         OnException)

                                        );


                if (CustomPullEVSEDataResponseParser != null)
                    PullEVSEDataResponse = CustomPullEVSEDataResponseParser(PullEVSEDataResponseXML,
                                                                            PullEVSEDataResponse);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, PullEVSEDataResponseXML, e);

                PullEVSEDataResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, PullEVSEDataResponseText, out PullEVSEDataResponse, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="Request">An PullPullEVSEDataResponse request.</param>
        /// <param name="PullEVSEDataResponseText">The text to parse.</param>
        /// <param name="PullEVSEDataResponse">The parsed EVSE statuses request.</param>
        /// <param name="CustomPullEVSEDataResponseParser">A delegate to parse custom PullEVSEData responses.</param>
        /// <param name="CustomEVSEDataParser">A delegate to parse custom EVSEData XML elements.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(PullEVSEDataRequest                            Request,
                                       String                                         PullEVSEDataResponseText,
                                       out PullEVSEDataResponse                       PullEVSEDataResponse,
                                       CustomXMLParserDelegate<PullEVSEDataResponse>  CustomPullEVSEDataResponseParser   = null,
                                       CustomXMLParserDelegate<EVSEData>              CustomEVSEDataParser               = null,
                                       CustomXMLParserDelegate<OperatorEVSEData>      CustomOperatorEVSEDataParser       = null,
                                       CustomXMLParserDelegate<EVSEDataRecord>        CustomEVSEDataRecordParser         = null,
                                       CustomXMLParserDelegate<Address>               CustomAddressParser                = null,
                                       CustomXMLParserDelegate<StatusCode>            CustomStatusCodeParser             = null,
                                       OnExceptionDelegate                            OnException                        = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(PullEVSEDataResponseText).Root,
                             out PullEVSEDataResponse,
                             CustomPullEVSEDataResponseParser,
                             CustomEVSEDataParser,
                             CustomOperatorEVSEDataParser,
                             CustomEVSEDataRecordParser,
                             CustomAddressParser,
                             CustomStatusCodeParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, PullEVSEDataResponseText, e);
            }

            PullEVSEDataResponse = null;
            return false;

        }

        #endregion

        #region ToXML(CustomPullEVSEDataResponseSerializer = null, CustomChargeDetailRecordSerializer = null, CustomIdentificationSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEDataResponseSerializer">A delegate to customize the serialization of PullEVSEData responses.</param>
        /// <param name="CustomChargeDetailRecordSerializer">A delegate to serialize custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<PullEVSEDataResponse>  CustomPullEVSEDataResponseSerializer   = null,
                              CustomXMLSerializerDelegate<EVSEData>              CustomEVSEDataSerializer               = null,
                              XName                                              OperatorEVSEDataXName                  = null,
                              CustomXMLSerializerDelegate<OperatorEVSEData>      CustomOperatorEVSEDataSerializer       = null,
                              XName                                              EVSEDataRecordXName                    = null,
                              Boolean                                            IncludeEVSEDataRecordMetadata          = true,
                              CustomXMLSerializerDelegate<EVSEDataRecord>        CustomEVSEDataRecordSerializer         = null,
                              CustomXMLSerializerDelegate<StatusCode>            CustomStatusCodeSerializer             = null)

        {

            var XML = new XElement(OICPNS.EVSEData + "eRoamingEVSEData",

                          EVSEData.ToXML(CustomEVSEDataSerializer,
                                         OperatorEVSEDataXName,
                                         CustomOperatorEVSEDataSerializer,
                                         EVSEDataRecordXName,
                                         IncludeEVSEDataRecordMetadata,
                                         CustomEVSEDataRecordSerializer),

                          StatusCode.HasValue
                              ? StatusCode.Value.ToXML(OICPNS.EVSEData + "StatusCode",
                                                       CustomStatusCodeSerializer)
                              : null

                      );


            return CustomPullEVSEDataResponseSerializer != null
                       ? CustomPullEVSEDataResponseSerializer(this, XML)
                       : XML;

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
        public static Boolean operator == (PullEVSEDataResponse PullEVSEDataResponse1, PullEVSEDataResponse PullEVSEDataResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(PullEVSEDataResponse1, PullEVSEDataResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PullEVSEDataResponse1 == null) || ((Object) PullEVSEDataResponse2 == null))
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
        public static Boolean operator != (PullEVSEDataResponse PullEVSEDataResponse1, PullEVSEDataResponse PullEVSEDataResponse2)

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
        {

            if (Object == null)
                return false;

            var PullEVSEDataResponse = Object as PullEVSEDataResponse;
            if ((Object) PullEVSEDataResponse == null)
                return false;

            return Equals(PullEVSEDataResponse);

        }

        #endregion

        #region Equals(PullEVSEDataResponse)

        /// <summary>
        /// Compares two PullEVSEData responses for equality.
        /// </summary>
        /// <param name="PullEVSEDataResponse">A PullEVSEData response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEDataResponse PullEVSEDataResponse)
        {

            if ((Object) PullEVSEDataResponse == null)
                return false;

            return EVSEData.Equals(PullEVSEDataResponse.EVSEData) &&

                   (StatusCode != null && PullEVSEDataResponse.StatusCode != null) ||
                   (StatusCode == null && PullEVSEDataResponse.StatusCode == null && StatusCode.Equals(PullEVSEDataResponse.StatusCode));

        }

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

                return EVSEData.GetHashCode() * 3 ^

                       (StatusCode.HasValue
                            ? StatusCode.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVSEData.ToString(),
                             StatusCode.HasValue
                                 ? " -> " + StatusCode.Value.Code
                                 : "");

        #endregion


        #region ToBuilder

        /// <summary>
        /// Return a response builder.
        /// </summary>
        public Builder ToBuilder
            => new Builder(this);

        #endregion

        #region (class) Builder

        /// <summary>
        /// A PullEVSEData response builder.
        /// </summary>
        public class Builder : AResponseBuilder<PullEVSEDataRequest,
                                                PullEVSEDataResponse>
        {

            #region Properties

            /// <summary>
            /// The static EVSE data.
            /// </summary>
            public EVSEData     EVSEData     { get; set; }

            /// <summary>
            /// The optional status code for this request.
            /// </summary>
            public StatusCode?  StatusCode   { get; set; }

            #endregion

            #region Constructor(s)

            #region Builder(Request,      CustomData = null)

            /// <summary>
            /// Create a new PullEVSEData response builder.
            /// </summary>
            /// <param name="Request">A PullEVSEData request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(PullEVSEDataRequest                  Request,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(Request,
                       CustomData)

            { }

            #endregion

            #region Builder(PullEVSEData, CustomData = null)

            /// <summary>
            /// Create a new PullEVSEData response builder.
            /// </summary>
            /// <param name="PullEVSEData">A PullEVSEData response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(PullEVSEDataResponse                 PullEVSEData,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(PullEVSEData?.Request,
                       PullEVSEData.HasCustomData
                           ? CustomData != null && CustomData.Any()
                                 ? PullEVSEData.CustomValues.Concat(CustomData)
                                 : PullEVSEData.CustomValues
                           : CustomData)

            {

                this.EVSEData    = PullEVSEData?.EVSEData;
                this.StatusCode  = PullEVSEData?.StatusCode;

            }

            #endregion

            #endregion


            #region Equals(PullEVSEDataResponse)

            /// <summary>
            /// Compares two PullEVSEData responses for equality.
            /// </summary>
            /// <param name="PullEVSEDataResponse">A PullEVSEData response to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(PullEVSEDataResponse PullEVSEDataResponse)
            {

                if ((Object) PullEVSEDataResponse == null)
                    return false;

                return  (EVSEData   != null && PullEVSEDataResponse.EVSEData   != null) ||
                        (EVSEData   == null && PullEVSEDataResponse.EVSEData   == null && EVSEData.  Equals(PullEVSEDataResponse.EVSEData)) &&

                        (StatusCode != null && PullEVSEDataResponse.StatusCode != null) ||
                        (StatusCode == null && PullEVSEDataResponse.StatusCode == null && StatusCode.Equals(PullEVSEDataResponse.StatusCode));

            }

            #endregion

            public override PullEVSEDataResponse ToImmutable

                => new PullEVSEDataResponse(Request,
                                            EVSEData,
                                            StatusCode,
                                            ImmutableCustomData);



        }

        #endregion

    }

}
