using CommonProcess.DependentProvider;
using DisConf.Web.Repository.Factory;
using PetaPoco;

namespace DisConf.Web.Service.Core
{
    /// <summary>
    /// 操作类依赖容器
    /// </summary>
    internal abstract class DisConfBaseDependentProvider : BaseDependentProvider
    {
        protected readonly Database Db;

        protected DisConfBaseDependentProvider(Database db)
        {
            this.Db = db;
        }

        /// <summary>
        /// 注册依赖项辅助方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected void RegistRepository<T>()
        {
            base.RegisterDependent<T>(RepositoryLocator.Container.Resolve<T>(this.Db));
        }
    }
}
