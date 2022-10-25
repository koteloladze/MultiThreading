/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        static CancellationToken CancellationToken = CancellationTokenSource.Token;

        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            string[] options = new string[] { "0", "1", "2" };
            string chosenOption = string.Empty;

            while (string.IsNullOrEmpty(chosenOption))
            {
                Console.WriteLine("Choose the flow of main task : 0 - normal, 1 - throw exception, 2 cancel main task");
                string input = Console.ReadLine();

                if (options.Contains(input))
                {
                    chosenOption = input;
                }
            }

            bool throwException = chosenOption.Equals("1");
            bool cancelThread = chosenOption.Equals("2");

            Task mainTask = Task.Run(() =>
            {
                Console.WriteLine($"Main task run, ThreadId : {Thread.CurrentThread.ManagedThreadId}");

                if (throwException)
                {
                    throw new Exception();
                }

                if (cancelThread)
                {
                    CancellationTokenSource.Cancel();
                    CancellationToken.ThrowIfCancellationRequested();
                }
            }, CancellationToken);


            Task firstContinuation = mainTask.ContinueWith(antecedant =>
            {
                Console.WriteLine("Firs task run regardless of main task result");
            }, TaskContinuationOptions.None);

            Task secondContinuation = mainTask.ContinueWith(antecedant =>
            {
                Console.WriteLine("Second task run when main task failed");
            }, TaskContinuationOptions.OnlyOnFaulted);

            Task thirdContinuation = mainTask.ContinueWith(antecedant =>
            {
                Console.WriteLine("Third task run when main task failed and continued same using same thread");
            }, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnFaulted);

            Task fourthContinuation = mainTask.ContinueWith(antecedant =>
            {
                Console.WriteLine("Third task run outside thread pool when main task was cancelled");
            }, TaskContinuationOptions.LongRunning | TaskContinuationOptions.OnlyOnCanceled);

            Console.ReadLine();
        }
    }
}
