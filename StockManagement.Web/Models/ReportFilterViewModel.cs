using System.ComponentModel.DataAnnotations;

namespace StockManagement.Web.Models;

public class ReportFilterViewModel
{
    [Display(Name = "Başlangıç Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; }

    [Display(Name = "Bitiş Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }
}
