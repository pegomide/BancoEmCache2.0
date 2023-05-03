using Newtonsoft.Json;
using System;

namespace Vale.DatabaseAsCache.Data.TableModels
{
    public class ColetaFuseData
    {
        public string PIER_CODE { get; set; }

        public string BOARDING_CODE { get; set; }

        public int INCREMENT_NUMBER { get; set; }

        public string PRODUCT_CODE { get; set; }

        public string CLIENT_CODE = null;

        public string STACKER_NAME = "CN02";

        public decimal? ESTIMATED_WEIGHT { get; set; }

        public int? PORAO1_ID { get; set; }

        public decimal? PORAO1_PESO1 { get; set; }
        public decimal? PORAO1_PESO2 { get; set; }

        public int? PORAO2_ID { get; set; }

        public decimal? PORAO2_PESO1 { get; set; }
        public decimal? PORAO2_PESO2 { get; set; }

        public int? PORAO3_ID { get; set; }

        public decimal? PORAO3_PESO1 { get; set; }
        public decimal? PORAO3_PESO2 { get; set; }

        public int? ORDER_NUMBER { get; set; }

        public decimal? BOARDING_LINE = 1;

        public DateTime? INCREMENT_DATETIME { get; set; }

        public string PARTIAL_SAMPLE { get; set; }

        public string SUBPARTIAL_SAMPLE { get; set; }

        public string SUBSUBPARTIAL_SAMPLE { get; set; }

        public decimal? WEIGHTATCUT { get; set; }

        public string STATUS_TYPE { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
