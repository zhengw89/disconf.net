using System.Collections.Generic;
using System.Linq;
using DisConf.Web.Model;
using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Repository.Models;
using PetaPoco;

namespace DisConf.Web.Repository.Repositories
{
    internal class ConfigRepository : BaseRepository<Config>, IConfigRepository
    {
        public ConfigRepository(Database db)
            : base(db)
        {
        }

        public bool Exists(int id)
        {
            return base.Db.Exists<T_CONFIG>(id);
        }

        public bool ExistsByName(int appId, int envId, string name)
        {
            var sql = new Sql();
            sql.Select("COUNT(*)");
            sql.From("T_CONFIG");
            sql.Where("APP_Id = @0 AND ENV_ID = @1 AND CON_Name = @2", appId, envId, name);

            return base.Db.ExecuteScalar<long>(sql) > 0;
        }

        public bool ExistsOtherByName(int appId, int envId, int id, string name)
        {
            var sql = new Sql();
            sql.Select("COUNT(*)");
            sql.From("T_CONFIG");
            sql.Where("APP_Id = @0 AND ENV_ID = @1 AND CON_Id = @2 AND CON_Name = @3", appId, envId, id, name);

            return base.Db.ExecuteScalar<long>(sql) > 0;
        }

        public int Create(Config config)
        {
            return (int)base.Db.Insert(base.Map<Config, T_CONFIG>(config));
        }

        public bool Update(Config config)
        {
            return base.Db.Update(base.Map<Config, T_CONFIG>(config)) > 0;
        }

        public bool Delete(int id)
        {
            return base.Db.Delete<T_CONFIG>(id) > 0;
        }

        public Config GetById(int id)
        {
            var queryResult = base.Db.SingleOrDefault<T_CONFIG>(id);
            return base.Map<T_CONFIG, Config>(queryResult);
        }

        public Config GetByName(int appId, int envId, string name)
        {
            var sql = new Sql();
            sql.Where("APP_Id = @0 AND ENV_Id = @1 AND CON_Name = @2", appId, envId,name);

            var queryResult = base.Db.SingleOrDefault<T_CONFIG>(sql);

            return base.Map<T_CONFIG, Config>(queryResult);
        }

        public List<Config> GetByAppAndEnv(int appId, int envId)
        {
            var sql = new Sql();
            sql.Where("APP_Id = @0 AND ENV_Id = @1", appId, envId);

            return base.Db.Query<T_CONFIG>(sql).Select(a => base.Map<T_CONFIG, Config>(a)).ToList();
        }

        public PageList<Config> GetByCondition(int appId, int envId, int pageIndex, int pageSize)
        {
            var sql = new Sql();
            sql.Where("APP_Id = @0 AND ENV_Id = @1", appId, envId);

            var queryResult = base.Db.Page<T_CONFIG>(pageIndex, pageSize, sql);
            if (queryResult == null) return null;

            var result = new PageList<Config>();
            if (queryResult.Items != null)
            {
                result.Items = queryResult.Items.Select(a => base.Map<T_CONFIG, Config>(a)).ToList();
            }

            base.CopyPageInfoToPageList(queryResult, result);

            return result;
        }
    }
}
