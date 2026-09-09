using JO.DataModel.DTOs;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.Pagination;

public partial class NumberedPagination
{
    [Parameter] public int TotalRows { get; set; }
    [Parameter] public int RowsPerPage { get; set; } = 10;
    [Parameter] public int MaxPageButtons { get; set; } = 5;

    // IMPORTANT: parent controls this
    [Parameter] public int CurrentPage { get; set; }

    [Parameter] public EventCallback<int> OnPageChanged { get; set; }

    private int TotalPages =>
        RowsPerPage == 0 ? 0 :
        (int)Math.Ceiling((double)TotalRows / RowsPerPage);

    private bool Show => TotalPages > 1;

    private int StartPage
    {
        get
        {
            int start = Math.Max(1, CurrentPage - (MaxPageButtons / 2));
            int end = Math.Min(TotalPages, start + MaxPageButtons - 1);

            if (end - start + 1 < MaxPageButtons)
                start = Math.Max(1, end - MaxPageButtons + 1);

            return start;
        }
    }

    private int EndPage => Math.Min(TotalPages, StartPage + MaxPageButtons - 1);

    private async Task ChangePage(int newPage)
    {
        if (newPage < 1 || newPage > TotalPages || newPage == CurrentPage)
            return;

        await OnPageChanged.InvokeAsync(newPage);
    }
}
