using System;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Server;

namespace IoT.Device.Service
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var mqttServer = new MqttFactory().CreateMqttServer();

            var handler = new DeviceConnectionHandler(mqttServer);
            mqttServer.ClientConnected += handler.ClientConnected;
            mqttServer.ClientDisconnected += handler.ClientDisconnected;
            mqttServer.ApplicationMessageReceived += async (s, e) => await handler.ApplicationMessageReceivedAsync(s, e);
            mqttServer.ClientSubscribedTopic += handler.ClientSubscribedTopic;
            mqttServer.ClientUnsubscribedTopic += handler.ClientUnsubscribedTopic;

            await mqttServer.StartAsync(new MqttServerOptions());
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
            await mqttServer.StopAsync();
        }
    }
}
