namespace Vale.GetFuseData.ApiService
{
    public class DatabaseFeedback
    {
        public int QuantRowsDeleted { get ; set; }
        public int QuantRowsFailed { get ; set; }
        public int QuantRowsInserted { get; set; }

        public int QuantRowsUpdated { get ; set; }

        public DatabaseFeedback(int quantRowsInserted, int quantUpdated, int quantRowsDeleted, int quantRowsFailed)
        {
            QuantRowsInserted = quantRowsInserted;
            QuantRowsUpdated = quantUpdated;
            QuantRowsDeleted = quantRowsDeleted;
            QuantRowsFailed = quantRowsFailed;
        }
    }
}
