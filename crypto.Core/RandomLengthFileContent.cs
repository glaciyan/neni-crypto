using System;
using System.Collections.Generic;
using System.IO;

namespace crypto.Core
{
    public class RandomLengthFileContent
    {
        public RandomLengthFileContent(byte[] content)
        {
            Content = content;
        }

        public RandomLengthFileContent()
        {
        }

        public byte[] Content { get; private set; }

        public byte[] GetBytes()
        {
            var output = new byte[Content.Length + sizeof(int)];
            output.CombineFrom(BitConverter.GetBytes(Content.Length), Content);
            return output;
        }

        public void WriteTo(Stream target)
        {
            target.Write(BitConverter.GetBytes(Content.Length));
            target.Write(Content);
        }

        public void ReadFrom(Stream source)
        {
            var longBuffer = new byte[sizeof(int)];
            source.Read(longBuffer);
            var size = BitConverter.ToInt32(longBuffer);

            Content = new byte[size];
            source.Read(Content);
        }
    }
}