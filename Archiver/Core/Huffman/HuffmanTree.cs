using System;

namespace Archiver.Core.Huffman
{
    public class HuffmanTree : IComparable<HuffmanTree>
    {
        public int Frequency { get; set; }
        public char Character { get; }

        public HuffmanTree Left { get; set; }
        public HuffmanTree Right { get; set; }

        public HuffmanTree(int frequency, char character, HuffmanTree left = null, HuffmanTree right = null)
        {
            Frequency = frequency;
            Character = character;
            Left = left;
            Right = right;
        }

        public int CompareTo(HuffmanTree other)
        {
            return Frequency.CompareTo(other.Frequency);
        }
    }
}