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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.EMP.client
{

    /// <summary>
    /// EMP requesting EVSE data and status tests.
    /// </summary>
    [TestFixture]
    public class PullEVSEDataAndStatusTests : AEMPClientAPITests
    {

        #region PullEVSEData_Test1()

        [Test]
        public async Task PullEVSEData_Test1()
        {

            if (empClientAPI is null ||
                empClient    is null)
            {
                Assert.Fail("empClientAPI or empClient is null!");
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

            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEData.Requests_OK);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEData.Requests_Error);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEData.Responses_OK);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEData.Responses_Error);

            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Requests_OK);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Requests_Error);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Responses_OK);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Responses_Error);

            var oicpResult  = await empClient.PullEVSEData(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsNotNull(oicpResult.Response);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            ClassicAssert.IsNotNull(oicpResult.Response?.EVSEDataRecords);
            ClassicAssert.IsFalse  (oicpResult.Response?.EVSEDataRecords.Any());

            ClassicAssert.AreEqual(1, empClient.   Counters.PullEVSEData.Requests_OK);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEData.Requests_Error);
            ClassicAssert.AreEqual(1, empClient.   Counters.PullEVSEData.Responses_OK);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEData.Responses_Error);

            ClassicAssert.AreEqual(1, empClientAPI.Counters.PullEVSEData.Requests_OK);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Requests_Error);
            ClassicAssert.AreEqual(1, empClientAPI.Counters.PullEVSEData.Responses_OK);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Responses_Error);

        }

        #endregion

        #region PullEVSEStatus_Test1()

        [Test]
        public async Task PullEVSEStatus_Test1()
        {

            if (empClientAPI is null ||
                empClient    is null)
            {
                Assert.Fail("empClientAPI or empClient is null!");
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

            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatus.Requests_OK);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatus.Requests_Error);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatus.Responses_OK);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatus.Responses_Error);

            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatus.Requests_OK);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatus.Requests_Error);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatus.Responses_OK);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatus.Responses_Error);

            var oicpResult  = await empClient.PullEVSEStatus(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsNotNull(oicpResult.Response);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            ClassicAssert.IsNotNull(oicpResult.Response?.OperatorEVSEStatus);
            ClassicAssert.IsFalse  (oicpResult.Response?.OperatorEVSEStatus.Any());

            ClassicAssert.AreEqual(1, empClient.   Counters.PullEVSEStatus.Requests_OK);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatus.Requests_Error);
            ClassicAssert.AreEqual(1, empClient.   Counters.PullEVSEStatus.Responses_OK);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatus.Responses_Error);

            ClassicAssert.AreEqual(1, empClientAPI.Counters.PullEVSEStatus.Requests_OK);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatus.Requests_Error);
            ClassicAssert.AreEqual(1, empClientAPI.Counters.PullEVSEStatus.Responses_OK);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatus.Responses_Error);

        }

        #endregion

        #region PullEVSEStatusById_Test1()

        [Test]
        public async Task PullEVSEStatusById_Test1()
        {

            if (empClientAPI is null ||
                empClient    is null)
            {
                Assert.Fail("empClientAPI or empClient is null!");
                return;
            }

            var request = new PullEVSEStatusByIdRequest(ProviderId:      Provider_Id.Parse("DE-GDF"),
                                                        EVSEIds:         new EVSE_Id[] {
                                                                             EVSE_Id.Parse("DE*GEF*E1234567*A*1")
                                                                         },
                                                        RequestTimeout:  TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatusById.Requests_OK);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatusById.Requests_Error);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatusById.Responses_OK);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatusById.Responses_Error);

            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusById.Requests_OK);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusById.Requests_Error);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusById.Responses_OK);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusById.Responses_Error);

            var oicpResult  = await empClient.PullEVSEStatusById(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsNotNull(oicpResult.Response);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            ClassicAssert.IsNotNull(oicpResult.Response?.EVSEStatusRecords);
            ClassicAssert.IsFalse  (oicpResult.Response?.EVSEStatusRecords.Any());

            ClassicAssert.AreEqual(1, empClient.   Counters.PullEVSEStatusById.Requests_OK);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatusById.Requests_Error);
            ClassicAssert.AreEqual(1, empClient.   Counters.PullEVSEStatusById.Responses_OK);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatusById.Responses_Error);

            ClassicAssert.AreEqual(1, empClientAPI.Counters.PullEVSEStatusById.Requests_OK);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusById.Requests_Error);
            ClassicAssert.AreEqual(1, empClientAPI.Counters.PullEVSEStatusById.Responses_OK);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusById.Responses_Error);

        }

        #endregion

        #region PullEVSEStatusByOperatorId_Test1()

        [Test]
        public async Task PullEVSEStatusByOperatorId_Test1()
        {

            if (empClientAPI is null ||
                empClient    is null)
            {
                Assert.Fail("empClientAPI or empClient is null!");
                return;
            }

            var request = new PullEVSEStatusByOperatorIdRequest(ProviderId:      Provider_Id.Parse("DE-GDF"),
                                                                OperatorIds:     new Operator_Id[] {
                                                                                     Operator_Id.Parse("DE*GEF")
                                                                                 },
                                                                RequestTimeout:  TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatusByOperatorId.Requests_OK);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatusByOperatorId.Requests_Error);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatusByOperatorId.Responses_OK);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatusByOperatorId.Responses_Error);

            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusByOperatorId.Requests_OK);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusByOperatorId.Requests_Error);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusByOperatorId.Responses_OK);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusByOperatorId.Responses_Error);

            var oicpResult  = await empClient.PullEVSEStatusByOperatorId(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsNotNull(oicpResult.Response);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            ClassicAssert.IsNotNull(oicpResult.Response?.OperatorEVSEStatus);
            ClassicAssert.IsFalse  (oicpResult.Response?.OperatorEVSEStatus.Any());

            ClassicAssert.AreEqual(1, empClient.   Counters.PullEVSEStatusByOperatorId.Requests_OK);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatusByOperatorId.Requests_Error);
            ClassicAssert.AreEqual(1, empClient.   Counters.PullEVSEStatusByOperatorId.Responses_OK);
            ClassicAssert.AreEqual(0, empClient.   Counters.PullEVSEStatusByOperatorId.Responses_Error);

            ClassicAssert.AreEqual(1, empClientAPI.Counters.PullEVSEStatusByOperatorId.Requests_OK);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusByOperatorId.Requests_Error);
            ClassicAssert.AreEqual(1, empClientAPI.Counters.PullEVSEStatusByOperatorId.Responses_OK);
            ClassicAssert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusByOperatorId.Responses_Error);

        }

        #endregion

    }

}
