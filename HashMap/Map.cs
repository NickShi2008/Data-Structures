using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    public class Map<TKey, TValue> : IDictionary<TKey, TValue>
    {
        public TValue this[TKey key]
        {
            get
            {
                foreach(var pair in HashArray[GetHashCode(key)])
                {
                    if(pair.Key.Equals(key))
                    {
                        return pair.Value;
                    }
                }
                   
                throw new KeyNotFoundException("Key: "+key+" Not Found");
            }
            set
            {
                Add(key, value);
            }
        }

        private LinkedList<KeyValuePair<TKey, TValue>>[] HashArray = new LinkedList<KeyValuePair<TKey, TValue>>[10];
        public ICollection<TKey> Keys => throw new NotImplementedException();

        public ICollection<TValue> Values => throw new NotImplementedException();

        public int Count => HashArray.Length;

        public bool IsReadOnly => false;

        int GetHashCode(TKey key)
        {
            return Math.Abs(key.GetHashCode() % Count);
        }
        public void Add(TKey key, TValue value)
        {
            if(key == null || value == null) throw new ArgumentNullException(nameof(key));

            int hash = GetHashCode(key);

            if (HashArray[hash] == null)
            {
                HashArray[hash] = new LinkedList<KeyValuePair<TKey, TValue>>([new KeyValuePair<TKey, TValue>(key, value)]);
            }
            else if (HashArray[hash].Count >= Count)
            {
                ReHash();
            }
            else
            {
                //Check if Linkedlist at index already contains the needed to be added keyvalue pair
                HashArray[hash].AddFirst(!HashArray[hash].Contains(new KeyValuePair<TKey, TValue>(key, value))
                    ? new KeyValuePair<TKey, TValue>(key, value) : throw new Exception("Already Exists"));
            }
        }

        private void ReHash()
        {
            LinkedList<KeyValuePair<TKey, TValue>>[] resizedArray = HashArray;
            HashArray = new LinkedList<KeyValuePair<TKey, TValue>>[resizedArray.Length * 2];

            foreach (var linkedList in resizedArray)
            {
                foreach(var pair in linkedList)
                {
                    Add(pair.Key, pair.Value);
                }
            }

        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (item.Key == null || item.Value == null) throw new ArgumentNullException(nameof(item.Key));
            int hash = GetHashCode(item.Key);

            if (HashArray[hash] == null)
                HashArray[hash] = new LinkedList<KeyValuePair<TKey, TValue>>([new KeyValuePair<TKey, TValue>(item.Key, item.Value)]);
            else if (HashArray[hash].Count >= Count)
                ReHash();
            else
                //Check if Linkedlist at index already contains the needed to be added keyvalue pair
                HashArray[hash].AddFirst(!HashArray[hash].Contains(new KeyValuePair<TKey, TValue>(item.Key, item.Value))
                    ? new KeyValuePair<TKey, TValue>(item.Key, item.Value) : throw new Exception("Already Exists"));
        }

        public void Clear()
        {
            HashArray = new LinkedList<KeyValuePair<TKey, TValue>>[HashArray.Length];
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if(item.Key == null || item.Value == null) throw new NullReferenceException();

            return HashArray[GetHashCode(item.Key)].Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            if (key == null) throw new NullReferenceException(nameof(key));

            foreach (var pair in HashArray[GetHashCode(key)])
            {
                if (pair.Key.Equals(key)) return true;
            }

            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if(array == null) throw new ArgumentNullException(nameof(array));
            else if(arrayIndex >= array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            List<KeyValuePair<TKey, TValue>> list = [];

            for(int i = arrayIndex; i < HashArray.Length; i++)
            {
                foreach(var pair in HashArray[i])
                {
                    list.Add(pair);
                }
            }

          
            array = list.ToArray(); 
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            else if (!ContainsKey(key)) throw new Exception("Doesn't exist");


            foreach (var pair in HashArray[GetHashCode(key)])
            {
                if (key.Equals(pair.Key))
                {
                    HashArray[GetHashCode(key)].Remove(pair);
                    return true;
                }
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if(item.Equals(null)) throw new NullReferenceException(nameof(item)); 
            return HashArray[GetHashCode(item.Key)].Remove(item);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (HashArray[GetHashCode(key)] == null)
            {
                throw new NullReferenceException("Hash Array Linked List is null");
            }

            foreach (var pair in HashArray[GetHashCode(key)])
            {
                if (pair.Key.Equals(key))
                {
                    value = pair.Value;
                    return true;
                }
            }

            value = default;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        
    }
}
