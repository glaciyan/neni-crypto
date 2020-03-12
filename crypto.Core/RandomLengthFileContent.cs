﻿using System;
using System.IO;

namespace crypto.Core
{
    // not memory friendly,
    // but good enough for the small amount of data I will write,
    // maybe replace with memory mapped file
    public class RandomLengthFileContent
    {
        public RandomLengthFileContent(byte[] data)
        {
            Data = data;
        }

        public byte[] Data { get; set; }

        public void WriteTo(Stream target)
        {
            target.Write(BitConverter.GetBytes(Data.Length));
            target.Write(Data);
        }

        public byte[] ReadFrom(Stream destination)
        {
            var longBuffer = new byte[sizeof(long)];
            destination.Read(longBuffer);
            var size = BitConverter.ToInt32(longBuffer);

            Data = new byte[size];
            destination.Read(Data);

            return Data;
        }
    }
}