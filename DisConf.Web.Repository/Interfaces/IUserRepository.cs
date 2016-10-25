using DisConf.Web.Model;

namespace DisConf.Web.Repository.Interfaces
{
    public interface IUserRepository
    {
        bool Exists(string userName);

        bool Create(User user);

        User GetByUserName(string userName);

        PageList<User> GetByCondition(int pageIndex, int pageSize);
    }
}
