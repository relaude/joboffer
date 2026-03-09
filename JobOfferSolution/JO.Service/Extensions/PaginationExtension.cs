using JO.DataModel.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Extensions
{
    public static class PaginationExtension
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            int page,
            int pageSize)
        {
            var result = new PagedResult<T>();

            result.TotalCount = await query.CountAsync();

            result.Data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            result.Page = page;
            result.PageSize = pageSize;

            return result;
        }
    }
}
