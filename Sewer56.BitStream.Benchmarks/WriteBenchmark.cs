using BenchmarkDotNet.Attributes;
using Sewer56.BitStream.ByteStreams;

namespace Sewer56.BitStream.Benchmarks
{
    [MemoryDiagnoser]
    public class WriteBenchmark : BenchmarkBase
    {
        [Benchmark]
        public int WriteBit()
        {
            var maxNumIterations = NumBytes * 8;
            var numIterations    = 0;
            var stream           = new ArrayByteStream(_data); 
            var bitStream        = new BitStream<ArrayByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.WriteBit(1);
            }

            return numIterations;
        }

        [Benchmark]
        public int Write8()
        {
            var maxNumIterations = NumBytes;
            var numIterations = 0;
            var stream = new ArrayByteStream(_data);
            var bitStream = new BitStream<ArrayByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.Write8((byte) numIterations, 8);
            }

            return numIterations;
        }

        [Benchmark]
        public int Write16()
        {
            var maxNumIterations = NumBytes / 2;
            var numIterations = 0;
            var stream = new ArrayByteStream(_data);
            var bitStream = new BitStream<ArrayByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.Write16((ushort)numIterations, 16);
            }

            return numIterations;
        }

        [Benchmark]
        public int Write32()
        {
            var maxNumIterations = NumBytes / 4;
            var numIterations = 0;
            var stream = new ArrayByteStream(_data);
            var bitStream = new BitStream<ArrayByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.Write32((uint)numIterations, 32);
            }

            return numIterations;
        }

        [Benchmark]
        public int Write64()
        {
            var maxNumIterations = NumBytes / 8;
            var numIterations = 0;
            var stream = new ArrayByteStream(_data);
            var bitStream = new BitStream<ArrayByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.Write64((ulong)numIterations, 32);
            }
            
            return numIterations;
        }

        [Benchmark]
        public int Write8Generic()
        {
            var maxNumIterations = NumBytes;
            var numIterations = 0;
            var stream = new ArrayByteStream(_data);
            var bitStream = new BitStream<ArrayByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.Write((byte) numIterations);
            }

            return numIterations;
        }
    }
}
