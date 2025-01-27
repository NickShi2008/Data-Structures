using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionFind
{
    public class QuickFind<T>
    {
        private int[] sets;
        private Dictionary<T, int> map;

        public QuickFind(IEnumerable<T> items)
        {
            map = new Dictionary<T, int>();
            for (int i = 0; i < items.Count();)
            {
                foreach (var item in items)
                {
                    map.Add(item, i++);
                }
            }
            sets = new int[items.Count()];
        }

        public int Find(T p) => sets[map[p]];
        public bool Union(T p, T q)
        {
            if (!map.ContainsKey(p) || !map.ContainsKey(q)) return false;
            if (AreConnected(p, q)) return true;

            int indexToRemove = sets[map[p]];
            for (int i = 0; i < sets.Length; i++)
            {
                if (sets[i] == indexToRemove) sets[i] = map[q];
            }
            return true;
        }
        public bool AreConnected(T p, T q)
        {
           return Find(p) == Find(q);
        }

    }
}
