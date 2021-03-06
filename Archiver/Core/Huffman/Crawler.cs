using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Archiver.Core.Huffman
{
    internal static class Crawler
    {

        public static IDictionary<char, int> CrawlFile(string filePath, Encoding encoding)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var dictionary = new Dictionary<char, int>();

            using var reader = new StreamReader(new BufferedStream(new FileStream(filePath, FileMode.Open)), encoding);
            var buffer = new char[1];

            while (!reader.EndOfStream)
            {
                reader.Read(buffer);

                if (!dictionary.ContainsKey(buffer[0]))
                    dictionary[buffer[0]] = 0;
                dictionary[buffer[0]]++;
            }

            return dictionary;
        }
    }
}
