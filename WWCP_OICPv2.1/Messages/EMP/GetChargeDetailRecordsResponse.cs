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
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// An OICP get charge detail records response.
    /// </summary>
    public class GetChargeDetailRecordsResponse : AResponse<GetChargeDetailRecordsRequest,
                                                            GetChargeDetailRecordsResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of charge detail records.
        /// </summary>
        public IEnumerable<ChargeDetailRecord>  ChargeDetailRecords   { get; }


        public StatusCode                       StatusCode            { get; }

        #endregion

        #region Constructor(s)

        #region GetChargeDetailRecordsResponse(Request, ChargeDetailRecords, ...)

        /// <summary>
        /// Create a new group of OICP Charge Detail Records.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        public GetChargeDetailRecordsResponse(GetChargeDetailRecordsRequest    Request,
                                              IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                                              StatusCode                       StatusCode    = null,
                                              CustomMapper2Delegate<Builder>   CustomMapper  = null)

            : base(Request)

        {

            #region Initial checks

            if (ChargeDetailRecords == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecords), "The given enumeration of charge detail records must not be null!");

            #endregion

            this.ChargeDetailRecords  = ChargeDetailRecords;
            this.StatusCode           = StatusCode;

            if (CustomMapper != null)
            {

                var Builder = CustomMapper.Invoke(new Builder(this));

                this.ChargeDetailRecords  = Builder.ChargeDetailRecords;
                this.CustomData           = Builder.CustomData;

            }

        }

        #endregion

        #region GetChargeDetailRecordsResponse(StatusCode, ...)

        /// <summary>
        /// Create a new OICP 'negative' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCode">The status code of the operation.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public GetChargeDetailRecordsResponse(GetChargeDetailRecordsRequest   Request,
                                              StatusCodes                     StatusCode,
                                              String                          StatusCodeDescription     = null,
                                              String                          StatusCodeAdditionalInfo  = null,
                                              CustomMapper2Delegate<Builder>  CustomMapper              = null)

            : this(Request,
                   null,
                   new StatusCode(StatusCode,
                                  StatusCodeDescription,
                                  StatusCodeAdditionalInfo),
                   CustomMapper)

        { }

        #endregion

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0"
        //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        // 
        //    <soapenv:Header/>
        // 
        //    <soapenv:Body>
        //       <Authorization:eRoamingChargeDetailRecords>
        // 
        //          <!--Zero or more repetitions:-->
        //          <Authorization:eRoamingChargeDetailRecord>
        //             [...]
        //          </Authorization:eRoamingChargeDetailRecord>
        // 
        //       </Authorization:eRoamingChargeDetailRecords>
        //    </soapenv:Body>
        // 
        // </soapenv:Envelope>

        #endregion

        #region (static) ParseXML(eRoamingChargeDetailRecordsXML, OnException = null)

        /// <summary>
        /// Parse the givem XML as an OICP charge detail records.
        /// </summary>
        /// <param name="ChargeDetailRecordsXML">A XML representation of an enumeration of OICP charge detail records.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetChargeDetailRecordsResponse ParseXML(GetChargeDetailRecordsRequest                                  Request,
                                                              XElement                                                       ChargeDetailRecordsXML,
                                                              CustomMapperDelegate<GetChargeDetailRecordsResponse, Builder>  CustomObjectMapper              = null,
                                                              CustomParserDelegate<ChargeDetailRecord>                       CustomChargeDetailRecordParser  = null,
                                                              OnExceptionDelegate                                            OnException                     = null)
        {

            if (ChargeDetailRecordsXML.Name != OICPNS.Authorization + "eRoamingChargeDetailRecords")
                throw new Exception("Invalid eRoamingChargeDetailRecords XML!");

            return new GetChargeDetailRecordsResponse(

                       Request,

                       ChargeDetailRecordsXML.MapElements(OICPNS.Authorization + "eRoamingChargeDetailRecord",
                                                          (XML, e) => ChargeDetailRecord.Parse(XML, CustomChargeDetailRecordParser, e),
                                                          OnException),

                       null,

                       responsebuilder => CustomObjectMapper != null
                                              ? CustomObjectMapper(ChargeDetailRecordsXML, responsebuilder)
                                              : responsebuilder

                   );

        }

        #endregion


        public override bool Equals(GetChargeDetailRecordsResponse AResponse)
        {
            throw new NotImplementedException();
        }


        public class Builder
        {

            #region Properties

            /// <summary>
            /// An enumeration of charge detail records.
            /// </summary>
            public IEnumerable<ChargeDetailRecord> ChargeDetailRecords { get; set; }

            public Dictionary<String, Object> CustomData { get; set; }

            #endregion

            public Builder(GetChargeDetailRecordsResponse GetChargeDetailRecordsResponse = null)
            {

                this.ChargeDetailRecords  = GetChargeDetailRecordsResponse.ChargeDetailRecords;
                this.CustomData           = new Dictionary<String, Object>();

                if (GetChargeDetailRecordsResponse.CustomData != null)
                    foreach (var item in GetChargeDetailRecordsResponse.CustomData)
                        CustomData.Add(item.Key, item.Value);

            }


            //public GetChargeDetailRecordsResponse ToImmutable()

            //    => new GetChargeDetailRecordsResponse(Request,
            //                                          Result,
            //                                          StatusCode,
            //                                          SessionId,
            //                                          PartnerSessionId,
            //                                          CustomData);

        }


    }

}
