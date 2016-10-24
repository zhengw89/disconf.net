using DisConf.Web.Service.Interfaces;
using DisConf.Web.Service.Services.App;
using DisConf.Web.Service.Services.Config;
using DisConf.Web.Service.Services.Env;
using DisConf.Web.Service.Services.User;

namespace DisConf.Web.Service.Factory
{
    public class ServiceLocator
    {
        private static readonly ServiceLocator Instance = new ServiceLocator();
        private readonly IServiceContainer _container;

        public static IServiceContainer Container
        {
            get { return Instance._container; }
        }

        private ServiceLocator()
        {
            _container = new ServiceContainer();
            RegisterDefaults(_container);
        }

        private void RegisterDefaults(IServiceContainer container)
        {
            container.Register<IUserService>(c => new UserService(c));
            container.Register<IAppService>(c => new AppService(c));
            container.Register<IEnvService>(c => new EnvService(c));
            container.Register<IConfigService>(c => new ConfigService(c));
        }
    }
}
