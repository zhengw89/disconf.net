using System.Web.Mvc;

namespace DisConf.Web.Helper.CustomHtmlHelper
{
    public class PagingHtmlBuilder
    {
        public string BuildHtmlItem(string url, string display, bool active = false, bool disabled = false)
        {
            // every item wrapped in LI tag
            var liTag = new TagBuilder("li");
            if (disabled)
            {
                // add disabled class and use a SPAN tag inside
                liTag.MergeAttribute("class", "disabled");
                var spanTag = new TagBuilder("span") { InnerHtml = display };
                liTag.InnerHtml = spanTag.ToString();
            }
            else
            {
                if (active)
                {
                    liTag.MergeAttribute("class", "active");
                }
                // use inner A tag
                var tag = new TagBuilder("a");
                tag.MergeAttribute("href", url);
                tag.InnerHtml = display;
                liTag.InnerHtml = tag.ToString();
            }
            return liTag.ToString();
        }
    }
}