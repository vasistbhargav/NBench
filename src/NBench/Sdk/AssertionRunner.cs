﻿// Copyright (c) Petabridge <https://petabridge.com/>. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using NBench.Reporting;

namespace NBench.Sdk
{
    /// <summary>
    ///     Responsible for running <see cref="Assertion" />s over <see cref="BenchmarkResults" />.
    /// </summary>
    public static class AssertionRunner
    {
        public static IReadOnlyList<AssertionResult> RunAssertions(BenchmarkSettings settings, BenchmarkResults results)
        {
            var assertionResults = new List<AssertionResult>();

            // collect all benchmark settings with non-empty assertions
            IList<IBenchmarkSetting> allSettings =
                settings.CounterBenchmarks.Concat<IBenchmarkSetting>(settings.GcBenchmarks)
                    .Concat(settings.MemoryBenchmarks).Where(x => !x.Assertion.Equals(Assertion.Empty))
                    .ToList();

            foreach (var setting in allSettings)
            {
                var stats = results.StatsByMetric[setting.MetricName];
                double valueToBeTested;
                if (setting.AssertionType == AssertionType.Throughput)
                {
                    valueToBeTested = stats.PerSecondMaxes.Mean;
                }
                else
                {
                    valueToBeTested = stats.Maxes.Mean;
                }
                var assertionResult = AssertionResult.CreateResult(setting.MetricName, stats.Unit, valueToBeTested,
                    setting.Assertion);
                assertionResults.Add(assertionResult);
            }

            return assertionResults;
        }
    }
}
