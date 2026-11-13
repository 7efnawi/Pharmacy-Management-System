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

            if (cboCategory.SelectedIndex < 0 && string.IsNullOrWhiteSpace(cboCategory.Text))
            {
                ShowError("Please select a category.");
                cboCategory.Focus();
                return;
            }

            var (priceOk, priceErr) = InputValidator.ValidatePrice(txtPrice.Text);
            if (!priceOk) { ShowError(priceErr); txtPrice.Focus(); return; }

            var (stockOk, stockErr) = InputValidator.ValidateStock(txtStock.Text);
            if (!stockOk) { ShowError(stockErr); txtStock.Focus(); return; }

            var (minStockOk, minStockErr) = InputValidator.ValidateStock(txtMinStock.Text);
            if (!minStockOk) { ShowError("Min stock level must be a valid non-negative number."); txtMinStock.Focus(); return; }

            var (expiryOk, expiryErr) = InputValidator.ValidateExpiryDate(dtpExpiry.Value);
            if (!expiryOk) { ShowError(expiryErr); dtpExpiry.Focus(); return; }

            if (cboUnit.SelectedIndex < 0 && string.IsNullOrWhiteSpace(cboUnit.Text))
            {
                ShowError("Please select a unit.");
                cboUnit.Focus();
                return;
            }

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
