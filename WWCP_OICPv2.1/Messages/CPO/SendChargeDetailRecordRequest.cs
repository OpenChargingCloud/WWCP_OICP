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
using System.Threading;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.CPO
{

    /// <summary>
    /// An OICP send charge detail record request.
    /// </summary>
    public class SendChargeDetailRecordRequest : ARequest<SendChargeDetailRecordRequest>
    {

        #region Properties

        /// <summary>
        /// The charge detail record to upload.
        /// </summary>
        public ChargeDetailRecord ChargeDetailRecord { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord XML/SOAP request.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record to upload.</param>
        public SendChargeDetailRecordRequest(ChargeDetailRecord  ChargeDetailRecord,

                                             DateTime?           Timestamp           = null,
                                             CancellationToken?  CancellationToken   = null,
                                             EventTracking_Id    EventTrackingId     = null,
                                             TimeSpan?           RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord), "The given charge detail record must not be null!");

            #endregion

            this.ChargeDetailRecord  = ChargeDetailRecord;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0"
        //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <Authorization:eRoamingChargeDetailRecord>
        // 
        //          [...]
        // 
        //       </Authorization:eRoamingChargeDetailRecord>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(SendChargeDetailRecordXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP send charge detail record request.
        /// </summary>
        /// <param name="SendChargeDetailRecordXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SendChargeDetailRecordRequest Parse(XElement             SendChargeDetailRecordXML,
                                                          OnExceptionDelegate  OnException = null)
        {

            SendChargeDetailRecordRequest _SendChargeDetailRecord;

            if (TryParse(SendChargeDetailRecordXML, out _SendChargeDetailRecord, OnException))
                return _SendChargeDetailRecord;

            return null;

        }

        #endregion

        #region (static) Parse(SendChargeDetailRecordText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP send charge detail record request.
        /// </summary>
        /// <param name="SendChargeDetailRecordText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SendChargeDetailRecordRequest Parse(String               SendChargeDetailRecordText,
                                                          OnExceptionDelegate  OnException = null)
        {

            SendChargeDetailRecordRequest _SendChargeDetailRecord;

            if (TryParse(SendChargeDetailRecordText, out _SendChargeDetailRecord, OnException))
                return _SendChargeDetailRecord;

            return null;

        }

        #endregion

        #region (static) TryParse(SendChargeDetailRecordXML,  out SendChargeDetailRecord, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP send charge detail record request.
        /// </summary>
        /// <param name="SendChargeDetailRecordXML">The XML to parse.</param>
        /// <param name="SendChargeDetailRecord">The parsed send charge detail record request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                           SendChargeDetailRecordXML,
                                       out SendChargeDetailRecordRequest  SendChargeDetailRecord,
                                       OnExceptionDelegate                OnException  = null)
        {

            try
            {

                SendChargeDetailRecord = new SendChargeDetailRecordRequest(
                                             ChargeDetailRecord.Parse(SendChargeDetailRecordXML)
                                         );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, SendChargeDetailRecordXML, e);

                SendChargeDetailRecord = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SendChargeDetailRecordText, out SendChargeDetailRecord, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP send charge detail record request.
        /// </summary>
        /// <param name="SendChargeDetailRecordText">The text to parse.</param>
        /// <param name="SendChargeDetailRecord">The parsed send charge detail record request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                             SendChargeDetailRecordText,
                                       out SendChargeDetailRecordRequest  SendChargeDetailRecord,
                                       OnExceptionDelegate                OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SendChargeDetailRecordText).Root,
                             out SendChargeDetailRecord,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, SendChargeDetailRecordText, e);
            }

            SendChargeDetailRecord = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => ChargeDetailRecord.ToXML(OICPNS.Authorization + "eRoamingChargeDetailRecord");

        #endregion


        #region Operator overloading

        #region Operator == (SendChargeDetailRecord1, SendChargeDetailRecord2)

        /// <summary>
        /// Compares two send charge detail record requests for equality.
        /// </summary>
        /// <param name="SendChargeDetailRecord1">An send charge detail record request.</param>
        /// <param name="SendChargeDetailRecord2">Another send charge detail record request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SendChargeDetailRecordRequest SendChargeDetailRecord1, SendChargeDetailRecordRequest SendChargeDetailRecord2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SendChargeDetailRecord1, SendChargeDetailRecord2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SendChargeDetailRecord1 == null) || ((Object) SendChargeDetailRecord2 == null))
                return false;

            return SendChargeDetailRecord1.Equals(SendChargeDetailRecord2);

        }

        #endregion

        #region Operator != (SendChargeDetailRecord1, SendChargeDetailRecord2)

        /// <summary>
        /// Compares two send charge detail record requests for inequality.
        /// </summary>
        /// <param name="SendChargeDetailRecord1">An send charge detail record request.</param>
        /// <param name="SendChargeDetailRecord2">Another send charge detail record request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SendChargeDetailRecordRequest SendChargeDetailRecord1, SendChargeDetailRecordRequest SendChargeDetailRecord2)

            => !(SendChargeDetailRecord1 == SendChargeDetailRecord2);

        #endregion

        #endregion

        #region IEquatable<SendChargeDetailRecord> Members

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

            var SendChargeDetailRecord = Object as SendChargeDetailRecordRequest;
            if ((Object) SendChargeDetailRecord == null)
                return false;

            return this.Equals(SendChargeDetailRecord);

        }

        #endregion

        #region Equals(SendChargeDetailRecord)

        /// <summary>
        /// Compares two send charge detail record requests for equality.
        /// </summary>
        /// <param name="SendChargeDetailRecord">An send charge detail record request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SendChargeDetailRecordRequest SendChargeDetailRecord)
        {

            if ((Object) SendChargeDetailRecord == null)
                return false;

            return ChargeDetailRecord.Equals(SendChargeDetailRecord.ChargeDetailRecord);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => ChargeDetailRecord.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => ChargeDetailRecord.ToString();

        #endregion


    }

}
