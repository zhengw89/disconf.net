using System;
using DisConf.Web.Model;
using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Interfaces;
using DisConf.Web.Service.Model;

namespace DisConf.Web.Service.Services
{
    internal class UserService : BaseService, IUserService
    {
        public UserService(ServiceConfig config)
            : base(config)
        {
        }

        public BizResult<User> GetByUserName(string userName)
        {
            //if (string.IsNullOrEmpty(userName))
            //{
            //    return new BizResult<User>()
            //    {
            //        Error = new BizError("用户名为空")
            //    };
            //}
            //using (var db = base.CreateDatabase())
            //{
            //    var userRepository = base.ResolveRepository<IUserRepository>(db);

            //    var user = userRepository.GetByUserName(userName);
            //    if (user == null)
            //    {
            //        return new BizResult<User>()
            //        {
            //            Error = new BizError("用户名不存在")
            //        };
            //    }

            //    return new BizResult<User>()
            //    {
            //        Data = user
            //    };
            //}
            throw new NotImplementedException();
        }
    }
}
