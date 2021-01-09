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
    /// The PullEVSEStatusById response.
    /// </summary>
    public class PullEVSEStatusByIdResponse : AResponse<PullEVSEStatusByIdRequest,
                                              PullEVSEStatusByIdResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE status records.
        /// </summary>
        [Mandatory]
        public IEnumerable<EVSEStatusRecord>  EVSEStatusRecords    { get; }

        /// <summary>
        /// The optional status code of this response.
        /// </summary>
        [Optional]
        public StatusCode                     StatusCode           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullEVSEStatusById response.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusById request.</param>
        /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
        /// <param name="StatusCode">An optional status code of this response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public PullEVSEStatusByIdResponse(PullEVSEStatusByIdRequest      Request,
                                          IEnumerable<EVSEStatusRecord>  EVSEStatusRecords,
                                          StatusCode                     StatusCode   = null,
                                          Process_Id?                    ProcessId    = null,
                                          JObject                        CustomData   = null)

            : base(Request,
                   DateTime.UtcNow,
                   ProcessId,
                   CustomData)

        {

            this.EVSEStatusRecords  = EVSEStatusRecords ?? throw new ArgumentNullException(nameof(EVSEStatusRecords), "The given enumeration of EVSE status records must not be null!");
            this.StatusCode         = StatusCode;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEStatus   = "http://www.hubject.com/b2b/services/evsestatus/v2.0"
        //                   xmlns:CommonTypes  = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <EVSEStatus:eRoamingEvseStatusById>
        //
        //          <!--Optional:-->
        //          <EVSEStatus:EvseStatusRecords>
        //
        //             <!--Zero or more repetitions:-->
        //             <EVSEStatus:EvseStatusRecord>
        //                <EVSEStatus:EvseId>?</EVSEStatus:EvseId>
        //                <EVSEStatus:EvseStatus>?</EVSEStatus:EvseStatus>
        //             </EVSEStatus:EvseStatusRecord>
        //
        //          </EVSEStatus:EvseStatusRecords>
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

        //#region (static) Parse   (Request, EVSEStatusByIdXML,  ..., OnException = null)

        ///// <summary>
        ///// Parse the given XML representation of an OICP EVSEStatusById request.
        ///// </summary>
        ///// <param name="Request">A EVSEStatusById request.</param>
        ///// <param name="EVSEStatusByIdXML">The XML to parse.</param>
        ///// <param name="CustomEVSEStatusByIdParser">A delegate to parse custom EVSEStatusById respones.</param>
        ///// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        ///// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        //public static PullEVSEStatusByIdResponse Parse(PullEVSEStatusByIdRequest                  Request,
        //                                   XElement                                   EVSEStatusByIdXML,
        //                                   CustomXMLParserDelegate<PullEVSEStatusByIdResponse>    CustomEVSEStatusByIdParser     = null,
        //                                   CustomXMLParserDelegate<EVSEStatusRecord>  CustomEVSEStatusRecordParser   = null,
        //                                   CustomXMLParserDelegate<StatusCode>        CustomStatusCodeParser         = null,
        //                                   OnExceptionDelegate                        OnException                    = null)
        //{

        //    if (TryParse(Request,
        //                 EVSEStatusByIdXML,
        //                 out PullEVSEStatusByIdResponse _EVSEStatusById,
        //                 CustomEVSEStatusByIdParser,
        //                 CustomEVSEStatusRecordParser,
        //                 CustomStatusCodeParser,
        //                 OnException))

        //        return _EVSEStatusById;

        //    return null;

        //}

        //#endregion

        //#region (static) Parse   (Request, EVSEStatusByIdText, ..., OnException = null)

        ///// <summary>
        ///// Parse the given text-representation of an OICP EVSEStatusById request.
        ///// </summary>
        ///// <param name="Request">A EVSEStatusById request.</param>
        ///// <param name="EVSEStatusByIdText">The text to parse.</param>
        ///// <param name="CustomEVSEStatusByIdParser">A delegate to parse custom EVSEStatusById respones.</param>
        ///// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        ///// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        //public static PullEVSEStatusByIdResponse Parse(PullEVSEStatusByIdRequest                  Request,
        //                                   String                                     EVSEStatusByIdText,
        //                                   CustomXMLParserDelegate<PullEVSEStatusByIdResponse>    CustomEVSEStatusByIdParser     = null,
        //                                   CustomXMLParserDelegate<EVSEStatusRecord>  CustomEVSEStatusRecordParser   = null,
        //                                   CustomXMLParserDelegate<StatusCode>        CustomStatusCodeParser         = null,
        //                                   OnExceptionDelegate                        OnException                    = null)
        //{

        //    if (TryParse(Request,
        //                 EVSEStatusByIdText,
        //                 out PullEVSEStatusByIdResponse _EVSEStatusById,
        //                 CustomEVSEStatusByIdParser,
        //                 CustomEVSEStatusRecordParser,
        //                 CustomStatusCodeParser,
        //                 OnException))

        //        return _EVSEStatusById;

        //    return null;

        //}

        //#endregion

        //#region (static) TryParse(Request, EVSEStatusByIdXML,  out EVSEStatusById, ..., OnException = null)

        ///// <summary>
        ///// Try to parse the given XML representation of an OICP EVSEStatusById request.
        ///// </summary>
        ///// <param name="Request">A EVSEStatusById request.</param>
        ///// <param name="EVSEStatusByIdXML">The XML to parse.</param>
        ///// <param name="EVSEStatusById">The parsed EVSEStatusById request.</param>
        ///// <param name="CustomEVSEStatusByIdParser">A delegate to parse custom EVSEStatusById respones.</param>
        ///// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        ///// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        //public static Boolean TryParse(PullEVSEStatusByIdRequest                  Request,
        //                               XElement                                   EVSEStatusByIdXML,
        //                               out PullEVSEStatusByIdResponse                         EVSEStatusById,
        //                               CustomXMLParserDelegate<PullEVSEStatusByIdResponse>    CustomEVSEStatusByIdParser     = null,
        //                               CustomXMLParserDelegate<EVSEStatusRecord>  CustomEVSEStatusRecordParser   = null,
        //                               CustomXMLParserDelegate<StatusCode>        CustomStatusCodeParser         = null,
        //                               OnExceptionDelegate                        OnException                    = null)
        //{

        //    try
        //    {

        //        if (EVSEStatusByIdXML.Name != OICPNS.EVSEStatus + "eRoamingEvseStatusById")
        //        {
        //            EVSEStatusById = null;
        //            return false;
        //        }

        //        var _EVSEStatusRecordsXML  = EVSEStatusByIdXML.Element   (OICPNS.EVSEStatus + "EvseStatusRecords");

        //        EVSEStatusById = new EVSEStatusById(

        //                             Request,

        //                             _EVSEStatusRecordsXML != null
        //                                 ? _EVSEStatusRecordsXML.MapElementsOrFail(OICPNS.EVSEStatus + "EvseStatusRecord",
        //                                                                           (s, e) => EVSEStatusRecord.Parse(s, CustomEVSEStatusRecordParser, e),
        //                                                                           OnException)
        //                                 : null,

        //                             EVSEStatusByIdXML.MapElement(OICPNS.EVSEStatus + "StatusCode",
        //                                                          (xml, e) =>  OICPv2_2.StatusCode.Parse(xml,
        //                                                                                                 CustomStatusCodeParser,
        //                                                                                                 e),
        //                                                          OnException));


        //        if (CustomEVSEStatusByIdParser != null)
        //            EVSEStatusById = CustomEVSEStatusByIdParser(EVSEStatusByIdXML,
        //                                                        EVSEStatusById);

        //        return true;

        //    }
        //    catch (Exception e)
        //    {

        //        OnException?.Invoke(DateTime.UtcNow, EVSEStatusByIdXML, e);

        //        EVSEStatusById = null;
        //        return false;

        //    }

        //}

        //#endregion

        //#region (static) TryParse(Request, EVSEStatusByIdText, out EVSEStatusById, ..., OnException = null)

        ///// <summary>
        ///// Try to parse the given text-representation of an OICP EVSEStatusById request.
        ///// </summary>
        ///// <param name="Request">A EVSEStatusById request.</param>
        ///// <param name="EVSEStatusByIdText">The text to parse.</param>
        ///// <param name="EVSEStatusById">The parsed EVSEStatusById request.</param>
        ///// <param name="CustomEVSEStatusByIdParser">A delegate to parse custom EVSEStatusById respones.</param>
        ///// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        ///// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        //public static Boolean TryParse(PullEVSEStatusByIdRequest                  Request,
        //                               String                                     EVSEStatusByIdText,
        //                               out PullEVSEStatusByIdResponse                         EVSEStatusById,
        //                               CustomXMLParserDelegate<PullEVSEStatusByIdResponse>    CustomEVSEStatusByIdParser     = null,
        //                               CustomXMLParserDelegate<EVSEStatusRecord>  CustomEVSEStatusRecordParser   = null,
        //                               CustomXMLParserDelegate<StatusCode>        CustomStatusCodeParser         = null,
        //                               OnExceptionDelegate                        OnException                    = null)
        //{

        //    try
        //    {

        //        if (TryParse(Request,
        //                     XDocument.Parse(EVSEStatusByIdText).Root,
        //                     out EVSEStatusById,
        //                     CustomEVSEStatusByIdParser,
        //                     CustomEVSEStatusRecordParser,
        //                     CustomStatusCodeParser,
        //                     OnException))

        //            return true;

        //    }
        //    catch (Exception e)
        //    {
        //        OnException?.Invoke(DateTime.UtcNow, EVSEStatusByIdText, e);
        //    }

        //    EVSEStatusById = null;
        //    return false;

        //}

        //#endregion

        //#region ToXML(CustomEVSEStatusByIdSerializer = null, XName = null, EVSEStatusRecordXName = null, CustomEVSEStatusRecordSerializer = null)

        ///// <summary>
        ///// Return a XML representation of this object.
        ///// </summary>
        ///// <param name="CustomEVSEStatusByIdSerializer">A delegate to serialize custom EVSEStatusById XML elements.</param>
        ///// <param name="XName">The XML name to use.</param>
        ///// <param name="EVSEStatusRecordXName">The EVSEStatusRecord XML name to use.</param>
        ///// <param name="CustomEVSEStatusRecordSerializer">A delegate to serialize custom EVSEStatusRecord XML elements.</param>
        //public XElement ToXML(CustomXMLSerializerDelegate<PullEVSEStatusByIdResponse>    CustomEVSEStatusByIdSerializer    = null,
        //                      XName                                          XName                             = null,
        //                      XName                                          EVSEStatusRecordXName             = null,
        //                      CustomXMLSerializerDelegate<EVSEStatusRecord>  CustomEVSEStatusRecordSerializer  = null)

        //{

        //    var XML = new XElement(XName ?? OICPNS.EVSEStatus + "eRoamingEvseStatusById",

        //                  EVSEStatusRecords.Any()
        //                      ? EVSEStatusRecords.SafeSelect(record => record.ToXML(EVSEStatusRecordXName,
        //                                                                            CustomEVSEStatusRecordSerializer))
        //                      : null,

        //                  StatusCode?.ToXML(OICPNS.EVSEStatus + "StatusCode")

        //              );


        //    return CustomEVSEStatusByIdSerializer != null
        //               ? CustomEVSEStatusByIdSerializer(this, XML)
        //               : XML;

        //}

        //#endregion


        #region Operator overloading

        #region Operator == (PullEVSEStatusByIdResponse1, PullEVSEStatusByIdResponse2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PullEVSEStatusByIdResponse1">An EVSE status by id request.</param>
        /// <param name="PullEVSEStatusByIdResponse2">Another EVSE status by id request.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (PullEVSEStatusByIdResponse PullEVSEStatusByIdResponse1,
                                           PullEVSEStatusByIdResponse PullEVSEStatusByIdResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PullEVSEStatusByIdResponse1, PullEVSEStatusByIdResponse2))
                return true;

            // If one is null, but not both, return false.
            if (PullEVSEStatusByIdResponse1 is null || PullEVSEStatusByIdResponse2 is null)
                return false;

            return PullEVSEStatusByIdResponse1.Equals(PullEVSEStatusByIdResponse2);

        }

        #endregion

        #region Operator != (PullEVSEStatusByIdResponse1, PullEVSEStatusByIdResponse2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PullEVSEStatusByIdResponse1">An EVSE status by id request.</param>
        /// <param name="PullEVSEStatusByIdResponse2">Another EVSE status by id request.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (PullEVSEStatusByIdResponse PullEVSEStatusByIdResponse1,
                                           PullEVSEStatusByIdResponse PullEVSEStatusByIdResponse2)

            => !(PullEVSEStatusByIdResponse1 == PullEVSEStatusByIdResponse2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEStatusByIdResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is PullEVSEStatusByIdResponse pullEVSEStatusByIdResponse &&
                   Equals(pullEVSEStatusByIdResponse);

        #endregion

        #region Equals(PullEVSEStatusByIdResponse)

        /// <summary>
        /// Compares two PullEVSEStatusById responses for equality.
        /// </summary>
        /// <param name="PullEVSEStatusByIdResponse">A PullEVSEStatusById response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEStatusByIdResponse PullEVSEStatusByIdResponse)

            => !(PullEVSEStatusByIdResponse is null) &&

               (!EVSEStatusRecords.Any() && !PullEVSEStatusByIdResponse.EVSEStatusRecords.Any()) ||
                (EVSEStatusRecords.Any() &&  PullEVSEStatusByIdResponse.EVSEStatusRecords.Any() && EVSEStatusRecords.Count().Equals(PullEVSEStatusByIdResponse.EVSEStatusRecords.Count())) &&

                (StatusCode != null && PullEVSEStatusByIdResponse.StatusCode != null) ||
                (StatusCode == null && PullEVSEStatusByIdResponse.StatusCode == null && StatusCode.Equals(PullEVSEStatusByIdResponse.StatusCode));

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

                return EVSEStatusRecords.Aggregate(0, (hashCode, evseStatusRecord) => hashCode ^ evseStatusRecord.GetHashCode()) ^
                       StatusCode?.GetHashCode() ?? 0;

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVSEStatusRecords.Count() + " EVSE status record(s)",
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
                           EVSEStatusRecords,
                           StatusCode,
                           ProcessId,
                           CustomData);

        #endregion

        #region (class) Builder

        /// <summary>
        /// An EVSEStatusById response builder.
        /// </summary>
        public new class Builder : AResponse<PullEVSEStatusByIdRequest,
                                             PullEVSEStatusByIdResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// An enumeration of EVSE status records.
            /// </summary>
            public HashSet<EVSEStatusRecord>  EVSEStatusRecords    { get; }

            /// <summary>
            /// The status code for this request.
            /// </summary>
            public StatusCode.Builder         StatusCode           { get; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new PullEVSEStatusById response builder.
            /// </summary>
            /// <param name="Request">A PullEVSEStatusById request.</param>
            /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
            /// <param name="StatusCode">An optional status code of this response.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(PullEVSEStatusByIdRequest      Request = null,
                           IEnumerable<EVSEStatusRecord>  EVSEStatusRecords   = null,
                           StatusCode                     StatusCode          = null,
                           Process_Id?                    ProcessId           = null,
                           JObject                        CustomData          = null)

                : base(Request,
                       DateTime.UtcNow,
                       ProcessId,
                       CustomData)

            {

                this.EVSEStatusRecords  = EVSEStatusRecords != null ? new HashSet<EVSEStatusRecord>(EVSEStatusRecords) : new HashSet<EVSEStatusRecord>();
                this.StatusCode         = StatusCode        != null ? StatusCode.ToBuilder()                           : new StatusCode.Builder();

            }

            #endregion


            #region Equals(EVSEStatusById)

            ///// <summary>
            ///// Compares two EVSE status by id responses for equality.
            ///// </summary>
            ///// <param name="EVSEStatusById">An EVSE status by id response to compare with.</param>
            ///// <returns>True if both match; False otherwise.</returns>
            //public override Boolean Equals(EVSEStatusById EVSEStatusById)
            //{

            //    if ((Object) EVSEStatusById == null)
            //        return false;

            //    return (!EVSEStatusRecords.Any() && !EVSEStatusById.EVSEStatusRecords.Any()) ||
            //            (EVSEStatusRecords.Any() && EVSEStatusById.EVSEStatusRecords.Any() && EVSEStatusRecords.Count().Equals(EVSEStatusById.EVSEStatusRecords.Count())) &&

            //            (StatusCode != null && EVSEStatusById.StatusCode != null) ||
            //            (StatusCode == null && EVSEStatusById.StatusCode == null && StatusCode.Equals(EVSEStatusById.StatusCode));

            //}

            #endregion

            public override PullEVSEStatusByIdResponse ToImmutable()

                => new PullEVSEStatusByIdResponse(Request,
                                                  EVSEStatusRecords,
                                                  StatusCode,
                                                  ProcessId,
                                                  CustomData);

        }

        #endregion

    }

}
