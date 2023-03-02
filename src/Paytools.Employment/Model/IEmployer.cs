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

namespace Paytools.Employment.Model;

/// <summary>
/// Interface that represents an employer for payroll purposes.
/// </summary>
public interface IEmployer
{
    /// <summary>
    /// Gets or sets the employer's HMRC PAYE reference, if known.
    /// </summary>
    HmrcPayeReference? HmrcPayeReference { get; set; }

    /// <summary>
    /// Gets or sets the employer's HMRC Accounts Office reference, if known.
    /// </summary>
    HmrcAccountsOfficeReference? AccountsOfficeReference { get; set; }
}