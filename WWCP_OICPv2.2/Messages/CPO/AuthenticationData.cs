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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.CPO
{

    /// <summary>
    /// A group of OICP authentication data records.
    /// </summary>
    public class AuthenticationData : AResponse<PullAuthenticationDataRequest,
                                                AuthenticationData>
    {

        #region Properties

        /// <summary>
        /// An enumeration of provider authentication data records.
        /// </summary>
        public IEnumerable<ProviderAuthenticationData>  ProviderAuthenticationDataRecords    { get; }

        /// <summary>
        /// The status code for this request.
        /// </summary>
        public StatusCode?                              StatusCode                           { get; }

        #endregion

        #region Constructor(s)

        #region AuthenticationData(Request, ProviderAuthenticationDataRecords, StatusCode = null, CustomData = null)

        /// <summary>
        /// Create a new group of OICP provider authentication data records.
        /// </summary>
        /// <param name="Request">A PullAuthenticationData request.</param>
        /// <param name="ProviderAuthenticationDataRecords">An enumeration of provider authentication data records.</param>
        /// <param name="StatusCode">An optional status code for this request.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public AuthenticationData(PullAuthenticationDataRequest            Request,
                                  IEnumerable<ProviderAuthenticationData>  ProviderAuthenticationDataRecords,
                                  StatusCode?                              StatusCode   = null,
                                  IReadOnlyDictionary<String, Object>      CustomData   = null)

            : base(Request,
                   CustomData)

        {

            #region Initial checks

            if (ProviderAuthenticationDataRecords == null)
                throw new ArgumentNullException(nameof(ProviderAuthenticationDataRecords),  "The given provider authentication data records must not be null!");

            #endregion

            this.ProviderAuthenticationDataRecords  = ProviderAuthenticationDataRecords;
            this.StatusCode                         = StatusCode;

        }

        #endregion

        #region AuthenticationData(Request, Code, Description = null, AdditionalInfo = null, CustomData = null)

        /// <summary>
        /// Create a new group of OICP provider authentication data records.
        /// </summary>
        /// <param name="Request">A PullAuthenticationData request.</param>
        /// <param name="Code">The result code of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="AdditionalInfo">An optional additional information.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public AuthenticationData(PullAuthenticationDataRequest        Request,
                                  StatusCodes                          Code,
                                  String                               Description      = null,
                                  String                               AdditionalInfo   = null,
                                  IReadOnlyDictionary<String, Object>  CustomData       = null)

            : this(Request,
                   new ProviderAuthenticationData[0],
                   new StatusCode(Code,
                                  Description,
                                  AdditionalInfo),
                   CustomData)

        { }

        #endregion

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv             = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:AuthenticationData  = "http://www.hubject.com/b2b/services/authenticationdata/v2.0"
        //                   xmlns:CommonTypes         = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <AuthenticationData:eRoamingAuthenticationData>
        //
        //          <AuthenticationData:AuthenticationData>
        //
        //             <!--Zero or more repetitions:-->
        //             <AuthenticationData:ProviderAuthenticationData>
        //
        //                <AuthenticationData:ProviderID>DE*GDF</AuthenticationData:ProviderID>
        //
        //                <!--Zero or more repetitions:-->
        //                <AuthenticationData:AuthenticationDataRecord>
        //                   <AuthenticationData:Identification>
        //
        //                      <!--You have a CHOICE of the next 4 items at this level-->
        //                      <CommonTypes:RFIDmifarefamilyIdentification>
        //                         <CommonTypes:UID>08152305</CommonTypes:UID>
        //                      </CommonTypes:RFIDmifarefamilyIdentification>
        //
        //                      <CommonTypes:AuthenticationData>
        //
        //                         <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //
        //                         <!--You have a CHOICE of the next 2 items at this level-->
        //                         <CommonTypes:PIN>?</CommonTypes:PIN>
        //
        //                         <CommonTypes:HashedPIN>
        //                            <CommonTypes:Value>f7cf02826ba923e3d31c1c3015899076</CommonTypes:Value>
        //                            <CommonTypes:Function>MD5|SHA-1</CommonTypes:Function>
        //                            <CommonTypes:Salt>22c7c09370af2a3f07fe8665b140498a</CommonTypes:Salt>
        //                         </CommonTypes:HashedPIN>
        //
        //                      </CommonTypes:AuthenticationData>
        //
        //                      <CommonTypes:PlugAndChargeIdentification>
        //                         <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //                      </CommonTypes:PlugAndChargeIdentification>
        //
        //                      <CommonTypes:RemoteIdentification>
        //                         <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //                      </CommonTypes:RemoteIdentification>
        //
        //                   </AuthenticationData:Identification>
        //                </AuthenticationData:AuthenticationDataRecord>
        //
        //             </AuthenticationData:ProviderAuthenticationData>
        //          </AuthenticationData:AuthenticationData>
        //
        //          <!--Optional:-->
        //          <AuthenticationData:StatusCode>
        //
        //             <CommonTypes:Code>?</CommonTypes:Code>
        //
        //             <!--Optional:-->
        //             <CommonTypes:Description>?</CommonTypes:Description>
        //
        //             <!--Optional:-->
        //             <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
        //
        //          </AuthenticationData:StatusCode>
        //
        //       </AuthenticationData:eRoamingAuthenticationData>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, AuthenticationDataXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP authentication data.
        /// </summary>
        /// <param name="Request">A PullAuthenticationData request.</param>
        /// <param name="AuthenticationDataXML">The XML to parse.</param>
        /// <param name="CustomAuthenticationDataParser">A delegate to parse custom AuthenticationData XML elements.</param>
        /// <param name="CustomProviderAuthenticationDataParser">A delegate to parse custom ProviderAuthenticationData XML elements.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthenticationData Parse(PullAuthenticationDataRequest                        Request,
                                               XElement                                             AuthenticationDataXML,
                                               CustomXMLParserDelegate<AuthenticationData>          CustomAuthenticationDataParser           = null,
                                               CustomXMLParserDelegate<ProviderAuthenticationData>  CustomProviderAuthenticationDataParser   = null,
                                               CustomXMLParserDelegate<Identification>              CustomIdentificationParser               = null,
                                               CustomXMLParserDelegate<RFIDIdentification>          CustomRFIDIdentificationParser           = null,
                                               CustomXMLParserDelegate<StatusCode>                  CustomStatusCodeParser                   = null,
                                               OnExceptionDelegate                                  OnException                              = null)
        {

            if (TryParse(Request,
                         AuthenticationDataXML,
                         out AuthenticationData _AuthenticationData,
                         CustomAuthenticationDataParser,
                         CustomProviderAuthenticationDataParser,
                         CustomIdentificationParser,
                         CustomRFIDIdentificationParser,
                         CustomStatusCodeParser,
                         OnException))

                return _AuthenticationData;

            return default;

        }

        #endregion

        #region (static) Parse   (Request, AuthenticationDataText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP authentication data.
        /// </summary>
        /// <param name="Request">A PullAuthenticationData request.</param>
        /// <param name="AuthenticationDataText">The text to parse.</param>
        /// <param name="CustomAuthenticationDataParser">A delegate to parse custom AuthenticationData XML elements.</param>
        /// <param name="CustomProviderAuthenticationDataParser">A delegate to parse custom ProviderAuthenticationData XML elements.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthenticationData Parse(PullAuthenticationDataRequest                        Request,
                                               String                                               AuthenticationDataText,
                                               CustomXMLParserDelegate<AuthenticationData>          CustomAuthenticationDataParser           = null,
                                               CustomXMLParserDelegate<ProviderAuthenticationData>  CustomProviderAuthenticationDataParser   = null,
                                               CustomXMLParserDelegate<Identification>              CustomIdentificationParser               = null,
                                               CustomXMLParserDelegate<RFIDIdentification>          CustomRFIDIdentificationParser           = null,
                                               CustomXMLParserDelegate<StatusCode>                  CustomStatusCodeParser                   = null,
                                               OnExceptionDelegate                                  OnException                              = null)
        {

            if (TryParse(Request,
                         AuthenticationDataText,
                         out AuthenticationData _AuthenticationData,
                         CustomAuthenticationDataParser,
                         CustomProviderAuthenticationDataParser,
                         CustomIdentificationParser,
                         CustomRFIDIdentificationParser,
                         CustomStatusCodeParser,
                         OnException))

                return _AuthenticationData;

            return default;

        }

        #endregion

        #region (static) TryParse(Request, AuthenticationDataXML,  out AuthenticationData, ..., OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP authentication data.
        /// </summary>
        /// <param name="Request">A PullAuthenticationData request.</param>
        /// <param name="AuthenticationDataXML">The XML to parse.</param>
        /// <param name="AuthenticationData">The parsed authentication data.</param>
        /// <param name="CustomAuthenticationDataParser">A delegate to parse custom AuthenticationData XML elements.</param>
        /// <param name="CustomProviderAuthenticationDataParser">A delegate to parse custom ProviderAuthenticationData XML elements.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(PullAuthenticationDataRequest                        Request,
                                       XElement                                             AuthenticationDataXML,
                                       out AuthenticationData                               AuthenticationData,
                                       CustomXMLParserDelegate<AuthenticationData>          CustomAuthenticationDataParser           = null,
                                       CustomXMLParserDelegate<ProviderAuthenticationData>  CustomProviderAuthenticationDataParser   = null,
                                       CustomXMLParserDelegate<Identification>              CustomIdentificationParser               = null,
                                       CustomXMLParserDelegate<RFIDIdentification>          CustomRFIDIdentificationParser           = null,
                                       CustomXMLParserDelegate<StatusCode>                  CustomStatusCodeParser                   = null,
                                       OnExceptionDelegate                                  OnException                              = null)
        {

            try
            {

                if (AuthenticationDataXML.Name != OICPNS.AuthenticationData + "eRoamingAuthenticationData")
                {
                    AuthenticationData = null;
                    return false;
                }

                AuthenticationData = new AuthenticationData(

                                         Request,

                                         AuthenticationDataXML.MapElements         (OICPNS.AuthenticationData + "ProviderAuthenticationData",
                                                                                    (xml, e) => ProviderAuthenticationData.Parse(xml,
                                                                                                                                 CustomProviderAuthenticationDataParser,
                                                                                                                                 CustomIdentificationParser,
                                                                                                                                 CustomRFIDIdentificationParser,
                                                                                                                                 e),
                                                                                    OnException),

                                         AuthenticationDataXML.MapElementOrNullable(OICPNS.AuthenticationData + "StatusCode",
                                                                                    (xml, e) => OICPv2_2.StatusCode.Parse(xml,
                                                                                                                          CustomStatusCodeParser,
                                                                                                                          e),
                                                                                    OnException)

                                     );


                if (CustomAuthenticationDataParser != null)
                    AuthenticationData = CustomAuthenticationDataParser(AuthenticationDataXML,
                                                                        AuthenticationData);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, AuthenticationDataXML, e);

                AuthenticationData = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, AuthenticationDataText, out AuthenticationData, ..., OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP authentication data.
        /// </summary>
        /// <param name="Request">A PullAuthenticationData request.</param>
        /// <param name="AuthenticationDataText">The text to parse.</param>
        /// <param name="AuthenticationData">The parsed authentication data.</param>
        /// <param name="CustomAuthenticationDataParser">A delegate to parse custom AuthenticationData XML elements.</param>
        /// <param name="CustomProviderAuthenticationDataParser">A delegate to parse custom ProviderAuthenticationData XML elements.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(PullAuthenticationDataRequest                        Request,
                                       String                                               AuthenticationDataText,
                                       out AuthenticationData                               AuthenticationData,
                                       CustomXMLParserDelegate<AuthenticationData>          CustomAuthenticationDataParser           = null,
                                       CustomXMLParserDelegate<ProviderAuthenticationData>  CustomProviderAuthenticationDataParser   = null,
                                       CustomXMLParserDelegate<Identification>              CustomIdentificationParser               = null,
                                       CustomXMLParserDelegate<RFIDIdentification>          CustomRFIDIdentificationParser           = null,
                                       CustomXMLParserDelegate<StatusCode>                  CustomStatusCodeParser                   = null,
                                       OnExceptionDelegate                                  OnException                              = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(AuthenticationDataText).Root,
                             out AuthenticationData,
                             CustomAuthenticationDataParser,
                             CustomProviderAuthenticationDataParser,
                             CustomIdentificationParser,
                             CustomRFIDIdentificationParser,
                             CustomStatusCodeParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, AuthenticationDataText, e);
            }

            AuthenticationData = default;
            return false;

        }

        #endregion

        #region ToXML(CustomAuthenticationDataSerializer = null, CustomProviderAuthenticationDataSerializer = null, CustomIdentificationSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizationStartByIdSerializer">A delegate to customize the serialization of AuthorizationStartById respones.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom ProviderAuthenticationData XML elements.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<AuthenticationData>         CustomAuthenticationDataSerializer           = null,
                              CustomXMLSerializerDelegate<ProviderAuthenticationData> CustomProviderAuthenticationDataSerializer   = null,
                              CustomXMLSerializerDelegate<Identification>             CustomIdentificationSerializer               = null)

        {

            var XML = new XElement(OICPNS.EVSEStatus + "eRoamingAuthenticationData",

                          ProviderAuthenticationDataRecords.Any()
                              ? ProviderAuthenticationDataRecords.Select(record => record.ToXML(CustomProviderAuthenticationDataSerializer,
                                                                                                CustomIdentificationSerializer))
                              : null,

                          StatusCode?.ToXML(OICPNS.AuthenticationData + "StatusCode")

                      );


            return CustomAuthenticationDataSerializer != null
                       ? CustomAuthenticationDataSerializer(this, XML)
                       : XML;

        }

        #endregion



        #region Operator overloading

        #region Operator == (AuthenticationData1, AuthenticationData2)

        /// <summary>
        /// Compares two authentication data for equality.
        /// </summary>
        /// <param name="AuthenticationData1">An authentication data.</param>
        /// <param name="AuthenticationData2">Another authentication data.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthenticationData AuthenticationData1, AuthenticationData AuthenticationData2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(AuthenticationData1, AuthenticationData2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AuthenticationData1 == null) || ((Object) AuthenticationData2 == null))
                return false;

            return AuthenticationData1.Equals(AuthenticationData2);

        }

        #endregion

        #region Operator != (AuthenticationData1, AuthenticationData2)

        /// <summary>
        /// Compares two authentication data for inequality.
        /// </summary>
        /// <param name="AuthenticationData1">An authentication data.</param>
        /// <param name="AuthenticationData2">Another authentication data.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthenticationData AuthenticationData1, AuthenticationData AuthenticationData2)

            => !(AuthenticationData1 == AuthenticationData2);

        #endregion

        #endregion

        #region IEquatable<AuthenticationData> Members

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

            var AuthenticationData = Object as AuthenticationData;
            if ((Object) AuthenticationData == null)
                return false;

            return Equals(AuthenticationData);

        }

        #endregion

        #region Equals(AuthenticationData)

        /// <summary>
        /// Compares two electric vehicle contract identifications with (hashed) pins for equality.
        /// </summary>
        /// <param name="AuthenticationData">An electric vehicle contract identification with (hashed) pin to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AuthenticationData AuthenticationData)
        {

            if ((Object) AuthenticationData == null)
                return false;

            return ProviderAuthenticationDataRecords.Count().Equals(AuthenticationData.ProviderAuthenticationDataRecords.Count()) &&

                   ((StatusCode == null && AuthenticationData.StatusCode == null) ||
                    (StatusCode != null &&  AuthenticationData.StatusCode != null) || StatusCode.Equals(AuthenticationData.StatusCode));

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return ProviderAuthenticationDataRecords.GetHashCode() * 3 ^

                       (StatusCode != null
                            ? StatusCode.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(ProviderAuthenticationDataRecords.Count(),
                             " Authentication data record(s)",

                             StatusCode.HasValue
                                 ? ", " + StatusCode
                                 : "");

        #endregion


    }

}
