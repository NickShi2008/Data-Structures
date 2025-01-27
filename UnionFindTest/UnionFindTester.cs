using System.Text.Json;
using UnionFind;

namespace UnionFindTest
{
    public class UnionFindTester
    {
        struct Link
        {
            public string FriendA { get; set; }
            public string FriendB { get; set; }
        }
        //Gmr
        // string edges = File.ReadAllText("C:\\Users\\Nicholas.Shi\\Downloads\\FriendsProblemEdges.txt");
        // string vertices = File.ReadAllText("C:\\Users\\Nicholas.Shi\\Downloads\\FriendsProblemVertices.txt");
        string edges = File.ReadAllText("C:\\Users\\nickj\\Downloads\\FriendsProblemEdges.txt");
        string vertices = File.ReadAllText("C:\\Users\\nickj\\Downloads\\FriendsProblemVertices.txt");

        [Fact]
        public void QuickUnionUnion()
        {
            string[] people = JsonSerializer.Deserialize<string[]>(vertices);
            Link[] links = JsonSerializer.Deserialize<Link[]>(edges);
            QuickUnion<string> union = new QuickUnion<string>(people);

            foreach(var link in links)
            {
                Assert.True(union.Union(link.FriendA, link.FriendB));
                Assert.True(union.AreConnected(link.FriendA, link.FriendB));
            }

           
        }

        [Fact]
        public void QuickUnionFind()
        {
            string[] people = JsonSerializer.Deserialize<string[]>(vertices);
            Link[] links = JsonSerializer.Deserialize<Link[]>(edges);
            QuickUnion<string> union = new QuickUnion<string>(people);

            foreach (var link in links)
            {
                union.Union(link.FriendA, link.FriendB);
            }

            
            Assert.True(union.Find(people[0]) == 0);
        }

        [Fact]
        public void QuickFindFind()
        {
            string[] people = JsonSerializer.Deserialize<string[]>(vertices);
            Link[] links = JsonSerializer.Deserialize<Link[]>(edges);
            QuickUnion<string> union = new QuickUnion<string>(people);

            foreach (var link in links)
            {
                Assert.True(union.Union(link.FriendA, link.FriendB));
            }

            Assert.True(union.Find(people[0]) == 0);
        }

        [Fact]
        public void QuickFindUnion()
        {
            string[] people = JsonSerializer.Deserialize<string[]>(vertices);
            Link[] links = JsonSerializer.Deserialize<Link[]>(edges);
            QuickUnion<string> union = new QuickUnion<string>(people);

            foreach (var link in links)
            {
                union.Union(link.FriendA, link.FriendB);
                Assert.True(union.AreConnected(link.FriendA, link.FriendB));
            }
            
        }
    }
}