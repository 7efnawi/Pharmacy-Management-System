using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyApp.Domain.Entities
{
    public class Sale
    {
        public string SaleId { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public List<SaleItem> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.Subtotal);
        public string PaymentMethod { get; set; } = "Cash";
        public string? PrescriptionId { get; set; }
    }
}
