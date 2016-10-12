namespace DisConf.Web.Model
{
   public interface IPageList
    {
         long CurrentPage { get; set; }

         long TotalPages { get; set; }

         long TotalItems { get; set; }

         long ItemsPerPage { get; set; }
    }
}
