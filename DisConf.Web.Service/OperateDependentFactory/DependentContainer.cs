using System;
using System.Collections.Concurrent;
using CommonProcess.DependentProvider;
using DisConf.Web.Service.Core;
using PetaPoco;

namespace DisConf.Web.Service.OperateDependentFactory
{
    internal class DependentContainer : IDependentContainer
    {
        private readonly ConcurrentDictionary<Type, object> _factories;

        public DependentContainer()
        {
            _factories = new ConcurrentDictionary<Type, object>();
        }

        public void Register<TDependentSource>(Func<BaseDependentProvider> factory)
        {
            Type key = typeof(TDependentSource);
            _factories[key] = factory;
        }

        public void Register<TDependentSource>(Func<Database, DisConfBaseDependentProvider> factory)
        {
            Type key = typeof(TDependentSource);
            _factories[key] = factory;
        }

        public DisConfBaseDependentProvider Resolve<TDependentSource>(Database db)
        {
            object factory;

            if (_factories.TryGetValue(typeof(TDependentSource), out factory))
                return ((Func<Database, DisConfBaseDependentProvider>)factory)(db);

            throw new ArgumentOutOfRangeException();
        }
    }
}
