/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extension methods for the OICP result.
    /// </summary>
    public static class OICPResultExtensions
    {

        /// <summary>
        /// The given OICP result is/was successful.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="OICPResult">The OICP result.</param>
        public static Boolean IsSuccess<T>(this OICPResult<T> OICPResult)
            where T : IResponse

            => OICPResult is not null &&
                   OICPResult.IsSuccessful;

    }

    /// <summary>
    /// A generic OICP result.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public class OICPResult<T>
        where T : IResponse

    {

        #region Properties

        /// <summary>
        /// The request.
        /// </summary>
        public Object                Request             { get; }

        /// <summary>
        /// The result.
        /// </summary>
        public T?                    Response            { get; }

        /// <summary>
        /// The request is/was successful.
        /// </summary>
        public Boolean               IsSuccessful        { get; }

        /// <summary>
        /// The request is/was not successful.
        /// </summary>
        public Boolean               IsNotSuccessful
            => !IsSuccessful;

        /// <summary>
        /// Possible request data validation errors.
        /// </summary>
        public ValidationErrorList?  ValidationErrors    { get; }

        /// <summary>
        /// The process identification of the result.
        /// </summary>
        public Process_Id?           ProcessId           { get; }


        /// <summary>
        /// The timestamp of the response.
        /// </summary>
        public DateTime?             ResponseTimestamp
            => Response?.ResponseTimestamp;

        /// <summary>
        /// The runtime of the request leading to this response.
        /// </summary>
        public TimeSpan?             Runtime
            => Response?.Runtime;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new generic OICP result.
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <param name="Response">The result.</param>
        /// <param name="WasSuccessful">The request was successful.</param>
        /// <param name="ValidationErrors">Possible request data validation errors.</param>
        /// <param name="ProcessId">The process identification of the result.</param>
        private OICPResult(Object                Request,
                           T?                    Response,
                           Boolean               WasSuccessful,
                           ValidationErrorList?  ValidationErrors   = null,
                           Process_Id?           ProcessId          = null)
        {

            this.Request           = Request;
            this.Response          = Response;
            this.IsSuccessful     = WasSuccessful;
            this.ValidationErrors  = ValidationErrors;
            this.ProcessId         = ProcessId;

        }

        #endregion


        #region Success   (Request, Result, ProcessId = null)

        /// <summary>
        /// The request succeed.
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <param name="Result">The result.</param>
        /// <param name="ProcessId">The process identification of the result.</param>
        public static OICPResult<T> Success(Object       Request,
                                            T            Result,
                                            Process_Id?  ProcessId   = null)

            => new (Request,
                    Result,
                    true,
                    null,
                    ProcessId);

        #endregion

        #region Failed    (Request, Result, ProcessId = null)

        /// <summary>
        /// The request failed.
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <param name="Result">The result.</param>
        /// <param name="ProcessId">The process identification of the result.</param>
        public static OICPResult<T> Failed(Object       Request,
                                           T            Result,
                                           Process_Id?  ProcessId   = null)

            => new (Request,
                    Result,
                    false,
                    null,
                    ProcessId);

        #endregion

        #region BadRequest(Request, ValidationErrors = null, ProcessId = null)

        /// <summary>
        /// The request had some data validation errors.
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <param name="ValidationErrors">Possible request data validation errors.</param>
        /// <param name="ProcessId">The process identification of the result.</param>
        public static OICPResult<T> BadRequest(Object                Request,
                                               ValidationErrorList?  ValidationErrors   = null,
                                               Process_Id?           ProcessId          = null)

            => new (Request,
                    default,
                    false,
                    ValidationErrors,
                    ProcessId);

        #endregion


        #region From      (OICPResult)

        /// <summary>
        /// Create an OICPResult of type T from an OICPResult of type T2.
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <param name="Result">The result.</param>
        /// <param name="ProcessId">The process identification of the result.</param>
        public static OICPResult<T> From<T2>(OICPResult<T2> OICPResult)
            where T2 : IResponse

            => new (OICPResult.Request,
                    OICPResult.Response is not null
                        ? (T) (Object) OICPResult.Response
                        : default,
                    OICPResult.IsSuccessful,
                    OICPResult.ValidationErrors,
                    OICPResult.ProcessId);

        #endregion


        #region ToJSON(RequestJSON, ResponseJSON)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="RequestJSON">A JSON representation of the request.</param>
        /// <param name="ResponseJSON">A JSON representation of the response.</param>
        public JObject ToJSON(JObject? RequestJSON,
                              JObject? ResponseJSON)

            => JSONObject.Create(

                   RequestJSON is not null
                       ? new JProperty("request",           RequestJSON)
                       : null,

                   ResponseJSON is not null
                       ? new JProperty("response",          ResponseJSON)
                       : null,

                   new JProperty("wasSuccessful",           IsSuccessful),

                   ValidationErrors is not null
                       ? new JProperty("validationErrors",  ValidationErrors.ToJSON())
                       : null,

                   new JProperty("processId",               ProcessId.       ToString())

               );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(IsSuccessful      ? "Successful" : "Failed",
                             ProcessId.HasValue ? ", ProcessId: " + ProcessId : "");

        #endregion

    }

}
