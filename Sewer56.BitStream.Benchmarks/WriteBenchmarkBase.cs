using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Sewer56.BitStream.Benchmarks
{
    public class WriteBenchmarkBase
    {
        protected const int NumBytes = 10000;
        protected readonly byte[] _data;

        public WriteBenchmarkBase()
        {
            _data = new byte[NumBytes];
        }
    }
}
