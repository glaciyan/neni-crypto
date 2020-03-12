using System;
using System.IO;

namespace crypto.Core
{
    public class RandomLengthFileContent
    {
        public byte[] Data { get; set; }
        public long Size { get; set; }

        public RandomLengthFileContent(byte[] data)
        {
            Data = data;
            Size = data.LongLength;
        }

        public void WriteTo(Stream target)
        {
            target.Write(BitConverter.GetBytes(Size));
            target.Write(Data);
        }

        public byte[] ReadFrom(Stream destination)
        {
            var longBuffer = new byte[sizeof(long)];
            destination.Read(longBuffer);
            Size = BitConverter.ToInt64(longBuffer);

            Data = new byte[Size];
            destination.Read(Data);

            return Data;
        }
    }
}