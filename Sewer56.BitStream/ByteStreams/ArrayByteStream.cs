using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Sewer56.BitStream.Interfaces;
using Sewer56.BitStream.Misc;

namespace Sewer56.BitStream.ByteStreams;

/// <summary>
/// A byte stream representing a native array.
/// </summary>
public readonly struct ArrayByteStream : IByteStream, IStreamWithMemoryCopy, IStreamWithReadBasicPrimitives
{
    public byte[] Array { get; }
    public ArrayByteStream(byte[] array) => Array = array;
    public byte Read(int index) => Array[index];
    public void Write(byte value, int index) => Array[index] = value;
    
    public void Read(Span<byte> data, int index) => Array.AsSpanFast(index, data.Length).CopyTo(data);
    public void Write(Span<byte> value, int index) => value.CopyTo(Array.AsSpanFast(index, value.Length));
    
    public ushort Read2(int index) => Array.DangerousGetReferenceAtAs<byte, ushort>(index);
    public void Write2(ushort value, int index) => Array.DangerousGetReferenceAtAs<byte, ushort>(index) = value;
    public uint Read4(int index) => Array.DangerousGetReferenceAtAs<byte, uint>(index);
    public void Write4(uint value, int index) => Array.DangerousGetReferenceAtAs<byte, uint>(index) = value;
    public ulong Read8(int index) => Array.DangerousGetReferenceAtAs<byte, ulong>(index);
    public void Write8(ulong value, int index) => Array.DangerousGetReferenceAtAs<byte, ulong>(index) = value;
}