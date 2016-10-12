namespace DisConf.Client.Restful
{
    internal interface IRestfulManager
    {
        T GetJsonData<T>(string url, int retryTimes, int retrySleepSeconds) where T : class;
    }
}
