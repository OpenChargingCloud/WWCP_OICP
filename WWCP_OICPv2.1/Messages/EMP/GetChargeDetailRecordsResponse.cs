/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// An OICP get charge detail records response.
    /// </summary>
    public class GetChargeDetailRecordsResponse : AResponse<GetChargeDetailRecordsRequest,
                                                            GetChargeDetailRecordsResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of charge detail records.
        /// </summary>
        public IEnumerable<ChargeDetailRecord>  ChargeDetailRecords   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group of OICP Charge Detail Records.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        public GetChargeDetailRecordsResponse(GetChargeDetailRecordsRequest        Request,
                                              IEnumerable<ChargeDetailRecord>      ChargeDetailRecords,
                                              IReadOnlyDictionary<String, Object>  CustomData = null)

            : base(Request,
                   CustomData)

        {

            #region Initial checks

            if (ChargeDetailRecords == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecords), "The given enumeration of charge detail records must not be null!");

            #endregion

            this.ChargeDetailRecords  = ChargeDetailRecords;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0"
        //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        // 
        //    <soapenv:Header/>
        // 
        //    <soapenv:Body>
        //       <Authorization:eRoamingChargeDetailRecords>
        // 
        //          <!--Zero or more repetitions:-->
        //          <Authorization:eRoamingChargeDetailRecord>
        //             [...]
        //          </Authorization:eRoamingChargeDetailRecord>
        // 
        //       </Authorization:eRoamingChargeDetailRecords>
        //    </soapenv:Body>
        // 
        // </soapenv:Envelope>

        #endregion

        #region (static) ParseXML(eRoamingChargeDetailRecordsXML, OnException = null)

        /// <summary>
        /// Parse the givem XML as an OICP charge detail records.
        /// </summary>
        /// <param name="ChargeDetailRecordsXML">A XML representation of an enumeration of OICP charge detail records.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetChargeDetailRecordsResponse ParseXML(GetChargeDetailRecordsRequest  Request,
                                                              XElement                       ChargeDetailRecordsXML,
                                                              Func<XElement, Dictionary<String, Object>> CustomerMapper1,
                                                              Func<XElement, Dictionary<String, Object>> CustomerMapper2,
                                                              OnExceptionDelegate            OnException  = null)
        {

            if (ChargeDetailRecordsXML.Name != OICPNS.Authorization + "eRoamingChargeDetailRecords")
                throw new Exception("Invalid eRoamingChargeDetailRecords XML!");

            return new GetChargeDetailRecordsResponse(

                       Request,

                       ChargeDetailRecordsXML.MapElements(OICPNS.Authorization + "eRoamingChargeDetailRecord",
                                                             //(XML, e) => CustomerMapper(XML, ChargeDetailRecord.Parse(XML, e)),
                                                             (XML, e) => ChargeDetailRecord.Parse(XML, e),
                                                             OnException),

                       CustomerMapper1 != null
                           ? CustomerMapper1(ChargeDetailRecordsXML)
                           : null

                   );

        }

        #endregion


        public override bool Equals(GetChargeDetailRecordsResponse AResponse)
        {
            throw new NotImplementedException();
        }


    }

}
