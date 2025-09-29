/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// An abstract OICP client.
    /// </summary>
    public abstract class AOICPClient : AHTTPClient
    {

        #region Data

        /// <summary>
        /// The HTTP client.
        /// </summary>
        protected HTTPTestClient  newHTTPClient;

        /// <summary>
        /// A HTTP client pool for low-latency HTTP requests.
        /// </summary>
        protected HTTPClientPool  httpClientPool;

        #endregion

        #region Properties

        public Newtonsoft.Json.Formatting  JSONFormatting    { get; set; }

        #endregion

        #region Constructor(s)

        public AOICPClient(URL                                                        RemoteURL,
                           HTTPHostname?                                              VirtualHostname              = null,
                           I18NString?                                                Description                  = null,
                           UInt16?                                                    MaxNumberOfPooledClients     = null,
                           Boolean?                                                   PreferIPv4                   = null,
                           RemoteTLSServerCertificateValidationHandler<IHTTPClient>?  RemoteCertificateValidator   = null,
                           LocalCertificateSelectionHandler?                          LocalCertificateSelector     = null,
                           X509Certificate?                                           ClientCert                   = null,
                           SslProtocols?                                              TLSProtocol                  = null,
                           HTTPContentType?                                           ContentType                  = null,
                           AcceptTypes?                                               Accept                       = null,
                           IHTTPAuthentication?                                       HTTPAuthentication           = null,
                           String?                                                    HTTPUserAgent                = null,
                           ConnectionType?                                            Connection                   = null,
                           TimeSpan?                                                  RequestTimeout               = null,
                           TransmissionRetryDelayDelegate?                            TransmissionRetryDelay       = null,
                           UInt16?                                                    MaxNumberOfRetries           = null,
                           UInt32?                                                    InternalBufferSize           = null,
                           Boolean?                                                   UseHTTPPipelining            = null,
                           Boolean?                                                   DisableLogging               = null,
                           IDNSClient?                                                DNSClient                    = null)

            : base(RemoteURL,
                   VirtualHostname,
                   Description,
                   PreferIPv4,
                   RemoteCertificateValidator,
                   LocalCertificateSelector,
                   ClientCert,
                   TLSProtocol,
                   ContentType,
                   Accept,
                   HTTPAuthentication,
                   HTTPUserAgent,
                   Connection,
                   RequestTimeout,
                   TransmissionRetryDelay,
                   MaxNumberOfRetries,
                   InternalBufferSize,
                   UseHTTPPipelining,
                   DisableLogging,
                   null,
                   DNSClient)

        {

            this.JSONFormatting  = Newtonsoft.Json.Formatting.None;

            this.newHTTPClient   = new HTTPTestClient(

                                       URL:                              base.RemoteURL,
                                       Description:                      Description,

                                       HTTPUserAgent:                    HTTPUserAgent,
                                       DefaultRequestBuilder:            () => new HTTPRequest.Builder(this, CancellationToken.None) {
                                                                                   Host         = this.RemoteURL.Hostname,
                                                                                   Accept       = AcceptTypes.FromHTTPContentTypes(HTTPContentType.Application.JSON_UTF8),
                                                                                   ContentType  = HTTPContentType.Application.JSON_UTF8,
                                                                                   UserAgent    = this.HTTPUserAgent ?? DefaultHTTPUserAgent,
                                                                                   Connection   = ConnectionType.KeepAlive
                                                                               },

                                       RemoteCertificateValidator:       RemoteCertificateValidator is not null
                                                                             ? (sender,
                                                                                certificate,
                                                                                certificateChain,
                                                                                httpTestClient,
                                                                                policyErrors) => RemoteCertificateValidator.Invoke(
                                                                                                     sender,
                                                                                                     certificate,
                                                                                                     certificateChain,
                                                                                                     this,
                                                                                                     policyErrors
                                                                                                 )
                                                                             :  null,
                                       LocalCertificateSelector:         LocalCertificateSelector,
                                       ClientCertificateChain:           ClientCert is not null
                                                                             ? [ ClientCert ]
                                                                             : null,
                                       TLSProtocols:                     TLSProtocols,
                                       CertificateRevocationCheckMode:   X509RevocationMode.NoCheck,
                                       ApplicationProtocols:             null,
                                       AllowRenegotiation:               null,
                                       AllowTLSResume:                   null,

                                       PreferIPv4:                       PreferIPv4,
                                       ConnectTimeout:                   null,
                                       ReceiveTimeout:                   null,
                                       SendTimeout:                      null,
                                       BufferSize:                       null,

                                       DNSClient:                        DNSClient

                                   );

            this.httpClientPool  = new HTTPClientPool(

                                       URL:                              base.RemoteURL,
                                       Description:                      Description,

                                       HTTPUserAgent:                    HTTPUserAgent,
                                       DefaultRequestBuilder:            () => new HTTPRequest.Builder(this, CancellationToken.None) {
                                                                                   Host         = this.RemoteURL.Hostname,
                                                                                   Accept       = AcceptTypes.FromHTTPContentTypes(HTTPContentType.Application.JSON_UTF8),
                                                                                   ContentType  = HTTPContentType.Application.JSON_UTF8,
                                                                                   UserAgent    = this.HTTPUserAgent ?? DefaultHTTPUserAgent,
                                                                                   Connection   = ConnectionType.KeepAlive
                                                                               },

                                       RemoteCertificateValidator:       RemoteCertificateValidator is not null
                                                                             ? (sender,
                                                                                certificate,
                                                                                certificateChain,
                                                                                httpTestClient,
                                                                                policyErrors) => RemoteCertificateValidator.Invoke(
                                                                                                     sender,
                                                                                                     certificate,
                                                                                                     certificateChain,
                                                                                                     this,
                                                                                                     policyErrors
                                                                                                 )
                                                                             :  null,
                                       LocalCertificateSelector:         LocalCertificateSelector,
                                       ClientCertificateChain:           ClientCert is not null
                                                                             ? [ ClientCert ]
                                                                             : null,
                                       TLSProtocols:                     TLSProtocols,
                                       CertificateRevocationCheckMode:   X509RevocationMode.NoCheck,
                                       ApplicationProtocols:             null,
                                       AllowRenegotiation:               null,
                                       AllowTLSResume:                   null,

                                       MaxNumberOfClients:               MaxNumberOfPooledClients ?? 6,

                                       PreferIPv4:                       PreferIPv4,
                                       ConnectTimeout:                   null,
                                       ReceiveTimeout:                   null,
                                       SendTimeout:                      null,
                                       BufferSize:                       null,

                                       DNSClient:                        DNSClient

                                   );

        }

        #endregion


        #region (protected) LogEvent     (OICPIO, Logger, LogHandler, ...)

        protected async Task LogEvent<TDelegate>(String                                             OICPIO,
                                                 TDelegate?                                         Logger,
                                                 Func<TDelegate, Task>                              LogHandler,
                                                 [CallerArgumentExpression(nameof(Logger))] String  EventName     = "",
                                                 [CallerMemberName()]                       String  OICPCommand   = "")

            where TDelegate : Delegate

        {
            if (Logger is not null)
            {
                try
                {

                    await Task.WhenAll(
                              Logger.GetInvocationList().
                                     OfType<TDelegate>().
                                     Select(LogHandler)
                          );

                }
                catch (Exception e)
                {
                    await HandleErrors(OICPIO, $"{OICPCommand}.{EventName}", e);
                }
            }
        }

        #endregion

        #region (virtual)   HandleErrors (Module, Caller, ErrorResponse)

        public virtual Task HandleErrors(String  Module,
                                         String  Caller,
                                         String  ErrorResponse)
        {

            DebugX.Log($"{Module}.{Caller}: {ErrorResponse}");

            return Task.CompletedTask;

        }

        #endregion

        #region (virtual)   HandleErrors (Module, Caller, ExceptionOccurred)

        public virtual Task HandleErrors(String     Module,
                                         String     Caller,
                                         Exception  ExceptionOccurred)
        {

            DebugX.LogException(ExceptionOccurred, $"{Module}.{Caller}");

            return Task.CompletedTask;

        }

        #endregion


        #region Dispose()

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }

        #endregion


    }

}
