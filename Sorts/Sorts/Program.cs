namespace Sorts
{
    internal class Program 
    {
        static void Main(string[] args)
        {
            int[] mixedArray = { 5, 3, 7, 8, 2, 0, 9, 1, 6, 4 };
            int[] bubbleArray = BubbleSort(mixedArray);
            //int[] selectionArray = SelectionSort(mixedArray);
           // int[] insertionArray = InsertionSort(mixedArray);
           /* for (int i = 0; i < sortedArray.Length; i++)
            {
                Console.WriteLine(sortedArray[i]);
            }*/
        }

        public static T[] BubbleSort <T>(T[] array) where T : IComparable
        {
            T[] sortedArray = new T[array.Length];
            array.CopyTo(sortedArray, 0);
            for (int count = sortedArray.Length; count > 0; count--)
            {
                for (int i = 1; i < count; i++)
                {
                    if (sortedArray[i - 1].CompareTo(sortedArray[i]) > 0)
                    {
                        T temp = sortedArray[i - 1];
                        sortedArray[i - 1] = sortedArray[i];
                        sortedArray[i] = temp;
                    }
                  
                }
                for (int i = 0; i < sortedArray.Length; i++)
                {
                    Console.Write(sortedArray[i]);
                }
                Console.WriteLine();
            }
            return sortedArray;
            
        }

        public static int[] SelectionSort(int[] array)
        {
            int[] sortedArray = new int[array.Length];
            array.CopyTo(sortedArray, 0);

            for(int i = 0; i < sortedArray.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < sortedArray.Length; j++)
                {
                    if (sortedArray[min] > sortedArray[j])
                    {
                        min = j;  
                    }
                }
                int temp = sortedArray[min];
                sortedArray[min] = sortedArray[i];
                sortedArray[i] = temp;
                for (int count = 0; count < sortedArray.Length; count++)
                {
                    Console.Write(sortedArray[count]);
                }
                Console.WriteLine();
            }
            return sortedArray;
        }

        public static int[] InsertionSort(int[] array)
        {
            int[] sortedArray = new int[array.Length];
            array.CopyTo(sortedArray, 0);
           
            
            for(int i = 1; i < sortedArray.Length; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    if (sortedArray[j - 1] > sortedArray[j])
                    {
                        int temp = sortedArray[j - 1];
                        sortedArray[j - 1] = sortedArray[j];
                        sortedArray[j] = temp;
                    }
                }
                for (int count = 0; count < sortedArray.Length; count++)
                {
                    Console.Write(sortedArray[count]);
                }
                Console.WriteLine();
            }

            return sortedArray;

        }

    }

}
