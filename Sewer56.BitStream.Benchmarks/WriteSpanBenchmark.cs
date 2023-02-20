using System;
using BenchmarkDotNet.Attributes;
using Sewer56.BitStream.ByteStreams;
using Sewer56.BitStream.Misc;

namespace Sewer56.BitStream.Benchmarks;

[DisassemblyDiagnoser]
public class WriteSpanBenchmark : BenchmarkBase
{
    private const int UnrollFactor = 4;
    private const int MaxTestedChunkSize = 1024 * UnrollFactor;
    private readonly byte[] _output = new byte[NumBytes + MaxTestedChunkSize];
    
    [Benchmark]
    public unsafe void WriteSpan_Aligned()
    {
        fixed (byte* bytePtr = _output)
        {
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream, 0);
            bitStream.WriteFast(_data);
        }
    }
    
    [Benchmark]
    public unsafe void WriteSpan_Aligned_Chunk_4()
    {
        fixed (byte* bytePtr = _output)
        {
            var dataSpan = _data.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream, 0);
            
            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 16)
            {
                bitStream.WriteFast(dataSpan.SliceFast(x, 4));
                bitStream.WriteFast(dataSpan.SliceFast(x + 4, 4));
                bitStream.WriteFast(dataSpan.SliceFast(x + 8, 4));
                bitStream.WriteFast(dataSpan.SliceFast(x + 12, 4));
            }
        }
    }
    
    [Benchmark]
    public unsafe void WriteSpan_Aligned_Chunk_8()
    {
        fixed (byte* bytePtr = _output)
        {
            var dataSpan = _data.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream, 0);
            
            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 32)
            {
                bitStream.WriteFast(dataSpan.SliceFast(x, 8));
                bitStream.WriteFast(dataSpan.SliceFast(x + 8, 8));
                bitStream.WriteFast(dataSpan.SliceFast(x + 16, 8));
                bitStream.WriteFast(dataSpan.SliceFast(x + 24, 8));
            }
        }
    }
    
    [Benchmark]
    public unsafe void WriteSpan_Aligned_Chunk_16()
    {
        fixed (byte* bytePtr = _output)
        {
            var dataSpan = _data.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream, 0);
            
            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 64)
            {
                bitStream.WriteFast(dataSpan.SliceFast(x, 16));
                bitStream.WriteFast(dataSpan.SliceFast(x + 16, 16));
                bitStream.WriteFast(dataSpan.SliceFast(x + 32, 16));
                bitStream.WriteFast(dataSpan.SliceFast(x + 48, 16));
            }
        }
    }
    
    [Benchmark]
    public unsafe void WriteSpan_Aligned_Chunk_32()
    {
        fixed (byte* bytePtr = _output)
        {
            var dataSpan = _data.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream, 0);
            
            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 128)
            {
                bitStream.WriteFast(dataSpan.SliceFast(x, 32));
                bitStream.WriteFast(dataSpan.SliceFast(x + 32, 32));
                bitStream.WriteFast(dataSpan.SliceFast(x + 64, 32));
                bitStream.WriteFast(dataSpan.SliceFast(x + 96, 32));
            }
        }
    }
    
    [Benchmark]
    public unsafe void WriteSpan_Aligned_Chunk_64()
    {
        fixed (byte* bytePtr = _output)
        {
            var dataSpan = _data.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream, 0);
            
            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 256)
            {
                bitStream.WriteFast(dataSpan.SliceFast(x, 64));
                bitStream.WriteFast(dataSpan.SliceFast(x + 64, 64));
                bitStream.WriteFast(dataSpan.SliceFast(x + 128, 64));
                bitStream.WriteFast(dataSpan.SliceFast(x + 192, 64));
            }
        }
    }
    
    [Benchmark]
    public unsafe void WriteSpan_Aligned_Chunk_128()
    {
        fixed (byte* bytePtr = _output)
        {
            var dataSpan = _data.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream, 0);
            
            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 512)
            {
                bitStream.WriteFast(dataSpan.SliceFast(x, 128));
                bitStream.WriteFast(dataSpan.SliceFast(x + 128, 128));
                bitStream.WriteFast(dataSpan.SliceFast(x + 256, 128));
                bitStream.WriteFast(dataSpan.SliceFast(x + 384, 128));
            }
        }
    }
    
    [Benchmark]
    public unsafe void WriteSpan_Aligned_Chunk_256()
    {
        fixed (byte* bytePtr = _output)
        {
            var dataSpan = _data.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream, 0);
            
            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 1024)
            {
                bitStream.WriteFast(dataSpan.SliceFast(x, 256));
                bitStream.WriteFast(dataSpan.SliceFast(x + 256, 256));
                bitStream.WriteFast(dataSpan.SliceFast(x + 512, 256));
                bitStream.WriteFast(dataSpan.SliceFast(x + 768, 256));
            }
        }
    }
    
    [Benchmark]
    public unsafe void WriteSpan_Aligned_Chunk_512()
    {
        fixed (byte* bytePtr = _output)
        {
            var dataSpan = _data.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream, 0);
            
            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 2048)
            {
                bitStream.WriteFast(dataSpan.SliceFast(x, 512));
                bitStream.WriteFast(dataSpan.SliceFast(x + 512, 512));
                bitStream.WriteFast(dataSpan.SliceFast(x + 1024, 512));
                bitStream.WriteFast(dataSpan.SliceFast(x + 1536, 512));
            }
        }
    }
    
    [Benchmark]
    public unsafe void WriteSpan_Aligned_Chunk_1024()
    {
        fixed (byte* bytePtr = _output)
        {
            var dataSpan = _data.AsSpan();
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream, 0);
            
            // Unroll a bit for accuracy.
            for (int x = 0; x < _data.Length; x += 4096)
            {
                bitStream.WriteFast(dataSpan.SliceFast(x, 1024));
                bitStream.WriteFast(dataSpan.SliceFast(x + 1024, 1024));
                bitStream.WriteFast(dataSpan.SliceFast(x + 2048, 1024));
                bitStream.WriteFast(dataSpan.SliceFast(x + 3072, 1024));
            }
        }
    }
    
    [Benchmark]
    public unsafe void WriteSpan_Unaligned()
    {
        fixed (byte* bytePtr = _output)
        {
            var stream = new PointerByteStream(bytePtr);
            var bitStream = new BitStream<PointerByteStream>(stream, 0);
            bitStream.WriteBit(1);
            bitStream.WriteFast(_data.AsSpan(0, _data.Length - 1));
        }
    }
}