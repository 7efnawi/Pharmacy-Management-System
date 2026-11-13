using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PharmacyApp.Domain.Entities;
using PharmacyApp.Domain.Interfaces;
using PharmacyApp.Domain.Repositories;
using PharmacyApp.Presentation.Controls;
using PharmacyApp.Presentation.Forms;

namespace PharmacyApp.Presentation
{
    public partial class MainForm : Form
    {
        private readonly IMedicineRepository _medicineRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IPrescriptionRepository _prescriptionRepository;

        public MainForm()
        {
            InitializeComponent();
            _medicineRepository = new InMemoryMedicineRepository(seedSampleData: true);
            _saleRepository = new InMemorySaleRepository();
            _prescriptionRepository = new InMemoryPrescriptionRepository();
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            SetupGridColumns();
            RefreshData();
        }

        private void SetupGridColumns()
        {
            dgvMedicines.Columns.Clear();
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MedicineId", HeaderText = "ID", FillWeight = 40 });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Medicine Name", FillWeight = 120 });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Category", HeaderText = "Category", FillWeight = 70 });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Price", HeaderText = "Price ($)", FillWeight = 45 });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "StockQuantity", HeaderText = "In Stock", FillWeight = 45 });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MinStockLevel", HeaderText = "Min Level", FillWeight = 45 });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Unit", HeaderText = "Unit", FillWeight = 40 });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ExpiryDate", HeaderText = "Expiry Date", FillWeight = 65 });
        }

        public void RefreshData()
        {
            var query = txtSearch.Text.Trim();
            var medicines = _medicineRepository.Search(query).ToList();
            dgvMedicines.DataSource = medicines;

            // Update GDI+ stock chart with latest data
            stockChart.SetData(_medicineRepository.GetAll());

            // Highlight alerts in grid
            foreach (DataGridViewRow row in dgvMedicines.Rows)
            {
                if (row.DataBoundItem is Medicine med)
                {
                    if (med.IsLowStock)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 235);
                        row.DefaultCellStyle.ForeColor = Color.DarkRed;
                    }
                    else if (med.IsExpiringSoon)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 220);
                        row.DefaultCellStyle.ForeColor = Color.DarkGoldenrod;
                    }
                }
            }

            // Update status & alert summary
            var lowStockCount = _medicineRepository.GetLowStock().Count();
            var expiringCount = _medicineRepository.GetExpiringSoon(30).Count();

            lblStatus.Text = $"Total SKUs: {medicines.Count}";
            if (lowStockCount > 0 || expiringCount > 0)
            {
                lblAlertSummary.Text = $"⚠️ Alerts: {lowStockCount} Low Stock | {expiringCount} Expiring Soon";
                lblAlertSummary.ForeColor = Color.DarkRed;
            }
            else
            {
                lblAlertSummary.Text = "✅ All stock levels normal";
                lblAlertSummary.ForeColor = Color.DarkGreen;
            }
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            RefreshData();
        }

        private void DgvMedicines_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvMedicines.Rows[e.RowIndex].DataBoundItem is Medicine med)
            {
                using var form = new MedicineForm(_medicineRepository, med);
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshData();
                }
            }
        }

        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                txtSearch.Focus();
                txtSearch.SelectAll();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                RefreshData();
                e.Handled = true;
            }
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            RefreshData();
        }

        private void MenuFileNewMedicine_Click(object? sender, EventArgs e)
        {
            using var form = new MedicineForm(_medicineRepository);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                RefreshData();
            }
        }

        private void MenuOperationsNewSale_Click(object? sender, EventArgs e)
        {
            using var form = new SaleForm(_medicineRepository, _saleRepository);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                RefreshData();
            }
        }

        private void MenuOperationsPrescription_Click(object? sender, EventArgs e)
        {
            using var form = new PrescriptionForm(_medicineRepository, _prescriptionRepository);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                RefreshData();
            }
        }

        private void MenuHelpAbout_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                "Pharmacy Management System (PMS)\nWindows Programming I (ICT-D-415)\nAssignment 1 Prototype\n\nFeatures Event-Driven Architecture, Keyboard Accelerators, and Live Filtering.",
                "About PMS",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void MenuFileExit_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
