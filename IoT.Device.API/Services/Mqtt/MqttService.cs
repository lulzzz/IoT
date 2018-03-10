using System;
using MQTTnet.Server;

namespace IoT.Device.API.Services.Mqtt
{
    public class MqttService : IMqttService
    {
        public MqttService()
        {
        }

        public void InterceptApplicationMessage(MqttApplicationMessageInterceptorContext ctx)
        {
            throw new NotImplementedException();
        }

        public void InterceptSubscription(MqttSubscriptionInterceptorContext obj)
        {
            throw new NotImplementedException();
        }

        public void ValidateConnection(MqttConnectionValidatorContext ctx)
        {
            throw new NotImplementedException();
        }
    }
}
