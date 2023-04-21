// Copyright (c) 2023 Paytools Foundation.
//
// Paytools Foundation licenses this file to you under one of the following licenses:
//
//  * GNU Affero General Public License, see https://www.gnu.org/licenses/agpl-3.0.html
//  * Paytools Commercial Use license [TBA]
//
// For further information on licensing options, see https://paytools.dev/licensing-paytools.html

using Paytools.Common.Model;
using Paytools.StudentLoans.Model;

namespace Paytools.Testing.Data.EndToEnd;

public class StaticInputTestDataEntry : IStaticInputTestDataEntry
{
    public string DefinitionVersion { get; set; } = string.Empty;
    public string TestIdentifier { get; set; } = string.Empty;
    public TaxYearEnding TaxYearEnding { get; set; }
    public string TestReference { get; set; } = string.Empty;
    public string EmployeeFirstName { get; set; } = string.Empty;
    public string EmployeeLastName { get; set; } = string.Empty;
    public TaxCode TaxCode { get; set; }
    public NiCategory NiCategory { get; set; }
    public StudentLoanType? StudentLoanPlan { get; set; }
    public bool GraduateLoan { get; set; }
    public string? PensionScheme { get; set; }
    public bool UsesSalaryExchange { get; set; }
    public decimal? EmployeePercentage { get; set; }
    public decimal? EmployeeFixedAmount { get; set; }
    public decimal? EmployerPercentage { get; set; }
}
