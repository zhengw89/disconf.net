using System.Linq;
using DisConf.Web.Model;
using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Repository.Models;
using PetaPoco;

namespace DisConf.Web.Repository.Repositories
{
    internal class ConfigLogRepository : BaseRepository<ConfigLog>, IConfigLogRepository
    {
        public ConfigLogRepository(Database db)
            : base(db)
        {
        }

        public bool Create(ConfigLog log)
        {
            return (int)base.Db.Insert(base.Map<ConfigLog, T_CONFIG_LOG>(log)) > 0;
        }

        public PageList<ConfigLog> GetByCondition(int? appId, int? envId, int? configId, int pageIndex, int pageSize)
        {
            var sql = new Sql();
            if (appId.HasValue)
            {
                sql.Where("APP_Id = @0", appId.Value);
            }
            if (envId.HasValue)
            {
                sql.Where("ENV_Id = @0", envId.Value);
            }
            if (configId.HasValue)
            {
                sql.Where("CON_Id = @0", configId.Value);
            }
            sql.OrderBy("OPT_Time DESC");

            var queryResult = base.Db.Page<T_CONFIG_LOG>(pageIndex, pageSize, sql);
            if (queryResult == null) return null;

            var result = new PageList<ConfigLog>();
            if (queryResult.Items != null)
            {
                result.Items = queryResult.Items.Select(a => base.Map<T_CONFIG_LOG, ConfigLog>(a)).ToList();
            }

            base.CopyPageInfoToPageList(queryResult, result);

            return result;
        }

        public PageList<ConfigLog> GetByCondition(int? appId, int? envId, string configNameFuzzy, int pageIndex, int pageSize)
        {
            var sql = new Sql();
            if (appId.HasValue)
            {
                sql.Where("APP_Id = @0", appId.Value);
            }
            if (envId.HasValue)
            {
                sql.Where("ENV_Id = @0", envId.Value);
            }
            if (!string.IsNullOrEmpty(configNameFuzzy))
            {
                sql.Where("CON_Name LIKE @0", string.Format("%{0}%", configNameFuzzy));
            }

            sql.OrderBy("OPT_Time DESC");

            var queryResult = base.Db.Page<T_CONFIG_LOG>(pageIndex, pageSize, sql);
            if (queryResult == null) return null;

            var result = new PageList<ConfigLog>();
            if (queryResult.Items != null)
            {
                result.Items = queryResult.Items.Select(a => base.Map<T_CONFIG_LOG, ConfigLog>(a)).ToList();
            }

            base.CopyPageInfoToPageList(queryResult, result);

            return result;
        }
    }
}
