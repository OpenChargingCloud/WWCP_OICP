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

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// A group of OICP EVSE status records.
    /// </summary>
    public class OperatorEVSEStatus : ACustomData,
                                      IEquatable<OperatorEVSEStatus>,
                                      IComparable<OperatorEVSEStatus>,
                                      IComparable
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE status records.
        /// </summary>
        public IEnumerable<EVSEStatusRecord>  EVSEStatusRecords   { get; }

        /// <summary>
        /// The unqiue identification of the EVSE operator maintaining the given EVSE status records.
        /// </summary>
        public Operator_Id                    OperatorId          { get; }

        /// <summary>
        /// The optional name of the EVSE operator maintaining the given EVSE status records.
        /// </summary>
        public String                         OperatorName        { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group of OICP EVSE status records.
        /// </summary>
        /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
        /// <param name="OperatorId">The unqiue identification of the EVSE operator maintaining the given EVSE status records.</param>
        /// <param name="OperatorName">An optional name of the EVSE operator maintaining the given EVSE status records.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public OperatorEVSEStatus(IEnumerable<EVSEStatusRecord>        EVSEStatusRecords,
                                  Operator_Id                          OperatorId,
                                  String                               OperatorName   = null,
                                  IReadOnlyDictionary<String, Object>  CustomData     = null)

            : base(CustomData)

        {

            #region Initial checks

            if (EVSEStatusRecords == null || !EVSEStatusRecords.Any())
                throw new ArgumentNullException(nameof(EVSEStatusRecords), "The given enumeration of EVSE status records must not be null or empty!");

            if (OperatorName != null)
                OperatorName = OperatorName.Trim();

            #endregion

            this.EVSEStatusRecords  = EVSEStatusRecords;
            this.OperatorId         = OperatorId;
            this.OperatorName       = OperatorName.SubstringMax(100);

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEStatus  = "http://www.hubject.com/b2b/services/evsestatus/v2.1"
        //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.1">
        //
        // [...]
        //
        // <!--Zero or more repetitions:-->
        // <EVSEStatus:OperatorEvseStatus>
        //
        //    <EVSEStatus:OperatorID>?</EVSEStatus:OperatorID>
        //
        //    <!--Optional:-->
        //    <EVSEStatus:OperatorName>?</EVSEStatus:OperatorName>
        //
        //    <!--Zero or more repetitions:-->
        //    <EVSEStatus:EvseStatusRecord>
        //       <EVSEStatus:EvseId>?</EVSEStatus:EvseId>
        //       <EVSEStatus:EvseStatus>?</EVSEStatus:EvseStatus>
        //    </EVSEStatus:EvseStatusRecord>
        //
        // </EVSEStatus:OperatorEvseStatus>
        //
        // [...]
        //

        #endregion

        #region (static) Parse(OperatorEVSEStatusXML,  CustomOperatorEVSEStatusParser = null, CustomEVSEStatusRecordParser = null, OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP operator EVSE status request.
        /// </summary>
        /// <param name="OperatorEVSEStatusXML">The XML to parse.</param>
        /// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom OperatorEVSEStatus XML elements.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static OperatorEVSEStatus Parse(XElement                                     OperatorEVSEStatusXML,
                                               CustomXMLParserDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusParser   = null,
                                               CustomXMLParserDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordParser     = null,
                                               OnExceptionDelegate                          OnException                      = null)
        {

            if (TryParse(OperatorEVSEStatusXML,
                         out OperatorEVSEStatus operatorEVSEStatus,
                         CustomOperatorEVSEStatusParser,
                         CustomEVSEStatusRecordParser,
                         OnException))

                return operatorEVSEStatus;

            return null;

        }

        #endregion

        #region (static) Parse(OperatorEVSEStatusText, CustomOperatorEVSEStatusParser = null, CustomEVSEStatusRecordParser = null, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP operator EVSE status request.
        /// </summary>
        /// <param name="OperatorEVSEStatusText">The text to parse.</param>
        /// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom OperatorEVSEStatus XML elements.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static OperatorEVSEStatus Parse(String                                       OperatorEVSEStatusText,
                                               CustomXMLParserDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusParser   = null,
                                               CustomXMLParserDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordParser     = null,
                                               OnExceptionDelegate                          OnException                      = null)
        {

            if (TryParse(OperatorEVSEStatusText,
                         out OperatorEVSEStatus operatorEVSEStatus,
                         CustomOperatorEVSEStatusParser,
                         CustomEVSEStatusRecordParser,
                         OnException))

                return operatorEVSEStatus;

            return null;

        }

        #endregion

        #region (static) TryParse(OperatorEVSEStatusXML,  out OperatorEVSEStatus, CustomOperatorEVSEStatusParser = null, CustomEVSEStatusRecordParser = null, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP operator EVSE status request.
        /// </summary>
        /// <param name="OperatorEVSEStatusXML">The XML to parse.</param>
        /// <param name="OperatorEVSEStatus">The parsed operator EVSE status request.</param>
        /// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom OperatorEVSEStatus XML elements.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                     OperatorEVSEStatusXML,
                                       out OperatorEVSEStatus                       OperatorEVSEStatus,
                                       CustomXMLParserDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusParser   = null,
                                       CustomXMLParserDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordParser     = null,
                                       OnExceptionDelegate                          OnException                      = null)
        {

            try
            {

                if (OperatorEVSEStatusXML.Name != OICPNS.EVSEStatus + "OperatorEvseStatus")
                {
                    OperatorEVSEStatus = null;
                    return false;
                }

                OperatorEVSEStatus = new OperatorEVSEStatus(

                                         OperatorEVSEStatusXML.MapElements          (OICPNS.EVSEStatus + "EvseStatusRecord",
                                                                                     (xml, e) => EVSEStatusRecord.Parse(xml,
                                                                                                                        CustomEVSEStatusRecordParser,
                                                                                                                        e),
                                                                                     OnException).
                                                                                     Where(operatorevsestatus => operatorevsestatus != null),

                                         OperatorEVSEStatusXML.MapValueOrFail       (OICPNS.EVSEStatus + "OperatorID",
                                                                                     Operator_Id.Parse),

                                         OperatorEVSEStatusXML.ElementValueOrDefault(OICPNS.EVSEStatus + "OperatorName")

                                     );


                if (CustomOperatorEVSEStatusParser != null)
                    OperatorEVSEStatus = CustomOperatorEVSEStatusParser(OperatorEVSEStatusXML, OperatorEVSEStatus);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, OperatorEVSEStatusXML, e);

                OperatorEVSEStatus = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(OperatorEVSEStatusText, out OperatorEVSEStatus, CustomOperatorEVSEStatusParser = null, CustomEVSEStatusRecordParser = null, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP operator EVSE status request.
        /// </summary>
        /// <param name="OperatorEVSEStatusText">The text to parse.</param>
        /// <param name="OperatorEVSEStatus">The parsed operator EVSE status request.</param>
        /// <param name="CustomOperatorEVSEStatusParser">A delegate to parse custom OperatorEVSEStatus XML elements.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom EVSEStatusRecord XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                       OperatorEVSEStatusText,
                                       out OperatorEVSEStatus                       OperatorEVSEStatus,
                                       CustomXMLParserDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusParser   = null,
                                       CustomXMLParserDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordParser     = null,
                                       OnExceptionDelegate                          OnException                      = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(OperatorEVSEStatusText).Root,
                             out OperatorEVSEStatus,
                             CustomOperatorEVSEStatusParser,
                             CustomEVSEStatusRecordParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, OperatorEVSEStatusText, e);
            }

            OperatorEVSEStatus = null;
            return false;

        }

        #endregion

        #region ToXML(OperatorEVSEStatusXName = null, CustomOperatorEVSEStatusSerializer = null, EVSEStatusRecordXName = null, CustomEVSEStatusRecordSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="OperatorEVSEStatusXName">The OperatorEVSEStatus XML name to use.</param>
        /// <param name="CustomOperatorEVSEStatusSerializer">A delegate to serialize custom OperatorEVSEStatus XML elements.</param>
        /// <param name="EVSEStatusRecordXName">The EVSEStatusRecord XML name to use.</param>
        /// <param name="CustomEVSEStatusRecordSerializer">A delegate to serialize custom EVSEStatusRecord XML elements.</param>
        public XElement ToXML(XName                                            OperatorEVSEStatusXName              = null,
                              CustomXMLSerializerDelegate<OperatorEVSEStatus>  CustomOperatorEVSEStatusSerializer   = null,
                              XName                                            EVSEStatusRecordXName                = null,
                              CustomXMLSerializerDelegate<EVSEStatusRecord>    CustomEVSEStatusRecordSerializer     = null)

        {

            var xml = new XElement(OperatorEVSEStatusXName ?? OICPNS.EVSEStatus + "OperatorEvseStatus",

                          new XElement(OICPNS.EVSEStatus + "OperatorID",          OperatorId.ToString()),

                          OperatorName.IsNotNullOrEmpty()
                              ? new XElement(OICPNS.EVSEStatus + "OperatorName",  OperatorName)
                              : null,

                          EVSEStatusRecords.Any()
                              ? EVSEStatusRecords.SafeSelect(evsestatusrecord => evsestatusrecord.ToXML(EVSEStatusRecordXName,
                                                                                                        CustomEVSEStatusRecordSerializer))
                              : null);

            return CustomOperatorEVSEStatusSerializer != null
                       ? CustomOperatorEVSEStatusSerializer(this, xml)
                       : xml;

        }

        #endregion


        #region Operator overloading

        #region Operator == (OperatorEVSEStatus1, OperatorEVSEStatus2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="OperatorEVSEStatus1">An operator EVSE status.</param>
        /// <param name="OperatorEVSEStatus2">Another operator EVSE status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (OperatorEVSEStatus OperatorEVSEStatus1, OperatorEVSEStatus OperatorEVSEStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(OperatorEVSEStatus1, OperatorEVSEStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) OperatorEVSEStatus1 == null) || ((Object) OperatorEVSEStatus2 == null))
                return false;

            return OperatorEVSEStatus1.Equals(OperatorEVSEStatus2);

        }

        #endregion

        #region Operator != (OperatorEVSEStatus1, OperatorEVSEStatus2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="OperatorEVSEStatus1">An operator EVSE status.</param>
        /// <param name="OperatorEVSEStatus2">Another operator EVSE status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (OperatorEVSEStatus OperatorEVSEStatus1, OperatorEVSEStatus OperatorEVSEStatus2)

            => !(OperatorEVSEStatus1 == OperatorEVSEStatus2);

        #endregion

        #region Operator <  (OperatorEVSEStatus1, OperatorEVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEStatus1">An operator EVSE status.</param>
        /// <param name="OperatorEVSEStatus2">Another operator EVSE status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (OperatorEVSEStatus OperatorEVSEStatus1, OperatorEVSEStatus OperatorEVSEStatus2)
        {

            if ((Object) OperatorEVSEStatus1 == null)
                throw new ArgumentNullException(nameof(OperatorEVSEStatus1), "The given OperatorEVSEStatus1 must not be null!");

            return OperatorEVSEStatus1.CompareTo(OperatorEVSEStatus2) < 0;

        }

        #endregion

        #region Operator <= (OperatorEVSEStatus1, OperatorEVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEStatus1">An operator EVSE status.</param>
        /// <param name="OperatorEVSEStatus2">Another operator EVSE status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (OperatorEVSEStatus OperatorEVSEStatus1, OperatorEVSEStatus OperatorEVSEStatus2)
            => !(OperatorEVSEStatus1 > OperatorEVSEStatus2);

        #endregion

        #region Operator >  (OperatorEVSEStatus1, OperatorEVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEStatus1">An operator EVSE status.</param>
        /// <param name="OperatorEVSEStatus2">Another operator EVSE status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (OperatorEVSEStatus OperatorEVSEStatus1, OperatorEVSEStatus OperatorEVSEStatus2)
        {

            if ((Object) OperatorEVSEStatus1 == null)
                throw new ArgumentNullException(nameof(OperatorEVSEStatus1), "The given OperatorEVSEStatus1 must not be null!");

            return OperatorEVSEStatus1.CompareTo(OperatorEVSEStatus2) > 0;

        }

        #endregion

        #region Operator >= (OperatorEVSEStatus1, OperatorEVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEStatus1">An operator EVSE status.</param>
        /// <param name="OperatorEVSEStatus2">Another operator EVSE status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (OperatorEVSEStatus OperatorEVSEStatus1, OperatorEVSEStatus OperatorEVSEStatus2)
            => !(OperatorEVSEStatus1 < OperatorEVSEStatus2);

        #endregion

        #endregion

        #region IComparable<OperatorEVSEStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is OperatorEVSEStatus OperatorEVSEStatus))
                throw new ArgumentException("The given object is not an OperatorEVSEStatus identification!", nameof(Object));

            return CompareTo(OperatorEVSEStatus);

        }

        #endregion

        #region CompareTo(OperatorEVSEStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEStatus">An object to compare with.</param>
        public Int32 CompareTo(OperatorEVSEStatus OperatorEVSEStatus)
        {

            if ((Object) OperatorEVSEStatus == null)
                throw new ArgumentNullException(nameof(OperatorEVSEStatus), "The given OperatorEVSEStatus must not be null!");

            return OperatorId.CompareTo(OperatorEVSEStatus.OperatorId);

        }

        #endregion

        #endregion

        #region IEquatable<OperatorEVSEStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object is null)
                return false;

            if (!(Object is OperatorEVSEStatus OperatorEVSEStatus))
                return false;

            return Equals(OperatorEVSEStatus);

        }

        #endregion

        #region Equals(OperatorEVSEStatus)

        /// <summary>
        /// Compares two operator EVSE statuss for equality.
        /// </summary>
        /// <param name="OperatorEVSEStatus">A operator EVSE status to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(OperatorEVSEStatus OperatorEVSEStatus)
        {

            if ((Object) OperatorEVSEStatus == null)
                return false;

            return OperatorId.Equals(OperatorEVSEStatus.OperatorId) &&

                   ((OperatorName   == null && OperatorEVSEStatus.OperatorName   == null) ||
                    (OperatorName   != null && OperatorEVSEStatus.OperatorName   != null && OperatorName.   Equals(OperatorEVSEStatus.OperatorName))) &&

                   ((!EVSEStatusRecords.Any() && !OperatorEVSEStatus.EVSEStatusRecords.Any()) ||
                     (EVSEStatusRecords.Any() &&  OperatorEVSEStatus.EVSEStatusRecords.Any() && EVSEStatusRecords.Count().Equals(OperatorEVSEStatus.EVSEStatusRecords.Count())));

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

                return OperatorId.GetHashCode() * 5 ^

                       (OperatorName.IsNotNullOrEmpty()
                            ? OperatorName.   GetHashCode()
                            : 0) * 3 ^

                       (EVSEStatusRecords.Any()
                            ? EVSEStatusRecords.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(OperatorId,
                             OperatorName.IsNotNullOrEmpty() ? ", " + OperatorName : "",
                             ", ",  EVSEStatusRecords.Count(), " EVSE status record(s)");

        #endregion

    }

}
