using System.Collections.Generic;
using System.Linq;
using DisConf.Web.Model;
using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Repository.Models;
using PetaPoco;

namespace DisConf.Web.Repository.Repositories
{
    internal class EnvRepository : BaseRepository<Env>, IEnvRepository
    {
        public EnvRepository(Database db)
            : base(db)
        {
        }

        public bool Exists(int id)
        {
            return base.Db.Exists<T_ENV>(id);
        }

        public Env GetById(int id)
        {
            var queryResult = base.Db.SingleOrDefault<T_ENV>(id);
            return base.Map<T_ENV, Env>(queryResult);
        }

        public Env GetByName(string name)
        {
            var sql=new Sql();
            sql.Where("ENV_Name = @0", name);

            var queryResult = base.Db.SingleOrDefault<T_ENV>(sql);
            return base.Map<T_ENV, Env>(queryResult);
        }

        public List<Env> GetAll()
        {
            var queryResult = base.Db.Query<T_ENV>(new Sql());
            return queryResult.Select(a => base.Map<T_ENV, Env>(a)).ToList();
        }
    }
}
