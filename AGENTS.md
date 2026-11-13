# 🏛️ AGENTS.md — Agent Constitution
## Pharmacy Management System | C# WinForms | Windows Programming I

> **Auto-loaded by Antigravity on every turn. These rules are non-negotiable.**

---

## 🎯 Project Identity

| Field | Value |
|-------|-------|
| **Project** | Pharmacy Management System |
| **Course** | Windows Programming I (ICT-D-415) |
| **Tech Stack** | C# · WinForms · .NET · GDI+ |
| **IDE** | Visual Studio |
| **Test Runner** | `dotnet test` |
| **Linter/Analyzer** | `dotnet build` (0 errors, 0 warnings) |
| **Deadline** | 17 November 2026 |

---

## 📌 The 5 Golden Rules (المبادئ الخمسة الحاكمة)

### Rule 1 — Strict Incremental Execution (العمل التدريجي غير المجمّع)
- **Zero Code Dumps:** لا تكتب كود من طبقات متعددة في خطوة واحدة.
- **Bite-Sized Sub-Tasks:** كل مهمة لا تأخذ أكثر من 5–15 دقيقة.
- **Strict Layer Sequence:**
  ```
  Domain (Entities + Interfaces)
      ↓
  Data (Models + Repositories)
      ↓
  Services (Business Logic)
      ↓
  Presentation (Forms + Controls + Events)
  ```

### Rule 2 — Verification & TDD First (التحقق واختبارات TDD أولاً)
- **Red-Green-Refactor:** اكتب الـ Test أولاً → تأكد من فشله (Red) → اكتب الكود → تأكد من نجاحه (Green) → Refactor.
- **Verification Gates:** لا تنهي أي Task حتى:
  - `dotnet test` → **100% Pass Rate**
  - `dotnet build` → **0 errors, 0 warnings**

### Rule 3 — Mandatory Technical Walkthrough (الشرح المعماري الإلزامي)
- بعد كل Sub-Task ناجح، اكتب تقرير معماري يشمل:
  - **"The Why before the How":** لماذا هذا القرار المعماري؟
  - **Code Anatomy:** شرح كل سطر مهم (inputs, outputs, dependencies).
  - **Design Patterns & SOLID:** Single Responsibility, Dependency Inversion.
  - **Edge Cases:** Null handling, validation, error states.

### Rule 4 — Clean Architecture Integrity (فصل المسؤوليات)
```
PharmacyApp/
├── Domain/          # Pure C# — Entities, Interfaces, Business Rules
│   ├── Entities/    # Medicine, Sale, Prescription, Supplier
│   └── Interfaces/  # IRepository contracts
├── Data/            # Data access — Repositories, (In-Memory or SQLite)
│   ├── Repositories/
│   └── InMemory/
├── Services/        # Business Logic — Use Cases, Validation
└── Presentation/    # WinForms — Forms, Controls, Event Handlers
    ├── Forms/
    ├── Controls/
    └── Resources/   # Icons, Menus, Toolbars, Accelerators
```

### Rule 5 — Atomic Semantic Commits (الـ Commits الذرية)
- كل Sub-Task ناجح = Commit فوري.
- Semantic prefixes:
  - `feat:` → ميزة جديدة
  - `test:` → اختبار جديد
  - `fix:` → إصلاح خطأ
  - `refactor:` → إعادة هيكلة بدون تغيير سلوك
  - `docs:` → تحديث ملفات التوثيق

---

## 🚫 Hard Constraints (قواعد لا تُكسر)

1. لا تنتقل من Layer لـ Layer قبل اكتمال الاختبارات.
2. لا تعمل أكثر من Entity أو Form واحد في نفس الـ commit.
3. لا تبدأ Presentation قبل اكتمال Domain وData.
4. كل Form لازم يكون ليه Control Specification Table في PROJECT.md.
5. كل Command لازم يكون ليه Command ID وKeyboard Shortcut محدد.

---

## 📋 Assignment Coverage Checklist

| Criterion | Status | Task |
|-----------|--------|------|
| P1 – Problem & Requirements | ✅ Done | Task 1 |
| P2 – Events & Event Handling | ✅ Done | Task 1 |
| P3 – Keyboard, Mouse & Input | ✅ Done | Task 1 |
| P4 – Graphics & Drawing | ✅ Done | Task 2 |
| P5 – Menus, Commands & Resources | ⬜ | Task 3 |
| P6 – Forms & Controls | ✅ Done | Task 2 |
| P7 – Integrate Components | ⬜ | Task 4 |
| P8 – Application Behavior | ⬜ | Task 4 |
| M1 – Analyze Event Flow | ✅ Done | Task 1 |
| M3 – Justify Design Decisions | ⬜ | Task 3 |
| D1 – Usability Evaluation | ⬜ | Task 4 |
| D2 – Evaluate Integration | ⬜ | Task 4 |
