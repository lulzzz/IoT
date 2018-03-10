using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet.Adapter;
using MQTTnet.AspNetCore;
using MQTTnet.Diagnostics;
using MQTTnet.Implementations;
using MQTTnet.Server;

namespace IoT.Device.API.Services.Mqtt
{
    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder UseMqtt(this IApplicationBuilder app)
        {
            app.UseMqttEndpoint();
            return app;
        }
    }
}
