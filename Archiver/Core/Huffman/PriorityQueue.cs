using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Archiver.Core.Huffman
{
    internal class PriorityQueue<T> : IEnumerable<T> where T : IComparable<T>
    {
        private readonly List<T> _elements;
        private readonly Comparison<T> _comparison;

        public int Count => _elements.Count;
        private void Reorder()
        {
            _elements.Sort((e1, e2) => _comparison(e1, e2));
        }

        public PriorityQueue(List<T> elements, Comparison<T> comparison = null)
        {
            _elements = elements;
            _comparison = comparison ?? ((x, y) => x.CompareTo(y));
            Reorder();
        }

        public T Pop()
        {
            var elem = _elements.First();
            _elements.Remove(elem);
            Reorder();
            return elem;
        }

        public void Push(T elem)
        {
            _elements.Add(elem);
            Reorder();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }
    }
}
