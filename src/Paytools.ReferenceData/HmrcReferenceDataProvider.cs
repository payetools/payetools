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

using Paytools.Common.Diagnostics;
using Paytools.Common.Model;
using Paytools.IncomeTax.ReferenceData;
using Paytools.NationalInsurance;
using Paytools.NationalInsurance.ReferenceData;
using Paytools.ReferenceData.IncomeTax;
using Paytools.ReferenceData.NationalInsurance;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Paytools.ReferenceData;

internal class HmrcReferenceDataProvider : IHmrcReferenceDataProvider
{
    private ConcurrentDictionary<TaxYearEnding, HmrcTaxYearReferenceDataSet> _referenceDataSets;

    /// <summary>
    /// Gets or sets (internal only) the health of this reference data provider as human-readable string.
    /// </summary>
    public string Health { get; internal set; }

    /// <summary>
    /// Initialises a new intance of <see cref="IHmrcReferenceDataProvider"/>.
    /// </summary>
    internal HmrcReferenceDataProvider()
    {
        _referenceDataSets = new ConcurrentDictionary<TaxYearEnding, HmrcTaxYearReferenceDataSet>();
        Health = "No tax years added";
    }

    internal bool TryAdd(HmrcTaxYearReferenceDataSet referenceDataSet)
        => _referenceDataSets.TryAdd(referenceDataSet.ApplicableTaxYearEnding, referenceDataSet);

    /// <summary>
    /// Retrieves the tax bands for a given tax year in the form of a dictionary (<see cref="ReadOnlyDictionary{CountriesForTaxPurposes, TaxBandwidthSet}"/>)
    /// keyed on tax regime, i.e., <see cref="CountriesForTaxPurposes"/>.
    /// </summary>
    /// <param name="taxYear">Desired tax year.</param>
    /// <param name="payFrequency">Pay frequency pertaining.  Used in conjunction with the taxPeriod parameter to
    /// determine the applicable date (in case of in-year tax changes).</param>
    /// <param name="taxPeriod">Tax period, e.g., 1 for month 1.  Currently ignored on the assumption that in-year
    /// tax changes are not anticipated but provided for future .</param>
    /// <returns>ReadOnlyDictionary of <see cref="TaxBandwidthSet"/>'s keyed on tax regime.</returns>
    /// <remarks>Although ReadOnlyDictionary is not guaranteed to be thread-safe, in the current implementation the
    /// underlying Dictionary is guaranteed not to change, so thread-safety can be assumed.</remarks>
    public ReadOnlyDictionary<CountriesForTaxPurposes, TaxBandwidthSet> GetTaxBandsForTaxYearAndPeriod(
        TaxYear taxYear,
        PayFrequency payFrequency,
        int taxPeriod)
    {
        var referenceDataSet = GetReferenceDataSetForTaxYear(taxYear);

        var taxBands = FindApplicableEntry<IncomeTaxBandSet>(referenceDataSet.IncomeTax,
            taxYear, payFrequency, taxPeriod);

        return new ReadOnlyDictionary<CountriesForTaxPurposes, TaxBandwidthSet>(taxBands.TaxEntries
            .ToDictionary(e => e.ApplicableCountries, e => new TaxBandwidthSet(e.GetTaxBandwidthEntries())));
    }

    /// <summary>
    /// Gets a read-only dictionary that maps <see cref="NiCategory"/> values to the set of rates to be applied
    /// for a given tax year and tax period.
    /// </summary>
    /// <param name="taxYear">Applicable tax year.</param>
    /// <param name="payFrequency">Applicable pay frequency.</param>
    /// <param name="taxPeriod">Application tax period.</param>
    /// <returns>Read-only dictionary that maps <see cref="NiCategory"/> values to the appropriate set of rates for
    /// the specified point in time.</returns>
    public ReadOnlyDictionary<NiCategory, INiCategoryRatesEntry> GetNiRatesForTaxYearAndPeriod(TaxYear taxYear, PayFrequency payFrequency, int taxPeriod)
    {
        var referenceDataSet = GetReferenceDataSetForTaxYear(taxYear);

        var niReferenceDataEntry = FindApplicableEntry<NiReferenceDataEntry>(referenceDataSet.NationalInsurance,
            taxYear, payFrequency, taxPeriod);

        var rates = GetNiCategoryRatesEntries(niReferenceDataEntry.EmployerRates, niReferenceDataEntry.EmployeeRates);

        return new ReadOnlyDictionary<NiCategory, INiCategoryRatesEntry>(rates);
    }

    /// <summary>
    /// Gets a read-only dictionary that maps <see cref="NiCategory"/> values to the set of rates to be applied
    /// for a given tax year and tax period.
    /// </summary>
    /// <param name="taxYear">Applicable tax year.</param>
    /// <param name="payFrequency">Applicable pay frequency.</param>
    /// <param name="taxPeriod">Application tax period.</param>
    /// <returns>Read-only dictionary that maps <see cref="NiCategory"/> values to the appropriate set of rates for
    /// the specified point in time.</returns>
    public ReadOnlyDictionary<NiCategory, INiCategoryRatesEntry> GetDirectorsNiRatesForTaxYearAndPeriod(TaxYear taxYear, PayFrequency payFrequency, int taxPeriod)
    {
        var referenceDataSet = GetReferenceDataSetForTaxYear(taxYear);

        var niReferenceDataEntry = FindApplicableEntry<NiReferenceDataEntry>(referenceDataSet.NationalInsurance,
            taxYear, payFrequency, taxPeriod);

        var rates = GetNiCategoryRatesEntries(niReferenceDataEntry.EmployerRates, niReferenceDataEntry.EmployeeRates);

        return new ReadOnlyDictionary<NiCategory, INiCategoryRatesEntry>(rates);
    }

    /// <summary>
    /// Gets the NI thresholds for the specified tax year and tax period, as denoted by the supplied pay frequency
    /// and pay period.
    /// </summary>
    /// <param name="taxYear">Applicable tax year.</param>
    /// <param name="payFrequency">Applicable pay frequency.</param>
    /// <param name="taxPeriod">Application tax period.</param>
    /// <returns>An instance of <see cref="INiThresholdSet"/> containing the thresholds for the specified point
    /// in time.</returns>
    public INiThresholdSet GetNiThresholdsForTaxYearAndPeriod(TaxYear taxYear, PayFrequency payFrequency, int taxPeriod)
    {
        var referenceDataSet = GetReferenceDataSetForTaxYear(taxYear);

        var niReferenceDataEntry = FindApplicableEntry<NiReferenceDataEntry>(referenceDataSet.NationalInsurance,
            taxYear, payFrequency, taxPeriod);

        var thresholds = niReferenceDataEntry.NiThresholds.Select(nit => new NiThresholdEntry() 
            { 
                ThresholdType = nit.ThresholdType,
                ThresholdValuePerWeek= nit.ThresholdValuePerWeek,
                ThresholdValuePerMonth= nit.ThresholdValuePerMonth,
                ThresholdValuePerYear = nit.ThresholdValuePerYear
            })
            .ToImmutableList();

        return new NiThresholdSet(thresholds);
    }

    private HmrcTaxYearReferenceDataSet GetReferenceDataSetForTaxYear(TaxYear taxYear)
    {
        if (!_referenceDataSets.TryGetValue(taxYear.TaxYearEnding, out var referenceDataSet))
            throw new ArgumentOutOfRangeException(nameof(taxYear), $"No reference data found for tax year {taxYear}");

        if (referenceDataSet == null ||
            referenceDataSet?.IncomeTax == null ||
            referenceDataSet?.NationalInsurance == null)
            throw new InvalidReferenceDataException($"Reference data for tax year ending {taxYear.EndOfTaxYear} is invalid or incomplete");

        return referenceDataSet;
    }

    private static T FindApplicableEntry<T>(IReadOnlyList<IApplicableBetween> entries, TaxYear taxYear, PayFrequency payFrequency, int taxPeriod)
    {
        var endDateOfPeriod = taxYear.GetLastDayOfTaxPeriod(payFrequency, taxPeriod);

        var entry = entries
            .FirstOrDefault(ite => ite.ApplicableFrom <= endDateOfPeriod && endDateOfPeriod <= ite.ApplicableTill);

        if (entry == null)
            throw new InvalidReferenceDataException($"Unable to find reference data entry for specified tax year, pay frequency and pay period (Type: '{typeof(T)}')");

        return (T)entry;
    }

    private static Dictionary<NiCategory, INiCategoryRatesEntry> GetNiCategoryRatesEntries(
        ImmutableList<NiEmployerRatesEntry> employerRateEntries,
        ImmutableList<NiEmployeeRatesEntry> employeeRateEntries)
    {
        var rateEntries = new Dictionary<NiCategory, NiCategoryRatesEntry>();

        employerRateEntries.ForEach(erEntry =>
        {
            erEntry.NiCategories.ForEach(erCategory =>
            {
                if (!rateEntries.TryGetValue(erCategory, out NiCategoryRatesEntry? rateEntry))
                {
                    rateEntry = new NiCategoryRatesEntry(erCategory);
                    rateEntries.Add(erCategory, rateEntry);
                }

                rateEntry.EmployerRateLELtoST = erEntry.EarningsAtOrAboveLELUpToAndIncludingST;
                rateEntry.EmployerRateSTtoFUST = erEntry.EarningsAboveSTUpToAndIncludingFUST;
                rateEntry.EmployerRateFUSTtoUEL = erEntry.EarningsAboveFUSTUpToAndIncludingUELOrUST;
                rateEntry.EmployerRateAboveUEL = erEntry.EarningsAboveUELOrUST;
            });
        });

        employeeRateEntries.ForEach(eeEntry =>
        {
            eeEntry.NiCategories.ForEach(erCategory =>
            {
                if (!rateEntries.TryGetValue(erCategory, out NiCategoryRatesEntry? rateEntry))
                {
                    rateEntry = new NiCategoryRatesEntry(erCategory);
                    rateEntries.Add(erCategory, rateEntry);
                }

                rateEntry.EmployeeRateToPT = eeEntry.EarningsAtOrAboveLELUpTAndIncludingPT;
                rateEntry.EmployeeRatePTToUEL = eeEntry.EarningsAbovePTUpToAndIncludingUEL;
                rateEntry.EmployeeRateAboveUEL = eeEntry.EarningsAboveUEL;
            });
        });

        return rateEntries
            .Select(kv => new KeyValuePair<NiCategory, INiCategoryRatesEntry>(kv.Key, kv.Value))
            .ToDictionary(kv => kv.Key, kv => kv.Value);
    }
}