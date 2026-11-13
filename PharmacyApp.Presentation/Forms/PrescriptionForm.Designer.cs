// PharmacyApp.Presentation/Forms/PrescriptionForm.Designer.cs
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PharmacyApp.Presentation.Forms
{
    partial class PrescriptionForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Label lblPatient;
        private TextBox txtPatientName;
        private Label lblDoctor;
        private TextBox txtDoctorName;
        private Label lblDate;
        private DateTimePicker dtpDate;

        private Label lblItemSelect;
        private ComboBox cboMedicines;
        private Label lblQuantity;
        private TextBox txtQuantity;
        private Button btnAddItem;
        private Button btnRemoveItem;
        private DataGridView dgvItems;

        private Label lblNotes;
        private TextBox txtNotes;

        private Label lblError;
        private Button btnSave;
        private Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            lblTitle = new Label
            {
                Text = "Prescription Archival & Dispensing",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(20, 15),
                Size = new Size(350, 25)
            };

            // Patient Name
            lblPatient = new Label { Text = "Patient Name*:", Location = new Point(20, 55), Size = new Size(110, 20) };
            txtPatientName = new TextBox { Location = new Point(135, 52), Size = new Size(250, 23), PlaceholderText = "e.g. John Doe", TabIndex = 0 };

            // Doctor Name
            lblDoctor = new Label { Text = "Doctor Name*:", Location = new Point(410, 55), Size = new Size(100, 20) };
            txtDoctorName = new TextBox { Location = new Point(515, 52), Size = new Size(220, 23), PlaceholderText = "e.g. Dr. Robert Smith", TabIndex = 1 };

            // Prescription Date
            lblDate = new Label { Text = "Date*:", Location = new Point(20, 90), Size = new Size(110, 20) };
            dtpDate = new DateTimePicker
            {
                Location = new Point(135, 87),
                Size = new Size(160, 23),
                Format = DateTimePickerFormat.Short,
                MaxDate = DateTime.Today,
                TabIndex = 2
            };

            // Medicine selection row
            lblItemSelect = new Label { Text = "Medicine*:", Location = new Point(20, 128), Size = new Size(80, 20) };
            cboMedicines = new ComboBox
            {
                Location = new Point(135, 125),
                Size = new Size(250, 23),
                DropDownStyle = ComboBoxStyle.DropDownList,
                TabIndex = 3
            };

            lblQuantity = new Label { Text = "Qty*:", Location = new Point(400, 128), Size = new Size(40, 20) };
            txtQuantity = new TextBox
            {
                Location = new Point(445, 125),
                Size = new Size(60, 23),
                Text = "1",
                TabIndex = 4
            };
            txtQuantity.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };

            btnAddItem = new Button
            {
                Text = "➕ Add Item",
                Location = new Point(520, 123),
                Size = new Size(100, 27),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                TabIndex = 5
            };
            btnAddItem.Click += new EventHandler(this.BtnAddItem_Click);

            btnRemoveItem = new Button
            {
                Text = "❌ Remove",
                Location = new Point(630, 123),
                Size = new Size(105, 27),
                TabIndex = 6
            };
            btnRemoveItem.Click += new EventHandler(this.BtnRemoveItem_Click);

            // Prescribed items grid
            dgvItems = new DataGridView
            {
                Location = new Point(20, 160),
                Size = new Size(715, 170),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                TabIndex = 7
            };

            // Notes
            lblNotes = new Label { Text = "Instructions / Notes:", Location = new Point(20, 342), Size = new Size(130, 20) };
            txtNotes = new TextBox
            {
                Location = new Point(155, 340),
                Size = new Size(580, 45),
                Multiline = true,
                PlaceholderText = "Dosage instructions, contraindications, or refills...",
                TabIndex = 8
            };

            // Error Label
            lblError = new Label
            {
                Location = new Point(20, 395),
                Size = new Size(715, 20),
                ForeColor = Color.Crimson,
                Font = new Font("Segoe UI", 8.5F),
                Visible = false
            };

            // Buttons
            btnSave = new Button
            {
                Text = "✔ Save Prescription",
                Location = new Point(475, 425),
                Size = new Size(150, 35),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                TabIndex = 9
            };
            btnSave.Click += new EventHandler(this.BtnSave_Click);

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(635, 425),
                Size = new Size(100, 35),
                TabIndex = 10
            };
            btnCancel.Click += new EventHandler(this.BtnCancel_Click);

            this.ClientSize = new Size(760, 480);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;

            this.Controls.AddRange(new Control[] {
                lblTitle,
                lblPatient, txtPatientName,
                lblDoctor, txtDoctorName,
                lblDate, dtpDate,
                lblItemSelect, cboMedicines, lblQuantity, txtQuantity, btnAddItem, btnRemoveItem,
                dgvItems,
                lblNotes, txtNotes,
                lblError, btnSave, btnCancel
            });
        }
    }
}
