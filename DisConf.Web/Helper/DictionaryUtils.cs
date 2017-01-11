using System.Web.Routing;

namespace DisConf.Web.Helper
{
    public class DictionaryUtils
    {
        public static RouteValueDictionary Clone(RouteValueDictionary dic)
        {
            var destDic = new RouteValueDictionary();
            foreach (var item in dic)
            {
                destDic.Add(item.Key, item.Value);
            }

            return destDic;
        }
    }
}