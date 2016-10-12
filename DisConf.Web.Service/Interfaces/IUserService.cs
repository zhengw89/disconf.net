﻿using DisConf.Web.Model;
using DisConf.Web.Service.Model;

namespace DisConf.Web.Service.Interfaces
{
    public interface IUserService
    {
        BizResult<User> GetByUserName(string userName);
    }
}
