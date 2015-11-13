/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/WorldWideCharging/WWCP_OICP>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
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

using org.GraphDefined.Vanaheimr.Illias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    public class eMAIdWithPIN
    {

        #region Properties

        #region eMAId

        private readonly eMA_Id _eMAId;

        public eMA_Id eMAId
        {
            get
            {
                return _eMAId;
            }
        }

        #endregion

        #region PIN

        private readonly String _PIN;

        public String PIN
        {
            get
            {
                return _PIN;
            }
        }

        #endregion

        #region Function

        private readonly PINCrypto _Function;

        public PINCrypto Function
        {
            get
            {
                return _Function;
            }
        }

        #endregion

        #region Salt

        private readonly String _Salt;

        public String Salt
        {
            get
            {
                return _Salt;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region eMAIdWithPIN(eMAId, PIN)

        public eMAIdWithPIN(eMA_Id  eMAId,
                            String  PIN)
        {

            this._eMAId     = eMAId;
            this._PIN       = PIN;
            this._Function  = PINCrypto.none;

        }

        #endregion

        #region eMAIdWithPIN(eMAId, PIN, Function, Salt = "")

        public eMAIdWithPIN(eMA_Id     eMAId,
                            String     PIN,
                            PINCrypto  Function,
                            String     Salt = "")
        {

            this._eMAId     = eMAId;
            this._PIN       = PIN;
            this._Function  = Function;
            this._Salt      = Salt;

        }

        #endregion

        #endregion


        #region (static) Parse(QRCodeIdentificationXML)

        public static eMAIdWithPIN Parse(XElement QRCodeIdentificationXML)
        {

            String _PIN    = null;

            var _eMA_Id       = QRCodeIdentificationXML.MapValueOrFail(OICPNS.CommonTypes + "EVCOID",
                                                                       eMA_Id.Parse,
                                                                       "The 'EVCOID' XML tag could not be found!");

            var PINXML        = QRCodeIdentificationXML.Element(OICPNS.CommonTypes + "PIN");
            var HashedPINXML  = QRCodeIdentificationXML.Element(OICPNS.CommonTypes + "HashedPIN");

            if (PINXML != null)
                return new eMAIdWithPIN(_eMA_Id,
                                        PINXML.Value.IsNotNullOrEmpty() ? PINXML.Value : String.Empty);


            var ValueXML     = HashedPINXML.Element(OICPNS.CommonTypes + "Value");
            var FunctionXML  = HashedPINXML.Element(OICPNS.CommonTypes + "Function");
            var SaltXML      = HashedPINXML.Element(OICPNS.CommonTypes + "Salt");

            if (ValueXML    == null)
                throw new Exception("Invalid 'HashedPIN Value'!");

            if (FunctionXML == null || (FunctionXML.Value != "MD5" && FunctionXML.Value != "SHA-1"))
                throw new Exception("Invalid 'HashedPIN Function'!");

            return new eMAIdWithPIN(_eMA_Id,
                                    ValueXML.Value.IsNotNullOrEmpty() ? ValueXML.Value : String.Empty,
                                    FunctionXML.Value == "MD5" ? PINCrypto.MD5 : PINCrypto.SHA1,
                                    SaltXML != null ? SaltXML.Value : String.Empty);

        }

        #endregion

        #region ToXML()

        public XElement ToXML( )
        {

            return new XElement(OICPNS.CommonTypes + "QRCodeIdentification",

                          new XElement(OICPNS.CommonTypes + "EVCOID", _eMAId.ToString()),

                          _Function == PINCrypto.none

                              ? new XElement(OICPNS.CommonTypes + "PIN", _PIN)

                              : new XElement(OICPNS.CommonTypes + "HashedPIN",
                                    new XElement(OICPNS.CommonTypes + "Value",      _PIN),
                                    new XElement(OICPNS.CommonTypes + "Function",   _Function == PINCrypto.MD5 ? "MD5" : "SHA-1"),
                                    new XElement(OICPNS.CommonTypes + "Salt",       _Salt))

                          );

        }

        #endregion


        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return String.Concat(_eMAId.ToString(), " -", _Function != PINCrypto.none ? _Function.ToString(): "", "-> ", _PIN );
        }

        #endregion
    }

}
