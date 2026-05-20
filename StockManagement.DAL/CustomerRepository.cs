using Microsoft.Data.SqlClient;
using StockManagement.Entities.Models;

namespace StockManagement.DAL;

public class CustomerRepository
{
    private readonly IDbConnectionFactory _db;

    public CustomerRepository(IDbConnectionFactory db) => _db = db;

    public List<Customer> GetAll()
    {
        const string sql = "SELECT ID, CUSTOMERTITLE, CUSTOMERNUMBER FROM Customer ORDER BY CUSTOMERTITLE";
        return Query(sql, Map);
    }

    public Customer? GetById(int id)
    {
        const string sql = "SELECT ID, CUSTOMERTITLE, CUSTOMERNUMBER FROM Customer WHERE ID = @Id";
        return Query(sql, Map, new SqlParameter("@Id", id)).FirstOrDefault();
    }

    public void Insert(Customer customer)
    {
        const string sql = "INSERT INTO Customer (CUSTOMERTITLE, CUSTOMERNUMBER) VALUES (@Title, @Number)";
        Execute(sql,
            new SqlParameter("@Title", (object?)customer.CustomerTitle ?? DBNull.Value),
            new SqlParameter("@Number", (object?)customer.CustomerNumber ?? DBNull.Value));
    }

    public void Update(Customer customer)
    {
        const string sql = @"UPDATE Customer SET CUSTOMERTITLE = @Title, CUSTOMERNUMBER = @Number WHERE ID = @Id";
        Execute(sql,
            new SqlParameter("@Title", (object?)customer.CustomerTitle ?? DBNull.Value),
            new SqlParameter("@Number", (object?)customer.CustomerNumber ?? DBNull.Value),
            new SqlParameter("@Id", customer.Id));
    }

    public void Delete(int id)
    {
        Execute("DELETE FROM Customer WHERE ID = @Id", new SqlParameter("@Id", id));
    }

    private static Customer Map(SqlDataReader r) => new()
    {
        Id = r.GetInt32(0),
        CustomerTitle = r.IsDBNull(1) ? null : r.GetString(1),
        CustomerNumber = r.IsDBNull(2) ? null : r.GetString(2)
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
