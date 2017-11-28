using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
class Zad1
{
    static void Main(string[] args)
    {
        ThreadPool.QueueUserWorkItem(Thread1, new object[] { 200 });
        ThreadPool.QueueUserWorkItem(Thread2, new object[] { 100 });
        Thread.Sleep(1000);
    }
    static void Thread1(Object stateInfo)
    {
        var Integer = ((object[])stateInfo)[0];
        Thread.Sleep((int)Integer);
        Console.WriteLine(Integer);
    }

    static void Thread2(Object stateInfo)
    {
        var Integer = ((object[])stateInfo)[0];
        Thread.Sleep((int)Integer);
        Console.WriteLine(Integer);
    }
}
