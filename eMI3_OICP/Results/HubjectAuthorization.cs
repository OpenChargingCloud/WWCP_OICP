/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
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

namespace org.emi3group.IO.OICP
{

    #region AuthorizationType

    /// <summary>
    /// Type of authorization result.
    /// </summary>
    public enum AuthorizationType
    {
        Start,
        Stop
    }

    #endregion

    #region HubjectAuthorization

    /// <summary>
    /// An abstract Hubject Authorization result.
    /// </summary>
    public abstract class HubjectAuthorization
    {

        #region Properties

        #region SessionID

        private readonly String _SessionID;

        /// <summary>
        /// The Hubject session identification.
        /// </summary>
        public String SessionID
        {
            get
            {
                return _SessionID;
            }
        }

        #endregion

        #region PartnerSessionID

        private readonly String _PartnerSessionID;

        /// <summary>
        /// Your own session identification.
        /// </summary>
        public String PartnerSessionID
        {
            get
            {
                return _PartnerSessionID;
            }
        }

        #endregion

        #region ProviderID

        private readonly String _ProviderID;

        /// <summary>
        /// The provider identification, e.g. BMW.
        /// </summary>
        public String ProviderID
        {
            get
            {
                return _ProviderID;
            }
        }

        #endregion

        #region AuthorizationStatus

        private readonly String _AuthorizationStatus;

        /// <summary>
        /// The authorization status, e.g. "ACCEPTED".
        /// </summary>
        public String AuthorizationStatus
        {
            get
            {
                return _AuthorizationStatus;
            }
        }

        #endregion

        #region Code

        private readonly UInt16 _Code;

        /// <summary>
        /// The result code.
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
        /// An description of the result.
        /// </summary>
        public String Description
        {
            get
            {
                return _Description;
            }
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new abstract Hubject Authorization result.
        /// </summary>
        /// <param name="AuthorizationType">The type of authorization [start|stop].</param>
        /// <param name="XML">The XML to parse.</param>
        public HubjectAuthorization(AuthorizationType AuthorizationType, XElement XML)
        {

            var ack                   = XML.Descendants(NS.OICPv1Authorization + "HubjectAuthorization" + AuthorizationType.ToString()).FirstOrDefault();

            this._SessionID           = ack.Element(NS.OICPv1Authorization + "SessionID").Value;
            this._PartnerSessionID    = ack.Element(NS.OICPv1Authorization + "PartnerSessionID").Value;
            this._ProviderID          = ack.Element(NS.OICPv1Authorization + "ProviderID").Value;
            this._AuthorizationStatus = ack.Element(NS.OICPv1Authorization + "AuthorizationStatus").Value;

            var StatusCode            = ack.Element(NS.OICPv1Authorization + "StatusCode");
            this._Code                = UInt16.Parse(StatusCode.Element(NS.OICPv1CommonTypes + "Code").Value);
            this._Description         = StatusCode.Element(NS.OICPv1CommonTypes + "Description").Value;

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

    #endregion

    #region HubjectAuthorizationStart

    /// <summary>
    /// A Hubject Authorization Start result.
    /// </summary>
    public class HubjectAuthorizationStart : HubjectAuthorization
    {

        #region Constructor

        /// <summary>
        /// Create a new Hubject Authorization Start result.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        public HubjectAuthorizationStart(XElement XML)
            : base(AuthorizationType.Start, XML)
        { }

        #endregion

        #region (static) Parse(XML)

        /// <summary>
        /// Create a new Hubject Authorization Start result.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        public static HubjectAuthorizationStart Parse(XElement XML)
        {
            try
            {
                return new HubjectAuthorizationStart(XML);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion

    }

    #endregion

    #region HubjectAuthorizationStop

    /// <summary>
    /// A Hubject Authorization Stop result.
    /// </summary>
    public class HubjectAuthorizationStop : HubjectAuthorization
    {

        #region Constructor

        /// <summary>
        /// Create a new Hubject Authorization Stop result.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        public HubjectAuthorizationStop(XElement XML)
            : base(AuthorizationType.Stop, XML)
        { }

        #endregion

        #region (static) Parse(XML)

        /// <summary>
        /// Create a new Hubject Authorization Stop result.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        public static HubjectAuthorizationStop Parse(XElement XML)
        {
            try
            {
                return new HubjectAuthorizationStop(XML);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion

    }

    #endregion

}
