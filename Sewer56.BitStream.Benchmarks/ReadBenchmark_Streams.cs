using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using Sewer56.BitStream.ByteStreams;

namespace Sewer56.BitStream.Benchmarks
{
    public class ReadBenchmark_Streams : ReadBenchmarkBase
    {
        [Benchmark]
        public int Read8_ArrayByteStream()
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
        public unsafe int Read8_PointerByteStream()
        {
            var maxNumIterations = NumBytes;
            var numIterations = 0;
            fixed (byte* arrayPtr = _data)
            {
                var stream = new PointerByteStream(arrayPtr);
                var bitStream = new BitStream<PointerByteStream>(stream, 0);

                for (; numIterations < maxNumIterations; numIterations++)
                {
                    bitStream.Read8(8);
                }

                return numIterations;
            }
        }

        [Benchmark]
        public int Read8_StreamByteStream()
        {
            var maxNumIterations = NumBytes;
            var numIterations = 0;
            var stream = new StreamByteStream(new MemoryStream(_data));
            var bitStream = new BitStream<StreamByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.Read8(8);
            }

            return numIterations;
        }

        [Benchmark]
        public int Read8_MemoryByteStream()
        {
            var maxNumIterations = NumBytes;
            var numIterations = 0;
            var stream = new MemoryByteStream(_data.AsMemory());
            var bitStream = new BitStream<MemoryByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.Read8(8);
            }

            return numIterations;
        }
    }
}
