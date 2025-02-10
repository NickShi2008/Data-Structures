using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCoding
{
    public class HuffmanCode
    {
        public PriorityQueue<HuffmanNode, int> priorityQueue;
        Dictionary<char, int> letterTracker;
        Dictionary<char, string> charConverter;
        public HuffmanCode()
        {
            priorityQueue = new PriorityQueue<HuffmanNode, int>();

            letterTracker = new Dictionary<char, int>();

        }

        public string Encode(string text)
        {
            foreach (char c in text)
            {
                letterTracker[c]++;
            }

            foreach (var i in letterTracker)
            {
                var huffynode = new HuffmanNode();
                huffynode.value = i.Value;
                huffynode.character = i.Key;
                priorityQueue.Enqueue(huffynode,huffynode.value);
            }

            //pop two lowest values or first two in queue
            //the two nodes combine into parent node of null with combined values
            //add to priority queue
            //repeat till 1 left

            while (priorityQueue.Count > 1)
            {
                HuffmanNode nodeOne = priorityQueue.Dequeue();
                HuffmanNode nodeTwo = priorityQueue.Dequeue();

                HuffmanNode parentNode = new HuffmanNode();
                parentNode.leftChild = nodeOne;
                parentNode.rightChild = nodeTwo;
                parentNode.value = nodeOne.value + nodeTwo.value;
                parentNode.character = default(char);

                priorityQueue.Enqueue(parentNode, parentNode.value);
            }

            

        }

        private void Search(HuffmanNode root, string current)
        {
            if (root.leftChild != null)
            {
                Search(root.leftChild,current+="0");
            }
            if (root.rightChild != null)
            {
                Search(root.rightChild,current+="1");
            }
            else
            {
                charConverter.Add(root.character, current);
            }
        }

        /*public string Decode(string text)
        {

        }*/
    }
}
