using System;

namespace Sewer56.BitStream.Benchmarks
{
    public class BenchmarkBase
    {
        public const int NumBytes = 10000;
        protected readonly byte[] _data;

        public BenchmarkBase()
        {
            _data = new byte[NumBytes];
            new Random().NextBytes(_data);
        }
    }
}
