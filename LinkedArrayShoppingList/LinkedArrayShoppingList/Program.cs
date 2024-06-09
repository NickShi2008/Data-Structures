using System.Text.Encodings.Web;

namespace LinkedArrayShoppingList
{
    public class Program
    {


        static void Main(string[] args)
        {

            LinkedList<int> array = new LinkedList<int>();
            array.AddFirst(9);
            array.AddLast(1);
            array.AddBefore(array.Tail, 2);
            array.AddBefore(array.Tail, 3);
            array.AddAfter(array.Head, 8);
            array.AddAfter(array.Head, 7);
            array.AddAfter(array.Head, 6);
            array.AddBefore(array.Tail, 5);
            array.AddBefore(array.Tail, 4);

            LinkedList<int> sortedArray = new LinkedList<int>();
            sortedArray.AddFirst(9);
            sortedArray.AddFirst(8);
            sortedArray.AddFirst(7);
            sortedArray.AddFirst(6);

           BubbleSort(array);
           // BubbleSort(sortedArray);


                    Node<int> another = array.Head;
                    while (another != null)
                    {
                        Console.WriteLine(another.Value);
                             another = another.Next;
                    }
              

        }

        public static void BubbleSort<T>(LinkedList<T> array) 
            where T : IComparable
        {
            bool isSorted = false;
            int counter = 0;

            while(!isSorted)
            { 
                isSorted = true;
                for (Node<T> current = array.Head;  current.Next != null; current = current.Next)
                {
                    if (current.Value.CompareTo(current.Next.Value) > 0)
                    {
                        isSorted = false;
                        Node<T> temp = new Node<T>(current.Value);
                        current.Value = current.Next.Value;
                        current.Next.Value = temp.Value;

                    }
                    

                }
                counter++;
            }
            Console.WriteLine(counter);

        }

        public class LinkedList<T>
        {
            public Node<T> Head { get; private set; }

            public Node<T> Tail { get; private set; }

            public int Count { get; private set; }

            public LinkedList()
            {
                Head = null;
                Tail = null;
                Count = 0;
            }

            public void AddFirst(T value)
            {
                if (Head == null)
                {
                    Head = new Node<T>(value);
                    Tail = Head;
                }
                else
                {
                    Node<T> temp = new Node<T>(value,Head);
                    Head = temp;
                }
                Count++;

            }

            public void AddLast(T value)
            {
                Node<T> temp = new Node<T>(value);
                if (Head == null)
                {
                    Head = temp;
                    Tail = Head;
                }
                else
                {
                    Tail.Next = temp;
                    Tail = Tail.Next;
                }
                Count++;
            }
           
            public void AddBefore(Node<T> node, T value)
            {
                Node<T> current = Head;
                while (current.Next != node)
                {
                    current = current.Next;
                }

                Node<T> temp = new Node<T>(value);
                temp.Next = node;
                current.Next = temp;
                Count++;
            }

            public void AddAfter(Node<T> node, T value)
            {
                Node<T> current = Head;
                while (current != node)
                {
                    current = current.Next;
                }
                Node<T> temp = new Node<T>(value, current.Next);
                current.Next = temp;
                Count++;
            }

            public bool RemoveFirst()
            {
                if (Count == 0)
                {
                    return false;
                }
                Head = Head.Next;
                Count--;
                return true;
            }

            public bool RemoveLast()
            {
                if (Count == 0)
                {
                    return false;
                }
                Node<T> current = Head;
                while (current.Next != Tail)
                {
                    current = current.Next;
                }
                current.Next = null;
                Tail = current;
                Count--;
                return true;
            }

            public bool Remove(T value)
            {
                Node<T> current = Head;
                while (current != null)
                {
                    if (current.Value.Equals(value))
                    {
                        Node<T> temp = Head;
                        while (temp.Next != current)
                        {
                            temp = temp.Next;
                        }
                        temp.Next = current.Next;
                        current = null;
                        return true;
                    }
                    current = current.Next;
                }
                return false;
            }

            public void Clear()
            {
                Head = null;
                Tail = null;
                Count = 0;
            }

            public bool Contains(T value)
            {
                Node<T> current = Head;
                while (current != null)
                {
                    if (current.Value.Equals(value))
                    {
                        return true;
                    }
                }
                return false;
            }

            public Node<T> Search(T value)
            {
                Node<T> current = Head;
                while (current != null)
                {
                    if (current.Value.Equals(value))
                    {
                        return current;
                    }
                    current = current.Next;
                }
                return null;
            }

        }


        public class Node<T>
        {
            public T value;
            public T Value
            {
                get
                {
                    return value;
                }
                set
                {
                    this.value = value;
                }
            }
            public Node<T> next;
            public Node<T> Next
            {
                get
                {
                    return next;
                }
                set
                {
                    next = value;
                }
            }

            public Node(T value)
            {
                this.value = value;
                next = null;
            }

            public Node(T value, Node<T> next)
            {
                this.value = value;
                this.next = next;
            }
        }
    }

}