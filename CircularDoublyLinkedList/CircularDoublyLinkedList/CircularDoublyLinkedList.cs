
namespace CircularDoublyLinkedList
{
    public class CircularDoublyLinkedList<T>
    {

        public class Node
        {

            public T Value { get; set; }

            public Node Next { get; set; }
            public Node Previous { get; set; }

            public Node(T value, Node next, Node previous)
            {
                Value = value;
                Next = null;
                Previous = null;
            }

            public Node(T Value)
            {
                this.Value = Value;
                Next = this;
                Previous = this;
            }

        }


        public Node Head { get; set; }
        public Node Tail { get; set; }
        public int Count { get; set; }



        public CircularDoublyLinkedList()
        {
            Head = Tail;
            Tail = Head;
           
            Count = 0;
        }

        public void AddFirst(T value)
        {
            Node temp = new Node(value);
            if (Head == null)
            {
                Head = temp;
                Tail = Head;
            }
            else
            {
                Head.Previous = temp;
                temp.Next = Head;
                Head = temp;
                Head.Previous = Tail;
                Tail.Next = Head;
                
            }
            Count++;
        }

        public void AddLast(T value)
        {
            Node temp = new Node(value);
            if (Head == null)
            {
                Head = temp;
                Tail = Head;
                Count++;
                return;

            }
            Node last = Tail;
            last.Next = temp;
            temp.Previous = last;
            Tail = temp;
            Tail.Next = Head;
            Head.Previous = Tail;
            Count++;
        }

        public void AddBefore(Node node, T value)
        {
            Node temp = new Node(value);
            Node current = Head;
            if(node == null)
            {
                Count++;
                return;
            }
            if (node == Head)
            {
                temp.Next = Head;
                Head.Previous = temp;
                Head = temp;
                Head.Previous = Tail;
                Tail.Next = Head;
                Count++;
                return;
            }
            
            do
            {
                current = current.Next;
            }
            while (!current.Value.Equals(node.Value) && current!= Head);




            if (current == node)
            {
                temp.Next = current;
                temp.Previous = current.Previous;

                current.Previous.Next = temp;
                current.Previous = temp;
            }
            Count++;
        }

        public void AddAfter(Node node, T value)
        {
            Node temp = new Node(value);
            Node current = Head;
            do
            {
                current = current.Next;
            }
            while (!current.Value.Equals(node.Value) && current != Head);

            if (current.Value.Equals(node.Value))
            {
                temp.Previous = current;
                temp.Next = current.Next;

                current.Next.Previous = temp;
                current.Next = temp;

            }

            if(node == Tail)
            {
                Tail = temp;
            }
            Count++;
        }

        public bool RemoveFirst()
        {
            if (Head == null) return false;
            else if (Head.Value.Equals(Tail.Value))
            {
                Head = null;
                Tail = Head;
                Count--;
                return true;
            }
            Head = Head.Next;
            Tail.Next = Head;
            Count--;
            return true;

        }

        public bool RemoveLast()
        {
            if (Tail == null) return false;
            else if (Tail.Previous == Head)
            {
                Tail = null;
                Head = Tail;
                Count--;
                return true;
            }
            Tail = Tail.Previous;
            Tail.Next = Head;
            Count--;
            return true;
        }

        public bool Remove(T value)
        {
            if (value == null) return false;
            Node current = Head;
            if (value.Equals(Head.Value)) return RemoveFirst();
            do
            {
                current = current.Next;
            }
            while (!current.Value.Equals(value) && current != Head);

            if (value.Equals(Tail.Value))
            {
                Tail = Tail.Previous;
                Tail.Next = Head;
                Count--;
                return true;
            }

            if (current.Value.Equals(value))
            {
                current.Previous = current.Next;
                current.Next = current.Previous;
                Count--;
                return true;
            }
            Count--;
            return false;
        }

        public bool IsEmpty()
        {
            if (Head == null)
            {
                return true;
            }
            return false;
        }



    }
}
