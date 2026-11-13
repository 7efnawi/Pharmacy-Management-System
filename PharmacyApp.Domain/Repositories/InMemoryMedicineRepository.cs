using System;
using System.Collections.Generic;
using System.Linq;
using PharmacyApp.Domain.Entities;
using PharmacyApp.Domain.Interfaces;

namespace PharmacyApp.Domain.Repositories
{
    public class InMemoryMedicineRepository : IMedicineRepository
    {
        private readonly List<Medicine> _medicines = new();
        private readonly object _lock = new();

        public InMemoryMedicineRepository(bool seedSampleData = false)
        {
            if (seedSampleData)
            {
                SeedDefaults();
            }
        }

        public void Add(Medicine medicine)
        {
            lock (_lock)
            {
                if (string.IsNullOrWhiteSpace(medicine.MedicineId))
                {
                    medicine.MedicineId = $"MED-{_medicines.Count + 1:D3}";
                }
                _medicines.Add(medicine);
            }
        }

        public void Update(Medicine medicine)
        {
            lock (_lock)
            {
                var existing = _medicines.FirstOrDefault(m => m.MedicineId == medicine.MedicineId);
                if (existing != null)
                {
                    existing.Name = medicine.Name;
                    existing.Category = medicine.Category;
                    existing.Manufacturer = medicine.Manufacturer;
                    existing.Price = medicine.Price;
                    existing.StockQuantity = medicine.StockQuantity;
                    existing.MinStockLevel = medicine.MinStockLevel;
                    existing.ExpiryDate = medicine.ExpiryDate;
                    existing.Unit = medicine.Unit;
                }
            }
        }

        public void Delete(string medicineId)
        {
            lock (_lock)
            {
                _medicines.RemoveAll(m => m.MedicineId == medicineId);
            }
        }

        public Medicine? GetById(string medicineId)
        {
            lock (_lock)
            {
                return _medicines.FirstOrDefault(m => m.MedicineId == medicineId);
            }
        }

        public IEnumerable<Medicine> GetAll()
        {
            lock (_lock)
            {
                return _medicines.ToList();
            }
        }

        public IEnumerable<Medicine> Search(string query)
        {
            lock (_lock)
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return _medicines.ToList();
                }

                var q = query.Trim().ToLowerInvariant();
                return _medicines.Where(m =>
                    m.Name.ToLowerInvariant().Contains(q) ||
                    m.Category.ToLowerInvariant().Contains(q) ||
                    m.MedicineId.ToLowerInvariant().Contains(q)
                ).ToList();
            }
        }

        public IEnumerable<Medicine> GetLowStock()
        {
            lock (_lock)
            {
                return _medicines.Where(m => m.IsLowStock).ToList();
            }
        }

        public IEnumerable<Medicine> GetExpiringSoon(int withinDays = 30)
        {
            lock (_lock)
            {
                var threshold = DateTime.Now.AddDays(withinDays);
                return _medicines.Where(m => m.ExpiryDate <= threshold && m.ExpiryDate > DateTime.Now).ToList();
            }
        }

        private void SeedDefaults()
        {
            _medicines.AddRange(new[]
            {
                new Medicine { MedicineId = "MED-001", Name = "Panadol Extra 500mg", Category = "Analgesic", Manufacturer = "GSK", Price = 18.00m, StockQuantity = 120, MinStockLevel = 25, ExpiryDate = DateTime.Now.AddMonths(18), Unit = "Box" },
                new Medicine { MedicineId = "MED-002", Name = "Amoxicillin 500mg", Category = "Antibiotic", Manufacturer = "MedicalUnion", Price = 32.50m, StockQuantity = 8, MinStockLevel = 20, ExpiryDate = DateTime.Now.AddMonths(10), Unit = "Strip" }, // Low Stock
                new Medicine { MedicineId = "MED-003", Name = "Augmentin 1g", Category = "Antibiotic", Manufacturer = "GSK", Price = 95.00m, StockQuantity = 15, MinStockLevel = 10, ExpiryDate = DateTime.Now.AddDays(20), Unit = "Box" }, // Expiring Soon
                new Medicine { MedicineId = "MED-004", Name = "Cataflam 50mg", Category = "NSAID", Manufacturer = "Novartis", Price = 28.00m, StockQuantity = 60, MinStockLevel = 15, ExpiryDate = DateTime.Now.AddMonths(24), Unit = "Box" },
                new Medicine { MedicineId = "MED-005", Name = "Concor 5mg", Category = "Cardiovascular", Manufacturer = "Merck", Price = 45.00m, StockQuantity = 5, MinStockLevel = 15, ExpiryDate = DateTime.Now.AddMonths(12), Unit = "Box" } // Low Stock
            });
        }
    }
}
