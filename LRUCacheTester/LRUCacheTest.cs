using LRUCacher;
namespace LRUCacheTester
{
    public class LRUCacheTest
    {
        [Fact]
        public void TryGetValueTest()
        {
            LRUCache<string, int> lRUCache = new LRUCache<string, int>();
            lRUCache.Put("a", 5);

            int value = 1;
            Assert.True(lRUCache.TryGetValue("a", out value));
            Assert.Equal(5, value);
        }

        [Fact]
        public void PutTest()
        {
            LRUCache<string, int> lRUCache = new LRUCache<string, int>();
            lRUCache.Put("a", 1);

            Assert.True(lRUCache.dict.ContainsKey("a"));
            Assert.False(lRUCache.dict.ContainsKey("b"));
            Assert.True(lRUCache.linkedList.Contains(new KeyValuePair<string, int>("a",1)));
        }
    }
}