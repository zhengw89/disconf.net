using CommonProcess;
using DisConf.Web.Model;
using PetaPoco;

namespace DisConf.Web.Service.Core
{
    public interface IDisConfProcessConfig : IDataProcessConfig
    {
        User User { get; }

        Database Db { get; }

        string ZookeeperHost { get; }
    }
}
