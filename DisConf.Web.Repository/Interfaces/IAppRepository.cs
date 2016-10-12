using System.Collections.Generic;
using DisConf.Web.Model;

namespace DisConf.Web.Repository.Interfaces
{
    public interface IAppRepository
    {
        bool Exists(int id);

        bool ExistsByName(string name);

        bool ExistsOtherByName(int id, string name);

        bool Create(App app);

        bool Update(App app);

        bool Delete(int id);

        List<App> GetAll();

        App GetById(int id);

        App GetByName(string name);

        PageList<App> GetByPage(int pageIndex, int pageSize);
    }
}
