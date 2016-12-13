﻿/*
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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An OICP charge detail record.
    /// </summary>
    public class ChargeDetailRecord : ACustomData,
                                      IEquatable <ChargeDetailRecord>,
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
        public Session_Id                   SessionId               { get; }

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
        public PartnerProduct_Id?           PartnerProductId        { get; }

        /// <summary>
        /// An optional partner session identification.
        /// </summary>
        [Optional]
        public PartnerSession_Id?           PartnerSessionId        { get; }

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
        public HubOperator_Id?              HubOperatorId           { get; }

        /// <summary>
        /// An optional identification of the hub provider.
        /// </summary>
        [Optional]
        public HubProvider_Id?              HubProviderId           { get; }

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
        /// <param name="CustomData">A dictionary of customer-specific data.</param>
        public ChargeDetailRecord(EVSE_Id                             EVSEId,
                                  Session_Id                          SessionId,
                                  DateTime                            SessionStart,
                                  DateTime                            SessionEnd,
                                  AuthorizationIdentification         Identification,
                                  PartnerProduct_Id?                  PartnerProductId       = null,
                                  PartnerSession_Id?                  PartnerSessionId       = null,
                                  DateTime?                           ChargingStart          = null,
                                  DateTime?                           ChargingEnd            = null,
                                  Double?                             MeterValueStart        = null,
                                  Double?                             MeterValueEnd          = null,
                                  IEnumerable<Double>                 MeterValuesInBetween   = null,
                                  Double?                             ConsumedEnergy         = null,
                                  String                              MeteringSignature      = null,
                                  HubOperator_Id?                     HubOperatorId          = null,
                                  HubProvider_Id?                     HubProviderId          = null,

                                  ReadOnlyDictionary<String, Object>  CustomData             = null)

            : base(CustomData)

        {

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

        #region (static) Parse   (ChargeDetailRecordXML,  CustomDataMapper = null, OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecordXML">The XML to parse.</param>
        /// <param name="CustomDataMapper">A delegate to parse custom xml elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ChargeDetailRecord Parse(XElement                                  ChargeDetailRecordXML,
                                               CustomMapperDelegate<ChargeDetailRecord>  CustomDataMapper  = null,
                                               OnExceptionDelegate                       OnException       = null)
        {

            ChargeDetailRecord _ChargeDetailRecord;

            if (TryParse(ChargeDetailRecordXML, out _ChargeDetailRecord, CustomDataMapper, OnException))
                return _ChargeDetailRecord;

            return null;

        }

        #endregion

        #region (static) Parse   (ChargeDetailRecordText, CustomDataMapper = null, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecordText">The text to parse.</param>
        /// <param name="CustomDataMapper">A delegate to parse custom xml elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ChargeDetailRecord Parse(String                                  ChargeDetailRecordText,
                                             CustomMapperDelegate<ChargeDetailRecord>  CustomDataMapper  = null,
                                             OnExceptionDelegate                     OnException       = null)
        {

            ChargeDetailRecord _ChargeDetailRecord;

            if (TryParse(ChargeDetailRecordText, out _ChargeDetailRecord, CustomDataMapper, OnException))
                return _ChargeDetailRecord;

            return null;

        }

        #endregion

        #region (static) TryParse(ChargeDetailRecordXML,  out ChargeDetailRecord, CustomDataMapper = null, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecordXML">The XML to parse.</param>
        /// <param name="ChargeDetailRecord">The parsed charge detail record.</param>
        /// <param name="CustomDataMapper">A delegate to parse custom xml elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                  ChargeDetailRecordXML,
                                       out ChargeDetailRecord                    ChargeDetailRecord,
                                       CustomMapperDelegate<ChargeDetailRecord>  CustomDataMapper  = null,
                                       OnExceptionDelegate                       OnException       = null)
        {

            try
            {

                if (ChargeDetailRecordXML.Name != OICPNS.Authorization + "eRoamingChargeDetailRecord")
                    throw new Exception("Invalid eRoamingChargeDetailRecord XML!");

                var Identification = ChargeDetailRecordXML.MapElementOrFail(OICPNS.Authorization + "Identification",
                                                                            AuthorizationIdentification.Parse,
                                                                            OnException);

                ChargeDetailRecord = new ChargeDetailRecord(

                                         ChargeDetailRecordXML.MapValueOrFail       (OICPNS.Authorization + "EvseID",
                                                                                     EVSE_Id.Parse),

                                         ChargeDetailRecordXML.MapValueOrFail       (OICPNS.Authorization + "SessionID",
                                                                                     Session_Id.Parse),

                                         ChargeDetailRecordXML.MapValueOrFail       (OICPNS.Authorization + "SessionStart",
                                                                                     DateTime.Parse),

                                         ChargeDetailRecordXML.MapValueOrFail       (OICPNS.Authorization + "SessionEnd",
                                                                                     DateTime.Parse),

                                         Identification,

                                         ChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "PartnerProductID",
                                                                                     PartnerProduct_Id.Parse),

                                         ChargeDetailRecordXML.MapValueOrDefault    (OICPNS.Authorization + "PartnerSessionID",
                                                                                     PartnerSession_Id.Parse),

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "ChargingStart",
                                                                                     DateTime.Parse),

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "ChargingEnd",
                                                                                     DateTime.Parse),

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "MeterValueStart",
                                                                                     Double.Parse),

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "MeterValueEnd",
                                                                                     Double.Parse),

                                         ChargeDetailRecordXML.MapValues            (OICPNS.Authorization + "MeterValuesInBetween",
                                                                                     OICPNS.Authorization + "MeterValue",
                                                                                     Double.Parse),

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "ConsumedEnergy",
                                                                                     Double.Parse),

                                         ChargeDetailRecordXML.ElementValueOrDefault(OICPNS.Authorization + "MeteringSignature"),

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "HubOperatorID",
                                                                                     HubOperator_Id.Parse),

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "HubProviderID",
                                                                                     HubProvider_Id.Parse)

                                     );

                if (CustomDataMapper != null)
                    ChargeDetailRecord = CustomDataMapper(ChargeDetailRecordXML, ChargeDetailRecord);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, ChargeDetailRecordXML, e);

                ChargeDetailRecord = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(ChargeDetailRecordText, out ChargeDetailRecord, CustomDataMapper = null, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecordText">The text to parse.</param>
        /// <param name="ChargeDetailRecord">The parsed charge detail record.</param>
        /// <param name="CustomDataMapper">A delegate to parse custom xml elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                    ChargeDetailRecordText,
                                       out ChargeDetailRecord                    ChargeDetailRecord,
                                       CustomMapperDelegate<ChargeDetailRecord>  CustomDataMapper  = null,
                                       OnExceptionDelegate                       OnException       = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(ChargeDetailRecordText).Root,
                             out ChargeDetailRecord,
                             CustomDataMapper,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, ChargeDetailRecordText, e);
            }

            ChargeDetailRecord = null;
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">The XML name to use.</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OICPNS.Authorization + "eRoamingChargeDetailRecord",

                   new XElement(OICPNS.Authorization + "SessionID",               SessionId.       ToString()),

                   PartnerSessionId.HasValue
                       ? new XElement(OICPNS.Authorization + "PartnerSessionID",  PartnerSessionId.ToString())
                       : null,

                   PartnerProductId.HasValue
                       ? new XElement(OICPNS.Authorization + "PartnerProductID",  PartnerProductId.ToString())
                       : null,

                   new XElement(OICPNS.Authorization + "EvseID",                  EVSEId.          ToString()),

                   Identification.ToXML(OICPNS.Authorization),

                   ChargingStart.HasValue
                       ? new XElement(OICPNS.Authorization + "ChargingStart",     ChargingStart.Value.ToIso8601())
                       : null,

                   ChargingEnd.HasValue
                       ? new XElement(OICPNS.Authorization + "ChargingEnd",       ChargingEnd.  Value.ToIso8601())
                       : null,

                   new XElement(OICPNS.Authorization + "SessionStart", SessionStart.ToIso8601()),
                   new XElement(OICPNS.Authorization + "SessionEnd",   SessionEnd.  ToIso8601()),

                   MeterValueStart.HasValue
                       ? new XElement(OICPNS.Authorization + "MeterValueStart",  String.Format("{0:0.###}", MeterValueStart).Replace(",", "."))
                       : null,

                   MeterValueEnd.HasValue
                       ? new XElement(OICPNS.Authorization + "MeterValueEnd",    String.Format("{0:0.###}", MeterValueEnd).  Replace(",", "."))
                       : null,

                   MeterValuesInBetween != null
                       ? new XElement(OICPNS.Authorization + "MeterValueInBetween",
                             MeterValuesInBetween.
                                 SafeSelect(value => new XElement(OICPNS.Authorization + "MeterValue", String.Format("{0:0.###}", value).Replace(",", "."))).
                                 ToArray()
                         )
                       : null,

                   ConsumedEnergy.HasValue
                       ? new XElement(OICPNS.Authorization + "ConsumedEnergy",    String.Format("{0:0.}", ConsumedEnergy).Replace(",", "."))
                       : null,

                   MeteringSignature != null
                       ? new XElement(OICPNS.Authorization + "MeteringSignature", MeteringSignature)
                       : null,

                   HubOperatorId.HasValue
                       ? new XElement(OICPNS.Authorization + "HubOperatorID",     HubOperatorId.ToString())
                       : null,

                   HubProviderId.HasValue
                       ? new XElement(OICPNS.Authorization + "HubProviderID",     HubProviderId.ToString())
                       : null

            );

        #endregion


        #region Operator overloading

        #region Operator == (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargeDetailRecord ChargeDetailRecord1, ChargeDetailRecord ChargeDetailRecord2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargeDetailRecord1, ChargeDetailRecord2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargeDetailRecord1 == null) || ((Object) ChargeDetailRecord2 == null))
                return false;

            return ChargeDetailRecord1.Equals(ChargeDetailRecord2);

        }

        #endregion

        #region Operator != (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargeDetailRecord ChargeDetailRecord1, ChargeDetailRecord ChargeDetailRecord2)
            => !(ChargeDetailRecord1 == ChargeDetailRecord2);

        #endregion

        #region Operator <  (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargeDetailRecord ChargeDetailRecord1, ChargeDetailRecord ChargeDetailRecord2)
        {

            if ((Object) ChargeDetailRecord1 == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord1), "The given ChargeDetailRecord1 must not be null!");

            return ChargeDetailRecord1.CompareTo(ChargeDetailRecord2) < 0;

        }

        #endregion

        #region Operator <= (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargeDetailRecord ChargeDetailRecord1, ChargeDetailRecord ChargeDetailRecord2)
            => !(ChargeDetailRecord1 > ChargeDetailRecord2);

        #endregion

        #region Operator >  (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargeDetailRecord ChargeDetailRecord1, ChargeDetailRecord ChargeDetailRecord2)
        {

            if ((Object) ChargeDetailRecord1 == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord1), "The given ChargeDetailRecord1 must not be null!");

            return ChargeDetailRecord1.CompareTo(ChargeDetailRecord2) > 0;

        }

        #endregion

        #region Operator >= (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargeDetailRecord ChargeDetailRecord1, ChargeDetailRecord ChargeDetailRecord2)
            => !(ChargeDetailRecord1 < ChargeDetailRecord2);

        #endregion

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
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var OICPChargeDetailRecord = Object as ChargeDetailRecord;
            if ((Object) OICPChargeDetailRecord == null)
                throw new ArgumentException("The given object is not a charge detail record!", nameof(Object));

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

            => String.Concat(SessionId,

                             ConsumedEnergy.HasValue
                                 ? ConsumedEnergy.Value + " kWh"
                                 : null,

                             PartnerProductId.HasValue
                                 ? " of " + PartnerProductId.Value
                                 : null,

                             " at ",  EVSEId,
                             " for ", Identification,

                             ", " + SessionStart.ToIso8601(), " -> ", SessionEnd.ToIso8601());

        #endregion


    }

}