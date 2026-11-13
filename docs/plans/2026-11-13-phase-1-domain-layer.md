# 📐 Phase 1 — Domain Layer: TDD Implementation Plan
## Pharmacy Management System | C# WinForms
**Date:** 2026-11-13
**Phase:** 1 — Domain Entities + Repository Interfaces

---

## 🎯 Objective

Build the **pure Domain layer** — zero dependencies on UI, database, or external libraries.
Only C# entities and interface contracts.

---

## 📁 Files to Create

```
PharmacyApp/
├── PharmacyApp.Domain/
│   ├── Entities/
│   │   ├── Medicine.cs
│   │   ├── Sale.cs
│   │   ├── SaleItem.cs
│   │   └── Prescription.cs
│   └── Interfaces/
│       ├── IMedicineRepository.cs
│       ├── ISaleRepository.cs
│       └── IPrescriptionRepository.cs
└── PharmacyApp.Tests/
    └── Domain/
        ├── MedicineTests.cs
        ├── SaleTests.cs
        └── PrescriptionTests.cs
```

---

## 🔴🟢🔵 TDD Sub-Tasks

---

### Sub-Task 1.1 — `Medicine` Entity

**Red Phase — Write test FIRST:**
```csharp
// MedicineTests.cs
[TestClass]
public class MedicineTests
{
    [TestMethod]
    public void Medicine_WithValidData_ShouldHaveCorrectProperties()
    {
        var med = new Medicine
        {
            MedicineId = "MED-001",
            Name = "Paracetamol",
            Category = "Painkiller",
            Manufacturer = "PharmaCo",
            Price = 5.50m,
            StockQuantity = 100,
            MinStockLevel = 20,
            ExpiryDate = DateTime.Now.AddMonths(12),
            Unit = "Tablet"
        };

        Assert.AreEqual("MED-001", med.MedicineId);
        Assert.AreEqual("Paracetamol", med.Name);
        Assert.AreEqual(5.50m, med.Price);
        Assert.AreEqual(100, med.StockQuantity);
        Assert.IsTrue(med.ExpiryDate > DateTime.Now);
    }

    [TestMethod]
    public void Medicine_IsLowStock_WhenQuantityBelowMinimum()
    {
        var med = new Medicine
        {
            StockQuantity = 10,
            MinStockLevel = 20
        };

        Assert.IsTrue(med.IsLowStock);
    }

    [TestMethod]
    public void Medicine_IsExpiringSoon_WhenExpiryWithin30Days()
    {
        var med = new Medicine
        {
            ExpiryDate = DateTime.Now.AddDays(15)
        };

        Assert.IsTrue(med.IsExpiringSoon);
    }

    [TestMethod]
    public void Medicine_IsExpired_WhenExpiryDatePassed()
    {
        var med = new Medicine
        {
            ExpiryDate = DateTime.Now.AddDays(-1)
        };

        Assert.IsTrue(med.IsExpired);
    }
}
```

**Green Phase — Implement `Medicine.cs`:**
```csharp
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

        // Computed properties (business rules)
        public bool IsLowStock => StockQuantity < MinStockLevel;
        public bool IsExpiringSoon => ExpiryDate <= DateTime.Now.AddDays(30) && ExpiryDate > DateTime.Now;
        public bool IsExpired => ExpiryDate <= DateTime.Now;
    }
}
```

**Verification:**
```bash
dotnet test → 4/4 passed ✅
dotnet build → 0 warnings ✅
```

**Commit:**
```
feat: add Medicine domain entity with business rule properties
test: add MedicineTests covering stock, expiry, and properties
```

---

### Sub-Task 1.2 — `SaleItem` + `Sale` Entities

**Red Phase:**
```csharp
[TestClass]
public class SaleTests
{
    [TestMethod]
    public void SaleItem_Subtotal_IsQuantityTimesPrice()
    {
        var item = new SaleItem
        {
            MedicineId = "MED-001",
            MedicineName = "Paracetamol",
            Quantity = 3,
            UnitPrice = 5.50m
        };

        Assert.AreEqual(16.50m, item.Subtotal);
    }

    [TestMethod]
    public void Sale_TotalAmount_IsSumOfAllItemSubtotals()
    {
        var sale = new Sale
        {
            SaleId = "SAL-001",
            SaleDate = DateTime.Now,
            PaymentMethod = "Cash",
            Items = new List<SaleItem>
            {
                new SaleItem { Quantity = 2, UnitPrice = 10.00m },
                new SaleItem { Quantity = 1, UnitPrice = 5.50m }
            }
        };

        Assert.AreEqual(25.50m, sale.TotalAmount);
    }
}
```

**Green Phase — Implement:**
```csharp
public class SaleItem
{
    public string MedicineId { get; set; } = string.Empty;
    public string MedicineName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal => Quantity * UnitPrice;  // ← computed
}

public class Sale
{
    public string SaleId { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public List<SaleItem> Items { get; set; } = new();
    public decimal TotalAmount => Items.Sum(i => i.Subtotal);  // ← computed
    public string PaymentMethod { get; set; } = string.Empty;
    public string? PrescriptionId { get; set; }
}
```

**Commit:**
```
feat: add SaleItem and Sale domain entities with computed totals
test: add SaleTests covering subtotal and total calculations
```

---

### Sub-Task 1.3 — `Prescription` Entity

**Red Phase:**
```csharp
[TestMethod]
public void Prescription_WithItems_ShouldHaveCorrectCount()
{
    var rx = new Prescription
    {
        PrescriptionId = "RX-001",
        PatientName = "Ahmed Ali",
        DoctorName = "Dr. Mohamed",
        Date = DateTime.Today,
        Items = new List<SaleItem>
        {
            new SaleItem { MedicineName = "Amoxicillin", Quantity = 1 }
        }
    };

    Assert.AreEqual(1, rx.Items.Count);
    Assert.AreEqual("Ahmed Ali", rx.PatientName);
}
```

**Green Phase:**
```csharp
public class Prescription
{
    public string PrescriptionId { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public List<SaleItem> Items { get; set; } = new();
    public string Notes { get; set; } = string.Empty;
}
```

**Commit:**
```
feat: add Prescription domain entity
test: add PrescriptionTests
```

---

### Sub-Task 1.4 — Repository Interfaces

> No tests needed for interfaces (they're contracts, not implementations).

```csharp
// IMedicineRepository.cs
public interface IMedicineRepository
{
    void Add(Medicine medicine);
    void Update(Medicine medicine);
    void Delete(string medicineId);
    Medicine? GetById(string medicineId);
    IEnumerable<Medicine> GetAll();
    IEnumerable<Medicine> Search(string query);
    IEnumerable<Medicine> GetLowStock();
    IEnumerable<Medicine> GetExpiringSoon(int withinDays = 30);
}

// ISaleRepository.cs
public interface ISaleRepository
{
    void Add(Sale sale);
    Sale? GetById(string saleId);
    IEnumerable<Sale> GetAll();
    IEnumerable<Sale> GetByDate(DateTime date);
}

// IPrescriptionRepository.cs
public interface IPrescriptionRepository
{
    void Add(Prescription prescription);
    Prescription? GetById(string prescriptionId);
    IEnumerable<Prescription> GetByPatient(string patientName);
    IEnumerable<Prescription> GetAll();
}
```

**Commit:**
```
feat: define IMedicineRepository, ISaleRepository, IPrescriptionRepository contracts
```

---

## ✅ Phase 1 Completion Gate

Before moving to Phase 2, verify:

```bash
dotnet test PharmacyApp.Tests
# Expected: All tests pass (100%)

dotnet build PharmacyApp.sln
# Expected: 0 errors, 0 warnings
```

| Check | Expected | Status |
|-------|----------|--------|
| `MedicineTests` | 4/4 pass | ⬜ |
| `SaleTests` | 2/2 pass | ⬜ |
| `PrescriptionTests` | 1/1 pass | ⬜ |
| Build warnings | 0 | ⬜ |

---

## 📝 Notes for Phase 2

- Domain layer is **pure C#** — no WinForms, no DB references.
- Phase 2 will implement `InMemory` repositories using `List<T>`.
- Seed data (5–10 medicines) will be added in Phase 2 for testing.
