﻿// Copyright (c) 2023 Paytools Foundation.  All rights reserved.
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

using Microsoft.Extensions.DependencyInjection;

namespace Paytools.ReferenceData.Tests;

public class HmrcReferenceDataProviderTests
{
    [Fact]
    public async Task LoadTestDataAsync()
    {
        var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();

        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>() ??
            throw new InvalidOperationException("Unable to create HttpClientfactory");

        var factory = new HmrcReferenceDataProviderFactory(httpClientFactory);

        var provider = await factory.CreateProviderAsync(new Uri("https://stellular-bombolone-34e67e.netlify.app/index.json"));
    }

}
