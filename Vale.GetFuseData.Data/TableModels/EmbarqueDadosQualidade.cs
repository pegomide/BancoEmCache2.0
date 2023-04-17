using System;

namespace Vale.GetFuseData.Data.TableModels
{
    public class EmbarqueDadosQualidade
    {
        public long EmbarqueDadosQualidadeId { get; set; }

        public string GpvEmbarqueId { get; set; }

        public string NomeNavio { get; set; }

        public string Pier { get; set; }

        public DateTime PrevisaoAtracacao { get; set; }

        public DateTime DataHoraRegistro { get; set; }

        public string Produto { get; set; }

        public int AmostrasProgramadas { get; set; }

        public int CargaTotal { get; set; }
    }
}


