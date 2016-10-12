using System;
using DisConf.Web.Model;

namespace DisConf.Web.Models.Shared
{
    public class PaginationViewModel
    {
        public Func<long, string> GenerateUrlFunc { get; set; }

        public IPageList PagedList { get; set; }
    }
}