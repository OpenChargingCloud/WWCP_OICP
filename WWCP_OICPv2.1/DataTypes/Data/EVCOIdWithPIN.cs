﻿/*
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An electric vehicle contract identification with (hashed) pin.
    /// </summary>
    public struct EVCOIdWithPIN : IEquatable <EVCOIdWithPIN>,
                                  IComparable<EVCOIdWithPIN>,
                                  IComparable
    {

        #region Properties

        /// <summary>
        /// The electric vehicle contract identification.
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

        #region EVCOIdWithPIN(EVCOId, PIN)

        /// <summary>
        /// Create a new electric vehicle contract identification with pin.
        /// </summary>
        /// <param name="EVCOId">An electric vehicle contract identification.</param>
        /// <param name="PIN">A pin.</param>
        public EVCOIdWithPIN(EVCO_Id  EVCOId,
                             String   PIN)
        {

            this.EVCOId    = EVCOId;
            this.PIN       = PIN;
            this.Function  = PINCrypto.none;
            this.Salt      = "";

        }

        #endregion

        #region EVCOIdWithPIN(EVCOId, HashedPIN, Function, Salt = "")

        /// <summary>
        /// Create a new electric vehicle contract identification with a hashed pin.
        /// </summary>
        /// <param name="EVCOId">An electric vehicle contract identification.</param>
        /// <param name="HashedPIN">A hashed pin.</param>
        /// <param name="Function">A crypto function.</param>
        /// <param name="Salt">A salt of the crypto function.</param>
        public EVCOIdWithPIN(EVCO_Id    EVCOId,
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
        /// Parse the givem XML as an electric vehicle contract identification with (hashed) pin.
        /// </summary>
        /// <param name="EVCOIdWithPinXML">A XML representation of an electric vehicle contract identification with (hashed) pin.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVCOIdWithPIN Parse(XElement             EVCOIdWithPinXML,
                                         OnExceptionDelegate  OnException  = null)
        {

            var _EVCO_Id       = EVCOIdWithPinXML.MapValueOrFail(OICPNS.CommonTypes + "EVCOID",
                                                                            EVCO_Id.Parse,
                                                                            "The 'EVCOID' XML tag could not be found!");

            var PINXML        = EVCOIdWithPinXML.Element(OICPNS.CommonTypes + "PIN");
            var HashedPINXML  = EVCOIdWithPinXML.Element(OICPNS.CommonTypes + "HashedPIN");

            if (PINXML != null)
                return new EVCOIdWithPIN(_EVCO_Id,
                                        PINXML.Value.IsNotNullOrEmpty() ? PINXML.Value : String.Empty);


            var ValueXML     = HashedPINXML.Element(OICPNS.CommonTypes + "Value");
            var FunctionXML  = HashedPINXML.Element(OICPNS.CommonTypes + "Function");
            var SaltXML      = HashedPINXML.Element(OICPNS.CommonTypes + "Salt");

            if (ValueXML    == null)
                throw new Exception("Invalid 'HashedPIN Value'!");

            if (FunctionXML == null || (FunctionXML.Value != "MD5" && FunctionXML.Value != "SHA-1"))
                throw new Exception("Invalid 'HashedPIN Function'!");

            return new EVCOIdWithPIN(_EVCO_Id,
                                    ValueXML.Value.IsNotNullOrEmpty() ? ValueXML.Value : String.Empty,
                                    FunctionXML.Value == "MD5" ? PINCrypto.MD5 : PINCrypto.SHA1,
                                    SaltXML != null ? SaltXML.Value : String.Empty);

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">The XML name to use.</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OICPNS.CommonTypes + "QRCodeIdentification",

                   new XElement(OICPNS.CommonTypes + "EVCOID", EVCOId.ToString()),

                   Function == PINCrypto.none

                       ? new XElement(OICPNS.CommonTypes + "PIN", PIN)

                       : new XElement(OICPNS.CommonTypes + "HashedPIN",
                             new XElement(OICPNS.CommonTypes + "Value",      PIN),
                             new XElement(OICPNS.CommonTypes + "Function",   Function == PINCrypto.MD5 ? "MD5" : "SHA-1"),
                             new XElement(OICPNS.CommonTypes + "Salt",       Salt))

                   );

        #endregion


        #region IComparable<EVCOIdWithPIN> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            if (!(Object is EVCOIdWithPIN))
                throw new ArgumentException("The given object is not an electric vehicle contract identification with (hashed) pin!", nameof(Object));

            return CompareTo((EVCOIdWithPIN) Object);

        }

        #endregion

        #region CompareTo(EVCOIdWithPIN)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCOIdWithPIN">An electric vehicle contract identification with (hashed) pin object to compare with.</param>
        public Int32 CompareTo(EVCOIdWithPIN EVCOIdWithPIN)
        {

            if ((Object) EVCOIdWithPIN == null)
                throw new ArgumentNullException(nameof(EVCOIdWithPIN),  "The given electric vehicle contract identification with (hashed) pin must not be null!");

            var result = EVCOId.CompareTo(EVCOIdWithPIN.EVCOId);
            if (result != 0)
                return result;

            result = String.Compare(PIN, EVCOIdWithPIN.PIN, StringComparison.Ordinal);
            if (result != 0)
                return result;

            result = Function.CompareTo(EVCOIdWithPIN.Function);
            if (result != 0)
                return result;

            return String.Compare(Salt, EVCOIdWithPIN.Salt, StringComparison.Ordinal);

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

            if (!(Object is EVCOIdWithPIN))
                return false;

            return this.Equals((EVCOIdWithPIN) Object);

        }

        #endregion

        #region Equals(EVCOIdWithPIN)

        /// <summary>
        /// Compares two electric vehicle contract identifications with (hashed) pins for equality.
        /// </summary>
        /// <param name="EVCOIdWithPIN">An electric vehicle contract identification with (hashed) pin to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVCOIdWithPIN EVCOIdWithPIN)
        {

            if ((Object) EVCOIdWithPIN == null)
                return false;

            if (!EVCOId.Equals(EVCOIdWithPIN.EVCOId))
                return false;

            if (!PIN.Equals(EVCOIdWithPIN.PIN))
                return false;

            if (!Function.Equals(EVCOIdWithPIN.Function))
                return false;

            return Salt.Equals(EVCOIdWithPIN.Salt);

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVCOId.ToString(),
                             " -",
                             Function != PINCrypto.none ? Function.ToString(): "",
                             "-> ",
                             PIN );

        #endregion

    }

}