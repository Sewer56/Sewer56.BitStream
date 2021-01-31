# Sewer56.BitStream
Efficient reusable BitStream library with support for generics; no virtual function calls, zero heap allocations.

## Introduction
Bitstreams unlike regular streams, operate at the bit level, allowing you to read/write bits.

This minimalistic library adds bit level manipulation to a user provided stream which can be backed by an arbitrary source such as a byte array, `Memory<T>` or `Stream`.
This library project attempts to add bit manipulation to a stream while using known `Stream`, `BinaryReader` and `BinaryWriter` class methods

## Usage
Initialize a `BitStream` using an implementation of `IByteStream` as Generic parameter.

```csharp
var stream = new ArrayByteStream(buffer); // buffer is byte[]
var bitStream = new BitStream<ArrayByteStream>(arrayStream, bitIndex);
```

Note: Existing implementations of `IByteStream` can be found in the `Sewer56.BitStream.ByteStreams` namespace.

## Features

### Reading/Writing Primitives
Reading and writing bits support the following standard integral types (`byte, sbyte, short, ushort, int, uint, long, ulong`).

To read bits, use the `Read<T>(int numbits)` method.

```csharp
// If size not supplied, uses size of supplied type.
bitStream.Read<byte>();  // 8 bits
bitStream.Read<short>(); // 16 bits
bitStream.Read<int>();   // 32 bits
bitStream.Read<long>();  // 64 bits

// Read specific number of bits.
bitStream.Read<byte>(5);   // 5 bits
bitStream.Read<short>(11); // 11 bits
```

To write bits, use the `Write<T>(T value, int numBits)` method.

```csharp
// If size supplied, uses size of supplied type.
stream.Write(Byte);  // 8 bits
stream.Write(Short); // 16 bits
stream.Write(Int);   // 32 bits
stream.Write(Long);  // 64 bits

// Write specific number of bits.
stream.Write(Byte, 5);   // 5 bits
stream.Write(Short, 11); // 11 bits
```

Due to clever under the hood trickery, there is no performance penalty for using the generic overloads. 

That said, if you really, really want, you can use `Read8`, `Read16`, `Write8`, `Write16` etc.

### Seeking
Seeking in a BitStream uses the `Seek(long offset, int bit)` and `SeekRelative(long offset, int bit)` to specify the stream position.

Alternatively you can modify the `BitIndex` to set the absolute bit index.

### Reading/Writing Structures

In order to read/write structures, you should implement the `IBitPackable` interface.

Example:
```csharp
public struct TestStruct : IBitPackable<TestStruct>
{
    public byte     Byte;
    public short    Short;
    public int      Int;
    public long     Long;

    // Deserialize
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

    // Serialize
    public void ToStream<T>(ref BitStream<T> stream) where T : IByteStream
    {
        stream.Write(Byte);
        stream.Write(Short);
        stream.Write(Int);
        stream.Write(Long);
    }
}
```

For performance reasons, it's generally recommended to use `structs` instead of `classes` because using classes incurs heap allocations.

You can then use the `Serialize<T>` and `Deserialize<T>` methods to read/write packable structs.

```csharp
 var expected = new TestStruct
{
    Byte  = (byte)x,
    Short = (short)(byte.MaxValue + x),
    Int   = short.MaxValue + x,
    Long  = (long)int.MaxValue + x,
};

stream.Serialize(ref expected);
var actual = stream.Deserialize<TestStruct>();
```

### Reading/Writing Structures (No Interface)
If you want to just write structs without any bit packing whatsoever, you can use `ReadStruct<T>` and `WriteStruct<T>` in order to read/write unmanaged structures. 

```csharp
var expected = new TestStruct
{
    Byte = (byte) x,
    Short = (short) (byte.MaxValue + x),
    Int = short.MaxValue + x,
    Long = (long)int.MaxValue + x,
};

// Write struct to memory.
stream.WriteStruct(ref expected);

// Seek and read the struct back.
stream.SeekRelative(-sizeof(TestStruct));
var actual = stream.ReadStruct<TestStruct>();

// They should be equal.
Assert.Equal(expected, actual);
```

Under the hood these options use pointers and spans (without allocations), operating on these structs as if they were bytes.