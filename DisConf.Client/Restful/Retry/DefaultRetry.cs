using System;
using System.Threading;
using DisConf.Client.Restful.Client;

namespace DisConf.Client.Restful.Retry
{
    internal class DefaultRetry : IRetryStrategy
    {
        public T Retry<T>(IUnreliableRequest unreliableRequest, int retryTimes, int sleepSeconds) where T : class
        {
            Exception ex = null;
            for (int i = 0; i <= retryTimes; i++)
            {
                try
                {
                    return unreliableRequest.Call<T>();
                }
                catch (Exception e)
                {
                    ex = e;
                    Thread.Sleep(sleepSeconds * 1000);
                }
            }

            if (ex != null)
            {
                throw ex;
            }
            else
            {
                throw new InvalidOperationException("Retry Times Error");
            }
        }
    }
}
