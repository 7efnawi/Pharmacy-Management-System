using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PharmacyApp.Domain.Entities;

namespace PharmacyApp.Tests.Domain
{
    [TestClass]
    public class MedicineTests
    {
        [TestMethod]
        public void Medicine_WithValidData_ShouldSetPropertiesCorrectly()
        {
            // Arrange & Act
            var medicine = new Medicine
            {
                MedicineId = "MED-001",
                Name = "Paracetamol 500mg",
                Category = "Analgesic",
                Manufacturer = "PharmaCorp",
                Price = 15.50m,
                StockQuantity = 100,
                MinStockLevel = 20,
                ExpiryDate = DateTime.Now.AddMonths(12),
                Unit = "Box"
            };

            // Assert
            Assert.AreEqual("MED-001", medicine.MedicineId);
            Assert.AreEqual("Paracetamol 500mg", medicine.Name);
            Assert.AreEqual("Analgesic", medicine.Category);
            Assert.AreEqual("PharmaCorp", medicine.Manufacturer);
            Assert.AreEqual(15.50m, medicine.Price);
            Assert.AreEqual(100, medicine.StockQuantity);
            Assert.AreEqual(20, medicine.MinStockLevel);
            Assert.AreEqual("Box", medicine.Unit);
        }

        [TestMethod]
        public void Medicine_IsLowStock_ReturnsTrue_WhenStockIsBelowMinimum()
        {
            // Arrange
            var medicine = new Medicine
            {
                StockQuantity = 10,
                MinStockLevel = 20
            };

            // Act & Assert
            Assert.IsTrue(medicine.IsLowStock);
        }

        [TestMethod]
        public void Medicine_IsExpiringSoon_ReturnsTrue_WhenExpiryIsWithin30Days()
        {
            // Arrange
            var medicine = new Medicine
            {
                ExpiryDate = DateTime.Now.AddDays(15)
            };

            // Act & Assert
            Assert.IsTrue(medicine.IsExpiringSoon);
        }

        [TestMethod]
        public void Medicine_IsExpired_ReturnsTrue_WhenExpiryDateIsPassed()
        {
            // Arrange
            var medicine = new Medicine
            {
                ExpiryDate = DateTime.Now.AddDays(-1)
            };

            // Act & Assert
            Assert.IsTrue(medicine.IsExpired);
        }
    }
}
