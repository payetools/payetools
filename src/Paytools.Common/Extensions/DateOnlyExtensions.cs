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

using Paytools.Common.Model;

namespace Paytools.Common.Extensions;

public static class DateOnlyExtensions
{
    public static DateTime ToMiddayUtcDateTime(this DateOnly date) =>
        date.ToDateTime(new TimeOnly(12, 0), DateTimeKind.Utc);

    public static TaxYearEnding ToTaxYearEnding(this DateOnly date)
    {
        var apr6 = new DateOnly(date.Year, date.Month, 6);

        var taxYear = date < apr6 ? date.Year : date.Year + 1;

        if (taxYear < (int)TaxYearEnding.MinValue || taxYear > (int)TaxYearEnding.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(date), $"Unsupported tax year; date must fall within range tax year ending 6 April {(int)TaxYearEnding.MinValue} to 6 April {(int)TaxYearEnding.MaxValue}");

        return (TaxYearEnding)taxYear;
    }
}