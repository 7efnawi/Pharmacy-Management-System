// PharmacyApp.Tests/Domain/CsvExporterTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PharmacyApp.Domain.Entities;
using PharmacyApp.Presentation.Helpers;
using System;
using System.Collections.Generic;

namespace PharmacyApp.Tests.Domain
{
    [TestClass]
    public class CsvExporterTests
    {
        [TestMethod]
        public void GenerateMedicineCsv_EmptyList_ReturnsHeaderOnly()
        {
            var csv = CsvExporter.GenerateMedicineCsv(new List<Medicine>());
            Assert.StartsWith("MedicineId,Name,Category,Manufacturer,Price,StockQuantity,MinStockLevel,Unit,ExpiryDate", csv);
            var lines = csv.Trim().Split(Environment.NewLine);
            Assert.HasCount(1, lines);
        }

        [TestMethod]
        public void GenerateMedicineCsv_WithItems_IncludesDataRows()
        {
            var medicines = new List<Medicine>
            {
                new() { MedicineId = "MED-0001", Name = "Aspirin", Category = "Analgesic", Manufacturer = "Bayer", Price = 9.99m, StockQuantity = 50, MinStockLevel = 10, Unit = "Box", ExpiryDate = new DateTime(2027, 1, 1) }
            };

            var csv = CsvExporter.GenerateMedicineCsv(medicines);
            var lines = csv.Trim().Split(Environment.NewLine);
            Assert.HasCount(2, lines);
            Assert.Contains("Aspirin", lines[1]);
            Assert.Contains("MED-0001", lines[1]);
        }
    }
}
