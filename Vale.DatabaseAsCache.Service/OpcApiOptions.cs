namespace Vale.DatabaseAsCache.Service
{
    public class OpcApiOptions
    {
        public string OpcApiUrl { get; set; }

        public string HostName { get; set; }
        public string ServerName { get; set; }

        /// <summary>
        /// Empyt constructor.
        /// </summary>
        public OpcApiOptions() { }

        /// <summary>
        /// Full constructor.
        /// </summary>
        /// <param name="opcApiUrl"></param>
        /// <param name="hostName"></param>
        /// <param name="serverName"></param>
        public OpcApiOptions(string opcApiUrl, string hostName, string serverName)
        {
            OpcApiUrl = opcApiUrl;
            HostName = hostName;
            ServerName = serverName;
        }
    }
}
