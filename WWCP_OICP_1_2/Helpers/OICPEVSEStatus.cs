/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/WorldWideCharging/WWCP_OICP>
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

namespace org.GraphDefined.WWCP.OICP_1_2
{

    public enum OICPEVSEStatus
    {
        Available,
        Reserved,
        Occupied,
        OutOfService,
        Unknown,
        EvseNotFound   // PullEVSEStatusById!
    }

    public static partial class Ext
    {

        public static EVSEStatusType AsEVSEStatusType(this OICPEVSEStatus EVSEStatus)
        {

            switch (EVSEStatus)
            {

                case OICPEVSEStatus.Available:
                    return EVSEStatusType.Available;

                case OICPEVSEStatus.Reserved:
                    return EVSEStatusType.Reserved;

                case OICPEVSEStatus.Occupied:
                    return EVSEStatusType.Occupied;

                case OICPEVSEStatus.OutOfService:
                    return EVSEStatusType.OutOfService;

                case OICPEVSEStatus.EvseNotFound:
                    return EVSEStatusType.EvseNotFound;

                default:
                    return EVSEStatusType.Unknown;

            }

        }

        public static OICPEVSEStatus AsOICPEVSEStatus(this EVSEStatusType EVSEStatus)
        {

            switch (EVSEStatus)
            {

                case EVSEStatusType.Available:
                    return OICPEVSEStatus.Available;

                case EVSEStatusType.Reserved:
                    return OICPEVSEStatus.Reserved;

                case EVSEStatusType.Occupied:
                    return OICPEVSEStatus.Occupied;

                case EVSEStatusType.OutOfService:
                    return OICPEVSEStatus.OutOfService;

                case EVSEStatusType.EvseNotFound:
                    return OICPEVSEStatus.EvseNotFound;

                default:
                    return OICPEVSEStatus.Unknown;

            }

        }

    }

}
