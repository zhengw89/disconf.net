namespace DisConf.Web.Service.Core.Process
{
    internal abstract class DisConfOperateProcess : DisConfBaseCoreOperateProcess
    {
        protected DisConfOperateProcess(IDisConfProcessConfig config)
            : this(config, false)
        {
        }

        protected DisConfOperateProcess(IDisConfProcessConfig config, bool needUpdateZookeeper)
            : base(config, needUpdateZookeeper)
        {
        }

        public bool ExecuteProcess()
        {
            return base.ExecuteCoreProcess();
        }
    }
}
