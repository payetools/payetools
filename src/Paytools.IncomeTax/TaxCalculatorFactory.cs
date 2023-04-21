﻿// Copyright (c) 2023 Paytools Foundation.
//
// Paytools Foundation licenses this file to you under one of the following licenses:
//
//  * GNU Affero General Public License, see https://www.gnu.org/licenses/agpl-3.0.html
//  * Paytools Commercial Use license [TBA]
//
// For further information on licensing options, see https://paytools.dev/licensing-paytools.html

using Paytools.Common.Diagnostics;
using Paytools.Common.Model;
using Paytools.IncomeTax.ReferenceData;

namespace Paytools.IncomeTax;

/// <summary>
/// Factory to generate <see cref="ITaxCalculator"/> implementations that are for a given pay date and specific tax regime.
/// </summary>
public class TaxCalculatorFactory : ITaxCalculatorFactory
{
    private readonly ITaxReferenceDataProvider _taxBandProvider;

    /// <summary>
    /// Initialises a new instance of <see cref="TaxCalculatorFactory"/> using the supplied tax band provider.
    /// </summary>
    /// <param name="taxBandProvider">Tax band provider for providing access to tax bands for given tax years.</param>
    public TaxCalculatorFactory(ITaxReferenceDataProvider taxBandProvider)
    {
        _taxBandProvider = taxBandProvider;
    }

    /// <summary>
    /// Gets an instance of an <see cref="ITaxCalculator"/> for the specified tax regime and pay date.
    /// </summary>
    /// <param name="applicableCountries">Applicable tax regime.</param>
    /// <param name="payDate">Pay date.</param>
    /// <returns>Instance of <see cref="ITaxCalculator"/> for the specified tax regime and pay date.</returns>
    /// <exception cref="InvalidReferenceDataException">Thrown if it was not possible to find a tax bandwidth set for the specified
    /// tax regime and tax year combination.</exception>
    public ITaxCalculator GetCalculator(CountriesForTaxPurposes applicableCountries, PayDate payDate) =>
        GetCalculator(applicableCountries, payDate.TaxYear, payDate.PayFrequency, payDate.TaxPeriod);

    /// <summary>
    /// Gets an instance of an <see cref="ITaxCalculator"/> for the specified tax regime, tax year, tax period and pay frequency.
    /// </summary>
    /// <param name="applicableCountries">Applicable tax regime.</param>
    /// <param name="taxYear">Relevant tax year.</param>
    /// <param name="payFrequency">Applicable pay frequency.</param>
    /// <param name="taxPeriod">Applicable tax period.</param>
    /// <returns>Instance of <see cref="ITaxCalculator"/> for the specified tax regime, tax year and period and pay frequency.</returns>
    /// <exception cref="InvalidReferenceDataException">Thrown if it was not possible to find a tax bandwidth set for the specified
    /// tax regime and tax year combination.</exception>
    public ITaxCalculator GetCalculator(CountriesForTaxPurposes applicableCountries, TaxYear taxYear, PayFrequency payFrequency, int taxPeriod)
    {
        var taxBandwidthSets = _taxBandProvider.GetTaxBandsForTaxYearAndPeriod(taxYear, payFrequency, taxPeriod);

        if (!taxBandwidthSets.TryGetValue(applicableCountries, out var taxBandwidthSet))
            throw new InvalidReferenceDataException($"Unable to find unique tax bands for countries/tax year combination [{applicableCountries}] {taxYear}");

        return new TaxCalculator(taxYear, applicableCountries, taxBandwidthSet, payFrequency, taxPeriod);
    }
}