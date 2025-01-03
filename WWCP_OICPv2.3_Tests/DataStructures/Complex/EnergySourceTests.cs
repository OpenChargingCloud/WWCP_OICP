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
    /// Energy source tests.
    /// </summary>
    [TestFixture]
    public class EnergySourceTests
    {

        #region EnergySource_Test1()

        [Test]
        public void EnergySource_Test1()
        {

            var energySource1 = new EnergySource(EnergyTypes.Coal);
            var energySource2 = new EnergySource(EnergyTypes.Coal);
            var energySource3 = new EnergySource(EnergyTypes.NuclearEnergy);

            ClassicAssert.IsNotNull  (energySource1);
            ClassicAssert.IsNotNull  (energySource2);
            ClassicAssert.IsNotNull  (energySource3);

            ClassicAssert.AreEqual   (EnergyTypes.Coal,          energySource1.EnergyType);
            ClassicAssert.AreEqual   (EnergyTypes.Coal,          energySource2.EnergyType);
            ClassicAssert.AreEqual   (EnergyTypes.NuclearEnergy, energySource3.EnergyType);

            ClassicAssert.IsNull     (energySource1.Percentage);
            ClassicAssert.IsNull     (energySource2.Percentage);
            ClassicAssert.IsNull     (energySource3.Percentage);

            ClassicAssert.AreEqual   (energySource1.EnergyType, energySource2.EnergyType);
            ClassicAssert.AreNotEqual(energySource1.EnergyType, energySource3.EnergyType);
            ClassicAssert.AreNotEqual(energySource2.EnergyType, energySource3.EnergyType);

            ClassicAssert.AreEqual   (energySource1, energySource2);
            ClassicAssert.AreNotEqual(energySource1, energySource3);
            ClassicAssert.AreNotEqual(energySource2, energySource3);

            ClassicAssert.IsFalse    (energySource1 < energySource2);
            ClassicAssert.IsFalse    (energySource1 > energySource2);

        }

        #endregion

        #region EnergySource_Percentage_Test1()

        [Test]
        public void EnergySource_Percentage_Test1()
        {

            var energySource1 = new EnergySource(EnergyTypes.Coal,           25);
            var energySource2 = new EnergySource(EnergyTypes.Coal,           95);
            var energySource3 = new EnergySource(EnergyTypes.NuclearEnergy, 120);

            ClassicAssert.IsNotNull  (energySource1);
            ClassicAssert.IsNotNull  (energySource2);
            ClassicAssert.IsNotNull  (energySource3);

            ClassicAssert.AreEqual   (EnergyTypes.Coal,          energySource1.EnergyType);
            ClassicAssert.AreEqual   (EnergyTypes.Coal,          energySource2.EnergyType);
            ClassicAssert.AreEqual   (EnergyTypes.NuclearEnergy, energySource3.EnergyType);

            ClassicAssert.AreEqual   (25,                        energySource1.Percentage);
            ClassicAssert.AreEqual   (95,                        energySource2.Percentage);
            ClassicAssert.AreEqual   (100,                       energySource3.Percentage);

            ClassicAssert.AreEqual   (energySource1.EnergyType,  energySource2.EnergyType);
            ClassicAssert.AreNotEqual(energySource1.EnergyType,  energySource3.EnergyType);
            ClassicAssert.AreNotEqual(energySource2.EnergyType,  energySource3.EnergyType);

            ClassicAssert.AreNotEqual(energySource1.Percentage,  energySource2.Percentage);
            ClassicAssert.AreNotEqual(energySource1.Percentage,  energySource3.Percentage);
            ClassicAssert.AreNotEqual(energySource2.Percentage,  energySource3.Percentage);

            ClassicAssert.AreNotEqual(energySource1, energySource2);
            ClassicAssert.AreNotEqual(energySource1, energySource3);
            ClassicAssert.AreNotEqual(energySource2, energySource3);

            ClassicAssert.IsTrue     (energySource1 < energySource2);
            ClassicAssert.IsFalse    (energySource1 > energySource2);

        }

        #endregion

        #region EnergySource_SerializeJSON_Test1()

        [Test]
        public void EnergySource_SerializeJSON_Test1()
        {

            var energySource1 = new EnergySource(EnergyTypes.Coal);
            var energySource2 = new EnergySource(EnergyTypes.NuclearEnergy, 120);

            ClassicAssert.IsNotNull(energySource1);
            ClassicAssert.IsNotNull(energySource2);

            var json1 = energySource1.ToJSON();
            var json2 = energySource2.ToJSON();

            ClassicAssert.IsNotNull(json1);
            ClassicAssert.IsNotNull(json2);

            ClassicAssert.AreEqual ("Coal",          json1["Energy"]?.Value<String>());
            ClassicAssert.AreEqual ("NuclearEnergy", json2["Energy"]?.Value<String>());

            ClassicAssert.IsNull   (json1["Percentage"]);
            ClassicAssert.AreEqual (100,             json2["Percentage"]?.Value<Byte>());

        }

        #endregion

        #region EnergySource_ParseJSON_Test1()

        [Test]
        public void EnergySource_ParseJSON_Test1()
        {

            var energySource1 = EnergySource.Parse   (new JObject(new JProperty("Energy",     "Coal")));

            var energySource2 = EnergySource.Parse   (new JObject(new JProperty("Energy",     "NuclearEnergy"),
                                                                 new JProperty("Percentage",  120)));

            var energySource3 = EnergySource.TryParse(new JObject(new JProperty("Energy",     "GeothermalEnergy"),
                                                                  new JProperty("Percentage",  25738)));

            var energySource4 = EnergySource.TryParse(new JObject(new JProperty("Energy",     "PoliticalTalks"),
                                                                  new JProperty("Percentage",  38)));

            ClassicAssert.IsNotNull(energySource1);
            ClassicAssert.IsNotNull(energySource2);
            ClassicAssert.IsNull   (energySource3);
            ClassicAssert.IsNull   (energySource4);

            ClassicAssert.AreEqual (EnergyTypes.Coal,          energySource1.EnergyType);
            ClassicAssert.AreEqual (EnergyTypes.NuclearEnergy, energySource2.EnergyType);
            ClassicAssert.AreEqual (100,                       energySource2.Percentage);

        }

        #endregion

    }

}
