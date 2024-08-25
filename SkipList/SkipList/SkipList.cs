using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipList
{
    public class SkipList<T> where T : IComparable
    {
        [DebuggerDisplay("Value = {Value}" )]
        class Node
        {
            public T? Value { get; set; }
            public List<Node?> Next { get; set; }

            public Node()
            {
                Value = default(T);
                Next = new List<Node?>();
                Next.Add(null);
            }

            public Node(T value, int height)
            {
                Value = value;
                Next = new List<Node?>();
                while (Next.Count < height)
                {
                    Next.Add(null);
                }
            }
        }

        private Node? Head;
        public int Count { get; private set; }

        public SkipList()
        {
            Head = new Node();
        }


        public int HeightGen()
        {
            return RandHelper(1);
        }

        private int RandHelper(int height)
        { 
            Random rand = new Random();
            int randInt = rand.Next(2);

            if (randInt == 0 || height > Head.Next.Count)
            {
                if (height > Head.Next.Count)
                { 
                    Head.Next.Add(null);
                }
                return height; 
            }
            else return RandHelper(height + 1);
        }

        public void Insert(T value)
        {
            Node current = Head;
            Node node = new Node(value, HeightGen());
            int height = current.Next.Count - 1;
            while (height > 0 && (current.Value.CompareTo(value) < 0 || current.Next[height] != null))
            {
                if (current.Value.CompareTo(value) < 0 && (current.Next[height] == null || current.Next[height].Value.CompareTo(value) > 0))
                {
                    if (node.Next.Count - 1 >= height)
                    {
                        node.Next[height] = current.Next[height];
                        current.Next[height] = node;
                    }
                    height--;
                }
                else
                {
                    current = current.Next[height];
                }
                
            }
            node.Next[height] = current.Next[height];
            current.Next[height] = node;
        }

        public void Remove(T value)
        {
            Node current = Head;
            int height = current.Next.Count - 1;
            while (height > 0)
            {
                if (current.Value.CompareTo(value) < 0 && (current.Next[height] == null || current.Next[height].Value.CompareTo(value) > 0))
                {
                   
                }
                else if (current.Value.Equals(value))
                {
                    while()
                    {

                    }
                }
                else
                {
                    current = current.Next[height];
                }
            }
            throw new ArgumentException("Can not remove value that does not exist");
        }




    }
}
