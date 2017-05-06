﻿/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.UnitTests
{

    public class MobileTests
    {

        #region TestMobileAuth(HubjectMobile, EVSEId, eMAIdWithPin)

        public async Task TestMobileAuth(MobileClient          HubjectMobile,
                                         EVSE_Id               EVSEId,
                                         QRCodeIdentification  QRCodeIdentification)
        {

            var MobileAuthorizationStart = await HubjectMobile.
                                   MobileAuthorizeStart(EVSEId,
                                                        QRCodeIdentification,
                                                        RequestTimeout: TimeSpan.FromSeconds(120));



            if (MobileAuthorizationStart.Content.AuthorizationStatus != AuthorizationStatusTypes.Authorized)
            {
                Console.WriteLine(MobileAuthorizationStart.Content.StatusCode.Value.Code);
                Console.WriteLine(MobileAuthorizationStart.Content.StatusCode.Value.Description);
                Console.WriteLine(MobileAuthorizationStart.Content.StatusCode.Value.AdditionalInfo);
            }

            else
            {

                Console.WriteLine("Ready to charge at charging station: " +
                                  MobileAuthorizationStart.Content.ChargingStationName[Languages.deu] +
                                  Environment.NewLine +
                                  MobileAuthorizationStart.Content.AdditionalInfo[Languages.deu]);

                var SessionId = MobileAuthorizationStart.Content.SessionId;


                Thread.Sleep(1000);


                var RemoteStart = await HubjectMobile.
                                            MobileRemoteStart(SessionId.Value,
                                                              RequestTimeout: TimeSpan.FromSeconds(240));

                if (!RemoteStart.Content.Result)
                {
                    Console.WriteLine(RemoteStart.Content.StatusCode.Code);
                    Console.WriteLine(RemoteStart.Content.StatusCode.Description);
                    Console.WriteLine(RemoteStart.Content.StatusCode.AdditionalInfo);
                }

                else
                {

                    Console.WriteLine("Charging session started!");

                    Thread.Sleep(2000);

                    var RemoteStop = await HubjectMobile.
                                               MobileRemoteStop(SessionId.Value,
                                                                RequestTimeout: TimeSpan.FromSeconds(120));


                    if (!RemoteStop.Content.Result)
                    {
                        Console.WriteLine(RemoteStop.Content.StatusCode.Code);
                        Console.WriteLine(RemoteStop.Content.StatusCode.Description);
                        Console.WriteLine(RemoteStop.Content.StatusCode.AdditionalInfo);
                    }

                    else
                    {
                        Console.WriteLine("Charging session stopped!");
                    }

                }

            }

        }

        #endregion

    }

}
