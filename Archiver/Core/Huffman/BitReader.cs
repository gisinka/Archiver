using System;
using System.IO;

namespace Archiver.Core.Huffman
{
    internal class BitReader : IDisposable
    {
        private readonly BinaryReader reader;
        private bool[] array;
        private byte index;
        public bool EndOfReader => reader.BaseStream.Position == reader.BaseStream.Length && index == 8;

        public BitReader(string path)
        {
            reader = new BinaryReader(new BufferedStream(new FileStream(path, FileMode.Open)));
            index = 0;
        }

        public void Dispose()
        {
            reader?.Dispose();
        }

        public bool ReadBoolean()
        {
            var result = false;
            if ((array == null || index == 8) && !EndOfReader)
            {
                var readByte = reader.ReadByte();
                array = ConvertByteToBoolArray(readByte);
                index = 0;
            }

            if (index < 8 && array != null)
            {
                result = array[index++];
            }

            return result;
        }

        private static bool[] ConvertByteToBoolArray(byte b)
        {
            var result = new bool[8];

            for (var i = 0; i < 8; i++)
                result[7 - i] = (b & (1 << i)) != 0;

            return result;
        }
    }
}