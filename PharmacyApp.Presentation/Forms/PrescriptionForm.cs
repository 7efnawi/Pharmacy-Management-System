// PharmacyApp.Presentation/Forms/PrescriptionForm.cs
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
    public partial class PrescriptionForm : Form
    {
        private readonly IMedicineRepository _medicineRepo;
        private readonly IPrescriptionRepository _prescriptionRepo;
        private readonly List<SaleItem> _items = new();
        private List<Medicine> _medicineList = new();

        public PrescriptionForm(IMedicineRepository medicineRepo, IPrescriptionRepository prescriptionRepo)
        {
            InitializeComponent();
            _medicineRepo = medicineRepo;
            _prescriptionRepo = prescriptionRepo;

            Text = "New Prescription Record";
            SetupGrid();
            LoadMedicines();
            UpdateItemsGrid();
        }

        private void SetupGrid()
        {
            dgvItems.AutoGenerateColumns = false;
            dgvItems.Columns.Clear();
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MedicineId", HeaderText = "ID", FillWeight = 30 });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MedicineName", HeaderText = "Medicine Name", FillWeight = 140 });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Quantity", HeaderText = "Quantity", FillWeight = 40 });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "UnitPrice", HeaderText = "Unit Price ($)", FillWeight = 50, DefaultCellStyle = new DataGridViewCellStyle { Format = "F2" } });
        }

        private void LoadMedicines()
        {
            _medicineList = _medicineRepo.GetAll().OrderBy(m => m.Name).ToList();
            cboMedicines.DataSource = _medicineList;
            cboMedicines.DisplayMember = "Name";
            cboMedicines.ValueMember = "MedicineId";
        }

        private void BtnAddItem_Click(object? sender, EventArgs e)
        {
            lblError.Visible = false;

            if (cboMedicines.SelectedItem is not Medicine med)
            {
                ShowError("Please select a medicine.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int qty) || qty <= 0)
            {
                ShowError("Quantity must be a positive whole number.");
                txtQuantity.Focus();
                return;
            }

            var existing = _items.FirstOrDefault(i => i.MedicineId == med.MedicineId);
            if (existing != null)
            {
                existing.Quantity += qty;
            }
            else
            {
                _items.Add(new SaleItem
                {
                    MedicineId = med.MedicineId,
                    MedicineName = med.Name,
                    Quantity = qty,
                    UnitPrice = med.Price
                });
            }

            txtQuantity.Text = "1";
            UpdateItemsGrid();
        }

        private void BtnRemoveItem_Click(object? sender, EventArgs e)
        {
            lblError.Visible = false;

            if (dgvItems.CurrentRow?.DataBoundItem is not SaleItem selected)
            {
                ShowError("Please select an item from the list to remove.");
                return;
            }

            _items.Remove(selected);
            UpdateItemsGrid();
        }

        private void UpdateItemsGrid()
        {
            dgvItems.DataSource = null;
            dgvItems.DataSource = _items.ToList();
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            lblError.Visible = false;

            var patientName = txtPatientName.Text.Trim();
            if (string.IsNullOrWhiteSpace(patientName))
            {
                ShowError("Patient name is required.");
                txtPatientName.Focus();
                return;
            }
            if (patientName.Length > 100)
            {
                ShowError("Patient name must not exceed 100 characters.");
                txtPatientName.Focus();
                return;
            }

            var doctorName = txtDoctorName.Text.Trim();
            if (string.IsNullOrWhiteSpace(doctorName))
            {
                ShowError("Doctor name is required.");
                txtDoctorName.Focus();
                return;
            }
            if (doctorName.Length > 100)
            {
                ShowError("Doctor name must not exceed 100 characters.");
                txtDoctorName.Focus();
                return;
            }

            if (dtpDate.Value.Date > DateTime.Today)
            {
                ShowError("Prescription date cannot be in the future.");
                dtpDate.Focus();
                return;
            }

            if (_items.Count == 0)
            {
                ShowError("At least one prescribed medicine item must be added.");
                cboMedicines.Focus();
                return;
            }

            var prescription = new Prescription
            {
                PatientName = patientName,
                DoctorName = doctorName,
                Date = dtpDate.Value.Date,
                Notes = txtNotes.Text.Trim(),
                Items = new List<SaleItem>(_items)
            };

            _prescriptionRepo.Add(prescription);

            MessageBox.Show(
                $"Prescription archived successfully!\nRef: {prescription.PrescriptionId}\nPatient: {prescription.PatientName}\nDoctor: {prescription.DoctorName}\nItems: {prescription.Items.Count}",
                "Prescription Saved",
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
