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
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
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
        /// A RFID identification.
        /// </summary>
        public UID?                   RFIDId                         { get; }

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
                               QRCodeIdentification?                QRCodeIdentification          = null,
                               EVCO_Id?                             PlugAndChargeIdentification   = null,
                               EVCO_Id?                             RemoteIdentification          = null,
                               IReadOnlyDictionary<String, Object>  CustomData                    = null)

            : base(CustomData)

        {

            this.RFIDId                       = RFIDId;
            this.QRCodeIdentification         = QRCodeIdentification;
            this.PlugAndChargeIdentification  = PlugAndChargeIdentification;
            this.RemoteIdentification         = RemoteIdentification;

        }

        #endregion


        //ToDo: Implement new RFIDIdentification complex type!

        #region (static) FromRFIDId                     (UID,                         CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="UID">An user identification.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public static Identification FromUID(UID                                  UID,
                                             IReadOnlyDictionary<String, Object>  CustomData  = null)

            => new Identification(RFIDId:     UID,
                                  CustomData: CustomData);

        #endregion

        #region (static) FromQRCodeIdentification       (EVCOId, PIN,                 CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="EVCOId">An electric vehicle contract identification (EVCO Id).</param>
        /// <param name="PIN">A PIN.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public static Identification FromQRCodeIdentification(EVCO_Id                              EVCOId,
                                                              String                               PIN,
                                                              IReadOnlyDictionary<String, Object>  CustomData  = null)

            => new Identification(QRCodeIdentification: new QRCodeIdentification(EVCOId, PIN),
                                  CustomData:           CustomData);

        #endregion

        #region (static) FromQRCodeIdentification       (QRCodeIdentification,        CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="QRCodeIdentification">A QR-code identification (EVCO Id).</param>
        /// <param name="CustomData">Optional custom data.</param>
        public static Identification FromQRCodeIdentification(QRCodeIdentification                 QRCodeIdentification,
                                                              IReadOnlyDictionary<String, Object>  CustomData  = null)

            => new Identification(QRCodeIdentification: QRCodeIdentification,
                                  CustomData:           CustomData);

        #endregion

        #region (static) FromPlugAndChargeIdentification(PlugAndChargeIdentification, CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="PlugAndChargeIdentification">A plug'n'charge identification (EVCO Id).</param>
        /// <param name="CustomData">Optional custom data.</param>
        public static Identification FromPlugAndChargeIdentification(EVCO_Id                              PlugAndChargeIdentification,
                                                                     IReadOnlyDictionary<String, Object>  CustomData  = null)

            => new Identification(PlugAndChargeIdentification: PlugAndChargeIdentification,
                                  CustomData:                  CustomData);

        #endregion

        #region (static) FromRemoteIdentification       (RemoteIdentification,        CustomData = null)

        /// <summary>
        /// Create a new identification.
        /// </summary>
        /// <param name="RemoteIdentification">A remote identification (EVCO Id).</param>
        /// <param name="CustomData">Optional custom data.</param>
        public static Identification FromRemoteIdentification(EVCO_Id                              RemoteIdentification,
                                                              IReadOnlyDictionary<String, Object>  CustomData  = null)

            => new Identification(RemoteIdentification: RemoteIdentification,
                                  CustomData:           CustomData);

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv            = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:AuthenticationData = "http://www.hubject.com/b2b/services/authenticationdata/v2.0"
        //                   xmlns:CommonTypes        = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        // 
        // [...]
        // 
        //    <Authorization:Identification>
        //       <!--You have a CHOICE of the next 4 items at this level-->
        //
        //       <CommonTypes:RFIDmifarefamilyIdentification>
        //          <CommonTypes:UID>08152305</CommonTypes:UID>
        //       </CommonTypes:RFIDmifarefamilyIdentification>
        //
        //       <CommonTypes:QRCodeIdentification>
        //
        //          <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //
        //          <!--You have a CHOICE of the next 2 items at this level-->
        //          <CommonTypes:PIN>1234</CommonTypes:PIN>
        //
        //          <CommonTypes:HashedPIN>
        //             <CommonTypes:Value>f7cf02826ba923e3d31c1c3015899076</CommonTypes:Value>
        //             <CommonTypes:Function>MD5|SHA-1</CommonTypes:Function>
        //             <CommonTypes:Salt>22c7c09370af2a3f07fe8665b140498a</CommonTypes:Salt>
        //          </CommonTypes:HashedPIN>
        //
        //       </CommonTypes:QRCodeIdentification>
        //
        //       <CommonTypes:PlugAndChargeIdentification>
        //          <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //       </CommonTypes:PlugAndChargeIdentification>
        //
        //       <CommonTypes:RemoteIdentification>
        //          <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //       </CommonTypes:RemoteIdentification>
        //
        //    </Authorization:Identification>
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
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Identification Parse(XElement                                 IdentificationXML,
                                           CustomXMLParserDelegate<Identification>  CustomIdentificationParser   = null,
                                           OnExceptionDelegate                      OnException                  = null)
        {

            if (TryParse(IdentificationXML,
                         out Identification _Identification,
                         CustomIdentificationParser,
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
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Identification Parse(String                                   IdentificationText,
                                           CustomXMLParserDelegate<Identification>  CustomIdentificationParser   = null,
                                           OnExceptionDelegate                      OnException                  = null)
        {

            if (TryParse(IdentificationText,
                         out Identification _Identification,
                         CustomIdentificationParser,
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
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                 IdentificationXML,
                                       out Identification                       Identification,
                                       CustomXMLParserDelegate<Identification>  CustomIdentificationParser   = null,
                                       OnExceptionDelegate                      OnException                  = null)
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

                                     IdentificationXML.MapValueOrNullable  (OICPNS.CommonTypes + "RFIDmifarefamilyIdentification",
                                                                            OICPNS.CommonTypes + "UID",
                                                                            UID.Parse),

                                     IdentificationXML.MapElement          (OICPNS.CommonTypes + "QRCodeIdentification",
                                                                            OICPv2_1.QRCodeIdentification.Parse,
                                                                            OnException),

                                     IdentificationXML.MapValueOrNullable  (OICPNS.CommonTypes + "PlugAndChargeIdentification",
                                                                            OICPNS.CommonTypes + "EVCOID",
                                                                            EVCO_Id.Parse),

                                     IdentificationXML.MapValueOrNullable  (OICPNS.CommonTypes + "RemoteIdentification",
                                                                            OICPNS.CommonTypes + "EVCOID",
                                                                            EVCO_Id.Parse)

                                 );


                if (CustomIdentificationParser != null)
                    Identification = CustomIdentificationParser(IdentificationXML,
                                                                Identification);

                // Returns 'false' when nothing was found...
                return Identification.RFIDId.                     HasValue ||
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
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                IdentificationText,
                                       out Identification                    Identification,
                                       CustomXMLParserDelegate<Identification>  CustomIdentificationParser   = null,
                                       OnExceptionDelegate                   OnException                  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(IdentificationText).Root,
                             out Identification,
                             CustomIdentificationParser,
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
        public XElement ToXML(XName                                     XName                            = null,
                              CustomXMLSerializerDelegate<Identification>  CustomIdentificationSerializer   = null)

        {

            var XML = new XElement(XName ?? OICPNS.Authorization + "Identification",

                          RFIDId.HasValue
                              ? new XElement(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification",
                                    new XElement(OICPNS.CommonTypes + "UID",     RFIDId.ToString()))
                              : null,

                          QRCodeIdentification.HasValue
                              ? QRCodeIdentification.Value.ToXML()
                              : null,

                          PlugAndChargeIdentification.HasValue
                              ? new XElement(OICPNS.CommonTypes + "PlugAndChargeIdentification",
                                    new XElement(OICPNS.CommonTypes + "EVCOID",  PlugAndChargeIdentification.ToString()))
                              : null,

                          RemoteIdentification.HasValue
                              ? new XElement(OICPNS.CommonTypes + "RemoteIdentification",
                                    new XElement(OICPNS.CommonTypes + "EVCOID",  RemoteIdentification.ToString()))
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

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var Identification = Object as Identification;
            if ((Object) Identification == null)
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

            if ((Object) Identification == null)
                throw new ArgumentNullException(nameof(Identification), "The given identification must not be null!");

            if (RFIDId.                     HasValue && Identification.RFIDId.                     HasValue)
                return RFIDId.                     Value.CompareTo(Identification.RFIDId.                     Value);

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

            if (Object == null)
                return false;

            var Identification = Object as Identification;
            if ((Object) Identification == null)
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

            if ((Object) Identification == null)
                return false;

            if (RFIDId.                     HasValue && Identification.RFIDId.                     HasValue)
                return RFIDId.                     Value.Equals(Identification.RFIDId.                     Value);

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            if (RFIDId.HasValue)
                return RFIDId.ToString();

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
