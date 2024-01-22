﻿// Copyright (c) 2023-2024, Payetools Foundation.
//
// Payetools Foundation licenses this file to you under the following license(s):
//
//   * The MIT License, see https://opensource.org/license/mit/

using Payetools.IncomeTax.Model;
using Payetools.NationalInsurance.Model;
using Payetools.Pensions.Model;
using Payetools.StudentLoans.Model;

namespace Payetools.Payroll.Model;

/// <summary>
/// Interface that represents a payrun entry for one employee for a specific payrun.
/// </summary>
public interface IEmployeePayRunResult
{
    /// <summary>
    /// Gets information about this payrun.
    /// </summary>
    ref IPayRunDetails PayRunDetails { get; }

    /// <summary>
    /// Gets an employee accessor that can provide the employee details for this entry on demand.
    /// </summary>
    IEmployeeAccessor EmployeeAccessor { get; }

    /// <summary>
    /// Gets a value indicating whether this employee is being recorded as left employment in this payrun.  Note that
    /// the employee's leaving date may be before the start of the pay period for this payrun.
    /// </summary>
    bool IsLeaverInThisPayRun { get; }

    /// <summary>
    /// Gets the results of this employee's income tax calculation for this payrun.
    /// </summary>
    ref ITaxCalculationResult TaxCalculationResult { get; }

    /// <summary>
    /// Gets the results of this employee's National Insurance calculation for this payrun.
    /// </summary>
    ref INiCalculationResult NiCalculationResult { get; }

    /// <summary>
    /// Gets the results of this employee's student loan calculation for this payrun, if applicable;
    /// null otherwise.
    /// </summary>
    ref IStudentLoanCalculationResult StudentLoanCalculationResult { get; }

    /// <summary>
    /// Gets the results of this employee's pension calculation for this payrun, if applicable.;
    /// null otherwise.
    /// </summary>
    ref IPensionContributionCalculationResult PensionContributionCalculationResult { get; }

    /// <summary>
    /// Gets the employee's total gross pay.
    /// </summary>
    decimal TotalGrossPay { get; }

    /// <summary>
    /// Gets the employee's total pay that is subject to National Insurance.
    /// </summary>
    decimal NicablePay { get; }

    /// <summary>
    /// Gets the employee's total taxable pay, before the application of any tax-free pay.
    /// </summary>
    decimal TaxablePay { get; }

    /// <summary>
    /// Gets the employee's final net pay.
    /// </summary>
    decimal NetPay { get; }

    /// <summary>
    /// Gets the historical set of information for an employee's payroll for the current tax year,
    /// not including the effect of this payrun.
    /// </summary>
    ref IEmployeePayrollHistoryYtd EmployeePayrollHistoryYtd { get; }
}