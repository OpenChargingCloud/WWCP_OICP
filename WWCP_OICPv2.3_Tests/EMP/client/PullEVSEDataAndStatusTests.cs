﻿/*
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

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;

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

                                                  Timestamp:                              Timestamp.Now,
                                                  CancellationToken:                      null,
                                                  EventTrackingId:                        EventTracking_Id.New,
                                                  RequestTimeout:                         TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empClient.   Counters.PullEVSEData.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEData.Requests_Error);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEData.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEData.Responses_Error);

            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Requests_Error);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Responses_Error);

            var oicpResult  = await empClient.PullEVSEData(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsNotNull(oicpResult.Response?.EVSEDataRecords);
            Assert.IsFalse  (oicpResult.Response?.EVSEDataRecords.Any());

            Assert.AreEqual(1, empClient.   Counters.PullEVSEData.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEData.Requests_Error);
            Assert.AreEqual(1, empClient.   Counters.PullEVSEData.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEData.Responses_Error);

            Assert.AreEqual(1, empClientAPI.Counters.PullEVSEData.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Requests_Error);
            Assert.AreEqual(1, empClientAPI.Counters.PullEVSEData.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Responses_Error);

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

                                                    Timestamp:            Timestamp.Now,
                                                    CancellationToken:    null,
                                                    EventTrackingId:      EventTracking_Id.New,
                                                    RequestTimeout:       TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatus.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatus.Requests_Error);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatus.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatus.Responses_Error);

            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatus.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatus.Requests_Error);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatus.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatus.Responses_Error);

            var oicpResult  = await empClient.PullEVSEStatus(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsNotNull(oicpResult.Response?.OperatorEVSEStatus);
            Assert.IsFalse  (oicpResult.Response?.OperatorEVSEStatus.Any());

            Assert.AreEqual(1, empClient.   Counters.PullEVSEStatus.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatus.Requests_Error);
            Assert.AreEqual(1, empClient.   Counters.PullEVSEStatus.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatus.Responses_Error);

            Assert.AreEqual(1, empClientAPI.Counters.PullEVSEStatus.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatus.Requests_Error);
            Assert.AreEqual(1, empClientAPI.Counters.PullEVSEStatus.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatus.Responses_Error);

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

            var request = new PullEVSEStatusByIdRequest(ProviderId:             Provider_Id.Parse("DE-GDF"),
                                                        EVSEIds:                new EVSE_Id[] {
                                                                                    EVSE_Id.Parse("DE*GEF*E1234567*A*1")
                                                                                },

                                                        Timestamp:              Timestamp.Now,
                                                        CancellationToken:      null,
                                                        EventTrackingId:        EventTracking_Id.New,
                                                        RequestTimeout:         TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatusById.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatusById.Requests_Error);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatusById.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatusById.Responses_Error);

            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusById.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusById.Requests_Error);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusById.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusById.Responses_Error);

            var oicpResult  = await empClient.PullEVSEStatusById(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsNotNull(oicpResult.Response?.EVSEStatusRecords);
            Assert.IsFalse  (oicpResult.Response?.EVSEStatusRecords.Any());

            Assert.AreEqual(1, empClient.   Counters.PullEVSEStatusById.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatusById.Requests_Error);
            Assert.AreEqual(1, empClient.   Counters.PullEVSEStatusById.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatusById.Responses_Error);

            Assert.AreEqual(1, empClientAPI.Counters.PullEVSEStatusById.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusById.Requests_Error);
            Assert.AreEqual(1, empClientAPI.Counters.PullEVSEStatusById.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusById.Responses_Error);

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

            var request = new PullEVSEStatusByOperatorIdRequest(ProviderId:             Provider_Id.Parse("DE-GDF"),
                                                                OperatorIds:            new Operator_Id[] {
                                                                                            Operator_Id.Parse("DE*GEF")
                                                                                        },

                                                                Timestamp:              Timestamp.Now,
                                                                CancellationToken:      null,
                                                                EventTrackingId:        EventTracking_Id.New,
                                                                RequestTimeout:         TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatusByOperatorId.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatusByOperatorId.Requests_Error);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatusByOperatorId.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatusByOperatorId.Responses_Error);

            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusByOperatorId.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusByOperatorId.Requests_Error);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusByOperatorId.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusByOperatorId.Responses_Error);

            var oicpResult  = await empClient.PullEVSEStatusByOperatorId(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsNotNull(oicpResult.Response?.OperatorEVSEStatus);
            Assert.IsFalse  (oicpResult.Response?.OperatorEVSEStatus.Any());

            Assert.AreEqual(1, empClient.   Counters.PullEVSEStatusByOperatorId.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatusByOperatorId.Requests_Error);
            Assert.AreEqual(1, empClient.   Counters.PullEVSEStatusByOperatorId.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEStatusByOperatorId.Responses_Error);

            Assert.AreEqual(1, empClientAPI.Counters.PullEVSEStatusByOperatorId.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusByOperatorId.Requests_Error);
            Assert.AreEqual(1, empClientAPI.Counters.PullEVSEStatusByOperatorId.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEStatusByOperatorId.Responses_Error);

        }

        #endregion

    }

}