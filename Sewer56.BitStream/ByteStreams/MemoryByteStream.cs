using System;
using Sewer56.BitStream.Interfaces;

namespace Sewer56.BitStream.ByteStreams;

/// <summary>
/// A byte stream representing a <see cref="Memory{T}"/> instance.
/// </summary>
public readonly struct MemoryByteStream : IByteStream
{
    public Memory<byte> Array { get; }
    public MemoryByteStream(Memory<byte> array) => Array = array;
    public byte Read(int index) => Array.Span[index];
    public void Write(byte value, int index) => Array.Span[index] = value;
}