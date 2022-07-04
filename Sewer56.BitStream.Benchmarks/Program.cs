using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace Sewer56.BitStream.Benchmarks;

internal class Program
{
    static void Main(string[] args)
    {
        var config = new InProcessConfig();
        BenchmarkRunner.Run<ReadBenchmark>(config);
        BenchmarkRunner.Run<ReadBenchmark_Streams>(config);
        BenchmarkRunner.Run<WriteBenchmark>(config);
        BenchmarkRunner.Run<WriteBenchmark_Streams>(config);
    }
}

internal class InProcessConfig : ManualConfig
{
    public InProcessConfig()
    {
        Add(DefaultConfig.Instance);
        AddJob(Job.MediumRun.WithLaunchCount(1).WithToolchain(new InProcessEmitToolchain(true)));
    }
}