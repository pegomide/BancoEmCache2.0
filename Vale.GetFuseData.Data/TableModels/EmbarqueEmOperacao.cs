using System;

namespace Vale.GetFuseData.Data.TableModels
{
    public  class EmbarqueEmOperacao
    {
        public long EmbarqueEmOperacaoId { get; set; }

        public string GpvEmbarqueId { get; set; }

        public string NomeNavio { get; set; }

        public string Pier { get; set; }

        public DateTime DataHoraRegistro { get; set; }

        public bool EmRota { get; set; }

        public int CargaTotal { get; set; }

        public string OrigemDados { get; set; }

        public string Produto { get; set; }
    }
}
