namespace DisConf.Client.Restful.Client
{
    internal interface IUnreliableRequest
    {
        T Call<T>() where T : class;
    }
}
