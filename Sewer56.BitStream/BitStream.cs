using System;
using Sewer56.BitStream.Interfaces;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Sewer56.BitStream.Misc;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace Sewer56.BitStream
{
    /// <summary>
    /// High performance stack based reader and writer of a stream of bits which allows for 
    /// accessing the bits regardless of their size (up to 64 bits), even if the accesses cross
    /// one or more byte boundaries. This can be used to efficiently pack bits such that
    /// individual values can be stored in less than one byte.
    ///
    /// Inspired and improved upon blog article `Sub-Byte Size` by Jackson Dunstan
    /// <a href="https://www.jacksondunstan.com/articles/5426"/>
    /// </summary>
    public unsafe struct BitStream<TByteStream> where TByteStream : IByteStream
    {
        /*
            Notes:
            
            A.

            The type constraint above is very important.

            It saves us a virtual method call, as the read function on the Stream is used
            directly (and possibly even inlined) as opposed to using the interface.
        
            B.
        
            Comparisons of constants against constants e.g. Read<int>(4) 
            are optimized out by the JIT in Release mode. The checks/branches do not
            exist after JIT-ting if the parameter value is known at JIT-time.
        */

        private const int ByteNumBits = sizeof(byte) * 8;
        private const int ShortNumBits = sizeof(short) * 8;
        private const int IntNumBits = sizeof(int) * 8;
        private const int LongNumBits = sizeof(long) * 8;

        /// <summary>
        /// Absolute index of the next bit to access.
        /// </summary>
        public int BitIndex { get; set; }

        /// <summary>
        /// The current byte offset.
        /// </summary>
        public int ByteOffset => BitIndex / ByteNumBits;

        /// <summary>
        /// The current bit offset.
        /// </summary>
        public int BitOffset => BitIndex % ByteNumBits;

        /// <summary>
        /// Returns the index of the next byte to be read/written.
        /// If bit offset within the current byte is 0, this value is equal to <see cref="BitIndex"/> % ByteNumBits
        /// Else the value is <see cref="BitIndex"/> + 1.
        /// </summary>
        public int NextByteIndex
        {
            get
            {
                var bitIndex = BitIndex;
                int byteOffset = bitIndex / ByteNumBits;
                int bitOffset = bitIndex % ByteNumBits;
                bool extraByte = bitOffset != 0;
                return byteOffset + Unsafe.As<bool, byte>(ref extraByte); // Branchless
            }
        }

        /// <summary>
        /// The stream to read from.
        /// </summary>
        public TByteStream Stream { get; private set; }

        /// <summary>
        /// Constructs a new instance of the BitStream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="bitIndex">The bit index.</param>
        public BitStream(TByteStream stream, int bitIndex = default)
        {
            BitIndex = bitIndex;
            Stream = stream;
        }

        /// <summary>
        /// Reads a single bit from the stream.
        /// </summary>
        /// <returns>The read value, stored in the least-significant bits.</returns>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
#if NETCOREAPP
        [MethodImpl(AggressiveOptimization)]
#endif
        public byte ReadBit()
        {
            const int bitCount = 1;
            const uint mask = 0b01;

            // Calculate where we are in the stream and advance.
            int bitIndex   = BitIndex;
            int byteOffset = bitIndex / ByteNumBits;
            int bitOffset  = bitIndex % ByteNumBits;
            BitIndex += bitCount;

            // Read the byte containing the bit we want and mask.
            byte highByte = Stream.Read(byteOffset);
            int highBitsAfterRead = ByteNumBits - bitOffset - bitCount;
            uint highMask = mask << highBitsAfterRead;
            return (byte)((highByte & highMask) >> highBitsAfterRead);
        }

        /// <summary>
        /// Read up to 8 bits starting at <see cref="BitIndex"/>.
        /// </summary>
        /// <param name="numBits">Number of bits to read.</param>
        /// <returns>The read value, stored in the least-significant bits.</returns>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
#if NETCOREAPP
        [MethodImpl(AggressiveOptimization)]
#endif
        public byte Read8(int numBits)
        {
            // Calculate where we are in the stream and advance.
            int bitIndex = BitIndex;
            int byteOffset = bitIndex / ByteNumBits;
            int bitOffset = bitIndex % ByteNumBits;
            BitIndex = bitIndex + numBits;

            // Read the byte with the high bits (first byte) and decide if that's the only byte.
            byte highByte = Stream.Read(byteOffset);
            int highBitsAvailable = ByteNumBits - bitOffset;
            int highBitsAfterRead = highBitsAvailable - numBits;
            uint highMask;

            if (highBitsAfterRead >= 0)
            {
                highMask = GetMask(numBits) << highBitsAfterRead;
                return (byte)((highByte & highMask) >> highBitsAfterRead);
            }

            // Read the low byte and combine with the high byte.
            highMask = GetMask(highBitsAvailable);

            int numLowBits = numBits - highBitsAvailable;
            int lowShift = ByteNumBits - numLowBits;
            uint lowMask = GetMask(numLowBits) << lowShift;

            byte lowByte = Stream.Read(byteOffset + 1);
            uint highPart = (highByte & highMask) << numLowBits;
            uint lowPart = (lowByte & lowMask) >> lowShift;
            return (byte)(highPart | lowPart);
        }

        /// <summary>
        /// Read up to 16 bits starting at <see cref="BitIndex"/>.
        /// </summary>
        /// <param name="numBits">Number of bits to read.</param>
        /// <returns>The read value, stored in the least-significant bits.</returns>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
#if NETCOREAPP
        [MethodImpl(AggressiveInlining | AggressiveOptimization)]
#else
        [MethodImpl(AggressiveInlining)]
#endif
        public ushort Read16(int numBits)
        {
            if (numBits <= ByteNumBits)
                return Read8(numBits);

            byte high = Read8(ByteNumBits);
            int numLowBits = numBits - ByteNumBits;
            byte low = Read8(numLowBits);
            return (ushort)((high << numLowBits) | low);
        }

        /// <summary>
        /// Read up to 32 bits starting at <see cref="BitIndex"/>.
        /// </summary>
        /// <param name="numBits">Number of bits to read.</param>
        /// <returns>The read value, stored in the least-significant bits.</returns>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
#if NETCOREAPP
        [MethodImpl(AggressiveInlining | AggressiveOptimization)]
#else
        [MethodImpl(AggressiveInlining)]
#endif
        public uint Read32(int numBits)
        {
            if (numBits <= ShortNumBits)
                return Read16(numBits);

            uint high = Read16(ShortNumBits);
            int numLowBits = numBits - ShortNumBits;
            uint low = Read16(numLowBits);
            return (high << numLowBits) | low;
        }

        /// <summary>
        /// Read up to 64 bits starting at <see cref="BitIndex"/>.
        /// </summary>
        /// <param name="numBits">Number of bits to read.</param>
        /// <returns>The read value, stored in the least-significant bits.</returns>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
#if NETCOREAPP
        [MethodImpl(AggressiveInlining | AggressiveOptimization)]
#else
        [MethodImpl(AggressiveInlining)]
#endif
        public ulong Read64(int numBits)
        {
            if (numBits <= IntNumBits)
                return Read32(numBits);

            ulong high = Read32(IntNumBits);
            int numLowBits = numBits - IntNumBits;
            ulong low = Read32(numLowBits);
            return (high << numLowBits) | low;
        }

        /// <summary>
        /// Reads a specified type (up to 64 bits) from the stream.
        /// </summary>
        /// <typeparam name="T">Any of the following: byte, sbyte, short, ushort, int, uint, long, ulong.</typeparam>
        /// <param name="numBits">Number of bits to read. Max: 64.</param>
        /// <remarks>Using this method has no additional overhead compared to the other methods in Release mode.</remarks>
        [MethodImpl(AggressiveInlining)]
        public T Read<T>(int numBits)
        {
            if (typeof(T) == typeof(byte)) return Number.Cast<T>(Read8(numBits));
            else if (typeof(T) == typeof(sbyte)) return Number.Cast<T>(Read8(numBits));

            else if (typeof(T) == typeof(short)) return Number.Cast<T>(Read16(numBits));
            else if (typeof(T) == typeof(ushort)) return Number.Cast<T>(Read16(numBits));

            else if (typeof(T) == typeof(int)) return Number.Cast<T>(Read32(numBits));
            else if (typeof(T) == typeof(uint)) return Number.Cast<T>(Read32(numBits));

            else if (typeof(T) == typeof(long)) return Number.Cast<T>(Read64(numBits));
            else if (typeof(T) == typeof(ulong)) return Number.Cast<T>(Read64(numBits));

#if DEBUG
            // Debug-only because exceptions prevent inlining.
            else throw new InvalidCastException();
#else
            else return default;
#endif
        }

        /// <summary>
        /// Reads a specified type (up to 64 bits) from the stream.
        /// Number of bits determined from type.
        /// </summary>
        /// <typeparam name="T">Any of the following: byte, sbyte, short, ushort, int, uint, long, ulong.</typeparam>
        /// <remarks>Using this method has no additional overhead compared to the other methods in Release mode.</remarks>
        [MethodImpl(AggressiveInlining)]
        public T Read<T>() where T : unmanaged
        {
            if (typeof(T) == typeof(byte)) return Number.Cast<T>(Read8(sizeof(T) * 8));
            else if (typeof(T) == typeof(sbyte)) return Number.Cast<T>(Read8(sizeof(T) * 8));

            else if (typeof(T) == typeof(short)) return Number.Cast<T>(Read16(sizeof(T) * 8));
            else if (typeof(T) == typeof(ushort)) return Number.Cast<T>(Read16(sizeof(T) * 8));

            else if (typeof(T) == typeof(int)) return Number.Cast<T>(Read32(sizeof(T) * 8));
            else if (typeof(T) == typeof(uint)) return Number.Cast<T>(Read32(sizeof(T) * 8));

            else if (typeof(T) == typeof(long)) return Number.Cast<T>(Read64(sizeof(T) * 8));
            else if (typeof(T) == typeof(ulong)) return Number.Cast<T>(Read64(sizeof(T) * 8));
#if DEBUG
            // Debug-only because exceptions prevent inlining.
            else throw new InvalidCastException();
#else
            else return default;
#endif
        }

        /// <summary>
        /// Writes a single bit starting at <see cref="BitIndex"/>.
        /// </summary>
        /// <param name="value">Value to write.</param>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
        public void WriteBit(byte value)
        {
            const int numBits = 1;
            const int singleBitMask = 0b01;

            // Calculate where we are in the stream and advance.
            int bitIndex = BitIndex;
            int localByteIndex = bitIndex / ByteNumBits;
            int localBitIndex = bitIndex % ByteNumBits;
            BitIndex = bitIndex + numBits;

            // Read the first byte, mix with our value and write back.
            byte firstByte = Stream.Read(localByteIndex);
            int firstBitsAvailable = ByteNumBits - localBitIndex;
            int firstBitsAfterWrite = firstBitsAvailable - numBits;
            var firstMask = (GetMask(localBitIndex) << firstBitsAvailable) | GetMask(firstBitsAfterWrite); // Masks out bits except those to write.
            uint valueMask = singleBitMask;
            Stream.Write((byte)((firstByte & firstMask) | ((value & valueMask) << firstBitsAfterWrite)), localByteIndex);
        }

        /// <summary>
        /// Write up to 8 bits starting at <see cref="BitIndex"/>.
        /// </summary>
        /// <param name="value">Value to write.</param>
        /// <param name="numBits">Number of bits to write.</param>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
        public void Write8(byte value, int numBits)
        {
            // Calculate where we are in the stream and advance.
            int bitIndex = BitIndex;
            int localByteIndex = bitIndex / ByteNumBits;
            int localBitIndex = bitIndex % ByteNumBits;
            BitIndex = bitIndex + numBits;

            // When overwriting a whole byte, there's no need to read existing
            // bytes and mix in the written bits.
            if (localBitIndex == 0 && numBits == ByteNumBits)
            {
                Stream.Write(value, localByteIndex);
                return;
            }

            // Read the first byte and decide if that's the only byte.
            byte firstByte = Stream.Read(localByteIndex);
            int firstBitsAvailable = ByteNumBits - localBitIndex;
            int firstBitsAfterWrite = firstBitsAvailable - numBits;
            uint valueMask;
            uint firstMask; // Masks out bits except those to write.
            if (firstBitsAfterWrite >= 0)
            {
                firstMask = (GetMask(localBitIndex) << firstBitsAvailable) | GetMask(firstBitsAfterWrite);
                valueMask = GetMask(numBits);
                Stream.Write((byte)((firstByte & firstMask) | ((value & valueMask) << firstBitsAfterWrite)), localByteIndex);
                return;
            }

            // Combine the high bits of the value to write with the high bits of the first byte and write.
            int numLowBits = numBits - firstBitsAvailable;
            valueMask = GetMask(firstBitsAvailable);
            firstMask = ~valueMask;
            Stream.Write((byte)((firstByte & firstMask) | (value >> numLowBits) & valueMask), localByteIndex);

            // Read the second byte and combine its low bits with the low bits of the value to write.
            byte secondByte = Stream.Read(localByteIndex + 1);
            
            valueMask = GetMask(numLowBits);
            int numSecond = ByteNumBits - numLowBits;
            uint secondMask = GetMask(numSecond);
            Stream.Write((byte)(((value & valueMask) << numSecond) | (secondByte & secondMask)), localByteIndex + 1);
        }

        /// <summary>
        /// Write up to 16 bits starting at <see cref="BitIndex"/>.
        /// </summary>
        /// <param name="value">Value to write.</param>
        /// <param name="numBits">Number of bits to write.</param>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
        public void Write16(ushort value, int numBits)
        {
            if (numBits <= ByteNumBits)
            {
                Write8((byte)value, numBits);
                return;
            }

            int numLow = numBits - ByteNumBits;
            Write8((byte)((value & (byte.MaxValue << numLow)) >> numLow), ByteNumBits);
            Write8((byte)(value & GetMask(numLow)), numLow);
        }

        /// <summary>
        /// Write up to 32 bits starting at <see cref="BitIndex"/>.
        /// </summary>
        /// <param name="value">Value to write.</param>
        /// <param name="numBits">Number of bits to write.</param>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
        public void Write32(uint value, int numBits)
        {
            if (numBits <= ShortNumBits)
            {
                Write16((ushort)value, numBits);
                return;
            }

            int numLow = numBits - ShortNumBits;
            Write16((ushort)((value & (ushort.MaxValue << numLow)) >> numLow), ShortNumBits);
            Write16((ushort)(value & GetMask(numLow)), numLow);
        }

        /// <summary>
        /// Write up to 64 bits starting at <see cref="BitIndex"/>.
        /// </summary>
        /// <param name="value">Value to write.</param>
        /// <param name="numBits">Number of bits to write.</param>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
        public void Write64(ulong value, int numBits)
        {
            if (numBits <= IntNumBits)
            {
                Write32((uint)value, numBits);
                return;
            }

            int numLow = numBits - IntNumBits;
            Write32((uint)((value & (uint.MaxValue << numLow)) >> numLow), IntNumBits);
            Write32((uint)(value & GetMaskLong(numLow)), numLow);
        }

        /// <summary>
        /// Writes a specified type (up to 64 bits) to the stream.
        /// </summary>
        /// <typeparam name="T">Any of the following: byte, sbyte, short, ushort, int, uint, long, ulong.</typeparam>
        /// <param name="value">The value to write.</param>
        /// <param name="numBits">Number of bits to write.</param>
        /// <remarks>Using this method has no additional overhead compared to the other methods in Release mode.</remarks>
        [MethodImpl(AggressiveInlining)]
        public void Write<T>(T value, int numBits)
        {
            if (typeof(T) == typeof(byte)) Write8(Number.Cast<T, byte>(value), numBits);
            else if (typeof(T) == typeof(sbyte)) Write8(Number.Cast<T, byte>(value), numBits);

            else if (typeof(T) == typeof(short)) Write16(Number.Cast<T, ushort>(value), numBits);
            else if (typeof(T) == typeof(ushort)) Write16(Number.Cast<T, ushort>(value), numBits);

            else if (typeof(T) == typeof(int)) Write32(Number.Cast<T, uint>(value), numBits);
            else if (typeof(T) == typeof(uint)) Write32(Number.Cast<T, uint>(value), numBits);

            else if (typeof(T) == typeof(long)) Write64(Number.Cast<T, ulong>(value), numBits);
            else if (typeof(T) == typeof(ulong)) Write64(Number.Cast<T, ulong>(value), numBits);
#if DEBUG
            // Debug-only because exceptions prevent inlining.
            else throw new InvalidCastException();
#endif
        }

        /// <summary>
        /// Writes a specified type (up to 64 bits) to the stream.
        /// Number of bits determined from type.
        /// </summary>
        /// <typeparam name="T">Any of the following: byte, sbyte, short, ushort, int, uint, long, ulong.</typeparam>
        /// <param name="value">The value to write.</param>
        /// <remarks>Using this method has no additional overhead compared to the other methods in Release mode.</remarks>
        [MethodImpl(AggressiveInlining)]
        public void Write<T>(T value) where T : unmanaged
        {
            if (typeof(T) == typeof(byte)) Write8(Number.Cast<T, byte>(value), sizeof(T) * 8);
            else if (typeof(T) == typeof(sbyte)) Write8(Number.Cast<T, byte>(value), sizeof(T) * 8);

            else if (typeof(T) == typeof(short)) Write16(Number.Cast<T, ushort>(value), sizeof(T) * 8);
            else if (typeof(T) == typeof(ushort)) Write16(Number.Cast<T, ushort>(value), sizeof(T) * 8);

            else if (typeof(T) == typeof(int)) Write32(Number.Cast<T, uint>(value), sizeof(T) * 8);
            else if (typeof(T) == typeof(uint)) Write32(Number.Cast<T, uint>(value), sizeof(T) * 8);

            else if (typeof(T) == typeof(long)) Write64(Number.Cast<T, ulong>(value), sizeof(T) * 8);
            else if (typeof(T) == typeof(ulong)) Write64(Number.Cast<T, ulong>(value), sizeof(T) * 8);
#if DEBUG
            // Debug-only because exceptions prevent inlining.
            else throw new InvalidCastException();
#endif
        }

        #region Extensions

        /// <summary>
        /// Seeks the stream to a specified location.
        /// </summary>
        /// <param name="offset">The byte offset.</param>
        /// <param name="bit">The bit value to add to the byte offset.</param>
        public void Seek(int offset, byte bit = 0) => BitIndex = (offset * ByteNumBits) + bit;

        /// <summary>
        /// Seeks the stream to a specified location.
        /// </summary>
        /// <param name="offset">The byte offset.</param>
        /// <param name="bit">The bit value to add to the byte offset.</param>
        public void SeekRelative(int offset = 0, byte bit = 0) => BitIndex += (offset * ByteNumBits) + bit;

        /// <summary>
        /// Reads an unmanaged struct from the current <see cref="BitStream"/>.
        /// </summary>
        /// <typeparam name="T">The type of value to be read from the stream.</typeparam>
        /// <returns>The read in struct.</returns>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
#if NETCOREAPP
        [MethodImpl(AggressiveOptimization)]
#endif
        public T ReadGeneric<T>() where T : unmanaged
        {
            Span<byte> stackSpan = stackalloc byte[sizeof(T)];
            for (int x = 0; x < stackSpan.Length; x++)
                stackSpan[x] = Read<byte>(ByteNumBits);

            return Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(stackSpan));
        }

        /// <summary>
        /// Reads an unmanaged struct from the current <see cref="BitStream"/>.
        /// </summary>
        /// <typeparam name="T">The type of value to be read from the stream.</typeparam>
        /// <returns>The read in struct.</returns>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
#if NETCOREAPP
        [MethodImpl(AggressiveOptimization)]
#endif
        public T ReadGeneric<T>(int numBits) where T : unmanaged
        {
            Span<byte> stackSpan = stackalloc byte[sizeof(T)];
            int remainingBits = numBits;

            for (int x = 0; x < stackSpan.Length; x++)
            {
                if (remainingBits < ByteNumBits)
                {
                    stackSpan[x] = Read<byte>(remainingBits);
                    break;
                }

                stackSpan[x] = Read<byte>(ByteNumBits);
                remainingBits -= ByteNumBits;
            }

            return Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(stackSpan));
        }

        /// <summary>
        /// Reads an unmanaged struct from the current <see cref="BitStream"/>.
        /// </summary>
        /// <typeparam name="T">The type of value to be read from the stream.</typeparam>
        /// <param name="value">The value read from the stream.</param>
        [MethodImpl(AggressiveInlining)]
        public void ReadGeneric<T>(out T value) where T : unmanaged => value = ReadGeneric<T>();

        /// <summary>
        /// Appends an unmanaged struct to the current <see cref="BitStream"/>.
        /// </summary>
        /// <typeparam name="T">The type of value to be written onto the stream.</typeparam>
        /// <param name="value">The value to write to the stream.</param>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
#if NETCOREAPP
        [MethodImpl(AggressiveOptimization)]
#endif
        public void WriteGeneric<T>(ref T value) where T : unmanaged
        {
            var valuePtr = (byte*)(Unsafe.AsPointer(ref value));
            for (int x = 0; x < sizeof(T); x++)
                Write<byte>(valuePtr[x], ByteNumBits);
        }

        /// <summary>
        /// Appends an unmanaged struct to the current <see cref="BitStream"/>.
        /// </summary>
        /// <typeparam name="T">The type of value to be written onto the stream.</typeparam>
        /// <param name="value">The value to write to the stream.</param>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
#if NETCOREAPP
        [MethodImpl(AggressiveOptimization)]
#endif
        public void WriteGeneric<T>(T value) where T : unmanaged
        {
            var valuePtr = (byte*)(Unsafe.AsPointer(ref value));
            for (int x = 0; x < sizeof(T); x++)
                Write<byte>(valuePtr[x], ByteNumBits);
        }

        /// <summary>
        /// Appends an unmanaged struct to the current <see cref="BitStream"/>.
        /// </summary>
        /// <typeparam name="T">The type of value to be written onto the stream.</typeparam>
        /// <param name="value">The value to write to the stream.</param>
        /// <param name="numBits">Number of bits to write to the stream.</param>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
#if NETCOREAPP
        [MethodImpl(AggressiveOptimization)]
#endif
        public void WriteGeneric<T>(ref T value, int numBits) where T : unmanaged
        {
            var valuePtr = (byte*)(Unsafe.AsPointer(ref value));
            int remainingBits = numBits;

            for (int x = 0; x < sizeof(T); x++)
            {
                if (remainingBits < ByteNumBits)
                {
                    Write<byte>(valuePtr[x], remainingBits);
                    break;
                }

                Write<byte>(valuePtr[x], ByteNumBits);
                remainingBits -= ByteNumBits;
            }
        }

        /// <summary>
        /// Appends an unmanaged struct to the current <see cref="BitStream"/>.
        /// </summary>
        /// <typeparam name="T">The type of value to be written onto the stream.</typeparam>
        /// <param name="value">The value to write to the stream.</param>
        /// <param name="numBits">Number of bits to write to the stream.</param>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
#if NETCOREAPP
        [MethodImpl(AggressiveOptimization)]
#endif
        public void WriteGeneric<T>(T value, int numBits) where T : unmanaged
        {
            var valuePtr = (byte*)(Unsafe.AsPointer(ref value));
            int remainingBits = numBits;

            for (int x = 0; x < sizeof(T); x++)
            {
                if (remainingBits < ByteNumBits)
                {
                    Write<byte>(valuePtr[x], remainingBits);
                    break;
                }

                Write<byte>(valuePtr[x], ByteNumBits);
                remainingBits -= ByteNumBits;
            }
        }

        /// <summary>
        /// Writes a seriablizable structure (see: <see cref="IBitPackable{TSelf}"/> to the stream.
        /// </summary>
        /// <typeparam name="T">Any type implementing <see cref="IBitPackable{TSelf}"/>.</typeparam>
        /// <param name="value">The value to write.</param>
        [MethodImpl(AggressiveInlining)]
        public void Serialize<T>(ref T value) where T : IBitPackable<T>, new() => value.ToStream(ref this);

        /// <summary>
        /// Reads a seriablizable structure (see: <see cref="IBitPackable{TSelf}"/> from the stream.
        /// </summary>
        /// <typeparam name="T">Any type implementing <see cref="IBitPackable{TSelf}"/>.</typeparam>
        [MethodImpl(AggressiveInlining)]
        public T Deserialize<T>() where T : IBitPackable<T>, new() => new T().FromStream(ref this);

        /// <summary>
        /// Writes a <see cref="Span{T}"/> of bytes to the stream.
        /// </summary>
        /// <param name="value">The span to write.</param>
        [MethodImpl(AggressiveInlining)]
        public void Write(Span<byte> value)
        {
            for (int x = 0; x < value.Length; x++)
                Write8(value[x], ByteNumBits);
        }

        /// <summary>
        /// Reads a given number of bytes into a <see cref="Span{T}"/> buffer.
        /// </summary>
        /// <param name="buffer">Span to write to.</param>
        [MethodImpl(AggressiveInlining)]
        public void Read(Span<byte> buffer)
        {
            for (int x = 0; x < buffer.Length; x++)
                buffer[x] = Read8(ByteNumBits);
        }

        /// <summary>
        /// Reads a specified signed type (up to 64 bits) from the stream.
        /// </summary>
        /// <typeparam name="T">Any of the following: byte, sbyte, short, ushort, int, uint, long, ulong.</typeparam>
        /// <param name="numBits">Number of bits to read. Max: 64.</param>
        /// <remarks>Using this method has no additional overhead compared to the other methods in Release mode.</remarks>
        [MethodImpl(AggressiveInlining)]
        public T ReadSigned<T>(int numBits)
        {
            if (typeof(T) == typeof(long)) return Number.Cast<T>(SignExtend((long)Number.Cast<T, ulong>(Read<T>(numBits)), numBits));
            else if (typeof(T) == typeof(ulong)) return Number.Cast<T>(SignExtend((long) Number.Cast<T, ulong>(Read<T>(numBits)), numBits));

            else if (typeof(T) == typeof(uint)) return Number.Cast<T>(SignExtend(Number.Cast<T, uint>(Read<T>(numBits)), numBits));
            else if (typeof(T) == typeof(int)) return Number.Cast<T>(SignExtend(Number.Cast<T, int>(Read<T>(numBits)), numBits));

            else if (typeof(T) == typeof(short)) return Number.Cast<T>(SignExtend(Number.Cast<T, short>(Read<T>(numBits)), numBits));
            else if (typeof(T) == typeof(ushort)) return Number.Cast<T>(SignExtend(Number.Cast<T, ushort>(Read<T>(numBits)), numBits));
            else if (typeof(T) == typeof(byte)) return Number.Cast<T>(SignExtend(Number.Cast<T, byte>(Read<T>(numBits)), numBits));
            else if (typeof(T) == typeof(sbyte)) return Number.Cast<T>(SignExtend(Number.Cast<T, sbyte>(Read<T>(numBits)), numBits));

#if DEBUG
            // Debug-only because exceptions prevent inlining.
            else throw new InvalidCastException();
#else
            else return default;
#endif
        }

        /// <summary>
        /// Writes a specified signed type (up to 64 bits) to the stream.
        /// Number of bits determined from type.
        /// </summary>
        /// <typeparam name="T">Any of the following: byte, sbyte, short, ushort, int, uint, long, ulong.</typeparam>
        /// <param name="value">The value to write.</param>
        /// <param name="numBits">Number of bits to write.</param>
        /// <remarks>Using this method has no additional overhead compared to the other methods in Release mode.</remarks>
        [MethodImpl(AggressiveInlining)]
        public void WriteSigned<T>(T value, int numBits) where T : unmanaged
        {
            if (typeof(T) == typeof(byte)) Write8((byte) SignShrink(Number.Cast<T, byte>(value)), numBits);
            else if (typeof(T) == typeof(sbyte)) Write8((byte) SignShrink(Number.Cast<T, byte>(value)), numBits);

            else if (typeof(T) == typeof(short)) Write16((ushort) SignShrink(Number.Cast<T, ushort>(value)), numBits);
            else if (typeof(T) == typeof(ushort)) Write16((ushort) SignShrink(Number.Cast<T, ushort>(value)), numBits);

            else if (typeof(T) == typeof(int)) Write32((uint) SignShrink((int)Number.Cast<T, uint>(value)), numBits);
            else if (typeof(T) == typeof(uint)) Write32((uint) SignShrink((int)Number.Cast<T, uint>(value)), numBits);

            else if (typeof(T) == typeof(long)) Write64((ulong) SignShrink((long)Number.Cast<T, ulong>(value)), numBits);
            else if (typeof(T) == typeof(ulong)) Write64((ulong) SignShrink((long)Number.Cast<T, ulong>(value)), numBits);
#if DEBUG
            // Debug-only because exceptions prevent inlining.
            else throw new InvalidCastException();
#endif
        }

        /// <summary>
        /// Reads a null terminated string from a given stream.
        /// </summary>
        /// <param name="maxLengthBytes">Maximum length in bytes.</param>
        /// <param name="encoding">The encoding to use.</param>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
        public string ReadString(int maxLengthBytes = 1024, System.Text.Encoding encoding = null)
        {
            encoding ??= System.Text.Encoding.UTF8;

#if NETCOREAPP
            Span<byte> span = stackalloc byte[maxLengthBytes];
#else
            byte* ptr = stackalloc byte[maxLengthBytes];
            Span<byte> span = new Span<byte>(ptr, maxLengthBytes);
#endif

            int length = 0;
            byte currentCharacter;
            do
            {
                currentCharacter = Read<byte>();
                span[length] = currentCharacter;
                length += 1;
            }
            while (currentCharacter != 0x0);
#if NETCOREAPP
            return encoding.GetString(span.Slice(0, length - 1));
#else
            return encoding.GetString(ptr, length - 1);
#endif
        }

        /// <summary>
        /// Writes a string to a given string.
        /// </summary>
        /// <param name="text">The text to write to the stream.</param>
        /// <param name="maxLengthBytes">Maximum length in bytes.</param>
        /// <param name="encoding">The encoding to use.</param>
#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
        public void WriteString(string text, int maxLengthBytes = 1024, System.Text.Encoding encoding = null)
        {
            encoding ??= System.Text.Encoding.UTF8;
#if NETCOREAPP
            Span<byte> span = stackalloc byte[maxLengthBytes];
            int encodedBytes = encoding.GetBytes(text, span);
            var sliced = span.Slice(0, encodedBytes);
#else
            Span<byte> sliced = encoding.GetBytes(text);
#endif
            for (int x = 0; x < sliced.Length; x++)
                Write(sliced[x]);

            // Null delimiter
            Write<byte>(0x0);
        }

#endregion

#region Utilities
        /// <summary>
        /// Creates a byte array from specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="item">The item to convert into a byte array.</param>
        /// <param name="buffer">Buffer inside which the result is to be stored.</param>
        /// <returns>Original span sliced to contain only the bytes of the struct.</returns>
        [MethodImpl(AggressiveInlining)]
        private static Span<byte> GetBytes<T>(ref T item, Span<byte> buffer) where T : unmanaged
        {
            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(buffer), item);
            return buffer.Slice(0, sizeof(T));
        }

        /// <summary>
        /// Gets the mask necessary to mask out a given number of bits.
        /// </summary>
#if NETCOREAPP
        [MethodImpl(AggressiveInlining | AggressiveOptimization)]
#else
        [MethodImpl(AggressiveInlining)]
#endif
        private uint GetMask(int numBits) => ((uint)1 << numBits) - 1;

        /// <summary>
        /// Gets the mask necessary to mask out a given number of bits.
        /// </summary>
#if NETCOREAPP
        [MethodImpl(AggressiveInlining | AggressiveOptimization)]
#else
        [MethodImpl(AggressiveInlining)]
#endif
        private ulong GetMaskLong(int numBits) => ((ulong)1 << numBits) - 1;

#if NETCOREAPP
        [MethodImpl(AggressiveInlining | AggressiveOptimization)]
#else
        [MethodImpl(AggressiveInlining)]
#endif
        private int SignShrink(int value) => SignExtend(value, IntNumBits);

#if NETCOREAPP
        [MethodImpl(AggressiveInlining | AggressiveOptimization)]
#else
        [MethodImpl(AggressiveInlining)]
#endif
        private long SignShrink(long value) => SignExtend(value, LongNumBits);

#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
#if NETCOREAPP
        [MethodImpl(AggressiveInlining | AggressiveOptimization)]
#else
        [MethodImpl(AggressiveInlining)]
#endif
        private int SignExtend(int value, int numBits)
        {
            var mask = 1 << (numBits - 1);
            return (value ^ mask) - mask;
        }

#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#endif
#if NETCOREAPP
        [MethodImpl(AggressiveInlining | AggressiveOptimization)]
#else
        [MethodImpl(AggressiveInlining)]
#endif
        private long SignExtend(long value, int numBits)
        {
            long mask = 1L << (numBits - 1);
            return (value ^ mask) - mask;
        }
#endregion
    }
}
