using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WebApiApp4111.Data
{
    public class DbConnectionHelper
    {
        private readonly IConfiguration _configuration;

        public DbConnectionHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("LibraryConnection"));
        }
    }
}
