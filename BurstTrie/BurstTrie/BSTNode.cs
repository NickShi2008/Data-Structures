using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurstTries
{
    public class BSTNode<T> where T : IComparable<T>
    {
        public T Value { get; set; }
        public BSTNode<T> Parent { get; set; }
        public BSTNode<T> LeftChild { get; set; }
        public BSTNode<T> RightChild { get; set; }

        public BSTNode(T value = default(T), BSTNode<T> parent = null,
            BSTNode<T> left = null, BSTNode<T> right = null)
        {
            Value = value;
            Parent = parent;
            LeftChild = left;
            RightChild = right;
        }

    }
}
