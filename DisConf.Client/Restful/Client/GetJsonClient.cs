using System.Net;
using Newtonsoft.Json;

namespace DisConf.Client.Restful.Client
{
    internal class GetJsonClient : IUnreliableRequest
    {
        private readonly string _url;

        public GetJsonClient(string url)
        {
            this._url = url;
        }

        public T Call<T>() where T : class
        {
            var wc = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var json = wc.DownloadString(this._url);

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
