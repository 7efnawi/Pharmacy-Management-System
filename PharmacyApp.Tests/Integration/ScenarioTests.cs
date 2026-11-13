// PharmacyApp.Tests/Integration/ScenarioTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PharmacyApp.Domain.Entities;
using PharmacyApp.Domain.Repositories;
using System;
using System.Linq;

namespace PharmacyApp.Tests.Integration
{
    [TestClass]
    public class ScenarioTests
    {
        [TestMethod]
        public void Scenario1_NewMedicineRegistration_ReflectsInRepositoryAndTotals()
        {
            var repo = new InMemoryMedicineRepository(seedSampleData: false);
            var newMed = new Medicine
            {
                Name = "Ciprofloxacin 500mg",
                Category = "Antibiotic",
                Manufacturer = "Bayer",
                Price = 18.50m,
                StockQuantity = 60,
                MinStockLevel = 20,
                Unit = "Box",
                ExpiryDate = DateTime.Today.AddMonths(18)
            };

            repo.Add(newMed);

            var retrieved = repo.GetById(newMed.MedicineId);
            Assert.IsNotNull(retrieved);
            Assert.AreEqual("Ciprofloxacin 500mg", retrieved.Name);
            Assert.IsFalse(retrieved.IsLowStock);
            Assert.IsFalse(retrieved.IsExpiringSoon);
            Assert.AreEqual(1, repo.GetAll().Count());
        }

        [TestMethod]
        public void Scenario2_SaleCheckout_DecrementsStockAndTriggersAlert()
        {
            var medRepo = new InMemoryMedicineRepository(seedSampleData: false);
            var saleRepo = new InMemorySaleRepository();

            var med = new Medicine
            {
                Name = "Amoxicillin 500mg",
                Category = "Antibiotic",
                Manufacturer = "GlaxoSmithKline",
                Price = 12.00m,
                StockQuantity = 20,
                MinStockLevel = 15,
                Unit = "Box",
                ExpiryDate = DateTime.Today.AddMonths(12)
            };
            medRepo.Add(med);

            // Execute sale of 10 units (drops stock from 20 to 10, which is < MinStockLevel of 15)
            int purchaseQty = 10;
            med.StockQuantity -= purchaseQty;
            medRepo.Update(med);

            var sale = new Sale
            {
                PaymentMethod = "Cash",
                Items = new()
                {
                    new SaleItem { MedicineId = med.MedicineId, MedicineName = med.Name, Quantity = purchaseQty, UnitPrice = med.Price }
                }
            };
            saleRepo.Add(sale);

            // Verify stock decremented
            var updatedMed = medRepo.GetById(med.MedicineId)!;
            Assert.AreEqual(10, updatedMed.StockQuantity);
            Assert.IsTrue(updatedMed.IsLowStock); // Alert triggered!
            Assert.AreEqual(120.00m, sale.TotalAmount);
            Assert.AreEqual(1, saleRepo.GetAll().Count());
        }

        [TestMethod]
        public void Scenario3_PrescriptionArchival_GeneratesIdAndMaintainsItems()
        {
            var rxRepo = new InMemoryPrescriptionRepository();

            var rx = new Prescription
            {
                PatientName = "Sarah Jenkins",
                DoctorName = "Dr. Michael Vance",
                Date = DateTime.Today,
                Notes = "Take one tablet after meals twice daily",
                Items = new()
                {
                    new SaleItem { MedicineId = "MED-0001", MedicineName = "Amoxicillin", Quantity = 2, UnitPrice = 12.50m },
                    new SaleItem { MedicineId = "MED-0002", MedicineName = "Paracetamol", Quantity = 1, UnitPrice = 5.00m }
                }
            };

            rxRepo.Add(rx);

            var retrieved = rxRepo.GetById(rx.PrescriptionId);
            Assert.IsNotNull(retrieved);
            Assert.StartsWith("RX-", retrieved.PrescriptionId);
            Assert.HasCount(2, retrieved.Items);
            Assert.AreEqual("Sarah Jenkins", retrieved.PatientName);
        }

        [TestMethod]
        public void Scenario4_InventoryFiltering_IsolatesDepletedAndExpiringSKUs()
        {
            var repo = new InMemoryMedicineRepository(seedSampleData: false);

            repo.Add(new Medicine { Name = "MedSafe", StockQuantity = 100, MinStockLevel = 20, ExpiryDate = DateTime.Today.AddMonths(12) });
            repo.Add(new Medicine { Name = "MedLow", StockQuantity = 5, MinStockLevel = 20, ExpiryDate = DateTime.Today.AddMonths(12) });
            repo.Add(new Medicine { Name = "MedExpiring", StockQuantity = 80, MinStockLevel = 20, ExpiryDate = DateTime.Today.AddDays(15) });

            var lowStock = repo.GetLowStock().ToList();
            var expiring = repo.GetExpiringSoon(30).ToList();

            Assert.HasCount(1, lowStock);
            Assert.AreEqual("MedLow", lowStock[0].Name);

            Assert.HasCount(1, expiring);
            Assert.AreEqual("MedExpiring", expiring[0].Name);
        }

        [TestMethod]
        public void Scenario5_MedicineDeletion_RemovesRecordSafely()
        {
            var repo = new InMemoryMedicineRepository(seedSampleData: false);
            var med = new Medicine { Name = "DiscontinuedDrug", StockQuantity = 0, MinStockLevel = 5, ExpiryDate = DateTime.Today.AddMonths(6) };
            repo.Add(med);

            Assert.AreEqual(1, repo.GetAll().Count());

            repo.Delete(med.MedicineId);

            Assert.AreEqual(0, repo.GetAll().Count());
            Assert.IsNull(repo.GetById(med.MedicineId));
        }
    }
}
