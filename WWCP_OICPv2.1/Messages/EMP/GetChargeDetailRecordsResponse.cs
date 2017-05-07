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


        public StatusCode?                      StatusCode            { get; }

        #endregion

        #region Constructor(s)

        #region GetChargeDetailRecordsResponse(Request, ChargeDetailRecords, StatusCode = null, CustomData = null)

        /// <summary>
        /// Create a new group of OICP Charge Detail Records.
        /// </summary>
        /// <param name="Request">A GetChargeDetailRecords request.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="StatusCode">An optional status code for this request.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public GetChargeDetailRecordsResponse(GetChargeDetailRecordsRequest        Request,
                                              IEnumerable<ChargeDetailRecord>      ChargeDetailRecords,
                                              StatusCode?                          StatusCode  = null,
                                              IReadOnlyDictionary<String, Object>  CustomData  = null)

            : base(Request,
                   CustomData)

        {

            #region Initial checks

            if (ChargeDetailRecords == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecords),  "The given enumeration of charge detail records must not be null!");

            #endregion

            this.ChargeDetailRecords  = ChargeDetailRecords;
            this.StatusCode           = StatusCode;

        }

        #endregion

        #region GetChargeDetailRecordsResponse(Request, StatusCode, Description = null, AdditionalInfo = null, CustomData = null)

        /// <summary>
        /// Create a new OICP 'negative' acknowledgement.
        /// </summary>
        /// <param name="Request">A GetChargeDetailRecords request.</param>
        /// <param name="StatusCode">The status code of the operation.</param>
        /// <param name="Description">An optional description of the status code.</param>
        /// <param name="AdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public GetChargeDetailRecordsResponse(GetChargeDetailRecordsRequest        Request,
                                              StatusCodes                          StatusCode,
                                              String                               Description      = null,
                                              String                               AdditionalInfo   = null,
                                              IReadOnlyDictionary<String, Object>  CustomData       = null)

            : this(Request,
                   new ChargeDetailRecord[0],
                   new StatusCode(StatusCode,
                                  Description,
                                  AdditionalInfo),
                   CustomData)

        { }

        #endregion

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv        = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Authorization  = "http://www.hubject.com/b2b/services/authorization/v2.0"
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
        public static GetChargeDetailRecordsResponse ParseXML(GetChargeDetailRecordsRequest                         Request,
                                                              XElement                                              ChargeDetailRecordsXML,
                                                              CustomXMLParserDelegate<GetChargeDetailRecordsResponse>  CustomGetChargeDetailRecordsResponseParser   = null,
                                                              CustomXMLParserDelegate<ChargeDetailRecord>              CustomChargeDetailRecordParser               = null,
                                                              CustomXMLParserDelegate<Identification>                  CustomIdentificationParser                   = null,
                                                              OnExceptionDelegate                                   OnException                                  = null)
        {

            if (ChargeDetailRecordsXML.Name != OICPNS.Authorization + "eRoamingChargeDetailRecords")
                throw new Exception("Invalid eRoamingChargeDetailRecords XML!");

            var GetChargeDetailRecordsResponse =  new GetChargeDetailRecordsResponse(

                                                      Request,

                                                      ChargeDetailRecordsXML.MapElements(OICPNS.Authorization + "eRoamingChargeDetailRecord",
                                                                                         (XML, e) => ChargeDetailRecord.Parse(XML,
                                                                                                                              CustomChargeDetailRecordParser,
                                                                                                                              CustomIdentificationParser,
                                                                                                                              e),
                                                                                         OnException)

                                                  );


            return CustomGetChargeDetailRecordsResponseParser != null
                       ? CustomGetChargeDetailRecordsResponseParser(ChargeDetailRecordsXML,
                                                                    GetChargeDetailRecordsResponse)
                       : GetChargeDetailRecordsResponse;

        }

        #endregion


        #region Equals(GetChargeDetailRecordsResponse)

        /// <summary>
        /// Compares two GetChargeDetailRecords responses for equality.
        /// </summary>
        /// <param name="GetChargeDetailRecordsResponse">A GetChargeDetailRecords response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetChargeDetailRecordsResponse GetChargeDetailRecordsResponse)
        {

            if ((Object) GetChargeDetailRecordsResponse == null)
                return false;

            return (!ChargeDetailRecords.Any() && !GetChargeDetailRecordsResponse.ChargeDetailRecords.Any()) ||
                    (ChargeDetailRecords.Any() && GetChargeDetailRecordsResponse.ChargeDetailRecords.Any() && ChargeDetailRecords.Count().Equals(GetChargeDetailRecordsResponse.ChargeDetailRecords.Count())) &&

                    (StatusCode != null && GetChargeDetailRecordsResponse.StatusCode != null) ||
                    (StatusCode == null && GetChargeDetailRecordsResponse.StatusCode == null && StatusCode.Equals(GetChargeDetailRecordsResponse.StatusCode));

        }

        #endregion


        #region ToBuilder

        /// <summary>
        /// Return a response builder.
        /// </summary>
        public Builder ToBuilder
            => new Builder(this);

        #endregion

        #region (class) Builder

        /// <summary>
        /// An AuthorizationStartById response builder.
        /// </summary>
        public class Builder : AResponseBuilder<GetChargeDetailRecordsRequest,
                                                GetChargeDetailRecordsResponse>
        {

            #region Properties

            /// <summary>
            /// An enumeration of charge detail records.
            /// </summary>
            public IEnumerable<ChargeDetailRecord>  ChargeDetailRecords   { get; set; }


            public StatusCode?                      StatusCode            { get; set; }

            #endregion

            #region Constructor(s)

            #region Builder(Request,        CustomData = null)

            /// <summary>
            /// Create a new GetChargeDetailRecords response builder.
            /// </summary>
            /// <param name="Request">A GetChargeDetailRecords request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(GetChargeDetailRecordsRequest        Request,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(Request,
                       CustomData)

            { }

            #endregion

            #region Builder(AuthorizationStartById, CustomData = null)

            /// <summary>
            /// Create a new GetChargeDetailRecords response builder.
            /// </summary>
            /// <param name="GetChargeDetailRecords">An GetChargeDetailRecords response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(GetChargeDetailRecordsResponse       GetChargeDetailRecords,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(GetChargeDetailRecords?.Request,
                       GetChargeDetailRecords.HasCustomData
                           ? CustomData != null && CustomData.Any()
                                 ? GetChargeDetailRecords.CustomValues.Concat(CustomData)
                                 : GetChargeDetailRecords.CustomValues
                           : CustomData)

            {

                if (GetChargeDetailRecords != null)
                {

                    this.ChargeDetailRecords  = GetChargeDetailRecords.ChargeDetailRecords;
                    this.StatusCode           = GetChargeDetailRecords.StatusCode;

                }

            }

            #endregion

            #endregion


            #region Equals(GetChargeDetailRecordsResponse)

            /// <summary>
            /// Compares two GetChargeDetailRecords responses for equality.
            /// </summary>
            /// <param name="GetChargeDetailRecordsResponse">A GetChargeDetailRecords response to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(GetChargeDetailRecordsResponse GetChargeDetailRecordsResponse)
            {

                if ((Object) GetChargeDetailRecordsResponse == null)
                    return false;

                return (!ChargeDetailRecords.Any() && !GetChargeDetailRecordsResponse.ChargeDetailRecords.Any()) ||
                        (ChargeDetailRecords.Any() && GetChargeDetailRecordsResponse.ChargeDetailRecords.Any() && ChargeDetailRecords.Count().Equals(GetChargeDetailRecordsResponse.ChargeDetailRecords.Count())) &&

                        (StatusCode != null && GetChargeDetailRecordsResponse.StatusCode != null) ||
                        (StatusCode == null && GetChargeDetailRecordsResponse.StatusCode == null && StatusCode.Equals(GetChargeDetailRecordsResponse.StatusCode));

            }

            #endregion

            public override GetChargeDetailRecordsResponse ToImmutable

                => new GetChargeDetailRecordsResponse(Request,
                                                      ChargeDetailRecords,
                                                      StatusCode,
                                                      ImmutableCustomData);



        }

        #endregion

    }

}
