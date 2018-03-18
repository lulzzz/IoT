using System;
using System.Threading;
using System.Threading.Tasks;
using IoT.Messages;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace IoT.TestClient
{
    public class Client : IDisposable
    {
        readonly int waitTime;
        readonly string subscribeTopic;
        readonly string publishTopic;
        readonly long nrOfMessages;

        readonly MqttClient client;
        readonly string clientId;

        public long ReceiveCount
        {
            get;
            set;
        }

        public long SendCount
        {
            get;
            set;
        }

        public Client(string clientId, int waitTime, long nrOfMessages, string subscribeTopic, string publishTopic)
        {
            this.nrOfMessages = nrOfMessages;
            this.publishTopic = publishTopic;
            this.subscribeTopic = subscribeTopic;
            this.waitTime = waitTime;
            this.clientId = clientId;

            client = new MqttClient("127.0.0.1");
            client.MqttMsgPublishReceived += MqttMsgPublishReceived;

            client.Connect(clientId);
            client.Subscribe(new string[] { subscribeTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        }

        public void Start()
        {
            for (int i = 0; i < nrOfMessages; i++)
            {
                string msg = clientId + " : " + i;
                client.Publish(publishTopic, MessageConverter.Serialize(msg), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
                SendCount++;
                if(SendCount % 100 == 0)
                {
                    Console.WriteLine("Sent: " + clientId + ": " + DateTime.Now.ToLongTimeString() + " : " + SendCount);  
                }
                Thread.Sleep(waitTime);
            }

            Console.WriteLine($"{clientId} Send    : {SendCount}");
            Console.WriteLine($"{clientId} Received: {ReceiveCount}");
        }

        void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            //Console.WriteLine(clientId + ": " + DateTime.Now.ToLongTimeString() + " : " + MessageConverter.Deserialize<string>(e.Message));  
            ReceiveCount++;
            if(ReceiveCount % 100 == 0)
            {
                Console.WriteLine("Received: " + clientId + ": " + DateTime.Now.ToLongTimeString() + " : " + ReceiveCount);     
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if(client.IsConnected)
                    {

                        client.Disconnect();   

                        Console.WriteLine($"Dispose: {clientId} Send    : {SendCount}");
                        Console.WriteLine($"Dispose: {clientId} Received: {ReceiveCount}");
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Client() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
