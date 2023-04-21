﻿// Copyright (c) 2023 Paytools Foundation.
//
// Paytools Foundation licenses this file to you under one of the following licenses:
//
//  * GNU Affero General Public License, see https://www.gnu.org/licenses/agpl-3.0.html
//  * Paytools Commercial Use license [TBA]
//
// For further information on licensing options, see https://paytools.dev/licensing-paytools.html

using Paytools.Common.Extensions;

namespace Paytools.Common.Model;

/// <summary>
/// Represents a specific pay date for a specific pay frequency.
/// </summary>
public readonly struct PayDate
{
    /// <summary>
    /// Gets the date of this <see cref="PayDate"/>.
    /// </summary>
    public DateOnly Date { get; init; }

    /// <summary>
    /// Gets the <see cref="TaxYear"/> for this <see cref="PayDate"/>.
    /// </summary>
    public TaxYear TaxYear { get; init; }

    /// <summary>
    /// Gets the tax period for this <see cref="PayDate"/>, for example, a pay date of 20th May for a monthly
    /// payroll would be tax period 2.
    /// </summary>
    public int TaxPeriod { get; init; }

    /// <summary>
    /// Gets the pay frequency for this <see cref="PayDate"/>.
    /// </summary>
    public PayFrequency PayFrequency { get; init; }

    /// <summary>
    /// Initialises a new <see cref="PayDate"/> based on the supplied date and pay frequency.
    /// </summary>
    /// <param name="date">Pay date.</param>
    /// <param name="payFrequency">Pay frequency.</param>
    public PayDate(DateOnly date, PayFrequency payFrequency)
    {
        Date = date;
        TaxYear = new TaxYear(date);
        TaxPeriod = TaxYear.GetTaxPeriod(date, payFrequency);
        PayFrequency = payFrequency;
    }

    /// <summary>
    /// Initialises a new <see cref="PayDate"/> based on the supplied date and pay frequency.
    /// </summary>
    /// <param name="year">Year.</param>
    /// <param name="month">Month (1-12).</param>
    /// <param name="day">Day.</param>
    /// <param name="payFrequency">Pay frequency.</param>
    public PayDate(int year, int month, int day, PayFrequency payFrequency)
        : this(new DateOnly(year, month, day), payFrequency)
    {
    }

    /// <summary>
    /// Gets the equivalent <see cref="DateTime"/> for this paydate, with the time portion set to midday (12:00:00) UTC.
    /// </summary>
    /// <param name="payDate"><see cref="PayDate"/> to get the DateTime for.</param>
    public static implicit operator DateTime(PayDate payDate) => payDate.Date.ToMiddayUtcDateTime();
}