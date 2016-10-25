using CommonProcess;
using PetaPoco;

namespace DisConf.Web.Service.Core
{
    public interface IDisConfProcessConfig : IDataProcessConfig
    {
        string UserName { get; }

        Database Db { get; }

        string ZookeeperHost { get; }
    }
}
