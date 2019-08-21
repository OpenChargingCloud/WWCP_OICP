/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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
    /// A group of OICP operator EVSE status records or a status code.
    /// </summary>
    public class EVSEStatus : AResponse<PullEVSEStatusRequest,
                                        EVSEStatus>
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE status records grouped by their operators.
        /// </summary>
        public IEnumerable<OperatorEVSEStatus>  OperatorEVSEStatus   { get; }

        /// <summary>
        /// The status code for this request.
        /// </summary>
        public StatusCode?                      StatusCode           { get; }

        #endregion

        #region Constructor(s)

        #region EVSEStatus(Request, OperatorEVSEStatus, StatusCode = null, CustomData = null)

        /// <summary>
        /// Create a new group of OICP operator EVSE status records or a status code.
        /// </summary>
        /// <param name="Request">A PullEVSEStatus request.</param>
        /// <param name="OperatorEVSEStatus">An enumeration of EVSE status records grouped by their operators.</param>
        /// <param name="StatusCode">An optional status code for this request.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public EVSEStatus(PullEVSEStatusRequest                Request,
                          IEnumerable<OperatorEVSEStatus>      OperatorEVSEStatus,
                          StatusCode?                          StatusCode  = null,
                          IReadOnlyDictionary<String, Object>  CustomData  = null)

            : base(Request,
                   CustomData)

        {

            #region Initial checks

            if (OperatorEVSEStatus == null)
                throw new ArgumentNullException(nameof(OperatorEVSEStatus),  "The given OperatorEVSEStatus must not be null!");

            #endregion

            this.OperatorEVSEStatus  = OperatorEVSEStatus;
            this.StatusCode          = StatusCode;

        }

        #endregion

        #region EVSEStatus(Request, Code, Description = null, AdditionalInfo = null, CustomData = null)

        /// <summary>
        /// Create a new group of OICP operator EVSE status records or a status code.
        /// </summary>
        /// <param name="Request">A PullEVSEStatus request.</param>
        /// <param name="Code">The result code of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="AdditionalInfo">An optional additional information.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public EVSEStatus(PullEVSEStatusRequest                Request,
                          StatusCodes                          Code,
                          String                               Description     = null,
                          String                               AdditionalInfo  = null,
                          IReadOnlyDictionary<String, Object>  CustomData      = null)

            : this(Request,
                   new OperatorEVSEStatus[0],
                   new StatusCode(Code,
                                  Description,
                                  AdditionalInfo),
                   CustomData)

        { }

        #endregion

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEStatus  = "http://www.hubject.com/b2b/services/evsestatus/v2.0"
        //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <EVSEStatus:eRoamingEvseStatus>
        //
        //          <EVSEStatus:EvseStatuses>
        //
        //             <!--Zero or more repetitions:-->
        //             <EVSEStatus:OperatorEvseStatus>
        //
        //                <EVSEStatus:OperatorID>?</EVSEStatus:OperatorID>
        //
        //                <!--Optional:-->
        //                <EVSEStatus:OperatorName>?</EVSEStatus:OperatorName>
        //
        //                <!--Zero or more repetitions:-->
        //                <EVSEStatus:EvseStatusRecord>
        //                   <EVSEStatus:EvseId>?</EVSEStatus:EvseId>
        //                   <EVSEStatus:EvseStatus>?</EVSEStatus:EvseStatus>
        //                </EVSEStatus:EvseStatusRecord>
        //
        //             </EVSEStatus:OperatorEvseStatus>
        //
        //          </EVSEStatus:EvseStatuses>
        //
        //          <!--Optional:-->
        //          <EVSEStatus:StatusCode>
        //
        //             <CommonTypes:Code>?</CommonTypes:Code>
        //
        //             <!--Optional:-->
        //             <CommonTypes:Description>?</CommonTypes:Description>
        //
        //             <!--Optional:-->
        //             <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
        //
        //          </EVSEStatus:StatusCode>
        //
        //       </EVSEStatus:eRoamingEvseStatus>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, EVSEStatusXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="Request">An PullEVSEStatus request.</param>
        /// <param name="EVSEStatusXML">The XML to parse.</param>
        /// <param name="CustomEVSEStatusParser">A delegate to parse custom EVSEStatus XML elements.</param>
        /// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom OperatorEVSEStatus XML elements.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEStatus Parse(PullEVSEStatusRequest                        Request,
                                       XElement                                     EVSEStatusXML,
                                       CustomXMLParserDelegate<EVSEStatus>          CustomEVSEStatusParser           = null,
                                       CustomXMLParserDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusParser   = null,
                                       CustomXMLParserDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordParser     = null,
                                       CustomXMLParserDelegate<StatusCode>          CustomStatusCodeParser           = null,
                                       OnExceptionDelegate                          OnException                      = null)
        {

            if (TryParse(Request,
                         EVSEStatusXML,
                         out EVSEStatus _EVSEStatus,
                         CustomEVSEStatusParser,
                         CustomOperatorEVSEStatusParser,
                         CustomEVSEStatusRecordParser,
                         CustomStatusCodeParser,
                         OnException))

                return _EVSEStatus;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, EVSEStatusText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="Request">An PullEVSEStatus request.</param>
        /// <param name="EVSEStatusText">The text to parse.</param>
        /// <param name="CustomEVSEStatusParser">A delegate to parse custom EVSEStatus XML elements.</param>
        /// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom OperatorEVSEStatus XML elements.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEStatus Parse(PullEVSEStatusRequest                        Request,
                                       String                                       EVSEStatusText,
                                       CustomXMLParserDelegate<EVSEStatus>          CustomEVSEStatusParser           = null,
                                       CustomXMLParserDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusParser   = null,
                                       CustomXMLParserDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordParser     = null,
                                       CustomXMLParserDelegate<StatusCode>          CustomStatusCodeParser           = null,
                                       OnExceptionDelegate                          OnException                      = null)
        {

            if (TryParse(Request,
                         EVSEStatusText,
                         out EVSEStatus _EVSEStatus,
                         CustomEVSEStatusParser,
                         CustomOperatorEVSEStatusParser,
                         CustomEVSEStatusRecordParser,
                         CustomStatusCodeParser,
                         OnException))

                return _EVSEStatus;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, EVSEStatusXML,  out EVSEStatus, ..., OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="Request">An PullEVSEStatus request.</param>
        /// <param name="EVSEStatusXML">The XML to parse.</param>
        /// <param name="EVSEStatus">The parsed EVSEStatus request.</param>
        /// <param name="CustomEVSEStatusParser">A delegate to parse custom EVSEStatus XML elements.</param>
        /// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom OperatorEVSEStatus XML elements.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(PullEVSEStatusRequest                        Request,
                                       XElement                                     EVSEStatusXML,
                                       out EVSEStatus                               EVSEStatus,
                                       CustomXMLParserDelegate<EVSEStatus>          CustomEVSEStatusParser           = null,
                                       CustomXMLParserDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusParser   = null,
                                       CustomXMLParserDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordParser     = null,
                                       CustomXMLParserDelegate<StatusCode>          CustomStatusCodeParser           = null,
                                       OnExceptionDelegate                          OnException                      = null)
        {

            try
            {

                if (EVSEStatusXML.Name != OICPNS.EVSEStatus + "eRoamingEvseStatus")
                {
                    EVSEStatus = null;
                    return false;
                }

                EVSEStatus  = new EVSEStatus(

                                  Request,

                                  EVSEStatusXML.MapElements(OICPNS.EVSEStatus + "EvseStatuses",
                                                            OICPNS.EVSEStatus + "OperatorEvseStatus",
                                                            (XML, e) => OICPv2_1.OperatorEVSEStatus.Parse(XML,
                                                                                                          CustomOperatorEVSEStatusParser,
                                                                                                          CustomEVSEStatusRecordParser,
                                                                                                          e),
                                                            OnException),

                                  EVSEStatusXML.MapElement (OICPNS.EVSEStatus + "StatusCode",
                                                           (XML, e) => OICPv2_1.StatusCode.Parse(XML,
                                                                                                 CustomStatusCodeParser,
                                                                                                 e),
                                                            OnException)

                              );


                if (CustomEVSEStatusParser != null)
                    EVSEStatus = CustomEVSEStatusParser(EVSEStatusXML,
                                                        EVSEStatus);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, EVSEStatusXML, e);

                EVSEStatus = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, EVSEStatusText, out EVSEStatus, ..., OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="Request">An PullEVSEStatus request.</param>
        /// <param name="EVSEStatusText">The text to parse.</param>
        /// <param name="EVSEStatus">The parsed EVSE statuses request.</param>
        /// <param name="CustomEVSEStatusParser">A delegate to parse custom EVSEStatus XML elements.</param>
        /// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom OperatorEVSEStatus XML elements.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(PullEVSEStatusRequest                        Request,
                                       String                                       EVSEStatusText,
                                       out EVSEStatus                               EVSEStatus,
                                       CustomXMLParserDelegate<EVSEStatus>          CustomEVSEStatusParser           = null,
                                       CustomXMLParserDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusParser   = null,
                                       CustomXMLParserDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordParser     = null,
                                       CustomXMLParserDelegate<StatusCode>          CustomStatusCodeParser           = null,
                                       OnExceptionDelegate                          OnException                      = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(EVSEStatusText).Root,
                             out EVSEStatus,
                             CustomEVSEStatusParser,
                             CustomOperatorEVSEStatusParser,
                             CustomEVSEStatusRecordParser,
                             CustomStatusCodeParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, EVSEStatusText, e);
            }

            EVSEStatus = null;
            return false;

        }

        #endregion

        #region ToXML(CustomEVSEStatusSerializer = null, OperatorEVSEStatusXName = null, CustomOperatorEVSEStatusSerializer = null, EVSEStatusRecordXName = null, CustomEVSEStatusRecordSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomEVSEStatusSerializer">A delegate to serialize custom EVSEStatus XML elements.</param>
        /// <param name="OperatorEVSEStatusXName">The OperatorEVSEStatus XML name to use.</param>
        /// <param name="CustomOperatorEVSEStatusSerializer">A delegate to serialize custom OperatorEVSEStatus XML elements.</param>
        /// <param name="EVSEStatusRecordXName">The EVSEStatusRecord XML name to use.</param>
        /// <param name="CustomEVSEStatusRecordSerializer">A delegate to serialize custom EVSEStatusRecord XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<EVSEStatus>          CustomEVSEStatusSerializer           = null,
                              XName                                            OperatorEVSEStatusXName              = null,
                              CustomXMLSerializerDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusSerializer   = null,
                              XName                                            EVSEStatusRecordXName                = null,
                              CustomXMLSerializerDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordSerializer     = null)

        {

            var XML = new XElement(OICPNS.EVSEStatus + "eRoamingEvseStatus",

                          new XElement(OICPNS.EVSEStatus + "EvseStatuses",
                              OperatorEVSEStatus.Any()
                                  ? OperatorEVSEStatus.SafeSelect(operatorevsestatus => operatorevsestatus.ToXML(OperatorEVSEStatusXName,
                                                                                                                 CustomOperatorEVSEStatusSerializer,
                                                                                                                 EVSEStatusRecordXName,
                                                                                                                 CustomEVSEStatusRecordSerializer))
                                  : null),

                          StatusCode.HasValue
                              ? StatusCode.Value.ToXML()
                              : null

                      );


            return CustomEVSEStatusSerializer != null
                       ? CustomEVSEStatusSerializer(this, XML)
                       : XML;

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
        public static Boolean operator == (EVSEStatus EVSEStatus1, EVSEStatus EVSEStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEStatus1, EVSEStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEStatus1 == null) || ((Object) EVSEStatus2 == null))
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
        public static Boolean operator != (EVSEStatus EVSEStatus1, EVSEStatus EVSEStatus2)

            => !(EVSEStatus1 == EVSEStatus2);

        #endregion

        #endregion

        #region IEquatable<EVSEStatus> Members

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

            var EVSEStatus = Object as EVSEStatus;
            if ((Object) EVSEStatus == null)
                return false;

            return Equals(EVSEStatus);

        }

        #endregion

        #region Equals(EVSEStatus)

        /// <summary>
        /// Compares two EVSE status for equality.
        /// </summary>
        /// <param name="EVSEStatus">An EVSE status to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(EVSEStatus EVSEStatus)
        {

            if ((Object) EVSEStatus == null)
                return false;

            return (!OperatorEVSEStatus.Any() && !EVSEStatus.OperatorEVSEStatus.Any()) ||
                    (OperatorEVSEStatus.Any() &&  EVSEStatus.OperatorEVSEStatus.Any() && OperatorEVSEStatus.Count().Equals(EVSEStatus.OperatorEVSEStatus.Count())) &&

                    (StatusCode == null && EVSEStatus.StatusCode == null) ||
                    (StatusCode != null && EVSEStatus.StatusCode != null && StatusCode.Equals(EVSEStatus.StatusCode));

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

                return (OperatorEVSEStatus.Any()
                           ? OperatorEVSEStatus.GetHashCode() * 5
                           : 0) ^

                       StatusCode.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(OperatorEVSEStatus.Count() + " operator EVSE status record(s)",
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
        /// An EVSEStatus response builder.
        /// </summary>
        public class Builder : AResponseBuilder<PullEVSEStatusRequest,
                                                EVSEStatus>
        {

            #region Properties

            /// <summary>
            /// An enumeration of EVSE status records grouped by their operators.
            /// </summary>
            public IEnumerable<OperatorEVSEStatus>  OperatorEVSEStatus   { get; set; }

            /// <summary>
            /// The status code for this request.
            /// </summary>
            public StatusCode?                      StatusCode           { get; set; }

            #endregion

            #region Constructor(s)

            #region Builder(Request,    CustomData = null)

            /// <summary>
            /// Create a new EVSEStatus response builder.
            /// </summary>
            /// <param name="Request">A PullEVSEStatus request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(PullEVSEStatusRequest                Request,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(Request,
                       CustomData)

            { }

            #endregion

            #region Builder(EVSEStatus, CustomData = null)

            /// <summary>
            /// Create a new EVSEStatus response builder.
            /// </summary>
            /// <param name="EVSEStatus">An EVSEStatus response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(EVSEStatus                           EVSEStatus,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(EVSEStatus?.Request,
                       EVSEStatus.HasCustomData
                           ? CustomData != null && CustomData.Any()
                                 ? EVSEStatus.CustomData.Concat(CustomData)
                                 : EVSEStatus.CustomData
                           : CustomData)

            {

                if (EVSEStatus != null)
                {

                    this.OperatorEVSEStatus  = EVSEStatus.OperatorEVSEStatus;
                    this.StatusCode          = EVSEStatus.StatusCode;

                }

            }

            #endregion

            #endregion


            #region Equals(EVSEStatus)

            /// <summary>
            /// Compares two EVSE status responses for equality.
            /// </summary>
            /// <param name="EVSEStatus">An EVSE status response to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(EVSEStatus EVSEStatus)
            {

                if ((Object) EVSEStatus == null)
                    return false;

                return (!OperatorEVSEStatus.Any() && !EVSEStatus.OperatorEVSEStatus.Any()) ||
                        (OperatorEVSEStatus.Any() &&  EVSEStatus.OperatorEVSEStatus.Any() && OperatorEVSEStatus.Count().Equals(EVSEStatus.OperatorEVSEStatus.Count())) &&

                        (StatusCode != null && EVSEStatus.StatusCode != null) ||
                        (StatusCode == null && EVSEStatus.StatusCode == null && StatusCode.Equals(EVSEStatus.StatusCode));

            }

            #endregion

            public override EVSEStatus ToImmutable

                => new EVSEStatus(Request,
                                  OperatorEVSEStatus,
                                  StatusCode,
                                  CustomData);

        }

        #endregion

    }

}
