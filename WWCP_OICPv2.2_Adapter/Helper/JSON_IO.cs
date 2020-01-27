/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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
using System.Linq;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    public static class JSON_IO
    {

        #region Address

        public static JObject ToJSON(this Address Address)

            => JSONObject.Create(

                   new JProperty("Country",        Address.Country.Alpha3Code),
                   new JProperty("City",           Address.City.   ToJSON()),
                   new JProperty("Street",         Address.Street),

                   Address.PostalCode.IsNotNullOrEmpty()
                       ? new JProperty("PostalCode",     Address.PostalCode)
                       : null,

                   Address.HouseNumber.IsNotNullOrEmpty()
                       ? new JProperty("HouseNumber",    Address.HouseNumber)
                       : null,

                   Address.FloorLevel.IsNotNullOrEmpty()
                       ? new JProperty("FloorLevel",     Address.FloorLevel)
                       : null,

                   Address.Region.IsNotNullOrEmpty()
                       ? new JProperty("Region",         Address.Region)
                       : null,

                   Address.Timezone.IsNotNullOrEmpty()
                       ? new JProperty("Timezone",       Address.Timezone)
                       : null

               );

        #endregion

        #region GeoCoordinate

        public static JObject ToJSON(this GeoCoordinate GeoCoordinate)

            => JSONObject.Create(

                   new JProperty("lat",         GeoCoordinate.Latitude. Value),
                   new JProperty("lng",         GeoCoordinate.Longitude.Value),

                   GeoCoordinate.Altitude.HasValue
                       ? new JProperty("alt",   GeoCoordinate.Altitude. Value)
                       : null

               );

        #endregion

    }

}
