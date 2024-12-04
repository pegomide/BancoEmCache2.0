using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using log4net;
using System;

namespace Vale.DatabaseAsCache.SendFuse
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog _log = LogManager.GetLogger("log");

            _log.Info("INICIANDO Vale.DatabaseAsCache.SendFuse ...");
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<ScheduleDatabasePooling>();
                })
                .Build();

            host.Run();
            Console.ReadLine();
        }
    }
}

