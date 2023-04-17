using System;

namespace Vale.GetFuseData.Data.TableModels
{
    public  class LoteEmOperacao
    {
        public long LoteEmOperacaoId { get; set; }

        public string GpvLoteId { get; set; }

        public string MatriculaPrimeiroVagao { get; set; }

        public string PrefixoTrem { get; set; }

        public string Virador { get; set; }

        public DateTime DataHoraRegistro { get; set; }

        public string Produto { get; set; }

        public string MatriculaUltimoVagao { get; set; }
    }
}
