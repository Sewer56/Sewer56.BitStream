using BenchmarkDotNet.Attributes;
using Sewer56.BitStream.ByteStreams;

namespace Sewer56.BitStream.Benchmarks;

[MemoryDiagnoser]
public class WriteBenchmark : BenchmarkBase
{
    [Benchmark]
    public int WriteBit()
    {
        var maxNumIterations = NumBytes * 8 / 8;
        var numIterations    = 0;
        var stream           = new ArrayByteStream(_data); 
        var bitStream        = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.WriteBit(1);
            bitStream.WriteBit(1);
            bitStream.WriteBit(1);
            bitStream.WriteBit(1);
            bitStream.WriteBit(1);
            bitStream.WriteBit(1);
            bitStream.WriteBit(1);
            bitStream.WriteBit(1);
        }

        return numIterations;
    }

    [Benchmark]
    public int Write8()
    {
        var maxNumIterations = NumBytes / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Write8((byte)numIterations, 8);
            bitStream.Write8((byte)numIterations, 8);
            bitStream.Write8((byte)numIterations, 8);
            bitStream.Write8((byte)numIterations, 8);
            bitStream.Write8((byte)numIterations, 8);
            bitStream.Write8((byte)numIterations, 8);
            bitStream.Write8((byte)numIterations, 8);
            bitStream.Write8((byte)numIterations, 8);
        }

        return numIterations;
    }
    
    [Benchmark]
    public int Write8Aligned()
    {
        var maxNumIterations = NumBytes / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Write8Aligned((byte)numIterations);
            bitStream.Write8Aligned((byte)numIterations);
            bitStream.Write8Aligned((byte)numIterations);
            bitStream.Write8Aligned((byte)numIterations);
            bitStream.Write8Aligned((byte)numIterations);
            bitStream.Write8Aligned((byte)numIterations);
            bitStream.Write8Aligned((byte)numIterations);
            bitStream.Write8Aligned((byte)numIterations);
        }

        return numIterations;
    }

    [Benchmark]
    public int Write16()
    {
        var maxNumIterations = NumBytes / 2 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Write16((ushort)numIterations, 16);
            bitStream.Write16((ushort)numIterations, 16);
            bitStream.Write16((ushort)numIterations, 16);
            bitStream.Write16((ushort)numIterations, 16);
            bitStream.Write16((ushort)numIterations, 16);
            bitStream.Write16((ushort)numIterations, 16);
            bitStream.Write16((ushort)numIterations, 16);
            bitStream.Write16((ushort)numIterations, 16);
        }

        return numIterations;
    }
    
    [Benchmark]
    public int Write16Aligned()
    {
        var maxNumIterations = NumBytes / 2 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Write16Aligned((ushort)numIterations);
            bitStream.Write16Aligned((ushort)numIterations);
            bitStream.Write16Aligned((ushort)numIterations);
            bitStream.Write16Aligned((ushort)numIterations);
            bitStream.Write16Aligned((ushort)numIterations);
            bitStream.Write16Aligned((ushort)numIterations);
            bitStream.Write16Aligned((ushort)numIterations);
            bitStream.Write16Aligned((ushort)numIterations);
        }

        return numIterations;
    }
    
    [Benchmark]
    public int Write16AlignedFast()
    {
        var maxNumIterations = NumBytes / 2 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Write16AlignedFast((ushort)numIterations);
            bitStream.Write16AlignedFast((ushort)numIterations);
            bitStream.Write16AlignedFast((ushort)numIterations);
            bitStream.Write16AlignedFast((ushort)numIterations);
            bitStream.Write16AlignedFast((ushort)numIterations);
            bitStream.Write16AlignedFast((ushort)numIterations);
            bitStream.Write16AlignedFast((ushort)numIterations);
            bitStream.Write16AlignedFast((ushort)numIterations);
        }

        return numIterations;
    }

    [Benchmark]
    public int Write32()
    {
        var maxNumIterations = NumBytes / 4 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Write32((uint)numIterations, 32);
            bitStream.Write32((uint)numIterations, 32);
            bitStream.Write32((uint)numIterations, 32);
            bitStream.Write32((uint)numIterations, 32);
            bitStream.Write32((uint)numIterations, 32);
            bitStream.Write32((uint)numIterations, 32);
            bitStream.Write32((uint)numIterations, 32);
            bitStream.Write32((uint)numIterations, 32);
        }

        return numIterations;
    }
    
    [Benchmark]
    public int Write32Aligned()
    {
        var maxNumIterations = NumBytes / 4 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Write32Aligned((uint)numIterations);
            bitStream.Write32Aligned((uint)numIterations);
            bitStream.Write32Aligned((uint)numIterations);
            bitStream.Write32Aligned((uint)numIterations);
            bitStream.Write32Aligned((uint)numIterations);
            bitStream.Write32Aligned((uint)numIterations);
            bitStream.Write32Aligned((uint)numIterations);
            bitStream.Write32Aligned((uint)numIterations);
        }

        return numIterations;
    }
    
    [Benchmark]
    public int Write32AlignedFast()
    {
        var maxNumIterations = NumBytes / 4 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Write32AlignedFast((uint)numIterations);
            bitStream.Write32AlignedFast((uint)numIterations);
            bitStream.Write32AlignedFast((uint)numIterations);
            bitStream.Write32AlignedFast((uint)numIterations);
            bitStream.Write32AlignedFast((uint)numIterations);
            bitStream.Write32AlignedFast((uint)numIterations);
            bitStream.Write32AlignedFast((uint)numIterations);
            bitStream.Write32AlignedFast((uint)numIterations);
        }

        return numIterations;
    }

    [Benchmark]
    public int Write64()
    {
        var maxNumIterations = NumBytes / 8 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Write64((ulong)numIterations, 64);
            bitStream.Write64((ulong)numIterations, 64);
            bitStream.Write64((ulong)numIterations, 64);
            bitStream.Write64((ulong)numIterations, 64);
            bitStream.Write64((ulong)numIterations, 64);
            bitStream.Write64((ulong)numIterations, 64);
            bitStream.Write64((ulong)numIterations, 64);
            bitStream.Write64((ulong)numIterations, 64);
        }
            
        return numIterations;
    }
    
    [Benchmark]
    public int Write64Aligned()
    {
        var maxNumIterations = NumBytes / 8 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Write64Aligned((ulong)numIterations);
            bitStream.Write64Aligned((ulong)numIterations);
            bitStream.Write64Aligned((ulong)numIterations);
            bitStream.Write64Aligned((ulong)numIterations);
            bitStream.Write64Aligned((ulong)numIterations);
            bitStream.Write64Aligned((ulong)numIterations);
            bitStream.Write64Aligned((ulong)numIterations);
            bitStream.Write64Aligned((ulong)numIterations);
        }
            
        return numIterations;
    }
    
    [Benchmark]
    public int Write64AlignedFast()
    {
        var maxNumIterations = NumBytes / 8 / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Write64AlignedFast((ulong)numIterations);
            bitStream.Write64AlignedFast((ulong)numIterations);
            bitStream.Write64AlignedFast((ulong)numIterations);
            bitStream.Write64AlignedFast((ulong)numIterations);
            bitStream.Write64AlignedFast((ulong)numIterations);
            bitStream.Write64AlignedFast((ulong)numIterations);
            bitStream.Write64AlignedFast((ulong)numIterations);
            bitStream.Write64AlignedFast((ulong)numIterations);
        }
            
        return numIterations;
    }

    [Benchmark]
    public int Write8Generic()
    {
        var maxNumIterations = NumBytes / 8;
        var numIterations = 0;
        var stream = new ArrayByteStream(_data);
        var bitStream = new BitStream<ArrayByteStream>(stream, 0);

        for (; numIterations < maxNumIterations; numIterations++)
        {
            bitStream.Write((byte)numIterations);
            bitStream.Write((byte)numIterations);
            bitStream.Write((byte)numIterations);
            bitStream.Write((byte)numIterations);
            bitStream.Write((byte)numIterations);
            bitStream.Write((byte)numIterations);
            bitStream.Write((byte)numIterations);
            bitStream.Write((byte)numIterations);
        }

        return numIterations;
    }
}