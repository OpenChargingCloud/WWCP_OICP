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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.datastructures
{

    /// <summary>
    /// Environmental impact tests.
    /// </summary>
    [TestFixture]
    public class EnvironmentalImpactTests
    {

        #region EnvironmentalImpact_Test1()

        [Test]
        public void EnvironmentalImpact_Test1()
        {

            var environmentalImpact1 = new EnvironmentalImpact();
            var environmentalImpact2 = new EnvironmentalImpact();

            var environmentalImpact3 = new EnvironmentalImpact(CO2Emission: 20000);
            var environmentalImpact4 = new EnvironmentalImpact(CO2Emission: 20000);
            var environmentalImpact5 = new EnvironmentalImpact(CO2Emission: 25000);

            var environmentalImpact6 = new EnvironmentalImpact(CO2Emission: 30000, NuclearWaste: 50000);
            var environmentalImpact7 = new EnvironmentalImpact(CO2Emission: 30000, NuclearWaste: 50000);

            Assert.That(environmentalImpact1,                           Is.Not.Null);
            Assert.That(environmentalImpact3,                           Is.Not.Null);
            Assert.That(environmentalImpact5,                           Is.Not.Null);
            Assert.That(environmentalImpact6,                           Is.Not.Null);

            Assert.That(environmentalImpact1.CO2Emission,               Is.Null);
            Assert.That(environmentalImpact1.NuclearWaste,              Is.Null);
            Assert.That(environmentalImpact3.NuclearWaste,              Is.Null);
            Assert.That(environmentalImpact5.NuclearWaste,              Is.Null);

            Assert.That(environmentalImpact3.CO2Emission,               Is.EqualTo(20000));
            Assert.That(environmentalImpact5.CO2Emission,               Is.EqualTo(25000));
            Assert.That(environmentalImpact6.CO2Emission,               Is.EqualTo(30000));
            Assert.That(environmentalImpact6.NuclearWaste,              Is.EqualTo(50000));

            Assert.That(environmentalImpact1,                           Is.EqualTo(environmentalImpact2));
            Assert.That(environmentalImpact3,                           Is.EqualTo(environmentalImpact4));
            Assert.That(environmentalImpact6,                           Is.EqualTo(environmentalImpact7));

            Assert.That(environmentalImpact1,                           Is.Not.EqualTo(environmentalImpact3));
            Assert.That(environmentalImpact1,                           Is.Not.EqualTo(environmentalImpact5));
            Assert.That(environmentalImpact1,                           Is.Not.EqualTo(environmentalImpact6));
            Assert.That(environmentalImpact3,                           Is.Not.EqualTo(environmentalImpact5));
            Assert.That(environmentalImpact3,                           Is.Not.EqualTo(environmentalImpact6));

            Assert.That(environmentalImpact3  < environmentalImpact4,   Is.False);
            Assert.That(environmentalImpact3  > environmentalImpact4,   Is.False);
            Assert.That(environmentalImpact3 == environmentalImpact4,   Is.True);
            Assert.That(environmentalImpact3 <= environmentalImpact4,   Is.True);
            Assert.That(environmentalImpact3 >= environmentalImpact4,   Is.True);
            Assert.That(environmentalImpact3  < environmentalImpact5,   Is.True);
            Assert.That(environmentalImpact3  > environmentalImpact5,   Is.False);
            Assert.That(environmentalImpact3 == environmentalImpact5,   Is.False);
            Assert.That(environmentalImpact3 <= environmentalImpact5,   Is.True);
            Assert.That(environmentalImpact3 >= environmentalImpact5,   Is.False);

        }

        #endregion

        #region EnvironmentalImpact_SerializeJSON_Test1()

        [Test]
        public void EnvironmentalImpact_SerializeJSON_Test1()
        {

            var environmentalImpact1 = new EnvironmentalImpact();
            var environmentalImpact2 = new EnvironmentalImpact(CO2Emission: 20000);
            var environmentalImpact3 = new EnvironmentalImpact(CO2Emission: 30000, NuclearWaste: 50000);

            Assert.That(environmentalImpact1,                       Is.Not.Null);
            Assert.That(environmentalImpact2,                       Is.Not.Null);
            Assert.That(environmentalImpact3,                       Is.Not.Null);

            var json1 = environmentalImpact1.ToJSON();
            var json2 = environmentalImpact2.ToJSON();
            var json3 = environmentalImpact3.ToJSON();

            Assert.That(json1,                                      Is.Null);
            Assert.That(json2,                                      Is.Not.Null);
            Assert.That(json3,                                      Is.Not.Null);

            Assert.That(json2?["CO2Emission"]?. Value<Decimal>(),   Is.EqualTo(20000));
            Assert.That(json3?["CO2Emission"]?. Value<Decimal>(),   Is.EqualTo(30000));
            Assert.That(json3?["NuclearWaste"]?.Value<Decimal>(),   Is.EqualTo(50000));

        }

        #endregion

        #region EnvironmentalImpact_ParseJSON_Test1()

        [Test]
        public void EnvironmentalImpact_ParseJSON_Test1()
        {

            var environmentalImpact1 = EnvironmentalImpact.Parse([]);

            var environmentalImpact2 = EnvironmentalImpact.Parse(new JObject(new JProperty("CO2Emission",  20000)));

            var environmentalImpact3 = EnvironmentalImpact.Parse(new JObject(new JProperty("CO2Emission",  30000),
                                                                             new JProperty("NuclearWaste", 50000)));

            Assert.That(environmentalImpact1,                Is.Not.Null);
            Assert.That(environmentalImpact2,                Is.Not.Null);
            Assert.That(environmentalImpact3,                Is.Not.Null);

            Assert.That(environmentalImpact1.CO2Emission,    Is.Null);
            Assert.That(environmentalImpact1.NuclearWaste,   Is.Null);
            Assert.That(environmentalImpact2.NuclearWaste,   Is.Null);

            Assert.That(environmentalImpact2.CO2Emission,    Is.EqualTo(20000));
            Assert.That(environmentalImpact3.CO2Emission,    Is.EqualTo(30000));
            Assert.That(environmentalImpact3.NuclearWaste,   Is.EqualTo(50000));

        }

        #endregion

    }

}
