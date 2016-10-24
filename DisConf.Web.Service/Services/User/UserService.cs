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
    }
}
