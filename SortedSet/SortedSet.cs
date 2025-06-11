using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SortedSet
{
    public class SortedSet<T> : ISortedSet<T>
    {
        public IComparer<T> Comparer { get; private set; }

        private RedBlackTree<T> Tree { get; set; }

        public int Count { get; private set; }

        public SortedSet(IComparer<T>? comparer = null) 
        {
            Comparer = comparer ?? Comparer<T>.Default;
            Count = 0;
            Tree = new RedBlackTree<T>(comparer);
        }

        public bool Add(T item)
        {
            return Tree.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public T Ceiling(T item)
        {
            if (Tree.Count == 0) throw new InvalidOperationException();
            T ceiling = Tree.Root.Value;
            return CeilingHelper(Tree.Root, item, ceiling);
        }

        private T CeilingHelper(Node<T> node, T item, T ceiling)
        {
            if (Comparer.Compare(node.Value, item) > 0 && Comparer.Compare(node.Value, item) < 0)
            {
                ceiling = node.Value;
            }

            if (node == null && Comparer.Compare(node.Value, item) < 0) throw new InvalidOperationException();
            else if (node == null && Comparer.Compare(node.Value, item) > 0) return ceiling;


            if (Comparer.Compare(Tree.Root.Value, item) > 0)
            {
                return CeilingHelper(node.RightChild, item, ceiling);
            }
            else
            {
                return CeilingHelper(node.LeftChild, item, ceiling);
            }
        }

        public void Clear()
        {
            Tree.Clear();
        }

        public bool Contains(T item)
        {
            return Tree.Contains(Tree.Root, item);
        }

        public T Floor(T item)
        {
            if (Tree.Count == 0) throw new InvalidOperationException();
            T floor = Tree.Root.Value;
            return FloorHelper(Tree.Root, item, floor);
        }

        private T FloorHelper(Node<T> node, T item, T floor)
        {
            if (Comparer.Compare(node.Value, item) < 0 && Comparer.Compare(node.Value, item) > 0)
            {
                floor = node.Value;
            }

            if (node == null && Comparer.Compare(node.Value, item) > 0) throw new InvalidOperationException();
            else if (node == null && Comparer.Compare(node.Value, item) < 0) return floor;


            if (Comparer.Compare(Tree.Root.Value, item) > 0)
            {
                return CeilingHelper(node.RightChild, item, floor);
            }
            else
            {
                return CeilingHelper(node.LeftChild, item, floor);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public ISortedSet<T> Intersection(ISortedSet<T> other)
        {
            throw new NotImplementedException();
        }

        public T Max()
        {
           return Tree.Maximum(Tree.Root).Value;
        }

        public T Min()
        {
            return Tree.Minimum(Tree.Root).Value;
        }

        public bool Remove(T item)
        {
            return Tree.Remove(item);
        }

        public ISortedSet<T> Union(ISortedSet<T> other)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
