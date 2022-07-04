using Sewer56.BitStream.ByteStreams;
using System;
using System.ComponentModel;
using Xunit;
using static Sewer56.BitStream.Tests.Helpers.Helpers;

namespace Sewer56.BitStream.Tests;

public class Write
{
    [Fact]
    public void WriteBit() => WriteTest
    (
        1,
        (str, val, num) => str.WriteBit((byte)val),
        (str, num) => str.ReadBit()
    );

    [Fact]
    public void Write8() => WriteTest
    (
        8, 
        (str, val, num) => str.Write8((byte) val, num),
        (str, num) => str.Read8(num)
    );

    [Fact]
    public void Write16() => WriteTest
    (
        16,
        (str, val, num) => str.Write16((ushort)val, num),
        (str, num) => str.Read16(num)
    );


    [Fact]
    public void Write32() => WriteTest
    (
        32,
        (str, val, num) => str.Write32((uint)val, num),
        (str, num) => str.Read32(num)
    );


    [Fact]
    public void Write64() => WriteTest
    (
        64,
        (str, val, num) => str.Write64((ulong)val, num),
        (str, num) => str.Read64(num)
    );

    [Fact]
    public void WriteBoundary()
    {
        var arrayStream = CreateArrayStream(sizeof(ulong) + 1, 0);
        var bitStream = new BitStream<ArrayByteStream>(arrayStream);

        int bitsPlayers = 4;

        for (int numPlayers = 0; numPlayers < 16; numPlayers++)
        {
            int expectedPlayers = numPlayers;
            for (int bitsFlags = 5; bitsFlags < 9; bitsFlags++)
            {
                int maxFlags = (1 << bitsFlags) - 1;
                for (int y = 0; y < maxFlags; y++)
                {
                    int expectedFlags = y;
                    ResetArray(arrayStream.Array, 0x0);

                    bitStream.BitIndex = 0;
                    bitStream.Write8((byte)expectedPlayers, bitsPlayers);
                    bitStream.Write8((byte)expectedFlags, bitsFlags);

                    bitStream.BitIndex = 0;
                    int playerCount = bitStream.Read8(bitsPlayers);
                    ushort flags    = bitStream.Read8(bitsFlags);

                    Assert.Equal(expectedPlayers, playerCount);
                    Assert.Equal(expectedFlags, flags);
                }
            }
        }
    }

    private void WriteTest(int maxNumBits, Action<BitStream<ArrayByteStream>, ulong, int> writeValue, Func<BitStream<ArrayByteStream>, int, ulong> readValue)
    {
        var arrayStream = CreateArrayStream(sizeof(ulong) + 1, 0b10101010);

        for (int bitIndex = 0; bitIndex < 8; ++bitIndex)
        {
            for (int numBits = 1; numBits <= maxNumBits; ++numBits)
            {
                ResetArray(arrayStream.Array);
                var stream = new BitStream<ArrayByteStream>(arrayStream, bitIndex);
                ulong value = (ulong)((1 << numBits) - 1);
                writeValue(stream, value, numBits);

                if (bitIndex > 0)
                {
                    stream.BitIndex = 0;
                    Assert.Equal(stream.Read8(bitIndex), MakeHighPatternBits(bitIndex));
                }

                stream.BitIndex = bitIndex;
                Assert.Equal(readValue(stream, numBits), value);
                int afterBitIndex = bitIndex + numBits;
                int afterLocalBitIndex = afterBitIndex % 8;
                if (afterLocalBitIndex != 0)
                {
                    stream.BitIndex = afterBitIndex;
                    int numLowBits = 8 - afterLocalBitIndex;
                    Assert.Equal(stream.Read8(numLowBits), MakeLowPatternBits(numLowBits));
                }
            }
        }
    }
}