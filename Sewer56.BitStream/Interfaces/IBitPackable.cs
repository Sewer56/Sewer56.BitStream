namespace Sewer56.BitStream.Interfaces
{
    /// <summary>
    /// Interface to be implemented by all types that support bit packing.
    /// </summary>
    /// <typeparam name="TSelf"></typeparam>
    public interface IBitPackable<out TSelf> where TSelf : new()
    {
        /// <summary>
        /// Deserializes the structure contents from the stream.
        /// </summary>
        /// <param name="stream">The stream to read the contents from.</param>
        public TSelf FromStream<T>(ref BitStream<T> stream) where T : IByteStream;

        /// <summary>
        /// Writes the contents of the current structure to a stream.
        /// </summary>
        /// <param name="stream">The stream to write the contents to.</param>
        public void ToStream<T>(ref BitStream<T> stream) where T : IByteStream;
    }
}