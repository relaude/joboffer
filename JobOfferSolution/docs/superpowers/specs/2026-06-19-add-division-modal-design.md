# Add Division Modal Design

## Scope

Update only the `modalAddDivision` presentation in `JO.BlazorApp/Components/Pages/Admin/User/NewUser.razor`. The modal is a static UI mockup: no filtering, selection, or submission behavior will be wired, and no build or runtime verification is requested.

## Layout

Use the existing large modal and SBAdmin2 styling. The child content contains one compact card with:

- A concise guidance row with a building icon and a `0 selected` badge.
- A responsive two-column filter area for Company and Division. Each search input includes a FontAwesome search icon and stacks to one column on small screens.
- A responsive, bordered, compact table with Checkbox, Company, Division Code, and Division Name columns.

The table uses realistic mock companies and divisions. All checkboxes start empty. Company and division codes use badges to improve scanning while division names remain the primary readable text.

## Footer

Add a primary small button labeled `Add selected division` with a FontAwesome plus icon. It is disabled because the static mockup begins with zero selected rows and has no interaction wiring.

## UX Principles

Keep controls visually grouped, labels explicit, table density compact, and spacing balanced. Preserve horizontal scrolling on narrow screens through `table-responsive`. Use muted supporting copy and badges sparingly so the table remains the main focus.

## Verification Constraint

Review markup statically only. Do not build or run the application.
