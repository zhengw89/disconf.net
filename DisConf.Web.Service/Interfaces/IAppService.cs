using DisConf.Web.Model;
using DisConf.Web.Service.Model;

namespace DisConf.Web.Service.Interfaces
{
    public interface IAppService
    {
        BizResult<bool> CreateApp(string name, string description);

        BizResult<bool> Update(int id, string name, string description);

        BizResult<bool> Delete(int id);

        BizResult<PageList<App>> GetByCondition(int pageIndex, int pageSize);

        BizResult<App> GetById(int appId);

        BizResult<App> GetByName(string name);
    }
}
