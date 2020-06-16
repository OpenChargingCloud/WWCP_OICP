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
using System.Xml.Linq;
using System.Threading;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.EMP
{

    /// <summary>
    /// An OICP push authentication data request.
    /// </summary>
    public class PushAuthenticationDataRequest : ARequest<PushAuthenticationDataRequest>
    {

        #region Properties

        /// <summary>
        /// The e-mobility provider authentication data.
        /// </summary>
        public ProviderAuthenticationData  ProviderAuthenticationData    { get; }

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        public Provider_Id                 ProviderId                    { get; }

        /// <summary>
        /// The server-side data management operation.
        /// </summary>
        public ActionTypes                 OICPAction                    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP PushAuthenticationDataRequest XML/SOAP request.
        /// </summary>
        /// <param name="ProviderAuthenticationData">The e-mobility provider authentication data.</param>
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public PushAuthenticationDataRequest(ProviderAuthenticationData  ProviderAuthenticationData,
                                             ActionTypes                 OICPAction          = ActionTypes.fullLoad,

                                             DateTime?                   Timestamp           = null,
                                             CancellationToken?          CancellationToken   = null,
                                             EventTracking_Id            EventTrackingId     = null,
                                             TimeSpan?                   RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            #region Initial checks

            if (ProviderAuthenticationData == null)
                throw new ArgumentNullException(nameof(ProviderAuthenticationData), "The given enumeration of authorization identifications must not be null!");

            #endregion

            this.ProviderAuthenticationData  = ProviderAuthenticationData;
            this.ProviderId                  = ProviderId;
            this.OICPAction                  = OICPAction;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv            = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:AuthenticationData = "http://www.hubject.com/b2b/services/authenticationdata/v2.0"
        //                   xmlns:CommonTypes        = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <AuthenticationData:eRoamingPushAuthenticationData>
        // 
        //          <AuthenticationData:ActionType>fullLoad|update|insert|delete</AuthenticationData:ActionType>
        // 
        //          <AuthenticationData:ProviderAuthenticationData>
        // 
        //             <AuthenticationData:ProviderID>DE*GDF</AuthenticationData:ProviderID>
        // 
        //             <!--Zero or more repetitions:-->
        //             <AuthenticationData:AuthenticationDataRecord>
        //                <AuthenticationData:Identification>
        // 
        //                   <!--You have a CHOICE of the next 4 items at this level-->
        //                   <CommonTypes:RFIDmifarefamilyIdentification>
        //                      <CommonTypes:UID>08152305</CommonTypes:UID>
        //                   </CommonTypes:RFIDmifarefamilyIdentification>
        // 
        //                   <CommonTypes:QRCodeIdentification>
        // 
        //                      <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        // 
        //                      <!--You have a CHOICE of the next 2 items at this level-->
        //                      <CommonTypes:PIN>?</CommonTypes:PIN>
        // 
        //                      <CommonTypes:HashedPIN>
        //                         <CommonTypes:Value>f7cf02826ba923e3d31c1c3015899076</CommonTypes:Value>
        //                         <CommonTypes:Function>MD5|SHA-1</CommonTypes:Function>
        //                         <CommonTypes:Salt>22c7c09370af2a3f07fe8665b140498a</CommonTypes:Salt>
        //                      </CommonTypes:HashedPIN>
        // 
        //                   </CommonTypes:QRCodeIdentification>
        // 
        //                   <CommonTypes:PlugAndChargeIdentification>
        //                      <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //                   </CommonTypes:PlugAndChargeIdentification>
        // 
        //                   <CommonTypes:RemoteIdentification>
        //                      <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //                   </CommonTypes:RemoteIdentification>
        // 
        //                </AuthenticationData:Identification>
        //             </AuthenticationData:AuthenticationDataRecord>
        // 
        //          </AuthenticationData:ProviderAuthenticationData>
        // 
        //       </AuthenticationData:eRoamingPushAuthenticationData>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(PushAuthenticationDataRequestXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP push authentication data request.
        /// </summary>
        /// <param name="PushAuthenticationDataRequestXML">The XML to parse.</param>
        /// <param name="CustomPushAuthenticationDataRequestParser">A delegate to parse custom PushAuthenticationData requests.</param>
        /// <param name="CustomProviderAuthenticationDataParser">A delegate to parse custom ProviderAuthenticationData XML elements.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static PushAuthenticationDataRequest

            Parse(XElement                                                PushAuthenticationDataRequestXML,
                  CustomXMLParserDelegate<PushAuthenticationDataRequest>  CustomPushAuthenticationDataRequestParser   = null,
                  CustomXMLParserDelegate<ProviderAuthenticationData>     CustomProviderAuthenticationDataParser      = null,
                  CustomXMLParserDelegate<Identification>                 CustomIdentificationParser                  = null,
                  CustomXMLParserDelegate<RFIDIdentification>             CustomRFIDIdentificationParser              = null,
                  OnExceptionDelegate                                     OnException                                 = null,

                  DateTime?                                               Timestamp                                   = null,
                  CancellationToken?                                      CancellationToken                           = null,
                  EventTracking_Id                                        EventTrackingId                             = null,
                  TimeSpan?                                               RequestTimeout                              = null)

        {

            if (TryParse(PushAuthenticationDataRequestXML,
                         out PushAuthenticationDataRequest _PushAuthenticationDataRequest,
                         CustomPushAuthenticationDataRequestParser,
                         CustomProviderAuthenticationDataParser,
                         CustomIdentificationParser,
                         CustomRFIDIdentificationParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _PushAuthenticationDataRequest;

            return null;

        }

        #endregion

        #region (static) Parse(PushAuthenticationDataRequestText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text representation of an OICP push authentication data request.
        /// </summary>
        /// <param name="PushAuthenticationDataRequestText">The text to parse.</param>
        /// <param name="CustomPushAuthenticationDataRequestParser">A delegate to parse custom PushAuthenticationData requests.</param>
        /// <param name="CustomProviderAuthenticationDataParser">A delegate to parse custom ProviderAuthenticationData XML elements.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static PushAuthenticationDataRequest

            Parse(String                                                  PushAuthenticationDataRequestText,
                  CustomXMLParserDelegate<PushAuthenticationDataRequest>  CustomPushAuthenticationDataRequestParser   = null,
                  CustomXMLParserDelegate<ProviderAuthenticationData>     CustomProviderAuthenticationDataParser      = null,
                  CustomXMLParserDelegate<Identification>                 CustomIdentificationParser                  = null,
                  CustomXMLParserDelegate<RFIDIdentification>             CustomRFIDIdentificationParser              = null,
                  OnExceptionDelegate                                     OnException                                 = null,

                  DateTime?                                               Timestamp                                   = null,
                  CancellationToken?                                      CancellationToken                           = null,
                  EventTracking_Id                                        EventTrackingId                             = null,
                  TimeSpan?                                               RequestTimeout                              = null)

        {

            if (TryParse(PushAuthenticationDataRequestText,
                         out PushAuthenticationDataRequest _PushAuthenticationDataRequest,
                         CustomPushAuthenticationDataRequestParser,
                         CustomProviderAuthenticationDataParser,
                         CustomIdentificationParser,
                         CustomRFIDIdentificationParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _PushAuthenticationDataRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(PushAuthenticationDataRequestXML,  out PushAuthenticationDataRequest, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP push authentication data request.
        /// </summary>
        /// <param name="PushAuthenticationDataRequestXML">The XML to parse.</param>
        /// <param name="PushAuthenticationDataRequest">The parsed push authentication data request.</param>
        /// <param name="CustomPushAuthenticationDataRequestParser">A delegate to parse custom PushAuthenticationData requests.</param>
        /// <param name="CustomProviderAuthenticationDataParser">A delegate to parse custom ProviderAuthenticationData XML elements.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                                PushAuthenticationDataRequestXML,
                                       out PushAuthenticationDataRequest                       PushAuthenticationDataRequest,
                                       CustomXMLParserDelegate<PushAuthenticationDataRequest>  CustomPushAuthenticationDataRequestParser   = null,
                                       CustomXMLParserDelegate<ProviderAuthenticationData>     CustomProviderAuthenticationDataParser      = null,
                                       CustomXMLParserDelegate<Identification>                 CustomIdentificationParser                  = null,
                                       CustomXMLParserDelegate<RFIDIdentification>             CustomRFIDIdentificationParser              = null,
                                       OnExceptionDelegate                                     OnException                                 = null,

                                       DateTime?                                               Timestamp                                   = null,
                                       CancellationToken?                                      CancellationToken                           = null,
                                       EventTracking_Id                                        EventTrackingId                             = null,
                                       TimeSpan?                                               RequestTimeout                              = null)

        {

            try
            {

                if (PushAuthenticationDataRequestXML.Name != OICPNS.AuthenticationData + "eRoamingPushAuthenticationData")
                {
                    PushAuthenticationDataRequest = null;
                    return false;
                }

                PushAuthenticationDataRequest = new PushAuthenticationDataRequest(

                                                    PushAuthenticationDataRequestXML.MapElement    (OICPNS.AuthenticationData + "ProviderAuthenticationData",
                                                                                                    (xml, e) => ProviderAuthenticationData.Parse(xml,
                                                                                                                                                 CustomProviderAuthenticationDataParser,
                                                                                                                                                 CustomIdentificationParser,
                                                                                                                                                 CustomRFIDIdentificationParser,
                                                                                                                                                 e),
                                                                                                    OnException),

                                                    PushAuthenticationDataRequestXML.MapValueOrFail(OICPNS.AuthenticationData + "ActionType",
                                                                                                    XML_IO.AsActionType),

                                                    Timestamp,
                                                    CancellationToken,
                                                    EventTrackingId,
                                                    RequestTimeout);


                if (CustomPushAuthenticationDataRequestParser != null)
                    PushAuthenticationDataRequest = CustomPushAuthenticationDataRequestParser(PushAuthenticationDataRequestXML,
                                                                                              PushAuthenticationDataRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, PushAuthenticationDataRequestXML, e);

                PushAuthenticationDataRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(PushAuthenticationDataRequestText, out PushAuthenticationDataRequest, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text representation of an OICP push authentication data request.
        /// </summary>
        /// <param name="PushAuthenticationDataRequestText">The text to parse.</param>
        /// <param name="PushAuthenticationDataRequest">The parsed push authentication data request.</param>
        /// <param name="CustomPushAuthenticationDataRequestParser">A delegate to parse custom PushAuthenticationData requests.</param>
        /// <param name="CustomProviderAuthenticationDataParser">A delegate to parse custom ProviderAuthenticationData XML elements.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                                  PushAuthenticationDataRequestText,
                                       out PushAuthenticationDataRequest                       PushAuthenticationDataRequest,
                                       CustomXMLParserDelegate<PushAuthenticationDataRequest>  CustomPushAuthenticationDataRequestParser   = null,
                                       CustomXMLParserDelegate<ProviderAuthenticationData>     CustomProviderAuthenticationDataParser      = null,
                                       CustomXMLParserDelegate<Identification>                 CustomIdentificationParser                  = null,
                                       CustomXMLParserDelegate<RFIDIdentification>             CustomRFIDIdentificationParser              = null,
                                       OnExceptionDelegate                                     OnException                                 = null,

                                       DateTime?                                               Timestamp                                   = null,
                                       CancellationToken?                                      CancellationToken                           = null,
                                       EventTracking_Id                                        EventTrackingId                             = null,
                                       TimeSpan?                                               RequestTimeout                              = null)

        {

            try
            {

                if (TryParse(XDocument.Parse(PushAuthenticationDataRequestText).Root,
                             out PushAuthenticationDataRequest,
                             CustomPushAuthenticationDataRequestParser,
                             CustomProviderAuthenticationDataParser,
                             CustomIdentificationParser,
                             CustomRFIDIdentificationParser,
                             OnException,

                             Timestamp,
                             CancellationToken,
                             EventTrackingId,
                             RequestTimeout))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, PushAuthenticationDataRequestText, e);
            }

            PushAuthenticationDataRequest = null;
            return false;

        }

        #endregion

        #region ToXML(CustomPushAuthenticationDataRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomPushAuthenticationDataRequestSerializer">A delegate to customize the serialization of PushAuthenticationDataRequest requests.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<PushAuthenticationDataRequest>  CustomPushAuthenticationDataRequestSerializer   = null)
        {

            var XML = new XElement(OICPNS.AuthenticationData + "eRoamingPushAuthenticationData",

                          new XElement(OICPNS.AuthenticationData + "ActionType",  XML_IO.AsText(OICPAction)),

                          ProviderAuthenticationData.ToXML()


                      );

            return CustomPushAuthenticationDataRequestSerializer != null
                       ? CustomPushAuthenticationDataRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PushAuthenticationDataRequest1, PushAuthenticationDataRequest2)

        /// <summary>
        /// Compares two push authentication data requests for equality.
        /// </summary>
        /// <param name="PushAuthenticationDataRequest1">A push authentication data request.</param>
        /// <param name="PushAuthenticationDataRequest2">Another push authentication data request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PushAuthenticationDataRequest PushAuthenticationDataRequest1, PushAuthenticationDataRequest PushAuthenticationDataRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PushAuthenticationDataRequest1, PushAuthenticationDataRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PushAuthenticationDataRequest1 == null) || ((Object) PushAuthenticationDataRequest2 == null))
                return false;

            return PushAuthenticationDataRequest1.Equals(PushAuthenticationDataRequest2);

        }

        #endregion

        #region Operator != (PushAuthenticationDataRequest1, PushAuthenticationDataRequest2)

        /// <summary>
        /// Compares two push authentication data requests for inequality.
        /// </summary>
        /// <param name="PushAuthenticationDataRequest1">A push authentication data request.</param>
        /// <param name="PushAuthenticationDataRequest2">Another push authentication data request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PushAuthenticationDataRequest PushAuthenticationDataRequest1, PushAuthenticationDataRequest PushAuthenticationDataRequest2)

            => !(PushAuthenticationDataRequest1 == PushAuthenticationDataRequest2);

        #endregion

        #endregion

        #region IEquatable<PushAuthenticationDataRequest> Members

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

            var PushAuthenticationDataRequest = Object as PushAuthenticationDataRequest;
            if ((Object) PushAuthenticationDataRequest == null)
                return false;

            return Equals(PushAuthenticationDataRequest);

        }

        #endregion

        #region Equals(PushAuthenticationDataRequest)

        /// <summary>
        /// Compares two push authentication data requests for equality.
        /// </summary>
        /// <param name="PushAuthenticationDataRequest">A push authentication data request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PushAuthenticationDataRequest PushAuthenticationDataRequest)
        {

            if ((Object) PushAuthenticationDataRequest == null)
                return false;

            return ProviderAuthenticationData.Equals(PushAuthenticationDataRequest.ProviderAuthenticationData) &&
                   ProviderId.                Equals(PushAuthenticationDataRequest.ProviderId)                 &&
                   OICPAction.                Equals(PushAuthenticationDataRequest.OICPAction);

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

                return ProviderAuthenticationData.GetHashCode() * 5 ^
                       ProviderId.                GetHashCode() * 3 ^
                       OICPAction.                GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("'", OICPAction, "' ",
                             " of ",
                             ProviderAuthenticationData.ToString());

        #endregion


    }

}
