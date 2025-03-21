using BurstTries;
using System.Text.Json.Serialization.Metadata;


namespace BurstTrieTest
{
    public class BurstTrieTester
    {
        const string url = "\\\\GMRDC1\\Folder Redirection\\Nicholas.Shi\\Documents\\Github\\Data-Structures\\BurstTrie\\BurstTrieTest\\words.txt";
        List<string> words = File.ReadAllLines(url).ToList();
        [Fact]
        public void InsertTest()
        {
            //todo linq
            //todo enumeration

            char min = words.Min(m => m.Min());
            char max = words.Max(m => m.Max());
            BurstTrie trie = new BurstTrie(min, max);

            trie.Insert("ca", 0);
            trie.Insert("car", 0);
            trie.Insert("cat", 0);
            trie.Insert("cast", 0);
            trie.Insert("caster", 0);
            trie.Insert("cats", 0);
            trie.Insert("cap", 0);
            trie.Insert("capybara", 0);
            trie.Insert("carnivore", 0);
            trie.Insert("cans", 0);
            trie.Insert("captivate", 0);

            List<string> container = new List<string>();
            trie.GetAll(container);
            Assert.Contains("ca", container);
            Assert.Contains("car", container);
            Assert.Contains("capybara", container);
        }

        [Fact]
        public void RemoveTest()
        {
            char min = words.Min(m => m.Min());
            char max = words.Max(m => m.Max());
            BurstTrie trie = new BurstTrie(min, max);
            trie.Insert("ca", 0);
            trie.Insert("car", 0);
            trie.Insert("cat", 0);
            trie.Insert("cast", 0);

            bool suc1 = false;
            bool suc2 = false;
            trie.Remove("car", 0, out suc1);
            trie.Remove("cast", 0, out suc2);
            List<string> container = new List<string>();
            trie.GetAll(container);
            Assert.True(suc1);
            Assert.True(suc2);
            Assert.DoesNotContain("car", container);
            Assert.DoesNotContain("cast", container);
        }

        [Fact]
        public void SearchTest()
        {
            char min = words.Min(m => m.Min());
            char max = words.Max(m => m.Max());
            BurstTrie trie = new BurstTrie(min, max);
            trie.Insert("ca", 0);
            trie.Insert("car", 0);
            trie.Insert("cat", 0);
            trie.Insert("cast", 0);

            Assert.True(trie.Search("ca", 0) != null);
            Assert.True(trie.Search("cat", 0) != null);
        }

    }
}