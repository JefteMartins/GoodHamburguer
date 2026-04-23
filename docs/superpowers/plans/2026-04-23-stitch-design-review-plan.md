# Stitch Design Review Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:requesting-code-review to execute this review plan with a dedicated reviewer context. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Review the Blazor frontend screen by screen and component by component against the Stitch project `9060755920845036151` to verify styling, spacing, consistency, and adaptation quality.

**Architecture:** This is a review-first plan, not an implementation plan. The reviewer should compare the current frontend shell, shared tokens, individual pages, and all visible UI states against the Stitch design system and generated screens, then produce findings grouped by severity with file and route references.

**Tech Stack:** Blazor Server, CSS isolation, shared `wwwroot/app.css` tokens, Docker Compose runtime, Stitch project `9060755920845036151`, Superpowers `requesting-code-review`.

---

## Review Scope

**Stitch project:** `9060755920845036151`

**Stitch design system direction to enforce:**
- Dark premium / industrial visual language
- Typography: `Space Grotesk` for headings, `Work Sans` for body, `Inter` for labels
- Palette anchored in charcoal surfaces with crimson `#BE1E2D` and golden accent `#FFB800`
- Tonal depth instead of loud shadows
- Tight but consistent 8px spacing rhythm
- Sidebar-driven dashboard shell
- Inputs, cards, buttons, and labels with consistent radii and border treatment

**Screens to validate against Stitch output:**
- `Product Menu`
- `New Order`
- `Order Details`
- `Orders Dashboard`

**Routes to inspect in app runtime:**
- `/`
- `/menu`
- `/orders/new`
- `/orders/{id}`
- `/dashboard/orders`

**Primary frontend files in scope:**
- `src/GoodHamburguer.Blazor/wwwroot/app.css`
- `src/GoodHamburguer.Blazor/Components/Layout/MainLayout.razor`
- `src/GoodHamburguer.Blazor/Components/Layout/MainLayout.razor.css`
- `src/GoodHamburguer.Blazor/Components/Layout/NavMenu.razor`
- `src/GoodHamburguer.Blazor/Components/Layout/NavMenu.razor.css`
- `src/GoodHamburguer.Blazor/Components/Layout/ReconnectModal.razor`
- `src/GoodHamburguer.Blazor/Components/Layout/ReconnectModal.razor.css`
- `src/GoodHamburguer.Blazor/Components/Pages/Home.razor`
- `src/GoodHamburguer.Blazor/Components/Pages/Menu.razor`
- `src/GoodHamburguer.Blazor/Components/Pages/Menu.razor.css`
- `src/GoodHamburguer.Blazor/Components/Pages/NewOrder.razor`
- `src/GoodHamburguer.Blazor/Components/Pages/NewOrder.razor.css`
- `src/GoodHamburguer.Blazor/Components/Pages/OrderDetails.razor`
- `src/GoodHamburguer.Blazor/Components/Pages/OrderDetails.razor.css`
- `src/GoodHamburguer.Blazor/Components/Pages/OrdersDashboard.razor`
- `src/GoodHamburguer.Blazor/Components/Pages/OrdersDashboard.razor.css`

**Artifacts the reviewer must have in hand before writing findings:**
- Stitch design system summary for project `9060755920845036151`
- Stitch screen titles and intended route mapping
- Current route HTML/runtime behavior from local app
- Current git diff or reviewed SHA range

---

### Task 1: Prepare Reviewer Context and Review Range

**Files:**
- Read: `docs/superpowers/plans/2026-04-23-stitch-design-review-plan.md`
- Read: `src/GoodHamburguer.Blazor/wwwroot/app.css`
- Read: `src/GoodHamburguer.Blazor/Components/Layout/MainLayout.razor`
- Read: `src/GoodHamburguer.Blazor/Components/Layout/NavMenu.razor`

- [ ] **Step 1: Capture the exact git review range**

Run:
```powershell
git status --short
git rev-parse HEAD
git log --oneline -n 10
```

Expected:
- You know whether review is against uncommitted local changes or commit-to-commit range
- You can identify the most relevant base point before the UI restyle work

- [ ] **Step 2: If a commit range exists, capture `BASE_SHA` and `HEAD_SHA`**

Run:
```powershell
git rev-parse HEAD~1
git rev-parse HEAD
```

Expected:
- A usable review range for `requesting-code-review`
- If the work is still uncommitted, note explicitly that the review must be done from working tree state instead of a strict SHA range

- [ ] **Step 3: Build the reviewer handoff description**

Required reviewer handoff content:
- What was implemented: “Blazor visual restyle to align with Stitch project `9060755920845036151`”
- Plan reference: this plan file
- Description: “Review styling, spacing, consistency, component adaptation, page fidelity, and cross-screen coherence”

- [ ] **Step 4: Dispatch the reviewer using the requesting-code-review workflow**

Use the template expectations from:
- `C:\Users\jefte\.codex\plugins\cache\openai-curated\superpowers\b7ed91c86dc6604b8217892f05c8866518c82852\skills\requesting-code-review\code-reviewer.md`

Expected:
- Reviewer is asked to evaluate design fidelity and consistency, not just code correctness

---

### Task 2: Validate Global Design Tokens and Foundations

**Files:**
- Read: `src/GoodHamburguer.Blazor/wwwroot/app.css`
- Read: `src/GoodHamburguer.Blazor/Components/_Imports.razor`

- [ ] **Step 1: Verify typography matches Stitch intent**

Inspect:
- Heading font family
- Body font family
- Label font family
- Weight and casing strategy

Expected findings categories:
- Missing font family alignment
- Inconsistent label casing
- Wrong visual hierarchy between headlines and body copy

- [ ] **Step 2: Verify color tokens and semantic mapping**

Check whether these concepts are represented cleanly:
- Background / surface / elevated surface
- Primary action red
- Secondary accent yellow
- On-surface text and muted text
- Error surfaces and borders

Expected:
- No random leftover light-theme colors
- No Bootstrap default blue driving the interface visually
- No page-level bespoke palette that fights shared tokens

- [ ] **Step 3: Verify spacing, radius, border, and shadow language**

Check for:
- Consistent padding rhythm
- Consistent panel radii
- Consistent border opacity
- Consistent button shapes
- Tonal depth instead of heavy mixed shadow styles

Expected:
- One coherent spacing system
- No “one-off” paddings that make screens feel unrelated

- [ ] **Step 4: Record any token drift as Important findings**

Reviewer output rule:
- If a shared token problem affects multiple pages, file it once at the shared source with explicit impact notes on affected screens

---

### Task 3: Review App Shell, Sidebar, Topbar, and Shared Layout

**Files:**
- Read: `src/GoodHamburguer.Blazor/Components/Layout/MainLayout.razor`
- Read: `src/GoodHamburguer.Blazor/Components/Layout/MainLayout.razor.css`
- Read: `src/GoodHamburguer.Blazor/Components/Layout/NavMenu.razor`
- Read: `src/GoodHamburguer.Blazor/Components/Layout/NavMenu.razor.css`
- Read: `src/GoodHamburguer.Blazor/Components/Layout/ReconnectModal.razor`
- Read: `src/GoodHamburguer.Blazor/Components/Layout/ReconnectModal.razor.css`

- [ ] **Step 1: Review shell composition against Stitch’s dashboard language**

Evaluate:
- Sidebar width and prominence
- Topbar visual weight
- Main content framing
- Whether the shell feels like the same product as the pages it contains

Expected:
- Sidebar should feel intentional, not like a leftover scaffold
- Topbar and content frame should support the dark premium direction

- [ ] **Step 2: Review nav states component by component**

Inspect:
- Default nav item
- Hover nav item
- Active nav item
- Icon treatment
- Label / sublabel hierarchy

Expected:
- Active state clearly reflects Stitch’s “filled/highlighted” intent
- Icons and labels feel like a system, not ad hoc per route

- [ ] **Step 3: Review shell responsiveness**

Check behavior at:
- Mobile width
- Tablet-ish width
- Desktop width

Expected:
- No clipped sidebar content
- No awkward topbar stacking
- No spacing collapse that breaks premium feel

- [ ] **Step 4: Review reconnect/error overlay consistency**

Check:
- Does reconnect modal clash with the design system
- Does error UI still look default/foreign

Expected:
- If visually off-brand, record as Minor or Important depending on prominence

---

### Task 4: Review Home Page as Entry Surface

**Files:**
- Read: `src/GoodHamburguer.Blazor/Components/Pages/Home.razor`
- Read shared styles from `src/GoodHamburguer.Blazor/wwwroot/app.css`

- [ ] **Step 1: Review hero section against the new shell**

Check:
- Does the home page visually belong to the same product as the Stitch-driven pages
- Is hierarchy crisp enough to serve as an entry surface

- [ ] **Step 2: Review metric cards and spacing**

Check:
- Card spacing
- Card typography
- Card consistency with cards on other pages

- [ ] **Step 3: Record whether `/` should remain simple or needs stronger parity**

Expected:
- Reviewer should explicitly say whether current minimalism is acceptable or feels unfinished beside the other four screens

---

### Task 5: Review `/menu` Against Stitch “Product Menu”

**Files:**
- Read: `src/GoodHamburguer.Blazor/Components/Pages/Menu.razor`
- Read: `src/GoodHamburguer.Blazor/Components/Pages/Menu.razor.css`

- [ ] **Step 1: Compare page structure to Stitch screen structure**

Check:
- Hero layout proportions
- Supporting stats placement
- Category card composition
- Density and rhythm of list items

- [ ] **Step 2: Review menu cards component by component**

Inspect:
- Category kicker
- Category title
- Item row
- Price emphasis
- Dividers and row spacing

Expected:
- Cards should feel premium and systematic
- Prices should read as intentional emphasis, not generic bold text

- [ ] **Step 3: Review all visible states**

States to inspect:
- Loading
- Error
- Empty
- Success/data loaded

Expected:
- State panels reuse the same visual language as loaded panels
- No fallback to generic alert styling

- [ ] **Step 4: Review content density and readability**

Check:
- Long category names
- Long item codes
- Price alignment
- Mobile stacking behavior

Expected:
- No visual crowding or broken alignment

---

### Task 6: Review `/orders/new` Against Stitch “New Order”

**Files:**
- Read: `src/GoodHamburguer.Blazor/Components/Pages/NewOrder.razor`
- Read: `src/GoodHamburguer.Blazor/Components/Pages/NewOrder.razor.css`

- [ ] **Step 1: Compare overall composition with Stitch**

Check:
- Hero plus note block relationship
- Form column versus side panel balance
- Whether the page reads as a purposeful order composer

- [ ] **Step 2: Review form components one by one**

Inspect:
- Field labels
- Select inputs
- Focus states
- Empty/default option presentation
- Submit button prominence

Expected:
- Form controls should not look like untouched Bootstrap defaults
- Primary CTA should visually own the screen

- [ ] **Step 3: Review supporting panels**

Inspect:
- Live preview card
- Metric cards
- Success state summary
- Success actions

Expected:
- Aside and success summary should feel part of the same design family as dashboard/menu cards

- [ ] **Step 4: Review feedback states**

States to inspect:
- Loading menu
- Menu unavailable
- Validation error
- Generic submit error
- Order created

Expected:
- Error and success treatment should be distinct but still on-brand

---

### Task 7: Review `/orders/{id}` Against Stitch “Order Details”

**Files:**
- Read: `src/GoodHamburguer.Blazor/Components/Pages\OrderDetails.razor`
- Read: `src/GoodHamburguer.Blazor/Components/Pages\OrderDetails.razor.css`

- [ ] **Step 1: Compare page hierarchy with Stitch details screen**

Check:
- Hero block
- Editable form area
- Financial summary sidebar
- Current configuration block

Expected:
- The page should feel like an operational detail view, not a reused create form

- [ ] **Step 2: Review action emphasis and destructive action handling**

Inspect:
- Save button prominence
- Delete button treatment
- Spacing between primary and destructive actions

Expected:
- Delete action must be visually clear without overpowering the screen

- [ ] **Step 3: Review summary modules**

Inspect:
- Financial summary readability
- Label/value contrast
- Compact summary variant consistency

Expected:
- Summary cards should feel data-driven and crisp

- [ ] **Step 4: Review detail page states**

States to inspect:
- Loading
- Not found / unavailable
- Validation error
- Update success
- Deleted state

Expected:
- Status surfaces do not visually regress to generic placeholder boxes

---

### Task 8: Review `/dashboard/orders` Against Stitch “Orders Dashboard”

**Files:**
- Read: `src/GoodHamburguer.Blazor/Components/Pages/OrdersDashboard.razor`
- Read: `src/GoodHamburguer.Blazor/Components/Pages/OrdersDashboard.razor.css`

- [ ] **Step 1: Compare dashboard composition to Stitch**

Check:
- Hero plus spotlight area
- Metrics row
- Order card list layout
- Page density and rhythm

Expected:
- Dashboard should feel like the flagship screen of the set

- [ ] **Step 2: Review order cards component by component**

Inspect:
- Ticket badge
- Order ID block
- Timestamp line
- Summary grid
- Details CTA

Expected:
- Cards should look like operational widgets, not generic repeated containers

- [ ] **Step 3: Review data hierarchy**

Check:
- Are totals readable
- Are summary labels secondary enough
- Is the “View details” action clearly subordinate to the data itself

- [ ] **Step 4: Review dashboard states**

States to inspect:
- Loading
- Empty
- Error
- Loaded list

Expected:
- Each state should preserve layout integrity and spacing standards

---

### Task 9: Review Cross-Screen Consistency and Shared Components

**Files:**
- Review all files listed in prior tasks

- [ ] **Step 1: Compare repeated patterns across all screens**

Patterns to compare:
- Hero sections
- Metric cards
- Panel padding
- Labels
- Buttons
- State panels
- Summary grids

Expected:
- Repeated patterns should feel deliberately shared, not approximately similar

- [ ] **Step 2: Find inconsistent spacing and rhythm**

Check for:
- Different vertical gaps for equivalent sections
- Different card paddings for equivalent modules
- Different title-to-body spacing for equivalent headers

- [ ] **Step 3: Find inconsistent visual semantics**

Check for:
- One page using yellow for emphasis while another uses red for the same semantic
- One page using muted labels while another uses bright labels for the same role
- One page using stronger borders or shadows without reason

- [ ] **Step 4: File shared consistency findings at the highest leverage layer**

Rule:
- If the inconsistency comes from page-local CSS, file page findings
- If the inconsistency should be solved through shared tokens or shared shell styling, file findings in shared files first

---

### Task 10: Review Adaptation Quality Versus Exact Fidelity

**Files:**
- Review all route files plus shared shell and tokens

- [ ] **Step 1: Separate “acceptable adaptation” from “design drift”**

Reviewer must classify differences as one of:
- Intentional adaptation that preserves the design language
- Acceptable engineering simplification
- Visual inconsistency that weakens the design system
- Clear mismatch against Stitch direction

- [ ] **Step 2: Flag false positives**

Do not file findings merely because:
- Blazor markup differs structurally from generated Stitch HTML
- Content text is not identical
- The app uses operational/runtime states that Stitch static screens do not show

- [ ] **Step 3: Flag true visual regressions**

Must file findings when:
- Shell looks like a different product than the target
- Shared components drift across pages
- Inputs/buttons/cards remain default-looking
- Spacing or typography breaks the premium industrial feel

---

### Task 11: Produce Final Review Output and Triage

**Files:**
- Reference: `C:\Users\jefte\.codex\plugins\cache\openai-curated\superpowers\b7ed91c86dc6604b8217892f05c8866518c82852\skills\requesting-code-review\code-reviewer.md`

- [ ] **Step 1: Produce findings in the required review format**

Output sections:
- Strengths
- Critical
- Important
- Minor
- Recommendations
- Assessment

- [ ] **Step 2: Require file and line references for every issue**

Expected:
- No vague “this page feels off” comments
- Every issue points to the concrete source file where the design drift is rooted

- [ ] **Step 3: Prioritize visual system issues correctly**

Severity guidance:
- Critical: broken layout, unreadable UI, inaccessible contrast, interaction-blocking visual bug
- Important: inconsistent design system, weak adaptation, off-brand core components, major spacing drift
- Minor: polish, micro-alignment, copy-level visual tuning

- [ ] **Step 4: Conclude with merge-readiness specifically for the design pass**

Final assessment must answer:
- Is the frontend visually consistent enough with Stitch to consider this pass standardized
- If not, what minimum set of Important findings must be fixed before claiming design alignment

---

## Self-Review

**Spec coverage:** This plan covers shared tokens, shell, sidebar, topbar, reconnect/error overlays, home page, `/menu`, `/orders/new`, `/orders/{id}`, `/dashboard/orders`, all key states, responsiveness, and cross-screen consistency.

**Placeholder scan:** No `TODO`, `TBD`, or “review later” placeholders remain. Every task names exact files, review targets, and expected reviewer behavior.

**Type consistency:** Route names, file paths, and Stitch project id are consistent across all tasks.

## Execution Handoff

Plan complete and saved to `docs/superpowers/plans/2026-04-23-stitch-design-review-plan.md`.

Next step:
1. Use `$superpowers:requesting-code-review` with this plan as the requirements reference.
2. Ask the reviewer to inspect the current Blazor styling against Stitch project `9060755920845036151`, page by page and component by component.
3. Treat unresolved Important findings as blockers for claiming design standardization.
