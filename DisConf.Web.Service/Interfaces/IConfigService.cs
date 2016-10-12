﻿using System.Collections.Generic;
using DisConf.Web.Model;
using DisConf.Web.Service.Model;

namespace DisConf.Web.Service.Interfaces
{
    public interface IConfigService
    {
        BizResult<bool> Create(int appId, int envId, string name, string value);

        BizResult<bool> Update(int id, string value);

        BizResult<bool> Delete(int id);

        BizResult<Config> GetById(int id);

        BizResult<Config> GetByName(int appId, int envId, string name);

        BizResult<List<Config>> GetAll(int appId, int envId);
        
        BizResult<PageList<Config>> GetByCondition(int appId, int envId, int pageIndex, int pageSize);
    }
}