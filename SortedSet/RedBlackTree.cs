using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortedSet
{
    public class RedBlackTree<T>
    {
        IComparer<T> Comparer { get; set; }
        public Node<T> Root { get; private set; }

        public int Count { get; private set; }
        public RedBlackTree(IComparer<T> comparer)
        {
            Comparer = comparer;
            Root = null;
            Count = 0;
        }
        public bool Add(T value)
        {
            Root = AddHelper(Root, value, out bool isContained);
            Root.IsRed = false;
            Count++;
            return !isContained;
        }

        private Node<T> AddHelper(Node<T> node, T value, out bool isContained)
        {
            isContained = false;
            if (node == null)
            {
                Node<T> current = new Node<T>(value);
                node = current;
            }
            else if (IsRed(node.RightChild) && IsRed(node.LeftChild))
            {
                FlipColor(node);
            }

            if(Comparer.Compare(node.Value, value) == 0)
            {
                isContained = true;
            }
            
            if (Comparer.Compare(node.Value, value) < 0)
            {
                node.RightChild = AddHelper(node.RightChild, value, out isContained);
            }
            else if (Comparer.Compare(node.Value, value) > 0)
            {
                node.LeftChild = AddHelper(node.LeftChild, value, out isContained);
            }

            if (IsRed(node.RightChild))
            {
                if (!IsRed(node.LeftChild))
                {
                    node = RotateLeft(node);
                }

            }

            if (IsRed(node.LeftChild) && IsRed(node.LeftChild.LeftChild))
            {
                node = RotateRight(node);
            }
            return FixUp(node);
        }

        private void FlipColor(Node<T> node)
        {
            node.IsRed = !node.IsRed;
            node.LeftChild.IsRed = !node.LeftChild.IsRed;
            node.RightChild.IsRed = !node.RightChild.IsRed;
        }

        private Node<T> RotateLeft(Node<T> node)
        {
            bool wasRed = node.IsRed;
            Node<T> temp = node;
            node = node.RightChild;
            temp.RightChild = node.LeftChild;
            node.LeftChild = temp;
            node.IsRed = wasRed;
            temp.IsRed = true;

            return node;
        }

        private Node<T> RotateRight(Node<T> node)
        {
            bool wasRed = node.IsRed;
            Node<T> temp = node;
            node = node.LeftChild;
            temp.LeftChild = node.RightChild;
            node.RightChild = temp;
            node.IsRed = wasRed;
            temp.IsRed = true;

            return node;
        }

        public bool Remove(T val)
        {
            int startingCount = Count;
            if (Root == null)
            {
                return false;
            }
            Root = RemoveHelper(Root, val);
            Root.IsRed = false;
            return startingCount != Count;
        }

        private Node<T> RemoveHelper(Node<T> node, T value)
        {
            if (node == null) return node;
            if (Comparer.Compare(node.Value, value) > 0)
            {
                if (!IsRed(node.LeftChild) && !IsRed(node.LeftChild.LeftChild))
                {
                    node = MoveRedLeft(node);
                }
                node.LeftChild = RemoveHelper(node.LeftChild, value);
            }
            else
            {
                if (IsRed(node.LeftChild) && IsRed(node.RightChild))
                {
                    FlipColor(node);
                }


                if (IsRed(node.LeftChild))
                {
                    node = RotateRight(node);
                }

                if (Comparer.Compare(node.Value, value) < 0)
                {
                    if (node.LeftChild == null && node.RightChild == null)
                    {
                        node = null;
                        Count--;
                        return node;
                    }
                    else if (node.RightChild != null)
                    {
                        node.Value = Minimum(node.RightChild).Value;
                        node.RightChild = RemoveHelper(node.RightChild, node.Value);
                    }
                }
                else
                {
                    if (!IsRed(node.RightChild) && !IsRed(node.RightChild.RightChild))
                    {
                        node = MoveRedRight(node);
                    }
                    node.RightChild = RemoveHelper(node.RightChild, value);
                }

            }


            return FixUp(node);
        }

        private Node<T> MoveRedRight(Node<T> node)
        {
            FlipColor(node);
            if (IsRed(node.LeftChild) && IsRed(node.LeftChild.LeftChild))
            {
                node = RotateRight(node);
                FlipColor(node);
            }

            return node;
        }

        private Node<T> MoveRedLeft(Node<T> node)
        {
            FlipColor(node);

            if (IsRed(node.RightChild.LeftChild))
            {
                node = RotateLeft(RotateRight(node));
                FlipColor(node);
            }


            if (IsRed(node.RightChild.RightChild))
            {
                node = RotateLeft(node);
            }

            return node;
        }

        private Node<T> FixUp(Node<T> node)
        {

            if (IsRed(node.RightChild))
                node = RotateLeft(node);

            if (IsRed(node.LeftChild) && IsRed(node.LeftChild.LeftChild))
            {
                node = RotateRight(node);
            }

            if (IsRed(node.LeftChild) && IsRed(node.RightChild))
            {
                FlipColor(node);
            }

            if (node.LeftChild != null && IsRed(node.LeftChild.RightChild)
                && !IsRed(node.LeftChild.LeftChild))
            {
                node.LeftChild = RotateRight(node.LeftChild);
                if (IsRed(node.LeftChild) && node.LeftChild.LeftChild.IsRed)
                {
                    node = RotateRight(node);
                }

            }

            return node;
        }

        public Node<T> Minimum(Node<T> current)
        {

            while (current.LeftChild != null)
            {
                current = current.LeftChild;
            }
            return current;
        }

        public Node<T> Maximum(Node<T> current)
        {
            while (current.RightChild != null)
            {
                current = current.RightChild;
            }
            return current;
        }

        public bool Contains(Node<T> node, T value)
        {
            if (Comparer.Compare(node.Value, value) == 0)
            {
                return true;
            }
            else if(node == null)
            {
                return false;
            }

            if (Comparer.Compare(node.Value, value) < 0)
            {
                return Contains(node.LeftChild, value);
            }
            else
            {
                return Contains(node.RightChild, value);
            }
        }

        //wish I didn't have to make two but makes readability better
        //using only IsRed brings problems when checking whether node != null && node.IsBlack
        //since null node will then bring true if check !IsRed
        //black is null so lies
        private bool IsRed(Node<T> node)
        {
            if (node == null) return false;
            return node.IsRed;
        }

        public void Clear()
        {
            Root = null;
        }

    }
}
