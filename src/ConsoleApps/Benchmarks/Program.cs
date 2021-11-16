// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using BenchmarkDotNet.Running;
using Benchmarks;

var summary = BenchmarkRunner.Run<EdgeDistanceBenchmarks>();
