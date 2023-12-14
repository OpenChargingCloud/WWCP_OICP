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
using NUnit.Framework.Legacy;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.CPO.client
{

    /// <summary>
    /// CPO sending EVSE data and status tests.
    /// </summary>
    [TestFixture]
    public class PushEVSEDataAndStatusTests : ACPOClientAPITests
    {

        #region PushEVSEData_Minimal1()

        [Test]
        public async Task PushEVSEData_Minimal1()
        {

            if (cpoClientAPI is null ||
                cpoClient    is null)
            {
                Assert.Fail("cpoClientAPI or cpoClient is null!");
                return;
            }

            var request = new PushEVSEDataRequest(
                              new OperatorEVSEData(
                                  new EVSEDataRecord[] {
                                      new EVSEDataRecord(

                                          Id:                                EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                          OperatorId:                        Operator_Id.Parse("DE*GEF"),
                                          OperatorName:                      "GraphDefined",
                                          ChargingStationName:               I18NText.Create(LanguageCode.de, "Station Eins").
                                                                                      Add   (LanguageCode.en, "Station One"),
                                          Address:                           new Address(
                                                                                 Country:           Country.Germany,
                                                                                 City:             "Jena",
                                                                                 Street:           "Biberweg",
                                                                                 PostalCode:       "07749",
                                                                                 HouseNumber:      "18",
                                                                                 Floor:             null,
                                                                                 Region:            null,
                                                                                 ParkingFacility:   false,
                                                                                 ParkingSpot:      "A1",
                                                                                 TimeZone:          Time_Zone.Parse("UTC+02:00"),
                                                                                 CustomData:        null
                                                                             ),
                                          GeoCoordinates:                    new GeoCoordinates(
                                                                                 50.927054,
                                                                                 11.5892372,
                                                                                 GeoCoordinatesFormats.DecimalDegree
                                                                             ),
                                          PlugTypes:                         new PlugTypes[] {
                                                                                 PlugTypes.CCSCombo2Plug_CableAttached
                                                                             },
                                          ChargingFacilities:                new ChargingFacility[] {
                                                                                 new ChargingFacility(
                                                                                     PowerType:      PowerTypes.AC_3_PHASE,
                                                                                     Power:          22,
                                                                                     Voltage:        400,
                                                                                     Amperage:       32,
                                                                                     ChargingModes:  new ChargingModes[] {
                                                                                                         ChargingModes.Mode_2
                                                                                                     },
                                                                                     CustomData:     null
                                                                                 )
                                                                             },
                                          RenewableEnergy:                   true,
                                          CalibrationLawDataAvailability:    CalibrationLawDataAvailabilities.External,
                                          AuthenticationModes:               new AuthenticationModes[] {
                                                                                 AuthenticationModes.NFC_RFID_Classic,
                                                                                 AuthenticationModes.REMOTE
                                                                             },
                                          PaymentOptions:                    new PaymentOptions[] {
                                                                                 PaymentOptions.Contract,
                                                                                 PaymentOptions.Direct
                                                                             },
                                          ValueAddedServices:                new ValueAddedServices[] {
                                                                                 ValueAddedServices.Reservation,
                                                                                 ValueAddedServices.DynamicPricing,
                                                                                 ValueAddedServices.ParkingSensors,
                                                                                 ValueAddedServices.MaximumPowerCharging,
                                                                                 ValueAddedServices.PredictiveChargePointUsage,
                                                                                 ValueAddedServices.ChargingPlans,
                                                                                 ValueAddedServices.RoofProvided
                                                                             },
                                          Accessibility:                     AccessibilityTypes.PayingPubliclyAccessible,
                                          HotlinePhoneNumber:                Phone_Number.Parse("+49555123456"),
                                          IsOpen24Hours:                     true,
                                          IsHubjectCompatible:               true,
                                          DynamicInfoAvailable:              FalseTrueAuto.True,

                                          DeltaType:                         null,
                                          LastUpdate:                        null,

                                          ChargingStationId:                 null,
                                          ChargingPoolId:                    null,
                                          HardwareManufacturer:              null,
                                          ChargingStationImageURL:           null,
                                          SubOperatorName:                   null,
                                          DynamicPowerLevel:                 null,
                                          EnergySources:                     null,
                                          EnvironmentalImpact:               null,
                                          MaxCapacity:                       null,
                                          AccessibilityLocationType:         null,
                                          AdditionalInfo:                    null,
                                          ChargingStationLocationReference:  null,
                                          GeoChargingPointEntrance:          null,
                                          OpeningTimes:                      null,
                                          HubOperatorId:                     null,
                                          ClearingHouseId:                   null,

                                          CustomData:                        null,
                                          InternalData:                      null

                                      )
                                  },
                                  Operator_Id.Parse("DE*GEF"),
                                  "GraphDefined"
                              ),
                              ActionTypes.FullLoad
                          );

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEData.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEData.Requests_Error);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEData.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEData.Responses_Error);

            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEData.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEData.Requests_Error);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEData.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEData.Responses_Error);

            var oicpResult  = await cpoClient.PushEVSEData(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsNotNull(oicpResult.Response);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            ClassicAssert.IsTrue   (oicpResult.Response?.Result);

            ClassicAssert.AreEqual(1, cpoClient.   Counters.PushEVSEData.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEData.Requests_Error);
            ClassicAssert.AreEqual(1, cpoClient.   Counters.PushEVSEData.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEData.Responses_Error);

            ClassicAssert.AreEqual(1, cpoClientAPI.Counters.PushEVSEData.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEData.Requests_Error);
            ClassicAssert.AreEqual(1, cpoClientAPI.Counters.PushEVSEData.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEData.Responses_Error);

        }

        #endregion

        #region PushEVSEData_Maximal1()

        [Test]
        public async Task PushEVSEData_Maximal1()
        {

            if (cpoClientAPI is null ||
                cpoClient    is null)
            {
                Assert.Fail("cpoClientAPI or cpoClient is null!");
                return;
            }

            var request = new PushEVSEDataRequest(
                              new OperatorEVSEData(
                                  new EVSEDataRecord[] {
                                      new EVSEDataRecord(

                                          Id:                                EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                          OperatorId:                        Operator_Id.Parse("DE*GEF"),
                                          OperatorName:                      "GraphDefined",
                                          ChargingStationName:               I18NText.Create(LanguageCode.de, "Station Eins").
                                                                                      Add   (LanguageCode.en, "Station One"),
                                          Address:                           new Address(
                                                                                 Country:           Country.Germany,
                                                                                 City:             "Jena",
                                                                                 Street:           "Biberweg",
                                                                                 PostalCode:       "07749",
                                                                                 HouseNumber:      "18",
                                                                                 Floor:             null,
                                                                                 Region:            null,
                                                                                 ParkingFacility:   false,
                                                                                 ParkingSpot:      "A1",
                                                                                 TimeZone:          Time_Zone.Parse("UTC+02:00"),
                                                                                 CustomData:        null
                                                                             ),
                                          GeoCoordinates:                    new GeoCoordinates(
                                                                                 50.927054,
                                                                                 11.5892372,
                                                                                 GeoCoordinatesFormats.DecimalDegree
                                                                             ),
                                          PlugTypes:                         new PlugTypes[] {
                                                                                 PlugTypes.CCSCombo2Plug_CableAttached
                                                                             },
                                          ChargingFacilities:                new ChargingFacility[] {
                                                                                 new ChargingFacility(
                                                                                     PowerType:      PowerTypes.AC_3_PHASE,
                                                                                     Power:          22,
                                                                                     Voltage:        400,
                                                                                     Amperage:       32,
                                                                                     ChargingModes:  new ChargingModes[] {
                                                                                                         ChargingModes.Mode_2
                                                                                                     },
                                                                                     CustomData:     null
                                                                                 )
                                                                             },
                                          RenewableEnergy:                   true,
                                          CalibrationLawDataAvailability:    CalibrationLawDataAvailabilities.External,
                                          AuthenticationModes:               new AuthenticationModes[] {
                                                                                 AuthenticationModes.NFC_RFID_Classic,
                                                                                 AuthenticationModes.REMOTE
                                                                             },
                                          PaymentOptions:                    new PaymentOptions[] {
                                                                                 PaymentOptions.Contract,
                                                                                 PaymentOptions.Direct
                                                                             },
                                          ValueAddedServices:                new ValueAddedServices[] {
                                                                                 ValueAddedServices.Reservation,
                                                                                 ValueAddedServices.DynamicPricing,
                                                                                 ValueAddedServices.ParkingSensors,
                                                                                 ValueAddedServices.MaximumPowerCharging,
                                                                                 ValueAddedServices.PredictiveChargePointUsage,
                                                                                 ValueAddedServices.ChargingPlans,
                                                                                 ValueAddedServices.RoofProvided
                                                                             },
                                          Accessibility:                     AccessibilityTypes.PayingPubliclyAccessible,
                                          HotlinePhoneNumber:                Phone_Number.Parse("+49555123456"),
                                          IsOpen24Hours:                     true,
                                          IsHubjectCompatible:               true,
                                          DynamicInfoAvailable:              FalseTrueAuto.True,

                                          DeltaType:                         null,
                                          LastUpdate:                        null,

                                          ChargingStationId:                 ChargingStation_Id.Parse("DE*GEF*S123*4567"),
                                          ChargingPoolId:                    ChargingPool_Id.   Parse("DE*GEF*P123"),
                                          HardwareManufacturer:              "GraphDefined Ladestations Manufaktur GmbH",
                                          ChargingStationImageURL:           URL.Parse("https://open.charging.cloud"),
                                          SubOperatorName:                   "GraphDefined Low Voltage Services",
                                          DynamicPowerLevel:                 true,
                                          EnergySources:                     new EnergySource[] {
                                                                                 new EnergySource(
                                                                                     EnergyTypes.Solar,
                                                                                     60
                                                                                 ),
                                                                                 new EnergySource(
                                                                                     EnergyTypes.Wind,
                                                                                     40
                                                                                 )
                                                                             },
                                          EnvironmentalImpact:               new EnvironmentalImpact(
                                                                                 CO2Emission:  400,
                                                                                 NuclearWaste:   0
                                                                             ),
                                          MaxCapacity:                       350,
                                          AccessibilityLocationType:         AccessibilityLocationTypes.OnStreet,
                                          AdditionalInfo:                    I18NText.Create(LanguageCode.de, "Wird mit Schokolade betrieben!").
                                                                                      Add   (LanguageCode.en, "Runs on chocolate!"),
                                          ChargingStationLocationReference:  I18NText.Create(LanguageCode.de, "Beim Hausmeister klingeln!").
                                                                                      Add   (LanguageCode.en, "Please ring the bell!"),
                                          GeoChargingPointEntrance:          new GeoCoordinates(
                                                                                 50.8,
                                                                                 11.6,
                                                                                 GeoCoordinatesFormats.DecimalDegree
                                                                             ),
                                          OpeningTimes:                      new OpeningTime[] {
                                                                                 new OpeningTime(
                                                                                     Periods:            new Period[] {
                                                                                                             new Period(
                                                                                                                 Begin: HourMinute.Parse("09:00"),
                                                                                                                 End:   HourMinute.Parse("18:00")
                                                                                                             )
                                                                                                         },
                                                                                     On:                 DaysOfWeek.Workdays,
                                                                                     UnstructuredText:  "Only in short weeks!",
                                                                                     CustomData:         null
                                                                                 ),
                                                                                 new OpeningTime(
                                                                                     Periods:            new Period[] {
                                                                                                             new Period(
                                                                                                                 Begin: HourMinute.Parse("10:00"),
                                                                                                                 End:   HourMinute.Parse("20:00")
                                                                                                             )
                                                                                                         },
                                                                                     On:                 DaysOfWeek.Weekend,
                                                                                     UnstructuredText:  "Lazy weekends!",
                                                                                     CustomData:         null
                                                                                 )
                                                                             },
                                          HubOperatorId:                     Operator_Id.     Parse("DE*HUB"),
                                          ClearingHouseId:                   ClearingHouse_Id.Parse("DE*CLR"),

                                          CustomData:                        null,
                                          InternalData:                      null

                                      )
                                  },
                                  Operator_Id.Parse("DE*GEF"),
                                  "GraphDefined"
                              ),
                              ActionTypes.FullLoad
                          );

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEData.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEData.Requests_Error);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEData.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEData.Responses_Error);

            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEData.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEData.Requests_Error);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEData.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEData.Responses_Error);

            var oicpResult  = await cpoClient.PushEVSEData(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsNotNull(oicpResult.Response);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            ClassicAssert.IsTrue   (oicpResult.Response?.Result);

            ClassicAssert.AreEqual(1, cpoClient.   Counters.PushEVSEData.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEData.Requests_Error);
            ClassicAssert.AreEqual(1, cpoClient.   Counters.PushEVSEData.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEData.Responses_Error);

            ClassicAssert.AreEqual(1, cpoClientAPI.Counters.PushEVSEData.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEData.Requests_Error);
            ClassicAssert.AreEqual(1, cpoClientAPI.Counters.PushEVSEData.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEData.Responses_Error);

        }

        #endregion


        #region PushEVSEStatus_Test1()

        [Test]
        public async Task PushEVSEStatus_Test1()
        {

            if (cpoClientAPI is null ||
                cpoClient    is null)
            {
                Assert.Fail("cpoClientAPI or cpoClient is null!");
                return;
            }

            var request = new PushEVSEStatusRequest(
                              OperatorEVSEStatus:   new OperatorEVSEStatus(
                                                        EVSEStatusRecords:   new EVSEStatusRecord[] {
                                                                                 new EVSEStatusRecord(
                                                                                     Id:          EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                                                                     Status:      EVSEStatusTypes.Available,
                                                                                     CustomData:  null
                                                                                 )
                                                                             },
                                                        OperatorId:          Operator_Id.Parse("DE*GEF"),
                                                        OperatorName:        "GraphDefined",
                                                        CustomData:          null
                                                    ),
                              Action:               ActionTypes.FullLoad
                          );

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEStatus.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEStatus.Requests_Error);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEStatus.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEStatus.Responses_Error);

            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEStatus.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEStatus.Requests_Error);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEStatus.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEStatus.Responses_Error);

            var oicpResult  = await cpoClient.PushEVSEStatus(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsNotNull(oicpResult.Response);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            ClassicAssert.IsTrue   (oicpResult.Response?.Result);

            ClassicAssert.AreEqual(1, cpoClient.   Counters.PushEVSEStatus.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEStatus.Requests_Error);
            ClassicAssert.AreEqual(1, cpoClient.   Counters.PushEVSEStatus.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClient.   Counters.PushEVSEStatus.Responses_Error);

            ClassicAssert.AreEqual(1, cpoClientAPI.Counters.PushEVSEStatus.Requests_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEStatus.Requests_Error);
            ClassicAssert.AreEqual(1, cpoClientAPI.Counters.PushEVSEStatus.Responses_OK);
            ClassicAssert.AreEqual(0, cpoClientAPI.Counters.PushEVSEStatus.Responses_Error);

        }

        #endregion

    }

}
