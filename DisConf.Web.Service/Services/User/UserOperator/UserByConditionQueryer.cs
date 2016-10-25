using DisConf.Web.Model;
using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.User.UserOperator
{
    internal class UserByConditionQueryerDependent : DisConfBaseDependentProvider
    {
        public UserByConditionQueryerDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IUserRepository>();
        }
    }

    internal class UserByConditionQueryer : DisConfQueryProcess<PageList<Web.Model.User>>
    {
        private readonly int _pageIndex, _pageSize;

        private readonly IUserRepository _userRepository;

        public UserByConditionQueryer(IDisConfProcessConfig config, int pageIndex, int pageSize)
            : base(config)
        {
            this._pageIndex = pageIndex;
            this._pageSize = pageSize;

            this._userRepository = base.ResolveDependency<IUserRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            if (this._pageIndex <= 0 || this._pageSize <= 0)
            {
                base.CacheArgumentError("分页参数错误");
                return false;
            }

            return true;
        }

        protected override PageList<Web.Model.User> Query()
        {
            return this._userRepository.GetByCondition(this._pageIndex, this._pageSize);
        }
    }
}
