/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of eMI3 OICP <http://www.github.com/eMI3/OICP-Bindings>
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

namespace com.graphdefined.eMI3.IO.OICP
{

    public enum HubjectEVSEState
    {
        Available,
        Reserved,
        Occupied,
        OutOfService,
        Unknown
    }

    public static class Ext
    {

        public static EVSEStatusType AsEVSEStatusType(this HubjectEVSEState EVSEStatus)
        {

            switch (EVSEStatus)
            {

                case HubjectEVSEState.Available:
                    return EVSEStatusType.Available;

                case HubjectEVSEState.Reserved:
                    return EVSEStatusType.Reserved;

                case HubjectEVSEState.Occupied:
                    return EVSEStatusType.Charging;

                case HubjectEVSEState.OutOfService:
                    return EVSEStatusType.OutOfService;

                default:
                    return EVSEStatusType.Unknown;

            }

        }

        public static HubjectEVSEState AsHubjectEVSEState(this EVSEStatusType EVSEStatus)
        {

            switch (EVSEStatus)
            {

                case EVSEStatusType.Available:
                    return HubjectEVSEState.Available;

                case EVSEStatusType.Reserved:
                    return HubjectEVSEState.Reserved;

                case EVSEStatusType.Charging:
                    return HubjectEVSEState.Occupied;

                case EVSEStatusType.OutOfService:
                    return HubjectEVSEState.OutOfService;

                default:
                    return HubjectEVSEState.Unknown;

            }

        }

    }

}
