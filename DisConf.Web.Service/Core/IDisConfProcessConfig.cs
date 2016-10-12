using CommonProcess;
using PetaPoco;

namespace DisConf.Web.Service.Core
{
    public interface IDisConfProcessConfig : IDataProcessConfig
    {
        Database Db { get; }

        string ZookeeperHost { get; }
    }
}
