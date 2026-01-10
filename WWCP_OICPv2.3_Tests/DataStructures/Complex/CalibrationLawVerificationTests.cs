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
            Assert.That(calibrationLawVerification1,                                               Is.Not.Null);
            Assert.That(calibrationLawVerification1.CalibrationLawCertificateId,                   Is.Null);
            Assert.That(calibrationLawVerification1.PublicKey,                                     Is.Null);
            Assert.That(calibrationLawVerification1.MeteringSignatureURL,                          Is.Null);
            Assert.That(calibrationLawVerification1.MeteringSignatureEncodingFormat,               Is.Null);
            Assert.That(calibrationLawVerification1.SignedMeteringValuesVerificationInstruction,   Is.Null);

            var calibrationLawVerification2 = new CalibrationLawVerification(CalibrationLawCertificateId: "2");
            Assert.That(calibrationLawVerification2,                                               Is.Not.Null);
            Assert.That(calibrationLawVerification2.CalibrationLawCertificateId,                   Is.EqualTo("2"));
            Assert.That(calibrationLawVerification2.PublicKey,                                     Is.Null);
            Assert.That(calibrationLawVerification2.MeteringSignatureURL,                          Is.Null);
            Assert.That(calibrationLawVerification2.MeteringSignatureEncodingFormat,               Is.Null);
            Assert.That(calibrationLawVerification2.SignedMeteringValuesVerificationInstruction,   Is.Null);

            var calibrationLawVerification3 = new CalibrationLawVerification(PublicKey: "3");
            Assert.That(calibrationLawVerification3,                                               Is.Not.Null);
            Assert.That(calibrationLawVerification3.CalibrationLawCertificateId,                   Is.Null);
            Assert.That(calibrationLawVerification3.PublicKey,                                     Is.EqualTo("3"));
            Assert.That(calibrationLawVerification3.MeteringSignatureURL,                          Is.Null);
            Assert.That(calibrationLawVerification3.MeteringSignatureEncodingFormat,               Is.Null);
            Assert.That(calibrationLawVerification3.SignedMeteringValuesVerificationInstruction,   Is.Null);

            var calibrationLawVerification4 = new CalibrationLawVerification(MeteringSignatureURL: URL.Parse("http://example.com"));
            Assert.That(calibrationLawVerification4,                                               Is.Not.Null);
            Assert.That(calibrationLawVerification4.CalibrationLawCertificateId,                   Is.Null);
            Assert.That(calibrationLawVerification4.PublicKey,                                     Is.Null);
            Assert.That(calibrationLawVerification4.MeteringSignatureURL,                          Is.EqualTo(URL.Parse("http://example.com")));
            Assert.That(calibrationLawVerification4.MeteringSignatureEncodingFormat,               Is.Null);
            Assert.That(calibrationLawVerification4.SignedMeteringValuesVerificationInstruction,   Is.Null);

            var calibrationLawVerification5 = new CalibrationLawVerification(MeteringSignatureEncodingFormat: "5");
            Assert.That(calibrationLawVerification5,                                               Is.Not.Null);
            Assert.That(calibrationLawVerification5.CalibrationLawCertificateId,                   Is.Null);
            Assert.That(calibrationLawVerification5.PublicKey,                                     Is.Null);
            Assert.That(calibrationLawVerification5.MeteringSignatureURL,                          Is.Null);
            Assert.That(calibrationLawVerification5.MeteringSignatureEncodingFormat,               Is.EqualTo("5"));
            Assert.That(calibrationLawVerification5.SignedMeteringValuesVerificationInstruction,   Is.Null);

            var calibrationLawVerification6 = new CalibrationLawVerification(SignedMeteringValuesVerificationInstruction: "6");
            Assert.That(calibrationLawVerification6,                                               Is.Not.Null);
            Assert.That(calibrationLawVerification6.CalibrationLawCertificateId,                   Is.Null);
            Assert.That(calibrationLawVerification6.PublicKey,                                     Is.Null);
            Assert.That(calibrationLawVerification6.MeteringSignatureURL,                          Is.Null);
            Assert.That(calibrationLawVerification6.MeteringSignatureEncodingFormat,               Is.Null);
            Assert.That(calibrationLawVerification6.SignedMeteringValuesVerificationInstruction,   Is.EqualTo("6"));

            var calibrationLawVerification7 = new CalibrationLawVerification(
                                                  CalibrationLawCertificateId:                  "22",
                                                  PublicKey:                                    "33",
                                                  MeteringSignatureURL:                          URL.Parse("http://example.com"),
                                                  MeteringSignatureEncodingFormat:              "55",
                                                  SignedMeteringValuesVerificationInstruction:  "66"
                                              );

            Assert.That(calibrationLawVerification7,                                               Is.Not.Null);
            Assert.That(calibrationLawVerification7.CalibrationLawCertificateId,                   Is.EqualTo("22"));
            Assert.That(calibrationLawVerification7.PublicKey,                                     Is.EqualTo("33"));
            Assert.That(calibrationLawVerification7.MeteringSignatureURL,                          Is.EqualTo(URL.Parse("http://example.com")));
            Assert.That(calibrationLawVerification7.MeteringSignatureEncodingFormat,               Is.EqualTo("55"));
            Assert.That(calibrationLawVerification7.SignedMeteringValuesVerificationInstruction,   Is.EqualTo("66"));

        }

        #endregion

        #region CalibrationLawVerification_ParseJSON_Test1()

        [Test]
        public void CalibrationLawVerification_ParseJSON_Test1()
        {

            var calibrationLawVerification1a = CalibrationLawVerification.Parse([]);
            Assert.That(calibrationLawVerification1a,                                               Is.Not.Null);
            Assert.That(calibrationLawVerification1a.CalibrationLawCertificateId,                   Is.Null);
            Assert.That(calibrationLawVerification1a.PublicKey,                                     Is.Null);
            Assert.That(calibrationLawVerification1a.MeteringSignatureURL,                          Is.Null);
            Assert.That(calibrationLawVerification1a.MeteringSignatureEncodingFormat,               Is.Null);
            Assert.That(calibrationLawVerification1a.SignedMeteringValuesVerificationInstruction,   Is.Null);

            Object? _null = null;

            var calibrationLawVerification1b = CalibrationLawVerification.Parse(
                                                   new JObject(
                                                       new JProperty("CalibrationLawCertificateID",                  _null),
                                                       new JProperty("PublicKey",                                    _null),
                                                       new JProperty("MeteringSignatureUrl",                         _null),
                                                       new JProperty("MeteringSignatureEncodingFormat",              _null),
                                                       new JProperty("SignedMeteringValuesVerificationInstruction",  _null)
                                                   )
                                               );

            Assert.That(calibrationLawVerification1b,                                               Is.Not.Null);
            Assert.That(calibrationLawVerification1b.CalibrationLawCertificateId,                   Is.Null);
            Assert.That(calibrationLawVerification1b.PublicKey,                                     Is.Null);
            Assert.That(calibrationLawVerification1b.MeteringSignatureURL,                          Is.Null);
            Assert.That(calibrationLawVerification1b.MeteringSignatureEncodingFormat,               Is.Null);
            Assert.That(calibrationLawVerification1b.SignedMeteringValuesVerificationInstruction,   Is.Null);

            var calibrationLawVerification2 = CalibrationLawVerification.Parse(
                                                  new JObject(
                                                      new JProperty("CalibrationLawCertificateID",  "2")
                                                  )
                                              );
            Assert.That(calibrationLawVerification2,                                                Is.Not.Null);
            Assert.That(calibrationLawVerification2.CalibrationLawCertificateId,                    Is.EqualTo("2"));
            Assert.That(calibrationLawVerification2.PublicKey,                                      Is.Null);
            Assert.That(calibrationLawVerification2.MeteringSignatureURL,                           Is.Null);
            Assert.That(calibrationLawVerification2.MeteringSignatureEncodingFormat,                Is.Null);
            Assert.That(calibrationLawVerification2.SignedMeteringValuesVerificationInstruction,    Is.Null);

            var calibrationLawVerification3 = CalibrationLawVerification.Parse(
                                                  new JObject(
                                                      new JProperty("PublicKey",  "3")
                                                  )
                                              );
            Assert.That(calibrationLawVerification3,                                                Is.Not.Null);
            Assert.That(calibrationLawVerification3.CalibrationLawCertificateId,                    Is.Null);
            Assert.That(calibrationLawVerification3.PublicKey,                                      Is.EqualTo("3"));
            Assert.That(calibrationLawVerification3.MeteringSignatureURL,                           Is.Null);
            Assert.That(calibrationLawVerification3.MeteringSignatureEncodingFormat,                Is.Null);
            Assert.That(calibrationLawVerification3.SignedMeteringValuesVerificationInstruction,    Is.Null);

            var calibrationLawVerification4 = CalibrationLawVerification.Parse(
                                                  new JObject(
                                                      new JProperty("MeteringSignatureUrl",  "http://example.com")
                                                  )
                                              );
            Assert.That(calibrationLawVerification4,                                                Is.Not.Null);
            Assert.That(calibrationLawVerification4.CalibrationLawCertificateId,                    Is.Null);
            Assert.That(calibrationLawVerification4.PublicKey,                                      Is.Null);
            Assert.That(calibrationLawVerification4.MeteringSignatureURL,                           Is.EqualTo(URL.Parse("http://example.com")));
            Assert.That(calibrationLawVerification4.MeteringSignatureEncodingFormat,                Is.Null);
            Assert.That(calibrationLawVerification4.SignedMeteringValuesVerificationInstruction,    Is.Null);

            var calibrationLawVerification5 = CalibrationLawVerification.Parse(
                                                  new JObject(
                                                      new JProperty("MeteringSignatureEncodingFormat",  "5")
                                                  )
                                              );
            Assert.That(calibrationLawVerification5,                                                Is.Not.Null);
            Assert.That(calibrationLawVerification5.CalibrationLawCertificateId,                    Is.Null);
            Assert.That(calibrationLawVerification5.PublicKey,                                      Is.Null);
            Assert.That(calibrationLawVerification5.MeteringSignatureURL,                           Is.Null);
            Assert.That(calibrationLawVerification5.MeteringSignatureEncodingFormat,                Is.EqualTo("5"));
            Assert.That(calibrationLawVerification5.SignedMeteringValuesVerificationInstruction,    Is.Null);

            var calibrationLawVerification6 = CalibrationLawVerification.Parse(
                                                  new JObject(
                                                      new JProperty("SignedMeteringValuesVerificationInstruction",  "6")
                                                  )
                                              );
            Assert.That(calibrationLawVerification6,                                                Is.Not.Null);
            Assert.That(calibrationLawVerification6.CalibrationLawCertificateId,                    Is.Null);
            Assert.That(calibrationLawVerification6.PublicKey,                                      Is.Null);
            Assert.That(calibrationLawVerification6.MeteringSignatureURL,                           Is.Null);
            Assert.That(calibrationLawVerification6.MeteringSignatureEncodingFormat,                Is.Null);
            Assert.That(calibrationLawVerification6.SignedMeteringValuesVerificationInstruction,    Is.EqualTo("6"));

            var calibrationLawVerification7 = CalibrationLawVerification.Parse(
                                                  new JObject(
                                                      new JProperty("CalibrationLawCertificateID",                  "22"),
                                                      new JProperty("PublicKey",                                    "33"),
                                                      new JProperty("MeteringSignatureUrl",                          URL.Parse("http://example.com").ToString()),
                                                      new JProperty("MeteringSignatureEncodingFormat",              "55"),
                                                      new JProperty("SignedMeteringValuesVerificationInstruction",  "66")
                                                  )
                                              );

            Assert.That(calibrationLawVerification7,                                                Is.Not.Null);
            Assert.That(calibrationLawVerification7.CalibrationLawCertificateId,                    Is.EqualTo("22"));
            Assert.That(calibrationLawVerification7.PublicKey,                                      Is.EqualTo("33"));
            Assert.That(calibrationLawVerification7.MeteringSignatureURL,                           Is.EqualTo(URL.Parse("http://example.com")));
            Assert.That(calibrationLawVerification7.MeteringSignatureEncodingFormat,                Is.EqualTo("55"));
            Assert.That(calibrationLawVerification7.SignedMeteringValuesVerificationInstruction,    Is.EqualTo("66"));
        }

        #endregion

        #region CalibrationLawVerification_ParseJSONText_Test1()

        [Test]
        public void CalibrationLawVerification_ParseJSONText_Test1()
        {

            var calibrationLawVerification1 = CalibrationLawVerification.Parse(
                                                  JObject.Parse(@"{ ""CalibrationLawCertificateID"": null, ""PublicKey"": null, ""MeteringSignatureUrl"": null, ""MeteringSignatureEncodingFormat"": ""OCMF"", ""SignedMeteringValuesVerificationInstruction"": null }")
                                              );

            Assert.That(calibrationLawVerification1,                                               Is.Not.Null);
            Assert.That(calibrationLawVerification1.CalibrationLawCertificateId,                   Is.Null);
            Assert.That(calibrationLawVerification1.PublicKey,                                     Is.Null);
            Assert.That(calibrationLawVerification1.MeteringSignatureURL,                          Is.Null);
            Assert.That(calibrationLawVerification1.MeteringSignatureEncodingFormat,               Is.EqualTo("OCMF"));
            Assert.That(calibrationLawVerification1.SignedMeteringValuesVerificationInstruction,   Is.Null);

        }

        #endregion

    }

}
