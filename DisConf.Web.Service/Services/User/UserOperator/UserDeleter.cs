using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.User.UserOperator
{
    internal class UserDeleterDependent : DisConfBaseDependentProvider
    {
        public UserDeleterDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IUserRepository>();
        }
    }

    internal class UserDeleter : DisConfOperateProcess
    {
        private readonly int _userId;

        private readonly IUserRepository _userRepository;

        public UserDeleter(IDisConfProcessConfig config, int userId)
            : base(config)
        {
            this._userId = userId;

            this._userRepository = base.ResolveDependency<IUserRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            return true;
        }

        protected override bool ProcessMainData()
        {
            if (!this._userRepository.Delete(this._userId))
            {
                base.CacheProcessError("用户删除失败");
                return false;
            }

            return true;
        }
    }
}
