using System.Collections.Generic;
using DisConf.Web.Model;
using DisConf.Web.Service.Model;

namespace DisConf.Web.Service.Interfaces
{
    public interface IEnvService
    {
        BizResult<Env> GetEnvById(int id);

        BizResult<Env> GetEnvByName(string envName);

        BizResult<List<Env>> GetAllEnv();
    }
}
