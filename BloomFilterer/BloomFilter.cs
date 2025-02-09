using System.Collections;
using System.Drawing;

namespace BloomFilter
{
    public class BloomFilter<T>
    {
        public BitArray bitArray { get; private set; }
        private int M => bitArray.Length;
        private int K => HashFunctions.Count;
        private HashSet<Func<T,int>> HashFunctions { get; set; }

        
        public BloomFilter(int cap)
        {
            bitArray = new BitArray(cap);
            HashFunctions = new HashSet<Func<T, int>>();
            HashFunctions.Add(HashFuncOne);
            HashFunctions.Add(HashFuncTwo);
            HashFunctions.Add(HashFuncThree);
        }

        public void LoadHashFunc(Func<T, int> hashFunc)
        {
            HashFunctions.Add(hashFunc);
        }

        public void Insert(T item)
        {
            foreach (Func<T, int> function in HashFunctions)
            {
                bitArray[function(item)] = true;
            }
        }

        public bool ProbablyContains(T item)
        { 
            foreach (Func<T, int> function in HashFunctions)
            {
                if(bitArray[function(item)])
                {
                    return true;
                }
            }
            return false;
        }

        private int HashFuncOne(T item)
        {
            int value = item.GetHashCode();
            return value * 2 % M;
        }

        private int HashFuncTwo(T item)
        {
            int value = item.GetHashCode();
            return value * 3 % M;
        }

        private int HashFuncThree(T item)
        {
            int value = item.GetHashCode();
            return value * 5 % M;
        }
    }
}
