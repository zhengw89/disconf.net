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

        public bool Exists(string userName)
        {
            var sql=new Sql().Select("COUNT(*)").From("T_USER").Where("USER_Name = @0", userName);
            return base.Db.ExecuteScalar<long>(sql) > 0;
        }

        public User GetByUserName(string userName)
        {
            var sql = new Sql().Where("USER_Name = @0", userName);

            var tUser = base.Db.SingleOrDefault<T_USER>(sql);
            return AutoMapper.Mapper.Map<User>(tUser);
        }
    }
}
