using BenchmarkDotNet.Attributes;
using Sewer56.BitStream.ByteStreams;

namespace Sewer56.BitStream.Benchmarks;

[MemoryDiagnoser]
public class ReadBenchmark : BenchmarkBase
{
    [Benchmark]
    public int ReadBit()
    {
        var maxNumIterations = (NumBytes * 8) / 8;
        var numIterations    = 0;
        var stream           = new ArrayByteStream(_data); 
        var bitStream        = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.ReadBit();
            bitStream.ReadBit();
            bitStream.ReadBit();
            bitStream.ReadBit();
            bitStream.ReadBit();
            bitStream.ReadBit();
            bitStream.ReadBit();
            bitStream.ReadBit();
        }

        return numIterations;
    }

    [Benchmark]
    public int Read8()
    {
        var maxNumIterations = NumBytes / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Read8(8);
            bitStream.Read8(8);
            bitStream.Read8(8);
            bitStream.Read8(8);
            bitStream.Read8(8);
            bitStream.Read8(8);
            bitStream.Read8(8);
            bitStream.Read8(8);
        }

        return numIterations;
    }

    [Benchmark]
    public int Read8Aligned()
    {
        var maxNumIterations = NumBytes / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Read8Aligned();
            bitStream.Read8Aligned();
            bitStream.Read8Aligned();
            bitStream.Read8Aligned();
            bitStream.Read8Aligned();
            bitStream.Read8Aligned();
            bitStream.Read8Aligned();
            bitStream.Read8Aligned();
        }

        return numIterations;
    }
    
    [Benchmark]
    public int Read16()
    {
        var maxNumIterations = NumBytes / 2 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Read16(16);
            bitStream.Read16(16);
            bitStream.Read16(16);
            bitStream.Read16(16);
            bitStream.Read16(16);
            bitStream.Read16(16);
            bitStream.Read16(16);
            bitStream.Read16(16);
        }

        return numIterations;
    }
    
    [Benchmark]
    public int Read16Aligned()
    {
        var maxNumIterations = NumBytes / 2 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Read16Aligned();
            bitStream.Read16Aligned();
            bitStream.Read16Aligned();
            bitStream.Read16Aligned();
            bitStream.Read16Aligned();
            bitStream.Read16Aligned();
            bitStream.Read16Aligned();
            bitStream.Read16Aligned();
        }

        return numIterations;
    }
    
    [Benchmark]
    public int Read16AlignedFast()
    {
        var maxNumIterations = NumBytes / 2 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Read16AlignedFast();
            bitStream.Read16AlignedFast();
            bitStream.Read16AlignedFast();
            bitStream.Read16AlignedFast();
            bitStream.Read16AlignedFast();
            bitStream.Read16AlignedFast();
            bitStream.Read16AlignedFast();
            bitStream.Read16AlignedFast();
        }

        return numIterations;
    }

    [Benchmark]
    public int Read32()
    {
        var maxNumIterations = NumBytes / 4 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Read32(32);
            bitStream.Read32(32);
            bitStream.Read32(32);
            bitStream.Read32(32);
            bitStream.Read32(32);
            bitStream.Read32(32);
            bitStream.Read32(32);
            bitStream.Read32(32);
        }

        return numIterations;
    }
    
    [Benchmark]
    public int Read32Aligned()
    {
        var maxNumIterations = NumBytes / 4 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Read32Aligned();
            bitStream.Read32Aligned();
            bitStream.Read32Aligned();
            bitStream.Read32Aligned();
            bitStream.Read32Aligned();
            bitStream.Read32Aligned();
            bitStream.Read32Aligned();
            bitStream.Read32Aligned();
        }

        return numIterations;
    }
    
    [Benchmark]
    public int Read32AlignedFast()
    {
        var maxNumIterations = NumBytes / 4 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Read32AlignedFast();
            bitStream.Read32AlignedFast();
            bitStream.Read32AlignedFast();
            bitStream.Read32AlignedFast();
            bitStream.Read32AlignedFast();
            bitStream.Read32AlignedFast();
            bitStream.Read32AlignedFast();
            bitStream.Read32AlignedFast();
        }

        return numIterations;
    }

    [Benchmark]
    public int Read64()
    {
        var maxNumIterations = NumBytes / 8 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Read64(64);
            bitStream.Read64(64);
            bitStream.Read64(64);
            bitStream.Read64(64);
            bitStream.Read64(64);
            bitStream.Read64(64);
            bitStream.Read64(64);
            bitStream.Read64(64);
        }
            
        return numIterations;
    }
    
    [Benchmark]
    public int Read64Aligned()
    {
        var maxNumIterations = NumBytes / 8 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Read64Aligned();
            bitStream.Read64Aligned();
            bitStream.Read64Aligned();
            bitStream.Read64Aligned();
            bitStream.Read64Aligned();
            bitStream.Read64Aligned();
            bitStream.Read64Aligned();
            bitStream.Read64Aligned();
        }
            
        return numIterations;
    }
    
    [Benchmark]
    public int Read64AlignedFast()
    {
        var maxNumIterations = NumBytes / 8 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Read64AlignedFast();
            bitStream.Read64AlignedFast();
            bitStream.Read64AlignedFast();
            bitStream.Read64AlignedFast();
            bitStream.Read64AlignedFast();
            bitStream.Read64AlignedFast();
            bitStream.Read64AlignedFast();
            bitStream.Read64AlignedFast();
        }
            
        return numIterations;
    }

    [Benchmark]
    public int Read8Generic()
    {
        var maxNumIterations = NumBytes / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Read<byte>();
            bitStream.Read<byte>();
            bitStream.Read<byte>();
            bitStream.Read<byte>();
            bitStream.Read<byte>();
            bitStream.Read<byte>();
            bitStream.Read<byte>();
            bitStream.Read<byte>();
        }

        return numIterations;
    }
}