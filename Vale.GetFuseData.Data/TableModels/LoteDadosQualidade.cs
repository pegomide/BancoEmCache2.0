using System;

namespace Vale.GetFuseData.Data.TableModels
{
    public class LoteDadosQualidade
    {
        public long LoteDadosQualidadeId { get; set; }

        public string GpvLoteId { get; set; }

        public string MatriculaPrimeiroVagao { get; set; }

        public string PrefixoTrem { get; set; }

        public DateTime PrevisaoChegada { get; set; }

        public DateTime DataHoraRegistro { get; set; }

        public string Produto { get; set; }

        public int VagoesProgramados { get; set; }

        public double TotalProgramado { get; set; }

        public string MatriculaUltimoVagao { get; set; }
    }
}
