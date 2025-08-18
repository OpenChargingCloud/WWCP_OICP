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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// OICP extensions methods.
    /// </summary>
    public static class OICPExtensions
    {

        public const String  ProcessId   = "Process-ID";
        public const String  ProviderId  = "providerId";
        public const String  OperatorId  = "operatorId";


        public static Process_Id? TryParseProcessId(this HTTPRequest Request)

            => Request.TryParseHeaderField<Process_Id>(
                   ProcessId,
                   Process_Id.TryParse
               );


        public static Boolean TryParseProviderId(this HTTPRequest  Request,
                                                 out Provider_Id   ProviderId)

            => Request.TryParseURLParameter(
                   OICPExtensions.ProviderId,
                   Provider_Id.TryParse,
                   out ProviderId
               );

        public static Boolean TryParseOperatorId(this HTTPRequest  Request,
                                                 out Operator_Id   OperatorId)

            => Request.TryParseURLParameter(
                   OICPExtensions.OperatorId,
                   Operator_Id.TryParse,
                   out OperatorId
               );


    }

}
