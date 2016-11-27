/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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
    /// An OICP push EVSE status request.
    /// </summary>
    public class PushEVSEStatusRequest : ARequest<PushEVSEStatusRequest>
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE status records.
        /// </summary>
        public IEnumerable<EVSEStatusRecord>  EVSEStatusRecords   { get; }

        /// <summary>
        /// The unqiue identification of the charging station operator maintaining the given EVSE status records.
        /// </summary>
        public Operator_Id     OperatorId          { get; }

        /// <summary>
        /// An optional name of the charging station operator maintaining the given EVSE status records.
        /// </summary>
        public String                         OperatorName        { get; }

        /// <summary>
        /// The server-side data management operation.
        /// </summary>
        public ActionTypes                    Action              { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP PushEVSEStatus XML/SOAP request.
        /// </summary>
        /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE status records.</param>
        /// <param name="OperatorName">An optional name of the charging station operator maintaining the given EVSE status records.</param>
        /// <param name="Action">The server-side data management operation.</param>
        public PushEVSEStatusRequest(IEnumerable<EVSEStatusRecord>  EVSEStatusRecords,
                                     Operator_Id                    OperatorId,
                                     String                         OperatorName        = null,
                                     ActionTypes                    Action              = ActionTypes.update,

                                     DateTime?                      Timestamp           = null,
                                     CancellationToken?             CancellationToken   = null,
                                     EventTracking_Id               EventTrackingId     = null,
                                     TimeSpan?                      RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            #region Initial checks

            if (EVSEStatusRecords == null)
                throw new ArgumentNullException(nameof(EVSEStatusRecords), "The given enumeration of EVSE status records must not be null!");

            #endregion

            this.EVSEStatusRecords  = EVSEStatusRecords;
            this.OperatorId         = OperatorId;
            this.OperatorName       = OperatorName;
            this.Action             = Action;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv    = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEStatus = "http://www.hubject.com/b2b/services/evsestatus/v2.0">
        // 
        //    <soapenv:Header/>
        // 
        //    <soapenv:Body>
        //       <EVSEStatus:eRoamingPushEvseStatus>
        // 
        //          <EVSEStatus:ActionType>fullLoad|update|insert|delete</EVSEStatus:ActionType>
        // 
        //          <EVSEStatus:OperatorEvseStatus>
        // 
        //             <EVSEStatus:OperatorID>DE*GEF</EVSEStatus:OperatorID>
        // 
        //             <!--Optional:-->
        //             <EVSEStatus:OperatorName>GraphDefined e-Mobility Operator</EVSEStatus:OperatorName>
        // 
        //             <!--One or more repetitions:-->
        //             <EVSEStatus:EvseStatusRecord>
        //                <EVSEStatus:EvseId>DE*GEF*E1234*1</EVSEStatus:EvseId>
        //                <EVSEStatus:EvseStatus>Occupied</EVSEStatus:EvseStatus>
        //             </EVSEStatus:EvseStatusRecord>
        // 
        //          </EVSEStatus:OperatorEvseStatus>
        // 
        //       </EVSEStatus>
        //    </soapenv:Body>
        // 
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(PushEVSEStatusXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP push EVSE status request.
        /// </summary>
        /// <param name="PushEVSEStatusXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static PushEVSEStatusRequest Parse(XElement             PushEVSEStatusXML,
                                                  OnExceptionDelegate  OnException = null)
        {

            PushEVSEStatusRequest _PushEVSEStatus;

            if (TryParse(PushEVSEStatusXML, out _PushEVSEStatus, OnException))
                return _PushEVSEStatus;

            return null;

        }

        #endregion

        #region (static) Parse(PushEVSEStatusText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP push EVSE status request.
        /// </summary>
        /// <param name="PushEVSEStatusText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static PushEVSEStatusRequest Parse(String               PushEVSEStatusText,
                                                  OnExceptionDelegate  OnException = null)
        {

            PushEVSEStatusRequest _PushEVSEStatus;

            if (TryParse(PushEVSEStatusText, out _PushEVSEStatus, OnException))
                return _PushEVSEStatus;

            return null;

        }

        #endregion

        #region (static) TryParse(PushEVSEStatusXML,  out PushEVSEStatus, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP push EVSE status request.
        /// </summary>
        /// <param name="PushEVSEStatusXML">The XML to parse.</param>
        /// <param name="PushEVSEStatus">The parsed push EVSE status request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                   PushEVSEStatusXML,
                                       out PushEVSEStatusRequest  PushEVSEStatus,
                                       OnExceptionDelegate        OnException  = null)
        {

            try
            {

                var OperatorEvseStatus  = OperatorEVSEStatus.Parse(PushEVSEStatusXML.ElementOrFail(OICPNS.EVSEStatus + "OperatorEvseStatus"));

                PushEVSEStatus = new PushEVSEStatusRequest(

                                     OperatorEvseStatus.EVSEStatusRecords,
                                     OperatorEvseStatus.OperatorId,
                                     OperatorEvseStatus.OperatorName,

                                     PushEVSEStatusXML.MapValueOrFail(OICPNS.EVSEStatus + "ActionType",
                                                                      XML_IO.AsActionType)

                                 );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, PushEVSEStatusXML, e);

                PushEVSEStatus = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(PushEVSEStatusText, out PushEVSEStatus, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP push EVSE status request.
        /// </summary>
        /// <param name="PushEVSEStatusText">The text to parse.</param>
        /// <param name="PushEVSEStatus">The parsed push EVSE status request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                     PushEVSEStatusText,
                                       out PushEVSEStatusRequest  PushEVSEStatus,
                                       OnExceptionDelegate        OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(PushEVSEStatusText).Root,
                             out PushEVSEStatus,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, PushEVSEStatusText, e);
            }

            PushEVSEStatus = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OICPNS.EVSEStatus + "eRoamingPushEvseStatus",

                                new XElement(OICPNS.EVSEStatus + "ActionType",              XML_IO.AsText(Action)),

                                new XElement(OICPNS.EVSEStatus + "OperatorEvseStatus",

                                    new XElement(OICPNS.EVSEStatus + "OperatorID",          OperatorId.OriginId),

                                    OperatorName.IsNotNullOrEmpty()
                                        ? new XElement(OICPNS.EVSEStatus + "OperatorName",  OperatorName)
                                        : null,

                                    EVSEStatusRecords.
                                        SafeSelect(evsestatusrecord => evsestatusrecord.ToXML())

                                )

                           );

        #endregion


        #region Operator overloading

        #region Operator == (PushEVSEStatus1, PushEVSEStatus2)

        /// <summary>
        /// Compares two push EVSE status requests for equality.
        /// </summary>
        /// <param name="PushEVSEStatus1">An push EVSE status request.</param>
        /// <param name="PushEVSEStatus2">Another push EVSE status request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PushEVSEStatusRequest PushEVSEStatus1, PushEVSEStatusRequest PushEVSEStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(PushEVSEStatus1, PushEVSEStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PushEVSEStatus1 == null) || ((Object) PushEVSEStatus2 == null))
                return false;

            return PushEVSEStatus1.Equals(PushEVSEStatus2);

        }

        #endregion

        #region Operator != (PushEVSEStatus1, PushEVSEStatus2)

        /// <summary>
        /// Compares two push EVSE status requests for inequality.
        /// </summary>
        /// <param name="PushEVSEStatus1">An push EVSE status request.</param>
        /// <param name="PushEVSEStatus2">Another push EVSE status request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PushEVSEStatusRequest PushEVSEStatus1, PushEVSEStatusRequest PushEVSEStatus2)

            => !(PushEVSEStatus1 == PushEVSEStatus2);

        #endregion

        #endregion

        #region IEquatable<PushEVSEStatus> Members

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

            var PushEVSEStatus = Object as PushEVSEStatusRequest;
            if ((Object) PushEVSEStatus == null)
                return false;

            return this.Equals(PushEVSEStatus);

        }

        #endregion

        #region Equals(PushEVSEStatus)

        /// <summary>
        /// Compares two push EVSE status requests for equality.
        /// </summary>
        /// <param name="PushEVSEStatus">An push EVSE status request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PushEVSEStatusRequest PushEVSEStatus)
        {

            if ((Object) PushEVSEStatus == null)
                return false;

            return EVSEStatusRecords.Count().Equals(PushEVSEStatus.EVSEStatusRecords.Count()) &&
                   OperatorId.               Equals(PushEVSEStatus.OperatorId)              &&

                   ((OperatorName == null && PushEVSEStatus.OperatorName == null) ||
                    (OperatorName != null && PushEVSEStatus.OperatorName != null && OperatorName.Equals(PushEVSEStatus.OperatorName))) &&

                   Action.                   Equals(PushEVSEStatus.Action);

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

                return EVSEStatusRecords.GetHashCode() * 17 ^
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
                             EVSEStatusRecords.Count(), " EVSE status record(s)",
                             " (", OperatorName != null ? OperatorName : "", " / ", OperatorName, ")");

        #endregion


    }

}
