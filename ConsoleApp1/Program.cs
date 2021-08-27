using System;
using System.IO;

namespace TestTask
{
    class Program
    {
        static void Main()
        {
            ListNode node1 = new ListNode();
            ListNode node2 = new ListNode();
            ListNode node3 = new ListNode();
            ListNode node4 = new ListNode();
            ListNode node5 = new ListNode();

            node1.Next = node2;
            node1.Previous = null;
            node1.Data = "text_node1";
            node1.Random = node5;

            node2.Next = node3;
            node2.Previous = node1;
            node2.Data = "node2";
            node2.Random = node4;

            node3.Next = node4;
            node3.Previous = node2;
            node3.Data = "node3_data";

            node4.Next = node5;
            node4.Previous = node3;
            node4.Data = "node4";
            node4.Random = node1;

            node5.Next = null;
            node5.Previous = node4;
            node5.Data = "node5_info";

            var listrandom = new ListRandom
            {
                Head = node1,
                Tail = node5,
                Count = 5
            };

            File.Delete("test.txt");
            using Stream streamW = File.OpenWrite("test.txt");
            listrandom.Serialize(streamW);

            ListRandom listDeserialized = new ListRandom();
            using Stream streamR = File.OpenRead("test.txt");
            listDeserialized.Deserialize(streamR);
            
            if(!listrandom.Equals(listDeserialized))
            {
                Console.WriteLine("List not equals");
            }
        }
    }
}

