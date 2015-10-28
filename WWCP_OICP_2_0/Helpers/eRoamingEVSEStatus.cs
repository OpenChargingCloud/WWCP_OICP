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

#region Usings

using System;
using System.Collections.Generic;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// A group of OICP v2.0 operator EVSE status records or a status code.
    /// </summary>
    public class eRoamingEVSEStatus
    {

        #region Properties

        #region OperatorEVSEStatus

        private readonly IEnumerable<OperatorEVSEStatus> _OperatorEVSEStatus;

        /// <summary>
        /// An enumeration of EVSE status records grouped by their operators.
        /// </summary>
        public IEnumerable<OperatorEVSEStatus> OperatorEVSEStatus
        {
            get
            {
                return _OperatorEVSEStatus;
            }
        }

        #endregion

        #region StatusCode

        private readonly StatusCode _StatusCode;

        /// <summary>
        /// The status code for this request.
        /// </summary>
        public StatusCode StatusCode
        {
            get
            {
                return _StatusCode;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region (private) eRoamingEVSEStatus(OperatorEVSEStatus, StatusCode  = null)

        /// <summary>
        /// Create a new group of OICP v2.0 operator EVSE status records or a status code.
        /// </summary>
        /// <param name="OperatorEVSEStatus">An enumeration of EVSE status records grouped by their operators.</param>
        /// <param name="StatusCode">An optional status code for this request.</param>
        private eRoamingEVSEStatus(IEnumerable<OperatorEVSEStatus>  OperatorEVSEStatus,
                                   StatusCode                       StatusCode  = null)
        {

            #region Initial checks

            if (OperatorEVSEStatus == null)
                throw new ArgumentNullException("OperatorEVSEStatus", "The given parameter must not be null!");

            #endregion

            this._OperatorEVSEStatus  = OperatorEVSEStatus;
            this._StatusCode          = StatusCode != null ? StatusCode : new StatusCode(0);

        }

        #endregion

        #region (private) eRoamingEVSEStatus(StatusCode)

        /// <summary>
        /// Create a new group of OICP v2.0 operator EVSE status records or a status code.
        /// </summary>
        /// <pparam name="StatusCode">The status code for this request.</pparam>
        private eRoamingEVSEStatus(StatusCode  StatusCode)
        {

            this._OperatorEVSEStatus  = new OperatorEVSEStatus[0];
            this._StatusCode          = StatusCode == null ? StatusCode : new StatusCode(-1);

        }

        #endregion

        #endregion


        #region (static) Parse(eRoamingEVSEStatusXML)

        public static eRoamingEVSEStatus Parse(XElement eRoamingEVSEStatusXML)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEStatus  = "http://www.hubject.com/b2b/services/evsestatus/v2.0"
            //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            // [...]
            //
            //  <EVSEStatus:eRoamingEvseStatus>
            //
            //     <EVSEStatus:EvseStatuses>
            //        <!--Zero or more repetitions:-->
            //        <EVSEStatus:OperatorEvseStatus>
            //
            //           <EVSEStatus:OperatorID>?</EVSEStatus:OperatorID>
            //
            //           <!--Optional:-->
            //           <EVSEStatus:OperatorName>?</EVSEStatus:OperatorName>
            //
            //           <!--Zero or more repetitions:-->
            //           <EVSEStatus:EvseStatusRecord>
            //              <EVSEStatus:EvseId>?</EVSEStatus:EvseId>
            //              <EVSEStatus:EvseStatus>?</EVSEStatus:EvseStatus>
            //           </EVSEStatus:EvseStatusRecord>
            //
            //        </EVSEStatus:OperatorEvseStatus>
            //     </EVSEStatus:EvseStatuses>
            //
            //     <!--Optional:-->
            //     <EVSEStatus:StatusCode>
            //
            //        <CommonTypes:Code>?</CommonTypes:Code>
            //
            //        <!--Optional:-->
            //        <CommonTypes:Description>?</CommonTypes:Description>
            //
            //        <!--Optional:-->
            //        <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
            //
            //     </EVSEStatus:StatusCode>
            //
            //  </EVSEStatus:eRoamingEvseStatus>
            //
            // [...]
            //

            #endregion

            if (eRoamingEVSEStatusXML.Name != OICPNS.EVSEStatus + "eRoamingEvseStatus")
                throw new Exception("Invalid eRoamingEvseStatus XML!");

            var EVSEStatusXML  = eRoamingEVSEStatusXML.Element(OICPNS.EVSEStatus + "EvseStatuses");
            var StatusCodeXML  = eRoamingEVSEStatusXML.Element(OICPNS.EVSEStatus + "StatusCode");

            if (EVSEStatusXML != null)
            {

                var OperatorEvseStatusXMLs = EVSEStatusXML.Elements(OICPNS.EVSEStatus + "OperatorEvseStatus");

                if (OperatorEvseStatusXMLs != null)
                    return new eRoamingEVSEStatus(OICP_2_0.OperatorEVSEStatus.Parse(OperatorEvseStatusXMLs),
                                                  StatusCodeXML != null ? StatusCode.Parse(StatusCodeXML) : null);

            }

            return new eRoamingEVSEStatus(StatusCodeXML != null ? StatusCode.Parse(StatusCodeXML) : null);

        }

        #endregion


    }

}
