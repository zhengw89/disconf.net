using DisConf.Web.Model;
using DisConf.Web.Repository.Models;
using PetaPoco;

namespace DisConf.Web.Repository.Repositories
{
    internal abstract class BaseRepository<T>
    {
        protected readonly Database Db;

        protected BaseRepository(Database db)
        {
            this.Db = db;

            MapperHelper.Initialize();
        }

        protected void CopyPageInfoToPageList<TT>(Page<TT> s, PageList<T> t)
        {
            t.CurrentPage = s.CurrentPage;
            t.ItemsPerPage = s.ItemsPerPage;
            t.TotalItems = s.TotalItems;
            t.TotalPages = s.TotalPages;
        }

        protected TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : class
            where TSource : class
        {
            if (source == null) return null;
            return AutoMapper.Mapper.Map<TSource, TDestination>(source);
        }
    }
}
