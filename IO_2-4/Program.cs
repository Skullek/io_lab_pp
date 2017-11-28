using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IO_2_4
{
    class Program
    {
        private static object client;
        private static object tlock = new object(); // Lock na kolor
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(ThreadProcServer);
            ThreadPool.QueueUserWorkItem(ThreadProcClient);
            ThreadPool.QueueUserWorkItem(ThreadProcClient);
            ThreadPool.QueueUserWorkItem(ThreadProcClient);
            ThreadPool.QueueUserWorkItem(ThreadProcClient);
            ThreadPool.QueueUserWorkItem(ThreadProcClient);
            ThreadPool.QueueUserWorkItem(ThreadProcClient);
            Console.ReadLine();
            Console.WriteLine("watek glowny");
        }
        static void ThreadProcClient(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            NetworkStream stream = client.GetStream();
            byte[] message = new ASCIIEncoding().GetBytes("wiadomosc");
            client.GetStream().Write(message, 0, message.Length);
            while (true)
            {
                byte[] buffer = new byte[1024];
                int len = client.GetStream().Read(buffer, 0, buffer.Length);
                string Wiad = new ASCIIEncoding().GetString(buffer, 0, len);
                writeConsoleMessage("Klient:" + Wiad, ConsoleColor.Green);
            }
        }
        static void ThreadProcServer(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                if (client != null)
                {
                    ThreadPool.QueueUserWorkItem(ThreadProcServerAdd, new object[] { client });
                }
            }
        }
        static void ThreadProcServerAdd(Object stateInfo)
        {
            while (true)
            {
                TcpClient client = (TcpClient)((object[])stateInfo)[0];
                byte[] buffer = new byte[1024];
                int len = client.GetStream().Read(buffer, 0, 1024);
                client.GetStream().Write(buffer, 0, len);
                string Wiad = new ASCIIEncoding().GetString(buffer, 0, len);
                writeConsoleMessage("Serwer:" + Wiad, ConsoleColor.Red);
            }
        }
        static void writeConsoleMessage(string message, ConsoleColor color)
        {
            lock (tlock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}

