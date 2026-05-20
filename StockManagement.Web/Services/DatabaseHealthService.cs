using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StockManagement.Data;

namespace StockManagement.Web.Services;

public class DatabaseHealthService
{
    public async Task<(bool Ok, string Message)> TestAsync(TestDbContext db, CancellationToken ct = default)
    {
        try
        {
            await db.Database.CanConnectAsync(ct);
            return (true, "Veritabanı bağlantısı başarılı.");
        }
        catch (Exception ex)
        {
            var sql = FindSqlException(ex);
            if (sql?.Number == 18456)
            {
                return (false,
                    "SQL Server Windows girişinizi reddetti (18456). " +
                    "sql/grant-access.sql veya sql/create-sql-login.sql scriptlerini SSMS'te çalıştırın.");
            }

            if (sql?.Number == 4060)
                return (false, "Veritabanı 'test' yok. Önce db.sql scriptini çalıştırın.");

            return (false, ex.InnerException?.Message ?? ex.Message);
        }
    }

    public static SqlException? FindSqlException(Exception? ex)
    {
        for (var current = ex; current != null; current = current.InnerException)
        {
            if (current is SqlException sql)
                return sql;
        }

        return null;
    }
}
