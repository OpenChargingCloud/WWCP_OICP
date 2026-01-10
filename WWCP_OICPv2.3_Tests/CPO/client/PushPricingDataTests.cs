/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.CPO.client
{

    /// <summary>
    /// CPO Sending PricingData tests.
    /// </summary>
    [TestFixture]
    public class PushPricingDataTests : ACPOClientAPITests
    {

        #region PushPricingProductData_Test1()

        [Test]
        public async Task PushPricingProductData_Test1()
        {

            if (cpoClientAPI is null)
            {
                Assert.Fail("cpoClientAPI must not be null!");
                return;
            }

            if (cpoClient is null)
            {
                Assert.Fail("cpoClient must not be null!");
                return;
            }

            var clientRequestLogging   = 0;
            var clientResponseLogging  = 0;
            var serverRequestLogging   = 0;
            var serverResponseLogging  = 0;

            var request = new PushPricingProductDataRequest(

                              new PricingProductData(

                                  OperatorId:                    Operator_Id.Parse("DE*GEF"),
                                  ProviderId:                    Provider_Id.Parse("DE-GDF"),
                                  PricingDefaultPrice:           1.23M,
                                  PricingDefaultPriceCurrency:   Currency_Id.EUR,
                                  PricingDefaultReferenceUnit:   Reference_Unit.HOUR,
                                  PricingProductDataRecords:     [
                                                                     new PricingProductDataRecord(
                                                                         ProductId:                    PartnerProduct_Id.Parse("AC1"),
                                                                         ReferenceUnit:                Reference_Unit.HOUR,
                                                                         ProductPriceCurrency:         Currency_Id.EUR,
                                                                         PricePerReferenceUnit:        1,
                                                                         MaximumProductChargingPower:  22,
                                                                         IsValid24hours:               false,
                                                                         ProductAvailabilityTimes:     [
                                                                                                           new ProductAvailabilityTimes(
                                                                                                               new Period(
                                                                                                                   Begin: HourMinute.Parse("09:00"),
                                                                                                                   End:   HourMinute.Parse("18:00")
                                                                                                               ),
                                                                                                               On: WeekDay.Everyday
                                                                                                           )
                                                                                                       ],
                                                                         AdditionalReferences:         [
                                                                                                           new AdditionalReferences(
                                                                                                               AdditionalReference:              Additional_Reference.ParkingFee,
                                                                                                               AdditionalReferenceUnit:          Reference_Unit.HOUR,
                                                                                                               PricePerAdditionalReferenceUnit:  2
                                                                                                           )
                                                                                                       ]
                                                                     )
                                                                 ],
                                  OperatorName:                  "GraphDefined",

                                  CustomData:                    new JObject(
                                                                     new JProperty("hello", "PricingProductData world!")
                                                                 )

                              ),
                              ActionTypes.FullLoad,

                              CustomData:  new JObject(
                                               new JProperty("hello", "pricings world!")
                                           )

                          );

            Assert.That(request,                                                                                           Is.Not.Null);

            Assert.That(cpoClient.   Counters.PushPricingProductData.Requests_OK,                                          Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushPricingProductData.Requests_Error,                                       Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushPricingProductData.Responses_OK,                                         Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushPricingProductData.Responses_Error,                                      Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.PushPricingProductData.Requests_OK,                                          Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushPricingProductData.Requests_Error,                                       Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushPricingProductData.Responses_OK,                                         Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushPricingProductData.Responses_Error,                                      Is.EqualTo(0));

            cpoClient.   OnPushPricingProductDataRequest  += (timestamp, cpoClient,    pushPricingProductDataRequest) => {

                Assert.That(pushPricingProductDataRequest.PricingProductData.OperatorId.ToString(),                        Is.EqualTo("DE*GEF"));
                Assert.That(pushPricingProductDataRequest.PricingProductData.ProviderId.ToString(),                        Is.EqualTo("DE-GDF"));
                Assert.That(pushPricingProductDataRequest.PricingProductData.PricingDefaultPrice,                          Is.EqualTo(1.23M));
                Assert.That(pushPricingProductDataRequest.PricingProductData.PricingDefaultPriceCurrency,                  Is.EqualTo(Currency_Id.EUR));
                Assert.That(pushPricingProductDataRequest.PricingProductData.PricingDefaultReferenceUnit,                  Is.EqualTo(Reference_Unit.HOUR));
                Assert.That(pushPricingProductDataRequest.PricingProductData.OperatorName,                                 Is.EqualTo("GraphDefined"));

                Assert.That(pushPricingProductDataRequest.PricingProductData.PricingProductDataRecords.Count,              Is.EqualTo(1));

                var pricingProductDataRecord = pushPricingProductDataRequest.PricingProductData.PricingProductDataRecords.FirstOrDefault();

                Assert.That(pricingProductDataRecord?.ProductId.ToString(),                                                Is.EqualTo("AC1"));
                Assert.That(pricingProductDataRecord?.ReferenceUnit,                                                       Is.EqualTo(Reference_Unit.HOUR));
                Assert.That(pricingProductDataRecord?.ProductPriceCurrency,                                                Is.EqualTo(Currency_Id.EUR));
                Assert.That(pricingProductDataRecord?.PricePerReferenceUnit,                                               Is.EqualTo(1));
                Assert.That(pricingProductDataRecord?.MaximumProductChargingPower,                                         Is.EqualTo(22));
                Assert.That(pricingProductDataRecord?.IsValid24hours,                                                      Is.False);
                Assert.That(pricingProductDataRecord?.ProductAvailabilityTimes.Count(),                                    Is.EqualTo(1));
                Assert.That(pricingProductDataRecord?.ProductAvailabilityTimes.ElementAt(0).Period.Begin,                  Is.EqualTo(HourMinute.Parse("09:00")));
                Assert.That(pricingProductDataRecord?.ProductAvailabilityTimes.ElementAt(0).Period.End,                    Is.EqualTo(HourMinute.Parse("18:00")));
                Assert.That(pricingProductDataRecord?.ProductAvailabilityTimes.ElementAt(0).On,                            Is.EqualTo(WeekDay.Everyday));
                Assert.That(pricingProductDataRecord?.AdditionalReferences.    Count(),                                    Is.EqualTo(1));
                Assert.That(pricingProductDataRecord?.AdditionalReferences.ElementAt(0).AdditionalReference,               Is.EqualTo(Additional_Reference.ParkingFee));
                Assert.That(pricingProductDataRecord?.AdditionalReferences.ElementAt(0).AdditionalReferenceUnit,           Is.EqualTo(Reference_Unit.HOUR));
                Assert.That(pricingProductDataRecord?.AdditionalReferences.ElementAt(0).PricePerAdditionalReferenceUnit,   Is.EqualTo(2));

                Assert.That(pushPricingProductDataRequest.Action,                                                          Is.EqualTo(ActionTypes.FullLoad));
                Assert.That(pushPricingProductDataRequest.CustomData?.Count,                                               Is.EqualTo(1));
                Assert.That(pushPricingProductDataRequest.CustomData?["hello"]?.Value<String>(),                           Is.EqualTo("pricings world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClient.   OnPushPricingProductDataResponse += (timestamp, cpoClient,    pushPricingProductDataRequest, oicpResponse, runtime) => {

                var pushPricingProductDataResponse = oicpResponse.Response;

                Assert.That(pushPricingProductDataResponse,                                                                Is.Not.Null);
                Assert.That(pushPricingProductDataResponse?.Result,                                                        Is.True);
                Assert.That(pushPricingProductDataResponse?.StatusCode.Code,                                               Is.EqualTo(StatusCodes.Success));

                Assert.That(pushPricingProductDataResponse?.SessionId,                                                     Is.Null);
                Assert.That(pushPricingProductDataResponse?.CPOPartnerSessionId,                                           Is.Null);
                Assert.That(pushPricingProductDataResponse?.EMPPartnerSessionId,                                           Is.Null);

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnPushPricingProductDataRequest  += (timestamp, cpoClientAPI, pushPricingProductDataRequest) => {

                Assert.That(pushPricingProductDataRequest.PricingProductData.OperatorId.ToString(),                        Is.EqualTo("DE*GEF"));
                Assert.That(pushPricingProductDataRequest.PricingProductData.ProviderId.ToString(),                        Is.EqualTo("DE-GDF"));
                Assert.That(pushPricingProductDataRequest.PricingProductData.PricingDefaultPrice,                          Is.EqualTo(1.23M));
                Assert.That(pushPricingProductDataRequest.PricingProductData.PricingDefaultPriceCurrency,                  Is.EqualTo(Currency_Id.EUR));
                Assert.That(pushPricingProductDataRequest.PricingProductData.PricingDefaultReferenceUnit,                  Is.EqualTo(Reference_Unit.HOUR));
                Assert.That(pushPricingProductDataRequest.PricingProductData.OperatorName,                                 Is.EqualTo("GraphDefined"));

                Assert.That(pushPricingProductDataRequest.PricingProductData.PricingProductDataRecords.Count,              Is.EqualTo(1));

                var pricingProductDataRecord = pushPricingProductDataRequest.PricingProductData.PricingProductDataRecords.FirstOrDefault();

                Assert.That(pricingProductDataRecord?.ProductId.ToString(),                                                Is.EqualTo("AC1"));
                Assert.That(pricingProductDataRecord?.ReferenceUnit,                                                       Is.EqualTo(Reference_Unit.HOUR));
                Assert.That(pricingProductDataRecord?.ProductPriceCurrency,                                                Is.EqualTo(Currency_Id.EUR));
                Assert.That(pricingProductDataRecord?.PricePerReferenceUnit,                                               Is.EqualTo(1));
                Assert.That(pricingProductDataRecord?.MaximumProductChargingPower,                                         Is.EqualTo(22));
                Assert.That(pricingProductDataRecord?.IsValid24hours,                                                      Is.False);
                Assert.That(pricingProductDataRecord?.ProductAvailabilityTimes.Count(),                                    Is.EqualTo(1));
                Assert.That(pricingProductDataRecord?.ProductAvailabilityTimes.ElementAt(0).Period.Begin,                  Is.EqualTo(HourMinute.Parse("09:00")));
                Assert.That(pricingProductDataRecord?.ProductAvailabilityTimes.ElementAt(0).Period.End,                    Is.EqualTo(HourMinute.Parse("18:00")));
                Assert.That(pricingProductDataRecord?.ProductAvailabilityTimes.ElementAt(0).On,                            Is.EqualTo(WeekDay.Everyday));
                Assert.That(pricingProductDataRecord?.AdditionalReferences.    Count(),                                    Is.EqualTo(1));
                Assert.That(pricingProductDataRecord?.AdditionalReferences.ElementAt(0).AdditionalReference,               Is.EqualTo(Additional_Reference.ParkingFee));
                Assert.That(pricingProductDataRecord?.AdditionalReferences.ElementAt(0).AdditionalReferenceUnit,           Is.EqualTo(Reference_Unit.HOUR));
                Assert.That(pricingProductDataRecord?.AdditionalReferences.ElementAt(0).PricePerAdditionalReferenceUnit,   Is.EqualTo(2));

                Assert.That(pushPricingProductDataRequest.Action,                                                          Is.EqualTo(ActionTypes.FullLoad));
                Assert.That(pushPricingProductDataRequest.CustomData?.Count,                                               Is.EqualTo(1));
                Assert.That(pushPricingProductDataRequest.CustomData?["hello"]?.Value<String>(),                           Is.EqualTo("pricings world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnPushPricingProductDataResponse += (timestamp, cpoClientAPI, pushPricingProductDataRequest, oicpResponse, runtime) => {

                var pushPricingProductDataResponse = oicpResponse.Response;

                Assert.That(pushPricingProductDataResponse,                                                                Is.Not.Null);
                Assert.That(pushPricingProductDataResponse?.Result,                                                        Is.True);
                Assert.That(pushPricingProductDataResponse?.StatusCode.Code,                                               Is.EqualTo(StatusCodes.Success));

                Assert.That(pushPricingProductDataResponse?.SessionId,                                                     Is.Null);
                Assert.That(pushPricingProductDataResponse?.CPOPartnerSessionId,                                           Is.Null);
                Assert.That(pushPricingProductDataResponse?.EMPPartnerSessionId,                                           Is.Null);

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await cpoClient.PushPricingProductData(request);

            Assert.That(oicpResult,                                                                                        Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                                           Is.True);
            Assert.That(oicpResult.Response?.Result,                                                                       Is.True);
            Assert.That(oicpResult.Response?.StatusCode.Code,                                                              Is.EqualTo(StatusCodes.Success));

            Assert.That(cpoClient.   Counters.PushPricingProductData.Requests_OK,                                          Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.PushPricingProductData.Requests_Error,                                       Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushPricingProductData.Responses_OK,                                         Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.PushPricingProductData.Responses_Error,                                      Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.PushPricingProductData.Requests_OK,                                          Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.PushPricingProductData.Requests_Error,                                       Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushPricingProductData.Responses_OK,                                         Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.PushPricingProductData.Responses_Error,                                      Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                                              Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                                             Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                                              Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                                             Is.EqualTo(1));

        }

        #endregion


        #region PushEVSEPricing_Test1()

        [Test]
        public async Task PushEVSEPricing_Test1()
        {

            if (cpoClientAPI is null)
            {
                Assert.Fail("cpoClientAPI must not be null!");
                return;
            }

            if (cpoClient is null)
            {
                Assert.Fail("cpoClient must not be null!");
                return;
            }

            var clientRequestLogging   = 0;
            var clientResponseLogging  = 0;
            var serverRequestLogging   = 0;
            var serverResponseLogging  = 0;

            var request = new PushEVSEPricingRequest(

                              OperatorId:    Operator_Id.Parse("DE*GEF"),
                              EVSEPricing:   [
                                                 new EVSEPricing(
                                                     EVSEId:             EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                                     EVSEIdProductList:  [ PartnerProduct_Id.Parse("AC1") ],
                                                     ProviderId:         Provider_Id.Parse("DE-GDF")
                                                 ),
                                                 new EVSEPricing( // '*' meaning for all providers!
                                                     EVSEId:             EVSE_Id.Parse("DE*GEF*E1234567*A*2"),
                                                     EVSEIdProductList:  [ PartnerProduct_Id.Parse("AC3") ]
                                                 )
                                             ],
                              Action:        ActionTypes.FullLoad,

                              CustomData:    new JObject(
                                                 new JProperty("hello", "pricings world!")
                                             )

                          );

            Assert.That(request,                                                                     Is.Not.Null);

            Assert.That(cpoClient.   Counters.PushEVSEPricing.Requests_OK,                           Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushEVSEPricing.Requests_Error,                        Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushEVSEPricing.Responses_OK,                          Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushEVSEPricing.Responses_Error,                       Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.PushEVSEPricing.Requests_OK,                           Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushEVSEPricing.Requests_Error,                        Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushEVSEPricing.Responses_OK,                          Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushEVSEPricing.Responses_Error,                       Is.EqualTo(0));

            cpoClient.   OnPushEVSEPricingRequest  += (timestamp, cpoClient,    pushEVSEPricingRequest) => {

                Assert.That(pushEVSEPricingRequest.OperatorId.ToString(),                            Is.EqualTo("DE*GEF"));

                Assert.That(pushEVSEPricingRequest.EVSEPricing.ElementAt(0).EVSEId.    ToString(),   Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(pushEVSEPricingRequest.EVSEPricing.ElementAt(0).EVSEIdProductList,       Is.EquivalentTo([ PartnerProduct_Id.Parse("AC1") ]));
                Assert.That(pushEVSEPricingRequest.EVSEPricing.ElementAt(0).ProviderId.ToString(),   Is.EqualTo("DE-GDF"));

                Assert.That(pushEVSEPricingRequest.EVSEPricing.ElementAt(1).EVSEId.    ToString(),   Is.EqualTo("DE*GEF*E1234567*A*2"));
                Assert.That(pushEVSEPricingRequest.EVSEPricing.ElementAt(1).EVSEIdProductList,       Is.EquivalentTo([ PartnerProduct_Id.Parse("AC3") ]));
                Assert.That(pushEVSEPricingRequest.EVSEPricing.ElementAt(1).ProviderId,              Is.Null);

                Assert.That(pushEVSEPricingRequest.Action,                                           Is.EqualTo(ActionTypes.FullLoad));

                Assert.That(pushEVSEPricingRequest.CustomData?.Count,                                Is.EqualTo(1));
                Assert.That(pushEVSEPricingRequest.CustomData?["hello"]?.Value<String>(),            Is.EqualTo("pricings world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClient.   OnPushEVSEPricingResponse += (timestamp, cpoClient,    pushEVSEPricingRequest, oicpResponse, runtime) => {

                var pushEVSEPricingResponse = oicpResponse.Response;

                Assert.That(pushEVSEPricingResponse,                                                 Is.Not.Null);
                Assert.That(pushEVSEPricingResponse?.Result,                                         Is.True);
                Assert.That(pushEVSEPricingResponse?.StatusCode.Code,                                Is.EqualTo(StatusCodes.Success));

                Assert.That(pushEVSEPricingResponse?.SessionId,                                      Is.Null);
                Assert.That(pushEVSEPricingResponse?.CPOPartnerSessionId,                            Is.Null);
                Assert.That(pushEVSEPricingResponse?.EMPPartnerSessionId,                            Is.Null);

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnPushEVSEPricingRequest  += (timestamp, cpoClientAPI, pushEVSEPricingRequest) => {

                Assert.That(pushEVSEPricingRequest.OperatorId.ToString(),                            Is.EqualTo("DE*GEF"));

                Assert.That(pushEVSEPricingRequest.EVSEPricing.ElementAt(0).EVSEId.    ToString(),   Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(pushEVSEPricingRequest.EVSEPricing.ElementAt(0).EVSEIdProductList,       Is.EquivalentTo([ PartnerProduct_Id.Parse("AC1") ]));
                Assert.That(pushEVSEPricingRequest.EVSEPricing.ElementAt(0).ProviderId.ToString(),   Is.EqualTo("DE-GDF"));

                Assert.That(pushEVSEPricingRequest.EVSEPricing.ElementAt(1).EVSEId.    ToString(),   Is.EqualTo("DE*GEF*E1234567*A*2"));
                Assert.That(pushEVSEPricingRequest.EVSEPricing.ElementAt(1).EVSEIdProductList,       Is.EquivalentTo([ PartnerProduct_Id.Parse("AC3") ]));
                Assert.That(pushEVSEPricingRequest.EVSEPricing.ElementAt(1).ProviderId,              Is.Null);

                Assert.That(pushEVSEPricingRequest.Action,                                           Is.EqualTo(ActionTypes.FullLoad));

                Assert.That(pushEVSEPricingRequest.CustomData?.Count,                                Is.EqualTo(1));
                Assert.That(pushEVSEPricingRequest.CustomData?["hello"]?.Value<String>(),            Is.EqualTo("pricings world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnPushEVSEPricingResponse += (timestamp, cpoClientAPI, pushEVSEPricingRequest, oicpResponse, runtime) => {

                var pushEVSEPricingResponse = oicpResponse.Response;

                Assert.That(pushEVSEPricingResponse,                                                 Is.Not.Null);
                Assert.That(pushEVSEPricingResponse?.Result,                                         Is.True);
                Assert.That(pushEVSEPricingResponse?.StatusCode.Code,                                Is.EqualTo(StatusCodes.Success));

                Assert.That(pushEVSEPricingResponse?.SessionId,                                      Is.Null);
                Assert.That(pushEVSEPricingResponse?.CPOPartnerSessionId,                            Is.Null);
                Assert.That(pushEVSEPricingResponse?.EMPPartnerSessionId,                            Is.Null);

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await cpoClient.PushEVSEPricing(request);

            Assert.That(oicpResult,                                                                  Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                     Is.True);
            Assert.That(oicpResult.Response?.Result,                                                 Is.True);
            Assert.That(oicpResult.Response?.StatusCode.Code,                                        Is.EqualTo(StatusCodes.Success));

            Assert.That(cpoClient.   Counters.PushEVSEPricing.Requests_OK,                           Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.PushEVSEPricing.Requests_Error,                        Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushEVSEPricing.Responses_OK,                          Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.PushEVSEPricing.Responses_Error,                       Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.PushEVSEPricing.Requests_OK,                           Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.PushEVSEPricing.Requests_Error,                        Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushEVSEPricing.Responses_OK,                          Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.PushEVSEPricing.Responses_Error,                       Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                        Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                       Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                        Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                       Is.EqualTo(1));

        }

        #endregion

    }

}
