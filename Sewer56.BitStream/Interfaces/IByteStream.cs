namespace Sewer56.BitStream.Interfaces;

/// <summary>
/// Abstraction of a stream of bytes.
/// </summary>
public interface IByteStream
{
    /// <summary>
    /// Read a byte from the stream at a given index.
    /// </summary>
    /// <param name="index">Index of the byte to read.</param>
    /// <returns>The byte at the given index.</returns>
    byte Read(int index);

    /// <summary>
    /// Write a byte at a given index.
    /// </summary>
    /// <param name="value">Byte to write.</param>
    /// <param name="index">Index of the byte to write.</param>
    void Write(byte value, int index);
}