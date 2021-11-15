using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Helpers
{
    public class ObjectPagedList<T> : List<T>
    {
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}