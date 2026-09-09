# Reusing pagination

For collections already loaded into memory, import `JO.Service.Extensions` and call
`items.ToPagedResult(page, pageSize)`. It returns the existing `PagedResult<T>` DTO.
Page numbers are one-based and clamped to the available range. Empty results use
page 1. Nonpositive page sizes throw `ArgumentOutOfRangeException`. Enumerables
without a collection count are materialized once per call, so keep the loaded
collection in page state for repeated navigation.

In JO.BlazorDemoApp, import `JO.BlazorDemoApp.Components.Pages.Pagination` and render the shared component:

```razor
<Pagination Result="@pagedItems"
            PageChanged="ChangePage"
            PageSizeChanged="ChangePageSize"
            Disabled="@isLoading"
            AriaLabel="Item pages" />
```

Render table rows from `pagedItems.Data`. Handle navigation by assigning
`pagedItems = items.ToPagedResult(page, pagedItems.PageSize)`. Handle size changes
by assigning `pagedItems = items.ToPagedResult(1, pageSize)`. Refresh the result
when the source collection changes. See ResponseRawData for a complete example.

The component infers the item type from Result. Optional PageSizes defaults to
10, 25, 50, and 100. It exposes first/previous/next/last navigation, direct page
entry, record counts, and disabled states for loading, errors, or empty results.
Callbacks are awaited, so other pages can also fetch data asynchronously.

For database paging, keep using `IQueryable<T>.ToPagedResultAsync` and pass its
result to the same component. Apply a deterministic OrderBy before paging and
validate the requested page and page size; the existing async extension retains
its original behavior. Do not use the in-memory overload for database queries,
as it materializes the sequence. ResponseRawData still loads all responses and
only pages their display; this change does not reduce database transfer size.

Both pagers now live in this folder with their C# logic in .razor.cs files.
NumberedPagination is the older numbered-button pager moved from JO.BlazorApp.
It retains TotalRows, RowsPerPage, MaxPageButtons, CurrentPage, and OnPageChanged parameters.
