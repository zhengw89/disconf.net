using DisConf.Web.Model;
using DisConf.Web.Service.Interfaces;
using DisConf.Web.Service.Model;
using DisConf.Web.Service.Services.App.AppOperator;

namespace DisConf.Web.Service.Services.App
{
    internal class AppService : BaseService, IAppService
    {
        public AppService(ServiceConfig config)
            : base(config)
        {
        }

        public BizResult<bool> CreateApp(string name, string description)
        {
            return base.ExeProcess(db =>
            {
                var creator = new AppCreator(
                    base.ResloveProcessConfig<AppCreator>(db),
                    name, description);

                return base.ExeOperateProcess(creator);
            });
        }

        public BizResult<bool> Update(int id, string name, string description)
        {
            return base.ExeProcess(db =>
            {
                var updater = new AppUpdater(
                    base.ResloveProcessConfig<AppUpdater>(db),
                    id, name, description);

                return base.ExeOperateProcess(updater);
            });
        }

        public BizResult<bool> Delete(int id)
        {
            return base.ExeProcess(db =>
            {
                var deleter = new AppDeleter(
                    base.ResloveProcessConfig<AppDeleter>(db),
                    id);

                return base.ExeOperateProcess(deleter);
            });
        }

        public BizResult<PageList<Web.Model.App>> GetByCondition(int pageIndex, int pageSize)
        {
            return base.ExeProcess(db =>
            {
                var queryer = new AppByConditionQueryer(
                    base.ResloveProcessConfig<AppByConditionQueryer>(db),
                    pageIndex, pageSize);

                return base.ExeQueryProcess(queryer);
            });
        }

        public BizResult<Web.Model.App> GetById(int appId)
        {
            return base.ExeProcess(db =>
            {
                var queryer = new AppByIdQueryer(
                    base.ResloveProcessConfig<AppByIdQueryer>(db),
                    appId);

                return base.ExeQueryProcess(queryer);
            });
        }

        public BizResult<Web.Model.App> GetByName(string name)
        {
            return base.ExeProcess(db =>
            {
                var queryer = new AppByNameQueryer(
                    base.ResloveProcessConfig<AppByNameQueryer>(db),
                    name);

                return base.ExeQueryProcess(queryer);
            });
        }
    }
}
