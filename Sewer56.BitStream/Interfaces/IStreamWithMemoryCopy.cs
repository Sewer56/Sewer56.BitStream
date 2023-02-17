using System;

namespace Sewer56.BitStream.Interfaces;

/// <summary>
/// Extension of <see cref="IByteStream"/> for streams that support reading/writing raw span data.
/// </summary>
public interface IStreamWithMemoryCopy
{
    /// <summary>
    /// Read X bytes from the stream at a given index.
    /// </summary>
    /// <param name="data">The data to read into.</param>
    /// <param name="index">Index of the byte to read.</param>
    /// <returns>The byte at the given index.</returns>
    void Read(Span<byte> data, int index);

    /// <summary>
    /// Write X bytes to the stream at a given index.
    /// </summary>
    /// <param name="value">Data to write.</param>
    /// <param name="index">Index of the byte to write.</param>
    void Write(Span<byte> value, int index);
}