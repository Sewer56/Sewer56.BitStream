using System;
using Sewer56.BitStream.Interfaces;
using Sewer56.BitStream.Misc;

namespace Sewer56.BitStream.ByteStreams;

/// <summary>
/// A byte stream representing a <see cref="Memory{T}"/> instance.
/// </summary>
public readonly struct MemoryByteStream : IByteStream, IStreamWithMemoryCopy
{
    public Memory<byte> Array { get; }
    public MemoryByteStream(Memory<byte> array) => Array = array;
    public byte Read(int index) => Array.Span[index];
    public void Write(byte value, int index) => Array.Span[index] = value;
    
    public void Read(Span<byte> data, int index) => Array.Span.SliceFast(index, data.Length).CopyTo(data);
    public void Write(Span<byte> value, int index) => value.CopyTo(Array.Span.SliceFast(index, value.Length));
}