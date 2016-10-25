using CommonProcess.DependentProvider;
using PetaPoco;

namespace DisConf.Web.Service.Core
{
    internal class DisConfProcessConfig : IDisConfProcessConfig
    {
        public IDependentProvider DependentProvider { get; set; }

        private readonly string _userName;
        /// <summary>
        /// 操作用户用户名
        /// </summary>
        public string UserName { get { return this._userName; } }

        private readonly Database _db;
        public Database Db { get { return this._db; } }

        private readonly string _zookeeperHost;
        public string ZookeeperHost { get { return this._zookeeperHost; } }

        public DisConfProcessConfig(string userName, Database db, string zookeeperHost)
        {
            this._userName = userName;
            this._db = db;
            this._zookeeperHost = zookeeperHost;
        }
    }
}
