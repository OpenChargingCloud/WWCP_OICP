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

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// A simple OICP v2.0 EMP client.
    /// </summary>
    public class EMPClient
    {

        #region Data

        private readonly EMPUpstreamService _EMPUpstreamService;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP v2.0 EMP client.
        /// </summary>
        /// <param name="Hostname">The OICP hostname to connect to.</param>
        /// <param name="TCPPort">An optional OICP TCP port to connect to.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual host name to use.</param>
        /// <param name="UserAgent">An optional HTTP user agent to use.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        public EMPClient(String     Hostname,
                         IPPort     TCPPort          = null,
                         String     HTTPVirtualHost  = null,
                         String     UserAgent        = "GraphDefined OICP v2.0 EMP Client",
                         TimeSpan?  QueryTimeout     = null,
                         DNSClient  DNSClient        = null)

        {

            this._EMPUpstreamService = new EMPUpstreamService(Hostname,
                                                              TCPPort != null ? TCPPort : IPPort.Parse(443),
                                                              HTTPVirtualHost,
                                                              Authorizator_Id.Parse("GraphDefined OICP v2.0 EMP Client"),
                                                              UserAgent,
                                                              QueryTimeout,
                                                              DNSClient);

        }

        #endregion


        #region GetEVSEByIdRequest(EVSEId, QueryTimeout = null) // <- Note!

        // Note: It's confusing, but this request does not belong here!
        //       It must be omplemented on the CPO client side!

        ///// <summary>
        ///// Create a new task requesting the static EVSE data
        ///// for the given EVSE identification.
        ///// </summary>
        ///// <param name="EVSEId">The unique identification of the EVSE.</param>
        ///// <param name="QueryTimeout">An optional timeout for this query.</param>
        //public Task<HTTPResponse<EVSEDataRecord>>

        //    GetEVSEByIdRequest(EVSE_Id    EVSEId,
        //                       TimeSpan?  QueryTimeout  = null)

        //{

        //    try
        //    {

        //        using (var _OICPClient = new SOAPClient(Hostname,
        //                                                TCPPort,
        //                                                HTTPVirtualHost,
        //                                                "/ibis/ws/eRoamingEvseData_V2.0",
        //                                                UserAgent,
        //                                                DNSClient))
        //        {

        //            return _OICPClient.Query(EMP_XMLMethods.GetEVSEByIdRequestXML(EVSEId),
        //                                     "eRoamingEvseById",
        //                                     QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

        //                                     OnSuccess: XMLData =>

        //                                         #region Documentation

        //                                         // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
        //                                         //                   xmlns:EVSEData    = "http://www.hubject.com/b2b/services/evsedata/v2.0"
        //                                         //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //                                         //   <soapenv:Header/>
        //                                         //   <soapenv:Body>
        //                                         //      <EVSEData:eRoamingEvseDataRecord deltaType="?" lastUpdate="?">
        //                                         //          [...]
        //                                         //      </EVSEData:eRoamingEvseDataRecord>
        //                                         //    </soapenv:Body>
        //                                         // </soapenv:Envelope>

        //                                         #endregion

        //                                         new HTTPResponse<EVSEDataRecord>(XMLData.HttpResponse,
        //                                                                          XMLMethods.ParseEVSEDataRecordXML(XMLData.Content)),

        //                                     OnSOAPFault: Fault =>
        //                                         new HTTPResponse<EVSEDataRecord>(
        //                                             Fault.HttpResponse,
        //                                             new Exception(Fault.Content.ToString())),

        //                                     OnHTTPError: (t, s, e) => SendOnHTTPError(t, s, e),

        //                                     OnException: (t, s, e) => SendOnException(t, s, e)

        //                                    );

        //        }

        //    }

        //    catch (Exception e)
        //    {

        //        SendOnException(DateTime.Now, this, e);

        //        return new Task<HTTPResponse<EVSEDataRecord>>(
        //            () => new HTTPResponse<EVSEDataRecord>(e));

        //    }

        //}

        #endregion

        #region PullEVSEData(ProviderId, SearchCenter = null, DistanceKM = 0, LastCall = null, QueryTimeout = null, ExceptionHandler = null)

        /// <summary>
        /// Create a new task querying EVSE data from the OICP server.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="LastCall">An optional timestamp of the last call.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        /// <param name="ExceptionHandler">An optional delegate called whenever an exception occured.</param>
        public async Task<HTTPResponse<IEnumerable<eRoamingEVSEData>>>

            PullEVSEData(EVSP_Id           ProviderId,
                         GeoCoordinate     SearchCenter      = null,
                         UInt64            DistanceKM        = 0,
                         DateTime?         LastCall          = null,
                         TimeSpan?         QueryTimeout      = null,
                         Action<Exception> ExceptionHandler  = null)

        {

            return await _EMPUpstreamService.PullEVSEData(ProviderId,
                                                          SearchCenter,
                                                          DistanceKM,
                                                          LastCall,
                                                          QueryTimeout,
                                                          ExceptionHandler);

        }

        #endregion


        #region PullEVSEStatusById(ProviderId, EVSEIds, QueryTimeout = null)

        /// <summary>
        /// Create a new task requesting the current status of up to 100 EVSEs by their EVSE Ids.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="EVSEIds">Up to 100 EVSE Ids.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, OICPEVSEStatus>>>>

            PullEVSEStatusById(EVSP_Id               ProviderId,
                               IEnumerable<EVSE_Id>  EVSEIds,
                               TimeSpan?             QueryTimeout = null)

        {

            return await _EMPUpstreamService.PullEVSEStatusById(ProviderId,
                                                                EVSEIds,
                                                                QueryTimeout);

        }

        #endregion

        #region PullEVSEStatus(ProviderId, SearchCenter = null, DistanceKM = 0, EVSEStatus = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task requesting the current status of all EVSEs (within an optional search radius and status).
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="EVSEStatus">An optional EVSE status as filter criteria.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, OICPEVSEStatus>>>>

            PullEVSEStatus(EVSP_Id          ProviderId,
                           GeoCoordinate    SearchCenter  = null,
                           UInt64           DistanceKM    = 0,
                           OICPEVSEStatus?  EVSEStatus    = null,
                           TimeSpan?        QueryTimeout  = null)

        {

            return await _EMPUpstreamService.PullEVSEStatus(ProviderId,
                                                            SearchCenter,
                                                            DistanceKM,
                                                            EVSEStatus,
                                                            QueryTimeout);

        }

        #endregion


    }

}
