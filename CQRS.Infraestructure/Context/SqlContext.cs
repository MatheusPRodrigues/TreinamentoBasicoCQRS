using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CQRS.Infraestructure.Context
{
    public class SqlContext : IAbstractFactory<SqlConnection>
    {
        private readonly string _connectionString;

        public SqlContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        SqlConnection IAbstractFactory<SqlConnection>.CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
