using DisConf.Web.Model;
using DisConf.Web.Repository.Models;
using PetaPoco;

namespace DisConf.Web.Repository.Repositories
{
    /// <summary>
    /// 数据仓库基类
    /// </summary>
    /// <typeparam name="T">数据类型泛型</typeparam>
    internal abstract class BaseRepository<T>
    {
        protected readonly Database Db;

        protected BaseRepository(Database db)
        {
            this.Db = db;

            MapperHelper.Initialize();
        }

        /// <summary>
        /// 拷贝分页相关信息
        /// </summary>
        /// <typeparam name="TT"></typeparam>
        /// <param name="s"></param>
        /// <param name="t"></param>
        protected void CopyPageInfoToPageList<TT>(Page<TT> s, PageList<T> t)
        {
            t.CurrentPage = s.CurrentPage;
            t.ItemsPerPage = s.ItemsPerPage;
            t.TotalItems = s.TotalItems;
            t.TotalPages = s.TotalPages;
        }

        /// <summary>
        /// 转换Model
        /// </summary>
        /// <typeparam name="TSource">转换源类型</typeparam>
        /// <typeparam name="TDestination">转换目标类型</typeparam>
        /// <param name="source">转换源对象</param>
        /// <returns></returns>
        protected TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : class
            where TSource : class
        {
            if (source == null) return null;
            return AutoMapper.Mapper.Map<TSource, TDestination>(source);
        }
    }
}
