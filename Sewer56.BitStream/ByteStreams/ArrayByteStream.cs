using System;
using Sewer56.BitStream.Interfaces;
using Sewer56.BitStream.Misc;

namespace Sewer56.BitStream.ByteStreams;

/// <summary>
/// A byte stream representing a native array.
/// </summary>
public readonly struct ArrayByteStream : IByteStream, IStreamWithMemoryCopy
{
    public byte[] Array { get; }
    public ArrayByteStream(byte[] array) => Array = array;
    public byte Read(int index) => Array[index];
    public void Write(byte value, int index) => Array[index] = value;
    
    public void Read(Span<byte> data, int index) => Array.AsSpanFast(index, data.Length).CopyTo(data);
    public void Write(Span<byte> value, int index) => value.CopyTo(Array.AsSpanFast(index, value.Length));
}