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
using System.Threading;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.CPO
{

    /// <summary>
    /// An OICP push EVSE data request.
    /// </summary>
    public class PushEVSEDataRequest : ARequest<PushEVSEDataRequest>
    {

        #region Properties

        /// <summary>
        /// The operator EVSE data records.
        /// </summary>
        public OperatorEVSEData             OperatorEVSEData   { get; }

        /// <summary>
        /// The enumeration of EVSE data records.
        /// </summary>
        public IEnumerable<EVSEDataRecord>  EVSEDataRecords
            => OperatorEVSEData.EVSEDataRecords;

        /// <summary>
        /// The unqiue identification of the charging station operator maintaining the given EVSE data records.
        /// </summary>
        public Operator_Id                  OperatorId
            => OperatorEVSEData.OperatorId;

        /// <summary>
        /// The optional name of the charging station operator maintaining the given EVSE data records.
        /// </summary>
        public String                       OperatorName
            => OperatorEVSEData.OperatorName;

        /// <summary>
        /// The server-side data management operation.
        /// </summary>
        public ActionTypes                  Action            { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP PushEVSEData XML/SOAP request.
        /// </summary>
        /// <param name="OperatorEVSEData">An operator EVSE data.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public PushEVSEDataRequest(OperatorEVSEData    OperatorEVSEData,
                                   ActionTypes         Action              = ActionTypes.fullLoad,

                                   DateTime?           Timestamp           = null,
                                   CancellationToken?  CancellationToken   = null,
                                   EventTracking_Id    EventTrackingId     = null,
                                   TimeSpan?           RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            #region Initial checks

            if (OperatorEVSEData == null)
                throw new ArgumentNullException(nameof(OperatorEVSEData), "The given operator EVSE data must not be null!");

            #endregion

            this.OperatorEVSEData  = OperatorEVSEData;
            this.Action            = Action;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEData     = "http://www.hubject.com/b2b/services/evsedata/EVSEData.0"
        //                   xmlns:CommonTypes  = "http://www.hubject.com/b2b/services/commontypes/EVSEData.0">
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
        //             <EVSEData:OperatorName>GraphDefined e-Mobility Operator</AuthorizationStart:OperatorName>
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

        #endregion

        #region (static) Parse(PushEVSEDataXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP push EVSE data request.
        /// </summary>
        /// <param name="PushEVSEDataXML">The XML to parse.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static PushEVSEDataRequest Parse(XElement                                   PushEVSEDataXML,
                                                CustomXMLParserDelegate<OperatorEVSEData>  CustomOperatorEVSEDataParser  = null,
                                                CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser    = null,
                                                OnExceptionDelegate                        OnException                   = null,

                                                DateTime?                                  Timestamp                     = null,
                                                CancellationToken?                         CancellationToken             = null,
                                                EventTracking_Id                           EventTrackingId               = null,
                                                TimeSpan?                                  RequestTimeout                = null)

        {

            if (TryParse(PushEVSEDataXML,
                         out PushEVSEDataRequest _PushEVSEData,
                         CustomOperatorEVSEDataParser,
                         CustomEVSEDataRecordParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _PushEVSEData;

            return null;

        }

        #endregion

        #region (static) Parse(PushEVSEDataText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text-representation of an OICP push EVSE data request.
        /// </summary>
        /// <param name="PushEVSEDataText">The text to parse.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static PushEVSEDataRequest Parse(String                                     PushEVSEDataText,
                                                CustomXMLParserDelegate<OperatorEVSEData>  CustomOperatorEVSEDataParser  = null,
                                                CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser    = null,
                                                OnExceptionDelegate                        OnException                   = null,

                                                DateTime?                                  Timestamp                     = null,
                                                CancellationToken?                         CancellationToken             = null,
                                                EventTracking_Id                           EventTrackingId               = null,
                                                TimeSpan?                                  RequestTimeout                = null)
        {

            if (TryParse(PushEVSEDataText,
                         out PushEVSEDataRequest _PushEVSEData,
                         CustomOperatorEVSEDataParser,
                         CustomEVSEDataRecordParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _PushEVSEData;

            return null;

        }

        #endregion

        #region (static) TryParse(PushEVSEDataXML,  out PushEVSEData, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP push EVSE data request.
        /// </summary>
        /// <param name="PushEVSEDataXML">The XML to parse.</param>
        /// <param name="PushEVSEData">The parsed push EVSE data request.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                   PushEVSEDataXML,
                                       out PushEVSEDataRequest                    PushEVSEData,
                                       CustomXMLParserDelegate<OperatorEVSEData>  CustomOperatorEVSEDataParser  = null,
                                       CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser    = null,
                                       OnExceptionDelegate                        OnException                   = null,

                                       DateTime?                                  Timestamp                     = null,
                                       CancellationToken?                         CancellationToken             = null,
                                       EventTracking_Id                           EventTrackingId               = null,
                                       TimeSpan?                                  RequestTimeout                = null)
        {

            try
            {

                if (PushEVSEDataXML.Name != OICPNS.EVSEData + "eRoamingPushEvseData")
                {
                    PushEVSEData = null;
                    return false;
                }

                PushEVSEData = new PushEVSEDataRequest(

                                   OperatorEVSEData.Parse(PushEVSEDataXML.ElementOrFail(OICPNS.EVSEData + "OperatorEvseData"),
                                                          CustomOperatorEVSEDataParser,
                                                          CustomEVSEDataRecordParser),

                                   PushEVSEDataXML.MapValueOrFail(OICPNS.EVSEData + "ActionType",
                                                                  ActionTypesExtentions.Parse),

                                   Timestamp,
                                   CancellationToken,
                                   EventTrackingId,
                                   RequestTimeout

                               );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, PushEVSEDataXML, e);

                PushEVSEData = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(PushEVSEDataText, out PushEVSEData, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text-representation of an OICP push EVSE data request.
        /// </summary>
        /// <param name="PushEVSEDataText">The text to parse.</param>
        /// <param name="PushEVSEData">The parsed push EVSE data request.</param>
        /// <param name="CustomOperatorEVSEDataParser">A delegate to parse custom OperatorEVSEData XML elements.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                     PushEVSEDataText,
                                       out PushEVSEDataRequest                    PushEVSEData,
                                       CustomXMLParserDelegate<OperatorEVSEData>  CustomOperatorEVSEDataParser  = null,
                                       CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser    = null,
                                       OnExceptionDelegate                        OnException                   = null,

                                       DateTime?                                  Timestamp                     = null,
                                       CancellationToken?                         CancellationToken             = null,
                                       EventTracking_Id                           EventTrackingId               = null,
                                       TimeSpan?                                  RequestTimeout                = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(PushEVSEDataText).Root,
                             out PushEVSEData,
                             CustomOperatorEVSEDataParser,
                             CustomEVSEDataRecordParser,
                             OnException,

                             Timestamp,
                             CancellationToken,
                             EventTrackingId,
                             RequestTimeout))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, PushEVSEDataText, e);
            }

            PushEVSEData = null;
            return false;

        }

        #endregion

        #region ToXML(CustomPushEVSEDataRequestSerializer = null, OperatorEVSEDataXName = null, CustomOperatorEVSEDataSerializer = null, EVSEDataRecordXName = null, IncludeEVSEDataRecordMetadata = true, CustomEVSEDataRecordSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomPushEVSEDataRequestSerializer">A delegate to customize the serialization of PushEVSEData requests.</param>
        /// <param name="OperatorEVSEDataXName">The OperatorEVSEData XML name to use.</param>
        /// <param name="CustomOperatorEVSEDataSerializer">A delegate to serialize custom OperatorEVSEData XML elements.</param>
        /// <param name="EVSEDataRecordXName">The EVSEDataRecord XML name to use.</param>
        /// <param name="IncludeEVSEDataRecordMetadata">Include EVSEDataRecord deltaType and lastUpdate meta data.</param>
        /// <param name="CustomEVSEDataRecordSerializer">A delegate to serialize custom EVSEDataRecord XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<PushEVSEDataRequest>  CustomPushEVSEDataRequestSerializer   = null,
                              XName                                             OperatorEVSEDataXName                 = null,
                              CustomXMLSerializerDelegate<OperatorEVSEData>     CustomOperatorEVSEDataSerializer      = null,
                              XName                                             EVSEDataRecordXName                   = null,
                              Boolean                                           IncludeEVSEDataRecordMetadata         = true,
                              CustomXMLSerializerDelegate<EVSEDataRecord>       CustomEVSEDataRecordSerializer        = null)

        {

            var XML = new XElement(OICPNS.EVSEData + "eRoamingPushEvseData",

                                       new XElement(OICPNS.EVSEData + "ActionType", Action.AsString()),

                                       OperatorEVSEData.ToXML(OperatorEVSEDataXName,
                                                              CustomOperatorEVSEDataSerializer,
                                                              EVSEDataRecordXName,
                                                              IncludeEVSEDataRecordMetadata,
                                                              CustomEVSEDataRecordSerializer)

                                  );

            return CustomPushEVSEDataRequestSerializer != null
                       ? CustomPushEVSEDataRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PushEVSEData1, PushEVSEData2)

        /// <summary>
        /// Compares two push EVSE data requests for equality.
        /// </summary>
        /// <param name="PushEVSEData1">An push EVSE data request.</param>
        /// <param name="PushEVSEData2">Another push EVSE data request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PushEVSEDataRequest PushEVSEData1, PushEVSEDataRequest PushEVSEData2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PushEVSEData1, PushEVSEData2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PushEVSEData1 == null) || ((Object) PushEVSEData2 == null))
                return false;

            return PushEVSEData1.Equals(PushEVSEData2);

        }

        #endregion

        #region Operator != (PushEVSEData1, PushEVSEData2)

        /// <summary>
        /// Compares two push EVSE data requests for inequality.
        /// </summary>
        /// <param name="PushEVSEData1">An push EVSE data request.</param>
        /// <param name="PushEVSEData2">Another push EVSE data request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PushEVSEDataRequest PushEVSEData1, PushEVSEDataRequest PushEVSEData2)

            => !(PushEVSEData1 == PushEVSEData2);

        #endregion

        #endregion

        #region IEquatable<PushEVSEData> Members

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

            var PushEVSEData = Object as PushEVSEDataRequest;
            if ((Object) PushEVSEData == null)
                return false;

            return this.Equals(PushEVSEData);

        }

        #endregion

        #region Equals(PushEVSEData)

        /// <summary>
        /// Compares two push EVSE data requests for equality.
        /// </summary>
        /// <param name="PushEVSEData">An push EVSE data request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PushEVSEDataRequest PushEVSEData)
        {

            if ((Object) PushEVSEData == null)
                return false;

            return OperatorEVSEData.Equals(PushEVSEData.OperatorEVSEData) &&
                   Action.          Equals(PushEVSEData.Action);

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

                return OperatorEVSEData.GetHashCode() * 3 ^
                       Action.          GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Action, " of ",
                             EVSEDataRecords.Count(), " EVSE data record(s)",
                             " (", OperatorId, OperatorName != null ? " / " + OperatorName : "", ")");

        #endregion


    }

}
