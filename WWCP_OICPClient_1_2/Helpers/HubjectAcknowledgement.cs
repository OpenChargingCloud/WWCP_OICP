/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICPClient <https://github.com/WorldWideCharging/WWCP_OICPClient>
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

using System;
using System.Linq;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPClient_1_2
{

    /// <summary>
    /// A Hubject Acknowledgement result.
    /// </summary>
    public class HubjectAcknowledgement
    {

        #region Properties

        #region Result

        private readonly Boolean _Result;

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public Boolean Result
        {
            get
            {
                return _Result;
            }
        }

        #endregion

        #region Code

        private readonly UInt16 _Code;

        /// <summary>
        /// The result code of the operation.
        /// </summary>
        public UInt16 Code
        {
            get
            {
                return _Code;
            }
        }

        #endregion

        #region Description

        private readonly String _Description;

        /// <summary>
        /// The description of the result code.
        /// </summary>
        public String Description
        {
            get
            {
                return _Description;
            }
        }

        #endregion

        #region AdditionalInfo

        private readonly String _AdditionalInfo;

        /// <summary>
        /// Additional information.
        /// </summary>
        public String AdditionalInfo
        {
            get
            {
                return _AdditionalInfo;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Hubject Acknowledgement result.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Code">The result code of the operation.</param>
        /// <param name="Description">The description of the result code.</param>
        /// <param name="AdditionalInfo">Additional information.</param>
        public HubjectAcknowledgement(Boolean  Result,
                                      UInt16   Code,
                                      String   Description,
                                      String   AdditionalInfo)
        {

            this._Result          = Result;
            this._Code            = Code;
            this._Description     = Description;
            this._AdditionalInfo  = AdditionalInfo;

        }

        #endregion


        #region (static) Parse(XML)

        /// <summary>
        /// Create a new Hubject Acknowledgement result.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        public static HubjectAcknowledgement Parse(XElement XML)
        {

            HubjectAcknowledgement _Acknowledgement;

            if (TryParse(XML, out _Acknowledgement))
                return _Acknowledgement;

            return null;

        }

        #endregion

        #region (static) TryParse(XML, out Acknowledgement)

        /// <summary>
        /// Create a new Hubject Acknowledgement result.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        /// <param name="Acknowledgement">The parsed acknowledgement</param>
        public static Boolean TryParse(XElement XML, out HubjectAcknowledgement Acknowledgement)
        {

            Acknowledgement = null;

            try
            {

                var ack              = XML.Descendants(NS.OICPv1_2CommonTypes + "eRoamingAcknowledgement").
                                           FirstOrDefault();

                if (ack == null && XML.Name == NS.OICPv1_2CommonTypes + "eRoamingAcknowledgement")
                    ack = XML;

                if (ack == null)
                    return false;

                var _Result          = (ack.Element(NS.OICPv1_2CommonTypes + "Result").Value == "true")
                                           ? true
                                           : false;

                var StatusCode       = ack.Element(NS.OICPv1_2CommonTypes + "StatusCode");

                UInt16 _Code;
                if (!UInt16.TryParse(StatusCode.Element(NS.OICPv1_2CommonTypes + "Code").Value, out _Code))
                    return false;

                var _Description     = StatusCode.Element(NS.OICPv1_2CommonTypes + "Description").Value;

                var _AdditionalInfo  = (StatusCode.Element(NS.OICPv1_2CommonTypes + "AdditionalInfo") != null)
                                           ? StatusCode.Element(NS.OICPv1_2CommonTypes + "AdditionalInfo").Value
                                           : String.Empty;

                Acknowledgement = new HubjectAcknowledgement(_Result, _Code, _Description, _AdditionalInfo);

                return true;

            }
            catch (Exception e)
            {
                return false;
            }

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return Description;
        }

        #endregion

    }

}
