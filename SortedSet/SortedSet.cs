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

        public int Count => Tree.Count;

        public SortedSet(IComparer<T>? comparer = null)
        {
            Comparer = comparer ?? Comparer<T>.Default;
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
        //could do ceiling and floor with new made in order traversal
        public T Ceiling(T item)
        {
            if (Tree.Count == 0) throw new InvalidOperationException();
            T ceiling = Tree.Root.Value;
            return CeilingHelper(Tree.Root, item, ceiling);
        }

        private T CeilingHelper(Node<T> node, T item, T ceiling)
        {
            if (node != null && Comparer.Compare(node.Value, item) > 0 && Comparer.Compare(node.Value, ceiling) < 0)
            {
                ceiling = node.Value;
            }
            else if (node == null)
            {
                if (Comparer.Compare(ceiling, item) < 0) throw new InvalidOperationException();
                else return ceiling;
            }


            if (Comparer.Compare(node.Value, item) > 0)
            {
                return CeilingHelper(node.LeftChild, item, ceiling);
            }
            else
            {
                return CeilingHelper(node.RightChild, item, ceiling);
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
            if (node != null && Comparer.Compare(node.Value, item) < 0)
            {
                if (Comparer.Compare(floor, item) < 0)
                {
                    if (Comparer.Compare(node.Value, floor) > 0)
                    {
                        floor = node.Value;
                    }
                }
                else
                {
                    floor = node.Value;
                }

            }
            else if (node == null)
            {
                if (Comparer.Compare(floor, item) > 0) throw new InvalidOperationException();
                else return floor;
            }


            if (Comparer.Compare(node.Value, item) > 0)
            {

                return FloorHelper(node.LeftChild, item, floor);
            }
            else
            {
                return FloorHelper(node.RightChild, item, floor);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            Stack<T> stack = new Stack<T>();
            List<T> nodes = new List<T>();
            List<T> list = Tree.InOrderTraversal(Tree.Root, stack, nodes);

            foreach (T val in list)
            {
                yield return val;
            }
        }

        //returns new sorted set that has same value between to sets
        public ISortedSet<T> Intersection(ISortedSet<T> other)
        {
            SortedSet<T> set = new SortedSet<T>(Comparer);
            foreach (T t in other)
            {
                if (this.Contains(t))
                {
                    set.Add(t);
                }
            }

            return set;
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
        //returns both the sets together without duplicates
        public ISortedSet<T> Union(ISortedSet<T> other)
        {
            SortedSet<T> set = new SortedSet<T>(Comparer);
            foreach (T t in this)
            {

                set.Add(t);

            }
            foreach (T t in other)
            {
                if (!this.Contains(t))
                {
                    set.Add(t);
                }
            }


            return set;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
