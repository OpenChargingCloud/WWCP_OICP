/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICPClient <https://github.com/WorldWideCharging/WWCP_OICPClient>
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

namespace org.GraphDefined.WWCP.OICPClient_2_0
{

    /// <summary>
    /// A OICP v2.0 Electric Vehicle Supply Equipment (EVSE).
    /// This is meant to be one electrical circuit which can charge a electric vehicle.
    /// </summary>
    public class EVSEDataRecord
    {

        #region Properties

        #region EVSEId

        private readonly EVSE_Id _EVSEId;

        public EVSE_Id EVSEId
        {
            get
            {
                return _EVSEId;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public EVSEDataRecord(EVSE_Id  EVSEId)

        {

            #region Initial checks

            if (EVSEId != null)
                throw new ArgumentNullException("EVSEId", "The given parameter must not be null!");

            #endregion

            this._EVSEId  = EVSEId;

        }

        #endregion


    }

}
