using Sewer56.BitStream.ByteStreams;
using Xunit;
using static Sewer56.BitStream.Tests.Helpers.Helpers;

namespace Sewer56.BitStream.Tests
{
    public class SignedTest
    {
        [Fact]
        public void SignedWrite()
        {
            var arrayStream = CreateArrayStream(sizeof(ulong) + 1, 0b10101010);
            var bitStream   = new BitStream<ArrayByteStream>(arrayStream);

            for (int numBits = 32; numBits >= 1; numBits--)
            {
                var minSignedValue = ((1 << (numBits - 1))) * -1;
                var maxSignedValue = (minSignedValue * -1) - 1;
                
                // Clamp number of tested values.
                if (minSignedValue < short.MinValue) minSignedValue = short.MinValue;
                if (maxSignedValue > short.MaxValue) maxSignedValue = short.MaxValue;

                for (int x = minSignedValue; x < maxSignedValue; x++)
                {
                    bitStream.Seek(0);
                    bitStream.WriteSigned(x, numBits);

                    bitStream.Seek(0);
                    var actual = bitStream.ReadSigned<int>(numBits);

                    Assert.Equal(x, actual);
                }
            }
        }
    }
}