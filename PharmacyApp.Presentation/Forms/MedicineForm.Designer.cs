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
