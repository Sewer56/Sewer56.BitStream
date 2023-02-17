using System;
using System.Collections;
using System.Collections.Generic;
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

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    private unsafe void WriteReadGeneric(bool unaligned)
    {
        const int numTestedValues = byte.MaxValue;

        var arrayStream = CreateArrayStream((sizeof(TestStruct) * numTestedValues) + 1, 0b10101010);
        var stream = new BitStream<ArrayByteStream>(arrayStream);

        // Write all values
        if (unaligned)
            stream.WriteBit(1);
        
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
        if (unaligned)
            stream.ReadBit();
        
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

    [Theory]
    [ClassData(typeof(CalculatorTestData))]
    private unsafe void WriteReadSpanArray(int bytesPerArray, bool unaligned)
    {
        const int numTestedValues = byte.MaxValue;

        var arrayStream = CreateArrayStream((bytesPerArray * numTestedValues) + 1, 0b10101010);
        var stream = new BitStream<ArrayByteStream>(arrayStream);
        WriteReadSpanGeneric(bytesPerArray, unaligned, numTestedValues, stream);
    }
    
    [Theory]
    [ClassData(typeof(CalculatorTestData))]
    private unsafe void WriteReadSpanMemory(int bytesPerArray, bool unaligned)
    {
        const int numTestedValues = byte.MaxValue;

        var arrayStream = CreateMemoryStream((bytesPerArray * numTestedValues) + 1, 0b10101010);
        var stream = new BitStream<MemoryByteStream>(arrayStream);
        WriteReadSpanGeneric(bytesPerArray, unaligned, numTestedValues, stream);
    }
    
    [Theory]
    [ClassData(typeof(CalculatorTestData))]
    private unsafe void WriteReadSpanStream(int bytesPerArray, bool unaligned)
    {
        const int numTestedValues = byte.MaxValue;

        var arrayStream = CreateStreamStream((bytesPerArray * numTestedValues) + 1, 0b10101010);
        var stream = new BitStream<StreamByteStream>(arrayStream);
        WriteReadSpanGeneric(bytesPerArray, unaligned, numTestedValues, stream);
    }
    
    [Theory]
    [ClassData(typeof(CalculatorTestData))]
    private unsafe void WriteReadSpanPointer(int bytesPerArray, bool unaligned)
    {
        const int numTestedValues = byte.MaxValue;

        var buffer = new byte[(bytesPerArray * numTestedValues) + 1];
        fixed (byte* bufferPtr = &buffer[0])
        {
            var arrayStream = CreatePointerStream(bufferPtr, buffer.Length, 0b10101010);
            var stream = new BitStream<PointerByteStream>(arrayStream);
            WriteReadSpanGeneric(bytesPerArray, unaligned, numTestedValues, stream);
        }
    }

    private static void WriteReadSpanGeneric<T>(int bytesPerArray, bool unaligned, int numTestedValues, BitStream<T> stream) where T : IByteStream
    {
        // Write all values
        var readWriteBuffer = new byte[bytesPerArray];
        var expectedValuesArray = new byte[numTestedValues][];

        if (unaligned)
            stream.WriteBit(0);

        for (int x = 0; x < numTestedValues; x++)
        {
            var random = new Random(x);
            random.NextBytes(readWriteBuffer);
            stream.Write(readWriteBuffer);
            expectedValuesArray[x] = readWriteBuffer.ToArray();
        }

        // Read all values.
        stream.BitIndex = 0;
        if (unaligned)
            stream.ReadBit();

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
    
    public class CalculatorTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            for (int x = 1; x < 256; x *= 2)
            {
                yield return new object[] { x, true };
                yield return new object[] { x, false };
                
                yield return new object[] { x + 1, true };
                yield return new object[] { x + 1, false };

                if (x <= 1) 
                    continue;
                
                yield return new object[] { x - 1, true };
                yield return new object[] { x - 1, false };
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    [Fact]
    private unsafe void NextByteIndex()
    {
        const int numTestedValues  = 3;
        const int numBytesPerValue = 1;

        var arrayStream = CreateArrayStream((numBytesPerValue * numTestedValues) + 1, 0b10101010);
        var stream = new BitStream<ArrayByteStream>(arrayStream);

        // Check case where bitOffset == 0
        Assert.Equal(0, stream.NextByteIndex);

        // Check case where bitOffset != 0
        for (int x = 0; x < 8; x++)
        {
            stream.WriteBit(0);
            Assert.Equal(1, stream.NextByteIndex);
        }

        // Check for correct loop.
        for (int x = 0; x < 8; x++)
        {
            stream.WriteBit(0);
            Assert.Equal(2, stream.NextByteIndex);
        }

        stream.WriteBit(0);
        Assert.Equal(3, stream.NextByteIndex);
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