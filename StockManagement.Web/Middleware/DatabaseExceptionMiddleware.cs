using Microsoft.AspNetCore.Diagnostics;
using StockManagement.Web.Services;

namespace StockManagement.Web.Middleware;

public class DatabaseExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DatabaseExceptionMiddleware> _logger;

    public DatabaseExceptionMiddleware(RequestDelegate next, ILogger<DatabaseExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex) when (!context.Response.HasStarted)
        {
            var sql = DatabaseHealthService.FindSqlException(ex);
            if (sql is null)
                throw;

            _logger.LogError(ex, "SQL hatası {Number}", sql.Number);
            var message = GetSqlMessage(sql);
            context.Response.Redirect(BuildErrorUrl(message));
        }
    }

    private static string GetSqlMessage(Microsoft.Data.SqlClient.SqlException ex) =>
        ex.Number switch
        {
            18456 => "SQL Server girişi reddedildi. Çözüm: SSMS'te sql/grant-access.sql (Windows) veya sql/create-sql-login.sql (SQL kullanıcı) çalıştırın, sonra appsettings.Development.json bağlantı dizesini güncelleyin.",
            4060 => "Veritabanı 'test' bulunamadı. Önce db.sql scriptini çalıştırın.",
            _ => "Veritabanı hatası: " + ex.Message
        };

    private static string BuildErrorUrl(string message) =>
        $"/Home/DbError?message={Uri.EscapeDataString(message)}";
}
