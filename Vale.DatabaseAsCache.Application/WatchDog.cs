using log4net;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Vale.DatabaseAsCache.Service;
using Vale.DatabaseAsCache.Service.Infrastructure;

namespace Vale.DatabaseAsCache.Application
{
    public class Watchdog : BackgroundService
    {
        /// <summary>
        /// Logger object
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger("log");

        /// <summary>
        /// Interface to connect into OpcAPI
        /// </summary>
        private readonly OpcApiInterface _opcApiInterface;

        /// <summary>
        /// Fixed interval for trigger the watchdog action (30 seconds)
        /// </summary>
        private readonly TimeSpan _watchdogInterval = new TimeSpan(0, 0, 30);

        /// <summary>
        /// Caminho para o arquivo local onde os dados serão armazenados
        /// </summary>
        private bool _lastValue = true;


        public Watchdog()
        {
            // Handling OpcApiInterface options
            OpcApiOptions opcApiOptions = new OpcApiOptions()
            {
                OpcApiUrl = ConfigurationManager.AppSettings["OpcApiUrl"],
                HostName = ConfigurationManager.AppSettings["HostName"],
                ServerName = ConfigurationManager.AppSettings["ServerName"],
            };
            if (opcApiOptions.OpcApiUrl is null || opcApiOptions.HostName is null || opcApiOptions.ServerName is null)
            {
                _log.Error($"Erro ao ler campo de configuração da API do OPC: {nameof(OpcApiOptions)}.");
                throw new FormatException();
            }
            _opcApiInterface = new OpcApiInterface(opcApiOptions);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var stopWatch = Stopwatch.StartNew();
                try
                {
                    _log.InfoFormat("### Gatilho de watchdog às {0:dd/MM/yyyy HH:mm:ss} ###", DateTime.Now);
                    if (_opcApiInterface.PostSendWatdogSignal(_lastValue))
                    {
                        _log.DebugFormat("Sinal de watchdog enviado com sucesso: {0}", _lastValue);
                        _lastValue = !_lastValue;
                    }
                    else
                    {
                        _log.ErrorFormat("Erro ao enviar sinal de watchdog. Tentativa de envio: {0}", _lastValue);
                    }
                }
                catch (Exception ex)
                {
                    _log.ErrorFormat("Erro no gatilho watchdog: {0}", ex.ToString().Replace(Environment.NewLine, string.Empty));
                }

                // Garante que o intervalo entre requisições seja respeitado, mesmo com tempo de execução alto
                stopWatch.Stop();
                TimeSpan executionDuration = stopWatch.Elapsed;
                if (executionDuration < _watchdogInterval)
                {
                    await Task.Delay(_watchdogInterval - executionDuration, stoppingToken);
                }
            }
        }
    }
}