// PharmacyApp.Presentation/Constants/AppCommands.cs
using System.Collections.Generic;

namespace PharmacyApp.Presentation.Constants
{
    public record CommandDescriptor(
        string CommandId,
        string Name,
        string Category,
        string ShortcutText,
        string Description,
        string ResourceId);

    public static class AppCommands
    {
        // Command IDs
        public const string CmdNewMedicine       = "CMD_FILE_NEW_MEDICINE";
        public const string CmdExportCsv         = "CMD_FILE_EXPORT_CSV";
        public const string CmdExit              = "CMD_FILE_EXIT";
        public const string CmdEditMedicine      = "CMD_EDIT_MEDICINE";
        public const string CmdDeleteMedicine    = "CMD_EDIT_DELETE";
        public const string CmdSearch            = "CMD_VIEW_SEARCH";
        public const string CmdRefresh           = "CMD_VIEW_REFRESH";
        public const string CmdFilterLowStock    = "CMD_VIEW_FILTER_LOW";
        public const string CmdFilterExpiring    = "CMD_VIEW_FILTER_EXPIRY";
        public const string CmdFilterAll         = "CMD_VIEW_FILTER_ALL";
        public const string CmdNewSale           = "CMD_OP_NEW_SALE";
        public const string CmdPrescription      = "CMD_OP_PRESCRIPTION";
        public const string CmdAbout             = "CMD_HELP_ABOUT";

        // Resource IDs
        public const string IdrMenuMain          = "IDR_MENU_MAIN";
        public const string IdrMenuContextGrid   = "IDR_MENU_CONTEXT_GRID";
        public const string IdrToolbarMain       = "IDR_TOOLBAR_MAIN";
        public const string IddMedicineDialog    = "IDD_MEDICINE_DIALOG";
        public const string IddSaleDialog        = "IDD_SALE_DIALOG";
        public const string IddPrescriptionDialog= "IDD_PRESCRIPTION_DIALOG";
        public const string IddAboutDialog       = "IDD_ABOUT_DIALOG";
        public const string IdaMainAccelerators  = "IDA_MAIN_ACCELERATORS";

        private static readonly List<CommandDescriptor> _commands = new()
        {
            new(CmdNewMedicine,    "New Medicine",       "File",       "Ctrl+N", "Registers a new drug SKU into repository",            IddMedicineDialog),
            new(CmdExportCsv,      "Export to CSV",      "File",       "Ctrl+S", "Exports inventory snapshot to CSV file",              IdrMenuMain),
            new(CmdExit,           "Exit",               "File",       "Alt+F4", "Safely closes application",                           IdrMenuMain),
            new(CmdEditMedicine,   "Edit Selected",      "Edit",       "F2",     "Opens edit dialog for selected medicine",             IddMedicineDialog),
            new(CmdDeleteMedicine, "Delete Selected",    "Edit",       "Delete", "Removes selected medicine with confirmation",         IdrMenuContextGrid),
            new(CmdSearch,         "Find Medicine",      "Edit",       "Ctrl+F", "Shifts focus to live search input box",               IdrMenuMain),
            new(CmdRefresh,        "Refresh All",        "View",       "F5",     "Reloads grid and GDI+ stock chart from repository",   IdrToolbarMain),
            new(CmdFilterLowStock, "Filter: Low Stock",  "View",       "Ctrl+L", "Restricts view to medicines below minimum threshold", IdrToolbarMain),
            new(CmdFilterExpiring, "Filter: Expiring",   "View",       "Ctrl+E", "Restricts view to medicines expiring within 30 days", IdrToolbarMain),
            new(CmdFilterAll,      "Show All SKUs",      "View",       "Ctrl+A", "Clears filters and displays full inventory",          IdrToolbarMain),
            new(CmdNewSale,        "Point of Sale (POS)","Operations", "Ctrl+O", "Opens checkout register dialog",                      IddSaleDialog),
            new(CmdPrescription,   "New Prescription",   "Operations", "Ctrl+K", "Opens prescription archival and dispensing dialog",   IddPrescriptionDialog),
            new(CmdAbout,          "About PMS",          "Help",       "F1",     "Displays application version and academic credits",   IddAboutDialog)
        };

        public static IReadOnlyList<CommandDescriptor> GetAllCommands() => _commands;
    }
}
