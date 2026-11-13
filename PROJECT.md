# 📋 PROJECT.md — Single Source of Truth
## Pharmacy Management System | C# WinForms

---

## 🏥 Problem Statement

A local pharmacy needs a Windows desktop application to manage its daily operations:
medicine inventory, sales transactions, prescription tracking, supplier management,
and expiry date alerts — replacing manual paper-based processes.

**Target Users:**
- **Pharmacist:** Main user — adds medicines, processes sales, issues prescriptions.
- **Cashier:** Processes sales and prints receipts.
- **Manager:** Views reports, manages inventory, monitors stock levels.

---

## 🏗️ Tech Stack

| Component | Technology |
|-----------|-----------|
| Language | C# (.NET) |
| UI Framework | Windows Forms (WinForms) |
| Graphics | GDI+ (`System.Drawing`) |
| Data Storage | In-Memory Collections (List<T>, Dictionary) |
| Testing | MSTest / xUnit |
| IDE | Visual Studio |
| Version Control | Git |

---

## 📐 Architecture — Layer Map

```
PharmacyApp/
│
├── Domain/
│   ├── Entities/
│   │   ├── Medicine.cs          # MedicineId, Name, Category, Price, Stock, ExpiryDate
│   │   ├── Sale.cs              # SaleId, Date, Items, Total, PaymentMethod
│   │   ├── SaleItem.cs          # MedicineId, Quantity, UnitPrice, Subtotal
│   │   └── Prescription.cs      # PrescriptionId, PatientName, DoctorName, Date, Items
│   └── Interfaces/
│       ├── IMedicineRepository.cs
│       ├── ISaleRepository.cs
│       └── IPrescriptionRepository.cs
│
├── Data/
│   └── InMemory/
│       ├── InMemoryMedicineRepository.cs
│       ├── InMemorySaleRepository.cs
│       └── InMemoryPrescriptionRepository.cs
│
├── Services/
│   ├── MedicineService.cs       # AddMedicine, EditMedicine, DeleteMedicine, Search, GetLowStock
│   ├── SaleService.cs           # CreateSale, GetSaleHistory, GetDailySales
│   └── PrescriptionService.cs   # AddPrescription, SearchByPatient
│
├── Presentation/
│   ├── Forms/
│   │   ├── MainForm.cs          # MDI Parent, MenuBar, ToolBar, Stock Chart
│   │   ├── MedicineForm.cs      # Add/Edit Medicine
│   │   ├── SaleForm.cs          # New Sale / Billing
│   │   ├── PrescriptionForm.cs  # Add Prescription
│   │   └── SearchForm.cs        # Search Medicine
│   ├── Controls/
│   │   └── StockChartControl.cs # Custom GDI+ Bar Chart
│   └── Resources/
│       ├── icons/
│       └── strings/
│
└── Tests/
    ├── Domain/
    └── Services/
```

---

## 💊 Domain Entities

### Medicine
```csharp
public class Medicine {
    public string MedicineId { get; set; }   // Auto-generated: "MED-001"
    public string Name { get; set; }          // Required, max 100 chars
    public string Category { get; set; }      // "Antibiotic", "Painkiller", etc.
    public string Manufacturer { get; set; }  // Required
    public decimal Price { get; set; }        // > 0
    public int StockQuantity { get; set; }    // >= 0
    public int MinStockLevel { get; set; }    // Alert threshold
    public DateTime ExpiryDate { get; set; }  // Must be future date
    public string Unit { get; set; }          // "Tablet", "Bottle", "Box"
}
```

### Sale
```csharp
public class Sale {
    public string SaleId { get; set; }
    public DateTime SaleDate { get; set; }
    public List<SaleItem> Items { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentMethod { get; set; }  // "Cash", "Card"
    public string? PrescriptionId { get; set; }
}
```

### Prescription
```csharp
public class Prescription {
    public string PrescriptionId { get; set; }
    public string PatientName { get; set; }
    public string DoctorName { get; set; }
    public DateTime Date { get; set; }
    public List<SaleItem> Medicines { get; set; }
    public string Notes { get; set; }
}
```

---

## 🖥️ Forms Specification

### 1. MainForm (MDI Parent)
| Control | Type | Purpose |
|---------|------|---------|
| `menuStrip1` | MenuStrip | Main navigation menu |
| `toolStrip1` | ToolStrip | Quick-access toolbar |
| `stockChartPanel` | Panel | GDI+ stock level chart area |
| `dgvMedicines` | DataGridView | Medicine list |
| `lblAlerts` | Label | Expiry/low-stock alerts |
| `statusStrip1` | StatusStrip | Current user, date, status |

### 2. MedicineForm (Add/Edit)
| Control | Type | Purpose | Validation |
|---------|------|---------|------------|
| `txtName` | TextBox | Medicine name | Required, max 100 |
| `cboCategory` | ComboBox | Category selection | Required |
| `txtPrice` | TextBox | Unit price | > 0, numeric |
| `txtStock` | TextBox | Initial stock | >= 0, integer |
| `txtMinStock` | TextBox | Alert threshold | >= 0, integer |
| `dtpExpiry` | DateTimePicker | Expiry date | Future date only |
| `cboUnit` | ComboBox | Unit type | Required |
| `btnSave` | Button | Save record | - |
| `btnCancel` | Button | Cancel | - |

### 3. SaleForm
| Control | Type | Purpose | Validation |
|---------|------|---------|------------|
| `txtSearch` | TextBox | Search medicine | - |
| `dgvAvailable` | DataGridView | Available medicines | - |
| `dgvCart` | DataGridView | Sale items | - |
| `txtQuantity` | TextBox | Quantity | > 0, <= stock |
| `lblTotal` | Label | Running total | - |
| `cboPayment` | ComboBox | Payment method | Required |
| `btnAddToCart` | Button | Add to cart | - |
| `btnCheckout` | Button | Finalize sale | Cart not empty |

### 4. PrescriptionForm
| Control | Type | Purpose | Validation |
|---------|------|---------|------------|
| `txtPatient` | TextBox | Patient name | Required |
| `txtDoctor` | TextBox | Doctor name | Required |
| `dtpDate` | DateTimePicker | Prescription date | Required |
| `dgvItems` | DataGridView | Prescribed medicines | Min 1 item |
| `txtNotes` | RichTextBox | Notes | Optional |

### 5. SearchForm
| Control | Type | Purpose |
|---------|------|---------|
| `txtSearch` | TextBox | Search query |
| `cboSearchBy` | ComboBox | Search field (Name/Category/ID) |
| `dgvResults` | DataGridView | Search results |
| `btnSelect` | Button | Select result |

---

## 🎮 Commands & Resources

| # | Command | Command ID | Shortcut | Handler | Toolbar |
|---|---------|-----------|----------|---------|---------|
| 1 | New Medicine | `CMD_NEW_MEDICINE` | Ctrl+N | `OnNewMedicine` | ✅ |
| 2 | Save | `CMD_SAVE` | Ctrl+S | `OnSave` | ✅ |
| 3 | Search | `CMD_SEARCH` | Ctrl+F | `OnSearch` | ✅ |
| 4 | New Sale | `CMD_NEW_SALE` | Ctrl+O | `OnNewSale` | ✅ |
| 5 | Print | `CMD_PRINT` | Ctrl+P | `OnPrint` | ✅ |
| 6 | Delete | `CMD_DELETE` | Delete | `OnDelete` | ❌ |
| 7 | Edit | `CMD_EDIT` | F2 | `OnEdit` | ❌ |
| 8 | Reports | `CMD_REPORTS` | Ctrl+R | `OnReports` | ✅ |
| 9 | Prescription | `CMD_PRESCRIPTION` | Ctrl+K | `OnPrescription` | ✅ |
| 10 | Refresh | `CMD_REFRESH` | F5 | `OnRefresh` | ✅ |
| 11 | Exit | `CMD_EXIT` | Alt+F4 | `OnExit` | ❌ |
| 12 | About | `CMD_ABOUT` | F1 | `OnAbout` | ❌ |

---

## 📊 Graphical Element — Stock Level Chart

**Type:** Custom GDI+ Bar Chart (`StockChartControl : UserControl`)

**Purpose:** Visualizes current stock levels vs. minimum threshold for top medicines.

**Visual Design:**
- X-axis: Medicine names
- Y-axis: Quantity
- Green bars → Stock above minimum
- Yellow bars → Stock near minimum (< 2x min)
- Red bars → Stock below minimum (critical)
- Dashed line → Minimum stock threshold

**Events it responds to:**
- Medicine added → Chart refreshes
- Sale completed → Chart updates
- Form resize → Chart redraws

---

## 🗓️ Project Phases

| Phase | Content | Status |
|-------|---------|--------|
| **Phase 0** | Scaffolding, AGENTS.md, PROJECT.md, PROJECT_LOG.md | ✅ Done |
| **Phase 1** | Domain Layer — Entities + Interfaces | ⬜ Pending |
| **Phase 2** | Data Layer — InMemory Repositories | ⬜ Pending |
| **Phase 3** | Services Layer — Business Logic | ⬜ Pending |
| **Phase 4** | Presentation — MainForm + Chart | ⬜ Pending |
| **Phase 5** | Presentation — MedicineForm | ⬜ Pending |
| **Phase 6** | Presentation — SaleForm | ⬜ Pending |
| **Phase 7** | Presentation — PrescriptionForm + SearchForm | ⬜ Pending |
| **Phase 8** | Menus, Toolbars, Shortcuts, Resources | ⬜ Pending |
| **Phase 9** | Integration, Scenarios, Usability Eval | ⬜ Pending |
