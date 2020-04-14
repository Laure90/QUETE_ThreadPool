using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QUETE_ThreadPool
{
    class Program
    {
        public ConcurrentQueue<int> Scores = new ConcurrentQueue<int>();
        private CountdownEvent _countdown;
        private Int32 _threadsCount;
        static void Main(string[] args)
        {
            Program p = new Program(1);
            p.Run();
        }
        public Program(Int32 threadsCount)
        {
            _threadsCount = threadsCount;
            _countdown = new CountdownEvent(threadsCount);
        }
        public void Run()
        {
            Random random = new Random();
            for (int i = 0; i < _threadsCount; i++)
            {
                ThreadPool.QueueUserWorkItem(x =>
                {
                    
                    Console.WriteLine("[INFO]: Waiting for results ...", Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(1000);
                    for (int i = 0; i < 1000000; i++)
                    {
                        Scores.Enqueue(random.Next(20, 60)); 
                    }
                    Thread.Sleep(1000);

                    Console.WriteLine("[INFO]: Download...", Thread.CurrentThread.ManagedThreadId);
                    _countdown.Signal();                    
                });                
            }

            while (_countdown.CurrentCount > 0)
            {
                Console.WriteLine("[INFO]: Start of the process");
                Thread.Sleep(3000);
            }

            _countdown.Wait(); 

            Console.WriteLine("[SUCESS]: Download of all race results complete");
            Thread.Sleep(3000);
            Console.WriteLine("There are " + Scores.Count + " scores that have been downloaded. \nPress [ENTER]");
            Console.ReadKey();
        }

    }
}
