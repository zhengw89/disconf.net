using System.Collections.Generic;
using DisConf.Web.Model;

namespace DisConf.Web.Repository.Interfaces
{
    public interface IConfigRepository
    {
        bool Exists(int id);

        bool ExistsByName(int appId, int envId, string name);

        bool ExistsOtherByName(int appId, int envId, int id, string name);

        int Create(Config config);

        bool Update(Config config);

        bool Delete(int id);

        Config GetById(int id);

        Config GetByName(int appId, int envId, string name);

        List<Config> GetByAppAndEnv(int appId, int envId);

        PageList<Config> GetByCondition(int appId, int envId, int pageIndex, int pageSize);
    }
}
