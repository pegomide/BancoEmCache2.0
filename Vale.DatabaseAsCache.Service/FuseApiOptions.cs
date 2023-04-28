namespace Vale.DatabaseAsCache.Service
{
    public class FuseApiOptions
    {
        public string FuseApiUrl { get; set; }


        /// <summary>
        /// Empyt constructor.
        /// </summary>
        public FuseApiOptions() { }

        /// <summary>
        /// Full constructor.
        /// </summary>
        /// <param name="fuseApiUrl"></param>
        public FuseApiOptions(string fuseApiUrl)
        {
            FuseApiUrl = fuseApiUrl;
        }
    }
}
