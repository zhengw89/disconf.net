using System;
using System.Text;
using System.Web.Mvc;
using DisConf.Web.Model;

namespace DisConf.Web.Helper.CustomHtmlHelper
{
    public static class PagingHelpers
    {
        //current index left&right buffer
        private const int NavPageSize = 3;

        public static MvcHtmlString PageLinks(
            this HtmlHelper html,
            IPageList pagingInfo,
            Func<long, string> pageUrl
        )
        {
            var pagingBuilder = new PagingHtmlBuilder();
            var result = new StringBuilder();
            //previous link
            string prevLink = (pagingInfo.CurrentPage == 1)
                ? pagingBuilder.BuildHtmlItem(pageUrl(pagingInfo.CurrentPage - 1), "<", false, true)
                : pagingBuilder.BuildHtmlItem(pageUrl(pagingInfo.CurrentPage - 1), "<");
            result.Append(prevLink);

            var start = (pagingInfo.CurrentPage <= NavPageSize + 1) ? 1 : (pagingInfo.CurrentPage - NavPageSize);

            var end = (pagingInfo.CurrentPage > (pagingInfo.TotalPages - NavPageSize)) ? pagingInfo.TotalPages : pagingInfo.CurrentPage + NavPageSize;

            for (long i = start; i <= end; i++)
            {
                string pageHtml = (i == pagingInfo.CurrentPage)
                    ? pagingBuilder.BuildHtmlItem(pageUrl(i), i.ToString(), true)
                    : pagingBuilder.BuildHtmlItem(pageUrl(i), i.ToString());
                result.Append(pageHtml);
            }

            // next link
            string nextLink = (pagingInfo.CurrentPage >= pagingInfo.TotalPages)
                ? pagingBuilder.BuildHtmlItem(pageUrl(pagingInfo.CurrentPage + 1), ">", false, true)
                : pagingBuilder.BuildHtmlItem(pageUrl(pagingInfo.CurrentPage + 1), ">");
            result.Append(nextLink);

            return MvcHtmlString.Create(result.ToString());
        }
    }
}