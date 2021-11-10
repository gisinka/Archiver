using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Archiver.Core.Huffman
{
    internal class Dictionary
    {
        private readonly Dictionary<char, List<bool>> dictionary;

        public Dictionary(Dictionary<char, List<bool>> dictionary)
        {
            this.dictionary = dictionary;
        }

        public Dictionary(string fileName, Encoding encoding)
        {
            var dict = new Dictionary<char, List<bool>>();
            var lines = File.ReadAllLines(fileName, encoding);

            foreach (var line in lines)
            {
                var pair = line.Split("~");
                var key = pair[0];

                var realKey = key switch
                {
                    "sn" => '\n',
                    "sr" => '\r',
                    _ => key[0]
                };

                var val = pair[1];
                var code = new List<bool>();

                foreach (var item in val)
                {
                    switch (item)
                    {
                        case '1':
                            code.Add(true);
                            break;
                        case '0':
                            code.Add(false);
                            break;
                    }
                }

                dict.Add(realKey, code);
            }

            dictionary = dict;
        }

        public List<bool> GetCodeFor(char letter)
        {
            if (!dictionary.ContainsKey(letter)) throw new ArgumentException($"{letter} is not in the dictionary");

            return dictionary[letter];
        }

        public bool Contains(char letter)
        {
            return dictionary.ContainsKey(letter);
        }

        public Dictionary<char, List<bool>> ToDictionary()
        {
            return dictionary;
        }
    }
}