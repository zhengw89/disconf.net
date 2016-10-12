using CommonProcess;

namespace DisConf.Web.Service.Core.Process
{
    internal abstract class DisConfQueryProcess<T> : QueryProcess<T>
    {
        protected DisConfQueryProcess(IDisConfProcessConfig config)
            : base(config)
        {
        }

        protected void CacheProcessError(string message)
        {
            base.CacheError(-100, message);
        }
    }
}
