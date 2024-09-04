using Asp.Versioning;
using BenchmarkDotNet;
using BenchmarkDotNet.Running;
using ConsoleBenchmarks.BoxesApiCalls;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

BenchmarkDotNet.Configs.IConfig cfg = new BenchmarkDotNet.Configs.ManualConfig();

Thread.Sleep(20000);

BenchmarkRunner.Run<CreationBenchmark>();


