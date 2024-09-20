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
        /// Tag for new IncrementData. If returns 1, there is new data to read.
        /// </summary>
        public static readonly string NewIncrement = ShipmentClpPrefix + "BD_AM_NEW_INCR_TX";
        
        /// <summary>
        /// Pier where the ship is.
        /// 1 = South
        /// 2 = North
        /// </summary>
        public static readonly string PierCode = ShipmentClpPrefix + "BD_ORDEM_AMOS_BERCO_TX";

        /// <summary>
        /// Product code tag.
        /// </summary>
        public static readonly string ProductCode = ShipmentClpPrefix + "BD_PRODUCT_CODE_TX";

        /// <summary>
        /// Boarding code tag for north pier.
        /// </summary>
        public static readonly string BoardingCode = ShipmentClpPrefix + "BD_BORDING_CODE_TX";

        /// <summary>
        /// Weigth estimated for north pier.
        /// </summary>
        public static readonly string EstimatedWeight = ShipmentClpPrefix + "BD_ESTIMATED_WEIGHT_TX";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string PoraoID1 = ShipmentClpPrefix + "BD_PORAO_NUM_1_TX";
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly string Porao1WeigthFirstScale = ShipmentClpPrefix + "BD_PORAO_TOT_1_PRI_TX";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string Porao1WeigthSecondScale = ShipmentClpPrefix + "BD_PORAO_TOT_1_SEC_TX";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string PoraoID2 = ShipmentClpPrefix + "BD_PORAO_NUM_2_TX";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string Porao2WeigthFirstScale = ShipmentClpPrefix + "BD_PORAO_TOT_2_PRI_TX";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string Porao2WeigthSecondScale = ShipmentClpPrefix + "BD_PORAO_TOT_2_SEC_TX";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string PoraoID3 = ShipmentClpPrefix + "BD_PORAO_NUM_3_TX";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string Porao3WeigthFirstScale = ShipmentClpPrefix + "BD_PORAO_TOT_3_PRI_TX";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string Porao3WeigthSecondScale = ShipmentClpPrefix + "BD_PORAO_TOT_3_SEC_TX";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string PartialSample = ShipmentClpPrefix + "BD_PARCIAL_TX";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string SubPartialSample = ShipmentClpPrefix + "BD_SUBPARCIAL_TX";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string SubSubPartialSample = ShipmentClpPrefix + "DB_EXPEDITA_TX";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string IncrementNumber = ShipmentClpPrefix + "DB_NUMERO_INCREMENTO_TX";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string WeightAtCut = ShipmentClpPrefix + "DB_PESO_CORTE_PROG_TX";

        /// <summary>
        /// Order name tag.
        /// </summary>
        public static readonly string OrderName = ShipmentClpPrefix + "BD_PORAO_NUM_1_TX";

        /// <summary>
        /// Watchdog signal tag.
        /// </summary>
        public static readonly string WatchdogSignal = ShipmentClpPrefix + "DB_WACTH_DOG_RX";

        /// <summary>
        /// Confirmation signal tag.
        /// </summary>
        public static readonly string ConfirmationSignal = ShipmentClpPrefix + "DB_INC_RECV_OK_RX";

        /// <summary>
        /// Tag que indica que o GPV está com delay para consumir mensagens.
        /// </summary>
        public static readonly string GPVWithDelay = ShipmentClpPrefix + "DB_ENV_GPV_FAULT_RX";
        
        // public static readonly string OrderName = ShipLoadClpPrefix + "%MW9904";
        // Another CLP possibility.
    }
}
