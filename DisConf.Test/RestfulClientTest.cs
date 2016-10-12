using System;
using System.IO;
//using DisConf.Core.Restful.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DisConf.Test
{
    [TestClass]
    public class RestfulClientTest
    {
        //[TestMethod]
        //public void GetJsonClientTest()
        //{
        //    var client = new GetJsonClient("http://v.juhe.cn/weather/index");
        //    var re = client.Call<Res>();

        //    Assert.IsTrue(re.resultcode.Equals("101"));
        //}

        //[TestMethod]
        //public void GetFileClientTest()
        //{
        //    var url = "https://juhecdn.oss-cn-hangzhou.aliyuncs.com/jquery/jquery.min.js";
        //    var path = @"C:\Users\Zheng\Desktop\Temp\file.data";

        //    if (File.Exists(path))
        //    {
        //        File.Delete(path);
        //    }

        //    var client = new GetFileClient(url, path);
        //    var desPath = client.Call<string>();

        //    Assert.IsTrue(File.Exists(desPath));
        //}
    }

    public class Res
    {
        public string resultcode { get; set; }

        public string reason { get; set; }
    }
}
