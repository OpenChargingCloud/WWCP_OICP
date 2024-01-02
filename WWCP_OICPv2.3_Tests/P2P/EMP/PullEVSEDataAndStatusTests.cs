/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using NUnit.Framework;
using NUnit.Framework.Legacy;

using cloud.charging.open.protocols.OICPv2_3.EMP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.P2P.EMP
{

    /// <summary>
    /// P2P EMP requesting EVSE data and status tests.
    /// </summary>
    [TestFixture]
    public class PullEVSEDataAndStatusTests : AP2PTests
    {

        #region PullEVSEData_Test1()

        [Test]
        public async Task PullEVSEData_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new PullEVSEDataRequest(ProviderId:                             Provider_Id.Parse("DE-GDF"),
                                                  LastCall:                               null,
                                                  OperatorIdFilter:                       null,
                                                  CountryCodeFilter:                      null,
                                                  AccessibilityFilter:                    null,
                                                  AuthenticationModeFilter:               null,
                                                  CalibrationLawDataAvailabilityFilter:   null,
                                                  RenewableEnergyFilter:                  null,
                                                  IsHubjectCompatibleFilter:              null,
                                                  IsOpen24HoursFilter:                    null,

                                                  SearchCenter:                           null,
                                                  DistanceKM:                             null,
                                                  GeoCoordinatesResponseFormat:           GeoCoordinatesFormats.DecimalDegree,

                                                  Page:                                   null,
                                                  Size:                                   null,
                                                  SortOrder:                              null,
                                                  CustomData:                             null,

                                                  RequestTimeout:                         TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            if (empP2P_DEGDF.GetEMPClient(DEGEF_Id) is EMPClient empClient)
            {

                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEData.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEData.Requests_Error);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEData.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEData.Responses_Error);

                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEData.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEData.Requests_Error);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEData.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEData.Responses_Error);


                var oicpResult  = await empClient.PullEVSEData(request);

                ClassicAssert.IsNotNull(oicpResult);
                ClassicAssert.IsNotNull(oicpResult.Response);
                ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
                ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                ClassicAssert.IsNotNull(oicpResult.Response?.EVSEDataRecords);
                ClassicAssert.IsFalse  (oicpResult.Response?.EVSEDataRecords.Any());


                ClassicAssert.AreEqual(1, empClient.                Counters.PullEVSEData.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEData.Requests_Error);
                ClassicAssert.AreEqual(1, empClient.                Counters.PullEVSEData.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEData.Responses_Error);

                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEData.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEData.Requests_Error);
                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEData.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEData.Responses_Error);

            }
            else
                Assert.Fail("Missing EMPClient!");

        }

        #endregion


        #region PullEVSEStatus_Empty()

        [Test]
        public async Task PullEVSEStatus_Empty()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new PullEVSEStatusRequest(ProviderId:           Provider_Id.Parse("DE-GDF"),
                                                    SearchCenter:         null,
                                                    DistanceKM:           null,
                                                    EVSEStatusFilter:     EVSEStatusTypes.Reserved,

                                                    //Page:                 null,
                                                    //Size:                 null,
                                                    //SortOrder:            null,
                                                    //CustomStatus:         null,

                                                    RequestTimeout:       TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            if (empP2P_DEGDF.GetEMPClient(DEGEF_Id) is EMPClient empClient)
            {

                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatus.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatus.Requests_Error);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatus.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatus.Responses_Error);

                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatus.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatus.Requests_Error);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatus.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatus.Responses_Error);


                var oicpResult  = await empClient.PullEVSEStatus(request);

                ClassicAssert.IsNotNull(oicpResult);
                ClassicAssert.IsNotNull(oicpResult.Response);
                ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
                ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                ClassicAssert.IsNotNull(oicpResult.Response?.OperatorEVSEStatus);
                ClassicAssert.IsFalse  (oicpResult.Response?.OperatorEVSEStatus.Any());


                ClassicAssert.AreEqual(1, empClient.                Counters.PullEVSEStatus.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatus.Requests_Error);
                ClassicAssert.AreEqual(1, empClient.                Counters.PullEVSEStatus.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatus.Responses_Error);

                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatus.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatus.Requests_Error);
                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatus.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatus.Responses_Error);

            }
            else
                Assert.Fail("Missing EMPClient!");

        }

        #endregion

        #region PullEVSEStatus_Test1()

        [Test]
        public async Task PullEVSEStatus_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new PullEVSEStatusRequest(ProviderId:           Provider_Id.Parse("DE-GDF"),
                                                    SearchCenter:         null,
                                                    DistanceKM:           null,
                                                    EVSEStatusFilter:     null,

                                                    //Page:                 null,
                                                    //Size:                 null,
                                                    //SortOrder:            null,
                                                    //CustomStatus:         null,

                                                    RequestTimeout:       TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            if (empP2P_DEGDF.GetEMPClient(DEGEF_Id) is EMPClient empClient)
            {

                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatus.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatus.Requests_Error);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatus.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatus.Responses_Error);

                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatus.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatus.Requests_Error);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatus.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatus.Responses_Error);


                var oicpResult  = await empClient.PullEVSEStatus(request);

                ClassicAssert.IsNotNull(oicpResult);
                ClassicAssert.IsNotNull(oicpResult.Response);
                ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
                ClassicAssert.AreEqual (StatusCodes.Success,                    oicpResult.Response?.StatusCode?.Code);
                ClassicAssert.IsNotNull(oicpResult.Response?.OperatorEVSEStatus);
                ClassicAssert.AreEqual (1,                                      oicpResult.Response?.OperatorEVSEStatus.Count());

                var operatorEVSEStatus  = oicpResult.Response?.OperatorEVSEStatus.FirstOrDefault();
                ClassicAssert.IsNotNull(operatorEVSEStatus);
                ClassicAssert.AreEqual ("GraphDefined",                         operatorEVSEStatus.OperatorName);
                ClassicAssert.AreEqual (2,                                      operatorEVSEStatus.EVSEStatusRecords.Count());

                var operatorEVSEStatus1 = operatorEVSEStatus.EVSEStatusRecords.ElementAt(0);
                ClassicAssert.AreEqual (EVSE_Id.Parse("DE*GEF*E1234567*A*1"),   operatorEVSEStatus1.Id);
                ClassicAssert.AreEqual (EVSEStatusTypes.Available,              operatorEVSEStatus1.Status);

                var operatorEVSEStatus2 = operatorEVSEStatus.EVSEStatusRecords.ElementAt(1);
                ClassicAssert.AreEqual (EVSE_Id.Parse("DE*GEF*E1234567*A*2"),   operatorEVSEStatus2.Id);
                ClassicAssert.AreEqual (EVSEStatusTypes.Occupied,               operatorEVSEStatus2.Status);


                ClassicAssert.AreEqual(1, empClient.                Counters.PullEVSEStatus.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatus.Requests_Error);
                ClassicAssert.AreEqual(1, empClient.                Counters.PullEVSEStatus.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatus.Responses_Error);

                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatus.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatus.Requests_Error);
                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatus.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatus.Responses_Error);

            }
            else
                Assert.Fail("Missing EMPClient!");

        }

        #endregion


        #region PullEVSEStatusById_Empty()

        [Test]
        public async Task PullEVSEStatusById_Empty()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new PullEVSEStatusByIdRequest(ProviderId:             Provider_Id.Parse("DE-GDF"),
                                                        EVSEIds:                new EVSE_Id[] {
                                                                                    EVSE_Id.Parse("DE*XXX*E1234567*A*X")
                                                                                },
                                                        RequestTimeout:         TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            if (empP2P_DEGDF.GetEMPClient(DEGEF_Id) is EMPClient empClient)
            {

                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatusById.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatusById.Requests_Error);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatusById.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatusById.Responses_Error);

                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatusById.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatusById.Requests_Error);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatusById.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatusById.Responses_Error);


                var oicpResult  = await empClient.PullEVSEStatusById(request);

                ClassicAssert.IsNotNull(oicpResult);
                ClassicAssert.IsNotNull(oicpResult.Response);
                ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
                ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                ClassicAssert.IsNotNull(oicpResult.Response?.EVSEStatusRecords);
                ClassicAssert.IsFalse  (oicpResult.Response?.EVSEStatusRecords.Any());


                ClassicAssert.AreEqual(1, empClient.                Counters.PullEVSEStatusById.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatusById.Requests_Error);
                ClassicAssert.AreEqual(1, empClient.                Counters.PullEVSEStatusById.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatusById.Responses_Error);

                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatusById.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatusById.Requests_Error);
                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatusById.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatusById.Responses_Error);

            }
            else
                Assert.Fail("Missing EMPClient!");

        }

        #endregion

        #region PullEVSEStatusById_Test1()

        [Test]
        public async Task PullEVSEStatusById_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new PullEVSEStatusByIdRequest(ProviderId:             Provider_Id.Parse("DE-GDF"),
                                                        EVSEIds:                new EVSE_Id[] {
                                                                                    EVSE_Id.Parse("DE*GEF*E1234567*A*1")
                                                                                },
                                                        RequestTimeout:         TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            if (empP2P_DEGDF.GetEMPClient(DEGEF_Id) is EMPClient empClient)
            {

                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatusById.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatusById.Requests_Error);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatusById.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatusById.Responses_Error);

                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatusById.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatusById.Requests_Error);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatusById.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatusById.Responses_Error);


                var oicpResult  = await empClient.PullEVSEStatusById(request);

                ClassicAssert.IsNotNull(oicpResult);
                ClassicAssert.IsNotNull(oicpResult.Response);
                ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
                ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                ClassicAssert.IsNotNull(oicpResult.Response?.EVSEStatusRecords);
                ClassicAssert.IsTrue   (oicpResult.Response?.EVSEStatusRecords.Any());

                var evseStatusRecord1 = oicpResult.Response?.EVSEStatusRecords.ElementAt(0);
                ClassicAssert.AreEqual (EVSE_Id.Parse("DE*GEF*E1234567*A*1"),   evseStatusRecord1.Value.Id);
                ClassicAssert.AreEqual (EVSEStatusTypes.Available,              evseStatusRecord1.Value.Status);


                ClassicAssert.AreEqual(1, empClient.                Counters.PullEVSEStatusById.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatusById.Requests_Error);
                ClassicAssert.AreEqual(1, empClient.                Counters.PullEVSEStatusById.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEStatusById.Responses_Error);

                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatusById.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatusById.Requests_Error);
                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatusById.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEStatusById.Responses_Error);

            }
            else
                Assert.Fail("Missing EMPClient!");

        }

        #endregion


    }

}
