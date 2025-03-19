using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurstTries
{
    public class ContainerNode : BurstNode
    {
        BinarySearchTree<string> bstTree;
        BurstTrie Trie { get; set; }
        public ContainerNode(BurstTrie trie) 
           : base(trie)
        {
            Trie = trie;
        }

        public override int Count => bstTree.Count;
        private int burstSize = 5;

        public override BurstNode Insert(string value, int index)
        {
            if(index == value.Length - 1)
            {
                if(Count > burstSize)
                {
                    InternalNode internalNode = new InternalNode(Trie);
                    while(bstTree.Count > 0)
                    {
                        internalNode.Insert(bstTree.Root.Value, index);
                        bstTree.Delete(bstTree.Root.Value);
                    }
                }
                return this;
            }
            bstTree.Insert(value);
            return Insert(value, 1);
        }

        public override BurstNode? Remove(string value, int index, out bool success)
        {
            success = bstTree.Search(bstTree.Root, value) != null;
            if (success)
            {
                bstTree.Delete(value);
                return this;
            }
            return null;
        }

        public override BurstNode? Search(string prefix, int index)
        {
            if(bstTree.Search(bstTree.Root, prefix) != null)
                return this;
            return null;
        }

        internal override void GetAll(List<string> output)
        {
            BSTNode<string> current = new BSTNode<string>();
            Queue<string> tracker = new Queue<string>();
            Stack<BSTNode<string>> returner = new Stack<BSTNode<string>>();
            InOrder(bstTree.Root, tracker, returner);
            output = tracker.ToList<string>();
        }

        private BSTNode<string> InOrder(BSTNode<string> current, Queue<string> trackQueue, Stack<BSTNode<string>> returnStack)
        {
            trackQueue.Enqueue(current.Value);

            if(current.LeftChild == null && current.RightChild == null)
            {
                current = InOrder(returnStack.Pop(), trackQueue, returnStack);
            }

            if (current.LeftChild != null && current.RightChild != null)
            {
                returnStack.Push(current.RightChild);
                current = InOrder(current.LeftChild, trackQueue, returnStack);
            }
            else if(current.LeftChild != null)
            {
                current = InOrder(current.LeftChild, trackQueue, returnStack);
            }
            else if(current.RightChild != null)
            {
                current = InOrder(current.RightChild, trackQueue, returnStack);
            }

            return current;
        }
    }
}
