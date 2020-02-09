using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitDemo
{
    class Program
    {
        /// <summary>
        /// 最终后台还是两个线程在执行了
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            Console.WriteLine($"before threadId {Thread.CurrentThread.ManagedThreadId}");
            await m1();
            for (var i = 0; i < 5; i++)
            {
                Thread.Sleep(700);
                Console.WriteLine($"main threadId is {Thread.CurrentThread.ManagedThreadId}");
            }

            
            Console.ReadKey();
        }

        static async Task m1()
        {
            if (!await Class1.GetFlag())
            {
                Console.WriteLine($"m1 threadId x:{Thread.CurrentThread.ManagedThreadId}");
            }
            Console.WriteLine($"m1 threadId :{Thread.CurrentThread.ManagedThreadId}");
        }

        static async void m2()
        {
            Class1.GetFlag();
            Console.WriteLine($"m2");
        }
    }
}
