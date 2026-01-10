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

        #region PushEVSEData_Maximal1()

        [Test]
        public async Task PushEVSEData_Maximal1()
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

            var request                = new PushEVSEDataRequest(

                                             new OperatorEVSEData(

                                                 [
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
                                                                                                Floor:             "2",
                                                                                                Region:            "TH",
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
                                                         PlugTypes:                         [
                                                                                                PlugTypes.CCSCombo2Plug_CableAttached
                                                                                            ],
                                                         ChargingFacilities:                [
                                                                                                new ChargingFacility(
                                                                                                    PowerType:      PowerTypes.AC_3_PHASE,
                                                                                                    Power:          22,
                                                                                                    Voltage:        400,
                                                                                                    Amperage:       32,
                                                                                                    ChargingModes:  [ ChargingModes.Mode_2 ],
                                                                                                    CustomData:     null
                                                                                                )
                                                                                            ],
                                                         RenewableEnergy:                   true,
                                                         CalibrationLawDataAvailability:    CalibrationLawDataAvailabilities.External,
                                                         AuthenticationModes:               [
                                                                                                AuthenticationModes.NFC_RFID_Classic,
                                                                                                AuthenticationModes.REMOTE
                                                                                            ],
                                                         PaymentOptions:                    [
                                                                                                PaymentOptions.Contract,
                                                                                                PaymentOptions.Direct
                                                                                            ],
                                                         ValueAddedServices:                [
                                                                                                ValueAddedServices.Reservation,
                                                                                                ValueAddedServices.DynamicPricing,
                                                                                                ValueAddedServices.ParkingSensors,
                                                                                                ValueAddedServices.MaximumPowerCharging,
                                                                                                ValueAddedServices.PredictiveChargePointUsage,
                                                                                                ValueAddedServices.ChargingPlans,
                                                                                                ValueAddedServices.RoofProvided
                                                                                            ],
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
                                                         EnergySources:                     [
                                                                                                new EnergySource(
                                                                                                    EnergyTypes.Solar,
                                                                                                    60
                                                                                                ),
                                                                                                new EnergySource(
                                                                                                    EnergyTypes.Wind,
                                                                                                    40
                                                                                                )
                                                                                            ],
                                                         EnvironmentalImpact:               new EnvironmentalImpact(
                                                                                                CO2Emission:  400,
                                                                                                NuclearWaste:   0
                                                                                            ),
                                                         MaxCapacity:                       WattHour.ParseKWh(350),
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
                                                         OpeningTimes:                      [
                                                                                                new OpeningTime(
                                                                                                    Periods:            [
                                                                                                                            new Period(
                                                                                                                                Begin: HourMinute.Parse("09:00"),
                                                                                                                                End:   HourMinute.Parse("18:00")
                                                                                                                            )
                                                                                                                        ],
                                                                                                    On:                 DaysOfWeek.Workdays,
                                                                                                    UnstructuredText:  "Only in short weeks!",
                                                                                                    CustomData:         null
                                                                                                ),
                                                                                                new OpeningTime(
                                                                                                    Periods:            [
                                                                                                                            new Period(
                                                                                                                                Begin: HourMinute.Parse("10:00"),
                                                                                                                                End:   HourMinute.Parse("20:00")
                                                                                                                            )
                                                                                                                        ],
                                                                                                    On:                 DaysOfWeek.Weekend,
                                                                                                    UnstructuredText:  "Lazy weekends!",
                                                                                                    CustomData:         null
                                                                                                )
                                                                                            ],
                                                         HubOperatorId:                     Operator_Id.     Parse("DE*HUB"),
                                                         ClearingHouseId:                   ClearingHouse_Id.Parse("DE*CLR"),

                                                         CustomData:                        new JObject(
                                                                                                new JProperty("hello", "EVSE world!")
                                                                                            )

                                                     )
                                                 ],

                                                 Operator_Id.Parse("DE*GEF"),
                                                 "GraphDefined",
                                                 new JObject(
                                                     new JProperty("hello", "CPO data world!")
                                                 )

                                             ),

                                             ActionTypes.FullLoad,

                                             CustomData:  new JObject(
                                                              new JProperty("hello", "PushData world!")
                                                          )

                                         );

            Assert.That(request,                                                                          Is.Not.Null);

            Assert.That(cpoClient.   Counters.PushEVSEData.Requests_OK,                                   Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushEVSEData.Requests_Error,                                Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushEVSEData.Responses_OK,                                  Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushEVSEData.Responses_Error,                               Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.PushEVSEData.Requests_OK,                                   Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushEVSEData.Requests_Error,                                Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushEVSEData.Responses_OK,                                  Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushEVSEData.Responses_Error,                               Is.EqualTo(0));

            cpoClient.   OnPushEVSEDataRequest  += (timestamp, cpoClient,    pushEVSEDataRequest) => {

                var evseData1 = pushEVSEDataRequest.OperatorEVSEData.EVSEDataRecords.FirstOrDefault();

                Assert.That(evseData1?.Id,                                                                Is.EqualTo(EVSE_Id.Parse("DE*GEF*E1234567*A*1")));
                Assert.That(evseData1?.OperatorId,                                                        Is.EqualTo(Operator_Id.Parse("DE*GEF")));
                Assert.That(evseData1?.OperatorName,                                                      Is.EqualTo("GraphDefined"));
                Assert.That(evseData1?.ChargingStationName[LanguageCode.de],                              Is.EqualTo("Station Eins"));
                Assert.That(evseData1?.ChargingStationName[LanguageCode.en],                              Is.EqualTo("Station One"));
                Assert.That(evseData1?.Address.Country,                                                   Is.EqualTo(Country.Germany));
                Assert.That(evseData1?.Address.City,                                                      Is.EqualTo("Jena"));
                Assert.That(evseData1?.Address.Street,                                                    Is.EqualTo("Biberweg"));
                Assert.That(evseData1?.Address.PostalCode,                                                Is.EqualTo("07749"));
                Assert.That(evseData1?.Address.HouseNumber,                                               Is.EqualTo("18"));
                Assert.That(evseData1?.Address.Floor,                                                     Is.EqualTo("2"));
                Assert.That(evseData1?.Address.Region,                                                    Is.EqualTo("TH"));
                Assert.That(evseData1?.Address.ParkingFacility,                                           Is.False);
                Assert.That(evseData1?.Address.ParkingSpot,                                               Is.EqualTo("A1"));
                Assert.That(evseData1?.Address.TimeZone,                                                  Is.EqualTo(Time_Zone.Parse("UTC+02:00")));
                Assert.That(evseData1?.GeoCoordinates.Latitude,                                           Is.EqualTo(50.927054));
                Assert.That(evseData1?.GeoCoordinates.Longitude,                                          Is.EqualTo(11.5892372));
                Assert.That(evseData1?.GeoCoordinates.GeoFormat,                                          Is.EqualTo(GeoCoordinatesFormats.DecimalDegree));
                Assert.That(evseData1?.PlugTypes,                                                         Is.EquivalentTo([
                                                                                                              PlugTypes.CCSCombo2Plug_CableAttached
                                                                                                          ]));
                Assert.That(evseData1?.ChargingFacilities.Count(),                                        Is.EqualTo(1));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).PowerType,                         Is.EqualTo(PowerTypes.AC_3_PHASE));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).Power,                             Is.EqualTo(22));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).Voltage,                           Is.EqualTo(400));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).Amperage,                          Is.EqualTo(32));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).ChargingModes,                     Is.EquivalentTo([
                                                                                                              ChargingModes.Mode_2
                                                                                                          ]));
                Assert.That(evseData1?.RenewableEnergy,                                                   Is.True);
                Assert.That(evseData1?.CalibrationLawDataAvailability,                                    Is.EqualTo(CalibrationLawDataAvailabilities.External));
                Assert.That(evseData1?.AuthenticationModes,                                               Is.EquivalentTo([
                                                                                                              AuthenticationModes.NFC_RFID_Classic,
                                                                                                              AuthenticationModes.REMOTE
                                                                                                          ]));
                Assert.That(evseData1?.PaymentOptions,                                                    Is.EquivalentTo([
                                                                                                              PaymentOptions.Contract,
                                                                                                              PaymentOptions.Direct
                                                                                                          ]));
                Assert.That(evseData1?.ValueAddedServices,                                                Is.EquivalentTo([
                                                                                                              ValueAddedServices.Reservation,
                                                                                                              ValueAddedServices.DynamicPricing,
                                                                                                              ValueAddedServices.ParkingSensors,
                                                                                                              ValueAddedServices.MaximumPowerCharging,
                                                                                                              ValueAddedServices.PredictiveChargePointUsage,
                                                                                                              ValueAddedServices.ChargingPlans,
                                                                                                              ValueAddedServices.RoofProvided
                                                                                                          ]));
                Assert.That(evseData1?.Accessibility,                                                     Is.EqualTo(AccessibilityTypes.PayingPubliclyAccessible));
                Assert.That(evseData1?.HotlinePhoneNumber,                                                Is.EqualTo(Phone_Number.Parse("+49555123456")));
                Assert.That(evseData1?.IsOpen24Hours,                                                     Is.True);
                Assert.That(evseData1?.IsHubjectCompatible,                                               Is.True);
                Assert.That(evseData1?.DynamicInfoAvailable,                                              Is.EqualTo(FalseTrueAuto.True));
                Assert.That(evseData1?.DeltaType,                                                         Is.Null);
                Assert.That(evseData1?.LastUpdate,                                                        Is.Null);

                Assert.That(evseData1?.ChargingStationId,                                                 Is.EqualTo(ChargingStation_Id.Parse("DE*GEF*S123*4567")));
                Assert.That(evseData1?.ChargingPoolId,                                                    Is.EqualTo(ChargingPool_Id.   Parse("DE*GEF*P123")));
                Assert.That(evseData1?.HardwareManufacturer,                                              Is.EqualTo("GraphDefined Ladestations Manufaktur GmbH"));
                Assert.That(evseData1?.ChargingStationImageURL,                                           Is.EqualTo(URL.Parse("https://open.charging.cloud")));
                Assert.That(evseData1?.SubOperatorName,                                                   Is.EqualTo("GraphDefined Low Voltage Services"));
                Assert.That(evseData1?.DynamicPowerLevel,                                                 Is.True);
                Assert.That(evseData1?.EnergySources,                                                     Is.EqualTo([
                                                                                                              new EnergySource(
                                                                                                                  EnergyTypes.Solar,
                                                                                                                  60
                                                                                                              ),
                                                                                                              new EnergySource(
                                                                                                                  EnergyTypes.Wind,
                                                                                                                  40
                                                                                                              )
                                                                                                          ]));
                Assert.That(evseData1?.EnvironmentalImpact,                                               Is.EqualTo(
                                                                                                              new EnvironmentalImpact(
                                                                                                                  CO2Emission:  400,
                                                                                                                  NuclearWaste:   0
                                                                                                              )));
                Assert.That(evseData1?.MaxCapacity,                                                       Is.EqualTo(WattHour.ParseKWh(350)));
                Assert.That(evseData1?.AccessibilityLocationType,                                         Is.EqualTo(AccessibilityLocationTypes.OnStreet));
                Assert.That(evseData1?.AdditionalInfo?[LanguageCode.de],                                  Is.EqualTo("Wird mit Schokolade betrieben!"));
                Assert.That(evseData1?.AdditionalInfo?[LanguageCode.en],                                  Is.EqualTo("Runs on chocolate!"));
                Assert.That(evseData1?.ChargingStationLocationReference?[LanguageCode.de],                Is.EqualTo("Beim Hausmeister klingeln!"));
                Assert.That(evseData1?.ChargingStationLocationReference?[LanguageCode.en],                Is.EqualTo("Please ring the bell!"));
                Assert.That(evseData1?.GeoChargingPointEntrance?.Latitude,                                Is.EqualTo(50.8). Within(1e-6));
                Assert.That(evseData1?.GeoChargingPointEntrance?.Longitude,                               Is.EqualTo(11.6). Within(1e-6));
                //Assert.That(evseData1?.OpeningTimes,                                                      Is.Empty);
                Assert.That(evseData1?.HubOperatorId?.  ToString(),                                       Is.EqualTo("DE*HUB"));
                Assert.That(evseData1?.ClearingHouseId?.ToString(),                                       Is.EqualTo("DE*CLR"));
                Assert.That(evseData1?.CustomData?.Count,                                                 Is.EqualTo(1));
                Assert.That(evseData1?.CustomData?["hello"]?.Value<String>(),                             Is.EqualTo("EVSE world!"));

                Assert.That(pushEVSEDataRequest.OperatorEVSEData.OperatorId.         ToString(),          Is.EqualTo("DE*GEF"));
                Assert.That(pushEVSEDataRequest.OperatorEVSEData.OperatorName,                            Is.EqualTo("GraphDefined"));
                Assert.That(pushEVSEDataRequest.OperatorEVSEData.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(pushEVSEDataRequest.OperatorEVSEData.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("CPO data world!"));

                Assert.That(pushEVSEDataRequest.Action,                                                   Is.EqualTo(ActionTypes.FullLoad));
                Assert.That(pushEVSEDataRequest.CustomData?.Count,                                        Is.EqualTo(1));
                Assert.That(pushEVSEDataRequest.CustomData?["hello"]?.Value<String>(),                    Is.EqualTo("PushData world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClient.   OnPushEVSEDataResponse += (timestamp, cpoClient,    pushEVSEDataRequest, oicpResponse, runtime) => {

                var pushEVSEDataResponse = oicpResponse.Response;

                Assert.That(pushEVSEDataResponse,                                                         Is.Not.Null);
                Assert.That(pushEVSEDataResponse?.Result,                                                 Is.True);
                Assert.That(pushEVSEDataResponse?.StatusCode.Code,                                        Is.EqualTo(StatusCodes.Success));

                Assert.That(pushEVSEDataResponse?.SessionId,                                              Is.Null);
                Assert.That(pushEVSEDataResponse?.CPOPartnerSessionId,                                    Is.Null);
                Assert.That(pushEVSEDataResponse?.EMPPartnerSessionId,                                    Is.Null);

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnPushEVSEDataRequest  += (timestamp, cpoClientAPI, pushEVSEDataRequest) => {

                var evseData1 = pushEVSEDataRequest.OperatorEVSEData.EVSEDataRecords.FirstOrDefault();

                Assert.That(evseData1?.Id,                                                                Is.EqualTo(EVSE_Id.Parse("DE*GEF*E1234567*A*1")));
                Assert.That(evseData1?.OperatorId,                                                        Is.EqualTo(Operator_Id.Parse("DE*GEF")));
                Assert.That(evseData1?.OperatorName,                                                      Is.EqualTo("GraphDefined"));
                Assert.That(evseData1?.ChargingStationName[LanguageCode.de],                              Is.EqualTo("Station Eins"));
                Assert.That(evseData1?.ChargingStationName[LanguageCode.en],                              Is.EqualTo("Station One"));
                Assert.That(evseData1?.Address.Country,                                                   Is.EqualTo(Country.Germany));
                Assert.That(evseData1?.Address.City,                                                      Is.EqualTo("Jena"));
                Assert.That(evseData1?.Address.Street,                                                    Is.EqualTo("Biberweg"));
                Assert.That(evseData1?.Address.PostalCode,                                                Is.EqualTo("07749"));
                Assert.That(evseData1?.Address.HouseNumber,                                               Is.EqualTo("18"));
                Assert.That(evseData1?.Address.Floor,                                                     Is.EqualTo("2"));
                Assert.That(evseData1?.Address.Region,                                                    Is.EqualTo("TH"));
                Assert.That(evseData1?.Address.ParkingFacility,                                           Is.False);
                Assert.That(evseData1?.Address.ParkingSpot,                                               Is.EqualTo("A1"));
                Assert.That(evseData1?.Address.TimeZone,                                                  Is.EqualTo(Time_Zone.Parse("UTC+02:00")));
                Assert.That(evseData1?.GeoCoordinates.Latitude,                                           Is.EqualTo(50.927054). Within(1e-6));
                Assert.That(evseData1?.GeoCoordinates.Longitude,                                          Is.EqualTo(11.5892372).Within(1e-6));
                Assert.That(evseData1?.GeoCoordinates.GeoFormat,                                          Is.EqualTo(GeoCoordinatesFormats.DecimalDegree));
                Assert.That(evseData1?.PlugTypes,                                                         Is.EquivalentTo([
                                                                                                              PlugTypes.CCSCombo2Plug_CableAttached
                                                                                                          ]));
                Assert.That(evseData1?.ChargingFacilities.Count(),                                        Is.EqualTo(1));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).PowerType,                         Is.EqualTo(PowerTypes.AC_3_PHASE));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).Power,                             Is.EqualTo(22));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).Voltage,                           Is.EqualTo(400));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).Amperage,                          Is.EqualTo(32));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).ChargingModes,                     Is.EquivalentTo([
                                                                                                              ChargingModes.Mode_2
                                                                                                          ]));
                Assert.That(evseData1?.RenewableEnergy,                                                   Is.True);
                Assert.That(evseData1?.CalibrationLawDataAvailability,                                    Is.EqualTo(CalibrationLawDataAvailabilities.External));
                Assert.That(evseData1?.AuthenticationModes,                                               Is.EquivalentTo([
                                                                                                              AuthenticationModes.NFC_RFID_Classic,
                                                                                                              AuthenticationModes.REMOTE
                                                                                                          ]));
                Assert.That(evseData1?.PaymentOptions,                                                    Is.EquivalentTo([
                                                                                                              PaymentOptions.Contract,
                                                                                                              PaymentOptions.Direct
                                                                                                          ]));
                Assert.That(evseData1?.ValueAddedServices,                                                Is.EquivalentTo([
                                                                                                              ValueAddedServices.Reservation,
                                                                                                              ValueAddedServices.DynamicPricing,
                                                                                                              ValueAddedServices.ParkingSensors,
                                                                                                              ValueAddedServices.MaximumPowerCharging,
                                                                                                              ValueAddedServices.PredictiveChargePointUsage,
                                                                                                              ValueAddedServices.ChargingPlans,
                                                                                                              ValueAddedServices.RoofProvided
                                                                                                          ]));
                Assert.That(evseData1?.Accessibility,                                                     Is.EqualTo(AccessibilityTypes.PayingPubliclyAccessible));
                Assert.That(evseData1?.HotlinePhoneNumber,                                                Is.EqualTo(Phone_Number.Parse("+49555123456")));
                Assert.That(evseData1?.IsOpen24Hours,                                                     Is.True);
                Assert.That(evseData1?.IsHubjectCompatible,                                               Is.True);
                Assert.That(evseData1?.DynamicInfoAvailable,                                              Is.EqualTo(FalseTrueAuto.True));
                Assert.That(evseData1?.DeltaType,                                                         Is.Null);
                Assert.That(evseData1?.LastUpdate,                                                        Is.Null);

                Assert.That(evseData1?.ChargingStationId,                                                 Is.EqualTo(ChargingStation_Id.Parse("DE*GEF*S123*4567")));
                Assert.That(evseData1?.ChargingPoolId,                                                    Is.EqualTo(ChargingPool_Id.   Parse("DE*GEF*P123")));
                Assert.That(evseData1?.HardwareManufacturer,                                              Is.EqualTo("GraphDefined Ladestations Manufaktur GmbH"));
                Assert.That(evseData1?.ChargingStationImageURL,                                           Is.EqualTo(URL.Parse("https://open.charging.cloud")));
                Assert.That(evseData1?.SubOperatorName,                                                   Is.EqualTo("GraphDefined Low Voltage Services"));
                Assert.That(evseData1?.DynamicPowerLevel,                                                 Is.True);
                Assert.That(evseData1?.EnergySources,                                                     Is.EqualTo([
                                                                                                              new EnergySource(
                                                                                                                  EnergyTypes.Solar,
                                                                                                                  60
                                                                                                              ),
                                                                                                              new EnergySource(
                                                                                                                  EnergyTypes.Wind,
                                                                                                                  40
                                                                                                              )
                                                                                                          ]));
                Assert.That(evseData1?.EnvironmentalImpact,                                               Is.EqualTo(
                                                                                                              new EnvironmentalImpact(
                                                                                                                  CO2Emission:  400,
                                                                                                                  NuclearWaste:   0
                                                                                                              )));
                Assert.That(evseData1?.MaxCapacity,                                                       Is.EqualTo(WattHour.ParseKWh(350)));
                Assert.That(evseData1?.AccessibilityLocationType,                                         Is.EqualTo(AccessibilityLocationTypes.OnStreet));
                Assert.That(evseData1?.AdditionalInfo?[LanguageCode.de],                                  Is.EqualTo("Wird mit Schokolade betrieben!"));
                Assert.That(evseData1?.AdditionalInfo?[LanguageCode.en],                                  Is.EqualTo("Runs on chocolate!"));
                Assert.That(evseData1?.ChargingStationLocationReference?[LanguageCode.de],                Is.EqualTo("Beim Hausmeister klingeln!"));
                Assert.That(evseData1?.ChargingStationLocationReference?[LanguageCode.en],                Is.EqualTo("Please ring the bell!"));
                Assert.That(evseData1?.GeoChargingPointEntrance?.Latitude,                                Is.EqualTo(50.8).Within(1e-6));
                Assert.That(evseData1?.GeoChargingPointEntrance?.Longitude,                               Is.EqualTo(11.6).Within(1e-6));
                //Assert.That(evseData1?.OpeningTimes,                                                      Is.Empty);
                Assert.That(evseData1?.HubOperatorId?.  ToString(),                                       Is.EqualTo("DE*HUB"));
                Assert.That(evseData1?.ClearingHouseId?.ToString(),                                       Is.EqualTo("DE*CLR"));
                Assert.That(evseData1?.CustomData?.Count,                                                 Is.EqualTo(1));
                Assert.That(evseData1?.CustomData?["hello"]?.Value<String>(),                             Is.EqualTo("EVSE world!"));

                Assert.That(pushEVSEDataRequest.OperatorEVSEData.OperatorId.         ToString(),          Is.EqualTo("DE*GEF"));
                Assert.That(pushEVSEDataRequest.OperatorEVSEData.OperatorName,                            Is.EqualTo("GraphDefined"));
                Assert.That(pushEVSEDataRequest.OperatorEVSEData.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(pushEVSEDataRequest.OperatorEVSEData.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("CPO data world!"));

                Assert.That(pushEVSEDataRequest.Action,                                                   Is.EqualTo(ActionTypes.FullLoad));
                Assert.That(pushEVSEDataRequest.CustomData?.Count,                                        Is.EqualTo(1));
                Assert.That(pushEVSEDataRequest.CustomData?["hello"]?.Value<String>(),                    Is.EqualTo("PushData world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnPushEVSEDataResponse += (timestamp, cpoClientAPI, pushEVSEDataRequest, oicpResponse, runtime) => {

                var pushEVSEDataResponse = oicpResponse.Response;

                Assert.That(pushEVSEDataResponse,                                                         Is.Not.Null);
                Assert.That(pushEVSEDataResponse?.Result,                                                 Is.True);
                Assert.That(pushEVSEDataResponse?.StatusCode.Code,                                        Is.EqualTo(StatusCodes.Success));

                Assert.That(pushEVSEDataResponse?.SessionId,                                              Is.Null);
                Assert.That(pushEVSEDataResponse?.CPOPartnerSessionId,                                    Is.Null);
                Assert.That(pushEVSEDataResponse?.EMPPartnerSessionId,                                    Is.Null);

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await cpoClient.PushEVSEData(request);

            Assert.That(oicpResult,                                                                       Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                          Is.True);
            Assert.That(oicpResult.Response?.Result,                                                      Is.True);
            Assert.That(oicpResult.Response?.StatusCode.Code,                                             Is.EqualTo(StatusCodes.Success));

            Assert.That(cpoClient.   Counters.PushEVSEData.Requests_OK,                                   Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.PushEVSEData.Requests_Error,                                Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushEVSEData.Responses_OK,                                  Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.PushEVSEData.Responses_Error,                               Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.PushEVSEData.Requests_OK,                                   Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.PushEVSEData.Requests_Error,                                Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushEVSEData.Responses_OK,                                  Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.PushEVSEData.Responses_Error,                               Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                             Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                            Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                             Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                            Is.EqualTo(1));

        }

        #endregion

        #region PushEVSEData_Minimal1()

        [Test]
        public async Task PushEVSEData_Minimal1()
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

            var request                = new PushEVSEDataRequest(

                                             new OperatorEVSEData(

                                                 [
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
                                                                                                Floor:             "2",
                                                                                                Region:            "TH",
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
                                                         PlugTypes:                         [
                                                                                                PlugTypes.CCSCombo2Plug_CableAttached
                                                                                            ],
                                                         ChargingFacilities:                [
                                                                                                new ChargingFacility(
                                                                                                    PowerType:      PowerTypes.AC_3_PHASE,
                                                                                                    Power:          22,
                                                                                                    Voltage:        400,
                                                                                                    Amperage:       32,
                                                                                                    ChargingModes:  [
                                                                                                                        ChargingModes.Mode_2
                                                                                                                    ],
                                                                                                    CustomData:     null
                                                                                                )
                                                                                            ],
                                                         RenewableEnergy:                   true,
                                                         CalibrationLawDataAvailability:    CalibrationLawDataAvailabilities.External,
                                                         AuthenticationModes:               [
                                                                                                AuthenticationModes.NFC_RFID_Classic,
                                                                                                AuthenticationModes.REMOTE
                                                                                            ],
                                                         PaymentOptions:                    [
                                                                                                PaymentOptions.Contract,
                                                                                                PaymentOptions.Direct
                                                                                            ],
                                                         ValueAddedServices:                [
                                                                                                ValueAddedServices.Reservation,
                                                                                                ValueAddedServices.DynamicPricing,
                                                                                                ValueAddedServices.ParkingSensors,
                                                                                                ValueAddedServices.MaximumPowerCharging,
                                                                                                ValueAddedServices.PredictiveChargePointUsage,
                                                                                                ValueAddedServices.ChargingPlans,
                                                                                                ValueAddedServices.RoofProvided
                                                                                            ],
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

                                                         CustomData:                        new JObject(
                                                                                                new JProperty("hello", "EVSE world!")
                                                                                            )

                                                     )
                                                 ],

                                                 Operator_Id.Parse("DE*GEF"),
                                                 "GraphDefined",
                                                 new JObject(
                                                     new JProperty("hello", "CPO data world!")
                                                 )

                                             ),

                                             ActionTypes.FullLoad,

                                             CustomData:  new JObject(
                                                              new JProperty("hello", "PushData world!")
                                                          )

                                         );

            Assert.That(request,                                                                          Is.Not.Null);

            Assert.That(cpoClient.   Counters.PushEVSEData.Requests_OK,                                   Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushEVSEData.Requests_Error,                                Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushEVSEData.Responses_OK,                                  Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushEVSEData.Responses_Error,                               Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.PushEVSEData.Requests_OK,                                   Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushEVSEData.Requests_Error,                                Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushEVSEData.Responses_OK,                                  Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushEVSEData.Responses_Error,                               Is.EqualTo(0));

            cpoClient.   OnPushEVSEDataRequest  += (timestamp, cpoClient,    pushEVSEDataRequest) => {

                var evseData1 = pushEVSEDataRequest.OperatorEVSEData.EVSEDataRecords.FirstOrDefault();

                Assert.That(evseData1?.Id,                                                                Is.EqualTo(EVSE_Id.Parse("DE*GEF*E1234567*A*1")));
                Assert.That(evseData1?.OperatorId,                                                        Is.EqualTo(Operator_Id.Parse("DE*GEF")));
                Assert.That(evseData1?.OperatorName,                                                      Is.EqualTo("GraphDefined"));
                Assert.That(evseData1?.ChargingStationName[LanguageCode.de],                              Is.EqualTo("Station Eins"));
                Assert.That(evseData1?.ChargingStationName[LanguageCode.en],                              Is.EqualTo("Station One"));
                Assert.That(evseData1?.Address.Country,                                                   Is.EqualTo(Country.Germany));
                Assert.That(evseData1?.Address.City,                                                      Is.EqualTo("Jena"));
                Assert.That(evseData1?.Address.Street,                                                    Is.EqualTo("Biberweg"));
                Assert.That(evseData1?.Address.PostalCode,                                                Is.EqualTo("07749"));
                Assert.That(evseData1?.Address.HouseNumber,                                               Is.EqualTo("18"));
                Assert.That(evseData1?.Address.Floor,                                                     Is.EqualTo("2"));
                Assert.That(evseData1?.Address.Region,                                                    Is.EqualTo("TH"));
                Assert.That(evseData1?.Address.ParkingFacility,                                           Is.False);
                Assert.That(evseData1?.Address.ParkingSpot,                                               Is.EqualTo("A1"));
                Assert.That(evseData1?.Address.TimeZone,                                                  Is.EqualTo(Time_Zone.Parse("UTC+02:00")));
                Assert.That(evseData1?.GeoCoordinates.Latitude,                                           Is.EqualTo(50.927054));
                Assert.That(evseData1?.GeoCoordinates.Longitude,                                          Is.EqualTo(11.5892372));
                Assert.That(evseData1?.GeoCoordinates.GeoFormat,                                          Is.EqualTo(GeoCoordinatesFormats.DecimalDegree));
                Assert.That(evseData1?.PlugTypes,                                                         Is.EquivalentTo([
                                                                                                              PlugTypes.CCSCombo2Plug_CableAttached
                                                                                                          ]));
                Assert.That(evseData1?.ChargingFacilities.Count(),                                        Is.EqualTo(1));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).PowerType,                         Is.EqualTo(PowerTypes.AC_3_PHASE));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).Power,                             Is.EqualTo(22));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).Voltage,                           Is.EqualTo(400));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).Amperage,                          Is.EqualTo(32));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).ChargingModes,                     Is.EquivalentTo([
                                                                                                              ChargingModes.Mode_2
                                                                                                          ]));
                Assert.That(evseData1?.RenewableEnergy,                                                   Is.True);
                Assert.That(evseData1?.CalibrationLawDataAvailability,                                    Is.EqualTo(CalibrationLawDataAvailabilities.External));
                Assert.That(evseData1?.AuthenticationModes,                                               Is.EquivalentTo([
                                                                                                              AuthenticationModes.NFC_RFID_Classic,
                                                                                                              AuthenticationModes.REMOTE
                                                                                                          ]));
                Assert.That(evseData1?.PaymentOptions,                                                    Is.EquivalentTo([
                                                                                                              PaymentOptions.Contract,
                                                                                                              PaymentOptions.Direct
                                                                                                          ]));
                Assert.That(evseData1?.ValueAddedServices,                                                Is.EquivalentTo([
                                                                                                              ValueAddedServices.Reservation,
                                                                                                              ValueAddedServices.DynamicPricing,
                                                                                                              ValueAddedServices.ParkingSensors,
                                                                                                              ValueAddedServices.MaximumPowerCharging,
                                                                                                              ValueAddedServices.PredictiveChargePointUsage,
                                                                                                              ValueAddedServices.ChargingPlans,
                                                                                                              ValueAddedServices.RoofProvided
                                                                                                          ]));
                Assert.That(evseData1?.Accessibility,                                                     Is.EqualTo(AccessibilityTypes.PayingPubliclyAccessible));
                Assert.That(evseData1?.HotlinePhoneNumber,                                                Is.EqualTo(Phone_Number.Parse("+49555123456")));
                Assert.That(evseData1?.IsOpen24Hours,                                                     Is.True);
                Assert.That(evseData1?.IsHubjectCompatible,                                               Is.True);
                Assert.That(evseData1?.DynamicInfoAvailable,                                              Is.EqualTo(FalseTrueAuto.True));
                Assert.That(evseData1?.DeltaType,                                                         Is.Null);
                Assert.That(evseData1?.LastUpdate,                                                        Is.Null);
                Assert.That(evseData1?.ChargingStationId,                                                 Is.Null);
                Assert.That(evseData1?.ChargingPoolId,                                                    Is.Null);
                Assert.That(evseData1?.HardwareManufacturer,                                              Is.Null);
                Assert.That(evseData1?.ChargingStationImageURL,                                           Is.Null);
                Assert.That(evseData1?.SubOperatorName,                                                   Is.Null);
                Assert.That(evseData1?.DynamicPowerLevel,                                                 Is.Null);
                Assert.That(evseData1?.EnergySources,                                                     Is.Empty);
                Assert.That(evseData1?.EnvironmentalImpact,                                               Is.Null);
                Assert.That(evseData1?.MaxCapacity,                                                       Is.Null);
                Assert.That(evseData1?.AccessibilityLocationType,                                         Is.Null);
                Assert.That(evseData1?.AdditionalInfo,                                                    Is.Null);
                Assert.That(evseData1?.ChargingStationLocationReference,                                  Is.Null);
                Assert.That(evseData1?.GeoChargingPointEntrance,                                          Is.Null);
                Assert.That(evseData1?.OpeningTimes,                                                      Is.Empty);
                Assert.That(evseData1?.HubOperatorId,                                                     Is.Null);
                Assert.That(evseData1?.ClearingHouseId,                                                   Is.Null);
                Assert.That(evseData1?.CustomData?.Count,                                                 Is.EqualTo(1));
                Assert.That(evseData1?.CustomData?["hello"]?.Value<String>(),                             Is.EqualTo("EVSE world!"));

                Assert.That(pushEVSEDataRequest.OperatorEVSEData.OperatorId.         ToString(),          Is.EqualTo("DE*GEF"));
                Assert.That(pushEVSEDataRequest.OperatorEVSEData.OperatorName,                            Is.EqualTo("GraphDefined"));
                Assert.That(pushEVSEDataRequest.OperatorEVSEData.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(pushEVSEDataRequest.OperatorEVSEData.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("CPO data world!"));

                Assert.That(pushEVSEDataRequest.Action,                                                   Is.EqualTo(ActionTypes.FullLoad));
                Assert.That(pushEVSEDataRequest.CustomData?.Count,                                        Is.EqualTo(1));
                Assert.That(pushEVSEDataRequest.CustomData?["hello"]?.Value<String>(),                    Is.EqualTo("PushData world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClient.   OnPushEVSEDataResponse += (timestamp, cpoClient,    pushEVSEDataRequest, oicpResponse, runtime) => {

                var pushEVSEDataResponse = oicpResponse.Response;

                Assert.That(pushEVSEDataResponse,                                                         Is.Not.Null);
                Assert.That(pushEVSEDataResponse?.Result,                                                 Is.True);
                Assert.That(pushEVSEDataResponse?.StatusCode.Code,                                        Is.EqualTo(StatusCodes.Success));

                Assert.That(pushEVSEDataResponse?.SessionId,                                              Is.Null);
                Assert.That(pushEVSEDataResponse?.CPOPartnerSessionId,                                    Is.Null);
                Assert.That(pushEVSEDataResponse?.EMPPartnerSessionId,                                    Is.Null);

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnPushEVSEDataRequest  += (timestamp, cpoClientAPI, pushEVSEDataRequest) => {

                var evseData1 = pushEVSEDataRequest.OperatorEVSEData.EVSEDataRecords.FirstOrDefault();

                Assert.That(evseData1?.Id,                                                                Is.EqualTo(EVSE_Id.Parse("DE*GEF*E1234567*A*1")));
                Assert.That(evseData1?.OperatorId,                                                        Is.EqualTo(Operator_Id.Parse("DE*GEF")));
                Assert.That(evseData1?.OperatorName,                                                      Is.EqualTo("GraphDefined"));
                Assert.That(evseData1?.ChargingStationName[LanguageCode.de],                              Is.EqualTo("Station Eins"));
                Assert.That(evseData1?.ChargingStationName[LanguageCode.en],                              Is.EqualTo("Station One"));
                Assert.That(evseData1?.Address.Country,                                                   Is.EqualTo(Country.Germany));
                Assert.That(evseData1?.Address.City,                                                      Is.EqualTo("Jena"));
                Assert.That(evseData1?.Address.Street,                                                    Is.EqualTo("Biberweg"));
                Assert.That(evseData1?.Address.PostalCode,                                                Is.EqualTo("07749"));
                Assert.That(evseData1?.Address.HouseNumber,                                               Is.EqualTo("18"));
                Assert.That(evseData1?.Address.Floor,                                                     Is.EqualTo("2"));
                Assert.That(evseData1?.Address.Region,                                                    Is.EqualTo("TH"));
                Assert.That(evseData1?.Address.ParkingFacility,                                           Is.False);
                Assert.That(evseData1?.Address.ParkingSpot,                                               Is.EqualTo("A1"));
                Assert.That(evseData1?.Address.TimeZone,                                                  Is.EqualTo(Time_Zone.Parse("UTC+02:00")));
                Assert.That(evseData1?.GeoCoordinates.Latitude,                                           Is.EqualTo(50.927054). Within(1e-6));
                Assert.That(evseData1?.GeoCoordinates.Longitude,                                          Is.EqualTo(11.5892372).Within(1e-6));
                Assert.That(evseData1?.GeoCoordinates.GeoFormat,                                          Is.EqualTo(GeoCoordinatesFormats.DecimalDegree));
                Assert.That(evseData1?.PlugTypes,                                                         Is.EquivalentTo([
                                                                                                              PlugTypes.CCSCombo2Plug_CableAttached
                                                                                                          ]));
                Assert.That(evseData1?.ChargingFacilities.Count(),                                        Is.EqualTo(1));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).PowerType,                         Is.EqualTo(PowerTypes.AC_3_PHASE));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).Power,                             Is.EqualTo(22));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).Voltage,                           Is.EqualTo(400));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).Amperage,                          Is.EqualTo(32));
                Assert.That(evseData1?.ChargingFacilities.ElementAt(0).ChargingModes,                     Is.EquivalentTo([
                                                                                                              ChargingModes.Mode_2
                                                                                                          ]));
                Assert.That(evseData1?.RenewableEnergy,                                                   Is.True);
                Assert.That(evseData1?.CalibrationLawDataAvailability,                                    Is.EqualTo(CalibrationLawDataAvailabilities.External));
                Assert.That(evseData1?.AuthenticationModes,                                               Is.EquivalentTo([
                                                                                                              AuthenticationModes.NFC_RFID_Classic,
                                                                                                              AuthenticationModes.REMOTE
                                                                                                          ]));
                Assert.That(evseData1?.PaymentOptions,                                                    Is.EquivalentTo([
                                                                                                              PaymentOptions.Contract,
                                                                                                              PaymentOptions.Direct
                                                                                                          ]));
                Assert.That(evseData1?.ValueAddedServices,                                                Is.EquivalentTo([
                                                                                                              ValueAddedServices.Reservation,
                                                                                                              ValueAddedServices.DynamicPricing,
                                                                                                              ValueAddedServices.ParkingSensors,
                                                                                                              ValueAddedServices.MaximumPowerCharging,
                                                                                                              ValueAddedServices.PredictiveChargePointUsage,
                                                                                                              ValueAddedServices.ChargingPlans,
                                                                                                              ValueAddedServices.RoofProvided
                                                                                                          ]));
                Assert.That(evseData1?.Accessibility,                                                     Is.EqualTo(AccessibilityTypes.PayingPubliclyAccessible));
                Assert.That(evseData1?.HotlinePhoneNumber,                                                Is.EqualTo(Phone_Number.Parse("+49555123456")));
                Assert.That(evseData1?.IsOpen24Hours,                                                     Is.True);
                Assert.That(evseData1?.IsHubjectCompatible,                                               Is.True);
                Assert.That(evseData1?.DynamicInfoAvailable,                                              Is.EqualTo(FalseTrueAuto.True));
                Assert.That(evseData1?.DeltaType,                                                         Is.Null);
                Assert.That(evseData1?.LastUpdate,                                                        Is.Null);
                Assert.That(evseData1?.ChargingStationId,                                                 Is.Null);
                Assert.That(evseData1?.ChargingPoolId,                                                    Is.Null);
                Assert.That(evseData1?.HardwareManufacturer,                                              Is.Null);
                Assert.That(evseData1?.ChargingStationImageURL,                                           Is.Null);
                Assert.That(evseData1?.SubOperatorName,                                                   Is.Null);
                Assert.That(evseData1?.DynamicPowerLevel,                                                 Is.Null);
                Assert.That(evseData1?.EnergySources,                                                     Is.Empty);
                Assert.That(evseData1?.EnvironmentalImpact,                                               Is.Null);
                Assert.That(evseData1?.MaxCapacity,                                                       Is.Null);
                Assert.That(evseData1?.AccessibilityLocationType,                                         Is.Null);
                Assert.That(evseData1?.AdditionalInfo,                                                    Is.Null);
                Assert.That(evseData1?.ChargingStationLocationReference,                                  Is.Null);
                Assert.That(evseData1?.GeoChargingPointEntrance,                                          Is.Null);
                Assert.That(evseData1?.OpeningTimes,                                                      Is.Empty);
                Assert.That(evseData1?.HubOperatorId,                                                     Is.Null);
                Assert.That(evseData1?.ClearingHouseId,                                                   Is.Null);
                Assert.That(evseData1?.CustomData?.Count,                                                 Is.EqualTo(1));
                Assert.That(evseData1?.CustomData?["hello"]?.Value<String>(),                             Is.EqualTo("EVSE world!"));

                Assert.That(pushEVSEDataRequest.OperatorEVSEData.OperatorId.         ToString(),          Is.EqualTo("DE*GEF"));
                Assert.That(pushEVSEDataRequest.OperatorEVSEData.OperatorName,                            Is.EqualTo("GraphDefined"));
                Assert.That(pushEVSEDataRequest.OperatorEVSEData.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(pushEVSEDataRequest.OperatorEVSEData.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("CPO data world!"));

                Assert.That(pushEVSEDataRequest.Action,                                                   Is.EqualTo(ActionTypes.FullLoad));
                Assert.That(pushEVSEDataRequest.CustomData?.Count,                                        Is.EqualTo(1));
                Assert.That(pushEVSEDataRequest.CustomData?["hello"]?.Value<String>(),                    Is.EqualTo("PushData world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnPushEVSEDataResponse += (timestamp, cpoClientAPI, pushEVSEDataRequest, oicpResponse, runtime) => {

                var pushEVSEDataResponse = oicpResponse.Response;

                Assert.That(pushEVSEDataResponse,                                                         Is.Not.Null);
                Assert.That(pushEVSEDataResponse?.Result,                                                 Is.True);
                Assert.That(pushEVSEDataResponse?.StatusCode.Code,                                        Is.EqualTo(StatusCodes.Success));

                Assert.That(pushEVSEDataResponse?.SessionId,                                              Is.Null);
                Assert.That(pushEVSEDataResponse?.CPOPartnerSessionId,                                    Is.Null);
                Assert.That(pushEVSEDataResponse?.EMPPartnerSessionId,                                    Is.Null);

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await cpoClient.PushEVSEData(request);

            Assert.That(oicpResult,                                                                       Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                          Is.True);
            Assert.That(oicpResult.Response?.Result,                                                      Is.True);
            Assert.That(oicpResult.Response?.StatusCode.Code,                                             Is.EqualTo(StatusCodes.Success));

            Assert.That(cpoClient.   Counters.PushEVSEData.Requests_OK,                                   Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.PushEVSEData.Requests_Error,                                Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushEVSEData.Responses_OK,                                  Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.PushEVSEData.Responses_Error,                               Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.PushEVSEData.Requests_OK,                                   Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.PushEVSEData.Requests_Error,                                Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushEVSEData.Responses_OK,                                  Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.PushEVSEData.Responses_Error,                               Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                             Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                            Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                             Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                            Is.EqualTo(1));

        }

        #endregion


        #region PushEVSEStatus_Test1()

        [Test]
        public async Task PushEVSEStatus_Test1()
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

            var request = new PushEVSEStatusRequest(

                              OperatorEVSEStatus:   new OperatorEVSEStatus(

                                                        EVSEStatusRecords:   [
                                                                                 new EVSEStatusRecord(
                                                                                     Id:          EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                                                                     Status:      EVSEStatusTypes.Available,
                                                                                     CustomData:  new JObject(
                                                                                                      new JProperty("hello", "EVSE world!")
                                                                                                  )
                                                                                 )
                                                                             ],

                                                        OperatorId:          Operator_Id.Parse("DE*GEF"),
                                                        OperatorName:        "GraphDefined",
                                                        CustomData:          new JObject(
                                                                                 new JProperty("hello", "CPO status world!")
                                                                             )

                                                    ),

                              Action:               ActionTypes.FullLoad,

                              CustomData:           new JObject(
                                                        new JProperty("hello", "PushStatus world!")
                                                    )

                          );

            Assert.That(request,                                                                            Is.Not.Null);

            Assert.That(cpoClient.   Counters.PushEVSEStatus.Requests_OK,                                   Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushEVSEStatus.Requests_Error,                                Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushEVSEStatus.Responses_OK,                                  Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushEVSEStatus.Responses_Error,                               Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.PushEVSEStatus.Requests_OK,                                   Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushEVSEStatus.Requests_Error,                                Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushEVSEStatus.Responses_OK,                                  Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushEVSEStatus.Responses_Error,                               Is.EqualTo(0));

            cpoClient.   OnPushEVSEStatusRequest  += (timestamp, cpoClient,    pushEVSEDataRequest) => {

                var evseStatus1 = pushEVSEDataRequest.OperatorEVSEStatus.EVSEStatusRecords.FirstOrDefault();

                Assert.That(evseStatus1.Id.ToString(),                                                      Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(evseStatus1.Status, Is.EqualTo(EVSEStatusTypes.Available));
                Assert.That(evseStatus1.CustomData?.Count,                                                  Is.EqualTo(1));
                Assert.That(evseStatus1.CustomData?["hello"]?.Value<String>(),                              Is.EqualTo("EVSE world!"));

                Assert.That(pushEVSEDataRequest.OperatorEVSEStatus.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(pushEVSEDataRequest.OperatorEVSEStatus.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("CPO status world!"));

                Assert.That(pushEVSEDataRequest.CustomData?.Count,                                          Is.EqualTo(1));
                Assert.That(pushEVSEDataRequest.CustomData?["hello"]?.Value<String>(),                      Is.EqualTo("PushStatus world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClient.   OnPushEVSEStatusResponse += (timestamp, cpoClient,    pushEVSEDataRequest, oicpResponse, runtime) => {

                var pushEVSEDataResponse = oicpResponse.Response;

                Assert.That(pushEVSEDataResponse,                                Is.Not.Null);
                Assert.That(pushEVSEDataResponse?.Result,                        Is.True);
                Assert.That(pushEVSEDataResponse?.StatusCode.Code,               Is.EqualTo(StatusCodes.Success));

                Assert.That(pushEVSEDataResponse?.SessionId,                     Is.Null);
                Assert.That(pushEVSEDataResponse?.CPOPartnerSessionId,           Is.Null);
                Assert.That(pushEVSEDataResponse?.EMPPartnerSessionId,           Is.Null);

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnPushEVSEStatusRequest  += (timestamp, cpoClientAPI, pushEVSEDataRequest) => {

                var evseStatus1 = pushEVSEDataRequest.OperatorEVSEStatus.EVSEStatusRecords.FirstOrDefault();

                Assert.That(evseStatus1.Id.ToString(),                                                      Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(evseStatus1.Status, Is.EqualTo(EVSEStatusTypes.Available));
                Assert.That(evseStatus1.CustomData?.Count,                                                  Is.EqualTo(1));
                Assert.That(evseStatus1.CustomData?["hello"]?.Value<String>(),                              Is.EqualTo("EVSE world!"));

                Assert.That(pushEVSEDataRequest.OperatorEVSEStatus.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(pushEVSEDataRequest.OperatorEVSEStatus.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("CPO status world!"));

                Assert.That(pushEVSEDataRequest.CustomData?.Count,                                          Is.EqualTo(1));
                Assert.That(pushEVSEDataRequest.CustomData?["hello"]?.Value<String>(),                      Is.EqualTo("PushStatus world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnPushEVSEStatusResponse += (timestamp, cpoClientAPI, pushEVSEDataRequest, oicpResponse, runtime) => {

                var pushEVSEDataResponse = oicpResponse.Response;

                Assert.That(pushEVSEDataResponse,                                Is.Not.Null);
                Assert.That(pushEVSEDataResponse?.Result,                        Is.True);
                Assert.That(pushEVSEDataResponse?.StatusCode.Code,               Is.EqualTo(StatusCodes.Success));

                Assert.That(pushEVSEDataResponse?.SessionId,                     Is.Null);
                Assert.That(pushEVSEDataResponse?.CPOPartnerSessionId,           Is.Null);
                Assert.That(pushEVSEDataResponse?.EMPPartnerSessionId,           Is.Null);

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await cpoClient.PushEVSEStatus(request);

            Assert.That(oicpResult,                                                                         Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                            Is.True);
            Assert.That(oicpResult.Response?.Result,                                                        Is.True);
            Assert.That(oicpResult.Response?.StatusCode.Code,                                               Is.EqualTo(StatusCodes.Success));

            Assert.That(cpoClient.   Counters.PushEVSEStatus.Requests_OK,                                   Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.PushEVSEStatus.Requests_Error,                                Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PushEVSEStatus.Responses_OK,                                  Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.PushEVSEStatus.Responses_Error,                               Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.PushEVSEStatus.Requests_OK,                                   Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.PushEVSEStatus.Requests_Error,                                Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PushEVSEStatus.Responses_OK,                                  Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.PushEVSEStatus.Responses_Error,                               Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                               Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                              Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                               Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                              Is.EqualTo(1));

        }

        #endregion

    }

}
