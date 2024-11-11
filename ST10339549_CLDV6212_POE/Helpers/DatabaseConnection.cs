using System.Data;
using System.Data.SqlClient;

namespace ST10339549_CLDV6212_POE.Helpers
{
    public class DatabaseConnection
    {
        private readonly IConfiguration _configuration;

        public DatabaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("AzureSqlDatabase"));
        }
    }
}
