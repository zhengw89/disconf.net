using DisConf.Web.Model;

namespace DisConf.Web.Repository.Interfaces
{
    public interface IUserRepository
    {
        bool Exists(string userName);

        User GetByUserName(string userName);
    }
}
