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

using System;

using NUnit.Framework;
using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests
{

    /// <summary>
    /// Calibration law verification tests.
    /// </summary>
    [TestFixture]
    public class CalibrationLawVerificationTests
    {

        #region CalibrationLawVerification_Test1()

        [Test]
        public void CalibrationLawVerification_Test1()
        {

            var calibrationLawVerification1 = new CalibrationLawVerification();

            Assert.IsNotNull     (calibrationLawVerification1);
            Assert.IsNull        (calibrationLawVerification1.CalibrationLawCertificateId);
            Assert.IsNull        (calibrationLawVerification1.PublicKey);
            Assert.IsNull        (calibrationLawVerification1.MeteringSignatureURL);
            Assert.IsNull        (calibrationLawVerification1.MeteringSignatureEncodingFormat);
            Assert.IsNull        (calibrationLawVerification1.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification2 = new CalibrationLawVerification(CalibrationLawCertificateId: "2");

            Assert.IsNotNull     (calibrationLawVerification2);
            Assert.AreEqual ("2", calibrationLawVerification2.CalibrationLawCertificateId);
            Assert.IsNull        (calibrationLawVerification2.PublicKey);
            Assert.IsNull        (calibrationLawVerification2.MeteringSignatureURL);
            Assert.IsNull        (calibrationLawVerification2.MeteringSignatureEncodingFormat);
            Assert.IsNull        (calibrationLawVerification2.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification3 = new CalibrationLawVerification(PublicKey: "3");

            Assert.IsNotNull    (calibrationLawVerification3);
            Assert.IsNull       (calibrationLawVerification3.CalibrationLawCertificateId);
            Assert.AreEqual("3", calibrationLawVerification3.PublicKey);
            Assert.IsNull       (calibrationLawVerification3.MeteringSignatureURL);
            Assert.IsNull       (calibrationLawVerification3.MeteringSignatureEncodingFormat);
            Assert.IsNull       (calibrationLawVerification3.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification4 = new CalibrationLawVerification(MeteringSignatureURL: URL.Parse("http://example.com"));

            Assert.IsNotNull                                 (calibrationLawVerification4);
            Assert.IsNull                                    (calibrationLawVerification4.CalibrationLawCertificateId);
            Assert.IsNull                                    (calibrationLawVerification4.PublicKey);
            Assert.AreEqual (URL.Parse("http://example.com"), calibrationLawVerification4.MeteringSignatureURL);
            Assert.IsNull                                    (calibrationLawVerification4.MeteringSignatureEncodingFormat);
            Assert.IsNull                                    (calibrationLawVerification4.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification5 = new CalibrationLawVerification(MeteringSignatureEncodingFormat: "5");

            Assert.IsNotNull    (calibrationLawVerification5);
            Assert.IsNull       (calibrationLawVerification5.CalibrationLawCertificateId);
            Assert.IsNull       (calibrationLawVerification5.PublicKey);
            Assert.IsNull       (calibrationLawVerification5.MeteringSignatureURL);
            Assert.AreEqual("5", calibrationLawVerification5.MeteringSignatureEncodingFormat);
            Assert.IsNull       (calibrationLawVerification5.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification6 = new CalibrationLawVerification(SignedMeteringValuesVerificationInstruction: "6");

            Assert.IsNotNull    (calibrationLawVerification6);
            Assert.IsNull       (calibrationLawVerification6.CalibrationLawCertificateId);
            Assert.IsNull       (calibrationLawVerification6.PublicKey);
            Assert.IsNull       (calibrationLawVerification6.MeteringSignatureURL);
            Assert.IsNull       (calibrationLawVerification6.MeteringSignatureEncodingFormat);
            Assert.AreEqual("6", calibrationLawVerification6.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification7 = new CalibrationLawVerification(
                                                  CalibrationLawCertificateId:                  "22",
                                                  PublicKey:                                    "33",
                                                  MeteringSignatureURL:                          URL.Parse("http://example.com"),
                                                  MeteringSignatureEncodingFormat:              "55",
                                                  SignedMeteringValuesVerificationInstruction:  "66");

            Assert.IsNotNull(calibrationLawVerification7);
            Assert.AreEqual("22",                            calibrationLawVerification7.CalibrationLawCertificateId);
            Assert.AreEqual("33",                            calibrationLawVerification7.PublicKey);
            Assert.AreEqual(URL.Parse("http://example.com"), calibrationLawVerification7.MeteringSignatureURL);
            Assert.AreEqual("55",                            calibrationLawVerification7.MeteringSignatureEncodingFormat);
            Assert.AreEqual("66",                            calibrationLawVerification7.SignedMeteringValuesVerificationInstruction);

        }

        #endregion

        #region CalibrationLawVerification_ParseJSON_Test1()

        [Test]
        public void CalibrationLawVerification_ParseJSON_Test1()
        {

            var calibrationLawVerification1a = CalibrationLawVerification.Parse(new JObject());

            Assert.IsNotNull     (calibrationLawVerification1a);
            Assert.IsNull        (calibrationLawVerification1a.CalibrationLawCertificateId);
            Assert.IsNull        (calibrationLawVerification1a.PublicKey);
            Assert.IsNull        (calibrationLawVerification1a.MeteringSignatureURL);
            Assert.IsNull        (calibrationLawVerification1a.MeteringSignatureEncodingFormat);
            Assert.IsNull        (calibrationLawVerification1a.SignedMeteringValuesVerificationInstruction);

            var calibrationLawVerification1b = CalibrationLawVerification.Parse(new JObject(
                                                                                    new JProperty("CalibrationLawCertificateID",                 null),
                                                                                    new JProperty("PublicKey",                                   null),
                                                                                    new JProperty("MeteringSignatureUrl",                        null),
                                                                                    new JProperty("MeteringSignatureEncodingFormat",             null),
                                                                                    new JProperty("SignedMeteringValuesVerificationInstruction", null)
                                                                               ));

            Assert.IsNotNull(calibrationLawVerification1b);
            Assert.IsNull(calibrationLawVerification1b.CalibrationLawCertificateId);
            Assert.IsNull(calibrationLawVerification1b.PublicKey);
            Assert.IsNull(calibrationLawVerification1b.MeteringSignatureURL);
            Assert.IsNull(calibrationLawVerification1b.MeteringSignatureEncodingFormat);
            Assert.IsNull(calibrationLawVerification1b.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification2 = CalibrationLawVerification.Parse(new JObject(new JProperty("CalibrationLawCertificateID", "2")));

            Assert.IsNotNull(calibrationLawVerification2);
            Assert.AreEqual("2", calibrationLawVerification2.CalibrationLawCertificateId);
            Assert.IsNull(calibrationLawVerification2.PublicKey);
            Assert.IsNull(calibrationLawVerification2.MeteringSignatureURL);
            Assert.IsNull(calibrationLawVerification2.MeteringSignatureEncodingFormat);
            Assert.IsNull(calibrationLawVerification2.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification3 = CalibrationLawVerification.Parse(new JObject(new JProperty("PublicKey", "3")));

            Assert.IsNotNull(calibrationLawVerification3);
            Assert.IsNull(calibrationLawVerification3.CalibrationLawCertificateId);
            Assert.AreEqual("3", calibrationLawVerification3.PublicKey);
            Assert.IsNull(calibrationLawVerification3.MeteringSignatureURL);
            Assert.IsNull(calibrationLawVerification3.MeteringSignatureEncodingFormat);
            Assert.IsNull(calibrationLawVerification3.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification4 = CalibrationLawVerification.Parse(new JObject(new JProperty("MeteringSignatureUrl", "http://example.com")));

            Assert.IsNotNull(calibrationLawVerification4);
            Assert.IsNull(calibrationLawVerification4.CalibrationLawCertificateId);
            Assert.IsNull(calibrationLawVerification4.PublicKey);
            Assert.AreEqual(URL.Parse("http://example.com"), calibrationLawVerification4.MeteringSignatureURL);
            Assert.IsNull(calibrationLawVerification4.MeteringSignatureEncodingFormat);
            Assert.IsNull(calibrationLawVerification4.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification5 = CalibrationLawVerification.Parse(new JObject(new JProperty("MeteringSignatureEncodingFormat", "5")));

            Assert.IsNotNull(calibrationLawVerification5);
            Assert.IsNull(calibrationLawVerification5.CalibrationLawCertificateId);
            Assert.IsNull(calibrationLawVerification5.PublicKey);
            Assert.IsNull(calibrationLawVerification5.MeteringSignatureURL);
            Assert.AreEqual("5", calibrationLawVerification5.MeteringSignatureEncodingFormat);
            Assert.IsNull(calibrationLawVerification5.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification6 = CalibrationLawVerification.Parse(new JObject(new JProperty("SignedMeteringValuesVerificationInstruction", "6")));

            Assert.IsNotNull(calibrationLawVerification6);
            Assert.IsNull(calibrationLawVerification6.CalibrationLawCertificateId);
            Assert.IsNull(calibrationLawVerification6.PublicKey);
            Assert.IsNull(calibrationLawVerification6.MeteringSignatureURL);
            Assert.IsNull(calibrationLawVerification6.MeteringSignatureEncodingFormat);
            Assert.AreEqual("6", calibrationLawVerification6.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification7 = CalibrationLawVerification.Parse(new JObject(
                                                                                    new JProperty("CalibrationLawCertificateID",                 "22"),
                                                                                    new JProperty("PublicKey",                                   "33"),
                                                                                    new JProperty("MeteringSignatureUrl",                        URL.Parse("http://example.com").ToString()),
                                                                                    new JProperty("MeteringSignatureEncodingFormat",             "55"),
                                                                                    new JProperty("SignedMeteringValuesVerificationInstruction", "66")
                                                                               ));

            Assert.IsNotNull(calibrationLawVerification7);
            Assert.AreEqual("22",                            calibrationLawVerification7.CalibrationLawCertificateId);
            Assert.AreEqual("33",                            calibrationLawVerification7.PublicKey);
            Assert.AreEqual(URL.Parse("http://example.com"), calibrationLawVerification7.MeteringSignatureURL);
            Assert.AreEqual("55",                            calibrationLawVerification7.MeteringSignatureEncodingFormat);
            Assert.AreEqual("66",                            calibrationLawVerification7.SignedMeteringValuesVerificationInstruction);

        }

        #endregion

        #region CalibrationLawVerification_ParseJSONText_Test1()

        [Test]
        public void CalibrationLawVerification_ParseJSONText_Test1()
        {

            var calibrationLawVerification1 = CalibrationLawVerification.Parse(@"{ ""CalibrationLawCertificateID"": null, ""PublicKey"": null, ""MeteringSignatureUrl"": null, ""MeteringSignatureEncodingFormat"": ""OCMF"", ""SignedMeteringValuesVerificationInstruction"": null }");

            Assert.IsNotNull(calibrationLawVerification1);
            Assert.IsNull(calibrationLawVerification1.CalibrationLawCertificateId);
            Assert.IsNull(calibrationLawVerification1.PublicKey);
            Assert.IsNull(calibrationLawVerification1.MeteringSignatureURL);
            Assert.AreEqual("OCMF", calibrationLawVerification1.MeteringSignatureEncodingFormat);
            Assert.IsNull(calibrationLawVerification1.SignedMeteringValuesVerificationInstruction);

        }

        #endregion

    }

}
