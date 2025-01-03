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

using NUnit.Framework;
using NUnit.Framework.Legacy;

using org.GraphDefined.Vanaheimr.Illias;

using cloud.charging.open.protocols.OICPv2_3.EMP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.P2P.EMP
{

    /// <summary>
    /// P2P EMP requesting pricing product data and EVSE pricing tests.
    /// </summary>
    [TestFixture]
    public class PullPricingDataTests : AP2PTests
    {

        #region PullPricingProductData_Test_Empty()

        [Test]
        public async Task PullPricingProductData_Test_Empty()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new PullPricingProductDataRequest(ProviderId:         Provider_Id.Parse("DE-GDF"),
                                                            OperatorIds:        new Operator_Id[] {
                                                                                    Operator_Id.Parse("DE*XXX")
                                                                                },
                                                            LastCall:           null,

                                                            Page:               null,
                                                            Size:               null,
                                                            SortOrder:          null,
                                                            CustomData:         null,

                                                            RequestTimeout:     TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            if (empP2P_DEGDF.GetEMPClient(DEGEF_Id) is EMPClient empClient)
            {

                ClassicAssert.AreEqual(0, empClient.                Counters.PullPricingProductData.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullPricingProductData.Requests_Error);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullPricingProductData.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullPricingProductData.Responses_Error);

                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullPricingProductData.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullPricingProductData.Requests_Error);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullPricingProductData.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullPricingProductData.Responses_Error);


                var oicpResult  = await empClient.PullPricingProductData(request);

                ClassicAssert.IsNotNull(oicpResult);
                ClassicAssert.IsNotNull(oicpResult.Response);
                ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
                ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                ClassicAssert.IsNotNull(oicpResult.Response?.PricingProductData);
                ClassicAssert.IsFalse  (oicpResult.Response?.PricingProductData.Any());


                ClassicAssert.AreEqual(1, empClient.                Counters.PullPricingProductData.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullPricingProductData.Requests_Error);
                ClassicAssert.AreEqual(1, empClient.                Counters.PullPricingProductData.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullPricingProductData.Responses_Error);

                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullPricingProductData.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullPricingProductData.Requests_Error);
                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullPricingProductData.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullPricingProductData.Responses_Error);

            }
            else
                Assert.Fail("Missing EMPClient!");

        }

        #endregion

        #region PullPricingProductData_Test1()

        [Test]
        public async Task PullPricingProductData_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new PullPricingProductDataRequest(ProviderId:         Provider_Id.Parse("DE-GDF"),
                                                            OperatorIds:        new Operator_Id[] {
                                                                                    Operator_Id.Parse("DE*GEF")
                                                                                },
                                                            LastCall:           null,

                                                            Page:               null,
                                                            Size:               null,
                                                            SortOrder:          null,
                                                            CustomData:         null,

                                                            RequestTimeout:     TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            if (empP2P_DEGDF.GetEMPClient(DEGEF_Id) is EMPClient empClient)
            {

                ClassicAssert.AreEqual(0, empClient.                Counters.PullPricingProductData.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullPricingProductData.Requests_Error);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullPricingProductData.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullPricingProductData.Responses_Error);

                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullPricingProductData.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullPricingProductData.Requests_Error);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullPricingProductData.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullPricingProductData.Responses_Error);


                var oicpResult                = await empClient.PullPricingProductData(request);

                ClassicAssert.IsNotNull(oicpResult);
                ClassicAssert.IsNotNull(oicpResult.Response);
                ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
                ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                ClassicAssert.IsNotNull(oicpResult.Response?.PricingProductData);
                ClassicAssert.AreEqual (1, oicpResult.Response?.PricingProductData.Count());

                var pricingProductData        = oicpResult.Response?.PricingProductData.FirstOrDefault();
                ClassicAssert.IsNotNull(pricingProductData);
                ClassicAssert.AreEqual (Operator_Id.Parse("DE*GEF"),      pricingProductData?.OperatorId);
                ClassicAssert.AreEqual ("GraphDefined",                   pricingProductData?.OperatorName);
                ClassicAssert.AreEqual (1.23M,                            pricingProductData?.PricingDefaultPrice);
                ClassicAssert.AreEqual (Currency_Id.EUR,                  pricingProductData?.PricingDefaultPriceCurrency);
                ClassicAssert.AreEqual (Reference_Unit.HOUR,              pricingProductData?.PricingDefaultReferenceUnit);
                ClassicAssert.AreEqual (1,                                pricingProductData?.PricingProductDataRecords.Count());

                var pricingProductDataRecord  = pricingProductData?.PricingProductDataRecords.FirstOrDefault();
                ClassicAssert.IsNotNull(pricingProductDataRecord);
                ClassicAssert.AreEqual (PartnerProduct_Id.Parse("AC1"),   pricingProductDataRecord?.ProductId);
                ClassicAssert.AreEqual (Reference_Unit.HOUR,              pricingProductDataRecord?.ReferenceUnit);
                ClassicAssert.AreEqual (Currency_Id.EUR,                  pricingProductDataRecord?.ProductPriceCurrency);
                ClassicAssert.AreEqual (1,                                pricingProductDataRecord?.PricePerReferenceUnit);
                ClassicAssert.AreEqual (22,                               pricingProductDataRecord?.MaximumProductChargingPower);
                ClassicAssert.AreEqual (false,                            pricingProductDataRecord?.IsValid24hours);
                ClassicAssert.AreEqual (1,                                pricingProductDataRecord?.ProductAvailabilityTimes.Count());
                ClassicAssert.AreEqual (1,                                pricingProductDataRecord?.AdditionalReferences?.   Count());

                var productAvailabilityTime   = pricingProductDataRecord?.ProductAvailabilityTimes.FirstOrDefault();
                ClassicAssert.IsNotNull(productAvailabilityTime);
                ClassicAssert.AreEqual (09,                               productAvailabilityTime?.Period.Begin.Hour);
                ClassicAssert.AreEqual (00,                               productAvailabilityTime?.Period.Begin.Minute);
                ClassicAssert.AreEqual (18,                               productAvailabilityTime?.Period.End.  Hour);
                ClassicAssert.AreEqual (00,                               productAvailabilityTime?.Period.End.  Minute);
                ClassicAssert.AreEqual (WeekDay.Everyday,                 productAvailabilityTime?.On);

                var additionalReference       = pricingProductDataRecord?.AdditionalReferences?.FirstOrDefault();
                ClassicAssert.IsNotNull(additionalReference);
                ClassicAssert.AreEqual (Additional_Reference.ParkingFee,  additionalReference?.AdditionalReference);
                ClassicAssert.AreEqual (Reference_Unit.HOUR,              additionalReference?.AdditionalReferenceUnit);
                ClassicAssert.AreEqual (2,                                additionalReference?.PricePerAdditionalReferenceUnit);


                ClassicAssert.AreEqual(1, empClient.                Counters.PullPricingProductData.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullPricingProductData.Requests_Error);
                ClassicAssert.AreEqual(1, empClient.                Counters.PullPricingProductData.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullPricingProductData.Responses_Error);

                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullPricingProductData.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullPricingProductData.Requests_Error);
                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullPricingProductData.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullPricingProductData.Responses_Error);

            }
            else
                Assert.Fail("Missing EMPClient!");

        }

        #endregion


        #region PullEVSEPricing_Test_Empty()

        [Test]
        public async Task PullEVSEPricing_Test_Empty()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new PullEVSEPricingRequest(ProviderId:           Provider_Id.Parse("DE-GDF"),
                                                     OperatorIds:          new Operator_Id[] {
                                                                               Operator_Id.Parse("DE*XXX")
                                                                           },
                                                     LastCall:             null,

                                                     //Page:                 null,
                                                     //Size:                 null,
                                                     //SortOrder:            null,
                                                     //CustomData:           null,

                                                     RequestTimeout:       TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            if (empP2P_DEGDF.GetEMPClient(DEGEF_Id) is EMPClient empClient)
            {

                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEPricing.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEPricing.Requests_Error);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEPricing.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEPricing.Responses_Error);

                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEPricing.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEPricing.Requests_Error);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEPricing.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEPricing.Responses_Error);


                var oicpResult  = await empClient.PullEVSEPricing(request);

                ClassicAssert.IsNotNull(oicpResult);
                ClassicAssert.IsNotNull(oicpResult.Response);
                ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
                ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                ClassicAssert.IsNotNull(oicpResult.Response?.OperatorEVSEPricings);
                ClassicAssert.IsFalse  (oicpResult.Response?.OperatorEVSEPricings.Any());


                ClassicAssert.AreEqual(1, empClient.                Counters.PullEVSEPricing.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEPricing.Requests_Error);
                ClassicAssert.AreEqual(1, empClient.                Counters.PullEVSEPricing.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEPricing.Responses_Error);

                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEPricing.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEPricing.Requests_Error);
                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEPricing.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEPricing.Responses_Error);

            }
            else
                Assert.Fail("Missing EMPClient!");

        }

        #endregion

        #region PullEVSEPricing_Test1()

        [Test]
        public async Task PullEVSEPricing_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new PullEVSEPricingRequest(ProviderId:           Provider_Id.Parse("DE-GDF"),
                                                     OperatorIds:          new Operator_Id[] {
                                                                               Operator_Id.Parse("DE*GEF")
                                                                           },
                                                     LastCall:             null,

                                                     //Page:                 null,
                                                     //Size:                 null,
                                                     //SortOrder:            null,
                                                     //CustomData:           null,

                                                     RequestTimeout:       TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            if (empP2P_DEGDF.GetEMPClient(DEGEF_Id) is EMPClient empClient)
            {

                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEPricing.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEPricing.Requests_Error);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEPricing.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEPricing.Responses_Error);

                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEPricing.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEPricing.Requests_Error);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEPricing.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEPricing.Responses_Error);


                var oicpResult  = await empClient.PullEVSEPricing(request);

                ClassicAssert.IsNotNull(oicpResult);
                ClassicAssert.IsNotNull(oicpResult.Response);
                ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
                ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                ClassicAssert.IsNotNull(oicpResult.Response?.OperatorEVSEPricings);
                ClassicAssert.AreEqual (1, oicpResult.Response?.OperatorEVSEPricings.Count());

                var operatorEVSEPricing  = oicpResult.Response?.OperatorEVSEPricings.FirstOrDefault();
                ClassicAssert.IsNotNull(operatorEVSEPricing);
                ClassicAssert.AreEqual (Operator_Id.Parse("DE*GEF"),           operatorEVSEPricing?.OperatorId);
                ClassicAssert.AreEqual ("GraphDefined",                        operatorEVSEPricing?.OperatorName);
                ClassicAssert.AreEqual (2,                                     operatorEVSEPricing?.EVSEPricings.Count());

                var evsePricing1         = operatorEVSEPricing?.EVSEPricings.FirstOrDefault();
                ClassicAssert.IsNotNull(evsePricing1);
                ClassicAssert.AreEqual (EVSE_Id.Parse("DE*GEF*E1234567*A*1"),  evsePricing1?.EVSEId);
                ClassicAssert.AreEqual (Provider_Id.Parse("DE-GDF"),           evsePricing1?.ProviderId);
                ClassicAssert.AreEqual (1,                                     evsePricing1?.EVSEIdProductList.Count());
                ClassicAssert.AreEqual (PartnerProduct_Id.AC1,                 evsePricing1?.EVSEIdProductList.FirstOrDefault());

                var evsePricing2         = operatorEVSEPricing?.EVSEPricings.Skip(1).FirstOrDefault();
                ClassicAssert.IsNotNull(evsePricing1);
                ClassicAssert.AreEqual (EVSE_Id.Parse("DE*GEF*E1234567*A*2"),  evsePricing2?.EVSEId);
                ClassicAssert.IsNull   (                                       evsePricing2?.ProviderId);
                ClassicAssert.AreEqual (1,                                     evsePricing2?.EVSEIdProductList.Count());
                ClassicAssert.AreEqual (PartnerProduct_Id.AC3,                 evsePricing2?.EVSEIdProductList.FirstOrDefault());


                ClassicAssert.AreEqual(1, empClient.                Counters.PullEVSEPricing.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEPricing.Requests_Error);
                ClassicAssert.AreEqual(1, empClient.                Counters.PullEVSEPricing.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PullEVSEPricing.Responses_Error);

                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEPricing.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEPricing.Requests_Error);
                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEPricing.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PullEVSEPricing.Responses_Error);

            }
            else
                Assert.Fail("Missing EMPClient!");

        }

        #endregion

    }

}
