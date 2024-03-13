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

using Newtonsoft.Json.Linq;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.OICPv2_3.CPO;
using cloud.charging.open.protocols.OICPv2_3.EMP;
using cloud.charging.open.protocols.OICPv2_3.p2p.CPO;
using cloud.charging.open.protocols.OICPv2_3.p2p.EMP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.P2P
{

    /// <summary>
    /// OICP P2P test defaults.
    /// </summary>
    public abstract class AP2PTests
    {

        #region Data

        protected readonly Operator_Id                                                 DEGEF_Id;
        protected readonly Operator_Id                                                 DEBDO_Id;
        protected          CPOPeer?                                                    cpoP2P_DEGEF;
        protected          CPOPeer?                                                    cpoP2P_DEBDO;

        protected readonly Provider_Id                                                 DEGDF_Id;
        protected readonly Provider_Id                                                 DEBDP_Id;
        protected          EMPPeer?                                                    empP2P_DEGDF;
        protected          EMPPeer?                                                    empP2P_DEBDP;

        protected readonly Dictionary<Operator_Id, HashSet<EVSEDataRecord>>            EVSEDataRecords;
        protected readonly Dictionary<Operator_Id, HashSet<EVSEStatusRecord>>          EVSEStatusRecords;
        protected readonly Dictionary<Operator_Id, HashSet<PricingProductDataRecord>>  PricingProductData;
        protected readonly Dictionary<Operator_Id, HashSet<EVSEPricing>>               EVSEPricings;
        protected readonly Dictionary<Operator_Id, HashSet<ChargeDetailRecord>>        ChargeDetailRecords;

        #endregion

        #region Constructor(s)

        public AP2PTests()
        {

            this.DEGEF_Id             = Operator_Id.Parse("DE*GEF");
            this.DEBDO_Id             = Operator_Id.Parse("DE*BDO");
            this.DEGDF_Id             = Provider_Id.Parse("DE-GDF");
            this.DEBDP_Id             = Provider_Id.Parse("DE-BDP");

            this.EVSEDataRecords      = new Dictionary<Operator_Id, HashSet<EVSEDataRecord>>();
            this.EVSEStatusRecords    = new Dictionary<Operator_Id, HashSet<EVSEStatusRecord>>();
            this.PricingProductData   = new Dictionary<Operator_Id, HashSet<PricingProductDataRecord>>();
            this.EVSEPricings         = new Dictionary<Operator_Id, HashSet<EVSEPricing>>();
            this.ChargeDetailRecords  = new Dictionary<Operator_Id, HashSet<ChargeDetailRecord>>();

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
        public virtual void SetupEachTest()
        {

            Timestamp.Reset();
            EVSEDataRecords.Clear();


            #region Register CPO "DE*GEF"

            cpoP2P_DEGEF = new CPOPeer(
                               OperatorId:       DEGEF_Id,
                               ExternalDNSName:  "cso.graphdefined.ops.charging.cloud",
                               HTTPServerPort:   IPPort.Parse(7001),
                               LoggingPath:      "tests",
                               AutoStart:        true
                           );

            ClassicAssert.IsNotNull(cpoP2P_DEGEF);
            ClassicAssert.IsNotNull(cpoP2P_DEGEF.EMPClientAPI);


            cpoP2P_DEGEF.EMPClientAPI.OnPullEVSEData                     += (timestamp, empClientAPI, pullEVSEDataRequest)                    => {

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

            cpoP2P_DEGEF.EMPClientAPI.OnPullEVSEStatus                   += (timestamp, empClientAPI, pullEVSEStatusRequest)                  => {

                var processId = Process_Id.NewRandom();

                if (pullEVSEStatusRequest.EVSEStatusFilter == EVSEStatusTypes.Reserved)
                    return Task.FromResult(
                               OICPResult<PullEVSEStatusResponse>.Success(
                                   pullEVSEStatusRequest,
                                   new PullEVSEStatusResponse(
                                       ResponseTimestamp:   Timestamp.Now,
                                       EventTrackingId:     EventTracking_Id.New,
                                       ProcessId:           processId,
                                       Runtime:             TimeSpan.FromMilliseconds(23),
                                       OperatorEVSEStatus:  Array.Empty<OperatorEVSEStatus>(),
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

                else
                    return Task.FromResult(
                               OICPResult<PullEVSEStatusResponse>.Success(
                                   pullEVSEStatusRequest,
                                   new PullEVSEStatusResponse(
                                       ResponseTimestamp:   Timestamp.Now,
                                       EventTrackingId:     EventTracking_Id.New,
                                       ProcessId:           processId,
                                       Runtime:             TimeSpan.FromMilliseconds(23),
                                       OperatorEVSEStatus:  new OperatorEVSEStatus[] {
                                                                new OperatorEVSEStatus(
                                                                    new EVSEStatusRecord[] {
                                                                        new EVSEStatusRecord(
                                                                            EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                                                            EVSEStatusTypes.Available
                                                                        ),
                                                                        new EVSEStatusRecord(
                                                                            EVSE_Id.Parse("DE*GEF*E1234567*A*2"),
                                                                            EVSEStatusTypes.Occupied
                                                                        )
                                                                    },
                                                                    Operator_Id.Parse("DE*GEF"),
                                                                    "GraphDefined"
                                                                )
                                                            },
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

            cpoP2P_DEGEF.EMPClientAPI.OnPullEVSEStatusById               += (timestamp, empClientAPI, pullEVSEStatusByIdRequest)              => {

                var processId = Process_Id.NewRandom();

                if (pullEVSEStatusByIdRequest.EVSEIds.Contains(EVSE_Id.Parse("DE*GEF*E1234567*A*1")))
                    return Task.FromResult(
                               OICPResult<PullEVSEStatusByIdResponse>.Success(
                                   pullEVSEStatusByIdRequest,
                                   new PullEVSEStatusByIdResponse(
                                       ResponseTimestamp:   Timestamp.Now,
                                       EventTrackingId:     EventTracking_Id.New,
                                       ProcessId:           processId,
                                       Runtime:             TimeSpan.FromMilliseconds(23),
                                       EVSEStatusRecords:   new EVSEStatusRecord[] {
                                                                new EVSEStatusRecord(
                                                                    EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                                                    EVSEStatusTypes.Available
                                                                )
                                                            },
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

                else
                    return Task.FromResult(
                               OICPResult<PullEVSEStatusByIdResponse>.Success(
                                   pullEVSEStatusByIdRequest,
                                   new PullEVSEStatusByIdResponse(
                                       ResponseTimestamp:   Timestamp.Now,
                                       EventTrackingId:     EventTracking_Id.New,
                                       ProcessId:           processId,
                                       Runtime:             TimeSpan.FromMilliseconds(23),
                                       EVSEStatusRecords:   Array.Empty<EVSEStatusRecord>(),
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


            cpoP2P_DEGEF.EMPClientAPI.OnPullPricingProductData           += (timestamp, empClientAPI, pullPricingProductDataRequest)          => {

                var processId = Process_Id.NewRandom();

                if (pullPricingProductDataRequest.OperatorIds.Contains(Operator_Id.Parse("DE*GEF")))
                    return Task.FromResult(
                               OICPResult<PullPricingProductDataResponse>.Success(
                                   pullPricingProductDataRequest,
                                   new PullPricingProductDataResponse(
                                       ResponseTimestamp:   Timestamp.Now,
                                       EventTrackingId:     EventTracking_Id.New,
                                       ProcessId:           processId,
                                       Runtime:             TimeSpan.FromMilliseconds(23),
                                       PricingProductData:  new PricingProductData[] {
                                                                new PricingProductData(
                                                                    Operator_Id.Parse("DE*GEF"),
                                                                    pullPricingProductDataRequest.ProviderId,
                                                                    1.23M,
                                                                    Currency_Id.EUR,
                                                                    Reference_Unit.HOUR,
                                                                    new PricingProductDataRecord[] {
                                                                        new PricingProductDataRecord(
                                                                            ProductId:                     PartnerProduct_Id.AC1,
                                                                            ReferenceUnit:                 Reference_Unit.HOUR,
                                                                            ProductPriceCurrency:          Currency_Id.EUR,
                                                                            PricePerReferenceUnit:         1,
                                                                            MaximumProductChargingPower:   22,
                                                                            IsValid24hours:                false,
                                                                            ProductAvailabilityTimes:      new ProductAvailabilityTimes[] {
                                                                                                               new ProductAvailabilityTimes(
                                                                                                                   new Period(
                                                                                                                       new HourMinute(09, 00),
                                                                                                                       new HourMinute(18, 00)
                                                                                                                   ),
                                                                                                                   WeekDay.Everyday
                                                                                                               )
                                                                                                           },
                                                                            AdditionalReferences:          new AdditionalReferences[] {
                                                                                                               new AdditionalReferences(
                                                                                                                   AdditionalReference:               Additional_Reference.ParkingFee,
                                                                                                                   AdditionalReferenceUnit:           Reference_Unit.HOUR,
                                                                                                                   PricePerAdditionalReferenceUnit:   2
                                                                                                               )
                                                                                                           }
                                                                        )
                                                                    },
                                                                    "GraphDefined"
                                                                )
                                                            },
                                       Request:             pullPricingProductDataRequest,
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

                else
                    return Task.FromResult(
                               OICPResult<PullPricingProductDataResponse>.Success(
                                   pullPricingProductDataRequest,
                                   new PullPricingProductDataResponse(
                                       ResponseTimestamp:   Timestamp.Now,
                                       EventTrackingId:     EventTracking_Id.New,
                                       ProcessId:           processId,
                                       Runtime:             TimeSpan.FromMilliseconds(23),
                                       PricingProductData:  Array.Empty<PricingProductData>(),
                                       Request:             pullPricingProductDataRequest,
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

            cpoP2P_DEGEF.EMPClientAPI.OnPullEVSEPricing                  += (timestamp, empClientAPI, pullEVSEPricingRequest)                 => {

                var processId = Process_Id.NewRandom();

                if (pullEVSEPricingRequest.OperatorIds.Contains(Operator_Id.Parse("DE*GEF")))
                    return Task.FromResult(
                               OICPResult<PullEVSEPricingResponse>.Success(
                                   pullEVSEPricingRequest,
                                   new PullEVSEPricingResponse(
                                       ResponseTimestamp:     Timestamp.Now,
                                       EventTrackingId:       EventTracking_Id.New,
                                       ProcessId:             processId,
                                       Runtime:               TimeSpan.FromMilliseconds(23),
                                       OperatorEVSEPricings:  new OperatorEVSEPricing[] {
                                                                  new OperatorEVSEPricing(
                                                                      new EVSEPricing[] {
                                                                          new EVSEPricing(
                                                                              EVSE_Id.    Parse("DE*GEF*E1234567*A*1"),
                                                                              Provider_Id.Parse("DE-GDF"),
                                                                              new PartnerProduct_Id[] {
                                                                                  PartnerProduct_Id.AC1
                                                                              }
                                                                          ),
                                                                          new EVSEPricing(
                                                                              EVSE_Id.    Parse("DE*GEF*E1234567*A*2"),
                                                                              new PartnerProduct_Id[] {
                                                                                  PartnerProduct_Id.AC3
                                                                              }
                                                                          )
                                                                      },
                                                                      Operator_Id.Parse("DE*GEF"),
                                                                      "GraphDefined"
                                                                  )
                                                              },
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

                else
                    return Task.FromResult(
                               OICPResult<PullEVSEPricingResponse>.Success(
                                   pullEVSEPricingRequest,
                                   new PullEVSEPricingResponse(
                                       ResponseTimestamp:     Timestamp.Now,
                                       EventTrackingId:       EventTracking_Id.New,
                                       ProcessId:             processId,
                                       Runtime:               TimeSpan.FromMilliseconds(23),
                                       OperatorEVSEPricings:  Array.Empty<OperatorEVSEPricing>(),
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


            cpoP2P_DEGEF.EMPClientAPI.OnPushAuthenticationData           += (timestamp, empClientAPI, pushAuthenticationDataRequest)          => {

                return Task.FromResult(
                    new OICPResult<Acknowledgement<PushAuthenticationDataRequest>>(
                        pushAuthenticationDataRequest,
                        new Acknowledgement<PushAuthenticationDataRequest>(
                            Request:               pushAuthenticationDataRequest,
                            ResponseTimestamp:     Timestamp.Now,
                            EventTrackingId:       EventTracking_Id.New,
                            Runtime:               TimeSpan.FromMilliseconds(2),
                            StatusCode:            new StatusCode(
                                                       StatusCodes.Success
                                                   ),
                            HTTPResponse:          null,
                            Result:                true,
                            ProcessId:             Process_Id.NewRandom(),
                            CustomData:            null),
                        false));

            };


            cpoP2P_DEGEF.EMPClientAPI.OnAuthorizeRemoteReservationStart  += (timestamp, empClientAPI, authorizeRemoteReservationStartRequest) => {

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
                                            ProcessId:             Process_Id.NewRandom(),
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
                                            ProcessId:             Process_Id.NewRandom(),
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
                            ProcessId:             Process_Id.NewRandom(),
                            CustomData:            null),
                        false));

            };

            cpoP2P_DEGEF.EMPClientAPI.OnAuthorizeRemoteReservationStop   += (timestamp, empClientAPI, authorizeRemoteReservationStopRequest)  => {

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
                                    ProcessId:             Process_Id.NewRandom(),
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
                                    ProcessId:             Process_Id.NewRandom(),
                                    CustomData:            null),
                                false))
                };

            };


            cpoP2P_DEGEF.EMPClientAPI.OnAuthorizeRemoteStart             += (timestamp, empClientAPI, authorizeRemoteStartRequest)            => {

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
                                            ProcessId:             Process_Id.NewRandom(),
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
                                            ProcessId:             Process_Id.NewRandom(),
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
                            ProcessId:             Process_Id.NewRandom(),
                            CustomData:            null),
                        true));

            };

            cpoP2P_DEGEF.EMPClientAPI.OnAuthorizeRemoteStop              += (timestamp, empClientAPI, authorizeRemoteStopRequest)             => {

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
                                    ProcessId:             Process_Id.NewRandom(),
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
                                    ProcessId:             Process_Id.NewRandom(),
                                    CustomData:            null),
                                false))
                };

            };


            cpoP2P_DEGEF.EMPClientAPI.OnGetChargeDetailRecords           += (timestamp, empClientAPI, getChargeDetailRecordsRequest)          => {

                var processId = Process_Id.NewRandom();

                if (Timestamp.Now - getChargeDetailRecordsRequest.To < TimeSpan.FromDays(1))
                    return Task.FromResult(
                               new OICPResult<GetChargeDetailRecordsResponse>(
                                   getChargeDetailRecordsRequest,
                                   new GetChargeDetailRecordsResponse(
                                       ResponseTimestamp:     Timestamp.Now,
                                       EventTrackingId:       EventTracking_Id.New,
                                       ProcessId:             processId,
                                       Runtime:               TimeSpan.FromMilliseconds(2),
                                       ChargeDetailRecords:   new ChargeDetailRecord[] {
                                                                  new ChargeDetailRecord(
                                                                      SessionId:                        Session_Id.Parse("4cfe3192-87ec-4757-9560-a6ce896bb88b"),
                                                                      EVSEId:                           EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                                                      Identification:                   Identification.FromUID(UID.Parse("AABBCCDD")),
                                                                      SessionStart:                     DateTime.Parse("2022-08-09T10:18:25.229Z"),
                                                                      SessionEnd:                       DateTime.Parse("2022-08-09T11:18:25.229Z"),
                                                                      ChargingStart:                    DateTime.Parse("2022-08-09T10:20:25.229Z"),
                                                                      ChargingEnd:                      DateTime.Parse("2022-08-09T11:13:25.229Z"),
                                                                      ConsumedEnergy:                   35,

                                                                      PartnerProductId:                 PartnerProduct_Id.Parse("AC3"),
                                                                      CPOPartnerSessionId:              CPOPartnerSession_Id.Parse("e9c6faad-75c8-4f5b-9b5c-164ae7459804"),
                                                                      EMPPartnerSessionId:              EMPPartnerSession_Id.Parse("290b96b3-57df-4021-b8f8-50d9c211c767"),
                                                                      MeterValueStart:                  3,
                                                                      MeterValueEnd:                    38,
                                                                      MeterValuesInBetween:             [
                                                                                                            4, 5 ,6
                                                                                                        ],
                                                                      SignedMeteringValues:             [
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
                                                                      CalibrationLawVerificationInfo:   new CalibrationLawVerification(
                                                                                                            CalibrationLawCertificateId:                  "4c6da173-6427-49ed-9b7d-ab0c674d4bc2",
                                                                                                            PublicKey:                                    "0x046eb5c26727e9477f916eb5c26727e9477f91f872d3d79b2bd9f872d3d79b2bd9",
                                                                                                            MeteringSignatureURL:                         URL.Parse("https://open.charging.cloud"),
                                                                                                            MeteringSignatureEncodingFormat:              "plain",
                                                                                                            SignedMeteringValuesVerificationInstruction:  "Just use the Chargy Transparency Software!",
                                                                                                            CustomData:                                   null
                                                                                                        ),
                                                                      HubOperatorId:                    Operator_Id.Parse("DE*GEF"),
                                                                      HubProviderId:                    Provider_Id.Parse("DE-GDF"),

                                                                      CustomData:                       null,
                                                                      InternalData:                     null
                                                                  )
                                                              },
                                       Request:               getChargeDetailRecordsRequest,
                                       FirstPage:             true,
                                       LastPage:              true,
                                       Number:                1,
                                       NumberOfElements:      1,
                                       Size:                  1,
                                       TotalElements:         1,
                                       TotalPages:            1,
                                       StatusCode:            new StatusCode(StatusCodes.Success),
                                       HTTPResponse:          null,
                                       CustomData:            null),
                                   true,
                                   null,
                                   processId));

                    else
                        return Task.FromResult(
                            new OICPResult<GetChargeDetailRecordsResponse>(
                                getChargeDetailRecordsRequest,
                                new GetChargeDetailRecordsResponse(
                                    ResponseTimestamp:     Timestamp.Now,
                                    EventTrackingId:       EventTracking_Id.New,
                                    ProcessId:             processId,
                                    Runtime:               TimeSpan.FromMilliseconds(2),
                                    ChargeDetailRecords:   Array.Empty<ChargeDetailRecord>(),
                                    Request:               getChargeDetailRecordsRequest,
                                    FirstPage:             true,
                                    LastPage:              true,
                                    Number:                1,
                                    NumberOfElements:      1,
                                    Size:                  1,
                                    TotalElements:         1,
                                    TotalPages:            1,
                                    StatusCode:            new StatusCode(StatusCodes.Success),
                                    HTTPResponse:          null,
                                    CustomData:            null),
                                true,
                                null,
                                processId));

            };


            #endregion

            #region Register EMP "DE-GDF"

            empP2P_DEGDF = new EMPPeer(
                               ProviderId:       DEGDF_Id,
                               ExternalDNSName: "emp.graphdefined.ops.charging.cloud",
                               HTTPServerPort:   IPPort.Parse(8001),
                               LoggingPath:      "tests",
                               AutoStart:        true
                           );

            ClassicAssert.IsNotNull(empP2P_DEGDF);
            ClassicAssert.IsNotNull(empP2P_DEGDF.CPOClientAPI);

            empP2P_DEGDF.CPOClientAPI.OnPushEVSEData                  += (timestamp, cpoClientAPI, pushEVSEDataRequest)                 => {

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

            empP2P_DEGDF.CPOClientAPI.OnPushEVSEStatus                += (timestamp, cpoClientAPI, pushEVSEStatusRequest)               => {

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


            empP2P_DEGDF.CPOClientAPI.OnPushPricingProductData        += (timestamp, cpoClientAPI, pushPricingProductDataRequest)       => {

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

                    case ActionTypes.Delete: {

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

            empP2P_DEGDF.CPOClientAPI.OnPushEVSEPricing               += (timestamp, cpoClientAPI, pushEVSEPricingRequest)              => {

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

                    case ActionTypes.Delete: {

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


            empP2P_DEGDF.CPOClientAPI.OnPullAuthenticationData        += (timestamp, cpoClientAPI, pullAuthenticationDataRequest)       => {

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


            empP2P_DEGDF.CPOClientAPI.OnAuthorizeStart                += (timestamp, cpoClientAPI, authorizeStartRequest)               => {

                var processId = Process_Id.NewRandom();

                if (authorizeStartRequest.CustomData is not null &&
                    authorizeStartRequest.CustomData["signatureValidation"]?.Value<Boolean>() is Boolean signatureValidation &&
                    signatureValidation == false) {

                    return Task.FromResult(
                           new OICPResult<AuthorizationStartResponse>(
                               authorizeStartRequest,
                               AuthorizationStartResponse.NotAuthorized(
                                   Request:               authorizeStartRequest,
                                   StatusCode:            new StatusCode(
                                                              StatusCodes.NoPositiveAuthenticationResponse,
                                                              "Invalid crypto signature!"
                                                          ),
                                   CPOPartnerSessionId:   authorizeStartRequest.CPOPartnerSessionId,
                                   ProviderId:            Provider_Id.Parse("DE-GDF")
                               ),
                               false,
                               null,
                               processId)
                           );

                }

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

            empP2P_DEGDF.CPOClientAPI.OnAuthorizeStop                 += (timestamp, cpoClientAPI, authorizeStopRequest)                => {

                var processId = Process_Id.NewRandom();

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


            empP2P_DEGDF.CPOClientAPI.OnChargingStartNotification     += (timestamp, cpoClientAPI, chargingStartNotificationRequest)    => {

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

            empP2P_DEGDF.CPOClientAPI.OnChargingProgressNotification  += (timestamp, cpoClientAPI, chargingProgressNotificationRequest) => {

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

            empP2P_DEGDF.CPOClientAPI.OnChargingEndNotification       += (timestamp, cpoClientAPI, chargingEndNotificationRequest)      => {

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

            empP2P_DEGDF.CPOClientAPI.OnChargingErrorNotification     += (timestamp, cpoClientAPI, chargingErrorNotificationRequest)    => {

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


            empP2P_DEGDF.CPOClientAPI.OnChargeDetailRecord            += (timestamp, cpoClientAPI, chargeDetailRecordRequest)           => {

                var processId = Process_Id.NewRandom();

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


            cpoP2P_DEGEF.RegisterProvider(DEGDF_Id,
                                          new CPOClient(
                                              URL.Parse("http://127.0.0.1:8001"),
                                              RequestTimeout: TimeSpan.FromSeconds(10)
                                          ));

            empP2P_DEGDF.RegisterOperator(DEGEF_Id,
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
