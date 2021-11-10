using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Archiver.Core
{
    internal class LZWArchiver : IArchiver
    {
        private readonly char[] chars;
        public readonly Encoding Encoding;
        private readonly int maxDictionarySize;

        public LZWArchiver(int codepage = 1251)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding = Encoding.GetEncoding(codepage);
            chars = Encoding.GetChars(Enumerable.Range(0, 256).Select(n => (byte)n).ToArray());
            maxDictionarySize = ushort.MaxValue + 1;
        }

        public void Compress(string inputFile, string outputFile)
        {
            var dictionary = InitializeDictionary();
            var entry = string.Empty;

            using var reader = new StreamReader(new BufferedStream(new FileStream(inputFile, FileMode.Open)), Encoding);
            using var writer = new BinaryWriter(new FileStream(outputFile, FileMode.Create));

            var buffer = new char[1];

            while (!reader.EndOfStream)
            {
                reader.Read(buffer);

                var symbol = buffer[0];

                if (dictionary.ContainsKey(entry + symbol))
                    entry += symbol;
                else
                {
                    writer.Write(dictionary[entry]);

                    if (dictionary.Count != maxDictionarySize)
                        dictionary.Add(entry + symbol, (ushort)dictionary.Count);

                    entry = symbol.ToString();
                }
            }

            writer.Write(dictionary[entry]);
        }

        public void Decompress(string inputFile, string outputFile)
        {
            var reverseDictionary = InitializeReverseDictionary();
            var entry = string.Empty;

            using var reader = new BinaryReader(new BufferedStream(new FileStream(inputFile, FileMode.Open)));
            using var writer = new StreamWriter(new FileStream(outputFile, FileMode.Create), Encoding);

            var previousCode = reader.ReadUInt16();
            writer.Write(reverseDictionary[previousCode]);

            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                var currentCode = reader.ReadUInt16();

                if (reverseDictionary.ContainsKey(currentCode))
                    entry = reverseDictionary[currentCode];
                else
                    entry += entry[0];

                writer.Write(entry);

                var currentSymbol = entry[0];

                if (reverseDictionary.Count != maxDictionarySize)
                {
                    reverseDictionary.Add((ushort)reverseDictionary.Count,
                        reverseDictionary[previousCode] + currentSymbol);
                }

                previousCode = currentCode;
            }
        }

        private Dictionary<string, ushort> InitializeDictionary()
        {
            var dictionary = new Dictionary<string, ushort>();

            for (var i = 0; i < chars.Length; i++)
                dictionary.Add(chars[i].ToString(), (ushort)i);

            return dictionary;
        }

        private Dictionary<ushort, string> InitializeReverseDictionary()
        {
            var reverseDictionary = new Dictionary<ushort, string>();

            for (var i = 0; i < chars.Length; i++)
                reverseDictionary.Add((ushort)i, chars[i].ToString());

            return reverseDictionary;
        }
    }
}