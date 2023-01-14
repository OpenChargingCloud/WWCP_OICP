/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.OICPv2_3.CPO;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.CPO.client
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
                               HTTPServerPort:   IPPort.Parse(9500),
                               LoggingPath:      "tests",
                               Autostart:        true
                           );

            Assert.IsNotNull(cpoClientAPI);


            cpoClientAPI.OnPushEVSEData                 += (timestamp, cpoClientAPI, pushEVSEDataRequest)                 => {

                return Task.FromResult(
                    OICPResult<Acknowledgement<PushEVSEDataRequest>>.Success(
                        pushEVSEDataRequest,
                        new Acknowledgement<PushEVSEDataRequest>(
                            Timestamp.Now,
                            pushEVSEDataRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - pushEVSEDataRequest.Timestamp,
                            new StatusCode(
                                StatusCodes.Success
                            ),
                            pushEVSEDataRequest,
                            null,
                            true
                        )
                    )
                );

            };

            cpoClientAPI.OnPushEVSEStatus               += (timestamp, cpoClientAPI, pushEVSEStatusRequest)               => {

                return Task.FromResult(
                    OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Success(
                        pushEVSEStatusRequest,
                        new Acknowledgement<PushEVSEStatusRequest>(
                            Timestamp.Now,
                            pushEVSEStatusRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - pushEVSEStatusRequest.Timestamp,
                            new StatusCode(
                                StatusCodes.Success
                            ),
                            pushEVSEStatusRequest,
                            null,
                            true
                        )
                    )
                );

            };


            cpoClientAPI.OnPushPricingProductData       += (timestamp, cpoClientAPI, pushPricingProductDataRequest)       => {

                return Task.FromResult(
                    OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Success(
                        pushPricingProductDataRequest,
                        new Acknowledgement<PushPricingProductDataRequest>(
                            Timestamp.Now,
                            pushPricingProductDataRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - pushPricingProductDataRequest.Timestamp,
                            new StatusCode(
                                StatusCodes.Success
                            ),
                            pushPricingProductDataRequest,
                            null,
                            true
                        )
                    )
                );

            };

            cpoClientAPI.OnPushEVSEPricing              += (timestamp, cpoClientAPI, pushEVSEPricingRequest)              => {

                return Task.FromResult(
                    OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Success(
                        pushEVSEPricingRequest,
                        new Acknowledgement<PushEVSEPricingRequest>(
                            Timestamp.Now,
                            pushEVSEPricingRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - pushEVSEPricingRequest.Timestamp,
                            new StatusCode(
                                StatusCodes.Success
                            ),
                            pushEVSEPricingRequest,
                            null,
                            true
                        )
                    )
                );

            };


            cpoClientAPI.OnPullAuthenticationData       += (timestamp, cpoClientAPI, pullAuthenticationDataRequest)       => {

                if (pullAuthenticationDataRequest.OperatorId == Operator_Id.Parse("DE*GEF"))
                    return Task.FromResult(
                        OICPResult<PullAuthenticationDataResponse>.Success(
                            pullAuthenticationDataRequest,
                            new PullAuthenticationDataResponse(
                                Timestamp.Now,
                                pullAuthenticationDataRequest.EventTrackingId ?? EventTracking_Id.New,
                                Process_Id.NewRandom(),
                                Timestamp.Now - pullAuthenticationDataRequest.Timestamp,
                                new ProviderAuthenticationData[] {
                                    new ProviderAuthenticationData(
                                        new Identification[] {

                                            Identification.FromUID(
                                                UID.Parse("11223344")
                                            ),

                                            Identification.FromRFIDIdentification(
                                                new RFIDIdentification(
                                                    UID:             UID.Parse("55667788"),
                                                    RFIDType:        RFIDTypes.MifareClassic,
                                                    EVCOId:          EVCO_Id.Parse("DE-GDF-C12345678-X"),
                                                    PrintedNumber:  "GDF-0001",
                                                    ExpiryDate:      DateTime.Parse("2022-08-09T10:18:25.229Z"),
                                                    CustomData:      null
                                                ),
                                                CustomData:  null
                                            ),

                                            Identification.FromQRCodeIdentification(
                                                new QRCodeIdentification(
                                                    EVCOId:          EVCO_Id.Parse("DE-GDF-C56781234-X"),
                                                    HashedPIN:       new HashedPIN(
                                                                         Hash_Value.Parse("XXX"),
                                                                         HashFunctions.Bcrypt
                                                                     )
                                                ),
                                                CustomData:  null
                                            ),

                                            Identification.FromRemoteIdentification(
                                                EVCO_Id.Parse("DE-GDF-C23456781-X"),
                                                CustomData:  null
                                            ),

                                            Identification.FromPlugAndChargeIdentification(
                                                EVCO_Id.Parse("DE-GDF-C81235674-X"),
                                                CustomData:  null
                                            )

                                        },
                                        Provider_Id.Parse("DE-GDF")
                                    )
                                },
                                pullAuthenticationDataRequest,
                                StatusCode: new StatusCode(
                                                StatusCodes.Success
                                            )
                            )
                        )
                    );

                else
                    return Task.FromResult(
                        OICPResult<PullAuthenticationDataResponse>.Success(
                            pullAuthenticationDataRequest,
                            new PullAuthenticationDataResponse(
                                Timestamp.Now,
                                pullAuthenticationDataRequest.EventTrackingId ?? EventTracking_Id.New,
                                Process_Id.NewRandom(),
                                Timestamp.Now - pullAuthenticationDataRequest.Timestamp,
                                Array.Empty<ProviderAuthenticationData>(),
                                pullAuthenticationDataRequest,
                                StatusCode: new StatusCode(
                                                StatusCodes.Success
                                            )
                            )
                        )
                    );

            };


            cpoClientAPI.OnAuthorizeStart               += (timestamp, empClientAPI, authorizeStartRequest)               => {

                return Task.FromResult(
                    OICPResult<AuthorizationStartResponse>.Success(
                        authorizeStartRequest,
                        AuthorizationStartResponse.Authorized(
                            authorizeStartRequest,
                            Session_Id.          Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"), // generated by Hubject!
                            authorizeStartRequest.CPOPartnerSessionId,
                            EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),
                            Provider_Id.Parse("DE-GDF"),
                            "Nice to see you!",
                            "Hello world!",
                            new Identification[] {
                                Identification.FromUID(UID.Parse("11223344")),
                                Identification.FromUID(UID.Parse("55667788"))
                            }
                        )
                    )
                );

            };

            cpoClientAPI.OnAuthorizeStop                += (timestamp, empClientAPI, authorizeStopRequest)                => {

                return Task.FromResult(
                    OICPResult<AuthorizationStopResponse>.Success(
                        authorizeStopRequest,
                        AuthorizationStopResponse.Authorized(
                            authorizeStopRequest,
                            authorizeStopRequest.SessionId,
                            authorizeStopRequest.CPOPartnerSessionId,
                            authorizeStopRequest.EMPPartnerSessionId,
                            Provider_Id.Parse("DE-GDF"),
                            "Have a nice day!",
                            "bye bye!"
                        )
                    )
                );

            };


            cpoClientAPI.OnChargingStartNotification    += (timestamp, cpoClientAPI, chargingStartNotificationRequest)    => {

                return Task.FromResult(
                    OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Success(
                        chargingStartNotificationRequest,
                        new Acknowledgement<ChargingStartNotificationRequest>(
                            Timestamp.Now,
                            chargingStartNotificationRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - chargingStartNotificationRequest.Timestamp,
                            new StatusCode(
                                StatusCodes.Success
                            ),
                            chargingStartNotificationRequest,
                            null,
                            true
                        )
                    )
                );

            };

            cpoClientAPI.OnChargingProgressNotification += (timestamp, cpoClientAPI, chargingProgressNotificationRequest) => {

                return Task.FromResult(
                    OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Success(
                        chargingProgressNotificationRequest,
                        new Acknowledgement<ChargingProgressNotificationRequest>(
                            Timestamp.Now,
                            chargingProgressNotificationRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - chargingProgressNotificationRequest.Timestamp,
                            new StatusCode(
                                StatusCodes.Success
                            ),
                            chargingProgressNotificationRequest,
                            null,
                            true
                        )
                    )
                );

            };

            cpoClientAPI.OnChargingEndNotification      += (timestamp, cpoClientAPI, chargingEndNotificationRequest)      => {

                return Task.FromResult(
                    OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Success(
                        chargingEndNotificationRequest,
                        new Acknowledgement<ChargingEndNotificationRequest>(
                            Timestamp.Now,
                            chargingEndNotificationRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - chargingEndNotificationRequest.Timestamp,
                            new StatusCode(
                                StatusCodes.Success
                            ),
                            chargingEndNotificationRequest,
                            null,
                            true
                        )
                    )
                );

            };

            cpoClientAPI.OnChargingErrorNotification    += (timestamp, cpoClientAPI, chargingErrorNotificationRequest)    => {

                return Task.FromResult(
                    OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Success(
                        chargingErrorNotificationRequest,
                        new Acknowledgement<ChargingErrorNotificationRequest>(
                            Timestamp.Now,
                            chargingErrorNotificationRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - chargingErrorNotificationRequest.Timestamp,
                            new StatusCode(
                                StatusCodes.Success
                            ),
                            chargingErrorNotificationRequest,
                            null,
                            true
                        )
                    )
                );

            };


            cpoClientAPI.OnChargeDetailRecord           += (timestamp, cpoClientAPI, chargeDetailRecordRequest)           => {

                return Task.FromResult(
                    OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Success(
                        chargeDetailRecordRequest,
                        new Acknowledgement<ChargeDetailRecordRequest>(
                            Timestamp.Now,
                            chargeDetailRecordRequest.EventTrackingId ?? EventTracking_Id.New,
                            Process_Id.NewRandom(),
                            Timestamp.Now - chargeDetailRecordRequest.Timestamp,
                            new StatusCode(
                                StatusCodes.Success
                            ),
                            chargeDetailRecordRequest,
                            null,
                            true
                        )
                    )
                );

            };



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
