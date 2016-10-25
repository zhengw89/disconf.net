using DisConf.Web.Service.Services.App.AppOperator;
using DisConf.Web.Service.Services.Config.ConfigLogOperator;
using DisConf.Web.Service.Services.Config.ConfigOperator;
using DisConf.Web.Service.Services.Env.EnvOperator;
using DisConf.Web.Service.Services.User.UserOperator;

namespace DisConf.Web.Service.OperateDependentFactory
{
    internal class OperateDependentLocator
    {
        private static readonly OperateDependentLocator Instance = new OperateDependentLocator();

        private readonly IDependentContainer _container;
        public static IDependentContainer Container { get { return Instance._container; } }

        private OperateDependentLocator()
        {
            _container = new DependentContainer();
            RegisterDefaults(_container);
        }

        private void RegisterDefaults(IDependentContainer container)
        {
            this.RegistApp(container);
            this.RegistConfig(container);
            this.RegistEnv(container);
            this.RegistUser(container);
        }

        private void RegistApp(IDependentContainer container)
        {
            container.Register<AppCreator>(db => new AppCreatorDependent(db));
            container.Register<AppUpdater>(db => new AppUpdaterDependent(db));
            container.Register<AppDeleter>(db => new AppDeleterDependent(db));
            container.Register<AppByConditionQueryer>(db => new AppByConditionQueryerDependent(db));
            container.Register<AppByIdQueryer>(db => new AppByIdQueryerDependent(db));
            container.Register<AppByNameQueryer>(db => new AppByNameQueryerDependent(db));
        }

        private void RegistConfig(IDependentContainer container)
        {
            container.Register<ConfigCreator>(db => new ConfigCreatorDependent(db));
            container.Register<ConfigValueUpdater>(db => new ConfigValueUpdaterDependent(db));
            container.Register<ConfigDeleter>(db => new ConfigDeleterDependent(db));
            container.Register<ConfigByIdQueryer>(db => new ConfigByIdQueryerDependent(db));
            container.Register<ConfigByNameQueryer>(db => new ConfigByNameQueryerDependent(db));
            container.Register<ConfigByAppAndEnvQueryer>(db => new ConfigByAppAndEnvQueryerDependent(db));
            container.Register<ConfigByConditionQueryer>(db => new ConfigByConditionQueryerDependent(db));

            container.Register<ConfigLogByConditionQueryer>(db => new ConfigLogByConditionQueryerDependent(db));
        }

        private void RegistEnv(IDependentContainer container)
        {
            container.Register<AllEnvQueryer>(db => new AllEnvQueryerDependent(db));
            container.Register<EnvByIdQueryer>(db => new EnvByIdQueryerDependent(db));
            container.Register<EnvByNameQueryer>(db => new EnvByNameQueryerDependent(db));
        }

        private void RegistUser(IDependentContainer container)
        {
            container.Register<UserByNameQueryer>(db => new UserByNameQueryerDependent(db));
            container.Register<UserByConditionQueryer>(db => new UserByConditionQueryerDependent(db));
            container.Register<UserCreator>(db => new UserCreatorDependent(db));
        }
    }
}
