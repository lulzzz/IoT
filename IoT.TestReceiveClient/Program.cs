using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace IoT.TestReceiveClient
{
    class Program
    {
        const string topic = "my/topic";

        static long receiveCount = 0;
        static string clientId = Guid.NewGuid().ToString();

        static void Main(string[] args)
        {

            var client = new MqttClient("127.0.0.1");
            client.MqttMsgPublishReceived += MqttMsgPublishReceived;

            client.Connect(clientId);
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });


            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        static void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            //Console.WriteLine(clientId + ": " + DateTime.Now.ToLongTimeString() + " : " + MessageConverter.Deserialize<string>(e.Message));  
            receiveCount++;
            if (receiveCount % 1000 == 0)
            {
                Console.WriteLine("Received: " + clientId + ": " + DateTime.Now.ToLongTimeString() + " : " + receiveCount);
            }
        }
    }
}
