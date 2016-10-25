using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.User.UserOperator
{
    internal class UserCreatorDependent : DisConfBaseDependentProvider
    {
        public UserCreatorDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IUserRepository>();
        }
    }

    internal class UserCreator : DisConfOperateProcess
    {
        private readonly string _userName, _password;

        private readonly IUserRepository _userRepository;

        public UserCreator(IDisConfProcessConfig config, string userName, string password)
            : base(config)
        {
            this._userName = userName;
            this._password = password;

            this._userRepository = base.ResolveDependency<IUserRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            if (string.IsNullOrEmpty(this._userName))
            {
                base.CacheProcessError("用户名为空");
                return false;
            }

            if (string.IsNullOrEmpty(this._password))
            {
                base.CacheProcessError("密码为空");
                return false;
            }

            if (this._userRepository.Exists(this._userName))
            {
                base.CacheProcessError("存在相同用户名");
                return false;
            }

            return true;
        }

        protected override bool ProcessMainData()
        {
            if (!this._userRepository.Create(new Web.Model.User()
            {
                Password = this._password,
                UserName = this._userName
            }))
            {
                base.CacheProcessError("创建用户失败");
                return false;
            }

            return true;
        }
    }
}
