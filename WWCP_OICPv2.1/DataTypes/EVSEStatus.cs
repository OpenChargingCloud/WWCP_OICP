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

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// A group of OICP operator EVSE status records or a status code.
    /// </summary>
    public class EVSEStatus
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE status records grouped by their operators.
        /// </summary>
        public IEnumerable<OperatorEVSEStatus>  OperatorEVSEStatus   { get; }

        /// <summary>
        /// The status code for this request.
        /// </summary>
        public StatusCode                       StatusCode           { get; }

        #endregion

        #region Constructor(s)

        #region EVSEStatus(OperatorEVSEStatus, StatusCode  = null)

        /// <summary>
        /// Create a new group of OICP operator EVSE status records or a status code.
        /// </summary>
        /// <param name="OperatorEVSEStatus">An enumeration of EVSE status records grouped by their operators.</param>
        /// <param name="StatusCode">An optional status code for this request.</param>
        public EVSEStatus(IEnumerable<OperatorEVSEStatus>  OperatorEVSEStatus,
                          StatusCode                       StatusCode  = null)
        {

            #region Initial checks

            if (OperatorEVSEStatus == null)
                throw new ArgumentNullException(nameof(OperatorEVSEStatus),  "The given OperatorEVSEStatus must not be null!");

            #endregion

            this.OperatorEVSEStatus  = OperatorEVSEStatus;
            this.StatusCode          = StatusCode ?? new StatusCode(StatusCodes.Success);

        }

        #endregion

        #region EVSEStatus(Code, Description = null, AdditionalInfo = null)

        /// <summary>
        /// Create a new group of OICP operator EVSE status records or a status code.
        /// </summary>
        /// <param name="Code">The result code of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="AdditionalInfo">An optional additional information.</param>
        public EVSEStatus(StatusCodes  Code,
                          String       Description     = null,
                          String       AdditionalInfo  = null)
        {

            this.OperatorEVSEStatus  = new OperatorEVSEStatus[0];
            this.StatusCode          = new StatusCode(Code,
                                                      Description,
                                                      AdditionalInfo);

        }

        #endregion

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEStatus  = "http://www.hubject.com/b2b/services/evsestatus/v2.0"
        //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        // [...]
        //
        //  <EVSEStatus:eRoamingEvseStatus>
        //
        //     <EVSEStatus:EvseStatuses>
        //
        //        <!--Zero or more repetitions:-->
        //        <EVSEStatus:OperatorEvseStatus>
        //
        //           <EVSEStatus:OperatorID>?</EVSEStatus:OperatorID>
        //
        //           <!--Optional:-->
        //           <EVSEStatus:OperatorName>?</EVSEStatus:OperatorName>
        //
        //           <!--Zero or more repetitions:-->
        //           <EVSEStatus:EvseStatusRecord>
        //              <EVSEStatus:EvseId>?</EVSEStatus:EvseId>
        //              <EVSEStatus:EvseStatus>?</EVSEStatus:EvseStatus>
        //           </EVSEStatus:EvseStatusRecord>
        //
        //        </EVSEStatus:OperatorEvseStatus>
        //
        //     </EVSEStatus:EvseStatuses>
        //
        //     <!--Optional:-->
        //     <EVSEStatus:StatusCode>
        //
        //        <CommonTypes:Code>?</CommonTypes:Code>
        //
        //        <!--Optional:-->
        //        <CommonTypes:Description>?</CommonTypes:Description>
        //
        //        <!--Optional:-->
        //        <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
        //
        //     </EVSEStatus:StatusCode>
        //
        //  </EVSEStatus:eRoamingEvseStatus>
        //
        // [...]
        //

        #endregion

        #region (static) Parse(EVSEStatusXML,  CustomOperatorEVSEStatusParser = null, CustomEVSEStatusRecordParser = null, OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="EVSEStatusXML">The XML to parse.</param>
        /// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom OperatorEVSEStatus xml elements.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord xml elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEStatus Parse(XElement                                  EVSEStatusXML,
                                       CustomParserDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusParser  = null,
                                       CustomParserDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordParser    = null,
                                       OnExceptionDelegate                       OnException                     = null)
        {

            EVSEStatus _EVSEStatus;

            if (TryParse(EVSEStatusXML,
                         out _EVSEStatus,
                         CustomOperatorEVSEStatusParser,
                         CustomEVSEStatusRecordParser,
                         OnException))

                return _EVSEStatus;

            return null;

        }

        #endregion

        #region (static) Parse(EVSEStatusText, CustomOperatorEVSEStatusParser = null, CustomEVSEStatusRecordParser = null, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="EVSEStatusText">The text to parse.</param>
        /// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom OperatorEVSEStatus xml elements.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord xml elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEStatus Parse(String                                    EVSEStatusText,
                                       CustomParserDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusParser  = null,
                                       CustomParserDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordParser    = null,
                                       OnExceptionDelegate                       OnException                     = null)
        {

            EVSEStatus _EVSEStatus;

            if (TryParse(EVSEStatusText,
                         out _EVSEStatus,
                         CustomOperatorEVSEStatusParser,
                         CustomEVSEStatusRecordParser,
                         OnException))

                return _EVSEStatus;

            return null;

        }

        #endregion

        #region (static) TryParse(EVSEStatusXML,  out EVSEStatus, CustomOperatorEVSEStatusParser = null, CustomEVSEStatusRecordParser = null, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="EVSEStatusXML">The XML to parse.</param>
        /// <param name="EVSEStatus">The parsed EVSEStatus request.</param>
        /// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom OperatorEVSEStatus xml elements.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord xml elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                  EVSEStatusXML,
                                       out EVSEStatus                            EVSEStatus,
                                       CustomParserDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusParser  = null,
                                       CustomParserDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordParser    = null,
                                       OnExceptionDelegate                       OnException                     = null)
        {

            try
            {

                if (EVSEStatusXML.Name != OICPNS.EVSEStatus + "eRoamingEvseStatus")
                {
                    EVSEStatus = null;
                    return false;
                }

                var EvseStatusesXML = EVSEStatusXML.Element(OICPNS.EVSEStatus + "EvseStatuses");
                if (EvseStatusesXML == null)
                {
                    EVSEStatus = null;
                    return false;
                }

                EVSEStatus = new EVSEStatus(

                                   EvseStatusesXML.MapElements(OICPNS.EVSEStatus + "OperatorEvseStatus",
                                                               (OperatorEvseStatusXML, onexception) =>
                                                               {

                                                                   var a = OICPv2_1.OperatorEVSEStatus.Parse(OperatorEvseStatusXML,
                                                                                                                                             CustomOperatorEVSEStatusParser,
                                                                                                                                             CustomEVSEStatusRecordParser,
                                                                                                                                             onexception);

                                                                   return a;

                                                               },
                                                               OnException),

                                   EVSEStatusXML.MapElement (OICPNS.EVSEStatus + "StatusCode",
                                                               StatusCode.Parse)

                               );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, EVSEStatusXML, e);

                EVSEStatus = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(EVSEStatusText, out EVSEStatus, CustomOperatorEVSEStatusParser = null, CustomEVSEStatusRecordParser = null, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="EVSEStatusText">The text to parse.</param>
        /// <param name="EVSEStatus">The parsed EVSE statuses request.</param>
        /// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom OperatorEVSEStatus xml elements.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord xml elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                    EVSEStatusText,
                                       out EVSEStatus                            EVSEStatus,
                                       CustomParserDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusParser  = null,
                                       CustomParserDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordParser    = null,
                                       OnExceptionDelegate                       OnException                     = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(EVSEStatusText).Root,
                             out EVSEStatus,
                             CustomOperatorEVSEStatusParser,
                             CustomEVSEStatusRecordParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, EVSEStatusText, e);
            }

            EVSEStatus = null;
            return false;

        }

        #endregion

        #region ToXML(OperatorEVSEStatusXName = null, CustomOperatorEVSEStatusSerializer = null, EVSEStatusRecordXName = null, CustomEVSEStatusRecordSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="OperatorEVSEStatusXName">The OperatorEVSEStatus XML name to use.</param>
        /// <param name="CustomOperatorEVSEStatusSerializer">A delegate to serialize custom OperatorEVSEStatus xml elements.</param>
        /// <param name="EVSEStatusRecordXName">The EVSEStatusRecord XML name to use.</param>
        /// <param name="CustomEVSEStatusRecordSerializer">A delegate to serialize custom EVSEStatusRecord xml elements.</param>
        public XElement ToXML(XName                                         OperatorEVSEStatusXName             = null,
                              CustomSerializerDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusSerializer  = null,
                              XName                                         EVSEStatusRecordXName               = null,
                              CustomSerializerDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordSerializer    = null)

            => new XElement(OICPNS.EVSEStatus + "eRoamingEvseStatus",

                   new XElement(OICPNS.EVSEStatus + "EvseStatuses",
                       OperatorEVSEStatus.Any()
                           ? OperatorEVSEStatus.SafeSelect(operatorevsestatus => operatorevsestatus.ToXML(OperatorEVSEStatusXName,
                                                                                                          CustomOperatorEVSEStatusSerializer,
                                                                                                          EVSEStatusRecordXName,
                                                                                                          CustomEVSEStatusRecordSerializer))
                           : null),

                   StatusCode != null
                       ? StatusCode.ToXML()
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="EVSEStatus1">An EVSE data.</param>
        /// <param name="EVSEStatus2">Another EVSE data.</param>
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
        /// <param name="EVSEStatus1">An EVSE data.</param>
        /// <param name="EVSEStatus2">Another EVSE data.</param>
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
        /// Compares two EVSE data for equality.
        /// </summary>
        /// <param name="EVSEStatus">An EVSE status to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEStatus EVSEStatus)
        {

            if ((Object) EVSEStatus == null)
                return false;

            return (!OperatorEVSEStatus.Any() && !EVSEStatus.OperatorEVSEStatus.Any()) ||
                    (OperatorEVSEStatus.Any() &&  EVSEStatus.OperatorEVSEStatus.Any() && OperatorEVSEStatus.Count().Equals(EVSEStatus.OperatorEVSEStatus.Count())) &&

                    (StatusCode != null && EVSEStatus.StatusCode != null) ||
                    (StatusCode == null && EVSEStatus.StatusCode == null && StatusCode.Equals(EVSEStatus.StatusCode));

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(OperatorEVSEStatus.Count() + " operator EVSE status record(s)",
                             StatusCode != null ? " -> " + StatusCode.Code.ToString() : "");

        #endregion


    }

}
