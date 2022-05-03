using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Sewer56.BitStream.ByteStreams;

namespace Sewer56.BitStream.Benchmarks
{
    [SimpleJob(RuntimeMoniker.CoreRt31)]
    [SimpleJob(RuntimeMoniker.CoreRt50)]
    public class ReadBenchmark
    {
        private const int NumBytes = 10000;
        private readonly byte[] _data;

        public ReadBenchmark()
        {
            _data = new byte[NumBytes];
            new Random().NextBytes(_data);
        }

        [Benchmark]
        public int ReadBit()
        {
            var maxNumIterations = NumBytes * 8;
            var numIterations    = 0;
            var stream           = new ArrayByteStream(_data); 
            var bitStream        = new BitStream<ArrayByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.ReadBit();
            }

            return numIterations;
        }

        [Benchmark]
        public int Read8()
        {
            var maxNumIterations = NumBytes;
            var numIterations = 0;
            var stream = new ArrayByteStream(_data);
            var bitStream = new BitStream<ArrayByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.Read8(8);
            }

            return numIterations;
        }

        [Benchmark]
        public int Read16()
        {
            var maxNumIterations = NumBytes / 2;
            var numIterations = 0;
            var stream = new ArrayByteStream(_data);
            var bitStream = new BitStream<ArrayByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.Read16(16);
            }

            return numIterations;
        }

        [Benchmark]
        public int Read32()
        {
            var maxNumIterations = NumBytes / 4;
            var numIterations = 0;
            var stream = new ArrayByteStream(_data);
            var bitStream = new BitStream<ArrayByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.Read32(32);
            }

            return numIterations;
        }

        [Benchmark]
        public int Read64()
        {
            var maxNumIterations = NumBytes / 8;
            var numIterations = 0;
            var stream = new ArrayByteStream(_data);
            var bitStream = new BitStream<ArrayByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.Read64(64);
            }
            
            return numIterations;
        }

        [Benchmark]
        public int Read8Generic()
        {
            var maxNumIterations = NumBytes;
            var numIterations = 0;
            var stream = new ArrayByteStream(_data);
            var bitStream = new BitStream<ArrayByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.Read<byte>();
            }

            return numIterations;
        }
    }
}
