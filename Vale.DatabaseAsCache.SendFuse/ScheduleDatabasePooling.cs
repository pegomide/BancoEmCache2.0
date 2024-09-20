using log4net;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
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
            if (!TimeSpan.TryParse(ConfigurationManager.AppSettings["SchedulerInterval"], out _poolingInterval))
            {
                _log.Error("Erro ao ler campo de configuração do agendador: SchedulerInterval.");
                throw new FormatException();
            }

            if (TimeSpan.Compare(_poolingInterval, minimalInterval).Equals(-1) || TimeSpan.Compare(_poolingInterval, maximalInterval).Equals(1))
            {
                _log.Error($"Configuração do agendador possui intervalo fora do aceitável: utilize intervalos entre {minimalInterval:c} e {maximalInterval:c}.");
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

                _log.Info($"Estabelecendo conexão com o banco de dados: {serverIP}");
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
                _log.Error($"Erro ao ler campo de configuração da API do OPC: {nameof(OpcApiOptions)}.");
                throw new FormatException();
            }
            _opcApiInterface = new OpcApiInterface(opcApiOptions);

            // Handling maximum pending status for GPV
            if (!Int32.TryParse(ConfigurationManager.AppSettings["PendingLimit"], out _maximoStatusPendentes))
            {
                _log.Error("Erro ao ler campo de configuração do agendador: PendingLimit.");
                throw new FormatException();
            }

            _log.Info($"Intervalo entre requisições: {_poolingInterval:c}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime triggerStartTime = DateTime.Now;
                try
                {
                    _log.Info($"### Gatilho de leitura às {triggerStartTime:dd/MM/yyyy HH:mm:ss} ###");
                    // ENVIO DE DADOS PENDENTES AO FUSE
                    ColetaFuseData data = _coletaFuseRepository.SelectMostRecentWithStatusPending();
                    if (data != null)
                    {
                        FuseApiRequestBody requestBody = FuseApiService.TransformDatabaseIntoRequestBody(data);
                        _log.Info($"Dado a ser enviado ao Fuse: {requestBody}");
                        if (_fuseApiInterface.PostSendData(requestBody))
                        {
                            if (_coletaFuseRepository.UpdateStatusToDone(data))
                            {
                                _log.Info("Status atualizado na tabela como enviado (DONE).");
                            }
                        }
                    }
                    else
                    {
                        _log.Info($"Sem dados pendentes para sincronização com Fuse!");
                    }

                    // VERIFICAÇÃO DE DADOS PENDENTES
                    int countEnviosPendentes = _coletaFuseRepository.CountStatusPending();
                    bool isGpvWithDelay = countEnviosPendentes > _maximoStatusPendentes;
                    if (isGpvWithDelay)
                    {
                        _log.Info($"Há {countEnviosPendentes} registros pendentes para envio ao GPV.");
                    }
                    _opcApiInterface.PostSendGPVDelay(isGpvWithDelay);
                }
                catch (Exception ex)
                {
                    _log.Error($"Erro no gatilho principal: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
                }
                // Garante que o intervalo entre requisições seja respeitado, mesmo com tempo de execução alto
                TimeSpan executionDuration = DateTime.Now - triggerStartTime;
                if (executionDuration < _poolingInterval)
                {
                    await Task.Delay(_poolingInterval - executionDuration, stoppingToken);
                }
            }
        }
    }
}