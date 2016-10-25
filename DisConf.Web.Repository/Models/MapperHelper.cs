using AutoMapper;
using DisConf.Web.Model;

namespace DisConf.Web.Repository.Models
{
    /// <summary>
    /// Model转换辅助
    /// </summary>
    internal class MapperHelper
    {
        private static bool _initialized = false;
        private static readonly object LockObj = new object();

        public static void Initialize()
        {
            //Mapper.CreateMap<T_APP, App>();
            //Mapper.CreateMap<App, T_APP>();
            if (!_initialized)
            {
                lock (LockObj)
                {
                    if (!_initialized)
                    {
                        Mapper.CreateMap<T_APP, App>();
                        Mapper.CreateMap<App, T_APP>();

                        Mapper.CreateMap<T_CONFIG, Config>();
                        Mapper.CreateMap<Config, T_CONFIG>();

                        Mapper.CreateMap<T_ENV, Env>();
                        Mapper.CreateMap<Env, T_ENV>();

                        Mapper.CreateMap<T_USER, User>();
                        Mapper.CreateMap<User, T_USER>();

                        _initialized = true;
                    }
                }
            }
        }
    }
}
