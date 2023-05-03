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

namespace Vale.DatabaseAsCache.Application
{
    public class ScheduleOpcRequest : BackgroundService
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
        private readonly OpcApiInterface _opcApiInterface;

        /// <summary>
        /// Interface to connect into database
        /// </summary>
        private readonly ColetaFuseRepository _coletaFuseRepository;

        public ScheduleOpcRequest()
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
                    var response = _opcApiInterface.PostTemNovoRegistro();
                    if (response is null)
                    {
                        _log.Info($"Erro ao requisitar verificação de novo registro");
                    }
                    else
                    {
                        if (OpcApiService.TemNovoRegistro(response))
                        {
                            _log.Info($"Novo registro disponível no CLP");
                            response = _opcApiInterface.PostVerificaPier();
                            if (response is null)
                            {
                                _log.Info($"Erro ao requisitar verificação do pier");
                            }
                            else
                            {
                                PierEnum pier = OpcApiService.ExtraiPier(response);
                                ColetaFuseData data = null;
                                switch (pier)
                                {
                                    case PierEnum.North:
                                        response = _opcApiInterface.PostDataNorth();
                                        data = OpcApiService.ExtractDataNorth(response, triggerDate);
                                        break;
                                    case PierEnum.South:
                                        response = _opcApiInterface.PostDataSouth();
                                        data = OpcApiService.ExtractDataSouth(response, triggerDate);
                                        break;
                                    default:
                                        _log.Error("Tipo de pier não identificado!");
                                        break;
                                }
                                if (data != null)
                                {
                                    var rowsInserted = _coletaFuseRepository.Insert(data);
                                    if (rowsInserted > 0)
                                    {
                                        _log.Info($"Dado foi salvo no banco!");
                                        _log.Debug($"{rowsInserted} dado(s) inserido(s): {data}");
                                    }
                                    else
                                    {
                                        _log.Info($"Erro ao salvar dados no banco.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            _log.Info($"Nenhum registro disponível no CLP");
                        }
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