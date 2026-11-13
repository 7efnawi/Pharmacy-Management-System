# 📊 PROJECT_LOG.md — Real-Time Task Tracker
## Pharmacy Management System | C# WinForms

> **Update this file after EVERY completed sub-task. Never skip.**

---

## 📍 Current Status

| Field | Value |
|-------|-------|
| **Active Phase** | Phase 1 — Domain Layer |
| **Last Updated** | 2026-11-13 |
| **Last Commit** | Pending (Sub-Task 1.1) |
| **Next Task** | Phase 1.2 — Define Sale & SaleItem Entities (TDD) |
| **Blockers** | None |

---

## ✅ Completed Tasks

| # | Date | Phase | Task | Commit | Tests |
|---|------|-------|------|--------|-------|
| 1 | 2026-11-13 | Phase 0 | Create AGENTS.md | ab30266 | N/A |
| 2 | 2026-11-13 | Phase 0 | Create PROJECT.md | ab30266 | N/A |
| 3 | 2026-11-13 | Phase 0 | Create PROJECT_LOG.md | ab30266 | N/A |
| 4 | 2026-11-13 | Phase 0 | Create docs/plans/ structure | ab30266 | N/A |
| 5 | 2026-11-13 | Phase 0 | Scaffold Solution & 3 Projects | ab30266 | 1/1 Pass |
| 6 | 2026-11-13 | Phase 1 | Sub-Task 1.1: Medicine Entity + Rules | Pending | 4/4 Pass (100%) |

---

## 🔄 Phase Progress

### Phase 0 — Scaffolding ✅
- [x] AGENTS.md created
- [x] PROJECT.md created
- [x] PROJECT_LOG.md created
- [x] docs/plans/ directory created
- [x] Phase 1 plan written

### Phase 1 — Domain Layer 🔄
- [x] 1.1 Define `Medicine` entity + unit tests
- [ ] 1.2 Define `Sale` + `SaleItem` entities + unit tests
- [ ] 1.3 Define `Prescription` entity + unit tests
- [ ] 1.4 Define `IMedicineRepository` interface
- [ ] 1.5 Define `ISaleRepository` interface
- [ ] 1.6 Define `IPrescriptionRepository` interface

### Phase 2 — Data Layer ⬜
- [ ] 2.1 Implement `InMemoryMedicineRepository` + tests
- [ ] 2.2 Implement `InMemorySaleRepository` + tests
- [ ] 2.3 Implement `InMemoryPrescriptionRepository` + tests

### Phase 3 — Services Layer ⬜
- [ ] 3.1 Implement `MedicineService` + tests
- [ ] 3.2 Implement `SaleService` + tests
- [ ] 3.3 Implement `PrescriptionService` + tests

### Phase 4 — MainForm + Chart ⬜
- [ ] 4.1 Create WinForms project structure
- [ ] 4.2 Build `MainForm` shell (MenuStrip, ToolStrip, StatusStrip)
- [ ] 4.3 Build `StockChartControl` (GDI+ Bar Chart)
- [ ] 4.4 Wire events: Resize, Refresh, Load

### Phase 5 — MedicineForm ⬜
- [ ] 5.1 Design MedicineForm controls
- [ ] 5.2 Wire Save/Cancel events + validation
- [ ] 5.3 Connect to MedicineService

### Phase 6 — SaleForm ⬜
- [ ] 6.1 Design SaleForm controls
- [ ] 6.2 Cart logic + running total
- [ ] 6.3 Checkout + stock deduction

### Phase 7 — PrescriptionForm + SearchForm ⬜
- [ ] 7.1 Build PrescriptionForm
- [ ] 7.2 Build SearchForm

### Phase 8 — Menus, Commands, Resources ⬜
- [ ] 8.1 Define all 12 commands with IDs and shortcuts
- [ ] 8.2 Build MenuStrip with all categories
- [ ] 8.3 Build ToolStrip with icons
- [ ] 8.4 Wire keyboard shortcuts (KeyPreview)

### Phase 9 — Integration & Evaluation ⬜
- [ ] 9.1 Integrate all forms into MainForm
- [ ] 9.2 Document 5 user scenarios
- [ ] 9.3 Conduct usability evaluation (5 users)
- [ ] 9.4 Identify 3 issues + propose improvements

---

## 📝 Session Log

### Session 1 — 2026-11-13
**Start:** Kickoff & Planning
**Done:**
- Read Assignment PDF
- Selected Pharmacy Management System
- Confirmed C# WinForms stack
- Applied Engineering Playbook
- Created all scaffold files
- Written Phase 1 TDD Plan

**Next Session Start Point:**
> Begin Phase 1.1 — Create `PharmacyApp` Visual Studio Solution + define `Medicine` entity with unit tests.
