using System.Collections.Generic;
using DisConf.Web.Model;

namespace DisConf.Web.Repository.Interfaces
{
    public interface IEnvRepository
    {
        bool Exists(int id);

        Env GetById(int id);

        Env GetByName(string name);

        List<Env> GetAll();
    }
}
