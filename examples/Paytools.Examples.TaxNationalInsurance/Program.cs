﻿//
// ##################### PAYTOOLS EXAMPLE SOURCE CODE ##########################
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this example code (the "Example"), to deal in the Example without restriction,
// including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Example, and to permit persons
// to whom the Example is furnished to do so without constraint.

// THE EXAMPLE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE EXAMPLE OR THE USE OR OTHER DEALINGS IN THE EXAMPLE.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Paytools.ReferenceData;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Paytools.Payroll.Payruns;
using Paytools.Common.Model;
using Paytools.Employment.Model;
using Paytools.Payroll.Model;
using Paytools.IncomeTax;
using Paytools.NationalInsurance;
using System.Security.AccessControl;

Console.WriteLine("Hello, World!");

AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();

var serviceProvider = new ServiceCollection()
    .AddHttpClient()
    .AddLogging(builder => builder.AddConsole())
    .BuildServiceProvider();

var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>() ??
    throw new InvalidOperationException("Unable to create HttpClientfactory");

var loggerFactory = serviceProvider.GetService<ILoggerFactory>() ??
    throw new InvalidOperationException("Unable to create ILoggerFactory");

var factory = new HmrcReferenceDataProviderFactory(httpClientFactory, 
    loggerFactory.CreateLogger<HmrcReferenceDataProviderFactory>());

IPayrunProcessorFactory payrunProcessorFactory = new PayrunProcessorFactory(factory, new Uri("https://stellular-bombolone-34e67e.netlify.app/index.json"));

PayDate payDate = new PayDate(2022, 8, 20, PayFrequency.Monthly);
PayReferencePeriod payPeriod = new PayReferencePeriod(new DateOnly(), new DateOnly());

IEmployer employer = new Employer();

var processor = await payrunProcessorFactory.GetProcessorAsync(employer, payDate, payPeriod);

List<IEmployeePayrunInputEntry> entries = new List<IEmployeePayrunInputEntry>();

var employee = new Employee()
{ };

IEmployeePayrollHistoryYtd history = new EmployeePayrollHistoryYtd() { TaxablePayYtd = 28333.32m -1841.69m + 450.12m, TaxPaidYtd = 6533.86m };

TaxCode.TryParse("1296L", out var taxCode);

var employment = new Employment(ref history) { TaxCode = taxCode, NiCategory = NiCategory.A};

var entry = new EmployeePayrunInputEntry(employee,
    employment,
    new List<EarningsEntry>() { 
        new EarningsEntry() { EarningsType = new GenericPayComponent() { IsSubjectToTax = true,  IsSubjectToNi = true,  IsPensionable = true, IsNetToGross = false         }, 
            FixedAmount = 7083.33m - 495.64m + 150.05m}
    },
    new List<DeductionEntry>() {
    },
    new List<IPayrolledBenefitForPeriod>() { },
    new PensionContributionLevels() {  });


entries.Add(entry);
processor.Process(entries, out var result);


Console.WriteLine();



static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
{
    // Log the exception, display it, etc
    Console.WriteLine((e.ExceptionObject as Exception).Message);
}