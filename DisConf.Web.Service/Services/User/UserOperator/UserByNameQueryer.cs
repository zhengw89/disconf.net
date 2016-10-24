using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.User.UserOperator
{
    internal class UserByNameQueryerDependent : DisConfBaseDependentProvider
    {
        public UserByNameQueryerDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IUserRepository>();
        }
    }

    internal class UserByNameQueryer : DisConfQueryProcess<Web.Model.User>
    {
        private readonly string _userName;

        private readonly IUserRepository _userRepository;

        public UserByNameQueryer(IDisConfProcessConfig config, string userName)
            : base(config)
        {
            this._userName = userName;

            this._userRepository = base.ResolveDependency<IUserRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            if (string.IsNullOrEmpty(this._userName))
            {
                base.CacheProcessError("用户名为空");
                return false;
            }

            return true;
        }

        protected override Web.Model.User Query()
        {
            return this._userRepository.GetByUserName(this._userName);
        }
    }
}
