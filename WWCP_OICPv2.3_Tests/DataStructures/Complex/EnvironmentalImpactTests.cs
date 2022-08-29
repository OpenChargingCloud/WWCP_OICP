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

            Assert.IsNotNull  (environmentalImpact1);
            Assert.IsNotNull  (environmentalImpact3);
            Assert.IsNotNull  (environmentalImpact5);
            Assert.IsNotNull  (environmentalImpact6);

            Assert.IsNull     (environmentalImpact1.CO2Emission);
            Assert.IsNull     (environmentalImpact1.NuclearWaste);
            Assert.IsNull     (environmentalImpact3.NuclearWaste);
            Assert.IsNull     (environmentalImpact5.NuclearWaste);

            Assert.AreEqual   (20000, environmentalImpact3.CO2Emission);
            Assert.AreEqual   (25000, environmentalImpact5.CO2Emission);
            Assert.AreEqual   (30000, environmentalImpact6.CO2Emission);
            Assert.AreEqual   (50000, environmentalImpact6.NuclearWaste);

            Assert.AreEqual   (environmentalImpact1,   environmentalImpact2);
            Assert.AreEqual   (environmentalImpact3,   environmentalImpact4);
            Assert.AreEqual   (environmentalImpact6,   environmentalImpact7);

            Assert.AreNotEqual(environmentalImpact1,   environmentalImpact3);
            Assert.AreNotEqual(environmentalImpact1,   environmentalImpact5);
            Assert.AreNotEqual(environmentalImpact1,   environmentalImpact6);
            Assert.AreNotEqual(environmentalImpact3,   environmentalImpact5);
            Assert.AreNotEqual(environmentalImpact3,   environmentalImpact6);

            Assert.IsFalse    (environmentalImpact3  < environmentalImpact4);
            Assert.IsFalse    (environmentalImpact3  > environmentalImpact4);
            Assert.IsTrue     (environmentalImpact3 == environmentalImpact4);
            Assert.IsTrue     (environmentalImpact3 <= environmentalImpact4);
            Assert.IsTrue     (environmentalImpact3 >= environmentalImpact4);

            Assert.IsTrue     (environmentalImpact3  < environmentalImpact5);
            Assert.IsFalse    (environmentalImpact3  > environmentalImpact5);
            Assert.IsFalse    (environmentalImpact3 == environmentalImpact5);
            Assert.IsTrue     (environmentalImpact3 <= environmentalImpact5);
            Assert.IsFalse    (environmentalImpact3 >= environmentalImpact5);

        }

        #endregion

        #region EnvironmentalImpact_SerializeJSON_Test1()

        [Test]
        public void EnvironmentalImpact_SerializeJSON_Test1()
        {

            var environmentalImpact1 = new EnvironmentalImpact();
            var environmentalImpact2 = new EnvironmentalImpact(CO2Emission: 20000);
            var environmentalImpact3 = new EnvironmentalImpact(CO2Emission: 30000, NuclearWaste: 50000);

            Assert.IsNotNull(environmentalImpact1);
            Assert.IsNotNull(environmentalImpact2);
            Assert.IsNotNull(environmentalImpact3);

            var json1 = environmentalImpact1.ToJSON();
            var json2 = environmentalImpact2.ToJSON();
            var json3 = environmentalImpact3.ToJSON();

            Assert.IsNull   (json1);
            Assert.IsNotNull(json2);
            Assert.IsNotNull(json3);

            Assert.AreEqual (20000, json2?["CO2Emission"]?. Value<Decimal>());
            Assert.AreEqual (30000, json3?["CO2Emission"]?. Value<Decimal>());
            Assert.AreEqual (50000, json3?["NuclearWaste"]?.Value<Decimal>());

        }

        #endregion

        #region EnvironmentalImpact_ParseJSON_Test1()

        [Test]
        public void EnvironmentalImpact_ParseJSON_Test1()
        {

            var environmentalImpact1 = EnvironmentalImpact.Parse(new JObject());

            var environmentalImpact2 = EnvironmentalImpact.Parse(new JObject(new JProperty("CO2Emission",  20000)));

            var environmentalImpact3 = EnvironmentalImpact.Parse(new JObject(new JProperty("CO2Emission",  30000),
                                                                             new JProperty("NuclearWaste", 50000)));

            Assert.IsNotNull(environmentalImpact1);
            Assert.IsNotNull(environmentalImpact2);
            Assert.IsNotNull(environmentalImpact3);

            Assert.IsNull   (environmentalImpact1.CO2Emission);
            Assert.IsNull   (environmentalImpact1.NuclearWaste);
            Assert.IsNull   (environmentalImpact2.NuclearWaste);

            Assert.AreEqual (20000, environmentalImpact2.CO2Emission);
            Assert.AreEqual (30000, environmentalImpact3.CO2Emission);
            Assert.AreEqual (50000, environmentalImpact3.NuclearWaste);

        }

        #endregion

    }

}
