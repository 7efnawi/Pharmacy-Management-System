using System.Drawing;
using System.Windows.Forms;
using PharmacyApp.Presentation.Controls;

namespace PharmacyApp.Presentation
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private MenuStrip menuStripMain;
        private ToolStripMenuItem menuFile;
        private ToolStripMenuItem menuFileNewMedicine;
        private ToolStripMenuItem menuFileExit;
        private ToolStripMenuItem menuOperations;
        private ToolStripMenuItem menuOperationsNewSale;
        private ToolStripMenuItem menuOperationsPrescription;
        private ToolStripMenuItem menuHelp;
        private ToolStripMenuItem menuHelpAbout;
        private ToolStrip toolStripMain;
        private ToolStripButton btnToolNewMedicine;
        private ToolStripButton btnToolNewSale;
        private ToolStripButton btnToolRefresh;
        private Panel panelSearch;
        private Label lblSearch;
        private TextBox txtSearch;
        private DataGridView dgvMedicines;
        private StatusStrip statusStripMain;
        private ToolStripStatusLabel lblStatus;
        private ToolStripStatusLabel lblAlertSummary;
        private TableLayoutPanel tableLayoutMain;
        private StockChartControl stockChart;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStripMain = new MenuStrip();
            this.menuFile = new ToolStripMenuItem();
            this.menuFileNewMedicine = new ToolStripMenuItem();
            this.menuFileExit = new ToolStripMenuItem();
            this.menuOperations = new ToolStripMenuItem();
            this.menuOperationsNewSale = new ToolStripMenuItem();
            this.menuOperationsPrescription = new ToolStripMenuItem();
            this.menuHelp = new ToolStripMenuItem();
            this.menuHelpAbout = new ToolStripMenuItem();
            this.toolStripMain = new ToolStrip();
            this.btnToolNewMedicine = new ToolStripButton();
            this.btnToolNewSale = new ToolStripButton();
            this.btnToolRefresh = new ToolStripButton();
            this.panelSearch = new Panel();
            this.lblSearch = new Label();
            this.txtSearch = new TextBox();
            this.dgvMedicines = new DataGridView();
            this.statusStripMain = new StatusStrip();
            this.lblStatus = new ToolStripStatusLabel();
            this.lblAlertSummary = new ToolStripStatusLabel();
            this.tableLayoutMain = new TableLayoutPanel();
            this.stockChart = new StockChartControl();

            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new ToolStripItem[] {
                this.menuFile,
                this.menuOperations,
                this.menuHelp
            });
            this.menuStripMain.Location = new Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new Size(980, 24);
            this.menuStripMain.TabIndex = 0;

            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new ToolStripItem[] {
                this.menuFileNewMedicine,
                new ToolStripSeparator(),
                this.menuFileExit
            });
            this.menuFile.Name = "menuFile";
            this.menuFile.Text = "&File";

            // 
            // menuFileNewMedicine
            // 
            this.menuFileNewMedicine.Name = "menuFileNewMedicine";
            this.menuFileNewMedicine.ShortcutKeys = Keys.Control | Keys.N;
            this.menuFileNewMedicine.Text = "&New Medicine";
            this.menuFileNewMedicine.Click += new System.EventHandler(this.MenuFileNewMedicine_Click);

            // 
            // menuFileExit
            // 
            this.menuFileExit.Name = "menuFileExit";
            this.menuFileExit.ShortcutKeys = Keys.Alt | Keys.F4;
            this.menuFileExit.Text = "E&xit";
            this.menuFileExit.Click += new System.EventHandler(this.MenuFileExit_Click);

            // 
            // menuOperations
            // 
            this.menuOperations.DropDownItems.AddRange(new ToolStripItem[] {
                this.menuOperationsNewSale,
                this.menuOperationsPrescription
            });
            this.menuOperations.Name = "menuOperations";
            this.menuOperations.Text = "&Operations";

            // 
            // menuOperationsNewSale
            // 
            this.menuOperationsNewSale.Name = "menuOperationsNewSale";
            this.menuOperationsNewSale.ShortcutKeys = Keys.Control | Keys.O;
            this.menuOperationsNewSale.Text = "New &Sale";
            this.menuOperationsNewSale.Click += new System.EventHandler(this.MenuOperationsNewSale_Click);

            // 
            // menuOperationsPrescription
            // 
            this.menuOperationsPrescription.Name = "menuOperationsPrescription";
            this.menuOperationsPrescription.ShortcutKeys = Keys.Control | Keys.K;
            this.menuOperationsPrescription.Text = "New &Prescription";
            this.menuOperationsPrescription.Click += new System.EventHandler(this.MenuOperationsPrescription_Click);

            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new ToolStripItem[] {
                this.menuHelpAbout
            });
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Text = "&Help";

            // 
            // menuHelpAbout
            // 
            this.menuHelpAbout.Name = "menuHelpAbout";
            this.menuHelpAbout.ShortcutKeys = Keys.F1;
            this.menuHelpAbout.Text = "&About";
            this.menuHelpAbout.Click += new System.EventHandler(this.MenuHelpAbout_Click);

            // 
            // toolStripMain
            // 
            this.toolStripMain.Items.AddRange(new ToolStripItem[] {
                this.btnToolNewMedicine,
                this.btnToolNewSale,
                this.btnToolRefresh
            });
            this.toolStripMain.Location = new Point(0, 24);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new Size(980, 25);
            this.toolStripMain.TabIndex = 1;

            // 
            // btnToolNewMedicine
            // 
            this.btnToolNewMedicine.Name = "btnToolNewMedicine";
            this.btnToolNewMedicine.Text = "➕ New Medicine";
            this.btnToolNewMedicine.Click += new System.EventHandler(this.MenuFileNewMedicine_Click);

            // 
            // btnToolNewSale
            // 
            this.btnToolNewSale.Name = "btnToolNewSale";
            this.btnToolNewSale.Text = "🛒 Checkout / Sale";
            this.btnToolNewSale.Click += new System.EventHandler(this.MenuOperationsNewSale_Click);

            // 
            // btnToolRefresh
            // 
            this.btnToolRefresh.Name = "btnToolRefresh";
            this.btnToolRefresh.Text = "🔄 Refresh (F5)";
            this.btnToolRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);

            // 
            // panelSearch
            // 
            this.panelSearch.Dock = DockStyle.Top;
            this.panelSearch.Height = 45;
            this.panelSearch.Padding = new Padding(10, 8, 10, 8);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.TabIndex = 2;

            // 
            // lblSearch
            // 
            this.lblSearch.Location = new Point(12, 12);
            this.lblSearch.Size = new Size(130, 20);
            this.lblSearch.Text = "🔍 Search (Ctrl+F):";
            this.lblSearch.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

            // 
            // txtSearch
            // 
            this.txtSearch.Location = new Point(150, 10);
            this.txtSearch.Size = new Size(320, 25);
            this.txtSearch.PlaceholderText = "Search by Medicine Name or Category...";
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.TxtSearch_TextChanged);

            // 
            // dgvMedicines
            // 
            this.dgvMedicines.Dock = DockStyle.Fill;
            this.dgvMedicines.ReadOnly = true;
            this.dgvMedicines.AllowUserToAddRows = false;
            this.dgvMedicines.AllowUserToDeleteRows = false;
            this.dgvMedicines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvMedicines.MultiSelect = false;
            this.dgvMedicines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMedicines.TabIndex = 3;
            this.dgvMedicines.CellDoubleClick += new DataGridViewCellEventHandler(this.DgvMedicines_CellDoubleClick);

            // 
            // stockChart
            // 
            this.stockChart.Dock = DockStyle.Fill;
            this.stockChart.BorderStyle = BorderStyle.FixedSingle;

            // 
            // tableLayoutMain — 2 rows: chart (180px fixed) + grid (fills rest)
            // 
            this.tableLayoutMain.Dock = DockStyle.Fill;
            this.tableLayoutMain.ColumnCount = 1;
            this.tableLayoutMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutMain.RowCount = 2;
            this.tableLayoutMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 180F));  // chart
            this.tableLayoutMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));   // grid
            this.tableLayoutMain.Controls.Add(this.stockChart, 0, 0);
            this.tableLayoutMain.Controls.Add(this.dgvMedicines, 0, 1);
            this.tableLayoutMain.TabIndex = 5;

            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new ToolStripItem[] {
                this.lblStatus,
                this.lblAlertSummary
            });
            this.statusStripMain.Location = new Point(0, 578);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new Size(980, 22);
            this.statusStripMain.TabIndex = 4;

            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(40, 17);
            this.lblStatus.Text = "Ready";

            // 
            // lblAlertSummary
            // 
            this.lblAlertSummary.Name = "lblAlertSummary";
            this.lblAlertSummary.Spring = true;
            this.lblAlertSummary.TextAlign = ContentAlignment.MiddleRight;
            this.lblAlertSummary.Text = "No alerts";

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(980, 600);
            this.Controls.Add(this.tableLayoutMain);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.menuStripMain);
            this.Controls.Add(this.statusStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Pharmacy Management System — Windows Programming I";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new KeyEventHandler(this.MainForm_KeyDown);

            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMedicines)).EndInit();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
