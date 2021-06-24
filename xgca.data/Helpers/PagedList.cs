using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace otm.data.Helpers
{
    public class PagedList<T>
    {
        public int TotalCount { get; private set; }
        public List<T> Items { get; private set; }

        public PagedList(IQueryable<T> source, int pageNumber, int pageSize, string orderBy = null)
        {
            TotalCount = source.Count();

            // sorting
            if (!string.IsNullOrEmpty(orderBy))
                source = source.OrderBy(orderBy.Replace(":", " "));
            else
                source = source.OrderBy("createdOn desc");

            // pagination
            Items = source.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
        }
    }
}