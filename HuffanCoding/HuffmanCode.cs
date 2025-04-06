using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HuffmanCoding
{
    public class HuffmanCode
    {
        public PriorityQueue<HuffmanNode, int> priorityQueue;
        Dictionary<char, int> letterTracker;
        Dictionary<char, string> charToString;
        Dictionary<char, byte> charToByte;
        Dictionary<byte, char> byteToChar;
        List<byte> code;
        public HuffmanCode()
        {
            priorityQueue = new PriorityQueue<HuffmanNode, int>();
            charToString = new Dictionary<char, string>();
            letterTracker = new Dictionary<char, int>();
            charToByte = new Dictionary<char, byte>();
            byteToChar = new Dictionary<byte, char>();

        }

        public string Encode(string text)
        {
            letterTracker.Clear();
            charToString.Clear();
            priorityQueue.Clear();
            foreach (char c in text)
            {
                if (!letterTracker.ContainsKey(c))
                {
                    letterTracker.Add(c, 0);
                }
                letterTracker[c]++;
            }

            foreach (var i in letterTracker)
            {
                var huffNode = new HuffmanNode();
                huffNode.value = i.Value;
                huffNode.character = i.Key;
                priorityQueue.Enqueue(huffNode,huffNode.value);
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

            Convert(priorityQueue.Peek(), "");

            string encodedString = "";
            foreach (char c in text)
            {
                encodedString += charToString[c];
            }

            return encodedString;
        }

       

        private void Convert(HuffmanNode node, string current)
        {
            if (node == null)
                return;

            if (node.leftChild == null && node.rightChild == null)
            {
                charToString[node.character] = current;
                return;
            }

            Convert(node.leftChild, current + "0");
            Convert(node.rightChild, current + "1");
        }

        public string Decode(string text)
        {
            
            string decodedString = "";
            HuffmanNode current = priorityQueue.Peek();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Equals('0'))
                {
                    current = current.leftChild;
                }
                else if (text[i].Equals('1'))
                {
                    current = current.rightChild;
                }

                if (current.leftChild == null && current.rightChild == null)
                {
                    decodedString += current.character;
                    current = priorityQueue.Peek();
                }

            }

            return decodedString;
        }

        public byte[] ByteEncode(string text)
        {
            charToByte.Clear();
            code = new List<byte>();
            Encode(text);


            foreach (var pair in charToString)
            {
                byte value = 0;
                int index = 0;
                string binaryString = pair.Value;

                // Read backwards to get least significant bit first
                for (int i = binaryString.Length - 1; i >= 0; i--)
                {
                    if (binaryString[i] == '1')
                    {
                        value |= (byte)(1 << (binaryString.Length - 1- index));
                    }
                    index++;
                }

                charToByte[pair.Key] = value;
            }

            byte val = 0;
            int bitIndex = 0;
            byte currentByte = 0;
            foreach (char c in text)
            {
              
                string binaryString = charToString[c];

                foreach(char bit in binaryString)
                {   
                    if(bit == '1')
                    {
                        currentByte |= (byte)(1 << bitIndex);
                    }

                    bitIndex++;

                    if (bitIndex == 8 || bitIndex == binaryString.Length)
                    {
                        code.Add(currentByte);
                        currentByte = 0;
                        bitIndex = 0;
                    }

                }


            }

            if (bitIndex > 0)
            {
                code.Add(currentByte);
            }

            return code.ToArray();
        }

        public string ByteDecode(byte[] code)
        {
            byteToChar.Clear();
            string text = "";


            foreach (var pair in charToByte)
            {
                byteToChar[pair.Value] = pair.Key;
            }

            foreach (byte val in code)
            {
                if (byteToChar.TryGetValue(val, out char character))
                {
                    text += character;
                }
            }
            return text;
        }
    }
}
