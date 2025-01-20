
using System.Collections.Immutable;
using System.Drawing;
using System.Net.Sockets;

namespace NonComparative_Sorts
{
    public class Program
    {

        static void Main(string[] args)
        {
            int min = -10;
            int max = 10;

            Random random = new Random();
            int randInt = random.Next(min, max);
            List<int> test = new List<int>();
            for (int i = -20; i < 21; i++)
            {
                test.Add(i);
                // test.Add(randInt);
                //randInt = random.Next(0, 10);
            }

            test = Shuffle(test);


            // int[] count = new int[max - min];

            /*   foreach(int integer in test)
               {
                   count[integer]++;
               }*/

            //  CountingSort(test);
            //BucketSort(test, 5);
            RadixSort(test);
            foreach (var val in test)
                Console.WriteLine(val);
        }

        public static List<T> Shuffle<T>(List<T> list) where T : IComparable
        {
            int n = list.Count;
            Random rand = new Random();
            while (n > 1)
            {
                int k = rand.Next(n--);
                T temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
            return list;
        }

        public static void CountingSort(List<int> list)
        {
            int min = list.Min();
            int max = list.Max();

            int counter = 0;
            int[] count = new int[max - min + 1];
            int[] sortedArray = new int[count.Length];

            foreach (int integer in list)
            {
                count[integer - min]++;
            }

            for (int i = 0; i < count.Length; i++)
            {
                for (int j = 0; j < count[i]; j++)
                {
                    list[counter++] = i + min;
                }
            }

        }

        public static void BucketSort(List<int> list, int numOfBuckets)
        {
            int min = list.Min();
            int max = list.Max();

            int size = max - min + 1;
            List<int[]> buckets = new List<int[]>();

            for(int i = 0; i < numOfBuckets; i++)
            {
                buckets.Add(new int[(size / numOfBuckets) + (i < size % numOfBuckets ? 1 : 0)]);
            }

            int[] IndicesForBuckets = new int[numOfBuckets];

            foreach (int integer in list)
            {
                int bucketIndex = (integer - min) * numOfBuckets / size;

                buckets[bucketIndex][IndicesForBuckets[bucketIndex]++] = integer;
            }

            int count = 0;
            foreach (var bucket in buckets)
            {
                Array.Sort(bucket);
                foreach (var value in bucket)
                {
                    list[count++] = value;
                }
            }
           
        }

        public static void RadixSort(List<int> list)
        {
            int min = list.Min();
            int max = list.Max();
            int size = max - min + 1;

            int[] buckets = new int[10];
            int[] sortedList = new int[list.Count];


            for(int digitPlace = 0; digitPlace < max.ToString().Length; digitPlace++)
            {
                foreach(int integer in list)
                {
                   buckets[(integer - min)/ ((int) Math.Pow(10, digitPlace)) % 10 ]++;
                }

                for (int j = 1; j < buckets.Length; j++)
                {
                    buckets[j] += buckets[j - 1];
                }

                for (int i = list.Count - 1; i >= 0; i--)
                {
                    sortedList[--buckets[(list[i] - min) / (int) Math.Pow(10, digitPlace) % 10]] = list[i];
                }


                Array.Clear(buckets);

                int count = 0;
                foreach (var value in sortedList)
                {
                    list[count++] = value;
                }
            }

            
        }


    }
}
