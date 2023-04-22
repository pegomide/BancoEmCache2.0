using System;

namespace Vale.GetFuseData.Data.TableModels
{
    public class ColetaFuseData
    {
        public string PRODUCT_CODE { get; set; }

        public string CLIENT_CODE { get; set; }

        public string PIER_CODE { get; set; }

        public string BOARDING_CODE { get; set; }

        public string STACKER_NAME { get; set; }

        public decimal? ESTIMATED_WEIGHT { get; set; }

        public int? PORAO1_ID { get; set; }

        public decimal? PORAO1_PESO { get; set; }

        public int? PORAO2_ID { get; set; }

        public decimal? PORAO2_PESO { get; set; }

        public int? PORAO3_ID { get; set; }

        public decimal? PORAO3_PESO { get; set; }

        public int? ORDER_NUMBER { get; set; }

        public decimal? BOARDING_LINE { get; set; }

        public DateTime? INCREMENT_DATETIME { get; set; }

        public string PARTIAL_SAMPLE { get; set; }

        public string SUBPARTIAL_SAMPLE { get; set; }

        public string SUBSUBPARTIAL_SAMPLE { get; set; }

        public int? INCREMENT_NUMBER { get; set; }

        public decimal? WEIGHTATCUT { get; set; }

        public string STATUS_TYPE { get; set; }

    }
}
