﻿// Copyright (c) 2023 Paytools Foundation.
//
// Licensed under the Apache License, Version 2.0 (the "License") ~
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

using Paytools.Common.Model;
using Paytools.IncomeTax;
using Paytools.NationalInsurance;
using Paytools.Payroll.Model;
using Paytools.Pensions;
using Paytools.StudentLoans;

namespace Paytools.Payroll.Payruns;

/// <summary>
/// Represents the calculator that can process an employee's set of input payroll data and
/// provide the results of the calculations in the form of an <see cref="IEmployeePayrunResult"/>.
/// </summary>
public class PayrunCalculator : IPayrunCalculator
{
    private readonly Dictionary<CountriesForTaxPurposes, ITaxCalculator> _incomeTaxCalculators;
    private readonly INiCalculator _niCalculator;
    private readonly IPensionContributionCalculatorFactory _pensionCalculatorFactory;
    private readonly IStudentLoanCalculator _studentLoanCalculator;

    /// <summary>
    /// Initialises a new instance of <see cref="PayrunCalculator"/> with the supplied factories
    /// and specified pay date.
    /// </summary>
    /// <param name="incomeTaxCalcFactory">Income tax calculator factory.</param>
    /// <param name="niCalcFactory">calculator factory.</param>
    /// <param name="pensionCalcFactory">Pension contributions calculator factory.</param>
    /// <param name="studentLoanCalcFactory">Student loan calculator factory.</param>
    /// <param name="payDate">Pay date for this payrun.</param>
    public PayrunCalculator(
        ITaxCalculatorFactory incomeTaxCalcFactory,
        INiCalculatorFactory niCalcFactory,
        IPensionContributionCalculatorFactory pensionCalcFactory,
        IStudentLoanCalculatorFactory studentLoanCalcFactory,
        PayDate payDate)
    {
        _incomeTaxCalculators = payDate.TaxYear.GetCountriesForYear()
            .Select(regime => (regime, calculator: incomeTaxCalcFactory.GetCalculator(regime, payDate)))
            .ToDictionary(kv => kv.regime, kv => kv.calculator);
        _niCalculator = niCalcFactory.GetCalculator(payDate);
        _pensionCalculatorFactory = pensionCalcFactory;
        _studentLoanCalculator = studentLoanCalcFactory.GetCalculator(payDate);
    }

    /// <summary>
    /// Processes the supplied payrun entry calculating all the earnings and deductions, income tax, national insurance and
    /// other statutory deductions, and generating a result structure which includes the final net pay.
    /// </summary>
    /// <param name="entry">Instance of <see cref="IEmployeePayrunEntry"/> containing all the necessary input data for the
    /// payroll calculation.</param>
    /// <returns>An instance of <see cref="IEmployeePayrunResult"/> containing the results of the payroll calculations.</returns>
    public ref IEmployeePayrunResult Process(ref IEmployeePayrunEntry entry)
    {
        throw new NotImplementedException();
    }
}