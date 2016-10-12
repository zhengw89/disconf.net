using System;
using System.Collections.Concurrent;
using PetaPoco;

namespace DisConf.Web.Repository.Factory
{
    internal class RepositoryContainer : IRepositoryPresenterContainer
    {
        private readonly ConcurrentDictionary<Type, object> _factories;

        public RepositoryContainer()
        {
            _factories = new ConcurrentDictionary<Type, object>();
        }

        public void Register<TRepositoryPresenter>(Func<Database, TRepositoryPresenter> factory)
        {
            Type key = typeof(TRepositoryPresenter);
            _factories[key] = factory;
        }

        public TRepositoryPresenter Resolve<TRepositoryPresenter>(Database db)
        {
            object factory;

            if (_factories.TryGetValue(typeof(TRepositoryPresenter), out factory))
                return ((Func<Database, TRepositoryPresenter>)factory)(db);

            throw new ArgumentException("*******RepositoryPresenterContainer unknow argument, not Register");
        }
    }
}
