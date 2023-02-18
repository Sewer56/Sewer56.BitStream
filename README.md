# Sewer56.BitStream
Efficient reusable BitStream library with support for generics; no virtual function calls, zero heap allocations.

<a href="https://www.nuget.org/packages/Sewer56.BitStream">
	<img src="https://img.shields.io/nuget/v/Sewer56.BitStream.svg" alt="NuGet" />
</a>

<a href="https://codecov.io/Sewer56/Sewer56.BitStream">
	<img src="https://codecov.io/gh/Sewer56/Sewer56.BitStream/branch/senpai/graph/badge.svg" alt="Coverage" />
</a>

## Introduction
Bitstreams unlike regular streams, operate at the bit level, allowing you to read/write bits.  

This minimalistic library adds bit level manipulation to a user provided stream which can be backed by an arbitrary source such as a byte array, `Memory<T>` or `Stream`.  

This library project attempts to add bit manipulation to a stream while using known `Stream`, `BinaryReader` and `BinaryWriter` class methods.  

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

### Usage as a ByteStream

While this library was originally optimised for the purpose of reading/writing bit-packed messages, it can also be used as a Byte Stream. For this purpose, the APIs `Read.*Aligned` are provided; which assume that `BitIndex is a multiple of 8`:  

```
|            Method |        Mean |     Error |    StdDev | Speed (MB/s) | Code Size | Allocated |
|------------------ |------------:|----------:|----------:|------------- |----------:|----------:|
|             Read8 | 12,834.8 ns |  77.08 ns |  64.37 ns |       779.13 |   1,548 B |         - |
|      Read8Aligned |  4,866.0 ns |  44.23 ns |  41.37 ns |      2055.09 |     297 B |         - |
|            Read16 | 12,430.5 ns |  54.80 ns |  48.58 ns |       804.47 |   2,990 B |         - |
|     Read16Aligned |  3,293.8 ns |  30.49 ns |  28.52 ns |      3036.03 |     502 B |         - |
| Read16AlignedFast |  2,633.4 ns |  18.13 ns |  16.96 ns |      3797.37 |     345 B |         - |
|            Read32 | 15,155.6 ns |  92.38 ns |  86.41 ns |       659.82 |   6,035 B |         - |
|     Read32Aligned |  2,577.6 ns |  14.90 ns |  13.94 ns |      3879.54 |     720 B |         - |
| Read32AlignedFast |  1,260.5 ns |   7.29 ns |   6.82 ns |      7933.57 |     313 B |         - |
|            Read64 | 14,497.0 ns | 118.25 ns | 110.61 ns |       689.80 |   7,567 B |         - |
|     Read64Aligned |  3,141.3 ns |  22.59 ns |  21.13 ns |      3183.41 |   1,669 B |         - |
| Read64AlignedFast |    630.6 ns |   4.72 ns |   4.41 ns |     15857.05 |     289 B |         - |
```

Example:  
```csharp
// [Assuming BitIndex aligned to start of a byte]
// Reads 4 bytes from the stream. 
bitStream.ReadAligned<uint>();

// Reads 4 bytes from the stream using optimised function (if available for the `IByteStream`).
bitStream.ReadAlignedFast<TStream, uint>();
```

Please note: Anything written to this byte stream is Big Endian.

#### Optimised Read/Write Functions

Optimised read/write functions are provided as extension methods for `IByteStreams` which support them.  

They are provided by the following interfaces.  
- `IStreamWithMemoryCopy`: Provides optimised aligned Read/Write operations for `Span<byte>`.  
- `IStreamWithReadBasicPrimitives`: Provides optimised Read/Write operations for 2/4/8 byte values.  

These are suffixed with `Fast`, so for `Write` you'd have `WriteFast`. 

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
If you want to just write structs without any bit packing whatsoever, you can use `ReadGeneric<T>` and `WriteGeneric<T>` in order to read/write unmanaged structures. 

```csharp
var expected = new TestStruct
{
    Byte = (byte) x,
    Short = (short) (byte.MaxValue + x),
    Int = short.MaxValue + x,
    Long = (long)int.MaxValue + x,
};

// Write struct to memory.
stream.WriteGeneric(ref expected);

// Seek and read the struct back.
stream.SeekRelative(-sizeof(TestStruct));
var actual = stream.ReadGeneric<TestStruct>();

// They should be equal.
Assert.Equal(expected, actual);
```

Under the hood these options use pointers and spans (without allocations), operating on these structs as if they were bytes.
