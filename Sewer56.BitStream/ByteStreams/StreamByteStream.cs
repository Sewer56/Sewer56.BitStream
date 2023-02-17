using System;
using System.IO;
using System.Runtime.CompilerServices;
using Sewer56.BitStream.Interfaces;
using Sewer56.BitStream.Misc;

namespace Sewer56.BitStream.ByteStreams;

/// <summary>
/// A byte stream representing a .NET Stream.
/// </summary>
public struct StreamByteStream : IByteStream 
#if NETCOREAPP3_1_OR_GREATER
    , IStreamWithMemoryCopy
#endif
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
    
#if NETCOREAPP3_1_OR_GREATER
    public void Read(Span<byte> data, int index)
    {
        Stream.Seek(index, SeekOrigin.Begin);
        TryReadAll(data);
    }

    public void Write(Span<byte> value, int index)
    {
        Stream.Seek(index, SeekOrigin.Begin);
        Stream.Write(value);
    }
    
    /// <summary>
    /// Reads a given number of bytes from a stream.
    /// </summary>
    /// <param name="result">The buffer to receive the bytes.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void TryReadAll(Span<byte> result)
    {
        int numBytesRead = 0;
        int numBytesToRead = result.Length;

        while (numBytesToRead > 0)
        {
            int bytesRead = Stream.Read(result.SliceFast(numBytesRead, numBytesToRead));
            if (bytesRead <= 0)
                return;

            numBytesRead += bytesRead;
            numBytesToRead -= bytesRead;
        }
    }
#endif
}