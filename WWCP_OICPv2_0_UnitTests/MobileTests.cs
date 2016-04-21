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
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0.UnitTests
{

    public class MobileTests
    {

        #region TestMobileAuth(HubjectMobile, EVSEId, eMAIdWithPin)

        public async Task TestMobileAuth(MobileClient  HubjectMobile,
                                         EVSE_Id       EVSEId,
                                         eMAIdWithPIN  eMAIdWithPin)
        {

            var MobileAuthorizationStart = await HubjectMobile.
                                   MobileAuthorizeStart(EVSEId,
                                                        eMAIdWithPin,
                                                        QueryTimeout: TimeSpan.FromSeconds(120));



            if (MobileAuthorizationStart.Content.AuthorizationStatus != AuthorizationStatusType.Authorized)
            {
                Console.WriteLine(MobileAuthorizationStart.Content.StatusCode.Code);
                Console.WriteLine(MobileAuthorizationStart.Content.StatusCode.Description);
                Console.WriteLine(MobileAuthorizationStart.Content.StatusCode.AdditionalInfo);
            }

            else
            {

                Console.WriteLine("Ready to charge at charging station: " +
                                  MobileAuthorizationStart.Content.ChargingStationName[Languages.de] +
                                  Environment.NewLine +
                                  MobileAuthorizationStart.Content.AdditionalInfo[Languages.de]);

                var SessionId = MobileAuthorizationStart.Content.SessionId;


                Thread.Sleep(1000);


                var RemoteStart = await HubjectMobile.
                                            MobileRemoteStart(SessionId,
                                                              QueryTimeout: TimeSpan.FromSeconds(240));

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
                                               MobileRemoteStop(SessionId,
                                                                QueryTimeout: TimeSpan.FromSeconds(120));


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
