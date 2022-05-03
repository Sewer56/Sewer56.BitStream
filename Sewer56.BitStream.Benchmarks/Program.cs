using System;
using BenchmarkDotNet.Running;

namespace Sewer56.BitStream.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ReadBenchmark>();
            //BenchmarkRunner.Run<WriteBenchmark>();
        }
    }
}
