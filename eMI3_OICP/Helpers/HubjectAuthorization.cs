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

    #region (abstract) HubjectAuthorization

    /// <summary>
    /// An abstract Hubject Authorization result.
    /// </summary>
    public abstract class HubjectAuthorization
    {

        #region (enum) AuthorizationType

        /// <summary>
        /// Type of authorization result.
        /// </summary>
        public enum AuthorizationType
        {
            Start,
            Stop
        }

        #endregion

        #region Properties

        #region SessionID

        private readonly ChargingSession_Id _SessionID;

        /// <summary>
        /// The Hubject session identification.
        /// </summary>
        public ChargingSession_Id SessionID
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

        private readonly AuthorizationStatusType _AuthorizationStatus;

        /// <summary>
        /// The authorization status, e.g. "Authorized".
        /// </summary>
        public AuthorizationStatusType AuthorizationStatus
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
        /// A description of the result.
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
        /// An additional information on the result.
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

        #region Constructor

        /// <summary>
        /// Create a new abstract Hubject Authorization result.
        /// </summary>
        /// <param name="AuthorizationType">The type of authorization [start|stop].</param>
        /// <param name="XML">The XML to parse.</param>
        public HubjectAuthorization(AuthorizationType AuthorizationType, XElement XML)
        {

            var ack                   = XML.Descendants(NS.OICPv1Authorization + "HubjectAuthorization" + AuthorizationType.ToString()).FirstOrDefault();

            this._SessionID           = ChargingSession_Id .Parse((ack.Element(NS.OICPv1Authorization + "SessionID") != null)  ? ack.Element(NS.OICPv1Authorization + "SessionID"). Value : "");
            this._PartnerSessionID    =  ack.Element(NS.OICPv1Authorization + "PartnerSessionID").Value;
            this._ProviderID          = (ack.Element(NS.OICPv1Authorization + "ProviderID") != null) ? ack.Element(NS.OICPv1Authorization + "ProviderID").Value : "";
            this._AuthorizationStatus = (ack.Element(NS.OICPv1Authorization + "AuthorizationStatus").Value.ToLower() == "authorized") ? AuthorizationStatusType.Authorized : AuthorizationStatusType.NotAuthorized;

            var StatusCode            = ack.Element(NS.OICPv1Authorization + "StatusCode");
            this._Code                = UInt16.Parse(StatusCode.Element(NS.OICPv1CommonTypes + "Code").Value);
            this._Description         =  StatusCode.Element(NS.OICPv1CommonTypes + "Description").Value;
            this._AdditionalInfo      = (StatusCode.Element(NS.OICPv1CommonTypes + "AdditionalInfo") != null) ? StatusCode.Element(NS.OICPv1CommonTypes + "AdditionalInfo").Value : String.Empty;

            // - Auth Start --------------------------------------------------------------------------------------------

            // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
            //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
            //   <soapenv:Body>
            //     <tns:HubjectAuthorizationStart>
            //       <tns:SessionID>60ce73f6-0a88-1296-3d3d-623fdd276ddc</tns:SessionID> 
            //       <tns:PartnerSessionID>9ab07cb6-ac05-4f17-b944-8fe87682d646</tns:PartnerSessionID> 
            //       <tns:ProviderID>BMW</tns:ProviderID> 
            //       <tns:AuthorizationStatus>Authorized</tns:AuthorizationStatus> 
            //       <tns:StatusCode>
            //         <cmn:Code>000</cmn:Code> 
            //         <cmn:Description>Success</cmn:Description> 
            //       </tns:StatusCode>
            //     </tns:HubjectAuthorizationStart>
            //   </soapenv:Body>
            // </soapenv:Envelope>

            // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
            //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
            //   <soapenv:Body>
            //     <tns:HubjectAuthorizationStart>
            //       <tns:PartnerSessionID>0815</tns:PartnerSessionID> 
            //       <tns:AuthorizationStatus>NotAuthorized</tns:AuthorizationStatus> 
            //       <tns:StatusCode>
            //         <cmn:Code>320</cmn:Code> 
            //         <cmn:Description>Service not available</cmn:Description> 
            //       </tns:StatusCode>
            //     </tns:HubjectAuthorizationStart>
            //   </soapenv:Body>
            // </soapenv:Envelope>

            // - Auth Stop ---------------------------------------------------------------------------------------------

            // <?xml version="1.0" encoding="UTF-8" ?> 
            // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
            //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
            //   <soapenv:Body>
            //     <tns:HubjectAuthorizationStop>
            //       <tns:SessionID>60dfacc8-0a88-1296-1aef-b675131f4510</tns:SessionID> 
            //       <tns:PartnerSessionID>5d4d5ff4-6f08-4f49-8e2b-db46ae3f2bc9</tns:PartnerSessionID> 
            //       <tns:ProviderID>BMW</tns:ProviderID> 
            //       <tns:AuthorizationStatus>Authorized</tns:AuthorizationStatus> 
            //       <tns:StatusCode>
            //         <cmn:Code>000</cmn:Code> 
            //         <cmn:Description>Success</cmn:Description> 
            //       </tns:StatusCode>
            //     </tns:HubjectAuthorizationStop>
            //   </soapenv:Body>
            // </soapenv:Envelope>

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
