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

        static void client_ServerEvent(object sender, string response)
        {
            Console.WriteLine("Message From Server:\n{0}\n\n", response);
        }
    }
}
