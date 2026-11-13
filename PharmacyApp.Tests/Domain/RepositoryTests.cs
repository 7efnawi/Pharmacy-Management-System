using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PharmacyApp.Domain.Entities;
using PharmacyApp.Domain.Interfaces;
using PharmacyApp.Domain.Repositories;

namespace PharmacyApp.Tests.Domain
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void MedicineRepository_Add_And_GetById_ReturnsStoredMedicine()
        {
            // Arrange
            IMedicineRepository repo = new InMemoryMedicineRepository();
            var medicine = new Medicine
            {
                MedicineId = "MED-001",
                Name = "Amoxicillin 500mg",
                Category = "Antibiotic",
                Price = 25.0m,
                StockQuantity = 50,
                MinStockLevel = 10,
                ExpiryDate = DateTime.Now.AddMonths(6)
            };

            // Act
            repo.Add(medicine);
            var retrieved = repo.GetById("MED-001");

            // Assert
            Assert.IsNotNull(retrieved);
            Assert.AreEqual("Amoxicillin 500mg", retrieved.Name);
        }

        [TestMethod]
        public void MedicineRepository_Search_ByNameOrCategory_ReturnsMatches()
        {
            // Arrange
            IMedicineRepository repo = new InMemoryMedicineRepository();
            repo.Add(new Medicine { MedicineId = "M1", Name = "Aspirin 100mg", Category = "Painkiller" });
            repo.Add(new Medicine { MedicineId = "M2", Name = "Panadol Extra", Category = "Painkiller" });
            repo.Add(new Medicine { MedicineId = "M3", Name = "Cataflam 50mg", Category = "Anti-inflammatory" });

            // Act
            var painkillerMatches = repo.Search("Painkiller").ToList();
            var nameMatches = repo.Search("Aspirin").ToList();

            // Assert
            Assert.HasCount(2, painkillerMatches);
            Assert.HasCount(1, nameMatches);
            Assert.AreEqual("Aspirin 100mg", nameMatches[0].Name);
        }

        [TestMethod]
        public void MedicineRepository_GetLowStock_ReturnsOnlyItemsBelowThreshold()
        {
            // Arrange
            IMedicineRepository repo = new InMemoryMedicineRepository();
            repo.Add(new Medicine { MedicineId = "M1", Name = "Amox", StockQuantity = 5, MinStockLevel = 10 }); // Low
            repo.Add(new Medicine { MedicineId = "M2", Name = "Panadol", StockQuantity = 50, MinStockLevel = 10 }); // Normal

            // Act
            var lowStock = repo.GetLowStock().ToList();

            // Assert
            Assert.HasCount(1, lowStock);
            Assert.AreEqual("M1", lowStock[0].MedicineId);
        }

        [TestMethod]
        public void SaleRepository_Add_And_GetAll_PersistsTransactions()
        {
            // Arrange
            ISaleRepository saleRepo = new InMemorySaleRepository();
            var sale = new Sale
            {
                SaleId = "SAL-001",
                PaymentMethod = "Cash",
                Items = { new SaleItem { MedicineId = "M1", Quantity = 2, UnitPrice = 10.0m } }
            };

            // Act
            saleRepo.Add(sale);
            var allSales = saleRepo.GetAll().ToList();

            // Assert
            Assert.HasCount(1, allSales);
            Assert.AreEqual("SAL-001", allSales[0].SaleId);
            Assert.AreEqual(20.0m, allSales[0].TotalAmount);
        }
    }
}
