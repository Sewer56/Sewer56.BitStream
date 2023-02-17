using System;
using System.Runtime.CompilerServices;
using Sewer56.BitStream.Interfaces;
using Sewer56.BitStream.Misc;

namespace Sewer56.BitStream.ByteStreams;

/// <summary>
/// A byte stream representing a native array.
/// </summary>
public readonly unsafe struct PointerByteStream : IByteStream, IStreamWithMemoryCopy
{
    public byte* ArrayPtr { get; }
    public PointerByteStream(byte* arrayPtr) => ArrayPtr = arrayPtr;
    public byte Read(int index) => ArrayPtr[index];
    public void Write(byte value, int index) => ArrayPtr[index] = value;
    
    public void Read(Span<byte> data, int index) => new Span<byte>(ArrayPtr + index, data.Length).CopyTo(data);
    public void Write(Span<byte> value, int index) => value.CopyTo(new Span<byte>(ArrayPtr + index, value.Length));
}