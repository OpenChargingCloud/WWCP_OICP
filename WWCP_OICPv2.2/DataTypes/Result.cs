///*
// * Copyright (c) 2014-2020 GraphDefined GmbH
// * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//#region Usings

//using System;
//using System.Xml.Linq;

//using org.GraphDefined.Vanaheimr.Illias;

//#endregion

//namespace org.GraphDefined.WWCP.OICPv2_2
//{

//    /// <summary>
//    /// A generic OICP result.
//    /// </summary>
//    public class Result
//    {

//        #region Properties

//        /// <summary>
//        /// The machine-readable result code.
//        /// </summary>
//        public ResultCodes  ResultCode     { get; }

//        /// <summary>
//        /// A human-readable error description.
//        /// </summary>
//        public String       Description    { get; }

//        #endregion

//        #region Statics

//        /// <summary>
//        /// Unknown result code.
//        /// </summary>
//        /// <param name="Description">A human-readable error description.</param>
//        public static Result Unknown(String Description = null)

//            => new Result(ResultCodes.Unknown,
//                          Description);


//        /// <summary>
//        /// Data accepted and processed.
//        /// </summary>
//        /// <param name="Description">A human-readable error description.</param>
//        public static Result OK(String Description = null)

//            => new Result(ResultCodes.OK,
//                          Description);


//        /// <summary>
//        /// Only part of the data was accepted.
//        /// </summary>
//        /// <param name="Description">A human-readable error description.</param>
//        public static Result Partly(String Description = null)

//            => new Result(ResultCodes.Partly,
//                          Description);


//        /// <summary>
//        /// Wrong username and/or password.
//        /// </summary>
//        /// <param name="Description">A human-readable error description.</param>
//        public static Result NotAuthorized(String Description = null)

//            => new Result(ResultCodes.NotAuthorized,
//                          Description);


//        /// <summary>
//        /// One or more ID (EVSE/Contract) were not valid for this user.
//        /// </summary>
//        /// <param name="Description">A human-readable error description.</param>
//        public static Result InvalidId(String Description = null)

//            => new Result(ResultCodes.InvalidId,
//                          Description);


//        /// <summary>
//        /// Internal server error.
//        /// </summary>
//        /// <param name="Description">A human-readable error description.</param>
//        public static Result Server(String Description = null)

//            => new Result(ResultCodes.Server,
//                          Description);


//        /// <summary>
//        /// Data has technical errors.
//        /// </summary>
//        /// <param name="Description">A human-readable error description.</param>
//        public static Result Format(String Description = null)

//            => new Result(ResultCodes.Format,
//                          Description);

//        #endregion

//        #region Constructor(s)

//        /// <summary>
//        /// Create a new generic OICP result.
//        /// </summary>
//        /// <param name="ResultCode">The machine-readable result code.</param>
//        /// <param name="Description">A human-readable error description.</param>
//        private Result(ResultCodes  ResultCode,
//                       String       Description = null)
//        {

//            this.ResultCode   = ResultCode;

//            this.Description  = Description.IsNotNullOrEmpty()
//                                    ? Description.Trim()
//                                    : "";

//        }

//        #endregion


//        #region Documentation

//        // <ns:result>
//        //
//        //    <ns:resultCode>
//        //       <ns:resultCode>?</ns:resultCode>
//        //    </ns:resultCode>
//        //
//        //    <ns:resultDescription>?</ns:resultDescription>
//        //
//        // </ns:result>

//        #endregion

//        #region (static) Parse(ResultXML,  OnException = null)

//        /// <summary>
//        /// Parse the given XML representation of an OICP result.
//        /// </summary>
//        /// <param name="ResultXML">The XML to parse.</param>
//        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
//        public static Result Parse(XElement             ResultXML,
//                                   OnExceptionDelegate  OnException = null)
//        {

//            Result _Result;

//            if (TryParse(ResultXML, out _Result, OnException))
//                return _Result;

//            return null;

//        }

//        #endregion

//        #region (static) Parse(ResultText, OnException = null)

//        /// <summary>
//        /// Parse the given text-representation of an OICP result.
//        /// </summary>
//        /// <param name="ResultText">The text to parse.</param>
//        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
//        public static Result Parse(String               ResultText,
//                                   OnExceptionDelegate  OnException = null)
//        {

//            Result _Result;

//            if (TryParse(ResultText, out _Result, OnException))
//                return _Result;

//            return null;

//        }

//        #endregion

//        #region (static) TryParse(ResultXML,  out Result, OnException = null)

//        /// <summary>
//        /// Try to parse the given XML representation of an OICP result.
//        /// </summary>
//        /// <param name="ResultXML">The XML to parse.</param>
//        /// <param name="Result">The parsed result.</param>
//        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
//        public static Boolean TryParse(XElement             ResultXML,
//                                       out Result           Result,
//                                       OnExceptionDelegate  OnException  = null)
//        {

//            try
//            {

//        //        Result = new Result(

//        //                     ResultXML.MapValueOrFail       (OICPNS.Default + "resultCode",
//        //                                                     OICPNS.Default + "resultCode",
//        //                                                     XML_IO.AsResultCode),

//        //                     ResultXML.ElementValueOrDefault(OICPNS.Default + "resultDescription")

//        //                 );

//                Result = default(Result);

//                return true;

//            }
//            catch (Exception e)
//            {

//                OnException?.Invoke(DateTime.UtcNow, ResultXML, e);

//                Result = null;
//                return false;

//            }

//        }

//        #endregion

//        #region (static) TryParse(ResultText, out Result, OnException = null)

//        /// <summary>
//        /// Try to parse the given text-representation of an OICP result.
//        /// </summary>
//        /// <param name="ResultText">The text to parse.</param>
//        /// <param name="Result">The parsed result.</param>
//        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
//        public static Boolean TryParse(String               ResultText,
//                                       out Result           Result,
//                                       OnExceptionDelegate  OnException  = null)
//        {

//            try
//            {

//                if (TryParse(XDocument.Parse(ResultText).Root,
//                             out Result,
//                             OnException))

//                    return true;

//            }
//            catch (Exception e)
//            {
//                OnException?.Invoke(DateTime.UtcNow, ResultText, e);
//            }

//            Result = null;
//            return false;

//        }

//        #endregion

//        #region ToXML()

//        /// <summary>
//        /// Return a XML representation of this object.
//        /// </summary>
//        //public XElement ToXML()

//        //    => new XElement(OICPNS.Default + "result",

//        //           new XElement(OICPNS.Default + "resultCode",
//        //               new XElement(OICPNS.Default + "resultCode",     XML_IO.AsText(ResultCode))
//        //           ),

//        //           new XElement(OICPNS.Default + "resultDescription",  Description)

//        //       );

//        #endregion


//        #region Operator overloading

//        #region Operator == (Result1, Result2)

//        /// <summary>
//        /// Compares two results for equality.
//        /// </summary>
//        /// <param name="Result1">A result.</param>
//        /// <param name="Result2">Another result.</param>
//        /// <returns>True if both match; False otherwise.</returns>
//        public static Boolean operator == (Result Result1, Result Result2)
//        {

//            // If both are null, or both are same instance, return true.
//            if (ReferenceEquals(Result1, Result2))
//                return true;

//            // If one is null, but not both, return false.
//            if (((Object) Result1 == null) || ((Object) Result2 == null))
//                return false;

//            return Result1.Equals(Result2);

//        }

//        #endregion

//        #region Operator != (Result1, Result2)

//        /// <summary>
//        /// Compares two results for inequality.
//        /// </summary>
//        /// <param name="Result1">A result.</param>
//        /// <param name="Result2">Another result.</param>
//        /// <returns>False if both match; True otherwise.</returns>
//        public static Boolean operator != (Result Result1, Result Result2)

//            => !(Result1 == Result2);

//        #endregion

//        #endregion

//        #region IEquatable<Result> Members

//        #region Equals(Object)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="Object">An object to compare with.</param>
//        /// <returns>true|false</returns>
//        public override Boolean Equals(Object Object)
//        {

//            if (Object == null)
//                return false;

//            // Check if the given object is a result.
//            var Result = Object as Result;
//            if ((Object) Result == null)
//                return false;

//            return this.Equals(Result);

//        }

//        #endregion

//        #region Equals(Result)

//        /// <summary>
//        /// Compares two results for equality.
//        /// </summary>
//        /// <param name="Result">An result to compare with.</param>
//        /// <returns>True if both match; False otherwise.</returns>
//        public Boolean Equals(Result Result)
//        {

//            if ((Object) Result == null)
//                return false;

//            return this.ResultCode. Equals(Result.ResultCode) &&
//                   this.Description.Equals(Result.Description);

//        }

//        #endregion

//        #endregion

//        #region GetHashCode()

//        /// <summary>
//        /// Return the HashCode of this object.
//        /// </summary>
//        /// <returns>The HashCode of this object.</returns>
//        public override Int32 GetHashCode()
//        {
//            unchecked
//            {

//                return ResultCode. GetHashCode() * 11 ^

//                       (Description != null
//                            ? Description.GetHashCode()
//                            : 0);

//            }
//        }

//        #endregion

//        #region (override) ToString()

//        /// <summary>
//        /// Return a text-representation of this object.
//        /// </summary>
//        public override String ToString()

//            => ResultCode + (Description.IsNotNullOrEmpty() ? " - " + Description : "");

//        #endregion


//    }

//}
