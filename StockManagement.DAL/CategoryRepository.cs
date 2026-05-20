using Microsoft.Data.SqlClient;
using StockManagement.Entities.Models;

namespace StockManagement.DAL;

public class CategoryRepository
{
    private readonly IDbConnectionFactory _db;

    public CategoryRepository(IDbConnectionFactory db) => _db = db;

    public List<Category> GetAll()
    {
        const string sql = "SELECT ID, NAME FROM Category ORDER BY NAME";
        return Query(sql, Map);
    }

    public Category? GetById(int id)
    {
        const string sql = "SELECT ID, NAME FROM Category WHERE ID = @Id";
        return Query(sql, Map, new SqlParameter("@Id", id)).FirstOrDefault();
    }

    public void Insert(Category category)
    {
        const string sql = "INSERT INTO Category (NAME) VALUES (@Name)";
        Execute(sql, new SqlParameter("@Name", (object?)category.Name ?? DBNull.Value));
    }

    public void Update(Category category)
    {
        const string sql = "UPDATE Category SET NAME = @Name WHERE ID = @Id";
        Execute(sql,
            new SqlParameter("@Name", (object?)category.Name ?? DBNull.Value),
            new SqlParameter("@Id", category.Id));
    }

    public void Delete(int id)
    {
        const string sql = "DELETE FROM Category WHERE ID = @Id";
        Execute(sql, new SqlParameter("@Id", id));
    }

    private static Category Map(SqlDataReader r) => new()
    {
        Id = r.GetInt32(0),
        Name = r.IsDBNull(1) ? null : r.GetString(1)
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
