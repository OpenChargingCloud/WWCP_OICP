﻿/*
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

namespace cloud.charging.open.protocols.OICPv2_3.tests.EMP.server
{

    /// <summary>
    /// OICP EMP test defaults.
    /// </summary>
    public abstract class AEMPTests
    {

        #region Data

        protected EMPServerAPI?        empServerAPI;
        protected EMPServerAPIClient?  empServerAPIClient;

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

            empServerAPI = new EMPServerAPI(
                               ExternalDNSName:  "open.charging.cloud",
                               HTTPServerPort:   IPPort.Parse(8000),
                               LoggingPath:      "tests",
                               AutoStart:        true
                           );

            ClassicAssert.IsNotNull(empServerAPI);


            empServerAPI.OnAuthorizeStart               += (timestamp, empServerAPI, authorizeStartRequest) => {

                if (authorizeStartRequest.Identification is not null)
                {

                    if (authorizeStartRequest.Identification.RFIDId             is not null)
                    {
                        return authorizeStartRequest.Identification.RFIDId.ToString() switch
                        {

                            "AABBCCDD" =>
                                Task.FromResult(
                                    AuthorizationStartResponse.Authorized(
                                        Request:                           authorizeStartRequest,
                                        SessionId:                         authorizeStartRequest.SessionId,
                                        CPOPartnerSessionId:               authorizeStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:               authorizeStartRequest.EMPPartnerSessionId,
                                        ProviderId:                        Provider_Id.Parse("DE-GDF"),
                                        StatusCodeDescription:             null,
                                        StatusCodeAdditionalInfo:          null,
                                        AuthorizationStopIdentifications:  new Identification[] {
                                                                               Identification.FromUID(UID.Parse("11223344")),
                                                                               Identification.FromUID(UID.Parse("55667788"))
                                                                           },
                                        ResponseTimestamp:                 Timestamp.Now,
                                        EventTrackingId:                   EventTracking_Id.New,
                                        Runtime:                           TimeSpan.FromMilliseconds(2),
                                        ProcessId:                         Process_Id.NewRandom(),
                                        HTTPResponse:                      null,
                                        CustomData:                        null)),

                            _ =>
                                Task.FromResult(
                                    AuthorizationStartResponse.NotAuthorized(
                                        Request:                           authorizeStartRequest,
                                        StatusCode:                        new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                        SessionId:                         authorizeStartRequest.SessionId,
                                        CPOPartnerSessionId:               authorizeStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:               authorizeStartRequest.EMPPartnerSessionId,
                                        ProviderId:                        Provider_Id.Parse("DE-GDF"),
                                        ResponseTimestamp:                 Timestamp.Now,
                                        EventTrackingId:                   EventTracking_Id.New,
                                        Runtime:                           TimeSpan.FromMilliseconds(2),
                                        ProcessId:                         Process_Id.NewRandom(),
                                        HTTPResponse:                      null,
                                        CustomData:                        null))

                        };

                    }


                    if (authorizeStartRequest.Identification.RFIDIdentification is not null)
                    {
                        return authorizeStartRequest.Identification.RFIDIdentification.UID.ToString() switch
                        {

                            "AABBCCDD" =>
                                Task.FromResult(
                                    AuthorizationStartResponse.Authorized(
                                        Request:                           authorizeStartRequest,
                                        SessionId:                         authorizeStartRequest.SessionId,
                                        CPOPartnerSessionId:               authorizeStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:               authorizeStartRequest.EMPPartnerSessionId,
                                        ProviderId:                        Provider_Id.Parse("DE-GDF"),
                                        StatusCodeDescription:             null,
                                        StatusCodeAdditionalInfo:          null,
                                        AuthorizationStopIdentifications:  new Identification[] {
                                                                               Identification.FromUID(UID.Parse("11223344")),
                                                                               Identification.FromUID(UID.Parse("55667788"))
                                                                           },
                                        ResponseTimestamp:                 Timestamp.Now,
                                        EventTrackingId:                   EventTracking_Id.New,
                                        Runtime:                           TimeSpan.FromMilliseconds(2),
                                        ProcessId:                         Process_Id.NewRandom(),
                                        HTTPResponse:                      null,
                                        CustomData:                        null)),

                            _ =>
                                Task.FromResult(
                                    AuthorizationStartResponse.NotAuthorized(
                                        Request:                           authorizeStartRequest,
                                        StatusCode:                        new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                        SessionId:                         authorizeStartRequest.SessionId,
                                        CPOPartnerSessionId:               authorizeStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:               authorizeStartRequest.EMPPartnerSessionId,
                                        ProviderId:                        Provider_Id.Parse("DE-GDF"),
                                        ResponseTimestamp:                 Timestamp.Now,
                                        EventTrackingId:                   EventTracking_Id.New,
                                        Runtime:                           TimeSpan.FromMilliseconds(2),
                                        ProcessId:                         Process_Id.NewRandom(),
                                        HTTPResponse:                      null,
                                        CustomData:                        null))

                        };

                    }

                }

                return Task.FromResult(
                    AuthorizationStartResponse.DataError(
                        Request:                    authorizeStartRequest,
                        StatusCodeDescription:      "authorizeStartRequest.Identification is null!",
                        StatusCodeAdditionalInfo:   null,
                        SessionId:                  authorizeStartRequest.SessionId,
                        CPOPartnerSessionId:        authorizeStartRequest.CPOPartnerSessionId,
                        EMPPartnerSessionId:        authorizeStartRequest.EMPPartnerSessionId,
                        ProviderId:                 Provider_Id.Parse("DE-GDF"),
                        ResponseTimestamp:          Timestamp.Now,
                        EventTrackingId:            EventTracking_Id.New,
                        Runtime:                    TimeSpan.FromMilliseconds(2),
                        ProcessId:                  Process_Id.NewRandom(),
                        HTTPResponse:               null,
                        CustomData:                 null));

            };

            empServerAPI.OnAuthorizeStop                += (timestamp, empServerAPI, authorizeStopRequest)  => {

                if (authorizeStopRequest.Identification is not null)
                {

                    if (authorizeStopRequest.Identification.RFIDId             is not null)
                    {
                        return authorizeStopRequest.Identification.RFIDId.ToString() switch
                        {

                            "AABBCCDD" =>
                                Task.FromResult(
                                    AuthorizationStopResponse.Authorized(
                                        Request:                    authorizeStopRequest,
                                        SessionId:                  authorizeStopRequest.SessionId,
                                        CPOPartnerSessionId:        authorizeStopRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:        authorizeStopRequest.EMPPartnerSessionId,
                                        ProviderId:                 Provider_Id.Parse("DE-GDF"),
                                        StatusCodeDescription:      null,
                                        StatusCodeAdditionalInfo:   null,
                                        ResponseTimestamp:          Timestamp.Now,
                                        EventTrackingId:            EventTracking_Id.New,
                                        Runtime:                    TimeSpan.FromMilliseconds(2),
                                        ProcessId:                  Process_Id.NewRandom(),
                                        HTTPResponse:               null,
                                        CustomData:                 null)),

                            _ =>
                                Task.FromResult(
                                    AuthorizationStopResponse.NotAuthorized(
                                        Request:                    authorizeStopRequest,
                                        StatusCode:                 new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                        SessionId:                  authorizeStopRequest.SessionId,
                                        CPOPartnerSessionId:        authorizeStopRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:        authorizeStopRequest.EMPPartnerSessionId,
                                        ProviderId:                 Provider_Id.Parse("DE-GDF"),
                                        ResponseTimestamp:          Timestamp.Now,
                                        EventTrackingId:            EventTracking_Id.New,
                                        Runtime:                    TimeSpan.FromMilliseconds(2),
                                        ProcessId:                  Process_Id.NewRandom(),
                                        HTTPResponse:               null,
                                        CustomData:                 null))

                        };

                    }


                    if (authorizeStopRequest.Identification.RFIDIdentification is not null)
                    {
                        return authorizeStopRequest.Identification.RFIDIdentification.UID.ToString() switch
                        {

                            "AABBCCDD" =>
                                Task.FromResult(
                                    AuthorizationStopResponse.Authorized(
                                        Request:                    authorizeStopRequest,
                                        SessionId:                  authorizeStopRequest.SessionId,
                                        CPOPartnerSessionId:        authorizeStopRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:        authorizeStopRequest.EMPPartnerSessionId,
                                        ProviderId:                 Provider_Id.Parse("DE-GDF"),
                                        StatusCodeDescription:      null,
                                        StatusCodeAdditionalInfo:   null,
                                        ResponseTimestamp:          Timestamp.Now,
                                        EventTrackingId:            EventTracking_Id.New,
                                        Runtime:                    TimeSpan.FromMilliseconds(2),
                                        ProcessId:                  Process_Id.NewRandom(),
                                        HTTPResponse:               null,
                                        CustomData:                 null)),

                            _ =>
                                Task.FromResult(
                                    AuthorizationStopResponse.NotAuthorized(
                                        Request:                    authorizeStopRequest,
                                        StatusCode:                 new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                        SessionId:                  authorizeStopRequest.SessionId,
                                        CPOPartnerSessionId:        authorizeStopRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:        authorizeStopRequest.EMPPartnerSessionId,
                                        ProviderId:                 Provider_Id.Parse("DE-GDF"),
                                        ResponseTimestamp:          Timestamp.Now,
                                        EventTrackingId:            EventTracking_Id.New,
                                        Runtime:                    TimeSpan.FromMilliseconds(2),
                                        ProcessId:                  Process_Id.NewRandom(),
                                        HTTPResponse:               null,
                                        CustomData:                 null))

                        };

                    }

                }

                return Task.FromResult(
                    AuthorizationStopResponse.DataError(
                        Request:                    authorizeStopRequest,
                        StatusCodeDescription:      "authorizeStopRequest.Identification is null!",
                        StatusCodeAdditionalInfo:   null,
                        SessionId:                  authorizeStopRequest.SessionId,
                        CPOPartnerSessionId:        authorizeStopRequest.CPOPartnerSessionId,
                        EMPPartnerSessionId:        authorizeStopRequest.EMPPartnerSessionId,
                        ProviderId:                 Provider_Id.Parse("DE-GDF"),
                        ResponseTimestamp:          Timestamp.Now,
                        EventTrackingId:            EventTracking_Id.New,
                        Runtime:                    TimeSpan.FromMilliseconds(2),
                        ProcessId:                  Process_Id.NewRandom(),
                        HTTPResponse:               null,
                        CustomData:                 null));

            };


            empServerAPI.OnChargingStartNotification    += (timestamp, empServerAPI, chargingStartNotificationRequest)    => {

                return Task.FromResult(
                    new Acknowledgement<ChargingStartNotificationRequest>(
                        Timestamp.Now,
                        chargingStartNotificationRequest.EventTrackingId ?? EventTracking_Id.New,
                        Process_Id.NewRandom(),
                        TimeSpan.Zero,
                        new StatusCode(StatusCodes.Success),
                        chargingStartNotificationRequest,
                        null,
                        true,
                        chargingStartNotificationRequest.SessionId,
                        chargingStartNotificationRequest.CPOPartnerSessionId,
                        chargingStartNotificationRequest.EMPPartnerSessionId,
                        null  // customData
                    )
                );

            };

            empServerAPI.OnChargingProgressNotification += (timestamp, empServerAPI, chargingProgressNotificationRequest) => {

                return Task.FromResult(
                    new Acknowledgement<ChargingProgressNotificationRequest>(
                        Timestamp.Now,
                        chargingProgressNotificationRequest.EventTrackingId ?? EventTracking_Id.New,
                        Process_Id.NewRandom(),
                        TimeSpan.Zero,
                        new StatusCode(StatusCodes.Success),
                        chargingProgressNotificationRequest,
                        null,
                        true,
                        chargingProgressNotificationRequest.SessionId,
                        chargingProgressNotificationRequest.CPOPartnerSessionId,
                        chargingProgressNotificationRequest.EMPPartnerSessionId,
                        null  // customData
                    )
                );

            };

            empServerAPI.OnChargingEndNotification      += (timestamp, empServerAPI, chargingEndNotificationRequest)      => {

                return Task.FromResult(
                    new Acknowledgement<ChargingEndNotificationRequest>(
                        Timestamp.Now,
                        chargingEndNotificationRequest.EventTrackingId ?? EventTracking_Id.New,
                        Process_Id.NewRandom(),
                        TimeSpan.Zero,
                        new StatusCode(StatusCodes.Success),
                        chargingEndNotificationRequest,
                        null,
                        true,
                        chargingEndNotificationRequest.SessionId,
                        chargingEndNotificationRequest.CPOPartnerSessionId,
                        chargingEndNotificationRequest.EMPPartnerSessionId,
                        null  // customData
                    )
                );

            };

            empServerAPI.OnChargingErrorNotification    += (timestamp, empServerAPI, chargingErrorNotificationRequest)    => {

                return Task.FromResult(
                    new Acknowledgement<ChargingErrorNotificationRequest>(
                        Timestamp.Now,
                        chargingErrorNotificationRequest.EventTrackingId ?? EventTracking_Id.New,
                        Process_Id.NewRandom(),
                        TimeSpan.Zero,
                        new StatusCode(StatusCodes.Success),
                        chargingErrorNotificationRequest,
                        null,
                        true,
                        chargingErrorNotificationRequest.SessionId,
                        chargingErrorNotificationRequest.CPOPartnerSessionId,
                        chargingErrorNotificationRequest.EMPPartnerSessionId,
                        null  // customData
                    )
                );

            };


            empServerAPI.OnChargeDetailRecord           += (timestamp, empServerAPI, chargeDetailRecordRequest) => {

                return Task.FromResult(
                    new Acknowledgement<ChargeDetailRecordRequest>(
                        Timestamp.Now,
                        chargeDetailRecordRequest.EventTrackingId ?? EventTracking_Id.New,
                        Process_Id.NewRandom(),
                        TimeSpan.Zero,
                        new StatusCode(StatusCodes.Success),
                        chargeDetailRecordRequest,
                        null,
                        true,
                        chargeDetailRecordRequest.ChargeDetailRecord.SessionId,
                        chargeDetailRecordRequest.ChargeDetailRecord.CPOPartnerSessionId,
                        chargeDetailRecordRequest.ChargeDetailRecord.EMPPartnerSessionId,
                        null  // customData
                    )
                );

            };



            empServerAPIClient = new EMPServerAPIClient(URL.Parse("http://127.0.0.1:8000"),
                                                        RequestTimeout: TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(empServerAPIClient);

        }


        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public void ShutdownEachTest()
        {
            empServerAPI?.Shutdown();
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
