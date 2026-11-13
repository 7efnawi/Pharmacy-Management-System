// PharmacyApp.Presentation/Helpers/CsvExporter.cs
using System;
using System.Collections.Generic;
using System.Text;
using PharmacyApp.Domain.Entities;

namespace PharmacyApp.Presentation.Helpers
{
    public static class CsvExporter
    {
        public static string GenerateMedicineCsv(IEnumerable<Medicine> medicines)
        {
            var sb = new StringBuilder();
            sb.AppendLine("MedicineId,Name,Category,Manufacturer,Price,StockQuantity,MinStockLevel,Unit,ExpiryDate");

            foreach (var m in medicines)
            {
                // Escape quotes and commas if present in strings
                string safeName = m.Name.Contains(',') || m.Name.Contains('"') 
                    ? $"\"{m.Name.Replace("\"", "\"\"")}\"" 
                    : m.Name;

                string safeManufacturer = m.Manufacturer.Contains(',') || m.Manufacturer.Contains('"')
                    ? $"\"{m.Manufacturer.Replace("\"", "\"\"")}\""
                    : m.Manufacturer;

                sb.AppendLine($"{m.MedicineId},{safeName},{m.Category},{safeManufacturer},{m.Price:F2},{m.StockQuantity},{m.MinStockLevel},{m.Unit},{m.ExpiryDate:yyyy-MM-dd}");
            }

            return sb.ToString();
        }
    }
}
