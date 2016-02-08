/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
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

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using System.Net.Security;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// Check any OICP v2.0 CPO service.
    /// </summary>
    /// <typeparam name="TResult">The type of data which will be processed on every update run.</typeparam>
    public class CPOServiceCheck<TResult> : AServiceCheck<TResult>
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public  const    String                            DefaultHTTPUserAgent   = "GraphDefined OICP v2.0 CPO Service Check";

        /// <summary>
        /// The default HTTP/SOAP/XML server name.
        /// </summary>
        public  const    String                            DefaultHTTPServerName  = "OICP v2.0 HTTP/SOAP/XML CPO Service Check Server API";


        private readonly CPOServiceCheckDelegate<TResult>  _ServiceChecker;

        #endregion

        #region Properties

        #region CPORoaming

        private readonly CPORoaming _CPORoaming;

        /// <summary>
        /// The CPO roaming provider for this service check.
        /// </summary>
        public CPORoaming CPORoaming
        {
            get
            {
                return _CPORoaming;
            }
        }

        #endregion

        #endregion

        #region Events

        ///// <summary>
        ///// A delegate called whenever a new service check was initiated.
        ///// </summary>
        //public delegate Task OnEveryRunDelegate(DateTime Timestamp, CPOServiceCheck<T> Sender, T Value);

        ///// <summary>
        ///// An event fired whenever a new service check was initiated.
        ///// </summary>
        //public event OnEveryRunDelegate OnEveryRun;

        #endregion

        #region Constructor(s)

        #region CPOServiceCheck(ClientId, ServiceChecker, OnFirstCheck, OnEveryCheck, CheckEvery, RemoteHostname, RemoteTCPPort, ...)

        public CPOServiceCheck(String                               ClientId,
                               CPOServiceCheckDelegate<TResult>     ServiceChecker,
                               Action<TResult>                      OnFirstCheck,
                               Action<TResult>                      OnEveryCheck,
                               TimeSpan                             CheckEvery,

                               String                               RemoteHostname,
                               IPPort                               RemoteTCPPort               = null,
                               String                               RemoteHTTPVirtualHost       = null,
                               RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                               String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                               TimeSpan?                            QueryTimeout                = null,

                               String                               ServerName                  = CPOServer.DefaultHTTPServerName,
                               IPPort                               ServerTCPPort               = null,
                               String                               ServerURIPrefix             = "",
                               Boolean                              ServerAutoStart             = false,

                               TimeSpan?                            InitialDelay                = null,
                               DNSClient                            DNSClient                   = null)

            : this(new CPORoaming(ClientId,
                                  RemoteHostname,
                                  RemoteTCPPort,
                                  RemoteHTTPVirtualHost,
                                  RemoteCertificateValidator,
                                  HTTPUserAgent,
                                  QueryTimeout,

                                  ServerName,
                                  ServerTCPPort,
                                  ServerURIPrefix,
                                  ServerAutoStart,

                                  DNSClient: DNSClient),

                   ServiceChecker,
                   OnFirstCheck,
                   OnEveryCheck,
                   CheckEvery,
                   InitialDelay)

        { }

        #endregion

        #region CPOServiceCheck(CPORoaming, ServiceChecker, OnFirstCheck, OnEveryCheck, CheckEvery = null)

        public CPOServiceCheck(CPORoaming                        CPORoaming,
                               CPOServiceCheckDelegate<TResult>  ServiceChecker,
                               Action<TResult>                   OnFirstCheck,
                               Action<TResult>                   OnEveryCheck,
                               TimeSpan                          CheckEvery,
                               TimeSpan?                         InitialDelay = null)

            : base(CheckEvery, OnFirstCheck, OnEveryCheck, InitialDelay, CPORoaming.DNSClient)

        {

            this._CPORoaming      = CPORoaming;
            this._ServiceChecker  = ServiceChecker;

        }

        #endregion

        #endregion

        protected override async Task EveryRun(DateTime Timestamp)
        {

           // var OnEveryRunLocal = OnEveryRun;
           // if (OnEveryRunLocal != null)
           //     OnEveryRunLocal(Timestamp, this, _Checker(DateTime.Now, this));

            _OnEveryCheck(await _ServiceChecker(DateTime.Now, this, this.CPORoaming));


            //var results = await Task.WhenAll(OnEveryRun.
            //                                     GetInvocationList().
            //                                     Select(subscriber => (subscriber as OnEveryRunDelegate)
            //                                         (Timestamp,
            //                                          this,
            //                                          ch)));



        }

    }

}
