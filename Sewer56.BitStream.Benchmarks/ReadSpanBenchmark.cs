using System;
using BenchmarkDotNet.Attributes;
using Sewer56.BitStream.ByteStreams;
using Sewer56.BitStream.Misc;

namespace Sewer56.BitStream.Benchmarks;

[DisassemblyDiagnoser]
public class ReadSpanBenchmark : BenchmarkBase
{
    private const int UnrollFactor = 4;
    private const int MaxTestedChunkSize = 1024 * UnrollFactor;
    private readonly byte[] _output = new byte[NumBytes + MaxTestedChunkSize];
    
    [Benchmark]
    public unsafe void ReadSpan_Aligned_Chunk_10000()
    {
        fixed (byte* bytePtr = _data)
        {
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream);
            bitStream.ReadFast(_output.AsSpan()[..(_data.Length - 1)]);
        }
    }
    
    [Benchmark]
    public unsafe void ReadSpan_Aligned_Chunk_4()
    {
        fixed (byte* bytePtr = _data)
        {
            var outputSpan = _output.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream);

            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 16)
            {
                bitStream.ReadFast(outputSpan.SliceFast(x, 4));
                bitStream.ReadFast(outputSpan.SliceFast(x + 4, 4));
                bitStream.ReadFast(outputSpan.SliceFast(x + 8, 4));
                bitStream.ReadFast(outputSpan.SliceFast(x + 12, 4));
            }
        }
    }
    
    [Benchmark]
    public unsafe void ReadSpan_Aligned_Chunk_8()
    {
        fixed (byte* bytePtr = _data)
        {
            var outputSpan = _output.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream);

            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 32)
            {
                bitStream.ReadFast(outputSpan.SliceFast(x, 8));
                bitStream.ReadFast(outputSpan.SliceFast(x + 8, 8));
                bitStream.ReadFast(outputSpan.SliceFast(x + 16, 8));
                bitStream.ReadFast(outputSpan.SliceFast(x + 24, 8));
            }
        }
    }

    [Benchmark]
    public unsafe void ReadSpan_Aligned_Chunk_16()
    {
        fixed (byte* bytePtr = _data)
        {
            var outputSpan = _output.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream);

            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 64)
            {
                bitStream.ReadFast(outputSpan.SliceFast(x, 16));
                bitStream.ReadFast(outputSpan.SliceFast(x + 16, 16));
                bitStream.ReadFast(outputSpan.SliceFast(x + 32, 16));
                bitStream.ReadFast(outputSpan.SliceFast(x + 48, 16));
            }
        }
    }
    
    [Benchmark]
    public unsafe void ReadSpan_Aligned_Chunk_32()
    {
        fixed (byte* bytePtr = _data)
        {
            var outputSpan = _output.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream);

            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 128)
            {
                bitStream.ReadFast(outputSpan.SliceFast(x, 32));
                bitStream.ReadFast(outputSpan.SliceFast(x + 32, 32));
                bitStream.ReadFast(outputSpan.SliceFast(x + 64, 32));
                bitStream.ReadFast(outputSpan.SliceFast(x + 96, 32));
            }
        }
    }
    
    [Benchmark]
    public unsafe void ReadSpan_Aligned_Chunk_64()
    {
        fixed (byte* bytePtr = _data)
        {
            var outputSpan = _output.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream);

            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 256)
            {
                bitStream.ReadFast(outputSpan.SliceFast(x, 64));
                bitStream.ReadFast(outputSpan.SliceFast(x + 64, 64));
                bitStream.ReadFast(outputSpan.SliceFast(x + 128, 64));
                bitStream.ReadFast(outputSpan.SliceFast(x + 196, 64));
            }
        }
    }
    
    [Benchmark]
    public unsafe void ReadSpan_Aligned_Chunk_128()
    {
        fixed (byte* bytePtr = _data)
        {
            var outputSpan = _output.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream);

            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 512)
            {
                bitStream.ReadFast(outputSpan.SliceFast(x, 128));
                bitStream.ReadFast(outputSpan.SliceFast(x + 128, 128));
                bitStream.ReadFast(outputSpan.SliceFast(x + 256, 128));
                bitStream.ReadFast(outputSpan.SliceFast(x + 384, 128));
            }
        }
    }
    
    [Benchmark]
    public unsafe void ReadSpan_Aligned_Chunk_256()
    {
        fixed (byte* bytePtr = _data)
        {
            var outputSpan = _output.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream);

            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 1024)
            {
                bitStream.ReadFast(outputSpan.SliceFast(x, 256));
                bitStream.ReadFast(outputSpan.SliceFast(x + 256, 256));
                bitStream.ReadFast(outputSpan.SliceFast(x + 512, 256));
                bitStream.ReadFast(outputSpan.SliceFast(x + 768, 256));
            }
        }
    }
    
    [Benchmark]
    public unsafe void ReadSpan_Aligned_Chunk_512()
    {
        fixed (byte* bytePtr = _data)
        {
            var outputSpan = _output.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream);

            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 2048)
            {
                bitStream.ReadFast(outputSpan.SliceFast(x, 512));
                bitStream.ReadFast(outputSpan.SliceFast(x + 512, 512));
                bitStream.ReadFast(outputSpan.SliceFast(x + 1024, 512));
                bitStream.ReadFast(outputSpan.SliceFast(x + 1536, 512));
            }
        }
    }
    
    [Benchmark]
    public unsafe void ReadSpan_Aligned_Chunk_1024()
    {
        fixed (byte* bytePtr = _data)
        {
            var outputSpan = _output.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream);

            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 4096)
            {
                bitStream.ReadFast(outputSpan.SliceFast(x, 1024));
                bitStream.ReadFast(outputSpan.SliceFast(x + 1024, 1024));
                bitStream.ReadFast(outputSpan.SliceFast(x + 2048, 1024));
                bitStream.ReadFast(outputSpan.SliceFast(x + 3072, 1024));
            }
        }
    }
    
    [Benchmark]
    public unsafe void ReadSpan_Unaligned_Chunk_10000()
    {
        fixed (byte* bytePtr = _data)
        {
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream);
            bitStream.WriteBit(1);
            bitStream.ReadFast(_output.AsSpan(0, _data.Length - 1));
        }
    }
}