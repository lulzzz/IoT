using System;
using MQTTnet.Server;

namespace IoT.Device.API.Services.Mqtt
{
    public interface IMqttService
    {
        void ValidateConnection(MqttConnectionValidatorContext ctx);

        void InterceptApplicationMessage(MqttApplicationMessageInterceptorContext ctx);

        void InterceptSubscription(MqttSubscriptionInterceptorContext obj);
    }
}
