namespace ArrayQueue
{
    public class Program
    {
        static void Main(string[] args)
        {
            Queue<int> q = new Queue<int>(capacity: 1);
            q.Enqueue(1);
            q.Enqueue(2);
            q.Enqueue(3);
            q.Enqueue(4);

            q.Dequeue();
            q.Dequeue();
            q.Dequeue();

            q.Enqueue(5);
            q.Enqueue(6);
            q.Enqueue(7);
            q.Enqueue(8);


           
            
        }
    }

    public class Queue<T>
    {
        public int Count { get; private set; }
        private T[] data;
        private int head;
        private int tail;


        public Queue(int capacity = 2)
        {
            data = new T[capacity];
        }

        public void Enqueue(T value)
        {
            if(tail == data.Length)
            {
                tail %= data.Length;
            }
            if(Count == data.Length)
            {
                Resize(data.Length << 1);
                float check = data.Length;
            }
            data[tail++] = value;
            Count++;
        }

        public T Dequeue()
        {
            if(data.Length == 0)
            {
                throw new Exception();
            }

            if(tail - head <= data.Length >> 2)
            {
                Resize(data.Length >> 1);
            }
            head++;
            Count--;
            return data[head - 1];
        }

        public T Peek()
        {
            if(tail >= data.Length)
            {
                throw new Exception();
            }

            return data[head];
        }

        public void Resize(int size)
        {
            T[] resized = new T[size];
            int count = 0;
           
            for(int i = 0; i < Count; i++)
            {
                if(count >= resized.Length)
                {
                    break;
                }
                if (i + tail >= data.Length)
                {
                    resized[count] = data[i + tail - data.Length];
                }
                else
                {
                    resized[count] = data[i + tail];
                }
                count++;
            }
            tail = data.Length;
            head = 0;
            data = resized;
           
        }
    }

}
