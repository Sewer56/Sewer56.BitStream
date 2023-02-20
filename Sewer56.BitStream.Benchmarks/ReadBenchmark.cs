using BenchmarkDotNet.Attributes;
using Sewer56.BitStream.ByteStreams;

namespace Sewer56.BitStream.Benchmarks;

[DisassemblyDiagnoser]
[MemoryDiagnoser]
public class ReadBenchmark : BenchmarkBase
{
    [Benchmark]
    public nuint ReadBit()
    {
        var maxNumIterations = (NumBytes * 8) / 8;
        var stream           = new ArrayByteStream(_data); 
        var bitStream        = new BitStream<ArrayByteStream>(stream, 0);
        nuint result = 0;
        
        for (var numIterations = 0; numIterations < maxNumIterations; numIterations++)
        {
            result += bitStream.ReadBit();
            result += bitStream.ReadBit();
            result += bitStream.ReadBit();
            result += bitStream.ReadBit();
            result += bitStream.ReadBit();
            result += bitStream.ReadBit();
            result += bitStream.ReadBit();
            result += bitStream.ReadBit();
        }

        return result;
    }

    [Benchmark]
    public nuint Read8()
    {
        var maxNumIterations = NumBytes / 8;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);
        nuint result = 0;

        for (var numIterations = 0; numIterations < maxNumIterations; numIterations++)
        {
            result += bitStream.Read8(8);
            result += bitStream.Read8(8);
            result += bitStream.Read8(8);
            result += bitStream.Read8(8);
            result += bitStream.Read8(8);
            result += bitStream.Read8(8);
            result += bitStream.Read8(8);
            result += bitStream.Read8(8);
        }

        return result;
    }

    [Benchmark]
    public nuint Read8Aligned()
    {
        var maxNumIterations = NumBytes / 8;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);
        nuint result = 0;

        for (var numIterations = 0; numIterations < maxNumIterations; numIterations++)
        {
            result += bitStream.Read8Aligned();
            result += bitStream.Read8Aligned();
            result += bitStream.Read8Aligned();
            result += bitStream.Read8Aligned();
            result += bitStream.Read8Aligned();
            result += bitStream.Read8Aligned();
            result += bitStream.Read8Aligned();
            result += bitStream.Read8Aligned();
        }

        return result;
    }
    
    [Benchmark]
    public nuint Read16()
    {
        var maxNumIterations = NumBytes / 2 / 8;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);
        nuint result = 0;

        for (var numIterations = 0; numIterations < maxNumIterations; numIterations++)
        {
            result += bitStream.Read16(16);
            result += bitStream.Read16(16);
            result += bitStream.Read16(16);
            result += bitStream.Read16(16);
            result += bitStream.Read16(16);
            result += bitStream.Read16(16);
            result += bitStream.Read16(16);
            result += bitStream.Read16(16);
        }

        return result;
    }
    
    [Benchmark]
    public nuint Read16Aligned()
    {
        var maxNumIterations = NumBytes / 2 / 8;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);
        nuint result = 0;

        for (var numIterations = 0; numIterations < maxNumIterations; numIterations++)
        {
            result += bitStream.Read16Aligned();
            result += bitStream.Read16Aligned();
            result += bitStream.Read16Aligned();
            result += bitStream.Read16Aligned();
            result += bitStream.Read16Aligned();
            result += bitStream.Read16Aligned();
            result += bitStream.Read16Aligned();
            result += bitStream.Read16Aligned();
        }

        return result;
    }
    
    [Benchmark]
    public nuint Read16AlignedFast()
    {
        var maxNumIterations = NumBytes / 2 / 8;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);
        nuint result = 0;

        for (var numIterations = 0; numIterations < maxNumIterations; numIterations++)
        {
            result += bitStream.Read16AlignedFast();
            result += bitStream.Read16AlignedFast();
            result += bitStream.Read16AlignedFast();
            result += bitStream.Read16AlignedFast();
            result += bitStream.Read16AlignedFast();
            result += bitStream.Read16AlignedFast();
            result += bitStream.Read16AlignedFast();
            result += bitStream.Read16AlignedFast();
        }

        return result;
    }
    
    [Benchmark]
    public nuint Read32()
    {
        var maxNumIterations = NumBytes / 4 / 8;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);
        nuint result = 0;

        for (var numIterations = 0; numIterations < maxNumIterations; numIterations++)
        {
            result += bitStream.Read32(32);
            result += bitStream.Read32(32);
            result += bitStream.Read32(32);
            result += bitStream.Read32(32);
            result += bitStream.Read32(32);
            result += bitStream.Read32(32);
            result += bitStream.Read32(32);
            result += bitStream.Read32(32);
        }

        return result;
    }
    
    [Benchmark]
    public nuint Read32Aligned()
    {
        var maxNumIterations = NumBytes / 4 / 8;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);
        nuint result = 0;

        for (var numIterations = 0; numIterations < maxNumIterations; numIterations++)
        {
            result += bitStream.Read32Aligned();
            result += bitStream.Read32Aligned();
            result += bitStream.Read32Aligned();
            result += bitStream.Read32Aligned();
            result += bitStream.Read32Aligned();
            result += bitStream.Read32Aligned();
            result += bitStream.Read32Aligned();
            result += bitStream.Read32Aligned();
        }

        return result;
    }
    
    [Benchmark]
    public nuint Read32AlignedFast()
    {
        var maxNumIterations = NumBytes / 4 / 8;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);
        nuint result = 0;

        for (var numIterations = 0; numIterations < maxNumIterations; numIterations++)
        {
            result += bitStream.Read32AlignedFast();
            result += bitStream.Read32AlignedFast();
            result += bitStream.Read32AlignedFast();
            result += bitStream.Read32AlignedFast();
            result += bitStream.Read32AlignedFast();
            result += bitStream.Read32AlignedFast();
            result += bitStream.Read32AlignedFast();
            result += bitStream.Read32AlignedFast();
        }

        return result;
    }

    [Benchmark]
    public nuint Read64()
    {
        var maxNumIterations = NumBytes / 8 / 8;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);
        nuint result = 0;

        for (var numIterations = 0; numIterations < maxNumIterations; numIterations++)
        {
            result += (nuint)bitStream.Read64(64);
            result += (nuint)bitStream.Read64(64);
            result += (nuint)bitStream.Read64(64);
            result += (nuint)bitStream.Read64(64);
            result += (nuint)bitStream.Read64(64);
            result += (nuint)bitStream.Read64(64);
            result += (nuint)bitStream.Read64(64);
            result += (nuint)bitStream.Read64(64);
        }
            
        return result;
    }
    
    [Benchmark]
    public nuint Read64Aligned()
    {
        var maxNumIterations = NumBytes / 8 / 8;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);
        nuint result = 0;

        for (var numIterations = 0; numIterations < maxNumIterations; numIterations++)
        {
            result += (nuint)bitStream.Read64Aligned();
            result += (nuint)bitStream.Read64Aligned();
            result += (nuint)bitStream.Read64Aligned();
            result += (nuint)bitStream.Read64Aligned();
            result += (nuint)bitStream.Read64Aligned();
            result += (nuint)bitStream.Read64Aligned();
            result += (nuint)bitStream.Read64Aligned();
            result += (nuint)bitStream.Read64Aligned();
        }
            
        return result;
    }
    
    [Benchmark]
    public nuint Read64AlignedFast()
    {
        var maxNumIterations = NumBytes / 8 / 8;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);
        nuint result = 0;
        
        for (var numIterations = 0; numIterations < maxNumIterations; numIterations++)
        {
            result += (nuint)bitStream.Read64AlignedFast();
            result += (nuint)bitStream.Read64AlignedFast();
            result += (nuint)bitStream.Read64AlignedFast();
            result += (nuint)bitStream.Read64AlignedFast();
            result += (nuint)bitStream.Read64AlignedFast();
            result += (nuint)bitStream.Read64AlignedFast();
            result += (nuint)bitStream.Read64AlignedFast();
            result += (nuint)bitStream.Read64AlignedFast();
        }
            
        return result;
    }

    [Benchmark]
    public nuint Read8Generic()
    {
        var maxNumIterations = NumBytes / 8;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);
        nuint result = 0;
        
        for (var numIterations = 0; numIterations < maxNumIterations; numIterations++)
        {
            result += bitStream.Read<byte>();
            result += bitStream.Read<byte>();
            result += bitStream.Read<byte>();
            result += bitStream.Read<byte>();
            result += bitStream.Read<byte>();
            result += bitStream.Read<byte>();
            result += bitStream.Read<byte>();
            result += bitStream.Read<byte>();
        }

        return result;
    }
}