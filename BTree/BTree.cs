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
            int i = 0;
            while (i < current.Keys.Count && key.CompareTo(current.Keys[i]) < 0)
            {
                i++;
            }

            if (i < current.Keys.Count && current.Keys[i].CompareTo(key) == 0) return current;

            if (current.Children.Count == 0) return null;


            return SearchHelper(key, current.Children[i]);
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
            while (SplitNode != null)
            { 

                if(!SplitNode.Equals(Root))
                { 
                    BTreeNode<T> current = Root;
                    int count = 0;
                    while (!current.Children.Contains(SplitNode))
                    {
                        if (SplitNode.Keys[1].CompareTo(current.Keys[count]) < 0)
                        {
                            current = current.Children[count];
                            count = 0;  
                        }
                        else if(SplitNode.Keys[1].CompareTo(current.Keys[count]) > 0 && current.Keys.Count == count + 1)
                        {
                            current = current.Children[count + 1];
                            count = 0;

                        }
                        else
                        {
                            count++;
                        }
                        
                    }
                    
                    current.Children.Remove(SplitNode);

                    int i = 0;
                    while (SplitNode.Keys[1].CompareTo(current.Keys[i]) > 0)
                    {
                        i++;
                        if(i == current.Keys.Count)
                        {
                            
                            break;
                        }    
                    }
                    current.Keys.Insert(i, SplitNode.Keys[1]);
                    current.Children.Insert(i, new BTreeNode<T>(SplitNode.Keys[0]));
                    
                    current.Children.Insert(i + 1, new BTreeNode<T>(SplitNode.Keys[2]));
                    if (SplitNode.Children.Count != 0)
                    {
                        List<BTreeNode<T>> tree = SplitNode.Children;
                        for (int j = 0; j < tree.Count; j++)
                        {
                            if (j < 2)
                            {
                                Root.Children[i].Children.Insert(j, tree[j]);
                            }
                            else
                            {
                                Root.Children[i + 1].Children.Insert(j - 2, tree[j]);
                            }
                        }
                    }

                    SplitNode = SplitFinder(key, current);
                }
                else
                {
                    Root = new BTreeNode<T>();
                    Root.Keys.Insert(0, SplitNode.Keys[1]);


                    Root.Children.Insert(0, new BTreeNode<T>(SplitNode.Keys[0]));

                    Root.Children.Insert(1, new BTreeNode<T>(SplitNode.Keys[2]));

                    if (SplitNode.Children.Count != 0)
                    {
                        List<BTreeNode<T>> tree = SplitNode.Children;
                        for(int i = 0; i < tree.Count; i++)
                        {
                            if(i < 2)
                            {
                                Root.Children[0].Children.Insert(i, tree[i]);
                            }
                            else
                            {
                                Root.Children[1].Children.Insert(i - 2, tree[i]);
                            }
                        }
                    }
                    SplitNode = SplitFinder(key, Root);
                }
                
            }
            AddHelper(key, Root);
            Count++;
        }

        private BTreeNode<T> AddHelper(T key, BTreeNode<T> current)
        {
            BTreeNode<T> node = new BTreeNode<T>();
            for (int i = 0; i < current.Keys.Count; i++)
            {
                int value = key.CompareTo(current.Keys[i]);
                if (value < 0 && current.Children.Count != 0)
                {
                    node = AddHelper(key, current.Children[i]);
                    break;
                }
                else if (value > 0 && current.Children.Count != 0 && i == current.Keys.Count - 1)
                {
                    node = AddHelper(key, current.Children[i + 1]);
                    break;
                }
                else if (value < 0)
                {
                    current.Keys.Insert(i, key);
                    return current;
                }
                else if (value > 0 && current.Keys.Count < current.Keys.Capacity
                    && i == current.Keys.Count - 1)
                {
                    current.Keys.Insert(i  + 1, key);
                    return current;
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
                    break;
                }
                else if(value > 0 && i == current.Keys.Count - 1 && !isCountEqualCap)
                {
                    node = SplitFinder(key, current.Children[i + 1]);
                    break;
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
