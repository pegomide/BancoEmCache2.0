namespace Vale.DatabaseAsCache.ApiService.Models
{
    public class Tag
    {
        /// <summary>
        /// Mnemonic of Ship Load PLC
        /// </summary>
        private static readonly string ShipLoadClpPrefix = "CN02!";

        /// <summary>
        /// Mnemonic of Shipment PLC
        /// </summary>
        private static readonly string ShipmentClpPrefix = "EMB!";

        /// <summary>
        /// Tag for new increment. If returns 1, there is new data to read.
        /// </summary>
        public static readonly string NewIncrement = ShipmentClpPrefix + "PIMS_AM_NEW_INCR";
        
        /// <summary>
        /// Pier where the ship is.
        /// 1 = South
        /// 2 = North
        /// </summary>
        public static readonly string PierCode = ShipmentClpPrefix + "Ordem_amos_berco";

        /// <summary>
        /// Product code tag.
        /// </summary>
        public static readonly string ProductCode = ShipmentClpPrefix + "PIMS_C20_MW29706_PRODUTO";

        /// <summary>
        /// Boarding code tag for south pier.
        /// </summary>
        public static readonly string BoardingCodeSouth = ShipmentClpPrefix + "PIMS_TIG_CNG02_GRL_SHIP_01_D";

        /// <summary>
        /// Boarding code tag for north pier.
        /// </summary>
        public static readonly string BoardingCodeNorth = ShipmentClpPrefix + "PIMS_TIG_CNG01_GRL_SHIP_01_D";
        
        /// <summary>
        /// Weigth estimated for south pier.
        /// </summary>
        public static readonly string EstimatedWeightSouth = ShipmentClpPrefix + "PIMS_CN01_BERCS_TOT_01_D";

        /// <summary>
        /// Weigth estimated for north pier.
        /// </summary>
        public static readonly string EstimatedWeightNorth = ShipmentClpPrefix + "PIMS_CN01_BERCN_TOT_01_D";
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly string PoraoID1 = ShipmentClpPrefix + "PIMS_AM_PORAO_NUM_1_D";
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly string Porao1WeigthFirstScale = ShipmentClpPrefix + "PIMS_AM_PORAO_TOT_1_D";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string Porao1WeigthSecondScale = ShipmentClpPrefix + "PIMS_AM_PORAO_TOT08_1_D";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string PoraoID2 = ShipmentClpPrefix + "PIMS_AM_PORAO_NUM_2_D";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string Porao2WeigthFirstScale = ShipmentClpPrefix + "PIMS_AM_PORAO_TOT_2_D";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string Porao2WeigthSecondScale = ShipmentClpPrefix + "PIMS_AM_PORAO_TOT08_2_D";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string PoraoID3 = ShipmentClpPrefix + "PIMS_AM_PORAO_NUM_3_D";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string Porao3WeigthFirstScale = ShipmentClpPrefix + "PIMS_AM_PORAO_TOT_3_D";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string Porao3WeigthSecondScale = ShipmentClpPrefix + "PIMS_AM_PORAO_TOT08_3_D";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string PartialSampleSouth = ShipmentClpPrefix + "PIMS_AM_SUL_PARC_D";
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly string PartialSampleNorth = ShipmentClpPrefix + "PIMS_AM_NORTE_PARC_D";
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly string SubPartialSampleSouth = ShipmentClpPrefix + "PIMS_AMOS_SUL_SP_VAL_QUAR";
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly string SubPartialSampleNorth = ShipmentClpPrefix + "PIMS_AM_NORTE_SP_VAL_QUAR";
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly string SubSubPartialSampleSouth = ShipmentClpPrefix + "PIMS_AM_SUL_QUA_SPED_VAL";
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly string SubSubPartialSampleNorth = ShipmentClpPrefix + "PIMS_AM_NORTE_QUA_SPED_VAL";
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly string IncrementNumberSouth = ShipmentClpPrefix + "AMOS_SUL_INC_NUM";
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly string IncrementNumberNorth = ShipmentClpPrefix + "AMOS_NORTE_INC_NUM";
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly string WeightAtCutSouth = ShipmentClpPrefix + "PIMS_AMOS_SUL_TOT_01_D";
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly string WeightAtCutNorth = ShipmentClpPrefix + "PIMS_AMOS_NORTE_TOT_01_D";

        /// <summary>
        /// Order name tag.
        /// </summary>
        public static readonly string OrderName = ShipmentClpPrefix + "NUM_PORAO_CAR";
        // public static readonly string OrderName = ShipLoadClpPrefix + "%MW9904";
        // Another CLP possibility.
    }
}
