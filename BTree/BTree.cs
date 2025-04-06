using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BTreeProj
{
    public class BTree<T> where T : IComparable<T>
    {
        public BTreeNode<T> Root { get; private set; }
        public int Count {  get; private set; }

        public BTree()
        {
            Count = 0;
            Root = new BTreeNode<T>();
        }

        public BTreeNode<T> Search(T key)
        {
            return SearchHelper(key, Root);
        }

        private BTreeNode<T> SearchHelper(T key, BTreeNode<T> current)
        {
            BTreeNode<T> node = new BTreeNode<T>();

            for (int i = 0; i < current.Keys.Count; i++)
            {
                if (key.CompareTo(current.Keys[i]) < 0)
                {
                    if (current.Children[i] == null)
                    {
                        return current;
                    }
                    node = SearchHelper(key, current.Children[i]);
                }
                else if (i == current.Keys.Count - 1)
                {
                    if (current.Children[current.Children.Count - 1] == null)
                    {
                        return current;
                    }
                    node = SearchHelper(key, current.Children[current.Children.Count - 1]);
                }
            }
            return node;
        }

        public void Add(T key)
        {
            if (Root.Keys.Count == 0)
            {
                Root.Keys.Add(key);
                Count++;
                return;
            }
            BTreeNode<T> SplitNode = SplitFinder(key, Root);
            if (SplitNode != null)
            {
                if(!SplitNode.Equals(Root))
                { 
                     BTreeNode<T> current = Root;
                    int count = 0;
                    while(!current.Children.Contains(SplitNode))
                    {
                        if (SplitNode.Keys[0].CompareTo(current.Keys[count]) < 0)
                        {
                            current = current.Children[count];
                        }
                        else if (count == current.Keys.Count && SplitNode.Keys[count].CompareTo(current.Keys[count]) > 0)
                        {
                            current = current.Children[count + 1];
                        
                        }
                        count++;
                    }
                    current.Children.Remove(SplitNode);
                    if (SplitNode.Keys[0].CompareTo(current.Keys[0]) < 0)
                    {
                        current.Keys.Insert(0, SplitNode.Keys[1]);

                        current.Children.Insert(0, new BTreeNode<T>(SplitNode.Keys[0]));
                        current.Children.Insert(1, new BTreeNode<T>(SplitNode.Keys[2]));
                    }
                    else
                    {
                        current.Keys.Insert(current.Keys.Count - 1, SplitNode.Keys[1]);

                        current.Children.Insert(current.Children.Count - 1, new BTreeNode<T>(SplitNode.Keys[0]));
                        current.Children.Insert(current.Children.Count, new BTreeNode<T>(SplitNode.Keys[2]));
                    }
                }
                else
                {
                    Root = new BTreeNode<T>();
                    Root.Keys.Insert(0, SplitNode.Keys[1]);

                    Root.Children.Insert(0, new BTreeNode<T>(SplitNode.Keys[0]));
                    Root.Children.Insert(1, new BTreeNode<T>(SplitNode.Keys[2]));
                }
               
            }
            AddHelper(key, Root);
            Count++;
        }
        

        //should not need to check for size conditions since that should be solved when entering
        private BTreeNode<T> AddHelper(T key, BTreeNode<T> current)
        {
            BTreeNode<T> node = new BTreeNode<T>();
            for (int i = 0; i < current.Keys.Count; i++)
            {
                int value = key.CompareTo(current.Keys[i]);
                if (value < 0 && current.Children.Count != 0)
                {
                    node = AddHelper(key, current.Children[i]);
                }
                else if (value > 0 && current.Children.Count != 0 && i == current.Keys.Count - 1)
                {
                    node = AddHelper(key, current.Children[i + 1]);
                }
                else if (value < 0)
                {
                    current.Keys.Insert(i, key);
                    break;
                }
                else if (value > 0 && current.Keys.Count < current.Keys.Capacity
                    && i == current.Keys.Count - 1)
                {
                    current.Keys.Insert(i  + 1, key);
                    break;
                }
            }
            return node;
        }

        private BTreeNode<T> SplitFinder(T key, BTreeNode<T> current)
        {
            if (current.Children.Count == 0 && current.Keys.Count != current.Keys.Capacity) return null;
            BTreeNode<T> node = new BTreeNode<T>();
            for (int i = 0; i < current.Keys.Count; i++)
            {
                int value = key.CompareTo(current.Keys[i]);
                bool isCountEqualCap = current.Keys.Count == current.Keys.Capacity;
                if (value < 0 && i < current.Keys.Count && !isCountEqualCap)
                {
                    node = SplitFinder(key, current.Children[i]);
                }
                else if(value > 0 && i == current.Keys.Count - 1 && !isCountEqualCap)
                {
                    node = SplitFinder(key, current.Children[i + 1]);
                }
                else if (value < 0 && isCountEqualCap)
                {
                    return current;
                }
                else if (value > 0 && i == current.Keys.Count - 1 && isCountEqualCap)
                {
                    return current;
                }

            }

            return node;
        }
    }
}
