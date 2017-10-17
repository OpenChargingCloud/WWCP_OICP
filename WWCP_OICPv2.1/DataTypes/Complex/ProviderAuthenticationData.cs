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
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An OICP Provider Authentication Data Record.
    /// </summary>
    public class ProviderAuthenticationData : IEquatable<ProviderAuthenticationData>,
                                              IComparable<ProviderAuthenticationData>,
                                              IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of an Electric Vehicle Service Provider.
        /// </summary>
        public Provider_Id                               ProviderId                     { get; }

        /// <summary>
        /// An enumeration of authorization identifications records.
        /// </summary>
        public IEnumerable<Identification>  AuthorizationIdentifications   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP provider authentication data record.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an Electric Vehicle Service Provider.</param>
        /// <param name="AuthorizationIdentifications">An enumeration of authorization identifications records.</param>
        public ProviderAuthenticationData(Provider_Id                               ProviderId,
                                          IEnumerable<Identification>  AuthorizationIdentifications)
        {

            this.ProviderId                    = ProviderId;
            this.AuthorizationIdentifications  = AuthorizationIdentifications ?? new Identification[0];

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv            = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:AuthenticationData = "http://www.hubject.com/b2b/services/authenticationdata/v2.0"
        //                   xmlns:CommonTypes        = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //  [...]
        //
        //    <AuthenticationData:ProviderAuthenticationData>
        //
        //       <AuthenticationData:ProviderID>DE*GDF</AuthenticationData:ProviderID>
        //
        //       <!--Zero or more repetitions:-->
        //       <AuthenticationData:AuthenticationDataRecord>
        //          <AuthenticationData:Identification>
        //
        //             <!--You have a CHOICE of the next 4 items at this level-->
        //             <CommonTypes:RFIDmifarefamilyIdentification>
        //                <CommonTypes:UID>08152305</CommonTypes:UID>
        //             </CommonTypes:RFIDmifarefamilyIdentification>
        //
        //             <CommonTypes:QRCodeIdentification>
        //
        //                <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //
        //                <!--You have a CHOICE of the next 2 items at this level-->
        //                <CommonTypes:PIN>?</CommonTypes:PIN>
        //
        //                <CommonTypes:HashedPIN>
        //                   <CommonTypes:Value>f7cf02826ba923e3d31c1c3015899076</CommonTypes:Value>
        //                   <CommonTypes:Function>MD5|SHA-1</CommonTypes:Function>
        //                   <CommonTypes:Salt>22c7c09370af2a3f07fe8665b140498a</CommonTypes:Salt>
        //                </CommonTypes:HashedPIN>
        //
        //             </CommonTypes:QRCodeIdentification>
        //
        //             <CommonTypes:PlugAndChargeIdentification>
        //                <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //             </CommonTypes:PlugAndChargeIdentification>
        //
        //             <CommonTypes:RemoteIdentification>
        //                <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //             </CommonTypes:RemoteIdentification>
        //
        //          </AuthenticationData:Identification>
        //       </AuthenticationData:AuthenticationDataRecord>
        //
        //    </AuthenticationData:ProviderAuthenticationData>
        //
        //  [...]
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (ProviderAuthenticationDataXML,  CustomProviderAuthenticationDataParser = null, CustomAuthorizationIdentificationParser = null, OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP provider authentication data.
        /// </summary>
        /// <param name="ProviderAuthenticationDataXML">The XML to parse.</param>
        /// <param name="CustomProviderAuthenticationDataParser">A delegate to parse custom ProviderAuthenticationData XML elements.</param>
        /// <param name="CustomAuthorizationIdentificationParser">A delegate to parse custom AuthorizationIdentification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ProviderAuthenticationData Parse(XElement                                             ProviderAuthenticationDataXML,
                                                       CustomXMLParserDelegate<ProviderAuthenticationData>  CustomProviderAuthenticationDataParser    = null,
                                                       CustomXMLParserDelegate<Identification>              CustomAuthorizationIdentificationParser   = null,
                                                       OnExceptionDelegate                                  OnException                               = null)
        {

            if (TryParse(ProviderAuthenticationDataXML,
                         out ProviderAuthenticationData _ProviderAuthenticationData,
                         CustomProviderAuthenticationDataParser,
                         CustomAuthorizationIdentificationParser,
                         OnException))

                return _ProviderAuthenticationData;

            return null;

        }

        #endregion

        #region (static) Parse   (ProviderAuthenticationDataText, CustomProviderAuthenticationDataParser = null, CustomAuthorizationIdentificationParser = null, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP provider authentication data.
        /// </summary>
        /// <param name="ProviderAuthenticationDataText">The text to parse.</param>
        /// <param name="CustomProviderAuthenticationDataParser">A delegate to parse custom ProviderAuthenticationData XML elements.</param>
        /// <param name="CustomAuthorizationIdentificationParser">A delegate to parse custom AuthorizationIdentification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ProviderAuthenticationData Parse(String                                               ProviderAuthenticationDataText,
                                                       CustomXMLParserDelegate<ProviderAuthenticationData>  CustomProviderAuthenticationDataParser    = null,
                                                       CustomXMLParserDelegate<Identification>              CustomAuthorizationIdentificationParser   = null,
                                                       OnExceptionDelegate                                  OnException                               = null)
        {

            if (TryParse(ProviderAuthenticationDataText,
                         out ProviderAuthenticationData _ProviderAuthenticationData,
                         CustomProviderAuthenticationDataParser,
                         CustomAuthorizationIdentificationParser,
                         OnException))

                return _ProviderAuthenticationData;

            return null;

        }

        #endregion

        #region (static) TryParse(ProviderAuthenticationDataXML,  out ProviderAuthenticationData, CustomProviderAuthenticationDataParser = null, CustomAuthorizationIdentificationParser = null, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP provider authentication data.
        /// </summary>
        /// <param name="ProviderAuthenticationDataXML">The XML to parse.</param>
        /// <param name="ProviderAuthenticationData">The parsed provider authentication data.</param>
        /// <param name="CustomProviderAuthenticationDataParser">A delegate to parse custom ProviderAuthenticationData XML elements.</param>
        /// <param name="CustomAuthorizationIdentificationParser">A delegate to parse custom AuthorizationIdentification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                             ProviderAuthenticationDataXML,
                                       out ProviderAuthenticationData                       ProviderAuthenticationData,
                                       CustomXMLParserDelegate<ProviderAuthenticationData>  CustomProviderAuthenticationDataParser    = null,
                                       CustomXMLParserDelegate<Identification>              CustomAuthorizationIdentificationParser   = null,
                                       OnExceptionDelegate                                  OnException                               = null)
        {

            try
            {

                if (ProviderAuthenticationDataXML.Name != OICPNS.AuthenticationData + "ProviderAuthenticationData")
                {
                    ProviderAuthenticationData = null;
                    return false;
                }

                ProviderAuthenticationData = new ProviderAuthenticationData(

                                                 ProviderAuthenticationDataXML.MapValueOrFail   (OICPNS.AuthenticationData + "ProviderID",
                                                                                                 Provider_Id.Parse),

                                                 ProviderAuthenticationDataXML.MapElementsOrFail(OICPNS.AuthenticationData + "AuthenticationDataRecord",
                                                                                                 OICPNS.AuthenticationData + "Identification",
                                                                                                 (XML, e) => Identification.Parse(XML,
                                                                                                                                  CustomAuthorizationIdentificationParser,
                                                                                                                                  e),
                                                                                                 OnException)

                                            );


                if (CustomProviderAuthenticationDataParser != null)
                    ProviderAuthenticationData = CustomProviderAuthenticationDataParser(ProviderAuthenticationDataXML,
                                                                                        ProviderAuthenticationData);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, ProviderAuthenticationDataXML, e);

                ProviderAuthenticationData = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(ProviderAuthenticationDataText, out ProviderAuthenticationData, CustomProviderAuthenticationDataParser = null, CustomAuthorizationIdentificationParser = null, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP provider authentication data.
        /// </summary>
        /// <param name="ProviderAuthenticationDataText">The text to parse.</param>
        /// <param name="ProviderAuthenticationData">The parsed provider authentication data.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                               ProviderAuthenticationDataText,
                                       out ProviderAuthenticationData                       ProviderAuthenticationData,
                                       CustomXMLParserDelegate<ProviderAuthenticationData>  CustomProviderAuthenticationDataParser    = null,
                                       CustomXMLParserDelegate<Identification>              CustomAuthorizationIdentificationParser   = null,
                                       OnExceptionDelegate                                  OnException                               = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(ProviderAuthenticationDataText).Root,
                             out ProviderAuthenticationData,
                             CustomProviderAuthenticationDataParser,
                             CustomAuthorizationIdentificationParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, ProviderAuthenticationDataText, e);
            }

            ProviderAuthenticationData = null;
            return false;

        }

        #endregion

        #region ToXML(CustomProviderAuthenticationDataSerializer = null, CustomIdentificationSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom ProviderAuthenticationData XML elements.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<ProviderAuthenticationData> CustomProviderAuthenticationDataSerializer   = null,
                              CustomXMLSerializerDelegate<Identification>             CustomIdentificationSerializer               = null)
        {

            var XML = new XElement(OICPNS.AuthenticationData + "ProviderAuthenticationData",

                          new XElement(OICPNS.AuthenticationData + "ProviderID", ProviderId.ToString()),

                          AuthorizationIdentifications.
                              SafeSelect(AuthorizationIdentification => new XElement(
                                                                            OICPNS.AuthenticationData + "AuthenticationDataRecord",
                                                                            AuthorizationIdentification.ToXML(OICPNS.AuthenticationData + "Identification",
                                                                                                              CustomIdentificationSerializer))
                                                                        )

                      );

            return CustomProviderAuthenticationDataSerializer != null
                       ? CustomProviderAuthenticationDataSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (ProviderAuthenticationData1, ProviderAuthenticationData2)

        /// <summary>
        /// Compares two provider authentication data for equality.
        /// </summary>
        /// <param name="ProviderAuthenticationData1">A push authentication data request.</param>
        /// <param name="ProviderAuthenticationData2">Another push authentication data request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ProviderAuthenticationData ProviderAuthenticationData1, ProviderAuthenticationData ProviderAuthenticationData2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ProviderAuthenticationData1, ProviderAuthenticationData2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ProviderAuthenticationData1 == null) || ((Object) ProviderAuthenticationData2 == null))
                return false;

            return ProviderAuthenticationData1.Equals(ProviderAuthenticationData2);

        }

        #endregion

        #region Operator != (ProviderAuthenticationData1, ProviderAuthenticationData2)

        /// <summary>
        /// Compares two provider authentication data for inequality.
        /// </summary>
        /// <param name="ProviderAuthenticationData1">A push authentication data request.</param>
        /// <param name="ProviderAuthenticationData2">Another push authentication data request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ProviderAuthenticationData ProviderAuthenticationData1, ProviderAuthenticationData ProviderAuthenticationData2)

            => !(ProviderAuthenticationData1 == ProviderAuthenticationData2);

        #endregion

        #region Operator <  (ProviderAuthenticationData1, ProviderAuthenticationData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderAuthenticationData1">A provider authentication data.</param>
        /// <param name="ProviderAuthenticationData2">Another provider authentication data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ProviderAuthenticationData ProviderAuthenticationData1, ProviderAuthenticationData ProviderAuthenticationData2)
        {

            if ((Object) ProviderAuthenticationData1 == null)
                throw new ArgumentNullException(nameof(ProviderAuthenticationData1), "The given ProviderAuthenticationData1 must not be null!");

            return ProviderAuthenticationData1.CompareTo(ProviderAuthenticationData2) < 0;

        }

        #endregion

        #region Operator <= (ProviderAuthenticationData1, ProviderAuthenticationData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderAuthenticationData1">A provider authentication data.</param>
        /// <param name="ProviderAuthenticationData2">Another provider authentication data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ProviderAuthenticationData ProviderAuthenticationData1, ProviderAuthenticationData ProviderAuthenticationData2)
            => !(ProviderAuthenticationData1 > ProviderAuthenticationData2);

        #endregion

        #region Operator >  (ProviderAuthenticationData1, ProviderAuthenticationData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderAuthenticationData1">A provider authentication data.</param>
        /// <param name="ProviderAuthenticationData2">Another provider authentication data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ProviderAuthenticationData ProviderAuthenticationData1, ProviderAuthenticationData ProviderAuthenticationData2)
        {

            if ((Object) ProviderAuthenticationData1 == null)
                throw new ArgumentNullException(nameof(ProviderAuthenticationData1), "The given ProviderAuthenticationData1 must not be null!");

            return ProviderAuthenticationData1.CompareTo(ProviderAuthenticationData2) > 0;

        }

        #endregion

        #region Operator >= (ProviderAuthenticationData1, ProviderAuthenticationData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderAuthenticationData1">A provider authentication data.</param>
        /// <param name="ProviderAuthenticationData2">Another provider authentication data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ProviderAuthenticationData ProviderAuthenticationData1, ProviderAuthenticationData ProviderAuthenticationData2)
            => !(ProviderAuthenticationData1 < ProviderAuthenticationData2);

        #endregion

        #endregion

        #region IComparable<ProviderAuthenticationData> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var ProviderAuthenticationData = Object as ProviderAuthenticationData;
            if ((Object) ProviderAuthenticationData == null)
                throw new ArgumentException("The given object is not a provider authentication data identification!", nameof(Object));

            return CompareTo(ProviderAuthenticationData);

        }

        #endregion

        #region CompareTo(ProviderAuthenticationData)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderAuthenticationData">An object to compare with.</param>
        public Int32 CompareTo(ProviderAuthenticationData ProviderAuthenticationData)
        {

            if ((Object) ProviderAuthenticationData == null)
                throw new ArgumentNullException(nameof(ProviderAuthenticationData), "The given provider authentication data must not be null!");

            var result = ProviderId.CompareTo(ProviderAuthenticationData.ProviderId);
            if (result != 0)
                return result;

            return AuthorizationIdentifications.Count().CompareTo(ProviderAuthenticationData.AuthorizationIdentifications.Count());

        }

        #endregion

        #endregion

        #region IEquatable<ProviderAuthenticationData> Members

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

            var ProviderAuthenticationData = Object as ProviderAuthenticationData;
            if ((Object) ProviderAuthenticationData == null)
                return false;

            return Equals(ProviderAuthenticationData);

        }

        #endregion

        #region Equals(ProviderAuthenticationData)

        /// <summary>
        /// Compares two provider authentication data for equality.
        /// </summary>
        /// <param name="ProviderAuthenticationData">A push authentication data request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ProviderAuthenticationData ProviderAuthenticationData)
        {

            if ((Object) ProviderAuthenticationData == null)
                return false;

            return ProviderId.                          Equals(ProviderAuthenticationData.ProviderId) &&
                   AuthorizationIdentifications.Count().Equals(ProviderAuthenticationData.AuthorizationIdentifications.Count());

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

                return ProviderId.                  GetHashCode() * 3 ^
                       AuthorizationIdentifications.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("'", ProviderId, "' ",
                             " having ",
                             AuthorizationIdentifications.Count(), " authorization identification(s)");

        #endregion


    }

}
