/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extension methods for OICP process identifications.
    /// </summary>
    public static class ProcessIdExtensions
    {

        /// <summary>
        /// Indicates whether this process identification is null or empty.
        /// </summary>
        /// <param name="ProcessId">A process identification.</param>
        public static Boolean IsNullOrEmpty(this Process_Id? ProcessId)
            => !ProcessId.HasValue || ProcessId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this process identification is null or empty.
        /// </summary>
        /// <param name="ProcessId">A process identification.</param>
        public static Boolean IsNotNullOrEmpty(this Process_Id? ProcessId)
            => ProcessId.HasValue && ProcessId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of an OICP process.
    /// </summary>
    public readonly struct Process_Id : IId<Process_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this process identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this process identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the process identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP process identification based on the given string.
        /// </summary>
        private Process_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) NewRandom(Mapper = null)

        /// <summary>
        /// Generate a new random process identification.
        /// </summary>
        /// <param name="Mapper">A delegate to modify the newly generated process identification.</param>
        public static Process_Id NewRandom(Func<String, String>? Mapper = null)

            => new(Mapper is not null
                        ? Mapper(Guid.NewGuid().ToString())
                        : Guid.NewGuid().ToString());

        #endregion

        #region (static) Parse    (Text)

        /// <summary>
        /// Parse the given string as a process identification.
        /// </summary>
        /// <param name="Text">A text representation of a process identification.</param>
        public static Process_Id Parse(String Text)
        {

            if (TryParse(Text, out var processId))
                return processId;

            throw new ArgumentException($"Invalid text representation of a process identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse (Text)

        /// <summary>
        /// Try to parse the given string as a process identification.
        /// </summary>
        /// <param name="Text">A text representation of a process identification.</param>
        public static Process_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var processId))
                return processId;

            return null;

        }

        #endregion

        #region (static) TryParse (Text, out ProcessId)

        /// <summary>
        /// Try to parse the given string as a process identification.
        /// </summary>
        /// <param name="Text">A text representation of a process identification.</param>
        /// <param name="ProcessId">The parsed process identification.</param>
        public static Boolean TryParse(String Text, out Process_Id ProcessId)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ProcessId = new Process_Id(Text.Trim());
                    return true;
                }
                catch
                { }
            }

            ProcessId = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this process identification.
        /// </summary>
        public Process_Id Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Operator overloading

        #region Operator == (ProcessIdId1, ProcessIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProcessIdId1">A process identification.</param>
        /// <param name="ProcessIdId2">Another process identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Process_Id ProcessIdId1,
                                           Process_Id ProcessIdId2)

            => ProcessIdId1.Equals(ProcessIdId2);

        #endregion

        #region Operator != (ProcessIdId1, ProcessIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProcessIdId1">A process identification.</param>
        /// <param name="ProcessIdId2">Another process identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Process_Id ProcessIdId1,
                                           Process_Id ProcessIdId2)

            => !ProcessIdId1.Equals(ProcessIdId2);

        #endregion

        #region Operator <  (ProcessIdId1, ProcessIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProcessIdId1">A process identification.</param>
        /// <param name="ProcessIdId2">Another process identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (Process_Id ProcessIdId1,
                                          Process_Id ProcessIdId2)

            => ProcessIdId1.CompareTo(ProcessIdId2) < 0;

        #endregion

        #region Operator <= (ProcessIdId1, ProcessIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProcessIdId1">A process identification.</param>
        /// <param name="ProcessIdId2">Another process identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (Process_Id ProcessIdId1,
                                           Process_Id ProcessIdId2)

            => ProcessIdId1.CompareTo(ProcessIdId2) <= 0;

        #endregion

        #region Operator >  (ProcessIdId1, ProcessIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProcessIdId1">A process identification.</param>
        /// <param name="ProcessIdId2">Another process identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (Process_Id ProcessIdId1,
                                          Process_Id ProcessIdId2)

            => ProcessIdId1.CompareTo(ProcessIdId2) > 0;

        #endregion

        #region Operator >= (ProcessIdId1, ProcessIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProcessIdId1">A process identification.</param>
        /// <param name="ProcessIdId2">Another process identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (Process_Id ProcessIdId1,
                                           Process_Id ProcessIdId2)

            => ProcessIdId1.CompareTo(ProcessIdId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ProcessId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two process identifications for equality.
        /// </summary>
        /// <param name="Object">A process identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Process_Id processId
                   ? CompareTo(processId)
                   : throw new ArgumentException("The given object is not a process identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ProcessId)

        /// <summary>
        /// Compares two process identifications.
        /// </summary>
        /// <param name="ProcessId">A process identification to compare with.</param>
        public Int32 CompareTo(Process_Id ProcessId)

            => String.Compare(InternalId,
                              ProcessId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ProcessId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two process identifications for equality.
        /// </summary>
        /// <param name="Object">A process identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Process_Id processId &&
                   Equals(processId);

        #endregion

        #region Equals(ProcessId)

        /// <summary>
        /// Compares two process identifications for equality.
        /// </summary>
        /// <param name="ProcessId">A process identification to compare with.</param>
        public Boolean Equals(Process_Id ProcessId)

            => String.Equals(InternalId,
                             ProcessId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
