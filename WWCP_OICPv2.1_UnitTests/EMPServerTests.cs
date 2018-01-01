/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.UnitTests
{

    public static class EMPServerTests
    {

        public static async Task<String> SendAuthorizeStart(String     Hostname,
                                                            IPPort     TCPPort,
                                                            String     URI,
                                                            DNSClient  DNSClient)
        {

            var response = await new HTTPClient(Hostname,
                                                TCPPort,
                                                DNSClient: DNSClient).

                                     Execute(client => client.POST(URI,

                                                                   requestbuilder => {
                                                                       requestbuilder.Host = Hostname;
                                                                       //requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                       requestbuilder.ContentType = HTTPContentType.XMLTEXT_UTF8;
                                                                       requestbuilder.Content = ("<?xml version = '1.0' encoding='UTF-8'?>" +
                                                                                                @"<soapenv:Envelope xmlns:cmn=""http://www.hubject.com/b2b/services/commontypes/v2.0""" +
                                                                                                @"                  xmlns:fn=""http://www.w3.org/2005/xpath-functions""" +
                                                                                                @"                  xmlns:isns=""http://schemas.xmlsoap.org/soap/envelope/""" +
                                                                                                @"                  xmlns:sbp=""http://www.inubit.com/eMobility/SBP""" +
                                                                                                @"                  xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""" +
                                                                                                @"                  xmlns:tns=""http://www.hubject.com/b2b/services/authorization/v2.0"">" +
                                                                                                "" +
                                                                                                "  <soapenv:Body>" +
                                                                                                "    <tns:eRoamingAuthorizeStart>" +
                                                                                                "      <tns:SessionID>29f5d1c3-0a88-1295-3d5c-bdaa4b1b8078</tns:SessionID>" +
                                                                                                "      <tns:OperatorID>+49*839</tns:OperatorID>" +
                                                                                                "      <tns:EVSEID>+49*839*678014123753</tns:EVSEID>" +
                                                                                                "      <tns:Identification>" +
                                                                                                "        <cmn:RFIDmifarefamilyIdentification>" +
                                                                                                "          <cmn:UID>0499F822173C80</cmn:UID>" +
                                                                                                "        </cmn:RFIDmifarefamilyIdentification>" +
                                                                                                "      </tns:Identification>" +
                                                                                                "    </tns:eRoamingAuthorizeStart>" +
                                                                                                "  </soapenv:Body>" +
                                                                                                "" +
                                                                                                "</soapenv:Envelope>").ToUTF8Bytes();
                                                                   }),

                                             RequestTimeout:     TimeSpan.FromSeconds(10),
                                             CancellationToken:  new CancellationTokenSource().Token);


            // HTTP / 1.1 200 OK
            // Date: Wed, 10 Feb 2016 16:10:01 GMT
            // Server: Apache / 2.2.16(Debian)
            // Content - Length: 74
            // Content - Type: application / json
            // 
            // { "EvseStatus":{ "\"49*822*483*1\"":{ "2016-02-10T17:09:28CET":"AVAILABLE"} } }

            if (response.HTTPStatusCode != HTTPStatusCode.OK)
                return "ERROR!";

            return response.HTTPBody.ToUTF8String();

        }

    }

}
