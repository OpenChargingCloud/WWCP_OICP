/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of eMI3 OICP <http://www.github.com/eMI3/OICP-Bindings>
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

namespace org.GraphDefined.eMI3.IO.OICP
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
        /// The result code of the operation.
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

        #region HubjectAcknowledgement(XML)

        /// <summary>
        /// Create a new Hubject Acknowledgement result.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        public HubjectAcknowledgement(XElement XML)
        {

            var ack               = XML.Descendants(NS.OICPv1CommonTypes + "HubjectAcknowledgement").FirstOrDefault();
            this._Result          = (ack.Element(NS.OICPv1CommonTypes + "Result").Value == "true") ? true : false;

            var StatusCode        = ack.Element(NS.OICPv1CommonTypes + "StatusCode");
            this._Code            = UInt16.Parse(StatusCode.Element(NS.OICPv1CommonTypes + "Code").Value);
            this._Description     = StatusCode.Element(NS.OICPv1CommonTypes + "Description").Value;
            this._AdditionalInfo  = (StatusCode.Element(NS.OICPv1CommonTypes + "AdditionalInfo") != null) ? StatusCode.Element(NS.OICPv1CommonTypes + "AdditionalInfo").Value : String.Empty;

        }

        #endregion

        #endregion

        #region (static) Parse(XML)

        /// <summary>
        /// Create a new Hubject Acknowledgement result.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        public static HubjectAcknowledgement Parse(XElement XML)
        {
            try
            {
                return new HubjectAcknowledgement(XML);
            }
            catch (Exception e)
            {
                return null;
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
