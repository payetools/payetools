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

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Paytools.Common.Serialization;

/// <summary>
/// JSON Converter for instances of <see cref="DateOnly"/> types.
/// </summary>
public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string DateOnlyJsonFormat = "yyyy-MM-dd";

    /// <summary>
    /// Reads an ISO 8601 format date in string format and converts to the appropriate <see cref="DateOnly"/> value.
    /// </summary>
    /// <param name="reader">JSON reader (UTF-8 format).</param>
    /// <param name="typeToConvert">Type to convert (unused).</param>
    /// <param name="options">JSON serializer options (unused).</param>
    /// <returns><see cref="DateOnly"/> value.</returns>
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        DateOnly.ParseExact(reader.GetString() ?? string.Empty, DateOnlyJsonFormat, CultureInfo.InvariantCulture);

    /// <summary>
    /// Writes a <see cref="DateOnly"/> value to the JSON stream in ISO 8601 format date in string format.
    /// </summary>
    /// <param name="writer">JSON writer (UTF-8 format).</param>
    /// <param name="value">DateOnly value to convert.</param>
    /// <param name="options">JSON serializer options (unused).</param>
    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString(DateOnlyJsonFormat, CultureInfo.InvariantCulture));
}