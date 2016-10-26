using DisConf.Web.Model;
using DisConf.Web.Service.Interfaces;
using DisConf.Web.Service.Model;
using DisConf.Web.Service.Services.User.UserOperator;

namespace DisConf.Web.Service.Services.User
{
    internal class UserService : BaseService, IUserService
    {
        public UserService(ServiceConfig config)
            : base(config)
        {
        }

        public BizResult<Web.Model.User> GetByUserName(string userName)
        {
            return base.ExeProcess(db =>
            {
                var queryer = new UserByNameQueryer(
                    base.ResloveProcessConfig<UserByNameQueryer>(db),
                    userName);

                return base.ExeQueryProcess(queryer);
            });
        }

        public BizResult<PageList<Web.Model.User>> GetByCondition(int pageIndex, int pageSize)
        {
            return base.ExeProcess(db =>
            {
                var queryer = new UserByConditionQueryer(
                    base.ResloveProcessConfig<UserByConditionQueryer>(db),
                    pageIndex, pageSize);

                return base.ExeQueryProcess(queryer);
            });
        }

        public BizResult<bool> CreateUser(string userName, string password)
        {
            return base.ExeProcess(db =>
            {
                var creator = new UserCreator(
                    base.ResloveProcessConfig<UserCreator>(db),
                    userName, password);

                return base.ExeOperateProcess(creator);
            });
        }

        public BizResult<bool> DeleteUser(int userId)
        {
            return base.ExeProcess(db =>
            {
                var deleter = new UserDeleter(
                    base.ResloveProcessConfig<UserDeleter>(db),
                    userId);

                return base.ExeOperateProcess(deleter);
            });
        }
    }
}
