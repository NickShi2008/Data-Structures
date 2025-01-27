using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionFind
{
    public class QuickUnion<T>
    {
        private int[] parents;
        private Dictionary<T, int> map;

        public QuickUnion(IEnumerable<T> items)
        {
            map = new Dictionary<T, int>();
            for (int i = 0; i < items.Count();)
            {
                foreach (var item in items)
                {
                    map.Add(item, i++);
                }
            }
            parents = new int[items.Count()];
        }

        public int Find(T p)
        {
            int index = parents[map[p]];
            int nextIndex = parents[index];
            while (index != parents[nextIndex])
            {
                index = nextIndex;
                nextIndex = parents[nextIndex];
            }
            return index;
        }
        public bool Union(T p, T q)
        {
            if (!map.ContainsKey(p) || !map.ContainsKey(q)) return false;

            parents[map[p]] = parents[map[q]];
            return true;
        }
        public bool AreConnected(T p, T q)
        {
            return Find(p) == Find(q);
        }


    }
}
