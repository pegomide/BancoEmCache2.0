using log4net;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Vale.GetFuseData.ApiService;
using Vale.GetFuseData.Data.Repository;
using Vale.GetFuseData.Data.TableModels;
using Vale.GetFuseData.Service.Models;

namespace Vale.GetFuseData.Service.Services
{
    public interface ILoteDadosQualidadeService
    {
        Task<int> SaveLoteDadosQualidade(LoteDadosQualidadeXML loteDadosXml);
    }
    public class LoteDadosQualidadeService
    {
        /// <summary>
        /// Interface com log do sistema.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger("log");

        /// <summary>
        /// Categoria de conexão com banco: primário ou secundário.
        /// </summary>
        private readonly string _databaseCategory;

        /// <summary>
        /// Interface com tabela do banco.
        /// </summary>
        private readonly ILoteDadosQualidadeRepository _loteDadosRepository;

        /// <summary>
        /// Construtor padrão.
        /// </summary>
        /// <param name="loteDadosRepository">Interface do repositório da tabela LoteDadosQualidade.</param>
        /// <param name="secondary">Enviar verdadeiro caso for conexão de banco secundário.</param>
        public LoteDadosQualidadeService(ILoteDadosQualidadeRepository loteDadosRepository, bool secondary = false)
        {
            _loteDadosRepository = loteDadosRepository;
            _databaseCategory = secondary ? "secundário" : "principal";
        }

        public DatabaseFeedback SaveLoteDadosQualidade(LoteDadosQualidadeXML loteDadosXml)
        {
            try
            {
                int countInserted = 0, countDeleted = 0, countFailed = 0;
                if (loteDadosXml.MessageField != null)
                {
                    countDeleted = _loteDadosRepository.ClearTable().Result;

                    foreach (var lot in loteDadosXml.MessageField.LotList.LotListElement)
                    {
                        try
                        {
                            double totalProgramado = Convert.ToDouble(lot.QtyTrans, new CultureInfo("pt-BR"));

                            LoteDadosQualidade loteDadosQualidade = MontaDadosLote(lot.FstLstWag, lot.LotId, lot.LotArrvlDt, lot.PrefixID, lot.ProdID, lot.NumVehicTrans, totalProgramado);

                            countInserted += _loteDadosRepository.Insert(loteDadosQualidade).Result;
                        }
                        catch (FormatException ex)
                        {
                            _log.Info($"Não foi possível converter data: {lot.LotArrvlDt}");
                            _log.Info($@"Dado de embarque perdido: {JsonConvert.SerializeObject(lot)}");

                            _log.ErrorFormat(ex.ToString());
                            countFailed++;
                        }
                        catch (Exception ex)
                        {
                            _log.Error(ex.ToString());
                            countFailed++;
                        }
                    }
                }

                if (loteDadosXml.Status != "OK")
                {
                    _log.Error($"LoteDadosQualidade retornou um status não esperado: {loteDadosXml.Status} - {loteDadosXml.MessageError ?? string.Empty}");
                }

                return new DatabaseFeedback(countInserted, 0, countDeleted, countFailed);
            }
            catch (Exception ex)
            {
                _log.Info($"Banco {_databaseCategory}: Não foi possível salvar informações do embarque: SaveLoteDadosQualidade");
                _log.Error(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Monta dados de qualidadde do lote.
        /// </summary>
        /// <param name="firstAndLastWagon">Primeiro e último vagão, concatenados.</param>
        /// <param name="lodId">ID do lote.</param>
        /// <param name="lotArrivalDate">Data de chegada do lote.</param>
        /// <param name="prefixId">ID do Prefixo.</param>
        /// <param name="productId">ID do produto.</param>
        /// <param name="numberVehiclesInTransit">Número de veículos em trânsito.</param>
        /// <param name="quantTransit">Quantidade de carga em trânsito.</param>
        /// <returns>Objeto com dados a serem salvos no banco.</returns>
        private static LoteDadosQualidade MontaDadosLote(string firstAndLastWagon, string lodId, string lotArrivalDate, string prefixId, string productId, int numberVehiclesInTransit, double quantTransit)
        {
            string[] wagons = new string[0];
            if (firstAndLastWagon != null)
            {
                wagons = firstAndLastWagon.Split(';');
            }
            DateTime previsao = DateTime.ParseExact(lotArrivalDate, "d/M/yyyy H:m", CultureInfo.InvariantCulture);
            LoteDadosQualidade loteDadosQualidade = new LoteDadosQualidade()
            {
                GpvLoteId = lodId,
                MatriculaPrimeiroVagao = wagons.Length > 0 ? wagons[0] : string.Empty,
                PrefixoTrem = prefixId,
                PrevisaoChegada = previsao,
                DataHoraRegistro = DateTime.Now,
                Produto = productId,
                VagoesProgramados = numberVehiclesInTransit,
                TotalProgramado = quantTransit,
                MatriculaUltimoVagao = wagons.Length > 1 ? wagons[1] : string.Empty,
            };
            return loteDadosQualidade;
        }
    }
}
