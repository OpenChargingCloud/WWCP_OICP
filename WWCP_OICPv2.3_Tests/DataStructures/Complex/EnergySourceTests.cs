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

            Assert.That(energySource1,                   Is.Not.Null);
            Assert.That(energySource2,                   Is.Not.Null);
            Assert.That(energySource3,                   Is.Not.Null);
            Assert.That(energySource1.EnergyType,        Is.EqualTo(EnergyTypes.Coal));
            Assert.That(energySource2.EnergyType,        Is.EqualTo(EnergyTypes.Coal));
            Assert.That(energySource3.EnergyType,        Is.EqualTo(EnergyTypes.NuclearEnergy));
            Assert.That(energySource1.Percentage,        Is.Null);
            Assert.That(energySource2.Percentage,        Is.Null);
            Assert.That(energySource3.Percentage,        Is.Null);
            Assert.That(energySource1.EnergyType,        Is.EqualTo(energySource2.EnergyType));
            Assert.That(energySource1.EnergyType,        Is.Not.EqualTo(energySource3.EnergyType));
            Assert.That(energySource2.EnergyType,        Is.Not.EqualTo(energySource3.EnergyType));
            Assert.That(energySource1,                   Is.EqualTo(energySource2));
            Assert.That(energySource1,                   Is.Not.EqualTo(energySource3));
            Assert.That(energySource2,                   Is.Not.EqualTo(energySource3));
            Assert.That(energySource1 < energySource2,   Is.False);
            Assert.That(energySource1 > energySource2,   Is.False);

        }

        #endregion

        #region EnergySource_Percentage_Test1()

        [Test]
        public void EnergySource_Percentage_Test1()
        {

            var energySource1 = new EnergySource(EnergyTypes.Coal,           25);
            var energySource2 = new EnergySource(EnergyTypes.Coal,           95);
            var energySource3 = new EnergySource(EnergyTypes.NuclearEnergy, 120);

            Assert.That(energySource1,                   Is.Not.Null);
            Assert.That(energySource2,                   Is.Not.Null);
            Assert.That(energySource3,                   Is.Not.Null);
            Assert.That(energySource1.EnergyType,        Is.EqualTo(EnergyTypes.Coal));
            Assert.That(energySource2.EnergyType,        Is.EqualTo(EnergyTypes.Coal));
            Assert.That(energySource3.EnergyType,        Is.EqualTo(EnergyTypes.NuclearEnergy));
            Assert.That(energySource1.Percentage,        Is.EqualTo(25));
            Assert.That(energySource2.Percentage,        Is.EqualTo(95));
            Assert.That(energySource3.Percentage,        Is.EqualTo(100));
            Assert.That(energySource1.EnergyType,        Is.EqualTo(energySource2.EnergyType));
            Assert.That(energySource1.EnergyType,        Is.Not.EqualTo(energySource3.EnergyType));
            Assert.That(energySource2.EnergyType,        Is.Not.EqualTo(energySource3.EnergyType));
            Assert.That(energySource1.Percentage,        Is.Not.EqualTo(energySource2.Percentage));
            Assert.That(energySource1.Percentage,        Is.Not.EqualTo(energySource3.Percentage));
            Assert.That(energySource2.Percentage,        Is.Not.EqualTo(energySource3.Percentage));
            Assert.That(energySource1,                   Is.Not.EqualTo(energySource2));
            Assert.That(energySource1,                   Is.Not.EqualTo(energySource3));
            Assert.That(energySource2,                   Is.Not.EqualTo(energySource3));
            Assert.That(energySource1 < energySource2,   Is.True);
            Assert.That(energySource1 > energySource2,   Is.False);

        }

        #endregion

        #region EnergySource_SerializeJSON_Test1()

        [Test]
        public void EnergySource_SerializeJSON_Test1()
        {

            var energySource1  = new EnergySource(EnergyTypes.Coal);
            var energySource2  = new EnergySource(EnergyTypes.NuclearEnergy, 120);

            Assert.That(energySource1,                          Is.Not.Null);
            Assert.That(energySource2,                          Is.Not.Null);

            var json1 = energySource1.ToJSON();
            var json2 = energySource2.ToJSON();

            Assert.That(json1,                                  Is.Not.Null);
            Assert.That(json2,                                  Is.Not.Null);
            Assert.That(json1["Energy"]?.    Value<String>(),   Is.EqualTo("Coal"));
            Assert.That(json2["Energy"]?.    Value<String>(),   Is.EqualTo("NuclearEnergy"));
            Assert.That(json1["Percentage"],                    Is.Null);
            Assert.That(json2["Percentage"]?.Value<Byte>(),     Is.EqualTo(100));

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

            Assert.That(energySource1,              Is.Not.Null);
            Assert.That(energySource2,              Is.Not.Null);
            Assert.That(energySource3,              Is.Null);
            Assert.That(energySource4,              Is.Null);
            Assert.That(energySource1.EnergyType,   Is.EqualTo(EnergyTypes.Coal));
            Assert.That(energySource2.EnergyType,   Is.EqualTo(EnergyTypes.NuclearEnergy));
            Assert.That(energySource2.Percentage,   Is.EqualTo(100));

        }

        #endregion

    }

}
