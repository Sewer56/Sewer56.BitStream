using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using Sewer56.BitStream.Interfaces;
using Sewer56.BitStream.Misc;
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
    /// <param name="bitStream">The bitstream, to read from.</param>
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

    #region IStreamWithReadBasicPrimitives

    /// <summary>
    /// Reads a specified type (up to 64 bits) from the stream.
    /// Number of bits determined from type.
    /// </summary>
    /// <typeparam name="T">Any of the following: byte, sbyte, short, ushort, int, uint, long, ulong.</typeparam>
    /// <typeparam name="TStream">Type of stream used in the bitstream.</typeparam>
    /// <remarks>Using this method has no additional overhead compared to the other methods in Release mode.</remarks>
    [MethodImpl(AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public static T ReadAlignedFast<TStream, T>(this ref BitStream<TStream> bitStream) where T : unmanaged 
        where TStream : IByteStream, IStreamWithReadBasicPrimitives
    {
        if (typeof(T) == typeof(byte)) return Number.Cast<T>(bitStream.Read8Aligned());
        else if (typeof(T) == typeof(sbyte)) return Number.Cast<T>(bitStream.Read8Aligned());

        else if (typeof(T) == typeof(short)) return Number.Cast<T>(bitStream.Read16AlignedFast());
        else if (typeof(T) == typeof(ushort)) return Number.Cast<T>(bitStream.Read16AlignedFast());

        else if (typeof(T) == typeof(int)) return Number.Cast<T>(bitStream.Read32AlignedFast());
        else if (typeof(T) == typeof(uint)) return Number.Cast<T>(bitStream.Read32AlignedFast());

        else if (typeof(T) == typeof(long)) return Number.Cast<T>(bitStream.Read64AlignedFast());
        else if (typeof(T) == typeof(ulong)) return Number.Cast<T>(bitStream.Read64AlignedFast());
#if DEBUG
        // Debug-only because exceptions prevent inlining.
        else throw new InvalidCastException();
#else
        else return default;
#endif
    }

    /// <summary>
    /// [ASSUMES ALIGNMENT. INCORRECT USE IF <see cref="BitStream{T}.BitIndex"/> IS NOT MULTIPLE OF 8]
    /// Reads a short from the stream.
    /// </summary>
    /// <returns>The read value.</returns>
    [MethodImpl(AggressiveInlining)]
    public static ushort Read16AlignedFast<T>(this ref BitStream<T> bitStream) where T : IByteStream, IStreamWithReadBasicPrimitives
    {
        int byteOffset = bitStream.BitIndex / BitStream<T>.ByteNumBits;
        var result = bitStream.Stream.Read2(byteOffset);
        bitStream.BitIndex += BitStream<T>.ShortNumBits;
        return BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(result) : result;
    }
    
    /// <summary>
    /// [ASSUMES ALIGNMENT. INCORRECT USE IF <see cref="BitStream{T}.BitIndex"/> IS NOT MULTIPLE OF 8]
    /// Reads a int from the stream.
    /// </summary>
    /// <returns>The read value.</returns>
    [MethodImpl(AggressiveInlining)]
    public static uint Read32AlignedFast<T>(this ref BitStream<T> bitStream) where T : IByteStream, IStreamWithReadBasicPrimitives
    {
        int byteOffset = bitStream.BitIndex / BitStream<T>.ByteNumBits;
        var result = bitStream.Stream.Read4(byteOffset);
        bitStream.BitIndex += BitStream<T>.IntNumBits;
        return BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(result) : result;
    }
    
    /// <summary>
    /// [ASSUMES ALIGNMENT. INCORRECT USE IF <see cref="BitStream{T}.BitIndex"/> IS NOT MULTIPLE OF 8]
    /// Reads a long from the stream.
    /// </summary>
    /// <returns>The read value.</returns>
    [MethodImpl(AggressiveInlining)]
    public static ulong Read64AlignedFast<T>(this ref BitStream<T> bitStream) where T : IByteStream, IStreamWithReadBasicPrimitives
    {
        int byteOffset = bitStream.BitIndex / BitStream<T>.ByteNumBits;
        var result = bitStream.Stream.Read8(byteOffset);
        bitStream.BitIndex += BitStream<T>.LongNumBits;
        return BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(result) : result;
    }

    /// <summary>
    /// [ASSUMES ALIGNMENT. INCORRECT USE IF <see cref="BitStream{T}.BitIndex"/> IS NOT MULTIPLE OF 8]
    /// Writes a specified type (up to 64 bits) to the stream.
    /// Number of bits determined from type.
    /// </summary>
    /// <typeparam name="T">Any of the following: byte, sbyte, short, ushort, int, uint, long, ulong.</typeparam>
    /// <typeparam name="TStream">The stream to write to.</typeparam>
    /// <param name="bitStream">The stream to write to./</param>
    /// <param name="value">The value to write.</param>
    /// <remarks>Using this method has no additional overhead compared to the other methods in Release mode.</remarks>
    [MethodImpl(AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public static void WriteAlignedFast<TStream, T>(this ref BitStream<TStream> bitStream, T value) where T : unmanaged 
        where TStream : IByteStream, IStreamWithReadBasicPrimitives
    {
        if (typeof(T) == typeof(byte)) bitStream.Write8Aligned(Number.Cast<T, byte>(value));
        else if (typeof(T) == typeof(sbyte)) bitStream.Write8Aligned(Number.Cast<T, byte>(value));

        else if (typeof(T) == typeof(short)) bitStream.Write16AlignedFast(Number.Cast<T, ushort>(value));
        else if (typeof(T) == typeof(ushort)) bitStream.Write16AlignedFast(Number.Cast<T, ushort>(value));

        else if (typeof(T) == typeof(int)) bitStream.Write32AlignedFast(Number.Cast<T, uint>(value));
        else if (typeof(T) == typeof(uint)) bitStream.Write32AlignedFast(Number.Cast<T, uint>(value));

        else if (typeof(T) == typeof(long)) bitStream.Write64AlignedFast(Number.Cast<T, ulong>(value));
        else if (typeof(T) == typeof(ulong)) bitStream.Write64AlignedFast(Number.Cast<T, ulong>(value));
#if DEBUG
        // Debug-only because exceptions prevent inlining.
        else throw new InvalidCastException();
#endif
    }

    /// <summary>
    /// [ASSUMES ALIGNMENT. INCORRECT USE IF <see cref="BitStream{T}.BitIndex"/> IS NOT MULTIPLE OF 8]
    /// Writes 2 bytes to the position starting at <see cref="BitStream{T}.BitIndex"/>.
    /// </summary>
    /// <param name="bitStream">The bitstream to write to.</param>
    /// <param name="value">Value to write.</param>
    [MethodImpl(AggressiveInlining)]
    public static void Write16AlignedFast<T>(this ref BitStream<T> bitStream, ushort value) 
        where T : IByteStream, IStreamWithReadBasicPrimitives
    {
        int localByteIndex = bitStream.BitIndex / BitStream<T>.ByteNumBits;
        bitStream.Stream.Write2(BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(value) : value, localByteIndex);
        bitStream.BitIndex += BitStream<T>.ShortNumBits;
    }
    
    /// <summary>
    /// [ASSUMES ALIGNMENT. INCORRECT USE IF <see cref="BitStream{T}.BitIndex"/> IS NOT MULTIPLE OF 8]
    /// Writes 4 bytes to the position starting at <see cref="BitStream{T}.BitIndex"/>.
    /// </summary>
    /// <param name="bitStream">The bitstream to write to.</param>
    /// <param name="value">Value to write.</param>
    [MethodImpl(AggressiveInlining)]
    public static void Write32AlignedFast<T>(this ref BitStream<T> bitStream, uint value) 
        where T : IByteStream, IStreamWithReadBasicPrimitives
    {
        int localByteIndex = bitStream.BitIndex / BitStream<T>.ByteNumBits;
        bitStream.Stream.Write4(BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(value) : value, localByteIndex);
        bitStream.BitIndex += BitStream<T>.IntNumBits;
    }
    
    /// <summary>
    /// [ASSUMES ALIGNMENT. INCORRECT USE IF <see cref="BitStream{T}.BitIndex"/> IS NOT MULTIPLE OF 8]
    /// Writes 8 bytes to the position starting at <see cref="BitStream{T}.BitIndex"/>.
    /// </summary>
    
    /// <param name="bitStream">The bitstream to write to.</param>
    /// <param name="value">Value to write.</param>
    [MethodImpl(AggressiveInlining)]
    public static void Write64AlignedFast<T>(this ref BitStream<T> bitStream, ulong value) 
        where T : IByteStream, IStreamWithReadBasicPrimitives
    {
        int localByteIndex = bitStream.BitIndex / BitStream<T>.ByteNumBits;
        bitStream.Stream.Write8(BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(value) : value, localByteIndex);
        bitStream.BitIndex += BitStream<T>.LongNumBits;
    }
    #endregion
}