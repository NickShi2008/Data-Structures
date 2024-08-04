namespace LinkedListQueue
{
    public class Program
    {
        static void Main(string[] args)
        {
            Queue<int> q = new Queue<int>();
            q.Enqueue(1);
            q.Enqueue(2);
            q.Enqueue(3);
                q.Enqueue(4);
            q.Enqueue(5);
            q.Enqueue(6);
            Console.WriteLine(q.Dequeue());
            Console.WriteLine(q.Peek());

        }
    }

    public class Queue<T>
    {
        public int Count { get; private set; }
        private LinkedList<T> data;
        public Queue()
        {
            data = new LinkedList<T>();
        }

        public void Enqueue(T value)
        {
            data.AddLast(value);
            Count++;
        }

        public T Dequeue()
        {
            if(data.Count == 0)
            {
                throw new Exception();
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
                throw new Exception();
            }
            return data.First();
        }

    }

}
