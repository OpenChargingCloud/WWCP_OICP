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
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// The current status of an OICP v2.0 Electric Vehicle Supply Equipment.
    /// </summary>
    public class EVSEStatusRecord
    {

        #region Properties

        #region EVSE

        private readonly EVSE _EVSE;

        /// <summary>
        /// The related the Electric Vehicle Supply Equipment (EVSE).
        /// </summary>
        public EVSE EVSE
        {
            get
            {
                return _EVSE;
            }
        }

        #endregion

        #region Id

        private readonly EVSE_Id _Id;

        /// <summary>
        /// The unique identification of an EVSE.
        /// </summary>
        public EVSE_Id Id
        {
            get
            {
                return _Id;
            }
        }

        #endregion

        #region Status

        private readonly EVSEStatusType _Status;

        /// <summary>
        /// The current status of an EVSE.
        /// </summary>
        public EVSEStatusType Status
        {
            get
            {
                return _Status;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region EVSEStatusRecord(EVSE)

        /// <summary>
        /// Create a new OICPv2.0 EVSE status record and store
        /// a reference to the given EVSE.
        /// </summary>
        /// <param name="EVSE">The current status of an EVSE.</param>
        public EVSEStatusRecord(EVSE EVSE)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException("EVSE", "The given EVSE must not be null!");

            #endregion

            this._EVSE    = EVSE;
            this._Id      = EVSE.Id;
            this._Status  = OICPMapper.AsOICPEVSEStatus(EVSE.Status.Value);

        }

        #endregion

        #region EVSEStatusRecord(Id, Status)

        /// <summary>
        /// Create a new OICPv2.0 EVSE status record.
        /// </summary>
        /// <param name="Id">The unique identification of an EVSE.</param>
        /// <param name="Status">The current status of an EVSE.</param>
        public EVSEStatusRecord(EVSE_Id         Id,
                                EVSEStatusType  Status)

        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException(nameof(Id), "The given unique identification of an EVSE must not be null!");

            #endregion

            this._Id      = Id;
            this._Status  = Status;

        }

        #endregion

        #endregion


        #region Parse(EVSEStatusRecordXML)

        /// <summary>
        /// Parse the EVSE identification and its current status from the given OICP XML.
        /// </summary>
        /// <param name="EVSEStatusRecordXML">An OICP XML.</param>
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
                    (EVSEStatusType) Enum.Parse(typeof(EVSEStatusType), EVSEStatusRecordXML.ElementValueOrFail(OICPNS.EVSEStatus + "EvseStatus"))
                );

            }
            catch (Exception e)
            {
                return null;
            }

        }

        #endregion

        #region Parse(KeyValuePair)

        /// <summary>
        /// Convert the given key-value-pair into an EVSE status record.
        /// </summary>
        public static EVSEStatusRecord Parse(KeyValuePair<EVSE_Id, EVSEStatusType> KeyValuePair)
        {
            return new EVSEStatusRecord(KeyValuePair.Key, KeyValuePair.Value);
        }

        #endregion


        #region ToXML()

        /// <summary>
        /// Return an OICP XML representation of this EVSE status record.
        /// </summary>
        /// <returns></returns>
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

        #region ToKeyValuePair()

        /// <summary>
        /// Conversion this EVSE status record to a key-value-pair.
        /// </summary>
        public KeyValuePair<EVSE_Id, EVSEStatusType> ToKeyValuePair()
        {
            return new KeyValuePair<EVSE_Id, EVSEStatusType>(_Id, _Status);
        }

        #endregion

    }

}
