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
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.OICPv2_3.EMP;
using cloud.charging.open.protocols.OICPv2_3.CPO;
using cloud.charging.open.protocols.OICPv2_3.p2p;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.P2P
{

    /// <summary>
    /// OICP P2P test defaults.
    /// </summary>
    public abstract class AP2PTests
    {

        #region Data

        protected          CPOPeer?                                                    cpoP2P_DEGEF;
        protected          CPOPeer?                                                    cpoP2P_DEBDO;

        protected          EMPPeer?                                                    empP2P_DEGDF;
        protected          EMPPeer?                                                    empP2P_DEBDP;

        protected readonly Dictionary<Operator_Id, HashSet<EVSEDataRecord>>            EVSEDataRecords;
        protected readonly Dictionary<Operator_Id, HashSet<EVSEStatusRecord>>          EVSEStatusRecords;
        protected readonly Dictionary<Operator_Id, HashSet<PricingProductDataRecord>>  PricingProductData;
        protected readonly Dictionary<Operator_Id, HashSet<EVSEPricing>>               EVSEPricings;

        #endregion

        #region Constructor(s)

        public AP2PTests()
        {

            this.EVSEDataRecords     = new Dictionary<Operator_Id, HashSet<EVSEDataRecord>>();
            this.EVSEStatusRecords   = new Dictionary<Operator_Id, HashSet<EVSEStatusRecord>>();
            this.PricingProductData  = new Dictionary<Operator_Id, HashSet<PricingProductDataRecord>>();
            this.EVSEPricings        = new Dictionary<Operator_Id, HashSet<EVSEPricing>>();

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


            #region Register CPO "DE*GEF"

            cpoP2P_DEGEF = new CPOPeer(
                               ExternalDNSName:  "open.charging.cloud",
                               HTTPServerPort:   IPPort.Parse(7001),
                               LoggingPath:      "tests",
                               Autostart:        true
                           );

            Assert.IsNotNull(cpoP2P_DEGEF);
            Assert.IsNotNull(cpoP2P_DEGEF.EMPClientAPI);


            cpoP2P_DEGEF.EMPClientAPI.OnAuthorizeRemoteReservationStart += (timestamp, cpoServerAPI, authorizeRemoteReservationStartRequest) => {

                if (authorizeRemoteReservationStartRequest.Identification is not null)
                {

                    if (authorizeRemoteReservationStartRequest.Identification.RemoteIdentification is not null)
                    {
                        return authorizeRemoteReservationStartRequest.Identification.RemoteIdentification.ToString() switch
                        {

                            "DE-GDF-C12345678X" =>
                                Task.FromResult(
                                    new OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>(
                                        authorizeRemoteReservationStartRequest,
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
                                            ProcessId:             Process_Id.NewRandom,
                                            CustomData:            null),
                                        true)),

                            _ =>
                                Task.FromResult(
                                    new OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>(
                                        authorizeRemoteReservationStartRequest,
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
                                            ProcessId:             Process_Id.NewRandom,
                                            CustomData:            null),
                                        false))
                        };

                    }

                }

                return Task.FromResult(
                    new OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>(
                        authorizeRemoteReservationStartRequest,
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
                            ProcessId:             Process_Id.NewRandom,
                            CustomData:            null),
                        false));

            };

            cpoP2P_DEGEF.EMPClientAPI.OnAuthorizeRemoteReservationStop  += (timestamp, cpoServerAPI, authorizeRemoteReservationStopRequest)  => {

                return authorizeRemoteReservationStopRequest.SessionId.ToString() switch {

                    "7e8f35a6-13c8-4b37-8099-b21323c83e85" =>
                        Task.FromResult(
                            new OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>(
                                authorizeRemoteReservationStopRequest,
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
                                    ProcessId:             Process_Id.NewRandom,
                                    CustomData:            null),
                                true)),

                    _ =>
                        Task.FromResult(
                            new OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>(
                                authorizeRemoteReservationStopRequest,
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
                                    ProcessId:             Process_Id.NewRandom,
                                    CustomData:            null),
                                false))
                };

            };


            cpoP2P_DEGEF.EMPClientAPI.OnAuthorizeRemoteStart            += (timestamp, cpoServerAPI, authorizeRemoteStartRequest) => {

                if (authorizeRemoteStartRequest.Identification is not null)
                {

                    if (authorizeRemoteStartRequest.Identification.RemoteIdentification is not null)
                    {
                        return authorizeRemoteStartRequest.Identification.RemoteIdentification.ToString() switch
                        {

                            "DE-GDF-C12345678X" =>
                                Task.FromResult(
                                    new OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>(
                                        authorizeRemoteStartRequest,
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
                                            ProcessId:             Process_Id.NewRandom,
                                            CustomData:            null),
                                        true)),

                            _ =>
                                Task.FromResult(
                                    new OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>(
                                        authorizeRemoteStartRequest,
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
                                            ProcessId:             Process_Id.NewRandom,
                                            CustomData:            null),
                                        false))
                        };

                    }

                }

                return Task.FromResult(
                    new OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>(
                        authorizeRemoteStartRequest,
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
                            ProcessId:             Process_Id.NewRandom,
                            CustomData:            null),
                        true));

            };

            cpoP2P_DEGEF.EMPClientAPI.OnAuthorizeRemoteStop             += (timestamp, cpoServerAPI, authorizeRemoteStopRequest)  => {

                return authorizeRemoteStopRequest.SessionId.ToString() switch {

                    "7e8f35a6-13c8-4b37-8099-b21323c83e85" =>
                        Task.FromResult(
                            new OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>(
                                authorizeRemoteStopRequest,
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
                                    ProcessId:             Process_Id.NewRandom,
                                    CustomData:            null),
                                true)),

                    _ =>
                        Task.FromResult(
                            new OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>(
                                authorizeRemoteStopRequest,
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
                                    ProcessId:             Process_Id.NewRandom,
                                    CustomData:            null),
                                false))
                };

            };

            #endregion


            #region Register EMP "DE-GDF"

            empP2P_DEGDF = new EMPPeer(
                               ExternalDNSName:  "open.charging.cloud",
                               HTTPServerPort:   IPPort.Parse(8001),
                               LoggingPath:      "tests",
                               Autostart:        true
                           );

            Assert.IsNotNull(empP2P_DEGDF);
            Assert.IsNotNull(empP2P_DEGDF.CPOClientAPI);

            empP2P_DEGDF.CPOClientAPI.OnPushEVSEData                 += (timestamp, cpoClientAPI, pushEVSEDataRequest)   => {

                var processId = Process_Id.NewRandom;

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

            empP2P_DEGDF.CPOClientAPI.OnPushEVSEStatus               += (timestamp, cpoClientAPI, pushEVSEStatusRequest) => {

                var processId = Process_Id.NewRandom;

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


            empP2P_DEGDF.CPOClientAPI.OnPushPricingProductData       += (timestamp, cpoClientAPI, pushPricingProductDataRequest) => {

                var processId = Process_Id.NewRandom;

                switch (pushPricingProductDataRequest.Action)
                {

                    case ActionTypes.FullLoad:
                        {

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

                    case ActionTypes.Update:
                        {

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
                                                            Request: pushPricingProductDataRequest,
                                                            StatusCodeDescription: "EVSE pricing product data record for update not found: " + missing.Value.ToString(),
                                                            StatusCodeAdditionalInfo: null,
                                                            ResponseTimestamp: Timestamp.Now,
                                                            EventTrackingId: EventTracking_Id.New,
                                                            Runtime: TimeSpan.FromMilliseconds(2),
                                                            ProcessId: processId,
                                                            HTTPResponse: null,
                                                            CustomData: null
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

                    case ActionTypes.Insert:
                        {

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
                                                            Request: pushPricingProductDataRequest,
                                                            StatusCodeDescription: "EVSE pricing product data record '" + duplicate.Value.ToString() + "' already exists!'",
                                                            StatusCodeAdditionalInfo: null,
                                                            ResponseTimestamp: Timestamp.Now,
                                                            EventTrackingId: EventTracking_Id.New,
                                                            Runtime: TimeSpan.FromMilliseconds(2),
                                                            ProcessId: processId,
                                                            HTTPResponse: null,
                                                            CustomData: null
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

                    case ActionTypes.Delete:
                        {

                            if (PricingProductData.ContainsKey(pushPricingProductDataRequest.OperatorId))
                            {

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
                                   Request: pushPricingProductDataRequest,
                                   ResponseTimestamp: Timestamp.Now,
                                   EventTrackingId: EventTracking_Id.New,
                                   Runtime: TimeSpan.FromMilliseconds(2),
                                   StatusCode: new StatusCode(
                                                            StatusCodes.Success
                                                        ),
                                   HTTPResponse: null,
                                   Result: true,
                                   ProcessId: processId,
                                   CustomData: null
                               ),
                               true,
                               null,
                               processId));

            };

            empP2P_DEGDF.CPOClientAPI.OnPushEVSEPricing              += (timestamp, cpoClientAPI, pushEVSEPricingRequest)        => {

                var processId = Process_Id.NewRandom;

                switch (pushEVSEPricingRequest.Action)
                {

                    case ActionTypes.FullLoad:
                        {

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

                    case ActionTypes.Update:
                        {

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
                                                            Request: pushEVSEPricingRequest,
                                                            StatusCodeDescription: "EVSE pricing record for update not found: " + missing.Value.ToString(),
                                                            StatusCodeAdditionalInfo: null,
                                                            ResponseTimestamp: Timestamp.Now,
                                                            EventTrackingId: EventTracking_Id.New,
                                                            Runtime: TimeSpan.FromMilliseconds(2),
                                                            ProcessId: processId,
                                                            HTTPResponse: null,
                                                            CustomData: null
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

                    case ActionTypes.Insert:
                        {

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
                                                            Request: pushEVSEPricingRequest,
                                                            StatusCodeDescription: "EVSE pricing record '" + duplicate.Value.ToString() + "' already exists!'",
                                                            StatusCodeAdditionalInfo: null,
                                                            ResponseTimestamp: Timestamp.Now,
                                                            EventTrackingId: EventTracking_Id.New,
                                                            Runtime: TimeSpan.FromMilliseconds(2),
                                                            ProcessId: processId,
                                                            HTTPResponse: null,
                                                            CustomData: null
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

                    case ActionTypes.Delete:
                        {

                            if (EVSEPricings.ContainsKey(pushEVSEPricingRequest.OperatorId))
                            {

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
                                   Request: pushEVSEPricingRequest,
                                   ResponseTimestamp: Timestamp.Now,
                                   EventTrackingId: EventTracking_Id.New,
                                   Runtime: TimeSpan.FromMilliseconds(2),
                                   StatusCode: new StatusCode(
                                                            StatusCodes.Success
                                                        ),
                                   HTTPResponse: null,
                                   Result: true,
                                   ProcessId: processId,
                                   CustomData: null
                               ),
                               true,
                               null,
                               processId));

            };


            empP2P_DEGDF.CPOClientAPI.OnAuthorizeStart               += (timestamp, cpoClientAPI, authorizeStartRequest) => {

                var processId = Process_Id.NewRandom;

                if (authorizeStartRequest.Identification.RFIDId?.ToString() == "11223344")
                    return Task.FromResult(
                               new OICPResult<AuthorizationStartResponse>(
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
                                   ),
                                   true,
                                   null,
                                   processId)
                           );

                return Task.FromResult(
                           new OICPResult<AuthorizationStartResponse>(
                               authorizeStartRequest,
                               AuthorizationStartResponse.NotAuthorized(
                                   Request:               authorizeStartRequest,
                                   StatusCode:            new StatusCode(
                                                              StatusCodes.NoPositiveAuthenticationResponse,
                                                              "Unknown RFID UID!"
                                                          ),
                                   CPOPartnerSessionId:   authorizeStartRequest.CPOPartnerSessionId,
                                   ProviderId:            Provider_Id.Parse("DE-GDF")
                               ),
                               false,
                               null,
                               processId)
                           );

            };

            empP2P_DEGDF.CPOClientAPI.OnAuthorizeStop                += (timestamp, cpoClientAPI, authorizeStopRequest)  => {

                var processId = Process_Id.NewRandom;

                return Task.FromResult(
                           new OICPResult<AuthorizationStopResponse>(
                               authorizeStopRequest,
                               AuthorizationStopResponse.Authorized(
                                   authorizeStopRequest,
                                   authorizeStopRequest.SessionId,
                                   authorizeStopRequest.CPOPartnerSessionId,
                                   authorizeStopRequest.EMPPartnerSessionId,
                                   Provider_Id.Parse("DE-GDF"),
                                   "Have a nice day!",
                                   "bye bye!"
                               ),
                               true,
                               null,
                               processId)
                       );

            };


            empP2P_DEGDF.CPOClientAPI.OnChargingStartNotification    += (timestamp, cpoClientAPI, chargingStartNotificationRequest)    => {

                var processId = Process_Id.NewRandom;

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

            empP2P_DEGDF.CPOClientAPI.OnChargingProgressNotification += (timestamp, cpoClientAPI, chargingProgressNotificationRequest) => {

                var processId = Process_Id.NewRandom;

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

            empP2P_DEGDF.CPOClientAPI.OnChargingEndNotification      += (timestamp, cpoClientAPI, chargingEndNotificationRequest)      => {

                var processId = Process_Id.NewRandom;

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

            empP2P_DEGDF.CPOClientAPI.OnChargingErrorNotification    += (timestamp, cpoClientAPI, chargingErrorNotificationRequest)    => {

                var processId = Process_Id.NewRandom;

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


            empP2P_DEGDF.CPOClientAPI.OnChargeDetailRecord           += (timestamp, cpoClientAPI, chargeDetailRecordRequest) => {

                var processId = Process_Id.NewRandom;

                return Task.FromResult(
                           new OICPResult<Acknowledgement<ChargeDetailRecordRequest>>(
                               chargeDetailRecordRequest,
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
                                   ProcessId:           processId,
                                   CustomData:          null
                               ),
                               true,
                               null,
                               processId));

            };

            #endregion



            cpoP2P_DEGEF.RegisterProvider(Provider_Id.Parse("DE-GDF"),
                                          new CPOClient(
                                              URL.Parse("http://127.0.0.1:8001"),
                                              RequestTimeout: TimeSpan.FromSeconds(10)
                                          ));

            empP2P_DEGDF.RegisterOperator(Operator_Id.Parse("DE*GEF"),
                                          new EMPClient(
                                              URL.Parse("http://127.0.0.1:7001"),
                                              RequestTimeout: TimeSpan.FromSeconds(10)
                                          ));

        }

        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public void ShutdownEachTest()
        {

            cpoP2P_DEGEF?. Shutdown();
            empP2P_DEGDF?. Shutdown();

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
