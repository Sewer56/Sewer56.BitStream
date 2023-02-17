using System.Runtime.CompilerServices;
using Sewer56.BitStream.ByteStreams;
using Xunit;
using static Sewer56.BitStream.Tests.Helpers.Helpers;

namespace Sewer56.BitStream.Tests;

/// <summary>
/// Set of tests which tests whether aligned and unaligned reads match.
/// </summary>
public class AlignedUnalignedTests
{
    [Fact]
    public void AlignedReadMatches()
    {
        const int offset = 256;
        const int numTestedValues = 8 + offset;
        var arrayStream = CreateArrayStream(numTestedValues + 1, 0b10101010);
        var stream = new BitStream<ArrayByteStream>(arrayStream);
        
        // Write 8 values.
        for (int x = 0; x < offset; x++)
        {
            stream.BitIndex = 8 * x;
            
            CompareAlignedUnalignedRead<byte>(stream);
            CompareAlignedUnalignedRead<short>(stream);
            CompareAlignedUnalignedRead<int>(stream);
            CompareAlignedUnalignedRead<long>(stream);
        }
        
        unsafe void CompareAlignedUnalignedRead<T>(BitStream<ArrayByteStream> bitStream) where T : unmanaged
        {
            var unaligned = bitStream.Read<T>();
            bitStream.SeekRelative(-sizeof(T));
            var aligned = bitStream.ReadAligned<T>();
            bitStream.SeekRelative(-sizeof(T));
            Assert.Equal(unaligned, aligned);
        }
    }
    
    [Fact]
    public void AlignedWriteMatches()
    {
        const int offset = 256;
        const int numTestedValues = 8 + offset;
        var arrayStream = CreateArrayStream(numTestedValues + 1, 0b10101010);
        var stream = new BitStream<ArrayByteStream>(arrayStream);
        
        // Write 8 values.
        for (int x = 0; x < offset; x++)
        {
            stream.BitIndex = 8 * x;
            
            CompareAlignedUnalignedWrite<byte>(stream, (byte)x);
            CompareAlignedUnalignedWrite<short>(stream, (short)x);
            CompareAlignedUnalignedWrite<int>(stream, x);
            CompareAlignedUnalignedWrite<long>(stream, x);
        }
        
        unsafe void CompareAlignedUnalignedWrite<T>(BitStream<ArrayByteStream> bitStream, T offset) where T : unmanaged
        {
            // First write unaligned, read with both and compare
            bitStream.Write<T>(offset);
            bitStream.SeekRelative(-sizeof(T));
            CompareAlignedUnalignedWriteValue(bitStream, offset);

            // Now write Aligned, And Repeat
            bitStream.WriteAligned<T>(offset);
            bitStream.SeekRelative(-sizeof(T));
            CompareAlignedUnalignedWriteValue(bitStream, offset);
        }
        
        unsafe void CompareAlignedUnalignedWriteValue<T>(BitStream<ArrayByteStream> bitStream, T offset) where T : unmanaged
        {
            var unaligned = bitStream.Read<T>();
            bitStream.SeekRelative(-sizeof(T));
            var aligned = bitStream.ReadAligned<T>();
            bitStream.SeekRelative(-sizeof(T));

            Assert.Equal(offset, unaligned);
            Assert.Equal(unaligned, aligned);
        }
    }
}