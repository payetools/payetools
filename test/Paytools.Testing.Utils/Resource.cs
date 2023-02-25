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

using System.Reflection;

namespace Paytools.Testing.Utils;

public static class Resource
{
    public static Stream Load(string resourcePath)
    {
        var assembly = Assembly.GetCallingAssembly();

        return assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{resourcePath.Replace('\\','.')}") ??
            throw new ArgumentException($"Unable to load resource from path '{resourcePath}'", nameof(resourcePath));
    }
}