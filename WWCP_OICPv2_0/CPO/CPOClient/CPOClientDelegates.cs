/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
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

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// A delegate called whenever new EVSE data record will be send upstream.
    /// </summary>
    public delegate void OnEVSEDataPushDelegate(DateTime                               Timestamp,
                                                Object                                 Sender,
                                                String                                 SenderId,
                                                ActionType                             ActionType,
                                                ILookup<EVSEOperator, EVSEDataRecord>  EVSEData,
                                                UInt32                                 NumberOfEVSEs);


    /// <summary>
    /// A delegate called whenever new EVSE data record had been send upstream.
    /// </summary>
    public delegate void OnEVSEDataPushedDelegate(DateTime                               Timestamp,
                                                  Object                                 Sender,
                                                  String                                 SenderId,
                                                  ActionType                             ActionType,
                                                  ILookup<EVSEOperator, EVSEDataRecord>  EVSEData,
                                                  UInt32                                 NumberOfEVSEs,
                                                  eRoamingAcknowledgement                Result,
                                                  TimeSpan                               Duration);

    
    /// <summary>
    /// A delegate called whenever new EVSE status will be send upstream.
    /// </summary>
    public delegate void OnEVSEStatusPushDelegate(DateTime                       Timestamp,
                                                  Object                         Sender,
                                                  String                         SenderId,
                                                  ActionType                     ActionType,
                                                  IEnumerable<EVSEStatusRecord>  EVSEStatusRecords,
                                                  UInt32                         NumberOfEVSEs);


    /// <summary>
    /// A delegate called whenever new EVSE status had been send upstream.
    /// </summary>
    public delegate void OnEVSEStatusPushedDelegate(DateTime                       Timestamp,
                                                    Object                         Sender,
                                                    String                         SenderId,
                                                    ActionType                     ActionType,
                                                    IEnumerable<EVSEStatusRecord>  EVSEStatusRecords,
                                                    UInt32                         NumberOfEVSEs,
                                                    eRoamingAcknowledgement        Result,
                                                    TimeSpan                       Duration);

}
