/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
using System.Threading;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The PullEVSEStatusById request.
    /// </summary>
    public class PullEVSEStatusByIdRequest : ARequest<PullEVSEStatusByIdRequest>
    {

        #region Properties

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        public Provider_Id           ProviderId     { get; }

        /// <summary>
        /// The enumeration of EVSE identifications to query.
        /// </summary>
        public IEnumerable<EVSE_Id>  EVSEIds        { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PullEVSEStatusById request.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="EVSEIds">An enumeration of up to 100 EVSE identifications to query.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public PullEVSEStatusByIdRequest(Provider_Id           ProviderId,
                                         IEnumerable<EVSE_Id>  EVSEIds,

                                         DateTime?             Timestamp           = null,
                                         CancellationToken?    CancellationToken   = null,
                                         EventTracking_Id      EventTrackingId     = null,
                                         TimeSpan?             RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            if (EVSEIds.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(EVSEIds),     "The given enumeration of EVSE identifications must not be null!");

            this.ProviderId  = ProviderId;
            this.EVSEIds     = EVSEIds;

        }

        #endregion


        #region Documentation

        // {
        //   "ProviderID": "string",
        //   "EvseID": [
        //     "string"
        //   ]
        // }

        #endregion

        //#region (static) Parse(PullEVSEStatusByIdXML,  ..., OnException = null, ...)

        ///// <summary>
        ///// Parse the given XML representation of an OICP pull EVSE status by id request.
        ///// </summary>
        ///// <param name="PullEVSEStatusByIdXML">The XML to parse.</param>
        ///// <param name="CustomPullEVSEStatusByIdRequestParser">A delegate to parse custom PullEVSEStatusById requests.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //public static PullEVSEStatusByIdRequest Parse(XElement                                            PullEVSEStatusByIdXML,
        //                                              CustomXMLParserDelegate<PullEVSEStatusByIdRequest>  CustomPullEVSEStatusByIdRequestParser   = null,
        //                                              OnExceptionDelegate                                 OnException                             = null,

        //                                              DateTime?                                           Timestamp                               = null,
        //                                              CancellationToken?                                  CancellationToken                       = null,
        //                                              EventTracking_Id                                    EventTrackingId                         = null,
        //                                              TimeSpan?                                           RequestTimeout                          = null)

        //{

        //    if (TryParse(PullEVSEStatusByIdXML,
        //                 out PullEVSEStatusByIdRequest _PullEVSEStatusById,
        //                 CustomPullEVSEStatusByIdRequestParser,
        //                 OnException,

        //                 Timestamp,
        //                 CancellationToken,
        //                 EventTrackingId,
        //                 RequestTimeout))

        //        return _PullEVSEStatusById;

        //    return null;

        //}

        //#endregion

        //#region (static) Parse(PullEVSEStatusByIdText, ..., OnException = null, ...)

        ///// <summary>
        ///// Parse the given text-representation of an OICP pull EVSE status by id request.
        ///// </summary>
        ///// <param name="PullEVSEStatusByIdText">The text to parse.</param>
        ///// <param name="CustomPullEVSEStatusByIdRequestParser">A delegate to parse custom PullEVSEStatusById requests.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //public static PullEVSEStatusByIdRequest Parse(String                                              PullEVSEStatusByIdText,
        //                                              CustomXMLParserDelegate<PullEVSEStatusByIdRequest>  CustomPullEVSEStatusByIdRequestParser   = null,
        //                                              OnExceptionDelegate                                 OnException                             = null,

        //                                              DateTime?                                           Timestamp                               = null,
        //                                              CancellationToken?                                  CancellationToken                       = null,
        //                                              EventTracking_Id                                    EventTrackingId                         = null,
        //                                              TimeSpan?                                           RequestTimeout                          = null)

        //{

        //    if (TryParse(PullEVSEStatusByIdText,
        //                 out PullEVSEStatusByIdRequest _PullEVSEStatusById,
        //                 CustomPullEVSEStatusByIdRequestParser,
        //                 OnException,

        //                 Timestamp,
        //                 CancellationToken,
        //                 EventTrackingId,
        //                 RequestTimeout))

        //        return _PullEVSEStatusById;

        //    return null;

        //}

        //#endregion

        //#region (static) TryParse(PullEVSEStatusByIdXML,  out PullEVSEStatusById, ..., OnException = null, ...)

        ///// <summary>
        ///// Try to parse the given XML representation of an OICP pull EVSE status by id request.
        ///// </summary>
        ///// <param name="PullEVSEStatusByIdXML">The XML to parse.</param>
        ///// <param name="PullEVSEStatusById">The parsed pull EVSE status by id request.</param>
        ///// <param name="CustomPullEVSEStatusByIdRequestParser">A delegate to parse custom PullEVSEStatusById requests.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //public static Boolean TryParse(XElement                                            PullEVSEStatusByIdXML,
        //                               out PullEVSEStatusByIdRequest                       PullEVSEStatusById,
        //                               CustomXMLParserDelegate<PullEVSEStatusByIdRequest>  CustomPullEVSEStatusByIdRequestParser   = null,
        //                               OnExceptionDelegate                                 OnException                             = null,

        //                               DateTime?                                           Timestamp                               = null,
        //                               CancellationToken?                                  CancellationToken                       = null,
        //                               EventTracking_Id                                    EventTrackingId                         = null,
        //                               TimeSpan?                                           RequestTimeout                          = null)

        //{

        //    try
        //    {

        //        if (PullEVSEStatusByIdXML.Name != OICPNS.EVSEStatus + "eRoamingPullEvseStatusById")
        //        {
        //            PullEVSEStatusById = null;
        //            return false;
        //        }

        //        PullEVSEStatusById = new PullEVSEStatusByIdRequest(PullEVSEStatusByIdXML.MapValueOrFail (OICPNS.EVSEStatus + "ProviderID",
        //                                                                                                 Provider_Id.Parse),

        //                                                           PullEVSEStatusByIdXML.MapValuesOrFail(OICPNS.EVSEStatus + "EvseId",
        //                                                                                                 EVSE_Id.Parse),

        //                                                           Timestamp,
        //                                                           CancellationToken,
        //                                                           EventTrackingId,
        //                                                           RequestTimeout);


        //        if (CustomPullEVSEStatusByIdRequestParser != null)
        //            PullEVSEStatusById = CustomPullEVSEStatusByIdRequestParser(PullEVSEStatusByIdXML,
        //                                                                       PullEVSEStatusById);

        //        return true;

        //    }
        //    catch (Exception e)
        //    {

        //        OnException?.Invoke(DateTime.UtcNow, PullEVSEStatusByIdXML, e);

        //        PullEVSEStatusById = null;
        //        return false;

        //    }

        //}

        //#endregion

        //#region (static) TryParse(PullEVSEStatusByIdText, out PullEVSEStatusById, ..., OnException = null, ...)

        ///// <summary>
        ///// Try to parse the given text-representation of an OICP pull EVSE status by id request.
        ///// </summary>
        ///// <param name="PullEVSEStatusByIdText">The text to parse.</param>
        ///// <param name="PullEVSEStatusById">The parsed pull EVSE status by id request.</param>
        ///// <param name="CustomPullEVSEStatusByIdRequestParser">A delegate to parse custom PullEVSEStatusById requests.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //public static Boolean TryParse(String                                              PullEVSEStatusByIdText,
        //                               out PullEVSEStatusByIdRequest                       PullEVSEStatusById,
        //                               CustomXMLParserDelegate<PullEVSEStatusByIdRequest>  CustomPullEVSEStatusByIdRequestParser   = null,
        //                               OnExceptionDelegate                                 OnException                             = null,

        //                               DateTime?                                           Timestamp                               = null,
        //                               CancellationToken?                                  CancellationToken                       = null,
        //                               EventTracking_Id                                    EventTrackingId                         = null,
        //                               TimeSpan?                                           RequestTimeout                          = null)

        //{

        //    try
        //    {

        //        if (TryParse(XDocument.Parse(PullEVSEStatusByIdText).Root,
        //                     out PullEVSEStatusById,
        //                     CustomPullEVSEStatusByIdRequestParser,
        //                     OnException,

        //                     Timestamp,
        //                     CancellationToken,
        //                     EventTrackingId,
        //                     RequestTimeout))

        //            return true;

        //    }
        //    catch (Exception e)
        //    {
        //        OnException?.Invoke(DateTime.UtcNow, PullEVSEStatusByIdText, e);
        //    }

        //    PullEVSEStatusById = null;
        //    return false;

        //}

        //#endregion

        //#region ToXML(CustomPullEVSEStatusByIdRequestSerializer = null)

        ///// <summary>
        ///// Return a XML representation of this object.
        ///// </summary>
        ///// <param name="CustomPullEVSEStatusByIdRequestSerializer">A delegate to serialize custom eRoamingPullEvseStatusById XML elements.</param>
        //public XElement ToXML(CustomXMLSerializerDelegate<PullEVSEStatusByIdRequest>  CustomPullEVSEStatusByIdRequestSerializer = null)
        //{

        //    var XML = new XElement(OICPNS.EVSEStatus + "eRoamingPullEvseStatusById",

        //                           new XElement(OICPNS.EVSEStatus + "ProviderID", ProviderId.ToString()),

        //                           EVSEIds.SafeSelect(evseid => new XElement(OICPNS.EVSEStatus + "EvseId", evseid.ToString()))

        //                          );

        //    return CustomPullEVSEStatusByIdRequestSerializer != null
        //               ? CustomPullEVSEStatusByIdRequestSerializer(this, XML)
        //               : XML;

        //}

        //#endregion


        #region Operator overloading

        #region Operator == (PullEVSEStatusById1, PullEVSEStatusById2)

        /// <summary>
        /// Compares two pull EVSE status by id requests for equality.
        /// </summary>
        /// <param name="PullEVSEStatusById1">An pull EVSE status by id request.</param>
        /// <param name="PullEVSEStatusById2">Another pull EVSE status by id request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullEVSEStatusByIdRequest PullEVSEStatusById1,
                                           PullEVSEStatusByIdRequest PullEVSEStatusById2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PullEVSEStatusById1, PullEVSEStatusById2))
                return true;

            // If one is null, but not both, return false.
            if ((PullEVSEStatusById1 is null) || (PullEVSEStatusById2 is null))
                return false;

            return PullEVSEStatusById1.Equals(PullEVSEStatusById2);

        }

        #endregion

        #region Operator != (PullEVSEStatusById1, PullEVSEStatusById2)

        /// <summary>
        /// Compares two pull EVSE status by id requests for inequality.
        /// </summary>
        /// <param name="PullEVSEStatusById1">An pull EVSE status by id request.</param>
        /// <param name="PullEVSEStatusById2">Another pull EVSE status by id request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullEVSEStatusByIdRequest PullEVSEStatusById1,
                                           PullEVSEStatusByIdRequest PullEVSEStatusById2)

            => !(PullEVSEStatusById1 == PullEVSEStatusById2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEStatusByIdRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is PullEVSEStatusByIdRequest pullEVSEStatusByIdRequest &&
                   Equals(pullEVSEStatusByIdRequest);

        #endregion

        #region Equals(PullEVSEStatusByIdRequest)

        /// <summary>
        /// Compares two pull EVSE status by id requests for equality.
        /// </summary>
        /// <param name="PullEVSEStatusByIdRequest">An pull EVSE status by id request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEStatusByIdRequest PullEVSEStatusByIdRequest)

            => !(PullEVSEStatusByIdRequest is null) &&

                 ProviderId.Equals(PullEVSEStatusByIdRequest.ProviderId) &&

                 EVSEIds.Count().Equals(PullEVSEStatusByIdRequest.EVSEIds.Count()) &&
                 EVSEIds.All(operatorId => PullEVSEStatusByIdRequest.EVSEIds.Contains(operatorId));

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

                return ProviderId.GetHashCode() * 3 ^
                       EVSEIds.Aggregate(0, (hashCode, evseId) => hashCode ^ evseId.GetHashCode());

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(ProviderId, ", ",
                             EVSEIds.Count(), " EVSE identifications");

        #endregion

    }

}
