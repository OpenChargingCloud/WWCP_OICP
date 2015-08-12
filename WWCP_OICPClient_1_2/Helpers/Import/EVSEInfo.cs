/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICPClient <https://github.com/WorldWideCharging/WWCP_OICP>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OICPClient_1_2
{

    public class EVSEInfo
    {

        public ChargingPool_Id      PoolId        { get; private set; }
        public Address              PoolAddress   { get; private set; }
        public GeoCoordinate        PoolLocation  { get; private set; }
        public ChargingStation_Id   StationId     { get; private set; }


        public EVSEInfo(ChargingPool_Id      PoolId,
                        Address              PoolAddress,
                        GeoCoordinate        PoolLocation,
                        ChargingStation_Id   StationId)
        {

            this.PoolId        = PoolId;
            this.PoolAddress   = PoolAddress;
            this.PoolLocation  = PoolLocation;
            this.StationId     = StationId;

        }

    }

}
