using System;
using Archiver.Core;

namespace Archiver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var lzw = new LZWArchiver();

            lzw.Compress("Onegin.fb2", "Onegin.lzw");
            lzw.Decompress("Onegin.lzw", "OneginD.fb2");

            /*
            var huffman = new HuffmanArchiver();
            huffman.Compress("Onegin.fb2", "Onegin.huff");
            huffman.Decompress("Onegin.huff", "OneginH.fb2");
            */

            var huffman = new HuffmanArchiver();
            huffman.Compress("text.txt", "text.huff");
            huffman.Decompress("text.huff", "textH.txt");

            Console.ReadKey();
        }
    }
}