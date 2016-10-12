using DisConf.Web.Model;
using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Repository.Models;
using PetaPoco;

namespace DisConf.Web.Repository.Repositories
{
    internal class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(Database db)
            : base(db)
        {
        }

        public User GetByUserName(string userName)
        {
            var sql = new Sql();
            sql.Where("USER_Name = @0", userName);

            var tUser = base.Db.SingleOrDefault<T_USER>(sql);

            if (tUser == null) return null;
            return AutoMapper.Mapper.Map<User>(tUser);
        }
    }
}
