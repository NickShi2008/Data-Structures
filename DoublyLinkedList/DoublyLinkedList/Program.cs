using System.Reflection.Metadata.Ecma335;

namespace DoublyLinkedList
{
    public class Program
    {
        static void Main(string[] args)
        {

            DoublyLinkedList<int> test = new DoublyLinkedList<int>();
            test.AddFirst(1);
            test.AddLast(9);
            test.AddBefore(test.Head, 3);
            test.AddAfter(test.Tail, 5);
            test.RemoveFirst();
            test.RemoveLast();
            test.Remove(test.Tail.Value);

         


            DoublyLinkedList<int>.Node current = test.Head;
            while (current != null)
            {
                Console.WriteLine(current.Value);
                current = current.Next;
            }

            test.Remove(test.Tail.Value);
            Console.WriteLine(test.IsEmpty());
    
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

        public void AddLast(T value)
        {
            Node temp = new Node(value);
            if (Head == null)
            {
                Head = temp;
                Tail = Head;
                return;
                
            }
            Node last = Tail;
            last.Next = temp;
            temp.Previous = last;
            Tail = temp;
            
        }

        public void AddBefore(Node node, T value)
        {
            Node temp = new Node(value);
            Node current = Head;
            if (node == Head)
            {
                temp.Next = Head;
                Head.Previous = temp;
                Head = temp;
                return;
            }

            while (current != null && !current.Value.Equals(node.Value))
            {
                current = current.Next;
            }

            

            if (current != null && current == node)
            {
                temp.Next = current;
                temp.Previous = current.Previous;

                if (current.Previous != null)
                {
                    current.Previous.Next = temp;
                }
                current.Previous = temp;
            }
        }

        public void AddAfter(Node node, T value)
        {
            Node temp = new Node(value);
            Node current = Head;
            while (current != null && !current.Value.Equals(node.Value))
            {
                current = current.Next;
            }

            if (node == Tail)
            {
                temp.Previous = Tail;
                Tail.Next = temp;
                Tail = temp;
                return;
            }
            if (current != null && current.Value.Equals(node.Value))
            {
                temp.Previous = current;
                temp.Next = current.Next;

                if (current.Next != null)
                { 
                    current.Next.Previous = temp;
                }
                current.Next = temp;

            }
        }

        public bool RemoveFirst()
        {
            if (Head == null) return false;
            else if(Head.Next == null)
            {
                Head = null;
                return true;
            }
            Head = Head.Next;
            Head.Previous = null;
            return true;

        }

        public bool RemoveLast()
        {
            if (Tail == null) return false;
            else if(Tail.Previous == null)
            {
                Tail = null;
                return true;
            }
            Tail = Tail.Previous;
            Tail.Next = null;
            return true;
        }

        public bool Remove(T value)
        {
            if (value == null) return false;
            Node current = Head;
            if(value.Equals( Head.Value))
            {
                Head = Head.Next;
                return true;
            }
            while(current != null && !current.Value.Equals(value))
            {
                current = current.Next;
            }

            if (value.Equals(Tail.Value))
            {
                Tail = Tail.Previous;
                Tail.Next = null;
                return true;
            }

            if (current != null && current.Value.Equals(value))
            {
                current.Previous = current.Next;
                current.Next = current.Previous;
                current = null;
                return true;
            }
            return false;
        }

        public bool IsEmpty()
        {
            if(Head == null)
            {
                return true;
            }
            return false;
        }



    }

}
