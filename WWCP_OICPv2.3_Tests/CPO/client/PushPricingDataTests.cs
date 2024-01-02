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

namespace cloud.charging.open.protocols.OICPv2_3.tests.CPO.client
{

    /// <summary>
    /// CPO sending pricing data tests.
    /// </summary>
    [TestFixture]
    public class PushPricingDataTests : ACPOClientAPITests
    {

        #region PushPricingProductData_Test1()

        [Test]
        public async Task PushPricingProductData_Test1()
        {

            if (cpoClientAPI is null ||
                cpoClient    is null)
            {
                Assert.Fail("cpoClientAPI or cpoClient is null!");
                return;
            }

            var request = new PushPricingProductDataRequest(
                              new PricingProductData(
                                  OperatorId:                    Operator_Id.Parse("DE*GEF"),
                                  ProviderId:                    Provider_Id.Parse("DE-GDF"),
                                  PricingDefaultPrice:           1.23M,
                                  PricingDefaultPriceCurrency:   Currency_Id.EUR,
                                  PricingDefaultReferenceUnit:   Reference_Unit.HOUR,
                                  PricingProductDataRecords:     new PricingProductDataRecord[] {
                                                                     new PricingProductDataRecord(
                                                                         ProductId:                    PartnerProduct_Id.Parse("AC1"),
                                                                         ReferenceUnit:                Reference_Unit.HOUR,
                                                                         ProductPriceCurrency:         Currency_Id.EUR,
                                                                         PricePerReferenceUnit:        1,
                                                                         MaximumProductChargingPower:  22,
                                                                         IsValid24hours:               false,
                                                                         ProductAvailabilityTimes:     new ProductAvailabilityTimes[] {
                                                                                                           new ProductAvailabilityTimes(
                                                                                                               new Period(
                                                                                                                   Begin: HourMinute.Parse("09:00"),
                                                                                                                   End:   HourMinute.Parse("18:00")
                                                                                                               ),
                                                                                                               On: WeekDay.Everyday)
                                                                                                       },
                                                                         AdditionalReferences:         new AdditionalReferences[] {
                                                                                                           new AdditionalReferences(
                                                                                                               AdditionalReference:              Additional_Reference.ParkingFee,
                                                                                                               AdditionalReferenceUnit:          Reference_Unit.HOUR,
                                                                                                               PricePerAdditionalReferenceUnit:  2
                                                                                                           )
                                                                                                       }
                                                                     )
                                                                 },
                                  OperatorName:                  "GraphDefined",
                                  CustomData:                    null,
                                  InternalData:                  null
                              ),
                              ActionTypes.FullLoad
                          );

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushPricingProductData.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushPricingProductData.Requests_Error);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushPricingProductData.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushPricingProductData.Responses_Error);

            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushPricingProductData.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushPricingProductData.Requests_Error);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushPricingProductData.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushPricingProductData.Responses_Error);

            var oicpResult  = await cpoClient.PushPricingProductData(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsNotNull(oicpResult.Response);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            ClassicAssert.IsTrue   (oicpResult.Response?.Result);

            ClassicAssert.AreEqual(1, cpoClient.   Counters.PushPricingProductData.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushPricingProductData.Requests_Error);
            ClassicAssert.AreEqual(1, cpoClient.   Counters.PushPricingProductData.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushPricingProductData.Responses_Error);

            ClassicAssert.AreEqual(1, cpoClientAPI.Counters.PushPricingProductData.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushPricingProductData.Requests_Error);
            ClassicAssert.AreEqual(1, cpoClientAPI.Counters.PushPricingProductData.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushPricingProductData.Responses_Error);

        }

        #endregion


        #region PushEVSEPricing_Test1()

        [Test]
        public async Task PushEVSEPricing_Test1()
        {

            if (cpoClientAPI is null ||
                cpoClient    is null)
            {
                Assert.Fail("cpoClientAPI or cpoClient is null!");
                return;
            }

            var request = new PushEVSEPricingRequest(
                              OperatorId:    Operator_Id.Parse("DE*GEF"),
                              EVSEPricing:   new EVSEPricing[] {
                                                 new EVSEPricing(
                                                     EVSEId:             EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                                     EVSEIdProductList:  new PartnerProduct_Id[] {
                                                                             PartnerProduct_Id.Parse("AC1")
                                                                         },
                                                     ProviderId:         Provider_Id.Parse("DE-GDF")
                                                 ),
                                                 new EVSEPricing( // '*' meaning for all providers!
                                                     EVSEId:             EVSE_Id.Parse("DE*GEF*E1234567*A*2"),
                                                     EVSEIdProductList:  new PartnerProduct_Id[] {
                                                                             PartnerProduct_Id.Parse("AC3")
                                                                         }
                                                 )
                                             },
                              Action:        ActionTypes.FullLoad
                          );

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEPricing.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEPricing.Requests_Error);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEPricing.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEPricing.Responses_Error);

            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEPricing.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEPricing.Requests_Error);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEPricing.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEPricing.Responses_Error);

            var oicpResult  = await cpoClient.PushEVSEPricing(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsNotNull(oicpResult.Response);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            ClassicAssert.IsTrue   (oicpResult.Response?.Result);

            ClassicAssert.AreEqual(1, cpoClient.   Counters.PushEVSEPricing.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEPricing.Requests_Error);
            ClassicAssert.AreEqual(1, cpoClient.   Counters.PushEVSEPricing.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEPricing.Responses_Error);

            ClassicAssert.AreEqual(1, cpoClientAPI.Counters.PushEVSEPricing.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEPricing.Requests_Error);
            ClassicAssert.AreEqual(1, cpoClientAPI.Counters.PushEVSEPricing.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEPricing.Responses_Error);

        }

        #endregion

    }

}
