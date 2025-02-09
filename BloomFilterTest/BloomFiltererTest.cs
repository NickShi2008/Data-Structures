
using BloomFilter;

namespace BloomFilterTest
{
    public class BloomFiltererTest
    {
        [Fact]
        public void InsertTest()
        {
            BloomFilter.BloomFilter<bool> filter = new BloomFilter<bool>(10);
            filter.Insert(true);

            Assert.True(filter.bitArray[2]);
        }

       

        [Fact]
        public void ContainsTest()
        {
            BloomFilter<bool> filter = new BloomFilter<bool>(10);
            filter.Insert(true);

            Assert.True(filter.ProbablyContains(true));
        }
    }
}