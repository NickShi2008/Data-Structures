namespace LRUCacher
{
    public class LRUCache<TKey,TValue>:ICache<TKey,TValue>
    {
        public LinkedList<KeyValuePair<TKey, TValue>> linkedList { get; private set; }
        public Dictionary<TKey, TValue> dict { get; private set; }

        public LRUCache()
        {
            linkedList = new LinkedList<KeyValuePair<TKey, TValue>>();
            dict = new Dictionary<TKey, TValue>();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (dict[key] != null)
            {
                value = dict[key];
                return true;
            }

            value = default(TValue);
            return false;
        }

        public void Put(TKey key, TValue value)
        {
            if (key != null)
            {
                dict[key] = value;
                linkedList.Remove(new KeyValuePair<TKey, TValue>(key, value));
            }
            else
            {
                dict.Add(key, value); 
            }

            linkedList.AddFirst(new KeyValuePair<TKey, TValue>(key, dict[key]));

        }
    }
}
