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
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.OICPv2_3.EMP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.EMP.client
{

    /// <summary>
    /// OICP EMP Client API test defaults.
    /// </summary>
    public abstract class AEMPClientAPITests
    {

        #region Data

        protected EMPClientAPI?  empClientAPI;
        protected EMPClient?     empClient;

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

            empClientAPI = new EMPClientAPI(
                               ExternalDNSName:  "open.charging.cloud",
                               HTTPServerPort:   IPPort.Parse(8500),
                               LoggingPath:      "tests",
                               AutoStart:        true
                           );

            ClassicAssert.IsNotNull(empClientAPI);


            empClientAPI.OnPullEVSEData                    += (timestamp, empClientAPI, pullEVSEDataRequest)                    => {

                return Task.FromResult(
                    OICPResult<PullEVSEDataResponse>.Success(
                        pullEVSEDataRequest,
                        new PullEVSEDataResponse(
                            Timestamp.Now,
                            pullEVSEDataRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - pullEVSEDataRequest.Timestamp,
                            Array.Empty<EVSEDataRecord>(),
                            pullEVSEDataRequest,
                            StatusCode: new StatusCode(
                                            StatusCodes.Success
                                        )
                        )
                    )
                );

            };

            empClientAPI.OnPullEVSEStatus                  += (timestamp, empClientAPI, pullEVSEStatusRequest)                  => {

                return Task.FromResult(
                    OICPResult<PullEVSEStatusResponse>.Success(
                        pullEVSEStatusRequest,
                        new PullEVSEStatusResponse(
                            Timestamp.Now,
                            pullEVSEStatusRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - pullEVSEStatusRequest.Timestamp,
                            Array.Empty<OperatorEVSEStatus>(),
                            pullEVSEStatusRequest,
                            StatusCode: new StatusCode(
                                            StatusCodes.Success
                                        )
                        )
                    )
                );

            };

            empClientAPI.OnPullEVSEStatusById              += (timestamp, empClientAPI, pullEVSEStatusByIdRequest)              => {

                return Task.FromResult(
                    OICPResult<PullEVSEStatusByIdResponse>.Success(
                        pullEVSEStatusByIdRequest,
                        new PullEVSEStatusByIdResponse(
                            Timestamp.Now,
                            pullEVSEStatusByIdRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - pullEVSEStatusByIdRequest.Timestamp,
                            Array.Empty<EVSEStatusRecord>(),
                            pullEVSEStatusByIdRequest,
                            StatusCode: new StatusCode(
                                            StatusCodes.Success
                                        )
                        )
                    )
                );

            };

            empClientAPI.OnPullEVSEStatusByOperatorId      += (timestamp, empClientAPI, pullEVSEStatusByOperatorIdRequest)      => {

                return Task.FromResult(
                    OICPResult<PullEVSEStatusByOperatorIdResponse>.Success(
                        pullEVSEStatusByOperatorIdRequest,
                        new PullEVSEStatusByOperatorIdResponse(
                            Timestamp.Now,
                            pullEVSEStatusByOperatorIdRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - pullEVSEStatusByOperatorIdRequest.Timestamp,
                            Array.Empty<OperatorEVSEStatus>(),
                            pullEVSEStatusByOperatorIdRequest,
                            StatusCode: new StatusCode(
                                            StatusCodes.Success
                                        )
                        )
                    )
                );

            };


            empClientAPI.OnPullPricingProductData          += (timestamp, empClientAPI, pullPricingProductDataRequest)          => {

                if (pullPricingProductDataRequest.OperatorIds.Contains(Operator_Id.Parse("DE*GEF")))
                    return Task.FromResult(
                        OICPResult<PullPricingProductDataResponse>.Success(
                            pullPricingProductDataRequest,
                            new PullPricingProductDataResponse(
                                Timestamp.Now,
                                pullPricingProductDataRequest.EventTrackingId ?? EventTracking_Id.New,
                                Process_Id.NewRandom(),
                                Timestamp.Now - pullPricingProductDataRequest.Timestamp,
                                new PricingProductData[] {
                                    new PricingProductData(
                                        OperatorId:                    Operator_Id.Parse("DE*GEF"),
                                        ProviderId:                    pullPricingProductDataRequest.ProviderId,
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
                                        OperatorName:                  "GraphDefined"
                                    )
                                },
                                pullPricingProductDataRequest,
                                StatusCode: new StatusCode(
                                                StatusCodes.Success
                                            )
                            )
                        )
                    );

                else
                    return Task.FromResult(
                        OICPResult<PullPricingProductDataResponse>.Success(
                            pullPricingProductDataRequest,
                            new PullPricingProductDataResponse(
                                Timestamp.Now,
                                pullPricingProductDataRequest.EventTrackingId ?? EventTracking_Id.New,
                                Process_Id.NewRandom(),
                                Timestamp.Now - pullPricingProductDataRequest.Timestamp,
                                Array.Empty<PricingProductData>(),
                                pullPricingProductDataRequest,
                                StatusCode: new StatusCode(
                                                StatusCodes.Success
                                            )
                            )
                        )
                    );

            };

            empClientAPI.OnPullEVSEPricing                 += (timestamp, empClientAPI, pullEVSEPricingRequest)                 => {

                if (pullEVSEPricingRequest.OperatorIds.Contains(Operator_Id.Parse("DE*GEF")))
                    return Task.FromResult(
                        OICPResult<PullEVSEPricingResponse>.Success(
                            pullEVSEPricingRequest,
                            new PullEVSEPricingResponse(
                                Timestamp.Now,
                                pullEVSEPricingRequest.EventTrackingId ?? EventTracking_Id.New,
                                Process_Id.NewRandom(),
                                Timestamp.Now - pullEVSEPricingRequest.Timestamp,
                                new OperatorEVSEPricing[] {
                                    new OperatorEVSEPricing(
                                        new EVSEPricing[] {
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
                                        OperatorId:     Operator_Id.Parse("DE*GEF"),
                                        OperatorName:  "GraphDefined"
                                    )
                                },
                                pullEVSEPricingRequest,
                                StatusCode: new StatusCode(
                                                StatusCodes.Success
                                            )
                            )
                        )
                    );

                else
                    return Task.FromResult(
                        OICPResult<PullEVSEPricingResponse>.Success(
                            pullEVSEPricingRequest,
                            new PullEVSEPricingResponse(
                                Timestamp.Now,
                                pullEVSEPricingRequest.EventTrackingId ?? EventTracking_Id.New,
                                Process_Id.NewRandom(),
                                Timestamp.Now - pullEVSEPricingRequest.Timestamp,
                                Array.Empty<OperatorEVSEPricing>(),
                                pullEVSEPricingRequest,
                                StatusCode: new StatusCode(
                                                StatusCodes.Success
                                            )
                            )
                        )
                    );

            };


            empClientAPI.OnPushAuthenticationData          += (timestamp, empClientAPI, pushPushAuthenticationDataRequest)      => {

                return Task.FromResult(
                    OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Success(
                        pushPushAuthenticationDataRequest,
                        new Acknowledgement<PushAuthenticationDataRequest>(
                            Timestamp.Now,
                            pushPushAuthenticationDataRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - pushPushAuthenticationDataRequest.Timestamp,
                            new StatusCode(
                                StatusCodes.Success
                            ),
                            pushPushAuthenticationDataRequest,
                            null,
                            true
                        )
                    )
                );

            };


            empClientAPI.OnAuthorizeRemoteReservationStart += (timestamp, empClientAPI, authorizeRemoteReservationStartRequest) => {

                return Task.FromResult(
                    OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Success(
                        authorizeRemoteReservationStartRequest,
                        new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                            Timestamp.Now,
                            authorizeRemoteReservationStartRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - authorizeRemoteReservationStartRequest.Timestamp,
                            new StatusCode(
                                StatusCodes.Success
                            ),
                            authorizeRemoteReservationStartRequest,
                            null,
                            true
                        )
                    )
                );

            };

            empClientAPI.OnAuthorizeRemoteReservationStop  += (timestamp, empClientAPI, authorizeRemoteReservationStopRequest)  => {

                return Task.FromResult(
                    OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Success(
                        authorizeRemoteReservationStopRequest,
                        new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                            Timestamp.Now,
                            authorizeRemoteReservationStopRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - authorizeRemoteReservationStopRequest.Timestamp,
                            new StatusCode(
                                StatusCodes.Success
                            ),
                            authorizeRemoteReservationStopRequest,
                            null,
                            true
                        )
                    )
                );

            };

            empClientAPI.OnAuthorizeRemoteStart            += (timestamp, empClientAPI, authorizeRemoteStartRequest)            => {

                return Task.FromResult(
                    OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Success(
                        authorizeRemoteStartRequest,
                        new Acknowledgement<AuthorizeRemoteStartRequest>(
                            Timestamp.Now,
                            authorizeRemoteStartRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - authorizeRemoteStartRequest.Timestamp,
                            new StatusCode(
                                StatusCodes.Success
                            ),
                            authorizeRemoteStartRequest,
                            null,
                            true
                        )
                    )
                );

            };

            empClientAPI.OnAuthorizeRemoteStop             += (timestamp, empClientAPI, authorizeRemoteStopRequest)             => {

                return Task.FromResult(
                   OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Success(
                       authorizeRemoteStopRequest,
                       new Acknowledgement<AuthorizeRemoteStopRequest>(
                           Timestamp.Now,
                           authorizeRemoteStopRequest.EventTrackingId ?? EventTracking_Id.New,
                           Process_Id.NewRandom(),
                           Timestamp.Now - authorizeRemoteStopRequest.Timestamp,
                           new StatusCode(
                               StatusCodes.Success
                           ),
                           authorizeRemoteStopRequest,
                           null,
                           true
                       )
                   )
               );

            };


            empClientAPI.OnGetChargeDetailRecords          += (timestamp, empClientAPI, getChargeDetailRecordsRequest)          => {

                if (Timestamp.Now - getChargeDetailRecordsRequest.To > TimeSpan.FromMinutes(1))
                    return Task.FromResult(
                        OICPResult<GetChargeDetailRecordsResponse>.Success(
                            getChargeDetailRecordsRequest,
                            new GetChargeDetailRecordsResponse(
                                Timestamp.Now,
                                getChargeDetailRecordsRequest.EventTrackingId ?? EventTracking_Id.New,
                                Process_Id.NewRandom(),
                                Timestamp.Now - getChargeDetailRecordsRequest.Timestamp,
                                Array.Empty<ChargeDetailRecord>(),
                                getChargeDetailRecordsRequest,
                                StatusCode: new StatusCode(
                                                StatusCodes.Success
                                            )
                            )
                        )
                    );

                else
                    return Task.FromResult(
                        OICPResult<GetChargeDetailRecordsResponse>.Success(
                            getChargeDetailRecordsRequest,
                            new GetChargeDetailRecordsResponse(
                                Timestamp.Now,
                                getChargeDetailRecordsRequest.EventTrackingId ?? EventTracking_Id.New,
                                Process_Id.NewRandom(),
                                Timestamp.Now - getChargeDetailRecordsRequest.Timestamp,
                                new ChargeDetailRecord[] {

                                    new ChargeDetailRecord(
                                        SessionId:                       Session_Id.Parse("4cfe3192-87ec-4757-9560-a6ce896bb88b"),
                                        EVSEId:                          EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                        Identification:                  Identification.FromUID(UID.Parse("AABBCCDD")),
                                        SessionStart:                    DateTime.Parse("2022-08-09T10:18:25.229Z"),
                                        SessionEnd:                      DateTime.Parse("2022-08-09T11:18:25.229Z"),
                                        ChargingStart:                   DateTime.Parse("2022-08-09T10:20:25.229Z"),
                                        ChargingEnd:                     DateTime.Parse("2022-08-09T11:13:25.229Z"),
                                        ConsumedEnergy:                  WattHour.ParseKWh(35),

                                        PartnerProductId:                PartnerProduct_Id.Parse("AC3"),
                                        CPOPartnerSessionId:             CPOPartnerSession_Id.Parse("e9c6faad-75c8-4f5b-9b5c-164ae7459804"),
                                        EMPPartnerSessionId:             EMPPartnerSession_Id.Parse("290b96b3-57df-4021-b8f8-50d9c211c767"),
                                        MeterValueStart:                 WattHour.ParseKWh( 3),
                                        MeterValueEnd:                   WattHour.ParseKWh(38),
                                        MeterValuesInBetween:            [
                                                                             WattHour.ParseKWh(4),
                                                                             WattHour.ParseKWh(5),
                                                                             WattHour.ParseKWh(6)
                                                                         ],
                                        SignedMeteringValues:            [
                                                                             new SignedMeteringValue(
                                                                                 "loooong start...",
                                                                                 MeteringStatusType.Start
                                                                             ),
                                                                             new SignedMeteringValue(
                                                                                 "loooong progress...",
                                                                                 MeteringStatusType.Progress
                                                                             ),
                                                                             new SignedMeteringValue(
                                                                                 "loooong end...",
                                                                                 MeteringStatusType.End
                                                                             )
                                                                         ],
                                        CalibrationLawVerificationInfo:  new CalibrationLawVerification(
                                                                             CalibrationLawCertificateId:                  "4c6da173-6427-49ed-9b7d-ab0c674d4bc2",
                                                                             PublicKey:                                    "0x046eb5c26727e9477f916eb5c26727e9477f91f872d3d79b2bd9f872d3d79b2bd9",
                                                                             MeteringSignatureURL:                         URL.Parse("https://open.charging.cloud"),
                                                                             MeteringSignatureEncodingFormat:              "plain",
                                                                             SignedMeteringValuesVerificationInstruction:  "Just use the Chargy Transparency Software!",
                                                                             CustomData:                                   null
                                                                         ),
                                        HubOperatorId:                   Operator_Id.Parse("DE*GEF"),
                                        HubProviderId:                   Provider_Id.Parse("DE-GDF"),

                                        CustomData:                      null,
                                        InternalData:                    null
                                    )

                                },
                                getChargeDetailRecordsRequest,
                                StatusCode: new StatusCode(
                                                StatusCodes.Success
                                            )
                            )
                        )
                    );

            };



            empClient = new EMPClient(URL.Parse("http://127.0.0.1:8500"),
                                      RequestTimeout: TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(empClient);

        }

        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public void ShutdownEachTest()
        {
            empClientAPI?.Shutdown();
        }

        #endregion

        #region ShutdownOnce()

        [OneTimeTearDown]
        public void ShutdownOnce()
        {

        }

        #endregion



        //ToDo: RAW tests: OperatorId != OperatorIdURL


        //protected static async Task<HTTPResponse> SendEMPAuthorizeStart(AuthorizeStartRequest Request)
        //{

        //    return await new HTTPSClient(URL.Parse("http://127.0.0.1:8000")).
        //                     Execute(client => client.POSTRequest(HTTPPath.Parse("/api/oicp/charging/v21/operators/DE*GEF/authorize/start"),
        //                                                          requestbuilder => {
        //                                                              requestBuilder.Host         = HTTPHostname.Localhost;
        //                                                              requestBuilder.ContentType  = HTTPContentType.Application.JSON_UTF8;
        //                                                              requestBuilder.Content      = Request.ToJSON().ToUTF8Bytes(Newtonsoft.Json.Formatting.None);
        //                                                              requestBuilder.Accept.Add(HTTPContentType.Application.JSON_UTF8);
        //                                                              requestBuilder.Connection   = ConnectionType.Close;
        //                                                          }),
        //                             //CancellationToken:    CancellationToken,
        //                             //EventTrackingId:      EventTrackingId,
        //                             RequestTimeout:       TimeSpan.FromSeconds(10)).

        //                     ConfigureAwait(false);

        //}

    }

}
