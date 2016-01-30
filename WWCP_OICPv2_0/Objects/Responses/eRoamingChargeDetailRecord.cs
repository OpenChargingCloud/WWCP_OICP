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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// An OICP v2.0 charge detail record.
    /// </summary>
    public class eRoamingChargeDetailRecord : IEquatable <eRoamingChargeDetailRecord>,
                                              IComparable<eRoamingChargeDetailRecord>,
                                              IComparable
    {

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

        #region Identification

        private readonly AuthorizationIdentification _Identification;

        [Optional]
        public AuthorizationIdentification Identification
        {
            get
            {
                return _Identification;
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

        private readonly HubOperator_Id _HubOperatorId;

        [Optional]
        public HubOperator_Id HubOperatorId
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
        /// Create a charge detail record.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The charging session identification from the Authorize Start request.</param>
        /// <param name="PartnerProductId">An unqiue identification for the consumed charging product.</param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="Identification">An identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="ChargingStart">An optional charging start timestamp.</param>
        /// <param name="ChargingEnd">An optional charging end timestamp.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="ConsumedEnergy">The optional amount of consumed energy.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="HubOperatorId">An optional identification of the hub operator.</param>
        /// <param name="HubProviderId">An optional identification of the hub provider.</param>
        public eRoamingChargeDetailRecord(EVSE_Id                      EVSEId,
                                          ChargingSession_Id           SessionId,
                                          ChargingProduct_Id           PartnerProductId,
                                          DateTime                     SessionStart,
                                          DateTime                     SessionEnd,
                                          AuthorizationIdentification  Identification,
                                          ChargingSession_Id           PartnerSessionId      = null,
                                          DateTime?                    ChargingStart         = null,
                                          DateTime?                    ChargingEnd           = null,
                                          Double?                      MeterValueStart       = null,
                                          Double?                      MeterValueEnd         = null,
                                          IEnumerable<Double>          MeterValuesInBetween  = null,
                                          Double?                      ConsumedEnergy        = null,
                                          String                       MeteringSignature     = null,
                                          HubOperator_Id               HubOperatorId         = null,
                                          EVSP_Id                      HubProviderId         = null)

        {

            #region Initial checks

            if (EVSEId           == null)
                throw new ArgumentNullException(nameof(EVSEId),            "The given parameter must not be null!");

            if (SessionId        == null)
                throw new ArgumentNullException(nameof(SessionId),         "The given parameter must not be null!");

            if (PartnerProductId == null)
                throw new ArgumentNullException("PartnerProductId",  "The given parameter must not be null!");

            if (SessionStart     == null)
                throw new ArgumentNullException("SessionStart",      "The given parameter must not be null!");

            if (SessionEnd       == null)
                throw new ArgumentNullException("SessionEnd",        "The given parameter must not be null!");

            #endregion

            this._EVSEId                = EVSEId;
            this._SessionId             = SessionId;
            this._PartnerProductId      = PartnerProductId;
            this._SessionStart          = SessionStart;
            this._SessionEnd            = SessionEnd;
            this._Identification        = Identification;
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


        #region (static) Parse(eRoamingChargeDetailRecordXML, OnException = null)

        public static eRoamingChargeDetailRecord Parse(XElement             eRoamingChargeDetailRecordXML,
                                                       OnExceptionDelegate  OnException  = null)
        {

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
            // 
            //             <Authorization:SessionID>de164e08-1c88-1293-537b-be355041070e</Authorization:SessionID>
            // 
            //             <!--Optional:-->
            //             <Authorization:PartnerSessionID>0815</Authorization:PartnerSessionID>
            // 
            //             <!--Optional:-->
            //             <Authorization:PartnerProductID>AC1</Authorization:PartnerProductID>
            // 
            //             <Authorization:EvseID>DE*GEF*E123456789*1</Authorization:EvseID>
            // 
            //             <Authorization:Identification>
            //               <!--You have a CHOICE of the next 4 items at this level-->
            // 
            //               <CommonTypes:RFIDmifarefamilyIdentification>
            //                  <CommonTypes:UID>08152305</CommonTypes:UID>
            //               </CommonTypes:RFIDmifarefamilyIdentification>
            // 
            //               <CommonTypes:QRCodeIdentification>
            // 
            //                  <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
            // 
            //                  <!--You have a CHOICE of the next 2 items at this level-->
            //                  <CommonTypes:PIN>1234</CommonTypes:PIN>
            // 
            //                  <CommonTypes:HashedPIN>
            //                     <CommonTypes:Value>f7cf02826ba923e3d31c1c3015899076</CommonTypes:Value>
            //                     <CommonTypes:Function>MD5|SHA-1</CommonTypes:Function>
            //                     <CommonTypes:Salt>22c7c09370af2a3f07fe8665b140498a</CommonTypes:Salt>
            //                  </CommonTypes:HashedPIN>
            // 
            //               </CommonTypes:QRCodeIdentification>
            // 
            //               <CommonTypes:PlugAndChargeIdentification>
            //                  <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
            //               </CommonTypes:PlugAndChargeIdentification>
            // 
            //               <CommonTypes:RemoteIdentification>
            //                  <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
            //               </CommonTypes:RemoteIdentification>
            // 
            //             </Authorization:Identification>
            // 
            //             <!--Optional:-->
            //             <Authorization:ChargingStart>2015-10-23T15:45:30.000Z</Authorization:ChargingStart>
            //             <!--Optional:-->
            //             <Authorization:ChargingEnd>2015-10-23T16:59:31.000Z</Authorization:ChargingEnd>
            // 
            //             <Authorization:SessionStart>2015-10-23T15:45:00.000Z</Authorization:SessionStart>
            //             <Authorization:SessionEnd>2015-10-23T17:45:00.000Z</Authorization:SessionEnd>
            // 
            //             <!--Optional: \d\.\d{0,3} -->
            //             <Authorization:MeterValueStart>123.456</Authorization:MeterValueStart>
            //             <!--Optional: \d\.\d{0,3} -->
            //             <Authorization:MeterValueEnd>234.567</Authorization:MeterValueEnd>
            //             <!--Optional:-->
            //             <Authorization:MeterValueInBetween>
            //               <!--1 or more repetitions: \d\.\d{0,3} -->
            //               <Authorization:MeterValue>123.456</Authorization:MeterValue>
            //               <Authorization:MeterValue>189.768</Authorization:MeterValue>
            //               <Authorization:MeterValue>223.312</Authorization:MeterValue>
            //               <Authorization:MeterValue>234.560</Authorization:MeterValue>
            //               <Authorization:MeterValue>234.567</Authorization:MeterValue>
            //             </Authorization:MeterValueInBetween>
            // 
            //             <!--Optional:-->
            //             <Authorization:ConsumedEnergy>111.111</Authorization:ConsumedEnergy>
            //             <!--Optional:-->
            //             <Authorization:MeteringSignature>?</Authorization:MeteringSignature>
            // 
            //             <!--Optional:-->
            //             <Authorization:HubOperatorID>?</Authorization:HubOperatorID>
            //             <!--Optional:-->
            //             <Authorization:HubProviderID>?</Authorization:HubProviderID>
            // 
            //          </Authorization:eRoamingChargeDetailRecord>
            // 
            //       </Authorization:eRoamingChargeDetailRecords>
            //    </soapenv:Body>
            // 
            // </soapenv:Envelope>

            #endregion


            if (eRoamingChargeDetailRecordXML.Name != OICPNS.Authorization + "eRoamingChargeDetailRecord")
                throw new Exception("Invalid eRoamingChargeDetailRecord XML!");

            var Identification = eRoamingChargeDetailRecordXML.MapElementOrFail(OICPNS.Authorization + "Identification",
                                                                                "The Identification is invalid!",
                                                                                AuthorizationIdentification.Parse,
                                                                                OnException);


            return new eRoamingChargeDetailRecord(

                eRoamingChargeDetailRecordXML.MapValueOrFail       (OICPNS.Authorization + "EvseID",
                                                                    EVSE_Id.Parse,
                                                                    "The EvseID is invalid!"),

                eRoamingChargeDetailRecordXML.MapValueOrFail       (OICPNS.Authorization + "SessionID",
                                                                    ChargingSession_Id.Parse,
                                                                    "The SessionID is invalid!"),

                eRoamingChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "PartnerProductID",
                                                                    ChargingProduct_Id.Parse),

                eRoamingChargeDetailRecordXML.MapValueOrFail       (OICPNS.Authorization + "SessionStart",
                                                                    DateTime.Parse,
                                                                    "The SessionStart is invalid!"),

                eRoamingChargeDetailRecordXML.MapValueOrFail       (OICPNS.Authorization + "SessionEnd",
                                                                    DateTime.Parse,
                                                                    "The SessionStart is invalid!"),
                Identification,
                eRoamingChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "PartnerSessionID",
                                                                    ChargingSession_Id.Parse),

                eRoamingChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "ChargingStart",
                                                                    v => new Nullable<DateTime>(DateTime.Parse(v)),
                                                                    null),

                eRoamingChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "ChargingEnd",
                                                                    v => new Nullable<DateTime>(DateTime.Parse(v)),
                                                                    null),

                eRoamingChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "MeterValueStart",
                                                                    v => new Nullable<Double>(Double.Parse(v)),
                                                                    null),

                eRoamingChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "MeterValueEnd",
                                                                    v => new Nullable<Double>(Double.Parse(v)),
                                                                    null),

                eRoamingChargeDetailRecordXML.MapValues            (OICPNS.Authorization + "MeterValuesInBetween",
                                                                    OICPNS.Authorization + "MeterValue",
                                                                    Double.Parse),

                eRoamingChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "ConsumedEnergy",
                                                                    v => new Nullable<Double>(Double.Parse(v)),
                                                                    null),

                eRoamingChargeDetailRecordXML.ElementValueOrDefault(OICPNS.Authorization + "MeteringSignature"),

                eRoamingChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "HubOperatorID",
                                                                                            HubOperator_Id.Parse,
                                                                                            null),

                eRoamingChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "HubProviderID",
                                                                                            EVSP_Id.Parse,
                                                                                            null));

        }

        #endregion

        #region ToXML()

        public XElement ToXML()
        {

            return new XElement(OICPNS.Authorization + "eRoamingChargeDetailRecord",

                new XElement(OICPNS.Authorization + "SessionID",        SessionId.ToString()),
                new XElement(OICPNS.Authorization + "PartnerSessionID", (PartnerSessionId != null) ? PartnerSessionId.ToString() : ""),
                new XElement(OICPNS.Authorization + "PartnerProductID", (PartnerProductId != null) ? PartnerProductId.ToString() : ""),
                new XElement(OICPNS.Authorization + "EvseID",           EVSEId.OriginId),

                new XElement(OICPNS.Authorization + "Identification",
                    (_Identification.AuthToken != null)
                        ? new XElement(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification",
                               new XElement(OICPNS.CommonTypes + "UID", _Identification.AuthToken.ToString())
                          )
                        : new XElement(OICPNS.CommonTypes + "RemoteIdentification",
                               new XElement(OICPNS.CommonTypes + "EVCOID", _Identification.RemoteIdentification.ToString())
                          )
                ),

                (ChargingStart.  HasValue) ? new XElement(OICPNS.Authorization + "ChargingStart",    ChargingStart.  Value.ToIso8601()) : null,
                (ChargingEnd.    HasValue) ? new XElement(OICPNS.Authorization + "ChargingEnd",      ChargingEnd.    Value.ToIso8601()) : null,

                new XElement(OICPNS.Authorization + "SessionStart", SessionStart.ToIso8601()),
                new XElement(OICPNS.Authorization + "SessionEnd",   SessionEnd.  ToIso8601()),

                (MeterValueStart.HasValue) ? new XElement(OICPNS.Authorization + "MeterValueStart",  String.Format("{0:0.###}", MeterValueStart).Replace(",", ".")) : null,
                (MeterValueEnd.  HasValue) ? new XElement(OICPNS.Authorization + "MeterValueEnd",    String.Format("{0:0.###}", MeterValueEnd).  Replace(",", ".")) : null,

                MeterValuesInBetween != null
                    ? new XElement(OICPNS.Authorization + "MeterValueInBetween",
                          MeterValuesInBetween.
                              SafeSelect(value => new XElement(OICPNS.Authorization + "MeterValue", String.Format("{0:0.###}", value).Replace(",", "."))).
                              ToArray()
                      )
                    : null,

                ConsumedEnergy    != null ? new XElement(OICPNS.Authorization + "ConsumedEnergy",    String.Format("{0:0.}", ConsumedEnergy).Replace(",", ".")) : null,
                MeteringSignature != null ? new XElement(OICPNS.Authorization + "MeteringSignature", MeteringSignature)        : null,

                HubOperatorId     != null ? new XElement(OICPNS.Authorization + "HubOperatorID",     HubOperatorId.ToString()) : null,
                HubProviderId     != null ? new XElement(OICPNS.Authorization + "HubProviderID",     HubProviderId.ToString()) : null

            );

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
            var OICPChargeDetailRecord = Object as eRoamingChargeDetailRecord;
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
        public Int32 CompareTo(eRoamingChargeDetailRecord OICPChargeDetailRecord)
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
            var OICPChargeDetailRecord = Object as eRoamingChargeDetailRecord;
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
        public Boolean Equals(eRoamingChargeDetailRecord OICPChargeDetailRecord)
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

        #region (override) ToString()

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
