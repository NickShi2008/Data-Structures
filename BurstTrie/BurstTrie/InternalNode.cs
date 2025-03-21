using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurstTries
{
    internal class InternalNode : BurstNode
    {
        BurstNode[] array;
        BurstTrie Trie { get; set; }
        public InternalNode(BurstTrie trie, char min, char max)
            : base(trie)
        {
            Trie = trie;
            for(int i = 0; i < array.Length; i++)
            {
                array[i] = new ContainerNode(trie, min, max);
            }
            array = new BurstNode[max - min];
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
            int i = value[index].GetHashCode() % 26 + 1;
            array[i].Remove(value, index, out success);
            if(success)
                return this;
            return null;
            
        }

        public override BurstNode? Search(string prefix, int index)
        {
            int i = prefix[index].GetHashCode() % 26 + 1;
            return array[i].Search(prefix, index);
        }

        internal override void GetAll(List<string> output)
        {
            List<List<string>> container = new List<List<string>>(27);
            for (int i = 0; i < array.Length; i++)
            {
                array[i].GetAll(container[i]);
            }

            foreach (var list in container)
            {
                foreach(var val in list)
                {
                    output.Add(val);
                }
            }
        }
    }
}
