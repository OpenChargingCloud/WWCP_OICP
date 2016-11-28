/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// The current dynamic status of an OICP EVSE.
    /// </summary>
    public class EVSEStatusRecord : ACustomData,
                                    IEquatable<EVSEStatusRecord>,
                                    IComparable<EVSEStatusRecord>,
                                    IComparable

    {

        #region Properties

        /// <summary>
        /// The unique identification of the EVSE.
        /// </summary>
        public EVSE_Id          Id       { get; }

        /// <summary>
        /// The current status of the EVSE.
        /// </summary>
        public EVSEStatusTypes  Status   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP EVSE status record.
        /// </summary>
        /// <param name="Id">The unique identification of an EVSE.</param>
        /// <param name="Status">The current status of an EVSE.</param>
        public EVSEStatusRecord(EVSE_Id          Id,
                                EVSEStatusTypes  Status)

        {

            this.Id      = Id;
            this.Status  = Status;

        }

        #endregion


        #region Parse(KeyValuePair)

        /// <summary>
        /// Convert the given key-value-pair into an EVSE status record.
        /// </summary>
        public static EVSEStatusRecord Parse(KeyValuePair<EVSE_Id, EVSEStatusTypes> KeyValuePair)

            => new EVSEStatusRecord(KeyValuePair.Key, KeyValuePair.Value);

        #endregion

        #region ToKeyValuePair()

        /// <summary>
        /// Conversion this EVSE status record to a key-value-pair.
        /// </summary>
        public KeyValuePair<EVSE_Id, EVSEStatusTypes> ToKeyValuePair()

            => new KeyValuePair<EVSE_Id, EVSEStatusTypes>(Id, Status);

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv    = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEStatus = "http://www.hubject.com/b2b/services/evsestatus/EVSEData.0">
        //
        // [...]
        //
        //   <EVSEStatus:EvseStatusRecord>
        //      <EVSEStatus:EvseId>?</EVSEData:EvseId>
        //      <EVSEStatus:EvseStatus>?</EVSEData:EvseStatus>
        //   </EVSEStatus:EvseStatusRecord>
        //
        // [...]

        #endregion

        #region Parse(EVSEStatusRecordXML)

        /// <summary>
        /// Parse the EVSE identification and its current status from the given OICP XML.
        /// </summary>
        /// <param name="EVSEStatusRecordXML">An OICP XML.</param>
        public static EVSEStatusRecord Parse(XElement EVSEStatusRecordXML)
        {

            try
            {

                if (EVSEStatusRecordXML.Name != OICPNS.EVSEStatus + "EvseStatusRecord")
                    throw new Exception("Illegal EVSEStatusRecord XML!");

                return new EVSEStatusRecord(

                           EVSEStatusRecordXML.MapValueOrFail(OICPNS.EVSEStatus + "EvseId",
                                                              EVSE_Id.Parse),

                           (EVSEStatusTypes) Enum.Parse(typeof(EVSEStatusTypes), EVSEStatusRecordXML.ElementValueOrFail(OICPNS.EVSEStatus + "EvseStatus"))

                       );

            }
            catch (Exception e)
            {
                return null;
            }

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return an OICP XML representation of this EVSE status record.
        /// </summary>
        /// <returns></returns>
        public XElement ToXML()

            => new XElement(OICPNS.EVSEStatus + "EvseStatusRecord",
                   new XElement(OICPNS.EVSEStatus + "EvseId",     Id.    ToString()),
                   new XElement(OICPNS.EVSEStatus + "EvseStatus", Status.ToString())
               );

        #endregion


        #region Operator overloading

        #region Operator == (EVSEStatusRecord1, EVSEStatusRecord2)

        /// <summary>
        /// Compares two EVSE status records for equality.
        /// </summary>
        /// <param name="EVSEStatusRecord1">An EVSE status record.</param>
        /// <param name="EVSEStatusRecord2">Another EVSE status record.</param>
        /// <returns>True if both match; False otherwise.</returns>
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
        /// Compares two EVSE status records for inequality.
        /// </summary>
        /// <param name="EVSEStatusRecord1">An EVSE status record.</param>
        /// <param name="EVSEStatusRecord2">Another EVSE status record.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EVSEStatusRecord EVSEStatusRecord1, EVSEStatusRecord EVSEStatusRecord2)

            => !(EVSEStatusRecord1 == EVSEStatusRecord2);

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

            if (Object == null)
                return false;

            if (!(Object is EVSEStatusRecord))
                return false;

            return this.Equals((EVSEStatusRecord) Object);

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

                return Id.    GetHashCode() * 5 ^
                       Status.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id,
                             " -> ",
                             Status);

        #endregion


        public int CompareTo(EVSEStatusRecord other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

    }

}
