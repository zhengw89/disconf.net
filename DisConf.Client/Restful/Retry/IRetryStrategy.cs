using DisConf.Client.Restful.Client;

namespace DisConf.Client.Restful.Retry
{
    internal interface IRetryStrategy
    {
        T Retry<T>(IUnreliableRequest unreliableRequest, int retryTimes, int sleepSeconds) where T : class;
    }
}
