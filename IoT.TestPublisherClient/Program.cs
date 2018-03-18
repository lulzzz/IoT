using System;
using System.Threading;
using IoT.Messages;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace IoT.TestPublisherClient
{
    class Program
    {
        const string topic = "my/topic";
        const long nrOfMessages = 1000000;
        const int waitTime = 100;

        static long sendCount = 0;
        static string clientId = Guid.NewGuid().ToString();

        static void Main(string[] args)
        {

            var client = new MqttClient("127.0.0.1");

            client.Connect(clientId);

            for (int i = 0; i < nrOfMessages; i++)
            {
                string msg = clientId + " : " + i;
                client.Publish(topic, MessageConverter.Serialize(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                sendCount++;
                if (sendCount % 1000 == 0)
                {
                    Console.WriteLine("Sent: " + clientId + ": " + DateTime.Now.ToLongTimeString() + " : " + sendCount);
                    Thread.Sleep(waitTime);
                }
            }

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
