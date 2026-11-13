using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PharmacyApp.Domain.Entities;

namespace PharmacyApp.Tests.Domain
{
    [TestClass]
    public class SaleTests
    {
        [TestMethod]
        public void SaleItem_Subtotal_CalculatesQuantityTimesUnitPrice()
        {
            // Arrange
            var item = new SaleItem
            {
                MedicineId = "MED-001",
                MedicineName = "Amoxicillin 500mg",
                Quantity = 3,
                UnitPrice = 12.50m
            };

            // Act & Assert
            Assert.AreEqual(37.50m, item.Subtotal);
        }

        [TestMethod]
        public void Sale_TotalAmount_CalculatesSumOfAllItemSubtotals()
        {
            // Arrange
            var sale = new Sale
            {
                SaleId = "SAL-001",
                SaleDate = DateTime.Now,
                PaymentMethod = "Cash",
                Items = new List<SaleItem>
                {
                    new SaleItem { MedicineId = "MED-001", Quantity = 2, UnitPrice = 10.00m },
                    new SaleItem { MedicineId = "MED-002", Quantity = 1, UnitPrice = 15.50m }
                }
            };

            // Act & Assert
            Assert.AreEqual(35.50m, sale.TotalAmount);
        }

        [TestMethod]
        public void Prescription_WithItems_InitializesCorrectly()
        {
            // Arrange
            var prescription = new Prescription
            {
                PrescriptionId = "RX-001",
                PatientName = "Mohamed Ali",
                DoctorName = "Dr. Hassan",
                Date = DateTime.Today,
                Items = new List<SaleItem>
                {
                    new SaleItem { MedicineId = "MED-001", MedicineName = "Paracetamol", Quantity = 1, UnitPrice = 5.0m }
                },
                Notes = "Take one tablet after meals"
            };

            // Act & Assert
            Assert.AreEqual("RX-001", prescription.PrescriptionId);
            Assert.AreEqual("Mohamed Ali", prescription.PatientName);
            Assert.HasCount(1, prescription.Items);
            Assert.AreEqual(5.0m, prescription.Items[0].Subtotal);
        }
    }
}
