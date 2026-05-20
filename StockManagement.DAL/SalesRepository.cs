using Microsoft.Data.SqlClient;
using StockManagement.Entities.Dtos;
using StockManagement.Entities.Models;

namespace StockManagement.DAL;

public class SalesRepository
{
    private readonly IDbConnectionFactory _db;

    public SalesRepository(IDbConnectionFactory db) => _db = db;

    public List<SalesListItem> GetAllFromSp()
    {
        const string sql = "EXEC SPGetAllSales";
        return QueryList(sql, r => new SalesListItem
        {
            Id = r.GetInt32(r.GetOrdinal("Id")),
            CustomerId = r.IsDBNull(r.GetOrdinal("CustomerId")) ? null : r.GetInt32(r.GetOrdinal("CustomerId")),
            CustomerName = r.IsDBNull(r.GetOrdinal("CustomerName")) ? null : r.GetString(r.GetOrdinal("CustomerName")),
            ProductId = r.IsDBNull(r.GetOrdinal("ProductId")) ? null : r.GetInt32(r.GetOrdinal("ProductId")),
            ProductName = r.IsDBNull(r.GetOrdinal("ProductName")) ? null : r.GetString(r.GetOrdinal("ProductName")),
            Quantity = r.IsDBNull(r.GetOrdinal("Quantity")) ? null : r.GetDouble(r.GetOrdinal("Quantity")),
            SalesPrice = r.IsDBNull(r.GetOrdinal("SalesPrice")) ? null : r.GetDouble(r.GetOrdinal("SalesPrice")),
            Date = r.IsDBNull(r.GetOrdinal("Date")) ? null : r.GetDateTime(r.GetOrdinal("Date")),
            Amount = r.IsDBNull(r.GetOrdinal("Amount")) ? null : r.GetDouble(r.GetOrdinal("Amount")),
            ListPrice = r.IsDBNull(r.GetOrdinal("ListPrice")) ? null : r.GetDouble(r.GetOrdinal("ListPrice")),
            DiscountRate = r.IsDBNull(r.GetOrdinal("DiscountRate")) ? null : r.GetDouble(r.GetOrdinal("DiscountRate"))
        });
    }

    public List<SalesReportItem> GetSalesDetailReport()
    {
        const string sql = "EXEC SPReportGetAllSalesDetail";
        return QueryList(sql, r => new SalesReportItem
        {
            Id = r.GetInt32(r.GetOrdinal("ID")),
            CategoryId = r.IsDBNull(r.GetOrdinal("CategoryId")) ? null : r.GetInt32(r.GetOrdinal("CategoryId")),
            CategoryName = r.IsDBNull(r.GetOrdinal("CategoryName")) ? null : r.GetString(r.GetOrdinal("CategoryName")),
            ProductId = r.IsDBNull(r.GetOrdinal("ProductId")) ? null : r.GetInt32(r.GetOrdinal("ProductId")),
            ProductName = r.IsDBNull(r.GetOrdinal("ProductName")) ? null : r.GetString(r.GetOrdinal("ProductName")),
            CustomerId = r.IsDBNull(r.GetOrdinal("CustomerId")) ? null : r.GetInt32(r.GetOrdinal("CustomerId")),
            CustomerName = r.IsDBNull(r.GetOrdinal("CustomerName")) ? null : r.GetString(r.GetOrdinal("CustomerName")),
            Quantity = r.IsDBNull(r.GetOrdinal("Quantity")) ? null : r.GetDouble(r.GetOrdinal("Quantity")),
            SalesPrice = r.IsDBNull(r.GetOrdinal("SalesPrice")) ? null : r.GetDouble(r.GetOrdinal("SalesPrice")),
            DiscountRate = r.IsDBNull(r.GetOrdinal("DiscountRate")) ? null : r.GetDouble(r.GetOrdinal("DiscountRate")),
            Date = r.IsDBNull(r.GetOrdinal("DATE")) ? null : r.GetDateTime(r.GetOrdinal("DATE")),
            TotalQuantity = r.IsDBNull(r.GetOrdinal("TotalQuantity")) ? null : r.GetDouble(r.GetOrdinal("TotalQuantity"))
        });
    }

    public void Insert(Sale sale)
    {
        const string sql = @"INSERT INTO Sales (PRODUCT_ID, QUANTITY, SALESPRICE, DATE, AMOUNT, CUSTOMER_ID, LISTPRICE, DISCOUNTRATE)
VALUES (@ProductId, @Quantity, @SalesPrice, @Date, @Amount, @CustomerId, @ListPrice, @DiscountRate)";
        Execute(sql,
            new SqlParameter("@ProductId", (object?)sale.ProductId ?? DBNull.Value),
            new SqlParameter("@Quantity", (object?)sale.Quantity ?? DBNull.Value),
            new SqlParameter("@SalesPrice", (object?)sale.SalesPrice ?? DBNull.Value),
            new SqlParameter("@Date", (object?)sale.Date ?? DateTime.Now),
            new SqlParameter("@Amount", (object?)sale.Amount ?? DBNull.Value),
            new SqlParameter("@CustomerId", (object?)sale.CustomerId ?? DBNull.Value),
            new SqlParameter("@ListPrice", (object?)sale.ListPrice ?? DBNull.Value),
            new SqlParameter("@DiscountRate", (object?)sale.DiscountRate ?? DBNull.Value));
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
