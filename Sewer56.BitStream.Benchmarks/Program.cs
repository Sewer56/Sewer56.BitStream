using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using System.Linq;

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
        AddColumn(new Speed());
    }
}

public class Speed : IColumn
{
    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        var ourReport = summary.Reports.First(x => x.BenchmarkCase.Equals(benchmarkCase));
        var mean = ourReport.ResultStatistics.Mean;
        var meanSeconds = mean / 1000_000_000F; // ns to seconds
        var sizeMb = BenchmarkBase.NumBytes / 1000.0 / 1000.0;

        // Convert to MB/s.
        return $"{(sizeMb / meanSeconds):#####.00}";
    }

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style) => GetValue(summary, benchmarkCase);
    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;
    public bool IsAvailable(Summary summary) => true;

    public string Id { get; } = nameof(Speed);
    public string ColumnName { get; } = "Speed (MB/s)";
    public bool AlwaysShow { get; } = true;
    public ColumnCategory Category { get; } = ColumnCategory.Custom;
    public int PriorityInCategory { get; } = 0;
    public bool IsNumeric { get; } = false;
    public UnitType UnitType { get; } = UnitType.Dimensionless;
    public string Legend { get; } = "The speed of pattern checking in megabytes per second";
}