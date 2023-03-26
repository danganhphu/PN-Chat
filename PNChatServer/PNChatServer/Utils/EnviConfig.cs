namespace PNChatServer.Utils
{
    public class EnviConfig
    {
        public static string DbConnectionString { get; private set; } = null!;

        public static void Config(IConfiguration configuration)
        {
            DbConnectionString = configuration.GetConnectionString("DbConnection")!;
        }
    }
}
