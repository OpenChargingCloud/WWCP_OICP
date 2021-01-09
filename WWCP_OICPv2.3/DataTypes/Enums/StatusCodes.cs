/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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

using System;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extentions methods for status codes.
    /// </summary>
    public static class StatusCodesExtentions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text-representation of a status code.
        /// </summary>
        /// <param name="Text">A text-representation of a status code.</param>
        public static StatusCodes Parse(String Text)
        {

            if (TryParse(Text, out StatusCodes statusCode))
                return statusCode;

            throw new ArgumentException("Undefined status code '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text-representation of a status code.
        /// </summary>
        /// <param name="Text">A text-representation of a status code.</param>
        public static StatusCodes? TryParse(String Text)
        {

            if (TryParse(Text, out StatusCodes statusCode))
                return statusCode;

            return default;

        }

        #endregion

        #region TryParse(Text, out StatusCode)

        /// <summary>
        /// Parses the given text-representation of a status code.
        /// </summary>
        /// <param name="Text">A text-representation of a status code.</param>
        /// <param name="StatusCode">The parsed status code.</param>
        public static Boolean TryParse(String Text, out StatusCodes StatusCode)
        {

            if (Int32.TryParse(Text, out Int32 Integer))
            {
                StatusCode = (StatusCodes) Integer;
                return true;
            }

            StatusCode = default;
            return false;

        }

        #endregion

        #region AsString(this StatusCode)

        /// <summary>
        /// Return a text-representation of the given status code.
        /// </summary>
        /// <param name="StatusCode">A status code.</param>
        public static String AsString(this StatusCodes StatusCode)

            => ((Int32) StatusCode).ToString("D3");

        #endregion

    }

    /// <summary>
    /// Error and status codes.
    /// </summary>
    public enum StatusCodes
    {

        #region General codes

        /// <summary>
        /// Success.
        /// </summary>
        Success                                          = 000,

        /// <summary>
        /// System error.
        /// </summary>
        SystemError                                      = 021,

        /// <summary>
        /// Data error.
        /// </summary>
        DataError                                        = 022,

        #endregion

        #region Internal system codes

        /// <summary>
        /// Hubject system error.
        /// </summary>
        HubjectSystemError                               = 001,

        /// <summary>
        /// Hubject database error.
        /// </summary>
        HubjectDatabaseError                             = 002,

        /// <summary>
        /// Data transaction error.
        /// </summary>
        DataTransactionError                             = 009,

        /// <summary>
        /// Unauthorized Access.
        /// </summary>
        UnauthorizedAccess                               = 017,

        /// <summary>
        /// Inconsistent EvseID.
        /// </summary>
        InconsistentEVSEID                               = 018,

        /// <summary>
        /// Inconsistent EvcoID.
        /// </summary>
        InconsistentEVCOID                               = 019,

        #endregion

        #region Authentication codes

        /// <summary>
        /// QR Code Authentication failed – Invalid Credentials.
        /// </summary>
        QRCodeAuthenticationFailed_InvalidCredentials    = 101,

        /// <summary>
        /// RFID Authentication failed – invalid UID.
        /// </summary>
        RFIDAuthenticationfailed_InvalidUID              = 102,

        /// <summary>
        /// RFID Authentication failed – card not readable.
        /// </summary>
        RFIDAuthenticationfailed_CardNotReadable         = 103,

        /// <summary>
        /// PLC Authentication failed - invalid EvcoID.
        /// </summary>
        PLCAuthenticationfailed_InvalidEVCOID            = 105,

        /// <summary>
        /// No positive authentication response.
        /// </summary>
        NoPositiveAuthenticationResponse                 = 106,

        /// <summary>
        /// QR Code App Authentication failed – time out error.
        /// </summary>
        QRCodeAppAuthenticationFailed_TimeoutError       = 110,

        /// <summary>
        /// PLC (ISO/ IEC 15118) Authentication failed – invalid underlying EvcoID.
        /// </summary>
        PLCAuthenticationFailed_InvalidUnderlyingEVCOID  = 120,

        /// <summary>
        /// PLC (ISO/ IEC 15118) Authentication failed – invalid certificate.
        /// </summary>
        PLCAuthenticationFailed_InvalidCertificate       = 121,

        /// <summary>
        /// PLC (ISO/ IEC 15118) Authentication failed – time out error.
        /// </summary>
        PLCAuthenticationFailed_TimeoutError             = 122,

        /// <summary>
        /// EvcoID locked.
        /// </summary>
        EVCOIDLocked                                     = 200,

        #endregion

        #region Session codes

        /// <summary>
        /// No valid contract.
        /// </summary>
        NoValidContract                                  = 210,

        /// <summary>
        /// Partner not found.
        /// </summary>
        PartnerNotFound                                  = 300,

        /// <summary>
        /// Partner did not respond.
        /// </summary>
        PartnerDidNotRespond                             = 310,

        /// <summary>
        /// Service not available.
        /// </summary>
        ServiceNotAvailable                              = 320,

        /// <summary>
        /// Session is invalid.
        /// </summary>
        SessionIsInvalid                                 = 400,

        #endregion

        #region EVSE codes

        /// <summary>
        /// Communication to EVSE failed.
        /// </summary>
        CommunicationToEVSEFailed                        = 501,

        /// <summary>
        /// No EV connected to EVSE.
        /// </summary>
        NoEVConnectedToEVSE                              = 510,

        /// <summary>
        /// EVSE already reserved.
        /// </summary>
        EVSEAlreadyReserved                              = 601,

        /// <summary>
        /// EVSE already in use/ wrong token.
        /// </summary>
        EVSEAlreadyInUse_WrongToken                      = 602,

        /// <summary>
        /// Unknown EVSE ID.
        /// </summary>
        UnknownEVSEID                                    = 603,

        /// <summary>
        /// EVSE ID is not Hubject compatible.
        /// </summary>
        EVSEIDIsNotHubjectCompatible                     = 604,

        /// <summary>
        /// EVSE out of service.
        /// </summary>
        EVSEOutOfService                                 = 700

        #endregion

    }

}
