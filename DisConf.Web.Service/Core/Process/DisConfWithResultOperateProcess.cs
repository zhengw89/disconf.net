﻿namespace DisConf.Web.Service.Core.Process
{
    internal abstract class DisConfWithResultOperateProcess<T> : DisConfBaseCoreOperateProcess
    {
        protected DisConfWithResultOperateProcess(IDisConfProcessConfig config)
            : this(config, false)
        {
        }

        protected DisConfWithResultOperateProcess(IDisConfProcessConfig config, bool needUpdateZookeeper)
            : base(config, needUpdateZookeeper)
        {
        }

        /// <summary>
        /// 返回结果方法
        /// </summary>
        /// <returns></returns>
        protected abstract T GetResult();

        public T ExecuteProcess()
        {
            return base.ExecuteCoreProcess() ? GetResult() : default(T);
        }
    }
}
