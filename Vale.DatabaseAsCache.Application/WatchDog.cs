using log4net;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using System;
using System.Configuration;
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
        /// Application path to registry key
        /// </summary>
        private string _registryPath = WindowsRegistryInfo.BasePath + @"\WatchDog";

        /// <summary>
        /// Registry key to store the last value sent
        /// </summary>
        private string _registryKey = "LastValueSent";

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
            bool lastValueSent = (bool)(Registry.GetValue(_registryPath, _registryKey, 0) ?? 0);
            bool currentValueToSend = !lastValueSent;
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime triggerStartTime = DateTime.Now;
                try
                {
                    _log.Info($"### Gatilho de watchdog às {triggerStartTime:dd/MM/yyyy HH:mm:ss} ###");
                    if (_opcApiInterface.PostSendWatdogSignal(currentValueToSend))
                    {
                        Registry.SetValue(_registryPath, _registryKey, currentValueToSend);
                        _log.Debug($"Sinal de watchdog enviado com sucesso. Valor enviado: {currentValueToSend}");
                    }
                    else
                    {
                        _log.Error($"Erro ao enviar sinal de watchdog. Tentativa de envio: {currentValueToSend}");
                    }
                }
                catch (Exception ex)
                {
                    _log.Error($"Erro no gatilho watchdog: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
                }
                var executionDuration = DateTime.Now - triggerStartTime;
                if (executionDuration < _watchdogInterval)
                {
                    await Task.Delay(_watchdogInterval - executionDuration, stoppingToken);
                }
            }
        }
    }
}