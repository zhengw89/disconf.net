using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DisConf.Web.Repository.Factory;
using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Zk;
using PetaPoco;
using ZooKeeperNet;

namespace DisConf.Web.Service
{
    public class ServiceInitializer
    {
        public static void Initialize(string connectionStringName, string zookeeperHost)
        {
            if (!string.IsNullOrEmpty(zookeeperHost))
            {
                using (var db = new Database(connectionStringName))
                {
                    var appRepository = RepositoryLocator.Container.Resolve<IAppRepository>(db);
                    var apps = appRepository.GetAll();
                    if (!apps.Any())
                    {
                        return;
                    }

                    var envRepository = RepositoryLocator.Container.Resolve<IEnvRepository>(db);
                    var envs = envRepository.GetAll();
                    if (!envs.Any())
                    {
                        return;
                    }

                    var configRepository = RepositoryLocator.Container.Resolve<IConfigRepository>(db);
                    var zkInitializer = new ZkInitializer();

                    using (var zk = new ZooKeeper(zookeeperHost, new TimeSpan(0, 0, 0, 50000), null))
                    {
                        foreach (var app in apps)
                        {
                            foreach (var env in envs)
                            {
                                var configs = configRepository.GetByAppAndEnv(app.Id, env.Id);

                                zkInitializer.App = app.Name;
                                zkInitializer.Env = env.Name;
                                zkInitializer.Configs = configs;
                                zkInitializer.ZooKeeper = zk;

                                zkInitializer.Initialize();
                            }
                        }
                    }
                }
            }
        }
    }
}
