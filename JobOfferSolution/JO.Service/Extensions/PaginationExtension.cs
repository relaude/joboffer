using JO.DataModel.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Extensions
{
    public static class PaginationExtension
    {
        /// <summary>
        /// Pages an in-memory sequence. Pages are one-based and clamped to the
        /// available range; an empty sequence has page 1. Page size must be positive.
        /// Use ToPagedResultAsync for database queries.
        /// </summary>
        public static PagedResult<T> ToPagedResult<T>(this IEnumerable<T> source, int page, int pageSize)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);

            var items = source as ICollection<T> ?? source.ToList();
            var totalPages = items.Count == 0 ? 1 : (items.Count - 1) / pageSize + 1;
            page = Math.Clamp(page, 1, totalPages);

            return new PagedResult<T>
            {
                Data = items.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                TotalCount = items.Count,
                Page = page,
                PageSize = pageSize
            };
        }

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
