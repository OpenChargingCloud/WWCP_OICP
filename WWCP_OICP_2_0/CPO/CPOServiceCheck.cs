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
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// Check any OICP v2.0 CPO service.
    /// </summary>
    /// <typeparam name="T">The type of data which will be processed on every update run.</typeparam>
    public class CPOServiceCheck<T> : AServiceCheck<T>
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public const String DefaultHTTPUserAgent = "GraphDefined OICP v2.0 CPO Service Check";

        /// <summary>
        /// The default HTTP/SOAP/XML server name.
        /// </summary>
        public const String DefaultHTTPServerName = "OICP v2.0 HTTP/SOAP/XML CPO Service Check Server API";

        private readonly Action<T> _OnFirstCheck;
        private readonly Action<T> _OnEveryCheck;
        private readonly Func<DateTime, CPOServiceCheck<T>, CPORoaming, Task<T>> _Checker;

        #endregion

        #region Properties

        #region CPORoaming

        private readonly CPORoaming _CPORoaming;

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

        public CPOServiceCheck(String                                 Hostname,
                               IPPort                                 IPPort                 = null,
                               String                                 RemoteHTTPVirtualHost  = null,
                               String                                 HTTPUserAgent          = DefaultHTTPUserAgent,
                               TimeSpan?                              QueryTimeout           = null,

                               String                                 ServerName             = CPOServer.DefaultHTTPServerName,
                               IPPort                                 ServerTCPPort          = null,
                               String                                 ServerURIPrefix        = "",
                               Boolean                                ServerAutoStart        = false,

                               Func<DateTime, CPOServiceCheck<T>, CPORoaming, Task<T>>  Checker                = null,
                               TimeSpan?                              CheckEvery             = null,
                               Action<T>                              OnFirstCheck           = null,
                               Action<T>                              OnEveryCheck           = null,

                               DNSClient                              DNSClient              = null)

            : this(new CPORoaming(Hostname,
                                               IPPort,
                                               RemoteHTTPVirtualHost,
                                               HTTPUserAgent,
                                               QueryTimeout,

                                               ServerName,
                                               ServerTCPPort,
                                               ServerURIPrefix,
                                               ServerAutoStart,

                                               DNSClient: DNSClient),

                   Checker, CheckEvery, OnFirstCheck, OnEveryCheck)

        { }

        public CPOServiceCheck(CPORoaming                                   CPORoaming,

                               Func<DateTime, CPOServiceCheck<T>, CPORoaming, Task<T>>  Checker                = null,
                               TimeSpan?                                    CheckEvery             = null,
                               Action<T>                                    OnFirstCheck           = null,
                               Action<T>                                    OnEveryCheck           = null)

            : base(CPORoaming.DNSClient, CheckEvery, Authorizator_Id.Parse("CPO Service Check"))

        {

            this._CPORoaming    = CPORoaming;

            this._Checker       = Checker;
            this._OnFirstCheck  = OnFirstCheck;
            this._OnEveryCheck  = OnEveryCheck;

        }

        #endregion

        protected override async Task EveryRun(DateTime Timestamp)
        {

           // var OnEveryRunLocal = OnEveryRun;
           // if (OnEveryRunLocal != null)
           //     OnEveryRunLocal(Timestamp, this, _Checker(DateTime.Now, this));

            var ch = await _Checker(DateTime.Now, this, this.CPORoaming);

            _OnEveryCheck(ch);


            //var results = await Task.WhenAll(OnEveryRun.
            //                                     GetInvocationList().
            //                                     Select(subscriber => (subscriber as OnEveryRunDelegate)
            //                                         (Timestamp,
            //                                          this,
            //                                          ch)));



        }

    }

}
