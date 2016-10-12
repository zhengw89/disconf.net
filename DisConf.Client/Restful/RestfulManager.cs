using DisConf.Client.Restful.Client;
using System;
using System.Threading;
using DisConf.Client.Restful.Retry;

namespace DisConf.Client.Restful
{
    internal class RestfulManager : IRestfulManager
    {
        private readonly IRetryStrategy _retryStrategy;

        public RestfulManager(IRetryStrategy retryStrategy)
        {
            this._retryStrategy = retryStrategy;
        }

        public T GetJsonData<T>(string url, int retryTimes, int retrySleepSeconds) where T : class
        {
            var unreliableRequest = new GetJsonClient(url);
            try
            {
                return this._retryStrategy.Retry<T>(unreliableRequest, retryTimes, retrySleepSeconds);
            }
            catch (Exception)
            {
                Thread.Sleep(1000);
            }

            throw new Exception(string.Format("cannot get : {0}", url));
        }
    }
}
