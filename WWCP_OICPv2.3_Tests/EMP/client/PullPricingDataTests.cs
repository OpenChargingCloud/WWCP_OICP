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

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.EMP.client
{

    /// <summary>
    /// EMP requesting pricing product data and EVSE pricing tests.
    /// </summary>
    [TestFixture]
    public class PullPricingDataTests : AEMPClientAPITests
    {

        #region PullPricingProductData_Test_Empty()

        [Test]
        public async Task PullPricingProductData_Test_Empty()
        {

            if (empClientAPI is null ||
                empClient    is null)
            {
                Assert.Fail("empClientAPI or empClient is null!");
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

                                                            Timestamp:          Timestamp.Now,
                                                            CancellationToken:  null,
                                                            EventTrackingId:    EventTracking_Id.New,
                                                            RequestTimeout:     TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empClient.   Counters.PullPricingProductData.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PullPricingProductData.Requests_Error);
            Assert.AreEqual(0, empClient.   Counters.PullPricingProductData.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PullPricingProductData.Responses_Error);

            Assert.AreEqual(0, empClientAPI.Counters.PullPricingProductData.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullPricingProductData.Requests_Error);
            Assert.AreEqual(0, empClientAPI.Counters.PullPricingProductData.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullPricingProductData.Responses_Error);

            var oicpResult  = await empClient.PullPricingProductData(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsNotNull(oicpResult.Response?.PricingProductData);
            Assert.IsFalse  (oicpResult.Response?.PricingProductData.Any());

            Assert.AreEqual(1, empClient.   Counters.PullPricingProductData.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PullPricingProductData.Requests_Error);
            Assert.AreEqual(1, empClient.   Counters.PullPricingProductData.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PullPricingProductData.Responses_Error);

            Assert.AreEqual(1, empClientAPI.Counters.PullPricingProductData.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullPricingProductData.Requests_Error);
            Assert.AreEqual(1, empClientAPI.Counters.PullPricingProductData.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullPricingProductData.Responses_Error);

        }

        #endregion

        #region PullPricingProductData_Test1()

        [Test]
        public async Task PullPricingProductData_Test1()
        {

            if (empClientAPI is null ||
                empClient    is null)
            {
                Assert.Fail("empClientAPI or empClient is null!");
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

                                                            Timestamp:          Timestamp.Now,
                                                            CancellationToken:  null,
                                                            EventTrackingId:    EventTracking_Id.New,
                                                            RequestTimeout:     TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empClient.   Counters.PullPricingProductData.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PullPricingProductData.Requests_Error);
            Assert.AreEqual(0, empClient.   Counters.PullPricingProductData.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PullPricingProductData.Responses_Error);

            Assert.AreEqual(0, empClientAPI.Counters.PullPricingProductData.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullPricingProductData.Requests_Error);
            Assert.AreEqual(0, empClientAPI.Counters.PullPricingProductData.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullPricingProductData.Responses_Error);

            var oicpResult                = await empClient.PullPricingProductData(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsNotNull(oicpResult.Response?.PricingProductData);
            Assert.AreEqual (1, oicpResult.Response?.PricingProductData.Count());

            var pricingProductData        = oicpResult.Response?.PricingProductData.FirstOrDefault();
            Assert.IsNotNull(pricingProductData);
            Assert.AreEqual (Operator_Id.Parse("DE*GEF"),      pricingProductData?.OperatorId);
            Assert.AreEqual ("GraphDefined",                   pricingProductData?.OperatorName);
            Assert.AreEqual (1.23M,                            pricingProductData?.PricingDefaultPrice);
            Assert.AreEqual (Currency_Id.EUR,                  pricingProductData?.PricingDefaultPriceCurrency);
            Assert.AreEqual (Reference_Unit.HOUR,              pricingProductData?.PricingDefaultReferenceUnit);
            Assert.AreEqual (1,                                pricingProductData?.PricingProductDataRecords.Count());

            var pricingProductDataRecord  = pricingProductData?.PricingProductDataRecords.FirstOrDefault();
            Assert.IsNotNull(pricingProductDataRecord);
            Assert.AreEqual (PartnerProduct_Id.Parse("AC1"),   pricingProductDataRecord?.ProductId);
            Assert.AreEqual (Reference_Unit.HOUR,              pricingProductDataRecord?.ReferenceUnit);
            Assert.AreEqual (Currency_Id.EUR,                  pricingProductDataRecord?.ProductPriceCurrency);
            Assert.AreEqual (1,                                pricingProductDataRecord?.PricePerReferenceUnit);
            Assert.AreEqual (22,                               pricingProductDataRecord?.MaximumProductChargingPower);
            Assert.AreEqual (false,                            pricingProductDataRecord?.IsValid24hours);
            Assert.AreEqual (1,                                pricingProductDataRecord?.ProductAvailabilityTimes.Count());
            Assert.AreEqual (1,                                pricingProductDataRecord?.AdditionalReferences?.   Count());

            var productAvailabilityTime   = pricingProductDataRecord?.ProductAvailabilityTimes.FirstOrDefault();
            Assert.IsNotNull(productAvailabilityTime);
            Assert.AreEqual (09,                               productAvailabilityTime?.Period.Begin.Hour);
            Assert.AreEqual (00,                               productAvailabilityTime?.Period.Begin.Minute);
            Assert.AreEqual (18,                               productAvailabilityTime?.Period.End.  Hour);
            Assert.AreEqual (00,                               productAvailabilityTime?.Period.End.  Minute);
            Assert.AreEqual (WeekDay.Everyday,                 productAvailabilityTime?.On);

            var additionalReference       = pricingProductDataRecord?.AdditionalReferences?.FirstOrDefault();
            Assert.IsNotNull(additionalReference);
            Assert.AreEqual (Additional_Reference.ParkingFee,  additionalReference?.AdditionalReference);
            Assert.AreEqual (Reference_Unit.HOUR,              additionalReference?.AdditionalReferenceUnit);
            Assert.AreEqual (2,                                additionalReference?.PricePerAdditionalReferenceUnit);

            Assert.AreEqual(1, empClient.   Counters.PullPricingProductData.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PullPricingProductData.Requests_Error);
            Assert.AreEqual(1, empClient.   Counters.PullPricingProductData.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PullPricingProductData.Responses_Error);

            Assert.AreEqual(1, empClientAPI.Counters.PullPricingProductData.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullPricingProductData.Requests_Error);
            Assert.AreEqual(1, empClientAPI.Counters.PullPricingProductData.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullPricingProductData.Responses_Error);

        }

        #endregion


        #region PullEVSEPricing_Test_Empty()

        [Test]
        public async Task PullEVSEPricing_Test_Empty()
        {

            if (empClientAPI is null ||
                empClient    is null)
            {
                Assert.Fail("empClientAPI or empClient is null!");
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

                                                     Timestamp:            Timestamp.Now,
                                                     CancellationToken:    null,
                                                     EventTrackingId:      EventTracking_Id.New,
                                                     RequestTimeout:       TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empClient.   Counters.PullEVSEPricing.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEPricing.Requests_Error);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEPricing.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEPricing.Responses_Error);

            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEPricing.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEPricing.Requests_Error);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEPricing.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEPricing.Responses_Error);

            var oicpResult  = await empClient.PullEVSEPricing(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsNotNull(oicpResult.Response?.OperatorEVSEPricings);
            Assert.IsFalse  (oicpResult.Response?.OperatorEVSEPricings.Any());

            Assert.AreEqual(1, empClient.   Counters.PullEVSEPricing.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEPricing.Requests_Error);
            Assert.AreEqual(1, empClient.   Counters.PullEVSEPricing.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEPricing.Responses_Error);

            Assert.AreEqual(1, empClientAPI.Counters.PullEVSEPricing.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEPricing.Requests_Error);
            Assert.AreEqual(1, empClientAPI.Counters.PullEVSEPricing.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEPricing.Responses_Error);

        }

        #endregion

        #region PullEVSEPricing_Test1()

        [Test]
        public async Task PullEVSEPricing_Test1()
        {

            if (empClientAPI is null ||
                empClient    is null)
            {
                Assert.Fail("empClientAPI or empClient is null!");
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

                                                     Timestamp:            Timestamp.Now,
                                                     CancellationToken:    null,
                                                     EventTrackingId:      EventTracking_Id.New,
                                                     RequestTimeout:       TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empClient.   Counters.PullEVSEPricing.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEPricing.Requests_Error);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEPricing.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEPricing.Responses_Error);

            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEPricing.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEPricing.Requests_Error);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEPricing.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEPricing.Responses_Error);

            var oicpResult  = await empClient.PullEVSEPricing(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsNotNull(oicpResult.Response?.OperatorEVSEPricings);
            Assert.AreEqual (1, oicpResult.Response?.OperatorEVSEPricings.Count());

            var operatorEVSEPricing  = oicpResult.Response?.OperatorEVSEPricings.FirstOrDefault();
            Assert.IsNotNull(operatorEVSEPricing);
            Assert.AreEqual (Operator_Id.Parse("DE*GEF"),         operatorEVSEPricing?.OperatorId);
            Assert.AreEqual ("GraphDefined",                      operatorEVSEPricing?.OperatorName);
            Assert.AreEqual (2,                                   operatorEVSEPricing?.EVSEPricings.Count());

            var evsePricing1         = operatorEVSEPricing?.EVSEPricings.FirstOrDefault();
            Assert.IsNotNull(evsePricing1);
            Assert.AreEqual (EVSE_Id.Parse("DE*GEF*E1234567*A*1"),  evsePricing1?.EVSEId);
            Assert.AreEqual (Provider_Id.Parse("DE-GDF"),         evsePricing1?.ProviderId);
            Assert.AreEqual (1,                                   evsePricing1?.EVSEIdProductList.Count());
            Assert.AreEqual (PartnerProduct_Id.Parse("AC1"),      evsePricing1?.EVSEIdProductList.FirstOrDefault());

            var evsePricing2         = operatorEVSEPricing?.EVSEPricings.Skip(1).FirstOrDefault();
            Assert.IsNotNull(evsePricing1);
            Assert.AreEqual (EVSE_Id.Parse("DE*GEF*E1234567*A*2"),  evsePricing2?.EVSEId);
            Assert.IsNull   (                                     evsePricing2?.ProviderId);
            Assert.AreEqual (1,                                   evsePricing2?.EVSEIdProductList.Count());
            Assert.AreEqual (PartnerProduct_Id.Parse("AC3"),      evsePricing2?.EVSEIdProductList.FirstOrDefault());

            Assert.AreEqual(1, empClient.   Counters.PullEVSEPricing.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEPricing.Requests_Error);
            Assert.AreEqual(1, empClient.   Counters.PullEVSEPricing.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PullEVSEPricing.Responses_Error);

            Assert.AreEqual(1, empClientAPI.Counters.PullEVSEPricing.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEPricing.Requests_Error);
            Assert.AreEqual(1, empClientAPI.Counters.PullEVSEPricing.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PullEVSEPricing.Responses_Error);

        }

        #endregion

    }

}
