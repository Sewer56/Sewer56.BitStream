using System;
using Sewer56.BitStream.Interfaces;
using Sewer56.BitStream.Misc;

namespace Sewer56.BitStream.ByteStreams;

/// <summary>
/// A byte stream representing a <see cref="Memory{T}"/> instance.
/// </summary>
public readonly struct MemoryByteStream : IByteStream, IStreamWithMemoryCopy, IStreamWithReadBasicPrimitives
{
    public Memory<byte> Array { get; }
    public MemoryByteStream(Memory<byte> array) => Array = array;
    public byte Read(int index) => Array.Span[index];
    public void Write(byte value, int index) => Array.Span[index] = value;
    
    public void Read(Span<byte> data, int index) => Array.Span.SliceFast(index, data.Length).CopyTo(data);
    public void Write(Span<byte> value, int index) => value.CopyTo(Array.Span.SliceFast(index, value.Length));
    
    public ushort Read2(int index) => Array.Span.DangerousGetReferenceAtAs<byte, ushort>(index);
    public void Write2(ushort value, int index) => Array.Span.DangerousGetReferenceAtAs<byte, ushort>(index) = value;
    public uint Read4(int index) => Array.Span.DangerousGetReferenceAtAs<byte, uint>(index);
    public void Write4(uint value, int index) => Array.Span.DangerousGetReferenceAtAs<byte, uint>(index) = value;
    public ulong Read8(int index) => Array.Span.DangerousGetReferenceAtAs<byte, ulong>(index);
    public void Write8(ulong value, int index) => Array.Span.DangerousGetReferenceAtAs<byte, ulong>(index) = value;
}