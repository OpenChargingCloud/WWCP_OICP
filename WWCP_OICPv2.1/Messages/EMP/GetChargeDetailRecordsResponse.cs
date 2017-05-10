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

        #region (static) Parse   (Request, GetChargeDetailRecordsResponseXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="Request">An PullGetChargeDetailRecordsResponse request.</param>
        /// <param name="GetChargeDetailRecordsResponseXML">The XML to parse.</param>
        /// <param name="CustomGetChargeDetailRecordsResponseParser">A delegate to parse custom GetChargeDetailRecords responses.</param>
        /// <param name="CustomOperatorGetChargeDetailRecordsResponseParser">A delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomGetChargeDetailRecordsResponseRecordParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetChargeDetailRecordsResponse

            Parse(GetChargeDetailRecordsRequest                            Request,
                  XElement                                                 GetChargeDetailRecordsResponseXML,
                  CustomXMLParserDelegate<GetChargeDetailRecordsResponse>  CustomGetChargeDetailRecordsResponseParser   = null,
                  CustomXMLParserDelegate<ChargeDetailRecord>              CustomChargeDetailRecordParser               = null,
                  CustomXMLParserDelegate<Identification>                  CustomIdentificationParser                   = null,
                  OnExceptionDelegate                                      OnException                                  = null)

        {

            GetChargeDetailRecordsResponse _GetChargeDetailRecordsResponse;

            if (TryParse(Request,
                         GetChargeDetailRecordsResponseXML,
                         out _GetChargeDetailRecordsResponse,
                         CustomGetChargeDetailRecordsResponseParser,
                         CustomChargeDetailRecordParser,
                         CustomIdentificationParser,
                         OnException))

                return _GetChargeDetailRecordsResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, GetChargeDetailRecordsResponseText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="Request">An PullGetChargeDetailRecordsResponse request.</param>
        /// <param name="GetChargeDetailRecordsResponseText">The text to parse.</param>
        /// <param name="CustomGetChargeDetailRecordsResponseParser">A delegate to parse custom GetChargeDetailRecords responses.</param>
        /// <param name="CustomOperatorGetChargeDetailRecordsResponseParser">A delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomGetChargeDetailRecordsResponseRecordParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetChargeDetailRecordsResponse

            Parse(GetChargeDetailRecordsRequest                            Request,
                  String                                                   GetChargeDetailRecordsResponseText,
                  CustomXMLParserDelegate<GetChargeDetailRecordsResponse>  CustomGetChargeDetailRecordsResponseParser   = null,
                  CustomXMLParserDelegate<ChargeDetailRecord>              CustomChargeDetailRecordParser               = null,
                  CustomXMLParserDelegate<Identification>                  CustomIdentificationParser                   = null,
                  OnExceptionDelegate                                      OnException                                  = null)

        {

            GetChargeDetailRecordsResponse _GetChargeDetailRecordsResponse;

            if (TryParse(Request,
                         GetChargeDetailRecordsResponseText,
                         out _GetChargeDetailRecordsResponse,
                         CustomGetChargeDetailRecordsResponseParser,
                         CustomChargeDetailRecordParser,
                         CustomIdentificationParser,
                         OnException))

                return _GetChargeDetailRecordsResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, GetChargeDetailRecordsResponseXML,  out GetChargeDetailRecordsResponse, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="Request">An PullGetChargeDetailRecordsResponse request.</param>
        /// <param name="GetChargeDetailRecordsResponseXML">The XML to parse.</param>
        /// <param name="GetChargeDetailRecordsResponse">The parsed GetChargeDetailRecordsResponse request.</param>
        /// <param name="CustomGetChargeDetailRecordsResponseParser">A delegate to parse custom GetChargeDetailRecords responses.</param>
        /// <param name="CustomOperatorGetChargeDetailRecordsResponseParser">A delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomGetChargeDetailRecordsResponseRecordParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetChargeDetailRecordsRequest                            Request,
                                       XElement                                                 GetChargeDetailRecordsResponseXML,
                                       out GetChargeDetailRecordsResponse                       GetChargeDetailRecordsResponse,
                                       CustomXMLParserDelegate<GetChargeDetailRecordsResponse>  CustomGetChargeDetailRecordsResponseParser   = null,
                                       CustomXMLParserDelegate<ChargeDetailRecord>              CustomChargeDetailRecordParser               = null,
                                       CustomXMLParserDelegate<Identification>                  CustomIdentificationParser                   = null,
                                       OnExceptionDelegate                                      OnException                                  = null)
        {

            try
            {

                if (GetChargeDetailRecordsResponseXML.Name != OICPNS.Authorization + "eRoamingChargeDetailRecords")
                {
                    GetChargeDetailRecordsResponse = null;
                    return false;
                }

                GetChargeDetailRecordsResponse =  new GetChargeDetailRecordsResponse(

                                                      Request,

                                                      GetChargeDetailRecordsResponseXML.MapElements(OICPNS.Authorization + "eRoamingChargeDetailRecord",
                                                                                                    (XML, e) => ChargeDetailRecord.Parse(XML,
                                                                                                                                         CustomChargeDetailRecordParser,
                                                                                                                                         CustomIdentificationParser,
                                                                                                                                         e),
                                                                                                    OnException)

                                                  );


                if (CustomGetChargeDetailRecordsResponseParser != null)
                    GetChargeDetailRecordsResponse = CustomGetChargeDetailRecordsResponseParser(GetChargeDetailRecordsResponseXML,
                                                                                                GetChargeDetailRecordsResponse);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, GetChargeDetailRecordsResponseXML, e);

                GetChargeDetailRecordsResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, GetChargeDetailRecordsResponseText, out GetChargeDetailRecordsResponse, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="Request">An PullGetChargeDetailRecordsResponse request.</param>
        /// <param name="GetChargeDetailRecordsResponseText">The text to parse.</param>
        /// <param name="GetChargeDetailRecordsResponse">The parsed EVSE statuses request.</param>
        /// <param name="CustomGetChargeDetailRecordsResponseParser">A delegate to parse custom GetChargeDetailRecords responses.</param>
        /// <param name="CustomOperatorGetChargeDetailRecordsResponseParser">A delegate to parse custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomGetChargeDetailRecordsResponseRecordParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetChargeDetailRecordsRequest                            Request,
                                       String                                                   GetChargeDetailRecordsResponseText,
                                       out GetChargeDetailRecordsResponse                       GetChargeDetailRecordsResponse,
                                       CustomXMLParserDelegate<GetChargeDetailRecordsResponse>  CustomGetChargeDetailRecordsResponseParser   = null,
                                       CustomXMLParserDelegate<ChargeDetailRecord>              CustomChargeDetailRecordParser               = null,
                                       CustomXMLParserDelegate<Identification>                  CustomIdentificationParser                   = null,
                                       OnExceptionDelegate                                      OnException                                  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(GetChargeDetailRecordsResponseText).Root,
                             out GetChargeDetailRecordsResponse,
                             CustomGetChargeDetailRecordsResponseParser,
                             CustomChargeDetailRecordParser,
                             CustomIdentificationParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, GetChargeDetailRecordsResponseText, e);
            }

            GetChargeDetailRecordsResponse = null;
            return false;

        }

        #endregion

        #region ToXML(CustomGetChargeDetailRecordsResponseSerializer = null, CustomChargeDetailRecordSerializer = null, CustomIdentificationSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomGetChargeDetailRecordsResponseSerializer">A delegate to customize the serialization of GetChargeDetailRecords responses.</param>
        /// <param name="CustomChargeDetailRecordSerializer">A delegate to serialize custom ChargeDetailRecord XML elements.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<GetChargeDetailRecordsResponse>  CustomGetChargeDetailRecordsResponseSerializer   = null,
                              CustomXMLSerializerDelegate<ChargeDetailRecord>              CustomChargeDetailRecordSerializer               = null,
                              CustomXMLSerializerDelegate<Identification>                  CustomIdentificationSerializer                   = null)

        {

            var XML = new XElement(OICPNS.Authorization + "eRoamingChargeDetailRecords",

                          ChargeDetailRecords.Any()
                                  ? ChargeDetailRecords.Select(cdr => cdr.ToXML(CustomChargeDetailRecordSerializer: CustomChargeDetailRecordSerializer,
                                                                                CustomIdentificationSerializer:     CustomIdentificationSerializer))
                                  : null

                      );


            return CustomGetChargeDetailRecordsResponseSerializer != null
                       ? CustomGetChargeDetailRecordsResponseSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (GetChargeDetailRecordsResponse1, GetChargeDetailRecordsResponse2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="GetChargeDetailRecordsResponse1">A GetChargeDetailRecords response.</param>
        /// <param name="GetChargeDetailRecordsResponse2">Another GetChargeDetailRecords response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetChargeDetailRecordsResponse GetChargeDetailRecordsResponse1, GetChargeDetailRecordsResponse GetChargeDetailRecordsResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GetChargeDetailRecordsResponse1, GetChargeDetailRecordsResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetChargeDetailRecordsResponse1 == null) || ((Object) GetChargeDetailRecordsResponse2 == null))
                return false;

            return GetChargeDetailRecordsResponse1.Equals(GetChargeDetailRecordsResponse2);

        }

        #endregion

        #region Operator != (GetChargeDetailRecordsResponse1, GetChargeDetailRecordsResponse2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="GetChargeDetailRecordsResponse1">A GetChargeDetailRecords response.</param>
        /// <param name="GetChargeDetailRecordsResponse2">Another GetChargeDetailRecords response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetChargeDetailRecordsResponse GetChargeDetailRecordsResponse1, GetChargeDetailRecordsResponse GetChargeDetailRecordsResponse2)

            => !(GetChargeDetailRecordsResponse1 == GetChargeDetailRecordsResponse2);

        #endregion

        #endregion

        #region IEquatable<GetChargeDetailRecordsResponse> Members

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

            var GetChargeDetailRecordsResponse = Object as GetChargeDetailRecordsResponse;
            if ((Object) GetChargeDetailRecordsResponse == null)
                return false;

            return Equals(GetChargeDetailRecordsResponse);

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
                    (ChargeDetailRecords.Any() &&  GetChargeDetailRecordsResponse.ChargeDetailRecords.Any() && ChargeDetailRecords.Count().Equals(GetChargeDetailRecordsResponse.ChargeDetailRecords.Count())) &&

                    (StatusCode != null && GetChargeDetailRecordsResponse.StatusCode != null) ||
                    (StatusCode == null && GetChargeDetailRecordsResponse.StatusCode == null && StatusCode.Equals(GetChargeDetailRecordsResponse.StatusCode));

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

                return (ChargeDetailRecords.Any()
                           ? ChargeDetailRecords.GetHashCode() * 5
                           : 0) ^

                       StatusCode.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(ChargeDetailRecords.Count() + " charge detail record(s)",
                             StatusCode.HasValue
                                 ? " -> " + StatusCode.Value.Code
                                 : "");

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
            /// <param name="GetChargeDetailRecords">A GetChargeDetailRecords response.</param>
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
