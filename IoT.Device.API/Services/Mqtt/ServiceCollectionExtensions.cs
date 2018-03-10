using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet.Adapter;
using MQTTnet.AspNetCore;
using MQTTnet.Diagnostics;
using MQTTnet.Implementations;
using MQTTnet.Server;

namespace IoT.Device.API.Services.Mqtt
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMqtt(this IServiceCollection services)
        {
            IMqttService mqttService = new MqttService();
            services.AddSingleton<IMqttService>(mqttService);

            var certificate = new X509Certificate(@"C:\certs\test\test.cer", "");

            var options = new MqttServerOptionsBuilder()
                .WithConnectionValidator(mqttService.ValidateConnection)
                .WithApplicationMessageInterceptor(mqttService.InterceptApplicationMessage)
                .WithSubscriptionInterceptor(mqttService.InterceptSubscription)
                .WithEncryptionCertificate(certificate.Export(X509ContentType.Cert))
                .Build();

            services.AddSingleton(options);
            services.AddSingleton<IMqttNetLogger>(new MqttNetLogger());
            services.AddSingleton<MqttHostedServer>();
            services.AddSingleton<IHostedService>(s => s.GetService<MqttHostedServer>());
            services.AddSingleton<IMqttServer>(s => s.GetService<MqttHostedServer>());

            services.AddSingleton<MqttWebSocketServerAdapter>();
            services.AddSingleton<MqttServerAdapter>();
            services.AddSingleton<IEnumerable<IMqttServerAdapter>>(s =>
            {
                List<IMqttServerAdapter> adapters = new List<IMqttServerAdapter>();
                adapters.Add(s.GetService<MqttServerAdapter>());
                adapters.Add(s.GetService<MqttWebSocketServerAdapter>());
                return adapters;
            });

            return services;
        }
    }
}
