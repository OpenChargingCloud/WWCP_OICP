/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    public static class OICPClientHelper
    {

        #region IsHubjectError(XML)

        public static Boolean IsHubjectError(XElement                             XML,
                                             out OICPException                    OICPException,
                                             Action<DateTime, Object, Exception>  OnError)
        {

            #region Initial checks

            if (OnError == null)
                throw new ArgumentNullException(nameof(OnError),  "The given OnError-delegate must not be null!");

            #endregion

            if (StatusCode.TryParse(XML, out StatusCode _StatusCode))
            {
                OICPException = new OICPException(_StatusCode);
                OnError(DateTime.Now, XML, OICPException);
                return true;
            }

            OICPException = null;

            return false;

        }

        #endregion

    }

}
