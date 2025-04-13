using BTreeProj;
namespace BTreeTester
{
    public class BTreeTests
    {
        [Fact]
        public void AddTest()
        {
            BTree<int> tree = new BTree<int>();
            int[] arr = [50, 40, 60, 30, 70, 80, 20, 10, 90, 100,
                 5, 110, 25, 35, 45, 55, 65, 75, 85, 95,
                 105, 115, 120, 15, 1, 36, 46, 56, 66, 76];
            for (int i = 0; i < arr.Length; i++)
            {
                tree.Add(arr[i]);
            }
            Assert.Contains(50, tree.Root.Keys);
            //Assert.Contains(20, tree.Root.Keys);
            //Assert.Contains(6, tree.Root.Keys);
            BTreeNode<int> check = tree.Search(76);

        }

        [Fact]
        public void SearchTest()
        {

        }
    }
}