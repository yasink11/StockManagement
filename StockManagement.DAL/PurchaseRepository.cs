using Microsoft.Data.SqlClient;
using StockManagement.Entities.Dtos;
using StockManagement.Entities.Models;

namespace StockManagement.DAL;

public class PurchaseRepository
{
    private readonly IDbConnectionFactory _db;

    public PurchaseRepository(IDbConnectionFactory db) => _db = db;

    public List<PurchaseListItem> GetAllFromSp()
    {
        const string sql = "EXEC SPGetAllPurchases";
        return QueryList(sql, r => new PurchaseListItem
        {
            Id = r.GetInt32(r.GetOrdinal("Id")),
            CustomerId = r.IsDBNull(r.GetOrdinal("CustomerId")) ? null : r.GetInt32(r.GetOrdinal("CustomerId")),
            CustomerName = r.IsDBNull(r.GetOrdinal("CustomerName")) ? null : r.GetString(r.GetOrdinal("CustomerName")),
            ProductId = r.IsDBNull(r.GetOrdinal("ProductId")) ? null : r.GetInt32(r.GetOrdinal("ProductId")),
            ProductName = r.IsDBNull(r.GetOrdinal("ProductName")) ? null : r.GetString(r.GetOrdinal("ProductName")),
            Quantity = r.IsDBNull(r.GetOrdinal("Quantity")) ? null : r.GetDouble(r.GetOrdinal("Quantity")),
            Price = r.IsDBNull(r.GetOrdinal("Price")) ? null : r.GetDouble(r.GetOrdinal("Price")),
            Date = r.IsDBNull(r.GetOrdinal("Date")) ? null : r.GetDateTime(r.GetOrdinal("Date")),
            Amount = r.IsDBNull(r.GetOrdinal("Amount")) ? null : r.GetDouble(r.GetOrdinal("Amount"))
        });
    }

    public void Insert(Purchase purchase)
    {
        const string sql = @"INSERT INTO Purchase (PRODUCT_ID, QUANTITY, PRICE, AMOUNT, DATE, CUSTOMER_ID)
VALUES (@ProductId, @Quantity, @Price, @Amount, @Date, @CustomerId)";
        Execute(sql,
            new SqlParameter("@ProductId", (object?)purchase.ProductId ?? DBNull.Value),
            new SqlParameter("@Quantity", (object?)purchase.Quantity ?? DBNull.Value),
            new SqlParameter("@Price", (object?)purchase.Price ?? DBNull.Value),
            new SqlParameter("@Amount", (object?)purchase.Amount ?? DBNull.Value),
            new SqlParameter("@Date", (object?)purchase.Date ?? DateTime.Now),
            new SqlParameter("@CustomerId", (object?)purchase.CustomerId ?? DBNull.Value));
    }

    private List<T> QueryList<T>(string sql, Func<SqlDataReader, T> map)
    {
        var list = new List<T>();
        using var conn = _db.CreateConnection();
        using var cmd = new SqlCommand(sql, conn);
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
