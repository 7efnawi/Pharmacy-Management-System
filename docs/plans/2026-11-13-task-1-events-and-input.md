# Task 1: Events, Event Handling, Keyboard & Mouse Input — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use `tdd`, `domain-modeling`, and `codebase-design` to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Fulfill all academic criteria for Task 1 (P1, P2, P3, M1) by specifying problem/users, defining 6 functional requirements, modeling 6 system events, generating the event-handling table, modeling 3 event-flow diagrams, specifying keyboard/mouse interactions, and implementing the underlying event and transaction logic in C# using TDD.

**Architecture:** Clean Architecture with Domain-Driven Event handling: Pure Domain entities & contracts handle state logic; Service layer acts as the event-processing seam; WinForms Presentation layer captures user inputs (keyboard, mouse) and delegates to event handlers.

**Tech Stack:** C# 10 / .NET 10, Windows Forms, MSTest, Mermaid.js (for Event-Flow Diagrams).

**Spec:** `Windows_Programming_I_Assign 1.pdf` (Task 1: Sections 1 to 6; Criteria: P1, P2, P3, M1).

## Global Constraints
- Target Framework: `net10.0-windows` (Presentation), `net10.0` (Domain, Tests).
- 0 Warnings, 0 Errors on `dotnet build`.
- 100% Pass rate on `dotnet test`.
- No speculative features: Implement strictly what is required by Task 1 (6 requirements, 6 events, 3 event flows, mouse/keyboard specs).
- Every code task must strictly follow TDD Red-Green-Refactor.

---

## 🛠️ Required Skills Matrix for Task 1

| Skill Name | Source Location | Responsibility in Task 1 |
|------------|-----------------|--------------------------|
| `writing-plans` | `.agents/skills/writing-plans` | Plan decomposition, strict task boundaries, zero placeholders. |
| `domain-modeling` | `.agents/skills/domain-modeling` | Formalize Users, Responsibilities, and 6 Functional Requirements. |
| `tdd` | `.agents/skills/tdd` | Test-first development for Sales, Cart, Prescriptions, and Event Handlers. |
| `codebase-design` | `.agents/skills/codebase-design` | Design clean seams between UI Event Handlers and Domain Services. |
| `ui-ux-pro-max` | `.agents/skills/ui-ux-pro-max` | Desktop input ergonomics: Tab Navigation order, Enter/Esc accelerators, Grid Double-click selection. |

---

## 📁 File Structure Map

```
PharmacyApp/
├── docs/
│   └── reports/
│       └── Task1_Events_and_Input_Report.md  # Academic deliverable for P1, P2, P3, M1
├── PharmacyApp.Domain/
│   ├── Entities/
│   │   ├── Medicine.cs                       # (Implemented in Sub-task 1.1)
│   │   ├── SaleItem.cs                       # Line item for sales and prescriptions
│   │   ├── Sale.cs                           # Sale transaction with computed total
│   │   └── Prescription.cs                   # Patient, Doctor, items
│   └── Interfaces/
│       ├── IMedicineRepository.cs            # Medicine contract
│       └── ISaleRepository.cs                # Sales transaction contract
├── PharmacyApp.Data/
│   └── InMemory/
│       ├── InMemoryMedicineRepository.cs     # In-memory storage with search & alert filters
│       └── InMemorySaleRepository.cs         # Sales storage & stock synchronization
├── PharmacyApp.Services/
│   ├── MedicineService.cs                    # Event processing for medicine management
│   └── SaleService.cs                        # Event processing for transactions & stock updates
├── PharmacyApp.Presentation/
│   └── Forms/
│       └── MainForm.cs                       # WinForms event handlers (Keyboard/Mouse)
└── PharmacyApp.Tests/
    └── Domain/
        ├── MedicineTests.cs                  # (Completed - 4 tests passing)
        ├── SaleTests.cs                      # Tests for Sale and SaleItem calculations
        ├── RepositoryTests.cs                # Tests for In-Memory repositories
        └── ServiceTests.cs                   # Tests for business event execution
```

---

## Task Decomposition

---

### Task 1.1: Academic Specification Document (P1 — Problem, Users & 6 Functional Requirements)
**Skills Applied:** `domain-modeling`, `writing-plans`

**Files:**
- Create: `docs/reports/Task1_Specification.md`

**Interfaces:**
- Produces: The 6 canonical Functional Requirements (FR1 to FR6) that drive all downstream event handlers and UI controls.

- [ ] **Step 1: Draft User Roles & Responsibilities Section**
Document target users (Pharmacist, Cashier, Manager) with specific responsibilities and permissions.

- [ ] **Step 2: Define 6 Functional Requirements (FR1 - FR6)**
Create a structured table with:
  1. Requirement ID & Title
  2. User Action (Trigger)
  3. System Expected Response
  4. Information Affected / State Change
- [ ] **Step 3: Review against Assignment Criterion P1**
Verify all 6 requirements represent distinct operational domains (Inventory, Sales, Search, Alerts, Prescriptions, Edits).

- [ ] **Step 4: Commit**
```bash
git add docs/reports/Task1_Specification.md
git commit -m "docs: define problem, target users and 6 functional requirements for P1"
```

---

### Task 1.2: Domain Transaction Entities — `SaleItem`, `Sale`, `Prescription` (TDD)
**Skills Applied:** `tdd`, `domain-modeling`

**Files:**
- Create: `PharmacyApp.Tests/Domain/SaleTests.cs`
- Create: `PharmacyApp.Domain/Entities/SaleItem.cs`
- Create: `PharmacyApp.Domain/Entities/Sale.cs`
- Create: `PharmacyApp.Domain/Entities/Prescription.cs`

**Interfaces:**
- Consumes: `Medicine.cs`
- Produces: `SaleItem` (`Subtotal => Quantity * UnitPrice`), `Sale` (`TotalAmount => Items.Sum(i => i.Subtotal)`), `Prescription`

- [ ] **Step 1: Write the failing tests in `SaleTests.cs`**
```csharp
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
            var item = new SaleItem
            {
                MedicineId = "MED-001",
                MedicineName = "Amoxicillin",
                Quantity = 3,
                UnitPrice = 12.50m
            };

            Assert.AreEqual(37.50m, item.Subtotal);
        }

        [TestMethod]
        public void Sale_TotalAmount_CalculatesSumOfAllItemSubtotals()
        {
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

            Assert.AreEqual(35.50m, sale.TotalAmount);
        }
    }
}
```

- [ ] **Step 2: Run tests to verify failure (Red Phase)**
Run: `dotnet test PharmacyApp.Tests/PharmacyApp.Tests.csproj --no-restore`
Expected: FAIL (`SaleItem` and `Sale` do not exist).

- [ ] **Step 3: Implement `SaleItem.cs`, `Sale.cs`, and `Prescription.cs`**
```csharp
// PharmacyApp.Domain/Entities/SaleItem.cs
namespace PharmacyApp.Domain.Entities
{
    public class SaleItem
    {
        public string MedicineId { get; set; } = string.Empty;
        public string MedicineName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal => Quantity * UnitPrice;
    }
}
```

```csharp
// PharmacyApp.Domain/Entities/Sale.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyApp.Domain.Entities
{
    public class Sale
    {
        public string SaleId { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public List<SaleItem> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.Subtotal);
        public string PaymentMethod { get; set; } = "Cash";
        public string? PrescriptionId { get; set; }
    }
}
```

```csharp
// PharmacyApp.Domain/Entities/Prescription.cs
using System;
using System.Collections.Generic;

namespace PharmacyApp.Domain.Entities
{
    public class Prescription
    {
        public string PrescriptionId { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Today;
        public List<SaleItem> Items { get; set; } = new();
        public string Notes { get; set; } = string.Empty;
    }
}
```

- [ ] **Step 4: Run tests to verify pass (Green Phase)**
Run: `dotnet test PharmacyApp.Tests/PharmacyApp.Tests.csproj --no-restore`
Expected: 6/6 tests PASS (4 Medicine + 2 Sale).

- [ ] **Step 5: Commit**
```bash
git add PharmacyApp.Domain/Entities/ PharmacyApp.Tests/Domain/SaleTests.cs
git commit -m "feat(domain): add SaleItem, Sale, and Prescription entities with computed totals"
```

---

### Task 1.3: Repository Contracts & In-Memory Data Storage (TDD)
**Skills Applied:** `tdd`, `codebase-design`

**Files:**
- Create: `PharmacyApp.Domain/Interfaces/IMedicineRepository.cs`
- Create: `PharmacyApp.Domain/Interfaces/ISaleRepository.cs`
- Create: `PharmacyApp.Domain/Interfaces/IPrescriptionRepository.cs`
- Create: `PharmacyApp.Domain/Repositories/InMemoryMedicineRepository.cs`
- Create: `PharmacyApp.Domain/Repositories/InMemorySaleRepository.cs`
- Create: `PharmacyApp.Tests/Domain/RepositoryTests.cs`

**Interfaces:**
- Consumes: `Medicine`, `Sale`, `Prescription`
- Produces: CRUD operations, Search, LowStock query, Expiry query.

- [ ] **Step 1: Write failing repository tests in `RepositoryTests.cs`**
Test `Add`, `GetById`, `Search` (by name or category), and `GetLowStock`.

- [ ] **Step 2: Run test to verify failure (Red Phase)**
Run: `dotnet test PharmacyApp.Tests/PharmacyApp.Tests.csproj --no-restore`
Expected: Compilation fail (Interfaces and Repository classes do not exist).

- [ ] **Step 3: Define Interfaces and Implement In-Memory Repositories**
Implement thread-safe `List<T>` backing stores with seeded sample medicines for realistic demonstration.

- [ ] **Step 4: Run test to verify pass (Green Phase)**
Run: `dotnet test PharmacyApp.Tests/PharmacyApp.Tests.csproj --no-restore`
Expected: PASS with 100% pass rate.

- [ ] **Step 5: Commit**
```bash
git add PharmacyApp.Domain/Interfaces/ PharmacyApp.Domain/Repositories/ PharmacyApp.Tests/Domain/RepositoryTests.cs
git commit -m "feat(data): implement in-memory repositories with search and filter capabilities"
```

---

### Task 1.4: Event Handling Specification & Table (P2)
**Skills Applied:** `domain-modeling`, `codebase-design`

**Files:**
- Modify: `docs/reports/Task1_Specification.md`

**Interfaces:**
- Specifies: 6 events across all required categories (Button, Menu, Keyboard, Mouse, Control, Lifecycle).

- [ ] **Step 1: Identify 6 Primary Application Events**
1. Button Click (`btnSave_Click` / `btnAddToCart_Click`)
2. Menu Click (`menuItemNewMedicine_Click`)
3. Keyboard KeyDown (`txtSearch_KeyDown` / Form Accelerator `KeyDown`)
4. Mouse DoubleClick (`dgvMedicines_CellDoubleClick`)
5. Control ValueChanged (`txtQuantity_TextChanged` / `cboCategory_SelectedIndexChanged`)
6. Form Lifecycle (`MainForm_Load` / `MainForm_FormClosing`)

- [ ] **Step 2: Compile the Event-Handling Table**
Columns:
- Event Source (Control ID & Type)
- Event Type (.NET Event Name)
- Triggering Action (Physical user action)
- Expected Response (Service method / validation)
- Resulting Behavior (UI update / state alteration)

- [ ] **Step 3: Commit**
```bash
git add docs/reports/Task1_Specification.md
git commit -m "docs: add 6 system events and comprehensive event-handling table for P2"
```

---

### Task 1.5: Event-Flow Diagrams (M1 — 3 Key Operations)
**Skills Applied:** `codebase-design`, `mermaid-diagrams`

**Files:**
- Modify: `docs/reports/Task1_Specification.md`

**Interfaces:**
- Produces: Sequence/flow diagrams showing User Action ➡️ Message/Event Detection ➡️ Handler Execution ➡️ Domain Service ➡️ State Change ➡️ UI Feedback.

- [ ] **Step 1: Operation 1 Flow — New Sale & Inventory Deduction**
Diagram user selecting item, entering quantity, event firing, stock validation, cart state update, and total re-calculation.

- [ ] **Step 2: Operation 2 Flow — Real-Time Medicine Search (Keyboard)**
Diagram user typing in `txtSearch`, `TextChanged`/`KeyDown` event dispatch, debouncing/query execution in repository, and grid rebinding.

- [ ] **Step 3: Operation 3 Flow — Expiry & Low-Stock Detection on Application Startup**
Diagram `Form_Load` event, service query for critical inventory, evaluation against thresholds, and status indicator updates.

- [ ] **Step 4: Commit**
```bash
git add docs/reports/Task1_Specification.md
git commit -m "docs: add 3 detailed event-flow diagrams for M1"
```

---

### Task 1.6: Keyboard, Mouse & Input Interaction Specifications (P3)
**Skills Applied:** `ui-ux-pro-max`, `codebase-design`

**Files:**
- Modify: `docs/reports/Task1_Specification.md`
- Modify: `PharmacyApp.Presentation/Form1.cs` (Wire baseline keyboard/mouse events)

**Interfaces:**
- Implements:
  - Selection: Single click on `DataGridView` row.
  - Double-Clicking: Double-click row opens Edit/View dialog.
  - Navigation: Sequential `TabIndex` order for all input fields.
  - Confirmation: `Enter` key triggers primary action (`AcceptButton`).
  - Cancellation: `Esc` key cancels/clears input (`CancelButton`).
  - Accelerators: `Ctrl+N` (New), `Ctrl+S` (Save), `Ctrl+F` (Search).

- [ ] **Step 1: Document input interaction matrix in `Task1_Specification.md`**
Define exact key bindings, mouse actions, control responses, and error-prevention behaviors.

- [ ] **Step 2: Wire baseline WinForms keyboard and mouse interaction handlers in `MainForm`**
Set `KeyPreview = true`, configure `ProcessCmdKey` or `KeyDown`, wire `CellDoubleClick`.

- [ ] **Step 3: Verification**
Verify project builds with 0 warnings: `dotnet build PharmacyApp.sln`.

- [ ] **Step 4: Commit**
```bash
git add docs/reports/Task1_Specification.md PharmacyApp.Presentation/
git commit -m "feat(presentation): implement and document keyboard and mouse interactions for P3"
```

---

## 🏁 Task 1 Verification Gate

Before concluding Task 1, verify all deliverables against the grading rubric:
- [ ] **P1:** Problem, target users, and 6 functional requirements fully documented.
- [ ] **P2:** 6 events and comprehensive Event-Handling table fully completed.
- [ ] **P3:** Keyboard, mouse, selection, double-click, and Enter/Esc interactions specified and wired.
- [ ] **M1:** 3 event-flow diagrams rendered and explained.
- [ ] **TDD Gate:** `dotnet test` achieves 100% pass rate.
- [ ] **Build Gate:** `dotnet build` succeeds with 0 warnings, 0 errors.
- [ ] **Log Updated:** `PROJECT_LOG.md` updated with all commits and progress.
