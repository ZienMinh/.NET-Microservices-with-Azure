using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace eCommerce.Infrastructure.DbContext;

public class DapperDbContext
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _connection;

    public DapperDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        string connectionString = _configuration.GetConnectionString("local");

        //Create a new MySql with the retrieved connection string
        _connection = new MySqlConnection(connectionString);
    }

    public IDbConnection DbConnection => _connection;
}
