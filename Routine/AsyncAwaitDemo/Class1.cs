using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitDemo
{
    public class Class1
    {
        public static async Task<bool> GetFlag()
        {
            var result = await Task.Run<bool>(() =>
            {
                Thread.Sleep(2000);
                Console.WriteLine($"taskRun threadId is {Thread.CurrentThread.ManagedThreadId}");
                return false;
            });
            Console.WriteLine($"after taskRun threadId is {Thread.CurrentThread.ManagedThreadId}");
            return result;
        }
    }
}
