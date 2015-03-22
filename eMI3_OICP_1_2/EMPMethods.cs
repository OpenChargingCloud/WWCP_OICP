/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
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
using System.Globalization;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Aegir;

using com.graphdefined.eMI3.IO.OICP;

#endregion

namespace com.graphdefined.eMI3.IO.OICP_1_2
{

    /// <summary>
    /// EMP management operations.
    /// </summary>
    public static class EMPMethods
    {

        #region SearchRequestXML(GeoCoordinate, Distance, ProviderId)

        /// <summary>
        /// Create a new EVSE Search request.
        /// </summary>
        /// <param name="GeoCoordinate">The geo coordinate of the search center.</param>
        /// <param name="DistanceKM">The search distance relative to the search center.</param>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        public static XElement SearchRequestXML(GeoCoordinate  GeoCoordinate,
                                                UInt64         DistanceKM,
                                                String         ProviderId)
        {

            return SOAP.Encapsulation(new XElement(NS.OICPv1_2EVSESearch + "eRoamingSearchEvse",

                                          new XElement(NS.OICPv1_2EVSESearch + "GeoCoordinates",
                                              new XElement(NS.OICPv1_2CommonTypes + "DecimalDegree",
                                                 new XElement(NS.OICPv1_2CommonTypes + "Longitude", GeoCoordinate.Longitude.ToString(CultureInfo.InvariantCulture.NumberFormat)),
                                                 new XElement(NS.OICPv1_2CommonTypes + "Latitude",  GeoCoordinate.Latitude. ToString(CultureInfo.InvariantCulture.NumberFormat))
                                              )
                                          ),

                                          new XElement(NS.OICPv1_2EVSESearch + "ProviderID", ProviderId),

                                          new XElement(NS.OICPv1_2EVSESearch + "Range", DistanceKM)

                                     ));

        }

        #endregion

        #region PullEVSEDataRequestXML(ProviderId, LastCall, GeoCoordinate, Distance)

        /// <summary>
        /// Create a new Pull EVSE Data request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="LastCall">An optional timestamp of the last call.</param>
        /// <param name="GeoCoordinate">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        public static XElement PullEVSEDataRequestXML(String         ProviderId,
                                                      DateTime?      LastCall       = null,
                                                      GeoCoordinate  GeoCoordinate  = null,
                                                      UInt64         DistanceKM     = 0)
        {

            return SOAP.Encapsulation(new XElement(NS.OICPv1_2EVSEData + "eRoamingPullEvseData",

                                          new XElement(NS.OICPv1_2EVSESearch + "ProviderID", ProviderId),

                                          (GeoCoordinate != null && DistanceKM > 0)
                                              ? new XElement(NS.OICPv1_2EVSEData + "SearchCenter",

                                                    new XElement(NS.OICPv1_2CommonTypes + "GeoCoordinates",
                                                        new XElement(NS.OICPv1_2CommonTypes + "DecimalDegree",
                                                           new XElement(NS.OICPv1_2CommonTypes + "Longitude", GeoCoordinate.Longitude.ToString(CultureInfo.InvariantCulture.NumberFormat)),
                                                           new XElement(NS.OICPv1_2CommonTypes + "Latitude",  GeoCoordinate.Latitude. ToString(CultureInfo.InvariantCulture.NumberFormat))
                                                        )
                                                    ),

                                                    new XElement(NS.OICPv1_2CommonTypes + "Radius", DistanceKM)

                                                )
                                              : null,

                                          (LastCall.HasValue)
                                              ? new XElement(NS.OICPv1_2EVSEData + "LastCall",  LastCall.Value)
                                              : null,

                                          new XElement(NS.OICPv1_2EVSEData + "GeoCoordinatesResponseFormat",  "DecimalDegree")

                                     ));

        }

        #endregion

        #region PullEVSEStatusRequestXML(ProviderId, LastCall, GeoCoordinate, Distance)

        /// <summary>
        /// Create a new Pull EVSE Status request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="GeoCoordinate">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        public static XElement PullEVSEStatusRequestXML(String         ProviderId,
                                                   //     DateTime?      LastCall       = null,
                                                        GeoCoordinate  GeoCoordinate  = null,
                                                        UInt64         DistanceKM     = 0)
        {

            return SOAP.Encapsulation(new XElement(NS.OICPv1_2EVSEData + "eRoamingPullEvseStatus",

                                          new XElement(NS.OICPv1_2EVSESearch + "ProviderID", ProviderId),

                                          (GeoCoordinate != null && DistanceKM > 0)
                                              ? new XElement(NS.OICPv1_2EVSEData + "SearchCenter",

                                                    new XElement(NS.OICPv1_2CommonTypes + "GeoCoordinates",
                                                        new XElement(NS.OICPv1_2CommonTypes + "DecimalDegree",
                                                           new XElement(NS.OICPv1_2CommonTypes + "Longitude", GeoCoordinate.Longitude.ToString(CultureInfo.InvariantCulture.NumberFormat)),
                                                           new XElement(NS.OICPv1_2CommonTypes + "Latitude",  GeoCoordinate.Latitude. ToString(CultureInfo.InvariantCulture.NumberFormat))
                                                        )
                                                    ),

                                                    new XElement(NS.OICPv1_2CommonTypes + "Radius", DistanceKM)

                                                )
                                              : null

                                     ));

        }

        #endregion

        #region PullEVSEStatusByIdRequestXML(ProviderId, LastCall, GeoCoordinate, Distance)

        /// <summary>
        /// Create a new Pull EVSE Status-By-Id request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="EVSEIds">Up to 100 EVSE Ids.</param>
        public static XElement PullEVSEStatusByIdRequestXML(String                ProviderId,
                                                            IEnumerable<EVSE_Id>  EVSEIds)
        {

            return SOAP.Encapsulation(new XElement(NS.OICPv1_2EVSEData + "eRoamingPullEvseStatusById",

                                          new XElement(NS.OICPv1_2EVSESearch + "ProviderID", ProviderId),

                                          EVSEIds.Select(EVSEId => new XElement(NS.OICPv1_2EVSEStatus + "EvseId", EVSEId.ToString())).
                                                  ToArray()

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

            return SOAP.Encapsulation(new XElement(NS.OICPv1_2MobileAuthorization + "eRoamingMobileAuthorizeStart",

                                          new XElement(NS.OICPv1_2MobileAuthorization + "EvseID", EVSEId.ToString()),

                                          new XElement(NS.OICPv1_2MobileAuthorization + "QRCodeIdentification",
                                              new XElement(NS.OICPv1_2CommonTypes + "EVCOID", EVCOId.ToString()),
                                              new XElement(NS.OICPv1_2CommonTypes + "PIN",    PIN)
                                          ),

                                          (PartnerProductId != null)
                                              ? new XElement(NS.OICPv1_2MobileAuthorization + "PartnerProductID", PartnerProductId)
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

            return SOAP.Encapsulation(new XElement(NS.OICPv1_2MobileAuthorization + "eRoamingMobileRemoteStart",
                                          new XElement(NS.OICPv1_2EVSESearch + "SessionID", (SessionId != null) ? SessionId : ChargingSession_Id.New)
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

            return SOAP.Encapsulation(new XElement(NS.OICPv1_2MobileAuthorization + "eRoamingMobileRemoteStop",
                                          new XElement(NS.OICPv1_2EVSESearch + "SessionID", (SessionId != null) ? SessionId : ChargingSession_Id.New)
                                     ));

        }

        #endregion

    }

}
