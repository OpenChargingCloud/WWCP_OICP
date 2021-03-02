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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    public class OICPResult<T>
    {

        #region Properties

        public Object               Request             { get; }

        public T                    Result              { get; }

        public Boolean              IsSuccess           { get; }

        public ValidationErrorList  ValidationErrors    { get; }

        public Process_Id?          ProcessId           { get; }

        #endregion

        #region Constructor(s)

        private OICPResult(Object               Request,
                           Boolean              IsSuccess,
                           T                    Result,
                           ValidationErrorList  ValidationErrors,
                           Process_Id?          ProcessId)
        {

            this.Request           = Request;
            this.IsSuccess         = IsSuccess;
            this.Result            = Result;
            this.ValidationErrors  = ValidationErrors;
            this.ProcessId         = ProcessId;

        }

        #endregion


        public static OICPResult<T> Success(Object       Request,
                                            T            Result,
                                            Process_Id?  ProcessId   = null)

            => new OICPResult<T>(Request,
                                 true,
                                 Result,
                                 null,
                                 ProcessId);


        public static OICPResult<T> Failed(Object       Request,
                                           T            Result,
                                           Process_Id?  ProcessId   = null)

            => new OICPResult<T>(Request,
                                 false,
                                 Result,
                                 null,
                                 ProcessId);

        public static OICPResult<T> BadRequest(Object               Request,
                                               ValidationErrorList  ValidationErrors   = null,
                                               Process_Id?          ProcessId          = null)

            => new OICPResult<T>(Request,
                                 false,
                                 default,
                                 ValidationErrors,
                                 ProcessId);


    }

}
