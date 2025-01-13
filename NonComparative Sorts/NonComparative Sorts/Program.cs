using static System.Net.Mime.MediaTypeNames;

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
            for (int i = -20; i < 20; i++)
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

          // CountingSort(test);
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

            for(int i = 0; i < count.Length; i++)
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

            List<int[]> buckets = new List<int[]>();

            int[] count = new int[max - min + 1];
            int[] sortedArray = new int[count.Length];

            
        }


    }
}
