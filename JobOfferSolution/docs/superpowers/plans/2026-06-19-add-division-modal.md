# Add Division Modal Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a polished, static Add Division selection mockup to the existing New User modal.

**Architecture:** Modify the `modalAddDivision` render fragment and add a private mock-record collection in the existing Razor page. Use Bootstrap/SBAdmin2 utility classes, a responsive filter row, a compact table rendered from mock data, and a non-wired footer action; add no state bindings or event handlers.

**Tech Stack:** Blazor Razor, Bootstrap 4/SBAdmin2, FontAwesome

---

### Task 1: Build the Add Division modal mockup

**Files:**
- Modify: `JO.BlazorApp/Components/Pages/Admin/User/NewUser.razor:204`

- [ ] **Step 1: Replace the empty modal content**

Replace the existing `modalAddDivision` block with markup containing:

- A compact SBAdmin2 card and instructional header with `fa-building`.
- A `0 selected` badge.
- A responsive Company dropdown defaulting to `All Companies` and a Division text input, with no bindings.
- A `table-responsive` wrapper around a bordered `table-sm` table.
- Columns for an empty checkbox, Company, Division Code, and Division Name.
- Six mock records spanning Apex Holdings, Northstar Group, and Meridian Industries, rendered with `foreach`; every checkbox remains unchecked.
- Company badges and monospace-style division-code badges for quick scanning.
- A primary footer button labeled `Add Selected Division/s` with `fa-plus` and no click handler.

Do not add `@bind`, `@onclick`, or change handlers because the requested result is presentational only.

- [ ] **Step 2: Review the scoped diff statically**

Run:

```powershell
git diff -- JO.BlazorApp/Components/Pages/Admin/User/NewUser.razor
```

Expected: only the `modalAddDivision` child content and footer template plus its local mock-data record/list change. Confirm responsive classes, all four requested columns, empty checkboxes, both filters, mock rows, badges, FontAwesome icons, and the non-wired footer action are present. Do not build or run the application.
