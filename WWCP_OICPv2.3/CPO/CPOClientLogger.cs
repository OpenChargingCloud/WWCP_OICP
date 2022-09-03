/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

using System.Text;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using static cloud.charging.open.protocols.OICPv2_3.CPO.ClientLogger;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    public static class EXTLogger
    {

        #region RegisterDefaultConsoleLogTarget(this APIClientRequestLogger,  Logger)

        /// <summary>
        /// Register the default console logger.
        /// </summary>
        /// <param name="APIClientRequestLogger">A request logger.</param>
        /// <param name="Logger">A logger.</param>
        public static APIClientRequestLogger  RegisterDefaultConsoleLogTarget(this APIClientRequestLogger  APIClientRequestLogger,
                                                                              ALogger                      Logger)

            => APIClientRequestLogger.RegisterLogTarget(LogTargets.Console,
                                                        Logger.Default_LogRequest_toConsole);

        #endregion

        #region RegisterDefaultConsoleLogTarget(this APIClientResponseLogger, Logger)

        /// <summary>
        /// Register the default console logger.
        /// </summary>
        /// <param name="APIClientResponseLogger">A response logger.</param>
        /// <param name="Logger">A logger.</param>
        public static APIClientResponseLogger RegisterDefaultConsoleLogTarget(this APIClientResponseLogger  APIClientResponseLogger,
                                                                              ALogger                       Logger)

            => APIClientResponseLogger.RegisterLogTarget(LogTargets.Console,
                                                         Logger.Default_LogResponse_toConsole);

        #endregion

    }


    /// <summary>
    /// The delegate for logging the request send by a client.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the outgoing request.</param>
    /// <param name="Client">The client sending the request.</param>
    /// <param name="Request">The outgoing HTTP request.</param>
    public delegate Task APIClientRequestLogHandler(DateTime  Timestamp,
                                                    Object    Client,
                                                    String?   Request);


    /// <summary>
    /// The delegate for logging the response received by a client.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the incoming response.</param>
    /// <param name="Client">The client receiving the request.</param>
    /// <param name="Request">The outgoing request.</param>
    /// <param name="Response">The incoming response.</param>
    public delegate Task APIClientResponseLogHandler(DateTime  Timestamp,
                                                     Object    Client,
                                                     String?   Request,
                                                     String?   Response,
                                                     TimeSpan  Runtime);


    public delegate Task   RequestLoggerDelegate (String LoggingPath, String Context, String LogEventName, String? Request);
    public delegate Task   ResponseLoggerDelegate(String LoggingPath, String Context, String LogEventName, String? Request, String? Response, TimeSpan Runtime);


    /// <summary>
    /// An API logger.
    /// </summary>
    public abstract class ALogger
    {

        #region Data

        private static readonly Object         LockObject               = new ();
        private static          SemaphoreSlim  LogRequest_toDisc_Lock   = new (1,1);
        private static          SemaphoreSlim  LogResponse_toDisc_Lock  = new (1,1);

        /// <summary>
        /// The maximum number of retries to write to a logfile.
        /// </summary>
        public  static readonly Byte           MaxRetries                   = 5;

        /// <summary>
        /// Maximum waiting time to enter a lock around a logfile.
        /// </summary>
        public  static readonly TimeSpan       MaxWaitingForALock           = TimeSpan.FromSeconds(15);

        /// <summary>
        /// A delegate for the default ToDisc logger returning a
        /// valid logfile name based on the given log event name.
        /// </summary>
        public         LogfileCreatorDelegate  LogfileCreator               { get; }

        protected readonly ConcurrentDictionary<String, HashSet<String>> _GroupTags;

        #endregion

        #region Properties

        /// <summary>
        /// The logging path.
        /// </summary>
        public String  LoggingPath    { get; }

        /// <summary>
        /// The context of this HTTP logger.
        /// </summary>
        public String  Context        { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new HTTP API logger using the given logging delegates.
        /// </summary>
        /// <param name="LoggingPath">The logging path.</param>
        /// <param name="Context">A context of this API.</param>
        /// 
        /// <param name="LogHTTPRequest_toConsole">A delegate to log incoming HTTP requests to console.</param>
        /// <param name="LogHTTPResponse_toConsole">A delegate to log HTTP requests/responses to console.</param>
        /// <param name="LogHTTPRequest_toDisc">A delegate to log incoming HTTP requests to disc.</param>
        /// <param name="LogHTTPResponse_toDisc">A delegate to log HTTP requests/responses to disc.</param>
        /// 
        /// <param name="LogHTTPRequest_toNetwork">A delegate to log incoming HTTP requests to a network target.</param>
        /// <param name="LogHTTPResponse_toNetwork">A delegate to log HTTP requests/responses to a network target.</param>
        /// <param name="LogHTTPRequest_toHTTPSSE">A delegate to log incoming HTTP requests to a HTTP server sent events source.</param>
        /// <param name="LogHTTPResponse_toHTTPSSE">A delegate to log HTTP requests/responses to a HTTP server sent events source.</param>
        /// 
        /// <param name="LogHTTPError_toConsole">A delegate to log HTTP errors to console.</param>
        /// <param name="LogHTTPError_toDisc">A delegate to log HTTP errors to disc.</param>
        /// <param name="LogHTTPError_toNetwork">A delegate to log HTTP errors to a network target.</param>
        /// <param name="LogHTTPError_toHTTPSSE">A delegate to log HTTP errors to a HTTP server sent events source.</param>
        /// 
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public ALogger(String                   LoggingPath,
                       String                   Context,

                       RequestLoggerDelegate?   LogHTTPRequest_toConsole    = null,
                       ResponseLoggerDelegate?  LogHTTPResponse_toConsole   = null,
                       RequestLoggerDelegate?   LogHTTPRequest_toDisc       = null,
                       ResponseLoggerDelegate?  LogHTTPResponse_toDisc      = null,

                       RequestLoggerDelegate?   LogHTTPRequest_toNetwork    = null,
                       ResponseLoggerDelegate?  LogHTTPResponse_toNetwork   = null,
                       RequestLoggerDelegate?   LogHTTPRequest_toHTTPSSE    = null,
                       ResponseLoggerDelegate?  LogHTTPResponse_toHTTPSSE   = null,

                       ResponseLoggerDelegate?  LogHTTPError_toConsole      = null,
                       ResponseLoggerDelegate?  LogHTTPError_toDisc         = null,
                       ResponseLoggerDelegate?  LogHTTPError_toNetwork      = null,
                       ResponseLoggerDelegate?  LogHTTPError_toHTTPSSE      = null,

                       LogfileCreatorDelegate?  LogfileCreator              = null)

        {

            #region Init data structures

            this.LoggingPath  = LoggingPath ?? "";
            this.Context      = Context     ?? "";
            this._GroupTags   = new ConcurrentDictionary<String, HashSet<String>>();

            #endregion

            #region Set default delegates

            if (LogHTTPRequest_toConsole  is null)
                LogHTTPRequest_toConsole   = Default_LogRequest_toConsole;

            if (LogHTTPRequest_toDisc     is null)
                LogHTTPRequest_toDisc      = Default_LogRequest_toDisc;

            if (LogHTTPResponse_toConsole is null)
                LogHTTPResponse_toConsole  = Default_LogResponse_toConsole;

            if (LogHTTPResponse_toDisc    is null)
                LogHTTPResponse_toDisc     = Default_LogResponse_toDisc;


            if (LogHTTPRequest_toDisc  is not null ||
                LogHTTPResponse_toDisc is not null ||
                LogHTTPError_toDisc    is not null)
            {
                if (this.LoggingPath.IsNotNullOrEmpty())
                    Directory.CreateDirectory(this.LoggingPath);
            }

            this.LogfileCreator  = LogfileCreator ?? ((loggingPath, context, logfileName) => String.Concat(loggingPath,
                                                                                                           context != null ? context + "_" : "",
                                                                                                           logfileName, "_",
                                                                                                           DateTime.UtcNow.Year, "-",
                                                                                                           DateTime.UtcNow.Month.ToString("D2"),
                                                                                                           ".log"));

            #endregion

        }

        #endregion


        // Default logging delegates

        #region Default_LogRequest_toConsole (Context, LogEventName, Request)

        /// <summary>
        /// A default delegate for logging incoming HTTP requests to console.
        /// </summary>
        /// <param name="Context">The context of the log request.</param>
        /// <param name="LogEventName">The name of the log event.</param>
        /// <param name="Request">The HTTP request to log.</param>
        public Task Default_LogRequest_toConsole(String  LoggingPath,
                                                 String  Context,
                                                 String  LogEventName,
                                                 String? Request)
        {

            lock (LockObject)
            {

                var PreviousColor = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("[" + Timestamp.Now.ToLocalTime() + " T:" + Environment.CurrentManagedThreadId.ToString() + "] ");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(Context + "/");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(LogEventName);

                Console.Write(" ");

                //Console.ForegroundColor = ConsoleColor.Gray;
                //Console.WriteLine(Request.HTTPSource.Socket == Request.LocalSocket
                //                      ? String.Concat(Request.LocalSocket, " -> ", Request.RemoteSocket)
                //                      : String.Concat(Request.HTTPSource,  " -> ", Request.LocalSocket));
                Console.WriteLine(Request);

                Console.ForegroundColor = PreviousColor;

            }

            return Task.CompletedTask;

        }

        #endregion

        #region Default_LogResponse_toConsole(Context, LogEventName, Request, Response, Runtime)

        /// <summary>
        /// A default delegate for logging HTTP requests/-responses to console.
        /// </summary>
        /// <param name="Context">The context of the log request.</param>
        /// <param name="LogEventName">The name of the log event.</param>
        /// <param name="Request">The HTTP request to log.</param>
        /// <param name="Response">The HTTP response to log.</param>
        public Task Default_LogResponse_toConsole(String    LoggingPath,
                                                  String    Context,
                                                  String    LogEventName,
                                                  String?   Request,
                                                  String?   Response,
                                                  TimeSpan  Runtime)
        {

            lock (LockObject)
            {

                var PreviousColor = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("[" + Timestamp.Now.ToLocalTime() + " T:" + Environment.CurrentManagedThreadId.ToString() + "] ");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(Context + "/");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(LogEventName);

                Console.Write(" ");

                //Console.ForegroundColor = ConsoleColor.Gray;
                //Console.Write(String.Concat(" from ", Request.HTTPSource, " => "));

                //if (Response.HTTPStatusCode == HTTPStatusCode.OK ||
                //    Response.HTTPStatusCode == HTTPStatusCode.Created)
                //    Console.ForegroundColor = ConsoleColor.Green;

                //else if (Response.HTTPStatusCode == HTTPStatusCode.NoContent)
                //    Console.ForegroundColor = ConsoleColor.Yellow;

                //else
                //    Console.ForegroundColor = ConsoleColor.Red;

                //Console.Write(Response.HTTPStatusCode);

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(String.Concat(Response, " in ", Math.Round(Runtime.TotalMilliseconds), "ms"));

                Console.ForegroundColor = PreviousColor;

            }

            return Task.CompletedTask;

        }

        #endregion


        #region Default_LogRequest_toDisc (Context, LogEventName, Request)

        /// <summary>
        /// A default delegate for logging incoming HTTP requests to disc.
        /// </summary>
        /// <param name="Context">The context of the log request.</param>
        /// <param name="LogEventName">The name of the log event.</param>
        /// <param name="Request">The HTTP request to log.</param>
        public async Task Default_LogRequest_toDisc(String  LoggingPath,
                                                    String  Context,
                                                    String  LogEventName,
                                                    String  Request)
        {

            //ToDo: Can we have a lock per logfile?
            var LockTaken = await LogRequest_toDisc_Lock.WaitAsync(MaxWaitingForALock);

            try
            {

                if (LockTaken)
                {

                    var retry = 0;

                    do
                    {

                        try
                        {

                            File.AppendAllText(LogfileCreator(LoggingPath, Context, LogEventName),
                                               String.Concat("[" + Timestamp.Now.ToLocalTime() + " T:" + Environment.CurrentManagedThreadId.ToString() + "] ",
                                                             Context + "/",
                                                             LogEventName,
                                                             Request,
                                                             Environment.NewLine),
                                               Encoding.UTF8);

                            break;

                        }
                        catch (IOException e)
                        {

                            if (e.HResult != -2147024864)
                            {
                                DebugX.LogT("File access error while logging to '" + LogfileCreator(LoggingPath, Context, LogEventName) + "' (retry: " + retry + "): " + e.Message);
                                Thread.Sleep(100);
                            }

                            else
                            {
                                DebugX.LogT("Could not log to '" + LogfileCreator(LoggingPath, Context, LogEventName) + "': " + e.Message);
                                break;
                            }

                        }
                        catch (Exception e)
                        {
                            DebugX.LogT("Could not log to '" + LogfileCreator(LoggingPath, Context, LogEventName) + "': " + e.Message);
                            break;
                        }

                    }
                    while (retry++ < MaxRetries);

                    if (retry >= MaxRetries)
                        DebugX.LogT("Could not write to logfile '"      + LogfileCreator(LoggingPath, Context, LogEventName) + "' for "   + retry + " retries!");

                    else if (retry > 0)
                        DebugX.LogT("Successfully written to logfile '" + LogfileCreator(LoggingPath, Context, LogEventName) + "' after " + retry + " retries!");

                }

                else
                    DebugX.LogT("Could not get lock to log to '" + LogfileCreator(LoggingPath, Context, LogEventName) + "'!");

            }
            finally
            {
                if (LockTaken)
                    LogRequest_toDisc_Lock.Release();
            }

        }

        #endregion

        #region Default_LogResponse_toDisc(Context, LogEventName, Request, Response)

        /// <summary>
        /// A default delegate for logging HTTP requests/-responses to disc.
        /// </summary>
        /// <param name="Context">The context of the log request.</param>
        /// <param name="LogEventName">The name of the log event.</param>
        /// <param name="Request">The HTTP request to log.</param>
        /// <param name="Response">The HTTP response to log.</param>
        public async Task Default_LogResponse_toDisc(String    LoggingPath,
                                                     String    Context,
                                                     String    LogEventName,
                                                     String    Request,
                                                     String    Response,
                                                     TimeSpan  Runtime)
        {

            //ToDo: Can we have a lock per logfile?
            var LockTaken = await LogResponse_toDisc_Lock.WaitAsync(MaxWaitingForALock);

            try
            {

                if (LockTaken)
                {

                    var retry = 0;

                    do
                    {

                        try
                        {

                            File.AppendAllText(LogfileCreator(LoggingPath, Context, LogEventName),
                                               String.Concat("[" + Timestamp.Now.ToLocalTime() + " T:" + Environment.CurrentManagedThreadId.ToString() + "] ",
                                                             Context + "/",
                                                             LogEventName,
                                                             Request,
                                                             " => ",
                                                             Response,
                                                             " in ", Math.Round(Runtime.TotalMilliseconds), "ms",
                                                             Environment.NewLine),
                                               Encoding.UTF8);

                            break;

                        }
                        catch (IOException e)
                        {

                            if (e.HResult != -2147024864)
                            {
                                DebugX.LogT("File access error while logging to '" + LogfileCreator(LoggingPath, Context, LogEventName) + "' (retry: " + retry + "): " + e.Message);
                                Thread.Sleep(100);
                            }

                            else
                            {
                                DebugX.LogT("Could not log to '" + LogfileCreator(LoggingPath, Context, LogEventName) + "': " + e.Message);
                                break;
                            }

                        }
                        catch (Exception e)
                        {
                            DebugX.LogT("Could not log to '" + LogfileCreator(LoggingPath, Context, LogEventName) + "': " + e.Message);
                            break;
                        }

                    }
                    while (retry++ < MaxRetries);

                    if (retry >= MaxRetries)
                        DebugX.LogT("Could not write to logfile '"      + LogfileCreator(LoggingPath, Context, LogEventName) + "' for "   + retry + " retries!");

                    else if (retry > 0)
                        DebugX.LogT("Successfully written to logfile '" + LogfileCreator(LoggingPath, Context, LogEventName) + "' after " + retry + " retries!");

                }

                else
                    DebugX.LogT("Could not get lock to log to '" + LogfileCreator(LoggingPath, Context, LogEventName) + "'!");

            }
            finally
            {
                if (LockTaken)
                    LogResponse_toDisc_Lock.Release();
            }

        }

        #endregion



        #region Debug(LogEventOrGroupName, LogTarget)

        /// <summary>
        /// Start debugging the given log event.
        /// </summary>
        /// <param name="LogEventOrGroupName">A log event of group name.</param>
        /// <param name="LogTarget">The log target.</param>
        public Boolean Debug(String      LogEventOrGroupName,
                             LogTargets  LogTarget)
        {

            if (_GroupTags.TryGetValue(LogEventOrGroupName,
                                       out HashSet<String> _LogEventNames))

                return _LogEventNames.
                           Select(logname => InternalDebug(logname, LogTarget)).
                           All   (result  => result == true);


            return InternalDebug(LogEventOrGroupName, LogTarget);

        }

        #endregion

        #region (protected) InternalDebug(LogEventName, LogTarget)

        protected abstract Boolean InternalDebug(String      LogEventName,
                                                 LogTargets  LogTarget);

        #endregion


        #region Undebug(LogEventOrGroupName, LogTarget)

        /// <summary>
        /// Stop debugging the given log event.
        /// </summary>
        /// <param name="LogEventOrGroupName">A log event of group name.</param>
        /// <param name="LogTarget">The log target.</param>
        public Boolean Undebug(String      LogEventOrGroupName,
                               LogTargets  LogTarget)
        {

            if (_GroupTags.TryGetValue(LogEventOrGroupName,
                                       out HashSet<String> _LogEventNames))

                return _LogEventNames.
                           Select(logname => InternalUndebug(logname, LogTarget)).
                           All   (result  => result == true);


            return InternalUndebug(LogEventOrGroupName, LogTarget);

        }

        #endregion

        #region (private) InternalUndebug(LogEventName, LogTarget)

        protected abstract Boolean InternalUndebug(String      LogEventName,
                                                   LogTargets  LogTarget);

        #endregion

    }





    /// <summary>
    /// A client logger.
    /// </summary>
    public class ClientLogger : ALogger
    {

        #region (class) APIClientRequestLogger

        /// <summary>
        /// A wrapper class to manage API event subscriptions for logging purposes.
        /// </summary>
        public class APIClientRequestLogger
        {

            #region Data

            private readonly Dictionary<LogTargets, APIClientRequestLogHandler>  _SubscriptionDelegates;
            private readonly HashSet<LogTargets>                                 _SubscriptionStatus;

            #endregion

            #region Properties

            /// <summary>
            /// The logging path.
            /// </summary>
            public String                              LoggingPath                     { get; }

            /// <summary>
            /// The context of the event to be logged.
            /// </summary>
            public String                              Context                         { get; }

            /// <summary>
            /// The name of the event to be logged.
            /// </summary>
            public String                              LogEventName                    { get; }

            /// <summary>
            /// A delegate called whenever the event is subscriped to.
            /// </summary>
            public Action<APIClientRequestLogHandler>  SubscribeToEventDelegate        { get; }

            /// <summary>
            /// A delegate called whenever the subscription of the event is stopped.
            /// </summary>
            public Action<APIClientRequestLogHandler>  UnsubscribeFromEventDelegate    { get; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new log event for the linked HTTP API event.
            /// </summary>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">The context of the event.</param>
            /// <param name="LogEventName">The name of the event.</param>
            /// <param name="SubscribeToEventDelegate">A delegate for subscribing to the linked event.</param>
            /// <param name="UnsubscribeFromEventDelegate">A delegate for subscribing from the linked event.</param>
            public APIClientRequestLogger(String                              LoggingPath,
                                          String                              Context,
                                          String                              LogEventName,
                                          Action<APIClientRequestLogHandler>  SubscribeToEventDelegate,
                                          Action<APIClientRequestLogHandler>  UnsubscribeFromEventDelegate)
            {

                #region Initial checks

                if (LogEventName.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(LogEventName),                 "The given log event name must not be null or empty!");

                if (SubscribeToEventDelegate     is null)
                    throw new ArgumentNullException(nameof(SubscribeToEventDelegate),     "The given delegate for subscribing to the linked HTTP API event must not be null!");

                if (UnsubscribeFromEventDelegate is null)
                    throw new ArgumentNullException(nameof(UnsubscribeFromEventDelegate), "The given delegate for unsubscribing from the linked HTTP API event must not be null!");

                #endregion

                this.LoggingPath                   = LoggingPath ?? "";
                this.Context                       = Context     ?? "";
                this.LogEventName                  = LogEventName;
                this.SubscribeToEventDelegate      = SubscribeToEventDelegate;
                this.UnsubscribeFromEventDelegate  = UnsubscribeFromEventDelegate;
                this._SubscriptionDelegates        = new Dictionary<LogTargets, APIClientRequestLogHandler>();
                this._SubscriptionStatus           = new HashSet<LogTargets>();

            }

            #endregion


            #region RegisterLogTarget(LogTarget, RequestDelegate)

            /// <summary>
            /// Register the given log target and delegate combination.
            /// </summary>
            /// <param name="LogTarget">A log target.</param>
            /// <param name="RequestDelegate">A delegate to call.</param>
            /// <returns>A request logger.</returns>
            public APIClientRequestLogger RegisterLogTarget(LogTargets             LogTarget,
                                                            RequestLoggerDelegate  RequestDelegate)
            {

                #region Initial checks

                if (RequestDelegate is null)
                    throw new ArgumentNullException(nameof(RequestDelegate),  "The given delegate must not be null!");

                #endregion

                if (_SubscriptionDelegates.ContainsKey(LogTarget))
                    throw new Exception("Duplicate log target!");

                _SubscriptionDelegates.Add(LogTarget,
                                           (timestamp, HTTPAPI, Request) => RequestDelegate(LoggingPath, Context, LogEventName, Request));

                return this;

            }

            #endregion

            #region Subscribe   (LogTarget)

            /// <summary>
            /// Subscribe the given log target to the linked event.
            /// </summary>
            /// <param name="LogTarget">A log target.</param>
            /// <returns>True, if successful; false else.</returns>
            public Boolean Subscribe(LogTargets LogTarget)
            {

                if (IsSubscribed(LogTarget))
                    return true;

                if (_SubscriptionDelegates.TryGetValue(LogTarget,
                                                       out APIClientRequestLogHandler clientRequestLogHandler))
                {
                    SubscribeToEventDelegate(clientRequestLogHandler);
                    _SubscriptionStatus.Add(LogTarget);
                    return true;
                }

                return false;

            }

            #endregion

            #region IsSubscribed(LogTarget)

            /// <summary>
            /// Return the subscription status of the given log target.
            /// </summary>
            /// <param name="LogTarget">A log target.</param>
            public Boolean IsSubscribed(LogTargets LogTarget)

                => _SubscriptionStatus.Contains(LogTarget);

            #endregion

            #region Unsubscribe (LogTarget)

            /// <summary>
            /// Unsubscribe the given log target from the linked event.
            /// </summary>
            /// <param name="LogTarget">A log target.</param>
            /// <returns>True, if successful; false else.</returns>
            public Boolean Unsubscribe(LogTargets LogTarget)
            {

                if (!IsSubscribed(LogTarget))
                    return true;

                if (_SubscriptionDelegates.TryGetValue(LogTarget,
                                                       out APIClientRequestLogHandler clientRequestLogHandler))
                {
                    UnsubscribeFromEventDelegate(clientRequestLogHandler);
                    _SubscriptionStatus.Remove(LogTarget);
                    return true;
                }

                return false;

            }

            #endregion

        }

        #endregion

        #region (class) APIClientResponseLogger

        /// <summary>
        /// A wrapper class to manage API event subscriptions for logging purposes.
        /// </summary>
        public class APIClientResponseLogger
        {

            #region Data

            private readonly Dictionary<LogTargets, APIClientResponseLogHandler>  _SubscriptionDelegates;
            private readonly HashSet<LogTargets>                                  _SubscriptionStatus;

            #endregion

            #region Properties

            /// <summary>
            /// The logging path.
            /// </summary>
            public String                               LoggingPath                     { get; }

            /// <summary>
            /// The context of the event to be logged.
            /// </summary>
            public String                               Context                         { get; }

            /// <summary>
            /// The name of the event to be logged.
            /// </summary>
            public String                               LogEventName                    { get; }

            /// <summary>
            /// A delegate called whenever the event is subscriped to.
            /// </summary>
            public Action<APIClientResponseLogHandler>  SubscribeToEventDelegate        { get; }

            /// <summary>
            /// A delegate called whenever the subscription of the event is stopped.
            /// </summary>
            public Action<APIClientResponseLogHandler>  UnsubscribeFromEventDelegate    { get; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new log event for the linked HTTP API event.
            /// </summary>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">The context of the event.</param>
            /// <param name="LogEventName">The name of the event.</param>
            /// <param name="SubscribeToEventDelegate">A delegate for subscribing to the linked event.</param>
            /// <param name="UnsubscribeFromEventDelegate">A delegate for subscribing from the linked event.</param>
            public APIClientResponseLogger(String                               LoggingPath,
                                           String                               Context,
                                           String                               LogEventName,
                                           Action<APIClientResponseLogHandler>  SubscribeToEventDelegate,
                                           Action<APIClientResponseLogHandler>  UnsubscribeFromEventDelegate)
            {

                #region Initial checks

                if (LogEventName.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(LogEventName),                 "The given log event name must not be null or empty!");

                if (SubscribeToEventDelegate     is null)
                    throw new ArgumentNullException(nameof(SubscribeToEventDelegate),     "The given delegate for subscribing to the linked  HTTP API event must not be null!");

                if (UnsubscribeFromEventDelegate is null)
                    throw new ArgumentNullException(nameof(UnsubscribeFromEventDelegate), "The given delegate for unsubscribing from the linked HTTP API event must not be null!");

                #endregion

                this.LoggingPath                   = LoggingPath ?? "";
                this.Context                       = Context     ?? "";
                this.LogEventName                  = LogEventName;
                this.SubscribeToEventDelegate      = SubscribeToEventDelegate;
                this.UnsubscribeFromEventDelegate  = UnsubscribeFromEventDelegate;
                this._SubscriptionDelegates        = new Dictionary<LogTargets, APIClientResponseLogHandler>();
                this._SubscriptionStatus           = new HashSet<LogTargets>();

            }

            #endregion


            #region RegisterLogTarget(LogTarget, ResponseDelegate)

            /// <summary>
            /// Register the given log target and delegate combination.
            /// </summary>
            /// <param name="LogTarget">A log target.</param>
            /// <param name="ResponseDelegate">A delegate to call.</param>
            /// <returns>A HTTP response logger.</returns>
            public APIClientResponseLogger RegisterLogTarget(LogTargets             LogTarget,
                                                             ResponseLoggerDelegate ResponseDelegate)
            {

                #region Initial checks

                if (ResponseDelegate is null)
                    throw new ArgumentNullException(nameof(ResponseDelegate), "The given delegate must not be null!");

                #endregion

                if (_SubscriptionDelegates.ContainsKey(LogTarget))
                    throw new Exception("Duplicate log target!");

                _SubscriptionDelegates.Add(LogTarget,
                                           (timestamp, HTTPAPI, Request, Response, Runtime) => ResponseDelegate(LoggingPath, Context, LogEventName, Request, Response, Runtime));

                return this;

            }

            #endregion

            #region Subscribe   (LogTarget)

            /// <summary>
            /// Subscribe the given log target to the linked event.
            /// </summary>
            /// <param name="LogTarget">A log target.</param>
            /// <returns>True, if successful; false else.</returns>
            public Boolean Subscribe(LogTargets LogTarget)
            {

                if (IsSubscribed(LogTarget))
                    return true;

                if (_SubscriptionDelegates.TryGetValue(LogTarget,
                                                       out APIClientResponseLogHandler clientResponseLogHandler))
                {
                    SubscribeToEventDelegate(clientResponseLogHandler);
                    _SubscriptionStatus.Add(LogTarget);
                    return true;
                }

                return false;

            }

            #endregion

            #region IsSubscribed(LogTarget)

            /// <summary>
            /// Return the subscription status of the given log target.
            /// </summary>
            /// <param name="LogTarget">A log target.</param>
            public Boolean IsSubscribed(LogTargets LogTarget)

                => _SubscriptionStatus.Contains(LogTarget);

            #endregion

            #region Unsubscribe (LogTarget)

            /// <summary>
            /// Unsubscribe the given log target from the linked event.
            /// </summary>
            /// <param name="LogTarget">A log target.</param>
            /// <returns>True, if successful; false else.</returns>
            public Boolean Unsubscribe(LogTargets LogTarget)
            {

                if (!IsSubscribed(LogTarget))
                    return true;

                if (_SubscriptionDelegates.TryGetValue(LogTarget,
                                                       out APIClientResponseLogHandler clientResponseLogHandler))
                {
                    UnsubscribeFromEventDelegate(clientResponseLogHandler);
                    _SubscriptionStatus.Remove(LogTarget);
                    return true;
                }

                return false;

            }

            #endregion

        }

        #endregion


        #region Data

        private readonly ConcurrentDictionary<String, APIClientRequestLogger>   _HTTPClientRequestLoggers;
        private readonly ConcurrentDictionary<String, APIClientResponseLogger>  _HTTPClientResponseLoggers;

        #endregion

        #region Properties

        /// <summary>
        /// The HTTP client of this logger.
        /// </summary>
        public IHTTPClient  HTTPClient        { get; }

        /// <summary>
        /// Whether to disable HTTP client logging.
        /// </summary>
        public Boolean      DisableLogging    { get; set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new HTTP client logger using the given logging delegates.
        /// </summary>
        /// <param name="HTTPClient">A HTTP client.</param>
        /// <param name="LoggingPath">The logging path.</param>
        /// <param name="Context">A context of this API.</param>
        /// 
        /// <param name="LogHTTPRequest_toConsole">A delegate to log incoming HTTP requests to console.</param>
        /// <param name="LogHTTPResponse_toConsole">A delegate to log HTTP requests/responses to console.</param>
        /// <param name="LogHTTPRequest_toDisc">A delegate to log incoming HTTP requests to disc.</param>
        /// <param name="LogHTTPResponse_toDisc">A delegate to log HTTP requests/responses to disc.</param>
        /// 
        /// <param name="LogHTTPRequest_toNetwork">A delegate to log incoming HTTP requests to a network target.</param>
        /// <param name="LogHTTPResponse_toNetwork">A delegate to log HTTP requests/responses to a network target.</param>
        /// <param name="LogHTTPRequest_toHTTPSSE">A delegate to log incoming HTTP requests to a HTTP server sent events source.</param>
        /// <param name="LogHTTPResponse_toHTTPSSE">A delegate to log HTTP requests/responses to a HTTP server sent events source.</param>
        /// 
        /// <param name="LogHTTPError_toConsole">A delegate to log HTTP errors to console.</param>
        /// <param name="LogHTTPError_toDisc">A delegate to log HTTP errors to disc.</param>
        /// <param name="LogHTTPError_toNetwork">A delegate to log HTTP errors to a network target.</param>
        /// <param name="LogHTTPError_toHTTPSSE">A delegate to log HTTP errors to a HTTP server sent events source.</param>
        /// 
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public ClientLogger(IHTTPClient              HTTPClient,
                            String?                  LoggingPath,
                            String                   Context,

                            RequestLoggerDelegate?   LogRequest_toConsole    = null,
                            ResponseLoggerDelegate?  LogResponse_toConsole   = null,
                            RequestLoggerDelegate?   LogRequest_toDisc       = null,
                            ResponseLoggerDelegate?  LogResponse_toDisc      = null,

                            RequestLoggerDelegate?   LogRequest_toNetwork    = null,
                            ResponseLoggerDelegate?  LogResponse_toNetwork   = null,
                            RequestLoggerDelegate?   LogRequest_toHTTPSSE    = null,
                            ResponseLoggerDelegate?  LogResponse_toHTTPSSE   = null,

                            ResponseLoggerDelegate?  LogError_toConsole      = null,
                            ResponseLoggerDelegate?  LogError_toDisc         = null,
                            ResponseLoggerDelegate?  LogError_toNetwork      = null,
                            ResponseLoggerDelegate?  LogError_toHTTPSSE      = null,

                            LogfileCreatorDelegate?  LogfileCreator          = null)

            : base(LoggingPath,
                   Context,

                   LogRequest_toConsole,
                   LogResponse_toConsole,
                   LogRequest_toDisc,
                   LogResponse_toDisc,

                   LogRequest_toNetwork,
                   LogResponse_toNetwork,
                   LogRequest_toHTTPSSE,
                   LogResponse_toHTTPSSE,

                   LogError_toConsole,
                   LogError_toDisc,
                   LogError_toNetwork,
                   LogError_toHTTPSSE,

                   LogfileCreator)

        {

            this.HTTPClient                  = HTTPClient ?? throw new ArgumentNullException(nameof(HTTPClient), "The given HTTP client must not be null!");

            this._HTTPClientRequestLoggers   = new ConcurrentDictionary<String, APIClientRequestLogger>();
            this._HTTPClientResponseLoggers  = new ConcurrentDictionary<String, APIClientResponseLogger>();


            //ToDo: Evaluate Logging targets!

          //  HTTPAPI.ErrorLog += async (Timestamp,
          //                             HTTPServer,
          //                             HTTPRequest,
          //                             HTTPResponse,
          //                             Error,
          //                             LastException) => {
          //
          //              DebugX.Log(Timestamp + " - " +
          //                         HTTPRequest.RemoteSocket.IPAddress + ":" +
          //                         HTTPRequest.RemoteSocket.Port + " " +
          //                         HTTPRequest.HTTPMethod + " " +
          //                         HTTPRequest.URI + " " +
          //                         HTTPRequest.ProtocolVersion + " => " +
          //                         HTTPResponse.HTTPStatusCode + " - " +
          //                         Error);

          //         };

        }

        #endregion


        #region (protected) RegisterRequestEvent(LogEventName, SubscribeToEventDelegate, UnsubscribeFromEventDelegate, params GroupTags)

        /// <summary>
        /// Register a log event for the linked HTTP API event.
        /// </summary>
        /// <param name="LogEventName">The name of the log event.</param>
        /// <param name="SubscribeToEventDelegate">A delegate for subscribing to the linked event.</param>
        /// <param name="UnsubscribeFromEventDelegate">A delegate for subscribing from the linked event.</param>
        /// <param name="GroupTags">An array of log event groups the given log event name is part of.</param>
        protected APIClientRequestLogger RegisterRequestEvent(String                              LogEventName,
                                                              Action<APIClientRequestLogHandler>  SubscribeToEventDelegate,
                                                              Action<APIClientRequestLogHandler>  UnsubscribeFromEventDelegate,
                                                              params String[]                     GroupTags)
        {

            #region Initial checks

            if (LogEventName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(LogEventName),                  "The given log event name must not be null or empty!");

            if (SubscribeToEventDelegate is null)
                throw new ArgumentNullException(nameof(SubscribeToEventDelegate),      "The given delegate for subscribing to the linked HTTP API event must not be null!");

            if (UnsubscribeFromEventDelegate is null)
                throw new ArgumentNullException(nameof(UnsubscribeFromEventDelegate),  "The given delegate for unsubscribing from the linked HTTP API event must not be null!");

            #endregion

            if (!_HTTPClientRequestLoggers. TryGetValue(LogEventName, out APIClientRequestLogger? apiClientRequestLogger) &&
                !_HTTPClientResponseLoggers.ContainsKey(LogEventName))
            {

                apiClientRequestLogger = new APIClientRequestLogger(LoggingPath, Context, LogEventName, SubscribeToEventDelegate, UnsubscribeFromEventDelegate);
                _HTTPClientRequestLoggers.TryAdd(LogEventName, apiClientRequestLogger);

                #region Register group tag mapping

                foreach (var GroupTag in GroupTags.Distinct())
                {

                    if (_GroupTags.TryGetValue(GroupTag, out HashSet<String>? logEventNames))
                        logEventNames.Add(LogEventName);

                    else
                        _GroupTags.TryAdd(GroupTag, new HashSet<String>(new String[] { LogEventName }));

                }

                #endregion

                return apiClientRequestLogger;

            }

            throw new Exception("Duplicate log event name!");

        }

        #endregion

        #region (protected) RegisterResponseEvent(LogEventName, SubscribeToEventDelegate, UnsubscribeFromEventDelegate, params GroupTags)

        /// <summary>
        /// Register a log event for the linked HTTP API event.
        /// </summary>
        /// <param name="LogEventName">The name of the log event.</param>
        /// <param name="SubscribeToEventDelegate">A delegate for subscribing to the linked event.</param>
        /// <param name="UnsubscribeFromEventDelegate">A delegate for subscribing from the linked event.</param>
        /// <param name="GroupTags">An array of log event groups the given log event name is part of.</param>
        protected APIClientResponseLogger RegisterResponseEvent(String                               LogEventName,
                                                                Action<APIClientResponseLogHandler>  SubscribeToEventDelegate,
                                                                Action<APIClientResponseLogHandler>  UnsubscribeFromEventDelegate,
                                                                params String[]                      GroupTags)
        {

            #region Initial checks

            if (LogEventName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(LogEventName),                  "The given log event name must not be null or empty!");

            if (SubscribeToEventDelegate is null)
                throw new ArgumentNullException(nameof(SubscribeToEventDelegate),      "The given delegate for subscribing to the linked HTTP API event must not be null!");

            if (UnsubscribeFromEventDelegate is null)
                throw new ArgumentNullException(nameof(UnsubscribeFromEventDelegate),  "The given delegate for unsubscribing from the linked HTTP API event must not be null!");

            #endregion

            if (!_HTTPClientResponseLoggers.TryGetValue(LogEventName, out APIClientResponseLogger? httpClientResponseLogger) &&
                !_HTTPClientRequestLoggers. ContainsKey(LogEventName))
            {

                httpClientResponseLogger = new APIClientResponseLogger(LoggingPath, Context, LogEventName, SubscribeToEventDelegate, UnsubscribeFromEventDelegate);
                _HTTPClientResponseLoggers.TryAdd(LogEventName, httpClientResponseLogger);

                #region Register group tag mapping

                foreach (var GroupTag in GroupTags.Distinct())
                {

                    if (_GroupTags.TryGetValue(GroupTag, out HashSet<String>? logEventNames))
                        logEventNames.Add(LogEventName);

                    else
                        _GroupTags.TryAdd(GroupTag, new HashSet<String>(new String[] { LogEventName }));

                }

                #endregion

                return httpClientResponseLogger;

            }

            throw new Exception("Duplicate log event name!");

        }

        #endregion


        #region (protected) InternalDebug  (LogEventName, LogTarget)

        protected override Boolean InternalDebug(String      LogEventName,
                                                 LogTargets  LogTarget)
        {

            var found = false;

            // HTTP Client
            if (_HTTPClientRequestLoggers. TryGetValue(LogEventName, out APIClientRequestLogger?  apiClientRequestLogger))
                found |= apiClientRequestLogger. Subscribe(LogTarget);

            if (_HTTPClientResponseLoggers.TryGetValue(LogEventName, out APIClientResponseLogger? apiClientResponseLogger))
                found |= apiClientResponseLogger.Subscribe(LogTarget);

            return found;

        }

        #endregion

        #region (protected) InternalUndebug(LogEventName, LogTarget)

        protected override Boolean InternalUndebug(String      LogEventName,
                                                   LogTargets  LogTarget)
        {

            var found = false;

            if (_HTTPClientRequestLoggers. TryGetValue(LogEventName, out APIClientRequestLogger?  apiClientRequestLogger))
                found |= apiClientRequestLogger. Unsubscribe(LogTarget);

            if (_HTTPClientResponseLoggers.TryGetValue(LogEventName, out APIClientResponseLogger? apiClientResponseLogger))
                found |= apiClientResponseLogger.Unsubscribe(LogTarget);

            return found;

        }

        #endregion


    }








    /// <summary>
    /// The CPO client.
    /// </summary>
    public partial class CPOClient
    {

        /// <summary>
        /// The CPO client (HTTP client) logger.
        /// </summary>
        public class API_Logger : ClientLogger
        {

            #region Data

            /// <summary>
            /// The default context for this logger.
            /// </summary>
            public const String DefaultContext = "OICPCPOClient";

            #endregion

            #region Properties

            /// <summary>
            /// The attached CPO client.
            /// </summary>
            public CPOClient  CPOClient    { get; }

            #endregion

            #region Constructor(s)

            #region Logger(CPOClient, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new CPO client logger using the default logging delegates.
            /// </summary>
            /// <param name="CPOClient">A CPO client.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public API_Logger(CPOClient                CPOClient,
                              String                   LoggingPath,
                              String                   Context         = DefaultContext,
                              LogfileCreatorDelegate?  LogfileCreator  = null)

                : this(CPOClient,
                       LoggingPath,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                       null,
                       null,
                       null,
                       null,

                       LogfileCreator: LogfileCreator)

            { }

            #endregion

            #region Logger(CPOClient, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new CPO client logger using the given logging delegates.
            /// </summary>
            /// <param name="CPOClient">A CPO client.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// 
            /// <param name="LogHTTPRequest_toConsole">A delegate to log incoming HTTP requests to console.</param>
            /// <param name="LogHTTPResponse_toConsole">A delegate to log HTTP requests/responses to console.</param>
            /// <param name="LogHTTPRequest_toDisc">A delegate to log incoming HTTP requests to disc.</param>
            /// <param name="LogHTTPResponse_toDisc">A delegate to log HTTP requests/responses to disc.</param>
            /// 
            /// <param name="LogHTTPRequest_toNetwork">A delegate to log incoming HTTP requests to a network target.</param>
            /// <param name="LogHTTPResponse_toNetwork">A delegate to log HTTP requests/responses to a network target.</param>
            /// <param name="LogHTTPRequest_toHTTPSSE">A delegate to log incoming HTTP requests to a HTTP client sent events source.</param>
            /// <param name="LogHTTPResponse_toHTTPSSE">A delegate to log HTTP requests/responses to a HTTP client sent events source.</param>
            /// 
            /// <param name="LogHTTPError_toConsole">A delegate to log HTTP errors to console.</param>
            /// <param name="LogHTTPError_toDisc">A delegate to log HTTP errors to disc.</param>
            /// <param name="LogHTTPError_toNetwork">A delegate to log HTTP errors to a network target.</param>
            /// <param name="LogHTTPError_toHTTPSSE">A delegate to log HTTP errors to a HTTP client sent events source.</param>
            /// 
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public API_Logger(CPOClient                CPOClient,
                              String                   LoggingPath,
                              String                   Context,

                              RequestLoggerDelegate?   LogRequest_toConsole    = null,
                              ResponseLoggerDelegate?  LogResponse_toConsole   = null,
                              RequestLoggerDelegate?   LogRequest_toDisc       = null,
                              ResponseLoggerDelegate?  LogResponse_toDisc      = null,

                              RequestLoggerDelegate?   LogRequest_toNetwork    = null,
                              ResponseLoggerDelegate?  LogResponse_toNetwork   = null,
                              RequestLoggerDelegate?   LogRequest_toHTTPSSE    = null,
                              ResponseLoggerDelegate?  LogResponse_toHTTPSSE   = null,

                              ResponseLoggerDelegate?  LogError_toConsole      = null,
                              ResponseLoggerDelegate?  LogError_toDisc         = null,
                              ResponseLoggerDelegate?  LogError_toNetwork      = null,
                              ResponseLoggerDelegate?  LogError_toHTTPSSE      = null,

                              LogfileCreatorDelegate?  LogfileCreator          = null)

                : base(CPOClient,
                       LoggingPath,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,

                       LogRequest_toConsole,
                       LogResponse_toConsole,
                       LogRequest_toDisc,
                       LogResponse_toDisc,

                       LogRequest_toNetwork,
                       LogResponse_toNetwork,
                       LogRequest_toHTTPSSE,
                       LogResponse_toHTTPSSE,

                       LogError_toConsole,
                       LogError_toDisc,
                       LogError_toNetwork,
                       LogError_toHTTPSSE,

                       LogfileCreator)

            {

                this.CPOClient = CPOClient ?? throw new ArgumentNullException(nameof(CPOClient), "The given CPO client must not be null!");

                #region PushEVSEData/Status

                //RegisterEvent("PushEVSEDataHTTPRequest",
                //              handler => CPOClient.OnPushEVSEDataHTTPRequest += handler,
                //              handler => CPOClient.OnPushEVSEDataHTTPRequest -= handler,
                //              "PushEVSEData", "push", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("PushEVSEDataHTTPResponse",
                //              handler => CPOClient.OnPushEVSEDataHTTPResponse += handler,
                //              handler => CPOClient.OnPushEVSEDataHTTPResponse -= handler,
                //              "PushEVSEData", "push", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);


                //RegisterEvent("PushEVSEStatusHTTPRequest",
                //              handler => CPOClient.OnPushEVSEStatusHTTPRequest += handler,
                //              handler => CPOClient.OnPushEVSEStatusHTTPRequest -= handler,
                //              "PushEVSEStatus", "push", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("PushEVSEStatusHTTPResponse",
                //              handler => CPOClient.OnPushEVSEStatusHTTPResponse += handler,
                //              handler => CPOClient.OnPushEVSEStatusHTTPResponse -= handler,
                //              "PushEVSEStatus", "push", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region PushPricingProductData/EVSEPricing

                //RegisterEvent("PushPricingProductDataHTTPRequest",
                //              handler => CPOClient.OnPushPricingProductDataHTTPRequest += handler,
                //              handler => CPOClient.OnPushPricingProductDataHTTPRequest -= handler,
                //              "PushPricingProductData", "PushPricing", "push", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("PushPricingProductDataHTTPResponse",
                //              handler => CPOClient.OnPushPricingProductDataHTTPResponse += handler,
                //              handler => CPOClient.OnPushPricingProductDataHTTPResponse -= handler,
                //              "PushPricingProductData", "PushPricing", "push", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);


                //RegisterEvent("PushEVSEPricingHTTPRequest",
                //              handler => CPOClient.OnPushEVSEPricingHTTPRequest += handler,
                //              handler => CPOClient.OnPushEVSEPricingHTTPRequest -= handler,
                //              "PushEVSEPricing", "PushPricing", "push", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("PushEVSEPricingHTTPResponse",
                //              handler => CPOClient.OnPushEVSEPricingHTTPResponse += handler,
                //              handler => CPOClient.OnPushEVSEPricingHTTPResponse -= handler,
                //              "PushEVSEPricing", "PushPricing", "push", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AuthorizeStart/Stop

                RegisterRequestEvent("AuthorizeStartRequest",
                                     handler => CPOClient.OnAuthorizeStartRequest += (timestamp, sender, request) => handler(timestamp, sender, CPOClient?.AuthorizeStartRequestConverter(timestamp, sender, request)),
                                     handler => CPOClient.OnAuthorizeStartRequest -= (timestamp, sender, request) => handler(timestamp, sender, CPOClient?.AuthorizeStartRequestConverter(timestamp, sender, request)),
                                     "authorizeStart", "authorize", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this);
                //  RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("AuthorizeStartResponse",
                                      handler => CPOClient.OnAuthorizeStartResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.AuthorizeStartRequestConverter(timestamp, sender, request), CPOClient?.AuthorizationStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => CPOClient.OnAuthorizeStartResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.AuthorizeStartRequestConverter(timestamp, sender, request), CPOClient?.AuthorizationStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "authorizeStart", "authorize", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this);
                //    RegisterDefaultDiscLogTarget(this);


                //RegisterEvent("AuthorizeStopHTTPRequest",
                //              handler => CPOClient.OnAuthorizeStopHTTPRequest += handler,
                //              handler => CPOClient.OnAuthorizeStopHTTPRequest -= handler,
                //              "authorizeStop", "authorize", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("AuthorizeStopHTTPResponse",
                //              handler => CPOClient.OnAuthorizeStopHTTPResponse += handler,
                //              handler => CPOClient.OnAuthorizeStopHTTPResponse -= handler,
                //              "authorizeStop", "authorize", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region ChargingNotifications

                //RegisterEvent("ChargingStartNotificationHTTPRequest",
                //              handler => CPOClient.OnChargingStartNotificationHTTPRequest += handler,
                //              handler => CPOClient.OnChargingStartNotificationHTTPRequest -= handler,
                //              "chargingStartNotification", "chargingNotifications", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("ChargingStartNotificationHTTPResponse",
                //              handler => CPOClient.OnChargingStartNotificationHTTPResponse += handler,
                //              handler => CPOClient.OnChargingStartNotificationHTTPResponse -= handler,
                //              "chargingStartNotification", "chargingNotifications", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);


                //RegisterEvent("ChargingProgressNotificationHTTPRequest",
                //              handler => CPOClient.OnChargingProgressNotificationHTTPRequest += handler,
                //              handler => CPOClient.OnChargingProgressNotificationHTTPRequest -= handler,
                //              "chargingProgressNotification", "chargingNotifications", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("ChargingProgressNotificationHTTPResponse",
                //              handler => CPOClient.OnChargingProgressNotificationHTTPResponse += handler,
                //              handler => CPOClient.OnChargingProgressNotificationHTTPResponse -= handler,
                //              "chargingProgressNotification", "chargingNotifications", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);


                //RegisterEvent("ChargingEndNotificationHTTPRequest",
                //              handler => CPOClient.OnChargingEndNotificationHTTPRequest += handler,
                //              handler => CPOClient.OnChargingEndNotificationHTTPRequest -= handler,
                //              "chargingEndNotification", "chargingNotifications", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("ChargingEndNotificationHTTPResponse",
                //              handler => CPOClient.OnChargingEndNotificationHTTPResponse += handler,
                //              handler => CPOClient.OnChargingEndNotificationHTTPResponse -= handler,
                //              "chargingEndNotification", "chargingNotifications", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);


                //RegisterEvent("ChargingErrorNotificationHTTPRequest",
                //              handler => CPOClient.OnChargingErrorNotificationHTTPRequest += handler,
                //              handler => CPOClient.OnChargingErrorNotificationHTTPRequest -= handler,
                //              "authorizeStop", "chargingNotifications", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("ChargingErrorNotificationHTTPResponse",
                //              handler => CPOClient.OnChargingErrorNotificationHTTPResponse += handler,
                //              handler => CPOClient.OnChargingErrorNotificationHTTPResponse -= handler,
                //              "chargingErrorNotification", "chargingNotifications", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region SendChargeDetailRecord

                //RegisterEvent("SendChargeDetailRecordHTTPRequest",
                //              handler => CPOClient.OnSendChargeDetailRecordHTTPRequest += handler,
                //              handler => CPOClient.OnSendChargeDetailRecordHTTPRequest -= handler,
                //              "sendChargeDetailRecord", "cdr", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("SendChargeDetailRecordHTTPResponse",
                //              handler => CPOClient.OnSendChargeDetailRecordHTTPResponse += handler,
                //              handler => CPOClient.OnSendChargeDetailRecordHTTPResponse -= handler,
                //              "sendChargeDetailRecord", "cdr", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                #endregion

            }

            #endregion

            #endregion

        }

     }

}
