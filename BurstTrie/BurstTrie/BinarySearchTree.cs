using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BurstTries
{
    public class BinarySearchTree<T> where T : IComparable<T>
    {
        public BSTNode<T> Root { get; set; }
        public int Count { get; private set; }

        public BinarySearchTree(BSTNode<T> root = null)
        {
            Root = root;
        }

        public BSTNode<T> Search(BSTNode<T> node, T value)
        {
            if (node == null) return null;
            else if (node.Value.Equals(value)) return node;

            BSTNode<T> match = new BSTNode<T>();
            if(node.LeftChild != null) match = Search(node.LeftChild, value);

            if(node.RightChild != null) match = Search(node.RightChild, value);


            return match;
        }

     

        public BSTNode<T> Minimum(BSTNode<T> node)
        {
            if (node == null) throw new NullReferenceException();
            while (node.LeftChild != null)
            {
                node = node.LeftChild;
            }

            return node;
        }

        public BSTNode<T> Maximum(BSTNode<T> node)
        {
            if (node == null) throw new NullReferenceException();
            while (node.RightChild != null)
            {
                node = node.RightChild;
            }

            return node;
        }

        public bool IsLeftChild(BSTNode<T> node)
        {
            if (node == null || node.Parent == null) throw new NullReferenceException();
            if (node.Parent.LeftChild == null) return false;
            return node.Parent.LeftChild.Equals(node);
        }

        public bool IsRightChild(BSTNode<T> node)
        {
            if (node == null || node.Parent == null) throw new NullReferenceException();
            if (node.Parent.RightChild == null) return false;
            return node.Parent.RightChild.Equals(node);
        }

        public void Insert(T val)
        {
            if (val == null) throw new NullReferenceException();
            Root = InsertHelper(Root, val, null);
            Count++;
        }

        private BSTNode<T> InsertHelper(BSTNode<T> node, T val, BSTNode<T> parent)
        {
            BSTNode<T> temp = new BSTNode<T>(val, parent);
            if (Root == null)
            {
                node = temp;
                return node;
            }
            else if(node == null)
            {
                node = temp;
                // if (IsLeftChild(node)) node.Parent.LeftChild = node;
                // else node.Parent.RightChild = node;
                if (parent.Value.CompareTo(node.Value) > 0) node.Parent.LeftChild = node;
                else node.Parent.RightChild = node;


                    return node;
            }

            if (val.CompareTo(node.Value) < 0) node.LeftChild = InsertHelper(node.LeftChild, val, node);
            else node.RightChild = InsertHelper(node.RightChild, val, node);


            return node;
        }

        public void Delete(T val)
        {
            if (val == null || Root == null) throw new NullReferenceException();
            BSTNode<T> nodeToRemove = DeleteHelper(Root, val);

            if(nodeToRemove.LeftChild != null && nodeToRemove.RightChild != null)
            {
                BSTNode<T> maxNode = Maximum(nodeToRemove.LeftChild);
                nodeToRemove.Parent.Value = maxNode.Value;
                maxNode = null;
            }

            if(nodeToRemove.LeftChild != null)
            {
                if (IsLeftChild(nodeToRemove))
                {
                    nodeToRemove.LeftChild.Parent = nodeToRemove.Parent;
                    nodeToRemove.Parent.LeftChild = nodeToRemove.LeftChild;
                }
                else
                {
                    nodeToRemove.LeftChild.Parent = nodeToRemove.Parent;
                    nodeToRemove.Parent.RightChild = nodeToRemove.LeftChild;
                }
            }
            else if(nodeToRemove.RightChild != null)
            {
                if (IsLeftChild(nodeToRemove))
                {
                    nodeToRemove.RightChild.Parent = nodeToRemove.Parent;
                    nodeToRemove.Parent.LeftChild = nodeToRemove.RightChild;
                }
                else
                {
                    nodeToRemove.RightChild.Parent = nodeToRemove.Parent;
                    nodeToRemove.Parent.RightChild = nodeToRemove.RightChild;
                }
            }   
            else
            {
                if (IsLeftChild(nodeToRemove)) nodeToRemove.Parent.LeftChild = null;
                else nodeToRemove.Parent.RightChild = null;
            }
            Count--;
        }

        private BSTNode<T> DeleteHelper(BSTNode<T> node, T val)
        {
            if (node.Value.Equals(val)) return node;

            BSTNode<T> deleter = new BSTNode<T>();
            if (val.CompareTo(node.Value) < 0) deleter = DeleteHelper(node.LeftChild, val);
            else deleter = DeleteHelper(node.RightChild, val);

            return deleter;
        }

    }
}
