using Microsoft.Data.SqlClient;
using StockManagement.Entities.Models;

namespace StockManagement.DAL;

public class ProductRepository
{
    private readonly IDbConnectionFactory _db;

    public ProductRepository(IDbConnectionFactory db) => _db = db;

    public List<Product> GetAll()
    {
        const string sql = @"
SELECT p.ID, p.CATEGORY_ID, p.NAME, p.IMAGE_SRC, p.SALESPRICE, c.NAME
FROM Product p
LEFT JOIN Category c ON c.ID = p.CATEGORY_ID
ORDER BY p.NAME";
        return Query(sql, Map);
    }

    public Product? GetById(int id)
    {
        const string sql = @"
SELECT p.ID, p.CATEGORY_ID, p.NAME, p.IMAGE_SRC, p.SALESPRICE, c.NAME
FROM Product p
LEFT JOIN Category c ON c.ID = p.CATEGORY_ID
WHERE p.ID = @Id";
        return Query(sql, Map, new SqlParameter("@Id", id)).FirstOrDefault();
    }

    public void Insert(Product product)
    {
        const string sql = @"INSERT INTO Product (CATEGORY_ID, NAME, IMAGE_SRC, SALESPRICE)
VALUES (@CategoryId, @Name, @ImageSrc, @SalesPrice)";
        Execute(sql,
            new SqlParameter("@CategoryId", (object?)product.CategoryId ?? DBNull.Value),
            new SqlParameter("@Name", (object?)product.Name ?? DBNull.Value),
            new SqlParameter("@ImageSrc", (object?)product.ImageSrc ?? DBNull.Value),
            new SqlParameter("@SalesPrice", (object?)product.SalesPrice ?? DBNull.Value));
    }

    public void Update(Product product)
    {
        const string sql = @"UPDATE Product SET CATEGORY_ID = @CategoryId, NAME = @Name,
IMAGE_SRC = @ImageSrc, SALESPRICE = @SalesPrice WHERE ID = @Id";
        Execute(sql,
            new SqlParameter("@CategoryId", (object?)product.CategoryId ?? DBNull.Value),
            new SqlParameter("@Name", (object?)product.Name ?? DBNull.Value),
            new SqlParameter("@ImageSrc", (object?)product.ImageSrc ?? DBNull.Value),
            new SqlParameter("@SalesPrice", (object?)product.SalesPrice ?? DBNull.Value),
            new SqlParameter("@Id", product.Id));
    }

    public void Delete(int id)
    {
        Execute("DELETE FROM Product WHERE ID = @Id", new SqlParameter("@Id", id));
    }

    private static Product Map(SqlDataReader r) => new()
    {
        Id = r.GetInt32(0),
        CategoryId = r.IsDBNull(1) ? null : r.GetInt32(1),
        Name = r.IsDBNull(2) ? null : r.GetString(2),
        ImageSrc = r.IsDBNull(3) ? null : r.GetString(3),
        SalesPrice = r.IsDBNull(4) ? null : r.GetDouble(4),
        CategoryName = r.IsDBNull(5) ? null : r.GetString(5)
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
