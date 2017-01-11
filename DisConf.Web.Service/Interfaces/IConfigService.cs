using System.Collections.Generic;
using DisConf.Web.Model;
using DisConf.Web.Service.Model;
using ZooKeeperNet;

namespace DisConf.Web.Service.Interfaces
{
    public interface IConfigService
    {
        BizResult<bool> Create(int appId, int envId, string name, string value);

        BizResult<bool> Update(int id, string value);

        BizResult<bool> Delete(int id);

        BizResult<Config> GetById(int id);

        BizResult<Config> GetByName(int appId, int envId, string name);

        BizResult<List<Config>> GetAll(int appId, int envId);

        BizResult<PageList<Config>> GetByCondition(int appId, int envId, int pageIndex, int pageSize);

        bool ForceRefresh(ZooKeeper zk, int appId, string appName, int envId, string envName);

        BizResult<PageList<ConfigLog>> GetConfigLogs(int? appId, int? envId, int? configId, string configNameFuzzy, int pageIndex, int pageSize);

        int GetSyncCount(ZooKeeper zk, string app, string env, string config, string value);
    }
}
