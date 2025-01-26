using HashMap;
using Newtonsoft.Json.Linq;


namespace HashMapTest
{
    public class MapTest
    {
        [Fact]
        public void FunctionTest()
        {
            Map<string, int> map = new Map<string, int>();

            map.Add("a", 1);
            Assert.True(map["a"] == 1);
            Assert.True(map.ContainsKey("a"));
            map.Add("b", 2);
            map.Add(new KeyValuePair<string, int>("c", 3));
            Assert.True(map.Count == 3);
            Assert.True(map.Remove("b"));
            Assert.False(map.Contains(new KeyValuePair<string, int>("b", 2)));
            Assert.False(map.Remove((new KeyValuePair<string, int>("c", 4))));

            map.Clear();
            int a = 0;
            map.Add("a", 0);
            Assert.True(map.TryGetValue("a",out a));
            Assert.True(map.ContainsKey("a"));
            Assert.False(map.Contains(new KeyValuePair<string, int>("a", 1)));

            KeyValuePair<string, int>[] copy = new KeyValuePair<string, int>[map.Count];
            map.CopyTo(copy, 0);
            Assert.True(copy[0].Key == "a");

            ICollection<string> strings = map.Keys;
            ICollection<int> ints = map.Values;
            Assert.True(strings.Contains("a"));
            Assert.True(ints.Contains(0));
        }

        [Fact]
        public void EnumeratorTest()
        {
            Map<string, int> map = new Map<string, int>();

            map.Add("a", 1);
            map.Add("b", 2);
            map.Add("c", 3);
            Assert.True(map.GetEnumerator().MoveNext());
            Assert.True(map.GetEnumerator().MoveNext());
            Assert.True(map.GetEnumerator().MoveNext());
        }
    }
}