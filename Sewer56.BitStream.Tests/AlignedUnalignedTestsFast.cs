using System.Runtime.CompilerServices;
using Sewer56.BitStream.ByteStreams;
using Sewer56.BitStream.Interfaces;
using Xunit;
using static Sewer56.BitStream.Tests.Helpers.Helpers;

namespace Sewer56.BitStream.Tests;

/// <summary>
/// Set of tests which tests whether aligned and unaligned reads match.
/// </summary>
public class AlignedUnalignedTestsFast
{
    const int Offset = 256;
    const int NumTestedValues = 8 + Offset;
    
    [Fact]
    public void AlignedReadFastMatches_Array()
    {
        var arrayStream = CreateArrayStream(NumTestedValues + 1, 0b10101010);
        var stream = new BitStream<ArrayByteStream>(arrayStream);
        AlignedReadFastMatches_Common(stream);
    }
    
    [Fact]
    public void AlignedReadFastMatches_Memory()
    {
        var arrayStream = CreateMemoryStream(NumTestedValues + 1, 0b10101010);
        var stream = new BitStream<MemoryByteStream>(arrayStream);
        AlignedReadFastMatches_Common(stream);
    }
    
    [Fact]
    public unsafe void AlignedReadFastMatches_Pointer()
    {
        var data = new byte[NumTestedValues + 1];
        fixed (byte* dataPtr = &data[0])
        {
            var arrayStream = CreatePointerStream(dataPtr, 0b10101010);
            var stream = new BitStream<PointerByteStream>(arrayStream);
            AlignedReadFastMatches_Common(stream);
        }
    }

    private static void AlignedReadFastMatches_Common<TStream>(BitStream<TStream> stream) where TStream : IByteStream, IStreamWithReadBasicPrimitives
    {
        // Write 8 values.
        for (int x = 0; x < Offset; x++)
        {
            stream.BitIndex = 8 * x;

            CompareAlignedUnalignedReadFast<TStream, byte>(stream);
            CompareAlignedUnalignedReadFast<TStream, short>(stream);
            CompareAlignedUnalignedReadFast<TStream, int>(stream);
            CompareAlignedUnalignedReadFast<TStream, long>(stream);
        }
    }

    static unsafe void CompareAlignedUnalignedReadFast<TStream, T>(BitStream<TStream> bitStream) where T : unmanaged 
        where TStream : IByteStream, IStreamWithReadBasicPrimitives
    {
        var unaligned = bitStream.Read<T>();
        bitStream.SeekRelative(-sizeof(T));
        var aligned = bitStream.ReadAlignedFast<TStream, T>();
        bitStream.SeekRelative(-sizeof(T));
        Assert.Equal(unaligned, aligned);
    }
    
    [Fact]
    public void AlignedWriteMatches_Array()
    {
        var arrayStream = CreateArrayStream(NumTestedValues + 1, 0b10101010);
        var stream = new BitStream<ArrayByteStream>(arrayStream);
        CompareAlignedUnalignedWriteFast_Common(stream);
    }
    
    [Fact]
    public unsafe void AlignedWriteMatches_Pointer()
    {
        var data = new byte[NumTestedValues + 1];
        fixed (byte* dataPtr = &data[0])
        {
            var arrayStream = CreatePointerStream(dataPtr, 0b10101010);
            var stream = new BitStream<PointerByteStream>(arrayStream);
            CompareAlignedUnalignedWriteFast_Common(stream);
        }
    }
    
    [Fact]
    public void AlignedWriteMatches_Memory()
    {
        var arrayStream = CreateMemoryStream(NumTestedValues + 1, 0b10101010);
        var stream = new BitStream<MemoryByteStream>(arrayStream);
        CompareAlignedUnalignedWriteFast_Common(stream);
    }

    private static void CompareAlignedUnalignedWriteFast_Common<TStream>(BitStream<TStream> stream) 
        where TStream : IByteStream, IStreamWithReadBasicPrimitives
    {
        for (int x = 0; x < Offset; x++)
        {
            stream.BitIndex = 8 * x;
            CompareAlignedUnalignedWriteFast(stream, (byte)x);
            CompareAlignedUnalignedWriteFast(stream, (short)x);
            CompareAlignedUnalignedWriteFast(stream, x);
            CompareAlignedUnalignedWriteFast<TStream, long>(stream, x);
        }
    }

    static unsafe void CompareAlignedUnalignedWriteFast<TStream, T>(BitStream<TStream> bitStream, T offset) where T : unmanaged
        where TStream : IByteStream, IStreamWithReadBasicPrimitives
    {
        // First write unaligned, read with both and compare
        bitStream.Write<T>(offset);
        bitStream.SeekRelative(-sizeof(T));
        CompareAlignedUnalignedWriteValueFast(bitStream, offset);

        // Now write Aligned, And Repeat
        bitStream.WriteAlignedFast(offset);
        bitStream.SeekRelative(-sizeof(T));
        CompareAlignedUnalignedWriteValueFast(bitStream, offset);
    }
        
    static unsafe void CompareAlignedUnalignedWriteValueFast<TStream, T>(BitStream<TStream> bitStream, T offset) where T : unmanaged
        where TStream : IByteStream, IStreamWithReadBasicPrimitives
    {
        var unaligned = bitStream.Read<T>();
        bitStream.SeekRelative(-sizeof(T));
        var aligned = bitStream.ReadAlignedFast<TStream, T>();
        bitStream.SeekRelative(-sizeof(T));

        Assert.Equal(offset, unaligned);
        Assert.Equal(unaligned, aligned);
    }
}