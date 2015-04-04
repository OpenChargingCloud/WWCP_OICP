﻿/*
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

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.eMI3.IO.OICP_1_2
{

    /// <summary>
    /// An abstract base class for all OICPv1.2 Upstream Service(s).
    /// </summary>
    public abstract class AOICPUpstreamService
    {

        #region Properties

        #region Hostname

        protected readonly String _Hostname;

        public String Hostname
        {
            get
            {
                return _Hostname;
            }
        }

        #endregion

        #region TCPPort

        protected readonly IPPort _TCPPort;

        public IPPort TCPPort
        {
            get
            {
                return _TCPPort;
            }
        }

        #endregion

        #region HTTPVirtualHost

        protected readonly String _HTTPVirtualHost;

        public String HTTPVirtualHost
        {
            get
            {
                return _HTTPVirtualHost;
            }
        }

        #endregion

        #region AuthorizatorId

        protected readonly Authorizator_Id _AuthorizatorId;

        public Authorizator_Id AuthorizatorId
        {
            get
            {
                return _AuthorizatorId;
            }
        }

        #endregion

        #region UserAgent

        protected readonly String _UserAgent;

        public String UserAgent
        {
            get
            {
                return _UserAgent;
            }
        }

        #endregion

        #region DNSClient

        protected readonly DNSClient _DNSClient;

        public DNSClient DNSClient
        {
            get
            {
                return _DNSClient;
            }
        }

        #endregion

        #endregion

        #region Events

        #region OnException

        /// <summary>
        /// A delegate called whenever an exception occured.
        /// </summary>
        public delegate void OnExceptionDelegate(DateTime Timestamp, Object Sender, Exception Exception);

        /// <summary>
        /// An event fired whenever an exception occured.
        /// </summary>
        public event OnExceptionDelegate OnException;

        #endregion

        #region OnHTTPError

        /// <summary>
        /// A delegate called whenever a HTTP error occured.
        /// </summary>
        public delegate void OnHTTPErrorDelegate(DateTime Timestamp, Object Sender, HTTPResponse HttpResponse);

        /// <summary>
        /// An event fired whenever a HTTP error occured.
        /// </summary>
        public event OnHTTPErrorDelegate OnHTTPError;

        #endregion

        #endregion

        #region Constructor(s)

        public AOICPUpstreamService(String           Hostname,
                                    IPPort           TCPPort,
                                    String           HTTPVirtualHost  = null,
                                    Authorizator_Id  AuthorizatorId   = null,
                                    String           UserAgent        = "GraphDefined OICP Gateway",
                                    DNSClient        DNSClient        = null)
        {

            this._Hostname         = Hostname;
            this._TCPPort          = TCPPort;

            this._HTTPVirtualHost  = (HTTPVirtualHost != null)
                                         ? HTTPVirtualHost
                                         : Hostname;

            this._AuthorizatorId   = (AuthorizatorId == null)
                                         ? Authorizator_Id.Parse("OICP Gateway")
                                         : AuthorizatorId;

            this._UserAgent        = UserAgent;

            this._DNSClient        = (DNSClient == null)
                                         ? new DNSClient()
                                         : DNSClient;

        }

        #endregion



        protected void SendOnHTTPError(DateTime      Timestamp,
                                       Object        Sender,
                                       HTTPResponse  HttpResponse)
        {

            var OnHTTPErrorLocal = OnHTTPError;
            if (OnHTTPErrorLocal != null)
                OnHTTPErrorLocal(Timestamp, Sender, HttpResponse);

        }

        protected void SendOnException(DateTime   Timestamp,
                                       Object     Sender,
                                       Exception  Exception)
        {

            var OnExceptionLocal = OnException;
            if (OnExceptionLocal != null)
                OnExceptionLocal(Timestamp, Sender, Exception);

        }


    }

}
