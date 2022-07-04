using Sewer56.BitStream.Interfaces;

namespace Sewer56.BitStream.ByteStreams;

/// <summary>
/// A byte stream representing a native array.
/// </summary>
public readonly unsafe struct PointerByteStream : IByteStream
{
    public byte* ArrayPtr { get; }
    public PointerByteStream(byte* arrayPtr) => ArrayPtr = arrayPtr;
    public byte Read(int index) => ArrayPtr[index];
    public void Write(byte value, int index) => ArrayPtr[index] = value;
}