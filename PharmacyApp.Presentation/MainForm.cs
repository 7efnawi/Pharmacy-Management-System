using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PharmacyApp.Domain.Entities;
using PharmacyApp.Domain.Interfaces;
using PharmacyApp.Domain.Repositories;
using PharmacyApp.Presentation.Constants;
using PharmacyApp.Presentation.Controls;
using PharmacyApp.Presentation.Forms;
using PharmacyApp.Presentation.Helpers;

namespace PharmacyApp.Presentation
{
    public partial class MainForm : Form
    {
        private enum FilterMode { All, LowStock, Expiring }
        private FilterMode _currentFilter = FilterMode.All;

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
            var allMatching = _medicineRepository.Search(query);

            var displayed = _currentFilter switch
            {
                FilterMode.LowStock => allMatching.Where(m => m.IsLowStock).ToList(),
                FilterMode.Expiring => allMatching.Where(m => m.IsExpiringSoon).ToList(),
                _ => allMatching.ToList()
            };

            dgvMedicines.DataSource = displayed;

            // Update GDI+ stock chart with latest repository data
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

            lblStatus.Text = $"Total SKUs: {_medicineRepository.GetAll().Count()} | Shown: {displayed.Count}";
            lblFilterState.Text = _currentFilter switch
            {
                FilterMode.LowStock => "View: Low Stock Only",
                FilterMode.Expiring => "View: Expiring Soon",
                _ => "View: All Items"
            };

            // Update menu checkmarks for active view filter
            menuViewFilterLow.Checked = (_currentFilter == FilterMode.LowStock);
            menuViewFilterExpiring.Checked = (_currentFilter == FilterMode.Expiring);
            menuViewFilterAll.Checked = (_currentFilter == FilterMode.All);

            // Update command enabled/disabled states based on selection
            UpdateCommandStates();

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

        private void DgvMedicines_SelectionChanged(object? sender, EventArgs e)
        {
            UpdateCommandStates();
        }

        private void UpdateCommandStates()
        {
            bool hasSelection = dgvMedicines.CurrentRow != null && dgvMedicines.CurrentRow.Index >= 0;
            menuEditMedicine.Enabled = hasSelection;
            menuEditDelete.Enabled = hasSelection;
            menuContextEdit.Enabled = hasSelection;
            menuContextDelete.Enabled = hasSelection;
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
            // Ctrl+F -> Search
            if (e.Control && e.KeyCode == Keys.F)
            {
                MenuEditFind_Click(sender, e);
                e.Handled = true;
            }
            // F5 -> Refresh
            else if (e.KeyCode == Keys.F5)
            {
                BtnRefresh_Click(sender, e);
                e.Handled = true;
            }
            // F2 -> Edit Selected
            else if (e.KeyCode == Keys.F2)
            {
                MenuEditMedicine_Click(sender, e);
                e.Handled = true;
            }
            // Delete -> Delete Selected (when not inside search textbox)
            else if (e.KeyCode == Keys.Delete && !txtSearch.Focused)
            {
                MenuEditDelete_Click(sender, e);
                e.Handled = true;
            }
            // Ctrl+L -> Filter Low Stock
            else if (e.Control && e.KeyCode == Keys.L)
            {
                MenuFilterLowStock_Click(sender, e);
                e.Handled = true;
            }
            // Ctrl+E -> Filter Expiring Soon
            else if (e.Control && e.KeyCode == Keys.E)
            {
                MenuFilterExpiring_Click(sender, e);
                e.Handled = true;
            }
            // Ctrl+A -> Show All Items
            else if (e.Control && e.KeyCode == Keys.A && !txtSearch.Focused)
            {
                MenuFilterShowAll_Click(sender, e);
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

        private void MenuFileExportCsv_Click(object? sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                FileName = $"Pharmacy_Inventory_{DateTime.Now:yyyyMMdd_HHmm}.csv",
                Title = "Export Inventory to CSV"
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var csv = CsvExporter.GenerateMedicineCsv(_medicineRepository.GetAll());
                    File.WriteAllText(sfd.FileName, csv);
                    MessageBox.Show(
                        $"Inventory successfully exported to:\n{sfd.FileName}",
                        "Export Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to export CSV: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MenuEditMedicine_Click(object? sender, EventArgs e)
        {
            if (dgvMedicines.CurrentRow?.DataBoundItem is Medicine med)
            {
                using var form = new MedicineForm(_medicineRepository, med);
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshData();
                }
            }
            else
            {
                MessageBox.Show("Please select a medicine row to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MenuEditDelete_Click(object? sender, EventArgs e)
        {
            if (dgvMedicines.CurrentRow?.DataBoundItem is Medicine med)
            {
                var confirm = MessageBox.Show(
                    $"Are you sure you want to delete '{med.Name}' (ID: {med.MedicineId})?\nThis action cannot be undone.",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    _medicineRepository.Delete(med.MedicineId);
                    RefreshData();
                }
            }
            else
            {
                MessageBox.Show("Please select a medicine row to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MenuEditFind_Click(object? sender, EventArgs e)
        {
            txtSearch.Focus();
            txtSearch.SelectAll();
        }

        private void MenuFilterLowStock_Click(object? sender, EventArgs e)
        {
            _currentFilter = FilterMode.LowStock;
            RefreshData();
        }

        private void MenuFilterExpiring_Click(object? sender, EventArgs e)
        {
            _currentFilter = FilterMode.Expiring;
            RefreshData();
        }

        private void MenuFilterShowAll_Click(object? sender, EventArgs e)
        {
            _currentFilter = FilterMode.All;
            RefreshData();
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
