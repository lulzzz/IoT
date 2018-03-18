using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IoT.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace IoT.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            int nrOfClients = 10;
            int waitTime = 20;
            long nrOfMessagesPerClient = 10000;

            var clients = new List<Client>();
            var topics = new[] { "topic/a", "topic/b" };
            for (int i = 0; i < nrOfClients; i++)
            {
                var client = new Client("client_"+i, waitTime, nrOfMessagesPerClient, topics[i % 2], topics[(i+1) % 2]);
                clients.Add(client);
            }

            var tasks = new List<Task>();
            foreach(var client in clients)
            {
                tasks.Add(Task.Run(() => client.Start()));
            }
            Task.WaitAll(tasks.ToArray());

            Console.WriteLine("all run, press enter to disconnect");
            Console.ReadLine();

            foreach (var client in clients)
            {
                client.Dispose();
            }
                
            Console.WriteLine("press enter to exit");
            Console.ReadLine();
        }
    }
}
