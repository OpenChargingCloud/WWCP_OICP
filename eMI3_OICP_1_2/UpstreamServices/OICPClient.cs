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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace org.GraphDefined.eMI3.IO.OICP_1_2
{

    /// <summary>
    /// A specialized HTTP client for the Open InterCharge Protocol.
    /// </summary>
    public class OICPClient : HTTPClient
    {

        #region Properties

        #region HTTPVirtualHost

        private readonly String _HTTPVirtualHost;

        /// <summary>
        /// The HTTP virtual host to use.
        /// </summary>
        public String HTTPVirtualHost
        {
            get
            {
                return _HTTPVirtualHost;
            }
        }

        #endregion

        #region URIPrefix

        private readonly String _URIPrefix;

        /// <summary>
        /// The URI-prefix of the OICP service.
        /// </summary>
        public String URIPrefix
        {
            get
            {
                return _URIPrefix;
            }
        }

        #endregion

        #region UserAgent

        private readonly String _UserAgent;

        /// <summary>
        /// The HTTP user agent.
        /// </summary>
        public String UserAgent
        {
            get
            {
                return _UserAgent;
            }
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new specialized HTTP client for the Open InterCharge Protocol.
        /// </summary>
        /// <param name="OICPHost">The hostname of the remote OICP service.</param>
        /// <param name="OICPPort">The TCP port of the remote OICP service.</param>
        /// <param name="HTTPVirtualHost">The HTTP virtual host to use.</param>
        /// <param name="URIPrefix">The URI-prefix of the OICP service.</param>
        /// <param name="UserAgent">The HTTP user agent to use.</param>
        public OICPClient(String     OICPHost,
                          IPPort     OICPPort,
                          String     HTTPVirtualHost,
                          String     URIPrefix,
                          String     UserAgent  = "GraphDefined Hubject Gateway",
                          DNSClient  DNSClient  = null)

            : base(OICPHost, OICPPort, DNSClient)

        {

            this._HTTPVirtualHost  = HTTPVirtualHost;
            this._URIPrefix        = URIPrefix;
            this._UserAgent        = UserAgent;

        }

        #endregion


        #region Query(Query, SOAPAction)

        public HTTPResponse Query(String Query, String SOAPAction)
        {

            var builder = this.POST(_URIPrefix);
            builder.Host         = HTTPVirtualHost;
            builder.Content      = Query.ToUTF8Bytes();
            builder.ContentType  = HTTPContentType.XMLTEXT_UTF8;
            builder.Set("SOAPAction", SOAPAction);
            builder.UserAgent    = UserAgent;

            return this.Execute_Synced(builder);

        }

        #endregion

        #region Query(Query, SOAPAction, OnSuccess, OnFault, TimeoutMSec = 20000)

        public Task<T> Query<T>(String             Query,
                                String             SOAPAction,
                                Func<XElement, T>  OnSuccess,
                                Func<XElement, T>  OnFault,
                                UInt32             TimeoutMSec = 20000)

        {

            var builder = this.POST(_URIPrefix);
            builder.Host               = HTTPVirtualHost;
            builder.Content            = Query.ToUTF8Bytes();
            builder.ContentType        = HTTPContentType.XMLTEXT_UTF8;
            builder.Set("SOAPAction",  SOAPAction);
            builder.UserAgent          = UserAgent;

            return this.ExecuteReturnResult(builder, TimeoutMSec: TimeoutMSec).
                        ContinueWith(HttpResponseTask => {

                            if (HttpResponseTask.Result == null)
                                return default(T);

                            var XML = XDocument.Parse(HttpResponseTask.Result.Content.ToUTF8String()).
                                      Root.
                                      Element(OICP_1_2.NS.SOAPEnvelope + "Body").
                                      Descendants().
                                      FirstOrDefault();

                            // <S:Fault xmlns:ns4="http://www.w3.org/2003/05/soap-envelope" xmlns:S="http://schemas.xmlsoap.org/soap/envelope/">
                            //   <faultcode>S:Client</faultcode>
                            //   <faultstring>Validation error: The request message is invalid</faultstring>
                            //   <detail>
                            //     <Validation>
                            //       <Errors>
                            //         <Error column="65" errorXpath="/eMI3:Envelope/eMI3:Body/EVSEStatus:eRoamingPullEvseStatusById/EVSEStatus:EvseId" line="3">Value '+45*045*010*0A96296' is not facet-valid with respect to pattern '([A-Za-z]{2}\*?[A-Za-z0-9]{3}\*?E[A-Za-z0-9\*]{1,30})|(\+?[0-9]{1,3}\*[0-9]{3,6}\*[0-9\*]{1,32})' for type 'EvseIDType'.</Error>
                            //         <Error column="65" errorXpath="/eMI3:Envelope/eMI3:Body/EVSEStatus:eRoamingPullEvseStatusById/EVSEStatus:EvseId" line="3">The value '+45*045*010*0A96296' of element 'EVSEStatus:EvseId' is not valid.</Error>
                            //       </Errors>
                            //       <OriginalDocument>
                            //         <eMI3:Envelope xmlns:eMI3="http://schemas.xmlsoap.org/soap/envelope/" xmlns:Authorization="http://www.hubject.com/b2b/services/authorization/v1.2" xmlns:CommonTypes="http://www.hubject.com/b2b/services/commontypes/v1.2" xmlns:EVSEData="http://www.hubject.com/b2b/services/evsedata/v1.2" xmlns:EVSESearch="http://www.hubject.com/b2b/services/evsesearch/v1.2" xmlns:EVSEStatus="http://www.hubject.com/b2b/services/evsestatus/v1.2" xmlns:MobileAuthorization="http://www.hubject.com/b2b/services/mobileauthorization/v1.2">
                            //           <eMI3:Header />
                            //           <eMI3:Body>
                            //             <EVSEStatus:eRoamingPullEvseStatusById>
                            //               <EVSEStatus:ProviderID>DE-8BD</EVSEStatus:ProviderID>
                            //               <EVSEStatus:EvseId>+45*045*010*0A96296</EVSEStatus:EvseId>
                            //               <EVSEStatus:EvseId>+46*899*02423*01</EVSEStatus:EvseId>
                            //             </EVSEStatus:eRoamingPullEvseStatusById>
                            //           </eMI3:Body>
                            //         </eMI3:Envelope>
                            //       </OriginalDocument>
                            //     </Validation>
                            //   </detail>
                            // </S:Fault>

                            if (XML.Name.LocalName != "Fault")
                            {

                                var OnSuccessLocal = OnSuccess;
                                if (OnSuccessLocal != null)
                                    return OnSuccessLocal(XML);

                            }

                            var OnFaultLocal = OnFault;
                            if (OnFaultLocal != null)
                                return OnFaultLocal(XML);

                            return default(T);

                        });

        }

        #endregion

    }

}
