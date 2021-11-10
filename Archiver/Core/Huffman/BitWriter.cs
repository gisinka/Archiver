using System;
using System.IO;

namespace Archiver.Core.Huffman
{
    internal class BitWriter : IDisposable
    {
        private readonly bool[] array;
        private readonly BinaryWriter writer;
        private int counter;

        public BitWriter(string path)
        {
            array = new bool[8];
            writer = new BinaryWriter(new FileStream(path, FileMode.Create));
            counter = 0;
        }

        public void Dispose()
        {
            WriteByte();
            writer?.Dispose();
        }

        public void WriteBool(bool toWrite)
        {
            if (counter >= 8)
            {
                WriteByte();
                counter = 0;
            }

            array[counter] = toWrite;
            counter++;
        }

        private void WriteByte()
        {
            var byteToWrite = ConvertBoolArrayToByte(array);
            writer.Write(byteToWrite);
        }

        private static byte ConvertBoolArrayToByte(bool[] source)
        {
            byte result = 0;
            var index = 8 - source.Length;

            foreach (var b in source)
            {
                if (b)
                    result |= (byte)(1 << (7 - index));

                index++;
            }

            return result;
        }
    }
}