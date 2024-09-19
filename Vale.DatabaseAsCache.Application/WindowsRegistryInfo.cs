using System.IO;

namespace Vale.DatabaseAsCache.Application
{
    /// <summary>
    /// Controla as chaves de registro do Windows utilizadas na aplicação
    /// </summary>
    public static class WindowsRegistryInfo
    {
        public const string BasePath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Vale\DatabaseAsCache\Settings";
    }
}
