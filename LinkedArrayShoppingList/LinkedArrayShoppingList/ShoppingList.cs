using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedArrayShoppingList
{
    class ShoppingList
    {
        private LinkedList<Item> shoppingList;
        private int count = 0;

        public void printList()
        {
          /*  if (shoppingList == null)
            {
                Console.WriteLine("Currently Empty");
            }
            Node<Item> current = shoppingList.Head;
            while (current != null)
            {
                Console.Write(current.ToString());
                if (current.Next != null)
                {
                    Console.Write(", ");
                }
                current = current.Next;
            }*/
        }

        public LinkedList<Item> List()
        {
           return shoppingList;
        }

    }
}
