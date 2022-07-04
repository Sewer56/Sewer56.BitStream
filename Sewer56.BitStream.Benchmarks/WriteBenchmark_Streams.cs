using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using Sewer56.BitStream.ByteStreams;

namespace Sewer56.BitStream.Benchmarks
{
    [MemoryDiagnoser]
    public class WriteBenchmark_Streams : WriteBenchmarkBase
    {
        [Benchmark]
        public int Write8_ArrayByteStream()
        {
            var maxNumIterations = NumBytes;
            var numIterations = 0;
            var stream = new ArrayByteStream(_data);
            var bitStream = new BitStream<ArrayByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.Write8((byte)numIterations, 8);
            }

            return numIterations;
        }

        [Benchmark]
        public unsafe int Write8_PointerByteStream()
        {
            var maxNumIterations = NumBytes;
            var numIterations = 0;
            fixed (byte* arrayPtr = _data)
            {
                var stream = new PointerByteStream(arrayPtr);
                var bitStream = new BitStream<PointerByteStream>(stream, 0);

                for (; numIterations < maxNumIterations; numIterations++)
                {
                    bitStream.Write8((byte)numIterations, 8);
                }

                return numIterations;
            }
        }

        [Benchmark]
        public int Write8_StreamByteStream()
        {
            var maxNumIterations = NumBytes;
            var numIterations = 0;
            var stream = new StreamByteStream(new MemoryStream(_data));
            var bitStream = new BitStream<StreamByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.Write8((byte)numIterations, 8);
            }

            return numIterations;
        }

        [Benchmark]
        public int Write8_MemoryByteStream()
        {
            var maxNumIterations = NumBytes;
            var numIterations = 0;
            var stream = new MemoryByteStream(_data.AsMemory());
            var bitStream = new BitStream<MemoryByteStream>(stream, 0);

            for (; numIterations < maxNumIterations; numIterations++)
            {
                bitStream.Write8((byte)numIterations, 8);
            }

            return numIterations;
        }
    }
}
