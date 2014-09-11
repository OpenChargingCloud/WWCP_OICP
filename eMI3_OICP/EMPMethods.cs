/*
 * Copyright (c) 2014 Achim Friedland <achim.friedland@graphdefined.com>
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
using System.Xml.Linq;

using eu.Vanaheimr.Aegir;

#endregion

namespace com.graphdefined.eMI3.IO.OICP
{

    /// <summary>
    /// EMP management operations.
    /// </summary>
    public static class EMPMethods
    {

        #region SearchRequestXML(GeoCoordinate, Distance, ProviderId = "8BD")

        /// <summary>
        /// Create a new EVSE Search request.
        /// </summary>
        /// <param name="GeoCoordinate">The geo coordinate of the search center.</param>
        /// <param name="DistanceKM">The search distance relative to the search center.</param>
        /// <param name="ProviderId">Your electromobility provider identification (EMP Id).</param>
        public static XElement SearchRequestXML(GeoCoordinate  GeoCoordinate,
                                                UInt64         DistanceKM,
                                                String         ProviderId = "8BD")
        {

            return SOAP.Encapsulation(new XElement(NS.OICPv1EVSESearch + "HubjectSearchEvse",

                                          new XElement(NS.OICPv1EVSESearch + "GeoCoordinates",
                                              new XElement(NS.OICPv1CommonTypes + "DecimalDegree",
                                                 new XElement(NS.OICPv1CommonTypes + "Longitude", GeoCoordinate.Longitude.ToString().Replace(',', '.')),
                                                 new XElement(NS.OICPv1CommonTypes + "Latitude",  GeoCoordinate.Latitude.ToString().Replace(',', '.'))
                                              )
                                          ),

                                          new XElement(NS.OICPv1EVSESearch + "ProviderID", ProviderId),

                                          new XElement(NS.OICPv1EVSESearch + "Range", DistanceKM)

                                     ));

        }

        #endregion


        #region MobileAuthorizeStartXML(EVSEId, EVCOId, PIN, PartnerProductId = null)

        /// <summary>
        /// Create a new mobile AuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="EVCOId"></param>
        /// <param name="PIN"></param>
        /// <param name="PartnerProductId">Your charging product identification (optional).</param>
        public static XElement MobileAuthorizeStartXML(EVSE_Id  EVSEId,
                                                       eMA_Id   EVCOId,
                                                       String   PIN,
                                                       String   PartnerProductId = null)
        {

            return SOAP.Encapsulation(new XElement(NS.OICPv1MobileAuthorization + "HubjectMobileAuthorizeStart",

                                          new XElement(NS.OICPv1MobileAuthorization + "EvseID", EVSEId.ToString()),

                                          new XElement(NS.OICPv1MobileAuthorization + "QRCodeIdentification",
                                              new XElement(NS.OICPv1CommonTypes + "EVCOID", EVCOId.ToString()),
                                              new XElement(NS.OICPv1CommonTypes + "PIN",    PIN)
                                          ),

                                          (PartnerProductId != null)
                                              ? new XElement(NS.OICPv1MobileAuthorization + "PartnerProductID", PartnerProductId)
                                              : null

                                     ));

        }

        #endregion

        #region MobileRemoteStartXML(SessionId = null)

        /// <summary>
        /// Create a new mobile AuthorizeStart request.
        /// </summary>
        /// <param name="SessionId">An OICP session identification from the MobileAuthorizationStart response.</param>
        public static XElement MobileRemoteStartXML(ChargingSession_Id  SessionId = null)
        {

            return SOAP.Encapsulation(new XElement(NS.OICPv1MobileAuthorization + "HubjectMobileRemoteStart",
                                          new XElement(NS.OICPv1EVSESearch + "SessionID", (SessionId != null) ? SessionId : ChargingSession_Id.New)
                                     ));

        }

        #endregion

        #region MobileRemoteStopXML(SessionId = null)

        /// <summary>
        /// Create a new mobile AuthorizeStop request.
        /// </summary>
        /// <param name="SessionId">The OICP session identification from the MobileAuthorizationStart response.</param>
        public static XElement MobileRemoteStopXML(ChargingSession_Id SessionId = null)
        {

            return SOAP.Encapsulation(new XElement(NS.OICPv1MobileAuthorization + "HubjectMobileRemoteStop",
                                          new XElement(NS.OICPv1EVSESearch + "SessionID", (SessionId != null) ? SessionId : ChargingSession_Id.New)
                                     ));

        }

        #endregion

    }

}
