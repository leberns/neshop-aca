namespace Contracts;

public static class Constants
{
    public static class ConnectionStringNames
    {
        public const string ShopDatabase = "ShopDatabase";
    }

    public static class Identity
    {
        public const string DatabaseTokenScope = "https://ossrdbms-aad.database.windows.net/.default";
    }

    public static class AiModels
    {
        public const string DefaultEmbeddingModel = "text-embedding-3-small";
    }
}