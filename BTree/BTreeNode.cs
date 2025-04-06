using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTreeProj
{
    public class BTreeNode<T> where T : IComparable<T>
    {
        public List<T> Keys {  get; set; }
        public List<BTreeNode<T>> Children { get; set; }

        public BTreeNode(T val)
        {
            Keys = new List<T>(3);
            Keys.Add(val);
            Children = new List<BTreeNode<T>>(4);
        }

        public BTreeNode()
        {
            Keys = new List<T>(3);
            Children = new List<BTreeNode<T>>(4);
        }

        
    }
}
