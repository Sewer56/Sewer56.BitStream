using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Sewer56.BitStream.ByteStreams;

namespace Sewer56.BitStream.Benchmarks
{
    public class ReadBenchmarkBase
    {
        protected const int NumBytes = 10000;
        protected readonly byte[] _data;

        public ReadBenchmarkBase()
        {
            _data = new byte[NumBytes];
            new Random().NextBytes(_data);
        }
    }
}
