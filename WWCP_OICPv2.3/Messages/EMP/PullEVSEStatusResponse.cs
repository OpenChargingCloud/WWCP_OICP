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
    /// The PullEVSEStatus response.
    /// </summary>
    public class PullEVSEStatusResponse : AResponse<PullEVSEStatusRequest,
                                                    PullEVSEStatusResponse>
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
        /// Create a new PullEVSEStatus response.
        /// </summary>
        /// <param name="Request">A PullEVSEStatus request.</param>
        /// <param name="OperatorEVSEStatus">An enumeration of EVSE status records grouped by their operators.</param>
        /// <param name="StatusCode">An optional status code of this response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public PullEVSEStatusResponse(PullEVSEStatusRequest            Request,
                                      IEnumerable<OperatorEVSEStatus>  OperatorEVSEStatus,
                                      StatusCode                       StatusCode   = null,
                                      Process_Id?                      ProcessId    = null,
                                      JObject                          CustomData   = null)

            : base(Request,
                   DateTime.UtcNow,
                   ProcessId,
                   CustomData)

        {

            this.OperatorEVSEStatus  = OperatorEVSEStatus ?? throw new ArgumentNullException(nameof(OperatorEVSEStatus), "The given OperatorEVSEStatus must not be null or empty!");
            this.StatusCode          = StatusCode;

        }

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

        //#region (static) Parse   (Request, EVSEStatusXML,  ..., OnException = null)

        ///// <summary>
        ///// Parse the given XML representation of an OICP EVSE statuses request.
        ///// </summary>
        ///// <param name="Request">An PullEVSEStatus request.</param>
        ///// <param name="EVSEStatusXML">The XML to parse.</param>
        ///// <param name="CustomEVSEStatusParser">A delegate to parse custom EVSEStatus XML elements.</param>
        ///// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom OperatorEVSEStatus XML elements.</param>
        ///// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        ///// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        //public static EVSEStatus Parse(PullEVSEStatusRequest                        Request,
        //                               XElement                                     EVSEStatusXML,
        //                               CustomXMLParserDelegate<EVSEStatus>          CustomEVSEStatusParser           = null,
        //                               CustomXMLParserDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusParser   = null,
        //                               CustomXMLParserDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordParser     = null,
        //                               CustomXMLParserDelegate<StatusCode>          CustomStatusCodeParser           = null,
        //                               OnExceptionDelegate                          OnException                      = null)
        //{

        //    if (TryParse(Request,
        //                 EVSEStatusXML,
        //                 out EVSEStatus _EVSEStatus,
        //                 CustomEVSEStatusParser,
        //                 CustomOperatorEVSEStatusParser,
        //                 CustomEVSEStatusRecordParser,
        //                 CustomStatusCodeParser,
        //                 OnException))

        //        return _EVSEStatus;

        //    return null;

        //}

        //#endregion

        //#region (static) Parse   (Request, EVSEStatusText, ..., OnException = null)

        ///// <summary>
        ///// Parse the given text-representation of an OICP EVSE statuses request.
        ///// </summary>
        ///// <param name="Request">An PullEVSEStatus request.</param>
        ///// <param name="EVSEStatusText">The text to parse.</param>
        ///// <param name="CustomEVSEStatusParser">A delegate to parse custom EVSEStatus XML elements.</param>
        ///// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom OperatorEVSEStatus XML elements.</param>
        ///// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        ///// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        //public static EVSEStatus Parse(PullEVSEStatusRequest                        Request,
        //                               String                                       EVSEStatusText,
        //                               CustomXMLParserDelegate<EVSEStatus>          CustomEVSEStatusParser           = null,
        //                               CustomXMLParserDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusParser   = null,
        //                               CustomXMLParserDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordParser     = null,
        //                               CustomXMLParserDelegate<StatusCode>          CustomStatusCodeParser           = null,
        //                               OnExceptionDelegate                          OnException                      = null)
        //{

        //    if (TryParse(Request,
        //                 EVSEStatusText,
        //                 out EVSEStatus _EVSEStatus,
        //                 CustomEVSEStatusParser,
        //                 CustomOperatorEVSEStatusParser,
        //                 CustomEVSEStatusRecordParser,
        //                 CustomStatusCodeParser,
        //                 OnException))

        //        return _EVSEStatus;

        //    return null;

        //}

        //#endregion

        //#region (static) TryParse(Request, EVSEStatusXML,  out EVSEStatus, ..., OnException = null)

        ///// <summary>
        ///// Try to parse the given XML representation of an OICP EVSE statuses request.
        ///// </summary>
        ///// <param name="Request">An PullEVSEStatus request.</param>
        ///// <param name="EVSEStatusXML">The XML to parse.</param>
        ///// <param name="EVSEStatus">The parsed EVSEStatus request.</param>
        ///// <param name="CustomEVSEStatusParser">A delegate to parse custom EVSEStatus XML elements.</param>
        ///// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom OperatorEVSEStatus XML elements.</param>
        ///// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        ///// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        //public static Boolean TryParse(PullEVSEStatusRequest                        Request,
        //                               XElement                                     EVSEStatusXML,
        //                               out EVSEStatus                               EVSEStatus,
        //                               CustomXMLParserDelegate<EVSEStatus>          CustomEVSEStatusParser           = null,
        //                               CustomXMLParserDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusParser   = null,
        //                               CustomXMLParserDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordParser     = null,
        //                               CustomXMLParserDelegate<StatusCode>          CustomStatusCodeParser           = null,
        //                               OnExceptionDelegate                          OnException                      = null)
        //{

        //    try
        //    {

        //        if (EVSEStatusXML.Name != OICPNS.EVSEStatus + "eRoamingEvseStatus")
        //        {
        //            EVSEStatus = null;
        //            return false;
        //        }

        //        EVSEStatus  = new EVSEStatus(

        //                          Request,

        //                          EVSEStatusXML.MapElements(OICPNS.EVSEStatus + "EvseStatuses",
        //                                                    OICPNS.EVSEStatus + "OperatorEvseStatus",
        //                                                    (XML, e) => OICPv2_2.OperatorEVSEStatus.Parse(XML,
        //                                                                                                  CustomOperatorEVSEStatusParser,
        //                                                                                                  CustomEVSEStatusRecordParser,
        //                                                                                                  e),
        //                                                    OnException),

        //                          EVSEStatusXML.MapElement (OICPNS.EVSEStatus + "StatusCode",
        //                                                   (XML, e) => OICPv2_2.StatusCode.Parse(XML,
        //                                                                                         CustomStatusCodeParser,
        //                                                                                         e),
        //                                                    OnException)

        //                      );


        //        if (CustomEVSEStatusParser != null)
        //            EVSEStatus = CustomEVSEStatusParser(EVSEStatusXML,
        //                                                EVSEStatus);

        //        return true;

        //    }
        //    catch (Exception e)
        //    {

        //        OnException?.Invoke(DateTime.UtcNow, EVSEStatusXML, e);

        //        EVSEStatus = null;
        //        return false;

        //    }

        //}

        //#endregion

        //#region (static) TryParse(Request, EVSEStatusText, out EVSEStatus, ..., OnException = null)

        ///// <summary>
        ///// Try to parse the given text-representation of an OICP EVSE statuses request.
        ///// </summary>
        ///// <param name="Request">An PullEVSEStatus request.</param>
        ///// <param name="EVSEStatusText">The text to parse.</param>
        ///// <param name="EVSEStatus">The parsed EVSE statuses request.</param>
        ///// <param name="CustomEVSEStatusParser">A delegate to parse custom EVSEStatus XML elements.</param>
        ///// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom OperatorEVSEStatus XML elements.</param>
        ///// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        ///// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        //public static Boolean TryParse(PullEVSEStatusRequest                        Request,
        //                               String                                       EVSEStatusText,
        //                               out EVSEStatus                               EVSEStatus,
        //                               CustomXMLParserDelegate<EVSEStatus>          CustomEVSEStatusParser           = null,
        //                               CustomXMLParserDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusParser   = null,
        //                               CustomXMLParserDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordParser     = null,
        //                               CustomXMLParserDelegate<StatusCode>          CustomStatusCodeParser           = null,
        //                               OnExceptionDelegate                          OnException                      = null)
        //{

        //    try
        //    {

        //        if (TryParse(Request,
        //                     XDocument.Parse(EVSEStatusText).Root,
        //                     out EVSEStatus,
        //                     CustomEVSEStatusParser,
        //                     CustomOperatorEVSEStatusParser,
        //                     CustomEVSEStatusRecordParser,
        //                     CustomStatusCodeParser,
        //                     OnException))

        //            return true;

        //    }
        //    catch (Exception e)
        //    {
        //        OnException?.Invoke(DateTime.UtcNow, EVSEStatusText, e);
        //    }

        //    EVSEStatus = null;
        //    return false;

        //}

        //#endregion

        //#region ToXML(CustomEVSEStatusSerializer = null, OperatorEVSEStatusXName = null, CustomOperatorEVSEStatusSerializer = null, EVSEStatusRecordXName = null, CustomEVSEStatusRecordSerializer = null)

        ///// <summary>
        ///// Return a XML representation of this object.
        ///// </summary>
        ///// <param name="CustomEVSEStatusSerializer">A delegate to serialize custom EVSEStatus XML elements.</param>
        ///// <param name="OperatorEVSEStatusXName">The OperatorEVSEStatus XML name to use.</param>
        ///// <param name="CustomOperatorEVSEStatusSerializer">A delegate to serialize custom OperatorEVSEStatus XML elements.</param>
        ///// <param name="EVSEStatusRecordXName">The EVSEStatusRecord XML name to use.</param>
        ///// <param name="CustomEVSEStatusRecordSerializer">A delegate to serialize custom EVSEStatusRecord XML elements.</param>
        //public XElement ToXML(CustomXMLSerializerDelegate<EVSEStatus>          CustomEVSEStatusSerializer           = null,
        //                      XName                                            OperatorEVSEStatusXName              = null,
        //                      CustomXMLSerializerDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusSerializer   = null,
        //                      XName                                            EVSEStatusRecordXName                = null,
        //                      CustomXMLSerializerDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordSerializer     = null)

        //{

        //    var XML = new XElement(OICPNS.EVSEStatus + "eRoamingEvseStatus",

        //                  new XElement(OICPNS.EVSEStatus + "EvseStatuses",
        //                      OperatorEVSEStatus.Any()
        //                          ? OperatorEVSEStatus.SafeSelect(operatorevsestatus => operatorevsestatus.ToXML(OperatorEVSEStatusXName,
        //                                                                                                         CustomOperatorEVSEStatusSerializer,
        //                                                                                                         EVSEStatusRecordXName,
        //                                                                                                         CustomEVSEStatusRecordSerializer))
        //                          : null),

        //                  StatusCode.HasValue
        //                      ? StatusCode.Value.ToXML()
        //                      : null

        //              );


        //    return CustomEVSEStatusSerializer != null
        //               ? CustomEVSEStatusSerializer(this, XML)
        //               : XML;

        //}

        //#endregion


        #region Operator overloading

        #region Operator == (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="EVSEStatus1">An EVSE status.</param>
        /// <param name="EVSEStatus2">Another EVSE status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullEVSEStatusResponse EVSEStatus1,
                                           PullEVSEStatusResponse EVSEStatus2)
        {

            if (ReferenceEquals(EVSEStatus1, EVSEStatus2))
                return true;

            if ((EVSEStatus1 is null) || (EVSEStatus2 is null))
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
        public static Boolean operator != (PullEVSEStatusResponse EVSEStatus1,
                                           PullEVSEStatusResponse EVSEStatus2)

            => !(EVSEStatus1 == EVSEStatus2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEStatusResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is PullEVSEStatusResponse pullEVSEStatusResponse &&
                   Equals(pullEVSEStatusResponse);

        #endregion

        #region Equals(PullEVSEStatusResponse)

        /// <summary>
        /// Compares two EVSE status for equality.
        /// </summary>
        /// <param name="PullEVSEStatusResponse">An EVSE status to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEStatusResponse PullEVSEStatusResponse)

            => !(PullEVSEStatusResponse is null) &&

               (!OperatorEVSEStatus.Any() && !PullEVSEStatusResponse.OperatorEVSEStatus.Any()) ||
                (OperatorEVSEStatus.Any() &&  PullEVSEStatusResponse.OperatorEVSEStatus.Any() && OperatorEVSEStatus.Count().Equals(PullEVSEStatusResponse.OperatorEVSEStatus.Count())) &&

                (StatusCode != null && PullEVSEStatusResponse.StatusCode != null) ||
                (StatusCode == null && PullEVSEStatusResponse.StatusCode == null && StatusCode.Equals(PullEVSEStatusResponse.StatusCode));

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
        public new class Builder : AResponse<PullEVSEStatusRequest,
                                             PullEVSEStatusResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// An enumeration of EVSE status records grouped by their operators.
            /// </summary>
            public HashSet<OperatorEVSEStatus>  OperatorEVSEStatus    { get; }

            /// <summary>
            /// The status code for this request.
            /// </summary>
            public StatusCode.Builder           StatusCode            { get; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new EVSEStatus response builder.
            /// </summary>
            /// <param name="Request">A PullEVSEStatus request.</param>
            /// <param name="OperatorEVSEStatus">An enumeration of EVSE status records grouped by their operators.</param>
            /// <param name="StatusCode">An optional status code for this request.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(PullEVSEStatusRequest            Request              = null,
                           IEnumerable<OperatorEVSEStatus>  OperatorEVSEStatus   = null,
                           StatusCode                       StatusCode           = null,
                           Process_Id?                      ProcessId            = null,
                           JObject                          CustomData           = null)

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
            ///// Compares two EVSE status responses for equality.
            ///// </summary>
            ///// <param name="EVSEStatus">An EVSE status response to compare with.</param>
            ///// <returns>True if both match; False otherwise.</returns>
            //public Boolean Equals(EVSEStatus EVSEStatus)

            //    => !(EVSEStatus is null) &&

            //       (!OperatorEVSEStatus.Any() && !EVSEStatus.OperatorEVSEStatus.Any()) ||
            //       (OperatorEVSEStatus.Any() &&  EVSEStatus.OperatorEVSEStatus.Any() && OperatorEVSEStatus.Count().Equals(EVSEStatus.OperatorEVSEStatus.Count())) &&

            //       (StatusCode != null && EVSEStatus.StatusCode != null) ||
            //       (StatusCode == null && EVSEStatus.StatusCode == null && StatusCode.Equals(EVSEStatus.StatusCode));

            #endregion

            public override PullEVSEStatusResponse ToImmutable()

                => new PullEVSEStatusResponse(Request,
                                  OperatorEVSEStatus,
                                  StatusCode,
                                  ProcessId,
                                  CustomData);

        }

        #endregion

    }

}
