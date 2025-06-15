using SortedSet;

namespace SortedSetTester
{
    public class SortedSetTest
    {

        class Comparer : IComparer<int>
        {
            public int Compare(int a, int b)
            {
                return a.CompareTo(b);
            }
        }
        [Fact]
        public void InOrderTraverseTest()
        {
            
            RedBlackTree<int> tree = new RedBlackTree<int>(new Comparer());
            int[] arr = [50, 40, 60, 30, 70, 80, 20, 10, 90, 100];
            //  5, 110, 25, 35, 45, 55, 65, 75, 85, 95,
            // 105, 115, 120, 15, 1, 36, 46, 56, 66, 76];
            for (int i = 0; i < arr.Length; i++)
            {
                tree.Add(arr[i]);
            }
            Stack<int> stack = new Stack<int>();
            List<int> nodes = new List<int>();
            List<int> list = tree.InOrderTraversal(tree.Root, stack, nodes);
            ;
        }
        [Fact]
        public void SortedSetTests()
        {
            var silly = new Comparer();
            SortedSet.SortedSet<int> set = new SortedSet.SortedSet<int>(silly);
            int[] arr = [50, 40, 60, 30, 70, 80, 20, 10, 90, 100];
            //  5, 110, 25, 35, 45, 55, 65, 75, 85, 95,
            // 105, 115, 120, 15, 1, 36, 46, 56, 66, 76];
            for (int i = 0; i < arr.Length/2; i++)
            {
                set.Add(arr[i]);
            }
            set.Clear();
            ;
            set.AddRange(arr);
            ;
            int ceiling = set.Ceiling(35);
            ;
            int floor = set.Floor(35);
            ;

            Assert.True(set.Contains(10));
            ;

            SortedSet.SortedSet<int> setTwo = new SortedSet.SortedSet<int>(silly);
            int[] arrTwo = [55, 45, 60, 35, 70, 85, 25];
            setTwo.AddRange(arrTwo);

            SortedSet.SortedSet<int> intersection = new SortedSet.SortedSet<int>(silly);
            intersection = (SortedSet.SortedSet<int>)set.Intersection(setTwo);
            
            SortedSet.SortedSet<int> union = new SortedSet.SortedSet<int>(silly);
            union = (SortedSet.SortedSet<int>)set.Union(setTwo);
            ;
        }
    }
}