using System.ComponentModel.DataAnnotations;

namespace StockManagement.Web.Models;

public class PurchaseCreateViewModel
{
    [Required]
    [Display(Name = "Müşteri")]
    public int CustomerId { get; set; }

    [Required]
    [Display(Name = "Ürün")]
    public int ProductId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    [Display(Name = "Miktar")]
    public double Quantity { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    [Display(Name = "Birim Fiyat")]
    public double Price { get; set; }

    [Display(Name = "Tutar")]
    public double Amount { get; set; }

    [Display(Name = "Tarih")]
    public DateTime Date { get; set; } = DateTime.Now;
}
