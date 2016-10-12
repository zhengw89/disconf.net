using System;
using System.Collections.Generic;
using CommonProcess.DependentProvider;
using DisConf.Web.Repository.Factory;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Model;
using DisConf.Web.Service.Core.Process;
using DisConf.Web.Service.Model;
using DisConf.Web.Service.OperateDependentFactory;
using PetaPoco;

namespace DisConf.Web.Service
{
    internal abstract class BaseService
    {
        protected readonly ServiceConfig Config;

        protected BaseService(ServiceConfig config)
        {
            this.Config = config;
        }

        #region Protected Method

        protected T ExeProcess<T>(Func<Database, T> func)
        {
            using (var db = this.CreateDatabase())
            {
                return func.Invoke(db);
            }
        }

        protected IDisConfProcessConfig ResloveProcessConfig<T>(Database db)
        {
            return new DisConfProcessConfig(db, this.Config.ZookeeperHost)
            {
                DependentProvider = this.ResloveOperateDependent<T>(db),
            };
        }

        protected BizResult<T> ExeQueryProcess<T>(DisConfQueryProcess<T> queryer)
        {
            return new DisConfProcessResult<T>()
            {
                Data = queryer.ExecuteQueryProcess(),
                Error = queryer.GetError().ToDisConfProcessError()
            }.ConvertToBizResult();
        }

        protected BizResult<bool> ExeOperateProcess(DisConfOperateProcess operate)
        {
            return new DisConfProcessResult<bool>()
            {
                Data = operate.ExecuteProcess(),
                Error = operate.GetError().ToDisConfProcessError()
            }.ConvertToBizResult();
        }

        protected BizResult<T> ExeOperateProcess<T>(DisConfWithResultOperateProcess<T> operate)
        {
            return new DisConfProcessResult<T>()
            {
                Data = operate.ExecuteProcess(),
                Error = operate.GetError().ToDisConfProcessError()
            }.ConvertToBizResult();
        }

        #endregion

        #region Private Method

        private Database CreateDatabase()
        {
            var db = new Database(this.Config.ConnectionName);
            return db;
        }

        private IDependentProvider ResloveOperateDependent<T>(Database db)
        {
            return OperateDependentLocator.Container.Resolve<T>(db);
        }

        #endregion
    }

    public class ServiceConfig
    {
        private readonly string _connectionName;
        public string ConnectionName { get { return this._connectionName; } }

        private readonly string _zookeeperHost;
        public string ZookeeperHost { get { return this._zookeeperHost; } }

        public ServiceConfig(string connectionName, string zookeeperHost)
        {
            this._connectionName = connectionName;
            this._zookeeperHost = zookeeperHost;
        }
    }
}
