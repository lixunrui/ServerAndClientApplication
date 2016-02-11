using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ClientApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Client client = new Client();

                client.ServerEvent += client_ServerEvent;

                while (true)
                {
                    string message = Console.ReadLine();

                    client.Send(message);
                }
            }
            catch (ClientException ex)
            {
                Console.WriteLine(ex.Error);
            }

            Console.ReadKey();
        }

        static void client_ServerEvent(object sender, object response)
        {
            HeaderMsg header = (HeaderMsg)response;
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("ID:{0}\n", header.messageID));
            builder.Append(String.Format("From:{0}\n", header.messageFrom));
            builder.Append(String.Format("To:{0}\n", header.messageTO));
            builder.Append(String.Format("Rest Size:{0}", header.messageSize));

            Console.WriteLine("Message From Server:\n{0}\n", builder.ToString());
        }
    }
}
