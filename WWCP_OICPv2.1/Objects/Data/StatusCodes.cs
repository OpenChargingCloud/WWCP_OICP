/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// OICP status codes.
    /// </summary>
    public enum StatusCodes
    {

        Success                                          = 000,
        HubjectSystemError                               = 001,
        HubjectDatabaseError                             = 002,
        DataTransactionError                             = 009,
        UnauthorizedAccess                               = 017,
        InconsistentEVSEID                               = 018,
        InconsistentEVCOID                               = 019,
        SystemError                                      = 021,
        DataError                                        = 022,
        QRCodeAuthenticationFfailed_InvalidCredentials   = 101,
        RFIDAuthenticationfailed_InvalidUID              = 102,
        RFIDAuthenticationfailed_CardNotReadable         = 103,
        PLCAuthenticationfailed_InvalidEVCOID            = 105,
        NoPositiveAuthenticationResponse                 = 106,
        QRCodeAppAuthenticationFailed_TimeoutError       = 110,
        PLCAuthenticationFailed_InvalidUnderlyingEVCOID  = 120,
        PLCAuthenticationFailed_InvalidCertificate       = 121,
        PLCAuthenticationFailed_TimeoutError             = 122,
        EVCOIDLocked                                     = 200,
        NoValidContract                                  = 210,
        PartnerNotFound                                  = 300,
        PartnerDidNotRespond                             = 310,
        ServiceNotAvailable                              = 320,
        SessionIsInvalid                                 = 400,
        CommunicationToEVSEFailed                        = 501,
        NoEVConnectedToEVSE                              = 510,
        EVSEAlreadyReserved                              = 601,
        EVSEAlreadyInUse_WrongToken                      = 602,
        UnknownEVSEID                                    = 603,
        EVSEIDIsNotHubjectCompatible                     = 604,
        EVSEOutOfService                                 = 700

    }

}
