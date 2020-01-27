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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// The unique identification of an OICP hub operator.
    /// </summary>
    public struct HubOperator_Id : IId,
                                   IEquatable <HubOperator_Id>,
                                   IComparable<HubOperator_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// The length of the hub operator identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId?.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new hub operator identification.
        /// based on the given string.
        /// </summary>
        private HubOperator_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a hub operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a hub operator identification.</param>
        public static HubOperator_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a hub operator must not be null or empty!");

            #endregion

            if (TryParse(Text, out HubOperator_Id hubOperatorId))
                return hubOperatorId;

            throw new ArgumentException("Illegal text representation of a hub operator identification: '" + Text + "'!", nameof(Text));

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a hub operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a hub operator identification.</param>
        public static HubOperator_Id? TryParse(String Text)
        {

            if (TryParse(Text, out HubOperator_Id hubOperatorId))
                return hubOperatorId;

            return new HubOperator_Id?();

        }

        #endregion

        #region TryParse(Text, out HubOperatorId)

        /// <summary>
        /// Try to parse the given string as a hub operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a hub operator identification.</param>
        /// <param name="HubOperatorId">The parsed hub operator identification.</param>
        public static Boolean TryParse(String Text, out HubOperator_Id HubOperatorId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                HubOperatorId = default;
                return false;
            }

            #endregion

            try
            {
                HubOperatorId = new HubOperator_Id(Text);
                return true;
            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            HubOperatorId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this hub operator identification.
        /// </summary>
        public HubOperator_Id Clone

            => new HubOperator_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (HubOperatorId1, HubOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HubOperatorId1">A hub operator identification.</param>
        /// <param name="HubOperatorId2">Another hub operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (HubOperator_Id HubOperatorId1, HubOperator_Id HubOperatorId2)
            => HubOperatorId1.Equals(HubOperatorId2);

        #endregion

        #region Operator != (HubOperatorId1, HubOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HubOperatorId1">A hub operator identification.</param>
        /// <param name="HubOperatorId2">Another hub operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (HubOperator_Id HubOperatorId1, HubOperator_Id HubOperatorId2)
            => !HubOperatorId1.Equals(HubOperatorId2);

        #endregion

        #region Operator <  (HubOperatorId1, HubOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HubOperatorId1">A hub operator identification.</param>
        /// <param name="HubOperatorId2">Another hub operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (HubOperator_Id HubOperatorId1, HubOperator_Id HubOperatorId2)
            => HubOperatorId1.CompareTo(HubOperatorId2) < 0;

        #endregion

        #region Operator <= (HubOperatorId1, HubOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HubOperatorId1">A hub operator identification.</param>
        /// <param name="HubOperatorId2">Another hub operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (HubOperator_Id HubOperatorId1, HubOperator_Id HubOperatorId2)
            => HubOperatorId1.CompareTo(HubOperatorId2) <= 0;

        #endregion

        #region Operator >  (HubOperatorId1, HubOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HubOperatorId1">A hub operator identification.</param>
        /// <param name="HubOperatorId2">Another hub operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (HubOperator_Id HubOperatorId1, HubOperator_Id HubOperatorId2)
            => HubOperatorId1.CompareTo(HubOperatorId2) > 0;

        #endregion

        #region Operator >= (HubOperatorId1, HubOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HubOperatorId1">A hub operator identification.</param>
        /// <param name="HubOperatorId2">Another hub operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (HubOperator_Id HubOperatorId1, HubOperator_Id HubOperatorId2)
            => HubOperatorId1.CompareTo(HubOperatorId2) >= 0;

        #endregion

        #endregion

        #region IComparable<HubOperatorId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is HubOperator_Id hubOperatorId
                   ? CompareTo(hubOperatorId)
                   : throw new ArgumentException("The given object is not a hub operator identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(HubOperatorId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HubOperatorId">An object to compare with.</param>
        public Int32 CompareTo(HubOperator_Id HubOperatorId)

            => String.Compare(InternalId,
                              HubOperatorId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<HubOperatorId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is HubOperator_Id hubOperatorId
                   ? Equals(hubOperatorId)
                   : false;

        #endregion

        #region Equals(HubOperatorId)

        /// <summary>
        /// Compares two HubOperatorIds for equality.
        /// </summary>
        /// <param name="HubOperatorId">A hub operator identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(HubOperator_Id HubOperatorId)

            => String.Equals(InternalId,
                             HubOperatorId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
            => InternalId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId;

        #endregion

    }

}
