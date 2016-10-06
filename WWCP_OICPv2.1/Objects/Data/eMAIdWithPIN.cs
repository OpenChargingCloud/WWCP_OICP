/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
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

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An e-mobility account identification with (hashed) pin.
    /// </summary>
    public class eMAIdWithPIN : IEquatable <eMAIdWithPIN>,
                                IComparable<eMAIdWithPIN>,
                                IComparable
    {

        #region Properties

        /// <summary>
        /// An e-mobility account identification.
        /// </summary>
        public eMobilityAccount_Id  eMAId       { get; }

        /// <summary>
        /// A pin.
        /// </summary>
        public String               PIN         { get; }

        /// <summary>
        /// A crypto function.
        /// </summary>
        public PINCrypto            Function    { get; }

        /// <summary>
        /// The salt for the crypto function.
        /// </summary>
        public String               Salt        { get; }

        #endregion

        #region Constructor(s)

        #region eMAIdWithPIN(eMAId, PIN)

        /// <summary>
        /// Create a new e-mobility account identification with pin.
        /// </summary>
        /// <param name="eMAId">The e-mobility account identification.</param>
        /// <param name="PIN">The pin.</param>
        public eMAIdWithPIN(eMobilityAccount_Id  eMAId,
                            String               PIN)
        {

            this.eMAId     = eMAId;
            this.PIN       = PIN;
            this.Function  = PINCrypto.none;

        }

        #endregion

        #region eMAIdWithPIN(eMAId, HashedPIN, Function, Salt = "")

        /// <summary>
        /// Create a new e-mobility account identification with a hashed pin.
        /// </summary>
        /// <param name="eMAId">The e-mobility account identification.</param>
        /// <param name="HashedPIN">The hashed pin.</param>
        /// <param name="Function">The crypto function.</param>
        /// <param name="Salt">The salt of the crypto function.</param>
        public eMAIdWithPIN(eMobilityAccount_Id  eMAId,
                            String               HashedPIN,
                            PINCrypto            Function,
                            String               Salt = "")
        {

            this.eMAId     = eMAId;
            this.PIN       = HashedPIN;
            this.Function  = Function;
            this.Salt      = Salt;

        }

        #endregion

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv            = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:AuthenticationData = "http://www.hubject.com/b2b/services/authenticationdata/v2.0"
        //                   xmlns:CommonTypes        = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        // 
        // [...]
        // 
        //    <CommonTypes:QRCodeIdentification>
        // 
        //       <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        // 
        //       <!--You have a CHOICE of the next 2 items at this level-->
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

        #region (static) Parse(QRCodeIdentificationXML, OnException = null)

        /// <summary>
        /// Parse the givem XML as an e-mobility account identification with (hashed) pin.
        /// </summary>
        /// <param name="eMAIdWithPinXML">A XML representation of an e-mobility account identification with (hashed) pin.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static eMAIdWithPIN Parse(XElement             eMAIdWithPinXML,
                                         OnExceptionDelegate  OnException  = null)
        {

            var _eMobilityAccount_Id       = eMAIdWithPinXML.MapValueOrFail(OICPNS.CommonTypes + "EVCOID",
                                                                            eMobilityAccount_Id.Parse,
                                                                            "The 'EVCOID' XML tag could not be found!");

            var PINXML        = eMAIdWithPinXML.Element(OICPNS.CommonTypes + "PIN");
            var HashedPINXML  = eMAIdWithPinXML.Element(OICPNS.CommonTypes + "HashedPIN");

            if (PINXML != null)
                return new eMAIdWithPIN(_eMobilityAccount_Id,
                                        PINXML.Value.IsNotNullOrEmpty() ? PINXML.Value : String.Empty);


            var ValueXML     = HashedPINXML.Element(OICPNS.CommonTypes + "Value");
            var FunctionXML  = HashedPINXML.Element(OICPNS.CommonTypes + "Function");
            var SaltXML      = HashedPINXML.Element(OICPNS.CommonTypes + "Salt");

            if (ValueXML    == null)
                throw new Exception("Invalid 'HashedPIN Value'!");

            if (FunctionXML == null || (FunctionXML.Value != "MD5" && FunctionXML.Value != "SHA-1"))
                throw new Exception("Invalid 'HashedPIN Function'!");

            return new eMAIdWithPIN(_eMobilityAccount_Id,
                                    ValueXML.Value.IsNotNullOrEmpty() ? ValueXML.Value : String.Empty,
                                    FunctionXML.Value == "MD5" ? PINCrypto.MD5 : PINCrypto.SHA1,
                                    SaltXML != null ? SaltXML.Value : String.Empty);

        }

        #endregion

        #region ToXML(Namespace = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XMLNamespace">The XML namespace to use.</param>
        public XElement ToXML(XNamespace Namespace = null)

            => new XElement((Namespace ?? OICPNS.CommonTypes) + "QRCodeIdentification",

                   new XElement(OICPNS.CommonTypes + "EVCOID", eMAId.ToString()),

                   Function == PINCrypto.none

                       ? new XElement(OICPNS.CommonTypes + "PIN", PIN)

                       : new XElement(OICPNS.CommonTypes + "HashedPIN",
                             new XElement(OICPNS.CommonTypes + "Value",      PIN),
                             new XElement(OICPNS.CommonTypes + "Function",   Function == PINCrypto.MD5 ? "MD5" : "SHA-1"),
                             new XElement(OICPNS.CommonTypes + "Salt",       Salt))

                   );

        #endregion



        #region IComparable<eMAIdWithPIN> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            // Check if the given object is an e-mobility account identification with (hashed) pin.
            var eMAIdWithPIN = Object as eMAIdWithPIN;
            if ((Object) eMAIdWithPIN == null)
                throw new ArgumentException("The given object is not an e-mobility account identification with (hashed) pin!");

            return CompareTo(eMAIdWithPIN);

        }

        #endregion

        #region CompareTo(eMAIdWithPIN)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdWithPIN">An e-mobility account identification with (hashed) pin object to compare with.</param>
        public Int32 CompareTo(eMAIdWithPIN eMAIdWithPIN)
        {

            if ((Object) eMAIdWithPIN == null)
                throw new ArgumentNullException(nameof(eMAIdWithPIN),  "The given e-mobility account identification with (hashed) pin must not be null!");

            var result = eMAId.CompareTo(eMAIdWithPIN.eMAId);
            if (result != 0)
                return result;

            result = String.Compare(PIN, eMAIdWithPIN.PIN, StringComparison.Ordinal);
            if (result != 0)
                return result;

            result = Function.CompareTo(eMAIdWithPIN.Function);
            if (result != 0)
                return result;

            return String.Compare(Salt, eMAIdWithPIN.Salt, StringComparison.Ordinal);

        }

        #endregion

        #endregion

        #region IEquatable<ChargeDetailRecord> Members

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

            // Check if the given object is an e-mobility account identification with (hashed) pin.
            var eMAIdWithPIN = Object as eMAIdWithPIN;
            if ((Object) eMAIdWithPIN == null)
                return false;

            return this.Equals(eMAIdWithPIN);

        }

        #endregion

        #region Equals(eMAIdWithPIN)

        /// <summary>
        /// Compares two e-mobility account identifications with (hashed) pins for equality.
        /// </summary>
        /// <param name="eMAIdWithPIN">An e-mobility account identification with (hashed) pin to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(eMAIdWithPIN eMAIdWithPIN)
        {

            if ((Object) eMAIdWithPIN == null)
                return false;

            if (!eMAId.Equals(eMAIdWithPIN.eMAId))
                return false;

            if (!PIN.Equals(eMAIdWithPIN.PIN))
                return false;

            if (!Function.Equals(eMAIdWithPIN.Function))
                return false;

            return Salt.Equals(eMAIdWithPIN.Salt);

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
                return eMAId.GetHashCode() * 23 ^ PIN.GetHashCode() * 17 ^ Function.GetHashCode() * 7 ^ Salt.GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(eMAId.ToString(), " -", Function != PINCrypto.none ? Function.ToString(): "", "-> ", PIN );

        #endregion

    }

}
