/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static List<int> sharedCollection = new List<int>();
        static SemaphoreSlim writeSemaphore = new SemaphoreSlim(1,1);
        static SemaphoreSlim readSemaphore = new SemaphoreSlim(1, 1);
        const int numberCount = 11;

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            ExecuteTask();

            Console.ReadLine();
        }

        static void ExecuteTask()
        {
            readSemaphore.Wait();

            var writingTask = Task.Run(() =>
            {
                for (int i = 0; i < numberCount; i++)
                {
                    writeSemaphore.Wait();

                    sharedCollection.Add(i);

                    readSemaphore.Release();
                }
            });

            var readingTask = Task.Run(() =>
            {
                for (int i = 0; i < numberCount; i++)
                {
                    readSemaphore.Wait();

                    Console.WriteLine(string.Join(", ", sharedCollection));

                    writeSemaphore.Release();
                }
            });

            Task.WaitAll(writingTask, readingTask);
        }
    }
}
