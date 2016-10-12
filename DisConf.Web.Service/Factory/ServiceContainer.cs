using System;
using System.Collections.Concurrent;

namespace DisConf.Web.Service.Factory
{
    internal class ServiceContainer : IServiceContainer
    {
        private readonly ConcurrentDictionary<Type, object> _factories;

        public ServiceContainer()
        {
            _factories = new ConcurrentDictionary<Type, object>();
        }
        
        public void Register<TService>(Func<ServiceConfig, TService> factory)
        {
            Type key = typeof(TService);
            _factories[key] = factory;
        }

        public TService Resolve<TService>(ServiceConfig config)
        {
            object factory;

            if (_factories.TryGetValue(typeof(TService), out factory))
                return ((Func<ServiceConfig, TService>)factory)(config);

            throw new ArgumentException("*******ServiceContainer unknow argument, not Register");
        }
    }
}
