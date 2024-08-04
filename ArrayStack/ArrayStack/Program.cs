namespace ArrayStack
{
    public class Program
    {
        static void Main(string[] args)
        {
            Stack<int> stack = new Stack<int>(2);
            stack.Push(1);
            stack.Push(2);
            Console.WriteLine(stack.Count);
            stack.Push(3);
            stack.Push(4);
            stack.Push(3);
            stack.Push(4);
            stack.Push(3);
            stack.Push(4);
            Console.WriteLine(stack.Count);
        }
    }

    public class Stack<T>
    {
        public int Count => currentIndex;
        private T[] data;
        private int currentIndex = 0;

        public Stack(int capacity)
        {
            data = new T[capacity];
        }

        public void Push(T value)
        {
            if(currentIndex >= data.Length)
            {
                Resize(2 * currentIndex);
            }
            data[currentIndex++] = value;
            
        }

        public T Pop()
        {
            if(currentIndex == 0)
            {
                throw new InvalidOperationException();
            }
            currentIndex--;
            return data[currentIndex];
        }

        public T Peek()
        {
            if (currentIndex == 0)
            {
                throw new InvalidOperationException();
            }
            return data[currentIndex];
        }

        public void Resize(int size)
        {
            T[] resized = new T[size];
            data.CopyTo(resized, 0);
            data = resized;
        }

    }
}
