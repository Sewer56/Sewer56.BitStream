using System;
using System.IO;
using System.Text;
using Sewer56.BitStream.ByteStreams;
using Xunit;
using static Sewer56.BitStream.Tests.Helpers.Helpers;

namespace Sewer56.BitStream.Tests;

public class StringTest
{
    [Fact]
    public void ReadWriteString()
    {
        for (int x = 0; x < 10000; x++)
        {
            string path     = Path.GetRandomFileName();
            var arrayStream = CreateArrayStream(Encoding.UTF8.GetByteCount(path) + 1, 0b10101010);
            var bitStream   = new BitStream<ArrayByteStream>(arrayStream);

            bitStream.BitIndex = 0;
            bitStream.WriteString(path);
            var indexPostWrite = bitStream.BitIndex;

            bitStream.BitIndex = 0;
            var copyPath = bitStream.ReadString();
            var indexPostRead = bitStream.BitIndex;

            Assert.Equal(indexPostWrite, indexPostRead);
            Assert.Equal(path, copyPath);
        }
    }
}