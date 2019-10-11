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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// An RFID identification.
    /// </summary>
    public struct RFIDIdentification : IEquatable <RFIDIdentification>,
                                       IComparable<RFIDIdentification>,
                                       IComparable
    {

        #region Properties

        /// <summary>
        /// The UID from the RFID-Card. It should be read from left to right using big-endian format.
        /// </summary>
        public UID        UID              { get; }

        /// <summary>
        /// The type of the used RFID card like mifareclassic, desfire.
        /// </summary>
        public RFIDTypes  RFIDType         { get; }

        /// <summary>
        /// An optional EVCOId for the given UID.
        /// </summary>
        public EVCO_Id?   EVCOId           { get; }

        /// <summary>
        /// A number printed on a customer’s card for manual authorization (e.q. via a call center).
        /// (1 - 150 characters).
        /// </summary>
        public String     PrintedNumber    { get; }

        /// <summary>
        /// Until when this card is valid. Should not be set if card does not have an expiration yet.
        /// </summary>
        public DateTime?  ExpiryDate       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new RFID identification.
        /// </summary>
        /// <param name="UID">The UID from the RFID-Card. It should be read from left to right using big-endian format.</param>
        /// <param name="EVCOId">An optional EVCOId for the given UID.</param>
        /// <param name="RFIDType">The type of the used RFID card like mifareclassic, desfire.</param>
        /// <param name="PrintedNumber">A number printed on a customer’s card for manual authorization (e.q. via a call center).</param>
        /// <param name="ExpiryDate">Until when this card is valid. Should not be set if card does not have an expiration yet.</param>
        public RFIDIdentification(UID         UID,
                                  RFIDTypes   RFIDType,
                                  EVCO_Id?    EVCOId         = null,
                                  String      PrintedNumber  = null,
                                  DateTime?   ExpiryDate     = null)
        {

            if (PrintedNumber != null)
                PrintedNumber = PrintedNumber.Trim();

            this.UID            = UID;
            this.RFIDType       = RFIDType;
            this.EVCOId         = EVCOId;
            this.PrintedNumber  = PrintedNumber?.Substring(0, Math.Min(PrintedNumber.Length, 150));
            this.ExpiryDate     = ExpiryDate;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv            = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:AuthenticationData = "http://www.hubject.com/b2b/services/authenticationdata/v2.0"
        //                   xmlns:CommonTypes        = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        // 
        // [...]
        // 
        //    <CommonTypes:RFIDIdentificationType>
        // 
        //       <CommonTypes:UID>...</CommonTypes:UID>
        // 
        //       <!--Optional:-->
        //       <CommonTypes:EvcoID>...</CommonTypes:EvcoID>
        // 
        //       <CommonTypes:RFIDType>...</CommonTypes:RFIDType>
        // 
        //       <!--Optional:-->
        //       <CommonTypes:PrintedNumber>...</CommonTypes:PrintedNumber>
        // 
        //       <!--Optional:-->
        //       <CommonTypes:ExpiryDate>...</CommonTypes:ExpiryDate>
        // 
        //    </CommonTypes:RFIDIdentificationType>
        // 
        // [...]
        // 
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(RFIDIdentificationXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP RFID identification.
        /// </summary>
        /// <param name="RFIDIdentificationXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static RFIDIdentification? Parse(XElement             RFIDIdentificationXML,
                                                  OnExceptionDelegate  OnException = null)
        {

            if (TryParse(RFIDIdentificationXML,
                         out RFIDIdentification _RFIDIdentification,
                         OnException))

                return _RFIDIdentification;

            return null;

        }

        #endregion

        #region (static) Parse(RFIDIdentificationText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP RFID identification.
        /// </summary>
        /// <param name="RFIDIdentificationText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static RFIDIdentification? Parse(String               RFIDIdentificationText,
                                                  OnExceptionDelegate  OnException = null)
        {

            if (TryParse(RFIDIdentificationText,
                         out RFIDIdentification _RFIDIdentification,
                         OnException))

                return _RFIDIdentification;

            return null;

        }

        #endregion

        #region (static) TryParse(RFIDIdentificationXML,  out RFIDIdentification, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP RFID identification.
        /// </summary>
        /// <param name="RFIDIdentificationXML">The XML to parse.</param>
        /// <param name="RFIDIdentification">The parsed RFID identification.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                RFIDIdentificationXML,
                                       out RFIDIdentification  RFIDIdentification,
                                       OnExceptionDelegate     OnException  = null)
        {

            try
            {

                //if (!(RFIDIdentificationXML.Name == OICPNS.CommonTypes         + "RFIDIdentification") &&
                //     (RFIDIdentificationXML.Name == OICPNS.MobileAuthorization + "RFIDIdentification"))
                //{
                    RFIDIdentification = default(RFIDIdentification);
                    return false;
                //}

                //var EVCOId        = RFIDIdentificationXML.MapValueOrFail       (OICPNS.CommonTypes + "EVCOID", EVCO_Id.Parse);
                //var PIN           = RFIDIdentificationXML.ElementValueOrDefault(OICPNS.CommonTypes + "PIN");
                //var HashedPINXML  = RFIDIdentificationXML.Element              (OICPNS.CommonTypes + "HashedPIN");

                //#region Parse a PIN

                //if (PIN != null && PIN.Trim().IsNotNullOrEmpty())

                //    RFIDIdentification = new RFIDIdentification(EVCOId, PIN.Trim());

                //#endregion

                //#region Parse a hashed PIN

                //else if (HashedPINXML != null)

                //    RFIDIdentification = new RFIDIdentification(EVCOId,

                //                                                    HashedPINXML.ElementValueOrFail(OICPNS.CommonTypes + "Value"),

                //                                                    HashedPINXML.MapValueOrFail    (OICPNS.CommonTypes + "Function",
                //                                                                                    text => {

                //                                                                                        switch (text.ToUpper())
                //                                                                                        {

                //                                                                                            case "MD5":
                //                                                                                                return PINCrypto.MD5;

                //                                                                                            case "SHA-1":
                //                                                                                                return PINCrypto.SHA1;

                //                                                                                        }

                //                                                                                        throw new Exception("Unknown PIN crypto '" + text + "'!");

                //                                                                                    }),

                //                                                    HashedPINXML.ElementValueOrFail(OICPNS.CommonTypes + "Salt"));

                //#endregion

                //else
                //    RFIDIdentification = new RFIDIdentification(EVCOId);

                //return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, RFIDIdentificationXML, e);

                RFIDIdentification = default(RFIDIdentification);
                return false;

            }

        }

        #endregion

        #region (static) TryParse(RFIDIdentificationText, out RFIDIdentification, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP RFID identification.
        /// </summary>
        /// <param name="RFIDIdentificationText">The text to parse.</param>
        /// <param name="RFIDIdentification">The parsed RFID identification.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                    RFIDIdentificationText,
                                       out RFIDIdentification  RFIDIdentification,
                                       OnExceptionDelegate       OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(RFIDIdentificationText).Root,
                             out RFIDIdentification,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, RFIDIdentificationText, e);
            }

            RFIDIdentification = default(RFIDIdentification);
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">The XML name to use.</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OICPNS.CommonTypes + "RFIDIdentification",

                   new XElement(OICPNS.CommonTypes       + "UID",           UID.       ToString()),

                   EVCOId.HasValue
                       ? new XElement(OICPNS.CommonTypes + "EvcoID",        EVCOId.    ToString())
                       : null,

                   new XElement(OICPNS.CommonTypes       + "RFIDType",      RFIDType.  ToString()),

                   PrintedNumber.IsNeitherNullNorEmpty()
                       ? new XElement(OICPNS.CommonTypes + "PrintedNumber", PrintedNumber)
                       : null,

                   ExpiryDate.HasValue
                       ? new XElement(OICPNS.CommonTypes + "ExpiryDate",    ExpiryDate.ToString())
                       : null

                   );

        #endregion


        #region Operator overloading

        #region Operator == (RFIDIdentification1, RFIDIdentification2)

        /// <summary>
        /// Compares two RFID identifications for equality.
        /// </summary>
        /// <param name="RFIDIdentification1">A RFID identification.</param>
        /// <param name="RFIDIdentification2">Another RFID identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RFIDIdentification RFIDIdentification1, RFIDIdentification RFIDIdentification2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(RFIDIdentification1, RFIDIdentification2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RFIDIdentification1 == null) || ((Object) RFIDIdentification2 == null))
                return false;

            return RFIDIdentification1.Equals(RFIDIdentification2);

        }

        #endregion

        #region Operator != (RFIDIdentification1, RFIDIdentification2)

        /// <summary>
        /// Compares two RFID identifications for inequality.
        /// </summary>
        /// <param name="RFIDIdentification1">A RFID identification.</param>
        /// <param name="RFIDIdentification2">Another RFID identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RFIDIdentification RFIDIdentification1, RFIDIdentification RFIDIdentification2)

            => !(RFIDIdentification1 == RFIDIdentification2);

        #endregion

        #region Operator <  (RFIDIdentification1, RFIDIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDIdentification1">A RFID identification.</param>
        /// <param name="RFIDIdentification2">Another RFID identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RFIDIdentification RFIDIdentification1, RFIDIdentification RFIDIdentification2)
        {

            if ((Object) RFIDIdentification1 == null)
                throw new ArgumentNullException(nameof(RFIDIdentification1), "The given RFID identification must not be null!");

            return RFIDIdentification1.CompareTo(RFIDIdentification2) < 0;

        }

        #endregion

        #region Operator <= (RFIDIdentification1, RFIDIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDIdentification1">A RFID identification.</param>
        /// <param name="RFIDIdentification2">Another RFID identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RFIDIdentification RFIDIdentification1, RFIDIdentification RFIDIdentification2)
            => !(RFIDIdentification1 > RFIDIdentification2);

        #endregion

        #region Operator >  (RFIDIdentification1, RFIDIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDIdentification1">A RFID identification.</param>
        /// <param name="RFIDIdentification2">Another RFID identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RFIDIdentification RFIDIdentification1, RFIDIdentification RFIDIdentification2)
        {

            if ((Object) RFIDIdentification1 == null)
                throw new ArgumentNullException(nameof(RFIDIdentification1), "The given RFID identification must not be null!");

            return RFIDIdentification1.CompareTo(RFIDIdentification2) > 0;

        }

        #endregion

        #region Operator >= (RFIDIdentification1, RFIDIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDIdentification1">A RFID identification.</param>
        /// <param name="RFIDIdentification2">Another RFID identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RFIDIdentification RFIDIdentification1, RFIDIdentification RFIDIdentification2)
            => !(RFIDIdentification1 < RFIDIdentification2);

        #endregion

        #endregion

        #region IComparable<RFIDIdentification> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            if (!(Object is RFIDIdentification))
                throw new ArgumentException("The given object is not an RFID identification!", nameof(Object));

            return CompareTo((RFIDIdentification) Object);

        }

        #endregion

        #region CompareTo(RFIDIdentification)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDIdentification">An RFID identification object to compare with.</param>
        public Int32 CompareTo(RFIDIdentification RFIDIdentification)
        {

            if ((Object) RFIDIdentification == null)
                throw new ArgumentNullException(nameof(RFIDIdentification),  "The given RFID identification must not be null!");

            var result = EVCOId.CompareTo(RFIDIdentification.EVCOId);
            if (result != 0)
                return result;

            result = String.Compare(PIN, RFIDIdentification.PIN, StringComparison.Ordinal);
            if (result != 0)
                return result;

            result = Function.CompareTo(RFIDIdentification.Function);
            if (result != 0)
                return result;

            return String.Compare(Salt, RFIDIdentification.Salt, StringComparison.Ordinal);

        }

        #endregion

        #endregion

        #region IEquatable<RFIDIdentification> Members

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

            if (!(Object is RFIDIdentification))
                return false;

            return Equals((RFIDIdentification) Object);

        }

        #endregion

        #region Equals(RFIDIdentification)

        /// <summary>
        /// Compares two RFID identificationss for equality.
        /// </summary>
        /// <param name="RFIDIdentification">An RFID identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RFIDIdentification RFIDIdentification)
        {

            if ((Object) RFIDIdentification == null)
                return false;

            return EVCOId.  Equals(RFIDIdentification.EVCOId)   &&
                   PIN.     Equals(RFIDIdentification.PIN)      &&
                   Function.Equals(RFIDIdentification.Function) &&
                   Salt.    Equals(RFIDIdentification.Salt);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return EVCOId.  GetHashCode() * 7 ^
                       PIN.     GetHashCode() * 5 ^
                       Function.GetHashCode() * 3 ^
                       Salt.    GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVCOId.ToString(),
                             Function != PINCrypto.none
                                 ? " -" + Function
                                 : "",
                             "-> ",
                             PIN ?? "");

        #endregion

    }

}
