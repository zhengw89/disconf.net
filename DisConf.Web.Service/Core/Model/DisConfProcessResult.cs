using DisConf.Web.Service.Model;

namespace DisConf.Web.Service.Core.Model
{
    /// <summary>
    /// 操作结果对象
    /// </summary>
    /// <typeparam name="T">结果类型</typeparam>
    internal class DisConfProcessResult<T>
    {
        public DisConfProcessError Error { get; set; }
        public T Data { get; set; }

        public DisConfProcessResult() { }
        public DisConfProcessResult(T data)
            : this(data, null)
        {
        }
        public DisConfProcessResult(DisConfProcessError error)
            : this(default(T), error)
        {
        }
        public DisConfProcessResult(T data, DisConfProcessError error)
        {
            this.Data = data;
            this.Error = error;
        }

        public BizResult<T> ConvertToBizResult()
        {
            var model = new BizResult<T>()
            {
                Data = this.Data
            };

            if (this.Error != null)
            {
                model.Error = this.Error.ConvertToBizError();
            }

            return model;
        }
    }
}
