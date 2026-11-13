// PharmacyApp.Presentation/Forms/SaleForm.Designer.cs
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PharmacyApp.Presentation.Forms
{
    partial class SaleForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Label lblAvailableHeader;
        private TextBox txtSearch;
        private DataGridView dgvAvailable;
        private Label lblQuantity;
        private TextBox txtQuantity;
        private Button btnAddToCart;

        private Label lblCartHeader;
        private DataGridView dgvCart;
        private Button btnRemoveFromCart;
        private Label lblPayment;
        private ComboBox cboPayment;
        private Label lblTotal;
        private Label lblError;
        private Button btnCheckout;
        private Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Form Title
            lblTitle = new Label
            {
                Text = "Point of Sale (POS) — New Sale",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(20, 12),
                Size = new Size(400, 25)
            };

            // Left Section: Available Catalog
            lblAvailableHeader = new Label
            {
                Text = "1. Select Medicine:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Location = new Point(20, 45),
                Size = new Size(180, 20)
            };

            txtSearch = new TextBox
            {
                Location = new Point(20, 70),
                Size = new Size(380, 23),
                PlaceholderText = "Search medicine by name or category...",
                TabIndex = 0
            };
            txtSearch.TextChanged += new EventHandler(this.TxtSearch_TextChanged);

            dgvAvailable = new DataGridView
            {
                Location = new Point(20, 100),
                Size = new Size(380, 260),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                TabIndex = 1
            };
            dgvAvailable.SelectionChanged += new EventHandler(this.DgvAvailable_SelectionChanged);

            lblQuantity = new Label
            {
                Text = "Quantity:",
                Location = new Point(20, 373),
                Size = new Size(60, 20)
            };

            txtQuantity = new TextBox
            {
                Location = new Point(85, 370),
                Size = new Size(70, 23),
                Text = "1",
                TabIndex = 2
            };
            txtQuantity.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };

            btnAddToCart = new Button
            {
                Text = "➕ Add to Cart",
                Location = new Point(165, 368),
                Size = new Size(110, 27),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                TabIndex = 3
            };
            btnAddToCart.Click += new EventHandler(this.BtnAddToCart_Click);

            // Right Section: Cart & Checkout
            lblCartHeader = new Label
            {
                Text = "2. Active Cart:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Location = new Point(420, 45),
                Size = new Size(180, 20)
            };

            dgvCart = new DataGridView
            {
                Location = new Point(420, 70),
                Size = new Size(380, 220),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                TabIndex = 4
            };

            btnRemoveFromCart = new Button
            {
                Text = "❌ Remove Item",
                Location = new Point(420, 300),
                Size = new Size(120, 26),
                TabIndex = 5
            };
            btnRemoveFromCart.Click += new EventHandler(this.BtnRemoveFromCart_Click);

            lblPayment = new Label
            {
                Text = "Payment Method*:",
                Location = new Point(420, 340),
                Size = new Size(115, 20)
            };

            cboPayment = new ComboBox
            {
                Location = new Point(540, 337),
                Size = new Size(160, 23),
                DropDownStyle = ComboBoxStyle.DropDownList,
                TabIndex = 6
            };
            cboPayment.Items.AddRange(new object[] { "Cash", "Credit Card", "Debit Card", "Insurance" });
            cboPayment.SelectedIndex = 0;

            lblTotal = new Label
            {
                Text = "Total: $0.00",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(420, 375),
                Size = new Size(280, 25)
            };

            // Error Label
            lblError = new Label
            {
                Location = new Point(20, 415),
                Size = new Size(780, 20),
                ForeColor = Color.Crimson,
                Font = new Font("Segoe UI", 8.5F),
                Visible = false
            };

            // Buttons
            btnCheckout = new Button
            {
                Text = "✔ Complete Sale",
                Location = new Point(540, 445),
                Size = new Size(130, 35),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                TabIndex = 7
            };
            btnCheckout.Click += new EventHandler(this.BtnCheckout_Click);

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(680, 445),
                Size = new Size(100, 35),
                TabIndex = 8
            };
            btnCancel.Click += new EventHandler(this.BtnCancel_Click);

            this.ClientSize = new Size(820, 495);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.AcceptButton = btnCheckout;
            this.CancelButton = btnCancel;

            this.Controls.AddRange(new Control[] {
                lblTitle,
                lblAvailableHeader, txtSearch, dgvAvailable, lblQuantity, txtQuantity, btnAddToCart,
                lblCartHeader, dgvCart, btnRemoveFromCart, lblPayment, cboPayment, lblTotal,
                lblError, btnCheckout, btnCancel
            });
        }
    }
}
