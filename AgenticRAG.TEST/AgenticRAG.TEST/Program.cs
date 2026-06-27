using System;
using System.Collections.Generic;

using System;

//class Program
//{
//    static void Main()
//    {
//        long before = GC.GetAllocatedBytesForCurrentThread();
//        Console.WriteLine($"Allocated before: {before:N0} bytes");

//        string text = "Hello World";

//        for (int i = 0; i < 100000; i++)
//        {
//            //var s = text.Substring(0, 5);
//            ReadOnlySpan<char> span = text.AsSpan(0, 5);
//        }

//        long after = GC.GetAllocatedBytesForCurrentThread();

//        Console.WriteLine($"Allocated: {after - before:N0} bytes");
//    }
//}

class Program
{
    static List<byte[]> references = new List<byte[]>();
    static List<Customer> customers = new List<Customer> ();

    static void Main()
    {
        Console.WriteLine("Start");

        // Step 1: Allocate many short-lived objects
        for (int i = 0; i < 10000; i++)
        {
            var data = new byte[1024]; // 1 KB object
            references.Add(data); // keep some alive
            customers.Add(new Customer { Name = $"Customer {i}" }); // keep some alive
        }

        Console.WriteLine("Allocated");

        PrintGCInfo("After allocation");

        // Step 2: Force GC
        GC.Collect(0); // Gen 0 GC
        GC.WaitForPendingFinalizers();

        PrintGCInfo("After Gen 0 GC");

        GC.Collect(); // Full GC (Gen 0 + Gen 1 + Gen 2)
        GC.WaitForPendingFinalizers();

        PrintGCInfo("After Full GC");

        Console.ReadLine();
    }

    static void PrintGCInfo(string label)
    {
        Console.WriteLine($"--- {label} ---");
        Console.WriteLine($"Gen 0: {GC.CollectionCount(0)}");
        Console.WriteLine($"Gen 1: {GC.CollectionCount(1)}");
        Console.WriteLine($"Gen 2: {GC.CollectionCount(2)}");
    }
}

public class Customer
{
    public string Name { get; set; }
}


//class Program
//{
//    static void Main()
//    {
//        Person p = new Person("John");

//        Console.WriteLine(p.Name);

//        p = null;

//        GC.Collect();

//        Console.WriteLine("End");
//    }
//}

//class Person
//{
//    public string Name { get; set; }

//    public Person(string name)
//    {
//        Name = name;
//    }
//}