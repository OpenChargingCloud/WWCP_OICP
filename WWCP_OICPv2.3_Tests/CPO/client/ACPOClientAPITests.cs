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

using System;

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO.client.tests
{

    /// <summary>
    /// OICP CPO Client API test defaults.
    /// </summary>
    public abstract class ACPOClientAPITests
    {

        #region Data

        protected CPOClientAPI?  cpoClientAPI;
        protected CPOClient?     cpoClient;

        #endregion


        #region SetupOnce()

        [OneTimeSetUp]
        public void SetupOnce()
        {

        }

        #endregion

        #region SetupEachTest()

        [SetUp]
        public void SetupEachTest()
        {

            Timestamp.Reset();

            cpoClientAPI = new CPOClientAPI(
                               ExternalDNSName:  "open.charging.cloud",
                               HTTPServerPort:   IPPort.Parse(8500),
                               LoggingPath:      "tests",
                               Autostart:        true
                           );

            Assert.IsNotNull(cpoClientAPI);

            //cpoClientAPI.OnPullEVSEData += (timestamp, sender, pullEVSEDataRequest) => {

            //    return Task.FromResult(
            //        OICPResult<PullEVSEDataResponse>.Success(
            //            pullEVSEDataRequest,
            //            new PullEVSEDataResponse(
            //                pullEVSEDataRequest,
            //                Timestamp.Now,
            //                pullEVSEDataRequest.EventTrackingId,
            //                Timestamp.Now - pullEVSEDataRequest.Timestamp,
            //                Array.Empty<EVSEDataRecord>(),
            //                StatusCode: new StatusCode(
            //                                StatusCodes.Success
            //                            )
            //            )
            //        )
            //    );

            //};



            cpoClient = new CPOClient(URL.Parse("http://127.0.0.1:9500"),
                                      RequestTimeout: TimeSpan.FromSeconds(10));

            Assert.IsNotNull(cpoClient);

        }


        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public void ShutdownEachTest()
        {
            cpoClientAPI?.Shutdown();
        }

        #endregion

        #region ShutdownOnce()

        [OneTimeTearDown]
        public void ShutdownOnce()
        {

        }

        #endregion



    }

}
