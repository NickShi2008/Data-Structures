namespace SkipList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SkipList<int> skip = new SkipList<int>();

            skip.Insert(5);
            skip.Insert(3);
            skip.Insert(7);
            skip.Insert(1);
            skip.Insert(2);
            skip.Insert(4);

            ;
        }
    }
}
