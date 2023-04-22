using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using log4net;

namespace Vale.DatabaseAsCache.Application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog _log = LogManager.GetLogger("log");

            _log.Info("INICIANDO Vale.DatabaseAsCache..Application ...");
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    services.AddHostedService<ScheduleDatabaseRequest>();
                })
                .Build();

            host.Run();
        }
    }
}

