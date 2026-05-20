using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Data;
using StockManagement.Web.Models;

namespace StockManagement.Web.Controllers;

public class ReportController : Controller
{
    private readonly ReportService _reportService;
    private readonly StockService _stockService;

    public ReportController(ReportService reportService, StockService stockService)
    {
        _reportService = reportService;
        _stockService = stockService;
    }

    public IActionResult SalesDetail() => View(new ReportFilterViewModel());

    [HttpPost]
    public IActionResult SalesDetail(ReportFilterViewModel filter) => View(filter);

    [HttpGet]
    public async Task<object> GetSalesDetail(
        DataSourceLoadOptions loadOptions,
        DateTime? startDate,
        DateTime? endDate,
        CancellationToken ct)
    {
        var data = await _reportService.GetSalesDetailReportAsync(startDate, endDate, ct);

        return DataSourceLoader.Load(data.AsQueryable(), loadOptions);
    }

    public IActionResult StockReport() => View();

    [HttpGet]
    public async Task<object> GetStockReport(DataSourceLoadOptions loadOptions, CancellationToken ct)
    {
        var data = await _stockService.GetStockReportAsync(ct);
        return DataSourceLoader.Load(data, loadOptions);
    }
}
