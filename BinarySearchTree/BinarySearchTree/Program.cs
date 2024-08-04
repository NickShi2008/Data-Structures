using System.Diagnostics.CodeAnalysis;

namespace BinarySearchTree
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            
        }
    }

    public class BinarySearchTree<T>  where T : IComparable<T>
    {
        T root;
        class Node<T>
        {
            private Node<T> leftChild;
            private Node<T> rightChild;

            public Node(int leftChild, int rightChild)
            {
             //   this.leftChild = leftChild;
           //     this.rightChild = rightChild;
            }
        }

        public BinarySearchTree()
        {
            
        }

    }

}
