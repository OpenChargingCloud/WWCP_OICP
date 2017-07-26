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
    /// A group of OICP operator EVSE status records or a status code.
    /// </summary>
    public class EVSEStatusById : AResponse<PullEVSEStatusByIdRequest,
                                            EVSEStatusById>
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE status records.
        /// </summary>
        public IEnumerable<EVSEStatusRecord>  EVSEStatusRecords   { get; }

        /// <summary>
        /// The status code for this request.
        /// </summary>
        public StatusCode?                    StatusCode          { get; }

        #endregion

        #region Constructor(s)

        #region EVSEStatusById(Request, OperatorEVSEStatus, StatusCode = null, CustomData = null)

        /// <summary>
        /// Create a new group of OICP operator EVSE status records or a status code.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusById request.</param>
        /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
        /// <param name="StatusCode">An optional status code for this request.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public EVSEStatusById(PullEVSEStatusByIdRequest            Request,
                              IEnumerable<EVSEStatusRecord>        EVSEStatusRecords,
                              StatusCode?                          StatusCode  = null,
                              IReadOnlyDictionary<String, Object>  CustomData  = null)

            : base(Request,
                   CustomData)

        {

            #region Initial checks

            if (EVSEStatusRecords == null)
                throw new ArgumentNullException(nameof(EVSEStatusRecords),  "The given enumeration of EVSE status records must not be null!");

            #endregion

            this.EVSEStatusRecords  = EVSEStatusRecords;
            this.StatusCode         = StatusCode;

        }

        #endregion

        #region EVSEStatusById(Code, Description = null, AdditionalInfo = null, CustomData = null)

        /// <summary>
        /// Create a new group of OICP operator EVSE status records or a status code.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusById request.</param>
        /// <param name="Code">The result code of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="AdditionalInfo">An optional additional information.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public EVSEStatusById(PullEVSEStatusByIdRequest            Request,
                              StatusCodes                          Code,
                              String                               Description     = null,
                              String                               AdditionalInfo  = null,
                              IReadOnlyDictionary<String, Object>  CustomData      = null)

            : this(Request,
                   new EVSEStatusRecord[0],
                   new StatusCode(Code,
                                  Description,
                                  AdditionalInfo),
                   CustomData)

        { }

        #endregion

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

        #region (static) Parse   (Request, EVSEStatusByIdXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP EVSEStatusById request.
        /// </summary>
        /// <param name="Request">A EVSEStatusById request.</param>
        /// <param name="EVSEStatusByIdXML">The XML to parse.</param>
        /// <param name="CustomEVSEStatusByIdParser">A delegate to parse custom EVSEStatusById respones.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEStatusById Parse(PullEVSEStatusByIdRequest                  Request,
                                           XElement                                   EVSEStatusByIdXML,
                                           CustomXMLParserDelegate<EVSEStatusById>    CustomEVSEStatusByIdParser     = null,
                                           CustomXMLParserDelegate<EVSEStatusRecord>  CustomEVSEStatusRecordParser   = null,
                                           CustomXMLParserDelegate<StatusCode>        CustomStatusCodeParser         = null,
                                           OnExceptionDelegate                        OnException                    = null)
        {

            if (TryParse(Request,
                         EVSEStatusByIdXML,
                         out EVSEStatusById _EVSEStatusById,
                         CustomEVSEStatusByIdParser,
                         CustomEVSEStatusRecordParser,
                         CustomStatusCodeParser,
                         OnException))

                return _EVSEStatusById;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, EVSEStatusByIdText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP EVSEStatusById request.
        /// </summary>
        /// <param name="Request">A EVSEStatusById request.</param>
        /// <param name="EVSEStatusByIdText">The text to parse.</param>
        /// <param name="CustomEVSEStatusByIdParser">A delegate to parse custom EVSEStatusById respones.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEStatusById Parse(PullEVSEStatusByIdRequest                  Request,
                                           String                                     EVSEStatusByIdText,
                                           CustomXMLParserDelegate<EVSEStatusById>    CustomEVSEStatusByIdParser     = null,
                                           CustomXMLParserDelegate<EVSEStatusRecord>  CustomEVSEStatusRecordParser   = null,
                                           CustomXMLParserDelegate<StatusCode>        CustomStatusCodeParser         = null,
                                           OnExceptionDelegate                        OnException                    = null)
        {

            if (TryParse(Request,
                         EVSEStatusByIdText,
                         out EVSEStatusById _EVSEStatusById,
                         CustomEVSEStatusByIdParser,
                         CustomEVSEStatusRecordParser,
                         CustomStatusCodeParser,
                         OnException))

                return _EVSEStatusById;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, EVSEStatusByIdXML,  out EVSEStatusById, ..., OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP EVSEStatusById request.
        /// </summary>
        /// <param name="Request">A EVSEStatusById request.</param>
        /// <param name="EVSEStatusByIdXML">The XML to parse.</param>
        /// <param name="EVSEStatusById">The parsed EVSEStatusById request.</param>
        /// <param name="CustomEVSEStatusByIdParser">A delegate to parse custom EVSEStatusById respones.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(PullEVSEStatusByIdRequest                  Request,
                                       XElement                                   EVSEStatusByIdXML,
                                       out EVSEStatusById                         EVSEStatusById,
                                       CustomXMLParserDelegate<EVSEStatusById>    CustomEVSEStatusByIdParser     = null,
                                       CustomXMLParserDelegate<EVSEStatusRecord>  CustomEVSEStatusRecordParser   = null,
                                       CustomXMLParserDelegate<StatusCode>        CustomStatusCodeParser         = null,
                                       OnExceptionDelegate                        OnException                    = null)
        {

            try
            {

                if (EVSEStatusByIdXML.Name != OICPNS.EVSEStatus + "eRoamingEvseStatusById")
                {
                    EVSEStatusById = null;
                    return false;
                }

                var _EVSEStatusRecordsXML  = EVSEStatusByIdXML.Element   (OICPNS.EVSEStatus + "EvseStatusRecords");

                EVSEStatusById = new EVSEStatusById(

                                     Request,

                                     _EVSEStatusRecordsXML != null
                                         ? _EVSEStatusRecordsXML.MapElementsOrFail(OICPNS.EVSEStatus + "EvseStatusRecord",
                                                                                   (s, e) => EVSEStatusRecord.Parse(s, CustomEVSEStatusRecordParser, e),
                                                                                   OnException)
                                         : null,

                                     EVSEStatusByIdXML.MapElement(OICPNS.EVSEStatus + "StatusCode",
                                                                  (xml, e) =>  OICPv2_1.StatusCode.Parse(xml,
                                                                                                         CustomStatusCodeParser,
                                                                                                         e),
                                                                  OnException));


                if (CustomEVSEStatusByIdParser != null)
                    EVSEStatusById = CustomEVSEStatusByIdParser(EVSEStatusByIdXML,
                                                                EVSEStatusById);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, EVSEStatusByIdXML, e);

                EVSEStatusById = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, EVSEStatusByIdText, out EVSEStatusById, ..., OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP EVSEStatusById request.
        /// </summary>
        /// <param name="Request">A EVSEStatusById request.</param>
        /// <param name="EVSEStatusByIdText">The text to parse.</param>
        /// <param name="EVSEStatusById">The parsed EVSEStatusById request.</param>
        /// <param name="CustomEVSEStatusByIdParser">A delegate to parse custom EVSEStatusById respones.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(PullEVSEStatusByIdRequest                  Request,
                                       String                                     EVSEStatusByIdText,
                                       out EVSEStatusById                         EVSEStatusById,
                                       CustomXMLParserDelegate<EVSEStatusById>    CustomEVSEStatusByIdParser     = null,
                                       CustomXMLParserDelegate<EVSEStatusRecord>  CustomEVSEStatusRecordParser   = null,
                                       CustomXMLParserDelegate<StatusCode>        CustomStatusCodeParser         = null,
                                       OnExceptionDelegate                        OnException                    = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(EVSEStatusByIdText).Root,
                             out EVSEStatusById,
                             CustomEVSEStatusByIdParser,
                             CustomEVSEStatusRecordParser,
                             CustomStatusCodeParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, EVSEStatusByIdText, e);
            }

            EVSEStatusById = null;
            return false;

        }

        #endregion

        #region ToXML(CustomEVSEStatusByIdSerializer = null, XName = null, EVSEStatusRecordXName = null, CustomEVSEStatusRecordSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomEVSEStatusByIdSerializer">A delegate to serialize custom EVSEStatusById XML elements.</param>
        /// <param name="XName">The XML name to use.</param>
        /// <param name="EVSEStatusRecordXName">The EVSEStatusRecord XML name to use.</param>
        /// <param name="CustomEVSEStatusRecordSerializer">A delegate to serialize custom EVSEStatusRecord XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<EVSEStatusById>    CustomEVSEStatusByIdSerializer    = null,
                              XName                                          XName                             = null,
                              XName                                          EVSEStatusRecordXName             = null,
                              CustomXMLSerializerDelegate<EVSEStatusRecord>  CustomEVSEStatusRecordSerializer  = null)

        {

            var XML = new XElement(XName ?? OICPNS.EVSEStatus + "eRoamingEvseStatusById",

                          EVSEStatusRecords.Any()
                              ? EVSEStatusRecords.SafeSelect(record => record.ToXML(EVSEStatusRecordXName,
                                                                                    CustomEVSEStatusRecordSerializer))
                              : null,

                          StatusCode?.ToXML(OICPNS.EVSEStatus + "StatusCode")

                      );


            return CustomEVSEStatusByIdSerializer != null
                       ? CustomEVSEStatusByIdSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSEStatusById1, EVSEStatusById2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusById1">An EVSE status by id request.</param>
        /// <param name="EVSEStatusById2">Another EVSE status by id request.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEStatusById EVSEStatusById1, EVSEStatusById EVSEStatusById2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEStatusById1, EVSEStatusById2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEStatusById1 == null) || ((Object) EVSEStatusById2 == null))
                return false;

            return EVSEStatusById1.Equals(EVSEStatusById2);

        }

        #endregion

        #region Operator != (EVSEStatusById1, EVSEStatusById2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusById1">An EVSE status by id request.</param>
        /// <param name="EVSEStatusById2">Another EVSE status by id request.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEStatusById EVSEStatusById1, EVSEStatusById EVSEStatusById2)
            => !(EVSEStatusById1 == EVSEStatusById2);

        #endregion

        #endregion

        #region IEquatable<EVSEStatusById> Members

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

            var EVSEStatusById = Object as EVSEStatusById;
            if ((Object) EVSEStatusById == null)
                return false;

            return this.Equals(EVSEStatusById);

        }

        #endregion

        #region Equals(EVSEStatusById)

        /// <summary>
        /// Compares two EVSE status by id requests for equality.
        /// </summary>
        /// <param name="EVSEStatusById">An EVSE status by id request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(EVSEStatusById EVSEStatusById)
        {

            if ((Object) EVSEStatusById == null)
                return false;

            return (!EVSEStatusRecords.Any() && !EVSEStatusById.EVSEStatusRecords.Any()) ||
                    (EVSEStatusRecords.Any() && EVSEStatusById.EVSEStatusRecords.Any() && EVSEStatusRecords.Count().Equals(EVSEStatusById.EVSEStatusRecords.Count())) &&

                    (StatusCode != null && EVSEStatusById.StatusCode != null) ||
                    (StatusCode == null && EVSEStatusById.StatusCode == null && StatusCode.Equals(EVSEStatusById.StatusCode));

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

                return (EVSEStatusRecords.Any()
                           ? EVSEStatusRecords.GetHashCode() * 5
                           : 0) ^

                       StatusCode.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVSEStatusRecords.Count() + " EVSE status record(s)",

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
        /// An EVSEStatusById response builder.
        /// </summary>
        public class Builder : AResponseBuilder<PullEVSEStatusByIdRequest,
                                                EVSEStatusById>
        {

            #region Properties

            /// <summary>
            /// An enumeration of EVSE status records.
            /// </summary>
            public IEnumerable<EVSEStatusRecord>  EVSEStatusRecords   { get; set; }

            /// <summary>
            /// The status code for this request.
            /// </summary>
            public StatusCode?                    StatusCode          { get; set; }

            #endregion

            #region Constructor(s)

            #region Builder(Request,        CustomData = null)

            /// <summary>
            /// Create a new EVSEStatusById response builder.
            /// </summary>
            /// <param name="Request">A PullEVSEStatusById request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(PullEVSEStatusByIdRequest            Request,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(Request,
                       CustomData)

            { }

            #endregion

            #region Builder(EVSEStatusById, CustomData = null)

            /// <summary>
            /// Create a new EVSEStatusById response builder.
            /// </summary>
            /// <param name="EVSEStatusById">An EVSEStatusById response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(EVSEStatusById                       EVSEStatusById,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(EVSEStatusById?.Request,
                       EVSEStatusById.HasCustomData
                           ? CustomData != null && CustomData.Any()
                                 ? EVSEStatusById.CustomValues.Concat(CustomData)
                                 : EVSEStatusById.CustomValues
                           : CustomData)

            {

                if (EVSEStatusById != null)
                {

                    this.EVSEStatusRecords  = EVSEStatusById.EVSEStatusRecords;
                    this.StatusCode         = EVSEStatusById.StatusCode;

                }

            }

            #endregion

            #endregion


            #region Equals(EVSEStatusById)

            /// <summary>
            /// Compares two EVSE status by id responses for equality.
            /// </summary>
            /// <param name="EVSEStatusById">An EVSE status by id response to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(EVSEStatusById EVSEStatusById)
            {

                if ((Object) EVSEStatusById == null)
                    return false;

                return (!EVSEStatusRecords.Any() && !EVSEStatusById.EVSEStatusRecords.Any()) ||
                        (EVSEStatusRecords.Any() && EVSEStatusById.EVSEStatusRecords.Any() && EVSEStatusRecords.Count().Equals(EVSEStatusById.EVSEStatusRecords.Count())) &&

                        (StatusCode != null && EVSEStatusById.StatusCode != null) ||
                        (StatusCode == null && EVSEStatusById.StatusCode == null && StatusCode.Equals(EVSEStatusById.StatusCode));

            }

            #endregion

            public override EVSEStatusById ToImmutable

                => new EVSEStatusById(Request,
                                      EVSEStatusRecords,
                                      StatusCode,
                                      ImmutableCustomData);

        }

        #endregion

    }

}
