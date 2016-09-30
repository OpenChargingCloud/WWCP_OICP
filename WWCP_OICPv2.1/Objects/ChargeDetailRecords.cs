/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// A group of OICP Charge Detail Records.
    /// </summary>
    public class ChargeDetailRecords
    {

        #region Properties

        /// <summary>
        /// An enumeration of charge detail records.
        /// </summary>
        public IEnumerable<ChargeDetailRecord> Records { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group of OICP Charge Detail Records.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        public ChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords)
        {

            #region Initial checks

            if (ChargeDetailRecords == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecords),  "The given parameter must not be null!");

            #endregion

            this.Records  = ChargeDetailRecords;

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
        public static IEnumerable<ChargeDetailRecord> ParseXML(XElement             ChargeDetailRecordsXML,
                                                               OnExceptionDelegate  OnException  = null)
        {

            if (ChargeDetailRecordsXML.Name != OICPNS.Authorization + "eRoamingChargeDetailRecords")
                throw new Exception("Invalid eRoamingChargeDetailRecords XML!");

            return ChargeDetailRecordsXML.MapElements(OICPNS.Authorization + "eRoamingChargeDetailRecord",
                                                      (XML, e) => ChargeDetailRecord.Parse(XML, e),
                                                      OnException);

        }

        #endregion


    }

}
