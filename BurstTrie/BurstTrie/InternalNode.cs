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
                array[i] = new ContainerNode(trie);
            }
        }

        public override int Count => array.Count();



        public override BurstNode Insert(string value, int index)
        {
            if (value.Length - 1 < index)
            {
                //Remember to ask if index should be reset when Nil index bursts
                array[0].Insert(value, index);
            }
            else
            {
                array[value[index].GetHashCode() % 26 + 1].Insert(value,index);
            }
            return this;
        }

        public override BurstNode? Remove(string value, int index, out bool success)
        {
            array[value[index].GetHashCode() % 26 + 1].Search(value, index);
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
