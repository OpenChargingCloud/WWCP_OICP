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
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// The current dynamic status of an OICP EVSE.
    /// </summary>
    public class EVSEStatusRecord : ACustomData,
                                    IEquatable <EVSEStatusRecord>,
                                    IComparable<EVSEStatusRecord>,
                                    IComparable

    {

        #region Properties

        /// <summary>
        /// The unique identification of the EVSE.
        /// </summary>
        [Mandatory]
        public EVSE_Id          Id       { get; }

        /// <summary>
        /// The current status of the EVSE.
        /// </summary>
        [Mandatory]
        public EVSEStatusTypes  Status   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP EVSE status record.
        /// </summary>
        /// <param name="Id">The unique identification of an EVSE.</param>
        /// <param name="Status">The current status of an EVSE.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public EVSEStatusRecord(EVSE_Id                              Id,
                                EVSEStatusTypes                      Status,
                                IReadOnlyDictionary<String, Object>  CustomData  = null)

            : base(CustomData)

        {

            this.Id      = Id;
            this.Status  = Status;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv    = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEStatus = "http://www.hubject.com/b2b/services/evsestatus/evsestatus/v2.1">
        //
        // [...]
        //
        //   <EVSEStatus:EvseStatusRecord>
        //      <EVSEStatus:EvseId>DE*GEF*123456789*1</EVSEStatus:EvseId>
        //      <EVSEStatus:EvseStatus>Available</EVSEStatus:EvseStatus>
        //   </EVSEStatus:EvseStatusRecord>
        //
        // [...]
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (EVSEStatusRecordXML,  CustomEVSEStatusRecordParser = null, OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP EVSE status record.
        /// </summary>
        /// <param name="EVSEStatusRecordXML">The XML to parse.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEStatusRecord Parse(XElement                                   EVSEStatusRecordXML,
                                             CustomXMLParserDelegate<EVSEStatusRecord>  CustomEVSEStatusRecordParser   = null,
                                             OnExceptionDelegate                        OnException                    = null)
        {

            if (TryParse(EVSEStatusRecordXML,
                         out EVSEStatusRecord evseStatusRecord,
                         CustomEVSEStatusRecordParser,
                         OnException))

                return evseStatusRecord;

            return null;

        }

        #endregion

        #region (static) Parse   (EVSEStatusRecordText, CustomEVSEStatusRecordParser = null, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP EVSE status record.
        /// </summary>
        /// <param name="EVSEStatusRecordText">The text to parse.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEStatusRecord Parse(String                                     EVSEStatusRecordText,
                                             CustomXMLParserDelegate<EVSEStatusRecord>  CustomEVSEStatusRecordParser   = null,
                                             OnExceptionDelegate                        OnException                    = null)
        {

            if (TryParse(EVSEStatusRecordText,
                         out EVSEStatusRecord evseStatusRecord,
                         CustomEVSEStatusRecordParser,
                         OnException))

                return evseStatusRecord;

            return null;

        }

        #endregion

        #region (static) TryParse(EVSEStatusRecordXML,  out EVSEStatusRecord, CustomEVSEStatusRecordParser = null, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP EVSE status record.
        /// </summary>
        /// <param name="EVSEStatusRecordXML">The XML to parse.</param>
        /// <param name="EVSEStatusRecord">The parsed EVSE status record.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                   EVSEStatusRecordXML,
                                       out EVSEStatusRecord                       EVSEStatusRecord,
                                       CustomXMLParserDelegate<EVSEStatusRecord>  CustomEVSEStatusRecordParser   = null,
                                       OnExceptionDelegate                        OnException                    = null)
        {

            try
            {

                if (EVSEStatusRecordXML.Name != OICPNS.EVSEStatus + "EvseStatusRecord")
                {
                    EVSEStatusRecord = null;
                    return false;
                }

                EVSEStatusRecord = new EVSEStatusRecord(

                                       EVSEStatusRecordXML.MapValueOrFail(OICPNS.EVSEStatus + "EvseId",
                                                                          EVSE_Id.Parse),

                                       EVSEStatusRecordXML.MapValueOrFail(OICPNS.EVSEStatus + "EvseStatus",
                                                                          XML_IO.AsEVSEStatusType)

                                   );


                if (CustomEVSEStatusRecordParser != null)
                    EVSEStatusRecord = CustomEVSEStatusRecordParser(EVSEStatusRecordXML, EVSEStatusRecord);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, EVSEStatusRecordXML, e);

                EVSEStatusRecord = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(EVSEStatusRecordText, out EVSEStatusRecord, CustomEVSEStatusRecordParser = null, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP EVSE status record.
        /// </summary>
        /// <param name="EVSEStatusRecordText">The text to parse.</param>
        /// <param name="EVSEStatusRecord">The parsed EVSE status record.</param>
        /// <param name="CustomEVSEStatusRecordParser">A delegate to parse custom XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                     EVSEStatusRecordText,
                                       out EVSEStatusRecord                       EVSEStatusRecord,
                                       CustomXMLParserDelegate<EVSEStatusRecord>  CustomEVSEStatusRecordParser   = null,
                                       OnExceptionDelegate                        OnException                    = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(EVSEStatusRecordText).Root,
                             out EVSEStatusRecord,
                             CustomEVSEStatusRecordParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, EVSEStatusRecordText, e);
            }

            EVSEStatusRecord = null;
            return false;

        }

        #endregion

        #region ToXML(XName = null, CustomEVSEStatusRecordSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">The XML name to use.</param>
        /// <param name="CustomEVSEStatusRecordSerializer">A delegate to serialize custom EVSEStatusRecord XML elements.</param>
        public XElement ToXML(XName                                          XName                              = null,
                              CustomXMLSerializerDelegate<EVSEStatusRecord>  CustomEVSEStatusRecordSerializer   = null)

        {

            var XML = new XElement(XName ?? OICPNS.EVSEStatus + "EvseStatusRecord",
                          new XElement(OICPNS.EVSEStatus + "EvseId",      Id.    ToString()),
                          new XElement(OICPNS.EVSEStatus + "EvseStatus",  XML_IO.AsText(Status))
                      );

            return CustomEVSEStatusRecordSerializer != null
                       ? CustomEVSEStatusRecordSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSEStatusRecord1, EVSEStatusRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusRecord1">An EVSE status record.</param>
        /// <param name="EVSEStatusRecord2">Another EVSE status record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEStatusRecord EVSEStatusRecord1, EVSEStatusRecord EVSEStatusRecord2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEStatusRecord1, EVSEStatusRecord2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEStatusRecord1 == null) || ((Object) EVSEStatusRecord2 == null))
                return false;

            return EVSEStatusRecord1.Equals(EVSEStatusRecord2);

        }

        #endregion

        #region Operator != (EVSEStatusRecord1, EVSEStatusRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusRecord1">An EVSE status record.</param>
        /// <param name="EVSEStatusRecord2">Another EVSE status record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEStatusRecord EVSEStatusRecord1, EVSEStatusRecord EVSEStatusRecord2)
            => !(EVSEStatusRecord1 == EVSEStatusRecord2);

        #endregion

        #region Operator <  (EVSEStatusRecord1, EVSEStatusRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusRecord1">An EVSE status record.</param>
        /// <param name="EVSEStatusRecord2">Another EVSE status record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEStatusRecord EVSEStatusRecord1, EVSEStatusRecord EVSEStatusRecord2)
        {

            if ((Object) EVSEStatusRecord1 == null)
                throw new ArgumentNullException(nameof(EVSEStatusRecord1), "The given EVSEStatusRecord1 must not be null!");

            return EVSEStatusRecord1.CompareTo(EVSEStatusRecord2) < 0;

        }

        #endregion

        #region Operator <= (EVSEStatusRecord1, EVSEStatusRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusRecord1">An EVSE status record.</param>
        /// <param name="EVSEStatusRecord2">Another EVSE status record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEStatusRecord EVSEStatusRecord1, EVSEStatusRecord EVSEStatusRecord2)
            => !(EVSEStatusRecord1 > EVSEStatusRecord2);

        #endregion

        #region Operator >  (EVSEStatusRecord1, EVSEStatusRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusRecord1">An EVSE status record.</param>
        /// <param name="EVSEStatusRecord2">Another EVSE status record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEStatusRecord EVSEStatusRecord1, EVSEStatusRecord EVSEStatusRecord2)
        {

            if ((Object) EVSEStatusRecord1 == null)
                throw new ArgumentNullException(nameof(EVSEStatusRecord1), "The given EVSEStatusRecord1 must not be null!");

            return EVSEStatusRecord1.CompareTo(EVSEStatusRecord2) > 0;

        }

        #endregion

        #region Operator >= (EVSEStatusRecord1, EVSEStatusRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusRecord1">An EVSE status record.</param>
        /// <param name="EVSEStatusRecord2">Another EVSE status record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEStatusRecord EVSEStatusRecord1, EVSEStatusRecord EVSEStatusRecord2)
            => !(EVSEStatusRecord1 < EVSEStatusRecord2);

        #endregion

        #endregion

        #region IComparable<EVSEStatusRecord> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is EVSEStatusRecord EVSEStatusRecord))
                throw new ArgumentException("The given object is not an EVSE status record identification!", nameof(Object));

            return CompareTo(EVSEStatusRecord);

        }

        #endregion

        #region CompareTo(EVSEStatusRecord)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusRecord">An object to compare with.</param>
        public Int32 CompareTo(EVSEStatusRecord EVSEStatusRecord)
        {

            if ((Object) EVSEStatusRecord == null)
                throw new ArgumentNullException(nameof(EVSEStatusRecord), "The given EVSE status record must not be null!");

            var result = Id.CompareTo(EVSEStatusRecord.Id);

            if (result == 0)
                result = Status.CompareTo(EVSEStatusRecord.Status);

            return result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEStatusRecord> Members

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

            if (!(Object is EVSEStatusRecord EVSEStatusRecord))
                return false;

            return Equals(EVSEStatusRecord);

        }

        #endregion

        #region Equals(EVSEStatusRecord)

        /// <summary>
        /// Compares two EVSE status records for equality.
        /// </summary>
        /// <param name="EVSEStatusRecord">An EVSE status record to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEStatusRecord EVSEStatusRecord)
        {

            if ((Object) EVSEStatusRecord == null)
                return false;

            return Id.    Equals(EVSEStatusRecord.Id) &&
                   Status.Equals(EVSEStatusRecord.Status);

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

                return Id.    GetHashCode() * 3 ^
                       Status.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id,
                             " -> ",
                             Status);

        #endregion

    }

}
