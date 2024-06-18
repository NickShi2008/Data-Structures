using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;

namespace BinarySearchTreeCopy
{
    public class Program
    {
        static void Main(string[] args)
        {
            BinarySearchTree<int> tree = new BinarySearchTree<int>(null);
            tree.Insert(tree.Root, 5);
            tree.Insert(tree.Root, 3);
            tree.Insert(tree.Root, 4);
            tree.Insert(tree.Root, 2);
            tree.Insert(tree.Root, 9);
            tree.Insert(tree.Root, 7);
            tree.Insert(tree.Root, 3);
            tree.Insert(tree.Root, 6);
            
            
            tree.Insert(tree.Root, 12);
            tree.Insert(tree.Root, 8);
            tree.Insert(tree.Root, 1);

            Console.WriteLine(tree.Minimum(tree.Root).Value);
            Console.WriteLine(tree.Maximum(tree.Root).Value);
            Console.WriteLine(tree.Maximum(tree.Root.LeftChild).Value);

            Console.WriteLine(tree.isLeftChild(tree.Root));
            Console.WriteLine(tree.isLeftChild(tree.Root.LeftChild.LeftChild));
            Console.WriteLine(tree.isRightChild(tree.Root.RightChild.RightChild));
            Console.WriteLine(tree.isLeftChild(tree.Root.LeftChild.LeftChild));
            Console.WriteLine(tree.isRightChild(tree.Root.RightChild.LeftChild));

            // Console.WriteLine(tree.Search(tree.Root, 1).Value);
            Console.WriteLine(tree.Search(tree.Root, 5).Value);
            Console.WriteLine(tree.Search(tree.Root, 2).Value);
            Console.WriteLine(tree.Search(tree.Root, 8).Value);

            tree.Delete(tree.Root, 5);
          //  tree.Delete(tree.Root, 1);
            BinarySearchTree<string> obj = new BinarySearchTree<string>(null);
            obj.Insert(obj.Root,"A");
            obj.Insert(obj.Root,"B");
            obj.Insert(obj.Root,"C");
            obj.Insert(obj.Root,"D");
            obj.Insert(obj.Root,"E");
            obj.Insert(obj.Root,"F");
            Console.WriteLine(obj.Maximum(obj.Root).Value);
            Console.WriteLine(obj.Minimum(obj.Root).Value);
            obj.Delete(obj.Root,"C");
            ;

        }

        /// <summary>
        /// allows duplicate values  <para />
        /// will search inorder traversal
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class BinarySearchTree<T> where T : IComparable
        {
            public class Node<T> where T : IComparable
            {
                public T Value { get;  set; }
                public Node<T> Previous { get; set; }

                public Node<T> LeftChild { get; set; }
                public Node<T> RightChild { get; set; }
                public Node(T value, Node<T> previous, Node<T> leftChild, Node<T> rightChild)

                {
                    Value = value;
                    Previous = previous;
                    LeftChild = leftChild;
                    RightChild = rightChild;
                }

                public Node(T value, Node<T> previous)
                   : this(value, previous, null, null) { }
                public Node(T value)
                  : this(value, null, null, null) { }
            }


            public Node<T> Root { get; private set; }

            public BinarySearchTree(Node<T> current)
            {
                Root = current;

            }


            public Node<T> Search(Node<T> node, T value)
            {
                if (node == null)
                {
                    return null;
                }

                if (node.Value.Equals(value))
                {
                    return node;
                }

                Node<T> foundNode = Search(node.LeftChild, value);
                if (foundNode == null)
                {
                    foundNode = Search(node.RightChild, value);
                }

                return foundNode;
            }

            public Node<T> Minimum(Node<T> current)
            {

                while (current.LeftChild != null)
                {
                    current = current.LeftChild;
                }
                return current;
            }

            public Node<T> Maximum(Node<T> current)
            {
                while (current.RightChild != null)
                {
                    current = current.RightChild;
                }
                return current;
            }

            public bool isLeftChild(Node<T> node)
            {
                if (node.Equals(Root))
                {
                    return false;
                }
                else if (node.Previous.LeftChild == node)
                {
                    return true;
                }
                return false;
            }

            public bool isRightChild(Node<T> node)
            {
                if (node.Equals(Root))
                {
                    return false;
                }
                else if (node.Previous.RightChild == node)
                {
                    return true;
                }
                return false;
            }

            public void Insert(Node<T> current, T target)
            {
                Node<T> temp = new Node<T>(target);
                if (Root == null)
                {
                    Root = new Node<T>(target);
                }
                else
                {
                    if (target.CompareTo(current.Value) < 0)
                    {
                        if (current.LeftChild != null)
                        {
                            Insert(current.LeftChild, target);
                        }
                        else
                        {
                            current.LeftChild = temp;
                        }

                    }
                    else
                    {
                        if (current.RightChild != null)
                        {
                            Insert(current.RightChild, target);
                        }
                        else
                        {
                            current.RightChild = temp;
                        }
                    }
                }
                temp.Previous = current;
            }

            //just realized that its supposed to replace the node not cut off the branch

            /// <summary>
            /// replaces deleted value with three possible conditions, and updates children
            /// </summary>
            /// <param name="check"></param>
            /// <param name="change"></param>
            public void DeleteHelper(Node<T> check, Node<T> change)
            {
                if (change != null)
                {
                    //checking for which branch to go up for replacement
                    if (isLeftChild(check))
                    {
                        check.Previous.LeftChild = change;
                    }
                    else
                    {
                        check.Previous.RightChild = change;
                    }
                    //replacing
                    change.Previous = check.Previous;
                    //make sure to check if the nodes have child
                    if (change != check)
                    {
                        // Re-assign left and right children of node
                        if (isLeftChild(change))
                        {
                            change.Previous.LeftChild = change.LeftChild;
                        }
                        else
                        {
                            change.Previous.RightChild = change.LeftChild;
                        }

                        change.LeftChild = check.LeftChild;
                        if (change.LeftChild != null)
                        {
                            change.LeftChild.Previous = change;
                        }

                        change.RightChild = check.RightChild;
                        if (change.RightChild != null)
                        {
                            change.RightChild.Previous = change;
                        }
                    }
                }
                else
                {
                    //no child case
                    if (isLeftChild(check))
                    {
                        check.Previous.LeftChild = null;
                    }
                    else if (isRightChild(check))
                    {
                        check.Previous.RightChild = null;
                    }
                }
            }

            public void Delete(Node<T> current, T value)
            {
                //check for null
                if (current == null)
                {
                    throw new NullReferenceException();
                }

                //check Generic so go either left or right 
                if (value.CompareTo(current.Value) < 0)
                {
                    Delete(current.LeftChild, value);
                }
                else if (value.CompareTo(current.Value) > 0)
                {
                    Delete(current.RightChild, value);
                }
                else
                {
                    //if equals
                    if (current.Equals(Root))
                    {
                        //only one node
                        if (Root.LeftChild == null && Root.RightChild == null)
                        {
                            Root = null;
                        }
                        else if (Root.RightChild == null)
                        {
                            //only leftChild available
                            Root = Root.LeftChild;
                            Root.Previous = null;
                        }
                        else if (Root.LeftChild == null)
                        {
                            //only rightChild is available
                            Root = Root.RightChild;
                            Root.Previous = null;
                        }
                        else
                        {
                            //if two children 
                            Node<T> child = Maximum(Root.LeftChild);
                            Root.Value = child.Value;
                            Delete(child, child.Value);
                        }
                    }
                    else if (current.LeftChild == null && current.RightChild == null)
                    {
                        DeleteHelper(current, null);
                    }
                    else if (current.LeftChild == null)
                    {
                        DeleteHelper(current, current.RightChild);
                    }
                    else if (current.RightChild == null)
                    {
                        DeleteHelper(current, current.LeftChild);
                    }
                    else
                    {
                        Node<T> child = Maximum(current.LeftChild);
                        current.Value = child.Value;
                        Delete(child, child.Value);
                    }
                }
            }





        }
    }
}
