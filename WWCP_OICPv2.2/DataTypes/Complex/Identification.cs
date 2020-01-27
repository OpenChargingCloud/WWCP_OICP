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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// An OICP identification (RFID or EVCO identification).
    /// </summary>
    public class Identification : ACustomData,
                                  IEquatable <Identification>,
                                  IComparable<Identification>,
                                  IComparable
    {

        #region Properties

        /// <summary>
        /// A RFID Mifare identification.
        /// </summary>
        public UID?                   RFIDId                         { get; }

        /// <summary>
        /// A RFID identification.
        /// </summary>
        public RFIDIdentification?    RFIDIdentification             { get; }

        /// <summary>
        /// An e-mobility account identification (EVCO Id) and a PIN.
        /// </summary>
        public QRCodeIdentification?  QRCodeIdentification           { get; }

        /// <summary>
        /// A plug'n'charge identification (EVCO Id).
        /// </summary>
        public EVCO_Id?               PlugAndChargeIdentification    { get; }

        /// <summary>
        /// A remote identification (EVCO Id).
        /// </summary>
        public EVCO_Id?               RemoteIdentification           { get; }

        #endregion

        #region Constructor(s)

        private Identification(UID?                                 RFIDId                        = null,
                               RFIDIdentification?                  RFIDIdentification            = null,
                               QRCodeIdentification?                QRCodeIdentification          = null,
                               EVCO_Id?                             PlugAndChargeIdentification   = null,
                               EVCO_Id?                             RemoteIdentification          = null,
                               IReadOnlyDictionary<String, Object>  CustomData                    = null)

            : base(CustomData)

        {

            this.RFIDId                       = RFIDId;
            this.RFIDIdentification           = RFIDIdentification;
            this.QRCodeIdentification         = QRCodeIdentification;
            this.PlugAndChargeIdentification  = PlugAndChargeIdentification;
            this.RemoteIdentification         = RemoteIdentification;

        }

        #endregion


        #region (static) FromUID                        (MifareUID,                   CustomData = null)

        /// <summary>
        /// Create a new Mifare identification.
        /// </summary>
        /// <param name="MifareUID">A Mifare user identification.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public static Identification FromUID(UID?                                 MifareUID,
                                             IReadOnlyDictionary<String, Object>  CustomData  = null)

            => MifareUID.HasValue
                   ? new Identification(RFIDId:  MifareUID.Value,
                                        CustomData:    CustomData)
                   : null;

        #endregion

        #region (static) FromRFIDId                     (UID,                         CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="UID">An user identification.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public static Identification FromRFID(UID?                                 UID,
                                              IReadOnlyDictionary<String, Object>  CustomData  = null)

            => UID.HasValue
                   ? new Identification(RFIDId:      UID.Value,
                                        CustomData:  CustomData)
                   : null;

        #endregion

        #region (static) FromQRCodeIdentification       (EVCOId, PIN,                 CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="EVCOId">An electric vehicle contract identification (EVCO Id).</param>
        /// <param name="PIN">A PIN.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public static Identification FromQRCodeIdentification(EVCO_Id?                             EVCOId,
                                                              String                               PIN,
                                                              IReadOnlyDictionary<String, Object>  CustomData  = null)

            => EVCOId.HasValue
                   ? new Identification(QRCodeIdentification:  new QRCodeIdentification(EVCOId.Value,
                                                                                        PIN),
                                        CustomData:            CustomData)
                   : null;

        #endregion

        #region (static) FromQRCodeIdentification       (QRCodeIdentification,        CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="QRCodeIdentification">A QR-code identification (EVCO Id).</param>
        /// <param name="CustomData">Optional custom data.</param>
        public static Identification FromQRCodeIdentification(QRCodeIdentification?                QRCodeIdentification,
                                                              IReadOnlyDictionary<String, Object>  CustomData  = null)

            => QRCodeIdentification.HasValue
                   ? new Identification(QRCodeIdentification:  QRCodeIdentification.Value,
                                        CustomData:            CustomData)
                   : null;

        #endregion

        #region (static) FromPlugAndChargeIdentification(PlugAndChargeIdentification, CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="PlugAndChargeIdentification">A plug'n'charge identification (EVCO Id).</param>
        /// <param name="CustomData">Optional custom data.</param>
        public static Identification FromPlugAndChargeIdentification(EVCO_Id?                             PlugAndChargeIdentification,
                                                                     IReadOnlyDictionary<String, Object>  CustomData  = null)

            => PlugAndChargeIdentification.HasValue
                   ? new Identification(PlugAndChargeIdentification:  PlugAndChargeIdentification.Value,
                                        CustomData:                   CustomData)
                   : null;

        #endregion

        #region (static) FromRemoteIdentification       (RemoteIdentification,        CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="RemoteIdentification">A remote identification (EVCO Id).</param>
        /// <param name="CustomData">Optional custom data.</param>
        public static Identification FromRemoteIdentification(EVCO_Id?                             RemoteIdentification,
                                                              IReadOnlyDictionary<String, Object>  CustomData  = null)

            => RemoteIdentification.HasValue
                   ? new Identification(RemoteIdentification: RemoteIdentification.Value,
                                        CustomData:           CustomData)
                   : null;

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv            = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:AuthenticationData = "http://www.hubject.com/b2b/services/authenticationdata/v2.1"
        //                   xmlns:CommonTypes        = "http://www.hubject.com/b2b/services/commontypes/v2.1">
        // 
        // [...]
        // 
        // <Authorization:Identification>
        //    <!--You have a CHOICE of the next 5 items at this level-->
        //
        //    <CommonTypes:RFIDMifareFamilyIdentification>
        //       <CommonTypes:UID>?</CommonTypes:UID>
        //    </CommonTypes:RFIDMifareFamilyIdentification>
        //
        //
        //    <CommonTypes:RFIDIdentification>
        //
        //       <CommonTypes:UID>?</CommonTypes:UID>
        //
        //       <!--Optional:-->
        //       <CommonTypes:EvcoID>?</CommonTypes:EvcoID>
        //
        //       <CommonTypes:RFIDType>?</CommonTypes:RFIDType>
        //
        //       <!--Optional:-->
        //       <CommonTypes:PrintedNumber>?</CommonTypes:PrintedNumber>
        //
        //       <!--Optional:-->
        //       <CommonTypes:ExpiryDate>?</CommonTypes:ExpiryDate>
        //
        //    </CommonTypes:RFIDIdentification>
        //
        //
        //    <CommonTypes:QRCodeIdentification>
        //
        //       <CommonTypes:EvcoID>?</CommonTypes:EvcoID>
        //
        //       <!--You have a CHOICE of the next 2 items at this level-->
        //       <CommonTypes:PIN>?</CommonTypes:PIN>
        //
        //       <CommonTypes:HashedPIN>
        //          <CommonTypes:Value>?</CommonTypes:Value>
        //          <CommonTypes:Function>?</CommonTypes:Function>
        //       </CommonTypes:HashedPIN>
        //
        //    </CommonTypes:QRCodeIdentification>
        //
        //
        //    <CommonTypes:PlugAndChargeIdentification>
        //       <CommonTypes:EvcoID>DE-MEG-C10145984-1</CommonTypes:EvcoID>
        //    </CommonTypes:PlugAndChargeIdentification>
        //
        //
        //    <CommonTypes:RemoteIdentification>
        //      <CommonTypes:EvcoID>DE-MEG-C10145984-1</CommonTypes:EvcoID>
        //    </CommonTypes:RemoteIdentification>
        //
        // </Authorization:Identification>
        // 
        // [...]
        // 
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (IdentificationXML,  CustomIdentificationParser = null, OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP identification.
        /// </summary>
        /// <param name="IdentificationXML">The XML to parse.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Identification Parse(XElement                                     IdentificationXML,
                                           CustomXMLParserDelegate<Identification>      CustomIdentificationParser       = null,
                                           CustomXMLParserDelegate<RFIDIdentification>  CustomRFIDIdentificationParser   = null,
                                           OnExceptionDelegate                          OnException                      = null)
        {

            if (TryParse(IdentificationXML,
                         out Identification _Identification,
                         CustomIdentificationParser,
                         CustomRFIDIdentificationParser,
                         OnException))

                return _Identification;

            return null;

        }

        #endregion

        #region (static) Parse   (IdentificationText, CustomIdentificationParser = null, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP identification.
        /// </summary>
        /// <param name="IdentificationText">The text to parse.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Identification Parse(String                                       IdentificationText,
                                           CustomXMLParserDelegate<Identification>      CustomIdentificationParser       = null,
                                           CustomXMLParserDelegate<RFIDIdentification>  CustomRFIDIdentificationParser   = null,
                                           OnExceptionDelegate                          OnException                      = null)
        {

            if (TryParse(IdentificationText,
                         out Identification _Identification,
                         CustomIdentificationParser,
                         CustomRFIDIdentificationParser,
                         OnException))

                return _Identification;

            return null;

        }

        #endregion

        #region (static) TryParse(IdentificationXML,  out Identification, CustomIdentificationParser = null, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP identification.
        /// </summary>
        /// <param name="IdentificationXML">The XML to parse.</param>
        /// <param name="Identification">The parsed identification.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                     IdentificationXML,
                                       out Identification                           Identification,
                                       CustomXMLParserDelegate<Identification>      CustomIdentificationParser       = null,
                                       CustomXMLParserDelegate<RFIDIdentification>  CustomRFIDIdentificationParser   = null,
                                       OnExceptionDelegate                          OnException                      = null)
        {

            try
            {

                if (!(IdentificationXML.Name == OICPNS.Authorization      + "Identification" ||
                      IdentificationXML.Name == OICPNS.Reservation        + "Identification" ||
                      IdentificationXML.Name == OICPNS.AuthenticationData + "Identification"))
                {
                    Identification = null;
                    return false;
                }

                Identification = new Identification(

                                     IdentificationXML.MapValueOrNullable  (OICPNS.CommonTypes + "RFIDMifareFamilyIdentification",
                                                                            OICPNS.CommonTypes + "UID",
                                                                            UID.Parse),

                                     IdentificationXML.MapElement          (OICPNS.CommonTypes + "RFIDIdentification",
                                                                            (xml, e) => OICPv2_2.RFIDIdentification.Parse(xml,
                                                                                                                          CustomRFIDIdentificationParser,
                                                                                                                          e),
                                                                            OnException),

                                     IdentificationXML.MapElement          (OICPNS.CommonTypes + "QRCodeIdentification",
                                                                            OICPv2_2.QRCodeIdentification.Parse,
                                                                            OnException),

                                     IdentificationXML.MapValueOrNullable  (OICPNS.CommonTypes + "PlugAndChargeIdentification",
                                                                            OICPNS.CommonTypes + "EvcoID",
                                                                            EVCO_Id.Parse),

                                     IdentificationXML.MapValueOrNullable  (OICPNS.CommonTypes + "RemoteIdentification",
                                                                            OICPNS.CommonTypes + "EvcoID",
                                                                            EVCO_Id.Parse)

                                 );


                if (CustomIdentificationParser != null)
                    Identification = CustomIdentificationParser(IdentificationXML,
                                                                Identification);

                // Returns 'false' when nothing was found...
                return Identification.RFIDId.                     HasValue ||
                       Identification.RFIDIdentification.         HasValue ||
                       Identification.QRCodeIdentification.       HasValue ||
                       Identification.PlugAndChargeIdentification.HasValue ||
                       Identification.RemoteIdentification.       HasValue;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, IdentificationXML, e);

                Identification = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(IdentificationText, out Identification, CustomIdentificationParser = null, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP identification.
        /// </summary>
        /// <param name="IdentificationText">The text to parse.</param>
        /// <param name="Identification">The parsed identification.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                       IdentificationText,
                                       out Identification                           Identification,
                                       CustomXMLParserDelegate<Identification>      CustomIdentificationParser       = null,
                                       CustomXMLParserDelegate<RFIDIdentification>  CustomRFIDIdentificationParser   = null,
                                       OnExceptionDelegate                          OnException                      = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(IdentificationText).Root,
                             out Identification,
                             CustomIdentificationParser,
                             CustomRFIDIdentificationParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, IdentificationText, e);
            }

            Identification = null;
            return false;

        }

        #endregion

        #region ToXML(XName = null, CustomIdentificationSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">The XML name to use.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification XML elements.</param>
        public XElement ToXML(XName                                        XName                            = null,
                              CustomXMLSerializerDelegate<Identification>  CustomIdentificationSerializer   = null)

        {

            var XML = new XElement(XName ?? OICPNS.Authorization + "Identification",

                          RFIDId.HasValue
                              ? new XElement(OICPNS.CommonTypes + "RFIDMifareFamilyIdentification",
                                    new XElement(OICPNS.CommonTypes + "UID",     RFIDId.ToString()))
                              : null,

                          RFIDIdentification.HasValue
                              ? RFIDIdentification.Value.ToXML()
                              : null,

                          QRCodeIdentification.HasValue
                              ? QRCodeIdentification.Value.ToXML()
                              : null,

                          PlugAndChargeIdentification.HasValue
                              ? new XElement(OICPNS.CommonTypes + "PlugAndChargeIdentification",
                                    new XElement(OICPNS.CommonTypes + "EvcoID",  PlugAndChargeIdentification.ToString()))
                              : null,

                          RemoteIdentification.HasValue
                              ? new XElement(OICPNS.CommonTypes + "RemoteIdentification",
                                    new XElement(OICPNS.CommonTypes + "EvcoID",  RemoteIdentification.ToString()))
                              : null);

            return CustomIdentificationSerializer != null
                       ? CustomIdentificationSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (Identification1, Identification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Identification1">An identification.</param>
        /// <param name="Identification2">Another identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Identification Identification1, Identification Identification2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(Identification1, Identification2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) Identification1 == null) || ((Object) Identification2 == null))
                return false;

            return Identification1.Equals(Identification2);

        }

        #endregion

        #region Operator != (Identification1, Identification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Identification1">An identification.</param>
        /// <param name="Identification2">Another identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Identification Identification1, Identification Identification2)
            => !(Identification1 == Identification2);

        #endregion

        #region Operator <  (Identification1, Identification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Identification1">An identification.</param>
        /// <param name="Identification2">Another identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Identification Identification1, Identification Identification2)
        {

            if ((Object) Identification1 == null)
                throw new ArgumentNullException(nameof(Identification1), "The given identification must not be null!");

            return Identification1.CompareTo(Identification2) < 0;

        }

        #endregion

        #region Operator <= (Identification1, Identification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Identification1">An identification.</param>
        /// <param name="Identification2">Another identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Identification Identification1, Identification Identification2)
            => !(Identification1 > Identification2);

        #endregion

        #region Operator >  (Identification1, Identification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Identification1">An identification.</param>
        /// <param name="Identification2">Another identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Identification Identification1, Identification Identification2)
        {

            if ((Object) Identification1 == null)
                throw new ArgumentNullException(nameof(Identification1), "The given identification must not be null!");

            return Identification1.CompareTo(Identification2) > 0;

        }

        #endregion

        #region Operator >= (Identification1, Identification2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Identification1">An identification.</param>
        /// <param name="Identification2">Another identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Identification Identification1, Identification Identification2)
            => !(Identification1 < Identification2);

        #endregion

        #endregion

        #region IComparable<Identification> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Identification Identification))
                throw new ArgumentException("The given object is not an identification identification!", nameof(Object));

            return CompareTo(Identification);

        }

        #endregion

        #region CompareTo(Identification)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Identification">An object to compare with.</param>
        public Int32 CompareTo(Identification Identification)
        {

            if (Identification is null)
                throw new ArgumentNullException(nameof(Identification), "The given identification must not be null!");

            if (RFIDId.                     HasValue && Identification.RFIDId.                     HasValue)
                return RFIDId.                     Value.CompareTo(Identification.RFIDId.                     Value);

            if (RFIDIdentification.         HasValue && Identification.RFIDIdentification.         HasValue)
                return RFIDIdentification.         Value.CompareTo(Identification.RFIDIdentification.         Value);

            if (QRCodeIdentification.       HasValue && Identification.QRCodeIdentification.       HasValue)
                return QRCodeIdentification.       Value.CompareTo(Identification.QRCodeIdentification.       Value);

            if (PlugAndChargeIdentification.HasValue && Identification.PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.Value.CompareTo(Identification.PlugAndChargeIdentification.Value);

            if (RemoteIdentification.       HasValue && Identification.RemoteIdentification.       HasValue)
                return RemoteIdentification.       Value.CompareTo(Identification.RemoteIdentification.       Value);


            return ToString().CompareTo(Identification.ToString());

        }

        #endregion

        #endregion

        #region IEquatable<Identification> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object is null)
                return false;

            if (!(Object is Identification Identification))
                return false;

            return Equals(Identification);

        }

        #endregion

        #region Equals(Identification)

        /// <summary>
        /// Compares two identifications for equality.
        /// </summary>
        /// <param name="Identification">An identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Identification Identification)
        {

            if (Identification is null)
                return false;

            if (RFIDId.                     HasValue && Identification.RFIDId.                     HasValue)
                return RFIDId.                     Value.Equals(Identification.RFIDId.                     Value);

            if (RFIDIdentification.         HasValue && Identification.RFIDIdentification.         HasValue)
                return RFIDIdentification.         Value.Equals(Identification.RFIDIdentification.         Value);

            if (QRCodeIdentification.       HasValue && Identification.QRCodeIdentification.       HasValue)
                return QRCodeIdentification.       Value.Equals(Identification.QRCodeIdentification.       Value);

            if (PlugAndChargeIdentification.HasValue && Identification.PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.Value.Equals(Identification.PlugAndChargeIdentification.Value);

            if (RemoteIdentification.       HasValue && Identification.RemoteIdentification.       HasValue)
                return RemoteIdentification.       Value.Equals(Identification.RemoteIdentification.       Value);

            return ToString().Equals(Identification.ToString());

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

            if (RFIDId.HasValue)
                return RFIDId.GetHashCode();

            if (RFIDIdentification.HasValue)
                return RFIDIdentification.GetHashCode();

            if (QRCodeIdentification.HasValue)
                return QRCodeIdentification.GetHashCode();

            if (PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.GetHashCode();

            if (RemoteIdentification.HasValue)
                return RemoteIdentification.GetHashCode();

            return 0;

        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {

            if (RFIDId.HasValue)
                return RFIDId.ToString();

            if (RFIDIdentification.HasValue)
                return RFIDIdentification.ToString();

            if (QRCodeIdentification.HasValue)
                return QRCodeIdentification.ToString();

            if (PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.ToString();

            if (RemoteIdentification.HasValue)
                return RemoteIdentification.ToString();

            return String.Empty;

        }

        #endregion


    }

}
