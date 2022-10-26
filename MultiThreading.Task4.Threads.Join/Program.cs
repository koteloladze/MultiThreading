/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();
            CreateThreads(11);
            CreateThreadsWithThreadPool(11);

            // feel free to add your code

            Console.ReadLine();
        }

        private static void CreateThreads(int state, int count = 1)
        {
            Console.WriteLine("starting of method : " + Thread.CurrentThread.ManagedThreadId);
            if (count > 10)
            {
                return;
            }

            Thread thread = new Thread(() => CreateThreads(state, count + 1));
            Console.WriteLine("Before run : " + Thread.CurrentThread.ManagedThreadId);
            thread.Start();
            Console.WriteLine("After start : "+Thread.CurrentThread.ManagedThreadId);
            Interlocked.Decrement(ref state);

            Console.WriteLine($"{thread.ManagedThreadId} changed state to : {state}");

            thread.Join();
        }

        private static void CreateThreadsWithThreadPool(int state, int count = 1)
        {
            if (count > 10)
            {
                return;
            }

            ThreadPool.QueueUserWorkItem((obj) =>
            {
                SemaphoreSlim.Wait();

                Interlocked.Decrement(ref state);

                Console.WriteLine($"{count}. ThreadId: {Thread.CurrentThread.ManagedThreadId}, State: {state}");

                CreateThreadsWithThreadPool(state, count + 1);

                Thread.Sleep(2000);

                SemaphoreSlim.Release();
            });
        }
    }
}
