using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Repository.Repositories;

namespace DisConf.Web.Repository.Factory
{
    public class RepositoryLocator
    {
        private static readonly RepositoryLocator Instance = new RepositoryLocator();
        private readonly IRepositoryPresenterContainer _container;

        public static IRepositoryPresenterContainer Container
        {
            get { return Instance._container; }
        }

        private RepositoryLocator()
        {
            _container = new RepositoryContainer();
            RegisterDefaults(_container);
        }

        private void RegisterDefaults(IRepositoryPresenterContainer container)
        {
            container.Register<IUserRepository>(db => new UserRepository(db));
            container.Register<IAppRepository>(db => new AppRepository(db));
            container.Register<IEnvRepository>(db => new EnvRepository(db));
            container.Register<IConfigRepository>(db => new ConfigRepository(db));
        }
    }
}
