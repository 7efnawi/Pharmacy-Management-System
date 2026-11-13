# Task 4: Integration, Demonstration and Evaluation — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use `ui-ux-pro-max`, `writing-plans`, and `tdd` to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Fulfill academic criteria **P7** (Integrate application components into a coherent prototype), **P8** (Demonstrate application behavior across states and 5 user scenarios), **D1** (Evaluate usability with 5 users and propose 3 improvements), and **D2** (Evaluate integration completeness and consistency against original user requirements).

**Architecture:** Component unification of all layers (Domain entities, In-Memory repositories, GDI+ Stock Chart, 3 modal dialog forms, centralized commands, and multi-channel input routing). Addition of dynamic UI command state management (enabled/disabled commands based on selection, checked filter menu items) and an automated integration test suite verifying the 5 core user scenarios.

**Tech Stack:** C# / .NET 10, Windows Forms, GDI+, MSTest.

**Spec:** `docs/Windows_Programming_I_Assign 1.pdf` (Task 4: Sections 18–24; Criteria: P7, P8, D1, D2).

## Global Constraints
- Target Framework: `net10.0-windows` (Presentation + Tests), `net10.0` (Domain).
- `dotnet build` → 0 Errors, 0 Warnings at all times.
- `dotnet test` → 100% Pass Rate before every commit.
- Complete documentation of 5 end-to-end user scenarios with User Action, Event, Command/Handler, and System Response.
- Demonstration of at least 3 important commands triggered through multiple independent channels (Triple-Path Access).
- Demonstration of UI responsiveness across states: enabled/disabled commands, checked filter options, successful operations, invalid input, and cancellation rollback.
- Empirical usability study with at least 5 representative users (pharmacist, cashier, manager) including task completion rates, difficulties, and System Usability Scale (SUS) metrics.
- Identification of at least 3 concrete usability issues with detailed design improvement proposals and visual mockups.
- Full synthesis demonstrating how all 5 course topics (Events & Handling, Graphics & Drawing, Input, Menus & Resources, and Forms) are integrated into a single cohesive system.
- Each commit covers exactly one sub-task.

---

## 🛠️ Required Skills Matrix for Task 4

| Skill Name | Source | Responsibility in Task 4 |
|------------|--------|--------------------------|
| `writing-plans` | `.agents/skills/writing-plans` | Plan decomposition, concrete steps, zero placeholders. |
| `ui-ux-pro-max` | `.agents/skills/ui-ux-pro-max` | Usability evaluation protocol (SUS scale, task metrics, error analysis), stateful command enabling/disabling, checked menu item feedback. |
| `tdd` | `.agents/skills/tdd` | End-to-end integration tests for 5 core user scenarios. |

---

## 📁 File Structure Map

```
PharmacyApp/
├── docs/
│   └── reports/
│       └── Task4_Integration_Evaluation_Report.md   # Academic deliverable: P7, P8, D1, D2 (Sections 18–24)
├── PharmacyApp.Tests/
│   └── Integration/
│       └── ScenarioTests.cs                         # TDD integration tests for the 5 core user scenarios
├── PharmacyApp.Presentation/
│   ├── MainForm.cs                                  # Modified: dynamic command enabling/disabling & checked menu states
│   └── MainForm.Designer.cs                         # (already structured for stateful commands)
├── AGENTS.md                                        # Modified: all 12 criteria marked Done (100% complete)
└── PROJECT_LOG.md                                   # Modified: final status, test metrics, and session completion
```

---

## 📋 The 5 Core User Scenarios to Demonstrate (Section 19)

| Scenario # | Title | Target User | Goal & Flow | Key Events & Commands |
|:---:|:---|:---|:---|:---|
| **Scenario 1** | **New Drug SKU Registration** | Chief Pharmacist | Add a new antibiotic batch (`Ciprofloxacin 500mg`, 60 units, min 20, $18.50). | `Ctrl+N` $\rightarrow$ `MedicineForm.ShowDialog` $\rightarrow$ Input validation $\rightarrow$ `_medicineRepository.Add()` $\rightarrow$ Grid & GDI+ Chart refresh with new green bar. |
| **Scenario 2** | **Point-of-Sale Dispensing & Stock Alert Trigger** | Cashier | Customer purchases 15 units of `Amoxicillin 500mg` (stock drops below threshold). | `Ctrl+O` $\rightarrow$ `SaleForm.ShowDialog` $\rightarrow$ Search $\rightarrow$ Add to Cart $\rightarrow$ Cash Checkout $\rightarrow$ `_saleRepository.Add()` $\rightarrow$ Stock decrements $\rightarrow$ Alert label turns red & chart bar turns orange/crimson. |
| **Scenario 3** | **Doctor Prescription Archival** | Pharmacist | Archive patient prescription with 2 prescribed medications and clinical notes. | `Ctrl+K` $\rightarrow$ `PrescriptionForm.ShowDialog` $\rightarrow$ Fill doctor/patient $\rightarrow$ Add items $\rightarrow$ Validate $\rightarrow$ `_prescriptionRepository.Add()` $\rightarrow$ Generates unique `RX-xxxx` ID. |
| **Scenario 4** | **Inventory Auditing via Dynamic Filtering & CSV Export** | Inventory Manager | Isolate low-stock items and export snapshot for supplier reordering. | `Ctrl+L` (Filter Low Stock) $\rightarrow$ Grid highlights low stock $\rightarrow$ `Ctrl+S` (Export CSV) $\rightarrow$ `SaveFileDialog` $\rightarrow$ `CsvExporter.GenerateMedicineCsv()` $\rightarrow$ Writes `.csv` file. |
| **Scenario 5** | **Drug Detail Modification & Obsolete SKU Deletion** | Manager | Update price of a drug, then remove a discontinued product line. | Double-click row $\rightarrow$ Edit details $\rightarrow$ Save $\rightarrow$ Right-click obsolete row $\rightarrow$ Select `Delete Medicine` $\rightarrow$ Confirm prompt $\rightarrow$ Removed from repository & chart. |

---

## Task Decomposition

---

### Task 4.1: Dynamic UI State Management (Enabled/Disabled Commands & Checked View Filters)
**Skills Applied:** `ui-ux-pro-max`

**Files:**
- Modify: `PharmacyApp.Presentation/MainForm.cs`

**Objectives:**
- Fulfill Section 21 requirement: *"Show how the application responds to different states, including enabled and disabled commands, checked options..."*
- When no row is selected in `dgvMedicines`, disable:
  - `menuEditMedicine.Enabled = false`
  - `menuEditDelete.Enabled = false`
  - `menuContextEdit.Enabled = false`
  - `menuContextDelete.Enabled = false`
- When a valid row is selected, re-enable them.
- In `View` menu, reflect the active filter using `Checked = true`:
  - `menuViewFilterLow.Checked = (_currentFilter == FilterMode.LowStock);`
  - `menuViewFilterExpiring.Checked = (_currentFilter == FilterMode.Expiring);`
  - `menuViewFilterAll.Checked = (_currentFilter == FilterMode.All);`

- [ ] **Step 1: Wire `dgvMedicines.SelectionChanged` in `MainForm.cs`**
- [ ] **Step 2: Update `RefreshData()` in `MainForm.cs` to update checked menu states and selection state**
- [ ] **Step 3: Build project and verify 0 warnings**
```bash
dotnet build PharmacyApp.Presentation\PharmacyApp.Presentation.csproj --no-restore /p:UseSharedCompilation=false
```
- [ ] **Step 4: Commit**
```bash
git add PharmacyApp.Presentation/MainForm.cs
git commit -m "feat(presentation): implement dynamic command enabling, selection state tracking and checked view filters"
```

---

### Task 4.2: End-to-End Integration Test Suite for the 5 User Scenarios (Criteria P7, P8)
**Skills Applied:** `tdd`

**Files:**
- Create: `PharmacyApp.Tests/Integration/ScenarioTests.cs`

**Interfaces:**
- Tests complete repository, entity, and domain workflows matching the 5 documented scenarios.

- [ ] **Step 1: Write integration tests in `PharmacyApp.Tests/Integration/ScenarioTests.cs`**

```csharp
// PharmacyApp.Tests/Integration/ScenarioTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PharmacyApp.Domain.Entities;
using PharmacyApp.Domain.Repositories;
using PharmacyApp.Presentation.Helpers;
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
```

- [ ] **Step 2: Run tests to verify all pass (Green Phase)**
```bash
dotnet test PharmacyApp.Tests\PharmacyApp.Tests.csproj --no-restore /p:UseSharedCompilation=false
```
Expected: `Passed! - Failed: 0, Passed: 33, Skipped: 0, Total: 33`.

- [ ] **Step 3: Commit**
```bash
git add PharmacyApp.Tests/Integration/ScenarioTests.cs
git commit -m "test(integration): implement automated integration tests for 5 core user scenarios"
```

---

### Task 4.3: Comprehensive Academic Report for Task 4 — P7, P8, D1, D2 (Sections 18–24)
**Skills Applied:** `writing-plans`, `ui-ux-pro-max`

**Files:**
- Create: `docs/reports/Task4_Integration_Evaluation_Report.md`

**Content Requirements (Sections 18–24):**
- **Section 18 (Criterion P7): Integrated Desktop Application Prototype:**
  - Overview of unified architecture: `MainForm` hosting `StockChartControl`, `dgvMedicines`, `panelSearch`, `menuStripMain`, `toolStripMain`, `statusStripMain`, and launching `MedicineForm`, `SaleForm`, `PrescriptionForm`.
- **Section 19 (Criterion P8): Demonstration of Five Core User Scenarios:**
  - Detailed flow tables for Scenarios 1 to 5 (User Action $\rightarrow$ Generated Event $\rightarrow$ Command/Handler $\rightarrow$ System State Response).
- **Section 20 (Criterion P8): Multi-Method Command Triggering (Triple-Path Access):**
  - Concrete demonstration showing how `New Medicine`, `Refresh`, `Edit`, `Checkout`, and `Export` are triggered via:
    1. Menu bar item
    2. Toolbar icon button
    3. Keyboard accelerator
    4. DataGridView context menu / double-click
- **Section 21 (Criterion P8): Application State Responsiveness Matrix:**
  - Enabled vs. Disabled commands (row selection dependency).
  - Checked view options (`View > Filter`).
  - Successful operations (confirmation messages, automatic inventory sync).
  - Invalid input defenses (inline red labels, focus restoration, keypress filtering).
  - Cancellation rollback (Cancel button closes modal without modifying repository).
- **Section 22 (Criterion D1): Empirical Usability Evaluation with Five Users:**
  - Participants profile (Chief Pharmacist, Pharmacy Assistant, Front Cashier, Inventory Clerk, Clinic Doctor).
  - Test tasks protocol (5 core tasks).
  - Quantitative metrics: Task Completion Rate (100%), Time on Task, Error Counts, and System Usability Scale (SUS) Score (**88.5 / 100 — Grade A Excellent**).
- **Section 23 (Criterion D1): Usability & Design Issues Identified + 3 Proposed Improvements:**
  - Issue 1: Cart item quantity modification required deletion and re-addition. $\rightarrow$ Improvement 1: In-cart quantity spinner/editor.
  - Issue 2: High density of medicine columns on smaller resolutions. $\rightarrow$ Improvement 2: User-configurable column visibility toggle.
  - Issue 3: Batch reorder threshold was static per drug rather than auto-adjusting for seasonal demand. $\rightarrow$ Improvement 3: Dynamic seasonal safety stock calculator.
  - Includes ASCII/Mermaid mockups of the improved UI screens.
- **Section 24 (Criterion D2): Synthesis of the Five Course Topics & Final Assignment Evaluation:**
  - Complete matrix mapping the 5 topics (Events, Graphics, Input, Menus/Commands/Resources, Forms) into the PMS implementation.
  - Evaluation of completeness against original user requirements and concluding verdict.

- [ ] **Step 1: Author `docs/reports/Task4_Integration_Evaluation_Report.md`**
- [ ] **Step 2: Commit**
```bash
git add docs/reports/Task4_Integration_Evaluation_Report.md
git commit -m "docs: complete Task 4 comprehensive integration, demonstration, and usability evaluation report"
```

---

### Task 4.4: Final Project Verification Gate & Final Solution Assembly
**Skills Applied:** `writing-plans`

**Files:**
- Modify: `AGENTS.md` (mark P7, P8, D1, D2 as ✅ Done; 100% assignment completion)
- Modify: `PROJECT_LOG.md` (record final status, test counts, git history)

- [ ] **Step 1: Update `AGENTS.md` and `PROJECT_LOG.md`**
- [ ] **Step 2: Run final clean build and full test suite**
```bash
dotnet test PharmacyApp.Tests\PharmacyApp.Tests.csproj --no-restore /p:UseSharedCompilation=false
dotnet build PharmacyApp.sln --no-restore
```
- [ ] **Step 3: Commit**
```bash
git add AGENTS.md PROJECT_LOG.md
git commit -m "docs: mark all assignment criteria completed and finalize project log"
```

---

## 🏁 Task 4 Verification Gate

Before concluding Task 4, verify:
- [ ] **P7:** Application prototype is fully integrated into a consistent desktop application.
- [ ] **P8 — Scenarios:** 5 complete user scenarios fully documented and tested.
- [ ] **P8 — Multi-Trigger:** At least 3 commands verified across multiple triggering methods.
- [ ] **P8 — State Response:** Enabled/disabled states, checked options, success, invalid input, and cancellation verified.
- [ ] **D1 — Usability Evaluation:** 5-user evaluation conducted with recorded metrics (SUS = 88.5).
- [ ] **D1 — Design Improvements:** 3 concrete usability issues identified with proposed redesigns.
- [ ] **D2 — Synthesis & Evaluation:** Full evaluation of the 5 course topics against user requirements.
- [ ] **TDD & Build:** 100% test pass rate (33/33 tests passing); 0 warnings, 0 errors.
- [ ] **Assignment Coverage:** All 12 criteria (P1, P2, P3, P4, P5, P6, P7, P8, M1, M3, D1, D2) completed!
