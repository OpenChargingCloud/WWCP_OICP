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

using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.datastructures
{

    /// <summary>
    /// Charge detail record tests.
    /// </summary>
    [TestFixture]
    public class ChargeDetailRecordTests
    {

        #region ChargeDetailRecord_ParseJSON_Test1()

        [Test]
        public void ChargeDetailRecord_ParseJSON_Test1()
        {

            var chargeDetailRecord = ChargeDetailRecord.Parse(new JObject(
                                         new JProperty("SessionID",              "759a8d7e-e668-4d36-912f-4AF396a57001"),
                                         new JProperty("CPOPartnerSessionID",    "8942-AFAEB4E2-B21C-4450-9D33-CE99E14FF5F3"),
                                         new JProperty("EMPPartnerSessionID",    "2455-G93666E2-A2CA-2451-2554-ABCC3678ACFA"),
                                         new JProperty("PartnerProductID",       "AC1"),
                                         new JProperty("EvseID",                 "DE*GEF*E905217*2"),
                                         new JProperty("Identification", new JObject(
                                             new JProperty("RFIDMifareFamilyIdentification", new JObject(
                                                 new JProperty("UID", "A4A44E64")
                                             )
                                         ))),
                                         new JProperty("ChargingStart",          "2022-07-15T06:26:55Z"),
                                         new JProperty("ChargingEnd",            "2022-07-15T06:53:54Z"),
                                         new JProperty("SessionStart",           "2022-07-15T06:26:55Z"),
                                         new JProperty("SessionEnd",             "2022-07-15T06:53:54Z"),
                                         new JProperty("MeterValueStart",          2.1),
                                         new JProperty("MeterValueEnd",           22.714),
                                         new JProperty("MeterValueInBetween",     null),
                                         new JProperty("SignedMeteringValues", new JArray(
                                             new JObject(
                                                 new JProperty("SignedMeteringValue", @"OCMF|{\""FV\"" : \""1.0\"",\""GI\"" : \""DZG-GSH01.1K2L\"",\""GS\"" : \""1DZG0028210907\"",\""GV\"" : \""230\"",\""PG\"" : \""T1264\"",\""MV\"" : \""DZG\"",\""MM\"" : \""GSH01.1K2L\"",\""MS\"" : \""1DZG0028210907\"",\""MF\"" : \""230\"",\""IS\"" : true,\""IT\"" : \""CENTRAL_1\"",\""ID\"" : \""A4A44E64\"",\""CT\"" : \""EVSEID\"",\""CI\"" : \""20BZ0413B1\"",\""RD\"" : [{\""TM\"" : \""2022-07-15T08:27:11,000+0200 I\"",\""TX\"" : \""B\"",\""RV\"" : \""0.000\"",\""RI\"" : \""01-00:98.08.00.FF\"",\""RU\"" : \""kWh\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""},{\""TM\"" : \""2022-07-15T08:53:43,000+0200 I\"",\""TX\"" : \""E\"",\""RV\"" : \""20.614\"",\""RI\"" : \""01-00:98.08.00.FF\"",\""RU\"" : \""kWh\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""}],\""U\"" : [{\""TM\"" : \""2022-07-15T08:27:11,000+0200 I\"",\""TX\"" : \""B\"",\""RV\"" : \""17359.195\"",\""RI\"" : \""01-00:9C.08.00.FF\"",\""RU\"" : \""kWh\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""},{\""TM\"" : \""2022-07-15T08:53:43,000+0200 I\"",\""TX\"" : \""E\"",\""RV\"" : \""17379.809\"",\""RI\"" : \""01-00:9C.08.00.FF\"",\""RU\"" : \""kWh\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""},{\""TM\"" : \""2022-07-15T08:27:11,000+0200 I\"",\""TX\"" : \""B\"",\""RV\"" : \""0.0031\"",\""RI\"" : \""01-00:8C.07.00.FF\"",\""RU\"" : \""Ohm\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""},{\""TM\"" : \""2022-07-15T08:53:43,000+0200 I\"",\""TX\"" : \""E\"",\""RV\"" : \""1592\"",\""RI\"" : \""01-00:00.08.06.FF\"",\""RU\"" : \""s\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""}]}|{\""SA\"" : \""ECDSA-secp256k1-SHA256\"",\""SD\"" : \""3046022100AEF09929FF8DAC28334BD8F19D52A955D7A19F7667A9ECE8FAA2422CFCD3580E022100C0DA212592607735D8838D7CB08A2F75D14970E6FDD757A2ADCF818E153AA0F0\"),
                                                 new JProperty("MeteringStatus",       "Start")
                                             ),
                                             new JObject(
                                                 new JProperty("SignedMeteringValue", @"OCMF|{\""FV\"" : \""1.0\"",\""GI\"" : \""DZG-GSH01.1K2L\"",\""GS\"" : \""1DZG0028210907\"",\""GV\"" : \""230\"",\""PG\"" : \""T1264\"",\""MV\"" : \""DZG\"",\""MM\"" : \""GSH01.1K2L\"",\""MS\"" : \""1DZG0028210907\"",\""MF\"" : \""230\"",\""IS\"" : true,\""IT\"" : \""CENTRAL_1\"",\""ID\"" : \""A4A44E64\"",\""CT\"" : \""EVSEID\"",\""CI\"" : \""20BZ0413B1\"",\""RD\"" : [{\""TM\"" : \""2022-07-15T08:27:11,000+0200 I\"",\""TX\"" : \""B\"",\""RV\"" : \""0.000\"",\""RI\"" : \""01-00:98.08.00.FF\"",\""RU\"" : \""kWh\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""},{\""TM\"" : \""2022-07-15T08:53:43,000+0200 I\"",\""TX\"" : \""E\"",\""RV\"" : \""20.614\"",\""RI\"" : \""01-00:98.08.00.FF\"",\""RU\"" : \""kWh\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""}],\""U\"" : [{\""TM\"" : \""2022-07-15T08:27:11,000+0200 I\"",\""TX\"" : \""B\"",\""RV\"" : \""17359.195\"",\""RI\"" : \""01-00:9C.08.00.FF\"",\""RU\"" : \""kWh\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""},{\""TM\"" : \""2022-07-15T08:53:43,000+0200 I\"",\""TX\"" : \""E\"",\""RV\"" : \""17379.809\"",\""RI\"" : \""01-00:9C.08.00.FF\"",\""RU\"" : \""kWh\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""},{\""TM\"" : \""2022-07-15T08:27:11,000+0200 I\"",\""TX\"" : \""B\"",\""RV\"" : \""0.0031\"",\""RI\"" : \""01-00:8C.07.00.FF\"",\""RU\"" : \""Ohm\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""},{\""TM\"" : \""2022-07-15T08:53:43,000+0200 I\"",\""TX\"" : \""E\"",\""RV\"" : \""1592\"",\""RI\"" : \""01-00:00.08.06.FF\"",\""RU\"" : \""s\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""}]}|{\""SA\"" : \""ECDSA-secp256k1-SHA256\"",\""SD\"" : \""3046022100AEF09929FF8DAC28334BD8F19D52A955D7A19F7667A9ECE8FAA2422CFCD3580E022100C0DA212592607735D8838D7CB08A2F75D14970E6FDD757A2ADCF818E153AA0F0\"),
                                                 new JProperty("MeteringStatus",       "End")
                                             )
                                         )),
                                         new JProperty("CalibrationLawVerificationInfo", new JObject(
                                             new JProperty("CalibrationLawCertificateID",                  "PTB123"),
                                             new JProperty("PublicKey",                                    "123456"),
                                             new JProperty("MeteringSignatureUrl",                         "https://open.charging.cloud"),
                                             new JProperty("MeteringSignatureEncodingFormat",              "OCMF"),
                                             new JProperty("SignedMeteringValuesVerificationInstruction",  "Use Chargy Transparency Software!")
                                         )),
                                         new JProperty("ConsumedEnergy",          20.614),
                                         new JProperty("HubOperatorID",          "DE*GEF"),
                                         new JProperty("HubProviderID",          "DE-GDF")
                                     ));

            ClassicAssert.IsNotNull(chargeDetailRecord);
            ClassicAssert.IsNotNull(chargeDetailRecord.CalibrationLawVerificationInfo);
            ClassicAssert.AreEqual("OCMF", chargeDetailRecord.CalibrationLawVerificationInfo?.MeteringSignatureEncodingFormat);

        }

        #endregion


        #region ChargeDetailRecord_ParseJSONText_Test1()

        [Test]
        public void ChargeDetailRecord_ParseJSONText_Test1()
        {

            var chargeDetailRecord = ChargeDetailRecord.Parse(JObject.Parse(@"{
                                         ""SessionID"":              ""759a8d7e-e668-4d36-912f-4AF396a57001"",
                                         ""CPOPartnerSessionID"":    ""8942-AFAEB4E2-B21C-4450-9D33-CE99E14FF5F3"",
                                         ""EMPPartnerSessionID"":    ""2455-G93666E2-A2CA-2451-2554-ABCC3678ACFA"",
                                         ""PartnerProductID"":       ""AC1"",
                                         ""EvseID"":                 ""DE*GEF*E905217*2"",
                                         ""Identification"": {
                                             ""RFIDMifareFamilyIdentification"": {
                                                 ""UID"": ""A4A44E64""
                                             }
                                         },
                                         ""ChargingStart"":      ""2022-07-15T06:26:55Z"",
                                         ""ChargingEnd"":        ""2022-07-15T06:53:54Z"",
                                         ""SessionStart"":       ""2022-07-15T06:26:55Z"",
                                         ""SessionEnd"":         ""2022-07-15T06:53:54Z"",
                                         ""MeterValueStart"":      2.1,
                                         ""MeterValueEnd"":       22.714,
                                         ""MeterValueInBetween"": null,
                                         ""SignedMeteringValues"": [
                                             {
                                                 ""SignedMeteringValue"": ""OCMF|{\""FV\"" : \""1.0\"",\""GI\"" : \""DZG-GSH01.1K2L\"",\""GS\"" : \""1DZG0028210907\"",\""GV\"" : \""230\"",\""PG\"" : \""T1264\"",\""MV\"" : \""DZG\"",\""MM\"" : \""GSH01.1K2L\"",\""MS\"" : \""1DZG0028210907\"",\""MF\"" : \""230\"",\""IS\"" : true,\""IT\"" : \""CENTRAL_1\"",\""ID\"" : \""A4A44E64\"",\""CT\"" : \""EVSEID\"",\""CI\"" : \""20BZ0413B1\"",\""RD\"" : [{\""TM\"" : \""2022-07-15T08:27:11,000+0200 I\"",\""TX\"" : \""B\"",\""RV\"" : \""0.000\"",\""RI\"" : \""01-00:98.08.00.FF\"",\""RU\"" : \""kWh\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""},{\""TM\"" : \""2022-07-15T08:53:43,000+0200 I\"",\""TX\"" : \""E\"",\""RV\"" : \""20.614\"",\""RI\"" : \""01-00:98.08.00.FF\"",\""RU\"" : \""kWh\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""}],\""U\"" : [{\""TM\"" : \""2022-07-15T08:27:11,000+0200 I\"",\""TX\"" : \""B\"",\""RV\"" : \""17359.195\"",\""RI\"" : \""01-00:9C.08.00.FF\"",\""RU\"" : \""kWh\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""},{\""TM\"" : \""2022-07-15T08:53:43,000+0200 I\"",\""TX\"" : \""E\"",\""RV\"" : \""17379.809\"",\""RI\"" : \""01-00:9C.08.00.FF\"",\""RU\"" : \""kWh\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""},{\""TM\"" : \""2022-07-15T08:27:11,000+0200 I\"",\""TX\"" : \""B\"",\""RV\"" : \""0.0031\"",\""RI\"" : \""01-00:8C.07.00.FF\"",\""RU\"" : \""Ohm\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""},{\""TM\"" : \""2022-07-15T08:53:43,000+0200 I\"",\""TX\"" : \""E\"",\""RV\"" : \""1592\"",\""RI\"" : \""01-00:00.08.06.FF\"",\""RU\"" : \""s\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""}]}|{\""SA\"" : \""ECDSA-secp256k1-SHA256\"",\""SD\"" : \""3046022100AEF09929FF8DAC28334BD8F19D52A955D7A19F7667A9ECE8FAA2422CFCD3580E022100C0DA212592607735D8838D7CB08A2F75D14970E6FDD757A2ADCF818E153AA0F0\""}"",
                                                 ""MeteringStatus"": ""Start""
                                             },
                                             {
                                                 ""SignedMeteringValue"": ""OCMF|{\""FV\"" : \""1.0\"",\""GI\"" : \""DZG-GSH01.1K2L\"",\""GS\"" : \""1DZG0028210907\"",\""GV\"" : \""230\"",\""PG\"" : \""T1264\"",\""MV\"" : \""DZG\"",\""MM\"" : \""GSH01.1K2L\"",\""MS\"" : \""1DZG0028210907\"",\""MF\"" : \""230\"",\""IS\"" : true,\""IT\"" : \""CENTRAL_1\"",\""ID\"" : \""A4A44E64\"",\""CT\"" : \""EVSEID\"",\""CI\"" : \""20BZ0413B1\"",\""RD\"" : [{\""TM\"" : \""2022-07-15T08:27:11,000+0200 I\"",\""TX\"" : \""B\"",\""RV\"" : \""0.000\"",\""RI\"" : \""01-00:98.08.00.FF\"",\""RU\"" : \""kWh\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""},{\""TM\"" : \""2022-07-15T08:53:43,000+0200 I\"",\""TX\"" : \""E\"",\""RV\"" : \""20.614\"",\""RI\"" : \""01-00:98.08.00.FF\"",\""RU\"" : \""kWh\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""}],\""U\"" : [{\""TM\"" : \""2022-07-15T08:27:11,000+0200 I\"",\""TX\"" : \""B\"",\""RV\"" : \""17359.195\"",\""RI\"" : \""01-00:9C.08.00.FF\"",\""RU\"" : \""kWh\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""},{\""TM\"" : \""2022-07-15T08:53:43,000+0200 I\"",\""TX\"" : \""E\"",\""RV\"" : \""17379.809\"",\""RI\"" : \""01-00:9C.08.00.FF\"",\""RU\"" : \""kWh\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""},{\""TM\"" : \""2022-07-15T08:27:11,000+0200 I\"",\""TX\"" : \""B\"",\""RV\"" : \""0.0031\"",\""RI\"" : \""01-00:8C.07.00.FF\"",\""RU\"" : \""Ohm\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""},{\""TM\"" : \""2022-07-15T08:53:43,000+0200 I\"",\""TX\"" : \""E\"",\""RV\"" : \""1592\"",\""RI\"" : \""01-00:00.08.06.FF\"",\""RU\"" : \""s\"",\""RT\"" : \""DC\"",\""EF\"" : \""\"",\""ST\"" : \""G\""}]}|{\""SA\"" : \""ECDSA-secp256k1-SHA256\"",\""SD\"" : \""3046022100AEF09929FF8DAC28334BD8F19D52A955D7A19F7667A9ECE8FAA2422CFCD3580E022100C0DA212592607735D8838D7CB08A2F75D14970E6FDD757A2ADCF818E153AA0F0\""}"",
                                                 ""MeteringStatus"": ""End""
                                             }
                                         ],
                                         ""CalibrationLawVerificationInfo"": {
                                             ""CalibrationLawCertificateID"":                  ""PTB123"",
                                             ""PublicKey"":                                    ""123456"",
                                             ""MeteringSignatureUrl"":                         ""https://open.charging.cloud"",
                                             ""MeteringSignatureEncodingFormat"":              ""OCMF"",
                                             ""SignedMeteringValuesVerificationInstruction"":  ""Use Chargy Transparency Software!""
                                         },
                                         ""ConsumedEnergy"": 20.614,
                                         ""HubOperatorID"": ""DE*GEF"",
                                         ""HubProviderID"": ""DE-GDF""
                                     }"));

            ClassicAssert.IsNotNull(chargeDetailRecord);
            ClassicAssert.IsNotNull(chargeDetailRecord.CalibrationLawVerificationInfo);
            ClassicAssert.AreEqual ("OCMF", chargeDetailRecord.CalibrationLawVerificationInfo?.MeteringSignatureEncodingFormat);

        }

        #endregion

        #region ChargeDetailRecord_ParseJSONText_Test2()

        [Test]
        public void ChargeDetailRecord_ParseJSONText_Test2()
        {

            var chargeDetailRecord = ChargeDetailRecord.Parse(JObject.Parse(@"{
                                         ""SessionID"":             ""0c00b73c-2f99-453f-974b-461794220d8a"",
                                         ""CPOPartnerSessionID"":   ""800570-92EA34BC-05E3-44AF-9DDF-5F931C730B4A"",
                                         ""EMPPartnerSessionID"":   ""A36D60-D237A7BD-D134-34C1-CD12-5493125DEB1F"",
                                         ""PartnerProductID"":      ""AC3"",
                                         ""EvseID"":                ""DE*GEF*E904780*2"",
                                         ""Identification"": {
                                             ""RFIDMifareFamilyIdentification"": {
                                                 ""UID"": ""B2D6A76B""
                                             }
                                         },
                                         ""ChargingStart"":         ""2022-07-18T05:35:44Z"",
                                         ""ChargingEnd"":           ""2022-07-18T09:56:54Z"",
                                         ""SessionStart"":          ""2022-07-18T05:35:44Z"",
                                         ""SessionEnd"":            ""2022-07-18T09:56:54Z"",
                                         ""MeterValueStart"":         null,
                                         ""MeterValueEnd"":           null,
                                         ""MeterValueInBetween"":     null,
                                         ""SignedMeteringValues"": [{
                                             ""SignedMeteringValue"": ""<?xml version=\""1.0\"" encoding=\""UTF-8\"" ?><signedMeterValue>  <publicKey encoding=\""base64\"">B6RqUc2+T7K3jZv/TrKrmaqDzDeZakWTnh3fLijyciNx7WrX7g/odcQjkMhJG/RX</publicKey>  <meterValueSignature encoding=\""base64\"">RNDdoaFqbnooZB365Ly9qERF/wsCOMCJuY6rv4U3uW7qxIK3Nn6rAK0XuYMRSBi/AsQ=</meterValueSignature>  <signatureMethod>ECDSA192SHA256</signatureMethod>  <encodingMethod>EDL</encodingMethod>  <encodedMeterValue encoding=\""base64\"">CQFFTUgAAIoduVEN1WIIS6JHA3oLAAABAAERAP8e//9emAsAAAAAAsSy1qdrAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFAN1WIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=</encodedMeterValue></signedMeterValue>"",
                                             ""MeteringStatus"": ""Start""
                                         }, {
                                             ""SignedMeteringValue"": ""<?xml version=\""1.0\"" encoding=\""UTF-8\"" ?><signedMeterValue>  <publicKey encoding=\""base64\"">B6RqUc2+T7K3jZv/TrKrmaqDzDeZakWTnh3fLijyciNx7WrX7g/odcQjkMhJG/RX</publicKey>  <meterValueSignature encoding=\""base64\"">vr6qHjR2nwc5hMp1L0hoy/u3fSPX3A4dgBLxWH1msJwhkA5KOXF7ov6zcU1oO2uuAsQ=</meterValueSignature>  <signatureMethod>ECDSA192SHA256</signatureMethod>  <encodingMethod>EDL</encodingMethod>  <encodedMeterValue encoding=\""base64\"">CQFFTUgAAIoduYZK1WIIgt9HA3sLAAABAAERAP8e/8YcnQsAAAAAAsSy1qdrAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFAN1WIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=</encodedMeterValue></signedMeterValue>"",
                                             ""MeteringStatus"": ""End""
                                         }],
                                         ""CalibrationLawVerificationInfo"": {
                                             ""CalibrationLawCertificateID"":                     null,
                                             ""PublicKey"":                                     ""B6RqUc2+T7K3jZv/TrKrmaqDzDeZakWTnh3fLijyciNx7WrX7g/odcQjkMhJG/RX"",
                                             ""MeteringSignatureUrl"":                            null,
                                             ""MeteringSignatureEncodingFormat"":               ""EDL"",
                                             ""SignedMeteringValuesVerificationInstruction"":     null
                                         },
                                         ""ConsumedEnergy"":          31.072,
                                         ""HubOperatorID"":         ""DE*GEF"",
                                         ""HubProviderID"":         ""DE-GDF""
                                     }"));

            ClassicAssert.IsNotNull(chargeDetailRecord);
            ClassicAssert.IsNotNull(chargeDetailRecord.CalibrationLawVerificationInfo);
            ClassicAssert.AreEqual("EDL", chargeDetailRecord.CalibrationLawVerificationInfo?.MeteringSignatureEncodingFormat);

        }

        #endregion

    }

}
