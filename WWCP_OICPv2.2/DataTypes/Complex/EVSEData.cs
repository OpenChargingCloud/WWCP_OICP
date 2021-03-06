﻿/*
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
    /// A group of operator EVSE data records.
    /// </summary>
    public class EVSEData : IEquatable<EVSEData>
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE data records grouped by their operators.
        /// </summary>
        public IEnumerable<OperatorEVSEData>  OperatorEVSEData   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group of operator EVSE data records.
        /// </summary>
        /// <param name="OperatorEVSEData">An enumeration of EVSE data records grouped by their operators.</param>
        public EVSEData(IEnumerable<OperatorEVSEData>  OperatorEVSEData)
        {

            this.OperatorEVSEData  = OperatorEVSEData ?? throw new ArgumentNullException(nameof(OperatorEVSEData),  "The given OperatorEVSEData must not be null!");

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:CommonTypes  = "http://www.hubject.com/b2b/services/commontypes/v2.0"
        //                   xmlns:EVSEData     = "http://www.hubject.com/b2b/services/evsedata/v2.1">
        //
        // [...]
        //
        //   <EVSEData:EvseData>
        //
        //      <!--Zero or more repetitions:-->
        //      <EVSEData:OperatorEvseData>
        //
        //         <EVSEData:OperatorID>DE*GEF</EVSEData:OperatorID>
        //
        //         <!--Optional:-->
        //         <EVSEData:OperatorName>GraphDefined</EVSEData:OperatorName>
        //
        //         <!--Zero or more repetitions:-->
        //         <EVSEData:EvseDataRecord deltaType="update|insert|delete" lastUpdate="2017-04-21T01:00:03.000Z">
        //
        //            <EVSEData:EvseId>DE*GEF*EVSE*CI*TESTS*A*1</EVSEData:EvseId>
        //            <EVSEData:ChargingStationId>DE*GEF*STATION*CI*TESTS*A*1</EVSEData:ChargingStationId>
        //            <EVSEData:ChargingStationName>DE*GEF*STATION*CI*TESTS*A*1</EVSEData:ChargingStationName>
        //            <EVSEData:EnChargingStationName>DE*GEF*STATION*CI*TESTS*A*1</EVSEData:EnChargingStationName>
        //
        //            <EVSEData:Address>
        //               <CommonTypes:Country>DE</CommonTypes:Country>
        //               <CommonTypes:City>Jena</CommonTypes:City>
        //               <CommonTypes:Street>Biberweg</CommonTypes:Street>
        //               <CommonTypes:PostalCode>07749</CommonTypes:PostalCode>
        //               <CommonTypes:HouseNum>18</CommonTypes:HouseNum>
        //            </EVSEData:Address>
        //
        //            <EVSEData:GeoCoordinates>
        //               <CommonTypes:DecimalDegree>
        //                  <CommonTypes:Longitude>11.6309461</CommonTypes:Longitude>
        //                  <CommonTypes:Latitude>50.9293504</CommonTypes:Latitude>
        //               </CommonTypes:DecimalDegree>
        //            </EVSEData:GeoCoordinates>
        //
        //            <EVSEData:Plugs>
        //               <EVSEData:Plug>Type 2 Outlet</EVSEData:Plug>
        //            </EVSEData:Plugs>
        //
        //            <EVSEData:AuthenticationModes>
        //               <EVSEData:AuthenticationMode>NFC RFID Classic</EVSEData:AuthenticationMode>
        //               <EVSEData:AuthenticationMode>NFC RFID DESFire</EVSEData:AuthenticationMode>
        //               <EVSEData:AuthenticationMode>REMOTE</EVSEData:AuthenticationMode>
        //               <EVSEData:AuthenticationMode>Direct Payment</EVSEData:AuthenticationMode>
        //            </EVSEData:AuthenticationModes>
        //
        //            <EVSEData:ValueAddedServices>
        //               <EVSEData:ValueAddedService>Reservation</EVSEData:ValueAddedService>
        //            </EVSEData:ValueAddedServices>
        //
        //            <EVSEData:Accessibility>Free publicly accessible</EVSEData:Accessibility>
        //            <EVSEData:HotlinePhoneNum>+4955512345</EVSEData:HotlinePhoneNum>
        //            <EVSEData:IsOpen24Hours>true</EVSEData:IsOpen24Hours>
        //            <EVSEData:HubOperatorID>DE*GEF</EVSEData:HubOperatorID>
        //            <EVSEData:IsHubjectCompatible>true</EVSEData:IsHubjectCompatible>
        //            <EVSEData:DynamicInfoAvailable>true</EVSEData:DynamicInfoAvailable>
        //
        //         </EVSEData:EvseDataRecord>
        //
        //      </EVSEData:OperatorEvseData>
        //
        //    </EVSEData:EvseData>
        //
        // [...]
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(EVSEDataXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP EVSE data request.
        /// </summary>
        /// <param name="EVSEDataXML">The XML to parse.</param>
        /// <param name="CustomEVSEDataParser">A delegate to parse custom EVSEData XML elements.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="CustomChargingFacilityParser">A delegate to parse custom ChargingFacility XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEData Parse(XElement                                   EVSEDataXML,
                                     CustomXMLParserDelegate<EVSEData>          CustomEVSEDataParser           = null,
                                     CustomXMLParserDelegate<OperatorEVSEData>  CustomOperatorEVSEDataParser   = null,
                                     CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser     = null,
                                     CustomXMLParserDelegate<Address>           CustomAddressParser            = null,
                                     CustomXMLParserDelegate<ChargingFacility>  CustomChargingFacilityParser   = null,
                                     OnExceptionDelegate                        OnException                    = null)
        {

            if (TryParse(EVSEDataXML,
                         out EVSEData _EVSEData,
                         CustomEVSEDataParser,
                         CustomOperatorEVSEDataParser,
                         CustomEVSEDataRecordParser,
                         CustomAddressParser,
                         CustomChargingFacilityParser,
                         OnException))

                return _EVSEData;

            return null;

        }

        #endregion

        #region (static) Parse(EVSEDataText, ..., OnException = null)

        /// <summary>
        /// Parse the given text-representation of an OICP EVSE data request.
        /// </summary>
        /// <param name="EVSEDataText">The text to parse.</param>
        /// <param name="CustomEVSEDataParser">A delegate to parse custom EVSEData XML elements.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="CustomChargingFacilityParser">A delegate to parse custom ChargingFacility XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEData Parse(String                                     EVSEDataText,
                                     CustomXMLParserDelegate<EVSEData>          CustomEVSEDataParser           = null,
                                     CustomXMLParserDelegate<OperatorEVSEData>  CustomOperatorEVSEDataParser   = null,
                                     CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser     = null,
                                     CustomXMLParserDelegate<Address>           CustomAddressParser            = null,
                                     CustomXMLParserDelegate<ChargingFacility>  CustomChargingFacilityParser   = null,
                                     OnExceptionDelegate                        OnException                    = null)
        {

            if (TryParse(EVSEDataText,
                         out EVSEData _EVSEData,
                         CustomEVSEDataParser,
                         CustomOperatorEVSEDataParser,
                         CustomEVSEDataRecordParser,
                         CustomAddressParser,
                         CustomChargingFacilityParser,
                         OnException))

                return _EVSEData;

            return null;

        }

        #endregion

        #region (static) TryParse(EVSEDataXML,  out EVSEData, ..., OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP EVSE data request.
        /// </summary>
        /// <param name="EVSEDataXML">The XML to parse.</param>
        /// <param name="EVSEData">The parsed EVSEData request.</param>
        /// <param name="CustomEVSEDataParser">A delegate to parse custom EVSEData XML elements.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="CustomChargingFacilityParser">A delegate to parse custom ChargingFacility XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                   EVSEDataXML,
                                       out EVSEData                               EVSEData,
                                       CustomXMLParserDelegate<EVSEData>          CustomEVSEDataParser           = null,
                                       CustomXMLParserDelegate<OperatorEVSEData>  CustomOperatorEVSEDataParser   = null,
                                       CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser     = null,
                                       CustomXMLParserDelegate<Address>           CustomAddressParser            = null,
                                       CustomXMLParserDelegate<ChargingFacility>  CustomChargingFacilityParser   = null,
                                       OnExceptionDelegate                        OnException                    = null)
        {

            try
            {

                if (EVSEDataXML.Name != OICPNS.EVSEData + "EvseData")
                {
                    EVSEData = null;
                    return false;
                }

                EVSEData = new EVSEData(

                               EVSEDataXML.MapElements(OICPNS.EVSEData + "OperatorEvseData",
                                                       (OperatorEvseDataXML, onexception) => OICPv2_2.OperatorEVSEData.Parse(OperatorEvseDataXML,
                                                                                                                             CustomOperatorEVSEDataParser,
                                                                                                                             CustomEVSEDataRecordParser,
                                                                                                                             CustomAddressParser,
                                                                                                                             CustomChargingFacilityParser,
                                                                                                                             onexception),
                                                       OnException)

                           );


                if (CustomEVSEDataParser != null)
                    EVSEData = CustomEVSEDataParser(EVSEDataXML,
                                                    EVSEData);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, EVSEDataXML, e);

                EVSEData = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(EVSEDataText, out EVSEData, ..., OnException = null)

        /// <summary>
        /// Try to parse the given text-representation of an OICP EVSE data request.
        /// </summary>
        /// <param name="EVSEDataText">The text to parse.</param>
        /// <param name="EVSEData">The parsed EVSEData request.</param>
        /// <param name="CustomEVSEDataParser">A delegate to parse custom EVSEData XML elements.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="CustomChargingFacilityParser">A delegate to parse custom ChargingFacility XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                     EVSEDataText,
                                       out EVSEData                               EVSEData,
                                       CustomXMLParserDelegate<EVSEData>          CustomEVSEDataParser           = null,
                                       CustomXMLParserDelegate<OperatorEVSEData>  CustomOperatorEVSEDataParser   = null,
                                       CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser     = null,
                                       CustomXMLParserDelegate<Address>           CustomAddressParser            = null,
                                       CustomXMLParserDelegate<ChargingFacility>  CustomChargingFacilityParser   = null,
                                       OnExceptionDelegate                        OnException                    = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(EVSEDataText).Root,
                             out EVSEData,
                             CustomEVSEDataParser,
                             CustomOperatorEVSEDataParser,
                             CustomEVSEDataRecordParser,
                             CustomAddressParser,
                             CustomChargingFacilityParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, EVSEDataText, e);
            }

            EVSEData = null;
            return false;

        }

        #endregion

        #region ToXML(CustomEVSEDataSerializer = null, OperatorEVSEDataXName = null, CustomOperatorEVSEDataSerializer = null, EVSEDataRecordXName = null, IncludeEVSEDataRecordMetadata = true, CustomEVSEDataRecordSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomEVSEDataSerializer">A delegate to serialize custom EVSEData XML elements.</param>
        /// <param name="OperatorEVSEDataXName">The OperatorEVSEData XML name to use.</param>
        /// <param name="CustomOperatorEVSEDataSerializer">A delegate to serialize custom OperatorEVSEData XML elements.</param>
        /// <param name="EVSEDataRecordXName">The EVSEDataRecord XML name to use.</param>
        /// <param name="IncludeEVSEDataRecordMetadata">Include EVSEDataRecord deltaType and lastUpdate meta data.</param>
        /// <param name="CustomEVSEDataRecordSerializer">A delegate to serialize custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressSerializer">A delegate to serialize custom Address XML elements.</param>
        /// <param name="CustomChargingFacilitySerializer">A delegate to serialize custom ChargingFacility XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<EVSEData>          CustomEVSEDataSerializer           = null,
                              XName                                          OperatorEVSEDataXName              = null,
                              CustomXMLSerializerDelegate<OperatorEVSEData>  CustomOperatorEVSEDataSerializer   = null,
                              XName                                          EVSEDataRecordXName                = null,
                              Boolean                                        IncludeEVSEDataRecordMetadata      = true,
                              CustomXMLSerializerDelegate<EVSEDataRecord>    CustomEVSEDataRecordSerializer     = null,
                              CustomXMLSerializerDelegate<Address>           CustomAddressSerializer            = null,
                              CustomXMLSerializerDelegate<ChargingFacility>  CustomChargingFacilitySerializer   = null)

        {

            var XML = new XElement(OICPNS.EVSEData + "EvseData",

                          OperatorEVSEData.Any()
                              ? OperatorEVSEData.SafeSelect(operatorevsedata => operatorevsedata.ToXML(OperatorEVSEDataXName,
                                                                                                       CustomOperatorEVSEDataSerializer,
                                                                                                       EVSEDataRecordXName,
                                                                                                       IncludeEVSEDataRecordMetadata,
                                                                                                       CustomEVSEDataRecordSerializer,
                                                                                                       CustomAddressSerializer,
                                                                                                       CustomChargingFacilitySerializer))
                              : null

                      );


            return CustomEVSEDataSerializer != null
                       ? CustomEVSEDataSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSEData1, EVSEData2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="EVSEData1">An EVSE data.</param>
        /// <param name="EVSEData2">Another EVSE data.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EVSEData EVSEData1, EVSEData EVSEData2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EVSEData1, EVSEData2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEData1 == null) || ((Object) EVSEData2 == null))
                return false;

            return EVSEData1.Equals(EVSEData2);

        }

        #endregion

        #region Operator != (EVSEData1, EVSEData2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="EVSEData1">An EVSE data.</param>
        /// <param name="EVSEData2">Another EVSE data.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EVSEData EVSEData1, EVSEData EVSEData2)

            => !(EVSEData1 == EVSEData2);

        #endregion

        #endregion

        #region IEquatable<EVSEData> Members

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

            var EVSEData = Object as EVSEData;
            if ((Object) EVSEData == null)
                return false;

            return Equals(EVSEData);

        }

        #endregion

        #region Equals(EVSEData)

        /// <summary>
        /// Compares two EVSE data for equality.
        /// </summary>
        /// <param name="EVSEData">An EVSE data to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEData EVSEData)
        {

            if ((Object) EVSEData == null)
                return false;

            return (!OperatorEVSEData.Any() && !EVSEData.OperatorEVSEData.Any()) ||
                    (OperatorEVSEData.Any() &&  EVSEData.OperatorEVSEData.Any() && OperatorEVSEData.Count().Equals(EVSEData.OperatorEVSEData.Count()));

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

                return OperatorEVSEData.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(OperatorEVSEData.Count(), " operator EVSE data record(s)");

        #endregion


    }

}
