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
using System.Threading;
using System.Diagnostics;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

using OICPv2_0 = org.GraphDefined.WWCP.OICPv2_0;
using System.Threading.Tasks;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// Check any OICP services.
    /// </summary>
    /// <typeparam name="TResult">The type of data which will be processed on every update run.</typeparam>
    public abstract class AServiceCheck<TResult>
    {

        #region Data

        private             Boolean          Started = false;
        private   readonly  Object           ServiceCheckLock;
        private   readonly  Timer            ServiceCheckTimer;
        protected readonly  Action<TResult>  _OnFirstCheck;
        protected readonly  Action<TResult>  _OnEveryCheck;

        #endregion

        #region Properties

        #region DNSClient

        private readonly DNSClient _DNSClient;

        public DNSClient DNSClient
        {
            get
            {
                return _DNSClient;
            }
        }

        #endregion

        #region CheckEvery

        private readonly TimeSpan _CheckEvery;

        public TimeSpan CheckEvery
        {
            get
            {
                return _CheckEvery;
            }
        }

        #endregion

        #region InitialDelay

        private readonly TimeSpan? _InitialDelay;

        public TimeSpan? InitialDelay
        {
            get
            {
                return _InitialDelay;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public AServiceCheck(TimeSpan         CheckEvery,
                             Action<TResult>  OnFirstCheck,
                             Action<TResult>  OnEveryCheck,
                             TimeSpan?        InitialDelay  = null,
                             DNSClient        DNSClient     = null)

        {

            this._CheckEvery    = CheckEvery;
            this._OnFirstCheck  = OnFirstCheck;
            this._OnEveryCheck  = OnEveryCheck;
            this._InitialDelay  = InitialDelay;
            this._DNSClient     = DNSClient != null ? DNSClient : new DNSClient();

            // Start not now but veeeeery later!
            ServiceCheckLock    = new Object();
            ServiceCheckTimer   = new Timer(RunServiceCheck, null, TimeSpan.FromDays(30), _CheckEvery);

        }

        #endregion


        #region Start()

        /// <summary>
        /// Start the OICP service check.
        /// </summary>
        public AServiceCheck<TResult> Start()
        {

            if (_InitialDelay.HasValue)
                Thread.Sleep(_InitialDelay.Value);

            if (Monitor.TryEnter(ServiceCheckLock))
            {

                try
                {

                    if (!Started)
                    {

                        //OnFirstRun(this, DownloadXMLData(_DNSClient));

                        DebugX.Log("Initital check run finished!");

                        ServiceCheckTimer.Change(TimeSpan.FromSeconds(1), CheckEvery);

                        Started = true;

                    }

                }
                catch (Exception e)
                {
                    DebugX.Log("Starting the 'OICP Service Check' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);
                }

                finally
                {
                    Monitor.Exit(ServiceCheckLock);
                }

            }

            return this;

        }

        #endregion

        protected abstract Task EveryRun(DateTime Timestamp);

        #region (private, Timer) RunServiceCheck(Status)

        private void RunServiceCheck(Object Status)
        {

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            if (Monitor.TryEnter(ServiceCheckLock))
            {

                #region Debug info

                #if DEBUG

                DebugX.LogT("'OICP Service Check' started");

                var StopWatch = new Stopwatch();
                StopWatch.Start();

                #endif

                #endregion

                try
                {

                    var Task = EveryRun(DateTime.Now);
                    Task.Wait();

                    #region Debug info

                    #if DEBUG

                        StopWatch.Stop();

                        DebugX.LogT("'OICP Service Check' finished after " + StopWatch.Elapsed.TotalSeconds + " seconds!");

                    #endif

                    #endregion

                }
                catch (Exception e)
                {
                    DebugX.LogT("'OICP Service Check' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);
                }

                finally
                {
                    Monitor.Exit(ServiceCheckLock);
                }

            }

            else
                DebugX.LogT("'OICP Service Check' skipped!");

        }

        #endregion

    }

}
