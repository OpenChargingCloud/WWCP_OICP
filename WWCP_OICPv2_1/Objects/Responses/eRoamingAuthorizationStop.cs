/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An OICP authorization stop result.
    /// </summary>
    public class eRoamingAuthorizationStop
    {

        #region Properties

        /// <summary>
        /// The charging session identification.
        /// </summary>
        public ChargingSession_Id       SessionId              { get; }

        /// <summary>
        /// An optional partner charging session identification.
        /// </summary>
        public ChargingSession_Id       PartnerSessionId       { get; }

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        public EVSP_Id                  ProviderId             { get; }

        /// <summary>
        /// The authorization status, e.g. "Authorized".
        /// </summary>
        public AuthorizationStatusType  AuthorizationStatus    { get; }

        /// <summary>
        /// The authorization status code.
        /// </summary>
        public StatusCode               StatusCode             { get; }

        #endregion

        #region Constructor(s)

        #region (private) eRoamingAuthorizationStop(AuthorizationStatus, ...)

        /// <summary>
        /// Create a new OICP authorization stop result.
        /// </summary>
        /// <param name="AuthorizationStatus">The authorization status.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="StatusCode">An optional status code.</param>
        private eRoamingAuthorizationStop(AuthorizationStatusType  AuthorizationStatus,
                                          ChargingSession_Id       SessionId         = null,
                                          ChargingSession_Id       PartnerSessionId  = null,
                                          EVSP_Id                  ProviderId        = null,
                                          StatusCode               StatusCode        = null)
        {

            this.AuthorizationStatus  = AuthorizationStatus;
            this.SessionId            = SessionId;
            this.PartnerSessionId     = PartnerSessionId;
            this.ProviderId           = ProviderId;
            this.StatusCode           = StatusCode;

        }

        #endregion

        #region eRoamingAuthorizationStop(SessionId, ...)

        /// <summary>
        /// Create a new OICP 'Authorized' authorization stop result.
        /// </summary>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        public eRoamingAuthorizationStop(ChargingSession_Id  SessionId,
                                         EVSP_Id             ProviderId                = null,
                                         ChargingSession_Id  PartnerSessionId          = null,
                                         String              StatusCodeDescription     = null,
                                         String              StatusCodeAdditionalInfo  = null)

            : this(AuthorizationStatusType.Authorized,
                   SessionId,
                   PartnerSessionId,
                   ProviderId,
                   new StatusCode(StatusCodes.Success,
                                  StatusCodeDescription,
                                  StatusCodeAdditionalInfo))

        { }

        #endregion

        #region eRoamingAuthorizationStop(StatusCode, ...)

        /// <summary>
        /// Create a new OICP 'NotAuthorized' authorization stop result.
        /// </summary>
        /// <param name="StatusCode">The status code of the operation.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public eRoamingAuthorizationStop(StatusCodes              StatusCode,
                                         String                   StatusCodeDescription     = null,
                                         String                   StatusCodeAdditionalInfo  = null,
                                         ChargingSession_Id       SessionId                 = null,
                                         ChargingSession_Id       PartnerSessionId          = null,
                                         EVSP_Id                  ProviderId                = null)
        {

            this.AuthorizationStatus  = AuthorizationStatusType.NotAuthorized;
            this.SessionId            = SessionId;
            this.PartnerSessionId     = PartnerSessionId;
            this.ProviderId           = ProviderId;
            this.StatusCode           = new StatusCode(StatusCode,
                                                       StatusCodeDescription,
                                                       StatusCodeAdditionalInfo);

        }

        #endregion

        #endregion


        #region (static) Parse(eRoamingAuthorizationStopXML)

        /// <summary>
        /// Parse the given XML representation of an OICP authorization stop result.
        /// </summary>
        /// <param name="eRoamingAuthorizationStopXML">The XML to parse.</param>
        public static eRoamingAuthorizationStop Parse(XElement eRoamingAuthorizationStopXML)
        {

            if (eRoamingAuthorizationStopXML.Name != OICPNS.Authorization + "eRoamingAuthorizationStop")
                throw new ArgumentException("Invalid eRoamingAuthorizationStop XML");

            return new eRoamingAuthorizationStop(
                           (AuthorizationStatusType) Enum.Parse(typeof(AuthorizationStatusType), eRoamingAuthorizationStopXML.ElementValueOrFail(OICPNS.Authorization + "AuthorizationStatus")),
                           eRoamingAuthorizationStopXML.MapValueOrNull(OICPNS.Authorization + "SessionID",         ChargingSession_Id.Parse),
                           eRoamingAuthorizationStopXML.MapValueOrNull(OICPNS.Authorization + "PartnerSessionID",  ChargingSession_Id.Parse),
                           eRoamingAuthorizationStopXML.MapValueOrNull(OICPNS.Authorization + "ProviderID",        EVSP_Id.           Parse),
                           eRoamingAuthorizationStopXML.MapElement    (OICPNS.Authorization + "StatusCode",        StatusCode.        Parse)
                       );

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()
            => new XElement(OICPNS.Authorization + "eRoamingAuthorizationStop",

                SessionId != null
                    ? new XElement(OICPNS.Authorization + "SessionID",         SessionId.ToString())
                    : null,

                PartnerSessionId != null
                    ? new XElement(OICPNS.Authorization + "PartnerSessionID",  PartnerSessionId.ToString())
                    : null,

                ProviderId != null
                    ? new XElement(OICPNS.Authorization + "ProviderID",        ProviderId.ToString())
                    : null,

                new XElement(OICPNS.Authorization + "AuthorizationStatus",     AuthorizationStatus.ToString()),

                StatusCode.ToXML()

            );

        #endregion

    }

}
