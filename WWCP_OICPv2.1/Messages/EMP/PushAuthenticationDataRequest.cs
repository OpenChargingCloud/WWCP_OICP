/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// An OICP push authentication data request.
    /// </summary>
    public class PushAuthenticationDataRequest : ARequest<PushAuthenticationDataRequest>
    {

        #region Properties

        /// <summary>
        /// An enumeration of authorization identifications.
        /// </summary>
        public IEnumerable<AuthorizationIdentification>  AuthorizationIdentifications   { get; }

        /// <summary>
        /// An e-mobility provider identification.
        /// </summary>
        public Provider_Id                               ProviderId                     { get; }

        /// <summary>
        /// The server-side data management operation.
        /// </summary>
        public ActionTypes                               OICPAction                     { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP PushAuthenticationDataRequest XML/SOAP request.
        /// </summary>
        /// <param name="AuthorizationIdentifications">An enumeration of authorization identifications.</param>
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        public PushAuthenticationDataRequest(IEnumerable<AuthorizationIdentification>  AuthorizationIdentifications,
                                             Provider_Id                               ProviderId,
                                             ActionTypes                               OICPAction          = ActionTypes.fullLoad,

                                             DateTime?                                 Timestamp           = null,
                                             CancellationToken?                        CancellationToken   = null,
                                             EventTracking_Id                          EventTrackingId     = null,
                                             TimeSpan?                                 RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            #region Initial checks

            if (AuthorizationIdentifications == null)
                throw new ArgumentNullException(nameof(AuthorizationIdentifications), "The given enumeration of authorization identifications must not be null!");

            #endregion

            this.AuthorizationIdentifications  = AuthorizationIdentifications;
            this.ProviderId                    = ProviderId;
            this.OICPAction                    = OICPAction;

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
        //             <AuthenticationData:ProviderID>?</AuthenticationData:ProviderID>
        // 
        //             <!--Zero or more repetitions:-->
        //             <AuthenticationData:AuthenticationDataRecord>
        //                <AuthenticationData:Identification>
        // 
        //                   <!--You have a CHOICE of the next 4 items at this level-->
        //                   <CommonTypes:RFIDmifarefamilyIdentification>
        //                      <CommonTypes:UID>?</CommonTypes:UID>
        //                   </CommonTypes:RFIDmifarefamilyIdentification>
        // 
        //                   <CommonTypes:QRCodeIdentification>
        // 
        //                      <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        // 
        //                      <!--You have a CHOICE of the next 2 items at this level-->
        //                      <CommonTypes:PIN>?</CommonTypes:PIN>
        // 
        //                      <CommonTypes:HashedPIN>
        //                         <CommonTypes:Value>?</CommonTypes:Value>
        //                         <CommonTypes:Function>?</CommonTypes:Function>
        //                         <CommonTypes:Salt>?</CommonTypes:Salt>
        //                      </CommonTypes:HashedPIN>
        // 
        //                   </CommonTypes:QRCodeIdentification>
        // 
        //                   <CommonTypes:PlugAndChargeIdentification>
        //                      <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //                   </CommonTypes:PlugAndChargeIdentification>
        // 
        //                   <CommonTypes:RemoteIdentification>
        //                      <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
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

        #region (static) Parse(PushAuthenticationDataRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP push authentication data request.
        /// </summary>
        /// <param name="PushAuthenticationDataRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static PushAuthenticationDataRequest Parse(XElement             PushAuthenticationDataRequestXML,
                                                          OnExceptionDelegate  OnException = null)
        {

            PushAuthenticationDataRequest _PushAuthenticationDataRequest;

            if (TryParse(PushAuthenticationDataRequestXML, out _PushAuthenticationDataRequest, OnException))
                return _PushAuthenticationDataRequest;

            return null;

        }

        #endregion

        #region (static) Parse(PushAuthenticationDataRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP push authentication data request.
        /// </summary>
        /// <param name="PushAuthenticationDataRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static PushAuthenticationDataRequest Parse(String               PushAuthenticationDataRequestText,
                                                          OnExceptionDelegate  OnException = null)
        {

            PushAuthenticationDataRequest _PushAuthenticationDataRequest;

            if (TryParse(PushAuthenticationDataRequestText, out _PushAuthenticationDataRequest, OnException))
                return _PushAuthenticationDataRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(PushAuthenticationDataRequestXML,  out PushAuthenticationDataRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP push authentication data request.
        /// </summary>
        /// <param name="PushAuthenticationDataRequestXML">The XML to parse.</param>
        /// <param name="PushAuthenticationDataRequest">The parsed push authentication data request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                           PushAuthenticationDataRequestXML,
                                       out PushAuthenticationDataRequest  PushAuthenticationDataRequest,
                                       OnExceptionDelegate                OnException  = null)
        {

            try
            {

                var ProviderAuthenticationDataXML = PushAuthenticationDataRequestXML.ElementOrFail(OICPNS.AuthenticationData + "ProviderAuthenticationData");

                PushAuthenticationDataRequest = new PushAuthenticationDataRequest(

                                                    ProviderAuthenticationDataXML.   MapElements   (OICPNS.AuthenticationData + "AuthenticationDataRecord",
                                                                                                    AuthorizationIdentification.Parse),

                                                    PushAuthenticationDataRequestXML.MapValueOrFail(OICPNS.AuthenticationData + "ProviderID",
                                                                                                    Provider_Id.Parse),

                                                    ProviderAuthenticationDataXML.   MapValueOrFail(OICPNS.AuthenticationData + "ActionType",
                                                                                                    XML_IO.AsActionType)

                                                );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, PushAuthenticationDataRequestXML, e);

                PushAuthenticationDataRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(PushAuthenticationDataRequestText, out PushAuthenticationDataRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP push authentication data request.
        /// </summary>
        /// <param name="PushAuthenticationDataRequestText">The text to parse.</param>
        /// <param name="PushAuthenticationDataRequest">The parsed push authentication data request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                             PushAuthenticationDataRequestText,
                                       out PushAuthenticationDataRequest  PushAuthenticationDataRequest,
                                       OnExceptionDelegate                OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(PushAuthenticationDataRequestText).Root,
                             out PushAuthenticationDataRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, PushAuthenticationDataRequestText, e);
            }

            PushAuthenticationDataRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OICPNS.AuthenticationData + "eRoamingPushAuthenticationData",

                   new XElement(OICPNS.AuthenticationData + "ActionType",      XML_IO.AsText(OICPAction)),

                   new XElement(OICPNS.AuthenticationData + "ProviderAuthenticationData",

                       new XElement(OICPNS.AuthenticationData + "ProviderID",  ProviderId.ToString()),

                       AuthorizationIdentifications.
                           SafeSelect(AuthorizationIdentification => new XElement(OICPNS.AuthenticationData + "AuthenticationDataRecord",
                                                                                  AuthorizationIdentification.ToXML(OICPNS.AuthenticationData)))

                   )

               );

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
            if (Object.ReferenceEquals(PushAuthenticationDataRequest1, PushAuthenticationDataRequest2))
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

            return AuthorizationIdentifications.Count().Equals(PushAuthenticationDataRequest.AuthorizationIdentifications.Count()) &&
                   ProviderId.                          Equals(PushAuthenticationDataRequest.ProviderId) &&
                   OICPAction.                          Equals(PushAuthenticationDataRequest.OICPAction);

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

                return AuthorizationIdentifications.GetHashCode() * 5  ^
                       ProviderId.                  GetHashCode() * 17 ^
                       OICPAction.                  GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(OICPAction, " ",
                             AuthorizationIdentifications, " authorization identification(s)",
                             " (", ProviderId, ")");

        #endregion


    }

}
