using System.Collections.Generic;
using DisConf.Client.Model;
using DisConf.Client.Restful;
using DisConf.Client.Restful.Retry;

namespace DisConf.Client.Manager
{
    /// <summary>
    /// disconf web 站点通信
    /// </summary>
    internal class ApiManager
    {
        /// <summary>
        /// 接口请求模板
        /// </summary>
        private const string GetAllConfigUrlTemplate = "{0}Api/GetAllConfig?appName={1}&envName={2}",
            GetConfigValueUrlTemplate = "{0}Api/GetConfigValue?appName={1}&envName={2}&configName={3}";

        private static readonly object LockObj = new object();

        private static ApiManager _manager;
        public static ApiManager Instance
        {
            get
            {
                if (_manager == null)
                {
                    lock (LockObj)
                    {
                        if (_manager == null)
                        {
                            _manager = new ApiManager();
                        }
                    }
                }
                return _manager;
            }
        }

        private string _host;
        private int _retryTimes, _retrySleepSeconds;

        private ApiManager()
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="host">disconf web host</param>
        /// <param name="retryTimes">重试次数</param>
        /// <param name="retrySleepSeconds">重试间隔时间</param>
        /// <returns></returns>
        public bool Initialize(string host, int retryTimes, int retrySleepSeconds)
        {
            this._host = host;
            this._retryTimes = retryTimes;
            this._retrySleepSeconds = retrySleepSeconds;

            return true;
        }

        /// <summary>
        /// 根据APP与ENV获取所有配置
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="envName"></param>
        /// <returns></returns>
        public ApiResponse<Dictionary<string, string>> GetByAppAndEnv(string appName, string envName)
        {
            var url = string.Format(GetAllConfigUrlTemplate, this._host, appName, envName);

            return new RestfulManager(new DefaultRetry()).GetJsonData<ApiResponse<Dictionary<string, string>>>(url, this._retryTimes, this._retrySleepSeconds);
        }

        /// <summary>
        /// 获取指定配置信息值
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="envName"></param>
        /// <param name="configName"></param>
        /// <returns></returns>
        public ApiResponse<Dictionary<string, string>> GetConfig(string appName, string envName, string configName)
        {
            var url = string.Format(GetConfigValueUrlTemplate, this._host, appName, envName, configName);

            return new RestfulManager(new DefaultRetry()).GetJsonData<ApiResponse<Dictionary<string, string>>>(url, this._retryTimes, this._retrySleepSeconds);
        }
    }
}
