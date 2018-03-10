using System;
using System.Text;
using System.Threading.Tasks;
using IoT.Messages;
using MQTTnet;
using MQTTnet.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IoT.Device.Service
{
    public class DeviceConnectionHandler
    {
        private readonly IMqttServer mqttServer;

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
        }

        public async Task ApplicationMessageReceivedAsync(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine($"ApplicationMessageReceived {e.ClientId}");
            Console.WriteLine($"    Topic   = {e.ApplicationMessage.Topic}");
            Console.WriteLine($"    Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            Console.WriteLine($"    QoS     = {e.ApplicationMessage.QualityOfServiceLevel}");

            await HandleStatusMessages(e);
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

        private async Task HandleStatusMessages(MqttApplicationMessageReceivedEventArgs e)
        {
            if (e.ApplicationMessage != null && e.ApplicationMessage.Topic.EndsWith("/config/request"))
            {
                var statusMsg = MessageConverter.Deserialize<StatusMsg>(e.ApplicationMessage.Payload);

                if (!statusMsg.Configured)
                {
                    var configMsg = new ConfigMsg()
                    {
                        BaseUrl = "127.0.0.1"
                    };

                    var mqttApplicationMessage = new MqttApplicationMessageBuilder()
                        .WithTopic($"/v1/{e.ClientId}/config/response")
                        .WithPayload(MessageConverter.Serialize(configMsg))
                        .WithAtLeastOnceQoS()
                        .Build();

                    await mqttServer.PublishAsync(mqttApplicationMessage);
                }
            }
        }
    }
}
