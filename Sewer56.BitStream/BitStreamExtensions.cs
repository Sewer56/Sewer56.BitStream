using System;
using System.Runtime.CompilerServices;
using Sewer56.BitStream.Interfaces;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace Sewer56.BitStream;

/// <summary>
/// Extension methods for the bitstream class with methods optimised for specific interface implementers.
/// </summary>
public static class BitStreamExtensions
{
    #region IStreamWithMemoryCopy
    /// <summary>
    /// Writes a <see cref="Span{T}"/> of bytes to the stream.
    /// </summary>
    /// <param name="bitStream">The bitstream to write the span array to.</param>
    /// <param name="value">The span to write.</param>
    /// <remarks>
    ///    Optimised function for implementers of <see cref="IStreamWithMemoryCopy"/>.
    /// </remarks>
    [MethodImpl(AggressiveInlining)]
    public static void WriteFast<T>(this ref BitStream<T> bitStream, Span<byte> value) where T : IByteStream, IStreamWithMemoryCopy
    {
        bool isAligned = bitStream.BitIndex % 8 == 0;
        if (isAligned)
        {
            if (value.Length >= 3)
            {
                bitStream.Stream.Write(value, bitStream.BitIndex / BitStream<T>.ByteNumBits);
                bitStream.BitIndex += value.Length * 8;
            }
            else
            {
                bitStream.WriteAligned(value);
            }
        }
        else
        {
            bitStream.WriteUnaligned(value);
        }
    }

    /// <summary>
    /// Reads a given number of bytes into a <see cref="Span{T}"/> buffer.
    /// </summary>
    /// <param name="bitStream">The bitstream, tp read from.</param>
    /// <param name="buffer">Span to write to.</param>
    [MethodImpl(AggressiveInlining)]
    public static void ReadFast<T>(this ref BitStream<T> bitStream, Span<byte> buffer) where T : IByteStream, IStreamWithMemoryCopy
    {
        bool isAligned = bitStream.BitIndex % 8 == 0;

        if (isAligned)
        {
            if (buffer.Length >= 3)
            {
                bitStream.Stream.Read(buffer, bitStream.BitIndex / BitStream<T>.ByteNumBits);
                bitStream.BitIndex += buffer.Length * 8;
            }
            else
            {
                bitStream.ReadAligned(buffer);
            }
        }
        else
        {
            bitStream.ReadUnaligned(buffer);
        }
    }
    #endregion
}