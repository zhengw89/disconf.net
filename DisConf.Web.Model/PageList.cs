using System.Collections;
using System.Collections.Generic;

namespace DisConf.Web.Model
{
    public class PageList<T> : IEnumerable<T>, IEnumerable, IPageList
    {
        public long CurrentPage { get; set; }

        public long TotalPages { get; set; }

        public long TotalItems { get; set; }

        public long ItemsPerPage { get; set; }

        public List<T> Items { get; set; }

        public PageList()
        {
            this.Items = new List<T>();
        }

        public void Add(T item)
        {
            this.Items.Add(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        public void CopyPageInfo(IPageList pageList)
        {
            this.CurrentPage = pageList.CurrentPage;
            this.TotalItems = pageList.TotalItems;
            this.TotalPages = pageList.TotalPages;
            this.ItemsPerPage = pageList.ItemsPerPage;
        }
    }
}
