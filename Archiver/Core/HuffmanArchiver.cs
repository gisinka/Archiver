using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Archiver.Core.Huffman;

namespace Archiver.Core
{
    internal class HuffmanArchiver : IArchiver
    {
        public readonly Encoding Encoding;

        public HuffmanArchiver()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding = Encoding.UTF8;
        }

        public void Compress(string inputFile, string outputFile)
        {
            var frequencies = Crawler.CrawlFile(inputFile, Encoding);
            var dictionary = DictionaryGenerator.GenerateDictionary(frequencies);
            var dictionaryFilename = $"{outputFile}.alphabet";

            using var reader = new StreamReader(new BufferedStream(new FileStream(inputFile, FileMode.Open)), Encoding);
            using var writer = new BitWriter(outputFile);

            var buffer = new char[1];

            while (!reader.EndOfStream)
            {
                reader.Read(buffer);

                dictionary.GetCodeFor(buffer[0]).ForEach(x => writer.WriteBool(x));
            }

            WriteAlphabetFile(dictionaryFilename, dictionary, Encoding);
        }

        public void Decompress(string inputFile, string outputFile)
        {
            var dictionary = new Dictionary($"{inputFile}.alphabet", Encoding);
            Decompress(inputFile, dictionary, outputFile);
        }

        private void Decompress(string inputFile, Dictionary dictionary, string outputFile)
        {
            using var reader = new BitReader(inputFile);
            using var output = new StreamWriter(new FileStream(outputFile, FileMode.Create), Encoding);

            while (!reader.EndOfReader)
            {
                var code = new List<bool> { reader.ReadBoolean() };

                while (!dictionary.ToDictionary().Values.Any(x => x.SequenceEqual(code)))
                    code.Add(reader.ReadBoolean());

                var character = dictionary.ToDictionary()
                    .First(c => dictionary.GetCodeFor(c.Key).SequenceEqual(code))
                    .Key;

                output.Write(character);
            }
        }

        private static void WriteAlphabetFile(string alphabetFilename, Dictionary alphabet, Encoding encoding)
        {
            using var output = new StreamWriter(new FileStream(alphabetFilename, FileMode.Create), encoding);

            foreach (var (key, value) in alphabet.ToDictionary())
            {
                var sb = new StringBuilder();
                foreach (var item in value) sb.Append(item ? '1' : '0');

                switch (key)
                {
                    case '\n':
                        output.Write($"sn~{sb}\r\n");
                        break;
                    case '\r':
                        output.Write($"sr~{sb}\r\n");
                        break;
                    default:
                        output.Write($"{key}~{sb}\r\n");
                        break;
                }
            }
        }
    }
}