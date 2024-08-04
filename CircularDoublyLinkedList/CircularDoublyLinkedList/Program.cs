
namespace CircularDoublyLinkedList
{
    public class Program
    {
        static void Main(string[] args)
        {

            CircularDoublyLinkedList<int> test = new CircularDoublyLinkedList<int>();
             test.AddFirst(1);
             test.AddLast(9);
             test.AddBefore(test.Head, 3);
             test.AddAfter(test.Tail, 5);
             test.RemoveFirst();
             test.RemoveLast();
             test.Remove(test.Head.Value);

            CircularDoublyLinkedList<int>.Node current = test.Head;
            if (current != null)
            {
                do
                {
                    Console.WriteLine(current.Value);
                    current = current.Next;
                }
                while (current != test.Head);
            }
            Console.WriteLine();

            //     test.Remove(test.Tail.Value);
             Console.WriteLine(test.IsEmpty());

        }
    }
}
