/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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
    /// A group of OICP EVSE Data Records.
    /// </summary>
    public class OperatorEVSEData : ACustomData,
                                    IEquatable<OperatorEVSEData>,
                                    IComparable<OperatorEVSEData>,
                                    IComparable
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE data records.
        /// </summary>
        public IEnumerable<EVSEDataRecord>  EVSEDataRecords   { get; }

        /// <summary>
        /// The unqiue identification of the EVSE operator maintaining the given EVSE data records.
        /// </summary>
        public Operator_Id                  OperatorId        { get; }

        /// <summary>
        /// The optional name of the EVSE operator maintaining the given EVSE data records.
        /// </summary>
        public String                       OperatorName      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group of OICP EVSE data records.
        /// </summary>
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        /// <param name="OperatorId">The unqiue identification of the EVSE operator maintaining the given EVSE data records.</param>
        /// <param name="OperatorName">An optional name of the EVSE operator maintaining the given EVSE data records.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public OperatorEVSEData(IEnumerable<EVSEDataRecord>          EVSEDataRecords,
                                Operator_Id                          OperatorId,
                                String                               OperatorName  = null,
                                IReadOnlyDictionary<String, Object>  CustomData    = null)

            : base(CustomData)

        {

            #region Initial checks

            if (EVSEDataRecords == null || !EVSEDataRecords.Any())
                throw new ArgumentNullException(nameof(EVSEDataRecords), "The given enumeration of EVSE data records must not be null or empty!");

            if (OperatorName != null)
                OperatorName = OperatorName.Trim();

            #endregion

            this.EVSEDataRecords  = EVSEDataRecords;
            this.OperatorId       = OperatorId;
            this.OperatorName     = OperatorName.SubstringMax(100);

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.1"
        //                   xmlns:EVSEData    = "http://www.hubject.com/b2b/services/evsedata/v2.1">
        // 
        //    <soapenv:Header/>
        // 
        //    <soapenv:Body>
        //       <EVSEData:eRoamingPushEvseData>
        // 
        //          <EVSEData:ActionType>fullLoad|update|insert|delete</AuthorizationStart:ActionType>
        // 
        //          <EVSEData:OperatorEvseData>
        // 
        //             <EVSEData:OperatorID>DE*GEF</AuthorizationStart:OperatorID>
        // 
        //             <!--Optional:-->
        //             <EVSEData:OperatorName>GraphDefined</AuthorizationStart:OperatorName>
        // 
        //             <!--Zero or more repetitions:-->
        //             <EVSEData:EvseDataRecord deltaType="update|insert|delete" lastUpdate="?">
        //                [...]
        //             </EVSEData:EvseDataRecord>
        // 
        //          </EVSEData:OperatorEvseData>
        //
        //       </EVSEData:eRoamingPushEvseData>
        //    </soapenv:Body>
        // </soapenv:Envelope>


        // <?xml version="1.0" encoding="UTF-8"?>
        // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:fn          = "http://www.w3.org/2005/xpath-functions"
        //                   xmlns:sbp         = "http://www.inubit.com/eMobility/SBP"
        //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.1"
        //                   xmlns:EVSEData    = "http://www.hubject.com/b2b/services/evsedata/v2.1">
        //
        //    <soapenv:Body>
        //       <EVSEData:eRoamingEvseData>
        //          <EVSEData:EvseData>
        //
        //             <!--Zero or more repetitions:-->
        //             <EVSEData:OperatorEvseData>
        //
        //                <EVSEData:OperatorID>DE*GEF</EVSEData:OperatorID>
        //
        //                <!--Optional:-->
        //                <EVSEData:OperatorName>GraphDefined</EVSEData:OperatorName>
        //
        //                <!--Zero or more repetitions:-->
        //                <EVSEData:EvseDataRecord deltaType="update|insert|delete" lastUpdate="2017-04-21T01:00:03.000Z">
        //
        //                   <EVSEData:EvseId>DE*GEF*EVSE*CI*TESTS*A*1</EVSEData:EvseId>
        //                   <EVSEData:ChargingStationId>DE*GEF*STATION*CI*TESTS*A*1</EVSEData:ChargingStationId>
        //                   <EVSEData:ChargingStationName>DE*GEF*STATION*CI*TESTS*A*1</EVSEData:ChargingStationName>
        //                   <EVSEData:EnChargingStationName>DE*GEF*STATION*CI*TESTS*A*1</EVSEData:EnChargingStationName>
        //
        //                   <EVSEData:Address>
        //                      <CommonTypes:Country>DE</CommonTypes:Country>
        //                      <CommonTypes:City>Jena</CommonTypes:City>
        //                      <CommonTypes:Street>Biberweg</CommonTypes:Street>
        //                      <CommonTypes:PostalCode>07749</CommonTypes:PostalCode>
        //                      <CommonTypes:HouseNum>18</CommonTypes:HouseNum>
        //                   </EVSEData:Address>
        //
        //                   <EVSEData:GeoCoordinates>
        //                      <CommonTypes:DecimalDegree>
        //                         <CommonTypes:Longitude>11.6309461</CommonTypes:Longitude>
        //                         <CommonTypes:Latitude>50.9293504</CommonTypes:Latitude>
        //                      </CommonTypes:DecimalDegree>
        //                   </EVSEData:GeoCoordinates>
        //
        //                   <EVSEData:Plugs>
        //                      <EVSEData:Plug>Type 2 Outlet</EVSEData:Plug>
        //                   </EVSEData:Plugs>
        //
        //                   <EVSEData:AuthenticationModes>
        //                      <EVSEData:AuthenticationMode>NFC RFID Classic</EVSEData:AuthenticationMode>
        //                      <EVSEData:AuthenticationMode>NFC RFID DESFire</EVSEData:AuthenticationMode>
        //                      <EVSEData:AuthenticationMode>REMOTE</EVSEData:AuthenticationMode>
        //                      <EVSEData:AuthenticationMode>Direct Payment</EVSEData:AuthenticationMode>
        //                   </EVSEData:AuthenticationModes>
        //
        //                   <EVSEData:ValueAddedServices>
        //                      <EVSEData:ValueAddedService>Reservation</EVSEData:ValueAddedService>
        //                   </EVSEData:ValueAddedServices>
        //
        //                   <EVSEData:Accessibility>Free publicly accessible</EVSEData:Accessibility>
        //                   <EVSEData:HotlinePhoneNum>+4955512345</EVSEData:HotlinePhoneNum>
        //                   <EVSEData:IsOpen24Hours>true</EVSEData:IsOpen24Hours>
        //                   <EVSEData:HubOperatorID>DE*GEF</EVSEData:HubOperatorID>
        //                   <EVSEData:IsHubjectCompatible>true</EVSEData:IsHubjectCompatible>
        //                   <EVSEData:DynamicInfoAvailable>true</EVSEData:DynamicInfoAvailable>
        //
        //                </EVSEData:EvseDataRecord>
        //
        //             </EVSEData:OperatorEvseData>
        // 
        //           </EVSEData:EvseData>
        //       </EVSEData:eRoamingEvseData>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (OperatorEVSEDataXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP operator EVSE data request.
        /// </summary>
        /// <param name="OperatorEVSEDataXML">The XML to parse.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static OperatorEVSEData Parse(XElement                                   OperatorEVSEDataXML,
                                             CustomXMLParserDelegate<OperatorEVSEData>  CustomOperatorEVSEDataParser   = null,
                                             CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser     = null,
                                             CustomXMLParserDelegate<Address>           CustomAddressParser            = null,
                                             OnExceptionDelegate                        OnException                    = null)
        {

            if (TryParse(OperatorEVSEDataXML,
                         out OperatorEVSEData operatorEVSEData,
                         CustomOperatorEVSEDataParser,
                         CustomEVSEDataRecordParser,
                         CustomAddressParser,
                         OnException))

                return operatorEVSEData;

            return null;

        }

        #endregion

        #region (static) Parse   (OperatorEVSEDataText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP operator EVSE data request.
        /// </summary>
        /// <param name="OperatorEVSEDataText">The text to parse.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static OperatorEVSEData Parse(String                                     OperatorEVSEDataText,
                                             CustomXMLParserDelegate<OperatorEVSEData>  CustomOperatorEVSEDataParser   = null,
                                             CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser     = null,
                                             CustomXMLParserDelegate<Address>           CustomAddressParser            = null,
                                             OnExceptionDelegate                        OnException                    = null)
        {

            if (TryParse(OperatorEVSEDataText,
                         out OperatorEVSEData operatorEVSEData,
                         CustomOperatorEVSEDataParser,
                         CustomEVSEDataRecordParser,
                         CustomAddressParser,
                         OnException))

                return operatorEVSEData;

            return null;

        }

        #endregion

        #region (static) TryParse(OperatorEVSEDataXML,  out OperatorEVSEData, ..., OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP operator EVSE data request.
        /// </summary>
        /// <param name="OperatorEVSEDataXML">The XML to parse.</param>
        /// <param name="OperatorEVSEData">The parsed operator EVSE data request.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                   OperatorEVSEDataXML,
                                       out OperatorEVSEData                       OperatorEVSEData,
                                       CustomXMLParserDelegate<OperatorEVSEData>  CustomOperatorEVSEDataParser   = null,
                                       CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser     = null,
                                       CustomXMLParserDelegate<Address>           CustomAddressParser            = null,
                                       OnExceptionDelegate                        OnException                    = null)
        {

            try
            {

                if (OperatorEVSEDataXML.Name != OICPNS.EVSEData + "OperatorEvseData")
                {
                    OperatorEVSEData = null;
                    return false;
                }

                OperatorEVSEData = new OperatorEVSEData(

                                       OperatorEVSEDataXML.MapElements          (OICPNS.EVSEData + "EvseDataRecord",
                                                                                 (xml, e) => EVSEDataRecord.Parse(xml,
                                                                                                                  CustomEVSEDataRecordParser,
                                                                                                                  CustomAddressParser,
                                                                                                                  e),
                                                                                 OnException).
                                                                                 Where(operatorevsedata => operatorevsedata != null),

                                       OperatorEVSEDataXML.MapValueOrFail       (OICPNS.EVSEData + "OperatorID",
                                                                                 Operator_Id.Parse),

                                       OperatorEVSEDataXML.ElementValueOrDefault(OICPNS.EVSEData + "OperatorName")

                                   );


                if (CustomOperatorEVSEDataParser != null)
                    OperatorEVSEData = CustomOperatorEVSEDataParser(OperatorEVSEDataXML,
                                                                    OperatorEVSEData);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, OperatorEVSEDataXML, e);

                OperatorEVSEData = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(OperatorEVSEDataText, out OperatorEVSEData, ..., OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP operator EVSE data request.
        /// </summary>
        /// <param name="OperatorEVSEDataText">The text to parse.</param>
        /// <param name="OperatorEVSEData">The parsed operator EVSE data request.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                     OperatorEVSEDataText,
                                       out OperatorEVSEData                       OperatorEVSEData,
                                       CustomXMLParserDelegate<OperatorEVSEData>  CustomOperatorEVSEDataParser   = null,
                                       CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser     = null,
                                       CustomXMLParserDelegate<Address>           CustomAddressParser            = null,
                                       OnExceptionDelegate                        OnException                    = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(OperatorEVSEDataText).Root,
                             out OperatorEVSEData,
                             CustomOperatorEVSEDataParser,
                             CustomEVSEDataRecordParser,
                             CustomAddressParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, OperatorEVSEDataText, e);
            }

            OperatorEVSEData = null;
            return false;

        }

        #endregion

        #region ToXML(OperatorEVSEDataXName = null, CustomOperatorEVSEDataSerializer = null, EVSEDataRecordXName = null, IncludeEVSEDataRecordMetadata = true, CustomEVSEDataRecordSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="OperatorEVSEDataXName">The OperatorEVSEData XML name to use.</param>
        /// <param name="CustomOperatorEVSEDataSerializer">A delegate to serialize custom OperatorEVSEData XML elements.</param>
        /// <param name="EVSEDataRecordXName">The EVSEDataRecord XML name to use.</param>
        /// <param name="IncludeEVSEDataRecordMetadata">Include EVSEDataRecord deltaType and lastUpdate meta data.</param>
        /// <param name="CustomEVSEDataRecordSerializer">A delegate to serialize custom EVSEDataRecord XML elements.</param>
        public XElement ToXML(XName                                          OperatorEVSEDataXName              = null,
                              CustomXMLSerializerDelegate<OperatorEVSEData>  CustomOperatorEVSEDataSerializer   = null,
                              XName                                          EVSEDataRecordXName                = null,
                              Boolean                                        IncludeEVSEDataRecordMetadata      = true,
                              CustomXMLSerializerDelegate<EVSEDataRecord>    CustomEVSEDataRecordSerializer     = null)

        {

            var xml =  new XElement(OperatorEVSEDataXName ?? OICPNS.EVSEData + "OperatorEvseData",

                           new XElement(OICPNS.EVSEData + "OperatorID",          OperatorId.ToString()),

                           OperatorName.IsNotNullOrEmpty()
                               ? new XElement(OICPNS.EVSEData + "OperatorName",  OperatorName)
                               : null,

                           EVSEDataRecords.Any()
                               ? EVSEDataRecords.SafeSelect(evsedatarecord => evsedatarecord.ToXML(EVSEDataRecordXName,
                                                                                                   IncludeEVSEDataRecordMetadata,
                                                                                                   CustomEVSEDataRecordSerializer))
                               : null

                       );

            return CustomOperatorEVSEDataSerializer != null
                       ? CustomOperatorEVSEDataSerializer(this, xml)
                       : xml;

        }

        #endregion


        #region Operator overloading

        #region Operator == (OperatorEVSEData1, OperatorEVSEData2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="OperatorEVSEData1">An operator EVSE data.</param>
        /// <param name="OperatorEVSEData2">Another operator EVSE data.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (OperatorEVSEData OperatorEVSEData1, OperatorEVSEData OperatorEVSEData2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(OperatorEVSEData1, OperatorEVSEData2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) OperatorEVSEData1 == null) || ((Object) OperatorEVSEData2 == null))
                return false;

            return OperatorEVSEData1.Equals(OperatorEVSEData2);

        }

        #endregion

        #region Operator != (OperatorEVSEData1, OperatorEVSEData2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="OperatorEVSEData1">An operator EVSE data.</param>
        /// <param name="OperatorEVSEData2">Another operator EVSE data.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (OperatorEVSEData OperatorEVSEData1, OperatorEVSEData OperatorEVSEData2)

            => !(OperatorEVSEData1 == OperatorEVSEData2);

        #endregion

        #region Operator <  (OperatorEVSEData1, OperatorEVSEData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEData1">An operator EVSE data.</param>
        /// <param name="OperatorEVSEData2">Another operator EVSE data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (OperatorEVSEData OperatorEVSEData1, OperatorEVSEData OperatorEVSEData2)
        {

            if ((Object) OperatorEVSEData1 == null)
                throw new ArgumentNullException(nameof(OperatorEVSEData1), "The given OperatorEVSEData1 must not be null!");

            return OperatorEVSEData1.CompareTo(OperatorEVSEData2) < 0;

        }

        #endregion

        #region Operator <= (OperatorEVSEData1, OperatorEVSEData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEData1">An operator EVSE data.</param>
        /// <param name="OperatorEVSEData2">Another operator EVSE data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (OperatorEVSEData OperatorEVSEData1, OperatorEVSEData OperatorEVSEData2)
            => !(OperatorEVSEData1 > OperatorEVSEData2);

        #endregion

        #region Operator >  (OperatorEVSEData1, OperatorEVSEData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEData1">An operator EVSE data.</param>
        /// <param name="OperatorEVSEData2">Another operator EVSE data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (OperatorEVSEData OperatorEVSEData1, OperatorEVSEData OperatorEVSEData2)
        {

            if ((Object) OperatorEVSEData1 == null)
                throw new ArgumentNullException(nameof(OperatorEVSEData1), "The given OperatorEVSEData1 must not be null!");

            return OperatorEVSEData1.CompareTo(OperatorEVSEData2) > 0;

        }

        #endregion

        #region Operator >= (OperatorEVSEData1, OperatorEVSEData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEData1">An operator EVSE data.</param>
        /// <param name="OperatorEVSEData2">Another operator EVSE data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (OperatorEVSEData OperatorEVSEData1, OperatorEVSEData OperatorEVSEData2)
            => !(OperatorEVSEData1 < OperatorEVSEData2);

        #endregion

        #endregion

        #region IComparable<OperatorEVSEData> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is OperatorEVSEData OperatorEVSEData))
                throw new ArgumentException("The given object is not an operator EVSE data identification!", nameof(Object));

            return CompareTo(OperatorEVSEData);

        }

        #endregion

        #region CompareTo(OperatorEVSEData)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEData">An object to compare with.</param>
        public Int32 CompareTo(OperatorEVSEData OperatorEVSEData)
        {

            if ((Object) OperatorEVSEData == null)
                throw new ArgumentNullException(nameof(OperatorEVSEData), "The given operator EVSE data must not be null!");

            return OperatorId.CompareTo(OperatorEVSEData.OperatorId);

        }

        #endregion

        #endregion

        #region IEquatable<OperatorEVSEData> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object is null)
                return false;

            if (!(Object is OperatorEVSEData OperatorEVSEData))
                return false;

            return Equals(OperatorEVSEData);

        }

        #endregion

        #region Equals(OperatorEVSEData)

        /// <summary>
        /// Compares two operator EVSE datas for equality.
        /// </summary>
        /// <param name="OperatorEVSEData">A operator EVSE data to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(OperatorEVSEData OperatorEVSEData)
        {

            if ((Object) OperatorEVSEData == null)
                return false;

            return OperatorId.Equals(OperatorEVSEData.OperatorId) &&

                   ((OperatorName   == null && OperatorEVSEData.OperatorName   == null) ||
                    (OperatorName   != null && OperatorEVSEData.OperatorName   != null && OperatorName.   Equals(OperatorEVSEData.OperatorName))) &&

                   ((!EVSEDataRecords.Any() && !OperatorEVSEData.EVSEDataRecords.Any()) ||
                     (EVSEDataRecords.Any() &&  OperatorEVSEData.EVSEDataRecords.Any() && EVSEDataRecords.Count().Equals(OperatorEVSEData.EVSEDataRecords.Count())));

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return OperatorId.GetHashCode() * 5 ^

                       (OperatorName.IsNotNullOrEmpty()
                            ? OperatorName.   GetHashCode()
                            : 0) * 3 ^

                       (EVSEDataRecords.Any()
                            ? EVSEDataRecords.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(OperatorId,
                             OperatorName.IsNotNullOrEmpty() ? ", " + OperatorName : "",
                             ", ",  EVSEDataRecords.Count(), " EVSE data record(s)");

        #endregion

    }

}
