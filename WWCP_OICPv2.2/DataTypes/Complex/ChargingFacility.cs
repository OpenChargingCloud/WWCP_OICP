/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

        /// <summary>
    /// A charging facility.
    /// </summary>
    public class ChargingFacility: IEquatable<ChargingFacility>
    {

        #region Properties

        /// <summary>
        /// The type of power of the charging facility.
        /// </summary>
        public PowerTypes?  PowerType    { get; }

        /// <summary>
        /// The voltage of the charging facility [V].
        /// </summary>
        public UInt32?      Voltage      { get; }

        /// <summary>
        /// The amperage of the charging facility [A].
        /// </summary>
        public UInt32?      Amperage     { get; }

        /// <summary>
        /// The power of the charging facility [KW].
        /// </summary>
        public Decimal?     Power        { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging facility.
        /// </summary>
        /// <param name="PowerType">The type of power of the charging facility.</param>
        /// <param name="Voltage">The voltage of the charging facility [V].</param>
        /// <param name="Amperage">The amperage of the charging facility [A].</param>
        /// <param name="Power">The power of the charging facility [KW].</param>
        public ChargingFacility(PowerTypes?  PowerType   = null,
                                UInt32?      Voltage     = null,
                                UInt32?      Amperage    = null,
                                Decimal?     Power       = null)
        {

            this.PowerType  = PowerType;
            this.Voltage    = Voltage;
            this.Amperage   = Amperage;
            this.Power      = Power;

        }

        #endregion


        #region Documentation

        // <evsedata:ChargingFacility>
        //
        //    <!--Optional:-->
        //    <evsedata:PowerType>?</evsedata:PowerType>
        //
        //    <!--Optional:-->
        //    <evsedata:Voltage>?</evsedata:Voltage>
        //
        //    <!--Optional:-->
        //    <evsedata:Amperage>?</evsedata:Amperage>
        //
        //    <!--Optional:-->
        //    <evsedata:Power>?</evsedata:Power>
        //
        // </evsedata:ChargingFacility>

        #endregion

        #region (static) Parse   (ChargingFacilityXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of a ChargingFacility object.
        /// </summary>
        /// <param name="ChargingFacilityXML">The XML to parse.</param>
        /// <param name="CustomChargingFacilityParser">An optional delegate to parse custom ChargingFacility XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ChargingFacility Parse(XElement                                   ChargingFacilityXML,
                                             CustomXMLParserDelegate<ChargingFacility>  CustomChargingFacilityParser,
                                             OnExceptionDelegate                        OnException = null)
        {

            if (TryParse(ChargingFacilityXML,
                         CustomChargingFacilityParser,
                         out ChargingFacility chargingFacility,
                         OnException))
            {
                return chargingFacility;
            }

            return null;

        }

        #endregion

        #region (static) Parse   (ChargingFacilityText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of a ChargingFacility object.
        /// </summary>
        /// <param name="ChargingFacilityText">The text to parse.</param>
        /// <param name="CustomChargingFacilityParser">An optional delegate to parse custom ChargingFacility XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ChargingFacility Parse(String                                     ChargingFacilityText,
                                             CustomXMLParserDelegate<ChargingFacility>  CustomChargingFacilityParser,
                                             OnExceptionDelegate                        OnException = null)
        {

            if (TryParse(ChargingFacilityText,
                         CustomChargingFacilityParser,
                         out ChargingFacility chargingFacility,
                         OnException))
            {
                return chargingFacility;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(ChargingFacilityXML,  ..., out ChargingFacility, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of a ChargingFacility object.
        /// </summary>
        /// <param name="ChargingFacilityXML">The XML to parse.</param>
        /// <param name="CustomChargingFacilityParser">An optional delegate to parse custom ChargingFacility XML elements.</param>
        /// <param name="ChargingFacility">The parsed ChargingFacility object.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                   ChargingFacilityXML,
                                       CustomXMLParserDelegate<ChargingFacility>  CustomChargingFacilityParser,
                                       out ChargingFacility                       ChargingFacility,
                                       OnExceptionDelegate                        OnException  = null)
        {

            try
            {

                ChargingFacility = new ChargingFacility(ChargingFacilityXML.MapValueOrNullable("PowerType",  ConversionMethods.AsPowerType),
                                                        ChargingFacilityXML.MapValueOrNullable("Voltage",    UInt32. Parse),
                                                        ChargingFacilityXML.MapValueOrNullable("Amperage",   UInt32. Parse),
                                                        ChargingFacilityXML.MapValueOrNullable("Power",      Decimal.Parse));


                if (CustomChargingFacilityParser != null)
                    ChargingFacility = CustomChargingFacilityParser(ChargingFacilityXML,
                                                                    ChargingFacility);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, ChargingFacilityXML, e);

                ChargingFacility = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(ChargingFacilityText, ..., out ChargingFacility, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of a ChargingFacility object.
        /// </summary>
        /// <param name="ChargingFacilityText">The text to parse.</param>
        /// <param name="CustomChargingFacilityParser">An optional delegate to parse custom ChargingFacility XML elements.</param>
        /// <param name="ChargingFacility">The parsed ChargingFacility object.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                     ChargingFacilityText,
                                       CustomXMLParserDelegate<ChargingFacility>  CustomChargingFacilityParser,
                                       out ChargingFacility                       ChargingFacility,
                                       OnExceptionDelegate                        OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(ChargingFacilityText).Root,
                             CustomChargingFacilityParser,
                             out ChargingFacility,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, ChargingFacilityText, e);
            }

            ChargingFacility = null;
            return false;

        }

        #endregion

        #region ToXML(CustomChargingFacilitySerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomChargingFacilitySerializer">A delegate to serialize custom set EVSE busy status request XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<ChargingFacility> CustomChargingFacilitySerializer = null)
        {

            var XML = new XElement("ChargingFacility",

                              PowerType.HasValue
                                  ? new XElement("PowerType",  PowerType.Value.AsText())
                                  : null,

                              Voltage.HasValue
                                  ? new XElement("Voltage",    Voltage.  Value.ToString())
                                  : null,

                              Amperage.HasValue
                                  ? new XElement("Amperage",   Amperage. Value.ToString())
                                  : null,

                              Power.HasValue
                                  ? new XElement("Power",      Power.    Value.ToString())
                                  : null

                      );


            return CustomChargingFacilitySerializer != null
                       ? CustomChargingFacilitySerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingFacility1, ChargingFacility2)

        /// <summary>
        /// Compares two ChargingFacility for equality.
        /// </summary>
        /// <param name="ChargingFacility1">An ChargingFacility.</param>
        /// <param name="ChargingFacility2">Another ChargingFacility.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingFacility ChargingFacility1, ChargingFacility ChargingFacility2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingFacility1, ChargingFacility2))
                return true;

            // If one is null, but not both, return false.
            if ((ChargingFacility1 is null) || (ChargingFacility2 is null))
                return false;

            return ChargingFacility1.Equals(ChargingFacility2);

        }

        #endregion

        #region Operator != (ChargingFacility1, ChargingFacility2)

        /// <summary>
        /// Compares two ChargingFacility for inequality.
        /// </summary>
        /// <param name="ChargingFacility1">An ChargingFacility.</param>
        /// <param name="ChargingFacility2">Another ChargingFacility.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingFacility ChargingFacility1, ChargingFacility ChargingFacility2)

            => !(ChargingFacility1 == ChargingFacility2);

        #endregion

        #endregion

        #region IEquatable<ChargingFacility> Members

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

            if (!(Object is ChargingFacility ChargingFacility))
                return false;

            return Equals(ChargingFacility);

        }

        #endregion

        #region Equals(ChargingFacility)

        /// <summary>
        /// Compares two ChargingFacility for equality.
        /// </summary>
        /// <param name="ChargingFacility">An ChargingFacility to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingFacility ChargingFacility)
        {

            if (ChargingFacility is null)
                return false;

            return ((!PowerType.HasValue && !ChargingFacility.PowerType.HasValue) ||
                     (PowerType.HasValue &&  ChargingFacility.PowerType.HasValue && PowerType.Value.Equals(ChargingFacility.PowerType.Value))) &&

                   ((!Voltage.  HasValue && !ChargingFacility.Voltage.  HasValue) ||
                     (Voltage.  HasValue &&  ChargingFacility.Voltage.  HasValue && Voltage.  Value.Equals(ChargingFacility.Voltage.  Value))) &&

                   ((!Amperage. HasValue && !ChargingFacility.Amperage. HasValue) ||
                     (Amperage. HasValue &&  ChargingFacility.Amperage. HasValue && Amperage. Value.Equals(ChargingFacility.Amperage. Value))) &&

                   ((!Power.    HasValue && !ChargingFacility.Power.    HasValue) ||
                     (Power.    HasValue &&  ChargingFacility.Power.    HasValue && Power.    Value.Equals(ChargingFacility.Power.    Value)));

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

                return (PowerType.HasValue
                            ? PowerType.GetHashCode() * 7
                            : 0) ^

                       (Voltage.HasValue
                            ? Voltage.  GetHashCode() * 5
                            : 0) ^

                       (Amperage.HasValue
                            ? Amperage. GetHashCode() * 3
                            : 0) ^

                       (Power.HasValue
                            ? Power.    GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => new String[] {
                   PowerType.HasValue ? PowerType.ToString()         : "",
                   Voltage.  HasValue ? Voltage.  ToString() + " V"  : "",
                   Amperage. HasValue ? Amperage. ToString() + " A"  : "",
                   Power.    HasValue ? Power.    ToString() + " KW" : ""
               }.AggregateWith(",");

        #endregion

    }


    /// <summary>
    /// OICP data conversion methods.
    /// </summary>
    public static partial class ConversionMethods
    {

        #region AsPowerType(Text)

        /// <summary>
        /// Parse the given text representation of a power type.
        /// </summary>
        /// <param name="Text">A text representation of a power type.</param>
        public static PowerTypes AsPowerType(String Text)
        {

            switch (Text)
            {

                case "AC_1_PHASE":
                    return PowerTypes.AC_1_PHASE;

                case "AC_3_PHASE":
                    return PowerTypes.AC_3_PHASE;

                case "DC":
                    return PowerTypes.DC;

                default:
                    return PowerTypes.Unspecified;

            }

        }

        #endregion

        #region AsText(this PowerType)

        /// <summary>
        /// Return a text representation of the given power type.
        /// </summary>
        /// <param name="BusyStatus">A power type.</param>
        public static String AsText(this PowerTypes PowerType)
        {

            switch (PowerType)
            {

                case PowerTypes.AC_1_PHASE:
                    return "AC_1_PHASE";

                case PowerTypes.AC_3_PHASE:
                    return "AC_3_PHASE";

                case PowerTypes.DC:
                    return "DC";

                default:
                    return "Unspecified";

            }

        }

        #endregion

    }


    /// <summary>
    /// OICP charging facilities.
    /// </summary>
    [Flags]
    public enum PowerTypes
    {

        /// <summary>
        /// Unspecified charging facilities.
        /// </summary>
        Unspecified,

        /// <summary>
        /// AC, 1 phase.
        /// </summary>
        AC_1_PHASE,

        /// <summary>
        /// AC, 3 phases.
        /// </summary>
        AC_3_PHASE,

        /// <summary>
        /// DC.
        /// </summary>
        DC

    }

}
