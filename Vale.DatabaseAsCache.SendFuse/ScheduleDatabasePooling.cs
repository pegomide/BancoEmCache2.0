using log4net;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Vale.DatabaseAsCache.ApiService.Models;
using Vale.DatabaseAsCache.Data.Repository;
using Vale.DatabaseAsCache.Data.TableModels;
using Vale.DatabaseAsCache.Service;
using Vale.DatabaseAsCache.Service.Infrastructure;
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

            _log.Info($"Intervalo entre requisições: {_poolingInterval:c}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var triggerDate = DateTime.Now;
                try
                {
                    _log.Info($"### Gatilho de leitura às {triggerDate:dd/MM/yyyy HH:mm:ss} ###");
                    var data = _coletaFuseRepository.SelectMostRecentWithStatusPending();
                    if (data != null)
                    {
                        var requestBody = FuseApiService.TransformDatabaseIntoRequestBody(data);
                        if (_fuseApiInterface.PostSendData(requestBody))
                        {
                            _log.Info($"Dado enviado ao Fuse: {{BoardingCode:{data.BOARDING_CODE}, PierCode:{data.PIER_CODE}, IncNumber:{data.INCREMENT_NUMBER}}}");
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
                }
                catch (Exception ex)
                {
                    _log.Error($"Erro no gatilho principal: {ex.ToString().Replace(Environment.NewLine, string.Empty)}");
                }
                var calculationTime = DateTime.Now - triggerDate;
                if (calculationTime < _poolingInterval)
                {
                    await Task.Delay(_poolingInterval - calculationTime, stoppingToken);
                }
            }
        }
    }
}