using System.Linq;
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
            var sql = new Sql().Select("COUNT(*)").From("T_USER").Where("USER_Name = @0", userName);
            return base.Db.ExecuteScalar<long>(sql) > 0;
        }

        public bool Create(User user)
        {
            return (int)base.Db.Insert(base.Map<User, T_USER>(user)) > 0;
        }

        public bool Delete(int id)
        {
            return base.Db.Delete<T_USER>(id) > 0;
        }

        public User GetByUserName(string userName)
        {
            var sql = new Sql().Where("USER_Name = @0", userName);

            var tUser = base.Db.SingleOrDefault<T_USER>(sql);
            return AutoMapper.Mapper.Map<User>(tUser);
        }

        public PageList<User> GetByCondition(int pageIndex, int pageSize)
        {
            var queryResult = base.Db.Page<T_USER>(pageIndex, pageSize, new Sql());
            if (queryResult == null) return null;

            var result = new PageList<User>();
            if (queryResult.Items != null)
            {
                result.Items = queryResult.Items.Select(a => base.Map<T_USER, User>(a)).ToList();
            }

            base.CopyPageInfoToPageList(queryResult, result);

            return result;
        }
    }
}
