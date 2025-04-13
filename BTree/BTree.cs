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
            while (i < current.Keys.Count && key.CompareTo(current.Keys[i]) > 0)
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
                    if (current.Children.Count != 0)
                    {
                        int c = 0;
                        while ( c < current.Children.Count)
                        {
                            if (key.CompareTo(current.Children[c].Keys[current.Children[c].Keys.Count - 1]) < 0)
                            {
                                SplitNode = SplitFinder(key, current.Children[c]);
                                break;
                            }
                            else
                            {
                                SplitNode = null;
                                c++;
                            }
                                
                        }
                        
                    }
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

        //removed redudant Find Split, just check if root to large as split can occur within add
        //now only other case comes from rootCount
  
        public void BetterAdd(T key)
        {
            if(Root.Keys.Count == 3)
            {

                //need place holder so Split works for all cases and allows middle to parent
                BTreeNode<T> placeHolder = new BTreeNode<T>();
                placeHolder.Children.Add(Root);

                Split(placeHolder, 0);
                Root = placeHolder;

            }

            BetterAddHelper(key, Root);
            Count++;
        }

        //Now compares the value to find path instead of overused boolean checks
        //however if the node is about to pass to children with max space, the split will run
        //runs till leaf reached
        public void BetterAddHelper(T key, BTreeNode<T> current)
        {
            int i = 0;
            while (i < current.Keys.Count && key.CompareTo(current.Keys[i]) > 0)
            {
                i++;
            }
            //recursive end check for leaf 
            if (current.Children.Count == 0)
            {

                current.Keys.Insert(i, key);
            }
            else
            {
                //before passing to node check if amount to large, removes problem with re checking parent
                //since we will go to children as soon as function happens
                if (current.Children[i].Keys.Count == 3)
                {
                    Split(current, i);
                    if (key.CompareTo(current.Keys[i]) > 0)
                    {
                        i++;
                    }
                }
                BetterAddHelper(key,current.Children[i]);
            }
        }

        //no need for splitfind and just made split function to reduce redundacy
        //intakes parent that has child too large along with what index the child is in
        //seperate right then remove from range
        //check for new child node if they have children then it must be max size
        //due to split cases and pass in incase of root, child must have either 0 or 4 children
        //if 4 and parent 3 then tree goes 1 -2 with 2 children on each which is what check does
        //then insert the middle as top parent which either is 0 or right after key is greater than other key
        //now add parent right to child, can't do before due to check which interrupts child.children
        public void Split(BTreeNode<T> parent, int index)
        {
            BTreeNode<T> child = parent.Children[index];

            BTreeNode<T> right = new BTreeNode<T>();
            right.Keys.Add(child.Keys[2]);
            T middle = child.Keys[1];
            child.Keys.RemoveRange(1, 2);
            if (child.Children.Count > 0)
            {
                right.Children.Add(child.Children[2]);
                right.Children.Add(child.Children[3]);
                child.Children.RemoveRange(2, 2);
            }

            parent.Keys.Insert(index, middle);
            
            parent.Children.Insert(index + 1, right);

        }
    }


}
