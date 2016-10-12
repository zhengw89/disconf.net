using System.Collections.Generic;

namespace DisConf.Web.Model
{
    public class PageList<T> : IPageList
    {
        public long CurrentPage { get; set; }

        public long TotalPages { get; set; }

        public long TotalItems { get; set; }

        public long ItemsPerPage { get; set; }

        public List<T> Items { get; set; }
    }
}
