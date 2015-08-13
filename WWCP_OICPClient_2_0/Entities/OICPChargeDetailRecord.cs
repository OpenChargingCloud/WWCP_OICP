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
using System.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// An OICP v2.0 charge detail record for a charging session.
    /// </summary>
    public class OICPChargeDetailRecord : IEquatable <OICPChargeDetailRecord>,
                                          IComparable<OICPChargeDetailRecord>,
                                          IComparable
    {

        #region Data

        /// <summary>
        /// The default max size of the charging station (aggregated EVSE) status history.
        /// </summary>
        public const UInt16 DefaultStationStatusHistorySize = 50;

        #endregion

        #region Properties

        #region EVSEId

        private readonly EVSE_Id _EVSEId;

        [Mandatory]
        public EVSE_Id EVSEId
        {
            get
            {
                return _EVSEId;
            }
        }

        #endregion

        #region SessionId

        private readonly ChargingSession_Id _SessionId;

        /// <summary>
        /// The charging session identification from the Authorize Start request.
        /// </summary>
        [Mandatory]
        public ChargingSession_Id SessionId
        {
            get
            {
                return _SessionId;
            }
        }

        #endregion

        #region PartnerProductId

        private readonly ChargingProduct_Id   _PartnerProductId;

        [Mandatory]
        public ChargingProduct_Id PartnerProductId
        {
            get
            {
                return _PartnerProductId;
            }
        }

        #endregion

        #region SessionStart

        private readonly DateTime _SessionStart;

        [Mandatory]
        public DateTime SessionStart
        {
            get
            {
                return _SessionStart;
            }
        }

        #endregion

        #region SessionEnd

        private readonly DateTime _SessionEnd;

        [Mandatory]
        public DateTime SessionEnd
        {
            get
            {
                return _SessionEnd;
            }
        }

        #endregion

        #region AuthToken

        private readonly Auth_Token _AuthToken;

        [Optional]
        public Auth_Token AuthToken
        {
            get
            {
                return _AuthToken;
            }
        }

        #endregion

        #region eMAId

        private readonly eMA_Id _eMAId;

        [Optional]
        public eMA_Id eMAId
        {
            get
            {
                return _eMAId;
            }
        }

        #endregion

        #region PartnerSessionId

        private readonly ChargingSession_Id _PartnerSessionId;

        [Optional]
        public ChargingSession_Id PartnerSessionId
        {
            get
            {
                return _PartnerSessionId;
            }
        }

        #endregion

        #region ChargingStart

        private readonly DateTime? _ChargingStart;

        [Optional]
        public DateTime? ChargingStart
        {
            get
            {
                return _ChargingStart;
            }
        }

        #endregion

        #region ChargingEnd

        private readonly DateTime? _ChargingEnd;

        [Optional]
        public DateTime? ChargingEnd
        {
            get
            {
                return _ChargingEnd;
            }
        }

        #endregion

        #region MeterValueStart

        private readonly Double? _MeterValueStart;

        [Optional]
        public Double? MeterValueStart
        {
            get
            {
                return _MeterValueStart;
            }
        }

        #endregion

        #region MeterValueEnd

        private readonly Double? _MeterValueEnd;

        [Optional]
        public Double? MeterValueEnd
        {
            get
            {
                return _MeterValueEnd;
            }
        }

        #endregion

        #region MeterValuesInBetween

        private readonly IEnumerable<Double>  _MeterValuesInBetween;

        [Optional]
        public IEnumerable<Double> MeterValuesInBetween
        {
            get
            {
                return _MeterValuesInBetween;
            }
        }

        #endregion

        #region ConsumedEnergy

        private readonly Double? _ConsumedEnergy;

        [Optional]
        public Double? ConsumedEnergy
        {
            get
            {
                return _ConsumedEnergy;
            }
        }

        #endregion

        #region MeteringSignature

        private readonly String _MeteringSignature;

        [Optional]
        public String MeteringSignature
        {
            get
            {
                return _MeteringSignature;
            }
        }

        #endregion

        #region HubOperatorId

        private readonly EVSEOperator_Id _HubOperatorId;

        [Optional]
        public EVSEOperator_Id HubOperatorId
        {
            get
            {
                return _HubOperatorId;
            }
        }

        #endregion

        #region HubProviderId

        private readonly EVSP_Id _HubProviderId;

        [Optional]
        public EVSP_Id HubProviderId
        {
            get
            {
                return _HubProviderId;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a charge detail record for the given charging session (identification).
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The charging session identification from the Authorize Start request.</param>
        /// <param name="PartnerProductId">An unqiue identification for the consumed charging product.</param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="AuthToken">An optional (RFID) user identification.</param>
        /// <param name="eMAId">An optional e-Mobility account identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="ChargingTime">Optional timestamps of the charging start/stop.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="ConsumedEnergy">The optional amount of consumed energy.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="HubOperatorId">An optional identification of the hub operator.</param>
        /// <param name="HubProviderId">An optional identification of the hub provider.</param>
        public OICPChargeDetailRecord(EVSE_Id              EVSEId,
                                      ChargingSession_Id   SessionId,
                                      ChargingProduct_Id   PartnerProductId,
                                      DateTime             SessionStart,
                                      DateTime             SessionEnd,
                                      Auth_Token           AuthToken             = null,
                                      eMA_Id               eMAId                 = null,
                                      ChargingSession_Id   PartnerSessionId      = null,
                                      DateTime?            ChargingStart         = null,
                                      DateTime?            ChargingEnd           = null,
                                      Double?              MeterValueStart       = null,
                                      Double?              MeterValueEnd         = null,
                                      IEnumerable<Double>  MeterValuesInBetween  = null,
                                      Double?              ConsumedEnergy        = null,
                                      String               MeteringSignature     = null,
                                      EVSEOperator_Id      HubOperatorId         = null,
                                      EVSP_Id              HubProviderId         = null)

        {

            #region Initial checks

            if (EVSEId           == null)
                throw new ArgumentNullException("EVSEId",            "The given parameter must not be null!");

            if (SessionId        == null)
                throw new ArgumentNullException("SessionId",         "The given parameter must not be null!");

            if (PartnerProductId == null)
                throw new ArgumentNullException("PartnerProductId",  "The given parameter must not be null!");

            if (SessionStart     == null)
                throw new ArgumentNullException("SessionStart",      "The given parameter must not be null!");

            if (SessionEnd       == null)
                throw new ArgumentNullException("SessionEnd",        "The given parameter must not be null!");

            if (AuthToken        == null &&
                eMAId            == null)
                throw new ArgumentNullException("AuthToken / eMAId", "At least one of the given parameters must not be null!");

            #endregion

            this._EVSEId                = EVSEId;
            this._SessionId             = SessionId;
            this._PartnerProductId      = PartnerProductId;
            this._SessionStart          = SessionStart;
            this._SessionEnd            = SessionEnd;
            this._AuthToken             = AuthToken;
            this._eMAId                 = eMAId;
            this._PartnerSessionId      = PartnerSessionId;
            this._ChargingStart         = ChargingStart;
            this._ChargingEnd           = ChargingEnd;
            this._MeterValueStart       = MeterValueStart;
            this._MeterValueEnd         = MeterValueEnd;
            this._MeterValuesInBetween  = MeterValuesInBetween;
            this._ConsumedEnergy        = ConsumedEnergy;
            this._MeteringSignature     = MeteringSignature;
            this._HubOperatorId         = HubOperatorId;
            this._HubProviderId         = HubProviderId;

        }

        #endregion


        #region IComparable<OICPChargeDetailRecord> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a charge detail record.
            var OICPChargeDetailRecord = Object as OICPChargeDetailRecord;
            if ((Object) OICPChargeDetailRecord == null)
                throw new ArgumentException("The given object is not a charge detail record!");

            return CompareTo(OICPChargeDetailRecord);

        }

        #endregion

        #region CompareTo(OICPChargeDetailRecord)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OICPChargeDetailRecord">A charge detail record object to compare with.</param>
        public Int32 CompareTo(OICPChargeDetailRecord OICPChargeDetailRecord)
        {

            if ((Object) OICPChargeDetailRecord == null)
                throw new ArgumentNullException("The given charge detail record must not be null!");

            return SessionId.CompareTo(OICPChargeDetailRecord.SessionId);

        }

        #endregion

        #endregion

        #region IEquatable<OICPChargeDetailRecord> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is a charge detail record.
            var OICPChargeDetailRecord = Object as OICPChargeDetailRecord;
            if ((Object) OICPChargeDetailRecord == null)
                return false;

            return this.Equals(OICPChargeDetailRecord);

        }

        #endregion

        #region Equals(OICPChargeDetailRecord)

        /// <summary>
        /// Compares two charge detail records for equality.
        /// </summary>
        /// <param name="OICPChargeDetailRecord">A charge detail record to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(OICPChargeDetailRecord OICPChargeDetailRecord)
        {

            if ((Object) OICPChargeDetailRecord == null)
                return false;

            return SessionId.Equals(OICPChargeDetailRecord.SessionId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            return SessionId.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return SessionId.ToString();
        }

        #endregion

    }

}
