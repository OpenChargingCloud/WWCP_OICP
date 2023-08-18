/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;
using System.Runtime.CompilerServices;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    public class PushSingleEVSEDataResult
    {

        public EVSEDataRecord             EVSE        { get; }
        public PushSingleDataResultTypes  Result      { get; }
        public IEnumerable<Warning>       Warnings    { get; }

        public PushSingleEVSEDataResult(EVSEDataRecord             EVSE,
                                        PushSingleDataResultTypes  Result,
                                        IEnumerable<Warning>?      Warnings   = null)
        {

            this.EVSE      = EVSE;
            this.Result    = Result;
            this.Warnings  = Warnings is not null
                                 ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                 : Array.Empty<Warning>();

        }

    }


    /// <summary>
    /// A PushData result.
    /// </summary>
    public class PushEVSEDataRecordResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                                    SenderId             { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushDataResultTypes                    Result             { get; }

        /// <summary>
        /// The optional description of the result code.
        /// </summary>
        public String?                                Description        { get; }

        /// <summary>
        /// The enumeration of successfully uploaded EVSEs.
        /// </summary>
        public IEnumerable<PushSingleEVSEDataResult>  SuccessfulEVSEs    { get; }

        /// <summary>
        /// The enumeration of rejected EVSEs.
        /// </summary>
        public IEnumerable<PushSingleEVSEDataResult>  RejectedEVSEs      { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>                   Warnings           { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                              Runtime            { get;  }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PushEVSEData result.
        /// </summary>
        /// <param name="SenderId">The unqiue identification of the sender.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="SuccessfulEVSEs">An enumeration of successfully uploaded EVSEs.</param>
        /// <param name="RejectedEVSEs">An enumeration of rejected EVSEs.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public PushEVSEDataRecordResult(IId                                     SenderId,
                                        PushDataResultTypes                     Result,
                                        IEnumerable<PushSingleEVSEDataResult>?  SuccessfulEVSEs   = null,
                                        IEnumerable<PushSingleEVSEDataResult>?  RejectedEVSEs     = null,
                                        String?                                 Description       = null,
                                        IEnumerable<Warning>?                   Warnings          = null,
                                        TimeSpan?                               Runtime           = null)
        {

            this.SenderId         = SenderId;
            this.Result           = Result;

            this.Description      = Description is not null && Description.IsNotNullOrEmpty()
                                        ? Description.Trim()
                                        : null;

            this.SuccessfulEVSEs  = SuccessfulEVSEs is not null
                                        ? SuccessfulEVSEs.Where(evse    => evse is not null)
                                        : Array.Empty<PushSingleEVSEDataResult>();

            this.RejectedEVSEs    = RejectedEVSEs   is not null
                                        ? RejectedEVSEs.  Where(evse    => evse is not null)
                                        : Array.Empty<PushSingleEVSEDataResult>();

            this.Warnings         = Warnings        is not null
                                        ? Warnings.       Where(warning => warning.IsNeitherNullNorEmpty())
                                        : Array.Empty<Warning>();

            this.Runtime          = Runtime;

        }

        #endregion


        #region (static) AdminDown

        public static PushEVSEDataRecordResult

            AdminDown(IId                    AuthId,
                      IEnumerable<EVSEDataRecord>     RejectedEVSEs,
                      String?                Description   = null,
                      IEnumerable<Warning>?  Warnings      = null,
                      TimeSpan?              Runtime       = null)

                => new (AuthId,
                        PushDataResultTypes.AdminDown,
                        Array.Empty<PushSingleEVSEDataResult>(),
                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.AdminDown, Warnings)),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Success

        public static PushEVSEDataRecordResult

            Success(IId                    AuthId,
                    IEnumerable<EVSEDataRecord>     SuccessfulEVSEs,
                    String?                Description       = null,
                    IEnumerable<Warning>?  Warnings          = null,
                    TimeSpan?              Runtime           = null)

                => new (AuthId,
                        PushDataResultTypes.Success,
                        SuccessfulEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Success, Warnings)),
                        Array.Empty<PushSingleEVSEDataResult>(),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Enqueued

        public static PushEVSEDataRecordResult

            Enqueued(IId                    AuthId,
                     IEnumerable<EVSEDataRecord>     EnqueuedEVSEs,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

                => new (AuthId,
                        PushDataResultTypes.Enqueued,
                        EnqueuedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Enqueued, Warnings)),
                        Array.Empty<PushSingleEVSEDataResult>(),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation

        public static PushEVSEDataRecordResult

            NoOperation(IId                    AuthId,
                        IEnumerable<EVSEDataRecord>     RejectedEVSEs,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

                => new (AuthId,
                        PushDataResultTypes.NoOperation,
                        Array.Empty<PushSingleEVSEDataResult>(),
                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.NoOperation, Warnings)),
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout

        public static PushEVSEDataRecordResult

            LockTimeout(IId                    AuthId,
                        IEnumerable<EVSEDataRecord>     RejectedEVSEs,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)


                => new (AuthId,
                        PushDataResultTypes.LockTimeout,
                        Array.Empty<PushSingleEVSEDataResult>(),
                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Timeout, Warnings)),
                        Description,
                        Warnings,
                        Runtime: Runtime);

        #endregion

        #region (static) Timeout

        public static PushEVSEDataRecordResult

            Timeout(IId                    AuthId,
                    IEnumerable<EVSEDataRecord>     RejectedEVSEs,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)


                => new (AuthId,
                        PushDataResultTypes.Timeout,
                        Array.Empty<PushSingleEVSEDataResult>(),
                        RejectedEVSEs.Select(evse => new PushSingleEVSEDataResult(evse, PushSingleDataResultTypes.Timeout, Warnings)),
                        Description,
                        Warnings,
                        Runtime: Runtime);

        #endregion

        #region (static) Error

        public static PushEVSEDataRecordResult

            Error(IId                                     AuthId,
                  IEnumerable<PushSingleEVSEDataResult>?  RejectedEVSEs   = null,
                  String?                                 Description     = null,
                  IEnumerable<Warning>?                   Warnings        = null,
                  TimeSpan?                               Runtime         = null)

            => new (AuthId,
                    PushDataResultTypes.Error,
                    Array.Empty<PushSingleEVSEDataResult>(),
                    RejectedEVSEs,
                    Description,
                    Warnings,
                    Runtime);

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: " + Result + "; " + Description);

        #endregion

    }


    public static class PushDataResultTypesExtensions
    {

        public static CommandResult ToWWCP(this PushDataResultTypes PushDataResultType)

            => PushDataResultType switch {

                   PushDataResultTypes.AdminDown    => CommandResult.AdminDown,
                   PushDataResultTypes.NoOperation  => CommandResult.NoOperation,
                   PushDataResultTypes.Enqueued     => CommandResult.Enqueued,
                   PushDataResultTypes.Success      => CommandResult.Success,
                   PushDataResultTypes.OutOfService => CommandResult.OutOfService,
                   PushDataResultTypes.LockTimeout  => CommandResult.LockTimeout,
                   PushDataResultTypes.Timeout      => CommandResult.Timeout,
                   PushDataResultTypes.Error        => CommandResult.Error,
                   _                                => CommandResult.Unspecified

               };


    }


    public enum PushDataResultTypes
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The service was disabled by the administrator.
        /// </summary>
        AdminDown,

        /// <summary>
        /// No operation e.g. because no EVSE data passed the EVSE filter.
        /// </summary>
        NoOperation,

        /// <summary>
        /// The data has been enqueued for later transmission.
        /// </summary>
        Enqueued,

        /// <summary>
        /// Success.
        /// </summary>
        Success,

        /// <summary>
        /// Out-Of-Service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// A lock timeout occured.
        /// </summary>
        LockTimeout,

        /// <summary>
        /// A timeout occured.
        /// </summary>
        Timeout,

        /// <summary>
        /// The operation led to an error.
        /// </summary>
        Error

    }

    public enum PushSingleDataResultTypes
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The service was disabled by the administrator.
        /// </summary>
        AdminDown,

        /// <summary>
        /// No operation e.g. because no EVSE data passed the EVSE filter.
        /// </summary>
        NoOperation,

        /// <summary>
        /// The data has been enqueued for later transmission.
        /// </summary>
        Enqueued,

        /// <summary>
        /// Success.
        /// </summary>
        Success,

        /// <summary>
        /// Out-Of-Service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// A lock timeout occured.
        /// </summary>
        LockTimeout,

        /// <summary>
        /// A timeout occured.
        /// </summary>
        Timeout,

        /// <summary>
        /// Error.
        /// </summary>
        Error

    }

}
