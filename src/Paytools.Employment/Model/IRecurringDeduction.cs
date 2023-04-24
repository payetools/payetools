﻿// Copyright (c) 2023 Paytools Foundation.
//
// Paytools Foundation licenses this file to you under one of the following licenses:
//
//   * GNU Affero General Public License, see https://www.gnu.org/licenses/agpl-3.0.html
//   * Paytools Commercial Use license [TBA]
//
// For further information on licensing options, see https://paytools.dev/licensing-paytools.html

using Paytools.Common.Model;

namespace Paytools.Employment.Model;

/// <summary>
/// Interface that represents a recurring deduction for an employee.
/// </summary>
public interface IRecurringDeduction : IApplicableFromTill, IPayrollAmount
{
    /// <summary>
    /// Gets the deduction type for this recurring deduction.
    /// </summary>
    IDeductionDetails DeductionType { get; }
}
