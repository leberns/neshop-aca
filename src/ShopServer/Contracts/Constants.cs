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

    public static class Ai
    {
        public const string EmbeddingModelName = "text-embedding-3-small";

        public const string ChatModelName = "gpt-4o-mini";

        /// <summary>
        /// Column type with embedding model dimensions (text-embedding-3-small has 1536 dimensions)
        /// </summary>
        public const string DbType = "vector(1536)";
    }
}