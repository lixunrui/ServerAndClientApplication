using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace ServerApplication
{
    internal class Server
    {
        static internal int sequence = 0;

        List<string> clients;

        TcpListener serverListener;
        TcpClient clientSocket;
        AutoResetEvent stopServer;

        /// <summary>
        /// 
        /// </summary>
        internal void InitServer(AutoResetEvent resetEvent)
        {
            IPAddress serverIP = IPAddress.Parse("0.0.0.0");
            serverListener = new TcpListener(serverIP, 8081); // range 0- 2^16-1 (0-65535), normally from 1025 - 65535

            serverListener.ExclusiveAddressUse = false;

            serverListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);


            clientSocket = new TcpClient();
            stopServer = resetEvent;

            // init clients
            clients = new List<string>();
        }
        /// <summary>
        /// Start the server
        /// </summary>
        internal void Start()
        {
            Thread serverThread = new Thread(() => RunServer());
            serverThread.Start();

            stopServer.WaitOne();

            serverThread.Join();
            Console.WriteLine("Server Stopped");
        }

        void RunServer()
        {
            if (serverListener!=null)
            {
                serverListener.Start();
                Console.WriteLine("Server started");
               
            while (true)
            {
                try
                {
                    if (!clientSocket.Connected)
                    {
                        Console.WriteLine("Waiting for a connection...");

                        clientSocket = serverListener.AcceptTcpClient();

                        Console.WriteLine("Connected");
                        
                        
                    }
                    NetworkStream networkStream = clientSocket.GetStream();

                    byte[] bytesFrom = new byte[10240];

                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);

                    string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);

                    dataFromClient = dataFromClient.TrimEnd('\0');

                    Console.WriteLine("Data From Client {0}:\n{1}\n", ((IPEndPoint)clientSocket.Client.RemoteEndPoint).Address.ToString(), dataFromClient);

                   // string serverResponse = "Data Received";

                    HeaderMsg header = new HeaderMsg();
                    header.messageID = sequence++;
                    header.messageFrom = "server";
                    header.messageTO = ((IPEndPoint)clientSocket.Client.RemoteEndPoint).Address.ToString();
                    header.messageSize = 0;

                    byte[] sendBytes = Utility.GetBytesFromStruct(header);

                    networkStream.Write(sendBytes, 0, sendBytes.Length);

                    networkStream.Flush();

                    Console.WriteLine("Response Sent");
                   
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("Exception: {0}", ex.Message);
                    clientSocket.Close();
                }
            }
            }
            else
            {
                Console.WriteLine("Server error");
                stopServer.Set();
                return;
            }
        }// run server

        
    }
}
