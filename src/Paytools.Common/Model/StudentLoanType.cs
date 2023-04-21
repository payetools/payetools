﻿// Copyright (c) 2023 Paytools Foundation.
//
// Paytools Foundation licenses this file to you under one of the following licenses:
//
//  * GNU Affero General Public License, see https://www.gnu.org/licenses/agpl-3.0.html
//  * Paytools Commercial Use license [TBA]
//
// For further information on licensing options, see https://paytools.dev/licensing-paytools.html

namespace Paytools.Common.Model;

/// <summary>
/// Enum that represents a given type of student loan plan.  Post-graduate loans are handled separately.
/// </summary>
public enum StudentLoanType
{
    /// <summary>Student loan Plan 1 type</summary>
    Plan1,

    /// <summary>Student loan Plan 2 type</summary>
    Plan2,

    /// <summary>Student loan Plan 4 type</summary>
    Plan4
}