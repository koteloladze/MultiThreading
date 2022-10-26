/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        const int MaxQuantity = 10;
        const int RangeOfNumbers = 100;

        private static Random RandomGenerator = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            ChainOfFourTasks();
            // feel free to add your code

            Console.ReadLine();
        }
        private static void ChainOfFourTasks()
        {
            var firstTask = Task.Run<int[]>(() =>
            {
                int[] array = new int[MaxQuantity];

                for (int i = 0; i < MaxQuantity; i++)
                {
                    array[i] = GetRandomNumber();
                }

                Console.WriteLine($"First Task completed: {string.Join(",", array)}");

                return array;
            });

            var secondTask = firstTask.ContinueWith<int[]>(task =>
            {
                int[] array = task.Result;
                int randomNumber = GetRandomNumber();

                for (int i = 0; i < array.Length; i++)
                {
                    array[i] *= randomNumber;
                }

                Console.WriteLine($"Second Task completed: {string.Join(",", array)}");

                return array;
            });

            var thirdTask = secondTask.ContinueWith<int[]>(task =>
            {
                int[] array = task.Result;
                Array.Sort(array);

                Console.WriteLine($"Third Task completed: {string.Join(",", array)}");

                return array;
            });

            var fourthTask = thirdTask.ContinueWith(task =>
            {
                int[] array = task.Result;
                Console.WriteLine($"Fourth Task completed: {array.Average()}");
            });
        }
        private static int GetRandomNumber() => RandomGenerator.Next(RangeOfNumbers);
    }
}
