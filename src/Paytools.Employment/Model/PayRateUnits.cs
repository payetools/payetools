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

namespace Paytools.Employment.Model;

/// <summary>
/// Enum representing different pay units, i.e., per annum, per hour, etc.
/// </summary>
public enum PayRateUnits
{
    /// <summary>
    /// Per annum pay type for salaried employees.
    /// </summary>
    PerAnnum,

    /// <summary>
    /// Hourly pay type for hourly-paid employees.
    /// </summary>
    PerHour,

    /// <summary>
    /// Daily rate, typically for salaried employees with regular working patterns.
    /// </summary>
    PerDay
}