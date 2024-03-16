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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.datastructures
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

            ClassicAssert.IsNotNull     (calibrationLawVerification1);
            ClassicAssert.IsNull        (calibrationLawVerification1.CalibrationLawCertificateId);
            ClassicAssert.IsNull        (calibrationLawVerification1.PublicKey);
            ClassicAssert.IsNull        (calibrationLawVerification1.MeteringSignatureURL);
            ClassicAssert.IsNull        (calibrationLawVerification1.MeteringSignatureEncodingFormat);
            ClassicAssert.IsNull        (calibrationLawVerification1.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification2 = new CalibrationLawVerification(CalibrationLawCertificateId: "2");

            ClassicAssert.IsNotNull     (calibrationLawVerification2);
            ClassicAssert.AreEqual ("2", calibrationLawVerification2.CalibrationLawCertificateId);
            ClassicAssert.IsNull        (calibrationLawVerification2.PublicKey);
            ClassicAssert.IsNull        (calibrationLawVerification2.MeteringSignatureURL);
            ClassicAssert.IsNull        (calibrationLawVerification2.MeteringSignatureEncodingFormat);
            ClassicAssert.IsNull        (calibrationLawVerification2.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification3 = new CalibrationLawVerification(PublicKey: "3");

            ClassicAssert.IsNotNull    (calibrationLawVerification3);
            ClassicAssert.IsNull       (calibrationLawVerification3.CalibrationLawCertificateId);
            ClassicAssert.AreEqual("3", calibrationLawVerification3.PublicKey);
            ClassicAssert.IsNull       (calibrationLawVerification3.MeteringSignatureURL);
            ClassicAssert.IsNull       (calibrationLawVerification3.MeteringSignatureEncodingFormat);
            ClassicAssert.IsNull       (calibrationLawVerification3.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification4 = new CalibrationLawVerification(MeteringSignatureURL: URL.Parse("http://example.com"));

            ClassicAssert.IsNotNull                                 (calibrationLawVerification4);
            ClassicAssert.IsNull                                    (calibrationLawVerification4.CalibrationLawCertificateId);
            ClassicAssert.IsNull                                    (calibrationLawVerification4.PublicKey);
            ClassicAssert.AreEqual (URL.Parse("http://example.com"), calibrationLawVerification4.MeteringSignatureURL);
            ClassicAssert.IsNull                                    (calibrationLawVerification4.MeteringSignatureEncodingFormat);
            ClassicAssert.IsNull                                    (calibrationLawVerification4.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification5 = new CalibrationLawVerification(MeteringSignatureEncodingFormat: "5");

            ClassicAssert.IsNotNull    (calibrationLawVerification5);
            ClassicAssert.IsNull       (calibrationLawVerification5.CalibrationLawCertificateId);
            ClassicAssert.IsNull       (calibrationLawVerification5.PublicKey);
            ClassicAssert.IsNull       (calibrationLawVerification5.MeteringSignatureURL);
            ClassicAssert.AreEqual("5", calibrationLawVerification5.MeteringSignatureEncodingFormat);
            ClassicAssert.IsNull       (calibrationLawVerification5.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification6 = new CalibrationLawVerification(SignedMeteringValuesVerificationInstruction: "6");

            ClassicAssert.IsNotNull    (calibrationLawVerification6);
            ClassicAssert.IsNull       (calibrationLawVerification6.CalibrationLawCertificateId);
            ClassicAssert.IsNull       (calibrationLawVerification6.PublicKey);
            ClassicAssert.IsNull       (calibrationLawVerification6.MeteringSignatureURL);
            ClassicAssert.IsNull       (calibrationLawVerification6.MeteringSignatureEncodingFormat);
            ClassicAssert.AreEqual("6", calibrationLawVerification6.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification7 = new CalibrationLawVerification(
                                                  CalibrationLawCertificateId:                  "22",
                                                  PublicKey:                                    "33",
                                                  MeteringSignatureURL:                          URL.Parse("http://example.com"),
                                                  MeteringSignatureEncodingFormat:              "55",
                                                  SignedMeteringValuesVerificationInstruction:  "66");

            ClassicAssert.IsNotNull(calibrationLawVerification7);
            ClassicAssert.AreEqual("22",                            calibrationLawVerification7.CalibrationLawCertificateId);
            ClassicAssert.AreEqual("33",                            calibrationLawVerification7.PublicKey);
            ClassicAssert.AreEqual(URL.Parse("http://example.com"), calibrationLawVerification7.MeteringSignatureURL);
            ClassicAssert.AreEqual("55",                            calibrationLawVerification7.MeteringSignatureEncodingFormat);
            ClassicAssert.AreEqual("66",                            calibrationLawVerification7.SignedMeteringValuesVerificationInstruction);

        }

        #endregion

        #region CalibrationLawVerification_ParseJSON_Test1()

        [Test]
        public void CalibrationLawVerification_ParseJSON_Test1()
        {

            var calibrationLawVerification1a = CalibrationLawVerification.Parse([]);

            ClassicAssert.IsNotNull     (calibrationLawVerification1a);
            ClassicAssert.IsNull        (calibrationLawVerification1a.CalibrationLawCertificateId);
            ClassicAssert.IsNull        (calibrationLawVerification1a.PublicKey);
            ClassicAssert.IsNull        (calibrationLawVerification1a.MeteringSignatureURL);
            ClassicAssert.IsNull        (calibrationLawVerification1a.MeteringSignatureEncodingFormat);
            ClassicAssert.IsNull        (calibrationLawVerification1a.SignedMeteringValuesVerificationInstruction);

            var calibrationLawVerification1b = CalibrationLawVerification.Parse(new JObject(
                                                                                    new JProperty("CalibrationLawCertificateID",                 null),
                                                                                    new JProperty("PublicKey",                                   null),
                                                                                    new JProperty("MeteringSignatureUrl",                        null),
                                                                                    new JProperty("MeteringSignatureEncodingFormat",             null),
                                                                                    new JProperty("SignedMeteringValuesVerificationInstruction", null)
                                                                               ));

            ClassicAssert.IsNotNull(calibrationLawVerification1b);
            ClassicAssert.IsNull(calibrationLawVerification1b.CalibrationLawCertificateId);
            ClassicAssert.IsNull(calibrationLawVerification1b.PublicKey);
            ClassicAssert.IsNull(calibrationLawVerification1b.MeteringSignatureURL);
            ClassicAssert.IsNull(calibrationLawVerification1b.MeteringSignatureEncodingFormat);
            ClassicAssert.IsNull(calibrationLawVerification1b.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification2 = CalibrationLawVerification.Parse(new JObject(new JProperty("CalibrationLawCertificateID", "2")));

            ClassicAssert.IsNotNull(calibrationLawVerification2);
            ClassicAssert.AreEqual("2", calibrationLawVerification2.CalibrationLawCertificateId);
            ClassicAssert.IsNull(calibrationLawVerification2.PublicKey);
            ClassicAssert.IsNull(calibrationLawVerification2.MeteringSignatureURL);
            ClassicAssert.IsNull(calibrationLawVerification2.MeteringSignatureEncodingFormat);
            ClassicAssert.IsNull(calibrationLawVerification2.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification3 = CalibrationLawVerification.Parse(new JObject(new JProperty("PublicKey", "3")));

            ClassicAssert.IsNotNull(calibrationLawVerification3);
            ClassicAssert.IsNull(calibrationLawVerification3.CalibrationLawCertificateId);
            ClassicAssert.AreEqual("3", calibrationLawVerification3.PublicKey);
            ClassicAssert.IsNull(calibrationLawVerification3.MeteringSignatureURL);
            ClassicAssert.IsNull(calibrationLawVerification3.MeteringSignatureEncodingFormat);
            ClassicAssert.IsNull(calibrationLawVerification3.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification4 = CalibrationLawVerification.Parse(new JObject(new JProperty("MeteringSignatureUrl", "http://example.com")));

            ClassicAssert.IsNotNull(calibrationLawVerification4);
            ClassicAssert.IsNull(calibrationLawVerification4.CalibrationLawCertificateId);
            ClassicAssert.IsNull(calibrationLawVerification4.PublicKey);
            ClassicAssert.AreEqual(URL.Parse("http://example.com"), calibrationLawVerification4.MeteringSignatureURL);
            ClassicAssert.IsNull(calibrationLawVerification4.MeteringSignatureEncodingFormat);
            ClassicAssert.IsNull(calibrationLawVerification4.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification5 = CalibrationLawVerification.Parse(new JObject(new JProperty("MeteringSignatureEncodingFormat", "5")));

            ClassicAssert.IsNotNull(calibrationLawVerification5);
            ClassicAssert.IsNull(calibrationLawVerification5.CalibrationLawCertificateId);
            ClassicAssert.IsNull(calibrationLawVerification5.PublicKey);
            ClassicAssert.IsNull(calibrationLawVerification5.MeteringSignatureURL);
            ClassicAssert.AreEqual("5", calibrationLawVerification5.MeteringSignatureEncodingFormat);
            ClassicAssert.IsNull(calibrationLawVerification5.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification6 = CalibrationLawVerification.Parse(new JObject(new JProperty("SignedMeteringValuesVerificationInstruction", "6")));

            ClassicAssert.IsNotNull(calibrationLawVerification6);
            ClassicAssert.IsNull(calibrationLawVerification6.CalibrationLawCertificateId);
            ClassicAssert.IsNull(calibrationLawVerification6.PublicKey);
            ClassicAssert.IsNull(calibrationLawVerification6.MeteringSignatureURL);
            ClassicAssert.IsNull(calibrationLawVerification6.MeteringSignatureEncodingFormat);
            ClassicAssert.AreEqual("6", calibrationLawVerification6.SignedMeteringValuesVerificationInstruction);


            var calibrationLawVerification7 = CalibrationLawVerification.Parse(new JObject(
                                                                                    new JProperty("CalibrationLawCertificateID",                 "22"),
                                                                                    new JProperty("PublicKey",                                   "33"),
                                                                                    new JProperty("MeteringSignatureUrl",                        URL.Parse("http://example.com").ToString()),
                                                                                    new JProperty("MeteringSignatureEncodingFormat",             "55"),
                                                                                    new JProperty("SignedMeteringValuesVerificationInstruction", "66")
                                                                               ));

            ClassicAssert.IsNotNull(calibrationLawVerification7);
            ClassicAssert.AreEqual("22",                            calibrationLawVerification7.CalibrationLawCertificateId);
            ClassicAssert.AreEqual("33",                            calibrationLawVerification7.PublicKey);
            ClassicAssert.AreEqual(URL.Parse("http://example.com"), calibrationLawVerification7.MeteringSignatureURL);
            ClassicAssert.AreEqual("55",                            calibrationLawVerification7.MeteringSignatureEncodingFormat);
            ClassicAssert.AreEqual("66",                            calibrationLawVerification7.SignedMeteringValuesVerificationInstruction);

        }

        #endregion

        #region CalibrationLawVerification_ParseJSONText_Test1()

        [Test]
        public void CalibrationLawVerification_ParseJSONText_Test1()
        {

            var calibrationLawVerification1 = CalibrationLawVerification.Parse(JObject.Parse(@"{ ""CalibrationLawCertificateID"": null, ""PublicKey"": null, ""MeteringSignatureUrl"": null, ""MeteringSignatureEncodingFormat"": ""OCMF"", ""SignedMeteringValuesVerificationInstruction"": null }"));

            ClassicAssert.IsNotNull(calibrationLawVerification1);
            ClassicAssert.IsNull(calibrationLawVerification1.CalibrationLawCertificateId);
            ClassicAssert.IsNull(calibrationLawVerification1.PublicKey);
            ClassicAssert.IsNull(calibrationLawVerification1.MeteringSignatureURL);
            ClassicAssert.AreEqual("OCMF", calibrationLawVerification1.MeteringSignatureEncodingFormat);
            ClassicAssert.IsNull(calibrationLawVerification1.SignedMeteringValuesVerificationInstruction);

        }

        #endregion

    }

}
