using System;
using CommonProcess.DependentProvider;
using DisConf.Web.Service.Core;
using PetaPoco;

namespace DisConf.Web.Service.OperateDependentFactory
{
    internal interface IDependentContainer
    {
        void Register<TDependentSource>(Func<BaseDependentProvider> factory);

        void Register<TDependentSource>(Func<Database, DisConfBaseDependentProvider> factory);

        DisConfBaseDependentProvider Resolve<TDependentSource>(Database db);
    }
}
