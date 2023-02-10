﻿// Copyright (c) 2023 Paytools Foundation
//
// Licensed under the Apache License, Version 2.0 (the "License")~
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Text.RegularExpressions;

namespace Paytools.Common.Model;

/// <summary>
/// Represents a Uk National Insurance number, also referred to as NI number or NINO.  A NINO is made up of 2 letters, 6 numbers
/// and a final letter which is always A, B, C or D.  (See HMRC's National Insurance Manual, section NIM39110 
/// (<see href="https://www.gov.uk/hmrc-internal-manuals/national-insurance-manual/nim39110"/>.)
/// </summary>
public readonly struct NiNumber
{
    private static readonly Regex _validationRegex =
        new Regex("^(?!BG)(?!GB)(?!NK)(?!KN)(?!TN)(?!NT)(?!ZZ)(?:[A-CEGHJ-PR-TW-Z][A-CEGHJ-NPR-TW-Z])(?:\\s*\\d\\s*){6}([A-D]|\\s)$");

    private readonly string _value;

    /// <summary>
    /// Operator for casting implicitly from a <see cref="NiNumber"/> instance to its string representation. 
    /// </summary>
    /// <param name="niNumber">NI number.</param>
    public static implicit operator string(NiNumber niNumber) => niNumber._value;

    /// <summary>
    /// Operator for casting implicitly from a string to an instance of a <see cref="NiNumber"/> 
    /// </summary>
    /// <param name="niNumber">NI number as string.</param>
    /// <exception cref="ArgumentException">Thrown if the supplied string is not a valid NI number.</exception>
    public static implicit operator NiNumber(string niNumber) => new NiNumber(niNumber);

    /// <summary>
    /// Instantiates a new <see cref="NiNumber"/> instance.
    /// </summary>
    /// <param name="niNumber">National insurance number as string.</param>
    /// <exception cref="ArgumentException">Thrown if the supplied string is not a valid NI number.</exception>
    public NiNumber(string niNumber)
    {
        var tidiedNiNumber = niNumber.ToUpper().Replace(" ", "");

        if (!IsValid(tidiedNiNumber))
            throw new ArgumentException("Argument is not a valid National Insurance Number", nameof(niNumber));

        _value = tidiedNiNumber;
    }

    /// <summary>
    /// Verifies whether the supplied string could be a valid NI number.
    /// </summary>
    /// <param name="value">String value to check.</param>
    /// <returns>True if the supplied value could be a valid NI number; false otherwise.</returns>
    /// <remarks>Although this method confirms whether the string supplied <em>could</em> be a valid Ni nummber,
    /// it does not guarantee that the supplied value is registered with HMRC against a given individual.</remarks>    
    public static bool IsValid(string value) => _validationRegex.IsMatch(value);

    /// <summary>
    /// Gets the string representation of this <see cref="NiNumber"/>.
    /// </summary>
    /// <returns>String representation of this NI number in non-spaced format.</returns>
    public override string ToString() => _value;

    /// <summary>
    /// Gets the string representation of this <see cref="NiNumber"/>.
    /// </summary>
    /// <param name="asSpacedFormat">True if spaced format is to be returned (e.g., "NA 12 34 67 C";
    /// false otherwise.</param>
    /// <returns>String representation of this NI number in either spaced or non-spaced format.</returns>
    public string ToString(bool asSpacedFormat) =>
        asSpacedFormat ? ToSpacedFormat(_value) : _value;

    private static string ToSpacedFormat(string value) =>
        value.Length == 9 ? $"{value[..2]} {value[2..4]} {value[4..6]} {value[6..8]} {value[^1]}" :
            throw new InvalidOperationException("NI numbers must be 9 characters in length");
}