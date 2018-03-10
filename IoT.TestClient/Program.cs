using System;
using System.Text;
using System.Threading;
using IoT.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace IoT.TestClient
{
    class Program
    {
        private static bool isConfigured = false;

        static void Main(string[] args)
        {
            // create client instance 
            var client = new MqttClient("127.0.0.1");

            // register to message received 
            client.MqttMsgPublishReceived += MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic "/home/temperature" with QoS 2 
            client.Subscribe(new string[] { $"/v1/{clientId}/config/response" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

            while(!isConfigured)
            {
                SendStatus(client, clientId);
                Thread.Sleep(2000);
            }
            SendStatus(client, clientId);

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
            client.Disconnect();
        }

        private static void SendStatus(MqttClient client, string clientId)
        {
            Console.WriteLine($"Send Status: Configured {isConfigured}");
            var msg = new StatusMsg()
            {
                Configured = isConfigured
            };

            // publish a message on "/home/temperature" topic with QoS 2 
            client.Publish($"/v1/{clientId}/config/request", MessageConverter.Serialize(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        private static void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if ((e.Topic != null) && e.Topic.EndsWith("/config/response"))
            {
                var msg = MessageConverter.Deserialize<ConfigMsg>(e.Message);
                Console.WriteLine($"Config received: {Encoding.UTF8.GetString(e.Message)}");
                isConfigured = true;
            }
        }
    }
}
