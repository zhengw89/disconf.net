using System;
using System.Collections.Generic;
using DisConf.Utility.Path;
using DisConf.Web.Model;
using ZooKeeperNet;

namespace DisConf.Web.Service.Zk
{
    /// <summary>
    /// Zookeeper初始化辅助对象
    /// </summary>
    internal class ZkInitializer
    {
        public string App { get; set; }

        public string Env { get; set; }

        public List<Config> Configs { get; set; }

        public ZooKeeper ZooKeeper { get; set; }

        /// <summary>
        /// 初始化，根据配置信息初始化Zookeeper节点
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            var rootNodePath = ZooPathManager.GetRootPath(this.App, this.Env);
            InitializeNode(rootNodePath, DateTime.Now.Ticks.ToString());

            InitializeNode(ZooPathManager.GetAddPath(this.App, this.Env), string.Empty);
            InitializeNode(ZooPathManager.GetDelPath(this.App, this.Env), string.Empty);

            if (this.Configs != null)
            {
                foreach (var config in this.Configs)
                {
                    var configNodePath = ZooPathManager.JoinPath(rootNodePath, config.Name);
                    InitializeNode(configNodePath, config.Value);
                }
            }

            return true;
        }

        private void InitializeNode(string path, string data)
        {
            byte[] nodeData = data.GetBytes();

            if (this.ZooKeeper.Exists(path, true) == null)
            {
                this.ZooKeeper.Create(path, nodeData, Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            }
            else
            {
                this.ZooKeeper.SetData(path, nodeData, -1);
            }
        }
    }
}
