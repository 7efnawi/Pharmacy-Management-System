using System;

namespace PharmacyApp.Domain.Entities
{
    public class Medicine
    {
        public string MedicineId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int MinStockLevel { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Unit { get; set; } = string.Empty;

        // Computed Business Rules
        public bool IsLowStock => StockQuantity < MinStockLevel;
        public bool IsExpiringSoon => ExpiryDate <= DateTime.Now.AddDays(30) && ExpiryDate > DateTime.Now;
        public bool IsExpired => ExpiryDate <= DateTime.Now;
    }
}
