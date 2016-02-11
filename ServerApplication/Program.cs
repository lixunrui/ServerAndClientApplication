using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServerApplication
{
    class Program
    {
        static AutoResetEvent _serverEvent = new AutoResetEvent(false);
        

        static void Main(string[] args)
        {
            Run();
            OtherTask();
        }

        static void Run()
        {
            Server _localServer = new Server();
            _localServer.InitServer(_serverEvent);
            Thread serverRun = new Thread(()=> _localServer.Start());
            serverRun.Start();
        }

        static void OtherTask()
        {
            while (true)
            {
                Console.WriteLine("Other task running...");
                Thread.Sleep(10000);
            }
        }
    }
}
