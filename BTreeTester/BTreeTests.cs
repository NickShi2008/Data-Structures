using BTreeProj;
namespace BTreeTester
{
    public class BTreeTests
    {
        [Fact]
        public void AddTest()
        {
            BTree<int> tree = new BTree<int>();
            tree.Add(1);
            tree.Add(2);
            tree.Add(3);
            tree.Add(0);
            tree.Add(4);
            tree.Add(5);
            tree.Add(6);
           // Assert.Contains(1, tree.Root.Keys);
           // Assert.Contains(2, tree.Root.Keys);
           // Assert.Contains(3, tree.Root.Keys);
            
        }

        [Fact]
        public void SearchTest()
        {

        }
    }
}