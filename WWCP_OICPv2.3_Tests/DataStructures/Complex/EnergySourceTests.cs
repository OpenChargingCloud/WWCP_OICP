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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests
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

            Assert.IsNotNull  (energySource1);
            Assert.IsNotNull  (energySource2);
            Assert.IsNotNull  (energySource3);

            Assert.AreEqual   (EnergyTypes.Coal,          energySource1.EnergyType);
            Assert.AreEqual   (EnergyTypes.Coal,          energySource2.EnergyType);
            Assert.AreEqual   (EnergyTypes.NuclearEnergy, energySource3.EnergyType);

            Assert.IsNull     (energySource1.Percentage);
            Assert.IsNull     (energySource2.Percentage);
            Assert.IsNull     (energySource3.Percentage);

            Assert.AreEqual   (energySource1.EnergyType, energySource2.EnergyType);
            Assert.AreNotEqual(energySource1.EnergyType, energySource3.EnergyType);
            Assert.AreNotEqual(energySource2.EnergyType, energySource3.EnergyType);

            Assert.AreEqual   (energySource1, energySource2);
            Assert.AreNotEqual(energySource1, energySource3);
            Assert.AreNotEqual(energySource2, energySource3);

            Assert.IsFalse    (energySource1 < energySource2);
            Assert.IsFalse    (energySource1 > energySource2);

        }

        #endregion

        #region EnergySource_Percentage_Test1()

        [Test]
        public void EnergySource_Percentage_Test1()
        {

            var energySource1 = new EnergySource(EnergyTypes.Coal,           25);
            var energySource2 = new EnergySource(EnergyTypes.Coal,           95);
            var energySource3 = new EnergySource(EnergyTypes.NuclearEnergy, 120);

            Assert.IsNotNull  (energySource1);
            Assert.IsNotNull  (energySource2);
            Assert.IsNotNull  (energySource3);

            Assert.AreEqual   (EnergyTypes.Coal,          energySource1.EnergyType);
            Assert.AreEqual   (EnergyTypes.Coal,          energySource2.EnergyType);
            Assert.AreEqual   (EnergyTypes.NuclearEnergy, energySource3.EnergyType);

            Assert.AreEqual   (25,                        energySource1.Percentage);
            Assert.AreEqual   (95,                        energySource2.Percentage);
            Assert.AreEqual   (100,                       energySource3.Percentage);

            Assert.AreEqual   (energySource1.EnergyType,  energySource2.EnergyType);
            Assert.AreNotEqual(energySource1.EnergyType,  energySource3.EnergyType);
            Assert.AreNotEqual(energySource2.EnergyType,  energySource3.EnergyType);

            Assert.AreNotEqual(energySource1.Percentage,  energySource2.Percentage);
            Assert.AreNotEqual(energySource1.Percentage,  energySource3.Percentage);
            Assert.AreNotEqual(energySource2.Percentage,  energySource3.Percentage);

            Assert.AreNotEqual(energySource1, energySource2);
            Assert.AreNotEqual(energySource1, energySource3);
            Assert.AreNotEqual(energySource2, energySource3);

            Assert.IsTrue     (energySource1 < energySource2);
            Assert.IsFalse    (energySource1 > energySource2);

        }

        #endregion

        #region EnergySource_SerializeJSON_Test1()

        [Test]
        public void EnergySource_SerializeJSON_Test1()
        {

            var energySource1 = new EnergySource(EnergyTypes.Coal);
            var energySource2 = new EnergySource(EnergyTypes.NuclearEnergy, 120);

            Assert.IsNotNull(energySource1);
            Assert.IsNotNull(energySource2);

            var json1 = energySource1.ToJSON();
            var json2 = energySource2.ToJSON();

            Assert.IsNotNull(json1);
            Assert.IsNotNull(json2);

            Assert.AreEqual ("Coal",          json1["Energy"]?.Value<String>());
            Assert.AreEqual ("NuclearEnergy", json2["Energy"]?.Value<String>());

            Assert.IsNull   (json1["Percentage"]);
            Assert.AreEqual (100,             json2["Percentage"]?.Value<Byte>());

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

            Assert.IsNotNull(energySource1);
            Assert.IsNotNull(energySource2);
            Assert.IsNull   (energySource3);
            Assert.IsNull   (energySource4);

            Assert.AreEqual (EnergyTypes.Coal,          energySource1.EnergyType);
            Assert.AreEqual (EnergyTypes.NuclearEnergy, energySource2.EnergyType);
            Assert.AreEqual (100,                       energySource2.Percentage);

        }

        #endregion

    }

}
