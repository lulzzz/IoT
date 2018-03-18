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
            mqttServer.ClientSubscribedTopic += handler.ClientSubscribedTopic;
            mqttServer.ClientUnsubscribedTopic += handler.ClientUnsubscribedTopic;
            mqttServer.ApplicationMessageReceived += handler.ApplicationMessageReceived;

            await mqttServer.StartAsync(new MqttServerOptions());

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
            await mqttServer.StopAsync();
        }
    }
}
