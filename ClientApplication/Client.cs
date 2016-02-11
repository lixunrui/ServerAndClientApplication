using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace ClientApplication
{
    
    internal class Client
    {
        TcpClient clientSocket;
        NetworkStream networkStream;

        AutoResetEvent serverEvent = new AutoResetEvent(false);

        internal delegate void ResponseFromServerEventHandler(object sender, string response);

        internal event ResponseFromServerEventHandler ServerEvent;

        internal Client()
        {
            clientSocket = new TcpClient();
            Thread connection = new Thread(() => StartConenction());
            connection.Start();
        }

    

        internal void Send(string msg)
        {
            byte[] outStream = Encoding.ASCII.GetBytes(msg);
            networkStream.Write(outStream, 0, outStream.Length);
            networkStream.Flush();
        }

        void ListenResponse()
        {
            Console.WriteLine("Start Client Listening");
            try
            {
                while (true)
                {
                    byte[] responseByte = new byte[10240];

                    networkStream.Read(responseByte, 0, (int)clientSocket.ReceiveBufferSize);

                    string responseData = Encoding.ASCII.GetString(responseByte);

                    responseData = responseData.TrimEnd('\0');

                    if (ServerEvent != null)
                    {
                        ServerEvent(this, responseData);
                    }

                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Server error:{0}", ex.Message);
                serverEvent.Set();
            }
            
        }

        void StartConenction()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        if (clientSocket == null)
                        {
                            clientSocket = new TcpClient();
                        }
                        Console.WriteLine("Connecting to server...");
                        clientSocket.Connect("127.0.0.1", 8081);
                    }
                    catch (System.Exception ex)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("Retry");
                        continue;
                    }
                    
                    networkStream = clientSocket.GetStream();

                    Console.WriteLine("Connected to server");

                    Thread listener = new Thread(() => ListenResponse());

                    listener.Start();

                    serverEvent.WaitOne();

                    listener.Join();

                    CleanUp();
                }
                
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error:{0}", ex.Message);
            }

        }

        void CleanUp()
        {
            clientSocket.Close();
            clientSocket = null;
        }
    }
}
