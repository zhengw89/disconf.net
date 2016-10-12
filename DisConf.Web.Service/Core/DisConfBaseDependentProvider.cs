using CommonProcess.DependentProvider;
using DisConf.Web.Repository.Factory;
using PetaPoco;

namespace DisConf.Web.Service.Core
{
    internal abstract class DisConfBaseDependentProvider : BaseDependentProvider
    {
        protected readonly Database Db;

        protected DisConfBaseDependentProvider(Database db)
        {
            this.Db = db;
        }

        protected void RegistRepository<T>()
        {
            base.RegisterDependent<T>(RepositoryLocator.Container.Resolve<T>(this.Db));
        }

        //protected T ResolveRepository<T>()
        //{
        //    return RepositoryLocator.Container.Resolve<T>(this.Db);
        //}
    }
}
