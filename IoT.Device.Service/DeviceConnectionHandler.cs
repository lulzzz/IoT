using System;
using System.Collections.Concurrent;
using MQTTnet;
using MQTTnet.Server;

namespace IoT.Device.Service
{
    public class DeviceConnectionHandler
    {
        private readonly IMqttServer mqttServer;

        public long Messages
        {
            get;
            set;
        }

        public ConcurrentDictionary<string, long> MessagePerClient => new ConcurrentDictionary<string, long>();

        public DeviceConnectionHandler(IMqttServer mqttServer)
        {
            this.mqttServer = mqttServer;
        }

        public void ClientConnected(object sender, MqttClientConnectedEventArgs e)
        {
            Console.WriteLine($"Client connected: {e.Client.ClientId}, Mqtt V{e.Client.ProtocolVersion}");
        }

        public void ClientDisconnected(object sender, MqttClientDisconnectedEventArgs e)
        {
            Console.WriteLine($"Client disconnected: {e.Client.ClientId}");

            Console.WriteLine($"Messages : {e.Client.ClientId} : {MessagePerClient[e.Client.ClientId]}");
        }

        public void ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            //Console.WriteLine($"ApplicationMessageReceived {e.ClientId}");
            //Console.WriteLine($"    Topic   = {e.ApplicationMessage.Topic}");
            //Console.WriteLine($"    Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            //Console.WriteLine($"    QoS     = {e.ApplicationMessage.QualityOfServiceLevel}");


            try
            {
                MessagePerClient.AddOrUpdate(e.ClientId, 1, (key, oldValue) => oldValue + 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Messages++;
            if((Messages % 1000) == 0)
            {
                Console.WriteLine(DateTime.Now.ToLongTimeString() + ": " + Messages);
            }
        }

        public void ClientSubscribedTopic(object sender, MqttClientSubscribedTopicEventArgs e)
        {
            Console.WriteLine($"ClientSubscribedTopic {e.ClientId}");
            Console.WriteLine($"    Topic   = {e.TopicFilter.Topic}");
            Console.WriteLine($"    QoS     = {e.TopicFilter.QualityOfServiceLevel}");
        }

        public void ClientUnsubscribedTopic(object sender, MqttClientUnsubscribedTopicEventArgs e)
        {
            Console.WriteLine($"ClientUnsubscribedTopic {e.ClientId}");
            Console.WriteLine($"    TopicFilter  = {e.TopicFilter}");
        }

    }
}
