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
using System.Globalization;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OICPClient_1_2
{

    /// <summary>
    /// OICP v1.2 EMP management methods.
    /// </summary>
    public static class EMP_XMLMethods
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

            return SOAP.Encapsulation(new XElement(OICPNS.EVSESearch + "eRoamingSearchEvse",

                                          new XElement(OICPNS.EVSESearch + "GeoCoordinates",
                                              new XElement(OICPNS.CommonTypes + "DecimalDegree",
                                                 new XElement(OICPNS.CommonTypes + "Longitude", GeoCoordinate.Longitude.ToString(CultureInfo.InvariantCulture.NumberFormat)),
                                                 new XElement(OICPNS.CommonTypes + "Latitude",  GeoCoordinate.Latitude. ToString(CultureInfo.InvariantCulture.NumberFormat))
                                              )
                                          ),

                                          new XElement(OICPNS.EVSESearch + "ProviderID", ProviderId),

                                          new XElement(OICPNS.EVSESearch + "Range", DistanceKM)

                                     ));

        }

        #endregion


        #region GetEVSEByIdRequestXML(EVSEId)

        /// <summary>
        /// Create a new Get-EVSE-By-Id request.
        /// </summary>
        /// <param name="EVSEId">An unique EVSE identification.</param>
        public static XElement GetEVSEByIdRequestXML(EVSE_Id  EVSEId)
        {

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v1      = "http://www.hubject.com/b2b/services/evsedata/v1">
            // 
            //    <soapenv:Body>
            //     <tns:eRoamingGetEvseById>
            //       <tns:EvseId>
            //         +46*899*02423*01
            //       </tns:EvseId>
            //     </tns:eRoamingGetEvseById>
            //   </soapenv:Body>
            // 
            // </soapenv:Envelope>

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingGetEvseById",
                                          new XElement(OICPNS.EVSEData + "EvseId", EVSEId.ToFormat(IdFormatType.OLD))
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
        public static XElement PullEVSEDataRequestXML(EVSP_Id        ProviderId,
                                                      DateTime?      LastCall       = null,
                                                      GeoCoordinate  GeoCoordinate  = null,
                                                      UInt64         DistanceKM     = 0)
        {

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingPullEvseData",

                                          new XElement(OICPNS.EVSEData + "ProviderID", ProviderId.ToString()),

                                          (GeoCoordinate != null && DistanceKM > 0)
                                              ? new XElement(OICPNS.EVSEData + "SearchCenter",

                                                    new XElement(OICPNS.CommonTypes + "GeoCoordinates",
                                                        new XElement(OICPNS.CommonTypes + "DecimalDegree",
                                                           new XElement(OICPNS.CommonTypes + "Longitude", GeoCoordinate.Longitude.ToString(CultureInfo.InvariantCulture.NumberFormat)),
                                                           new XElement(OICPNS.CommonTypes + "Latitude",  GeoCoordinate.Latitude. ToString(CultureInfo.InvariantCulture.NumberFormat))
                                                        )
                                                    ),

                                                    new XElement(OICPNS.CommonTypes + "Radius", DistanceKM)

                                                )
                                              : null,

                                          (LastCall.HasValue)
                                              ? new XElement(OICPNS.EVSEData + "LastCall",  LastCall.Value)
                                              : null,

                                          new XElement(OICPNS.EVSEData + "GeoCoordinatesResponseFormat",  "DecimalDegree")

                                     ));

        }

        #endregion

        #region PullEVSEStatusByIdRequestXML(ProviderId, EVSEIds)

        /// <summary>
        /// Create a new Pull EVSE Status-By-Id request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="EVSEIds">Up to 100 EVSE Ids.</param>
        public static XElement PullEVSEStatusByIdRequestXML(EVSP_Id               ProviderId,
                                                            IEnumerable<EVSE_Id>  EVSEIds)
        {

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v1      = "http://www.hubject.com/b2b/services/evsestatus/v1">
            // 
            //    <soapenv:Header/>
            //
            //    <soapenv:Body>
            //       <v1:eRoamingPullEvseStatusById>
            //          <v1:ProviderID>8BD</v1:ProviderID>
            //          <!--1 to 100 repetitions:-->
            //          <v1:EvseId>+45*045*010*096296</v1:EvseId>
            //          <v1:EvseId>+46*899*02423*01</v1:EvseId>
            //       </v1:eRoamingPullEvseStatusById>
            //    </soapenv:Body>
            // 
            // </soapenv:Envelope>

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEStatus + "eRoamingPullEvseStatusById",

                                          new XElement(OICPNS.EVSEStatus + "ProviderID", ProviderId.ToString()),

                                          EVSEIds.Select(EVSEId => new XElement(OICPNS.EVSEStatus + "EvseId", EVSEId.OriginId)).
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

            return SOAP.Encapsulation(new XElement(OICPNS.MobileAuthorization + "eRoamingMobileAuthorizeStart",

                                          new XElement(OICPNS.MobileAuthorization + "EvseID", EVSEId.ToString()),

                                          new XElement(OICPNS.MobileAuthorization + "QRCodeIdentification",
                                              new XElement(OICPNS.CommonTypes + "EVCOID", EVCOId.ToString()),
                                              new XElement(OICPNS.CommonTypes + "PIN",    PIN)
                                          ),

                                          (PartnerProductId != null)
                                              ? new XElement(OICPNS.MobileAuthorization + "PartnerProductID", PartnerProductId)
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

            return SOAP.Encapsulation(new XElement(OICPNS.MobileAuthorization + "eRoamingMobileRemoteStart",
                                          new XElement(OICPNS.EVSESearch + "SessionID", (SessionId != null) ? SessionId : ChargingSession_Id.New)
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

            return SOAP.Encapsulation(new XElement(OICPNS.MobileAuthorization + "eRoamingMobileRemoteStop",
                                          new XElement(OICPNS.EVSESearch + "SessionID", (SessionId != null) ? SessionId : ChargingSession_Id.New)
                                     ));

        }

        #endregion

    }

}
