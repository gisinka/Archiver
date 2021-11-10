using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Archiver.Core.Huffman
{
    internal class PriorityQueue<T> : IEnumerable<T> where T : IComparable<T>
    {
        private readonly List<T> elements;
        private readonly Comparison<T> comparison;

        public int Count => elements.Count;
        private void Reorder()
        {
            elements.Sort((e1, e2) => comparison(e1, e2));
        }

        public PriorityQueue(List<T> elements, Comparison<T> comparison = null)
        {
            this.elements = elements;
            this.comparison = comparison ?? ((x, y) => x.CompareTo(y));
            Reorder();
        }

        public T Pop()
        {
            var elem = elements.First();
            elements.Remove(elem);
            Reorder();
            return elem;
        }

        public void Push(T elem)
        {
            elements.Add(elem);
            Reorder();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return elements.GetEnumerator();
        }
    }
}
