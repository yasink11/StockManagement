using Microsoft.Data.SqlClient;
using StockManagement.Entities.Models;

namespace StockManagement.DAL;

public class StockRepository
{
    private readonly IDbConnectionFactory _db;

    public StockRepository(IDbConnectionFactory db) => _db = db;

    public List<StockEntry> GetAll()
    {
        const string sql = @"
SELECT s.ID, s.PRODUCT_ID, s.QUANTITY, s.DATE, p.NAME
FROM Stock s
LEFT JOIN Product p ON p.ID = s.PRODUCT_ID
ORDER BY s.DATE DESC";
        return Query(sql, Map);
    }

    public void Insert(StockEntry entry)
    {
        const string sql = "INSERT INTO Stock (PRODUCT_ID, QUANTITY, DATE) VALUES (@ProductId, @Quantity, @Date)";
        Execute(sql,
            new SqlParameter("@ProductId", (object?)entry.ProductId ?? DBNull.Value),
            new SqlParameter("@Quantity", (object?)entry.Quantity ?? DBNull.Value),
            new SqlParameter("@Date", (object?)entry.Date ?? DateTime.Now));
    }

    public double GetAvailableQuantity(int productId)
    {
        const string sql = @"
SELECT
  ISNULL((SELECT SUM(QUANTITY) FROM Purchase WHERE PRODUCT_ID = @ProductId), 0)
  + ISNULL((SELECT SUM(QUANTITY) FROM Stock WHERE PRODUCT_ID = @ProductId), 0)
  - ISNULL((SELECT SUM(QUANTITY) FROM Sales WHERE PRODUCT_ID = @ProductId), 0)";
        using var conn = _db.CreateConnection();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@ProductId", productId);
        conn.Open();
        var result = cmd.ExecuteScalar();
        return result == DBNull.Value ? 0 : Convert.ToDouble(result);
    }

    private static StockEntry Map(SqlDataReader r) => new()
    {
        Id = r.GetInt32(0),
        ProductId = r.IsDBNull(1) ? null : r.GetInt32(1),
        Quantity = r.IsDBNull(2) ? null : r.GetDouble(2),
        Date = r.IsDBNull(3) ? null : r.GetDateTime(3),
        ProductName = r.IsDBNull(4) ? null : r.GetString(4)
    };

    private List<T> Query<T>(string sql, Func<SqlDataReader, T> map, params SqlParameter[] parameters)
    {
        var list = new List<T>();
        using var conn = _db.CreateConnection();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddRange(parameters);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(map(reader));
        return list;
    }

    private void Execute(string sql, params SqlParameter[] parameters)
    {
        using var conn = _db.CreateConnection();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddRange(parameters);
        conn.Open();
        cmd.ExecuteNonQuery();
    }
}
