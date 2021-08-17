using Sewer56.BitStream.ByteStreams;
using System;
using Xunit;
using static Sewer56.BitStream.Tests.Helpers.Helpers;

namespace Sewer56.BitStream.Tests
{
    public class Read
    {
        [Fact]
        public void ReadBit() => ReadTest(1, (expected, numBits, stream) => Assert.Equal((byte) expected, stream.ReadBit()));

        [Fact]
        public void Read8() => ReadTest(8, (expected, numBits, stream) => Assert.Equal((byte) expected, stream.Read<byte>(numBits)));

        [Fact]
        public void Read16() => ReadTest(16, (expected, numBits, stream) => Assert.Equal((ushort) expected, stream.Read<ushort>(numBits)));

        [Fact]
        public void Read32() => ReadTest(32, (expected, numBits, stream) => Assert.Equal((uint) expected, stream.Read<uint>(numBits)));

        [Fact]
        public void Read64() => ReadTest(64, (expected, numBits, stream) => Assert.Equal(expected, stream.Read<ulong>(numBits)));

        private void ReadTest(int maxNumBits, Action<ulong, int, BitStream<ArrayByteStream>> assertAction)
        {
            var arrayStream = CreateArrayStream(sizeof(ulong) + 1, 0b10101010);
            for (int bitIndex = 0; bitIndex < 8; ++bitIndex)
            {
                ulong expected = 0;
                ulong next = (ulong)(bitIndex & 1);

                for (int numBits = 1; numBits <= maxNumBits; ++numBits)
                {
                    next = ~next & 1;
                    expected = (expected << 1) | next;

                    var stream = new BitStream<ArrayByteStream>(arrayStream, bitIndex);
                    assertAction(expected, numBits, stream);
                }
            }
        }
    }
}
