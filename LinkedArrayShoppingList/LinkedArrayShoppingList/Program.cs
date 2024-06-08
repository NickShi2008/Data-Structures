namespace LinkedArrayShoppingList
{
    public class Program
    {


        static void Main(string[] args)
        {
            LinkedList<Item> shoppingList = new LinkedList<Item>();
            String userInput = "no";


        }

        class LinkedList<T>
        {
            public Node<T> head;
            public Node<T> Head
            {
                get
                {
                    return head;
                }

                set
                {
                    head = value;
                }
            }
            public Node<T> tail;
            public Node<T> Tail
            {
                get
                {
                    return tail;
                }

                set
                {
                    tail = value;
                }
            }

            public int count;
            public int Count
            {
                get
                {
                    return count;
                }
                set
                {
                    count = value;
                }
            }

            public void AddFirst(T value)
            {
                Node<T> temp = new Node<T>(value);
                if (head != null)
                {
                    temp.Next = head;
                }
                head = temp;
                count++;

            }

            public void AddLast(T value)
            {
                Node<T> temp = new Node<T>(value);
                if (head == null)
                {
                    head = temp;
                    tail = head;
                }
                else
                {
                    tail.Next = temp;
                    tail = tail.Next;
                }
                count++;
            }


            //Ask about the github
            public void AddBefore(Node<T> node, T value)
            {
                Node<T> current = head;
                while (current != node)
                {
                    current = current.Next;
                }

                Node<T> temp = new Node<T>(value, node);
                current.Next = temp;
            }

            public void AddAfter(Node<T> node, T value)
            {
                Node<T> current = head;
                while (current != node)
                {
                    current = current.Next;
                }
                Node<T> temp = new Node<T>(value, current.Next);
                current.Next = temp;
                count++;
            }

            public bool RemoveFirst()
            {
                if (count == 0)
                {
                    return false;
                }
                head = head.Next;
                count--;
                return true;
            }

            public bool RemoveLast()
            {
                if (count == 0)
                {
                    return false;
                }
                Node<T> current = head;
                while (current != tail)
                {
                    current = current.Next;
                }
                current.Next = null;
                tail = current;
                count--;
                return true;
            }

            public bool Remove(T value)
            {
                Node<T> current = head;
                while (current != null)
                {
                    if (current.Value.Equals(value))
                    {
                        Node<T> temp = head;
                        while (temp != current)
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
                head = null;
                tail = null;
                count = 0;
            }

            public bool Contains(T value)
            {
                Node<T> current = head;
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
                Node<T> current = head;
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


        class Node<T>
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