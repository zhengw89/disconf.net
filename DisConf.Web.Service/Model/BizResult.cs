namespace DisConf.Web.Service.Model
{
    public class BizResult<T>
    {
        public T Data { get; set; }

        public BizError Error { get; set; }

        public bool HasError
        {
            get { return this.Error != null; }
        }
    }
}
