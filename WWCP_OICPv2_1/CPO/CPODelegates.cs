﻿/*
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
using System.Xml.Linq;
using System.Threading.Tasks;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    public delegate EVSEDataRecord EVSE2EVSEDataRecordDelegate  (EVSE EVSE, EVSEDataRecord EVSEDataRecord);

    public delegate XElement       EVSEDataRecord2XMLDelegate   (EVSEDataRecord EVSEDataRecord, XElement XML);

    public delegate XElement       EVSEStatusRecord2XMLDelegate (EVSEStatusRecord EVSEStatusRecord, XElement XML);

    public delegate XElement       XMLPostProcessingDelegate    (XElement XML);

    public delegate Task<TResult>  CPOServiceCheckDelegate<TResult>(DateTime Timestamp, CPOServiceCheck<TResult> CPOServiceCheck, CPORoaming CPORoaming);


}
