﻿// Copyright (c) 2023 Paytools Foundation.
//
// Paytools Foundation licenses this file to you under one of the following licenses:
//
//  * GNU Affero General Public License, see https://www.gnu.org/licenses/agpl-3.0.html
//  * Paytools Commercial Use license [TBA]
//
// For further information on licensing options, see https://paytools.dev/licensing-paytools.html

using Paytools.Common.Model;

namespace Paytools.Pensions.Model;

/// <summary>
/// Interface for types that represent pension schemes.
/// </summary>
/// <remarks>A <see cref="IPensionScheme"/> instance refers to a pension scheme with a specific provider, with
/// specific tax treatment and earnings basis.  Whilst it is not common for pension schemes to change
/// tax treatment, it is quite possible for an employer to operate more than one type of earnings basis
/// across its employee base.  In this case, two (or more) instances of this type would be required, one
/// for each earnings basis in use, even though all contributions might be flowing to a single scheme
/// with the same provider.</remarks>
public interface IPensionScheme
{
    /// <summary>
    /// Gets the earnings basis for this pension scheme.
    /// </summary>
    PensionsEarningsBasis EarningsBasis { get; }

    /// <summary>
    /// Gets the tax treatment for this pension scheme.
    /// </summary>
    PensionTaxTreatment TaxTreatment { get; }
}
