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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.OICPv2_3.EMP;
using cloud.charging.open.protocols.OICPv2_3.CPO;
using cloud.charging.open.protocols.OICPv2_3.CentralService;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.CentralService
{

    /// <summary>
    /// OICP Central Service test defaults.
    /// </summary>
    public abstract class ACentralServiceTests
    {

        #region Data

        protected          CentralServiceAPI?                                          centralServiceAPI;

        protected          CPORoaming?                                                 cpoRoaming_DEGEF;
        protected          CPORoaming?                                                 cpoRoaming_DEBDO;

        protected          EMPRoaming?                                                 empRoaming_DEGDF;
        protected          EMPRoaming?                                                 empRoaming_DEBDP;

        protected readonly Dictionary<Operator_Id, HashSet<EVSEDataRecord>>            EVSEDataRecords;
        protected readonly Dictionary<Operator_Id, HashSet<EVSEStatusRecord>>          EVSEStatusRecords;
        protected readonly Dictionary<Operator_Id, HashSet<PricingProductDataRecord>>  PricingProductData;
        protected readonly Dictionary<Operator_Id, HashSet<EVSEPricing>>               EVSEPricings;
        protected readonly Dictionary<Operator_Id, HashSet<ChargeDetailRecord>>        ChargeDetailRecords;

        #endregion

        #region Constructor(s)

        public ACentralServiceTests()
        {

            this.EVSEDataRecords      = [];
            this.EVSEStatusRecords    = [];
            this.PricingProductData   = [];
            this.EVSEPricings         = [];
            this.ChargeDetailRecords  = [];

        }

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
            EVSEDataRecords.Clear();

            centralServiceAPI = new CentralServiceAPI(
                                    ExternalDNSName:  "open.charging.cloud",
                                    HTTPServerPort:   IPPort.Parse(6001),
                                    LoggingPath:      "tests",
                                    AutoStart:        true
                                );

            ClassicAssert.IsNotNull(centralServiceAPI);


            #region CPOClientAPI delegates...

            centralServiceAPI.CPOClientAPI.OnPushEVSEData                    +=       (timestamp, cpoClientAPI, pushEVSEDataRequest)   => {

                var processId = Process_Id.NewRandom();

                switch (pushEVSEDataRequest.Action)
                {

                    case ActionTypes.FullLoad: {

                            if (!EVSEDataRecords.ContainsKey(pushEVSEDataRequest.OperatorId))
                                EVSEDataRecords.Add(pushEVSEDataRequest.OperatorId, new HashSet<EVSEDataRecord>(pushEVSEDataRequest.EVSEDataRecords));

                            else
                            {

                                EVSEDataRecords[pushEVSEDataRequest.OperatorId].Clear();

                                foreach (var evseDataRecord in pushEVSEDataRequest.EVSEDataRecords)
                                    EVSEDataRecords[pushEVSEDataRequest.OperatorId].Add(evseDataRecord);

                            }

                        }
                        break;

                    case ActionTypes.Update: {

                            // Update NOT Insert

                            if (!EVSEDataRecords.ContainsKey(pushEVSEDataRequest.OperatorId))
                                EVSEDataRecords.Add(pushEVSEDataRequest.OperatorId, new HashSet<EVSEDataRecord>(pushEVSEDataRequest.EVSEDataRecords));

                            else
                            {

                                EVSE_Id? missing = default;

                                var allEVSEIds = EVSEDataRecords[pushEVSEDataRequest.OperatorId].Select(evseDataRecord => evseDataRecord.Id).ToHashSet();

                                foreach (var evseDataRecord in pushEVSEDataRequest.EVSEDataRecords)
                                {

                                    missing = pushEVSEDataRequest.EVSEDataRecords.FirstOrDefault(evseDataRecord => !allEVSEIds.Contains(evseDataRecord.Id))?.Id;

                                    if (missing.HasValue)
                                        return Task.FromResult(
                                                    OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                        pushEVSEDataRequest,
                                                        Acknowledgement<PushEVSEDataRequest>.DataError(
                                                            Request:                    pushEVSEDataRequest,
                                                            StatusCodeDescription:     "EVSE data record for update not found: " + missing.Value.ToString(),
                                                            StatusCodeAdditionalInfo:   null,
                                                            ResponseTimestamp:          Timestamp.Now,
                                                            EventTrackingId:            EventTracking_Id.New,
                                                            Runtime:                    TimeSpan.FromMilliseconds(2),
                                                            ProcessId:                  processId,
                                                            HTTPResponse:               null,
                                                            CustomData:                 null
                                                        ),
                                                        processId
                                                    )
                                                );

                                }

                                foreach (var evseDataRecord in pushEVSEDataRequest.EVSEDataRecords)
                                    EVSEDataRecords[pushEVSEDataRequest.OperatorId].Add(evseDataRecord);
                            }

                        }
                        break;

                    case ActionTypes.Insert: {

                            // Will fail if the EVSE data record already exists!

                            if (!EVSEDataRecords.ContainsKey(pushEVSEDataRequest.OperatorId))
                                EVSEDataRecords.Add(pushEVSEDataRequest.OperatorId, new HashSet<EVSEDataRecord>(pushEVSEDataRequest.EVSEDataRecords));

                            else
                            {

                                EVSE_Id? duplicate = default;

                                var allEVSEIds = EVSEDataRecords[pushEVSEDataRequest.OperatorId].Select(evseDataRecord => evseDataRecord.Id).ToHashSet();

                                foreach (var evseDataRecord in pushEVSEDataRequest.EVSEDataRecords)
                                {

                                    duplicate = pushEVSEDataRequest.EVSEDataRecords.FirstOrDefault(evseDataRecord => allEVSEIds.Contains(evseDataRecord.Id))?.Id;

                                    if (duplicate.HasValue)
                                        return Task.FromResult(
                                                    OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                        pushEVSEDataRequest,
                                                        Acknowledgement<PushEVSEDataRequest>.DataError(
                                                            Request:                    pushEVSEDataRequest,
                                                            StatusCodeDescription:     "EVSE data record '" + duplicate.Value.ToString() + "' already exists!'",
                                                            StatusCodeAdditionalInfo:   null,
                                                            ResponseTimestamp:          Timestamp.Now,
                                                            EventTrackingId:            EventTracking_Id.New,
                                                            Runtime:                    TimeSpan.FromMilliseconds(2),
                                                            ProcessId:                  processId,
                                                            HTTPResponse:               null,
                                                            CustomData:                 null
                                                        ),
                                                        processId
                                                    )
                                                );

                                }

                                foreach (var evseDataRecord in pushEVSEDataRequest.EVSEDataRecords)
                                    EVSEDataRecords[pushEVSEDataRequest.OperatorId].Add(evseDataRecord);

                            }

                        }
                        break;

                    case ActionTypes.Delete: {

                            if (EVSEDataRecords.ContainsKey(pushEVSEDataRequest.OperatorId)) {

                                foreach (var evseDataRecord in pushEVSEDataRequest.EVSEDataRecords)
                                    EVSEDataRecords[pushEVSEDataRequest.OperatorId].Remove(evseDataRecord);

                            }

                        }
                        break;

                }

                return Task.FromResult(
                           OICPResult<Acknowledgement<PushEVSEDataRequest>>.Success(
                               pushEVSEDataRequest,
                               new Acknowledgement<PushEVSEDataRequest>(
                                   Request:             pushEVSEDataRequest,
                                   ResponseTimestamp:   Timestamp.Now,
                                   EventTrackingId:     EventTracking_Id.New,
                                   Runtime:             TimeSpan.FromMilliseconds(2),
                                   StatusCode:          new StatusCode(
                                                            StatusCodes.Success
                                                        ),
                                   HTTPResponse:        null,
                                   Result:              true,
                                   ProcessId:           processId,
                                   CustomData:          null
                               ),
                               processId
                           )
                       );

            };

            centralServiceAPI.CPOClientAPI.OnPushEVSEStatus                  +=       (timestamp, cpoClientAPI, pushEVSEStatusRequest) => {

                var processId = Process_Id.NewRandom();

                switch (pushEVSEStatusRequest.Action)
                {

                    case ActionTypes.FullLoad: {

                            if (!EVSEStatusRecords.ContainsKey(pushEVSEStatusRequest.OperatorId))
                                EVSEStatusRecords.Add(pushEVSEStatusRequest.OperatorId, new HashSet<EVSEStatusRecord>(pushEVSEStatusRequest.EVSEStatusRecords));

                            else
                            {

                                EVSEStatusRecords[pushEVSEStatusRequest.OperatorId].Clear();

                                foreach (var evseStatusRecord in pushEVSEStatusRequest.EVSEStatusRecords)
                                    EVSEStatusRecords[pushEVSEStatusRequest.OperatorId].Add(evseStatusRecord);

                            }

                        }
                        break;

                    case ActionTypes.Update: {

                            // Update OR Insert

                            if (!EVSEStatusRecords.ContainsKey(pushEVSEStatusRequest.OperatorId))
                                EVSEStatusRecords.Add(pushEVSEStatusRequest.OperatorId, new HashSet<EVSEStatusRecord>(pushEVSEStatusRequest.EVSEStatusRecords));

                            else
                                foreach (var evseStatusRecord in pushEVSEStatusRequest.EVSEStatusRecords)
                                    EVSEStatusRecords[pushEVSEStatusRequest.OperatorId].Add(evseStatusRecord);

                        }
                        break;

                    case ActionTypes.Insert: {

                            // Will fail if the EVSE data record already exists!

                            if (!EVSEStatusRecords.ContainsKey(pushEVSEStatusRequest.OperatorId))
                                EVSEStatusRecords.Add(pushEVSEStatusRequest.OperatorId, new HashSet<EVSEStatusRecord>(pushEVSEStatusRequest.EVSEStatusRecords));

                            else
                            {

                                EVSE_Id? duplicate = default;

                                var allEVSEIds = EVSEStatusRecords[pushEVSEStatusRequest.OperatorId].Select(evseStatusRecord => evseStatusRecord.Id).ToHashSet();

                                foreach (var evseStatusRecord in pushEVSEStatusRequest.EVSEStatusRecords)
                                {

                                    duplicate = pushEVSEStatusRequest.EVSEStatusRecords.FirstOrDefault(evseStatusRecord => allEVSEIds.Contains(evseStatusRecord.Id)).Id;

                                    if (duplicate.HasValue)
                                        return Task.FromResult(
                                                    OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                        pushEVSEStatusRequest,
                                                        Acknowledgement<PushEVSEStatusRequest>.DataError(
                                                            Request:                    pushEVSEStatusRequest,
                                                            StatusCodeDescription:     "Duplicate EVSE status found: " + duplicate.Value.ToString(),
                                                            StatusCodeAdditionalInfo:   null,
                                                            ResponseTimestamp:          Timestamp.Now,
                                                            EventTrackingId:            EventTracking_Id.New,
                                                            Runtime:                    TimeSpan.FromMilliseconds(2),
                                                            ProcessId:                  processId,
                                                            HTTPResponse:               null,
                                                            CustomData:                 null
                                                        ),
                                                        processId
                                                    )
                                                );

                                }

                                foreach (var evseStatusRecord in pushEVSEStatusRequest.EVSEStatusRecords)
                                    EVSEStatusRecords[pushEVSEStatusRequest.OperatorId].Add(evseStatusRecord);

                            }

                        }
                        break;

                    case ActionTypes.Delete: {

                            if (EVSEStatusRecords.ContainsKey(pushEVSEStatusRequest.OperatorId)) {

                                foreach (var evseStatusRecord in pushEVSEStatusRequest.EVSEStatusRecords)
                                    EVSEStatusRecords[pushEVSEStatusRequest.OperatorId].Remove(evseStatusRecord);

                            }

                        }
                        break;

                }

                return Task.FromResult(
                           OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Success(
                               pushEVSEStatusRequest,
                               new Acknowledgement<PushEVSEStatusRequest>(
                                   Request:             pushEVSEStatusRequest,
                                   ResponseTimestamp:   Timestamp.Now,
                                   EventTrackingId:     EventTracking_Id.New,
                                   Runtime:             TimeSpan.FromMilliseconds(2),
                                   StatusCode:          new StatusCode(
                                                            StatusCodes.Success
                                                        ),
                                   HTTPResponse:        null,
                                   Result:              true,
                                   ProcessId:           processId,
                                   CustomData:          null
                               ),
                               processId
                           )
                       );

            };


            centralServiceAPI.CPOClientAPI.OnPushPricingProductData          +=       (timestamp, cpoClientAPI, pushPricingProductDataRequest) => {

                var processId = Process_Id.NewRandom();

                switch (pushPricingProductDataRequest.Action)
                {

                    case ActionTypes.FullLoad: {

                            if (!PricingProductData.ContainsKey(pushPricingProductDataRequest.OperatorId))
                                PricingProductData.Add(pushPricingProductDataRequest.OperatorId, new HashSet<PricingProductDataRecord>(pushPricingProductDataRequest.PricingProductDataRecords));

                            else
                            {

                                PricingProductData[pushPricingProductDataRequest.OperatorId].Clear();

                                foreach (var pricingProductDataRecord in pushPricingProductDataRequest.PricingProductDataRecords)
                                    PricingProductData[pushPricingProductDataRequest.OperatorId].Add(pricingProductDataRecord);

                            }

                        }
                        break;

                    case ActionTypes.Update: {

                            // Update NOT Insert

                            if (!PricingProductData.ContainsKey(pushPricingProductDataRequest.OperatorId))
                                PricingProductData.Add(pushPricingProductDataRequest.OperatorId, new HashSet<PricingProductDataRecord>(pushPricingProductDataRequest.PricingProductDataRecords));

                            else
                            {

                                PartnerProduct_Id? missing = default;

                                var allEVSEIds = PricingProductData[pushPricingProductDataRequest.OperatorId].Select(pricingProductDataRecord => pricingProductDataRecord.ProductId).ToHashSet();

                                foreach (var pricingProductDataRecord in pushPricingProductDataRequest.PricingProductDataRecords)
                                {

                                    missing = pushPricingProductDataRequest.PricingProductDataRecords.FirstOrDefault(pricingProductDataRecord => !allEVSEIds.Contains(pricingProductDataRecord.ProductId))?.ProductId;

                                    if (missing.HasValue)
                                        return Task.FromResult(
                                                    OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                                        pushPricingProductDataRequest,
                                                        Acknowledgement<PushPricingProductDataRequest>.DataError(
                                                            Request:                    pushPricingProductDataRequest,
                                                            StatusCodeDescription:     "EVSE pricing product data record for update not found: " + missing.Value.ToString(),
                                                            StatusCodeAdditionalInfo:   null,
                                                            ResponseTimestamp:          Timestamp.Now,
                                                            EventTrackingId:            EventTracking_Id.New,
                                                            Runtime:                    TimeSpan.FromMilliseconds(2),
                                                            ProcessId:                  processId,
                                                            HTTPResponse:               null,
                                                            CustomData:                 null
                                                        ),
                                                        processId
                                                    )
                                                );

                                }

                                foreach (var pricingProductDataRecord in pushPricingProductDataRequest.PricingProductDataRecords)
                                    PricingProductData[pushPricingProductDataRequest.OperatorId].Add(pricingProductDataRecord);
                            }

                        }
                        break;

                    case ActionTypes.Insert: {

                            // Will fail if the EVSE data record already exists!

                            if (!PricingProductData.ContainsKey(pushPricingProductDataRequest.OperatorId))
                                PricingProductData.Add(pushPricingProductDataRequest.OperatorId, new HashSet<PricingProductDataRecord>(pushPricingProductDataRequest.PricingProductDataRecords));

                            else
                            {

                                PartnerProduct_Id? duplicate = default;

                                var allEVSEIds = PricingProductData[pushPricingProductDataRequest.OperatorId].Select(pricingProductDataRecord => pricingProductDataRecord.ProductId).ToHashSet();

                                foreach (var pricingProductDataRecord in pushPricingProductDataRequest.PricingProductDataRecords)
                                {

                                    duplicate = pushPricingProductDataRequest.PricingProductDataRecords.FirstOrDefault(pricingProductDataRecord => allEVSEIds.Contains(pricingProductDataRecord.ProductId))?.ProductId;

                                    if (duplicate.HasValue)
                                        return Task.FromResult(
                                                    OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                                        pushPricingProductDataRequest,
                                                        Acknowledgement<PushPricingProductDataRequest>.DataError(
                                                            Request:                    pushPricingProductDataRequest,
                                                            StatusCodeDescription:     "EVSE pricing product data record '" + duplicate.Value.ToString() + "' already exists!'",
                                                            StatusCodeAdditionalInfo:   null,
                                                            ResponseTimestamp:          Timestamp.Now,
                                                            EventTrackingId:            EventTracking_Id.New,
                                                            Runtime:                    TimeSpan.FromMilliseconds(2),
                                                            ProcessId:                  processId,
                                                            HTTPResponse:               null,
                                                            CustomData:                 null
                                                        ),
                                                        processId
                                                    )
                                                );

                                }

                                foreach (var pricingProductDataRecord in pushPricingProductDataRequest.PricingProductDataRecords)
                                    PricingProductData[pushPricingProductDataRequest.OperatorId].Add(pricingProductDataRecord);

                            }

                        }
                        break;

                    case ActionTypes.Delete: {

                            if (PricingProductData.ContainsKey(pushPricingProductDataRequest.OperatorId)) {

                                foreach (var pricingProductDataRecord in pushPricingProductDataRequest.PricingProductDataRecords)
                                    PricingProductData[pushPricingProductDataRequest.OperatorId].Remove(pricingProductDataRecord);

                            }

                        }
                        break;

                }

                return Task.FromResult(
                           new OICPResult<Acknowledgement<PushPricingProductDataRequest>>(
                               pushPricingProductDataRequest,
                               new Acknowledgement<PushPricingProductDataRequest>(
                                   Request:             pushPricingProductDataRequest,
                                   ResponseTimestamp:   Timestamp.Now,
                                   EventTrackingId:     EventTracking_Id.New,
                                   Runtime:             TimeSpan.FromMilliseconds(2),
                                   StatusCode:          new StatusCode(
                                                            StatusCodes.Success
                                                        ),
                                   HTTPResponse:        null,
                                   Result:              true,
                                   ProcessId:           processId,
                                   CustomData:          null
                               ),
                               true,
                               null,
                               processId));

            };

            centralServiceAPI.CPOClientAPI.OnPushEVSEPricing                 +=       (timestamp, cpoClientAPI, pushEVSEPricingRequest)        => {

                var processId = Process_Id.NewRandom();

                switch (pushEVSEPricingRequest.Action)
                {

                    case ActionTypes.FullLoad: {

                            if (!EVSEPricings.ContainsKey(pushEVSEPricingRequest.OperatorId))
                                EVSEPricings.Add(pushEVSEPricingRequest.OperatorId, new HashSet<EVSEPricing>(pushEVSEPricingRequest.EVSEPricing));

                            else
                            {

                                EVSEPricings[pushEVSEPricingRequest.OperatorId].Clear();

                                foreach (var evsePricing in pushEVSEPricingRequest.EVSEPricing)
                                    EVSEPricings[pushEVSEPricingRequest.OperatorId].Add(evsePricing);

                            }

                        }
                        break;

                    case ActionTypes.Update: {

                            // Update NOT Insert

                            if (!EVSEPricings.ContainsKey(pushEVSEPricingRequest.OperatorId))
                                EVSEPricings.Add(pushEVSEPricingRequest.OperatorId, new HashSet<EVSEPricing>(pushEVSEPricingRequest.EVSEPricing));

                            else
                            {

                                EVSE_Id? missing = default;

                                var allEVSEIds = EVSEPricings[pushEVSEPricingRequest.OperatorId].Select(evsePricing => evsePricing.EVSEId).ToHashSet();

                                foreach (var evsePricing in pushEVSEPricingRequest.EVSEPricing)
                                {

                                    missing = pushEVSEPricingRequest.EVSEPricing.FirstOrDefault(evsePricing => !allEVSEIds.Contains(evsePricing.EVSEId))?.EVSEId;

                                    if (missing.HasValue)
                                        return Task.FromResult(
                                                    OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                                        pushEVSEPricingRequest,
                                                        Acknowledgement<PushEVSEPricingRequest>.DataError(
                                                            Request:                    pushEVSEPricingRequest,
                                                            StatusCodeDescription:     "EVSE pricing record for update not found: " + missing.Value.ToString(),
                                                            StatusCodeAdditionalInfo:   null,
                                                            ResponseTimestamp:          Timestamp.Now,
                                                            EventTrackingId:            EventTracking_Id.New,
                                                            Runtime:                    TimeSpan.FromMilliseconds(2),
                                                            ProcessId:                  processId,
                                                            HTTPResponse:               null,
                                                            CustomData:                 null
                                                        ),
                                                        processId
                                                    )
                                                );

                                }

                                foreach (var evsePricing in pushEVSEPricingRequest.EVSEPricing)
                                    EVSEPricings[pushEVSEPricingRequest.OperatorId].Add(evsePricing);
                            }

                        }
                        break;

                    case ActionTypes.Insert: {

                            // Will fail if the EVSE data record already exists!

                            if (!EVSEPricings.ContainsKey(pushEVSEPricingRequest.OperatorId))
                                EVSEPricings.Add(pushEVSEPricingRequest.OperatorId, new HashSet<EVSEPricing>(pushEVSEPricingRequest.EVSEPricing));

                            else
                            {

                                EVSE_Id? duplicate = default;

                                var allEVSEIds = EVSEPricings[pushEVSEPricingRequest.OperatorId].Select(evsePricing => evsePricing.EVSEId).ToHashSet();

                                foreach (var evsePricing in pushEVSEPricingRequest.EVSEPricing)
                                {

                                    duplicate = pushEVSEPricingRequest.EVSEPricing.FirstOrDefault(evsePricing => allEVSEIds.Contains(evsePricing.EVSEId))?.EVSEId;

                                    if (duplicate.HasValue)
                                        return Task.FromResult(
                                                    OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                                        pushEVSEPricingRequest,
                                                        Acknowledgement<PushEVSEPricingRequest>.DataError(
                                                            Request:                    pushEVSEPricingRequest,
                                                            StatusCodeDescription:     "EVSE pricing record '" + duplicate.Value.ToString() + "' already exists!'",
                                                            StatusCodeAdditionalInfo:   null,
                                                            ResponseTimestamp:          Timestamp.Now,
                                                            EventTrackingId:            EventTracking_Id.New,
                                                            Runtime:                    TimeSpan.FromMilliseconds(2),
                                                            ProcessId:                  processId,
                                                            HTTPResponse:               null,
                                                            CustomData:                 null
                                                        ),
                                                        processId
                                                    )
                                                );

                                }

                                foreach (var evsePricing in pushEVSEPricingRequest.EVSEPricing)
                                    EVSEPricings[pushEVSEPricingRequest.OperatorId].Add(evsePricing);

                            }

                        }
                        break;

                    case ActionTypes.Delete: {

                            if (EVSEPricings.ContainsKey(pushEVSEPricingRequest.OperatorId)) {

                                foreach (var evsePricing in pushEVSEPricingRequest.EVSEPricing)
                                    EVSEPricings[pushEVSEPricingRequest.OperatorId].Remove(evsePricing);

                            }

                        }
                        break;

                }

                return Task.FromResult(
                           new OICPResult<Acknowledgement<PushEVSEPricingRequest>>(
                               pushEVSEPricingRequest,
                               new Acknowledgement<PushEVSEPricingRequest>(
                                   Request:             pushEVSEPricingRequest,
                                   ResponseTimestamp:   Timestamp.Now,
                                   EventTrackingId:     EventTracking_Id.New,
                                   Runtime:             TimeSpan.FromMilliseconds(2),
                                   StatusCode:          new StatusCode(
                                                            StatusCodes.Success
                                                        ),
                                   HTTPResponse:        null,
                                   Result:              true,
                                   ProcessId:           processId,
                                   CustomData:          null
                               ),
                               true,
                               null,
                               processId));

            };


            centralServiceAPI.CPOClientAPI.OnAuthorizeStart                  += async (timestamp, cpoClientAPI, authorizeStartRequest) => {

                var processId = Process_Id.NewRandom();

                if (centralServiceAPI.EMPServerAPIClients.Count != 0)
                {

                    var broadcastToAll  = centralServiceAPI.EMPServerAPIClients.Values.
                                              Select(cli => cli.AuthorizeStart(authorizeStartRequest)).
                                              ToArray();

                    var responses       = await Task.WhenAll(broadcastToAll);

                    var success         = responses.Where(response => response          is not null &&
                                                                      response.IsSuccessful &&
                                                                      response.Response is not null &&
                                                                      response.Response.AuthorizationStatus == AuthorizationStatusTypes.Authorized).
                                                    ToArray();

                    if (success.Length != 0)
                        return success.First();

                }

                return OICPResult<AuthorizationStartResponse>.Failed(
                           authorizeStartRequest,
                           AuthorizationStartResponse.NotAuthorized(
                               Request:               authorizeStartRequest,
                               StatusCode:            new StatusCode(
                                                           StatusCodes.NoValidContract,
                                                           "No positive response from any connected e-mobility provider!"
                                                       ),
                               CPOPartnerSessionId:   authorizeStartRequest.CPOPartnerSessionId,
                               ResponseTimestamp:     Timestamp.Now,
                               EventTrackingId:       authorizeStartRequest.EventTrackingId,
                               Runtime:               TimeSpan.FromMilliseconds(23),
                               ProcessId:             processId,
                               CustomData:            null
                           )
                       );

            };

            centralServiceAPI.CPOClientAPI.OnAuthorizeStop                   += async (timestamp, cpoClientAPI, authorizeStopRequest)  => {

                var processId = Process_Id.NewRandom();

                if (centralServiceAPI.EMPServerAPIClients.Count != 0)
                {

                    var broadcastToAll  = centralServiceAPI.EMPServerAPIClients.Values.
                                              Select(cli => cli.AuthorizeStop(authorizeStopRequest)).
                                              ToArray();

                    var responses       = await Task.WhenAll(broadcastToAll);

                    var success         = responses.Where(response => response          is not null &&
                                                                      response.IsSuccessful &&
                                                                      response.Response is not null &&
                                                                      response.Response.AuthorizationStatus == AuthorizationStatusTypes.Authorized).
                                                    ToArray();

                    if (success.Length != 0)
                        return success.First();

                }

                return OICPResult<AuthorizationStopResponse>.Failed(
                           authorizeStopRequest,
                           AuthorizationStopResponse.NotAuthorized(
                               Request:               authorizeStopRequest,
                               StatusCode:            new StatusCode(
                                                           StatusCodes.NoValidContract,
                                                           "No positive response from any connected e-mobility provider!"
                                                       ),
                               CPOPartnerSessionId:   authorizeStopRequest.CPOPartnerSessionId,
                               ResponseTimestamp:     Timestamp.Now,
                               EventTrackingId:       authorizeStopRequest.EventTrackingId,
                               Runtime:               TimeSpan.FromMilliseconds(23),
                               ProcessId:             processId,
                               CustomData:            null
                           )
                       );

            };


            centralServiceAPI.CPOClientAPI.OnChargingStartNotification       +=       (timestamp, cpoClientAPI, chargingStartNotificationRequest)    => {

                var processId = Process_Id.NewRandom();

                return Task.FromResult(
                           new OICPResult<Acknowledgement<ChargingStartNotificationRequest>>(
                               chargingStartNotificationRequest,
                               new Acknowledgement<ChargingStartNotificationRequest>(
                                   Request:             chargingStartNotificationRequest,
                                   ResponseTimestamp:   Timestamp.Now,
                                   EventTrackingId:     EventTracking_Id.New,
                                   Runtime:             TimeSpan.FromMilliseconds(2),
                                   StatusCode:          new StatusCode(
                                                            StatusCodes.Success
                                                        ),
                                   HTTPResponse:        null,
                                   Result:              true,
                                   ProcessId:           processId,
                                   CustomData:          null
                               ),
                               true,
                               null,
                               processId));

            };

            centralServiceAPI.CPOClientAPI.OnChargingProgressNotification    +=       (timestamp, cpoClientAPI, chargingProgressNotificationRequest) => {

                var processId = Process_Id.NewRandom();

                return Task.FromResult(
                           new OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>(
                               chargingProgressNotificationRequest,
                               new Acknowledgement<ChargingProgressNotificationRequest>(
                                   Request:             chargingProgressNotificationRequest,
                                   ResponseTimestamp:   Timestamp.Now,
                                   EventTrackingId:     EventTracking_Id.New,
                                   Runtime:             TimeSpan.FromMilliseconds(2),
                                   StatusCode:          new StatusCode(
                                                            StatusCodes.Success
                                                        ),
                                   HTTPResponse:        null,
                                   Result:              true,
                                   ProcessId:           processId,
                                   CustomData:          null
                               ),
                               true,
                               null,
                               processId));

            };

            centralServiceAPI.CPOClientAPI.OnChargingEndNotification         +=       (timestamp, cpoClientAPI, chargingEndNotificationRequest)      => {

                var processId = Process_Id.NewRandom();

                return Task.FromResult(
                           new OICPResult<Acknowledgement<ChargingEndNotificationRequest>>(
                               chargingEndNotificationRequest,
                               new Acknowledgement<ChargingEndNotificationRequest>(
                                   Request:             chargingEndNotificationRequest,
                                   ResponseTimestamp:   Timestamp.Now,
                                   EventTrackingId:     EventTracking_Id.New,
                                   Runtime:             TimeSpan.FromMilliseconds(2),
                                   StatusCode:          new StatusCode(
                                                            StatusCodes.Success
                                                        ),
                                   HTTPResponse:        null,
                                   Result:              true,
                                   ProcessId:           processId,
                                   CustomData:          null
                               ),
                               true,
                               null,
                               processId));

            };

            centralServiceAPI.CPOClientAPI.OnChargingErrorNotification       +=       (timestamp, cpoClientAPI, chargingErrorNotificationRequest)    => {

                var processId = Process_Id.NewRandom();

                return Task.FromResult(
                           new OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>(
                               chargingErrorNotificationRequest,
                               new Acknowledgement<ChargingErrorNotificationRequest>(
                                   Request:             chargingErrorNotificationRequest,
                                   ResponseTimestamp:   Timestamp.Now,
                                   EventTrackingId:     EventTracking_Id.New,
                                   Runtime:             TimeSpan.FromMilliseconds(2),
                                   StatusCode:          new StatusCode(
                                                            StatusCodes.Success
                                                        ),
                                   HTTPResponse:        null,
                                   Result:              true,
                                   ProcessId:           processId,
                                   CustomData:          null
                               ),
                               true,
                               null,
                               processId));

            };


            centralServiceAPI.CPOClientAPI.OnChargeDetailRecord              += async (timestamp, cpoClientAPI, chargeDetailRecordRequest) => {

                var processId = Process_Id.NewRandom();

                // Probably Hubject uses the session id here instead...
                if (chargeDetailRecordRequest.ChargeDetailRecord.HubProviderId.HasValue &&
                    centralServiceAPI.EMPServerAPIClients.TryGetValue(chargeDetailRecordRequest.ChargeDetailRecord.HubProviderId.Value,
                                                                      out EMPServerAPIClient? empServerAPIClient) &&
                    empServerAPIClient is not null)
                {

                    var response = await empServerAPIClient.SendChargeDetailRecord(chargeDetailRecordRequest);

                    return response;

                }

                return OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Success(
                           chargeDetailRecordRequest,
                           new Acknowledgement<ChargeDetailRecordRequest>(
                               ResponseTimestamp:     Timestamp.Now,
                               EventTrackingId:       chargeDetailRecordRequest.EventTrackingId ?? EventTracking_Id.New,
                               ProcessId:             processId,
                               Runtime:               TimeSpan.FromMilliseconds(23),
                               StatusCode:            new StatusCode(StatusCodes.Success),
                               Request:               chargeDetailRecordRequest,
                               HTTPResponse:          null,
                               Result:                true,
                               SessionId:             Session_Id.NewRandom(),
                               CPOPartnerSessionId:   CPOPartnerSession_Id.NewRandom(),
                               EMPPartnerSessionId:   EMPPartnerSession_Id.NewRandom(),
                               CustomData:            null
                           )
                       );

            };

            #endregion

            #region EMPClientAPI delegates...

            centralServiceAPI.EMPClientAPI.OnPullEVSEData                    +=       (timestamp, empClientAPI, pullEVSEDataRequest)                    => {

                var processId = Process_Id.NewRandom();

                return Task.FromResult(
                           OICPResult<PullEVSEDataResponse>.Success(
                               pullEVSEDataRequest,
                               new PullEVSEDataResponse(
                                   ResponseTimestamp:   Timestamp.Now,
                                   EventTrackingId:     EventTracking_Id.New,
                                   ProcessId:           processId,
                                   Runtime:             TimeSpan.FromMilliseconds(23),
                                   EVSEDataRecords:     EVSEDataRecords.ContainsKey(Operator_Id.Parse("DE*GEF"))
                                                            ? EVSEDataRecords[Operator_Id.Parse("DE*GEF")]
                                                            : Array.Empty<EVSEDataRecord>(),
                                   Request:             pullEVSEDataRequest,
                                   StatusCode:          new StatusCode(
                                                            StatusCodes.Success
                                                        ),
                                   HTTPResponse:        null,
                                   CustomData:          null,
                                   Warnings:            null
                               ),
                               processId
                           )
                       );

            };

            centralServiceAPI.EMPClientAPI.OnPullEVSEStatus                  +=       (timestamp, empClientAPI, pullEVSEStatusRequest)                  => {

                var processId = Process_Id.NewRandom();

                return Task.FromResult(
                           OICPResult<PullEVSEStatusResponse>.Success(
                               pullEVSEStatusRequest,
                               new PullEVSEStatusResponse(
                                   ResponseTimestamp:   Timestamp.Now,
                                   EventTrackingId:     EventTracking_Id.New,
                                   ProcessId:           processId,
                                   Runtime:             TimeSpan.FromMilliseconds(23),
                                   [
                                       new OperatorEVSEStatus(EVSEDataRecords.ContainsKey(Operator_Id.Parse("DE*GEF"))
                                                                  ? EVSEStatusRecords[Operator_Id.Parse("DE*GEF")]
                                                                  : Array.Empty<EVSEStatusRecord>(),
                                                              Operator_Id.Parse("DE*GEF"),
                                                              "GraphDefined")
                                   ],
                                   Request:             pullEVSEStatusRequest,
                                   StatusCode:          new StatusCode(
                                                            StatusCodes.Success
                                                        ),
                                   HTTPResponse:        null,
                                   CustomData:          null
                               ),
                               processId
                           )
                       );

            };

            centralServiceAPI.EMPClientAPI.OnPullEVSEStatusById              +=       (timestamp, empClientAPI, pullEVSEStatusByIdRequest)              => {

                var processId = Process_Id.NewRandom();

                return Task.FromResult(
                           OICPResult<PullEVSEStatusByIdResponse>.Success(
                               pullEVSEStatusByIdRequest,
                               new PullEVSEStatusByIdResponse(
                                   ResponseTimestamp:   Timestamp.Now,
                                   EventTrackingId:     EventTracking_Id.New,
                                   ProcessId:           processId,
                                   Runtime:             TimeSpan.FromMilliseconds(23),
                                   EVSEStatusRecords:   [],
                                   Request:             pullEVSEStatusByIdRequest,
                                   StatusCode:          new StatusCode(
                                                            StatusCodes.Success
                                                        ),
                                   HTTPResponse:        null,
                                   CustomData:          null
                               ),
                               processId
                           )
                       );

            };

            centralServiceAPI.EMPClientAPI.OnPullEVSEStatusByOperatorId      +=       (timestamp, empClientAPI, pullEVSEStatusByOperatorIdRequest)      => {

                var processId = Process_Id.NewRandom();

                return Task.FromResult(
                           OICPResult<PullEVSEStatusByOperatorIdResponse>.Success(
                               pullEVSEStatusByOperatorIdRequest,
                               new PullEVSEStatusByOperatorIdResponse(
                                   ResponseTimestamp:    Timestamp.Now,
                                   EventTrackingId:      EventTracking_Id.New,
                                   ProcessId:            processId,
                                   Runtime:              TimeSpan.FromMilliseconds(23),
                                   OperatorEVSEStatus:   [],
                                   Request:              pullEVSEStatusByOperatorIdRequest,
                                   StatusCode:           new StatusCode(
                                                             StatusCodes.Success
                                                         ),
                                   HTTPResponse:         null,
                                   CustomData:           null
                               ),
                               processId
                           )
                       );

            };


            centralServiceAPI.EMPClientAPI.OnPullPricingProductData          +=       (timestamp, empClientAPI, pullPricingProductDataRequest)          => {

                var processId = Process_Id.NewRandom();

                return Task.FromResult(
                           OICPResult<PullPricingProductDataResponse>.Success(
                               pullPricingProductDataRequest,
                               new PullPricingProductDataResponse(
                                   ResponseTimestamp:    Timestamp.Now,
                                   EventTrackingId:      EventTracking_Id.New,
                                   ProcessId:            processId,
                                   Runtime:              TimeSpan.FromMilliseconds(23),
                                   PricingProductData:   [
                                                             new PricingProductData(
                                                                 Operator_Id.Parse("DE*GEF"),
                                                                 pullPricingProductDataRequest.ProviderId,
                                                                 1.2M,
                                                                 Currency_Id.EUR,
                                                                 Reference_Unit.KILOWATT_HOUR,
                                                                 PricingProductData.ContainsKey(Operator_Id.Parse("DE*GEF"))
                                                                     ? PricingProductData[Operator_Id.Parse("DE*GEF")]
                                                                     : Array.Empty<PricingProductDataRecord>(),
                                                                 "GraphDefined"
                                                             )
                                                         ],
                                   Request:              pullPricingProductDataRequest,
                                   StatusCode:           new StatusCode(
                                                             StatusCodes.Success
                                                         ),
                                   HTTPResponse:         null,
                                   CustomData:           null,
                                   Warnings:             null
                               ),
                               processId
                           )
                       );

            };

            centralServiceAPI.EMPClientAPI.OnPullEVSEPricing                 +=       (timestamp, empClientAPI, pullEVSEPricingRequest)                 => {

                var processId = Process_Id.NewRandom();

                return Task.FromResult(
                           OICPResult<PullEVSEPricingResponse>.Success(
                               pullEVSEPricingRequest,
                               new PullEVSEPricingResponse(
                                   ResponseTimestamp:     Timestamp.Now,
                                   EventTrackingId:       EventTracking_Id.New,
                                   ProcessId:             processId,
                                   Runtime:               TimeSpan.FromMilliseconds(23),
                                   OperatorEVSEPricings:  [
                                                              new OperatorEVSEPricing(
                                                                  EVSEPricings.ContainsKey(Operator_Id.Parse("DE*GEF"))
                                                                      ? EVSEPricings[Operator_Id.Parse("DE*GEF")]
                                                                      : Array.Empty<EVSEPricing>(),
                                                                  Operator_Id.Parse("DE*GEF"),
                                                                  "GraphDefined"
                                                              )
                                                          ],
                                   Request:               pullEVSEPricingRequest,
                                   StatusCode:            new StatusCode(
                                                              StatusCodes.Success
                                                          ),
                                   HTTPResponse:          null,
                                   CustomData:            null,
                                   Warnings:              null
                               ),
                               processId
                           )
                       );

            };


            centralServiceAPI.EMPClientAPI.OnPushAuthenticationData          +=       (timestamp, empClientAPI, pushAuthenticationDataRequest)          => {

                return Task.FromResult(
                    new OICPResult<Acknowledgement<PushAuthenticationDataRequest>>(
                        pushAuthenticationDataRequest,
                        new Acknowledgement<PushAuthenticationDataRequest>(
                            Request:               pushAuthenticationDataRequest,
                            ResponseTimestamp:     Timestamp.Now,
                            EventTrackingId:       EventTracking_Id.New,
                            Runtime:               TimeSpan.FromMilliseconds(2),
                            StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                            HTTPResponse:          null,
                            Result:                false,
                            ProcessId:             Process_Id.NewRandom(),
                            CustomData:            null),
                        false));

            };


            centralServiceAPI.EMPClientAPI.OnAuthorizeRemoteReservationStart += async (timestamp, empClientAPI, authorizeRemoteReservationStartRequest) => {

                var processId = Process_Id.NewRandom();

                if (centralServiceAPI.CPOServerAPIClients.TryGetValue(authorizeRemoteReservationStartRequest.EVSEId.OperatorId,
                                                                      out CPOServerAPIClient? cpoServerAPIClient) &&
                    cpoServerAPIClient is not null)
                {

                    return await cpoServerAPIClient.AuthorizeRemoteReservationStart(
                                     new AuthorizeRemoteReservationStartRequest(
                                         authorizeRemoteReservationStartRequest.ProviderId,
                                         authorizeRemoteReservationStartRequest.EVSEId,
                                         authorizeRemoteReservationStartRequest.Identification,
                                         authorizeRemoteReservationStartRequest.SessionId,
                                         authorizeRemoteReservationStartRequest.CPOPartnerSessionId,
                                         authorizeRemoteReservationStartRequest.EMPPartnerSessionId,
                                         authorizeRemoteReservationStartRequest.PartnerProductId,
                                         authorizeRemoteReservationStartRequest.Duration,
                                         processId,
                                         authorizeRemoteReservationStartRequest.CustomData,
                                         authorizeRemoteReservationStartRequest.Timestamp,
                                         authorizeRemoteReservationStartRequest.EventTrackingId,
                                         TimeSpan.FromSeconds(10),
                                         authorizeRemoteReservationStartRequest.CancellationToken
                                     )
                                 );

                }

                return OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(
                           authorizeRemoteReservationStartRequest,
                           Acknowledgement<AuthorizeRemoteReservationStartRequest>.NoValidContract(
                               Request:                   authorizeRemoteReservationStartRequest,
                               SessionId:                 authorizeRemoteReservationStartRequest.SessionId,
                               CPOPartnerSessionId:       null,
                               EMPPartnerSessionId:       authorizeRemoteReservationStartRequest.EMPPartnerSessionId,
                               StatusCodeDescription:     "The charge point operator '" + authorizeRemoteReservationStartRequest.EVSEId.OperatorId.ToString() + "' is unknown!",
                               StatusCodeAdditionalInfo:  null,
                               ResponseTimestamp:         Timestamp.Now,
                               EventTrackingId:           authorizeRemoteReservationStartRequest.EventTrackingId,
                               Runtime:                   TimeSpan.FromMilliseconds(23),
                               ProcessId:                 processId,
                               HTTPResponse:              null,
                               CustomData:                null
                           ),
                           ProcessId:  processId
                       );

            };

            centralServiceAPI.EMPClientAPI.OnAuthorizeRemoteReservationStop  += async (timestamp, empClientAPI, authorizeRemoteReservationStopRequest)  => {

                var processId = Process_Id.NewRandom();

                if (centralServiceAPI.CPOServerAPIClients.TryGetValue(authorizeRemoteReservationStopRequest.EVSEId.OperatorId,
                                                                      out CPOServerAPIClient? cpoServerAPIClient) &&
                    cpoServerAPIClient is not null)
                {

                    return await cpoServerAPIClient.AuthorizeRemoteReservationStop(
                                     new AuthorizeRemoteReservationStopRequest(
                                         authorizeRemoteReservationStopRequest.ProviderId,
                                         authorizeRemoteReservationStopRequest.EVSEId,
                                         authorizeRemoteReservationStopRequest.SessionId,
                                         authorizeRemoteReservationStopRequest.CPOPartnerSessionId,
                                         authorizeRemoteReservationStopRequest.EMPPartnerSessionId,
                                         processId,
                                         authorizeRemoteReservationStopRequest.CustomData,
                                         authorizeRemoteReservationStopRequest.Timestamp,
                                         authorizeRemoteReservationStopRequest.EventTrackingId,
                                         TimeSpan.FromSeconds(10),
                                         authorizeRemoteReservationStopRequest.CancellationToken
                                     )
                                 );

                }

                return OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(
                           authorizeRemoteReservationStopRequest,
                           Acknowledgement<AuthorizeRemoteReservationStopRequest>.NoValidContract(
                               Request:                   authorizeRemoteReservationStopRequest,
                               SessionId:                 authorizeRemoteReservationStopRequest.SessionId,
                               CPOPartnerSessionId:       null,
                               EMPPartnerSessionId:       authorizeRemoteReservationStopRequest.EMPPartnerSessionId,
                               StatusCodeDescription:     "The charge point operator '" + authorizeRemoteReservationStopRequest.EVSEId.OperatorId.ToString() + "' is unknown!",
                               StatusCodeAdditionalInfo:  null,
                               ResponseTimestamp:         Timestamp.Now,
                               EventTrackingId:           authorizeRemoteReservationStopRequest.EventTrackingId,
                               Runtime:                   TimeSpan.FromMilliseconds(23),
                               ProcessId:                 processId,
                               HTTPResponse:              null,
                               CustomData:                null
                           ),
                           ProcessId:  processId
                       );

            };


            centralServiceAPI.EMPClientAPI.OnAuthorizeRemoteStart            += async (timestamp, empClientAPI, authorizeRemoteStartRequest)            => {

                var processId = Process_Id.NewRandom();

                if (centralServiceAPI.CPOServerAPIClients.TryGetValue(authorizeRemoteStartRequest.EVSEId.OperatorId,
                                                                      out CPOServerAPIClient? cpoServerAPIClient) &&
                    cpoServerAPIClient is not null)
                {

                    return await cpoServerAPIClient.AuthorizeRemoteStart(
                                     new AuthorizeRemoteStartRequest(
                                         authorizeRemoteStartRequest.ProviderId,
                                         authorizeRemoteStartRequest.EVSEId,
                                         authorizeRemoteStartRequest.Identification,
                                         authorizeRemoteStartRequest.SessionId,
                                         authorizeRemoteStartRequest.CPOPartnerSessionId,
                                         authorizeRemoteStartRequest.EMPPartnerSessionId,
                                         authorizeRemoteStartRequest.PartnerProductId,
                                         processId,
                                         authorizeRemoteStartRequest.CustomData,
                                         authorizeRemoteStartRequest.Timestamp,
                                         authorizeRemoteStartRequest.EventTrackingId,
                                         TimeSpan.FromSeconds(10),
                                         authorizeRemoteStartRequest.CancellationToken
                                     )
                                 );

                }

                return OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Failed(
                           authorizeRemoteStartRequest,
                           Acknowledgement<AuthorizeRemoteStartRequest>.NoValidContract(
                               Request:                   authorizeRemoteStartRequest,
                               SessionId:                 authorizeRemoteStartRequest.SessionId,
                               CPOPartnerSessionId:       null,
                               EMPPartnerSessionId:       authorizeRemoteStartRequest.EMPPartnerSessionId,
                               StatusCodeDescription:     "The charge point operator '" + authorizeRemoteStartRequest.EVSEId.OperatorId.ToString() + "' is unknown!",
                               StatusCodeAdditionalInfo:  null,
                               ResponseTimestamp:         Timestamp.Now,
                               EventTrackingId:           authorizeRemoteStartRequest.EventTrackingId,
                               Runtime:                   TimeSpan.FromMilliseconds(23),
                               ProcessId:                 processId,
                               HTTPResponse:              null,
                               CustomData:                null
                           ),
                           ProcessId:  processId
                       );

            };

            centralServiceAPI.EMPClientAPI.OnAuthorizeRemoteStop             += async (timestamp, empClientAPI, authorizeRemoteStopRequest)             => {

                var processId = Process_Id.NewRandom();

                if (centralServiceAPI.CPOServerAPIClients.TryGetValue(authorizeRemoteStopRequest.EVSEId.OperatorId,
                                                                      out CPOServerAPIClient? cpoServerAPIClient) &&
                    cpoServerAPIClient is not null)
                {

                    return await cpoServerAPIClient.AuthorizeRemoteStop(
                                     new AuthorizeRemoteStopRequest(
                                         authorizeRemoteStopRequest.ProviderId,
                                         authorizeRemoteStopRequest.EVSEId,
                                         authorizeRemoteStopRequest.SessionId,
                                         authorizeRemoteStopRequest.CPOPartnerSessionId,
                                         authorizeRemoteStopRequest.EMPPartnerSessionId,
                                         processId,
                                         authorizeRemoteStopRequest.CustomData,
                                         authorizeRemoteStopRequest.Timestamp,
                                         authorizeRemoteStopRequest.EventTrackingId,
                                         TimeSpan.FromSeconds(10),
                                         authorizeRemoteStopRequest.CancellationToken
                                     )
                                 );

                }

                return OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Failed(
                           authorizeRemoteStopRequest,
                           Acknowledgement<AuthorizeRemoteStopRequest>.NoValidContract(
                               Request:                   authorizeRemoteStopRequest,
                               SessionId:                 authorizeRemoteStopRequest.SessionId,
                               CPOPartnerSessionId:       null,
                               EMPPartnerSessionId:       authorizeRemoteStopRequest.EMPPartnerSessionId,
                               StatusCodeDescription:     "The charge point operator '" + authorizeRemoteStopRequest.EVSEId.OperatorId.ToString() + "' is unknown!",
                               StatusCodeAdditionalInfo:  null,
                               ResponseTimestamp:         Timestamp.Now,
                               EventTrackingId:           authorizeRemoteStopRequest.EventTrackingId,
                               Runtime:                   TimeSpan.FromMilliseconds(23),
                               ProcessId:                 processId,
                               HTTPResponse:              null,
                               CustomData:                null
                           ),
                           ProcessId:  processId
                       );

            };


            centralServiceAPI.EMPClientAPI.OnGetChargeDetailRecords          += async (timestamp, empClientAPI, getChargeDetailRecordsRequest)          => {

                var processId = Process_Id.NewRandom();

                return OICPResult<GetChargeDetailRecordsResponse>.Success(
                           getChargeDetailRecordsRequest,
                           new GetChargeDetailRecordsResponse(
                               Request:                   getChargeDetailRecordsRequest,
                               ResponseTimestamp:         Timestamp.Now,
                               EventTrackingId:           getChargeDetailRecordsRequest.EventTrackingId ?? EventTracking_Id.New,
                               Runtime:                   TimeSpan.FromMilliseconds(23),
                               ChargeDetailRecords:       [

                                                              new ChargeDetailRecord(
                                                                  SessionId:                       Session_Id.NewRandom(),
                                                                  EVSEId:                          EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                                                  Identification:                  Identification.FromUID(UID.Parse("AABBCCDD")),
                                                                  SessionStart:                    Timestamp.Now - TimeSpan.FromMinutes(60),
                                                                  SessionEnd:                      Timestamp.Now - TimeSpan.FromMinutes(10),
                                                                  ChargingStart:                   Timestamp.Now - TimeSpan.FromMinutes(50),
                                                                  ChargingEnd:                     Timestamp.Now - TimeSpan.FromMinutes(20),
                                                                  ConsumedEnergy:                  WattHour.ParseKWh(35),

                                                                  PartnerProductId:                PartnerProduct_Id.Parse("AC1"),
                                                                  CPOPartnerSessionId:             CPOPartnerSession_Id.NewRandom(),
                                                                  EMPPartnerSessionId:             EMPPartnerSession_Id.NewRandom(),
                                                                  MeterValueStart:                 WattHour.ParseKWh( 3),
                                                                  MeterValueEnd:                   WattHour.ParseKWh(38),
                                                                  MeterValuesInBetween:            [],
                                                                  SignedMeteringValues:            [],
                                                                  CalibrationLawVerificationInfo:  new CalibrationLawVerification(),
                                                                  HubOperatorId:                   Operator_Id.Parse("DE*GEF"),
                                                                  HubProviderId:                   Provider_Id.Parse("DE-GDF"),

                                                                  CustomData:                      null,
                                                                  InternalData:                    null
                                                              ),

                                                              new ChargeDetailRecord(
                                                                  SessionId:                       Session_Id.NewRandom(),
                                                                  EVSEId:                          EVSE_Id.Parse("DE*GEF*E1234567*A*2"),
                                                                  Identification:                  Identification.FromUID(UID.Parse("CCDDEEFFAABBCC")),
                                                                  SessionStart:                    Timestamp.Now - TimeSpan.FromMinutes(60),
                                                                  SessionEnd:                      Timestamp.Now - TimeSpan.FromMinutes(10),
                                                                  ChargingStart:                   Timestamp.Now - TimeSpan.FromMinutes(50),
                                                                  ChargingEnd:                     Timestamp.Now - TimeSpan.FromMinutes(20),
                                                                  ConsumedEnergy:                  WattHour.ParseKWh(35),

                                                                  PartnerProductId:                PartnerProduct_Id.Parse("AC3"),
                                                                  CPOPartnerSessionId:             CPOPartnerSession_Id.NewRandom(),
                                                                  EMPPartnerSessionId:             EMPPartnerSession_Id.NewRandom(),
                                                                  MeterValueStart:                 WattHour.ParseKWh( 3),
                                                                  MeterValueEnd:                   WattHour.ParseKWh(38),
                                                                  MeterValuesInBetween:            [],
                                                                  SignedMeteringValues:            [],
                                                                  CalibrationLawVerificationInfo:  new CalibrationLawVerification(),
                                                                  HubOperatorId:                   Operator_Id.Parse("DE*GEF"),
                                                                  HubProviderId:                   Provider_Id.Parse("DE-GDF"),

                                                                  CustomData:                      null,
                                                                  InternalData:                    null
                                                              )

                                                          ],
                               HTTPResponse:              null,
                               ProcessId:                 processId,
                               StatusCode:                null,
                               FirstPage:                 true,
                               LastPage:                  true,
                               Number:                    1,
                               NumberOfElements:          2,
                               Size:                      getChargeDetailRecordsRequest.Size,
                               TotalElements:             2,
                               TotalPages:                1,
                               CustomData:                null
                           ),
                           ProcessId:                 processId);

            };

            #endregion



            #region Register CPO "DE*GEF"

            cpoRoaming_DEGEF = new CPORoaming(

                                   new CPOClient(
                                       RemoteURL:        URL.Parse("http://127.0.0.1:6001"),
                                       RequestTimeout:   TimeSpan.FromSeconds(10)
                                   ),

                                   new CPOServerAPI(
                                       ExternalDNSName:  "open.charging.cloud",
                                       HTTPServerPort:   IPPort.Parse(7001),
                                       LoggingPath:      "tests",
                                       AutoStart:        true
                                   )

                               );

            ClassicAssert.IsNotNull(cpoRoaming_DEGEF);
            ClassicAssert.IsNotNull(cpoRoaming_DEGEF.CPOClient);
            ClassicAssert.IsNotNull(cpoRoaming_DEGEF.CPOServer);


            cpoRoaming_DEGEF.CPOServer.OnAuthorizeRemoteReservationStart += (timestamp, cpoServerAPI, authorizeRemoteReservationStartRequest) => {

                if (authorizeRemoteReservationStartRequest.Identification is not null)
                {

                    if (authorizeRemoteReservationStartRequest.Identification.RemoteIdentification is not null)
                    {
                        return authorizeRemoteReservationStartRequest.Identification.RemoteIdentification.ToString() switch
                        {

                            "DE-GDF-C12345678X" =>
                                Task.FromResult(
                                    new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                        Request:               authorizeRemoteReservationStartRequest,
                                        ResponseTimestamp:     Timestamp.Now,
                                        EventTrackingId:       EventTracking_Id.New,
                                        Runtime:               TimeSpan.FromMilliseconds(2),
                                        StatusCode:            new StatusCode(StatusCodes.Success),
                                        HTTPResponse:          null,
                                        Result:                true,
                                        SessionId:             authorizeRemoteReservationStartRequest.SessionId,
                                        CPOPartnerSessionId:   authorizeRemoteReservationStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:   authorizeRemoteReservationStartRequest.EMPPartnerSessionId,
                                        ProcessId:             Process_Id.NewRandom(),
                                        CustomData:            null)),

                            _ =>
                                Task.FromResult(
                                    new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                        Request:               authorizeRemoteReservationStartRequest,
                                        ResponseTimestamp:     Timestamp.Now,
                                        EventTrackingId:       EventTracking_Id.New,
                                        Runtime:               TimeSpan.FromMilliseconds(2),
                                        StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                        HTTPResponse:          null,
                                        Result:                false,
                                        SessionId:             authorizeRemoteReservationStartRequest.SessionId,
                                        CPOPartnerSessionId:   authorizeRemoteReservationStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:   authorizeRemoteReservationStartRequest.EMPPartnerSessionId,
                                        ProcessId:             Process_Id.NewRandom(),
                                        CustomData:            null))
                        };

                    }

                }

                return Task.FromResult(
                    new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                        Request:               authorizeRemoteReservationStartRequest,
                        ResponseTimestamp:     Timestamp.Now,
                        EventTrackingId:       EventTracking_Id.New,
                        Runtime:               TimeSpan.FromMilliseconds(2),
                        StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                        HTTPResponse:          null,
                        Result:                false,
                        SessionId:             authorizeRemoteReservationStartRequest.SessionId,
                        CPOPartnerSessionId:   authorizeRemoteReservationStartRequest.CPOPartnerSessionId,
                        EMPPartnerSessionId:   authorizeRemoteReservationStartRequest.EMPPartnerSessionId,
                        ProcessId:             Process_Id.NewRandom(),
                        CustomData:            null));

            };

            cpoRoaming_DEGEF.CPOServer.OnAuthorizeRemoteReservationStop  += (timestamp, cpoServerAPI, authorizeRemoteReservationStopRequest)  => {

                return authorizeRemoteReservationStopRequest.SessionId.ToString() switch {

                    "7e8f35a6-13c8-4b37-8099-b21323c83e85" =>
                        Task.FromResult(
                            new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                Request:               authorizeRemoteReservationStopRequest,
                                ResponseTimestamp:     Timestamp.Now,
                                EventTrackingId:       EventTracking_Id.New,
                                Runtime:               TimeSpan.FromMilliseconds(2),
                                StatusCode:            new StatusCode(StatusCodes.Success),
                                HTTPResponse:          null,
                                Result:                true,
                                SessionId:             authorizeRemoteReservationStopRequest.SessionId,
                                CPOPartnerSessionId:   authorizeRemoteReservationStopRequest.CPOPartnerSessionId,
                                EMPPartnerSessionId:   authorizeRemoteReservationStopRequest.EMPPartnerSessionId,
                                ProcessId:             Process_Id.NewRandom(),
                                CustomData:            null)),

                    _ =>
                        Task.FromResult(
                            new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                Request:               authorizeRemoteReservationStopRequest,
                                ResponseTimestamp:     Timestamp.Now,
                                EventTrackingId:       EventTracking_Id.New,
                                Runtime:               TimeSpan.FromMilliseconds(2),
                                StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                HTTPResponse:          null,
                                Result:                false,
                                SessionId:             authorizeRemoteReservationStopRequest.SessionId,
                                CPOPartnerSessionId:   authorizeRemoteReservationStopRequest.CPOPartnerSessionId,
                                EMPPartnerSessionId:   authorizeRemoteReservationStopRequest.EMPPartnerSessionId,
                                ProcessId:             Process_Id.NewRandom(),
                                CustomData:            null))
                };

            };


            cpoRoaming_DEGEF.CPOServer.OnAuthorizeRemoteStart            += (timestamp, cpoServerAPI, authorizeRemoteStartRequest) => {

                if (authorizeRemoteStartRequest.Identification is not null)
                {

                    if (authorizeRemoteStartRequest.Identification.RemoteIdentification is not null)
                    {
                        return authorizeRemoteStartRequest.Identification.RemoteIdentification.ToString() switch
                        {

                            "DE-GDF-C12345678X" =>
                                Task.FromResult(
                                    new Acknowledgement<AuthorizeRemoteStartRequest>(
                                        Request:               authorizeRemoteStartRequest,
                                        ResponseTimestamp:     Timestamp.Now,
                                        EventTrackingId:       EventTracking_Id.New,
                                        Runtime:               TimeSpan.FromMilliseconds(2),
                                        StatusCode:            new StatusCode(StatusCodes.Success),
                                        HTTPResponse:          null,
                                        Result:                true,
                                        SessionId:             authorizeRemoteStartRequest.SessionId,
                                        CPOPartnerSessionId:   authorizeRemoteStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:   authorizeRemoteStartRequest.EMPPartnerSessionId,
                                        ProcessId:             Process_Id.NewRandom(),
                                        CustomData:            null)),

                            _ =>
                                Task.FromResult(
                                    new Acknowledgement<AuthorizeRemoteStartRequest>(
                                        Request:               authorizeRemoteStartRequest,
                                        ResponseTimestamp:     Timestamp.Now,
                                        EventTrackingId:       EventTracking_Id.New,
                                        Runtime:               TimeSpan.FromMilliseconds(2),
                                        StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                        HTTPResponse:          null,
                                        Result:                false,
                                        SessionId:             authorizeRemoteStartRequest.SessionId,
                                        CPOPartnerSessionId:   authorizeRemoteStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:   authorizeRemoteStartRequest.EMPPartnerSessionId,
                                        ProcessId:             Process_Id.NewRandom(),
                                        CustomData:            null))
                        };

                    }

                }

                return Task.FromResult(
                    new Acknowledgement<AuthorizeRemoteStartRequest>(
                        Request:               authorizeRemoteStartRequest,
                        ResponseTimestamp:     Timestamp.Now,
                        EventTrackingId:       EventTracking_Id.New,
                        Runtime:               TimeSpan.FromMilliseconds(2),
                        StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                        HTTPResponse:          null,
                        Result:                false,
                        SessionId:             authorizeRemoteStartRequest.SessionId,
                        CPOPartnerSessionId:   authorizeRemoteStartRequest.CPOPartnerSessionId,
                        EMPPartnerSessionId:   authorizeRemoteStartRequest.EMPPartnerSessionId,
                        ProcessId:             Process_Id.NewRandom(),
                        CustomData:            null));

            };

            cpoRoaming_DEGEF.CPOServer.OnAuthorizeRemoteStop             += (timestamp, cpoServerAPI, authorizeRemoteStopRequest)  => {

                return authorizeRemoteStopRequest.SessionId.ToString() switch {

                    "7e8f35a6-13c8-4b37-8099-b21323c83e85" =>
                        Task.FromResult(
                            new Acknowledgement<AuthorizeRemoteStopRequest>(
                                Request:               authorizeRemoteStopRequest,
                                ResponseTimestamp:     Timestamp.Now,
                                EventTrackingId:       EventTracking_Id.New,
                                Runtime:               TimeSpan.FromMilliseconds(2),
                                StatusCode:            new StatusCode(StatusCodes.Success),
                                HTTPResponse:          null,
                                Result:                true,
                                SessionId:             authorizeRemoteStopRequest.SessionId,
                                CPOPartnerSessionId:   authorizeRemoteStopRequest.CPOPartnerSessionId,
                                EMPPartnerSessionId:   authorizeRemoteStopRequest.EMPPartnerSessionId,
                                ProcessId:             Process_Id.NewRandom(),
                                CustomData:            null)),

                    _ =>
                        Task.FromResult(
                            new Acknowledgement<AuthorizeRemoteStopRequest>(
                                Request:               authorizeRemoteStopRequest,
                                ResponseTimestamp:     Timestamp.Now,
                                EventTrackingId:       EventTracking_Id.New,
                                Runtime:               TimeSpan.FromMilliseconds(2),
                                StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                HTTPResponse:          null,
                                Result:                false,
                                SessionId:             authorizeRemoteStopRequest.SessionId,
                                CPOPartnerSessionId:   authorizeRemoteStopRequest.CPOPartnerSessionId,
                                EMPPartnerSessionId:   authorizeRemoteStopRequest.EMPPartnerSessionId,
                                ProcessId:             Process_Id.NewRandom(),
                                CustomData:            null))
                };

            };


            centralServiceAPI.CPOServerAPIClients.Add(Operator_Id.Parse("DE*GEF"),
                                                      new CPOServerAPIClient(
                                                          URL.Parse("http://127.0.0.1:7001"),
                                                          RequestTimeout: TimeSpan.FromSeconds(10)
                                                      ));

            #endregion

            #region Register CPO "DE*BDO"

            cpoRoaming_DEBDO = new CPORoaming(

                                   new CPOClient(
                                       RemoteURL:        URL.Parse("http://127.0.0.1:6001"),
                                       RequestTimeout:   TimeSpan.FromSeconds(10)
                                   ),

                                   new CPOServerAPI(
                                       ExternalDNSName:  "open.charging.cloud",
                                       HTTPServerPort:   IPPort.Parse(7002),
                                       LoggingPath:      "tests",
                                       AutoStart:        true
                                   )

                               );

            ClassicAssert.IsNotNull(cpoRoaming_DEBDO);
            ClassicAssert.IsNotNull(cpoRoaming_DEBDO.CPOClient);
            ClassicAssert.IsNotNull(cpoRoaming_DEBDO.CPOServer);


            cpoRoaming_DEBDO.CPOServer.OnAuthorizeRemoteReservationStart += (timestamp, cpoServerAPI, authorizeRemoteReservationStartRequest) => {

                if (authorizeRemoteReservationStartRequest.Identification is not null)
                {

                    if (authorizeRemoteReservationStartRequest.Identification.RemoteIdentification is not null)
                    {
                        return authorizeRemoteReservationStartRequest.Identification.RemoteIdentification.ToString() switch
                        {

                            "DE-GDF-C12345678X" =>
                                Task.FromResult(
                                    new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                        Request:               authorizeRemoteReservationStartRequest,
                                        ResponseTimestamp:     Timestamp.Now,
                                        EventTrackingId:       EventTracking_Id.New,
                                        Runtime:               TimeSpan.FromMilliseconds(2),
                                        StatusCode:            new StatusCode(StatusCodes.Success),
                                        HTTPResponse:          null,
                                        Result:                true,
                                        SessionId:             authorizeRemoteReservationStartRequest.SessionId,
                                        CPOPartnerSessionId:   authorizeRemoteReservationStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:   authorizeRemoteReservationStartRequest.EMPPartnerSessionId,
                                        ProcessId:             Process_Id.NewRandom(),
                                        CustomData:            null)),

                            _ =>
                                Task.FromResult(
                                    new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                        Request:               authorizeRemoteReservationStartRequest,
                                        ResponseTimestamp:     Timestamp.Now,
                                        EventTrackingId:       EventTracking_Id.New,
                                        Runtime:               TimeSpan.FromMilliseconds(2),
                                        StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                        HTTPResponse:          null,
                                        Result:                false,
                                        SessionId:             authorizeRemoteReservationStartRequest.SessionId,
                                        CPOPartnerSessionId:   authorizeRemoteReservationStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:   authorizeRemoteReservationStartRequest.EMPPartnerSessionId,
                                        ProcessId:             Process_Id.NewRandom(),
                                        CustomData:            null))
                        };

                    }

                }

                return Task.FromResult(
                    new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                        Request:               authorizeRemoteReservationStartRequest,
                        ResponseTimestamp:     Timestamp.Now,
                        EventTrackingId:       EventTracking_Id.New,
                        Runtime:               TimeSpan.FromMilliseconds(2),
                        StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                        HTTPResponse:          null,
                        Result:                false,
                        SessionId:             authorizeRemoteReservationStartRequest.SessionId,
                        CPOPartnerSessionId:   authorizeRemoteReservationStartRequest.CPOPartnerSessionId,
                        EMPPartnerSessionId:   authorizeRemoteReservationStartRequest.EMPPartnerSessionId,
                        ProcessId:             Process_Id.NewRandom(),
                        CustomData:            null));

            };

            cpoRoaming_DEBDO.CPOServer.OnAuthorizeRemoteReservationStop  += (timestamp, cpoServerAPI, authorizeRemoteReservationStopRequest)  => {

                return authorizeRemoteReservationStopRequest.SessionId.ToString() switch {

                    "7e8f35a6-13c8-4b37-8099-b21323c83e85" =>
                        Task.FromResult(
                            new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                Request:               authorizeRemoteReservationStopRequest,
                                ResponseTimestamp:     Timestamp.Now,
                                EventTrackingId:       EventTracking_Id.New,
                                Runtime:               TimeSpan.FromMilliseconds(2),
                                StatusCode:            new StatusCode(StatusCodes.Success),
                                HTTPResponse:          null,
                                Result:                true,
                                SessionId:             authorizeRemoteReservationStopRequest.SessionId,
                                CPOPartnerSessionId:   authorizeRemoteReservationStopRequest.CPOPartnerSessionId,
                                EMPPartnerSessionId:   authorizeRemoteReservationStopRequest.EMPPartnerSessionId,
                                ProcessId:             Process_Id.NewRandom(),
                                CustomData:            null)),

                    _ =>
                        Task.FromResult(
                            new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                Request:               authorizeRemoteReservationStopRequest,
                                ResponseTimestamp:     Timestamp.Now,
                                EventTrackingId:       EventTracking_Id.New,
                                Runtime:               TimeSpan.FromMilliseconds(2),
                                StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                HTTPResponse:          null,
                                Result:                false,
                                SessionId:             authorizeRemoteReservationStopRequest.SessionId,
                                CPOPartnerSessionId:   authorizeRemoteReservationStopRequest.CPOPartnerSessionId,
                                EMPPartnerSessionId:   authorizeRemoteReservationStopRequest.EMPPartnerSessionId,
                                ProcessId:             Process_Id.NewRandom(),
                                CustomData:            null))
                };

            };


            cpoRoaming_DEBDO.CPOServer.OnAuthorizeRemoteStart            += (timestamp, cpoServerAPI, authorizeRemoteStartRequest) => {

                if (authorizeRemoteStartRequest.Identification is not null)
                {

                    if (authorizeRemoteStartRequest.Identification.RemoteIdentification is not null)
                    {
                        return authorizeRemoteStartRequest.Identification.RemoteIdentification.ToString() switch
                        {

                            "DE-GDF-C12345678X" =>
                                Task.FromResult(
                                    new Acknowledgement<AuthorizeRemoteStartRequest>(
                                        Request:               authorizeRemoteStartRequest,
                                        ResponseTimestamp:     Timestamp.Now,
                                        EventTrackingId:       EventTracking_Id.New,
                                        Runtime:               TimeSpan.FromMilliseconds(2),
                                        StatusCode:            new StatusCode(StatusCodes.Success),
                                        HTTPResponse:          null,
                                        Result:                true,
                                        SessionId:             authorizeRemoteStartRequest.SessionId,
                                        CPOPartnerSessionId:   authorizeRemoteStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:   authorizeRemoteStartRequest.EMPPartnerSessionId,
                                        ProcessId:             Process_Id.NewRandom(),
                                        CustomData:            null)),

                            _ =>
                                Task.FromResult(
                                    new Acknowledgement<AuthorizeRemoteStartRequest>(
                                        Request:               authorizeRemoteStartRequest,
                                        ResponseTimestamp:     Timestamp.Now,
                                        EventTrackingId:       EventTracking_Id.New,
                                        Runtime:               TimeSpan.FromMilliseconds(2),
                                        StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                        HTTPResponse:          null,
                                        Result:                false,
                                        SessionId:             authorizeRemoteStartRequest.SessionId,
                                        CPOPartnerSessionId:   authorizeRemoteStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:   authorizeRemoteStartRequest.EMPPartnerSessionId,
                                        ProcessId:             Process_Id.NewRandom(),
                                        CustomData:            null))
                        };

                    }

                }

                return Task.FromResult(
                    new Acknowledgement<AuthorizeRemoteStartRequest>(
                        Request:               authorizeRemoteStartRequest,
                        ResponseTimestamp:     Timestamp.Now,
                        EventTrackingId:       EventTracking_Id.New,
                        Runtime:               TimeSpan.FromMilliseconds(2),
                        StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                        HTTPResponse:          null,
                        Result:                false,
                        SessionId:             authorizeRemoteStartRequest.SessionId,
                        CPOPartnerSessionId:   authorizeRemoteStartRequest.CPOPartnerSessionId,
                        EMPPartnerSessionId:   authorizeRemoteStartRequest.EMPPartnerSessionId,
                        ProcessId:             Process_Id.NewRandom(),
                        CustomData:            null));

            };

            cpoRoaming_DEBDO.CPOServer.OnAuthorizeRemoteStop             += (timestamp, cpoServerAPI, authorizeRemoteStopRequest)  => {

                return authorizeRemoteStopRequest.SessionId.ToString() switch {

                    "7e8f35a6-13c8-4b37-8099-b21323c83e85" =>
                        Task.FromResult(
                            new Acknowledgement<AuthorizeRemoteStopRequest>(
                                Request:               authorizeRemoteStopRequest,
                                ResponseTimestamp:     Timestamp.Now,
                                EventTrackingId:       EventTracking_Id.New,
                                Runtime:               TimeSpan.FromMilliseconds(2),
                                StatusCode:            new StatusCode(StatusCodes.Success),
                                HTTPResponse:          null,
                                Result:                true,
                                SessionId:             authorizeRemoteStopRequest.SessionId,
                                CPOPartnerSessionId:   authorizeRemoteStopRequest.CPOPartnerSessionId,
                                EMPPartnerSessionId:   authorizeRemoteStopRequest.EMPPartnerSessionId,
                                ProcessId:             Process_Id.NewRandom(),
                                CustomData:            null)),

                    _ =>
                        Task.FromResult(
                            new Acknowledgement<AuthorizeRemoteStopRequest>(
                                Request:               authorizeRemoteStopRequest,
                                ResponseTimestamp:     Timestamp.Now,
                                EventTrackingId:       EventTracking_Id.New,
                                Runtime:               TimeSpan.FromMilliseconds(2),
                                StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                HTTPResponse:          null,
                                Result:                false,
                                SessionId:             authorizeRemoteStopRequest.SessionId,
                                CPOPartnerSessionId:   authorizeRemoteStopRequest.CPOPartnerSessionId,
                                EMPPartnerSessionId:   authorizeRemoteStopRequest.EMPPartnerSessionId,
                                ProcessId:             Process_Id.NewRandom(),
                                CustomData:            null))
                };

            };


            centralServiceAPI.CPOServerAPIClients.Add(Operator_Id.Parse("DE*BDO"),
                                                      new CPOServerAPIClient(
                                                          URL.Parse("http://127.0.0.1:7002"),
                                                          RequestTimeout: TimeSpan.FromSeconds(10)
                                                      ));

            #endregion


            #region Register EMP "DE-GDF"

            empRoaming_DEGDF = new EMPRoaming(

                                   new EMPClient(
                                       RemoteURL:        URL.Parse("http://127.0.0.1:6001"),
                                       RequestTimeout:   TimeSpan.FromSeconds(10)
                                   ),

                                   new EMPServerAPI(
                                       ExternalDNSName:  "open.charging.cloud",
                                       HTTPServerPort:   IPPort.Parse(8001),
                                       LoggingPath:      "tests",
                                       AutoStart:        true
                                   )

                               );

            ClassicAssert.IsNotNull(cpoRoaming_DEGEF);
            ClassicAssert.IsNotNull(cpoRoaming_DEGEF.CPOClient);
            ClassicAssert.IsNotNull(cpoRoaming_DEGEF.CPOServer);


            empRoaming_DEGDF.OnAuthorizeStart     += (timestamp, empClientAPI, authorizeStartRequest)     => {

                if (authorizeStartRequest.Identification.RFIDId?.ToString() == "11223344")
                    return Task.FromResult(
                               AuthorizationStartResponse.Authorized(
                                   authorizeStartRequest,
                                   Session_Id.          Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"), // generated by Hubject!
                                   authorizeStartRequest.CPOPartnerSessionId,
                                   EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),
                                   Provider_Id.Parse("DE-GDF"),
                                   "Nice to see you!",
                                   "Hello world!",
                                   [
                                       Identification.FromUID(UID.Parse("11223344")),
                                       Identification.FromUID(UID.Parse("55667788"))
                                   ]
                               )
                           );

                return Task.FromResult(
                               AuthorizationStartResponse.NotAuthorized(
                                   Request:               authorizeStartRequest,
                                   StatusCode:            new StatusCode(
                                                              StatusCodes.NoPositiveAuthenticationResponse,
                                                              "Unknown RFID UID!"
                                                          ),
                                   CPOPartnerSessionId:   authorizeStartRequest.CPOPartnerSessionId,
                                   ProviderId:            Provider_Id.Parse("DE-GDF")
                               )
                           );

            };

            empRoaming_DEGDF.OnAuthorizeStop      += (timestamp, empClientAPI, authorizeStopRequest)      => {

                return Task.FromResult(
                           AuthorizationStopResponse.Authorized(
                               authorizeStopRequest,
                               authorizeStopRequest.SessionId,
                               authorizeStopRequest.CPOPartnerSessionId,
                               authorizeStopRequest.EMPPartnerSessionId,
                               Provider_Id.Parse("DE-GDF"),
                               "Have a nice day!",
                               "bye bye!"
                           )
                       );

            };


            empRoaming_DEGDF.OnChargeDetailRecord += (timestamp, cpoServerAPI, chargeDetailRecordRequest) => {

                return Task.FromResult(
                            new Acknowledgement<ChargeDetailRecordRequest>(
                                Request:             chargeDetailRecordRequest,
                                ResponseTimestamp:   Timestamp.Now,
                                EventTrackingId:     EventTracking_Id.New,
                                Runtime:             TimeSpan.FromMilliseconds(2),
                                StatusCode:          new StatusCode(
                                                         StatusCodes.Success
                                                     ),
                                HTTPResponse:        null,
                                Result:              true,
                                ProcessId:           Process_Id.NewRandom(),
                                CustomData:          null));

            };


            centralServiceAPI.EMPServerAPIClients.Add(Provider_Id.Parse("DE-GDF"),
                                                      new EMPServerAPIClient(
                                                          URL.Parse("http://127.0.0.1:8001"),
                                                          RequestTimeout: TimeSpan.FromSeconds(10)
                                                      ));

            #endregion

            #region Register EMP "DE-BDP"

            empRoaming_DEBDP = new EMPRoaming(

                                   new EMPClient(
                                       RemoteURL:        URL.Parse("http://127.0.0.1:6001"),
                                       RequestTimeout:   TimeSpan.FromSeconds(10)
                                   ),

                                   new EMPServerAPI(
                                       ExternalDNSName:  "open.charging.cloud",
                                       HTTPServerPort:   IPPort.Parse(8002),
                                       LoggingPath:      "tests",
                                       AutoStart:        true
                                   )

                               );

            ClassicAssert.IsNotNull(cpoRoaming_DEGEF);
            ClassicAssert.IsNotNull(cpoRoaming_DEGEF.CPOClient);
            ClassicAssert.IsNotNull(cpoRoaming_DEGEF.CPOServer);


            empRoaming_DEBDP.OnAuthorizeStart     += (timestamp, empClientAPI, authorizeStartRequest)     => {

                if (authorizeStartRequest.Identification.RFIDId?.ToString() == "11223344556677")
                    return Task.FromResult(
                               AuthorizationStartResponse.Authorized(
                                   authorizeStartRequest,
                                   Session_Id.          Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"), // generated by Hubject!
                                   authorizeStartRequest.CPOPartnerSessionId,
                                   EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),
                                   Provider_Id.Parse("DE-BDP"),
                                   "Nice to see you!",
                                   "Hello world!",
                                   [
                                       Identification.FromUID(UID.Parse("11223344556677")),
                                       Identification.FromUID(UID.Parse("33445566778899"))
                                   ]
                               )
                           );

                return Task.FromResult(
                               AuthorizationStartResponse.NotAuthorized(
                                   Request:               authorizeStartRequest,
                                   StatusCode:            new StatusCode(
                                                              StatusCodes.NoPositiveAuthenticationResponse,
                                                              "Unknown RFID UID!"
                                                          ),
                                   CPOPartnerSessionId:   authorizeStartRequest.CPOPartnerSessionId,
                                   ProviderId:            Provider_Id.Parse("DE-BDP")
                               )
                           );

            };

            empRoaming_DEBDP.OnAuthorizeStop      += (timestamp, empClientAPI, authorizeStopRequest)      => {

                return Task.FromResult(
                           AuthorizationStopResponse.Authorized(
                               authorizeStopRequest,
                               authorizeStopRequest.SessionId,
                               authorizeStopRequest.CPOPartnerSessionId,
                               authorizeStopRequest.EMPPartnerSessionId,
                               Provider_Id.Parse("DE-BDP"),
                               "Have a nice day!",
                               "bye bye!"
                           )
                       );

            };


            empRoaming_DEBDP.OnChargeDetailRecord += (timestamp, cpoServerAPI, chargeDetailRecordRequest) => {

                return Task.FromResult(
                            new Acknowledgement<ChargeDetailRecordRequest>(
                                Request:             chargeDetailRecordRequest,
                                ResponseTimestamp:   Timestamp.Now,
                                EventTrackingId:     EventTracking_Id.New,
                                Runtime:             TimeSpan.FromMilliseconds(2),
                                StatusCode:          new StatusCode(
                                                         StatusCodes.Success
                                                     ),
                                HTTPResponse:        null,
                                Result:              true,
                                ProcessId:           Process_Id.NewRandom(),
                                CustomData:          null));

            };


            centralServiceAPI.EMPServerAPIClients.Add(Provider_Id.Parse("DE-BDP"),
                                                      new EMPServerAPIClient(
                                                          URL.Parse("http://127.0.0.1:8002"),
                                                          RequestTimeout: TimeSpan.FromSeconds(10)
                                                      ));

            #endregion

        }

        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public void ShutdownEachTest()
        {

            centralServiceAPI?.Shutdown();

            cpoRoaming_DEGEF?. Shutdown();
            empRoaming_DEGDF?. Shutdown();

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
