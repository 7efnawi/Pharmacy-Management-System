# Task 2: Graphics & Drawing; Forms & User Input — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use `tdd`, `ui-ux-pro-max`, and `writing-plans` to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Fulfill academic criteria P4 (at least one functional GDI+ graphical element that reacts to data) and P6 (at least three forms with suitable controls, control specification tables, and validation rules for at least five input fields) — all strictly within the Pharmacy Management System scope.

**Architecture:** A custom `StockChartControl` (UserControl) renders GDI+ bars from live repository data; the existing `MainForm` hosts it in a split panel. Three modal dialog forms (`MedicineForm`, `SaleForm`, `PrescriptionForm`) follow standard Windows dialog pattern — opened via `ShowDialog()`, wired to the existing repository layer, and return `DialogResult` to the parent.

**Tech Stack:** C# / .NET 10, Windows Forms, GDI+ (`System.Drawing`), MSTest, `InMemoryMedicineRepository`, `InMemorySaleRepository` (already implemented in Task 1).

**Spec:** `Windows_Programming_I_Assign 1.pdf` (Task 2: Sections 7–13; Criteria: P4, P6).

## Global Constraints
- Target Framework: `net10.0-windows` (Presentation), `net10.0` (Domain + Tests).
- `dotnet build` → 0 Errors, 0 Warnings at all times.
- `dotnet test` → 100% Pass Rate before every commit.
- Strictly implement only what Sections 7–13 of the assignment require — no extra features.
- Each commit covers exactly one sub-task.

---

## 🛠️ Required Skills Matrix for Task 2

| Skill Name | Source | Responsibility in Task 2 |
|------------|--------|--------------------------|
| `writing-plans` | `.agents/skills/writing-plans` | Plan decomposition, file map, zero placeholders. |
| `tdd` | `.agents/skills/tdd` | Test-first validation logic for form input rules. |
| `ui-ux-pro-max` | `.agents/skills/ui-ux-pro-max` | GDI+ chart visual design, Tab order, control sizing, color palette. |

---

## 📁 File Structure Map

```
PharmacyApp/
├── docs/
│   └── reports/
│       └── Task2_Graphics_and_Forms_Report.md   # Academic deliverable: P4 & P6
├── PharmacyApp.Domain/
│   └── (no changes — entities already in place)
├── PharmacyApp.Tests/
│   └── Domain/
│       └── ValidationTests.cs                   # TDD tests for validation helpers
├── PharmacyApp.Presentation/
│   ├── Controls/
│   │   └── StockChartControl.cs                 # GDI+ custom UserControl (P4)
│   └── Forms/
│       ├── MedicineForm.cs                       # Add / Edit medicine (P6)
│       ├── MedicineForm.Designer.cs
│       ├── SaleForm.cs                           # Point-of-sale checkout (P6)
│       ├── SaleForm.Designer.cs
│       ├── PrescriptionForm.cs                   # Prescription archival (P6)
│       └── PrescriptionForm.Designer.cs
│   └── MainForm.cs (modified — embed chart, wire form launches)
```

---

## Task Decomposition

---

### Task 2.1: GDI+ Stock Chart Control — `StockChartControl` (Criterion P4)
**Skills Applied:** `ui-ux-pro-max`

**Files:**
- Create: `PharmacyApp.Presentation/Controls/StockChartControl.cs`
- Modify: `PharmacyApp.Presentation/MainForm.cs` (embed chart in top split panel)
- Modify: `PharmacyApp.Presentation/MainForm.Designer.cs` (add `panelChart`, resize layout)

**Interfaces:**
- Consumes: `IEnumerable<Medicine>` from `IMedicineRepository.GetAll()`
- Produces: Visual GDI+ bar chart showing stock vs. minimum threshold per medicine. Redraws on `Invalidate()` triggered by any data change.

**Design decisions:**
- Bars colored by stock health: **Green** (above minimum), **Amber** (within 150% of minimum), **Red** (below minimum).
- Dashed horizontal line drawn at the minimum stock threshold of each medicine using the same X position.
- Medicine name labels drawn below each bar (rotated or truncated).
- Chart redraws automatically on `Paint` event — no polling or timers needed.

- [ ] **Step 1: Create `Controls/` directory and `StockChartControl.cs`**

```csharp
// PharmacyApp.Presentation/Controls/StockChartControl.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PharmacyApp.Domain.Entities;

namespace PharmacyApp.Presentation.Controls
{
    public class StockChartControl : UserControl
    {
        private List<Medicine> _medicines = new();
        private const int BarWidth = 40;
        private const int BarGap = 20;
        private const int PaddingLeft = 50;
        private const int PaddingBottom = 50;
        private const int PaddingTop = 20;

        public StockChartControl()
        {
            DoubleBuffered = true;  // Eliminates flicker on repaint
            ResizeRedraw = true;    // Redraws automatically when control resizes
        }

        /// <summary>
        /// Sets the data source for the chart and triggers a repaint.
        /// Call this whenever medicine data changes (after save, edit, or sale).
        /// </summary>
        public void SetData(IEnumerable<Medicine> medicines)
        {
            _medicines = medicines
                .OrderBy(m => m.StockQuantity)
                .Take(8)   // Show up to 8 medicines — prevents chart overflow
                .ToList();
            Invalidate();  // Triggers OnPaint → redraws chart with new data
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_medicines.Count == 0) return;

            var g = e.Graphics;
            g.Clear(Color.White);

            int maxStock = _medicines.Max(m => Math.Max(m.StockQuantity, m.MinStockLevel));
            if (maxStock == 0) maxStock = 1;

            int chartHeight = Height - PaddingBottom - PaddingTop;
            int chartWidth  = Width - PaddingLeft;

            // Draw chart title
            using var titleFont = new Font("Segoe UI", 10, FontStyle.Bold);
            g.DrawString("📊 Stock Level Overview", titleFont, Brushes.DimGray, PaddingLeft, 4);

            for (int i = 0; i < _medicines.Count; i++)
            {
                var med = _medicines[i];
                int x = PaddingLeft + i * (BarWidth + BarGap);
                if (x + BarWidth > Width - 10) break;  // Guard against overflow

                // Determine bar colour based on stock health
                Color barColor = med.StockQuantity >= med.MinStockLevel
                    ? (med.StockQuantity <= med.MinStockLevel * 1.5 ? Color.Orange : Color.SeaGreen)
                    : Color.Crimson;

                // Calculate bar height proportional to chart area
                int barHeight = (int)((double)med.StockQuantity / maxStock * chartHeight);
                int barTop = Height - PaddingBottom - barHeight;

                // Draw filled bar
                using var barBrush = new SolidBrush(barColor);
                g.FillRectangle(barBrush, x, barTop, BarWidth, barHeight);

                // Draw bar outline
                g.DrawRectangle(Pens.DimGray, x, barTop, BarWidth, barHeight);

                // Draw minimum threshold dashed line
                if (med.MinStockLevel > 0)
                {
                    int minY = Height - PaddingBottom - (int)((double)med.MinStockLevel / maxStock * chartHeight);
                    using var dashPen = new Pen(Color.Red, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
                    g.DrawLine(dashPen, x, minY, x + BarWidth, minY);
                }

                // Draw stock quantity label above bar
                using var valueFont = new Font("Segoe UI", 7.5F, FontStyle.Bold);
                string qtyLabel = med.StockQuantity.ToString();
                var labelSize = g.MeasureString(qtyLabel, valueFont);
                g.DrawString(qtyLabel, valueFont, Brushes.Black,
                    x + (BarWidth - labelSize.Width) / 2f,
                    Math.Max(barTop - labelSize.Height - 2, PaddingTop));

                // Draw medicine name below bar (truncated to 8 chars)
                using var nameFont = new Font("Segoe UI", 7F);
                string shortName = med.Name.Length > 9 ? med.Name[..9] + "…" : med.Name;
                g.DrawString(shortName, nameFont, Brushes.DimGray,
                    x, Height - PaddingBottom + 4);
            }

            // Draw Y-axis baseline
            g.DrawLine(Pens.DimGray,
                PaddingLeft, PaddingTop,
                PaddingLeft, Height - PaddingBottom);

            // Draw X-axis baseline
            g.DrawLine(Pens.DimGray,
                PaddingLeft, Height - PaddingBottom,
                Width - 10, Height - PaddingBottom);
        }
    }
}
```

- [ ] **Step 2: Embed `StockChartControl` inside `MainForm`**

Modify `MainForm.Designer.cs`:
- Change layout to `SplitContainer` or `TableLayoutPanel` with top row height 180px for chart, bottom row fills with `dgvMedicines`.
- Instantiate `StockChartControl` and dock it in the top panel.

Modify `MainForm.cs`:
- In `MainForm_Load` and `RefreshData()`, call `stockChart.SetData(_medicineRepository.GetAll())`.

- [ ] **Step 3: Verify build — 0 warnings**

```bash
dotnet build PharmacyApp.Presentation\PharmacyApp.Presentation.csproj --no-restore
```
Expected: `Build succeeded. 0 Warning(s), 0 Error(s)`.

- [ ] **Step 4: Commit**

```bash
git add PharmacyApp.Presentation/Controls/ PharmacyApp.Presentation/MainForm.cs PharmacyApp.Presentation/MainForm.Designer.cs
git commit -m "feat(presentation): implement GDI+ StockChartControl with dynamic bar colors and threshold line"
```

---

### Task 2.2: Input Validation Helper + TDD Tests (supporting P6)
**Skills Applied:** `tdd`

**Files:**
- Create: `PharmacyApp.Presentation/Helpers/InputValidator.cs`
- Create: `PharmacyApp.Tests/Domain/ValidationTests.cs`

**Interfaces:**
- Produces: `InputValidator.ValidateMedicine(...)` returning `(bool isValid, string errorMessage)`.
- Consumed by: `MedicineForm.cs`, `SaleForm.cs`, `PrescriptionForm.cs`.

> These are the 5+ validation rules required by assignment Section 13.

- [ ] **Step 1: Write failing validation tests in `ValidationTests.cs`**

```csharp
// PharmacyApp.Tests/Domain/ValidationTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PharmacyApp.Presentation.Helpers;
using System;

namespace PharmacyApp.Tests.Domain
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void ValidateName_EmptyString_ReturnsFalse()
        {
            var (isValid, msg) = InputValidator.ValidateName(string.Empty);
            Assert.IsFalse(isValid);
            Assert.IsFalse(string.IsNullOrWhiteSpace(msg));
        }

        [TestMethod]
        public void ValidateName_ValidString_ReturnsTrue()
        {
            var (isValid, _) = InputValidator.ValidateName("Paracetamol 500mg");
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidatePrice_Zero_ReturnsFalse()
        {
            var (isValid, msg) = InputValidator.ValidatePrice("0");
            Assert.IsFalse(isValid);
            Assert.IsFalse(string.IsNullOrWhiteSpace(msg));
        }

        [TestMethod]
        public void ValidatePrice_NegativeNumber_ReturnsFalse()
        {
            var (isValid, _) = InputValidator.ValidatePrice("-5.00");
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void ValidatePrice_ValidPositiveDecimal_ReturnsTrue()
        {
            var (isValid, _) = InputValidator.ValidatePrice("25.50");
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidateStock_NegativeInteger_ReturnsFalse()
        {
            var (isValid, _) = InputValidator.ValidateStock("-1");
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void ValidateStock_Zero_ReturnsTrue()
        {
            var (isValid, _) = InputValidator.ValidateStock("0");
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidateExpiryDate_PastDate_ReturnsFalse()
        {
            var (isValid, msg) = InputValidator.ValidateExpiryDate(DateTime.Today.AddDays(-1));
            Assert.IsFalse(isValid);
            Assert.IsFalse(string.IsNullOrWhiteSpace(msg));
        }

        [TestMethod]
        public void ValidateExpiryDate_FutureDate_ReturnsTrue()
        {
            var (isValid, _) = InputValidator.ValidateExpiryDate(DateTime.Today.AddMonths(6));
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidateSaleQuantity_ExceedsStock_ReturnsFalse()
        {
            var (isValid, msg) = InputValidator.ValidateSaleQuantity("150", 100);
            Assert.IsFalse(isValid);
            Assert.IsFalse(string.IsNullOrWhiteSpace(msg));
        }

        [TestMethod]
        public void ValidateSaleQuantity_ValidQuantity_ReturnsTrue()
        {
            var (isValid, _) = InputValidator.ValidateSaleQuantity("5", 100);
            Assert.IsTrue(isValid);
        }
    }
}
```

- [ ] **Step 2: Run tests to verify failure (Red Phase)**
```bash
dotnet test PharmacyApp.Tests\PharmacyApp.Tests.csproj --no-restore
```
Expected: FAIL (compilation error — `InputValidator` does not exist yet).

- [ ] **Step 3: Implement `InputValidator.cs`**

```csharp
// PharmacyApp.Presentation/Helpers/InputValidator.cs
using System;

namespace PharmacyApp.Presentation.Helpers
{
    public static class InputValidator
    {
        public static (bool isValid, string errorMessage) ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Medicine name is required and cannot be empty.");
            if (name.Trim().Length > 100)
                return (false, "Medicine name must not exceed 100 characters.");
            return (true, string.Empty);
        }

        public static (bool isValid, string errorMessage) ValidatePrice(string priceText)
        {
            if (!decimal.TryParse(priceText, out decimal price))
                return (false, "Price must be a valid number (e.g. 12.50).");
            if (price <= 0)
                return (false, "Price must be greater than zero.");
            return (true, string.Empty);
        }

        public static (bool isValid, string errorMessage) ValidateStock(string stockText)
        {
            if (!int.TryParse(stockText, out int stock))
                return (false, "Stock quantity must be a whole number (e.g. 50).");
            if (stock < 0)
                return (false, "Stock quantity cannot be negative.");
            return (true, string.Empty);
        }

        public static (bool isValid, string errorMessage) ValidateExpiryDate(DateTime date)
        {
            if (date <= DateTime.Today)
                return (false, "Expiry date must be a future date (after today).");
            return (true, string.Empty);
        }

        public static (bool isValid, string errorMessage) ValidateSaleQuantity(string qtyText, int availableStock)
        {
            if (!int.TryParse(qtyText, out int qty))
                return (false, "Quantity must be a whole number.");
            if (qty <= 0)
                return (false, "Quantity must be greater than zero.");
            if (qty > availableStock)
                return (false, $"Quantity ({qty}) exceeds available stock ({availableStock}).");
            return (true, string.Empty);
        }
    }
}
```

- [ ] **Step 4: Add `PharmacyApp.Presentation` as reference to `PharmacyApp.Tests`**

```bash
dotnet add PharmacyApp.Tests\PharmacyApp.Tests.csproj reference PharmacyApp.Presentation\PharmacyApp.Presentation.csproj
```

- [ ] **Step 5: Run tests to verify pass (Green Phase)**
```bash
dotnet test PharmacyApp.Tests\PharmacyApp.Tests.csproj --no-restore
```
Expected: `Passed! — Failed: 0, Passed: 22, Skipped: 0, Total: 22`

- [ ] **Step 6: Commit**
```bash
git add PharmacyApp.Presentation/Helpers/ PharmacyApp.Tests/Domain/ValidationTests.cs PharmacyApp.Tests/PharmacyApp.Tests.csproj
git commit -m "feat(validation): implement InputValidator with 5 validation rules and TDD tests"
```

---

### Task 2.3: `MedicineForm` — Add / Edit Medicine (Criterion P6)
**Skills Applied:** `ui-ux-pro-max`

**Files:**
- Create: `PharmacyApp.Presentation/Forms/MedicineForm.Designer.cs`
- Create: `PharmacyApp.Presentation/Forms/MedicineForm.cs`

**Interfaces:**
- Consumes: `Medicine?` (null = Add mode, non-null = Edit mode), `IMedicineRepository`
- Produces: Updated or new `Medicine` persisted to repository on `DialogResult.OK`.

#### Control Specification Table — MedicineForm

| Control Name | Type | Purpose | Input/Output | Validation Rule |
|---|---|---|---|---|
| `lblName` / `txtName` | Label + TextBox | Medicine trade/generic name | Input (string) | Required; max 100 chars |
| `lblCategory` / `cboCategory` | Label + ComboBox | Drug classification | Input (selection) | Required — must select an item |
| `lblManufacturer` / `txtManufacturer` | Label + TextBox | Manufacturer name | Input (string) | Required |
| `lblPrice` / `txtPrice` | Label + TextBox | Unit selling price | Input (decimal) | Must be a positive decimal > 0 |
| `lblStock` / `txtStock` | Label + TextBox | Current stock quantity | Input (integer) | Must be integer >= 0 |
| `lblMinStock` / `txtMinStock` | Label + TextBox | Low-stock alert threshold | Input (integer) | Must be integer >= 0 |
| `lblExpiry` / `dtpExpiry` | Label + DateTimePicker | Expiry date | Input (date) | Must be a future date |
| `lblUnit` / `cboUnit` | Label + ComboBox | Measurement unit | Input (selection) | Required — must select an item |
| `btnSave` | Button | Validate and persist record | Action | AcceptButton — fires on Enter |
| `btnCancel` | Button | Discard changes and close | Action | CancelButton — fires on Esc |
| `lblError` | Label | Inline error message display | Output (string) | Hidden when no error; shown in red |

- [ ] **Step 1: Create `Forms/` directory**
```bash
New-Item -ItemType Directory -Force -Path "PharmacyApp.Presentation/Forms"
```

- [ ] **Step 2: Implement `MedicineForm.Designer.cs`**

```csharp
// PharmacyApp.Presentation/Forms/MedicineForm.Designer.cs
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PharmacyApp.Presentation.Forms
{
    partial class MedicineForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle, lblName, lblCategory, lblManufacturer;
        private Label lblPrice, lblStock, lblMinStock, lblExpiry, lblUnit, lblError;
        private TextBox txtName, txtManufacturer, txtPrice, txtStock, txtMinStock;
        private ComboBox cboCategory, cboUnit;
        private DateTimePicker dtpExpiry;
        private Button btnSave, btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            // Title
            lblTitle = new Label { Text = "Medicine Details", Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(20, 15), Size = new Size(300, 25) };
            // Name
            lblName = new Label { Text = "Name*:", Location = new Point(20, 60), Size = new Size(100, 20) };
            txtName = new TextBox { Location = new Point(130, 57), Size = new Size(280, 23), PlaceholderText = "e.g. Paracetamol 500mg", MaxLength = 100, TabIndex = 0 };
            // Category
            lblCategory = new Label { Text = "Category*:", Location = new Point(20, 95), Size = new Size(100, 20) };
            cboCategory = new ComboBox { Location = new Point(130, 92), Size = new Size(180, 23), DropDownStyle = ComboBoxStyle.DropDownList, TabIndex = 1 };
            cboCategory.Items.AddRange(new object[] { "Analgesic", "Antibiotic", "Antihistamine", "Cardiovascular", "Dermatology", "Gastrointestinal", "NSAID", "Vitamin" });
            // Manufacturer
            lblManufacturer = new Label { Text = "Manufacturer*:", Location = new Point(20, 130), Size = new Size(105, 20) };
            txtManufacturer = new TextBox { Location = new Point(130, 127), Size = new Size(280, 23), TabIndex = 2 };
            // Price
            lblPrice = new Label { Text = "Price ($)*:", Location = new Point(20, 165), Size = new Size(100, 20) };
            txtPrice = new TextBox { Location = new Point(130, 162), Size = new Size(100, 23), TabIndex = 3 };
            txtPrice.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.') e.Handled = true; };
            // Stock
            lblStock = new Label { Text = "In Stock*:", Location = new Point(20, 200), Size = new Size(100, 20) };
            txtStock = new TextBox { Location = new Point(130, 197), Size = new Size(100, 23), TabIndex = 4 };
            txtStock.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };
            // Min Stock
            lblMinStock = new Label { Text = "Min Level*:", Location = new Point(20, 235), Size = new Size(100, 20) };
            txtMinStock = new TextBox { Location = new Point(130, 232), Size = new Size(100, 23), TabIndex = 5 };
            txtMinStock.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };
            // Expiry
            lblExpiry = new Label { Text = "Expiry Date*:", Location = new Point(20, 270), Size = new Size(100, 20) };
            dtpExpiry = new DateTimePicker { Location = new Point(130, 267), Size = new Size(180, 23), Format = DateTimePickerFormat.Short, MinDate = DateTime.Today.AddDays(1), TabIndex = 6 };
            // Unit
            lblUnit = new Label { Text = "Unit*:", Location = new Point(20, 305), Size = new Size(100, 20) };
            cboUnit = new ComboBox { Location = new Point(130, 302), Size = new Size(130, 23), DropDownStyle = ComboBoxStyle.DropDownList, TabIndex = 7 };
            cboUnit.Items.AddRange(new object[] { "Box", "Bottle", "Strip", "Tablet", "Capsule", "Vial", "Sachet" });
            // Error label
            lblError = new Label { Location = new Point(20, 340), Size = new Size(390, 20), ForeColor = Color.Crimson, Font = new Font("Segoe UI", 8.5F), Visible = false };
            // Buttons
            btnSave = new Button { Text = "&Save", Location = new Point(130, 370), Size = new Size(90, 30), BackColor = Color.SeaGreen, ForeColor = Color.White, TabIndex = 8 };
            btnSave.Click += new EventHandler(this.BtnSave_Click);
            btnCancel = new Button { Text = "&Cancel", Location = new Point(230, 370), Size = new Size(90, 30), TabIndex = 9 };
            btnCancel.Click += new EventHandler(this.BtnCancel_Click);

            this.ClientSize = new Size(440, 420);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
            this.Controls.AddRange(new Control[] {
                lblTitle, lblName, txtName, lblCategory, cboCategory,
                lblManufacturer, txtManufacturer, lblPrice, txtPrice,
                lblStock, txtStock, lblMinStock, txtMinStock,
                lblExpiry, dtpExpiry, lblUnit, cboUnit,
                lblError, btnSave, btnCancel
            });
        }
    }
}
```

- [ ] **Step 3: Implement `MedicineForm.cs`**

```csharp
// PharmacyApp.Presentation/Forms/MedicineForm.cs
using System;
using System.Windows.Forms;
using PharmacyApp.Domain.Entities;
using PharmacyApp.Domain.Interfaces;
using PharmacyApp.Presentation.Helpers;

namespace PharmacyApp.Presentation.Forms
{
    public partial class MedicineForm : Form
    {
        private readonly IMedicineRepository _repo;
        private readonly Medicine? _existing;  // null = Add mode; non-null = Edit mode

        public MedicineForm(IMedicineRepository repo, Medicine? existing = null)
        {
            InitializeComponent();
            _repo = repo;
            _existing = existing;
            Text = existing == null ? "New Medicine" : "Edit Medicine";

            if (existing != null)
            {
                txtName.Text         = existing.Name;
                cboCategory.Text     = existing.Category;
                txtManufacturer.Text = existing.Manufacturer;
                txtPrice.Text        = existing.Price.ToString("F2");
                txtStock.Text        = existing.StockQuantity.ToString();
                txtMinStock.Text     = existing.MinStockLevel.ToString();
                dtpExpiry.Value      = existing.ExpiryDate > DateTime.Today ? existing.ExpiryDate : DateTime.Today.AddMonths(1);
                cboUnit.Text         = existing.Unit;
            }
            else
            {
                dtpExpiry.Value = DateTime.Today.AddMonths(12);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            lblError.Visible = false;

            var (nameOk, nameErr) = InputValidator.ValidateName(txtName.Text);
            if (!nameOk) { ShowError(nameErr); txtName.Focus(); return; }

            if (cboCategory.SelectedIndex < 0) { ShowError("Please select a category."); cboCategory.Focus(); return; }

            var (priceOk, priceErr) = InputValidator.ValidatePrice(txtPrice.Text);
            if (!priceOk) { ShowError(priceErr); txtPrice.Focus(); return; }

            var (stockOk, stockErr) = InputValidator.ValidateStock(txtStock.Text);
            if (!stockOk) { ShowError(stockErr); txtStock.Focus(); return; }

            var (expiryOk, expiryErr) = InputValidator.ValidateExpiryDate(dtpExpiry.Value);
            if (!expiryOk) { ShowError(expiryErr); dtpExpiry.Focus(); return; }

            if (cboUnit.SelectedIndex < 0) { ShowError("Please select a unit."); cboUnit.Focus(); return; }

            if (_existing == null)
            {
                _repo.Add(new Medicine
                {
                    Name           = txtName.Text.Trim(),
                    Category       = cboCategory.Text,
                    Manufacturer   = txtManufacturer.Text.Trim(),
                    Price          = decimal.Parse(txtPrice.Text),
                    StockQuantity  = int.Parse(txtStock.Text),
                    MinStockLevel  = int.Parse(txtMinStock.Text),
                    ExpiryDate     = dtpExpiry.Value,
                    Unit           = cboUnit.Text
                });
            }
            else
            {
                _existing.Name          = txtName.Text.Trim();
                _existing.Category      = cboCategory.Text;
                _existing.Manufacturer  = txtManufacturer.Text.Trim();
                _existing.Price         = decimal.Parse(txtPrice.Text);
                _existing.StockQuantity = int.Parse(txtStock.Text);
                _existing.MinStockLevel = int.Parse(txtMinStock.Text);
                _existing.ExpiryDate    = dtpExpiry.Value;
                _existing.Unit          = cboUnit.Text;
                _repo.Update(_existing);
            }

            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ShowError(string message)
        {
            lblError.Text    = $"⚠ {message}";
            lblError.Visible = true;
        }
    }
}
```

- [ ] **Step 4: Wire `MedicineForm` launch from `MainForm`**

Replace the placeholder `MessageBox.Show(...)` stub in `MenuFileNewMedicine_Click` and `DgvMedicines_CellDoubleClick`:
```csharp
private void MenuFileNewMedicine_Click(object? sender, EventArgs e)
{
    using var form = new MedicineForm(_medicineRepository);
    if (form.ShowDialog() == DialogResult.OK) RefreshData();
}
// In DgvMedicines_CellDoubleClick: open MedicineForm in Edit mode
using var form = new MedicineForm(_medicineRepository, med);
if (form.ShowDialog() == DialogResult.OK) RefreshData();
```

- [ ] **Step 5: Build and verify 0 warnings**
```bash
dotnet build PharmacyApp.Presentation\PharmacyApp.Presentation.csproj --no-restore
```

- [ ] **Step 6: Commit**
```bash
git add PharmacyApp.Presentation/Forms/MedicineForm.cs PharmacyApp.Presentation/Forms/MedicineForm.Designer.cs PharmacyApp.Presentation/MainForm.cs
git commit -m "feat(presentation): implement MedicineForm with 5 validation rules and Add/Edit modes"
```

---

### Task 2.4: `SaleForm` — Point-of-Sale Checkout (Criterion P6)
**Skills Applied:** `ui-ux-pro-max`

**Files:**
- Create: `PharmacyApp.Presentation/Forms/SaleForm.Designer.cs`
- Create: `PharmacyApp.Presentation/Forms/SaleForm.cs`

**Interfaces:**
- Consumes: `IMedicineRepository`, `ISaleRepository`
- Produces: Completed `Sale` persisted to `ISaleRepository`; stock deducted from `IMedicineRepository` atomically.

#### Control Specification Table — SaleForm

| Control Name | Type | Purpose | Input/Output | Validation Rule |
|---|---|---|---|---|
| `txtSearch` | TextBox | Live-filter medicine catalog | Input (string) | None — empty shows all |
| `dgvAvailable` | DataGridView | Shows searchable medicine catalog | Output (list) | Read-only; row selection triggers detail fill |
| `txtQuantity` | TextBox | Quantity to add to cart | Input (integer) | Must be integer 1 ≤ qty ≤ stock |
| `dgvCart` | DataGridView | Active sale items (shopping cart) | Output (list) | Read-only |
| `lblTotal` | Label | Running total (sum of cart) | Output (currency) | Auto-computed — never editable |
| `cboPayment` | ComboBox | Cash or Card payment selection | Input (selection) | Required before checkout |
| `btnAddToCart` | Button | Adds selected item+qty to cart | Action | Stock must be sufficient |
| `btnRemoveFromCart` | Button | Removes selected cart row | Action | At least one item must be selected |
| `btnCheckout` | Button | Finalizes sale, deducts stock | Action | Cart must not be empty; payment must be selected |
| `btnCancel` | Button | Closes form discarding all changes | Action | No stock deducted if cancelled |

- [ ] **Step 1: Implement `SaleForm.Designer.cs` and `SaleForm.cs`**

Exact same pattern as MedicineForm — constructor receives repos; `btnCheckout_Click` validates cart, iterates items, deducts stock, creates `Sale`, persists to `ISaleRepository`, closes with `DialogResult.OK`.

```csharp
// Key logic in SaleForm.cs:
private void BtnAddToCart_Click(object? sender, EventArgs e)
{
    if (dgvAvailable.CurrentRow?.DataBoundItem is not Medicine med) return;
    var (ok, err) = InputValidator.ValidateSaleQuantity(txtQuantity.Text, med.StockQuantity);
    if (!ok) { ShowError(err); txtQuantity.Focus(); return; }
    int qty = int.Parse(txtQuantity.Text);
    _cartItems.Add(new SaleItem { MedicineId = med.MedicineId, MedicineName = med.Name, Quantity = qty, UnitPrice = med.Price });
    dgvCart.DataSource = null;
    dgvCart.DataSource = _cartItems;
    lblTotal.Text = $"Total: ${_cartItems.Sum(i => i.Subtotal):F2}";
}

private void BtnCheckout_Click(object? sender, EventArgs e)
{
    if (_cartItems.Count == 0) { ShowError("Cart is empty. Add at least one item."); return; }
    if (cboPayment.SelectedIndex < 0) { ShowError("Please select a payment method."); return; }
    foreach (var item in _cartItems)
    {
        var med = _medicineRepo.GetById(item.MedicineId)!;
        med.StockQuantity -= item.Quantity;
        _medicineRepo.Update(med);
    }
    _saleRepo.Add(new Sale { PaymentMethod = cboPayment.Text, Items = new(_cartItems) });
    DialogResult = DialogResult.OK;
}
```

- [ ] **Step 2: Wire `SaleForm` launch from `MainForm.MenuOperationsNewSale_Click`**

```csharp
private void MenuOperationsNewSale_Click(object? sender, EventArgs e)
{
    using var form = new SaleForm(_medicineRepository, _saleRepository);
    if (form.ShowDialog() == DialogResult.OK) RefreshData();
}
```

- [ ] **Step 3: Build and verify 0 warnings**

- [ ] **Step 4: Commit**
```bash
git add PharmacyApp.Presentation/Forms/SaleForm.cs PharmacyApp.Presentation/Forms/SaleForm.Designer.cs PharmacyApp.Presentation/MainForm.cs
git commit -m "feat(presentation): implement SaleForm with cart management, stock deduction and checkout"
```

---

### Task 2.5: `PrescriptionForm` — Prescription Archival (Criterion P6)
**Skills Applied:** `ui-ux-pro-max`

**Files:**
- Create: `PharmacyApp.Presentation/Forms/PrescriptionForm.Designer.cs`
- Create: `PharmacyApp.Presentation/Forms/PrescriptionForm.cs`

**Interfaces:**
- Consumes: `IMedicineRepository`, `IPrescriptionRepository`
- Produces: Completed `Prescription` persisted to `IPrescriptionRepository`.

#### Control Specification Table — PrescriptionForm

| Control Name | Type | Purpose | Input/Output | Validation Rule |
|---|---|---|---|---|
| `txtPatientName` | TextBox | Patient full name | Input (string) | Required; non-empty |
| `txtDoctorName` | TextBox | Prescribing doctor name | Input (string) | Required; non-empty |
| `dtpDate` | DateTimePicker | Prescription issue date | Input (date) | Required; not in future |
| `dgvItems` | DataGridView | Prescribed medicine items | Output (list) | Must have at least 1 item |
| `txtNotes` | RichTextBox | Doctor notes / instructions | Input (string) | Optional |
| `btnAddItem` | Button | Add selected medicine to prescription | Action | Medicine must be selected |
| `btnSavePrescription` | Button | Persist prescription record | Action | AcceptButton; requires patient, doctor, and ≥1 item |
| `btnCancel` | Button | Discard and close | Action | CancelButton |

- [ ] **Step 1: Implement `PrescriptionForm.Designer.cs` and `PrescriptionForm.cs`**

Same structural pattern as MedicineForm and SaleForm. `btnSavePrescription_Click` validates: `txtPatientName` non-empty, `txtDoctorName` non-empty, `dgvItems` has at least one row, then creates and persists `Prescription`.

- [ ] **Step 2: Wire `PrescriptionForm` launch from `MainForm.MenuOperationsPrescription_Click`**

```csharp
private void MenuOperationsPrescription_Click(object? sender, EventArgs e)
{
    using var form = new PrescriptionForm(_medicineRepository, _prescriptionRepository);
    if (form.ShowDialog() == DialogResult.OK) RefreshData();
}
```

- [ ] **Step 3: Add `IPrescriptionRepository` / `InMemoryPrescriptionRepository` to `MainForm` (inject alongside medicine and sale repos)**

- [ ] **Step 4: Build and verify 0 warnings**

- [ ] **Step 5: Commit**
```bash
git add PharmacyApp.Presentation/Forms/PrescriptionForm.cs PharmacyApp.Presentation/Forms/PrescriptionForm.Designer.cs PharmacyApp.Presentation/MainForm.cs
git commit -m "feat(presentation): implement PrescriptionForm with patient, doctor and item validation"
```

---

### Task 2.6: Academic Documentation — P4 & P6 Report
**Skills Applied:** `writing-plans`

**Files:**
- Create: `docs/reports/Task2_Graphics_and_Forms_Report.md`

**Content required:**
- Section 7–9 (P4): Graphical element purpose, design decisions, annotated mock-up description of chart in three states: all healthy / some low / some expiring.
- Section 10–12 (P6): Three-form summary with control specification tables (already defined in plan above).
- Section 13 (Validation): Five invalid input examples per rule, error message wording, and prevention mechanism (KeyPress suppression, `lblError` display, `AcceptButton` guard).

- [ ] **Step 1: Draft `Task2_Graphics_and_Forms_Report.md` with all required sections**

- [ ] **Step 2: Commit**
```bash
git add docs/reports/Task2_Graphics_and_Forms_Report.md
git commit -m "docs: complete Task 2 report covering P4 and P6 criteria"
```

---

## 🏁 Task 2 Verification Gate

Before concluding Task 2, verify all deliverables:
- [ ] **P4:** `StockChartControl` renders live bar chart with green/amber/red bars and dashed threshold line; repaints on data change.
- [ ] **P6 — Forms:** `MedicineForm`, `SaleForm`, `PrescriptionForm` open from menu, accept keyboard shortcuts, support Enter/Esc, use `ShowDialog()`, and return `DialogResult`.
- [ ] **P6 — Control Specs:** Each form has a completed control specification table in the report.
- [ ] **P6 — Validation:** 5+ input fields have explicit validation; errors shown inline via `lblError`; numeric fields guard non-numeric keys via `KeyPress`.
- [ ] **TDD Gate:** `dotnet test` → 100% pass rate (22+ tests).
- [ ] **Build Gate:** `dotnet build` → 0 warnings, 0 errors.
- [ ] **Log Updated:** `PROJECT_LOG.md` updated with all new commits.
