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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// A OICP v2.0 Electric Vehicle Supply Equipment Status (EVSE).
    /// </summary>
    public class EVSEStatusRecord
    {

        #region Properties

        #region Id

        private readonly EVSE_Id _Id;

        public EVSE_Id Id
        {
            get
            {
                return _Id;
            }
        }

        #endregion

        #region Status

        private readonly OICPEVSEStatusType _Status;

        public OICPEVSEStatusType Status
        {
            get
            {
                return _Status;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public EVSEStatusRecord(EVSE_Id         Id,
                                OICPEVSEStatusType  Status)

        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The given parameter must not be null!");

            #endregion

            this._Id  = Id;
            this._Status  = Status;

        }

        #endregion


        #region Parse(EVSEStatusRecordXML)

        public static EVSEStatusRecord Parse(XElement EVSEStatusRecordXML)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEStatus  = "http://www.hubject.com/b2b/services/evsestatus/v2.0"
            //
            // [...]
            //
            //    <EVSEStatus:EvseStatusRecord>
            //       <EVSEStatus:EvseId>?</EVSEStatus:EvseId>
            //       <EVSEStatus:EvseStatus>?</EVSEStatus:EvseStatus>
            //    </EVSEStatus:EvseStatusRecord>
            //
            // [...]

            #endregion

            try
            {

                if (EVSEStatusRecordXML.Name != OICPNS.EVSEStatus + "EvseStatusRecord")
                    throw new Exception("Illegal EVSEStatusRecord XML!");

                return new EVSEStatusRecord(
                    EVSE_Id.Parse(EVSEStatusRecordXML.ElementValueOrFail(OICPNS.EVSEStatus + "EvseId")),
                    (OICPEVSEStatusType) Enum.Parse(typeof(OICPEVSEStatusType), EVSEStatusRecordXML.ElementValueOrFail(OICPNS.EVSEStatus + "EvseStatus"))
                );

            }
            catch (Exception e)
            {
                return null;
            }

        }

        #endregion

        #region ToXML()

        public XElement ToXML()
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv    = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEStatus = "http://www.hubject.com/b2b/services/evsestatus/EVSEData.0">
            //
            // [...]
            //
            //   <EVSEStatus:EvseStatusRecord>
            //      <EVSEStatus:EvseId>?</EVSEData:EvseId>
            //      <EVSEStatus:EvseStatus>?</EVSEData:EvseStatus>
            //   </EVSEStatus:EvseStatusRecord>
            //
            // [...]

            #endregion

            return new XElement(OICPNS.EVSEStatus + "EvseStatusRecord",
                       new XElement(OICPNS.EVSEStatus + "EvseId",     _Id.    OriginId),
                       new XElement(OICPNS.EVSEStatus + "EvseStatus", _Status.ToString())
                   );

        }

        #endregion


        #region (implicit) ToEVSEStatusRecord(KeyValuePair)

        /// <summary>
        /// Implicit conversion from KeyValuePair to EVSEStatusRecord.
        /// </summary>
        public EVSEStatusRecord ToEVSEStatusRecord(KeyValuePair<EVSE_Id, OICPEVSEStatusType> KeyValuePair)
        {
            return new EVSEStatusRecord(KeyValuePair.Key, KeyValuePair.Value);
        }

        #endregion

        #region (implicit) ToKeyValuePair(EVSEStatusRecord EVSEStatusRecord)

        /// <summary>
        /// Implicit conversion from EVSEStatusRecord to KeyValuePair.
        /// </summary>
        public KeyValuePair<EVSE_Id, OICPEVSEStatusType> ToKeyValuePair(EVSEStatusRecord EVSEStatusRecord)
        {
            return new KeyValuePair<EVSE_Id, OICPEVSEStatusType>(EVSEStatusRecord.Id, EVSEStatusRecord.Status);
        }

        #endregion

    }

}
