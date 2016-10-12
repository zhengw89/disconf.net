using System;

namespace DisConf.Web.Service.Factory
{
    public interface IServiceContainer
    {
        void Register<TService>(Func<ServiceConfig, TService> factory);

        TService Resolve<TService>(ServiceConfig config);
    }
}
