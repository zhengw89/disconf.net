using System.Collections.Generic;
using System.Linq;
using DisConf.Web.Model;
using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Repository.Models;
using PetaPoco;

namespace DisConf.Web.Repository.Repositories
{
    internal class AppRepository : BaseRepository<App>, IAppRepository
    {
        public AppRepository(Database db)
            : base(db)
        {
        }

        public bool Exists(int id)
        {
            return base.Db.Exists<T_APP>(id);
        }

        public bool ExistsByName(string name)
        {
            var sql = new Sql();
            sql.Select("COUNT(*)");
            sql.From("T_APP");
            sql.Where("APP_Name = @0", name);

            return base.Db.ExecuteScalar<long>(sql) > 0;
        }

        public bool ExistsOtherByName(int id, string name)
        {
            var sql = new Sql();
            sql.Select("COUNT(*)");
            sql.From("T_APP");
            sql.Where("APP_Name = @0 AND APP_Id != @1", name, id);

            return base.Db.ExecuteScalar<long>(sql) > 0;
        }

        public bool Create(App app)
        {
            return (int)base.Db.Insert(base.Map<App, T_APP>(app)) > 0;
        }

        public bool Update(App app)
        {
            return base.Db.Update(base.Map<App, T_APP>(app)) > 0;
        }

        public bool Delete(int id)
        {
            return base.Db.Delete(id) > 0;
        }

        public List<App> GetAll()
        {
            return base.Db.Query<T_APP>(new Sql()).Select(a => base.Map<T_APP, App>(a)).ToList();
        }

        public App GetById(int id)
        {
            var queryResult = base.Db.SingleOrDefault<T_APP>(id);
            return base.Map<T_APP, App>(queryResult);
        }

        public App GetByName(string name)
        {
            var sql = new Sql();
            sql.Where("APP_Name = @0", name);

            var queryResult = base.Db.SingleOrDefault<T_APP>(sql);
            return base.Map<T_APP, App>(queryResult);
        }

        public PageList<App> GetByPage(int pageIndex, int pageSize)
        {
            var queryResult = base.Db.Page<T_APP>(pageIndex, pageSize, new Sql());
            if (queryResult == null) return null;

            var result = new PageList<App>();
            if (queryResult.Items != null)
            {
                result.Items = queryResult.Items.Select(a => base.Map<T_APP, App>(a)).ToList();
            }

            base.CopyPageInfoToPageList(queryResult, result);

            return result;
        }
    }
}
