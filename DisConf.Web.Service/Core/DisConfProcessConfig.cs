using CommonProcess.DependentProvider;
using DisConf.Web.Model;
using PetaPoco;

namespace DisConf.Web.Service.Core
{
    internal class DisConfProcessConfig : IDisConfProcessConfig
    {
        public IDependentProvider DependentProvider { get; set; }

        private readonly User _user;
        public User User { get { return this._user; } }

        private readonly Database _db;
        public Database Db { get { return this._db; } }

        private readonly string _zookeeperHost;
        public string ZookeeperHost { get { return this._zookeeperHost; } }

        public DisConfProcessConfig(User user, Database db, string zookeeperHost)
        {
            this._user = user;
            this._db = db;
            this._zookeeperHost = zookeeperHost;
        }
    }
}
