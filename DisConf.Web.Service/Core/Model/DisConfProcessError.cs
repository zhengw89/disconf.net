using DisConf.Web.Service.Model;

namespace DisConf.Web.Service.Core.Model
{
    internal class DisConfProcessError
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public DisConfProcessError() { }
        public DisConfProcessError(int errorCode, string errorMessage)
        {
            this.Code = errorCode;
            this.Message = errorMessage;
        }

        public override string ToString()
        {
            return string.Format("Code:{0},Message:{1}", Code, Message);
        }

        public BizError ConvertToBizError()
        {
            return new BizError(this.Code, this.Message);
        }
    }
}
