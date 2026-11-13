# Task 3: Menus, Commands & Resources — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use `ui-ux-pro-max`, `writing-plans`, and `tdd` to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Fulfill academic criteria **P5** (Design menus, commands, and resources with at least 10 commands, 5+ shortcuts, toolbar, menu bar, and resource IDs) and **M3** (Justify design decisions and resource allocation) strictly within the Pharmacy Management System scope.

**Architecture:** Centralized command definition architecture (`AppCommands`) mapping logical command IDs to UI triggers (MenuStrip, ToolStrip, ContextMenuStrip, and KeyPreview keyboard accelerators). Actions are dispatched to existing repository and form layers, including new features: Inventory CSV Export, Item Deletion with safety confirmation, and Low-Stock/Expiry quick filters.

**Tech Stack:** C# / .NET 10, Windows Forms (`MenuStrip`, `ToolStrip`, `ContextMenuStrip`, `SaveFileDialog`), MSTest, Domain Repositories.

**Spec:** `docs/Windows_Programming_I_Assign 1.pdf` (Task 3: Sections 14–17; Criteria: P5, M3).

## Global Constraints
- Target Framework: `net10.0-windows` (Presentation + Tests), `net10.0` (Domain).
- `dotnet build` → 0 Errors, 0 Warnings at all times.
- `dotnet test` → 100% Pass Rate before every commit.
- Minimum 10 application commands with unique Command IDs, purposes, actions, and handlers.
- Minimum 5 keyboard shortcuts assigned to frequently used commands.
- Resource identification with explicit Resource IDs (Menus, Dialogs, Icons, Strings, Accelerators).
- Detailed architectural justifications fulfilling Criterion M3.
- Each commit covers exactly one sub-task.

---

## 🛠️ Required Skills Matrix for Task 3

| Skill Name | Source | Responsibility in Task 3 |
|------------|--------|--------------------------|
| `writing-plans` | `.agents/skills/writing-plans` | Plan decomposition, concrete code steps, zero placeholders. |
| `ui-ux-pro-max` | `.agents/skills/ui-ux-pro-max` | Menu hierarchy, Fitts' law toolbar grouping, mnemonic access keys, context menus. |
| `tdd` | `.agents/skills/tdd` | Unit tests for command registry and shortcut uniqueness. |

---

## 📁 File Structure Map

```
PharmacyApp/
├── docs/
│   └── reports/
│       └── Task3_Menus_Commands_Resources_Report.md   # Academic deliverable: P5 & M3
├── PharmacyApp.Tests/
│   └── Presentation/
│       └── CommandTests.cs                             # TDD tests for command registry & uniqueness
├── PharmacyApp.Presentation/
│   ├── Constants/
│   │   └── AppCommands.cs                              # Centralized Command IDs, Resource IDs, & Metadata
│   ├── MainForm.cs                                     # Modified: wire new commands, filters, export, delete
│   └── MainForm.Designer.cs                            # Modified: expanded MenuStrip, ToolStrip, ContextMenuStrip
```

---

## 🎮 Command Specification Table (12 Commands — Exceeds Assignment Minimum of 10)

| # | Command Name | Unique Command ID | Shortcut | Menu Location | Toolbar Button | Context Menu | Command Handler |
|---|--------------|-------------------|----------|---------------|:--------------:|:------------:|-----------------|
| 1 | **New Medicine** | `CMD_FILE_NEW_MEDICINE` | `Ctrl+N` | `File > New Medicine` | ➕ New Medicine | ➕ Add New | `MenuFileNewMedicine_Click` |
| 2 | **Export CSV** | `CMD_FILE_EXPORT_CSV` | `Ctrl+S` | `File > Export to CSV...` | 💾 Export CSV | — | `MenuFileExportCsv_Click` |
| 3 | **Exit** | `CMD_FILE_EXIT` | `Alt+F4` | `File > Exit` | — | — | `MenuFileExit_Click` |
| 4 | **Edit Medicine** | `CMD_EDIT_MEDICINE` | `F2` | `Edit > Edit Selected...` | — | ✏️ Edit Details | `MenuEditMedicine_Click` |
| 5 | **Delete Medicine** | `CMD_EDIT_DELETE` | `Delete` | `Edit > Delete Selected` | — | 🗑️ Delete | `MenuEditDelete_Click` |
| 6 | **Find / Search** | `CMD_VIEW_SEARCH` | `Ctrl+F` | `Edit > Find Medicine` | — | — | `MenuEditFind_Click` |
| 7 | **Refresh Data** | `CMD_VIEW_REFRESH` | `F5` | `View > Refresh All` | 🔄 Refresh (F5) | 🔄 Refresh | `BtnRefresh_Click` |
| 8 | **Filter Low Stock** | `CMD_VIEW_FILTER_LOW` | `Ctrl+L` | `View > Filter: Low Stock` | ⚠️ Low Stock | — | `MenuFilterLowStock_Click` |
| 9 | **Filter Expiring** | `CMD_VIEW_FILTER_EXPIRY`| `Ctrl+E` | `View > Filter: Expiring Soon` | ⏳ Expiring | — | `MenuFilterExpiring_Click` |
| 10 | **Show All** | `CMD_VIEW_FILTER_ALL` | `Ctrl+A` | `View > Show All Items` | 📋 Show All | — | `MenuFilterShowAll_Click` |
| 11 | **New Sale** | `CMD_OP_NEW_SALE` | `Ctrl+O` | `Operations > Point of Sale (POS)` | 🛒 New Sale | — | `MenuOperationsNewSale_Click` |
| 12 | **Prescription** | `CMD_OP_PRESCRIPTION` | `Ctrl+K` | `Operations > New Prescription` | 💊 Prescription | — | `MenuOperationsPrescription_Click` |
| 13 | **About PMS** | `CMD_HELP_ABOUT` | `F1` | `Help > About PMS` | — | — | `MenuHelpAbout_Click` |

---

## 📦 Resource Specification Table (Assignment Requirement Section 17)

| Resource Type | Resource ID | Description / Purpose | Associated Component |
|:---|:---|:---|:---|
| **Menu** | `IDR_MENU_MAIN` | Main application hierarchical menu bar | `menuStripMain` |
| **Menu (Context)**| `IDR_MENU_CONTEXT_GRID` | Right-click contextual actions for inventory rows | `contextMenuGrid` |
| **Toolbar** | `IDR_TOOLBAR_MAIN` | Quick-action button strip with icons and accelerators | `toolStripMain` |
| **Dialog Box** | `IDD_MEDICINE_DIALOG` | Modal dialog for registering and editing drug SKUs | `MedicineForm` |
| **Dialog Box** | `IDD_SALE_DIALOG` | Modal point-of-sale dispensing and checkout dialog | `SaleForm` |
| **Dialog Box** | `IDD_PRESCRIPTION_DIALOG`| Modal clinical prescription archival dialog | `PrescriptionForm` |
| **Dialog Box** | `IDD_ABOUT_DIALOG` | Standard informational modal with application build metadata | `MessageBox` (`MenuHelpAbout`) |
| **Accelerator** | `IDA_MAIN_ACCELERATORS`| Global keyboard shortcut dispatch table (`KeyPreview`) | `MainForm_KeyDown` |
| **String** | `IDS_APP_TITLE` | Application window title string | `"Pharmacy Management System — Windows Programming I"` |
| **String** | `IDS_STATUS_READY` | Default status strip message | `"Ready"` |
| **String** | `IDS_ALERT_LOW_STOCK` | Alert string template for depleted inventory | `"⚠️ Alerts: {0} Low Stock | {1} Expiring Soon"` |
| **Bitmap / Glyphs**| `IDB_GLYPH_ADD` | Visual glyph for addition actions (➕) | Toolbar / Menu items |
| **Bitmap / Glyphs**| `IDB_GLYPH_SALE` | Visual glyph for POS sales checkout (🛒) | Toolbar / Menu items |
| **Bitmap / Glyphs**| `IDB_GLYPH_RX` | Visual glyph for clinical prescription dispensing (💊) | Toolbar / Menu items |
| **Bitmap / Glyphs**| `IDB_GLYPH_REFRESH` | Visual glyph for data synchronization (🔄) | Toolbar / Menu items |
| **Bitmap / Glyphs**| `IDB_GLYPH_EXPORT` | Visual glyph for CSV file export (💾) | Toolbar / Menu items |

---

## Task Decomposition

---

### Task 3.1: Centralized Command & Resource Architecture (`AppCommands.cs` + TDD Tests)
**Skills Applied:** `tdd`, `ui-ux-pro-max`

**Files:**
- Create: `PharmacyApp.Presentation/Constants/AppCommands.cs`
- Create: `PharmacyApp.Tests/Presentation/CommandTests.cs`

**Interfaces:**
- Produces: `AppCommands` static class containing:
  - Command ID string constants (`CMD_FILE_NEW_MEDICINE`, etc.)
  - Resource ID string constants (`IDR_MENU_MAIN`, `IDD_MEDICINE_DIALOG`, etc.)
  - `IReadOnlyList<CommandDescriptor> GetAllCommands()` for reflective command auditing.

- [ ] **Step 1: Write failing command registry tests in `CommandTests.cs`**

```csharp
// PharmacyApp.Tests/Presentation/CommandTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PharmacyApp.Presentation.Constants;
using System.Linq;

namespace PharmacyApp.Tests.Presentation
{
    [TestClass]
    public class CommandTests
    {
        [TestMethod]
        public void GetAllCommands_ReturnsAtLeastTenCommands()
        {
            var commands = AppCommands.GetAllCommands();
            Assert.IsTrue(commands.Count >= 10, $"Expected at least 10 commands, got {commands.Count}");
        }

        [TestMethod]
        public void CommandIds_MustBeUnique()
        {
            var commands = AppCommands.GetAllCommands();
            var duplicateIds = commands
                .GroupBy(c => c.CommandId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            Assert.AreEqual(0, duplicateIds.Count, $"Duplicate Command IDs found: {string.Join(", ", duplicateIds)}");
        }

        [TestMethod]
        public void KeyboardShortcuts_HasAtLeastFiveConfigured()
        {
            var commands = AppCommands.GetAllCommands();
            int shortcutCount = commands.Count(c => !string.IsNullOrWhiteSpace(c.ShortcutText));
            Assert.IsTrue(shortcutCount >= 5, $"Expected at least 5 shortcuts, found {shortcutCount}");
        }

        [TestMethod]
        public void Shortcuts_MustBeUniqueExceptNone()
        {
            var commands = AppCommands.GetAllCommands()
                .Where(c => !string.IsNullOrWhiteSpace(c.ShortcutText));

            var duplicateShortcuts = commands
                .GroupBy(c => c.ShortcutText)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            Assert.AreEqual(0, duplicateShortcuts.Count, $"Duplicate shortcuts found: {string.Join(", ", duplicateShortcuts)}");
        }
    }
}
```

- [ ] **Step 2: Run test to verify failure (Red Phase)**
```bash
dotnet test PharmacyApp.Tests\PharmacyApp.Tests.csproj --no-restore
```
Expected: FAIL (Compilation error: `AppCommands` does not exist).

- [ ] **Step 3: Implement `PharmacyApp.Presentation/Constants/AppCommands.cs`**

```csharp
// PharmacyApp.Presentation/Constants/AppCommands.cs
using System.Collections.Generic;

namespace PharmacyApp.Presentation.Constants
{
    public record CommandDescriptor(
        string CommandId,
        string Name,
        string Category,
        string ShortcutText,
        string Description,
        string ResourceId);

    public static class AppCommands
    {
        // Command IDs
        public const string CmdNewMedicine       = "CMD_FILE_NEW_MEDICINE";
        public const string CmdExportCsv         = "CMD_FILE_EXPORT_CSV";
        public const string CmdExit              = "CMD_FILE_EXIT";
        public const string CmdEditMedicine      = "CMD_EDIT_MEDICINE";
        public const string CmdDeleteMedicine    = "CMD_EDIT_DELETE";
        public const string CmdSearch            = "CMD_VIEW_SEARCH";
        public const string CmdRefresh           = "CMD_VIEW_REFRESH";
        public const string CmdFilterLowStock    = "CMD_VIEW_FILTER_LOW";
        public const string CmdFilterExpiring    = "CMD_VIEW_FILTER_EXPIRY";
        public const string CmdFilterAll         = "CMD_VIEW_FILTER_ALL";
        public const string CmdNewSale           = "CMD_OP_NEW_SALE";
        public const string CmdPrescription      = "CMD_OP_PRESCRIPTION";
        public const string CmdAbout             = "CMD_HELP_ABOUT";

        // Resource IDs
        public const string IdrMenuMain          = "IDR_MENU_MAIN";
        public const string IdrMenuContextGrid   = "IDR_MENU_CONTEXT_GRID";
        public const string IdrToolbarMain       = "IDR_TOOLBAR_MAIN";
        public const string IddMedicineDialog    = "IDD_MEDICINE_DIALOG";
        public const string IddSaleDialog        = "IDD_SALE_DIALOG";
        public const string IddPrescriptionDialog= "IDD_PRESCRIPTION_DIALOG";
        public const string IddAboutDialog       = "IDD_ABOUT_DIALOG";
        public const string IdaMainAccelerators  = "IDA_MAIN_ACCELERATORS";

        private static readonly List<CommandDescriptor> _commands = new()
        {
            new(CmdNewMedicine,    "New Medicine",       "File",       "Ctrl+N", "Registers a new drug SKU into repository",            IddMedicineDialog),
            new(CmdExportCsv,      "Export to CSV",      "File",       "Ctrl+S", "Exports inventory snapshot to CSV file",              IdrMenuMain),
            new(CmdExit,           "Exit",               "File",       "Alt+F4", "Safely closes application",                           IdrMenuMain),
            new(CmdEditMedicine,   "Edit Selected",      "Edit",       "F2",     "Opens edit dialog for selected medicine",             IddMedicineDialog),
            new(CmdDeleteMedicine, "Delete Selected",    "Edit",       "Delete", "Removes selected medicine with confirmation",         IdrMenuContextGrid),
            new(CmdSearch,         "Find Medicine",      "Edit",       "Ctrl+F", "Shifts focus to live search input box",               IdrMenuMain),
            new(CmdRefresh,        "Refresh All",        "View",       "F5",     "Reloads grid and GDI+ stock chart from repository",   IdrToolbarMain),
            new(CmdFilterLowStock, "Filter: Low Stock",  "View",       "Ctrl+L", "Restricts view to medicines below minimum threshold", IdrToolbarMain),
            new(CmdFilterExpiring, "Filter: Expiring",   "View",       "Ctrl+E", "Restricts view to medicines expiring within 30 days", IdrToolbarMain),
            new(CmdFilterAll,      "Show All SKUs",      "View",       "Ctrl+A", "Clears filters and displays full inventory",          IdrToolbarMain),
            new(CmdNewSale,        "Point of Sale (POS)","Operations", "Ctrl+O", "Opens checkout register dialog",                      IddSaleDialog),
            new(CmdPrescription,   "New Prescription",   "Operations", "Ctrl+K", "Opens prescription archival and dispensing dialog",   IddPrescriptionDialog),
            new(CmdAbout,          "About PMS",          "Help",       "F1",     "Displays application version and academic credits",   IddAboutDialog)
        };

        public static IReadOnlyList<CommandDescriptor> GetAllCommands() => _commands;
    }
}
```

- [ ] **Step 4: Run tests to verify pass (Green Phase)**
```bash
dotnet test PharmacyApp.Tests\PharmacyApp.Tests.csproj --no-restore
```
Expected: `Passed! - Failed: 0, Passed: 26, Skipped: 0, Total: 26`.

- [ ] **Step 5: Commit**
```bash
git add PharmacyApp.Presentation/Constants/AppCommands.cs PharmacyApp.Tests/Presentation/CommandTests.cs
git commit -m "feat(commands): implement centralized AppCommands registry with 13 commands and TDD tests"
```

---

### Task 3.2: MenuStrip, ToolStrip & ContextMenuStrip Expansion in `MainForm`
**Skills Applied:** `ui-ux-pro-max`

**Files:**
- Modify: `PharmacyApp.Presentation/MainForm.Designer.cs`
- Modify: `PharmacyApp.Presentation/MainForm.cs`

**UI Enhancements Required:**
1. **MenuStrip:**
   - `File`: New Medicine (`Ctrl+N`), Export to CSV (`Ctrl+S`), Separator, Exit (`Alt+F4`).
   - `Edit`: Edit Selected (`F2`), Delete Selected (`Delete`), Separator, Find Medicine (`Ctrl+F`).
   - `View`: Refresh All (`F5`), Separator, Filter: Low Stock (`Ctrl+L`), Filter: Expiring Soon (`Ctrl+E`), Show All Items (`Ctrl+A`).
   - `Operations`: Point of Sale / Checkout (`Ctrl+O`), New Prescription (`Ctrl+K`).
   - `Help`: About PMS (`F1`).
2. **ToolStrip:**
   - ➕ New Med | 🛒 New Sale | 💊 Prescription | 💾 Export CSV | 🔄 Refresh (F5) | ⚠️ Low Stock | ⏳ Expiring | 📋 Show All
3. **ContextMenuStrip (`contextMenuGrid`):**
   - Attached to `dgvMedicines`:
     - ✏️ Edit Medicine Details (`F2`)
     - 🗑️ Delete Medicine (`Delete`)
     - Separator
     - ➕ Register New Medicine (`Ctrl+N`)
     - 🔄 Refresh (`F5`)
4. **Command Handlers:**
   - Implement `MenuFileExportCsv_Click`: Prompts user via `SaveFileDialog`, writes CSV formatted string.
   - Implement `MenuEditDelete_Click`: Prompts confirmation box `"Are you sure you want to delete '{Name}'?"`, deletes from `_medicineRepository`, refreshes grid + chart.
   - Implement `MenuFilterLowStock_Click`, `MenuFilterExpiring_Click`, `MenuFilterShowAll_Click`: updates active filter and status bar.

- [ ] **Step 1: Modify `MainForm.Designer.cs` to declare and layout expanded controls**
- [ ] **Step 2: Modify `MainForm.cs` to implement command handlers and key handling**
- [ ] **Step 3: Build `PharmacyApp.Presentation` and verify 0 errors, 0 warnings**
```bash
dotnet build PharmacyApp.Presentation\PharmacyApp.Presentation.csproj --no-restore
```
- [ ] **Step 4: Run unit tests**
```bash
dotnet test PharmacyApp.Tests\PharmacyApp.Tests.csproj --no-restore
```
Expected: 26/26 tests passing.
- [ ] **Step 5: Commit**
```bash
git add PharmacyApp.Presentation/MainForm.cs PharmacyApp.Presentation/MainForm.Designer.cs
git commit -m "feat(presentation): expand MenuStrip, ToolStrip, and ContextMenuStrip with 13 commands"
```

---

### Task 3.3: CSV Export Helper & Deletion Logic with TDD
**Skills Applied:** `tdd`

**Files:**
- Create: `PharmacyApp.Presentation/Helpers/CsvExporter.cs`
- Create: `PharmacyApp.Tests/Domain/CsvExporterTests.cs`

**Interfaces:**
- Produces: `CsvExporter.GenerateMedicineCsv(IEnumerable<Medicine> medicines)` returning CSV string.
- Consumed by: `MenuFileExportCsv_Click` in `MainForm.cs`.

- [ ] **Step 1: Write failing CSV export tests in `CsvExporterTests.cs`**
```csharp
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
            Assert.IsTrue(csv.StartsWith("MedicineId,Name,Category,Manufacturer,Price,StockQuantity,MinStockLevel,Unit,ExpiryDate"));
            var lines = csv.Trim().Split(Environment.NewLine);
            Assert.AreEqual(1, lines.Length);
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
            Assert.AreEqual(2, lines.Length);
            Assert.IsTrue(lines[1].Contains("Aspirin"));
            Assert.IsTrue(lines[1].Contains("MED-0001"));
        }
    }
}
```

- [ ] **Step 2: Run test to verify failure (Red Phase)**
- [ ] **Step 3: Implement `CsvExporter.cs`**
```csharp
// PharmacyApp.Presentation/Helpers/CsvExporter.cs
using System;
using System.Collections.Generic;
using System.Text;
using PharmacyApp.Domain.Entities;

namespace PharmacyApp.Presentation.Helpers
{
    public static class CsvExporter
    {
        public static string GenerateMedicineCsv(IEnumerable<Medicine> medicines)
        {
            var sb = new StringBuilder();
            sb.AppendLine("MedicineId,Name,Category,Manufacturer,Price,StockQuantity,MinStockLevel,Unit,ExpiryDate");

            foreach (var m in medicines)
            {
                // Escape commas if present in strings
                string safeName = m.Name.Contains(',') ? $"\"{m.Name}\"" : m.Name;
                string safeManufacturer = m.Manufacturer.Contains(',') ? $"\"{m.Manufacturer}\"" : m.Manufacturer;
                sb.AppendLine($"{m.MedicineId},{safeName},{m.Category},{safeManufacturer},{m.Price:F2},{m.StockQuantity},{m.MinStockLevel},{m.Unit},{m.ExpiryDate:yyyy-MM-dd}");
            }

            return sb.ToString();
        }
    }
}
```
- [ ] **Step 4: Run test to verify pass (Green Phase)**
- [ ] **Step 5: Wire `CsvExporter` inside `MainForm.MenuFileExportCsv_Click`**
- [ ] **Step 6: Build & Test**
```bash
dotnet test PharmacyApp.Tests\PharmacyApp.Tests.csproj --no-restore
```
Expected: 28/28 tests passing.
- [ ] **Step 7: Commit**
```bash
git add PharmacyApp.Presentation/Helpers/CsvExporter.cs PharmacyApp.Tests/Domain/CsvExporterTests.cs PharmacyApp.Presentation/MainForm.cs
git commit -m "feat(export): implement CsvExporter with unit tests and wire into File Export command"
```

---

### Task 3.4: Academic Report for Task 3 — P5 & M3 Criteria
**Skills Applied:** `writing-plans`

**Files:**
- Create: `docs/reports/Task3_Menus_Commands_Resources_Report.md`

**Content Requirements (Sections 14–17 + Criterion M3):**
- **Section 14: Menu Bar Hierarchy:**
  - Complete hierarchical tree diagram of all 5 top-level menus (`File`, `Edit`, `View`, `Operations`, `Help`), sub-items, access keys, shortcut notations, and visual separator lines.
- **Section 15: Toolbar Design & Iconography:**
  - Purpose of Toolbar strip, Fitts' law grouping, corresponding menu commands table.
- **Section 16: Command Specification Table:**
  - All 13 commands with Command ID, Name, Purpose, Action, Handler, Shortcut, and Toolbar status.
- **Section 17: Resource Identification & Allocation:**
  - Resource IDs (`IDR_*`, `IDD_*`, `IDA_*`, `IDS_*`, `IDB_*`), resource types, and functional purposes.
- **Criterion M3: Architectural & Usability Justifications:**
  - *M3.1 Justification of Menu Categories & Information Architecture:* Cognitive grouping, separation of safe viewing vs. mutative actions.
  - *M3.2 Justification of Command Redundancy:* Triple-path access pattern (Menu Bar + Toolbar + Keyboard Accelerators) accommodating novice, intermediate, and power users.
  - *M3.3 Justification of Keyboard Shortcuts:* Compliance with Microsoft Windows User Experience Interaction Guidelines (e.g., standard `Ctrl+N`, `Ctrl+S`, `Ctrl+F`, `F5`).
  - *M3.4 Justification of Context Menus:* Right-click efficiency on grid rows eliminating mouse travel distance to top menus.
  - *M3.5 Justification of Modal Dialogs & Resource IDs:* Architectural integrity, data isolation, and clean separation of concerns.

- [ ] **Step 1: Author `docs/reports/Task3_Menus_Commands_Resources_Report.md`**
- [ ] **Step 2: Commit**
```bash
git add docs/reports/Task3_Menus_Commands_Resources_Report.md
git commit -m "docs: complete Task 3 academic report covering P5 and M3 criteria"
```

---

## 🏁 Task 3 Verification Gate

Before concluding Task 3, verify:
- [ ] **P5 — Menu Bar:** 5 top-level menus with logically grouped items, access keys (`&`), shortcuts, and separators.
- [ ] **P5 — Toolbar:** Quick-access strip with visual icons/glyphs matching primary menu commands.
- [ ] **P5 — Context Menu:** Right-click context menu on `dgvMedicines` with Edit, Delete, Add, Refresh.
- [ ] **P5 — Commands Table:** $\ge 10$ commands (we have 13) with unique Command IDs, purposes, and handlers.
- [ ] **P5 — Shortcuts:** $\ge 5$ shortcuts (we have 12) correctly wired via `KeyPreview` in `MainForm`.
- [ ] **P5 — Resources Table:** Explicit Resource IDs (`IDR_*`, `IDD_*`, `IDA_*`, `IDS_*`, `IDB_*`).
- [ ] **M3 — Design Justification:** Full academic justifications for menus, shortcuts, toolbars, and resources.
- [ ] **TDD & Build:** `dotnet test` → 100% pass (28+ tests); `dotnet build` → 0 warnings, 0 errors.
- [ ] **Checklist & Tracker:** Update `AGENTS.md` (P5, M3 ✅) and `PROJECT_LOG.md`.
