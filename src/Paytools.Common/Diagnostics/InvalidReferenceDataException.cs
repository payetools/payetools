﻿// Copyright (c) 2023 Paytools Foundation.
//
// Paytools Foundation licenses this file to you under one of the following licenses:
//
//  * GNU Affero General Public License, see https://www.gnu.org/licenses/agpl-3.0.html
//  * Paytools Commercial Use license [TBA]
//
// For further information on licensing options, see https://paytools.dev/licensing-paytools.html

namespace Paytools.Common.Diagnostics;

/// <summary>
/// Exception that is thrown when invalid reference data is provided.
/// </summary>
public class InvalidReferenceDataException : Exception
{
    /// <summary>
    /// Initialises a new instance of the <see cref="InvalidReferenceDataException"/> class.
    /// </summary>
    /// <param name="message">Human-readable text providing further details on the exception.</param>
    public InvalidReferenceDataException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="InvalidReferenceDataException"/> class with the supplied
    /// inner exception.
    /// </summary>
    /// <param name="message">Human-readable text providing further details on the exception.</param>
    /// <param name="innerException">Inner exception.</param>
    public InvalidReferenceDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}