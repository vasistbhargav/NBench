﻿// Copyright (c) Petabridge <https://petabridge.com/>. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.

using System;

namespace NBench.Reporting.Targets
{
    /// <summary>
    /// An <see cref="IBenchmarkOutput"/> designed to run assertions against the data we collect on each run and in the final benchmark.
    /// </summary>
    public sealed class ActionBenchmarkOutput : IBenchmarkOutput
    {
        private readonly Action<BenchmarkRunReport> _runAction;
        private readonly Action<BenchmarkFinalResults> _benchmarkAction;

        public ActionBenchmarkOutput(Action<BenchmarkRunReport> runAction, Action<BenchmarkFinalResults> benchmarkAction)
        {
            _runAction = runAction;
            _benchmarkAction = benchmarkAction;
        }

        public void WriteStartingBenchmark(string benchmarkName)
        {
            //no-op
        }

        public void WriteRun(BenchmarkRunReport report, bool isWarmup = false)
        {
            if (!isWarmup)
            {
                _runAction(report);
            }
        }

        public void WriteBenchmark(BenchmarkFinalResults results)
        {
            _benchmarkAction(results);
        }
    }
}
