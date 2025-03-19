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

        public override BurstNode Insert(string value, int index)
        {
            if(index == value.Length - 1)
            {
                if(Count > 5)
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
            bstTree.Delete(value);

            return this;
        }

        public override BurstNode? Search(string prefix, int index)
        {
            bstTree.Search(bstTree.Root, prefix);
            return this;
        }

        internal override void GetAll(List<string> output)
        {
            throw new NotImplementedException();
        }
    }
}
