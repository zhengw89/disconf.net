using System.Collections.Generic;
using DisConf.Web.Service.Interfaces;
using DisConf.Web.Service.Model;
using DisConf.Web.Service.Services.Env.EnvOperator;

namespace DisConf.Web.Service.Services.Env
{
    internal class EnvService : BaseService, IEnvService
    {
        public EnvService(ServiceConfig config)
            : base(config)
        {
        }

        public BizResult<Web.Model.Env> GetEnvById(int id)
        {
            return base.ExeProcess(db =>
            {
                var queryer = new EnvByIdQueryer(
                    base.ResloveProcessConfig<EnvByIdQueryer>(db),
                    id);

                return base.ExeQueryProcess(queryer);
            });
        }

        public BizResult<Web.Model.Env> GetEnvByName(string envName)
        {
            return base.ExeProcess(db =>
            {
                var queryer = new EnvByNameQueryer(
                    base.ResloveProcessConfig<EnvByNameQueryer>(db),
                    envName);

                return base.ExeQueryProcess(queryer);
            });
        }

        public BizResult<List<Web.Model.Env>> GetAllEnv()
        {
            return base.ExeProcess(db =>
            {
                var queryer = new AllEnvQueryer(
                    base.ResloveProcessConfig<AllEnvQueryer>(db));

                return base.ExeQueryProcess(queryer);
            });
        }
    }
}
