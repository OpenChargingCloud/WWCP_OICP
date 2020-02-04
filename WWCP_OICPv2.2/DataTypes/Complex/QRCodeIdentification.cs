/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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
    /// An QR code identification with (hashed) pin.
    /// </summary>
    public struct QRCodeIdentification : IEquatable <QRCodeIdentification>,
                                         IComparable<QRCodeIdentification>,
                                         IComparable
    {

        #region Properties

        /// <summary>
        /// The QR code identification.
        /// </summary>
        public EVCO_Id    EVCOId     { get; }

        /// <summary>
        /// A pin.
        /// </summary>
        public String     PIN        { get; }

        /// <summary>
        /// A crypto function.
        /// </summary>
        public PINCrypto  Function   { get; }

        /// <summary>
        /// The salt for the crypto function.
        /// </summary>
        public String     Salt       { get; }

        #endregion

        #region Constructor(s)

        #region QRCodeIdentification(EVCOId, PIN = null)

        /// <summary>
        /// Create a new QR code identification with pin.
        /// </summary>
        /// <param name="EVCOId">An QR code identification.</param>
        /// <param name="PIN">A optional pin.</param>
        public QRCodeIdentification(EVCO_Id  EVCOId,
                                    String   PIN  = null)
        {

            this.EVCOId    = EVCOId;
            this.PIN       = PIN ?? "";
            this.Function  = PINCrypto.none;
            this.Salt      = "";

        }

        #endregion

        #region QRCodeIdentification(EVCOId, HashedPIN, Function, Salt = "")

        /// <summary>
        /// Create a new QR code identification with a hashed pin.
        /// </summary>
        /// <param name="EVCOId">An QR code identification.</param>
        /// <param name="HashedPIN">A hashed pin.</param>
        /// <param name="Function">A crypto function.</param>
        /// <param name="Salt">A salt of the crypto function.</param>
        public QRCodeIdentification(EVCO_Id    EVCOId,
                                    String     HashedPIN,
                                    PINCrypto  Function,
                                    String     Salt = "")
        {

            this.EVCOId    = EVCOId;
            this.PIN       = HashedPIN;
            this.Function  = Function;
            this.Salt      = Salt ?? "";

        }

        #endregion

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv            = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:AuthenticationData = "http://www.hubject.com/b2b/services/authenticationdata/v2.1"
        //                   xmlns:CommonTypes        = "http://www.hubject.com/b2b/services/commontypes/v2.1">
        // 
        // [...]
        // 
        //    <CommonTypes:QRCodeIdentification>
        // 
        //       <CommonTypes:EvcoID>DE*GDF*01234ABCD*Z</CommonTypes:EvcoID>
        // 
        //       <!--You have a CHOICE of nothing or one of the next 2 items at this level-->
        //       <CommonTypes:PIN>?</CommonTypes:PIN>
        // 
        //       <CommonTypes:HashedPIN>
        //          <CommonTypes:Value>f7cf02826ba923e3d31c1c3015899076</CommonTypes:Value>
        //          <CommonTypes:Function>MD5|SHA-1</CommonTypes:Function>
        //          <CommonTypes:Salt>22c7c09370af2a3f07fe8665b140498a</CommonTypes:Salt>
        //       </CommonTypes:HashedPIN>
        // 
        //    </CommonTypes:QRCodeIdentification>
        // 
        // [...]
        // 
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(QRCodeIdentificationXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP QR code identification.
        /// </summary>
        /// <param name="QRCodeIdentificationXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static QRCodeIdentification? Parse(XElement             QRCodeIdentificationXML,
                                                  OnExceptionDelegate  OnException = null)
        {

            if (TryParse(QRCodeIdentificationXML,
                         out QRCodeIdentification _QRCodeIdentification,
                         OnException))

                return _QRCodeIdentification;

            return null;

        }

        #endregion

        #region (static) Parse(QRCodeIdentificationText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP QR code identification.
        /// </summary>
        /// <param name="QRCodeIdentificationText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static QRCodeIdentification? Parse(String               QRCodeIdentificationText,
                                                  OnExceptionDelegate  OnException = null)
        {

            if (TryParse(QRCodeIdentificationText,
                         out QRCodeIdentification _QRCodeIdentification,
                         OnException))

                return _QRCodeIdentification;

            return null;

        }

        #endregion

        #region (static) TryParse(QRCodeIdentificationXML,  out QRCodeIdentification, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP QR code identification.
        /// </summary>
        /// <param name="QRCodeIdentificationXML">The XML to parse.</param>
        /// <param name="QRCodeIdentification">The parsed QR code identification.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                  QRCodeIdentificationXML,
                                       out QRCodeIdentification  QRCodeIdentification,
                                       OnExceptionDelegate       OnException  = null)
        {

            try
            {

                if (!(QRCodeIdentificationXML.Name == OICPNS.CommonTypes         + "QRCodeIdentification") &&
                     (QRCodeIdentificationXML.Name == OICPNS.MobileAuthorization + "QRCodeIdentification"))
                {
                    QRCodeIdentification = default;
                    return false;
                }

                var EVCOId        = QRCodeIdentificationXML.MapValueOrFail       (OICPNS.CommonTypes + "EvcoID", EVCO_Id.Parse);
                var PIN           = QRCodeIdentificationXML.ElementValueOrDefault(OICPNS.CommonTypes + "PIN");
                var HashedPINXML  = QRCodeIdentificationXML.Element              (OICPNS.CommonTypes + "HashedPIN");

                #region Parse a PIN

                if (PIN != null && PIN.Trim().IsNotNullOrEmpty())
                    QRCodeIdentification = new QRCodeIdentification(EVCOId, PIN.Trim());

                #endregion

                #region Parse a hashed PIN

                else if (HashedPINXML != null)
                    QRCodeIdentification = new QRCodeIdentification(EVCOId,

                                                                    HashedPINXML.ElementValueOrFail(OICPNS.CommonTypes + "Value"),

                                                                    HashedPINXML.MapValueOrFail    (OICPNS.CommonTypes + "Function",
                                                                                                    text => {

                                                                                                        switch (text.ToUpper())
                                                                                                        {

                                                                                                            case "MD5":
                                                                                                                return PINCrypto.MD5;

                                                                                                            case "SHA-1":
                                                                                                                return PINCrypto.SHA1;

                                                                                                        }

                                                                                                        throw new Exception("Unknown PIN crypto '" + text + "'!");

                                                                                                    }),

                                                                    HashedPINXML.ElementValueOrFail(OICPNS.CommonTypes + "Salt"));

                #endregion

                else
                    QRCodeIdentification = new QRCodeIdentification(EVCOId);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, QRCodeIdentificationXML, e);

                QRCodeIdentification = default;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(QRCodeIdentificationText, out QRCodeIdentification, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP QR code identification.
        /// </summary>
        /// <param name="QRCodeIdentificationText">The text to parse.</param>
        /// <param name="QRCodeIdentification">The parsed QR code identification.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                    QRCodeIdentificationText,
                                       out QRCodeIdentification  QRCodeIdentification,
                                       OnExceptionDelegate       OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(QRCodeIdentificationText).Root,
                             out QRCodeIdentification,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, QRCodeIdentificationText, e);
            }

            QRCodeIdentification = default(QRCodeIdentification);
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">The XML name to use.</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OICPNS.CommonTypes + "QRCodeIdentification",

                   new XElement(OICPNS.CommonTypes + "EvcoID", EVCOId.ToString()),

                   Function == PINCrypto.none

                       ? new XElement(OICPNS.CommonTypes + "PIN", PIN)

                       : new XElement(OICPNS.CommonTypes + "HashedPIN",
                             new XElement(OICPNS.CommonTypes + "Value",     PIN),
                             new XElement(OICPNS.CommonTypes + "Function",  Function == PINCrypto.MD5 ? "MD5" : "SHA-1"),
                             new XElement(OICPNS.CommonTypes + "Salt",      Salt))

                   );

        #endregion


        #region Operator overloading

        #region Operator == (QRCodeIdentification1, QRCodeIdentification2)

        /// <summary>
        /// Compares two QR code identifications for equality.
        /// </summary>
        /// <param name="QRCodeIdentification1">A QR code identification.</param>
        /// <param name="QRCodeIdentification2">Another QR code identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (QRCodeIdentification QRCodeIdentification1, QRCodeIdentification QRCodeIdentification2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(QRCodeIdentification1, QRCodeIdentification2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) QRCodeIdentification1 == null) || ((Object) QRCodeIdentification2 == null))
                return false;

            return QRCodeIdentification1.Equals(QRCodeIdentification2);

        }

        #endregion

        #region Operator != (QRCodeIdentification1, QRCodeIdentification2)

        /// <summary>
        /// Compares two QR code identifications for inequality.
        /// </summary>
        /// <param name="QRCodeIdentification1">A QR code identification.</param>
        /// <param name="QRCodeIdentification2">Another QR code identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (QRCodeIdentification QRCodeIdentification1, QRCodeIdentification QRCodeIdentification2)

            => !(QRCodeIdentification1 == QRCodeIdentification2);

        #endregion

        #region Operator <  (QRCodeIdentification1, QRCodeIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="QRCodeIdentification1">A QR code identification.</param>
        /// <param name="QRCodeIdentification2">Another QR code identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (QRCodeIdentification QRCodeIdentification1, QRCodeIdentification QRCodeIdentification2)
        {

            if ((Object) QRCodeIdentification1 == null)
                throw new ArgumentNullException(nameof(QRCodeIdentification1), "The given QR code identification must not be null!");

            return QRCodeIdentification1.CompareTo(QRCodeIdentification2) < 0;

        }

        #endregion

        #region Operator <= (QRCodeIdentification1, QRCodeIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="QRCodeIdentification1">A QR code identification.</param>
        /// <param name="QRCodeIdentification2">Another QR code identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (QRCodeIdentification QRCodeIdentification1, QRCodeIdentification QRCodeIdentification2)
            => !(QRCodeIdentification1 > QRCodeIdentification2);

        #endregion

        #region Operator >  (QRCodeIdentification1, QRCodeIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="QRCodeIdentification1">A QR code identification.</param>
        /// <param name="QRCodeIdentification2">Another QR code identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (QRCodeIdentification QRCodeIdentification1, QRCodeIdentification QRCodeIdentification2)
        {

            if ((Object) QRCodeIdentification1 == null)
                throw new ArgumentNullException(nameof(QRCodeIdentification1), "The given QR code identification must not be null!");

            return QRCodeIdentification1.CompareTo(QRCodeIdentification2) > 0;

        }

        #endregion

        #region Operator >= (QRCodeIdentification1, QRCodeIdentification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="QRCodeIdentification1">A QR code identification.</param>
        /// <param name="QRCodeIdentification2">Another QR code identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (QRCodeIdentification QRCodeIdentification1, QRCodeIdentification QRCodeIdentification2)
            => !(QRCodeIdentification1 < QRCodeIdentification2);

        #endregion

        #endregion

        #region IComparable<QRCodeIdentification> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            if (!(Object is QRCodeIdentification QRCodeIdentification))
                throw new ArgumentException("The given object is not an QR code identification with (hashed) pin!", nameof(Object));

            return CompareTo(QRCodeIdentification);

        }

        #endregion

        #region CompareTo(QRCodeIdentification)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="QRCodeIdentification">An QR code identification with (hashed) pin object to compare with.</param>
        public Int32 CompareTo(QRCodeIdentification QRCodeIdentification)
        {

            if ((Object) QRCodeIdentification == null)
                throw new ArgumentNullException(nameof(QRCodeIdentification),  "The given QR code identification with (hashed) pin must not be null!");

            var result = EVCOId.CompareTo(QRCodeIdentification.EVCOId);

            if (result == 0)
                result = String.Compare(PIN, QRCodeIdentification.PIN, StringComparison.OrdinalIgnoreCase);

            if (result == 0)
                result = Function.CompareTo(QRCodeIdentification.Function);

            if (result == 0)
                result = String.Compare(Salt, QRCodeIdentification.Salt, StringComparison.OrdinalIgnoreCase);

            return result;

        }

        #endregion

        #endregion

        #region IEquatable<QRCodeIdentification> Members

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

            if (!(Object is QRCodeIdentification QRCodeIdentification))
                return false;

            return Equals(QRCodeIdentification);

        }

        #endregion

        #region Equals(QRCodeIdentification)

        /// <summary>
        /// Compares two QR code identifications with (hashed) pins for equality.
        /// </summary>
        /// <param name="QRCodeIdentification">An QR code identification with (hashed) pin to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(QRCodeIdentification QRCodeIdentification)
        {

            if ((Object) QRCodeIdentification == null)
                return false;

            return EVCOId.  Equals(QRCodeIdentification.EVCOId)   &&
                   PIN.     Equals(QRCodeIdentification.PIN)      &&
                   Function.Equals(QRCodeIdentification.Function) &&
                   Salt.    Equals(QRCodeIdentification.Salt);

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
