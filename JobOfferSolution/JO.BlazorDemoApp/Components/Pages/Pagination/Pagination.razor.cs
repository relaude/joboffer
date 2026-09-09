using JO.DataModel.DTOs;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.Pagination;

public partial class Pagination<TItem>
{
    [Parameter, EditorRequired] public PagedResult<TItem> Result { get; set; } = default!;
    [Parameter] public EventCallback<int> PageChanged { get; set; }
    // The parent resets to page 1 when applying a new page size.
    [Parameter] public EventCallback<int> PageSizeChanged { get; set; }
    [Parameter] public IReadOnlyList<int> PageSizes { get; set; } = new[] { 10, 25, 50, 100 };
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string AriaLabel { get; set; } = "Pagination";

    private bool IsDisabled => Disabled || Result.TotalCount == 0;
    private int TotalPages => Result.PageSize <= 0 || Result.TotalCount == 0 ? 1 : (Result.TotalCount - 1) / Result.PageSize + 1;
    private long FirstRecord => Result.TotalCount == 0 ? 0 : (long)(Result.Page - 1) * Result.PageSize + 1;
    private long LastRecord => Math.Min((long)Result.Page * Result.PageSize, Result.TotalCount);
    private Task ChangePageAsync(int page) => IsDisabled
        ? Task.CompletedTask
        : PageChanged.InvokeAsync(Math.Clamp(page, 1, TotalPages));

    private Task ChangePageSizeAsync(ChangeEventArgs args) =>
        !IsDisabled && int.TryParse(args.Value?.ToString(), out var size) && size > 0 && PageSizes.Contains(size)
            ? PageSizeChanged.InvokeAsync(size)
            : Task.CompletedTask;
}
