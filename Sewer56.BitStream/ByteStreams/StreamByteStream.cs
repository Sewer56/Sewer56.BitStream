using System.IO;
using Sewer56.BitStream.Interfaces;

namespace Sewer56.BitStream.ByteStreams;

/// <summary>
/// A byte stream representing a .NET Stream.
/// </summary>
public struct StreamByteStream : IByteStream
{
    public Stream Stream { get; }
    public StreamByteStream(Stream stream) => Stream = stream;
    public byte Read(int index)
    {
        Stream.Seek(index, SeekOrigin.Begin);
        return (byte) Stream.ReadByte();
    }

    public void Write(byte value, int index)
    {
        Stream.Seek(index, SeekOrigin.Begin);
        Stream.WriteByte(value);
    }
}