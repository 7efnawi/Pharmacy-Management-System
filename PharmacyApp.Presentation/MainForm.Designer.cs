using System.Drawing;
using System.Windows.Forms;
using PharmacyApp.Presentation.Controls;

namespace PharmacyApp.Presentation
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // MenuStrip & Menus
        private MenuStrip menuStripMain;
        private ToolStripMenuItem menuFile;
        private ToolStripMenuItem menuFileNewMedicine;
        private ToolStripMenuItem menuFileExportCsv;
        private ToolStripMenuItem menuFileExit;

        private ToolStripMenuItem menuEdit;
        private ToolStripMenuItem menuEditMedicine;
        private ToolStripMenuItem menuEditDelete;
        private ToolStripMenuItem menuEditFind;

        private ToolStripMenuItem menuView;
        private ToolStripMenuItem menuViewRefresh;
        private ToolStripMenuItem menuViewFilterLow;
        private ToolStripMenuItem menuViewFilterExpiring;
        private ToolStripMenuItem menuViewFilterAll;

        private ToolStripMenuItem menuOperations;
        private ToolStripMenuItem menuOperationsNewSale;
        private ToolStripMenuItem menuOperationsPrescription;

        private ToolStripMenuItem menuHelp;
        private ToolStripMenuItem menuHelpAbout;

        // ToolStrip
        private ToolStrip toolStripMain;
        private ToolStripButton btnToolNewMedicine;
        private ToolStripButton btnToolNewSale;
        private ToolStripButton btnToolPrescription;
        private ToolStripButton btnToolExport;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton btnToolRefresh;
        private ToolStripButton btnToolFilterLow;
        private ToolStripButton btnToolFilterExpiring;
        private ToolStripButton btnToolFilterAll;

        // ContextMenuStrip for DataGridView
        private ContextMenuStrip contextMenuGrid;
        private ToolStripMenuItem menuContextEdit;
        private ToolStripMenuItem menuContextDelete;
        private ToolStripMenuItem menuContextNew;
        private ToolStripMenuItem menuContextRefresh;

        // Search, Grid, Chart & Status
        private Panel panelSearch;
        private Label lblSearch;
        private TextBox txtSearch;
        private DataGridView dgvMedicines;
        private StatusStrip statusStripMain;
        private ToolStripStatusLabel lblStatus;
        private ToolStripStatusLabel lblFilterState;
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
            this.menuFileExportCsv = new ToolStripMenuItem();
            this.menuFileExit = new ToolStripMenuItem();

            this.menuEdit = new ToolStripMenuItem();
            this.menuEditMedicine = new ToolStripMenuItem();
            this.menuEditDelete = new ToolStripMenuItem();
            this.menuEditFind = new ToolStripMenuItem();

            this.menuView = new ToolStripMenuItem();
            this.menuViewRefresh = new ToolStripMenuItem();
            this.menuViewFilterLow = new ToolStripMenuItem();
            this.menuViewFilterExpiring = new ToolStripMenuItem();
            this.menuViewFilterAll = new ToolStripMenuItem();

            this.menuOperations = new ToolStripMenuItem();
            this.menuOperationsNewSale = new ToolStripMenuItem();
            this.menuOperationsPrescription = new ToolStripMenuItem();

            this.menuHelp = new ToolStripMenuItem();
            this.menuHelpAbout = new ToolStripMenuItem();

            this.toolStripMain = new ToolStrip();
            this.btnToolNewMedicine = new ToolStripButton();
            this.btnToolNewSale = new ToolStripButton();
            this.btnToolPrescription = new ToolStripButton();
            this.btnToolExport = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.btnToolRefresh = new ToolStripButton();
            this.btnToolFilterLow = new ToolStripButton();
            this.btnToolFilterExpiring = new ToolStripButton();
            this.btnToolFilterAll = new ToolStripButton();

            this.contextMenuGrid = new ContextMenuStrip(this.components);
            this.menuContextEdit = new ToolStripMenuItem();
            this.menuContextDelete = new ToolStripMenuItem();
            this.menuContextNew = new ToolStripMenuItem();
            this.menuContextRefresh = new ToolStripMenuItem();

            this.panelSearch = new Panel();
            this.lblSearch = new Label();
            this.txtSearch = new TextBox();
            this.dgvMedicines = new DataGridView();
            this.statusStripMain = new StatusStrip();
            this.lblStatus = new ToolStripStatusLabel();
            this.lblFilterState = new ToolStripStatusLabel();
            this.lblAlertSummary = new ToolStripStatusLabel();
            this.tableLayoutMain = new TableLayoutPanel();
            this.stockChart = new StockChartControl();

            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new ToolStripItem[] {
                this.menuFile,
                this.menuEdit,
                this.menuView,
                this.menuOperations,
                this.menuHelp
            });
            this.menuStripMain.Location = new Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new Size(1020, 24);
            this.menuStripMain.TabIndex = 0;

            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new ToolStripItem[] {
                this.menuFileNewMedicine,
                this.menuFileExportCsv,
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
            // menuFileExportCsv
            // 
            this.menuFileExportCsv.Name = "menuFileExportCsv";
            this.menuFileExportCsv.ShortcutKeys = Keys.Control | Keys.S;
            this.menuFileExportCsv.Text = "&Export to CSV...";
            this.menuFileExportCsv.Click += new System.EventHandler(this.MenuFileExportCsv_Click);

            // 
            // menuFileExit
            // 
            this.menuFileExit.Name = "menuFileExit";
            this.menuFileExit.ShortcutKeys = Keys.Alt | Keys.F4;
            this.menuFileExit.Text = "E&xit";
            this.menuFileExit.Click += new System.EventHandler(this.MenuFileExit_Click);

            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange(new ToolStripItem[] {
                this.menuEditMedicine,
                this.menuEditDelete,
                new ToolStripSeparator(),
                this.menuEditFind
            });
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Text = "&Edit";

            // 
            // menuEditMedicine
            // 
            this.menuEditMedicine.Name = "menuEditMedicine";
            this.menuEditMedicine.ShortcutKeys = Keys.F2;
            this.menuEditMedicine.Text = "&Edit Selected...";
            this.menuEditMedicine.Click += new System.EventHandler(this.MenuEditMedicine_Click);

            // 
            // menuEditDelete
            // 
            this.menuEditDelete.Name = "menuEditDelete";
            this.menuEditDelete.ShortcutKeys = Keys.Delete;
            this.menuEditDelete.Text = "&Delete Selected";
            this.menuEditDelete.Click += new System.EventHandler(this.MenuEditDelete_Click);

            // 
            // menuEditFind
            // 
            this.menuEditFind.Name = "menuEditFind";
            this.menuEditFind.ShortcutKeys = Keys.Control | Keys.F;
            this.menuEditFind.Text = "&Find Medicine";
            this.menuEditFind.Click += new System.EventHandler(this.MenuEditFind_Click);

            // 
            // menuView
            // 
            this.menuView.DropDownItems.AddRange(new ToolStripItem[] {
                this.menuViewRefresh,
                new ToolStripSeparator(),
                this.menuViewFilterLow,
                this.menuViewFilterExpiring,
                this.menuViewFilterAll
            });
            this.menuView.Name = "menuView";
            this.menuView.Text = "&View";

            // 
            // menuViewRefresh
            // 
            this.menuViewRefresh.Name = "menuViewRefresh";
            this.menuViewRefresh.ShortcutKeys = Keys.F5;
            this.menuViewRefresh.Text = "&Refresh All";
            this.menuViewRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);

            // 
            // menuViewFilterLow
            // 
            this.menuViewFilterLow.Name = "menuViewFilterLow";
            this.menuViewFilterLow.ShortcutKeys = Keys.Control | Keys.L;
            this.menuViewFilterLow.Text = "Filter: &Low Stock Only";
            this.menuViewFilterLow.Click += new System.EventHandler(this.MenuFilterLowStock_Click);

            // 
            // menuViewFilterExpiring
            // 
            this.menuViewFilterExpiring.Name = "menuViewFilterExpiring";
            this.menuViewFilterExpiring.ShortcutKeys = Keys.Control | Keys.E;
            this.menuViewFilterExpiring.Text = "Filter: &Expiring Soon Only";
            this.menuViewFilterExpiring.Click += new System.EventHandler(this.MenuFilterExpiring_Click);

            // 
            // menuViewFilterAll
            // 
            this.menuViewFilterAll.Name = "menuViewFilterAll";
            this.menuViewFilterAll.ShortcutKeys = Keys.Control | Keys.A;
            this.menuViewFilterAll.Text = "Show &All Items";
            this.menuViewFilterAll.Click += new System.EventHandler(this.MenuFilterShowAll_Click);

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
            this.menuOperationsNewSale.Text = "Point of &Sale (POS)";
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
            this.menuHelpAbout.Text = "&About PMS";
            this.menuHelpAbout.Click += new System.EventHandler(this.MenuHelpAbout_Click);

            // 
            // toolStripMain
            // 
            this.toolStripMain.Items.AddRange(new ToolStripItem[] {
                this.btnToolNewMedicine,
                this.btnToolNewSale,
                this.btnToolPrescription,
                this.btnToolExport,
                this.toolStripSeparator1,
                this.btnToolRefresh,
                this.btnToolFilterLow,
                this.btnToolFilterExpiring,
                this.btnToolFilterAll
            });
            this.toolStripMain.Location = new Point(0, 24);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new Size(1020, 25);
            this.toolStripMain.TabIndex = 1;

            // 
            // btnToolNewMedicine
            // 
            this.btnToolNewMedicine.Name = "btnToolNewMedicine";
            this.btnToolNewMedicine.Text = "➕ New Med";
            this.btnToolNewMedicine.ToolTipText = "Register New Medicine (Ctrl+N)";
            this.btnToolNewMedicine.Click += new System.EventHandler(this.MenuFileNewMedicine_Click);

            // 
            // btnToolNewSale
            // 
            this.btnToolNewSale.Name = "btnToolNewSale";
            this.btnToolNewSale.Text = "🛒 New Sale";
            this.btnToolNewSale.ToolTipText = "Point of Sale Checkout (Ctrl+O)";
            this.btnToolNewSale.Click += new System.EventHandler(this.MenuOperationsNewSale_Click);

            // 
            // btnToolPrescription
            // 
            this.btnToolPrescription.Name = "btnToolPrescription";
            this.btnToolPrescription.Text = "💊 Prescription";
            this.btnToolPrescription.ToolTipText = "Archive Prescription (Ctrl+K)";
            this.btnToolPrescription.Click += new System.EventHandler(this.MenuOperationsPrescription_Click);

            // 
            // btnToolExport
            // 
            this.btnToolExport.Name = "btnToolExport";
            this.btnToolExport.Text = "💾 Export CSV";
            this.btnToolExport.ToolTipText = "Export Inventory to CSV (Ctrl+S)";
            this.btnToolExport.Click += new System.EventHandler(this.MenuFileExportCsv_Click);

            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 25);

            // 
            // btnToolRefresh
            // 
            this.btnToolRefresh.Name = "btnToolRefresh";
            this.btnToolRefresh.Text = "🔄 Refresh (F5)";
            this.btnToolRefresh.ToolTipText = "Refresh Data (F5)";
            this.btnToolRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);

            // 
            // btnToolFilterLow
            // 
            this.btnToolFilterLow.Name = "btnToolFilterLow";
            this.btnToolFilterLow.Text = "⚠️ Low Stock";
            this.btnToolFilterLow.ToolTipText = "Show Low Stock Items Only (Ctrl+L)";
            this.btnToolFilterLow.Click += new System.EventHandler(this.MenuFilterLowStock_Click);

            // 
            // btnToolFilterExpiring
            // 
            this.btnToolFilterExpiring.Name = "btnToolFilterExpiring";
            this.btnToolFilterExpiring.Text = "⏳ Expiring";
            this.btnToolFilterExpiring.ToolTipText = "Show Expiring Items Only (Ctrl+E)";
            this.btnToolFilterExpiring.Click += new System.EventHandler(this.MenuFilterExpiring_Click);

            // 
            // btnToolFilterAll
            // 
            this.btnToolFilterAll.Name = "btnToolFilterAll";
            this.btnToolFilterAll.Text = "📋 Show All";
            this.btnToolFilterAll.ToolTipText = "Clear Filters and Show All Items (Ctrl+A)";
            this.btnToolFilterAll.Click += new System.EventHandler(this.MenuFilterShowAll_Click);

            // 
            // contextMenuGrid
            // 
            this.contextMenuGrid.Items.AddRange(new ToolStripItem[] {
                this.menuContextEdit,
                this.menuContextDelete,
                new ToolStripSeparator(),
                this.menuContextNew,
                this.menuContextRefresh
            });
            this.contextMenuGrid.Name = "contextMenuGrid";
            this.contextMenuGrid.Size = new Size(205, 104);

            // 
            // menuContextEdit
            // 
            this.menuContextEdit.Name = "menuContextEdit";
            this.menuContextEdit.Text = "✏️ Edit Medicine Details (F2)";
            this.menuContextEdit.Click += new System.EventHandler(this.MenuEditMedicine_Click);

            // 
            // menuContextDelete
            // 
            this.menuContextDelete.Name = "menuContextDelete";
            this.menuContextDelete.Text = "🗑️ Delete Medicine (Delete)";
            this.menuContextDelete.Click += new System.EventHandler(this.MenuEditDelete_Click);

            // 
            // menuContextNew
            // 
            this.menuContextNew.Name = "menuContextNew";
            this.menuContextNew.Text = "➕ Register New Medicine";
            this.menuContextNew.Click += new System.EventHandler(this.MenuFileNewMedicine_Click);

            // 
            // menuContextRefresh
            // 
            this.menuContextRefresh.Name = "menuContextRefresh";
            this.menuContextRefresh.Text = "🔄 Refresh List";
            this.menuContextRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);

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
            this.dgvMedicines.ContextMenuStrip = this.contextMenuGrid;
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
                this.lblFilterState,
                this.lblAlertSummary
            });
            this.statusStripMain.Location = new Point(0, 578);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new Size(1020, 22);
            this.statusStripMain.TabIndex = 4;

            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(40, 17);
            this.lblStatus.Text = "Ready";

            // 
            // lblFilterState
            // 
            this.lblFilterState.Name = "lblFilterState";
            this.lblFilterState.Size = new Size(80, 17);
            this.lblFilterState.Text = "View: All Items";
            this.lblFilterState.ForeColor = Color.DarkSlateGray;

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
            this.ClientSize = new Size(1020, 600);
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
            this.contextMenuGrid.ResumeLayout(false);
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
