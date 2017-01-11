using DisConf.Web.Model;

namespace DisConf.Web.Repository.Interfaces
{
    public interface IConfigLogRepository
    {
        bool Create(ConfigLog log);

        PageList<ConfigLog> GetByCondition(int? appId, int? envId, int? configId, int pageIndex, int pageSize);

        PageList<ConfigLog> GetByCondition(int? appId, int? envId, string configNameFuzzy, int pageIndex, int pageSize);
    }
}
