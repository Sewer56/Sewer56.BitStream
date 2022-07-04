using Sewer56.BitStream.Interfaces;

namespace Sewer56.BitStream.ByteStreams;

/// <summary>
/// A byte stream representing a native array.
/// </summary>
public readonly struct ArrayByteStream : IByteStream
{
    public byte[] Array { get; }
    public ArrayByteStream(byte[] array) => Array = array;
    public byte Read(int index) => Array[index];
    public void Write(byte value, int index) => Array[index] = value;
}