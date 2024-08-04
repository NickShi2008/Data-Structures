using System.Reflection.Metadata;

namespace LinkedListStack
{
    public class Program
    {
        static void Main(string[] args)
        {
            Stack<int> stack = new Stack<int>();

            stack.TryPop(out int value);

           /* for(int i = 0; i < 10; i++)
            {
                stack.Push(i);
                Console.WriteLine(stack.Peek());
            }
            Console.WriteLine(stack.Pop());
            Console.WriteLine(stack.Peek());*/


        }
    }

    public class Stack<T>
    {
        private int count;
        public int Count
        {
            get
            {
                return count;
            }
            private set
            {
                count = value;
            }
        }

        private LinkedList<T> data;

        public Stack()
        {
            data = new LinkedList<T>();
        }

        public void Push(T value)
        {
           data.AddFirst(value);
            Count++;
        }

        public T Pop()
        {
            if (data.Count == 0)
            {
                throw new InvalidOperationException();
            }
            T value = data.First();
            data.RemoveFirst();
            Count--;
            return value;
        }

        public T Peek()
        {
            if (data.Count == 0)
            {
                throw new InvalidOperationException();   
            }
            return data.First();
        }

        public bool TryPop(out T value)
        {
            if(Count == 0)
            {
                value = default;
                return false;
            }
            value = Pop();
            return true;
        }
    }

}
