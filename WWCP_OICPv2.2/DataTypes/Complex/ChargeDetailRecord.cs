/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
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
        public Identification               Identification          { get; }

        /// <summary>
        /// An unqiue identification for the consumed charging product.
        /// </summary>
        [Mandatory]
        public PartnerProduct_Id?           PartnerProductId        { get; }

        /// <summary>
        /// An optional CPO partner session identification.
        /// </summary>
        [Optional]
        public CPOPartnerSession_Id?        CPOPartnerSessionId     { get; }

        /// <summary>
        /// An optional EMP partner session identification.
        /// </summary>
        [Optional]
        public EMPPartnerSession_Id?        EMPPartnerSessionId     { get; }

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
        public Decimal?                     MeterValueStart         { get; }

        /// <summary>
        /// An optional final value of the energy meter.
        /// </summary>
        [Optional]
        public Decimal?                     MeterValueEnd           { get; }

        /// <summary>
        /// An optional enumeration of meter values during the charging session.
        /// </summary>
        [Optional]
        public IEnumerable<Decimal>         MeterValuesInBetween    { get; }

        /// <summary>
        /// The optional amount of consumed energy.
        /// </summary>
        [Optional]
        public Decimal?                     ConsumedEnergy          { get; }

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
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ChargingStart">An optional charging start timestamp.</param>
        /// <param name="ChargingEnd">An optional charging end timestamp.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="ConsumedEnergy">The optional amount of consumed energy.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="HubOperatorId">An optional identification of the hub operator.</param>
        /// <param name="HubProviderId">An optional identification of the hub provider.</param>
        /// 
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargeDetailRecord(EVSE_Id                              EVSEId,
                                  Session_Id                           SessionId,
                                  DateTime                             SessionStart,
                                  DateTime                             SessionEnd,
                                  Identification                       Identification,
                                  PartnerProduct_Id?                   PartnerProductId       = null,
                                  CPOPartnerSession_Id?                CPOPartnerSessionId    = null,
                                  EMPPartnerSession_Id?                EMPPartnerSessionId    = null,
                                  DateTime?                            ChargingStart          = null,
                                  DateTime?                            ChargingEnd            = null,
                                  Decimal?                             MeterValueStart        = null,  // xx.yyy
                                  Decimal?                             MeterValueEnd          = null,  // xx.yyy
                                  IEnumerable<Decimal>                 MeterValuesInBetween   = null,  // xx.yyy
                                  Decimal?                             ConsumedEnergy         = null,  // xx.yyy
                                  String                               MeteringSignature      = null,  // maxlength: 200
                                  HubOperator_Id?                      HubOperatorId          = null,
                                  HubProvider_Id?                      HubProviderId          = null,

                                  IReadOnlyDictionary<String, Object>  CustomData             = null)

            : base(CustomData)

        {

            this.EVSEId                = EVSEId;
            this.SessionId             = SessionId;
            this.SessionStart          = SessionStart;
            this.SessionEnd            = SessionEnd;
            this.Identification        = Identification;
            this.PartnerProductId      = PartnerProductId;
            this.CPOPartnerSessionId   = CPOPartnerSessionId;
            this.EMPPartnerSessionId   = EMPPartnerSessionId;
            this.ChargingStart         = ChargingStart;
            this.ChargingEnd           = ChargingEnd;
            this.MeterValueStart       = MeterValueStart.HasValue ? (Decimal) Math.Round(MeterValueStart.Value, 3) : new Decimal?();
            this.MeterValueEnd         = MeterValueEnd.  HasValue ? (Decimal) Math.Round(MeterValueEnd.  Value, 3) : new Decimal?();
            this.MeterValuesInBetween  = MeterValuesInBetween.SafeAny() ? MeterValuesInBetween.Select(metervalue => (Decimal) Math.Round(metervalue, 3)) : null;
            this.ConsumedEnergy        = ConsumedEnergy. HasValue ? (Decimal) Math.Round(ConsumedEnergy. Value, 3) : new Decimal?();
            this.MeteringSignature     = MeteringSignature.SubstringMax(200);
            this.HubOperatorId         = HubOperatorId;
            this.HubProviderId         = HubProviderId;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0"
        //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        // [...]
        //
        //    <Authorization:eRoamingChargeDetailRecord>
        //
        //       <Authorization:SessionID>de164e08-1c88-1293-537b-be355041070e</Authorization:SessionID>
        //
        //       <!--Optional:-->
        //       <Authorization:CPOPartnerSessionID>?</Authorization:CPOPartnerSessionID>
        //
        //       <!--Optional:-->
        //       <Authorization:EMPPartnerSessionID>?</Authorization:EMPPartnerSessionID>
        //
        //       <!--Optional:-->
        //       <Authorization:PartnerProductID>AC1</Authorization:PartnerProductID>
        //
        //       <Authorization:EvseID>DE*GEF*E123456789*1</Authorization:EvseID>
        //
        //       <Authorization:Identification>
        //         <!--You have a CHOICE of the next 4 items at this level-->
        //
        //         <CommonTypes:RFIDmifarefamilyIdentification>
        //            <CommonTypes:UID>08152305</CommonTypes:UID>
        //         </CommonTypes:RFIDmifarefamilyIdentification>
        //
        //         <CommonTypes:QRCodeIdentification>
        //
        //            <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //
        //            <!--You have a CHOICE of the next 2 items at this level-->
        //            <CommonTypes:PIN>1234</CommonTypes:PIN>
        //
        //            <CommonTypes:HashedPIN>
        //               <CommonTypes:Value>f7cf02826ba923e3d31c1c3015899076</CommonTypes:Value>
        //               <CommonTypes:Function>MD5|SHA-1</CommonTypes:Function>
        //               <CommonTypes:Salt>22c7c09370af2a3f07fe8665b140498a</CommonTypes:Salt>
        //            </CommonTypes:HashedPIN>
        //
        //         </CommonTypes:QRCodeIdentification>
        //
        //         <CommonTypes:PlugAndChargeIdentification>
        //            <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //         </CommonTypes:PlugAndChargeIdentification>
        //
        //         <CommonTypes:RemoteIdentification>
        //            <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //         </CommonTypes:RemoteIdentification>
        //
        //       </Authorization:Identification>
        //
        //       <!--Optional:-->
        //       <Authorization:ChargingStart>2015-10-23T15:45:30.000Z</Authorization:ChargingStart>
        //       <!--Optional:-->
        //       <Authorization:ChargingEnd>2015-10-23T16:59:31.000Z</Authorization:ChargingEnd>
        //
        //       <Authorization:SessionStart>2015-10-23T15:45:00.000Z</Authorization:SessionStart>
        //       <Authorization:SessionEnd>2015-10-23T17:45:00.000Z</Authorization:SessionEnd>
        //
        //       <!--Optional: \d\.\d{0,3} -->
        //       <Authorization:MeterValueStart>123.456</Authorization:MeterValueStart>
        //       <!--Optional: \d\.\d{0,3} -->
        //       <Authorization:MeterValueEnd>234.567</Authorization:MeterValueEnd>
        //       <!--Optional:-->
        //       <Authorization:MeterValueInBetween>
        //         <!--1 or more repetitions: \d\.\d{0,3} -->
        //         <Authorization:MeterValue>123.456</Authorization:MeterValue>
        //         <Authorization:MeterValue>189.768</Authorization:MeterValue>
        //         <Authorization:MeterValue>223.312</Authorization:MeterValue>
        //         <Authorization:MeterValue>234.560</Authorization:MeterValue>
        //         <Authorization:MeterValue>234.567</Authorization:MeterValue>
        //       </Authorization:MeterValueInBetween>
        //
        //       <!--Optional:-->
        //       <Authorization:ConsumedEnergy>111.111</Authorization:ConsumedEnergy>
        //       <!--Optional:-->
        //       <Authorization:MeteringSignature>?</Authorization:MeteringSignature>
        //
        //       <!--Optional:-->
        //       <Authorization:HubOperatorID>?</Authorization:HubOperatorID>
        //       <!--Optional:-->
        //       <Authorization:HubProviderID>?</Authorization:HubProviderID>
        //
        //    </Authorization:eRoamingChargeDetailRecord>
        //
        // [...]
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (ChargeDetailRecordXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecordXML">The XML to parse.</param>
        /// <param name="CustomChargeDetailRecordParser">A delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ChargeDetailRecord Parse(XElement                                     ChargeDetailRecordXML,
                                               CustomXMLParserDelegate<ChargeDetailRecord>  CustomChargeDetailRecordParser   = null,
                                               CustomXMLParserDelegate<Identification>      CustomIdentificationParser       = null,
                                               CustomXMLParserDelegate<RFIDIdentification>  CustomRFIDIdentificationParser   = null,
                                               OnExceptionDelegate                          OnException                      = null)
        {

            if (TryParse(ChargeDetailRecordXML,
                         out ChargeDetailRecord _ChargeDetailRecord,
                         CustomChargeDetailRecordParser,
                         CustomIdentificationParser,
                         CustomRFIDIdentificationParser,
                         OnException))

                return _ChargeDetailRecord;

            return null;

        }

        #endregion

        #region (static) Parse   (ChargeDetailRecordText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecordText">The text to parse.</param>
        /// <param name="CustomChargeDetailRecordParser">A delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ChargeDetailRecord Parse(String                                       ChargeDetailRecordText,
                                               CustomXMLParserDelegate<ChargeDetailRecord>  CustomChargeDetailRecordParser   = null,
                                               CustomXMLParserDelegate<Identification>      CustomIdentificationParser       = null,
                                               CustomXMLParserDelegate<RFIDIdentification>  CustomRFIDIdentificationParser   = null,
                                               OnExceptionDelegate                          OnException                      = null)
        {

            if (TryParse(ChargeDetailRecordText,
                         out ChargeDetailRecord _ChargeDetailRecord,
                         CustomChargeDetailRecordParser,
                         CustomIdentificationParser,
                         CustomRFIDIdentificationParser,
                         OnException))

                return _ChargeDetailRecord;

            return null;

        }

        #endregion

        #region (static) TryParse(ChargeDetailRecordXML,  out ChargeDetailRecord, ..., OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecordXML">The XML to parse.</param>
        /// <param name="ChargeDetailRecord">The parsed charge detail record.</param>
        /// <param name="CustomChargeDetailRecordParser">A delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                     ChargeDetailRecordXML,
                                       out ChargeDetailRecord                       ChargeDetailRecord,
                                       CustomXMLParserDelegate<ChargeDetailRecord>  CustomChargeDetailRecordParser   = null,
                                       CustomXMLParserDelegate<Identification>      CustomIdentificationParser       = null,
                                       CustomXMLParserDelegate<RFIDIdentification>  CustomRFIDIdentificationParser   = null,
                                       OnExceptionDelegate                          OnException                      = null)
        {

            try
            {

                if (ChargeDetailRecordXML.Name != OICPNS.Authorization + "eRoamingChargeDetailRecord")
                    throw new Exception("Invalid eRoamingChargeDetailRecord XML!");

                var Identification = ChargeDetailRecordXML.MapElementOrFail(OICPNS.Authorization + "Identification",
                                                                            (xml, e) => OICPv2_2.Identification.Parse(xml,
                                                                                                                      CustomIdentificationParser,
                                                                                                                      CustomRFIDIdentificationParser,
                                                                                                                      e),
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

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "PartnerProductID",
                                                                                     PartnerProduct_Id.Parse),

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "CPOPartnerSessionID",
                                                                                     CPOPartnerSession_Id.Parse),

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "EMPPartnerSessionID",
                                                                                     EMPPartnerSession_Id.Parse),

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "ChargingStart",
                                                                                     DateTime.Parse),

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "ChargingEnd",
                                                                                     DateTime.Parse),

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "MeterValueStart",
                                                                                     Decimal.Parse),

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "MeterValueEnd",
                                                                                     Decimal.Parse),

                                         ChargeDetailRecordXML.MapValues            (OICPNS.Authorization + "MeterValuesInBetween",
                                                                                     OICPNS.Authorization + "MeterValue",
                                                                                     Decimal.Parse),

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "ConsumedEnergy",
                                                                                     Decimal.Parse),

                                         ChargeDetailRecordXML.ElementValueOrDefault(OICPNS.Authorization + "MeteringSignature"),

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "HubOperatorID",
                                                                                     HubOperator_Id.Parse),

                                         ChargeDetailRecordXML.MapValueOrNullable   (OICPNS.Authorization + "HubProviderID",
                                                                                     HubProvider_Id.Parse)

                                     );

                if (CustomChargeDetailRecordParser != null)
                    ChargeDetailRecord = CustomChargeDetailRecordParser(ChargeDetailRecordXML, ChargeDetailRecord);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, ChargeDetailRecordXML, e);

                ChargeDetailRecord = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(ChargeDetailRecordText, out ChargeDetailRecord, ..., OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecordText">The text to parse.</param>
        /// <param name="ChargeDetailRecord">The parsed charge detail record.</param>
        /// <param name="CustomChargeDetailRecordParser">A delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                       ChargeDetailRecordText,
                                       out ChargeDetailRecord                       ChargeDetailRecord,
                                       CustomXMLParserDelegate<ChargeDetailRecord>  CustomChargeDetailRecordParser   = null,
                                       CustomXMLParserDelegate<Identification>      CustomIdentificationParser       = null,
                                       CustomXMLParserDelegate<RFIDIdentification>  CustomRFIDIdentificationParser   = null,
                                       OnExceptionDelegate                          OnException                      = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(ChargeDetailRecordText).Root,
                             out ChargeDetailRecord,
                             CustomChargeDetailRecordParser,
                             CustomIdentificationParser,
                             CustomRFIDIdentificationParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, ChargeDetailRecordText, e);
            }

            ChargeDetailRecord = null;
            return false;

        }

        #endregion

        #region ToXML(XName = null, CustomChargeDetailRecordSerializer = null, CustomIdentificationSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">The XML name to use.</param>
        /// <param name="CustomChargeDetailRecordSerializer">A delegate to serialize custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification XML elements.</param>
        public XElement ToXML(XName                                            XName                                = null,
                              CustomXMLSerializerDelegate<ChargeDetailRecord>  CustomChargeDetailRecordSerializer   = null,
                              CustomXMLSerializerDelegate<Identification>      CustomIdentificationSerializer       = null)

        {

            var XML = new XElement(XName ?? OICPNS.Authorization + "eRoamingChargeDetailRecord",

                          new XElement(OICPNS.Authorization + "SessionID",                  SessionId.          ToString()),

                          CPOPartnerSessionId.HasValue
                              ? new XElement(OICPNS.Authorization + "CPOPartnerSessionID",  CPOPartnerSessionId.ToString())
                              : null,

                          EMPPartnerSessionId.HasValue
                              ? new XElement(OICPNS.Authorization + "EMPPartnerSessionID",  EMPPartnerSessionId.ToString())
                              : null,

                          PartnerProductId.HasValue
                              ? new XElement(OICPNS.Authorization + "PartnerProductID",     PartnerProductId.   ToString())
                              : null,

                          new XElement(OICPNS.Authorization + "EvseID",                     EVSEId.             ToString()),

                          Identification.ToXML(CustomIdentificationSerializer: CustomIdentificationSerializer),

                          ChargingStart.HasValue
                              ? new XElement(OICPNS.Authorization + "ChargingStart",        ChargingStart.Value.ToIso8601())
                              : null,

                          ChargingEnd.HasValue
                              ? new XElement(OICPNS.Authorization + "ChargingEnd",          ChargingEnd.  Value.ToIso8601())
                              : null,

                          new XElement(OICPNS.Authorization + "SessionStart",               SessionStart.       ToIso8601()),
                          new XElement(OICPNS.Authorization + "SessionEnd",                 SessionEnd.         ToIso8601()),

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
                              ? new XElement(OICPNS.Authorization + "ConsumedEnergy",    String.Format("{0:0.###}", ConsumedEnergy).Replace(",", "."))
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

            return CustomChargeDetailRecordSerializer != null
                       ? CustomChargeDetailRecordSerializer(this, XML)
                       : XML;

        }

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
            if (ReferenceEquals(ChargeDetailRecord1, ChargeDetailRecord2))
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

            if (!(Object is ChargeDetailRecord ChargeDetailRecord))
                throw new ArgumentException("The given object is not a charge detail record!", nameof(Object));

            return CompareTo(ChargeDetailRecord);

        }

        #endregion

        #region CompareTo(ChargeDetailRecord)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record object to compare with.</param>
        public Int32 CompareTo(ChargeDetailRecord ChargeDetailRecord)
        {

            if (ChargeDetailRecord is null)
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

            if (!(Object is ChargeDetailRecord ChargeDetailRecord))
                return false;

            return Equals(ChargeDetailRecord);

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

            if (ChargeDetailRecord is null)
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
        /// Return a text representation of this object.
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


        #region ToBuilder(NewEVSEId = null)

        /// <summary>
        /// Return a builder for this charge detail record.
        /// </summary>
        public Builder ToBuilder()

            => new Builder(EVSEId,
                           SessionId,
                           SessionStart,
                           SessionEnd,
                           Identification,
                           PartnerProductId,
                           CPOPartnerSessionId,
                           EMPPartnerSessionId,
                           ChargingStart,
                           ChargingEnd,
                           MeterValueStart,
                           MeterValueEnd,
                           MeterValuesInBetween,
                           ConsumedEnergy,
                           MeteringSignature,
                           HubOperatorId,
                           HubProviderId,
                           CustomData);

        #endregion

        #region (class) Builder

        /// <summary>
        /// An OICP charge detail record.
        /// </summary>
        public new class Builder : ACustomData.Builder
        {

            #region Properties

            /// <summary>
            /// An EVSE identification.
            /// </summary>
            [Mandatory]
            public EVSE_Id                      EVSEId                  { get; set; }

            /// <summary>
            /// A charging session identification.
            /// </summary>
            [Mandatory]
            public Session_Id                   SessionId               { get; set; }

            /// <summary>
            /// The timestamp of the session start.
            /// </summary>
            [Mandatory]
            public DateTime                     SessionStart            { get; set; }

            /// <summary>
            /// The timestamp of the session end.
            /// </summary>
            [Mandatory]
            public DateTime                     SessionEnd              { get; set; }

            /// <summary>
            /// An identification.
            /// </summary>
            [Optional]
            public Identification               Identification          { get; set; }

            /// <summary>
            /// An unqiue identification for the consumed charging product.
            /// </summary>
            [Mandatory]
            public PartnerProduct_Id?           PartnerProductId        { get; set; }

            /// <summary>
            /// An optional CPO partner session identification.
            /// </summary>
            [Optional]
            public CPOPartnerSession_Id?        CPOPartnerSessionId     { get; set; }

            /// <summary>
            /// An optional EMP partner session identification.
            /// </summary>
            [Optional]
            public EMPPartnerSession_Id?        EMPPartnerSessionId     { get; set; }

            /// <summary>
            /// An optional charging start timestamp.
            /// </summary>
            [Optional]
            public DateTime?                    ChargingStart           { get; set; }

            /// <summary>
            /// An optional charging end timestamp.
            /// </summary>
            [Optional]
            public DateTime?                    ChargingEnd             { get; set; }

            /// <summary>
            /// An optional initial value of the energy meter.
            /// </summary>
            [Optional]
            public Decimal?                     MeterValueStart         { get; set; }

            /// <summary>
            /// An optional final value of the energy meter.
            /// </summary>
            [Optional]
            public Decimal?                     MeterValueEnd           { get; set; }

            /// <summary>
            /// An optional enumeration of meter values during the charging session.
            /// </summary>
            [Optional]
            public IEnumerable<Decimal>         MeterValuesInBetween    { get; set; }

            /// <summary>
            /// The optional amount of consumed energy.
            /// </summary>
            [Optional]
            public Decimal?                     ConsumedEnergy          { get; set; }

            /// <summary>
            /// An optional signature for the metering values.
            /// </summary>
            [Optional]
            public String                       MeteringSignature       { get; set; }

            /// <summary>
            /// An optional identification of the hub operator.
            /// </summary>
            [Optional]
            public HubOperator_Id?              HubOperatorId           { get; set; }

            /// <summary>
            /// An optional identification of the hub provider.
            /// </summary>
            [Optional]
            public HubProvider_Id?              HubProviderId           { get; set; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new OICP charge detail record.
            /// </summary>
            /// <param name="EVSEId">An EVSE identification.</param>
            /// <param name="SessionId">A charging session identification.</param>
            /// <param name="SessionStart">The timestamp of the session start.</param>
            /// <param name="SessionEnd">The timestamp of the session end.</param>
            /// <param name="Identification">An identification.</param>
            /// <param name="PartnerProductId">An unqiue identification for the consumed charging product.</param>
            /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
            /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
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
            public Builder(EVSE_Id                                    EVSEId,
                           Session_Id                                 SessionId,
                           DateTime                                   SessionStart,
                           DateTime                                   SessionEnd,
                           Identification                             Identification,
                           PartnerProduct_Id?                         PartnerProductId       = null,
                           CPOPartnerSession_Id?                      CPOPartnerSessionId    = null,
                           EMPPartnerSession_Id?                      EMPPartnerSessionId    = null,
                           DateTime?                                  ChargingStart          = null,
                           DateTime?                                  ChargingEnd            = null,
                           Decimal?                                   MeterValueStart        = null,  // xx.yyy
                           Decimal?                                   MeterValueEnd          = null,  // xx.yyy
                           IEnumerable<Decimal>                       MeterValuesInBetween   = null,  // xx.yyy
                           Decimal?                                   ConsumedEnergy         = null,  // xx.yyy
                           String                                     MeteringSignature      = null,  // maxlength: 200
                           HubOperator_Id?                            HubOperatorId          = null,
                           HubProvider_Id?                            HubProviderId          = null,

                           IEnumerable<KeyValuePair<String, Object>>  CustomData             = null)

                : base(CustomData)

            {

                this.EVSEId                = EVSEId;
                this.SessionId             = SessionId;
                this.SessionStart          = SessionStart;
                this.SessionEnd            = SessionEnd;
                this.Identification        = Identification ?? throw new ArgumentNullException(nameof(Identification),  "The given identification must not be null!");
                this.PartnerProductId      = PartnerProductId;
                this.CPOPartnerSessionId   = CPOPartnerSessionId;
                this.EMPPartnerSessionId   = EMPPartnerSessionId;
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


            #region Build()

            /// <summary>
            /// Return an immutable version of the charge detail record.
            /// </summary>
            public ChargeDetailRecord Build()

                => new ChargeDetailRecord(EVSEId,
                                          SessionId,
                                          SessionStart,
                                          SessionEnd,
                                          Identification,
                                          PartnerProductId,
                                          CPOPartnerSessionId,
                                          EMPPartnerSessionId,
                                          ChargingStart,
                                          ChargingEnd,
                                          MeterValueStart,
                                          MeterValueEnd,
                                          MeterValuesInBetween,
                                          ConsumedEnergy,
                                          MeteringSignature,
                                          HubOperatorId,
                                          HubProviderId,
                                          CustomData);

            #endregion

        }

        #endregion

    }

}
