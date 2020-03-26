using System;
using System.IO;

namespace crypto.Core
{
    public class RandomLengthFileContent
    {
        public RandomLengthFileContent(byte[] data)
        {
            Data = data;
        }

        public RandomLengthFileContent()
        {
        }

        public byte[] Data { get; private set; }

        public void WriteTo(Stream target)
        {
            target.Write(BitConverter.GetBytes(Data.Length));
            target.Write(Data);
        }

        public void ReadFrom(Stream source)
        {
            var longBuffer = new byte[sizeof(int)];
            source.Read(longBuffer);
            var size = BitConverter.ToInt32(longBuffer);

            Data = new byte[size];
            source.Read(Data);
        }
    }
}