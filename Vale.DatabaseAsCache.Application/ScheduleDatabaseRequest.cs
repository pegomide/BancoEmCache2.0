using log4net;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Vale.DatabaseAsCache.ApiService;
using Vale.DatabaseAsCache.Data.Repository;
using Vale.DatabaseAsCache.Service;
using Vale.DatabaseAsCache.Service.Infrastructure;
using Vale.DatabaseAsCache.Service.Models;
using Vale.DatabaseAsCache.Service.Services;

namespace Vale.DatabaseAsCache.Application
{
    public class ScheduleDatabaseRequest : BackgroundService
    {
        private static readonly ILog _log = LogManager.GetLogger("log");

        private readonly TimeSpan _poolingInterval;
        private readonly OpcApiOptions _fuseApiOptions;

        private readonly EmbarqueDadosQualidadeRepository _embarqueRepositoryMain;
        private readonly LoteDadosQualidadeRepository _loteRepositoryMain;
        private readonly EmbarqueDadosQualidadeRepository _embarqueRepositorySecondary;
        private readonly LoteDadosQualidadeRepository _loteRepositorySecondary;

        private readonly TimeSpan minimalInterval = TimeSpan.FromSeconds(5);
        private readonly TimeSpan maximalInterval = TimeSpan.FromMinutes(30);

        public ScheduleDatabaseRequest()
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

            // Handling FuseAPI options
            _fuseApiOptions = new OpcApiOptions()
            {
                OpcApiUrl = ConfigurationManager.AppSettings["OpcApiUrl"],
                CertificateFile = ConfigurationManager.AppSettings["CertificateFileName"],
            };
            if (_fuseApiOptions.OpcApiUrl is null || _fuseApiOptions.CertificateFile is null)
            {
                _log.Error($"Erro ao ler campo de configuração da API do fuse: {nameof(OpcApiOptions)}.");
                throw new FormatException();
            }

            //Handling SqlServer repository
            if (ConfigurationManager.ConnectionStrings["Vale.Local.Cache"] != null)
            {
                string connectionStringMain = ConfigurationManager.ConnectionStrings["Vale.Scada.Main"].ConnectionString;
                Regex r = new Regex(@"(\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}", RegexOptions.IgnoreCase);
                string serverIP = r.Match(connectionStringMain).ToString();

                _log.Info($"Estabelecendo conexão com o banco de dados: {serverIP}");
                _embarqueRepositoryMain = new EmbarqueDadosQualidadeRepository(connectionStringMain);
                _loteRepositoryMain = new LoteDadosQualidadeRepository(connectionStringMain);
                if (!_embarqueRepositoryMain.IsConnectionOpen())
                {
                    _log.Error($"Não foi possível estabelecer conexão com o banco!");
                }
            }

            _log.Info($"Intervalo entre requisições: {_poolingInterval:c}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _log.Info($"### Gatilho de leitura às {DateTimeOffset.Now:dd/MM/yyyy HH:mm:ss} ###");

                // Requesting Embarque
                _log.Info("## Embarque ##");
                FuseApiIntegration api = new FuseApiIntegration(_fuseApiOptions.CertificateFile);
                EmbarqueDadosQualidadeXML embarqueDadosQualidade = api.GetEmbarqueDadosQualidade(_fuseApiOptions.EmbarqueDadosQualidadePath);
                if (embarqueDadosQualidade != null)
                {
                    _log.Info("Requisição de dados de embarque feito com sucesso!");
                    _log.Debug($"Dados do embarque: {embarqueDadosQualidade.ToString().Replace(Environment.NewLine, string.Empty)}");
                    if (_embarqueRepositoryMain != null)
                    {
                        EmbarqueDadosQualidadeService embarqueServiceMain = new EmbarqueDadosQualidadeService(_embarqueRepositoryMain);
                        DatabaseFeedback dbReturn = embarqueServiceMain.SaveEmbarqueDadosQualidade(embarqueDadosQualidade);

                        if (dbReturn != null)
                        {
                            _log.Info($"Banco Principal - Deletados: {dbReturn.QuantRowsDeleted}, Inseridos: {dbReturn.QuantRowsInserted}, Perdidos: {dbReturn.QuantRowsFailed}.");
                        }
                    }
                    if (_embarqueRepositorySecondary != null)
                    {
                        EmbarqueDadosQualidadeService embarqueServiceSecondary = new EmbarqueDadosQualidadeService(_embarqueRepositorySecondary, true);
                        DatabaseFeedback dbReturn = embarqueServiceSecondary.SaveEmbarqueDadosQualidade(embarqueDadosQualidade);

                        if (dbReturn != null)
                        {
                            _log.Info($"Banco Secundário - Deletados: {dbReturn.QuantRowsDeleted}, Inseridos: {dbReturn.QuantRowsInserted}, Perdidos: {dbReturn.QuantRowsFailed}.");
                        }
                    }
                }

                // Requesting Lote
                _log.Info("## Lote ##");
                LoteDadosQualidadeXML loteDadosQualidade = api.GetLoteDadosQualidade(_fuseApiOptions.LoteDadosQualidadePath);
                if (loteDadosQualidade != null)
                {
                    _log.Info("Requisição de dados do lote feito com sucesso!");
                    _log.Debug($"Dados do lote: {loteDadosQualidade.ToString().Replace(Environment.NewLine, string.Empty)}");
                    if (_loteRepositoryMain != null)
                    {
                        LoteDadosQualidadeService loteServiceMain = new LoteDadosQualidadeService(_loteRepositoryMain);
                        DatabaseFeedback dbReturn = loteServiceMain.SaveLoteDadosQualidade(loteDadosQualidade);

                        if (dbReturn != null)
                        {
                            _log.Info($"Banco Principal - Deletados: {dbReturn.QuantRowsDeleted}, Inseridos: {dbReturn.QuantRowsInserted}, Perdidos: {dbReturn.QuantRowsFailed}.");
                        }
                    }
                    if (_loteRepositorySecondary != null)
                    {
                        LoteDadosQualidadeService loteServiceSecondary = new LoteDadosQualidadeService(_loteRepositorySecondary, true);
                        DatabaseFeedback dbReturn = loteServiceSecondary.SaveLoteDadosQualidade(loteDadosQualidade);

                        if (dbReturn != null)
                        {
                            _log.Info($"Banco Principal - Deletados: {dbReturn.QuantRowsDeleted}, Inseridos: {dbReturn.QuantRowsInserted}, Perdidos: {dbReturn.QuantRowsFailed}.");
                        }
                    }
                }

                await Task.Delay(_poolingInterval, stoppingToken);
            }
        }
    }
}