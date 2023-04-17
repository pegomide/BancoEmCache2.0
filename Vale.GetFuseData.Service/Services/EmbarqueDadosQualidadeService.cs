using Vale.GetFuseData.Data.Repository;
using Vale.GetFuseData.Data.TableModels;
using Vale.GetFuseData.Service.Models;
using System.Threading.Tasks;
using System;
using log4net;
using System.Globalization;
using System.Linq;
using Vale.GetFuseData.ApiService;
using static Vale.GetFuseData.Service.Models.EmbarqueDadosQualidadeXML.Message.Pier.PierData.Shipment.ShipmentData.Product;
using Newtonsoft.Json;

namespace Vale.GetFuseData.Service.Services
{
    public interface IEmbarqueDadosQualidadeService
    {
        Task<int> SaveEmbarqueDadosQualidade(EmbarqueDadosQualidadeXML embarqueDadosXml);
    }
    public class EmbarqueDadosQualidadeService
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
        private readonly IEmbarqueDadosQualidadeRepository _embarqueRepository;

        /// <summary>
        /// Construtor padrão.
        /// </summary>
        /// <param name="embarqueRepository">Interface do repositório da tabela EmbarqueDadosQualidade.</param>
        /// <param name="secondary">Enviar verdadeiro caso for conexão de banco secundário.</param>
        public EmbarqueDadosQualidadeService(IEmbarqueDadosQualidadeRepository embarqueRepository, bool secondary = false)
        {
            _embarqueRepository = embarqueRepository;
            _databaseCategory = secondary ? "secundário" : "principal";
        }

        /// <summary>
        /// Salva os dados de embarque conforme mensagem recebida.
        /// </summary>
        /// <param name="embarqueDadosXml">Classe com dados de embarque oriunda do XML recebido.</param>
        public DatabaseFeedback SaveEmbarqueDadosQualidade(EmbarqueDadosQualidadeXML embarqueDadosXml)
        {
            try
            {
                int countInserted = 0, countDeleted = 0, countFailed = 0;
                if (embarqueDadosXml.MessageField != null)
                {
                    countDeleted = _embarqueRepository.ClearTable().Result;
                    foreach (var pier in embarqueDadosXml.MessageField.PierList.PierListElement)
                    {
                        foreach (var shipment in pier.ShipmentList.ShipmentListElement)
                        {
                            try
                            {
                                EmbarqueDadosQualidade embarqueDados = MontaEmbarqueDados(pier.PierId, shipment.EventDt, shipment.ShipID, shipment.ShipName, shipment.ProductList.ProductListElement);

                                countInserted += _embarqueRepository.Insert(embarqueDados).Result;
                            }
                            catch (FormatException ex)
                            {
                                _log.Info($"Não foi possível converter data: {shipment.EventDt}");
                                _log.Info($@"Dado de embarque perdido: {JsonConvert.SerializeObject(shipment)}");
                                _log.Error(ex.ToString());
                                countFailed++;
                            }
                            catch (Exception ex)
                            {
                                _log.Error(ex.ToString());
                                countFailed++;
                            }
                        }
                    }
                }

                if (embarqueDadosXml.Status != "OK")
                {
                    _log.Error($"EmbarqueDadosQualidade retornou um status não esperado: {embarqueDadosXml.Status} - {embarqueDadosXml.MessageError ?? string.Empty}");
                }

                return new DatabaseFeedback(countInserted, 0, countDeleted, countFailed);
            }
            catch (Exception ex)
            {
                _log.Info($"Banco {_databaseCategory}: Não foi possível salvar informações do embarque: SaveEmbarqueDadosQualidade");
                _log.Error(ex.ToString());

                return null;
            }
        }

        /// <summary>
        /// Monta o objeto com os dados de Embarque.
        /// </summary>
        /// <param name="pierId">Identificação do pier.</param>
        /// <param name="eventDate">Data do evento.</param>
        /// <param name="shipID">Identificação do navio.</param>
        /// <param name="shipName">Nome do navio.</param>
        /// <param name="products">Lista de produtos.</param>
        /// <returns>Objeto com os dados do embarque a serem salvos no banco.</returns>
        private static EmbarqueDadosQualidade MontaEmbarqueDados(string pierId, string eventDate, string shipID, string shipName, ProductData[] products)
        {
            string prodId = null;
            int cargaTotalPorNavio = 0;

            foreach (var produto in products)
            {
                if (prodId == null)
                {
                    // Apenas o primeiro produto válido por navio é listado.
                    // Demais nomes de produto do mesmo navio são DESCONSIDERADOS.
                    prodId = produto.ProdId;
                }
                cargaTotalPorNavio += produto.ClientList.ClienteListElement.Sum(client => (int)client.FcstQty + (int)client.ActQty);
            }
            DateTime previsao = DateTime.ParseExact(eventDate, "d/M/yyyy H:m", CultureInfo.InvariantCulture);

            EmbarqueDadosQualidade embarqueDados = new EmbarqueDadosQualidade()
            {
                GpvEmbarqueId = shipID,
                NomeNavio = shipName,
                Pier = pierId,
                PrevisaoAtracacao = previsao,
                DataHoraRegistro = DateTime.Now,
                Produto = prodId,
                AmostrasProgramadas = 0, // GPV Portos ainda não envia esta informação, logo o Infoplus.21 a registra com valor vazio. Fonte: ET_CTA_XXX - CR110561-[DIOP_TIG]-Integracao_Qualidade_Embarque_Lotes_v01
                CargaTotal = cargaTotalPorNavio
            };
            return embarqueDados;
        }
    }
}
