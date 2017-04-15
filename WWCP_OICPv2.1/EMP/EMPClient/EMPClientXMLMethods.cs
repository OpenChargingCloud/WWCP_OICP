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
using System.Globalization;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// OICP EMP client XML methods.
    /// </summary>
    public static class EMPClientXMLMethods
    {

        #region GetEVSEByIdRequestXML(EVSEId) // <- Note!

        // Note: It's confusing, but this request does not belong here!
        //       It must be omplemented on the CPO client side!

        ///// <summary>
        ///// Create a new Get-EVSE-By-Id request.
        ///// In case that CPOs do not upload EVSE data to Hubject, Hubject requests specific EVSE data on demand.
        ///// </summary>
        ///// <param name="EVSEId">An unique EVSE identification.</param>
        //public static XElement GetEVSEByIdRequestXML(EVSE_Id  EVSEId)
        //{

        //    #region Documentation

        //    // <soapenv:Envelope xmlns:soapenv  = "http://schemas.xmlsoap.org/soap/envelope/"
        //    //                   xmlns:EVSEData = "http://www.hubject.com/b2b/services/evsedata/v2.0">
        //    //
        //    //    <soapenv:Header/>
        //    //    <soapenv:Body>
        //    //       <EVSEData:eRoamingGetEvseById>
        //    //
        //    //          <EVSEData:EvseId>+49*123*1234567*1</EVSEData:EvseId>
        //    //
        //    //       </EVSEData:eRoamingGetEvseById>
        //    //    </soapenv:Body>
        //    // </soapenv:Envelope>

        //    #endregion

        //    #region Initial checks

        //    if (EVSEId == null)
        //        throw new ArgumentNullException(nameof(EVSEId), "The given parameter must not be null!");

        //    #endregion

        //    return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingGetEvseById",
        //                                  new XElement(OICPNS.EVSEData + "EvseId", EVSEId.OriginId)
        //                             ));

        //}

        #endregion


    }

}
