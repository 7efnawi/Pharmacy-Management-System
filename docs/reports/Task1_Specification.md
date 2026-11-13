# 📑 Task 1 Specification: Problem, Requirements, Events & Input
## Pharmacy Management System (PMS) | Windows Programming I (ICT-D-415)

---

## 1. Problem Description & Target Users (Criterion P1)

### 1.1 Context and Problem Statement
Local retail pharmacies handle hundreds of daily transactions, manage thousands of drug stock-keeping units (SKUs), and process critical medical prescriptions. Most independent pharmacies currently rely on fragmented spreadsheet entries or manual paper logs. This operational mode causes critical vulnerabilities:
1. **Dispensing Expired Medications:** Lack of automated batch and expiry tracking risks patient safety and regulatory penalties.
2. **Stockouts of Essential Drugs:** Absence of threshold alerts leads to unfulfilled prescriptions and lost revenue.
3. **Slow Dispensing and Long Queues:** Manual price lookups and manual bill calculation cause checkout bottlenecks.
4. **Prescription Traceability Loss:** Paper prescriptions are frequently misplaced or disconnected from stock records.

The **Pharmacy Management System (PMS)** is an interactive, event-driven Windows desktop application engineered to centralize inventory monitoring, streamline sales transactions, automate expiry/low-stock alerts, and securely record doctor prescriptions.

---

### 1.2 Target Users, Roles, and Responsibilities

| User Role | Primary Responsibilities | Main Application Tasks |
|-----------|--------------------------|------------------------|
| **Chief Pharmacist** | • Medicine catalog governance<br>• Clinical validation of prescribed medications<br>• Overseeing critical alerts | • Add/Edit medicine records<br>• Validate and archive doctor prescriptions<br>• Review expiring drug reports |
| **Cashier / Counter Staff** | • Rapid front-desk customer checkout<br>• Payment handling and receipt issuance<br>• Real-time stock inquiries | • Search medicine catalog by trade/generic name<br>• Add items to customer cart<br>• Process cash/card transactions |
| **Inventory Manager** | • Stock replenishment and auditing<br>• Monitoring minimum reorder levels<br>• Supplier consignment verification | • Monitor low-stock dashboard and chart<br>• Update inventory stock counts<br>• Generate inventory status summaries |

---

## 2. Functional Requirements Specification (Criterion P1)

The system defines six core functional requirements covering complete daily pharmacy operations. Each requirement establishes the exact trigger action, expected system response, and affected system data.

### 2.1 Functional Requirements Matrix

| Req ID | Requirement Title | Target User | User Action (Trigger) | Expected System Response | Information Affected |
|:------:|:------------------|:------------|:----------------------|:-------------------------|:---------------------|
| **FR1** | **Medicine Registration** | Pharmacist | Enters medicine details (Name, Category, Price, Stock, Expiry Date, Min Threshold) in `MedicineForm` and clicks **Save** (or presses `Ctrl+S`). | System validates all inputs; if valid, assigns unique `MedicineId` (`MED-xxx`), persists record to repository, displays success confirmation, and updates the inventory view. | • `Medicine` catalog repository<br>• Inventory data view<br>• Total catalog count |
| **FR2** | **Dynamic Medicine Search** | Cashier / Pharmacist | Types characters into search input box (`txtSearch`) or presses `Ctrl+F` and selects category filter from dropdown. | System intercepts `TextChanged` event, queries repository across Name and Category with debounced filter, and dynamically rebinds the matching items in the results grid. | • Current active search view<br>• Grid selection state |
| **FR3** | **Point-of-Sale Checkout** | Cashier | Selects medicine from catalog, enters sale quantity, clicks **Add to Cart**, then clicks **Checkout** (or presses `Ctrl+O`). | System checks stock availability, computes line subtotal (`Qty * Price`), updates cart running total, deducts quantity from inventory upon checkout, and generates sale record. | • Medicine `StockQuantity`<br>• `Sale` transaction log<br>• Cash register balance |
| **FR4** | **Stock & Expiry Alert Detection** | Pharmacist / Manager | Opens application or clicks **Refresh Alerts** button on main dashboard toolbar. | System scans all inventory records; flags items with `StockQuantity < MinStockLevel` as Low Stock, and items with `ExpiryDate <= Now + 30 Days` as Expiring Soon. Highlights them with visual indicators. | • System alert badges<br>• Color-coded inventory rows<br>• Stock status indicators |
| **FR5** | **Prescription Archival** | Pharmacist | Fills patient name, doctor name, issue date, prescribes medicines, and clicks **Save Prescription** (`Ctrl+K`). | System validates required patient/doctor fields and medicine items; generates unique `PrescriptionId` (`RX-xxx`), stores prescription record, and optionally links it to a new checkout transaction. | • `Prescription` repository<br>• Patient history log |
| **FR6** | **Medicine Detail Modification** | Manager / Pharmacist | Double-clicks an existing medicine row in the data grid, edits price, stock count, or reorder threshold, and clicks **Update**. | System checks data validation rules (e.g. Price > 0, Stock >= 0); updates the targeted medicine entity in memory, and triggers an event to refresh both the grid and the visual stock chart. | • Target `Medicine` record<br>• Stock Chart visualization |

---

### 2.2 Input Validation Rules for Functional Requirements
To prevent corrupted or illegal application state, strict validation rules guard all inputs:
1. **Medicine Name:** Mandatory, non-empty, trimmed, maximum length of 100 characters.
2. **Unit Price:** Positive decimal (`> 0.00`), formatted with 2 decimal places.
3. **Stock Quantity:** Non-negative integer (`>= 0`).
4. **Minimum Stock Threshold:** Non-negative integer (`>= 0`).
5. **Expiry Date:** Must be a future date (`> DateTime.Today`); past dates are strictly rejected.
6. **Sale Quantity:** Integer strictly greater than 0 (`> 0`) and less than or equal to current available stock (`<= StockQuantity`).
