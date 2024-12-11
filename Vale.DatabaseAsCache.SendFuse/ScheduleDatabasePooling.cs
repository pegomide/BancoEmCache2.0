using log4net;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Vale.DatabaseAsCache.Data.Repository;
using Vale.DatabaseAsCache.Data.TableModels;
using Vale.DatabaseAsCache.Service;
using Vale.DatabaseAsCache.Service.Infrastructure;
using Vale.GetFuseData.ApiService.Models;
using Vale.GetFuseData.ApiService.Services;

namespace Vale.DatabaseAsCache.SendFuse
{
    public class ScheduleDatabasePooling : BackgroundService
    {
        /// <summary>
        /// Logger object
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger("log");

        /// <summary>
        /// Controlling interval valid values
        /// </summary>
        private readonly TimeSpan minimalInterval = TimeSpan.FromSeconds(5);
        private readonly TimeSpan maximalInterval = TimeSpan.FromMinutes(30);

        /// <summary>
        /// Interval for trigger the pooling action
        /// </summary>
        private readonly TimeSpan _poolingInterval;

        /// <summary>
        /// Interface to connect into OpcAPI
        /// </summary>
        private readonly FuseApiInterface _fuseApiInterface;

        /// <summary>
        /// Interface to connect into database
        /// </summary>
        private readonly ColetaFuseRepository _coletaFuseRepository;

        /// <summary>
        /// Interface to connect into OpcAPI
        /// </summary>
        private readonly OpcApiInterface _opcApiInterface;

        /// <summary>
        /// Número máximo permitido de status pendentes para envio ao GPV.
        /// </summary>
        private readonly int _maximoStatusPendentes;

        public ScheduleDatabasePooling()
        {
            // Handling scheduler pooling interval
            if (!TimeSpan.TryParse(ConfigurationManager.AppSettings["SchedulerInterval"], CultureInfo.InvariantCulture, out _poolingInterval))
            {
                _log.Error("Erro ao ler campo de configuração do agendador: SchedulerInterval.");
                throw new FormatException();
            }

            if (TimeSpan.Compare(_poolingInterval, minimalInterval).Equals(-1) || TimeSpan.Compare(_poolingInterval, maximalInterval).Equals(1))
            {
                _log.ErrorFormat("Configuração do agendador possui intervalo fora do aceitável: utilize intervalos entre {0:c} e {1:c}.", minimalInterval, maximalInterval);
                throw new FormatException();
            }

            // Handling FuseApiInterface options
            FuseApiOptions fuseApiOptions = new FuseApiOptions()
            {
                FuseApiUrl = ConfigurationManager.AppSettings["FuseApiUrl"],
            };
            if (fuseApiOptions.FuseApiUrl is null)
            {
                _log.Error($"Erro ao ler campo de configuração da API do OPC: {nameof(fuseApiOptions)}.");
                throw new FormatException();
            }
            _fuseApiInterface = new FuseApiInterface(fuseApiOptions);


            //Handling SqlServer repository connection
            if (ConfigurationManager.ConnectionStrings["Vale.Local.Cache"] != null)
            {
                string connectionStringMain = ConfigurationManager.ConnectionStrings["Vale.Local.Cache"].ConnectionString;
                Regex r = new Regex(@"(\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}", RegexOptions.IgnoreCase);
                string serverIP = r.Match(connectionStringMain).ToString();

                _log.InfoFormat("Estabelecendo conexão com o banco de dados: {0}", serverIP);
                _coletaFuseRepository = new ColetaFuseRepository(connectionStringMain);
                if (!_coletaFuseRepository.IsConnectionOpen())
                {
                    _log.Error($"Não foi possível estabelecer conexão com o banco!");
                    throw new FormatException();
                }
            }
            else
            {
                _log.Error($"Erro ao ler configuração de conexão ao banco de dados.");
                throw new FormatException();
            }

            // Handling OpcApiInterface options
            OpcApiOptions opcApiOptions = new OpcApiOptions()
            {
                OpcApiUrl = ConfigurationManager.AppSettings["OpcApiUrl"],
                HostName = ConfigurationManager.AppSettings["HostName"],
                ServerName = ConfigurationManager.AppSettings["ServerName"],
            };
            if (opcApiOptions.OpcApiUrl is null || opcApiOptions.HostName is null || opcApiOptions.ServerName is null)
            {
                _log.ErrorFormat("Erro ao ler campo de configuração da API do OPC: {0}.", nameof(OpcApiOptions));
                throw new FormatException();
            }
            _opcApiInterface = new OpcApiInterface(opcApiOptions);

            // Handling maximum pending status for GPV
            if (!Int32.TryParse(ConfigurationManager.AppSettings["PendingLimit"], out _maximoStatusPendentes))
            {
                _log.Error("Erro ao ler campo de configuração do agendador: PendingLimit.");
                throw new FormatException();
            }

            _log.InfoFormat("Intervalo entre requisições: {0:c}", _poolingInterval);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var stopWatch = Stopwatch.StartNew();
                DateTime triggerStartTime = DateTime.Now;
                try
                {
                    _log.InfoFormat("### Gatilho de leitura às {0:dd/MM/yyyy HH:mm:ss} ###", triggerStartTime);
                    // ENVIO DE DADOS PENDENTES AO FUSE
                    ColetaFuseData data = _coletaFuseRepository.SelectMostRecentWithStatusPending();
                    if (data != null)
                    {
                        FuseApiRequestBody requestBody = FuseApiService.TransformDatabaseIntoRequestBody(data);
                        _log.InfoFormat("Dado a ser enviado ao Fuse: {0}", requestBody);
                        bool isSentFuse = _fuseApiInterface.PostSendData(requestBody);
                        bool isUpdatedStatus = _coletaFuseRepository.UpdateStatusToDone(data);
                        if (isSentFuse && isUpdatedStatus)
                        {
                            _log.Info("Status atualizado na tabela como enviado (DONE).");
                        }
                    }
                    else
                    {
                        _log.Info("Sem dados pendentes para sincronização com Fuse!");
                    }

                    // VERIFICAÇÃO DE DADOS PENDENTES
                    int countEnviosPendentes = _coletaFuseRepository.CountStatusPending();
                    bool isGpvWithDelay = countEnviosPendentes >= _maximoStatusPendentes;
                    if (isGpvWithDelay)
                    {
                        _log.InfoFormat("Há {0} registros pendentes para envio ao GPV.", countEnviosPendentes);
                    }
                    _opcApiInterface.PostSendGPVDelay(isGpvWithDelay);
                }
                catch (Exception ex)
                {
                    _log.ErrorFormat("Erro no gatilho principal: {0}", ex.ToString().Replace(Environment.NewLine, string.Empty));
                }
                // Garante que o intervalo entre requisições seja respeitado, mesmo com tempo de execução alto
                stopWatch.Stop();
                TimeSpan executionDuration = stopWatch.Elapsed;
                if (executionDuration < _poolingInterval)
                {
                    await Task.Delay(_poolingInterval - executionDuration, stoppingToken);
                }
            }
        }
    }
}