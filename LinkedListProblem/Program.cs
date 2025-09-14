namespace LinkedListProblem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Node node1 = new Node(1);
            Node node2 = new Node(2);
            Node node3 = new Node(3);
            Node node4 = new Node(4);
            Node node5 = new Node(5);

            node1.Next = node2;
            node2.Next = node3;
            node3.Next = node4;
            node4.Next = node5;
            node5.Next = null;

            /*while (node1 != null)
            {
                Console.WriteLine(node1.Value);
                node1 = node1.Next;
            }*/
            Node reversed = ReverseLinkedList(node1);

            while (reversed != null)
            {
                Console.WriteLine(reversed.Value);
                reversed = reversed.Next;
            }
            //Console.WriteLine(FunnyFunction(node1));
        }
        static bool FunnyFunction(Node node)
        {
            if (node == null || node.Next == null) return false;
            Node delay = node;
            Node fast = node.Next;
            while (fast.Next != null)
            {
                if (delay != fast.Next) return true;
                fast = node.Next.Next;
                delay = node.Next;
            }


            return false;
        }
        
        static Node ReverseLinkedList(Node start)
        {
            Node first = start;
            Node temp = first.Next;
            first.Next = null;
            Node second = temp;
            
            while (temp != null)
            {
                temp = second.Next;
                second.Next = first;

                first = second;
                second = temp;
            }

            
            return first;
        }

    }
}
