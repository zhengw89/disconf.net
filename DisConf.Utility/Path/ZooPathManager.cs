using System.Text;

namespace DisConf.Utility.Path
{
    /// <summary>
    /// Zookeeper路径辅助
    /// </summary>
    public class ZooPathManager
    {
        /// <summary>
        /// 获取Zookeeper根节点路径
        /// </summary>
        /// <param name="app">APP名称</param>
        /// <param name="env">ENV名称</param>
        /// <returns></returns>
        public static string GetRootPath(string app, string env)
        {
            return string.Format("{0}{1}_{2}", DisConfConstants.SepString, app, env);
        }

        /// <summary>
        /// 获取Zookeeper节点路径
        /// </summary>
        /// <param name="app">APP名称</param>
        /// <param name="env">ENV名称</param>
        /// <param name="key">配置名称</param>
        /// <returns></returns>
        public static string GetPath(string app, string env, string key)
        {
            var sb = new StringBuilder();

            sb.Append(GetRootPath(app, env));
            sb.Append(DisConfConstants.SepString);
            sb.Append(key);

            return sb.ToString();
        }

        /// <summary>
        /// 添加节点标识
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static string GetAddPath(string app, string env)
        {
            return JoinPath(GetRootPath(app, env),
                string.Format("{0}_{1}_{2}", app, env, DisConfConstants.Add));
        }

        /// <summary>
        /// 删除节点标识
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static string GetDelPath(string app, string env)
        {
            return JoinPath(GetRootPath(app, env),
                string.Format("{0}_{1}_{2}", app, env, DisConfConstants.Del));
        }

        /// <summary>
        /// 连接Zookeeper路径
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static string JoinPath(string path1, string path2)
        {
            return path1 + DisConfConstants.SepString + path2;
        }

        /// <summary>
        /// 根据Zookeeper路径获取信息
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <param name="app">APP名称</param>
        /// <param name="env">ENV名称</param>
        /// <param name="key">配置名称</param>
        /// <returns></returns>
        public static bool TryGetInfo(string path, out string app, out string env, out string key)
        {
            app = null;
            env = null;
            key = null;

            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            var info1 = path.Split(DisConfConstants.SepString.ToCharArray());
            if (info1.Length != 3)
            {
                return false;
            }

            var info2 = info1[1].Split('_');
            if (info2.Length != 2)
            {
                return false;
            }

            app = info2[0];
            env = info2[1];
            key = info1[2];

            return true;
        }
    }
}
