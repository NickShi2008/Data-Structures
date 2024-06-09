namespace DoublyLinkedList
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
    
        }
    }

    public class DoublyLinkedList<T>
    {

        public class Node
        {

            public T Value { get; set; }

            public Node Next { get; set; }
            public Node Previous { get; set; }

            public Node(T value, Node next, Node previous)
            {
                Value = value;
                Next = next;
                Previous = previous;
            }

            public Node(T Value)
            {
                this.Value = Value;
                Next = null;
                Previous = null;
            }

        }


        public Node Head { get; set; }
        public Node Tail { get; set; }
        public int Count { get; set; }

        

        public DoublyLinkedList()
        {
            Head = null;
            Tail = null;
            Count = 0;
        }

        public void AddFirst(T value)
        {
            Node temp = new Node(value);
            if(Head == null)
            {
                Head = temp;
                Tail = Head;
            }
            else
            {
                Head.Previous = temp;
                temp.Next = Head;
                Head = temp;
            }
            
        }
    }
}
