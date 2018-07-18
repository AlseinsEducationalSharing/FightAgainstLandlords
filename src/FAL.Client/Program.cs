using System;
using System.Threading.Tasks;
using Alsein.Utilities;
using Autofac;
using FAL.Client.Services;
using Microsoft.AspNetCore.SignalR.Client;

namespace FAL.Client
{
    class Program
    {
        static async Task Main(string[] args) => await ConfigureServices().Resolve<MainService>().Main(args);

        private static IContainer ConfigureServices()
        {
            var builder = new ContainerBuilder();
            var container = default(IContainer);
            builder.Register(x => container).SingleInstance();
            builder.RegisterType<HubConnectionBuilder>().SingleInstance();
            builder.Register(x => x.Resolve<HubConnectionBuilder>().WithUrl("http://127.0.0.1:5000/fal").Build()).SingleInstance();
            builder.RegisterAllServices();
            container = builder.Build();
            return container;
        }
    }
}
