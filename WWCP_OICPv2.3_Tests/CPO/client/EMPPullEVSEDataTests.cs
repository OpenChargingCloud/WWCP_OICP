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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO.client.tests
{

    /// <summary>
    /// CPO sending EVSE data tests.
    /// </summary>
    [TestFixture]
    public class CPOPullEVSEDataTests : ACPOClientAPITests
    {

        //#region CPOPullEVSEData_Test1()

        //[Test]
        //public async Task CPOPullEVSEData_Test1()
        //{

        //    if (empClientAPI is null ||
        //        empClient    is null)
        //    {
        //        Assert.Fail("empClientAPI or empClient is null!");
        //        return;
        //    }

        //    var request = new PullEVSEDataRequest(ProviderId:                             Provider_Id.Parse("DE-GDF"),
        //                                          LastCall:                               null,
        //                                          OperatorIdFilter:                       null,
        //                                          CountryCodeFilter:                      null,
        //                                          AccessibilityFilter:                    null,
        //                                          AuthenticationModeFilter:               null,
        //                                          CalibrationLawDataAvailabilityFilter:   null,
        //                                          RenewableEnergyFilter:                  null,
        //                                          IsHubjectCompatibleFilter:              null,
        //                                          IsOpen24HoursFilter:                    null,

        //                                          SearchCenter:                           null,
        //                                          DistanceKM:                             null,
        //                                          GeoCoordinatesResponseFormat:           GeoCoordinatesFormats.DecimalDegree,

        //                                          Page:                                   null,
        //                                          Size:                                   null,
        //                                          SortOrder:                              null,
        //                                          CustomData:                             null,

        //                                          Timestamp:                              Timestamp.Now,
        //                                          CancellationToken:                      null,
        //                                          EventTrackingId:                        EventTracking_Id.New,
        //                                          RequestTimeout:                         TimeSpan.FromSeconds(10));

        //    Assert.IsNotNull(request);

        //    Assert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Requests_OK);
        //    Assert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Requests_Error);
        //    Assert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Responses_OK);
        //    Assert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Responses_Error);

        //    var oicpResult  = await empClient.PullEVSEData(request);

        //    Assert.IsNotNull(oicpResult);
        //    Assert.IsTrue   (oicpResult.WasSuccessful);
        //    Assert.AreEqual (StatusCodes.Success, oicpResult.Response.StatusCode.Code);
        //    Assert.IsNotNull(oicpResult.Response.EVSEDataRecords);
        //    Assert.IsFalse  (oicpResult.Response.EVSEDataRecords.Any());

        //    Assert.AreEqual(1, empClientAPI.Counters.PullEVSEData.Requests_OK);
        //    Assert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Requests_Error);
        //    Assert.AreEqual(1, empClientAPI.Counters.PullEVSEData.Responses_OK);
        //    Assert.AreEqual(0, empClientAPI.Counters.PullEVSEData.Responses_Error);

        //}

        //#endregion

    }

}
