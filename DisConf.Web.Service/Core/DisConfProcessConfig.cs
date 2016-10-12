using CommonProcess.DependentProvider;
using PetaPoco;

namespace DisConf.Web.Service.Core
{
    internal class DisConfProcessConfig : IDisConfProcessConfig
    {
        private readonly Database _db;

        public Database Db { get { return this._db; } }
        public IDependentProvider DependentProvider { get; set; }

        private readonly string _zookeeperHost;
        public string ZookeeperHost { get { return this._zookeeperHost; } }

        public DisConfProcessConfig(Database db, string zookeeperHost)
        {
            this._db = db;
            this._zookeeperHost = zookeeperHost;
        }
    }
}
