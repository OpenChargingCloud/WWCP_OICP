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
    /// An OICP charge detail record.
    /// </summary>
    public class ChargeDetailRecord : IEquatable <ChargeDetailRecord>,
                                      IComparable<ChargeDetailRecord>,
                                      IComparable
    {

        #region Properties

        /// <summary>
        /// An EVSE identification.
        /// </summary>
        [Mandatory]
        public EVSE_Id                      EVSEId                  { get; }

        /// <summary>
        /// A charging session identification.
        /// </summary>
        [Mandatory]
        public ChargingSession_Id           SessionId               { get; }

        /// <summary>
        /// The timestamp of the session start.
        /// </summary>
        [Mandatory]
        public DateTime                     SessionStart            { get; }

        /// <summary>
        /// The timestamp of the session end.
        /// </summary>
        [Mandatory]
        public DateTime                     SessionEnd              { get; }

        /// <summary>
        /// An identification.
        /// </summary>
        [Optional]
        public AuthorizationIdentification  Identification          { get; }

        /// <summary>
        /// An unqiue identification for the consumed charging product.
        /// </summary>
        [Mandatory]
        public ChargingProduct_Id           PartnerProductId        { get; }

        /// <summary>
        /// An optional partner session identification.
        /// </summary>
        [Optional]
        public ChargingSession_Id           PartnerSessionId        { get; }

        /// <summary>
        /// An optional charging start timestamp.
        /// </summary>
        [Optional]
        public DateTime?                    ChargingStart           { get; }

        /// <summary>
        /// An optional charging end timestamp.
        /// </summary>
        [Optional]
        public DateTime?                    ChargingEnd             { get; }

        /// <summary>
        /// An optional initial value of the energy meter.
        /// </summary>
        [Optional]
        public Double?                      MeterValueStart         { get; }

        /// <summary>
        /// An optional final value of the energy meter.
        /// </summary>
        [Optional]
        public Double?                      MeterValueEnd           { get; }

        /// <summary>
        /// An optional enumeration of meter values during the charging session.
        /// </summary>
        [Optional]
        public IEnumerable<Double>          MeterValuesInBetween    { get; }

        /// <summary>
        /// The optional amount of consumed energy.
        /// </summary>
        [Optional]
        public Double?                      ConsumedEnergy          { get; }

        /// <summary>
        /// An optional signature for the metering values.
        /// </summary>
        [Optional]
        public String                       MeteringSignature       { get; }

        /// <summary>
        /// An optional identification of the hub operator.
        /// </summary>
        [Optional]
        public HubOperator_Id               HubOperatorId           { get; }

        /// <summary>
        /// An optional identification of the hub provider.
        /// </summary>
        [Optional]
        public eMobilityProvider_Id         HubProviderId           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a charge detail record.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">A charging session identification.</param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="Identification">An identification.</param>
        /// <param name="PartnerProductId">An unqiue identification for the consumed charging product.</param>
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
        public ChargeDetailRecord(EVSE_Id                      EVSEId,
                                  ChargingSession_Id           SessionId,
                                  DateTime                     SessionStart,
                                  DateTime                     SessionEnd,
                                  AuthorizationIdentification  Identification,
                                  ChargingProduct_Id           PartnerProductId      = null,
                                  ChargingSession_Id           PartnerSessionId      = null,
                                  DateTime?                    ChargingStart         = null,
                                  DateTime?                    ChargingEnd           = null,
                                  Double?                      MeterValueStart       = null,
                                  Double?                      MeterValueEnd         = null,
                                  IEnumerable<Double>          MeterValuesInBetween  = null,
                                  Double?                      ConsumedEnergy        = null,
                                  String                       MeteringSignature     = null,
                                  HubOperator_Id               HubOperatorId         = null,
                                  eMobilityProvider_Id         HubProviderId         = null)

        {

            #region Initial checks

            if (EVSEId    == null)
                throw new ArgumentNullException(nameof(EVSEId),     "The given EVSE identification must not be null!");

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),  "The given session identification must not be null!");

            #endregion

            this.EVSEId                = EVSEId;
            this.SessionId             = SessionId;
            this.SessionStart          = SessionStart;
            this.SessionEnd            = SessionEnd;
            this.Identification        = Identification;
            this.PartnerProductId      = PartnerProductId;
            this.PartnerSessionId      = PartnerSessionId;
            this.ChargingStart         = ChargingStart;
            this.ChargingEnd           = ChargingEnd;
            this.MeterValueStart       = MeterValueStart;
            this.MeterValueEnd         = MeterValueEnd;
            this.MeterValuesInBetween  = MeterValuesInBetween;
            this.ConsumedEnergy        = ConsumedEnergy;
            this.MeteringSignature     = MeteringSignature;
            this.HubOperatorId         = HubOperatorId;
            this.HubProviderId         = HubProviderId;

        }

        #endregion


        #region Documentation

        // [...]
        // <Authorization:eRoamingChargeDetailRecord>
        // 
        //    <Authorization:SessionID>de164e08-1c88-1293-537b-be355041070e</Authorization:SessionID>
        // 
        //    <!--Optional:-->
        //    <Authorization:PartnerSessionID>0815</Authorization:PartnerSessionID>
        // 
        //    <!--Optional:-->
        //    <Authorization:PartnerProductID>AC1</Authorization:PartnerProductID>
        // 
        //    <Authorization:EvseID>DE*GEF*E123456789*1</Authorization:EvseID>
        // 
        //    <Authorization:Identification>
        //      <!--You have a CHOICE of the next 4 items at this level-->
        // 
        //      <CommonTypes:RFIDmifarefamilyIdentification>
        //         <CommonTypes:UID>08152305</CommonTypes:UID>
        //      </CommonTypes:RFIDmifarefamilyIdentification>
        // 
        //      <CommonTypes:QRCodeIdentification>
        // 
        //         <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        // 
        //         <!--You have a CHOICE of the next 2 items at this level-->
        //         <CommonTypes:PIN>1234</CommonTypes:PIN>
        // 
        //         <CommonTypes:HashedPIN>
        //            <CommonTypes:Value>f7cf02826ba923e3d31c1c3015899076</CommonTypes:Value>
        //            <CommonTypes:Function>MD5|SHA-1</CommonTypes:Function>
        //            <CommonTypes:Salt>22c7c09370af2a3f07fe8665b140498a</CommonTypes:Salt>
        //         </CommonTypes:HashedPIN>
        // 
        //      </CommonTypes:QRCodeIdentification>
        // 
        //      <CommonTypes:PlugAndChargeIdentification>
        //         <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //      </CommonTypes:PlugAndChargeIdentification>
        // 
        //      <CommonTypes:RemoteIdentification>
        //         <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //      </CommonTypes:RemoteIdentification>
        // 
        //    </Authorization:Identification>
        // 
        //    <!--Optional:-->
        //    <Authorization:ChargingStart>2015-10-23T15:45:30.000Z</Authorization:ChargingStart>
        //    <!--Optional:-->
        //    <Authorization:ChargingEnd>2015-10-23T16:59:31.000Z</Authorization:ChargingEnd>
        // 
        //    <Authorization:SessionStart>2015-10-23T15:45:00.000Z</Authorization:SessionStart>
        //    <Authorization:SessionEnd>2015-10-23T17:45:00.000Z</Authorization:SessionEnd>
        // 
        //    <!--Optional: \d\.\d{0,3} -->
        //    <Authorization:MeterValueStart>123.456</Authorization:MeterValueStart>
        //    <!--Optional: \d\.\d{0,3} -->
        //    <Authorization:MeterValueEnd>234.567</Authorization:MeterValueEnd>
        //    <!--Optional:-->
        //    <Authorization:MeterValueInBetween>
        //      <!--1 or more repetitions: \d\.\d{0,3} -->
        //      <Authorization:MeterValue>123.456</Authorization:MeterValue>
        //      <Authorization:MeterValue>189.768</Authorization:MeterValue>
        //      <Authorization:MeterValue>223.312</Authorization:MeterValue>
        //      <Authorization:MeterValue>234.560</Authorization:MeterValue>
        //      <Authorization:MeterValue>234.567</Authorization:MeterValue>
        //    </Authorization:MeterValueInBetween>
        // 
        //    <!--Optional:-->
        //    <Authorization:ConsumedEnergy>111.111</Authorization:ConsumedEnergy>
        //    <!--Optional:-->
        //    <Authorization:MeteringSignature>?</Authorization:MeteringSignature>
        // 
        //    <!--Optional:-->
        //    <Authorization:HubOperatorID>?</Authorization:HubOperatorID>
        //    <!--Optional:-->
        //    <Authorization:HubProviderID>?</Authorization:HubProviderID>
        // 
        // </Authorization:eRoamingChargeDetailRecord>
        // [...]

        #endregion

        #region (static) Parse(ChargeDetailRecordXML, OnException = null)

        /// <summary>
        /// Parse the givem XML as an OICP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecordXML">A XML representation of an OICP charge detail record.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ChargeDetailRecord Parse(XElement             ChargeDetailRecordXML,
                                               OnExceptionDelegate  OnException  = null)
        {

            if (ChargeDetailRecordXML.Name != OICPNS.Authorization + "eRoamingChargeDetailRecord")
                throw new Exception("Invalid eRoamingChargeDetailRecord XML!");

            var Identification = ChargeDetailRecordXML.MapElementOrFail(OICPNS.Authorization + "Identification",
                                                                                "The Identification is invalid!",
                                                                                AuthorizationIdentification.Parse,
                                                                                OnException);

            return new ChargeDetailRecord(

                ChargeDetailRecordXML.MapValueOrFail       (OICPNS.Authorization + "EvseID",
                                                            EVSE_Id.Parse,
                                                            "The EvseID is invalid!"),

                ChargeDetailRecordXML.MapValueOrFail       (OICPNS.Authorization + "SessionID",
                                                            ChargingSession_Id.Parse,
                                                            "The SessionID is invalid!"),

                ChargeDetailRecordXML.MapValueOrFail       (OICPNS.Authorization + "SessionStart",
                                                            DateTime.Parse,
                                                            "The SessionStart is invalid!"),

                ChargeDetailRecordXML.MapValueOrFail       (OICPNS.Authorization + "SessionEnd",
                                                            DateTime.Parse,
                                                            "The SessionStart is invalid!"),

                Identification,

                ChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "PartnerProductID",
                                                            ChargingProduct_Id.Parse),

                ChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "PartnerSessionID",
                                                            ChargingSession_Id.Parse),

                ChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "ChargingStart",
                                                            v => new DateTime?(DateTime.Parse(v)),
                                                            null),

                ChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "ChargingEnd",
                                                            v => new DateTime?(DateTime.Parse(v)),
                                                            null),

                ChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "MeterValueStart",
                                                            v => new Double?(Double.Parse(v)),
                                                            null),

                ChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "MeterValueEnd",
                                                            v => new Double?(Double.Parse(v)),
                                                            null),

                ChargeDetailRecordXML.MapValues            (OICPNS.Authorization + "MeterValuesInBetween",
                                                            OICPNS.Authorization + "MeterValue",
                                                            Double.Parse),

                ChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "ConsumedEnergy",
                                                            v => new Double?(Double.Parse(v)),
                                                            null),

                ChargeDetailRecordXML.ElementValueOrDefault(OICPNS.Authorization + "MeteringSignature"),

                ChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "HubOperatorID",
                                                            HubOperator_Id.Parse,
                                                            null),

                ChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "HubProviderID",
                                                            eMobilityProvider_Id.Parse,
                                                            null));

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OICPNS.Authorization + "eRoamingChargeDetailRecord",

                   new XElement(OICPNS.Authorization + "SessionID",        SessionId.ToString()),
                   PartnerSessionId != null ? new XElement(OICPNS.Authorization + "PartnerSessionID", PartnerSessionId.ToString()) : null,
                   PartnerProductId != null ? new XElement(OICPNS.Authorization + "PartnerProductID", PartnerProductId.ToString()) : null,
                   new XElement(OICPNS.Authorization + "EvseID",           EVSEId.OriginId),

                   Identification.ToXML(OICPNS.Authorization),

                   ChargingStart.HasValue ? new XElement(OICPNS.Authorization + "ChargingStart",    ChargingStart.  Value.ToIso8601()) : null,
                   ChargingEnd.  HasValue ? new XElement(OICPNS.Authorization + "ChargingEnd",      ChargingEnd.    Value.ToIso8601()) : null,

                   new XElement(OICPNS.Authorization + "SessionStart", SessionStart.ToIso8601()),
                   new XElement(OICPNS.Authorization + "SessionEnd",   SessionEnd.  ToIso8601()),

                   MeterValueStart.HasValue ? new XElement(OICPNS.Authorization + "MeterValueStart",  String.Format("{0:0.###}", MeterValueStart).Replace(",", ".")) : null,
                   MeterValueEnd.  HasValue ? new XElement(OICPNS.Authorization + "MeterValueEnd",    String.Format("{0:0.###}", MeterValueEnd).  Replace(",", ".")) : null,

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

        #endregion


        #region IComparable<ChargeDetailRecord> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            // Check if the given object is a charge detail record.
            var OICPChargeDetailRecord = Object as ChargeDetailRecord;
            if ((Object) OICPChargeDetailRecord == null)
                throw new ArgumentException("The given object is not a charge detail record!");

            return CompareTo(OICPChargeDetailRecord);

        }

        #endregion

        #region CompareTo(ChargeDetailRecord)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record object to compare with.</param>
        public Int32 CompareTo(ChargeDetailRecord ChargeDetailRecord)
        {

            if ((Object) ChargeDetailRecord == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord),  "The given charge detail record must not be null!");

            return SessionId.CompareTo(ChargeDetailRecord.SessionId);

        }

        #endregion

        #endregion

        #region IEquatable<ChargeDetailRecord> Members

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
            var OICPChargeDetailRecord = Object as ChargeDetailRecord;
            if ((Object) OICPChargeDetailRecord == null)
                return false;

            return this.Equals(OICPChargeDetailRecord);

        }

        #endregion

        #region Equals(ChargeDetailRecord)

        /// <summary>
        /// Compares two charge detail records for equality.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargeDetailRecord ChargeDetailRecord)
        {

            if ((Object) ChargeDetailRecord == null)
                return false;

            return SessionId.Equals(ChargeDetailRecord.SessionId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
            => SessionId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => SessionId.ToString();

        #endregion

    }

}
