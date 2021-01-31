using Sewer56.BitStream.ByteStreams;
using System;

namespace Sewer56.BitStream.Tests.Helpers
{
    public static class Helpers
    {
        /// <summary>
        /// Creates an array filled with a given value..
        /// </summary>
        /// <param name="numBytes">The number of bytes to create.</param>
        /// <param name="value">The value to fill the array contents with.</param>
        public static ArrayByteStream CreateArrayStream(int numBytes, byte value = 0b10101010)
        {
            var array = new byte[numBytes];
            Array.Fill<byte>(array, value);
            return new ArrayByteStream(array);
        }

        /// <summary>
        /// Resets the contents of the given array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="value">The value to set.</param>
        public static void ResetArray(byte[] array, byte value = 0b10101010) => Array.Fill(array, value);

        public static byte MakeHighPatternBits(int num, byte value = 0b10101010)
        {
            int remain = 8 - num;
            return (byte)((value & ((1 << num) - 1) << remain) >> remain);
        }

        public static byte MakeLowPatternBits(int num, byte value = 0b10101010)
        {
            return (byte)(value & ((1 << num) - 1));
        }
    }
}
