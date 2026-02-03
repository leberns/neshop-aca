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

    public static class AiAzureEmbedding
    {
        public const string DeploymentName = "embedding-model";

        /// <summary>
        /// Column type with embedding model dimensions (text-embedding-3-small has 1536 dimensions)
        /// </summary>
        public const string DbType = "vector(1536)";
    }

    public static class AiAzureChat
    {
        public const string DeploymentName = "chat-model";
    }
}