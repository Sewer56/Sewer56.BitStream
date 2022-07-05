using System;
using System.Linq;
using System.Runtime.InteropServices;
using Sewer56.BitStream.ByteStreams;
using Sewer56.BitStream.Interfaces;
using Xunit;
using static Sewer56.BitStream.Tests.Helpers.Helpers;

namespace Sewer56.BitStream.Tests;

public class Extensions
{
    [Fact]
    private unsafe void ReadWriteStruct()
    {
        const int numTestedValues = byte.MaxValue;

        var arrayStream = CreateArrayStream((sizeof(TestStruct) * numTestedValues) + 1, 0b10101010);
        var stream      = new BitStream<ArrayByteStream>(arrayStream);
            
        // Write all values
        for (int x = 0; x < numTestedValues; x++)
        {
            var expected = new TestStruct
            {
                Byte = (byte) x,
                Short = (short) (byte.MaxValue + x),
                Int = short.MaxValue + x,
                Long = (long)int.MaxValue + x,
            };

            stream.WriteGeneric(ref expected);
        }

        // Read all values.
        stream.BitIndex = 0;
        for (int x = 0; x < numTestedValues; x++)
        {
            var expected = new TestStruct
            {
                Byte = (byte)x,
                Short = (short)(byte.MaxValue + x),
                Int = short.MaxValue + x,
                Long = (long)int.MaxValue + x,
            };

            // Read values.
            Assert.Equal(expected, stream.ReadGeneric<TestStruct>());
        }
    }

    [Fact]
    private unsafe void ReadWriteSeek()
    {
        const int numTestedValues = byte.MaxValue;

        var arrayStream = CreateArrayStream((sizeof(TestStruct) * numTestedValues) + 1, 0b10101010);
        var stream = new BitStream<ArrayByteStream>(arrayStream);

        // Write all values
        for (int x = 0; x < numTestedValues; x++)
        {
            var expected = new TestStruct
            {
                Byte = (byte)x,
                Short = (short)(byte.MaxValue + x),
                Int = short.MaxValue + x,
                Long = (long)int.MaxValue + x,
            };

            stream.WriteGeneric(ref expected);
        }

        // Read all values.
        stream.BitIndex = 0;
        for (int x = 0; x < numTestedValues; x++)
        {
            var expected = new TestStruct
            {
                Byte = (byte)x,
                Short = (short)(byte.MaxValue + x),
                Int = short.MaxValue + x,
                Long = (long)int.MaxValue + x,
            };

            // Read values.
            Assert.Equal(expected, stream.ReadGeneric<TestStruct>());

            // Seek back.
            stream.SeekRelative(-sizeof(TestStruct));
            Assert.Equal(expected, stream.ReadGeneric<TestStruct>());
        }
    }

    [Fact]
    private unsafe void SerializeDeserialize()
    {
        const int numTestedValues = byte.MaxValue;

        var arrayStream = CreateArrayStream((sizeof(TestStruct) * numTestedValues) + 1, 0b10101010);
        var stream = new BitStream<ArrayByteStream>(arrayStream);

        // Write all values
        for (int x = 0; x < numTestedValues; x++)
        {
            var expected = new TestStruct
            {
                Byte = (byte)x,
                Short = (short)(byte.MaxValue + x),
                Int = short.MaxValue + x,
                Long = (long)int.MaxValue + x,
            };

            stream.Serialize(ref expected);
        }

        // Read all values.
        stream.BitIndex = 0;
        for (int x = 0; x < numTestedValues; x++)
        {
            var expected = new TestStruct
            {
                Byte = (byte)x,
                Short = (short)(byte.MaxValue + x),
                Int = short.MaxValue + x,
                Long = (long)int.MaxValue + x,
            };

            // Read values.
            var actual = stream.Deserialize<TestStruct>();
            Assert.Equal(expected, actual);
        }
    }

    [Fact]
    private unsafe void WriteReadGeneric()
    {
        const int numTestedValues = byte.MaxValue;

        var arrayStream = CreateArrayStream((sizeof(TestStruct) * numTestedValues) + 1, 0b10101010);
        var stream = new BitStream<ArrayByteStream>(arrayStream);

        // Write all values
        for (int x = 0; x < numTestedValues; x++)
        {
            var expected = new TestStruct
            {
                Byte = (byte)x,
                Short = (short)(byte.MaxValue + x),
                Int = short.MaxValue + x,
                Long = (long)int.MaxValue + x,
            };

            stream.WriteGeneric(ref expected);
        }

        // Read all values.
        stream.BitIndex = 0;
        for (int x = 0; x < numTestedValues; x++)
        {
            var expected = new TestStruct
            {
                Byte = (byte)x,
                Short = (short)(byte.MaxValue + x),
                Int = short.MaxValue + x,
                Long = (long)int.MaxValue + x,
            };

            // Read values.
            var actual = stream.ReadGeneric<TestStruct>();
            Assert.Equal(expected, actual);
        }
    }

    [Fact]
    private unsafe void WriteReadGenericWithNumBits()
    {
        const int numTestedValues = byte.MaxValue;
        const int ByteAndShortBitLength = 8 + 16;

        var arrayStream = CreateArrayStream((sizeof(TestStruct) * numTestedValues) + 1, 0b10101010);
        var stream = new BitStream<ArrayByteStream>(arrayStream);

        
        // Write all values
        for (int x = 0; x < numTestedValues; x++)
        {
            var expected = new TestStruct
            {
                Byte = (byte)x,
                Short = (short)(byte.MaxValue + x),
                Int = short.MaxValue + x,
                Long = (long)int.MaxValue + x,
            };

            stream.WriteGeneric(ref expected, ByteAndShortBitLength);
        }

        // Read all values.
        stream.BitIndex = 0;
        for (int x = 0; x < numTestedValues; x++)
        {
            var expected = new TestStruct
            {
                Byte = (byte)x,
                Short = (short)(byte.MaxValue + x),
                Int = short.MaxValue + x,
                Long = (long)int.MaxValue + x,
            };

            // Read values.
            var actual = stream.ReadGeneric<TestStruct>(ByteAndShortBitLength);
            Assert.Equal(expected.Byte, actual.Byte);
            Assert.Equal(expected.Short, actual.Short);
        }
    }

    [Fact]
    private unsafe void WriteReadSpanArray()
    {
        const int numTestedValues = byte.MaxValue;
        const int numBytesPerArray = 32;

        var arrayStream = CreateArrayStream((numBytesPerArray * numTestedValues) + 1, 0b10101010);
        var stream = new BitStream<ArrayByteStream>(arrayStream);

        // Write all values
        var readWriteBuffer = new byte[numBytesPerArray];
        var expectedValuesArray = new byte[numTestedValues][];

        for (int x = 0; x < numTestedValues; x++)
        {
            var random = new Random(x);
            random.NextBytes(readWriteBuffer);
            stream.Write(readWriteBuffer);
            expectedValuesArray[x] = readWriteBuffer.ToArray();
        }

        // Read all values.
        stream.BitIndex = 0;
        for (int x = 0; x < numTestedValues; x++)
        {
            // Using same seed, recreate arrays and compare.
            var random = new Random(x);
            random.NextBytes(readWriteBuffer);
            
            // Read values.
            stream.Read(readWriteBuffer);
            Assert.Equal(expectedValuesArray[x], readWriteBuffer);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TestStruct : IEquatable<TestStruct>, IBitPackable<TestStruct>
    {
        public byte     Byte;
        public short    Short;
        public int      Int;
        public long     Long;

        public TestStruct FromStream<T>(ref BitStream<T> stream) where T : IByteStream
        {
            return new TestStruct
            {
                Byte  = stream.Read<byte>(),
                Short = stream.Read<short>(),
                Int   = stream.Read<int>(),
                Long  = stream.Read<long>()
            }; 
        }

        public void ToStream<T>(ref BitStream<T> stream) where T : IByteStream
        {
            stream.Write(Byte);
            stream.Write(Short);
            stream.Write(Int);
            stream.Write(Long);
        }

        // Autogenerated
        public bool Equals(TestStruct other) => Byte == other.Byte && Short == other.Short && Int == other.Int && Long == other.Long;
        public override bool Equals(object obj) => obj is TestStruct other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Byte, Short, Int, Long);
    }
}