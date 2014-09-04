/*
 * Copyright (c) 2014 Achim Friedland <achim.friedland@graphdefined.com>
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

#region Usings

using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

#endregion

namespace com.graphdefined.eMI3.IO.OICP
{

    public static class ExtentionMethods
    {

        public static IEnumerable<HubjectEVSESearchReply> ParseSearchReplies(XElement XML)
        {
            return (from   EvseMatch
                    in     XML.Descendants(NS.OICPv1EVSESearch + "EvseMatch")
                    select new HubjectEVSESearchReply(EvseMatch)).ToArray();
        }

    }

}
