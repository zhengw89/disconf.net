﻿using DisConf.Web.Model;

namespace DisConf.Web.Repository.Interfaces
{
    public interface IUserRepository
    {
        User GetByUserName(string userName);
    }
}
