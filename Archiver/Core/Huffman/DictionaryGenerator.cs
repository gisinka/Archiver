using System.Collections.Generic;

namespace Archiver.Core.Huffman
{
    internal class DictionaryGenerator
    {
        private readonly Dictionary<char, List<bool>> codes = new();

        public static Dictionary GenerateDictionary(IDictionary<char, int> frequencies)
        {
            return new DictionaryGenerator().Generate(frequencies);
        }

        private Dictionary Generate(IDictionary<char, int> frequencies)
        {
            var queue = InitQueue(frequencies);
            HuffmanTree root = null;

            while (queue.Count > 1)
            {
                var first = queue.Pop();
                var second = queue.Pop();
                var f1 = first.Frequency;
                var f2 = second.Frequency;
                var sum = new HuffmanTree(f1 + f2, '$', first, second);
                queue.Push(sum);
                root = sum;
            }

            CollectCodes(root);
            return new Dictionary(codes);
        }

        private PriorityQueue<HuffmanTree> InitQueue(IDictionary<char, int> frequencies)
        {
            var list = new List<HuffmanTree>();

            foreach (var (key, value) in frequencies)
            {
                var tree = new HuffmanTree(value, key);
                list.Add(tree);
            }

            var queue = new PriorityQueue<HuffmanTree>(list);
            return queue;
        }

        private void CollectCodes(HuffmanTree tree, List<bool> code = null)
        {
            code ??= new List<bool>();

            if (tree == null)
                return;

            if (tree.Left == null && tree.Right == null && tree.Character != '$')
            {
                codes.Add(tree.Character, code);
                return;
            }

            CollectCodes(tree.Left, new List<bool>(code) { false });
            CollectCodes(tree.Right, new List<bool>(code) { true });
        }
    }
}