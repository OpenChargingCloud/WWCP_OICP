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

            ClassicAssert.IsNotNull  (environmentalImpact1);
            ClassicAssert.IsNotNull  (environmentalImpact3);
            ClassicAssert.IsNotNull  (environmentalImpact5);
            ClassicAssert.IsNotNull  (environmentalImpact6);

            ClassicAssert.IsNull     (environmentalImpact1.CO2Emission);
            ClassicAssert.IsNull     (environmentalImpact1.NuclearWaste);
            ClassicAssert.IsNull     (environmentalImpact3.NuclearWaste);
            ClassicAssert.IsNull     (environmentalImpact5.NuclearWaste);

            ClassicAssert.AreEqual   (20000, environmentalImpact3.CO2Emission);
            ClassicAssert.AreEqual   (25000, environmentalImpact5.CO2Emission);
            ClassicAssert.AreEqual   (30000, environmentalImpact6.CO2Emission);
            ClassicAssert.AreEqual   (50000, environmentalImpact6.NuclearWaste);

            ClassicAssert.AreEqual   (environmentalImpact1,   environmentalImpact2);
            ClassicAssert.AreEqual   (environmentalImpact3,   environmentalImpact4);
            ClassicAssert.AreEqual   (environmentalImpact6,   environmentalImpact7);

            ClassicAssert.AreNotEqual(environmentalImpact1,   environmentalImpact3);
            ClassicAssert.AreNotEqual(environmentalImpact1,   environmentalImpact5);
            ClassicAssert.AreNotEqual(environmentalImpact1,   environmentalImpact6);
            ClassicAssert.AreNotEqual(environmentalImpact3,   environmentalImpact5);
            ClassicAssert.AreNotEqual(environmentalImpact3,   environmentalImpact6);

            ClassicAssert.IsFalse    (environmentalImpact3  < environmentalImpact4);
            ClassicAssert.IsFalse    (environmentalImpact3  > environmentalImpact4);
            ClassicAssert.IsTrue     (environmentalImpact3 == environmentalImpact4);
            ClassicAssert.IsTrue     (environmentalImpact3 <= environmentalImpact4);
            ClassicAssert.IsTrue     (environmentalImpact3 >= environmentalImpact4);

            ClassicAssert.IsTrue     (environmentalImpact3  < environmentalImpact5);
            ClassicAssert.IsFalse    (environmentalImpact3  > environmentalImpact5);
            ClassicAssert.IsFalse    (environmentalImpact3 == environmentalImpact5);
            ClassicAssert.IsTrue     (environmentalImpact3 <= environmentalImpact5);
            ClassicAssert.IsFalse    (environmentalImpact3 >= environmentalImpact5);

        }

        #endregion

        #region EnvironmentalImpact_SerializeJSON_Test1()

        [Test]
        public void EnvironmentalImpact_SerializeJSON_Test1()
        {

            var environmentalImpact1 = new EnvironmentalImpact();
            var environmentalImpact2 = new EnvironmentalImpact(CO2Emission: 20000);
            var environmentalImpact3 = new EnvironmentalImpact(CO2Emission: 30000, NuclearWaste: 50000);

            ClassicAssert.IsNotNull(environmentalImpact1);
            ClassicAssert.IsNotNull(environmentalImpact2);
            ClassicAssert.IsNotNull(environmentalImpact3);

            var json1 = environmentalImpact1.ToJSON();
            var json2 = environmentalImpact2.ToJSON();
            var json3 = environmentalImpact3.ToJSON();

            ClassicAssert.IsNull   (json1);
            ClassicAssert.IsNotNull(json2);
            ClassicAssert.IsNotNull(json3);

            ClassicAssert.AreEqual (20000, json2?["CO2Emission"]?. Value<Decimal>());
            ClassicAssert.AreEqual (30000, json3?["CO2Emission"]?. Value<Decimal>());
            ClassicAssert.AreEqual (50000, json3?["NuclearWaste"]?.Value<Decimal>());

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

            ClassicAssert.IsNotNull(environmentalImpact1);
            ClassicAssert.IsNotNull(environmentalImpact2);
            ClassicAssert.IsNotNull(environmentalImpact3);

            ClassicAssert.IsNull   (environmentalImpact1.CO2Emission);
            ClassicAssert.IsNull   (environmentalImpact1.NuclearWaste);
            ClassicAssert.IsNull   (environmentalImpact2.NuclearWaste);

            ClassicAssert.AreEqual (20000, environmentalImpact2.CO2Emission);
            ClassicAssert.AreEqual (30000, environmentalImpact3.CO2Emission);
            ClassicAssert.AreEqual (50000, environmentalImpact3.NuclearWaste);

        }

        #endregion

    }

}
