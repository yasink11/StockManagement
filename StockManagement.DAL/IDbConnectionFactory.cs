using Microsoft.Data.SqlClient;

namespace StockManagement.DAL;

public interface IDbConnectionFactory
{
    SqlConnection CreateConnection();
}
