using System;
using PetaPoco;

namespace DisConf.Web.Repository.Factory
{
    public interface IRepositoryPresenterContainer
    {
        void Register<TRepositoryPresenter>(Func<Database, TRepositoryPresenter> factory);

        /// <summary>
        /// 所使用的DB对象 若为null则为默认db对象
        /// </summary>
        /// <typeparam name="TRepositoryPresenter"></typeparam>
        /// <param name="db"></param>
        /// <returns></returns>
        TRepositoryPresenter Resolve<TRepositoryPresenter>(Database db);
    }
}
