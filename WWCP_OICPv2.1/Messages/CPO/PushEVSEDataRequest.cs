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
using System.Threading;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.CPO
{

    /// <summary>
    /// An OICP push EVSE data request.
    /// </summary>
    public class PushEVSEDataRequest : ARequest<PushEVSEDataRequest>
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE data records.
        /// </summary>
        public IEnumerable<EVSEDataRecord>  EVSEDataRecords   { get; }

        /// <summary>
        /// The unqiue identification of the charging station operator maintaining the given EVSE data records.
        /// </summary>
        public Operator_Id                  OperatorId        { get; }

        /// <summary>
        /// An optional name of the charging station operator maintaining the given EVSE data records.
        /// </summary>
        public String                       OperatorName      { get; }

        /// <summary>
        /// The server-side data management operation.
        /// </summary>
        public ActionTypes                  Action            { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP PushEVSEData XML/SOAP request.
        /// </summary>
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE data records.</param>
        /// <param name="OperatorName">An optional name of the charging station operator maintaining the given EVSE data records.</param>
        /// <param name="Action">The server-side data management operation.</param>
        public PushEVSEDataRequest(IEnumerable<EVSEDataRecord>  EVSEDataRecords,
                                   Operator_Id                  OperatorId,
                                   String                       OperatorName        = null,
                                   ActionTypes                  Action              = ActionTypes.fullLoad,

                                   DateTime?                    Timestamp           = null,
                                   CancellationToken?           CancellationToken   = null,
                                   EventTracking_Id             EventTrackingId     = null,
                                   TimeSpan?                    RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            #region Initial checks

            if (EVSEDataRecords == null)
                throw new ArgumentNullException(nameof(EVSEDataRecords), "The given enumeration of EVSE data records must not be null!");

            #endregion

            this.EVSEDataRecords  = EVSEDataRecords.Where(evsedatarecord => evsedatarecord != null);
            this.OperatorId       = OperatorId;
            this.OperatorName     = OperatorName;
            this.Action           = Action;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEData     = "http://www.hubject.com/b2b/services/evsedata/EVSEData.0"
        //                   xmlns:CommonTypes  = "http://www.hubject.com/b2b/services/commontypes/EVSEData.0">
        // 
        //    <soapenv:Header/>
        // 
        //    <soapenv:Body>
        //       <EVSEData:eRoamingPushEvseData>
        // 
        //          <EVSEData:ActionType>fullLoad|update|insert|delete</EVSEStatus:ActionType>
        // 
        //          <EVSEData:OperatorEvseData>
        // 
        //             <EVSEData:OperatorID>DE*GEF</EVSEStatus:OperatorID>
        // 
        //             <!--Optional:-->
        //             <EVSEData:OperatorName>GraphDefined e-Mobility Operator</EVSEStatus:OperatorName>
        // 
        //             <!--Zero or more repetitions:-->
        //             <EVSEData:EvseDataRecord deltaType="update|insert|delete" lastUpdate="?">
        //                [...]
        //             </EVSEData:EvseDataRecord>
        // 
        //          </EVSEData:OperatorEvseData>
        //       </EVSEData:eRoamingPushEvseData>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(PushEVSEDataXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP push EVSE data request.
        /// </summary>
        /// <param name="PushEVSEDataXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static PushEVSEDataRequest Parse(XElement             PushEVSEDataXML,
                                                OnExceptionDelegate  OnException = null)
        {

            PushEVSEDataRequest _PushEVSEData;

            if (TryParse(PushEVSEDataXML, out _PushEVSEData, OnException))
                return _PushEVSEData;

            return null;

        }

        #endregion

        #region (static) Parse(PushEVSEDataText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP push EVSE data request.
        /// </summary>
        /// <param name="PushEVSEDataText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static PushEVSEDataRequest Parse(String               PushEVSEDataText,
                                                OnExceptionDelegate  OnException = null)
        {

            PushEVSEDataRequest _PushEVSEData;

            if (TryParse(PushEVSEDataText, out _PushEVSEData, OnException))
                return _PushEVSEData;

            return null;

        }

        #endregion

        #region (static) TryParse(PushEVSEDataXML,  out PushEVSEData, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP push EVSE data request.
        /// </summary>
        /// <param name="PushEVSEDataXML">The XML to parse.</param>
        /// <param name="PushEVSEData">The parsed push EVSE data request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                 PushEVSEDataXML,
                                       out PushEVSEDataRequest  PushEVSEData,
                                       OnExceptionDelegate      OnException  = null)
        {

            try
            {

                var OperatorEvseData  = OperatorEVSEData.Parse(PushEVSEDataXML.ElementOrFail(OICPNS.EVSEData + "OperatorEvseData"));

                PushEVSEData = new PushEVSEDataRequest(

                                   OperatorEvseData.EVSEDataRecords,
                                   OperatorEvseData.OperatorId,
                                   OperatorEvseData.OperatorName,

                                   PushEVSEDataXML.MapValueOrFail(OICPNS.EVSEData + "ActionType",
                                                                  XML_IO.AsActionType)

                               );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, PushEVSEDataXML, e);

                PushEVSEData = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(PushEVSEDataText, out PushEVSEData, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP push EVSE data request.
        /// </summary>
        /// <param name="PushEVSEDataText">The text to parse.</param>
        /// <param name="PushEVSEData">The parsed push EVSE data request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                   PushEVSEDataText,
                                       out PushEVSEDataRequest  PushEVSEData,
                                       OnExceptionDelegate      OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(PushEVSEDataText).Root,
                             out PushEVSEData,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, PushEVSEDataText, e);
            }

            PushEVSEData = null;
            return false;

        }

        #endregion

        #region ToXML(IncludeMetadata = false)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="IncludeMetadata">Include </param>
        public XElement ToXML(Boolean IncludeMetadata = false)

            => new XElement(OICPNS.EVSEData + "eRoamingPushEvseData",

                                new XElement(OICPNS.EVSEData + "ActionType",              XML_IO.AsText(Action)),

                                new XElement(OICPNS.EVSEData + "OperatorEvseData",

                                    new XElement(OICPNS.EVSEData + "OperatorID",          OperatorId.ToString()),

                                    OperatorName.IsNotNullOrEmpty()
                                        ? new XElement(OICPNS.EVSEData + "OperatorName",  OperatorName)
                                        : null,

                                    EVSEDataRecords.
                                        SafeSelect(evsedatarecord => evsedatarecord.ToXML(IncludeMetadata))

                                )

                           );

        #endregion


        #region Operator overloading

        #region Operator == (PushEVSEData1, PushEVSEData2)

        /// <summary>
        /// Compares two push EVSE data requests for equality.
        /// </summary>
        /// <param name="PushEVSEData1">An push EVSE data request.</param>
        /// <param name="PushEVSEData2">Another push EVSE data request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PushEVSEDataRequest PushEVSEData1, PushEVSEDataRequest PushEVSEData2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(PushEVSEData1, PushEVSEData2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PushEVSEData1 == null) || ((Object) PushEVSEData2 == null))
                return false;

            return PushEVSEData1.Equals(PushEVSEData2);

        }

        #endregion

        #region Operator != (PushEVSEData1, PushEVSEData2)

        /// <summary>
        /// Compares two push EVSE data requests for inequality.
        /// </summary>
        /// <param name="PushEVSEData1">An push EVSE data request.</param>
        /// <param name="PushEVSEData2">Another push EVSE data request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PushEVSEDataRequest PushEVSEData1, PushEVSEDataRequest PushEVSEData2)

            => !(PushEVSEData1 == PushEVSEData2);

        #endregion

        #endregion

        #region IEquatable<PushEVSEData> Members

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

            var PushEVSEData = Object as PushEVSEDataRequest;
            if ((Object) PushEVSEData == null)
                return false;

            return this.Equals(PushEVSEData);

        }

        #endregion

        #region Equals(PushEVSEData)

        /// <summary>
        /// Compares two push EVSE data requests for equality.
        /// </summary>
        /// <param name="PushEVSEData">An push EVSE data request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PushEVSEDataRequest PushEVSEData)
        {

            if ((Object) PushEVSEData == null)
                return false;

            return EVSEDataRecords.Count().Equals(PushEVSEData.EVSEDataRecords.Count()) &&
                   OperatorId.             Equals(PushEVSEData.OperatorId)              &&

                   ((OperatorName == null && PushEVSEData.OperatorName == null) ||
                    (OperatorName != null && PushEVSEData.OperatorName != null && OperatorName.Equals(PushEVSEData.OperatorName))) &&

                   Action.                 Equals(PushEVSEData.Action);

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

                return EVSEDataRecords.GetHashCode() * 17 ^
                       OperatorId.     GetHashCode() * 11 ^

                       (OperatorName != null
                            ? OperatorName.GetHashCode() * 5
                            : 0) ^

                       Action.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Action, " of ",
                             EVSEDataRecords.Count(), " EVSE data record(s)",
                             " (", OperatorName != null ? OperatorName : "", " / ", OperatorName, ")");

        #endregion


    }

}
