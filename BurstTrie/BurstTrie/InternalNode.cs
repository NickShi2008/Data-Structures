using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurstTries
{
    internal class InternalNode : BurstNode
    {
        BurstNode[] array = new BurstNode[27];
        BurstTrie Trie { get; set; }
        public InternalNode(BurstTrie trie)
            : base(trie)
        {
            Trie = trie;
            for(int i = 0; i < array.Length; i++)
            {
                array = new ContainerNode(trie);
            }
        }

        public override int Count => throw new NotImplementedException();



        public override BurstNode Insert(string value, int index)
        {
            if (value.Length - 1 < index)
            {
                //Remember to ask if index should be reset when Nil index bursts
                array[0].Insert(value, index);
            }
            for(int i = 1; i < array.Length; i++)
            {
               
            }
        }

        public override BurstNode? Remove(string value, int index, out bool success)
        {
            throw new NotImplementedException();
        }

        public override BurstNode? Search(string prefix, int index)
        {
            throw new NotImplementedException();
        }

        internal override void GetAll(List<string> output)
        {
            throw new NotImplementedException();
        }
    }
}
