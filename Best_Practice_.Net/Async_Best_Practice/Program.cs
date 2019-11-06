using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Async_Best_Practice
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var thingsToLoop = new List<int> { 1, 2, 3, 4, 5 };
            await BadLoopAsync(thingsToLoop);
            await GoodLoopAsync(thingsToLoop);

            Console.WriteLine($"OK");
            Console.ReadKey();
        }


        //Asyncronous method to be called
        public static async Task DoAsync(int item)
        {
            if(item != 3)
            {
                await Task.Delay(1000);
                Console.WriteLine($"Item: {item}");
            }
            else
            {
                await Task.Delay(5000);
                Console.WriteLine($"Item: {item}");
            }
        }

        //Looping method
        public static async Task BadLoopAsync(IEnumerable<int> thingsToLoop)
        {
            foreach (var thing in thingsToLoop)
            {
                await DoAsync(thing);
            }
        }

        public static async Task GoodLoopAsync(IEnumerable<int> thingsToLoop)
        {
            List<Task> listOfTasks = new List<Task>();

            foreach (var thing in thingsToLoop)
            {
                listOfTasks.Add(DoAsync(thing));
            }

            await Task.WhenAll(listOfTasks);
        }
    }
}
