using System;
using CommonProcess.DependentProvider;
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

        /// <summary>
        /// 执行业务过程
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="func">核心逻辑</param>
        /// <returns></returns>
        protected T ExeProcess<T>(Func<Database, T> func)
        {
            using (var db = this.CreateDatabase())
            {
                return func.Invoke(db);
            }
        }

        /// <summary>
        /// 根据业务执行类解析其配置对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <returns></returns>
        protected IDisConfProcessConfig ResloveProcessConfig<T>(Database db)
        {
            return new DisConfProcessConfig(this.Config.UserName, db, this.Config.ZookeeperHost)
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

    /// <summary>
    /// 业务配置对象
    /// </summary>
    public class ServiceConfig
    {
        private readonly string _userName;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get { return this._userName; } }

        private readonly string _connectionName;
        /// <summary>
        /// 数据库连接字符串名称
        /// </summary>
        public string ConnectionName { get { return this._connectionName; } }

        private readonly string _zookeeperHost;
        /// <summary>
        /// Zookeeper地址
        /// </summary>
        public string ZookeeperHost { get { return this._zookeeperHost; } }

        public ServiceConfig(string userName, string connectionName, string zookeeperHost)
        {
            this._userName = userName;
            this._connectionName = connectionName;
            this._zookeeperHost = zookeeperHost;
        }
    }
}
