// PharmacyApp.Presentation/Forms/SaleForm.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PharmacyApp.Domain.Entities;
using PharmacyApp.Domain.Interfaces;
using PharmacyApp.Presentation.Helpers;

namespace PharmacyApp.Presentation.Forms
{
    public partial class SaleForm : Form
    {
        private readonly IMedicineRepository _medicineRepo;
        private readonly ISaleRepository _saleRepo;
        private readonly List<SaleItem> _cartItems = new();

        public SaleForm(IMedicineRepository medicineRepo, ISaleRepository saleRepo)
        {
            InitializeComponent();
            _medicineRepo = medicineRepo;
            _saleRepo = saleRepo;

            Text = "New Sale / Checkout";
            SetupGrids();
            LoadCatalog();
            UpdateCartView();
        }

        private void SetupGrids()
        {
            // dgvAvailable columns
            dgvAvailable.AutoGenerateColumns = false;
            dgvAvailable.Columns.Clear();
            dgvAvailable.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MedicineId", HeaderText = "ID", FillWeight = 30 });
            dgvAvailable.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Name", FillWeight = 120 });
            dgvAvailable.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Price", HeaderText = "Price ($)", FillWeight = 50, DefaultCellStyle = new DataGridViewCellStyle { Format = "F2" } });
            dgvAvailable.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "StockQuantity", HeaderText = "Stock", FillWeight = 45 });
            dgvAvailable.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Unit", HeaderText = "Unit", FillWeight = 40 });

            // dgvCart columns
            dgvCart.AutoGenerateColumns = false;
            dgvCart.Columns.Clear();
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MedicineName", HeaderText = "Item", FillWeight = 110 });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Quantity", HeaderText = "Qty", FillWeight = 40 });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "UnitPrice", HeaderText = "Price", FillWeight = 45, DefaultCellStyle = new DataGridViewCellStyle { Format = "F2" } });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Subtotal", HeaderText = "Subtotal ($)", FillWeight = 60, DefaultCellStyle = new DataGridViewCellStyle { Format = "F2" } });
        }

        private void LoadCatalog()
        {
            var query = txtSearch.Text.Trim();
            var list = _medicineRepo.Search(query).ToList();
            dgvAvailable.DataSource = list;
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            LoadCatalog();
        }

        private void DgvAvailable_SelectionChanged(object? sender, EventArgs e)
        {
            lblError.Visible = false;
        }

        private void BtnAddToCart_Click(object? sender, EventArgs e)
        {
            lblError.Visible = false;

            if (dgvAvailable.CurrentRow?.DataBoundItem is not Medicine med)
            {
                ShowError("Please select a medicine from the list first.");
                return;
            }

            int alreadyInCart = _cartItems
                .Where(i => i.MedicineId == med.MedicineId)
                .Sum(i => i.Quantity);

            int remainingStock = med.StockQuantity - alreadyInCart;

            var (ok, err) = InputValidator.ValidateSaleQuantity(txtQuantity.Text, remainingStock);
            if (!ok)
            {
                ShowError(err);
                txtQuantity.Focus();
                return;
            }

            int qty = int.Parse(txtQuantity.Text);

            var existingCartItem = _cartItems.FirstOrDefault(i => i.MedicineId == med.MedicineId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += qty;
            }
            else
            {
                _cartItems.Add(new SaleItem
                {
                    MedicineId = med.MedicineId,
                    MedicineName = med.Name,
                    Quantity = qty,
                    UnitPrice = med.Price
                });
            }

            txtQuantity.Text = "1";
            UpdateCartView();
        }

        private void BtnRemoveFromCart_Click(object? sender, EventArgs e)
        {
            lblError.Visible = false;

            if (dgvCart.CurrentRow?.DataBoundItem is not SaleItem selectedItem)
            {
                ShowError("Please select an item from the cart to remove.");
                return;
            }

            _cartItems.Remove(selectedItem);
            UpdateCartView();
        }

        private void UpdateCartView()
        {
            dgvCart.DataSource = null;
            dgvCart.DataSource = _cartItems.ToList();

            decimal total = _cartItems.Sum(i => i.Subtotal);
            lblTotal.Text = $"Total: ${total:F2}";
        }

        private void BtnCheckout_Click(object? sender, EventArgs e)
        {
            lblError.Visible = false;

            if (_cartItems.Count == 0)
            {
                ShowError("Cart is empty. Add at least one medicine before checkout.");
                return;
            }

            if (cboPayment.SelectedIndex < 0 && string.IsNullOrWhiteSpace(cboPayment.Text))
            {
                ShowError("Please select a payment method.");
                cboPayment.Focus();
                return;
            }

            // Verify stock once more before deducting
            foreach (var item in _cartItems)
            {
                var med = _medicineRepo.GetById(item.MedicineId);
                if (med == null || med.StockQuantity < item.Quantity)
                {
                    ShowError($"Insufficient stock for '{item.MedicineName}'.");
                    return;
                }
            }

            // Deduct stock
            foreach (var item in _cartItems)
            {
                var med = _medicineRepo.GetById(item.MedicineId)!;
                med.StockQuantity -= item.Quantity;
                _medicineRepo.Update(med);
            }

            // Persist Sale
            var sale = new Sale
            {
                SaleDate = DateTime.Now,
                PaymentMethod = cboPayment.Text,
                Items = new List<SaleItem>(_cartItems)
            };
            _saleRepo.Add(sale);

            MessageBox.Show(
                $"Sale completed successfully!\nReceipt #{sale.SaleId}\nItems: {sale.Items.Count}\nTotal: ${sale.TotalAmount:F2}\nPayment: {sale.PaymentMethod}",
                "Checkout Successful",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ShowError(string message)
        {
            lblError.Text = $"⚠ {message}";
            lblError.Visible = true;
        }
    }
}
