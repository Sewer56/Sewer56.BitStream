namespace Sewer56.BitStream.Interfaces;

/// <summary>
/// Extension of <see cref="IByteStream"/> that adds support for basic primitives.
/// </summary>
public interface IStreamWithReadBasicPrimitives
{
    /// <summary>
    /// Reads 2 bytes from the stream at a given index.
    /// </summary>
    /// <param name="index">Index of the byte to read.</param>
    /// <returns>The byte at the given index.</returns>
    ushort Read2(int index);

    /// <summary>
    /// Writes 2 bytes to the stream at a given index.
    /// </summary>
    /// <param name="value">Data to write.</param>
    /// <param name="index">Index of the byte to write.</param>
    void Write2(ushort value, int index);
    
    /// <summary>
    /// Reads 4 bytes from the stream at a given index.
    /// </summary>
    /// <param name="index">Index of the byte to read.</param>
    /// <returns>The byte at the given index.</returns>
    uint Read4(int index);

    /// <summary>
    /// Writes 4 bytes to the stream at a given index.
    /// </summary>
    /// <param name="value">Data to write.</param>
    /// <param name="index">Index of the byte to write.</param>
    void Write4(uint value, int index);
    
    /// <summary>
    /// Reads 4 bytes from the stream at a given index.
    /// </summary>
    /// <param name="index">Index of the byte to read.</param>
    /// <returns>The byte at the given index.</returns>
    ulong Read8(int index);

    /// <summary>
    /// Writes 4 bytes to the stream at a given index.
    /// </summary>
    /// <param name="value">Data to write.</param>
    /// <param name="index">Index of the byte to write.</param>
    void Write8(ulong value, int index);
}