using System.ComponentModel.DataAnnotations;

namespace StockManagement.Web.Models;

public class SaleCreateViewModel
{
    [Required(ErrorMessage = "Müşteri seçiniz")]
    [Display(Name = "Müşteri")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Ürün seçiniz")]
    [Display(Name = "Ürün")]
    public int ProductId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Miktar 0'dan büyük olmalı")]
    [Display(Name = "Miktar")]
    public double Quantity { get; set; }

    [Display(Name = "Liste Fiyatı")]
    public double ListPrice { get; set; }

    [Required]
    [Display(Name = "Satış Fiyatı")]
    public double SalesPrice { get; set; }

    [Display(Name = "İskonto Oranı (%)")]
    public double DiscountRate { get; set; }

    [Display(Name = "Tutar")]
    public double Amount { get; set; }

    [Display(Name = "Tarih")]
    public DateTime Date { get; set; } = DateTime.Now;
}
