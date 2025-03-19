using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurstTries
{
    public class BurstTrie
    {
        private BurstNode Root { get; set; }

        public BurstTrie(char min, char max)
        {

        }

        public BurstNode Insert(string value, int index)
        {
            return Root.Insert(value, index);
        }
        // Abstract recursive deletion function, returns replacement value for back-propagation    
        public BurstNode? Remove(string value, int index, out bool success)
        {
            return Root.Remove(value, index, out success);
        }
        // Get a Node containing a defined prefix
        public BurstNode? Search(string prefix, int index)
        {
            return Root.Search(prefix, index);
        }
        // Gets all items in order recursively
        internal void GetAll(List<string> output)
        {
            Root.GetAll(output);
        }
    }
}
