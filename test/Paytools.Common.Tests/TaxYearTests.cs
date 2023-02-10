﻿// Copyright (c) 2023 Paytools Foundation
//
// Licensed under the Apache License, Version 2.0 (the "License");
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

using FluentAssertions;
using Paytools.Common.Model;

namespace Paytools.Common.Tests;

public class TaxYearTests
{
    [Fact]
    public void TestMonthlyTaxPeriod()
    {
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2022, new DateOnly(2021, 4, 6), PayFrequency.Monthly, 1);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2023, new DateOnly(2022, 5, 5), PayFrequency.Monthly, 1);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2019, new DateOnly(2018, 5, 6), PayFrequency.Monthly, 2);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2022, new DateOnly(2021, 12, 31), PayFrequency.Monthly, 9);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2020, new DateOnly(2020, 1, 5), PayFrequency.Monthly, 9);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2021, new DateOnly(2021, 1, 6), PayFrequency.Monthly, 10);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2021, new DateOnly(2021, 2, 15), PayFrequency.Monthly, 11);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2022, new DateOnly(2022, 3, 20), PayFrequency.Monthly, 12);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2022, new DateOnly(2022, 4, 5), PayFrequency.Monthly, 12);
    }

    [Fact]
    public void TestWeeklyTaxPeriod()
    {
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2022, new DateOnly(2021, 4, 6), PayFrequency.Weekly, 1);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2023, new DateOnly(2022, 5, 5), PayFrequency.Weekly, 5);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2019, new DateOnly(2018, 5, 15), PayFrequency.Weekly, 6);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2022, new DateOnly(2021, 12, 31), PayFrequency.Weekly, 39);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2020, new DateOnly(2020, 1, 5), PayFrequency.Weekly, 40);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2021, new DateOnly(2021, 1, 6), PayFrequency.Weekly, 40);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2021, new DateOnly(2021, 2, 15), PayFrequency.Weekly, 46);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2022, new DateOnly(2022, 3, 20), PayFrequency.Weekly, 50);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2022, new DateOnly(2022, 4, 3), PayFrequency.Weekly, 52);
        TaxYearTestHelper.RunTaxPeriodTest(TaxYearEnding.Apr5_2022, new DateOnly(2022, 4, 5), PayFrequency.Weekly, 53);
    }

    [Fact]
    public void TestInvalidTaxPeriod()
    {
        Action action = () =>
        {
            var taxYear = new TaxYear(TaxYearEnding.Apr5_2022);
            var periodNumber = taxYear.GetTaxPeriod(new DateOnly(2022, 4, 6), PayFrequency.Monthly);
        };

        //TaxYear.FromDate(new DateOnly((int)TaxYearEnding.MinValue, 4, 6));

        action.Should().Throw<ArgumentException>()
            .WithMessage("Pay date of 06/04/2022 is outside this tax year 06/04/2021 - 05/04/2022 (Parameter 'payDate')");
    }
}