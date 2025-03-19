using BurstTries;


namespace BurstTrieTest
{
    public class BurstTrieTester
    {
        [Fact]
        public void InsertTest()
        {
            //todo linq
            //todo enumeration
            const string url = "\\GMRDC1\\Folder Redirection\\Nicholas.Shi\\Documents\\Github\\Data - Structures\\BurstTrie\\BurstTrieTest\\words.txt";
            List<string> words = File.ReadAllLines(url).ToList();
            
            BurstTrie trie = new BurstTrie();
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

        }

        [Fact]
        public void DeleteTest()
        {

        }

        [Fact]
        public void SearchTest()
        {

        }

        [Fact]
        public void GetAllTest()
        {

        }
    }
}