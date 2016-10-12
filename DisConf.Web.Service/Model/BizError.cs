namespace DisConf.Web.Service.Model
{
    public class BizError
    {
        public const int DefaultCode = -100;

        public int Code { get; set; }

        public string Message { get; set; }

        public BizError(string message)
            : this(DefaultCode, message)
        {

        }

        public BizError(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

    }
}
