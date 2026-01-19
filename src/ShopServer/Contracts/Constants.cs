namespace Contracts;

public static class Constants
{
    public static class Metadata
    {
        public const string AppName = "ShopServer";
        public const string AppVersion = "0.1";
    }

    public static class ConnectionStringNames
    {
        public const string ShopDatabase = "ShopDatabase";
    }

    public static class Identity
    {
        public const string DatabaseTokenScope = "https://ossrdbms-aad.database.windows.net/.default";
    }
}