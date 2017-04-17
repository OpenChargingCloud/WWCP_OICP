/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    public static class JSON_IO
    {

        #region Address

        public static JObject ToJSON(this Address Address)

            => JSONObject.Create(
                   new JProperty("Street",         Address.Street),
                   new JProperty("HouseNumber",    Address.HouseNumber),
                   new JProperty("FloorLevel",     Address.FloorLevel),
                   new JProperty("PostalCode",     Address.PostalCode),
                   new JProperty("PostalCodeSub",  Address.PostalCodeSub),
                   new JProperty("City",           Address.City.   ToJSON()),
                   new JProperty("Country",        Address.Country.Alpha3Code),
                   new JProperty("Comment",        Address.Comment.ToJSON())
               );

        #endregion

        #region Address

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
