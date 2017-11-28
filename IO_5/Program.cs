using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IO_5
{
   class Zadanie5
    {
        List<int> tab;
        int array_lenght, sum;
        AutoResetEvent MyEvent = new AutoResetEvent(false);
        WaitHandle[] waitHandles;
        private static object tlock = new object();

        Zadanie5(int x, int y, int z)
        {
            tab = new List<int>(x);
            array_lenght = y;
            sum = 0;
            waitHandles = new WaitHandle[z];
        }
        static void Main(string[] args)
        {
            String size, threads;
            Console.WriteLine("===================================\nIle elementow ma byc w tablicy?");
            size = Console.ReadLine();
            Console.WriteLine("===================================\nIle watkow uzyc?");
            threads = Console.ReadLine();
            float z = float.Parse(size) / float.Parse(threads);
            int roundedUp = (int)Math.Ceiling(z);
            Zadanie5 z5 = new Zadanie5(Int32.Parse(size), Int32.Parse(threads), Int32.Parse(threads));
            z5.tab = MakeTable(Int32.Parse(size));
            for(int i=0; i< Int32.Parse(threads); i++)
            {
                z5.waitHandles[i] = new AutoResetEvent(false);
            };
            var watch = new Stopwatch();
            watch.Start();           
            for (int i = 0; i < Int32.Parse(threads); i++)
            {
                float start = roundedUp * i;
                float stop = 0;
                if (i == 0)
                {
                     stop = roundedUp -1;
                }
                else {  stop = (roundedUp * (i+1)) - 1; }
                ThreadPool.QueueUserWorkItem(new WaitCallback(z5.SumUnit), new object[] { start, stop ,z5.waitHandles[i] });
            }
            WaitHandle.WaitAll(z5.waitHandles);
            watch.Stop();
            Console.WriteLine("Czas: {0} ms", 1000.0 * watch.ElapsedTicks / Stopwatch.Frequency);
            Console.WriteLine("================================\nSuma calosciowa "+z5.sum);
            Console.ReadLine();

        }
        void SumUnit(Object stateInfo)
        {
            float start = (float)((object[])stateInfo)[0];
            float stop = (float)((object[])stateInfo)[1];
           // float suma = 0;
            for (int i = (int)start; i <= (int)stop; i++)
            {
                if (tab.Count > i)
                // lock (tlock)
                {
                    sum += tab[i];
                }
                else break;
            }
           // sum += (int)suma;
            Console.WriteLine(sum);
            AutoResetEvent waitHandle = (AutoResetEvent)((object[])stateInfo)[2];
            waitHandle.Set();
        }
        static List<int> MakeTable(int n)
        {
            Random random = new Random();
            List<int> tab = new List<int>();
            for (int i = 0; i < n; i++)
            {
                tab.Add(random.Next(0, 1000));
                //
                tab.Add(i);

            }
            return tab;
        }
    }
}



