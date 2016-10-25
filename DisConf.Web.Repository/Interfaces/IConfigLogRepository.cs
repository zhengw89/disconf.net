using DisConf.Web.Model;

namespace DisConf.Web.Repository.Interfaces
{
    public interface IConfigLogRepository
    {
        bool Create(ConfigLog log);

        PageList<ConfigLog> GetByCondition(int configId, int pageIndex, int pageSize);
    }
}
