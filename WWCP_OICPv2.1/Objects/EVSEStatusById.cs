/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// A group of OICP operator EVSE status records or a status code.
    /// </summary>
    public class EVSEStatusById
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE status records.
        /// </summary>
        public IEnumerable<EVSEStatusRecord>  EVSEStatusRecords   { get; }

        /// <summary>
        /// The status code for this request.
        /// </summary>
        public StatusCode                     StatusCode          { get; }

        #endregion

        #region Constructor(s)

        #region EVSEStatusById(OperatorEVSEStatus, StatusCode  = null)

        /// <summary>
        /// Create a new group of OICP operator EVSE status records or a status code.
        /// </summary>
        /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
        /// <param name="StatusCode">An optional status code for this request.</param>
        public EVSEStatusById(IEnumerable<EVSEStatusRecord>  EVSEStatusRecords,
                              StatusCode                     StatusCode  = null)
        {

            #region Initial checks

            if (EVSEStatusRecords == null)
                throw new ArgumentNullException(nameof(EVSEStatusRecords),  "The given enumeration of EVSE status records must not be null!");

            #endregion

            this.EVSEStatusRecords  = EVSEStatusRecords;
            this.StatusCode         = StatusCode ?? new StatusCode(StatusCodes.Success);

        }

        #endregion

        #region EVSEStatusById(Code, Description = null, AdditionalInfo = null)

        /// <summary>
        /// Create a new group of OICP operator EVSE status records or a status code.
        /// </summary>
        /// <param name="Code">The result code of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="AdditionalInfo">An optional additional information.</param>
        public EVSEStatusById(StatusCodes  Code,
                              String       Description     = null,
                              String       AdditionalInfo  = null)
        {

            this.EVSEStatusRecords  = new EVSEStatusRecord[0];
            this.StatusCode         = new StatusCode(Code,
                                                     Description,
                                                     AdditionalInfo);

        }

        #endregion

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEStatus  = "http://www.hubject.com/b2b/services/evsestatus/v2.0"
        //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        // [...]
        //
        //  <EVSEStatus:eRoamingEvseStatusById>
        //
        //     <!--Optional:-->
        //     <EVSEStatus:EvseStatusRecords>
        //
        //        <!--Zero or more repetitions:-->
        //        <EVSEStatus:EvseStatusRecord>
        //           <EVSEStatus:EvseId>?</EVSEStatus:EvseId>
        //           <EVSEStatus:EvseStatus>?</EVSEStatus:EvseStatus>
        //        </EVSEStatus:EvseStatusRecord>
        //
        //     </EVSEStatus:EvseStatusRecords>
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

        #region (static) Parse(eRoamingEvseStatusByIdXML)

        /// <summary>
        /// Parse the givem XML as OICP EVSE status records or a status code.
        /// </summary>
        /// <param name="EVSEStatusByIdXML">A XML representation of EVSE status records or a status code.</param>
        public static EVSEStatusById Parse(XElement EVSEStatusByIdXML)
        {

            if (EVSEStatusByIdXML.Name != OICPNS.EVSEStatus + "eRoamingEvseStatusById")
                throw new Exception("Invalid eRoamingEvseStatusById XML!");

            var _EVSEStatusRecordsXML  = EVSEStatusByIdXML.Element   (OICPNS.EVSEStatus + "EvseStatusRecords");
            var _StatusCode            = EVSEStatusByIdXML.MapElement(OICPNS.EVSEStatus + "StatusCode",
                                                                              StatusCode.Parse);

            if (_EVSEStatusRecordsXML != null)

                return new EVSEStatusById(_EVSEStatusRecordsXML.
                                              Elements  (OICPNS.EVSEStatus + "EvseStatusRecord").
                                              SafeSelect(EvseStatusRecordXML => EVSEStatusRecord.Parse(EvseStatusRecordXML)).
                                              Where     (statusrecord        => statusrecord != null),
                                          _StatusCode);


            return _StatusCode != null

                       ? new EVSEStatusById(_StatusCode.Code,
                                            _StatusCode.Description,
                                            _StatusCode.AdditionalInfo)

                       : new EVSEStatusById(StatusCodes.DataError);

        }

        #endregion


    }

}
