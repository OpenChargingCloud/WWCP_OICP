/*
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.CPO
{

    /// <summary>
    /// An OICP push EVSE data response.
    /// </summary>
    public class PushEVSEDataResponse : AResponse<PushEVSEDataRequest,
                                                  PushEVSEDataResponse>
    {

        #region Properties

        /// <summary>
        /// Wether the response has a result.
        /// </summary>
        public Boolean             HasResult          { get; }

        /// <summary>
        /// The status code of the response.
        /// </summary>
        public StatusCode          StatusCode         { get; }

        /// <summary>
        /// An optional charging session identification.
        /// </summary>
        public Session_Id?         SessionId          { get; }

        /// <summary>
        /// An optional partner charging session identification.
        /// </summary>
        public PartnerSession_Id?  PartnerSessionId   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP push EVSE data response.
        /// </summary>
        /// <param name="Request">The push EVSE data request leading to this response.</param>
        /// <param name="HasResult">Wether the response has a result.</param>
        /// <param name="StatusCode">The status code of the response.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public PushEVSEDataResponse(PushEVSEDataRequest  Request,
                                    Boolean              HasResult,
                                    StatusCode           StatusCode,
                                    Session_Id?          SessionId         = null,
                                    PartnerSession_Id?   PartnerSessionId  = null)

            : base(Request)

        {

            this.HasResult         = HasResult;
            this.StatusCode        = StatusCode;
            this.SessionId         = SessionId;
            this.PartnerSessionId  = PartnerSessionId;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:SoapEnv     = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //    <SoapEnv:Header/>
        //
        //    <SoapEnv:Body>
        //       <CommonTypes:eRoamingAcknowledgement>
        // 
        //          <CommonTypes:Result>?</CommonTypes:Result>
        // 
        //          <CommonTypes:StatusCode>
        //
        //             <CommonTypes:Code>?</CommonTypes:Code>
        //
        //             <!--Optional:-->
        //             <CommonTypes:Description>?</CommonTypes:Description>
        //
        //             <!--Optional:-->
        //             <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
        //
        //          </CommonTypes:StatusCode>
        // 
        //          <!--Optional:-->
        //          <CommonTypes:SessionID>?</CommonTypes:SessionID>
        // 
        //          <!--Optional:-->
        //          <CommonTypes:PartnerSessionID>?</CommonTypes:PartnerSessionID>
        // 
        //       </CommonTypes:eRoamingAcknowledgement>
        //    </SoapEnv:Body>
        //
        // </SoapEnv:Envelope>

        #endregion

        #region (static) Parse   (Request, PushEVSEDataResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP push EVSE data response.
        /// </summary>
        /// <param name="Request">The push EVSE data request leading to this response.</param>
        /// <param name="PushEVSEDataResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static PushEVSEDataResponse Parse(PushEVSEDataRequest  Request,
                                                 XElement             PushEVSEDataResponseXML,
                                                 OnExceptionDelegate  OnException = null)
        {

            PushEVSEDataResponse _PushEVSEDataResponse;

            if (TryParse(Request, PushEVSEDataResponseXML, out _PushEVSEDataResponse, OnException))
                return _PushEVSEDataResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, PushEVSEDataResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP push EVSE data response.
        /// </summary>
        /// <param name="Request">The push EVSE data request leading to this response.</param>
        /// <param name="PushEVSEDataResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static PushEVSEDataResponse Parse(PushEVSEDataRequest  Request,
                                                 String               PushEVSEDataResponseText,
                                                 OnExceptionDelegate  OnException = null)
        {

            PushEVSEDataResponse _PushEVSEDataResponse;

            if (TryParse(Request, PushEVSEDataResponseText, out _PushEVSEDataResponse, OnException))
                return _PushEVSEDataResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, PushEVSEDataResponseXML,  out PushEVSEDataResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP push EVSE data response.
        /// </summary>
        /// <param name="Request">The push EVSE data request leading to this response.</param>
        /// <param name="PushEVSEDataResponseXML">The XML to parse.</param>
        /// <param name="PushEVSEDataResponse">The parsed push EVSE data response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(PushEVSEDataRequest       Request,
                                       XElement                  PushEVSEDataResponseXML,
                                       out PushEVSEDataResponse  PushEVSEDataResponse,
                                       OnExceptionDelegate       OnException  = null)
        {

            try
            {

                PushEVSEDataResponse = new PushEVSEDataResponse(

                                           Request,

                                           PushEVSEDataResponseXML.MapValueOrFail    (OICPNS.CommonTypes + "Result",
                                                                                      s => s == "true"),

                                           PushEVSEDataResponseXML.MapElementOrFail  (OICPNS.CommonTypes + "StatusCode",
                                                                                      StatusCode.Parse,
                                                                                      OnException),

                                           PushEVSEDataResponseXML.MapValueOrNullable(OICPNS.CommonTypes + "SessionID",
                                                                                      Session_Id.Parse),

                                           PushEVSEDataResponseXML.MapValueOrNullable(OICPNS.CommonTypes + "PartnerSessionID",
                                                                                      PartnerSession_Id.Parse)

                                       );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, PushEVSEDataResponseXML, e);

                PushEVSEDataResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, PushEVSEDataResponseText, out PushEVSEDataResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP push EVSE data response.
        /// </summary>
        /// <param name="Request">The push EVSE data request leading to this response.</param>
        /// <param name="PushEVSEDataResponseText">The text to parse.</param>
        /// <param name="PushEVSEDataResponse">The parsed push EVSE data response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(PushEVSEDataRequest       Request,
                                       String                    PushEVSEDataResponseText,
                                       out PushEVSEDataResponse  PushEVSEDataResponse,
                                       OnExceptionDelegate       OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(PushEVSEDataResponseText).Root,
                             out PushEVSEDataResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, PushEVSEDataResponseText, e);
            }

            PushEVSEDataResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                   new XElement(OICPNS.CommonTypes + "Result",                  Result),

                   StatusCode.ToXML(),

                   SessionId.HasValue
                       ? new XElement(OICPNS.CommonTypes + "SessionID",         SessionId.       ToString())
                       : null,

                   PartnerSessionId.HasValue
                       ? new XElement(OICPNS.CommonTypes + "PartnerSessionID",  PartnerSessionId.ToString())
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (PushEVSEDataResponse1, PushEVSEDataResponse2)

        /// <summary>
        /// Compares two push EVSE data responses for equality.
        /// </summary>
        /// <param name="PushEVSEDataResponse1">A push EVSE data response.</param>
        /// <param name="PushEVSEDataResponse2">Another push EVSE data response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PushEVSEDataResponse PushEVSEDataResponse1, PushEVSEDataResponse PushEVSEDataResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(PushEVSEDataResponse1, PushEVSEDataResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PushEVSEDataResponse1 == null) || ((Object) PushEVSEDataResponse2 == null))
                return false;

            return PushEVSEDataResponse1.Equals(PushEVSEDataResponse2);

        }

        #endregion

        #region Operator != (PushEVSEDataResponse1, PushEVSEDataResponse2)

        /// <summary>
        /// Compares two push EVSE data responses for inequality.
        /// </summary>
        /// <param name="PushEVSEDataResponse1">A push EVSE data response.</param>
        /// <param name="PushEVSEDataResponse2">Another push EVSE data response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PushEVSEDataResponse PushEVSEDataResponse1, PushEVSEDataResponse PushEVSEDataResponse2)

            => !(PushEVSEDataResponse1 == PushEVSEDataResponse2);

        #endregion

        #endregion

        #region IEquatable<PushEVSEDataResponse> Members

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

            var PushEVSEDataResponse = Object as PushEVSEDataResponse;
            if ((Object) PushEVSEDataResponse == null)
                return false;

            return Equals(PushEVSEDataResponse);

        }

        #endregion

        #region Equals(PushEVSEDataResponse)

        /// <summary>
        /// Compares two push EVSE data responses for equality.
        /// </summary>
        /// <param name="PushEVSEDataResponse">A push EVSE data response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PushEVSEDataResponse PushEVSEDataResponse)
        {

            if ((Object) PushEVSEDataResponse == null)
                return false;

            return HasResult. Equals(PushEVSEDataResponse.HasResult)  &&
                   StatusCode.Equals(PushEVSEDataResponse.StatusCode) &&

                   ((!SessionId.       HasValue && !PushEVSEDataResponse.SessionId.       HasValue) ||
                     (SessionId.       HasValue &&  PushEVSEDataResponse.SessionId.       HasValue && SessionId.       Value.Equals(PushEVSEDataResponse.SessionId.       Value))) &&

                   ((!PartnerSessionId.HasValue && !PushEVSEDataResponse.PartnerSessionId.HasValue) ||
                     (PartnerSessionId.HasValue &&  PushEVSEDataResponse.PartnerSessionId.HasValue && PartnerSessionId.Value.Equals(PushEVSEDataResponse.PartnerSessionId.Value)));

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

                return HasResult. GetHashCode() * 7 ^
                       StatusCode.GetHashCode() * 5 ^

                       (SessionId.HasValue
                            ? SessionId.       GetHashCode() * 3
                            : 0) ^

                       (PartnerSessionId.HasValue
                            ? PartnerSessionId.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(StatusCode,

                             SessionId.HasValue
                                 ? ", " + SessionId.Value
                                 : "",

                             PartnerSessionId.HasValue
                                 ? ", " + PartnerSessionId.Value
                                 : "");

        #endregion


    }

}
