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
    /// The current dynamic status of an OICP Electric Vehicle Supply Equipment.
    /// </summary>
    public class EVSEStatusRecord
    {

        #region Properties

        /// <summary>
        /// The unique identification of an EVSE.
        /// </summary>
        public EVSE_Id         Id       { get; }

        /// <summary>
        /// The current status of an EVSE.
        /// </summary>
        public EVSEStatusTypes  Status   { get; }

        #endregion

        #region Constructor(s)

        #region EVSEStatusRecord(EVSE)

        /// <summary>
        /// Create a new OICP EVSE status record based on the given EVSE.
        /// </summary>
        /// <param name="EVSE">The current status of an EVSE.</param>
        public EVSEStatusRecord(EVSE EVSE)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE),  "The given EVSE must not be null!");

            if (!Definitions.EVSEIdRegExpr.IsMatch(EVSE.Id.ToString()))
                throw new ArgumentException("The given EVSE identification '" + EVSE.Id + "' does not match the OICP definition!", nameof(EVSE));

            #endregion

            this.Id      = EVSE.Id;
            this.Status  = OICPMapper.AsOICPEVSEStatus(EVSE.Status.Value);

        }

        #endregion

        #region EVSEStatusRecord(Id, Status)

        /// <summary>
        /// Create a new OICP EVSE status record.
        /// </summary>
        /// <param name="Id">The unique identification of an EVSE.</param>
        /// <param name="Status">The current status of an EVSE.</param>
        public EVSEStatusRecord(EVSE_Id         Id,
                                EVSEStatusTypes  Status)

        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException(nameof(Id),  "The given unique identification of an EVSE must not be null!");

            if (!Definitions.EVSEIdRegExpr.IsMatch(Id.ToString()))
                throw new ArgumentException("The given EVSE identification '" + Id + "' does not match the OICP definition!", nameof(Id));

            #endregion

            this.Id      = Id;
            this.Status  = Status;

        }

        #endregion

        #endregion


        #region Parse(KeyValuePair)

        /// <summary>
        /// Convert the given key-value-pair into an EVSE status record.
        /// </summary>
        public static EVSEStatusRecord Parse(KeyValuePair<EVSE_Id, EVSEStatusTypes> KeyValuePair)

            => new EVSEStatusRecord(KeyValuePair.Key, KeyValuePair.Value);

        #endregion

        #region ToKeyValuePair()

        /// <summary>
        /// Conversion this EVSE status record to a key-value-pair.
        /// </summary>
        public KeyValuePair<EVSE_Id, EVSEStatusTypes> ToKeyValuePair()

            => new KeyValuePair<EVSE_Id, EVSEStatusTypes>(Id, Status);

        #endregion


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

        #region Parse(EVSEStatusRecordXML)

        /// <summary>
        /// Parse the EVSE identification and its current status from the given OICP XML.
        /// </summary>
        /// <param name="EVSEStatusRecordXML">An OICP XML.</param>
        public static EVSEStatusRecord Parse(XElement EVSEStatusRecordXML)
        {

            try
            {

                if (EVSEStatusRecordXML.Name != OICPNS.EVSEStatus + "EvseStatusRecord")
                    throw new Exception("Illegal EVSEStatusRecord XML!");

                return new EVSEStatusRecord(
                    EVSE_Id.Parse(EVSEStatusRecordXML.ElementValueOrFail(OICPNS.EVSEStatus + "EvseId")),
                    (EVSEStatusTypes) Enum.Parse(typeof(EVSEStatusTypes), EVSEStatusRecordXML.ElementValueOrFail(OICPNS.EVSEStatus + "EvseStatus"))
                );

            }
            catch (Exception e)
            {
                return null;
            }

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return an OICP XML representation of this EVSE status record.
        /// </summary>
        /// <returns></returns>
        public XElement ToXML()

            => new XElement(OICPNS.EVSEStatus + "EvseStatusRecord",
                   new XElement(OICPNS.EVSEStatus + "EvseId",     Id.    OriginId),
                   new XElement(OICPNS.EVSEStatus + "EvseStatus", Status.ToString())
               );

        #endregion


    }

}
