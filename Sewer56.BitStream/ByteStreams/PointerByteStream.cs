using System;
using Sewer56.BitStream.Interfaces;

namespace Sewer56.BitStream.ByteStreams;

/// <summary>
/// A byte stream representing a native array.
/// </summary>
public readonly unsafe struct PointerByteStream : IByteStream, IStreamWithMemoryCopy, IStreamWithReadBasicPrimitives
{
    public byte* ArrayPtr { get; }
    public PointerByteStream(byte* arrayPtr) => ArrayPtr = arrayPtr;
    public byte Read(int index) => ArrayPtr[index];
    public void Write(byte value, int index) => ArrayPtr[index] = value;
    
    public void Read(Span<byte> data, int index) => new Span<byte>(ArrayPtr + index, data.Length).CopyTo(data);
    public void Write(Span<byte> value, int index) => value.CopyTo(new Span<byte>(ArrayPtr + index, value.Length));
    
    public ushort Read2(int index) => *(ushort*)(ArrayPtr + index);
    public void Write2(ushort value, int index) => *(ushort*)(ArrayPtr + index) = value;
    public uint Read4(int index) => *(uint*)(ArrayPtr + index);
    public void Write4(uint value, int index) => *(uint*)(ArrayPtr + index) = value;
    public ulong Read8(int index) => *(ulong*)(ArrayPtr + index);
    public void Write8(ulong value, int index) => *(ulong*)(ArrayPtr + index) = value;
}